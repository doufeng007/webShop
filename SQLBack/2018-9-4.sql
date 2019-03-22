==============================
Author:hq
Description:流程task-意见征询
Module:
==============================
ALTER TABLE dbo.WorkFlowTask add Inquiry VARCHAR(MAX)
ALTER TABLE dbo.WorkFlowTask add InquiryGroupID uniqueidentifier
insert into WorkFlowButtons (ID,Title,Ico,Script,Note,Sort) values (NEWID(),'意见征询','/Images/ico/file_edit.gif','flowInquiry();','意见征询',22)