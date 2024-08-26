using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Model;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class KeShi_JiJin : PageBase
    {
        /// <summary>
        /// 
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
                //
                bool edit = this.IsEdit();
                if (!edit)
                {
                    ScriptManager1.AddScript("#{Btn_Edit}.hide();#{Btn_Del}.hide();#{Btn_Add}.hide();");
                }

                data(DateTime.Now.ToString("yyyyMM"));

                
            }
        }

        //protected void isNull_Drjj(string datetime)
        //{
        //    InputSingleAward inputSingleAward = new InputSingleAward();
        //    bool result = inputSingleAward.GetIsNullDrjj(datetime);

        //    if (!result)
        //    {
        //        this.Btn_Add.Disabled = true;
        //        this.Btn_Edit.Disabled = true;

        //        RowSelectionModelListeners rowselection = new RowSelectionModelListeners();
        //        string handlerContent = rowselection.RowSelect.Handler;
        //        handlerContent = handlerContent.Replace("#{Btn_Edit}.enable()", "");
        //    }
        //    else
        //    {
        //        this.Btn_Add.Disabled = false;
        //        this.Btn_Edit.Disabled = false;
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        protected void data(string datetime)
        {
            string conditin = this.DeptFilter("DEPT_CODE");
            InputSingleAward inputSingleAward = new InputSingleAward();
            User user = (User)Session["CURRENTSTAFF"];
            DataTable table = new DataTable();
            table = inputSingleAward.GetKejijin(datetime);
            Store1.DataSource = table;
            Store1.DataBind();
            this.summoney.Text = "合计：" + table.Compute("Sum(MONEY)", "").ToString();
        }

        //编辑按钮触发事件
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            string date = cbbYear.SelectedItem.Value.ToString() + "0" + cbbmonth.SelectedItem.Value.ToString();
            InputSingleAward inputSingleAward = new InputSingleAward();
            //bool result = inputSingleAward.GetIsNullDrjj(date);

            //if (!result)
            //{
            //    this.ShowMessage("系统提示", "本月奖金已存在！");
            //}
            //else
            //{
                Dictionary<string, string>[] selectRow = GetSelectRow(e);
                if (selectRow != null)
                {
                    this.DetailWin.Height = new Unit("380");
                    LoadConfig loadcfg = getLoadConfig("KeShi_JiJinEdit.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("SingleAwardID", selectRow[0]["ID"]));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("SingleAwardMode", "Edit"));
                    showDetailWin(loadcfg);
                }
            //}
        }

        //弹窗触发事件
        protected void Btn_Window(object sender, AjaxEventArgs e)
        {
            this.ShowMessage("系统提示", "科室已存在！");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonsearche(object sender, AjaxEventArgs e)
        {
            string conditin = this.DeptFilter("DEPT_CODE");
            InputSingleAward inputSingleAward = new InputSingleAward();
            User user = (User)Session["CURRENTSTAFF"];
            DataTable table = new DataTable();
            table = inputSingleAward.GetKejijin( GetBeginDate());
            System.Data.DataView dv = table.DefaultView;
            if (this.txt_SearchTxt.Text != "")
            {
                dv.RowFilter = string.Format("AWARDDATE like '{0}%' or DEPTNAME like '{0}%' or TYPENAME like '{0}%'", this.txt_SearchTxt.Text);
            }
            Store1.DataSource = dv;
            Store1.DataBind();
            this.summoney.Text = "合计：" + table.Compute("Sum(MONEY)", string.Format("AWARDDATE like '{0}%' or DEPTNAME like '{0}%' or TYPENAME like '{0}%'", this.txt_SearchTxt.Text)).ToString();
        }

        //查看按钮触发事件
        protected void Btn_Look_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                this.DetailWin.Height = new Unit("480");
                LoadConfig loadcfg = getLoadConfig("KeShi_JiJinEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("SingleAwardID", selectRow[0]["ID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("SingleAwardMode", "Look"));
                showDetailWin(loadcfg);
            }
        }

        //添加按钮触发事件
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            string date = cbbYear.SelectedItem.Value.ToString() + "0" + cbbmonth.SelectedItem.Value.ToString();
            InputSingleAward inputSingleAward = new InputSingleAward();
            //bool result = inputSingleAward.GetIsNullDrjj(date);

            //if (!result)
            //{
            //    this.ShowMessage("系统提示", "本月奖金已存在！");
            //}
            //else
            //{
                this.DetailWin.Height = new Unit("380");
                LoadConfig loadcfg = getLoadConfig("KeShi_JiJinEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("SingleAwardMode", "Add"));
                showDetailWin(loadcfg);
            //}
        }

        //删除按钮触发事件
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string id = selectRow[0]["ID"].ToString();
                InputSingleAward inputSingleAward = new InputSingleAward();
                try
                {
                    inputSingleAward.DeleteSingleAward(id);
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
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteSingleAward");
                }
            }
        }

        //编条件查询按钮触发事件
        protected void Btn_Search_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate());
        }

        //刷新按钮触发事件
        protected void Btn_Ref_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
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