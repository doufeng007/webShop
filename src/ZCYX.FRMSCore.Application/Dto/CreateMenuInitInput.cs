using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Dto
{
    public class CreateMenuInitInput<T>
    {
        public long MenuId { get; set; }

        public T List { get; set; }

        public bool HasComplateInit { get; set; }
    }
}
