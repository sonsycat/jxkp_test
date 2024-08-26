using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Model;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class DeptBonusList : PageBase
    {
        public static DataTable table = new DataTable();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                data(DateTime.Now.ToString("yyyyMM"), "");
            }
        }

        /// <summary>
        /// 获取奖金列表数据
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="othertype"></param>
        protected void data(string datetime, string othertype)
        {
            string conditin = this.DeptFilter("DEPT_CODE");
            InputOtherAward inputOtherAward = new InputOtherAward();
            table = inputOtherAward.GetDeptBonusList(conditin, datetime, othertype);
            Store1.DataSource = table;
            Store1.DataBind();
            this.summoney.Text = "合计：" + table.Compute("Sum(MONEY)", "").ToString();
        }

        /// <summary>
        /// 编辑按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                this.DetailWin.Height = new Unit("300");
                LoadConfig loadcfg = getLoadConfig("DeptBonusEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("DeptBonusID", selectRow[0]["ID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("DeptBonusMode", "Edit"));
                showDetailWin(loadcfg);
            }
        }

        /// <summary>
        /// 查看处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Look_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                this.DetailWin.Height = new Unit("300");
                LoadConfig loadcfg = getLoadConfig("DeptBonusEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("DeptBonusID", selectRow[0]["ID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("DeptBonusMode", "Look"));
                showDetailWin(loadcfg);
            }

        }

        /// <summary>
        /// 添加按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            this.DetailWin.Height = new Unit("300");
            LoadConfig loadcfg = getLoadConfig("DeptBonusEdit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("DeptBonusMode", "Add"));
            showDetailWin(loadcfg);
        }

        /// <summary>
        /// 添加按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string id = selectRow[0]["ID"].ToString();
                InputOtherAward inputOtherAward = new InputOtherAward();
                try
                {
                    inputOtherAward.DeleteDeptBonus(id);
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

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonsearche(object sender, AjaxEventArgs e)
        {
            string conditin = this.DeptFilter("DEPT_CODE");
            InputOtherAward inputOtherAward = new InputOtherAward();
            table = inputOtherAward.GetDeptBonusList(conditin, GetBeginDate(), "");

            System.Data.DataView dv = table.DefaultView;
            if (this.txt_SearchTxt.Text != "")
            {
                dv.RowFilter = string.Format("INPUTDATE like '{0}%' or DEPTNAME like '{0}%' or REMARK like '{0}%'", this.txt_SearchTxt.Text);
            }

            Store1.DataSource = dv;
            Store1.DataBind();
            this.summoney.Text = "合计：" + table.Compute("Sum(MONEY)", string.Format("INPUTDATE like '{0}%' or DEPTNAME like '{0}%' or REMARK like '{0}%'", this.txt_SearchTxt.Text)).ToString();
        }

        /// <summary>
        /// 添加按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Ref_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate(), "");
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            data(GetBeginDate(), "");
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Search_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate(), "");
        }

        /// <summary>
        /// 保存处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Save_Click(object sender, AjaxEventArgs e)
        {
            InputOtherAward inputOtherAward = new InputOtherAward();
            User user = (User)Session["CURRENTSTAFF"];
            try
            {
                inputOtherAward.SaveDeptBonus(GetBeginDate(), table, user.UserName, "");
                data(GetBeginDate(), "");
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
        /// 获取选择时间
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
