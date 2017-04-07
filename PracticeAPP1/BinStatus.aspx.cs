using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticeAPP1
{
    public partial class BinStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["username"] != null)
            {
                onlineuser.Text = "Welcome \n" + Session["username"].ToString() + " ";
                if (!IsPostBack)
                {
                    SetImageURL();
                }
               
            }
            else
            {
                Response.Redirect("Index.aspx");
            }

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            SetImageURL();
        }

        protected void LogOut_Click(object sender, EventArgs e)
        {
            Session["username"] = null;
            
            Response.Redirect("Index.aspx");

        }

        private void SetImageURL()
        {
            if (ViewState["imagedisplayed"]==null)
            {
                binimage.ImageUrl = "~/Images/1.png";
                ViewState["imagedisplayed"] = 1;


            }
            else
            {
                int i = (int)ViewState["imagedisplayed"];
                if (i==10)
                {
                    binimage.ImageUrl = "~/Images/1.png";
                    ViewState["imagedisplayed"] = 1;

                }
                else
                {
                    i = i + 1;
                    binimage.ImageUrl = "~/Images/" + i.ToString() + ".png";
                    ViewState["imagedisplayed"] = i;
                }
            }
            
            
        }

        protected void StartLiveView_Click(object sender, EventArgs e)
        {
            if (Timer1.Enabled)
            {
                Timer1.Enabled = false;
            }
            else
            {
                Timer1.Enabled = true;
            }
        }
    }
}