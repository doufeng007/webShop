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
using ZCYX.FRMSCore;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.Application.Services;

namespace Project
{
    public class Code_AppraisalTypeAppService : FRMSCoreAppServiceBase, ICode_AppraisalTypeAppService
    {
        private readonly IRepository<Code_AppraisalType> _code_AppraisalTypeRepository;
        public Code_AppraisalTypeAppService(IRepository<Code_AppraisalType> code_AppraisalTypeRepository)
        {
            _code_AppraisalTypeRepository = code_AppraisalTypeRepository;
        }

        public async Task CreateOrUpdateCode_AppraisalType(CreateOrUpdateCode_AppraisalTypeInput input)
        {
            if (input.Id.HasValue)
            {
                await UpdateCode_AppraisalTypeAsync(input);
            }
            else
            {
                await CreateCode_AppraisalTypeAsync(input);
            }
        }

        public async Task<int> CreateOrUpdate(CreateOrUpdateCode_AppraisalTypeInput input)
        {
            if (input.Id.HasValue)
            {
                await UpdateCode_AppraisalTypeAsync(input);
                return input.Id.Value;
            }
            else
            {
                return await CreateCode_AppraisalType(input);
            }
        }



        public async Task<GetCode_AppraisalTypeForEditOutput> GetCode_AppraisalTypeForEdit(NullableIdDto<int> input)
        {
            if (!input.Id.HasValue)
                return new GetCode_AppraisalTypeForEditOutput();
            var _code_AppraisalType = await _code_AppraisalTypeRepository.GetAsync(input.Id.Value);
            var output = _code_AppraisalType.MapTo<GetCode_AppraisalTypeForEditOutput>();
            return output;
        }

        public async Task<PagedResultDto<Code_AppraisalTypeListDto>> GetCode_AppraisalTypes(GetCode_AppraisalTypeListInput input)
        {
            try
            {

                var query = from code in _code_AppraisalTypeRepository.GetAll()
                            join pcoded in _code_AppraisalTypeRepository.GetAll() on code.ParentId equals pcoded.Id into g
                            from pcode in g.DefaultIfEmpty()
                            where (!input.ParentId.HasValue || code.ParentId == input.ParentId.Value)
                            select new Code_AppraisalTypeListDto { Id = code.Id, Name = code.Name, ParentName = pcode.Name, ParentId = code.ParentId, Sort = code.Sort };
                var count = await query.CountAsync();
                var _code_AppraisalTypes = await query
                 .OrderBy(input.Sorting)
                 .PageBy(input)
                 .ToListAsync();
                return new PagedResultDto<Code_AppraisalTypeListDto>(count, _code_AppraisalTypes);
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public ListResultDto<Code_AppraisalTypeListDto> GetAllCode_AppraisalTypes(GetCode_AppraisalTypeListInput input)
        {
            var query = _code_AppraisalTypeRepository.GetAll();
            var _code_AppraisalTypes = query.Where(r => !input.ParentId.HasValue || r.ParentId == input.ParentId.Value)
             .OrderBy(input.Sorting)
             .ToList();
            var code_AppraisalTypeDtos = _code_AppraisalTypes.MapTo<List<Code_AppraisalTypeListDto>>();
            return new ListResultDto<Code_AppraisalTypeListDto> { Items = code_AppraisalTypeDtos };
        }


        public async Task DeleteCode_AppraisalType(EntityDto<int> input)
        {
            var _code_AppraisalType = await _code_AppraisalTypeRepository.GetAsync(input.Id);
            await _code_AppraisalTypeRepository.DeleteAsync(_code_AppraisalType);
        }



        protected virtual async Task UpdateCode_AppraisalTypeAsync(CreateOrUpdateCode_AppraisalTypeInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var _code_AppraisalType = await _code_AppraisalTypeRepository.GetAsync(input.Id.Value);
            _code_AppraisalType.Name = input.Name;
            _code_AppraisalType.Sort = input.Sort;
            _code_AppraisalType.ParentId = input.ParentId;
            await _code_AppraisalTypeRepository.UpdateAsync(_code_AppraisalType);
        }

        protected virtual async Task CreateCode_AppraisalTypeAsync(CreateOrUpdateCode_AppraisalTypeInput input)
        {
            var _code_AppraisalType = new Code_AppraisalType()
            {
                Name = input.Name,
                Sort = input.Sort,
                ParentId = input.ParentId

            };
            await _code_AppraisalTypeRepository.InsertAndGetIdAsync(_code_AppraisalType);
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.
        }

        protected virtual async Task<int> CreateCode_AppraisalType(CreateOrUpdateCode_AppraisalTypeInput input)
        {
            var _code_AppraisalType = new Code_AppraisalType()
            {
                Name = input.Name,
                Sort = input.Sort,
                ParentId = input.ParentId
            };
            var retid = await _code_AppraisalTypeRepository.InsertAndGetIdAsync(_code_AppraisalType);
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.
            return retid;
        }


    }
}
