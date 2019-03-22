==============================
Author:hq
Description:项目查询关键字 关联 送审单位
Module:
==============================

USE [FRMSCore_Dev]
GO
/****** Object:  StoredProcedure [dbo].[spGetProjectListWithPage]    Script Date: 2018/11/1 14:15:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spGetProjectListWithPage]
	-- Add the parameters for the stored procedure here
	@userid BIGINT, 
	@searchId UNIQUEIDENTIFIER =NULL,
	@isimportent BIT = 0,
	 @isfllow BIT =0,
	 @startTime DATETIME =NULL,
	 @endTime DATETIME =NULL,
	 @searchKey NVARCHAR(200)=NULL,
	 @entrustmentNumber NVARCHAR(200)=NULL,
	 @userIdList BigIntegerTableType READONLY,
	 @groupid UNIQUEIDENTIFIER =NULL,
	 @sendunitid int =null
	 
AS
BEGIN
	
	DECLARE @ProjectIds table
	(
		Id UNIQUEIDENTIFIER,
		CreatorUserId BIGINT,
		UserId BIGINT,
		GXR_UserId BIGINT,
		HasAuth BIT
	);

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO @ProjectIds
	SELECT a.id,a.CreatorUserId,b.UserId,c.UserID AS GXR_UserId, 0 AS HasAuth FROM dbo.ProjectBase a
	LEFT JOIN dbo.ProjectAuditMember b ON a.Id = b.ProjectBaseId
	LEFT JOIN dbo.ProjectRealationUser c ON a.Id = c.InstanceID
	WHERE (a.IsDeleted IS NULL OR a.IsDeleted =0)  AND (b.IsDeleted IS NULL OR b.IsDeleted =0)
	
	MERGE INTO @ProjectIds  AS pb
	USING @userIdList ul ON ( pb.CreatorUserId  = ul.Value  ) 
	WHEN   MATCHED
	       THEN UPDATE SET pb.HasAuth = 1;

    MERGE INTO  @ProjectIds AS pb
	USING @userIdList ul ON ( pb.UserId  = ul.Value  ) 
	WHEN MATCHED
	     THEN UPDATE SET pb.HasAuth = 1;

	MERGE INTO @ProjectIds AS pb
	USING @userIdList ul ON ( pb.GXR_UserId  = ul.Value  ) 
	WHEN MATCHED
	     THEN UPDATE SET pb.HasAuth = 1;

    DECLARE @sql  VARCHAR(1000);
	SELECT p.Id,p.ProjectName,
			p.ProjectCode,
			c.Name as AppraisalTypeName,
			p.AppraisalTypeId,
			p.SendTime,
			p.EntrustmentNumber as EntrustmentNumber,
			p.SendTotalBudget,
			NEWID() as StepId,
			'' as StepName ,
			(SELECT TOP 1 FlowID FROM dbo.WorkFlowTask WHERE InstanceID= CAST(p.Id AS VARCHAR(50)) ORDER BY p.SendTime ) as FlowID ,
			NEWID() as GroupID ,
			p.Is_Important AS IsImportant
			,p.Status,p.CreationTime,p.CreatorUserId
			,CAST(( case when uf.Id is NULL then 0 else 1 end) AS BIT) as IsFollow
			,ISNULL(p.IsReturnAudit,0) AS IsReturnAudit
			,ISNULL(p.IsStop,0) AS IsStop
			,uf.Id AS ufid
			,g.Name as GroupName
			,co.Name as SendUnitName
	   from ProjectBase as p
	   join Code_AppraisalType c on p.AppraisalTypeId = c.Id
	   left join UserFollowProject uf on uf.Userid=@userid and uf.Projectid=p.Id
	   left join ProjectAuditGroup g on g.Id =p.GroupId
	   left join ConstructionOrganizations  co on co.id=p.SendUnit
	   where ((p.IsDeleted is null) or p.IsDeleted =0)  AND  p.id IN (SELECT  DISTINCT Id FROM @ProjectIds WHERE HasAuth =1)
			AND ((@isimportent=1 and Is_Important=1)or(@isimportent=0)) 
			AND ( @isfllow=0 or ( @isfllow=1 and (uf.Id IS NOT NULL)))
			AND (@startTime IS NULL  OR ( @startTime IS NOT NULL AND  SendTime>@startTime))
			AND (@endTime IS NULL  OR ( @endTime IS NOT NULL AND  SendTime<@endTime))
			AND (@searchKey IS NULL  OR ( @searchKey IS NOT NULL AND  ((ProjectName LIKE '%'+@searchKey + '%') OR (ProjectCode LIKE '%'+@searchKey + '%') OR (co.Name LIKE '%'+@searchKey + '%') )  ))
			AND (@entrustmentNumber IS NULL  OR ( @entrustmentNumber IS NOT NULL AND  EntrustmentNumber = @entrustmentNumber))
			and (@groupid is null or (p.GroupId=@groupid))
			and (@sendunitid is null or (p.SendUnit=@sendunitid))

		order by 
		IsFollow DESC,
		IsImportant DESC,
		CreationTime DESC

END
