using Xbim.Ifc4.ExternalReferenceResource;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.ProductExtension;
using Xbim.Ifc4.PropertyResource;
using Xbim.Ifc4.SharedComponentElements;

namespace Examples.ComplexProperty
{
    class ComplexPropertyExample : AbstractExample
    {
        public ComplexPropertyExample() : base("Martin", "Cerny", "xBIM Ltd.") { }

        public override string Annotation => @"This example shows how complex properties can be used to describe
declaration of performance of construction products. This utilises strong measure typing system and
units from IFC schema. This approach can be used to store CPR data.";

        public override void Run()
        {
            using (var txn = model.BeginTransaction("Example"))
            {
                ModelViews.Add("ProductDataView");

                var lib = New<IfcProjectLibrary>(l => l.Name = "Declaration of Performance");
                var mm = New<IfcSIUnit>(u =>
                {
                    u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.METRE;
                    u.Prefix = Xbim.Ifc4.Interfaces.IfcSIPrefix.MILLI;
                    u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.LENGTHUNIT;
                });
                lib.UnitsInContext = New<IfcUnitAssignment>(ua =>
                {
                    ua.Units.Add(mm);
                });

                var declarations = New<IfcRelDeclares>(rel =>
                {
                    rel.RelatingContext = lib;
                }).RelatedDefinitions;

                var psetTemplate = New<IfcPropertySetTemplate>(ps =>
                {
                    ps.Name = "dimensions";
                    ps.ApplicableEntity = nameof(IfcElement);
                });
                Comment(psetTemplate, "Template of the complex property to define all it's aspects.\r\nThis can contain any number of descriptive properties.");

                declarations.Add(psetTemplate);

                var lengthTemplate = New<IfcComplexPropertyTemplate>(c =>
                {
                    c.Name = "length";
                    c.UsageName = "length";
                    c.HasPropertyTemplates.AddRange(new[] {
                            New<IfcSimplePropertyTemplate>(v => {
                                v.Name = "Value";
                                v.TemplateType = Xbim.Ifc4.Interfaces.IfcSimplePropertyTemplateTypeEnum.P_SINGLEVALUE;
                                v.PrimaryUnit = mm;
                                v.PrimaryMeasureType = nameof(IfcLengthMeasure);
                            }),
                            New<IfcSimplePropertyTemplate>(v => {
                                v.Name = "ReferenceDocument";
                                v.TemplateType = Xbim.Ifc4.Interfaces.IfcSimplePropertyTemplateTypeEnum.P_REFERENCEVALUE;
                                v.PrimaryMeasureType = nameof(IfcDocumentReference);
                            })
                        });
                });
                psetTemplate.HasPropertyTemplates.Add(lengthTemplate);


                var brick = New<IfcBuildingElementPart>(b =>
                {
                    b.Name = "Porotherm 50 EKO+ Profi R";
                    b.PredefinedType = Xbim.Ifc4.Interfaces.IfcBuildingElementPartTypeEnum.USERDEFINED;
                    b.ObjectType = "BRICK";
                });
                declarations.Add(brick);

                var pset = New<IfcPropertySet>(ps =>
                {
                    ps.Name = psetTemplate.Name;
                    ps.HasProperties.Add(New<IfcComplexProperty>(c =>
                    {
                        c.Name = lengthTemplate.Name?.ToString();
                        c.UsageName = c.Name;
                        c.HasProperties.AddRange(new IfcProperty[] {
                            New<IfcPropertySingleValue>(v => {
                                v.Name = "Value";
                                v.Unit = mm;
                                v.NominalValue = new IfcLengthMeasure(300);
                            }),
                            New<IfcPropertyReferenceValue>(v => {
                                v.Name = "ReferenceDocument";
                                v.PropertyReference = New<IfcDocumentReference>(d => {
                                    d.Name = "EN 772-1";
                                });
                            })
                        });
                    }));
                });
                New<IfcRelDefinesByTemplate>(r =>
                {
                    r.RelatedPropertySets.Add(pset);
                    r.RelatingTemplate = psetTemplate;
                });
                New<IfcRelDefinesByProperties>(r =>
                {
                    r.RelatingPropertyDefinition = pset;
                    r.RelatedObjects.Add(brick);
                });

                txn.Commit();
            }

            SaveAs("complex_properties", true, typeof(IfcProjectLibrary));
        }
    }
}
