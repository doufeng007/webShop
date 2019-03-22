using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    public enum ProjectAuditGroupRoleEnum
    {
        项目负责人 = 1,
        联系人一 = 2,
        联系人二 = 3,
    }

    public enum ArchivesTypeEnum
    {
        项目 = 1,
        公文 = 2
    }


    public enum SecrecyLevelEnum
    {
        一般 = 1,
        机密 = 2
    }


    public enum ProjectNatureEnum
    {
        新建 = 1,
        改建 = 2,
        扩建 = 3,
    }
}
