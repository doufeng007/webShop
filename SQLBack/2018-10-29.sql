==============================
Author:hq
Description:��ɫ
Module:
==============================

ALTER TABLE dbo.AbpRoles ADD IsSelect bit NOT NULL DEFAULT 1

INSERT INTO [dbo].[AbpRoles]
           ([ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorUserId]
           ,[DisplayName]
           ,[IsDefault]
           ,[IsDeleted]
           ,[IsStatic]
           ,[Name]
           ,[NormalizedName]
           ,[IsSelect])
     VALUES
           ('931EA277-3199-4FB3-8757-710AD11D68B4'
           ,GETDATE()
           ,1
           ,'��Ŀ�����ˡ����á�'
           ,0
           ,0
           ,1
           ,'XMFZR'
           ,'XMFZR'
           ,0)
