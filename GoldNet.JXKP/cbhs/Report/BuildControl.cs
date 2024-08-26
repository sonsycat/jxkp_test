using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.Comm;


namespace GoldNet.JXKP.cbhs.Report
{
    public class BuildControl
    {
        public void AddRecord(string recordfieldname, Store store)
        {
            RecordField recordfield = new RecordField();
            recordfield.Name = recordfieldname;
            store.AddField(recordfield);
        }
        public void AddDColumn(string fieldName, string headName, ExtGridPanel gridpanel, Store store, bool hide)
        {
            AddDColumn(fieldName, headName, gridpanel, store, hide, false, true);
        }
        public void AddDColumn(string fieldName, string headName, ExtGridPanel gridpanel, Store store, bool hide, bool edits)
        {
            AddDColumn(fieldName, headName, gridpanel, store, hide, false, edits);
        }
        public void AddDColumn(string fieldName, string headName, ExtGridPanel gridpanel, Store store, bool hide, bool fix, bool edits)
        {
            ExtColumn bonusColumn = new ExtColumn();
            bonusColumn.ColumnID = fieldName;
            bonusColumn.Header = headName;
            bonusColumn.DataIndex = fieldName;
            bonusColumn.MenuDisabled = true;
            bonusColumn.Hidden = hide;
            bonusColumn.Locked = fix;
            bonusColumn.Width = 200;
            if (edits)
            {
                TextField textfield = new TextField();
                textfield.ID = "txt" + fieldName;
                bonusColumn.Editor.Add(textfield);
            }
            gridpanel.ColumnModel.Columns.Add(bonusColumn);
            
            RecordField recordfield = new RecordField();
            recordfield.Name = fieldName;
            store.AddField(recordfield);
        }
        public void AddGridPanel(DataTable table, string fieldName, string headName, ExtGridPanel gridpanel, Store store)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                ExtColumn bonusColumn = new ExtColumn();
                bonusColumn.ColumnID = table.Rows[i][fieldName].ToString();
                bonusColumn.Header = table.Rows[i][headName].ToString();
                bonusColumn.DataIndex = table.Rows[i][fieldName].ToString();
                bonusColumn.MenuDisabled = true;
                bonusColumn.Width = 200;
                gridpanel.ColumnModel.Columns.Add(bonusColumn);

