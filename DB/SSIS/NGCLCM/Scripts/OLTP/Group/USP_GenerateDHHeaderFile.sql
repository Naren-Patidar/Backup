

ALTER PROCEDURE [dbo].[USP_GenerateDHHeaderFile]  

            @Working_Path VARCHAR(200),@ExtrationType CHAR(1)  

AS  

BEGIN  

  

DECLARE @Optional1 VARCHAR(200)  

DECLARE @Optional2 VARCHAR(200)  

DECLARE @Optional3 VARCHAR(200)  

DECLARE @AllOptional VARCHAR(600)  

DECLARE @SQL VARCHAR(100)  

DECLARE @Header1 VARCHAR(max)  

DECLARE @Header2 VARCHAR(max)  

DECLARE @TXNHeader1 VARCHAR(max)  

IF @ExtrationType ='C'   

BEGIN  

  SELECT @Optional1 = PreferenceDescEnglish FROM Preference WHERE PreferenceID = 39 AND ISDeleted ='N'  

  SELECT @Optional2 = PreferenceDescEnglish FROM Preference WHERE PreferenceID = 40 AND ISDeleted ='N'  

  SELECT @Optional3 = PreferenceDescEnglish FROM Preference WHERE PreferenceID = 41 AND ISDeleted ='N'  

  

  IF (@Optional1 IS NOT NULL AND @Optional2 IS NOT NULL AND @Optional3 IS NOT NULL)  

  BEGIN  

   SET @AllOptional = @Optional1 + '|' +  @Optional2 + '|' + @Optional3  

  END  

  

  IF @Optional1 IS NOT NULL AND @Optional2 IS NOT NULL AND @AllOptional IS NULL  

  BEGIN  

   SET @AllOptional =@Optional1 + '|' + @Optional2  

  END  

  

  IF @Optional1 IS NOT NULL AND @Optional3 IS NOT NULL AND @AllOptional IS NULL  

  BEGIN  

   SET @AllOptional =@Optional1 + '|' + @Optional3  

  END  

  

  IF @Optional2 IS NOT NULL AND @Optional3 IS NOT NULL AND @AllOptional IS NULL  

  BEGIN  

   SET @AllOptional =@Optional2 + '|' + @Optional3  

  END   

  

  IF @Optional1 IS NOT NULL AND @AllOptional IS NULL  

  BEGIN  

   SET @AllOptional =@Optional1  

  END   

  

  IF @Optional2 IS NOT NULL AND @AllOptional IS NULL  

  BEGIN  

   SET @AllOptional =@Optional2  

  END   

  

  IF @Optional3 IS NOT NULL AND @AllOptional IS NULL  

  BEGIN  

   SET @AllOptional =@Optional3  

  END   

  

  SET @Header1= 'Customer Number|Primary Customer Number|Customer Mail Status Code|Customer Use Status Code|Income Band Code|Preferred Contact Type Code|Joined Date|Home Store|Preferred Store|Language Code|Allow promotions via mail flag|Allow promotions via phone flag|Allow group promotions flag|Allow third party promotions flag|Diabetic Flag|Vegetarian Flag|Teetotal Flag|Halal Flag|Celiac|Lactose'  

  SET @Header2 = 'Family Member 1 Date of Birth|Family Member 1 Age|Family Member 1 Gender Code|Family Member 2 Date of Birth|Family Member 2 Age|Family Member 2 Gender Code|Family Member 3 Date of Birth|Family Member 3 Age|Family Member 3 Gender Code|Family Member 4 Date of Birth|Family Member 4 Age|Family Member 4 Gender Code|Family Member 5 Date of Birth|Family Member 5 Age|Family Member 5 Gender Code|Family Member 6 Date of Birth|Family Member 6 Age|Family Member 6 Gender Code|Postal Code|Business Type Code|Race Descriptions|Card Account Type Code|Expact|TescoGroupMail|TescoGroupEmail|TescoGroupPhone|TescoGroupSMS|PartnerMail|PartnerEmail|PartnerPhone|PartnerSMS|ResearchMail|ResearchEmail|ResearchPhone|ResearchSMS|BCMMail|BCMEmail|BCMPhone|BCMSMS'  

  

  CREATE TABLE CustomerHeader (CName VARCHAR(MAX))  

  INSERT INTO CustomerHeader VALUES (@Header1 + '|' + CASE WHEN @AllOptional IS NULL THEN '' ELSE  @AllOptional + '|' END + @Header2)  

  SELECT @SQL = 'bcp '+ DB_Name() +'.dbo.CustomerHeader out ' + @Working_Path + 'cust_header.txt -c -S -T -P'  

  EXEC master..xp_cmdshell @SQL  

  DROP TABLE CustomerHeader  

END  

ELSE IF @ExtrationType ='T'   

BEGIN  

 SET @TXNHeader1= 'CustomerId|TescoStoreId|TransactionDateTime|TransactionType|SourcePOSID|SourceSystemTransactionID|CashierID'  

 CREATE TABLE DHTransactionHeader (TName VARCHAR(MAX))  

 INSERT INTO DHTransactionHeader VALUES(@TXNHeader1)  

 SELECT @SQL = 'bcp '+ DB_Name() +'.dbo.DHTransactionHeader out ' + @Working_Path + 'txn_header.txt -c -S -T -P'  

 EXEC master..xp_cmdshell @SQL  

 DROP TABLE DHTransactionHeader  

  

END  

END  
