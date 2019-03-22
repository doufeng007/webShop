using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using System.Dynamic;

namespace HR
{
    public interface IHrSystemAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<HrSystemListOutputDto>> GetList(GetHrSystemListInput input);
        List<ExpandoObject> GetHrSystemType(string value = null, string setEmpty = null);
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<HrSystemOutputDto> Get(NullableIdDto<Guid> input);
        Task<PagedResultDto<HrSystemListOutputDto>> GetListByMe(GetHrSystemListInput input);
        /// <summary>
        /// 添加一个HrSystem
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Create(CreateHrSystemInput input);

		/// <summary>
        /// 修改一个HrSystem
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateHrSystemInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
    }
}