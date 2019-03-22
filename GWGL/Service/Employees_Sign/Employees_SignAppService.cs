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

namespace GWGL
{
    public class Employees_SignAppService : FRMSCoreAppServiceBase, IEmployees_SignAppService
    {
        private readonly IRepository<Employees_Sign, Guid> _repository;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;

        public Employees_SignAppService(IRepository<Employees_Sign, Guid> repository, ProjectAuditManager projectAuditManager, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository

        )
        {
            this._repository = repository;
            _projectAuditManager = projectAuditManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<Employees_SignListOutputDto>> GetList(GetEmployees_SignListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in UserManager.Users on a.UserId equals b.Id
                        join uo in _userOrganizationUnitRepository.GetAll() on a.UserId equals uo.UserId
                        join o in _organizationUnitRepository.GetAll() on uo.OrganizationUnitId equals o.Id
                        where uo.IsMain == true
                        select new
                        {
                            a.Id,
                            User_Name = b.Name,
                            User_OrgName = o.DisplayName,
                            a.SignType,
                            a.Status,
                            a.CreationTime,
                            a.LastModificationTime,
                            //ActiveDate = a.Status != GW_EmployeesSignStatusEnmu.启用 ? null : a.LastModificationTime.HasValue ? a.LastModificationTime : a.CreationTime,
                        };
            if (input.Status.HasValue)
                query = query.Where(r => r.Status == input.Status.Value);
            var toalCount = await query.CountAsync();
            var data = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            var ret = new List<Employees_SignListOutputDto>();
            foreach (var item in data)
            {
                ret.Add(new Employees_SignListOutputDto()
                {
                    ActiveDate = item.Status != GW_EmployeesSignStatusEnmu.启用 ? null : item.LastModificationTime.HasValue ? item.LastModificationTime : item.CreationTime,
                    CreationTime = item.CreationTime,
                    Id = item.Id,
                    SignType_Name = item.SignType.ToString(),
                    Status_Title = item.Status.ToString(),
                    User_Name = item.User_Name,
                    User_OrgName = item.User_OrgName,
                });
            }
            return new PagedResultDto<Employees_SignListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<Employees_SignOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var usermodel = await UserManager.GetUserByIdAsync(model.UserId);
            var ret = new Employees_SignOutputDto()
            {
                CreationTime = model.CreationTime,
                Id = model.Id,
                SignType = model.SignType,
                Status = model.Status,
                SignType_Name = model.SignType.ToString(),
                Status_Title = model.Status.ToString(),
                UserId = model.UserId,
                UserName = usermodel.Name,
                FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
                {
                    BusinessId = model.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.员工签名图片
                }),
            };
            return ret;
        }
        /// <summary>
        /// 添加一个Employees_Sign
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateEmployees_SignInput input)
        {
            if (_repository.GetAll().Any(x => x.UserId == input.UserId))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "员工已存在签名。");
            }

            var newmodel = new Employees_Sign()
            {
                Id = Guid.NewGuid(),
                UserId = input.UserId,
                SignType = input.SignType,
                Status = input.Status
            };

            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = newmodel.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.员工签名图片,
                    Files = fileList
                });
            }

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个Employees_Sign
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateEmployees_SignInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }
                if (_repository.GetAll().Any(x => x.UserId == input.UserId && x.Id != input.Id))
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "员工已存在签名。");
                }
                var old_Model = dbmodel.DeepClone();
                var old_changeModel = GetChangeModel(old_Model);
                var fileData = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.Id.ToString(), BusinessType = (int)AbpFileBusinessType.员工签名图片 });
                old_changeModel.Files = fileData.Select(r => new AbpFileChangeDto { Id = r.Id, FileName = r.FileName }).ToList();

                dbmodel.UserId = input.UserId;
                dbmodel.SignType = input.SignType;
                dbmodel.Status = input.Status;

                await _repository.UpdateAsync(dbmodel);

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
                    BusinessType = (int)AbpFileBusinessType.员工签名图片,
                    Files = fileList
                });
                var new_changeModel = GetChangeModel(dbmodel);
                new_changeModel.Files = input.FileList.Select(r => new AbpFileChangeDto { Id = r.Id, FileName = r.FileName }).ToList();

                var logs = old_changeModel.GetColumnAllLogs(new_changeModel);
                await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), typeof(Employees_SignAppService).ToString());

            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }

        private Employees_SignChangeDto GetChangeModel(Employees_Sign model)
        {
            /// 如果有外键数据 在这里转换
            var ret = new Employees_SignChangeDto();
            ret.Id = model.Id;
            var usermodel = UserManager.Users.FirstOrDefault(x=>x.Id==model.UserId);
            ret.Name = usermodel.Name;
            ret.SignType_Name = model.SignType.ToString();
            ret.Status_Title = model.Status.ToString();
            return ret;
        }


        public async Task<List<ChangeLog>> GetChangeLogList(EntityDto<Guid> input)
        {
            return await _projectAuditManager.GetChangeLog(input, typeof(Employees_SignAppService).ToString());

        }
    }
}