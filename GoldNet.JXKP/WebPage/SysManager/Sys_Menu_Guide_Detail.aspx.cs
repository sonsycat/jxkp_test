using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using GoldNet.Model;
using GoldNet.Comm.ExportData;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Sys_Menu_Guide_Detail : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string appid = Request["appid"].ToString();
            string deptcode = Request["deptcode"].ToString();
            string guidecode = Request["guidecode"].ToString();
            string tjyf = Request["tjyf"].ToString();
            data(appid,deptcode,guidecode,tjyf);
        }
        private void data(string appid, string deptcode,string guidecode,string tjyf)
        {
            SE_ROLE deptpercent = new SE_ROLE();
            DataTable table = deptpercent.GetMenuGuideDetail(appid,deptcode,guidecode,tjyf);
            DataRow dr = table.NewRow();
            if (table.Rows.Count > 0)
            {
                if (table.Rows[0]["ACCOUNT_FLAGS"].ToString().Equals("1"))
                {
                    dr["GUIDE_NAME"] = "合计";
                    dr["GUIDE_VALUE"] = table.Compute("Sum(GUIDE_VALUE)", "");
                }
            }
            table.Rows.Add(dr);
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }
    }
}
