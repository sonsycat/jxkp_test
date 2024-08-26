using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;


namespace GoldNet.JXKP.jxkh
{
    public partial class Guide_SQLExpress : System.Web.UI.Page
    {
        private Goldnet.Dal.Guide_Dict dal = new Goldnet.Dal.Guide_Dict();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    Response.End();
                    return;
                }
                if (Session["SelectedGuide"] == null)
                {
                    Response.End();
                }
                SetInitState(Session["SelectedGuide"]);
            }
        }

        //设置页面各控件状态
        private void SetInitState(object selectedRow)
        {
            //日期选择
            string startdate = DateTime.Now.Year.ToString() + "-01-01";
            string enddate = System.DateTime.Now.ToString("yyyy-MM-dd");
            this.dd1.Value = startdate;
            this.dd2.Value = enddate;
            this.dd1.MinDate = System.DateTime.Now.AddYears(-10);
            this.dd1.MaxDate = System.DateTime.Now;
            this.dd2.MinDate = System.DateTime.Now.AddYears(-10);
            this.dd2.MaxDate = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-12-31");

            //组织下拉列表BtnSave_Click
            Dictionary<string, string> row = (Dictionary<string, string>)selectedRow;
            this.OrganComb.Items.Add(new Goldnet.Ext.Web.ListItem(row["ORGAN_CLASS_NAME"], row["ORGAN"]));
            this.OrganComb.SelectedIndex = 0;
            this.GuideCodeTxt.Value = row["GUIDE_CODE"];
            this.GuideNameTxt.Value = row["GUIDE_NAME"];
            DataTable dt = dal.GetGuideExpressByGuideCode(row["GUIDE_CODE"]).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string strt = dt.Rows[0]["GUIDE_SQL"].ToString().ToUpper();
                GuideSQLTxt.Text = strt;
                GuideSQLSumTxt.Text = dt.Rows[0]["GUIDE_SQL_SUM"].ToString();
                GuideSQLDetailTxt.Text = dt.Rows[0]["GUIDE_SQL_DETAIL"].ToString();
            }

        }

        //确定按钮事件
        protected void BtnSave_Click(object sender, AjaxEventArgs e)
        {
            string guidecode = this.GuideCodeTxt.Value.ToString();
            string guidesql = this.GuideSQLTxt.Value.ToString().Trim();
            string guidesumsql = this.GuideSQLSumTxt.Value.ToString().Trim();
            string guidedetailsql = this.GuideSQLDetailTxt.Value.ToString().Trim();

            string rtn = dal.SaveGuideSqls(guidecode, guidesql, guidesumsql, guidedetailsql);
            showMsg(rtn.Equals("") ? "指标算法保存成功！" : rtn);
            if (rtn.Equals(""))
            {
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.Store1.reload();");
            }

        }

        //预览按钮事件
        protected void BtnPreview_Click(object sender, AjaxEventArgs e)
        {
            Store_RefreshData(sender, null);
            this.GridWin.Show();
        }

        //预览数据刷新
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string startime = Request.Form["dd1"].ToString().Trim();
            string endtime = Request.Form["dd2"].ToString().Trim();
            string rtn = CheckMonthDate(startime, endtime);
            if (!rtn.Equals(""))
            {
                showMsg(rtn);
                return;
            }
            string guidecode = this.GuideCodeTxt.Value.ToString();
            DataTable dt = OracleOledbBase.ExecuteDataSet(string.Format("SELECT TJYF,UNIT_CODE,GUIDE_CODE,GUIDE_VALUE,GUIDE_TYPE FROM {0}.GUIDE_VALUE WHERE GUIDE_CODE ='{1}' AND TJYF BETWEEN '{2}' AND '{3}' ORDER BY TJYF", DataUser.HOSPITALSYS, guidecode, startime, endtime)).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 生成指标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnCreat_Click(object sender, AjaxEventArgs e)
        {
            string startime = Request.Form["dd1"].ToString().Trim();
            string endtime = Request.Form["dd2"].ToString().Trim();
            //检查开始结束月份
            string rtn = CheckMonthDate(startime, endtime);
            if (!rtn.Equals(""))
            {
                showMsg(rtn);
                return;
            }
            string tjny = "";
            int months = (Convert.ToInt32(endtime.Substring(0, 4)) - Convert.ToInt32(startime.Substring(0, 4))) * 12 + (Convert.ToInt32(endtime.Substring(4, 2)) - Convert.ToInt32(startime.Substring(4, 2))) + 1;
            for (int i = 0; i < months; i++)
            {
                tjny = tjny + (Convert.ToInt32(startime.Substring(0, 4)) + (i - 1 + Convert.ToInt32(startime.Substring(4, 2))) / 12).ToString() + (((i - 1 + Convert.ToInt32(startime.Substring(4, 2))) % 12) + 1).ToString().PadLeft(2, '0') + ",";
            }
            tjny = tjny.TrimEnd(',');

            string guidesql = this.GuideSQLTxt.Value.ToString().Trim();
            if (guidesql.Equals(""))
            {
                showMsg("请输入单月份指标SQL！");
                return;
            }
            string guidecode = this.GuideCodeTxt.Value.ToString();
            string guidesumsql = this.GuideSQLSumTxt.Value.ToString().Trim();
            string guidedetailsql = this.GuideSQLDetailTxt.Value.ToString().Trim();
            rtn = dal.SaveGuideSqls(guidecode, guidesql, guidesumsql, guidedetailsql);
            if (!rtn.Equals(""))
            {
                showMsg(rtn);
                return;
            }
            try
            {
                OleDbParameter[] parameters = {
                    new OleDbParameter("TJNY",tjny),
                    new OleDbParameter("guidecode", guidecode) };
                OracleOledbBase.RunProcedure(DataUser.HOSPITALSYS + ".GUIDE_VALUE_ADD", parameters);
                this.BtnPreview.Disabled = false;
                rtn = "指标数据生成成功！";
            }
            catch (Exception ee)
            {
                rtn = "数据生成失败！<br/>原因：" + ee.Message;
            }
            showMsg(rtn);
            return;
        }

        /// <summary>
        /// 检查开始结束月份
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public string CheckMonthDate(string startdate, string enddate)
        {
            string rtn = "";
            if (startdate.Equals("") || enddate.Equals(""))
            {
                rtn = "请输入开始月份和结束月份!";
                return rtn;
            }
            if (startdate.Length != 6 || enddate.Length != 6)
            {
                rtn = "请输入有效的开始月份和结束月份!";
                return rtn;
            }
            string startime = startdate.Substring(0, 4) + "-" + startdate.Substring(4, 2) + "-01";
            string endtime = enddate.Substring(0, 4) + "-" + enddate.Substring(4, 2) + "-01";
            DateTime dd1 = default(DateTime);
            DateTime dd2 = default(DateTime);
            if (!DateTime.TryParse(startime, out dd1))
            {
                rtn = "请以'YYYYMM'格式输入开始月份!";
                return rtn;
            }
            if (!DateTime.TryParse(endtime, out dd2))
            {
                rtn = "请以'YYYYMM'格式输入结束月份!";
                return rtn;
            }

            if (dd1 > dd2)
            {
                rtn = "开始月份不能晚于结束月份，请检查!";
                return rtn;
            }
            if ((dd2.Year - dd1.Year) * 12 + (dd2.Month - dd1.Month) > 24)
            {
                rtn = "数据生成月份不建议超过2年，请重新设置!";
                return rtn;
            }

            return rtn;
        }

        //显示提示信息
        public void showMsg(string msg)
        {
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
              {
                  Title = SystemMsg.msgtitle4,
                  Message = msg,
                  Buttons = MessageBox.Button.OK,
                  Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
              });
        }


    }
}
