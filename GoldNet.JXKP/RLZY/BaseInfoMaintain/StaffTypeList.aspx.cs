using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm.DAL.Oracle;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class StaffTypeList : PageBase
    {
        BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {

                string deptcode = this.DeptFilter("");
                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + deptcode;
                this.Store3.Proxy.Add(proxy);

                DataTable l_dt = PersTypeFilter();
                string sort = "";
                for (int i = 0; i < l_dt.Rows.Count; i++)
                {
                    sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
                }
                if (l_dt.Rows.Count == 0)
                {
                    sort = "'-1'";
                }
                
                //生成人员分类列表
                this.BuildDeptBonusDetail(Store1, GridPanel_Show);

                Store1.DataSource = dal.ViewStaffTypeList("", true, deptcode, sort.TrimEnd(new char[]{','})).Tables[0];
                Store1.DataBind();
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataTable l_dt = PersTypeFilter();
            string sort = "";
            for (int i = 0; i < l_dt.Rows.Count; i++)
            {
                sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
            }
            if (l_dt.Rows.Count == 0)
            {
                sort = "'-1'";
            }

            Store1.RemoveAll();

            //生成人员分类列表
            this.BuildDeptBonusDetail(Store1, GridPanel_Show);

            Store1.DataSource = dal.ViewStaffTypeList(this.DeptCodeCombo.SelectedItem.Value, this.cbxInline.Checked, this.DeptFilter(""), sort.TrimEnd(new char[] { ',' })).Tables[0];
            Store1.DataBind();
        }

        /// <summary>
        /// 生成人员分类列表
        /// </summary>
        /// <param name="store"></param>
        /// <param name="gridpanel"></param>
        private void BuildDeptBonusDetail(Store store, GridPanel gridpanel)
        {
            // 获取人员分类
            string strcode = String.Format(@"SELECT 'A' || SERIAL_NO AS ID,PERS_SORT_NAME  FROM RLZY.PERS_SORT_DICT");
            DataTable table = OracleOledbBase.ExecuteDataSet(strcode).Tables[0];

            AddDColumn("DEPT_NAME", "科室", gridpanel, store, false, false, false, false);
            
            gridpanel.AutoWidth = true;

            AddGridPanel(table, "ID", "PERS_SORT_NAME", gridpanel, store);
        }

        /// <summary>
        /// 构造人员类别列表
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="headName">表头</param>
        /// <param name="gridpanel">区域对象</param>
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
            bonusColumn.Width = 140;
            bonusColumn.Sortable = true;
 
            gridpanel.ColumnModel.Columns.Add(bonusColumn);
            //gridpanel.AddColumn(bonusColumn);
 
            RecordField recordfield = new RecordField();
            recordfield.Name = fieldName;

            store.AddField(recordfield);
        }

        /// <summary>
        /// 构造人员类别列表
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
                Column bonusColumn = new Column();
                bonusColumn.ColumnID = table.Rows[i][fieldName].ToString();
                bonusColumn.Header = table.Rows[i][headName].ToString();
                bonusColumn.DataIndex = table.Rows[i][fieldName].ToString();
                bonusColumn.MenuDisabled = true;
                bonusColumn.Width = 110;
                bonusColumn.Sortable = true;
                gridpanel.ColumnModel.Columns.Add(bonusColumn);

                RecordField recordfield = new RecordField();
                recordfield.Name = table.Rows[i][fieldName].ToString();
                
                store.AddField(recordfield);
            }
        }


    }
}
