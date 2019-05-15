using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace B_H5
{
    public interface IB_OrderAppService : IApplicationService
    {

        Task<PagedResultDto<GetUserBlanceListOutput>> GetBlanceList(GetB_OrderListInput input);


        Task<PagedResultDto<GetUserGoodPaymentListOutput>> GetGoodPaymentList(GetB_OrderListInput input);



        Task<PagedResultDto<AgencyMoneyStaticDto>> GetAgencyMoneyStatic(GetAgencyMoneyStaticInput input);


        Task<PagedResultDto<AgencyMoneyDetailListDto>> GetAgencyMoneyDetailList(GetAgencyMoneyDetailListInput input);


        Task<UserBlanceListDto> Get();


        /// <summary>
        /// 添加一个B_Order
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task CreateAsync(CreateB_OrderInput input);


        void Create(CreateB_OrderInput input);

        /// <summary>
        /// 修改一个B_Order
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdateB_OrderInput input);

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);
    }
}