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
using System.Net;
using Abp.Authorization;
using HR.Enum;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Model;

namespace HR
{
    [AbpAuthorize]
    public class EmployeeTrainingSystemAppService : FRMSCoreAppServiceBase, IEmployeeTrainingSystemAppService
    { 
        private const string RoleName= "HR.Training.Operation";
        private readonly IRepository<EmployeeTrainingSystem, Guid> _repository;
        private readonly IRepository<EmployeeTrainingSystemUnitPosts, Guid> _postsRepository;
        private readonly IRepository<UserPosts, Guid> _userPostRepository;

        public EmployeeTrainingSystemAppService(IRepository<EmployeeTrainingSystem, Guid> repository,
            IRepository<EmployeeTrainingSystemUnitPosts, Guid> postsRepository, IRepository<UserPosts, Guid> userPostRepository)
        {
            this._repository = repository;
            _postsRepository = postsRepository;
            _userPostRepository = userPostRepository;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<EmployeeTrainingSystemListOutputDto>> GetList(GetEmployeeTrainingSystemListInput input)
	    {
	        var user =await base.GetCurrentUserAsync();
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && x.Type == input.Type)
                select new EmployeeTrainingSystemListOutputDto()
                {
                    Id = a.Id,
                    Title = a.Title,
                    CreationTime = a.CreationTime
                };
	        if (input.Type == TrainingSystemType.Post && input.ShowAllTrain == 1)
            {
                query = (from a in query
                    join b in _postsRepository.GetAll().Where(x => !x.IsDeleted) on a.Id equals b.SysId
                    join c in _userPostRepository.GetAll().Where(x => !x.IsDeleted && x.UserId == user.Id) on b.PortsId
                        equals c.OrgPostId
                    select new EmployeeTrainingSystemListOutputDto()
                    {
                        Id = a.Id,
                        Title = a.Title,
                        CreationTime = a.CreationTime
                    });
            }
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).ToListAsync();
            return new PagedResultDto<EmployeeTrainingSystemListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task<EmployeeTrainingSystemOutputDto> Get(NullableIdDto<Guid> input)
		{
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            return model.MapTo<EmployeeTrainingSystemOutputDto>();
		}

        /// <summary>
        /// 添加一个EmployeeTrainingSystem
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        [AbpAuthorize(RoleName)]
        public async Task Create(CreateEmployeeTrainingSystemInput input)
        {
            if ((input.PortsIds == null || !input.PortsIds.Any()) && input.Type == TrainingSystemType.Post)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请至少选择一个部门岗位!");
            }
            var newmodel = new EmployeeTrainingSystem()
            {
                Title = input.Title,
                Contents = input.Contents,
                Type = input.Type
            };

            await _repository.InsertAsync(newmodel);
            if (input.PortsIds != null && input.PortsIds.Any())
            {
                input.PortsIds.ForEach(async x =>
                {
                    await _postsRepository.InsertAsync(new EmployeeTrainingSystemUnitPosts()
                    {
                        SysId = newmodel.Id,
                        PortsId = x
                    });
                });
            }
        }

        /// <summary>
        /// 修改一个EmployeeTrainingSystem
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        [AbpAuthorize(RoleName)]
        public async Task Update(EmployeeTrainingSystem input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                dbmodel.Title = input.Title;
                dbmodel.Contents = input.Contents;
                await _repository.UpdateAsync(dbmodel);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        [AbpAuthorize(RoleName)]
        public async Task Delete(EntityDto<Guid> input)
        {
            
            await _repository.DeleteAsync(x=>x.Id == input.Id);
        }
    }
}