set mstestPath="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\MStest.exe"
set Category="Sanity"

echo %dashes%
echo Starting the Test Run
@echo:

if exist D:\MCA_TESTFRAMEWORK\Results\Sanity\SanityResult.trx (
    del D:\MCA_TESTFRAMEWORK\Results\Sanity\SanityResult.trx
)
%mstestPath% /testcontainer:"D:\MCA_TESTFRAMEWORK\Tesco.MCA.TestFrameworks\Tesco.Framework.UITesting\bin\Debug\Tesco.Framework.UITesting.dll" /category:%Category% /resultsfile:D:\MCA_TESTFRAMEWORK\Results\Sanity\SanityResult.trx

