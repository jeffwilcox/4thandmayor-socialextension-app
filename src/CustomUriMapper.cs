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

using System;
using System.Windows.Navigation;

namespace JeffWilcox.Mayor.SocialExtension
{
    // The Windows Phone 8.1 OS "me" tile will bring people
    // to a URI that looks like this:
    //   /PeopleExtension?action=Check_In
    //
    // The Post An Update experience would go here:
    //   /_default#/PeopleExtension?action=Post_Update

    /// <summary>
    /// Type for mapping incoming OS-level URI requests into
    /// a local application designation.
    /// </summary>
    internal class CustomUriMapper : UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            // Map the "check in" request to a local page.
            if (uri.ToString().Contains("PeopleExtension?action=Check_In"))
            {
                return new Uri("/MainPage.xaml?action=checkin", UriKind.Relative);
            }
            else return uri;
        }
    }
}