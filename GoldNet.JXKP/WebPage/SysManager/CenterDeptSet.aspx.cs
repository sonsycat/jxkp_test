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
    public partial class CenterDeptSet :PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
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

            DataRow[] centerrow = dal.GetCenterList(0).Tables[0].Select();
            foreach (DataRow row in centerrow)
            {
                this.ComboBox_Center.Items.Add(new Goldnet.Ext.Web.ListItem(row["center_name"].ToString(), row["id"].ToString()));
            }

        }

        /// <summary>
        /// 选择专科中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedCenter(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            this.Store2.DataSource = dal.GetDeptByCenter(this.ComboBox_Center.SelectedItem.Value).Tables[0];
            this.Store2.DataBind();
            SelectedDept(null,null);

        }


        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            if (this.ComboBox_Center.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("提示", "专科中心不能为空！");
            }
            else
            {
                List<PageModels.deptselected> deptlist = e.Object<PageModels.deptselected>();
                Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
                try
                {
                    dal.SaveCenterDept(deptlist, int.Parse(this.ComboBox_Center.SelectedItem.Value));
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData();");
                    scManager.AddScript("parent.CenterDeptSet.hide();");
                   

                }
                catch(Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "SubmitData");

                }
            }
        }
        /// <summary>
        /// 选择科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedDept(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            this.Store1.DataSource = dal.GetDeptBytypeCenter(this.Combo_SelectDept.SelectedItem.Value,this.ComboBox_Center.SelectedItem.Value);
            this.Store1.DataBind();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public void SetValue(string centerid)
        {
            this.ComboBox_Center.SelectedItem.Value = centerid;
            SelectedCenter(null, null);
        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.CenterDeptSet.hide();");
        }
    }
}