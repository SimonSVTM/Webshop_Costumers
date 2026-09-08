-- =========================================================
-- Webshop Database - Category table
-- Corresponds to the "Kategori" class in the domain model
-- <<table>>, <<PK>>, <<surrogate>>
-- =========================================================

IF OBJECT_ID('dbo.Category', 'U') IS NOT NULL
    DROP TABLE dbo.Category;
GO

CREATE TABLE dbo.Category
(
    CategoryId  INT IDENTITY(1,1) NOT NULL,   -- <<PK>> <<surrogate>>
    Name        NVARCHAR(100)     NOT NULL,

    CONSTRAINT PK_Category PRIMARY KEY (CategoryId),
    CONSTRAINT UQ_Category_Name UNIQUE (Name)
);
GO

-- Sample data to test the application with
INSERT INTO dbo.Category (Name)
VALUES ('Electronics'), ('Clothing'), ('Books'), ('Home & Garden');
GO
