using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    public partial class CheckPersonBonusAdd : PageBase
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
                if (Request["DeptID"] != null && Request["CheckBonusYear"] != null && Request["CheckBonusMonth"] != null)
                {
                    //初始化年，月
                    string year = Request["CheckBonusYear"];
                    string month = Request["CheckBonusMonth"];
                    BoundComm boundcomm = new BoundComm();
                    CheckPersonBonus checkpersonbonus = new CheckPersonBonus();
                    Store3.DataSource = boundcomm.getYears();
                    Store3.DataBind();
                    cbbYear.SetValue(year);
                    Store4.DataSource = boundcomm.getMonth();
                    Store4.DataBind();
                    cbbmonth.SetValue(month);
                    if (boundcomm.GetAccountType(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), "10001','20001"))
                    {

                        DataTable dtPerson = checkpersonbonus.GetCheckPersonAdd(year, month, Request["DeptID"]);
                        Store1.DataSource = dtPerson;
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

                        Store1.DataSource = checkpersonbonus.BuildCheckPersonBounusDaysList();
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
        }

        /// <summary>
        /// 根据年月奖科室取得科室的人
        /// </summary>
        private void GetPageData()
        {
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            BoundComm boundcomm = new BoundComm();
            CheckPersonBonus checkpersonbonus = new CheckPersonBonus();
            if (boundcomm.GetAccountType(year, month, "10001','20001"))
            {
                DataTable dtPerson = checkpersonbonus.GetCheckPersonAdd(year, month, Request["DeptID"]);
                Store1.DataSource = dtPerson;
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

                Store1.DataSource = checkpersonbonus.BuildCheckPersonBounusDaysList();
                Store1.DataBind();
            }
        }
        /// <summary>
        /// 保存奖科室的人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Save_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                try
                {
                    User user = (User)Session["CURRENTSTAFF"];
                    CheckPersonBonus checkpersonsbonus = new CheckPersonBonus();
                    checkpersonsbonus.SaveCheckPersonBonusDays(selectRow, cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString(), user, Request["DeptID"], "");

                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "保存成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveAddCheckPersonBonusDaysDetail");
                }
            }
        }

        /// <summary>
        /// 返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Back_Click(object sender, AjaxEventArgs e)
        {
            string year = Request["CheckBonusYear"];
            string month = Request["CheckBonusMonth"];
            Mask.Config msgconfig = new Mask.Config();
            msgconfig.Msg = "页面转向中...";
            msgconfig.MsgCls = "x-mask-loading";
            Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
            Goldnet.Ext.Web.Ext.Redirect("CheckPersonsAdd.aspx?CheckYear=" + year + "&CheckMonth=" + month + "&pageid=" + Request.QueryString["pageid"].ToString() + "");
        }

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);

            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        //对奖添加人
        protected void btn_Add_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("CheckPersonBonusSimpleAdd.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonSimpleYear", Request["CheckBonusYear"].ToString()));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonSimpleMonth", Request["CheckBonusMonth"].ToString()));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonSimpleDeptID", Request["DeptID"]));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonSimpleDeptName", Request["DeptName"].ToString()));
            showDetailWin(loadcfg);
        }
        //显示详细窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }
    }
}
