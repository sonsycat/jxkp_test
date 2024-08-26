using System;
using System.Data;
using System.Text;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.bbgl.QualityManagement
{
    public partial class QualityIndicators : System.Web.UI.Page
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


                this.Store1.DataSource = GetStoreData("1","2");

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


        /// <summary>
        /// 质量监控数据
        /// </summary>
        private DataTable GetStoreData(string startime, string endtime)
        {
            //取得传入的选择的会话状态
            //DataTable dt = OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];


            DataTable dt = CustomDataTable();

            return dt;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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



                l_dt.Rows.Add(l_dr);

            }


            return l_dt;

        }



        /// <summary>
        /// 质量监控
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetQulityList(string startime, string endtime)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                        SELECT   NVL (c.dept_old_name, '全院合计') AS 科室,
                             SUM (CASE
                                     WHEN a.guide_code = '20102018'
                                        THEN a.guide_value
                                     ELSE 0
                                  END
                                 ) AS 院内感染数,
                             CASE
                                WHEN SUM (CASE
                                             WHEN a.guide_code = '20102038'
                                                THEN a.guide_value
                                             ELSE 0
                                          END
                                         ) = 0
                                   THEN 0
                                ELSE (  ROUND (  SUM (CASE
                                                         WHEN a.guide_code = '20102018'
                                                            THEN a.guide_value
                                                         ELSE 0
                                                      END
                                                     )
                                               / SUM (CASE
                                                         WHEN a.guide_code = '20102038'
                                                            THEN a.guide_value
                                                         ELSE 0
                                                      END
                                                     ),
                                               2
                                              )
                                      * 100
                                     )
                             END AS 院内感染率,
                             SUM (CASE
                                     WHEN a.guide_code = '20102016'
                                        THEN a.guide_value
                                     ELSE 0
                                  END
                                 ) AS 手术并发症发生数,
                             CASE
                                WHEN SUM
                                       (CASE
                                           WHEN a.guide_code = '20102038'
                                              THEN a.guide_value
                                           ELSE 0
                                        END
                                       ) = 0
                                   THEN 0
                                ELSE (  ROUND (  SUM (CASE
                                                         WHEN a.guide_code = '20102016'
                                                            THEN a.guide_value
                                                         ELSE 0
                                                      END
                                                     )
                                               / SUM (CASE
                                                         WHEN a.guide_code = '20102038'
                                                            THEN a.guide_value
                                                         ELSE 0
                                                      END
                                                     ),
                                               2
                                              )
                                      * 100
                                     )
                             END AS 手术并发症发生率,
                             SUM (CASE
                                     WHEN a.guide_code = '20102014'
                                        THEN a.guide_value
                                     ELSE 0
                                  END
                                 ) AS 非手术并发症发生数,
                             CASE
                                WHEN SUM
                                       (CASE
                                           WHEN a.guide_code = '20102038'
                                              THEN a.guide_value
                                           ELSE 0
                                        END
                                       ) = 0
                                   THEN 0
                                ELSE (  ROUND (  SUM (CASE
                                                         WHEN a.guide_code = '20102014'
                                                            THEN a.guide_value
                                                         ELSE 0
                                                      END
                                                     )
                                               / SUM (CASE
                                                         WHEN a.guide_code = '20102038'
                                                            THEN a.guide_value
                                                         ELSE 0
                                                      END
                                                     ),
                                               2
                                              )
                                      * 100
                                     )
                             END AS 非手术并发症发生率,
                             SUM (CASE
                                     WHEN a.guide_code = '20102030'
                                        THEN a.guide_value
                                     ELSE 0
                                  END
                                 ) AS 医疗事故发生数,
                             SUM (CASE
                                     WHEN a.guide_code = '20102031'
                                        THEN a.guide_value
                                     ELSE 0
                                  END
                                 ) AS 医疗差错发生数,
                             SUM (CASE
                                     WHEN a.guide_code = '20102097'
                                        THEN a.guide_value
                                     ELSE 0
                                  END
                                 ) AS 护理差错发生数,
                             SUM (CASE
                                     WHEN a.guide_code = '20102026'
                                        THEN a.guide_value
                                     ELSE 0
                                  END
                                 ) AS 甲级病案数,
                             CASE
                                WHEN SUM (CASE
                                             WHEN a.guide_code = '20102038'
                                                THEN a.guide_value
                                             ELSE 0
                                          END
                                         ) = 0
                                   THEN 0
                                ELSE (  ROUND (  SUM (CASE
                                                         WHEN a.guide_code = '20102026'
                                                            THEN a.guide_value
                                                         ELSE 0
                                                      END
                                                     )
                                               / SUM (CASE
                                                         WHEN a.guide_code = '20102038'
                                                            THEN a.guide_value
                                                         ELSE 0
                                                      END
                                                     ),
                                               2
                                              )
                                      * 100
                                     )
                             END AS 甲级病案率
                        FROM hospitalsys.guide_value a,
                             hospitalsys.guide_name_dict b,
                             cbhs.cbhs_dept_dict c
                        WHERE a.guide_code = b.guide_code
                         AND a.unit_code = c.dept_new_code
                         AND TO_DATE (a.tjyf, 'yyyyMM') >= TO_DATE ('{0}', 'yyyyMM')
                         AND TO_DATE (a.tjyf, 'yyyyMM') <= TO_DATE ('{1}', 'yyyyMM')
                        GROUP BY ROLLUP (c.dept_old_name)", startime, endtime);
            return str;
        }




    }
}
