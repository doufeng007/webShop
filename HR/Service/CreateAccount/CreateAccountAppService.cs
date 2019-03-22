using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.WorkFlow;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HR
{
    /// <summary>
    ///新员工入职后，人事创建帐号申请
    /// </summary>
    [AbpAuthorize]
    public class CreateAccountAppService : FRMSCoreAppServiceBase, ICreateAccountAppService
    {
        private readonly IRepository<CreateAccount, Guid> _createAccountRepository;
        private readonly IRepository<PostInfo, Guid> _postsRepository;
        private readonly IRepository<OrganizationUnitPosts, Guid> _organizationUnitPostsRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        public CreateAccountAppService(IRepository<CreateAccount, Guid> createAccountRepository, IRepository<PostInfo, Guid> postsRepository
            , IRepository<OrganizationUnitPosts, Guid> organizationUnitPostsRepository
            , IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository)
        {
            _createAccountRepository = createAccountRepository;
            _postsRepository = postsRepository;
            _organizationUnitPostsRepository = organizationUnitPostsRepository;
            _organizationUnitRepository = organizationUnitRepository;
        }
        /// <summary>
        /// 人事创建帐号申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(CreateAccountInput input)
        {
            var model = input.MapTo<CreateAccount>();
            model.Id = Guid.NewGuid();
            _createAccountRepository.Insert(model);
            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };

        }
        /// <summary>
        /// 管理员查看帐号创建申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<CreateAccountDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = (await _createAccountRepository.GetAsync(id)).MapTo<CreateAccountDto>();

            model.Post_Name = (from a in _organizationUnitPostsRepository.GetAll()
                               join b in _postsRepository.GetAll() on a.PostId equals b.Id
                               where a.Id == model.Post 
                               select b).First().Name;
            model.Department_Name = _organizationUnitRepository.Get(model.Department).DisplayName;

            return model;
        }

        public void Update(CreateAccountDto input)
        {
            throw new NotImplementedException();
        }
    }
}
