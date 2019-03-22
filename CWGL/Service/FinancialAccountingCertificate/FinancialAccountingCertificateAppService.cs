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
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;
using Abp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ZCYX.FRMSCore.Configuration;
using WebClientServer;
using Abp.Authorization;
using Abp.Application.Services;
using Abp.Domain.Uow;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using Abp.WorkFlowDictionary;

namespace CWGL
{
    public class FinancialAccountingCertificateAppService : FRMSCoreAppServiceBase, IFinancialAccountingCertificateAppService
    {
        private readonly IRepository<FinancialAccountingCertificate, Guid> _repository;
        private readonly IRepository<FACertificateDetail, Guid> _detailRepository;
        private readonly IRepository<AccountantCourse, Guid> _aCRepository;
        private IHostingEnvironment hostingEnv;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryRepository;

        public FinancialAccountingCertificateAppService(IRepository<FinancialAccountingCertificate, Guid> repository
        , IRepository<FACertificateDetail, Guid> detailRepository, IRepository<AccountantCourse, Guid> aCRepository, IHostingEnvironment env
            , WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager, ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IRepository<AbpDictionary, Guid> abpDictionaryRepository, IWorkFlowTaskRepository workFlowTaskRepository
        )
        {
            this._repository = repository;
            _detailRepository = detailRepository;
            _aCRepository = aCRepository;
            hostingEnv = env;
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _abpDictionaryRepository = abpDictionaryRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<FinancialAccountingCertificateListOutputDto>> GetList(GetFinancialAccountingCertificateListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && x.IsAccounting)                        
                        select new FinancialAccountingCertificateListOutputDto()
                        {
                            Id = a.Id,
                            Code = a.Code,
                            BusinessType = a.BusinessType,
                            BusinessId = a.BusinessId,
                            UserId = a.UserId,
                            OrgId = a.OrgId,
                            KeepUserId = a.KeepUserId,
                            AuditUserId = a.AuditUserId,
                            CashierUserId = a.CashierUserId,
                            MakeUserId = a.MakeUserId,
                            Summary = a.Summary,
                            TotalDebitAmount = a.TotalDebitAmount,
                            TotalCreditAmount = a.TotalCreditAmount,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                item.BusinessTypeName = ((FACertificateBusinessType)item.BusinessType).ToString();
                var task = _workFlowTaskRepository.GetAll().FirstOrDefault(x => x.Sort == 1 && x.InstanceID == item.BusinessId.ToString());
                if (task != null)
                {
                    item.FlowId = task.FlowID;
                    var flow = _workFlowCacheManager.GetWorkFlowModelFromCache(task.FlowID);
                    var step = flow.Steps.FirstOrDefault(x=>x.ID==task.StepID);
                    if (step != null && step.WorkFlowModelId.HasValue)
                        item.WorkFlowModelId = step.WorkFlowModelId.Value;
                }
            }

            return new PagedResultDto<FinancialAccountingCertificateListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<FinancialAccountingCertificateOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var ret = model.MapTo<FinancialAccountingCertificateOutputDto>();
        
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                if (ret.Type.HasValue)
                    ret.TypeName = _abpDictionaryRepository.GetAll().FirstOrDefault(x => x.Id == ret.Type.Value)?.Title;
                ret.BusinessType_Name = ((FACertificateBusinessType)ret.BusinessType).ToString();
                var details = from a in _detailRepository.GetAll()
                              join b in _aCRepository.GetAll() on a.AccountingCourseId equals b.Id
                              where !a.IsDeleted && a.MainId == model.Id
                              select new FACertificateDetailListOutputDto()
                              {
                                  AccountingCourseId = b.Id,
                                  AccountingCourseId_Name = b.Name,
                                  Amount = a.Amount,
                                  BusinessType = a.BusinessType,
                                  CreationTime = a.CreationTime,
                                  Id = a.Id,
                                  MainId = a.MainId,

                              };
                foreach (var item in details)
                {
                    item.BusinessType_Name = ((FACertificateDetailBusinessType)item.BusinessType).ToString();
                    ret.Details.Add(item);
                }
            }
            var cwUsers = _workFlowOrganizationUnitsManager.GetAbpUsersByRoleCode("CW");
            var clUsers = _workFlowOrganizationUnitsManager.GetAbpUsersByRoleCode("CL");
            if (cwUsers.Count() > 0)
                ret.CWGL_Name = cwUsers.FirstOrDefault().Name;
            if (clUsers.Count() > 0)
                ret.CWCL_Name = clUsers.FirstOrDefault().Name;

