
GO
/****** Object:  StoredProcedure [dbo].[USP_AuthGateway_Pos_Set]    Script Date: 02/15/2013 13:59:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
--<Modification Version="1.1" Date="27-Mar-2012">            
--<Modified By>Sabhareesan O.K</Modified By>            
--<Modified History>Added NGCV36T.dbo. to All transaction related table (ClubcardOffer),ClubcardTransaction)</Modified History>            
--<Reason>Changed from "0" to "1" and Vice Versa as per Retalix Suggesstion (for printing and displaying  on cashier display.)            
--1 Denotes Success and 0 denotes Failure </Reason>            
--</Modification>            
              
ALTER PROC [dbo].[USP_AuthGateway_Pos_Set]                    
@CultureIsoCode VARCHAR(7),                     
@UsrName VARCHAR(36),                     
@SessionID VARCHAR(36),                     
@ClubcardID BIGINT,                     
@CustomerName NVARCHAR(50),                     
@TxnTypeCode SMALLINT,                     
@TescoStoreID INT,                     
@TxnDate DATETIME,                     
@PosID VARCHAR(6),                     
@TxnNbr VARCHAR(5),                     
@CashierID VARCHAR(14),                     
@AmountSpent DECIMAL(15,2),                     
@TotalPoints DECIMAL(13,2),                     
@WelcomePoints DECIMAL(13,2),                     
--@ProductPoints DECIMAL(13,2),                     
@ExtraPoints1 DECIMAL(13,2),                     
@ExtraPoints2 DECIMAL(13,2),                     
@BonusPoints DECIMAL(13,2),                    
@Training Bit,                    
@DefaultDataProtectionPref SMALLINT,                    
--Modified for US Loyalty                    
@AlternateID NVARCHAR(50),                    
@GreenPoints DECIMAL(13,2),                   
@CreateCustomerSkeletonFlag CHAR(1),                   
@StatusMsgNo SMALLINT  OUTPUT                    
AS DECLARE @@THISERROR INT                     
  /* RETURN CODES (version 4.0)                    
  0 - Success (incl. Late Transactions)                    
  1 - Transaction already added (only a Transient error)                    
  2 - Card account owner is unknown                     
  3 - Head of household is unknown                     
  4 - No current offer                     
  5 - Tried and failed to register new card account                     
  6 - POS Type is unknown                    
  7 - Transaction Date is in the future                     
  8 - Store is unknown                     
  9 - Card Doesnot exists                  
   */ /* Declare Variables */ DECLARE @TxnReasonCode SMALLINT SET @TxnReasonCode=0                     
                      
  --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END                     
  DECLARE @datenow DATETIME SET @datenow=GETDATE()                     
  DECLARE @future_time INT                    
  --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END                     
  DECLARE @datenow_plus DATETIME                     
  --Select @future_time =  configuration_value From Admin_Configuration where configuration_name = 'POSFutureTime'                    
  SET @future_time = 2                    
  SET @datenow_plus=DATEADD(hour,@future_time,@datenow)                     
  --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END                     
  DECLARE @TxnID BIGINT                     
  DECLARE @CustomerID BIGINT                     
  DECLARE @HouseholderID BIGINT                     
  DECLARE @OfferID BIGINT                     
  DECLARE @STARTDATETIME DATETIME                     
  DECLARE @ENDDATETIME DATETIME                     
  DECLARE @CustomerUseStatusCode SMALLINT                     
  DECLARE @ClubcardStatus SMALLINT                     
  DECLARE @ClubcardOfferID BIGINT                     
  DECLARE @LastTxnDate DATETIME                     
  DECLARE @CustomerWelcomedFlag VARCHAR(1)                     
  DECLARE @StoreWelcomePoints DECIMAL(13,2)                     
  DECLARE @MessageWelcomePoints char(9)                    
  --Commented By Noushad                    
  --DECLARE @UsrID VARCHAR(36)                     
  DECLARE @ClubcardNumber BIGINT                    
  DECLARE @InsertDateTime DATETIME                    
  DECLARE @AmendDateTime DATETIME       
  DECLARE @IsDeleted CHAR(1)                    
  DECLARE @UserId SMALLINT                    
  DECLARE @ParmDefinition NVARCHAR(500)                    
  DECLARE @TransactionalDBName  NVARCHAR(100);        
  SET @TransactionalDBName =  dbo.TransactionalDBName();                         
  SET @MessageWelcomePoints = @WelcomePoints                    
  SET @InsertDateTime = GETDATE()                    
  SET @AmendDateTime = @InsertDateTime                    
  SET @IsDeleted = 'N'                    
                      
  SELECT @UserID = UserID from ApplicationUser WHERE UserName = @UsrName                    
                    
 BEGIN TRAN A                    
                    
 --Commented By Noushad                    
 --Select @UsrID = UserID from ApplicationUser where UserName = @UsrName                    
                     
  /* Return code 0 used to overcome issue discovered during Turkish pilot UAT Till Systems returning transactions with negative spend under the following conditions 1 Voiding entire transaction and 2 Voiding previously swiped clubcard*/                   
 
   
 IF (@TxnTypeCode = 2 OR @TxnTypeCode = 3)                     
 BEGIN                    
                  
  --RETURN(0)              
  --Changed from 0 to 1            
  --<Reason>Changed from "0" to "1" and Vice Versa as per Retalix Suggesstion (for printing and displaying  on cashier display.)            
  --1 Denotes Success and 0 denotes Failure </Reason>            
  SET @StatusMsgNo = 1                   
  ROLLBACK TRAN A                    
  RETURN                    
 END                    
  /* Get Store welcome points */ SELECT @StoreWelcomePoints =  TescoStore.StoreWelcomePointsQty  FROM TescoStore WHERE  TescoStore.TescoStoreID  =  @TescoStoreID  AND IsDeleted = 'N'               
                     
 --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END                     
                    
 IF (@StoreWelcomePoints IS NULL)                     
 BEGIN                    
  --RETURN(8)                      
  SET @StatusMsgNo = 8                    
  ROLLBACK TRAN A                    
  RETURN                    
 END                    
  /* Check Transaction is New */ SELECT @TxnID =  CT.ClubcardTransactionID  FROM VW_CurrentClubcardTransaction CT (nolock) WHERE  CT.TescoStoreID  =  @TescoStoreID         AND  CT.TransactionDateTime  =  @TxnDate                    
    AND  CT.SourcePOSID  =  @PosID                      
    AND  CT.SourceSystemTransactionID  =  @TxnNbr AND CT.IsDeleted = 'N' AND CT.ClubcardID =   @ClubcardID            
 --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END                     
      
