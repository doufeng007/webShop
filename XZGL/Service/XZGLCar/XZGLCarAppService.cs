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

namespace XZGL
{
    public class XZGLCarAppService : FRMSCoreAppServiceBase, IXZGLCarAppService
    { 
        private readonly IRepository<XZGLCar, Guid> _repository;
        private readonly IRepository<XZGLCarBorrow, Guid> _xZGLCarBorrowRepository;
        private readonly IRepository<XZGLCarInfo, Guid> _xZGLCarInfoRepository;
		
        public XZGLCarAppService(IRepository<XZGLCar, Guid> repository, IRepository<XZGLCarBorrow, Guid> xZGLCarBorrowRepository, IRepository<XZGLCarInfo, Guid> xZGLCarInfoRepository)
        {
            this._repository = repository;
            _xZGLCarBorrowRepository = xZGLCarBorrowRepository;
            _xZGLCarInfoRepository = xZGLCarInfoRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<List<XZGLCarListOutputDto>> GetByBorrowList(Guid id)
        {
            var model = _xZGLCarBorrowRepository.Get(id);
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && x.IsEnable)
                        let c = (from b in _xZGLCarInfoRepository.GetAll()
                                 join c in _xZGLCarBorrowRepository.GetAll() on b.CarBorrowId equals c.Id
                                 where b.CarId == a.Id && ((c.StartTime <= model.StartTime && c.EndTime >= model.StartTime) || (c.StartTime <= model.EndTime && c.EndTime >= model.EndTime))
                                 select b).Count() > 0
                        select new XZGLCarListOutputDto()
                        {
                            Id = a.Id,
                            CarNum = a.CarNum,
                            CarType = a.CarType,
                            SeatNum = a.SeatNum,
                            Amount = a.Amount,
                            Status = c ? XZGLCarStatus.借用中 : (a.IsEnable ? XZGLCarStatus.可申请 : XZGLCarStatus.停用),
                        };
            var ret = query.ToList();
            foreach (var item in ret)
                item.StatusName = item.Status.ToString();
            return ret;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<XZGLCarListOutputDto>> GetList(GetXZGLCarListInput input)
        {
            var dt = DateTime.Now;
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted).WhereIf(input.IsEnable.HasValue,r=>r.IsEnable==input.IsEnable.Value)
						let c= (from b in _xZGLCarInfoRepository.GetAll() join c in _xZGLCarBorrowRepository.GetAll() on b.CarBorrowId equals c.Id
                               where b.CarId== a.Id  && c.StartTime<=dt &&c.EndTime>=dt  select b).Count()>0
                        select new XZGLCarListOutputDto()
                        {
                            Id = a.Id,
                            CarNum = a.CarNum,
                            CarType = a.CarType,
                            SeatNum = a.SeatNum,
                            CarColor = a.CarColor,
                            Amount = a.Amount,
                            Variable = a.Variable,
                            Remark = a.Remark,
                            Number = a.Number,
                            Type = a.Type,
                            UserName = a.UserName,
                            UserType = a.UserType,
                            Address = a.Address,
                            DrivingType = a.DrivingType,
                            DrivingNumber = a.DrivingNumber,
                            EngineNumber = a.EngineNumber,
                            RegisterTime = a.RegisterTime,
                            CertificationTime = a.CertificationTime,
                            DrivingRemark = a.DrivingRemark,
                            IsEnable = a.IsEnable,
                            Status=c? XZGLCarStatus.借用中:(a.IsEnable? XZGLCarStatus.可申请: XZGLCarStatus.停用),
                            CreationTime = a.CreationTime
                        };
            if (!string.IsNullOrEmpty(input.SearchKey))
                query = query.Where(x => x.CarNum.Contains(input.SearchKey) || x.CarType.Contains(input.SearchKey) || x.SeatNum.Contains(input.SearchKey));
            if (input.Status.HasValue)
                query = query.Where(x => x.Status == input.Status.Value);
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
                item.StatusName = item.Status.ToString();
            return new PagedResultDto<XZGLCarListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<XZGLCarOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在。");
            }
            var ret= model.MapTo<XZGLCarOutputDto>();
            ret.VariableName = ret.Variable.ToString();
            return ret;
		}
		/// <summary>
        /// 添加一个XZGLCar
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public async Task Create(CreateXZGLCarInput input)
        {
            if (_repository.GetAll().Any(ite => ite.CarNum == input.CarNum))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "车牌号重复。");
            }
            var newmodel = new XZGLCar()
                {
                    CarNum = input.CarNum,
                    CarType = input.CarType,
                    SeatNum = input.SeatNum,
                    CarColor = input.CarColor,
                    Amount = input.Amount,
                    Variable = input.Variable,
                    Remark = input.Remark,
                    Number = input.Number,
                    Type = input.Type,
                    UserName = input.UserName,
                    UserType = input.UserType,
                    Address = input.Address,
                    DrivingType = input.DrivingType,
                    DrivingNumber = input.DrivingNumber,
                    EngineNumber = input.EngineNumber,
                    RegisterTime = input.RegisterTime,
                    CertificationTime = input.CertificationTime,
                    IsEnable = true,
                    DrivingRemark = input.DrivingRemark
		        };
				
                await _repository.InsertAsync(newmodel);
				
        }

		/// <summary>
        /// 修改一个XZGLCar
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateXZGLCarInput input)
        {
		    if (input.Id != Guid.Empty)
            {
                if (_repository.GetAll().Any(ite => ite.CarNum == input.CarNum && ite.Id != input.Id))
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "车牌号重复。");
                }
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在。");
               }
			   
			   dbmodel.CarNum = input.CarNum;
			   dbmodel.CarType = input.CarType;
			   dbmodel.SeatNum = input.SeatNum;
			   dbmodel.CarColor = input.CarColor;
			   dbmodel.Amount = input.Amount;
			   dbmodel.Variable = input.Variable;
			   dbmodel.Remark = input.Remark;
			   dbmodel.Number = input.Number;
			   dbmodel.Type = input.Type;
			   dbmodel.UserName = input.UserName;
			   dbmodel.UserType = input.UserType;
			   dbmodel.Address = input.Address;
			   dbmodel.DrivingType = input.DrivingType;
			   dbmodel.DrivingNumber = input.DrivingNumber;
			   dbmodel.EngineNumber = input.EngineNumber;
			   dbmodel.RegisterTime = input.RegisterTime;
			   dbmodel.CertificationTime = input.CertificationTime;
			   dbmodel.DrivingRemark = input.DrivingRemark;
               await _repository.UpdateAsync(dbmodel);
			   
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在。");
            }
        }
		
		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task Delete(EntityDto<Guid> input)
        {
            if(_xZGLCarInfoRepository.GetAll().Any(x=>x.CarId==input.Id))
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "已存在用车，不可删除。");
            await _repository.DeleteAsync(x=>x.Id == input.Id);
        }
        /// <summary>
        /// 车辆启用或禁用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Enable(EntityDto<Guid> input)
        {
            var car = _repository.Get(input.Id);
            car.IsEnable = !car.IsEnable;
            await _repository.UpdateAsync(car);
        }
    }
}