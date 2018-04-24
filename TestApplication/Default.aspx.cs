using System;
using System.Collections.Generic;
using System.Data;
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
            // Create the gridview and all of the services
            GridView1.RowCreated += GridView1_RowCreated;
            label1.Text = "Last Update: " + DateTime.Now.ToString("hh:mm:ss tt");
            currentServices = new List<Services>();
            currentServices.Add(new Services("Pilots", "https://pilots.up.edu", true));
            currentServices.Add(new Services("Moodle", "https://learning.up.edu/moodle/login/index.php", true));
            currentServices.Add(new Services("Library", "https://library.up.edu", true));
            currentServices.Add(new Services("DegreeWorks", "https://degreeworks.up.edu", true));
            currentServices.Add(new Services("University of Portland", "https://www.up.edu", true));

            Label2.Text = "All Services Operational";
            Label2.ForeColor = System.Drawing.Color.Green;
            foreach ( Services val in currentServices)
            {
                val.Status = checkAvailabilityWebRequest(val);

                if(! val.Status)
                {
                    Label2.Text = "Some Services Unavailable";
                    Label2.ForeColor = System.Drawing.Color.Red;
                }
            }

            GridView1.DataSource = currentServices;
            GridView1.DataBind();

        }


        public bool checkAvailabilityWebRequest(Services service)
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
            catch( Exception ex)
            {
                return false;
            }
        }

        /*
         * Method to handle the even when a Row is created in the gridview
         * */
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int myCol = 1;
                Services _row = (Services)e.Row.DataItem;
                if (_row != null)
                {
                    // If the service is enabled say operational and make green
                    if (_row.Status)
                    {
                        e.Row.Cells[myCol].BackColor = System.Drawing.ColorTranslator.FromHtml("#00FF00");
                        e.Row.Cells[myCol].Text = "Operational";
                    }
                    else // Else make red and disabled
                    {
                        e.Row.Cells[myCol].BackColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                        e.Row.Cells[myCol].Text = "Disabled";
                    }
                }
                
            }
        }

        /*
         * Handle the Timer fire
         * */
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            // Whenever the timer fires (every five minutes)
            // Update the status of the services and rebind the data
            label1.Text = "Last Update: " + DateTime.Now.ToString("hh:mm:ss tt");
            Label2.Text = "All Services Operational";
            Label2.ForeColor = System.Drawing.Color.Green;

            foreach (Services val in currentServices)
            {
                val.Status = checkAvailabilityWebRequest(val);

                if (!val.Status)
                {
                    Label2.Text = "Some Services Unavailable";
                    Label2.ForeColor = System.Drawing.Color.Red;
                }
            }

            GridView1.DataSource = currentServices;
            GridView1.DataBind();
        }

        /*
         * A simple class to define the services
         * 
         * */
        public class Services
        {
            public string Name { get; set; }
            public bool Status { get; set; }
            public string URL { get; set; }

            public Services(string inName, string inUrl, bool inIsEnabled)
            {
                Name = inName;
                URL = inUrl;
                Status = inIsEnabled;
            }


        }
    }
}