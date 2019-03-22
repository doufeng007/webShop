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
using Abp.Authorization;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Model;

namespace HR
{
    [AbpAuthorize]
    public class EmployeeOtherInfoAppService : FRMSCoreAppServiceBase, IEmployeeOtherInfoAppService
    { 
        private readonly IRepository<EmployeeOtherInfo, Guid> _repository;

        public EmployeeOtherInfoAppService(IRepository<EmployeeOtherInfo, Guid> repository)
        {
            this._repository = repository;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<EmployeeOtherInfoListOutputDto>> GetList(GetEmployeeOtherInfoListInput input)
        {
			var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && x.UserId == input.UserId)
			             join b in base.UserManager.Users on a.UserId equals b.Id
                        select new EmployeeOtherInfoListOutputDto()
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            UserName = b.Name,
                            Remark = a.Remark,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultDto<EmployeeOtherInfoListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task<EmployeeOtherInfoOutputDto> Get(NullableIdDto<Guid> input)
		{
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            return model.MapTo<EmployeeOtherInfoOutputDto>();
		}
		/// <summary>
        /// 添加一个EmployeeOtherInfo
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Create(CreateEmployeeOtherInfoInput input)
		{
		    if (string.IsNullOrEmpty(input.Remark))
		    {
		        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请填写内容。");
		    }
            var user = UserManager.Users.FirstOrDefault(x => x.Id == input.UserId);
            if (user == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该用户不存在。");
            }
            var newmodel = new EmployeeOtherInfo()
                {
                    UserId = input.UserId,
                    Remark = input.Remark
		        };
                await _repository.InsertAsync(newmodel);
        }

        /// <summary>
        /// 修改一个EmployeeOtherInfo
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(EmployeeOtherInfo input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                var user = UserManager.Users.FirstOrDefault(x => x.Id == input.UserId);
                if (user == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该用户不存在。");
                }
                dbmodel.UserId = input.UserId;
                dbmodel.Remark = input.Remark;

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