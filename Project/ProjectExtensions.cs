using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace Project
{
    
    public static class ProjectExtensionHelper
    {
        public static string GetMessage(this ProjectBudgetControl model)
        {
            if (model.ValidationMoney == null)
                return $"控制类型：{model.CodeName},名称:{model.Name},批准:{model.ApprovalMoney},送审:{model.SendMoney} 审定:{model.ValidationMoney} ";
            else
            {
                return $"控制类型：{model.CodeName},名称:{model.Name},批准:{model.ApprovalMoney},送审:{model.SendMoney}";
            }

        }
    }
}

