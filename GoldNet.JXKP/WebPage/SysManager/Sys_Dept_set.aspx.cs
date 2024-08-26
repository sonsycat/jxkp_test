using System;
using System.Data;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Sys_Dept_set : PageBase
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                if (Request["APP_MENU_ID"] != null)
                {
                    SetValue(Request["APP_MENU_ID"].ToString());
                }
            }
        }

        /// <summary>
        /// 下拉框设置
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataRow[] depttyperow = dal.GetDeptType().Tables[0].Select();
            foreach (DataRow rw in depttyperow)
            {
                this.Combo_SelectDept.Items.Add(new Goldnet.Ext.Web.ListItem(rw["ATTRIBUE"].ToString(), rw["ID"].ToString()));
            }
        }

        /// <summary>
        /// 保存科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {

            List<PageModels.deptselected> deptlist = e.Object<PageModels.deptselected>();
            Goldnet.Dal.SE_ROLE dal = new Goldnet.Dal.SE_ROLE();
            try
            {
                dal.SaveMenuDept(deptlist, Request["APP_MENU_ID"].ToString());
                this.SaveSucceed();
            }
            catch (Exception ex)
            {
                ShowDataError(ex, Request.Url.LocalPath, "SubmitData");

            }
        }

        /// <summary>
        /// 选择科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedDept(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SE_ROLE prodal = new Goldnet.Dal.SE_ROLE();
            string menuclass = Request["MenuClass"].ToString();
            if (menuclass.Equals("0"))
            {
                this.Store1.DataSource = prodal.GetNoCheckDeptBymenuid(Request["APP_MENU_ID"].ToString(), this.Combo_SelectDept.SelectedItem.Value);
            }
            else
            {
                this.Store1.DataSource = prodal.GetNoCheckDeptBymenuidByGroup(Request["APP_MENU_ID"].ToString(), this.Combo_SelectDept.SelectedItem.Value);
            }
            this.Store1.DataBind();
        }

        /// <summary>
        /// 获取已选择的科室
        /// </summary>
        /// <param name="model"></param>
        public void SetValue(string menuid)
        {
            Goldnet.Dal.SE_ROLE prodal = new Goldnet.Dal.SE_ROLE();
            this.Store2.DataSource = prodal.GetDeptBymenuid(menuid).Tables[0];
            this.Store2.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            //scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.DeptWin.hide();");
        }

    }
}