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
using Abp.File;
using Abp.Runtime.Caching;
using Abp.WorkFlow;
using Newtonsoft.Json;
using Train.Enum;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;

namespace Train
{
    public class CourseSettingAppService : FRMSCoreAppServiceBase, ICourseSettingAppService
    { 
        private readonly IRepository<CourseSetting, Guid> _repository;
        private readonly ICacheManager _cacheManager;

        public CourseSettingAppService(IRepository<CourseSetting, Guid> repository
            , ICacheManager cacheManager

        )
        {
            this._repository = repository;
            _cacheManager = cacheManager;
        }
        /// <summary>
        /// 设置视频设置
        /// </summary>
        /// <returns></returns>
        public async Task<CourseSet> Get()
        {
            try
            {
                var result = new CourseSet();
                var cachemodel = await _cacheManager.GetCache("CourseSetting").GetAsync("CourseSettingKey",
                    async () => await _repository.GetAll().FirstOrDefaultAsync());
                if (cachemodel != null)
                {
                    result = JsonConvert.DeserializeObject<CourseSet>(cachemodel.Content);
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// 获取视频配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Set(CourseSet input)
        {
            input.ViewingRatio = input.ViewingRatio <= 0 ? 1 : input.ViewingRatio > 100 ? 100 : input.ViewingRatio;
            var model =await _repository.GetAll().FirstOrDefaultAsync();
            if (model != null)
            {
                model.Content = JsonConvert.SerializeObject(input);
                await _repository.UpdateAsync(model);
            }
            else
            {
                model = new CourseSetting()
                {
                    Id = Guid.NewGuid(),
                    Content = JsonConvert.SerializeObject(input)
                };
                await _repository.InsertAsync(model);
            }
            //更新缓存
            await _cacheManager.GetCache("CourseSetting").SetAsync("CourseSettingKey", model);
        }

        /// <summary>
        /// 根据必修选修专业难度返回对应的课程设置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isSpecial"></param>
        /// <returns></returns>
        public async Task<CourseSetScore> GetSetVal(CourseLearnType type, bool isSpecial)
        {
            CourseSetScore set;
            var courseSet = await Get();
            if (type == CourseLearnType.Must || type == CourseLearnType.MustAll)
            {
                set = isSpecial ? courseSet.RequiredMajor : courseSet.RequiredCommonly;
            }
            else
            {
                set = isSpecial ? courseSet.ElectiveMajor : courseSet.ElectiveCommonly;
            }
            return set;
        }
    }
}