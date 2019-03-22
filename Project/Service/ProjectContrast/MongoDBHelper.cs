using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Bson;
using Abp.Runtime.Caching;
using ZCYX.FRMSCore.Application;
using Abp.Extensions;
using Abp.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ZCYX.FRMSCore.Configuration;

namespace Project
{
    [RemoteService(IsEnabled = false)]
    public class MongoDBHelper : ApplicationService
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        //private const string conn = "mongodb://project_cost_test:zcyx1406@192.168.0.150:27017/project_cost_test";
        private static string conn = "";

        /// <summary>
        /// 指定的数据库
        /// </summary>
        //private const string dbName = "project_cost_test";


        private const string dbName = "project_cost_test";

        /// <summary>
        /// 国家规范表
        /// </summary>
        public const string sub_regulation = "MID_regulation_sub_list";
        /// <summary>
        /// 分部分项清单表
        /// </summary>
        public const string fbfxTableName = "HISTORY_CJZ_main_sub_engineering";


        /// <summary>
        /// 分部分项清单子项表
        /// </summary>
        public const string fbfxSubTableName = "HISTORY_CJZ_sub_sub_engineering";

        /// <summary>
        /// 区域表
        /// </summary>
        private const string areaTableName = "BASE_china_area_code";

        /// <summary>
        /// 中间表
        /// </summary>
        private const string middleTableName = "MID_history_sub_engineering";


        private const string descriptionTableName = "table_description";

        /// <summary>
        /// 工料机表
        /// </summary>
        public const string labormaterialmachinecjzTableName = "HISTORY_CJZ_labor_material_machine";

        /// <summary>
        /// 工程造价汇总表
        /// </summary>
        public const string engineeringcostcollectTableName = "HISTORY_CJZ_engineering_cost_collect";

        private static MongoClient client = new MongoClient(conn);

        private static IMongoDatabase database = client.GetDatabase(dbName);

        //private static IMongoDatabase database_f = client.GetDatabase(dbName_f);

        public static List<MongonCounty_Middle> County_MiddleCache = GetMongonCounty_MiddlesFromCache();


        public static List<MongonREGULATION_Sub> REGULATION_SubCache = GetREGULATION_SubsFromCache();


        private readonly IHostingEnvironment _hostingEnv;
        private readonly IConfigurationRoot _appConfiguration;

        public MongoDBHelper(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
            _appConfiguration = AppConfigurations.Get(hostingEnv.ContentRootPath, hostingEnv.EnvironmentName, hostingEnv.IsDevelopment());
            var dbhost = _appConfiguration["App:CJZMongoDB"];
            conn = $"mongodb://project_cost_test:{dbhost}:27017/project_cost_test";
        }


