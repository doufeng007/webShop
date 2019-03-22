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
using Abp.UI;
using Newtonsoft.Json.Linq;
using Abp.Extensions;
using Abp.Application.Services;
using ZCYX.FRMSCore.Authorization.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ZCYX.FRMSCore.Configuration;
using Abp;
using System.Net.Http;
using System.Net;
using ZCYX.FRMSCore.Web;
using Abp.Reflection.Extensions;
using Abp.Authorization;

namespace Project
{
    public class RabbitMQRecevieCallBackAppService : ApplicationService, IRabbitMQRecevieCallBackAppService
    {
        private readonly IRepository<User, long> _useRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IProjectBaseRepository _projectBaseRepository;


        public RabbitMQRecevieCallBackAppService(IRepository<User, long> useRepository, IProjectBaseRepository projectBaseRepository)
        {
            this._useRepository = useRepository;
            var coreAssemblyDirectoryPath = typeof(RabbitMQRecevieCallBackAppService).GetAssembly().GetDirectoryPathOrNull();
            _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath);
            _projectBaseRepository = projectBaseRepository;
        }

        public async Task<RabbitMqRecevieCallBackOutput> RabbitMqRecevieCallBack(RabbitMqRecevieCallBackInput input)
        {
            //var callback = RabbitMqRecevieCallBackFactory.CreateCallBack(input.MessageType);
            //var result = await callback.RabbitMqRecevieCallBack(input);


            var framGovUrl = _appConfiguration["RabbitMQ:frmsgovUrl"];

            var requestUrl = $"{framGovUrl}/api/services/app/project/GetProjectBudgetForEdit";
            var param = new Dictionary<string, string>()
                {
                  {"id", input.Parameter}
                 };
            var result = await DoPost(requestUrl, param);

            try
            {
                var projectService = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectAppService>();
                var projectModel = JObject.Parse(result);
                var issuccess = false;
                if (bool.TryParse(projectModel["success"].ToString(), out issuccess) && issuccess)
                {
                    var data = projectModel["result"];
                    var baseOutPut = data["baseOutput"];
                    await projectService.CreateAsync(new CreateOrUpdateProJectBudgetManagerInput()
                    {
                        ProjectName = baseOutPut["projectName"].ToString(),
                        ProjectCode = baseOutPut["projectCode"].ToString(),
                        Days = int.Parse(baseOutPut["days"].ToString()),
                        Gov_Code = "FRMS",
                        Gov_ProjectId = Guid.Parse(baseOutPut["id"].ToString()),
                        FlowId = Guid.Parse("27D705F7-A953-4F2C-9A4A-E68F7C085DA3"),
                        FlowTitle = $"【财评中心】{baseOutPut["projectName"].ToString()}",

                    });
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            var ret = new RabbitMqRecevieCallBackOutput();
            ret.Result = RabbitMqCallBackResultType.Succesefull;
            return ret;

        }

        [AbpAuthorize]
        public async Task SendProjectAuditResultToFRMS(Guid projectId)
        {
            var projectModel = await _projectBaseRepository.GetAsync(projectId);
            if (projectModel.Gov_ProjectId.HasValue && !projectModel.Gov_Code.IsNullOrWhiteSpace())
            {
                var queueName = projectModel.Gov_Code;
                var obj = new { MessageType = "2", Parameter = projectModel.Gov_ProjectId.ToString() };
                var message = JToken.FromObject(obj);
                RabbitMqPublish.PushInfo(queueName, message.ToString());
            }
        }

        /// <summary>
        /// HttpClient实现Post请求
        /// </summary>
        private async Task<string> DoPost(string url, IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            var result = "";
            try
            {

                //设置HttpClientHandler的AutomaticDecompression
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                //创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    //使用FormUrlEncodedContent做HttpContent
                    var content = new FormUrlEncodedContent(nameValueCollection);

                    //await异步等待回应

                    var response = await http.PostAsync(url, content);
                    //确保HTTP成功状态值
                    //response.EnsureSuccessStatusCode();
                    //await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                    result = await response.Content.ReadAsStringAsync();

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }




















    }
}
