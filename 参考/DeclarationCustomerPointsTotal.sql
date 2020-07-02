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

Date: 2020-06-04 13:24:07
*/


-- ----------------------------
-- Table structure for DeclarationCustomerPointsTotal
-- ----------------------------
DROP TABLE [dbo].[DeclarationCustomerPointsTotal]
GO
CREATE TABLE [dbo].[DeclarationCustomerPointsTotal] (
[Customer] int NOT NULL ,
[CustomerID] uniqueidentifier NULL ,
[Amount] decimal(18) NULL ,
[UpdateTime] datetime NULL ,
[IsCount] bit NULL ,
[Id] int NOT NULL 
)


GO

-- ----------------------------
-- Indexes structure for table DeclarationCustomerPointsTotal
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table DeclarationCustomerPointsTotal
-- ----------------------------
ALTER TABLE [dbo].[DeclarationCustomerPointsTotal] ADD PRIMARY KEY ([Id])
GO
