using Xbim.Ifc4.ConstraintResource;
using Xbim.Ifc4.ControlExtension;
using Xbim.Ifc4.ExternalReferenceResource;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.SharedComponentElements;

namespace Examples.ComplexProperty
{
    class NPDExample : AbstractExample
    {
        public NPDExample() : base("Martin", "Cerny", "xBIM Ltd.") { }

        public override string Annotation => @"This example shows how property templates can be used to describe
declaration of performance of construction products. This utilises strong measure typing system and
units from IFC schema. This approach can be used to store CPR data.";

        public override void Run()
        {
            using (var txn = model.BeginTransaction("Example"))
            {
                ModelViews.Add("ProductDataView");

                var lib = New<IfcProjectLibrary>(l => l.Name = "Declaration of Performance");
                Comment(lib, @"Root element of this file. Because this doesn't define a specific instance in the building it is a library.
It can be used to declare elements, properties, property templates and other library objects 
which can be later used in the actual design.");
                var mm = New<IfcSIUnit>(u =>
                {
                    u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.METRE;
                    u.Prefix = Xbim.Ifc4.Interfaces.IfcSIPrefix.MILLI;
                    u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.LENGTHUNIT;
                    Comment(u, @"All units are machine readable in IFC. They are either SI or their definitions
are infered from SI units. Because of that, relations between all units are well defined,
reliable and it is possible to validate them.");
                });
                lib.UnitsInContext = New<IfcUnitAssignment>(ua =>
                {
                    ua.Units.Add(mm);
                    Comment(ua, @"Units are assigned to the library project to that when units are not explicitely
defined for properties thay can be found on the project level. There are exact rules to find units for
all measure types (for example 'LENGTHUNIT' for 'IfcLengthMeasue' etc.)");
                });

                var declarations = New<IfcRelDeclares>(rel =>
                {
                    rel.RelatingContext = lib;
                }).RelatedDefinitions;
                Comment(declarations.OwningEntity, @"This relation points to all definitions declared within the scope of the library.
These can be elements, element types, properties or property templates");

                var psetTemplate = New<IfcPropertySetTemplate>(ps =>
                {
                    ps.Name = "Essential Characteristics";
                    ps.ApplicableEntity = nameof(IfcBuildingElementPart);
                    ps.TemplateType = Xbim.Ifc4.Interfaces.IfcPropertySetTemplateTypeEnum.PSET_TYPEDRIVENOVERRIDE;
                });
                Comment(psetTemplate, @"Template of the property set is used to group properties in logical groups.
This can contain any number of properties with the name unique within the set. 'IfcBuildingElementPart' is the best IFC
entity type fit for a brick so it is used as the applicable entity definition");

                declarations.Add(psetTemplate);

                var lengthTemplate = New<IfcComplexPropertyTemplate>(t =>
                {
                    t.Name = "dimensions";
                    t.UsageName = "dimensions";
                    t.HasPropertyTemplates.Add(
                        New<IfcSimplePropertyTemplate>(v =>
                        {
                            v.Name = "length";
                            v.TemplateType = Xbim.Ifc4.Interfaces.IfcSimplePropertyTemplateTypeEnum.P_SINGLEVALUE;
                            Comment(v, "Simple name-definition-value property template defining actual value. " +
                                "Constraints are user to require either length value of certain value or NPD identifier for No Performence Declared");

                            New<IfcRelAssociatesConstraint>(r =>
                            {
                                r.RelatedObjects.Add(v);
                                r.RelatingConstraint = New<IfcObjective>(o => {
                                    o.Name = "Length constraints";
                                    o.LogicalAggregator = Xbim.Ifc4.Interfaces.IfcLogicalOperatorEnum.LOGICALXOR;
                                    o.BenchmarkValues.Add(New<IfcMetric>(m =>
                                    {
                                        m.Name = "Required length";
                                        m.ConstraintGrade = Xbim.Ifc4.Interfaces.IfcConstraintEnum.HARD;
                                        m.ConstraintSource = "EN 772-1";
                                        m.ValueSource = "EN 772-1";
                                        m.Benchmark = Xbim.Ifc4.Interfaces.IfcBenchmarkEnum.LESSTHANOREQUALTO;
                                        m.DataValue = New<IfcMeasureWithUnit>(mu => {
                                            mu.ValueComponent = new IfcLengthMeasure(300);
                                            mu.UnitComponent = mm;
                                            Comment(mu, "Measure with unit is used to set a required value type and unit");
                                        });
                                        Comment(m, "Metric constraint used to define required value of 300 mm");
                                    }));
                                    o.BenchmarkValues.Add(New<IfcMetric>(m =>
                                    {
                                        m.Name = "Allowed NPD value";
                                        m.ConstraintGrade = Xbim.Ifc4.Interfaces.IfcConstraintEnum.HARD;
                                        m.Benchmark = Xbim.Ifc4.Interfaces.IfcBenchmarkEnum.EQUALTO;
                                        m.ConstraintSource = "305/2011";
                                        m.ValueSource = "305/2011";
                                        m.DataValue = new IfcIdentifier("NPD");
                                        Comment(m, "Metric constraint used to define required value of 300 mm");
                                    }));
                                    Comment(o, "Objective is used to combine constrains so that either value with unit is evaluated or 'NPD' value is required");
                                });
                                
                                Comment(r, "Relation used to set a constraint on the template property");
                            });
                        })
                    );
                });
                psetTemplate.HasPropertyTemplates.Add(lengthTemplate);
                Comment(lengthTemplate, @"Complex property template is used to describe all aspects of the property. 
Both property set template and property template have globally unique GUID which can be used for 
global management of definitions and to maintain relations to data dictionaries. ");

                var docRel = New<IfcRelAssociatesDocument>(r =>
                {
                    r.RelatedObjects.Add(lengthTemplate);
                    r.RelatingDocument = New<IfcDocumentReference>(d =>
                    {
                        d.Name = "EN 772-1";
                        d.Location = new IfcURIReference("https://standards.cen.eu");
                        d.Identification = "3bhhlYovD1xOzSEP5n2HRY";
                        Comment(d, "Document reference pointing to relevant document. GUID is used for unique identification.");
                    });
                });
                Comment(docRel, @"Relation associates this property template with the document reference");


                Comment(lib.OwnerHistory, @"Owner history is used to define ownership of the information.");

                txn.Commit();
            }

            SaveAs("npd_example", true, typeof(IfcProjectLibrary));
        }
    }
}
