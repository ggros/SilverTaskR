namespace Apprenda.Taskr.Web
{

    using System;
    using System.Web.UI;
    using Taskr.Client;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    using Apprenda.SaaSGrid;
    using System.ServiceModel;
    using Apprenda.Services.Logging;

    public partial class Admin : System.Web.UI.Page
    {
        #region Private Members

        private static readonly ILogger logger = LogManager.Instance().GetLogger(typeof(Admin));
        private Label StatusMessage
        {
            get { return ((Label)this.Master.FindControl("StatusMessageLabel")); }
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserContext.Instance.IsAuthorized("Modify Tags"))
            {
                Response.Redirect("Errors/Unauthorized.aspx", true);
            }

            if (!IsPostBack)
            {
                BindTagList();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            TagList.RowDataBound += new GridViewRowEventHandler(TagList_RowDataBound);
            base.OnInit(e);
        }

        /// <summary>
        /// Handles the RowDataBound event of the TagList control.  This method adds row attributes which help the rendering of the grid.
        /// Specifically, it adds hover behaviors to the rows of the gris.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void TagList_RowDataBound(object sender, GridViewRowEventArgs args)
        {
            if (args.Row.RowType != DataControlRowType.DataRow) return;

            //set row hover 
            GridView grid = (GridView)sender;
            args.Row.Attributes.Add("onmouseover", "this.className='datarow-hover';");
            args.Row.Attributes.Add("onmouseout", "this.className='" + grid.RowStyle.CssClass + "';");
        }

        #endregion

        #region Page Methods

        /// <summary>
        /// This method retrieves the list of existing tags and binds that list as the
        /// datasource of the grid control that displays them on the page.
        /// </summary>
        private void BindTagList()
        {

            IList<TagDTO> tags = new List<TagDTO>();
            using (TaskrAdminProxy service = new TaskrAdminProxy())
            {
                try
                {
                    tags = service.ListTags();
                }
                catch (FaultException<NotAuthorizedDetail> fault)
                {
                    new StatusPresenter().Error(fault.Message);
                }
                catch (Exception exception)
                {

                    if (logger.IsErrorEnabled)
                    {
                        logger.Error("An error occurred while retrieving the list of tags.", exception);
                    }

                    new StatusPresenter().Error("A problem occurred while retrieving the list of tags.");
                    return;
                }
                finally
                {
                    TagList.DataSource = tags;
                    TagList.DataBind();
                }
            }
        }

        /// <summary>
        /// This method handles the command event (callback) that initiates the save of a new tag.
        /// The method gets user input from the form on the page and attempts to save the tag through the admin service.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="CommandEventArgs"/> instance containing the event data.</param>
        protected void SaveTag(object sender, CommandEventArgs args)
        {
            if (String.IsNullOrEmpty(TagField.Text))
            {
                return;
            }

            TagDTO tag = new TagDTO();
            tag.Label = TagField.Text;

            using (TaskrAdminProxy service = new TaskrAdminProxy())
            {
                try
                {
                    service.SaveTag(tag);
                    new StatusPresenter().Success(string.Format("Created {0} tag.", tag.Label));
                    BindTagList();
                    ResetForm();
                }
                catch (FaultException<NotAuthorizedDetail> fault)
                {
                    new StatusPresenter().Error(fault.Message);
                    return;
                }
                catch (Exception exception)
                {
                    if (logger.IsErrorEnabled)
                    {
                        logger.Error("An error occurred while saving a tag.", exception);
                    }

                    new StatusPresenter().Error("An unknown error occurred. Please try again.");
                    return;
                }
            }

        }

        /// <summary>
        /// This method handles the command event (callback) initiated by a delete button control on the page.
        /// The id of the tag to delete is passed from the page as the command argument.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.CommandEventArgs"/> instance containing the event data.</param>
        protected void DeleteTag(object sender, CommandEventArgs args)
        {
            Guid targetId = new Guid((string)args.CommandArgument);
            using (TaskrAdminProxy service = new TaskrAdminProxy())
            {
                try
                {
                    if (service.DeleteTag(targetId))
                    {
                        BindTagList();
                        new StatusPresenter().Success("Tag deleted.");
                    }
                    else
                    {
                        new StatusPresenter().Error("Could not delete tag.");
                    }
                }
                catch (FaultException<NotAuthorizedDetail> fault)
                {
                    new StatusPresenter().Error(fault.Message);
                }
                catch (Exception exception)
                {
                    if (logger.IsErrorEnabled)
                    {
                        logger.Error("An error occurred while trying to delete a tag.", exception);
                    }

                    new StatusPresenter().Error("An error occurred while trying to delete the tag.");
                    return;
                }
            }
        }

        private void ResetForm()
        {
            TagField.Text = "";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ResetForm", "GiveFocus()", true);
        }

        #endregion
    }
}