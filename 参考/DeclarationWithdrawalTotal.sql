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

Date: 2020-06-04 13:16:20
*/


-- ----------------------------
-- Table structure for DeclarationWithdrawalTotal
-- ----------------------------
DROP TABLE [dbo].[DeclarationWithdrawalTotal]
GO
CREATE TABLE [dbo].[DeclarationWithdrawalTotal] (
[CustomerId] int NOT NULL ,
[CustomerGuid] uniqueidentifier NULL ,
[IsCount] bit NULL ,
[TotalAmount] decimal(18,2) NULL ,
[TotalDecShareAmount] decimal(18,2) NULL ,
[TotalFreezeAmount] decimal(18,2) NULL ,
[TotalLuckyAmount] decimal(18,2) NULL ,
[TotalPushAmount] decimal(18,2) NULL ,
[TotalStoreShareAmount] decimal(18,2) NULL ,
[UpdateTime] datetime NULL ,
[Id] int NOT NULL DEFAULT ((0)) 
)


GO

-- ----------------------------
-- Indexes structure for table DeclarationWithdrawalTotal
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table DeclarationWithdrawalTotal
-- ----------------------------
ALTER TABLE [dbo].[DeclarationWithdrawalTotal] ADD PRIMARY KEY ([Id])
GO
