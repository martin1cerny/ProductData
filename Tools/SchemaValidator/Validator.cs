using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xbim.Common;
using Xbim.Common.Enumerations;
using Xbim.Common.Exceptions;
using Xbim.Common.ExpressValidation;
using Xbim.Common.Metadata;
using Xbim.Ifc;
using Xbim.Ifc.Validation;
using Xbim.Ifc4.Interfaces;
using Xbim.IO.Memory;
using Xbim.IO.Xml;

namespace SchemaValidator
{
    public class Validator
    {
        public IEnumerable<LogMessage> Errors => appender.Errors;
        public IEnumerable<LogMessage> Warnings => appender.Warnings;

        private ILogger log;
        private MemoryLog appender;

        private bool CheckInternal(IModel model, Dictionary<int, string> idMap)
        {
            // check for parser exceptions
            var v = new IfcValidator
            {
                ValidateLevel = ValidationFlags.All,
                CreateEntityHierarchy = true
            };
            var schemaErrors = v.Validate(model.Instances);
            var templateErrors = CheckPropertyTemplateTypesAndUnits(model);
            var propErrors = CheckPropertyUnits(model);

            foreach (var err in
                schemaErrors
                .Concat(templateErrors)
                .Concat(propErrors))
            {
                var identity = err.Item.GetType().Name;
                if (err.Item is IPersistEntity entity)
                {
                    // use XML entity id if available
                    if (idMap.TryGetValue(entity.EntityLabel, out string xmlId))
                        identity = $"XML entity '{xmlId}' ({entity.ExpressType.ExpressName})";
                    else
                        identity = $"#{entity.EntityLabel}={entity.ExpressType.ExpressName}";
                }
                var msg = new StringBuilder();
                msg.AppendLine($"{identity} is invalid.");
                var details = new Stack<ValidationResult>(err.Details);
                while (details.Any())
                {
                    var detail = details.Pop();
                    foreach (var d in detail.Details)
                        details.Push(d);

                    var report = detail.Message;
                    if (string.IsNullOrWhiteSpace(report))
                        report = detail.Report();
                    msg.AppendLine("    " + report);

                    if (detail.IssueType == ValidationFlags.EntityWhereClauses || detail.IssueType == ValidationFlags.TypeWhereClauses)
                    {
                        var source = detail.IssueSource.Split('.')[0].ToLower();
                        msg.AppendLine($"http://www.buildingsmart-tech.org/ifc/IFC4/Add2/html/link/{source}.htm");
                    }
                }
                log.LogError(msg.ToString());
            }

            return !schemaErrors.Any();
        }

        private IEnumerable<ValidationResult> CheckPropertyUnits(IModel model)
        {
            var properties = model.Instances.OfType<IIfcSimpleProperty>();
            ValidationResult detail = null;
            foreach (var property in properties)
            {
                var err = new ValidationResult
                {
                    IssueSource = "Template Types and Units validation",
                    Item = property,
                    IssueType = ValidationFlags.Properties
                };

                if (property is IIfcPropertySingleValue single)
                {
                    if (single.NominalValue == null)
                        continue;
                    if ((detail = CheckUnit(single.NominalValue.GetType().Name, single.Unit, property)) != null)
                        err.AddDetail(detail);
                    continue;
                }

                if (property is IIfcPropertyListValue list)
                {
                    if (list.ListValues.FirstOrDefault() == null)
                        continue;
                    if ((detail = CheckUnit(list.ListValues.FirstOrDefault().GetType().Name, list.Unit, property)) != null)
                        err.AddDetail(detail);
                    continue;
                }

                if (property is IIfcPropertyBoundedValue bounded)
                {
                    if (bounded.UpperBoundValue != null)
                        if ((detail = CheckUnit(bounded.UpperBoundValue.GetType().Name, bounded.Unit, property)) != null)
                            err.AddDetail(detail);
                    if (bounded.SetPointValue != null)
                        if ((detail = CheckUnit(bounded.SetPointValue.GetType().Name, bounded.Unit, property)) != null)
                            err.AddDetail(detail);
                    if (bounded.LowerBoundValue != null)
                        if ((detail = CheckUnit(bounded.LowerBoundValue.GetType().Name, bounded.Unit, property)) != null)
                            err.AddDetail(detail);
                    continue;
                }

                if (property is IIfcPropertyTableValue table)
                {
                    if (table.DefinedValues.FirstOrDefault() != null)
                        if ((detail = CheckUnit(table.DefinedValues.FirstOrDefault().GetType().Name, table.DefinedUnit, property)) != null)
                            err.AddDetail(detail);
                    if (table.DefiningValues.FirstOrDefault() != null)
                        if ((detail = CheckUnit(table.DefiningValues.FirstOrDefault().GetType().Name, table.DefiningUnit, property)) != null)
                            err.AddDetail(detail);
                    continue;
                }

                if (err.Details != null && err.Details.Any())
                    yield return err;

            }
        }

