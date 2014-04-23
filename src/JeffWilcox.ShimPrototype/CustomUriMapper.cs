using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace JeffWilcox.Mayor.SocialExtension
{
    // {/PeopleExtension?action=Check_In}
    internal class CustomUriMapper : UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            if (uri.ToString().Contains("PeopleExtension?action=Check_In"))
            {
                return new Uri("/MainPage.xaml?action=checkin", UriKind.Relative);
            }
            else return uri;
        }
    }
}