
GO
/****** Object:  StoredProcedure [dbo].[USP_CreateView]    Script Date: 02/15/2013 13:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[USP_CreateView]  
AS  
BEGIN  
 DECLARE offer_cur CURSOR LOCAL READ_ONLY FORWARD_ONLY FOR   
 SELECT OfferID FROM OFFER WHERE IsDeleted='N'  
   
 DECLARE @OfferID SMALLINT  
 DECLARE @count INT  
 DECLARE @cmd VARCHAR(MAX)  
 
 DECLARE @TransactionalDBName  NVARCHAR(100);        
 SET @TransactionalDBName =  dbo.TransactionalDBName(); 
   
 IF EXISTS(SELECT * FROM SYS.views WHERE name='ClubcardOffer')  
  DROP VIEW ClubcardOffer  
 IF EXISTS(SELECT * FROM SYS.views WHERE name='ClubcardTransaction')  
  DROP VIEW ClubcardTransaction  
 IF EXISTS(SELECT * FROM SYS.views WHERE name='ClubcardTransactionCashierID')  
  DROP VIEW ClubcardTransactionCashierID  
 IF EXISTS(SELECT * FROM SYS.views WHERE name='CouponTypeInClubCardOffer')  
  DROP VIEW CouponTypeInClubCardOffer  
 IF EXISTS(SELECT * FROM SYS.views WHERE name='VoucherTypeInClubCardOffer')  
  DROP VIEW VoucherTypeInClubCardOffer  
  
  SET @count = 0  
 SET @cmd = 'CREATE VIEW ClubcardOffer AS'  
  
 OPEN offer_cur  
 FETCH NEXT FROM offer_cur INTO @OfferID  
   
 WHILE(@@FETCH_STATUS=0)  
 BEGIN  
  If(@count=1)  
   SET @cmd = @cmd + ' UNION ALL'  
  SET @cmd = @cmd + ' SELECT [ClubcardID]  
        ,[PointsBalanceQty]  
        ,[SKUPointsQty]  
        ,[BonusPointsQty]  
        ,[PartnerPointsBalanceQty]  
        ,[PointsBroughtForward]  
        ,[WelcomePointsQty]  
        ,[PointsCarriedForward]  
        ,[VoucherRedeemedValue]  
        ,[RewardInd]  
        ,[HighRewardInd]  
        ,[ConvertedFlag]  
        ,[MailedFlag]  
        ,[RewardReissuedDate]  
        ,[RewardReissueRequestedBy]  
        ,[RewardReissueRequestedDate]  
        ,[RewardreissuedInd]  
        ,[SignOffInd]  
        ,[InsertDateTime]  
        ,[InsertBy]  
        ,[AmendDateTime]  
        ,[AmendBy]  
        ,[IsDeleted]  
        ,[RequiresReissueInd]  
        --,[HighRewardFlag]  
        ,[GreenPointsQty]  
        ,'+Convert(Varchar,@OfferID)+' AS OfferID  
     FROM '+ @TransactionalDBName +'.dbo.ClubcardOffer_'+Convert(Varchar,@OfferID)  
  FETCH NEXT FROM offer_cur INTO @OfferID  
  SET @count = 1    
 END    
 --print @cmd
 Exec (@cmd)  
 CLOSE offer_cur  
  
-- --~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  
 SET @count = 0  
 --print '--------sab'+@cmd
 set @cmd=''
 SET @cmd = 'CREATE VIEW ClubcardTransaction AS'  
 OPEN offer_cur  
 FETCH NEXT FROM offer_cur INTO @OfferID  
   
 WHILE(@@FETCH_STATUS=0)  
 BEGIN  
  If(@count=1)  
   SET @cmd = @cmd + ' UNION ALL'  
  SET @cmd = @cmd + ' SELECT [ClubcardTransactionID]  
        ,[ClubcardID]  
        ,[SourceSystemTransactionID]  
        ,[TransactionDateTime]  
        ,[TransactionType]  
        ,[TransactionReasonID]  
        ,[AmountSpent]  
        ,[TescoStoreID]  
        ,[SourcePOSID]  
        ,[PartnerID]  
        ,[PartnerOutletID]  
        ,[CashierID]  
        ,[SKUPointsQty]  
        ,[WelcomePointsQty]  
        ,[ManualPointsQty]  
        ,[GreenPointsQty]  
        ,[InsertDateTime]  
        ,[InsertBy]  
        ,[AmendDateTime]  
        ,[AmendBy]  
        ,[IsDeleted]  
        ,[NormalPoints]  
        ,[BonusPointsQty]  
        ,'+Convert(Varchar,@OfferID)+' AS OfferID  
       FROM '+ @TransactionalDBName +'.dbo.ClubcardTransaction_'+Convert(Varchar,@OfferID)  
  FETCH NEXT FROM offer_cur INTO @OfferID  
  SET @count = 1  
 END    
 Exec (@cmd)  
 CLOSE offer_cur  
--*************************************************************************************************************  
--Added for NGCV32 Req.No 015 development  
 SET @count = 0  
 set @cmd=''
 SET @cmd = 'CREATE VIEW ClubcardTransactionCashierID AS'  
 OPEN offer_cur  
 FETCH NEXT FROM offer_cur INTO @OfferID  
   
 WHILE(@@FETCH_STATUS=0)  
 BEGIN  
  If(@count=1)  
   SET @cmd = @cmd + ' UNION ALL'  
  SET @cmd = @cmd + ' SELECT [ClubcardTransactionID]  
        ,[ClubcardID]  
        ,[CashierID]  
        ,[TescoStoreID]          
        ,'+Convert(Varchar,@OfferID)+' AS OfferID  
       FROM '+ @TransactionalDBName +'.dbo.ClubcardTransactionCashierID_'+Convert(Varchar,@OfferID)  
  FETCH NEXT FROM offer_cur INTO @OfferID  
  SET @count = 1  
 END    
 --print @cmd
 Exec (@cmd)  
 CLOSE offer_cur  
  
--***************************************************************************************************************  
 SET @count = 0  
 set @cmd=''
 SET @cmd = 'CREATE VIEW VoucherTypeInClubCardOffer AS'  
  
 OPEN offer_cur  
 FETCH NEXT FROM offer_cur INTO @OfferID  
   
 WHILE(@@FETCH_STATUS=0)  
 BEGIN  
  If(@count=1)  
   SET @cmd = @cmd + ' UNION ALL'  
  SET @cmd = @cmd + ' SELECT *  
        ,'+Convert(Varchar,@OfferID)+' AS OfferID  
       FROM '+ @TransactionalDBName +'.dbo.VoucherTypeInClubCardOffer_'+Convert(Varchar,@OfferID)  
  FETCH NEXT FROM offer_cur INTO @OfferID  
  SET @count = 1  
 END    
 --print @cmd
 Exec (@cmd)  
 CLOSE offer_cur  
 DEALLOCATE offer_cur  
END  