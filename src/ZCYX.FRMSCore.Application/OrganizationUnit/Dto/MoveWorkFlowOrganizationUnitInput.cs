using System.ComponentModel.DataAnnotations;

namespace ZCYX.FRMSCore.Application
{
    public class MoveWorkFlowOrganizationUnitInput
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        public long? NewParentId { get; set; }
    }
}