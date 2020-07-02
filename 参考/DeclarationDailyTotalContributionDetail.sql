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

Date: 2020-06-04 13:24:27
*/


-- ----------------------------
-- Table structure for DeclarationDailyTotalContributionDetail
-- ----------------------------
DROP TABLE [dbo].[DeclarationDailyTotalContributionDetail]
GO
CREATE TABLE [dbo].[DeclarationDailyTotalContributionDetail] (
[IsCount] bit NULL ,
[Id] int NOT NULL 
)


GO

-- ----------------------------
-- Indexes structure for table DeclarationDailyTotalContributionDetail
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table DeclarationDailyTotalContributionDetail
-- ----------------------------
ALTER TABLE [dbo].[DeclarationDailyTotalContributionDetail] ADD PRIMARY KEY ([Id])
GO
