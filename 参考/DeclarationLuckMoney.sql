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

Date: 2020-06-04 13:24:33
*/


-- ----------------------------
-- Table structure for DeclarationLuckMoney
-- ----------------------------
DROP TABLE [dbo].[DeclarationLuckMoney]
GO
CREATE TABLE [dbo].[DeclarationLuckMoney] (
[Customer] int NULL ,
[Operater] int NULL ,
[CustomerID] uniqueidentifier NULL ,
[OperaterID] uniqueidentifier NULL ,
[Amount] decimal(18) NULL ,
[TotalAmount] decimal(18) NULL ,
[CustomerAmount] int NULL ,
[isOut] bit NULL ,
[Comment] nvarchar(MAX) NULL ,
[SendTime] datetime NULL ,
[StartTime] datetime NULL ,
[EndTime] datetime NULL ,
[IsCount] bit NULL ,
[Id] int NOT NULL 
)


GO

-- ----------------------------
-- Indexes structure for table DeclarationLuckMoney
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table DeclarationLuckMoney
-- ----------------------------
ALTER TABLE [dbo].[DeclarationLuckMoney] ADD PRIMARY KEY ([Id])
GO
