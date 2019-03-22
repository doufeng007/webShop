using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using Abp.Domain.Repositories;
using Abp.Authorization;
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class ReplyUnitAppService : FRMSCoreAppServiceBase, IReplyUnitAppService
    {
        private readonly IRepository<ReplyUnit> _replyUnitRepository;
        public ReplyUnitAppService(IRepository<ReplyUnit> replyUnitRepository)
        {
            _replyUnitRepository = replyUnitRepository;
        }

    //    [AbpAuthorize("HR.Training.Operation")]
        public async Task CreateOrUpdate(ReplyUnitDto input)
        {
            if (input.Id.HasValue)
            {
                await UpdateAsync(input);
            }
            else
            {
                await CreateAsync(input);
            }
        }

        private async Task UpdateAsync(ReplyUnitDto input)
        {
            if (_replyUnitRepository.GetAll().Any(p => p.Name == input.Name && p.Id != input.Id.Value))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该名称已经存在");
            }
            var model = await _replyUnitRepository.GetAsync(input.Id.Value);
            model.Name = input.Name;
            model.Sort = input.Sort;
            await _replyUnitRepository.UpdateAsync(model);
        }

        [AbpAuthorize("HR.Training.Operation")]
        private async Task CreateAsync(ReplyUnitDto input)
        {
            if (_replyUnitRepository.GetAll().Any(p => p.Name == input.Name))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该名称已经存在");
            }
            var model = new ReplyUnit
            {
                Name = input.Name,
                Sort = input.Sort
        };
            await _replyUnitRepository.InsertAsync(model);
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.
        }

        public async Task<GetReplyUnitForEditOutput> GetForEdit(NullableIdDto<int> input)
        {
            if (!input.Id.HasValue)
                return new GetReplyUnitForEditOutput();
            var model = await _replyUnitRepository.GetAsync(input.Id.Value);
            var output = model.MapTo<GetReplyUnitForEditOutput>();
            return output;
        }

        public async Task<PagedResultDto<ReplyUnitList>> GetPage(GetReplyUnitListInput input)
        {
            var query = _replyUnitRepository.GetAll();
            var count = await query.CountAsync();
            var model = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var dto = model.MapTo<List<ReplyUnitList>>();
            return new PagedResultDto<ReplyUnitList>(count, dto);
        }

        public async Task Delete(EntityDto<int> input)
        {
            var model = await _replyUnitRepository.GetAsync(input.Id);
            await _replyUnitRepository.DeleteAsync(model);
        }

        public List<ReplyUnitList> GetAll()
        {
            var model = _replyUnitRepository.GetAll().OrderBy(r=>r.Sort).ToList();
            var dto = model.MapTo<List<ReplyUnitList>>();
            return dto;
        }

        public string GetName(EntityDto input)
        {
            var model = _replyUnitRepository.GetAll().FirstOrDefault(r => r.Id == input.Id);
            if (model == null) return "";
            return model.Name;
        }

    }
}