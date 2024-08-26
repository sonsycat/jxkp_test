using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using GoldNet.Model;
using GoldNet.Comm.ExportData;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Sys_Menu_Detail_Search : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                BoundComm boundcomm = new BoundComm();
                Store3.DataSource = boundcomm.getYears();
                Store3.DataBind();
                cbbYear.SetValue(DateTime.Now.Year);
                Store4.DataSource = boundcomm.getMonth();
                Store4.DataBind();
                cbbmonth.SetValue(DateTime.Now.Month);
                data(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            }
        }
        private void data(string year, string month)
        {
            SE_ROLE deptpercent = new SE_ROLE();
            string deptcode = this.DeptFilter("");
            DataTable table = deptpercent.GetMenuDetailSearch(year, month, Request["appmenuid"].ToString(), deptcode);
            DataTable menuitem = deptpercent.GetSysMenuGuide(Request["appmenuid"].ToString());
            DataRow dr = table.NewRow();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                this.Store1.AddField(record);
                Column cl = new Column();
                cl.Header = table.Columns[i].ColumnName;
                cl.Sortable = false;
                cl.MenuDisabled = true;
                cl.DataIndex = table.Columns[i].ColumnName;
               
                if (cl.Header.Equals("DEPT_CODE") || cl.Header.Equals("科室名称"))
                {
                    if (cl.Header.Equals("DEPT_CODE"))
                    {
                        cl.Hidden = true;
                    }
                    if (cl.Header.Equals("科室名称"))
                    {
                        cl.Align = Alignment.Left;
                    }
                    this.GridPanel2.ColumnModel.Columns.Add(cl);
                }
                else
                {
                    for (int j = 0; j < menuitem.Rows.Count; j++)
                    {
                        if (table.Columns[i].ColumnName.Substring(1,cl.Header.Length-1).Equals(menuitem.Rows[j]["GUIDE_CODE"].ToString()))
                        {

                            if (menuitem.Rows[j]["ACCOUNT_FLAGS"].ToString().Equals("1"))
                            {
                                dr["科室名称"] = "合计";
                                dr[cl.Header] = table.Compute(string.Format("Sum({0})", cl.Header), "");
                               
                            }
                            if (menuitem.Rows[j]["DETAIL_FLAGS"].ToString().Equals("1"))
                            {
                                ImageCommand icomm = new ImageCommand();
                                icomm.CommandName = "Edit";
                                icomm.Text = "...";
                                icomm.ToolTip.Text = "关联指标";
                                cl.Commands.Add(icomm);
                            }
                            cl.Header = menuitem.Rows[j]["GUIDE_NAME"].ToString();
                            cl.ColumnID = menuitem.Rows[j]["GUIDE_CODE"].ToString();
                            cl.Align = Alignment.Right;

                            this.GridPanel2.ColumnModel.Columns.Add(cl);
                        }
                    }
                }

            }
            table.Rows.Add(dr);
            this.Store1.DataSource = table;
            this.Store1.DataBind();
            User user = (User)Session["CURRENTSTAFF"];
            Session[user.UserId + "menusearchdetail"] = table;
        }
        protected void Query(object sender, AjaxEventArgs e)
        {
            data(this.cbbYear.SelectedItem.Value.ToString(), this.cbbmonth.SelectedItem.Value.ToString());
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            SE_ROLE dal = new SE_ROLE();
            DataTable menuitem = dal.GetSysMenuGuide(Request["appmenuid"].ToString());
            User user = (User)Session["CURRENTSTAFF"];
            ExportData ex = new ExportData();
            DataTable table = (DataTable)Session[user.UserId + "menusearchdetail"];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                for (int j = 0; j < menuitem.Rows.Count; j++)
                {
                    if (table.Columns[i].ColumnName.Substring(1, table.Columns[i].ColumnName.Length - 1).Equals(menuitem.Rows[j]["GUIDE_CODE"].ToString()))
                        table.Columns[i].ColumnName = menuitem.Rows[j]["GUIDE_NAME"].ToString();
                }
            }
                //this.outexcel(table,"指标数据");
                ex.ExportToLocal(table, this.Page, "xls", "指标数据");
        }
       
        [AjaxMethod]
        public void GetInfo(string command, string colIndex, string deptcode)
        {
            if (deptcode != "")
            {
                string years = this.cbbYear.SelectedItem.Value.ToString();
                string months = this.cbbmonth.SelectedItem.Value.ToString();
                if (months.Length == 1)
                    months = "0" + months;
                LoadConfig loadcfg = getLoadConfig("Sys_Menu_Guide_Detail.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("appid", Request["appmenuid"].ToString()));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("guidecode", colIndex.Substring(1, colIndex.Length - 1)));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("tjyf", years + months));
                showCenterSet(this.GuideDetail, loadcfg);
            }
        }
    }
}
