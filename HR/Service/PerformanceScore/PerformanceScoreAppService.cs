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

namespace HR
{
    public class PerformanceScoreAppService : FRMSCoreAppServiceBase, IPerformanceScoreAppService
    { 
        private readonly IRepository<PerformanceScore, Guid> _repository;
        private readonly IRepository<PerformanceScoreType, int> _typeRepository;
		
        public PerformanceScoreAppService(IRepository<PerformanceScore, Guid> repository, IRepository<PerformanceScoreType, int> typeRepository)
        {
            this._repository = repository;
            _typeRepository = typeRepository;
        }
		
	    /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<List<PerformanceScoreTypeListOutput>> GetList()
        {
            var queryBase = _typeRepository.GetAll().OrderBy(x=>x.Sort).ToList();
            var q =
                from a in queryBase.Where(x => x.ParentId == null)
                select new PerformanceScoreTypeListOutput
                {
                    Id = a.Id,
                    Title = a.Title,
                    IsEdit = a.IsEdit,
                    Nodes = (from b in queryBase.Where(x => x.ParentId == a.Id)
                             select new PerformanceScoreTypeListOutput()
                             {
                                 Id = b.Id,
                                 Title = b.Title,
                                 IsEdit = b.IsEdit,
                                 Score = (from c in _repository.GetAll().Where(x => !x.IsDeleted && x.ScoreTypeId == b.Id)
                                          select new PerformanceScoreListOutputDto()
                                          {
                                              Id = c.Id,
                                              Title = c.Title,
                                              Than1 = c.Than1,
                                              Than21 = c.Than21,
                                              Than22 = c.Than22,
                                              Than3 = c.Than3,
                                              Than1Score = c.Than1Score,
                                              Than2Score = c.Than2Score,
                                              Than3Score = c.Than3Score,
                                              ScoreTypeId = c.ScoreTypeId,
                                              Unit = c.Unit.ToString()== "百分比"?"%":c.Unit.ToString(),
                                              ScoreType = c.ScoreType,
                                              CreationTime = c.CreationTime
                                          }).ToList()
                             }).ToList()
                };

            return q.ToList();
        }
	 

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<PerformanceScoreOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<PerformanceScoreOutputDto>();
		}
	

		/// <summary>
        /// 修改一个PerformanceScore
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(List<UpdatePerformanceScoreInput> input)
        {
		    if (input.Count>0)
            {
                foreach (var item in input)
                {
                    var model = await _repository.FirstOrDefaultAsync(x => x.Id == item.Id);
                    if (model == null)
                    {
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                    }
                    model.Than1 = item.Than1;
                    model.Than21 = item.Than21;
                    model.Than22 = item.Than22;
                    model.Than3 = item.Than3;
                    model.Than1Score = item.Than1Score;
                    model.Than2Score = item.Than2Score;
                    model.Than3Score = item.Than3Score;
                    await _repository.UpdateAsync(model);
                }
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }
		
    }
}