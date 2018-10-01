# IFC sample files tooling

This is a Visual Studio solution containing a set of C# projects using [xBIM Toolkit](https://github.com/xBimTeam) 
to create and validate IFC4 files.


## Schema Validator Project

Schema Validator is a command line application. When started without arguments it asks for an input IFC file with standard Windows file open dialog. Otherwise any number of IFC files can be defined as arguments.
Files are checked for syntactic errors, e.g.

- invalid type assignments
- non-existing data types
- schema errors
- required fields
- cardinality of attributes 
- IFC "Where Rules"
- and more

These are all written both to console and to *.log* file next to the original IFC file. The validator can take both *.ifc* and *.ifcXML* files. Part of the functionality is specific to the IFC4 schema.

### Use the Schema Validator in command line mode

1. Download the latest build of the SchemaValidator.zip from [here](https://ci.appveyor.com/api/projects/klacol/productdata/artifacts/Tools%2FSchemaValidator%2Fbin%2FSchemaValidator.zip)
2. Unzip the build into a local folder an your computer
3. Start the SchemeValidator.exe as following
```powershell
SchemaValidator.exe "The file you want to validate.ifcXML"
```
4. Look into the log file, that is located next to your IFC file. If the file is emtpy, your file is valid, Good Job :-)

### Use the Schema Validator in your own software

