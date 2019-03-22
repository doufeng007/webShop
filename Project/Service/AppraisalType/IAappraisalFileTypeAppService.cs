using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IAappraisalFileTypeAppService : IApplicationService
    {

        //ListResultDto<AappraisalFileTypeListDto> GetAappraisalFileTypes();
        Task<PagedResultDto<AappraisalFileTypeListDto>> GetAappraisalFileTypes(GetAappraisalFileTypeListInput input);

        ListResultDto<AappraisalFileTypeListDto> GetAllAappraisalFileTypes(GetAappraisalFileTypeListInput input);


        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetAappraisalFileTypeForEditOutput> GetAappraisalFileTypeForEdit(NullableIdDto<int> input);


        /// <summary>
        /// 新增或更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAappraisalFileType(CreateOrUpdateAappraisalFileTypeInput input);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAappraisalFileType(EntityDto<int> input);


        Task<int> CreateOrUpdate(CreateOrUpdateAappraisalFileTypeInput input);

    }

}
