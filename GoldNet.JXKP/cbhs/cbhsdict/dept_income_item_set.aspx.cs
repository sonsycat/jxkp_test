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

namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class dept_income_item_set : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                HttpProxy pro1 = new HttpProxy();
                pro1.Method = HttpMethod.POST;
                pro1.Url = "../WebService/CostItems.ashx";
                this.Store1.Proxy.Add(pro1);

                //获取参数并绑定初值
                string dept_code = Request.QueryString["dept_code"].ToString();
                string item_class = Request.QueryString["item_class"].ToString();
                if ((dept_code != null && item_class != null) && (!dept_code.Equals("") && !item_class.Equals("")))
                {
                    EditInit(dept_code,item_class);
                }

            }
        }
        protected void Buttonsave_Click(object sender,EventArgs e)
        {
            string dept_code = this.DEPT_CODE.Value.ToString();
            IncomeItem model = new IncomeItem();
            model.Item_class = this.ITEM_CLASS.Value.ToString();
            model.Item_name = this.ITEM_NAME.Value.ToString();
            model.Order_dept_distribut = Convert.ToDouble(this.ORDER_DEPT_DISTRIBUT.Value);
            model.Perform_dept_distribut = Convert.ToDouble(this.PERFORM_DEPT_DISTRIBUT.Value);
            model.Nursing_percen = Convert.ToDouble(this.NURSING_PERCEN.Value);
            model.Out_opdept_percen = Convert.ToDouble(this.OUT_OPDEPT_PERCEN.Value);
            model.Out_exdept_percen = Convert.ToDouble(this.OUT_EXDEPT_PERCEN.Value);
            model.Out_nursing_percen = Convert.ToDouble(this.OUT_NURSING_PERCEN.Value);
            model.Cooperant_prercen = Convert.ToDouble(this.COOPERANT_PERCEN.Value);
            model.Fixed_percen = Convert.ToDouble(this.FIXED_PERCEN.Value);
            model.Profit_rate = Convert.ToDouble(this.PROFIT_RATE.Value);
            if (model.Fixed_percen < 100)
            {
                model.Cost_code = this.COST_CODE.SelectedItem.Value.ToString();
            }
            else
            {
                model.Cost_code = "";
            }
            if ((model.Order_dept_distribut + model.Perform_dept_distribut + model.Nursing_percen) != 100)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "住院比例填写不正确",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                }); 
                return;
            }
            if ((model.Out_opdept_percen + model.Out_exdept_percen + model.Out_nursing_percen) != 100)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "门诊比例填写不正确",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            Cbhs_dict dal = new Cbhs_dict();
            try
            {
                dal.UpdateDeptIncomeItem(dept_code, model, "0");//0表示由科室明细界面设置的记录
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.RefreshData('科室收入项目设置成功');");
                scManager.AddScript("parent.DetailWin.hide();");
            }
            catch
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "科室收入项目设置失败",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }

        //给界面控件绑定初值
        protected void EditInit(string dept_code, string item_class)
        {

            Cbhs_dict cd = new Cbhs_dict();
            DataTable dt = cd.GetDeptIncomeItem(dept_code, item_class).Tables[0];

            this.DEPT_CODE.Value = dt.Rows[0]["DEPT_CODE"].ToString();
            this.DEPT_NAME.Value = dt.Rows[0]["DEPT_NAME"].ToString();
            this.ITEM_NAME.Value = dt.Rows[0]["ITEM_NAME"].ToString();
            this.ITEM_CLASS.Value = dt.Rows[0]["ITEM_CLASS"].ToString();
            this.ORDER_DEPT_DISTRIBUT.Value = Convert.ToDouble(dt.Rows[0]["ORDER_DEPT_DISTRIBUT"]);
            this.PERFORM_DEPT_DISTRIBUT.Value = Convert.ToDouble(dt.Rows[0]["PERFORM_DEPT_DISTRIBUT"]);
            this.NURSING_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["NURSING_PERCEN"]);
            this.OUT_OPDEPT_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["OUT_OPDEPT_PERCEN"]);
            this.OUT_EXDEPT_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["OUT_EXDEPT_PERCEN"]);
            this.OUT_NURSING_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["OUT_NURSING_PERCEN"]);
            this.COOPERANT_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["COOPERANT_PERCEN"]);
            this.FIXED_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["FIXED_PERCEN"]);
            this.COST_CODE.Value = dt.Rows[0]["COST_CODE"].ToString();
            this.PROFIT_RATE.Value = Convert.ToDouble(dt.Rows[0]["PROFIT_RATE"]);
            if (!this.FIXED_PERCEN.Value.Equals("") && Convert.ToDouble(this.FIXED_PERCEN.Value) < 100)
            {
                this.COST_CODE.Disabled = false;
                this.Label10.Hidden = false;
            }

        }
    }
}
