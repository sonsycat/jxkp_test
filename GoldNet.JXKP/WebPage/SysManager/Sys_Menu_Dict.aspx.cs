using System;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Sys_Menu_Dict : PageBase
    {
        private SE_ROLE dal_prog = new SE_ROLE();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //获取主菜单
                DataTable appdb = dal_prog.GetApptype();
                Store1.DataSource = appdb;
                Store1.DataBind();

                //获取菜单类型
                DataTable menutypetb = dal_prog.GetMenutype();
                Store3.DataSource = menutypetb;
                Store3.DataBind();

                //获取菜单属性
                DataTable menuattrtb = dal_prog.GetMenuattr();
                Store4.DataSource = menuattrtb;
                Store4.DataBind();

                if (Request["EditMode"] != null)
                {
                    //编辑模式
                    string mode = Request["EditMode"].ToString();
                    if (mode == "Edit")
                    {
                        string menuclass = Request["MenuClass"].ToString();
                        string appid = Request["APP_MENU_ID"].ToString();
                        DataTable dt ;
                        if (menuclass.Equals("0"))
                        {
                            dt = dal_prog.GetSysMenulist(appid);
                        }
                        else
                        {
                            dt = dal_prog.GetSysMenulistByGroup(appid);
                        }

                        //DataTable dt = dal_prog.GetSysMenulist(appid);
                        if (dt.Rows.Count > 0)
                        {
                            apptype.SelectedItem.Value = dt.Rows[0]["MODID"].ToString();
                            Store2.DataSource = dal_prog.GetMenutype(appid);
                            Store2.DataBind();
                            menutype.SelectedItem.Value = dt.Rows[0]["GROUPTEXT"].ToString();
                            this.Menutypes.SelectedItem.Value = dt.Rows[0]["TYPE_ID"].ToString();
                            this.Menuattr.SelectedItem.Value = dt.Rows[0]["ATTR_ID"].ToString();
                            this.MENU_NAME.Text = dt.Rows[0]["APP_MENU_NAME"].ToString();
                            this.apptype.Disabled = true;
                            this.menutype.Disabled = true;
                            this.Menutypes.Disabled = true;

                        }
                    }
                    else if (mode == "Add")
                    {
                        //增加模式
                        if (appdb.Rows.Count > 0)
                            apptype.SelectedItem.Value = appdb.Rows[0]["APP_ID"].ToString();
                        if (menutypetb.Rows.Count > 0)
                            this.Menutypes.SelectedItem.Value = menutypetb.Rows[0]["TYPE_ID"].ToString();
                        if (menuattrtb.Rows.Count > 0)
                            this.Menuattr.SelectedItem.Value = menuattrtb.Rows[0]["ATTR_ID"].ToString();
                        Store2.DataSource = dal_prog.GetMenutype(appdb.Rows[0]["APP_ID"].ToString());
                        Store2.DataBind();
                        this.menutype.SelectedIndex = 0;
                        MENU_NAME.Text = "";
                    }

                }
            }
        }

        /// <summary>
        /// 选择类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Selectedtype(object sender, AjaxEventArgs e)
        {
            Store2.DataSource = dal_prog.GetMenutype(this.apptype.SelectedItem.Value);
            Store2.DataBind();
            this.menutype.SelectedIndex = 0;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveProg_onClick(object sender, AjaxEventArgs e)
        {
            if (Request["EditMode"] != null && this.apptype.SelectedItem.Value != "" && menutype.SelectedItem.Value != "" && MENU_NAME.Text != "")
            {
                string menuclass =Request["MenuClass"].ToString();
                string mode = Request["EditMode"].ToString();

                string menuname = MENU_NAME.Text.Trim().Replace("'", "''");

                if (mode == "Add")
                {
                    try
                    {
                        string apptype = this.apptype.SelectedItem.Value;
                        string menu_type = this.menutype.SelectedItem.Value;
                        string menutypes = this.Menutypes.SelectedItem.Value;
                        string menuattr = this.Menuattr.SelectedItem.Value;
                        string menutypename = this.Menutypes.SelectedItem.Text;
                        dal_prog.Saveappmenu(apptype, menu_type, menuname, menutypes, menuattr, menutypename, menuclass);
                        Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                        scManager.AddScript("parent.RefreshData('添加成功');");
                        scManager.AddScript("parent.MenuGuide.hide();");
                        scManager.AddScript("parent.MenuGuide.clearContent();");
                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveProgInsert");
                    }
                }
                else if (mode == "Edit")
                {
                    try
                    {
                        string appid = Request["APP_MENU_ID"].ToString();
                        string funcid = Request["FUNCTION_ID"].ToString();
                        string menuid = Request["MENUID"].ToString();
                        string apptype = this.apptype.SelectedItem.Value;
                        string menu_type = this.menutype.SelectedItem.Value;
                        string menuattr = this.Menuattr.SelectedItem.Value;
                        string menutypename = this.Menutypes.SelectedItem.Text;
                        dal_prog.Updateappmenu(appid, apptype, menu_type, menuname, funcid, menuid, menuattr, menutypename);
                        Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                        scManager.AddScript("parent.RefreshData('修改成功');");
                        scManager.AddScript("parent.MenuGuide.hide();");
                        scManager.AddScript("parent.MenuGuide.clearContent();");
                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveProgEidt");
                    }
                }
            }
        }

    }
}
