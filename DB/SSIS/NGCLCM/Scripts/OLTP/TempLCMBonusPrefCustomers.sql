DECLARE @varCountry NVARCHAR(40)
SET @varCountry = 'UK'

IF(@varCountry = 'UK')
BEGIN

	SET ANSI_NULLS ON

	SET QUOTED_IDENTIFIER ON

	SET ANSI_PADDING ON

	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_TempLCMBonusPrefCustomersGRP_IsUpdated]') AND type = 'D')
	BEGIN
	ALTER TABLE [dbo].[TempLCMBonusPrefCustomersGRP] DROP CONSTRAINT [DF_TempLCMBonusPrefCustomersGRP_IsUpdated]
	END

	/****** Object:  Table [dbo].[TempLCMBonusPrefCustomersGRP]    Script Date: 11/19/2012 22:46:03 ******/
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TempLCMBonusPrefCustomersGRP]') AND type in (N'U'))
	DROP TABLE [dbo].[TempLCMBonusPrefCustomersGRP]
	
	CREATE TABLE [dbo].[TempLCMBonusPrefCustomersGRP](
		[RowNo] [int] IDENTITY(1,1) NOT NULL,
		[HouseHoldID] [bigint] NULL,
		BCMMail CHAR(1) NULL,
		BCMEmail CHAR(1) NULL,
		BCMPhone CHAR(1) NULL,
		BCMSMS CHAR(1) NULL,
		[IsUpdated] [int] NULL
	) ON [PRIMARY]

	ALTER TABLE [dbo].[TempLCMBonusPrefCustomersGRP] ADD  CONSTRAINT [DF_TempLCMBonusPrefCustomersGRP_IsUpdated]  DEFAULT ((1)) FOR [IsUpdated]

	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_TempLCMBonusPrefCustomers_IsUpdated]') AND type = 'D')
	BEGIN
	ALTER TABLE [dbo].[TempLCMBonusPrefCustomers] DROP CONSTRAINT [DF_TempLCMBonusPrefCustomers_IsUpdated]
	END

	/****** Object:  Table [dbo].[TempLCMBonusPrefCustomers]    Script Date: 11/19/2012 22:46:03 ******/
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TempLCMBonusPrefCustomers]') AND type in (N'U'))
	DROP TABLE [dbo].[TempLCMBonusPrefCustomers]
	
	CREATE TABLE [dbo].[TempLCMBonusPrefCustomers](
		[RowNo] [int] IDENTITY(1,1) NOT NULL,
		[HouseHoldID] [bigint] NULL,
		LCMPreference CHAR(1) NULL,
		[IsUpdated] [int] NULL
	) ON [PRIMARY]

	ALTER TABLE [dbo].[TempLCMBonusPrefCustomers] ADD  CONSTRAINT [DF_TempLCMBonusPrefCustomers_IsUpdated]  DEFAULT ((1)) FOR [IsUpdated]
END