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

Date: 2020-06-17 00:18:16
*/


-- ----------------------------
-- Table structure for Product_Category_Mapping
-- ----------------------------
DROP TABLE [dbo].[DeclarationProduct_Category_Mapping]
GO
CREATE TABLE [dbo].[DeclarationProduct_Category_Mapping] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ProductId] int NOT NULL ,
[CategoryId] int NOT NULL ,
[IsFeaturedProduct] bit NOT NULL ,
[DisplayOrder] int NOT NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[DeclarationProduct_Category_Mapping]', RESEED, 80)
GO

-- ----------------------------
-- Indexes structure for table Product_Category_Mapping
-- ----------------------------
CREATE INDEX [IX_CategoryId] ON [dbo].[DeclarationProduct_Category_Mapping]
([CategoryId] ASC) 
GO
CREATE INDEX [IX_ProductId] ON [dbo].[DeclarationProduct_Category_Mapping]
([ProductId] ASC) 
GO
CREATE INDEX [IX_PCM_Product_and_Category] ON [dbo].[DeclarationProduct_Category_Mapping]
([CategoryId] ASC, [ProductId] ASC) 
GO
CREATE INDEX [IX_IsFeaturedProduct] ON [dbo].[DeclarationProduct_Category_Mapping]
([IsFeaturedProduct] ASC) 
GO

-- ----------------------------
-- Primary Key structure for table Product_Category_Mapping
-- ----------------------------
ALTER TABLE [dbo].[DeclarationProduct_Category_Mapping] ADD PRIMARY KEY ([Id])
GO
