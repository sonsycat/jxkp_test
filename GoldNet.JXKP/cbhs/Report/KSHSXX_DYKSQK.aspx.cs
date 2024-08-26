using System;
using Goldnet.Ext.Web;
using GoldNet.Comm.ExportData;
using System.Data;
using GoldNet.Model;

namespace GoldNet.JXKP.cbhs.Report
{
    public partial class KSHSXX_DYKSQK : PageBase
    {
        public static string deptcode = "";
        public static string searchdate = "";
        public static string endsearchdate = "";

        Goldnet.Dal.Assess dal = new Goldnet.Dal.Assess();

        /// <summary>
        /// 初始化处理
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
                SetInitState();
            }
        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            string filename = "绩效考核结果临时归档文件";
            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            ExportData ex = new ExportData();
            //string stationyear = this.Comb_StartYear.SelectedItem.Value;
            //stationyear = stationyear.PadLeft(4, '0');
            //DataTable dt = dal.GetDeptAssessSavedTemp(incount, stationyear).Tables[0];
            // ex.ExportToLocal(dt, this.Page, "xls", filename);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //string startyear = this.Comb_StartYear.SelectedItem.Value;
            //string startmonth = this.Comb_StartMonth.SelectedItem.Value;

            //DataTable dt = dal.GetDeptAssessResultTemp(incount, startyear).Tables[0];
            //this.Store1.DataSource = dt;
            //this.Store1.DataBind();
            //if (dt.Rows.Count > 0)
            //{
            //    this.Btn_Excel.Disabled = false;
            //    this.Btn_Save.Disabled = false;
            //}
        }

        /// <summary>
        /// 设置页面控件初始化状态
        /// </summary>
        protected void SetInitState()
        {
            deptcode = Request.QueryString["id"].ToString();
            searchdate = Request.QueryString["sy"].ToString();
            endsearchdate = Request.QueryString["esy"].ToString();

            DataTable dt = dal.GetKSHSXX_DYKSQK(searchdate, deptcode, endsearchdate).Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.Btn_Excel.Disabled = false;
            }
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
    }
}
