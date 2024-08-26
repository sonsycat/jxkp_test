using System;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace GoldNet.JXKP.cbhs.Report
{
    public partial class KSHSXX_SYXX : PageBase
    {
        Goldnet.Dal.Assess dal = new Goldnet.Dal.Assess();
        private static List<WorkItem> TaskThread;
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {
                SetInitState();
            }
        }

        /// <summary>
        /// 视图刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string startyear = this.Comb_StartYear.SelectedItem.Value;
            string startmonth = this.Comb_StartMonth.SelectedItem.Value;

            string endyear = this.ccbYearTo.SelectedItem.Value;
            string endmonth = this.ccbMonthTo.SelectedItem.Value;

            //DataTable dt = dal.GetDeptAssessResultTemp(incount, startyear).Tables[0];
            //this.Store1.DataSource = dt;
            //this.Store1.DataBind();
            //if (dt.Rows.Count > 0)
            //{
            //    this.Btn_Excel.Disabled = false;
            //    this.Btn_Save.Disabled = false;
            //}
        }

        /// <summary>
        /// 设置页面控件初始化状态
        /// </summary>
        protected void SetInitState()
        {
            //年份、月份下拉框
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.Comb_StartYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                this.ccbYearTo.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            for (int i = 1; i <= 12; i++)
            {
                if (i < 10)
                {
                    this.Comb_StartMonth.Items.Add(new Goldnet.Ext.Web.ListItem('0' + i.ToString(), '0' + i.ToString()));
                    this.ccbMonthTo.Items.Add(new Goldnet.Ext.Web.ListItem('0' + i.ToString(), '0' + i.ToString()));
                }
                else
                {
                    this.Comb_StartMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
                    this.ccbMonthTo.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
                }
            }
        }

        /// <summary>
        /// 下拉菜单命令执行
        /// </summary>
        /// <param name="command"></param>
        /// <param name="stationcode"></param>
        /// <param name="guidegathercode"></param>
        /// <param name="stationname"></param>
        /// <param name="deptcode"></param>
        /// <param name="deptname"></param>
        [AjaxMethod(ClientProxy = ClientProxy.Ignore)]
        public void ShowDetailWindow(string command, string deptcode, string deptname)
        {
            //command="YLZC","一类支出"
            //command="XXZC","详细支出"
            //command="BBXMSR","报表项目收入"
            //command="HSXMSR","核算项目收入"
            //command="DYKSQK","对应科室情况"
            //开始日期
            string startyear = this.Comb_StartYear.SelectedItem.Value;
            string startmonth = this.Comb_StartMonth.SelectedItem.Value;
            string searchdate = startyear + startmonth + "01";
            //结束日期
            string endyear = this.ccbYearTo.SelectedItem.Value;
            string endmonth = this.ccbMonthTo.SelectedItem.Value;
            string endsearchdate = endyear + endmonth + "01";


            if (command == "YLZC")
            {
                showDetailWin(getLoadConfig("KSHSXX_YLZC.aspx?id=" + deptcode + "&sy=" + searchdate + "&esy=" + endsearchdate), "一类支出--" + searchdate + "期间", "900");
            }
            else if (command == "XXZC")
            {
                showDetailWin(getLoadConfig("KSHSXX_XXZC.aspx?id=" + deptcode + "&sy=" + searchdate + "&esy=" + endsearchdate), "详细支出--" + searchdate + "期间", "900");
            }
            else if (command == "BBXMSR")
            {
                showDetailWin(getLoadConfig("KSHSXX_BBXMSR.aspx?id=" + deptcode + "&sy=" + searchdate + "&esy=" + endsearchdate), " 报表项目收入--" + searchdate + "期间", "900");
            }
            else if (command == "HSXMSR")
            {
                showDetailWin(getLoadConfig("KSHSXX_HSXMSR.aspx?id=" + deptcode + "&sy=" + searchdate + "&esy=" + endsearchdate), "核算项目收入--" + searchdate + "期间", "900");
            }
            else if (command == "DYKSQK")
            {
                showDetailWin(getLoadConfig("KSHSXX_DYKSQK.aspx?id=" + deptcode + "&sy=" + searchdate + "&esy=" + endsearchdate), "对应科室情况--" + searchdate + "期间", "900");
            }
        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["KSHSXX_SYXX"] != null)
            {
                DataTable table = (DataTable)Session["KSHSXX_SYXX"];
                this.outexcel(table, "收益信息");
            }
        }

        /// <summary>
        /// 查询按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_View_Click(object sender, AjaxEventArgs e)
        {
            //开始日期
            string startyear = this.Comb_StartYear.SelectedItem.Value;
            string startmonth = this.Comb_StartMonth.SelectedItem.Value;
            string searchdate = startyear + startmonth + "01";
            //结束日期
            string endyear = this.ccbYearTo.SelectedItem.Value;
            string endmonth = this.ccbMonthTo.SelectedItem.Value;
            string endsearchdate = endyear + endmonth + "01";

            string rtn = "";
            rtn = CheckMonthDate(startyear, startmonth);
            if (!rtn.Equals(""))
            {
                showMsg(rtn);
                return;
            }
            //string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            //startyear = startyear.PadLeft(4, '0');
            DataTable dt = dal.GetKSHSXX(searchdate, endsearchdate).Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.Btn_Excel.Disabled = false;
                //string flg = dal.GetAssessArchFlg(incount);
                //this.Btn_Arch.Disabled = flg.Equals("0") ? false : true;
            }
            this.Store1.DataSource = dt;
            this.Store1.DataBind();

            Session.Remove("KSHSXX_SYXX");
            Session["KSHSXX_SYXX"] = dt;
        }

        /// <summary>
        /// 显示详细窗口
        /// </summary>
        /// <param name="loadcfg"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        private void showDetailWin(LoadConfig loadcfg, string title, string width)
        {
            DetailWin.ClearContent();
            if (!title.Trim().Equals(""))
            {
                DetailWin.SetTitle(title);
            }
            if (!width.Trim().Equals(""))
            {
                DetailWin.Width = Unit.Pixel(Convert.ToInt16(width));
            }
            DetailWin.Center();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        /// <summary>
        /// 载入参数设置
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private LoadConfig getLoadConfig(string url)
        {
            LoadConfig loadcfg = new LoadConfig();
            loadcfg.Url = url;
            loadcfg.Mode = LoadMode.IFrame;
            loadcfg.MaskMsg = "载入中...";
            loadcfg.ShowMask = true;
            loadcfg.NoCache = true;
            return loadcfg;
        }

        /// <summary>
        /// 检查开始结束月份
        /// </summary>
        /// <param name="year1"></param>
        /// <param name="month1"></param>
        /// <param name="year2"></param>
        /// <param name="month2"></param>
        /// <returns></returns>
        private string CheckMonthDate(string year1, string month1)
        {
            month1 = month1.PadLeft(2, '0');
            string rtn = "";
            if (year1.Equals("") || month1.Equals(""))
            {
                rtn = "请输入开始月份和结束月份!";
                return rtn;
            }

            return rtn;
        }

        /// <summary>
        /// 反序列化得到客户端提交的gridpanel数据行
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0)
            {
                return null;
            }
            else
            {
                return selectRow;
            }
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="msg"></param>
        private void showMsg(string msg)
        {
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = SystemMsg.msgtitle4,
                Message = msg,
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
        }

    }
}
