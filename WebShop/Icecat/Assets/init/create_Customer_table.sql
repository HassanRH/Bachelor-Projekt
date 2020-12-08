USE [HM_Products]

/****** Object:  Table [dbo].[Customer]    Script Date: 29-09-2020 11:23:53 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](99) NOT NULL,
	[Email] [varchar](99) NOT NULL,
	[Address] [varchar](99) NOT NULL,
	[Company] [varchar](99) NULL,
	[Apartsuite] [varchar](99) NULL,
	[Zip] [varchar](12) NOT NULL,
	[City] [varchar](99) NOT NULL,
	[Phone] [varchar](20) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
