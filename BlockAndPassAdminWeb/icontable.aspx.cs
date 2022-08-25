//using Microsoft.Reporting.WebForms;
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
    public partial class icontable : System.Web.UI.Page
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
    }

}