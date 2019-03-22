using System;
using System.Collections.Generic;
using System.Text;

namespace Docment
{
    public class DocmentFlowingDto
    {
        public Guid Id { get; set; }

        public long CreatorUserId { get; set; }

        public string UserName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Des { get; set; }

        public Guid DocmentId { get; set; }
        /// <summary>
        /// 是否外部流转记录
        /// </summary>
        public bool IsOut { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class DocmentFlowingInput {
        /// <summary>
        /// 描述
        /// </summary>
        public string Des { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public string UserName { get; set; }
        public List< Guid> DocmentIds { get; set; }
    }
}
