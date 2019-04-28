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
using Abp.Authorization;

namespace B_H5
{
    public class B_AgencyDisableRecordAppService : FRMSCoreAppServiceBase, IB_AgencyDisableRecordAppService
    {
        private readonly IRepository<B_AgencyDisableRecord, Guid> _repository;
        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;

        public B_AgencyDisableRecordAppService(IRepository<B_AgencyDisableRecord, Guid> repository, IRepository<B_Agency, Guid> b_AgencyRepository

        )
        {
            this._repository = repository;
            _b_AgencyRepository = b_AgencyRepository;

        }

        /// <summary>
        /// 封号记录
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_AgencyDisableRecordListOutputDto>> GetList(GetB_AgencyDisableRecordListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join u in UserManager.Users on a.CreatorUserId equals u.Id
                        where a.AgencyId == input.AgencyId
                        select new B_AgencyDisableRecordListOutputDto()
                        {
                            Id = a.Id,
                            AgencyId = a.AgencyId,
                            Reason = a.Reason,
                            Remark = a.Remark,
                            CreationTime = a.CreationTime,
                            CreateionUserName = u.Name
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_AgencyDisableRecordListOutputDto>(toalCount, ret);
        }


        /// <summary>
        /// 封号
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task Create(CreateB_AgencyDisableRecordInput input)
        {
            var newmodel = new B_AgencyDisableRecord()
            {
                AgencyId = input.AgencyId,
                Reason = input.Reason,
                Remark = input.Remark
            };

            await _repository.InsertAsync(newmodel);

            var b_Model = await _b_AgencyRepository.FirstOrDefaultAsync(r => r.Id == input.AgencyId);
            if (b_Model == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该代理不存在！");
            b_Model.Status = B_AgencyAcountStatusEnum.封号;
            await _b_AgencyRepository.UpdateAsync(b_Model);

        }

        
    }
}