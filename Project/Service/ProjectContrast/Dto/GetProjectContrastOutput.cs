using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using CjzContrast;
using CjzDataBase;

namespace Project
{
    public class GetProjectContrastOutput
    {
        public Guid ProjectId { get; set; }

        public int CompareType { get; set; }

        public CjzData FileData { get; set; }


        public CtaData CompareData { get; set; }



        public GetProjectContrastOutput()
        {
            this.FileData = new CjzData();
            CompareData = new CtaData();
        }

    }


    public class ProjectCjzData
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public List<ProjectCjzLeftMenu> Menus { get; set; }




        public ProjectCjzData()
        {
            this.Menus = new List<ProjectCjzLeftMenu>();
        }

    }


    public class ProjectCjzCompareData
    {
        public ProjectCompareFileInfo File1Info { get; set; }

        public ProjectCompareFileInfo File2Info { get; set; }

        public ProjectCjzData Result { get; set; }

        public ProjectCjzCompareData()
        {
            this.Result = new ProjectCjzData();
            this.File1Info = new ProjectCompareFileInfo();
            this.File2Info = new ProjectCompareFileInfo();
        }

    }


    /// <summary>
    /// 对应 DocData  单项工程 单位工程数据， 通过他遍历加载出左侧菜单结构
    /// </summary>
    public class ProjectCjzLeftMenu
    {
        public string Id { get; set; }


        public string Text { get; set; }

        public int Level { get; set; }

        public string NodeType { get; set; }

        /// <summary>
        /// 工程类别
        /// </summary>
        public string ProjecctCategory { get; set; }

        /// <summary>
        /// 工程规模
        /// </summary>
        public string ProjectScale { get; set; }


        public string Parent { get; set; }


        public List<ProjectCjzCategoryTable> Tables { get; set; }

        public ProjectCjzLeftMenu()
        {

            this.Tables = new List<ProjectCjzCategoryTable>();
        }

    }

    public class ProjectCjzCategoryTable
    {
        public string Id { get; set; }

        public string MenuId { get; set; }

        public string Name { get; set; }


        public int MaxDeep { get; set; }



        public List<ProjectCjzCategoryTableCol> ColList { get; set; }


        public List<ProjectCjzDataRow> ORecList { get; set; }


        public List<ProjectCjzDataRow> NRecList { get; set; }

        public ProjectCjzCategoryTable()
        {
            this.ColList = new List<ProjectCjzCategoryTableCol>();
            this.ORecList = new List<ProjectCjzDataRow>();
            this.NRecList = new List<ProjectCjzDataRow>();

        }


    }


    public class ProjectCjzCategoryTableCol
    {
        public string ColName { get; set; }

        public string Id { get; set; }

        public string data { get; set; }

    }

    public class ProjectCjzDataRow
    {
        public List<ProjectCjzCellData> CellDatas { get; set; }

        private Type ItemType { get; set; }


        public string Id { get; set; }

        public int Sid { get; set; }


        public int Sort { get; set; }


        public bool HasChildRow { get; set; }


        public int ChildRowCount { get; set; }


        public string IsShow { get; set; }

        public int Deep { get; set; }

        public string XMTZ { get; set; }

        public int XMTZStatus { get; set; }

        public string MaterialName { get; set; }


        public string ProjectNo { get; set; }


        public int Status { get; set; }


        public ProjectCjzDataRow()
        {
            this.CellDatas = new List<ProjectCjzCellData>();
            this.IsShow = "showtr";
        }

        public string GetMaterialName()
        {
            if (this.CellDatas.Count > 0)
            {
                var item = this.CellDatas.FirstOrDefault(r => r.ColName == "项目名称");
                if (item == null)
                    return "";
                else
                    return item.Value;
            }
            else
            {
                return "";
            }
        }

        public string GetProjectNo()
        {
            if (this.CellDatas.Count > 0)
            {
                var item = this.CellDatas.FirstOrDefault(r => r.ColName == "项目编码");
                if (item == null)
                    return "";
                else
                    return item.Value;
            }
            else
            {
                return "";
            }
        }

        public string GetSinglePrice()
        {
            if (this.CellDatas.Count > 0)
            {
                var item = this.CellDatas.FirstOrDefault(r => r.ColName == "综合单价");
                if (item == null)
                    return "";
                else
                    return item.Value;
            }
            else
            {
                return "";
            }
        }
        public void SetSinglePriceStatus(CellDataStatus status)
        {
            if (this.CellDatas.Count > 0)
            {
                var item = this.CellDatas.FirstOrDefault(r => r.ColName == "综合单价");
                if (item != null)
                {
                    item.Status = (int)status;
                    this.Status = this.Status + (int)status;
                }
            }
        }

        public void SetProjectNameStatus(CellDataStatus status)
        {
            if (this.CellDatas.Count > 0)
            {
                var item = this.CellDatas.FirstOrDefault(r => r.ColName == "项目名称");
                if (item != null)
                {
                    item.Status = (int)status;
                    this.Status = this.Status + (int)status;
                }
            }
        }
        public void SetProjectCodeStatus(CellDataStatus status)
        {
            if (this.CellDatas.Count > 0)
            {
                var item = this.CellDatas.FirstOrDefault(r => r.ColName == "项目编码");
                if (item != null)
                {
                    item.Status = (int)status;
                    this.Status = this.Status + (int)status;
                }
            }
        }

        public void SetXMTZStatus(CellDataStatus status)
        {
            if (this.CellDatas.Count > 0)
            {
                var item = this.CellDatas.FirstOrDefault(r => r.ColName == "项目特征");
                if (item != null)
                {
                    item.Status = (int)status;
                    this.XMTZStatus = (int)status;
                    this.Status = this.Status + (int)status;
                }
            }
        }

    }

    public enum CellDataStatus
    {
        正常 = 0,
        综合单价过高 = 2,
        综合单价过低 = 4,
        项目编码异常 = 8,
        项目名称异常 = 16,
        项目特征多项 = 32,
        项目特征少项 = 64,
        项目特征数据异常 = 128,
    }


    public class ProjectCjzCellData
    {
        public string Value { get; set; }

        public int Index { get; set; }

        public int Status { get; set; }

        public string ColName { get; set; }


    }


    public class ProjectContrastResultData
    {

        public string Id { get; set; }


        public string ParentId { get; set; }

        /// <summary>
        ///  0 子节点 1  1叶子节点
        /// </summary>
        public int NodeType { get; set; }

        public string Text { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }



}