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
    /// 对科室效益录入进行编辑和新增
    /// </summary>
    public partial class BenefitAdjustEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
              
                if (Request["BenefitAdjustMode"] != null)
                {
                 
                    string mode = Request["BenefitAdjustMode"].ToString();
                    InputBenefitAdjust inputbenefitadjust = new InputBenefitAdjust();
                    DataTable dtDept = inputbenefitadjust.GetDept();
                    Store2.DataSource = dtDept;
                    Store2.DataBind();
                    //如果是编辑界面
                    if (mode == "Edit" && Request["BenefitAdjustID"] != null)
                    {
                       
                        string id = Request["BenefitAdjustID"].ToString();
                        DataTable dt = inputbenefitadjust.GetBenefitAdjustSimple(id);
                        //根据ID查找效益数据，并且赋值给控件
                        if (dt.Rows.Count > 0)
                        {
                            cbbdept.SelectedItem.Value = dt.Rows[0]["DEPT_CODE"].ToString();
                            cbbdept.SelectedItem.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                            dfInputDate.Value=dt.Rows[0]["ADJUST_DATE"];
                            for (int i = 0; i < rgType.Items.Count; i++)
                            {
                                if (rgType.Items[i].BoxLabel == dt.Rows[0]["TYPE"].ToString())
                                {
                                    rgType.Items[i].Checked=true;
                                }
                                else
                                {
                                    rgType.Items[i].Checked = false;
                                }
                            }
                            nfNumber.Value =Convert.ToDouble(dt.Rows[0]["MONEY"]);
                            if (dt.Rows[0]["MEMO"] != null && dt.Rows[0]["MEMO"].ToString()!="")
                            { taMEMO.Value = dt.Rows[0]["MEMO"]; }
                            else
                            { taMEMO.Value = ""; }
                            for (int i = 0; i < rgDirection.Items.Count; i++)
                            {
                                if (rgDirection.Items[i].BoxLabel == dt.Rows[0]["DIRECTION"].ToString())
                                {
                                    rgDirection.Items[i].Checked = true;
                                }
                                else
                                {
                                    rgDirection.Items[i].Checked = false;
                                }
                            }
                            tfInputer.Visible = false;
                            tfInputDate.Visible = false;
                            tfModifier.Visible = false;
                            tfModifyDate.Visible = false;
                        }
                        
                    }
                    else if (mode == "Add")
                    {//新建效益数据，页面控件清空
                        cbbdept.SetValue("");
                        dfInputDate.Value=DateTime.Now.ToString();
                        rgType.SetValue("");
                        nfNumber.Value =Convert.ToDouble(0);
                        taMEMO.Value = "";
                        rgDirection.SetValue("");
                        tfInputer.Value = "";
                        tfInputDate.Value = "";
                        tfModifier.Value = "";
                        tfModifyDate.Value = "";
                        tfInputer.Visible = false;
                        tfInputDate.Visible = false;
                        tfModifier.Visible = false;
                        tfModifyDate.Visible = false;

                    }
                    if (mode == "Look" && Request["BenefitAdjustID"] != null)
                    {
                        //查看效益数据，根据ID找到数据并且赋值给控件同时控件不可用
                        string id = Request["BenefitAdjustID"].ToString();
                        DataTable dt = inputbenefitadjust.GetBenefitAdjustSimple(id);
                        if (dt.Rows.Count > 0)
                        {
                            cbbdept.SelectedItem.Value = dt.Rows[0]["DEPT_CODE"].ToString();
                            cbbdept.SelectedItem.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                            dfInputDate.Value = dt.Rows[0]["ADJUST_DATE"];
                            for (int i = 0; i < rgType.Items.Count; i++)
                            {
                                if (rgType.Items[i].BoxLabel == dt.Rows[0]["TYPE"].ToString())
                                {
                                    rgType.Items[i].Checked = true;
                                }
                                else
                                {
                                    rgType.Items[i].Checked = false;
                                }
                            }

                            nfNumber.Value = Convert.ToDouble(dt.Rows[0]["MONEY"]);
                            if (dt.Rows[0]["MEMO"] != null && dt.Rows[0]["MEMO"].ToString() != "")
                            { taMEMO.Value = dt.Rows[0]["MEMO"]; }
                            else
                            { taMEMO.Value = ""; }
                            for (int i = 0; i < rgDirection.Items.Count; i++)
                            {
                                if (rgDirection.Items[i].BoxLabel == dt.Rows[0]["DIRECTION"].ToString())
                                {
                                    rgDirection.Items[i].Checked = true;
                                }
                                else
                                {
                                    rgDirection.Items[i].Checked = false;
                                }
                            }
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
                            rgType.Disabled = true;
                            nfNumber.Disabled = true;
                            taMEMO.Disabled = true;
                            rgDirection.Disabled = true;
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
                }
               SetStoreProxy();
            }
        }
        private void SetStoreProxy()
        {           
            DateTime iDate = Convert.ToDateTime(dfInputDate.Value);
            //查找科室信息
            HttpProxy pro = new HttpProxy();
            pro.Method = HttpMethod.POST;
            pro.Url = "../WebService/BonusDepts.ashx?dept_date="+iDate;
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
        [AjaxMethod]
        public void SetServeiceDate()
        {
           // SetStoreProxy();
        }
        protected void Save(object sender, EventArgs e)
        {
            string type = "";
            for (int i = 0; i < rgType.Items.Count; i++)
            {
                if (rgType.Items[i].Checked)
                {
                    type = rgType.Items[i].BoxLabel;
                }
            }
            Ext.Msg.Confirm("系统提示", "您确定要对'"+type+"'进行调整吗？", new MessageBox.ButtonsConfig
            {
                Yes = new MessageBox.ButtonConfig
                {
                    Handler = "Goldnet.SaveEditSimpleEncourage()",
                    Text = "确定"

                },
                No = new MessageBox.ButtonConfig
                {
                    Text = "取消"
                }
            }).Show();

        }
        /// <summary>
        /// 保存效益数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [AjaxMethod]
        public void SaveEditSimpleEncourage()
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            if (Request["BenefitAdjustMode"] != null)
            {
                //获得页面中控件的值
                InputBenefitAdjust inputbenefitadjust = new InputBenefitAdjust();
                string mode = Request["BenefitAdjustMode"].ToString();

                string deptid = cbbdept.SelectedItem.Value.Trim().ToString();
                string deptname = cbbdept.SelectedItem.Text.Trim().ToString();
                DateTime adjustdate = dfInputDate.SelectedDate;
                string type = "";
                for (int i = 0; i < rgType.Items.Count; i++)
                {
                    if (rgType.Items[i].Checked)
                    {
                        type = rgType.Items[i].BoxLabel;
                    }
                }
                double money = Convert.ToDouble(nfNumber.Text);
                string memo = taMEMO.Text;
                string direction = "";
                for (int i = 0; i < rgDirection.Items.Count; i++)
                {
                    if (rgDirection.Items[i].Checked)
                    {
                        direction = rgDirection.Items[i].BoxLabel;
                    }
                }

                if (mode == "Add")
                {//新建效益数据，插入的效益表中
                    try
                    {
                       User user = (User)Session["CURRENTSTAFF"];
                       inputbenefitadjust.InsertBenefitAdjust(deptname, deptid, type, direction, money, memo, adjustdate, user.UserName);
                       //scManager.AddScript(string.Format("alert('{0}');", type + "调整成功!"));
                       //this.ShowMessage("提示",type+"调整成功！");
                       scManager.AddScript("parent.RefreshData();");
                       //scManager.AddScript("parent.DetailWin.hide();");
                       //scManager.AddScript("parent.DetailWin.clearContent();");
                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveInSertBenefitAdjust");
                    }
                }
                else if (mode == "Edit")
                {//修改效益数据，更新到效益表中
                    string id = Request["BenefitAdjustID"].ToString();
                    try
                    {
                        User user = (User)Session["CURRENTSTAFF"];
                        inputbenefitadjust.UpdateBenefitAdjust(id, deptname, deptid, type, direction, money, memo, adjustdate, user.UserName);
                        scManager.AddScript(string.Format("alert('{0}');", type + "修改成功!"));
                        //this.ShowMessage("提示", type + "修改成功！");
                        scManager.AddScript("parent.RefreshData();");
                        scManager.AddScript("parent.DetailWin.hide();");
                        scManager.AddScript("parent.DetailWin.clearContent();");
                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveEditBenefitAdjust");
                    }
                }
            }          
           

        }
    }
}
