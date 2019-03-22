==============================
Author:hq
Description:工作联系字段
Module:
==============================
alter table ProjectRegistration  add IsSummary bit not null default (0)
alter table ProjectRegistration  add RegistrationId  uniqueidentifier 
alter table ProjectRegistration  add PersonOnCharge  bigint 
alter table ProjectRegistration  add PersonOnChargeType int not null default (0)