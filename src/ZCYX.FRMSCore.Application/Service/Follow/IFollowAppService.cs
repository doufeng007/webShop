using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    public interface IFollowAppService : IApplicationService
    {	

		/// <summary>
        /// 添加一个Follow
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateFollowInput input);

    }
}