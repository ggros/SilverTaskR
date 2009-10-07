namespace Apprenda.Taskr.Service
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Apprenda.SaaSGrid.Support;
    using Apprenda.SaaSGrid;
    using System.ServiceModel;
    using Apprenda.Taskr.Repository;
    using System.Linq;
    using Apprenda.Configuration;
    using Apprenda.Taskr.Service;
    using System.ServiceModel.Web;
    using System.IO;

    [ServiceBehavior]
    public class TaskrAdminService : ITaskrAdminService, IClientAccessPolicy
    {
        [Log]
        [Secured(Constants.SecurableModifyTags)]
        public Guid SaveTag(TagDTO tag)
        {
            Guid tagId = Guid.Empty;

            if (tag == null)
                throw new FaultException("Unable to save null tag.");

            TaskrDataContext db = new TaskrDataContext(ConfigurationProvider.GetConnection("Taskr"));

            if (tag.Id == Guid.Empty)
            {
                Tag _tag = tag.MapTo();
                tagId = _tag.Id;
                db.Tags.InsertOnSubmit(_tag);
            }
            else
            {
                Tag _tag = db.Tags.SingleOrDefault(t => t.Id == tag.Id);
                tag.MapInto(_tag);
                tagId = _tag.Id;
            }

            db.SubmitChanges();
            return tagId;
        }

        [Log]
        [Secured(Constants.SecurableModifyTags)]
        public bool DeleteTag(Guid tagId)
        {
            if (tagId == Guid.Empty)
                throw new FaultException("Unable to delete tag because the passed in id was empty");

            TaskrDataContext db = new TaskrDataContext(ConfigurationProvider.GetConnection("Taskr"));

            Tag tag = db.Tags.SingleOrDefault(t => t.Id == tagId);

            if (tag == null)
            {
                throw new FaultException(string.Format("Tag with id {0} not exist", tagId));
            }

            try
            {
                db.Tags.DeleteOnSubmit(tag);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }            
        }

        [Log]
        public IList<TagDTO> ListTags()
        {
            TaskrDataContext db = new TaskrDataContext(ConfigurationProvider.GetConnection("Taskr"));
            IList<TagDTO> result = new List<TagDTO>();

            foreach (var item in db.Tags.OrderBy(t => t.Label))
            {
                result.Add(TagDTO.StaticMapFrom(item));
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
