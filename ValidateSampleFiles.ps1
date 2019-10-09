$BaseFolder = "C:\projects\productdata"
#$BaseFolder = "C:\Development\GitHub\buildingSMART\ProductData"
$ValidatorExe="$BaseFolder\Tools\SchemaValidator\bin\Release\net48\SchemaValidator.exe"
$SampleFiles = Get-ChildItem -Path "$BaseFolder\SampleFiles"  -Recurse -Include *.ifc,*.ifcXML
$ExitCode=0
foreach ($file in $SampleFiles | Sort-Object -Property FullName) 
{
   if     ((!$file.FullName.Contains("SampleFiles\HC (Italy)\Bricks Catalogue")) `
      -And (!$file.FullName.Contains("\SampleFiles\NisbetSample")) `
      -And (!$file.FullName.Contains("\SampleFiles\TriluxLightingProducts\SourceDataFromPimSystem\IFCModelFile")))   
   {
       Write-Host "++++++++++++++++++++++++++++++++++++++"
       $filename = $file.FullName.Replace($BaseFolder,"")
       Write-Host Validating $filename
       & $ValidatorExe $file.FullName
       if ($LastExitCode -eq '0') 
       { 
         Write-Host "OK: $filename is valid" -ForegroundColor Green
       }
       else 
       {
         Write-Host "ERROR: $filename is not valid" -ForegroundColor Red
         Write-Host "Please check the validation errors and correct them. Thanks for your contribution!" -ForegroundColor Red
         $ExitCode=1
       }
   }
}
Exit $ExitCode
