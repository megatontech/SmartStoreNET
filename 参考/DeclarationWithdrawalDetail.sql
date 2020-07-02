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

Date: 2020-06-04 13:24:45
*/


-- ----------------------------
-- Table structure for DeclarationWithdrawalDetail
-- ----------------------------
DROP TABLE [dbo].[DeclarationWithdrawalDetail]
GO
CREATE TABLE [dbo].[DeclarationWithdrawalDetail] (
[Customer] int NOT NULL ,
[Operater] int NULL ,
[CustomerID] uniqueidentifier NULL ,
[OperaterID] uniqueidentifier NULL ,
[Amount] decimal(18,2) NULL ,
[isOut] bit NULL ,
[Comment] nvarchar(MAX) NULL ,
[WithdrawType] int NULL ,
[WithdrawTime] datetime NULL ,
[IsCount] bit NULL ,
[Id] int NOT NULL IDENTITY(1,1) 
)


GO
DBCC CHECKIDENT(N'[dbo].[DeclarationWithdrawalDetail]', RESEED, 5)
GO

-- ----------------------------
-- Indexes structure for table DeclarationWithdrawalDetail
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table DeclarationWithdrawalDetail
-- ----------------------------
ALTER TABLE [dbo].[DeclarationWithdrawalDetail] ADD PRIMARY KEY ([Id])
GO
