param(
  [string]$apppoolname = "apigatewaypool",
  [string]$apigatewayapp = "Default Web Site\stgclubcard\myaccountapi",
  [string]$enginepath = "D:\apigateway"
)

Import-Module "WebAdministration"

$apigatewayapp1 = "Default Web Site\stgclubcard"
$apigatewayapp2 = "Default Web Site\stgclubcard\myaccountapi"
$fullapigatewayapp1 = 'IIS:\Sites\' + $apigatewayapp1 + '\'
$fullapigatewayapp2 = 'IIS:\Sites\' + $apigatewayapp2 + '\'

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

if(!(Test-Path IIS:\Sites\$apigatewayapp1))
{
	echo "Creating Application '$fullapigatewayapp1'"
	New-Item $fullapigatewayapp1 -PhysicalPath %SystemDrive%\inetpub\wwwroot -Type Application
	# Set-ItemProperty $fullapigatewayapp1 -name applicationPool -value $apppoolname
	echo "Finished creating Application '$fullapigatewayapp1'."	
}
else
{
	echo "site is already present"
}

if(!(Test-Path IIS:\Sites\$apigatewayapp2))
{
	echo "Creating Application '$fullapigatewayapp2'"
	New-Item $fullapigatewayapp2 -PhysicalPath $enginepath -Type Application
	Set-ItemProperty $fullapigatewayapp2 -name applicationPool -value $apppoolname
	echo "Finished creating Application '$fullapigatewayapp2'."
}
else
{
	echo "site is already present"
}