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
using Abp.Authorization;
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class ProjectFileAdditionalAppService : FRMSCoreAppServiceBase, IProjectFileAdditionalAppService
    {
        private readonly IRepository<ProjectFileAdditional, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;

        public ProjectFileAdditionalAppService(IRepository<ProjectFileAdditional, Guid> repository, IAbpFileRelationAppService abpFileRelationAppService)
        {
            this._repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProjectFileAdditionalListOutputDto>> GetList(GetProjectFileAdditionalListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in UserManager.Users on a.CreatorUserId equals b.Id
                        select new ProjectFileAdditionalListOutputDto()
                        {
                            Id = a.Id,
                            FileTypeName = a.FileTypeName,
                            ProjectBaseId = a.ProjectBaseId,
                            PaperFileNumber = a.PaperFileNumber,
                            IsPaperFile = a.IsPaperFile,
                            IsNeedReturn = a.IsNeedReturn,
                            CreationTime = a.CreationTime,
                            CreateUserName = b.Name,

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                item.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = item.Id.ToString(), BusinessType = (int)AbpFileBusinessType.补充资料附件 });
            }
            return new PagedResultDto<ProjectFileAdditionalListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task<ProjectFileAdditionalOutputDto> Get(NullableIdDto<Guid> input)
        {
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var ret = model.MapTo<ProjectFileAdditionalOutputDto>();
            ret.CreateUserName = (await UserManager.GetUserByIdAsync(model.CreatorUserId.Value)).Name;
            ret.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = ret.Id.ToString(), BusinessType = (int)AbpFileBusinessType.补充资料附件 });
            return ret;
        }
        /// <summary>
        /// 添加一个ProjectFileAdditional
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task Create(CreateProjectFileAdditionalInput input)
        {
            var newmodel = new ProjectFileAdditional()
            {
                Id = Guid.NewGuid(),
                FileTypeName = input.FileTypeName,
                ProjectBaseId = input.ProjectBaseId,
                PaperFileNumber = input.PaperFileNumber,
                IsPaperFile = input.IsPaperFile,
                IsNeedReturn = input.IsNeedReturn
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
                    BusinessType = (int)AbpFileBusinessType.补充资料附件,
                    Files = fileList
                });
            }

            await _repository.InsertAsync(newmodel);
        }

        /// <summary>
        /// 修改一个ProjectFileAdditional
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateProjectFileAdditionalInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                dbmodel.FileTypeName = input.FileTypeName;
                dbmodel.ProjectBaseId = input.ProjectBaseId;
                dbmodel.PaperFileNumber = input.PaperFileNumber;
                dbmodel.IsPaperFile = input.IsPaperFile;
                dbmodel.IsNeedReturn = input.IsNeedReturn;
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
                    BusinessType = (int)AbpFileBusinessType.补充资料附件,
                    Files = fileList
                });
                await _repository.UpdateAsync(dbmodel);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
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
    }
}