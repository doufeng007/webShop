-- 评审发文使用公文发文流程
ALTER TABLE dbo.NoticeDocument ADD NoticeDocumentBusinessType INT NOT NULL DEFAULT 0
	ALTER TABLE dbo.NoticeDocument ADD Additional NVARCHAR(1000)  NULL
	ALTER TABLE dbo.NoticeDocument ADD ProjectLeader NVARCHAR(200)  NULL
	ALTER TABLE dbo.NoticeDocument ADD ProjectReviewer NVARCHAR(200)  NULL

	ALTER TABLE dbo.NoticeDocument ADD StartDate  DATETIME NULL
	ALTER TABLE dbo.NoticeDocument ADD EndDate  DATETIME NULL
	ALTER TABLE dbo.NoticeDocument ADD AuditAmount MONEY  NULL
	
	ALTER TABLE dbo.NoticeDocument ADD ProjectUndertakeCode NVARCHAR(200)  NULL
	ALTER TABLE dbo.NoticeDocument ADD SendUnitName NVARCHAR(200)  NULL