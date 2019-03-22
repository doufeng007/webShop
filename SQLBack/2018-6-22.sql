==============================
Author: zcl
Description: 出差请假加班 时长变小数 精确到1位小数
Module:Project+HR
Date:
==============================

ALTER TABLE dbo.EmployeeAskForLeave ALTER COLUMN Hours  DECIMAL(18,2)
ALTER TABLE dbo.OAWorkon ALTER COLUMN Hours  DECIMAL(18,2)
ALTER TABLE dbo.OAWorkout ALTER COLUMN Hours  DECIMAL(18,2)

==============================
Author:hq
Description:工作记录-公文表
Module:项目管理
==============================
ALTER TABLE ProjectRegistration ADD DealWithUsers varchar(500)




