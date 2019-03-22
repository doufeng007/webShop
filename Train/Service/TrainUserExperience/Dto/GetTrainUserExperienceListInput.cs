using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train.Enum;
using ZCYX.FRMSCore.Application.Dto;

namespace Train
{
    public class GetTrainUserExperienceListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public Guid TrainId { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
    public class GetTrainUserExperienceOrganListInput 
    {
        public Guid TrainId { get; set; }
    }
    public class GetTrainUserExperienceInput 
    {

        public Guid TrainId { get; set; }

        public TrainExperienceType ExperienceType { get; set; }
        public bool IsMy { get; set; } = false;
    }
}
