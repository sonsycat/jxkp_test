using System;
using System.Collections.Generic;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Sys_Menu_List : PageBase
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
                Store1.DataSource = dal_appor.GetSysMenulist("");
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
                LoadConfig loadcfg = getLoadConfig("Sys_Menu_Dict.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("APP_MENU_ID", selectRow[0]["APP_MENU_ID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("FUNCTION_ID", selectRow[0]["FUNCTION_ID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("MENUID", selectRow[0]["MENUID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("EditMode", "Edit"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("MenuClass", "0"));
                showCenterSet(this.Menudict, loadcfg);
            }
        }

        /// <summary>
        /// 添加按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("Sys_Menu_Dict.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("EditMode", "Add"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("MenuClass", "0"));
            showCenterSet(this.Menudict, loadcfg);
        }

        /// <summary>
        /// 属性设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Attr_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("Sys_Menu_Attr.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("MenuClass", "0"));
            showCenterSet(this.Menuattr, loadcfg);
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
                string appid = selectRow[0]["APP_MENU_ID"].ToString();
                string modid = selectRow[0]["MODID"].ToString();
                string menuid = selectRow[0]["MENUID"].ToString();
                string funcid = selectRow[0]["FUNCTION_ID"].ToString();
                try
                {
                    dal_appor.DeleteAppMenu(appid, modid, menuid, funcid);
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
            Store1.DataSource = dal_appor.GetSysMenulist("");
            Store1.DataBind();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuid"></param>
        [AjaxMethod]
        public void SetCost(string menuid)
        {
            LoadConfig loadcfg = getLoadConfig("Sys_Menu_set.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("APP_MENU_ID", menuid));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("MenuClass", "0"));
            showCenterSet(this.CostWin, loadcfg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuid"></param>
        [AjaxMethod]
        public void SetDept(string menuid)
        {
            LoadConfig loadcfg = getLoadConfig("Sys_Dept_set.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("APP_MENU_ID", menuid));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("MenuClass", "0"));
            showCenterSet(this.DeptWin, loadcfg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuid"></param>
        [AjaxMethod]
        public void SetGuide(string menuid)
        {
            LoadConfig loadcfg = getLoadConfig("Sys_Menu_Guide.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("APP_MENU_ID", menuid));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("MenuClass", "0"));
            showCenterSet(this.MenuGuide, loadcfg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuid"></param>
        [AjaxMethod]
        public void SetGuidedetail(string menuid)
        {
            LoadConfig loadcfg = getLoadConfig("Sys_Menu_Guide_Detail_Set.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("APP_MENU_ID", menuid));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("MenuClass", "0"));
            showCenterSet(this.MenuGuide, loadcfg);
        }
    }
}
