==============================
Author:hq
Description:����task-�����ѯ
Module:
==============================
ALTER TABLE dbo.WorkFlowTask add Inquiry VARCHAR(MAX)
ALTER TABLE dbo.WorkFlowTask add InquiryGroupID uniqueidentifier
insert into WorkFlowButtons (ID,Title,Ico,Script,Note,Sort) values (NEWID(),'�����ѯ','/Images/ico/file_edit.gif','flowInquiry();','�����ѯ',22)