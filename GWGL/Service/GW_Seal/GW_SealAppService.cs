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
    public class GW_SealAppService : FRMSCoreAppServiceBase, IGW_SealAppService
    {
        private readonly IRepository<GW_Seal, Guid> _repository;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        public GW_SealAppService(IRepository<GW_Seal, Guid> repository, ProjectAuditManager projectAuditManager, IAbpFileRelationAppService abpFileRelationAppService

        )
        {
            this._repository = repository;
            _projectAuditManager = projectAuditManager;
            _abpFileRelationAppService = abpFileRelationAppService;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<GW_SealListOutputDto>> GetList(GetGW_SealListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in UserManager.Users on a.KeepUser equals b.Id
                        let c = from u in UserManager.Users
                                where a.AuditUser.GetStrContainsArray(u.Id.ToString())
                                select u.Name
                        select new
                        {
                            a.Id,
                            a.Name,
                            KeepUser_Name = b.Name,
                            a.SealType,
                            a.Status,
                            a.Remark,
                            a.LastModificationTime,
                            AuditUserNames = c,
                            a.CreationTime
                        };
            if (input.Status.HasValue)
            {
                query = query.Where(r => r.Status == input.Status.Value);
            }
            if (input.SealType.HasValue)
            {
                query = query.Where(r => r.SealType == input.SealType.Value);
            }

            var toalCount = await query.CountAsync();
            var data = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            var ret = new List<GW_SealListOutputDto>();
            foreach (var item in data)
            {
                ret.Add(new GW_SealListOutputDto()
                {
                    ActiveDate = item.Status != GW_SealStatusEnmu.启用 ? null : item.LastModificationTime.HasValue ? item.LastModificationTime : item.CreationTime,
                    AuditUser_Name = string.Join(",", item.AuditUserNames),
                    CreationTime = item.CreationTime,
                    Id = item.Id,
                    KeepUser_Name = item.KeepUser_Name,
                    Name = item.Name,
                    Remark = item.Remark,
                    SealType_Name = item.SealType.ToString(),
                    Status_Title = item.Status.ToString(),
                });

            }
            return new PagedResultDto<GW_SealListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<GW_SealOutputDto> Get(EntityDto<Guid> input)
        {
            var ret = new GW_SealOutputDto();

            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in UserManager.Users on a.KeepUser equals b.Id
                        let c = from u in UserManager.Users
                                where a.AuditUser.GetStrContainsArray(u.Id.ToString())
                                select u
                        where a.Id == input.Id
                        select new
                        {
                            a.Id,
                            a.KeepUser,
                            a.Name,
                            KeepUser_Name = b.Name,
                            a.SealType,
                            a.Status,
                            a.Remark,
                            a.LastModificationTime,
                            AuditUserNames = c,
                            a.CreationTime
                        };
            var item = await query.FirstOrDefaultAsync();

            ret = new GW_SealOutputDto()
            {
                //ActiveDate = item.Status != GW_SealStatusEnmu.启用 ? null : item.LastModificationTime.HasValue ? item.LastModificationTime : item.CreationTime,
                AuditUser_Name = string.Join(",", item.AuditUserNames.Select(r => r.Name)),
                AuditUser = string.Join(",", item.AuditUserNames.Select(r => r.Id)),
                Id = item.Id,
                KeepUser = item.KeepUser,
                KeepUser_Name = item.KeepUser_Name,
                Name = item.Name,
                Remark = item.Remark,
                SealType = item.SealType,
                SealType_Name = item.SealType.ToString(),
                Status_Title = item.Status.ToString(),
                Status = item.Status,
            };
            ret.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = ret.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.公章图片
            });

            return ret;


        }
        /// <summary>
        /// 添加一个GW_Seal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateGW_SealInput input)
        {
            if (_repository.GetAll().Any(r => r.Name == input.Name && r.SealType == input.SealType))
                throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "同类型公章名称重复。");
            var newmodel = new GW_Seal()
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                KeepUser = input.KeepUser,
                AuditUser = input.AuditUser,
                SealType = input.SealType,
                Status = input.Status,
                Remark = input.Remark
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
                    BusinessType = (int)AbpFileBusinessType.公章图片,
                    Files = fileList
                });
            }

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个GW_Seal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateGW_SealInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                if (_repository.GetAll().Any(r => r.Id != input.Id && r.Name == input.Name && r.SealType == input.SealType))
                    throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "同类型公章名称重复。");

                var old_Model = dbmodel.DeepClone();
                var old_changeModel = GetChangeModel(old_Model);
                var fileData = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.Id.ToString(), BusinessType = (int)AbpFileBusinessType.公章图片 });
                old_changeModel.Files = fileData.Select(r => new AbpFileChangeDto { Id = r.Id, FileName = r.FileName }).ToList();


                dbmodel.Name = input.Name;
                dbmodel.KeepUser = input.KeepUser;
                dbmodel.AuditUser = input.AuditUser;
                dbmodel.SealType = input.SealType;
                dbmodel.Status = input.Status;
                dbmodel.Remark = input.Remark;

                await _repository.UpdateAsync(dbmodel);

                var fileList = new List<AbpFileListInput>();
                if (input.FileList != null)
                {
                    foreach (var item in input.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort, });
                    }
                }
                await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = input.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.公章图片,
                    Files = fileList
                });
                var new_changeModel = GetChangeModel(dbmodel);
                new_changeModel.Files = input.FileList.Select(r => new AbpFileChangeDto { Id = r.Id, FileName = r.FileName }).ToList();

                var logs = old_changeModel.GetColumnAllLogs(new_changeModel);
                await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), typeof(GW_SealAppService).ToString());

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


        private GW_SealChangeDto GetChangeModel(GW_Seal model)
        {
            /// 如果有外键数据 在这里转换
            var ret = new GW_SealChangeDto();
            ret.Id = model.Id;
            ret.Name = model.Name;
            ret.KeepUser_Name = UserManager.Users.SingleOrDefault(r => r.Id == model.KeepUser).Name;
            ret.AuditUser_Name = string.Join(",", UserManager.Users.Where(r => model.AuditUser.GetStrContainsArray(r.Id.ToString())).Select(r => r.Name));
            ret.SealType_Name = model.SealType.ToString();
            ret.Status_Title = model.Status.ToString();
            ret.Remark = model.Remark;
            return ret;
        }


        public async Task<List<ChangeLog>> GetChangeLogList(EntityDto<Guid> input)
        {
            return await _projectAuditManager.GetChangeLog(input, typeof(GW_SealAppService).ToString());

        }
    }
}