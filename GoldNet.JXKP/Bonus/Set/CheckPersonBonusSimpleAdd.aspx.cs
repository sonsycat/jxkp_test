using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;


namespace GoldNet.JXKP.Bonus.Set
{
    public partial class CheckPersonBonusSimpleAdd : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 检查输入是否合法并且添加到奖科室人员列表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                CheckPersonBonus checkpersonbonus = new CheckPersonBonus();
                if (checkpersonbonus.CheckPersonByName(year, month, deptid, deptname, staffname))
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

                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.addRecord('" + year + "','" + month + "','" + deptid + "','" + deptname + "','" + staffname + "'," + isbonus + "," + days + "," + bonusmodules + "," + PERSONSMODULUS + "," + SUBSIDYMODULUS + ");");
                scManager.AddScript("parent.DetailWin.hide();");
                scManager.AddScript("parent.DetailWin.clearContent();");
            }

        }
    }
}
