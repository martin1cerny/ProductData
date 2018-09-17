using Xbim.Ifc4.ExternalReferenceResource;
using Xbim.Ifc4.HvacDomain;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.PropertyResource;

namespace Examples.DynamicValues
{
    class DynamicValuesExample : AbstractExample
    {
        public DynamicValuesExample() : 
            base("Martin", "Cerny", "xBIM") { }

        public override string Annotation => @"This example shows various approaches
to store dynamic data in form of table, function or documentation reference.";

        public override void Run()
        {
            using (var txn = model.BeginTransaction("Example"))
            {
                var lib = New<IfcProjectLibrary>(l => l.Name = "Air Terminal Library");
                var hz = New<IfcSIUnit>(u =>
                {
                    u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.HERTZ;
                    u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.FREQUENCYUNIT;
                });
                var watt = New<IfcSIUnit>(u =>
                {
                    u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.WATT;
                    u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.POWERUNIT;
                });
                var db = New<IfcDerivedUnit>(u =>
                {
                    u.UnitType = Xbim.Ifc4.Interfaces.IfcDerivedUnitEnum.SOUNDPRESSURELEVELUNIT;
                    u.Elements.Add(New<IfcDerivedUnitElement>(due =>
                    {
                        due.Exponent = 1;
                        due.Unit = watt;
                    }));
                    u.Elements.Add(New<IfcDerivedUnitElement>(due =>
                    {
                        due.Exponent = -1;
                        due.Unit = watt;
                    }));
                });
                lib.UnitsInContext = New<IfcUnitAssignment>(ua =>
                {
                    ua.Units.Add(hz);
                    ua.Units.Add(db);
                });

                var type = New<IfcAirTerminalType>(t => t.Name = "Air Terminal");
                New<IfcRelDeclares>(rel =>
                {
                    rel.RelatingContext = lib;
                    rel.RelatedDefinitions.Add(type);
                });
                var pset = New<IfcPropertySet>(ps =>
                {
                    ps.Name = "Air Terminal Properties";
                });
                type.HasPropertySets.Add(pset);

                var prop = New<IfcPropertyTableValue>(t =>
                {
                    t.Name = "Acustic Performance [table of values]";
                    t.DefiningUnit = hz;
                    t.DefinedUnit = db;
                    t.DefiningValues.AddRange(new IfcValue[] {
                            new IfcFrequencyMeasure(63),
                            new IfcFrequencyMeasure(125),
                            new IfcFrequencyMeasure(250),
                            new IfcFrequencyMeasure(500),
                            new IfcFrequencyMeasure(1000),
                            new IfcFrequencyMeasure(2000),
                            new IfcFrequencyMeasure(4000),
                            new IfcFrequencyMeasure(8000)

                    });
                    t.DefinedValues.AddRange(new IfcValue[] {
                            new IfcSoundPressureLevelMeasure(102),
                            new IfcSoundPressureLevelMeasure(99),
                            new IfcSoundPressureLevelMeasure(98),
                            new IfcSoundPressureLevelMeasure(98),
                            new IfcSoundPressureLevelMeasure(97),
                            new IfcSoundPressureLevelMeasure(95),
                            new IfcSoundPressureLevelMeasure(86),
                            new IfcSoundPressureLevelMeasure(81)
                    });
                });
                pset.HasProperties.Add(prop);

                var docProp = New<IfcPropertyReferenceValue>(r =>
                {
                    r.Name = "Acustic Performance [document reference]";
                    r.PropertyReference = New<IfcDocumentReference>(doc =>
                    {
                        doc.Name = "Acustic Performance Documentation";
                        doc.Location = "https://www.daikinac.com/content/assets/DOC/EngineeringManuals/EDUS041501.pdf";
                    });
                });
                pset.HasProperties.Add(docProp);

                var fce = New<IfcPropertySingleValue>(r =>
                {
                    r.Name = "Acustic Performance [function]";
                    r.NominalValue = new IfcText("0.0492424242 * Math.pow(x,4) - 1.0328282828 * Math.pow(x,3) + 6.8068181818 * Math.pow(x,2) - 17.753968254 * x + 114.14285714");
                    r.Description = "ISO/IEC 22275:2018";
                });
                pset.HasProperties.Add(fce);

                txn.Commit();
            }

            // this will save .ifc, .ifcxml and .log with all potential schema errors
            SaveAs("properties", true, typeof(IfcProjectLibrary));
        }
    }
}
