USE [HM_Products]

/****** Object:  Table [dbo].[ShopOrders]    Script Date: 29-09-2020 11:25:50 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[ShopOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Createdate] [datetime] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[ShopOrders] ADD  CONSTRAINT [DF_Order_Createdate]  DEFAULT (getdate()) FOR [Createdate]

ALTER TABLE [dbo].[ShopOrders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id]) ON DELETE CASCADE

ALTER TABLE [dbo].[ShopOrders] CHECK CONSTRAINT [FK_Orders_Customer]
