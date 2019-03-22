using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Project
{

    [AutoMap(typeof(ConstructionOrganizations))]
    public class CreateOrUpdateConstructionOrganizationInput
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(150)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string ContactUser { get; set; }

        [StringLength(50)]
        public string ContactTel { get; set; }
    }


    [AutoMap(typeof(ConstructionOrganizations))]
    public class ConstructionOrganizationsDto: EntityDto
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(150)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string ContactUser { get; set; }

        [StringLength(50)]
        public string ContactTel { get; set; }
    }
}
