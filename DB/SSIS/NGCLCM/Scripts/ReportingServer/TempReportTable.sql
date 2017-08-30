--USE TSMData
/****** Object:  Table [dbo].[TempCCData]    Script Date: 11/30/2012 17:39:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TempCCData]') AND type in (N'U'))
DROP TABLE [dbo].[TempCCData]
GO

/****** Object:  Table [dbo].[TempCCData]    Script Date: 11/30/2012 17:40:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TempCCData](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[HouseholdData] [nvarchar](2000) NULL
) ON [PRIMARY]

