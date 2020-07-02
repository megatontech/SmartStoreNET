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

Date: 2020-06-04 13:24:00
*/


-- ----------------------------
-- Table structure for DeclarationCustomerPointsDetail
-- ----------------------------
DROP TABLE [dbo].[DeclarationCustomerPointsDetail]
GO
CREATE TABLE [dbo].[DeclarationCustomerPointsDetail] (
[Amount] decimal(18) NULL DEFAULT ((0)) ,
[Comment] nvarchar(MAX) NULL ,
[Customer] int NOT NULL DEFAULT ((0)) ,
[CustomerID] uniqueidentifier NULL ,
[IsCount] bit NULL ,
[isOut] bit NULL ,
[PointGetType] int NULL ,
[PointUseType] int NULL ,
[UpdateTime] datetime NULL ,
[Id] int NOT NULL 
)


GO

-- ----------------------------
-- Indexes structure for table DeclarationCustomerPointsDetail
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table DeclarationCustomerPointsDetail
-- ----------------------------
ALTER TABLE [dbo].[DeclarationCustomerPointsDetail] ADD PRIMARY KEY ([Id])
GO
