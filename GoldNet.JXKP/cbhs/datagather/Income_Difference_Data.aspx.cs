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
    public partial class Income_Difference_Data : PageBase
    {
        private Income_Difference_Dal dal_income_difference = new Income_Difference_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetData();
            }
        }
        private void SetData()
        {

            if (Session["DifferentSelect"] != null && Session["DifferentSelect"].ToString() != "")
            {
                Dictionary<string, string>[] dt = (Dictionary<string, string>[])Session["DifferentSelect"];
                for (int i = 0; i < dt.Length; i++)
                {
                    NewStore(dt[i]["DIFFTYPETABLENAME"].ToString() + "");
                }
            }
        }
        private void NewStore(string tableName)
        {
            DataTable dt = dal_income_difference.GetTableColumn(tableName);
            Store store = new Store();
            store.ID = "s_" + tableName;
            JsonReader jr = new JsonReader();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    RecordField rf = new RecordField();
                    rf.Name = dt.Rows[i]["COLUMN_NAME"].ToString();
                    jr.Fields.Add(rf);

                }
            }
            RecordField rfRow = new RecordField();
            rfRow.Name = "ROW_ID";
            jr.Fields.Add(rfRow);
            store.Reader.Add(jr);

            NewGridPanel(store, dt, tableName);
        }
        private void NewGridPanel(Store store, DataTable dt, string tableName)
        {
            GridPanel gp = new GridPanel();
            gp.ID = "gp_" + tableName;
            gp.Title = tableName;
            gp.StoreID = store.ID;
            gp.TrackMouseOver = false;
            gp.Border = false;
            gp.Controls.Add(store);
            gp.Height = new Unit(300);
            gp.LoadMask.ShowMask = true;
            gp.LoadMask.Msg = "载入中...";
            PagingToolbar ptl = new PagingToolbar();
            ptl.ID = "ptl_" + tableName;
            ptl.PageSize = 200;
            ptl.AutoDataBind = true;
            gp.BottomBar.Add(ptl);
            CheckboxSelectionModel csm = new CheckboxSelectionModel();
            csm.ID = "csm_" + tableName;
            gp.SelectionModel.Add(csm);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Column cx = new Column();
                cx.ColumnID = dt.Rows[i]["COLUMN_NAME"].ToString();
                cx.Header = dt.Rows[i]["COLUMN_NAME"].ToString();
                cx.DataIndex = dt.Rows[i]["COLUMN_NAME"].ToString();
                cx.MenuDisabled = true;
                cx.Sortable = false;
                gp.ColumnModel.Columns.Add(cx);
            }

            Anchor anch = new Anchor(gp);
            anch.Horizontal = "98.5%";
            FormLayout1.Controls.Add(gp);
            FormLayout1.Anchors.Add(anch);

            Goldnet.Ext.Web.Parameter p = new Goldnet.Ext.Web.Parameter();
            p.Name = tableName;
            p.Value = "Ext.encode(#{gp_" + tableName + "}.getRowsValues())";
            p.Mode = ParameterMode.Raw;
            Btn_Update.AjaxEvents.Click.ExtraParams.Add(p);

            DataTable dtStore = dal_income_difference.GetTableData(tableName);
            store.DataSource = dtStore;
            store.DataBind();
        }
        public void Btn_Pre_Click(object sender, AjaxEventArgs e)
        {
            Mask.Config msgconfig = new Mask.Config();
            msgconfig.Msg = "页面转向中...";
            msgconfig.MsgCls = "x-mask-loading";
            Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
            Goldnet.Ext.Web.Ext.Redirect("Income_Difference.aspx");
        }
        public void Btn_Update_Click(object sender, AjaxEventArgs e)
        {
            if (Session["DifferentSelect"] != null && Session["DifferentSelect"].ToString() != "")
            {
                StringBuilder sb = new StringBuilder();
                Dictionary<string, string>[] dt = (Dictionary<string, string>[])Session["DifferentSelect"]; ;

                for (int i = 0; i < dt.Length; i++)
                {
                    string table_name = dt[i]["DIFFTYPETABLENAME"].ToString() + "_tmp";
                    Dictionary<string, string>[] selectRow = GetSelectRow(e, dt[i]["DIFFTYPETABLENAME"].ToString());
                    if (selectRow != null && selectRow.Length > 0)
                    {
                        
                        for (int j = 0; j < selectRow.Length; j++)
                        {
                            sb.Append(selectRow[j]["ROW_ID"].ToString()+",");
                        }
                    }
                }
                try
                {
                    string date_time = (string)Session["comparemonth"];
                    string sourcetype = (string)Session["sourcetype"];
                    string rtmsg = "";
                    rtmsg = dal_income_difference.Exec_Sp_Incomesource_Update(sourcetype, sb.ToString(), date_time);
                    if (rtmsg != "")
                    {
                        this.ShowDataError(rtmsg, Request.Path, "Btn_Update_Click");
                    }
                    else
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "信息提示",
                            Message = "更新成功",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        SetData();
                    }
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Btn_Update_Click");
                }
            }
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e, string valueName)
        {
            string row = e.ExtraParams[valueName].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
    }
}
