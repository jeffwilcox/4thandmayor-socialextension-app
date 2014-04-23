using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.System;
using Microsoft.Phone.Tasks;

namespace JeffWilcox.Mayor.SocialExtension
{
    public partial class MainPage : PhoneApplicationPage
    {
        Windows.Storage.ApplicationDataContainer _settings;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _settings = Windows.Storage.ApplicationData.Current.LocalSettings.CreateContainer("settings", Windows.Storage.ApplicationDataCreateDisposition.Always);
        }

        public bool IsInstallConfirmed
        {
            get
            {
                object value;
                if (_settings.Values.TryGetValue("confirmed", out value))
                {
                    if (value is bool)
                    {
                        return (bool)value;
                    }
                }

                return false;
            }

            set
            {
                _settings.Values["confirmed"] = value;
            }
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UpdateViewState();

            _check.IsChecked = IsInstallConfirmed;

            if (e.Uri.ToString().Contains("action"))
            {
                if (IsInstallConfirmed)
                {
                    LayoutRoot.Visibility = System.Windows.Visibility.Collapsed;
                    OnAppBarButtonClick(this, null);
                }
                else
                {
                    MessageBox.Show("Please check the box confirming that you have installed 4th & Mayor in order to continue.",
                        "Please confirm", MessageBoxButton.OK);
                }
            }
        }

        private async void OnAppBarButtonClick(object sender, EventArgs e)
        {
            var uri = new Uri("jw-4thandmayor:checkin");
            var success = await Launcher.LaunchUriAsync(uri);
            if (success && IsInstallConfirmed)
            {
                System.Windows.Application.Current.Terminate();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();

            marketplaceDetailTask.ContentIdentifier = "c7d13b8d-9951-e011-854c-00237de2db9e";
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;

            marketplaceDetailTask.Show();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb != null)
            {
                IsInstallConfirmed = (cb.IsChecked == true);
                UpdateViewState();
            }
        }

        private void UpdateViewState()
        {
            Installed.Visibility = IsInstallConfirmed ? Visibility.Visible : Visibility.Collapsed;
            NotInstalled.Visibility = IsInstallConfirmed ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}