using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using System.Collections;

namespace GoldNet.JXKP.cbhs.xyhs.xyhsdict
{
    public partial class xyhs_item_type_sfyl : PageBase
    {
        private XyhsDict dal_appor = new XyhsDict();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Store1.DataSource = dal_appor.GetItemTypeSfyl();
                Store1.DataBind();
            }
        }

        //添加按钮触发事件
        protected void Button_save_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {

                dal_appor.Saveitemtypesfyl(selectRow);
                this.SaveSucceed();

                Store1.DataSource = dal_appor.GetItemTypeSfyl();
                Store1.DataBind();

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
