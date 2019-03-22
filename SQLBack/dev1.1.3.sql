/*
此脚本由 Visual Studio 于 2018/11/29 在 9:37 创建。
请对 192.168.0.150.FRMSCore_Test (sa) 运行此脚本，使其与 192.168.0.150.FRMSCore_Dev (sa) 相同。
此脚本按照以下顺序执行操作:
1. 禁用外键约束。
2. 执行 DELETE 命令。
3. 执行 UPDATE 命令。
4. 执行 INSERT 命令。
5. 重新启用外键约束。
请在运行此脚本之前备份目标数据库。
*/
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
/*用于文本/图像更新的指针。可能不需要该指针，但在此声明以备万一*/
DECLARE @pv binary(16)
BEGIN TRANSACTION
SET IDENTITY_INSERT [dbo].[AbpMenuBase] ON
INSERT INTO [dbo].[AbpMenuBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [Icon], [Order], [IsEnabled], [CustomData], [Target], [RequiresAuthentication], [IsVisible], [Url], [RequiredPermissionName], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId]) VALUES (20115, NULL, 20077, N'个人财务', N'CWGL.GRCW', N'CWGL', N'个人财务', N'', 0, 0, N'', N'', 1, 0, N'/cwgl/grcw', N'', '20181128 19:19:37.5719070', 1, NULL, NULL, 0, NULL, NULL)
INSERT INTO [dbo].[AbpMenuBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [Icon], [Order], [IsEnabled], [CustomData], [Target], [RequiresAuthentication], [IsVisible], [Url], [RequiredPermissionName], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId]) VALUES (20116, NULL, 38, N'绩效查看', N'HR.JXCK', N'', N'', N'', 11, 1, N'', N'', 1, 0, N'', N'HR.JXCK', '20181128 20:02:44.1704324', 1, NULL, NULL, 0, NULL, NULL)
INSERT INTO [dbo].[AbpMenuBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [Icon], [Order], [IsEnabled], [CustomData], [Target], [RequiresAuthentication], [IsVisible], [Url], [RequiredPermissionName], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId]) VALUES (20117, NULL, 38, N'制度查看', N'HR.ZDCK', N'HR', N'', N'', 12, 1, N'', N'', 1, 0, N'', N'HR.ZDCK', '20181128 20:03:22.1518721', 1, NULL, NULL, 0, NULL, NULL)
INSERT INTO [dbo].[AbpMenuBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [Icon], [Order], [IsEnabled], [CustomData], [Target], [RequiresAuthentication], [IsVisible], [Url], [RequiredPermissionName], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId]) VALUES (20118, NULL, 38, N'绩效管理', N'HR.JXGL', N'HR', N'', N'', 13, 0, N'', N'', 1, 0, N'', N'HR.JXGL', '20181128 20:04:17.3734044', 1, NULL, NULL, 0, NULL, NULL)
INSERT INTO [dbo].[AbpMenuBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [Icon], [Order], [IsEnabled], [CustomData], [Target], [RequiresAuthentication], [IsVisible], [Url], [RequiredPermissionName], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId]) VALUES (20119, NULL, 38, N'制度管理', N'HR.ZDGL', N'HR', N'', N'', 0, 0, N'', N'', 1, 0, N'', N'HR.ZDGL', '20181128 20:05:00.6634740', 1, NULL, NULL, 0, NULL, NULL)
INSERT INTO [dbo].[AbpMenuBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [Icon], [Order], [IsEnabled], [CustomData], [Target], [RequiresAuthentication], [IsVisible], [Url], [RequiredPermissionName], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId]) VALUES (20120, NULL, 10062, N'单位信息', N'HR.CompanyInfo', N'XZGL', N'', N'', 0, 0, N'', NULL, 0, 0, N'/rlzy/danwei', N'', '20181128 20:13:43.0221275', NULL, NULL, NULL, 0, '20181128 20:13:43.0847949', 1)
SET IDENTITY_INSERT [dbo].[AbpMenuBase] OFF
COMMIT TRANSACTION

