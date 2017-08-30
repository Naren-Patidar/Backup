/****** Object:  Table [dbo].[TempMailingData]    Script Date: 02/06/2013 11:02:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TempMailingData]') AND type in (N'U'))
DROP TABLE [dbo].[TempMailingData]
GO


/****** Object:  Table [dbo].[TempMailingData]    Script Date: 02/06/2013 11:02:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TempMailingData](
	[ClubcardID] [bigint] NULL,
	[CustomerID] [bigint] NOT NULL,
	[HouseholdID] [bigint] NULL,
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
	[EmailAddress] [nvarchar](2000) NULL,
	[MobilePhoneNumber] [varchar](40) NULL,
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
	[TescoProducts] [char](1) NULL,
	[TescoPartners] [char](1) NULL,
	[CustomerResearch] [char](1) NULL,
 CONSTRAINT [PK_TempMailData] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


