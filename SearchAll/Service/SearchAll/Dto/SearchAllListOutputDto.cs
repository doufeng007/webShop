using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Nest;
using System;

namespace SearchAll
{
    public class Search
    {
        public Guid? Id { get; set; }
        public string User { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public string Link { get; set; }
        public int Type { get; set; }
        public Attachment Attachment { get; set; }
    }
    public class SearchAllListOutputDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreateTime { get; set; }
        public string Link { get; set; }
    }
    public class SearchInput
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
    }

}
