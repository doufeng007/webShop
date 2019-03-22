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
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class ProjectAreaAppService : ApplicationService, IProjectAreaAppService
    {
        private readonly IRepository<ProjectAreas, Guid> _projectAreaRepository;
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<User, long> _userRepository;

        public ProjectAreaAppService(IRepository<ProjectAreas, Guid> projectAreaRepository, IRepository<User, long> userRepository,
            IProjectBaseRepository projectBaseRepository)
        {
            _projectAreaRepository = projectAreaRepository;
            _projectBaseRepository = projectBaseRepository;
            _userRepository = userRepository;
        }
        public void CreatorUpdate(ProjectAreaDto input)
        {
            var model = input.MapTo<ProjectAreas>();
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请输入片区名字。");
            }
            if (model.Name.Length > 20)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeLenErr, "片区名字不能大于10个汉字。");
            }
            if (model.User_Id.HasValue == false)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请选择片区负责人。");
            }
            _projectAreaRepository.InsertOrUpdate(model);
        }

        public long GetAreaInProjectUserId(Guid projectid)
        {
            var baseinfo = _projectBaseRepository.Get(projectid);
            if (baseinfo == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到项目信息");
            }
            if (baseinfo.Area_Id.HasValue == false)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请先选择片区。");
            }
            var area = _projectAreaRepository.Get(baseinfo.Area_Id.Value);
            if (area == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "片区不存在。");
            }
            if (area.User_Id.HasValue == false)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "选择的片区未指定负责人");
            }
            return area.User_Id.Value;
        }

        public PagedResultDto<ProjectAreaDto> GetList(PagedAndSortedInputDto input)
        {
            var list = from a in _projectAreaRepository.GetAll()
                       join b in _userRepository.GetAll() on a.User_Id equals b.Id
                       orderby a.CreationTime descending
                       select new ProjectAreaDto
                       {
                           CreationTime = a.CreationTime,
                           CreatorUserId = a.CreatorUserId,
                           Id = a.Id,
                           Name = a.Name,
                           ParentName = "",
                           Parent_Id = a.Parent_Id,
                           User_Id = a.User_Id,
                           Surname = b.Name,
                           IsDeleted = a.IsDeleted
                       };
            var count = list.Count();
            var result = list.PageBy(input).ToList();
            return new PagedResultDto<ProjectAreaDto>(count, result);
        }


    }
}
