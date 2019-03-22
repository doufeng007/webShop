using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.IM
{
    public class IM_Access_TokenReturnDto
    {
        public string access_token { get; set; }


        public int expires_in { get; set; }


        public string application { get; set; }

    }


}
