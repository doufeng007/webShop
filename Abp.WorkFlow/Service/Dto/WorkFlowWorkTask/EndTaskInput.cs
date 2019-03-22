using Abp.File;
using Abp.WorkFlow;
using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp
{
    public class EndTaskInput
    {
        public Guid TaskId { get; set; }


        public bool IsStopNotNoPass { get; set; } = false;

        /// <summary>
        /// 备注 （意见）
        /// </summary>
        public string Comment { get; set; }
        public List<GetAbpFilesOutput> FileList { get; set; }
        public EndTaskInput()
        {
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }
    }
}
