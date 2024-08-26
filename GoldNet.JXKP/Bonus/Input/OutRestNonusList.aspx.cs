using System;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class OutRestNonusList : PageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                OutRestNonusDal dal = new OutRestNonusDal();

                string year = DateTime.Now.AddMonths(0).Year.ToString();
                string month = DateTime.Now.AddMonths(0).Month.ToString();

                BoundComm boundcomm = new BoundComm();

                //年度下拉框内容
                this.SYear.DataSource = boundcomm.getYears();
                this.SYear.DataBind();
                this.Combo_Year.Value = year;

                //月份下拉框内容
                this.SMonth.DataSource = boundcomm.getMonth();
                this.SMonth.DataBind();
                this.cbbmonth.Value = month;

                DataTable dt1 = dal.GetHolidays().Tables[0];
                this.Store4.DataSource = dt1;
                this.Store4.DataBind();

                data(GetBeginDate());
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        protected void data(string datetime)
        {
            OutRestNonusDal dal = new OutRestNonusDal();
            DataTable table = dal.GetOutRestNonusList(datetime);
            Store1.DataSource = table;
            Store1.DataBind();
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate2());
        }

        /// <summary>
        /// 保存处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                OutRestNonusDal dal = new OutRestNonusDal();
                try
                {
                    dal.SaveOutRestNonusList(selectRow);

                    this.ShowMessage("信息提示", "数据保存成功！");
                    Store_RefreshData(null, null);
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteOtherAward");
                }
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            data(GetBeginDate2());
        }
        
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetBeginDate()
        {
            string year = Combo_Year.Value.ToString();
            string month = cbbmonth.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + month + "01";
            return benginDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetBeginDate2()
        {
            string year = Combo_Year.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + month + "01";
            return benginDate;
        }

    }
}
