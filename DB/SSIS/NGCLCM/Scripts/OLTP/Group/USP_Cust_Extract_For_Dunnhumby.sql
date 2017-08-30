/****** Object:  StoredProcedure [dbo].[USP_Cust_Extract_For_Dunnhumby]    Script Date: 11/19/2012 14:53:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--==============================================================================
--<Application Version number="3.1.1" day="07" month="07" year="2009">
--    <developer>Sakthi Ganesh</developer>
--    <SP version> 0.1 </SP Version>
--    <Modified by>Netra KAlakannavar for Req.ID-033b Optional dieatry preferences.</ modified by >
-- </ Application Version>
--============================================================================

ALTER PROCEDURE [dbo].[USP_Cust_Extract_For_Dunnhumby]
					  @Working_Path VARCHAR(200),@Export_Path VARCHAR(200),@From_Date DATETIME = NULL,@To_Date DATETIME = NULL
AS

SET NOCOUNT ON
DECLARE @cmd varchar(1000)
DECLARE @Path varchar(1000)
DECLARE @temppath varchar(1000)
DECLARE @dbname varchar(50)
DECLARE @Last_Run DATETIME
DECLARE @Current_Run DATETIME
DECLARE @NoOfRuns bigint
DECLARE @IsDeleted CHAR(1)
DECLARE @Action CHAR(2)
SET @dbname = DB_NAME()

/* The SP is used to create Header file in the specified Woking Path parameter based on the optional dietary preference */
	EXEC USP_GenerateDHHeaderFile @Working_Path,'C' --- 'C' is standard for Customer Extraction
/************************************************************************************************************************/

/********************************************************************************************************/
	-- Get Country code and Generate Output file
/*******************************************************************************************************/
       /* DECLARE @chvDomainName NVARCHAR(100), 
        EXEC master.dbo.xp_regread 'HKEY_LOCAL_MACHINE','SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon',N'CachePrimaryDomain',@chvDomainName OUTPUT*/
        DECLARE @CountryCode varchar(20)
		SET @CountryCode = SUBSTRING(@@ServerName,1,2)
		SET @IsDeleted ='N'

		SET @temppath = @Working_Path+'NGC_' + @CountryCode + '_CUST_' +convert(varchar,year(getdate()))+REPLICATE('0',2 - DATALENGTH(convert(varchar,month( getdate() ) ) )) + convert(varchar,month( getdate() ) )+REPLICATE('0',2 - DATALENGTH(convert(varchar,day( getdate() ) ) )) + convert(varchar,day( getdate() ) )+'temp.txt'
		SET @Path = @Export_Path+'NGC_'+ @CountryCode + '_CUST_' +convert(varchar,year(getdate()))+REPLICATE('0',2 - DATALENGTH(convert(varchar,month( getdate() ) ) )) + convert(varchar,month( getdate() ) )+REPLICATE('0',2 - DATALENGTH(convert(varchar,day( getdate() ) ) )) + convert(varchar,day( getdate() ) )+'.txt'
		
		IF ((@From_Date IS NULL) AND (@To_Date IS NOT NULL)) 
		BEGIN SET @Action = '02' GOTO ExitScript END
/********************************************************************************************************/

/*************** Optional Dietary Preferences Logic ***********************************************************/
DECLARE @Optionalflag VARCHAR(200)
DECLARE @OptionalValue VARCHAR(200)
DECLARE @Optional1 VARCHAR(200)
DECLARE @Optional2 VARCHAR(200)
DECLARE @Optional3 VARCHAR(200)
DECLARE @AllOptColumn VARCHAR(600)
DECLARE @AllOptValue VARCHAR(600)
DECLARE @AllOptionalPreferences VARCHAR(600)
DECLARE @SQLResult VARCHAR(MAX)
DECLARE @SQLStatement VARCHAR(MAX)
DECLARE @SQLUpdate VARCHAR(MAX)
DECLARE @SQLTempCust VARCHAR(MAX)

SELECT @Optional1 = PreferenceDescEnglish FROM Preference WHERE PreferenceID = 39 AND ISDeleted ='N'
SELECT @Optional2 = PreferenceDescEnglish FROM Preference WHERE PreferenceID = 40 AND ISDeleted ='N'
SELECT @Optional3 = PreferenceDescEnglish FROM Preference WHERE PreferenceID = 41 AND ISDeleted ='N'