Validator can also be referenced in other projects as a library. In that scenario models created with 
[xBIM Toolkit](https://github.com/xBimTeam) can be validated to meet base schema constrains before they are saved. The use is like this:

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
`AbstractExample` class. Utility methods include:
 - Model creation - IFC4 in-memory model is created automaticaly and ready for use
 - Comments and annotations - It is useful to extend sample files with comments to explain meaning and intended use of various data types and data structures. Utility method `Comment()` can be used to set these comments. Abstract property `Annotation` is used to create a header in the generated **.ifc* and **.ifcXML* files.
 - Dual file generation - using utility method `SaveAs()` creates both **.ifc* and **.ifcXML* files both containing annotation in the header and all comments placed just above the relevant entities. Also, any commented entity in IFC STEP21 file (**.ifc*) will contain comment lile with names of attributes in right order to make developer navigation in the file easier

### Sample file generation code example:

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
 * This example shows how complex properties can be used to describe
 * declaration of performance of construction products. This utilises strong measure typing system and
 * units from IFC schema. This approach can be used to store CPR data.
 * 
 * Created: 2018-09-19T10:14:12 (UTC)
 ****************************************************************/
ISO-10303-21;
HEADER;
FILE_DESCRIPTION (('ProductDataView'), '2;1');
FILE_NAME ('', '2018-09-19T12:14:11', (''), (''), 'Xbim File Processor version 4.0.0.0', 'Xbim version 4.0.0.0', '');
FILE_SCHEMA (('IFC4'));
ENDSEC;
DATA;

/* * *
 * Root element of this file. Because this doesn't define a specific instance in the building it is a library.
 * It can be used to declare elements, properties, property templates and other library objects 
 * which can be later used in the actual design.
 * IFCPROJECTLIBRARY(GlobalId, OwnerHistory, Name, Description, ObjectType, LongName, Phase, RepresentationContexts, UnitsInContext) */
#1=IFCPROJECTLIBRARY('0DVzLtYpf1H9cLmN52Kli6',#2,'Declaration of Performance',$,$,$,$,$,#8);

/* * *
 * Owner history is used to define ownership of the information.
 * IFCOWNERHISTORY(OwningUser, OwningApplication, State, ChangeAction, LastModifiedDate, LastModifyingUser, LastModifyingApplication, CreationDate) */
#2=IFCOWNERHISTORY(#5,#6,$,.ADDED.,1537359252,$,$,0);
#3=IFCPERSON($,'Martin','Cerny',$,$,$,$,$);
#4=IFCORGANIZATION($,'xBIM Ltd.',$,$,$);
#5=IFCPERSONANDORGANIZATION(#3,#4,$);
#6=IFCAPPLICATION(#4,'4.0','xBIM Toolkit','xBIM');

/* * *
 * All units are machine readable in IFC. They are either SI or their definitions
 * are infered from SI units. Because of that, relations between all units are well defined,
 * reliable and it is possible to validate them.
 * IFCSIUNIT(Dimensions, UnitType, Prefix, Name) */
#7=IFCSIUNIT(*,.LENGTHUNIT.,.MILLI.,.METRE.);

/* * *
 * Units are assigned to the library project to that when units are not explicitely
 * defined for properties thay can be found on the project level. There are exact rules to find units for
 * all measure types (for example 'LENGTHUNIT' for 'IfcLengthMeasue' etc.)
 * IFCUNITASSIGNMENT(Units) */
#8=IFCUNITASSIGNMENT((#7));

/* * *
 * This relation points to all definitions declared within the scope of the library.
 * These can be elements, element types, properties or property templates
 * IFCRELDECLARES(GlobalId, OwnerHistory, Name, Description, RelatingContext, RelatedDefinitions) */
#9=IFCRELDECLARES('1zZpFD4OD5AQd1c57I$gzb',#2,$,$,#1,(#10,#14));

/* * *
 * Template of the property set is used to group properties in logical groups.
 * This can contain any number of properties with name unique within the set.
 * IFCPROPERTYSETTEMPLATE(GlobalId, OwnerHistory, Name, Description, TemplateType, ApplicableEntity, HasPropertyTemplates) */
#10=IFCPROPERTYSETTEMPLATE('17gc5IwQf2o97OhSr1mo5L',#2,'dimensions',$,$,'IfcElement',(#11));

/* * *
 * Simple name-definition-value property template defining actual value, type of measure (length) and unit (mm)
 * IFCSIMPLEPROPERTYTEMPLATE(GlobalId, OwnerHistory, Name, Description, TemplateType, PrimaryMeasureType, SecondaryMeasureType, Enumerators, PrimaryUnit, SecondaryUnit, Expression, AccessState) */
#12=IFCSIMPLEPROPERTYTEMPLATE('2Moa3XjTfFUAJ48qoRZbkX',#2,'Value',$,.P_SINGLEVALUE.,'IfcLengthMeasure',$,$,#7,$,$,$);

/* * *
 * Reference property template pointing to document relevant to the 'length' property
 * IFCSIMPLEPROPERTYTEMPLATE(GlobalId, OwnerHistory, Name, Description, TemplateType, PrimaryMeasureType, SecondaryMeasureType, Enumerators, PrimaryUnit, SecondaryUnit, Expression, AccessState) */
#13=IFCSIMPLEPROPERTYTEMPLATE('1Ex1IP3WH9gPYuXtsr342d',#2,'ReferenceDocument',$,.P_REFERENCEVALUE.,'IfcDocumentReference',$,$,$,$,$,$);

/* * *
 * Template of the complex property to define all it's aspects. This can contain any number of descriptive properties.
 * Both property set template and property template contain globally unique GUID which 
 * can be used for global management of definitions and to maintain relations to data dictionaries.
 * IFCCOMPLEXPROPERTYTEMPLATE(GlobalId, OwnerHistory, Name, Description, UsageName, TemplateType, HasPropertyTemplates) */
#11=IFCCOMPLEXPROPERTYTEMPLATE('2wstqGVyTBsAQCrWD0XQEd',#2,'length',$,'length',$,(#12,#13));

/* * *
 * The best IfcElement subtype for brick is 'IfcBuildingElementPart'. 
 * This type should not be considered as significant because authoring tools will often not support all 
 * IFC element types and might not provide sufficient configurations or modelling workflows to create
 * such a data. Combination of 'PredefinedType = USERDEFINED' and 'ObjectType = "BRICK"' can be
 * used to identify the element. 'IfcBuildingElementPart' should also be used for materials.
 * IFCBUILDINGELEMENTPART(GlobalId, OwnerHistory, Name, Description, ObjectType, ObjectPlacement, Representation, Tag, PredefinedType) */
#14=IFCBUILDINGELEMENTPART('0wbosbQMrDUe$EyMKPc5RL',#2,'Porotherm 50 EKO+ Profi R',$,'BRICK',$,$,$,.USERDEFINED.);
#17=IFCPROPERTYSINGLEVALUE('Value',$,IFCLENGTHMEASURE(300.),#7);
#19=IFCDOCUMENTREFERENCE($,$,'EN 772-1',$,$);
#18=IFCPROPERTYREFERENCEVALUE('ReferenceDocument',$,$,#19);
#16=IFCCOMPLEXPROPERTY('length',$,'length',(#17,#18));

/* * *
 * This property set contains actual values and points to the defining template.
 * IFCPROPERTYSET(GlobalId, OwnerHistory, Name, Description, HasProperties) */
#15=IFCPROPERTYSET('2uPPBcw016CwNQtPB4qu5V',#2,'dimensions',$,(#16));

/* * *
 * This relation binds actual property set with values to it's template.
 * IFCRELDEFINESBYTEMPLATE(GlobalId, OwnerHistory, Name, Description, RelatedPropertySets, RelatingTemplate) */
#20=IFCRELDEFINESBYTEMPLATE('3pVuLUXij2ZwunflqyZPXD',#2,$,$,(#15),#10);

/* * *
 * This relation sets the set of properties to the library element
 * IFCRELDEFINESBYPROPERTIES(GlobalId, OwnerHistory, Name, Description, RelatedObjects, RelatingPropertyDefinition) */
#21=IFCRELDEFINESBYPROPERTIES('02YPUmxP54oAQyDBtmUmez',#2,$,$,(#14),#15);
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
