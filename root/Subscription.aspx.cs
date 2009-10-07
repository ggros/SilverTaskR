namespace Apprenda.Taskr.Web
{

    using System;
    using System.Web;
    using Apprenda.SaaSGrid;
    using Apprenda.SaaSGrid.Subscription;

    public partial class Subscription : System.Web.UI.Page
    {

        public UserContext CurrentUser = UserContext.Instance;
        public ITenantUser UserData = UserContext.Instance.CurrentUser;
        public SubscriptionContext CurrentSubscription = SubscriptionContext.Instance;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
