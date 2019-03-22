using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    /// <summary>
    /// 工料机
    /// </summary>
    public class CJZGLJDto
    {
        public string unit { get; set; }

        public string material_single_price { get; set; }

        public string material_name { get; set; }

        public string fixed_price { get; set; }

        public string specification { get; set; }

        public string code { get; set; }

        public string quantity { get; set; }


        public string project_name { get; set; }


        public string project_time { get; set; }

       

    }
}
