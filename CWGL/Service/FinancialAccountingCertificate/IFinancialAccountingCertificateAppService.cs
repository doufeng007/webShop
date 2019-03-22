using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace CWGL
{
    public interface IFinancialAccountingCertificateAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<FinancialAccountingCertificateListOutputDto>> GetList(GetFinancialAccountingCertificateListInput input);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<FinancialAccountingCertificateOutputDto> Get(NullableIdDto<Guid> input);

        ///// <summary>
        //      /// 添加一个FinancialAccountingCertificate
        //      /// </summary>
        //      /// <param name="input">实体</param>
        //      /// <returns></returns>
        //Task<FinancialAccountingCertificateOutputDto> Create(CreateFinancialAccountingCertificateInput input);

        ///// <summary>
        //      /// 修改一个FinancialAccountingCertificate
        //      /// </summary>
        //      /// <param name="input">实体</param>
        //      /// <returns></returns>
        //Task Update(UpdateFinancialAccountingCertificateInput input);


        void CreateOrUpdate(CreateOrUpdateFinancialAccountingCertificateInput input);


        Task UpdateAccounting(NullableIdDto<Guid> input);

        void CreateOrUpdateWithOutNLP(CreateOrUpdateFinancialAccountingCertificateInput input, bool isUpdateForChange = false, Guid? flowId = null);

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);



        /// <summary>
        /// 根据业务id获取实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<FinancialAccountingCertificateOutputDto> GetByBusinessId(GetByBusinessIdInput input);
    }
}