--Check for Duplicate in previous Transaction Table as well[Changes done as per Business Req]      
IF (@TxnID IS NULL)                     
BEGIN        
  /* Check Transaction is New */ SELECT @TxnID =  CT.ClubcardTransactionID  FROM VW_PreviousClubcardTransaction CT (nolock) WHERE  CT.TescoStoreID  =  @TescoStoreID         AND  CT.TransactionDateTime  =  @TxnDate                    
    AND  CT.SourcePOSID  =  @PosID                      
    AND  CT.SourceSystemTransactionID  =  @TxnNbr AND CT.IsDeleted = 'N' AND CT.ClubcardID =   @ClubcardID      
END      
                     
 IF (@TxnID IS NOT NULL)                     
 BEGIN                    
  --RETURN(1)                
                 
  --(Changed from 1 to 0)             
  --<Reason>Changed from "0" to "1" and Vice Versa as per Retalix Suggesstion (for printing and displaying  on cashier display.)            
  --1 Denotes Success and 0 denotes Failure </Reason>            
  SET @StatusMsgNo = 0                    
  ROLLBACK TRAN A                    
  RETURN                    
 END                    
                    
 --Modified for US Loyalty                    
 IF((@ClubcardID = '') OR (@ClubcardID IS NULL) OR (@ClubcardID = 0))                    
 BEGIN                    
  SELECT DISTINCT @ClubcardID = PrimaryClubcardId FROM ClubCard                     
  WHERE CustomerID = (SELECT CustomerID FROM CustomerAlternateID WHERE CustomerAlternateID = @AlternateID)            
 END       
   
 IF(  @ClubcardID IS NULL)  
 BEGIN  
  SET @StatusMsgNo =9  
  ROLLBACK TRAN A                    
  RETURN   
 END             
                    
  /* Get Card Account */ SELECT @CustomerID =  Clubcard.CustomerID , @ClubcardNumber =  Clubcard.ClubcardID ,                     
       @ClubcardStatus =  Clubcard.ClubcardStatus --, @LastTxnDate =  card_account.last_txn_date                 
       FROM Clubcard WITH (NOLOCK) WHERE  Clubcard.ClubcardID  =  @ClubcardID AND Clubcard.IsDeleted = 'N'                    
 SELECT @LastTxnDate = Max(TransactionDateTime) From ClubcardTransaction where ClubcardID = @ClubcardID AND ClubcardTransaction.IsDeleted = 'N'            
   
 IF(@LastTxnDate IS NULL)  
 BEGIN  
 SELECT @LastTxnDate = Max(TransactionDateTime) From VW_PreviousClubcardTransaction where ClubcardID = @ClubcardID AND VW_PreviousClubcardTransaction.IsDeleted = 'N'                    
