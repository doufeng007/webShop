==============================
Author:hq
Description:财务用表
Module:
==============================
ALTER TABLE dbo.CWGLBorrowMoney ADD  RepaymentTime datetime NULL 

CREATE TABLE [dbo].[CWGLRepayment](
	[Id] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NULL,
	[DeleterUserId] [bigint] NULL,
	[LastModificationTime] [datetime] NULL,
	[LastModifierUserId] [bigint] NULL,
	[CreationTime] [datetime] NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[DeletionTime] [datetime] NULL,
	[TenantId] [int] NULL,
	[DealWithUsers] [varchar](500) NULL,
	[Status] [int] NOT NULL,
	[BorrowMoneyId] [uniqueidentifier] NOT NULL,
	[Money] [money] NOT NULL,
	[Mode] [int] NOT NULL,
	[BankName] [nvarchar](100) NULL,
	[CardNumber] [nvarchar](64) NULL,
	[BankOpenName] [nvarchar](100) NULL,
 CONSTRAINT [PK_CWGLREPAYMENT] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
--创建还款

CREATE TABLE [dbo].[CWGLReceivable](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Money] [money] NOT NULL,
	[Mode] [int] NOT NULL,
	[BankName] [nvarchar](100) NULL,
	[CardNumber] [nvarchar](64) NULL,
	[BankOpenName] [nvarchar](100) NULL,
	[Note] [nvarchar](max) NOT NULL,
	[Nummber] [int] NULL,
	[IsDeleted] [bit] NULL,
	[DeleterUserId] [bigint] NULL,
	[LastModificationTime] [datetime] NULL,
	[LastModifierUserId] [bigint] NULL,
	[CreationTime] [datetime] NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[DeletionTime] [datetime] NULL,
	[TenantId] [int] NULL,
	[DealWithUsers] [varchar](max) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_CWGLRECEIVABLE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
--创建收款管理