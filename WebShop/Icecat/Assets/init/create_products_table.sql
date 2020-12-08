USE [HM_Products]


/****** Object:  Table [dbo].[Products]    Script Date: 17-08-2020 14:33:38 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

DROP TABLE IF EXISTS [dbo].[Products];

CREATE TABLE [dbo].[Products](
	[ID] [int] NOT NULL,
	[Sku] [varchar](max) NOT NULL,
	[EANCode] [varchar](13) NULL,
	[Title] [varchar](max) NULL,
	[Price] [money] NULL,
	[Brand_id] [int] NULL,
	[Category_id] [int] NULL,
	[Description] [varchar](max) NULL,
	[Short_summary_description] [varchar](max) NULL,
	[Long_summary_description] [varchar](max) NULL,
	[Thumb_pic] [varbinary](max) NULL,
	[Manual] [varbinary](max) NULL,
 CONSTRAINT [PK_products] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Brand] FOREIGN KEY([Brand_id])
REFERENCES [dbo].[Brand] ([Brand_id]) ON DELETE CASCADE

ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Brand]

ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Category] FOREIGN KEY([Category_id])
REFERENCES [dbo].[Category] ([Category_id]) ON DELETE CASCADE

ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Category]
