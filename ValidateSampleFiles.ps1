$BaseFolder = "C:\projects\productdata"
$ValidatorExe="$BaseFolder\Tools\SchemaValidator\bin\Release\SchemaValidator.exe"
$SampleFiles = Get-ChildItem -Path $BaseFolder  -Recurse -Include *.ifc,*.ifcXML
$ExitCode=0
foreach ($file in $SampleFiles | Sort-Object -Property FullName) 
{
   Write-Host "++++++++++++++++++++++++++++++++++++++"
   Write-Host Validating $file.Name
   Write-Host "++++++++++++++++++++++++++++++++++++++"  
   & $ValidatorExe $file.FullName
   if ($LastExitCode -eq '1') {$ExitCode=1}
}
Exit $ExitCode