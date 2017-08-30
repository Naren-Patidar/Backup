set mstestPath="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\MStest.exe"
set Category="P0"

echo %dashes%
echo Starting the Test Run
@echo:

if exist D:\MCA_TESTFRAMEWORK\Results\Regression\Regression_P0\Regression_P0.trx (
    del D:\MCA_TESTFRAMEWORK\Results\Regression\Regression_P0\Regression_P0.trx
)
%mstestPath% /testcontainer:"D:\MCA_TESTFRAMEWORK\Tesco.MCA.TestFrameworks\Tesco.Framework.UITesting\bin\Debug\Tesco.Framework.UITesting.dll" /category:%Category% /resultsfile:D:\MCA_TESTFRAMEWORK\Results\Regression\Regression_P0\Regression_P0.trx