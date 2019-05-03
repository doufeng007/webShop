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
    /// <summary>
    /// 云仓
    /// </summary>
    public class B_CloudWarehouseAppService : FRMSCoreAppServiceBase, IB_CloudWarehouseAppService
    {
        private readonly IRepository<B_Categroy, Guid> _b_CategroyRepository;
        private readonly IRepository<B_CWUserInventory, Guid> _b_CWUserInventoryRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;

        public B_CloudWarehouseAppService(IRepository<B_Categroy, Guid> b_CategroyRepository, IRepository<B_CWUserInventory, Guid> b_CWUserInventoryRepository
            , IAbpFileRelationAppService abpFileRelationAppService)
        {
            _b_CategroyRepository = b_CategroyRepository;
            _b_CWUserInventoryRepository = b_CWUserInventoryRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
        }

        /// <summary>
        ///  获取云仓进出明显列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<List<B_CWInOutDetailListOutputDto>> GetCWInOutDetailListAsync(GetB_CWInOutDetailListInput input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取云仓库存列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<List<B_CWInventoryListOutputDto>> GetCWInventoryListAsync(GetB_CWInventoryListInput input)
        {
            var query = from a in _b_CategroyRepository.GetAll()
                        join b in _b_CWUserInventoryRepository.GetAll() on a.Id equals b.CategroyId
                        where a.FirestLevelCategroyPropertyId == input.CategroyPropertyId && b.UserId == AbpSession.UserId.Value
                        select new B_CWInventoryListOutputDto
                        {
                            Id = a.Id,
                            Title = a.Name,
                            CanExtractCount = b.Count,
                            TakeLessCount = b.LessCount,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            var businessIds = ret.Select(r => r.Id.ToString()).ToList();
            if (businessIds.Count > 0)
            {
                var fileGroups = await _abpFileRelationAppService.GetMultiListAsync(new GetMultiAbpFilesInput()
                {
                    BusinessIds = businessIds,
                    BusinessType = AbpFileBusinessType.商品类别图
                });
                foreach (var item in ret)
                    if (fileGroups.Any(r => r.BusinessId == item.Id.ToString()))
                    {
                        var fileModel = fileGroups.FirstOrDefault(r => r.BusinessId == item.Id.ToString());
                        if (fileModel != null)
                        {
                            var files = fileModel.Files;
                            if (files.Count > 0)
                                item.File = files.FirstOrDefault();
                        }

                    }
            }
            return ret;
        }




    }
}
