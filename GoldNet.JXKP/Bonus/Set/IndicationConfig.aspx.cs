using System;
using System.Collections.Generic;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP
{
    public partial class IndicationConfig : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                IndicationConfigDAL indicationConfig = new IndicationConfigDAL();
                Store1.DataSource = indicationConfig.GetIndicationConfig();
                Store1.DataBind(); 
            }
        }
        protected void Save(object sender, AjaxEventArgs e)
        {
            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中            
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                IndicationConfigDAL indicationConfig = new IndicationConfigDAL();
                if (indicationConfig.SaveDeptPercent(selectRow))
                {                    
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title ="提示",
                        Message = "设置成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });  
                    Store1.DataSource = indicationConfig.GetIndicationConfig();
                    Store1.DataBind();
                }
                else
                {
                    ShowDataError("", Request.Url.LocalPath, "SaveIndicationConfig");
                }
            }
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {           
            string row = e.ExtraParams["Values"].ToString();
            if (row.Length == 2)
            {
                return null;
            }
            int index = row.IndexOf(':', 0);
            row = row.Substring(index+1, row.Length - index-2);
            row=row.Replace("\\","");

            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);

            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
    }
}
