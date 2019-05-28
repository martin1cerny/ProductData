using System.Data;
using System.IO;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.PropertyResource;
using Xbim.Ifc4.SharedBldgElements;
using Xbim.Ifc4.SharedComponentElements;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using Xbim.Ifc4.ExternalReferenceResource;

namespace Examples.CatalogueExample
{
    class CatalogueExampleLightFixtures : AbstractExample
    {
        /// <summary>
        /// These credentials will be passed to the model constructor to set an author of the example data
        /// in form of IfcOwnerHistory entity
        /// </summary>
        public CatalogueExampleLightFixtures() : base("Robert", "Heinze", "CEN-TC442-WG2-TG3") { }
        private string baseFolder = @"..\..\..\..\..\SampleFiles\TriluxLightingProducts\SourceDataFromPimSystem";
        private string sourceFile = "TRILUX_Baselist_RH190520.xlsx";
        private string targetFile = "TriluxLightingProducts";

        public override string Annotation => "This is an IFC product library that contains configured lighting products from the manufacturer Trilux.";

        public override void Run()
        {
            using (var txn = model.BeginTransaction("Example creation"))
            {

                var project = model.Instances.New<IfcProject>(p => p.Name = "Basic Creation");

                //Insert a project library to store the product data templates and type products
                var ifcProductDataLibrary = New<IfcProjectLibrary>(l => {
                    l.Name = "TriluxLightingProducts";
                    l.GlobalId = "1DbshdzGD71ejurQqQcxbw";
                    l.Description = "Library for Trilux light fixtures product data templates based on the ZVEI European core properties";
                    l.Phase = "Design,Build,Operate";
                });
                Comment(ifcProductDataLibrary, @"Root element of this file. Because this doesn't define a specific instance in the building it is a library. It can be used to declare elements, properties, property templates and other library objects which can be later used in the actual design.");
                Comment(ifcProductDataLibrary.OwnerHistory, @"Owner history is used to define ownership of the information.");

                var ifcProductDataLibraryDeclarations = New<IfcRelDeclares>(rel =>
                {
                    rel.RelatingContext = ifcProductDataLibrary;
                }).RelatedDefinitions;
                Comment(ifcProductDataLibraryDeclarations.OwningEntity, @"This relation points to all definitions declared within the scope of the library. These can be elements, element types, properties or property templates");


                //Creating an IfcPropertySetTemplate manually
                //This is not optimal
                //Instead of creating the IfcPropertySetTemplates manually, 
                //they should be loaded from the publishing dictionary

                //Read templates from excel sheet
                var workbook = new XLWorkbook(Path.Combine(baseFolder, sourceFile));
                IXLWorksheet worksheetTemplates;
                IXLRange rangeTemplates;
                worksheetTemplates = workbook.Worksheet("Templates");
                rangeTemplates = worksheetTemplates.Range("A1:H27");
                IXLTable rawDataTemplates = rangeTemplates.AsTable();
                DataTable dtTemplates = ReadDataTable(worksheetTemplates);


                IfcPropertySetTemplate ifcPropertySetTemplate = model.Instances.New<IfcPropertySetTemplate>(pset =>
                {
                    pset.GlobalId = "1DbshTzGD71ejurQqQcxbw";
                    pset.Name = "IfcPropertySetTemplate";
                    pset.Description = "Group of properties for " + ifcProductDataLibrary.Name;
                    pset.ApplicableEntity = "IfcLightFixture/USERDEFINED";
                    pset.TemplateType = Xbim.Ifc4.Interfaces.IfcPropertySetTemplateTypeEnum.PSET_TYPEDRIVENONLY;
                });
                Comment(ifcPropertySetTemplate, @"Declaration of 'IfcPropertySetTemplate' within the library for lighting product data templates.");
                Comment(ifcPropertySetTemplate, @"Insert property templates; they should be loaded from the publishing dictionary");

                foreach (DataRow row in dtTemplates.Rows)
                {
                    //Publisher SystemName  GlobalId PrimaryMeasureType  DataColumn

                    ifcPropertySetTemplate.HasPropertyTemplates.AddRange(new[]
                    {
                        model.Instances.New<IfcSimplePropertyTemplate>(pt =>
                        {
                            pt.Name = row["SystemName"].ToString();
                            pt.Description = "";
                            pt.GlobalId = row["GlobalId"].ToString();
                            pt.TemplateType = Xbim.Ifc4.Interfaces.IfcSimplePropertyTemplateTypeEnum.P_SINGLEVALUE;
                            pt.AccessState = Xbim.Ifc4.Interfaces.IfcStateEnum.LOCKED;
                            pt.PrimaryMeasureType = row["PrimaryMeasureType"].ToString();

                            string primaryMeasureType = row["PrimaryMeasureType"].ToString();

                            if (primaryMeasureType == typeof(IfcLengthMeasure).Name)
                                    pt.PrimaryUnit = model.Instances.New<IfcSIUnit>(u=>
                                    {
                                        u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.LENGTHUNIT;
                                        u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.METRE;
                                        u.Prefix = Xbim.Ifc4.Interfaces.IfcSIPrefix.MILLI;
                                    });
                            else if (primaryMeasureType == typeof(IfcMassMeasure).Name)
                                    pt.PrimaryUnit = model.Instances.New<IfcSIUnit>(u=>
                                    {
                                        u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.MASSUNIT;
                                        u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.GRAM;
                                    });
                            else if (primaryMeasureType == typeof(IfcPlaneAngleMeasure).Name)
                                    pt.PrimaryUnit = model.Instances.New<IfcSIUnit>(u=>
                                    {
                                        u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.PLANEANGLEUNIT;
                                        u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.RADIAN;
                                    });
                                    //pt.PrimaryUnit = model.Instances.New<IfcConversionBasedUnit>(u=>
                                    //{
                                    //    //Convert the angel measure from the unit grad to the SI Unit radian
                                    //    //rad = Pi * grad / 180
                                    //    u.Name = "Grad";
                                    //    u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.PLANEANGLEUNIT;
                                    //    u.ConversionFactor = model.Instances.New<IfcMeasureWithUnit>(mwu1=>
                                    //    {
                                    //        mwu1.ValueComponent = new IfcReal(1000);
                                    //        mwu1.UnitComponent = model.Instances.New<IfcMeasureWithUnit>(mwu2=>
                                    //        {
                                    //            mwu2.UnitComponent Item = new IfcSIUnit()
                                    //            {
                                    //                Id = unitId.Replace("unit_", "siunit1_"),
                                    //                Name = IfcSIUnitName.Pascal,
                                    //                UnitType = etask.Ifc4.IfcUnitEnum.Pressureunit
                                    //            };
                                    //        });
                                    //    };
                                    //    u.Dimensions = model.Instances.New<IfcDimensionalExponents>
                                    //    {
                                    //        LengthExponent = -1,
                                    //        MassExponent = 1,
                                    //        TimeExponent = -2,
                                    //        ElectricCurrentExponent = 0,
                                    //        ThermodynamicTemperatureExponent = 0,
                                    //        AmountOfSubstanceExponent = 0,
                                    //        LuminousIntensityExponent = 0
                                    //    },
                                    //};
                        })
                    });
                };
           

                ifcProductDataLibraryDeclarations.Add(ifcPropertySetTemplate);
  
                //Read source data from excel sheet
                var workbookData = new XLWorkbook(Path.Combine(baseFolder, sourceFile));
                IXLWorksheet worksheetData;
                IXLRange rangeData;
                worksheetData = workbookData.Worksheet("BASELIST_1");
                rangeData = worksheetData.Range("A1:Z690");
                IXLTable rawData = rangeData.AsTable();
                DataTable dtData = ReadDataTable(worksheetData);

                var ifcRelDefinesByTemplate = New<IfcRelDefinesByTemplate>(dbt =>
                {
                    dbt.RelatingTemplate = ifcPropertySetTemplate;
                });

                int n = 0;
                do
                {
                    foreach (DataRow row in dtData.Rows)
                    {
                        var ifcTypeProduct = model.Instances.New<IfcTypeProduct>();
                        ifcTypeProduct.GlobalId = "1DbshTzGD71ejurQqQcxbw"; //How to generate a fresh IFC GlobalId with XBim?
                        ifcTypeProduct.Name = row["Name"].ToString();
                        ifcTypeProduct.Description = "Description of " + ifcTypeProduct.Name;
                        ifcTypeProduct.ApplicableOccurrence = "IfcLightFixture";

                        
                        IfcPropertySet ifcPropertySet = model.Instances.New<IfcPropertySet>(pset =>
                        {
                            pset.Name = "Properties of " + ifcTypeProduct.Name;
                        });

                        foreach (DataRow template in dtTemplates.Rows)
                        {
                            if (template["PropertyWithDocumentLink"].ToString() == "Yes")
                            {
                                //Insert the product information into documents
                                string folderName = template["SystemName"].ToString();
                                string docName = row[template["SystemName"].ToString()].ToString();

                                IfcDocumentInformation ifcDocumentInformation = model.Instances.New<IfcDocumentInformation>(doc =>
                                {
                                    doc.Identification = docName;
                                    doc.Name = docName;
                                    doc.Location = $@"{folderName}/{docName}";
                                    doc.Confidentiality = Xbim.Ifc4.Interfaces.IfcDocumentConfidentialityEnum.PUBLIC;
                                    doc.ElectronicFormat = MimeTypes.GetMimeType(docName);
                                    doc.IntendedUse = "Product information";
                                    doc.Purpose = "Product information";
                                });

                                IfcRelAssociatesDocument ifcRelAssociatesDocument = model.Instances.New<IfcRelAssociatesDocument>(docref =>
                                {
                                    docref.RelatedObjects.Add(ifcTypeProduct);
                                    docref.RelatingDocument = ifcDocumentInformation;
                                });

                                //<IfcRelAssociatesDocument GlobalId="3vBcwkKGf1cxmQZUtNnL0g">
                                //   < RelatedObjects >
                                //      < IfcTransportElement xsi: nil = "true" ref= "i143" />
                                //   </ RelatedObjects >
                                //   < RelatingDocument >
                                //      < IfcDocumentInformation xsi: nil = "true" ref= "i150" />
                                //   </ RelatingDocument >
                                //</ IfcRelAssociatesDocument >
                            }
                            else
                                //Insert the product information into Properties
                                ifcPropertySet.HasProperties.AddRange(new[]
                                {
                                    model.Instances.New<IfcPropertySingleValue>(p =>
                                    {
                                        string propertyName = template["SystemName"].ToString();
                                        var dataValue = row[propertyName];

                                        p.Name = propertyName;
                                        p.Description = "";

                                        string primaryMeasureType = template["PrimaryMeasureType"].ToString();
                                        if (primaryMeasureType==typeof(IfcLengthMeasure).Name)
                                           p.NominalValue = new IfcMassMeasure(Double.Parse(dataValue.ToString()));
                                        else if (primaryMeasureType==typeof(IfcMassMeasure).Name)
                                           p.NominalValue = new IfcMassMeasure(Double.Parse(dataValue.ToString()));
                                        else if (primaryMeasureType==typeof(IfcPlaneAngleMeasure).Name)
                                           p.NominalValue = new IfcPlaneAngleMeasure(Double.Parse(dataValue.ToString()));
                                        else
                                           p.NominalValue = new IfcLabel(dataValue.ToString());
                                    })
                                });
                        };

                        ifcTypeProduct.HasPropertySets.Add(ifcPropertySet);

                        ifcProductDataLibraryDeclarations.Add(ifcTypeProduct);
                        Comment(ifcTypeProduct, @"Declaration of 'IfcTypeProduct' within the library for a ligthing product.");

                        ifcRelDefinesByTemplate.RelatedPropertySets.Add(ifcPropertySet);
                    }
                    n++;
                }
                while (n < 1);

                txn.Commit();
            }

            string targetFileName = Path.Combine(baseFolder, targetFile);
            SaveAs(targetFileName, true, typeof(IfcProjectLibrary));
            
        }
    
        private DataTable ReadDataTable(IXLWorksheet ws)
        {
            DataTable dt = new DataTable();

            bool firstRow = true;
            foreach (IXLRow row in ws.Rows())
            {
                //Use the first row to add columns to DataTable.
                if (firstRow)
                {
                    foreach (IXLCell cell in row.Cells())
                    {
                        dt.Columns.Add(cell.Value.ToString());
                    }
                    firstRow = false;
                }
                else
                {
                    //Add rows to DataTable.
                    dt.Rows.Add();
                    int i = 0;

                    foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                        i++;
                    }
                }
            }
            return dt;
        }
    }
}
