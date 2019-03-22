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
using Abp.Configuration;

namespace ZCYX.FRMSCore.Application
{
    public class SettingAppService : FRMSCoreAppServiceBase, ISettingAppService
    { 
        private readonly IRepository<Setting, long> _repository;
        private readonly ISettingManager _setting;

        public SettingAppService(IRepository<Setting, long> repository, SettingManager setting)
        {
            this._repository = repository;
            _setting = setting;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<List<SettingOutputDto>> GetList(GetSettingInput input)
        {
            var query = from a in _setting.GetAllSettingValuesForApplication()
                        select new SettingOutputDto()
                        {
                            Name = a.Name,
                            Value = a.Value
                        };
            return query.ToList();
        }
		
		public async Task Update(UpdateSettingInput input)
        {
            foreach (var item in input.updateSettings)
            {
                _setting.ChangeSettingForApplication(item.Name,item.Value);
            }
        }


    }
}