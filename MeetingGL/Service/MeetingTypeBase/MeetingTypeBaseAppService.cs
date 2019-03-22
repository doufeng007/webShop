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

namespace MeetingGL
{
    public class MeetingTypeBaseAppService : FRMSCoreAppServiceBase, IMeetingTypeBaseAppService
    {
        private readonly IRepository<MeetingTypeBase, Guid> _repository;

        public MeetingTypeBaseAppService(IRepository<MeetingTypeBase, Guid> repository

        )
        {
            this._repository = repository;

        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<MeetingTypeBaseListOutputDto>> GetList(GetMeetingTypeBaseListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)

                        select new MeetingTypeBaseListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            ReturnReceiptEnable = a.ReturnReceiptEnable,
                            WarningEnable = a.WarningEnable,
                            WarningDateNumber = a.WarningDateNumber,
                            WraningDataType = a.WraningDataType,
                            WraingType = a.WraingType,
                            Status = a.Status,
                            CreationTime = a.CreationTime

                        };
            if (input.IsEnable.HasValue)
            {
                if (input.IsEnable.Value)
                    query = query.Where(r => r.Status == 1);
                else
                    query = query.Where(r => r.Status == 0);
            }

            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<MeetingTypeBaseListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<MeetingTypeBaseOutputDto> Get(EntityDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<MeetingTypeBaseOutputDto>();
        }
        /// <summary>
        /// 添加一个MeetingTypeBase
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateMeetingTypeBaseInput input)
        {
            if (_repository.GetAll().Any(r => r.Name == input.Name))
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "会议类型重复");
            var newmodel = new MeetingTypeBase()
            {
                Name = input.Name,
                ReturnReceiptEnable = input.ReturnReceiptEnable,
                WarningEnable = input.WarningEnable,
                WarningDateNumber = input.WarningDateNumber,
                WraningDataType = input.WraningDataType,
                WraingType = input.WraingType,
                SignTime1 = input.SignTime1,
                SignTime2 = input.SignTime2,
                SignType = input.SignType,
                Status = input.Status
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个MeetingTypeBase
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateMeetingTypeBaseInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                if (_repository.GetAll().Any(r => r.Id != input.Id && r.Name == input.Name))
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "会议类型重复");

                dbmodel.Name = input.Name;
                dbmodel.ReturnReceiptEnable = input.ReturnReceiptEnable;
                dbmodel.WarningEnable = input.WarningEnable;
                dbmodel.WarningDateNumber = input.WarningDateNumber;
                dbmodel.WraningDataType = input.WraningDataType;
                dbmodel.WraingType = input.WraingType;
                dbmodel.SignTime1 = input.SignTime1;
                dbmodel.SignTime2 = input.SignTime2;
                dbmodel.SignType = input.SignType;
                dbmodel.Status = input.Status;

                await _repository.UpdateAsync(dbmodel);

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