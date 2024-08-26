using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Comm;
using GoldNet.Model;
namespace GoldNet.JXKP
{
    public partial class BonusBenefitDetail : PageBase
    {
        //------------------------------------------------------------
        //1、显示科室收入，按收入项目分组
        //2、显示科室的成本，按成本项目分组
        //------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {

                string Years = Request["Years"].ToString();
                string Months = Request["Months"].ToString();
                User user = (User)Session["CURRENTSTAFF"];
                string dept_code = user.AccountDeptCode;
                LookDeptBonus lookDeptBonus = new LookDeptBonus();
                //查询科室收入
                DataTable dtIncomes = lookDeptBonus.GetDeptIncomesDetail(Years, Months, dept_code);
                if (dtIncomes != null)
                {
                    SIncomes.DataSource = dtIncomes;
                    SIncomes.DataBind();
                }
                //查询科室成本
                DataTable dtCosts = lookDeptBonus.GetDeptCostsDetail(Years, Months, dept_code);
                if (dtCosts != null)
                {
                    SCost.DataSource = dtCosts;
                    SCost.DataBind();
                }
               
               
            }
        }       
    }
}
