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

namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class Hospital_cost_edit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                HttpProxy pro1 = new HttpProxy();
                pro1.Method = HttpMethod.POST;
                pro1.Url = "../WebService/CostItems.ashx";
                this.Store1.Proxy.Add(pro1);

                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                //pro.Url = "../../../WebService/Depts.ashx";
                pro.Url = "../../../RLZY/WebService/DeptInfo.ashx";
                this.Store2.Proxy.Add(pro);
                SetDict();
               
                if (Request.QueryString["id"] != null&&Request.QueryString["flags"].ToString()=="edit")
                {
                    string id = Request.QueryString["id"].ToString();
                    Edit_Init(id);
                }
            }
        }

        //初始化菜单项
        protected void SetDict()
        {
            Cbhs_dict dal = new Cbhs_dict();
            DataTable prog = dal.GetProgdict("2").Tables[0];
            for (int i = 0; i < prog.Rows.Count; i++)
            {
                this.hosprogcode.Items.Add(new Goldnet.Ext.Web.ListItem(prog.Rows[i]["PROG_NAME"].ToString(), prog.Rows[i]["PROG_CODE"].ToString()));
            }
            DataTable deptprog = dal.GetProgdict("1").Tables[0];
            for (int i = 0; i < deptprog.Rows.Count; i++)
            {
                this.deptprogcode.Items.Add(new Goldnet.Ext.Web.ListItem(deptprog.Rows[i]["PROG_NAME"].ToString(), deptprog.Rows[i]["PROG_CODE"].ToString()));
            }
        }
        //保存
        protected void Buttonsave_Click(object sender, EventArgs e)
        {

            string date_time = Convert.ToDateTime(this.ACCOUNTING_DATE.Value).ToString("yyyyMM");
            Cbhs_dict dal_dict = new Cbhs_dict();
            AccountingData dal_acc = new AccountingData();
            if (dal_dict.IsBonusSave(date_time))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "该月奖金已经生成、不可以改变收入数据!",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            else
            {
                //验证核算数据是否生成
                if (dal_acc.IsAccount(date_time))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "该月奖金核算已经完成、不可以改变收入数据!",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }

            if (this.RECK_ITEM.SelectedItem.Value == string.Empty || this.COSTS.Value.ToString() == string.Empty|| this.ACCOUNTING_DATE.Value.ToString() == string.Empty)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "填写信息不完整",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            
            User user = (User)Session["CURRENTSTAFF"];
           
            Appended_income dal = new Appended_income();
            if (Request.QueryString["id"] != null && Request.QueryString["flags"].ToString() == "edit")
            {
                string id = Request.QueryString["id"].ToString();
                dal.updateHospitalcost(int.Parse(id), this.deptcode.SelectedItem.Value, this.COST_NAME.Text, Convert.ToDateTime(this.ACCOUNTING_DATE.Value).ToString("yyyy-MM-dd"), double.Parse(this.COSTS.Value.ToString()), this.RECK_ITEM.SelectedItem.Value, this.hosprogcode.SelectedItem.Value,this.deptprogcode.SelectedItem.Value, user.UserName, this.memo.Text);
            }
            else
            {
                dal.AddHospitalcost(this.deptcode.SelectedItem.Value, this.COST_NAME.Text, Convert.ToDateTime(this.ACCOUNTING_DATE.Value).ToString("yyyy-MM-dd"), double.Parse(this.COSTS.Value.ToString()), this.RECK_ITEM.SelectedItem.Value, this.hosprogcode.SelectedItem.Value, this.deptprogcode.SelectedItem.Value, user.UserName, this.memo.Text);
            }
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.DetailWin.hide();");
        }
        public void Edit_Init(string id)
        {
            //获取指定行数据
            Appended_income dal = new Appended_income();
            DataTable dt = dal.GetHospitalcostsdetail(int.Parse(id)).Tables[0];
            //绑定界面初始化值
            this.ID.Value = dt.Rows[0]["ID"].ToString();
            this.deptcode.SelectedItem.Value = dt.Rows[0]["dept_code"].ToString();
            this.RECK_ITEM.SelectedItem.Value = dt.Rows[0]["to_cost_code"].ToString();
            this.COST_NAME.Text = dt.Rows[0]["cost_name"].ToString();
            this.ACCOUNTING_DATE.Value = dt.Rows[0]["account_date"].ToString();
            this.COSTS.Value = Convert.ToDouble(dt.Rows[0]["costs"]);
            this.hosprogcode.SelectedItem.Value = dt.Rows[0]["hos_prog_code"].ToString();
            this.deptprogcode.SelectedItem.Value = dt.Rows[0]["dept_prog_code"].ToString();
            this.memo.Value = dt.Rows[0]["memo"].ToString();
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.DetailWin.hide();");

        }
    }
}
