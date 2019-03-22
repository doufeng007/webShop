using Abp.Application.Services;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Reflection.Extensions;
using Castle.Windsor;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.UI;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;

namespace ZCYX.FRMSCore
{
    [RemoteService(IsEnabled = false)]
    public class ProjectAuditManager : ApplicationService
    {
        private readonly IRepository<ProjectAudit, Guid> _projectAuditRepository;
        private readonly IRepository<User, long> _userRepository;



        public ProjectAuditManager(IRepository<ProjectAudit, Guid> projectAuditRepository, IRepository<User, long> userRepository)
        {
            _projectAuditRepository = projectAuditRepository;
            _userRepository = userRepository;
        }

        [AbpAuthorize]
        public async Task InsertAsync(List<LogColumnModel> logs, string instanceId, string tableName)
        {
            var groupId = Guid.NewGuid();
            foreach (var item in logs)
            {
                await _projectAuditRepository.InsertAsync(new ProjectAudit
                {
                    Id = Guid.NewGuid(),
                    TaskId = Guid.Empty,
                    UserId = AbpSession.UserId.Value,
                    InstanceId = instanceId,
                    CreationTime = DateTime.Now,
                    FieldName = item.FieldName,
                    OldValue = item.OldValue,
                    NewValue = item.NewValue,
                    ChangeType = item.ChangeType,
                    TableName = tableName,
                    GroupId = groupId,
                });
            }
        }
        [AbpAuthorize]
        public void Insert(List<LogColumnModel> logs, string instanceId, string tableName)
        {
            var groupId = Guid.NewGuid();
            foreach (var item in logs)
            {
                 _projectAuditRepository.Insert(new ProjectAudit
                {
                    Id = Guid.NewGuid(),
                    TaskId = Guid.Empty,
                    UserId = AbpSession.UserId.Value,
                    InstanceId = instanceId,
                    CreationTime = DateTime.Now,
                    FieldName = item.FieldName,
                    OldValue = item.OldValue,
                    NewValue = item.NewValue,
                    ChangeType = item.ChangeType,
                    TableName = tableName,
                    GroupId = groupId,
                });
            }
        }
        [AbpAuthorize]
        public async Task InsertAsync(List<LogColumnModel> logs, string instanceId, string tableName, Guid groupId)
        {
            foreach (var item in logs)
            {
                await _projectAuditRepository.InsertAsync(new ProjectAudit
                {
                    Id = Guid.NewGuid(),
                    TaskId = Guid.Empty,
                    UserId = AbpSession.UserId.Value,
                    InstanceId = instanceId,
                    CreationTime = DateTime.Now,
                    FieldName = item.FieldName,
                    OldValue = item.OldValue,
                    NewValue = item.NewValue,
                    ChangeType = item.ChangeType,
                    TableName = tableName,
                    GroupId = groupId,
                });
            }
        }



        public async Task<List<ChangeLog>> GetChangeLog(EntityDto<Guid> input, string key)
        {
            var query = (from a in _projectAuditRepository.GetAll()
                         where a.InstanceId == input.Id.ToString() && a.TableName == key
                         group a by new { a.GroupId } into g
                         select new { CreatTime = g.First().CreationTime, g }).OrderByDescending(c => c.CreatTime);

            var logs = await query.ToListAsync();
            var ret = new List<ChangeLog>();
            foreach (var item in logs)
            {
                var firstModel = item.g.FirstOrDefault();
                var entity = new ChangeLog()
                {
                    ChangeTime = firstModel.CreationTime,
                    UserName = (await _userRepository.SingleAsync(r => r.Id == firstModel.CreatorUserId.Value)).Name,
                };
                //entity.Content = ModelColumnLogsHepler.MakeContent(item.g.ToList());
                foreach (var model in item.g)
                {
                    var logModel = new LogColumnModel()
                    {
                        ChangeType = model.ChangeType,
                        FieldName = model.FieldName,
                        NewValue = model.NewValue,
                        OldValue = model.OldValue,
                    };
                    entity.ContentModel.Add(logModel);
                }
                ret.Add(entity);
            }
            return ret;
        }

    }
}