            return ret;
        }


        /// <summary>
        /// 根据业务id获取实体
        /// </summary>
        /// <param name="input"></param>
        /// 只传id， content为空 表示： 只获取结果； 没有历史数据 则返回new；
        /// 传id+content不为空， 表示识别；若content与历史的summary不同则再次识别；相同则返回历史数据
        /// <returns></returns>
        public async Task<FinancialAccountingCertificateOutputDto> GetByBusinessId(GetByBusinessIdInput input)
        {

            var ret = new FinancialAccountingCertificateOutputDto();

            var model = await _repository.FirstOrDefaultAsync(x => x.BusinessId == input.BusinessId && x.BusinessType == (int)input.BusinessType);
            if (model == null)
            {
                if (input.Content.IsNullOrEmpty())
                {
                    return new FinancialAccountingCertificateOutputDto();
                }
                var clUrl = _appConfiguration["CLService:clUrl"];
                var requestUrl = $"{clUrl}/discern ";
                var parameter = new { words = input.Content, tenantid = AbpSession.TenantId };
                var retResult = HttpClientHelper.PostResponse<CLResultInfo>(requestUrl, parameter);
                Abp.Logging.LogHelper.Logger.Info($"访问财来接口：{requestUrl},参数：{Newtonsoft.Json.JsonConvert.SerializeObject(parameter)},返回结果:{Newtonsoft.Json.JsonConvert.SerializeObject(retResult)}");
                if (retResult == null)
                    throw new UserFriendlyException((int)ErrorCode.HttpPortErr, "文本无法识别，请编辑后再试");
                var createParameter = new CreateFinancialAccountingCertificateInput()
                {
                    BusinessId = input.BusinessId,
                    BusinessType = (int)input.BusinessType,
                    Code = "",
                    ResultId = retResult.Id,
                    Summary = input.Content,

                };
                foreach (var item in retResult.Content)
                {
                    createParameter.Details.Add(new CreateFACertificateDetailInput() { AccountingCourseId = item.ASid, Amount = item.Money, BusinessType = item.FACType });
                }
                ret = Create(createParameter);
            }
            else
            {
                if (input.Content.IsNullOrEmpty())
                {
                    ret = model.MapTo<FinancialAccountingCertificateOutputDto>();
                    ret.BusinessType_Name = ((FACertificateBusinessType)ret.BusinessType).ToString();
                    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                    {
                        var details = from a in _detailRepository.GetAll()
                                      join b in _aCRepository.GetAll() on a.AccountingCourseId equals b.Id
                                      where !a.IsDeleted && a.MainId == model.Id
                                      select new FACertificateDetailListOutputDto()
                                      {
                                          AccountingCourseId = b.Id,
                                          AccountingCourseId_Name = b.Name,
                                          Amount = a.Amount,
                                          BusinessType = a.BusinessType,
                                          CreationTime = a.CreationTime,
                                          Id = a.Id,
                                          MainId = a.MainId,
                                          Summary = a.Summary
                                      };

                        foreach (var item in details)
                        {
                            item.BusinessType_Name = ((FACertificateDetailBusinessType)item.BusinessType).ToString();
                            ret.Details.Add(item);
                        }
                    }

                }
                else
                {
                    if (input.Content != model.Summary)
                    {
                        var clUrl = _appConfiguration["CLService:clUrl"];
                        var requestUrl = $"{clUrl}/discern ";
                        var parameter = new { words = input.Content, tenantid = AbpSession.TenantId };
                        var retResult = HttpClientHelper.PostResponse<CLResultInfo>(requestUrl, parameter);
                        Abp.Logging.LogHelper.Logger.Info($"访问财来接口：{requestUrl},参数：{Newtonsoft.Json.JsonConvert.SerializeObject(parameter)},返回结果:{Newtonsoft.Json.JsonConvert.SerializeObject(retResult)}");
                        if (retResult == null)
                            throw new UserFriendlyException((int)ErrorCode.HttpPortErr, "文本无法识别，请编辑后再试");
                        model.Summary = input.Content;
                        model.ResultId = retResult.Id;

                        ret = model.MapTo<FinancialAccountingCertificateOutputDto>();
                        ret.BusinessType_Name = ((FACertificateBusinessType)ret.BusinessType).ToString();
                        ret.ResultId = retResult.Id;
                        _detailRepository.Delete(r => r.MainId == model.Id);
                        using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                        {
                            foreach (var item in retResult.Content)
                            {
                                var entity = new FACertificateDetail()
                                {
                                    Id = Guid.NewGuid(),
                                    AccountingCourseId = item.ASid,
                                    Amount = item.Money,
                                    BusinessType = item.FACType,
                                    MainId = model.Id,
                                };
                                _detailRepository.Insert(entity);

                                var entityOutput = new FACertificateDetailListOutputDto();
                                entityOutput.AccountingCourseId = item.ASid;
                                entityOutput.AccountingCourseId_Name = (_aCRepository.Get(item.ASid)).Name;
                                entityOutput.Amount = item.Money;
                                entityOutput.BusinessType = item.FACType;
                                entityOutput.BusinessType_Name = ((FACertificateDetailBusinessType)item.FACType).ToString();
                                entityOutput.Id = entity.Id;
                                entityOutput.MainId = model.Id;
                                ret.Details.Add(entityOutput);

                            }
                        }




                    }
                    else
                    {
                        ret = model.MapTo<FinancialAccountingCertificateOutputDto>();
                        ret.BusinessType_Name = ((FACertificateBusinessType)ret.BusinessType).ToString();
                        using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                        {
                            var details = from a in _detailRepository.GetAll()
                                          join b in _aCRepository.GetAll() on a.AccountingCourseId equals b.Id
                                          where !a.IsDeleted && a.MainId == model.Id
                                          select new FACertificateDetailListOutputDto()
                                          {
                                              AccountingCourseId = b.Id,
                                              AccountingCourseId_Name = b.Name,
                                              Amount = a.Amount,
                                              BusinessType = a.BusinessType,
                                              CreationTime = a.CreationTime,
                                              Id = a.Id,
                                              MainId = a.MainId
                                          };

                            foreach (var item in details)
                            {
                                item.BusinessType_Name = ((FACertificateDetailBusinessType)item.BusinessType).ToString();
                                ret.Details.Add(item);
                            }
                        }

                    }

                }

            }
            var cwUsers = _workFlowOrganizationUnitsManager.GetAbpUsersByRoleCode("CW");
            var clUsers = _workFlowOrganizationUnitsManager.GetAbpUsersByRoleCode("CL");
            if (cwUsers.Count() > 0)
                ret.CWGL_Name = cwUsers.FirstOrDefault().Name;
            if (clUsers.Count() > 0)
                ret.CWCL_Name = clUsers.FirstOrDefault().Name;
            if (ret.Type.HasValue)
                ret.TypeName = _abpDictionaryRepository.GetAll().FirstOrDefault(x => x.Id == ret.Type.Value)?.Title;
            return ret;
        }




