/****** Object:  StoredProcedure [dbo].[USP_CreateCustomer]    Script Date: 12/12/2012 17:09:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_CreateCustomer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_CreateCustomer]
GO

/****** Object:  StoredProcedure [dbo].[USP_CreateCustomer]    Script Date: 12/12/2012 17:09:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[USP_CreateCustomer](@pDotcomCustomerID        BIGINT,                                          
                                                   @pTitleEnglish           VARCHAR(20),                                          
                                                   @pName1                  NVARCHAR(200),                                          
                                                   @pName2                  NVARCHAR(200),                                          
                                                   @pName3                  NVARCHAR(200),                                          
                                                   @pDateOfBirth            DATETIME,                                          
                                                   @pSex                    NCHAR(2),                                          
                                                   @pMailingAddressLine1    NVARCHAR(320),                                          
                                                   @pMailingAddressLine2    NVARCHAR(320),                                          
                                                   @pMailingAddressLine3    NVARCHAR(320),                                          
                                                   @pMailingAddressLine4    NVARCHAR(320),                                          
                                                   @pMailingAddressLine5    NVARCHAR(320),                                          
                                                   @pMailingAddressLine6    NVARCHAR(320),                                          
                                                   @pMailingAddressPostCode NVARCHAR(40),                                          
                                                   @pIsDiabetic             NCHAR(2),                                          
                                                   @pIsVegetarian           NCHAR(2),                                          
                                                   @pIsTeeTotal             NCHAR(2),                                          
                                                   @pIsKosher               NCHAR(2),                                          
                                                   @pIsHalal                NCHAR(2),                                          
                                                   --@pWantTescoInfo          NCHAR(2),                                          
                                                   --@pWantPartnerInfo        NCHAR(2),                                          
                                                   --@pIsResearchContactable  NCHAR(2),                     
                                                   @pTescoGroupMail NCHAR(2),                    
                                                   @pTescoGroupEmail NCHAR(2),                    
                                                   @pTescoGroupPhone NCHAR(2),                    
                                                    @pTescoGroupSms NCHAR(2),                    
                                                   @pPartnerMail NCHAR(2),                    
                                                   @pPartnerEmail NCHAR(2),                    
                                                   @pPartnerPhone NCHAR(2),                    
                                                   @pPartnerSms NCHAR(2),                    
                                                   @pResearchMail NCHAR(2),                    
                                                   @pResearchEmail NCHAR(2),                    
                                                   @pResearchPhone  NCHAR(2),                    
                                                   @pResearchSms  NCHAR(2),                   
                             @pRace        NVARCHAR(10),     
                                                   @pLanguage    NVARCHAR(20),                             
          @pIsContactable          NCHAR(2),               
                                                   @pEmailAddress         NVARCHAR(640),                                          
               @pDayContactNumber       VARCHAR(40),                                 
                                                   @pEveningContactNumber   VARCHAR(40),                                          
                                                   @pMobileContactNumber    VARCHAR(40),                                          
                                                   @pNumberOfPeople         TINYINT,                                          
                                                   @pAge1                   TINYINT,                                          
                                                   @pAge2                   TINYINT,                                          
                                                   @pAge3                   TINYINT,                                          
                                                   @pAge4                   TINYINT,               
                                                   @pAge5                   TINYINT,                                           
                                                   @pCustomerSource         NCHAR(8),                                          
                                                   @pIsDuplicate            NCHAR(2),                                          
                                                   @pInsertBy               SMALLINT,                                      
                                                   @pExpat                   CHAR(1),                                      
                                                   @pPromotionCode           NVARCHAR(20),                          
                                                   @pSSN         VARCHAR(100),                  
                                                   @pPassport      VARCHAR(100),          
               @pJoinedStoreID           INT,              
               @DynamicPreferences   VARCHAR(100),        
               @CustomerUseStatusID VARCHAR(100),        
               @CustomerMailStatus VARCHAR(100),        
               @CustomerMobilePhoneStatus VARCHAR(100),        
               @CustomerEmailStatus VARCHAR(100))                                         
AS                  ---------------------------------------------------------------------------------------------------                                          
------                                          
------ Copyrights (C) 2009, Tesco HSC Pte Ltd,81-82, EPIP Area, WhiteFiled, Bangalore-66                                          
------ All rights reserved                                          
------                                           
------ Current Ver : 1.0                                          
------                                          
------ Version: 1.0       Author : P. Suresh Kumar     Date : 2010-May-21 Friday                                           
------ Description : Initial version                                          
------                                           
------ Title :  [USP_CreatePendingCustomer]                                          
------ Purpose :                                           
------ Pre-requisite :                                           
------                                           
--------------------------------------------------------------------------------------------------                                          
------ Procedure        : [USP_CreatePendingCustomer]                                          
------                  :                                           
------ Input Parameter  : @pDotcomCustomerID, @pTitleEnglish, @pName1, @pName2, @pName3 , @pDateOfBirth, @pSex,          
------                  : @pMailingAddressLine1, @pMailingAddressLine2, @pMailingAddressLine3, @pMailingAddressLine4,         
------                  : @pMailingAddressLine5, @pMailingAddressLine6, @pMailingAddressPostCode, @pIsDiabetic,                                        
------                  : @pIsVegetarian, @pIsTeeTotal, @pIsKosher, @pIsHalal, @pWantTescoInfo, @pWantPartnerInfo,                                           
------                  : @pIsResearchContactable, @pIsContactable, @pEmailAddress, @pDayContactNumber,                                           
------         : @pEveningContactNumber, @pMobileContactNumber, @pNumberOfPeople, @pAge1, @pAge2, @pAge3,                                          
------ : @pAge4, @pCustomerSource, @pIsDuplicate, @pInsertDateTime, @pInsertBy, @pAmendDateTime, @pAmendBy,                                          
------                  : @pIsDeleted                                          
------                  :                                           
------ Output Parameter : @pClubcardID, @pExecutionStatus (i.e if 0 = Success. if Error then <> 0 ) , @pExeStatusMessage                          
------                  :                                           
------ Description      : This PROCEDURE IS used TO INSERT the records IN PendingCustomer (NEW NGG CCO Table).                                          
------                  :                                           
------ Remark           : The ClubcardNumber will be retrieved by calling the stored procedure USP_GetClubcardNumber                                           
------                  : which will accept the Clubcard Type as the input parameter and will return the Clubcard Number.                                          
------           :                                           
------                  : The Clubcard Type will be derived from the CustomerSource parameter as follows:-                                          
------                  :                                           
------                  : If the CustomerSource is ‘D’ then the ClubcardType should be retrieved from the ClubcardType table                                           
------                  : where the ClubcardTypeDescEnglish is ‘Online Dotcom’. A new row will be required for this ClubcardType.                                          
------                  :                                           
------                  : If the CustomerSource is ‘C’ then the ClubcardType should be retrieved from the ClubcardType table                                           
------                  : where the ClubcardTypeDescEnglish is ‘Online Standard’. A new row will be required for this ClubcardType.                                          
------                  :                                           
------ How to Execute   :                                           
------                  : DELETE FROM PendingCustomer;                                          
/*                                           
BEGIN                                
                                                
       EXEC [USP_CreateCustomer] @pDotcomCustomerID = 9237,                                          
        @pTitleEnglish = 'MR',                                          
                                                @pName1= 'JOHN1',                                          
                                              @pName2 = null, -- 'W',                                          
                                                @pName3 = null, -- 'KEIGHLEY',                              
                                                @pDateOfBirth = NULL,                                          
                                                @pSex = 'M',                                          
                     @pMailingAddressLine1 = '3 WOODLAND AVENUE' ,                                          
                                                @pMailingAddressLine2 = null, -- 'HAGLEY',                                           
                                                @pMailingAddressLine3 = null, -- 'STOURBRIDGE' ,                                           
                                       @pMailingAddressLine4 = null, -- 'WEST MIDLANDS',                                           
                                                @pMailingAddressLine5 = null, -- 'WEST MIDLANDS' ,                                           
@pMailingAddressLine6 = NULL,                                           
                                                @pMailingAddressPostCode = 'DY8 2XQ',                                           
                                                @pIsDiabetic = NULL ,                                           
   @pIsVegetarian = NULL ,                                          
                                                @pIsTeeTotal = NULL ,                                           
                                                @pIsKosher = NULL ,                                           
                                                @pIsHalal = NULL ,                                           
                                                @pWantTescoInfo = NULL ,                                       
                                                @pWantPartnerInfo = NULL ,                                           
                                                @pIsResearchContactable = NULL ,                                           
                                                @pIsContactable = NULL ,                                    
                                                @pEmailAddress = NULL ,                                          
                                                @pDayContactNumber = NULL ,                                           
            @pEveningContactNumber = NULL ,                                           
                                                @pMobileContactNumber = NULL ,                                           
                                                @pNumberOfPeople = NULL ,                                          
                                                @pAge1 = 22 ,                                           
                                                @pAge2 = 22 ,                                           
                                                @pAge3 = 32 ,                                           
                                                @pAge4 = 45 ,                                          
                                                @pCustomerSource = '1' ,                                           
                                                @pIsDuplicate = NULL ,                                            
                                                @pInsertBy = 99,                                    
                                                @pExpat = 'Y',                                      
                                                   @pPromotionCode = 'Addf123',                                      
                                                   @pSSN  = '123444',                                      
                                                   @pJoinedStoreID=12233;                                          
       -- SELECT * FROM PendingCustomer;                                          
END;                                          
*/                                          
------=============================================================================================                                          
BEGIN                                          
       SET NOCOUNT ON                                          
       DECLARE @varClubcardID       BIGINT;                                          
       DECLARE @pExecutionStatus   INT;                                          
       DECLARE @pExeStatusMessage   NVARCHAR(200);                                         
       DECLARE @varCtr           INT;                                          
       DECLARE @RowId    BIGINT;                              
       DECLARE @JoinRoute   SMALLINT;
       DECLARE @PreferenceID NVARCHAR(100);                                                        
       BEGIN TRY                                          
              SET @JoinRoute = CONVERT(INT, @pCustomerSource)                                      
              EXEC [USP_GetClubcardNumber] @JoinRoute, @varClubcardID OUTPUT                                       
                                      
              SET @pExecutionStatus = 0;                            
              SET @pExeStatusMessage = NULL;                                          
              INSERT INTO Customer (TitleEnglish, Name1, Name2, Name3, DateOfBirth,                                           
                                           Sex, MailingAddressLine1, MailingAddressLine2, MailingAddressLine3, MailingAddressLine4,                                           
                                           MailingAddressLine5, MailingAddressLine6, MailingAddressPostCode, CustomerUseStatusID, CustomerMailStatus, CustomerMobilePhoneStatus, CustomerEmailStatus,                                
                                           EmailAddress,ISOLanguageCode,RaceID,DaytimePhoneNumber, EveningPhoneNumber, MobilePhoneNumber, InsertDateTime,        
                                           JoinedDate,InsertBy,AmendDateTime,AmendBy,IsDeleted,        
                                           JoinedStoreID, JoinPromotionCode, JoinRouteID, CustomerWelcomedFlag, NoOfHouseHoldMember,PassportNo,SSN)                                          
                                   VALUES (@pTitleEnglish,               
                              @pName1,@pName2,@pName3,                                                                                   
                                           @pDateOfBirth,                                          
                                           @pSex,                                         
                                           @pMailingAddressLine1,@pMailingAddressLine2,                 
                                           @pMailingAddressLine3,@pMailingAddressLine4,@pMailingAddressLine5,                                        
                                           @pMailingAddressLine6, @pMailingAddressPostCode,@CustomerUseStatusID,@CustomerMailStatus,@CustomerMobilePhoneStatus,@CustomerEmailStatus,                                  
                                           @pEmailAddress,@pLanguage,@pRace,@pDayContactNumber, @pEveningContactNumber, @pMobileContactNumber,                                        
                                           GETDATE() ,GETDATE(), @pInsertBy,                                          
                                           NULL, NULL,'N',@pJoinedStoreID,@pPromotionCode, @JoinRoute,'N', @pNumberOfPeople,@pPassport,@pSSN);                                          
              SET @RowId = SCOPE_IDENTITY();                            
              UPDATE customer set PrimaryCustomerID=@RowId where CustomerID=@RowId                           
              INSERT INTO Clubcard (ClubcardID, CustomerID, ClubcardType, ClubcardStatus, PrimaryClubcardID, CardIssuedDate, InsertDateTime,                                          
                                           InsertBy,AmendDateTime,AmendBy,IsDeleted)                                          
                                   VALUES (@varClubcardID,                                      
                                           @RowId,1,1,   --@RowId,@pCustomerSource,'1',                     
                                           @varClubcardID,                                          
                                           GETDATE(),                                         
                                           GETDATE() , @pInsertBy,                                          
                                    NULL, NULL,'N');                                
                
               
 -- Update Dynamic Dietary Pref From Join Page              
 -- EXEC [USP_UpdateDynamicDietaryPreferences] @RowId,@DynamicPreferences,@pInsertBy
 
