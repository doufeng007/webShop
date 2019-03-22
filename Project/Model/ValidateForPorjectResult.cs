using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    public class ValidateForPorjectResult
    {

        public string Category { get; set; }


        public string FieldName { get; set; }
    }

    public class ValidateForPorjectResultOutput
    {
        public bool IsRight { get; set; }

        public List<ValidateForPorjectResult> LackData { get; set; }
    }
}
