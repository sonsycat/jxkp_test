using System;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Model;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class WorkloadSet_manager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //    HttpProxy pro1 = new HttpProxy();
                //    pro1.Method = HttpMethod.POST;
                //    pro1.Url = "../WebService/CostItems.ashx";
                //    this.Store1.Proxy.Add(pro1);

                SetStoreProxy();
                //获取参数并绑定初值
                string class_code = Request.QueryString["class_code"].ToString();
                string TYPE_CODE = Request.QueryString["TYPE_CODE"].ToString();
                if (class_code != null && !class_code.Equals(""))
                {
                    EditInit(class_code, TYPE_CODE);
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
            //try
            //{
            IncomeItem model = new IncomeItem();
            model.Item_class = this.ITEM_CLASS.Value.ToString();
            model.Item_name = this.ITEM_NAME.Value.ToString();
            model.Input_code = this.INPUT_CODE.Value.ToString();
            model.INP_GRADE = Convert.ToDouble(this.INP_GRADE.Value);
            model.OUP_GRADE = Convert.ToDouble(this.OUP_GRADE.Value);
            model.TYPE_CODE = Convert.ToDouble(Request.QueryString["TYPE_CODE"]);
            Cbhs_dict dal = new Cbhs_dict();
            dal.UpdateleibieItem(model);

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData('工作量分配设置成功');");
            scManager.AddScript("parent.DetailWin.hide();");
            //}
            //catch
            //{
            //    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            //    {
            //        Title = "提示",
            //        Message = "工作量分配设置失败",
            //        Buttons = MessageBox.Button.OK,
            //        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            //    });
            //}
        }
        //给界面控件绑定初值
        protected void EditInit(string class_code, string TYPE_CODE)
        {
            Cbhs_dict cd = new Cbhs_dict();
            DataTable dt = cd.GetLeibieIncomeItem(class_code, TYPE_CODE).Tables[0];
            this.ITEM_NAME.Value = dt.Rows[0]["ITEM_NAME"].ToString();
            this.ITEM_CLASS.Value = dt.Rows[0]["ITEM_CLASS"].ToString();
            this.INPUT_CODE.Value = dt.Rows[0]["INPUT_CODE"].ToString();
            if (dt.Rows[0]["INP_GRADE"] != DBNull.Value && !dt.Rows[0]["OUP_GRADE"].Equals(""))
            {
                this.INP_GRADE.Value = Convert.ToDouble(dt.Rows[0]["INP_GRADE"]);
                this.OUP_GRADE.Value = Convert.ToDouble(dt.Rows[0]["OUP_GRADE"]);
            }



        }
    }
}