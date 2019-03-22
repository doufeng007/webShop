==============================
Author:hq
Description:”√∆∑-…Í«Îº«¬º
Module:
==============================
CREATE TABLE [dbo].[SupplyApplySubBak](
	[Id] [uniqueidentifier] NOT NULL,
	[Status] [int] NULL,
	[IsDeleted] [bit] NULL,
	[DeleterUserId] [bigint] NULL,
	[LastModificationTime] [datetime] NULL,
	[LastModifierUserId] [bigint] NULL,
	[CreationTime] [datetime] NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[DeletionTime] [datetime] NULL,
	[MainId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Version] [nvarchar](50) NOT NULL,
	[Number] [int] NOT NULL,
	[Unit] [nvarchar](50) NOT NULL,
	[Money] [nvarchar](50) NULL,
	[Des] [nvarchar](200) NULL,
	[GetTime] [datetime] NOT NULL,
	[SupplyId] [uniqueidentifier] NULL,
	[UserId] [varchar](50) NULL,
	[Type] [int] NULL,
	[Result] [int] NOT NULL,
	[ResultRemark] [nvarchar](100) NULL,
 CONSTRAINT [PK_SupplyApplySubBak] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SupplyApplySubBak] ADD  DEFAULT ((0)) FOR [Result]
GO