        [AbpAuthorize]
        private FinancialAccountingCertificateOutputDto Create(CreateFinancialAccountingCertificateInput input)
        {
            var ret = new FinancialAccountingCertificateOutputDto();
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = service.GetUserPostInfoV2(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
            var newmodel = new FinancialAccountingCertificate()
            {
                Code = input.Code,
                BusinessType = input.BusinessType,
                BusinessId = input.BusinessId,
                //UserId = input.UserId,
                UserId = AbpSession.UserId.Value,
                OrgId = userOrgModel.OrgId,
                KeepUserId = input.KeepUserId,
                AuditUserId = input.AuditUserId,
                CashierUserId = input.CashierUserId,
                MakeUserId = input.MakeUserId,
                Summary = input.Summary,
                Type = input.Type,
                Name = input.Name,
                Region = input.Region,
                TotalDebitAmount = input.TotalDebitAmount,
                TotalCreditAmount = input.TotalCreditAmount,
                Id = Guid.NewGuid(),
                ResultId = input.ResultId,

            };
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                foreach (var item in input.Details)
                {
                    var entity = new FACertificateDetail()
                    {
                        Id = Guid.NewGuid(),
                        AccountingCourseId = item.AccountingCourseId,
                        Amount = item.Amount,
                        BusinessType = item.BusinessType,
                        MainId = newmodel.Id,
                    };

                    _detailRepository.Insert(entity);


                    var entityOutput = new FACertificateDetailListOutputDto();
                    entityOutput.AccountingCourseId = item.AccountingCourseId;
                    entityOutput.AccountingCourseId_Name = (_aCRepository.Get(item.AccountingCourseId)).Name;
                    entityOutput.Amount = item.Amount;
                    entityOutput.BusinessType = item.BusinessType;
                    entityOutput.BusinessType_Name = ((FACertificateDetailBusinessType)item.BusinessType).ToString();
                    entityOutput.Id = entity.Id;
                    entityOutput.MainId = newmodel.Id;
                    //if (item.BusinessType == (int)FACertificateDetailBusinessType.借)
                    //    newmodel.TotalDebitAmount = newmodel.TotalDebitAmount + item.Amount;
                    //else
                    //    newmodel.TotalCreditAmount = newmodel.TotalCreditAmount + item.Amount;
                    ret.Details.Add(entityOutput);
                }
            }

            if (input.IsResultChangeByUser)
            {
                var clUrl = _appConfiguration["CLService:clUrl"];
                var requestUrl = $"{clUrl}/modify ";
                var param = new CLResultInfo { Id = input.ResultId, Content = input.Details.Select(r => new CLResultDetailInfo { ASid = r.AccountingCourseId, Money = r.Amount, FACType = r.BusinessType }).ToList(), };
                Task.Run(() =>
                {
                    var result = HttpClientHelper.PostResponse(requestUrl, param);
                    Abp.Logging.LogHelper.Logger.Info($"访问财来接口：{requestUrl},参数:{Newtonsoft.Json.JsonConvert.SerializeObject(param)},返回结果:{result}");

                });
            }
            _repository.Insert(newmodel);
            ret.ResultId = input.ResultId;
            ret.BusinessId = input.BusinessId;
            ret.BusinessType = input.BusinessType;
            ret.BusinessType_Name = ((FACertificateBusinessType)ret.BusinessType).ToString();
            ret.AuditUserId = input.AuditUserId;
            ret.CashierUserId = input.CashierUserId;
            ret.Code = input.Code;
            ret.CreationTime = DateTime.Now;
            ret.Id = newmodel.Id;
            ret.KeepUserId = input.KeepUserId;
            ret.MakeUserId = input.MakeUserId;
            ret.OrgId = newmodel.OrgId;
            ret.Summary = newmodel.Summary;
            ret.Type = newmodel.Type;
            ret.Name = newmodel.Name;
            ret.Region = newmodel.Region;

            return ret;

        }


