using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using GoldNet.Comm;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;
namespace GoldNet.JXKP
{
    public partial class UpdateUserPass : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {

            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string oldPass, newPass;

            if (txtYpass.Text.Equals(string.Empty))
            {
                txtYpass.Text = "";
                txtNewpass.Text = "";
                txtNewPassT.Text = "";
                this.ShowMessage("系统提示", "原密码不能为空！");
            }
            else if (txtNewpass.Text.Equals(string.Empty))
            {
                txtYpass.Text = "";
                txtNewpass.Text = "";
                txtNewPassT.Text = "";
                this.ShowMessage("系统提示", "新密码不能为空！");
              
            }
            else if (txtNewpass.Text != txtNewPassT.Text)
            {
                txtYpass.Text = "";
                txtNewpass.Text = "";
                txtNewPassT.Text = "";
                this.ShowMessage("系统提示", "重复密码和新密码不相同！");
               
            }
            else
            {
                //oldPass = CleanString.InputText(txtYpass.Text, 25);
                //newPass = CleanString.InputText(txtNewpass.Text.ToUpper().ToString(), 25);
                oldPass = DESEncrypt.Encrypt(txtYpass.Text.ToString().Trim());
                newPass = DESEncrypt.Encrypt(txtNewpass.Text.ToString().Trim());
                //try
                //{
                //    //showorder = int.Parse(this.showorder.Text);
                //}
                //catch
                //{
                //    this.ShowMessage("系统提示", "显示顺序请输入数字！");
                //}
                try
                {
                    Goldnet.Dal.TempList ti = new TempList();
                    User user = (User)Session["CURRENTSTAFF"];
                    string dept_code = user.UserId;
                    DeptPercent deptpercent = new DeptPercent();
                    if (ti.UpdateUserpass(dept_code, oldPass, newPass))
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "提示",
                            Message = "密码设置成功",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        txtNewpass.Text = "";
                        txtNewPassT.Text = "";
                        txtYpass.Text = "";
                    }
                    else
                    {
                        ShowDataError("", Request.Url.LocalPath, "Button1_Click");
                    }
                }

                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "Button1_Click");

                }
            }
        }
        //protected void save_Click(object sender, AjaxEventArgs e)
        //{
        //    string oldPass, newPass;

        //    if (txtYpass.Text.Equals(string.Empty))
        //    {
        //        this.ShowMessage("系统提示", "原密码不能为空！");
        //    }
        //    else if (txtNewpass.Text.Equals(string.Empty))
        //    {
        //        this.ShowMessage("系统提示", "新密码不能为空！");
        //    }
        //    else
        //    {
        //        oldPass = CleanString.InputText(txtYpass.Text, 25);
        //        newPass = CleanString.InputText(txtNewpass.Text, 25);

        //        //try
        //        //{
        //        //    //showorder = int.Parse(this.showorder.Text);
        //        //}
        //        //catch
        //        //{
        //        //    this.ShowMessage("系统提示", "显示顺序请输入数字！");
        //        //}
        //        try
        //        {
        //            Goldnet.Dal.TempList ti = new TempList();
        //            User user = (User)Session["CURRENTSTAFF"];
        //            string dept_code = user.UserId;
        //            DeptPercent deptpercent = new DeptPercent();
        //            if (ti.UpdateUserpass(dept_code, oldPass, newPass))
        //            {
        //                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
        //                {
        //                    Title = "提示",
        //                    Message = "密码设置成功",
        //                    Buttons = MessageBox.Button.OK,
        //                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
        //                });
        //            }
        //            else
        //            {
        //                ShowDataError("", Request.Url.LocalPath, "save_Click");
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            ShowDataError(ex, Request.Url.LocalPath, "save_Click");

        //        }
        //    }


        //}
    }

}
