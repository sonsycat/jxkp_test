using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using System.Collections;
using GoldNet.Comm;

namespace Goldnet.Dal
{
    public class DeptDalDict
    {
        public DeptDalDict()
		{ }

        /// <summary>
        /// 已选指标SQL
        /// </summary>
        /// <returns></returns>
        public DataSet getReportExDept(string dept_code)
        {

            StringBuilder sql = new StringBuilder();

            if (dept_code != "*")
            {
                sql.AppendFormat(@"SELECT DEPT_CODE VALUE,DEPT_NAME TEXT  FROM  COMM.SYS_DEPT_DICT WHERE DEPT_CODE IN ({0})", dept_code);
            }
            else
            {
                sql.AppendFormat(@"SELECT DEPT_CODE VALUE,DEPT_NAME TEXT  FROM  COMM.SYS_DEPT_DICT WHERE 1=2 ");
            }

            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        }

        /// <summary>
        /// 条件字符串查询
        /// </summary>
        /// <param name="terms">条件字符串</param>
        /// <returns></returns>
        public DataSet AnalyzeStringLeftSelector(string terms)
        {
            string[] AnalyzeString = terms.Split(';');

            StringBuilder str = new StringBuilder();

            str.Append("SELECT ");

            str.Append(" DEPT_CODE VALUE,DEPT_NAME TEXT from comm.SYS_DEPT_DICT where attr='是' ");

            if (AnalyzeString[0] != "*" || AnalyzeString[1] != "*")
            {
                str.Append(" and (");

            }

            if (AnalyzeString[0] != "*")
            {

                str.Append("( ");

                str.Append(" dept_type = 0");

                str.Append(" and DEPT_LCATTR in  ");

                str.Append("( ");

                string[] lcdcode = AnalyzeString[0].Split(',');

                for (int x = 0; x < lcdcode.Length; x++)
                {

                    if (x == lcdcode.Length - 1)
                    {
                        str.Append(lcdcode[x]);

                    }
                    else
                    {
                        str.Append(lcdcode[x]);
                        str.Append(",");
                    }
                }
                str.Append(" ) ) ");
            }

            if (AnalyzeString[0] != "*" && AnalyzeString[1] != "*")
            {
                str.Append(" or  ");

            }

            if (AnalyzeString[1] != "*")
            {
                str.Append(" dept_type in ( ");

                string[] deptType = AnalyzeString[1].Split(',');

                for (int y = 0; y < deptType.Length; y++)
                {
                    if (y == deptType.Length - 1)
                    {
                        str.Append(deptType[y]);

                    }
                    else
                    {
                        str.Append(deptType[y]);
                        str.Append(",");
                    }
                }
                str.Append("  )");
            }

            if (AnalyzeString[0] != "*" || AnalyzeString[1] != "*")
            {
                str.Append(" ) ");
            }

            if (AnalyzeString[2] != "*")
            {

                str.Append(" and DEPT_CODE not in ( ");

                string[] deptCodeNotIn = AnalyzeString[2].Split(',');


                for (int z = 0; z < deptCodeNotIn.Length; z++)
                {
                    if (z == deptCodeNotIn.Length - 1)
                    {
                        if (z == 0)
                        {
                            str.Append("'");
                        }
                        str.Append(deptCodeNotIn[z]);
                        str.Append("'");

                    }
                    else
                    {
                        if(z==0) 
                        {
                            str.Append("'");
                        }
                        str.Append(deptCodeNotIn[z]);
                        str.Append("','");
                    }
                }

                str.Append("  )");
            }
            if (AnalyzeString[0] == "*" && AnalyzeString[1] == "*" && AnalyzeString[2] == "*") 
            {
                str.Append(" and 1=2 ");
            }

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }

        /// <summary>
        /// 按2级科室查询
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        public DataSet StringLeftSelector(string terms) 
        {
            string[] temp = terms.Split(';');
            string DeptSecond = temp[0].Replace(",", "','").Insert(0, "'") + "'";
            string DeptCode = temp[1] == "*" ? "" : "AND DEPT_CODE NOT IN (" + temp[1].Replace(",", "','").Insert(0, "'") + "'" + ")";

            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@" SELECT TO_CHAR (DEPT_CODE) VALUE, DEPT_NAME TEXT
                                  FROM {0}.SYS_DEPT_DICT
                                 WHERE DEPT_CODE_SECOND IN ({1}) {2} AND SHOW_FLAG = '0'", DataUser.COMM,DeptSecond,DeptCode);
            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        }

