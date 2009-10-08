using System;
using System.Collections.Generic;
using System.Data.Services;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using Apprenda.Configuration;
using Apprenda.Taskr.Repository;

namespace Apprenda.Taskr.Service
{
    public class TaskRDataService : DataService<TaskrDataContext>, IClientAccessPolicy
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            // config.SetEntitySetAccessRule("MyEntityset", EntitySetRights.AllRead);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            //config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
        }
        //public static void CreateDatasource
        protected override TaskrDataContext CreateDataSource()
        {
            return new TaskrDataContext(ConfigurationProvider.GetConnection("Taskr"));
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
