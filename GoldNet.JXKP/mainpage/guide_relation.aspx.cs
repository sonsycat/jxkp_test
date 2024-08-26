using System;
using System.Data;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OleDb;
using System.Text;
using System.Collections;

namespace GoldNet.JXKP.mainpage
{
    public partial class guide_relation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                //日期选择范围

                Int32 id = 0;
                Int32 nid = 0;
                if (!Int32.TryParse(Request.QueryString["id"], out id) || !Int32.TryParse(Request.QueryString["nid"], out nid))
                {
                    return;
                }
                this.GuideCodeHidden.Text = id.ToString();
                this.Store1.DataSource = GetStoreData(id.ToString());
                this.Store1.DataBind();
            }
        }


        /// <summary>
        /// 可行性分析报表
        /// </summary>
        private DataTable GetStoreData(string GuideCode)
        {
            string yearstr = Session["curdateyear"] == null ? DateTime.Now.ToString("yyyy") : Session["curdateyear"].ToString();
            string startdate = yearstr + "0101";
            string enddate = yearstr + System.DateTime.Now.ToString("MMdd");

            //取得传入的选择的会话状态
            string deptcode = Session["curdeptcode"] == null ? ((User)Session["CURRENTSTAFF"]).AccountDeptCode : Session["curdeptcode"].ToString();
            string incount = ((User)Session["CURRENTSTAFF"]).StaffId;
            if (incount == "") incount = "NotUserid";
            string personid = Session["curpersonid"] == null ? incount : ((User)Session["CURRENTSTAFF"]).GetStaffid(Session["curpersonid"].ToString());
            DataTable dt = GetCorrelation(startdate, enddate, incount, GuideCode, deptcode, personid).Tables[0];

            return dt;
        }
         //查询按钮触发事件
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            Stack myStack = null;
            if (Session["MyStackhis"] == null)
            {
                myStack = new Stack();
            }
            else 
            {
                myStack = (Stack)Session["MyStackhis"];
            }
            myStack.Push(this.GuideCodeHidden.Text);
            this.GuideCodeHidden.Text = selectRow[0]["GUIDE_CODE"];
            Session["MyStackhis"] = myStack;
            this.Store1.DataSource = GetStoreData(selectRow[0]["GUIDE_CODE"]);
            this.Store1.DataBind();

        }
        //查询按钮触发事件
        protected void GetBackPortalet(object sender, AjaxEventArgs e)
        {
            if (Session["MyStackhis"] != null) 
            {
                Stack myStack = (Stack)Session["MyStackhis"];
                if (myStack.Count != 0) 
                {
                    string GuideCode = myStack.Pop().ToString();
                    this.GuideCodeHidden.Text = myStack.Peek().ToString();
                    Session["MyStackhis"] = myStack;
                    this.Store1.DataSource = GetStoreData(GuideCode);
                    this.Store1.DataBind();
                }
            }
        }



        /// <summary>
        /// 获取可行性分析报表
        /// </summary>
        /// <param name="StartDate">开始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <param name="incount">user_id</param>
        /// <param name="gudie_code">指标编码</param>
        /// <param name="unit_code_p">科室编码</param>
        /// <returns></returns>
        public DataSet GetCorrelation(string StartDate, string EndDate, string incount, string gudie_code, string unit_code_p, string person)
        {
            string unit_type = string.Empty;
            if (gudie_code.Substring(0, 1) == "1")
            {
                unit_type = "Y";
                unit_code_p = "00";
            }
            else if (gudie_code.Substring(0, 1) == "2")
            {
                unit_type = "K";
            }
            else if (gudie_code.Substring(0, 1) == "3")
            {
                unit_type = "R";
                unit_code_p = person;
            }
            else if (gudie_code.Substring(0, 1) == "4")
            {
                unit_type = "Z";
            }


            OleDbParameter[] parameters = {
                    new OleDbParameter("StartDate", StartDate),
                    new OleDbParameter("EndDate", EndDate),
                    new OleDbParameter("incount",incount),
                    new OleDbParameter("guidecode",gudie_code),
                    
                                                            };
            OracleOledbBase.RunProcedure("HOSPITALSYS.Correlation", parameters);

            StringBuilder str = new StringBuilder();


            string fieldName = unit_type == "R" ? "PERSON_ID" : "DEPT_CODE";


            str.AppendFormat(@"SELECT   A.GUIDE_CODE , B.GUIDE_NAME , SUM (A.GUIDE_VALUE) AS GUIDE_VALUE
                            FROM {4}.CORRELATION_TEMP A, HOSPITALSYS.GUIDE_NAME_DICT B
                           WHERE A.GUIDE_CODE = B.GUIDE_CODE
                             AND A.{3} = '{0}'
                             AND A.COUNTING(+) = '{1}'
                             AND A.GUIDE_CODE IN (
                                             SELECT GUIDE_CODE
                                               FROM {4}.GUIDE_GATHERS
                                              WHERE GUIDE_GATHER_CODE =
                                                                      (SELECT GUIDE_GATHER_CODE
                                                                         FROM {4}.GUIDE_NAME_DICT
                                                                        WHERE GUIDE_CODE = '{2}'))
                        GROUP BY A.GUIDE_CODE, B.GUIDE_NAME", unit_code_p, incount, gudie_code, fieldName,DataUser.HOSPITALSYS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }

        ///// <summary>
        ///// 导出数据至excel
        ///// </summary>
        //protected void ToExcel(object sender, EventArgs e)
        //{
        //    string json = GridData.Value.ToString();
        //    StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        //    XmlNode xml = eSubmit.Xml;

        //    this.Response.Clear();
        //    this.Response.ContentType = "application/vnd.ms-excel";
        //    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
        //    XslCompiledTransform xtExcel = new XslCompiledTransform();
        //    xtExcel.Load(Server.MapPath("Excel.xsl"));
        //    xtExcel.Transform(xml, null, this.Response.OutputStream);
        //    this.Response.End();
        //}


    }
}
