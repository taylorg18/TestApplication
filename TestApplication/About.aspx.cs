using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StatusPage
{
    public partial class About : Page
    {
        List<_Default.Services> currentServices;
        int startTime;
        int[,] data;

        protected void Page_Load(object sender, EventArgs e)
        {
            startTime = DateTime.Now.Second;
            currentServices = new List<_Default.Services>();
            currentServices.Add(new _Default.Services("Pilots", "https://pilots.up.edu", true));
            currentServices.Add(new _Default.Services("Moodle", "https://learning.up.edu/moodle/login/index.php", true));
            currentServices.Add(new _Default.Services("SelfServe", "https://selfserve.up.edu", true));
            currentServices.Add(new _Default.Services("Library", "https://library.up.edu", true));
            currentServices.Add(new _Default.Services("DegreeWorks", "https://degreeworks.up.edu", true));
            currentServices.Add(new _Default.Services("WebPrint", "https://webprint.up.edu", true));
            currentServices.Add(new _Default.Services("University of Portland", "https://www.up.edu", true));


            foreach (_Default.Services val in currentServices)
            {
                val.Status = checkAvailabilityWebRequest(val);
            }

            Chart1.ChartAreas.Add("chtArea");
            Chart1.ChartAreas[0].AxisX.Title = "Time";
            Chart1.ChartAreas[0].AxisY.Title = "Operational";
            Chart1.Legends.Add("Pilots");
            Chart1.Series.Add("Pilots");
            Chart1.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            




        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            foreach (_Default.Services val in currentServices)
            {
                val.Status = checkAvailabilityWebRequest(val);
            }



        }

        public bool checkAvailabilityWebRequest(_Default.Services service)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(service.URL);
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
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}