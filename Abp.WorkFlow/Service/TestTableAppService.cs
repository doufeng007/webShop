using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.WorkFlow.Entity;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Abp.WorkFlow
{
    public class TestTableAppService : AsyncCrudAppService<TestTable, TestTableDto, Guid, PagedResultRequestDto, CreateTestTableDto, TestTableDto>, ITestTableAppService
    {
        private readonly IRepository<TestTable, Guid> _repository;
        public TestTableAppService(IRepository<TestTable, Guid> repository)
            : base(repository)
        {
            _repository = repository;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<CreateTestTableOutput> CreateTest(CreateTestTableDto input)
        {
            var model = input.MapTo<TestTable>();var ret = new CreateTestTableOutput();
            ret.InStanceId = (await _repository.InsertAndGetIdAsync(model)).ToString();
            return ret;
        }


    }
}
