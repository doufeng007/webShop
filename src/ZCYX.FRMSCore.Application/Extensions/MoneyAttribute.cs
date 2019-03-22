using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZCYX.FRMSCore.Extensions
{
    public class MoneyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                decimal d;
                if (decimal.TryParse(value.ToString(), out d) && d > 0)
                    return true;
            }
            return false;
        }
    }
}
