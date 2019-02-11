using Xbim.Ifc4.ActorResource;
using Xbim.Ifc4.ExternalReferenceResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.ProcessExtension;
using Xbim.Ifc4.ProductExtension;
using Xbim.Ifc4.SharedMgmtElements;

namespace Examples.LOIN
{
    internal class LoinExample : AbstractExample
    {
        public LoinExample() : base("Bob", "Builder", "The Builders Ltd.")
        {
        }

        public override string Annotation => "Level of Information Need";

        public override void Run()
        {
            using (var txn = model.BeginTransaction())
            {
                // LOIN = level of information need for specific purpose, role, milestone and classification
                // root object type, there might be many of them
                var loin = New<IfcProjectLibrary>(lib =>
                {
                    lib.Name = "Level of Information Need";
                    Comment(lib, "Root level element which brings together purpose, actor, milestone, classification and requirements");
                    Comment(lib, "expressed as a set of property set templates. There might be one or more IfcProjectLibrary objects");
                    Comment(lib, "in a single file.");
                });

                // Purpose of the data requirement/exchange
                New<IfcRelAssignsToControl>(rel =>
                {
                    rel.RelatedObjects.Add(loin);
                    rel.RelatingControl = New<IfcActionRequest>(r =>
                    {
                        r.Name = "Thermal Analysis Information Exchange Request";
                        r.ObjectType = "INFORMATION_REQUEST";
                        r.PredefinedType = IfcActionRequestTypeEnum.USERDEFINED;
                        Comment(r, "Action request is used to describe the reason or purpose of the data exchange. It might also be called 'BIM Use'");
                    });
                    Comment(rel, "Relation assigning 'Reason' to 'Level of Information Need'");
                });

                // Actor / Role = Who in interested in the data
                New<IfcRelAssignsToActor>(r =>
                {
                    r.ActingRole = New<IfcActorRole>(ar => ar.Role = IfcRoleEnum.CLIENT);
                    r.RelatedObjects.Add(loin);
                    r.RelatingActor = New<IfcActor>(a =>
                    {
                        a.TheActor = New<IfcPerson>(p =>
                        {
                            p.FamilyName = "Builder";
                            p.GivenName = "Bob";
                        });
                    });
                    Comment(r, "Relation assigning actor and/or role to the 'Level of Information Need'");
                });

                // Milestone = point in time
                New<IfcRelAssignsToProcess>(rel =>
                {
                    rel.RelatedObjects.Add(loin);
                    rel.RelatingProcess = New<IfcTask>(t =>
                    {
                        t.Name = "Initial design";
                        t.IsMilestone = true;
                    });
                    Comment(rel, "Relation assigning milestone (point in time) when the information is required.");
                });


                // Classification = subject of interest
                New<IfcRelAssociatesClassification>(rel =>
                {
                    rel.RelatedObjects.Add(loin);
                    rel.RelatingClassification =
                        New<IfcClassificationReference>(c => {
                            c.Identification = "NF2.3";
                            c.ReferencedSource = New<IfcClassification>(cs => {
                                cs.Name = "Uniclass 2015";
                                cs.Description = "Unified Classification System";
                                cs.Source = "https://toolkit.thenbs.com/articles/classification#classificationtables";
                            });
                            Comment(c, "Classification reference is pointing to the classification system where it is defined.");
                            Comment(c, "Many classification systems might exist in a single model/file");
                        });
                    Comment(rel, "Relation associating classification reference to the 'Level of Information Need' to express the subject of requirements.");
                });

                // Declared data requirements / templates
                New<IfcRelDeclares>(decl =>
                {
                    decl.RelatingContext = loin;
                    decl.RelatedDefinitions.Add(New<IfcPropertySetTemplate>(ps =>
                    {
                        ps.Name = "Performance Data";
                        ps.ApplicableEntity = nameof(IfcCivilElement);
                        ps.HasPropertyTemplates.AddRange(new[] {
                                New<IfcSimplePropertyTemplate>(p => {
                                    p.Name = "Finish Grade";
                                    p.PrimaryMeasureType = nameof(IfcIdentifier);
                                    p.TemplateType = IfcSimplePropertyTemplateTypeEnum.P_SINGLEVALUE;
                                    New<IfcRelAssociatesDocument>(rd => {
                                        rd.RelatedObjects.Add(p);
                                        rd.RelatingDocument = New<IfcDocumentReference>(doc => {
                                            doc.Identification = "EN 14351-1";
                                            doc.Location = "https://standards.cen.eu/dyn/www/f?p=204:110:0::::FSP_PROJECT:34668&cs=1D6D74C0078C46BD2D89C093BF069EB14";
                                            doc.Name = "Windows and doors - Product standard, performance characteristics - Part 1: Windows and external pedestrian doorsets without resistance to fire and/or smoke leakage characteristics";

                                        });
                                    Comment(rd, "Additional relations can be used to specify document(s) relevant to the property as a definition source for example.");
                                    });
                                }),
                                New<IfcSimplePropertyTemplate>(p => {
                                    p.Name = "Fire Resistance";
                                    p.PrimaryMeasureType = nameof(IfcTimeMeasure);
                                    p.PrimaryUnit = New<IfcSIUnit>(t => {
                                        t.Name = IfcSIUnitName.SECOND;
                                        t.Prefix = IfcSIPrefix.MILLI;
                                        t.UnitType = IfcUnitEnum.TIMEUNIT;
                                    });
                                    p.TemplateType = IfcSimplePropertyTemplateTypeEnum.P_SINGLEVALUE;
                                })
                                // .... add all properties here
                            });
                        Comment(ps, "Property set templates and property templates are used to express required structure of the data.");
                        Comment(ps, "Additionally, constraints might be used to specify required/allowed values or ranges.");
                    }));
                    // ... add all property sets here

                    Comment(decl, "Relationship defining all declarations/requirements within the scope of 'Level of Information Need'");
                });

                txn.Commit();
            }

            try
            {
                Validate(false, true);
            }
            catch (System.Exception)
            {
            }
            SaveAs("LevelOfInformationNeed", true, typeof(IfcProjectLibrary));
        }
    }
}
