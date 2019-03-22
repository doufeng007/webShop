using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Train.Enum
{
    public enum TrainScoreFromType
    {
        [Description("学习课程")] CourseLearn = 0,
        [Description("评论课程")] CourseComment = 1,
        [Description("课程心得体会")] CourseExperience = 2,
        [Description("参与培训")] TrainLearn = 3,
        [Description("培训心得体会")] TrainExperience = 4,
    }
}
