IF OBJECT_ID('dbo.Product', 'U') IS NOT NULL
    DROP TABLE dbo.Product;
GO

CREATE TABLE dbo.Product
(
    ProductID      INT IDENTITY(1,1) NOT NULL,
    ProductName    NVARCHAR(150) NOT NULL,
    CategoryId     INT NOT NULL,
    Price          DECIMAL(18,2) NOT NULL,
    StockQuantity  INT NOT NULL,

    CONSTRAINT PK_Product PRIMARY KEY (ProductID),
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryId)
        REFERENCES dbo.Category(CategoryId),
    CONSTRAINT CK_Product_Price CHECK (Price >= 0),
    CONSTRAINT CK_Product_StockQuantity CHECK (StockQuantity >= 0)
);
GO
