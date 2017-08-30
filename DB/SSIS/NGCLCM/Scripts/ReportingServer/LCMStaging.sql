/****** Object:  Table [dbo].[LCMStaging]    Script Date: 11/06/2012 12:56:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCMStaging]') AND type in (N'U'))
DROP TABLE [dbo].[LCMStaging]
GO

/****** Object:  Table [dbo].[LCMStaging]    Script Date: 11/06/2012 12:56:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LCMStaging](	
	[HouseholdID] [bigint] NULL,
	[ClubcardID] [bigint] NULL,	
	[CustomerName1] [nvarchar](200) NULL,
	[CustomerName2] [nvarchar](200) NULL,
	[CustomerName3] [nvarchar](200) NULL,
	[CustomerTitle] [varchar](20) NULL,
	[preferredStoreCode] [int] NULL,
	[AddressLine1] [nvarchar](320) NULL,
	[AddressLine2] [nvarchar](320) NULL,
	[AddressLine3] [nvarchar](320) NULL,
	[City] [nvarchar](320) NULL,
	[Province] [nvarchar](320) NULL,
	[Country] [nvarchar](320) NULL,
	[PostCode] [nvarchar](40) NULL,
	[Gender] [nchar](2) NULL,
	[EmailAddress] [nvarchar](640) NULL,
	[MobilePhoneNumber] [varchar](40) NULL,
	[MailingFlag1] [nvarchar](200) NULL,
	[MailingFlag2] [nvarchar](200) NULL,
	[MailingFlag3] [nvarchar](200) NULL,
	[MailingFlag4] [nvarchar](200) NULL,
	[MailingFlag5] [nvarchar](200) NULL,
	[MailingFlag6] [nvarchar](200) NULL,
	[MailingFlag7] [nvarchar](200) NULL,
	[MailingFlag8] [nvarchar](200) NULL,
	[MailingFlag9] [nvarchar](200) NULL,
	[MailingFlag10] [nvarchar](200) NULL,
	[MailingFlag11] [nvarchar](200) NULL,
	[MailingFlag12] [nvarchar](200) NULL,
	[MailingFlag13] [nvarchar](200) NULL,
	[MailingFlag14] [nvarchar](200) NULL,
	[MailingFlag15] [nvarchar](200) NULL,
	[TescoGroupMail]  [char](1) NULL,
	[TescoGroupEmail] [char](1) NULL,
	[TescoGroupPhone] [char](1) NULL,
	[TescoGroupSMS] [char](1) NULL,
	[PartnerThirdPartyMail] [char](1) NULL,
	[PartnerThirdPartyEmail] [char](1) NULL,
	[PartnerThirdPartyPhone] [char](1) NULL,
	[PartnerThirdPartySMS] [char](1) NULL,
	[ResearchMail] [char](1) NULL,
	[ResearchEmail] [char](1) NULL,
	[ResearchPhone] [char](1) NULL,
	[ResearchSMS] [char](1) NULL,
	[BonusCouponMailing] [char](1) NULL,
	[BCMMail] [char](1) NULL,
	[BCMEmail] [char](1) NULL,	
	[BCMPhone] [char](1) NULL,
	[BCMSMS] [char](1) NULL,
	[TescoProducts] [char](1) NULL,
	[TescoPartners] [char](1) NULL,
	[CustomerResearch] [char](1) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


/****** Object:  Table [dbo].[Loyal_Coupon_Customers]    Script Date: 11/06/2012 12:59:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Loyal_Coupon_Customers]') AND type in (N'U'))
DROP TABLE [dbo].[Loyal_Coupon_Customers]
GO

/****** Object:  Table [dbo].[Loyal_Coupon_Customers]    Script Date: 11/06/2012 12:59:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Loyal_Coupon_Customers](
	[HouseholdID] [varchar](10) NULL,
	[ClubcardID] [bigint] NULL,
	[CustomerName1] [nvarchar](200) NULL,
	[CustomerName2] [nvarchar](200) NULL,
	[CustomerName3] [nvarchar](200) NULL,
	[CustomerTitle] [varchar](50) NULL,
	[preferredStoreCode] [int] NULL,
	[AddressLine1] [nvarchar](320) NULL,
	[AddressLine2] [nvarchar](320) NULL,
	[AddressLine3] [nvarchar](320) NULL,
	[City] [nvarchar](320) NULL,
	[Province] [nvarchar](320) NULL,
	[Country] [nvarchar](320) NULL,
	[PostCode] [nvarchar](320) NULL,
	[Gender] [Nchar](4) NULL,
	[MailingFlag1] [nvarchar](200) NULL,
	[MailingFlag2] [nvarchar](200) NULL,
	[MailingFlag3] [nvarchar](200) NULL,
	[MailingFlag4] [nvarchar](200) NULL,
	[MailingFlag5] [nvarchar](200) NULL,
	[MailingFlag6] [nvarchar](200) NULL,
	[MailingFlag7] [nvarchar](200) NULL,
	[MailingFlag8] [nvarchar](200) NULL,
	[MailingFlag9] [nvarchar](200) NULL,
	[MailingFlag10] [nvarchar](200) NULL,
	[MailingFlag11] [nvarchar](200) NULL,
	[MailingFlag12] [nvarchar](200) NULL,
	[MailingFlag13] [nvarchar](200) NULL,
	[MailingFlag14] [nvarchar](200) NULL,
	[MailingFlag15] [nvarchar](200) NULL,
	[LifeStyle] [varchar](4) NULL,
	[Shabit] [char](2) NULL,
	[Home_Ro] [varchar](4) NULL,
	[ControlFlag] [char](1) NULL,
	[Filler] [varchar](32) NULL,
	[StreamCode] [char](1) NULL,
	[MobilePhoneNumber] [varchar](40) NULL,
	[EmailAddress] [nvarchar](640) NULL,
	[TescoGroupMail] [char](1) NULL,
	[TescoGroupEmail] [char](1) NULL,
	[TescoGroupPhone] [char](1) NULL,
	[TescoGroupSMS] [char](1) NULL,
	[PartnerThirdPartyMail] [char](1) NULL,
	[PartnerThirdPartyEmail] [char](1) NULL,
	[PartnerThirdPartyPhone] [char](1) NULL,
	[PartnerThirdPartySMS] [char](1) NULL,
	[ResearchMail] [char](1) NULL,
	[ResearchEmail] [char](1) NULL,
	[ResearchPhone] [char](1) NULL,
	[ResearchSMS] [char](1) NULL,
	[BonusCouponMailing] [char](1) NULL,
	[BCMMail] [char](1) NULL,
	[BCMEmail] [char](1) NULL,	
	[BCMPhone] [char](1) NULL,
	[BCMSMS] [char](1) NULL,
	[ControlIndicator] [char](1) NULL,
	[VIPIndicator] [char](1) NULL,
	[Basestock] [varchar](5) NULL,
	[TescoProducts] [char](1) NULL,
	[TescoPartners] [char](1) NULL,
	[CustomerResearch] [char](1) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


IF EXISTS (SELECT     * FROM         sys.objects
WHERE     object_id = OBJECT_ID(N'[dbo].[TempHouseHoldId]') AND type IN (N'U')) 
DROP TABLE [dbo].[TempHouseHoldId] 
CREATE TABLE [dbo].[TempHouseHoldId](
	[HouseHoldId] [varchar](10) NULL,
	)
GO
