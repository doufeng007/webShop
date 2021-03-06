﻿create table EmployeePlan (
 Id                   uniqueidentifier     not null,
   IsDeleted            bit                  null,
   DeleterUserId        bigint               null,
   LastModificationTime datetime             null,
   LastModifierUserId   bigint               null,
   CreationTime         datetime             not null,
   CreatorUserId        bigint               null,
   DeletionTime         datetime             null,
   ApplyUser            nvarchar(500)        not null,
   ApplyOrgId           bigint               null,
   ApplyPostId          uniqueidentifier     null,
   ApplyNo              varchar(50)          not null,
   ApplyCount           int                  not null,
   ApplyTime            datetime             null,
   EmployeeUserIds      nvarchar(500)        null,
   MergeUserId          nvarchar(500)        null,
   RecordUserId         nvarchar(500)        null,
   Comment              nvarchar(1000)       null,
   Discuss              nvarchar(1000)       null,
   VerifyDiscuss        nvarchar(1000)       null,
   NeedAdmin            bit                  null,
   Phone nvarchar(50)           null,
   Result               int                  null,
   AdminUserId          nvarchar(500)        null,
   VerifyUserId          nvarchar(500)        null,
   AdminVerifyDiscuss   nvarchar(1000)       null,
   TenantId             int                  null,
   Status               int                  not null,
   ApplyJob             nvarchar(50)         not null,
   IsJoin bit                  null,
   JoinDes nvarchar(1000)       null,
   DealWithUsers nvarchar(500)          null,
   constraint PK_EMPLOYEEPLAN primary key (Id)
)

create table EmployeeResult (
   Id                   uniqueidentifier     not null,
   IsDeleted            bit                  null,
   DeleterUserId        bigint               null,
   LastModificationTime datetime             null,
   LastModifierUserId   bigint               null,
   CreationTime         datetime             not null,
   CreatorUserId        bigint               null,
   DeletionTime         datetime             null,
   ApplyUser            nvarchar(500)        not null,
   ApplyOrgId           bigint                null,
   ApplyPostId          uniqueidentifier      null,
   ApplyNo              varchar(50)          not null,
   ApplyCount           int                  not null,
   ApplyTime            datetime             null,
   Phone nvarchar(50)           null,
   EmployeeUserIds      nvarchar(500)        null,
   MergeUserId          nvarchar(500)        null,
   RecordUserId         nvarchar(500)        null,
   Comment              nvarchar(1000)       null,
   Discuss              nvarchar(1000)       null,
   VerifyDiscuss        nvarchar(1000)       null,
   NeedAdmin            bit                  null,
   Result               int                  null,
   AdminUserId          nvarchar(500)        null,
      VerifyUserId          nvarchar(500)        null,
   AdminVerifyDiscuss   nvarchar(1000)       null,
   EmployeePlanId       uniqueidentifier     not null,
   TenantId             int                  null,
   ApplyJob             nvarchar(50)         not null,
    IsJoin bit                  null,
   JoinDes nvarchar(1000)       null,
   constraint PK_EMPLOYEERESULT primary key (Id)
)