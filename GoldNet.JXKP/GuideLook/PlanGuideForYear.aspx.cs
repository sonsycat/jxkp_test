using System;
using System.Data;
using Goldnet.Ext.Web;
using System.Collections.Generic;
using Goldnet.Dal;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.GuideLook.Statement
{
    public partial class PlanGuideForYear : PageBase
    {
        private StatementDal dal = new StatementDal();
        private GuideDalDict gdal = new GuideDalDict();
        //private static DataTable Currdt = new DataTable();
        private static DataTable dt2 = new DataTable();
        //private static string guidestaticcodes = "";
        //private String wkey = "njh";

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //if (flags.Text == "0")
                //{
                //    Currdt.Rows.Clear();
                //    Currdt.Columns.Clear();
                //}
                StatementDal dal = new StatementDal();
                //设置查询年度
                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.years.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));

                }
                this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");

                //权限设置
                bool isEdit = this.IsEdit();
                if (isEdit)
                {
                    //保存按钮可见
                    ScriptManager1.AddScript("#{Button_save}.show();");
                }

                SetDict(this.Store1);
            }
        }

        /// <summary>
        /// 设定控件
        /// </summary>
        protected void SetDict(Store store)
        {
            //获取科室对应的岗位信息
            DataTable Currdt = dal.GetStationListByDeptCode("", this.years.SelectedItem.Value).Tables[0];

            DataTable Coldt = dal.BuildDeptBonusDetail();

            AddDColumn("DEPTCODE", "科室代码", GridPanel1, store, true, true, false, false);
            AddDColumn("DEPTNAME", "科室", GridPanel1, store, false, false, false, false);
            AddDColumn("STATIONCODE", "岗位代码", GridPanel1, store, true, true, false, false);
            AddDColumn("STATIONNAME", "岗位", GridPanel1, store, false, false, false, false);

            AddGridPanel(Coldt, "GUIDE_CODE", "GUIDE_NAME", GridPanel1, store);

            Store1.DataSource = Currdt;
            Store1.DataBind();
            //Session.Remove("BonusDeptList");
            //Session["BonusDeptList"] = dtBonusValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
        }

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e, string epValue)
        {
            string row = e.ExtraParams[epValue].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Item_SelectOnChange(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Save_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e, "Values");

            if (selectRow != null)
            {
                try
                {

                    dal.SavePlanGuideForYear(selectRow, this.years.SelectedItem.Value);

                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "保存成功!",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    SetDict(this.Store1);
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "PlanGuideStats");
                }
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            SetDict(this.Store1);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Data_Bind()
        {

            //Store1.DataSource = Currdt;
            //Store1.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            //if (Currdt.Rows.Count < 1) return;
            //ExportData ex = new ExportData();
            //DataTable ExcelTable = new DataTable();
            //Currdt.Columns.Remove("DEPT_CODE");
            //Currdt.Columns[0].ColumnName = "科室";
            //Currdt.Columns[1].ColumnName = "门诊量";
            //Currdt.Columns[2].ColumnName = "平均住院日";
            //Currdt.Columns[3].ColumnName = "人均住院费用";
            //ExcelTable = Currdt;
            //ex.ExportToLocal(ExcelTable, this.Page, "xls", "年计划指标查询");
        }

        /// <summary>
        /// 生成动态列表的列
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fieldName"></param>
        /// <param name="headName"></param>
        /// <param name="gridpanel"></param>
        /// <param name="store"></param>
        private void AddGridPanel(DataTable table, string fieldName, string headName, GridPanel gridpanel, Store store)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                //生成列
                Column bonusColumn = new Column();
                bonusColumn.ColumnID = table.Rows[i][fieldName].ToString();
                bonusColumn.Header = "<center>" + table.Rows[i][headName].ToString() + "</center>";
                bonusColumn.DataIndex = table.Rows[i][fieldName].ToString();
                bonusColumn.MenuDisabled = true;
                bonusColumn.Align = Alignment.Right;
                //设定列宽度
                if (DBNull.Value.Equals(table.Rows[i]["show_width"]))
                {
                    // 默认的场合
                    bonusColumn.Width = 150;
                }
                else
                {
                    //指定宽度的场合
                    bonusColumn.Width = Convert.ToInt32(table.Rows[i]["show_width"].ToString());
                }
                ////设定列样式
                //if (DBNull.Value.Equals(table.Rows[i]["show_style"]))
                //{
                //    bonusColumn.Renderer.Fn = "rmbMoney";
                //}
                //else
                //{
                //    if (table.Rows[i]["show_style"].ToString().Equals("0"))
                //    {
                //        bonusColumn.Renderer.Fn = "rmbMoney";
                //    }
                //    else
                //    {
                //        bonusColumn.Renderer.Fn = "highLight";
                //    }
                //}
                NumberField fils = new NumberField();
                fils.ID = i.ToString();
                fils.SelectOnFocus = true;
                fils.DecimalPrecision = 2;
                bonusColumn.Editor.Add(fils);
                gridpanel.ColumnModel.Columns.Add(bonusColumn);

                RecordField recordfield = new RecordField();
                recordfield.Name = table.Rows[i][fieldName].ToString();
                store.AddField(recordfield);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="headName"></param>
        /// <param name="gridpanel"></param>
        /// <param name="store"></param>
        /// <param name="hide"></param>
        /// <param name="fix"></param>
        /// <param name="edits"></param>
        /// <param name="money"></param>
        private void AddDColumn(string fieldName, string headName, GridPanel gridpanel, Store store, bool hide, bool fix, bool edits, bool money)
        {
            Column bonusColumn = new Column();
            bonusColumn.ColumnID = fieldName;
            bonusColumn.Header = headName;
            bonusColumn.DataIndex = fieldName;
            bonusColumn.MenuDisabled = true;
            bonusColumn.Hidden = hide;
            bonusColumn.Width = 130;
            if (money)
            {
                bonusColumn.Renderer.Fn = "rmbMoney";
            }
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

    }
}
