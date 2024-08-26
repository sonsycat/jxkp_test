using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class PowerManager :PageBase
    {
        /// <summary>
        /// 初始化
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
                pro.Url = "WebService/HisDepts.ashx";
                this.Store3.Proxy.Add(pro);
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
                this.Store3.Reader.Add(jr);

                SetDict();
            }
        }

        /// <summary>
        /// 下拉框设置
        /// </summary>
        public void SetDict()
        {
            //科室下拉框
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataTable table = dal.GetHisDept("", this.DeptFilter("dept_code")).Tables[0];
            //DataTable tb=table.Select("dept_code='200201'").CopyToDataTable();
            //int it = tb.Rows.Count;
            this.Store3.DataSource = table;
            this.Store3.DataBind();
            //角色下拉框
            User user = (User)Session["CURRENTSTAFF"];
            DataTable roletable = dal.GetList("").Tables[0];
            DataRow[] selrole=roletable.Select(user.GetRoleFilter("role_app"));
            foreach (DataRow dr in selrole)
            {
                this.ComboBox_Role.Items.Add(new Goldnet.Ext.Web.ListItem(dr["role_name"].ToString(), dr["role_id"].ToString()));
            }
        }
        
        /// <summary>
        /// 选择科室下拉框触发，获取his用户
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        protected void SelectedDept(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataSet ds = dal.GetHisUsersbyrole(this.ComboBox_Role.SelectedItem.Value);
            System.Data.DataView dv=ds.Tables[0].DefaultView;
            if(this.Combo_Dept.SelectedItem.Value!=string.Empty)
            {
                dv.RowFilter="dept_code='"+this.Combo_Dept.SelectedItem.Value+"'";
            }
            this.Store1.DataSource = dv;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            SelectedDept(null,null);
        }

        /// <summary>
        /// 人员查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void select_users(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataSet ds = dal.GetHisUsers(this.txt_SearchTxt.Text);
            this.Store1.DataSource = ds;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 选择角色下拉框触发，获取角色用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Selectedrole(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_ROLE_DICT roledal = new Goldnet.Dal.SYS_ROLE_DICT();
            this.Store2.DataSource = roledal.GetRoleUser(this.ComboBox_Role.SelectedItem.Value).Tables[0];
            this.Store2.DataBind();
            SelectedDept(null, null);
        }

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            if (this.ComboBox_Role.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "角色不能为空！");
            }
            else
            {
                List<PageModels.userselected> users = e.Object<PageModels.userselected>();
                Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
                try
                {
                    dal.SaveRoleUsers(users, int.Parse(this.ComboBox_Role.SelectedItem.Value));
                    this.SaveSucceed();
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
