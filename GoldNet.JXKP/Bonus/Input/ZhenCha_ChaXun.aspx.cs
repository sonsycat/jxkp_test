using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Dal.Properties.Bound;
using Goldnet.Ext.Web;
using GoldNet.Comm.ExportData;
using GoldNet.Model;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class ZhenCha_ChaXun : System.Web.UI.Page
    {
        OperationDal dal = new OperationDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                                

            }

        }

        private void BindDate(string date)
        {
            string user_id = ((User)Session["CURRENTSTAFF"]).UserId;
            DataTable dt = dal.GetZhenCha_ChaXun(date, user_id);
            Store1.DataSource = dt;
            Store1.DataBind();
            Session.Remove("ZhenCha_ChaXun");
            Session["ZhenCha_ChaXun"] = dt;

        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["ZhenCha_ChaXun"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["ZhenCha_ChaXun"];

                ex.ExportToLocal(dt, this.Page, "xls", "诊查费查询");
                //MHeaderTabletoExcel(dt, null, null, null, 0);
                //ex.ExportToLocal(l_dt, this.Page, "xls", "人员信息");
            }
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            BindDate(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM"));
        }

        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            BindDate(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM"));
        }
        protected void RowSelect(object sender, AjaxEventArgs e)
        {
        }

        /// <summary>
        /// 反序列化得到客户端提交的gridpanel数据行
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

    }
}