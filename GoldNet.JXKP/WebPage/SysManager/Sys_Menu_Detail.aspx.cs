using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Model;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Sys_Menu_Detail : PageBase
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
                BoundComm boundcomm = new BoundComm();
                Store3.DataSource = boundcomm.getYears();
                Store3.DataBind();
                cbbYear.SetValue(DateTime.Now.Year);
                Store4.DataSource = boundcomm.getMonth();
                Store4.DataBind();
                cbbmonth.SetValue(DateTime.Now.Month);
                data(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
                this.bsave.Visible = this.IsEdit();
            }
        }

        /// <summary>
        /// 数据提取
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        private void data(string year, string month)
        {
            SE_ROLE deptpercent = new SE_ROLE();
            string deptcode = this.DeptFilter("");
            DataTable table = deptpercent.GetMenuDetail(year, month, Request["appmenuid"].ToString(), deptcode);
            DataTable menuitem = deptpercent.GetSysMenu(Request["appmenuid"].ToString(), "");
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

                if (cl.Header.Equals("DEPT_CODE") || cl.Header.Equals("科室名称") )
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
                else if (cl.Header.Equals("备注"))
                {
                    TextField fils = new TextField();
                    fils.ID = "GUIDE_MEMO";
                    fils.SelectOnFocus = true;
                    cl.Editor.Add(fils);
                    cl.Align = Alignment.Left;
                    cl.Width = Unit.Point(200);

                    this.GridPanel2.ColumnModel.Columns.Add(cl);
                }
                else
                {
                    for (int j = 0; j < menuitem.Rows.Count; j++)
                    {
                        if (table.Columns[i].ColumnName.Equals(menuitem.Rows[j]["MENU_GUIDE_NAME"].ToString()))
                        {
                            if (menuitem.Rows[j]["FIELD_TYPE"].ToString().Equals("0"))
                            {
                                if (menuitem.Rows[j]["ACCOUNT_FLAGS"].ToString().Equals("1"))
                                {
                                    dr["科室名称"] = "合计";
                                    dr[cl.Header] = table.Compute(string.Format("Sum({0})", cl.Header), "");

                                }
                                NumberField fils = new NumberField();
                                fils.ID = i.ToString();
                                fils.SelectOnFocus = true;
                                fils.DecimalPrecision = 2;
                                cl.Editor.Add(fils);
                                cl.Align = Alignment.Right;
                                cl.Width = Unit.Point(int.Parse(menuitem.Rows[j]["SHOW_WIDTH"].ToString()));

                                this.GridPanel2.ColumnModel.Columns.Add(cl);
                            }
                            else
                            {
                                DataTable tb = deptpercent.GetMenuItem(menuitem.Rows[j]["APP_MENU_ID"].ToString(), menuitem.Rows[j]["MENU_GUIDE_ID"].ToString(), "", "");
                                ComboBox com = new ComboBox();
                                com.ID = i.ToString();
                                com.SelectOnFocus = true;
                                for (int k = 0; k < tb.Rows.Count; k++)
                                {
                                    com.Items.Add(new Goldnet.Ext.Web.ListItem(tb.Rows[k]["ITEM_NAME"].ToString(), tb.Rows[k]["ITEM_NAME"].ToString()));
                                }
                                com.ReadOnly = true;
                                cl.Editor.Add(com);
                                cl.Align = Alignment.Center;
                                cl.Width = Unit.Point(int.Parse(menuitem.Rows[j]["SHOW_WIDTH"].ToString()));
                                this.GridPanel2.ColumnModel.Columns.Add(cl);
                                for (int m = 0; m < table.Rows.Count; m++)
                                {
                                    if (tb.Rows.Count > 0)
                                    {
                                        bool flags = false;
                                        for (int h = 0; h < tb.Rows.Count; h++)
                                        {
                                            if (table.Rows[m][i].ToString().Equals(tb.Rows[h]["ITEM_ID"].ToString()))
                                            {
                                                table.Rows[m][i] = tb.Rows[h]["ITEM_NAME"].ToString();
                                                flags = true;
                                            }

                                        }
                                        if (flags == false)
                                            table.Rows[m][i] = tb.Rows[0]["ITEM_NAME"].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            table.Rows.Add(dr);
            this.Store1.DataSource = table;
            this.Store1.DataBind();

            User user = (User)Session["CURRENTSTAFF"];
            Session[user.UserId + "menudetail"] = table;
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Query(object sender, AjaxEventArgs e)
        {
            data(this.cbbYear.SelectedItem.Value.ToString(), this.cbbmonth.SelectedItem.Value.ToString());
        }

        /// <summary>
        /// 数据保存处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save(object sender, AjaxEventArgs e)
        {
            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中            
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                SE_ROLE deptpercent = new SE_ROLE();
                if (deptpercent.SaveMenuDetail(selectRow, cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString(), Request["appmenuid"].ToString()))
                {
                    data(this.cbbYear.SelectedItem.Value.ToString(), this.cbbmonth.SelectedItem.Value.ToString());
                    this.ShowMessage("系统提示", "保存成功！");
                }
                else
                {
                    ShowDataError("", Request.Url.LocalPath, "Save");
                }
            }
        }

        /// <summary>
        /// 导出EXCEL处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            ExportData ex = new ExportData();
            DataTable table = (DataTable)Session[user.UserId + "menudetail"];

            ex.ExportToLocal(table, this.Page, "xls", "录入数据");
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
