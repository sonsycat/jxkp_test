using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP
{
    public partial class HospitalBonusLists : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string year = DateTime.Now.AddMonths(-1).Year.ToString();
                string month = DateTime.Now.AddMonths(-1).Month.ToString();
                BoundComm boundcomm = new BoundComm();
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.Value = year;
                ccbYearTo.Value = year;
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;
                ccbMonthTo.Value = month;
               

                data(DateTime.Now.AddMonths(-1).ToString("yyyyMM"), DateTime.Now.AddMonths(-1).ToString("yyyyMM"));
            }
        }
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            data(this.cbbYear.SelectedItem.Value.ToString() + this.cbbmonth.SelectedItem.Value.ToString(), this.ccbYearTo.SelectedItem.Value.ToString() + this.ccbMonthTo.SelectedItem.Value.ToString());
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            CalculateBonus calculateBouns = new CalculateBonus();

            DataTable table = calculateBouns.GetHospitalsysBonus(this.cbbYear.SelectedItem.Value.ToString() + this.cbbmonth.SelectedItem.Value.ToString(), this.ccbYearTo.SelectedItem.Value.ToString() + this.ccbMonthTo.SelectedItem.Value.ToString());
            DataRow dr = table.NewRow();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Columns[i].ColumnName.Equals("科室名称"))
                {
                    dr[table.Columns[i].ColumnName] = "合计";
                }
               
                else
                {
                    dr[table.Columns[i].ColumnName] = table.Compute(string.Format("Sum({0})", table.Columns[i].ColumnName), "");
                    
                }   
            }
            table.Rows.Add(dr);
            this.outexcel(table, "全院奖金分发表");
        }
        private void data(string starmonth, string endmonth)
        {
            CalculateBonus calculateBouns = new CalculateBonus();

            DataTable table = calculateBouns.GetHospitalsysBonus(starmonth, endmonth);
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

                if (cl.Header.Equals("招行账号") || cl.Header.Equals("姓名") || cl.Header.Equals("科室名称") || cl.Header.Equals("ID") || cl.Header.Equals("DEPT_CODE"))
                {
                    TextField fils = new TextField();
                    fils.ID = i.ToString();
                    fils.SelectOnFocus = true;
                    if (cl.Header.Equals("姓名") || cl.Header.Equals("科室名称"))
                    {
                        fils.ReadOnly = true;
                    }
                    if (cl.Header.Equals("ID") || cl.Header.Equals("DEPT_CODE"))
                    {
                        cl.Hidden = true;
                    }
                    if (cl.Header.Equals("科室名称"))
                    {
                        dr[cl.Header] = "合计";
                    }
                    cl.Editor.Add(fils);
                }
                else if (cl.Header.Equals("人员类别"))
                {
                    ComboBox fils = new ComboBox();
                    fils.ID = i.ToString();
                    DataTable tb = calculateBouns.GetBonusPersonType();
                    for (int j = 0; j < tb.Rows.Count; j++)
                    {
                        fils.Items.Insert(j, new Goldnet.Ext.Web.ListItem(tb.Rows[j]["BONUS_PSERSONS_TYPE"].ToString(), tb.Rows[j]["BONUS_PSERSONS_TYPE"].ToString()));
                    }
                    fils.ReadOnly = true;
                    cl.Editor.Add(fils);
                }
                else
                {
                    dr[cl.Header] = table.Compute(string.Format("Sum({0})", cl.Header), "");
                    NumberField fils = new NumberField();
                    fils.ID = i.ToString();
                    fils.SelectOnFocus = true;
                    fils.DecimalPrecision = 2;
                    cl.Align = Alignment.Right;
                    if (cl.Header.Equals("合计"))
                    {
                        fils.ReadOnly = true;
                    }
                    cl.Editor.Add(fils);
                }

                this.GridPanel2.ColumnModel.Columns.Add(cl);
            }
            table.Rows.Add(dr);
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }
    }
}