IF (@Optional1 IS NOT NULL AND @Optional2 IS NOT NULL AND @Optional3 IS NOT NULL)
BEGIN
	SET @AllOptColumn = '[39] AS PID39,''0'',[40] AS PID40,''0'',[41] AS PID41,''0'''
	SET @AllOptValue = '[39],[40],[41]'
    SET @AllOptionalPreferences = 'Optional1 INT, Optional1Flag Char(1), Optional2 INT, Optional2Flag Char(1), Optional3 INT, Optional3Flag Char(1)'
    SET @Optionalflag = ' Optional1Flag Char(1), Optional2Flag Char(1), Optional3Flag Char(1)'
    SET @OptionalValue = ' Optional1Flag , Optional2Flag , Optional3Flag '	
END

IF @Optional1 IS NOT NULL AND @Optional2 IS NOT NULL AND @AllOptColumn IS NULL
BEGIN
	SET @AllOptColumn = '[39] AS PID39,''0'',[40] AS PID40,''0'''
    SET @AllOptValue = '[39],[40]'
    SET @AllOptionalPreferences = 'Optional1 INT, Optional1Flag Char(1), Optional2 INT, Optional2Flag Char(1)'
    SET @Optionalflag = ' Optional1Flag Char(1), Optional2Flag Char(1)'
    SET @OptionalValue = ' Optional1Flag , Optional2Flag '	
END

IF @Optional1 IS NOT NULL AND @Optional3 IS NOT NULL AND @AllOptColumn IS NULL
BEGIN
	SET @AllOptColumn = '[39] AS PID39,''0'',[41] AS PID41,''0'''
	SET @AllOptValue = '[39],[41]'
    SET @AllOptionalPreferences = 'Optional1 INT, Optional1Flag Char(1), Optional3 INT, Optional3Flag Char(1)'
    SET @Optionalflag = ' Optional1Flag Char(1), Optional3Flag Char(1)'
    SET @OptionalValue = ' Optional1Flag  , Optional3Flag '
END

IF @Optional2 IS NOT NULL AND @Optional3 IS NOT NULL AND @AllOptColumn IS NULL
BEGIN
	SET @AllOptColumn = '[40] AS PID40,''0'',[41] AS PID41,''0'''
	SET @AllOptValue = '[40],[41]'
    SET @AllOptionalPreferences = ' Optional2 INT, Optional2Flag Char(1), Optional3 INT, Optional3Flag Char(1)'
    SET @Optionalflag = 'Optional2Flag Char(1), Optional3Flag Char(1)'
    SET @OptionalValue = ' Optional2Flag , Optional3Flag '	
END 

IF @Optional1 IS NOT NULL AND @AllOptColumn IS NULL
BEGIN
	SET @AllOptColumn = '[39] AS PID39,''0'''
	SET @AllOptValue = '[39]'
    SET @AllOptionalPreferences = 'Optional1 INT, Optional1Flag Char(1)'
    SET @Optionalflag = ' Optional1Flag Char(1)'
    SET @OptionalValue = ' Optional1Flag '	
END 

IF @Optional2 IS NOT NULL AND @AllOptColumn IS NULL
BEGIN
	SET @AllOptColumn = '[40] AS PID40,''0'''
	SET @AllOptValue = '[40]'
    SET @AllOptionalPreferences = 'Optional2 INT, Optional2Flag Char(1)'
    SET @Optionalflag = ' Optional2Flag Char(1)'
    SET @OptionalValue = '  Optional2Flag  '
END 

IF @Optional3 IS NOT NULL AND @AllOptColumn IS NULL
BEGIN
	SET @AllOptColumn = '[41] AS PID41,''0'''
	SET @AllOptValue = '[41]'
    SET @AllOptionalPreferences = ' Optional3 INT, Optional3Flag Char(1)'
    SET @Optionalflag = ' Optional3Flag Char(1)' 
    SET @OptionalValue = ' Optional3Flag '
END 

