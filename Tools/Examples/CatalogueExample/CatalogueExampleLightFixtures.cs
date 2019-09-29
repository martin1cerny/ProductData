using System.Data;
using System.IO;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.PropertyResource;
using ClosedXML.Excel;
using System;
using Xbim.Ifc4.ExternalReferenceResource;
using System.IO.Compression;
using System.Linq;
using System.Collections.Generic;
using Xbim.Ifc4.UtilityResource;
using Xbim.Ifc4.ActorResource;

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


                //Insert Classification system
                var ifcClassificationSystemOmniClass = model.Instances.New<IfcClassification>(cs =>
                {
                    cs.Name = "Omniclass";
                    cs.Edition = "1.0";
                    cs.EditionDate = "2018-12-27T00:00:00.0000000";
                    cs.Description = "The OmniClass Construction Classification System (known as OmniClass™ or OCCS) is a classification system for the construction industry. OmniClass is useful for many applications, from organizing library materials, product literature, and project information, to providing a classification structure for electronic databases. It incorporates other extant systems currently in use as the basis of many of its Tables – MasterFormat™ for work results, UniFormat for elements, and EPIC (Electronic Product Information Cooperation) for structuring products.";
                    cs.Location = "http://www.omniclass.org/";
                });

                //Insertion of some sample classification references
                //I would be better to insert the whole classification system and reference the appropriate code
                var ifcClassificationReferenceOmniClass = model.Instances.New<IfcClassificationReference>(cr =>
                {
                    cr.Identification = "23-35-47";
                    cr.Name = "Electrical Lighting";
                    cr.Description = "";
                    cr.ReferencedSource = ifcClassificationSystemOmniClass;
                });

                var ifcRelAssociatesClassificationOmniClass = model.Instances.New<IfcRelAssociatesClassification>(relc=>
                {
                    relc.RelatingClassification = ifcClassificationReferenceOmniClass;
                });

                //Insert Classification system
                var ifcClassificationSystemUniClass = model.Instances.New<IfcClassification>(cs =>
                {
                    cs.Name = "Uniclass";
                    cs.Edition = "2015";
                    cs.EditionDate = "01.01.2015";
                    cs.Description = "Uniclass is a voluntary classification system for the construction industry that can be used for structuring project information, such as building information models (BIM).";
                    cs.Location = "https://www.thenbs.com/our-tools/introducing-uniclass-2015";
                });

                //Insertion of some sample classification references
                //I would be better to insert the whole classification system and reference the appropriate code
                var ifcClassificationReferenceUniClass = model.Instances.New<IfcClassificationReference>(cr =>
                {
                    cr.Identification = "CA-70-10-30";
                    cr.Name = "Site lighting equipment";
                    cr.Description = "";
                    cr.ReferencedSource = ifcClassificationSystemUniClass;
                });

                var ifcRelAssociatesClassificationUniClass = model.Instances.New<IfcRelAssociatesClassification>(relc=>
                { 
                    relc.RelatingClassification = ifcClassificationReferenceUniClass;
                });

                //Insert a project library to store the product data templates and type products

                var ifcProductDataLibrary = New<IfcProjectLibrary>(l => {
                    l.Name = "TriluxLightingProductsLibrary";
                    l.GlobalId = "1DbshdzGD71ejurQqQcxbw";
                    l.Description = "Library for Trilux light fixtures product data templates based on the ZVEI European core properties";
                    l.Phase = "Design,Build,Operate";
                    l.OwnerHistory = New<IfcOwnerHistory>(oh =>
                    {
                        oh.OwningUser = New<IfcPersonAndOrganization>(po =>
                        {
                            po.TheOrganization = New<IfcOrganization>(o =>
                            {
                                o.Name = "TRILUX GmbH & Co. KG";
                            });

                            po.ThePerson = New<IfcPerson>(p =>
                            {
                                p.GivenName = "Robert";
                                p.FamilyName = "Heinze";
                            });
                        });
                        oh.OwningApplication = New<IfcApplication>(app =>
                        {
                            app.ApplicationIdentifier = "ID_OF_PIM-SYSTEM";
                            app.ApplicationFullName = "My Product Information System (PIM)";
                            app.ApplicationDeveloper = New<IfcOrganization>(o =>
                            {
                                o.Name = "The software company, that developed the PIM system";
                            });
                            app.Version = "1.0";
                        });
                    });
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
                var workbookTemplates = new XLWorkbook(Path.Combine(sourceFolder, sourceFile));
                IXLWorksheet worksheetTemplates = workbookTemplates.Worksheet("Templates");
                //IXLRange rangeTemplates = worksheetTemplates.Range("A1:Z690");
                //IXLTable rawDataTemplates = rangeTemplates.AsTable();
                DataTable dtTemplates = ReadDataTable(worksheetTemplates);

                var productDataTemplates = from DataRow dr in dtTemplates.Rows orderby dr["DataTemplate"] group dr by dr["DataTemplate"];
                foreach (var productDataTemplate in productDataTemplates)
                {
                    IfcPropertySetTemplate ifcPropertySetTemplate = model.Instances.New<IfcPropertySetTemplate>(pset =>
                    {
                        pset.GlobalId = Xbim.Ifc4.UtilityResource.IfcGloballyUniqueId.ConvertToBase64(Guid.NewGuid());
                        pset.Name = productDataTemplate.Key.ToString();
                        pset.Description = "Data Template by " + productDataTemplate.ElementAt(0)["Publisher"].ToString();
                        pset.ApplicableEntity = "IfcLightFixture/USERDEFINED";
                        pset.TemplateType = Xbim.Ifc4.Interfaces.IfcPropertySetTemplateTypeEnum.PSET_TYPEDRIVENONLY;
                    });
                    Comment(ifcPropertySetTemplate, @"Declaration of 'IfcPropertySetTemplate' within the library for lighting product data templates.");
                    Comment(ifcPropertySetTemplate, @"Insert property templates; they should be loaded from the publishing dictionary");

                    foreach (DataRow propertyTemplate in productDataTemplate)
                    {
                        //Publisher SystemName  GlobalId PrimaryMeasureType  DataColumn
                        ifcPropertySetTemplate.HasPropertyTemplates.AddRange(new[]
                        {
                            model.Instances.New<IfcSimplePropertyTemplate>(pt =>
                            {
                                pt.Name = propertyTemplate["SystemName"].ToString();
                                pt.Description = propertyTemplate["Definition"].ToString();
                                pt.Expression = "";
                                pt.GlobalId = propertyTemplate["GlobalId"].ToString();
                                pt.TemplateType = Xbim.Ifc4.Interfaces.IfcSimplePropertyTemplateTypeEnum.P_SINGLEVALUE;
                                pt.AccessState = Xbim.Ifc4.Interfaces.IfcStateEnum.LOCKED;
                                pt.PrimaryMeasureType = propertyTemplate["PrimaryMeasureType"].ToString();

                                string primaryMeasureType = propertyTemplate["PrimaryMeasureType"].ToString();

                                if ((primaryMeasureType == "IfcDocumentInformation") ||(primaryMeasureType == "IfcClassificationReference"))
                                        {
                                            pt.PrimaryMeasureType = "IfcLabel";
                                        }
                                else if (primaryMeasureType == typeof(IfcLengthMeasure).Name)
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
                }
  
                //Read source data from excel sheet
                var workbookData = new XLWorkbook(Path.Combine(sourceFolder, sourceFile));
                IXLWorksheet worksheetData = workbookData.Worksheet("Sheets");
                //IXLRange rangeData = worksheetData.Range("A1:Z690");
                //IXLTable rawData = rangeData.AsTable();
                DataTable dtData = ReadDataTable(worksheetData);

                //Loop through the products in the data sheet
                foreach (DataRow product in dtData.Rows)
                {
                    var ifcTypeProduct = model.Instances.New<IfcTypeProduct>();                   
                    ifcTypeProduct.Name = product["Name"].ToString();
                    ifcTypeProduct.Description = "Description of " + ifcTypeProduct.Name;
                    ifcTypeProduct.ApplicableOccurrence = "IfcLightFixture";

                    //Create the property sets, and relate them to the templates
                    foreach (IfcPropertySetTemplate ifcPropertySetTemplate in model.Instances.OfType<IfcPropertySetTemplate>().ToList())
                    { 
                        IfcPropertySet ifcPropertySet = model.Instances.New<IfcPropertySet>(pset =>
                        {
                            pset.Name = ifcPropertySetTemplate.Name;
                            pset.Description = ifcPropertySetTemplate.Description;
                        });

                        //Relate the property set to the definition propert
                        var ifcRelDefinesByTemplate = New<IfcRelDefinesByTemplate>(dbt =>
                        {
                            dbt.RelatingTemplate = ifcPropertySetTemplate;
                        });
                        ifcRelDefinesByTemplate.RelatedPropertySets.Add(ifcPropertySet);
                        ifcTypeProduct.HasPropertySets.Add(ifcPropertySet);
                    }

                    //loop through the properties of the product, based on the data template
                    foreach (DataRow property in dtTemplates.Rows)
                    {
                        //Identify the correct IfcPropertySet for this property

                        IfcPropertySet ifcPropertySet = (IfcPropertySet)ifcTypeProduct.HasPropertySets
                                                                       .Where(x => x.Name == property["DataTemplate"].ToString())
                                                                       .FirstOrDefault();

                        switch (property["PrimaryMeasureType"].ToString())
                        {
                            case "IfcGloballyUniqueId":
                                //Insert the unique number for the product type

                                //Create a fallback 
                                string guid = product[property["SystemName"].ToString()].ToString();

                                IfcGloballyUniqueId IfcGlobalId;
                                try
                                {
                                    //Assume, that is is a UUID based on IETF RFC 4122
                                    IfcGlobalId = Xbim.Ifc4.UtilityResource.IfcGloballyUniqueId.ConvertToBase64(Guid.Parse(guid));
                                }
                                catch
                                {
                                    try
                                    {
                                        //Assume, that is is already an IFC compliant IfcGloballyUniqueId
                                        IfcGlobalId = Xbim.Ifc4.UtilityResource.IfcGloballyUniqueId.ConvertFromBase64(guid);
                                    }
                                    catch
                                    {
                                        //Generate a new GUID every time
                                        IfcGlobalId = Xbim.Ifc4.UtilityResource.IfcGloballyUniqueId.ConvertToBase64(Guid.NewGuid());
                                    }                                 
                                }

                                ifcTypeProduct.GlobalId = IfcGlobalId;
                                
                                break;

                            case "IfcDocumentInformation":
                                //Insert the product information that are in documents
                                string folderName = property["SystemName"].ToString();
                                string docName = product[property["SystemName"].ToString()].ToString();
                                if (docName.Length > 0)
                                { 
                                    string fileLocation = $"{folderName}/{docName}";

                                    IfcDocumentInformation ifcDocumentInformation;
                                    var existingInsertedDocumentInformation = model.Instances.OfType<IfcDocumentInformation>().Where(x => x.Location == fileLocation);
                                    if (existingInsertedDocumentInformation.Count() == 0)
                                    {
                                        ifcDocumentInformation = model.Instances.New<IfcDocumentInformation>(doc =>
                                        {
                                            doc.Identification = docName;
                                            doc.Name = docName;
                                            doc.Location = $@"{folderName}/{docName}";
                                            doc.CreationTime = DateTime.Now.ToString("dd.MM.yyyy");
                                            doc.Confidentiality = Xbim.Ifc4.Interfaces.IfcDocumentConfidentialityEnum.PUBLIC;
                                            doc.ElectronicFormat = MimeTypes.GetMimeType(docName);
                                            doc.IntendedUse = "Product information";
                                            doc.Purpose = "Product information";
                                            doc.ValidFrom = "01.01.2018";
                                            doc.ValidUntil = "31.12.2021";
                                            doc.Scope = "Europa";
                                            doc.Revision = "1.0";

                                        });

                                        string test = Path.GetExtension(docName);
                                        switch (Path.GetExtension(docName))
                                        {
                                            case ".pdf":
                                                ifcDocumentInformation.Description = "Produktdatenblatt";
                                                break;
                                            case ".3ds":
                                                ifcDocumentInformation.Description = "3D-Visualisierung";
                                                break;
                                            case ".jpg":
                                                ifcDocumentInformation.Description = "Produktphoto";
                                                break;
                                            case ".ies":
                                                ifcDocumentInformation.Description = "Lichtverteilung von IES Standard";
                                                break;
                                        }


                                        IfcRelAssociatesDocument ifcRelAssociatesDocument = model.Instances.New<IfcRelAssociatesDocument>(docref =>
                                        {
                                            docref.RelatedObjects.Add(ifcTypeProduct);
                                            docref.RelatingDocument = ifcDocumentInformation;
                                        });
                                    }
                                    else
                                    {
                                        ifcDocumentInformation = existingInsertedDocumentInformation.FirstOrDefault();
                                        var existingDocumentInformationRelation = model.Instances.OfType<IfcRelAssociatesDocument>()
                                                                                    .Where(x => x.RelatingDocument == ifcDocumentInformation).FirstOrDefault();

                                        existingDocumentInformationRelation.RelatedObjects.Add(ifcTypeProduct);
                                    }
                                }
                                break;

                            case "IfcClassificationReference":

                                switch (property["SystemName"].ToString())
                                {
                                    case "Omniclass":
                                        var classificationReference = model.Instances.OfType<IfcClassificationReference>()
                                                                                .Where(x => x.Identification == product[property["SystemName"].ToString()].ToString()).FirstOrDefault();

                                        ifcRelAssociatesClassificationOmniClass.RelatedObjects.Add(ifcTypeProduct);
                                        break;
                                    case "Uniclass":

                                        ifcRelAssociatesClassificationUniClass.RelatedObjects.Add(ifcTypeProduct);
                                        break;
                                }                                                                
                                    
                                break;

                            default:
                                //Insert the product information into the property set
                                ifcPropertySet.HasProperties.AddRange(new[]
                                {
                                    model.Instances.New<IfcPropertySingleValue>(p =>
                                    {
                                        string propertyName = property["SystemName"].ToString();
                                        var dataValue = product[propertyName];

                                        p.Name = propertyName;
                                        p.Description = "";

                                        string primaryMeasureType = property["PrimaryMeasureType"].ToString();
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
                                 
                                break;
                        }                                             
                    };

                    ifcProductDataLibraryDeclarations.Add(ifcTypeProduct);
                    Comment(ifcTypeProduct, @"Declaration of 'IfcTypeProduct' within the library for a ligthing product.");
                }

                txn.Commit();
            }

            string targetFileName = Path.Combine(targetFolder, targetFile);
            SaveAs(targetFileName, false, typeof(IfcProjectLibrary));

            //DirtyFix with Schema location
            //https://github.com/xBimTeam/XbimEssentials/issues/288
            string contentOfFile = File.ReadAllText($"{targetFileName}.ifcXML");
            string oldNameSpaceLocation = @"xsi:schemaLocation=""http://www.buildingsmart-tech.org/ifcXML/IFC4/Add2 http://www.buildingsmart-tech.org/ifc/IFC4/Add2/IFC4_ADD2.xsd""";
            string newNameSpaceLocation = @"xsi:schemaLocation=""http://www.buildingsmart-tech.org/ifcXML/IFC4/Add2 ../../IFC4_ADD2.xsd""";
            contentOfFile = contentOfFile.Replace(oldNameSpaceLocation, newNameSpaceLocation);
            File.WriteAllText($"{targetFileName}.ifcXML", contentOfFile);


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

                    var usedCells = row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber);
                    foreach (IXLCell cell in usedCells)
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
