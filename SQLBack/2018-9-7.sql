==============================
Author:zcl
Description:文件表 文件relation表 加上多租户字段
Module:
==============================
ALTER TABLE dbo.AbpFile ADD  TenantId INT NULL 
ALTER TABLE dbo.AbpFileRelation ADD  TenantId INT NULL 