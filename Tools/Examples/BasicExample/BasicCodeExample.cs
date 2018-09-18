using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.SharedComponentElements;

namespace Examples.BasicExample
{
    class BasicCodeExample : AbstractExample
    {
        /// <summary>
        /// These credentials will be passed to the model constructor to set an author of the example data
        /// in form of IfcOwnerHistory entity
        /// </summary>
        public BasicCodeExample() : base("Martin", "Cerny", "xBIM Ltd.") { }

        /// <summary>
        /// Annotation will be used as a header comment in generated IFC files
        /// </summary>
        public override string Annotation => "This is a basic example showing elementary structure of IFC project library";

        /// <summary>
        /// This is the main method to be called to create the example
        /// </summary>
        public override void Run()
        {
            // IFC4 model is already created by the framework
            using (var txn = model.BeginTransaction("Example creation"))
            {
                // New<IfcProjectLibrary>() is a shortcut for model.Instances.New<IfcProjectLibrary>()
                var lib = New<IfcProjectLibrary>(l => {
                    l.Name = "Basic Library";
                });
                // This comment will be placed above the library entity both in STEP21 and XML files by the framework
                Comment(lib, @"Root element of this file. Because this doesn't define a specific instance in the building it is a library.
It can be used to declare elements, properties, property templates and other library objects 
which can be later used in the actual design.");

                var declarations = New<IfcRelDeclares>(rel =>
                {
                    rel.RelatingContext = lib;
                }).RelatedDefinitions;
                Comment(declarations.OwningEntity, @"This relation points to all definitions declared within the scope of the library.
These can be elements, element types, properties or property templates");

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
used to identify the element. 'IfcBuildingElementPart' might also be used for materials.");

                // owner history is assigned automatically. Let's add a comment to that
                Comment(lib.OwnerHistory, @"Owner history is used to define ownership of the information.");

                // commit to confirm changes in the model
                txn.Commit();
            }

            // Utility method will save the model as 'BasicExample.ifc' and 'BasicExample.ifcXML'.
            // Both will have the same comments in place. True says that method should throw an exception
            // if there are any validation errors. This makes sure only valid IFC files are ever created.
            // typeof(IfcProjectLibrary) sets the root element for ifcXML file. Most important type from the
            // example should be specified here to create most understandable XML file.
            SaveAs("BasicExample", true, typeof(IfcProjectLibrary));
        }
    }
}
