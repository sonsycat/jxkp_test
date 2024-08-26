using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using System.Collections;

namespace GoldNet.JXKP
{
    public partial class allcost_set : PageBase
    {
        private XyhsDict dal_appor = new XyhsDict();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string itemcode = Request["item_code"].ToString();
                Store1.DataSource = dal_appor.GetAllcost(itemcode);
                Store1.DataBind();
                XyhsDict dal = new XyhsDict(); 
                DataTable prog = dal.GetProgdict("9").Tables[0];
                for (int i = 0; i < prog.Rows.Count; i++)
                {
                    this.proglist.Items.Add(new Goldnet.Ext.Web.ListItem(prog.Rows[i]["PROG_NAME"].ToString(), prog.Rows[i]["PROG_NAME"].ToString()));
                }
            }
        }
       
        //添加按钮触发事件
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                try
                {
                    string itemcode = Request["item_code"].ToString();
                    dal_appor.SaveAllcostsdict(selectRow, itemcode);
                    this.SaveSucceed();
                }
                catch (Exception ex)
                {
                    this.ShowMessage("系统提示", "数据填写不完整！");
                }
            }
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
