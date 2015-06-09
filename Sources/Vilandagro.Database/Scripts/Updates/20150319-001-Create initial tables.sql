﻿CREATE TABLE [Category]
(
	[Id] INT IDENTITY NOT NULL CONSTRAINT Category_PK PRIMARY KEY,
	[Name] NVARCHAR(128) NOT NULL,
	[Description] NVARCHAR(512) NULL,
	[Image] NVARCHAR(64) NULL
)
GO

CREATE TABLE [Product]
(
	[Id] INT IDENTITY NOT NULL CONSTRAINT Product_PK PRIMARY KEY,
	[CategoryId] INT NOT NULL,
	[Name] NVARCHAR(128) NOT NULL,
	[Description] NVARCHAR(512) NULL,
	[Image] NVARCHAR(64) NULL,

	FOREIGN KEY (CategoryId) REFERENCES Category(Id)
)
GO

CREATE TABLE [SpringProduct]
(
	[Id] INT NOT NULL CONSTRAINT Spring_Product_PK PRIMARY KEY,
	[Diametr] DECIMAL NULL,
	[Weight] DECIMAL NULL,

	FOREIGN KEY (Id) REFERENCES Product(Id)
)
GO

CREATE TABLE [UnitOfPrice]
(
	[Id] INT IDENTITY NOT NULL CONSTRAINT UnitOfPrice_PK PRIMARY KEY,
	[Name] NVARCHAR(64) NOT NULL,
	[Description] NVARCHAR(128) NULL
)
GO

CREATE TABLE [ProductPrice]
(
	[Id] INT IDENTITY NOT NULL CONSTRAINT ProductPrice_PL Primary KEY,
	[ProductId] INT NOT NULL,
	[UnitOfPriceId] INT NOT NULL,

	FOREIGN KEY (ProductId) REFERENCES Product(Id),
	FOREIGN KEY (UnitOfPriceId) REFERENCES UnitOfPrice(Id)
)
GO