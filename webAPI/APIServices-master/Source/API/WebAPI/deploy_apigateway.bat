@echo off 
set vTargetDir="D:\apigateway"
DeployAPI.msi /qn TARGETDIR=%vTargetDir%
Powershell.exe -executionpolicy remotesigned -File  %CD%\deploy_apigateway.ps1 -apppoolname "apigatewaypool" -apigatewayapplication "Default Web Site\Clubcard\MyAccount\apigateway" -enginepath %vTargetDir%
echo "completed"