using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Taskr.Silverlight.TaskrServices.Admin;
using Apprenda.SaaSGrid.WCF;
using System.ServiceModel;

namespace Taskr.Silverlight.Views
{
    public partial class Admin : Page
    {
        
        public Admin()
        {
            InitializeComponent();
            BindTagList();
        }

        private void BindTagList()
        {

            (Application.Current.RootVisual as MainPage).ClearStatus();

            TaskrAdminClient proxy =
                new TaskrAdminClient(
                    new SaaSGridSilverlightCustomBinding(new SaaSGridContextInspector()),
                    new EndpointAddress(App.Current.Host.InitParams["taskrAdminAddress"])
                );
            
            proxy.ListTagsCompleted +=
                (object sender, ListTagsCompletedEventArgs args) =>
                {
                    if (null == args.Error)
                    {
                        tagList.ItemsSource = args.Result;
                    }
                    else
                    {
                        (Application.Current.RootVisual as MainPage).SetStatus(args.Error.Message, MainPage.MessageStatus.Error);
                    }                                        
                };

            proxy.ListTagsAsync();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {

            (Application.Current.RootVisual as MainPage).ClearStatus();

            Tag task = new Tag()
            {
                Label = txtNewTag.Text.Trim()
            };

            TaskrAdminClient proxy =
                new TaskrAdminClient(
                    new SaaSGridSilverlightCustomBinding(new SaaSGridContextInspector()),
                    new EndpointAddress(App.Current.Host.InitParams["taskrAdminAddress"])
                );

            proxy.SaveTagCompleted +=
                (object s, SaveTagCompletedEventArgs args) =>
                {
                    if (null == args.Error)
                    {                        
                        BindTagList();
                        (Application.Current.RootVisual as MainPage).SetStatus("Tag saved successfully.", MainPage.MessageStatus.Success);
                    }
                    else
                    {
                        (Application.Current.RootVisual as MainPage).SetStatus(args.Error.Message, MainPage.MessageStatus.Error);
                    }
                };
            proxy.SaveTagAsync(task);
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            (Application.Current.RootVisual as MainPage).ClearStatus();            

            TaskrAdminClient proxy =
                new TaskrAdminClient(
                    new SaaSGridSilverlightCustomBinding(new SaaSGridContextInspector()),
                    new EndpointAddress(App.Current.Host.InitParams["taskrAdminAddress"])
                );

            proxy.DeleteTagCompleted +=
                (object s, DeleteTagCompletedEventArgs args) =>
                {
                    if (null == args.Error)
                    {
                        BindTagList();
                        (Application.Current.RootVisual as MainPage).SetStatus("Tag deleted successfully.", MainPage.MessageStatus.Success);
                    }
                    else
                    {
                        (Application.Current.RootVisual as MainPage).SetStatus(args.Error.Message, MainPage.MessageStatus.Error);
                    }
                };
            proxy.DeleteTagAsync(new Guid(((Button)e.OriginalSource).DataContext.ToString()));
        }

    }
}
