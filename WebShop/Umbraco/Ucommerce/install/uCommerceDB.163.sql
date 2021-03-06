IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Tax'
          AND Object_ID = Object_ID(N'dbo.uCommerce_Payment'))
BEGIN
    ALTER TABLE dbo.uCommerce_Payment ADD Tax MONEY
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'TaxRate'
          AND Object_ID = Object_ID(N'dbo.uCommerce_Payment'))
BEGIN
    ALTER TABLE dbo.uCommerce_Payment ADD TaxRate MONEY
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'GrossAmount'
          AND Object_ID = Object_ID(N'dbo.uCommerce_Payment'))
BEGIN
    ALTER TABLE dbo.uCommerce_Payment ADD GrossAmount MONEY
END