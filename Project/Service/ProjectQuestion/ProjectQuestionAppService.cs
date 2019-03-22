using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Authorization.Users;
using System.Linq;
using Abp.Linq.Extensions;
using ZCYX.FRMSCore.Application.Dto;
using Microsoft.EntityFrameworkCore;

namespace Project
{
    public class ProjectQuestionAppService : ApplicationService, IProjectQuestionAppService
    {
        private readonly IRepository<ProjectQuestion, Guid> _projectQuestionRepository;
        private readonly IRepository<User, long> _userrepository;
        public ProjectQuestionAppService(IRepository<ProjectQuestion, Guid> projectQuestionRepository, IRepository<User, long> userrepository) {
            _projectQuestionRepository = projectQuestionRepository;
            _userrepository = userrepository;
        }
        /// <summary>
        /// 项目经理回复
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Answer(ProjectQuestionAnswerInput input)
        {
            var q = _projectQuestionRepository.Get(input.Id);
            q.Answer = input.Answer;
            await _projectQuestionRepository.UpdateAsync(q);
        }
        /// <summary>
        /// 工程评审人创建问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Create(CreateProjectQuestionInput input)
        {
            var model = input.MapTo<ProjectQuestion>();
            await _projectQuestionRepository.InsertAsync(model);

        }
        /// <summary>
        /// 项目经理获取所有问题列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProjectQuestionDto>> GetAllList(GetListInput input)
        {
            var query = from a in _projectQuestionRepository.GetAll()
                        join b in _userrepository.GetAll() on a.CreatorUserId equals b.Id
                        where a.ProjectId == input.ProjectId
                        select new ProjectQuestionDto()
                        {
                            Answer = a.Answer,
                            Content = a.Content,
                            Id = a.Id,
                            ProjectId = a.ProjectId,
                            Title = a.Title,
                            UserName = b.Name,
                            CreationTime = a.CreationTime
                        };
            var total =await query.CountAsync();
            var model = query.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<ProjectQuestionDto>>();

            return new PagedResultDto<ProjectQuestionDto>(total,model);
        }
        /// <summary>
        /// 工程评审人获取自己的问题列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProjectQuestionDto>> GetMyList(GetListInput input)
        {
            var query = from a in _projectQuestionRepository.GetAll()
                        join b in _userrepository.GetAll() on a.CreatorUserId equals b.Id
                        where a.ProjectId == input.ProjectId&&a.CreatorUserId==AbpSession.UserId
                        select new ProjectQuestionDto()
                        {
                            Answer = a.Answer,
                            Content = a.Content,
                            Id = a.Id,
                            ProjectId = a.ProjectId,
                            Title = a.Title,
                            UserName = b.Name,
                            CreationTime = a.CreationTime
                        };
            var total = await query.CountAsync();
            var model = query.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<ProjectQuestionDto>>();

            return new PagedResultDto<ProjectQuestionDto>(total, model);
        }
    }
}
