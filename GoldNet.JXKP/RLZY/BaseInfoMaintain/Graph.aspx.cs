using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class Graph : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }
            if (!Ext.IsAjaxRequest)
            {
                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                string deptCode = this.DeptFilter("");
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + deptCode;
                this.Store3.Proxy.Add(proxy);

                DeptPower = deptCode;
                //等级字典信息
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
                StaffSort = sort.TrimEnd(new char[] { ',' });
            }
        }


        public static string _DeptPower;
        public static string DeptPower
        {
            set { _DeptPower = value; }
            get { return _DeptPower; }
        }

        public static string _StaffSort;
        public static string StaffSort
        {
            set { _StaffSort = value; }
            get { return _StaffSort; }
        }

    }
}
