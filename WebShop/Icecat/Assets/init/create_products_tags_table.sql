USE [HM_Products]

/****** Object:  Table [dbo].[Products_Tags]    Script Date: 28-08-2020 14:50:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Products_Tags](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TagID] [int] NULL,
	[ProductID] [int] NULL
) ON [PRIMARY]

ALTER TABLE [dbo].[Products_Tags]  WITH CHECK ADD  CONSTRAINT [FK_Products_Tags_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ID])
ON DELETE CASCADE

ALTER TABLE [dbo].[Products_Tags] CHECK CONSTRAINT [FK_Products_Tags_Products]




