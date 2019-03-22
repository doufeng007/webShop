using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Application
{

    public class OrgPostAllNameDto
    {

        public string Id { get; set; }

        public Guid? OrgPostId { get; set; }

        public long? OrgId { get; set; }

        public Guid? PostId { get; set; }

        /// <summary>
        /// 岗位级别，值越小级别越高
        /// </summary>
        public Level? Level { get; set; }
        public string Pid { get; set; }

        /// <summary>
        ///  true为职位 false为部门
        /// </summary>
        public bool IsPost { get; set; }

        public string Name { get; set; }


    }





}