---------------------------------------------------------------------------------------------------------------------------------------
-- Start : Create Temp cust table -----------------------------------------------------------------------------------------------------
		IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[TempCust]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
		BEGIN
			DROP TABLE [dbo].[TempCust]
		END 

			SET @SQLTempCust ='CREATE TABLE [TempCust] ( '
			SET @SQLTempCust = @SQLTempCust + 'CustomerID BIGINT NOT NULL,'
			SET @SQLTempCust = @SQLTempCust + 'PrimaryCustomerID BIGINT NULL,'
			SET @SQLTempCust = @SQLTempCust + 'CustomerMailStatus TINYINT  NULL,'
			SET @SQLTempCust = @SQLTempCust + 'CustomerUseStatusID TINYINT NULL,'
			SET @SQLTempCust = @SQLTempCust + 'IncomeBandID TINYINT NULL,'
			SET @SQLTempCust = @SQLTempCust + 'PreferredContactTypeCode SMALLINT  NULL,'
			SET @SQLTempCust = @SQLTempCust + 'JoinedDate DATETIME NULL,'
			SET @SQLTempCust = @SQLTempCust + 'JoinedStoreID INT NULL,	'				
			SET @SQLTempCust = @SQLTempCust + 'PreferredStoreID INT NULL,'
			SET @SQLTempCust = @SQLTempCust + 'ISOLanguageCode CHAR(3) NULL,'
					
			SET @SQLTempCust = @SQLTempCust + 'AllowpromotionsViaMailFlag VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'AllowpromotionsviaPhoneFlag VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'AllowGroupPromotionsFlag VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'AllowThirdpartyPromotionsFlag VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'DiabeticFlag VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'VegetarianFlag VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'TeetotalFlag VARCHAR(2) NULL,'
			--SET @SQLTempCust = @SQLTempCust + 'KosherFlag VARCHAR(2)NULL,'
			SET @SQLTempCust = @SQLTempCust + 'HalalFlag VARCHAR(2)NULL,'
			SET @SQLTempCust = @SQLTempCust + 'CeliacFlag VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'LactoseFlag VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + CASE WHEN @OptionalFlag IS NULL THEN '' ELSE  @OptionalFlag + ',' END

			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_1_DOB DATETIME NULL,'
			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_1_Age TINYINT NULL,'
			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_1_GenderCode CHAR(1) NULL,'

			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_2_DOB DATETIME NULL,'
			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_2_Age TINYINT NULL,'
			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_2_GenderCode CHAR(1) NULL,'

			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_3_DOB DATETIME NULL,'
			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_3_Age TINYINT NULL,'
			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_3_GenderCode CHAR(1) NULL,'

			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_4_DOB DATETIME NULL,'
			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_4_Age TINYINT NULL,'
			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_4_GenderCode CHAR(1) NULL,'

			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_5_DOB DATETIME NULL,'
			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_5_Age TINYINT NULL,'
			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_5_GenderCode CHAR(1) NULL,'

			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_6_DOB DATETIME NULL,'
			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_6_Age TINYINT NULL,'
			SET @SQLTempCust = @SQLTempCust + 'FamilyMember_6_GenderCode CHAR(1) NULL,'

			SET @SQLTempCust = @SQLTempCust + 'PostalCode nvarchar(10) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'BusinessTypeCode TINYINT NULL,'
			SET @SQLTempCust = @SQLTempCust + 'RaceDescriptions VARCHAR(100) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'CardaccountTypeCode TINYINT NULL,'
			SET @SQLTempCust = @SQLTempCust + 'ExpatFlag VARCHAR(2)NULL,'
			SET @SQLTempCust = @SQLTempCust + 'TescoGRPMail VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'TescoGRPEmail VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'TescoGRPPhone VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'TescoGRPSMS VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'PartnerMail VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'PartnerEmail VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'PartnerPhone VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'PartnerSMS VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'ResearchMail VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'ResearchEmail VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'ResearchPhone VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'ResearchSMS VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'BCMMail VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'BCMEMail VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'BCMPhone VARCHAR(2) NULL,'
			SET @SQLTempCust = @SQLTempCust + 'BCMSMS VARCHAR(2) NULL	)'
			
			EXEC (@SQLTempCust)			

-- End : Create Temp cust table ----------------------------------------------------------------------------------------------------
/* *********************************************************************************************************************************/
			SELECT TOP 1 @Last_Run=PrevRun FROM DunnHumby_Cust ORDER BY PrevRun DESC
			
			IF(@Last_Run IS NULL)
				BEGIN
					SELECT @Last_Run=MIN(AmendDateTime) FROM Customer
				END
			SELECT @Current_Run=GETDATE()
			SELECT @NoOfRuns=COUNT(1) FROM DunnHumby_Cust			
/***********************************************************************************************************************************/
/* Start: The below block is used to find Preference of Customer *******************************************************************/

DECLARE @SQLPreference VARCHAR(500)
CREATE TABLE #Result (CustomerID BIGINT, PreferenceID INT,Flag Char(1))

