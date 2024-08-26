using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;


namespace GoldNet.JXKP
{
    public partial class AddPersons : PageBase
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
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                //pro.Url = "WebService/HisDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
                pro.Url = "../../cbhs/WebService/BonusDepts.ashx";
                this.Store3.Proxy.Add(pro);
                JsonReader jr = new JsonReader();
                jr.ReaderID = "DEPT_CODE";
                jr.Root = "Bonusdepts";
                jr.TotalProperty = "totalCount";
                RecordField rf = new RecordField();
                rf.Name = "DEPT_CODE";
                jr.Fields.Add(rf);
                RecordField rfn = new RecordField();
                rfn.Name = "DEPT_NAME";
                jr.Fields.Add(rfn);
                this.Store3.Reader.Add(jr);

                SetDict();
            }
        }

        /// <summary>
        /// 下拉框设置
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_DEPT_DICT dept = new Goldnet.Dal.SYS_DEPT_DICT();
            string deptcode = Request["deptcode"].ToString();
            this.ComboBox_Role.Items.Add(new Goldnet.Ext.Web.ListItem(dept.GetDeptNameByDeptcode(deptcode), deptcode));
            this.ComboBox_Role.SelectedIndex = 0;
        }

        /// <summary>
        /// 提取his用户
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        protected void SelectedDept(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataSet ds = dal.GetRlzyPersons(this.Combo_Dept.SelectedItem.Value);

            this.Store1.DataSource = ds.Tables[0];
            this.Store1.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            SelectedDept(null, null);
        }

        /// <summary>
        /// 保存人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            if (this.ComboBox_Role.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "本科室不能为空！");
            }
            else
            {
                string index = Request["bonusid"].ToString();
                string deptcode = this.ComboBox_Role.SelectedItem.Value;
                string deptname = this.ComboBox_Role.SelectedItem.Text;
                List<PageModels.rlzylected> users = e.Object<PageModels.rlzylected>();
                Goldnet.Dal.CheckPersons dal = new Goldnet.Dal.CheckPersons();
                try
                {
                    dal.SaverlzyUsers(users, int.Parse(index), deptcode, deptname);
                    this.SaveSucceed();
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData();");
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "SubmitData");

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeptRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            string inputcode = e.Parameters["query"];
            SetDept(inputcode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.addpersonsWin.hide();");
        }

        /// <summary>
        /// 单个添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void add_Click(object sender, EventArgs e)
        {
            if (this.personsname.Text.Trim() != "")
            {
                string index = Request["bonusid"].ToString();
                string deptcode = this.ComboBox_Role.SelectedItem.Value;
                string deptname = this.ComboBox_Role.SelectedItem.Text;
                Goldnet.Dal.CheckPersons dal = new Goldnet.Dal.CheckPersons();
                try
                {
                    dal.savesingepersons(int.Parse(index), this.personsname.Text, deptcode, deptname);
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    this.SaveSucceed();
                    scManager.AddScript("parent.RefreshData();");
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "add_Click");

                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputcode"></param>
        public void SetDept(string inputcode)
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataTable table = dal.GetHisDept(inputcode).Tables[0];

            System.Data.DataView dv = table.DefaultView;
            dv.RowFilter = this.DeptFilter("dept_code");
            this.Store3.DataSource = dv;
            this.Store3.DataBind();

        }

    }
}
