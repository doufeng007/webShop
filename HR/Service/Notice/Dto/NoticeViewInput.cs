using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.ComponentModel.DataAnnotations;


namespace HR
{
    public class NoticeViewInput
    {

        public Guid LogId { get; set; }

        public Guid TextId { get; set; }
    }
}