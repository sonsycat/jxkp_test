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
    public partial class DeptAdd : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                //pro.Url = "WebService/HisDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
                pro.Url = "WebService/HisDepts.ashx";
                this.Store2.Proxy.Add(pro);
                SetDict();
                //this.save.Visible = this.IsEdit();

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
                //if (Request["deptcode"] != null)
                //{
                //    GoldNet.Model.SYS_DEPT_DICT model = new GoldNet.Model.SYS_DEPT_DICT(Request["deptcode"].ToString());
                //    SetDept(model);
                //}
            }
        }

      
        /// <summary>
        /// 保存科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            if (TextDeptcode.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "科室代码不能为空！");
            }
            else if (TextDeptname.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "科室名称不能为空！");
            }
          
            else if (!dal.GetHisDeptcodeExit(this.TextDeptcode.Text).Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "科室代码已经存在！");
            }
            else if (!dal.GetHisDeptnameExit(this.TextDeptname.Text).Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "科室名称已经存在！");
            }
            else
            {
                GoldNet.Model.SYS_DEPT_DICT model = new SYS_DEPT_DICT();
                model.ID = "";
                model.DEPT_CODE = this.TextDeptcode.Text.Trim();
                model.DEPT_NAME = this.TextDeptname.Text.Trim();
                model.INPUT_CODE = pinyin.GetChineseSpell(this.TextDeptname.Text.Trim());
               

                try
                {
                    dal.adddept(this.TextDeptcode.Text.Trim(),this.TextDeptname.Text.Trim());
                  
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData();");
                    scManager.AddScript("parent.Dept_Set.hide();");
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "Buttonsave_Click");

                }
            }
        }
        /// <summary>
        /// 字典设置
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
            DataTable lctable = dal.GetDeptLcattr().Tables[0];
            if (lctable.Rows.Count > 0)
            {
                for (int i = 0; i < lctable.Rows.Count; i++)
                {
                    this.ComLcattr.Items.Add(new Goldnet.Ext.Web.ListItem(lctable.Rows[i]["ATTRIBUE"].ToString(), lctable.Rows[i]["id"].ToString()));

                }

            }
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.Dept_Set.hide();");
        }
        protected void SelectedDepttype(object sender, AjaxEventArgs e)
        {
            if (this.Combo_DeptType.SelectedItem.Value.Equals("0"))
            {
                this.ComLcattr.Disabled = false;
            }
            else
            {
                this.ComLcattr.Disabled = true;
                this.ComLcattr.SelectedItem.Value = string.Empty;
                this.ComLcattr.SelectedItem.Text = string.Empty;
            }
        }
    }
}