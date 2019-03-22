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
using Abp.Extensions;
using ZCYX.FRMSCore.Users;
using ZCYX.FRMSCore.Authorization.Users;
using Microsoft.Extensions.Configuration;
using ZCYX.FRMSCore.Application;
using Abp.Reflection.Extensions;
using ZCYX.FRMSCore.Configuration;
using Abp.File;
using ZCYX.FRMSCore.Model;

namespace HR
{
    public class CollaborativeInstitutionsAppService : FRMSCoreAppServiceBase, ICollaborativeInstitutionsAppService
    {
        private readonly IRepository<CollaborativeInstitutions, Guid> _repository;
        private readonly UserManagerNotRemote _IuserManger;
        private readonly IRepository<User, long> _userRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;

        public CollaborativeInstitutionsAppService(IRepository<CollaborativeInstitutions, Guid> repository, UserManagerNotRemote IuserManger, IRepository<User, long> userRepository
            , IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository, IAbpFileRelationAppService abpFileRelationAppService)
        {
            this._repository = repository;
            _IuserManger = IuserManger;
            _userRepository = userRepository;
            var coreAssemblyDirectoryPath = typeof(CollaborativeInstitutionsAppService).GetAssembly().GetDirectoryPathOrNull();
            _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath);
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CollaborativeInstitutionsListOutputDto>> GetList(GetCollaborativeInstitutionsListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _userRepository.GetAll() on a.UserId equals b.Id
                        select new CollaborativeInstitutionsListOutputDto()
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            UserAcount = b.UserName,
                            Name = a.Name,
                            Function = a.Function,
                            ScaleNum = a.ScaleNum,
                            Address = a.Address,
                            Head = a.Head,
                            Tel = a.Tel,
                            BankNum = a.BankNum,
                            BankName = a.BankName,
                            BankDeposit = a.BankDeposit,
                            CreationTime = a.CreationTime,
                            EmailAddress = b.EmailAddress,
                        };
            if (!input.SearchKey.IsNullOrWhiteSpace())
                query = query.Where(r => r.Name.Contains(input.SearchKey));
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultDto<CollaborativeInstitutionsListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task<CollaborativeInstitutionsOutputDto> Get(EntityDto<Guid> input)
        {
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var ret = model.MapTo<CollaborativeInstitutionsOutputDto>();
            var userModel = await _userRepository.GetAsync(model.UserId);
            ret.UserAcount = userModel.UserName;
            ret.EmailAddress = userModel.EmailAddress;
            ret.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.Id.ToString(), BusinessType = (int)AbpFileBusinessType.人力资源协作机构合同 });
            return ret;
        }
        /// <summary>
        /// 添加一个CollaborativeInstitutions
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Create(CreateCollaborativeInstitutionsInput input)
        {
            var xietongdanweiOrgId = _appConfiguration["App:XietongdanweiOrgId"];
            var account = await _IuserManger.MakeUserName(input.Name);
            var userId = await _IuserManger.Create(new ZCYX.FRMSCore.Users.Dto.CreateUserDto()
            {
                Name = input.Name,
                IsActive = true,
                PhoneNumber = input.Tel,
                RoleNames = new string[] { "XZDW" },
                Password = "psxzry",
                UserName = account,
                EmailAddress = input.EmailAddress,
                Surname = input.Name,
            });
            var newUserOrg = new WorkFlowUserOrganizationUnits() { IsMain = true, OrganizationUnitId = xietongdanweiOrgId.ToLong(), UserId = userId };
            await _userOrganizationUnitRepository.InsertAsync(newUserOrg);
            var newmodel = new CollaborativeInstitutions()
            {
                UserId = userId,
                Name = input.Name,
                Function = input.Function,
                ScaleNum =(int)input.ScaleNum,
                Address = input.Address,
                Head = input.Head,
                Tel = input.Tel,
                BankNum = input.BankNum,
                BankName = input.BankName,
                BankDeposit = input.BankDeposit
            };
            await _repository.InsertAsync(newmodel);
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = newmodel.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.人力资源协作机构合同,
                    Files = fileList
                });
            }

        }

        /// <summary>
        /// 修改一个CollaborativeInstitutions
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateCollaborativeInstitutionsInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                //dbmodel.UserId = input.UserId;
                dbmodel.Name = input.Name;
                dbmodel.Function = input.Function;
                dbmodel.ScaleNum = (int)input.ScaleNum;
                dbmodel.Address = input.Address;
                dbmodel.Head = input.Head;
                dbmodel.Tel = input.Tel;
                dbmodel.BankNum = input.BankNum;
                dbmodel.BankName = input.BankName;
                dbmodel.BankDeposit = input.BankDeposit;
                await _repository.UpdateAsync(dbmodel);
                var userModel = await _userRepository.GetAsync(dbmodel.UserId);
                userModel.Name = input.Name;
                userModel.PhoneNumber = input.Tel;
                userModel.EmailAddress = input.EmailAddress;


                var fileList = new List<AbpFileListInput>();
                if (input.FileList != null)
                {
                    foreach (var item in input.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                    }
                }
                await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = input.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.人力资源协作机构合同,
                    Files = fileList
                });
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }

        // <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            var dbmodel = await _repository.GetAsync(input.Id);
            await _repository.DeleteAsync(x => x.Id == input.Id);
            await _userRepository.DeleteAsync(x => x.Id == dbmodel.UserId);

        }



    }
}