using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace HR
{
    public interface IQuestionLibraryAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<QuestionLibraryListOutputDto>> GetList(GetQuestionLibraryListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<QuestionLibraryOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个QuestionLibrary
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateQuestionLibraryInput input);

		/// <summary>
        /// 修改一个QuestionLibrary
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateQuestionLibraryInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
    }
}