-- Master Query for inserting #Result table --------------------------------------------------------------------------------
SET @SQLPreference = 'INSERT INTO #Result Select C.CustomerID,CP.PreferenceID,''1'''
SET @SQLPreference = @SQLPreference + ' FROM Customer C INNER JOIN CustomerPreference CP ON C.CustomerID = CP.CustomerID AND PreferenceOptStatusID = 1'
----------------------------------------------------------------------------------------------------------------------------
-- Where Clause for the #Result table 
IF(@NoOfRuns<>0)
	BEGIN	
-- Start :Action 99
	IF((@From_Date IS NULL) AND (@To_Date IS NULL)) 
		BEGIN			
			SET @SQLPreference = @SQLPreference + ' WHERE C.AmendDateTime > ' +'''' + CONVERT(VARCHAR,@Last_Run) + ''''+ ' AND C.AmendDateTime < ' + '''' + CONVERT(VARCHAR,@Current_Run) + ''' AND C.IsDeleted= ''' + @IsDeleted	+ ''''
			EXEC(@SQLPreference)
		END	
-- End Action 99
-- Start: Action 1
	ELSE IF((@From_Date IS NOT NULL) AND (@To_Date IS NULL))
	BEGIN			
		SET @SQLPreference = @SQLPreference + ' WHERE C.AmendDateTime > ' +'''' + CONVERT(VARCHAR,@From_Date) + ''''+ ' AND C.AmendDateTime < ' + '''' + CONVERT(VARCHAR,@Current_Run) + ''' AND C.IsDeleted= ''' + @IsDeleted	+ ''''
		EXEC(@SQLPreference)
		SET @Last_Run = @From_Date
	END
-- End Action 1
--Start Action 3
	ELSE IF((@From_Date IS NOT NULL) AND (@To_Date IS NOT NULL))
	BEGIN		
		SET @SQLPreference = @SQLPreference + ' WHERE C.AmendDateTime > ' +'''' + CONVERT(VARCHAR,@From_Date) + ''''+ ' AND C.AmendDateTime < ' + '''' + CONVERT(VARCHAR,@To_Date) + '''  AND C.IsDeleted= ''' + @IsDeleted	+ ''''
		EXEC(@SQLPreference)
		PRINT @SQLPreference
		SET @Last_Run = @From_Date
	END
--End Action 3
	END
