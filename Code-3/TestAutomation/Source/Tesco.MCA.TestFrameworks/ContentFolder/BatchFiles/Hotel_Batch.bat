
set mstestPath="D:\Program Files\Common7\IDE\MStest.exe"
set Category="Hotel & A2"

echo %dashes%
echo Starting the Test Run
@echo:
%mstestPath% /testcontainer:"E:\MCA_TESTFRAMEWORK\Tesco.MCA.TestFrameworks\Tesco.Framework.UITesting\bin\Debug\Tesco.Framework.UITesting.dll" /category:%Category% 