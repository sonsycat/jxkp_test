using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.bbgl.MedicalIndicators
{
    public partial class DeptGradeBrowse : System.Web.UI.Page
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
                string startdate = DateTime.Now.Year.ToString() + "-01-01";
                string enddate = System.DateTime.Now.ToString("yyyy-MM-dd");

                this.dd1.Value = startdate;
                this.dd2.Value = enddate;

                this.dd1.MinDate = System.DateTime.Now.AddYears(-10);
                this.dd1.MaxDate = System.DateTime.Now.AddYears(1);
                this.dd2.MinDate = System.DateTime.Now.AddYears(-10);
                this.dd2.MaxDate = System.DateTime.Now.AddYears(1);


                this.Store1.DataSource = GetStoreData("1", "2");

                this.Store1.DataBind();

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Store1.RemoveAll();
            Store1.DataSource = GetStoreData("1", "2");
            Store1.DataBind();

        }


 
        private DataTable GetStoreData(string startime, string endtime)
        {
            //取得传入的选择的会话状态
            //DataTable dt = OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];


            DataTable dt = CustomDataTable();

            return dt;

        }



        private DataTable CustomDataTable()
        {


            DataTable l_dt = new DataTable();


             l_dt.Columns.Add("Columns1"); 
            l_dt.Columns.Add("Columns2"); 
            l_dt.Columns.Add("Columns3"); 
            l_dt.Columns.Add("Columns4"); 
            l_dt.Columns.Add("Columns5"); 
            l_dt.Columns.Add("Columns6"); 
            l_dt.Columns.Add("Columns7"); 
            l_dt.Columns.Add("Columns8"); 
            l_dt.Columns.Add("Columns9"); 
            l_dt.Columns.Add("Columns10"); 
            l_dt.Columns.Add("Columns11"); 
            l_dt.Columns.Add("Columns12"); 
            l_dt.Columns.Add("Columns13"); 
            l_dt.Columns.Add("Columns14"); 
            l_dt.Columns.Add("Columns15"); 
            l_dt.Columns.Add("Columns16"); 
            l_dt.Columns.Add("Columns17"); 
            l_dt.Columns.Add("Columns18"); 
            l_dt.Columns.Add("Columns19"); 
            l_dt.Columns.Add("Columns20"); 
            l_dt.Columns.Add("Columns21"); 
            l_dt.Columns.Add("Columns22"); 
            l_dt.Columns.Add("Columns23"); 
            l_dt.Columns.Add("Columns24"); 
            l_dt.Columns.Add("Columns25"); 
            l_dt.Columns.Add("Columns26"); 
            l_dt.Columns.Add("Columns27"); 
            l_dt.Columns.Add("Columns28"); 
            l_dt.Columns.Add("Columns29"); 
            l_dt.Columns.Add("Columns30"); 
            l_dt.Columns.Add("Columns31"); 
            l_dt.Columns.Add("Columns32"); 
            l_dt.Columns.Add("Columns33"); 
            l_dt.Columns.Add("Columns34"); 
            l_dt.Columns.Add("Columns35"); 
            l_dt.Columns.Add("Columns36"); 
            l_dt.Columns.Add("Columns37"); 
            l_dt.Columns.Add("Columns38"); 
            l_dt.Columns.Add("Columns39"); 
            l_dt.Columns.Add("Columns40"); 
            l_dt.Columns.Add("Columns41"); 
            l_dt.Columns.Add("Columns42"); 


            for (int i = 0; i < 50; i++)
            {

                DataRow l_dr = l_dt.NewRow();


                l_dr[0] = "保健科";
                l_dr[1] = "20";
                l_dr[2] = "20";
                l_dr[3] = "20";
                l_dr[4] = "20";
                l_dr[5] = "20";
                l_dr[6] = "20";
                l_dr[7] = "20";
                l_dr[8] = "20";
                l_dr[9] = "20";
                l_dr[10] = "20";
                l_dr[11] = "20";
                l_dr[12] = "20";
                l_dr[13] = "20";
                l_dr[14] = "20";
                l_dr[15] = "20";
                l_dr[16] = "20";
                l_dr[17] = "20";
                l_dr[18] = "20";
                l_dr[19] = "20";
                l_dr[20] = "20";
                l_dr[21] = "20";
                l_dr[22] = "20";
                l_dr[23] = "20";
                l_dr[24] = "20";
                l_dr[25] = "20";
                l_dr[26] = "20";
                l_dr[27] = "20";
                l_dr[28] = "20";
                l_dr[29] = "20";
                l_dr[30] = "20";
                l_dr[31] = "20";
                l_dr[32] = "20";
                l_dr[33] = "20";
                l_dr[34] = "20";
                l_dr[35] = "20";
                l_dr[36] = "20";
                l_dr[37] = "20";
                l_dr[38] = "20";
                l_dr[39] = "20";
                l_dr[40] = "20";
                l_dr[41] = "20"; 



                l_dt.Rows.Add(l_dr);

            }


            return l_dt;

        }


    }
}
