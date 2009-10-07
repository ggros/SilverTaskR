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
using Apprenda.SaaSGrid;
using Apprenda.SaaSGrid.Subscription;

namespace Taskr.Silverlight.Views
{
    public partial class Subscription : Page
    {
        public Subscription()
        {
            InitializeComponent();            
            this.Loaded += new RoutedEventHandler(Subscription_Loaded);
        }

        void Subscription_Loaded(object sender, RoutedEventArgs e)
        {
            // User Info
            UserContext.Instance.GetCurrentUser(userResult =>
                {
                    if (!userResult.IsError)
                    {
                        nameField.Text = string.Format("{0} {1}", userResult.Value.FirstName, userResult.Value.LastName);
                        emailField.Text = userResult.Value.Email;
                    }
                    else
                    {
                        nameField.Text = "<Error loading user>";
                    }
                });

            // Meter Info
            SubscriptionContext.Instance.GetLimit("Number of Tasks", limitResult =>
                {
                    if (!limitResult.IsError)
                    {
                        allowedTasksField.Text = limitResult.Value.Value.ToString();
                    }
                    else 
                    {
                        allowedTasksField.Text = "<Error loading number of tasks>";
                    }
                });

            SubscriptionContext.Instance.GetLimitCount("Number of Tasks", limitCountResult =>
                {
                    if (!limitCountResult.IsError)
                    {
                        currentTasksField.Text = limitCountResult.Value.Value.ToString();
                    }
                    else
                    {
                        allowedTasksField.Text = "<Error loading max number of tasks>";
                    }
                });

            SubscriptionContext.Instance.IsToggleEnabled("Priority Management", priorityManagementResult =>
                {
                    if (priorityManagementResult.Value.Value)
                    {
                        priorityManagementField.Text = "Subscribed";
                    }
                    else if (!priorityManagementResult.IsError)
                    {
                        priorityManagementField.Text = "Not Subscribed";
                        priorityManagementField.Foreground = new SolidColorBrush() { Color = Colors.Red };
                    }
                    else
                    {
                        priorityManagementField.Text = "<Error determining priority management>";
                        priorityManagementField.Foreground = new SolidColorBrush() { Color = Colors.Red };
                    }
                });

            // Securable Info
            UserContext.Instance.IsAuthorized("Delete Tasks", deleteTasksResult =>
                {
                    if (!deleteTasksResult.IsError)
                    {
                        deleteTasksField.Text = deleteTasksResult.Value.ToString();
                    }
                    else
                    {
                        deleteTasksField.Text = "<Error during security check>";
                    }
                });

            UserContext.Instance.IsAuthorized("Change Task Priority", changePriorityResult =>
            {
                if (!changePriorityResult.IsError)
                {
                    changePriorityField.Text = changePriorityResult.Value.ToString();
                }
                else
                {
                    changePriorityField.Text = "<Error during security check>";
                }
            });

            UserContext.Instance.IsAuthorized("Manage Tags", manageTagsResult =>
            {
                if (!manageTagsResult.IsError)
                {
                    manageTagsField.Text = manageTagsResult.Value.ToString();
                }
                else
                {
                    manageTagsField.Text = "<Error during security check>";
                }
            });

        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
