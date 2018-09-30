$BaseFolder = "C:\projects\productdata"
$ValidatorExe="$BaseFolder\Tools\SchemaValidator\bin\Release\SchemaValidator.exe"
$SampleFiles = Get-ChildItem -Path $BaseFolder  -Recurse -Include *.ifc,*.ifcXML
$ExitCode=0
foreach ($file in $SampleFiles | Sort-Object -Property FullName) 
{
   Write-Host "++++++++++++++++++++++++++++++++++++++"
   Write-Host Validating $file.Name
   & $ValidatorExe $file.FullName
   if ($LastExitCode -eq '0') 
   { 
     Write-Host "OK - $file.Name is valid" -ForegroundColor Green
   }
   else 
   {
     Write-Host "ERROR - $file.Name is not valid" -ForegroundColor Red
     $ExitCode=1
   }
}
Exit $ExitCode