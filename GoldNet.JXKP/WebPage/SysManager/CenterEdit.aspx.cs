using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Collections.Generic;
using GoldNet.Model;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class CenterEdit :PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                pro.Url = "WebService/HisUsers.ashx";
                this.Store1.Proxy.Add(pro);
                getdept();
                SetDict();
                Edit();
            }
        }

        private void getdept()
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
        }
        /// <summary>
        /// 下拉框设置
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();

            DataTable roletable = dal.GetList("role_app='-1'").Tables[0];
            DataRow[] selrole = roletable.Select();
            foreach (DataRow dr in selrole)
            {
                this.Combo_Role.Items.Add(new Goldnet.Ext.Web.ListItem(dr["role_name"].ToString(), dr["role_id"].ToString()));
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            if (this.Text_CenterName.Text == string.Empty)
            {
                this.ShowMessage("系统提示", "专科中心名称不能为空！");
            }
            else
            {
                Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();

                string centername = this.Text_CenterName.Text;
                string directorid = this.Com_Director.SelectedItem.Value;
                string directorname = this.Com_Director.SelectedItem.Text;
                string roleid = this.Combo_Role.SelectedItem.Value;
                int id = 0;
                if (this.Com_Director.SelectedItem.Text == string.Empty)
                {
                    directorid = "";
                    directorname = "";
                }

                if (Session["centertable"] != null)
                {
                    id = int.Parse(((DataTable)(Session["centertable"])).Rows[0]["id"].ToString());
                    if (this.Com_Director.SelectedItem.Value == this.Com_Director.SelectedItem.Text)
                    {
                        directorid = ((DataTable)(Session["centertable"])).Rows[0]["CENTER_DIRECTOR_ID"].ToString();
                        directorname = ((DataTable)(Session["centertable"])).Rows[0]["CENTER_DIRECTOR_NAME"].ToString();
                    }
                }
                try
                {
                    dal.EditCenter(id, centername, directorid, directorname, roleid);
                    this.ShowMessage("提示", "保存成功！");
                    Session["centerid"] = null;
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData();");
                    scManager.AddScript("parent.Center_Edit.hide();");
                    scManager.AddScript("parent.Center_Edit.clearContent();");
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "Buttonsave_Click");

                }
                
            }

        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public void Edit()
        {
            if (Request["centerid"].ToString() != "")
            {
                Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
                DataTable table = dal.GetCenterList(int.Parse(Request["centerid"].ToString())).Tables[0];
                if (table.Rows.Count > 0)
                {

                    this.Text_CenterName.Text = table.Rows[0]["CENTER_NAME"].ToString();
                    this.Com_Director.SelectedItem.Text = table.Rows[0]["CENTER_DIRECTOR_NAME"].ToString();
                    this.Combo_Role.SelectedItem.Value = table.Rows[0]["role_id"].ToString();
                    Session["centertable"] = table;
                }
            }
            else
            {
                Session["centertable"] = null;
                NewValue();
            }
            
        }
        /// <summary>
        /// 置空新增
        /// </summary>
        public void NewValue()
        {
            this.Text_CenterName.Text = string.Empty;
            this.Com_Director.SelectedItem.Text = string.Empty;
            this.Combo_Role.SelectedItem.Value = string.Empty;
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.Center_Edit.hide();");
        }
    }
}