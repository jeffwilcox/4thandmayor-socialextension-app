//
// Copyright © Wilcox Digital, LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Windows.Storage;
using Windows.System;

namespace JeffWilcox.Mayor.SocialExtension
{
    public partial class MainPage : PhoneApplicationPage
    {
        ApplicationDataContainer _settings;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _settings = ApplicationData.Current.LocalSettings.CreateContainer(
                "settings", 
                ApplicationDataCreateDisposition.Always);
        }

        /// <summary>
        /// Gets or sets a persisted value indicating whether the
        /// user has confirmed the presence of the app.
        /// </summary>
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UpdateViewState();

            _check.IsChecked = IsInstallConfirmed;

            // The user is coming from the People App if the
            // Uri Mapper implementation redirects them to
            // this page including the action segment.
            if (e.Uri.ToString().Contains("action"))
            {
                if (IsInstallConfirmed)
                {
                    // Hide this "app" experience since we
                    // will directly forward the user.
                    LayoutRoot.Visibility = Visibility.Collapsed;
                    OnAppBarButtonClick(this, null);
                }
                else
                {
                    // The user must confirm this before we
                    // want to hide the UI. Ideally for only
                    // the first time.
                    MessageBox.Show("Please check the box confirming that you have installed 4th & Mayor in order to continue.",
                        "Please confirm", MessageBoxButton.OK);
                }
            }
        }

        private async void OnAppBarButtonClick(object sender, EventArgs e)
        {
            var uri = new Uri("jw-4thandmayor:checkin");
            var success = await Launcher.LaunchUriAsync(uri);

            // Only terminate (exit) the application if they have
            // confirmed that they have the app installed. Now the
            // edge case here is if the user is not running the
            // version 3.18 of the app which defined for the first
            // time the 'jw-4thandmayor:' protocol, then the
            // experience is bad since they'll just appear right
            // back on the ME social page. Nothing I can do on
            // the app side for that. This is only a temporary
            // problem with the roll out of the app since most
            // 8.1 users will be auto-updated or move on their
            // own to a newer version soon enough.
            if (success && IsInstallConfirmed)
            {
                Application.Current.Terminate();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Open the Store page to allow the user to download
            // 4th & Mayor.
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

        /// <summary>
        /// Toggle which simple UI components are shown.
        /// </summary>
        private void UpdateViewState()
        {
            Installed.Visibility = IsInstallConfirmed ? Visibility.Visible : Visibility.Collapsed;
            NotInstalled.Visibility = IsInstallConfirmed ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}