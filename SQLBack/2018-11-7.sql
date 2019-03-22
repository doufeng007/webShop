==============================
Author:hq
Description:权限关联
Module:
==============================
alter table EmployeeAskForLeave ADD  RelationUserId bigint NULL 
alter table OAWorkout ADD  RelationUserId bigint NULL 
alter table AbpUserRoles ADD  RelationId uniqueidentifier NULL  
alter table WorkFlowTask ADD  RelationId uniqueidentifier NULL  
alter table AbpPermissions ADD  RelationId uniqueidentifier NULL  
alter table AbpUserRoles ADD  Discriminator nvarchar(max) NULL  
update AbpUserRoles set Discriminator= 'UserRole'


CREATE TABLE [dbo].[RoleRelation](
	[Id] [uniqueidentifier] NOT NULL,
	[RelationId] [uniqueidentifier] NOT NULL,
	[Type] [int] NOT NULL,
	[RelationUserId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[Roles] [nvarchar](max) NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[TenantId] [int] NULL,
	[DeletionTime] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[DeleterUserId] [bigint] NULL,
	[LastModificationTime] [datetime] NULL,
	[LastModifierUserId] [bigint] NULL,
	[CreationTime] [datetime] NOT NULL,
	[CreatorUserId] [bigint] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO