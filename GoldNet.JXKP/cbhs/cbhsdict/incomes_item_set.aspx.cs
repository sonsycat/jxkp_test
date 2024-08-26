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
using GoldNet.Model;

namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class incomes_item_set : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                HttpProxy pro1 = new HttpProxy();
                pro1.Method = HttpMethod.POST;
                pro1.Url = "../WebService/CostItems.ashx";
                this.Store1.Proxy.Add(pro1);

                SetStoreProxy();
                //获取参数并绑定初值
                string class_code = Request.QueryString["class_code"].ToString();
                if (class_code != null && !class_code.Equals(""))
                {
                    EditInit(class_code);
                }
                SetDict();

            }
        }
        private void SetStoreProxy()
        {

            //查找科室信息
            HttpProxy pro = new HttpProxy();
            pro.Method = HttpMethod.POST;
            pro.Url = "../WebService/BonusDepts.ashx";
            this.SDept.Proxy.Add(pro);
            JsonReader jr = new JsonReader();
            jr.ReaderID = "DEPT_CODE";
            jr.Root = "Bonusdepts";
            jr.TotalProperty = "totalCount";
            RecordField rf = new RecordField();
            rf.Name = "DEPT_CODE";
            jr.Fields.Add(rf);
            RecordField rfn = new RecordField();
            rfn.Name = "DEPT_NAME";
            jr.Fields.Add(rfn);
            this.SDept.Reader.Add(jr);
        }
        //初始化界面菜单
        protected void SetDict()
        {
            Cbhs_dict dal = new Cbhs_dict();
            DataTable dt = dal.GetAccount_Type().Tables[0];
            Store2.DataSource = dt;
            Store2.DataBind();

            DataTable dttype = dal.GetItem_Type();
            Store3.DataSource = dttype;
            Store3.DataBind();
        }
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            try
            {
                IncomeItem model = new IncomeItem();
                model.Item_class = Request.QueryString["class_code"].ToString();
                model.Item_name = this.ITEM_NAME.Value.ToString();
                model.Input_code = this.INPUT_CODE.Value.ToString();
                model.Order_dept_distribut = Convert.ToDouble(this.ORDER_DEPT_DISTRIBUT.Value);
                model.Perform_dept_distribut = Convert.ToDouble(this.PERFORM_DEPT_DISTRIBUT.Value);
                model.Nursing_percen = Convert.ToDouble(this.NURSING_PERCEN.Value);
                model.Out_opdept_percen = Convert.ToDouble(this.OUT_OPDEPT_PERCEN.Value);
                model.Out_exdept_percen = Convert.ToDouble(this.OUT_EXDEPT_PERCEN.Value);
                model.Out_nursing_percen = Convert.ToDouble(this.OUT_NURSING_PERCEN.Value);
                model.Cooperant_prercen = Convert.ToDouble(this.COOPERANT_PERCEN.Value);
                model.Calculation_type = this.CALCULATION_TYPE.SelectedItem.Value.ToString();
                model.Fixed_percen = Convert.ToDouble(this.FIXED_PERCEN.Value);
                model.Profit_rate = 0;
                //model.OTHER_DEPT = cbbThirdDept.SelectedItem.Value.ToString();
                model.PERFRO_DEPT = cbbdept.SelectedItem.Value.ToString();
                //model.OTHER_PERCEN = Convert.ToDouble(OTHER_PERCEN.Value);
                //model.OUT_OTHER_DEPT = cbbThirdoutDept.SelectedItem.Value.ToString();

                //model.OUT_OTHER_PERCEN = Convert.ToDouble(OUT_OTHER_PERCEN.Value);
                model.ZJCBBL =0;
                model.JJCBBL = 0;
                model.DCCB = 0;
                model.CLASSTYPE = this.classtype.SelectedItem.Value.ToString();
                model.CLASSNAME = this.classtype.SelectedItem.Text.ToString();
                if (model.Fixed_percen < 100)
                {
                    model.Cost_code = this.COST_CODE.SelectedItem.Value.ToString();
                }
                else
                {
                    model.Cost_code = "";
                }
                Cbhs_dict dal = new Cbhs_dict();

                DataTable dt_dept = dal.getAccountType().Tables[0];
                if (this.FLAG.Value != null && !this.FLAG.Value.Equals(""))
                {
                    dal.UpdateIncomeItem(model);

                }
                else
                {
                    dal.SetIncomeItem(model);

                }

                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.RefreshData('收入项目设置成功');");
                scManager.AddScript("parent.DetailWin.hide();");
            }
            catch
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "收入项目设置失败",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }
        //给界面控件绑定初值
        protected void EditInit(string class_code)
        {
            Cbhs_dict cd = new Cbhs_dict();
            DataTable dt = cd.GetIncomeItem(class_code).Tables[0];
            this.ITEM_NAME.Value = dt.Rows[0]["ITEM_NAME"].ToString();
            //this.ITEM_CLASS.Value = Request.QueryString["class_code"].ToString();
            this.INPUT_CODE.Value = dt.Rows[0]["INPUT_CODE"].ToString();
            if (dt.Rows[0]["ORDER_DEPT_DISTRIBUT"] != DBNull.Value && !dt.Rows[0]["ORDER_DEPT_DISTRIBUT"].Equals(""))
            {
                this.ORDER_DEPT_DISTRIBUT.Value = Convert.ToDouble(dt.Rows[0]["ORDER_DEPT_DISTRIBUT"]);
                this.PERFORM_DEPT_DISTRIBUT.Value = Convert.ToDouble(dt.Rows[0]["PERFORM_DEPT_DISTRIBUT"]);
                this.NURSING_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["NURSING_PERCEN"]);
                this.OUT_OPDEPT_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["OUT_OPDEPT_PERCEN"]);
                this.OUT_EXDEPT_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["OUT_EXDEPT_PERCEN"]);
                this.OUT_NURSING_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["OUT_NURSING_PERCEN"]);
                this.COOPERANT_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["COOPERANT_PERCEN"]);
                this.FIXED_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["FIXED_PERCEN"]);
                //this.PROFIT_RATE.Value = 0;
                //this.cbbThirdDept.Value = dt.Rows[0]["OTHER_DEPT"].ToString();
                //this.cbbThirdoutDept.Value = dt.Rows[0]["OUT_OTHER_DEPT"].ToString();
                //this.OTHER_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["OTHER_PERCEN"]);
                //this.OUT_OTHER_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["OUT_OTHER_PERCEN"]);
                this.cbbdept.Value = dt.Rows[0]["PERFRO_DEPT"].ToString();
                //this.ZJCBBL.Value = 0;
                //this.JJCBBL.Value = 0;
                //this.DCCB.Value = 0;
                this.classtype.Value = dt.Rows[0]["CLASS_TYPE"].ToString();
            }
            else
            {
                //this.COOPERANT_PERCEN.Value = 100;
                //this.FIXED_PERCEN.Value = 100;
            }

            this.CALCULATION_TYPE.SelectedItem.Value = dt.Rows[0]["CALCULATION_TYPE"].ToString();
            this.COST_CODE.Value = dt.Rows[0]["COST_CODE"].ToString();
            this.FLAG.Value = dt.Rows[0]["ROW_ID"].ToString();
            if (!this.FIXED_PERCEN.Value.Equals("") && Convert.ToDouble(this.FIXED_PERCEN.Value) < 100)
            {
                this.COST_CODE.Disabled = false;
                this.Label10.Hidden = false;
            }

        }
    }
}
