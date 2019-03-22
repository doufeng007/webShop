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
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.Application.Services;
using Abp.Authorization;
using ZCYX.FRMSCore.Users;

namespace XZGL
{
    public class XZGLCarBorrowAppService : FRMSCoreAppServiceBase, IXZGLCarBorrowAppService
    {
        private readonly IRepository<XZGLCarBorrow, Guid> _repository;
        private readonly IRepository<XZGLCarInfo, Guid> _carInfoRepository;
        private readonly IRepository<XZGLCar, Guid> _carRepository;
        private readonly IRepository<XZGLCarUser, Guid> _carUserRepository;
        private readonly IRepository<XZGLCarRelation, Guid> _carRelationRepository;
        private readonly IRepository<XZGLCarUserInfo, Guid> _carUserInfoRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<User, long> _usersRepository;
        private readonly IRepository<WorkFlowTask, Guid> _workFlowTaskRepository;
        public XZGLCarBorrowAppService(IRepository<XZGLCarBorrow, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IRepository<WorkFlowTask, Guid> workFlowTaskRepository, IRepository<User, long> usersRepository, IRepository<XZGLCarInfo, Guid> carInfoRepository, IRepository<XZGLCarUser, Guid> carUserRepository, IRepository<XZGLCarRelation, Guid> carRelationRepository, IRepository<XZGLCarUserInfo, Guid> carUserInfoRepository, IRepository<XZGLCar, Guid> carRepository
        )
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
            _usersRepository = usersRepository;
            _carInfoRepository = carInfoRepository;
            _carUserRepository = carUserRepository;
            _carRelationRepository = carRelationRepository;
            _carUserInfoRepository = carUserInfoRepository;
            _carRepository = carRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<XZGLCarBorrowListOutputDto>> GetList(GetXZGLCarBorrowListInput input)
        {
            var isAll = false;
            var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManagerNotRemote>();
            var userRoles = userManager.GetRoles(AbpSession.UserId.Value);
            if (userRoles.Any(r => r == "XZRY" || r == "ZJL"))
            {
                isAll = true;
            }
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted).WhereIf(input.CarType.HasValue,x=>x.CarType==input.CarType.Value).WhereIf(input.Status.HasValue,x=>x.Status==input.Status.Value)
                        join b in _usersRepository.GetAll() on a.CreatorUserId equals b.Id
                        let openModel = (from b in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() && x.ReceiveID == AbpSession.UserId.Value) select b)
                        where (isAll || (a.CreatorUserId == AbpSession.UserId.Value || a.DealWithUsers.GetStrContainsArray(AbpSession.UserId.HasValue ? AbpSession.UserId.Value.ToString() : "")))
                        select new XZGLCarBorrowListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            Status = a.Status,
                            UserId = a.UserId,
                            CreatorUserName = b.Name,
                            Tel = a.Tel,
                            StartTime = a.StartTime,
                            EndTime = a.EndTime,
                            CarType = a.CarType,
                            CarTypeName = a.CarType.ToString(),
                            Note = a.Note,
                            Remark = a.Remark,
                            CompanyRemark = a.CompanyRemark,
                            SupplierId = a.SupplierId,
                            SupplierTel = a.SupplierTel,
                            SupplierRemark = a.SupplierRemark,
                            CarRemark = a.CarRemark,
                            Consumption = a.Consumption,
                            CarReturnRemark = a.CarReturnRemark,
                            OtherRemark = a.OtherRemark,
                            UserCarRemark = a.UserCarRemark,
                            IsCompanyCar = a.IsCompanyCar,
                            IsCompanyRent = a.IsCompanyRent,
                            IsUserRent = a.IsUserRent
                            ,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0 ? 1 : 2,
                        };
            if (!string.IsNullOrEmpty(input.SearchKey))
            {
                query = query.Where(x=>x.Note.Contains(input.SearchKey) || x.CreatorUserName.Contains(input.SearchKey));
            }
            var toalCount = query.Count();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret) { item.InstanceId = item.Id.ToString(); _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item); }
            return new PagedResultDto<XZGLCarBorrowListOutputDto>(toalCount, ret);
        }
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<XZGLCarBorrowListOutputDto>> GetCarList(GetXZGLCarBorrowByCarListInput input)
        {
            var car = _carInfoRepository.GetAll().Where(x => x.CarId == input.CarId).Select(x => x.CarBorrowId).ToList();
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _usersRepository.GetAll() on a.CreatorUserId equals b.Id
                        where car.Contains(a.Id)
                        select new XZGLCarBorrowListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            Status = a.Status,
                            UserId = a.UserId,
                            CreatorUserName = b.Name,
                            Tel = a.Tel,
                            StartTime = a.StartTime,
                            EndTime = a.EndTime,
                            CarType = a.CarType,
                            CarTypeName = a.CarType.ToString(),
                            Note = a.Note,
                            Remark = a.Remark,
                            CompanyRemark = a.CompanyRemark,
                            SupplierId = a.SupplierId,
                            SupplierTel = a.SupplierTel,
                            SupplierRemark = a.SupplierRemark,
                            CarRemark = a.CarRemark,
                            Consumption = a.Consumption,
                            CarReturnRemark = a.CarReturnRemark,
                            OtherRemark = a.OtherRemark,
                            UserCarRemark = a.UserCarRemark,
                            IsCompanyCar = a.IsCompanyCar,
                            IsCompanyRent = a.IsCompanyRent,
                            IsUserRent = a.IsUserRent
                        };
            var toalCount = query.Count();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret) { item.InstanceId = item.Id.ToString(); _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item); }
            return new PagedResultDto<XZGLCarBorrowListOutputDto>(toalCount, ret);
        }
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<XZGLWorkOutCarOutputDto>> GetWorkOutList(GetXZGLCarBorrowByWorkOutListInput input)
        {
            var query = from a in _carRelationRepository.GetAll()
                        join c in _repository.GetAll() on a.CarBorrowId equals c.Id
                        join b in _usersRepository.GetAll() on c.CreatorUserId equals b.Id
                        where a.BusinessId==input.WorkOutId && a.BusinessType== CarRelationType.出差
                        select new XZGLWorkOutCarOutputDto()
                        {
                            Id = a.Id,
                           StartTime=c.StartTime,
                           EndTime=c.EndTime,
                           UserName=b.Name,
                           CarBorrowId=c.Id,
                        };
            var toalCount = query.Count();
            var ret = await query.OrderByDescending(r => r.StartTime).PageBy(input).ToListAsync();
            return new PagedResultDto<XZGLWorkOutCarOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task DeleteWorkOutRelation(EntityDto<Guid> input)
        {
            if (!_carRelationRepository.GetAll().Any(x => x.Id == input.Id))
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在。");
            await _carRelationRepository.DeleteAsync(x => x.Id == input.Id);
        }
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<XZGLCarBorrowOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = _repository.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var ret = model.MapTo<XZGLCarBorrowOutputDto>();

            var user = _usersRepository.Get(model.CreatorUserId.Value);
            ret.CreatorUserId = model.CreatorUserId;
            ret.CreatorUserName = user.Name;

            ret.CarTypeName = model.CarType.ToString();
            user = _usersRepository.Get(model.UserId);
            ret.UserName = user.Name;
            ret.CarUser = _carUserRepository.GetAll().Where(x => x.CarBorrowId == model.Id).Select(x => new XZGLCarUserOutput()
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Tel = x.Tel,
                CarMoney = x.CarMoney,
                OrderNum = x.OrderNum,
                CarType = x.CarType,
                CarNum = x.CarNum
            }).ToList();
            ret.CarInfo = (from a in _carInfoRepository.GetAll()
                           join b in _usersRepository.GetAll() on a.UserId equals b.Id
                           where a.CarBorrowId == model.Id
                           select new XZGLCarInfoOutput()
                           {
                               Id = a.Id,
                               CarId = a.CarId,
                               UserId = a.UserId,
                               UserName = b.Name,
                               Remark = a.Remark,
                               CarNum = a.CarNum,
                               CarType = a.CarType,
                               SeatNum = a.SeatNum,
                               Amount = a.Amount
                           }).ToList();
            var carUserInfo = _carUserInfoRepository.FirstOrDefault(x => x.CarBorrowId == model.Id);
            if (carUserInfo != null)
                ret.CarUserInfo = carUserInfo;
            ret.RelationWorkOut = _carRelationRepository.GetAll().Where(x => x.BusinessType == CarRelationType.出差 && x.CarBorrowId == model.Id).Select(x => x.BusinessId).ToList();
            ret.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.用车
            });
            return ret;
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<XZGLCarBorrowCarInfoOutputDto> GetCarInfo(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = _repository.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var dbmodel = _carInfoRepository.FirstOrDefault(x => x.CarBorrowId == id && x.UserId == AbpSession.UserId.Value);
            if (dbmodel == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "用车安排不存在！");
            var ret = model.MapTo<XZGLCarBorrowCarInfoOutputDto>();
            var user = _usersRepository.Get(model.CreatorUserId.Value);

            ret.CreatorUserId = model.CreatorUserId;
            ret.CreatorUserName = user.Name;

            ret.CarTypeName = model.CarType.ToString();
            user = _usersRepository.Get(model.UserId);
            ret.UserName = user.Name;
            user = _usersRepository.Get(dbmodel.UserId);
            ret.CarInfo = new XZGLCarInfoOutput()
            {
                Id = dbmodel.Id,
                CarId = dbmodel.CarId,
                UserId = dbmodel.UserId,
                UserName = user.Name,
                Remark = dbmodel.Remark,
                CarNum = dbmodel.CarNum,
                CarType = dbmodel.CarType,
                SeatNum = dbmodel.SeatNum,
                Amount = dbmodel.Amount
            };
            return ret;
        }

        /// <summary>
        /// 添加一个XZGLCarBorrow
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(CreateXZGLCarBorrowInput input)
        {
            var newmodel = new XZGLCarBorrow()
            {
                UserId = input.UserId,
                Tel = input.Tel,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                CarType = input.CarType,
                Note = input.Note,
                Remark = input.Remark,
                CompanyRemark = input.CompanyRemark,
                SupplierId = input.SupplierId,
                SupplierTel = input.SupplierTel,
                SupplierRemark = input.SupplierRemark,
                CarRemark = input.CarRemark,
                Consumption = input.Consumption,
                CarReturnRemark = input.CarReturnRemark,
                OtherRemark = input.OtherRemark,
                UserCarRemark = input.UserCarRemark,
                IsCompanyCar = input.IsCompanyCar,
                IsCompanyRent = input.IsCompanyRent,
                IsUserRent = input.IsUserRent
            };
            newmodel.Status = 0;
            _repository.Insert(newmodel);
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }
        
        public async Task UpdateRelation(RelationInput input)
        {
            _carRelationRepository.GetAll().Where(x => x.BusinessId == input.Id && x.BusinessType == input.TypeId).ToList().ForEach(x =>
             {
                 _carRelationRepository.Delete(x.Id);
             });
            if (input.CarBorrowIds.Count > 0)
            {
                foreach (var item in input.CarBorrowIds)
                {
                    var model = new XZGLCarRelation();
                    model.Id = Guid.NewGuid();
                    model.BusinessId = input.Id;
                    model.BusinessType = input.TypeId;
                    model.CarBorrowId = item;
                    _carRelationRepository.Insert(model);
                }
            }
        }
        public async Task UpdateCarInfo(UpdateXZGLCarInfoInput input)
        {
            var dbmodel = _repository.FirstOrDefault(x => x.Id == input.CarBorrowId);
            if (dbmodel == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var model = _carInfoRepository.FirstOrDefault(x => x.CarBorrowId == input.CarBorrowId && x.UserId == AbpSession.UserId.Value);
            if(model==null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "用车安排不存在！");
            model.Remark = input.Remark;
           await _carInfoRepository.UpdateAsync(model);
        }
        /// <summary>
        /// 修改一个XZGLCarBorrow
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateXZGLCarBorrowInput input)
        {
            if (input.InStanceId != Guid.Empty)
            {
                var dbmodel = _repository.FirstOrDefault(x => x.Id == input.InStanceId);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }
                var logModel = new XZGLCarBorrowLogDto();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.MapTo<XZGLCarBorrowLogDto>();
                    logModel.CarTypeName = dbmodel.CarType.ToString();
                    logModel.CarUser = _carUserRepository.GetAll().Where(x => x.CarBorrowId == dbmodel.Id).Select(x => new XZGLCarUserLogDto()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Address = x.Address,
                        Tel = x.Tel,
                        CarMoney = x.CarMoney,
                        OrderNum = x.OrderNum,
                        CarType = x.CarType,
                        CarNum = x.CarNum
                    }).ToList();
                    logModel.CarInfo = (from a in _carInfoRepository.GetAll()
                                        join b in _usersRepository.GetAll() on a.UserId equals b.Id
                                        where a.CarBorrowId == dbmodel.Id
                                        select new XZGLCarInfoLogDto()
                                        {
                                            Id= a.CarNum,
                                            UserName = b.Name,
                                            CarNum = a.CarNum,
                                        }).ToList();
                    var carUserInfo = _carUserInfoRepository.FirstOrDefault(x => x.CarBorrowId == dbmodel.Id);
                    if (carUserInfo != null)
                        logModel.CarUserInfo = carUserInfo.MapTo<XZGLCarUserInfoLogDto>();
                    logModel.Workouts = input.OldWorkouts;
                }
                dbmodel.UserId = input.UserId;
                dbmodel.Tel = input.Tel;
                dbmodel.StartTime = input.StartTime;
                dbmodel.EndTime = input.EndTime;
                dbmodel.CarType = input.CarType;
                dbmodel.Note = input.Note;
                dbmodel.Remark = input.Remark;
                dbmodel.CompanyRemark = input.CompanyRemark;
                dbmodel.SupplierId = input.SupplierId;
                dbmodel.SupplierTel = input.SupplierTel;
                dbmodel.SupplierRemark = input.SupplierRemark;
                dbmodel.CarRemark = input.CarRemark;
                dbmodel.Consumption = input.Consumption;
                dbmodel.CarReturnRemark = input.CarReturnRemark;
                dbmodel.OtherRemark = input.OtherRemark;
                dbmodel.UserCarRemark = input.UserCarRemark;
                dbmodel.IsCompanyCar = input.IsCompanyCar;
                dbmodel.IsCompanyRent = input.IsCompanyRent;
                dbmodel.IsUserRent = input.IsUserRent;
                await UpdateCarInfo(dbmodel.Id, input.CarInfo);
                await UpdateCarUser(dbmodel.Id, input.CarUser);
                await UpdateCarUserInfo(dbmodel.Id, input.CarUserInfo);
                await UpdateCarWorkOut(dbmodel.Id, input.RelationWorkOut);
                _repository.Update(dbmodel);

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
                    BusinessId = dbmodel.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.用车,
                    Files = fileList
                });
                if (input.IsUpdateForChange)
                {
                    var new_Model = dbmodel.MapTo<XZGLCarBorrowLogDto>();
                    new_Model.CarTypeName = input.CarType.ToString();
                    new_Model.CarUser = input.CarUser.Select(x => new XZGLCarUserLogDto()
                    {
                        Id=x.Id,
                        Name = x.Name,
                        Address = x.Address,
                        Tel = x.Tel,
                        CarMoney = x.CarMoney,
                        OrderNum = x.OrderNum,
                        CarType = x.CarType,
                        CarNum = x.CarNum
                    }).ToList();
                    new_Model.Workouts = input.NewWorkouts;
                    new_Model.CarInfo = (from a in input.CarInfo
                                        join b in _usersRepository.GetAll() on a.UserId equals b.Id
                                        join c in _carRepository.GetAll() on a.CarId equals c.Id
                                        select new XZGLCarInfoLogDto()
                                        {
                                            Id = c.CarNum,
                                            UserName = b.Name,
                                            CarNum = c.CarNum,
                                        }).ToList();
                    var carUserInfo = input.CarUserInfo;
                    if (carUserInfo != null)
                        new_Model.CarUserInfo = carUserInfo.MapTo<XZGLCarUserInfoLogDto>();

                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.CodeValErr, "流程不存在");
                    var files = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = logModel.Id.ToString(), BusinessType = (int)AbpFileBusinessType.用车 });
                    if (files.Count > 0)
                        logModel.Files = files.Select(r => new AbpFileChangeDto { Id = r.Id, FileName = r.FileName }).ToList();

                    if (input.FileList.Count > 0)
                    {
                        new_Model.Files = input.FileList.Select(r => new AbpFileChangeDto { FileName = r.FileName, Id = r.Id }).ToList();
                    }
                    var logs = logModel.GetColumnAllLogs(new_Model);
                    await _projectAuditManager.InsertAsync(logs, input.InStanceId.ToString(), flowModel.TitleField.Table);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }
        private async Task UpdateCarUserInfo(Guid id, XZGLCarUserInfoInput userInfo)
        {
            if (userInfo != null)
            {
                var model = _carUserInfoRepository.FirstOrDefault(x => x.CarBorrowId == id);
                var isnew = model == null;
                if (isnew)
                    model = new XZGLCarUserInfo();
                model.Name = userInfo.Name;
                model.CarNum = userInfo.CarNum;
                model.CarType = userInfo.CarType;
                model.Amount = userInfo.Amount;
                model.SeatNum = userInfo.SeatNum;
                model.Number = userInfo.Number;
                model.Type = userInfo.Type;
                model.CarTypeNum = userInfo.CarTypeNum;
                model.Remark = userInfo.Remark;
                if (isnew)
                {
                    model.CarBorrowId = id;
                    model.Id = Guid.NewGuid();
                    await _carUserInfoRepository.InsertAsync(model);
                }
                else
                    await _carUserInfoRepository.UpdateAsync(model);
            }
        }
        private async Task UpdateCarWorkOut(Guid id, List<Guid> list)
        {
            var carRelations = _carRelationRepository.GetAll().Where(x => x.CarBorrowId == id && x.BusinessType == CarRelationType.出差).ToList();
            foreach (var item in carRelations)
            {
                _carRelationRepository.Delete(x=>x.Id==item.Id);
            } 
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    var model = new XZGLCarRelation();
                    model.Id = Guid.NewGuid();
                    model.BusinessId = item;
                    model.BusinessType = CarRelationType.出差;
                    model.CarBorrowId = id;
                    _carRelationRepository.Insert(model);
                }
            }
        }
        private async Task UpdateCarUser(Guid id, List<XZGLCarUserInput> list)
        {
            var carUsers = _carUserRepository.GetAll().Where(x => x.CarBorrowId == id).ToList();
            foreach (var item in carUsers)
            {
                _carUserRepository.Delete(x => x.Id == item.Id);
            }
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    var model = new XZGLCarUser();
                    model.Id = Guid.NewGuid();
                    model.Name = item.Name;
                    model.Address = item.Address;
                    model.Tel = item.Tel;
                    model.CarMoney = item.CarMoney;
                    model.OrderNum = item.OrderNum;
                    model.CarType = item.CarType;
                    model.CarNum = item.CarNum;
                    model.CarBorrowId = id;
                    _carUserRepository.Insert(model);
                }
            }
        }

        private async Task UpdateCarInfo(Guid id, List<XZGLCarInfoInput> list)
        {
            var carinfos = _carInfoRepository.GetAll().Where(x => x.CarBorrowId == id).ToList();
            if (list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var model = carinfos.FirstOrDefault(x => x.CarId == item.CarId);
                    var car = _carRepository.FirstOrDefault(x => x.Id == item.CarId);
                    if (car == null)
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "选中车辆不存在。");
                    var isnew = model == null;
                    if (isnew)
                        model = new XZGLCarInfo();
                    model.CarId = car.Id;
                    model.UserId = item.UserId;
                    model.CarNum = car.CarNum;
                    model.CarType = car.CarType;
                    model.SeatNum = car.SeatNum;
                    model.Amount = car.Amount;
                    if (isnew)
                    {
                        model.CarBorrowId = id;
                        model.Id = Guid.NewGuid();
                        await _carInfoRepository.InsertAsync(model);
                    }
                    else
                        await _carInfoRepository.UpdateAsync(model);
                }
            }
            if (list.Count > 0)
            {
                foreach (var item in carinfos.Where(x => !list.Select(y => y.CarId).Contains(x.CarId)))
                    _carInfoRepository.Delete(x => x.Id == item.Id);
            }
        }

        [RemoteService(IsEnabled = false)]
        [AbpAuthorize]
        public XZGLCarBorrow GetCarBorrow(string InstanceID)
        {
            var id = Guid.Parse(InstanceID);
            var model = _repository.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model;
        }

        [RemoteService(IsEnabled = false)]
        [AbpAuthorize]
        public string GetCarDriver(string InstanceID)
        {
            var id = Guid.Parse(InstanceID);
            var model = _repository.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var users = string.Join(',',_carInfoRepository.GetAll().Where(x => x.CarBorrowId == id).Select(x=>"u_"+x.UserId).ToList());
            return users;
        }

        [RemoteService(IsEnabled = false)]
        [AbpAuthorize]
        public string GetUserId(string InstanceID)
        {
            var id = Guid.Parse(InstanceID);
            var model = _repository.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            if (model.CreatorUserId == model.UserId)
                return "";
            return "u_"+model.UserId;
        }
    }
}