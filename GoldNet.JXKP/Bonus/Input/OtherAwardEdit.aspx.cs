using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    /// <summary>
    /// 其他奖项录入编辑界面
    /// </summary>
    public partial class OtherAwardEdit :PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                
                if (Request["OtherAwardMode"] != null)
                {

                    string mode = Request["OtherAwardMode"].ToString();
                    InputOtherAward inputOtherAward = new InputOtherAward();
                    DataTable dtType = inputOtherAward.GetSimpleType();
                    Store2.DataSource = dtType;
                    Store2.DataBind();
                    //如果是编辑界面
                    if (mode == "Edit" && Request["OtherAwardID"] != null)
                    {
                        //根据ID查找其他奖励数据，并且赋值给控件
                        string id = Request["OtherAwardID"].ToString();
                        DataTable dt = inputOtherAward.GetOtherAwardSimple(id);
                        if (dt.Rows.Count > 0)
                        {
                            cbbdept.SelectedItem.Value =  dt.Rows[0]["DEPT_CODE"].ToString();
                            cbbdept.SelectedItem.Text =  dt.Rows[0]["DEPT_NAME"].ToString();
                            ccbtype.SelectedItem.Value = dt.Rows[0]["OTHER_DICT_ID"].ToString();
                            ccbtype.SelectedItem.Text = dt.Rows[0]["OTHER_DICT_NAME"].ToString();                           
                            dfInputDate.Value = dt.Rows[0]["INPUT_DATE"];
                            nfNumber.Value = Convert.ToDouble(dt.Rows[0]["MONEY"]);
                            if (dt.Rows[0]["REASON"] != null && dt.Rows[0]["REASON"].ToString() != "")
                            { taReason.Value = dt.Rows[0]["REASON"]; }
                            else
                            { taReason.Value = ""; }
                            tfInputer.Visible = false;
                            tfInputDate.Visible = false;
                            tfModifier.Visible = false;
                            tfModifyDate.Visible = false;
                        }

                    }
                    else if (mode == "Add")
                    {//新建其他奖励数据，页面控件清空
                        cbbdept.SetValue("");
                        ccbtype.SetValue("");
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
                    if (mode == "Look" && Request["OtherAwardID"] != null)
                    {
                        //查看其他奖励数据，根据ID找到数据并且赋值给控件同时控件不可用
                        string id = Request["OtherAwardID"].ToString();
                        DataTable dt = inputOtherAward.GetOtherAwardSimple(id);
                        if (dt.Rows.Count > 0)
                        {
                            cbbdept.SelectedItem.Value = dt.Rows[0]["DEPT_CODE"].ToString();
                            cbbdept.SelectedItem.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                            ccbtype.SelectedItem.Value = dt.Rows[0]["OTHER_DICT_ID"].ToString();
                            ccbtype.SelectedItem.Text = dt.Rows[0]["OTHER_DICT_NAME"].ToString();                          
                            dfInputDate.Value = dt.Rows[0]["INPUT_DATE"];
                            nfNumber.Value = Convert.ToDouble(dt.Rows[0]["MONEY"]);
                            if (dt.Rows[0]["REASON"] != null && dt.Rows[0]["REASON"].ToString() != "")
                            { taReason.Value = dt.Rows[0]["REASON"]; }
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
                            ccbtype.Disabled = true;
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
        /// 根据其他奖励的项目设置是否对原因进行填写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TypeSelect(object sender, AjaxEventArgs e)
        {
            InputOtherAward inputOtherAward = new InputOtherAward();
            string id = ccbtype.SelectedItem.Value.ToString();
            DataTable dtType = inputOtherAward.GetSimpleType(id);
            if (dtType != null && dtType.Rows.Count > 0)
            {
                if (dtType.Rows[0]["EDITABLE"].ToString() == "1")
                {
                    //taReason.Disabled = false;
                    taReason.Value = "";
                }
                else
                {
                    //taReason.Disabled = true;
                    taReason.Value = dtType.Rows[0]["TYPENAME"].ToString();
                }
               
            }
            else
            {
                taReason.Value = "";
            }
        }
        /// <summary>
        /// 保存其他奖励数据到数据库中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveEditOtherAward(object sender, AjaxEventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            if (Request["OtherAwardMode"] != null)
            {
                //获得页面中控件的值
                InputOtherAward inputOtherAward = new InputOtherAward();
                string mode = Request["OtherAwardMode"].ToString();

                string deptid = cbbdept.SelectedItem.Value.Trim().ToString();
                string deptname = cbbdept.SelectedItem.Text.Trim().ToString();
                string typeid = ccbtype.SelectedItem.Value.Trim().ToString();
                string typename = ccbtype.SelectedItem.Text.Trim().ToString();
                DateTime inputdate = dfInputDate.SelectedDate;
                double money = Convert.ToDouble(nfNumber.Text);
                string reason = taReason.Value.ToString();
                if (mode == "Add")
                {//新建其他奖励数据，插入的其他奖励表中
                    try
                    {
                        User user = (User)Session["CURRENTSTAFF"];
                        inputOtherAward.InsertOtherAward(deptname, deptid, typeid, typename,reason, money, inputdate, user.UserName);
                      //  scManager.AddScript("alert('增加成功');");
                        scManager.AddScript("parent.RefreshData();");
                        //scManager.AddScript("parent.DetailWin.hide();");
                        //scManager.AddScript("parent.DetailWin.clearContent();");

                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveInSertOtherAward");
                    }
                }
                else if (mode == "Edit")
                {//修改其他奖励数据，更新到其他奖励表中
                    string id = Request["OtherAwardID"].ToString();
                    try
                    {
                        User user = (User)Session["CURRENTSTAFF"];
                        inputOtherAward.UpdateOtherAward(id, deptname, deptid, typeid, typename, reason, money, inputdate, user.UserName);
                       // scManager.AddScript("alert('修改成功');");
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
