using System;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Model;
namespace GoldNet.JXKP
{
    public partial class BonusSimpleAwardDetail : PageBase
    {
        //------------------------------------------------------------
        //1、科室单项奖惩明细
        //
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
                //查询科室单项奖惩明细
                DataTable dtSimpleAward = lookDeptBonus.GetSimpleAwardDetail(Years, Months, dept_code);
                if (dtSimpleAward != null)
                {
                    SSimpleAward.DataSource = dtSimpleAward;
                    SSimpleAward.DataBind();
                }
                gpSimpleAward.Title = "科室单项奖惩信息";
            }
        }
        //页面跳转
        protected void Btn_Back(object sender, AjaxEventArgs e)
        {
            //tagID：奖金ID，tagMode：页面的模式：test生成测算，详见BonusShow.aspx注释 DeptType：Avg平均奖科室，Acc核算科室
            string Years = this.GetStringByQueryStr("Years");
            string Months = this.GetStringByQueryStr("Months");
            string bonusid = this.GetStringByQueryStr("tagID");
            string tag = this.GetStringByQueryStr("tagMode"); string deptType = this.GetStringByQueryStr("DeptType");
            if (deptType == "Avg")
            {//如果科室是平均奖科室，返回到平均奖科室的奖金明细分类查看页面 Avg
                Response.Redirect("BonusAvgSelfLook.aspx?Years=" + this.EncryptTheQueryString(Years) + "&Months=" + this.EncryptTheQueryString(Months) + "&tagMode=" + this.EncryptTheQueryString(tag) + "&tagID=" + this.EncryptTheQueryString(bonusid));
            }
            else
            {//如果科室是核算科室，返回到核算科室的奖金明细分类查看页面 Acc
                Response.Redirect("BonusAccountSelfLook.aspx?Years=" + this.EncryptTheQueryString(Years) + "&Months=" + this.EncryptTheQueryString(Months) + "&tagMode=" + this.EncryptTheQueryString(tag) + "&tagID=" + this.EncryptTheQueryString(bonusid));
            }

        }
    }
}
