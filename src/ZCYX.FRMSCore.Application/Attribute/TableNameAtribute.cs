using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore
{
    public class TableNameAtribute : Attribute
    {
        public string Name { get; set; }

        public TableNameAtribute(string name)
        {
            Name = name;
        }
    }
}
