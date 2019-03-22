using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IBackgroudWorkJobWithHangFire
    {
        void CreateJobForProjectForceSubmit(Guid projectId);

        void RemoveIfExistsJobForProjectForceSubmit(Guid projectId);
        /// <summary>
        /// 根据项目工时自动创建工作项到期待办提醒
        /// </summary>
        /// <param name="porjectId"></param>
        /// <param name="taskId"></param>
        void CreatProjectProgressTask(Guid instancId, Guid taskId, int hours);
        void RemoveProjectProgressTask(Guid instancId);
    }
}
