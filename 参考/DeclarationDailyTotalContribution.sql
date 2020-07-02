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

Date: 2020-06-04 13:24:21
*/


-- ----------------------------
-- Table structure for DeclarationDailyTotalContribution
-- ----------------------------
DROP TABLE [dbo].[DeclarationDailyTotalContribution]
GO
CREATE TABLE [dbo].[DeclarationDailyTotalContribution] (
[TotalValue] decimal(18) NULL ,
[ContributionValue] decimal(18) NULL ,
[DecValue] decimal(18) NULL ,
[ContributionTime] datetime NULL ,
[CreateTime] datetime NULL ,
[UpdateTime] datetime NULL ,
[IsCount] bit NULL ,
[Id] int NOT NULL 
)


GO

-- ----------------------------
-- Indexes structure for table DeclarationDailyTotalContribution
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table DeclarationDailyTotalContribution
-- ----------------------------
ALTER TABLE [dbo].[DeclarationDailyTotalContribution] ADD PRIMARY KEY ([Id])
GO
