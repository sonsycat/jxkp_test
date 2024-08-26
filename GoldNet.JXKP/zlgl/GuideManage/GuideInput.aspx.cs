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
    public partial class GuideInput : PageBase
    {
        public int lbx;
        public string lby;
        public int lbz;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataFiller();
                int guidtypeid = Convert.ToInt32(Request["GuideTypeID"].ToString());
                if (guidtypeid == 9)
                {
                    this.TBManaDept.Visible = false;
                    this.DEPT_INPUT.Visible = true;
                    this.DEPT_ID.Visible = true;
                }
                else
                {
                    this.DEPT_INPUT.Visible = false;
                    this.DEPT_ID.Visible = false;
                    this.TBManaDept.Visible = true;

                }
                if (guidtypeid == 2)
                {
                    this.Label1.Visible = true;
                    this.Dropdownlisttype.Visible = true;
                }
                this.DDLGuideType.Enabled = false;
                this.DDLGuideName.Enabled = false;

            }
        }
        private void DataFiller()
        {
            //获取指标类别信息
            Goldnet.Dal.ZLGL_Guide_Dict dal = new Goldnet.Dal.ZLGL_Guide_Dict();
            int guidtypeid = Convert.ToInt32(Request["GuideTypeID"].ToString());
            DDLGuideType.DataSource = dal.Guide_Type_Dict_by_id(guidtypeid).Tables[0].DefaultView;
            DDLGuideType.DataTextField = "GuideType";
            DDLGuideType.DataValueField = "ID";
            DDLGuideType.DataBind();
            DDLGuideType.SelectedIndex = DDLGuideType.Items.IndexOf(DDLGuideType.Items.FindByValue(guidtypeid.ToString()));
            

            DDLGuideType.Items.Add("请选择指标类别");
            DDLGuideType.Items[DDLGuideType.Items.Count - 1].Text = "请选择指标类别";
            DDLGuideType.Items[DDLGuideType.Items.Count - 1].Value = "0";
            //DDLGuideType.Items[DDLGuideType.Items.Count - 1].Selected = true;

            DDLGuideName.Items.Add("请选择指标名称");
            DDLGuideName.Items[DDLGuideName.Items.Count - 1].Text = "请选择指标名称";
            DDLGuideName.Items[DDLGuideName.Items.Count - 1].Value = "0";
           // DDLGuideName.Items[DDLGuideName.Items.Count - 1].Selected = true;
            DDLGuideType_SelectedIndexChanged(null,null);
            
        }

        protected void DDLGuideType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //获取某一指标类别下的指标名称信息
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            lbx = Convert.ToInt32(DDLGuideType.SelectedItem.Value.ToString());  //被选择指标类别的ID号
            lby = dal.GuideType_Edit_View(lbx).Tables[0].Rows[0]["TypeSign"].ToString();   //某一类别对应的类别标志
            DDLGuideName.DataSource = dal.Guide_Name_Dict(lby, lbx).Tables[0].DefaultView;
            DDLGuideName.DataTextField = "GuideName";
            DDLGuideName.DataValueField = "GuideNameID";
            DDLGuideName.DataBind();
            DDLGuideName.Items.Add("请选择指标名称");
            DDLGuideName.Items[DDLGuideName.Items.Count - 1].Text = "请选择指标名称";
            DDLGuideName.Items[DDLGuideName.Items.Count - 1].Value = "0";
            //DDLGuideName.Items[DDLGuideName.Items.Count - 1].Selected = true;
            TBManaDept.Text = "";
            TBGuideNum.Text = "";
            TBCheckDept.Text = "";
            DDLGuideName.SelectedIndex = DDLGuideName.Items.IndexOf(DDLGuideName.Items.FindByValue(Request["GuideNameID"].ToString()));
            DDLGuideName_SelectedIndexChanged(null,null);
        }

        protected void DDLGuideName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //填充主管部门、考评分值、考核科室
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            lbx = Convert.ToInt32(DDLGuideType.SelectedItem.Value.ToString());  //被选择指标类别的ID号
            lby = dal.GuideType_Edit_View(lbx).Tables[0].Rows[0]["TypeSign"].ToString();   //某一类别对应的类别标志
            lbz = Convert.ToInt32(DDLGuideName.SelectedItem.Value.ToString());      //被选择指标名称的ID号
            DataSet ds = dal.Guide_Name(lby, lbz);
            if (ds.Tables[0].Rows.Count > 0)
            {
                TBManaDept.Text = ds.Tables[0].Rows[0]["ManaDept"].ToString();
                TBGuideNum.Text = ds.Tables[0].Rows[0]["GuideNum"].ToString();
                if (lby != "0")
                {
                    TBCheckDept.Text = "";
                }
                else if (lby == "1")
                {
                    TBCheckDept.Text = ds.Tables[0].Rows[0]["CheckDept"].ToString();
                }
                GuideTypeID.Text = lbx.ToString();
                TypeSign.Text = lby.ToString();
                GuideNameID.Text = lbz.ToString();
            }

            if (this.DDLGuideName.SelectedValue != "0")
            {
                this.Dropdownlisttype.DataSource = dal.Conten_Type_all(DDLGuideName.SelectedItem.Text).Tables[0].DefaultView;
                this.Dropdownlisttype.DataTextField = "CONTENT_TYPE";
                this.Dropdownlisttype.DataValueField = "ID";
                this.Dropdownlisttype.DataBind();
            }

        }

        protected void BtnSave_Click(object sender, AjaxEventArgs e)
        {
            if (TBCheckCont.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "考评内容不能为空！");
            }
            else if (TBCheckStan.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "考评标准不能为空！");
            }
            else
            {
                Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();

                string strCheckCont = TBCheckCont.Text.ToString();
                string strCheckStan = TBCheckStan.Text.ToString();
                string strCheckMeth = TBCheckMeth.Text.ToString();
                int intGuideNameID = Convert.ToInt32(GuideNameID.Text.ToString());
                int intGuideTypeID = Convert.ToInt32(GuideTypeID.Text.ToString());
                int iskpi = this.iskpi.Checked == true ? 1 : 0;
                try
                {
                    if (this.TBManaDept.Visible == true)
                    {
                        bool GuideCheck = dal.GuideCheckContent_Add(strCheckCont, strCheckStan, strCheckMeth, intGuideNameID, intGuideTypeID, this.Dropdownlisttype.SelectedValue.ToString(),iskpi);
                    }
                    else
                    {
                        bool GuideCheck = dal.GuideCheckContentdept_Add(strCheckCont, strCheckStan, strCheckMeth, intGuideNameID, intGuideTypeID, this.DEPT_ID.Value.Trim());


                    }
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData();");
                    scManager.AddScript("parent.guidecontedit.hide();");
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "BtnSave_Click");

                }
            }
           
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.guidecontedit.hide();");
        }
    }
}
