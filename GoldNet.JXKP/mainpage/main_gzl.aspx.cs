using System;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal.home;
using GoldNet.Model;

namespace GoldNet.JXKP.mainpage
{
    public partial class main_gzl : System.Web.UI.Page
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                this.Store1.DataSource = GetStoreData();
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            Store1.DataSource = GetStoreData();
            Store1.DataBind();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetStoreData()
        {
            //取得传入的选择的会话状态
            string deptcode = Session["curdeptcode"] == null ? ((User)Session["CURRENTSTAFF"]).AccountDeptCode : Session["curdeptcode"].ToString();
            string personid = Session["curpersonid"] == null ? ((User)Session["CURRENTSTAFF"]).StaffId : ((User)Session["CURRENTSTAFF"]).GetStaffid(Session["curpersonid"].ToString());

            string yearstr = Session["curdateyear"] == null ? DateTime.Now.ToString("yyyy") : Session["curdateyear"].ToString();
            string months = Session["curmonths"] == null ? DateTime.Now.ToString("MM") : Session["curmonths"].ToString();

            string stationcode = Session["curstationcode"] == null ? ((User)Session["CURRENTSTAFF"]).GetStationCode(((User)Session["CURRENTSTAFF"]).StaffId, DateTime.Now.Year.ToString()) : Session["curstationcode"].ToString();

            HomeDal dal = new HomeDal();
            DataTable dt = dal.GetGuidess_gzl(deptcode, stationcode, personid, yearstr, DateTime.Now.ToString("yyyyMMdd"), months).Tables[0];
            return dt;
        }
    }
}
