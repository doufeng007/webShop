using Abp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Configuration;

namespace Project
{
    public class RecevieProjectAuditResult : IRabbitMqRecevieCallBack
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;


        public RecevieProjectAuditResult(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
        }
        public async Task<RabbitMqRecevieCallBackOutput> RabbitMqRecevieCallBack(RabbitMqRecevieCallBackInput input)
        {
            //var framGovUrl = _appConfiguration["RabbitMQ:frmsgovUrl"];

            //var requestUrl = $"{framGovUrl}/api/services/app/project/GetProjectBudgetForEdit";
            //var param = new Dictionary<string, string>()
            //    {
            //      {"id", input.Parameter}
            //     };
            //var result = await DoPost(requestUrl, param);
            //var projectService = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectAppService>();
            //await projectService.CreateAsync(new CreateOrUpdateProJectBudgetManagerInput() { ProjectName = "测试rabbitMQ" });
            var ret = new RabbitMqRecevieCallBackOutput();
            ret.Result = RabbitMqCallBackResultType.Succesefull;
            return ret;
        }


        
    }
}
