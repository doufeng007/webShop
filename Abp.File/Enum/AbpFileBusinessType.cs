using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.File
{
    public enum TurnType
    {
        源文件 = 0,
        转PDF = 1
    }
    public enum AbpFileBusinessType
    {
        代理头像 = 1,
        商品缩略图 = 2,
        商品类别图 = 3,




        提案附件 = 43,
        收文附件 = 44,

        #region 三位数

        公章图片 = 100,
        员工签名图片 = 101,
        会议资料 = 201,
        会议请假申请附件 = 202,


        #endregion

        #region 四位数

        #region HR 模块

        面试评审表 = 2001,
        员工合同 = 2002,
        员工毕业证书 = 2003,
        员工面试题 = 2004,
        员工信息登记表 = 2005,
        员工薪水核定表 = 2006,
        转正业绩资料 = 2007,

        #endregion

        #region  档案模块
        档案附件 = 5001,
        销毁证明文件 = 5002,
        移交证明文件 = 5003,
        外部借阅证明 = 5004,
        #endregion

        #region  行政模块
        车辆协议 = 7001,
        用车里程照片 = 7002,
        物业管理 = 7003,
        单位信息 = 7004,
        费用管理 = 7005,
        用车 = 7006,
        #endregion

        #region  流程
        流程意见 = 8001,
        #endregion


        #region  财务管理
        ImFile = 9000,
        借款申请 = 9001,
        付款申请 = 9002,
        报销 = 9003,
        凭据 = 9004,
        差旅费报销 = 9005,
        贷款申请 = 9006,
        收款管理 = 9007,
        邮件附件 = 9008,
        #endregion

        #endregion

        #region  五位数

        补充资料附件 = 10001,

        #region   采购
        采购发票 = 30030,
        供应商合同 = 30031,
        固化采购计划附件 = 30032,
        固化采购计划整改附件 = 30033,
        #endregion

        #region 培训管理
        培训讲师合同 = 40001,
        培训课程文件 = 40002,
        培训课程封面 = 40003,
        培训 = 40004,
        培训心得 = 40005,
        #endregion

        #region 人力资源协
        人力资源协作机构合同 = 60001,
        人力资源专家库合同 = 60002,
        人力资源法律顾问合同 = 60003,
        人力资源面试者简历 = 60010,
        人力资源面试者结果 = 60011,
        人力资源招聘人才库 = 60004,
        人力资源题库 = 60005,
        #endregion



        #endregion
    }
}
