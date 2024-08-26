using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    public partial class CheckPersonsAdd : PageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {


                string year = DateTime.Today.Year.ToString();
                string month = DateTime.Today.Month.ToString();

                if (Request["CheckYear"] != null && Request["CheckMonth"] != null)
                {
                    year = Request["CheckYear"];
                    month = Request["CheckMonth"];
                }

                BoundComm boundcomm = new BoundComm();
                CheckPersons checkpersons = new CheckPersons();
                Store3.DataSource = boundcomm.getYears();
                Store3.DataBind();
                cbbYear.SetValue(year);
                Store4.DataSource = boundcomm.getMonth();
                Store4.DataBind();
                cbbmonth.SetValue(month);
                if (boundcomm.GetAccountType(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), "30001','40001','50001','80001','60001"))
                {
                    Store1.DataSource = checkpersons.GetNewCheckPerson(year, month);
                    Store1.DataBind();
                    Store2.DataSource = checkpersons.GetNewCheckDept(year, month, true);
                    Store2.DataBind();
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

                    Store1.DataSource = checkpersons.BuildCheckPersons();
                    Store1.DataBind();
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Qurey_Click(object sender, AjaxEventArgs e)
        {
            GetPageData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Refresh_Click(object sender, AjaxEventArgs e)
        {
            GetPageData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetPageData();
        }

        /// <summary>
        /// 根据核算年月查找核算科室人数
        /// </summary>
        private void GetPageData()
        {

            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            BoundComm boundcomm = new BoundComm();
            CheckPersons checkpersons = new CheckPersons();
            if (boundcomm.GetAccountType(year, month, "30001','40001','50001','80001','60001"))
            {

                Store1.DataSource = checkpersons.GetNewCheckPerson(year, month);
                Store1.DataBind();
                Store2.DataSource = checkpersons.GetNewCheckDept(year, month, true);
                Store2.DataBind();
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
                Store1.DataSource = checkpersons.BuildCheckPersons();
                Store1.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Save_Click(object sender, AjaxEventArgs e)
        {
            string pageid = Request["pageid"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                try
                {
                    User user = (User)Session["CURRENTSTAFF"];
                    CheckPersons checkpersons = new CheckPersons();
                    checkpersons.SaveCheckPersons(selectRow, cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString(), user);
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "核算科人数设置成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Store1.DataSource = checkpersons.GetCheckPersons(cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString(),this.DeptFilter("",pageid));
                    Store1.DataBind();
                    Response.Redirect("CheckPersonsList.aspx?pageid=" + Request.QueryString["pageid"].ToString() + "");
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveCheckPersons");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Persons_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                CheckPersons checkpersons = new CheckPersons();
                string year = selectRow[0]["YEARS"];
                string month = selectRow[0]["MONTHS"];
                string deptid = selectRow[0]["DEPTID"];
                string deptname = selectRow[0]["DEPTNAME"];
                if (checkpersons.GetcheckPerson(year, month, deptid))
                {
                    Mask.Config msgconfig = new Mask.Config();
                    msgconfig.Msg = "页面转向中...";
                    msgconfig.MsgCls = "x-mask-loading";
                    Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
                    Goldnet.Ext.Web.Ext.Redirect("CheckPersonBonusAdd.aspx?CheckBonusYear=" + year + "&CheckBonusMonth=" + month + "&DeptID=" + deptid + "&DeptName=" + deptname);
                }
                else
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "请先设置" + deptname + "核算科人数",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                }
            }
        }
        protected void btn_Back_Click(object sender, AjaxEventArgs e)
        {
            Mask.Config msgconfig = new Mask.Config();
            msgconfig.Msg = "页面转向中...";
            msgconfig.MsgCls = "x-mask-loading";
            Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
            Goldnet.Ext.Web.Ext.Redirect("CheckPersonsList.aspx?pageid=" + Request.QueryString["pageid"].ToString() + "");
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
    }
}
