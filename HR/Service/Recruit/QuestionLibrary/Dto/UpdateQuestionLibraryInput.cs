using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;

namespace HR
{
    public class UpdateQuestionLibraryInput : QuestionLibrary
    {
        public List<GetAbpFilesOutput> FileList { get; set; }


        public UpdateQuestionLibraryInput()
        {
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }
    }
}