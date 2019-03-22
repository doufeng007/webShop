using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.File;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Dto;

namespace Supply
{
    public interface IUserSupplyAppService : IApplicationService
    {


        Task<UserSupplyDto> Get(GetWorkFlowTaskCommentInput input);

        /// <summary>
        /// 个人用品列表获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InitMenuPagedResultDto<UserSupplyListDto>> GetAll(GetUserSupplyListInput input);



        Task<AbpFileUploadResultModel> ExportExcle();


        /// <summary>
        /// 新增或编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateMenuInitInput<List<UserSupplyBatchCreateInput>> input);
        /// <summary>
        /// 删除用品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
         Task Delete(Guid input);
    }

}
