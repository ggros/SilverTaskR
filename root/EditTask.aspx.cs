namespace Apprenda.Taskr.Web
{

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
    using Taskr.Client;
    using System.Collections.Generic;

    public partial class EditTask : System.Web.UI.Page
    {

        private Label StatusMessage
        {
            get { return ((Label)this.Master.FindControl("StatusMessageLabel")); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                if (string.IsNullOrEmpty(Request.Params["taskId"]))
                {
                    new StatusPresenter().Error("Unable to load that task.");
                    return;
                }

                BindTagList();
                BindPriorityDropDown();
                BindTaskForm(GetTask(new Guid(Request.Params["taskId"])));
            }
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

        private void BindPriorityDropDown()
        {
            foreach (string item in Enum.GetNames(typeof(TaskPriorityDTO)))
            {
                PriorityField.Items.Add(
                    new ListItem(item, item));
            }
        }

        private void BindTaskForm(TaskDTO task)
        {
            TaskIdField.Value = task.Id.ToString();
            SubjectField.Text = task.Subject;
            DescriptionField.Text = task.Description;
            PriorityField.SelectedValue = task.Priority.ToString();
            DueDate.Text = task.DueDate.ToShortDateString();

            foreach (TagDTO tag in task.Tags)
            {
                try
                {
                    TagList.Items.FindByValue(tag.Id.ToString()).Selected = true;
                }
                catch { }
            }

        }

        private TaskDTO GetTask(Guid taskId)
        {
            using (TaskrCoreProxy service = new TaskrCoreProxy())
            {
                try
                {
                    return service.GetTask(taskId);
                }
                catch (Exception e)
                {                    
                    StatusMessage.CssClass = "status-error";
                    StatusMessage.Text = e.Message;
                    return new TaskDTO();
                }
            }        
        }

        protected void SaveTask(object sender, EventArgs args)
        {

            if (String.IsNullOrEmpty(SubjectField.Text))
            {
                new StatusPresenter().Error("A subject is required for your task."); 
                return;
            }

            TaskDTO task = new TaskDTO();
            task.Id = new Guid(TaskIdField.Value);
            task.Subject = SubjectField.Text;
            task.Description = DescriptionField.Text;
            DateTime parsedDate;
            if(!DateTime.TryParse(DueDate.Text, out parsedDate))
            {
                new StatusPresenter().Error(string.Format("The due date '{0}' is invalid", DueDate.Text));
                return;
            }
            task.DueDate = parsedDate;
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
                    Response.Redirect("Default.aspx", true);
                }
                catch (Exception e)
                {
                    new StatusPresenter().Error(e.Message);
                    return;
                }
            }

        }

    }
}
