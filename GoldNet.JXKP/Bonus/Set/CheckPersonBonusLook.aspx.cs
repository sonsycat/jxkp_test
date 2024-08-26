using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP
{
    public partial class CheckPersonBonusLook : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }
            if (!Ext.IsAjaxRequest)
            {
                if (Request["CheckBonusYear"] != null && Request["CheckBonusMonth"] != null && Request["DeptID"] != null)
                {

                    string year = Request["CheckBonusYear"];
                    string month = Request["CheckBonusMonth"];
                    string deptid = Request["DeptID"];

                    BoundComm boundcomm = new BoundComm();
                    CheckPersonBonus checkpersonsbonus = new CheckPersonBonus();
                    Store3.DataSource = boundcomm.getYears();
                    Store3.DataBind();
                    cbbYear.SetValue(year);
                    Store4.DataSource = boundcomm.getMonth();
                    Store4.DataBind();
                    cbbmonth.SetValue(month);
                    if (boundcomm.GetAccountType(year, month, "10001','20001"))
                    {
                        //获得所在月份平均奖科室人的信息
                        Store1.DataSource = checkpersonsbonus.GetCheckPersonBounusDaysList(year, month, deptid, "");
                        Store1.DataBind();
                    }
                    else
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "提示",
                            Message = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月核算科室还未设置",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });

                        Store1.DataSource = checkpersonsbonus.BuildCheckPersonBounusDaysList();
                        Store1.DataBind();
                    }

                }
            }
        }

        protected void btn_Qurey_Click(object sender, AjaxEventArgs e)
        {
            GetPageData();
        }
        protected void btn_Refresh_Click(object sender, AjaxEventArgs e)
        {
            GetPageData();
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetPageData();
        }
        private void GetPageData()
        {

            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            string deptid = Request["DeptID"];
            BoundComm boundcomm = new BoundComm();
            CheckPersonBonus checkpersonsbonus = new CheckPersonBonus();
            if (boundcomm.GetAccountType(year, month, "10001','20001"))
            {
                //获得所在月份平均奖科室人的信息
                Store1.DataSource = checkpersonsbonus.GetCheckPersonBounusDaysList(year, month, deptid, "");
                Store1.DataBind();
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = year + "年" + month + "月核算科室还未设置",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });

                Store1.DataSource = checkpersonsbonus.BuildCheckPersonBounusDaysList();
                Store1.DataBind();
            }
        }
        protected void btn_Back_Click(object sender, AjaxEventArgs e)
        {
            string year = Request["CheckBonusYear"];
            string month = Request["CheckBonusMonth"];
            Mask.Config msgconfig = new Mask.Config();
            msgconfig.Msg = "页面转向中...";
            msgconfig.MsgCls = "x-mask-loading";
            Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
            Goldnet.Ext.Web.Ext.Redirect("CheckPersonsLook.aspx?CheckYear=" + year + "&CheckMonth=" + month + "&pageid=" + Request.QueryString["pageid"].ToString() + "");
        }
    }
}
