using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.File;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;

namespace HR.Service.EmployeeDocment
{
    /// <summary>
    /// 员工档案管理
    /// </summary>
    public class EmployeeDocmentAppService : FRMSCoreAppServiceBase, IEmployeeDocmentAppService
    {
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly AbpOrganizationUnitsManager _organizationUnitManager;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<PostInfo, Guid> _postsRepository;
        private readonly IRepository<UserPosts, Guid> _userPostsRepository;
        private readonly IRepository<OrganizationUnitPosts, Guid> _organizationUnitPostsRepository;
        public EmployeeDocmentAppService(
            IRepository<Employee, Guid> employeeRepository,
            IAbpFileRelationAppService abpFileRelationAppService,
            IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository,
            IRepository<PostInfo, Guid> postsRepository, IRepository<UserPosts, Guid> userPostsRepository, IRepository<OrganizationUnitPosts, Guid> organizationUnitPostsRepository
            ) {
            _employeeRepository = employeeRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _postsRepository = postsRepository;
            _userPostsRepository = userPostsRepository;
            _organizationUnitPostsRepository = organizationUnitPostsRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
        }

        public EmployeeDocmentListDto Get(Guid employeeId)
        {
            var e = _employeeRepository.Get(employeeId);
            var r = e.MapTo<EmployeeDocmentListDto>();
            r.AuditFileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.面试评审表 });
            r.ContactFileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.员工合同 });
            r.EduFileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.员工毕业证书 });
            r.FaceFileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.员工面试题 });
            r.InfoFileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.员工信息登记表 });
            r.SalaryFileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.员工薪水核定表 });

            return r;
        }

        /// <summary>
        /// 获取员工档案信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<EmployeeDocmentListDto>> GetList(EmployeeSearchInput input)
        {
            var query = _employeeRepository.GetAll();
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                query = query.Where(ite => ite.Name.Contains(input.SearchKey) || ite.Phone.Contains(input.SearchKey) || ite.Code.Contains(input.SearchKey));
            }
            if (input.OrgId.HasValue)
            {
                var orgu = _userOrganizationUnitRepository.GetAll().Where(ite => ite.OrganizationUnitId == input.OrgId.Value).Select(ite => ite.UserId).ToList();
                query = query.Where(ite => orgu.Contains(ite.UserId.Value));
            }
            if (input.IsTemp.HasValue && input.IsTemp.Value)
            {
                query = query.Where(ite => ite.IsTemp == true);
            }
            var totalCount = query.Count();

            var ret = (await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync()).MapTo<List<EmployeeDocmentListDto>>();

            foreach (var r in ret)
            {
                if (r.UserId.HasValue)
                {
                    var org = await(from a in _userOrganizationUnitRepository.GetAll()
                                    join b in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals b.Id
                                    where a.UserId == r.UserId.Value
                                    select new SimpleOrganizationDto()
                                    {
                                        Code = b.Code,
                                        Id = a.OrganizationUnitId,
                                        IsMain = a.IsMain,
                                        Title = b.DisplayName
                                    }).ToListAsync();
                    var post = from a in _postsRepository.GetAll()
                               join b in _userPostsRepository.GetAll() on a.Id equals b.PostId
                               join c in _organizationUnitPostsRepository.GetAll() on b.OrgPostId equals c.Id
                               join d in _organizationUnitRepository.GetAll() on c.OrganizationUnitId equals d.Id
                               where b.UserId == r.UserId.Value
                               select new UserPostDto { Id = b.Id, OrgPostId = c.Id, PostId = a.Id, PostName = a.Name, OrgName = d.DisplayName };
                    r.Organization = org.ToList();
                    r.Posts = post.ToList();
                }
                //r.AuditFileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.面试评审表 });
                //r.ContactFileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.员工合同 });
                //r.EduFileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.员工毕业证书 });
                //r.FaceFileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.员工面试题 });
                //r.InfoFileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.员工信息登记表 });
                //r.SalaryFileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.员工薪水核定表 });
            }
            return new PagedResultDto<EmployeeDocmentListDto>(totalCount, ret);
        }
        /// <summary>
        /// 更新员工档案信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Update(EmployeeDocmentFileInput input)
        {
            if (input.FileList != null) {
                var fileList = new List<AbpFileListInput>();
                foreach (var ite in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = ite.Id, Sort = ite.Sort });
                }
               await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = input.BusinessId.ToString(),
                    BusinessType =(int) input.Type,
                    Files = fileList
                });
            }
        }
    }
}
