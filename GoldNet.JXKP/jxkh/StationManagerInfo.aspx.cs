using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Data;
using GoldNet.Model;

namespace GoldNet.JXKP.jxkh
{
    public partial class StationManagerInfo : System.Web.UI.Page
    {
        private Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                     Response.End();
                     return;
                }
                string stationYear = "";
                string stationCode = "";
                if (Request["id"] != null)
                {
                    stationCode = Request["id"].ToString();
                }
                if (Request["sy"] != null)
                {
                    stationYear = Request["sy"].ToString();
                }
                if ((stationCode == "") || (stationYear == ""))
                {
                    Response.End();
                    return;
                }
                //if ((!Int32.TryParse(stationYear, out tmpid)) || (!Int32.TryParse(stationCode, out tmpid)))
                //{
                //    Response.End();
                //    return;
                //}
                SetInitState(stationCode, stationYear);
            }            
        }

        /// <summary>
        /// 设置页面各控件状态
        /// </summary>
        /// <param name="stationCode"></param>
        /// <param name="stationYear"></param>
        private void SetInitState(string stationCode, string stationYear)
        {
            this.StationCodeTxt.Value = stationCode;
            this.StationYearTxt.Value = stationYear;


            DataTable dt = dal.GetStationDeptList(stationCode, stationYear).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.DeptCombo.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["DEPT_NAME"].ToString(), dt.Rows[i]["DEPT_CODE"].ToString()));
            }
            //岗位类别
            dt = dal.GetDUTY_ORDER().Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.DeptDutyCombo.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["STATION_TYPE_NAME"].ToString(), dt.Rows[i]["STATION_TYPE_CODE"].ToString()));
            }
            //指标集类别
            dt = dal.GetGuide_type().Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.GatherTypeCombo.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["GUIDE_GROUP_TYPE_NAME"].ToString(), dt.Rows[i]["GUIDE_GROUP_TYPE"].ToString()));
            }
            dt = dal.GetStationInfo(stationCode,stationYear).Tables[0];
            if (dt.Rows.Count == 0)
            {
                Response.End();
                return;
            }
            this.StationNameTxt.Value = dt.Rows[0]["STATION_NAME"].ToString();
            DateTime dd = default(DateTime);
            if (DateTime.TryParse(dt.Rows[0]["INPUT_TIME"].ToString(), out dd))
            {
                this.CreateDate.Value = dd;
            }

            this.ScoreNum1.Text = dt.Rows[0]["STATION_BSC_CLASS_01"].ToString();
            this.ScoreNum2.Text = dt.Rows[0]["STATION_BSC_CLASS_02"].ToString();
            this.ScoreNum3.Text = dt.Rows[0]["STATION_BSC_CLASS_03"].ToString();
            this.ScoreNum4.Text = dt.Rows[0]["STATION_BSC_CLASS_04"].ToString();
            this.StationTxt.Value = dt.Rows[0]["STATION_CODE_REMARK"].ToString();
            this.WorkTxt.Value = dt.Rows[0]["WORK_CONTENT"].ToString();
            this.TitleTxt.Value = dt.Rows[0]["POST_COMPETENCY"].ToString();
            this.JobTxt.Value = dt.Rows[0]["WORK_CIRCUMSTANCE"].ToString();
            this.SalaryTxt.Text = dt.Rows[0]["STIPEND"].ToString();
            this.ApplyTxt.Text = dt.Rows[0]["AUDITING_USER"].ToString();
            this.CreateTxt.Text = dt.Rows[0]["INPUT_USER"].ToString();

            this.Radio1.Checked = dt.Rows[0]["WHETHER"].ToString().Equals("是") ? true : false;
            this.Radio2.Checked = !this.Radio1.Checked;
            this.DeptCombo.Value = dt.Rows[0]["DEPT_CODE"].ToString();
            this.DeptDutyCombo.Value = dt.Rows[0]["DUTY_ORDER"].ToString();
            this.GatherTypeCombo.Value = dt.Rows[0]["GUIDE_TYPE"].ToString();

            //DataTable dt1 = dal.GetGuideGather("").Tables[0];
            DataTable dt1 = dal.GetGuideGather(dt.Rows[0]["GUIDE_TYPE"].ToString()).Tables[0];
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                this.GuideGatherCombo.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt1.Rows[i]["GUIDE_GATHER_NAME"].ToString(), dt1.Rows[i]["GUIDE_GATHER_CODE"].ToString().Trim()));
            }
            this.GuideGatherCombo.Value = dt.Rows[0]["GUIDE_GATHER_CODE"].ToString();
            this.GuideGather_Original.Value = dt.Rows[0]["GUIDE_GATHER_CODE"].ToString();

            if (Session["CURRENTSTAFF"] != null)
            {
                if (dt.Rows[0]["INPUT_USER"].ToString().Trim().Equals(""))
                {
                    this.CreateTxt.Text = ((User)Session["CURRENTSTAFF"]).UserName;
                }
            }

        }

        /// <summary>
        /// 指标集类别选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedGatherType(object sender, AjaxEventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript(GuideGatherCombo.ClientID + ".store.removeAll();");
            scManager.AddScript(GuideGatherCombo.ClientID + ".clearValue();");
            string gatherType = this.GatherTypeCombo.SelectedItem.Value.ToString();
            if (!gatherType.Equals(""))
            {
                DataTable dt = dal.GetGuideGather(gatherType).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.GuideGatherCombo.AddItem(dt.Rows[i]["GUIDE_GATHER_NAME"].ToString(), dt.Rows[i]["GUIDE_GATHER_CODE"].ToString());
                }
                this.GuideGatherCombo.SelectedIndex = 0;
            }

        }

        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSave_Click(object sender, AjaxEventArgs e)
        {
            string Values = e.ExtraParams["Values"].ToString();
            Dictionary<string, string> FormValues = JSON.Deserialize<Dictionary<string, string>>(Values);
            dal.UpdateSatationInfo(FormValues);

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData();");
            //不需要进行岗位指标量化操作，直接关闭窗口
            if ((FormValues["GuideGatherCombo_Value"] == "") || (FormValues["GuideGather_Original"] == FormValues["GuideGatherCombo_Value"]))
            {
                scManager.AddScript("parent.DetailWin.hide();");
            }
            //页面迁移至岗位指标量化画面,同时扩大窗口尺寸并居中
            else
            {
                scManager.AddScript("parent.DetailWin.setTitle('岗位指标量化--" + FormValues["StationYearTxt"] + "年度');");
                //scManager.AddScript("parent.DetailWin.maximize();");
                scManager.AddScript("var width = parent.Ext.getBody().getViewSize().width; parent.DetailWin.setWidth(width - 20); parent.DetailWin.center();");

                Mask.Config msgconfig = new Mask.Config();
                msgconfig.Msg = "转向中...";
                msgconfig.MsgCls = "x-mask-loading";
                Ext.Mask.Show(msgconfig);
                Ext.Redirect("StationGuideInformation.aspx?id=" + FormValues["StationCodeTxt"] + "&sy=" + FormValues["StationYearTxt"] + "&gd=" + FormValues["GuideGatherCombo_Value"]);
            }

        }

    }
}
