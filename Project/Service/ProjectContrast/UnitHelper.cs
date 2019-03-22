using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IUnitHelper
    {
        BsonDocument ChangUnit();
    }

    public class UnitHelper
    {
        public static string[] unitArry10 = new string[] { "10m", "10m2", "10m3", "10kg" };
        public static string[] unitArry100 = new string[] { "100m", "100m2", "100m3", "100kg" };
        public static string[] unitArry1000 = new string[] { "1000m", "1000m2", "1000m3", "1000mm3" };
        public static bool ConverUnit(string unit, out int multiple)
        {
            var needChange = true;
            multiple = 1;
            if (unitArry10.Contains(unit))
            {
                multiple = 10;
            }
            else if (unitArry100.Contains(unit))
            {
                multiple = 100;
            }
            else if (unitArry1000.Contains(unit))
            {
                multiple = 1000;
            }
            else
            {
                needChange = false;
            }
            return needChange;
        }
    }
}
