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
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Model;

namespace CWGL
{
    public class CWGLCredentialAppService : FRMSCoreAppServiceBase, ICWGLCredentialAppService
    {
        private readonly IRepository<CWGLCredential, Guid> _repository;
        private readonly IRepository<User, long> _usersRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;

        public CWGLCredentialAppService(IRepository<CWGLCredential, Guid> repository,
            IRepository<User, long> usersRepository, IAbpFileRelationAppService abpFileRelationAppService
        )
        {
            this._repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CWGLCredentialListOutputDto>> GetList(GetCWGLCredentialListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && !x.IsPay)
                       join b in _usersRepository.GetAll() on a.CreatorUserId equals b.Id
                        select new CWGLCredentialListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            Name = a.Name,
                            Money = a.Money,
                            UserName = b.Name
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<CWGLCredentialListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CWGLCredentialListOutputDto>> GetPayList(GetCWGLCredentialListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && x.IsPay)
                        join b in _usersRepository.GetAll() on a.CreatorUserId equals b.Id
                        select new CWGLCredentialListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            Name = a.Name,
                            Money = a.Money,
                            UserName = b.Name
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<CWGLCredentialListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<CWGLCredentialOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var tmp = model.MapTo<CWGLCredentialOutputDto>();
            tmp.UserId = model.CreatorUserId.Value;
            if (model.CreatorUserId.HasValue)
            {
                var user = _usersRepository.Get(model.CreatorUserId.Value);
                tmp.UserName = user.Name;
            }
            tmp.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.凭据
            });
            return tmp;
        }
        /// <summary>
        /// 添加一个CWGLCredential
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateCWGLCredentialInput input)
        {
            var id = Guid.NewGuid();
            var newmodel = new CWGLCredential()
            {
                Id = id,
                Name = input.Name,
                Cause = input.Cause,
                Money = input.Money,
                Mode = input.Mode,
                IsPay = input.IsPay,
                BankName = input.BankName,
                ContractNum = input.ContractNum,
                FlowNumber = input.FlowNumber,
                CardNumber = input.CardNumber,
                BankOpenName = input.BankOpenName,
                Nummber = input.Nummber,
                BusinessId = input.BusinessId,
                BusinessType = input.BusinessType
            };
            if (input.CreationTime.HasValue)
                newmodel.CreationTime = input.CreationTime.Value;

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
                    BusinessId = id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.凭据,
                    Files = fileList
                });
            }

        }

        /// <summary>
        /// 修改一个CWGLCredential
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateCWGLCredentialInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                dbmodel.Name = input.Name;
                dbmodel.Cause = input.Cause;
                dbmodel.Money = input.Money;
                dbmodel.Mode = input.Mode;
                dbmodel.BankName = input.BankName;
                dbmodel.CardNumber = input.CardNumber;
                dbmodel.BankOpenName = input.BankOpenName;
                dbmodel.Nummber = input.Nummber;

                await _repository.UpdateAsync(dbmodel);
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
                    BusinessType = (int)AbpFileBusinessType.凭据,
                    Files = fileList
                });
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }

    }
}