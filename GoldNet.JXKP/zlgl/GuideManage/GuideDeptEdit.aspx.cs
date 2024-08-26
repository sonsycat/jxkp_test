using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GoldNet.JXKP.PowerManager;
using GoldNet.Comm;
using GoldNet.JXKP.Templet.BLL;
using GoldNet.JXKP.BLL.Organise;
using Goldnet.Ext.Web;
using GoldNet.Model;

namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class GuideDeptEdit :PageBase
    {
        public int intID;
        public string straction;
        public string strsigntype;
        protected void Page_Load(object sender, EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            intID = Convert.ToInt32(Request.QueryString["id"].ToString());
            straction = Request["straction"].ToString();
            strsigntype = Request["strsigntype"].ToString();
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                DataSet ds = dal.SpecialtyGuide_Edit_View(intID);
                fieldset3.Title = ds.Tables[0].Rows[0]["SpecGuideName"].ToString();

                Goldnet.Dal.SYS_DEPT_DICT dept = new Goldnet.Dal.SYS_DEPT_DICT();
                string deptcodes = ds.Tables[0].Rows[0]["CheckDept"].ToString();
                this.Store2.DataSource = dept.GetDeptByDeptCodes(deptcodes.Substring(0, deptcodes.Length - 1));
                this.Store2.DataBind();
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
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            List<PageModels.deptselected> deptlist = e.Object<PageModels.deptselected>();
            StringBuilder strCheckDept = new StringBuilder();
            foreach (GoldNet.Model.PageModels.deptselected dept in deptlist)
            {
                strCheckDept.Append(dept.DEPT_CODE + ",");
            }
            try
            {
                dal.UpdateGuideDept(intID, strCheckDept.ToString());
                this.SaveSucceed();
            }
            catch (Exception ex)
            {
                ShowDataError(ex, Request.Url.LocalPath, "SubmitData");

            }
            
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("Guide_View.aspx");
        }
        /// <summary>
        /// 选择科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedDept(object sender, EventArgs e)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            this.Store1.DataSource = dal.GetDeptBytype(this.Combo_SelectDept.SelectedItem.Value);
            this.Store1.DataBind();
        }
    }
}
