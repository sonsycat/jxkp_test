using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    /// <summary>
    /// 新建平均奖科室的人，根据月份找到这个月中的所有的平均奖科室并且根据科室找到平均奖科室对应的人
    /// </summary>
    public partial class AverageBonusDaysAdd : PageBase
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
                //初始化年，月
                string year = DateTime.Today.Year.ToString();
                string month = DateTime.Today.Month.ToString();
                BoundComm boundcomm = new BoundComm();
                AverageBonusDays averagebonusdays = new AverageBonusDays();
                Store3.DataSource = boundcomm.getYears();
                Store3.DataBind();
                cbbYear.SetValue(year);
                Store4.DataSource = boundcomm.getMonth();
                Store4.DataBind();
                cbbmonth.SetValue(month);

                if (boundcomm.GetAccountType(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), "20001"))
                {
                    //获得人力资源中平均奖的科室人员
                    DataTable dtPerson = averagebonusdays.GetAverageBounusPerson(year, month, "");
                    Store1.DataSource = dtPerson;
                    Store1.DataBind();
                    //获得新建当月的设置的平均奖科室
                    Store2.DataSource = averagebonusdays.GetAverageDeptAdd(year, month, true);
                    Store2.DataBind();
                }
                else
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月平均奖科室还未设置",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });

                    Store1.DataSource = averagebonusdays.BuildAverageBonusDaysList();
                    Store1.DataBind();
                    //获取科室列表并绑定
                    Store2.DataSource = averagebonusdays.BuildAverageDept();
                    Store2.DataBind();
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Refresh_Click(object sender, AjaxEventArgs e)
        {
            GetPageData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetPageData();
        }

        /// <summary>
        /// 根据年月平均奖科室取得科室的人
        /// </summary>
        private void GetPageData()
        {
            //查询年月
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();

            BoundComm boundcomm = new BoundComm();
            AverageBonusDays averagebonusdays = new AverageBonusDays();
            if (boundcomm.GetAccountType(year, month, "20001"))
            {
                DataTable dtPerson = averagebonusdays.GetAverageBounusPerson(year, month, this.cbbdept.SelectedItem.Value);
                Store1.DataSource = dtPerson;
                Store1.DataBind();
                Store2.DataSource = averagebonusdays.GetAverageDeptAdd(year, month, true);
                Store2.DataBind();
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = year + "年" + month + "月平均奖科室还未设置",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });

                Store1.DataSource = averagebonusdays.BuildAverageBonusDaysList();
                Store1.DataBind();
                Store2.DataSource = averagebonusdays.BuildAverageDept();
                Store2.DataBind();
            }
        }

        /// <summary>
        /// 保存平均奖科室的人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Save_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                try
                {
                    User user = (User)Session["CURRENTSTAFF"];
                    AverageBonusDays averageBonusDays = new AverageBonusDays();
                    averageBonusDays.SaveAverageBonusDays(selectRow, cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString(), user, this.cbbdept.SelectedItem.Value);
                    //Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    //{
                    //    Title ="提示",
                    //    Message = "添加成功",
                    //    Buttons = MessageBox.Button.OK,
                    //    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    //});
                    Mask.Config msgconfig = new Mask.Config();
                    msgconfig.Msg = "页面转向中...";
                    msgconfig.MsgCls = "x-mask-loading";
                    Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
                    Goldnet.Ext.Web.Ext.Redirect("AverageBonusDaysList.aspx");
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveAddAverageBonusDaysDetail");
                }
            }
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

            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        /// <summary>
        /// 对平均奖添加人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Add_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("AverageBonusDaysAddOnePerson.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonYear", cbbYear.SelectedItem.Value.ToString()));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonMonth", cbbmonth.SelectedItem.Value.ToString()));
            showDetailWin(loadcfg);
        }

        /// <summary>
        /// 显示详细窗口
        /// </summary>
        /// <param name="loadcfg"></param>
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        ////载入参数设置
        //private LoadConfig getLoadConfig(string url)
        //{
        //    LoadConfig loadcfg = new LoadConfig();
        //    loadcfg.Url = url;
        //    loadcfg.Mode = LoadMode.IFrame;
        //    loadcfg.MaskMsg = "载入中...";
        //    loadcfg.ShowMask = true;
        //    loadcfg.NoCache = true;
        //    return loadcfg;
        //}
    }
}
