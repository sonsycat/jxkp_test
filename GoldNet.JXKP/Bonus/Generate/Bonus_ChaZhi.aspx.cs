using System;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.Bonus.Generate
{
    public partial class Bonus_ChaZhi : System.Web.UI.Page
    {
        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                BoundComm boundcomm = new BoundComm();
                //初始化年
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.SetValue(DateTime.Now.Year);
                //初始化月
                SMonths.DataSource = boundcomm.getMonth();
                SMonths.DataBind();
                cbbmonth.SetValue(DateTime.Now.Month);
                //初始化条件大于，小于等
                StoreRelation.DataSource = boundcomm.dtBonusAmount();
                StoreRelation.DataBind();
            }
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["Bonus_ChaZhi"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["Bonus_ChaZhi"];

                ex.ExportToLocal(dt, this.Page, "xls", "奖金汇总差值");
                //MHeaderTabletoExcel(dt, null, null, null, 0);
                //ex.ExportToLocal(l_dt, this.Page, "xls", "人员信息");
            }

        }
        /// <summary>
        /// 根据页面输入的条件，查询人员奖金
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            string years = cbbYear.SelectedItem.Value;
            string months = cbbmonth.SelectedItem.Value;
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            string benginDate = years + "" + months ;
            //string deptName = tfDeptName.Text;
            //string staffName = tfStaff.Text;
            //string bankCode = tfBankCode.Text;
            //string bonus = "";
            //if (cbbBonus.SelectedItem.Value != "")
            //{
            //    bonus = cbbBonus.SelectedItem.Value + nfBonus.Text;
            //}
            
            //查询人员奖金
            CalculateBonus calculateBonus_dal = new CalculateBonus();
            DataTable dt = calculateBonus_dal.SearchBonus_ChaZhi(years, months, benginDate);
            if (dt.Rows.Count > 0)
            {//找到时绑定数据源
                SSearch.DataSource = dt;
                SSearch.DataBind();
                Session.Remove("Bonus_ChaZhi");
                Session["Bonus_ChaZhi"] = calculateBonus_dal.SearchBonus_ChaZhi(years, months, benginDate);
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "未找到奖金",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }
    }
}