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

Date: 2020-06-04 13:24:13
*/


-- ----------------------------
-- Table structure for DeclarationDailyCustomerContributionDetail
-- ----------------------------
DROP TABLE [dbo].[DeclarationDailyCustomerContributionDetail]
GO
CREATE TABLE [dbo].[DeclarationDailyCustomerContributionDetail] (
[Customer] int NOT NULL ,
[CustomerID] uniqueidentifier NULL ,
[ContributionTime] datetime NULL ,
[CreateTime] datetime NULL ,
[UpdateTime] datetime NULL ,
[ActiveLine] int NULL ,
[TotalLine] int NULL ,
[TotalPointValue] decimal(18) NULL ,
[TotalPoint] int NULL ,
[TotalValue] decimal(18) NULL ,
[CountTotalValue] decimal(18) NULL ,
[IsCount] bit NULL ,
[Id] int NOT NULL 
)


GO

-- ----------------------------
-- Indexes structure for table DeclarationDailyCustomerContributionDetail
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table DeclarationDailyCustomerContributionDetail
-- ----------------------------
ALTER TABLE [dbo].[DeclarationDailyCustomerContributionDetail] ADD PRIMARY KEY ([Id])
GO
