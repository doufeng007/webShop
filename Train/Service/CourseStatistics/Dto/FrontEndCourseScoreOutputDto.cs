using System;
using System.Collections.Generic;
using System.Text;

namespace Train
{
    public class FrontEndCourseScoreOutputDto
    {
        public string UserName { get; set; }
        public string Department { get; set; }
        public string Post { get; set; }

        public int LearnScore { get; set; }
        public int UnLearnScore { get; set; }
        public int ExperienceScore { get; set; }
        public int CommentScore { get; set; }

        public int AllScore { get; set; }
    }
}
