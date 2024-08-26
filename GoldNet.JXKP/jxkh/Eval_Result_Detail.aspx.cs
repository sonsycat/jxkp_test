using System;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.jxkh
{
    public partial class Eval_Result_Detail : System.Web.UI.Page
    {
        Goldnet.Dal.Appraisal dal = new Goldnet.Dal.Appraisal();
        string organ = "K";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (this.Request["organ1"] != null)
            {
                organ = Request.QueryString["organ1"].ToString().Equals("R") ? "R" : "K";
            }
            if (!Ext.IsAjaxRequest)
            {
                if (((Session["EvalDeptTable"] == null) && (organ.Equals("K"))) || ((Session["EvalPersonTable"] == null) && (organ.Equals("R"))))
                {
                    Response.End();
                    return;
                }
                SetInitState();
                //if (!organ.Equals("K"))
                //{
                //    this.Hide_Flag.Value = "1";
                //}
            }
        }

        //页面控件状态初始化
        protected void SetInitState()
        {
            string strwhere = organ.Equals("K") ? " WHERE STATION_TYPE='2' " : " WHERE STATION_TYPE='4' ";
            DataTable dt = dal.GetEvalutetype(strwhere).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.Comb_EvalType.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["EVALUATE_CLASS_NAME"].ToString(), dt.Rows[i]["EVALUATE_CLASS_CODE"].ToString()));
            }
            //dt = dal.GetEvaluteBonustype().Tables[0];
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    this.Comb_BonusClass.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["EVALUATE_CLASS_NAME"].ToString(), dt.Rows[i]["EVALUATE_CLASS_CODE"].ToString()));                
            //}

        }

        //保存按钮
        protected void Btn_Save_Click(object sender, AjaxEventArgs e)
        {
            string settingstr = organ.Equals("K") ? Session["EvalDeptSetting"].ToString() : Session["EvalPersonSetting"].ToString();
            string startdate = settingstr.Split(',')[0];
            string enddate = settingstr.Split(',')[1];
            string evalapp = settingstr.Split(',')[2];
            string incount = settingstr.Split(',')[3];
            string unitype = organ;
            DataTable dt = organ.Equals("K") ? (DataTable)Session["EvalDeptTable"] : (DataTable)Session["EvalPersonTable"];

            string evaname = this.Txt_EvalName.Value.ToString().Trim();
            string evatype = this.Comb_EvalType.SelectedItem.Value.ToString().Trim();
            string evamemo = this.TxtMemo.Value.ToString();
            string bonusclass = "";
            //if (this.Hide_Flag.Value.Equals("0"))
            //{
            //    bonusclass = this.Comb_BonusClass.SelectedItem.Value.ToString().Trim();
            //}

            if (((Session["EvalDeptTable"] == null) && (organ.Equals("K"))) || ((Session["EvalPersonTable"] == null) && (organ.Equals("R"))))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config { Title = SystemMsg.msgtitle4, Message = "会话超时，请重新登录系统！", Buttons = MessageBox.Button.OK, Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO") });
                return;
            }
            if (evaname.Equals("") || evatype.Equals(""))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config { Title = SystemMsg.msgtitle4, Message = "请输入评价名称并选择评价类型！", Buttons = MessageBox.Button.OK, Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO") });
                return;
            }

            string rtn = dal.SaveEvaluate(dt, startdate, enddate, incount, evalapp, evaname, evatype, evamemo, organ, bonusclass);
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config { Title = SystemMsg.msgtitle4, Message = rtn.Equals("") ? "评价保存成功！" : rtn, Buttons = MessageBox.Button.OK, Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO") });
            if (rtn.Equals(""))
            {
                this.Btn_Save.Disabled = true;
                if (organ.Equals("K"))
                {
                    Session.Remove("EvalDeptTable");
                    Session.Remove("EvalDeptSetting");
                }
                else
                {
                    Session.Remove("EvalPersonTable");
                    Session.Remove("EvalPersonSetting");
                }
            }
        }

        //数据查询绑定
        protected void GridDataBind(object sender, AjaxEventArgs e)
        {
            for (int i = 0; i < GridPanel_Show.ColumnModel.Columns.Count; i++)
            {
                GridPanel_Show.RemoveColumn(i);
            }
            this.Store1.RemoveFields();
            string tablename = organ.Equals("K") ? "EvalDeptTable" : "EvalPersonTable";
            DataTable table = (DataTable)Session[tablename];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField field = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                if (i > 1)
                {
                    field.Type = RecordFieldType.Float;
                }
                this.Store1.AddField(field, i);
                Column col = new Column();
                col.Header = table.Columns[i].ColumnName.Replace("（科）", "").Replace("(科)", "").Replace("（科)", "").Replace("(科）", "").Replace("（人）", "").Replace("(人)", "").Replace("（人)", "").Replace("(人）", "");
                col.Width = col.Header.Length * 18 + 20;
                col.Sortable = true;
                col.DataIndex = table.Columns[i].ColumnName;
                col.ColumnID = table.Columns[i].Caption;
                if (col.Header.Equals("科室名称") || col.Header.Equals("人员姓名") || col.Header.Equals("科室代码") || col.Header.Equals("人员编号"))
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
            string tablename = organ.Equals("K") ? "EvalDeptTable" : "EvalPersonTable";
            string filename = "评价结果";
            if (Session[tablename] == null)
            {
                return;
            }
            DataTable dt = (DataTable)Session[tablename];
            ExportData ex = new ExportData();
            ex.ExportToLocal(dt, this.Page, "xls", filename);
        }


    }
}
