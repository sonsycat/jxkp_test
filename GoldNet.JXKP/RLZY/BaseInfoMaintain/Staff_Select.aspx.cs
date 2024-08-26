using System;
using System.Data;
using Goldnet.Ext.Web;
using System.Web.UI.WebControls;
using Goldnet.Dal;

namespace GoldNet.JXKP.RLZY
{
    public partial class Staff_Select : PageBase
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
                //this.GridPanel2.ColumnModel.RegisterCommandStyleRules();
                DataTable table = data("0");
                this.Store1.DataSource = table;
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            DataTable table = data(this.cbbType.SelectedItem.Value);
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 查询统计人员信息
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        private DataTable data(string types)
        {
            BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
            DataTable table = dal.Staffselect(types).Tables[0];
            DataRow dr = table.NewRow();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                this.Store1.AddField(record);
                Column cl = new Column();
                if (table.Columns[i].ColumnName == "SUMS")
                {
                    cl.Header = "合计";
                    dr["SUMS"] = table.Compute("Sum(SUMS)", "");
                }
                if (table.Columns[i].ColumnName == "ZG")
                {
                    cl.Header = "正高";
                    dr["ZG"] = table.Compute("Sum(ZG)", "");
                }
                if (table.Columns[i].ColumnName == "FG")
                {
                    cl.Header = "副高";
                    dr["FG"] = table.Compute("Sum(FG)", "");
                }
                if (table.Columns[i].ColumnName == "ZJ")
                {
                    cl.Header = "中级";
                    dr["ZJ"] = table.Compute("Sum(ZJ)", "");
                }
                if (table.Columns[i].ColumnName == "CJ")
                {
                    cl.Header = "初级";
                    dr["CJ"] = table.Compute("Sum(CJ)", "");
                }
                if (table.Columns[i].ColumnName == "QT")
                {
                    cl.Header = "其它";
                    dr["QT"] = table.Compute("Sum(QT)", "");
                }
                if (table.Columns[i].ColumnName == "TYPE_NAME")
                {
                    dr["TYPE_NAME"] = "合计";
                    cl.Header = "类别";
                }
                cl.Sortable = false;
                cl.MenuDisabled = true;
                cl.Align = Alignment.Right;
                cl.DataIndex = table.Columns[i].ColumnName;
                TextField fils = new TextField();
                fils.ReadOnly = true;
                fils.ID = i.ToString();
                fils.SelectOnFocus = false;
                //fils.DecimalPrecision = 2;
                cl.Editor.Add(fils);
                if (table.Columns[i].ColumnName == "TYPE_NAME")
                {
                    cl.Align = Alignment.Left;
                }
                this.GridPanel2.ColumnModel.Columns.Add(cl);
            }
            table.Rows.Add(dr);
            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            DataTable table = data(this.cbbType.SelectedItem.Value);
            TableCell[] header = new TableCell[7];
            header[0] = new TableHeaderCell();
            if (this.cbbType.SelectedItem.Value == "0")
            {
                header[0].Text = "年龄（岁）";
            }
            if (this.cbbType.SelectedItem.Value == "1")
            {
                header[0].Text = "干部类别";
            }
            if (this.cbbType.SelectedItem.Value == "2")
            {
                header[0].Text = "学历";
            }
            header[1] = new TableHeaderCell();
            header[1].Text = "合计";
            header[2] = new TableHeaderCell();
            header[2].Text = "正高";
            header[3] = new TableHeaderCell();
            header[3].Text = "副高";
            header[4] = new TableHeaderCell();
            header[4].Text = "中级";
            header[5] = new TableHeaderCell();
            header[5].Text = "初级";
            header[6] = new TableHeaderCell();
            header[6].Text = "其它";
            MHeaderTabletoExcel(table, header, this.cbbType.SelectedItem.Text, null, 0);

        }
    }
}
