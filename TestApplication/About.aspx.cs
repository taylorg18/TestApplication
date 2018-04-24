using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace StatusPage
{
    public partial class About : Page
    {
        List<_Default.Services> currentServices;



        private Random rand = new Random();


        protected void Page_Load(object sender, EventArgs e)
        {

            currentServices = new List<_Default.Services>();
            currentServices.Add(new _Default.Services("Pilots", "https://pilots.up.edu", true));
            currentServices.Add(new _Default.Services("Moodle", "https://learning.up.edu/moodle/login/index.php", true));
            currentServices.Add(new _Default.Services("Library", "https://library.up.edu", true));
            currentServices.Add(new _Default.Services("DegreeWorks", "https://degreeworks.up.edu", true));
            currentServices.Add(new _Default.Services("SelfServe", "https://selfserve.up.edu", true));
            currentServices.Add(new _Default.Services("University of Portland", "https://www.up.edu", true));

            // Add series into chart
            Chart1.Series.Clear();
            foreach (_Default.Services val in currentServices)
            {
                val.Status = checkAvailabilityWebRequest(val);
                Series newSeries = new Series(val.Name);
                newSeries.IsVisibleInLegend = true;

                newSeries.ChartType = SeriesChartType.Line;

                newSeries.BorderWidth = 2;
                newSeries.XValueType = ChartValueType.Int32;
                Chart1.Series.Add(newSeries);
            }

            Chart1.ChartAreas.Add("chtArea");
            Chart1.ChartAreas[0].AxisX.Title = "Time";
            Chart1.ChartAreas[0].AxisY.Title = "Operational";

            int minValue = DateTime.Now.Second;
            int maxValue = minValue + 120;

            Chart1.ChartAreas[0].AxisX.Minimum = minValue;
            Chart1.ChartAreas[0].AxisX.Maximum = maxValue;
            Chart1.ChartAreas[0].AxisY.Minimum = -currentServices.Count;
            Chart1.ChartAreas[0].AxisY.Interval = 1;
            Chart1.ChartAreas[0].AxisY.Maximum = currentServices.Count;
            Chart1.Legends.Add("Legend");

            AddData();
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

        /*
       * Handle the Timer fire
       * */
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            // Whenever the timer fires (every five seconds)
            // Update the status of the services and rebind the data


        }

      

        public void AddData()
        {
            int size = Chart1.Series[0].Points.Count;
            int timestamp = 0;

            timestamp = (int)Chart1.ChartAreas[0].AxisX.Minimum + DateTime.Now.Second;
            
            int i = 0;
            int val = i + 1;
            foreach (Series ptSeries in Chart1.Series)
            {
                val = i + 1;
                if(!currentServices[i].Status)
                {
                    val = -val;
                }
                AddNewPoint(timestamp, ptSeries, currentServices[i], val);
                i++;
            }

        }

        public void AddNewPoint(int timeStamp, Series ptSeries, _Default.Services service, int index)
        {
            double newVal = 0;

            if (ptSeries.Points.Count >= 0)
            {
                newVal = (rand.NextDouble());
            }

            ptSeries.Points.AddXY(0, index);
            ptSeries.Points.AddXY(timeStamp, index);

            Chart1.ChartAreas[0].AxisX.Maximum = timeStamp;
        }


       

    }
}