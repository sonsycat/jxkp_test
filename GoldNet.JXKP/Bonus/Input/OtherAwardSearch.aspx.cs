using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Comm;

namespace GoldNet.JXKP
{
    public partial class OtherAwardSearch : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                BoundComm boundComm = new BoundComm();
                StoreDate.DataSource = boundComm.dtDate();
                StoreDate.DataBind();
                StoreCondition.DataSource = boundComm.dtCondition();
                StoreCondition.DataBind();
                StoreRelation.DataSource = boundComm.dtRelation();
                StoreRelation.DataBind();
            }
        }

        protected void Search_Click(object sender, AjaxEventArgs e)
        {
            string condition = "";
            condition = GetDateCondition(condition);
            condition = GetDeptCondition(condition);
            condition = GetItemCondition(condition);
            condition = GetNumberCondition(condition);
            condition = GetInputerCondition(condition);
            condition = GetModifyCondition(condition);
            if (condition != "")
            {
                Session["OtherAwardSearch"] = condition;
            }
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.Search.hide();");
            scManager.AddScript("parent.Search.clearContent();");
        }
        //时间条件
        private string GetDateCondition(string condition)
        {
            string dtDate = "";
            if (cbbCTime.SelectedItem.Value != null && cbbCTime.SelectedItem.Value.ToString() != "条件" && cbbCTime.SelectedItem.Value.ToString() != "")
            {
                if (dfDate.Value != null && dfDate.Value.ToString() != "")
                {
                    dtDate = dfDate.SelectedDate.ToString("yyyyMMdd");

                }
            }
            if (dtDate != "")
            {
                string sign = GetSign(cbbCTime.SelectedItem.Value.ToString());
                condition += "INPUT_DATE" + sign + "to_date('" + dtDate + "','yyyymmdd')";
            }
            return condition;
        }
        //科室条件
        private string GetDeptCondition(string condition)
        {
            string dept = "";
            if (cbbCDept.SelectedItem.Value != null && cbbCDept.SelectedItem.Value.ToString() != "条件" && cbbCDept.SelectedItem.Value.ToString() != "")
            {
                if (tfDept.Value != null && tfDept.Value.ToString() != "")
                {
                    dept = tfDept.Value.ToString();
                }
            }
            if (dept != "")
            {
                if (condition != "")
                {
                    if (cbbRTime.SelectedItem.Value != null && cbbRTime.SelectedItem.Value.ToString() != "")
                    {
                        string rela = cbbRTime.SelectedItem.Value.ToString();
                        if (rela == "BingQie")
                        {
                            condition += " and DEPT_NAME" + GetSign(cbbCDept.SelectedItem.Value.ToString(), tfDept.Value.ToString());
                        }
                        else
                        {
                            condition += " or DEPT_NAME" + GetSign(cbbCDept.SelectedItem.Value.ToString(), tfDept.Value.ToString());
                        }
                    }

                }
                else
                {
                    condition += " DEPT_NAME" + GetSign(cbbCDept.SelectedItem.Value.ToString(), tfDept.Value.ToString());
                }
            }
            return condition;
        }
        //奖惩项目条件
        private string GetItemCondition(string condition)
        {
            string item = "";
            if (cbbCItem.SelectedItem.Value != null && cbbCItem.SelectedItem.Value.ToString() != "条件" && cbbCItem.SelectedItem.Value.ToString() != "")
            {
                if (tfDept.Value != null && tfDept.Value.ToString() != "")
                {
                    item = tfItem.Value.ToString();
                }
            }
            if (item != "")
            {
                if (condition != "")
                {
                    if (cbbRDept.SelectedItem.Value != null && cbbRDept.SelectedItem.Value.ToString() != "")
                    {
                        string rela = cbbRDept.SelectedItem.Value.ToString();
                        if (rela == "BingQie")
                        {
                            condition += " and TYPE_NAME" + GetSign(cbbCItem.SelectedItem.Value.ToString(), item);
                        }
                        else
                        {
                            condition += " or TYPE_NAME" + GetSign(cbbCItem.SelectedItem.Value.ToString(), item);
                        }
                    }

                }
                else
                {
                    condition += " TYPE_NAME" + GetSign(cbbCItem.SelectedItem.Value.ToString(), item);
                }
            }
            return condition;
        }
        //金额条件
        private string GetNumberCondition(string condition)
        {
            string number = "";
            if (cbbCValue.SelectedItem.Value != null && cbbCValue.SelectedItem.Value.ToString() != "条件" && cbbCValue.SelectedItem.Value.ToString() != "")
            {
                if (tfNumber.Value != null && tfNumber.Value.ToString() != "")
                {
                    number = tfNumber.Value.ToString();
                }
            }
            if (number != "")
            {
                if (condition != "")
                {
                    if (cbbRItem.SelectedItem.Value != null && cbbRItem.SelectedItem.Value.ToString() != "")
                    {
                        string rela = cbbRItem.SelectedItem.Value.ToString();
                        if (rela == "BingQie")
                        {
                            condition += " and MONEY " + GetSign(cbbCValue.SelectedItem.Value.ToString()) + " " + number;
                        }
                        else
                        {
                            condition += " or MONEY " + GetSign(cbbCValue.SelectedItem.Value.ToString()) + " " + number;
                        }
                    }

                }
                else
                {
                    condition += " MONEY " + GetSign(cbbCValue.SelectedItem.Value.ToString()) + " " + number;
                }
            }
            return condition;
        }
        //录入人条件
        private string GetInputerCondition(string condition)
        {
            string inputer = "";
            if (cbbCInputer.SelectedItem.Value != null && cbbCInputer.SelectedItem.Value.ToString() != "条件" && cbbCInputer.SelectedItem.Value.ToString() != "")
            {
                if (tfNumber.Value != null && tfNumber.Value.ToString() != "")
                {
                    inputer = tfInputer.Value.ToString();
                }
            }
            if (inputer != "")
            {
                if (condition != "")
                {
                    if (cbbRNumber.SelectedItem.Value != null && cbbRNumber.SelectedItem.Value.ToString() != "")
                    {
                        string rela = cbbRNumber.SelectedItem.Value.ToString();
                        if (rela == "BingQie")
                        {
                            condition += " and INPUTER" + GetSign(cbbCInputer.SelectedItem.Value.ToString(), inputer);
                        }
                        else
                        {
                            condition += " or INPUTER" + GetSign(cbbCInputer.SelectedItem.Value.ToString(), inputer);
                        }
                    }

                }
                else
                {
                    condition += " INPUTER" + GetSign(cbbCInputer.SelectedItem.Value.ToString(), inputer);
                }
            }
            return condition;
        }
        //修改人条件
        private string GetModifyCondition(string condition)
        {
            string modifier = "";
            if (cbbCModifier.SelectedItem.Value != null && cbbCModifier.SelectedItem.Value.ToString() != "条件" && cbbCModifier.SelectedItem.Value.ToString() != "")
            {
                if (tfNumber.Value != null && tfNumber.Value.ToString() != "")
                {
                    modifier = tfModifier.Value.ToString();
                }
            }
            if (modifier != "")
            {
                if (condition != "")
                {
                    if (cbbRInputer.SelectedItem.Value != null && cbbRInputer.SelectedItem.Value.ToString() != "")
                    {
                        string rela = cbbRInputer.SelectedItem.Value.ToString();
                        if (rela == "BingQie")
                        {
                            condition += " and MODIFIER" + GetSign(cbbCModifier.SelectedItem.Value.ToString(), modifier);
                        }
                        else
                        {
                            condition += " or MODIFIER" + GetSign(cbbCModifier.SelectedItem.Value.ToString(), modifier);
                        }
                    }

                }
                else
                {
                    condition += " MODIFIER" + GetSign(cbbCModifier.SelectedItem.Value.ToString(), modifier);
                }
            }
            return condition;
        }
        private string GetSign(string sign)
        {
            switch (sign)
            {
                case "DaYu":
                    return ">";
                case "DaYuDengYu":
                    return ">=";
                case "XiaoYu":
                    return "<";
                case "XiaoYuDengYu":
                    return "<=";
                case "BuDengYu":
                    return "<>";
                case "DengYu":
                    return "=";
            }
            return "";
        }
        private string GetSign(string sign, string value)
        {
            switch (sign)
            {
                case "DengYu":
                    return "='" + value + "'";
                case "BaoHan":
                    return " like '%" + value + "%'";
            }
            return "";
        }
    }
}
