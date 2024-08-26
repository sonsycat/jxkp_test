using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP
{
    /// <summary>
    ///平均奖科室人员按年月设置查看界面
    /// </summary>
    public partial class AverageBonusDaysLook : PageBase
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                if (Request["AverageYear"] != null && Request["AverageMonth"] != null)
                {
                    string year = Request["AverageYear"];
                    string month = Request["AverageMonth"];

                    BoundComm boundcomm = new BoundComm();
                    AverageBonusDays averagebonusdays = new AverageBonusDays();
                    Store3.DataSource = boundcomm.getYears();
                    Store3.DataBind();
                    cbbYear.SetValue(year);
                    Store4.DataSource = boundcomm.getMonth();
                    Store4.DataBind();
                    cbbmonth.SetValue(month);
                    //获得所在月份平均奖科室人的信息
                    Store1.DataSource = averagebonusdays.GetAverageBounusDaysList(year, month, "", "");
                    Store1.DataBind();
                    //获得人力资源中平均奖的科室人员
                    Store2.DataSource = averagebonusdays.GetAverageDept(year, month, true);
                    Store2.DataBind();
                    //获得所在月份没有录入进来但是人力资源中有的平均奖科室的人
                    Store5.DataSource = averagebonusdays.GetRLZYAveragePersonList(year, month, "");
                    Store5.DataBind();
                }
            }
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Qurey_Click(object sender, AjaxEventArgs e)
        {
            GetPageData();
        }

        /// <summary>
        /// 刷新处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Refresh_Click(object sender, AjaxEventArgs e)
        {
            GetPageData();
        }

        /// <summary>
        /// 获取数据并绑定
        /// </summary>
        private void GetPageData()
        {
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            AverageBonusDays averagebonusdays = new AverageBonusDays();
            //获得所在月份平均奖科室人的信息
            Store1.DataSource = averagebonusdays.GetAverageBounusDaysList(year, month, "", "");
            Store1.DataBind();
            //获得人力资源中平均奖的科室人员
            Store2.DataSource = averagebonusdays.GetAverageDept(year, month, true);
            Store2.DataBind();
            //获得所在月份没有录入进来但是人力资源中有的平均奖科室的人
            Store5.DataSource = averagebonusdays.GetRLZYAveragePersonList(year, month, "");
            Store5.DataBind();
        }
    }
}
