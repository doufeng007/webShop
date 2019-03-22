==============================
Author:ZCL
Description:workflowtask 加上instanceId的非聚集索引
Module:
==============================

ALTER TABLE dbo.WorkFlowTask ALTER COLUMN InstanceID NVARCHAR(200) NOT NULL

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_WorkFlowTask_InstanceID]    Script Date: 2018/7/20 14:36:05 ******/
CREATE NONCLUSTERED INDEX [IX_WorkFlowTask_InstanceID] ON [dbo].[WorkFlowTask]
(
	[InstanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


