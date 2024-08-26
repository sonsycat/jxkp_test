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

namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class Manual_Decompose_deptset : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                string costvalue = Request["costvalue"].ToString();
                this.memu.Text = costvalue.ToString();
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
            User user = (User)Session["CURRENTSTAFF"];
            double costvalue = Convert.ToDouble(Request["costvalue"].ToString());
            string deptcode = Request["deptcode"].ToString();
            string itemcode = Request["itemcode"].ToString();
            string stardate = Request["stardate"].ToString();
            string enddate = Request["enddate"].ToString();
            double numbers = 0;
            List<PageModels.deptselected> deptlist = e.Object<PageModels.deptselected>();
            Goldnet.Dal.Appor_Prog_Dict dal = new Goldnet.Dal.Appor_Prog_Dict();
            foreach (GoldNet.Model.PageModels.deptselected dept in deptlist)
            {
                numbers += Convert.ToDouble(dept.RATIO);
            }
            if (Math.Round(numbers, 4) != costvalue)
            {
                this.ShowMessage("系统提示", "分摊总和:" + numbers + "，和要分解的金额不相等，请检查！");
                
            }
            else
            {
                try
                {
                    dal.SaveCosttoDeptDecom(deptlist, deptcode, itemcode, stardate, enddate, "1", user.UserName);
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData();");
                    scManager.AddScript("parent.deptset.hide();");
                    this.SaveSucceed();
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "SubmitData");

                }
            }
        }
        //[AjaxMethod]
        //public void SaveCostDecom()
        //{
        //    List<PageModels.deptselected> deptlist = new List<PageModels.deptselected>();
        //    User user = (User)Session["CURRENTSTAFF"];
        //    string deptcode = Request["deptcode"].ToString();
        //    string itemcode = Request["itemcode"].ToString();
        //    string stardate = Request["stardate"].ToString();
        //    string enddate = Request["enddate"].ToString();
        //    Goldnet.Dal.Appor_Prog_Dict dal = new Goldnet.Dal.Appor_Prog_Dict();
        //    dal.SaveCosttoDeptDecom(deptlist, deptcode, itemcode, stardate, enddate, "1", user.UserName);
        //    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
        //    scManager.AddScript("parent.RefreshData();");
        //    scManager.AddScript("parent.deptset.hide();");
        //}
        /// <summary>
        /// 选择科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedDept(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.Appor_Prog_Dict prodal = new Goldnet.Dal.Appor_Prog_Dict();
            this.Store1.DataSource = prodal.GetCheckDeptBydepttype(this.Combo_SelectDept.SelectedItem.Value);
            this.Store1.DataBind();
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.deptset.hide();");
        }

    }
}