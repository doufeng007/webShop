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
using Abp.Application.Services;

namespace SearchAll
{
    [RemoteService(IsEnabled = false)]
    public class ImSearchAppService : ApplicationService
    {

        private readonly IConfigurationRoot _appConfiguration;
        private ElasticClient client = null;
        public ImSearchAppService(IHostingEnvironment env)
        {

            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
            var url = _appConfiguration["ElasticClient:Url"];
            var node = new Uri(url);
            var settings = new ConnectionSettings(node);
            client = new ElasticClient(settings);
            if (!client.IndexExists("search-im").Exists) { 
            var descriptor = new CreateIndexDescriptor("search-im")
    .Mappings(ms => ms
        .Map<ImSearch>(m => m.Properties(ps=>ps.Text(s=>s.Name(x=>x.To).Fielddata(true))))
    );
                client.CreateIndex(descriptor);
            }
        }

        public async Task<List<ImSearch>> GetListByIds(Guid[] input)
        {
            if (input == null && input.Count() == 0)
                return new List<ImSearch>();

            var mustQuerys = new List<Func<QueryContainerDescriptor<ImSearch>, QueryContainer>>();
            mustQuerys.Add(mt => mt.Ids(tm =>tm.Values(input)));
            var response = client.Search<ImSearch>(x => x.Index("search-im").MatchAll()
            .Query(q => q.Bool(b => b.Must(mustQuerys))).Size(100));
            StringBuilder swere = new StringBuilder();
            var list = response.Hits.OrderByDescending(x => x.Source.CreationTime).Select(x => x.Source).ToList();
            return list;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<ImSearch>> GetList(GetImSearchListInput input)
        {
            var mustQuerys = new List<Func<QueryContainerDescriptor<ImSearch>, QueryContainer>>();
            if (!string.IsNullOrEmpty(input.Type))
                mustQuerys.Add(mt => mt.Term(tm => tm.Field(fd => fd.Type).Value(input.Type)));
            if (!string.IsNullOrEmpty(input.To))
                mustQuerys.Add(mt => mt.Term(tm => tm.Field(fd => fd.UserId == AbpSession.UserId.Value)) || mt.Term(tm => tm.Field(fd => fd.To).Value(input.To)));
            if (!string.IsNullOrEmpty(input.SearchKey)) { 
                mustQuerys.Add(mt => mt.Match(qs => qs.Field(fd=>fd.Msg).Query("*" + input.SearchKey + "*")) || mt.Match(qs => qs.Field(fd=>fd.FileName).Query("*" + input.SearchKey + "*")));
            }
            var response = client.Search<ImSearch>(x => x.Index("search-im").Query(q => q.Bool(b => b.Must(mustQuerys)))
               .Size(input.MaxResultCount)
                .From(input.SkipCount)
                .Sort(st => st.Descending(d => d.CreationTime)));
            StringBuilder swere = new StringBuilder();
            var list = response.Hits.OrderByDescending(x => x.Source.CreationTime).Select(x => x.Source).ToList();
            return new PagedResultDto<ImSearch>((int)response.Total, list);
        }


        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<List<ImSearchCount>> GetSearchList(GetImSearchInput input)
        {
            var mustQuerys = new List<Func<QueryContainerDescriptor<ImSearch>, QueryContainer>>();

            if (!string.IsNullOrEmpty(input.To))
                mustQuerys.Add(mt => mt.Term(tm => tm.Field(fd => fd.UserId == AbpSession.UserId.Value)) || mt.Term(tm => tm.Field(fd => string.Join(',', input.To).Contains(fd.To))));
            if (!string.IsNullOrEmpty(input.SearchKey)) {
                mustQuerys.Add(mt => mt.Match(qs => qs.Field(fd => fd.Msg).Query("*" + input.SearchKey + "*")) || mt.Match(qs => qs.Field(fd => fd.FileName).Query("*" + input.SearchKey + "*")));
            }
            var response = client.Search<ImSearch>(x => x.Index("search-im").Query(q => q.Bool(b => b.Must(mustQuerys))).Aggregations(a => a.Terms("all_interests", st => st.Field(o => o.To))));
            var allAggs = response.Aggs.Terms("all_interests");
            if (allAggs == null)
                return new List<ImSearchCount>();
            var list = new List<ImSearchCount>();
            foreach (var item in allAggs.Buckets)
            {
                list.Add(new ImSearchCount() {To=item.Key,Count=item.DocCount });
            }
            return list;
        }


        public async Task Create(ImSearchCreateInput input)
        {
            var model = new ImSearch();
            input.MapTo(model);
            var response2 = client.Index(model, idx => idx.Index("search-im"));
        }



        public async Task Delete(EntityDto<Guid> input)
        {

            DocumentPath<ImSearch> docPath = new DocumentPath<ImSearch>(
                                                                     input.Id
                                                                      );

            IDeleteResponse delResponse = client.Delete<ImSearch>(
                                                                docPath,
                                                                p => p
                                                                .Index("search-im")
                                                                );
        }

    }
}