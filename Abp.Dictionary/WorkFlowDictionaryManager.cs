using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abp.WorkFlowDictionary
{

    public class WorkFlowDictionaryManager : DomainService
    {
        protected IRepository<AbpDictionary, Guid> _abpDictionaryRepository { get; private set; }

        public WorkFlowDictionaryManager(IRepository<AbpDictionary, Guid> abpDictionaryRepository)
        {
            _abpDictionaryRepository = abpDictionaryRepository;
        }


        public async Task<Guid> GetIDByCode(string code)
        {
            var query = await _abpDictionaryRepository.FirstOrDefaultAsync(r => r.Code == code);
            if (query != null)
                return query.Id;
            else
                return Guid.Empty;
        }





    }
}
