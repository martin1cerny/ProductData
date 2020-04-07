<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/"> 
  <html>   
    
    <xsl:variable name="c12">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:variable name="c1" select="@Name"/>
        <xsl:if test="$c = 'idTP2'">
          <xsl:value-of select="$c1"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="c1bis">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:if test="$c = 'idTP2'">
          <xsl:value-of select="HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate/Enumerators/EnumerationValues/IfcLengthMeasure-wrapper"/>
        </xsl:if>        
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="c1tris">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:if test="$c = 'idTP2'">
          <xsl:value-of select="HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate/Enumerators/Unit/IfcSIUnit/@ref"/>
        </xsl:if>        
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="b1">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:variable name="c1" select="@Name"/>
        <xsl:if test="$c = 'idTP1'">
          <xsl:value-of select="$c1"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="b1bis">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:if test="$c = 'idTP1'">
          <xsl:value-of select="HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate/Enumerators/EnumerationValues/IfcLengthMeasure-wrapper"/>
        </xsl:if>        
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="b1tris">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:if test="$c = 'idTP1'">
          <xsl:value-of select="HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate/Enumerators/Unit/IfcSIUnit/@ref"/>
        </xsl:if>        
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="a1">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:variable name="c1" select="@Name"/>
        <xsl:if test="$c = 'idTP0'">
          <xsl:value-of select="$c1"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="a1bis">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:if test="$c = 'idTP0'">
          <xsl:value-of select="HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate/Enumerators/EnumerationValues/IfcLengthMeasure-wrapper"/>
        </xsl:if>        
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="a1tris">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:if test="$c = 'idTP0'">
          <xsl:value-of select="HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate/Enumerators/Unit/IfcSIUnit/@ref"/>
        </xsl:if>        
      </xsl:for-each>      
    </xsl:variable>   
    
    <xsl:variable name="norma_idSP3">
      <xsl:for-each select="ifcXML/IfcSimplePropertyTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:variable name="c1" select="@Name"/>
        <xsl:if test="$c = 'idPS3'">
          <xsl:value-of select="$c1"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="dimensione">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcComplexPropertyTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:variable name="c1" select="@Name"/>
        <xsl:if test="$c = 'idTP39'">
          <xsl:value-of select="$c1"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
   
    <xsl:variable name="tolleranze">
      <xsl:for-each select="ifcXML/IfcSimplePropertyTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:variable name="c1" select="@Name"/>
        <xsl:if test="$c = 'idPS0'">
          <xsl:value-of select="$c1"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="campo">
      <xsl:for-each select="ifcXML/IfcSimplePropertyTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:variable name="c1" select="@Name"/>
        <xsl:if test="$c = 'idPS1'">
          <xsl:value-of select="$c1"/>
        </xsl:if>
      </xsl:for-each>   
    </xsl:variable>
    
    <xsl:variable name="T2">
      <xsl:for-each select="ifcXML/IfcSimplePropertyTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:if test="$c = 'idPS0'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcLabel-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="R2">
      <xsl:for-each select="ifcXML/IfcSimplePropertyTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:if test="$c = 'idPS1'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcLabel-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
      
    <xsl:variable name="Categoria_resistenza_compressione">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="c" select="@id"/>
        <xsl:if test="$c = 'idTP5'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcLabel-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable> 
    
    <xsl:variable name="ValoreMedio_resistenza_compressione">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="h" select="@id"/>
        <xsl:if test="$h = 'idTP6'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcLengthMeasure-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable> 
    
    <xsl:variable name="DirezioneCarico_resistenza_compressione">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="i" select="@id"/>
        <xsl:if test="$i = 'idTP7'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcLabel-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable> 
    
    <xsl:variable name="Spostamento_umidità">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="l" select="@id"/>
        <xsl:if test="$l = 'idTP9'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcLengthMeasure-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable> 
    
    <xsl:variable name="Forza_Adesione">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="o" select="@id"/>
        <xsl:if test="$o = 'idTP10'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcPressureMeasure-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable> 
    
    <xsl:variable name="Percentuale_vuoti">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="o" select="@id"/>
        <xsl:if test="$o = 'idTP13'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcNumericMeasure-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable> 
    
    <xsl:variable name="Spessore_minimo_esterni">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="p" select="@id"/>
        <xsl:if test="$p = 'idTP14'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcLengthMeasure-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable> 
    
    <xsl:variable name="Spessore_minimo_interni">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="q" select="@id"/>
        <xsl:if test="$q = 'idTP15'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcLengthMeasure-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable> 
    
    <xsl:variable name="Assorbimento_acqua">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="r" select="@id"/>
        <xsl:if test="$r = 'idTP19'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcNumericMeasure-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable> 
    
    <xsl:variable name="Massa_volumica">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="s" select="@id"/>
        <xsl:if test="$s = 'idTP24'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcMassDensityMeasure-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="Massa_vol_lorda_secco">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="t" select="@id"/>
        <xsl:if test="$t = 'idTP25'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcMassDensityMeasure-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="Peso">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="u" select="@id"/>
        <xsl:if test="$u = 'idTP26'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcMassMeasure-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="Configurazione">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="v" select="@id"/>
        <xsl:if test="$v = 'idTP27'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcLabel-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="Conducibilità_termica">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="x" select="@id"/>
        <xsl:if test="$x = 'idTP21'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcThermalConductivityMeasure-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="Resistenza_Termica">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
        <xsl:variable name="y" select="@id"/>
        <xsl:if test="$y = 'idTP22'">
          <xsl:value-of select="Enumerators/EnumerationValues/IfcPressureMeasure-wrapper"/>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>
  
    
    <!--    unità di misura-->
    <xsl:variable name="Ref_MegaPascal">
      <xsl:for-each select="ifcXML/IfcSIUnit">  
        <xsl:variable name="unit1" select="@id"/>
        <xsl:variable name="unit11" select="@UnitType"/>
        <xsl:if test="$unit11 = 'pressureunit'">
          <xsl:value-of select="$unit1"/>
        </xsl:if>        
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="MegaPascal">
      <xsl:value-of select="'N/mm^2'"/>
    </xsl:variable>
    
    <xsl:variable name="Ref_mm">
      <xsl:for-each select="ifcXML/IfcSIUnit">  
        <xsl:variable name="unit2" select="@id"/>
        <xsl:variable name="unit21" select="@Prefix"/>
        <xsl:if test="$unit21 = 'milli'">
          <xsl:value-of select="$unit2"/>
        </xsl:if>        
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="mm">
      <xsl:value-of select="'mm'"/>
    </xsl:variable>
    
    <xsl:variable name="Ref_grammi">
      <xsl:for-each select="ifcXML/IfcSIUnit">  
        <xsl:variable name="unit3" select="@id"/>
        <xsl:variable name="unit31" select="@id"/>
        <xsl:if test="$unit31 = 'id04'">
          <xsl:value-of select="$unit3"/>
        </xsl:if>        
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="grammi">
      <xsl:value-of select="'g'"/>
    </xsl:variable>
    
    <!--<xsl:variable name="Ref_Spostamento">
      <xsl:for-each select="ifcXML/IfcDerivedUnit">  
        <xsl:variable name="n" select="@id"/>
        <xsl:variable name="n1" select="@UserDefinedType"/>
        <xsl:if test="$n1 = 'spostamento'">
          <xsl:value-of select="$n"/>
        </xsl:if>        
      </xsl:for-each>      
    </xsl:variable>
    
    <xsl:variable name="Spostamento">
      <xsl:value-of select="'mm/m'"/>
    </xsl:variable>-->
    
    
    <!--    unità di misura-->
    
    
    <!--<xsl:variable name="test">
      <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate">  
        <xsl:variable name="id" select="@id"/>
        <xsl:if test="$id = 'idTP56'">        
          <xsl:variable name="ref" select="HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate/@ref"/>
          <xsl:if test="$ref = 'idPS2'">            
            <xsl:value-of select="HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate[@id=$ref/@ref]/@Name"/>
          </xsl:if>
        </xsl:if>
      </xsl:for-each>      
    </xsl:variable>-->
    
    <table border="1" width="50%">
      <tr>                
        <th>      
          <xsl:value-of select="'CE'"/><br></br>
          <xsl:value-of select="'DoP n. xxxx'"/><br></br>
          <xsl:value-of select="ifcXML/IfcCivilElement/@Name"/><br></br>
          <xsl:value-of select="ifcXML/IfcCivilElement/@Description"/><br></br>
          <xsl:value-of select="$norma_idSP3"/>
        </th>        
        <th><img src="Logo_Harpaceas1.jpg" height="90" width="250"/></th>
      </tr>    
    </table>
    
    <table border="1" width="50%">
      <tr>
        <th><img src="DoP1.JPG" height="90" width="180"/></th>
        <xsl:for-each select="ifcXML">
          <th>      
            <xsl:value-of select="IfcCivilElement/@Name"/><br></br>
            <xsl:value-of select="IfcCivilElement/@Description"/>
          </th>
        </xsl:for-each>
      </tr>    
    </table>           
    
    <table border="1" width="50%"> 
      <th>
      <font align="center" size ="6"> Dichiarazione di prestazione n. xxxx </font>
      </th>
    </table>       
    
    <table border="1" width="50%">
      <th>
        <font align="left"> Norma armonizzata:  </font>
        <xsl:value-of select="$norma_idSP3"/>
      </th>
    </table>
    
    <table border="1" width="50%"> 
      <tr>
        <th rowspan="2" width="29%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate">  
            <xsl:variable name="c" select="@id"/>
            <xsl:variable name="c1" select="@Name"/>
            <xsl:if test="$c = 'idTP8'">
              <xsl:value-of select="$c1"/>
            </xsl:if>
          </xsl:for-each> 
        </th>
        <td>
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="c" select="@Name"/>
            <xsl:variable name="c1" select="@Description"/>
            <xsl:if test="$c = 'AENOR 0099'">
              <xsl:value-of select="$c1"/>
            </xsl:if>
          </xsl:for-each> 
        </td>
        <td width="21%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="c" select="@Name"/>
            <xsl:if test="$c = 'AENOR 0099'">
              <xsl:value-of select="$c"/>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
      <tr>
        <td>
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="c" select="@Name"/>
            <xsl:variable name="c1" select="@Description"/>
            <xsl:if test="$c = '0099-CPR-A73-0023'">
              <xsl:value-of select="$c1"/>
            </xsl:if>
          </xsl:for-each>
        </td>         
        <td>
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="c" select="@Name"/>
            <xsl:if test="$c = '0099-CPR-A73-0023'">
              <xsl:value-of select="$c"/>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>   
    </table>
    
    <table border="1" width="50%"> 
      <tr>
        <th> <xsl:value-of select="$dimensione"/> </th>      
        <th> <xsl:value-of select="$tolleranze"/> </th>
        <th> <xsl:value-of select="$campo"/> </th>
      </tr>
      <tr>
        <th> <xsl:value-of select="$a1"/> <xsl:value-of select="' ' "/> <xsl:value-of select="$a1bis"/> <xsl:value-of select="' ' "/> <xsl:value-of select="$a1tris"/></th>      
        <th> <xsl:value-of select="$T2"/> </th>
        <th> <xsl:value-of select="$R2"/> </th>
      </tr>
      <tr>
        <th> <xsl:value-of select="$b1"/> <xsl:value-of select="' ' "/> <xsl:value-of select="$b1bis"/> <xsl:value-of select="' ' "/> <xsl:value-of select="$b1tris"/></th>      
        <th> <xsl:value-of select="$T2"/> </th>
        <th> <xsl:value-of select="$R2"/> </th>
      </tr>
      <tr>
        <th> <xsl:value-of select="$c12"/> <xsl:value-of select="' ' "/> <xsl:value-of select="$c1bis"/> <xsl:value-of select="' ' "/> <xsl:value-of select="$c1tris"/> </th>      
        <th> <xsl:value-of select="$T2"/> </th>
        <th> <xsl:value-of select="$R2"/> </th>
      </tr>
    </table> 
    
    <table border="1" width="50%"> 
      <tr>
        <th rowspan="3" width="29%" align="left">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate">  
            <xsl:variable name="e" select="@GlobalId"/>
            <xsl:variable name="e1" select="@Name"/>
            <xsl:if test="$e = '0DnaFUGi5DQ8CFnScU0ji8'">
              <xsl:value-of select="$e1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td>
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="f" select="@id"/>
            <xsl:variable name="f1" select="@Name"/>
            <xsl:if test="$f = 'idTP5'">
              <xsl:value-of select="$f1"/>
            </xsl:if>
          </xsl:for-each>
        </td>
        <td width="30%">
          <xsl:value-of select="$Categoria_resistenza_compressione"/>
        </td>
      </tr>
      <tr>
        <td>
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="g" select="@id"/>
            <xsl:variable name="g1" select="@Name"/>
            <xsl:if test="$g = 'idTP6'">
              <xsl:value-of select="$g1"/>
            </xsl:if>
          </xsl:for-each>
        </td>
        <td>
          <xsl:value-of select="$ValoreMedio_resistenza_compressione"/> <xsl:value-of select="' ' "/> 
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="h" select="@id"/>
            <xsl:if test="$h = 'idTP6'">              
              <xsl:variable name="h2" select="Enumerators/Unit/IfcSIUnit/@ref"/>
              <xsl:if test="$h2 = $Ref_MegaPascal">
                <xsl:value-of select="$MegaPascal"/>
              </xsl:if>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
      <tr>
        <td>
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="i" select="@id"/>
            <xsl:variable name="i1" select="@Name"/>
            <xsl:if test="$i = 'idTP7'">
              <xsl:value-of select="$i1"/>
            </xsl:if>
          </xsl:for-each>
        </td>
        <td width="30%">
          <xsl:value-of select="$DirezioneCarico_resistenza_compressione"/>
        </td>
      </tr>
    </table>
    
    <table border="1" width="50%"> 
      <tr>
        <th width="29%" align="left">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
           <xsl:variable name="l" select="@id"/>
           <xsl:variable name="l1" select="@Name"/>
           <xsl:if test="$l = 'idTP9'">
            <xsl:value-of select="$l1"/>
           </xsl:if>
         </xsl:for-each>
        </th>      
        <td> 
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="m" select="@id"/>
            <xsl:variable name="m1" select="@Description"/>
            <xsl:if test="$m = 'idTP9'">
              <xsl:value-of select="$m1"/>
            </xsl:if>
          </xsl:for-each>
        </td>
        <td width="30%"> 
          <xsl:value-of select="$Spostamento_umidità"/> <xsl:value-of select="' ' "/>
          <!--<xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="n" select="@id"/>
            <xsl:if test="$n = 'idTP9'">              
              <xsl:variable name="n2" select="Enumerators/Unit/IfcDerivedUnit/@ref"/>
              <xsl:if test="$n2 = $Ref_Spostamento">
                <xsl:value-of select="$Spostamento"/>
              </xsl:if>
            </xsl:if>
          </xsl:for-each>-->
        </td>
      </tr>
    </table>
    
    <table border="1" width="50%"> 
      
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="n" select="@id"/>
            <xsl:variable name="n1" select="@Name"/>
            <xsl:if test="$n = 'idTP10'">
              <xsl:value-of select="$n1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:value-of select="$Forza_Adesione"/>
        </td>
      </tr>
      
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="o" select="@id"/>
            <xsl:variable name="o1" select="@Name"/>
            <xsl:if test="$o = 'idTP11'">
              <xsl:value-of select="$o1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="o1" select="@id"/>
            <xsl:variable name="o11" select="Enumerators/EnumerationValues/IfcLabel-wrapper"/>
            <xsl:if test="$o1 = 'idTP11'">
              <xsl:value-of select="$o11"/>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
      
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="p" select="@id"/>
            <xsl:variable name="p1" select="@Name"/>
            <xsl:if test="$p = 'idTP12'">
              <xsl:value-of select="$p1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="p1" select="@id"/>
            <xsl:variable name="p11" select="Enumerators/EnumerationValues/IfcLabel-wrapper"/>
            <xsl:if test="$p1 = 'idTP12'">
              <xsl:value-of select="$p11"/>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
      
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="q" select="@id"/>
            <xsl:variable name="q1" select="@Name"/>
            <xsl:if test="$q = 'idTP13'">
              <xsl:value-of select="$q1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:value-of select="$Percentuale_vuoti"/>
        </td>
      </tr>
     
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="r" select="@id"/>
            <xsl:variable name="r1" select="@Name"/>
            <xsl:if test="$r = 'idTP14'">
              <xsl:value-of select="$r1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:value-of select="$Spessore_minimo_esterni"/> <xsl:value-of select="' ' "/>
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="rr" select="@id"/>
            <xsl:if test="$rr = 'idTP14'">              
              <xsl:variable name="rr2" select="Enumerators/Unit/IfcSIUnit/@ref"/>
              <xsl:if test="$rr2 = $Ref_mm">
                <xsl:value-of select="$mm"/>
              </xsl:if>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
     
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="s" select="@id"/>
            <xsl:variable name="s1" select="@Name"/>
            <xsl:if test="$s = 'idTP15'">
              <xsl:value-of select="$s1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:value-of select="$Spessore_minimo_interni"/> <xsl:value-of select="' ' "/>
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="ss" select="@id"/>
            <xsl:if test="$ss = 'idTP15'">              
              <xsl:variable name="ss2" select="Enumerators/Unit/IfcSIUnit/@ref"/>
              <xsl:if test="$ss2 = $Ref_mm">
                <xsl:value-of select="$mm"/>
              </xsl:if>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
     
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="t" select="@id"/>
            <xsl:variable name="t1" select="@Name"/>
            <xsl:if test="$t = 'idTP16'">
              <xsl:value-of select="$t1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="t1" select="@id"/>
            <xsl:variable name="t11" select="Enumerators/EnumerationValues/IfcLabel-wrapper"/>
            <xsl:if test="$t1 = 'idTP16'">
              <xsl:value-of select="$t11"/>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
     
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="u" select="@id"/>
            <xsl:variable name="u1" select="@Name"/>
            <xsl:if test="$u = 'idTP17'">
              <xsl:value-of select="$u1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="u1" select="@id"/>
            <xsl:variable name="u11" select="Enumerators/EnumerationValues/IfcLabel-wrapper"/>
            <xsl:if test="$u1 = 'idTP17'">
              <xsl:value-of select="$u11"/>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
      
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="v" select="@id"/>
            <xsl:variable name="v1" select="@Name"/>
            <xsl:if test="$v = 'idTP18'">
              <xsl:value-of select="$v1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="v1" select="@id"/>
            <xsl:variable name="v11" select="Enumerators/EnumerationValues/IfcLabel-wrapper"/>
            <xsl:if test="$v1 = 'idTP18'">
              <xsl:value-of select="$v11"/>
            </xsl:if>
          </xsl:for-each>
          
          <xsl:value-of select="' ' "/>
          
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="v" select="@id"/>
            <xsl:variable name="v1" select="@Description"/>
            <xsl:if test="$v = 'idTP18'">
              <xsl:value-of select="$v1"/>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
     
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="x" select="@id"/>
            <xsl:variable name="x1" select="@Name"/>
            <xsl:if test="$x = 'idTP19'">
              <xsl:value-of select="$x1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:value-of select="$Assorbimento_acqua"/>
        </td>
      </tr>
     
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="y" select="@id"/>
            <xsl:variable name="y1" select="@Name"/>
            <xsl:if test="$y = 'idTP20'">
              <xsl:value-of select="$y1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="y1" select="@id"/>
            <xsl:variable name="y11" select="Enumerators/EnumerationValues/IfcLabel-wrapper"/>
            <xsl:if test="$y1 = 'idTP20'">
              <xsl:value-of select="$y11"/>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
    </table> 
    
    <table border="1" width="50%"> 
      <tr>
        <th rowspan="4" width="29%" align="left">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate">  
            <xsl:variable name="ab" select="@id"/>
            <xsl:variable name="ab1" select="@Name"/>
            <xsl:if test="$ab = 'idTP99'">
              <xsl:value-of select="$ab1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td>
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="ac" select="@id"/>
            <xsl:variable name="ac1" select="@Name"/>
            <xsl:if test="$ac = 'idTP24'">
              <xsl:value-of select="$ac1"/>
            </xsl:if>
          </xsl:for-each>
        </td>
        <td width="30%">
          <xsl:value-of select="$Massa_volumica"/>
        </td>
      </tr>
      <tr>
        <td>
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="ad" select="@id"/>
            <xsl:variable name="ad1" select="@Name"/>
            <xsl:if test="$ad = 'idTP25'">
              <xsl:value-of select="$ad1"/>
            </xsl:if>
          </xsl:for-each>
        </td>
        <td>
          <xsl:value-of select="$Massa_vol_lorda_secco"/>
        </td>
      </tr>
      <tr>
        <td>
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="ae" select="@id"/>
            <xsl:variable name="ae1" select="@Name"/>
            <xsl:if test="$ae = 'idTP26'">
              <xsl:value-of select="$ae1"/>
            </xsl:if>
          </xsl:for-each>
        </td>
        <td width="30%">
          <xsl:value-of select="$Peso"/> <xsl:value-of select="' ' "/>
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="aee" select="@id"/>
            <xsl:if test="$aee = 'idTP26'">              
              <xsl:variable name="aee2" select="Enumerators/Unit/IfcSIUnit/@ref"/>
              <xsl:if test="$aee2 = $Ref_grammi">
                <xsl:value-of select="$grammi"/>
              </xsl:if>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
      <tr>
        <td>
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="af" select="@id"/>
            <xsl:variable name="af1" select="@Name"/>
            <xsl:if test="$af = 'idTP27'">
              <xsl:value-of select="$af1"/>
            </xsl:if>
          </xsl:for-each>
        </td>
        <td>
          <xsl:value-of select="$Configurazione"/>
        </td>
      </tr>
    </table>
    
    <table border="1" width="50%"> 
      
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="ag" select="@id"/>
            <xsl:variable name="ag1" select="@Name"/>
            <xsl:if test="$ag = 'idTP21'">
              <xsl:value-of select="$ag1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:value-of select="$Conducibilità_termica"/>
        </td>
      </tr>
      
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="ah" select="@id"/>
            <xsl:variable name="ah1" select="@Name"/>
            <xsl:if test="$ah = 'idTP22'">
              <xsl:value-of select="$ah1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:value-of select="$Resistenza_Termica"/>
        </td>
      </tr>
      
      <tr align="left">
        <th width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="ai" select="@id"/>
            <xsl:variable name="ai1" select="@Name"/>
            <xsl:if test="$ai = 'idTP23'">
              <xsl:value-of select="$ai1"/>
            </xsl:if>
          </xsl:for-each>
        </th>
        <td width="50%">
          <xsl:for-each select="ifcXML/IfcProjectLibrary/Declares/IfcRelDeclares/RelatedDefinitions/IfcPropertySetTemplate/HasPropertyTemplates/IfcComplexPropertyTemplate/HasPropertyTemplates/IfcSimplePropertyTemplate">  
            <xsl:variable name="ai1" select="@id"/>
            <xsl:variable name="ai11" select="Enumerators/EnumerationValues/IfcLabel-wrapper"/>
            <xsl:if test="$ai1 = 'idTP23'">
              <xsl:value-of select="$ai11"/>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
    </table>
    
  </html>
  
</xsl:template> 
</xsl:stylesheet>


