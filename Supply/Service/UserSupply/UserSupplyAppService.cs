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
using Abp.UI;
using ZCYX.FRMSCore.Dto;
using ZCYX.FRMSCore.Model;
using System.Data;
using Abp.Excel;
using Abp;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Supply
{
    public class UserSupplyAppService : FRMSCoreAppServiceBase, IUserSupplyAppService
    {
        private readonly IRepository<UserSupply, Guid> _userSupplyRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly ISupplyBaseRepository _supplyBaseRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly IRepository<UserMenuInit, Guid> _userMenuInitRepository;
        private IHostingEnvironment hostingEnv;
        private readonly IRepository<AbpFile, Guid> _abpFilerepository;


        public UserSupplyAppService(IRepository<UserSupply, Guid> userSupplyRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , ISupplyBaseRepository supplyBaseRepository, WorkFlowTaskManager workFlowTaskManager
            , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager
            , IRepository<UserMenuInit, Guid> userMenuInitRepository, IHostingEnvironment env
            , IRepository<AbpFile, Guid> abpFilerepository)
        {
            _userSupplyRepository = userSupplyRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _supplyBaseRepository = supplyBaseRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _userMenuInitRepository = userMenuInitRepository;
            hostingEnv = env;
            _abpFilerepository = abpFilerepository;
        }

        public async Task<UserSupplyDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var userSupply = from a in _userSupplyRepository.GetAll()
                             join b in UserManager.Users on a.UserId equals b.Id
                             where a.Id == id
                             select new { a, UserName = b.Name };
            var data = await userSupply.FirstOrDefaultAsync();
            if (data == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "个人用品未找到");
            var output = new UserSupplyDto();
            var supply = await _supplyBaseRepository.GetAsync(data.a.SupplyId);
            output.Supply = supply.MapTo<SupplyDto>();
            output.Supply.UserId_Name = _workFlowOrganizationUnitsManager.GetNames(output.Supply.UserId);
            output.StartTime = data.a.StartTime;
            output.EndTime = data.a.EndTime;
            output.UserId = data.a.UserId;
            output.UserId_Name = data.UserName;
            return output;
        }



        [AbpAuthorize]
        public async Task<InitMenuPagedResultDto<UserSupplyListDto>> GetAll(GetUserSupplyListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var menuInit = await _userMenuInitRepository.FirstOrDefaultAsync(r => r.MenuId == input.MenuId && r.UserId == currentUserId);
            if (menuInit == null) return new InitMenuPagedResultDto<UserSupplyListDto>() { HasComplateInit = false };
            else
            {
                var hasComplateInit = true;
                if (menuInit.Status != -1)
                    hasComplateInit = false;
                var query = from m in _userSupplyRepository.GetAll()
                            join b in UserManager.Users on m.UserId equals b.Id
                            join u in _supplyBaseRepository.GetAll() on m.SupplyId equals u.Id
                            where b.Id == AbpSession.UserId.Value && m.Status != (int)UserSupplyStatus.已退还 && m.Status != (int)UserSupplyStatus.已报废
                            select new UserSupplyListDto()
                            {
                                Id = m.Id,
                                Unit = u.Unit,
                                SupplyId = u.Id,
                                SupplyType = u.Type,
                                Supply_Code = u.Code,
                                Supply_Money = u.Money,
                                Supply_Name = u.Name,
                                Supply_Version = u.Version,
                                Status = m.Status,
                                StartTime = m.StartTime,
                                EndTime = m.EndTime,
                                Supply_UserId = u.UserId,

                                CreationTime = m.CreationTime,
                            };

                if (!input.SearchKey.IsNullOrWhiteSpace())
                {
                    query = query.Where(r => r.Supply_Code.Contains(input.SearchKey) || r.Supply_Name.Contains(input.SearchKey));
                }
                query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
                var count = await query.CountAsync();
                var userSupplys = await query
                 .OrderByDescending(ite => ite.CreationTime)
                 .PageBy(input)
                 .ToListAsync();
                foreach (var item in userSupplys)
                {
                    item.SupplyTypeTitle = ((SupplyType)item.SupplyType).ToString();
                    item.StatusTitle = ((UserSupplyStatus)item.Status).ToString();
                    item.Supply_UserId_Name = _workFlowOrganizationUnitsManager.GetNames(item.Supply_UserId);

                }
                return new InitMenuPagedResultDto<UserSupplyListDto>(count, userSupplys, hasComplateInit);
            }
        }


        [AbpAuthorize]
        public async Task<AbpFileUploadResultModel> ExportExcle()
        {
            var query = from m in _userSupplyRepository.GetAll()
                        join b in UserManager.Users on m.UserId equals b.Id
                        join u in _supplyBaseRepository.GetAll() on m.SupplyId equals u.Id
                        join ownerUser in UserManager.Users on u.UserId equals "u_" + ownerUser.Id into g
                        from ou in g.DefaultIfEmpty()
                        where b.Id == AbpSession.UserId.Value && m.Status != (int)UserSupplyStatus.已退还 && m.Status != (int)UserSupplyStatus.已报废
                        select new UserSupplyListDto()
                        {
                            Id = m.Id,
                            Unit = u.Unit,
                            SupplyId = u.Id,
                            SupplyType = u.Type,
                            Supply_Code = u.Code,
                            Supply_Money = u.Money,
                            Supply_Name = u.Name,
                            Supply_Version = u.Version,
                            Status = m.Status,
                            StartTime = m.StartTime,
                            EndTime = m.EndTime,
                            Supply_UserId = u.UserId,
                            CreationTime = m.CreationTime,
                            SupplyTypeTitle = ((SupplyType)u.Type).ToString(),
                            StatusTitle = ((UserSupplyStatus)m.Status).ToString(),
                            Supply_UserId_Name = ou == null ? "" : ou.Name,
                        };
            var dt = new DataTable("用品清单");
            dt.Columns.Add("编号", typeof(String));
            dt.Columns.Add("名称", typeof(String));
            dt.Columns.Add("品牌型号", typeof(String));
            dt.Columns.Add("类别", typeof(String));
            dt.Columns.Add("单价(元)", typeof(decimal));
            dt.Columns.Add("用户", typeof(String));
            dt.Columns.Add("领用日期", typeof(DateTime));
            dt.Columns.Add("检定到期日期", typeof(DateTime));
            dt.Columns.Add("状态", typeof(String));
            foreach (var item in query)
            {
                dt.Rows.Add(new object[] { item.Supply_Code, item.Supply_Name, item.Supply_Version, item.SupplyTypeTitle,
                    item.Supply_Money, item.Supply_UserId_Name, item.StartTime, item.EndTime, item.StatusTitle });
            }
            var abpExcleManager = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<AbpExcleManager>();
            string filePath = hostingEnv.WebRootPath + $@"/Files/makeExcel/userSupply/{DateTime.Now.Year}/{DateTime.Now.Month}/";
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            var userName = (await UserManager.GetUserByIdAsync(AbpSession.UserId.Value)).Name;
            var fileName = $"{userName}-个人用品清单";
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

        [AbpAuthorize]
        public async Task CreateOrUpdate(CreateMenuInitInput<List<UserSupplyBatchCreateInput>> input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var menuInit = await _userMenuInitRepository.FirstOrDefaultAsync(r => r.MenuId == input.MenuId && r.UserId == currentUserId);
            if (menuInit == null)
            {
                var init = new UserMenuInit() { MenuId = input.MenuId, UserId = currentUserId, Status = input.HasComplateInit ? -1 : 0 };
                _userMenuInitRepository.Insert(init);
            }
            else
            {
                if (input.HasComplateInit)
                {
                    menuInit.Status = -1;
                    _userMenuInitRepository.Update(menuInit);
                }
            }
            if (input.List.Count > 50)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "批量修改最大支持50条数据");
            var uids = input.List.Where(ite => ite.Id.HasValue).Select(ite => ite.Id).ToList();
            var sids = input.List.Where(ite => ite.SupplyId.HasValue).Select(ite => ite.SupplyId).ToList();

            //删除没有的数据
            //_userSupplyRepository.Delete(ite => ite.CreatorUserId == AbpSession.UserId.Value && !uids.Contains(ite.Id));
            //_supplyBaseRepository.Delete(ite => ite.CreatorUserId == AbpSession.UserId.Value && !sids.Contains(ite.Id));

            foreach (var item in input.List)
            {
                if (item.Id.HasValue)
                {
                    if (!item.SupplyId.HasValue)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "用品数据异常");
                    var model = await _supplyBaseRepository.GetAsync(item.SupplyId.Value);
                    model.CreatorUserId = AbpSession.UserId;
                    model.Unit = item.Unit;
                    model.Money = item.Money;
                    model.Version = item.Version;
                    model.Name = item.Name;
                    model.Type = item.Type;
                    model.UserId = item.Supply_UserId;
                    model.ExpiryDate = item.EndTime;

                    //model.Status = (int)SupplyStatus.被领用;

                    var usersupplyModel = await _userSupplyRepository.GetAsync(item.Id.Value);
                    usersupplyModel.StartTime = item.StartTime;
                    usersupplyModel.EndTime = item.EndTime;
                    // usersupplyModel.Status = (int)UserSupplyStatus.使用中;
                }
                else
                {
                    var supplyId = Guid.NewGuid();
                    var model = new SupplyBase()
                    {
                        Id = supplyId,
                        Code = "",
                        Unit = item.Unit,
                        Money = item.Money,
                        Name = item.Name,
                        Type = item.Type,
                        Version = item.Version,
                        CreatorUserId = AbpSession.UserId,
                        ExpiryDate = item.EndTime,
                        ProductDate = DateTime.Now,
                        UserId = item.Supply_UserId,
                        Status = (int)SupplyStatus.被领用
                    };
                    await _supplyBaseRepository.InsertAsync(model);
                    var usersupplyModel = new UserSupply()
                    {
                        SupplyId = supplyId,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        Id = Guid.NewGuid(),
                        UserId = AbpSession.UserId.Value,
                        Status = (int)UserSupplyStatus.使用中
                    };
                    await _userSupplyRepository.InsertAsync(usersupplyModel);
                }

            }
        }
        /// <summary>
        /// 用品删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task Delete(Guid input)
        {
            var us = _userSupplyRepository.Get(input);
            var s = _supplyBaseRepository.Get(us.SupplyId);
            await _userSupplyRepository.DeleteAsync(us);
            await _supplyBaseRepository.DeleteAsync(s);
        }
    }
}

