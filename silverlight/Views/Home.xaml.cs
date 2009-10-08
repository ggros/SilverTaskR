using System.Collections.Generic;
using System;
using System.Data.Services.Client;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Taskr.Silverlight.TaskrServices.Core;
using System.Runtime.Serialization;
using System;
using System.ServiceModel.Channels;
using System.ComponentModel;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading;
using System.Runtime.CompilerServices;
using Apprenda.SaaSGrid.WCF;

namespace Taskr.Silverlight.Views
{
    public partial class Home : Page
    {
        private IList<TaskRDataService.Task> allTasks;
        public Home()
        {
            InitializeComponent();
            BindTaskList();
            CallDataService();
        }

        private void CallDataService()
        {
            var ctx = new TaskRDataService.TaskrDataContext(new Uri("http://localhost:7777/TaskRDataService"));
            ctx.SendingRequest += (sender,args) =>
                                      {
                                          Console.WriteLine(args.RequestHeaders.ToString());
                                      };

            var dsquery = (DataServiceQuery<TaskRDataService.Task>)(from task in ctx.Tasks select task);
            dsquery.BeginExecute(
                result =>
                    {
                        try
                        {
                            var collection = dsquery.EndExecute(result);
                            allTasks = collection.ToList();
                            var sb = new StringBuilder();
                            foreach (var t in allTasks)
                            {
                                sb.AppendLine(t.Subject);
                            }
                            this.txtTaskDescription.Text = sb.ToString(); 
                        }
                        catch(Exception ex)
                        {
                            this.txtTaskDescription.Text = String.Format("Exception: {0}", ex);
                        }
                    }
                , null);
        }

        private void BindTaskList()
        {

            (Application.Current.RootVisual as MainPage).ClearStatus();
            
            TaskrCoreClient proxy = 
                new TaskrCoreClient(
                    new SaaSGridSilverlightCustomBinding(new SaaSGridContextInspector()), 
                    new EndpointAddress(HttpUtility.UrlDecode(App.Current.Host.InitParams["taskrCoreAddress"]))
                );

            proxy.ListTasksCompleted +=
                (object sender, ListTasksCompletedEventArgs args) =>
                {
                    if (null == args.Error)
                    {                        
                        if (null != args.Result && args.Result.Count > 0)
                        {
                            taskList.ItemsSource = args.Result;
                            taskList.Visibility = Visibility.Visible;
                            lblEmptyTasks.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            taskList.Visibility = Visibility.Collapsed;
                            lblEmptyTasks.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        taskList.Visibility = Visibility.Collapsed;
                        lblEmptyTasks.Visibility = Visibility.Visible;
                        (Application.Current.RootVisual as MainPage).SetStatus(args.Error.Message, MainPage.MessageStatus.Error);
                    }
                };

            proxy.ListTasksAsync();
            
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        // borrowed from http://stackoverflow.com/questions/894991/increase-columns-width-in-silverlight-datagrid-to-fill-whole-dg-width
        void dg_sql_data_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DataGrid myDataGrid = (DataGrid)sender;
            // Do not change column size if Visibility State Changed
            if (myDataGrid.RenderSize.Width != 0)
            {
                double all_columns_sizes = 0.0;
                foreach (DataGridColumn dg_c in myDataGrid.Columns)
                {
                    all_columns_sizes += dg_c.ActualWidth;
                }
                // Space available to fill ( -18 Standard vScrollbar)
                double space_available = (myDataGrid.RenderSize.Width - 18) - all_columns_sizes;
                foreach (DataGridColumn dg_c in myDataGrid.Columns)
                {
                    dg_c.Width = new DataGridLength(dg_c.ActualWidth + (space_available / myDataGrid.Columns.Count));
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            (Application.Current.RootVisual as MainPage).ClearStatus();

            TaskrCoreClient proxy =
                new TaskrCoreClient(
                    new SaaSGridSilverlightCustomBinding(new SaaSGridContextInspector()),
                    new EndpointAddress(App.Current.Host.InitParams["taskrCoreAddress"])
                );

            proxy.DeleteTaskCompleted +=
                (object s, DeleteTaskCompletedEventArgs args) =>
                {
                    if (null == args.Error)
                    {
                        ClearInput();
                        BindTaskList();
                        (Application.Current.RootVisual as MainPage).SetStatus("Task deleted successfully.", MainPage.MessageStatus.Success);
                    }
                    else
                    {
                        (Application.Current.RootVisual as MainPage).SetStatus(args.Error.Message, MainPage.MessageStatus.Error);
                    }
                };
            proxy.DeleteTaskAsync(new Guid(((Button)e.OriginalSource).DataContext.ToString()));
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {

            (Application.Current.RootVisual as MainPage).ClearStatus();

            Task task = new Task()
            {
                Subject = txtTaskSubject.Text.Trim(),
                Description = txtTaskDescription.Text.Trim(),
                DueDate = DateTime.Parse(txtTaskDueDate.Text.Trim())
            };

            TaskrCoreClient proxy =
                new TaskrCoreClient(
                    new SaaSGridSilverlightCustomBinding(new SaaSGridContextInspector()),
                    new EndpointAddress(App.Current.Host.InitParams["taskrCoreAddress"])
                );

            proxy.SaveTaskCompleted +=
                (object s, SaveTaskCompletedEventArgs args) =>
                {
                    if (null == args.Error)
                    {
                        ClearInput();
                        BindTaskList();
                        (Application.Current.RootVisual as MainPage).SetStatus("Task saved successfully.", MainPage.MessageStatus.Success);
                    }
                    else
                    {
                        (Application.Current.RootVisual as MainPage).SetStatus(args.Error.Message, MainPage.MessageStatus.Error);
                    }
                };
            proxy.SaveTaskAsync(task);
        }

        private void ClearInput()
        {
            txtTaskSubject.Text = string.Empty;
            txtTaskDescription.Text = string.Empty;
            txtTaskDueDate.Text = string.Empty;
        }

    }
}