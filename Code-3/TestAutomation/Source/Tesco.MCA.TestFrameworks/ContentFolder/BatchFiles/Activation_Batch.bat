@echo off
COLOR 0A
TITLE Customer Service Journey - Regression


:: --------- SET The BATCH Variables ---------------

:: Set the Microsoft Visual Studio - MSTEST path
:: set MSVSPath=C:\Program Files (x86)

:: Set the Customer Service Journey module name - examples: PF/PSR/PQ etc.
set module=RG

:: -------------------------------------------------


set mstestPath="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\MStest.exe"


set dashes=-------------------------------------------------------------------------

::---------------------------------------------------------------------------------------
:: MULTI-CATEGORY TEST RUN
:: Category="Sanity" - Runs tests in the test category "Sanity"
:: Category="Sanity&Activation" - Runs tests that are in both test categories "Sanity" and "Activation"
:: Category="Sanity|Activation" - Runs tests that are in test category "Sanity" or "Activation". Tests that are in both test categories will also be run
:: Category="Sanity&!Activation" - Runs tests from the test category "Sanity" that are not in the test category "Activation". A test that is in both test category "Sanity" and "Activation" will not be run

:: Category="Sanity&!Activation&!Join" - Runs tests from the test category "Sanity" that are not in the test category "Activation" & "Join". A test that is in all the 3 test categories will not be run

set Category="P1"

set yy=%date:~-4%
set mm=%date:~-7,2%
set dd=%date:~-10,2%

set hour=%time:~0,2%
if "%hour:~0,1%" == " " set hour=0%hour:~1,1%
set min=%time:~3,2%
if "%min:~0,1%" == " " set min=0%min:~1,1%
set secs=%time:~6,2%
if "%secs:~0,1%" == " " set secs=0%secs:~1,1%

set MYDATE=%dd%-%mm%-%yy%_%hour%%min%%secs%
set file=D:\Automation\MCA_TESTFRAMEWORK\Tesco.MCA.TestFrameworks\ContentFolder\BatchFiles\CMDLog\cmdoutput.log

::---------------------------------------------------------------------------------------

echo %dashes%
echo Starting the Test Run
@echo:
%mstestPath% /testcontainer:"D:\Automation\MCA_TESTFRAMEWORK\Tesco.MCA.TestFrameworks\Tesco.Framework.UITesting\bin\Debug\Tesco.Framework.UITesting.dll" /category:%Category% /resultsfile:"D:\Automation\MCA_TESTFRAMEWORK\Tesco.MCA.TestFrameworks\TestResults\UITestResult_%module%_%MYDATE%.trx" >"%file%" 

:: --------------------- Call Report Generator ---------------------------------

echo %dashes%
@echo:
ECHO Generating HTML Report..
"D:\Automation\MCA_TESTFRAMEWORK\Tesco.MCA.TestFrameworks\Tesco.Framework.ReportGenerator\bin\Debug\Tesco.Framework.ReportGenerator.exe" 
echo %dashes%


