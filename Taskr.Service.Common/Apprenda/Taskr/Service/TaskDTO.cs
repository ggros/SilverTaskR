namespace Apprenda.Taskr.Service
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
        #region Fields
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
        private IList<TagDTO> tags = new List<TagDTO>();
        #endregion

        #region Properties
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
        #endregion

        #region Mapping Methods
        public Task MapTo()
        {
            Task instance = new Task(subject);
            instance.Id = Guid.NewGuid();
            instance.Priority = (TaskPriority)priority;
            instance.DueDate = dueDate;
            instance.Description = description;

            return instance;
        }

        public void MapInto(Task instance)
        {
            instance.Subject = subject;
            instance.Description = description;
            instance.DueDate = dueDate;
            instance.Priority = (TaskPriority)priority;
        }

        public void MapFrom(Task instance)
        {
            id = instance.Id;
            subject = instance.Subject;
            dueDate = instance.DueDate;
            description = instance.Description;
            priority = (TaskPriorityDTO)instance.Priority;
        }

        public static TaskDTO StaticMapFrom(Task instance)
        {
            if (null == instance) { return null; }

            TaskDTO dto = new TaskDTO();
            dto.MapFrom(instance);
            return dto;
        }
        #endregion

        public override string ToString()
        {
            return string.Format
            (
                "Id: '{0}'\nSubject: '{1}'\nDescription: '{2}'\nDue Date: '{3}'\nPriority: '{4}'\n",
                id,
                subject,
                description,
                dueDate,
                priority
            );
        }
    }
}
