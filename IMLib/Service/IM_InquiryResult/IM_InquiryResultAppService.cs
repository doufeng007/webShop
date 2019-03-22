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
using ZCYX.FRMSCore.Model;

namespace IMLib
{
    public class IM_InquiryResultAppService : FRMSCoreAppServiceBase, IIM_InquiryResultAppService
    { 
        private readonly IRepository<IM_InquiryResult,Guid > _repository;
		
        public IM_InquiryResultAppService(IRepository<IM_InquiryResult, Guid> repository
		
		)
        {
            this._repository = repository;
			
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<IM_InquiryResultListOutputDto>> GetList(GetIM_InquiryResultListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
                        select new IM_InquiryResultListOutputDto()
                        {
                            Id = a.Id,
                            IM_GroupId = a.IM_GroupId,
                            InquiryId = a.InquiryId,
                            ReplyUserId = a.ReplyUserId.Value,
                            ReplyContent = a.ReplyContent,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<IM_InquiryResultListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<IM_InquiryResultOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            }
            return model.MapTo<IM_InquiryResultOutputDto>();
		}
		/// <summary>
        /// 添加一个IM_InquiryResult
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public async Task Create(CreateIM_InquiryResultInput input)
        {
                var newmodel = new IM_InquiryResult()
                {
                    Id = input.Id,
                    IM_GroupId = input.IM_GroupId,
                    InquiryId = input.InquiryId,
                    ReplyUserId = input.ReplyUserId,
                    ReplyContent = input.ReplyContent
		        };
				
                await _repository.InsertAsync(newmodel);
				
        }

		/// <summary>
        /// 修改一个IM_InquiryResult
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateIM_InquiryResultInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
               }
			   
			   dbmodel.Id = input.Id;
			   dbmodel.IM_GroupId = input.IM_GroupId;
			   dbmodel.InquiryId = input.InquiryId;
			   dbmodel.ReplyUserId = input.ReplyUserId;
			   dbmodel.ReplyContent = input.ReplyContent;

               await _repository.UpdateAsync(dbmodel);
			   
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
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