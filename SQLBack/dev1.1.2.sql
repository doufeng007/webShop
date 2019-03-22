/*
版本：dev1.1.2
更新内容：档案管理菜单调整
*/

/*用于文本/图像更新的指针。可能不需要该指针，但在此声明以备万一*/
BEGIN TRANSACTION
UPDATE [dbo].[AbpMenuBase] SET [DisplayName]=N'借阅管理', [Icon]=N'md-happy', [RequiredPermissionName]=NULL, [CreationTime]='20181115 14:10:39.0495626', [LastModificationTime]='20181115 14:10:39.1328014' WHERE [Id]=10060
UPDATE [dbo].[AbpMenuBase] SET [DisplayName]=N'流转记录', [Code]=N'Docment.gly_lzjl', [Description]=N'所有档案流转记录管理', [Icon]=N'ios-book', [Url]=N'/dagl/gly_lzjl', [CreationTime]='20181122 20:53:54.0630187', [LastModificationTime]='20181122 20:53:54.0707030' WHERE [Id]=10061
UPDATE [dbo].[AbpMenuBase] SET [ParentId]=10057, [DisplayName]=N'文件袋', [Code]=N'Docment.DAD', [MoudleName]=N'Docment', [Icon]=N'md-briefcase', [IsVisible]=0, [Url]=N'/dagl/dad', [CreationTime]='20181124 10:22:26.3203141', [LastModificationTime]='20181124 10:22:26.3267131' WHERE [Id]=10067
INSERT INTO [dbo].[WorkFlowButtons] ([ID], [Title], [Ico], [Script], [Note], [Sort]) VALUES (N'3ee69b7d-3f5c-45bf-92dd-ef0d05aea370', N'接收', NULL, N'sureback()', N'扫码确认归还档案', 22)
INSERT INTO [dbo].[WorkFlowButtons] ([ID], [Title], [Ico], [Script], [Note], [Sort]) VALUES (N'3ee69b7d-3f5c-45bf-92dd-ef0d05aea379', N'确认领取', NULL, N'sureget()', N'扫码确认领取档案', 21)
INSERT INTO [dbo].[WorkFlowButtons] ([ID], [Title], [Ico], [Script], [Note], [Sort]) VALUES (N'dc4ca98d-313d-488d-b17d-ddf753faa8ef', N'办结归档', N'/Images/ico/file_edit.gif', N'flowCreateDocument();', N'办结归档', 23)
INSERT INTO [dbo].[WorkFlowButtons] ([ID], [Title], [Ico], [Script], [Note], [Sort]) VALUES (N'e0ecef52-6d28-4d72-9aa2-e9528ebc8cf4', N'创办任务', N'/Images/ico/file_edit.gif', N'flowCreateTask();', N'创办任务', 24)
INSERT INTO [dbo].[WorkFlowButtons] ([ID], [Title], [Ico], [Script], [Note], [Sort]) VALUES (N'9dab426a-6a4e-4bbc-a504-4e05d8a93e45', N'发送加签', N'/Images/ico/arrow_medium_right.png', N'flowAddWriteAndSend();', N'发送到下一步', 25)
INSERT INTO [dbo].[WorkFlowButtons] ([ID], [Title], [Ico], [Script], [Note], [Sort]) VALUES (N'c7fc97e0-a6ae-46e6-bce2-8fb3e1661372', N'提交并登记下一个', NULL, N'flowSend(1);', N'提交并登记下一个', 23)

DELETE EmployeeReceipt 
ALTER TABLE EmployeeReceipt DROP COLUMN DocType
COMMIT TRANSACTION
/*
公章管理相关sql
*/
BEGIN TRANSACTION
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-book', [RequiresAuthentication]=1, [CreationTime]='20180331 17:16:18.3370000', [CreatorUserId]=1, [LastModificationTime]=NULL, [LastModifierUserId]=NULL WHERE [Id]=8
UPDATE [dbo].[AbpMenuBase] SET [DisplayName]=N'用品管理', [Icon]=N'md-cube', [CreationTime]='20181126 16:43:11.4480607', [LastModificationTime]='20181126 16:43:11.4792507' WHERE [Id]=30

