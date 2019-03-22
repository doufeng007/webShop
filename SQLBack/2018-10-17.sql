
---标记业务上需要隐藏的待办 还是用status=-1表示隐藏 该字段表示是由于业务需要隐藏的
ALTER TABLE dbo.WorkFlowTask ADD IsHideByBusiness BIT NULL 



---收文标题长度Max
ALTER TABLE dbo.EmployeeReceipt ALTER COLUMN Title NVARCHAR(max)  NOT NULL

---发文标题长度Max
ALTER TABLE dbo.NoticeDocument ALTER COLUMN Title NVARCHAR(1000)  NOT NULL
--归档备注长度Max
ALTER TABLE dbo.Docment ALTER COLUMN [Des] VARCHAR(MAX)   NULL
ALTER TABLE dbo.Docment ALTER COLUMN Name VARCHAR(max)  NOT NULL
ALTER TABLE dbo.WorkExperience alter column  EndTime datetime    null 