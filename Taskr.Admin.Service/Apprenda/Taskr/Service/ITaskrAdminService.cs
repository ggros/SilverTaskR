namespace Apprenda.Taskr.Service
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.ServiceModel;
    using Apprenda.SaaSGrid;

    [ServiceContract(Name = "TaskrAdmin", Namespace = "urn:Apprenda.Taskr.Service")]
    public interface ITaskrAdminService
    {
        /// <summary>
        /// Method to save a tag. This method will either create a new tag
        /// or save an existing tag. It will identify whether it is a new or
        /// existing tag based on the Id of the tag. If the id is an empty
        /// guid then it is considered to be a new tag, otherwise it should
        /// try to update the existing tag.
        /// </summary>
        /// <param name="tag">The tag to be saved</param>
        /// <returns>Returns the id of the tag, this is usefull for when a 
        /// tag is created.</returns>
        [OperationContract(Action = "urn:Apprenda.Taskr.Service.SaveTag",
                           ReplyAction = "urn:Apprenda.Taskr.Service.SaveTagReply")]
        [FaultContract(typeof(NotAuthorizedDetail), Action = "urn:Apprenda.SaaSGrid.Fault")]
        Guid SaveTag(TagDTO tag);

        /// <summary>
        /// Method to delete a tag. This method will try to delete a tag
        /// with a given id. If the tag does not exist the method will 
        /// return false.
        /// </summary>
        /// <param name="tagId">The id of the tag being deleted.</param>
        /// <returns>Returns true if successfully delete the tag, false otherwise.
        /// If trying to delete a non-existant tag it will return false.</returns>
        [OperationContract(Action = "urn:Apprenda.Taskr.Service.DeleteTag",
                           ReplyAction = "urn:Apprenda.Taskr.Service.DeleteTagReply")]
        [FaultContract(typeof(NotAuthorizedDetail), Action = "urn:Apprenda.SaaSGrid.Fault")]
        bool DeleteTag(Guid tagId);

        /// <summary>
        /// Method to list all tags. This method will return a list of all the tags
        /// in the system or an empty list if no tags are defined.
        /// </summary>
        /// <returns>A list with the tags defined in the system</returns>
        [OperationContract(Action = "urn:Apprenda.Taskr.Service.ListTags",
                           ReplyAction = "urn:Apprenda.Taskr.Service.ListTagsReply")]
        IList<TagDTO> ListTags();
    }
}
