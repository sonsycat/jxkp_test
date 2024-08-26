using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal.Properties.Bound;
using System.Collections.Generic;
using GoldNet.Comm.ExportData;
using GoldNet.Model;

namespace GoldNet.JXKP.cbhs.Report
{
    public partial class Men_Zhen : System.Web.UI.Page
    {
        OperationDal dal = new OperationDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                          
                Goldnet.Dal.SYS_DEPT_DICT dal1 = new Goldnet.Dal.SYS_DEPT_DICT();
                DataRow[] rolerow = dal1.Getleibie_dict().Tables[0].Select();
                foreach (DataRow row in rolerow)
                {
                    this.leibie_SelectDept.Items.Add(new Goldnet.Ext.Web.ListItem(row["ITEM_NAME"].ToString(), row["ITEM_NAME"].ToString()));
                }
                this.leibie_SelectDept.Value = "住院人数";
            }


        }

        private void BindDate(string date, string end, string ITEM_NAME)
        {
            DataTable dt = dal.GetGongMen_Zhen(date, end, ITEM_NAME);
            Store1.DataSource = dt;
            Store1.DataBind();
            Session.Remove("Men_Zhen");
            Session["Men_Zhen"] = dt;

        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["Men_Zhen"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["Men_Zhen"];

                ex.ExportToLocal(dt, this.Page, "xls", this.leibie_SelectDept.SelectedItem.Value+"查询");
                //MHeaderTabletoExcel(dt, null, null, null, 0);
                //ex.ExportToLocal(l_dt, this.Page, "xls", "人员信息");
            }
        }
        private string GetBeginDate()
        {

            string benginDate = Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd") + " 19:00:00";
            return benginDate;
        }
        private string GetEndDate()
        {

            string benginDate = Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd") + " 19:00:00";
            return benginDate;
        }
        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {

            BindDate(GetBeginDate(), GetEndDate(), this.leibie_SelectDept.SelectedItem.Value);
        }

        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {

            BindDate(GetBeginDate(), GetEndDate(), this.leibie_SelectDept.SelectedItem.Value);
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