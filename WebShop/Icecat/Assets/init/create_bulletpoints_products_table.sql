USE [HM_Products]

/****** Object:  Table [dbo].[BulletPoints_Products]    Script Date: 02-09-2020 09:50:24 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[BulletPoints_Products](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[BulletPointID] [int] NULL,
 CONSTRAINT [PK_BulletPoints_Products] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[BulletPoints_Products]  WITH CHECK ADD  CONSTRAINT [FK_BulletPoints_Products_BulletPoint] FOREIGN KEY([BulletPointID])
REFERENCES [dbo].[BulletPoint] ([ID]) ON DELETE CASCADE

ALTER TABLE [dbo].[BulletPoints_Products] CHECK CONSTRAINT [FK_BulletPoints_Products_BulletPoint]

ALTER TABLE [dbo].[BulletPoints_Products]  WITH CHECK ADD  CONSTRAINT [FK_BulletPoints_Products_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ID]) ON DELETE CASCADE

ALTER TABLE [dbo].[BulletPoints_Products] CHECK CONSTRAINT [FK_BulletPoints_Products_Products]
