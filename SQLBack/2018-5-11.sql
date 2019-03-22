==============================
Author: zcl
Description:
Module:  WorkFlow
Date:
==============================

ALTER TABLE dbo.WorkFlow ADD VersionNum INT NOT NULL DEFAULT 0
ALTER TABLE dbo.WorkFlowTask ADD VersionNum INT NOT NULL DEFAULT 0



CREATE TABLE [dbo].[WorkFlowVersionNum](
	[Id] [UNIQUEIDENTIFIER] NOT NULL,
	[FlowId] [UNIQUEIDENTIFIER] NOT NULL,
	[VersionNum] [INT] NOT NULL,
	[RunJSON] [NVARCHAR](MAX) NULL,
	[TenantId] [INT]  NULL,
 CONSTRAINT [PK_WorkFlowVersionNum] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


