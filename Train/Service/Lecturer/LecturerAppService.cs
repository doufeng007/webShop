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
using HR;

namespace Train
{
    public class LecturerAppService : FRMSCoreAppServiceBase, ILecturerAppService
    { 
        private readonly IRepository<Lecturer, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;

        public LecturerAppService(IRepository<Lecturer, Guid> repository
		, IAbpFileRelationAppService abpFileRelationAppService
        )
        {
            this._repository = repository;

            _abpFileRelationAppService = abpFileRelationAppService;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<LecturerListOutputDto>> GetList(GetLecturerListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
                        select new LecturerListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            TeachSubsidy = a.TeachSubsidy,
                            Tel = a.Tel,
                            Email = a.Email,
                            BankId = a.BankId,
                            Bank = a.Bank,
                            OpenBank = a.OpenBank,
                            Introduction = a.Introduction,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<LecturerListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<LecturerOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var result= model.MapTo<LecturerOutputDto>();

		    result.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.Id.ToString(), BusinessType = (int)AbpFileBusinessType.培训讲师合同 });
            return result;
		}
		/// <summary>
        /// 添加一个Lecturer
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public async Task Create(CreateLecturerInput input)
        {
                var newmodel = new Lecturer()
                {
                    Name = input.Name,
                    TeachSubsidy = input.TeachSubsidy,
                    Tel = input.Tel,
                    Email = input.Email,
                    BankId = input.BankId,
                    Bank = input.Bank,
                    OpenBank = input.OpenBank,
                    Introduction = input.Introduction
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
                    BusinessId = newmodel.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.培训讲师合同,
                    Files = fileList
                });
            }
				
        }

		/// <summary>
        /// 修改一个Lecturer
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateLecturerInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
               }
			   
			   dbmodel.Name = input.Name;
			   dbmodel.TeachSubsidy = input.TeachSubsidy;
			   dbmodel.Tel = input.Tel;
			   dbmodel.Email = input.Email;
			   dbmodel.BankId = input.BankId;
			   dbmodel.Bank = input.Bank;
			   dbmodel.OpenBank = input.OpenBank;
			   dbmodel.Introduction = input.Introduction;

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
                    BusinessType = (int)AbpFileBusinessType.提案附件,
                    Files = fileList
                });
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
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