        /// <summary>
        /// 条件集合查询
        /// </summary>
        /// <param name="list">条件集合</param>
        /// <returns></returns>
        public DataSet AnalyzeStringLeftSelector(ArrayList list)
        {
            //临床的情况下记录临床内科室的编号
            ArrayList SubidArr = new ArrayList();

            //临床的情况下记录临床的编号
            string LcdId = "";

            //记录除临床以外的编号
            ArrayList ArrtId = new ArrayList();

            StringBuilder str = new StringBuilder();

            str.Append("SELECT DEPT_CODE VALUE,DEPT_NAME TEXT from comm.SYS_DEPT_DICT where attr='是' ");

            if (list.Count == 0) 
            {
                str.Append(" and 1=2");
            }
            for (int i = 0; i < list.Count; i++)
            {
                string[] code = list[i].ToString().Split(',');

                string pid = code[1].ToString();

                string id = code[0].ToString();

                if (id.Substring(0, 1) == "l")
                {
                    SubidArr.Add(id.Substring(1));
                    LcdId = pid;
                }
                else
                {
                    ArrtId.Add(id);
                }
            }


            if (SubidArr.Count > 0 || ArrtId.Count > 0)
            {
                str.Append(" and (");
            }

            if (SubidArr.Count > 0)
            {
                str.Append("( ");

                str.Append(" dept_type = ");

                str.Append(LcdId);

                str.Append(" and DEPT_LCATTR in  ");
                str.Append("( ");

                for (int j = 0; j < SubidArr.Count; j++)
                {
                    if (j == SubidArr.Count - 1)
                    {
                        str.Append(SubidArr[j].ToString());
                    }
                    else
                    {
                        str.Append(SubidArr[j].ToString());
                        str.Append(",");
                    }
                }

                str.Append("  ) ) ");
            }


            if (SubidArr.Count > 0 && ArrtId.Count > 0)
            {

                str.Append("  or  ");

            }


            if (ArrtId.Count > 0)
            {

                str.Append(" dept_type in ( ");

                for (int x = 0; x < ArrtId.Count; x++)
                {
                    if (x == ArrtId.Count - 1)
                    {
                        str.Append(ArrtId[x].ToString());
                    }
                    else
                    {
                        str.Append(ArrtId[x].ToString());
                        str.Append(",");
                    }
                }

                str.Append("  )");

            }

            if (SubidArr.Count > 0 || ArrtId.Count > 0)
            {

                str.Append(" ) ");

            }

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 按2级科室查询科室
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        public DataSet AnalyzeTermsToDeptBySecond(string terms) 
        {
            
            StringBuilder sql = new StringBuilder();
            string deptSecond = terms == "" ? "1=2" : "DEPT_CODE_SECOND IN (" + terms.Replace(';', ',').TrimEnd(new char[] { ',' }).Replace(",", "','").Insert(0, "'") + "'" + ")";
            sql.AppendFormat(@"SELECT DEPT_CODE VALUE,DEPT_NAME TEXT 
                                FROM {0}.SYS_DEPT_DICT 
                                WHERE {1}", DataUser.COMM, deptSecond);
            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        }

        /// <summary>
        /// 分析条件字符查询部门代码
        /// </summary>
        /// <param name="terms">条件字符串</param>
        /// <returns></returns>
        public DataSet AnalyzeTermsToDeptCode(string terms)
        {
            string[] AnalyzeString = terms.Split(';');

            StringBuilder str = new StringBuilder();

            str.Append("SELECT ");

            str.Append(" DEPT_CODE from comm.SYS_DEPT_DICT where attr='是' ");

            if (AnalyzeString[AnalyzeString.Length - 1] == "0")
            {
                str.Append(" and DEPT_CODE in ( ");

                str.Append(AnalyzeString[AnalyzeString.Length - 2].ToString().TrimEnd(new char[] { ',' }).Replace(",", "','").Insert(0, "'") + "'");

                str.Append(")");

            }
            else
            {
                if (AnalyzeString[0] != "*" || AnalyzeString[1] != "*")
                {
                    str.Append(" and (");

                }

                if (AnalyzeString[0] != "*")
                {

                    str.Append("( ");

                    str.Append(" dept_type = 0");

                    str.Append(" and DEPT_LCATTR in  ");

                    str.Append("( ");

                    string[] lcdcode = AnalyzeString[0].Split(',');

                    for (int x = 0; x < lcdcode.Length; x++)
                    {

                        if (x == lcdcode.Length - 1)
                        {
                            str.Append(lcdcode[x]);

                        }
                        else
                        {
                            str.Append(lcdcode[x]);
                            str.Append(",");
                        }
                    }
                    str.Append(" ) ) ");
                }

                if (AnalyzeString[0] != "*" && AnalyzeString[1] != "*")
                {
                    str.Append(" or  ");

                }

                if (AnalyzeString[1] != "*")
                {

                    str.Append(" dept_type in ( ");

                    string[] deptType = AnalyzeString[1].Split(',');


                    for (int y = 0; y < deptType.Length; y++)
                    {
                        if (y == deptType.Length - 1)
                        {
                            str.Append(deptType[y]);

                        }
                        else
                        {
                            str.Append(deptType[y]);
                            str.Append(",");
                        }
                    }

                    str.Append("  )");
                }

                if (AnalyzeString[0] != "*" || AnalyzeString[1] != "*")
                {
                    str.Append(" ) ");

                }

                if (AnalyzeString[2] != "*")
                {

                    str.Append(" and DEPT_CODE not in ( ");

                    string[] deptCodeNotIn = AnalyzeString[2].Split(',');


                    for (int z = 0; z < deptCodeNotIn.Length; z++)
                    {
                        if (z == deptCodeNotIn.Length - 1)
                        {
                            str.Append(deptCodeNotIn[z]);
                            str.Append("'");
                        }
                        else
                        {
                            if (z == 0)
                            {
                                str.Append("'");
                            }
                            str.Append(deptCodeNotIn[z]);
                            str.Append("','");
                        }
                    }

                    str.Append("  )");
                }


            }

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }

        /// <summary>
        /// 组织关系SQL
        /// </summary>
        /// <returns></returns>
        public DataSet OrginStore()
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("select organ_class_code as ID,organ_class_name as TEXT from hospitalsys.JXGL_GUIDE_ORGAN_CLASS_DICT");

            return OracleOledbBase.ExecuteDataSet(sql.ToString());

        }

        /// <summary>
        /// 科室关系SQL
        /// </summary>
        /// <returns></returns>
        public DataSet DeptStore()
        {
            return OracleOledbBase.ExecuteDataSet(string.Format("SELECT '00' AS ID, '公共部分' AS TEXT FROM DUAL  UNION SELECT DEPT_CLASS_CODE ID,DEPT_CLASS_NAME TEXT FROM {0}.JXGL_GUIDE_DEPT_CLASS_DICT", DataUser.HOSPITALSYS));
        }

        /// <summary>
        /// 部门SQL
        /// </summary>
        /// <returns></returns>
        public DataSet DeptAttrStore()
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("select ID,ATTRIBUE as TEXT from COMM.SYS_LCDEPT_ATTR_DICT");

            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        }

