using System;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;
using System.Collections.Generic;
using System.Web.UI.WebControls;


namespace GoldNet.JXKP.Bonus.Input
{
    public partial class Out_Weeks_set :PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {

                data();
            }
        }

        protected void data()
        {
            OutRestNonusDal dal = new OutRestNonusDal();
            DataTable table = dal.GetWeeks();
            Store1.DataSource = table;
            Store1.DataBind();
        }

       

        /// <summary>
        /// 编辑按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string id = selectRow[0]["ID"].ToString();
            
            LoadConfig loadcfg = getLoadConfig("OutRestNonus.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));
            showDetailWin(loadcfg);
        }


        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            data();
        }

        //显示详细窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }


    }
}
