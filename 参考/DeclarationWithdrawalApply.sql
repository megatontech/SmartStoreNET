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

Date: 2020-06-04 13:24:39
*/


-- ----------------------------
-- Table structure for DeclarationWithdrawalApply
-- ----------------------------
DROP TABLE [dbo].[DeclarationWithdrawalApply]
GO
CREATE TABLE [dbo].[DeclarationWithdrawalApply] (
[Customer] int NOT NULL ,
[Operater] int NULL ,
[CustomerID] uniqueidentifier NULL ,
[OperaterID] uniqueidentifier NULL ,
[Amount] decimal(18,2) NULL ,
[isOut] bit NULL ,
[Comment] nvarchar(MAX) NULL ,
[WithdrawTime] datetime NULL ,
[WithdrawAuditTime] datetime NULL ,
[WithdrawAuditCustomer] int NULL ,
[WithdrawAuditComment] nvarchar(MAX) NULL ,
[WithdrawType] int NULL ,
[WithdrawStatus] int NULL ,
[UpdateTime] datetime NULL ,
[IsCount] bit NULL ,
[Id] int NOT NULL 
)


GO

-- ----------------------------
-- Indexes structure for table DeclarationWithdrawalApply
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table DeclarationWithdrawalApply
-- ----------------------------
ALTER TABLE [dbo].[DeclarationWithdrawalApply] ADD PRIMARY KEY ([Id])
GO
