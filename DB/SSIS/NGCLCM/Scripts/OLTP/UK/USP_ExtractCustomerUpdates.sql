/****** Object:  StoredProcedure [dbo].[USP_ExtractCustomerUpdates]    Script Date: 11/06/2012 11:03:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[USP_ExtractCustomerUpdates]        
AS        
---------------------------------------------------------------------------------------------------        
------        
------ Copyrights (C) 2009, Tesco HSC Pte Ltd,81-82, EPIP Area, WhiteFiled, Bangalore-66        
------ All rights reserved        
------        
------ Date : 2010 May 10 Monday        Version: 1.0        Author : P. Suresh Kumar.        
------ Description : Initial version        

------ Date : 2012 Oct 22 Monday        Version: 1.1        Author : Prakasha G.        
------ Description : Added column called "BCM" as part of LCM Change to the resultset.
------         
------ Title :  [USP_ExtractCustomerUpdates]        
------         
------ Purpose : Extract the fields from the Customer table, CustomerPreferences and FamilyMember.        
------           All these fields should be extracts into one record that includes Customer updates         
------           and preferences updates (opt-in/out) and family ages.        
------           All preference should converted to flags (“Y” or “N”) and any reward         
------           (Airmiles, BA miles, Xmas club) indicates A,B or X.        
------           All Family ages, currently it holds on date of birth need to convert them in ages         
------           For example If the DOB is 20-sep-1990 it should show age 20.        
------           This is a daily job, extract the data for one days starting from 22:00 (day1) to 22:00 (day2).        
------           Add header with date DDMMYYYY and trailer with number of records.         
------         
------ Current Ver : 1.0        
------         
------ Pre-requisite :         
------         
--------------------------------------------------------------------------------------------------        
------ Procedure        : [USP_ExtractCustomerUpdates]        
------ Input Parameter  : none        
------ Output Parameter :         
------ Description      :         
------                  :         
------ Remark           :         
------                  :         
------ How to Execute   :         
------         
------=============================================================================================        
BEGIN        
       SET NOCOUNT ON;        
               
       DECLARE @varJobID                        INT;        
       DECLARE @varJobSerialNo                  INT;        
       DECLARE @varJobStatus                    TINYINT;        
       DECLARE @varBatchStartDateTime           DATETIME;        
       DECLARE @varBatchEndDateTime             DATETIME;        
       DECLARE @varExtractStartDateTime         DATETIME;        
       DECLARE @varExtractEndDateTime           DATETIME;        
       DECLARE @varRemark                       VARCHAR(200);        
       DECLARE @varPreviousJobSerialNo          INT;        
       DECLARE @varPreviousJobStatus            TINYINT;        
       DECLARE @varPreviousExtractStartDateTime DATETIME;        
       DECLARE @varPreviousExtractEndDateTime   DATETIME;        
               
       DECLARE @vErrMsg      NVARCHAR(4000);        
       DECLARE @vErrSeverity INT;        
       DECLARE @vErrState    INT;        
       DECLARE @varCount     INT;        
             
       BEGIN TRY      
               
       SELECT @varJobID = 6,        
              @varJobStatus = 0,        
              @varBatchStartDateTime = GETDATE(),        
              @varExtractStartDateTime = DATEADD(DD,-1,DATEADD(HH,-2,CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),111),111))),        
              @varExtractEndDatetime = DATEADD(HH,-2,CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),111),111)),        
              @varRemark = 'Started .....';        
        
       SELECT @varCount = COUNT(*)         
         FROM JobControl (nolock)      
         WHERE JobID = 6        
               
       IF @varCount = 0  -- First Time we are executing this batch        
       BEGIN        
              SELECT @varJobSerialNo = 1        
       END;        
       ELSE   -- Not a First Time         
       BEGIN  -- Not a First Time         
                      
              SELECT TOP 1 @varPreviousJobSerialNo = JobSerialNo,        
                     @varPreviousJobStatus = JobStatus,        
                     @varPreviousExtractStartDateTime = ExtractStartDateTime,        
                     @varPreviousExtractEndDateTime = ExtractEndDateTime        
                FROM JobControl (nolock)        
               WHERE JobID = 6         
               ORDER BY JobSerialNo DESC;        
                      
              IF @varPreviousJobStatus = 0   -- i.e. Last Batch still not yet completed...        
              BEGIN        
                   IF EXISTS(SELECT NAME FROM sys.tables (nolock) WHERE NAME = 'TempCustExtractionLog')         
                           TRUNCATE TABLE TempCustExtractionLog;        
                   ELSE        
                           CREATE TABLE TempCustExtractionLog (LogText VARCHAR(100));        
                                   
                                   
                   INSERT INTO  TempCustExtractionLog VALUES ('Customer Extraction Already is in Procress; Please Check JobControl Table .....');        
                   RAISERROR ('Customer Extraction Already is in Procress; Please Check JobControl Table .....', 16,1 );         
              END;        
              ELSE IF @varPreviousJobStatus = 1 -- i.e. Last Batch Completed Successfully......        
              BEGIN                             -- Select Minimum of (LastBatch's ExtractEndDateTime, Default ExtractStartDateTime) to Default ExtractEndDatetime        
                     SELECT @varJobSerialNo = @varPreviousJobSerialNo + 1,        
                            @varExtractStartDateTime = (CASE WHEN @varPreviousExtractEndDateTime < @varExtractStartDateTime THEN @varPreviousExtractEndDateTime ELSE @varExtractStartDateTime END);        
                     --         
              END; -- ELSE IF @varPreviousJobStatus = 1        
              ELSE IF @varPreviousJobStatus = 2 -- i.e. Last Batch got failure .....        
              BEGIN                             -- LastBatch's ExtractStartDateTime to Default ExtractEndDatetime.        
                     SELECT @varJobSerialNo = @varPreviousJobSerialNo + 1,        
                            @varExtractStartDateTime = @varPreviousExtractStartDateTime;        
                     -- RAISERROR ('Last Batch got failure ......', 16,1 );        
              END; -- ELSE IF @varPreviousJobStatus = 2        
                      
       END;  -- Not a First Time         
               
       IF @varPreviousJobStatus IS NULL OR @varPreviousJobStatus <> 0         
       BEGIN         
               
            
       SET IDENTITY_INSERT JobControl ON;        
       INSERT INTO JobControl (JobID,        
                               JobSerialNo,        
                               JobStatus,        
                               BatchStartDateTime,        
                               ExtractStartDateTime,        
                               ExtractEndDateTime,        
                               Remark)        
                   SELECT @varJobID,        
                          @varJobSerialNo,        
                          @varJobStatus,        
                          @varBatchStartDateTime,        
                          @varExtractStartDateTime,        
                          @varExtractEndDatetime,        
                          @varRemark;        
       SET IDENTITY_INSERT JobControl ON;        
               
       TRUNCATE TABLE CustomerExtracts;        
               
  INSERT INTO CustomerExtracts (CustomerID,TitleEnglish,Name1,Name2,Name3,DateOfBirth,        
  Sex,MailingAddressLine1,MailingAddressLine2,MailingAddressLine3,MailingAddressLine4,        
  MailingAddressLine5,MailingAddressLine6,MailingAddressPostCode,DaytimePhoneNumber,NoOfHouseHoldMember)        
  SELECT MAIN.CustomerID,         
  UPPER(CU.TitleEnglish) TitleEnglish,        
  UPPER(CU.Name1) Name1,         
  UPPER(CU.Name2) Name2,         
  UPPER(CU.Name3) Name3,        
  CU.DateOfBirth,         
  CU.Sex,         
  UPPER(CU.MailingAddressLine1) MailingAddressLine1,         
  UPPER(CU.MailingAddressLine2) MailingAddressLine2,         
  UPPER(CU.MailingAddressLine3) MailingAddressLine3,         
  UPPER(CU.MailingAddressLine4) MailingAddressLine4,        
  UPPER(CU.MailingAddressLine5) MailingAddressLine5,        
  UPPER(CU.MailingAddressLine6) MailingAddressLine6,        
  UPPER(CU.MailingAddressPostCode) MailingAddressPostCode,        
   CU.DaytimePhoneNumber, CU.NoOfHouseHoldMember        
  FROM (        
  SELECT DISTINCT CustomerID         
  FROM (SELECT DISTINCT CustomerID FROM CustomerUpdates (nolock)         
     UNION        
     SELECT DISTINCT CustomerID FROM CustomerPreferenceUpdates (nolock)         
     UNION        
     SELECT DISTINCT CustomerID FROM FamilyMemberUpdates (nolock)) TEMP) MAIN        
  LEFT OUTER JOIN CustomerUpdates CU        
  ON MAIN.CustomerID = CU.CustomerID;        
          
  UPDATE CE        
  SET CE.TitleEnglish           = UPPER(CU.TitleEnglish),        
      CE.Name1					= UPPER(CU.Name1),         
      CE.Name2                  = UPPER(CU.Name2),         
      CE.Name3                  = UPPER(CU.Name3),        
      CE.DateOfBirth            = CU.DateOfBirth,         
      CE.Sex                    = CU.Sex,         
      CE.MailingAddressLine1    = UPPER(CU.MailingAddressLine1) ,         
      CE.MailingAddressLine2    = UPPER(CU.MailingAddressLine2) ,         
      CE.MailingAddressLine3    = UPPER(CU.MailingAddressLine3) ,         
      CE.MailingAddressLine4    = UPPER(CU.MailingAddressLine4) ,         
      CE.MailingAddressLine5    = UPPER(CU.MailingAddressLine5) ,         
      CE.MailingAddressLine6    = UPPER(CU.MailingAddressLine6) ,         
      CE.MailingAddressPostCode = UPPER(CU.MailingAddressPostCode) ,        
      CE.DaytimePhoneNumber     = CU.DaytimePhoneNumber,         
      CE.NoOfHouseHoldMember    = CU.NoOfHouseHoldMember        
  FROM CustomerExtracts CE, Customer CU  (nolock)        
  WHERE CE.Name1 IS NULL AND CE.Name3 IS NULL AND CE.CustomerID = CU.CustomerID;        
          
    UPDATE CE         
       SET "Diabetic"          = CASE WHEN GP.[1]=1  THEN 'N' END,       
           "Kosher"            = CASE WHEN GP.[2]=1  THEN 'N' END,       
           "Halal"             = CASE WHEN GP.[3]=1  THEN 'N' END,       
           "Vegetarian"        = CASE WHEN GP.[4]=1  THEN 'N' END,       
           "Teetotal"          = CASE WHEN GP.[5]=1  THEN 'N' END,       
           "PhoneContact"      = CASE WHEN GP.[6]=1  THEN 'N' END,       
           "TescoProducts"     = CASE WHEN GP.[7]=1  THEN 'N' END,       
           "TescoPartners"     = CASE WHEN GP.[8]=1  THEN 'N' END,       
           "CustomerResearch"  = CASE WHEN GP.[9]=1  THEN 'N' END,       
           "BAAirmiles"        = CASE WHEN GP.[10]=1 THEN 'N' END,       
           "AirmilesStandard"  = CASE WHEN GP.[11]=1 THEN 'N' END,       
           "AirmilesPremium"   = CASE WHEN GP.[12]=1 THEN 'N' END,       
           "XmasSaver"         = CASE WHEN GP.[13]=1 THEN 'N' END,       
           "BAAirmilesPremium" = CASE WHEN GP.[14]=1 THEN 'N' END,       
           "Ecoupon"           = CASE WHEN GP.[15]=1 THEN 'N' END,      
           "SaveTrees"         = CASE WHEN GP.[16]=1 THEN 'N' END,  
           "EMailContact"      = CASE WHEN GP.[43]=1 THEN 'N' END,  
           "MobileSMS"		   = CASE WHEN GP.[44]=1 THEN 'N' END,  
           "PostContact"       = CASE WHEN GP.[45]=1 THEN 'N' END,  
           "BT"				   = CASE WHEN GP.[48]=1 THEN 'N' END,                             
           "VA"				   = CASE WHEN GP.[17]=1 THEN 'N' END,
           "BCM"               = CASE WHEN GP.[49]=1 THEN 'N' END
       FROM CustomerExtracts CE, (SELECT CustomerID,[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[43],[44],[45],[48],[17],[49]
                                      FROM (SELECT CustomerPreferenceUpdates.CustomerID,PreferenceID,1 a         
                                              FROM CustomerPreferenceUpdates (nolock),CustomerExtracts TC (nolock)      
                   WHERE CustomerPreferenceUpdates.CustomerID  = TC.CustomerID       
                                              AND CustomerPreferenceUpdates.IsDeleted = 'Y'       
                                             ) t        
       Pivot ( Max(a) FOR PreferenceID IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[43],[44],[45],[48],[17],[49]) ) PVT ) GP        
       WHERE CE.CustomerID = GP.CustomerID;      
             
       UPDATE CE         
       SET "Diabetic"          = CASE WHEN GP.[1]=1 THEN 'Y' ELSE 'N' END,        
           "Kosher"            = CASE WHEN GP.[2]=1 THEN 'Y' ELSE 'N' END,        
           "Halal"             = CASE WHEN GP.[3]=1 THEN 'Y' ELSE 'N' END,     
           "Vegetarian"        = CASE WHEN GP.[4]=1 THEN 'Y' ELSE 'N' END,        
           "Teetotal"          = CASE WHEN GP.[5]=1 THEN 'Y' ELSE 'N' END,        
           "PhoneContact"      = CASE WHEN GP.[6]=1 THEN 'Y' ELSE 'N' END,        
           "TescoProducts"     = CASE WHEN GP.[7]=1 THEN 'Y' ELSE 'N' END,        
           "TescoPartners"     = CASE WHEN GP.[8]=1 THEN 'Y' ELSE 'N' END,        
           "CustomerResearch"  = CASE WHEN GP.[9]=1 THEN 'Y' ELSE 'N' END,        
           "BAAirmiles"        = CASE WHEN GP.[10]=1 THEN 'B' ELSE 'N' END,        
           "AirmilesStandard"  = CASE WHEN GP.[11]=1 THEN 'A' ELSE 'N' END,        
           "AirmilesPremium"   = CASE WHEN GP.[12]=1 THEN 'Y' ELSE 'N' END,        
           "XmasSaver"         = CASE WHEN GP.[13]=1 THEN 'X' ELSE 'N' END,         
           "BAAirmilesPremium" = CASE WHEN GP.[14]=1 THEN 'Y' ELSE 'N' END,        
           "Ecoupon"           = CASE WHEN GP.[15]=1 THEN 'Y' ELSE 'N' END,      
           "SaveTrees"         = CASE WHEN GP.[16]=1 THEN 'Y' ELSE 'N' END,  
           "EMailContact"      = CASE WHEN GP.[43]=1 THEN 'Y' ELSE 'N' END,  
           "MobileSMS"		   = CASE WHEN GP.[44]=1 THEN 'Y' ELSE 'N' END,  
           "PostContact"       = CASE WHEN GP.[45]=1 THEN 'Y' ELSE 'N' END,  
           "BT"				   = CASE WHEN GP.[48]=1 THEN 'Y' ELSE 'N' END,  
           "VA"				   = CASE WHEN GP.[17]=1 THEN 'V' ELSE 'N' END,
           "BCM"               = CASE WHEN GP.[49]=1 THEN 'Y' ELSE 'N' END
       FROM CustomerExtracts CE, (SELECT CustomerID,[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[43],[44],[45],[48],[17],[49]         
                                      FROM (SELECT CustomerPreferenceUpdates.CustomerID,PreferenceID,1 a         
                                              FROM CustomerPreferenceUpdates (nolock),CustomerExtracts TC (nolock)      
                                              WHERE CustomerPreferenceUpdates.CustomerID  = TC.CustomerID       
                                              AND CustomerPreferenceUpdates.IsDeleted = 'N'        
                                             ) t        
       Pivot ( Max(a) FOR PreferenceID IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[43],[44],[45],[48],[17],[49]) ) PVT ) GP        
       WHERE CE.CustomerID = GP.CustomerID;        
             
     UPDATE CE         
       SET "Diabetic"          = CASE WHEN GP.[1]=1 THEN 'Y' ELSE 'N' END,        
           "Kosher"            = CASE WHEN GP.[2]=1 THEN 'Y' ELSE 'N' END,        
           "Halal"             = CASE WHEN GP.[3]=1 THEN 'Y' ELSE 'N' END,        
           "Vegetarian"        = CASE WHEN GP.[4]=1 THEN 'Y' ELSE 'N' END,        
           "Teetotal"          = CASE WHEN GP.[5]=1 THEN 'Y' ELSE 'N' END,        
           "PhoneContact"      = CASE WHEN GP.[6]=1 THEN 'Y' ELSE 'N' END,        
           "TescoProducts"     = CASE WHEN GP.[7]=1 THEN 'Y' ELSE 'N' END,        
           "TescoPartners"     = CASE WHEN GP.[8]=1 THEN 'Y' ELSE 'N' END,        
           "CustomerResearch"  = CASE WHEN GP.[9]=1 THEN 'Y' ELSE 'N' END,        
           "BAAirmiles"        = CASE WHEN GP.[10]=1 THEN 'B' ELSE 'N' END,        
           "AirmilesStandard"  = CASE WHEN GP.[11]=1 THEN 'A' ELSE 'N' END,        
           "AirmilesPremium"   = CASE WHEN GP.[12]=1 THEN 'Y' ELSE 'N' END,        
           "XmasSaver"         = CASE WHEN GP.[13]=1 THEN 'X' ELSE 'N' END,         
           "BAAirmilesPremium" = CASE WHEN GP.[14]=1 THEN 'Y' ELSE 'N' END,        
           "Ecoupon"           = CASE WHEN GP.[15]=1 THEN 'Y' ELSE 'N' END,      
           "SaveTrees"         = CASE WHEN GP.[16]=1 THEN 'Y' ELSE 'N' END,  
           "EMailContact"      = CASE WHEN GP.[43]=1 THEN 'Y' ELSE 'N' END,  
           "MobileSMS"		   = CASE WHEN GP.[44]=1 THEN 'Y' ELSE 'N' END,  
           "PostContact"       = CASE WHEN GP.[45]=1 THEN 'Y' ELSE 'N' END,  
           "BT"				   = CASE WHEN GP.[48]=1 THEN 'Y' ELSE 'N' END,
           "VA"				   = CASE WHEN GP.[17]=1 THEN 'V' ELSE 'N' END,  
           "BCM"			   = CASE WHEN GP.[49]=1 THEN 'Y' ELSE 'N' END  
       FROM CustomerExtracts CE, (SELECT CustomerID,[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[43],[44],[45],[48],[17],[49]         
                                      FROM (      
                                      --SELECT CustomerPreferenceUpdates.CustomerID,PreferenceID,1 a         
                                      --        FROM CustomerPreferenceUpdates,CustomerExtracts TC        
                                      --        WHERE CustomerPreferenceUpdates.CustomerID  = TC.CustomerID         
                                      --        AND CustomerPreferenceUpdates.IsDeleted = 'N'        
          SELECT CustomerPreferenceUpdates.CustomerID,      
              PreferenceID,1 a         
            FROM CustomerPreferenceUpdates (nolock) INNER JOIN CustomerExtracts TC        
           ON CustomerPreferenceUpdates.CustomerID  = TC.CustomerID         
           WHERE  CustomerPreferenceUpdates.IsDeleted = 'N'       
          UNION ALL      
          SELECT A.CustomerID,A.PreferenceID, A.a      
          FROM (      
          SELECT CustomerPreference.CustomerID,PreferenceID,1 a         
            FROM CustomerPreference (nolock) ,CustomerExtracts TC        
           WHERE CustomerPreference.CustomerID  = TC.CustomerID         
             AND CustomerPreference.IsDeleted = 'N'  AND CustomerPreference.PreferenceOptStatusID = 1    
             ) A      
          WHERE NOT EXISTS       
          (         
           SELECT * FROM CustomerPreferenceUpdates (nolock)       
           WHERE CustomerID = A.CustomerID      
           AND PreferenceID = A.PreferenceID       
           )                                                     
                                             ) t        
       Pivot ( Max(a) FOR PreferenceID IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[43],[44],[45],[48],[17],[49]) ) PVT ) GP        
       WHERE CE.CustomerID = GP.CustomerID;        
             
      UPDATE CE         
       SET "Diabetic"          = CASE WHEN GP.[1]=1 THEN 'Y' ELSE 'N' END,        
           "Kosher"            = CASE WHEN GP.[2]=1 THEN 'Y' ELSE 'N' END,        
           "Halal"             = CASE WHEN GP.[3]=1 THEN 'Y' ELSE 'N' END,        
           "Vegetarian"        = CASE WHEN GP.[4]=1 THEN 'Y' ELSE 'N' END,        
           "Teetotal"          = CASE WHEN GP.[5]=1 THEN 'Y' ELSE 'N' END,        
           "PhoneContact"      = CASE WHEN GP.[6]=1 THEN 'Y' ELSE 'N' END,        
           "TescoProducts"     = CASE WHEN GP.[7]=1 THEN 'Y' ELSE 'N' END,        
           "TescoPartners"     = CASE WHEN GP.[8]=1 THEN 'Y' ELSE 'N' END,        
           "CustomerResearch"  = CASE WHEN GP.[9]=1 THEN 'Y' ELSE 'N' END,        
           "BAAirmiles"        = CASE WHEN GP.[10]=1 THEN 'B' ELSE 'N' END,        
           "AirmilesStandard"  = CASE WHEN GP.[11]=1 THEN 'A' ELSE 'N' END,        
           "AirmilesPremium"   = CASE WHEN GP.[12]=1 THEN 'Y' ELSE 'N' END,        
           "XmasSaver"         = CASE WHEN GP.[13]=1 THEN 'X' ELSE 'N' END,        
           "BAAirmilesPremium" = CASE WHEN GP.[14]=1 THEN 'Y' ELSE 'N' END,        
           "Ecoupon"           = CASE WHEN GP.[15]=1 THEN 'Y' ELSE 'N' END,      
           "SaveTrees"         = CASE WHEN GP.[16]=1 THEN 'Y' ELSE 'N' END,  
           "EMailContact"      = CASE WHEN GP.[43]=1 THEN 'Y' ELSE 'N' END,  
           "MobileSMS"		   = CASE WHEN GP.[44]=1 THEN 'Y' ELSE 'N' END,  
           "PostContact"       = CASE WHEN GP.[45]=1 THEN 'Y' ELSE 'N' END,  
           "BT"                = CASE WHEN GP.[48]=1 THEN 'Y' ELSE 'N' END,
           "VA"                = CASE WHEN GP.[17]=1 THEN 'V' ELSE 'N' END,     
           "BCM"               = CASE WHEN GP.[49]=1 THEN 'Y' ELSE 'N' END     
       FROM CustomerExtracts CE, (SELECT CustomerID,[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[43],[44],[45],[48],[17],[49]         
                                      FROM (SELECT CustomerPreference.CustomerID,PreferenceID,1 a         
                                              FROM CustomerPreference (nolock) ,CustomerExtracts TC        
                                              WHERE CustomerPreference.CustomerID  = TC.CustomerID         
                                              AND TC.CustomerID IN ( SELECT CustomerID FROM CustomerExtracts        
                                                                      WHERE Diabetic IS NULL AND         
                         Kosher IS NULL AND         
                         Halal IS NULL AND         
                         Vegetarian IS NULL AND         
                         Teetotal IS NULL AND         
                         PhoneContact IS NULL AND         
                         TescoProducts IS NULL AND         
                         TescoPartners IS NULL AND         
                         CustomerResearch IS NULL AND         
                         BAAirmiles IS NULL AND         
                         AirmilesStandard IS NULL AND         
                         AirmilesPremium IS NULL AND         
                         XmasSaver IS NULL AND         
                         BAAirmilesPremium IS NULL AND         
                         Ecoupon IS NULL AND      
                         SaveTrees IS NULL AND
                         EMailContact IS NULL AND
                         MobileSMS IS NULL AND
                         PostContact IS NULL AND
                         BT IS NULL AND                                             
                         VA IS NULL AND
                         BCM IS NULL) )t 
       Pivot ( Max(a) FOR PreferenceID IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[43],[44],[45],[48],[17],[49]) ) PVT ) GP        
       WHERE CE.CustomerID = GP.CustomerID;        
               
        UPDATE CustomerExtracts        
          SET Diabetic = 'N',         
           Kosher   = 'N',         
           Halal    = 'N',         
           Vegetarian = 'N',         
           Teetotal = 'N',         
           PhoneContact = 'N',         
           TescoProducts = 'N',         
           TescoPartners = 'N',         
           CustomerResearch = 'N',         
           BAAirmiles = 'N',         
           AirmilesStandard = 'N',         
           AirmilesPremium = 'N',         
           XmasSaver = 'N',         
           BAAirmilesPremium = 'N',         
           Ecoupon = 'N',      
           SaveTrees = 'N',  
           EMailContact = 'N',  
           MobileSMS = 'N',  
           PostContact = 'N',  
           BT = 'N',
           VA = 'N',
           BCM='N'
        WHERE Diabetic IS NULL AND         
              Kosher IS NULL AND         
           Halal IS NULL AND         
           Vegetarian IS NULL AND         
           Teetotal IS NULL AND         
           PhoneContact IS NULL AND         
           TescoProducts IS NULL AND         
           TescoPartners IS NULL AND         
           CustomerResearch IS NULL AND         
           BAAirmiles IS NULL AND         
           AirmilesStandard IS NULL AND         
           AirmilesPremium IS NULL AND         
           XmasSaver IS NULL AND         
           BAAirmilesPremium IS NULL AND         
           Ecoupon IS NULL AND      
           SaveTrees IS NULL AND
           EMailContact IS NULL AND
           MobileSMS IS NULL AND
           PostContact IS NULL AND
           BT IS NULL AND
           VA IS NULL AND
           BCM IS NULL;        
                                 
               
       UPDATE CE        
       SET FMemberSeqNo1 = CASE WHEN F.[1]!=0 THEN 1 ELSE 0 END,        
           FMemberSeqNo2 = CASE WHEN F.[2]!=0 THEN 2 ELSE 0 END,        
           FMemberSeqNo3 = CASE WHEN F.[3]!=0 THEN 3 ELSE 0 END,        
           FMemberSeqNo4 = CASE WHEN F.[4]!=0 THEN 4 ELSE 0 END,        
           FMemberSeqNo5 = CASE WHEN F.[5]!=0 THEN 5 ELSE 0 END,        
           HHAge1 = CASE WHEN F.[1]!=0 THEN F.[1] ELSE 0 END,        
           HHAge2 = CASE WHEN F.[2]!=0 THEN F.[2] ELSE 0 END,        
           HHAge3 = CASE WHEN F.[3]!=0 THEN F.[3] ELSE 0 END,        
           HHAge4 = CASE WHEN F.[4]!=0 THEN F.[4] ELSE 0 END,        
           HHAge5 = CASE WHEN F.[5]!=0 THEN F.[5] ELSE 0 END        
       FROM CustomerExtracts CE, (SELECT CustomerID,[1],[2],[3],[4],[5]        
                                      FROM (SELECT FamilyMemberUpdates.CustomerID,FamilyMemberSeqNo,        
                                                   Convert(varchar(30),Datediff(yy,FamilyMemberUpdates.DateOfBirth,getdate()),111) dateofbirth         
                                            FROM FamilyMemberUpdates (nolock) , CustomerExtracts TC        
                                              WHERE FamilyMemberUpdates.CustomerID  = TC.CustomerID         
                                                   ) t        
       Pivot ( max(dateofbirth) for FamilyMemberSeqNo in ([1],[2],[3],[4],[5]) ) Pvt) F        
       WHERE CE.CustomerID = F.CustomerID;        
               
               
       UPDATE CE        
       SET FMemberSeqNo1 = CASE WHEN F.[1]!=0 THEN 1 ELSE 0 END,        
           FMemberSeqNo2 = CASE WHEN F.[2]!=0 THEN 2 ELSE 0 END,        
           FMemberSeqNo3 = CASE WHEN F.[3]!=0 THEN 3 ELSE 0 END,        
           FMemberSeqNo4 = CASE WHEN F.[4]!=0 THEN 4 ELSE 0 END,        
           FMemberSeqNo5 = CASE WHEN F.[5]!=0 THEN 5 ELSE 0 END,        
           HHAge1 = CASE WHEN F.[1]!=0 THEN F.[1] ELSE 0 END,        
           HHAge2 = CASE WHEN F.[2]!=0 THEN F.[2] ELSE 0 END,        
           HHAge3 = CASE WHEN F.[3]!=0 THEN F.[3] ELSE 0 END,        
           HHAge4 = CASE WHEN F.[4]!=0 THEN F.[4] ELSE 0 END,        
           HHAge5 = CASE WHEN F.[5]!=0 THEN F.[5] ELSE 0 END        
       FROM CustomerExtracts CE, (SELECT CustomerID,[1],[2],[3],[4],[5]        
                                      FROM (SELECT FamilyMember.CustomerID,FamilyMemberSeqNo,        
                                                   Convert(varchar(30),Datediff(yy,FamilyMember.DateOfBirth,getdate()),111) dateofbirth         
                                            FROM FamilyMember (nolock) , CustomerExtracts TC        
           WHERE FamilyMember.CustomerID  = TC.CustomerID         
                                              AND TC.CustomerID IN ( SELECT CustomerID FROM CustomerExtracts        
                                                                      WHERE FMemberSeqNo1 IS NULL AND         
                                                                            FMemberSeqNo2 IS NULL AND         
                                                                            FMemberSeqNo3 IS NULL AND         
                                                                            FMemberSeqNo4 IS NULL AND         
                                                                            FMemberSeqNo5 IS NULL AND         
                                                                            HHAge1 IS NULL AND         
                                              HHAge2 IS NULL AND         
                                                                            HHAge3 IS NULL AND         
                                                                            HHAge4 IS NULL AND         
                                                                            HHAge5 IS NULL)           
                                                   ) t        
       Pivot ( max(dateofbirth) for FamilyMemberSeqNo in ([1],[2],[3],[4],[5]) ) Pvt) F        
       WHERE CE.CustomerID = F.CustomerID;        
               
               
       UPDATE CustomerExtracts        
              SET FMemberSeqNo1 = 0,        
                  FMemberSeqNo2 = 0,        
                  FMemberSeqNo3 = 0,        
                  FMemberSeqNo4 = 0,        
                  FMemberSeqNo5 = 0,        
                  HHAge1 = 0,        
                  HHAge2 = 0,        
                  HHAge3 = 0,        
				  HHAge4 = 0,        
                  HHAge5 = 0        
            WHERE FMemberSeqNo1 IS NULL AND         
                  FMemberSeqNo2 IS NULL AND         
                  FMemberSeqNo3 IS NULL AND         
                  FMemberSeqNo4 IS NULL AND         
                  FMemberSeqNo5 IS NULL AND         
                  HHAge1 IS NULL AND         
                  HHAge2 IS NULL AND         
                  HHAge3 IS NULL AND         
                  HHAge4 IS NULL AND         
                  HHAge5 IS NULL;        
        
       WITH GetCardDate(CustomerID,Cdate)         
       AS (SELECT CustomerID,Max(CardIssuedDate)         
              FROM Clubcard (nolock)         
             WHERE IsDeleted = 'N'        
          GROUP BY CustomerID)        
       UPDATE CE         
         SET ClubcardID = C.ClubCardID         
        FROM CustomerExtracts CE, Clubcard C (nolock) , GetCardDate G        
       WHERE CE.CustomerId = C.CustomerID         
         AND C.CustomerID = G.CustomerID         
         AND C.CardIssuedDate = G.Cdate;        
        
       END;        
                      
     UPDATE Cust      
        SET TitleEnglish = TIT.TitleCG      
       FROM CustomerExtracts Cust, Title TIT      
      WHERE Cust.TitleEnglish = TIT.TitleEnglish      
                      
        TRUNCATE TABLE CustomerUpdates;        
        TRUNCATE TABLE CustomerPreferenceUpdates;          
        TRUNCATE TABLE FamilyMemberUpdates;        
               
     SELECT @varJobSerialNo 'BatchNo'        
               
       --SELECT CustomerID,         
       --       ClubcardID,         
       --       TitleEnglish,         
       --       SUBSTRING(Name3,1,25) 'Name3',        
       --       SUBSTRING(Name1,1,20) 'Name1',         
       --       SUBSTRING(Name2,1,2) 'Name2',        
       --       DateOfBirth,         
       --       (CASE WHEN SEX <> 'M' AND SEX <> 'F' THEN ' '         
       --          WHEN SEX IS NULL THEN ' ' ELSE SEX END) Sex,         
       --      (CASE WHEN Kosher = 'Y' AND Halal = 'N' AND Vegetarian = 'N' THEN 'K'         
       --             WHEN Kosher = 'N' AND Halal = 'Y' AND Vegetarian = 'N' THEN 'H'         
       --             WHEN Kosher = 'N' AND Halal = 'N' AND Vegetarian = 'Y' THEN 'V' ELSE ' ' END) 'Dietary',        
       --      (CASE WHEN Diabetic = 'Y' THEN 'D' ELSE ' ' END) 'Diabetic',        
       --       Teetotal,        
       --       MailingAddressLine1, MailingAddressLine2, MailingAddressLine3,         
       --       MailingAddressLine4, MailingAddressLine5,        
       --       MailingAddressPostCode, DaytimePhoneNumber,        
       --       HHAge1,HHAge2,HHAge3,HHAge4,HHAge5,        
       --       NoOfHouseHoldMember,        
       --       TescoProducts, TescoPartners, CustomerResearch,        
       --       (CASE WHEN BAAirmiles ='B' AND BAAirmilesPremium = 'N' AND AirmilesStandard = 'N'AND AirmilesPremium = 'N' AND XmasSaver = 'N' THEN 'B'        
       --             WHEN BAAirmiles ='N' AND BAAirmilesPremium = 'Y' AND AirmilesStandard = 'N'AND AirmilesPremium = 'N' AND XmasSaver = 'N' THEN 'B'        
       --             WHEN BAAirmiles ='N' AND BAAirmilesPremium = 'N' AND AirmilesStandard = 'A'AND AirmilesPremium = 'N' AND XmasSaver = 'N' THEN 'A'        
       --             WHEN BAAirmiles ='N' AND BAAirmilesPremium = 'N' AND AirmilesStandard = 'N'AND AirmilesPremium = 'Y' AND XmasSaver = 'N' THEN 'A'        
       --             WHEN BAAirmiles ='N' AND BAAirmilesPremium = 'N' AND AirmilesStandard = 'N'AND AirmilesPremium = 'N' AND XmasSaver = 'X' THEN 'X'        
       --             ELSE ' ' END) 'RewardStatus',Ecoupon        
       --       -- ,FMemberSeqNo1,FMemberSeqNo2,FMemberSeqNo3,FMemberSeqNo4,FMemberSeqNo5,        
       -- FROM CustomerExtracts        
       END TRY        
       BEGIN CATCH        
               SELECT @vErrMsg = ' While Executing USP_ExtractCustomerUpdates ..... ' + ERROR_MESSAGE(),        
                     @vErrSeverity = ERROR_SEVERITY(),        
                     @vErrState = ERROR_STATE();        
              RAISERROR (@vErrMsg, @vErrSeverity, @vErrState );        
       END CATCH      
      
END; -- PROCEDURE USP_ExtractCustomerUpdates 