BEGIN TRANSACTION
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-people', [CreationTime]='20180731 15:24:58.1096780', [LastModificationTime]='20180731 15:24:58.1535742' WHERE [Id]=3
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-people', [CreationTime]='20180731 15:25:37.7917969', [LastModificationTime]='20180731 15:25:37.8272960' WHERE [Id]=4
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-share', [CreationTime]='20180731 15:26:59.6384129', [LastModificationTime]='20180731 15:26:59.6736100' WHERE [Id]=5
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'logo-buffer', [CreationTime]='20180731 15:27:30.3815418', [LastModificationTime]='20180731 15:27:30.4213937' WHERE [Id]=6
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-egg', [CreationTime]='20180731 15:27:50.7772668', [LastModificationTime]='20180731 15:27:50.8166154' WHERE [Id]=7
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-chatbubbles', [CreationTime]='20180802 16:28:18.5278377', [LastModificationTime]='20180802 16:28:18.5407882' WHERE [Id]=11
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-chatboxes', [CreationTime]='20180802 16:27:41.0312995', [LastModificationTime]='20180802 16:27:41.0646986' WHERE [Id]=13
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-create', [CreationTime]='20180802 16:30:34.7847599', [LastModificationTime]='20180802 16:30:34.8263873' WHERE [Id]=14
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-bookmark', [CreationTime]='20180802 16:33:30.8179562', [LastModificationTime]='20180802 16:33:30.8226625' WHERE [Id]=15
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-search', [CreationTime]='20180802 16:34:44.9000572', [LastModificationTime]='20180802 16:34:44.9120568' WHERE [Id]=16
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-at', [CreationTime]='20180802 16:27:08.4693293', [LastModificationTime]='20180802 16:27:08.4831470' WHERE [Id]=18
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-book', [CreationTime]='20181127 09:11:50.2075939', [LastModificationTime]='20181127 09:11:50.2975039' WHERE [Id]=26
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-brush', [CreationTime]='20180802 16:35:51.9877244', [LastModificationTime]='20180802 16:35:52.0297007' WHERE [Id]=28
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-calculator', [CreationTime]='20180607 10:24:02.7231960', [LastModificationTime]='20180607 10:24:02.7566597' WHERE [Id]=29
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-trash', [CreationTime]='20180802 16:46:28.3404720', [DeleterUserId]=1, [DeletionTime]='20181128 14:58:44.0445703', [IsDeleted]=1, [LastModificationTime]='20180802 16:46:28.5619283' WHERE [Id]=31
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-build', [CreationTime]='20180802 16:49:45.4507401', [DeleterUserId]=1, [DeletionTime]='20181128 14:58:52.1069752', [IsDeleted]=1, [LastModificationTime]='20180802 16:49:45.4670069' WHERE [Id]=32
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-cog', [CreationTime]='20181127 09:12:29.8049794', [LastModificationTime]='20181127 09:12:29.9503031' WHERE [Id]=33
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-man', [CreationTime]='20180731 14:44:08.3482739', [LastModificationTime]='20180731 14:44:08.5489847' WHERE [Id]=39
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-people', [CreationTime]='20180731 14:45:13.2595363', [LastModificationTime]='20180731 14:45:13.2907725' WHERE [Id]=40
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-pricetags', [CreationTime]='20180731 14:45:51.3152658', [LastModificationTime]='20180731 14:45:51.3513421' WHERE [Id]=41
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-flag', [CreationTime]='20180731 14:46:22.8839843', [LastModificationTime]='20180731 14:46:22.9229972' WHERE [Id]=42
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-home', [CreationTime]='20180802 16:49:59.5894424', [LastModificationTime]='20180802 16:49:59.6311235' WHERE [Id]=10035
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-briefcase', [CreationTime]='20180802 16:50:15.3713587', [LastModificationTime]='20180802 16:50:15.4095251' WHERE [Id]=10036
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-paper-plane', [CreationTime]='20180802 17:00:38.0645268', [LastModificationTime]='20180802 17:00:38.0804096' WHERE [Id]=10037
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-appstore', [CreationTime]='20180802 16:54:19.1132469', [LastModificationTime]='20180802 16:54:19.1478210' WHERE [Id]=10038
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-pin', [CreationTime]='20180802 17:18:50.4059328', [LastModificationTime]='20180802 17:18:50.4446929' WHERE [Id]=10039
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-people', [CreationTime]='20180802 16:59:59.0944370', [LastModificationTime]='20180802 16:59:59.1385600' WHERE [Id]=10040
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-browsers', [CreationTime]='20180802 17:00:08.3661233', [LastModificationTime]='20180802 17:00:08.3770016' WHERE [Id]=10041
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-easel', [CreationTime]='20180731 14:49:50.6483536', [LastModificationTime]='20180731 14:49:50.6899212' WHERE [Id]=10044
UPDATE [dbo].[AbpMenuBase] SET [ParentId]=NULL, [Icon]=N'md-git-merge', [IsVisible]=1, [RequiredPermissionName]=NULL, [CreationTime]='20181128 20:11:36.5992860', [LastModificationTime]='20181128 20:11:36.7787424' WHERE [Id]=10045
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-funnel', [IsVisible]=1, [CreationTime]='20181128 20:10:58.3374349', [LastModificationTime]='20181128 20:10:58.4226958' WHERE [Id]=10046
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-download', [CreationTime]='20180607 10:40:42.6323478', [LastModificationTime]='20180607 10:40:42.9644835' WHERE [Id]=10048
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-download', [CreationTime]='20180607 10:41:01.9863671', [LastModificationTime]='20180607 10:41:02.0343038' WHERE [Id]=10049
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-open', [CreationTime]='20180731 15:15:55.5157454', [LastModificationTime]='20180731 15:15:55.7088540' WHERE [Id]=10050
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-open', [Url]=N'/gwgl/fw', [CreationTime]='20181126 11:17:06.0183482', [LastModificationTime]='20181126 11:17:06.0960229' WHERE [Id]=10051
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-list-box', [IsVisible]=1, [CreationTime]='20181128 20:11:17.2012308', [LastModificationTime]='20181128 20:11:17.3270480' WHERE [Id]=10052
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-card', [CreationTime]='20180802 17:19:30.9994836', [LastModificationTime]='20180802 17:19:31.0399931' WHERE [Id]=10055
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-lock', [CreationTime]='20180802 17:21:05.3322324', [LastModificationTime]='20180802 17:21:05.3624728' WHERE [Id]=10056
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-menu', [CreationTime]='20181122 20:50:19.5967024', [LastModificationTime]='20181122 20:50:19.6138485' WHERE [Id]=10058
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-open', [CreationTime]='20180731 14:52:30.3288653', [DeleterUserId]=1, [DeletionTime]='20181124 10:40:34.3807647', [LastModificationTime]='20180731 14:52:30.3747817' WHERE [Id]=10059
UPDATE [dbo].[AbpMenuBase] SET [DeleterUserId]=NULL, [DeletionTime]=NULL WHERE [Id]=10060
UPDATE [dbo].[AbpMenuBase] SET [CreationTime]='20181124 13:52:54.4448272', [LastModificationTime]='20181124 13:52:54.5151504' WHERE [Id]=10061
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-car', [CreationTime]='20180731 15:18:15.9159153', [LastModificationTime]='20180731 15:18:15.9616792' WHERE [Id]=10063
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-folder', [CreationTime]='20180802 17:21:27.0685241', [LastModificationTime]='20180802 17:21:27.0816070' WHERE [Id]=10069
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-clipboard', [CreationTime]='20180731 15:19:16.5076251', [LastModificationTime]='20180731 15:19:16.5520809' WHERE [Id]=10070
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-people', [CreationTime]='20180607 10:44:04.0301553', [LastModificationTime]='20180607 10:44:04.0627566' WHERE [Id]=10071
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-medkit', [CreationTime]='20180731 14:47:44.6642084', [LastModificationTime]='20180731 14:47:44.7060122' WHERE [Id]=10072
UPDATE [dbo].[AbpMenuBase] SET [DisplayName]=N'内外勤', [Icon]=N'md-clock', [CreationTime]='20181128 19:59:06.3983124', [LastModificationTime]='20181128 19:59:06.5935271' WHERE [Id]=10073
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-document', [IsVisible]=0, [CreationTime]='20180731 14:53:56.5754368', [LastModificationTime]='20180731 14:53:56.6137632' WHERE [Id]=10074
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-cash', [CreationTime]='20180731 15:19:52.0914100', [LastModificationTime]='20180731 15:19:52.1102567' WHERE [Id]=20078
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-clipboard', [CreationTime]='20180731 15:20:12.8787678', [LastModificationTime]='20180731 15:20:12.9037689' WHERE [Id]=20079
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-card', [CreationTime]='20180731 15:20:31.5935253', [LastModificationTime]='20180731 15:20:31.6221775' WHERE [Id]=20081
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'logo-yen', [CreationTime]='20180731 15:20:59.2918211', [LastModificationTime]='20180731 15:20:59.3376652' WHERE [Id]=20082
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-clipboard', [CreationTime]='20180731 15:23:57.8685764', [LastModificationTime]='20180731 15:23:57.9105303' WHERE [Id]=20083
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-calendar-outline', [CreationTime]='20180716 09:54:04.3315982', [LastModificationTime]='20180716 09:54:04.3614885' WHERE [Id]=20084
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-hand', [CreationTime]='20180731 14:50:45.6307487', [LastModificationTime]='20180731 14:50:45.6839109' WHERE [Id]=20085
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-people-outline', [IsVisible]=1, [CreationTime]='20181128 20:11:51.7818165', [LastModificationTime]='20181128 20:11:51.8759630' WHERE [Id]=20086
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'logo-markdown', [IsVisible]=1, [CreationTime]='20181128 20:12:02.4853277', [LastModificationTime]='20181128 20:12:02.6249112' WHERE [Id]=20087
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-briefcase', [CreationTime]='20180731 15:23:39.5324274', [LastModificationTime]='20180731 15:23:39.5662654' WHERE [Id]=20088
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-trending-down', [CreationTime]='20180731 15:22:21.2006821', [LastModificationTime]='20180731 15:22:21.2389016' WHERE [Id]=20089
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-trending-up', [CreationTime]='20180731 15:22:40.5079352', [LastModificationTime]='20180731 15:22:40.5429260' WHERE [Id]=20090
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-create-outline', [CreationTime]='20180807 15:22:09.2411194', [LastModificationTime]='20180807 15:22:09.4302269' WHERE [Id]=20091
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-list-box-outline', [CreationTime]='20180815 16:15:52.0348280', [LastModificationTime]='20180815 16:15:52.2278893' WHERE [Id]=20092
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-list-box', [CreationTime]='20180912 11:10:12.5118069', [LastModificationTime]='20180912 11:10:12.5433917' WHERE [Id]=20094
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-map-outline', [CreationTime]='20180912 11:13:13.7321248', [LastModificationTime]='20180912 11:13:13.7477914' WHERE [Id]=20095
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'md-trending-up', [CreationTime]='20180912 11:15:42.2124850', [LastModificationTime]='20180912 11:15:42.4366090' WHERE [Id]=20097
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-paper', [CreationTime]='20180912 11:06:49.0003958', [LastModificationTime]='20180912 11:06:49.0486400' WHERE [Id]=20103
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-recording', [CreationTime]='20180912 11:07:16.9912390', [LastModificationTime]='20180912 11:07:17.0563850' WHERE [Id]=20104
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-calendar-outline', [CreationTime]='20180927 21:39:13.8177470', [LastModificationTime]='20180927 21:39:13.8276526' WHERE [Id]=20105
UPDATE [dbo].[AbpMenuBase] SET [Icon]=N'ios-send-outline', [CreationTime]='20181008 17:01:37.5252248', [LastModificationTime]='20181008 17:01:37.7361237' WHERE [Id]=20106
UPDATE [dbo].[AbpMenuBase] SET [ParentId]=10062, [Target]=NULL, [RequiresAuthentication]=0, [CreationTime]='20181128 20:09:58.9475528', [CreatorUserId]=NULL, [DeleterUserId]=1, [DeletionTime]='20181128 20:10:05.6806935', [LastModificationTime]='20181128 20:09:59.2822468', [LastModifierUserId]=1 WHERE [Id]=20110
UPDATE [dbo].[AbpMenuBase] SET [DeleterUserId]=1, [DeletionTime]='20181128 20:10:23.9179668', [IsDeleted]=1 WHERE [Id]=20111
UPDATE [dbo].[AbpMenuBase] SET [DeleterUserId]=1, [DeletionTime]='20181128 20:10:18.6918400', [IsDeleted]=1 WHERE [Id]=20112
UPDATE [dbo].[AbpMenuBase] SET [RequiredPermissionName]=N'', [DeleterUserId]=1, [DeletionTime]='20181128 20:10:25.7363311', [IsDeleted]=1 WHERE [Id]=20114
COMMIT TRANSACTION

