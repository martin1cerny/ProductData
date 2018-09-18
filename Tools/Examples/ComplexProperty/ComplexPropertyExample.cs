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
                    ps.Name = "dimensions";
                    ps.ApplicableEntity = nameof(IfcElement);
                });
                Comment(psetTemplate, @"Template of the property set is used to group properties in logical groups.
This can contain any number of properties with name unique within the set.");

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
                                Comment(v, "Simple name-definition-value property template defining actual value, type of measure (length) and unit (mm)");
                            }),
                            New<IfcSimplePropertyTemplate>(v => {
                                v.Name = "ReferenceDocument";
                                v.TemplateType = Xbim.Ifc4.Interfaces.IfcSimplePropertyTemplateTypeEnum.P_REFERENCEVALUE;
                                v.PrimaryMeasureType = nameof(IfcDocumentReference);
                                Comment(v, "Reference property template pointing to document relevant to the 'length' property");
                            })
                        });
                });
                psetTemplate.HasPropertyTemplates.Add(lengthTemplate);
                Comment(lengthTemplate, @"Template of the complex property to define all it's aspects. This can contain any number of descriptive properties.
Both property set template and property template contain globally unique GUID which 
can be used for global management of definitions and to maintain relations to data dictionaries.");


                var brick = New<IfcBuildingElementPart>(b =>
                {
                    b.Name = "Porotherm 50 EKO+ Profi R";
                    b.PredefinedType = Xbim.Ifc4.Interfaces.IfcBuildingElementPartTypeEnum.USERDEFINED;
                    b.ObjectType = "BRICK";
                });
                declarations.Add(brick);
                Comment(brick, @"The best IfcElement subtype for brick is 'IfcBuildingElementPart'. 
This type should not be considered as significant because authoring tools will often not support all 
IFC element types and might not provide sufficient configurations or modelling workflows to create
such a data. Combination of 'PredefinedType = USERDEFINED' and 'ObjectType = ""BRICK""' can be
used to identify the element. 'IfcBuildingElementPart' should also be used for materials.");

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
                    Comment(ps, @"This property set contains actual values and points to the defining template.");
                });
                New<IfcRelDefinesByTemplate>(r =>
                {
                    r.RelatedPropertySets.Add(pset);
                    r.RelatingTemplate = psetTemplate;
                    Comment(r, "This relation binds actual property set with values to it's template.");
                });
                New<IfcRelDefinesByProperties>(r =>
                {
                    r.RelatingPropertyDefinition = pset;
                    r.RelatedObjects.Add(brick);
                    Comment(r, "This relation sets the set of properties to the library element");
                });

                Comment(lib.OwnerHistory, @"Owner history is used to define ownership of the information.");

                txn.Commit();
            }

            SaveAs("Example_7_Dimensions", true, typeof(IfcProjectLibrary));
        }
    }
}
