GO
/****** Object:  StoredProcedure [dbo].[USP_CreateTables]    Script Date: 02/15/2013 13:57:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[USP_CreateTables]      
@OfferID INT      
AS      
BEGIN      
 Declare @cmd varchar(8000)   
 DECLARE @TransactionalDBName  NVARCHAR(100);      
 SET @TransactionalDBName =  dbo.TransactionalDBName();          
 Set @cmd='CREATE TABLE '+@TransactionalDBName+'.[dbo].[ClubcardOffer_'+Convert(Varchar,@OfferID)+'](      
    [ClubcardID] [bigint] NOT NULL,      
    [PointsBalanceQty] [int] NULL,      
    [SKUPointsQty] [int] NULL,      
    [BonusPointsQty] [int] NULL,      
    [PartnerPointsBalanceQty] [int] NULL,      
    [PointsBroughtForward] [int] NULL,      
    [WelcomePointsQty] [smallint] NULL,      
    [PointsCarriedForward] [int] NULL,      
    [VoucherRedeemedValue] [numeric](15, 2) NULL,      
    [RewardInd] [char](1) NULL,      
    [HighRewardInd] [char](1) NULL,      
    [RewardReissuedDate] [smalldatetime] NULL,      
    [RewardReissueRequestedBy] [varchar](20) NULL,      
    [RewardReissueRequestedDate] [smalldatetime] NULL,      
    [RewardreissuedInd] [char](1) NULL,      
    [SignOffInd] [char](1) NULL,      
    [InsertDateTime] [datetime] NULL,      
    [InsertBy] [smallint] NULL,      
    [AmendDateTime] [datetime] NULL,      
    [AmendBy] [smallint] NULL,      
    [IsDeleted] [char](1) NULL,      
    [MailedFlag] [char](1) null,      
    [ConvertedFlag] [char](1) null,      
    [RequiresReissueInd] CHAR(1) NULL,
    [OfferArchivedInd] [char](1) NULL,
    [ExpiredPoints] [int] NULL,      
    [GreenPointsQty] [int] NULL,      
    CONSTRAINT [PK_ClubcardOffer_'+Convert(Varchar,@OfferID)+'] PRIMARY KEY CLUSTERED       
   (      
    [ClubcardID] ASC      
   )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]      
   ) ON [PRIMARY]'      
    
 Exec (@cmd)      
 SELECT @cmd='CREATE TABLE '+@TransactionalDBName+'.[dbo].[ClubcardTransaction_'+CONVERT(VARCHAR,@OfferID)+'](      
      [ClubcardTransactionID] [bigint] IDENTITY(1,1) NOT NULL,      
      [ClubcardID] [bigint] NOT NULL,      
      [SourceSystemTransactionID] [int] NULL,      
      [TransactionDateTime] [datetime] NOT NULL,      
      [TransactionType] [tinyint] NULL,      
      [TransactionReasonID] [tinyint] NULL,      
      [AmountSpent] [numeric](15, 2) NULL,      
      [TescoStoreID] [int] NULL,      
      [SourcePOSID] [smallint] NULL,      
      [PartnerID] [bigint] NULL,      
      [PartnerOutletID] [int] NULL,      
      [CashierID] [nvarchar](20) NULL,      
      [SKUPointsQty] [int] NULL,      
      [WelcomePointsQty] [int] NULL,      
      [ManualPointsQty] [int] NULL,      
      [GreenPointsQty] [int] NULL,      
      [InsertDateTime] [datetime] NULL,      
      [InsertBy] [smallint] NULL,      
      [AmendDateTime] [datetime] NULL,      
      [AmendBy] [smallint] NULL,      
      [IsDeleted] [char](1) NULL,      
      [NormalPoints] [int],
      [TxnArchivedInd] [char](1) NULL,
	  [MerchantID] [int] NULL, 
	  [BonusPointsQty] [int] NULL,    
      CONSTRAINT [PK_ClubcardTransaction_'+CONVERT(VARCHAR,@OfferID)+'] PRIMARY KEY CLUSTERED       
     (      
      [ClubcardTransactionID] ASC      
     )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]      
     ) ON [PRIMARY]'      
       
 EXEC(@cmd)      
 SELECT @cmd = 'CREATE TABLE '+@TransactionalDBName+'.[dbo].[CouponTypeInClubCardOffer_'+CONVERT(VARCHAR,@OfferID)+'](      
      [ClubcardID] [bigint] NOT NULL,      
      [CouponType] [int] NOT NULL,      
      [CouponIssuedQty] [tinyint] NULL,      
      [InsertDateTime] [datetime] NULL,      
      [InsertBy] [smallint] NULL,      
      [AmendDateTime] [datetime] NULL,      
      [AmendBy] [smallint] NULL,      
      [IsDeleted] [char](1) NULL,      
      CONSTRAINT [PK_CouponTypeInClubCardOffer_'+CONVERT(VARCHAR,@OfferID)+'] PRIMARY KEY CLUSTERED       
     (      
      [ClubcardID] ASC,      
      [CouponType] ASC      
     )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]      
     ) ON [PRIMARY] '     
      
       
 EXEC(@cmd)      
 SELECT @cmd = 'CREATE TABLE '+@TransactionalDBName+'.[dbo].[VoucherTypeInClubCardOffer_'+CONVERT(VARCHAR,@OfferID)+'](      
      [VoucherTypeInClubcardOfferID] [bigint] IDENTITY(1,1) NOT NULL,      
      [VoucherType] [int] NOT NULL,      
      [ClubcardID] [bigint] NOT NULL,      
      [VoucherIssuedQty] [tinyint] NULL,      
      [InsertDateTime] [datetime] NULL,      
      [InsertBy] [smallint] NULL,      
      [AmendDateTime] [datetime] NULL,      
      [AmendBy] [smallint] NULL,      
      [IsDeleted] [char](1) NULL,       
      [VoucherExpiryDate] [datetime] null,      
 [VoucherRedeemedDate] [datetime] null,      
      CONSTRAINT [PK_VoucherTypeInClubCardOffer_'+CONVERT(VARCHAR,@OfferID)+'] PRIMARY KEY CLUSTERED       
     (      
     [VoucherTypeInClubcardOfferID] ASC      
     )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]      
     ) ON [PRIMARY]'      
 EXEC(@cmd)      
      
 SELECT @cmd = 'CREATE TABLE '+@TransactionalDBName+'.[dbo].[ClubcardTransactionCashierID_'+CONVERT(VARCHAR,@OfferID)+'](      
      [ClubcardTransactionID] [bigint] NOT NULL,      
      [ClubcardID] [bigint] NOT NULL,      
      [CashierID] [nvarchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,      
   [TescoStoreID] [int] NULL,    
     CONSTRAINT [PK_ClubcardTransactionCashierID_'+CONVERT(VARCHAR,@OfferID)+'] PRIMARY KEY CLUSTERED       
     (      
     [ClubcardTransactionID] ASC      
     )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]      
     ) ON [PRIMARY]'    
       
 EXEC(@cmd)      
      
END      
      
      
      
      
      
--Exec USP_CreateTables 9999 