        /// <summary>
        /// 根据部门类别查询部门代码
        /// </summary>
        /// <param name="DeptType">部门类别</param>
        /// <param name="DeptAttr">部门属性</param>
        /// <returns></returns>
        public DataSet getDeptByDeptType(string DeptType,string DeptAttr) 
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat("SELECT DEPT_CODE ID ,DEPT_NAME TEXT FROM {0}.SYS_DEPT_DICT WHERE ATTR = '是' AND DEPT_TYPE={1} ", DataUser.COMM, DeptType);
            if (DeptAttr != "") 
            {
                sql.AppendFormat(" AND DEPT_LCATTR={0}", DeptAttr);
            }
            sql.AppendFormat(" ORDER BY SORT_NO");

            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        
        }

        /// <summary>
        /// 根据科室类别及临床类别得到科室列表
        /// </summary>
        /// <param name="classlist"></param>
        /// <returns></returns>
        public DataSet GetDeptLeftSelector(string classlist)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                    SELECT DEPT_CODE VALUE,DEPT_NAME TEXT FROM {0}.SYS_DEPT_DICT WHERE ATTR='是' 
                     AND ( ( DEPT_LCATTR IS NULL AND DEPT_TYPE IN ({1}) )  OR ( 'l'||DEPT_LCATTR  IN ({1}) ) )", DataUser.COMM, classlist);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
    }
}
