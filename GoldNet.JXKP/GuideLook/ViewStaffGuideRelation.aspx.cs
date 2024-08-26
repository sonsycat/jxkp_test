using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;
using Goldnet.Dal;

namespace GoldNet.JXKP.GuideLook
{
    public partial class ViewStaffGuideRelation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                string DeptCode = Request.QueryString["DEPT_CODE"].ToString();

                string dd1 = Request.QueryString["FromDate"].ToString();

                string dd2 = Request.QueryString["ToDate"].ToString();

                string GuideCode = Request.QueryString["GuideCode"].ToString();

                string incount = ((User)Session["CURRENTSTAFF"]).StaffId;

                if (incount == "") incount = "NotUserid";

                this.Store1.DataSource = GetStoreData(dd1, dd2, incount, GuideCode,"" , DeptCode);
                this.Store1.DataBind();

            }
        }

        /// <summary>
        /// 人员相关性分析报表
        /// </summary>
        /// <param name="startime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="incount">STAFFID</param>
        /// <param name="gudie_code">指标CODE</param>
        /// <param name="unit_code_p">科室代码</param>
        /// <param name="personid">人员代码</param>
        /// <returns>相关性分析集合</returns>
        private DataTable GetStoreData(string startime, string endtime, string incount, string gudie_code, string unit_code_p, string personid)
        {
            ReportDalDict dal = new ReportDalDict();

            DataTable dt = dal.GetSatffCorrelation(startime, endtime, incount, gudie_code, personid).Tables[0];

            return dt;

        }

    



    }
}
