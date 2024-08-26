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
    public partial class BenefitAdjustSearch :PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            { 
                BoundComm boundComm=new BoundComm();
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
             condition=GetDateCondition(condition);
             condition = GetDeptCondition(condition);
             condition = GetTypeCondition(condition);
             condition = GetNumberCondition(condition);
             condition = GetDicrectionCondition(condition);
             if (condition != "")
             {
                 Session["BenefitAdjustSearch"] = condition;
             }
             Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
             scManager.AddScript("parent.Search.hide();");
             scManager.AddScript("parent.Search.clearContent();");
        }
        //时间条件
        private string GetDateCondition(string condition)
        {
            string dtDate = "";
            if (cbbCTime.SelectedItem.Value != null && cbbCTime.SelectedItem.Value.ToString() != "条件" && cbbCTime.SelectedItem.Value.ToString()!="")
            {
                if (dfDate.Value != null && dfDate.Value.ToString() != "")
                {
                    dtDate = dfDate.SelectedDate.ToString("yyyyMMdd");
                    
                }
            }
            if (dtDate != "")
            {
                string sign = GetSign(cbbCTime.SelectedItem.Value.ToString());
                condition += "ADJUST_DATE" + sign + "to_date('" + dtDate + "','yyyymmdd')";
            }
            return condition;
        }
        //部门条件
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
        //类别条件
        private string GetTypeCondition(string condition)
        {
            string type = "";
            if (cbbCType.SelectedItem.Value != null && cbbCType.SelectedItem.Value.ToString() != "条件" && cbbCType.SelectedItem.Value.ToString() != "")
            {
                for (int i = 0; i < rgType.Items.Count; i++)
                {
                    if (rgType.Items[i].Checked)
                    {
                        type = rgType.Items[i].BoxLabel;
                    }
                }
            }
            if (type != "")
            {               
                if (condition != "")
                {
                    if (cbbRDept.SelectedItem.Value != null && cbbRDept.SelectedItem.Value.ToString() != "")
                    {
                        string rela = cbbRDept.SelectedItem.Value.ToString();
                        if (rela == "BingQie")
                        {
                            condition += " and TYPE" + GetSign(cbbCType.SelectedItem.Value.ToString(), type);
                        }
                        else
                        {
                            condition += " or TYPE" + GetSign(cbbCType.SelectedItem.Value.ToString(), type);
                        }
                    }
                   
                }
                else
                {
                    condition += " TYPE" + GetSign(cbbCType.SelectedItem.Value.ToString(), type);
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
                    if (cbbRType.SelectedItem.Value != null && cbbRType.SelectedItem.Value.ToString() != "")
                    {
                        string rela = cbbRType.SelectedItem.Value.ToString();
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
        //调整条件
        private string GetDicrectionCondition(string condition)
        {
            string direction = "";
            if (cbbCDirection.SelectedItem.Value != null && cbbCDirection.SelectedItem.Value.ToString() != "条件" && cbbCDirection.SelectedItem.Value.ToString() != "")
            {
                for (int i = 0; i < rgDirection.Items.Count; i++)
                {
                    if (rgDirection.Items[i].Checked)
                    {
                        direction = rgDirection.Items[i].BoxLabel;
                    }
                }
            }
            if (direction != "")
            {
                if (condition != "")
                {
                    if (cbbRNumber.SelectedItem.Value != null && cbbRNumber.SelectedItem.Value.ToString() != "")
                    {
                        string rela = cbbRNumber.SelectedItem.Value.ToString();
                        if (rela == "BingQie")
                        {
                            condition += " and DIRECTION" + GetSign(cbbCDirection.SelectedItem.Value.ToString(), direction);
                        }
                        else
                        {
                            condition += " or DIRECTION" + GetSign(cbbCDirection.SelectedItem.Value.ToString(), direction);
                        }
                    }

                }
                else
                {
                    condition += " DIRECTION" + GetSign(cbbCDirection.SelectedItem.Value.ToString(), direction);
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
