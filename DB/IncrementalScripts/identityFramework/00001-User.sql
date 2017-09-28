USE [AWSNet]
GO

/****** Object:  Table [dbo].[User]    Script Date: 9/26/2017 2:40:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Country] [nvarchar](100) NULL,
	[State] [nvarchar](100) NULL,
	[City] [nvarchar](100) NULL,
	[Address] [nvarchar](256) NULL,
	[ZipCode] [nvarchar](50) NULL,
	[CellPhone] [nvarchar](100) NULL,
	[LanguageId] [int] NOT NULL,
	[TimeZoneId] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsEnabled] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreationUser] [nvarchar](256) NULL,
	[CreationDate] [datetime] NOT NULL,
	[LastModificationUser] [nvarchar](256) NULL,
	[LastModificationDate] [datetime] NULL,
	[ProfileImagePath] [nvarchar](300) NULL,
	[MigrationId] [int] NULL,
 CONSTRAINT [PK_dbo.User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


