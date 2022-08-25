using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BlockAndPass.AdminWeb
{
    public partial class funcionales : System.Web.UI.Page
    {
        long USUARIO = 12345;
        string idRepote = string.Empty;
        int idRepoteInt = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            idRepote = Request.Form["SendA"];
            if (Request.Form["SendB"] != null && Request.Form["SendB"].ToString() != "")
            {
                idRepoteInt = Convert.ToInt32(Request.Form["SendB"]);
            }

            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            else
            {
                if (!Page.IsPostBack)
                {

                }  
            }
        }

        protected void savebtn_Click(object sender, EventArgs e)
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;

            //ReportViewer1.ServerReport.ReportServerCredentials = new ReportServerCredentials("Parquearse", "Admin2217", "CLOUDSERVER");

            ServerReport serverReport = ReportViewer1.ServerReport;

            serverReport.ReportServerUrl = new Uri("http://169.47.207.158/reportserver");
            serverReport.ReportPath = "/Florida_Reports/" + idRepote;
            //  serverReport.ReportPath = "/Millens_reports/" + idRepote;
        }

        public int GetIntValue()
        {
            return idRepoteInt;
        }
    }

}