SET IDENTITY_INSERT [dbo].[AbpMenuBase] ON
INSERT INTO [dbo].[AbpMenuBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [Icon], [Order], [IsEnabled], [CustomData], [Target], [RequiresAuthentication], [IsVisible], [Url], [RequiredPermissionName], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId]) VALUES (20108, NULL, 10047, N'公文辅助', N'GWGL.GWFZ', N'GWGL', N'', N'', 0, 0, N'', NULL, 0, 1, N'/gwgl/gwfz', N'GWGL.gwfz', '20181126 13:38:22.4688134', NULL, NULL, NULL, 0, '20181126 13:38:22.4762410', 1)
INSERT INTO [dbo].[AbpMenuBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [Icon], [Order], [IsEnabled], [CustomData], [Target], [RequiresAuthentication], [IsVisible], [Url], [RequiredPermissionName], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId]) VALUES (20109, NULL, 10047, N'用章管理', N'GWGL.YZGL', N'GWGL', N'', N'', 0, 0, N'', N'', 1, 1, N'/gwgl/yzgl', N'GWGL.yzgl', '20181126 13:38:06.3524663', 1, NULL, NULL, 0, NULL, NULL)
INSERT INTO [dbo].[AbpMenuBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [Icon], [Order], [IsEnabled], [CustomData], [Target], [RequiresAuthentication], [IsVisible], [Url], [RequiredPermissionName], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId]) VALUES (20110, NULL, 38, N'后勤管理', N'HR.HouQin', N'XZGL', N'后勤管理', N'', 0, 0, N'', N'', 1, 1, N'/rlzy/houqin', NULL, '20181126 14:12:30.4313547', 1, NULL, NULL, 0, NULL, NULL)
INSERT INTO [dbo].[AbpMenuBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [Icon], [Order], [IsEnabled], [CustomData], [Target], [RequiresAuthentication], [IsVisible], [Url], [RequiredPermissionName], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId]) VALUES (20111, NULL, 38, N'单位信息', N'HR.CompanyInfo', N'XZGL', N'', N'', 0, 0, N'', N'', 1, 1, N'/rlzy/danwei', N'', '20181126 14:13:29.9746632', 1, NULL, NULL, 0, NULL, NULL)
INSERT INTO [dbo].[AbpMenuBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [Icon], [Order], [IsEnabled], [CustomData], [Target], [RequiresAuthentication], [IsVisible], [Url], [RequiredPermissionName], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId]) VALUES (20112, NULL, 10062, N'物业管理', N'XZGL.WYGL', N'XZGL', N'', N'', 6, 0, N'', NULL, 0, 0, N'', N'XZGL.WYGL', '20181126 17:35:24.4089918', NULL, NULL, NULL, 0, '20181126 17:35:24.4650215', 1)
INSERT INTO [dbo].[AbpMenuBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [Icon], [Order], [IsEnabled], [CustomData], [Target], [RequiresAuthentication], [IsVisible], [Url], [RequiredPermissionName], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId]) VALUES (20113, NULL, 10062, N'费用管理', N'XZGL.FYGL', N'XZGL', N'', N'', 7, 0, N'', N'', 1, 0, N'', N'XZGL.FYGL', '20181126 17:36:39.4048565', 1, 1, '20181126 17:43:38.4122934', 1, NULL, NULL)
INSERT INTO [dbo].[AbpMenuBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [Icon], [Order], [IsEnabled], [CustomData], [Target], [RequiresAuthentication], [IsVisible], [Url], [RequiredPermissionName], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId]) VALUES (20114, NULL, 10062, N'单位信息', N'XZGL.DWXX', N'XZGL', N'', N'', 8, 0, N'', N'', 1, 0, N'', N'XZGL.DWXX', '20181126 17:37:59.9355294', 1, NULL, NULL, 0, NULL, NULL)

