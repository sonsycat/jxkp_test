using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Comm.ExportData;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    public partial class BonusShow : PageBase
    {
        //---------------------------------------------------------
        //1、概要显示奖金界面，可以通过此界面进行奖金的公开，审核，存档
        //2、此界面在审核时才有同意和不同意的按钮出现
        //3、在此界面可以进行奖金的审核
        //4、通过此界面可以查看核算科室和平均奖科室的奖金详细信息
        //5、奖金流程：奖金生成—>奖金公开—>奖金提交审核—>奖金审批—>奖金归档—>奖金发放
        //6、通过此界面可以查看本次奖金年月中登录用户所在科室的详细信息
        //----------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //tag标示进入此界面是操作类型：test生成奖金测试，open：奖金公开,check:奖金审核,archived:奖金归档
                string tag = this.GetStringByQueryStr("tag");
                //奖金的标示ID
                string bonusid = this.GetStringByQueryStr("bonusid");
                CalculateBonus calculatebonus = new CalculateBonus();
                //根据奖金ID找到奖金的详细信息
                DataTable dtBonus = calculatebonus.GetBonusById(bonusid, tag);
                if (dtBonus != null)
                {
                    string caption = "奖金名称:" + calculatebonus.GetBonusCaption(bonusid, "BONUSNAME");
                    GridPanel2.Title = caption;
                    SBONUS.DataSource = dtBonus;
                    SBONUS.DataBind();
                }
                //审核按钮组以“同意”被选中
                //verify.Items[0].Checked = true;
                //根据奖金页面的需要隐藏控件test生成奖金测试，open：奖金公开,check:奖金审核,archived:奖金归档
                InitControl(tag);
                //设置查看平均奖或核算科室奖金详细数据的URL 1表示核算科室，0表示平均奖科室
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        private void InitControl(string tag)
        {
            CalculateBonus calculateBonus = new CalculateBonus();
            string bonusid = this.GetStringByQueryStr("bonusid");
            // 只有生成奖金页面有奖金管理按钮（test），其他页面这些按钮都隐藏。
            if (this.IsPass())
            {
                this.Btn_Exl.Visible = true;
            }
            else
                this.Btn_Exl.Visible = false;

            bool edit = this.IsEdit();
            switch (tag)
            {
                case "test"://奖金测试                  
                    //奖金是否公开
                    bool open = calculateBonus.IsBonusOpen(bonusid, "STATEOPENED");
                    if (open)
                    {//奖金如果是公开的，公开按钮隐藏
                        Btn_Public.Visible = false;

                        if (!edit)
                        {
                            ScriptManager1.AddScript("#{Btn_ReBuild}.hide();#{Btn_Del}.hide();#{Btn_UpCheck}.hide();");
                        }
                    }
                    else
                    {//如果不是公开的话，提交审核按钮隐藏
                        Btn_UpCheck.Visible = false;
                        if (!edit)
                        {
                            ScriptManager1.AddScript("#{Btn_ReBuild}.hide();#{Btn_Del}.hide();#{Btn_Public}.hide();");
                        }
                    }
                    break;
                case "open"://奖金公开是，公开奖金的按钮隐藏                   
                    break;
                case "check"://如果是审核，只显示审核按钮和查看本科奖金按钮                   
                    Btn_ReBuild.Visible = false;
                    Btn_Del.Visible = false;
                    Btn_Public.Visible = false;
                    Btn_UpCheck.Visible = false;
                    buttonok.Visible = true;
                    buttonon.Visible = true;

                    break;
                case "archived"://如果是归档，则只显示查看本科奖金按钮                   
                    Btn_ReBuild.Visible = false;
                    Btn_Del.Visible = false;
                    Btn_Public.Visible = false;
                    Btn_UpCheck.Visible = false;
                    break;
            }
        }

        /// <summary>
        /// 页面跳转回奖金列表页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Back_Clcik(object sender, AjaxEventArgs e)
        {
            string tag = this.GetStringByQueryStr("tag");
            Response.Redirect("BonusList.aspx?tag=" + this.EncryptTheQueryString(tag) + "&pageid=" + Request.QueryString["pageid"].ToString());
        }

        /// <summary>
        /// 页面跳转到查看科室奖金信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Link_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                //tag标示进入此界面是操作类型：test生成奖金测试，open：奖金公开,check:奖金审核,archived:奖金归档
                string tag = this.GetStringByQueryStr("tag");
                //奖金的标示ID
                string bonusid = this.GetStringByQueryStr("bonusid");
                string item = selectRow[0]["ITEM_NAME"];
                CalculateBonus calculatebonus = new CalculateBonus();
                string beginyear = calculatebonus.GetBonusCaption(bonusid, "BEGINYEAR");
                string beginmonth = calculatebonus.GetBonusCaption(bonusid, "BEGINMONTH");
                User user = (User)Session["CURRENTSTAFF"];
                //根据登陆的用户的所在科室以及查看奖金的年月
                string deptType = calculatebonus.CheckDeptAccountType(user.AccountDeptCode, beginyear, beginmonth);
                if (item == "核算科室奖金")
                {
                    //if ((deptType == "30001" || deptType == "40001" || deptType == "50001" || deptType == "60001" || deptType == "70001")&&)
                    Response.Redirect("BonusDeptList_New.aspx?tagMode=" + this.EncryptTheQueryString(tag) + "&tag=" + this.EncryptTheQueryString("1") + "&tagID=" + this.EncryptTheQueryString(bonusid) + "&rMode=" + this.EncryptTheQueryString("1") + "&pageid=" + Request.QueryString["pageid"].ToString());
                }
                else if (item == "平均奖科室奖金")
                {
                    Response.Redirect("BonusDeptList_New.aspx?tagMode=" + this.EncryptTheQueryString(tag) + "&tag=" + this.EncryptTheQueryString("0") + "&tagID=" + this.EncryptTheQueryString(bonusid) + "&rMode=" + this.EncryptTheQueryString("1") + "&pageid=" + Request.QueryString["pageid"].ToString());


                }
                else if (item == "奖金审核")
                {

                    string access = selectRow[0]["ITEM_VALUE"];
                    //CalculateBonus calculatebonus = new CalculateBonus();
                    if (access == "1")
                    {//同意被选中，则奖金归档
                        calculatebonus.SetBonusArchived(bonusid);
                        Btn_Back_Clcik(null, null);
                    }
                    else
                    {//不同意被选中，则奖金被退回，变为公开之前的状态
                        calculatebonus.SetBonusCheckBack(bonusid);
                        Btn_Back_Clcik(null, null);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_subok_Click(object sender, AjaxEventArgs e)
        {
            string bonusid = this.GetStringByQueryStr("bonusid");
            CalculateBonus calculatebonus = new CalculateBonus();
            calculatebonus.SetBonusArchived(bonusid);
            Btn_Back_Clcik(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_subno_Click(object sender, AjaxEventArgs e)
        {
            string bonusid = this.GetStringByQueryStr("bonusid");
            CalculateBonus calculatebonus = new CalculateBonus();
            calculatebonus.SetBonusCheckBack(bonusid);
            Btn_Back_Clcik(null, null);
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
        /// 奖金审核通过或不通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveCheck_Click(object sender, AjaxEventArgs e)
        {
            //string tag = this.GetStringByQueryStr("tag");
            //string bonusid = this.GetStringByQueryStr("bonusid");
            //bool access = false;
            ////获得单选按钮组中同意被选择还是不同意被选中
            //for (int i = 0; i < verify.Items.Count; i++)
            //{
            //    if (verify.Items[i].Checked)
            //    {
            //        if (verify.Items[i].BoxLabel == "同意")
            //        {
            //            access = true;
            //        }
            //    }
            //}
            //CalculateBonus calculatebonus = new CalculateBonus();
            //if (access)
            //{//同意被选中，则奖金归档
            //    calculatebonus.SetBonusArchived(bonusid);
            //    Btn_Back_Clcik(null, null);
            //}
            //else
            //{//不同意被选中，则奖金被退回，变为公开之前的状态
            //    calculatebonus.SetBonusCheckBack(bonusid);
            //    Btn_Back_Clcik(null, null);
            //}
        }

        /// <summary>
        /// 奖金公开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Open(object sender, AjaxEventArgs e)
        {
            string bonusid = this.GetStringByQueryStr("bonusid");
            CalculateBonus calculateBonus = new CalculateBonus();
            try
            {
                //奖金公开
                if (calculateBonus.SetBonusOpen(bonusid))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "已公开奖金",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Btn_Back_Clcik(null, null);
                }
            }
            catch (Exception ex)
            {
                ShowDataError(ex.ToString(), Request.Url.LocalPath, "BonusOpen");
            }
        }

        /// <summary>
        /// 奖金提交审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Check(object sender, AjaxEventArgs e)
        {
            string bonusid = this.GetStringByQueryStr("bonusid");
            CalculateBonus calculateBonus = new CalculateBonus();
            //奖金必须公开后才可以提交审核
            if (!calculateBonus.IsBonusOpen(bonusid, "STATEOPENED"))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "奖金还未公开，不可以提交审核",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
            }
            try
            {
                if (calculateBonus.SetBonusCheck(bonusid))
                {
                    string tag = this.GetStringByQueryStr("tag");
                    //this.Alert("已提交审核奖金", "BonusList.aspx?tag=" + this.EncryptTheQueryString(tag));
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                   {
                       Title = "提示",
                       Message = "已提交审核奖金",
                       Buttons = MessageBox.Button.OK,
                       Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                   });
                    Btn_Back_Clcik(null, null);
                }
            }
            catch (Exception ex)
            {
                ShowDataError(ex.ToString(), Request.Url.LocalPath, "BonusCheck");
            }
        }

        //科室奖金删除
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            string bonusid = this.GetStringByQueryStr("bonusid");
            CalculateBonus calculateBonus = new CalculateBonus();
            try
            {
                //如果奖金提交审核或已经归档，则不可以删除奖金
                if (calculateBonus.IsBonusOpen(bonusid, "STATECHECKED") || calculateBonus.IsBonusOpen(bonusid, "STATEARCHIVED"))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "此奖金不可以删除",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                calculateBonus.DeleteBonusById(bonusid);
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "奖金删除成功",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                string tag = this.GetStringByQueryStr("tag");
                //跳转回奖金列表页面
                Response.Redirect("BonusList.aspx?tag=" + this.EncryptTheQueryString(tag) + "&pageid=" + Request.QueryString["pageid"].ToString());

            }
            catch (Exception ex)
            {
                ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteBonus");
            }
        }

        /// <summary>
        /// 查看本科室奖金
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Look_SelfDept(object sender, AjaxEventArgs e)
        {
            string bonusid = this.GetStringByQueryStr("bonusid");
            string tag = this.GetStringByQueryStr("tag");
            CalculateBonus calculatebonus = new CalculateBonus();
            string beginyear = calculatebonus.GetBonusCaption(bonusid, "BEGINYEAR");
            string beginmonth = calculatebonus.GetBonusCaption(bonusid, "BEGINMONTH");
            User user = (User)Session["CURRENTSTAFF"];
            //根据登陆的用户的所在科室以及查看奖金的年月
            string deptType = calculatebonus.CheckDeptAccountType(user.AccountDeptCode, beginyear, beginmonth);
            if (deptType == "10001")//奖金年月科室未设置表明未参加奖金
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                   {
                       Title = "提示",
                       Message = "您所在的科室未参加奖金分配",
                       Buttons = MessageBox.Button.OK,
                       Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                   });
            }
            else if (deptType == "20001")//奖金年月此科室是平均奖科室
            {
                Response.Redirect("BonusLook/BonusAvgSelfLook.aspx?Years=" + this.EncryptTheQueryString(beginyear) + "&Months=" + this.EncryptTheQueryString(beginmonth) + "&tagMode=" + this.EncryptTheQueryString(tag) + "&tagID=" + this.EncryptTheQueryString(bonusid) + "&pageid=" + Request.QueryString["pageid"].ToString());

            }
            else if (deptType != "10001" && deptType != "20001")//奖金年月此科室是核算科室
            {
                Response.Redirect("BonusLook/BonusAccountSelfLook.aspx?Years=" + this.EncryptTheQueryString(beginyear) + "&Months=" + this.EncryptTheQueryString(beginmonth) + "&tagMode=" + this.EncryptTheQueryString(tag) + "&tagID=" + this.EncryptTheQueryString(bonusid) + "&pageid=" + Request.QueryString["pageid"].ToString());
            }
        }

        /// <summary>
        /// 添加按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            string bonusid = this.GetStringByQueryStr("bonusid");
            CalculateBonus calculatebonus = new CalculateBonus();
            string BonusName = calculatebonus.GetBonusCaption(bonusid, "BONUSNAME");
            string Years = calculatebonus.GetBonusCaption(bonusid, "BEGINYEAR");
            string Months = calculatebonus.GetBonusCaption(bonusid, "BEGINMONTH");
            LoadConfig loadcfg = getLoadConfig("BonusNewAdd.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Mode", "NewRebuild"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("BonusName", BonusName));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Years", Years));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Months", Months));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("BonusID", bonusid));
            showDetailWin(loadcfg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {

            ExportData ex = new ExportData();
            string bonusid = this.GetStringByQueryStr("bonusid");
            CalculateBonus calculateBouns = new CalculateBonus();
            DataTable table = calculateBouns.GetBonusPersons(bonusid);
            //this.outexcel(table,"奖金明细");
            ex.ExportToLocal(table, this.Page, "xls", "奖金明细");

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string bonusid = this.GetStringByQueryStr("bonusid");
            string tag = this.GetStringByQueryStr("tag");
            CalculateBonus calculatebonus = new CalculateBonus();
            //根据奖金ID找到奖金的详细信息
            DataTable dtBonus = calculatebonus.GetBonusById(bonusid, tag);
            if (dtBonus != null)
            {
                SBONUS.DataSource = dtBonus;
                SBONUS.DataBind();
            }
        }

    }
}
