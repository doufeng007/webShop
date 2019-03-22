using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Nest;
using System;

namespace SearchAll
{

    [AutoMapTo(typeof(ImSearch))]
    public class ImSearchCreateInput
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public string To { get; set; }
        public bool RoomType { get; set; }
        public string ChatType { get; set; }
        public string Msg { get; set; }
        public DateTime CreationTime { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
    }

}
