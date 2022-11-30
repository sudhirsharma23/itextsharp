using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        public static string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowData();
            }
        }
        //method for Displaying Data in Gridview  
        protected void ShowData()
        {
            SqlConnection con = new SqlConnection(constr);
            string str = "Select * from Employees;";
            con.Open();
            SqlCommand cmd = new SqlCommand(str, con);
            DataTable dt = new DataTable();
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            DataView dv = dt.DefaultView;
            GridView1.DataSource = dv;
            GridView1.DataBind();
            int i = 0;
            con.Close();
            Response.Redirect("PdfProfile.aspx?Id=1");

        }
        protected void Submit(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT FirstName,LastName FROM Employees", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                DataSet dataset = new DataSet();
                adapter.Fill(dataset);
                string line = new string('-', 40) + Environment.NewLine;
                Console.WriteLine(builder.GetUpdateCommand().CommandText);
                Console.WriteLine(line);
                Console.WriteLine(builder.GetDeleteCommand().CommandText);
                Console.WriteLine(line);
                Console.WriteLine(builder.GetInsertCommand().CommandText);
                Console.WriteLine(line);
                Console.ReadLine();
                // insert a row  
                SqlCommand insert = builder.GetInsertCommand();
                insert.Parameters["@P1"].Value = "Ak";
                insert.Parameters["@P2"].Value = "KK";
                insert.Parameters["@P3"].Value = "Paul Kimmel";
                insert.ExecuteNonQuery();
                adapter.Fill(dataset);
                DataRow[] rows = dataset.Tables[0].Select("ID = '4'");
                if (rows.Length == 1) Console.WriteLine(rows[0]["FirstName"]);
                Console.ReadLine();
            }
        }
        protected void OnCommandPdf_click(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
            string id = e.CommandArgument.ToString();
         }
    }
}