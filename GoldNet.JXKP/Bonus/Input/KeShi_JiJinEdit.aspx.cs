using System;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Model;


namespace GoldNet.JXKP.Bonus.Input
{
    public partial class KeShi_JiJinEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                User user = (User)Session["CURRENTSTAFF"];
                if (Request["SingleAwardMode"] != null)
                {
                    //如果是编辑界面
                    string mode = Request["SingleAwardMode"].ToString();
                    InputSingleAward inputSingleAward = new InputSingleAward();
                    DataTable dtType = inputSingleAward.GetSimpleType(user);
                    Store2.DataSource = dtType;
                    Store2.DataBind();
                    taCheckStan.ReadOnly = true;
                    if (mode == "Edit" && Request["SingleAwardID"] != null)
                    {
                        //根据ID查找单项奖励数据，并且赋值给控件
                        string id = Request["SingleAwardID"].ToString();
                        DataTable dt = inputSingleAward.GetSingKejijin(id);
                        if (dt.Rows.Count > 0)
                        {
                            cbbdept.SelectedItem.Value = dt.Rows[0]["DEPT_CODE"].ToString();
                            cbbdept.SelectedItem.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                            ccbtype.SelectedItem.Value = dt.Rows[0]["TYPE_ID"].ToString();
                            ccbtype.SelectedItem.Text = dt.Rows[0]["TYPE_NAME"].ToString();
                            dfInputDate.Value = dt.Rows[0]["AWARD_DATE"];
                            nfNumber.Value = Convert.ToDouble(dt.Rows[0]["MONEY"]);
                            taCheckStan.Value = dt.Rows[0]["CHECKSTAN"].ToString();
                            if (dt.Rows[0]["REMARK"] != null && dt.Rows[0]["REMARK"].ToString() != "")
                            { taRemark.Value = dt.Rows[0]["REMARK"]; }
                            else
                            { taRemark.Value = ""; }
                            tfInputer.Visible = false;
                            tfInputDate.Visible = false;
                            tfModifier.Visible = false;
                            tfModifyDate.Visible = false;
                        }

                    }
                    else if (mode == "Add")
                    {//新建单项奖励数据，页面控件清空
                        cbbdept.SetValue("");
                        ccbtype.SetValue("");
                        dfInputDate.Value = DateTime.Now.ToShortDateString();
                        taCheckStan.Value = "";
                        taRemark.Value = "";
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
                    //查看单项奖惩
                    if (mode == "Look" && Request["SingleAwardID"] != null)
                    {
                        //查看单项奖励数据，根据ID找到数据并且赋值给控件同时控件不可用
                        string id = Request["SingleAwardID"].ToString();
                        DataTable dt = inputSingleAward.GetSingKejijin(id);
                        if (dt.Rows.Count > 0)
                        {
                            cbbdept.SelectedItem.Value = dt.Rows[0]["DEPT_CODE"].ToString();
                            cbbdept.SelectedItem.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                            ccbtype.SelectedItem.Value = dt.Rows[0]["TYPE_ID"].ToString();
                            ccbtype.SelectedItem.Text = dt.Rows[0]["TYPE_NAME"].ToString();
                            dfInputDate.Value = dt.Rows[0]["AWARD_DATE"];
                            nfNumber.Value = Convert.ToDouble(dt.Rows[0]["MONEY"]);
                            taCheckStan.Value = dt.Rows[0]["CHECKSTAN"].ToString();
                            if (dt.Rows[0]["REMARK"] != null && dt.Rows[0]["REMARK"].ToString() != "")
                            { taRemark.Value = dt.Rows[0]["REMARK"]; }
                            else
                            { taRemark.Value = ""; }
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
                            taCheckStan.Disabled = true;
                            taRemark.Disabled = true;
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
        /// 设置Store的属性
        /// </summary>
        private void SetStore()
        {
            DateTime iDate = Convert.ToDateTime(dfInputDate.Value);
            //查找科室信息
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
        /// 根据单项奖励的项目设置是否对原因进行填写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TypeSelect(object sender, AjaxEventArgs e)
        {
            InputSingleAward inputSingleAward = new InputSingleAward();
            string id = ccbtype.SelectedItem.Value.ToString();
            DataTable dtType = inputSingleAward.GetSimpleType_kjj(id);
            if (dtType != null && dtType.Rows.Count > 0)
            {
                taCheckStan.Value = dtType.Rows[0]["CHECKSTAN"].ToString();
            }
            else
            {
                taCheckStan.Value = "";
            }
        }
        /// <summary>
        /// 保存单项奖励数据到数据库中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveEditSingleAward(object sender, AjaxEventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            if (Request["SingleAwardMode"] != null)
            {
                //获得页面中控件的值
                InputSingleAward inputSingleAward = new InputSingleAward();
                string mode = Request["SingleAwardMode"].ToString();
                string deptid = cbbdept.SelectedItem.Value.Trim().ToString();
                string deptname = cbbdept.SelectedItem.Text.Trim().ToString();
                string typeid = ccbtype.SelectedItem.Value.Trim().ToString();
                string typename = ccbtype.SelectedItem.Text.Trim().ToString();
                DateTime inputdate = dfInputDate.SelectedDate;
                double money = Convert.ToDouble(nfNumber.Text);
                string checkstan = taCheckStan.Value.ToString();
                string remark = taRemark.Value.ToString();
                if (mode == "Add")
                {//新建单项奖励数据，插入的单项奖励表中
                    try
                    {
                       InputSingleAward inputSingleAwarda = new InputSingleAward();
                       bool result = inputSingleAwarda.GetIsNullDrjj(inputdate,deptid);

                        if (!result)
                        {
                            this.ShowMessage("系统提示", "本月奖金已存在！");
                        }
                        else if (inputSingleAward.ResultDept(inputdate, deptid)<1)
                        {
                            User user = (User)Session["CURRENTSTAFF"];
                            inputSingleAward.InsertSingleAward_KJJ(deptname, deptid, typeid, typename, checkstan, remark, money, inputdate, user.UserName);
                            //scManager.AddScript("alert('增加成功');");
                            scManager.AddScript("parent.RefreshData();");
                            scManager.AddScript("parent.DetailWin.hide();");
                            //scManager.AddScript("parent.DetailWin.clearContent();");
                        }
                        else
                        {
                            //scManager.AddScript("alert('科室已存在');");
                            scManager.AddScript("parent.Btnwindow();");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveInSertSingleAward");
                    }
                }
                else if (mode == "Edit")
                {//修改单项奖励数据，更新到单项奖励表中
                    string id = Request["SingleAwardID"].ToString();
                    try
                    {
                        InputSingleAward inputSingleAwarda = new InputSingleAward();
                       bool result = inputSingleAwarda.GetIsNullDrjj(inputdate,deptid);

                       if (!result)
                       {
                           this.ShowMessage("系统提示", "本月奖金已存在！");
                       }
                       else
                       {
                           User user = (User)Session["CURRENTSTAFF"];
                           inputSingleAward.UpdateKjj(id, deptname, deptid, typeid, typename, checkstan, remark, money, inputdate, user.UserName);
                           scManager.AddScript("parent.Select();");
                           scManager.AddScript("parent.DetailWin.hide();");
                           //scManager.AddScript("parent.DetailWin.clearContent();");
                       }
                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveEditSingleAward");
                    }
                }
            }


        }
    }
}