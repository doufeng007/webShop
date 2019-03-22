==============================
Author:ZCL
Description:新增流程按钮：终止
Module:
==============================
INSERT INTO dbo.WorkFlowButtons VALUES ( NEWID(), N'终止', '/Images/ico/file_del.gif', 'taskEndStop();', '终止当前流程', 21 )