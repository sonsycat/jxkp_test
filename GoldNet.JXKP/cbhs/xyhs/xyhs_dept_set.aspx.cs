using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class xyhs_dept_set : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                pro.Url = "WebService/xyhs_dept_dicts.ashx";
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

                SetDict();
                string dept_code = Request.QueryString["dept_code"].ToString();
                EditInit(dept_code);
            }
        }
        //初始化菜单项
        protected void SetDict()
        {
            XyhsDict dal = new XyhsDict();
            DataTable dt = dal.GetDeptType().Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.DEPT_TYPE.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["XYHS_DEPT_TYPE"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
            }

        }
        //编辑初始化
        private void EditInit(string dept_code)
        {
            XyhsOperation dal = new XyhsOperation();
            DataTable dt = dal.GetDeptList(dept_code).Tables[0];

            this.DEPT_CODE.Value = string.Empty;
            this.DEPT_NAME.Value = string.Empty;
            this.DEPT_TYPE.SelectedItem.Value = string.Empty;
            this.ACCOUNT_DEPT.SelectedItem.Value = string.Empty;
            this.ATTR.SelectedItem.Value = string.Empty;
            this.SORT_NO.Value = string.Empty;
            this.INPUT_CODE.Value = string.Empty;
            this.SHOW_FLAG.SelectedItem.Value = string.Empty;

            if (dt.Rows.Count > 0)
            {
                this.DEPT_CODE.Value = dt.Rows[0]["DEPT_CODE"].ToString();
                this.DEPT_NAME.Value = dt.Rows[0]["DEPT_NAME"].ToString();
                this.DEPT_TYPE.SelectedItem.Value = dt.Rows[0]["DEPT_TYPE"].ToString();
                this.ACCOUNT_DEPT.SelectedItem.Value = dt.Rows[0]["ACCOUNT_DEPT_NAME"].ToString();
                this.ATTR.SelectedItem.Value = dt.Rows[0]["ATTR"].ToString();
                this.SORT_NO.Value = dt.Rows[0]["SORT_NO"].ToString();
                this.INPUT_CODE.Value = dt.Rows[0]["INPUT_CODE"].ToString();
                this.SHOW_FLAG.SelectedItem.Value = dt.Rows[0]["SHOW_FLAG"].ToString();
            }
        }
        //保存
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            if (this.ATTR.SelectedItem.Text.Equals("是") && DEPT_NAME.Text.Trim() != this.ACCOUNT_DEPT.SelectedItem.Text.Trim())
            {
                this.ShowMessage("系统提示", "'是否核算'选择了“是”,'核算科室'必须选择本科室！");
                return;
            }

            string dept_code = this.DEPT_CODE.Value.ToString();
            string dept_name = this.DEPT_NAME.Value.ToString();
            string dept_type = this.DEPT_TYPE.SelectedItem.Value.ToString();
            string attr = this.ATTR.SelectedItem.Value.ToString();
            string input_code = this.INPUT_CODE.Value.ToString();
            string account_dept_code = this.ACCOUNT_DEPT.SelectedItem.Value.ToString();
            XyhsDict dal = new XyhsDict();
            //DataTable dt = dal.GetDeptList(dept_code).Tables[0];
            //if (this.ACCOUNT_DEPT.SelectedItem.Value != this.ACCOUNT_DEPT.SelectedItem.Text | this.ACCOUNT_DEPT.SelectedItem.Value == string.Empty)
            //{
            //    account_dept_code = this.ACCOUNT_DEPT.SelectedItem.Value.ToString();
            //}
            //else
            //{
            //    account_dept_code = dt.Rows[0]["ACCOUNT_DEPT_CODE"].ToString();
            //}
            string account_dept_name = this.ACCOUNT_DEPT.SelectedItem.Text;
            string sort_no = this.SORT_NO.Value.ToString();
            string show_flag = this.SHOW_FLAG.SelectedItem.Value.ToString();

            try
            {
                dal.SaveDept(dept_code, dept_name, dept_type, attr, input_code, account_dept_code, account_dept_name, sort_no, show_flag);
                //刷新父界面
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.RefreshData();");
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "保存成功",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Buttonsave_Click");
            }
        }
    }
}
