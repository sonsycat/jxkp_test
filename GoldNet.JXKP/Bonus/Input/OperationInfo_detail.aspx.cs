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
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;
using GoldNet.JXKP.cbhs.datagather;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class OperationInfo_detail : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //HttpProxy pro1 = new HttpProxy();
                //pro1.Method = HttpMethod.POST;
                //pro1.Url = "../WebService/ReckItems.ashx";
                //this.Store1.Proxy.Add(pro1);

                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                pro.Url = "../../../WebService/Depts.ashx";
                this.Store2.Proxy.Add(pro);

                HttpProxy pro2 = new HttpProxy();
                pro2.Method = HttpMethod.POST;
                //pro.Url = "WebService/HisUsers.ashx?deptfilter=" + this.DeptFilter("dept_code");
                pro2.Url = "../../WebPage/SysManager/WebService/HisUsers.ashx";
                this.Store3.Proxy.Add(pro2);

                SetDict();
                string id = Request.QueryString["id"].ToString();
                if (id != null && !id.Equals(""))
                {
                    Edit_Init(id);
                }
            }
        }

        public void Edit_Init(string id)
        {
            //获取指定行数据
            OperationInfo dal = new OperationInfo();
            DataTable dt = dal.GetOperationInfoById(id);
            //绑定界面初始化值
            ST_DATE.Value = dt.Rows[0]["ST_DATE"].ToString();
            cb_dept.SelectedItem.Value = dt.Rows[0]["DEPT_NAME"].ToString();
            LEVEL_J.SelectedItem.Value = dt.Rows[0]["LEVEL_J"].ToString();
            EMERGENCY.SelectedItem.Value = dt.Rows[0]["EMERGENCY"].ToString();
            OPERATOR.SelectedItem.Value = dt.Rows[0]["OPERATOR_NAME"].ToString();
            OPERATOR.SelectedItem.Text = dt.Rows[0]["OPERATOR"].ToString();
            FIRST_ASSISTANT.SelectedItem.Value = dt.Rows[0]["FIRST_ASSISTANT_N"].ToString();
            SECOND_ASSISTANT.SelectedItem.Value = dt.Rows[0]["SECOND_ASSISTANT_N"].ToString();
            ANESTHESIA_DOCTOR.SelectedItem.Value = dt.Rows[0]["ANESTHESIA_DOCTOR_N"].ToString();
            HS1.SelectedItem.Value = dt.Rows[0]["HS_NAME1"].ToString();
            HS2.SelectedItem.Value = dt.Rows[0]["HS_NAME2"].ToString();
            HS3.SelectedItem.Value = dt.Rows[0]["HS_NAME3"].ToString();
            HS4.SelectedItem.Value = dt.Rows[0]["HS_NAME4"].ToString();
            HS5.SelectedItem.Value = dt.Rows[0]["HS_NAME5"].ToString();

        }

         //初始化菜单项
        protected void SetDict()
        {
        }
        protected void Buttonsave_Click(object sender, AjaxEventArgs e)
        {
            string op = Request.QueryString["op"].ToString();
            if (op == "add")
            {
                DateTime date = Convert.ToDateTime(ST_DATE.Value);
                string st_date = date.ToString("yyyy-MM-dd");
                OperationInfo dal = new OperationInfo();
                dal.AddOperationInfo(st_date, cb_dept.SelectedItem.Text, cb_dept.SelectedItem.Value, LEVEL_J.SelectedItem.Value
                    , OPERATOR.SelectedItem.Text, OPERATOR.SelectedItem.Value, FIRST_ASSISTANT.SelectedItem.Text, FIRST_ASSISTANT.SelectedItem.Value,
                    SECOND_ASSISTANT.SelectedItem.Text, SECOND_ASSISTANT.SelectedItem.Value, ANESTHESIA_DOCTOR.SelectedItem.Text,
                    ANESTHESIA_DOCTOR.SelectedItem.Value, EMERGENCY.SelectedItem.Value, HS1.SelectedItem.Text, HS1.SelectedItem.Value, HS2.SelectedItem.Text,
                    HS2.SelectedItem.Value, HS3.SelectedItem.Text, HS3.SelectedItem.Value, HS4.SelectedItem.Text, HS4.SelectedItem.Value, HS5.SelectedItem.Text,
                    HS5.SelectedItem.Value);
            }
            else
            {
                
                    DateTime date = Convert.ToDateTime(ST_DATE.Value);
                string st_date = date.ToString("yyyy-MM-dd");
                OperationInfo dal = new OperationInfo();
                dal.EditOperationinfo(Request.QueryString["id"].ToString(),st_date, cb_dept.SelectedItem.Text, cb_dept.SelectedItem.Value, LEVEL_J.SelectedItem.Value
                    , OPERATOR.SelectedItem.Text, OPERATOR.SelectedItem.Value, FIRST_ASSISTANT.SelectedItem.Text, FIRST_ASSISTANT.SelectedItem.Value,
                    SECOND_ASSISTANT.SelectedItem.Text, SECOND_ASSISTANT.SelectedItem.Value, ANESTHESIA_DOCTOR.SelectedItem.Text,
                    ANESTHESIA_DOCTOR.SelectedItem.Value, EMERGENCY.SelectedItem.Value, HS1.SelectedItem.Text, HS1.SelectedItem.Value, HS2.SelectedItem.Text,
                    HS2.SelectedItem.Value, HS3.SelectedItem.Text, HS3.SelectedItem.Value, HS4.SelectedItem.Text, HS4.SelectedItem.Value, HS5.SelectedItem.Text,
                    HS5.SelectedItem.Value);
            }
        }
        







    }
}