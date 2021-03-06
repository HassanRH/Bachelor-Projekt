SET NUMERIC_ROUNDABORT OFF

SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON

SET DATEFORMAT YMD

SET XACT_ABORT ON

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE


-- Pointer used for text / image updates. This might not be needed, but is declared here just in case
DECLARE @pv binary(16)
BEGIN TRANSACTION

-- Drop constraints from [dbo].[uCommerce_OrderStatus]
ALTER TABLE [dbo].[uCommerce_OrderStatus] DROP CONSTRAINT [uCommerce_FK_OrderStatus_OrderStatus1]

-- Drop constraint uCommerce_FK_Payment_PaymentStatus from [dbo].[uCommerce_Payment]
ALTER TABLE [dbo].[uCommerce_Payment] DROP CONSTRAINT [uCommerce_FK_Payment_PaymentStatus]

-- Update 2 rows in [dbo].[uCommerce_OrderStatus]
UPDATE [dbo].[uCommerce_OrderStatus] SET [Pipeline]=N'ToCompletedOrder' WHERE [OrderStatusId]=3
UPDATE [dbo].[uCommerce_OrderStatus] SET [Pipeline]=N'ToCancelled' WHERE [OrderStatusId]=7

-- Add 9 rows to [dbo].[uCommerce_PaymentStatus]
INSERT INTO [dbo].[uCommerce_PaymentStatus] ([PaymentStatusId], [Name]) VALUES (10000001, N'Pending Authorization')
INSERT INTO [dbo].[uCommerce_PaymentStatus] ([PaymentStatusId], [Name]) VALUES (10000002, N'Authorized')
INSERT INTO [dbo].[uCommerce_PaymentStatus] ([PaymentStatusId], [Name]) VALUES (10000003, N'Acquired')
INSERT INTO [dbo].[uCommerce_PaymentStatus] ([PaymentStatusId], [Name]) VALUES (10000004, N'Cancelled')
INSERT INTO [dbo].[uCommerce_PaymentStatus] ([PaymentStatusId], [Name]) VALUES (10000005, N'Refunded')
INSERT INTO [dbo].[uCommerce_PaymentStatus] ([PaymentStatusId], [Name]) VALUES (10000006, N'Complete')
INSERT INTO [dbo].[uCommerce_PaymentStatus] ([PaymentStatusId], [Name]) VALUES (10000008, N'Declined')
INSERT INTO [dbo].[uCommerce_PaymentStatus] ([PaymentStatusId], [Name]) VALUES (10000009, N'Acquire failed')

SET IDENTITY_INSERT [dbo].[uCommerce_OrderStatus] ON

-- Add 2 rows to [dbo].[uCommerce_OrderStatus]
INSERT INTO [dbo].[uCommerce_OrderStatus] ([OrderStatusId], [Name], [Sort], [RenderChildren], [RenderInMenu], [NextOrderStatusId], [ExternalId], [IncludeInAuditTrail], [Order], [AllowUpdate], [AlwaysAvailable], [Pipeline]) VALUES (1000000, N'Requires attention', 3, 1, 1, 3, NULL, 1, NULL, 1, 0, NULL)
INSERT INTO [dbo].[uCommerce_OrderStatus] ([OrderStatusId], [Name], [Sort], [RenderChildren], [RenderInMenu], [NextOrderStatusId], [ExternalId], [IncludeInAuditTrail], [Order], [AllowUpdate], [AlwaysAvailable], [Pipeline]) VALUES (1000001, N'Manually cancelled', 10, 1, 1, NULL, NULL, 1, NULL, 1, 0, NULL)

SET IDENTITY_INSERT [dbo].[uCommerce_OrderStatus] OFF

-- Add constraints to [dbo].[uCommerce_OrderStatus]
ALTER TABLE [dbo].[uCommerce_OrderStatus] ADD CONSTRAINT [uCommerce_FK_OrderStatus_OrderStatus1] FOREIGN KEY ([NextOrderStatusId]) REFERENCES [dbo].[uCommerce_OrderStatus] ([OrderStatusId])

-- Add constraint uCommerce_FK_Payment_PaymentStatus to [dbo].[uCommerce_Payment]
ALTER TABLE [dbo].[uCommerce_Payment] WITH NOCHECK ADD CONSTRAINT [uCommerce_FK_Payment_PaymentStatus] FOREIGN KEY ([PaymentStatusId]) REFERENCES [dbo].[uCommerce_PaymentStatus] ([PaymentStatusId])

-- Add default payment counter
INSERT INTO [dbo].[uCommerce_OrderNumberSerie]
           ([OrderNumberName]
           ,[Prefix]
           ,[Postfix]
           ,[Increment]
           ,[CurrentNumber]
           ,[Deleted])
     VALUES
           ('Default Payment Reference'
           ,'Reference-'
           ,NULL
           ,1
           ,1
           ,0)

COMMIT TRANSACTION

