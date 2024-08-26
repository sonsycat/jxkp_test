using System;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Comm.DAL.Oracle;


namespace GoldNet.JXKP
{
    public class BuildControl
    {
        /// <summary>
        /// 建立部门奖金明细的GridPanel
        /// </summary>
        /// <param name="bonusId">奖金ID</param>
        /// <param name="bonusType">平均奖 /核算科室</param>
        /// <param name="store"></param>
        /// <param name="gridpanel"></param>
        public void BuildDeptBonusDetail(string bonusId, string bonusType, Store store, GridPanel gridpanel, string org)
        {
            string orgstr = "";
            if (org == "2" || org == "3")
            {
                orgstr = " and a.show_col='1'";
            }

            string strcode = "";
            if (bonusType == "1")//1表示核算科室
            {
                strcode = String.Format(@"SELECT 'A' || a.guide_code guide_code, a.guide_name,A.SHOW_WIDTH,a.show_style,b.EXPLAIN
                                      FROM performance.bonus_guide a,hospitalsys.GUIDE_NAME_DICT b,performance.SET_BONUSGUIDE_GATHER c
                                           where a.index_id='{0}' and a.guide_code=b.guide_code and b.guide_code=c.guide_code
                                       and a.guide_type='5' {1} order by c.sort", bonusId, orgstr);
            }
            else
            {
                strcode = String.Format(@"SELECT 'A' || a.guide_code guide_code, a.guide_name,A.SHOW_WIDTH,a.show_style,b.EXPLAIN
                                      FROM performance.bonus_guide a,hospitalsys.GUIDE_NAME_DICT b,performance.SET_BONUSGUIDE_GATHER c
                                           where a.index_id='{0}' and a.guide_code=b.guide_code and b.guide_code=c.guide_code
                                       and a.guide_type='6' {1} order by c.sort", bonusId, orgstr); 
            }
            DataTable table = OracleOledbBase.ExecuteDataSet(strcode).Tables[0];

            AddDColumn("UNIT_CODE", "科室ID", gridpanel, store, true, true, false, false);
            AddDColumnRender("UNIT_NAME", "科室", gridpanel, store, false, true, false, false);
            if (bonusType == "1")
            {
                AddDColumn("UNIT_DIRECTOR", "科主任", gridpanel, store, true, false, false, false);
            }
            gridpanel.AutoWidth = true;

            AddGridPanel(table, "guide_code", "guide_name", gridpanel, store);
        }

        /// <summary>
        /// 新控件使用
        /// </summary>
        /// <param name="bonusId"></param>
        /// <param name="bonusType"></param>
        /// <param name="store"></param>
        /// <param name="gridpanel"></param>
        /// <param name="org"></param>
        public DataTable BuildDeptBonusDetail_new(string bonusId, string bonusType, string org,string user_id,string deptcode)
        {
            string orgstr = "";
            if (org == "2" || org == "3")
            {
                orgstr = " and a.show_col='1'";
            }

            string type = "";

            if (string.IsNullOrEmpty(deptcode))
            {
                string target = String.Format(@"select power_id from COMM.SYS_POWER_DETAIL where target_id='{0}' ", user_id);

                //int resultTarget = int.Parse(OracleOledbBase.ExecuteScalar(target).ToString());

                int resultTarget = 2;

                string deptType = String.Format(@"select dept_type from comm.SYS_DEPT_DICT where dept_code={0} ", deptcode);

                //int resultDeptType = int.Parse(OracleOledbBase.ExecuteScalar(deptType).ToString());

                int resultDeptType = 2;

                if (resultTarget == 1)
                { }
                else if (resultTarget == 2)
                {
                    if (resultDeptType == 0)
                    {
                        type = String.Format(@" and a.guide_code in (select guide_code from HOSPITALSYS.GUIDE_NAME_DICT where indextype in ('0','1') ) ");
                    }
                    else if (resultDeptType == 2)
                    {
                        type = String.Format(@" and a.guide_code in (select guide_code from HOSPITALSYS.GUIDE_NAME_DICT where indextype in ('0','2') ) ");
                    }
                }
            }

            string strcode = "";
            if (bonusType == "1")//1表示核算科室
            {
                strcode = String.Format(@"SELECT 'A' || a.guide_code guide_code, a.guide_name,A.SHOW_WIDTH,a.show_style,b.EXPLAIN
                                      FROM performance.bonus_guide a,hospitalsys.GUIDE_NAME_DICT b,performance.SET_BONUSGUIDE_GATHER c
                                           where a.index_id='{0}' and a.guide_code=b.guide_code and b.guide_code=c.guide_code
                                       and a.guide_type='5' {1} {2} order by c.sort", bonusId, orgstr,type);
            }
            else
            {
                strcode = String.Format(@"SELECT 'A' || a.guide_code guide_code, a.guide_name,A.SHOW_WIDTH,a.show_style,b.EXPLAIN
                                      FROM performance.bonus_guide a,hospitalsys.GUIDE_NAME_DICT b,performance.SET_BONUSGUIDE_GATHER c
                                           where a.index_id='{0}' and a.guide_code=b.guide_code and b.guide_code=c.guide_code
                                       and a.guide_type='6' {1} {2} order by c.sort", bonusId, orgstr, type);
            }
            return  OracleOledbBase.ExecuteDataSet(strcode).Tables[0];

            //AddDColumn("UNIT_CODE", "科室ID", gridpanel, store, true, true, false, false);
            //AddDColumnRender("UNIT_NAME", "科室", gridpanel, store, false, true, false, false);
            //if (bonusType == "1")
            //{
            //    AddDColumn("UNIT_DIRECTOR", "科主任", gridpanel, store, true, false, false, false);
            //}
            //gridpanel.AutoWidth = true;

            //AddGridPanel(table, "guide_code", "guide_name", gridpanel, store);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="store"></param>
        /// <param name="gridpanel"></param>
        public void BuildDistributeBonus(DataTable dt, Store store, GridPanel gridpanel)
        {
            gridpanel.Reconfigure();
            gridpanel.ColumnModel.Columns.Clear();
            store.RemoveFields();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "DEPT_NAME")
                {
                    ADDDtributeColumn("DEPT_NAME", "科室", gridpanel, store, false, false, false, false);

                }
                else if (dt.Columns[i].ColumnName == "SCORE")
                {
                    ADDDtributeColumn("SCORE", "得分", gridpanel, store, false, false, false, false);

                }
                else if (dt.Columns[i].ColumnName == "BONUS")
                {
                    ADDDtributeColumn("BONUS", "奖金", gridpanel, store, false, false, false, true);

                }
                else if (dt.Columns[i].ColumnName == "DEPT_CODE")
                {
                    ADDDtributeColumn("DEPT_CODE", "科室代码", gridpanel, store, true, false, false, false);

                }
                else
                {
                    ADDDtributeColumn(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName, gridpanel, store, false, false, false, false);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="store"></param>
        /// <param name="gridpanel"></param>
        public void BuildDistributeBonus1(DataTable dt, Store store, GridPanel gridpanel)
        {
            gridpanel.Reconfigure();
            gridpanel.ColumnModel.Columns.Clear();
            store.RemoveFields();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "DEPT_NAME")
                {
                    AddColumn("DEPT_NAME", "科室", gridpanel, store, false, false, false, false);

                }
                else if (dt.Columns[i].ColumnName == "SCORE")
                {
                    AddColumn("SCORE", "得分", gridpanel, store, false, false, false, false);

                }
                else if (dt.Columns[i].ColumnName == "BONUS")
                {
                    AddColumn("BONUS", "奖金", gridpanel, store, false, false, false, true);

                }
                else if (dt.Columns[i].ColumnName == "DEPT_CODE")
                {
                    AddColumn("DEPT_CODE", "科室代码", gridpanel, store, true, false, false, false);

                }
                else
                {
                    AddColumn(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName, gridpanel, store, false, false, false, false);
                }
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
        private void ADDDtributeColumn(string fieldName, string headName, GridPanel gridpanel, Store store, bool hide, bool fix, bool edits, bool money)
        {
            Column bonusColumn = new Column();
            bonusColumn.ColumnID = fieldName;
            bonusColumn.Header = headName;
            bonusColumn.DataIndex = fieldName;
            bonusColumn.MenuDisabled = true;
            bonusColumn.Hidden = hide;
            //bonusColumn.Locked = fix;
            bonusColumn.Width = Unit.Pixel(200);

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
            gridpanel.AddColumn(bonusColumn);
            RecordField recordfield = new RecordField();
            recordfield.Name = fieldName;
            store.AddField(recordfield);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="headName"></param>
        /// <param name="gridpanel"></param>
        /// <param name="store"></param>
        /// <param name="hide"></param>
        /// <param name="money"></param>
        private void AddColumn(string fieldName, string headName, GridPanel gridpanel, Store store, bool hide, bool money)
        {
            AddColumn(fieldName, headName, gridpanel, store, hide, true, money);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="headName"></param>
        /// <param name="gridpanel"></param>
        /// <param name="store"></param>
        /// <param name="hide"></param>
        /// <param name="edits"></param>
        /// <param name="money"></param>
        private void AddColumn(string fieldName, string headName, GridPanel gridpanel, Store store, bool hide, bool edits, bool money)
        {
            AddColumn(fieldName, headName, gridpanel, store, hide, false, edits, money);
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
        private void AddColumn(string fieldName, string headName, GridPanel gridpanel, Store store, bool hide, bool fix, bool edits, bool money)
        {
            Column bonusColumn = new Column();
            bonusColumn.ColumnID = fieldName;
            bonusColumn.Header = headName;
            bonusColumn.DataIndex = fieldName;
            bonusColumn.MenuDisabled = true;
            bonusColumn.Hidden = hide;
            bonusColumn.Width = 200;

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
            gridpanel.AddColumn(bonusColumn);
            RecordField recordfield = new RecordField();
            recordfield.Name = fieldName;
            store.AddField(recordfield);
        }

        /// <summary>
        /// 建立核算科室人员的列
        /// </summary>
        /// <param name="store"></param>
        /// <param name="gridpanel"></param>
        public void BuildAccountPerson(Store store, GridPanel gridpanel)
        {

            gridpanel.Reconfigure();
            gridpanel.ColumnModel.Columns.Clear();
            store.RemoveFields();

            AddColumn("NAME", "人员", gridpanel, store, false, false, false);
            AddColumn("BONUS", "三级奖金", gridpanel, store, false, true);
            AddColumn("SEC_BONUS", "二级奖金", gridpanel, store, false, true);
            AddColumn("BANK_CODE", "账号", gridpanel, store, false, false);
            AddRecord("ID", store);
            AddRecord("NAME", store);
            AddRecord("BONUS", store);
            AddRecord("SEC_BONUS", store);
            AddRecord("BANK_CODE", store);
            AddRecord("ISSAVE", store);
        }

        /// <summary>
        /// 建立平均奖科室人员的列
        /// </summary>
        /// <param name="store"></param>
        /// <param name="gridpanel"></param>
        public void BuildAvgPerson(Store store, GridPanel gridpanel)
        {
            gridpanel.Reconfigure();
            gridpanel.ColumnModel.Columns.Clear();
            store.RemoveFields();

            AddColumn("NAME", "人员", gridpanel, store, false, false, false);
            AddColumn("BONUSMODULUS", "系数", gridpanel, store, false, false, false);
            AddColumn("BONUS", "奖金", gridpanel, store, false, true, true);
            AddColumn("BANK_CODE", "账号", gridpanel, store, false, true, false);
            AddRecord("ID", store);
            AddRecord("NAME", store);
            AddRecord("BONUSMODULUS", store);
            AddRecord("BONUS", store);
            AddRecord("BANK_CODE", store);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordfieldname"></param>
        /// <param name="store"></param>
        private void AddRecord(string recordfieldname, Store store)
        {
            RecordField recordfield = new RecordField();
            recordfield.Name = recordfieldname;
            store.AddField(recordfield);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="headName"></param>
        /// <param name="gridpanel"></param>
        /// <param name="store"></param>
        /// <param name="hide"></param>
        /// <param name="money"></param>
        private void AddDColumn(string fieldName, string headName, ExtGridPanel gridpanel, Store store, bool hide, bool money)
        {
            AddDColumn(fieldName, headName, gridpanel, store, hide, true, money);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="headName"></param>
        /// <param name="gridpanel"></param>
        /// <param name="store"></param>
        /// <param name="hide"></param>
        /// <param name="edits"></param>
        /// <param name="money"></param>
        private void AddDColumn(string fieldName, string headName, ExtGridPanel gridpanel, Store store, bool hide, bool edits, bool money)
        {
            AddDColumn(fieldName, headName, gridpanel, store, hide, false, edits, money);
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
            //Column bonusColumn = new Column();
            ExtColumn bonusColumn = new ExtColumn();
            bonusColumn.ColumnID = fieldName;
            bonusColumn.Header = headName;
            bonusColumn.DataIndex = fieldName;
            bonusColumn.MenuDisabled = true;
            bonusColumn.Hidden = hide;
            bonusColumn.Locked = fix;
            bonusColumn.Sortable = false;
            bonusColumn.Width = 200;
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
            //gridpanel.AddColumn(bonusColumn);
            RecordField recordfield = new RecordField();
            recordfield.Name = fieldName;
            store.AddField(recordfield);
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
        private void AddDColumnRender(string fieldName, string headName, GridPanel gridpanel, Store store, bool hide, bool fix, bool edits, bool money)
        {
            //Column bonusColumn = new Column();
            ExtColumn bonusColumn = new ExtColumn();
            bonusColumn.ColumnID = fieldName;
            bonusColumn.Header = headName;
            bonusColumn.DataIndex = fieldName;
            bonusColumn.MenuDisabled = true;
            bonusColumn.Hidden = hide;
            bonusColumn.Locked = fix;
            bonusColumn.Sortable = false;

            bonusColumn.Width = 200;

            bonusColumn.Renderer.Fn = "highLight";

            gridpanel.ColumnModel.Columns.Add(bonusColumn);
            // gridpanel.AddColumn(bonusColumn);
            RecordField recordfield = new RecordField();
            recordfield.Name = fieldName;
            store.AddField(recordfield);
        }

        /// <summary>
        /// 
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
                ExtColumn bonusColumn = new ExtColumn();
                bonusColumn.ColumnID = table.Rows[i][fieldName].ToString();
                bonusColumn.Header = "<div style='text-align:center;'>" + table.Rows[i][headName].ToString() + "</div>";
                bonusColumn.DataIndex = table.Rows[i][fieldName].ToString();
                bonusColumn.MenuDisabled = true;
                bonusColumn.Sortable = false;
                if (DBNull.Value.Equals (table.Rows[i]["show_width"]))
                {
                    bonusColumn.Width = 150;
                }
                else
                {
                    bonusColumn.Width = Convert.ToInt32(table.Rows[i]["show_width"].ToString());
                }
                bonusColumn.Tooltip = table.Rows[i]["EXPLAIN"].ToString();
                bonusColumn.Align = Alignment.Right;
                if (DBNull.Value.Equals(table.Rows[i]["show_style"]))
                {
                    bonusColumn.Renderer.Fn = "rmbMoney";
                }
                else
                {
                    if (table.Rows[i]["show_style"].ToString().Equals("0"))
                    {
                        bonusColumn.Renderer.Fn = "rmbMoney";
                    }
                    else
                    {
                        bonusColumn.Renderer.Fn = "highLight";
                    }
                }

                gridpanel.ColumnModel.Columns.Add(bonusColumn);

                RecordField recordfield = new RecordField();
                recordfield.Name = table.Rows[i][fieldName].ToString();
                store.AddField(recordfield);
            }

            ExtColumn bonusColumnE = new ExtColumn();
            bonusColumnE.ColumnID = "AAAAAA";
            bonusColumnE.Header = "其他";
            bonusColumnE.DataIndex = "AAAAAA";
            bonusColumnE.MenuDisabled = true;
            bonusColumnE.Sortable = false;
            bonusColumnE.Width = 60;
            gridpanel.ColumnModel.Columns.Add(bonusColumnE);
        }

    }
}
