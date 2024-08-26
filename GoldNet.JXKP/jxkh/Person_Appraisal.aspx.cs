using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using System.Data.OracleClient;
using GoldNet.Comm.DAL.Oracle;
using System.Data;
using GoldNet.Model;

namespace GoldNet.JXKP.jxkh
{
    public partial class Person_Appraisal : PageBase
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }
            if (!Ext.IsAjaxRequest)
            {
                SetInitState();
            }
        }

        //设置页面控件初始化状态
        protected void SetInitState()
        {
            //年份、月份下拉框
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.Comb_StartYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                this.Comb_EndYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            for (int i = 1; i <= 12; i++)
            {
                this.Comb_StartMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
                this.Comb_EndMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_EndMonth.SelectedIndex = DateTime.Now.Month - 1;
        }

        //评价按钮，取得并检查评价的各项参数
        protected void Btn_Eval_Click(object sender, AjaxEventArgs e)
        {
            string selectedunit = e.ExtraParams["multi1"];
            string selectedguide = e.ExtraParams["multi2"];
            Dictionary<string, string>[] Persons = JSON.Deserialize<Dictionary<string, string>[]>(selectedunit);
            Dictionary<string, string>[] Guides = JSON.Deserialize<Dictionary<string, string>[]>(selectedguide);
            if (Persons.Length.Equals(0) || Guides.Length.Equals(0))
            {
                showMsg("请选择评价人员和评价指标！");
                return;
            }
            if (Guides.Length > 200)
            {
                showMsg("评价指标建议不要超过200项！");
                return;
            }
            string startyear = this.Comb_StartYear.SelectedItem.Value;
            string startmonth = this.Comb_StartMonth.SelectedItem.Value;
            string endyear = this.Comb_EndYear.SelectedItem.Value;
            string endmonth = this.Comb_EndMonth.SelectedItem.Value;
            string evalapp = this.Comb_App.SelectedItem.Value.Trim();
            if (evalapp.Equals(""))
            {
                showMsg("请选择评价方法！");
                return;
            }
            string rtn = "";
            rtn = CheckMonthDate(startyear, startmonth, endyear, endmonth);
            if (!rtn.Equals(""))
            {
                showMsg(rtn);
                return;
            }
            startyear = startyear.PadLeft(4, '0');
            endyear = endyear.PadLeft(4, '0');
            startmonth = startmonth.PadLeft(2, '0');
            endmonth = endmonth.PadLeft(2, '0');
            string startdate = startyear + startmonth + "01";
            string enddate = endyear + endmonth + DateTime.DaysInMonth(Convert.ToInt32(endyear), Convert.ToInt32(endmonth)).ToString();

            rtn = EvalAppraisal(startdate, enddate, evalapp, Persons, Guides);
            if (rtn.Equals(""))
            {
                showDetailWin(getLoadConfig("Eval_Result_Detail.aspx?organ1=R"), "人员评价结果一览", "900");
            }
            else
            {
                showMsg(rtn);
            }
        }

        /// <summary>
        /// 开始评价
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="evalapp"></param>
        /// <param name="Depts"></param>
        /// <param name="Guides"></param>
        /// <returns></returns>
        public string EvalAppraisal(string startdate, string enddate, string evalapp, Dictionary<string, string>[] Persons, Dictionary<string, string>[] Guides)
        {
            string rtn = "";
            string guidestr = "";
            string qzstr = "";
            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;

            for (int i = 0; i < Guides.Length; i++)
            {
                guidestr = guidestr + Guides[i]["value"] + ",";
                qzstr = qzstr + Guides[i]["text"].Substring(Guides[i]["text"].LastIndexOf('(') + 1, (Guides[i]["text"].LastIndexOf(')') - Guides[i]["text"].LastIndexOf('(') - 1)) + ",";
            }
            guidestr = guidestr.TrimEnd(',');
            qzstr = guidestr.TrimEnd(',');

            //生成评价数据
            try
            {
                OracleParameter[] parameters = {
                    new OracleParameter("startdate",startdate),
                    new OracleParameter("enddate",enddate),
                    new OracleParameter("incount",incount),
                    new OracleParameter("guidecode", guidestr)  };
                OracleBase.RunProcedure(DataUser.HOSPITALSYS + ".EVALUATE_COMPUTE", parameters);
            }
            catch (Exception ee)
            {
                rtn = "评价数据生成失败！<br/>原因：" + ee.Message;
                return rtn;
            }

            //取出生成的结果，并根据评价方法计算最终得分
            Goldnet.Dal.Appraisal dal = new Goldnet.Dal.Appraisal();
            DataTable table = dal.GetAppraisalByGuide(Persons, Guides, "R", incount);
            DataTable dt = dal.GetEvalGuideQZHIGHT(guidestr, qzstr).Tables[0];

            double[] qz = new double[Guides.Length];//权重
            double[] hight = new double[Guides.Length];//是否高优、低优是否是绝对值
            double[,] guidearr = new double[Persons.Length, Guides.Length];
            for (int i = 0; i < Guides.Length; i++)
            {
                hight[i] = Convert.ToDouble(dt.Rows[i]["HIGHT"].ToString());
                qz[i] = Convert.ToDouble(dt.Rows[i]["QZ"].ToString());
            }
            for (int i = 0; i < Persons.Length; i++)
            {
                for (int j = 0; j < Guides.Length; j++)
                {
                    guidearr[i, j] = Convert.ToDouble(table.Rows[i][j + 3].ToString().Equals("") ? "0" : table.Rows[i][j + 3].ToString());
                }
            }
            double[] values = new double[Persons.Length];
            if (evalapp.Equals("Topsis"))
            {
                Goldnet.Comm.AppraisalMethod.Topsis evalmeth1 = new Goldnet.Comm.AppraisalMethod.Topsis();
                values = evalmeth1.EvaluationOper(guidearr, hight, qz);
            }
            else if (evalapp.Equals("Nearly"))
            {
                Goldnet.Comm.AppraisalMethod.Nearly evalmeth2 = new Goldnet.Comm.AppraisalMethod.Nearly();
                values = evalmeth2.EvaluationOper(guidearr, hight, qz);
            }
            else if (evalapp.Equals("RSR"))
            {
                Goldnet.Comm.AppraisalMethod.RSR evalmeth3 = new Goldnet.Comm.AppraisalMethod.RSR();
                values = evalmeth3.EvaluationOper(guidearr, hight, qz);
            }
            for (int i = 0; i < values.Length; i++)
            {
                table.Rows[i][2] = values[i].ToString();
            }
            Session.Remove("EvalPersonTable");
            Session.Add("EvalPersonTable", table);
            Session.Remove("EvalPersonSetting");
            Session.Add("EvalPersonSetting", startdate + "," + enddate + "," + evalapp + "," + incount + "," + "R");
            return rtn;
        }

        //检查开始结束月份
        public string CheckMonthDate(string year1, string month1, string year2, string month2)
        {
            string rtn = "";
            if (year1.Equals("") || month1.Equals("") || year2.Equals("") || month2.Equals(""))
            {
                rtn = "请输入开始月份和结束月份!";
                return rtn;
            }
            year1 = year1.PadLeft(4, '0');
            year2 = year2.PadLeft(4, '0');
            month1 = month1.PadLeft(2, '0');
            month2 = month2.PadLeft(2, '0');

            string startime = year1 + "-" + month1 + "-01";
            string endtime = year2 + "-" + month2 + "-01";
            DateTime dd1 = default(DateTime);
            DateTime dd2 = default(DateTime);
            if (!DateTime.TryParse(startime, out dd1))
            {
                rtn = "请选择有效的评价开始时间!";
                return rtn;
            }
            if (!DateTime.TryParse(endtime, out dd2))
            {
                rtn = "请选择有效的评价结束时间!";
                return rtn;
            }
            if (dd1 > dd2)
            {
                rtn = "评价开始时间不能晚于结束结束，请检查!";
                return rtn;
            }
            if ((dd2.Year - dd1.Year) * 12 + (dd2.Month - dd1.Month) > 12)
            {
                rtn = "评价区间建议不超过1年，请重新设置!";
                return rtn;
            }
            return rtn;
        }

        //选择评价对像
        protected void Btn_SelectPerson_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("Eval_Person_Selector.aspx");
            //获取科室权限
            string deptFilter = DeptFilter("dept_code");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptFilter", deptFilter));
            showDetailWin(loadcfg, "选择评价人员", "760");
        }

        //选择评价指标
        protected void Btn_SelectGuide_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("Eval_Guide_Selector.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("isEdit", IsEdit().ToString()));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("organ", "R"));
            showDetailWin(loadcfg, "选择评价指标", "760");
        }

        //显示详细窗口
        private void showDetailWin(LoadConfig loadcfg, string title, string width)
        {
            DetailWin.ClearContent();
            if (!title.Trim().Equals(""))
            {
                DetailWin.SetTitle(title);
            }
            if (!width.Trim().Equals(""))
            {
                DetailWin.Width = Unit.Pixel(Convert.ToInt16(width));
            }
            DetailWin.Center();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
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