ELSE
	BEGIN	
		SET @SQLPreference = @SQLPreference + ' WHERE C.IsDeleted= ''' + @IsDeleted	+ ''' ORDER BY C.CustomerID'
		EXEC(@SQLPreference)		
	END


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[DHCustPreference]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) 
	DROP TABLE [dbo].[DHCustPreference]

SET @SQLStatement = 'CREATE TABLE DHCustPreference (CustomerID BIGINT,Diabetic INT,DiabeticFlag Char(1) null, Vegetarian INT,VegetarianFlag Char(1) null,Teetotal INT, TeetotalFlag Char(1),Halal INT,HalalFlag Char(1), Expat INT,ExpatFlag Char(1),  Celiac INT,CeliacFlag Char(1), Lactose INT,LactoseFlag Char(1)'
SET @SQLStatement = @SQLStatement + ',' + CASE WHEN @AllOptionalPreferences IS NULL THEN '' ELSE @AllOptionalPreferences + ',' END +  'TescoGRPMail INT,TescoGRPMailFlag Char(1), TescoGRPEmail INT,TescoGRPEmailFlag Char(1), TescoGRPPhone INT,TescoGRPPhoneFlag Char(1), TescoGRPSMS INT,TescoGRPSMSFlag Char(1)'
SET @SQLStatement = @SQLStatement + ',' + ' PartnerMail INT,PartnerMailFlag Char(1), PartnerEmail INT,PartnerEmailFlag Char(1),PartnerPhone INT,PartnerPhoneFlag Char(1),PartnerSMS INT,PartnerSMSFlag Char(1),ResearchMail INT,ResearchMailFlag Char(1)'
SET @SQLStatement = @SQLStatement + ',' + 'ResearchEmail INT,ResearchEmailFlag Char(1), ResearchPhone INT,ResearchPhoneFlag Char(1),  ResearchSMS INT,ResearchSMSFlag Char(1),BCMMail INT,BCMMailFlag Char(1),BCMEMail INT,BCMEMailFlag Char(1),BCMPhone INT,BCMPhoneFlag Char(1),BCMSMS INT,BCMSMSFlag Char(1)'
SET @SQLStatement = @SQLStatement + ',' + ' AllowpromotionsViaMail INT,AllowpromotionsViaMailFlag Char(1), AllowpromotionsviaPhone INT,AllowpromotionsviaPhoneFlag Char(1)'
SET @SQLStatement = @SQLStatement + ',' + ' AllowgroupPromotions INT,AllowgroupPromotionsFlag Char(1), AllowThirdpartyPromotions INT,AllowThirdpartyPromotionsFlag Char(1),PreferredContactType INT )'

EXEC(@SQLStatement)

SET @SQLResult = 'INSERT INTO DHCustPreference '
SET @SQLResult = @SQLResult + ' SELECT '
SET @SQLResult = @SQLResult + ' CustomerID,'
SET @SQLResult = @SQLResult + ' [20] AS PID20,''0'',[1] AS PID1,''0'',[5] AS PID5,''0'',[3] AS PID3,''0'',[26] AS PID26,''0'',[24] AS PID24,''0'',[25] AS PID25,''0'','
SET @SQLResult = @SQLResult + CASE WHEN @AllOptColumn IS NULL THEN '' ELSE @AllOptColumn + ',' END --'[39] AS PID39,'0',[40] AS PID40,'0',[41] AS PID41,'0',
SET @SQLResult = @SQLResult + ' [27] AS PID27,''0'',[28] AS PID28,''0'',[29] AS PID29,''0'',[30] AS PID30,''0'',[31] AS PID31,''0'',[32] AS PID32,''0'',[33] AS PID33,''0'',[34] AS PID34,''0'','
SET @SQLResult = @SQLResult + ' [35] AS PID35,''0'',[36] AS PID36,''0'',[37] AS PID37,''0'',[38] AS PID38,''0'',[50] AS PID38,''0'',[51] AS PID38,''0'',[52] AS PID38,''0'',[53] AS PID38,''0'',[17] AS PID17,''0'',[16] AS PID16,''0'',[23] AS PID23,''0'',[18] AS PID18,''0'',''0'''
SET @SQLResult = @SQLResult + ' FROM (Select CustomerID,PreferenceID FROM #Result) As p'
SET @SQLResult = @SQLResult + ' PIVOT (SUM(PreferenceID) FOR PreferenceID in([20],[1],[5],[4],[3],[17],[16],[23],[18],[26],[24],[25],' + CASE WHEN @AllOptValue IS NULL THEN '' ELSE @AllOptValue + ',' END+ '[27],[28],[29],[30],[31],[32],[33],[34],[35],[36],[37],[38],[50],[51],[52],[53])) AS pvt'

EXEC(@SQLResult)
print @SQLResult
DECLARE @CustID BIGINT
DECLARE @PreferenceID INT
DECLARE @Flag Char(1)
DECLARE @PreferredContactType TINYINT

