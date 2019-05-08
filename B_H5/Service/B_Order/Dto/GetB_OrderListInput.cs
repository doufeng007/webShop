using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_OrderListInput : PagedAndSortedInputDto, IShouldNormalize
    {




        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }


    public class GetUserBlanceListOutput
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public DateTime CreateTime { get; set; }


        public decimal Amout { get; set; }


        public OrderAmoutEnum InOrOut { get; set; }

        public string OrderNo { get; set; }


        public string Status { get; set; }


    }

    public class GetUserGoodPaymentListOutput : GetUserBlanceListOutput
    {

    }


    public class UserBlanceListDto
    {

        public decimal Blance { get; set; }

        public decimal Deposit { get; set; }

        public decimal GoodPayment { get; set; }

        
    }

}
