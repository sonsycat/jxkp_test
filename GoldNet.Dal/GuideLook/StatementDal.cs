using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    public class StatementDal
    {
        public StatementDal()
        { }

        /// <summary>
        /// 查询现有类别
        /// </summary>
        /// <returns></returns>
        public DataSet getCalcSchm(string TableName)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   ITEM_CLASS, ITEM_NAME
                                FROM {0}.CBHS_DISTRIBUTION_CALC_SCHM
                                WHERE ITEM_CLASS IN (SELECT DISTINCT ITEM_CLASS
                                                               FROM {1}.{2})
                                ORDER BY ITEM_CLASS", DataUser.CBHS, DataUser.HISFACT, TableName);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 未结算收入
        /// </summary>
        /// <param name="start_dae"></param>
        /// <param name="end_date"></param>
        /// <param name="type"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetDeptIncomeNoSettle(string fromDate, string ToDate, string type, string power, DataTable dt, string TableName)
        {
            power = power == "" ? "" : "AND B.DEPT_CODE IN(" + power + ")";

            StringBuilder str1 = new StringBuilder();
            StringBuilder str2 = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str1.AppendFormat(",NVL(SUM(DECODE(A.ITEM_CLASS,'{0}',{2},0.00)),0.00) AS \"{1}\"", dt.Rows[i]["item_class"].ToString().Trim(), dt.Rows[i]["item_name"].ToString().Trim(), type);
                str2.AppendFormat(",SUM (AA.\"{0}\")", dt.Rows[i]["item_name"].ToString().Trim());
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                             SELECT AA.*
                                          FROM (SELECT B.DEPT_NAME 科室名称, A.*
                                  FROM (SELECT  C.ACCOUNT_DEPT_CODE 科室编码, SUM ({6}) AS 合计 {7}
                                            FROM {0}.{9} A,
                                                 {1}.CBHS_DISTRIBUTION_CALC_SCHM B,
                                                 {3}.SYS_DEPT_DICT C
                                           WHERE A.ITEM_CLASS = B.ITEM_CLASS
                                             AND DISCHARGE_DATE_TIME >= TO_DATE ('{4}', 'yyyyMMdd')
                                             AND DISCHARGE_DATE_TIME <= TO_DATE ('{5}', 'yyyyMMdd')
                                             AND A.DEPT_CODE = C.DEPT_CODE
                                        GROUP BY C.ACCOUNT_DEPT_CODE) A,
                                       {3}.SYS_DEPT_DICT B
                                 WHERE A.科室编码 = B.DEPT_CODE {2}) AA
                                        UNION ALL
                                        SELECT '合计' AS 科室名称, '99999999' AS 科室编码, SUM (AA.合计) {8}
                                          FROM (SELECT B.DEPT_NAME 科室名称, A.*
                                  FROM (SELECT  C.ACCOUNT_DEPT_CODE 科室编码, SUM ({6}) AS 合计 {7}
                                            FROM {0}.{9} A,
                                                 {1}.CBHS_DISTRIBUTION_CALC_SCHM B,
                                                 {3}.SYS_DEPT_DICT C
                                           WHERE A.ITEM_CLASS = B.ITEM_CLASS
                                             AND DISCHARGE_DATE_TIME >= TO_DATE ('{4}', 'yyyyMMdd')
                                             AND DISCHARGE_DATE_TIME <= TO_DATE ('{5}', 'yyyyMMdd')
                                             AND A.DEPT_CODE = C.DEPT_CODE
                                        GROUP BY C.ACCOUNT_DEPT_CODE) A,
                                       {3}.SYS_DEPT_DICT B
                                 WHERE A.科室编码 = B.DEPT_CODE {2}) AA", DataUser.HISFACT, DataUser.CBHS, power, DataUser.COMM, fromDate, ToDate, type, str1.ToString(), str2.ToString(), TableName);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 未结算病人费用
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="dept_code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataSet GetPerIncomeNoSettle(string fromDate, string ToDate, string deptCode, string type, DataTable dt, string TableName)
        {

            StringBuilder str1 = new StringBuilder();
            StringBuilder str2 = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str1.AppendFormat(",NVL(SUM(DECODE(A.ITEM_CLASS,'{0}',{2},0)),0.00) AS \"{1}\"", dt.Rows[i]["item_class"].ToString().Trim(), dt.Rows[i]["item_name"].ToString().Trim(), type);
                str2.AppendFormat(",SUM (AA.\"{0}\")", dt.Rows[i]["item_name"].ToString().Trim());
            }

            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"
                                    SELECT AA.* FROM ( SELECT   C.DEPT_NAME 科室名称, A.NAME 姓名, TO_CHAR(A.PATIENT_ID) ID号,
                                             TO_CHAR(A.INP_NO) 住院号,
                                             (SELECT CC.DEPT_NAME FROM {2}.SYS_DEPT_DICT CC,{0}.{9} DD WHERE CC.DEPT_CODE=DD.DEPT_DISCHARGE_FROM) AS 出院科室,
                                             TO_CHAR (A.ADMISSION_DATE_TIME, 'yyyy-mm-dd hh24:MM:ss') AS 入院时间,
                                             TO_CHAR (A.DISCHARGE_DATE_TIME, 'yyyy-mm-dd hh24:MM:ss') AS 出院时间,
                                             TO_CHAR(A.CHARGE_TYPE) AS 费别, SUM ({3}) AS 合计 {7}
                                        FROM {0}.{9} A,
                                             {1}.CBHS_DISTRIBUTION_CALC_SCHM B,
                                             {2}.SYS_DEPT_DICT C
                                       WHERE A.ITEM_CLASS = B.ITEM_CLASS
                                         AND A.DEPT_CODE = C.DEPT_CODE
                                         AND {3} <> 0
                                         AND C.ACCOUNT_DEPT_CODE = '{4}'
                                         AND A.DISCHARGE_DATE_TIME >= TO_DATE ('{5}', 'yyyyMMdd')
                                         AND A.DISCHARGE_DATE_TIME <= TO_DATE ('{6}', 'yyyyMMdd')
                                    GROUP BY C.DEPT_NAME,
                                             NAME,
                                             A.ADMISSION_DATE_TIME,
                                             A.DISCHARGE_DATE_TIME,
                                             A.CHARGE_TYPE,
                                             A.PATIENT_ID,
                                             A.INP_NO) AA 
                                    UNION ALL 
                                    SELECT '合计' AS 科室名称,'' AS 姓名,'' AS ID号,'' AS 住院号,'' AS 出院科室, '' AS 入院时间,'' AS 出院时间,
                                            '' AS 费别,SUM(AA.合计) {8} FROM ( SELECT   C.DEPT_NAME 科室名称, A.NAME 姓名, TO_CHAR(A.PATIENT_ID) ID号,
                                             TO_CHAR(A.INP_NO) 住院号,
                                             (SELECT CC.DEPT_NAME FROM {2}.SYS_DEPT_DICT CC,{0}.{9} DD WHERE CC.DEPT_CODE=DD.DEPT_DISCHARGE_FROM) AS 出院科室,
                                             TO_CHAR (A.ADMISSION_DATE_TIME, 'yyyy-mm-dd hh24:MM:ss') AS 入院时间,
                                             TO_CHAR (A.DISCHARGE_DATE_TIME, 'yyyy-mm-dd hh24:MM:ss') AS 出院时间,
                                             TO_CHAR(A.CHARGE_TYPE) AS 费别, SUM ({3}) AS 合计 {7}
                                        FROM {0}.{9} A,
                                             {1}.CBHS_DISTRIBUTION_CALC_SCHM B,
                                             {2}.SYS_DEPT_DICT C
                                       WHERE A.ITEM_CLASS = B.ITEM_CLASS
                                         AND A.DEPT_CODE = C.DEPT_CODE
                                         AND {3} <> 0
                                         AND C.ACCOUNT_DEPT_CODE = '{4}'
                                         AND A.DISCHARGE_DATE_TIME >= TO_DATE ('{5}', 'yyyyMMdd')
                                         AND A.DISCHARGE_DATE_TIME <= TO_DATE ('{6}', 'yyyyMMdd')
                                    GROUP BY C.DEPT_NAME,
                                             NAME,
                                             A.ADMISSION_DATE_TIME,
                                             A.DISCHARGE_DATE_TIME,
                                             A.CHARGE_TYPE,
                                             A.PATIENT_ID,
                                             A.INP_NO) AA", DataUser.HISFACT, DataUser.CBHS, DataUser.COMM, type, deptCode, fromDate, ToDate, str1.ToString(), str2.ToString(), TableName);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 全院收治一览表
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public DataSet GetDeptBudget(string fromDate, string ToDate, string power)
        {
            power = power == "" ? "" : "AND B.DEPT_CODE IN(" + power + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"															
                            SELECT AA.*																	
                              FROM (SELECT B.DEPT_NAME 科室名称, A.*																	
                                    FROM (SELECT   B.ACCOUNT_DEPT_CODE AS 科室编码, SUM (A.ZJ) AS 总计,																	
                                                   SUM (A.JDXJ) 军队小计, SUM (A.DFXJ) 地方小计,																	
                                                   SUM (A.JYSGB) 军以上干部, SUM (A.SZGB) 师职干部,																	
                                                   SUM (A.TYXGB) 团以下干部, SUM (A.ZS) 战士,																	
                                                   SUM (A.ZG) 职工, SUM (A.MJFJS) 免减费家属,																	
                                                   SUM (A.YB) 医疗保险, SUM (A.XNH) 新农合,																	
                                                   SUM (A.YBRY) 一般人员, SUM (A.QT) 其他																	
                                             FROM {0}.DEPT_PAT_IN A, {1}.SYS_DEPT_DICT B																	
                                             WHERE A.DEPT_CODE = B.DEPT_CODE																	
                                                   AND A.ST_DATE >= TO_DATE ('{2}', 'YYYYMM')																	
                                                   AND A.ST_DATE <= TO_DATE ('{3}', 'YYYYMM')																	
                                             GROUP BY B.ACCOUNT_DEPT_CODE) A,																	
                                         {1}.SYS_DEPT_DICT B																	
                                   WHERE A.科室编码 = B.DEPT_CODE {4}) AA																	
                            UNION ALL																	
                            SELECT '合计' 科室名称, '999999' AS 科室编码, SUM (AA.总计),																	
                                   SUM (AA.军队小计), SUM (AA.地方小计), SUM (AA.军以上干部),																	
                                   SUM (AA.师职干部), SUM (AA.团以下干部), SUM (AA.战士), SUM (AA.职工),																	
                                   SUM (AA.免减费家属), SUM (AA.医疗保险), SUM (AA.新农合),																	
                                   SUM (AA.一般人员), SUM (AA.其他)																	
                              FROM (SELECT B.DEPT_NAME 科室名称, A.*																	
                                    FROM (SELECT   B.ACCOUNT_DEPT_CODE AS 科室编码, SUM (A.ZJ) AS 总计,																	
                                                   SUM (A.JDXJ) 军队小计, SUM (A.DFXJ) 地方小计,																	
                                                   SUM (A.JYSGB) 军以上干部, SUM (A.SZGB) 师职干部,																	
                                                   SUM (A.TYXGB) 团以下干部, SUM (A.ZS) 战士,																	
                                                   SUM (A.ZG) 职工, SUM (A.MJFJS) 免减费家属,																	
                                                   SUM (A.YB) 医疗保险, SUM (A.XNH) 新农合,																	
                                                   SUM (A.YBRY) 一般人员, SUM (A.QT) 其他																	
                                             FROM {0}.DEPT_PAT_IN A, {1}.SYS_DEPT_DICT B																	
                                             WHERE A.DEPT_CODE = B.DEPT_CODE																	
                                                   AND A.ST_DATE >= TO_DATE ('{2}', 'YYYYMM')																	
                                                   AND A.ST_DATE <= TO_DATE ('{3}', 'YYYYMM')																	
                                             GROUP BY B.ACCOUNT_DEPT_CODE) A,																	
                                         {1}.SYS_DEPT_DICT B																	
                                   WHERE A.科室编码 = B.DEPT_CODE {4}) AA																	
                            ", DataUser.HISFACT, DataUser.COMM, fromDate, ToDate, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 部门CODE
        /// </summary>
        /// <param name="inputcode">部门输入编码</param>
        /// <returns>部门信息</returns>
        public DataSet GetDeptInfo(string inputcode, string power)
        {
            string Power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            string strSql = string.Format("select DISTINCT A.DEPT_CODE, a.dept_name,a.input_code from {0}.SYS_DEPT_DICT a WHERE a.input_code like ? {1} and show_flag = '0' and attr='是' order by a.DEPT_CODE", DataUser.COMM, Power);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql, cmdPara);
        }

        /// <summary>
        /// 人员CODE
        /// </summary>
        /// <param name="inputcode">人员输入编码</param>
        /// <returns>人员信息</returns>
        public DataSet GetStaffInfo(string inputcode, string power)
        {
            string Power = power == "" ? "" : "AND B.DEPT_CODE IN (" + power + ")";
            string strSql = string.Format(@"
                                            SELECT DISTINCT B.STAFF_ID,B.NAME STAFF_NAME, A.DEPT_NAME, A.INPUT_CODE
                                                   FROM {0}.SYS_DEPT_DICT A,{1}.NEW_STAFF_INFO B
                                                  WHERE B.INPUT_CODE LIKE ?
                                                        AND A.DEPT_CODE = B.DEPT_CODE
                                                        AND B.ADD_MARK = '1' and a.show_flag = '0' {2}
                                               ORDER BY B.NAME", DataUser.COMM, DataUser.RLZY, Power);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql, cmdPara);
        }

        /// <summary>
        /// 全院收治一览表(人)
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataSet GetPerBudget(string start_date, string end_date, string dept_code)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT B.DEPT_NAME AS 科室,A.CONSULTING_DOCTOR AS 收治医生,A.DOCTOR_IN_CHARGE AS 经治医生, 
                                      D.NAME AS 病人姓名, A.PATIENT_ID AS ID号,D.INP_NO AS 住院号,
                                       A.IDENTITY AS 身份, E.DIAGNOSIS_DESC AS 入院诊断,
                                       --C.NURSING_CLASS AS 护理等级,
                                       TO_CHAR (A.ADMISSION_DATE_TIME, 'yyyy-mm-dd') AS 入院时间,
                                       TO_CHAR (A.DISCHARGE_DATE_TIME, 'yyyy-mm-dd') AS 出院时间,
                                        A.SERVICE_AGENCY AS 工作单位
                                  FROM {0}.PAT_VISIT A,
                                       --PATS_IN_HOSPITAL@ORCL C,
                                       {1}.SYS_DEPT_DICT B,
                                       {0}.PAT_MASTER_INDEX D,
                                       {0}.DIAGNOSIS E
                                 WHERE A.DEPT_ADMISSION_TO = B.DEPT_CODE
                                   --AND A.PATIENT_ID = C.PATIENT_ID(+)
                                   AND A.PATIENT_ID = D.PATIENT_ID(+)
                                   AND A.PATIENT_ID = E.PATIENT_ID(+)
                                   AND A.VISIT_ID = E.VISIT_ID(+)
                                   AND E.DIAGNOSIS_TYPE(+) = '2'
                                   AND E.DIAGNOSIS_NO(+) = '1'
                                   AND A.ADMISSION_DATE_TIME >= TO_DATE ('{2}', 'yyyymm')
                                   AND A.ADMISSION_DATE_TIME < TO_DATE ('{3}', 'yyyymm')
                                   AND B.ACCOUNT_DEPT_CODE = '{4}'  ", DataUser.HISDATA, DataUser.COMM, start_date, end_date, dept_code);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 医生收入
        /// </summary>
        /// <param name="fromDate">开始时间</param>
        /// <param name="ToDate">结束时间</param>
        /// <param name="power">权限</param>
        /// <param name="deptCode">选择科室CODE</param>
        /// <param name="staffId">选择人员ID</param>
        /// <returns></returns>
        public DataSet getDeptBenefit(string fromDate, string ToDate, string power, string deptCode, string staffId)
        {
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE IN(SELECT DEPT_CODE FROM COMM.SYS_DEPT_DICT WHERE ACCOUNT_DEPT_CODE = '" + deptCode + "')";
            staffId = staffId == "" ? "" : "AND STAFF_ID = " + staffId;
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   B.DEPT_NAME,AA.ACCOUNT_DEPT_CODE DEPT_CODE, TO_CHAR(AA.STAFF_ID) STAFF_ID, AA.NAME,                                                                                        
                                                 TO_CHAR(SUM (AA.MZJM),'9g999g990d00') AS MZJM, TO_CHAR(SUM (AA.MZDF),'9g999g990d00') MZDF, 
                                                 TO_CHAR(SUM (AA.MZXJ),'9g999g990d00') MZXJ,                                                                                        
                                                 TO_CHAR(SUM (AA.ZYJM),'9g999g990d00') ZYJM, TO_CHAR(SUM (AA.ZYDF),'9g999g990d00') ZYDF, 
                                                 TO_CHAR(SUM (AA.ZYXJ),'9g999g990d00') ZYXJ,                                                                                        
                                                 TO_CHAR(SUM (AA.ZYJM + AA.MZJM),'9g999g990d00') MSRJM, 
                                                 TO_CHAR(SUM (AA.ZYJM + AA.MZJM),'9g999g990d00') MSRDF,                                                                                        
                                                 TO_CHAR(SUM (AA.MZXJ + AA.ZYXJ),'9g999g990d00') MSRXJ                                                                                        
                                            FROM (SELECT A.*, B.ACCOUNT_DEPT_CODE, C.NAME                                                                                        
                                                FROM (SELECT ST_DATE, DEPT_CODE, STAFF_ID, COUNT_INCOME AS MZJM,                                                                                        
                                                             CHARGES AS MZDF, COSTS AS MZXJ, 0 AS ZYJM, 0 AS ZYDF,                                                                                        
                                                             0 AS ZYXJ                                                                                        
                                                        FROM {0}.OUTP_CLASS_INCOME_DAY                                                                                        
                                                        WHERE   ST_DATE >= TO_DATE ('{3}', 'yyyy-mm-dd')                                                                                        
                                                                AND ST_DATE <= TO_DATE ('{4}', 'yyyy-mm-dd')  {5} {6} {7}                                                                                       
                                                      UNION ALL                                                                                        
                                                      SELECT ST_DATE, DEPT_CODE, STAFF_ID, 0 AS MZJM, 0 AS MZDF,                                                                                        
                                                             0 AS MZXJ, COUNT_INCOME AS ZYJM, CHARGES AS ZYDF,                                                                                        
                                                             COSTS AS ZYXJ                                                                                        
                                                        FROM {0}.INP_CLASS_INCOME_DAY                                                                                        
                                                        WHERE  ST_DATE >= TO_DATE ('{3}', 'yyyy-mm-dd')                                                                                        
                                                               AND ST_DATE <= TO_DATE ('{4}', 'yyyy-mm-dd')    {5} {6} {7}                                                                                        
                                                        ) A,                                                                                        
                                                     {1}.SYS_DEPT_DICT B,                                                                                        
                                                     {2}.NEW_STAFF_INFO C                                                                                        
                                               WHERE A.DEPT_CODE = B.DEPT_CODE                                                                                        
                                                 AND A.STAFF_ID = C.STAFF_ID                                                                                        
                                                 ORDER BY A.DEPT_CODE DESC     ) AA, {1}.SYS_DEPT_DICT B                                                                                        
                                           WHERE AA.ACCOUNT_DEPT_CODE = B.DEPT_CODE                                                                                        
                                        GROUP BY AA.ACCOUNT_DEPT_CODE, AA.STAFF_ID, AA.NAME,B.DEPT_NAME                                                                                        
                                        UNION ALL                                                                                        
                                        SELECT   '合计' AS DEPT_NAME, '999999' AS DEPT_CODE, '999999' STAFF_ID, '' NAME,                                                                                        
                                                 TO_CHAR(SUM (MZJM),'9g999g990d00') AS MZJM, TO_CHAR(SUM (MZDF),'9g999g990d00') MZDF, 
                                                 TO_CHAR(SUM (MZXJ),'9g999g990d00') MZXJ, TO_CHAR(SUM (ZYJM),'9g999g990d00') ZYJM,                                                                                        
                                                 TO_CHAR(SUM (ZYDF),'9g999g990d00') ZYDF, 
                                                 TO_CHAR(SUM (ZYXJ),'9g999g990d00') ZYXJ, TO_CHAR(SUM (ZYJM + MZJM),'9g999g990d00') MSRJM,                                                                                        
                                                 TO_CHAR(SUM (ZYJM + MZJM),'9g999g990d00') MSRDF, 
                                                 TO_CHAR(SUM (MZXJ + ZYXJ),'9g999g990d00') MSRXJ                                                                                         
                                                 FROM (SELECT A.*, B.ACCOUNT_DEPT_CODE, C.NAME                                                                                        
                                                FROM (SELECT ST_DATE, DEPT_CODE, STAFF_ID, COUNT_INCOME AS MZJM,                                                                                        
                                                             CHARGES AS MZDF, COSTS AS MZXJ, 0 AS ZYJM, 0 AS ZYDF,                                                                                        
                                                             0 AS ZYXJ                                                                                        
                                                        FROM {0}.OUTP_CLASS_INCOME_DAY                                                                                        
                                                        WHERE   ST_DATE >= TO_DATE ('{3}', 'yyyy-mm-dd')                                                                                        
                                                                AND ST_DATE <= TO_DATE ('{4}', 'yyyy-mm-dd')  {5} {6} {7}                                                                                       
                                                      UNION ALL                                                                                        
                                                      SELECT ST_DATE, DEPT_CODE, STAFF_ID, 0 AS MZJM, 0 AS MZDF,                                                                                        
                                                             0 AS MZXJ, COUNT_INCOME AS ZYJM, CHARGES AS ZYDF,                                                                                        
                                                             COSTS AS ZYXJ                                                                                        
                                                        FROM {0}.INP_CLASS_INCOME_DAY                                                                                        
                                                        WHERE  ST_DATE >= TO_DATE ('{3}', 'yyyy-mm-dd')                                                                                        
                                                               AND ST_DATE <= TO_DATE ('{4}', 'yyyy-mm-dd')    {5} {6} {7}                                                                                        
                                                        ) A,                                                                                        
                                                     {1}.SYS_DEPT_DICT B,                                                                                        
                                                     {2}.NEW_STAFF_INFO C                                                                                        
                                               WHERE A.DEPT_CODE = B.DEPT_CODE                                                                                        
                                                 AND A.STAFF_ID = C.STAFF_ID                                                                                        
                                                 ORDER BY A.DEPT_CODE DESC     ) AA", DataUser.HISFACT, DataUser.COMM, DataUser.RLZY, fromDate, ToDate, power, deptCode, staffId);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        private string[] arrFd = { "", "", "", "", "COUNT_INCOME", "CHARGES", "COSTS", "COUNT_INCOME", "CHARGES", "COSTS", "COUNT_INCOME", "CHARGES", "COSTS" };

        private string[] arrProjectFd = { "", "", "", "", "SUM (COSTS) - SUM (CHARGES)", "SUM (CHARGES)", "SUM (COSTS)", " SUM (AA.COSTS) - SUM (AA.CHARGES) ", " SUM (CHARGES)", "SUM (COSTS)", "SUM (COSTS) - SUM (CHARGES)", "SUM (CHARGES)", "SUM (COSTS)" };

        private string[] arrProjectTrem = { "", "", "", "", "AND COSTS - CHARGES <> 0", "AND CHARGES <> 0", "", "AND AA.COSTS - AA.CHARGES <> 0", "AND CHARGES <> 0", "", "AND COSTS - CHARGES <> 0", "AND CHARGES <> 0", "" };

        private string[] arrName = { "", "", "", "", "OUTP_CLASS_INCOME_DAY", "OUTP_CLASS_INCOME_DAY", "OUTP_CLASS_INCOME_DAY", "INP_CLASS_INCOME_DAY", "INP_CLASS_INCOME_DAY", "INP_CLASS_INCOME_DAY" };

        public DataSet getDeptBenefitInfo(string fromDate, string toDate, string deptCode, string staffid, string type)
        {
            StringBuilder str = new StringBuilder();
            int col = Convert.ToInt32(type);
            string Index = col < 10 ? "1" : "2";

            switch (Index)
            {
                case "1":
                    str.AppendFormat(@"SELECT   INCOM_TYPE_CODE, MIN (INCOM_TYPE_NAME) AS INCOM_TYPE_NAME,															
                                                 SUM ({2}) AS FY,'{4}' FROMDATE,'{5}' TODATE,'{6}' STAFF_ID,'{7}' DEPT_CODE,'{9}' TYPE															
                                            FROM {0}.{3} SJ, {8}.CBHS_INCOM_TYPE_DICT ZD															
                                           WHERE SJ.RECK_CLASS LIKE ZD.INCOM_TYPE_CODE || '%'															
                                             AND ST_DATE >= TO_DATE ('{4}', 'yyyy-mm-dd')															
                                             AND ST_DATE <= TO_DATE ('{5}', 'yyyy-mm-dd')															
                                             AND STAFF_ID = '{6}'															
                                             AND DEPT_CODE IN (SELECT DEPT_CODE                                                        															
                                                                 FROM {1}.SYS_DEPT_DICT                                                        															
                                                                WHERE ACCOUNT_DEPT_CODE = '{7}')   															
                                            GROUP BY INCOM_TYPE_CODE															
                                            ORDER BY INCOM_TYPE_CODE															
                                        ", DataUser.HISFACT, DataUser.COMM, arrFd[col], arrName[col], fromDate, toDate, staffid, deptCode, DataUser.CBHS, type);
                    break;
                case "2":
                    str.AppendFormat(@"SELECT   INCOM_TYPE_CODE, MIN (INCOM_TYPE_NAME) AS INCOM_TYPE_NAME,														
                                                     SUM ({4}) AS FY,'{0}' FROMDATE,'{1}' TODATE,'{2}' STAFF_ID,'{3}' DEPT_CODE,'{8}' TYPE														
                                                FROM (SELECT RECK_CLASS, {4}														
                                                        FROM {5}.OUTP_CLASS_INCOME_DAY														
                                                       WHERE ST_DATE >= TO_DATE ('{0}', 'yyyy-mm-dd')														
                                                         AND ST_DATE <= TO_DATE ('{1}', 'yyyy-mm-dd')														
                                                         AND STAFF_ID = '{2}'														
                                                         AND DEPT_CODE IN (SELECT DEPT_CODE														
                                                                             FROM {6}.SYS_DEPT_DICT														
                                                                            WHERE ACCOUNT_DEPT_CODE = '{3}')														
                                                      UNION ALL														
                                                      SELECT RECK_CLASS, {4}														
                                                        FROM {5}.INP_CLASS_INCOME_DAY														
                                                       WHERE ST_DATE >= TO_DATE ('{0}', 'yyyy-mm-dd')														
                                                         AND ST_DATE <= TO_DATE ('{1}', 'yyyy-mm-dd')														
                                                         AND STAFF_ID = '{2}'														
                                                         AND DEPT_CODE IN (SELECT DEPT_CODE														
                                                                             FROM {6}.SYS_DEPT_DICT														
                                                                            WHERE ACCOUNT_DEPT_CODE = '{3}')) SJ,														
                                                     {7}.CBHS_INCOM_TYPE_DICT ZD														
                                               WHERE SJ.RECK_CLASS LIKE ZD.INCOM_TYPE_CODE || '%'														
                                            GROUP BY INCOM_TYPE_CODE", fromDate, toDate, staffid, deptCode, arrFd[col], DataUser.HISFACT, DataUser.COMM, DataUser.CBHS, type);
                    break;
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        public DataSet getPerBenefitInfo(string fromDate, string toDate, string deptCode, string staffid, string type, string itemClass)
        {
            StringBuilder str = new StringBuilder();
            int index = Convert.ToInt32(type);
            if (index <= 6)
            {
                str.AppendFormat(@"SELECT   C.ITEM_NAME, C.PRICE, A.UNITS, SUM (AMOUNT) AS AMOUNT,																		
                                                 {5} AS JE																		
                                            FROM {6}.OUTP_ORDERS_COSTS A,																		
                                                 {7}.NEW_STAFF_INFO B,																		
                                                 {6}.PRICE_LIST C																		
                                           WHERE A.ORDERED_BY_DOCTOR = B.NAME(+)																		
                                             AND A.ITEM_CLASS = C.ITEM_CLASS(+)																		
                                             AND A.ITEM_CODE = C.ITEM_CODE(+)																		
                                             AND A.ITEM_SPEC = C.ITEM_SPEC(+)																		
                                             AND A.UNITS = C.UNITS(+)																		
                                             AND A.VISIT_DATE >= C.START_DATE																		
                                             AND A.VISIT_DATE < NVL (C.STOP_DATE, SYSDATE)																		
                                             AND VISIT_DATE >= TO_DATE ('{0}', 'yyyy-mm-dd')																		
                                             AND VISIT_DATE <= TO_DATE ('{1}', 'yyyy-mm-dd')																		
                                             AND ORDERED_BY_DEPT IN (SELECT DEPT_CODE																		
                                                                       FROM COMM.SYS_DEPT_DICT																		
                                                                      WHERE ACCOUNT_DEPT_CODE = '{2}')																		
                                             AND B.STAFF_ID = '{3}'																		
                                             AND B.ADD_MARK = '1'																		
                                             AND SUBSTR (C.CLASS_ON_RECKONING, 0, 1) = '{4}'																		
                                              {8}																		
                                        GROUP BY SUBSTR (C.CLASS_ON_RECKONING, 0, 1), 
                                                 C.ITEM_NAME, C.PRICE, A.UNITS", fromDate, toDate, deptCode, staffid, itemClass, arrProjectFd[index], DataUser.HISDATA, DataUser.RLZY, arrProjectTrem[index]);
            }

            if (index == 9 || index == 8 || index == 7)
            {
                str.AppendFormat(@"SELECT   DD.ITEM_NAME, DD.PRICE, AA.UNITS, SUM (AMOUNT) AMOUNT,																			
                                             {5} JE																			
                                        FROM {7}.INP_BILL_DETAIL AA,																			
                                             {7}.TRANSFER BB,																			
                                             (SELECT STAFF_ID, NAME																			
                                                FROM {8}.NEW_STAFF_INFO																			
                                               WHERE ADD_MARK = '1') CC,																			
                                             {7}.PRICE_LIST DD																			
                                       WHERE AA.BILLING_DATE_TIME >= BB.ADMISSION_DATE_TIME(+)																			
                                         AND AA.BILLING_DATE_TIME <= NVL (BB.DISCHARGE_DATE_TIME(+), SYSDATE)																			
                                         AND AA.PATIENT_ID = BB.PATIENT_ID(+)																			
                                         AND AA.VISIT_ID = BB.VISIT_ID(+)																			
                                         AND AA.ORDERED_BY = BB.DEPT_STAYED(+)																			
                                         AND BB.DOCTOR_IN_CHARGE = CC.NAME(+)																			
                                         AND AA.ITEM_CLASS = DD.ITEM_CLASS(+)																			
                                         AND AA.ITEM_CODE = DD.ITEM_CODE(+)																			
                                         AND AA.ITEM_SPEC = DD.ITEM_SPEC(+)																			
                                         AND AA.UNITS = DD.UNITS(+)																			
                                         AND AA.BILLING_DATE_TIME >= DD.START_DATE																			
                                         AND AA.BILLING_DATE_TIME < NVL (DD.STOP_DATE, SYSDATE)                                                                            
                                         AND BILLING_DATE_TIME >= TO_DATE ('{0}', 'yyyy-mm-dd')                                                                            
                                         AND BILLING_DATE_TIME <= TO_DATE ('{1}', 'yyyy-mm-dd')                                                                           
                                         AND SUBSTR (DD.CLASS_ON_RECKONING, 0, 1) = '{2}'                                                                            
                                         AND AA.ORDERED_BY IN (SELECT DEPT_CODE                                                                            
                                                                      FROM {9}.SYS_DEPT_DICT                                                                            
                                                                     WHERE ACCOUNT_DEPT_CODE = '{3}')                                                                            
                                         AND CC.STAFF_ID = '{4}'                                                                            
                                          {6}                                                                            
                                    GROUP BY AA.ITEM_CODE,                                                                            
                                             ORDERED_BY,                                                                            
                                             CC.STAFF_ID,                                                                            
                                             DD.ITEM_NAME,                                                                            
                                             AA.UNITS,                                                                            
                                             DD.CLASS_ON_RECKONING,                                                                            
                                             BB.DOCTOR_IN_CHARGE,                                                                            
                                             DD.PRICE                                                                            
                                    ORDER BY DD.CLASS_ON_RECKONING, AA.ITEM_CODE, DD.ITEM_NAME ", fromDate, toDate, itemClass, deptCode, staffid, arrProjectFd[index], arrProjectTrem[index], DataUser.HISDATA, DataUser.RLZY, DataUser.COMM);
            }

            if (index > 9)
            {
                str.AppendFormat(@"SELECT   ITEM_NAME, PRICE, UNITS, SUM (AMOUNT) AS AMOUNT,																			
                                                 SUM (JD_INCOME) AS JE																			
                                            FROM (SELECT   C.ITEM_NAME, C.PRICE, A.UNITS, SUM (AMOUNT) AS AMOUNT,																			
                                                            {5} JD_INCOME																			
                                                      FROM {7}.OUTP_ORDERS_COSTS A,																			
                                                           {8}.NEW_STAFF_INFO B,																			
                                                           {7}.PRICE_LIST C																			
                                                     WHERE A.ORDERED_BY_DOCTOR = B.NAME(+)																			
                                                       AND A.ITEM_CLASS = C.ITEM_CLASS(+)																			
                                                       AND A.ITEM_CODE = C.ITEM_CODE(+)																			
                                                       AND A.ITEM_SPEC = C.ITEM_SPEC(+)																			
                                                       AND A.UNITS = C.UNITS(+)																			
                                                       AND A.VISIT_DATE >= C.START_DATE																			
                                                       AND A.VISIT_DATE < NVL (C.STOP_DATE, SYSDATE)																			
                                                       AND VISIT_DATE >= TO_DATE ('{0}', 'yyyy-mm-dd')																			
                                                       AND VISIT_DATE <= TO_DATE ('{1}', 'yyyy-mm-dd')																			
                                                       AND SUBSTR (C.CLASS_ON_RECKONING, 0, 1) = '{2}'																			
                                                       AND ORDERED_BY_DEPT IN (SELECT DEPT_CODE																			
                                                                                 FROM {9}.SYS_DEPT_DICT																			
                                                                                WHERE ACCOUNT_DEPT_CODE = '{3}')					                                                        
                                                       AND B.STAFF_ID = '{4}'                                                                            
                                                       AND B.ADD_MARK = '1'                                                                            
                                                        {6}                                                                            
                                                  GROUP BY SUBSTR (C.CLASS_ON_RECKONING, 0, 1),                                                                            
                                                           C.ITEM_NAME,                                                                            
                                                           A.UNITS,                                                                            
                                                           C.PRICE                                                                            
                                                  UNION ALL                                                                            
                                                  SELECT   DD.ITEM_NAME, DD.PRICE, AA.UNITS, SUM (AMOUNT) AMOUNT,                                                                            
                                                           {5} JD_INCOME                                                                            
                                                      FROM {7}.INP_BILL_DETAIL AA,                                                                            
                                                           {7}.TRANSFER BB,                                                                            
                                                           (SELECT STAFF_ID, NAME                                                                            
                                                              FROM {8}.NEW_STAFF_INFO                                                                            
                                                             WHERE ADD_MARK = '1') CC,                                                                            
                                                           {7}.PRICE_LIST DD                                                                            
                                                     WHERE AA.BILLING_DATE_TIME >= BB.ADMISSION_DATE_TIME(+)                                                                            
                                                       AND AA.BILLING_DATE_TIME <= NVL (BB.DISCHARGE_DATE_TIME(+),                                                                            
                                                                                        SYSDATE)                                                                            
                                                       AND AA.PATIENT_ID = BB.PATIENT_ID(+)                                                                            
                                                       AND AA.VISIT_ID = BB.VISIT_ID(+)                                                                            
                                                       AND AA.ORDERED_BY = BB.DEPT_STAYED(+)                                                                            
                                                       AND BB.DOCTOR_IN_CHARGE = CC.NAME(+)                                                                            
                                                       AND AA.ITEM_CLASS = DD.ITEM_CLASS(+)                                                                            
                                                       AND AA.ITEM_CODE = DD.ITEM_CODE(+)                                                                            
                                                       AND AA.ITEM_SPEC = DD.ITEM_SPEC(+)                                                                            
                                                       AND AA.UNITS = DD.UNITS(+)                                                                            
                                                       AND AA.BILLING_DATE_TIME >= DD.START_DATE                                                                            
                                                       AND AA.BILLING_DATE_TIME < NVL (DD.STOP_DATE, SYSDATE)                                                                            
                                                       AND BILLING_DATE_TIME >= TO_DATE ('{0}', 'yyyy-mm-dd')                                                                            
                                                       AND BILLING_DATE_TIME <= TO_DATE ('{1}', 'yyyy-mm-dd')                                                                           
                                                       AND SUBSTR (DD.CLASS_ON_RECKONING, 0, 1) = '{2}'                                                                            
                                                       AND AA.ORDERED_BY IN (SELECT DEPT_CODE                                                                            
                                                                                    FROM {9}.SYS_DEPT_DICT                                                                            
                                                                                   WHERE ACCOUNT_DEPT_CODE = '{3}')                                                                            
                                                       AND CC.STAFF_ID = '{4}'                                                                            
                                                        {6}                                                                            
                                                  GROUP BY AA.ITEM_CODE,                                                                            
                                                           ORDERED_BY,                                                                            
                                                           CC.STAFF_ID,                                                                            
                                                           DD.ITEM_NAME,                                                                            
                                                           AA.UNITS,                                                                            
                                                           DD.CLASS_ON_RECKONING,                                                                            
                                                           BB.DOCTOR_IN_CHARGE,                                                                            
                                                           DD.PRICE)                                                                            
                                        GROUP BY ITEM_NAME, PRICE, UNITS                                                                            
                                      ", fromDate, toDate, itemClass, deptCode, staffid, arrProjectFd[index], arrProjectTrem[index], DataUser.HISDATA, DataUser.RLZY, DataUser.COMM);
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        public DataSet getPerformDeptIncome(string fromDate, string toDate, string deptCode, string power)
        {

            deptCode = deptCode == "" ? "" : "AND A.PERFORMED_BY IN (SELECT DEPT_CODE FROM COMM.SYS_DEPT_DICT WHERE ACCOUNT_DEPT_CODE = '" + deptCode + "')";
            power = power == "" ? "" : "AND A.PERFORMED_BY IN (SELECT DEPT_CODE FROM COMM.SYS_DEPT_DICT WHERE DEPT_CODE IN (" + power + "))";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                                SELECT AA.*
                                  FROM (SELECT   CLASS_NAME, CLASS_CODE,
                                               SUM (NVL (COSTSMZ, 0)) + SUM (NVL (COSTSZY, 0)) COSTS,
                                               NVL (SUM (COSTSMZ), '0') COSTSMZ,
                                               NVL (SUM (CHARGESMZ), '0') CHARGESMZ,
                                               NVL (SUM (COUNT_INCOMEMZ), '0') COUNT_INCOMEMZ,
                                               NVL (SUM (COSTSZY), '0') COSTSZY,
                                               NVL (SUM (CHARGESZY), '0') CHARGESZY,
                                               NVL (SUM (COUNT_INCOMEZY), '0') COUNT_INCOMEZY
                                          FROM (SELECT   CLASS_ON_RECKONING RECK_CLASS, SUM (COSTS) COSTSMZ,
                                                         SUM (CHARGES) CHARGESMZ,
                                                         SUM (COSTS - CHARGES) COUNT_INCOMEMZ
                                                    FROM {0}.OUTP_BILL_ITEMS A, {0}.PRICE_LIST B
                                                   WHERE A.ITEM_CLASS = B.ITEM_CLASS(+)
                                                     AND A.ITEM_CODE = B.ITEM_CODE(+)
                                                     AND A.ITEM_SPEC = B.ITEM_SPEC(+)
                                                     AND A.ITEM_NAME = B.ITEM_NAME(+)
                                                     AND A.UNITS = B.UNITS(+)
                                                     AND A.VISIT_DATE >= B.START_DATE
                                                     AND A.VISIT_DATE < NVL (B.STOP_DATE, SYSDATE)
                                                     AND VISIT_DATE >= TO_DATE ('{2}', 'yyyy-mm-dd')
                                                     AND VISIT_DATE < TO_DATE ('{3}', 'yyyy-mm-dd')
                                                     {4} {5}
                                                GROUP BY CLASS_ON_RECKONING) A,
                                               (SELECT   RECK_CLASS, SUM (COSTS) COSTSZY,
                                                         SUM (CHARGES) CHARGESZY,
                                                         SUM (COUNT_INCOME) COUNT_INCOMEZY
                                                    FROM {1}.INP_CLASS_INCOME_DAY A
                                                   WHERE ST_DATE >= TO_DATE ('{2}', 'yyyy-mm-dd')
                                                     AND ST_DATE < TO_DATE ('{3}', 'yyyy-mm-dd')
                                                     {4} {5}
                                                GROUP BY RECK_CLASS) B,
                                               {0}.RECK_ITEM_CLASS_DICT C
                                         WHERE A.RECK_CLASS(+) = C.CLASS_CODE
                                           AND B.RECK_CLASS(+) = C.CLASS_CODE
                                           AND NVL (COSTSMZ, 0) <> 0
                                           AND NVL (COSTSZY, 0) <> 0
                                      GROUP BY CLASS_CODE, CLASS_NAME
                                      ORDER BY CLASS_CODE) AA
                                UNION ALL
                                SELECT '合计' CLASS_NAME, '999999' CLASS_CODE, NVL (SUM (COSTS), 0),
                                       NVL (SUM (COSTSMZ), 0), NVL (SUM (CHARGESMZ), 0),
                                       NVL (SUM (COUNT_INCOMEMZ), 0), NVL(SUM (COSTSZY),0), NVL(SUM (CHARGESZY),0),
                                       NVL(SUM (COUNT_INCOMEZY),0)
                                  FROM (SELECT   CLASS_NAME, CLASS_CODE,
                                               SUM (NVL (COSTSMZ, 0)) + SUM (NVL (COSTSZY, 0)) COSTS,
                                               NVL (SUM (COSTSMZ), '0') COSTSMZ,
                                               NVL (SUM (CHARGESMZ), '0') CHARGESMZ,
                                               NVL (SUM (COUNT_INCOMEMZ), '0') COUNT_INCOMEMZ,
                                               NVL (SUM (COSTSZY), '0') COSTSZY,
                                               NVL (SUM (CHARGESZY), '0') CHARGESZY,
                                               NVL (SUM (COUNT_INCOMEZY), '0') COUNT_INCOMEZY
                                          FROM (SELECT   CLASS_ON_RECKONING RECK_CLASS, SUM (COSTS) COSTSMZ,
                                                         SUM (CHARGES) CHARGESMZ,
                                                         SUM (COSTS - CHARGES) COUNT_INCOMEMZ
                                                    FROM {0}.OUTP_BILL_ITEMS A, {0}.PRICE_LIST B
                                                   WHERE A.ITEM_CLASS = B.ITEM_CLASS(+)
                                                     AND A.ITEM_CODE = B.ITEM_CODE(+)
                                                     AND A.ITEM_SPEC = B.ITEM_SPEC(+)
                                                     AND A.ITEM_NAME = B.ITEM_NAME(+)
                                                     AND A.UNITS = B.UNITS(+)
                                                     AND A.VISIT_DATE >= B.START_DATE
                                                     AND A.VISIT_DATE < NVL (B.STOP_DATE, SYSDATE)
                                                     AND VISIT_DATE >= TO_DATE ('{2}', 'yyyy-mm-dd')
                                                     AND VISIT_DATE < TO_DATE ('{3}', 'yyyy-mm-dd')
                                                     {4} {5}
                                                GROUP BY CLASS_ON_RECKONING) A,
                                               (SELECT   RECK_CLASS, SUM (COSTS) COSTSZY,
                                                         SUM (CHARGES) CHARGESZY,
                                                         SUM (COUNT_INCOME) COUNT_INCOMEZY
                                                    FROM {1}.INP_CLASS_INCOME_DAY A
                                                   WHERE ST_DATE >= TO_DATE ('{2}', 'yyyy-mm-dd')
                                                     AND ST_DATE < TO_DATE ('{3}', 'yyyy-mm-dd')
                                                     {4} {5}
                                                GROUP BY RECK_CLASS) B,
                                               {0}.RECK_ITEM_CLASS_DICT C
                                         WHERE A.RECK_CLASS(+) = C.CLASS_CODE
                                           AND B.RECK_CLASS(+) = C.CLASS_CODE
                                           AND NVL (COSTSMZ, 0) <> 0
                                           AND NVL (COSTSZY, 0) <> 0
                                      GROUP BY CLASS_CODE, CLASS_NAME
                                      ORDER BY CLASS_CODE)", DataUser.HISDATA, DataUser.HISFACT, fromDate, toDate, deptCode, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        private string[] arrPerFormIncomeName = { "", "", "", "OUTP_BILL_ITEMS", "OUTP_BILL_ITEMS", "OUTP_BILL_ITEMS", "INP_BILL_DETAIL", "INP_BILL_DETAIL", "INP_BILL_DETAIL" };
        private string[] arrPerFormFd = { "", "", "", "A.COSTS-A.CHARGES", "A.CHARGES", "A.COSTS", "A.COSTS-A.CHARGES", "CHARGES", "COSTS" };
        private string[] arrPerFormTrem = { "", "", "", "AND (A.COSTS-A.CHARGES)<>0", "AND CHARGES <> 0", "AND COSTS <> 0", "AND (A.COSTS-A.CHARGES)<>0", "AND CHARGES <> 0", "AND COSTS <> 0" };
        private string[] arrPerFormTime = { "", "", "", "A.VISIT_DATE", "A.VISIT_DATE", "A.VISIT_DATE", "A.BILLING_DATE_TIME", "A.BILLING_DATE_TIME", "A.BILLING_DATE_TIME" };
        public DataSet getPerformIncomeInfo(string fromDate, string toDate, string deptCode, string ClassCode, string type)
        {
            StringBuilder str = new StringBuilder();
            deptCode = deptCode == "" ? "" : "AND A.PERFORMED_BY IN (SELECT DEPT_CODE FROM COMM.SYS_DEPT_DICT WHERE ACCOUNT_DEPT_CODE = '" + deptCode + "')";
            int index = Convert.ToInt32(type);
            if (index == 2)
            {
                str.AppendFormat(@"SELECT   ITEM_NAME, SUM (AMOUNT) AMOUNT, TO_CHAR(PRICE,'9g999g990d000') PRICE , UNITS, TO_CHAR(SUM (COSTS),'9g999g990d00') JE
                                                FROM (SELECT A.ITEM_NAME, A.AMOUNT, B.PRICE, A.UNITS, A.COSTS
                                                        FROM {0}.INP_BILL_DETAIL A, {0}.PRICE_LIST B
                                                       WHERE BILLING_DATE_TIME >= TO_DATE ('{2}', 'yyyy-mm-dd')
                                                         AND BILLING_DATE_TIME < TO_DATE ('{3}', 'yyyy-mm-dd')
                                                         AND A.ITEM_CLASS = B.ITEM_CLASS(+)
                                                         AND A.ITEM_CODE = B.ITEM_CODE(+)
                                                         AND A.ITEM_SPEC = B.ITEM_SPEC(+)
                                                         AND A.UNITS = B.UNITS(+)
                                                         AND A.BILLING_DATE_TIME >= B.START_DATE(+)
                                                         AND A.BILLING_DATE_TIME < NVL (B.STOP_DATE(+), SYSDATE)
                                                         AND B.CLASS_ON_RECKONING = '{1}'
                                                         {4}
                                                      UNION ALL
                                                      SELECT A.ITEM_NAME, A.AMOUNT, B.PRICE, A.UNITS, A.COSTS
                                                        FROM {0}.OUTP_BILL_ITEMS A, {0}.PRICE_LIST B
                                                       WHERE VISIT_DATE >= TO_DATE ('{2}', 'yyyy-mm-dd')
                                                         AND VISIT_DATE < TO_DATE ('{3}', 'yyyy-mm-dd')
                                                         AND A.ITEM_CLASS = B.ITEM_CLASS(+)
                                                         AND A.ITEM_CODE = B.ITEM_CODE(+)
                                                         AND A.ITEM_SPEC = B.ITEM_SPEC(+)
                                                         AND A.UNITS = B.UNITS(+)
                                                         AND A.VISIT_DATE >= B.START_DATE(+)
                                                         AND A.VISIT_DATE < NVL (B.STOP_DATE(+), SYSDATE)
                                                         AND B.CLASS_ON_RECKONING = '{1}' {4})
                                            GROUP BY ITEM_NAME, PRICE, UNITS", DataUser.HISDATA, ClassCode, fromDate, toDate, deptCode);
            }
            else
            {
                str.AppendFormat(@"SELECT   ITEM_NAME, SUM (AMOUNT) AMOUNT, TO_CHAR(PRICE,'9g999g990d000') PRICE, UNITS, TO_CHAR(SUM (JE),'9g999g990d00') JE
                                        FROM (SELECT A.ITEM_NAME, A.AMOUNT, B.PRICE, A.UNITS, {2} JE
                                                FROM {0}.{1} A, {0}.PRICE_LIST B
                                               WHERE {8} >= TO_DATE ('{5}', 'yyyy-mm-dd')
                                                 AND {8} < TO_DATE ('{6}', 'yyyy-mm-dd')
                                                 AND A.ITEM_CLASS = B.ITEM_CLASS(+)
                                                 AND A.ITEM_CODE = B.ITEM_CODE(+)
                                                 AND A.ITEM_SPEC = B.ITEM_SPEC(+)
                                                 AND A.UNITS = B.UNITS(+)
                                                 AND {8} >= B.START_DATE(+)
                                                 AND {8} < NVL (B.STOP_DATE(+), SYSDATE)
                                                 AND B.CLASS_ON_RECKONING = '{7}'
                                                 {3} {4})
                                    GROUP BY ITEM_NAME, PRICE, UNITS", DataUser.HISDATA, arrPerFormIncomeName[index], arrPerFormFd[index], arrPerFormTrem[index], deptCode, fromDate, toDate, ClassCode, arrPerFormTime[index]);
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 药品
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="deptCode"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public DataSet getDeptDrug(string fromDate, string ToDate, string deptCode, string power, string type)
        {
            StringBuilder str = new StringBuilder();

            StringBuilder strType = new StringBuilder();

            deptCode = deptCode == "" ? "" : "AND B.DEPT_CODE = '" + deptCode + "'";

            switch (type)
            {
                case "qb":
                    strType.AppendFormat(@"SELECT DEPT_CODE, COSTS COSTS,
                                                     CASE WHEN (ITEM_CODE LIKE '01%') THEN COSTS ELSE 0 END KSS,
                                                     DECODE(RECK_CLASS,'A01',COSTS,0) XYXJ,
                                                     DECODE(RECK_CLASS,'A01',CHARGES,0) XYDF,
                                                     DECODE(RECK_CLASS,'A01',COUNT_INCOME,0) XYJM,
                                                     DECODE(RECK_CLASS,'A02',COSTS,0) ZYXJ,
                                                     DECODE(RECK_CLASS,'A02',CHARGES,0) ZYDF,
                                                     DECODE(RECK_CLASS,'A02',COUNT_INCOME,0) ZYJM,
                                                     DECODE(RECK_CLASS,'A03',COSTS,0) ZKYYXJ,
                                                     DECODE(RECK_CLASS,'A03',CHARGES,0) ZKYYDF,
                                                     DECODE(RECK_CLASS,'A03',COUNT_INCOME,0) ZKYYJM
                                                FROM {0}.INP_CLASS_INCOME_DAY A
                                               WHERE ST_DATE >= TO_DATE ('{2}', 'yyyy-mm-dd')
                                                 AND ST_DATE < TO_DATE ('{3}', 'yyyy-mm-dd')
                                                 AND RECK_CLASS LIKE 'A%'
                                              UNION ALL
                                              SELECT ORDERED_BY_DEPT DEPT_CODE, COSTS COSTS,
                                                     CASE WHEN (A.ITEM_CODE LIKE '01%') THEN COSTS ELSE 0 END KSS,
                                                     DECODE(CLASS_ON_RECKONING,'A01',COSTS,0) XYXJ,
                                                     DECODE(CLASS_ON_RECKONING,'A01',CHARGES,0) XYDF,
                                                     DECODE(CLASS_ON_RECKONING,'A01',(COSTS - CHARGES),0) XYJM,
                                                     DECODE(CLASS_ON_RECKONING,'A02',COSTS,0) ZYXJ,
                                                     DECODE(CLASS_ON_RECKONING,'A02',CHARGES,0) ZYDF,
                                                     DECODE(CLASS_ON_RECKONING,'A02',(COSTS - CHARGES),0) ZYJM,
                                                     DECODE(CLASS_ON_RECKONING,'A03',COSTS,0) ZKYYXJ,
                                                     DECODE(CLASS_ON_RECKONING,'A03',CHARGES,0) ZKYYDF,
                                                     DECODE(CLASS_ON_RECKONING,'A03',(COSTS - CHARGES),0) ZKYYJM
                                                FROM {1}.OUTP_BILL_ITEMS A,
                                                     {1}.PRICE_LIST B,
                                                     {1}.OUTP_ORDER_DESC C
                                               WHERE A.VISIT_DATE = C.VISIT_DATE
                                                 AND A.VISIT_NO = C.VISIT_NO
                                                 AND A.ITEM_CLASS = B.ITEM_CLASS(+)
                                                 AND A.ITEM_CODE = B.ITEM_CODE(+)
                                                 AND A.ITEM_SPEC = B.ITEM_SPEC(+)
                                                 AND A.UNITS = B.UNITS(+)
                                                 AND A.VISIT_DATE >= B.START_DATE(+)
                                                 AND A.VISIT_DATE < NVL (B.STOP_DATE(+), SYSDATE)
                                                 AND A.VISIT_DATE >= TO_DATE ('{2}', 'yyyy-mm-dd')
                                                 AND A.VISIT_DATE < TO_DATE ('{3}', 'yyyy-mm-dd')
                                                 AND CLASS_ON_RECKONING LIKE 'A%'", DataUser.HISFACT, DataUser.HISDATA, fromDate, ToDate);
                    break;
                case "mz":
                    strType.AppendFormat(@"SELECT ORDERED_BY_DEPT DEPT_CODE, COSTS COSTS,
                                                     CASE WHEN (A.ITEM_CODE LIKE '01%') THEN COSTS ELSE 0 END KSS,
                                                     DECODE(CLASS_ON_RECKONING,'A01',COSTS,0) XYXJ,
                                                     DECODE(CLASS_ON_RECKONING,'A01',CHARGES,0) XYDF,
                                                     DECODE(CLASS_ON_RECKONING,'A01',(COSTS - CHARGES),0) XYJM,
                                                     DECODE(CLASS_ON_RECKONING,'A02',COSTS,0) ZYXJ,
                                                     DECODE(CLASS_ON_RECKONING,'A02',CHARGES,0) ZYDF,
                                                     DECODE(CLASS_ON_RECKONING,'A02',(COSTS - CHARGES),0) ZYJM,
                                                     DECODE(CLASS_ON_RECKONING,'A03',COSTS,0) ZKYYXJ,
                                                     DECODE(CLASS_ON_RECKONING,'A03',CHARGES,0) ZKYYDF,
                                                     DECODE(CLASS_ON_RECKONING,'A03',(COSTS - CHARGES),0) ZKYYJM
                                                FROM {0}.OUTP_BILL_ITEMS A,
                                                     {0}.PRICE_LIST B,
                                                     {0}.OUTP_ORDER_DESC C
                                               WHERE A.VISIT_DATE = C.VISIT_DATE
                                                 AND A.VISIT_NO = C.VISIT_NO
                                                 AND A.ITEM_CLASS = B.ITEM_CLASS(+)
                                                 AND A.ITEM_CODE = B.ITEM_CODE(+)
                                                 AND A.ITEM_SPEC = B.ITEM_SPEC(+)
                                                 AND A.UNITS = B.UNITS(+)
                                                 AND A.VISIT_DATE >= B.START_DATE(+)
                                                 AND A.VISIT_DATE < NVL (B.STOP_DATE(+), SYSDATE)
                                                 AND A.VISIT_DATE >= TO_DATE ('{1}', 'yyyy-mm-dd')
                                                 AND A.VISIT_DATE < TO_DATE ('{2}', 'yyyy-mm-dd')
                                                 AND CLASS_ON_RECKONING LIKE 'A%'", DataUser.HISDATA, fromDate, ToDate);
                    break;
                case "zy":
                    strType.AppendFormat(@"SELECT DEPT_CODE, COSTS COSTS,
                                                     CASE WHEN (ITEM_CODE LIKE '01%') THEN COSTS ELSE 0 END KSS,
                                                     DECODE(RECK_CLASS,'A01',COSTS,0) XYXJ,
                                                     DECODE(RECK_CLASS,'A01',CHARGES,0) XYDF,
                                                     DECODE(RECK_CLASS,'A01',COUNT_INCOME,0) XYJM,
                                                     DECODE(RECK_CLASS,'A02',COSTS,0) ZYXJ,
                                                     DECODE(RECK_CLASS,'A02',CHARGES,0) ZYDF,
                                                     DECODE(RECK_CLASS,'A02',COUNT_INCOME,0) ZYJM,
                                                     DECODE(RECK_CLASS,'A03',COSTS,0) ZKYYXJ,
                                                     DECODE(RECK_CLASS,'A03',CHARGES,0) ZKYYDF,
                                                     DECODE(RECK_CLASS,'A03',COUNT_INCOME,0) ZKYYJM
                                                FROM {0}.INP_CLASS_INCOME_DAY A
                                               WHERE ST_DATE >= TO_DATE ('{1}', 'yyyy-mm-dd')
                                                 AND ST_DATE < TO_DATE ('{2}', 'yyyy-mm-dd')
                                                 AND RECK_CLASS LIKE 'A%'", DataUser.HISFACT, fromDate, ToDate);
                    break;
            }

            str.AppendFormat(@"
                                    SELECT AA.* FROM (SELECT   B.ACCOUNT_DEPT_CODE DEPT_CODE, B.DEPT_NAME, SUM (COSTS) ZJ,
                                             SUM (KSS) KSS, SUM (XYXJ) XYXJ, SUM (XYDF) XYDF, SUM (XYJM) XYJM,
                                             SUM (ZYXJ) ZYXJ, SUM (ZYDF) ZYDF, SUM (ZYJM) ZYJM,
                                             SUM (ZKYYXJ) ZKYYXJ, SUM (ZKYYDF) ZKYYDF, SUM (ZKYYJM) ZKYYJM
                                        FROM ({1}) A,
                                             {0}.SYS_DEPT_DICT B
                                       WHERE A.DEPT_CODE = B.DEPT_CODE {2}
                                    GROUP BY B.DEPT_NAME, B.ACCOUNT_DEPT_CODE) AA
                                    UNION ALL 
                                    SELECT '999999' DEPT_CODE, '合计' DEPT_NAME, SUM (ZJ) ZJ, SUM (KSS) KSS,
                                             SUM (XYXJ) XYXJ, SUM (XYDF) XYDF, SUM (XYJM) XYJM, SUM (ZYXJ) ZYXJ,
                                             SUM (ZYDF) ZYDF, SUM (ZYJM) ZYJM, SUM (ZKYYXJ) ZKYYXJ,
                                             SUM (ZKYYDF) ZKYYDF, SUM (ZKYYJM) ZKYYJM 
                                        FROM 
                                           (SELECT   B.ACCOUNT_DEPT_CODE DEPT_CODE, B.DEPT_NAME, SUM (COSTS) ZJ,
                                             SUM (KSS) KSS, SUM (XYXJ) XYXJ, SUM (XYDF) XYDF, SUM (XYJM) XYJM,
                                             SUM (ZYXJ) ZYXJ, SUM (ZYDF) ZYDF, SUM (ZYJM) ZYJM,
                                             SUM (ZKYYXJ) ZKYYXJ, SUM (ZKYYDF) ZKYYDF, SUM (ZKYYJM) ZKYYJM
                                             FROM ({1}) A,
                                             {0}.SYS_DEPT_DICT B
                                             WHERE A.DEPT_CODE = B.DEPT_CODE {2}
                                             GROUP BY B.DEPT_NAME, B.ACCOUNT_DEPT_CODE)", DataUser.COMM, strType.ToString(), deptCode);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 核算类别
        /// </summary>
        /// <returns></returns>
        public DataSet getReckItemClass()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT '全部' CLASS_CODE,'全部' CLASS_NAME FROM DUAL UNION ALL
                                    SELECT CLASS_CODE, CLASS_NAME
                                      FROM {0}.RECK_ITEM_CLASS_DICT
                                     WHERE LENGTH (CLASS_CODE) = 1", DataUser.HISDATA);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        public DataSet getDeptOutpOrdes(string fromdate, string todate, string power, string deptCode, string type)
        {
            power = power == "" ? "" : "AND C.DEPT_CODE IN (" + power + ")";
            deptCode = deptCode == "" ? "" : "AND C.DEPT_CODE = '" + deptCode + "'";
            type = type == "全部" ? "" : "AND E.CLASS_ON_RECKONING LIKE '" + type + "%'";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   TO_CHAR (A.VISIT_DATE, 'yyyy-mm-dd') VISIT_DATE,
                                             C.DEPT_CODE , C.DEPT_NAME , B.NAME,
                                             A.PATIENT_ID, E.CLASS_ON_MR, A.ITEM_NAME, E.PRICE, A.UNITS,
                                             TO_CHAR (A.AMOUNT) AMOUNT,
                                             CASE
                                                WHEN CHARGE_INDICATOR = '1'
                                                   THEN '已计费'
                                                ELSE '未计费'
                                             END CHARGE_INDICATOR,
                                             A.ORDERED_BY_DOCTOR, A.COSTS, A.CHARGES
                                        FROM {0}.OUTP_ORDERS_COSTS A,
                                             {0}.PAT_MASTER_INDEX B,
                                             {1}.SYS_DEPT_DICT C,
                                             {0}.PRICE_LIST E
                                       WHERE A.PATIENT_ID = B.PATIENT_ID
                                         AND A.ORDERED_BY_DEPT = C.DEPT_CODE
                                         AND C.ATTR = '是'
                                         AND A.ITEM_CLASS = E.ITEM_CLASS(+)
                                         AND A.ITEM_CODE = E.ITEM_CODE(+)
                                         AND A.ITEM_SPEC = E.ITEM_SPEC(+)
                                         AND A.UNITS = E.UNITS(+)
                                         AND A.VISIT_DATE >= E.START_DATE(+)
                                         AND A.VISIT_DATE < NVL (E.STOP_DATE(+), SYSDATE)
                                         AND A.VISIT_DATE >= TO_DATE ('{2}', 'yyyy-mm-dd')
                                         AND A.VISIT_DATE < TO_DATE ('{3}', 'yyyy-mm-dd') {4} {5} {6}
                                    ORDER BY A.VISIT_DATE", DataUser.HISDATA, DataUser.COMM, fromdate, todate, power, deptCode, type);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        public DataSet getInpCostInfo(string fromdate, string todate, string patinet_id, string inp_no, string power)
        {
            power = power == "" ? "" : "A.ORDERED_BY IN (" + power + ") AND A.PERFORMED_BY IN (" + power + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT TO_CHAR(A.BILLING_DATE_TIME, 'yyyy-mm-dd') DATE_TIME, B.NAME, A.ITEM_NAME,
                                       A.ITEM_SPEC, A.AMOUNT, A.UNITS,
                                       (SELECT DEPT_NAME
                                          FROM {0}.DEPT_DICT
                                         WHERE DEPT_CODE = A.ORDERED_BY) ORDERED_BY,
                                       (SELECT DEPT_NAME
                                          FROM {0}.DEPT_DICT
                                         WHERE DEPT_CODE = A.PERFORMED_BY) PERFORMED_BY, A.COSTS, A.CHARGES
                                  FROM {0}.INP_BILL_DETAIL A, {0}.PAT_MASTER_INDEX B
                                 WHERE A.PATIENT_ID = B.PATIENT_ID
                                   AND A.BILLING_DATE_TIME >= TO_DATE ('{1}', 'YYYY-MM-DD')
                                   AND A.BILLING_DATE_TIME < TO_DATE ('{2}', 'YYYY-MM-DD')
                                   AND A.PATIENT_ID ='{3}' 
                                   AND B.INP_NO = '{4}' {5}", DataUser.HISDATA, fromdate, todate, patinet_id, inp_no, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取二级科室
        /// </summary>
        /// <returns></returns>
        public DataTable GetSecondDept(string power)
        {
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            string sql = string.Format(@"SELECT DEPT_CODE, DEPT_NAME,0 CAUSE_VALUE,0 FACT_VALUE,0 COMPARE
                                        FROM   comm.sys_dept_dict
                                       WHERE  show_flag = '0' and attr='是' and dept_type != 3 and dept_type != 1 and dept_type != 2 and dept_type != 9 and  dept_code_second IS NOT NULL {0}
                                    ORDER BY   DEPT_CODE", power);

            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetGuideDict(string id)
        {
            string sql = string.Format(@"SELECT   b.GUIDE_CODE, b.GUIDE_NAME
                                          FROM   HOSPITALSYS.GUIDE_GATHERS a, HOSPITALSYS.GUIDE_NAME_DICT b
                                         WHERE   a.GUIDE_GATHER_CODE = '{0}' AND A.GUIDE_CODE = b.GUIDE_CODE ORDER BY A.RANKING", id);

            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tjrq"></param>
        /// <param name="months"></param>
        /// <param name="guidecode"></param>
        /// <param name="guidename"></param>
        /// <param name="incount"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public DataTable getPlanGuideStatsInfo(string tjrq, int months, string guidecode, string guidename, string incount, string power)
        {
            string tjrq_qianqizzhi = "";

            power = power == "" ? "" : "AND UNIT_CODE IN (" + power + ")";
            string[] rq = tjrq.Split(',');

            string[] rq1 = new string[2];
            rq1[0] = (Convert.ToInt32(rq[0].Substring(0, 4)) - 1).ToString() + "0101";
            rq1[1] = (Convert.ToInt32(rq[0].Substring(0, 4)) - 1).ToString() + "1231";
            tjrq_qianqizzhi = rq1[0] + "," + rq1[1];

            OleDbParameter[] parameters = {
            new OleDbParameter("tjrq", tjrq_qianqizzhi),
            new OleDbParameter("guidecode", guidecode),
            new OleDbParameter("incount",incount)
                                           };
            OracleOledbBase.RunProcedure("HOSPITALSYS.GUIDE_VALUE_ADD_SUM", parameters);


            string sql = "";
            sql = string.Format(@"SELECT  C1.DEPT_CODE,c1.DEPT_NAME,nvl(c2.GUIDE_VALUE,0)GUIDE_VALUE FROM   
                                     (
                                               select dept_CODE,dept_name from comm.sys_dept_dict
                                               where show_flag='0' and DEPT_TYPE<>'3'
                                                     AND DEPT_TYPE<>'1' AND attr='是' 
                                                     AND dept_code_second IS NOT NULL
                                             )c1,
                                             (
                                                 select UNIT_CODE,GUIDE_VALUE 
                                                 from HOSPITALSYS.guide_value_SUM
                                                 where guide_type = 'K' AND counting = '{0}'
                                                 and (
                                                     STARTDATE>=to_date('{2}','yyyyMMdd')
                                                     or ENDDATE<=to_date('{3}','yyyyMMdd')                                         
                                                 )         
                                                 AND GUIDE_CODE = '{1}'
                                             ) c2
                                     WHERE   c1.dept_code=c2.UNIT_CODE(+)  
                                    {4}                                                                                
                                    order by dept_CODE", incount, guidecode, rq1[0], rq1[1], power);
            DataTable dt_qian = OracleOledbBase.ExecuteDataSet(sql).Tables[0];
            sql = "";


            OleDbParameter[] parameters1 = {
            new OleDbParameter("tjrq", tjrq),
            new OleDbParameter("guidecode", guidecode),
            new OleDbParameter("incount",incount)
                                           };
            OracleOledbBase.RunProcedure("HOSPITALSYS.GUIDE_VALUE_ADD_SUM", parameters1);
            sql = string.Format(@"SELECT  A.UNIT_CODE DEPT_CODE,nvl(a.GUIDE_VALUE,0)GUIDE_VALUE         
                                  FROM   HOSPITALSYS.guide_value_SUM a
                                 WHERE       a.guide_type = 'K'
                                         AND a.counting = '{0}'
                                         and (
                                         A.STARTDATE>=to_date('{2}','yyyyMMdd')
                                         or A.ENDDATE<=to_date('{3}','yyyyMMdd')                                         
                                         )         
                                         AND A.GUIDE_CODE = '{1}' {4}
                                        order by UNIT_CODE", incount, guidecode, rq[0], rq[1], power);
            DataTable dt_hou = OracleOledbBase.ExecuteDataSet(sql).Tables[0];

            bool flags = guidename.Contains("率") || guidename.Contains("均") || guidename.Contains("比") ? true : false;
            DataTable dtt = new DataTable();
            DataColumn col1 = new DataColumn("DEPT_CODE");
            DataColumn col2 = new DataColumn("DEPT_NAME");
            DataColumn col3 = new DataColumn("CAUSE_VALUE");
            DataColumn col4 = new DataColumn("FACT_VALUE");
            DataColumn col5 = new DataColumn("COMPARE");
            DataColumn col6 = new DataColumn("SUB_VALUE");
            dtt.Columns.Add(col1);
            dtt.Columns.Add(col2);
            dtt.Columns.Add(col3);
            dtt.Columns.Add(col4);
            dtt.Columns.Add(col5);
            dtt.Columns.Add(col6);
            DataRow drr;
            int length = dt_hou.Rows.Count;
            foreach (DataRow dr in dt_qian.Rows)
            {
                drr = dtt.NewRow();
                drr["DEPT_CODE"] = dr["DEPT_CODE"];
                drr["DEPT_NAME"] = dr["DEPT_NAME"];
                if (flags == false)
                    drr["CAUSE_VALUE"] = Math.Round(Convert.ToDouble(dr["GUIDE_VALUE"]) / 12 * months, 2);
                else
                    drr["CAUSE_VALUE"] = dr["GUIDE_VALUE"];
                drr["FACT_VALUE"] = "0";
                drr["COMPARE"] = "0";
                drr["SUB_VALUE"] = "0";
                for (int i = 0; i < length; )
                {
                    if (dr["DEPT_CODE"].ToString() == dt_hou.Rows[i]["DEPT_CODE"].ToString())
                    {
                        drr["FACT_VALUE"] = dt_hou.Rows[i]["GUIDE_VALUE"];
                        if (drr["CAUSE_VALUE"].ToString() == "0")
                            drr["COMPARE"] = "0";
                        else
                            drr["COMPARE"] = Math.Round((Convert.ToDouble(drr["FACT_VALUE"]) - Convert.ToDouble(drr["CAUSE_VALUE"])) / Convert.ToDouble(drr["CAUSE_VALUE"]) * 100, 2);
                        drr["SUB_VALUE"] = Math.Round(Convert.ToDouble(drr["FACT_VALUE"]) - Convert.ToDouble(drr["CAUSE_VALUE"]), 2);

                        dt_hou.Rows.RemoveAt(i);
                        length--;
                        break;
                    }
                    else
                        i++;
                }
                dtt.Rows.Add(drr);
            }
            return dtt;
        }

        /// <summary>
        /// 保存目标值
        /// </summary>
        /// <param name="PersonBouns"></param>
        public void SavePlanGuideStats(Dictionary<string, string>[] selectRow, string guide_code)
        {
            MyLists listttrans = new MyLists();

            List list = new List();
            list.StrSql = string.Format(@"delete HOSPITALSYS.PLANGUIDESTATS_DICT where GUIDE_CODE = '{0}'", guide_code);
            list.Parameters = new OleDbParameter[] { };
            listttrans.Add(list);
            for (int i = 0; i < selectRow.Length; i++)
            {
                StringBuilder strdetail = new StringBuilder();
                strdetail.Append("insert into HOSPITALSYS.PLANGUIDESTATS_DICT(");
                strdetail.Append("DEPT_CODE,DEPT_NAME,CAUSE_VALUE,GUIDE_CODE)");
                strdetail.Append(" values (");
                strdetail.Append("?,?,?,?)");

                OleDbParameter[] parameterdetail = {
					            new OleDbParameter("DEPT_CODE", selectRow[i]["DEPT_CODE"].ToString()),
					            new OleDbParameter("DEPT_NAME", selectRow[i]["DEPT_NAME"].ToString()),                    
                                new OleDbParameter("CAUSE_VALUE", Convert.ToDouble(selectRow[i]["CAUSE_VALUE"])),
                                new OleDbParameter("GUIDE_CODE", guide_code) 
                        };
                List listdetail = new List();
                listdetail.StrSql = strdetail.ToString();
                listdetail.Parameters = parameterdetail;
                listttrans.Add(listdetail);
            }

            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 根据科室编码和岗位年度，获取岗位名称、岗位说明、所属部门、录入员、录入时间，用于显示
        /// </summary>
        /// <param name="dept_code">科室编码</param>
        /// <param name="station_year">岗位年度</param>
        /// <returns></returns>
        public DataSet GetStationListByDeptCode(string dept_code, string station_year)
        {
            string strdict = string.Format("select GUIDE_CODE from hospitalsys.guide_year_cause_dict order by GUIDE_CODE");
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];

            StringBuilder strb = new StringBuilder();
            if (station_year.Equals(""))
            {
                station_year = DateTime.Now.Year.ToString();
            }

            strb.AppendFormat("SELECT b.dept_code DEPTCODE,c.dept_name DEPTNAME,a.station_code STATIONCODE,b.station_name STATIONNAME ");
            for (int i = 0; i < tabledict.Rows.Count; i++)
            {
                strb.AppendFormat(" ,sum(case when a.guide_code='{0}' then a.guide_cause else 0 end) A{0} ", tabledict.Rows[i]["GUIDE_CODE"].ToString());
            }
            strb.AppendFormat(@"  FROM hospitalsys.station_guide_information a,
                                       comm.sys_station_basic_information b,
                                       COMM.sys_dept_dict c
                                 WHERE A.STATION_CODE=B.STATION_CODE
                                   and b.dept_code = c.dept_code
                                   AND A.station_year = '{0}'
                                   AND B.STATION_YEAR='{0}' AND A.GUIDE_CODE IN (SELECT GUIDE_CODE FROM hospitalsys.guide_year_cause_dict)
                                   group by b.dept_code , c.dept_name , a.station_code , b.station_name 
                                   ORDER BY b.dept_code", station_year);

            return OracleOledbBase.ExecuteDataSet(strb.ToString());
        }


        /// <summary>
        /// 获取列表的所有列
        /// </summary>
        /// <param name="bonusId"></param>
        /// <param name="bonusType"></param>
        /// <param name="store"></param>
        /// <param name="gridpanel"></param>
        public DataTable BuildDeptBonusDetail()
        {
            string strcode = "";
            strcode = String.Format(@"SELECT 'A'||A.GUIDE_CODE guide_code,A.GUIDE_TYPE,A.SHOW_WIDTH,A.SHOW_STYLE,B.GUIDE_NAME
                                      FROM hospitalsys.guide_year_cause_dict a, hospitalsys.guide_name_dict b
                                     WHERE a.guide_code = b.guide_code");

            return OracleOledbBase.ExecuteDataSet(strcode).Tables[0];

        }


        /// <summary>
        /// 保存年目标值
        /// </summary>
        /// <param name="selectRow"></param>
        /// <param name="dept_code"></param>
        /// <param name="year"></param>
        public void SavePlanGuideForYear(Dictionary<string, string>[] selectRow, string year)
        {
            MyLists listttrans = new MyLists();
            List list = new List();

            for (int i = 0; i < selectRow.Length; i++)
            {
                //获取科室对应的岗位信息
                //dd = GetStationListByDeptCode(selectRow[i]["DEPT_CODE"], year).Tables[0];
                string strdict = string.Format("select GUIDE_CODE from hospitalsys.guide_year_cause_dict order by GUIDE_CODE");
                DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];

                foreach (DataRow dr in tabledict.Rows)
                {
                    string guidename = "A"+dr["GUIDE_CODE"].ToString();

                    StringBuilder strdetail = new StringBuilder();
                    strdetail.AppendFormat(@"UPDATE HOSPITALSYS.STATION_GUIDE_INFORMATION set GUIDE_CAUSE={0} 
                    WHERE STATION_CODE={1} and STATION_YEAR= {2}  and GUIDE_CODE='{3}'",
                    Convert.ToDouble(selectRow[i][guidename]), selectRow[i]["STATIONCODE"].ToString(), year, dr["GUIDE_CODE"].ToString());
                    List listdetail = new List();
                    listdetail.StrSql = strdetail.ToString();
                    listttrans.Add(listdetail);
                }
            }

            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 获取年计划指标值
        /// </summary>
        /// <param name="years"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataSet getPlanYearValueDataBind(string years, string key)
        {
            //dept.dept_code_second =DEPT.DEPT_CODE AND 
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT DEPT_CODE,DEPT_NAME,
                                MAX(CASE WHEN GUIDE_CODE='20101004' THEN GUIDE_CAUSE ELSE 0 END ) A20101004,
                                MAX(CASE WHEN GUIDE_CODE='20110001' THEN GUIDE_CAUSE ELSE 0 END ) A20110001,
                                MAX(CASE WHEN GUIDE_CODE='20202030' THEN GUIDE_CAUSE ELSE 0 END ) A20202030
                                FROM
                                (
                                SELECT DEPT_CODE,DEPT_NAME,GUIDE_CODE,MAX(GUIDE_CAUSE) GUIDE_CAUSE FROM
                                (
                                select a.*,b.GUIDE_CAUSE from 
                                (SELECT   DEPT.dept_code_second,dept.DEPT_TYPE,dept.dept_code,dept.dept_name,station.station_code,station.station_name,C.GUIDE_CODE
                                        FROM comm.sys_station_basic_information station RIGHT JOIN comm.sys_dept_dict dept
                                             ON station.dept_code = dept.dept_code and station.station_year = {0} 
                                             JOIN HOSPITALSYS.GUIDE_GATHERS C
                                             ON C.GUIDE_GATHER_CODE='{1}'
                                       where dept.DEPT_TYPE != 3 
                                         AND dept.DEPT_TYPE != 1
                                         AND dept.DEPT_TYPE != 9
                                         and attr='是'
                                         AND dept.show_flag='0'
                                         AND dept.dept_lcattr is not null
                                    order by dept.dept_code,STATION.STATION_CODE) a,
                                (SELECT a.station_code,a.guide_code,a.guide_cause
                                  FROM hospitalsys.station_guide_information a join HOSPITALSYS.GUIDE_GATHERS b
                                    on a.guide_code=b.guide_code and a.station_year = {0} and b.GUIDE_GATHER_CODE='{1}') b
                                    
                                where a.station_code=b.station_code(+)
                                  AND A.GUIDE_CODE=B.GUIDE_CODE(+)
                                order by a.dept_code,A.STATION_CODE,A.GUIDE_CODE
                                )
                                GROUP BY DEPT_CODE,DEPT_NAME,GUIDE_CODE
                                )
                                GROUP BY DEPT_CODE,DEPT_NAME 
                                ORDER BY DEPT_CODE,DEPT_NAME ", years, GetConfig.GetConfigString(key));
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

    }
}
