using System;
using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;
using ZCYX.FRMSCore.Application.Dto;

namespace ZCYX.FRMSCore.Application
{
    public class GetWorkFlowOrganizationUnitUsersInput : PagedAndSortedInputDto, IShouldNormalize
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }
        public string SearchKey { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "user.Name, user.Surname";
            }
            else if (Sorting.Contains("userName"))
            {
                Sorting = Sorting.Replace("userName", "user.userName");
            }
            else if (Sorting.Contains("addedTime"))
            {
                Sorting = Sorting.Replace("addedTime", "uou.creationTime");
            }
        }
    }
    public class GetUserUnderPostSearch : PagedAndSortedInputDto
    {
        public Guid orgPostId{ get; set; }
        public string SearchKey { get; set; }
    }
}