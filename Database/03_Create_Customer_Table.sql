-- Customer table for the Webshop case
CREATE TABLE Customer
(
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    Country NVARCHAR(100) NOT NULL,
    Points INT NOT NULL CONSTRAINT CK_Customer_Points_NonNegative CHECK (Points >= 0)
);

CREATE INDEX IX_Customer_Email ON Customer(Email);
CREATE INDEX IX_Customer_LastName_FirstName ON Customer(LastName, FirstName);
