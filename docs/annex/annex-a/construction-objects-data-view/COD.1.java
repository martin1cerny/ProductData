	public class IfcRelAssociatesConstraint extends IfcRelAssociates
	{
		private string Intent;
		private IfcConstraint RelatingConstraint;
	}
	

	public enum IfcSimplePropertyTemplateTypeEnum
	{
		P_SINGLEVALUE,
		P_ENUMERATEDVALUE,
		P_BOUNDEDVALUE,
		P_LISTVALUE,
		P_TABLEVALUE,
		P_REFERENCEVALUE,
		Q_LENGTH,
		Q_AREA,
		Q_VOLUME,
		Q_COUNT,
		Q_WEIGHT,
		Q_TIME,
	}
	

	public interface IfcDefinitionSelect
	{
	}
	

	public class IfcComplexPropertyTemplate extends IfcPropertyTemplate
	{
		private string UsageName;
		private IfcComplexPropertyTemplateTypeEnum TemplateType;
		private IfcPropertyTemplate[] HasPropertyTemplates;
	}
	

	public class IfcContext extends IfcObjectDefinition
	{
		private string ObjectType;
		private string LongName;
		private string Phase;
		private IfcRepresentationContext[] RepresentationContexts;
		private IfcUnitAssignment UnitsInContext;
		private IfcRelDefinesByProperties[] IsDefinedBy;
		private IfcRelDeclares[] Declares;
	}
	

	public class IfcObject extends IfcObjectDefinition
	{
		private string ObjectType;
		private IfcRelDefinesByObject[] IsDeclaredBy;
		private IfcRelDefinesByObject[] Declares;
		private IfcRelDefinesByType[] IsTypedBy;
		private IfcRelDefinesByProperties[] IsDefinedBy;
	}
	

	public class IfcObjectDefinition extends IfcRoot
	{
		private IfcRelAssigns[] HasAssignments;
		private IfcRelNests[] Nests;
		private IfcRelNests[] IsNestedBy;
		private IfcRelDeclares[] HasContext;
		private IfcRelAggregates[] IsDecomposedBy;
		private IfcRelAggregates[] Decomposes;
		private IfcRelAssociates[] HasAssociations;
	}
	

	public class IfcProjectLibrary extends IfcContext
	{
	}
	

	public class IfcPropertyDefinition extends IfcRoot
	{
		private IfcRelDeclares[] HasContext;
		private IfcRelAssociates[] HasAssociations;
	}
	

	public class IfcPropertySet extends IfcPropertySetDefinition
	{
		private IfcProperty[] HasProperties;
	}
	

	public class IfcPropertySetDefinition extends IfcPropertyDefinition
	{
		private IfcTypeObject[] DefinesType;
		private IfcRelDefinesByTemplate[] IsDefinedBy;
		private IfcRelDefinesByProperties[] DefinesOccurrence;
	}
	

	public class IfcPropertySetTemplate extends IfcPropertyTemplateDefinition
	{
		private IfcPropertySetTemplateTypeEnum TemplateType;
		private string ApplicableEntity;
		private IfcPropertyTemplate[] HasPropertyTemplates;
		private IfcRelDefinesByTemplate[] Defines;
	}
	

	public class IfcPropertyTemplate extends IfcPropertyTemplateDefinition
	{
		private IfcComplexPropertyTemplate[] PartOfComplexTemplate;
		private IfcPropertySetTemplate[] PartOfPsetTemplate;
	}
	

	public class IfcPropertyTemplateDefinition extends IfcPropertyDefinition
	{
	}
	

	public class IfcRelAssociates extends IfcRelationship
	{
		private IfcDefinitionSelect[] RelatedObjects;
	}
	

	public class IfcRelAssociatesClassification extends IfcRelAssociates
	{
		private IfcClassificationSelect RelatingClassification;
	}
	

	public class IfcRelAssociatesDocument extends IfcRelAssociates
	{
		private IfcDocumentSelect RelatingDocument;
	}
	

	public class IfcRelationship extends IfcRoot
	{
	}
	

	public class IfcRelDefines extends IfcRelationship
	{
	}
	

	public class IfcRelDefinesByTemplate extends IfcRelDefines
	{
		private IfcPropertySetDefinition[] RelatedPropertySets;
		private IfcPropertySetTemplate RelatingTemplate;
	}
	

	public class IfcRelDefinesByType extends IfcRelDefines
	{
		private IfcObject[] RelatedObjects;
		private IfcTypeObject RelatingType;
	}
	

	public class IfcRoot extends IfcBase
	{
		private string GlobalId;
		private IfcOwnerHistory OwnerHistory;
		private string Name;
		private string Description;
	}
	

	public class IfcSimplePropertyTemplate extends IfcPropertyTemplate
	{
		private IfcSimplePropertyTemplateTypeEnum TemplateType;
		private string PrimaryMeasureType;
		private string SecondaryMeasureType;
		private IfcPropertyEnumeration Enumerators;
		private IfcUnit PrimaryUnit;
		private IfcUnit SecondaryUnit;
		private string Expression;
		private IfcStateEnum AccessState;
	}
	

	public class IfcTypeObject extends IfcObjectDefinition
	{
		private string ApplicableOccurrence;
		private IfcPropertySetDefinition[] HasPropertySets;
		private IfcRelDefinesByType[] Types;
	}
	

	public enum IfcBenchmarkEnum
	{
		GREATERTHAN,
		GREATERTHANOREQUALTO,
		LESSTHAN,
		LESSTHANOREQUALTO,
		EQUALTO,
		NOTEQUALTO,
		INCLUDES,
		NOTINCLUDES,
		INCLUDEDIN,
		NOTINCLUDEDIN,
	}
	

	public enum IfcConstraintEnum
	{
		HARD,
		SOFT,
		ADVISORY,
		USERDEFINED,
		NOTDEFINED,
	}
	

	public interface IfcMetricValueSelect
	{
	}
	

	public class IfcConstraint extends IfcBase
	{
		private string Name;
		private string Description;
		private IfcConstraintEnum ConstraintGrade;
		private string ConstraintSource;
		private IfcActorSelect CreatingActor;
		private string CreationTime;
		private string UserDefinedGrade;
		private IfcExternalReferenceRelationship[] HasExternalReferences;
		private IfcResourceConstraintRelationship[] PropertiesForConstraint;
	}
	

	public class IfcMetric extends IfcConstraint
	{
		private IfcBenchmarkEnum Benchmark;
		private string ValueSource;
		private IfcMetricValueSelect DataValue;
		private IfcReference ReferencePath;
	}
	

	/* IfcDate : STRING (Java does not support structures, so usage of defined types are inline for efficiency.) */
	

	/* IfcURIReference : STRING (Java does not support structures, so usage of defined types are inline for efficiency.) */
	

	public interface IfcClassificationReferenceSelect
	{
	}
	

	public interface IfcClassificationSelect
	{
	}
	

	public interface IfcDocumentSelect
	{
	}
	

	public class IfcClassification extends IfcExternalInformation
	{
		private string Source;
		private string Edition;
		private string EditionDate;
		private string Name;
		private string Description;
		private string Location;
		private string[] ReferenceTokens;
		private IfcRelAssociatesClassification[] ClassificationForObjects;
		private IfcClassificationReference[] HasReferences;
	}
	

	public class IfcClassificationReference extends IfcExternalReference
	{
		private IfcClassificationReferenceSelect ReferencedSource;
		private string Description;
		private string Sort;
		private IfcRelAssociatesClassification[] ClassificationRefForObjects;
		private IfcClassificationReference[] HasReferences;
	}
	

	public class IfcDocumentInformation extends IfcExternalInformation
	{
		private string Identification;
		private string Name;
		private string Description;
		private string Location;
		private string Purpose;
		private string IntendedUse;
		private string Scope;
		private string Revision;
		private IfcActorSelect DocumentOwner;
		private IfcActorSelect[] Editors;
		private string CreationTime;
		private string LastRevisionTime;
		private string ElectronicFormat;
		private string ValidFrom;
		private string ValidUntil;
		private IfcDocumentConfidentialityEnum Confidentiality;
		private IfcDocumentStatusEnum Status;
		private IfcRelAssociatesDocument[] DocumentInfoForObjects;
		private IfcDocumentReference[] HasDocumentReferences;
		private IfcDocumentInformationRelationship[] IsPointedTo;
		private IfcDocumentInformationRelationship[] IsPointer;
	}
	

	public class IfcDocumentReference extends IfcExternalReference
	{
		private string Description;
		private IfcDocumentInformation ReferencedDocument;
		private IfcRelAssociatesDocument[] DocumentRefForObjects;
	}
	

	public class IfcExternalInformation extends IfcBase
	{
	}
	

	public class IfcExternalReference extends IfcBase
	{
		private string Location;
		private string Identification;
		private string Name;
		private IfcExternalReferenceRelationship[] ExternalReferenceForResources;
	}
	

	/* IfcIdentifier : STRING (Java does not support structures, so usage of defined types are inline for efficiency.) */
	

	/* IfcInteger : INTEGER (Java does not support structures, so usage of defined types are inline for efficiency.) */
	

	/* IfcLabel : STRING (Java does not support structures, so usage of defined types are inline for efficiency.) */
	

	/* IfcText : STRING (Java does not support structures, so usage of defined types are inline for efficiency.) */
	

	public enum IfcDerivedUnitEnum
	{
		ANGULARVELOCITYUNIT,
		AREADENSITYUNIT,
		COMPOUNDPLANEANGLEUNIT,
		DYNAMICVISCOSITYUNIT,
		HEATFLUXDENSITYUNIT,
		INTEGERCOUNTRATEUNIT,
		ISOTHERMALMOISTURECAPACITYUNIT,
		KINEMATICVISCOSITYUNIT,
		LINEARVELOCITYUNIT,
		MASSDENSITYUNIT,
		MASSFLOWRATEUNIT,
		MOISTUREDIFFUSIVITYUNIT,
		MOLECULARWEIGHTUNIT,
		SPECIFICHEATCAPACITYUNIT,
		THERMALADMITTANCEUNIT,
		THERMALCONDUCTANCEUNIT,
		THERMALRESISTANCEUNIT,
		THERMALTRANSMITTANCEUNIT,
		VAPORPERMEABILITYUNIT,
		VOLUMETRICFLOWRATEUNIT,
		ROTATIONALFREQUENCYUNIT,
		TORQUEUNIT,
		MOMENTOFINERTIAUNIT,
		LINEARMOMENTUNIT,
		LINEARFORCEUNIT,
		PLANARFORCEUNIT,
		MODULUSOFELASTICITYUNIT,
		SHEARMODULUSUNIT,
		LINEARSTIFFNESSUNIT,
		ROTATIONALSTIFFNESSUNIT,
		MODULUSOFSUBGRADEREACTIONUNIT,
		ACCELERATIONUNIT,
		CURVATUREUNIT,
		HEATINGVALUEUNIT,
		IONCONCENTRATIONUNIT,
		LUMINOUSINTENSITYDISTRIBUTIONUNIT,
		MASSPERLENGTHUNIT,
		MODULUSOFLINEARSUBGRADEREACTIONUNIT,
		MODULUSOFROTATIONALSUBGRADEREACTIONUNIT,
		PHUNIT,
		ROTATIONALMASSUNIT,
		SECTIONAREAINTEGRALUNIT,
		SECTIONMODULUSUNIT,
		SOUNDPOWERLEVELUNIT,
		SOUNDPOWERUNIT,
		SOUNDPRESSURELEVELUNIT,
		SOUNDPRESSUREUNIT,
		TEMPERATUREGRADIENTUNIT,
		TEMPERATURERATEOFCHANGEUNIT,
		THERMALEXPANSIONCOEFFICIENTUNIT,
		WARPINGCONSTANTUNIT,
		WARPINGMOMENTUNIT,
		USERDEFINED,
	}
	

	public enum IfcUnitEnum
	{
		ABSORBEDDOSEUNIT,
		AMOUNTOFSUBSTANCEUNIT,
		AREAUNIT,
		DOSEEQUIVALENTUNIT,
		ELECTRICCAPACITANCEUNIT,
		ELECTRICCHARGEUNIT,
		ELECTRICCONDUCTANCEUNIT,
		ELECTRICCURRENTUNIT,
		ELECTRICRESISTANCEUNIT,
		ELECTRICVOLTAGEUNIT,
		ENERGYUNIT,
		FORCEUNIT,
		FREQUENCYUNIT,
		ILLUMINANCEUNIT,
		INDUCTANCEUNIT,
		LENGTHUNIT,
		LUMINOUSFLUXUNIT,
		LUMINOUSINTENSITYUNIT,
		MAGNETICFLUXDENSITYUNIT,
		MAGNETICFLUXUNIT,
		MASSUNIT,
		PLANEANGLEUNIT,
		POWERUNIT,
		PRESSUREUNIT,
		RADIOACTIVITYUNIT,
		SOLIDANGLEUNIT,
		THERMODYNAMICTEMPERATUREUNIT,
		TIMEUNIT,
		VOLUMEUNIT,
		USERDEFINED,
	}
	

	public interface IfcSimpleValue
	{
	}
	

	public interface IfcUnit
	{
	}
	

	public interface IfcValue
	{
	}
	

	public class IfcDerivedUnit extends IfcBase
	{
		private IfcDerivedUnitElement[] Elements;
		private IfcDerivedUnitEnum UnitType;
		private string UserDefinedType;
		private IfcDimensionalExponents Dimensions;
	}
	

	public class IfcDerivedUnitElement extends IfcBase
	{
		private IfcNamedUnit Unit;
		private INTEGER Exponent;
	}
	

	public class IfcDimensionalExponents extends IfcBase
	{
		private INTEGER LengthExponent;
		private INTEGER MassExponent;
		private INTEGER TimeExponent;
		private INTEGER ElectricCurrentExponent;
		private INTEGER ThermodynamicTemperatureExponent;
		private INTEGER AmountOfSubstanceExponent;
		private INTEGER LuminousIntensityExponent;
	}
	

	public class IfcMeasureWithUnit extends IfcBase
	{
		private IfcValue ValueComponent;
		private IfcUnit UnitComponent;
	}
	

	public class IfcMonetaryUnit extends IfcBase
	{
		private string Currency;
	}
	

	public class IfcNamedUnit extends IfcBase
	{
		private IfcDimensionalExponents Dimensions;
		private IfcUnitEnum UnitType;
	}
	

	public class IfcProperty extends IfcPropertyAbstraction
	{
		private string Name;
		private string Description;
		private IfcPropertySet[] PartOfPset;
		private IfcPropertyDependencyRelationship[] PropertyForDependance;
		private IfcPropertyDependencyRelationship[] PropertyDependsOn;
		private IfcComplexProperty[] PartOfComplex;
		private IfcResourceConstraintRelationship[] HasConstraints;
		private IfcResourceApprovalRelationship[] HasApprovals;
	}
	

	public class IfcPropertyAbstraction extends IfcBase
	{
		private IfcExternalReferenceRelationship[] HasExternalReferences;
	}
	

	/* IfcGloballyUniqueId : STRING (Java does not support structures, so usage of defined types are inline for efficiency.) */
	

	public class IfcTable extends IfcBase
	{
		private string Name;
		private IfcTableRow[] Rows;
		private IfcTableColumn[] Columns;
		private int NumberOfCellsInRow;
		private int NumberOfHeadings;
		private int NumberOfDataRows;
	}
	

