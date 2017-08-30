/****** Object:  StoredProcedure [dbo].[USP_Upload_CustomerClubcardData]    Script Date: 11/27/2012 12:35:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_Upload_CustomerClubcardData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_Upload_CustomerClubcardData]
GO

/****** Object:  StoredProcedure [dbo].[USP_Upload_CustomerClubcardData]    Script Date: 11/27/2012 12:35:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO 
      
        
CREATE PROCEDURE [dbo].[USP_Upload_CustomerClubcardData]                      
AS                      
------------------------------------------------------------------------------------------------------------------------                      
------                      
------ Copyrights (C) 2009, Tesco HSC Pte Ltd,81-82, EPIP Area, WhiteFiled, Bangalore-66                      
------ All rights reserved                      
------                      
------                       
------ Title :  [USP_Upload_CustomerClubcardData]                      
------ Purpose :                       
------                       
------ Current Ver : 1.1                      
------                       
------ Pre-requisite :                       
------                       
------------------------------------------------------------------------------------------------------------------------                      
------ Procedure        : [USP_CCO_Upload_CustomerClubcardData]                      
------ Input Parameter  : none                      
------ Output Parameter : none                      
------ Description      :                       
------                  :                        
------ Remark           :                       
------                  :                       
------ How to Execute   :                       
------                  :                       
---=====================================================================================================================                      
------                       
------ Modification History:                      
------ ====================                      
------                       
------ Version: 1.1     Author: P. Suresh Kumar     Date: 2010-Jun-17                       
------ Description : Adding/Updating SaveTrees PreferenceID (16) in CustomerPreference Table.                      
------                       
------ Version: 1.0     Author: P. Suresh Kumar     Date : 2010 May 10 Monday                       
------ Description : Initial version                      
------                       
---=====================================================================================================================                      
--               Part 1 : B U S I N E S S   R U L E S   F O R   C U S T O M E R - I N S E R T                         --                      
-- =====================================================================================================================                      
----                          
----    2.2 New Customer details will include a ‘CG Primary Customer No’.                       
----        This will need to be converted to an NGC-Customer-ID before writing the new row.                       
----                              
----        i) If the value of ‘CG Primary Customer No’ is the same as the value of ‘CG Customer No’                       
----           then this is a “Main” customer, and we can set PimaryCustomerID to the same value as CustomerID.                      
----                          
----        ii) If ‘CG Primary Customer No’ is different to ‘CG Customer No’ then this is an “Associate” customer so                       
----            we need to find the NGC-Customer-ID of the “Main” customer as follows:                      
----            On CustomerAlternateID table,                       
----            find row with CustomerAlternateIDType = ‘1’ and                       
----                          CustomerAlternateID = ‘CG Primary Customer No.’ then use the CustomerID                       
----            found as the PrimaryCustomerID on the new Customer record.                       
----                          
----4.  Insert relationship between new NGC Customer and CG Household no.                      
----    Insert row in CustomerAlternateID table, for this CustomerID, with CustomerAlternateIDType = ‘3’                       
---- and set CustomerAlternateID to the value of the CG Household no. (Done)                      
----         
----5.  Insert Customer Preferences•                      
----    Insert one row on the CustomerPreference table for each customer preference set to ‘yes’ on the feed,                      
----    as defined in the Customer + Card Mapping document.                      
----                          
----6.  for 1. BA Airmiles Standard, 2. BA Airmiles Premium, 3. Airmiles Standard, 4. Airmiles Premium.                      
----                          
----    If Reward-Flag = “A”                       
----           If the Deal Status = “R” or “S”                      
----                  Set as "Airmiles Premium Customer". (i.e. PreferenceID: 12)                      
----           Else                      
----                  Set as "Airmiles Standard Customer". (i.e. PreferenceID: 11)                      
----           End-IF                      
----    End-IF                      
----                          
----    If Reward-Flag = “B”                       
----           If the Deal Status = “K” or “M”                      
----               Set as "BA Airmiles Standard Customer". (i.e. PreferenceID: 10)                      
----           Else                      
----                  Set as "BA Airmiles Premium Customer". (i.e. PreferenceID: 14)                      
----           End-IF                      
----    End-IF                      
----                          
-- =====================================================================================================================                      
--               Part 2 : B U S I N E S S   R U L E S   F O R   C L U B C A R D - I N S E R T                         --                      
-- =====================================================================================================================                      
----                          
----1.  Check this Clubcard number is actually new to NGC.                      
----    Look for entry on Clubcard table where ClubcardID = ‘Clubcard number’ from feed.                      
----    Note: If Clubcard number needs to be encrypted, then a further translation table will need to be used here first.                      
----    IF FOUND We are moving TO LOG FILE. (Else continue.                      
----                           
----3.  Insert details of new Clubcard into NGC.                      
----     Create and insert new row on Clubcard table as follows:                      
----     ClubcardStatus and CardIssuedDate mapped from feed as per Data Mapping document.                      
----     ClubcardType derived from Clubcard number as per Data Mapping document.                      
----     CustomerID as found in step above                      
----     PrimaryClubcardID should be set to same value as ClubcardID (t.b.c.)                       
----     ClubcardID should be set to Clubcard number – unless Clubcard number needs to be encrypted,                       
----     in which case this will change a bit.                      
----                          
-- =====================================================================================================================                      
--               Part 3 : B U S I N E S S   R U L E S   F O R   C L U B C A R D - U P D A T E                         --                      
-- =====================================================================================================================                      
----                          
----1.  Check this Clubcard number already exists in NGC.                      
----    Look for entry on Clubcard table where ClubcardID = ‘Clubcard number’ from feed.                      
----    Note: If Clubcard number needs to be encrypted, then a further translation table will need to be used here first.                      
----    IF NOT FOUND – there has been a serious error so alert support team (t.b.c) Else continue.                      
----                          
----3.  Check for any other changes.                      
----    Check for change of value in either of the other two fields that are on the feed (ClubcardStatus and CardIssuedDate)                       
----    using the Data Mapping document. If either has changed, update the relevant field and re-write this row.                      
----    (Note should be done at same time as amending CustomerID, if both are required – although that is extremely unlikely)                      
----                          
-- =====================================================================================================================                      
--               Part 4 : B U S I N E S S   R U L E S   F O R   C U S T O M E R - U P D A T E                         --                  
-- =====================================================================================================================                      
----                          
----2.  Check whether any details on Customer table have changed, and amend as needed.                      
----                          
----    2. 1. Using CustomerID found above, read relevant row from Customer table.                      
----                          
----    2. 2. Check if any of the fields that map to the feed have changed values.                      
----          (Title, Name, DoB, Sex, Address, Grid Refs, Phone, Join Date, Status, No in Household) and if so,                       
----          change values as per Customer + Card data mapping document and re-write row.                      
----                          
----    2. 3. Also need to check whether PrimaryCustomerID has changed and update if needed. To do this:                      
----          If CG Primary Customer No is the same as CG Customer No on the changes record, and PrimaryCustomerID                    
----          is the same as CustomerID on this NGC Customer, then no change required.                      
----          Else find NGC CustomerID relating to the CG Primary Customer No on the changes file by going to the                       
----          CustomerAlternateID table and finding the row with CustomerAlternateIDType = ‘1’ and CustomerAlternateID = ‘CG Primary Customer No.’                        
----          If this NGC CustomerID is different to the PrimaryCustomerID on the Customer record,                      
----          update the PrimaryCustomerID to this new NGC CustomerID.                      
----                          
----3.  Check whether CG household no has changed for this customer, and amend as needed.                      
----    Find row in CustomerAlternateID table, for this CustomerID, with CustomerAlternateIDType = ‘3’.                      
----    If CustomerAlternateID is not equal to the value of the CG Household no on the feed, amend it and re-write row.                      
----                          
----4.  Update customer preferences if required.                      
----                          
----    4. 1. For each preference that is shown on the feed (see Customer + Card mapping document for details) –                      
----                          
----    4. 2. If preference is set to ‘yes’ on the feed, check whether CustomerPreference entry exists for this CustomerID                       
----          and PreferenceID, and if not, INSERT one.                      
----                          
----    4. 3. If preference is set to ‘no’ on the feed, check whether CustomerPreference entry exists for this CustomerID                       
----             and PreferenceID, and if it does, DELETE it.                      
----                          
----5.  Ignore ‘Family Member’ details See Notes below:                       
----                          
-- =====================================================================================================================                      
--               Part 5 : B U S I N E S S   R U L E S   F O R   C L U B C A R D - D E L E T E                         --                      
-- =====================================================================================================================                      
----                          
----1.  Check this Clubcard number currently exists in NGC.                      
----    Look for entry on Clubcard table where ClubcardID = ‘Clubcard number’ from feed.                      
----    Note: If Clubcard number needs to be encrypted, then a further translation table will need to be used here first.                      
----    IF NOT FOUND – there has been a serious error so alert support team (??) Else continue.                      
----                          
----2.  Delete any transactions held against this card (t.b.c)                      
----                          
----3.  Delete ClubcardOffer entries and/or Points Summary entries if they relate to this specific card. (t.b.c)                      
----                          
----4.  Delete this Clubcard entry from NGC.                      
----                          
-- =====================================================================================================================                      
--               Part 6 : B U S I N E S S   R U L E S   F O R   C U S T O M E R - D E L E T E                         --                      
-- =====================================================================================================================                      
----                          
----1.  Get relevant NGC CustomerID.                      
----    On CustomerAlternateID table, find row with CustomerAlternateIDType = ‘1’ and CustomerAlternateID = ‘CG Customer no.’                      
----    IF NOT FOUND – there has been a serious error so alert support team (??) Else continue.               
----                          
----2.  Delete all FamilyMember entries for this Customer.                      
----    Read all FamilyMember entries for this NGC CustomerID.                      
----    For each one found – set to “deleted” (if not already deleted).                      
----                          
----3.  Delete all CustomerPreference entries for this Customer.                      
----    Read all CustomerPreference entries for this NGC CustomerID.                      
----    For each one found – set to “deleted” (if not already deleted).                      
----                          
----4.  Delete all CustomerAlternateID entries for this Customer.                      
----    Read all CustomerAlternateID entries for this NGC CustomerID.                      
----    For each one found – set to “deleted” (if not already deleted).                      
----    Delete this Customer entry from NGC. Set it to “deleted”.                      
----                          
-- =====================================================================================================================                      
BEGIN                      
       SET NOCOUNT ON;                      
--     -- Local Variables                       
--     -- Variables related to customer data.                      
       DECLARE @varRecType                  VARCHAR(1)                      
       DECLARE @varCustomerID               VARCHAR(10)                      
       DECLARE @varTitleEnglish             VARCHAR(4)                       
       DECLARE @varName3                    VARCHAR(25)                      
       DECLARE @varName1                    VARCHAR(20)                      
       DECLARE @varName2                    VARCHAR(2)                       
       DECLARE @varSex                      VARCHAR(1)                       
       DECLARE @varPrimaryCustomerID        VARCHAR(10)                      
       DECLARE @varCustomerUseStatusID      VARCHAR(1)                       
       DECLARE @varJoinedDate               VARCHAR(10)                      
       DECLARE @varDietaryPreferenceID      VARCHAR(1)                       
       DECLARE @varMedicalPreferenceID      VARCHAR(1)                       
       DECLARE @varTeeTotalPreferenceID     VARCHAR(1)                       
       DECLARE @varMailingAddressLine1    VARCHAR(30)                      
       DECLARE @varMailingAddressLine2      VARCHAR(30)                      
       DECLARE @varMailingAddressLine3      VARCHAR(30)                      
       DECLARE @varMailingAddressLine4      VARCHAR(30)                      
       DECLARE @varMailingAddressLine5      VARCHAR(20)                      
       DECLARE @varMailingAddressPostCode   VARCHAR(9)                       
       DECLARE @varDaytimePhoneNumber       VARCHAR(15)                      
       DECLARE @varHHAge1                   VARCHAR(2)                       
       DECLARE @varHHAge2                   VARCHAR(2)                       
       DECLARE @varHHAge3                   VARCHAR(2)                       
       DECLARE @varHHAge4                   VARCHAR(2)                       
       DECLARE @varHHAge5                   VARCHAR(2)                        DECLARE @varNoOfHouseHoldMember      VARCHAR(2)                       
       DECLARE @varProductsPreferenceID     VARCHAR(1)                       
       DECLARE @varPartnersPreferenceID     VARCHAR(1)                       
       DECLARE @varPanelPreferenceID        VARCHAR(1)                       
       DECLARE @varCustResearchPreferenceID VARCHAR(1)                       
       DECLARE @varAirMilesPreferenceID     VARCHAR(1)                       
       DECLARE @varDealStatusPreferenceID   VARCHAR(1)                       
       DECLARE @varDateOfBirth              VARCHAR(10)                      
       DECLARE @varLongitude                VARCHAR(5)                       
       DECLARE @varLatitude                 VARCHAR(5)                       
       DECLARE @varPrimaryInd               VARCHAR(1)                       
       DECLARE @varHouseHoldNumber          VARCHAR(10)                       
       DECLARE @varCustomerMailStatus       VARCHAR(1)                       
       DECLARE @varSaveTrees                VARCHAR(1)                       
       DECLARE @varTitleFlag                VARCHAR(1)                       
       DECLARE @varSurnameFlag              VARCHAR(1)                       
       DECLARE @varFirstnameFlag            VARCHAR(1)                       
       DECLARE @varInitialFlag              VARCHAR(1)                       
       DECLARE @varDOBFlag                  VARCHAR(1)                       
       DECLARE @varSexFlag           VARCHAR(1)                       
       DECLARE @varDietFlag                 VARCHAR(1)                       
       DECLARE @varPhoneFlag                VARCHAR(1)                       
       DECLARE @varAddrFlag                 VARCHAR(1)                       
       DECLARE @varAgeFlag                  VARCHAR(1)                       
       DECLARE @varMailingStatusFlag        VARCHAR(1)                       
       DECLARE @varPersonInHHFlag           VARCHAR(1)                       
       DECLARE @varAirMilesFlag             VARCHAR(1)                     
       DECLARE @varJoinedStoreID   VARCHAR(10)                   
       DECLARE @varJoinedStoredIDFlag  VARCHAR(1)                  
                             
--       -- Variables related to clubcard data.                      
                             
       DECLARE @varCCRecType        VARCHAR(1)                      
       DECLARE @varCCClubcardID     VARCHAR(18)                      
       DECLARE @varCCCustomerID     VARCHAR(10)                      
       DECLARE @varCCCardIssueDate  VARCHAR(10)                      
       DECLARE @varCCClubcardStatus VARCHAR(1)                      
       DECLARE @varCCClubcardType   TINYINT;   
                             
--       -- Miscellaneous Variables.                      
       DECLARE @varClubcardStatus TINYINT;                      
       DECLARE @varCustTotRecCtr INT, @varCustInsertCtr INT, @varCustUpdateCtr INT, @varCustDeleteCtr INT, @varCustErrorCtr  INT;                      
       DECLARE @varCardTotRecCtr INT, @varCardInsertCtr INT, @varCardUpdateCtr INT, @varCardDeleteCtr INT, @varCardErrorCtr  INT;                      
                             
       DECLARE @varCAltInsertCtr INT;                      
       DECLARE @varCAltDeleteCtr INT;                      
                             
       DECLARE @varCustPrefInsertCtr INT;                      
       DECLARE @varCustPrefDeleteCtr INT;                      
                             
    DECLARE @varFamMemInsertCtr INT;                      
       DECLARE @varFamMemDeleteCtr INT;                      
                            
       DECLARE @varISCustomerExists INT;                      
       DECLARE @varISClubcardExists INT;                                        
       DECLARE @varNeedtoUpdateCustRec INT;                      
       DECLARE @varNeedtoUpdateCardRec INT;                      
                             
       DECLARE @varLineNo VARCHAR(4);                      
       DECLARE @varLogText VARCHAR(500);                      
       DECLARE @varRowcount INT;                      
       DECLARE @varDotcomCustomerID BIGINT;                      
       DECLARE @varCustomerSource NVARCHAR(1);                      
                             
       DECLARE @varExistingTitleEnglish           VARCHAR(20);                       DECLARE @varExistingName1                  nvarchar(200);                      
       DECLARE @varExistingName2                  nvarchar(200);                      
       DECLARE @varExistingName3                  nvarchar(200);                      
       DECLARE @varExistingDateOfBirth            datetime;                      
       DECLARE @varExistingSex                    nchar;                      
       DECLARE @varExistingPrimaryCustomerID      bigint;                      
       DECLARE @varExistingCustomerUseStatusID    tinyint;                      
       DECLARE @varExistingJoinedDate             datetime;                      
       DECLARE @varExistingMailingAddressLine1    nvarchar(320);                      
       DECLARE @varExistingMailingAddressLine2    nvarchar(320);                      
       DECLARE @varExistingMailingAddressLine3    nvarchar(320);                      
       DECLARE @varExistingMailingAddressLine4    nvarchar(320);                      
       DECLARE @varExistingMailingAddressLine5    nvarchar(320);                      
       DECLARE @varExistingMailingAddressPostCode nvarchar(40);                      
       DECLARE @varExistingLongitude              NUMERIC(9,6);                      
       DECLARE @varExistingLatitude               NUMERIC(9,6);                      
       DECLARE @varExistingDaytimePhoneNumber     VARCHAR(40);                      
       DECLARE @varExistingNoOfHouseHoldMember    tinyint;                      
       DECLARE @varExistingCustomerMailStatus     tinyint;                  
       DECLARE @varExistingJoinedStoreID       int;                      
                             
       BEGIN TRY                      
       UPDATE TempDataUploadCustomer SET DateOfBirth = NULL WHERE DateOfBirth = '0000/00/00';                      
                             
       SELECT @varCustTotRecCtr = 0, @varCustInsertCtr = 0, @varCustUpdateCtr = 0, @varCustDeleteCtr = 0, @varCustErrorCtr  = 0,                      
              @varCardTotRecCtr = 0, @varCardInsertCtr = 0, @varCardUpdateCtr = 0, @varCardDeleteCtr = 0, @varCardErrorCtr  = 0,                      
              @varCAltInsertCtr = 0, @varCAltDeleteCtr = 0, @varCustPrefInsertCtr = 0, @varCustPrefDeleteCtr = 0,                       
              @varFamMemInsertCtr = 0, @varFamMemDeleteCtr = 0;                      
                             
-- =====================================================================================================================                      
--                 Part 1 : C U S T O M E R - I N S E R T STARTS FROM HERE                                     --                      
-- =====================================================================================================================                      
                            
       DECLARE cur_Customer CURSOR FOR SELECT * FROM TempDataUploadCustomer (nolock) WHERE RecType = 'I';                      
       OPEN cur_Customer                      
       FETCH NEXT FROM cur_Customer INTO @varRecType, @varCustomerID, @varTitleEnglish, @varName3, @varName1,                      
                                             @varName2, @varSex, @varPrimaryCustomerID, @varCustomerUseStatusID, @varJoinedDate,                      
      @varDietaryPreferenceID, @varMedicalPreferenceID, @varTeeTotalPreferenceID, @varMailingAddressLine1,                      
                                             @varMailingAddressLine2,@varMailingAddressLine3, @varMailingAddressLine4, @varMailingAddressLine5,                
                                             @varMailingAddressPostCode, @varDaytimePhoneNumber, @varHHAge1, @varHHAge2, @varHHAge3, @varHHAge4,                      
                                             @varHHAge5, @varNoOfHouseHoldMember, @varProductsPreferenceID, @varPartnersPreferenceID,                       
                                             @varCustResearchPreferenceID, @varPanelPreferenceID,                       
                                             @varAirMilesPreferenceID, @varDealStatusPreferenceID, @varDateOfBirth,                      
                                      @varLongitude, @varLatitude, @varPrimaryInd, @varHouseHoldNumber,@varCustomerMailStatus,@varSaveTrees,                      
                                             @varTitleFlag, @varSurnameFlag, @varFirstnameFlag, @varInitialFlag, @varDOBFlag,                      
                                             @varSexFlag, @varDietFlag, @varPhoneFlag, @varAddrFlag, @varAgeFlag,                      
                                             @varMailingStatusFlag, @varPersonInHHFlag, @varAirMilesFlag, @varJoinedStoreID, @varJoinedStoredIDFlag;                      
       WHILE @@FETCH_STATUS = 0                      
       BEGIN                      
       SET @varCustTotRecCtr = @varCustTotRecCtr + 1;                 
       BEGIN TRY            
                      
              -- -----------------------------------------------------------------------------------------------------------------                      
              -- 1. Inserting Customer Record                      
              -- -----------------------------------------------------------------------------------------------------------------                      
              SET @varLineNo = ''                      
              SET IDENTITY_INSERT Customer ON                      
              INSERT INTO Customer (CustomerID,TitleEnglish,Name3,Name1,Name2,                      
                                    DateOfBirth,Sex,PrimaryCustomerID,CustomerUseStatusID,JoinedDate,MailingAddressLine1,                      
                                    MailingAddressLine2,MailingAddressLine3,MailingAddressLine4,MailingAddressLine5,                      
                                    MailingAddressPostCode, Longitude, Latitude, DaytimePhoneNumber,CustomerMailStatus,NoOfHouseHoldMember,InsertDateTime,                      
                                    InsertBy,IsDeleted,CustomerWelcomedFlag,JoinedStoreID)                      
                            VALUES (@varCustomerID,                       
                                    (case when @varTitleEnglish = '    ' then null else @varTitleEnglish end),                       
                                    @varName3,                      
                                    @varName1,                       
                                    @varName2,                       
                                    @varDateOfBirth,                       
                                    @varSex,                       
                                    @varPrimaryCustomerID,                      
                                    @varCustomerUseStatusID,                       
                                    @varJoinedDate,                       
                                    @varMailingAddressLine1,          
                                    @varMailingAddressLine2,                       
                                    @varMailingAddressLine3,                       
                                    @varMailingAddressLine4,                      
                                    @varMailingAddressLine5,                       
                                    @varMailingAddressPostCode,                       
                                    CONVERT(NUMERIC,replace(@varLongitude,'     ','00000'))/100000,                      
                                    CONVERT(NUMERIC,replace(@varLatitude,'     ','00000'))/100000 ,                      
                                    @varDaytimePhoneNumber,                      
                                    @varCustomerMailStatus,                       
                                    @varNoOfHouseHoldMember, GETDATE(), 99, 'N', 'N', @varJoinedStoreID);                   
              IF @@ROWCOUNT > 0 SET @varCustInsertCtr = @varCustInsertCtr + 1;                      
              SET IDENTITY_INSERT Customer OFF;                               
                                    
              -- -----------------------------------------------------------------------------------------------------------------                      
              -- 2. Inserting CustomerAlternateID Record                      
              -- -----------------------------------------------------------------------------------------------------------------                      
              SET @varLineNo = ''                      
              INSERT INTO CustomerAlternateID (CustomerID, CustomerAlternateIDType, CustomerAlternateID, InsertDateTime, InsertBy, IsDeleted)                      
                                       VALUES (@varCustomerID, 3, @varHouseHoldNumber, GETDATE(), 99 ,'N');                      
              IF @@ROWCOUNT > 0 SET @varCAltInsertCtr = @varCAltInsertCtr + 1;                      
                                    
              -- -----------------------------------------------------------------------------------------------------------------                      
              -- 3. Inserting Customer Preferences Record                      
              -- -----------------------------------------------------------------------------------------------------------------        
                  
              --  3.1. Dietary PreferenceID (i.e. H -> 3; K -> 2; V -> 4)            
                               
              IF ( @varDietaryPreferenceID <> ' ' AND @varDietaryPreferenceID IN ( 'H', 'K', 'V') )                       
              BEGIN               
                    
                     SET @varLineNo = '246'                      
                     INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                   VALUES (@varCustomerID, (CASE WHEN @varDietaryPreferenceID = 'H' THEN 3                       
                                       WHEN @varDietaryPreferenceID = 'K' THEN 2                       
                                       WHEN @varDietaryPreferenceID = 'V' THEN 4 END), GETDATE(), 99, 'N', 1);                      
                     IF @@ROWCOUNT > 0 SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
              END; -- IF @varDietaryPreferenceID <> ' '                      
                      
              -- -----------------------------------------------------------------------------------------------------------------                      
                                    
              --  3.2. Diabetic PreferenceID; @varMedicalPreferenceID (i.e  D -> 1)                       
              IF @varMedicalPreferenceID = 'D'                      
              BEGIN                      
                     SET @varLineNo = '259'                      
                     INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                   VALUES (@varCustomerID, (CASE WHEN @varMedicalPreferenceID = 'D' THEN 1 END), GETDATE(), 99, 'N', 1);                      
                     IF @@ROWCOUNT > 0 SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
              END; -- IF @varTeeTotalPreferenceID <> ' '                      
                                    
              -- -----------------------------------------------------------------------------------------------------------------                      
                                    
              --  3.3. TeeTotal PreferenceID (i.e  Y -> 5)                       
              IF @varTeeTotalPreferenceID = 'Y'                      
              BEGIN                      
       SET @varLineNo = '270'                      
                     INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                   VALUES (@varCustomerID, (CASE WHEN @varTeeTotalPreferenceID = 'Y' THEN 5 END), GETDATE(), 99, 'N', 1);                      
                     IF @@ROWCOUNT > 0 SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
              END; -- IF @varTeeTotalPreferenceID <> ' '                      
                                
              -- -----------------------------------------------------------------------------------------------------------------                      
                                    
              --  3.4. Products PreferenceID (i.e  Y -> 7)                       
              IF @varProductsPreferenceID = 'Y'                      
              BEGIN                      
                     SET @varLineNo = '281'                      
                     INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                   VALUES (@varCustomerID, (CASE WHEN @varProductsPreferenceID = 'Y' THEN 7 END), GETDATE(), 99, 'N', 1);                      
                     IF @@ROWCOUNT > 0 SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
              END; -- IF @varProductsPreferenceID <> ' '  
                
               --LCM Changes        
    --Begin        
                   
     IF(@varProductsPreferenceID = 'Y')      
     BEGIN      
      IF((SELECT IsDeleted FROM Preference WHERE PreferenceID = 49) = 'N')       
      BEGIN      
        INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)          
        VALUES(@varCustomerID,49,GETDATE(),99,'N',1)        
        
        DELETE FROM CustomerPreferenceUpdates WHERE CustomerID = @varCustomerID AND PreferenceID = 49  
      INSERT INTO CustomerPreferenceUpdates (CustomerID, PreferenceID, InsertDateTime, InsertBy, AmendDateTime, AmendBy, IsDeleted )   
		VALUES(@varCustomerID,49,GETDATE(),99,GETDATE(),99,'N')               
      END 
        
     END      
     --End        
     --LCM Changes                        
                                    
              -- -----------------------------------------------------------------------------------------------------------------                      
            
              --  3.5. Partners PreferenceID (i.e  Y -> 8)                      
              IF @varPartnersPreferenceID = 'Y'                      
              BEGIN               
              SET @varLineNo = '292'                      
              INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                   VALUES (@varCustomerID, (CASE WHEN @varPartnersPreferenceID = 'Y' THEN 8 END), GETDATE(), 99, 'N', 1);                      
                     IF @@ROWCOUNT > 0 SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
              END; -- IF @varPartnersPreferenceID <> ' '                      
                                    
              -- -----------------------------------------------------------------------------------------------------------------                      
                     
              --  3.6.1 Customer Research PreferenceID (i.e  Y -> 9)                       
              IF @varCustResearchPreferenceID = 'Y'                      
              BEGIN                      
              SET @varLineNo = '303'                      
              INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                   VALUES (@varCustomerID, (CASE WHEN @varCustResearchPreferenceID = 'Y' THEN 9 END), GETDATE(), 99, 'N', 1);                      
           IF @@ROWCOUNT > 0 SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
              END; -- IF @varCustResearchPreferenceID <> ' '                      
                              
              -- -----------------------------------------------------------------------------------------------------------------                      
                                    
           --  3.6.2 SaveTrees PreferenceID (i.e  Y -> 16)                       
              IF @varSaveTrees = 'Y'                      
              BEGIN                      
              SET @varLineNo = '303'                      
              INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                   VALUES (@varCustomerID, (CASE WHEN @varSaveTrees = 'Y' THEN 16 END), GETDATE(), 99, 'N', 1);                      
                     IF @@ROWCOUNT > 0 SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
              END; -- IF @varSaveTrees <> ' '                      
                                    
              -- -----------------------------------------------------------------------------------------------------------------                      
                                   
              --  3.7. Regarding AirMilesPreferenceID and DealStatusPreferenceID is Pending                      
             IF (@varAirMilesPreferenceID = 'A' AND @varDealStatusPreferenceID = 'P' )                       
          BEGIN -- Set as "Airmiles Premium Customer". (i.e. PreferenceID: 12)                      
                    SET @varLineNo = '314'                      
                    INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                   VALUES (@varCustomerID, 12, GETDATE(), 99, 'N', 1);                      
                    IF @@ROWCOUNT > 0 SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
             END;                      
             ELSE IF (@varAirMilesPreferenceID = 'A' AND @varDealStatusPreferenceID = 'S' )                      
             BEGIN -- Set as "Airmiles Standard Customer". (i.e. PreferenceID: 11)                      
                    SET @varLineNo = '321'                      
                    INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                   VALUES (@varCustomerID, 11, GETDATE(), 99, 'N', 1);                      
                    IF @@ROWCOUNT > 0 SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
             END;               
                                    
             IF (@varAirMilesPreferenceID = 'B' AND @varDealStatusPreferenceID = 'S' )                      
             BEGIN -- Set as "BA Airmiles Standard Customer". (i.e. PreferenceID: 10)                      
                    SET @varLineNo = '329'                      
                    INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                   VALUES (@varCustomerID, 10, GETDATE(), 99, 'N', 1);                      
                    IF @@ROWCOUNT > 0 SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
             END;                      
             ELSE IF (@varAirMilesPreferenceID = 'B' AND @varDealStatusPreferenceID =  'P' )                      
             BEGIN -- Set as "BA Airmiles Premium Customer". (i.e. PreferenceID: 14)                      
                    SET @varLineNo = '337'                      
                    INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                   VALUES (@varCustomerID, 14, GETDATE(), 99, 'N', 1);                      
                    IF @@ROWCOUNT > 0 SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
             END;                      
                                    
              -- -----------------------------------------------------------------------------------------------------------------                      
                                    
              --  3.8. Regarding Xmas Saver                       
             IF (@varAirMilesPreferenceID = 'X')                       
             BEGIN -- Set as "Xmas Saver Customer". (i.e. PreferenceID: 13)                      
                    SET @varLineNo = '314'                      
                    INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                   VALUES (@varCustomerID, 13, GETDATE(), 99, 'N', 1);                      
                    IF @@ROWCOUNT > 0 SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
             END;          
                       
              --  3.9. Regarding Virgin Atlantic                       
             IF (@varAirMilesPreferenceID = 'V')                       
             BEGIN -- Set as "Virgin Atlantic Customer". (i.e. PreferenceID: 17)                      
                    SET @varLineNo = '315'                      
                    INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                   VALUES (@varCustomerID, 17, GETDATE(), 99, 'N', 1);                      
                    IF @@ROWCOUNT > 0 SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
             END;                     
                                    
              -- -----------------------------------------------------------------------------------------------------------------                      
              -- 4. Inserting FamilyMember Record(s)                      
              -- -----------------------------------------------------------------------------------------------------------------                      
              IF @varHHAge1 > 0   --  1. HHAge1 > 0 -- 84                      
              BEGIN                      
                   SET @varLineNo = '347'                      
                   INSERT FamilyMember(CustomerID, FamilyMemberSeqNo, DateOfBirth, InsertDateTime, InsertBy)                  
                   VALUES (@varCustomerID, 1, CONVERT(VARCHAR,DATEPART(YEAR,DATEADD(YEAR,-(CONVERT(INT,@varHHAge1)),GETDATE()))) + '/01/01', GETDATE(),99);                      
                   IF @@ROWCOUNT > 0 SET @varFamMemInsertCtr = @varFamMemInsertCtr + 1;                      
              END; -- IF @varHHAge1 > 0                      
                                    
              IF @varHHAge2 > 0   --  2. HHAge2 > 0 -- 54                      
              BEGIN                      
                   SET @varLineNo = '355'                      
                  INSERT FamilyMember(CustomerID, FamilyMemberSeqNo, DateOfBirth, InsertDateTime, InsertBy)                      
                   VALUES (@varCustomerID, 2, CONVERT(VARCHAR,DATEPART(YEAR,DATEADD(YEAR,-(CONVERT(INT,@varHHAge2)),GETDATE()))) + '/01/01', GETDATE(),99);                      
                   IF @@ROWCOUNT > 0 SET @varFamMemInsertCtr = @varFamMemInsertCtr + 1;                      
              END; -- IF @varHHAge2 > 0                      
                                    
              IF @varHHAge3 > 0   --  3. HHAge3 > 0 -- 32                      
              BEGIN                      
                   SET @varLineNo = '363'                      
                   INSERT FamilyMember(CustomerID, FamilyMemberSeqNo, DateOfBirth, InsertDateTime, InsertBy)                      
                   VALUES (@varCustomerID, 3, CONVERT(VARCHAR,DATEPART(YEAR,DATEADD(YEAR,-(CONVERT(INT,@varHHAge3)),GETDATE()))) + '/01/01', GETDATE(),99);                      
                   IF @@ROWCOUNT > 0 SET @varFamMemInsertCtr = @varFamMemInsertCtr + 1;                      
              END; -- IF @varHHAge3 > 0                      
                                    
             IF @varHHAge4 > 0   --  4. HHAge4 > 0 -- 19                      
              BEGIN                      
              SET @varLineNo = '371'                      
              INSERT FamilyMember(CustomerID, FamilyMemberSeqNo, DateOfBirth, InsertDateTime, InsertBy)                      
                     VALUES (@varCustomerID, 4, CONVERT(VARCHAR,DATEPART(YEAR,DATEADD(YEAR,-(CONVERT(INT,@varHHAge4)),GETDATE()))) + '/01/01', GETDATE(),99);                      
                     IF @@ROWCOUNT > 0 SET @varFamMemInsertCtr = @varFamMemInsertCtr + 1;                      
              END; -- IF @varHHAge4 > 0                      
                    
              IF @varHHAge5 > 0   --  5. HHAge5 > 0 -- 7                      
              BEGIN                      
                     SET @varLineNo = '379'                
                     INSERT FamilyMember(CustomerID, FamilyMemberSeqNo, DateOfBirth, InsertDateTime, InsertBy)                      
                     VALUES (@varCustomerID, 5, CONVERT(VARCHAR,DATEPART(YEAR,DATEADD(YEAR,-(CONVERT(INT,@varHHAge5)),GETDATE()))) + '/01/01', GETDATE(),99);                      
                     IF @@ROWCOUNT > 0 SET @varFamMemInsertCtr = @varFamMemInsertCtr + 1;                      
              END; -- IF @varHHAge5 > 0                      
              -- -----------------------------------------------------------------------------------------------------------------                      
       END TRY                      
       BEGIN CATCH                      
              INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Error: Inserting Customer - CustomerID : ' + CONVERT(VARCHAR,@varCustomerID) + ' @ Line no.: '  + CONVERT(VARCHAR,@varLineNo) + ERROR_MESSAGE());                      
              SET @varCustErrorCtr = @varCustErrorCtr + 1;                 
       END CATCH  -- Functionaly Part 1 : C U S T O M E R - I N S E R T  - Completing here                      
                             
       FETCH NEXT FROM cur_Customer INTO @varRecType, @varCustomerID, @varTitleEnglish, @varName3, @varName1,                      
                                             @varName2, @varSex, @varPrimaryCustomerID, @varCustomerUseStatusID, @varJoinedDate,                      
                                             @varDietaryPreferenceID, @varMedicalPreferenceID, @varTeeTotalPreferenceID, @varMailingAddressLine1,                      
                                             @varMailingAddressLine2,@varMailingAddressLine3, @varMailingAddressLine4, @varMailingAddressLine5,                      
                                             @varMailingAddressPostCode, @varDaytimePhoneNumber, @varHHAge1, @varHHAge2, @varHHAge3, @varHHAge4,                      
                                   @varHHAge5, @varNoOfHouseHoldMember, @varProductsPreferenceID, @varPartnersPreferenceID,                      
                                             @varCustResearchPreferenceID, @varPanelPreferenceID,                       
                                             @varAirMilesPreferenceID, @varDealStatusPreferenceID, @varDateOfBirth,                      
                                             @varLongitude, @varLatitude, @varPrimaryInd, @varHouseHoldNumber,@varCustomerMailStatus,@varSaveTrees,                      
                                             @varTitleFlag, @varSurnameFlag, @varFirstnameFlag, @varInitialFlag, @varDOBFlag,                      
                                             @varSexFlag, @varDietFlag, @varPhoneFlag, @varAddrFlag, @varAgeFlag,                      
                                             @varMailingStatusFlag, @varPersonInHHFlag, @varAirMilesFlag, @varJoinedStoreID, @varJoinedStoredIDFlag;                      
                  
       END; -- WHILE @@FETCH_STATUS = 0                      
       CLOSE cur_Customer;                      
       DEALLOCATE cur_Customer;                      
                             
-- =====================================================================================================================                      
--                 Part 2 : C L U B C A R D - I N S E R T STARTS FROM HERE                                            --                      
-- =====================================================================================================================                      
                             
       DECLARE cur_Clubcard CURSOR FOR SELECT * FROM TempDataUploadClubcard (nolock) WHERE RecType = 'I';                      
       OPEN cur_Clubcard                      
       FETCH NEXT FROM cur_Clubcard INTO @varCCRecType, @varCCClubcardID, @varCCCustomerID, @varCCCardIssueDate, @varCCClubcardStatus;                      
       WHILE @@FETCH_STATUS = 0                      
       BEGIN                      
              SET @varCardTotRecCtr = @varCardTotRecCtr + 1;                      
    BEGIN TRY                      
                       -- -----------------------------------------------------------------------------------------------                      
                       -- 1. Inserting Clubcard Record                      
                       -- -----------------------------------------------------------------------------------------------                      
                     SELECT @varCCClubcardType = ClubcardType                      
                     FROM ClubcardRange (nolock)                      
                     WHERE @varCCClubcardID BETWEEN MinCardNumber AND MaxCardNumber;                      
                     -- WHERE SUBSTRING(CONVERT(VARCHAR,@varCCClubcardID),1,6) BETWEEN MinCardNumber AND MaxCardNumber;                      
                                           
                     INSERT INTO Clubcard(ClubcardID, CustomerID, CardIssuedDate, ClubcardType,                      
                                          ClubcardStatus, PrimaryClubcardID, InsertDateTime, InsertBy, IsDeleted)                      
                              VALUES (@varCCClubcardID, @varCCCustomerID, CONVERT(DATETIME,@varCCCardIssueDate), @varCCClubcardType,         
                                      @varCCClubcardStatus, @varCCClubcardID, GETDATE(), 99 , 'N');                      
                
                     SET @varRowcount = @@ROWCOUNT;                      
                                           
                     IF @varRowcount > 0                       
                     BEGIN                      
                           SELECT @varCardInsertCtr = @varCardInsertCtr + 1,                      
                                  @varDotcomCustomerID = NULL,                      
                                  @varCustomerSource = NULL;                      
                                                     
                           SELECT @varDotcomCustomerID = DotcomCustomerID, @varCustomerSource = CustomerSource                      
                           FROM PendingCustomer (nolock)                      
                           WHERE ClubcardID = @varCCClubcardID;                      
                                                 
                           IF @varCustomerSource IS NOT NULL                      
                           BEGIN                      
                                  DELETE FROM PendingCustomer WHERE ClubcardID = @varCCClubcardID;               
                                  IF @varCustomerSource = 'D' OR @varCustomerSource = 'C' OR @varCustomerSource = '2' OR @varCustomerSource = '3'                       
                                  INSERT INTO CustomerAlternateID (CustomerID, CustomerAlternateIDType, CustomerAlternateID,                       
                                                                   InsertDateTime, InsertBy, IsDeleted)                      
                                       VALUES (@varCCCustomerID, 2, @varDotcomCustomerID, GETDATE(), 99 ,'N');                      
                           END;                      
                                                 
                  END;                      
              END TRY                      
              BEGIN CATCH                      
                     INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Error: Inserting Clubcard - ClubcardID : ' + CONVERT(VARCHAR,@varCCClubcardID) + ' ' + ERROR_MESSAGE());                      
                     IF @@ROWCOUNT > 0 SET @varCardErrorCtr = @varCardErrorCtr + 1;                      
              END CATCH   -- Functionaly Part 2 : C L U B C A R D - I N S E R T  - Completing here                      
              FETCH NEXT FROM cur_Clubcard INTO @varCCRecType, @varCCClubcardID, @varCCCustomerID, @varCCCardIssueDate, @varCCClubcardStatus;                      
       END; -- WHILE @@FETCH_STATUS = 0                      
       CLOSE cur_Clubcard;                      
       DEALLOCATE cur_Clubcard;                      
                             
-- =====================================================================================================================                      
--                 Part 3 : C L U B C A R D - U P D A T E STARTS FROM HERE                                            --                      
-- =====================================================================================================================                      
                             
       DECLARE cur_Clubcard CURSOR FOR SELECT * FROM TempDataUploadClubcard (nolock) WHERE RecType = 'U';                      
       OPEN cur_Clubcard                      
       FETCH NEXT FROM cur_Clubcard INTO @varCCRecType, @varCCClubcardID, @varCCCustomerID, @varCCCardIssueDate, @varCCClubcardStatus;                      
       WHILE @@FETCH_STATUS = 0                      
       BEGIN                      
              SET @varCardTotRecCtr = @varCardTotRecCtr + 1;                      
              BEGIN TRY                      
                      -- -----------------------------------------------------------------------------------------------------------------                      
                      -- 1. Update Clubcard Record                      
                      -- -----------------------------------------------------------------------------------------------------------------                      
                      UPDATE Clubcard                      
                         SET ClubcardStatus = @varCCClubcardStatus,                      
                             CardIssuedDate = CONVERT(DATETIME,@varCCCardIssueDate),                      
                             CustomerID = @varCCCustomerID,                      
                             AmendDateTime = GETDATE(), AmendBy = 99                      
                       WHERE ClubcardID = @varCCClubcardID;               
                                            
                      SET @varRowcount = @@ROWCOUNT;                      
                                            
                      IF @varRowcount =  0 -- treat there has been a serious error so alert support team                      
                      BEGIN                      
                             INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Error: Updating Clubcard - ClubcardID : ' + CONVERT(VARCHAR,@varCCClubcardID) + ' Clubcard Record Does not Exists.....' );                      
                             IF @@ROWCOUNT > 0 SET @varCardErrorCtr = @varCardErrorCtr + 1;                      
                      END;                      
                      ELSE IF @varRowcount > 0                       
                      BEGIN                      
                             SET @varCardUpdateCtr = @varCardUpdateCtr + 1;                      
                                                   
                             UPDATE NGCLIVET..PointSummary                      
                                SET MainCustomerID = @varCCCustomerID                      
                                WHERE MainClubcardID = @varCCClubcardID                      
                                  AND MainCustomerID <>@varCCCustomerID;                      
                                                   
                             UPDATE NGCLIVET..PointSummary                      
                                SET AssociateCustomerID = @varCCCustomerID                      
                                WHERE AssociateClubcardID = @varCCClubcardID                      
                                  AND AssociateCustomerID <>@varCCCustomerID;                      
                                                   
                      END;                      
              END TRY                      
              BEGIN CATCH                      
                     INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Error: Updating Clubcard - ClubcardID : ' + CONVERT(VARCHAR,@varCCClubcardID) + ' ' + ERROR_MESSAGE());                      
              END CATCH                      
              FETCH NEXT FROM cur_Clubcard INTO @varCCRecType, @varCCClubcardID, @varCCCustomerID, @varCCCardIssueDate, @varCCClubcardStatus;                      
       END; -- WHILE @@FETCH_STATUS = 0                      
       CLOSE cur_Clubcard;                      
       DEALLOCATE cur_Clubcard;                      
                             
-- =====================================================================================================================                      
--                 Part 4 : C U S T O M E R - U P D A T E STARTS FROM HERE                                            --                      
-- =====================================================================================================================                      
                             
       DECLARE cur_Customer CURSOR FOR SELECT * FROM TempDataUploadCustomer (nolock) WHERE RecType = 'U';                      
       OPEN cur_Customer                      
       FETCH NEXT FROM cur_Customer INTO @varRecType, @varCustomerID, @varTitleEnglish, @varName3, @varName1,                      
                                             @varName2, @varSex, @varPrimaryCustomerID, @varCustomerUseStatusID, @varJoinedDate,                      
 @varDietaryPreferenceID, @varMedicalPreferenceID, @varTeeTotalPreferenceID, @varMailingAddressLine1,                      
                                             @varMailingAddressLine2,@varMailingAddressLine3, @varMailingAddressLine4, @varMailingAddressLine5,                      
                                             @varMailingAddressPostCode, @varDaytimePhoneNumber, @varHHAge1, @varHHAge2, @varHHAge3, @varHHAge4,                      
                                             @varHHAge5, @varNoOfHouseHoldMember, @varProductsPreferenceID, @varPartnersPreferenceID,                       
                                             @varCustResearchPreferenceID, @varPanelPreferenceID,                       
                                             @varAirMilesPreferenceID, @varDealStatusPreferenceID, @varDateOfBirth,                      
                                             @varLongitude, @varLatitude, @varPrimaryInd, @varHouseHoldNumber,@varCustomerMailStatus,@varSaveTrees,                      
                                             @varTitleFlag, @varSurnameFlag, @varFirstnameFlag, @varInitialFlag, @varDOBFlag,                      
                                             @varSexFlag, @varDietFlag, @varPhoneFlag, @varAddrFlag, @varAgeFlag,                      
                                             @varMailingStatusFlag, @varPersonInHHFlag, @varAirMilesFlag, @varJoinedStoreID, @varJoinedStoredIDFlag;                      
       WHILE @@FETCH_STATUS = 0                      
       BEGIN                      
       SET @varCustTotRecCtr = @varCustTotRecCtr + 1;                      
       BEGIN TRY                      
                     -- -----------------------------------------------------------------------------------------------------------------                      
                     -- 1. Updating Customer Record                      
                     -- -----------------------------------------------------------------------------------------------------------------                      
                      SET @varLineNo = '981';                      
                      
                      SELECT @varExistingTitleEnglish = TitleEnglish,                      
                            @varExistingName1 = Name1,                      
                            @varExistingName2 = Name2,                      
                            @varExistingName3 = Name3,                      
                            @varExistingDateOfBirth = DateOfBirth,                      
                            @varExistingSex = Sex,                      
                            @varExistingPrimaryCustomerID = PrimaryCustomerID,                      
                            @varExistingCustomerUseStatusID = CustomerUseStatusID,                      
                            @varExistingJoinedDate = JoinedDate,                      
                            @varExistingMailingAddressLine1 = MailingAddressLine1,                      
                            @varExistingMailingAddressLine2 = MailingAddressLine2,                      
                   @varExistingMailingAddressLine3 = MailingAddressLine3,                      
                            @varExistingMailingAddressLine4 = MailingAddressLine4,                      
                            @varExistingMailingAddressLine5 = MailingAddressLine5,                      
                            @varExistingMailingAddressPostCode = MailingAddressPostCode,                      
                            @varExistingLongitude = Longitude,                      
                   @varExistingLatitude = Latitude,                      
                            @varExistingDaytimePhoneNumber = DaytimePhoneNumber,                      
                            @varExistingNoOfHouseHoldMember = NoOfHouseHoldMember,                      
                            @varExistingCustomerMailStatus = CustomerMailStatus,                  
                            @varExistingJoinedStoreID = JoinedStoreID                                                  
                      FROM Customer (nolock)                      
                      WHERE CustomerID = @varCustomerID;                      
                     IF @@ROWCOUNT = 1                     
                     BEGIN                       
                            SET @varLineNo = '1006';                      
                                                  
                            SELECT @varTitleEnglish = (CASE WHEN @varTitleFlag = 'Y' THEN @varTitleEnglish ELSE @varExistingTitleEnglish END),                      
                                   @varName1 = (CASE WHEN @varFirstnameFlag = 'Y' THEN @varName1 ELSE @varExistingName1 END),                      
                                   @varName2 = (CASE WHEN @varInitialFlag = 'Y' THEN @varName2 ELSE @varExistingName2 END),                      
                                   @varName3 = (CASE WHEN @varSurnameFlag = 'Y' THEN @varName3 ELSE @varExistingName3 END),                      
                                   @varDateOfBirth = (CASE WHEN @varDOBFlag = 'Y' THEN @varDateOfBirth ELSE CONVERT(VARCHAR(10), @varExistingDateOfBirth, 111) END),                      
                                   @varSex = (CASE WHEN @varSexFlag = 'Y' THEN @varSex ELSE @varExistingSex END),                      
                                   @varPrimaryCustomerID = (CASE WHEN LEN(@varPrimaryCustomerID) > 0 THEN @varPrimaryCustomerID ELSE @varExistingPrimaryCustomerID END),                      
     @varCustomerUseStatusID = (CASE WHEN LEN(@varCustomerUseStatusID) > 0 THEN @varCustomerUseStatusID ELSE @varExistingCustomerUseStatusID END),                      
                                   @varJoinedDate = (CASE WHEN LEN(@varJoinedDate) > 0 THEN @varJoinedDate ELSE CONVERT(VARCHAR(10),@varExistingJoinedDate,111) END),                      
                                   @varMailingAddressLine1 = (CASE WHEN @varAddrFlag = 'Y' THEN @varMailingAddressLine1 ELSE @varExistingMailingAddressLine1 END),                      
                                   @varMailingAddressLine2 = (CASE WHEN @varAddrFlag = 'Y' THEN @varMailingAddressLine2 ELSE @varExistingMailingAddressLine2 END),                      
                                   @varMailingAddressLine3 = (CASE WHEN @varAddrFlag = 'Y' THEN @varMailingAddressLine3 ELSE @varExistingMailingAddressLine3 END),                      
                                   @varMailingAddressLine4 = (CASE WHEN @varAddrFlag = 'Y' THEN @varMailingAddressLine4 ELSE @varExistingMailingAddressLine4 END),                      
       @varMailingAddressLine5 = (CASE WHEN @varAddrFlag = 'Y' THEN @varMailingAddressLine5 ELSE @varExistingMailingAddressLine5 END),                      
                                   @varMailingAddressPostCode = (CASE WHEN @varAddrFlag = 'Y' THEN @varMailingAddressPostCode ELSE @varExistingMailingAddressPostCode END),                      
                                   @varLongitude = (CASE WHEN @varAddrFlag = 'Y' THEN @varLongitude ELSE CONVERT(VARCHAR,@varExistingLongitude) END),                 
                                   @varLatitude = (CASE WHEN @varAddrFlag = 'Y' THEN @varLatitude ELSE CONVERT(VARCHAR,@varExistingLatitude) END),                      
                                   @varDaytimePhoneNumber = (CASE WHEN @varPhoneFlag = 'Y' THEN @varDaytimePhoneNumber ELSE @varExistingDaytimePhoneNumber END),                      
                                 @varCustomerMailStatus = (CASE WHEN LEN(@varCustomerMailStatus) > 0 THEN @varCustomerMailStatus ELSE @varExistingCustomerMailStatus END),                      
                  @varNoOfHouseHoldMember = (CASE WHEN @varPersonInHHFlag = 'Y' THEN @varNoOfHouseHoldMember ELSE @varExistingNoOfHouseHoldMember END),                  
                                   @varJoinedStoreID = (CASE WHEN @varJoinedStoredIDFlag = 'Y' THEN @varJoinedStoreID ELSE @varExistingJoinedStoreID END);                  
                                                                                            
                     SET @varLineNo = '1027';                      
                                           
                     END;                      
                                         
                     UPDATE Customer                      
    SET TitleEnglish = REPLACE(@varTitleEnglish, '    ', 'Unknown'),                      
                              Name3 = @varName3,                      
   Name1 = @varName1,                      
                              Name2 = @varName2,                      
                              DateOfBirth = convert(datetime, @varDateOfBirth,111),                      
                              Sex = @varSex,                      
                              PrimaryCustomerID = @varPrimaryCustomerID,                      
                              CustomerUseStatusID = @varCustomerUseStatusID,                      
                              JoinedDate = convert(datetime,@varJoinedDate,111),                      
        MailingAddressLine1 = @varMailingAddressLine1,                      
                              MailingAddressLine2 = @varMailingAddressLine2,                      
                              MailingAddressLine3 = @varMailingAddressLine3,                      
                              MailingAddressLine4 = @varMailingAddressLine4,                      
                              MailingAddressLine5 = @varMailingAddressLine5,                      
                              MailingAddressPostCode = @varMailingAddressPostCode,                      
           Longitude = CONVERT(NUMERIC,replace(@varLongitude,'     ','00000'))/100000,                      
                              Latitude = CONVERT(NUMERIC,replace(@varLatitude,'     ','00000'))/100000,                                        DaytimePhoneNumber = @varDaytimePhoneNumber,                      
                              CustomerMailStatus = @varCustomerMailStatus,                      
                              NoOfHouseHoldMember = @varNoOfHouseHoldMember,                      
                              AmendDateTime = GETDATE(),                      
                              AmendBy = 99,                  
                              JoinedStoreID=@varJoinedStoreID                  
                          WHERE CustomerID = @varCustomerID;                      
                                                
                          SET @varRowcount = @@ROWCOUNT;                      
                                                
                                           
                     IF @varRowcount =  0 -- treat there has been a serious error so alert support team                      
                     BEGIN                      
                             INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Error: Updating Customer - CustomerID : ' + CONVERT(VARCHAR,@varCustomerID) + ' Customer Record Does not Exists.....' );                      
                             SET @varCustErrorCtr = @varCustErrorCtr + 1;                      
                     END;                      
ELSE IF @varRowcount > 0                       
                     BEGIN                      
                     SET @varCustUpdateCtr = @varCustUpdateCtr + 1;                      
                      
                                                  
                     -- -----------------------------------------------------------------------------------------------------------------                      
                     -- 2. Check whether CG household no has changed for this customer, and amend as needed. (i.e. CustomerAlternateID)                      
                     -- -----------------------------------------------------------------------------------------------------------------                      
                                                  
                            UPDATE CustomerAlternateID                      
      SET CustomerAlternateID = @varHouseHoldNumber,                      
                                   AmendDateTime = GETDATE(),                      
                                   AmendBy = 99                      
                             WHERE CustomerID = @varCustomerID                      
                               AND CustomerAlternateIDType = 3                      
                               AND CustomerAlternateID <> @varHouseHoldNumber;                      
                  
                     -- -----------------------------------------------------------------------------------------------------------------                      
                     -- 3. Updating Customer Preferences Record                      
                     -- -----------------------------------------------------------------------------------------------------------------                      
                     --  3.1. Dietary PreferenceID (i.e. H -> 3; K -> 2; V -> 4)                       
                     SET @varLineNo = '1077';                      
                                           
                     IF @varDietFlag = 'Y'                       
                     BEGIN                       
                     DELETE FROM CustomerPreference WHERE CustomerID = @varCustomerID AND PreferenceID IN (2,3,4);                      
                     IF @@ROWCOUNT > 0 SET @varCustPrefDeleteCtr = @varCustPrefDeleteCtr + 1;                      
                                           
              IF ( @varDietaryPreferenceID <> ' ' AND @varDietaryPreferenceID IN ( 'H', 'K', 'V') )                       
      BEGIN                      
                            INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted,PreferenceOptStatusID)                      
                                          VALUES (@varCustomerID, (CASE WHEN @varDietaryPreferenceID = 'H' THEN 3                       
                                              WHEN @varDietaryPreferenceID = 'K' THEN 2                       
                                              WHEN @varDietaryPreferenceID = 'V' THEN 4 END), GETDATE(), 99, 'N', 1);                      
                            SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
          END; -- IF @varDietaryPreferenceID <> ' '                      
                                           
                     -- -----------------------------------------------------------------------------------------------------------------                      
                                           
                     --  3.2. Diabetic PreferenceID @varMedicalPreferenceID (i.e  D -> 1)                       
                                           
                     DELETE FROM CustomerPreference WHERE CustomerID = @varCustomerID AND PreferenceID = 1;                      
                     IF @@ROWCOUNT > 0 SET @varCustPrefDeleteCtr = @varCustPrefDeleteCtr + 1;                      
                                           
                     IF @varMedicalPreferenceID = 'D'                      
                     BEGIN                      
                            INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                          VALUES (@varCustomerID, (CASE WHEN @varMedicalPreferenceID = 'D' THEN 1 END), GETDATE(), 99, 'N',1);                      
                            SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
                     END; -- IF @varTeeTotalPreferenceID <> ' '                      
                                           
                     -- -----------------------------------------------------------------------------------------------------------------                      
                                           
                     --  3.3. TeeTotal PreferenceID (i.e  Y -> 5)                       
                                           
                     DELETE FROM CustomerPreference WHERE CustomerID = @varCustomerID AND PreferenceID = 5;                      
                     IF @@ROWCOUNT > 0 SET @varCustPrefDeleteCtr = @varCustPrefDeleteCtr + 1;                      
                                           
                     IF @varTeeTotalPreferenceID = 'Y'                      
                     BEGIN                      
                            INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                          VALUES (@varCustomerID, (CASE WHEN @varTeeTotalPreferenceID = 'Y' THEN 5 END), GETDATE(), 99, 'N', 1);                      
                            SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
                     END; -- IF @varTeeTotalPreferenceID <> ' '                      
                                    
                     END; -- IF @varDietFlag = 'Y'                       
                                           
                     -- -----------------------------------------------------------------------------------------------------------------                      
                     SET @varLineNo = '1123';                      
                     IF @varMailingStatusFlag = 'Y'                       
                     BEGIN                      
                     --  3.4. Products PreferenceID (i.e  Y -> 7)                       
                                           
                     DELETE FROM CustomerPreference WHERE CustomerID = @varCustomerID AND PreferenceID = 7;                      
                     IF @@ROWCOUNT > 0 SET @varCustPrefDeleteCtr = @varCustPrefDeleteCtr + 1;                      
                                           
                     IF @varProductsPreferenceID = 'Y'                      
                     BEGIN                      
                            INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                          VALUES (@varCustomerID, (CASE WHEN @varProductsPreferenceID = 'Y' THEN 7 END), GETDATE(), 99, 'N', 1);                      
                            SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
                     END; -- IF @varProductsPreferenceID <> ' '                      
                                           
                          
                     -- -----------------------------------------------------------------------------------------------------------------                      
                                           
                     --  3.5. Partners PreferenceID (i.e  Y -> 8)                       
                                
                     DELETE FROM CustomerPreference WHERE CustomerID = @varCustomerID AND PreferenceID = 8;                      
                     IF @@ROWCOUNT > 0 SET @varCustPrefDeleteCtr = @varCustPrefDeleteCtr + 1;                      
                                           
                     IF @varPartnersPreferenceID = 'Y'                      
                     BEGIN                      
                     INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                   VALUES (@varCustomerID, (CASE WHEN @varPartnersPreferenceID = 'Y' THEN 8 END), GETDATE(), 99, 'N', 1);                      
                            SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
                     END; -- IF @varPartnersPreferenceID <> ' '                      
                                           
                     -- -----------------------------------------------------------------------------------------------------------------                      
                                           
                     --  3.6.1 Customer Research PreferenceID (i.e  Y -> 9)                       
                                           
                     DELETE FROM CustomerPreference WHERE CustomerID = @varCustomerID AND PreferenceID = 9;                      
                     IF @@ROWCOUNT > 0 SET @varCustPrefDeleteCtr = @varCustPrefDeleteCtr + 1;                      
                                           
                     IF @varCustResearchPreferenceID = 'Y'                      
                     BEGIN                      
                     INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                          VALUES (@varCustomerID, (CASE WHEN @varCustResearchPreferenceID = 'Y' THEN 9 END), GETDATE(), 99, 'N', 1);                      
           SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
                     END; -- IF @varCustResearchPreferenceID <> ' '                      
                                           
                     -- -----------------------------------------------------------------------------------------------------------------                      
                                           
                     --  3.6.2 SaveTrees PreferenceID (i.e  Y -> 16)                 
                                     
                     DELETE FROM CustomerPreference WHERE CustomerID = @varCustomerID AND PreferenceID = 16;                      
                     IF @@ROWCOUNT > 0 SET @varCustPrefDeleteCtr = @varCustPrefDeleteCtr + 1;                      
                                           
                     IF @varSaveTrees = 'Y'                      
                     BEGIN                      
                     INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                          VALUES (@varCustomerID, (CASE WHEN @varSaveTrees = 'Y' THEN 16 END), GETDATE(), 99, 'N', 1);                      
                            SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
                     END; -- IF @varSaveTrees <> ' '                      
                                           
                     END; -- IF @varMailingStatusFlag = 'Y'                       
                     -- -----------------------------------------------------------------------------------------------------------------                      
                                           
                     --  3.7. Regarding AirMilesPreferenceID and DealStatusPreferenceID is Pending                      
                     SET @varLineNo = '1170';                      
                                           
                     IF (@varAirMilesFlag = 'Y')                        
                     BEGIN                      
                                           
                     DELETE FROM CustomerPreference WHERE CustomerID = @varCustomerID AND PreferenceID IN (10,11,12,13,14,17);                      
                     IF @@ROWCOUNT > 0                       
                     BEGIN                      
                           SET @varCustPrefDeleteCtr = @varCustPrefDeleteCtr + @varLogText;                      
END;                      
                                           
      IF (@varAirMilesPreferenceID = 'A' AND @varDealStatusPreferenceID = 'P')                       
                    BEGIN -- Set as "Airmiles Premium Customer". (i.e. PreferenceID: 12)                      
                                                 
                           INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                          VALUES (@varCustomerID, 12, GETDATE(), 99, 'N', 1);                      
                           SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
                    END;                      
                    ELSE IF (@varAirMilesPreferenceID = 'A' AND @varDealStatusPreferenceID = 'S')                      
                    BEGIN -- Set as "Airmiles Standard Customer". (i.e. PreferenceID: 11)                      
                                                 
                           INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                          VALUES (@varCustomerID, 11, GETDATE(), 99, 'N', 1);                      
                           SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
                    END;                      
                                           
                    IF (@varAirMilesPreferenceID = 'B' AND @varDealStatusPreferenceID = 'S' )                      
                    BEGIN -- Set as "BA Airmiles Standard Customer". (i.e. PreferenceID: 10)                      
                           
                           INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                          VALUES (@varCustomerID, 10, GETDATE(), 99, 'N', 1);                      
                           SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
                    END;                      
                    ELSE IF (@varAirMilesPreferenceID = 'B' AND @varDealStatusPreferenceID =  'P')                      
                    BEGIN -- Set as "BA Airmiles Premium Customer". (i.e. PreferenceID: 14)                      
                   
                           INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                          VALUES (@varCustomerID, 14, GETDATE(), 99, 'N', 1);                      
                           SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
                    END;                      
                                           
                     -- -----------------------------------------------------------------------------------------------------------------                      
                                           
                     --  3.8. Regarding Xmas Saver                       
            IF (@varAirMilesPreferenceID = 'X')                       
                    BEGIN -- Set as "Xmas Saver Customer". (i.e. PreferenceID: 13)                      
                           SET @varLineNo = '314'                      
                           INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                          VALUES (@varCustomerID, 13, GETDATE(), 99, 'N', 1);                      
                           SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
                    END;                      
                                           
                           
                              
                    --  3.9. Regarding Virgin Atlantic                       
            IF (@varAirMilesPreferenceID = 'V')             
                    BEGIN -- Set as "Virgin Atlantic Customer". (i.e. PreferenceID: 17)                      
                           SET @varLineNo = '315'                      
                           INSERT CustomerPreference (CustomerID, PreferenceID, InsertDateTime, InsertBy, IsDeleted, PreferenceOptStatusID)                      
                                          VALUES (@varCustomerID, 17, GETDATE(), 99, 'N', 1);                      
                           SET @varCustPrefInsertCtr = @varCustPrefInsertCtr + 1;                      
                    END;                      
                                           
                    END; -- IF (@varAirMilesFlag = 'Y')                       
                          
                         
                     -- -----------------------------------------------------------------------------------------------------------------                      
                     -- 4. Updating FamilyMember Record(s)                      
                     -- -----------------------------------------------------------------------------------------------------------------                      
                     SET @varLineNo = '1221';                      
                     IF @varAgeFlag = 'Y'                       
 BEGIN                      
                     DELETE FROM FamilyMember WHERE CustomerID = @varCustomerID;                      
                     IF @@ROWCOUNT > 0 SET @varFamMemDeleteCtr  = @varFamMemDeleteCtr + @@ROWCOUNT;                      
                                           
                     IF @varHHAge1 > 0   --  1. HHAge1 > 0                       
                     BEGIN                      
                          INSERT FamilyMember(CustomerID, FamilyMemberSeqNo, DateOfBirth, InsertDateTime, InsertBy)        
                          VALUES (@varCustomerID, 1, CONVERT(VARCHAR,DATEPART(YEAR,DATEADD(YEAR,-(CONVERT(INT,@varHHAge1)),GETDATE()))) + '/01/01', GETDATE(),99);                      
                          SET @varFamMemInsertCtr = @varFamMemInsertCtr + 1;                      
                     END; -- IF @varHHAge1 > 0                      
                                           
                     IF @varHHAge2 > 0   --  2. HHAge2 > 0                       
         BEGIN                      
                          INSERT FamilyMember(CustomerID, FamilyMemberSeqNo, DateOfBirth, InsertDateTime, InsertBy)                      
                          VALUES (@varCustomerID, 2, CONVERT(VARCHAR,DATEPART(YEAR,DATEADD(YEAR,-(CONVERT(INT,@varHHAge2)),GETDATE()))) + '/01/01', GETDATE(),99);                      
                          SET @varFamMemInsertCtr = @varFamMemInsertCtr + 1;                      
                     END; -- IF @varHHAge2 > 0                      
                                           
                     IF @varHHAge3 > 0   --  3. HHAge3 > 0                       
                     BEGIN                      
                          INSERT FamilyMember(CustomerID, FamilyMemberSeqNo, DateOfBirth, InsertDateTime, InsertBy)                      
                          VALUES (@varCustomerID, 3, CONVERT(VARCHAR,DATEPART(YEAR,DATEADD(YEAR,-(CONVERT(INT,@varHHAge3)),GETDATE()))) + '/01/01', GETDATE(),99);                      
                          SET @varFamMemInsertCtr = @varFamMemInsertCtr + 1;                      
                     END; -- IF @varHHAge3 > 0                      
                                           
                     IF @varHHAge4 > 0   --  4. HHAge4 > 0                       
                     BEGIN                      
                          INSERT FamilyMember(CustomerID, FamilyMemberSeqNo, DateOfBirth, InsertDateTime, InsertBy)                      
                          VALUES (@varCustomerID, 4, CONVERT(VARCHAR,DATEPART(YEAR,DATEADD(YEAR,-(CONVERT(INT,@varHHAge4)),GETDATE()))) + '/01/01', GETDATE(),99);                      
                          SET @varFamMemInsertCtr = @varFamMemInsertCtr + 1;                      
                     END; -- IF @varHHAge4 > 0                      
                                           
                     IF @varHHAge5 > 0   --  5. HHAge5 > 0                       
                     BEGIN                     
                          INSERT FamilyMember(CustomerID, FamilyMemberSeqNo, DateOfBirth, InsertDateTime, InsertBy)                      
                          VALUES (@varCustomerID, 5, CONVERT(VARCHAR,DATEPART(YEAR,DATEADD(YEAR,-(CONVERT(INT,@varHHAge5)),GETDATE()))) + '/01/01', GETDATE(),99);                      
                          SET @varFamMemInsertCtr = @varFamMemInsertCtr + 1;                      
                     END; -- IF @varHHAge5 > 0                      
                     END; -- IF @varAgeFlag = 'Y'                       
                     -- -----------------------------------------------------------------------------------------------------------------                      
              END -- ELSE IF @@ROWCOUNT > 0                        
       END TRY                      
       BEGIN CATCH                      
              INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Error: Updating Customer - CustomerID : ' + @varLineNo + ' : ' + CONVERT(VARCHAR,@varCustomerID) + ' ' + ERROR_MESSAGE());                      
              SET @varCustErrorCtr = @varCustErrorCtr + 1;                      
       END CATCH                      
                             
       FETCH NEXT FROM cur_Customer INTO @varRecType, @varCustomerID, @varTitleEnglish, @varName3, @varName1,                      
                                             @varName2, @varSex, @varPrimaryCustomerID, @varCustomerUseStatusID, @varJoinedDate,                      
                                             @varDietaryPreferenceID, @varMedicalPreferenceID, @varTeeTotalPreferenceID, @varMailingAddressLine1,                      
                                             @varMailingAddressLine2,@varMailingAddressLine3, @varMailingAddressLine4, @varMailingAddressLine5,                      
                                             @varMailingAddressPostCode, @varDaytimePhoneNumber, @varHHAge1, @varHHAge2, @varHHAge3, @varHHAge4,                      
                                             @varHHAge5, @varNoOfHouseHoldMember, @varProductsPreferenceID, @varPartnersPreferenceID,                      
           @varCustResearchPreferenceID, @varPanelPreferenceID,                       
                                             @varAirMilesPreferenceID, @varDealStatusPreferenceID, @varDateOfBirth,                      
                                             @varLongitude, @varLatitude, @varPrimaryInd, @varHouseHoldNumber,@varCustomerMailStatus,@varSaveTrees,                      
                                             @varTitleFlag, @varSurnameFlag, @varFirstnameFlag, @varInitialFlag, @varDOBFlag,                      
                                             @varSexFlag, @varDietFlag, @varPhoneFlag, @varAddrFlag, @varAgeFlag,                      
                                             @varMailingStatusFlag, @varPersonInHHFlag, @varAirMilesFlag, @varJoinedStoreID, @varJoinedStoredIDFlag;                      
                             
       END; -- WHILE @@FETCH_STATUS = 0                      
       CLOSE cur_Customer;                      
       DEALLOCATE cur_Customer;                      
                             
-- =====================================================================================================================                      
--                 Part 5 : C L U B C A R D - D E L E T E STARTS FROM HERE                                            --                      
-- =====================================================================================================================                      
                             
       DECLARE cur_Clubcard CURSOR FOR SELECT * FROM TempDataUploadClubcard (nolock) WHERE RecType = 'D';              
       OPEN cur_Clubcard                      
       FETCH NEXT FROM cur_Clubcard INTO @varCCRecType, @varCCClubcardID, @varCCCustomerID, @varCCCardIssueDate, @varCCClubcardStatus;                      
       WHILE @@FETCH_STATUS = 0                      
       BEGIN                      
              SET @varCardTotRecCtr = @varCardTotRecCtr + 1;                      
              BEGIN TRY                      
                     -- -----------------------------------------------------------------------------------------------------------------                      
                     -- 1. Delete Clubcard Record                      
                     -- -----------------------------------------------------------------------------------------------------------------                      
          DELETE FROM Clubcard WHERE ClubcardID = @varCCClubcardID;                      
                     IF @@ROWCOUNT = 0 -- treat there has been a serious error so alert support team                      
                     BEGIN                      
                            INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Error: Deleting Clubcard - ClubcardID : ' + CONVERT(VARCHAR,@varCCClubcardID) + ' Clubcard Record Does not in Clubcard Table ..' );                      
                            SET @varCardErrorCtr = @varCardErrorCtr + 1;                      
                     END;                      
                     IF @@ROWCOUNT > 0 SET @varCardDeleteCtr = @varCardDeleteCtr + 1;                      
              END TRY                      
              BEGIN CATCH                      
                     INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Error: Deleting Clubcard - ClubcardID : ' + CONVERT(VARCHAR,@varCCClubcardID) + ' ' + ERROR_MESSAGE());                      
              END CATCH                      
              FETCH NEXT FROM cur_Clubcard INTO @varCCRecType, @varCCClubcardID, @varCCCustomerID, @varCCCardIssueDate, @varCCClubcardStatus;                      
       END; -- WHILE @@FETCH_STATUS = 0                      
       CLOSE cur_Clubcard;                      
       DEALLOCATE cur_Clubcard;                      
                             
-- =====================================================================================================================                      
--            Part 6 : C U S T O M E R - D E L E T E   STARTS FROM HERE                                            --                      
-- =====================================================================================================================                      
                             
       DECLARE cur_Customer CURSOR FOR SELECT * FROM TempDataUploadCustomer (nolock) WHERE RecType = 'D';                      
       OPEN cur_Customer                      
       FETCH NEXT FROM cur_Customer INTO @varRecType, @varCustomerID, @varTitleEnglish, @varName3, @varName1,                      
                                             @varName2, @varSex, @varPrimaryCustomerID, @varCustomerUseStatusID, @varJoinedDate,                      
                                             @varDietaryPreferenceID, @varMedicalPreferenceID, @varTeeTotalPreferenceID, @varMailingAddressLine1,                      
                                             @varMailingAddressLine2,@varMailingAddressLine3, @varMailingAddressLine4, @varMailingAddressLine5,                      
                                             @varMailingAddressPostCode, @varDaytimePhoneNumber, @varHHAge1, @varHHAge2, @varHHAge3, @varHHAge4,                      
                                             @varHHAge5, @varNoOfHouseHoldMember, @varProductsPreferenceID, @varPartnersPreferenceID,                      
                                    @varCustResearchPreferenceID, @varPanelPreferenceID,                      
                                             @varAirMilesPreferenceID, @varDealStatusPreferenceID, @varDateOfBirth,                      
                                             @varLongitude, @varLatitude, @varPrimaryInd, @varHouseHoldNumber,@varCustomerMailStatus,@varSaveTrees,                      
                                             @varTitleFlag, @varSurnameFlag, @varFirstnameFlag, @varInitialFlag, @varDOBFlag,                      
                                             @varSexFlag, @varDietFlag, @varPhoneFlag, @varAddrFlag, @varAgeFlag,                      
                                             @varMailingStatusFlag, @varPersonInHHFlag, @varAirMilesFlag, @varJoinedStoreID, @varJoinedStoredIDFlag;                      
       WHILE @@FETCH_STATUS = 0                      
       BEGIN                      
       SET @varCustTotRecCtr = @varCustTotRecCtr + 1;                      
       BEGIN TRY                      
              -- -----------------------------------------------------------------------------------------------------------------                      
              -- 1. Deleting Customer Record                      
              -- -----------------------------------------------------------------------------------------------------------------                      
              DELETE FROM CustomerPreference WHERE CustomerID = @varCustomerID;                      
              IF @@ROWCOUNT > 0 SET @varCustPrefDeleteCtr = @varCustPrefDeleteCtr + @@ROWCOUNT;                      
                      
              DELETE FROM CustomerAlternateID WHERE CustomerID = @varCustomerID;                      
              IF @@ROWCOUNT > 0 SET @varCAltDeleteCtr = @varCAltDeleteCtr + @@ROWCOUNT;                      
                      
              DELETE FROM FamilyMember WHERE CustomerID = @varCustomerID;                      
              IF @@ROWCOUNT > 0 SET @varFamMemDeleteCtr  = @varFamMemDeleteCtr + @@ROWCOUNT;                      
                      
              DELETE FROM Customer WHERE CustomerID = @varCustomerID;                      
              IF @@ROWCOUNT = 0 -- treat as "there has been a serious error so alert support team (??)".                      
              BEGIN                      
                     INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Error: While Deleting the Customer the respective CustomerID ( ' +                       
                                                                  CONVERT(VARCHAR,@varCustomerID) + ' ) not exists in Customer Table.' );                      
    SET @varCustErrorCtr = @varCustErrorCtr + 1;                      
              END;                 
              ELSE IF @@ROWCOUNT > 0 SET @varCustDeleteCtr = @varCustDeleteCtr + @@ROWCOUNT;                      
       END TRY                      
       BEGIN CATCH                      
             INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Error: Deleting Customer - CustomerID : ' + CONVERT(VARCHAR,@varCustomerID) + ' ' + ERROR_MESSAGE());                      
              SET @varCustErrorCtr = @varCustErrorCtr + 1;                      
       END CATCH                      
                             
       FETCH NEXT FROM cur_Customer INTO @varRecType, @varCustomerID, @varTitleEnglish, @varName3, @varName1,                      
                                             @varName2, @varSex, @varPrimaryCustomerID, @varCustomerUseStatusID, @varJoinedDate,                      
                                             @varDietaryPreferenceID, @varMedicalPreferenceID, @varTeeTotalPreferenceID, @varMailingAddressLine1,                      
                                             @varMailingAddressLine2,@varMailingAddressLine3, @varMailingAddressLine4, @varMailingAddressLine5,                      
 @varMailingAddressPostCode, @varDaytimePhoneNumber, @varHHAge1, @varHHAge2, @varHHAge3, @varHHAge4,                      
                                             @varHHAge5, @varNoOfHouseHoldMember, @varProductsPreferenceID, @varPartnersPreferenceID,                      
                                             @varCustResearchPreferenceID, @varPanelPreferenceID,                      
                                             @varAirMilesPreferenceID, @varDealStatusPreferenceID, @varDateOfBirth,                      
                                             @varLongitude, @varLatitude, @varPrimaryInd, @varHouseHoldNumber,@varCustomerMailStatus,@varSaveTrees,                      
                                             @varTitleFlag, @varSurnameFlag, @varFirstnameFlag, @varInitialFlag, @varDOBFlag,                      
                                             @varSexFlag, @varDietFlag, @varPhoneFlag, @varAddrFlag, @varAgeFlag,                      
                                             @varMailingStatusFlag, @varPersonInHHFlag, @varAirMilesFlag, @varJoinedStoreID, @varJoinedStoredIDFlag;                      
                             
       END; -- WHILE @@FETCH_STATUS = 0                      
       CLOSE cur_Customer;                      
       DEALLOCATE cur_Customer;                      
                             
       UPDATE Tcust                      
        SET TitleEnglish = Tit.TitleEnglish                      
         FROM (SELECT Cust.CustomerID,Cust.TitleEnglish                      
                 FROM Customer Cust,TempDataUploadCustomer TDU                      
                WHERE Cust.CustomerID = TDU.CustomerID) Tcust, Title Tit                      
         WHERE Tcust.TitleEnglish = Tit.TitleCG                       
                             
-- =====================================================================================================================                      
                             
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Customer : Total No.of Record(s) Processed     : ' + CONVERT(VARCHAR,@varCustTotRecCtr));                      
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Customer : No.of Record(s) Inserted            : ' + CONVERT(VARCHAR,@varCustInsertCtr));                      
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Customer : No.of Record(s) Updated             : ' + CONVERT(VARCHAR,@varCustUpdateCtr));                      
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Customer : No.of Record(s) Deleted             : ' + CONVERT(VARCHAR,@varCustDeleteCtr));                      
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Customer : No.of Record(s) Having Error        : ' + CONVERT(VARCHAR,@varCustErrorCtr));                      
                             
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Clubcard : Total No.of Record(s) Processed     : ' + CONVERT(VARCHAR,@varCardTotRecCtr));                      
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Clubcard : No.of Record(s) Inserted            : ' + CONVERT(VARCHAR,@varCardInsertCtr));                      
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Clubcard : No.of Record(s) Updated             : ' + CONVERT(VARCHAR,@varCardUpdateCtr));                      
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Clubcard : No.of Record(s) Deleted             : ' + CONVERT(VARCHAR,@varCardDeleteCtr));                      
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Clubcard : No.of Record(s) Having Error        : ' + CONVERT(VARCHAR,@varCardErrorCtr));                      
                             
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('CustomerAlternateID : No.of Record(s) Inserted : ' + CONVERT(VARCHAR,@varCAltInsertCtr));                      
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('CustomerAlternateID : No.of Record(s) Deleted  : ' + CONVERT(VARCHAR,@varCAltDeleteCtr));                      
                             
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('CustomerPreference : No.of Record(s) Inserted  : ' + CONVERT(VARCHAR,@varCustPrefInsertCtr));                      
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('CustomerPreference : No.of Record(s) Deleted   : ' + CONVERT(VARCHAR,@varCustPrefDeleteCtr));                      
                             
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('FamilyMember : Total No.of Record(s) Inserted  : ' + CONVERT(VARCHAR,@varFamMemInsertCtr));                      
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('FamilyMember : Total No.of Record(s) Deleted   : ' + CONVERT(VARCHAR,@varFamMemDeleteCtr));                      
                             
       INSERT INTO TempDataUploadLog(LOGTEXT) VALUES ('Customer and Clubcard Data Upload Ending Time : ' + CONVERT(VARCHAR(25), GETDATE(), 131) );                      
                             
-- =====================================================================================================================                      
                      
       SET NOCOUNT OFF;                      
       END TRY                        
       BEGIN CATCH                        
              DECLARE @vErrMsg      NVARCHAR(4000)                        
              DECLARE @vErrSeverity INT                        
              DECLARE @vErrState    INT                  
               SELECT @vErrMsg = ' While Executing USP_Upload_CustomerClubcardData ..... ' + ERROR_MESSAGE(),                        
                     @vErrSeverity = ERROR_SEVERITY(),                        
                     @vErrState = ERROR_STATE();                        
              RAISERROR (@vErrMsg, @vErrSeverity, @vErrState );                        
       END CATCH                        
END; -- [USP_Upload_CustomerClubcardData]       
      
      

GO


