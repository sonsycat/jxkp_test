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
using GoldNet.Model;
using GoldNet.Comm;
using System.Collections.Generic;
namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class Decompose_Detail : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Cost_detail dal = new Cost_detail();
                DataTable dt = dal.GetDecomposeDetail().Tables[0];
                this.Store1.DataSource = dt;
                this.Store1.DataBind();
            }
        }
       
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.Decompose.hide();");
        }
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            string date_time = Request["stardate"].Substring(0,4);
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
          
            List<CostDetail> costdetail = e.Object<CostDetail>();
            Cost_detail dal = new Cost_detail();
            string item_code = Request["itemcode"].ToString();
            User user = (User)Session["CURRENTSTAFF"];
            string operators = user.UserName;
            try
            {
                string datetime = Convert.ToDateTime(Request["stardate"].ToString().Substring(0, 4) + "-"+Request["stardate"].ToString().Substring(4, 2)+"-01").ToString("yyyy-MM-dd");
                dal.SaveDecomposeCosts(costdetail, item_code, datetime, operators, "1", Request["deptcode"].ToString()); 
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "保存成功",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
            }
        }
    }
}