END  
 --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END                     
                    
 IF (@ClubcardNumber IS NULL) BEGIN                    
                       
  --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END                     
                      
  --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END n                    
  --SET @card_account_crmid=newid()                     
  --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END                     
  DECLARE @RETURNCODE INT                    
  DECLARE @CustID BIGINT                     
                    
  IF(@CreateCustomerSkeletonFlag = '1')                  
 EXEC @RETURNCODE = USP_CreateCustomerSkeleton @UserId, @CustomerName,@ClubcardId,@TescoStoreID,@TxnDate,@DefaultDataProtectionPref,@CustID OUTPUT                     
  ELSE           
  BEGIN                  
   SET @StatusMsgNo = 9                  
   ROLLBACK TRAN A                    
   RETURN                    
  END                   
  SET @CustomerID = @CustID                     
  SET @HouseholderID=@CustomerID                     
  --IF (@@ERROR != 0) RETURN(256)                    
  IF (@RETURNCODE != 0)                     
  BEGIN                    
   --RETURN(5)                      
   SET @StatusMsgNo = 5                    
   ROLLBACK TRAN A                    
   RETURN                    
  END                     
  SET @CustomerUseStatusCode = 0                    
 END ELSE BEGIN                    
  IF (@CustomerID IS NULL)                     
  BEGIN                    
   --RETURN(2)                      
   SET @StatusMsgNo = 2                    
   ROLLBACK TRAN A                    
   RETURN                    
  END                      
                    
   SELECT @HouseholderID =  Customer.PrimaryCustomerID , @CustomerUseStatusCode =  Customer.CustomerUseStatusID ,                    
    @CustomerWelcomedFlag =  Customer.CustomerWelcomedFlag  FROM Customer WITH (NOLOCK)                    
    WHERE  customer.CustomerID  =  @CustomerID                       
  --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END                     
  IF (@HouseholderID IS NULL)                     
  BEGIN                    
   --RETURN(3)                      
   SET @StatusMsgNo = 3                    
   ROLLBACK TRAN A                    
   RETURN                    
  END                       
 END                    
                     
                     
 IF (@CustomerWelcomedFlag != 0) AND (@CustomerWelcomedFlag IS NOT NULL) BEGIN            
  SET @WelcomePoints = 0                    
 END ELSE BEGIN                    
                       
  SET @WelcomePoints = @StoreWelcomePoints                    
 END                    
                     
                      
 SET @TotalPoints = @TotalPoints - @MessageWelcomePoints + @WelcomePoints                    
                    
 SET @AmountSpent = @AmountSpent/100                    
                     
 IF (@TxnTypeCode = 2 OR @TxnTypeCode = 3) BEGIN                    
  SET @AmountSpent = @AmountSpent * -1                    
  SET @TotalPoints = @TotalPoints * -1                    
  SET @WelcomePoints = @WelcomePoints * -1                    
  --SET @ProductPoints = @ProductPoints * -1                    
  SET @ExtraPoints1 = @ExtraPoints1 * -1                    
  SET @ExtraPoints2 = @ExtraPoints2 * -1                    
  SET @BonusPoints = @BonusPoints * -1                    
  SET @GreenPoints = @GreenPoints * -1                    
 END                    
                     
                    
 IF ((@LastTxnDate < @TxnDate) OR (@LastTxnDate IS NULL)) BEGIN                    
  SET @LastTxnDate = @TxnDate                    
 END                    
                     
  /* Get Current Offer and collection start date */ SELECT @OfferID =  Offer.OfferID , @StartDateTime =  offer.StartDateTime ,                     
  @EndDateTime=  Offer.EndDateTime  FROM Offer                     
   WHERE  CASE WHEN (offer.StartDateTime <= GETDATE()) AND (offer.EndDateTime > GETDATE()-1) THEN '1' ELSE '0' END  =  '1'                       
 --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END                     
                  
 IF (@OfferID IS NULL)                     
 BEGIN                    
  --RETURN(4)                      
  SET @StatusMsgNo = 4                    
  ROLLBACK TRAN A                    
  RETURN                    
 END                     
  /* Check the transaction date is valid */                     
 IF (DATEDIFF(minute,@StartDateTime,@TxnDate) < 0) BEGIN                    
  SET @TxnReasonCode = 1                    
 END ELSE BEGIN                    
  IF (DATEDIFF(minute,@TxnDate, @datenow_plus) < 0)                     
  BEGIN                    
   --RETURN(7)                      
   SET @StatusMsgNo = 7                    
   ROLLBACK TRAN A                    
   RETURN                    
  END                    
                      
 END                    
                     
                    
  DECLARE @PriClubcardID Bigint                    
     SELECT @PriClubcardID = Clubcard.PrimaryClubcardID FROM clubcard WHERE CustomerID = @HouseholderID And ClubcardID = PrimaryClubcardID And IsDeleted = 'N'           
                    
  SELECT @ClubcardOfferID =  ClubcardOffer.ClubcardID  FROM ClubcardOffer WITH (NOLOCK)             
  WHERE  ClubcardOffer.ClubcardID   = @PriClubcardID             
 --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END                     
                                 
 /* Increment Household's Point Balance */                     
                  
 DECLARE @update VARCHAR(1)                    
 IF (@ClubcardOfferID IS NOT NULL) BEGIN                    
  SET @update = '1'                    
 END ELSE BEGIN                    
  SET @update = '0'                    
                      
  DECLARE @SQL NVARCHAR(4000)                    
  SET @SQL='INSERT INTO ' + @TransactionalDBName+ '..ClubcardOffer_'+CONVERT(VARCHAR,@OfferID)+'(                    
     ClubcardOffer.SKUPointsQty,                    
     ClubcardOffer.HighRewardInd,                    
     ClubcardOffer.WelcomePointsQty,                    
  ClubcardOffer.BonusPointsQty,                    
     ClubcardOffer.GreenPointsQty,                    
     ClubcardOffer.RewardReissuedDate,                    
     ClubcardOffer.RewardreissuedInd,                     
     ClubcardOffer.RewardReissueRequestedDate,                    
     ClubcardOffer.PointsCarriedForward,                    
     ClubcardOffer.PointsBroughtForward,                    
     ClubcardOffer.PointsBalanceQty,                    
     ClubcardOffer.ClubcardID ,                    
     ClubcardOffer.PartnerPointsBalanceQty,                    
     ClubcardOffer.RewardReissueRequestedBy,                    
     ClubcardOffer.VoucherRedeemedValue,                    
     ClubcardOffer.InsertDateTime,                    
     ClubcardOffer.InsertBy,                    
     ClubcardOffer.AmendDateTime,                    
     ClubcardOffer.AmendBy,                    
     ClubcardOffer.IsDeleted,                    
     ClubcardOffer.ConvertedFlag,                    
     ClubcardOffer.SignOffInd,                    
     ClubcardOffer.MailedFlag,                    
     ClubcardOffer.RequiresReissueInd                    
     )                     
   VALUES(                     
     '+CONVERT(VARCHAR,@ExtraPoints1)+',                     
     ''0'',                     
     '+CONVERT(VARCHAR,@WelcomePoints)+',                 
     '+CONVERT(VARCHAR,@BonusPoints)+',                    
     '+CONVERT(VARCHAR,@GreenPoints)+',                         
     NULL,                     
     ''0'',                    
     NULL,                     
     0,                     
     0,                     
     '+CONVERT(VARCHAR,@TotalPoints)+',                    
     '+CONVERT(VARCHAR,@PriClubcardID)+',                    
     '+CONVERT(VARCHAR,@ExtraPoints2)+',                    
     NULL,                     
     0 ,'''                    
     +CONVERT(VARCHAR,@InsertDateTime,21)+''',                    
     0,'''                    
     +CONVERT(VARCHAR,@AmendDateTime,21)+''',                    
     0,'''                    
     +CONVERT(VARCHAR,@IsDeleted)+''',''0'',''0'',''0'',''0'')'                    
   PRINT @SQL                    
   EXEC sp_executesql @SQL                    
    SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN                     
   IF (@@THISERROR = 2601) BEGIN                    
     SELECT @ClubcardOfferID =  ClubcardOffer.ClubcardID  FROM ClubcardOffer WITH (NOLOCK)             
    WHERE  ClubcardOffer.OfferID  =  @OfferID              
     AND  ClubcardOffer.ClubcardID  =  @HouseholderID               
   --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN (256)  END                     
    SET @update = '1'                    
   END ELSE BEGIN                    
    --RETURN (256)                    
    ROLLBACK TRAN A                    
    RETURN                    
   END                    
     END                     
 END                    
                    
 IF (@update = '1') BEGIN                    
   SET @SQL =''                    
   SET @SQL='UPDATE '+ @TransactionalDBName+'..ClubcardOffer_'+CONVERT(VARCHAR,@OfferID)+'                    
    SET SKUPointsQty = SKUPointsQty +'+ CONVERT(VARCHAR,@ExtraPoints1)+',                     
    WelcomePointsQty = WelcomePointsQty +'+ CONVERT(VARCHAR,@WelcomePoints)+',                     
    BonusPointsQty = BonusPointsQty +'+ CONVERT(VARCHAR,@BonusPoints)+',                     
