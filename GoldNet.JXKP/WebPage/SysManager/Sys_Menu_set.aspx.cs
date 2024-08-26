using System;
using System.Collections.Generic;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Sys_Menu_set : PageBase
    {
        private SE_ROLE dal_appor = new SE_ROLE();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Store1.DataSource = dal_appor.GetSysMenu(Request["APP_MENU_ID"].ToString(), "");
                Store1.DataBind();
            }
        }

        /// <summary>
        /// 编辑按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                LoadConfig loadcfg = getLoadConfig("Sys_Menu_Edit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("APP_MENU_ID", Request["APP_MENU_ID"].ToString()));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("MENU_GUIDE_ID", selectRow[0]["MENU_GUIDE_ID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("EditMode", "Edit"));
                showDetailWin(loadcfg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Select_Click(object sender, AjaxEventArgs e)
        {

            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                if (selectRow[0]["FIELD_TYPE"].ToString().Equals("1"))
                {
                    LoadConfig loadcfg = getLoadConfig("Sys_Menu_Item.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("APP_MENU_ID", Request["APP_MENU_ID"].ToString()));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("MENU_GUIDE_ID", selectRow[0]["MENU_GUIDE_ID"]));
                    showCenterSet(this.MenuitemSet, loadcfg);
                }
                else
                {
                    this.ShowMessage("提示", "字段类型为“选项”才能有选项设置！");
                }
            }
        }

        /// <summary>
        /// 添加按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("Sys_Menu_Edit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("APP_MENU_ID", Request["APP_MENU_ID"].ToString()));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("EditMode", "Add"));
            showDetailWin(loadcfg);
        }

        /// <summary>
        /// 添加按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string appid = Request["APP_MENU_ID"].ToString();
                string menuid = selectRow[0]["MENU_GUIDE_ID"].ToString();
                try
                {
                    dal_appor.DeleteMenu(appid, menuid);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            Store1.DataSource = dal_appor.GetSysMenu(Request["APP_MENU_ID"].ToString(), "");
            Store1.DataBind();
        }

        /// <summary>
        /// 显示详细窗口
        /// </summary>
        /// <param name="loadcfg"></param>
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        /// <summary>
        /// 反序列化得到客户端提交的gridpanel数据行 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

    }
}
