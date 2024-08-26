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

namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class cost_item_power : System.Web.UI.Page
    {
        static string item_code = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            { 

                //查找科室信息
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                pro.Url = "../WebService/HisUsers.ashx" ;
                this.Store2.Proxy.Add(pro);
                JsonReader jr = new JsonReader();
                jr.ReaderID = "STAFF_ID";
                jr.Root = "STAFFINFO";
                jr.TotalProperty = "TOTALCOUNT";
                RecordField rf = new RecordField();
                rf.Name = "STAFF_ID";
                jr.Fields.Add(rf);
                RecordField rfn = new RecordField();
                rfn.Name = "STAFF_NAME";
                jr.Fields.Add(rfn);
                this.Store2.Reader.Add(jr);


                item_code = Request.QueryString["item_code"].ToString();
                Bindlist();
            }
        }
        protected void Bindlist()
        {
            Cbhs_dict dal = new Cbhs_dict();
            DataTable dt= dal.GetCostItemPower(item_code).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //添加权限人员
        protected void Button_add_click(object sender, EventArgs e)
        {
            string user_id = this.USER.SelectedItem.Value.ToString();
            string user_name = this.USER.SelectedItem.Text.ToString();
            Cbhs_dict dal = new Cbhs_dict();
            try
            {
                bool _is=dal.PowerIsExist(item_code,user_id);
                if (_is)
                {
                    Ext.Msg.Alert("提示","您添加的用户已经存在").Show();
                }
                else
                {
                    dal.AddCostItemPower(item_code,user_id,user_name);
                    Bindlist();
                }

            }
            catch
            {
                Ext.Msg.Alert("提示","添加失败");
            }
        }
        protected void Button_del_click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                Ext.Msg.Alert("提示", "请选择要移除的人员！").Show();
            }
            else
            {
                string user_id = sm.SelectedRow.RecordID;
                Cbhs_dict dal = new Cbhs_dict();
                try
                {
                    dal.DelCostItemPower(item_code, user_id);
                    Bindlist();
                }
                catch
                {
                    Ext.Msg.Alert("提示","移除失败").Show();
                }
                
               
            }
        }
        protected void Button_accept_click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData('设置完成');");
            scManager.AddScript("parent.PowerWin.hide();");
        }
    }
}
