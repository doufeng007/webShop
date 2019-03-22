using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore
{
    public class LogColumnAttribute : Attribute
    {
        public LogColumnAttribute() { }
        public LogColumnAttribute(string name) { Name = name; }
        public LogColumnAttribute(string name, bool log) { Name = name; IsLog = log; }
        public string Name { get; set; }

        public bool IsNameField { get; set; } = false;


        public bool IsLog
        {
            get; set;
        } = true;
    }
}
