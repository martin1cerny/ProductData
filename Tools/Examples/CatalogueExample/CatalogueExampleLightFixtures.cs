using System.Data;
using System.IO;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.PropertyResource;
using Xbim.Ifc4.SharedBldgElements;
using Xbim.Ifc4.SharedComponentElements;
using ClosedXML.Excel;
using System;

namespace Examples.CatalogueExample
{
    class CatalogueExampleLightFixtures : AbstractExample
    {
        /// <summary>
        /// These credentials will be passed to the model constructor to set an author of the example data
        /// in form of IfcOwnerHistory entity
        /// </summary>
        public CatalogueExampleLightFixtures() : base("Klaus", "Aengenvoort", "CEN-TC442-WG2-TG3") { }
        private string baseFolder = @"C:\Users\Aengenvoort.ZENTRALE\Downloads\TRILUX_Baselist_RH190520";
        private string sourceFile = "RawData.xlsx";
        private string sheetName = "MassData";
        private string targetFile = "ProductDataGenerated";

        public override string Annotation => "This is a example showing elementary structure of IFC project library that contains a configurable product in a catalogue as an import from an Excel sheet";

        public override void Run()
        {
            using (var txn = model.BeginTransaction("Example creation"))
            {

                //How to fill the header with xBIM Eseentials?
                //model.Header.Name = "Product data file";
                //model.Header.Organization = "HC Italy";
                //model.Header.Author = "Ugo";
                //model.Header.Authorization = "CEN-TC442-WG2-TG3";
                //model.Header.TimeStamp = DateTime.Now;
                //model.Header.OriginatingSystem = "CEN-TC442-WG2-TG3-Writer";

                //there should always be one project in the model
                var project = model.Instances.New<IfcProject>(p => p.Name = "Basic Creation");
                //our shortcut to define basic default units
                project.Initialize(Xbim.Common.ProjectUnits.SIUnitsUK);

                //Insert a project library to store the product data templates
                var ifcProductDataLibrary = New<IfcProjectLibrary>(l => {
                    l.Name = "Library for brick product data templates";
                    l.GlobalId = "1DbshdzGD71ejurQqQcxbw";
                    l.Description = "";
                    l.Phase = "";
                });
                Comment(ifcProductDataLibrary, @"Root element of this file. Because this doesn't define a specific instance in the building it is a library. It can be used to declare elements, properties, property templates and other library objects which can be later used in the actual design.");
                Comment(ifcProductDataLibrary.OwnerHistory, @"Owner history is used to define ownership of the information.");

                var ifcProductDataLibraryDeclarations = New<IfcRelDeclares>(rel =>
                {
                    rel.RelatingContext = ifcProductDataLibrary;
                }).RelatedDefinitions;
                Comment(ifcProductDataLibraryDeclarations.OwningEntity, @"This relation points to all definitions declared within the scope of the library. These can be elements, element types, properties or property templates");

                IfcPropertySetTemplate ifcPropertySetTemplate = model.Instances.New<IfcPropertySetTemplate>(pset=>
                {
                    pset.GlobalId = "1DbshTzGD71ejurQqQcxbw";
                    pset.Name = "My IfcPropertySetTemplate";
                    pset.Description = "Group of properties for " + ifcProductDataLibrary.Name;
                    pset.ApplicableEntity = "IfcWall/USERDEFINED";
                    pset.TemplateType = Xbim.Ifc4.Interfaces.IfcPropertySetTemplateTypeEnum.PSET_TYPEDRIVENONLY;
                    pset.HasPropertyTemplates.AddRange(new[]
                    {
                        model.Instances.New<IfcSimplePropertyTemplate>(pt =>
                        {
                            pt.Name = "zone";
                            pt.Description = "";
                            pt.GlobalId = "1DbshTzGD71ejuZQqQcxba";
                            pt.TemplateType = Xbim.Ifc4.Interfaces.IfcSimplePropertyTemplateTypeEnum.P_SINGLEVALUE;
                            pt.AccessState = Xbim.Ifc4.Interfaces.IfcStateEnum.LOCKED;
                            pt.PrimaryMeasureType = "IfcLabel";
                        }),
                        model.Instances.New<IfcSimplePropertyTemplate>(pt =>
                        {
                            pt.Name = "height";
                            pt.Description = "";
                            pt.GlobalId = "1DbshTzGD71ejuZQqQkxad";
                            pt.TemplateType = Xbim.Ifc4.Interfaces.IfcSimplePropertyTemplateTypeEnum.P_SINGLEVALUE;
                            pt.AccessState = Xbim.Ifc4.Interfaces.IfcStateEnum.LOCKED;
                            pt.PrimaryMeasureType = "IfcReal";
                            pt.PrimaryUnit = model.Instances.New<IfcSIUnit>(u=>
                            {
                                u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.METRE;
                                u.Prefix = Xbim.Ifc4.Interfaces.IfcSIPrefix.MILLI;
                                u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.LENGTHUNIT;
                            });
                        }),
                        model.Instances.New<IfcSimplePropertyTemplate>(pt =>
                        {
                            pt.Name = "width";
                            pt.Description = "";
                            pt.GlobalId = "1DbshTzGD71ejuZQqQcxac";
                            pt.TemplateType = Xbim.Ifc4.Interfaces.IfcSimplePropertyTemplateTypeEnum.P_SINGLEVALUE;
                            pt.AccessState = Xbim.Ifc4.Interfaces.IfcStateEnum.LOCKED;
                            pt.PrimaryMeasureType = "IfcReal";
                            pt.PrimaryUnit = model.Instances.New<IfcSIUnit>(u=>
                            {
                                u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.METRE;
                                u.Prefix = Xbim.Ifc4.Interfaces.IfcSIPrefix.MILLI;
                                u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.LENGTHUNIT;
                            });
                        }),
                        model.Instances.New<IfcSimplePropertyTemplate>(pt =>
                        {
                            pt.Name = "length";
                            pt.Description = "";
                            pt.GlobalId = "1DbshTzGD71ejuZQqQcxab";
                            pt.TemplateType = Xbim.Ifc4.Interfaces.IfcSimplePropertyTemplateTypeEnum.P_SINGLEVALUE;
                            pt.AccessState = Xbim.Ifc4.Interfaces.IfcStateEnum.LOCKED;
                            pt.PrimaryMeasureType = "IfcReal";
                            pt.PrimaryUnit = project.UnitsInContext.LengthUnit;
                        })
                    });
                });

                ifcProductDataLibraryDeclarations.Add(ifcPropertySetTemplate);
                Comment(ifcPropertySetTemplate, @"Declaration of 'IfcPropertySetTemplate' within the library for brick product data templates.");

                //Read source data from excel sheet
                var workbook = new XLWorkbook(Path.Combine(baseFolder, sourceFile));
                IXLWorksheet worksheet;
                IXLRange dataRange;
                worksheet = workbook.Worksheet(sheetName);
                dataRange = worksheet.Range("A1:E4231");
                IXLTable rawData = dataRange.AsTable();
                DataTable dt = ReadDataTable(worksheet);


                var ifcRelDefinesByTemplate = New<IfcRelDefinesByTemplate>(dbt =>
                {
                    dbt.RelatingTemplate = ifcPropertySetTemplate;
                });

                int n = 0;
                do
                {
                    foreach (DataRow row in dt.Rows)
                    {

                        var ifcTypeProduct = model.Instances.New<IfcTypeProduct>();
                        ifcTypeProduct.GlobalId = "1DbshTzGD71ejurQqQcxbw"; //How to generate a fresh IFC GlobalId with XBim?
                        ifcTypeProduct.Tag = string.Format("{0}", row["Id"].ToString());
                        ifcTypeProduct.Name = string.Format("Brick Product variant #{0}", row["Id"].ToString());
                        ifcTypeProduct.Description = "Description of " + ifcTypeProduct.Name;
                        ifcTypeProduct.ApplicableOccurrence = "IfcWall";

                        IfcPropertySet ifcPropertySet = model.Instances.New<IfcPropertySet>(pset =>
                        {
                            pset.Name = "Properties of "+ ifcTypeProduct.Name;
                            pset.HasProperties.AddRange(new[]
                            {
                                model.Instances.New<IfcPropertySingleValue>(p =>
                                {
                                    p.Name = "zone";
                                    p.Description = "";
                                    p.NominalValue = new IfcText(row["Zone"].ToString().TrimEnd());
                                }),

                                model.Instances.New<IfcPropertySingleValue>(p =>
                                {
                                    p.Name = "height";
                                    p.NominalValue = new IfcLengthMeasure(Double.Parse(row["Height"].ToString()));
                                    p.Unit = project.UnitsInContext.LengthUnit;
                                }),
                                model.Instances.New<IfcPropertySingleValue>(p =>
                                {
                                    p.Name = "width";
                                    p.NominalValue = new IfcLengthMeasure(Double.Parse(row["Width"].ToString()));
                                    p.Unit = project.UnitsInContext.LengthUnit;
                                }),
                                model.Instances.New<IfcPropertySingleValue>(p =>
                                {
                                    p.Name = "lenght";
                                    p.NominalValue = new IfcLengthMeasure(Double.Parse(row["Length"].ToString()));
                                    p.Unit = project.UnitsInContext.LengthUnit;
                                })
                             });
                        });

                        ifcTypeProduct.HasPropertySets.Add(ifcPropertySet);
                        ifcProductDataLibraryDeclarations.Add(ifcTypeProduct);
                        Comment(ifcTypeProduct, @"Declaration of 'IfcTypeProduct' within the library for brick product data templates.");

                        ifcRelDefinesByTemplate.RelatedPropertySets.Add(ifcPropertySet);
                    }
                    n++;
                }
                while (n < 1);

                // commit to confirm changes in the model

                txn.Commit();
            }

            string targetFileName = Path.Combine(baseFolder, targetFile + sheetName);
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
