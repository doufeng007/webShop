using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Extensions;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using Abp.WorkFlow;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.File;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.UI;
using ZCYX.FRMSCore.Users;
using Abp.Application.Services;
using ZCYX.FRMSCore.Authorization.Roles;
using ZCYX.FRMSCore.Model;
using System.Data;
using Abp;
using Abp.Excel;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Abp.Authorization.Users;

namespace Supply
{
    [AbpAuthorize]
    public class SupplyAppService : FRMSCoreAppServiceBase, ISupplyAppService
    {
        private readonly ISupplyBaseRepository _supplyRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserSupply, Guid> _userSupplyRepository;
        private readonly SupplyManager _supplyManager;
        private readonly ProjectNoticeManager _noticeManager;
        private IHostingEnvironment hostingEnv;
        private readonly IRepository<AbpFile, Guid> _abpFilerepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserMenuInit, Guid> _userMenuInitRepository;
        public SupplyAppService(ISupplyBaseRepository supplyRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService, IRepository<User, long> userRepository
            , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager, IRepository<UserSupply, Guid> userSupplyRepository
            , SupplyManager supplyManager, ProjectNoticeManager noticeManager, IHostingEnvironment env, IRepository<UserMenuInit, Guid> userMenuInitRepository
            , IRepository<AbpFile, Guid> abpFilerepository, IRepository<UserRole, long> userRoleRepository, IRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _supplyRepository = supplyRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _userSupplyRepository = userSupplyRepository;
            _supplyManager = supplyManager;
            _noticeManager = noticeManager;
            hostingEnv = env;
            _abpFilerepository = abpFilerepository;
            _userMenuInitRepository = userMenuInitRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
        }

        //[Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<SupplyDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var entity = new SupplyDto();
            var model = await _supplyRepository.GetAsync(id);
            model.MapTo(entity);

