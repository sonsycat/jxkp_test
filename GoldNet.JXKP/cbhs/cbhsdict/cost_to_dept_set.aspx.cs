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


namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class cost_to_dept_set :PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    //               Response.End();
                }
                Bindlist();
                HttpProxy pro1 = new HttpProxy();
                pro1.Method = HttpMethod.POST;
                pro1.Url = "../WebService/ProgLists.ashx?flags=1";
                this.Store2.Proxy.Add(pro1);

                HttpProxy pro2 = new HttpProxy();
                pro2.Method = HttpMethod.POST;
                pro2.Url = "../../../WebService/Depts.ashx";
                this.Store3.Proxy.Add(pro2);
            }
        }
        protected void Bindlist()
        {
            Cbhs_dict sd = new Cbhs_dict();
            DataTable dt = sd.GetCost_toDeptset();
            DataRow rw = dt.NewRow();
            if (dt.Rows.Count > 0)
            {
                rw["id"] = int.Parse(dt.Rows[dt.Rows.Count - 1]["id"].ToString()) + 1;
            }
            else
                rw["id"] = "1";
            dt.Rows.Add(rw);
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //刷新store
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
        }
        //刷新
        protected void Button_refresh_click(object sender, EventArgs e)
        {
            Bindlist();
        }

        //保存
        protected void Button_Save_click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                try
                {
                    Cbhs_dict sd = new Cbhs_dict();
                    sd.SaveCostToDept(selectRow);
                    this.ShowMessage("提示","设置成功！");
                    Store_RefreshData(null,null);
                }
                catch 
                {
                    this.ShowMessage("系统提示", "数据填写不完整！");
                }
            }
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        

        [AjaxMethod]
        public void SetItem(string indexid)
        {
            LoadConfig loadcfg = getLoadConfig("cost_to_dept_costset.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("indexid", indexid));
            showCenterSet(this.itemset, loadcfg);
        }

        [AjaxMethod]
        public void SetDept(string indexid)
        {
            LoadConfig loadcfg = getLoadConfig("cost_to_dept_deptset.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("indexid", indexid));
            showCenterSet(this.deptset, loadcfg);
        }
    }
}
