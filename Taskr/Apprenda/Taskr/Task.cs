namespace Apprenda.Taskr
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Apprenda.SaaSGrid;
    using Home = global::Taskr;
    using System.ServiceModel;

    /// <summary>
    /// This class represents a task or an activity. These are the main
    /// entities of the system.
    /// </summary>
    public partial class Task
    {
        #region Constructors
        public Task(string subject)
        {
            if (string.IsNullOrEmpty(subject))
                throw new FaultException(Home.Resources.Task_MissingSubjectNew);

            Id = Guid.NewGuid();
            this.Subject = subject;
        }
        #endregion

        #region Properties
        public TaskPriority Priority
        {
            get
            {
                if (null == RawPriority)
                {
                    return TaskPriority.Unspecified;
                }
                else
                {
                    return (TaskPriority)RawPriority;
                }
            }
            set
            {
                if (TaskPriority.Unspecified == value)
                {
                    RawPriority = null;
                }
                else
                {
                    RawPriority = (int?)value;
                }
            }
        }
        #endregion

        #region Validation
        partial void OnSubjectChanging(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new FaultException(Home.Resources.Task_MissingSubject);
            }
        }
        #endregion
    }
}
