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
    public class GetProjectContrastInput
    {
        public string Id { get; set; }


        public string FilePath1 { get; set; }


        public string FilePath2 { get; set; }


        public Guid ProjectId { get; set; }

        public int CompareType { get; set; }


    }


    public class UpdateCompareResultRemarkInput
    {
        public Guid Id { get; set; }

        public string Remark { get; set; }
    }

    public class InsertIntoMongoDBInput
    {
        public string FilePath { get; set; }

        public bool IncludFBFXQD { get; set; }

        public bool IncludGCZJHZ { get; set; }

        public bool IncludGLJ { get; set; }

        //public string ProjectName { get; set; }

        //public int Row { get; set; }

        //public int SkipCount { get; set; }
    }

    public enum ImprotTypeEnum
    {
        分部分项清单 = 1,
        工程造价汇总 = 2,
    }


    public class GetProjectFileForCompareInput
    {
        public Guid ProjectId { get; set; }

        public int AappraisalFileType { get; set; }

    }




}