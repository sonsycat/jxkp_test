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
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class xyhs_get_total_costs_detail : PageBase
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
                HttpProxy pro1 = new HttpProxy();
                pro1.Method = HttpMethod.POST;
                pro1.Url = "../WebService/CostItems.ashx";
                this.Store1.Proxy.Add(pro1);
                string op = Request.QueryString["op"].ToString();
                if (op == "edit")
                {
                    this.COSTS_ITEM.Disabled = true;
                    string item_code = Request.QueryString["item_code"].ToString();
                    string date_time = Request.QueryString["date_time"].ToString();
                    SetInit(item_code, date_time);
                }
                else
                {
                    this.Tag.Hidden = false;
                }
            }
        }
        //编辑初始化
        private void SetInit(string item_code,string date_time)
        {
            this.COSTS_ITEM.SelectedItem.Value = item_code;
            XyhsDetail dal = new XyhsDetail();
            DataTable dt = dal.GetTotalCosts(date_time, item_code).Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.DATA_ID.Value = dt.Rows[0]["ID"].ToString();
                this.COSTS.Value =Convert.ToDouble(dt.Rows[0]["COSTS"]);
                this.REMARKS.Value = dt.Rows[0]["MEMO"].ToString();
            }
        }
        //保存
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            XyhsDetail dal = new XyhsDetail();
            string date_time = Request.QueryString["date_time"].ToString();
            DataTable dt = dal.GetTotalCosts(date_time, this.COSTS_ITEM.SelectedItem.Value.ToString()).Tables[0];
            string op = Request.QueryString["op"].ToString();
            if (dt.Rows.Count > 0 && op == "add")
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "该成本项本月已经存在!",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            string data_id = this.DATA_ID.Value.ToString();
            string item_code = this.COSTS_ITEM.SelectedItem.Value.ToString();
            string item_name = this.COSTS_ITEM.SelectedItem.Text;
            string costs= this.COSTS.Value.ToString();
            string memo = this.REMARKS.Value.ToString();
            GoldNet.Model.User user = (GoldNet.Model.User)Session["CURRENTSTAFF"];
            string operators = user.UserId;
            try
            {
                dal.SaveTotalCosts(data_id,date_time, item_code, costs, item_name, operators, memo);
                //勾选同步时把该项成本添加到默认成本项目中
                //if (this.Tag.Checked && op.Equals("add"))
                //{
                //    dal.InsertIntoTotalCosts(item_code,item_name);
                //}
                //刷新父界面
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.RefreshData();");

                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "保存成功!",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Buttonsave_Click");
            }
        }
    }
}