DECLARE CurPreference CURSOR FOR SELECT CustomerID,PreferenceID,Flag FROM #Result
OPEN CurPreference
FETCH CurPreference INTO @CustID,@PreferenceID,@Flag
WHILE @@Fetch_Status = 0
   BEGIN	
			UPDATE DHCustPreference SET PreferredContactType = @PreferenceID WHERE CustomerID =@CustID AND @PreferenceID IN(6,43,44,45,46,47)
			UPDATE DHCustPreference SET DiabeticFlag = @Flag WHERE CustomerID =@CustID AND @PreferenceID= 20 -- 21
			UPDATE DHCustPreference SET VegetarianFlag =@Flag WHERE CustomerID =@CustID AND @PreferenceID=1
			UPDATE DHCustPreference SET HalalFlag=@Flag WHERE CustomerID =@CustID AND @PreferenceID=3	
			--UPDATE DHCustPreference SET KosherFlag=@Flag WHERE CustomerID =@CustID AND @PreferenceID =4
			UPDATE DHCustPreference SET TeetotalFlag=@Flag WHERE CustomerID = @CustID AND @PreferenceID=5

			UPDATE DHCustPreference SET ExpatFlag=@Flag WHERE CustomerID =@CustID AND @PreferenceID=26
			UPDATE DHCustPreference SET CeliacFlag=@Flag WHERE CustomerID = @CustID AND @PreferenceID=24
			UPDATE DHCustPreference SET LactoseFlag=@Flag WHERE CustomerID = @CustID AND @PreferenceID=25

			IF (@Optional1 IS NOT NULL)
			 BEGIN
				SET @SQLUpdate =NULL	    
				SET @SQLUpdate = 'UPDATE DHCustPreference SET Optional1Flag= ' + @Flag + ' WHERE CustomerID ='+ CONVERT(VARCHAR,@CustID) + ' AND 39 =' + CONVERT(VARCHAR,@PreferenceID)				
				EXEC (@SQLUpdate)
				
			END
			IF (@Optional2 IS NOT NULL) BEGIN
				 SET @SQLUpdate =NULL
				 SET @SQLUpdate = 'UPDATE DHCustPreference SET Optional2Flag= ' + @Flag + ' WHERE CustomerID =' + CONVERT(VARCHAR,@CustID) + ' AND 40 =' + CONVERT(VARCHAR,@PreferenceID)
				 EXEC (@SQLUpdate)
			END
			IF (@Optional3 IS NOT NULL) BEGIN
				 SET @SQLUpdate =NULL
				 SET @SQLUpdate = 'UPDATE DHCustPreference SET Optional3Flag= ' + @Flag + ' WHERE CustomerID =' + CONVERT(VARCHAR,@CustID) + 'AND 41 =' + CONVERT(VARCHAR,@PreferenceID)
				 EXEC (@SQLUpdate)
			END

			UPDATE DHCustPreference SET TescoGRPMailFlag = @Flag WHERE CustomerID = @CustID AND @PreferenceID=27
			UPDATE DHCustPreference SET TescoGRPEmailFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=28
			UPDATE DHCustPreference SET TescoGRPPhoneFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=29
			UPDATE DHCustPreference SET TescoGRPSMSFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=30
			UPDATE DHCustPreference SET PartnerMailFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=31
			UPDATE DHCustPreference SET PartnerEmailFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=32
			UPDATE DHCustPreference SET PartnerPhoneFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=33
			UPDATE DHCustPreference SET PartnerSMSFlag  =@Flag WHERE CustomerID = @CustID AND @PreferenceID=34
			UPDATE DHCustPreference SET ResearchMailFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=35
			UPDATE DHCustPreference SET ResearchEmailFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=36
			UPDATE DHCustPreference SET ResearchPhoneFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=37
			UPDATE DHCustPreference SET ResearchSMSFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=38
			UPDATE DHCustPreference SET BCMMailFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=50
			UPDATE DHCustPreference SET BCMEMailFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=51
			UPDATE DHCustPreference SET BCMPhoneFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=52
			UPDATE DHCustPreference SET BCMSMSFlag =@Flag WHERE CustomerID = @CustID AND @PreferenceID=53
			UPDATE DHCustPreference SET AllowpromotionsViaMailFlag = @Flag WHERE CustomerID =@CustID AND @PreferenceID =17
			UPDATE DHCustPreference SET AllowpromotionsviaPhoneFlag = @Flag WHERE CustomerID =@CustID AND @PreferenceID =16
			UPDATE DHCustPreference SET AllowgroupPromotionsFlag = @Flag WHERE CustomerID =@CustID AND @PreferenceID = 23 --20
			UPDATE DHCustPreference SET AllowThirdpartyPromotionsFlag = @Flag WHERE CustomerID =@CustID AND @PreferenceID=18

	FETCH CurPreference INTO @CustID,@PreferenceID,@Flag--,@PreferredContactType	
  END             
CLOSE CurPreference
DEALLOCATE CurPreference

/* End : ********************************************************************************************************/

/* This SP is used to process family member details *************************************************************/
			IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE NAME ='##FamilyMemberTemp') DROP TABLE ##FamilyMemberTemp			
			EXEC USP_Cust_Extract_Dunnhumby_FamilyMember @Last_Run,@Current_Run
/****************************************************************************************************************/

