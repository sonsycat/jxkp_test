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
    public partial class dept_income_item : PageBase
    {
        static string item_class = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    //               Response.End();
                }
                //item_class = Request.QueryString["item_class"].ToString();
                Bindlist();

                HttpProxy pro1 = new HttpProxy();
                pro1.Method = HttpMethod.POST;
                pro1.Url = "../WebService/CostItems.ashx";
                this.Store2.Proxy.Add(pro1);

                HttpProxy pro2 = new HttpProxy();
                pro2.Method = HttpMethod.POST;
                //pro2.Url = "../WebService/BonusDepts.ashx";
                pro2.Url = "../../../WebService/Depts.ashx";
                this.Store3.Proxy.Add(pro2);

                HttpProxy pro4 = new HttpProxy();
                pro4.Method = HttpMethod.POST;
                pro4.Url = "../WebService/ReckItems.ashx";
                this.Store4.Proxy.Add(pro4);

            }
        }
        protected void Bindlist()
        {
            Cbhs_dict sd = new Cbhs_dict();

            DataTable dt = sd.GetDeptIncomeItem().Tables[0];

            //string item_code = item_class;
            //string item_name = sd.GetIncomeItemname(item_class);
            //GridPanel1.Title = "<div style='color:Red'>收入代码：" + item_code + " 收入项目：" + item_name + "</div>";
          
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
                    sd.SaveDeptIncome(selectRow);
                    this.ShowMessage("提示", "设置成功！");
                    Store_RefreshData(null, null);
                }
                catch 
                {
                    this.ShowMessage("系统提示","数据填写不完整！");
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
        protected void Button_set_click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                Ext.Msg.Alert("提示", "请选择一条记录！").Show();
            }
            else
            {
                string dept_code = sm.SelectedRow.RecordID;
                Cbhs_dict cd = new Cbhs_dict();
                DataTable dt = cd.GetDeptIncomeItem(dept_code, item_class).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    LoadConfig loadcfg = getLoadConfig("dept_income_item_set.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_code", dept_code));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("item_class", item_class));
                    showDetailWin(loadcfg);
                }
                else
                {
                    this.ShowMessage("系统提示", "新加科室在界面设置！");
                }
            }
        }
        //显示弹出窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        //载入参数设置
        //zjc private LoadConfig getLoadConfig(string url)
        private new LoadConfig getLoadConfig(string url)
        {
            LoadConfig loadcfg = new LoadConfig();
            loadcfg.Url = url;
            loadcfg.Mode = LoadMode.IFrame;
            loadcfg.MaskMsg = "载入中...";
            loadcfg.ShowMask = true;
            loadcfg.NoCache = true;
            return loadcfg;
        }
        [AjaxMethod]
        public void SetDept(string itemcode,string id)
        {
            LoadConfig loadcfg = getLoadConfig("incom_other_dept_set.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("item_code", itemcode));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("other_flags", "1"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("other_id", id));
            showCenterSet(this.DeptWin, loadcfg);
        }
    }
}
