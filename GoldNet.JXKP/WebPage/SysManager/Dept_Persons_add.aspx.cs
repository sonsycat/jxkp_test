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
    public partial class Dept_Persons_add : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                pro.Url = "../../WebService/AccountDepts.ashx";
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
                //
                HttpProxy pro2 = new HttpProxy();
                pro2.Method = HttpMethod.POST;
                pro2.Url = "WebService/HisStaffs.ashx";
                this.Store1.Proxy.Add(pro2);
                //SetDict();
                //Edit();
            }
        }
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            if (this.Com_Director.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "人员姓名不能为空！");
            }
            else if (this.ComAccountdeptcode.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "科室不能为空！");
            }
            else if (dal.ExitPersons(this.Com_Director.SelectedItem.Value,this.Com_Director.SelectedItem.Text))
            {
                this.ShowMessage("系统提示", this.Com_Director.SelectedItem.Text+"已经添加了！");
            }
            else
            {
                try
                {
                    dal.AddPersons(this.ComAccountdeptcode.SelectedItem.Value, this.Com_Director.SelectedItem.Value,this.Com_Director.SelectedItem.Text);
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
