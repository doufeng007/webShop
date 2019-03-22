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
using Abp.WorkFlowDictionary;
using ZCYX.FRMSCore.Model;

namespace HR
{
    public class QuestionLibraryAppService : FRMSCoreAppServiceBase, IQuestionLibraryAppService
    { 
        private readonly IRepository<QuestionLibrary, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryRepository;
        private readonly IRepository<AbpFileRelation, Guid> _abpFileRelationRepository;
        private readonly IRepository<AbpFile, Guid> _abpFileRepository;
        public QuestionLibraryAppService(IRepository<QuestionLibrary, Guid> repository,IAbpFileRelationAppService abpFileRelationAppService, IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IRepository<PostInfo, Guid> postsRepository, IRepository<AbpDictionary, Guid> abpDictionaryRepository, IRepository<AbpFileRelation, Guid> abpFileRelationRepository, IRepository<AbpFile, Guid> abpFileRepository
        )
        {
            this._repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _abpDictionaryRepository = abpDictionaryRepository;

            _abpFileRelationRepository = abpFileRelationRepository;
            _abpFileRepository = abpFileRepository;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<QuestionLibraryListOutputDto>> GetList(GetQuestionLibraryListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
                        join b in _abpDictionaryRepository.GetAll() on a.TypeId equals b.Id
                        select new QuestionLibraryListOutputDto()
                        {
                            Id = a.Id,
                            Number = a.Number,
                            Title = a.Title,
                            TypeId = a.TypeId,
                            TypeId_Name = b.Title,
                            Remark = a.Remark,
                            CreationTime = a.CreationTime
                        };
            if (!input.TypeId.IsEmptyGuid())
                query = query.Where(x => x.TypeId == input.TypeId);
            if (!string.IsNullOrEmpty(input.SearchKey))
            {
                query = query.Where(x => x.Title.Contains(input.SearchKey) || x.Remark.Contains(input.SearchKey) || (from a in _abpFileRepository.GetAll()
                                                                                                                     join b in _abpFileRelationRepository.GetAll() on a.Id equals b.FileId
                                                                                                                     where a.FileName.Contains(input.SearchKey) && b.BusinessId == x.Id.ToString() && b.BusinessType == (int)AbpFileBusinessType.人力资源题库
                                                                                                                     select a.Id).Count() > 0);
            }
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var r in ret)
            {
                r.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.人力资源题库 });
            }
            return new PagedResultDto<QuestionLibraryListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<QuestionLibraryOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var entity = model.MapTo<QuestionLibraryOutputDto>();
            entity.TypeId_Name = _abpDictionaryRepository.Get(model.TypeId)?.Title;
            entity.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.人力资源题库
            });
            return entity;
        }
        /// <summary>
        /// 添加一个QuestionLibrary
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateQuestionLibraryInput input)
        {
            if (_repository.GetAll().Any(x => x.Title == input.Title))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该题库已存在。");
            }
            var id = Guid.NewGuid();
            var newmodel = new QuestionLibrary()
            {
                Id = id,
                Title = input.Title,
                TypeId = input.TypeId,
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
                    BusinessId = id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.人力资源题库,
                    Files = fileList
                });
            }
            await _repository.InsertAsync(newmodel);

        }

		/// <summary>
        /// 修改一个QuestionLibrary
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateQuestionLibraryInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                if (_repository.GetAll().Any(x => x.Title == input.Title && x.Id != input.Id))
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该题库已存在。");
                }

                dbmodel.Title = input.Title;
			   dbmodel.TypeId = input.TypeId;
			   dbmodel.Remark = input.Remark;
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
                    BusinessType = (int)AbpFileBusinessType.人力资源题库,
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
            await _repository.DeleteAsync(x=>x.Id == input.Id);
        }
    }
}