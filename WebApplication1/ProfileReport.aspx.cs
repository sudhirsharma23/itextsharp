using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class ProfileReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string filePath = Server.MapPath(string.Concat("~/Resume/", Request.QueryString["Id"]));
            try
            {
                //Open pdf in web browser  
                WebClient client = new WebClient();
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    Byte[] buffer = client.DownloadData(filePath + "/" + "UserProfile.pdf");
                    if (buffer != null)
                    {
                        HttpContext.Current.Response.ContentType = "application/pdf";
                        HttpContext.Current.Response.AddHeader("content-length", buffer.Length.ToString());
                        HttpContext.Current.Response.BinaryWrite(buffer);
                        // HttpContext.Current.Response.End();  
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}