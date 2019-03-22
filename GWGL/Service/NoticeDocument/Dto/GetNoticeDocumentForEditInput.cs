using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Validation;
using ZCYX.FRMSCore.Application.Dto;
using Abp.WorkFlow;
using Project;

namespace GWGL
{


    public class GetNoticeDocumentListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {

        public bool? IsNeedRes { get; set; }

        public string StepId { get; set; }

        /// <summary>
        ///  0 所有数据 1 公文拟稿 2 发行稿 3 公文核稿 4已核公文  5 我的公文
        /// </summary>
        public int SearchType { get; set; } = 0;


        public string GW_DocumentTypeIds { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime DESC";
            }
        }
    }


    public class NoticeDocumentListOutput : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }


        public string Title { get; set; }


        public bool IsNeedRes { get; set; }


        public string StepName { get; set; }


        public string Query { get; set; }

        /// <summary>
        /// 发文机关
        /// </summary>
        public string DispatchUnitName { get; set; }

        public string DispatchCode { get; set; }

        /// <summary>
        /// 公文类别
        /// </summary>
        public string DocumentTyepName { get; set; }

        /// <summary>
        /// 发文人名称
        /// </summary>
        public string PubilishUserName { get; set; }

        /// <summary>
        /// 拟文时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        public NoticeDocumentBusinessType NoticeDocumentBusinessType { get; set; }

        public EmergencyDegreeProperty? Urgency { get; set; }


        public string Urgency_Name { get; set; }


        public RankProperty? SecretLevel { get; set; }


        public string SecretLevel_Name { get; set; }


        public Guid GW_DocumentTypeId { get; set; }

        public string GW_DocumentTypeName { get; set; }

    }


    public class GetNoticeDocumentForEditInput
    {
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// 获取发文实体
    /// </summary>
    public class GetNoticeDocumentForEditOutput : WorkFlowTaskCommentResult
    {


        public Guid Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        public int NoticeType { get; set; }


        public long? DispatchUnit { get; set; }



        /// <summary>
        /// 发文时间
        /// </summary>
        public DateTime DispatchTime { get; set; }


        public int PrintNum { get; set; }

        /// <summary>
        /// 发文文号
        /// </summary>
        public string DispatchCode { get; set; }


        public EmergencyDegreeProperty? Urgency { get; set; }


        public string Urgency_Name { get; set; }


        public RankProperty? SecretLevel { get; set; }


        public string SecretLevel_Name { get; set; }



        public string ReceiveId { get; set; }


        /// <summary>
        /// 抄送人
        /// </summary>
        public string ReceiveName { get; set; }



        public string Reason { get; set; }


        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }


        public bool IsNeedRes { get; set; }


        public int Status { get; set; }




        public Guid? ProjectId { get; set; }


        public List<FileUploadFiles> Files { get; set; }


        /// <summary>
        /// 发文人名称
        /// </summary>
        public string PubilishUserName { get; set; }

        /// <summary>
        /// 主送人
        /// </summary>
        public string MainReceiveName { get; set; }


        /// <summary>
        /// 公文类别
        /// </summary>
        public int? DocumentTyep { get; set; }

        public string DocumentTyepName { get; set; }


        /// <summary>
        /// 发文机关
        /// </summary>
        public string DispatchUnitName { get; set; }


        /// <summary>
        /// 回复内容
        /// </summary>
        public string ReplyContent { get; set; }


        public NoticeDocumentBusinessType NoticeDocumentBusinessType { get; set; } = new NoticeDocumentBusinessType();


        /// <summary>
        /// 批文签发内容
        /// </summary>
        public DispatchPublishOutput DispatchMessage { get; set; }




        public bool IsNeedAddWrite { get; set; }



        public int AddType { get; set; }


        public int WriteType { get; set; }

        public string AddWriteUsers { get; set; }


        public string AddWriteOrgIds { get; set; }


        public string AddWriteOrgIdName { get; set; }

        public Guid? GW_DocumentTypeId { get; set; }


        public string GW_DocumentTypeName { get; set; }


        /// <summary>
        /// 校对人
        /// </summary>
        public long? CheckUser { get; set; }


        public string CheckUser_Name { get; set; }



        public GetNoticeDocumentForEditOutput()
        {
            this.Files = new List<FileUploadFiles>();
        }
    }




}
