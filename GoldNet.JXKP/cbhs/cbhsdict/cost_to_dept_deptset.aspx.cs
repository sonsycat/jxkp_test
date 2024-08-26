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
namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class cost_to_dept_deptset : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                if (Request["indexid"] != null)
                {
                    SetValue(Request["indexid"].ToString());
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
            Goldnet.Dal.Appor_Prog_Dict dal = new Goldnet.Dal.Appor_Prog_Dict();
            try
            {
                dal.SaveCosttoDept(deptlist, Request["indexid"].ToString());
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
            Goldnet.Dal.Appor_Prog_Dict prodal = new Goldnet.Dal.Appor_Prog_Dict();
            this.Store1.DataSource = prodal.GetNoCheckDeptByindex(Request["indexid"].ToString(), this.Combo_SelectDept.SelectedItem.Value);
            this.Store1.DataBind();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public void SetValue(string progcode)
        {
            Selectedrole(null, null);
        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            //scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.deptset.hide();");
        }

        /// <summary>
        /// 分解方案项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Selectedrole(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.Appor_Prog_Dict prodal = new Goldnet.Dal.Appor_Prog_Dict();
            this.Store2.DataSource = prodal.GetCostTodept(Request["indexid"].ToString()).Tables[0];
            this.Store2.DataBind();
            SelectedDept(null, null);

        }
    }
}