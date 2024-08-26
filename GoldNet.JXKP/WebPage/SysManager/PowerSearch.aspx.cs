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
    public partial class PowerSearch :PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                //pro.Url = "WebService/HisUsers.ashx?deptfilter=" + this.DeptFilter("dept_code");
                pro.Url = "WebService/HisUsers.ashx";
                this.Store2.Proxy.Add(pro);
                string users = this.UserFilter("staff");
            }
        }
        protected void SelectPower(object sender, AjaxEventArgs e)
        {
            string userid = this.ComboBox1.SelectedItem.Value;
            Goldnet.Dal.SYS_ROLE_DICT roledal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataTable table = GetPowerList(userid).Tables[0];
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }
        public DataSet GetPowerList(string userid)
        {
            Goldnet.Dal.SYS_ROLE_DICT roledal = new Goldnet.Dal.SYS_ROLE_DICT();
            return roledal.GetUserPower(userid);
        }
    }
}
