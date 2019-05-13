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

namespace B_H5
{
    public class B_CategroyAppService : FRMSCoreAppServiceBase, IB_CategroyAppService
    {
        private readonly IRepository<B_Categroy, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        public B_CategroyAppService(IRepository<B_Categroy, Guid> repository, IAbpFileRelationAppService abpFileRelationAppService

        )
        {
            this._repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;

        }

        /// <summary>
        /// H5 获取我的云仓-进货 可进货类别列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_CategroyListOutputDto>> GetCWCategroyList(GetB_CategroyListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        where a.FirestLevelCategroyPropertyId == FirestLevelCategroyProperty.进提货 && !a.P_Id.HasValue
                        select new B_CategroyListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Price = a.Price,
                            Unit = a.Unit,
                            Tag = a.Tag,
                            Status = a.Status,
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

            return new PagedResultDto<B_CategroyListOutputDto>(toalCount, ret);
        }


        /// <summary>
        /// 获取商品类别列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_CategroyListOutputDto>> GetList(GetB_CategroyListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)

                        select new B_CategroyListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            P_Id = a.P_Id,
                            Price = a.Price,
                            Unit = a.Unit,
                            Tag = a.Tag,
                            Remark = a.Remark,
                            Status = a.Status,
                            CreationTime = a.CreationTime

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_CategroyListOutputDto>(toalCount, ret);
        }



        /// <summary>
        /// 根据类别id获取下级类别------云仓提货
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_CategroyListUnderCategroyIdDto>> GetListByCategroyId(GetB_CategroyListByCategroyIdInput input)
        {
            var query = from a in _repository.GetAll()
                        where a.P_Id == input.CategroyId
                        select new B_CategroyListUnderCategroyIdDto
                        {
                            Id = a.Id,
                            Name = a.Name
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.Name).PageBy(input).ToListAsync();
            return new PagedResultDto<B_CategroyListUnderCategroyIdDto>(toalCount, ret);

        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_CategroyOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var ret = model.MapTo<B_CategroyOutputDto>();


            var fielRet = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.商品类别图
            });
            if (fielRet.Count() > 0)
                ret.File = fielRet.FirstOrDefault();

            return ret;
        }
        /// <summary>
        /// 添加一个商品类别
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Create(CreateB_CategroyInput input)
        {
            if (!input.P_Id.HasValue)
            {
                //if (!(int)input.FirestLevelCategroyPropertyId <= 0)
                //    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "一级商品类别必须设置属性值！");
            }
            var newmodel = new B_Categroy()
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                P_Id = input.P_Id,
                Price = input.Price,
                Unit = input.Unit,
                Tag = input.Tag,
                Remark = input.Remark,
                Status = input.Status,
                FirestLevelCategroyPropertyId = input.FirestLevelCategroyPropertyId,
            };


            await _repository.InsertAsync(newmodel);

            var fileList3 = new List<AbpFileListInput>();

            fileList3.Add(new AbpFileListInput() { Id = input.File.Id, Sort = input.File.Sort });

            await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
            {
                BusinessId = newmodel.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.商品类别图,
                Files = fileList3
            });

        }

        /// <summary>
        /// 修改一个B_Categroy
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_CategroyInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                dbmodel.Name = input.Name;
                dbmodel.P_Id = input.P_Id;
                dbmodel.Price = input.Price;
                dbmodel.Unit = input.Unit;
                dbmodel.Tag = input.Tag;
                dbmodel.Remark = input.Remark;
                dbmodel.Status = input.Status;
                //dbmodel.FirestLevelCategroyPropertyId = input.FirestLevelCategroyPropertyId;

                await _repository.UpdateAsync(dbmodel);

                var fileList = new List<AbpFileListInput>();

                fileList.Add(new AbpFileListInput() { Id = input.File.Id, Sort = input.File.Sort });

                await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = dbmodel.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.商品类别图,
                    Files = fileList
                });

            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }
    }
}