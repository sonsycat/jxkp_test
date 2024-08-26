using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    public partial class CheckPersonsEdit : PageBase
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
                if (Request["CheckYear"] != null && Request["CheckMonth"] != null)
                {
                    string year = Request["CheckYear"];
                    string month = Request["CheckMonth"];
                    string pageid = Request["pageid"];
                    //年月下拉列表初始化
                    BoundComm boundcomm = new BoundComm();
                    CheckPersons checkpersons = new CheckPersons();
                    Store3.DataSource = boundcomm.getYears();
                    Store3.DataBind();
                    cbbYear.SetValue(year);
                    Store4.DataSource = boundcomm.getMonth();
                    Store4.DataBind();
                    cbbmonth.SetValue(month);

                    if (boundcomm.GetAccountTypeByGroup(year, month, "01"))
                    {
                        //
                        Store1.DataSource = checkpersons.GetCheckPersons(year, month,this.DeptFilter("",pageid));
                        Store1.DataBind();
                        //Store2.DataSource = checkpersons.GetCheckDept(year, month, true);
                        //Store2.DataBind();
                    }
                    else
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "提示",
                            Message = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月核算科室还未设置",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO"),
                            Modal = true
                        });

                        Store1.DataSource = checkpersons.BuildCheckPersons();
                        Store1.DataBind();
                    }

                    Store5.DataSource = checkpersons.GetDeptType(year, month);
                    Store5.DataBind();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitType()
        {

        }

        /// <summary>
        /// 查询处理
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
            string pageid = Request["pageid"];
            BoundComm boundcomm = new BoundComm();
            CheckPersons checkpersons = new CheckPersons();
            if (boundcomm.GetAccountType(year, month, "10001','20001"))
            {
                Store1.DataSource = checkpersons.GetCheckPersons(year, month, this.DeptFilter("", pageid));
                Store1.DataBind();
                //Store2.DataSource = checkpersons.GetCheckDept(year, month, true);
                //Store2.DataBind();
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = year + "年" + month + "月核算科室还未设置",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO"),
                    Modal = true
                });
                Store1.DataSource = checkpersons.BuildCheckPersons();
                Store1.DataBind();
            }
        }

        /// <summary>
        /// 保存独立核算人员数
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
                    CheckPersons checkpersons = new CheckPersons();
                    checkpersons.SaveCheckPersons(selectRow, cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString(), user);
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "核算科人数设置成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO"),
                        Modal = true
                    });
                    GetPageData();
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
        protected void btn_Back_Click(object sender, AjaxEventArgs e)
        {
            Mask.Config msgconfig = new Mask.Config();
            msgconfig.Msg = "页面转向中...";
            msgconfig.MsgCls = "x-mask-loading";
            Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
            Goldnet.Ext.Web.Ext.Redirect("CheckPersonsList.aspx?pageid=" + Request.QueryString["pageid"].ToString() + "");
        }

        /// <summary>
        /// 选择科室进入人员明细编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Persons_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string year = selectRow[0]["YEARS"];
                string month = Request["CheckMonth"];
                string deptid = selectRow[0]["DEPTID"];
                string deptname = selectRow[0]["DEPTNAME"];
                Mask.Config msgconfig = new Mask.Config();
                msgconfig.Msg = "页面转向中...";
                msgconfig.MsgCls = "x-mask-loading";
                Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
                Goldnet.Ext.Web.Ext.Redirect("CheckPersonBonusEdit.aspx?CheckBonusYear=" + year + "&CheckBonusMonth=" + month + "&DeptID=" + deptid + "&DeptName=" + deptname  +"&pageid=" + Request.QueryString["pageid"].ToString() + "");
            }
        }

        /// <summary>
        /// 反序列化得到客户端提交的gridpanel数据行 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

    }
}
