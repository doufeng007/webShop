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
using ServiceReference;
using Abp;
using B_H5.Service.CloudService.Dto;

namespace B_H5
{
    public class B_GoodsAppService : FRMSCoreAppServiceBase, IB_GoodsAppService
    {
        private readonly IRepository<B_Goods, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<B_Categroy, Guid> _b_CategroyRepository;
        public APIWebServiceSoapClient SoapClient { get; set; }

        public B_GoodsAppService(IRepository<B_Goods, Guid> repository, IAbpFileRelationAppService abpFileRelationAppService, IRepository<B_Categroy, Guid> b_CategroyRepository

        )
        {
            this._repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _b_CategroyRepository = b_CategroyRepository;
            SoapClient = new ServiceReference.APIWebServiceSoapClient(ServiceReference.APIWebServiceSoapClient.EndpointConfiguration.APIWebServiceSoap);

        }

        /// <summary>
        /// 后台-商品管理
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_GoodsListOutputDto>> GetList(GetB_GoodsListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_CategroyRepository.GetAll() on a.CategroyIdP equals b.Id
                        join c1 in _b_CategroyRepository.GetAll() on a.CategroyId equals c1.Id into g
                        from c in g.DefaultIfEmpty()

                        select new B_GoodsListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            CategroyIdName = c.Name,
                            CategroyIdPName = b.Name,
                            Code = a.Code,
                            ModeType = a.ModeType,
                            Spe = a.Spe,
                            UnitName = a.UnitName,
                            Price = a.Price,
                            Pirce1 = a.Pirce1,
                            CreationTime = a.CreationTime,
                            Status = a.Status,
                            CategroyIdP = a.CategroyIdP,
                            CategroyId = a.CategroyId

                        };

            query = query.WhereIf(input.CategroyIdP.HasValue, r => r.CategroyIdP == input.CategroyIdP.Value).WhereIf(input.CategroyId.HasValue, r => r.CategroyId == input.CategroyId)
                .WhereIf(!input.SearchKey.IsNullOrEmpty(), r => r.Code.Contains(input.SearchKey) || r.Name.Contains(input.SearchKey));
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            var businessIds = ret.Select(r => r.Id.ToString()).ToList();
            if (businessIds.Count > 0)
            {
                var fileGroups = await _abpFileRelationAppService.GetMultiListAsync(new GetMultiAbpFilesInput()
                {
                    BusinessIds = businessIds,
                    BusinessType = AbpFileBusinessType.商品缩略图
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


            return new PagedResultDto<B_GoodsListOutputDto>(toalCount, ret);
        }


        /// <summary>
        /// 后台-商品库存管理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_GoodsInventoryListOutputDto>> GetInventoryList(GetB_GoodsInventoryListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_CategroyRepository.GetAll() on a.CategroyIdP equals b.Id
                        join c1 in _b_CategroyRepository.GetAll() on a.CategroyId equals c1.Id into g
                        from c in g.DefaultIfEmpty()

                        select new B_GoodsInventoryListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            CategroyIdName = c.Name,
                            CategroyIdPName = b.Name,
                            Code = a.Code,
                            ModeType = a.ModeType,
                            Spe = a.Spe,
                            UnitName = a.UnitName,
                            CreationTime = a.CreationTime,
                            CategroyIdP = a.CategroyIdP,
                            CategroyId = a.CategroyId,
                            Inventory = a.Inventory,
                        };

            query = query.WhereIf(input.CategroyIdP.HasValue, r => r.CategroyIdP == input.CategroyIdP.Value).WhereIf(input.CategroyId.HasValue, r => r.CategroyId == input.CategroyId)
                .WhereIf(!input.SearchKey.IsNullOrEmpty(), r => r.Code.Contains(input.SearchKey) || r.Name.Contains(input.SearchKey));
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();




            return new PagedResultDto<B_GoodsInventoryListOutputDto>(toalCount, ret);
        }


        /// <summary>
        /// 根据商品类别获取商品列表----云仓提货 商品类别
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_GoodsListOutputDto>> GetListByCategroyId(GetB_GoodsListByCategroyIdInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        where a.CategroyId == input.CategroyId
                        select new B_GoodsListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            CategroyId = a.CategroyId,
                            Price = a.Price,
                            Pirce1 = a.Pirce1,

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
                    BusinessType = AbpFileBusinessType.商品缩略图
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

            return new PagedResultDto<B_GoodsListOutputDto>(toalCount, ret);
        }




        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_GoodsOutputDto> Get(EntityDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var ret = model.MapTo<B_GoodsOutputDto>();
            var files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = input.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.商品缩略图
            });

            if (files.Count() > 0)
                ret.File = files.FirstOrDefault();


            return ret;
        }



        /// <summary>
        /// 后台-新增商品
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Create(CreateB_GoodsInput input)
        {
            var newmodel = new B_Goods()
            {
                Id = Guid.NewGuid(),
                Code = DateTime.Now.DateTimeToStamp().ToString(),
                ModeType = input.ModeType,
                Spe = input.Spe,
                UnitName = input.UnitName,
                UnitId = input.UnitId,
                Name = input.Name,
                CategroyId = input.CategroyId,
                CategroyIdP = input.CategroyIdP,
                Price = input.Price,
                Pirce1 = input.Pirce1,
                Price2 = input.Price2
            };

            await _repository.InsertAsync(newmodel);

            var fileList3 = new List<AbpFileListInput>();

            fileList3.Add(new AbpFileListInput() { Id = input.File.Id, Sort = input.File.Sort });

            await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
            {
                BusinessId = newmodel.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.商品缩略图,
                Files = fileList3
            });


            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<CloudService>();
            var goods = new Service.CloudService.Dto.GFF_GoodsItem()
            {
                CnName = newmodel.Name,
                CustomcCode = "",
                GoodsCode = newmodel.Code,
                Price = newmodel.Price.ToString(),
                SKU = newmodel.Code,

            };
            var goodsList = new List<GFF_GoodsItem>();
            goodsList.Add(goods);
            await service.CreateGoods(new Service.CloudService.Dto.CreateGoodsInput()
            {
                GFF_Goods = new Service.CloudService.Dto.GFF_Goods()
                {
                    item = goodsList
                }
            });

        }



        /// <summary>
        /// 后台-商品修改
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_GoodsInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                dbmodel.Name = input.Name;
                dbmodel.CategroyId = input.CategroyId;
                dbmodel.CategroyIdP = input.CategroyIdP;
                dbmodel.Price = input.Price;
                dbmodel.Pirce1 = input.Pirce1;
                dbmodel.Price2 = input.Price2;
                dbmodel.Code = input.Code;
                dbmodel.Spe = input.Spe;
                dbmodel.UnitId = input.UnitId;
                dbmodel.UnitName = input.UnitName;
                dbmodel.ModeType = input.ModeType;

                await _repository.UpdateAsync(dbmodel);


                var fileList3 = new List<AbpFileListInput>();

                fileList3.Add(new AbpFileListInput() { Id = input.File.Id, Sort = input.File.Sort });

                await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = dbmodel.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.商品缩略图,
                    Files = fileList3
                });
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }

        /// <summary>
        /// 后台-商品删除
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }


        /// <summary>
        /// 后台上架下架
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Enable(EntityDto<Guid> input)
        {
            var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (dbmodel == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }

            if (dbmodel.Status == GoodStatusEnum.上架)
                dbmodel.Status = GoodStatusEnum.下架;
            else
                dbmodel.Status = GoodStatusEnum.上架;

            await _repository.UpdateAsync(dbmodel);
        }
    }
}