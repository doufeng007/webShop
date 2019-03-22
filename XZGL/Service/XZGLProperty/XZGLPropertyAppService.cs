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
using Abp.WorkFlowDictionary;
using ZCYX.FRMSCore.Authorization.Users;

namespace XZGL
{
    public class XZGLPropertyAppService : FRMSCoreAppServiceBase, IXZGLPropertyAppService
    { 
        private readonly IRepository<XZGLProperty, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<AbpDictionary, Guid> _dictionaryRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Follow, Guid> _followRepository;
        public XZGLPropertyAppService(IRepository<XZGLProperty, Guid> repository, IAbpFileRelationAppService abpFileRelationAppService, IRepository<AbpDictionary, Guid> dictionaryRepository, IRepository<User, long> userRepository, IRepository<Follow, Guid> followRepository)
        {
            this._repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _dictionaryRepository = dictionaryRepository;
            _userRepository = userRepository;
            _followRepository = followRepository;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<XZGLPropertyListOutputDto>> GetList(GetXZGLPropertyListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
                        join b in _userRepository.GetAll() on a.UserId equals b.Id into tmp1 from c in tmp1.DefaultIfEmpty()
                        join g in _followRepository.GetAll().Where(x => x.BusinessType == FollowType.物业管理 && x.CreatorUserId == AbpSession.UserId.Value) on a.Id equals g.BusinessId into tmp2
                        from h in tmp2.DefaultIfEmpty()
                        where (input.IsFollow.HasValue && input.IsFollow.Value ? h != null : true)
                        select new XZGLPropertyListOutputDto()
                        {
                            Id = a.Id,
                            Number = a.Number,
                            Type = a.Type,
                            TypeName = a.TypeName,
                            Name = a.Name,
                            IsFollow = h != null,
                            Remark = a.Remark,
                            UserName = c != null ? c.Name : "",
                            UserId = a.UserId,
                            IsEnable = a.IsEnable,
                            CreationTime = a.CreationTime
                        };
            if (!string.IsNullOrEmpty(input.SearchKey))
                query = query.Where(x => x.Name.Contains(input.SearchKey) || x.Remark.Contains(input.SearchKey));
            if (input.IsEnable.HasValue)
            {
                query = query.Where(x => x.IsEnable==input.IsEnable.Value);
            }
            if (input.Type.HasValue)
                query = query.Where(x => x.Type == input.Type.Value);
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<XZGLPropertyListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<XZGLPropertyOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }

            var isFollow = _followRepository.GetAll().FirstOrDefault(x => x.BusinessType == FollowType.物业管理 && x.CreatorUserId == AbpSession.UserId.Value && x.BusinessId == model.Id) != null;
            var ret = model.MapTo<XZGLPropertyOutputDto>();
            ret.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.物业管理
            });
            var user = _userRepository.GetAll().FirstOrDefault(x => x.Id == ret.UserId);
            ret.UserName = user?.Name;
            ret.IsFollow = isFollow;
            return ret;
		}
        /// <summary>
        /// 添加一个XZGLProperty
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateXZGLPropertyInput input)
        {
            var id = Guid.NewGuid();
            var type = _dictionaryRepository.GetAll().FirstOrDefault(x => !x.IsDeleted && x.Id == input.Type);
            var newmodel = new XZGLProperty()
            {
                Id = id,
                Type = input.Type,
                Name = input.Name,
                Remark = input.Remark,
                TypeName = type?.Title,
                UserId = input.UserId,
                IsEnable = input.IsEnable
            };

            await _repository.InsertAsync(newmodel);
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
                    BusinessType = (int)AbpFileBusinessType.物业管理,
                    Files = fileList
                });
            }
        }

		/// <summary>
        /// 修改一个XZGLProperty
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateXZGLPropertyInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }
                var type = _dictionaryRepository.GetAll().FirstOrDefault(x => !x.IsDeleted && x.Id == input.Type);
                dbmodel.TypeName = type?.Title;
                dbmodel.Type = input.Type;
			   dbmodel.Name = input.Name;
			   dbmodel.Remark = input.Remark;
			   dbmodel.UserId = input.UserId;
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
                    BusinessType = (int)AbpFileBusinessType.物业管理,
                    Files = fileList
                });
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Enable(EntityDto<Guid> input)
        {
            var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (dbmodel == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            if (dbmodel.IsEnable.HasValue)
                dbmodel.IsEnable = !dbmodel.IsEnable;
            else
                dbmodel.IsEnable = true;
            await _repository.UpdateAsync(dbmodel);
        }
        /// <summary>
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