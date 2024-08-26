using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP
{
    public partial class CheckPersonsLook : PageBase
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
                if (Request["CheckYear"] != null && Request["CheckMonth"] != null)
                {
                    string year = Request["CheckYear"];
                    string month = Request["CheckMonth"];
                    string pageid = Request["pageid"];
                    BoundComm boundcomm = new BoundComm();
                    CheckPersons checkpersons = new CheckPersons();
                    Store3.DataSource = boundcomm.getYears();
                    Store3.DataBind();
                    cbbYear.SetValue(year);
                    Store4.DataSource = boundcomm.getMonth();
                    Store4.DataBind();
                    cbbmonth.SetValue(month);
                    Store1.DataSource = checkpersons.GetCheckPersons(year, month,this.DeptFilter("",pageid));
                    Store1.DataBind();
                    Store2.DataSource = checkpersons.GetCheckDept(year, month, true);
                    Store2.DataBind();
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
        /// 
        /// </summary>
        private void GetPageData()
        {
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            string pageid = Request["pageid"];
            CheckPersons checkpersons = new CheckPersons();
            Store1.DataSource = checkpersons.GetCheckPersons(year, month,this.DeptFilter("",pageid));
            Store1.DataBind();
            Store2.DataSource = checkpersons.GetCheckDept(year, month, true);
            Store2.DataBind();
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
                string year = selectRow[0]["YEARS"];
                string month = selectRow[0]["MONTHS"];
                string deptid = selectRow[0]["DEPTID"];
                string deptname = selectRow[0]["DEPTNAME"];
                Mask.Config msgconfig = new Mask.Config();
                msgconfig.Msg = "页面转向中...";
                msgconfig.MsgCls = "x-mask-loading";
                Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
                Goldnet.Ext.Web.Ext.Redirect("CheckPersonBonusLook.aspx?CheckBonusYear=" + year + "&CheckBonusMonth=" + month + "&DeptID=" + deptid + "&DeptName=" + deptname + "&pageid=" + Request.QueryString["pageid"].ToString() + "");
            }
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
