namespace Apprenda.Taskr.Web
{

    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Apprenda.SaaSGrid;
    using Apprenda.SaaSGrid.Subscription;
    using Taskr.Client;

    public class NonSerializableClass
    {
        public string str = null;
    }

    [Serializable]
    public class SerializableClass
    {
        public string str = null;
    }

    public partial class _Default : System.Web.UI.Page
    {

        private int DueToday = 0;
        private int Overdue = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTaskList();
                BindTagList();
                BindPriorityDropDown();
                DueDate.Text = DateTime.Now.ToShortDateString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            TaskList.RowDataBound += new GridViewRowEventHandler(TaskList_RowDataBound);
            base.OnInit(e);
        }

        /// <summary>
        /// Handles the RowDataBound event of the TaskList control.  This method adds row attributes which help the rendering of the grid.
        /// Specifically, it adds hover behaviors to the rows of the gris.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void TaskList_RowDataBound(object sender, GridViewRowEventArgs args)
        {
            if (args.Row.RowType != DataControlRowType.DataRow) return;

            //set row hover 
            GridView grid = (GridView)sender;
            args.Row.Attributes.Add("onmouseover", "this.className='datarow-hover';");
            args.Row.Attributes.Add("onmouseout", "this.className='" + grid.RowStyle.CssClass + "';");
        }

        private void BindTagList()
        {
            IList<TagDTO> tags = new List<TagDTO>();
            using (TaskrAdminProxy service = new TaskrAdminProxy())
            {
                tags = service.ListTags();
            }

            foreach (TagDTO tag in tags)
            {
                TagList.Items.Add(new ListItem(tag.Label, tag.Id.ToString()));
            }

        }

        private void BindTaskList()
        {
            IList<TaskDTO> tasks = new List<TaskDTO>();
            using (TaskrCoreProxy service = new TaskrCoreProxy())
            {
                tasks = service.ListTasks();
            }

            TaskList.DataSource = tasks;
            TaskList.DataBind();

            foreach (TaskDTO task in tasks)
            {
                if (task.DueDate.Date == DateTime.Now.Date)
                {
                    DueToday++;
                }

                if (task.DueDate.Date < DateTime.Now.Date)
                {
                    Overdue++;
                }
            }

            DueTodayLabel.Text = DueToday.ToString();
            OverdueLabel.Text = Overdue.ToString();

            if (!SubscriptionContext.Instance.IsToggleEnabled("Priority Management").Value)
            {
                TaskList.Columns[3].Visible = false;
            }
        }

        private void BindPriorityDropDown()
        {
            foreach (string item in Enum.GetNames(typeof(TaskPriorityDTO)))
            {
                PriorityField.Items.Add(
                    new ListItem(item, item));
            }
        }

        private Label StatusMessage
        {
            get { return ((Label)this.Master.FindControl("StatusMessageLabel")); }
        }

        private Label DueTodayLabel
        {
            get { return ((Label)this.Master.FindControl("DueTodayLabel")); }
        }

        private Label OverdueLabel
        {
            get { return ((Label)this.Master.FindControl("OverdueLabel")); }
        }

        protected void SaveTask(object sender, CommandEventArgs args)
        {

            if (String.IsNullOrEmpty(SubjectField.Text))
            {
                new StatusPresenter().Error("A subject is required for your task.");
                return;
            }

            TaskDTO task = new TaskDTO();
            task.Id = Guid.Empty;
            task.Subject = SubjectField.Text;
            task.Description = DescriptionField.Text;

            DateTime parsedDateTime;
            if (!DateTime.TryParse(DueDate.Text, out parsedDateTime))
            {
                new StatusPresenter().Error(string.Format("The due date '{0}' is invalid", DueDate.Text));
                return;
            }
            task.DueDate = parsedDateTime;
            task.Tags = new List<TagDTO>();

            TaskPriorityDTO priority = new TaskPriorityDTO();

            foreach (ListItem item in PriorityField.Items)
            {
                if (item.Selected)
                {
                    priority = (TaskPriorityDTO)Enum.Parse(typeof(TaskPriorityDTO), item.Text);
                }
            }

            task.Priority = priority;

            foreach (ListItem item in TagList.Items)
            {
                if (item.Selected)
                {
                    task.Tags.Add(new TagDTO(new Guid(item.Value), item.Text));
                }
            }

            using (TaskrCoreProxy service = new TaskrCoreProxy())
            {
                try
                {
                    service.SaveTask(task);
                    BindTaskList();
                    new StatusPresenter().Success(string.Format("Successfully created '{0}' task.", task.Subject));
                    ResetForm();
                }
                catch (FaultException<LimiterExhaustedDetail> e)
                {
                    new StatusPresenter().Error(e.Message);
                    return;
                }
                catch (Exception)
                {
                    new StatusPresenter().Error("An unknown error ocurred, please try again.");
                }
            }
        }

        protected void DeleteTask(object sender, CommandEventArgs args)
        {
            try
            {
                using (TaskrCoreProxy service = new TaskrCoreProxy())
                {
                    if (service.DeleteTask(new Guid(args.CommandArgument.ToString())))
                    {
                        new StatusPresenter().Success("Task deleted.");
                    }
                    else
                    {
                        throw new Exception("Could not delete the task.");
                    }
                }
            }
            catch (FaultException<NotAuthorizedDetail> fault)
            {
                new StatusPresenter().Error(fault.Message);
            }
            catch (FaultException<LimiterExhaustedDetail> fault)
            {
                new StatusPresenter().Error(fault.Message);
            }
            catch (Exception)
            {
                new StatusPresenter().Error("An unknown error ocurred, please try again.");
            }
            finally
            {
                BindTaskList();
            }
        }

        private void ResetForm()
        {
            foreach (Control control in NewTaskPanel.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Text = "";
                }

                if (control is DropDownList)
                {
                    ((DropDownList)control).SelectedIndex = 0;
                }

                if (control is ListBox)
                {
                    ((ListBox)control).SelectedIndex = -1;
                }

                if (control is Calendar)
                {
                    ((Calendar)control).SelectedDate = DateTime.Today;
                }
            }
            DueDate.Text = DateTime.Now.ToShortDateString();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ResetForm", "GiveFocus()", true);
        }
    }
}
