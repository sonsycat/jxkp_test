using System;
using System.Data;
using Goldnet.Ext.Web;
namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Dept_Persons : PageBase
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
                ScriptManager1.RegisterIcon(Goldnet.Ext.Web.Icon.Accept);
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                pro.Url = "WebService/HisDepts.ashx";
                this.Store2.Proxy.Add(pro);
                JsonReader jr = new JsonReader();
                jr.ReaderID = "DEPT_CODE";
                jr.Root = "deptlist";
                jr.TotalProperty = "totalCount";
                RecordField rf = new RecordField();
                rf.Name = "DEPT_CODE";
                jr.Fields.Add(rf);
                RecordField rfn = new RecordField();
                rfn.Name = "DEPT_NAME";
                jr.Fields.Add(rfn);
                this.Store2.Reader.Add(jr);
                persons_select(null, null);
            }
        }

        /// <summary>
        /// 获取人组对照数据并绑定显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void persons_select(object sender, EventArgs e)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataTable table = new DataTable();
            try
            {
                table = dal.GetDeptPersons(this.ComAccountdeptcode.SelectedItem.Value).Tables[0];
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "persons_select");
            }
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataTable table = new DataTable();
            try
            {
                table = dal.GetDeptPersons(this.ComAccountdeptcode.SelectedItem.Value).Tables[0];
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "persons_select");
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
        }

        /// <summary>
        /// 转科
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void persons_changes(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                string id = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("Persons_Change_List.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("userid", id));
                showCenterSet(this.persons_change, loadcfg);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void persons_add(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("Dept_Add_Persons.aspx");
            showCenterSet(this.add_persons, loadcfg);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void persons_del(object sender, EventArgs e)
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
            string userid = sm.SelectedRow.RecordID;
            bll.deldeptpersons(userid);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("RefreshData();");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void select_users(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataSet ds = dal.GetDeptPersonsByfilter(this.txt_SearchTxt.Text.Trim());
            this.Store1.DataSource = ds;
            this.Store1.DataBind();
        }

        //设置
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
                showCenterSet(this.persons_change, loadcfg);
            }
        }

    }
}
