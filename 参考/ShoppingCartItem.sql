/*
Navicat SQL Server Data Transfer

Source Server         : localhost
Source Server Version : 105000
Source Host           : .:1433
Source Database       : dev
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 105000
File Encoding         : 65001

Date: 2020-06-19 04:13:28
*/


-- ----------------------------
-- Table structure for ShoppingCartItem
-- ----------------------------
DROP TABLE [dbo].[ShoppingCartItem]
GO
CREATE TABLE [dbo].[ShoppingCartItem] (
[Id] int NOT NULL IDENTITY(1,1) ,
[StoreId] int NOT NULL ,
[ParentItemId] int NULL ,
[BundleItemId] int NULL ,
[ShoppingCartTypeId] int NOT NULL ,
[CustomerId] int NOT NULL ,
[ProductId] int NOT NULL ,
[AttributesXml] nvarchar(MAX) NULL ,
[CustomerEnteredPrice] decimal(18,4) NOT NULL ,
[Quantity] int NOT NULL ,
[CreatedOnUtc] datetime NOT NULL ,
[UpdatedOnUtc] datetime NOT NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[ShoppingCartItem]', RESEED, 11)
GO

-- ----------------------------
-- Indexes structure for table ShoppingCartItem
-- ----------------------------
CREATE INDEX [IX_BundleItemId] ON [dbo].[ShoppingCartItem]
([BundleItemId] ASC) 
GO
CREATE INDEX [IX_CustomerId] ON [dbo].[ShoppingCartItem]
([CustomerId] ASC) 
GO
CREATE INDEX [IX_ProductId] ON [dbo].[ShoppingCartItem]
([ProductId] ASC) 
GO
CREATE INDEX [IX_ShoppingCartItem_ShoppingCartTypeId_CustomerId] ON [dbo].[ShoppingCartItem]
([ShoppingCartTypeId] ASC, [CustomerId] ASC) 
GO

-- ----------------------------
-- Primary Key structure for table ShoppingCartItem
-- ----------------------------
ALTER TABLE [dbo].[ShoppingCartItem] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[ShoppingCartItem]
-- ----------------------------
ALTER TABLE [dbo].[ShoppingCartItem] ADD FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
GO
ALTER TABLE [dbo].[ShoppingCartItem] ADD FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
GO
ALTER TABLE [dbo].[ShoppingCartItem] ADD FOREIGN KEY ([BundleItemId]) REFERENCES [dbo].[ProductBundleItem] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO
