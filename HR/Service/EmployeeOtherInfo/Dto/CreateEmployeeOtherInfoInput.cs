using Abp.AutoMapper;

namespace HR
{
    [AutoMapTo(typeof(EmployeeOtherInfo))]
    public class CreateEmployeeOtherInfoInput
    {
        #region 表字段
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        #endregion
    }
}