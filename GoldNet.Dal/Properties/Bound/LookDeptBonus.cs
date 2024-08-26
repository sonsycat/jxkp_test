using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.Comm;
using System.Data.OleDb;
using System.Data.OracleClient;
namespace Goldnet.Dal
{
    public class LookDeptBonus
    {
        /// <summary>
        /// 获得科室效益数据
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetDeptBenefit(string years,string months,string deptcode)
        {
            if(months.Length==1)
            {
                months="0"+months;
            }
            DateTime deSTDate = Convert.ToDateTime(years + "-" + months + "-01");
            string sql = " select a.* from performance.LOOK_TOTAL_BENEFIT a,comm.sys_dept_dict b where b.account_dept_code='" + deptcode + "' and ";
                   sql += " a.st_date=to_date('" + deSTDate.ToString("yyyyMMdd") + "','yyyymmdd') and a.dept_code=b.dept_code";
           DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
           if (ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
           {
               DataTable dtBenefit = BuildBenefit();
               for (int i = 0; i < dtBenefit.Rows.Count; i++)
               { 
                   DataRow []dr=ds.Tables[0].Select("TypeName='"+ dtBenefit.Rows[i]["Type"].ToString()+"'");
                   if (dr.Length > 0)
                   {
                       dtBenefit.Rows[i]["Money"] = dr[0]["BENEFIT"];
                   }
               }
               return dtBenefit;
           }
           else
           {
               return BuildBenefit();
           }
           
        }
        /// <summary>
        /// 科室效益虚拟表
        /// </summary>
        /// <returns></returns>
        private DataTable BuildBenefit()
        {
            DataTable dt = new DataTable();
            DataColumn dcID = new DataColumn("Type");
            DataColumn dcDEPTNAME = new DataColumn("Money");
            dt.Columns.AddRange(new DataColumn[] { dcID, dcDEPTNAME });
            DataRow dr1 = dt.NewRow();
            dr1["Type"] = "实际收入";
            dr1["Money"] = 0;
            DataRow dr2 = dt.NewRow();
            dr2["Type"] = "免费收入";
            dr2["Money"] = 0;
            DataRow dr3 = dt.NewRow();
            dr3["Type"] = "科室成本";
            dr3["Money"] = 0;
            DataRow dr4 = dt.NewRow();
            dr4["Type"] = "科室效益";
            dr4["Money"] = 0;
            dt.Rows.Add(dr1);
            dt.Rows.Add(dr2);
            dt.Rows.Add(dr3);
            dt.Rows.Add(dr4);
            return dt;
        }
        /// <summary>
        /// 获得质量得分
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetDeptQuality(string years,string months,string deptcode)
        {
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            DateTime deSTDate = Convert.ToDateTime(years + "-" + months + "-01");
            string qualityType = "select guidetype,0 as worth,0 as score from zlgl.g_guidetype order by guidetype ";
            DataSet ds = OracleOledbBase.ExecuteDataSet(qualityType.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                string qualityscore = "select nvl(guidetype,'汇总') guidetype,sum(worth) worth,sum(score) score ";
                qualityscore += " from performance.LOOK_DEPT_QUALITY ";
                qualityscore += " where st_date=to_date('" + deSTDate.ToString("yyyyMMdd") + "','yyyymmdd') and dept_code='" + deptcode + "' ";
                qualityscore += " group by rollup(guidetype) ";
                DataSet dsScore = OracleOledbBase.ExecuteDataSet(qualityscore.ToString());
                if (dsScore.Tables.Count > 0 && dsScore.Tables[0] != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow []dr=dsScore.Tables[0].Select("guidetype='" + ds.Tables[0].Rows[i]["guidetype"] + "'");
                        if (dr.Length > 0)
                        {
                            ds.Tables[0].Rows[i]["worth"] = dr[0]["worth"];
                            ds.Tables[0].Rows[i]["score"] = dr[0]["score"];
                        }
                    }
                    DataRow drTotal = ds.Tables[0].NewRow();
                    drTotal["guidetype"] = "汇总";
                    drTotal["worth"] = "0";
                    drTotal["score"] = "0";
                    DataRow[] drHui = dsScore.Tables[0].Select("guidetype='汇总'");
                    if (drHui.Length > 0)
                    {
                        drTotal["worth"] = drHui[0]["worth"];
                        drTotal["score"] = drHui[0]["score"];
                    }
                    ds.Tables[0].Rows.Add(drTotal);
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }   
        }
        /// <summary>
        /// 查找单项奖惩的数据
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable GetSimpleAward(string years, string months, string dept_code)
        {
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            DateTime deSTDate = Convert.ToDateTime(years + "-" + months + "-01");
            string sqlSA = " select b.DEPT_NAME,TYPE_NAME,round(sum(money),2) money from performance.INPUT_SINGLEAWARD a left join comm.sys_dept_dict b on a.dept_code=b.dept_code";
            sqlSA += " where del_flag=0 and trunc(award_date,'mm')=to_date('" + deSTDate.ToString("yyyyMMdd") + "','yyyymmdd') and (a.dept_code='" + dept_code + "' or b.ACCOUNT_DEPT_CODE='" + dept_code + "')";
            sqlSA += " group by b.DEPT_NAME,TYPE_NAME ";
            DataSet dsSA = OracleOledbBase.ExecuteDataSet(sqlSA.ToString());
            if (dsSA.Tables.Count > 0 && dsSA.Tables[0] != null && dsSA.Tables[0].Rows.Count > 0)
            {
                return dsSA.Tables[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获得其他奖惩信息
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable GetOtherAward(string years, string months, string dept_code)
        {
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            DateTime deSTDate = Convert.ToDateTime(years + "-" + months + "-01");
            string sqlOA = " select b.dept_name,OTHER_DICT_NAME as TYPE_NAME,round(sum(money),2) money from performance.INPUT_OTHERAWARD  a left join comm.sys_dept_dict b on a.dept_code=b.dept_code";
            sqlOA += " where del_flag=0 and trunc(INPUT_DATE,'mm')=to_date('" + deSTDate.ToString("yyyyMMdd") + "','yyyymmdd') and (a.dept_code='" + dept_code + "' or b.ACCOUNT_DEPT_CODE='" + dept_code + "')";
            sqlOA += " group by b.dept_name,OTHER_DICT_NAME ";
            DataSet dsOA = OracleOledbBase.ExecuteDataSet(sqlOA.ToString());
            if (dsOA.Tables.Count > 0 && dsOA.Tables[0] != null && dsOA.Tables[0].Rows.Count > 0)
            {
                return dsOA.Tables[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获得核算科室的收入明细
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable GetDeptIncomesDetail(string years,string months,string dept_code)
        {
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            DateTime deSTDate = Convert.ToDateTime(years + "-" + months + "-01");
            string sqlincomes = " select dept_name,reck_item,class_name,incomes,incomes_charges,total_incomes from performance.LOOK_DEPT_INCOMES_DETAIL ";
            sqlincomes += " where trunc(ST_DATE,'mm')=to_date('" + deSTDate.ToString("yyyyMMdd") + "','yyyymmdd') and (ACCOUNT_DEPT_CODE='" + dept_code + "' or DEPT_CODE='"+dept_code+"')";
            sqlincomes += " order by dept_name";
            DataSet dsIncomes = OracleOledbBase.ExecuteDataSet(sqlincomes.ToString());
            if (dsIncomes.Tables.Count > 0 && dsIncomes.Tables[0] != null && dsIncomes.Tables[0].Rows.Count > 0)
            {
                return dsIncomes.Tables[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获得成本明细
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable GetDeptCostsDetail(string years, string months, string dept_code)
        {
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            DateTime deSTDate = Convert.ToDateTime(years + "-" + months + "-01");
            string sqlcosts = " select DEPT_NAME,COST_ITEM,ITEM_NAME,COSTS from performance.LOOK_DEPT_COST_DETAIL ";
            sqlcosts += " where trunc(ST_DATE,'mm')=to_date('" + deSTDate.ToString("yyyyMMdd") + "','yyyymmdd') and （ACCOUNT_DEPT_CODE='" + dept_code + "' or DEPT_CODE='" + dept_code + "')";
            sqlcosts += " order by dept_name";
            DataSet dsCosts = OracleOledbBase.ExecuteDataSet(sqlcosts.ToString());
            if (dsCosts.Tables.Count > 0 && dsCosts.Tables[0] != null && dsCosts.Tables[0].Rows.Count > 0)
            {
                return dsCosts.Tables[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获得单项奖惩的明细
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable GetSimpleAwardDetail(string years, string months, string dept_code)
        {
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            DateTime deSTDate = Convert.ToDateTime(years + "-" + months + "-01");
            string sqlSA = " select DEPT_NAME,TYPE_NAME,CHECKSTAN,REMARK,round(MONEY,2) MONEY,to_char(AWARD_DATE,'yyyy-mm-dd') AWARD_DATE,INPUTER from performance.INPUT_SINGLEAWARD ";
            sqlSA += " where del_flag=0 and trunc(AWARD_DATE,'mm')=to_date('" + deSTDate.ToString("yyyyMMdd") + "','yyyymmdd') and dept_code='" + dept_code + "'";
            DataSet dsSA = OracleOledbBase.ExecuteDataSet(sqlSA.ToString());
            if (dsSA.Tables.Count > 0 && dsSA.Tables[0] != null && dsSA.Tables[0].Rows.Count > 0)
            {
                return dsSA.Tables[0];
            }
            else
            {
                return null;
            }
         
        }
        /// <summary>
        /// 获得其他奖惩明细
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable GetOtherAwardDetail(string years, string months, string dept_code)
        {
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            DateTime deSTDate = Convert.ToDateTime(years + "-" + months + "-01");
            string sqlOA = " select DEPT_NAME,OTHER_DICT_NAME,REASON,round(MONEY,2) MONEY,to_char(INPUT_DATE,'yyyy-mm-dd') INPUT_DATE,INPUTER from performance.INPUT_OTHERAWARD ";
            sqlOA += " where del_flag=0 and trunc(INPUT_DATE,'mm')=to_date('" + deSTDate.ToString("yyyyMMdd") + "','yyyymmdd') and dept_code='" + dept_code + "'";
            DataSet dsOA = OracleOledbBase.ExecuteDataSet(sqlOA.ToString());
            if (dsOA.Tables.Count > 0 && dsOA.Tables[0] != null && dsOA.Tables[0].Rows.Count > 0)
            {
                return dsOA.Tables[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 平均奖科室的人员以及奖金是否发放，出勤天数，奖金系数
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable GetAvgDeptPerson(string years, string months, string dept_code)
        {
            string sqlAvg = "select staff_name,isbonus,days,bonusmodulus from performance.SET_AVERAGEBONUSDAYS a ";
            sqlAvg += " inner join performance.SET_ACCOUNTDEPTTYPE b ";
            sqlAvg += " on  ";
            sqlAvg += " TO_DATE(a.years|| CASE WHEN LENGTH (a.months) < 2 THEN '0' || a.months ELSE a.months END || '01','yyyymmdd')=b.st_date ";
            sqlAvg += " and a.dept_id=b.dept_code ";
            sqlAvg += " and b.dept_type IN ('20001') ";
            sqlAvg += " and a.dept_id='"+dept_code+"' and a.years="+years+" and a.months="+months+"";
            DataSet dsAvg = OracleOledbBase.ExecuteDataSet(sqlAvg.ToString());
            if (dsAvg.Tables.Count > 0 && dsAvg.Tables[0] != null && dsAvg.Tables[0].Rows.Count > 0)
            {
                return dsAvg.Tables[0];
            }
            else
            {
                return null;
            }
        }
    }
}
