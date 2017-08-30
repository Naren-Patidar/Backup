@echo off
for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"
@echo:
set mstestPath="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\MStest.exe"
set "Root_Path=D:\Automation_MVC_Reskin\"
set "Build=Master"

set "Framework_Path=%Root_Path%Framework_UK_STG\"
set "Framework=%Framework_Path%Tesco.Framework.UITesting.IE.dll"
set "localfile=%Framework_Path%Local.testsettings"
set "Mongo_Report_EXE=%Root_Path%Services\MongoReportingExe.exe"

set "TRX_Path=%Root_Path%TRXs\UK\"
set "File=STG_P0Set1_IE_UK_Reskin_%YYYY%%MMM%%DD%_%HH%%Min%%Sec%.trx"
set "TRX_Report=%TRX_Path%%File%
set "Report_EXE=%Root_Path%Reporting\ReportGenerator.exe"
set "HTML_Report_Path=%Root_Path%Reports\UK_STG\P0Set1\"
set "HTML_File=UK_IE_STG_P0Set1_Reskin_%YYYY%%MMM%%DD%_%HH%%Min%%Sec%.html"

echo Starting the Test Run
@echo:
%mstestPath% /testcontainer:%Framework% /category:P0Set1 /resultsfile:%TRX_Report% /testsettings:%localfile%
echo Test Run Completed
%Report_EXE% %TRX_Report% %HTML_Report_Path% UK IE
echo HTML Report Generated
%Mongo_Report_EXE% %TRX_Report% %Build%
echo Mongo API Called

