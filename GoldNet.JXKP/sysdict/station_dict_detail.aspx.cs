using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using System.Data;

namespace GoldNet.JXKP.sysdict
{
    public partial class station_dict_detail : System.Web.UI.Page
    {
        private Goldnet.Dal.Station_Dict dal = new Goldnet.Dal.Station_Dict();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
 			//检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }
            if (!Ext.IsAjaxRequest)
            {
                string opflg = "";
                string opid = "";
                if (Request["op"] != null)
                {
                    opflg = Request["op"].ToString().ToLower();
                }
                if (Request["id"] != null)
                {
                    opid = Request["id"].ToString();
                }
                if ((!opflg.Equals("add")) && (!opflg.Equals("edit")) && (!opflg.Equals("del")))
                {
                    Response.End();
                    return;
                }
                int tmpid;
                if ((!opid.Equals("")) && (!Int32.TryParse(opid, out tmpid)))
                {
                    Response.End();
                    return;
                }
                SetInitState(opflg, opid);
            }
           
        }

        //设置页面各控件状态
        private void SetInitState(string op, string id)
        {
            if ((!op.Equals("add")) && (id.Equals("")))
            {
                Response.End();
                return;
            }

            DataTable dt = dal.GetStationTypeList().Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.StationTypeCombo.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["TYPENAME"].ToString(), dt.Rows[i]["TYPECODE"].ToString()));
            }
            dt = dal.GetDUTY_ORDER().Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.DeptDutyCombo.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["STATION_TYPE_NAME"].ToString(), dt.Rows[i]["STATION_TYPE_CODE"].ToString()));
            }
            dt = dal.GetGuide_type().Tables[0];
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.GatherTypeCombo.Items.Insert(i , new Goldnet.Ext.Web.ListItem(dt.Rows[i]["GUIDE_GROUP_TYPE_NAME"].ToString(), dt.Rows[i]["GUIDE_GROUP_TYPE"].ToString()));
            }

            if (!id.Equals(""))
            {
                dt = dal.GetStationDetail(id).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    Response.End();
                    return;
                }

                if (!op.Equals("add"))
                {
                    this.StationNameTxt.Value = dt.Rows[0]["STATION_NAME"].ToString();
                    DateTime dd = default(DateTime);
                    if (DateTime.TryParse(dt.Rows[0]["INPUT_TIME"].ToString(), out dd))
                    {
                        this.CreateDate.Value = dd;
                    }
                }
                this.SortNoNum.Text = dt.Rows[0]["SEQUENCE"].ToString();
                this.SortNoNumHidden.Text = dt.Rows[0]["SEQUENCE"].ToString();
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
                this.StationTypeCombo.Value = dt.Rows[0]["STATION_TYPE_CODE"].ToString();
                this.DeptDutyCombo.Value = dt.Rows[0]["DUTY_ORDER"].ToString();
                this.GatherTypeCombo.Value = dt.Rows[0]["GUIDE_TYPE"].ToString();
                DataTable dt1 = dal.GetGuideGather(dt.Rows[0]["GUIDE_TYPE"].ToString()).Tables[0];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    this.GuideGatherCombo.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt1.Rows[i]["GUIDE_GATHER_NAME"].ToString(), dt1.Rows[i]["GUIDE_GATHER_CODE"].ToString()));
                }
                this.GuideGatherCombo.Value = dt.Rows[0]["GUIDE_GATHER_CODE"].ToString();
            }

            if (op.Equals("add"))
            {
                if (id.Equals(""))
                {
                    this.StationTypeCombo.SelectedIndex = 0;
                }
                this.CreateDate.Value = DateTime.Now;
                this.BtnSave.Icon = (Icon)Enum.Parse(typeof(Icon), "Add");
                this.BtnSave.Text = "添加";
            }
            if (op.Equals("edit"))
            {
                this.BtnSave.Text = "保存";
            }

            if (op.Equals("del"))
            {
                this.BtnSave.Icon =   (Icon)Enum.Parse(typeof(Icon), "Delete");
                this.BtnSave.Text="删除";
                this.Panel1.Disabled = true;
                this.Btn_ShowOrder.Disabled = true;
                this.StationTypeCombo.Disabled = true;
                this.GuideGatherCombo.Disabled = true;
                this.GatherTypeCombo.Disabled = true;
                this.DeptDutyCombo.Disabled = true;
                this.CreateDate.Disabled = true;
            }
        }

        //指标集类别选择事件
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

        //排序按钮触发事件
        protected void ShowSortNum(object sender, AjaxEventArgs e)
        {
            string DeptType = this.StationTypeCombo.SelectedItem.Value;
            if (DeptType.Equals("") )
            {
                return;
            }
            DataTable dt = dal.GetStationDictListSort(DeptType).Tables[0];
            this.Store1.DataSource = dt;            
            this.Store1.DataBind();
        }

        //确定按钮事件
        protected void BtnSave_Click(object sender, AjaxEventArgs e)
        {
            string id = this.Request["id"].ToString();
            string rtnstr = "";

            //删除时
            if (this.BtnSave.Text.Equals("删除"))
            {
                rtnstr = dal.DeleteSatationDict(id);

            }
            else
            {

                string Values = e.ExtraParams["Values"].ToString();
                Dictionary<string, string> FormValues = JSON.Deserialize<Dictionary<string, string>>(Values);
                //新增时
                if (this.BtnSave.Text.Equals("添加"))
                {
                    dal.InsertSatationDict(FormValues,"");
                }
                //编辑时
                else if (this.BtnSave.Text.Equals("保存"))
                {
                    dal.UpdateSatationDict(FormValues, id);
                }

            }

            if (rtnstr.Equals(""))
            {
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.RefreshData();");
                scManager.AddScript("parent.DetailWin.hide();");
                scManager.AddScript("parent.DetailWin.clearContent();");
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle1,
                    Message = rtnstr,
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }

    }
}
