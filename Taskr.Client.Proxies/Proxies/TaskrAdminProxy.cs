namespace Apprenda.Taskr.Client
{

    using System;
    using System.Collections.Generic;
    using Apprenda.ServiceModel;

    public class TaskrAdminProxy : FailSafeClient<ITaskrAdminService>, ITaskrAdminService
    {
        #region ITaskrAdminService Members

        public Guid SaveTag(TagDTO tag)
        {
            return base.Channel.SaveTag(tag);
        }

        public bool DeleteTag(Guid tagId)
        {
            return base.Channel.DeleteTag(tagId);
        }

        public IList<TagDTO> ListTags()
        {
            return base.Channel.ListTags();
        }

        #endregion
    }
}