GreenPointsQty = GreenPointsQty +'+ CONVERT(VARCHAR,@GreenPoints)+',                     
    PointsBalanceQty = PointsBalanceQty+'+ CONVERT(VARCHAR,@TotalPoints)+',                    
    PartnerPointsBalanceQty = PartnerPointsBalanceQty + '+CONVERT(VARCHAR,@ExtraPoints2) +',                    
    AmendDateTime = ''' + CONVERT(VARCHAR,@AmendDateTime,21)+'''                    
    WHERE ClubcardID = '+Convert(varchar,@PriClubcardID)                    
   SET @ParmDefinition = '@ClubcardId BIGINT';                    
  --PRINT @SQL                    
   EXEC sp_executesql @SQL,@ParmDefinition,@ClubcardId                    
  --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN (256)  END                     
 END                    
                    
 IF (@CustomerWelcomedFlag = 0) OR (@CustomerWelcomedFlag IS NULL) BEGIN            
   UPDATE customer                     
    SET Customer.CustomerWelcomedFlag = '1'             
    WHERE                     
     Customer.CustomerId = (Select CustomerID FROM Clubcard where Clubcard.ClubcardID = @ClubcardId)                     
                     
  --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN (256)  END                     
 END                    
 -----Changes Made to Turkey last minute fix -----------
    SET @TxnTypeCode = 2
    SET @TxnReasonCode = 2  
  ---------------Changes End ------------------------------          
 --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN (256)  END                     
  SET @SQL=''                  
  DECLARE @NormalPointsQty BIGINT                    
  SET @NormalPointsQty = @TotalPoints - (@WelcomePoints + @ExtraPoints1 + @BonusPoints + @ExtraPoints2 + @GreenPoints)                    
  SET @SQL='INSERT INTO ' +@TransactionalDBName+'..ClubcardTransaction_'+CONVERT(VARCHAR,@OfferID)+'                    
   (ClubcardID,  SourceSystemTransactionID,TransactionDateTime,TransactionType,TransactionReasonID,AmountSpent,TescoStoreID,                    
   SourcePOSID,PartnerID,PartnerOutletID,CashierID,SKUPointsQty,WelcomePointsQty,ManualPointsQty,               
   GreenPointsQty,BonusPointsQty,InsertDateTime, InsertBy, AmendDateTime, AmendBy, IsDeleted,NormalPoints) VALUES (                     
   '+CONVERT(VARCHAR,@ClubcardId)+','                    
   +CONVERT(VARCHAR,@TxnNbr)+','''                    
   +CONVERT(VARCHAR,@TxnDate,21)+''','                    
   +CONVERT(VARCHAR,@TxnTypeCode)+','                    
   +CONVERT(VARCHAR,@TxnReasonCode)+','                    
   +CONVERT(VARCHAR,@AmountSpent)+','                    
   +CONVERT(VARCHAR,@TescoStoreID)+','                    
   +CONVERT(VARCHAR,@PosID)+',                    
   NULL,                    
   NULL,'''                    
   +CONVERT(VARCHAR,@CashierID)+''','                    
   +CONVERT(VARCHAR,@ExtraPoints1)+','                    
   +CONVERT(VARCHAR,@WelcomePoints)+','                  
   +CONVERT(VARCHAR,@ExtraPoints2)+','                  
   +CONVERT(VARCHAR,@GreenPoints)+','                  
   +CONVERT(VARCHAR,@BonusPoints)+','''                  
   +CONVERT(VARCHAR,@InsertDateTime,21)+''',                    
   0,'''                    
   +CONVERT(VARCHAR,@AmendDateTime,21)+''',                    
   0,'''                    
   +CONVERT(VARCHAR,@IsDeleted)+''',' + Convert(VARCHAR,@NormalPointsQty)+ ')'                    
   --PRINT @SQL                    
   EXEC sp_executesql @SQL                    
  --SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN (256)  END                     
                    
 IF @Training = 1                    
 Begin                    
  Rollback Tran A             
  --Changed from 0 to 1            
  --<Reason>Changed from "0" to "1" and Vice Versa as per Retalix Suggesstion (for printing and displaying  on cashier display.)            
  --1 Denotes Success and 0 denotes Failure </Reason>                   
  SET @StatusMsgNo = 1                    
  RETURN                    
 End                    
 -- There is a possibility that when two transaction handle the same txn                    
 -- both decide it is a new txn, and both try to add it to the txn table.                    
 -- Normally only one transaction will succeed.                    
 -- But, if @offer_crmid is no longer current then this transaction                     
 -- might successfully added the txn to partition x+1, while the other                    
 -- transaction has successfully added the same txn to partition x.                    
 -- To prevent this for occurring, always rollback the transaction if the                    
 -- current collection period has changed.       IF @Training = 0                    
 BEGIN                    
  IF (@EndDateTime+1 <= GETDATE())                     
  BEGIN                    
  --ROLLBACK                    
  --BEGIN TRANSACTION                    
  Rollback Tran A                    
  DECLARE @RC INT                    
  EXEC USP_AuthGateway_Pos_Set @CultureIsoCode, @UsrName , @SessionID,                    
   @ClubcardID, @CustomerName,                     
   @TxnTypeCode, @TescoStoreID, @TxnDate,                     
   @PosId, @TxnNbr, @CashierID, @AmountSpent,                    
   @TotalPoints, @WelcomePoints, --@product_points,                    
   @ExtraPoints1, @ExtraPoints2, @BonusPoints, @Training, @DefaultDataProtectionPref,                    
   @AlternateID, @GreenPoints, @StatusMsgNo OUTPUT                    
                      
  END                    
  ELSE                    
  BEGIN                    
   Commit Tran A               
   --Changed from 0 to 1            
   --<Reason>Changed from "0" to "1" and Vice Versa as per Retalix Suggesstion (for printing and displaying  on cashier display.)            
   --1 Denotes Success and 0 denotes Failure </Reason>                 
   SET @StatusMsgNo = 1                   
    RETURN                     
  END                    
 END   