                RecordField recordfield = new RecordField();
                recordfield.Name = table.Rows[i][fieldName].ToString();
                store.AddField(recordfield);
            }
        }
        /// <summary>
        /// 收入的8个报表专用生成列和记录
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fieldName"></param>
        /// <param name="headName"></param>
        /// <param name="gridpanel"></param>
        /// <param name="store"></param>
        public void AddIncomeGridPanel(DataTable table, ExtGridPanel gridpanel, Store store)
        {
            gridpanel.Reconfigure();
            gridpanel.ColumnModel.Columns.Clear();
            store.RemoveFields();
            //store.Reader.Clear();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField recordfield = new RecordField();
                recordfield.Name = table.Columns[i].ColumnName.ToString();
                store.AddField(recordfield);
                if (table.Columns[i].ColumnName.ToString() != "DEPT_CODE" && table.Columns[i].ColumnName.ToString() != "ACCOUNT_DEPT_CODE")
                {
                    ExtColumn ec = new ExtColumn();
                    ec.ColumnID = table.Columns[i].ColumnName;
                    ec.Header = "<div style='text-align:center;'>"+table.Columns[i].ColumnName+"</div>";
                    ec.DataIndex = table.Columns[i].ColumnName;
                    ec.MenuDisabled = true;
                  
                    ec.Width = 100;
                    if (table.Columns[i].ColumnName.ToString() == "科室名称")
                    {
                        ec.Locked = true;
                        ec.Align = Alignment.Center;
                    }
                    else
                    {
                        ec.Renderer.Fn = "rmbMoney";
                        ec.Align = Alignment.Right;
                    }
                    gridpanel.AddColumn(ec);
                    gridpanel.ColumnModel.Columns.Add(ec);
                }
               
            }
        }
        public DataTable DataBind(DataTable table, ExtGridPanel GridPanel_Show, Store SReport)
        {
            GridPanel_Show.AutoWidth = true;
            GridPanel_Show.Reconfigure();
            GridPanel_Show.ColumnModel.Columns.Clear();
            GridPanel_Show.ColumnModel.Controls.Clear();
            DataTable dt = new DataTable();
            string stra = table.Rows[0]["TITLE"].ToString();
            stra = stra.TrimStart(',');
            dt.Columns.Add("DEPT_CODE");
            dt.Columns.Add("DEPT_NAME");
            dt.Columns.Add("合计");
            string[] colname = stra.Split(',');
            for (int i = 0; i < colname.Length; i++)
            {
                dt.Columns.Add(colname[i]);
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {//table.Rows[i]["DEPT_CODE"].ToString() + "," +
                string stra0 = table.Rows[i]["DEPT_CODE"].ToString() + "," + table.Rows[i]["DEPT_NAME"].ToString() + ",合计" + table.Rows[i]["VALUESTR"].ToString();
                string[] row0 = stra0.Split(',');
                dt.Rows.Add(row0);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double total = 0;
                DataRow dr = dt.Rows[i];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName != "DEPT_CODE" && dt.Columns[j].ColumnName != "DEPT_NAME" && dt.Columns[j].ColumnName != "合计" && dr[dt.Columns[j].ColumnName] != null && dr[dt.Columns[j].ColumnName].ToString() != "")
                    {
                        total += Convert.ToDouble(dr[dt.Columns[j].ColumnName]);
                    }
                    dr["合计"] = total;
                }
            }

            DataRow drTotal = dt.NewRow();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string[] str = dt.Columns[i].ColumnName.ToString().Split('，');
                string headcol = "";
                for (int j = 0; j < str.Length; j++)
                {
                    headcol += str[j].ToString();
                }
                dt.Columns[i].ColumnName = headcol;
                //
                double total = 0;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    DataRow dr = dt.Rows[j];
                    if (dt.Columns[i].ColumnName != "DEPT_CODE" && dt.Columns[i].ColumnName != "DEPT_NAME" && dr[dt.Columns[i].ColumnName] != null && dr[dt.Columns[i].ColumnName].ToString() != "")
                    {
                        total += Convert.ToDouble(dr[dt.Columns[i].ColumnName]);
                    }
                }
                drTotal[dt.Columns[i].ColumnName] = total;
                if (dt.Columns[i].ColumnName == "DEPT_NAME")
                {
                    drTotal[dt.Columns[i].ColumnName] = "合计";
                }

            }
            dt.Rows.Add(drTotal);
            //构建store组成字段的前三列

            JsonReader jr = new JsonReader();
            jr.ReaderID = "DEPT_CODE";
            //
            SReport.RemoveFields();
            RecordField field0 = new RecordField("DEPT_CODE");
            SReport.AddField(field0);

            //jr.Fields.Add(field0);

            RecordField field1 = new RecordField("DEPT_NAME");
            SReport.AddField(field1);
            //jr.Fields.Add(field1);
            RecordField field2 = new RecordField("合计");
            SReport.AddField(field2);
            //jr.Fields.Add(field2);
           

            //构建多表头表格的前两列
            ExtColumn col1 = new ExtColumn();
            col1.ColumnID = "DEPT_CODE";
            col1.DataIndex = "DEPT_CODE";
            col1.Width = Unit.Pixel(120);
            col1.MenuDisabled = true;
            col1.Sortable = false;
            col1.Hidden = true;
            col1.Align = Alignment.Right;
            col1.Header = "<center>科室代码</center>";
            GridPanel_Show.ExtColumnModel.Columns.Add(col1);
            GridPanel_Show.AddColumn(col1);
            ExtColumn col0 = new ExtColumn();
            col0.ColumnID = "DEPT_NAME";
            col0.DataIndex = "DEPT_NAME";
            col0.Width = Unit.Pixel(120);
            col0.MenuDisabled = true;
            col0.Align = Alignment.Right;
            col0.Locked = true;
            col0.Header = "<center>科室</center>";
            GridPanel_Show.ExtColumnModel.Columns.Add(col0);
            GridPanel_Show.AddColumn(col0);

            ExtColumn col2 = new ExtColumn();
            col2.ColumnID = "合计";
            col2.DataIndex = "合计";
            col2.Width = Unit.Pixel(120);
            col2.MenuDisabled = true;
            col2.Align = Alignment.Right;
            col2.Header = "<center>合计</center>";
            col2.Renderer.Fn = "rmbMoney";
            GridPanel_Show.ExtColumnModel.Columns.Add(col2);
            GridPanel_Show.AddColumn(col2);

            ExtColumn color = new ExtColumn();
            //添加剩余的store字段和多表头的剩余列
            for (int i = 0; i < colname.Length; i++)
            {
                RecordField field = new RecordField(colname[i]);
                SReport.AddField(field);
                //jr.Fields.Add(field);
                if (colname[i] == "*")
                {
                    color.ColumnID = colname[i];
                    color.DataIndex = colname[i];
                    color.Width = Unit.Pixel(120);
                    color.MenuDisabled = true;
                    color.Locked = false;
                    color.Sortable = false;
                    color.Align = Alignment.Right;
                    color.Header = "<center>" + colname[i] + "</center>";
                    color.Renderer.Fn = "rmbMoney";
                    continue;
                }
                ExtColumn col = new ExtColumn();
                col.ColumnID = colname[i];
                col.DataIndex = colname[i];
                col.Width = Unit.Pixel(120);
                col.MenuDisabled = true;
                col.Locked = false;
                col.Sortable = false;
                col.Align = Alignment.Right;
                
                col.Header = "<center>" + colname[i].ToString() + "</center>";
                col.Renderer.Fn = "rmbMoney";
                GridPanel_Show.ExtColumnModel.Columns.Add(col);
                GridPanel_Show.AddColumn(col);
            }
            GridPanel_Show.ExtColumnModel.Columns.Add(color);
            GridPanel_Show.AddColumn(color);
            //store数据绑下显示
            //SReport.Reader.Add(jr);
            SReport.DataSource = dt;
            SReport.DataBind();

            return dt;
        }
    }
}
