using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class StaffStateList : PageBase
    {
        BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {
                this.dtfStratDate.Value=DateTime.Now.ToString("yyyy-MM-dd");

                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + this.DeptFilter("");
                this.Store3.Proxy.Add(proxy);

                DataTable l_dt = PersTypeFilter();

                string sort = "";
                for (int i = 0; i < l_dt.Rows.Count; i++)
                {
                    sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
                }
                if (l_dt.Rows.Count == 0)
                {
                    sort = "'-1'";
                }

                this.Store1.DataSource = dal.ViewStaffStateListInfo("", DateTime.Now.ToString("yyyy-MM-dd"), this.DeptFilter(""), false, sort.TrimEnd(new char[] { ',' }));
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            if (this.dtfStratDate.SelectedValue == null)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "请选择时间",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }

            DataTable l_dt = PersTypeFilter();

            string sort = "";
            for (int i = 0; i < l_dt.Rows.Count; i++)
            {
                sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
            }
            if (l_dt.Rows.Count == 0)
            {
                sort = "'-1'";
            }


            this.Store1.DataSource = dal.ViewStaffStateListInfo(this.DeptCodeCombo.SelectedItem.Value, this.dtfStratDate.SelectedDate.ToString("yyyy-MM-dd"), this.DeptFilter(""), this.cbxInline.Checked, sort.TrimEnd(new char[] { ',' }));
            this.Store1.DataBind();
        }

        protected void InsertCase(object sender, AjaxEventArgs e)
        {
            string deptCode = e.ExtraParams["deptCode"].ToString().Replace("\"","");
            string Name = e.ExtraParams["Name"].ToString().Replace("\"", "");
            dal.InserteCase(deptCode, Name, this.dtfStratDate.SelectedDate.ToString("yyyy-MM-dd"), this.cboCaseType.SelectedItem.Text, 
                            this.txtMemo.Text, this.txtInputStaff.Text);
        }

        protected void UpdateCase(object sender, AjaxEventArgs e)
        {
            if (this.dtfSetUp.SelectedDate >= this.dtfUpDate.SelectedDate)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "销假日期不能小于请假日期",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            
            string id = e.ExtraParams["id"].ToString().Replace("\"", "");
            dal.UpdateCase("", this.dtfUpDate.SelectedDate.ToString("yyyy-MM-dd"), id);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btnSetCase.setDisabled(true);");
            scManager.AddScript("btnUpCase.setDisabled(true);");
            scManager.AddScript("winUpDate.hide();");
        }
    }
}
