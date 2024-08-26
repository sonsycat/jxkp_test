using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Goldnet.Ext.Web;
using System.Data;

namespace GoldNet.JXKP.jxkh
{
    public partial class DeptGuideInformation : PageBase
    {
        //public static string deptcode = "";
        //public static string stationyear = "";
        //public static string guidegathercode = "";

        /// <summary>
        /// 
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
                string deptcode = Request.QueryString["deptcode"].ToString();
                string stationyear = Request.QueryString["years"].ToString();
                string guidegathercode = Request.QueryString["gathercode"].ToString();

                Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();

                //dal.InitStationGuideTarget(stationcode, stationyear, guidegathercode);

                DataTable dt = dal.GetDeptGuideTarget(deptcode, stationyear, guidegathercode).Tables[0];

                DataRow[] Rows;
                DataTable dtt;

                for (int i = 1; i <= 4; i++)
                {
                    Rows = dt.Select("BSC_TYPE='0" + i.ToString() + "'");
                    if (Rows.Length > 0)
                    {
                        Control ucp = this.Page.LoadControl("StationGuideTarget.ascx");

                        ucp.ID = "BSC" + "0" + i.ToString();

                        dtt = dt.Clone();
                        foreach (DataRow dr in Rows)
                        {
                            dtt.ImportRow(dr);
                        }
                        ((GoldNet.JXKP.jxkh.StationGuideTarget)(ucp)).Store1.DataSource = dtt;
                        ((GoldNet.JXKP.jxkh.StationGuideTarget)(ucp)).Store1.DataBind();
                        ((GoldNet.JXKP.jxkh.StationGuideTarget)(ucp)).lbl_groupname.Text = dtt.Rows[0]["BSC_TYPE_NAME"].ToString() + "  总分：";
                        ((GoldNet.JXKP.jxkh.StationGuideTarget)(ucp)).num_bscpoint.Text = dtt.Rows[0]["BSCPOINT"].ToString();

                        this.Panel1.BodyControls.Add(ucp);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonset_Click(object sender, EventArgs e)
        {
            string deptcode = Request.QueryString["deptcode"].ToString();
            string stationyear = Request.QueryString["years"].ToString();
            string guidegathercode = Request.QueryString["gathercode"].ToString();

            LoadConfig loadcfg = getLoadConfig("DeptGuideSet.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stationyear", stationyear));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("guidegathercode", guidegathercode));
            showCenterSet(this.MonthsSetWin, loadcfg);
        }

        /// <summary>
        /// 保存处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSave_Click(object sender, AjaxEventArgs e)
        {
            string deptcode = Request.QueryString["deptcode"].ToString();
            string stationyear = Request.QueryString["years"].ToString();
            string guidegathercode = Request.QueryString["gathercode"].ToString();
            ArrayList selectRows = GetSelectRow(e);
            if (selectRows == null)
            {
                return;
            }
            else
            {
                Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();
                try
                {
                    dal.SaveDeptGuideTarget(deptcode, stationyear, guidegathercode, selectRows);
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData();");
                    Ext.Msg.Alert("系统提示", "科室指标量化保存成功!").Show();
                }
                catch (Exception ee)
                {
                    Ext.Msg.Alert("系统提示", "岗位指标量化保存失败!<br/>" + "原因:" + ee.Message).Show();
                }
            }
        }

        /// <summary>
        /// 返回格式 这样取值((Dictionary<string,string>)SelectRows[1])["BSC"]
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private ArrayList GetSelectRow(AjaxEventArgs e)
        {
            ArrayList SelectRows = new ArrayList();
            for (int i = 1; i <= 4; i++)
            {
                string rows = e.ExtraParams["Value" + i.ToString()].ToString();
                Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(rows);
                if (selectRow != null)
                {
                    string bscpoint = selectRow[0]["BSCPOINT"];
                    decimal guide_value = 0;
                    for (int j = 0; j < selectRow.Length; j++)
                    {
                        guide_value += Convert.ToDecimal(selectRow[j]["GUIDE_VALUE"]);
                        SelectRows.Add(selectRow.ToArray()[j]);
                    }
                    //if (!Convert.ToDecimal(bscpoint).Equals(guide_value))
                    //{
                    //    Ext.Msg.Alert("系统提示", selectRow[0]["BSC_TYPE_NAME"].ToString() + "总分 " + bscpoint + "分与您分配的总分值 " + guide_value.ToString() + "分不一致，<br/>请调整一致后再保存！").Show();
                    //    return null;
                    //}
                }
            }
            return SelectRows;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void Buttonset_Click(object sender, EventArgs e)
        //{
        //    string deptcode = Request.QueryString["id"].ToString();
        //    string stationyear = Request.QueryString["sy"].ToString();
        //    LoadConfig loadcfg = getLoadConfig("StationsMonthsSet.aspx");
        //    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stationcode", deptcode));
        //    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stationyear", stationyear));
        //    showCenterSet(this.MonthsSetWin, loadcfg);
        //}

    }
}
