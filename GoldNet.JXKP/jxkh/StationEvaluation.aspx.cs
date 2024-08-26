using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Collections;
using System.Data;
using GoldNet.Model;
using System.Web.UI.WebControls;

namespace GoldNet.JXKP.jxkh
{
    public partial class StationEvaluation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    Response.End();
                    return;
                }
                string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
                string stationYear = "";
                string stationCode = "";
                if (Request["id"] != null)
                {
                    stationCode = Request["id"].ToString();
                }
                if (Request["sy"] != null)
                {
                    stationYear = Request["sy"].ToString();
                }
                if ((stationCode == "") || (stationYear == ""))
                {
                    Response.End();
                    return;
                }
                initGrid(stationCode,stationYear,incount);
            }
        }

        /// <summary>
        /// 数据绑定，显示
        /// </summary>
        /// <param name="stationCode"></param>
        /// <param name="stationYear"></param>
        /// <param name="incount"></param>
        protected void initGrid(string stationCode,string stationYear,string incount)
        {
            string rtn = "";
            Goldnet.Dal.Assess dal = new Goldnet.Dal.Assess();
            DataTable table = new DataTable();
            try 
            {
                table = dal.GetStationTest(stationYear, incount, stationCode).Tables[0];
            }
            catch (Exception ee)
            {
                rtn = "岗位评测失败，<BR/>原因：" + ee.Message;
            }
            if (!rtn.Equals(""))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle1,
                    Message = rtn,
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            if (table.Rows.Count.Equals(0))
            {
                return;
            }
            DataTable dt = new DataTable();
            string stra = table.Rows[0]["TITLE"].ToString();
            stra = stra.TrimStart(',').Replace("____", ",");
            dt.Columns.Add("人员");
            dt.Columns.Add("PERSONID");
            dt.Columns.Add("总分");
            string[] colname = stra.Split(',');
            for (int i = 0; i < colname.Length; i++)
            {
                dt.Columns.Add(colname[i]);
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string stra0 = table.Rows[i]["PERSON_NAME"].ToString() + "," + table.Rows[i]["PERSON_ID"].ToString() + "," + table.Rows[i]["SUMVALUE"].ToString() + table.Rows[i]["VALUESTR"].ToString();
                stra0 = stra0.Replace("____", ",");
                string[] row0 = stra0.Split(',');
                dt.Rows.Add(row0);
            }

            //构建store组成字段的前三列
            this.Store1.RemoveFields();
            RecordField field0 = new RecordField("人员", RecordFieldType.String);
            this.Store1.AddField(field0, 0);
            RecordField field1 = new RecordField("PERSONID", RecordFieldType.String);
            this.Store1.AddField(field1, 0);
            RecordField field2 = new RecordField("总分", RecordFieldType.String);
            this.Store1.AddField(field2, 0);

            this.GridPanel_Show.ExtColumnModel.Columns.RemoveAt(0);
            this.GridPanel_Show.ExtColumnModel.HeadRows.RemoveAt(0);
            //构建多表头表格的前两列
            ExtColumn col0 = new ExtColumn();
            col0.ColumnID = "人员";
            col0.DataIndex = "人员";
            col0.Width = Unit.Pixel(120);
            col0.MenuDisabled = true;
            col0.Align = Alignment.Right;
            col0.Locked = true;
            col0.Sortable = false;
            col0.Header = "<center>人员</center>";
            this.GridPanel_Show.ExtColumnModel.Columns.Add(col0);
            ExtColumn col1 = new ExtColumn();
            col1.ColumnID = "总分";
            col1.DataIndex = "总分";
            col1.Width = Unit.Pixel(120);
            col1.MenuDisabled = true;
            col1.Sortable = false;
            col1.Align = Alignment.Right;
            col1.Header = "<center>总分</center>";
            this.GridPanel_Show.ExtColumnModel.Columns.Add(col1);

            //添加剩余的store字段和多表头的剩余列
            for (int i = 0; i < colname.Length; i++)
            {
                RecordField field = new RecordField(colname[i], RecordFieldType.String);
                this.Store1.AddField(field, i+3);
                ExtColumn col = new ExtColumn();
                col.ColumnID = colname[i];
                col.DataIndex =colname[i];
                col.Width = Unit.Pixel(120);
                col.MenuDisabled = true;
                col.Sortable = false;
                col.Align = Alignment.Right;
                col.Header ="<center>"+( (i % 2).Equals(0) ?"实际值":"目标值") +"</center>";
                this.GridPanel_Show.ExtColumnModel.Columns.Add( col);

            }
            //多表头表格的前两列header 空白
            ExtRows headrows = new ExtRows();
                ExtRow headrow0 = new ExtRow();
                headrow0.Header ="";
                headrow0.ColSpan = 1;
                headrow0.Align = Alignment.Center;
                headrows.Rows.Add(headrow0);
                ExtRow headrow1 = new ExtRow();
                headrow1.Header ="";
                headrow1.ColSpan = 1;
                headrow1.Align = Alignment.Center;
                headrows.Rows.Add(headrow1);

            for (int i=0; i<colname.Length; i=i+2)
            {
                ExtRow headrow = new ExtRow();
                headrow.Header =colname[i].Replace("实际值","");
                headrow.ColSpan = 2;
                headrow.Align = Alignment.Center;
                headrows.Rows.Add(headrow);
            }
            this.GridPanel_Show.ExtColumnModel.HeadRows.Add(headrows);
            //store数据绑下显示
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }


    }
}
