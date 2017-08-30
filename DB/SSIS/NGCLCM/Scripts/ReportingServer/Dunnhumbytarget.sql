--USE TSMData

/****** Object:  Table [dbo].[Dunnhumbytarget]    Script Date: 11/05/2012 17:40:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Dunnhumbytarget]') AND type in (N'U'))
DROP TABLE [dbo].[Dunnhumbytarget]
GO

/****** Object:  Table [dbo].[Dunnhumbytarget]    Script Date: 11/05/2012 17:41:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Dunnhumbytarget](
	[HouseholdNumber] [bigint] NULL,
	[CouponSlotNumber] [varchar](5) NULL,
	[TriggerValue] [varchar](5) NULL,
	[Basestock] [varchar](5) NULL,
	[VIPIndicator] [varchar](1) NULL,
	[ControlIndicator] [varchar](1) NULL,
	[FileNumber] [int] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TempLCMFinalData]') AND type in (N'U'))
 BEGIN
	   DROP TABLE TempLCMFinalData 
END

GO                               

CREATE TABLE TempLCMFinalData(Data NVARCHAR(MAX))




