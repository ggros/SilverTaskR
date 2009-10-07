namespace Apprenda.Taskr.Web
{

    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class StatusPresenter
    {

        private Label targetLabel;

        public StatusPresenter()
        {
            targetLabel = (Label)((Page)HttpContext.Current.Handler).Master.FindControl("StatusLabel");
        }

        public StatusPresenter(Label targetControl)
        {
            this.targetLabel = targetControl;
        }

        public void Success(string message)
        {
            targetLabel.CssClass = "status-success";
            targetLabel.Text = message;
        }

        public void Error(string message)
        {
            targetLabel.CssClass = "status-error";
            targetLabel.Text = message;
        }

        public void Info(string message)
        {
            targetLabel.CssClass = "status-info";
            targetLabel.Text = message;
        }

        public void Clear()
        {
            targetLabel.CssClass = "";
            targetLabel.Text = "";
        }

    }
}
