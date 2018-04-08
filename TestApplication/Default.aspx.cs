using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StatusPage
{
    public partial class _Default : Page
    {
        List<Services> currentServices;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentServices = new List<Services>();
            currentServices.Add(new Services("Pilots", "https://pilots.up.edu", true));
            currentServices.Add(new Services("Moodle", "https://learning.up.edu/moodle/login/index.php", true));
            currentServices.Add(new Services("SelfServe", "https://selfserve.up.edu", true));
            currentServices.Add(new Services("Library", "https://library.up.edu", true));
            currentServices.Add(new Services("DegreeWorks", "https://degreeworks.up.edu", true));
            currentServices.Add(new Services("WebPrint", "https://webprint.up.edu", true));
            currentServices.Add(new Services("University of Portland", "https://www.up.edu", true));


            foreach ( Services val in currentServices)
            {
                val.isEnabled = checkAvailabilityWebRequest(val);
            }

            GridView1.DataSource = currentServices;
            GridView1.DataBind();

        }


        public bool checkAvailabilityWebRequest(Services service)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(service.url);
                request.MaximumAutomaticRedirections = 10;
                request.MaximumResponseHeadersLength = 10;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
                request.Method = "HEAD";
            
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine(response.ContentType);
                if (response == null || response.StatusCode != HttpStatusCode.OK)
                {
                    response.Close();
                    return false;
                }
                // do something with response.Headers to find out information about the request
                response.Close();
                return true;
            }
            catch (WebException wex)
            {
                //set flag if there was a timeout or some other issues
                return false;
            }
            catch( Exception ex)
            {
                return false;
            }
        }

        public bool checkAvailabilityPing(Services service)
        {
            var ping = new System.Net.NetworkInformation.Ping();

            var result = ping.Send(service.url);

            if (result.Status != System.Net.NetworkInformation.IPStatus.Success)
                return false;

            return true;
        }


        public class Services
        {
            public string name { get; set; }
            public bool isEnabled { get; set; }
            public string url { get; set; }

            public Services(string inName, string inUrl, bool inIsEnabled)
            {
                name = inName;
                url = inUrl;
                isEnabled = inIsEnabled;
            }


        }
    }
}