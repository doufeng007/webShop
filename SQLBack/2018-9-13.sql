-- 组织架构岗位角色调整
ALTER TABLE OrganizationUnitPostsBase ADD [Level]  int  NULL  --岗位权重越小权限越大

ALTER TABLE UserPosts ADD [IsMain]  bit  not NULL default 0  --是否主岗位

CREATE TABLE [dbo].[OrganizationUnitPostsRole](  --岗位角色关联表
	[Id] [uniqueidentifier] NOT NULL,
	[OrgPostId] [uniqueidentifier] NOT NULL,
	[RoleId] [int] NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_OrganizationUnitPostsRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
