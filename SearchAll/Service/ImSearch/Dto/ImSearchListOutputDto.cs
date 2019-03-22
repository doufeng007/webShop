using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Nest;
using System;

namespace SearchAll
{

    public class ImSearch
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public string User { get; set; }
        public string To { get; set; }
        public bool RoomType { get; set; }
        public string ChatType { get; set; }
        [Text(Name = "msg", Analyzer = "ik_max_word",SearchAnalyzer = "ik_max_word")]
        public string Msg { get; set; }
        public DateTime CreationTime { get; set; }
        public string  Type { get; set; }
        [Text(Name = "filename", Analyzer = "ik_max_word", SearchAnalyzer = "ik_max_word")]
        public string  FileName { get; set; }
    }

    public class ImSearchCount
    {
        public string To { get; set; }
        public long? Count { get; set; }
    }

}
