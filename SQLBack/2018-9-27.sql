ALTER TABLE dbo.ArchivesManager ADD  DealWithUsers nvarchar(500) NULL 

ALTER TABLE dbo.[Docment] ADD  IsProject bit default(0) not null
ALTER TABLE dbo.[Docment] ADD  ArchiveId uniqueidentifier null