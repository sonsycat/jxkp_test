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
    public partial class InHospitalCommunicate : System.Web.UI.Page
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


                l_dt.Rows.Add(l_dr);

            }


            return l_dt;

        }


    }
}
