using System;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Model;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class WorkloadSet_ADD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Ext.IsAjaxRequest)
            //{
            //    SetStoreProxy();
            //    //获取参数并绑定初值
            //    string class_code = Request.QueryString["class_code"].ToString();
            //    if (class_code != null && !class_code.Equals(""))
            //    {
            //        EditInit(class_code);
            //    }
            //   // SetDict();

            //}
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
        //protected void SetDict()
        //{
        //    Cbhs_dict dal = new Cbhs_dict();
        //    DataTable dt = dal.GetAccount_Type().Tables[0];
        //    Store2.DataSource = dt;
        //    Store2.DataBind();

        //    DataTable dttype = dal.GetItem_Type();
        //    Store3.DataSource = dttype;
        //    Store3.DataBind();
        //}
        protected void Buttonsave_Click(object sender, EventArgs e)
        {

            IncomeItem model = new IncomeItem();
            model.Item_class = this.ITEM_CLASS.Value.ToString();
            model.Item_name = this.ITEM_NAME.Value.ToString();
            model.Input_code = this.INPUT_CODE.Value.ToString();
            model.INP_GRADE = Convert.ToDouble(this.INP_GRADE.Value);
            model.OUP_GRADE = Convert.ToDouble(this.OUP_GRADE.Value);
            model.TYPE_CODE =  Convert.ToDouble(Request.QueryString["TYPE_CODE"]);
            Cbhs_dict dal = new Cbhs_dict();
            dal.InsertleibieItem(model);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData('工作量设置成功');");
            scManager.AddScript("parent.DetailWin.hide();");

        }
        //给界面控件绑定初值
        protected void EditInit(string class_code)
        {
            //Cbhs_dict cd = new Cbhs_dict();
            //DataTable dt = cd.GetIncomeItem(class_code).Tables[0];
            //this.ITEM_NAME.Value = dt.Rows[0]["ITEM_NAME"].ToString();
            //this.ITEM_CLASS.Value = dt.Rows[0]["ITEM_CLASS"].ToString();
            //this.INPUT_CODE.Value = dt.Rows[0]["INPUT_CODE"].ToString();
            //if (dt.Rows[0]["ORDER_DEPT_DISTRIBUT"] != DBNull.Value && !dt.Rows[0]["ORDER_DEPT_DISTRIBUT"].Equals(""))
            //{
            //    this.ORDER_DEPT_DISTRIBUT.Value = Convert.ToDouble(dt.Rows[0]["ORDER_DEPT_DISTRIBUT"]);
            //    this.PERFORM_DEPT_DISTRIBUT.Value = Convert.ToDouble(dt.Rows[0]["PERFORM_DEPT_DISTRIBUT"]);
            //    this.NURSING_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["NURSING_PERCEN"]);
            //    this.OUT_OPDEPT_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["OUT_OPDEPT_PERCEN"]);
            //    this.OUT_EXDEPT_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["OUT_EXDEPT_PERCEN"]);
            //    this.OUT_NURSING_PERCEN.Value = Convert.ToDouble(dt.Rows[0]["OUT_NURSING_PERCEN"]);
            //}



        }
    }
}