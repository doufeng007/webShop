using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Extensions;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.Application.Services;
using ZCYX.FRMSCore.Authorization.Users;

namespace ZCYX.FRMSCore.Application
{
    public class RealationSystemAppService : AsyncCrudAppService<RealationSystem, RealationSystemDto, Guid, PagedResultRequestDto, CreateRealationSystemDto, RealationSystemDto>, IRealationSystemAppService
    {
        private readonly IRepository<RealationSystem, Guid> _repository;
        private readonly UserManager _userManager;

        public RealationSystemAppService(IRepository<RealationSystem, Guid> repository
            , UserManager userManager
            )
            : base(repository)
        {
            _repository = repository;
            _userManager = userManager;

        }

        public override async Task<RealationSystemDto> Update(RealationSystemDto input)
        {
            var model = await _repository.GetAsync(input.Id);
            model.Name = input.Name;
            model.Code = input.Code;
            model.ServiceUrl = input.ServiceUrl;
            model.SystemType = (SystemType)input.SystemType;
            model.Remark = input.Remark;
            model.UserId = input.UserId;
            await _repository.UpdateAsync(model);
            return model.MapTo<RealationSystemDto>();
        }

        public override async Task<RealationSystemDto> Get(EntityDto<Guid> input)
        {
            var model = await base.Get(input);
            if (model.UserId.HasValue)
            {
                model.UserName = (await _userManager.GetUserByIdAsync(model.UserId.Value)).Name;
            }
            return model;
        }

        public override async Task<PagedResultDto<RealationSystemDto>> GetAll(PagedResultRequestDto input)
        {
            var query = from a in _repository.GetAll()
                        join u in _userManager.Users on a.UserId equals u.Id into g
                        from u in g.DefaultIfEmpty()
                        select new RealationSystemDto
                        {
                            Code = a.Code,
                            Id = a.Id,
                            Name = a.Name,
                            Remark = a.Remark,
                            ServiceUrl = a.ServiceUrl,
                            SystemType = (int)a.SystemType,
                            UserId = a.UserId,
                            UserName = u == null ? "" : u.Name
                        };
            var totalCount = await query.CountAsync();
            var data = await query
            .OrderBy(r => r.Name)
            .PageBy(input)
            .ToListAsync();
            return new PagedResultDto<RealationSystemDto>(totalCount, data);
        }
    }
}
