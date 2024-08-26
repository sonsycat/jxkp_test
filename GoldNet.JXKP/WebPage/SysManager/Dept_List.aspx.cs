using System;
using System.Data;
using Goldnet.Ext.Web;


namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Dept_List : PageBase
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                SelectedDepttype(null, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            this.GridPanel1.Dispose();
            this.Store1.Dispose();
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataTable table = new DataTable();
            try
            {
                table = dal.GetDeptSet(this.Combo_DeptType.SelectedItem.Value, this.ComShowflag.SelectedItem.Value, "").Tables[0];
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "SelectedDepttype");
            }
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 初始化科室下拉列表
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataTable table = dal.GetDeptType().Tables[0];
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.Combo_DeptType.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["ATTRIBUE"].ToString(), table.Rows[i]["id"].ToString()));
                }
            }
        }

        /// <summary>
        /// 查询科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedDepttype(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataTable table = new DataTable();
            try
            {
                table = dal.GetDeptSet(this.Combo_DeptType.SelectedItem.Value, this.ComShowflag.SelectedItem.Value, "").Tables[0];
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "SelectedDepttype");
            }
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Find(object sender, AjaxEventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;

            //sm.SelectRow(int.Parse(this.Fields.Text),true);
        }

        //双击设置
        protected void DbRowClick(object sender, AjaxEventArgs e)
        {

            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                string deptcode = sm.SelectedRow.RecordID;

                LoadConfig loadcfg = getLoadConfig("DeptSet.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
                showCenterSet(this.Dept_Set, loadcfg);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonadd_Click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("DeptAdd.aspx");
            showCenterSet(this.Dept_Set, loadcfg);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttondel_Click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.SelectRecord();
            }
            else
            {

                Ext.Msg.Confirm("系统提示", "您确定要删除选中记录吗？", new MessageBox.ButtonsConfig
                {
                    Yes = new MessageBox.ButtonConfig
                    {
                        Handler = "Goldnet.del()",
                        Text = "确定"

                    },
                    No = new MessageBox.ButtonConfig
                    {
                        Text = "取消"
                    }
                }).Show();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [AjaxMethod]
        public void del()
        {
            Goldnet.Dal.SYS_DEPT_DICT bll = new Goldnet.Dal.SYS_DEPT_DICT();
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            string deptcode = sm.SelectedRow.RecordID;
            bll.DelDept(deptcode);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("RefreshData();");
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonset_Click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                string deptcode = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("DeptSet.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
                showCenterSet(this.Dept_Set, loadcfg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void select_dept(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataSet ds = dal.GetDeptSet("", "", this.txt_SearchTxt.Text);
            this.Store1.DataSource = ds.Tables[0];
            this.Store1.DataBind();
        }

    }

}
