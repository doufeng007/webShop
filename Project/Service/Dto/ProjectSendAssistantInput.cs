using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    public class ProjectSendAssistantInput
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 1送审金额  2审定金额
        /// </summary>
        private int _showField = 1;
        public int ShowField
        {
            get { return _showField; }
            set { _showField = value; }
        }
    }

    public class ProjectCountAssistantInput
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int ProjectType { get; set; }

        /// <summary>
        /// 1在审  2已审
        /// </summary>
        private int _status = 1;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
    }
}
