using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Text;
using System.Web.Services;
using System.Web;


namespace GoldNet.JXKP.GuideLook
{

    
    public partial class DiseaseAnalyseHightCharts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod(EnableSession=true)]
        public static string GetData(string name,string type)
        {

            string DisName = HttpContext.Current.Server.UrlDecode(name).ToString();

            ChartsDalDict dal = new ChartsDalDict();

            DataTable l_dt = dal.getDiseaseAnalyseOperationCharts(DisName, "0.15", "0.05", type).Tables[0];

            string title = l_dt.Rows[0][0].ToString();

            l_dt.Columns.Remove("JBPM");

            string Categories = DataTableTakeCategories(l_dt, "DAY_MARKING");

            l_dt.Columns.Remove("DAY_MARKING");

            string TotalCost = l_dt.Rows[0]["TOTALCOSTS"].ToString();

            l_dt.Columns.Remove("TOTALCOSTS");

            string TotalBenefit = l_dt.Rows[0]["TOTALBENEFIT"].ToString();

            l_dt.Columns.Remove("TOTALBENEFIT");


            DataTable l_dtData = dal.getDisdeaseInhospitaldates(DisName, type).Tables[0];


            string perhos = "0";

            string numbers = "0";

            string inhos = "0";

            string bedcosts = "0";

            if (l_dtData.Rows.Count > 0)
            {
                double days = 0;

                if (type == "DEPT_OPERATION_DICT")
                {
                    perhos = l_dtData.Rows[0]["AVG_LOS_BEFORE_OPER"].ToString();

                    days = double.Parse(l_dtData.Rows[0]["AVG_LOS_BEFORE_OPER"].ToString()) + double.Parse(l_dtData.Rows[0]["AVG_IN_HOSPITAL_DAYS"].ToString());
                }
                else
                {
                    days = double.Parse(l_dtData.Rows[0]["AVG_IN_HOSPITAL_DAYS"].ToString());
                }
                numbers = l_dtData.Rows[0]["NUMBER_OF_PATIENTS"].ToString();
                inhos = days.ToString();
                if (days != 0)
                {
                    bedcosts = string.Format("{0:N}", Convert.ToDouble(TotalBenefit) / days);
                }
            }


            string CaleInfo = TotalCost + "," + TotalBenefit + "," + perhos + "," + numbers + "," + inhos + "," + bedcosts;


            l_dt.Columns[0].ColumnName = "收入";

            l_dt.Columns[1].ColumnName = "收益";


            string chartData = DataTableConvertToJson(l_dt);

            string ChartsInfo = title + "$" + Categories + "$" + chartData + "$" + CaleInfo;

            return ChartsInfo;
        }



        private static string DataTableTakeCategories(DataTable l_dt,string FieldName) 
        {
            StringBuilder Categories = new StringBuilder();

            //Categories.Append("{ name:'d' , data: [");

            for (int i = 0; i < l_dt.Rows.Count;i++ )
            {
                    if (i == l_dt.Rows.Count - 1)
                    {
                        Categories.Append("'");
                        Categories.Append(l_dt.Rows[i][FieldName].ToString().Trim());
                        Categories.Append("'");
                    }
                    else
                    {
                        Categories.Append("'");
                        Categories.Append(l_dt.Rows[i][FieldName].ToString().Trim());
                        Categories.Append("',");
                    }
            }

            //Categories.Append("]}");

            return Categories.ToString();
        }


        /// <summary>
        /// DataTable转化成Json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static string DataTableConvertToJson(DataTable l_dt)
        {
            StringBuilder ChartData = new StringBuilder();
           
            for (int j = 0; j < l_dt.Columns.Count; j++)
            {
                string LabelName = l_dt.Columns[j].ColumnName;

                ChartData.Append("{ name:'" + LabelName + "' , data: [");


                for (int i = 0; i < l_dt.Rows.Count; i++)
                {
                    if (i == l_dt.Rows.Count - 1)
                    {

                        ChartData.Append(l_dt.Rows[i][j]);
                    }
                    else
                    {
                        ChartData.Append( l_dt.Rows[i][j] + ",");
                    }

                }

                if (j == l_dt.Columns.Count - 1)
                {

                    ChartData.Append("]}");

                }
                else
                {
                    ChartData.Append("]}* ");

                }
            }
            return ChartData.ToString();

        }
    }
}
