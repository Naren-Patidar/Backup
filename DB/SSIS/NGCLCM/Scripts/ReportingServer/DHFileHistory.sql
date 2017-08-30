--USE TSMData

/****** Object:  Table [dbo].[DHFileHistory]    Script Date: 11/05/2012 17:49:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DHFileHistory]') AND type in (N'U'))
DROP TABLE [dbo].[DHFileHistory]
GO


/****** Object:  Table [dbo].[DHFileHistory]    Script Date: 11/05/2012 17:50:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DHFileHistory](
	[FileNumber] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](1000) NULL
) ON [PRIMARY]

GO


