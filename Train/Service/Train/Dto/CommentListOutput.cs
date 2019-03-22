using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace Train
{
    public class CommentListOutput
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
    }
    public class CommentOutput
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
    }
}