USE [HM_Products]

/****** Object:  Table [dbo].[Category]    Script Date: 17-08-2020 14:33:23 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

DROP TABLE IF EXISTS [dbo].[Category];

CREATE TABLE [dbo].[Category](
	[Category_id] [int] NOT NULL,
	[Category_name] [varchar](max) NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


