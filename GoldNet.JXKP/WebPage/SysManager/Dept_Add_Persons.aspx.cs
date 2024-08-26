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

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Dept_Add_Persons : PageBase
    {
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
                //
                HttpProxy projx = new HttpProxy();
                projx.Method = HttpMethod.POST;
                //pro.Url = "WebService/HisDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
                projx.Url = "../../WebService/Depts.ashx";
                this.Store4.Proxy.Add(projx);
                JsonReader jrjx = new JsonReader();
                jrjx.ReaderID = "DEPT_CODE";
                jrjx.Root = "deptlist";
                jrjx.TotalProperty = "totalCount";
                RecordField rfjx = new RecordField();
                rfjx.Name = "DEPT_CODE";
                jrjx.Fields.Add(rf);
                RecordField rfnjx = new RecordField();
                rfnjx.Name = "DEPT_NAME";
                jrjx.Fields.Add(rfnjx);
                this.Store4.Reader.Add(jrjx);

               
            }
        }
       

        /// <summary>
        /// 提取his用户
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        protected void SelectedDept(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataSet ds = dal.GetHisdoctorbydept(this.Combo_Dept.SelectedItem.Value);
            
            this.Store1.DataSource = ds;
            this.Store1.DataBind();
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            SelectedDept(null, null);
        }
        /// <summary>
        /// 人员查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void select_users(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataTable table=dal.GetDeptdoctor(this.txt_SearchTxt.Text.Trim()).Tables[0];
            if (table.Rows.Count > 0)
            {
                this.ShowMessage("提示", "这个人已经在" + table.Rows[0]["dept_name"].ToString());
            }
            else
            {
                DataSet ds = dal.GetHisdoctor(this.txt_SearchTxt.Text.Trim());
                this.Store1.DataSource = ds;
                this.Store1.DataBind();
            }
        }
        /// <summary>
        /// 对照用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedjxDept(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            this.Store2.DataSource = dal.GetDeptPersons(this.JX_dept.SelectedItem.Value).Tables[0];
            this.Store2.DataBind();
            //SelectedDept(null, null);

        }
        /// <summary>
        /// 保存医生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            if (this.JX_dept.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "绩效科室不能为空！");
            }
            else
            {
                List<PageModels.doctorselected> users = e.Object<PageModels.doctorselected>();
                Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
                try
                {
                    dal.SaveDeptDoctor(users, this.JX_dept.SelectedItem.Value);
                    this.SaveSucceed();
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "SubmitData");

                }
            }
        }
        protected void DeptRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            string inputcode = e.Parameters["query"];
            SetDept(inputcode);

        }
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
