using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;

namespace GoldNet.JXKP
{
    public partial class SimpleEncourageEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                if (Request["SimpleEncourageMode"] != null)
                {
                    string mode = Request["SimpleEncourageMode"].ToString();
                    if (mode=="Edit" && Request["SimpleEncourageID"] != null)
                    {
                        SimpleEncourage simpleEncourage = new SimpleEncourage();
                        string id = Request["SimpleEncourageID"].ToString();
                        DataTable dt = simpleEncourage.GetSimpleEncourage(id);
                        if (dt.Rows.Count > 0)
                        {
                            tfITEMNAME.Text = dt.Rows[0]["ITEMNAME"].ToString();
                            taCHECKSTAN.Text = dt.Rows[0]["CHECKSTAN"].ToString();
                            taREMARK.Text = dt.Rows[0]["REMARK"].ToString();
                        }
                    }
                    else if (mode == "Add")
                    {
                        tfITEMNAME.Text ="";
                        taCHECKSTAN.Text = "";
                        taREMARK.Text = "";
                    }
                }
            }
        }
        protected void SaveEditSimpleEncourage(object sender, AjaxEventArgs e)
        {
            if (Request["SimpleEncourageMode"] != null)
            {
                SimpleEncourage simpleencourage = new SimpleEncourage();
                string mode = Request["SimpleEncourageMode"].ToString();
                
                string itemname = tfITEMNAME.Text.Trim().Replace("'", "''");
                string checkstan = taCHECKSTAN.Text.Trim().Replace("'", "''");
                string remark = taREMARK.Text.Trim().Replace("'", "''");
                if (mode == "Add")
                {
                    try
                    {
                        simpleencourage.InsertSimpleEncourage(itemname, checkstan, remark);
                         Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    	{
                        Title ="提示",
                        Message = "添加成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    	}); 
                    	Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
			            scManager.AddScript("parent.RefreshData();"); 
			            scManager.AddScript("parent.DetailWin.hide();");
			            scManager.AddScript("parent.DetailWin.clearContent();");
                    	}
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveInSertSimpleEncourage");
                    }
                }
                else if (mode == "Edit")
                {
                    string id = Request["SimpleEncourageID"].ToString();
                    try
                    {
                        simpleencourage.UpdateSimpleEncourage(id, itemname, checkstan, remark);
                         Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    	{
                        Title ="提示",
                        Message = "修改成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    	}); 
                        Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
			            scManager.AddScript("parent.RefreshData();"); 
			            scManager.AddScript("parent.DetailWin.hide();");
			            scManager.AddScript("parent.DetailWin.clearContent();");
                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveEditSimpleEncourage");
                    }
                }
            }
          
           
        }
    }
}
