USE [HM_Products]

/****** Object:  Table [dbo].[Products_Gallery]    Script Date: 17-08-2020 14:31:39 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

DROP TABLE IF EXISTS [dbo].[Products_Gallery];

CREATE TABLE [dbo].[Products_Gallery](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Picture] [varbinary](max) NULL,
	[Product_id] [int] NULL,
	[Content-Type] [varchar(99)] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[Products_Gallery]  WITH CHECK ADD  CONSTRAINT [FK_Products_Gallary_Products] FOREIGN KEY([Product_id])
REFERENCES [dbo].[Products] ([ID]) ON DELETE CASCADE 

ALTER TABLE [dbo].[Products_Gallery] CHECK CONSTRAINT [FK_Products_Gallary_Products]


