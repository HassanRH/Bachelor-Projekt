USE [HM_Products]
GO

/****** Object:  Table [dbo].[Orderline]    Script Date: 29-09-2020 11:24:58 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Orderline](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Name] [varchar](99) NULL,
	[ProductSKU] [varchar](99) NOT NULL,
	[ProductID] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[Price] [money] NOT NULL,
 CONSTRAINT [PK_Orderline] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO