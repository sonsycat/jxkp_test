using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Model;

namespace GoldNet.JXKP
{
   /// <summary>
   /// 单独对平均奖科室添加一个人
   /// </summary>
    public partial class AverageBonusDaysAddPerson : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                if (Request["PersonYear"] != null && Request["PersonMonth"] != null)
                {
                    AverageBonusDays averagebonusdays = new AverageBonusDays();
                    string year = Request["PersonYear"];
                    string month = Request["PersonMonth"];
                    //只显示此年月的平均奖科室
                    Store2.DataSource = averagebonusdays.GetAverageDept(year, month,false);
                    Store2.DataBind();

                }
            }
        }
        /// <summary>
        /// 保存新建平均奖科室人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveAddPerson(object sender, AjaxEventArgs e)
        {
            if (Request["PersonYear"] != null && Request["PersonMonth"] != null)
            {
                string year = Request["PersonYear"];
                string month = Request["PersonMonth"];
                string deptid = cbbdept.SelectedItem.Value.ToString();
                string deptname = cbbdept.SelectedItem.Text.ToString();
                string staffname = tfStaffName.Text.Trim().Replace("'", "''");
                int isbonus = Convert.ToInt32(cbisbouns.Checked);
                int days = Convert.ToInt32(nfDAYS.Text);
                double bonusmodules = Convert.ToDouble(nfBONUSMODULUS.Text);

                try
                {
                    AverageBonusDays averagebonusdays = new AverageBonusDays();
                    if (averagebonusdays.CheckPersonByName(year, month, deptid, deptname, staffname))
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "提示",
                            Message = year+"年"+month+"月"+deptname+"的"+staffname+"重复,不可以添加",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        return;
                    }
                    //添加平均奖科室人员
                    User user = (User)Session["CURRENTSTAFF"];
                    
                    averagebonusdays.AddOnePerson(year, month, deptid, deptname, staffname, isbonus, days, bonusmodules, user);
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    //保存成功关闭窗口
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
