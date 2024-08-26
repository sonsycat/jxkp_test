using System;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.Pic;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class DeptCollate : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Goldnet.Dal.BoundComm boundcomm = new Goldnet.Dal.BoundComm();
                Goldnet.Dal.SYS_DEPT_INFO sysdeptinfo = new Goldnet.Dal.SYS_DEPT_INFO();
                Store1.DataSource = sysdeptinfo.GetDeptInfo(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
                Store1.DataBind();
                Store3.DataSource = boundcomm.getYears();
                Store3.DataBind();
                cbbYear.SetValue(DateTime.Now.Year);
                Store4.DataSource = boundcomm.getMonth();
                Store4.DataBind();
                cbbmonth.SetValue(DateTime.Now.Month);
               

            }
        }
        protected void Btn_Save_Click(object sender, AjaxEventArgs e)
        {  
            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中            
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                try
                {
                    Goldnet.Dal.SYS_DEPT_INFO sysdeptinfo = new Goldnet.Dal.SYS_DEPT_INFO();
                    sysdeptinfo.SaveDeptInfo(selectRow, cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString());
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "科室快照保存成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Store1.DataSource = sysdeptinfo.GetDeptInfo(cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString());
                    Store1.DataBind();
                   
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveDeptCollate");
                }
            }
        
        }
        //编辑按钮触发事件
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                LoadConfig loadcfg = getLoadConfig("DeptCollateEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("DeptCollateID", selectRow[0]["ID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("DeptCollateMode", "Edit"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Year", cbbYear.SelectedItem.Value));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Month", cbbmonth.SelectedItem.Value));
                showDetailWin(loadcfg);
            }

        }
         //添加按钮触发事件
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string years = cbbYear.SelectedItem.Value.ToString();
            string months = cbbmonth.SelectedItem.Value.ToString();
            Goldnet.Dal.SYS_DEPT_INFO sysdeptinfo = new Goldnet.Dal.SYS_DEPT_INFO();
            Store1.DataSource = sysdeptinfo.GetDeptInfo(years, months);
            Store1.DataBind();
        }
        //添加按钮触发事件
        protected void Btn_Collate_Click(object sender, AjaxEventArgs e)
        {
            string years = cbbYear.SelectedItem.Value.ToString();
            string months = cbbmonth.SelectedItem.Value.ToString();
            Goldnet.Dal.SYS_DEPT_INFO sysdeptinfo = new Goldnet.Dal.SYS_DEPT_INFO();
            sysdeptinfo.DeptCollateInfo(years, months);
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = "提示",
                Message = "更新成功",
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
            Store1.DataSource = sysdeptinfo.GetDeptInfo(years, months);
            Store1.DataBind();
        }
        //添加按钮触发事件
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("DeptCollateEdit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("DeptCollateMode", "Add"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Year",cbbYear.SelectedItem.Value));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Month", cbbmonth.SelectedItem.Value));
            showDetailWin(loadcfg);
        }
        //添加按钮触发事件
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string id = selectRow[0]["ID"].ToString();
                Goldnet.Dal.SYS_DEPT_INFO sysdeptinfo = new Goldnet.Dal.SYS_DEPT_INFO();
                try
                {
                    sysdeptinfo.Delete(id);
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
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteDeptCollate");
                }

            }
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            Goldnet.Dal.SYS_DEPT_INFO sysdeptinfo = new Goldnet.Dal.SYS_DEPT_INFO();
            Store1.DataSource = sysdeptinfo.GetDeptInfo(cbbYear.SelectedItem.Value, cbbmonth.SelectedItem.Value);
            Store1.DataBind();
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        //显示详细窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }
        ////载入参数设置
        //private LoadConfig getLoadConfig(string url)
        //{
        //    LoadConfig loadcfg = new LoadConfig();
        //    loadcfg.Url = url;
        //    loadcfg.Mode = LoadMode.IFrame;
        //    loadcfg.MaskMsg = "载入中...";
        //    loadcfg.ShowMask = true;
        //    loadcfg.NoCache = true;
        //    return loadcfg;
        //}       
    }
}
