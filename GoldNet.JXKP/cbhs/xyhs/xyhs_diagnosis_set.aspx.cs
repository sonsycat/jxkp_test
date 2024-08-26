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
using System.Collections.Generic;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class xyhs_diagnosis_set : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //HttpProxy pro2 = new HttpProxy();
                //pro2.Method = HttpMethod.POST;
                //pro2.Url = "../WebService/diagnosisList.ashx";
                //this.Store2.Proxy.Add(pro2);
                Bindlist();
            }
        }
        //刷新store
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
        }
        //查询、绑定数据
        public void Bindlist()
        {
            this.GridPanel1.Dispose();
            this.Store1.Dispose();
            XyhsDetail sd = new XyhsDetail();
            DataTable dt = sd.GetDiagnosisList().Tables[0];
            DataRow rw = dt.NewRow();

            dt.Rows.Add(rw);
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //刷新
        protected void Button_refresh_click(object sender, EventArgs e)
        {
            Bindlist();
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        protected void Button_Save_click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                try
                {
                    Cbhs_dict sd = new Cbhs_dict();
                    sd.SaveDiagnosis(selectRow);
                    this.ShowMessage("提示", "设置成功！");
                    Store_RefreshData(null, null);
                }
                catch (Exception ex)
                {
                    this.ShowMessage("系统提示", "数据重复或数据库出错！");
                }
            }
        }



    }
}
