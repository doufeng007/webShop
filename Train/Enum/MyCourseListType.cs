using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Train.Enum
{
    public enum MyCourseListType
    {
        [Description("必修课程")] RequiredCourse = 0,
        [Description("选修课程")] ElectiveCourse = 1,
        [Description("完结课程")] ComplateCourse = 2
    }
}
