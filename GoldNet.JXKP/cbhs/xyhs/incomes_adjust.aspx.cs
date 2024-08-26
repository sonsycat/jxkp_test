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

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class incomes_adjust : PageBase
    {
        private BoundComm boundcomm = new BoundComm();
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
                Bindlist(System.DateTime.Now.ToString("yyyy-MM"));

            }
        }

        //查取数据、绑定结果
        public void Bindlist(string datetime)
        {
            XyhsDetail dal = new XyhsDetail();
            DataTable ds = dal.GetXyhsIncomesAdjust().Tables[0];
            this.Store1.DataSource = ds;
            this.Store1.DataBind();
        }

        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist(System.DateTime.Now.ToString("yyyy-MM"));
        }

        //查询记录
        protected void Button_look_click(object sender, EventArgs e)
        {
            Bindlist(System.DateTime.Now.ToString("yyyy-MM"));
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
            LoadConfig loadcfg = getLoadConfig("incomes_adjust_detail.aspx");
            if (id != null)
            {
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("row_id", id));
            }
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "add"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("date_time", this.years.SelectedItem.Value + this.months.SelectedItem.Value));
            showDetailWin(loadcfg);
        }

         //修改记录
        [AjaxMethod]
        public void data_edit(string rowsid)
        {
            LoadConfig loadcfg = getLoadConfig("incomes_adjust_detail.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "edit"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("row_id", rowsid));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("date_time", this.years.SelectedItem.Value + this.months.SelectedItem.Value));
            showDetailWin(loadcfg);
        }

         //删除
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow == null || selectRow.Length < 1)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "请至少选择一条记录",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            else
            {
                for (int i = 0; i < selectRow.Length; i++)
                {
                    XyhsDetail dal = new XyhsDetail();
                    dal.DelXyhsIncomesAdjustByid(selectRow[i]["ID"]);
                }
                string date_time = this.years.SelectedItem.Value + "-" + this.months.SelectedItem.Value;
                Bindlist(date_time);
            }
        }

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        //显示添加窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        //时间改变再去验证奖金是否生成
        protected void Date_SelectOnChange(object sender, EventArgs e)
        {
        }


    }
}
