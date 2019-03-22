using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.IM
{
    public class IMResponeDto
    {
        public string action { get; set; }

        public string application { get; set; }


        public string uri { get; set; }

        public dynamic entities { get; set; }

        public dynamic data { get; set; }

        public long timestamp { get; set; }

        public int duration { get; set; }

        public string organization { get; set; }

        public string applicationName { get; set; }

    }
}
