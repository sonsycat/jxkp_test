using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    public partial class CheckPersonBonusSimpleEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void SaveAddPerson(object sender, AjaxEventArgs e)
        {
            if (Request["PersonSimpleYear"] != null && Request["PersonSimpleMonth"] != null && Request["PersonSimpleDeptID"] != null && Request["PersonSimpleDeptName"] != null)
            {
                string year = Request["PersonSimpleYear"];
                string month = Request["PersonSimpleMonth"];
                string deptid = Request["PersonSimpleDeptID"];
                string deptname = Request["PersonSimpleDeptName"];
                string staffname = tfStaffName.Text.Trim().Replace("'", "''");
                int isbonus = Convert.ToInt32(cbisbouns.Checked);
                int days = Convert.ToInt32(nfDAYS.Text);
                double bonusmodules = Convert.ToDouble(nfBONUSMODULUS.Text);

                double PERSONSMODULUS = Convert.ToDouble(nfPERSONSMODULUS.Text);
                double SUBSIDYMODULUS = Convert.ToDouble(nfSUBSIDYMODULUS.Text);

                try
                {
                    CheckPersonBonus checkpersonbonus = new CheckPersonBonus();
                    if (checkpersonbonus.CheckPersonByName(year, month, deptid, deptname, staffname))
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "提示",
                            Message = year + "年" + month + "月" + deptname + "的" + staffname + "重复,不可以添加",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        return;
                    }
                    //添加平均奖科室人员
                    User user = (User)Session["CURRENTSTAFF"];

                    checkpersonbonus.AddOnePerson(year, month, deptid, deptname, staffname, isbonus, days, bonusmodules, user, PERSONSMODULUS, SUBSIDYMODULUS);
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    //保存成功关闭窗口
                    scManager.AddScript("parent.RefreshData();");
                    scManager.AddScript("parent.DetailWin.hide();");
                    scManager.AddScript("parent.DetailWin.clearContent();");
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveCheckPersonBonus");
                }

            }

        }

    }
}