INSERT INTO [dbo].[AbpPermissionBase] ( [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Order]) VALUES ( NULL, 10055, N'公文辅助', N'GWGL.gwfz', N'GWGL', N'', '20181126 13:35:43.3645567', 1, NULL, NULL, 0, NULL, NULL, 0)
INSERT INTO [dbo].[AbpPermissionBase] ( [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Order]) VALUES ( NULL, 10055, N'用章管理', N'GWGL.yzgl', N'GWGL', N'', '20181126 13:36:07.3616067', 1, NULL, NULL, 0, NULL, NULL, 0)
INSERT INTO [dbo].[AbpPermissionBase] ( [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Order]) VALUES ( NULL, 10069, N'物业管理', N'XZGL.WYGL', N'XZGL', N'', '20181126 17:34:04.3500953', NULL, NULL, NULL, 0, '20181126 17:34:04.4114409', 1, 6)
INSERT INTO [dbo].[AbpPermissionBase] ( [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Order]) VALUES ( NULL, 10069, N'费用管理', N'XZGL.FYGL', N'XZGL', N'', '20181126 17:33:01.7862914', 1, NULL, NULL, 0, NULL, NULL, 6)
INSERT INTO [dbo].[AbpPermissionBase] ( [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Order]) VALUES ( NULL, 10069, N'单位信息', N'XZGL.DWXX', N'XZGL', N'', '20181126 17:33:31.2776807', 1, NULL, NULL, 0, NULL, NULL, 7)

SET IDENTITY_INSERT [dbo].[AbpMenuBase] OFF
COMMIT TRANSACTION
/*
版本：dev1.1.2
更新内容：所有部门补充创建“分管领导岗”“部门领导岗”

INSERT INTO PostInfo
VALUES
( '{335934f2-2036-4872-be8e-55d3249304f2}', N'分管领导', NULL, 0, 0, NULL, NULL, NULL, N'2018-11-23T11:51:22.42', 1, NULL, NULL ), 
( '{bbf326de-b834-4971-9bcf-f3497e00308c}', N'部门领导', NULL, 0, 0, NULL, NULL, NULL, N'2018-11-23T11:51:40.357', 1, NULL, NULL )

DECLARE @Id bigint 
DECLARE My_Cursor CURSOR 
FOR (SELECT id FROM dbo.AbpOrganizationUnits)   
OPEN My_Cursor;   
FETCH NEXT FROM My_Cursor INTO @Id; 
WHILE @@FETCH_STATUS = 0  
    BEGIN  
        PRINT @Id; 
		if (select count(*) from  OrganizationUnitPostsBase where OrganizationUnitId=@Id and level=0) = 0
		begin
		INSERT INTO OrganizationUnitPostsBase
VALUES
( NEWID(), @Id, '{335934f2-2036-4872-be8e-55d3249304f2}', 10, 0, NULL, NULL, NULL, N'2018-08-14T15:36:59.9', 1, NULL, NULL, 0, 'OrganizationUnitPosts', NULL, NULL, 0 )
		end
		if (select count(*) from  OrganizationUnitPostsBase where OrganizationUnitId=@Id and level=1) = 0
		begin
		INSERT INTO OrganizationUnitPostsBase
VALUES
( NEWID(), @Id, '{bbf326de-b834-4971-9bcf-f3497e00308c}', 10, 0, NULL, NULL, NULL, N'2018-08-14T15:36:59.9', 1, NULL, NULL, 0, 'OrganizationUnitPosts', NULL, NULL, 1 )
        end
        FETCH NEXT FROM My_Cursor INTO @Id; 
		  
    END  
CLOSE My_Cursor; 
DEALLOCATE My_Cursor; */