  -- 模板编辑增加锁定功能
  ALTER TABLE dbo.WorkFlowTemplate ADD  IsLocked bit not NULL  default(0)
    ALTER TABLE dbo.WorkFlowTemplate ADD  LastLockTime datetime  NULL 
	  ALTER TABLE dbo.WorkFlowTemplate ADD  LastLockUserId bigint  NULL  
	    ALTER TABLE dbo.WorkFlowTemplate ADD  LastLockIP varchar(100)  NULL 
		ALTER TABLE dbo.WorkFlowTemplate ADD  TenantId int  NULL  
		ALTER TABLE dbo.WorkFlowTemplate ADD  IsDeleted bit  not NULL  default(0)
		ALTER TABLE dbo.WorkFlowTemplate ADD  DeleterUserId bigint   NULL 
		ALTER TABLE dbo.WorkFlowTemplate ADD  DeletionTime datetime   NULL 
		ALTER TABLE dbo.WorkFlowTemplate ADD  LastModificationTime datetime   NULL 
		ALTER TABLE dbo.WorkFlowTemplate ADD  LastModifierUserId bigint   NULL 
		ALTER TABLE dbo.WorkFlowTemplate ADD  CreationTime datetime   not null default(getdate()) 
		ALTER TABLE dbo.WorkFlowTemplate ADD  CreatorUserId bigint    null 

-- 模板编辑增加历史记录功能
CREATE TABLE [dbo].[WorkFlowTemplateLog](
	[Id] [uniqueidentifier] NOT NULL,
	[VueTemplate] [text] NOT NULL,
	[EditUserId] [bigint] NOT NULL,
	[EditTime] [datetime] NOT NULL,
	[TemplateId] [uniqueidentifier] NOT NULL,
	[VersionNum] [int] NOT NULL,
	[LastLockIP] [varchar](100) NOT NULL,
 CONSTRAINT [PK_WorkFlowTemplateLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

--项目信息增加评审组字段
ALTER TABLE dbo.ProjectBase ADD  GroupId uniqueidentifier    null 