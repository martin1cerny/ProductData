function ValidateXmlFile {
    param ([string]$xmlFile       = $(read-host "Please specify the path to the Xml file"))
	$xmlFile = resolve-path $xmlFile
    "======================================================================"
    "Validating '$xmlFile' using the schemas locations specified in it"
    "======================================================================"
    # The validating reader silently fails to catch any problems if the schema locations aren't set up properly
    # So attempt to get to the right place....
    pushd (Split-Path $xmlFile)

    try {
        $ns = @{xsi='http://www.w3.org/2001/XMLSchema-instance'}
	    # of course, if it's not well formed, it will barf here. Then we've also found a problem
        # use * in the XPath because not all files begin with Configuration any more. We'll still 
        # assume the location is on the root element 
        $locationAttr = Select-Xml -Path $xmlFile -Namespace $ns -XPath */@xsi:schemaLocation
        if ($locationAttr -eq $null) {throw "Can't find schema location attribute. This ain't gonna work"}
        Write-Host "Using this schema: "$locationAttr
        Write-Host " "

        $schemaLocation = resolve-path $locationAttr.Path
        Write-Host "schemaLocation:"$schemaLocation

        if ($schemaLocation -eq $null) 
        {
            throw "Can't find schema at location specified in Xml file. Bailing" 
        }

        $settings = new-object System.Xml.XmlReaderSettings
        $settings.ValidationType = [System.Xml.ValidationType]::Schema
        $settings.ValidationFlags = $settings.ValidationFlags `
                -bor [System.Xml.Schema.XmlSchemaValidationFlags]::ProcessSchemaLocation
        $handler = [System.Xml.Schema.ValidationEventHandler] {
            $args = $_ # entering new block so copy $_
            switch ($args.Severity) {
                Error {
                    # Exception is an XmlSchemaException
                    Write-Host "ERROR: line $($args.Exception.LineNumber)" -nonewline
                    Write-Host " position $($args.Exception.LinePosition)"
                    Write-Host $args.Message
                    break
                }
                Warning {
                    # So far, everything that has caused the handler to fire, has caused an Error...
                    # So this /might/ be unreachable
                    Write-Host "Warning:: " + $args.Message
                    break
                }
            }
        }
        $settings.add_ValidationEventHandler($handler)
        $reader = [System.Xml.XmlReader]::Create($xmlfile, $settings)
        while($reader.Read()){}
        $reader.Close()

    }
    catch {
        throw
    }
    finally {
        popd         
    }

}

#$BaseFolder = "C:\projects\productdata"
$BaseFolder = "C:\Development\GitHub\buildingSMART\ProductData"

#Download the schema files 
#wget https://standards.buildingsmart.org/IFC/RELEASE/IFC4/FINAL/XML/ifcXML4.xsd -OutFile ifcXML4.xsd
#wget https://standards.buildingsmart.org/IFC/RELEASE/IFC4/ADD1/XML/IFC4_ADD1.xsd -OutFile IFC4_ADD1.xsd
#wget https://standards.buildingsmart.org/IFC/RELEASE/IFC4/ADD2_TC1/XML/IFC4.xsd -OutFile IFC4_ADD2.xsd

 # Validate
ValidateXmlFile "$BaseFolder/SampleFiles/bSDD product room sample/bSDD product room sample.ifcXML" 
#ValidateXmlFile "$BaseFolder/SampleFiles/CAFM-Connect-ProductsSample/CAFM-Connect-ProductsSample.ifcXML" --schema IFC4_ADD2.xsd --noout
#ValidateXmlFile "$BaseFolder/SampleFiles/CE180_RED_06.17_Nextrema_G3_HFS_LLWC/CE180_RED_06.17_Nextrema_G3_HFS_LLWC.ifcXML" --schema IFC4_ADD2.xsd --noout
#ValidateXmlFile "$BaseFolder/SampleFiles/EN ISO 10077/EN ISO 10077.ifcXML" --schema IFC4_ADD2.xsd --noout
#ValidateXmlFile "$BaseFolder/SampleFiles/EULUMDAT_PDT_RH180508/EULUMDAT_PDT_RH180508.ifcXML" --schema IFC4_ADD2.xsd --noout
#ValidateXmlFile "$BaseFolder/SampleFiles/Fireray 50100RV Linear Smoke Detectors/Fireray 50100RV Linear Smoke Detectors.ifcXML" --schema IFC4_ADD2.xsd --noout
#ValidateXmlFile "$BaseFolder/SampleFiles/HC (Italy)/Sample PDT (ifcXML)/Brick_Sample_SchemaValidator.ifcXML" --schema IFC4_ADD2.xsd --noout  
#ValidateXmlFile "$BaseFolder/SampleFiles/HC (Italy)/Bricks Catalogue/bricks catalogue sample.ifcXML" --schema IFC4_ADD2.xsd --noout  
#ValidateXmlFile "$BaseFolder/SampleFiles/HC (Italy)/Bricks Catalogue/bricks catalogue sample including request.ifcXML" --schema IFC4_ADD2.xsd --noout  
#ValidateXmlFile "$BaseFolder/SampleFiles/HC (Italy)/Bricks Catalogue/ProductDataGenerated.ifcXML" --schema IFC4_ADD1.xsd --noout  
#ValidateXmlFile "$BaseFolder/SampleFiles/CatalogExample/catalogExample.ifcXML" --schema IFC4_ADD2.xsd --noout   
#ValidateXmlFile "$BaseFolder/SampleFiles/ConstraintExample/constraintExample.ifcXML" --schema IFC4_ADD1.xsd --noout   
#ValidateXmlFile "$BaseFolder/SampleFiles/NisbetSample/Example1.ifcXML" --schema IFC4_ADD1.xsd --noout    
#ValidateXmlFile "$BaseFolder/IFC4 Library/IkeaKitchenCabinets.ifcXML" --schema IFC4_ADD2.xsd --noout    