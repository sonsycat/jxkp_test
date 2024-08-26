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
using GoldNet.JXKP.BLL.Guide;

namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class GuideTypeEdit : PageBase
    {
        public int intID;
        public string straction;
        protected void Page_Load(object sender, EventArgs e)
        {
           //this.BtnSave.Click += new EventHandler(BtnSave_Click);
            intID = Convert.ToInt32(Request.QueryString["id"].ToString());   //要修改的指标类别ID号
            straction = Request["straction"].ToString();                     //添加或者修改标志
            if (!Ext.IsAjaxRequest)
            {
                this.Text_guidenumber.Visible = false;
                if (straction == "edit")
                {
                    Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
                    DataTable tb=dal.GetGuideTypeByid(intID).Tables[0];
                    if(tb.Rows.Count>0)
                    {
                        this.Text_guidetype.Text = tb.Rows[0]["GUIDETYPE"].ToString();
                        this.Text_guidenumber.Text = tb.Rows[0]["GUIDETYPENUM"].ToString();
                        if (tb.Rows[0]["TypeSign"].ToString() == "1")
                        {
                            this.Text_guidenumber.Visible = true;
                        }
                        else
                            this.Text_guidenumber.Visible = false;
                    }
                    
                }
            }
        }
        protected void BtnSave_Click(object sender, AjaxEventArgs e)
        {
            bool flash=true;
            if (Text_guidetype.Text.Equals(string.Empty))
            {
                flash = false;
                this.ShowMessage("系统提示", "指标分类不能为空！");
            }
            else if (!this.Text_guidenumber.Text.Equals(string.Empty))
            {
                try
                {
                    double num = double.Parse(this.Text_guidenumber.Text);
                }
                catch
                {
                    flash = false;
                    this.ShowMessage("系统提示", "分值请输入数字！");
                }
            }
            if (flash)
            {
                Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
                try
                {
                    //保存指标类别数据
                    if (straction == "add")   //保存新增数据
                    {
                        dal.GuideType_Add(this.Text_guidetype.Text.ToString(), "0");
                        Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                        scManager.AddScript("parent.RefreshData();");
                        scManager.AddScript("parent.guidetypeedit.hide();");

                    }
                    else
                    {
                        if (straction == "edit")  //保存修改数据
                        {
                            string GuideType = this.Text_guidetype.Text.ToString();
                            int GuideTypeNum = Convert.ToInt32(this.Text_guidenumber.Text.ToString());

                            dal.GuideType_Edit(GuideType, GuideTypeNum, intID);
                            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                            scManager.AddScript("parent.RefreshData();");
                            scManager.AddScript("parent.guidetypeedit.hide();");
                        }
                    }
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
            scManager.AddScript("parent.guidetypeedit.hide();");
        }
    }
}
