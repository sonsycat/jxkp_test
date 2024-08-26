using System;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.jxkh
{
    public partial class Assess_ResultInfo : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {

                string person_id = Request.QueryString["person_id"].ToString();
                string bsc_class = Request.QueryString["bsc_class"].ToString();
                if (Request.QueryString["assess_code"] == null)
                {
                    string counting = Request.QueryString["counting"].ToString();

                    BindList1(counting, person_id, bsc_class);
                }
                else
                {
                    string assess_code = Request.QueryString["assess_code"].ToString();
                    BindList2(assess_code, person_id, bsc_class);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="counting"></param>
        /// <param name="person_id"></param>
        /// <param name="bsc_class"></param>
        protected void BindList1(string counting, string person_id, string bsc_class)
        {
            Assess dal = new Assess();
            string bsc_where = "";
            if (bsc_class != null && bsc_class != "")
            {
                bsc_where = "T.BSC_CLASS LIKE '" + bsc_class + "%' AND";
            }
            DataTable dt = dal.GetResultInfoTemp(counting, person_id, bsc_where).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assess_code"></param>
        /// <param name="person_id"></param>
        /// <param name="bsc_class"></param>
        protected void BindList2(string assess_code, string person_id, string bsc_class)
        {
            Assess dal = new Assess();
            string bsc_where = "";
            if (bsc_class != null && bsc_class != "")
            {
                bsc_where = "T.BSC_CLASS LIKE '" + bsc_class + "%' AND";
            }
            DataTable dt = dal.GetResultInfo(assess_code, person_id, bsc_where).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
    }
}
