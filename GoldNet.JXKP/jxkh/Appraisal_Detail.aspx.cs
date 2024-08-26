using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.jxkh
{
    public partial class Appraisal_Detail : PageBase
    {
        Goldnet.Dal.Appraisal dal = new Goldnet.Dal.Appraisal();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //Session.Add("APPRAISALDETAIL", appraisalid + "," + appraisalname + "," + startdate + "," + enddate + "," + archflg + "," + organ);
                if (Session["APPRAISALDETAIL"] == null)
                {
                    Response.End();
                    return;
                }
                Dictionary<string, string>[] selectedRow = (Dictionary<string, string>[])Session["APPRAISALDETAIL"];
                string startdate = selectedRow[0]["START_DATE"];
                string enddate = selectedRow[0]["END_DATE"];
                string archflg = selectedRow[0]["ARCHIVE_TAGS"];
                string desc = selectedRow[0]["EVALUATE_DESCRIPTION"];
                if (selectedRow[0]["IS_EVLUATE_BONUS"] != null && selectedRow[0]["IS_EVLUATE_BONUS"] != "")
                {
                    this.IS_EVLUATE_BONUS.SelectedItem.Value = selectedRow[0]["IS_EVLUATE_BONUS"];
                    this.Hide_Flag.Value = selectedRow[0]["IS_EVLUATE_BONUS"];
                }
                else
                {
                    this.IS_EVLUATE_BONUS.SelectedItem.Value = "否";
                }
                bool isEdit = Convert.ToBoolean(Request.QueryString["isEdit"].ToString());
                if (isEdit)
                {
                    this.IS_EVLUATE_BONUS.Disabled = false;
                    this.Btn_Arch.Disabled = false;
                    this.Btn_Del.Disabled = false;
                }
                else
                {
                    this.IS_EVLUATE_BONUS.Disabled = true;
                    this.Btn_Arch.Disabled = true;
                    this.Btn_Del.Disabled = true;
                }
                SetInitState(startdate, enddate, archflg, desc);
            }
        }

        //页面控件状态初始化
        protected void SetInitState(string startdate, string enddate, string archflg, string desc)
        {
            this.Lbl_AppMonth.Text = "评价区间  (从" + startdate + " 至 " + enddate + ") ";
            this.TxtMemo.Value = desc;
            if (archflg.Equals("1"))
            {
                this.Btn_Arch.Disabled = true;
            }
        }

        //删除按钮
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            if (Session["APPRAISALDETAIL"] == null)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config { Title = SystemMsg.msgtitle4, Message = "会话超时，请重新登录系统", Buttons = MessageBox.Button.OK, Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO") });
                return;
            }
            Dictionary<string, string>[] selectedRow = (Dictionary<string, string>[])Session["APPRAISALDETAIL"];
            string appraisalid = selectedRow[0]["EVALUATE_CODE"];
            dal.DelApprailsaList(appraisalid);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.Ext.Msg.show({ title: '系统提示', msg:  '评价已删除!',icon: 'ext-mb-info',  buttons: { ok: true }  });");
            scManager.AddScript("parent.Store1.reload();");
            scManager.AddScript("parent.DetailWin.hide();");
        }

        //归档按钮
        protected void Btn_Arch_Click(object sender, AjaxEventArgs e)
        {
            if (Session["APPRAISALDETAIL"] == null)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config { Title = SystemMsg.msgtitle4, Message = "会话超时，请重新登录系统", Buttons = MessageBox.Button.OK, Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO") });
                return;
            }
            Dictionary<string, string>[] selectedRow = (Dictionary<string, string>[])Session["APPRAISALDETAIL"];
            string appraisalid = selectedRow[0]["EVALUATE_CODE"];
            string desc = this.TxtMemo.Value.ToString().Trim().Replace("'", "''");
            this.Hide_Flag.Value = this.IS_EVLUATE_BONUS.SelectedItem.Value.ToString();
            dal.UpdateApprailsalist(appraisalid, desc, this.IS_EVLUATE_BONUS.SelectedItem.Value.ToString());
            selectedRow[0]["EVALUATE_DESCRIPTION"] = this.TxtMemo.Value.ToString();
            selectedRow[0]["ARCHIVE_TAGS"] = "1";
            Session["APPRAISALDETAIL"] = selectedRow;
            this.Btn_Arch.Disabled = true;
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config { Title = SystemMsg.msgtitle4, Message = "评价已归档！", Buttons = MessageBox.Button.OK, Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO") });
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.Store1.reload();");
        }

        //数据查询绑定
        protected void GridDataBind(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectedRow = (Dictionary<string, string>[])Session["APPRAISALDETAIL"];
            string startdate = selectedRow[0]["START_DATE"];
            string appraisalid = selectedRow[0]["EVALUATE_CODE"];
            string organ = selectedRow[0]["ORG_TYPE"];
            for (int i = 0; i < GridPanel_Show.ColumnModel.Columns.Count; i++)
            {
                GridPanel_Show.RemoveColumn(i);
            }
            this.Store1.RemoveFields();

            string deptFilter = Request.QueryString["deptFilter"].ToString();

            DataTable table = dal.GetEvaluateDetail(appraisalid, deptFilter).Tables[0];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField field = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                //由数值列开始使用Float类型
                if (i > 4)
                {
                    field.Type = RecordFieldType.Float;
                }
                this.Store1.AddField(field, i);
                if (!(table.Columns[i].ColumnName.Equals("EVALUATE_DEPT_CODE") || table.Columns[i].ColumnName.Equals("STAFF_ID") || table.Columns[i].ColumnName.Equals("ORG_TYPE") || ((!organ.Equals("R")) && (table.Columns[i].ColumnName.Equals("姓名")))))
                {
                    Column col = new Column();
                    col.Header = table.Columns[i].Caption.Replace("（科）", "").Replace("(科)", "").Replace("（科)", "").Replace("(科）", "").Replace("（人）", "").Replace("(人)", "").Replace("（人)", "").Replace("(人）", "");
                    col.Width = col.Header.Length * 18 + 20;
                    col.Sortable = true;
                    col.DataIndex = table.Columns[i].ColumnName;
                    if (col.Header.Equals("科室名称") || col.Header.Equals("姓名"))
                    {
                        col.Align = Alignment.Center;
                    }
                    else
                    {
                        col.Align = Alignment.Right;
                        col.Renderer.Fn = "rmbMoney";
                    }
                    col.MenuDisabled = true;
                    this.GridPanel_Show.AddColumn(col);
                }

            }
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            Dictionary<string, string>[] selectedRow = (Dictionary<string, string>[])Session["APPRAISALDETAIL"];
            string filename = selectedRow[0]["EVALUATE_NAME"];
            string appraisalid = selectedRow[0]["EVALUATE_CODE"];

            string deptFilter = Request.QueryString["deptFilter"].ToString();
            DataTable dt = dal.GetEvaluateDetail(appraisalid, deptFilter).Tables[0];
            ExportData ex = new ExportData();
            ex.ExportToLocal(dt, this.Page, "xls", filename);
        }

    }
}