        private IEnumerable<ValidationResult> CheckPropertyTemplateTypesAndUnits(IModel model)
        {
            var templates = model.Instances.OfType<IIfcSimplePropertyTemplate>();
            ValidationResult detail = null;
            foreach (var property in templates)
            {
                if (!property.TemplateType.HasValue)
                {
                    // if template type is not defined there is no point in validation
                    continue;
                }

                var err = new ValidationResult
                {
                    IssueSource = "Template Types and Units validation",
                    Item = property,
                    IssueType = ValidationFlags.Properties
                };

                switch (property.TemplateType.Value)
                {
                    case IfcSimplePropertyTemplateTypeEnum.P_SINGLEVALUE:
                    case IfcSimplePropertyTemplateTypeEnum.P_LISTVALUE:
                        if ((detail = CheckMeasureType(property.PrimaryMeasureType, model.Metadata, false)) != null)
                            err.AddDetail(detail);
                        if ((detail = CheckUnit(property.PrimaryMeasureType, property.PrimaryUnit, property)) != null)
                            err.AddDetail(detail);
                        break;

                    case IfcSimplePropertyTemplateTypeEnum.P_ENUMERATEDVALUE:
                        if (property.Enumerators == null)
                        {
                            detail = new ValidationResult
                            {
                                Message = "Enumerators must be defined",
                                IssueType = ValidationFlags.Properties
                            };
                            err.AddDetail(detail);
                        }
                        break;

                    case IfcSimplePropertyTemplateTypeEnum.P_TABLEVALUE:
                    case IfcSimplePropertyTemplateTypeEnum.P_BOUNDEDVALUE:
                        if ((detail = CheckMeasureType(property.PrimaryMeasureType, model.Metadata, false)) != null)
                            err.AddDetail(detail);
                        if ((detail = CheckMeasureType(property.SecondaryMeasureType, model.Metadata, false)) != null)
                            err.AddDetail(detail);

                        if ((detail = CheckUnit(property.PrimaryMeasureType, property.PrimaryUnit, property)) != null)
                            err.AddDetail(detail);
                        if ((detail = CheckUnit(property.SecondaryMeasureType, property.SecondaryUnit, property)) != null)
                            err.AddDetail(detail);
                        break;

                    case IfcSimplePropertyTemplateTypeEnum.P_REFERENCEVALUE:
                        if ((detail = CheckMeasureType(property.PrimaryMeasureType, model.Metadata, true)) != null)
                            err.AddDetail(detail);
                        break;

                    case IfcSimplePropertyTemplateTypeEnum.Q_LENGTH:
                    case IfcSimplePropertyTemplateTypeEnum.Q_AREA:
                    case IfcSimplePropertyTemplateTypeEnum.Q_VOLUME:
                    case IfcSimplePropertyTemplateTypeEnum.Q_COUNT:
                    case IfcSimplePropertyTemplateTypeEnum.Q_WEIGHT:
                    case IfcSimplePropertyTemplateTypeEnum.Q_TIME:
                    default:
                        break;
                }

                if (err.Details != null && err.Details.Any())
                    yield return err;
            }
        }

