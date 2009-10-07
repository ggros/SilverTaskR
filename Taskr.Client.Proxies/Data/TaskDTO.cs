namespace Apprenda.Taskr.Client
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// This class represents a task or an activity. These are the main
    /// entities of the system.
    /// </summary>
    [DataContract(Name="Task", Namespace="Apprenda.Taskr")]
    public class TaskDTO
    {
        [DataMember(Name = "Id")]
        private Guid id;
        [DataMember(Name = "Subject")]
        private string subject;
        [DataMember(Name = "Description")]
        private string description;
        [DataMember(Name = "DueDate")]
        private DateTime dueDate;
        [DataMember(Name = "Priority")]
        private TaskPriorityDTO priority;
        [DataMember(Name = "Tags")]
        private IList<TagDTO> tags;

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        public TaskPriorityDTO Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public IList<TagDTO> Tags
        {
            get { return tags; }
            set { tags = value; }
        }
    }
}
