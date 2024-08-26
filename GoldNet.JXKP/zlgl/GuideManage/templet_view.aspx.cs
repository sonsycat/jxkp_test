using System;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.Pic;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.JXKP.Templet.BLL;
using System.Collections;
using System.ComponentModel;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using GoldNet.JXKP.PowerManager;

namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class templet_view : PageBase
    {
        private int _templetID;
        private TempletBO _templet;
        private DataTable _dataTable;

        // 定义当前显示的视图，可能是默认视图，也可能是由查询产生的临时视图。
        private GoldNet.JXKP.Templet.BLL.ListView _currentView;

        private const string FILTER_STRING = "FILTER_STRING";
        private const string SORT_FIELD = "SORT_FIELD";
        private const string SORT_DESC = "SORT_DESC";
        private const string CURRENT_PAGE = "CURRENT_PAGE";

        private int _dateFieldID, _deptFieldID;
        private GoldNet.JXKP.Templet.BLL.Field _dateField, _deptField;
        private DateTime _startDate, _endDate;
        private string _deptCode;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
                _templetID = this.GetInt32ByQueryStr("templetid");
                _deptFieldID = this.GetInt32ByQueryStr("deptfieldid");
                _dateFieldID = this.GetInt32ByQueryStr("datefieldid");
                _startDate = Convert.ToDateTime(this.GetStringByQueryStr("startdate"));
                _endDate = Convert.ToDateTime(this.GetStringByQueryStr("enddate"));
                _deptCode = this.GetStringByQueryStr("deptcode");

                _templet = new TempletBO(_templetID);
                _dateField = new GoldNet.JXKP.Templet.BLL.Field(_dateFieldID);
                _deptField = new GoldNet.JXKP.Templet.BLL.Field(_deptFieldID);
                //FieldCollectionTable deptfield = _templet.GetDeptFields();
                //GoldNet.JXKP.Templet.BLL.Field fields = (GoldNet.JXKP.Templet.BLL.Field)deptfield[0];

                // 得到当前用户
                IStaffTargetInfo currentStaff = Staff.GetStaff();
                string str11 = currentStaff.Dept;

                _templet = new TempletBO(_templetID);
                _dataTable = _templet.GetAllRecord();
                //
                string filterStr = "(" + _dateField.ListDisplayDataName + " <= #" + _endDate.ToShortDateString() + "#) AND "
                + "(" + _dateField.ListDisplayDataName + " >= #" + _startDate.ToShortDateString() + "#) AND "
                + "(" + _deptField.ListDisplayDataName.Replace("DEPT_", "DEPT_ID_") + " in  " + dal.GetDeptbyaccountdept(_deptCode) + ")";
                //
                System.Data.DataView dv = _dataTable.DefaultView;
                dv.RowFilter = filterStr;
                for (int i = 0; i < _dataTable.Columns.Count; i++)
                {
                    RecordField field = new RecordField();
                    if (_dataTable.Columns[i].ColumnName.Length > 4 && _dataTable.Columns[i].ColumnName.Substring(0, 4) == "DATE")
                    {
                        field = new RecordField(_dataTable.Columns[i].ColumnName.ToUpper(), RecordFieldType.Date);
                    }
                    else field = new RecordField(_dataTable.Columns[i].ColumnName.ToUpper(), RecordFieldType.String);
                    this.Store1.AddField(field, i);
                }
                this.Store1.DataSource = dv;
                this.Store1.DataBind();

                _currentView = _templet.DefaultView;
                Session["CURRENT_VIEW"] = _currentView;

                // 添加显示绑定列
                foreach (GoldNet.JXKP.Templet.BLL.Field field in _currentView.DisplayFields)
                {
                    Column cl = new Column();
                    cl.Header = field.FieldName;
                    // cl.Width = Unit.Pixel(66);
                    cl.Sortable = true;
                    cl.MenuDisabled = true;
                    cl.ColumnID = field.ListDisplayDataName.ToUpper();
                    cl.DataIndex = field.ListDisplayDataName.ToUpper();
                    if (field.FieldTypeName == "日期")
                    {

                        cl.Renderer.Fn = "Ext.util.Format.dateRenderer('Y-m-d')";
                    }
                    this.GridPanel.AddColumn(cl);
                }
                if (!(_currentView.PageCount == 0))
                {
                    PagingToolbar ptb = new PagingToolbar();
                    ptb.ID = "PagingToolBar1";
                    ptb.PageSize = _currentView.PageCount;
                    ptb.StoreID = "Store1";
                    ptb.AutoWidth = true;
                    ptb.AutoDataBind = true;

                    GridPanel.BottomBar.Add(ptb);
                }



            }
        }

       
        //双击查看详细信息
        protected void DbRowClick(object sender, AjaxEventArgs e)
        {
            //string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            //string menuid = pageids[0].ToString();
            _templetID = this.GetInt32ByQueryStr("templetid");

            RowSelectionModel sm = this.GridPanel.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.SelectRecord();
            }
            else
            {
                string id = sm.SelectedRow.RecordID;

                LoadConfig loadcfg = getLoadConfig("../Templet/Page/View.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("templetid", _templetID.ToString()));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("recid", id));
                showCenterSet(this.ListDetail, loadcfg);
            }
        }
       

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("DeptQualityView.aspx?Deptcode=" + this.GetStringByQueryStr("deptcode") + "&year=" + Convert.ToDateTime(this.GetStringByQueryStr("startdate")).Year.ToString() + "&month=" + Convert.ToDateTime(this.GetStringByQueryStr("startdate")).Month.ToString());
        }

    }
}
