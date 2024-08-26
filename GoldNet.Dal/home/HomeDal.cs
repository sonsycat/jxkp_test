using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;
using System.Data.OleDb;

namespace Goldnet.Dal.home
{
    public class HomeDal
    {
        public HomeDal()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <returns></returns>
        public DataSet getDeptType(string DeptCode)
        {
            DeptCode = DeptCode == "" ? "" : string.Format("WHERE ID IN (SELECT DISTINCT DEPT_TYPE FROM {0}.SYS_DEPT_DICT WHERE SHOW_FLAG='0' and DEPT_CODE IN (" + DeptCode + "))", DataUser.COMM);

            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT   ID, ATTRIBUE DEPT_TYPE
                                   FROM {0}.SYS_DEPT_ATTR_DICT
                                   {1}
                                   ORDER BY SORTNO", DataUser.COMM, DeptCode);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="power"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataSet getDeptDict(string power, string type)
        {

            StringBuilder str = new StringBuilder();

            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";

            str.AppendFormat(@"SELECT DEPT_CODE,DEPT_NAME 
                               FROM {0}.SYS_DEPT_DICT 
                               WHERE DEPT_TYPE = {1} AND ATTR='是' AND SHOW_FLAG = '0' {2} ORDER BY SORT_NO", DataUser.COMM, type, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 取岗位信息
        /// </summary>
        /// <param name="deptcode">科室代码</param>
        /// <param name="year">岗位年度</param>
        /// <returns>返回相应的结果集dataset</returns>
        public DataSet GetStateStionBydept(string deptcode, string year)
        {
            string str = "SELECT STATION_CODE,STATION_NAME FROM COMM.SYS_STATION_BASIC_INFORMATION WHERE DEPT_CODE in (select dept_code from comm.sys_dept_dict where account_dept_code= '" + deptcode + "') AND STATION_YEAR='" + year + "' ORDER BY SEQUENCE";
            return OracleOledbBase.ExecuteDataSet(str);
        }

        /// <summary>
        /// 取岗位下的职员信息
        /// </summary>
        /// <param name="stationcode">岗位代码</param>
        /// <param name="year">岗位年度</param>
        /// <returns>返回相应的结果集dataset</returns>
        public DataSet GetStatePersonByStion(string stationcode, string year)
        {
            //string str = "select person_id,person_name from COMM.SYS_STATION_PERSONNEL_INFO where station_code='" + stationcode + "' and station_year='" + year + "'";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT NEW_STAFF_INFO.USER_ID PERSON_ID, PERSON_NAME
                              FROM {0}.SYS_STATION_PERSONNEL_INFO, {1}.NEW_STAFF_INFO
                             WHERE SYS_STATION_PERSONNEL_INFO.STATION_CODE = '{2}' AND STATION_YEAR = '{3}' 
                                   AND SYS_STATION_PERSONNEL_INFO.PERSON_ID = NEW_STAFF_INFO.STAFF_ID AND  NEW_STAFF_INFO.USER_ID IS NOT NULL", DataUser.COMM, DataUser.RLZY, stationcode, year);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 指标查询
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="stationcode"></param>
        /// <param name="personid"></param>
        /// <param name="years"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataSet GetGuides(string deptcode, string stationcode, string personid, string years, string date)
        {
            StringBuilder str = new StringBuilder();
            StringBuilder sql = new StringBuilder();
            StringBuilder organstr = new StringBuilder();
            sql.AppendFormat(@" SELECT COUNT(*) from HOSPITALSYS.STATION_GUIDE_CUSTOM WHERE AREA_CATEGORY ='00' AND STATION_CODE = '{0}'", stationcode); ;
            string cnt = OracleOledbBase.ExecuteScalar(sql.ToString()).ToString();

            organstr.AppendFormat(@" SELECT distinct b.organ
                                      FROM comm.sys_station_basic_information a, hospitalsys.station_guide_type b
                                     WHERE a.duty_order = b.station_type
                                       AND a.station_code = '{0}'
                                       AND a.station_year = '{1}'", stationcode, years); ;
            DataTable organTbl = OracleOledbBase.ExecuteDataSet(organstr.ToString()).Tables[0];

            if (organTbl.Rows.Count > 0)
            {
                if ("01".Equals(organTbl.Rows[0]["organ"].ToString()))
                {
                    deptcode = "00";
                }
            }

            if (!cnt.Equals("0"))
            {
                str.AppendFormat(@"SELECT BBB.GUIDE_TYPE ZBL,
                                           ZBDM,
                                           ZBMC,
                                           MBZ,
                                           WCZ,
                                           TQWCZ,
                                           CASE WHEN TQWCZ=0 THEN 100 ELSE (WCZ-TQWCZ)/TQWCZ*100 END ZZL,
                                           --CASE WHEN MBZ = 0 THEN '' ELSE TO_CHAR(WCBFB) END WCBFB,
                                           BBB.ORGAN,
                                           CASE WHEN BBB.ORGAN = '01' THEN NVL(CCC.DEPT_GUIDE_CODE, ZBDM) WHEN BBB.ORGAN = '02' THEN NVL(CCC.MEMBER_GUIDE_CODE, ZBDM) ELSE ZBDM END NEXTGUIDE,
                                           '{2}' PERSONID
                                      FROM (SELECT 
                                                 D.GUIDE_CODE ZBDM,
                                                 E.GUIDE_NAME ZBMC,
                                                 SUM(CASE WHEN B.STATION_YEAR ='{3}' THEN B.GUIDE_CAUSE ELSE 0 END) MBZ,
                                                 SUM(CASE WHEN C.YEARS ='{3}' THEN TO_NUMBER (C.GUIDE_VALUE) ELSE 0 END) WCZ,
                                                 SUM(CASE WHEN C.YEARS = TO_CHAR(TO_NUMBER({3})-1) THEN TO_NUMBER (C.GUIDE_VALUE) ELSE 0 END) TQWCZ,
                                                 --SUM(CASE WHEN C.YEARS ='{3}' THEN ROUND(DECODE(B.GUIDE_CAUSE ,0,0,C.GUIDE_VALUE/B.GUIDE_CAUSE),4) ELSE 0 END) AS WCBFB,
                                                 SHOW_ORDER
                                              FROM 
                                                   (SELECT * 
                                                      FROM HOSPITALSYS.GUIDE_VALUE_STATION_SUM A
                                                     WHERE (A.YEARS = '{3}' OR A.YEARS= TO_CHAR(TO_NUMBER({3})-1)) 
                                                       AND A.UNIT_CODE='{1}'
                                                   ) C,
                                                   (select a.guide_code,
                                                           round((case when guide_name like '%比%' or guide_name like '%率%' or guide_name like '%均%'
                                                                  then GUIDE_CAUSE
                                                                  else GUIDE_CAUSE/12*TO_NUMBER(TO_CHAR(SYSDATE,'mm'))
                                                             end),2) as GUIDE_CAUSE,
                                                           STATION_YEAR
                                                      from HOSPITALSYS.STATION_GUIDE_INFORMATION a,
                                                           HOSPITALSYS.GUIDE_NAME_DICT B
                                                     where A.GUIDE_CODE = B.GUIDE_CODE
                                                       AND a.STATION_CODE = '{0}'
                                                       AND a.STATION_YEAR = '{3}'
                                                   )  B,
                                                   (SELECT * 
                                                      FROM HOSPITALSYS.STATION_GUIDE_CUSTOM D 
                                                     WHERE D.AREA_CATEGORY ='00' 
                                                       AND D.STATION_CODE = '{0}'
                                                   ) D,
                                                   HOSPITALSYS.GUIDE_NAME_DICT E
                                             WHERE B.GUIDE_CODE(+) = D.GUIDE_CODE
                                               AND C.GUIDE_CODE(+) = D.GUIDE_CODE
                                               AND E.GUIDE_CODE(+) = D.GUIDE_CODE
                                               GROUP BY D.GUIDE_CODE,E.GUIDE_NAME,D.SHOW_ORDER
                                            ) AAA
                                      LEFT JOIN HOSPITALSYS.GUIDE_NAME_DICT BBB ON AAA.ZBDM = BBB.GUIDE_CODE
                                      LEFT JOIN HOSPITALSYS.GUIDE_HOSPITAL_DEPT_MEMBER CCC ON AAA.ZBDM = CCC.HOSPITAL_GUIDE_CODE OR AAA.ZBDM = CCC.DEPT_GUIDE_CODE
                                     ORDER BY SHOW_ORDER", stationcode, deptcode, personid, years, date);

                return OracleOledbBase.ExecuteDataSet(str.ToString(), new OleDbParameter[] { });
            }
            else
            {
                str.AppendFormat(@"SELECT BBB.GUIDE_TYPE ZBL,
                                           ZBDM,
                                           ZBMC,
                                           MBZ,
                                           WCZ,
                                           TQWCZ,
                                           CASE WHEN TQWCZ=0 THEN 100 ELSE (WCZ-TQWCZ)/TQWCZ*100 END ZZL,
                                           --CASE WHEN MBZ = 0 THEN '' ELSE TO_CHAR(WCBFB) END WCBFB,
                                           BBB.ORGAN,
                                           CASE WHEN BBB.ORGAN = '01' THEN
                                              NVL(CCC.DEPT_GUIDE_CODE, ZBDM)
                                             WHEN BBB.ORGAN = '02' THEN
                                              NVL(CCC.MEMBER_GUIDE_CODE, ZBDM)
                                             ELSE
                                           ZBDM END NEXTGUIDE,
                                            '{2}' PERSONID
                                      FROM (SELECT B.GUIDE_CODE ZBDM,
                                                   B.GUIDE_NAME ZBMC,
                                                   SUM(CASE WHEN B.STATION_YEAR ='{3}' THEN B.GUIDE_CAUSE ELSE 0 END) MBZ,
                                                   SUM(CASE WHEN C.YEARS ='{3}' THEN TO_NUMBER (C.GUIDE_VALUE) ELSE 0 END) WCZ,
                                                   SUM(CASE WHEN C.YEARS = TO_CHAR(TO_NUMBER({3})-1) THEN TO_NUMBER (C.GUIDE_VALUE) ELSE 0 END) TQWCZ,
                                                   --SUM(CASE WHEN C.YEARS ='{3}' THEN ROUND(DECODE(B.GUIDE_CAUSE ,0,0,C.GUIDE_VALUE/B.GUIDE_CAUSE),4) ELSE 0 END) AS WCBFB,
                                                   0 SHOW_ORDER
                                              FROM 
                                                   (SELECT * 
                                                      FROM HOSPITALSYS.GUIDE_VALUE_STATION_SUM A
                                                     WHERE (A.YEARS = '{3}' OR A.YEARS= TO_CHAR(TO_NUMBER({3})-1)) 
                                                       AND A.UNIT_CODE='{1}'
                                                   ) C,
                                                   (SELECT a.guide_code,
                                                           b.guide_name,
                                                           round((case when guide_name like '%比%' or guide_name like '%率%' or guide_name like '%均%'
                                                                       then GUIDE_CAUSE
                                                                       else GUIDE_CAUSE/12*TO_NUMBER(TO_CHAR(SYSDATE,'mm'))
                                                                  end),2) as GUIDE_CAUSE,
                                                           STATION_YEAR
                                                       FROM HOSPITALSYS.STATION_GUIDE_INFORMATION a,
                                                            HOSPITALSYS.GUIDE_NAME_DICT b
                                                      WHERE a.guide_code=b.guide_code
                                                        AND a.STATION_CODE = '{0}'
                                                        AND a.STATION_YEAR = '{3}'
                                                   ) B
                                             WHERE C.GUIDE_CODE(+) = B.GUIDE_CODE
                                             
                                             GROUP BY B.GUIDE_CODE,B.GUIDE_NAME
                                            ) AAA
                                      LEFT JOIN HOSPITALSYS.GUIDE_NAME_DICT BBB ON AAA.ZBDM = BBB.GUIDE_CODE
                                      LEFT JOIN HOSPITALSYS.GUIDE_HOSPITAL_DEPT_MEMBER CCC ON AAA.ZBDM = CCC.HOSPITAL_GUIDE_CODE OR AAA.ZBDM = CCC.DEPT_GUIDE_CODE
                                     ORDER BY SHOW_ORDER", stationcode, deptcode, personid, years, date);

                return OracleOledbBase.ExecuteDataSet(str.ToString(), new OleDbParameter[] { });
            }
        }

        /// <summary>
        /// 指标查询
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="stationcode"></param>
        /// <param name="personid"></param>
        /// <param name="guidcode"></param>
        /// <param name="year"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataSet GetGuidess(string deptcode, string stationcode, string guidcode, string year, string personid)
        {
            deptcode = guidcode.Substring(0, 1) == "1" ? "AND A.UNIT_CODE = '00'" : "AND A.UNIT_CODE = '" + deptcode + "'";

            StringBuilder str = new StringBuilder();
            //            str.AppendFormat(@"SELECT ZBL,D.GUIDE_CODE ZBDM,D.GUIDE_NAME ZBMC,NVL(MBZ,'0') MBZ,NVL(WCZ,'0') WCZ ,NVL(WCBFB,'0') WCBFB FROM
            //                                   (SELECT DISTINCT C.GUIDE_TYPE ZBL, A.GUIDE_CODE ZBDM, C.GUIDE_NAME ZBMC, B.GUIDE_CAUSE MBZ,A.GUIDE_FACT WCZ,
            //                                            ROUND(
            //                                             CASE
            //                                                WHEN C.ISHIGHGUIDE = '1' AND B.GUIDE_CAUSE != 0 AND C.ISABS = '0' THEN A.GUIDE_FACT / B.GUIDE_CAUSE
            //                                                WHEN C.ISHIGHGUIDE = '1' AND B.GUIDE_CAUSE != 0 AND C.ISABS = '1' THEN A.GUIDE_FACT / B.GUIDE_CAUSE / 365  * TO_NUMBER (TO_CHAR (TO_DATE('20110314','YYYYMMDD'), 'DDD'))
            //                                                WHEN C.ISHIGHGUIDE = '1' AND B.GUIDE_CAUSE = 0  THEN 0
            //                                                WHEN C.ISHIGHGUIDE = '0' AND A.GUIDE_FACT != 0  AND C.ISABS = '0' THEN B.GUIDE_CAUSE / A.GUIDE_FACT
            //                                                WHEN C.ISHIGHGUIDE = '0' AND A.GUIDE_FACT != 0  AND C.ISABS = '1' THEN   B.GUIDE_CAUSE / A.GUIDE_FACT / 365 * TO_NUMBER (TO_CHAR (TO_DATE('20110314','YYYYMMDD'), 'DDD'))
            //                                                WHEN C.ISHIGHGUIDE = '0' AND A.GUIDE_FACT = 0 THEN 1
            //                                             END, 4  ) AS WCBFB
            //                                     FROM {0}.ASSESS_RESULT_CURRENT A, {0}.STATION_GUIDE_INFORMATION B,{0}.GUIDE_NAME_DICT C
            //                                     WHERE A.GUIDE_CODE = C.GUIDE_CODE
            //                                       AND A.GUIDE_CODE = B.GUIDE_CODE
            //                                       AND A.GUIDE_CODE='{3}'
            //                                       AND B.STATION_CODE = '{1}'
            //                                       AND B.STATION_YEAR='{2}' 
            //                                       {4}
            //                                       ORDER BY ZBL, WCBFB DESC) E , {0}.GUIDE_NAME_DICT D
            //                             WHERE E.ZBDM(+) = D.GUIDE_CODE AND  D.GUIDE_CODE='{3}'", DataUser.HOSPITALSYS
            //                                , stationcode, year, guidcode, deptcode);

            str.AppendFormat(@"    SELECT DISTINCT 
                           (D.GUIDE_CODE) ZBDM, 
                           D.GUIDE_NAME ZBMC, 
                           NVL(B.GUIDE_CAUSE,0) MBZ,
                           NVL(A.GUIDE_FACT,C.GUIDE_VALUE) WCZ,
                           NVL(ROUND(CASE WHEN B.GUIDE_CAUSE !=0 THEN decode(B.ISHIGHGUIDE,'1', A.GUIDE_FACT/B.GUIDE_CAUSE,(DECODE(C.GUIDE_VALUE,0,0,1/NVL(A.GUIDE_FACT,C.GUIDE_VALUE)))/(1/B.GUIDE_CAUSE)) ELSE 0 END,4),0) AS WCBFB
                      FROM (SELECT * 
                              FROM HOSPITALSYS.STATION_GUIDE_ASSESS_RESULT A 
                             WHERE A.ASSESS_YEAR = '{1}' AND a.STATION_CODE = '{0}' AND A.PERSON_ID = '{4}'
                           ) A,
                           (SELECT * 
                              FROM HOSPITALSYS.GUIDE_VALUE_STATION_SUM A
                             WHERE A.YEARS = '{1}' 
                               AND GUIDE_CODE NOT IN (
                                    SELECT GUIDE_CODE 
                                      FROM HOSPITALSYS.STATION_GUIDE_ASSESS_RESULT A 
                                     WHERE A.ASSESS_YEAR = '{1}' AND a.STATION_CODE = '{0}'AND A.PERSON_ID = '{4}' )
                               {3}
                           ) C,
                           (select  a.guide_code,
                                    b1.guide_name,
                                      B1.ISHIGHGUIDE,
                                    (case when ISABS='0'
                                          then GUIDE_CAUSE
                                          else GUIDE_CAUSE/12*TO_NUMBER(TO_CHAR(SYSDATE,'mm'))
                                     end) as GUIDE_CAUSE,
                                    a.STATION_CODE  
                               from HOSPITALSYS.STATION_GUIDE_INFORMATION a,
                                    HOSPITALSYS.GUIDE_NAME_DICT b1
                              where a.guide_code=b1.guide_code
                                and a.STATION_CODE = '{0}'
                                AND a.STATION_YEAR = '{1}'
                           ) B,
                           (SELECT * 
                              FROM HOSPITALSYS.GUIDE_NAME_DICT D
                             WHERE D.GUIDE_CODE='{2}'
                           ) D
                     WHERE A.GUIDE_CODE(+) = D.GUIDE_CODE
                       AND C.GUIDE_CODE(+) = D.GUIDE_CODE
                       AND B.GUIDE_CODE(+) = D.GUIDE_CODE", stationcode, year, guidcode, deptcode, personid);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 门诊当月完成情况
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="stationcode"></param>
        /// <param name="guidcode"></param>
        /// <param name="year"></param>
        /// <param name="personid"></param>
        /// <returns></returns>
        public DataSet GetGuidess_mz(string deptcode, string stationcode, string guidcode, string year, string personid,string month)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT    a.tjyf,
                                         a.unit_code ZBMC,
                                         a.guide_code ZBDM,
                                         round(a.guide_value) WCZ,
                                         round(b.guide_cause) MBZ,
                                         CASE WHEN NVL(B.GUIDE_CAUSE,0)=0 THEN 0 ELSE ROUND(A.guide_value/NVL(B.GUIDE_CAUSE,0)*100,1) END WCBFB
                                  FROM   hospitalsys.guide_value a, HOSPITALSYS.HOS_GUIDE_INFORMATION b
                                 WHERE       a.guide_code=b.guide_code
                                         AND a.unit_code=B.DEPT_CODE
                                         AND a.guide_code = '10101004'
                                         AND a.tjyf = '{0}'||'{1}'
                                         AND a.unit_code = '00'
                                         and b.cause_year='{0}'", year, month);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 住院当月完成情况
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="stationcode"></param>
        /// <param name="guidcode"></param>
        /// <param name="year"></param>
        /// <param name="personid"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataSet GetGuidess_zy(string deptcode, string stationcode, string guidcode, string year, string personid, string month)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT    a.tjyf,
                                         a.unit_code ZBMC,
                                         a.guide_code ZBDM,
                                         round(a.guide_value) WCZ,
                                         round(b.guide_cause) MBZ,
                                         CASE WHEN NVL(B.GUIDE_CAUSE,0)=0 THEN 0 ELSE ROUND(A.guide_value/NVL(B.GUIDE_CAUSE,0)*100,1) END WCBFB
                                  FROM   hospitalsys.guide_value a, HOSPITALSYS.HOS_GUIDE_INFORMATION b
                                 WHERE       a.guide_code=b.guide_code
                                         AND a.unit_code=B.DEPT_CODE
                                         AND a.guide_code = '10111008'
                                         AND a.tjyf = '{0}'||'{1}'
                                         AND a.unit_code = '00'
                                         and b.cause_year='{0}'", year, month);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 手术当月完成情况
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="stationcode"></param>
        /// <param name="guidcode"></param>
        /// <param name="year"></param>
        /// <param name="personid"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataSet GetGuidess_ss(string deptcode, string stationcode, string guidcode, string year, string personid, string month)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT    a.tjyf,
                                         a.unit_code ZBMC,
                                         a.guide_code ZBDM,
                                         round(a.guide_value) WCZ,
                                         round(b.guide_cause) MBZ,
                                         CASE WHEN NVL(B.GUIDE_CAUSE,0)=0 THEN 0 ELSE ROUND(A.guide_value/NVL(B.GUIDE_CAUSE,0)*100,1) END WCBFB
                                  FROM   hospitalsys.guide_value a, HOSPITALSYS.HOS_GUIDE_INFORMATION b
                                 WHERE       a.guide_code=b.guide_code
                                         AND a.unit_code=B.DEPT_CODE
                                         AND a.guide_code = '10104001'
                                         AND a.tjyf = '{0}'||'{1}'
                                         AND a.unit_code = '00'
                                         and b.cause_year='{0}'", year, month);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 药占比当月完成情况
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="stationcode"></param>
        /// <param name="guidcode"></param>
        /// <param name="year"></param>
        /// <param name="personid"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataSet GetGuidess_yzb(string deptcode, string stationcode, string guidcode, string year, string personid, string month)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT    a.tjyf,
                                         a.unit_code ZBMC,
                                         a.guide_code ZBDM,
                                         round(a.guide_value) WCZ,
                                         round(b.guide_cause) MBZ,
                                         CASE WHEN NVL(B.GUIDE_CAUSE,0)=0 OR NVL(guide_value,0)=0 THEN 0 ELSE ROUND((1/NVL(A.guide_value,0))/(1/NVL(B.GUIDE_CAUSE,0))*100,1) END WCBFB
                                  FROM   hospitalsys.guide_value a, HOSPITALSYS.HOS_GUIDE_INFORMATION b
                                 WHERE       a.guide_code=b.guide_code
                                         AND a.unit_code=B.DEPT_CODE
                                         AND a.guide_code = '10101009'
                                         AND a.tjyf = '{0}'||'{1}'
                                         AND a.unit_code = '00'
                                         and b.cause_year='{0}'", year, month);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询当月经济核算数据
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="stationcode"></param>
        /// <param name="guidcode"></param>
        /// <param name="year"></param>
        /// <param name="personid"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataSet GetGuidess_jjhs(string deptcode, string stationcode, string guidcode, string year, string personid, string month)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT   CASE WHEN GUIDE_CODE='20204075' THEN '对外实收' 
                                              WHEN GUIDE_CODE='20203021' THEN '军人计价'
                                              WHEN GUIDE_CODE='20201007' THEN '门诊总收'
                                              WHEN GUIDE_CODE='20202017' THEN '住院总收'
                                              WHEN GUIDE_CODE='20112011' THEN '成本'
                                              WHEN GUIDE_CODE='20207001' THEN '药品收益'
                                              WHEN GUIDE_CODE='20207002' THEN '核算收益'
                                            ELSE '' 
                                         END ZBMC,
                                         SUM(CASE WHEN TJYF='{0}{1}' THEN NVL(GUIDE_VALUE,0) ELSE 0 END) WCZ,
                                         SUM(CASE WHEN TJYF=TO_CHAR(ADD_MONTHS(TO_DATE('{0}{1}','yyyymm'),-12),'yyyymm') THEN NVL(GUIDE_VALUE,0) ELSE 0 END) TQWCZ,
                                         SUM(CASE WHEN TJYF=TO_CHAR(ADD_MONTHS(TO_DATE('{0}{1}','yyyymm'),-1),'yyyymm') THEN NVL(GUIDE_VALUE,0) ELSE 0 END) ZZL
                                  FROM   HOSPITALSYS.GUIDE_VALUE A
                                 WHERE   (TJYF = '{0}{1}' OR TJYF = TO_CHAR(ADD_MONTHS(TO_DATE('{0}{1}','yyyymm'),-12),'yyyymm') OR TJYF=TO_CHAR(ADD_MONTHS(TO_DATE('{0}{1}','yyyymm'),-1),'yyyymm'))
                                 AND GUIDE_CODE IN ('20204075','20203021','20201007','20202017','20112011','20207001','20207002')
                                 
                                GROUP BY GUIDE_CODE", year, month);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询当月工作量
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="stationcode"></param>
        /// <param name="guidcode"></param>
        /// <param name="year"></param>
        /// <param name="personid"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataSet GetGuidess_gzl(string deptcode, string stationcode, string guidcode, string year, string personid, string month)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT * FROM (SELECT   CASE WHEN GUIDE_CODE='10101004' THEN '门诊量' 
                                              WHEN GUIDE_CODE='10101012' THEN '出诊人次'
                                              WHEN GUIDE_CODE='20208010' THEN '收治人次'
                                              WHEN GUIDE_CODE='20104001' THEN '手术量'
                                              WHEN GUIDE_CODE='20105004' THEN '占床总日数'
                                              WHEN GUIDE_CODE='20105005' THEN '危重床日数'
                                            ELSE '' 
                                         END ZBMC,
                                         CASE WHEN GUIDE_CODE='10101004' THEN 1 
                                              WHEN GUIDE_CODE='10101012' THEN 2
                                              WHEN GUIDE_CODE='20208010' THEN 3
                                              WHEN GUIDE_CODE='20104001' THEN 4
                                              WHEN GUIDE_CODE='20105004' THEN 5
                                              WHEN GUIDE_CODE='20105005' THEN 6
                                            ELSE 0 
                                         END ZBL,
                                         SUM(CASE WHEN TJYF='{0}{1}' THEN NVL(GUIDE_VALUE,0) ELSE 0 END) WCZ,
                                         SUM(CASE WHEN TJYF=TO_CHAR(ADD_MONTHS(TO_DATE('{0}{1}','yyyymm'),-12),'yyyymm') THEN NVL(GUIDE_VALUE,0) ELSE 0 END) TQWCZ,
                                         SUM(CASE WHEN TJYF=TO_CHAR(ADD_MONTHS(TO_DATE('{0}{1}','yyyymm'),-1),'yyyymm') THEN NVL(GUIDE_VALUE,0) ELSE 0 END) ZZL
                                  FROM   HOSPITALSYS.GUIDE_VALUE A
                                 WHERE   (TJYF = '{0}{1}' OR TJYF = TO_CHAR(ADD_MONTHS(TO_DATE('{0}{1}','yyyymm'),-12),'yyyymm') OR TJYF=TO_CHAR(ADD_MONTHS(TO_DATE('{0}{1}','yyyymm'),-1),'yyyymm'))
                                 AND GUIDE_CODE IN ('10101004','10101012','20208010','20104001','20105004','20105005')
                                GROUP BY GUIDE_CODE) ORDER BY ZBL", year, month);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询当月质量效率数据
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="stationcode"></param>
        /// <param name="guidcode"></param>
        /// <param name="year"></param>
        /// <param name="personid"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataSet GetGuidess_zlxl(string deptcode, string stationcode, string guidcode, string year, string personid, string month)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT   CASE WHEN GUIDE_CODE='10110001' THEN '平均住院日' 
                                              WHEN GUIDE_CODE='10111006' THEN '床位使用率'
                                              WHEN GUIDE_CODE='10110005' THEN '术前平均住院日'
                                              WHEN GUIDE_CODE='10101009' THEN '药占比'
                                            ELSE '' 
                                         END ZBMC,
                                         SUM(CASE WHEN TJYF='{0}{1}' THEN NVL(GUIDE_VALUE,0) ELSE 0 END) WCZ,
                                         SUM(CASE WHEN TJYF=TO_CHAR(ADD_MONTHS(TO_DATE('{0}{1}','yyyymm'),-12),'yyyymm') THEN NVL(GUIDE_VALUE,0) ELSE 0 END) TQWCZ,
                                         SUM(CASE WHEN TJYF=TO_CHAR(ADD_MONTHS(TO_DATE('{0}{1}','yyyymm'),-1),'yyyymm') THEN NVL(GUIDE_VALUE,0) ELSE 0 END) ZZL
                                  FROM   HOSPITALSYS.GUIDE_VALUE A
                                 WHERE   (TJYF = '{0}{1}' OR TJYF = TO_CHAR(ADD_MONTHS(TO_DATE('{0}{1}','yyyymm'),-12),'yyyymm') OR TJYF=TO_CHAR(ADD_MONTHS(TO_DATE('{0}{1}','yyyymm'),-1),'yyyymm'))
                                 AND GUIDE_CODE IN ('10110001','10111006','10110005','10101009')
                                 
                                GROUP BY GUIDE_CODE", year, month);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 指标查询
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="stationcode"></param>
        /// <param name="personid"></param>
        /// <param name="guidcode"></param>
        /// <param name="year"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataSet GetGuides(string deptcode, string personid, string guidcode, string years)
        {
            if (guidcode.Substring(0, 1) == "2")
            {
                deptcode = "AND D.UNIT_CODE=B.DEPT_CODE";
            }
            if (guidcode.Substring(0, 1) == "3")
            {
                deptcode = "AND D.UNIT_CODE IN (SELECT STAFF_ID  FROM RLZY.NEW_STAFF_INFO WHERE DEPT_CODE =  '" + deptcode + "')";
            }

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"  SELECT C.GUIDE_TYPE ZBL,
                                C.GUIDE_CODE ZBDM, 
                                C.GUIDE_NAME ZBMC,
                                D.GUIDE_VALUE WCZ,
                                B.DEPT_NAME DEPTNAME
                            FROM 
                            (
                                SELECT * 
                                  FROM HOSPITALSYS.GUIDE_VALUE_STATION_SUM A 
                                 WHERE A.YEARS = '{2}' AND A.GUIDE_CODE='{0}'
                            ) D,
                            COMM.SYS_DEPT_DICT B,
                            HOSPITALSYS.GUIDE_NAME_DICT C
                            WHERE  D.GUIDE_CODE = C.GUIDE_CODE AND D.GUIDE_VALUE <>0 {1}", guidcode, deptcode, years);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 指标月趋势
        /// </summary>
        /// <param name="guidecode"></param>
        /// <param name="datetime"></param>
        /// <param name="unitcode"></param>
        /// <returns></returns>
        public DataSet GetTrendLine(string guidecode, string datetime, string unitcode)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT   A.*, B.GUIDE_NAME
                                FROM {3}.GUIDE_VALUE A, {3}.GUIDE_NAME_DICT B
                               WHERE A.GUIDE_CODE = B.GUIDE_CODE
                                 AND A.GUIDE_CODE = '{0}'
                                 AND SUBSTR (A.TJYF, 0, 4) = '{1}'
                                 AND A.UNIT_CODE = '{2}'
                            ORDER BY TJYF", guidecode, datetime, unitcode, DataUser.HOSPITALSYS);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取图表数据
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="stationcode"></param>
        /// <param name="personid"></param>
        /// <param name="years"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataSet GetMzl(string deptcode, string stationcode, string personid, string years, String area)
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
            SELECT CCC.GUIDE_NAME ZBMC,
                   AAA.ZBDM,
                   AAA.WCZ,
                   AAA.BT,
                   AAA.BS,
                   AAA.SHOW_ORDER
              FROM (SELECT D.GUIDE_CODE ZBDM,
                           TO_NUMBER(NVL(A.GUIDE_FACT, 0)) WCZ,
                           D.SHOW_TITLE BT,
                           D.SHOW_FLAG BS,
                           D.SHOW_ORDER
                      FROM HOSPITALSYS.STATION_GUIDE_CUSTOM D,
                           (SELECT *
                              FROM HOSPITALSYS.STATION_GUIDE_ASSESS_RESULT A
                             WHERE A.ASSESS_YEAR = '{3}'
                               AND A.PERSON_ID = '{2}') A
                     WHERE D.GUIDE_CODE = A.GUIDE_CODE(+)
                       AND D.STATION_CODE = A.STATION_CODE(+)
                       AND D.AREA_CATEGORY = '{4}'
                       AND D.STATION_CODE = '{0}') AAA,
                   HOSPITALSYS.STATION_GUIDE_INFORMATION BBB,
                   HOSPITALSYS.GUIDE_NAME_DICT CCC
             WHERE AAA.ZBDM = CCC.GUIDE_CODE
               AND AAA.ZBDM = BBB.GUIDE_CODE
               AND BBB.STATION_CODE = '{0}'
               AND BBB.STATION_YEAR='{3}'
             ORDER BY AAA.SHOW_ORDER", stationcode, deptcode, personid, years, area);

            return OracleOledbBase.ExecuteDataSet(str.ToString(), new OleDbParameter[] { });
        }

        /// <summary>
        /// 查询指标BSC分类，构造树形结构
        /// </summary>
        /// <param name="stationCode"></param>
        /// <returns></returns>
        public DataSet getBSCGuideTree(string stationCode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" 
                SELECT AA.NAME,AA.ID,AA.PID FROM(
                       SELECT  C.NAME,
                               C.ID,
                               LEVEL GLEVEL,
                               CONNECT_BY_ISLEAF LEAF ,
                               C.PID
                         FROM ( 
                            SELECT BSC_CLASS_NAME NAME, 
                                   BSC_CLASS_CODE ID ,  
                                   DECODE( LENGTH(B.BSC_CLASS_CODE) ,2 ,'0', SUBSTR(B.BSC_CLASS_CODE,1,LENGTH(B.BSC_CLASS_CODE)- 2)   ) AS PID,
                                   '' DEPT,
                                   '' ORGAN
                              FROM hospitalsys.JXGL_GUIDE_BSC_CLASS_DICT   B 
                              UNION ALL
                            SELECT A.GUIDE_NAME NAME , 
                                   A.GUIDE_CODE CODE ,
                                   A.BSC PID , 
                                   A.DEPT,
                                   A.ORGAN
                              FROM hospitalsys.GUIDE_NAME_DICT A
                             WHERE A.ISPAGE = '1'
                           ) C  
                      START WITH PID= '0'  CONNECT BY PRIOR ID = PID 
                      )AA
                 WHERE AA.GLEVEL =1 OR (AA.GLEVEL = 2 AND  AA.LEAF = 0) ", stationCode);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取包含在指标集中的指标树
        /// </summary>
        /// <param name="stationCode"></param>
        /// <returns></returns>
        public DataSet getBSCGuideTreeAllGathers(string stationCode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" 
                SELECT AA.NAME,AA.ID,AA.PID,AA.GLEVEL,AA.LEAF FROM(
                       SELECT  C.NAME,
                               C.ID,
                               LEVEL GLEVEL,
                               CONNECT_BY_ISLEAF LEAF ,
                               C.PID
                         FROM ( 
                            SELECT BSC_CLASS_NAME NAME, 
                                   BSC_CLASS_CODE ID ,  
                                   DECODE( LENGTH(B.BSC_CLASS_CODE) ,2 ,'0', SUBSTR(B.BSC_CLASS_CODE,1,LENGTH(B.BSC_CLASS_CODE)- 2)   ) AS PID,
                                   '' DEPT,
                                   '' ORGAN
                              FROM hospitalsys.JXGL_GUIDE_BSC_CLASS_DICT   B 
                              UNION ALL
                            SELECT A.GUIDE_NAME NAME , 
                                   A.GUIDE_CODE CODE ,
                                   A.BSC PID , 
                                   A.DEPT,
                                   A.ORGAN
                              FROM hospitalsys.GUIDE_NAME_DICT A
                             WHERE A.GUIDE_CODE IN (select distinct guide_code from hospitalsys.GUIDE_GATHERS)
                           ) C 
                      WHERE LEVEL <> 2 OR CONNECT_BY_ISLEAF <> 1 
                      START WITH PID= '0'  CONNECT BY PRIOR ID = PID 
                      )AA ", stationCode);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询指标BSC分类，构造树形结构
        /// </summary>
        /// <param name="stationCode"></param>
        /// <returns></returns>
        public DataSet getBSCGuideTreeSelect(string stationCode, string area)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" 
                SELECT   AA.SHOW_TITLE AS SHOW_TITLE,
         AA.SHOW_FLAG AS SHOW_FLAG,
         AA.NAME,
         AA.ID,
         AA.PID
  FROM   (    SELECT   C.SHOW_TITLE AS SHOW_TITLE,
                       C.SHOW_FLAG AS SHOW_FLAG,
                       C.NAME,
                       C.ID,
                       LEVEL GLEVEL,
                       CONNECT_BY_ISLEAF LEAF,
                       C.PID,
                       SHOW_ORDER
                FROM   (SELECT   '' AS SHOW_TITLE,
                                 '' AS SHOW_FLAG,
                                 BSC_CLASS_NAME NAME,
                                 BSC_CLASS_CODE ID,
                                 DECODE (
                                    LENGTH (B.BSC_CLASS_CODE),
                                    2,
                                    '0',
                                    SUBSTR (B.BSC_CLASS_CODE,
                                            1,
                                            LENGTH (B.BSC_CLASS_CODE) - 2)
                                 )
                                    AS PID,
                                 '' DEPT,
                                 '' ORGAN,
                                 999 SHOW_ORDER
                          FROM   hospitalsys.JXGL_GUIDE_BSC_CLASS_DICT B
                        UNION ALL
                        SELECT   C.SHOW_TITLE AS SHOW_TITLE,
                                 C.SHOW_FLAG AS SHOW_FLAG,
                                 A.GUIDE_NAME NAME,
                                 A.GUIDE_CODE CODE,
                                 A.BSC PID,
                                 A.DEPT,
                                 A.ORGAN,
                                 C.SHOW_ORDER
                          FROM   hospitalsys.GUIDE_NAME_DICT A,
                                 HOSPITALSYS.STATION_GUIDE_CUSTOM C
                         WHERE       A.GUIDE_CODE = C.GUIDE_CODE
                                 AND A.ISPAGE = '1'
                                 AND C.STATION_CODE = '{0}'
                                 AND C.AREA_CATEGORY = '{1}'
                                 )
                       C
          START WITH   PID = '0'
          CONNECT BY   PRIOR ID = PID) AA
 WHERE   AA.GLEVEL = 3 ORDER BY SHOW_ORDER", stationCode, area);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stationCode"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public DataSet getBSCGuideTreeSelect()
        {
            StringBuilder str = new StringBuilder();
            str.Append(@" 
                SELECT A.GUIDE_CODE ID,B.GUIDE_NAME GUIDENAME,A.GUIDE_TYPE GUIDETYPE,A.SHOW_WIDTH SHOWWIDTH,A.SHOW_STYLE SHOWSTYLE
                  FROM hospitalsys.guide_year_cause_dict a, hospitalsys.guide_name_dict b
                 WHERE A.GUIDE_CODE=B.GUIDE_CODE");

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取岗位年度
        /// </summary>
        /// <param name="stationCode"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public DataSet getStationYear()
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"SELECT DISTINCT station_year year
                               FROM comm.sys_station_basic_information
                           ORDER BY station_year DESC");

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取年度岗位
        /// </summary>
        /// <returns></returns>
        public DataSet getStationByYear(string year, string deptstr)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT DISTINCT A.station_code stationcode, B.DEPT_NAME||'-'||A.station_name stationname
                               FROM comm.sys_station_basic_information A,COMM.SYS_DEPT_DICT B
                              WHERE A.DEPT_CODE= B.DEPT_CODE
                                AND A.station_year = '{0}' {1}
                    ORDER BY station_code", year, deptstr);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stationCode"></param>
        /// <returns></returns>
        public DataSet getBSCGuideTreeAll(string nodeID, string stationCode, string years)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" 
                SELECT AA.NAME,AA.ID,AA.PID FROM(
                       SELECT  C.NAME,C.ID,LEVEL GLEVEL,CONNECT_BY_ISLEAF LEAF ,C.PID
                         FROM ( 
                            SELECT BSC_CLASS_NAME NAME, BSC_CLASS_CODE ID ,  
                                   DECODE( LENGTH(B.BSC_CLASS_CODE) ,2 ,'0', SUBSTR(B.BSC_CLASS_CODE,1,LENGTH(B.BSC_CLASS_CODE)- 2)   ) AS PID,
                                   '' DEPT,'' ORGAN
                              FROM hospitalsys.JXGL_GUIDE_BSC_CLASS_DICT   B 
                              UNION ALL
                            SELECT A.GUIDE_NAME NAME , A.GUIDE_CODE CODE ,A.BSC PID , A.DEPT,A.ORGAN
                              FROM hospitalsys.GUIDE_NAME_DICT A,
                                  (select distinct B.ORGAN from comm.SYS_STATION_BASIC_INFORMATION a,HOSPITALSYS.STATION_GUIDE_TYPE B where A.DUTY_ORDER =B.STATION_TYPE AND A.station_code='{0}' and A.station_year='{2}') b
                             WHERE A.ISPAGE = '1'
                               AND A.ORGAN = B.ORGAN 
                           ) C 
                      START WITH PID= '0' CONNECT BY PRIOR ID = PID 
                      )AA
                 WHERE AA.GLEVEL =3
                 AND AA.PID = '{1}'", stationCode, nodeID, years);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="stationCode"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        public DataSet getBSCGuideTreeAll(string nodeID)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" 
                SELECT AA.ID,AA.NAME GUIDENAME,AA.PID,AA.GUIDETYPE,100 SHOWWIDTH FROM(
                       SELECT  C.NAME,C.ID,LEVEL GLEVEL,CONNECT_BY_ISLEAF LEAF ,C.PID,C.GUIDETYPE
                         FROM ( 
                            SELECT BSC_CLASS_NAME NAME, BSC_CLASS_CODE ID ,  
                                   DECODE( LENGTH(B.BSC_CLASS_CODE) ,2 ,'0', SUBSTR(B.BSC_CLASS_CODE,1,LENGTH(B.BSC_CLASS_CODE)- 2)   ) AS PID,
                                   '' DEPT,'' ORGAN,'' GUIDETYPE
                              FROM hospitalsys.JXGL_GUIDE_BSC_CLASS_DICT   B 
                              UNION ALL
                            SELECT A.GUIDE_NAME NAME , A.GUIDE_CODE CODE ,A.BSC PID , A.DEPT,A.ORGAN,DECODE(SUBSTR(GUIDE_CODE,1,1),'1','Y','2','K','3','R','4','Z') GUIDETYPE
                              FROM hospitalsys.GUIDE_NAME_DICT A
                             WHERE A.ISPAGE = '1'
                           ) C 
                      START WITH PID= '0' CONNECT BY PRIOR ID = PID 
                      )AA
                 WHERE AA.GLEVEL =3
                 AND AA.PID = '{0}'", nodeID);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取指标集中包含的所有指标
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public DataSet getBSCGuideTreeByGathers(string nodeID, string stationyear)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" 
                                SELECT   B.*, C.DEPT_NAME
                  FROM   (  SELECT   A.DEPT_CODE,
                                     GATHER_CODE,
                                     STATION_YEAR,
                                     GUIDE_CODE
                              FROM   hospitalsys.DEPT_GATHERS A, hospitalsys.GUIDE_GATHERS B
                             WHERE       A.GATHER_CODE = GUIDE_GATHER_CODE
                                     AND A.STATION_YEAR = '{1}'
                                     AND B.GUIDE_CODE = '{0}'
                          ORDER BY   DEPT_CODE) A,
                         HOSPITALSYS.DEPT_GUIDE_INFORMATION B,
                         COMM.SYS_DEPT_DICT C
                 WHERE       A.DEPT_CODE = B.DEPT_CODE(+)
                         AND A.GUIDE_CODE = B.GUIDE_CODE(+)
                         AND A.DEPT_CODE = C.DEPT_CODE(+)
                         AND B.station_year = '{1}'", nodeID, stationyear);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guidegatherid"></param>
        /// <param name="selectedRow"></param>
        /// <returns></returns>
        public string updateBSCGuide(string stationCode, string deptType, string showTitle, string showFlag, Dictionary<string, string>[] selectedRow)
        {
            string resultstr = "";
            MyLists SQLStringList = new MyLists();
            List sqlList = new List();
            string sqlstr = string.Format(" DELETE FROM {0}.STATION_GUIDE_CUSTOM WHERE STATION_CODE = '{1}' AND AREA_CATEGORY = '{2}'", DataUser.HOSPITALSYS, stationCode, deptType);
            sqlList.StrSql = sqlstr;
            SQLStringList.Add(sqlList);
            if (selectedRow.Length > 0)
            {
                List sqlList2 = new List();
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat(@" INSERT INTO {0}.STATION_GUIDE_CUSTOM (STATION_CODE,AREA_CATEGORY,GUIDE_CODE,SHOW_TITLE,SHOW_FLAG,SHOW_ORDER ) ", DataUser.HOSPITALSYS);

                for (int i = 0; i < selectedRow.Length; i++)
                {
                    string guide_stationCode = stationCode;
                    string guide_deptType = deptType;
                    string guide_id = selectedRow[i]["ID"].Trim();
                    string guide_showTitle = showTitle;
                    string guide_showFlag = showFlag;
                    sql.AppendFormat(@" SELECT  '{0}' AS STATION_CODE,'{1}' AS AREA_CATEGORY, '{2}' AS GUIDE_CODE, '{3}' AS SHOW_TITLE, {4} AS SHOW_FLAG,{5} AS SHOW_ORDER FROM DUAL UNION ALL ", guide_stationCode, guide_deptType, guide_id, guide_showTitle, guide_showFlag, i);
                }
                sql.AppendFormat("END");
                sqlList2.StrSql = sql.ToString().Replace("UNION ALL END", "");
                SQLStringList.Add(sqlList2);
            }
            OracleOledbBase.ExecuteSqlTranList(OracleOledbBase.ConnString, SQLStringList);
            return resultstr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedRow"></param>
        /// <returns></returns>
        public string updateBSCGuide(Dictionary<string, string>[] selectedRow)
        {
            string resultstr = "";
            MyLists SQLStringList = new MyLists();
            List sqlList = new List();
            string sqlstr = string.Format(" DELETE FROM {0}.GUIDE_YEAR_CAUSE_DICT ", DataUser.HOSPITALSYS);
            sqlList.StrSql = sqlstr;
            SQLStringList.Add(sqlList);

            if (selectedRow.Length > 0)
            {
                List sqlList2 = new List();
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat(@" INSERT INTO {0}.GUIDE_YEAR_CAUSE_DICT (GUIDE_CODE,GUIDE_TYPE,SHOW_WIDTH) ", DataUser.HOSPITALSYS);

                for (int i = 0; i < selectedRow.Length; i++)
                {
                    string guide_id = selectedRow[i]["ID"].Trim();
                    string guide_type = selectedRow[i]["GUIDETYPE"].Trim();
                    string guide_showwidth = "";
                    if (selectedRow[i]["SHOWWIDTH"] == null)
                    {
                        guide_showwidth = "100";
                    }
                    else
                    {
                        guide_showwidth = selectedRow[i]["SHOWWIDTH"].Trim();
                    }
                    if (guide_showwidth.Equals(""))
                    {
                        guide_showwidth = "100";
                    }

                    sql.AppendFormat(@" SELECT '{0}' AS GUIDE_CODE,'{1}' AS GUIDE_TYPE, '{2}' AS SHOW_WIDTH FROM DUAL UNION ALL ", guide_id, guide_type, guide_showwidth);
                }
                sql.AppendFormat("END");
                sqlList2.StrSql = sql.ToString().Replace("UNION ALL END", "");
                SQLStringList.Add(sqlList2);
            }
            OracleOledbBase.ExecuteSqlTranList(OracleOledbBase.ConnString, SQLStringList);
            return resultstr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="years"></param>
        /// <returns></returns>
        public string CreateStationGuide(string years, string incount, string startdate, string enddate)
        {
            string rtn = "数据生成成功！";

            //生成评价数据
            try
            {
                OleDbParameter[] parameters = {
                    new OleDbParameter("startdate",startdate),
                    new OleDbParameter("enddate",enddate),
                    new OleDbParameter("guideyear",years),
                    new OleDbParameter("incount", "admin")  };
                OracleOledbBase.RunProcedure(DataUser.HOSPITALSYS + ".STATION_GUIDE_ASSESS_COMPUTE", parameters);
            }
            catch (Exception ee)
            {
                rtn = "评价数据生成失败！<br/>原因：" + ee.Message;
            }

            return rtn;
        }

    }
}
