using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Apprenda.SaaSGrid;
using Apprenda.Taskr.Client;
using System.Collections.Generic;

namespace Taskr.Web.Templates
{
    public partial class Default : System.Web.UI.MasterPage
    {

        public int DueToday = 0;
        public int Overdue = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            BindDueDateStats();
            DueTodayLabel.Text = DueToday.ToString();
            OverdueLabel.Text = Overdue.ToString();
        }

        private void BindDueDateStats()
        {
            IList<TaskDTO> tasks = new List<TaskDTO>();
            using (TaskrCoreProxy service = new TaskrCoreProxy())
            {
                try
                {
                    tasks = service.ListTasks();
                }
                catch { }
            }

            if (tasks.Count > 0)
            {
                foreach (TaskDTO task in tasks)
                {
                    if (task.DueDate.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        DueToday++;
                    }

                    if (task.DueDate.Date < DateTime.Now.Date)
                    {
                        Overdue++;
                    }
                }
            }

        }
    }
}
