using System;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Model;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class DeptBonusEdit : PageBase
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
                if (Request["DeptBonusMode"] != null)
                {
                    string mode = Request["DeptBonusMode"].ToString();
                    InputOtherAward inputOtherAward = new InputOtherAward();

                    //如果是编辑界面
                    if (mode == "Edit" && Request["DeptBonusID"] != null)
                    {
                        //根据ID查找其他奖励数据，并且赋值给控件
                        string id = Request["DeptBonusID"].ToString();
                        DataTable dt = inputOtherAward.GetDeptBonus(id);
                        if (dt.Rows.Count > 0)
                        {
                            cbbdept.SelectedItem.Value = dt.Rows[0]["DEPT_CODE"].ToString();
                            cbbdept.SelectedItem.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                            dfInputDate.Value = dt.Rows[0]["BONUS_DATE"];
                            nfNumber.Value = Convert.ToDouble(dt.Rows[0]["MONEY"]);

                            if (dt.Rows[0]["REMARK"] != null && dt.Rows[0]["REMARK"].ToString() != "")
                            {
                                taReason.Value = dt.Rows[0]["REMARK"];
                            }
                            else
                            { 
                                taReason.Value = ""; 
                            }

                            tfInputer.Visible = false;
                            tfInputDate.Visible = false;
                            tfModifier.Visible = false;
                            tfModifyDate.Visible = false;
                        }
                    }
                    else if (mode == "Add")
                    {//新建其他奖励数据，页面控件清空
                        cbbdept.SetValue("");
                        dfInputDate.Value = DateTime.Now.ToShortDateString();
                        taReason.Value = "";
                        nfNumber.Value = Convert.ToDouble(0);
                        tfInputer.Value = "";
                        tfInputDate.Value = "";
                        tfModifier.Value = "";
                        tfModifyDate.Value = "";
                        tfInputer.Visible = false;
                        tfInputDate.Visible = false;
                        tfModifier.Visible = false;
                        tfModifyDate.Visible = false;
                    }

                    if (mode == "Look" && Request["DeptBonusID"] != null)
                    {
                        //查看其他奖励数据，根据ID找到数据并且赋值给控件同时控件不可用
                        string id = Request["DeptBonusID"].ToString();
                        DataTable dt = inputOtherAward.GetOtherAwardSimple(id);
                        if (dt.Rows.Count > 0)
                        {
                            cbbdept.SelectedItem.Value = dt.Rows[0]["DEPT_CODE"].ToString();
                            cbbdept.SelectedItem.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                            dfInputDate.Value = dt.Rows[0]["INPUT_DATE"];
                            nfNumber.Value = Convert.ToDouble(dt.Rows[0]["MONEY"]);
                            
                            if (dt.Rows[0]["REMARK"] != null && dt.Rows[0]["REMARK"].ToString() != "")
                            { taReason.Value = dt.Rows[0]["REMARK"]; }
                            else
                            { taReason.Value = ""; }

                            tfInputer.Value = dt.Rows[0]["INPUTER"].ToString();
                            tfInputDate.Value = dt.Rows[0]["INPUTE_DATE"].ToString();
                            if (dt.Rows[0]["MODIFIER"] != null && dt.Rows[0]["MODIFY_DATE"] != null)
                            {
                                tfModifier.Value = dt.Rows[0]["MODIFIER"].ToString();
                                tfModifyDate.Value = dt.Rows[0]["MODIFY_DATE"].ToString();
                            }
                            else
                            {
                                tfModifier.Value = "";
                                tfModifyDate.Value = "";
                            }
                            BtnSave.Visible = false;
                            cbbdept.Disabled = true;
                            dfInputDate.Disabled = true;
                            nfNumber.Disabled = true;
                            taReason.Disabled = true;
                            tfInputer.Disabled = true;
                            tfInputDate.Disabled = true;
                            tfModifier.Disabled = true;
                            tfModifyDate.Disabled = true;
                            tfInputer.Disabled = true;
                            tfInputDate.Disabled = true;
                            tfModifier.Disabled = true;
                            tfModifyDate.Disabled = true;
                        }
                    }
                    SetStore();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetStore()
        {
            //查找科室信息
            DateTime iDate = Convert.ToDateTime(dfInputDate.Value);
            HttpProxy pro = new HttpProxy();
            pro.Method = HttpMethod.POST;
            pro.Url = "../WebService/BonusDepts.ashx?dept_date=" + iDate;
            this.Store3.Proxy.Add(pro);
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
            this.Store3.Reader.Add(jr);
        }

        /// <summary>
        /// 保存其他奖励数据到数据库中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveEditOtherAward(object sender, AjaxEventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            if (Request["DeptBonusMode"] != null)
            {
                //获得页面中控件的值
                InputOtherAward inputOtherAward = new InputOtherAward();
                string mode = Request["DeptBonusMode"].ToString();

                string deptid = cbbdept.SelectedItem.Value.Trim().ToString();
                string deptname = cbbdept.SelectedItem.Text.Trim().ToString();
                DateTime inputdate = dfInputDate.SelectedDate;
                double money = Convert.ToDouble(nfNumber.Text);
                string reason = taReason.Value.ToString();
                if (mode == "Add")
                {//新建其他奖励数据，插入的其他奖励表中
                    try
                    {
                        User user = (User)Session["CURRENTSTAFF"];
                        inputOtherAward.InsertDeptBonus(deptname, deptid, reason, money, inputdate, user.UserName);
                        scManager.AddScript("parent.RefreshData();");
                        scManager.AddScript("parent.DetailWin.hide();");
                        scManager.AddScript("parent.DetailWin.clearContent();");
                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeptBonusList");
                    }
                }
                else if (mode == "Edit")
                {//修改其他奖励数据，更新到其他奖励表中
                    string id = Request["DeptBonusID"].ToString();
                    try
                    {
                        User user = (User)Session["CURRENTSTAFF"];
                        inputOtherAward.UpdateDeptBonusDetail(id, deptname, deptid, reason, money, inputdate, user.UserName);
                        scManager.AddScript("parent.RefreshData();");
                        scManager.AddScript("parent.DetailWin.hide();");
                        scManager.AddScript("parent.DetailWin.clearContent();");

                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveEditOtherAward");
                    }
                }
            }
        }

    }
}
