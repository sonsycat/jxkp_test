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
    public partial class GuideEdit :PageBase
    {
        public int intID;
        public string strGuideName;
        public int GuideTypeID;
        public int GuideNameID;
        protected void Page_Load(object sender, EventArgs e)
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            intID = Convert.ToInt32(Request.QueryString["id"].ToString());
            //strGuideName = Request.QueryString["GuideName"].ToString();
            GuideTypeID = Convert.ToInt32(Request.QueryString["GuideTypeID"].ToString());
            GuideNameID = Convert.ToInt32(Request.QueryString["GuideNameID"].ToString());
            //strGuideName=dal.GuideNameByguide(GuideNameID.ToString(), GuideTypeID.ToString());
            if (!IsPostBack)
            {
                DataFiller();
            }
        }
        private void DataFiller()
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            //显示要修改的考评内容
            DataSet ds = new DataSet();
            ds = dal.Guide_Cont_Dict(intID);
            TBGuideName.Text = dal.GuideNameByguide(GuideNameID.ToString(), GuideTypeID.ToString());
            TBCheckCont.Text = ds.Tables[0].Rows[0]["CheckCont"].ToString();
            TBCheckStan.Text = ds.Tables[0].Rows[0]["CheckStan"].ToString();
            TBCheckMeth.Text = ds.Tables[0].Rows[0]["CheckMeth"].ToString();
            this.iskpi.Checked = ds.Tables[0].Rows[0]["ISKPI"].ToString() == "1" ? true : false;
        }

        protected void BtnSave_Click(object sender, AjaxEventArgs e)
        {
            if (TBCheckCont.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示","考评内容不能为空！");
            }
            else if (TBCheckStan.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "考评标准不能为空！");
            }
            else
            {
                Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
                //保存要修改的数据
                string strCheckCont = TBCheckCont.Text.ToString();
                string strCheckStan = TBCheckStan.Text.ToString();
                string strCheckMeth = TBCheckMeth.Text.ToString();
                int iskpi = this.iskpi.Checked == true ? 1 : 0;
                try
                {
                    bool GuideCont = dal.GuideCheckContent_Edit(strCheckCont, strCheckStan, strCheckMeth, intID,iskpi);
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
