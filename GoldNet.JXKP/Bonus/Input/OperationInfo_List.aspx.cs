using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.JXKP.cbhs.datagather;
using System.Collections.Generic;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class OperationInfo_List : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    Response.End();
                }

                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.years.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                }
                this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
                this.months.SelectedItem.Value = System.DateTime.Now.ToString("MM");
                Bindlist(System.DateTime.Now.ToString("yyyyMM"));

            }
        }

        //查取数据、绑定结果
        public void Bindlist(string datetime)
        {
            OperationInfo dal = new OperationInfo();
            DataTable ds = dal.GetOperationInfoByDate(datetime);
            this.Store1.DataSource = ds;
            this.Store1.DataBind();
        }

        

        /// <summary>
        /// 显示添加窗口
        /// </summary>
        /// <param name="loadcfg"></param>
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

        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            string datetime = this.years.SelectedItem.Value  + this.months.SelectedItem.Value;
            Bindlist(datetime);
        }
        
        //查询记录
        protected void Button_look_click(object sender, EventArgs e)
        {
            Bindlist(years.SelectedItem.Value + months.SelectedItem.Value);
        }
        //添加记录
        protected void Button_add_click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string id = string.Empty;
            if (selectRow != null && selectRow.Length == 1)
            {
                id = selectRow[0]["ID"];
            }
            LoadConfig loadcfg = getLoadConfig("OperationInfo_detail.aspx");
            if (id != null)
            {
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));
            }
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "add"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("date_time", this.years.SelectedItem.Value + this.months.SelectedItem.Value));
            showDetailWin(loadcfg);
        }
        //编辑按钮触发事件
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {

                string id = string.Empty;
                if (selectRow != null && selectRow.Length == 1)
                {
                    id = selectRow[0]["ID"];
                }
                LoadConfig loadcfg = getLoadConfig("OperationInfo_detail.aspx");
                if (id != null)
                {
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));
                }
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "edit"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("date_time", this.years.SelectedItem.Value + this.months.SelectedItem.Value));
                showDetailWin(loadcfg);
            }

        }
        //删除
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {

                string id = string.Empty;
                if (selectRow != null && selectRow.Length == 1)
                {
                    id = selectRow[0]["ID"];
                }
                OperationInfo dal = new OperationInfo();
                dal.DelOperation(id);
                Bindlist(years.SelectedItem.Value + months.SelectedItem.Value);
            }
        }






    }
}