using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    public partial class OtherAwardList : PageBase
    {
        public static DataTable table = new DataTable();
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
                    this.Edit_Type.Hidden = true;
                }
                else
                {
                    this.Edit_Type.Hidden = false;
                }
                InputOtherAward inputOtherAward = new InputOtherAward();
                DataTable dtType = inputOtherAward.GetSimpleType();
                Store2.DataSource = dtType;
                Store2.DataBind();
                data(DateTime.Now.ToString("yyyyMM"), "");
            }
        }
        protected void data(string datetime, string othertype)
        {
            string conditin = this.DeptFilter("DEPT_CODE");
            InputOtherAward inputOtherAward = new InputOtherAward();
            // DataTable table = new DataTable();
            table = inputOtherAward.GetOtherAwardList(conditin, datetime, othertype);
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
                this.DetailWin.Height = new Unit("300");
                LoadConfig loadcfg = getLoadConfig("OtherAwardEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("OtherAwardID", selectRow[0]["ID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("OtherAwardMode", "Edit"));
                showDetailWin(loadcfg);
            }

        }
        //编辑按钮触发事件
        protected void Btn_Look_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                this.DetailWin.Height = new Unit("400");
                LoadConfig loadcfg = getLoadConfig("OtherAwardEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("OtherAwardID", selectRow[0]["ID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("OtherAwardMode", "Look"));
                showDetailWin(loadcfg);
            }

        }


        //添加按钮触发事件
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            this.DetailWin.Height = new Unit("300");
            LoadConfig loadcfg = getLoadConfig("OtherAwardEdit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("OtherAwardMode", "Add"));
            showDetailWin(loadcfg);
        }
        //奖惩项目维护
        protected void Edit_Type_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("OtherAward_Type.aspx");

            showCenterSet(this.edittype, loadcfg);
        }
        //添加按钮触发事件
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string id = selectRow[0]["ID"].ToString();
                InputOtherAward inputOtherAward = new InputOtherAward();
                try
                {
                    inputOtherAward.DeleteOtherAward(id);
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
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteOtherAward");
                }

            }
        }
        protected void buttonsearche(object sender, AjaxEventArgs e)
        {

            string conditin = this.DeptFilter("DEPT_CODE");
            InputOtherAward inputOtherAward = new InputOtherAward();
            //DataTable table = new DataTable();
            table = inputOtherAward.GetOtherAwardList(conditin, GetBeginDate(), "");
            //Store1.DataSource = table;
            //Store1.DataBind();
            System.Data.DataView dv = table.DefaultView;
            if (this.txt_SearchTxt.Text != "")
            {
                dv.RowFilter = string.Format("INPUTDATE like '{0}%' or DEPTNAME like '{0}%' or REASON like '{0}%'", this.txt_SearchTxt.Text);
            }
            Store1.DataSource = dv;
            Store1.DataBind();
            this.summoney.Text = "合计：" + table.Compute("Sum(MONEY)", string.Format("INPUTDATE like '{0}%' or DEPTNAME like '{0}%' or REASON like '{0}%'", this.txt_SearchTxt.Text)).ToString();


        }
        //添加按钮触发事件
        protected void Btn_Ref_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate(), this.ccbtype.SelectedItem.Value);
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            data(GetBeginDate(), this.ccbtype.SelectedItem.Value);
        }
        protected void Btn_Search_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate(), this.ccbtype.SelectedItem.Value);
        }
        protected void Btn_Save_Click(object sender, AjaxEventArgs e)
        {
            InputOtherAward inputOtherAward = new InputOtherAward();
            User user = (User)Session["CURRENTSTAFF"];
            try
            {
                inputOtherAward.SaveOtherAward(GetBeginDate(), table, user.UserName, ccbtype.SelectedItem.Value);
                data(GetBeginDate(), this.ccbtype.SelectedItem.Value);
                this.ShowMessage("系统提示", "保存成功！");
            }
            catch (Exception)
            {
                this.ShowMessage("系统提示", "保存失败！");
            }
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
