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

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class incomes_adjust_detail : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                pro.Url = "../../../WebService/Depts.ashx";
                this.Store2.Proxy.Add(pro);
                SetDict();
                string id = Request.QueryString["row_id"].ToString();
                if (id != null && !id.Equals(""))
                {
                    Edit_Init(id);
                }
            }
        }

        //初始化菜单项
        protected void SetDict()
        {
            Cbhs_dict dal = new Cbhs_dict();
            DataTable ds = dal.GetIncome_Type().Tables[0];

            if (Request["op"] != null && Request["op"].Equals("add"))
            {
                this.save.Text = "添加";
                this.INCOMES_DIFFERENCE.Disabled = true;
            }
            if (Request["op"] != null && Request["op"].Equals("edit"))
            {
                this.save.Text = "保存";
            }
            if (Request["op"] != null && Request["op"].Equals("del"))
            {
                this.save.Text = "删除";
                this.ROW_ID.Disabled = true;
                this.ST_DATE.Disabled = true;
                this.INCOMES_ADJUST.Disabled = true;
                this.INCOMES_DIFFERENCE.Disabled = true;
                this.DEPT.Disabled = true;
            }
        }

        //保存
        protected void Buttonsave_Click(object sender, EventArgs e)
        {

            
            Cbhs_dict dal_dict = new Cbhs_dict();
            AccountingData dal_acc = new AccountingData();


            string date_time = Convert.ToDateTime(this.ST_DATE.Value).ToString("yyyyMM");
            XyhsDetail dal = new XyhsDetail();
            //获取科级核算收入总数
            DataTable dt = dal.GetDeptIncomes(date_time);
            decimal dept_incmes = Convert.ToDecimal(dt.Rows[0][0]);

            //string id = Request.QueryString["id"].ToString();

            string st_date =  Convert.ToDateTime(this.ST_DATE.Value).ToString("yyyy-MM-dd");
            string Row_id = this.ROW_ID.Value.ToString();
            string incomes_adjust = INCOMES_ADJUST.Value.ToString();
            decimal incmes_d = Convert.ToDecimal(INCOMES_ADJUST.Value) - dept_incmes;
            string dept_code = DEPT.SelectedItem.Value;
            string dept_name = DEPT.SelectedItem.Text;

            
            if (this.save.Text == "添加")
            {
                try
                {
                    //添加数据行
                    dal.AddXyhsIncomesAdjust(st_date, incomes_adjust.ToString(), incmes_d.ToString(), dept_code, dept_name);
                    //刷新父页面
                    string year = Convert.ToDateTime(this.ST_DATE.Value).ToString("yyyy");
                    string month = Convert.ToDateTime(this.ST_DATE.Value).ToString("MM");
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData('添加成功','" + year + "','" + month + "');");
                    scManager.AddScript("parent.DetailWin.hide();");
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Buttonsave_Click");
                }
            }
            else if (this.save.Text == "保存")
            {
                try
                {
                    dal.UpdateXyhsIncomesAdjust(st_date, incomes_adjust.ToString(), incmes_d.ToString(), dept_code, dept_name, Row_id);
                    string year = Convert.ToDateTime(this.ST_DATE.Value).ToString("yyyy");
                    string month = Convert.ToDateTime(this.ST_DATE.Value).ToString("MM");
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData('更新成功','" + year + "','" + month + "');");
                    scManager.AddScript("parent.DetailWin.hide();");

                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Buttonsave_Click");
                }
            }
            else if (this.save.Text == "删除")
            {
                try
                {
                    //dal.Del_Income(model.Row_id);
                    string year = Convert.ToDateTime(this.ST_DATE.Value).ToString("yyyy");
                    string month = Convert.ToDateTime(this.ST_DATE.Value).ToString("MM");
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData('删除成功','" + year + "','" + month + "');");
                    scManager.AddScript("parent.DetailWin.hide();");
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Buttonsave_Click");
                }
            }

        }

        public void Edit_Init(string id)
        {
            //获取指定行数据
            XyhsDetail dal = new XyhsDetail();
            DataTable dt = dal.GetXyhsIncomesAdjustByid(id).Tables[0];
            //绑定界面初始化值
            this.ROW_ID.Value = dt.Rows[0]["ID"].ToString();
            this.ST_DATE.Value = dt.Rows[0]["ST_DATE"].ToString();
            INCOMES_ADJUST.Value = Convert.ToDouble(dt.Rows[0]["INCOMES_ADJUST"]);
            INCOMES_DIFFERENCE.Value = Convert.ToDouble(dt.Rows[0]["INCOMES_DIFFERENCE"]);
            DEPT.SelectedItem.Value = dt.Rows[0]["DEPT_CODE"].ToString();
        }




    }
}
