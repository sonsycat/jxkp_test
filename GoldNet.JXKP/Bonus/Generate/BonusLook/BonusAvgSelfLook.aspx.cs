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
    public partial class BonusAvgSelfLook : PageBase
    {
        //----------------------------------------------------------
        //1、查看平均奖科室的人员设置和出勤天数以及奖金系数
        //2、查看所在科室的质量得分
        //3、查看科室单项奖惩
        //4、查看科室的其他奖励
        ///注意：在页面传递参数是有很多参数在本页面没有用到，但是在页面跳转（返回）时，有的参数是其他页面用到的，所以在此页面时参数只是传递作用。如TagMode
        //-------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string Years = this.GetStringByQueryStr("Years");
                string Months = this.GetStringByQueryStr("Months");
                string bonusid = this.GetStringByQueryStr("tagID");
                string tag = this.GetStringByQueryStr("tagMode");
                User user = (User)Session["CURRENTSTAFF"];
                string dept_code = user.AccountDeptCode;
                LookDeptBonus lookDeptBonus = new LookDeptBonus();
                //科室的效益
                DataTable dtAvg = lookDeptBonus.GetAvgDeptPerson(Years, Months, dept_code);
                if (dtAvg != null)
                {
                    SAvgPerson.DataSource = dtAvg;
                    SAvgPerson.DataBind();
                }
                //科室的质量得分
                DataTable dtQuality = lookDeptBonus.GetDeptQuality(Years, Months, dept_code);
                if (dtQuality != null)
                {
                    SQuality.DataSource = dtQuality;
                    SQuality.DataBind();
                }
                //科室的单项奖惩
                DataTable dtSimpleAward = lookDeptBonus.GetSimpleAward(Years, Months, dept_code);
                if (dtSimpleAward != null)
                {
                    SSimpleAward.DataSource = dtSimpleAward;
                    SSimpleAward.DataBind();
                }
                //科室的其他奖励
                DataTable dtOtherAward = lookDeptBonus.GetOtherAward(Years, Months, dept_code);
                if (dtOtherAward != null)
                {
                    SOtherAward.DataSource = dtOtherAward;
                    SOtherAward.DataBind();
                }
                }
        }
        //返回当奖金大概信息页面
        protected void Btn_Back(object sender, AjaxEventArgs e)
        {
            string bonusid = this.GetStringByQueryStr("tagID");
            string tag = this.GetStringByQueryStr("tagMode");
            Response.Redirect("../BonusShow.aspx?bonusid=" + this.EncryptTheQueryString(bonusid) + "&tag=" + this.EncryptTheQueryString(tag) + "&pageid=" + Request.QueryString["pageid"].ToString());

        }
        
        [AjaxMethod]
        //质量
        public void Btn_Quality_Click()
        {

        }
        [AjaxMethod]
        //科室单项奖惩详细信息
        public void Btn_SimleAward_Click()
        {
            string Years = this.GetStringByQueryStr("Years");
            string Months = this.GetStringByQueryStr("Months");
            LoadConfig loadcfg = getLoadConfig("BonusSimpleAwardDetail.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Years", Years));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Months", Months));
            SimpleWin.ClearContent();
            SimpleWin.Show();
            SimpleWin.LoadContent(loadcfg);
        }
        [AjaxMethod]
        //科室单项奖惩详细信息
        public void Btn_OtherAward_Click()
        {
            string Years = this.GetStringByQueryStr("Years");
            string Months = this.GetStringByQueryStr("Months");
            LoadConfig loadcfg = getLoadConfig("BonusOtherAwardDetail.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Years", Years));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Months", Months));
            OtherWin.ClearContent();
            OtherWin.Show();
            OtherWin.LoadContent(loadcfg);
        }
    }
}
