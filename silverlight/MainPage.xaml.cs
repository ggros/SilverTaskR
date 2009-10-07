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
using Apprenda.SaaSGrid;
using Apprenda.SaaSGrid.Subscription;
using Apprenda.SaaSGrid.API;
using Taskr.Silverlight.Views;
using System.Threading;

namespace Taskr.Silverlight
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            // We initialize the SessionContext in the constructor of this page.    
            SessionContext.Instance.Initialize(result =>
            {
                if (result.IsError)
                {
                    // perform work to prevent further processing...
                }
            });  
            
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            InitializeComponent();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ApplySubscriptionSecurity();
            ApplySubscriptionMetadata();
        }

        private void ApplySubscriptionSecurity()
        {

            // hide the admin link if the user doesn't have access
            UserContext.Instance.IsAuthorized("Modify Tags",
                userResult =>
                {
                    if (!userResult.Value)
                    {
                        adminLink.Visibility = Visibility.Collapsed;
                    }
                }
            );
        }

        private void ApplySubscriptionMetadata()
        {
            UserContext.Instance.GetCurrentUser(
                userResult =>
                {                    
                    if (!userResult.IsError)
                    {
                        greeting.Text = string.Format("Hello, {0}", userResult.Value.FirstName);
                    }
                    else
                    {
                        greeting.Text = "Could not load current user.";
                    }                        
                }
            );
            
            copyrightNotice.Text = string.Format("Copyright {0}, Apprenda Inc.", DateTime.Now.Year);
        
        }

        public void SetStatus(string message, MessageStatus status)
        {
            
            statusLabel.Text = message;
            statusLabel.Visibility = Visibility.Visible;            

            switch (status)
            {
                case(MessageStatus.Success):
                    statusLabel.Foreground = new SolidColorBrush(Colors.Green);
                    break;
                case (MessageStatus.Error):
                    statusLabel.Foreground = new SolidColorBrush(Colors.Red);
                    break;
                case (MessageStatus.Information):
                    statusLabel.Foreground = new SolidColorBrush(Colors.Blue);
                    break;
            }

        }

        public void ClearStatus()
        {
            statusLabel.Text = "";
            statusLabel.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        public enum MessageStatus
        {
            Success,
            Error,
            Information
        }

    }
}
