alter table [dbo].[XZGLMeeting]
alter column [ModeratorId] nvarchar(max) not null
alter table [dbo].[XZGLMeeting]
alter column [ModeratorName] nvarchar(20) not null
alter table [dbo].[XZGLMeeting]
alter column [RecorderId] nvarchar(max) not null
alter table [dbo].[XZGLMeeting]
alter column [RecorderName] nvarchar(20) not null
update [XZGLMeeting] set [ModeratorId]='u_'+[ModeratorId] where [ModeratorId] is not null and len([ModeratorId])>0
update [XZGLMeeting] set [RecorderId]='u_'+[RecorderId] where [RecorderId] is not null and len([RecorderId])>0