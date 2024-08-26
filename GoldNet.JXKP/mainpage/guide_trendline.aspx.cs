using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Web;
using System.Text;
using GoldNet.Comm;
using GoldNet.Comm.Pic;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using Goldnet.Dal.home;


namespace GoldNet.JXKP.mainpage
{
    public partial class guide_trendline : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }

            this.FCLiteral.Text = CreateChart();

        }


        public string CreateChart()
        {
            Int32 id = 0;
            Int32 nid = 0;
            if (!Int32.TryParse(Request.QueryString["id"], out id) || !Int32.TryParse(Request.QueryString["nid"], out nid))
            {
                Response.Write("error");
                return "";
            }
            //oid = Request.QueryString["oid"].ToString();

            string yearstr = Session["curdateyear"] == null ? DateTime.Now.ToString("yyyy") : Session["curdateyear"].ToString();
            
            
            string unitcode = "";
            if (id.ToString().Substring(0,1) == "1")
            {
                unitcode = "00";
            }
            else if (id.ToString().Substring(0, 1) == "2")
            {
                string deptcode = Session["curdeptcode"] == null ? ((User)Session["CURRENTSTAFF"]).AccountDeptCode : Session["curdeptcode"].ToString();
                unitcode = Session["CorrelationDept"] == null ? deptcode : Session["CorrelationDept"].ToString();
            }
            else
            {
                string incount = ((User)Session["CURRENTSTAFF"]).StaffId;
                if (incount == "") incount = "NotUserid";
                unitcode = Session["curpersonid"] == null ? incount : ((User)Session["CURRENTSTAFF"]).GetStaffid(Session["curpersonid"].ToString());
            }


            HomeDal dal = new HomeDal();
            DataTable oRs = dal.GetTrendLine(id.ToString(), yearstr, unitcode).Tables[0];
         

            StringBuilder strXML=new StringBuilder();

            strXML.AppendFormat("<graph baseFontSize='12' caption='{0}'  decimalPrecision='0' showNames='1' numberSuffix='' pieSliceDepth='30' formatNumberScale='0' rotateNames='1' thickness='10' background='red'>", oRs.Rows.Count > 0 ? oRs.Rows[0]["guide_name"].ToString() : "");

            string[] colors = new string[8];
            colors[0] = "AFD8F8";
            colors[1] = "F6BD0F";
            colors[2] = "8BBA00";
            colors[3] = "FF8E46";
            colors[4] = "008E8E";
            colors[5] = "D64646";
            colors[6] = "8E468E";
            colors[7] = "588526";

            //Iterate through each factory
            for (int i = 0; i < oRs.Rows.Count; i++)
            {
                int j = 0;
                //Generate <set name='..' value='..' /> 
                if (i < colors.Length)
                {
                    j = i;
                }
                else
                {
                    j = i - colors.Length;
                }
                strXML.Append("<set name='" + oRs.Rows[i]["tjyf"].ToString() + "' value='" + oRs.Rows[i]["guide_value"].ToString() + "' color='" + colors[j] + "'/>");
            }

            strXML.Append("</graph>");
            return Charts.RenderChart("/resources/flash/FCF_Line.swf", "", strXML.ToString(), "FactorySum", "500", "240", false, false);
        }
    }
}
