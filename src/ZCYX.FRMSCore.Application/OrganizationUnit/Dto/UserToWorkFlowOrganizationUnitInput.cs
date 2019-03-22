using System.ComponentModel.DataAnnotations;

namespace ZCYX.FRMSCore.Application
{
    public class UserToWorkFlowOrganizationUnitInput
    {
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        [Range(1, long.MaxValue)]
        public long OrganizationUnitId { get; set; }
    }
}