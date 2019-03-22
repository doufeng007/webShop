using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.WorkFlow;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Model;
using Abp.File;

namespace Project
{
    public class WorkRegistrationAppService : ApplicationService, IWorkRegistrationAppService
    {
        private readonly IRepository<ProjectRegistration, Guid> _projectRegistrationRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectBaseRepository;
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<NoticeDocument, Guid> _noticeRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;

        public WorkRegistrationAppService(IRepository<ProjectRegistration, Guid> projectRegistrationRepository, IRepository<User, long> userRepository,
            IRepository<NoticeDocument, Guid> noticeRepository, IProjectBaseRepository projectBaseRepository,IAbpFileRelationAppService abpFileRelationAppService, IRepository<SingleProjectInfo, Guid> singleProjectBaseRepository)
        {
            _projectRegistrationRepository = projectRegistrationRepository;
            _userRepository = userRepository;
            _noticeRepository = noticeRepository;
            _projectBaseRepository = projectBaseRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _singleProjectBaseRepository = singleProjectBaseRepository;
        }

        public async Task<WorkRegistrationForViewOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            if (!input.Id.HasValue)
                return new WorkRegistrationForViewOutput();
            var model = await _projectRegistrationRepository.GetAsync(input.Id.Value);
            var output = model.MapTo<WorkRegistrationForViewOutput>();
            output.Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.Id.ToString(), BusinessType = (int)AbpFileBusinessType.工作联系附件 });
            return output;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<WorkRegistrationForFlowOutput> GetForEditForFlow(GetWorkFlowTaskCommentInput input)
        {
            var id = input.InstanceId.ToGuid();
            var model = await _projectRegistrationRepository.GetAsync(id);
            var output = model.MapTo<WorkRegistrationForFlowOutput>();
            output.Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = model.Id.ToString(), BusinessType = (int)AbpFileBusinessType.工作联系附件 });
            return output;
        }



        public async Task<WorkRegistrationOutput> Get(NullableIdDto<Guid> input)
        {
            if (!input.Id.HasValue)
                return new WorkRegistrationOutput();
            var model = await _projectRegistrationRepository.GetAsync(input.Id.Value);
           var singleProject= _singleProjectBaseRepository.Get(model.ProjectId.Value);
            var p = _projectBaseRepository.Get(singleProject.ProjectId);
            var output = model.MapTo<WorkRegistrationOutput>();
            output.ProjectName = p.ProjectName;
            var noticeDocument = await _noticeRepository.FirstOrDefaultAsync(r => r.ProjectRegistrationId == model.Id);
            if (noticeDocument != null)
                output.ReplyContent = noticeDocument.ReplyContent;
            if (model.PersonOnChargeType == PersonOnChargeTypeEnum.负责人)
            {
                output.List = (from a in _projectRegistrationRepository.GetAll()
                               where a.RegistrationId == model.Id && a.IsSummary
                               select new WorkRegistrationModelOutput()
                               {
                                   Title = a.Title,
                                   Content = a.Content,
                                   Id = a.Id
                               }).ToList();
            }

            output.Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.Id.ToString(), BusinessType = (int)AbpFileBusinessType.工作联系附件 });
            return output;
        }
        public async Task<WorkRegistrationForViewPublishOutput> GetPublish(EntityDto<Guid> input)
        {
            var output = new WorkRegistrationForViewPublishOutput();
            var model = await _projectRegistrationRepository.GetAsync(input.Id);
            var noticeDucumentModel = await _noticeRepository.FirstOrDefaultAsync(r => r.ProjectRegistrationId == model.ProjectId);
            if (noticeDucumentModel == null)
            {

                output.HasPublish = false;
            }
            else
            {
                output = model.MapTo<WorkRegistrationForViewPublishOutput>();
                output.HasPublish = true;
            }
            return output;
        }

        public async Task<WorkRegistrationForViewRelyOutput> GetRely(EntityDto<Guid> input)
        {
            var output = new WorkRegistrationForViewRelyOutput();
            var model = await _projectRegistrationRepository.GetAsync(input.Id);
            var noticeDucumentModel = await _noticeRepository.FirstOrDefaultAsync(r => r.ProjectRegistrationId == model.ProjectId);
            if (noticeDucumentModel == null)
            {

                output.HasRely = false;
            }
            else
            {
                output = model.MapTo<WorkRegistrationForViewRelyOutput>();
                output.HasRely = true;
            }
            return output;
        }


        public async Task<List<WorkRegistrationList>> GetWorkRegPage(GetWorkRegistrationInput input)
        {
            var userId = base.AbpSession.UserId;

            var list = new List<WorkRegistrationList>();
            var listTemp = GetWorkRegistrations(input, PersonOnChargeTypeEnum.普通);
            var types = listTemp.GroupBy(x => x.StepName).ToList();
            foreach (var item in types)
            {
                if (list.Count(x => x.StepName == item.Key) == 0)
                {
                    list.Add(new WorkRegistrationList
                    {
                        PersonOnChargeType = PersonOnChargeTypeEnum.普通,
                        StepName = item.Key,
                        Nodes = listTemp.Where(x => x.StepName == item.Key).ToList()
                    });
                }
            }

            var pmList = GetWorkRegistrations(input, PersonOnChargeTypeEnum.负责人);
            var waitList = GetWorkRegistrations(input, PersonOnChargeTypeEnum.待汇总);
            if (waitList.Count() > 0)
                pmList.Add(new WorkRegistrationList
                {
                    PersonOnChargeType = PersonOnChargeTypeEnum.待汇总,
                    StepName = "待汇总",
                    Nodes = waitList
                });

            if (pmList.Count() > 0)
                list.Add(new WorkRegistrationList
                {
                    PersonOnChargeType = PersonOnChargeTypeEnum.已汇总,
                    StepName = "初审汇总",
                    Nodes = pmList
                });

            return list;
        }

        public List<WorkRegistrationList> GetWorkRegistrations(GetWorkRegistrationInput input, PersonOnChargeTypeEnum type)
        {
            var query = (from reg in _projectRegistrationRepository.GetAll()
                         join user in _userRepository.GetAll() on reg.SendUserId equals user.Id into workreg
                         from user in workreg.DefaultIfEmpty()
                         where reg.ProjectId == input.ProjectId
                         && reg.Type == input.Type && reg.PersonOnChargeType == type
                         orderby reg.CreationTime descending
                         select new WorkRegistrationList
                         {
                             Id = reg.Id,
                             CreationTime = reg.CreationTime,
                             ProjectId = reg.ProjectId,
                             Title = reg.Title,
                             Code = reg.Code,
                             SendUserId = reg.SendUserId,
                             UserName = user.Name,
                             UserId = reg.SendUserId,
                             TaskId = reg.TaskId,
                             StepId = reg.StepId,
                             PersonOnChargeType = reg.PersonOnChargeType,
                             PersonOnChargeTypeName = reg.PersonOnChargeType.ToString(),
                             StepName = reg.PersonOnChargeType == PersonOnChargeTypeEnum.待汇总 ? user.Name : reg.StepName
                         });
            var list = query.ToList();
            for (int i = 0; i < list.Count(); i++)
            {
                var item = list[i];
                if (item.PersonOnChargeType == PersonOnChargeTypeEnum.负责人)
                    item.Nodes = (from reg in _projectRegistrationRepository.GetAll()
                                  join user in _userRepository.GetAll() on reg.SendUserId equals user.Id into workreg
                                  from user in workreg.DefaultIfEmpty()
                                  where reg.ProjectId == input.ProjectId
                                  && reg.RegistrationId == item.Id
                                  orderby reg.CreationTime descending
                                  select new WorkRegistrationList
                                  {
                                      Id = reg.Id,
                                      CreationTime = reg.CreationTime,
                                      ProjectId = reg.ProjectId,
                                      Title = reg.Title,
                                      Code = reg.Code,
                                      SendUserId = reg.SendUserId,
                                      UserName = user.Name,
                                      UserId = reg.SendUserId,
                                      TaskId = reg.TaskId,
                                      StepId = reg.StepId,
                                      PersonOnChargeType = reg.PersonOnChargeType,
                                      PersonOnChargeTypeName = reg.PersonOnChargeType.ToString(),
                                      StepName = user.Name
                                  }).ToList();
            }
            return list;
        }


        public async Task<PagedResultDto<WorkRegistrationList>> GetWorkRegistrationList(GetWorkRegistrationListInput input)
        {
            var userId = base.AbpSession.UserId;

            var query = (from reg in _projectRegistrationRepository.GetAll()
                         join user in _userRepository.GetAll() on reg.SendUserId equals user.Id into workreg
                         from user in workreg.DefaultIfEmpty()
                         where reg.ProjectId == input.ProjectId
                         && reg.Type == 1 && !reg.IsSummary
                         orderby reg.CreationTime descending
                         select new WorkRegistrationList
                         {
                             Id = reg.Id,
                             CreationTime = reg.CreationTime,
                             ProjectId = reg.ProjectId,
                             Title = reg.Title,
                             Content = reg.Content,
                             Code = reg.Code,
                             SendUserId = reg.SendUserId,
                             UserName = user.Name,
                             UserId = reg.SendUserId,
                             TaskId = reg.TaskId,
                             StepId = reg.StepId,
                             StepName = reg.StepName
                         });
            if (query == null)
                return new PagedResultDto<WorkRegistrationList>(0, new List<WorkRegistrationList>());

            var total = await query.CountAsync();
            var list = await query.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();
            foreach (var item in list)
            {
                item.Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = item.Id.ToString(), BusinessType = (int)AbpFileBusinessType.工作联系附件 });
            }
            return new PagedResultDto<WorkRegistrationList>(total, list);
        }


        public async Task<PagedResultDto<WorkRegistrationList>> GetWorkRegistrationOldList(GetWorkRegistrationListOldInput input)
        {
            var userId = base.AbpSession.UserId;

            var query = (from reg in _projectRegistrationRepository.GetAll()
                         join user in _userRepository.GetAll() on reg.SendUserId equals user.Id into workreg
                         from user in workreg.DefaultIfEmpty()
                         where reg.RegistrationId == input.RegistrationId
                         && reg.Type == 1 && reg.IsSummary
                         orderby reg.CreationTime descending
                         select new WorkRegistrationList
                         {
                             Id = reg.Id,
                             CreationTime = reg.CreationTime,
                             ProjectId = reg.ProjectId,
                             Title = reg.Title,
                             Code = reg.Code,
                             SendUserId = reg.SendUserId,
                             UserName = user.Name,
                             UserId = reg.SendUserId,
                             TaskId = reg.TaskId,
                             StepId = reg.StepId,
                             StepName = reg.StepName
                         });
            if (query == null)
                return new PagedResultDto<WorkRegistrationList>(0, new List<WorkRegistrationList>());

            var total = await query.CountAsync();
            var list = await query.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();

            return new PagedResultDto<WorkRegistrationList>(total, list);
        }
        public async Task<PagedResultDto<PmProjectOutput>> GetWorkPmProject(GetPmProjectInput input)
        {
            var userId = base.AbpSession.UserId;
            var query = (from p in _projectBaseRepository.GetAll()
                         let count = _projectRegistrationRepository.GetAll().Count(x => x.ProjectId == p.Id && x.PersonOnCharge == userId && !x.IsSummary)
                         where count > 0
                         select new PmProjectOutput
                         {
                             Id = p.Id,
                             Name = p.ProjectName,
                             Count = count
                         });

            var total = await query.CountAsync();
            var list = await query.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();

            return new PagedResultDto<PmProjectOutput>(total, list);
        }

        public async Task<PagedResultDto<PmOldProjectOutput>> GetWorkOldPmProject(GetPmProjectInput input)
        {
            var userId = base.AbpSession.UserId;
            var query = (from p in _projectRegistrationRepository.GetAll()
                         join pBase in _projectBaseRepository.GetAll() on p.ProjectId equals pBase.Id
                         where p.PersonOnChargeType == PersonOnChargeTypeEnum.负责人 && p.SendUserId == userId
                         select new PmOldProjectOutput
                         {
                             Id = p.Id,
                             ProjectName = pBase.ProjectName,
                             Title = p.Title,
                             Code = p.Code,
                             CreateTime = p.CreationTime
                         });

            var total = await query.CountAsync();
            var list = await query.OrderByDescending(x => x.CreateTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();

            return new PagedResultDto<PmOldProjectOutput>(total, list);
        }
    }
}