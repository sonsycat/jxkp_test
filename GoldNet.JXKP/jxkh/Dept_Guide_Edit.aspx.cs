using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Collections.Generic;
using GoldNet.Model;

namespace GoldNet.JXKP.jxkh
{
    public partial class Dept_Guide_Edit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                Edit();
                STATION_BSC_CLASS_01.FieldLabel = GetConfig.GetConfigString("BSC01");
                STATION_BSC_CLASS_02.FieldLabel = GetConfig.GetConfigString("BSC02");
                STATION_BSC_CLASS_03.FieldLabel = GetConfig.GetConfigString("BSC03");
                STATION_BSC_CLASS_04.FieldLabel = GetConfig.GetConfigString("BSC04");
            }
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.DeptGuide_Edit.hide();");
        }
        /// <summary>
        /// 下拉框设置
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();

            DataTable roletable = dal.GetGuideGather().Tables[0];
            DataRow[] selrole = roletable.Select();
            foreach (DataRow dr in selrole)
            {
                this.Combo_Role.Items.Add(new Goldnet.Ext.Web.ListItem(dr["GATHER_NAME"].ToString(), dr["GATHER_CODE"].ToString()));
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public void Edit()
        {
            if (Request["deptcode"].ToString() != "")
            {

                Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();
                DataTable table = dal.GetDeptGuide(Request["years"].ToString(), Request["deptcode"].ToString(),"").Tables[0];
                if (table.Rows.Count > 0)
                {

                    this.Text_DeptName.Text = table.Rows[0]["DEPT_NAME"].ToString();
                    this.Combo_Role.SelectedItem.Value = table.Rows[0]["GATHER_CODE"].ToString();
                    this.STATION_BSC_CLASS_01.Text = table.Rows[0]["STATION_BSC_CLASS_01"].ToString() == "" ? "100" : table.Rows[0]["STATION_BSC_CLASS_01"].ToString();
                    this.STATION_BSC_CLASS_02.Text = table.Rows[0]["STATION_BSC_CLASS_02"].ToString() == "" ? "100" : table.Rows[0]["STATION_BSC_CLASS_02"].ToString();
                    this.STATION_BSC_CLASS_03.Text = table.Rows[0]["STATION_BSC_CLASS_03"].ToString() == "" ? "100" : table.Rows[0]["STATION_BSC_CLASS_03"].ToString();
                    this.STATION_BSC_CLASS_04.Text = table.Rows[0]["STATION_BSC_CLASS_04"].ToString() == "" ? "100" : table.Rows[0]["STATION_BSC_CLASS_04"].ToString();
                    Session["centertable"] = table;
                }
            }
            

        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            if (this.Combo_Role.SelectedItem.Value == string.Empty)
            {
                this.ShowMessage("系统提示", "指标集不能为空！");
            }
            else
            {
                Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();

                string years = Request["years"].ToString();
                string deptcode = Request["deptcode"].ToString();
                string gathercode = this.Combo_Role.SelectedItem.Value;
                string gather1 = STATION_BSC_CLASS_01.Text;
                string gather2 = STATION_BSC_CLASS_02.Text;
                string gather3 = STATION_BSC_CLASS_03.Text;
                string gather4 = STATION_BSC_CLASS_04.Text;
                
                try
                {
                    dal.UpdateDeptGather(deptcode, gathercode, gather1, gather2, gather3, gather4,years);
                    this.ShowMessage("提示", "保存成功！");
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("RefreshData();");
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "Buttonsave_Click");

                }

            }

        }
    }
}
