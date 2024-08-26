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

namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class income_update_pingridate : PageBase
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
                Bindlist(System.DateTime.Now.ToString("yyyy-MM"));

            }
        }

        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            string datetime = this.years.SelectedItem.Value + "-" + this.months.SelectedItem.Value;
            Bindlist(datetime);
        }
        //查取数据、绑定结果
        public void Bindlist(string datetime)
        {
            Cbhs_dict del = new Cbhs_dict();
            DataTable ds = del.GetUpdatePRData(datetime).Tables[0];
            this.Store1.DataSource = ds;
            this.Store1.DataBind();
        }
        //查询记录
        protected void Button_look_click(object sender, EventArgs e)
        {
            string datetime = this.years.SelectedItem.Value + "-" + this.months.SelectedItem.Value;
            Bindlist(datetime);
        }
        //添加记录
        protected void Button_add_click(object sender, AjaxEventArgs e)
        {

            LoadConfig loadcfg = getLoadConfig("income_update_pingridate_detail.aspx");
            showDetailWin(loadcfg);
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
            string datetime = this.years.SelectedItem.Value + "-" + this.months.SelectedItem.Value;
            Bindlist(datetime);
        }
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;            
            string datetime = this.years.SelectedItem.Value + "-" + this.months.SelectedItem.Value;
            string id = sm.SelectedRow.RecordID;
            Cbhs_dict del = new Cbhs_dict();
            del.Delete_PRdate(sm.SelectedRow.RecordID);           
            Bindlist(datetime);
        }
    }
    }
