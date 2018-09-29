$ValidatorExe="Tools\SchemaValidator\bin\Release\SchemaValidator.exe"
$SampleFiles = Get-ChildItem  -Recurse -Include *.ifc,*.ifcXML
foreach ($file in $SampleFiles | Sort-Object -Property FullName) 
{
   Write-Host "++++++++++++++++++++++++++++++++++++++"
   Write-Host Validating $file.Name
   Write-Host "++++++++++++++++++++++++++++++++++++++"  
   & $ValidatorExe $file.FullName
}