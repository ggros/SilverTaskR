namespace Apprenda.Taskr.Service
{
    
    using System.ServiceModel.Web;
    using System.ServiceModel;
    using System.IO;

    [ServiceContract]
    public interface IClientAccessPolicy
    {
        [OperationContract, WebGet(UriTemplate = "/clientaccesspolicy.xml")]
        Stream GetClientAccessPolicy();
    }

}