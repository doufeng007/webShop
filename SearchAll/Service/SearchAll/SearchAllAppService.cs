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
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using Microsoft.Extensions.Configuration;
using ZCYX.FRMSCore.Configuration;
using Microsoft.AspNetCore.Hosting;
using Nest;

namespace SearchAll
{
    public class SearchAllAppService : FRMSCoreAppServiceBase, ISearchAllAppService
    {

        private readonly IConfigurationRoot _appConfiguration;
        private ElasticClient client = null;
        public SearchAllAppService(IHostingEnvironment env)
        {
         
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
            var url = _appConfiguration["ElasticClient:Url"];
            var node = new Uri(url);
            var settings = new ConnectionSettings(node);
            client = new ElasticClient(settings);
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<SearchAllListOutputDto>> GetList(GetSearchAllListInput input)
        {
            var response = client.Search<Search>(x => x.Analyzer("ik_max_word").Index("search-index").Query(q => q.MultiMatch(qs => qs.Query("*"+input.SearchKey + "*").Fields(new[] {"title","message","attachment.content" })))
                .Highlight(h => h.PreTags("<span style='color:red;'>")//改成strong以符合博客园的样式
                                .PostTags("</span>")//
                                .Fields(
                                    hf => hf.Field(p => p.Title)//标题高亮
                                            .HighlightQuery(q => q
                                                             .Match(m => m
                                                             .Field(p => p.Title)
                                                             .Query(input.SearchKey)
                                   )
                               ),
                                   hf => hf.Field(p => p.Message)//内容高亮
                                           .HighlightQuery(q => q
                                                            .Match(m => m
                                                            .Field(p => p.Message)
                                                            .Query(input.SearchKey)
                                    )
                               ),
                                   hf => hf.Field(p => p.Attachment.Content)//内容高亮
                                           .HighlightQuery(q => q
                                                            .Match(m => m
                                                            .Field(p => p.Attachment.Content)
                                                            .Query(input.SearchKey)
                                    )
                               ))
                           ).Size(input.MaxResultCount)
                .From(input.SkipCount)
                .Sort(st => st.Descending(d => d.CreateTime)));
          //  response.Total;
            StringBuilder swere = new StringBuilder();
            var list = new List<SearchAllListOutputDto>();
           
            response.Hits.OrderByDescending(x=>x.Source.CreateTime).ToList().ForEach(x =>
            {
                if (x.Highlights.Count > 0)
                {
                    var model = new SearchAllListOutputDto();
                    model.Link = x.Source.Link;
                    model.CreateTime = x.Source.CreateTime;
                    model.Title = x.Highlights == null ? x.Source.Title : x.Highlights.Keys.Contains("title") ? string.Join("", x.Highlights["title"].Highlights) : x.Source.Title;
                    model.Message = x.Highlights == null ? x.Source.Title : x.Highlights.Keys.Contains("message") ? string.Join("", x.Highlights["message"].Highlights) : x.Source.Message;
                    if (x.Source.Type == 1)
                    {
                        x.Source.Attachment.Content = x.Highlights == null ? x.Source.Attachment.Content : x.Highlights.Keys.Contains("content") ? string.Join("", x.Highlights["content"].Highlights) : x.Source.Attachment.Content;
                        model.Message = x.Source.Attachment.Content;
                    }
                    list.Add(model);
                }
            });
            return new PagedResultDto<SearchAllListOutputDto>((int)response.Total,list);
            //return list;
        }

        public async Task Create(SearchInput input)
        {
            var model = new Search();
            var fileId = Guid.Empty;
            if (input.Id.HasValue)
                model.Id = input.Id.Value;
            else
                model.Id = Guid.NewGuid();
            model.Type = 0;
            model.Message = input.Message;
            model.Title = input.Title;
            model.Link = input.Link;
            model.CreateTime = DateTime.Now;
            var response2 = client.Index(model, idx => idx.Index("search-index"));
        }


        public async Task CreateOffice(Search input)
        {
            var data = new Search();
            data.Id = input.Id;
            data.Link = "/api/AbpFile/Show?id=" + input.Id;
            data.Title = input.Title;
            data.Type =1;
            data.CreateTime = DateTime.Now;
            data.Content = Convert.ToBase64String(System.IO.File.ReadAllBytes(input.Content));            
            var response2 = client.Index(data, idx => idx.Index("search-index").Pipeline("attachments"));
        }

        public async Task Update(SearchInput input)
        {
            var model = new Search();
            var fileId = Guid.Empty;
            if (input.Id.HasValue)
                model.Id = input.Id.Value;
            else
                model.Id = Guid.NewGuid();
            model.Type = 0;
            model.Message = input.Message;
            model.Title = input.Title;
            model.Link = input.Link;
            model.CreateTime = DateTime.Now;
            var response2 = client.Index(model, idx => idx.Index("search-index"));
        }


        public async Task Delete(EntityDto<Guid> input)
        {

            DocumentPath<Search> docPath = new DocumentPath<Search>(
                                                                     input.Id
                                                                      );

            IDeleteResponse delResponse = client.Delete<Search>(
                                                                docPath,
                                                                p => p
                                                                .Index("search-index")
                                                                );
        }

    }
}