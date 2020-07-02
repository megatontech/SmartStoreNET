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

Date: 2020-06-17 00:27:21
*/


-- ----------------------------
-- Table structure for Product_Picture_Mapping
-- ----------------------------
DROP TABLE [dbo].[Product_Picture_Mapping]
GO
CREATE TABLE [dbo].[Product_Picture_Mapping] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ProductId] int NOT NULL ,
[PictureId] int NOT NULL ,
[DisplayOrder] int NOT NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Product_Picture_Mapping]', RESEED, 189)
GO

-- ----------------------------
-- Indexes structure for table Product_Picture_Mapping
-- ----------------------------
CREATE INDEX [IX_PictureId] ON [dbo].[Product_Picture_Mapping]
([PictureId] ASC) 
GO
CREATE INDEX [IX_ProductId] ON [dbo].[Product_Picture_Mapping]
([ProductId] ASC) 
GO

-- ----------------------------
-- Primary Key structure for table Product_Picture_Mapping
-- ----------------------------
ALTER TABLE [dbo].[Product_Picture_Mapping] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[Product_Picture_Mapping]
-- ----------------------------
ALTER TABLE [dbo].[Product_Picture_Mapping] ADD FOREIGN KEY ([PictureId]) REFERENCES [dbo].[Picture] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
GO
ALTER TABLE [dbo].[Product_Picture_Mapping] ADD FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
GO