WHILE LEN(@DynamicPreferences) > 0            
   BEGIN            
      SET @PreferenceID = LEFT(@DynamicPreferences,ISNULL(NULLIF(CHARINDEX(',', @DynamicPreferences) - 1, -1),LEN(@DynamicPreferences)))                     
      SET @DynamicPreferences = SUBSTRING(@DynamicPreferences,ISNULL(NULLIF(CHARINDEX(',', @DynamicPreferences), 0),LEN(@DynamicPreferences)) + 1, LEN(@DynamicPreferences))            
            
     INSERT INTO CustomerPreference               
     (CustomerID,PreferenceID,PrimaryPreferenceOfTypeInd,PreferenceOfTypeSeqNo,InsertDateTime,InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)              
     VALUES(@RowId,@PreferenceID  ,NULL,NULL,Getdate(),@pInsertBy,Getdate(),@pInsertBy,'N',1)          
               
     INSERT into CustomerPreferenceUpdates 
     (CustomerID, PreferenceID, InsertDateTime, InsertBy, AmendDateTime, AmendBy, IsDeleted )           
     VALUES (@RowId, @PreferenceID,Getdate(),@pInsertBy,Getdate(),@pInsertBy,'N')          
   END       
                                            
 
 
                                   
 IF  @pTescoGroupMail ='Y'                   
    INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
    VALUES (@RowId, 27, GETDATE() , @pInsertBy, NULL, NULL,'N',1)                                
                       
 IF  @pTescoGroupEmail ='Y'                                
    INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
    VALUES (@RowId, 28, GETDATE() , @pInsertBy, NULL, NULL,'N',1)                         
                         
    IF  @pTescoGroupPhone ='Y'                                
    INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
    VALUES (@RowId, 29, GETDATE() , @pInsertBy, NULL, NULL,'N',1)                       
                                               
 IF  @pTescoGroupSms ='Y'                                
    INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
    VALUES (@RowId, 30, GETDATE() , @pInsertBy, NULL, NULL,'N',1)                      
                        
    IF  @pPartnerMail ='Y'                                
    INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
    VALUES (@RowId, 31, GETDATE() , @pInsertBy, NULL, NULL,'N',1)                                                                        
                        
    IF  @pPartnerEmail ='Y'                                
    INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
    VALUES (@RowId, 32, GETDATE() , @pInsertBy, NULL, NULL,'N',1)                      
                         
    IF  @pPartnerPhone ='Y'                                
    INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
    VALUES (@RowId, 33, GETDATE() , @pInsertBy, NULL, NULL,'N',1)                      
                        
    IF  @pPartnerSms ='Y'                                
    INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
    VALUES (@RowId, 34, GETDATE() , @pInsertBy, NULL, NULL,'N',1)                       
                        
    IF  @pResearchMail ='Y'                                
    INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
    VALUES (@RowId, 35, GETDATE() , @pInsertBy, NULL, NULL,'N',1)                     
                        
    IF  @pResearchEmail ='Y'                                
    INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
    VALUES (@RowId, 36, GETDATE() , @pInsertBy, NULL, NULL,'N',1)                    
                        
    IF  @pResearchPhone ='Y'                         
    INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
    VALUES (@RowId, 37, GETDATE() , @pInsertBy, NULL, NULL,'N',1)                    
                        
    IF  @pResearchSms ='Y'                                
    INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
    VALUES (@RowId, 38, GETDATE() , @pInsertBy, NULL, NULL,'N',1)                     
                        
                        
    --LCM Changes
    IF(@pTescoGroupMail = 'Y' OR @pPartnerMail = 'Y' OR @pResearchMail = 'Y')
    BEGIN
		INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
			VALUES (@RowId, 50, GETDATE() , @pInsertBy, NULL, NULL,'N',1)    
    END
    IF(@pTescoGroupEmail = 'Y' OR @pPartnerEmail = 'Y' OR @pResearchEmail = 'Y')
    BEGIN
		INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
			VALUES (@RowId, 51, GETDATE() , @pInsertBy, NULL, NULL,'N',1)    
    END
    IF(@pTescoGroupPhone = 'Y' OR @pPartnerPhone = 'Y' OR @pResearchPhone = 'Y')
    BEGIN
		INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
			VALUES (@RowId, 52, GETDATE() , @pInsertBy, NULL, NULL,'N',1)    
    END
    IF(@pTescoGroupSms = 'Y' OR @pPartnerSms = 'Y' OR @pResearchSms = 'Y')
    BEGIN
		INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,AmendBy,IsDeleted,PreferenceOptStatusID)                                 
			VALUES (@RowId, 53, GETDATE() , @pInsertBy, NULL, NULL,'N',1)    
    END
    --End of LCM Changes
                        
   --IF  @pExpat ='Y'                   
   -- INSERT INTO CustomerPreference (CustomerID,PreferenceID,InsertDateTime, InsertBy,AmendDateTime,                                
   -- AmendBy,IsDeleted)                                 
   -- VALUES (@RowId, 26, GETDATE() , @pInsertBy, NULL, NULL,'N')                                
                                  
     --IF (@pDateOfBirth is not null)                            
     --   BEGIN                            
     --         INSERT INTO FamilyMember (CustomerID, FamilyMemberSeqNo,Sex, DateOfBirth, InsertDateTime,                            
     --                                   InsertBy, AmendDateTime, AmendBy, IsDeleted )                             
     --                           VALUES (@RowId, 1,@pSex, @pDateOfBirth , getdate(),                            
     --                                   @pInsertBy, getdate(), NULL, 'N');                            
                                     
     -- INSERT INTO FamilyMemberUpdates (CustomerID, FamilyMemberSeqNo,Sex, DateOfBirth, InsertDateTime,                            
     --                                   InsertBy, AmendDateTime, AmendBy, IsDeleted )                             
     --                           VALUES (@RowId, 1,@pSex, @pDateOfBirth , getdate(),                            
                                        --@pInsertBy, getdate(), NULL, 'N');                           
         --END           
                               
    IF (@pNumberOfPeople <> 0)                        
    BEGIN                        
  IF (@pAge1 is not null and @pNumberOfPeople > 1)                            
   BEGIN                            
      INSERT INTO FamilyMember (CustomerID, FamilyMemberSeqNo,Sex, DateOfBirth, InsertDateTime,                            
           InsertBy, AmendDateTime, AmendBy, IsDeleted )                             
         VALUES (@RowId, 1,'U', Convert(Varchar,DATEPART(yyyy,DATEADD(YEAR, -@pAge1 , GETDATE())))+'-01-01' , getdate(),                            
           @pInsertBy, getdate(), NULL, 'N');                            
                                      
      INSERT INTO FamilyMemberUpdates (CustomerID, FamilyMemberSeqNo,Sex, DateOfBirth, InsertDateTime,                            
           InsertBy, AmendDateTime, AmendBy, IsDeleted )                             
         VALUES (@RowId, 1,'U', Convert(Varchar,DATEPART(yyyy,DATEADD(YEAR, -@pAge1 , GETDATE())))+'-01-01' , getdate(),                            
           @pInsertBy, getdate(), NULL, 'N');                           
    END                           
    IF (@pAge2 is not null and @pNumberOfPeople > 2)                            
   BEGIN                            
      INSERT INTO FamilyMember (CustomerID, FamilyMemberSeqNo,Sex, DateOfBirth, InsertDateTime,                            
           InsertBy, AmendDateTime, AmendBy, IsDeleted )                             
         VALUES (@RowId, 2,'U', Convert(Varchar,DATEPART(yyyy,DATEADD(YEAR, -@pAge2 , GETDATE())))+'-01-01' , getdate(),                            
           @pInsertBy, getdate(), NULL, 'N');                            
                                      
      INSERT INTO FamilyMemberUpdates (CustomerID, FamilyMemberSeqNo,Sex, DateOfBirth, InsertDateTime,                            
   InsertBy, AmendDateTime, AmendBy, IsDeleted )                             
         VALUES (@RowId, 2,'U', Convert(Varchar,DATEPART(yyyy,DATEADD(YEAR, -@pAge2 , GETDATE())))+'-01-01' , getdate(),                            
           @pInsertBy, getdate(), NULL, 'N');                           
    END                           
     IF (@pAge3 is not null and @pNumberOfPeople > 3)                            
   BEGIN                            
      INSERT INTO FamilyMember (CustomerID, FamilyMemberSeqNo,Sex, DateOfBirth, InsertDateTime,       
           InsertBy, AmendDateTime, AmendBy, IsDeleted )                             
         VALUES (@RowId, 3,'U', Convert(Varchar,DATEPART(yyyy,DATEADD(YEAR, -@pAge3 , GETDATE())))+'-01-01' , getdate(),                            
           @pInsertBy, getdate(), NULL, 'N');                            
                                      
      INSERT INTO FamilyMemberUpdates (CustomerID, FamilyMemberSeqNo,Sex, DateOfBirth, InsertDateTime,                            
           InsertBy, AmendDateTime, AmendBy, IsDeleted )                             
         VALUES (@RowId, 3,'U', Convert(Varchar,DATEPART(yyyy,DATEADD(YEAR, -@pAge3 , GETDATE())))+'-01-01' , getdate(),                            
           @pInsertBy, getdate(), NULL, 'N');                           
    END                           
     IF (@pAge4 is not null and @pNumberOfPeople > 4)                            
   BEGIN                            
  INSERT INTO FamilyMember (CustomerID, FamilyMemberSeqNo,Sex, DateOfBirth, InsertDateTime,                            
           InsertBy, AmendDateTime, AmendBy, IsDeleted )                             
         VALUES (@RowId, 4,'U', Convert(Varchar,DATEPART(yyyy,DATEADD(YEAR, -@pAge4 , GETDATE())))+'-01-01' , getdate(),                                     
         @pInsertBy, getdate(), NULL, 'N');                            
                                      
      INSERT INTO FamilyMemberUpdates (CustomerID, FamilyMemberSeqNo,Sex, DateOfBirth, InsertDateTime,                            
           InsertBy, AmendDateTime, AmendBy, IsDeleted )                             
         VALUES (@RowId, 4,'U', Convert(Varchar,DATEPART(yyyy,DATEADD(YEAR, -@pAge4 , GETDATE())))+'-01-01' , getdate(),                            
           @pInsertBy, getdate(), NULL, 'N');                           
    END             
     IF (@pAge5 is not null and @pNumberOfPeople > 5)                            
   BEGIN                            
  INSERT INTO FamilyMember (CustomerID, FamilyMemberSeqNo,Sex, DateOfBirth, InsertDateTime,                            
           InsertBy, AmendDateTime, AmendBy, IsDeleted )                             
         VALUES (@RowId, 5,'U', Convert(Varchar,DATEPART(yyyy,DATEADD(YEAR, -@pAge5 , GETDATE())))+'-01-01' , getdate(),                            
           @pInsertBy, getdate(), NULL, 'N');                            
                                      
      INSERT INTO FamilyMemberUpdates (CustomerID, FamilyMemberSeqNo,Sex, DateOfBirth, InsertDateTime,                            
           InsertBy, AmendDateTime, AmendBy, IsDeleted )                             
         VALUES (@RowId, 5,'U', Convert(Varchar,DATEPART(yyyy,DATEADD(YEAR, -@pAge5 , GETDATE())))+'-01-01' , getdate(),                            
           @pInsertBy, getdate(), NULL, 'N');                           
    END                        
        END                                               
    SELECT @varClubcardID                   
                                                         
       END TRY                                          
       BEGIN CATCH                                          
              DECLARE @vErrSeverity INT                                          
              DECLARE @vErrState    INT                                          
               SELECT @vErrSeverity = ERROR_SEVERITY(),                                          
                      @vErrState = ERROR_STATE(),                                        
                      @pExeStatusMessage = ' USP_CreateCustomer - While Inserting CreateCustomer ..... ' + ERROR_MESSAGE();                                        
              RAISERROR (@pExeStatusMessage, @vErrSeverity, @vErrState );                                          
       END CATCH                                  
SET NOCOUNT OFF                                          
END


GO


