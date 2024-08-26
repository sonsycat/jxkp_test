using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    public partial class AverageBonusDaysDetail : PageBase
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
                    //初始化年月下拉列表
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

                    if (boundcomm.GetAccountType(year, month, "20001"))
                    {
                        //获得所在月份平均奖科室人的信息
                        Store1.DataSource = averagebonusdays.GetAverageBounusDaysList(year, month, this.staffname.Text, "");
                        Store1.DataBind();
                        //获得新建当月的设置的平均奖科室，设置科室下拉列表
                        Store2.DataSource = averagebonusdays.GetAverageDept(year, month, true);
                        Store2.DataBind();
                        //获得所在月份没有录入进来但是人力资源中有的平均奖科室的人
                        Store5.DataSource = averagebonusdays.GetRLZYAveragePersonList(year, month, this.staffname.Text);
                        Store5.DataBind();
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

                        //Store1.DataSource = averagebonusdays.BuildAverageBonusDaysList();
                        //Store1.DataBind();
                        Store2.DataSource = averagebonusdays.BuildAverageDept();
                        Store2.DataBind();
                        Store5.DataSource = averagebonusdays.BuildRLZY();
                        Store5.DataBind();
                    }
                }
                else
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
                        Store1.DataSource = averagebonusdays.GetAverageBounusDaysList(year, month, "", "");
                        Store1.DataBind();
                        //获得新建当月的设置的平均奖科室
                        Store2.DataSource = averagebonusdays.GetAverageDept(year, month, true);
                        Store2.DataBind();

                        //获得所在月份没有录入进来但是人力资源中有的平均奖科室的人
                        Store5.DataSource = averagebonusdays.GetRLZYAveragePersonList(year, month, "");
                        Store5.DataBind();
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

                        Store2.DataSource = averagebonusdays.BuildAverageDept();
                        Store2.DataBind();
                        Store5.DataSource = averagebonusdays.BuildRLZY();
                        Store5.DataBind();
                    }
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
        /// 数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetPageData();
        }

        /// <summary>
        /// 获取数据并绑定处理
        /// </summary>
        private void GetPageData()
        {
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            string staffname = this.staffname.Text;
            BoundComm boundcomm = new BoundComm();
            AverageBonusDays averagebonusdays = new AverageBonusDays();
            if (boundcomm.GetAccountType(year, month, "20001"))
            {
                //获得所在月份平均奖科室人的信息
                Store1.DataSource = averagebonusdays.GetAverageBounusDaysList(year, month, staffname, this.cbbdept.SelectedItem.Value);
                Store1.DataBind();
                //获得人力资源中平均奖的科室人员
                Store2.DataSource = averagebonusdays.GetAverageDept(year, month, true);
                Store2.DataBind();
                //获得所在月份没有录入进来但是人力资源中有的平均奖科室的人
                Store5.DataSource = averagebonusdays.GetRLZYAveragePersonList(year, month, staffname);
                Store5.DataBind();
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
                Store5.DataSource = averagebonusdays.BuildRLZY();
                Store5.DataBind();
            }
        }

        /// <summary>
        /// 保存平均奖科室的人1、先删除人，2根据页面添加人
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
                    averageBonusDays.SaveAverageBonusDays(selectRow, cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString(), user, cbbdept.SelectedItem.Value);
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "设置成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    GetPageData();
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveAverageBonusDaysDetail");
                }
            }
        }

        /// <summary>
        /// 同步人员系数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Attendance_Click(object sender, AjaxEventArgs e)
        {
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            BoundComm boundcomm = new BoundComm();
            AverageBonusDays averageBonusDays = new AverageBonusDays();
            if (boundcomm.GetAccountType(year, month, "20001"))
            {
                try
                {
                    averageBonusDays.EditAverageBonusDays(year, month);
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "人员系数同步成功！",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    GetPageData();
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteAverageBonusDaysDetail");
                }
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

                GetPageData();
            }
        }

        /// <summary>
        /// 删除一个平均奖科室的人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Delete_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                try
                {
                    AverageBonusDays averageBonusDays = new AverageBonusDays();
                    averageBonusDays.DeleteAverageBonusDays(selectRow);
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "删除成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    GetPageData();
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteAverageBonusDaysDetail");
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
        /// 增加人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Add_Click(object sender, AjaxEventArgs e)
        {
            string months = this.cbbmonth.SelectedItem.Text.Length == 1 ? "0" + this.cbbmonth.SelectedItem.Text : this.cbbmonth.SelectedItem.Text;
            LoadConfig loadcfg = getLoadConfig("AverageBonusDaysAddPerson.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonYear", this.cbbYear.SelectedItem.Text));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonMonth", months));
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
    }
}
