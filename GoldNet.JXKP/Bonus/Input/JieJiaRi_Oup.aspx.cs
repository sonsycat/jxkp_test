using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Dal.Properties.Bound;
using Goldnet.Ext.Web;
using GoldNet.Comm.ExportData;
using GoldNet.Model;


namespace GoldNet.JXKP.Bonus.Input
{
    public partial class JieJiaRi_Oup : System.Web.UI.Page
    {
        OperationDal dal = new OperationDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string user_id = ((User)Session["CURRENTSTAFF"]).UserId;
                Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
                DataRow[] deptrolerow1 = dal.Getyiji_dict(user_id).Tables[0].Select();                

            }

        }

        private void BindDate(string date)
        {

            DataTable dt = dal.GetJieJiaRi_OUP(date);
            Store1.DataSource = dt;
            Store1.DataBind();
            Session.Remove("JieJiaRi_Oup");
            Session["JieJiaRi_Oup"] = dt;

        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["JieJiaRi_Oup"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["JieJiaRi_Oup"];

                ex.ExportToLocal(dt, this.Page, "xls", "节假日门诊诊查费");
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