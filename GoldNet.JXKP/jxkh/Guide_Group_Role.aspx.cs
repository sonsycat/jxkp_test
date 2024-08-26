using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Model;
using System.Collections.Generic;

namespace GoldNet.JXKP.jxkh
{
    public partial class Guide_Group_Role : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string guidegathercode = Request.QueryString["guidegathercode"].ToString();
                SetDict(guidegathercode);
            }

        }
        //初始化
        public void SetDict(string guidegathercode)
        {
            this.GuideGatherCode.Value = guidegathercode;
            Goldnet.Dal.Guide_Group dal = new Goldnet.Dal.Guide_Group();
            DataTable dt1 = dal.GetNoSelectRole(guidegathercode).Tables[0];
            this.Store1.DataSource = dt1;
            this.Store1.DataBind();
            DataTable dt2 = dal.GetSelectRole(guidegathercode).Tables[0];
            this.Store2.DataSource = dt2;
            this.Store2.DataBind();
        }
        //保存
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            List<PageModels.roleselected> rolelist = e.Object<PageModels.roleselected>();
            Goldnet.Dal.Guide_Group dal = new Goldnet.Dal.Guide_Group();
            try
            {
                dal.SaveGatherRole(rolelist, this.GuideGatherCode.Value.ToString());

                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "保存完毕",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            catch
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "保存失败",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            
        }
        //返回
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RoleEditWin.hide();");
        }
    }
}
