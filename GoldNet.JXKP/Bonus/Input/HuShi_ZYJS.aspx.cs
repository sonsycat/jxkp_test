using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Dal.Properties.Bound;
using Goldnet.Ext.Web;
using GoldNet.Comm.ExportData;
using GoldNet.Model;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class HuShi_ZYJS : System.Web.UI.Page
    {
        OperationDal dal = new OperationDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                
                Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
                DataRow[] deptrolerow1 = dal.GetHL_ZYJS().Tables[0].Select();
                foreach (DataRow roww in deptrolerow1)
                {
                    this.dept_SelectDept.Items.Add(new Goldnet.Ext.Web.ListItem(roww["DEPT_NAME"].ToString(), roww["DEPT_CODE"].ToString()));
                }

            }

        }

        private void BindDate(string date, string end, string dept_code)
        {

            DataTable dt = dal.GetHL_ZYJS(date, end, dept_code);
            Store1.DataSource = dt;
            Store1.DataBind();
            Session.Remove("HuShi_ZYJS");
            Session["HuShi_ZYJS"] = dt;

        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["HuShi_ZYJS"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["HuShi_ZYJS"];

                ex.ExportToLocal(dt, this.Page, "xls", "护士中医技术工作量");
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
            BindDate(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMMdd"), Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyyMMdd"), this.dept_SelectDept.SelectedItem.Value);
        }

        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            BindDate(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMMdd"), Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyyMMdd"), this.dept_SelectDept.SelectedItem.Value);
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