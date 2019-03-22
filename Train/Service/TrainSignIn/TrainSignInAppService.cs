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
using Abp.Auditing;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Authorization.Users;

namespace Train
{
    public class TrainSignInAppService : FRMSCoreAppServiceBase, ITrainSignInAppService
    { 
        private readonly IRepository<TrainSignIn, Guid> _repository;
        public IClientInfoProvider ClientInfoProvider { get; set; }
        private readonly IRepository<Train, Guid> _trainRepository;
        private readonly IRepository<User, long> _userRepository;
        public TrainSignInAppService(IRepository<TrainSignIn, Guid> repository, IRepository<Train, Guid> trainRepository, IRepository<User, long> userRepository)
        {
            this._repository = repository;
            this._trainRepository = trainRepository;
            ClientInfoProvider = NullClientInfoProvider.Instance;
            _userRepository = userRepository; 
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<TrainSignInListOutputDto>> GetList(GetTrainSignInListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted && x.UserId==input.UserId&& x.TrainId==input.TrainId)
                        join b in _userRepository.GetAll() on a.UserId equals b.Id
                        select new TrainSignInListOutputDto()
                        {
                            Id = a.Id,
                            TrainId = a.TrainId,
                            UserId = a.UserId,
                            UserName =b.Name,
                            SignInTime = a.SignInTime,
                            SignOutTime = a.SignOutTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.SignInTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<TrainSignInListOutputDto>(toalCount, ret);
        }
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<TrainSignInListOutputDto>> GetListByTime(GetTrainSignInListByTimeInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted && x.TrainId==input.TrainId)
                        join b in _userRepository.GetAll() on a.UserId equals b.Id
                        select new TrainSignInListOutputDto()
                        {
                            Id = a.Id,
                            TrainId = a.TrainId,
                            UserId = a.UserId,
                            UserName = b.Name,
                            SignInTime = a.SignInTime,
                            SignOutTime = a.SignOutTime
                        };
            if (input.SignInTime.HasValue) {
                query = query.Where(x => x.SignInTime.Date == input.SignInTime.Value.Date);
            }
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.SignInTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<TrainSignInListOutputDto>(toalCount, ret);
        }
		/// <summary>
        /// 添加一个TrainSignIn
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>		
		public async Task<TrainSignInOutputDto> SignIn(CreateTrainSignInInput input)
        {
            if (!_trainRepository.GetAll().Any(x=>x.Id==input.TrainId))
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "培训数据不存在。");
            if (!_userRepository.GetAll().Any(x=>x.Id==input.UserId))
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "用户不存在。");
            var train = _trainRepository.GetAll().First(x=>x.Id==input.TrainId);
            if (train.StartTime > DateTime.Now)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "还未开始培训。");
            if (train.EndTime < DateTime.Now)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "培训时间已过。");

            var siginDate = DateTime.Now;
            var date = siginDate.Date;
            int state = 0;
            if (_repository.GetAll().Any(x => x.TrainId == input.TrainId && x.UserId == input.UserId && x.SignInTime.Date==date))
            {
                var info = _repository.GetAll().First(x => x.TrainId == input.TrainId && x.UserId == input.UserId && x.SignInTime.Date == date);
                if (info.SignOutTime == null)
                {
                    info.SignOutTime = siginDate;
                    info.SignOutIp = ClientInfoProvider.ClientIpAddress;
                    _repository.Update(info);
                    state = 1;
                }
                else 
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "今天已经签过到了。");
            }
            else {
                var model = new TrainSignIn();
                model.Id = Guid.NewGuid();
                model.TrainId = input.TrainId;
                model.UserId = input.UserId;
                model.SignInTime = siginDate;
                model.SignInIp = ClientInfoProvider.ClientIpAddress;
                _repository.Insert(model);
            }
            return new TrainSignInOutputDto() { TrainTitle=train.Title,SignState=state, SignDate = siginDate, UserName= _userRepository.GetAll().First(x => x.Id == input.UserId).Name };
        }	
    }
}