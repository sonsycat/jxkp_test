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
namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class ContenTypeEdit :PageBase
    {
        public int intID=0;
        public string straction;
        public string guidetypeid;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.BtnSave.Click += new EventHandler(BtnSave_Click);
            intID = Convert.ToInt32(Request.QueryString["id"].ToString());   //要修改的指标类别ID号
            straction = Request["straction"].ToString();                     //添加或者修改标志
            guidetypeid = Request["guidetype"].ToString();
            if (!Ext.IsAjaxRequest)
            {
                if (straction == "edit")
                {
                    Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
                    DataTable tb = dal.GetContenTypeByid(intID).Tables[0];
                    if (tb.Rows.Count > 0)
                    {
                        this.Text_contentype.Text = tb.Rows[0]["CONTENT_TYPE"].ToString();
                    }

                }
            }
        }

        void BtnSave_Click(object sender, EventArgs e)
        {
             
            if (this.Text_contentype.Text == "")
            {
                this.ShowMessage("系统提示", "指标内容分类不能为空！");
            }
            else
            {
                try
                {
                    Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
                    dal.UpdateContenType(intID, straction, Text_contentype.Text, guidetypeid);
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData();");
                    scManager.AddScript("parent.contentypeedit.hide();");
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
            scManager.AddScript("parent.contentypeedit.hide();");
        }
    }
}
