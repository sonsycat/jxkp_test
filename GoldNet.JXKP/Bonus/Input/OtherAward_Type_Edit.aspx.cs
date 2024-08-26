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
using Goldnet.Dal;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class OtherAward_Type_Edit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Edit();
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            if (this.Text_CenterName.Text == string.Empty)
            {
                this.ShowMessage("系统提示", "项目名称不能为空！");
            }
            else
            {
                InputOtherAward dal = new InputOtherAward();

                string typename = this.Text_CenterName.Text;

                int id = int.Parse(Request["id"].ToString());
               
                try
                {
                    dal.SaveOtherAwardType(id, typename);
                    this.ShowMessage("提示", "保存成功！");
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData();");
                    scManager.AddScript("parent.add_edit.hide();");
                    scManager.AddScript("parent.add_edit.clearContent();");
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "Buttonsave_Click");

                }

            }

        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public void Edit()
        {
            if (Request["id"].ToString() != "0")
            {
                InputOtherAward dal = new InputOtherAward();
                DataTable table = dal.GetOtherAwardType(Request["id"].ToString());
                if (table.Rows.Count > 0)
                {
                    this.Text_CenterName.Text = table.Rows[0]["NAME"].ToString();   
                    
                }
            }
            else
            {
                NewValue();
            }

        }
        /// <summary>
        /// 置空新增
        /// </summary>
        public void NewValue()
        {
            this.Text_CenterName.Text = string.Empty;
      
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.add_edit.hide();");
        }
    }
}