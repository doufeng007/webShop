using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class CJZSubFBFXQDDto
    {
        public string _id { get; set; }
        public string project_no { get; set; }

        public string parent_no1 { get; set; }


        public string parent_no2 { get; set; }


        public string material_name { get; set; }


        public string unit { get; set; }


        public string wastage { get; set; }


        public string quantity { get; set; }


        public string single_price { get; set; }


        public string total_price { get; set; }


        public string quantity_calculation_method { get; set; }


        public string nocalprice { get; set; }


        public string project_name { get; set; }


        public string project_id { get; set; }


        public void ChangUnit()
        {
            var multiple = 0;
            if (UnitHelper.ConverUnit(this.unit, out multiple))
            {
                var quantityF = 0f;
                if (float.TryParse(this.quantity, out quantityF))
                {
                    this.quantity = (quantityF * multiple).ToString();
                }

                var single_priceF = 0f;
                if (float.TryParse(this.single_price, out single_priceF))
                {
                    this.single_price = (single_priceF / multiple).ToString();
                }

                this.unit = this.unit.Replace(multiple.ToString(), "");
            }
            
        }

    }
}
