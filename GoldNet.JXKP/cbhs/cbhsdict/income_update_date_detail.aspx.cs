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

namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class income_update_date_detail : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {

            }
        }


        //保存
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            string d_date = Convert.ToDateTime(D_DATE.Value).ToString("yyyy-MM-dd") + " " + Txt_d_hh.Text + ":" + Txt_d_mm.Text + ":" + Txt_d_ss.Text;
            string s_date = Convert.ToDateTime(S_DATE.Value).ToString("yyyy-MM-dd") + " " + Txt_S_hh.Text + ":" + Txt_S_mm.Text + ":" + Txt_S_ss.Text;
            string to_date_time = Convert.ToDateTime(TO_DATE_TIME.Value).ToString("yyyy-MM-dd");            
            Cbhs_dict del = new Cbhs_dict();

            del.InsertIncomeAdd( s_date, d_date, to_date_time);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            //scManager.AddScript("parent.RefreshData('添加成功','" + year + "','" + month + "');");
            scManager.AddScript("parent.DetailWin.hide();");

        }

    }
}