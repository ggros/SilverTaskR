namespace Apprenda.Taskr
{
    using System;

    public sealed class Constants
    {
        private Constants() { }

        /*Securable Names*/
        /// <summary>
        /// The SecurableChangePriority constant allows for changing priorities.
        /// By default tasks are created with a given priority but only
        /// those users that are authorized to change a priority will be
        /// able to do so. This is achived using securables.
        /// </summary>
        public const string SecurableChangePriority = "Change Priority";

        /// <summary>
        /// The SecurableDeleteTask constant allows for deleting tasks.
        /// Only users with this securable will be able to delete tasks,
        /// otherwise they will only be able to create or updata tasks.
        /// </summary>
        public const string SecurableDeleteTask = "Delete Tasks";

        /// <summary>
        /// The SecurableModifyTags constant allows for modification of tags.
        /// By default users can only create tasks but not tags. Only the 
        /// users that are authorized to modify tags will be able to create,
        /// update or delete tags. Otherwise users will only be able to 
        /// assign existing tags to a task.
        /// </summary>
        public const string SecurableModifyTags = "Modify Tags";

        /// <summary>
        /// The FeatureNumberOfTasks constant defines the feature that 
        /// controls the number of tasks that a given user can have.
        /// </summary>
        public const string FeatureNumberOfTasks = "Number of Tasks";

        /// <summary>
        /// The FeaturePriorityManagement constant defines the feature that 
        /// controls wether priority management is enabled or not.
        /// </summary>
        public const string FeaturePriorityManagement = "Priority Management";
    }
}
