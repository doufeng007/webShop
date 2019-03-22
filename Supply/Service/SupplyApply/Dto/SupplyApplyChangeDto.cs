using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace Supply
{
    public class SupplyApplyChangeDto
    {
        [LogColumn("紧急")]
        public bool IsImportant { get; set; }


        [LogColumn("申领用品")]
        public List<SupplyApplySubChangeDto> SubSubpply { get; set; }

        public SupplyApplyChangeDto()
        {
            this.SubSubpply = new List<SupplyApplySubChangeDto>();
        }


    }

    public class SupplyApplySubChangeDto
    {
        [LogColumn("主键", IsLog = false)]
        public Guid Id { get; set; }


        [LogColumn("用品名称", IsNameField = true)]
        public string Name { get; set; }

        [LogColumn("品牌型号")]
        public string Version { get; set; }

        [LogColumn("数量")]
        public int Number { get; set; }

        [LogColumn("计量单位")]
        public string Unit { get; set; }

        [LogColumn("价格")]
        public string Money { get; set; }

        [LogColumn("备注")]
        public string Des { get; set; }

        [LogColumn("期望交付时间")]
        public DateTime GetTime { get; set; }

        [LogColumn("用户")]
        public string UserId_Name { get; set; }

        [LogColumn("类别")]
        public string Type_Name { get; set; }

        [LogColumn("附件")]
        public List<AbpFileChangeDto> Files { get; set; }

        public SupplyApplySubChangeDto()
        {
            this.Files = new List<AbpFileChangeDto>();
        }
    }
}
