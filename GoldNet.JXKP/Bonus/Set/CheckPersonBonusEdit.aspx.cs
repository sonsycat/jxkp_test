using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    public partial class CheckPersonBonusEdit : PageBase
    {
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
                return;
            }
            if (!Ext.IsAjaxRequest)
            {
                if (Request["CheckBonusYear"] != null && Request["CheckBonusMonth"] != null && Request["DeptID"] != null)
                {
                    if (this.DeptFilter("", Request.QueryString["pageid"].ToString()) != "")
                    GridPanel2.ColumnModel.Columns[5].Editor[0].ReadOnly = true;
                    //查找科室信息
                    HttpProxy pro = new HttpProxy();
                    pro.Method = HttpMethod.POST;
                    pro.Url = "../../cbhs/WebService/BonusDepts.ashx";
                    this.SDept.Proxy.Add(pro);
                    JsonReader jr = new JsonReader();
                    jr.ReaderID = "DEPT_CODE";
                    jr.Root = "Bonusdepts";
                    jr.TotalProperty = "totalCount";
                    RecordField rf = new RecordField();
                    rf.Name = "DEPT_CODE";
                    jr.Fields.Add(rf);
                    RecordField rfn = new RecordField();
                    rfn.Name = "DEPT_NAME";
                    jr.Fields.Add(rfn);
                    this.SDept.Reader.Add(jr);
                    //
                    string year = Request["CheckBonusYear"];
                    string month = Request["CheckBonusMonth"];
                    string deptid = Request["DeptID"];
                    this.cbbdept.SelectedItem.Value = deptid;
                    BoundComm boundcomm = new BoundComm();
                    CheckPersonBonus checkpersonsbonus = new CheckPersonBonus();
                    Store3.DataSource = boundcomm.getYears();
                    Store3.DataBind();
                    cbbYear.SetValue(year);
                    Store4.DataSource = boundcomm.getMonth();
                    Store4.DataBind();
                    cbbmonth.SetValue(month);
                    if (boundcomm.GetAccountType(year, month, "10001','20001"))
                    {
                        //获得所在月份平均奖科室人的信息
                        Store1.DataSource = checkpersonsbonus.GetCheckPersonBounusDaysList(year, month, deptid, this.staffname.Text);
                        Store1.DataBind();
                        //获得所在月份没有录入进来但是人力资源中有的平均奖科室的人
                        Store5.DataSource = checkpersonsbonus.GetRLZYCheckPersonBounusList(year, month, deptid, this.staffname.Text);
                        Store5.DataBind();
                    }
                    else
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "提示",
                            Message = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月核算科室还未设置",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        //获取已选择
                        Store1.DataSource = checkpersonsbonus.BuildCheckPersonBounusDaysList();
                        Store1.DataBind();
                        //获取未选择
                        Store5.DataSource = checkpersonsbonus.BuildRLZY();
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
        /// 数据刷新吹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetPageData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_dept_Click(object sender, AjaxEventArgs e)
        {
            dept();
        }

        /// <summary>
        /// 
        /// </summary>
        private void dept()
        {
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            //string deptid = Request["DeptID"];
            string deptid = this.ComboBox1.SelectedItem.Value;
            string staffname = this.staffname.Text;
            BoundComm boundcomm = new BoundComm();
            CheckPersonBonus checkpersonsbonus = new CheckPersonBonus();
            if (boundcomm.GetAccountType(year, month, "10001','20001"))
            {
                //获得所在月份平均奖科室人的信息
                //Store1.DataSource = checkpersonsbonus.GetCheckPersonBounusDaysList(year, month, deptid, staffname);
                //Store1.DataBind();
                //获得所在月份没有录入进来但是人力资源中有的平均奖科室的人
                Store5.DataSource = checkpersonsbonus.GetRLZYCheckPersonBounusList(year, month, deptid, staffname);
                Store5.DataBind();
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = year + "年" + month + "月核算科室还未设置",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });

                //Store1.DataSource = checkpersonsbonus.BuildCheckPersonBounusDaysList();
                //Store1.DataBind();
                Store5.DataSource = checkpersonsbonus.BuildRLZY();
                Store5.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetPageData()
        {

            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            //string deptid = Request["DeptID"];
            string deptid = this.cbbdept.SelectedItem.Value;
            string staffname = this.staffname.Text;
            BoundComm boundcomm = new BoundComm();
            CheckPersonBonus checkpersonsbonus = new CheckPersonBonus();
            if (boundcomm.GetAccountType(year, month, "10001','20001"))
            {
                //获得所在月份平均奖科室人的信息
                Store1.DataSource = checkpersonsbonus.GetCheckPersonBounusDaysList(year, month, deptid, staffname);
                Store1.DataBind();
                //获得所在月份没有录入进来但是人力资源中有的平均奖科室的人
                Store5.DataSource = checkpersonsbonus.GetRLZYCheckPersonBounusList(year, month,deptid,staffname);
                Store5.DataBind();
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = year + "年" + month + "月核算科室还未设置",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });

                Store1.DataSource = checkpersonsbonus.BuildCheckPersonBounusDaysList();
                Store1.DataBind();
                Store5.DataSource = checkpersonsbonus.BuildRLZY();
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
                    CheckPersonBonus checkpersonsbonus = new CheckPersonBonus();
                    checkpersonsbonus.SaveCheckPersonBonusDays(selectRow, cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString(), user, this.cbbdept.SelectedItem.Value, this.cbbdept.SelectedItem.Text);
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
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveCheckPersonBonusDaysDetail");
                }
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
                    CheckPersonBonus checkpersonsbonus = new CheckPersonBonus();
                    checkpersonsbonus.DeleteCheckPersonBonusDays(selectRow);
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
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteCheckPersonBonusDaysDetail");
                }
            }
        }

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Add_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("CheckPersonBonusSimpleEdit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonSimpleYear", Request["CheckBonusYear"].ToString()));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonSimpleMonth", Request["CheckBonusMonth"].ToString()));
            //loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonSimpleDeptID", Request["DeptID"]));
            //loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonSimpleDeptName", Request["DeptName"].ToString()));

            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonSimpleDeptID", this.cbbdept.SelectedItem.Value));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("PersonSimpleDeptName", this.cbbdept.SelectedItem.Text));
            showDetailWin(loadcfg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Back_Click(object sender, AjaxEventArgs e)
        {
            string year = Request["CheckBonusYear"];
            string month = Request["CheckBonusMonth"];
            Mask.Config msgconfig = new Mask.Config();
            msgconfig.Msg = "页面转向中...";
            msgconfig.MsgCls = "x-mask-loading";
            Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
            Goldnet.Ext.Web.Ext.Redirect("CheckPersonsEdit.aspx?CheckYear=" + year + "&CheckMonth=" + month + "&pageid=" + Request.QueryString["pageid"].ToString() + "");
        }

        //显示详细窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }
    }
}
