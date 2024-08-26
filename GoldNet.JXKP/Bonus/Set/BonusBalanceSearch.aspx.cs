using System;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Collections.Generic;
using GoldNet.Model;

namespace GoldNet.JXKP.Bonus.Set
{
    public partial class BonusBalanceSearch : System.Web.UI.Page
    {
        private BoundComm boundcomm = new BoundComm();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Store3.DataSource = boundcomm.getYears();
                Store3.DataBind();
                cbbYear.SetValue(DateTime.Now.Year);
                Store4.DataSource = boundcomm.getMonth();
                Store4.DataBind();
                cbbmonth.SetValue(DateTime.Now.Month);

                Bindlist(DateTime.Now.Year.ToString(), (DateTime.Now.Month - 1).ToString());
            }
        }

        //查询科室信息表Dept_info
        protected void Bindlist(string year, string month)
        {
            CheckPersons dal = new CheckPersons();
            DataTable dt = dal.GetBonusBalance(year, month);
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        //查询
        protected void Button_look_click(object sender, EventArgs e)
        {
            string year = cbbYear.SelectedItem.Value;
            string month = cbbmonth.SelectedItem.Value;
            Bindlist(year, month);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string year = cbbYear.SelectedItem.Value;
            string month = cbbmonth.SelectedItem.Value;
            Bindlist(year, month);
        }
    }
}
