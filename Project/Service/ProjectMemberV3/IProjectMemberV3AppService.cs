using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Project.Service.ProjectMemberV3.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IProjectMemberV3AppService : IApplicationService
    {

        #region 项目评审人员的增删查改
        /// <summary>
        /// 设置项目的事项分配
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAsync(CreateOrUpdateProjectMemberInput input);
        
        #endregion

    }




}
