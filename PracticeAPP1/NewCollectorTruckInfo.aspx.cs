using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticeAPP1
{
    public partial class NewCollectorTruckInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["username"] != null)
            {
                onlineuser.Text = "Welcome \n" + Session["username"].ToString() + " ";
                if (!IsPostBack)
                {

                    relodEverything();
                    

                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }

        }

        protected void LogOut_Click(object sender, EventArgs e)
        {
            Session["username"] = null;
            
            Response.Redirect("Index.aspx");

        }

        

        protected void infoSubmit_Click(object sender, EventArgs e)
        {
            bindCollectorInfo();

        }

        protected void viewinfo_Click(object sender, EventArgs e)
        {
            CollectorInfo.ActiveViewIndex = 0;
            currentbardisplay.Text = "View Collector's Info.";
            viewinfo.Enabled = false;
            addinfo.Enabled = true;
            Citylist.DataSource = GetData("spGetCities", null);
            Citylist.DataBind();
            Debug.WriteLine("Applying Data Data");
            ListItem liCity = new ListItem("Select City", "-1");
            Citylist.Items.Insert(0, liCity);

            ListItem liZone = new ListItem("Select Zone", "-1");
            Zonelist.Items.Insert(0, liZone);

            Zonelist.Enabled = false;
        }

        protected void addinfo_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("In Add Info");
            CollectorInfo.ActiveViewIndex = 1;
            currentbardisplay.Text = "Add New Collector's Info.";
            addinfo.Enabled = false;
            viewinfo.Enabled = true;
            Debug.WriteLine("Getting Data");
            addCity_Name.DataSource = GetData("spGetCities", null);
            addCity_Name.DataBind();
            Debug.WriteLine("Applying Data Data");
            ListItem addliCity = new ListItem("Select City", "-1");
            addCity_Name.Items.Insert(0, addliCity);

            ListItem addliZone = new ListItem("Select Zone", "-1");
            addZone_Name.Items.Insert(0, addliZone);

            addZone_Name.Enabled = false;
        }

        protected void infoAdd_Click(object sender, EventArgs e)
        {
            if (username.Value == "" || userpassword.Value == "" || addCity_Name.SelectedValue == "-1" ||addZone_Name.SelectedValue == "-1" || contactno.Value == "" || licenceno.Value == "")
            {
                displayMSG.Text = "Invalid Information Try Again";
            }
            else
            {
                string query = "INSERT into Collector_Info VALUES('" + username.Value + "','" + userpassword.Value + "','" + contactno.Value + "','" + addCity_Name.SelectedItem.ToString() + "','" + addZone_Name.SelectedItem.ToString() + "','" + licenceno.Value + "')";
                string cs = ConfigurationManager.ConnectionStrings["SmartWasteMgtDB"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    cmd.ExecuteNonQuery();


                }
                displayMSG.Text = "Successfully Inserted The Data";
                
                username.Value = "";
                userpassword.Value = "";
                contactno.Value = "";
                licenceno.Value = "";
                errormsg.Text = "";
                addCity_Name.SelectedIndex = 0;
                addZone_Name.SelectedIndex = 0;
            }
        }

        private DataSet GetData(string SPName, SqlParameter SPParameter)
        {
            string cs = ConfigurationManager.ConnectionStrings["SmartWasteMgtDB"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlDataAdapter da = new SqlDataAdapter(SPName, con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            if (SPParameter != null)
            {
                da.SelectCommand.Parameters.Add(SPParameter);
            }
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;

        }

       /* private void bindCollectorInfo()
        {
            string query = "SELECT Username,Contact_No,City,Zone,Licence from Collector_Info WHERE City='"+Citylist.SelectedItem+"' AND Zone='"+Zonelist.SelectedItem+"'";
            string cs = ConfigurationManager.ConnectionStrings["SmartWasteMgtDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                showCollectorInformation.DataSource = dt;
                showCollectorInformation.DataBind();
            }

        }*/

        protected void Citylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Citylist.SelectedIndex==0)
            {
                Zonelist.SelectedIndex = 0;
                Zonelist.Enabled = false;
            }
            else
            {
                Zonelist.Enabled = true;
                SqlParameter parameter = new SqlParameter("@City_Id",Citylist.SelectedValue);
                DataSet ds= GetData("spGetZonesByCity_Id",parameter);
                Zonelist.DataSource = ds;
                Zonelist.DataBind();
                ListItem liZone = new ListItem("Select Zone", "-1");
                Zonelist.Items.Insert(0, liZone);

                                
            }
        }

        protected void addCity_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (addCity_Name.SelectedIndex == 0)
            {
                addZone_Name.SelectedIndex = 0;
                addZone_Name.Enabled = false;
            }
            else
            {
                addZone_Name.Enabled = true;
                SqlParameter parameter = new SqlParameter("@City_Id", addCity_Name.SelectedValue);
                DataSet ds = GetData("spGetZonesByCity_Id", parameter);
                addZone_Name.DataSource = ds;
                addZone_Name.DataBind();
                ListItem liZone = new ListItem("Select Zone", "-1");
                addZone_Name.Items.Insert(0, liZone);


            }

            
        }

        private void relodEverything()
        {
            Citylist.DataSource = GetData("spGetCities", null);
            Citylist.DataBind();
            
            ListItem liCity = new ListItem("Select City", "-1");
            Citylist.Items.Insert(0, liCity);

            ListItem liZone = new ListItem("Select Zone", "-1");
            Zonelist.Items.Insert(0, liZone);

            Zonelist.Enabled = false;

            Debug.WriteLine("In Add Info");
            
            
            
            
            addCity_Name.DataSource = GetData("spGetCities", null);
            addCity_Name.DataBind();
            
            ListItem addliCity = new ListItem("Select City", "-1");
            addCity_Name.Items.Insert(0, addliCity);

            ListItem addliZone = new ListItem("Select Zone", "-1");
            addZone_Name.Items.Insert(0, addliZone);

            addZone_Name.Enabled = false;
        }
    }


}