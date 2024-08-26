using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.Comm;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    public class OperationInfo
    {
        public void AddOperationInfo(string st_date, string dept_code, string dept_name, string level_j, string _operator, string OPERATOR_NAME, string FIRST_ASSISTANT, string FIRST_ASSISTANT_N, string SECOND_ASSISTANT, string SECOND_ASSISTANT_N, string ANESTHESIA_DOCTOR, string ANESTHESIA_DOCTOR_N, string EMERGENCY, string HS1, string HS_NAME1, string HS2, string HS_NAME2, string HS3, string HS_NAME3, string HS4, string HS_NAME4, string HS5, string HS_NAME5)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"INSERT INTO PERFORMANCE.OPERATIONINFO (ID,
                           ST_DATE,
                           DEPT_CODE,
                           DEPT_NAME,
                           LEVEL_J,
                           OPERATOR,
                           OPERATOR_NAME,
                           FIRST_ASSISTANT,
                           FIRST_ASSISTANT_N,
                           SECOND_ASSISTANT,
                           SECOND_ASSISTANT_N,
                           ANESTHESIA_DOCTOR,
                           ANESTHESIA_DOCTOR_N,
                           EMERGENCY,
                           HS1,
                           HS_NAME1,
                           HS2,
                           HS_NAME2,
                           HS3,
                           HS_NAME3,
                           HS4,
                           HS_NAME4,
                           HS5,
                           HS_NAME5)
  VALUES   ({0},to_date('{1}','yyyy-mm-dd'),'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                                                                                                OracleOledbBase.GetMaxID("id", "PERFORMANCE.OPERATIONINFO"),
                                                                                                                                st_date,dept_code,dept_name,level_j,_operator,
                                                                                                                                OPERATOR_NAME,FIRST_ASSISTANT,FIRST_ASSISTANT_N,
                                                                                                                                SECOND_ASSISTANT, SECOND_ASSISTANT_N, ANESTHESIA_DOCTOR,
                                                                                                                                ANESTHESIA_DOCTOR_N, EMERGENCY, HS1, HS_NAME1,
                                                                                                                                HS2, HS_NAME2, HS3, HS_NAME3, HS4, HS_NAME4, HS5, HS_NAME5);
            OracleOledbBase.ExecuteNonQuery(sql.ToString());
        }

        public DataTable GetOperationInfoByDate(string st_date)
        {
            string sql = string.Format(@"select ID,
         TO_CHAR (ST_DATE, 'yyyy-mm-dd') ST_DATE,
         DEPT_CODE,
         DEPT_NAME,
         CASE
            WHEN LEVEL_J = '1' THEN '一级'
            WHEN LEVEL_J = '2' THEN '二级'
            WHEN LEVEL_J = '3' THEN '三级'
            WHEN LEVEL_J = '4' THEN '四级'
         END
            LEVEL_J,
         OPERATOR,
         OPERATOR_NAME,
         FIRST_ASSISTANT,
         FIRST_ASSISTANT_N,
         SECOND_ASSISTANT,
         SECOND_ASSISTANT_N,
         ANESTHESIA_DOCTOR,
         ANESTHESIA_DOCTOR_N,
         CASE
            WHEN EMERGENCY = '0' THEN '不是'
            WHEN EMERGENCY = '1' THEN '是'
         END
            EMERGENCY,
         HS1,
         HS_NAME1,
         HS2,
         HS_NAME2,
         HS3,
         HS_NAME3,
         HS4,
         HS_NAME4,
         HS5,
         HS_NAME5 from  PERFORMANCE.OPERATIONINFO where to_char(st_date,'yyyymm')='{0}'", st_date);
            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        public void EditOperationinfo(string id,string st_date, string dept_code, string dept_name, string level_j, string _operator, string OPERATOR_NAME, string FIRST_ASSISTANT, string FIRST_ASSISTANT_N, string SECOND_ASSISTANT, string SECOND_ASSISTANT_N, string ANESTHESIA_DOCTOR, string ANESTHESIA_DOCTOR_N, string EMERGENCY, string HS1, string HS_NAME1, string HS2, string HS_NAME2, string HS3, string HS_NAME3, string HS4, string HS_NAME4, string HS5, string HS_NAME5)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"update PERFORMANCE.OPERATIONINFO set
                           ST_DATE=to_date('{1}','yyyy-mm-dd'),
                           DEPT_CODE='{2}',
                           DEPT_NAME='{3}',
                           LEVEL_J='{4}',
                           OPERATOR='{5}',
                           OPERATOR_NAME='{6}',
                           FIRST_ASSISTANT='{7}',
                           FIRST_ASSISTANT_N='{8}',
                           SECOND_ASSISTANT='{9}',
                           SECOND_ASSISTANT_N='{10}',
                           ANESTHESIA_DOCTOR='{11}',
                           ANESTHESIA_DOCTOR_N='{12}',
                           EMERGENCY='{13}',
                           HS1='{14}',
                           HS_NAME1='{15}',
                           HS2='{16}',
                           HS_NAME2='{17}',
                           HS3='{18}',
                           HS_NAME3='{19}',
                           HS4='{20}',
                           HS_NAME4='{21}',
                           HS5='{22}',
                           HS_NAME5='{23}' where id={0}", id, st_date, dept_code, dept_name, level_j, _operator,
                                                          OPERATOR_NAME, FIRST_ASSISTANT, FIRST_ASSISTANT_N,
                                                          SECOND_ASSISTANT, SECOND_ASSISTANT_N, ANESTHESIA_DOCTOR,
                                                          ANESTHESIA_DOCTOR_N, EMERGENCY, HS1, HS_NAME1,
                                                          HS2, HS_NAME2, HS3, HS_NAME3, HS4, HS_NAME4, HS5, HS_NAME5);
            OracleOledbBase.ExecuteNonQuery(sql.ToString());
        }

        public DataTable GetOperationInfoById(string id)
        {
            string sql = string.Format(@"select ID,
         TO_CHAR (ST_DATE, 'yyyy-mm-dd') ST_DATE,
         DEPT_CODE,
         DEPT_NAME,
        
            LEVEL_J,
         OPERATOR,
         OPERATOR_NAME,
         FIRST_ASSISTANT,
         FIRST_ASSISTANT_N,
         SECOND_ASSISTANT,
         SECOND_ASSISTANT_N,
         ANESTHESIA_DOCTOR,
         ANESTHESIA_DOCTOR_N,
         
            EMERGENCY,
         HS1,
         HS_NAME1,
         HS2,
         HS_NAME2,
         HS3,
         HS_NAME3,
         HS4,
         HS_NAME4,
         HS5,
         HS_NAME5 from  PERFORMANCE.OPERATIONINFO where id={0}",id);
            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        public void DelOperation(string id)
        {
            string sql = string.Format(@"delete from PERFORMANCE.OPERATIONINFO where id={0}", id);
            OracleOledbBase.ExecuteNonQuery(sql);
        }

//        /// <summary>
//        /// 手术单项奖励（护士按人给的）
//        /// </summary>
//        /// <param name="s_date"></param>
//        /// <param name="d_date"></param>
//        /// <returns></returns>
//        public DataTable GetOperationInfo(string s_date, string d_date)
//        {
//            StringBuilder sql = new StringBuilder();

//            sql.AppendFormat(@"
//  SELECT   decode(ACCOUNT_DEPT_NAME,null,'总计',ACCOUNT_DEPT_NAME) as  ACCOUNT_DEPT_NAME, decode(THENUMBERFO,null,'小计',THENUMBERFO) THENUMBERFO, sum(MFS) MFS
//    FROM   (  SELECT   B.ACCOUNT_DEPT_name,
//                       A.OPERATOR TheNumberFo,
//                       SUM(                                     --临床主刀医生
//                           CASE
//                              WHEN A.LEVEL_J = '1'
//                              THEN
//                                 (CASE
//                                     WHEN A.EMERGENCY = '1'
//                                     THEN
//                                        (CASE
//                                            WHEN FIRST_ASSISTANT = NULL
//                                                 AND SECOND_ASSISTANT = NULL
//                                            THEN
//                                               (50 + 20)
//                                            ELSE
//                                               (50 + 20) * 0.5
//                                         END)
//                                     ELSE
//                                        (CASE
//                                            WHEN FIRST_ASSISTANT = NULL
//                                                 AND SECOND_ASSISTANT = NULL
//                                            THEN
//                                               50
//                                            ELSE
//                                               50 * 0.5
//                                         END)
//                                  END)
//                              --二级
//                           WHEN A.LEVEL_J = '2'
//                              THEN
//                                 (CASE
//                                     WHEN A.EMERGENCY = '1'
//                                     THEN
//                                        (CASE
//                                            WHEN FIRST_ASSISTANT = NULL
//                                                 AND SECOND_ASSISTANT = NULL
//                                            THEN
//                                               (100 + 20)
//                                            ELSE
//                                               (100 + 20) * 0.5
//                                         END)
//                                     ELSE
//                                        (CASE
//                                            WHEN FIRST_ASSISTANT = NULL
//                                                 AND SECOND_ASSISTANT = NULL
//                                            THEN
//                                               +100
//                                            ELSE
//                                               +100 * 0.5
//                                         END)
//                                  END)
//                              --三级
//                           WHEN A.LEVEL_J = '3'
//                              THEN
//                                 (CASE
//                                     WHEN A.EMERGENCY = '1'
//                                     THEN
//                                        (CASE
//                                            WHEN FIRST_ASSISTANT = NULL
//                                                 AND SECOND_ASSISTANT = NULL
//                                            THEN
//                                               (200 + 20)
//                                            ELSE
//                                               (200 + 20) * 0.5
//                                         END)
//                                     ELSE
//                                        (CASE
//                                            WHEN FIRST_ASSISTANT = NULL
//                                                 AND SECOND_ASSISTANT = NULL
//                                            THEN
//                                               200
//                                            ELSE
//                                               200 * 0.5
//                                         END)
//                                  END)
//                              --四级
//                           WHEN A.LEVEL_J = '4'
//                              THEN
//                                 (CASE
//                                     WHEN A.EMERGENCY = '1'
//                                     THEN
//                                        (CASE
//                                            WHEN FIRST_ASSISTANT = NULL
//                                                 AND SECOND_ASSISTANT = NULL
//                                            THEN
//                                               + (300 + 20)
//                                            ELSE
//                                               +300 * 0.5 + 20
//                                         END)
//                                     ELSE
//                                        (CASE
//                                            WHEN FIRST_ASSISTANT = NULL
//                                                 AND SECOND_ASSISTANT = NULL
//                                            THEN
//                                               +300
//                                            ELSE
//                                               +300 * 0.5
//                                         END)
//                                  END)
//                           END)
//                          mfs
//                FROM   PERFORMANCE.OPERATIONINFO A, COMM.SYS_DEPT_dict B
//               WHERE       TO_CHAR (a.ST_DATE, 'yyyymmdd') >= '{0}'
//                       AND TO_CHAR (a.ST_DATE, 'yyyymmdd') <= '{1}'
//                       AND a.DEPT_NAME = B.DEPT_CODE
//            GROUP BY   B.ACCOUNT_DEPT_name, A.OPERATOR
//            UNION ALL
//              SELECT   B.ACCOUNT_DEPT_name,
//                       A.FIRST_ASSISTANT TheNumberFo,
//                       CASE
//                          WHEN FIRST_ASSISTANT IS NOT NULL
//                          THEN
//                             SUM(                               --临床一助医生
//                                 CASE
//                                    WHEN A.LEVEL_J = '1'
//                                    THEN
//                                       (CASE
//                                           WHEN A.EMERGENCY = '1'
//                                           THEN
//                                              (CASE
//                                                  WHEN SECOND_ASSISTANT = NULL
//                                                  THEN
//                                                     (50 + 20) * 0.5
//                                                  ELSE
//                                                     (50 + 20) * 0.3
//                                               END)
//                                           ELSE
//                                              (CASE
//                                                  WHEN SECOND_ASSISTANT = NULL
//                                                  THEN
//                                                     50 * 0.5
//                                                  ELSE
//                                                     50 * 0.3
//                                               END)
//                                        END)
//                                    --二级
//                                 WHEN A.LEVEL_J = '2'
//                                    THEN
//                                       (CASE
//                                           WHEN A.EMERGENCY = '1'
//                                           THEN
//                                              (CASE
//                                                  WHEN SECOND_ASSISTANT = NULL
//                                                  THEN
//                                                     (100 + 20) * 0.5
//                                                  ELSE
//                                                     (100 + 20) * 0.3
//                                               END)
//                                           ELSE
//                                              (CASE
//                                                  WHEN SECOND_ASSISTANT = NULL
//                                                  THEN
//                                                     100 * 0.5
//                                                  ELSE
//                                                     100 * 0.3
//                                               END)
//                                        END)
//                                    --三级
//                                 WHEN A.LEVEL_J = '3'
//                                    THEN
//                                       (CASE
//                                           WHEN A.EMERGENCY = '1'
//                                           THEN
//                                              (CASE
//                                                  WHEN SECOND_ASSISTANT = NULL
//                                                  THEN
//                                                     (200 + 20) * 0.5
//                                                  ELSE
//                                                     (200 + 20) * 0.3
//                                               END)
//                                           ELSE
//                                              (CASE
//                                                  WHEN SECOND_ASSISTANT = NULL
//                                                  THEN
//                                                     200 * 0.5
//                                                  ELSE
//                                                     200 * 0.3
//                                               END)
//                                        END)
//                                    --四级
//                                 WHEN A.LEVEL_J = '4'
//                                    THEN
//                                       (CASE
//                                           WHEN A.EMERGENCY = '1'
//                                           THEN
//                                              (CASE
//                                                  WHEN SECOND_ASSISTANT = NULL
//                                                  THEN
//                                                     (300 + 20) * 0.5
//                                                  ELSE
//                                                     (300 + 20) * 0.3
//                                               END)
//                                           ELSE
//                                              (CASE
//                                                  WHEN SECOND_ASSISTANT = NULL
//                                                  THEN
//                                                     300 * 0.5
//                                                  ELSE
//                                                     300 * 0.3
//                                               END)
//                                        END)
//                                 END)
//                          ELSE
//                             0
//                       END
//                          mfs
//                FROM   PERFORMANCE.OPERATIONINFO A, COMM.SYS_DEPT_dict B
//               WHERE       TO_CHAR (a.ST_DATE, 'yyyymmdd') >= '{0}'
//                       AND TO_CHAR (a.ST_DATE, 'yyyymmdd') <= '{1}'
//                       AND a.DEPT_NAME = B.DEPT_CODE
//            GROUP BY   B.ACCOUNT_DEPT_name, A.FIRST_ASSISTANT
//            UNION ALL
//              SELECT   B.ACCOUNT_DEPT_name,
//                       A.SECOND_ASSISTANT TheNumberFo,
//                       CASE
//                          WHEN SECOND_ASSISTANT IS NOT NULL
//                          THEN
//                             SUM(                               --临床二助医生
//                                 CASE
//                                    WHEN A.LEVEL_J = '1'
//                                    THEN
//                                       (CASE
//                                           WHEN A.EMERGENCY = '1'
//                                           THEN
//                                              (50 + 20) * 0.2
//                                           ELSE
//                                              50 * 0.2
//                                        END)
//                                    --二级
//                                 WHEN A.LEVEL_J = '2'
//                                    THEN
//                                       (CASE
//                                           WHEN A.EMERGENCY = '1'
//                                           THEN
//                                              (100 + 20) * 0.2
//                                           ELSE
//                                              100 * 0.2
//                                        END)
//                                    --三级
//                                 WHEN A.LEVEL_J = '3'
//                                    THEN
//                                       (CASE
//                                           WHEN A.EMERGENCY = '1'
//                                           THEN
//                                              (200 + 20) * 0.2
//                                           ELSE
//                                              200 * 0.2
//                                        END)
//                                    --四级
//                                 WHEN A.LEVEL_J = '4'
//                                    THEN
//                                       (CASE
//                                           WHEN A.EMERGENCY = '1'
//                                           THEN
//                                              (300 + 20) * 0.2
//                                           ELSE
//                                              300 * 0.2
//                                        END)
//                                 END)
//                          ELSE
//                             0
//                       END
//                          mfs
//                FROM   PERFORMANCE.OPERATIONINFO A, COMM.SYS_DEPT_dict B
//               WHERE       TO_CHAR (a.ST_DATE, 'yyyymmdd') >= '{0}'
//                       AND TO_CHAR (a.ST_DATE, 'yyyymmdd') <= '{1}'
//                       AND a.DEPT_NAME = B.DEPT_CODE
//            GROUP BY   B.ACCOUNT_DEPT_name, A.SECOND_ASSISTANT
//            UNION ALL
//            SELECT   'a麻醉师' ACCOUNT_DEPT_name,
//           ANESTHESIA_DOCTOR TheNumberFo,
//           SUM(                                                 --临床麻醉医生
//               CASE
//                  WHEN A.LEVEL_J = '1'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE
//                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL
//                               THEN
//                                  10 + 10
//                               ELSE
//                                  0
//                            END
//                         ELSE
//                            CASE
//                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL THEN 10
//                               ELSE 0
//                            END
//                      END)
//                  --二级
//               WHEN A.LEVEL_J = '2'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE
//                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL
//                               THEN
//                                  20 + 10
//                               ELSE
//                                  0
//                            END
//                         ELSE
//                            CASE
//                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL THEN 20
//                               ELSE 0
//                            END
//                      END)
//                  --三级
//               WHEN A.LEVEL_J = '3'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE
//                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL
//                               THEN
//                                  30 + 10
//                               ELSE
//                                  0
//                            END
//                         ELSE
//                            CASE
//                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL THEN 30
//                               ELSE 0
//                            END
//                      END)
//                  --四级
//               WHEN A.LEVEL_J = '4'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE
//                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL
//                               THEN
//                                  50 + 10
//                               ELSE
//                                  0
//                            END
//                         ELSE
//                            CASE
//                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL THEN 50
//                               ELSE 0
//                            END
//                      END)
//               END)
//              mfs
//    FROM   PERFORMANCE.OPERATIONINFO A, COMM.SYS_DEPT_dict B
//   WHERE       TO_CHAR (a.ST_DATE, 'yyyymmdd') >= '{0}'
//           AND TO_CHAR (a.ST_DATE, 'yyyymmdd') <= '{1}'
//           AND a.DEPT_NAME = B.DEPT_CODE
//GROUP BY   ANESTHESIA_DOCTOR
//            UNION ALL
//  SELECT   'a护士' ACCOUNT_DEPT_name,
//           HS1 TheNumberFo,
//           SUM(                                                 --临床护士医生
//               CASE
//                  WHEN A.LEVEL_J = '1'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS1 IS NOT NULL THEN 5 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS1 IS NOT NULL THEN 5 ELSE 0 END
//                      END)
//                  --二级
//               WHEN A.LEVEL_J = '2'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS1 IS NOT NULL THEN 10 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS1 IS NOT NULL THEN 10 ELSE 0 END
//                      END)
//                  --三级
//               WHEN A.LEVEL_J = '3'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS1 IS NOT NULL THEN 15 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS1 IS NOT NULL THEN 15 ELSE 0 END
//                      END)
//                  --四级
//               WHEN A.LEVEL_J = '4'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS1 IS NOT NULL THEN 20 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS1 IS NOT NULL THEN 20 ELSE 0 END
//                      END)
//               END)
//              mfs
//    FROM   PERFORMANCE.OPERATIONINFO A, COMM.SYS_DEPT_dict B
//   WHERE       TO_CHAR (a.ST_DATE, 'yyyymmdd') >= '{0}'
//           AND TO_CHAR (a.ST_DATE, 'yyyymmdd') <= '{1}'
//           AND a.DEPT_NAME = B.DEPT_CODE
//GROUP BY   HS1
//union all
// SELECT   'a护士' ACCOUNT_DEPT_name,
//           HS2 TheNumberFo,
//           SUM(                                                 --临床护士医生
//               CASE
//                  WHEN A.LEVEL_J = '1'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS2 IS NOT NULL THEN 5 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS2 IS NOT NULL THEN 5 ELSE 0 END
//                      END)
//                  --二级
//               WHEN A.LEVEL_J = '2'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS2 IS NOT NULL THEN 10 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS2 IS NOT NULL THEN 10 ELSE 0 END
//                      END)
//                  --三级
//               WHEN A.LEVEL_J = '3'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS2 IS NOT NULL THEN 15 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS2 IS NOT NULL THEN 15 ELSE 0 END
//                      END)
//                  --四级
//               WHEN A.LEVEL_J = '4'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS2 IS NOT NULL THEN 20 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS2 IS NOT NULL THEN 20 ELSE 0 END
//                      END)
//               END)
//              mfs
//    FROM   PERFORMANCE.OPERATIONINFO A, COMM.SYS_DEPT_dict B
//   WHERE       TO_CHAR (a.ST_DATE, 'yyyymmdd') >= '{0}'
//           AND TO_CHAR (a.ST_DATE, 'yyyymmdd') <= '{1}'
//           AND a.DEPT_NAME = B.DEPT_CODE
//GROUP BY   HS2
//union all
// SELECT   'a护士' ACCOUNT_DEPT_name,
//           HS3 TheNumberFo,
//           SUM(                                                 --临床护士医生
//               CASE
//                  WHEN A.LEVEL_J = '1'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS3 IS NOT NULL THEN 5 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS3 IS NOT NULL THEN 5 ELSE 0 END
//                      END)
//                  --二级
//               WHEN A.LEVEL_J = '2'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS3 IS NOT NULL THEN 10 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS3 IS NOT NULL THEN 10 ELSE 0 END
//                      END)
//                  --三级
//               WHEN A.LEVEL_J = '3'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS3 IS NOT NULL THEN 15 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS3 IS NOT NULL THEN 15 ELSE 0 END
//                      END)
//                  --四级
//               WHEN A.LEVEL_J = '4'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS3 IS NOT NULL THEN 20 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS3 IS NOT NULL THEN 20 ELSE 0 END
//                      END)
//               END)
//              mfs
//    FROM   PERFORMANCE.OPERATIONINFO A, COMM.SYS_DEPT_dict B
//   WHERE       TO_CHAR (a.ST_DATE, 'yyyymmdd') >= '{0}'
//           AND TO_CHAR (a.ST_DATE, 'yyyymmdd') <= '{1}'
//           AND a.DEPT_NAME = B.DEPT_CODE
//GROUP BY   HS3
//union all
// SELECT   'a护士' ACCOUNT_DEPT_name,
//           HS4 TheNumberFo,
//           SUM(                                                 --临床护士医生
//               CASE
//                  WHEN A.LEVEL_J = '1'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS4 IS NOT NULL THEN 5 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS4 IS NOT NULL THEN 5 ELSE 0 END
//                      END)
//                  --二级
//               WHEN A.LEVEL_J = '2'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS4 IS NOT NULL THEN 10 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS4 IS NOT NULL THEN 10 ELSE 0 END
//                      END)
//                  --三级
//               WHEN A.LEVEL_J = '3'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS4 IS NOT NULL THEN 15 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS4 IS NOT NULL THEN 15 ELSE 0 END
//                      END)
//                  --四级
//               WHEN A.LEVEL_J = '4'
//                  THEN
//                     (CASE
//                         WHEN A.EMERGENCY = '1'
//                         THEN
//                            CASE WHEN HS4 IS NOT NULL THEN 20 + 5 ELSE 0 END
//                         ELSE
//                            CASE WHEN HS4 IS NOT NULL THEN 20 ELSE 0 END
//                      END)
//               END)
//              mfs
//    FROM   PERFORMANCE.OPERATIONINFO A, COMM.SYS_DEPT_dict B
//   WHERE       TO_CHAR (a.ST_DATE, 'yyyymmdd') >= '{0}'
//           AND TO_CHAR (a.ST_DATE, 'yyyymmdd') <= '{1}'
//           AND a.DEPT_NAME = B.DEPT_CODE
//GROUP BY   HS4)
//   WHERE   THENUMBERFO IS NOT NULL
//GROUP BY   ROLLUP (ACCOUNT_DEPT_NAME, THENUMBERFO)", s_date, d_date);

//            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
//        }

        /// <summary>
        /// 手术单项奖励（护士按每例给的）
        /// </summary>
        /// <param name="s_date"></param>
        /// <param name="d_date"></param>
        /// <returns></returns>
        public DataTable GetOperationInfo(string s_date, string d_date)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"
  SELECT   decode(ACCOUNT_DEPT_NAME,null,'总计',ACCOUNT_DEPT_NAME) as  ACCOUNT_DEPT_NAME, decode(THENUMBERFO,null,'小计',THENUMBERFO) THENUMBERFO, sum(MFS) MFS
    FROM   (  SELECT   B.ACCOUNT_DEPT_name,
                       A.OPERATOR TheNumberFo,
                       SUM(                                     --临床主刀医生
                           CASE
                              WHEN A.LEVEL_J = '1'
                              THEN
                                 (CASE
                                     WHEN A.EMERGENCY = '1'
                                     THEN
                                        (CASE
                                            WHEN FIRST_ASSISTANT is NULL
                                                 AND SECOND_ASSISTANT is NULL
                                            THEN
                                               (50 + 20)
                                            ELSE
                                               (50 + 20) * 0.5
                                         END)
                                     ELSE
                                        (CASE
                                            WHEN FIRST_ASSISTANT is NULL
                                                 AND SECOND_ASSISTANT is NULL
                                            THEN
                                               50
                                            ELSE
                                               50 * 0.5
                                         END)
                                  END)
                              --二级
                           WHEN A.LEVEL_J = '2'
                              THEN
                                 (CASE
                                     WHEN A.EMERGENCY = '1'
                                     THEN
                                        (CASE
                                            WHEN FIRST_ASSISTANT is NULL
                                                 AND SECOND_ASSISTANT is NULL
                                            THEN
                                               (100 + 20)
                                            ELSE
                                               (100 + 20) * 0.5
                                         END)
                                     ELSE
                                        (CASE
                                            WHEN FIRST_ASSISTANT is NULL
                                                 AND SECOND_ASSISTANT is NULL
                                            THEN
                                               +100
                                            ELSE
                                               +100 * 0.5
                                         END)
                                  END)
                              --三级
                           WHEN A.LEVEL_J = '3'
                              THEN
                                 (CASE
                                     WHEN A.EMERGENCY = '1'
                                     THEN
                                        (CASE
                                            WHEN FIRST_ASSISTANT is NULL
                                                 AND SECOND_ASSISTANT is NULL
                                            THEN
                                               (200 + 20)
                                            ELSE
                                               (200 + 20) * 0.5
                                         END)
                                     ELSE
                                        (CASE
                                            WHEN FIRST_ASSISTANT is NULL
                                                 AND SECOND_ASSISTANT is NULL
                                            THEN
                                               200
                                            ELSE
                                               200 * 0.5
                                         END)
                                  END)
                              --四级
                           WHEN A.LEVEL_J = '4'
                              THEN
                                 (CASE
                                     WHEN A.EMERGENCY = '1'
                                     THEN
                                        (CASE
                                            WHEN FIRST_ASSISTANT is NULL
                                                 AND SECOND_ASSISTANT is NULL
                                            THEN
                                               (300 + 20)
                                            ELSE
                                               (300+ 20) * 0.5 
                                         END)
                                     ELSE
                                        (CASE
                                            WHEN FIRST_ASSISTANT is NULL
                                                 AND SECOND_ASSISTANT is NULL
                                            THEN
                                               +300
                                            ELSE
                                               +300 * 0.5
                                         END)
                                  END)
                           END)
                          mfs
                FROM   PERFORMANCE.OPERATIONINFO A, COMM.SYS_DEPT_dict B
               WHERE       TO_CHAR (a.ST_DATE, 'yyyymmdd') >= '{0}'
                       AND TO_CHAR (a.ST_DATE, 'yyyymmdd') <= '{1}'
                       AND a.DEPT_NAME = B.DEPT_CODE
            GROUP BY   B.ACCOUNT_DEPT_name, A.OPERATOR
            UNION ALL
              SELECT   B.ACCOUNT_DEPT_name,
                       A.FIRST_ASSISTANT TheNumberFo,
                       CASE
                          WHEN FIRST_ASSISTANT IS NOT NULL
                          THEN
                             SUM(                               --临床一助医生
                                 CASE
                                    WHEN A.LEVEL_J = '1'
                                    THEN
                                       (CASE
                                           WHEN A.EMERGENCY = '1'
                                           THEN
                                              (CASE
                                                  WHEN SECOND_ASSISTANT is NULL
                                                  THEN
                                                     (50 + 20) * 0.5
                                                  ELSE
                                                     (50 + 20) *0.5* (3/5)
                                               END)
                                           ELSE
                                              (CASE
                                                  WHEN SECOND_ASSISTANT is NULL
                                                  THEN
                                                     50 * 0.5
                                                  ELSE
                                                     50 * 0.5*(3/5)
                                               END)
                                        END)
                                    --二级
                                 WHEN A.LEVEL_J = '2'
                                    THEN
                                       (CASE
                                           WHEN A.EMERGENCY = '1'
                                           THEN
                                              (CASE
                                                  WHEN SECOND_ASSISTANT is NULL
                                                  THEN
                                                     (100 + 20) * 0.5
                                                  ELSE
                                                     (100 + 20) *0.5* (3/5)
                                               END)
                                           ELSE
                                              (CASE
                                                  WHEN SECOND_ASSISTANT is NULL
                                                  THEN
                                                     100 * 0.5
                                                  ELSE
                                                     100 * 0.5*(3/5)
                                               END)
                                        END)
                                    --三级
                                 WHEN A.LEVEL_J = '3'
                                    THEN
                                       (CASE
                                           WHEN A.EMERGENCY = '1'
                                           THEN
                                              (CASE
                                                  WHEN SECOND_ASSISTANT is NULL
                                                  THEN
                                                     (200 + 20) * 0.5
                                                  ELSE
                                                     (200 + 20) *0.5* (3/5)
                                               END)
                                           ELSE
                                              (CASE
                                                  WHEN SECOND_ASSISTANT is NULL
                                                  THEN
                                                     200 * 0.5
                                                  ELSE
                                                     200 *0.5* (3/5)
                                               END)
                                        END)
                                    --四级
                                 WHEN A.LEVEL_J = '4'
                                    THEN
                                       (CASE
                                           WHEN A.EMERGENCY = '1'
                                           THEN
                                              (CASE
                                                  WHEN SECOND_ASSISTANT is NULL
                                                  THEN
                                                     (300 + 20) * 0.5
                                                  ELSE
                                                     (300 + 20) *0.5* (3/5)
                                               END)
                                           ELSE
                                              (CASE
                                                  WHEN SECOND_ASSISTANT is NULL
                                                  THEN
                                                     300 * 0.5
                                                  ELSE
                                                     300 *0.5* (3/5)
                                               END)
                                        END)
                                 END)
                          ELSE
                             0
                       END
                          mfs
                FROM   PERFORMANCE.OPERATIONINFO A, COMM.SYS_DEPT_dict B
               WHERE       TO_CHAR (a.ST_DATE, 'yyyymmdd') >= '{0}'
                       AND TO_CHAR (a.ST_DATE, 'yyyymmdd') <= '{1}'
                       AND a.DEPT_NAME = B.DEPT_CODE
            GROUP BY   B.ACCOUNT_DEPT_name, A.FIRST_ASSISTANT
            UNION ALL
              SELECT   B.ACCOUNT_DEPT_name,
                       A.SECOND_ASSISTANT TheNumberFo,
                       CASE
                          WHEN SECOND_ASSISTANT IS NOT NULL
                          THEN
                             SUM(                               --临床二助医生
                                 CASE
                                    WHEN A.LEVEL_J = '1'
                                    THEN
                                       (CASE
                                           WHEN A.EMERGENCY = '1'
                                           THEN
                                              (50 + 20) *0.5* (2/5)
                                           ELSE
                                              50 *0.5* (2/5)
                                        END)
                                    --二级
                                 WHEN A.LEVEL_J = '2'
                                    THEN
                                       (CASE
                                           WHEN A.EMERGENCY = '1'
                                           THEN
                                              (100 + 20) *0.5* (2/5)
                                           ELSE
                                              100 *0.5* (2/5)
                                        END)
                                    --三级
                                 WHEN A.LEVEL_J = '3'
                                    THEN
                                       (CASE
                                           WHEN A.EMERGENCY = '1'
                                           THEN
                                              (200 + 20) *0.5* (2/5)
                                           ELSE
                                              200 *0.5* (2/5)
                                        END)
                                    --四级
                                 WHEN A.LEVEL_J = '4'
                                    THEN
                                       (CASE
                                           WHEN A.EMERGENCY = '1'
                                           THEN
                                              (300 + 20) *0.5* (2/5)
                                           ELSE
                                              300 *0.5* (2/5)
                                        END)
                                 END)
                          ELSE
                             0
                       END
                          mfs
                FROM   PERFORMANCE.OPERATIONINFO A, COMM.SYS_DEPT_dict B
               WHERE       TO_CHAR (a.ST_DATE, 'yyyymmdd') >= '{0}'
                       AND TO_CHAR (a.ST_DATE, 'yyyymmdd') <= '{1}'
                       AND a.DEPT_NAME = B.DEPT_CODE
            GROUP BY   B.ACCOUNT_DEPT_name, A.SECOND_ASSISTANT
            UNION ALL
            SELECT   'a麻醉师' ACCOUNT_DEPT_name,
           ANESTHESIA_DOCTOR TheNumberFo,
           SUM(                                                 --临床麻醉医生
               CASE
                  WHEN A.LEVEL_J = '1'
                  THEN
                     (CASE
                         WHEN A.EMERGENCY = '1'
                         THEN
                            CASE
                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL
                               THEN
                                  10 + 10
                               ELSE
                                  0
                            END
                         ELSE
                            CASE
                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL THEN 10
                               ELSE 0
                            END
                      END)
                  --二级
               WHEN A.LEVEL_J = '2'
                  THEN
                     (CASE
                         WHEN A.EMERGENCY = '1'
                         THEN
                            CASE
                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL
                               THEN
                                  20 + 10
                               ELSE
                                  0
                            END
                         ELSE
                            CASE
                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL THEN 20
                               ELSE 0
                            END
                      END)
                  --三级
               WHEN A.LEVEL_J = '3'
                  THEN
                     (CASE
                         WHEN A.EMERGENCY = '1'
                         THEN
                            CASE
                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL
                               THEN
                                  30 + 10
                               ELSE
                                  0
                            END
                         ELSE
                            CASE
                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL THEN 30
                               ELSE 0
                            END
                      END)
                  --四级
               WHEN A.LEVEL_J = '4'
                  THEN
                     (CASE
                         WHEN A.EMERGENCY = '1'
                         THEN
                            CASE
                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL
                               THEN
                                  50 + 10
                               ELSE
                                  0
                            END
                         ELSE
                            CASE
                               WHEN ANESTHESIA_DOCTOR_N IS NOT NULL THEN 50
                               ELSE 0
                            END
                      END)
               END)
              mfs
    FROM   PERFORMANCE.OPERATIONINFO A, COMM.SYS_DEPT_dict B
   WHERE       TO_CHAR (a.ST_DATE, 'yyyymmdd') >= '{0}'
           AND TO_CHAR (a.ST_DATE, 'yyyymmdd') <= '{1}'
           AND a.DEPT_NAME = B.DEPT_CODE
GROUP BY   ANESTHESIA_DOCTOR
            UNION ALL
SELECT  ACCOUNT_DEPT_NAME  ACCOUNT_DEPT_name,
           '护士' TheNumberFo,
           SUM(                                                 --临床护士医生
               CASE
                  WHEN A.LEVEL_J = '1'
                  THEN
                     (CASE
                         WHEN A.EMERGENCY = '1'
                         THEN
                            CASE
                               WHEN    HS1 IS NOT NULL
                                    OR HS2 IS NOT NULL
                                    OR HS3 IS NOT NULL
                                    OR HS4 IS NOT NULL
                                    OR HS5 IS NOT NULL
                               THEN
                                  5 + 5
                               ELSE
                                  0
                            END
                         ELSE
                            CASE
                               WHEN    HS1 IS NOT NULL
                                    OR HS2 IS NOT NULL
                                    OR HS3 IS NOT NULL
                                    OR HS4 IS NOT NULL
                                    OR HS5 IS NOT NULL
                               THEN
                                  5
                               ELSE
                                  0
                            END
                      END)
                  --二级
               WHEN A.LEVEL_J = '2'
                  THEN
                     (CASE
                         WHEN A.EMERGENCY = '1'
                         THEN
                            CASE
                               WHEN    HS1 IS NOT NULL
                                    OR HS2 IS NOT NULL
                                    OR HS3 IS NOT NULL
                                    OR HS4 IS NOT NULL
                                    OR HS5 IS NOT NULL
                               THEN
                                  10 + 5
                               ELSE
                                  0
                            END
                         ELSE
                            CASE
                               WHEN    HS1 IS NOT NULL
                                    OR HS2 IS NOT NULL
                                    OR HS3 IS NOT NULL
                                    OR HS4 IS NOT NULL
                                    OR HS5 IS NOT NULL
                               THEN
                                  10
                               ELSE
                                  0
                            END
                      END)
                  --三级
               WHEN A.LEVEL_J = '3'
                  THEN
                     (CASE
                         WHEN A.EMERGENCY = '1'
                         THEN
                            CASE
                               WHEN    HS1 IS NOT NULL
                                    OR HS2 IS NOT NULL
                                    OR HS3 IS NOT NULL
                                    OR HS4 IS NOT NULL
                                    OR HS5 IS NOT NULL
                               THEN
                                  15 + 5
                               ELSE
                                  0
                            END
                         ELSE
                            CASE
                               WHEN    HS1 IS NOT NULL
                                    OR HS2 IS NOT NULL
                                    OR HS3 IS NOT NULL
                                    OR HS4 IS NOT NULL
                                    OR HS5 IS NOT NULL
                               THEN
                                  15
                               ELSE
                                  0
                            END
                      END)
                  --四级
               WHEN A.LEVEL_J = '4'
                  THEN
                     (CASE
                         WHEN A.EMERGENCY = '1'
                         THEN
                            CASE
                               WHEN    HS1 IS NOT NULL
                                    OR HS2 IS NOT NULL
                                    OR HS3 IS NOT NULL
                                    OR HS4 IS NOT NULL
                                    OR HS5 IS NOT NULL
                               THEN
                                  20 + 5
                               ELSE
                                  0
                            END
                         ELSE
                            CASE
                               WHEN    HS1 IS NOT NULL
                                    OR HS2 IS NOT NULL
                                    OR HS3 IS NOT NULL
                                    OR HS4 IS NOT NULL
                                    OR HS5 IS NOT NULL
                               THEN
                                  20
                               ELSE
                                  0
                            END
                      END)
               END)
              mfs
    FROM   (SELECT   LEVEL_J,
                     EMERGENCY,
                     HS_NAME1,
                     B.USER_DEPT,
                     HS1,
                     HS2,
                     HS3,
                     HS4,
                     HS5
              FROM   PERFORMANCE.OPERATIONINFO a, hisdata.users b
             WHERE       a.HS_NAME1 = b.DB_USER
                     AND TO_CHAR (a.ST_DATE, 'yyyymmdd') >=  '{0}'
                     AND TO_CHAR (a.ST_DATE, 'yyyymmdd') <=  '{1}') A,
           COMM.SYS_DEPT_dict B
   WHERE   a.USER_DEPT = B.DEPT_CODE
GROUP BY   ACCOUNT_DEPT_NAME
)
   WHERE   THENUMBERFO IS NOT NULL
GROUP BY   ROLLUP (ACCOUNT_DEPT_NAME, THENUMBERFO)", s_date, d_date);

            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
        }



    }
}
