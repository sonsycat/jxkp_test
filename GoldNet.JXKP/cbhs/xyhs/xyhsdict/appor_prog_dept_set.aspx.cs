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
using Goldnet.Dal.cbhs;
using Goldnet.Dal;

namespace GoldNet.JXKP.cbhs.xyhs.xyhsdict
{
    public partial class appor_prog_dept_set : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                if (Request["prog_code"] != null)
                {
                    SetValue(Request["prog_code"].ToString());
                }
            }
        }


        /// <summary>
        /// 下拉框设置
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataRow[] depttyperow = dal.GetXYHSDeptType2().Tables[0].Select();
            foreach (DataRow rw in depttyperow)
            {
                this.Combo_SelectDept.Items.Add(new Goldnet.Ext.Web.ListItem(rw["ATTRIBUE"].ToString(), rw["ID"].ToString()));
            }
            Xyhs_Appor_Prog_Dict prodal = new Xyhs_Appor_Prog_Dict();
            DataRow[] rolerow = prodal.GetFJFA().Tables[0].Select();
            foreach (DataRow row in rolerow)
            {
                this.ComboBox_Role.Items.Add(new Goldnet.Ext.Web.ListItem(row["PROG_NAME"].ToString(), row["PROG_CODE"].ToString()));
            }

        }



        /// <summary>
        /// 保存科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            if (this.ComboBox_Role.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("提示", "分解方案不能为空！");
            }
            else
            {
                List<PageModels.deptselected> deptlist = e.Object<PageModels.deptselected>();
                Xyhs_Appor_Prog_Dict dal = new Xyhs_Appor_Prog_Dict();
                try
                {
                    dal.SaveProgDept(deptlist, this.ComboBox_Role.SelectedItem.Value);
                    this.SaveSucceed();
                }
                catch (Exception ex)
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
            XyhsOperation prodal = new XyhsOperation();
            this.Store1.DataSource = prodal.GetNoCheckDeptByProg(this.ComboBox_Role.SelectedItem.Value, this.Combo_SelectDept.SelectedItem.Value);
            this.Store1.DataBind();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public void SetValue(string progcode)
        {
            this.ComboBox_Role.SelectedItem.Value = progcode;

            Selectedrole(null, null);
        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            //scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.DeptWin.hide();");
        }

        /// <summary>
        /// 分解方案项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Selectedrole(object sender, AjaxEventArgs e)
        {
            XyhsOperation prodal = new XyhsOperation();
            this.Store2.DataSource = prodal.GetDeptByProg(this.ComboBox_Role.SelectedItem.Value).Tables[0];
            this.Store2.DataBind();
            SelectedDept(null, null);

        }






    }
}
