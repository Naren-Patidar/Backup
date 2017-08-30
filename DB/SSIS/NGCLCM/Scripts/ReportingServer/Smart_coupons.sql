--USE TSMData

/****** Object:  Table [dbo].[Smart_coupons]    Script Date: 11/05/2012 18:01:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Smart_coupons]') AND type in (N'U'))
DROP TABLE [dbo].[Smart_coupons]
GO


/****** Object:  Table [dbo].[Smart_coupons]    Script Date: 11/05/2012 18:02:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Smart_coupons](
	[HouseholdID] [bigint] NULL,
	[CouponSmartCode] [varchar](22) NULL,
	[CouponAlphaCode] [varchar](12) NULL,
	[TriggerValue] [varchar](5) NULL,
	[CouponSlotNumber] [bigint] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

IF EXISTS (SELECT     * FROM         sys.objects
WHERE     object_id = OBJECT_ID(N'[dbo].[TempCouponInsuance]') AND type IN (N'U')) 
DROP TABLE [dbo].[TempCouponInsuance] 
CREATE TABLE [dbo].[TempCouponInsuance](
	[LCMCouponNumber] [varchar](50) NOT NULL,
	[LCMCouponIssued] [varchar](50) NOT NULL
	)

GO
IF EXISTS (SELECT     * FROM         sys.objects
WHERE     object_id = OBJECT_ID(N'[dbo].[TempCouponIssuanceTemplate]') AND type IN (N'U')) 
DROP TABLE [dbo].[TempCouponIssuanceTemplate] 
CREATE TABLE [dbo].[TempCouponIssuanceTemplate](
	[TempColumn1] [varchar](50) NOT NULL,
	[TempColumn2] [varchar](50) NOT NULL
	)