SET @SQLTempCust = 'INSERT INTO TempCust'
SET @SQLTempCust = @SQLTempCust + ' SELECT DISTINCT '
SET @SQLTempCust = @SQLTempCust + ' C.CustomerID,'
SET @SQLTempCust = @SQLTempCust + ' C.PrimaryCustomerID,'
SET @SQLTempCust = @SQLTempCust + ' C.CustomerMailStatus,'
SET @SQLTempCust = @SQLTempCust + ' C.CustomerUseStatusID,'
SET @SQLTempCust = @SQLTempCust + ' C.IncomeBandID,'
SET @SQLTempCust = @SQLTempCust + ' CP.PreferredContactType,' ------------------------Preferred_contact_type_code ****
SET @SQLTempCust = @SQLTempCust + ' C.JoinedDate,'
SET @SQLTempCust = @SQLTempCust + ' C.JoinedStoreID,'
SET @SQLTempCust = @SQLTempCust + ' C.PreferredStoreID,'
SET @SQLTempCust = @SQLTempCust + ' C.ISOLanguageCode,	'	
SET @SQLTempCust = @SQLTempCust + ' CP.AllowpromotionsViaMailFlag, '-----------------------AllowpromotionsViaMailFlag
SET @SQLTempCust = @SQLTempCust + ' CP.AllowpromotionsviaPhoneFlag,' ----------------------AllowpromotionsviaPhoneFlag
SET @SQLTempCust = @SQLTempCust + ' CP.AllowgroupPromotionsFlag,' ----------------------AllowgroupPromotionsFlag
SET @SQLTempCust = @SQLTempCust + ' CP.AllowThirdpartyPromotionsFlag, '--------------------AllowThirdpartyPromotionsFlag
SET @SQLTempCust = @SQLTempCust + ' CP.DiabeticFlag,' -------------------------------------DiabeticFlag
SET @SQLTempCust = @SQLTempCust + ' CP.VegetarianFlag, '-----------------------------------VegetarianFlag
SET @SQLTempCust = @SQLTempCust + ' CP.TeetotalFlag,' -------------------------------------TeetotalFlag
--SET @SQLTempCust = @SQLTempCust + ' CP.KosherFlag, '---------------------------------------KosherFlag
SET @SQLTempCust = @SQLTempCust + ' CP.HalalFlag,' ----------------------------------------HalalFlag
SET @SQLTempCust = @SQLTempCust + ' CP.CeliacFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.LactoseFlag,'
SET @SQLTempCust = @SQLTempCust +  CASE WHEN @OptionalValue IS NULL THEN '' ELSE @OptionalValue + ',' END
SET @SQLTempCust = @SQLTempCust + ' C.DateOfBirth AS FamilyMemberDOB1,'
SET @SQLTempCust = @SQLTempCust + ' DATEDIFF(YEAR,C.DateOfBirth,GETDATE()) AS FamilyMemberAge1,'
SET @SQLTempCust = @SQLTempCust + ' C.SEX AS FamilyMemberGenderCode1,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberDOB1 AS FamilyMemberDOB2,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberAge1 AS FamilyMemberAge2,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberGenderCode1 AS FamilyMemberGenderCode2,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberDOB2 AS FamilyMemberDOB3,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberAge2 AS FamilyMemberAge3,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberGenderCode2 AS FamilyMemberGenderCode3,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberDOB3 AS FamilyMemberDOB4,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberAge3 AS FamilyMemberDOB4,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberGenderCode3 AS FamilyMemberGenderCode4,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberDOB4 AS FamilyMemberDOB5,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberAge4 AS FamilyMemberAge5,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberGenderCode4 AS FamilyMemberGenderCode5,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberDOB5 AS FamilyMemberDOB6,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberAge5 AS FamilyMemberAge6,'
SET @SQLTempCust = @SQLTempCust + ' FM.FamilyMemberGenderCode5 AS FamilyMemberGenderCode6,'
SET @SQLTempCust = @SQLTempCust + ' C.MailingAddressPostCode,'
SET @SQLTempCust = @SQLTempCust + ' C.BusinessType,'
SET @SQLTempCust = @SQLTempCust + ' R.RaceDescEnglish,'
SET @SQLTempCust = @SQLTempCust + ' CC.ClubcardType,'
SET @SQLTempCust = @SQLTempCust + ' CP.ExpatFlag, '----------------------------------------ExpatFlag
SET @SQLTempCust = @SQLTempCust + ' CP.TescoGRPMailFlag, '
SET @SQLTempCust = @SQLTempCust + ' CP.TescoGRPEmailFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.TescoGRPPhoneFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.TescoGRPSMSFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.PartnerMailFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.PartnerEmailFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.PartnerPhoneFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.PartnerSMSFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.ResearchMailFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.ResearchEmailFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.ResearchPhoneFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.ResearchSMSFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.BCMMailFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.BCMEMailFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.BCMPhoneFlag,'
SET @SQLTempCust = @SQLTempCust + ' CP.BCMSMSFlag FROM Customer C (nolock)'
SET @SQLTempCust = @SQLTempCust + ' INNER JOIN Race R ON C.RaceID=R.RaceID'
SET @SQLTempCust = @SQLTempCust + ' INNER JOIN Clubcard CC ON C.CustomerID=CC.CustomerID'
SET @SQLTempCust = @SQLTempCust + ' LEFT OUTER JOIN ##FamilyMemberTemp FM ON C.CustomerID=FM.CustomerID'
SET @SQLTempCust = @SQLTempCust + ' LEFT OUTER JOIN DHCustPreference CP ON C.CustomerID=CP.CustomerID  '

