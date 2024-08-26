using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.RLZY.DICT
{
    public partial class DictMainTain : System.Web.UI.Page
    {
        private static string fieldId = "ID";
        private static string fieldName = "";
        private static string flg = "IS_DEL";
        private static bool flgDel = false;
        private static string gridePanelIDName = "序号";
        private static string gridePanelName = "";
        private static string TableName = "";
        DictMainTainDal dal = new DictMainTainDal();

        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                RecoveryDeInfo();
                TableName = Request.QueryString["TABLENAME"].ToString();
                this.Store1.DataSource = TableInfo(TableName);
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 查询字典
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns>集合</returns>
        private DataTable TableInfo(string TableName) 
        {

            switch (TableName) 
            {
                case "CIVILSERVICECLASS_DICT":
                    fieldName = "CIVILSERVICECLASS";
                    flgDel = true;
                    gridePanelName = "文职级";
                    break;
                case "JOB_DICT":
                    fieldName = "JOB";
                    flgDel = true;
                    gridePanelName = "职称";
                    break;
                case "SPEC_CENTER_DICT":
                    fieldName = "CENTER_NAME";
                    gridePanelName = "专科中心名称";
                    gridePanelIDName = "专科中心代码";
                    break;
                //科室类别字典
                case "DEPT_SORT_DICT":
                    fieldId = "SEID";
                    fieldName = "SORT_NAME";
                    gridePanelName = "专科中心类别名称";
                    break;
                //人才培养类别字典
                case "PERSONS_PLANT_DICT":
                    fieldName = "PERSONS_PLANT";
                    flgDel = true;
                    gridePanelName = "人才培养";
                    break;
                //任务来源字典
                case "TASK_SOURCE_DICT":
                    fieldName = "TASK_SOURCE";
                    flgDel = true;
                    gridePanelName = "任务来源";
                    break;
                //刊物等级字典
                case "PUBLICATION_GRADE_DICT":
                    fieldName = "PUBLICATION_GRADE";
                    flgDel = true;
                    gridePanelName = "刊物等级";
                    break;
                //学术会议等级字典
                case "TECH_GRADE_DICT":
                    fieldName = "GRADE_NAME";
                    gridePanelName = "等级名称";
                    break;
                //开本字典
                case "FORMAT_DICT":
                    fieldName = "FORMAT";
                    flgDel = true;
                    gridePanelName = "开本";
                    break;
                //成果性质字典
                case "FRUIT_QUALITY_DICT":
                    fieldName = "FRUIT_QUALITY";
                    flgDel = true;
                    gridePanelName = "成果性质";
                    break;
                ////特殊诊疗项目字典,特殊字段
                //case "ESPE_DIAG_ITEM_DICT":
                //    fieldId = "DIAG_CODE";
                //    fieldName = "DIAG_CODE";
                //    gridePanelName = "名称";
                //    break;
            }
            this.GridPanel_Show.ColumnModel.Columns[1].Header = gridePanelIDName;
            this.GridPanel_Show.ColumnModel.Columns[1].Header = gridePanelName;

            return dal.getDictInfo(TableName, fieldId, fieldName, flgDel, flg).Tables[0];
        }

        [AjaxMethod]
        public void DictAjaxOper(string DictCode, string DictName, string OperType)
        {
            switch (OperType)
            {
                case "1":
                    dal.InsertDictInfo(TableName, fieldId, fieldName, DictCode, DictName);
                    break;
                case "2":
                    dal.UpdatedictInfo(TableName, fieldId, fieldName, flgDel, flg, DictCode, DictName);
                    break;
                case "3":
                    dal.DelDictInfo(TableName, fieldId,flgDel, flg, DictCode);
                    break;
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            ReportDalDict dal = new ReportDalDict();
            Store1.RemoveAll();
            Store1.DataSource = TableInfo(TableName);
            Store1.DataBind();
        }


        private void RecoveryDeInfo() 
        {
             fieldId = "ID";
             fieldName = "";
             flg = "IS_DEL";
             flgDel = false;
             gridePanelIDName = "序号";
             gridePanelName = "";
             TableName = "";
        }
    }
}
