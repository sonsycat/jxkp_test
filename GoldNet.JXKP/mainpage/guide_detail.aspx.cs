using System;
using System.Data;
using System.Text;
using GoldNet.Comm.Pic;
using GoldNet.Model;
using Goldnet.Dal.home;


namespace GoldNet.JXKP.mainpage
{
    public partial class guide_detail : PageBase
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateChart()
        {
            Int32 id = 0;
            Int32 nid = 0;
            string oid = "";
            if (!Int32.TryParse(Request.QueryString["id"], out id) || !Int32.TryParse(Request.QueryString["nid"], out nid))
            {
                Response.Write("error");
                return "";
            }
            oid = Request.QueryString["oid"].ToString();

            if ((nid == id) | (nid == 0) | (oid != "01" && oid != "02"))
            {
                Response.Write("没有设下级指标");
                return "";
            }

            string deptcode = Session["curdeptcode"] == null ? ((User)Session["CURRENTSTAFF"]).AccountDeptCode : Session["curdeptcode"].ToString();
            string incount = ((User)Session["CURRENTSTAFF"]).StaffId;
            if (incount == "") incount = "NotUserid";

            string personid = Session["curpersonid"] == null ? incount : ((User)Session["CURRENTSTAFF"]).GetStaffid(Session["curpersonid"].ToString());
            string yearstr = Session["curdateyear"] == null ? DateTime.Now.ToString("yyyy") : Session["curdateyear"].ToString();
            HomeDal dal = new HomeDal();
            DataTable oRs = dal.GetGuides(deptcode, personid, nid.ToString(), yearstr).Tables[0];// GoldNet.Comm.Power.Power.GetGuides(deptcode, stationcode, personid, nid.ToString(), "next", "").Tables[0];
            string[] colors = new string[8];
            colors[0] = "AFD8F8";
            colors[1] = "1F45B0";
            colors[2] = "1184E0";
            colors[3] = "FF8E46";
            colors[4] = "656BDF";
            colors[5] = "55A3FF";
            colors[6] = "2C2C85";
            colors[7] = "D64646";
            if (this.App.SelectedValue == "StackedBar3D")
            {
                StringBuilder xmlData = new StringBuilder();
                StringBuilder categories = new StringBuilder();
                StringBuilder strDataProdA = new StringBuilder();
                xmlData.AppendFormat("<chart caption='{0}' numberPrefix='' formatNumberScale='0' baseFontSize='12'>", oRs.Rows.Count > 0 ? oRs.Rows[0]["ZBMC"].ToString() : "");
                categories.Append("<categories>");
                strDataProdA.Append("<dataset seriesName=''>");
                int chartheight = oRs.Rows.Count * 18 + 60;
                for (int i = 0; i < oRs.Rows.Count; i++)
                {
                    int j = i % colors.Length;

                    categories.AppendFormat("<category name='{0}'  />", oRs.Rows[i]["deptname"].ToString());

                    strDataProdA.AppendFormat("<set value='{0}' color='{1}'/>", oRs.Rows[i]["WCZ"].ToString(), colors[j]);
                }
                categories.Append("</categories>");
                strDataProdA.Append("</dataset>");
                xmlData.Append(categories.ToString());
                xmlData.Append(strDataProdA.ToString());
                xmlData.Append("</chart>");
                return Charts.RenderChart("/resources/flash/StackedBar3D.swf", "", xmlData.ToString(), "productSales", "550", chartheight.ToString(), false, false);
            }
            else if (this.App.SelectedValue == "Pie3D")
            {
                StringBuilder strXML = new StringBuilder();
                strXML.AppendFormat("<graph baseFontSize='12' caption='{0}'  decimalPrecision='0' showNames='1' numberSuffix='' pieSliceDepth='30' formatNumberScale='0' rotateNames='1' thickness='10' background='red'>", oRs.Rows.Count > 0 ? oRs.Rows[0]["ZBMC"].ToString() : "");
                for (int i = 0; i < oRs.Rows.Count; i++)
                {
                    int j = i % colors.Length;
                    strXML.AppendFormat("<set name='{0}' value='{1}' color='{2}' />", oRs.Rows[i]["deptname"].ToString(), oRs.Rows[i]["WCZ"].ToString(), colors[j]);
                }
                strXML.Append("</graph>");
                return Charts.RenderChart("/resources/flash/" + this.App.SelectedValue + ".swf", "", strXML.ToString(), "FactorySum", "500", "380", false, false);
            }
            else
            {
                return "";
            }
        }
    }
}
