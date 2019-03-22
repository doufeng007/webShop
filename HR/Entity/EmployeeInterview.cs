using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    /// <summary>
    /// 员工面试信息登记
    /// </summary>
    [Table("EmployeeInterview")]
    public class EmployeeInterview : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public long Department { get; set; }
        public Guid Job { get; set; }
        public string JobName { get; set; }
        public string Name { get; set; }

        public string Sex { get; set; }

        public int Age { get; set; }

        public DateTime Birthday { get; set; }

        public string Nature { get; set; }

        public string HomeTown { get; set; }

        public int Hight { get; set; }

        public string Blood { get; set; }

        public string Health { get; set; }

        public string Party { get; set; }

        public string Marray { get; set; }

        public string Education { get; set; }
        public string School { get; set; }

        public string OldCompany { get; set; }

        public string OldCompanyType { get; set; }
        public string Professional { get; set; }
        public string Duty { get; set; }
        public string StartWork { get; set; }
        public string IdCard { get; set; }
        public string Address { get; set; }
        public string OldCompanyRelation { get; set; }
        public string AddressPost { get; set; }
        public string HomeAddress { get; set; }
        public string HomeAddressPost { get; set; }
        public string DocmentAddress { get; set; }
        public string HomeTel { get; set; }
        public string Phone { get; set; }

        public string WarningPeople { get; set; }
        public string WarningPeoplePhone { get; set; }
        public string ForeignLevel { get; set; }
        public string Specialty { get; set; }
        public bool CanDistribution { get; set; }
        public string Sick { get; set; }
        public string Crime { get; set; }
        public string Insurance { get; set; }
        public int Status { get; set; }
        public int? TenantId { get; set; }

    }
}
