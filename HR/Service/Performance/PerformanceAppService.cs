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
    public class PerformanceAppService : FRMSCoreAppServiceBase, IPerformanceAppService
    { 
        private readonly IRepository<Performance, Guid> _repository;
        private readonly IRepository<PerformanceScoreType, int> _scoreTypeRepository;
		
        public PerformanceAppService(IRepository<Performance, Guid> repository, IRepository<PerformanceScoreType, int> scoreTypeRepository)
        {
            this._repository = repository;
            _scoreTypeRepository = scoreTypeRepository;
        }

        

        /// <summary>
        /// 个人数字化绩效考核获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<List<PerformanceDataListOutputDto>> GetDataList(GetPerformanceListInput input)
        {
            var list = from a in _scoreTypeRepository.GetAll().Where(x => x.ParentId != null)
                       join b in _scoreTypeRepository.GetAll() on a.ParentId equals b.Id
                       let c = from d in _repository.GetAll().Where(x => !x.IsDeleted && x.Type == PerformanceType.数字化绩效考核 && x.ScoreTypeId == a.Id ).WhereIf(input.Time.HasValue,x => x.CreationTime.Year == input.Time.Value.Year && x.CreationTime.Month == input.Time.Value.Month) 
                             .Where(x=>(input.GetMy && x.UserId==AbpSession.UserId.Value) || (!input.GetMy && x.UserId ==input.UserId.Value))
                               select new PerformanceListOutputDto()
                               {
                                   Id = d.Id,
                                   UserId = d.UserId,
                                   Matter = d.Matter,
                                   Record = d.Record,
                                   Score = d.Score,
                                   Type = d.Type,
                                   CreationTime = d.CreationTime
                               }
                       select new PerformanceDataListOutputDto
                       {
                           Title = b.Title,
                           Name = a.Title,
                           Nodes = c.ToList()
                       };

            return list.ToList();
        }
        /// <summary>
        /// 个人非数字化绩效考核获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<PerformanceListOutputDto>> GetNoDataList(GetPerformanceListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted && x.Type==PerformanceType.非数字化绩效考核 )
                        .WhereIf(input.Time.HasValue, x => x.CreationTime.Year == input.Time.Value.Year && x.CreationTime.Month == input.Time.Value.Month)
                             .Where(x => (input.GetMy && x.UserId == AbpSession.UserId.Value) || (!input.GetMy && x.UserId == input.UserId.Value))
                        select new PerformanceListOutputDto()
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            Matter = a.Matter,
                            Record = a.Record,
                            Score = a.Score,
                            Type = a.Type,
                            CreationTime = a.CreationTime							
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<PerformanceListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<PerformanceOutputDto> Get(NullableIdDto<Guid> input)
		{
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<PerformanceOutputDto>();
		}
        /// <summary>
        /// 添加一个Performance
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreatePerformanceInput input)
        {
            var newmodel = new Performance()
            {
                UserId = input.UserId,
                Matter = input.Matter,
                Record = input.Record,
                Score = input.Score,
                Type = input.Type
            };
            await _repository.InsertAsync(newmodel);

        }
    }
}