        public async void Insert()
        {

            MongoClient client = new MongoClient(conn);
            var database = client.GetDatabase(dbName);
            var collection = database.GetCollection<BsonDocument>(fbfxTableName);
            var filter = new BsonDocument();
            var count = 0;
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        // process document
                        count++;
                    }
                }
            }
        }

        public async Task InsertMany(IEnumerable<BsonDocument> datas)
        {
            var collection = database.GetCollection<BsonDocument>(fbfxTableName);
            await collection.InsertManyAsync(datas);
        }

        public async Task InsertManyAsync(IEnumerable<BsonDocument> datas, string tableName)
        {
            //MongoClient client = new MongoClient(conn);
            //var database = client.GetDatabase(dbName);
            var collection = database.GetCollection<BsonDocument>(tableName);
            await collection.InsertManyAsync(datas, new InsertManyOptions() { IsOrdered = false, BypassDocumentValidation = false });
        }


        public void InsertMany(IEnumerable<BsonDocument> datas, string tableName)
        {
            var collection = database.GetCollection<BsonDocument>(tableName);
            foreach (var item in datas)
            {
                collection.InsertOne(item);
            }

        }




        public void InsertOne(BsonDocument datas, string tableName)
        {
            var collection = database.GetCollection<BsonDocument>(tableName);
            collection.InsertOne(datas);
        }


        public async Task GetFBFXQDStatus(ProjectCjzCategoryTable table, string county_or_district)
        {
            foreach (var row in table.NRecList)
            {
                if (row.ProjectNo.IsNullOrEmpty() || row.ProjectNo.Length < 12)
                    continue;
                var project_no_9 = row.ProjectNo.Substring(0, 9);
                var query = County_MiddleCache.FirstOrDefault(r => r.county_or_district == county_or_district && r.material_name == row.MaterialName && r.project_no_9 == project_no_9);
                if (query == null)
                {

                }
                else
                {
                    var lowerPrice = query.lower_threshold?.ToDecimal() ?? 0m;
                    var uppperPrice = query.upper_threshold?.ToDecimal() ?? 0m;
                    var rowSinglePrice = row.GetSinglePrice()?.ToDecimal() ?? 0m;

                    if (lowerPrice > rowSinglePrice)
                    {
                        row.SetSinglePriceStatus(CellDataStatus.综合单价过低);
                    }

                    if (uppperPrice < rowSinglePrice)
                    {
                        row.SetSinglePriceStatus(CellDataStatus.综合单价过高);
                    }
                }

            }
        }


        public async Task GetFBFXQDXMTZStatus(ProjectCjzCategoryTable table)
        {
            foreach (var row in table.NRecList)
            {
                if (row.ProjectNo.IsNullOrEmpty() || row.ProjectNo.Length < 12)
                    continue;
                var projectNo_9 = row.ProjectNo.Substring(0, 9);
                var query = REGULATION_SubCache.FirstOrDefault(r => r.project_id.ToString() == projectNo_9);
                if (query == null)
                {
                    row.SetProjectCodeStatus(CellDataStatus.项目编码异常);
                }
                else
                {
                    if (row.MaterialName != query.project_name)
                    {
                        row.SetProjectNameStatus(CellDataStatus.项目名称异常);
                    }
                    else
                    {
                        var feature_list = query.feature_of_project_list;
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(feature_list);
                        foreach (var item in result)
                        {
                            if (!row.XMTZ.Contains(item))
                            {
                                row.SetXMTZStatus(CellDataStatus.项目特征数据异常);
                                break;
                            }
                        }
                    }
                }
            }
        }




        public async Task<MongonArea> GetAreaCode(string fiter)
        {
            var ret = new MongonArea();
            var collection = database.GetCollection<BsonDocument>(areaTableName);
            if (fiter.EndsWith("市") || fiter.EndsWith("省"))
            {
                fiter = fiter.Remove(fiter.Length - 1);
            }
            var filter = new BsonDocument()
                {
                    {"city_or_state",fiter},
                };
            using (var cursor = await collection.FindAsync(filter))
            {
                var query = await cursor.FirstOrDefaultAsync();
                if (query != null)
                {
                    ret.Province = query.Elements.FirstOrDefault(r => r.Name == "province").Value.ToString();
                    ret.County_or_district = query.Elements.FirstOrDefault(r => r.Name == "county_or_district").Value.ToString();
                    ret.City_or_state = query.Elements.FirstOrDefault(r => r.Name == "city_or_state").Value.ToString();
                    ret.Area_code = query.Elements.FirstOrDefault(r => r.Name == "area_code").Value.ToString();
                    return ret;
                }
            }

            var filter2 = new BsonDocument()
                {
                    {"county_or_district",fiter},
                };
            using (var cursor = await collection.FindAsync(filter2))
            {
                var query = await cursor.FirstOrDefaultAsync();
                if (query != null)
                {
                    ret.Province = query.Elements.FirstOrDefault(r => r.Name == "province").Value.ToString();
                    ret.County_or_district = query.Elements.FirstOrDefault(r => r.Name == "county_or_district").Value.ToString();
                    ret.City_or_state = query.Elements.FirstOrDefault(r => r.Name == "city_or_state").Value.ToString();
                    ret.Area_code = query.Elements.FirstOrDefault(r => r.Name == "area_code").Value.ToString();
                    return ret;
                }
            }
            return null;
        }


        public static List<MongonREGULATION_Sub> GetREGULATION_SubsFromCache()
        {
            var cacheName = "REGULATION_Sub";
            var _cacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICacheManager>();
            return _cacheManager
               .GetCache(cacheName)
               .Get<string, List<MongonREGULATION_Sub>>("REGULATION_Sub", f => GetREGULATION_Subs());

        }

        public static List<MongonREGULATION_Sub> GetREGULATION_Subs()
        {
            var ret = new List<MongonREGULATION_Sub>();
            var collection = database.GetCollection<BsonDocument>(sub_regulation);
            var filter = new BsonDocument();
            var cursor = collection.Find(filter);
            foreach (var item in cursor.ToList())
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<MongonREGULATION_Sub>(item.ToString());
                ret.Add(obj);
            }

            return ret;
        }

        public static List<MongonCounty_Middle> GetMongonCounty_MiddlesFromCache()
        {
            var cacheName = "MongonCounty_Middle";
            var _cacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICacheManager>();
            return _cacheManager
               .GetCache(cacheName)
               .Get<string, List<MongonCounty_Middle>>("MongonCounty_Middle", f => GetMongonCounty_Middles());

        }

        public static List<MongonCounty_Middle> GetMongonCounty_Middles()
        {
            var collection = database.GetCollection<MongonCounty_Middle>(middleTableName);
            var filter = new BsonDocument();
            var cursor = collection.Find(filter);
            return cursor.ToList();
        }

    }


    public class XMTZInfo
    {
        public string Name { get; set; }

        public List<string> Value { get; set; }
    }

    public class MongonArea
    {
        public string Province { get; set; }

        public string County_or_district { get; set; }

        public string City_or_state { get; set; }

        public string Area_code { get; set; }


    }

    public class MongonREGULATION_Sub
    {
        public string _id { get; set; }

        public string project_name { get; set; }

        public string catalog_1 { get; set; }


        public string nbr_of_feature { get; set; }

        public string unit { get; set; }

        public string project_id { get; set; }

        public string feature_of_project { get; set; }

        public string feature_of_project_list { get; set; }


    }


    public class MongonCounty_Middle
    {
        public string _id { get; set; }

        public string material_name { get; set; }

        public string upper_threshold { get; set; }


        public string project_no_9 { get; set; }

        public string lower_threshold { get; set; }

        public string county_or_district { get; set; }


    }



}

