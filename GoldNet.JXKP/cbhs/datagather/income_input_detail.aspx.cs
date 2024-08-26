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
    public partial class income_input_add : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                HttpProxy pro1 = new HttpProxy();
                pro1.Method = HttpMethod.POST;
                pro1.Url = "../WebService/ReckItems.ashx";
                this.Store1.Proxy.Add(pro1);

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
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                this.INCOM_TYPE.Items.Add(new Goldnet.Ext.Web.ListItem(ds.Rows[i]["IMCOM_TYPE"].ToString(), ds.Rows[i]["ID"].ToString()));
            }
            ds = dal.GetAccount_Signs().Tables[0];
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                this.ACCOUNT_TYPE.Items.Add(new Goldnet.Ext.Web.ListItem(ds.Rows[i]["ACCOUNT_TYPE"].ToString(), ds.Rows[i]["ID"].ToString()));
            }
            if (Request["op"] != null && Request["op"].Equals("add"))
            {
                this.save.Text = "添加";
            }
            if (Request["op"] != null && Request["op"].Equals("edit"))
            {
                this.save.Text = "保存";
            }
            if (Request["op"] != null && Request["op"].Equals("del"))
            {
                this.save.Text = "删除";
                this.RECK_ITEM.Disabled = true;
                this.INCOMES.Disabled = true;
                this.INCOMES_CHARGES.Disabled = true;
                this.ORDERED_BY.Disabled = true;
                this.PERFORMED_BY.Disabled = true;
                this.WARD_CODE.Disabled = true;
                this.ORDER_DOCTOR.Disabled = true;
                this.INCOM_TYPE.Disabled = true;
                this.ACCOUNT_TYPE.Disabled = true;
                this.ACCOUNTING_DATE.Disabled = true;
                this.REMARKS.Disabled = true;
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

            if (this.RECK_ITEM.SelectedItem.Value == string.Empty || this.INCOMES.Value.ToString() == string.Empty || this.INCOMES_CHARGES.Value.ToString() == string.Empty || this.ORDERED_BY.SelectedItem.Value == string.Empty || this.PERFORMED_BY.SelectedItem.Value == string.Empty || this.INCOM_TYPE.SelectedItem.Value == string.Empty || this.ACCOUNT_TYPE.SelectedItem.Value == string.Empty || this.ACCOUNTING_DATE.Value.ToString() == string.Empty)
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
            //if (Convert.ToDouble(this.INCOMES.Value) < Convert.ToDouble(this.INCOMES_CHARGES.Value))
            //{
            //    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            //    {
            //        Title = "信息提示",
            //        Message = "应收金额不能小于实收金额",
            //        Buttons = MessageBox.Button.OK,
            //        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            //    });
            //    return;
            //}

            string id = Request.QueryString["row_id"].ToString();

            AppendIncome model = new AppendIncome();
            Appended_income dal = new Appended_income();
            DataTable dt = dal.Get_Income(id).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model.Ordered_by = dt.Rows[0]["ordered_by"].ToString();
                model.Performed_by = dt.Rows[0]["performed_by"].ToString();
                model.Ward_code = dt.Rows[0]["ward_code"].ToString();
            }
            model.Row_id = this.ROW_ID.Value.ToString();
            model.Reck_item = this.RECK_ITEM.SelectedItem.Value;
            model.Incomes = Convert.ToDouble(this.INCOMES.Value);
            model.Incomes_charges = Convert.ToDouble(this.INCOMES_CHARGES.Value);
            if (this.ORDERED_BY.SelectedItem.Value != this.ORDERED_BY.SelectedItem.Text | this.ORDERED_BY.SelectedItem.Value == string.Empty)
            {
                model.Ordered_by = this.ORDERED_BY.SelectedItem.Value.ToString();
            } if (this.PERFORMED_BY.SelectedItem.Value != this.PERFORMED_BY.SelectedItem.Text | this.PERFORMED_BY.SelectedItem.Value == string.Empty)
            {
                model.Performed_by = this.PERFORMED_BY.SelectedItem.Value.ToString();
            } if (this.WARD_CODE.SelectedItem.Value != this.WARD_CODE.SelectedItem.Text | this.WARD_CODE.SelectedItem.Value == string.Empty)
            {
                model.Ward_code = this.WARD_CODE.SelectedItem.Value.ToString();
            }
            model.Order_doctor = this.ORDER_DOCTOR.Value.ToString();
            model.Incom_type = this.INCOM_TYPE.SelectedItem.Value;
            model.Account_type = this.ACCOUNT_TYPE.SelectedItem.Value;
            model.Accounting_date = Convert.ToDateTime(this.ACCOUNTING_DATE.Value).ToString("yyyy-MM-dd");
            model.Remarks = this.REMARKS.Value.ToString();
            if (this.save.Text == "添加")
            {
                try
                {
                    //添加数据行
                    dal.Add_Income(model);
                    //刷新父页面
                    string year = Convert.ToDateTime(this.ACCOUNTING_DATE.Value).ToString("yyyy");
                    string month = Convert.ToDateTime(this.ACCOUNTING_DATE.Value).ToString("MM");
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
                    dal.Update_Income(model);
                    string year = Convert.ToDateTime(this.ACCOUNTING_DATE.Value).ToString("yyyy");
                    string month = Convert.ToDateTime(this.ACCOUNTING_DATE.Value).ToString("MM");
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
                    dal.Del_Income(model.Row_id);
                    string year = Convert.ToDateTime(this.ACCOUNTING_DATE.Value).ToString("yyyy");
                    string month = Convert.ToDateTime(this.ACCOUNTING_DATE.Value).ToString("MM");
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
            Appended_income dal = new Appended_income();
            DataTable dt = dal.Get_Income(id).Tables[0];
            //绑定界面初始化值
            this.ROW_ID.Value = dt.Rows[0]["ROW_ID"].ToString();
            this.RECK_ITEM.SelectedItem.Value = dt.Rows[0]["reck_item"].ToString();
            this.INCOMES.Value = Convert.ToDouble(dt.Rows[0]["incomes"]);
            this.INCOMES_CHARGES.Value = Convert.ToDouble(dt.Rows[0]["incomes_charges"]);
            this.ORDERED_BY.SelectedItem.Value = dt.Rows[0]["ordered_by_name"].ToString();
            this.PERFORMED_BY.SelectedItem.Value = dt.Rows[0]["performed_by_name"].ToString();
            this.WARD_CODE.SelectedItem.Value = dt.Rows[0]["ward_code_name"].ToString();
            this.ORDER_DOCTOR.Value = dt.Rows[0]["order_doctor"].ToString();
            this.INCOM_TYPE.Value = dt.Rows[0]["incom_type"].ToString();
            this.ACCOUNT_TYPE.Value = dt.Rows[0]["account_type"].ToString();
            this.ACCOUNTING_DATE.Value = dt.Rows[0]["accounting_date"].ToString();
        }
    }
}