-- Start :Action 99
IF(@NoOfRuns<>0)	
	BEGIN	
		IF((@From_Date IS NULL) AND (@To_Date IS NULL) ) 
			BEGIN			
				SET @Action ='99'
				SET @SQLTempCust = @SQLTempCust + ' WHERE C.AmendDateTime > ' +'''' + CONVERT(VARCHAR,@Last_Run) + ''''+ ' AND C.AmendDateTime < ' + '''' + CONVERT(VARCHAR,@Current_Run) + ''' AND C.IsDeleted= ''' + @IsDeleted	+ ''''
				EXEC(@SQLTempCust)			
			END		
--- End Action 99

--- Start: Action 1
	ELSE IF((@From_Date IS NOT NULL) AND (@To_Date IS NULL))
	BEGIN
		SET @Action ='01'
		SET @SQLTempCust = @SQLTempCust + ' WHERE C.AmendDateTime > ' +'''' + CONVERT(VARCHAR,@From_Date) + ''''+ ' AND C.AmendDateTime < ' + '''' + CONVERT(VARCHAR,@Current_Run) + ''' AND C.IsDeleted= ''' + @IsDeleted	+ ''''
		EXEC(@SQLTempCust)	
	END
-- End Action 1
--Start Action 3
	ELSE IF((@From_Date IS NOT NULL) AND (@To_Date IS NOT NULL))
	BEGIN
	SET @Action ='03'
		SET @SQLTempCust = @SQLTempCust + ' WHERE C.AmendDateTime > ' +'''' + CONVERT(VARCHAR,@From_Date) + ''''+ ' AND C.AmendDateTime < ' + '''' + CONVERT(VARCHAR,@To_Date) + ''' AND C.IsDeleted= ''' + @IsDeleted	+ ''''
		EXEC(@SQLTempCust)
	END
--End Action 3
-- Start Action 2
	ELSE IF((@From_Date IS NULL) AND (@To_Date IS NOT NULL))
	BEGIN
		SET @Action = '02'		
	END
--End Action
END
	ELSE	
		BEGIN
			SET @Action = ISNULL(@Action,'0')
			SET @SQLTempCust = @SQLTempCust + ' WHERE C.IsDeleted= ''' + @IsDeleted	+ ''' ORDER BY C.CustomerID'
			EXEC(@SQLTempCust)
		END
print @SQLTempCust
ExitScript:
IF (@Action !='02')
BEGIN
	SELECT @cmd = 'bcp "Select * From ['+@dbname+'].dbo.TempCust" queryout ' + @temppath + ' -S ' + @@servername +  ' -c -C1252 -T -t "|" '
	EXEC  master..xp_cmdshell @cmd

	SELECT @cmd = 'copy ' + @Working_Path + 'cust_header.txt+' + @temppath +' '+ @Path + ' /B'
	EXEC  master..xp_cmdshell @cmd, no_output

	/* DELETE ALL TEMPORARY TABLE ****************************************************************************************/
	IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[TempCust]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [dbo].[TempCust]
	
	IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE NAME ='##FamilyMemberTemp') DROP TABLE ##FamilyMemberTemp
	IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE NAME ='#Result') DROP TABLE #Result
	
	IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[DHCustPreference]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		DROP TABLE [dbo].[DHCustPreference]
	END 
	/**********************************************************************************************************************/

----------------CREATE OK File Block -------------------------------------
	SET @Path=REPLACE(@Path,'txt','ok')
	SELECT @Cmd = 'copy Nul ' + @Path
	EXEC  master..xp_cmdshell @Cmd
---------------------------------------------------------------------------

	SELECT @cmd = 'del '+ @temppath
	EXEC  master..xp_cmdshell @cmd, no_output

	IF(@Action='99' or @NoOfRuns=0)
		BEGIN			
			INSERT INTO DunnHumby_Cust VALUES(@Current_Run)
		END
END 
ELSE IF (@Action='02')
BEGIN
	SELECT @Cmd = 'bcp "Select ''Inputs Parameters are not sufficient for generating output''" queryout '+ @Path +' -S' + @@servername + ' -c -C1252 -t "|" -T '
	EXEC master..xp_cmdshell @Cmd
END
