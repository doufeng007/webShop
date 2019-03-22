using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.Runtime.Session;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Newtonsoft.Json.Linq;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.Application.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using Abp.Authorization;
using Abp.File;
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class ProjectResultAppService : FRMSCoreAppServiceBase, IProjectResultAppService
    {
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ProjectBudgetControl, Guid> _projectBudgetControlRepository;
        private readonly IProjectFileRepository _projectFileRepository;
        private readonly IRepository<ProjectAuditMember, Guid> _projectAuditMemberRepository;
        private readonly IRepository<ProjectAuditMemberResult, Guid> _projectAuditMemberResultRepository;
        private readonly IRepository<ProjectPersentFinish, Guid> _projectPersentFinishRepository;
        private readonly IRepository<ProjectPersentFinishAllot, Guid> _projectPersentFinishAllotRepository;
        private readonly IRepository<ProjectPersentFinishResult, Guid> _projectPersentFinishResultRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        IRepository<SingleProjectInfo, Guid> _singleProjectInfoRepository;

        public ProjectResultAppService(IProjectBaseRepository projectBaseRepository, IRepository<User, long> userRepository,
            IRepository<ProjectAuditMember, Guid> projectAuditMemberRepository, IRepository<ProjectAuditMemberResult, Guid> projectAuditMemberResultRepository
            , IRepository<ProjectBudgetControl, Guid> projectBudgetControlRepository,
            IProjectFileRepository projectFileRepository, IRepository<SingleProjectInfo, Guid> singleProjectInfoRepository
            , IRepository<ProjectPersentFinish, Guid> projectPersentFinishRepository, IRepository<ProjectPersentFinishAllot, Guid> projectPersentFinishAllotRepository, IRepository<ProjectPersentFinishResult, Guid> projectPersentFinishResultRepository
            , IAbpFileRelationAppService abpFileRelationAppService)
        {
            _userRepository = userRepository;
            _projectBaseRepository = projectBaseRepository;
            _projectBudgetControlRepository = projectBudgetControlRepository;
            _projectFileRepository = projectFileRepository;
            _projectAuditMemberRepository = projectAuditMemberRepository;
            _projectAuditMemberResultRepository = projectAuditMemberResultRepository;
            _projectPersentFinishResultRepository = projectPersentFinishResultRepository;
            _projectPersentFinishAllotRepository = projectPersentFinishAllotRepository;
            _projectPersentFinishRepository = projectPersentFinishRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _singleProjectInfoRepository = singleProjectInfoRepository;
        }

        [AbpAuthorize]
        public async Task<Guid> CreateOrUpdateAsync(CreateOrUpdateProjectResultInput input)
        {
            if (input.Id.HasValue)
            {
                await UpdateAsync(input);
                return input.Id.Value;
            }
            else
            {
                return await CreateAsync(input);
            }
        }

        public async Task<GetProjectAuditResultBaseOutput> GetMemberResultForEditAsync(GetProjectResultForEditInput input)
        {
            var ret = new GetProjectLeaderResultOutput();
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                        .IocManager.IocContainer.Resolve<IProjectAppService>();
            var projectBase = await service.GetProjectBudgetForEdit(new GetProjectForEditInput() { AppraisalTypeId = 8, Id = input.ProjectBaseId });
            ret.ProjectInfo = projectBase;
            ret.ProjectBaseId = input.ProjectBaseId;
            ret.AuditRoleId = input.AuditRoleId;
            ret.Result = await GetAuditMemberResult(input.ProjectBaseId, input.AuditRoleId, AbpSession.UserId.Value);
            return ret;
        }


        [AbpAuthorize]
        public async Task CreateOrUpdateFinishResultAsync(CreateUpdateProjectAuditInput input)
        {
            foreach (var item in input.Results)
            {
                if (input.Action == 1)//
                {

                    if (item.CjzFile == null)
                    {
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "CJZ文件不能为空");
                    }
                    if (item.Files == null || item.Files.Count == 0)
                    {
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "评审文件不能为空");
                    }
                }
                else if (input.Action == 2)//汇总时判断是否事项负责人，是负责人才需要上传汇总
                {
                    var allot = _projectPersentFinishAllotRepository.FirstOrDefault(item.AllotId);
                    if (allot!=null&&allot.IsMain) {
                        var member = _projectAuditMemberRepository.FirstOrDefault(ite => ite.Id == allot.AuditMembeId);
                        if (member != null && member.UserId == AbpSession.UserId.Value) {
                            if (item.CjzFile == null)
                            {
                                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "CJZ汇总文件不能为空");
                            }
                            if (item.Files == null || item.Files.Count == 0)
                            {
                                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "评审汇总文件不能为空");
                            }
                        }
                    }
                }
                else {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "【Action】参数错误");
                }
                if (item.Id.HasValue)
                {
                    var entity = await _projectPersentFinishResultRepository.FirstOrDefaultAsync(r => r.ProjectId == input.ProjectBaseId && r.AllotId == item.AllotId && r.ResultType == input.Action);
                    if (entity == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "事务评审结果数据异常");
                    entity.AuditAmount = item.AuditAmount;
                    entity.SurePersent = item.SurePersent;
                    var cjzFileNews = new List<AbpFileListInput>();
                    if (item.CjzFile != null)
                    {
                        cjzFileNews.Add(new AbpFileListInput() { Id = item.CjzFile.Id, Sort = item.CjzFile.Sort });
                    }

                    await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput() { BusinessId = entity.Id.ToString(), BusinessType = input.Action == 1 ? (int)AbpFileBusinessType.工程评审CJZ结果 : (int)AbpFileBusinessType.评审事项汇总CJZ结果, Files = cjzFileNews });
                    await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput() { BusinessId = entity.Id.ToString(), BusinessType = input.Action == 1 ? (int)AbpFileBusinessType.工程评审结果 : (int)AbpFileBusinessType.评审事项汇总结果, Files = item.Files });
                    entity.Remark = item.Remark;
                    await _projectPersentFinishResultRepository.UpdateAsync(entity);
                }
                else
                {
                    var entity = new ProjectPersentFinishResult()
                    {
                        Id = Guid.NewGuid(),
                        AllotId = item.AllotId,
                        AuditAmount = item.AuditAmount,
                        SurePersent=item.SurePersent,
                        //CJZFiles = Newtonsoft.Json.JsonConvert.SerializeObject(item.CjzFile),
                        //Files = Newtonsoft.Json.JsonConvert.SerializeObject(item.Files),
                        ProjectId = input.ProjectBaseId,
                        Remark = item.Remark,
                        ResultType = input.Action,
                        UserId = AbpSession.UserId.Value
                    };
                    if (item.CjzFile != null)
                        await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput() { BusinessId = entity.Id.ToString(), BusinessType = input.Action == 1 ? (int)AbpFileBusinessType.工程评审CJZ结果 : (int)AbpFileBusinessType.评审事项汇总CJZ结果, Files = new List<AbpFileListInput>() { item.CjzFile } });
                    await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput() { BusinessId = entity.Id.ToString(), BusinessType = input.Action == 1 ? (int)AbpFileBusinessType.工程评审结果 : (int)AbpFileBusinessType.评审事项汇总结果, Files = item.Files });
                    await _projectPersentFinishResultRepository.InsertAsync(entity);
                }

            }
        }

        /// <summary>
        /// 工程评审人员用的获取评审界面
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<GetProjectAuditResultOutput> GetAuditAsync(GetProjectResultForEditInput input)
        {
            var ret = new GetProjectAuditResultOutput();
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                           .IocManager.IocContainer.Resolve<IProjectAppService>();
            var projectBase = await service.GetSingleProject(new GetSingleProjectInput() {  Id=input.ProjectBaseId});
            ret.ProjectInfo = projectBase;
            ret.ProjectBaseId = input.ProjectBaseId;
            var finish_query = from a in _projectPersentFinishRepository.GetAll()
                               join b in _projectPersentFinishAllotRepository.GetAll() on a.Id equals b.FinishId
                               join c in _projectAuditMemberRepository.GetAll() on b.AuditMembeId equals c.Id
                               join d in _projectPersentFinishResultRepository.GetAll() on new { allotid = b.Id, userid = c.UserId } equals new { allotid = d.AllotId, userid = d.UserId } into g
                               from r in g.DefaultIfEmpty()
                               where c.UserAuditRole == input.AuditRoleId && c.UserId == AbpSession.UserId.Value && (r == null || (r.UserId == AbpSession.UserId.Value)) && a.ProjectId == input.ProjectBaseId
                               select new { A = a, AllotId = b.Id, ResultId = r == null ? Guid.Empty : r.Id, CjzFile = r == null ? "" : r.CJZFiles, Files = r == null ? "" : r.Files, Amount = r == null ? 0 : r.AuditAmount, Remark = r == null ? "" : r.Remark, SurePersent=r.SurePersent, ManagerSurePersent=r.ManagerSurePersent };
            foreach (var item in finish_query)
            {
                var entity = new ProjectFinishItem() { FinishId = item.A.Id, FinishName = item.A.Name, WorkDay = item.A.WorkDay, AllotId = item.AllotId };
                entity.Result = new ProjectAuditResultInfoOutput();
                if (item.ResultId != Guid.Empty)
                {
                    entity.Result.Id = item.ResultId;
                    entity.Result.CjzFile = await _abpFileRelationAppService.GetAsync(new GetAbpFilesInput() { BusinessId = entity.Result.Id.ToString(), BusinessType = (int)AbpFileBusinessType.工程评审CJZ结果 });
                    entity.Result.Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = entity.Result.Id.ToString(), BusinessType = (int)AbpFileBusinessType.工程评审结果 });
                    entity.Result.AuditAmount = item.Amount;
                    entity.Result.Remark = item.Remark;
                    entity.Result.SurePersent = item.SurePersent;
                    entity.Result.ManagerSurePersent = item.ManagerSurePersent;
                }
                ret.FinishItems.Add(entity);

            }

            //var resultType = (int)FinishResultTypeEnum.评审结果;
            //var auditResult_query = await _projectPersentFinishResultRepository.GetAll().FirstOrDefaultAsync(r => r.ProjectId == input.ProjectBaseId && r.UserId == AbpSession.UserId.Value && r.ResultType == resultType);
            //if (auditResult_query != null)
            //{
            //    ret.Result.Id = auditResult_query.Id;
            //    if (!auditResult_query.CJZFiles.IsNullOrWhiteSpace())
            //        ret.Result.CjzFile = Newtonsoft.Json.JsonConvert.DeserializeObject<FileUploadFiles>(auditResult_query.CJZFiles);
            //    if (!auditResult_query.Files.IsNullOrWhiteSpace())
            //        ret.Result.Files = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileUploadFiles>>(auditResult_query.Files);
            //    ret.AuditRoleId = input.AuditRoleId;
            //    ret.Result.AuditAmount = auditResult_query.AuditAmount;
            //    ret.Result.Remark = auditResult_query.Remark;
            //}
            return ret;
        }

        public async Task<GetProjectHuiZongResultOutput> GetAuditForHuiZong(GetProjectResultForEditInput input)
        {
            var ret = new GetProjectHuiZongResultOutput();
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                           .IocManager.IocContainer.Resolve<IProjectAppService>();
            var projectBase = await service.GetSingleProject(new GetSingleProjectInput() {  Id=input.ProjectBaseId});
            ret.ProjectInfo = projectBase;
            ret.ProjectBaseId = input.ProjectBaseId;
            ret.Finishs = await GetFinishResult(input.ProjectBaseId,input.AuditRoleId);
            return ret;

        }


        public async Task<GetProjectLeaderResultOutput> GetLeaderAuditAsync(GetProjectResultForEditInput input)
        {
            var ret = new GetProjectLeaderResultOutput();
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                        .IocManager.IocContainer.Resolve<IProjectAppService>();
            var projectBase = await service.GetSingleProject(new GetSingleProjectInput() {  Id=input.ProjectBaseId});
            ret.ProjectInfo = projectBase;
            ret.ProjectBaseId = input.ProjectBaseId;
            ret.Result = await GetAuditMemberResult(input.ProjectBaseId, (int)AuditRoleEnum.项目负责人);
            ret.Finishs = await GetFinishResult(input.ProjectBaseId, (int)AuditRoleEnum.项目负责人);

            return ret;
        }

        /// <summary>
        /// 获取财务初审界面
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetProjectAuditResultBaseOutput> GetCWFAuditAsync(GetProjectResultForEditInput input)
        {
            var ret = new GetProjectAuditResultBaseOutput();
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                        .IocManager.IocContainer.Resolve<IProjectAppService>();
            var projectBase = await service.GetSingleProject(new GetSingleProjectInput() { Id = input.ProjectBaseId });
            ret.ProjectInfo = projectBase;
            ret.ProjectBaseId = input.ProjectBaseId;
            ret.Result = await GetAuditMemberResult(input.ProjectBaseId, (int)AuditRoleEnum.财务初审);
            return ret;
        }

        /// <summary>
        /// 获取财务终审界面
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetProjectCWEResultOutput> GetCWEAuditAsync(GetProjectResultForEditInput input)
        {
            var ret = new GetProjectCWEResultOutput();
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                        .IocManager.IocContainer.Resolve<IProjectAppService>();
            var projectBase = await service.GetSingleProject(new GetSingleProjectInput() { Id = input.ProjectBaseId });
            ret.ProjectInfo = projectBase;
            ret.ProjectBaseId = input.ProjectBaseId;
            ret.Result = await GetAuditMemberResult(input.ProjectBaseId, (int)AuditRoleEnum.财务评审);
            ret.CWFResult = await GetAuditMemberResult(input.ProjectBaseId, (int)AuditRoleEnum.财务初审);
            return ret;
        }


        public async Task<GetProjectReviewResultOutput> GetReviewResultAsync(GetProjectResultForEditInput input)
        {
            var ret = new GetProjectReviewResultOutput();
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                     .IocManager.IocContainer.Resolve<IProjectAppService>();
            var projectBase = await service.GetSingleProject(new GetSingleProjectInput() { Id = input.ProjectBaseId });
            ret.ProjectInfo = projectBase;
            ret.ProjectBaseId = input.ProjectBaseId;
            ret.LeaderResult = await GetAuditMemberResult(input.ProjectBaseId, (int)AuditRoleEnum.项目负责人);
            ret.ReviewResult = await GetAuditMemberResult(input.ProjectBaseId, (int)AuditRoleEnum.复核人);
            ret.ReviewResult2 = await GetAuditMemberResult(input.ProjectBaseId, (int)AuditRoleEnum.复核人二);
            ret.ReviewResult3 = await GetAuditMemberResult(input.ProjectBaseId, (int)AuditRoleEnum.复核人三);
            ret.FinanceResult1 = await GetAuditMemberResult(input.ProjectBaseId, (int)AuditRoleEnum.财务初审);
            ret.FinanceResult2 = await GetAuditMemberResult(input.ProjectBaseId, (int)AuditRoleEnum.财务评审);
            ret.Finishs = await GetFinishResult(input.ProjectBaseId,input.AuditRoleId);

            ret.Result = await GetAuditMemberResult(input.ProjectBaseId, input.AuditRoleId);
            return ret;
        }

        public async Task<ProjectAuditResultInfoOutput> GetAuditMemberResult(Guid projectId, int roleId, long? userId = null)
        {
            var ret = new ProjectAuditResultInfoOutput();
            if (roleId == (int)AuditRoleEnum.工程评审)
            {
                return await GetProjectFirstAuditResult(projectId);
            }

            var auditResult_query = from a in _projectAuditMemberResultRepository.GetAll()
                                    join b in _projectAuditMemberRepository.GetAll() on a.Pid equals b.Id
                                    where b.UserAuditRole == roleId && b.ProjectBaseId == projectId && (!userId.HasValue || (userId.HasValue && b.UserId == userId.Value))
                                    select new { a, b };
            var auditResult = await auditResult_query.FirstOrDefaultAsync();
            if (auditResult == null)
            {
                return new ProjectAuditResultInfoOutput();
            }
            ret.Id = auditResult.a.Id;
            var fileBusinessType = 0;
            var fileBusinessType2 = 0;
            if (roleId == (int)AuditRoleEnum.项目负责人)
            {
                fileBusinessType = (int)AbpFileBusinessType.项目负责人汇总结果;
                fileBusinessType2 = (int)AbpFileBusinessType.项目负责人汇总CJZ结果;
            }
            else if (roleId == (int)AuditRoleEnum.财务评审)
            {
                fileBusinessType = (int)AbpFileBusinessType.财务评审;
            }
            else if (roleId == (int)AuditRoleEnum.复核人)
            {
                fileBusinessType = (int)AbpFileBusinessType.复核结果;
                fileBusinessType2 = (int)AbpFileBusinessType.复核CJZ结果;
            }
            else if (roleId == (int)AuditRoleEnum.财务初审)
            {
                fileBusinessType = (int)AbpFileBusinessType.财务初审;
            }
            else if (roleId == (int)AuditRoleEnum.复核人二)
            {
                fileBusinessType2 = (int)AbpFileBusinessType.二级复核CJZ结果;
                fileBusinessType = (int)AbpFileBusinessType.二级复核结果;
            }
            else if (roleId == (int)AuditRoleEnum.复核人三)
            {
                fileBusinessType2 = (int)AbpFileBusinessType.三级复核CJZ结果;
                fileBusinessType = (int)AbpFileBusinessType.三级复核结果;
            }
            else if (roleId == (int)AuditRoleEnum.工程评审)
            {

            }
            else if (roleId == (int)AuditRoleEnum.联系人一)
            {
                fileBusinessType = (int)AbpFileBusinessType.联系人初审结果汇总;
                fileBusinessType2 = (int)AbpFileBusinessType.联系人初审结果汇总CJZ;
            }
            ret.Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = ret.Id.ToString(), BusinessType = fileBusinessType });
            if (fileBusinessType2 > 0)
                ret.CjzFile = await _abpFileRelationAppService.GetAsync(new GetAbpFilesInput() { BusinessId = ret.Id.ToString(), BusinessType = fileBusinessType2 });
            ret.AuditAmount = auditResult.a.AuditAmount;
            ret.Remark = auditResult.a.Remark;
            ret.Id = auditResult.a.Id;
            ret.SurePersent=
            ret.UserId = auditResult.b.UserId;
            ret.UserName = (await _userRepository.GetAsync(ret.UserId)).Name;
            ret.AuditRoleId = auditResult.b.UserAuditRole;
            ret.AuditRoleName = ((AuditRoleEnum)ret.AuditRoleId).ToString();
            return ret;
        }

        public async Task<List<CreateOrUpdateFinishOutput>> GetFinishResult(Guid projectId,int auditRole, bool isForHuizongBySelf = false)
        {
            var ret = new List<CreateOrUpdateFinishOutput>();
            if (isForHuizongBySelf)
            {
                if (!AbpSession.UserId.HasValue)
                {
                    throw new UserFriendlyException((int)ErrorCode.SignCodeErr, "未登录");
                }
            }
            var finish_query = from a in _projectPersentFinishRepository.GetAll()
                               join b in _projectPersentFinishAllotRepository.GetAll() on a.Id equals b.FinishId
                               join e in _projectAuditMemberRepository.GetAll() on b.AuditMembeId equals e.Id
                               join cc in _projectPersentFinishResultRepository.GetAll() on b.Id equals cc.AllotId into g
                               from c in g.DefaultIfEmpty()
                               join d in _userRepository.GetAll() on e.UserId equals d.Id
                               where a.ProjectId == projectId && (!isForHuizongBySelf || (isForHuizongBySelf && b.IsMain && e.UserId == AbpSession.UserId.Value))
                               select new { FinishId = a.Id, FinishName = a.Name, WorkDay = a.WorkDay, AllotId = b.Id, IsMain = b.IsMain, UserId = d.Id, UserName = d.Name,
                                   Persent = a.Persent,
                                   ResultId = c == null ? Guid.Empty : c.Id, ResultType = c == null ? 0 : c.ResultType, CjzFile = c == null ? "" : c.CJZFiles,
                                   Files = c == null ? "" : c.Files, Remark = c == null ? "" : c.Remark, AuditaAmount = c == null ? 0 : c.AuditAmount,c.SurePersent };

            var datas = await finish_query.ToListAsync();
            var finishs = datas.Select(r => r.FinishId).Distinct();
            foreach (var item in finishs)
            {
                var finish_allots = datas.Where(r => r.FinishId == item);
                var finish_One = finish_allots.FirstOrDefault();
                var entity_finish = new CreateOrUpdateFinishOutput() { Id = finish_One.FinishId, Name = finish_One.FinishName, WorkDay = finish_One.WorkDay,Persent=finish_One.Persent };
                var finishs_allots_ids = finish_allots.Select(r => r.AllotId).Distinct();
                foreach (var allots_id in finishs_allots_ids)
                {
                    var finish_allots_result = datas.Where(r => r.AllotId == allots_id);
                    var finish_allots_One = finish_allots_result.FirstOrDefault();
                    var entity_allot = new CreateOrUpdateFinishAllotOutput()
                    {
                        Id = allots_id,
                        IsMain = finish_allots_One.IsMain,
                        UserId = finish_allots_One.UserId,
                        UserName = finish_allots_One.UserName,
                    };
                    var result_data = finish_allots_result.FirstOrDefault(r => r.ResultType == 1);
                    if (result_data != null)
                    {
                        entity_allot.Result = new ProjectAuditResultInfoOutput()
                        {
                            AuditAmount = result_data.AuditaAmount,
                            CjzFile = await _abpFileRelationAppService.GetAsync(new GetAbpFilesInput() { BusinessId = result_data.ResultId.ToString(), BusinessType = (int)AbpFileBusinessType.工程评审CJZ结果 }),
                            Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = result_data.ResultId.ToString(), BusinessType = (int)AbpFileBusinessType.工程评审结果 }),
                            Remark = result_data.Remark,
                            Id = result_data.ResultId,
                             SurePersent=result_data.SurePersent,
                        };


                        var result_data2 = finish_allots_result.FirstOrDefault(r => r.ResultType == 2);
                        if (result_data2 != null)
                        {
                            entity_finish.GatherResult = new ProjectAuditResultInfoOutput()
                            {
                                AuditAmount = result_data2.AuditaAmount,
                                CjzFile = await _abpFileRelationAppService.GetAsync(new GetAbpFilesInput() { BusinessId = result_data2.ResultId.ToString(), BusinessType = (int)AbpFileBusinessType.评审事项汇总CJZ结果 }),
                                Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = result_data2.ResultId.ToString(), BusinessType = (int)AbpFileBusinessType.评审事项汇总结果 }),
                                Remark = result_data2.Remark,
                                Id = result_data.ResultId,
                                SurePersent = result_data.SurePersent,
                            };
                        }
                    }


                    

                    entity_finish.FinishMembers.Add(entity_allot);
                }
                var mainAllot = entity_finish.FinishMembers.FirstOrDefault(r => r.IsMain);
                if (mainAllot == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据异常");
                entity_finish.MainAllotId = mainAllot.Id;
                switch ((AuditRoleEnum)auditRole) {
                    case AuditRoleEnum.工程评审:
                        if (entity_finish.FinishMembers.Exists(ite => ite.UserId == AbpSession.UserId.Value)) {
                            ret.Add(entity_finish);
                        }
                        break;
                    default:
                        ret.Add(entity_finish);
                        break;
                }
               
            }
            return ret;
        }


        /// <summary>
        /// 获取初审结果汇总界面--联系人汇总
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetProjectFirstAuditCollectResultOutput> GetProjectFirstAuditCollect(GetProjectResultForEditInput input)
        {
            var ret = new GetProjectFirstAuditCollectResultOutput();
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                        .IocManager.IocContainer.Resolve<IProjectAppService>();
            var projectBase = await service.GetSingleProject(new GetSingleProjectInput() { AppraisalTypeId = 8, Id = input.ProjectBaseId });
            ret.ProjectInfo = projectBase;
            ret.ProjectBaseId = input.ProjectBaseId;
            ret.Result = await GetAuditMemberResult(input.ProjectBaseId, (int)AuditRoleEnum.联系人一);
            ret.Finishs = await GetFinishResult(input.ProjectBaseId,input.AuditRoleId);
            return ret;
        }

        /// <summary>
        /// 获取初步评审结果  政务版  默认一个评审人员
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private async Task<ProjectAuditResultInfoOutput> GetProjectFirstAuditResult(Guid projectId)
        {
            var auditRoleId = (int)AuditRoleEnum.工程评审;
            var ret = new ProjectAuditResultInfoOutput();
            var query = from p in _projectBaseRepository.GetAll()
                        join member in _projectAuditMemberRepository.GetAll() on p.Id equals member.ProjectBaseId
                        join allot in _projectPersentFinishAllotRepository.GetAll() on member.Id equals allot.AuditMembeId
                        join allotResult in _projectPersentFinishResultRepository.GetAll() on allot.Id equals allotResult.AllotId
                        where p.Id == projectId && member.UserAuditRole == auditRoleId
                        select allotResult;
            var data = await query.FirstOrDefaultAsync();
            if (data != null)
            {
                ret.AuditAmount = data.AuditAmount;
                ret.AuditRoleId = (int)AuditRoleEnum.工程评审;
                ret.AuditRoleName = AuditRoleEnum.工程评审.ToString();
                ret.UserId = data.UserId;
                ret.UserName = (await _userRepository.GetAsync(data.UserId)).Name;
                ret.Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = data.Id.ToString(), BusinessType = (int)AbpFileBusinessType.工程评审结果 });
                ret.CjzFile = await _abpFileRelationAppService.GetAsync(new GetAbpFilesInput() { BusinessId = data.Id.ToString(), BusinessType = (int)AbpFileBusinessType.工程评审CJZ结果 });
            }
            return ret;

        }

        private async Task<Guid> CreateAsync(CreateOrUpdateProjectResultInput input)
        {
            if (input.AuditAmount.HasValue == false || input.AuditAmount.Value <= 0) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "审定金额不能为空或小于0.");
            }
           // var projectModel = _projectBaseRepository.GetAsync(input.ProjectBaseId);
            var memberModel = await _projectAuditMemberRepository.GetAll().FirstOrDefaultAsync(r => r.ProjectBaseId == input.ProjectBaseId && r.UserAuditRole == input.AuditRoleId && r.UserId == AbpSession.UserId.Value);
            if (memberModel == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "评审人员数据异常");
            var entity = new ProjectAuditMemberResult();
            entity.Id = Guid.NewGuid();
            entity.Pid = memberModel.Id;
            
            entity.AuditAmount = input.AuditAmount;
            var businessType1 = 0;
            var businessType2 = 0;
            if (input.AuditRoleId == (int)AuditRoleEnum.项目负责人)
            {
                businessType1 = (int)AbpFileBusinessType.项目负责人汇总CJZ结果;
                businessType2 = (int)AbpFileBusinessType.项目负责人汇总结果;
            }
            else if (input.AuditRoleId == (int)AuditRoleEnum.复核人)
            {
                businessType1 = (int)AbpFileBusinessType.复核CJZ结果;
                businessType2 = (int)AbpFileBusinessType.复核结果;
            }
            else if (input.AuditRoleId == (int)AuditRoleEnum.财务评审)
            {
                businessType2 = (int)AbpFileBusinessType.财务评审;
            }
            else if (input.AuditRoleId == (int)AuditRoleEnum.财务初审)
            {
                businessType2 = (int)AbpFileBusinessType.财务初审;
            }
            else if (input.AuditRoleId == (int)AuditRoleEnum.复核人二)
            {
                businessType1 = (int)AbpFileBusinessType.二级复核CJZ结果;
                businessType2 = (int)AbpFileBusinessType.二级复核结果;
            }
            else if (input.AuditRoleId == (int)AuditRoleEnum.复核人三)
            {
                businessType1 = (int)AbpFileBusinessType.三级复核CJZ结果;
                businessType2 = (int)AbpFileBusinessType.三级复核结果;
                var singemodele = await _singleProjectInfoRepository.GetAsync(input.ProjectBaseId);
                singemodele.AuditAmount = input.AuditAmount;

            }
            else if (input.AuditRoleId == (int)AuditRoleEnum.联系人一)
            {
                businessType1 = (int)AbpFileBusinessType.联系人初审结果汇总CJZ;
                businessType2 = (int)AbpFileBusinessType.联系人初审结果汇总;
            }
            if (input.CjzFile != null)
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = entity.Id.ToString(),
                    BusinessType = businessType1,
                    Files = new List<AbpFileListInput>() { new AbpFileListInput() { Id = input.CjzFile.Id, Sort = input.CjzFile.Sort } }
                });
            var fileList = new List<AbpFileListInput>();
            if (input.Files == null || input.Files.Count == 0) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请上传评审附件。");
            }
            foreach (var filemodel in input.Files)
            {
                var fileone = new AbpFileListInput() { Id = filemodel.Id, Sort = filemodel.Sort };
                fileList.Add(fileone);
            }

            await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
            {
                BusinessId = entity.Id.ToString(),
                BusinessType = businessType2,
                Files = fileList
            });

            //entity.Files = Newtonsoft.Json.JsonConvert.SerializeObject(input.Files);
            //entity.CJZFiles = Newtonsoft.Json.JsonConvert.SerializeObject(input.CjzFile);
            entity.Remark = input.Remark;
            await _projectAuditMemberResultRepository.InsertAsync(entity);
            return entity.Id;
        }

        private async Task UpdateAsync(CreateOrUpdateProjectResultInput input)
        {
            if (input.AuditAmount.HasValue == false || input.AuditAmount.Value <= 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "审定金额不能为空或小于0.");
            }
            var entity = await _projectAuditMemberResultRepository.GetAsync(input.Id.Value);
            entity.AuditAmount = input.AuditAmount;
            entity.Remark = input.Remark;
            var businessType1 = 0;
            var businessType2 = 0;
            if (input.AuditRoleId == (int)AuditRoleEnum.项目负责人)
            {
                businessType1 = (int)AbpFileBusinessType.项目负责人汇总CJZ结果;
                businessType2 = (int)AbpFileBusinessType.项目负责人汇总结果;
            }
            else if (input.AuditRoleId == (int)AuditRoleEnum.复核人)
            {
                businessType1 = (int)AbpFileBusinessType.复核CJZ结果;
                businessType2 = (int)AbpFileBusinessType.复核结果;
            }
            else if (input.AuditRoleId == (int)AuditRoleEnum.财务评审)
            {
                businessType2 = (int)AbpFileBusinessType.财务评审;
            }
            else if (input.AuditRoleId == (int)AuditRoleEnum.财务初审)
            {
                businessType2 = (int)AbpFileBusinessType.财务初审;
            }
            else if (input.AuditRoleId == (int)AuditRoleEnum.复核人二)
            {
                businessType1 = (int)AbpFileBusinessType.二级复核CJZ结果;
                businessType2 = (int)AbpFileBusinessType.二级复核结果;
            }
            else if (input.AuditRoleId == (int)AuditRoleEnum.复核人三)
            {
                businessType1 = (int)AbpFileBusinessType.三级复核CJZ结果;
                businessType2 = (int)AbpFileBusinessType.三级复核结果;
                var singemodele = await _singleProjectInfoRepository.GetAsync(input.ProjectBaseId);
                singemodele.AuditAmount = input.AuditAmount;
            }
            else if (input.AuditRoleId == (int)AuditRoleEnum.联系人一)
            {
                businessType1 = (int)AbpFileBusinessType.联系人初审结果汇总CJZ;
                businessType2 = (int)AbpFileBusinessType.联系人初审结果汇总;
            }

            var cjzFileNews = new List<AbpFileListInput>();
            if (input.CjzFile != null)
            {
                cjzFileNews.Add(new AbpFileListInput() { Id = input.CjzFile.Id, Sort = input.CjzFile.Sort });
            }
            await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
            {
                BusinessId = entity.Id.ToString(),
                BusinessType = businessType1,
                Files = cjzFileNews
            });
            var fileList = new List<AbpFileListInput>();
            foreach (var filemodel in input.Files)
            {
                var fileone = new AbpFileListInput() { Id = filemodel.Id, Sort = filemodel.Sort };
                fileList.Add(fileone);
            }

            await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
            {
                BusinessId = entity.Id.ToString(),
                BusinessType = businessType2,
                Files = fileList
            });


            await _projectAuditMemberResultRepository.UpdateAsync(entity);
        }


    }





























}





