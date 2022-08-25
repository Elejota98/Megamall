using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BlockAndPass.AdminWeb
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("template.aspx");
            }
            else
            {
                string a = Request.QueryString["emailcamp"];
                string b = Request.QueryString["passcamp"];
                //string c = Request.QueryString["checkcamp"];

                if (a != null && b != null && a != string.Empty && b != string.Empty)
                {

                    var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                    string query = string.Empty;

                    query = "select documento, contraseña from t_usuarios where usuario='" + a + "' and Estado = 'true'";

                    string documento = string.Empty;
                    string clave = string.Empty;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                // Check is the reader has any rows at all before starting to read.
                                if (reader.HasRows)
                                {
                                    // Read advances to the next row.
                                    while (reader.Read())
                                    {
                                        documento = reader["documento"].ToString();
                                        clave = reader["contraseña"].ToString();
                                    }
                                }
                            }
                        }
                    }




                    if (documento != string.Empty)
                    {
                        if (Decrypt(clave) == b)
                        {
                            //FormsAuthentication.RedirectFromLoginPage(documento, false);
                            FormsAuthentication.SetAuthCookie(documento, false);
                            Response.Redirect("template.aspx");
                        }
                        else
                        {
                            Response.Redirect("login.aspx?invalidpassword=true");
                        }
                    }
                    else
                    {
                        Response.Redirect("login.aspx?nofinduser=true");
                    }

                }
                else
                {
                    //Response.Redirect("login.aspx/invalidpassword");
                }
            }
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }

    
}