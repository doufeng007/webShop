using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.Application.Services;
using ZCYX.FRMSCore.Application.Dto;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Project
{
    public class ProjcetAuditResultCheckRoleAppService : ApplicationService, IProjcetAuditResultCheckRoleAppService
    {
        private readonly IRepository<ProjcetAuditResultCheckRole, Guid> _projcetAuditResultCheckRoleRepository;
        private readonly IRepository<User, long> _userRepository;

        public ProjcetAuditResultCheckRoleAppService(IRepository<ProjcetAuditResultCheckRole, Guid> projcetAuditResultCheckRoleRepository, IRepository<User, long> userRepository,
            IProjectBaseRepository projectBaseRepository)
        {
            _projcetAuditResultCheckRoleRepository = projcetAuditResultCheckRoleRepository;
            _userRepository = userRepository;
        }
        public async Task CreatorUpdate(CreateorUpdateCheckRoleInput input)
        {
            if (input.Id.HasValue)
            {
                var model = await _projcetAuditResultCheckRoleRepository.GetAsync(input.Id.Value);
                input.MapTo(model);
                await _projcetAuditResultCheckRoleRepository.UpdateAsync(model);
            }
            else
            {
                var model = input.MapTo<ProjcetAuditResultCheckRole>();
                model.Id = Guid.NewGuid();
                await _projcetAuditResultCheckRoleRepository.InsertAsync(model);
            }

        }

        public async Task<ProjcetAuditResultCheckRoleDto> GetAsync(EntityDto<Guid> input)
        {
            var model = await _projcetAuditResultCheckRoleRepository.GetAsync(input.Id);
            return model.MapTo<ProjcetAuditResultCheckRoleDto>();
        }

        public async Task<PagedResultDto<ProjcetAuditResultCheckRoleDto>> GetList(PagedAndSortedInputDto input)
        {
            var list = _projcetAuditResultCheckRoleRepository.GetAll().WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), r => r.Content.Contains(input.SearchKey));
            var count = await list.CountAsync();
            var result = list.PageBy(input).ToList();
            return new PagedResultDto<ProjcetAuditResultCheckRoleDto>(count, result.MapTo<List<ProjcetAuditResultCheckRoleDto>>());
        }
    }
}
