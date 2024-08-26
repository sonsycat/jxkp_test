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
    public partial class appor_prog_sqlexpress :PageBase
    {
        private Appor_Prog_Dict dal_appor = new Appor_Prog_Dict();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Store1.DataSource = dal_appor.GetProgSql();
                Store1.DataBind();
            }
        }
        //编辑按钮触发事件
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                LoadConfig loadcfg = getLoadConfig("appor_prog_sqlexpress_edit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("ApporProgSQLID", selectRow[0]["PROG_ITEM"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("ApproProgSQLEditMode", "Edit"));
                showDetailWin(loadcfg);
            }

        }
        //添加按钮触发事件
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("appor_prog_sqlexpress_edit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("ApproProgSQLEditMode", "Add"));
            showDetailWin(loadcfg);
        }
        //添加按钮触发事件
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string id = selectRow[0]["PROG_ITEM"].ToString();
                try
                {
                    dal_appor.DeleteProgSql(id);
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "删除成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Store_RefreshData(null, null);
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteApporProg");
                }

            }
        }

        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            Store1.DataSource = dal_appor.GetProgSql();
            Store1.DataBind();
        }
        //显示详细窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
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
