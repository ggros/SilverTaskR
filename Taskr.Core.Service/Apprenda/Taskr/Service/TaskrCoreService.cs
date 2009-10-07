namespace Apprenda.Taskr.Service
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Apprenda.SaaSGrid;
    using Apprenda.SaaSGrid.Subscription;
    using Apprenda.SaaSGrid.Support;
    using Apprenda.Taskr.Repository;
    using System.Data.Linq;
    using System.Linq;
    using Home = global::Taskr;
    using Apprenda.Configuration;
    using Apprenda.Taskr.Service;
    using System.ServiceModel.Web;
    using System.IO;
    using System.Text;

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class TaskrCoreService : ITaskrCoreService, IClientAccessPolicy
    {
        [Log]
        public Guid SaveTask(TaskDTO task)
        {
            Guid taskId = Guid.Empty;

            if (task == null)
                throw new FaultException("Unable to save null task");
                        
            TaskrDataContext db = new TaskrDataContext(ConfigurationProvider.GetConnection("Taskr"));
            
            if (task.Id == Guid.Empty)
            {
                //If a new task is created, instruct SaaSGrid to adjust the limit counter appriopriately.
                MeterStatus result = SubscriptionContext.Instance.IncrementLimitCounter(Constants.FeatureNumberOfTasks);

                if (MeterStatus.NotExhausted == result)
                {
                    try
                    {
                        Task _task = task.MapTo();
                        db.Tasks.InsertOnSubmit(_task);
                        db.SubmitChanges();
                        taskId = _task.Id;
                    }
                    catch
                    {
                        SubscriptionContext.Instance.DecrementLimitCounter(Constants.FeatureNumberOfTasks);
                        throw;
                    }
                }
                else
                {
                    throw new LimiterExhaustedException
                    (
                        Constants.FeatureNumberOfTasks,
                        result,
                        Home.Resources.Task_MaximumLimitReached
                    );
                }
            }
            else
            {
                Task existingTask = db.Tasks.SingleOrDefault(t => t.Id == task.Id);
                task.MapInto(existingTask);
                db.SubmitChanges();
                taskId = existingTask.Id;
            }

            return taskId;
        }
        
        [Log]
        [Secured(Constants.SecurableDeleteTask)]
        [DecrementLimiter(Constants.FeatureNumberOfTasks)]
        public bool DeleteTask(Guid taskId)
        {
            if (taskId == Guid.Empty)
                throw new FaultException("Unable to delete task because the passed in id was empty");

            TaskrDataContext db = new TaskrDataContext(ConfigurationProvider.GetConnection("Taskr"));

            Task task = db.Tasks.SingleOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                throw new FaultException(string.Format("Task with id {0} not exist", taskId));
            }
                        
            try
            {
                db.Tasks.DeleteOnSubmit(task);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Log]
        public TaskDTO GetTask(Guid taskId)
        {
            if (taskId == Guid.Empty)
                throw new FaultException("Unable to retrieve task because the passed in id was empty");

            TaskrDataContext db = new TaskrDataContext(ConfigurationProvider.GetConnection("Taskr"));
            return TaskDTO.StaticMapFrom(db.Tasks.SingleOrDefault(t => t.Id == taskId));
        }

        [Log]
        public IList<TaskDTO> ListTasks()
        {
            TaskrDataContext db = new TaskrDataContext(ConfigurationProvider.GetConnection("Taskr"));
            IList<TaskDTO> result = new List<TaskDTO>();
            foreach (var item in db.Tasks.OrderBy(t => t.DueDate))
            {
                result.Add(TaskDTO.StaticMapFrom(item));
            } 

            return result;
        }

        #region IClientAccessPolicy Members

        public System.IO.Stream GetClientAccessPolicy()
        {
            const string result = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                        <access-policy>
                                            <cross-domain-access>
                                                <policy>
                                                    <allow-from http-request-headers=""*"">
                                                        <domain uri=""*""/>
                                                    </allow-from>
                                                    <grant-to>
                                                        <resource path=""/"" include-subpaths=""true""/>
                                                    </grant-to>
                                                </policy>
                                            </cross-domain-access>
                                        </access-policy>";

            if (WebOperationContext.Current != null)

                WebOperationContext.Current.OutgoingResponse.ContentType = "application/xml";

            return new MemoryStream(Encoding.UTF8.GetBytes(result));
        }

        #endregion
    }
}