        private ValidationResult CheckUnit(string measureType, IIfcUnit unit, IPersistEntity entity)
        {
            if (string.IsNullOrWhiteSpace(measureType))
                return null;

            measureType = measureType.ToUpperInvariant();

            // no kind is defined for some measures (boolean, text, monetary, date etc.)
            if (!MeasureUnitMaps.UnitKinds.TryGetValue(measureType, out UnitKind kind))
                return null;

            switch (kind)
            {
                case UnitKind.IfcUnitEnum:
                    {
                        if (!MeasureUnitMaps.Units.TryGetValue(measureType, out IfcUnitEnum unitType))
                            throw new ArgumentOutOfRangeException();
                        if (unit != null)
                        {
                            if (!(unit is IIfcNamedUnit namedUnit) || namedUnit.UnitType != unitType)
                            {
                                return new ValidationResult
                                {
                                    IssueType = ValidationFlags.Properties,
                                    Item = entity,
                                    Message = $"Named unit of type {unitType.ToString()} must be defined for {measureType}",
                                    IssueSource = "Units to Measure Types validation"
                                };
                            }
                        }
                        else
                        {
                            // try to find unit assignment
                            unit = entity.Model.Instances
                                .OfType<IIfcUnitAssignment>()
                                .Select(a => a.Units.FirstOrDefault<IIfcNamedUnit>(u => u.UnitType == unitType))
                                .FirstOrDefault();
                            if (unit == null)
                            {
                                return new ValidationResult
                                {
                                    IssueType = ValidationFlags.Properties,
                                    Item = entity,
                                    Message = $"Named unit of type {unitType.ToString()} must be defined for {measureType} but it was not found directly or in global units assignment.",
                                    IssueSource = "Units to Measure Types validation"
                                };
                            }
                        }
                    }
                    break;
                case UnitKind.IfcDerivedUnitEnum:
                    {
                        if (!MeasureUnitMaps.DerivedUnits.TryGetValue(measureType, out IfcDerivedUnitEnum unitType))
                            throw new ArgumentOutOfRangeException();
                        if (unit != null)
                        {
                            if (!(unit is IIfcDerivedUnit derivedUnit) || derivedUnit.UnitType != unitType)
                            {
                                return new ValidationResult
                                {
                                    IssueType = ValidationFlags.Properties,
                                    Item = entity,
                                    Message = $"Derived unit of type {unitType.ToString()} must be defined for {measureType}",
                                    IssueSource = "Units to Measure Types validation"
                                };
                            }
                        }
                        else
                        {
                            // try to find unit assignment
                            unit = entity.Model.Instances
                                .OfType<IIfcUnitAssignment>()
                                .Select(a => a.Units.FirstOrDefault<IIfcDerivedUnit>(u => u.UnitType == unitType))
                                .FirstOrDefault();
                            if (unit == null)
                            {
                                return new ValidationResult
                                {
                                    IssueType = ValidationFlags.Properties,
                                    Item = entity,
                                    Message = $"Derived unit of type {unitType.ToString()} must be defined for {measureType} but it was not found directly or in global units assignment.",
                                    IssueSource = "Units to Measure Types validation"
                                };
                            }
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }

        private ValidationResult CheckMeasureType(string typeName, ExpressMetaData metadata, bool reference)
        {
            // null or empty is a valid value
            if (string.IsNullOrWhiteSpace(typeName))
                return null;

            typeName = typeName.ToUpperInvariant();
            var err = new ValidationResult
            {
                IssueSource = "Measure type validation",
                IssueType = ValidationFlags.Properties,

            };
            if (!metadata.TryGetExpressType(typeName, out ExpressType eType))
            {
                err.Message = $"Type {typeName} is not an IFC type.";
                return err;
            }

            var type = eType.Type;
            if (!reference && !typeof(IIfcValue).IsAssignableFrom(type))
            {
                err.Message = $"Type {typeName} is not applicable. This must be assignable to 'IfcValue'";
                return err;
            }

            if (reference && !typeof(IIfcObjectReferenceSelect).IsAssignableFrom(type))
            {
                err.Message = $"Type {typeName} is not applicable. This must be assignable to 'IfcObjectReferenceSelect'";
                return err;
            }

            return null;
        }

        public bool Check(IModel model, string logFile = null)
        {
            // set up error logger handler to get errors from parser
            appender = Logger.Setup(logFile);
            log = XbimLogging.CreateLogger("Validator");

            return CheckInternal(model, new Dictionary<int, string>());
        }

        public bool Check(string file)
        {
            // set up error logger handler to get errors from parser
            var logFile = file + ".log";
            appender = Logger.Setup(logFile);
            log = XbimLogging.CreateLogger("Validator");


            var ext = Path.GetExtension(file).ToUpperInvariant().Trim('.');
            var format = "UNKNOWN";
            switch (ext)
            {
                case "IFC":
                    format = "STEP21 (*.ifc)";
                    break;
                case "IFCXML":
                    format = "XML (*.ifcXML)";
                    break;
                default:
                    break;
            }
            log.LogInformation($"Validating file: {file}");
            log.LogInformation($"File format: {format}");


            try
            {
                // open as an in-memory model (all syntactic errors fill be picked up)
                using (var model = IfcStore.Open(file, null, -1))
                {
                    // header information
                    log.LogInformation($"Schema version: {string.Join(", ", model.Header.SchemaVersion)}");
                    log.LogInformation($"Model View Definitions: {string.Join(", ", model.Header.FileDescription.Description)}");

                    // STEP21 syntactic errors will be reported in the log already
                    if (appender.Errors.Any())
                        // do not proceed because the data is incomplete
                        return false;

                    log.LogInformation($"Number of entities: {model.Instances.Count}");
                    LogEntityHistogram(model);

                    var idMap = new Dictionary<int, string>();
                    if (file.ToLower().EndsWith(".ifcxml"))
                    {
                        using (var stream = File.OpenRead(file))
                        {
                            idMap = GetXmlEntityMap(stream, model);
                        }
                    }

                    CheckInternal(model, idMap);
                }
            }
            // XML syntactic errors will be fired as an exception
            catch (XbimParserException pe)
            {
                log.LogError($"Parser failure: {pe.Message}.");
                return false;
            }
            catch (Exception ge)
            {
                log.LogError($"General failure: {ge.Message}.", ge);
                return false;
            }

            return !Errors.Any() && !Warnings.Any();
        }

        private void LogEntityHistogram(IModel model)
        {
            var histogram = new Dictionary<string, int>();
            foreach (var item in model.Instances)
            {
                var type = item.ExpressType.ExpressName;
                if (histogram.TryGetValue(type, out int value))
                    histogram[type]++;
                else
                    histogram.Add(type, 1);
            }

            var msg = new StringBuilder();
            msg.AppendLine("Types frequency");
            foreach (var kvp in histogram.OrderByDescending(kvp => kvp.Value))
            {
                msg.AppendLine($"{kvp.Key, 40}: {kvp.Value}");
            }
            log.LogInformation($"Number of types: {histogram.Count}");
            log.LogInformation(msg.ToString());
        }

        private Dictionary<int, string> GetXmlEntityMap(Stream stream, IModel model)
        {
            var result = new Dictionary<int, string>(model.Instances.Count());
            var schema = model.Header.FileSchema.Schemas.First();
            if (string.Equals(schema, "IFC2X3", StringComparison.OrdinalIgnoreCase))
            {
                log.LogWarning("Only IFC4 models are supported to report XML ids");
                return result;
            }

            _dummyModel = new MemoryModel(_factory4);
            // pass in null logger as this is not critical
            var xmlReader = new XbimXmlReader4(GetOrCreateXMLEntity, entity => { }, model.Metadata, NullLogger.Instance);
            xmlReader.Read(stream, _dummyModel);

            // swap the dictionary
            foreach (var item in xmlReader.IdMap)
                result.Add(item.Value, item.Key);

            //purge
            _read.Clear();
            _dummyModel = null;
            return result;
        }

        private readonly Dictionary<int, IPersistEntity> _read = new Dictionary<int, IPersistEntity>();
        private readonly IEntityFactory _factory4 = new Xbim.Ifc4.EntityFactoryIfc4();
        private IModel _dummyModel;
        private IPersistEntity GetOrCreateXMLEntity(int label, Type type)
        {
            if (_read.TryGetValue(label, out IPersistEntity exist))
                return exist;

            var ent = _factory4.New(_dummyModel, type, label, true);
            _read.Add(label, ent);
            return ent;
        }
    }
}
