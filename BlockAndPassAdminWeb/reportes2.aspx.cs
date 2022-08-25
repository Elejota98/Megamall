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
    public partial class reportes2 : System.Web.UI.Page
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
            //ReportViewer1.ProcessingMode = ProcessingMode.Remote;

            //ReportViewer1.ServerReport.ReportServerCredentials = new ReportServerCredentials("Parquearse", "Admin2217", "CLOUDSERVER");

            //ServerReport serverReport = ReportViewer1.ServerReport;

            //serverReport.ReportServerUrl = new Uri("http://169.47.207.158/reportserver");
            //serverReport.ReportPath = "/Florida_Reports/" + idRepote;
            //  serverReport.ReportPath = "/Millens_reports/" + idRepote;
        }

        public int GetIntValue()
        {
            return idRepoteInt;
        }
    }

    /// <summary>
    /// Local implementation of IReportServerCredentials
    /// </summary>
    //public class ReportServerCredentials : IReportServerCredentials
    //{
    //    private string _userName;
    //    private string _password;
    //    private string _domain;

    //    public ReportServerCredentials(string userName, string password, string domain)
    //    {
    //        _userName = userName;
    //        _password = password;
    //        _domain = domain;
    //    }

    //    public WindowsIdentity ImpersonationUser
    //    {
    //        get
    //        {
    //            // Use default identity.
    //            return null;
    //        }
    //    }

    //    public ICredentials NetworkCredentials
    //    {
    //        get
    //        {
    //            // Use default identity.
    //            return new NetworkCredential(_userName, _password, _domain);
    //        }
    //    }

    //    public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
    //    {
    //        // Do not use forms credentials to authenticate.
    //        authCookie = null;
    //        user = password = authority = null;
    //        return false;
    //    }
    //}
}