BEGIN TRANSACTION
UPDATE [dbo].[AbpPermissionBase] SET [ParentId]=10069, [Code]=N'XZGL.yzgl', [MoudleName]=N'XZGL', [CreationTime]='20181129 09:42:49.5115069', [CreatorUserId]=NULL, [LastModificationTime]='20181129 09:42:49.7934290', [LastModifierUserId]=1 WHERE [Id]=20115
SET IDENTITY_INSERT [dbo].[AbpPermissionBase] ON
INSERT INTO [dbo].[AbpPermissionBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Order]) VALUES (20119, NULL, 18, N'绩效查看', N'HR.JXCK', N'HR', N'', '20181128 20:00:13.9345557', 1, NULL, NULL, 0, NULL, NULL, 0)
INSERT INTO [dbo].[AbpPermissionBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Order]) VALUES (20120, NULL, 18, N'制度查看', N'HR.ZDCK', N'HR', N'', '20181128 20:00:49.0055385', 1, NULL, NULL, 0, NULL, NULL, 0)
INSERT INTO [dbo].[AbpPermissionBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Order]) VALUES (20121, NULL, 18, N'绩效管理', N'HR.JXGL', N'HR', N'', '20181128 20:01:20.6315982', 1, NULL, NULL, 0, NULL, NULL, 0)
INSERT INTO [dbo].[AbpPermissionBase] ([Id], [TenantId], [ParentId], [DisplayName], [Code], [MoudleName], [Description], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Order]) VALUES (20122, NULL, 18, N'制度管理', N'HR.ZDGL', N'HR', N'', '20181128 20:01:54.1481632', 1, NULL, NULL, 0, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[AbpPermissionBase] OFF
COMMIT TRANSACTION

BEGIN TRANSACTION
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'0715b405-dec8-4c1c-9b2b-08d652005f29', N'', N'', N'', N'1ebe4b1d-aa3f-4105-9b22-08d652005f29', 0, N'空调费', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:33:12.883')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'0a15684f-76fb-4bf8-9b2c-08d652005f29', N'', N'', N'', N'f9f15711-7490-487b-9b23-08d652005f29', 0, N'证件', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:33:34.297')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'0e838242-9fb7-4951-9b25-08d652005f29', N'', N'', N'', N'2e312319-e9aa-456c-9b21-08d652005f29', 0, N'门禁', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:32:20.183')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'1274ece2-6f4d-46aa-9b2f-08d652005f29', N'', N'', N'', N'f9f15711-7490-487b-9b23-08d652005f29', 0, N'虚拟', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:33:50.943')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'1ebe4b1d-aa3f-4105-9b22-08d652005f29', N'', N'', N'', N'143aad16-adf1-47e7-38e0-08d55351d147', 0, N'费用类型', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:31:40.977')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'2e312319-e9aa-456c-9b21-08d652005f29', N'', N'', N'', N'143aad16-adf1-47e7-38e0-08d55351d147', 0, N'物业类型', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:31:26.950')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'60a34b6a-a65c-4dba-9b2d-08d652005f29', N'', N'', N'', N'f9f15711-7490-487b-9b23-08d652005f29', 0, N'证书', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:33:39.980')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'72fa4cbd-0639-4d4d-9b24-08d652005f29', N'', N'', N'', N'2e312319-e9aa-456c-9b21-08d652005f29', 0, N'监控', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:32:12.363')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'8bd43558-739a-46c7-9b2a-08d652005f29', N'', N'', N'', N'1ebe4b1d-aa3f-4105-9b22-08d652005f29', 0, N'电话费', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:33:05.580')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'a2866c46-fb00-4ead-9b27-08d652005f29', N'', N'', N'', N'2e312319-e9aa-456c-9b21-08d652005f29', 0, N'安全保卫', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:32:37.650')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'a7e44318-f6e7-4f83-9b2e-08d652005f29', N'', N'', N'', N'f9f15711-7490-487b-9b23-08d652005f29', 0, N'荣誉', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:33:45.637')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'b992de72-32e1-48fe-9b29-08d652005f29', N'', N'', N'', N'1ebe4b1d-aa3f-4105-9b22-08d652005f29', 0, N'电费', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:32:59.540')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'e2f53448-462f-4209-9b28-08d652005f29', N'', N'', N'', N'1ebe4b1d-aa3f-4105-9b22-08d652005f29', 0, N'水费', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:32:54.250')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'ebd48ac8-3d5d-4bbe-9b26-08d652005f29', N'', N'', N'', N'2e312319-e9aa-456c-9b21-08d652005f29', 0, N'环境卫生', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:32:27.870')
INSERT INTO [dbo].[Dictionary] ([Id], [Code], [Note], [Other], [ParentID], [Sort], [Title], [Value], [IsDeleted], [DeleterUserId], [DeletionTime], [LastModificationTime], [LastModifierUserId], [CreatorUserId], [CreationTime]) VALUES (N'f9f15711-7490-487b-9b23-08d652005f29', N'', N'', N'', N'143aad16-adf1-47e7-38e0-08d55351d147', 0, N'单位信息类型', N'', 0, NULL, NULL, NULL, NULL, 1, '20181124 19:31:58.080')
COMMIT TRANSACTION