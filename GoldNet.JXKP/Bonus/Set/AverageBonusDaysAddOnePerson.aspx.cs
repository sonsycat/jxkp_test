using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    public partial class AverageBonusDaysAddOnePerson : PageBase
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                if (Request["PersonYear"] != null && Request["PersonMonth"] != null)
                {
                    AverageBonusDays averagebonusdays = new AverageBonusDays();
                    string year = Request["PersonYear"];
                    string month = Request["PersonMonth"];
                    //根据年月获得平均奖科室
                    Store2.DataSource = averagebonusdays.GetAverageDeptAdd(year, month, false);
                    Store2.DataBind();
                }
            }
        }

        /// <summary>
        /// 检查输入是否合法并且添加到平均奖科室人员列表中
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
                string dayss = nfDAYS.Text == "" ? "0" : nfDAYS.Text;
                int days = Convert.ToInt32(dayss);
                string numbs = nfBONUSMODULUS.Text == "" ? "0" : nfBONUSMODULUS.Text;
                double bonusmodules = Convert.ToDouble(numbs);
                User user = (User)Session["CURRENTSTAFF"];
                AverageBonusDays averagebonusdays = new AverageBonusDays();
                if (averagebonusdays.CheckPersonByName(year, month, deptid, deptname, staffname))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = year + "年" + month + "月" + deptname + "的" + staffname + ",不可以添加",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                averagebonusdays.AddOnePerson(year, month, deptid, deptname, staffname, isbonus, days, bonusmodules, user);
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                //scManager.AddScript("parent.addRecord('" + year + "','" + month + "','" + deptid + "','" + deptname + "','" + staffname + "'," + isbonus + "," + days + "," + bonusmodules + ",0);");
                scManager.AddScript("parent.Store1.reload();");
                scManager.AddScript("parent.DetailWin.hide();");
                scManager.AddScript("parent.DetailWin.clearContent();");
            }
        }

    }
}
