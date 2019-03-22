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
using Abp.Application.Services;
using ZCYX.FRMSCore;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.UI;
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class AappraisalFileTypeAppService : ApplicationService, IAappraisalFileTypeAppService
    {
        private readonly IRepository<AappraisalFileType> _aappraisalFileTypeRepository;
        private readonly IRepository<Code_AppraisalType> _code_AppraisalTypeRepository;
        public AappraisalFileTypeAppService(IRepository<AappraisalFileType> AappraisalFileTypeRepository, IRepository<Code_AppraisalType> code_AppraisalTypeRepository)
        {
            _aappraisalFileTypeRepository = AappraisalFileTypeRepository;
            _code_AppraisalTypeRepository = code_AppraisalTypeRepository;
        }

        public async Task CreateOrUpdateAappraisalFileType(CreateOrUpdateAappraisalFileTypeInput input)
        {
            if (input.Id.HasValue)
            {
                await UpdateAappraisalFileTypeAsync(input);
            }
            else
            {
                await CreateAappraisalFileTypeAsync(input);
            }
        }

        public async Task<int> CreateOrUpdate(CreateOrUpdateAappraisalFileTypeInput input)
        {
            if (input.Id.HasValue)
            {
                await UpdateAappraisalFileTypeAsync(input);
                return input.Id.Value;
            }
            else
            {
                return await CreateAappraisalFileType(input);
            }
        }



        public async Task<GetAappraisalFileTypeForEditOutput> GetAappraisalFileTypeForEdit(NullableIdDto<int> input)
        {
            if (!input.Id.HasValue)
                return new GetAappraisalFileTypeForEditOutput();
            var aappraisalFileType = await _aappraisalFileTypeRepository.GetAsync(input.Id.Value);
            var output = aappraisalFileType.MapTo<GetAappraisalFileTypeForEditOutput>();
            return output;
        }

        public async Task<PagedResultDto<AappraisalFileTypeListDto>> GetAappraisalFileTypes(GetAappraisalFileTypeListInput input)
        {

            var query = from a in _aappraisalFileTypeRepository.GetAll()
                        join code in _code_AppraisalTypeRepository.GetAll() on a.AppraisalTypeId equals code.Id
                        select new AappraisalFileTypeListDto
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Sort = a.Sort,
                            Code_AappTypeName = code.Name,
                            AuditRoleIds = a.AuditRoleIds,
                             CreationTime=a.CreationTime
                        };

            var count = await query.CountAsync();
            var aappraisalFileTypes = await query
             .OrderByDescending(ite=>ite.CreationTime)
             .PageBy(input)
             .ToListAsync();
            foreach (var a in aappraisalFileTypes)
            {
                if (string.IsNullOrWhiteSpace(a.AuditRoleIds) == false)
                {
                    var str = Array.ConvertAll(a.AuditRoleIds.Split(","), new Converter<string, string>(i => { return ((AuditRoleEnum)int.Parse(i)).ToString(); }));
                    a.AuditRoleIds = string.Join(",", str);
                }
                else
                {
                    a.AuditRoleIds = "全部";
                }
                
                
            }
            return new PagedResultDto<AappraisalFileTypeListDto>(count, aappraisalFileTypes);


        }

        public ListResultDto<AappraisalFileTypeListDto> GetAllAappraisalFileTypes(GetAappraisalFileTypeListInput input)
        {
            var query = _aappraisalFileTypeRepository.GetAll();
            var aappraisalFileTypes = query
             .OrderBy(input.Sorting)
             .ToList();
            var aappraisalFileTypeDtos = aappraisalFileTypes.MapTo<List<AappraisalFileTypeListDto>>();
            return new ListResultDto<AappraisalFileTypeListDto> { Items = aappraisalFileTypeDtos };
        }


        public async Task DeleteAappraisalFileType(EntityDto<int> input)
        {
            var aappraisalFileType = await _aappraisalFileTypeRepository.GetAsync(input.Id);
            await _aappraisalFileTypeRepository.DeleteAsync(aappraisalFileType);
        }



        protected virtual async Task UpdateAappraisalFileTypeAsync(CreateOrUpdateAappraisalFileTypeInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            if (_aappraisalFileTypeRepository.GetAll().Any(x => x.Name == input.Name && x.AppraisalTypeId == input.AppraisalTypeId && x.Id!=input.Id.Value))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "同一评审类型名称不能相同。");
            }

            var aappraisalFileType = await _aappraisalFileTypeRepository.GetAsync(input.Id.Value);
            aappraisalFileType.Name = input.Name;
            aappraisalFileType.AppraisalTypeId = input.AppraisalTypeId;
            aappraisalFileType.IsPaperFile = input.IsPaperFile;
            aappraisalFileType.IsMust = input.IsMust;
            aappraisalFileType.Sort = input.Sort;
            aappraisalFileType.AuditRoleIds = input.AuditRoleIds;
            await _aappraisalFileTypeRepository.UpdateAsync(aappraisalFileType);
        }

        protected virtual async Task CreateAappraisalFileTypeAsync(CreateOrUpdateAappraisalFileTypeInput input)
        {
            try
            {
                if (_aappraisalFileTypeRepository.GetAll().Any(x => x.Name == input.Name &&x.AppraisalTypeId==input.AppraisalTypeId))
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "同一评审类型名称不能相同。");
                }
                var aappraisalFileType = new AappraisalFileType()
                {
                    Name = input.Name,
                    AppraisalTypeId = input.AppraisalTypeId,
                    IsPaperFile = input.IsPaperFile,
                    IsMust = input.IsMust,
                    Sort = input.Sort,
                    AuditRoleIds = input.AuditRoleIds,
                };
                await _aappraisalFileTypeRepository.InsertAndGetIdAsync(aappraisalFileType);
                await CurrentUnitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw;
            }


        }

        protected virtual async Task<int> CreateAappraisalFileType(CreateOrUpdateAappraisalFileTypeInput input)
        {
            var aappraisalFileType = new AappraisalFileType()
            {
                Name = input.Name,
                AppraisalTypeId = input.AppraisalTypeId,
                IsMust = input.IsMust,
                IsPaperFile = input.IsPaperFile,
                AuditRoleIds = input.AuditRoleIds
                // Sort_id = input.Sort_id
            };
            var retid = await _aappraisalFileTypeRepository.InsertAndGetIdAsync(aappraisalFileType);
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.
            return retid;
        }


    }
}
