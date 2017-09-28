USE [AWSNet]
GO

/****** Object:  Table [dbo].[Category]    Script Date: 9/26/2017 10:30:51 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[CreationUser] [nvarchar](256) NULL,
	[CreationDate] [datetime] NOT NULL,
	[LastModificationDate] [datetime] NULL,
	[LastModificationUser] [nvarchar](256) NULL,
 CONSTRAINT [PK_dbo.Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


