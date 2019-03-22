using Abp.AutoMapper;
using Abp.File;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZCYX.FRMSCore.Application.Dto;

namespace Project

{
    [AutoMapFrom(typeof(ProjectRegistration))]
    public class WorkRegistrationList
    {
        public Guid Id { get; set; }

        public Guid TaskId { get; set; }

        public long SendUserId { get; set; }

        public Guid? ProjectId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        public string Code { get; set; }
        public PersonOnChargeTypeEnum PersonOnChargeType { get; set; }
        public string PersonOnChargeTypeName { get; set; }

        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserName { get; set; }

        public long UserId { get; set; }
        public Guid? StepId { get; set; }

        /// <summary>
        /// 阶段名称
        /// </summary>
        public string StepName { get; set; }
        public List<WorkRegistrationList> Nodes { get; set; }
        public List<GetAbpFilesOutput> Files { get; set; }
    }

    public class PmProjectOutput {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count{ get; set; }
    }
    public class PmOldProjectOutput
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public string  Title { get; set; }
        public string  Code { get; set; }
        public DateTime  CreateTime { get; set; }
    }
    public class GetWorkRegistrationInput : PagedAndSortedInputDto, IShouldNormalize
    {
        private Guid _projectId;

        public Guid ProjectId
        {
            get
            {
                return _projectId;
            }
            set
            {
                _projectId = value;
            }
        }

        private int _type = 1;

        public int Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public bool IncludeDetele { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }

    public class GetWorkRegistrationListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public Guid ProjectId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }

    public class GetWorkRegistrationListOldInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public Guid RegistrationId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
    public class GetPmProjectInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}