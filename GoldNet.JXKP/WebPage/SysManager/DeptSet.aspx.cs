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
    public partial class DeptSet : PageBase
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
                if (Request["deptcode"] != null)
                {
                    GoldNet.Model.SYS_DEPT_DICT model = new GoldNet.Model.SYS_DEPT_DICT(Request["deptcode"].ToString());
                    SetDept(model);
                }
            }
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        /// <param name="dept"></param>
        public void SetDept(GoldNet.Model.SYS_DEPT_DICT dept)
        {
           
            this.TextDeptcode.Text = string.Empty;
            this.TextDeptname.Text = string.Empty;
            this.Combo_DeptType.SelectedItem.Value = string.Empty;
            this.ComPdeptcode.SelectedItem.Value = string.Empty;
            this.ComAccountdeptcode.SelectedItem.Value = string.Empty;
            this.ComDeptcodesecond.SelectedItem.Value = string.Empty;
            this.ComLcattr.SelectedItem.Value = string.Empty;
            this.ComIsaccount.SelectedItem.Value = string.Empty;
            this.NumSortid.Text = string.Empty;
            this.ComShowflag.SelectedItem.Value = string.Empty;

            this.TextDeptcode.Text = dept.DEPT_CODE;
            this.TextDeptname.Text = dept.DEPT_NAME;
            this.Combo_DeptType.SelectedItem.Value = dept.DEPT_TYPE;
            this.ComPdeptcode.SelectedItem.Value = dept.P_DEPT_Name;
            this.ComAccountdeptcode.SelectedItem.Value = dept.ACCOUNT_DEPT_NAME;
            this.ComDeptcodesecond.SelectedItem.Value = dept.DEPT_NAME_SECOND;
            this.ComLcattr.SelectedItem.Value = dept.DEPT_LCATTR;
            this.ComIsaccount.SelectedItem.Text = dept.ATTR;
            this.NumSortid.Text = dept.SORT_NO;
            this.ComShowflag.SelectedItem.Value = dept.SHOW_FLAG;
            this.ComboBoxGroup.SelectedItem.Value = dept.DEPT_GROUP;
            if (dept.DEPT_TYPE.Equals("0"))
            {
                this.ComLcattr.Disabled = false;
            }
            else
            {
                this.ComLcattr.Disabled = true;
                this.ComLcattr.SelectedItem.Text="";
                this.ComLcattr.SelectedItem.Value = "";
                
            }

        }
        /// <summary>
        /// 保存科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            if (Combo_DeptType.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示","科室类别不能为空！");
            }
            else if (ComAccountdeptcode.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "核算科室不能为空！");
            }
            else if (ComDeptcodesecond.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "二级科室不能为空！");
            }
            else if (ComIsaccount.SelectedItem.Text.Equals("是") && TextDeptname.Text.Trim() != ComAccountdeptcode.SelectedItem.Text.Trim())
            {
                this.ShowMessage("系统提示", "'是否核算'选择了“是”,'核算科室'必须选择本科室！");
            }
            else if (this.ComLcattr.SelectedItem.Value.Equals("") && Combo_DeptType.SelectedItem.Value.Equals("0"))
            {
                this.ShowMessage("系统提示", "临床科室的临床属性不能为空！");
            }
            else
            {
                GoldNet.Model.SYS_DEPT_DICT model = new SYS_DEPT_DICT(this.TextDeptcode.Text);
                model.DEPT_TYPE = this.Combo_DeptType.SelectedItem.Value;
                model.DEPT_LCATTR = this.ComLcattr.SelectedItem.Value;
                model.ATTR = this.ComIsaccount.SelectedItem.Text;
                model.SHOW_FLAG = this.ComShowflag.SelectedItem.Value;
                model.SORT_NO = this.NumSortid.Text;
                model.OUT_OR_IN = this.out_or_in.SelectedItem.Value;
                model.DEPT_GROUP = "0";
                if (this.ComPdeptcode.SelectedItem.Value != this.ComPdeptcode.SelectedItem.Text | this.ComPdeptcode.SelectedItem.Value == string.Empty)
                {
                    model.P_DEPT_CODE = this.ComPdeptcode.SelectedItem.Value;
                    model.P_DEPT_Name = this.ComPdeptcode.SelectedItem.Text;
                }
                if (this.ComAccountdeptcode.SelectedItem.Value != this.ComAccountdeptcode.SelectedItem.Text | this.ComAccountdeptcode.SelectedItem.Value == string.Empty)
                {
                    model.ACCOUNT_DEPT_CODE = this.ComAccountdeptcode.SelectedItem.Value;
                    model.ACCOUNT_DEPT_NAME = this.ComAccountdeptcode.SelectedItem.Text;
                }
                if (this.ComDeptcodesecond.SelectedItem.Value != this.ComDeptcodesecond.SelectedItem.Text | this.ComDeptcodesecond.SelectedItem.Value == string.Empty)
                {
                    model.DEPT_CODE_SECOND = this.ComDeptcodesecond.SelectedItem.Value;
                    model.DEPT_NAME_SECOND = this.ComDeptcodesecond.SelectedItem.Text;
                }
                if (this.ComboBoxGroup.SelectedItem.Value != "")
                {
                    model.DEPT_GROUP = this.ComboBoxGroup.SelectedItem.Value;
                }
                Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
                try
                {
                    if (model.ID.Equals(string.Empty))
                    {
                        dal.Add(model);
                    }
                    else
                    {
                        dal.Update(model);
                    }
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
            DataTable typestable = dal.GetDeptgroupType().Tables[0];
            if (typestable.Rows.Count > 0)
            {
                for (int i = 0; i < typestable.Rows.Count; i++)
                {
                    this.ComboBoxGroup.Items.Add(new Goldnet.Ext.Web.ListItem(typestable.Rows[i]["DEPT_GROUP"].ToString(), typestable.Rows[i]["id"].ToString()));

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