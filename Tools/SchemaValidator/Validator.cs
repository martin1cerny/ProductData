using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xbim.Common;
using Xbim.Common.Enumerations;
using Xbim.Common.Exceptions;
using Xbim.Common.ExpressValidation;
using Xbim.Ifc;
using Xbim.Ifc.Validation;
using Xbim.IO.Memory;
using Xbim.IO.Xml;

namespace SchemaValidator
{
    public class Validator
    {
        public IEnumerable<Error> Errors => appender.Errors;

        private ILog log;
        private ErrorAppender appender;

        private bool CheckInternal(IModel model, Dictionary<int, string> idMap)
        {
            // check for parser exceptions
            var v = new IfcValidator
            {
                ValidateLevel = ValidationFlags.All,
                CreateEntityHierarchy = true
            };
            var errors = v.Validate(model.Instances);
            foreach (var err in errors)
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
                log.Error(msg.ToString());
            }

            return !errors.Any();
        }

        public bool Check(IModel model, string logFile = null)
        {
            // set up error logger handler to get errors from parser
            appender = Logger.Setup(logFile);
            log = LogManager.GetLogger("Validator");

            return CheckInternal(model, new Dictionary<int, string>());
        }

        public bool Check(string file)
        {
            // set up error logger handler to get errors from parser
            var logFile = file + ".log";
            appender = Logger.Setup(logFile);
            log = LogManager.GetLogger("Validator");

            try
            {
                // open as an in-memory model (all syntactic errors fill be picked up)
                using (var model = IfcStore.Open(file, null, -1))
                {
                    // STEP21 syntactic errors will be reported in the log
                    if (appender.Errors.Any())
                        // do not proceed because the data is incomplete
                        return false;

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
                log.Error($"Parser failure: {pe.Message}.");
                return false;
            }
            catch (Exception ge)
            {
                log.Error($"General failure: {ge.Message}.", ge);
                return false;
            }

            return !Errors.Any();
        }

        private Dictionary<int, string> GetXmlEntityMap(Stream stream, IModel model)
        {
            var result = new Dictionary<int, string>(model.Instances.Count());
            var schema = model.Header.FileSchema.Schemas.First();
            if (string.Equals(schema, "IFC2X3", StringComparison.OrdinalIgnoreCase))
            {
                log.Warn("Only IFC4 models are supported to report XML ids");
                return result;
            }

            _dummyModel = new MemoryModel(_factory4);
            var xmlReader = new XbimXmlReader4(GetOrCreateXMLEntity, entity => { }, model.Metadata);
            xmlReader.Read(stream);

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
