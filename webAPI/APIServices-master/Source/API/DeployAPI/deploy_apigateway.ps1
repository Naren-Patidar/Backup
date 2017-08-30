param(
  [string]$apppoolname = "apigateway",
  [string]$apigatewayapp = "Default Web Site\apigateway",
  [string]$enginepath = "D:\apigateway\"
)

Import-Module "WebAdministration"

$fullapigatewayapp = 'IIS:\Sites\' + $apigatewayapp + '\'

if(!(Test-Path IIS:\AppPools\$apppoolname))
{
	echo "Creating Application Pool $apppoolname."
	New-Item IIS:\AppPools\$apppoolname
	Set-ItemProperty IIS:\AppPools\$apppoolname managedRuntimeVersion v4.0
	echo "Finished creating Application Pool $apppoolname."
}
else
{
	echo "Application Pool $apppoolname already present in system."
}

if(!(Test-Path IIS:\Sites\$apigatewayapp))
{
	echo "Creating Application '$apigatewayapp'"
	New-Item $fullapigatewayapp -PhysicalPath $enginepath -Type Application
	Set-ItemProperty $fullapigatewayapp -name applicationPool -value $apppoolname
	echo "Finished creating Application '$apigatewayapp'."
}
else
{
	echo "site is already present"
}