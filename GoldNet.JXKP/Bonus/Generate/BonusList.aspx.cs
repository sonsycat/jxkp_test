using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Model;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP
{
    public partial class BonusList : PageBase
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
                string tag = this.GetStringByQueryStr("tag");
                CalculateBonus calculateBonus = new CalculateBonus();
                Store1.DataSource = calculateBonus.GetBonusList(tag);
                Store1.DataBind();
               
                InitControl(tag);
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        private void InitControl(string tag)
        {
            bool edit = this.IsEdit();
            
            // 只有生成奖金页面有奖金管理按钮（test），其他页面这些按钮都隐藏。
            switch (tag)
            {
                case "test":
                    if (!edit)
                    {
                        ScriptManager1.AddScript("#{Btn_Add}.hide();#{Btn_Del}.hide();");
                    }
                    break;
                case "open":
                    Btn_Add.Visible = false;
                    Btn_Del.Visible = false;
                    break;
                case "check":
                    Btn_Add.Visible = false;
                    Btn_Del.Visible = false;
                    break;
                case "archived":
                    Btn_Add.Visible = false;
                    Btn_Del.Visible = false;
                    break;
            }
        }

        //编辑按钮触发事件
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
             Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string tag = this.GetStringByQueryStr("tag");
                string bonusId = selectRow[0]["ID"];
                string conditin = this.DeptFilter("");
                if (tag != "archived" || conditin=="")
                {
                    Response.Redirect("BonusShow.aspx?bonusid=" + this.EncryptTheQueryString(bonusId.ToString()) + "&tag=" + this.EncryptTheQueryString(tag) + "&pageid=" + Request.QueryString["pageid"].ToString());
                }
                else
                {
                    
                    if (conditin != "")
                    {
                        Response.Redirect("BonusShow.aspx?bonusid=" + this.EncryptTheQueryString(bonusId.ToString()) + "&tag=" + this.EncryptTheQueryString(tag) + "&pageid=" + Request.QueryString["pageid"].ToString());
                    }
                    CalculateBonus calculatebonus = new CalculateBonus();
                    string beginyear = calculatebonus.GetBonusCaption(bonusId, "BEGINYEAR");
                    string beginmonth = calculatebonus.GetBonusCaption(bonusId, "BEGINMONTH");
                    User user = (User)Session["CURRENTSTAFF"];
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
                    else if (deptType == "30001" )//奖金年月此科室是平均奖科室
                    {
                        Response.Redirect("BonusDeptList_New.aspx?tagMode=" + this.EncryptTheQueryString(tag) + "&tag=" + this.EncryptTheQueryString("0") + "&tagID=" + this.EncryptTheQueryString(bonusId) + "&pageid=" + Request.QueryString["pageid"].ToString());
                
                    }
                    else if (deptType == "20001" )//奖金年月此科室是核算科室
                    {
                        Response.Redirect("BonusDeptList_New.aspx?tagMode=" + this.EncryptTheQueryString(tag) + "&tag=" + this.EncryptTheQueryString("1") + "&tagID=" + this.EncryptTheQueryString(bonusId) + "&pageid=" + Request.QueryString["pageid"].ToString());
                    }
                }
            }
        }

        //添加按钮触发事件
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("BonusNewAdd.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Mode", "NewAdd"));
            showDetailWin(loadcfg);
        }

        //添加按钮触发事件
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string id = selectRow[0]["ID"].ToString();
                CalculateBonus calculateBonus = new CalculateBonus();
                try
                {
                    if (calculateBonus.IsBonusOpen(id, "STATECHECKED") || calculateBonus.IsBonusOpen(id, "STATEARCHIVED"))
                    {
                    	Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title ="提示",
                        Message = "此奖金不可以删除",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    }); 
                        return;
                    }
                    calculateBonus.DeleteBonusById(id);
                    Store_RefreshData(null, null);
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteBonus");
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            string tag = this.GetStringByQueryStr("tag");
            CalculateBonus calculateBonus = new CalculateBonus();
            Store1.DataSource = calculateBonus.GetBonusList(tag);
            Store1.DataBind();
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
    }
}
