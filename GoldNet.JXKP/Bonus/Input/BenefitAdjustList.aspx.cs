using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP
{
    public partial class BenefitAdjustList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                BoundComm boundcomm = new BoundComm();
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.Value = year;

                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;

                //
                bool edit = this.IsEdit();
                if (!edit)
                {
                    ScriptManager1.AddScript("#{Btn_Edit}.hide();#{Btn_Del}.hide();#{Btn_Add}.hide();");
                }
                data(DateTime.Now.ToString("yyyyMM"));
                //string conditin = this.DeptFilter("DEPT_CODE");
                //InputBenefitAdjust inputbenefitadjust = new InputBenefitAdjust();
                //DataTable table = new DataTable();
                //table = inputbenefitadjust.GetBenefitAdjustList(conditin, GetBeginDate());
                //Store1.DataSource = table;
                //Store1.DataBind();
                //this.summoney.Text = "合计：" + table.Compute("Sum(MONEY)", "").ToString();
            }
        }
        protected void data(string datetime)
        {
            string conditin = this.DeptFilter("DEPT_CODE");
            InputBenefitAdjust inputbenefitadjust = new InputBenefitAdjust();
            DataTable table = new DataTable();
            table = inputbenefitadjust.GetBenefitAdjustList(conditin, datetime);
            Store1.DataSource = table;
            Store1.DataBind();
            this.summoney.Text = "合计：" + table.Compute("Sum(MONEY)", "").ToString();
        }
        //编辑按钮触发事件
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                this.DetailWin.Height = new Unit("360");
                LoadConfig loadcfg = getLoadConfig("BenefitAdjustEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("BenefitAdjustID", selectRow[0]["ID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("BenefitAdjustMode", "Edit"));
                showDetailWin(loadcfg);
            }

        }
        //编条件查询按钮触发事件
        protected void Btn_Search_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate());
        }
        //编辑按钮触发事件
        protected void Btn_Look_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                this.DetailWin.Height = new Unit("460");
                LoadConfig loadcfg = getLoadConfig("BenefitAdjustEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("BenefitAdjustID", selectRow[0]["ID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("BenefitAdjustMode", "Look"));
                showDetailWin(loadcfg);
            }

        }
        //添加按钮触发事件
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            this.DetailWin.Height = new Unit("360");
            LoadConfig loadcfg = getLoadConfig("BenefitAdjustEdit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("BenefitAdjustMode", "Add"));
            showDetailWin(loadcfg);
        }
        //添加按钮触发事件
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string id = selectRow[0]["ID"].ToString();
                InputBenefitAdjust inputbenefitadjust = new InputBenefitAdjust();
                try
                {
                    inputbenefitadjust.DeleteBenefitAdjust(id);
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "删除成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Store_RefreshData(null, null);
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteBenefitAdjust");
                }

            }
        }
        //添加按钮触发事件
        protected void Btn_Ref_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate());
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            data(GetBeginDate());

        }

        //显示详细窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        protected void select_benefit(object sender, AjaxEventArgs e)
        {
            InputBenefitAdjust inputbenefitadjust = new InputBenefitAdjust();
            DataTable table = new DataTable();
            table = inputbenefitadjust.GetBenefitAdjustListt(this.txt_SearchTxt.Text);
            Store1.DataSource = table;
            Store1.DataBind();
            this.summoney.Text = "合计：" + table.Compute("Sum(MONEY)", "").ToString();
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        /// <returns></returns>
        private string GetBeginDate()
        {
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + month;
            return benginDate;
        }

    }
}
