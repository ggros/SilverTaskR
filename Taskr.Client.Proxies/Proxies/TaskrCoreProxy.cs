namespace Apprenda.Taskr.Client
{

    using System;
    using System.Runtime.Serialization;
    using Apprenda.ServiceModel;
    using System.Collections.Generic;

    public class TaskrCoreProxy : FailSafeClient<ITaskrCoreService>, ITaskrCoreService
    {
        #region ITaskrCoreService Members

        public Guid SaveTask(TaskDTO task)
        {
            return base.Channel.SaveTask(task);
        }

        public bool DeleteTask(Guid taskId)
        {
            return base.Channel.DeleteTask(taskId);
        }

        public TaskDTO GetTask(Guid taskId)
        {
            return base.Channel.GetTask(taskId);
        }

        public IList<TaskDTO> ListTasks()
        {
            return base.Channel.ListTasks();
        }

        #endregion
    }

}