        [AbpAuthorize]
        private FinancialAccountingCertificateOutputDto CreateWithOutNLP(CreateFinancialAccountingCertificateInput input)
        {
            var ret = new FinancialAccountingCertificateOutputDto();
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = service.GetUserPostInfoV2(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });

            var newmodel = new FinancialAccountingCertificate()
            {
                Code = input.Code == null ? "" : input.Code,
                //BusinessType = input.BusinessType,
                BusinessType = 1,
                BusinessId = input.BusinessId,
                //UserId = input.UserId,
                UserId = AbpSession.UserId.Value,
                OrgId = userOrgModel.OrgId,
                KeepUserId = input.KeepUserId,
                AuditUserId = input.AuditUserId,
                CashierUserId = input.CashierUserId,
                MakeUserId = input.MakeUserId,
                Summary = input.Summary,
                Type = input.Type,
                Name = input.Name,
                Region = input.Region,
                TotalDebitAmount = input.TotalDebitAmount,
                TotalCreditAmount = input.TotalCreditAmount,
                Id = Guid.NewGuid(),
                ResultId = input.ResultId,

            };
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                foreach (var item in input.Details)
                {
                    var entity = new FACertificateDetail()
                    {
                        Id = Guid.NewGuid(),
                        AccountingCourseId = item.AccountingCourseId,
                        Amount = item.Amount,
                        Summary = item.Summary,
                        BusinessType = item.BusinessType,
                        MainId = newmodel.Id,
                    };

                    _detailRepository.Insert(entity);


                    var entityOutput = new FACertificateDetailListOutputDto();
                    entityOutput.AccountingCourseId = item.AccountingCourseId;
                    entityOutput.AccountingCourseId_Name = (_aCRepository.Get(item.AccountingCourseId)).Name;
                    entityOutput.Amount = item.Amount;
                    entityOutput.Summary = item.Summary;
                    entityOutput.BusinessType = item.BusinessType;
                    entityOutput.BusinessType_Name = ((FACertificateDetailBusinessType)item.BusinessType).ToString();
                    entityOutput.Id = entity.Id;
                    entityOutput.MainId = newmodel.Id;
                    //if (item.BusinessType == (int)FACertificateDetailBusinessType.借)
                    //    newmodel.TotalDebitAmount = newmodel.TotalDebitAmount + item.Amount;
                    //else
                    //    newmodel.TotalCreditAmount = newmodel.TotalCreditAmount + item.Amount;
                    ret.Details.Add(entityOutput);
                }
            }

            _repository.Insert(newmodel);
            ret.ResultId = input.ResultId;
            ret.BusinessId = input.BusinessId;
            ret.BusinessType = input.BusinessType;
            ret.BusinessType_Name = ((FACertificateBusinessType)ret.BusinessType).ToString();
            ret.AuditUserId = input.AuditUserId;
            ret.CashierUserId = input.CashierUserId;
            ret.Code = input.Code;
            ret.CreationTime = DateTime.Now;
            ret.Id = newmodel.Id;
            ret.KeepUserId = input.KeepUserId;
            ret.MakeUserId = input.MakeUserId;
            ret.OrgId = newmodel.OrgId;
            ret.Summary = newmodel.Summary;
            return ret;

        }





        /// <summary>
        /// 修改一个FinancialAccountingCertificate
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        private void Update(UpdateFinancialAccountingCertificateInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = _repository.FirstOrDefault(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                //var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
                //var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });

                dbmodel.Code = input.Code;
                dbmodel.BusinessType = input.BusinessType;
                dbmodel.BusinessId = input.BusinessId;
                //dbmodel.UserId = input.UserId;
                //dbmodel.OrgId = userOrgModel.OrgId;
                dbmodel.KeepUserId = input.KeepUserId;
                dbmodel.AuditUserId = input.AuditUserId;
                dbmodel.CashierUserId = input.CashierUserId;
                dbmodel.MakeUserId = input.MakeUserId;
                dbmodel.Summary = input.Summary;
                dbmodel.Type = input.Type;
                dbmodel.Name = input.Name;
                dbmodel.Region = input.Region;
                dbmodel.TotalDebitAmount = input.TotalDebitAmount;
                dbmodel.TotalCreditAmount = input.TotalCreditAmount;
                dbmodel.ResultId = input.ResultId;
                if (input.IsResultChangeByUser)
                {
                    _detailRepository.Delete(r => r.MainId == dbmodel.Id);
                    foreach (var item in input.Details)
                    {
                        var entity = new FACertificateDetail()
                        {
                            Id = Guid.NewGuid(),
                            AccountingCourseId = item.AccountingCourseId,
                            Amount = item.Amount,
                            BusinessType = item.BusinessType,
                            MainId = dbmodel.Id,
                        };

                        _detailRepository.Insert(entity);
                    }

                    var clUrl = _appConfiguration["CLService:clUrl"];
                    var requestUrl = $"{clUrl}/modify ";
                    var param = new { id = input.ResultId, content = input.Details.Select(r => new { ASid = r.AccountingCourseId, Money = r.Amount, FACType = r.BusinessType }).ToList() };
                    Task.Run(() =>
                    {
                        var result = HttpClientHelper.PostResponse(requestUrl, param);
                        Abp.Logging.LogHelper.Logger.Info($"访问财来接口：{requestUrl},返回结果:{result}");
                    });
                }
                _repository.Update(dbmodel);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }

        }


        /// <summary>
        /// 修改一个FinancialAccountingCertificate
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        private void UpdateWithOutNLP(UpdateFinancialAccountingCertificateInput input, bool isUpdateForChange, Guid? flowId)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = _repository.FirstOrDefault(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                var old_Model = new FinancialAccountingCertificateChangeModelDto();
                dbmodel.Code = input.Code;
                dbmodel.BusinessType = input.BusinessType;
                dbmodel.BusinessId = input.BusinessId;
                dbmodel.KeepUserId = input.KeepUserId;
                dbmodel.AuditUserId = input.AuditUserId;
                dbmodel.CashierUserId = input.CashierUserId;
                dbmodel.MakeUserId = input.MakeUserId;
                dbmodel.Summary = input.Summary;
                dbmodel.Type = input.Type;
                dbmodel.Name = input.Name;
                dbmodel.Region = input.Region;
                dbmodel.TotalDebitAmount = input.TotalDebitAmount;
                dbmodel.TotalCreditAmount = input.TotalCreditAmount;
                dbmodel.ResultId = input.ResultId;

                var db_detailsQuery = from a in _detailRepository.GetAll()
                                      join b in _aCRepository.GetAll() on a.AccountingCourseId equals b.Id
                                      where a.MainId == input.Id
                                      select new FACertificateDetailChangeDto { Id = a.Id, AccountingCourseName = b.Name, Amount = a.Amount, BusinessType_Name = a.BusinessType == (int)FACertificateDetailBusinessType.借 ? "借" : "贷" };

                var delete_Ids = new List<Guid>();
                if (db_detailsQuery.Count() > 0)
                {
                    old_Model.Details = db_detailsQuery.ToList();
                    delete_Ids = old_Model.Details.Select(r => r.Id.Value).Except(input.Details.Where(r => r.Id.HasValue).Select(r => r.Id.Value)).ToList();
                }
                foreach (var item in delete_Ids)
                {
                    _detailRepository.Delete(r => r.Id == item);
                }


                foreach (var item in input.Details)
                {
                    if (item.Id.HasValue)
                    {
                        var entity_detail = _detailRepository.Get(item.Id.Value);
                        entity_detail.AccountingCourseId = item.AccountingCourseId;
                        entity_detail.Amount = item.Amount;
                        entity_detail.BusinessType = item.BusinessType;
                    }
                    else
                    {
                        var entity = new FACertificateDetail()
                        {
                            Id = Guid.NewGuid(),
                            AccountingCourseId = item.AccountingCourseId,
                            Amount = item.Amount,
                            BusinessType = item.BusinessType,
                            MainId = dbmodel.Id,
                            Summary = item.Summary
                        };

                        _detailRepository.Insert(entity);
                    }
                }

                _repository.Update(dbmodel);

                if (isUpdateForChange)
                {
                    var new_Model = GetChangeModel(input);
                    var logs = old_Model.GetColumnAllLogs(new_Model);
                    if (!flowId.HasValue) throw new UserFriendlyException((int)ErrorCode.CodeValErr, "流程编号为空");
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(flowId.Value);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.CodeValErr, "流程不存在");
                    var groupId = input.GroupId.HasValue ? input.GroupId.Value : Guid.NewGuid();
                    _projectAuditManager.InsertAsync(logs, input.BusinessId.ToString(), flowModel.TitleField.Table, groupId);
                }

            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }

        }

        [RemoteService(IsEnabled = false)]
        public void CreateOrUpdate(CreateOrUpdateFinancialAccountingCertificateInput input)
        {
            if (input.Id.HasValue)
            {
                var inputBase = input as UpdateFinancialAccountingCertificateInput;
                inputBase.Id = input.Id.Value;
                Update(inputBase);
            }
            else
                Create(input);
        }
        public async Task UpdateAccounting(NullableIdDto<Guid> input)
        {
            var models = _repository.GetAll().Where(x => x.BusinessId == input.Id.Value.ToString() && !x.IsAccounting).ToList();
            foreach (var model in models)
            {
                model.IsAccounting = true;
                await _repository.UpdateAsync(model);
            }
        }


        [RemoteService(IsEnabled = false)]
        public void CreateOrUpdateWithOutNLP(CreateOrUpdateFinancialAccountingCertificateInput input, bool isUpdateForChange = false, Guid? flowId = null)
        {
            if (input.Id.HasValue && input.Id.Value != Guid.Empty)
            {
                var inputBase = input as UpdateFinancialAccountingCertificateInput;
                inputBase.Id = input.Id.Value;
                UpdateWithOutNLP(inputBase, isUpdateForChange, flowId);
            }
            else
                CreateWithOutNLP(input);
        }

        // <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }



        private FinancialAccountingCertificateChangeModelDto GetChangeModel(UpdateFinancialAccountingCertificateInput model)
        {
            var ret = new FinancialAccountingCertificateChangeModelDto() { };
            foreach (var item in model.Details)
            {
                var entity = new FACertificateDetailChangeDto();
                entity.Id = item.Id;
                var acModel = _aCRepository.FirstOrDefault(r => r.Id == item.AccountingCourseId);
                if (acModel != null)
                    entity.AccountingCourseName = acModel.Name;
                entity.BusinessType_Name = item.BusinessType == (int)FACertificateDetailBusinessType.借 ? "借" : "贷";
                entity.Amount = item.Amount;
                ret.Details.Add(entity);
            }
            return ret;

        }


    }
}