            entity.UserId_Name = _workFlowOrganizationUnitsManager.GetNames(model.UserId);
            //entity.StatusTitle = ((SupplyStatus)entity.Status).ToString();
            entity.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.用品附件 });
            return entity;
        }



        /// <summary>
        /// 获取用品列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<SupplyListDto>> GetAll(GetSupplyListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _supplyRepository.GetAll()
                            //join u in _userRepository.GetAll() on m.CreatorUserId equals u.Id
                        select new SupplyListDto()
                        {
                            Code = m.Code,
                            Id = m.Id,
                            CreationTime = m.CreationTime,
                            ExpiryDate = m.ExpiryDate,
                            Money = m.Money,
                            Name = m.Name,
                            ProductDate = m.ProductDate,
                            Status = m.Status,
                            Type = m.Type,
                            Unit = m.Unit,
                            UserId = m.UserId,
                            Version = m.Version,
                            //CreationUserName = u.Name,
                            LastModificationTime = m.LastModificationTime.Value,
                            PutInDate = m.PutInDate,
                        };

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Name.Contains(input.SearchKey) || r.Code.Contains(input.SearchKey));

            }
            if (!input.Status.IsNullOrWhiteSpace())
            {
                var tmp = Array.ConvertAll<string, int>(input.Status.Split(",", StringSplitOptions.RemoveEmptyEntries), new Converter<string, int>(ite => int.Parse(ite)));
                query = query.Where(ite => tmp.Contains(ite.Status));
            }
            var count = await query.CountAsync();
            var Supplys = await query
              .OrderByDescending(ite => ite.LastModificationTime)
             .PageBy(input)
             .ToListAsync();
            var SupplyDtos = new List<SupplyListDto>();
            foreach (var item in Supplys)
            {
                item.StatusTitle = ((SupplyStatus)item.Status).ToString();
                item.Type_Name = ((SupplyType)item.Type).ToString();
                item.UserId_Name = _workFlowOrganizationUnitsManager.GetNames(item.UserId);
            }
            return new PagedResultDto<SupplyListDto>(count, Supplys);

        }

        [AbpAuthorize]
        public async Task<AbpFileUploadResultModel> ExportExcle()
        {
            var query = from m in _supplyRepository.GetAll()
                            //join u in _userRepository.GetAll() on m.CreatorUserId equals u.Id
                        select new SupplyListDto()
                        {
                            Code = m.Code,
                            Id = m.Id,
                            CreationTime = m.CreationTime,
                            ExpiryDate = m.ExpiryDate,
                            Money = m.Money,
                            Name = m.Name,
                            ProductDate = m.ProductDate,
                            Status = m.Status,
                            Type = m.Type,
                            Unit = m.Unit,
                            UserId = m.UserId,
                            Version = m.Version,
                            //CreationUserName = u.Name,
                            LastModificationTime = m.LastModificationTime.Value,
                            PutInDate = m.PutInDate,
                        };


            var dt = new DataTable("用品总台账");
            dt.Columns.Add("编号", typeof(String));
            dt.Columns.Add("名称", typeof(String));
            dt.Columns.Add("品牌型号", typeof(String));
            dt.Columns.Add("类别", typeof(String));
            dt.Columns.Add("单价(元)", typeof(decimal));
            dt.Columns.Add("领用日期", typeof(DateTime));
            dt.Columns.Add("入库日期", typeof(DateTime));
            dt.Columns.Add("检定到期日期", typeof(DateTime));
            dt.Columns.Add("状态", typeof(String));
            foreach (var item in query)
            {
                dt.Rows.Add(new object[] { item.Code, item.Name, item.Version, ((SupplyType)item.Type).ToString(),
                    item.Money, item.ProductDate, item.PutInDate, item.ExpiryDate, ((SupplyStatus)item.Status).ToString() });
            }
            var abpExcleManager = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<AbpExcleManager>();
            string filePath = hostingEnv.WebRootPath + $@"/Files/makeExcel/supply/{DateTime.Now.Year}/{DateTime.Now.Month}/";
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            var userName = (await UserManager.GetUserByIdAsync(AbpSession.UserId.Value)).Name;
            var fileName = $"用品总台账_{DateTime.Now.ToString("yyyyMMdd")}";
            var fileExtend = ".xls";
            string fileFullName = filePath + fileName + fileExtend;
            abpExcleManager.WriteExcel(dt, fileFullName);
            var entity = new AbpFile()
            {
                Id = Guid.NewGuid(),
                FileName = fileName + fileExtend,
                FileExtend = fileExtend,
                FileSize = 0,
                FilePath = fileFullName,
            };
            _abpFilerepository.Insert(entity);
            var ret = new AbpFileUploadResultModel()
            {
                Id = entity.Id,
                FileName = entity.FileName + entity.FileExtend,
                Size = 0,
                FileExtend = entity.FileExtend,
            };
            return ret;
        }


        //[Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(SupplyCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var Supply = new SupplyBase();
            input.MapTo(Supply);
            var id = Guid.NewGuid();
            Supply.Id = id;
            Supply.Status = (int)SupplyStatus.在库;
            Supply.CreatorUserId = AbpSession.UserId;
            Supply.PutInDate = input.PutInDate;
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.用品附件,
                    Files = fileList
                });
            }
            await _supplyRepository.InsertAsync(Supply);
            ret.InStanceId = id.ToString();
            return ret;

        }


        public async Task Update(SupplyUpdateInput input)
        {
            var supply = await _supplyRepository.GetAsync(input.Id);
            input.MapTo(supply);
            var fileList = new List<AbpFileListInput>();
            if (input.FileList != null)
            {
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
            }
            await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
            {
                BusinessId = input.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.固定资产附件,
                Files = fileList
            });
            await _supplyRepository.UpdateAsync(supply);
        }
        /// <summary>
        /// 批量新增用品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateList(List<SupplyCreateInput> input)
        {
            if (input != null)
            {
                foreach (var x in input)
                {
                    var Supply = new SupplyBase();
                    x.MapTo(Supply);
                    var id = Guid.NewGuid();
                    Supply.Id = id;
                    Supply.Status = (int)SupplyStatus.在库;
                    Supply.CreatorUserId = AbpSession.UserId;
                    await _supplyRepository.InsertAsync(Supply);
                }
            }

        }

        public async Task Send(SupplySendDto input)
        {
            var user = _userRepository.GetAll().FirstOrDefault(ite => ite.Id == input.UserId);
            if (user == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到要发放的对象。");
            }
            var menuInit = await _userMenuInitRepository.FirstOrDefaultAsync(r => r.MenuId == 28 && r.UserId == input.UserId);
            if (menuInit==null||menuInit.Status!=-1)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "当前用户还未完成用品初始化，不能发放用品。");
            }
            var supplys = _supplyRepository.GetAll().Where(ite => input.SupplyId.Contains(ite.Id));
            foreach (var s in supplys)
            {
                s.Status = (int)SupplyStatus.被领用;
                var has = _userSupplyRepository.FirstOrDefault(ite => ite.SupplyId == s.Id && ite.Status != (int)UserSupplyStatus.已退还 && ite.Status == (int)UserSupplyStatus.已报废);
                if (has != null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "当前用品正在被别人使用，发放失败。");
                }
                var us = new UserSupply()
                {
                    Status = (int)UserSupplyStatus.使用中,
                    StartTime = DateTime.Now,
                    SupplyId = s.Id,
                    UserId = input.UserId,
                    EndTime = s.ExpiryDate
                };
                s.UserId = "u_" + input.UserId;
                await _userSupplyRepository.InsertAsync(us);
                await _supplyRepository.UpdateAsync(s);
            }
            await _noticeManager.CreateOrUpdateNoticeAsync(new ZCYX.FRMSCore.Application.NoticePublishInput()
            {
                Title = "用品发放",
                Content = "行政人员为你发放了新的个人用品，请注意查收",
                NoticeUserIds = input.UserId.ToString(),
                NoticeType = 1
            });

        }
        /// <summary>
        /// 计算当前资产总价值
        /// </summary>
        /// <returns></returns>
        public decimal Worth()
        {

            var sum = _supplyRepository.GetAll().Sum(ite => ite.Money);
            return sum;
        }



        /// <summary>
        /// 是否需要分管领导审核 若行政人员发起的流程 则需要分管领导审核
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        [AbpAuthorize]
        public bool IsNeedFGLDAudit(Guid flowID, Guid groupID)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowTaskManager>();
            var firstSender = _service.GetFirstSnderID(flowID, groupID);
            var query = from a in _roleRepository.GetAll()
                        join b in _userRoleRepository.GetAll() on a.Id equals b.RoleId
                        where b.UserId == AbpSession.UserId.Value
                        select a;

            if (query.Any(r => r.Name == "XZRY"))
                return true;
            else
                return false;
        }
    }
}

