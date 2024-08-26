using System;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Collections.Generic;
using GoldNet.Model;



namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class cbhs_Dept_Contrast : PageBase
    {
        private BoundComm boundcomm = new BoundComm();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
               
              Bindlist();
            }
        }
        //查询科室信息表Dept_info
        protected void Bindlist()
        {
            AccountDeptType accountDeptType = new AccountDeptType();
            DataTable dt = accountDeptType.getContrastDept();
            if (dt == null)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message =  "查询数据失败",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }


            else
            {
                Store1.DataSource = dt;
                Store1.DataBind();
            }
        }

        //保存
        protected void Button_save(object sender, AjaxEventArgs e)     
        {
          
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            AccountDeptType accountDeptType = new AccountDeptType();
            try
            {
                if (selectRow == null || selectRow.Length == 0)
                {
                    return;
                }

                accountDeptType.SaveContrastDeptInfo(selectRow);
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "保存成功",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });

                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("Store1.reload();");
                Bindlist();
            }
            catch
            {
                Ext.Msg.Alert("提示", "保存失败").Show();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
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
