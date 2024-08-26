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

namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class Hospital_costs_list : PageBase
    {
        private BoundComm boundcomm = new BoundComm();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                bool flags = this.IsPersons();
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
            Appended_income dal = new Appended_income();
            DataTable ds = dal.GetHospitalcostsList(datetime).Tables[0];
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
            LoadConfig loadcfg = getLoadConfig("Hospital_cost_edit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("flags", "add"));
            showDetailWin(loadcfg);
        }
        //修改记录
        [AjaxMethod]
        public void data_edit(string rowsid)
        {
            LoadConfig loadcfg = getLoadConfig("Hospital_cost_edit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", rowsid));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("flags", "edit"));
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
                    Appended_income dal = new Appended_income();
                    dal.Del_Hospitalcost(selectRow[i]["ID"]);
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
            string datetime = this.years.SelectedItem.Value + "-" + this.months.SelectedItem.Value;
            Bindlist(datetime);
        }
        protected void Button_decompose_look(object sender, EventArgs e)
        {
            string datetime = this.years.SelectedItem.Value + "-" + this.months.SelectedItem.Value+"-01";
            LoadConfig loadcfg = getLoadConfig("Hospital_Decompose_Detail.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stardate", datetime));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("enddate", datetime));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("gettype", "4"));

            showCenterSet(this.Hospital_Detail, loadcfg);
        }
        protected void Button_decompose_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
            Cbhs_dict dal_dict = new Cbhs_dict();
            AccountingData dal_acc = new AccountingData();
            if (dal_dict.IsBonusSave(date_time))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "该月奖金已经生成、不可以改变成本数据!",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            
            else
            {
                //验证核算数据是否生成
                if (dal_acc.IsAccount(date_time))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "该月奖金核算已经完成、不可以改变成本数据!",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }
            string rtnmsg = "";
            try
            {
                Cost_detail dal = new Cost_detail();
                rtnmsg = dal.Exec_sp_extract_hospital_cost(date_time);
                this.ShowMessage("提示","分解成功，可以点击'分解查看',查看分解后的数据！");
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_break_click");
            }
        }
    }

}
