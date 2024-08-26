using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm;
using System.Collections;
using System.Text;

namespace GoldNet.JXKP
{
    public partial class Income_Difference : PageBase
    {
        static DataTable difDt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                for (int i = 0; i < 10; i++)
                {
                    int year = System.DateTime.Now.Year - i;
                    this.years.Items.Add(new Goldnet.Ext.Web.ListItem(year.ToString(), year.ToString()));
                }
                this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
                this.months.SelectedItem.Value = System.DateTime.Now.ToString("MM");
                LoadData();
            }
        }
        private void LoadData()
        {

            difDt = new DataTable();
            DataColumn dc = new DataColumn();
            dc.ColumnName = "ID";
            difDt.Columns.Add(dc);
            DataColumn dc1 = new DataColumn();
            dc1.ColumnName = "DIFFTYPE";
            difDt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn();
            dc2.ColumnName = "DIFFTYPETABLENAME";
            difDt.Columns.Add(dc2);

            string tableName = System.Configuration.ConfigurationManager.AppSettings["CBHSUpdateData"];
            if (tableName != "")
            {
                string[] name = tableName.Split(';');
                if (name.Length > 0)
                {
                    for (int i = 0; i < name.Length; i++)
                    {
                        if (name[i] != "")
                        {
                            string[] table = name[i].Split(',');
                            if (table.Length > 0)
                            {
                                string aa = "";
                                for (int j = 1; j < table.Length; j++)
                                {
                                    DataRow dr = difDt.NewRow();
                                    dr["ID"] = i;
                                    dr["DIFFTYPE"] = table[0];
                                    aa = table[j];
                                    dr["DIFFTYPETABLENAME"] = aa;
                                    difDt.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
            }
            Store1.DataSource = difDt;
            Store1.DataBind();
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        public void Btn_Next_Click(object sender, AjaxEventArgs e)
        {
            Income_Difference_Dal dal = new Income_Difference_Dal();
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            //对比表类别标识
            string sourcetype = "";
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            if (selectRow == null || selectRow.Length < 1)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "请至少选择一条记录",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            else
            {

                StringBuilder str = new StringBuilder();
                for (int i = 0; i < selectRow.Length; i++)
                {
                    str.Append(selectRow[i]["ID"]);
                    if (i < selectRow.Length - 1)
                    {
                        str.Append(",");
                    }
                }
                sourcetype = str.ToString();
            }

            try
            {
                string rtmsg = dal.Exec_Sp_Incomesource_Minus(date_time, sourcetype);
                if (rtmsg != "")
                {
                    this.ShowDataError(rtmsg, Request.Path, "Btn_Next_Click");
                }
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Btn_Next_Click");
            }


            //临时表查询结果数据集
            DataTable dt = new DataTable();
            string table_name = "";

            //差异数据大于50行的表名
            StringBuilder tableNames = new StringBuilder();
            //差异数据在1-50行间的
            StringBuilder strTemp = new StringBuilder();
            for (int i = 0; i < selectRow.Length; i++)
            {
                table_name = selectRow[i]["DIFFTYPETABLENAME"].ToString();
                dt = dal.GetTableData(table_name);
                if (dt.Rows.Count > 50)
                {
                    if (tableNames.ToString().Length > 0)
                    {
                        tableNames.Append(",");
                    }
                    tableNames.Append(table_name);
                }
                else if (dt.Rows.Count > 0)
                {
                    if (strTemp.ToString().Length > 0)
                    {
                        strTemp.Append(",");
                    }
                    strTemp.Append(table_name);
                }
            }

            if (tableNames.ToString().Length > 0)
            {
                if (strTemp.ToString().Length > 0)
                {
                    tableNames.Append("," + strTemp.ToString());
                }
                string rtmsg = dal.Exec_Extrac_Income_Data(date_time, tableNames.ToString());
                if (rtmsg == null || rtmsg.Equals(""))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "差异过大，数据已从新提取",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                }
                else
                {

                    this.ShowDataError(rtmsg, Request.Path, "Btn_Next_Click");

                }
            }
            else
            {
                Session["comparemonth"] = date_time;
                Session["sourcetype"] = sourcetype;
                Session["DifferentSelect"] = selectRow;
                Mask.Config msgconfig = new Mask.Config();
                msgconfig.Msg = "页面转向中...";
                msgconfig.MsgCls = "x-mask-loading";
                Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
                Goldnet.Ext.Web.Ext.Redirect("Income_Difference_Data.aspx");
            }
        }

    }
}
