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
using System.IO.Compression;

namespace Examples.CatalogueExample
{
    class CatalogueExampleLightFixtures : AbstractExample
    {
        /// <summary>
        /// These credentials will be passed to the model constructor to set an author of the example data
        /// in form of IfcOwnerHistory entity
        /// </summary>
        public CatalogueExampleLightFixtures() : base("Robert", "Heinze", "CEN-TC442-WG2-TG3") { }
        private string sourceFolder = @"..\..\..\..\..\SampleFiles\TriluxLightingProducts\SourceDataFromPimSystem";
        private string sourceFile = "TRILUX_Baselist_RH190520.xlsx";
        private string targetFolder = @"..\..\..\..\..\SampleFiles\TriluxLightingProducts\OpenProductLibrary";
        private string targetFile = "TriluxLightingProducts";
        private string zipFile = @"..\..\..\..\..\SampleFiles\TriluxLightingProducts\TriluxLightingProducts.ifczip";

        public override string Annotation => "This is an IFC product library that contains configured lighting products from the manufacturer Trilux.";

        public override void Run()
        {
            using (var txn = model.BeginTransaction("Example creation"))
            {
                IfcProject project = model.Instances.New<IfcProject>(p => p.Name = "TriluxLightingProducts");
                IfcUnitAssignment ifcUnitAssignment = model.Instances.New<IfcUnitAssignment>(ua =>
                {
                    ua.Units.Add(model.Instances.New<IfcSIUnit>(u =>
                    {
                        u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.LENGTHUNIT;
                        u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.METRE;
                        u.Prefix = Xbim.Ifc4.Interfaces.IfcSIPrefix.MILLI;
                    }));
                    ua.Units.Add(model.Instances.New<IfcSIUnit>(u =>
                    {
                        u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.MASSUNIT;
                        u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.GRAM;
                        u.Prefix = Xbim.Ifc4.Interfaces.IfcSIPrefix.KILO;
                    }));
                });

                var ifcClassificationSystemOmniClass = model.Instances.New<IfcClassification>();
                ifcClassificationSystemOmniClass.Name = "Omniclass";
                ifcClassificationSystemOmniClass.Edition = "1.0";
                ifcClassificationSystemOmniClass.EditionDate = "2018-12-27T00:00:00.0000000";
                ifcClassificationSystemOmniClass.Description = "The OmniClass Construction Classification System (known as OmniClass™ or OCCS) is a classification system for the construction industry. OmniClass is useful for many applications, from organizing library materials, product literature, and project information, to providing a classification structure for electronic databases. It incorporates other extant systems currently in use as the basis of many of its Tables – MasterFormat™ for work results, UniFormat for elements, and EPIC (Electronic Product Information Cooperation) for structuring products.";
                ifcClassificationSystemOmniClass.Location = "http://www.omniclass.org/";

                var ifcClassificationReferenceOmniClass = model.Instances.New<IfcClassificationReference>();
                ifcClassificationReferenceOmniClass.Identification = "23-35-47";
                ifcClassificationReferenceOmniClass.Name = "Electrical Lighting";
                ifcClassificationReferenceOmniClass.Description = "NOT PROVIDED";
                ifcClassificationReferenceOmniClass.ReferencedSource = ifcClassificationSystemOmniClass;

                var ifcRelAssociatesClassificationOmniClass = model.Instances.New<IfcRelAssociatesClassification>();
                ifcRelAssociatesClassificationOmniClass.RelatingClassification = ifcClassificationReferenceOmniClass;

                var ifcClassificationSystemUniClass = model.Instances.New<IfcClassification>();
                ifcClassificationSystemUniClass.Name = "Uniclass";
                ifcClassificationSystemUniClass.Edition = "2015";
                ifcClassificationSystemUniClass.EditionDate = "";
                ifcClassificationSystemUniClass.Description = "";
                ifcClassificationSystemUniClass.Location = "https://www.thenbs.com/our-tools/introducing-uniclass-2015";

                var ifcClassificationReferenceUniClass = model.Instances.New<IfcClassificationReference>();
                ifcClassificationReferenceUniClass.Identification = "CA-70-10-30";
                ifcClassificationReferenceUniClass.Name = "Site lighting equipment";
                ifcClassificationReferenceUniClass.Description = "NOT PROVIDED";
                ifcClassificationReferenceUniClass.ReferencedSource = ifcClassificationSystemUniClass;

                var ifcRelAssociatesClassificationUniClass = model.Instances.New<IfcRelAssociatesClassification>();
                ifcRelAssociatesClassificationUniClass.RelatingClassification = ifcClassificationReferenceUniClass;

                //Insert a project library to store the product data templates and type products
                IfcProjectLibrary ifcProductDataLibrary = New<IfcProjectLibrary>(l => {
                    l.Name = "TriluxLightingProductsLibrary";
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
                var workbook = new XLWorkbook(Path.Combine(sourceFolder, sourceFile));
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
                                    pt.PrimaryUnit = model.Instances.New<IfcConversionBasedUnit>(punit=>
                                    {
                                        //Convert the angel measure from the unit grad to the SI Unit radian
                                        //rad=grad*(PI/180)
                                        punit.Name = "Grad";
                                        punit.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.PLANEANGLEUNIT;
                                        punit.ConversionFactor = model.Instances.New<IfcMeasureWithUnit>(mwu=>
                                        {
                                            mwu.UnitComponent = model.Instances.New<IfcSIUnit>(siUnit=>
                                            {
                                               siUnit.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.PLANEANGLEUNIT;
                                               siUnit.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.RADIAN;
                                            });
                                            mwu.ValueComponent = new IfcReal(Math.PI / 180);
                                        });
                                        punit.Dimensions = model.Instances.New<IfcDimensionalExponents>(dim=>
                                        {
                                            dim.LengthExponent = 0;
                                            dim.MassExponent = 0;
                                            dim.TimeExponent = 0;
                                            dim.ElectricCurrentExponent = 0;
                                            dim.ThermodynamicTemperatureExponent = 0;
                                            dim.AmountOfSubstanceExponent = 0;
                                            dim.LuminousIntensityExponent = 0;
                                        });
                                    });
                        })
                    });
                };
           

                ifcProductDataLibraryDeclarations.Add(ifcPropertySetTemplate);
  
                //Read source data from excel sheet
                var workbookData = new XLWorkbook(Path.Combine(sourceFolder, sourceFile));
                IXLWorksheet worksheetData;
                IXLRange rangeData;
                worksheetData = workbookData.Worksheet("Sheets");
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

                        ifcRelAssociatesClassificationOmniClass.RelatedObjects.Add(ifcTypeProduct);
                        ifcRelAssociatesClassificationUniClass.RelatedObjects.Add(ifcTypeProduct);

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

            string targetFileName = Path.Combine(targetFolder, targetFile);
            SaveAs(targetFileName, true, typeof(IfcProjectLibrary));

            //Create ifcZip file
            File.Delete(zipFile);
            ZipFile.CreateFromDirectory(sourceFolder, zipFile);
            using (ZipArchive zipArchive = ZipFile.Open(zipFile,ZipArchiveMode.Update))
            {
                zipArchive.GetEntry(sourceFile).Delete();
                zipArchive.CreateEntryFromFile($"{targetFolder}/{targetFile}.ifcXML", $"{targetFile}.ifcXML");
                zipArchive.CreateEntryFromFile($"{targetFolder}/{targetFile}.ifc", $"{targetFile}.ifc");
            }
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
