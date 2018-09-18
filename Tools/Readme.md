# IFC sample files tooling

This is a Visual Studio solution containing a set of C# projects using [xBIM Toolkit](https://github.com/xBimTeam) 
to create and validate IFC4 files. 


## Schema Validator Project

Schema Validator is a command line application. When started without arguments it
asks for an input IFC file with standard Windows file open dialog. Otherwise any number of IFC files can be defined as arguments.
Files are check for syntactic errors (invalid type assignments, non-existing data types etc.) and for schema errors 
(required fields, cardinality of attributes, IFC "Where Rules"). These are all written both to console and to *.log* file 
next to the original IFC file. Validator can take both *.ifc* and *.ifcXML* files. Part of the functionality is specific to IFC4
schema.

Validator can also be referenced in other projects as a library. In that scenario models created with [xBIM Toolkit](https://github.com/xBimTeam) can be validated to meet base schema constrains before they are saved. The use is like this:

```CS
var validator = new Validator();
var logFile = "errors.log";
// if logFile argument is null no file output is created
var ok = validator.Check(model, logFile);
if (!ok)
    throw new Exception(string.Concat("\r\n", validator.Errors.Select(e => e.Message)));
```

## Examples Project

Examples is a project which can be used to produce commented IFC4 examples both as *.ifc* and *.ifcXML*. It contains 
abstract class `AbstractExample` which contains number of utility methods which support rapid development of examples.
Examples can either be added to this project or can just reference this assembly as a library and inherit from the
`AbstractExample` class.

For example:

```cs
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
```

This will produce these two files:

Step21:

```
/****************************************************************
 * This is a basic example showing elementary structure of IFC project library
 * 
 * Created: 2018-09-18T14:57:05 (UTC)
 ****************************************************************/
ISO-10303-21;
HEADER;
FILE_DESCRIPTION ((''), '2;1');
FILE_NAME ('', '2018-09-18T16:57:05', (''), (''), 'Xbim File Processor version 4.0.0.0', 'Xbim version 4.0.0.0', '');
FILE_SCHEMA (('IFC4'));
ENDSEC;
DATA;

/* * *
 * Root element of this file. Because this doesn't define a specific instance in the building it is a library.
 * It can be used to declare elements, properties, property templates and other library objects 
 * which can be later used in the actual design.
 */
#1=IFCPROJECTLIBRARY('16KZ2oG$jFJ99bf3sKW_LK',#2,'Basic Library',$,$,$,$,$,$);

/* * *
 * Owner history is used to define ownership of the information.
 */
#2=IFCOWNERHISTORY(#5,#6,$,.ADDED.,1537289825,$,$,0);
#3=IFCPERSON($,'Martin','Cerny',$,$,$,$,$);
#4=IFCORGANIZATION($,'xBIM Ltd.',$,$,$);
#5=IFCPERSONANDORGANIZATION(#3,#4,$);
#6=IFCAPPLICATION(#4,'4.0','xBIM Toolkit','xBIM');

/* * *
 * This relation points to all definitions declared within the scope of the library.
 * These can be elements, element types, properties or property templates
 */
#7=IFCRELDECLARES('3ptNTDi3H3bfdELHLPsGHQ',#2,$,$,#1,(#8));

/* * *
 * The best IfcElement subtype for brick is 'IfcBuildingElementPart'. 
 * This type should not be considered as significant because authoring tools will often not support all 
 * IFC element types and might not provide sufficient configurations or modelling workflows to create
 * such a data. Combination of 'PredefinedType = USERDEFINED' and 'ObjectType = "BRICK"' can be
 * used to identify the element. 'IfcBuildingElementPart' might also be used for materials.
 */
#8=IFCBUILDINGELEMENTPART('3LcbelihPEResuCmyU8CSr',#2,'Porotherm 50 EKO+ Profi R',$,'BRICK',$,$,$,.USERDEFINED.);
ENDSEC;
END-ISO-10303-21;
```

XML:

```xml
<?xml version="1.0" encoding="utf-8"?>
<!-- This is a basic example showing elementary structure of IFC project library -->
<!--  -->
<!-- Created: 2018-09-18T14:57:05 (UTC) -->
<ifcXML xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:ifc="http://www.buildingsmart-tech.org/ifcXML/IFC4/Add2" xsi:schemaLocation="http://www.buildingsmart-tech.org/ifcXML/IFC4/Add2 http://www.buildingsmart-tech.org/ifc/IFC4/Add2/IFC4_ADD2.xsd" id="uos_1" express="http://www.buildingsmart-tech.org/ifc/IFC4/Add2/IFC4_ADD2.exp" configuration="http://www.buildingsmart-tech.org/ifc/IFC4/Add2/IFC4_ADD2_config.xml" xmlns="http://www.buildingsmart-tech.org/ifcXML/IFC4/Add2">
  <header>
    <time_stamp>2018-09-18T16:57:05</time_stamp>
    <preprocessor_version>Xbim File Processor version 4.0.0.0</preprocessor_version>
    <originating_system>Xbim version 4.0.0.0</originating_system>
  </header>
  <!-- Root element of this file. Because this doesn't define a specific instance in the building it is a library. -->
  <!-- It can be used to declare elements, properties, property templates and other library objects  -->
  <!-- which can be later used in the actual design. -->
  <IfcProjectLibrary id="i1" GlobalId="16KZ2oG$jFJ99bf3sKW_LK" Name="Basic Library">
    <!-- Owner history is used to define ownership of the information. -->
    <OwnerHistory id="i2" xsi:type="IfcOwnerHistory" ChangeAction="added" LastModifiedDate="1537289825" CreationDate="0">
      <OwningUser id="i5" xsi:type="IfcPersonAndOrganization">
        <ThePerson id="i3" xsi:type="IfcPerson" FamilyName="Martin" GivenName="Cerny" MiddleNames="" PrefixTitles="" SuffixTitles="" />
        <TheOrganization id="i4" xsi:type="IfcOrganization" Name="xBIM Ltd." />
      </OwningUser>
      <OwningApplication id="i6" xsi:type="IfcApplication" Version="4.0" ApplicationFullName="xBIM Toolkit" ApplicationIdentifier="xBIM">
        <ApplicationDeveloper ref="i4" xsi:type="IfcOrganization" xsi:nil="true" />
      </OwningApplication>
    </OwnerHistory>
    <Declares>
      <!-- This relation points to all definitions declared within the scope of the library. -->
      <!-- These can be elements, element types, properties or property templates -->
      <IfcRelDeclares id="i7" pos="0" GlobalId="3ptNTDi3H3bfdELHLPsGHQ">
        <OwnerHistory ref="i2" xsi:type="IfcOwnerHistory" xsi:nil="true" />
        <RelatedDefinitions>
          <!-- The best IfcElement subtype for brick is 'IfcBuildingElementPart'.  -->
          <!-- This type should not be considered as significant because authoring tools will often not support all  -->
          <!-- IFC element types and might not provide sufficient configurations or modelling workflows to create -->
          <!-- such a data. Combination of 'PredefinedType = USERDEFINED' and 'ObjectType = "BRICK"' can be -->
          <!-- used to identify the element. 'IfcBuildingElementPart' might also be used for materials. -->
          <IfcBuildingElementPart id="i8" pos="0" GlobalId="3LcbelihPEResuCmyU8CSr" Name="Porotherm 50 EKO+ Profi R" ObjectType="BRICK" PredefinedType="userdefined">
            <OwnerHistory ref="i2" xsi:type="IfcOwnerHistory" xsi:nil="true" />
          </IfcBuildingElementPart>
        </RelatedDefinitions>
      </IfcRelDeclares>
    </Declares>
  </IfcProjectLibrary>
</ifcXML>
```