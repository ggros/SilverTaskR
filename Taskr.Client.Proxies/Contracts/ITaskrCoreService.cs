namespace Apprenda.Taskr.Client
{

    using System;
    using System.ServiceModel;
    using System.Collections.Generic;
    using Apprenda.SaaSGrid;
    using Apprenda.SaaSGrid.Subscription;

    /// <summary>
    /// This interface defines all core functionality to be performed by the
    /// application. 
    /// </summary>
    [ServiceContract(Name = "TaskrCore", Namespace = "urn:Apprenda.Taskr.Service")]
    public interface ITaskrCoreService : IDisposable
    {
        /// <summary>
        /// Method to save a task. This method will either create a new task
        /// or save an existing task. It will identify whether it is a new or
        /// existing task based on the Id of the task. If the id is an empty
        /// guid then it is considered to be a new task, otherwise it should
        /// try to update the existing task.
        /// </summary>
        /// <param name="task">The task to be saved</param>
        /// <returns>Returns the id of the task, this is usefull for when a 
        /// task is created.</returns>        
        [OperationContract(Action = "urn:Apprenda.Taskr.Service.SaveTask",
                           ReplyAction = "urn:Apprenda.Taskr.Service.SaveTaskReply")]
        [FaultContract(typeof(LimiterExhaustedDetail), Action = "urn:Apprenda.SaaSGrid.Fault")]
        Guid SaveTask(TaskDTO task);

        /// <summary>
        /// Method to delete a task. This method will try to delete a task
        /// with a given id. If the task does not exist the method will 
        /// return false.
        /// </summary>
        /// <param name="taskId">The id of the task being deleted.</param>
        /// <returns>Returns true if successfully delete the task, false otherwise.
        /// If trying to delete a non-existant task it will return false.</returns>
        [OperationContract(Action = "urn:Apprenda.Taskr.Service.DeleteTask",
                           ReplyAction = "urn:Apprenda.Taskr.Service.DeleteTaskReply")]
        [FaultContract(typeof(NotAuthorizedDetail), Action = "urn:Apprenda.SaaSGrid.Fault")]
        [FaultContract(typeof(LimiterExhaustedDetail), Action = "urn:Apprenda.SaaSGrid.Fault")]
        bool DeleteTask(Guid taskId);

        /// <summary>
        /// Method to retrieve a specific task. This method will retrieve a task
        /// with a given id. If the task exists then it will be retrieved fully
        /// hidrated, otherwise it will return null.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [OperationContract(Action = "urn:Apprenda.Taskr.Service.GetTask",
                           ReplyAction = "urn:Apprenda.Taskr.Service.GetTaskReply")]
        TaskDTO GetTask(Guid taskId);

        /// <summary>
        /// Method to list all tasks. This method will return a list of all the tasks
        /// in the system or an empty list if no tasks are defined.
        /// </summary>
        /// <returns>A list with the tasks defined in the system</returns>
        [OperationContract(Action = "urn:Apprenda.Taskr.Service.ListTasks",
                           ReplyAction = "urn:Apprenda.Taskr.Service.ListTasksReply")]
        IList<TaskDTO> ListTasks();
    }
}
