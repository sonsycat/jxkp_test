using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;


namespace GoldNet.JXKP
{
    public partial class BonusGuidePrimary : PageBase
    {
        Goldnet.Dal.BonusGuideDict dal_bonusGuideDict = new Goldnet.Dal.BonusGuideDict();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Store1.DataSource = dal_bonusGuideDict.GetPrimaryGuide();
                Store1.DataBind();
            }
        }

        /// <summary>
        /// 保存指标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save(object sender, AjaxEventArgs e)
        {
            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中            
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string msg = dal_bonusGuideDict.SetPrimaryGuide(selectRow);
                if (msg == "奖金基础指标设置成功")
                {
                    Store1.DataSource = dal_bonusGuideDict.GetPrimaryGuide();
                    Store1.DataBind();
                }
                else
                {
                    ShowDataError("", Request.Url.LocalPath, "SaveIndicationConfig");
                }
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = msg,
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
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
