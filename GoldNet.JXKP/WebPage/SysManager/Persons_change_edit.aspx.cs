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
    public partial class Persons_change_edit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
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
                dataset();
            }
        }
        protected void dataset()
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            if (Request["id"] != null)
            {
                DataTable table = dal.GetPersonByid(int.Parse(Request["id"].ToString())).Tables[0];
                if (table.Rows.Count > 0)
                {
                    deptcode.Text = table.Rows[0]["dept_code"].ToString();
                    this.Combo_Person.SelectedItem.Text = table.Rows[0]["user_name"].ToString();
                    this.ComAccountdeptcode.SelectedItem.Text = table.Rows[0]["dept_name"].ToString();
                    this.ComAccountdeptcode.SelectedItem.Value = table.Rows[0]["dept_name"].ToString();
                    this.st_date.Value = table.Rows[0]["st_date"].ToString();
                    this.END_Date.Value = table.Rows[0]["end_date"].ToString();
                }
            }
            else if(Request["userid"]!=null)
            {
                this.Combo_Person.SelectedItem.Text = dal.GetUserNameByUserid(Request["userid"].ToString());
                this.ComAccountdeptcode.SelectedItem.Text = "";
                this.ComAccountdeptcode.SelectedItem.Value = "";
                this.st_date.Value = "";
                this.END_Date.Value = "";
            }
        }
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
           
             if (this.ComAccountdeptcode.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "科室不能为空！");
            }
             else if (this.st_date.Value.Equals(string.Empty))
             {
                 this.ShowMessage("系统提示", "开始时间不能为空！");
             }
             else
             {
                 try
                 {
                     User user = (User)Session["CURRENTSTAFF"];
                     if (Request["userid"] != null)
                     {
                         dal.AddPersonschange(Request["userid"].ToString(), this.ComAccountdeptcode.SelectedItem.Value, this.st_date.Value.ToString(), this.END_Date.Value.ToString(), user.UserName, DateTime.Now.ToString("yyyy-MM-dd"));
                     }
                     else
                     {
                         string code = this.ComAccountdeptcode.SelectedItem.Value == this.ComAccountdeptcode.SelectedItem.Text ? this.deptcode.Text : this.ComAccountdeptcode.SelectedItem.Value;
                         dal.ModifyPersonschange(Request["id"].ToString(), code, this.st_date.Value.ToString(), this.END_Date.Value.ToString(), user.UserName, DateTime.Now.ToString("yyyy-MM-dd"));
                     }
                     this.ShowMessage("提示", "保存成功！");

                     Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                     scManager.AddScript("parent.RefreshData();");
                     scManager.AddScript("parent.add_persons.hide();");
                     scManager.AddScript("parent.add_persons.clearContent();");
                 }
                 catch (Exception ex)
                 {
                     ShowDataError(ex, Request.Url.LocalPath, "Buttonsave_Click");

                 }

             }

        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.add_persons.hide();");
        }
    }
}
