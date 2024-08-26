using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GoldNet.Comm;
using System.Data.OleDb;
using GoldNet.Comm.DAL.Oracle;

namespace Goldnet.Dal
{
    public class StaffDalDict
    {
        public StaffDalDict()
        { }



        /// <summary>
        /// 查询人员
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public DataSet GetStaff(string where)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"select staff_id as VALUE,case when
                                  (select count(NAME) from RLZY.NEW_STAFF_INFO t2 where T2.NAME = t1.name and t2.add_mark='1') = 2 
                                  THEN t1.name ||'('|| DEPT_NAME ||')'
                                  ELSE t1.name 
                                  END as TEXT from {0}.NEW_STAFF_INFO t1 where add_mark='1' ", DataUser.RLZY);

            if (where != "")
            {
                str.Append(" " + where);
            }
            str.Append(" order by name");

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 人员类别
        /// </summary>
        /// <returns></returns>
        public DataSet getPSort()
        {
            //staffsort
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select distinct case when a.staffsort is null then '为空' else trim(a.staffsort) end  ID,b.SERIAL_NO 
                            from {0}.NEW_STAFF_INFO a , {0}.PERS_SORT_DICT b 
                            where ADD_MARK = '1' and trim(A.staffsort) = B.PERS_SORT_NAME(+)   order by b.SERIAL_NO", DataUser.RLZY);

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }

        /// <summary>
        ///  技术级
        /// </summary>
        /// <returns></returns>
        public DataSet getTechnicclass()
        {
            //TECHINCCLASS
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select distinct case when a.TECHINCCLASS is null then '为空' else a.TECHINCCLASS end  techID,b.id 
                                from {0}.NEW_STAFF_INFO a,(select * from {0}.TECHNICCLASS_DICT where IS_DEL=0) b where A.TECHINCCLASS = B.TECHNICCLASS(+) order by b.ID", DataUser.RLZY);


            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 文职级
        /// </summary>
        /// <returns></returns>
        public DataSet getCivilserviceclass()
        {
            //CIVILSERVICECLASS
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select distinct case when a.CIVILSERVICECLASS is null then '为空' else a.CIVILSERVICECLASS end  civID ,b.ID
                                from {0}.NEW_STAFF_INFO a, (select * from {0}.CIVILSERVICECLASS_DICT where IS_DEL=0) b where a.CIVILSERVICECLASS = b.CIVILSERVICECLASS(+) order by b.ID", DataUser.RLZY);
            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }


        /// <summary>
        /// 学位
        /// </summary>
        /// <returns></returns>
        public DataSet getDegee()
        {
            //edu1
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select distinct case when a.edu1 is null then '为空' else a.edu1 end  eduID ,b.id

                            from {0}.NEW_STAFF_INFO a,(select * from {0}.DEGREE_DICT where IS_DEL=0) b where a.edu1 = b.DEGREE(+) order by b.ID", DataUser.RLZY);
            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }

        /// <summary>
        /// 职称序列
        /// </summary>
        /// <returns></returns>
        public DataSet getTitlelist()
        {
            //title_list
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select distinct case when title_list is null then '为空' else a.title_list end  ID ,b.SERIAL_NO 
                            from {0}.NEW_STAFF_INFO a,{0}.TITLE_LIST_DICT b where a.title_list = b.TITLE_LIST_NAME(+) order by b.SERIAL_NO", DataUser.RLZY);

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }


        /// <summary>
        /// 岗位
        /// </summary>
        /// <param name="DeptType">部门类别</param>
        /// <param name="deptattr">部门属性</param>
        /// <returns></returns>
        public DataSet getStations(string DeptType, string deptattr)
        {

            StringBuilder sql = new StringBuilder();

            sql.AppendFormat("select DISTINCT B.STATION_CODE as ID, a.station_name AS TEXT ,b.dept_code  ");

            sql.AppendFormat("from {0}.SYS_STATION_MAINTENANCE_DICT a,{1}.NEW_STAFF_INFO b  ", DataUser.COMM, DataUser.RLZY);

            sql.AppendFormat("where a.ID = b.STATION_CODE AND B.ADD_MARK = '1'   ");

            sql.AppendFormat("and b.dept_code in (select t1.dept_code from {0}.sys_dept_dict t1 where t1.dept_type='{1}' ", DataUser.COMM, DeptType);

            if (!deptattr.Equals(""))
            {
                sql.AppendFormat(" and t1.DEPT_LCATTR = '{0}'  ", deptattr);

            }
            sql.AppendFormat(" )  order by a.station_name");
            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        }


        /// <summary>
        /// 过滤字符
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string Stringfilter(string item)
        {
            if (item == "为空")
            {
                item = " is null ";
            }
            else
            {
                item = " = '" + item + "'";
            }
            return item;
        }


        /// <summary>
        /// 根据字符条件查询人员
        /// </summary>
        /// <param name="terms">字符条件</param>
        /// <returns></returns>
        public DataSet AnalyzeTermsToStaffId(string terms)
        {

            string[] StaffCodes = terms.Split(';');

            string[] ComboxInfos = StaffCodes[0].Split(',');

            string where = "select staff_id  from rlzy.NEW_STAFF_INFO where add_mark='1'";

            if (StaffCodes[StaffCodes.Length - 1].Equals("1"))
            {
                if (ComboxInfos[0].ToString() != "*")
                {
                    where = where + "and trim(staffsort) " + this.Stringfilter(ComboxInfos[0].ToString().Trim());
                }

                if (ComboxInfos[1].ToString() != "*")
                {
                    where = where + "  and techincclass " + this.Stringfilter(ComboxInfos[1].ToString());
                }

                if (ComboxInfos[2].ToString() != "*")
                {
                    where = where + "  and title_list " + this.Stringfilter(ComboxInfos[2].ToString());
                }

                if (ComboxInfos[3].ToString() != "*")
                {
                    where = where + " and TechnicClassDate is not null and ";

                    where = where + " substr(nvl(TechnicClassDate,'') ,1,4)|| lpad(replace(replace(replace(substr(nvl(TechnicClassDate,'') ,6,2),'.',''),'-',''),'/',''),2,'0')||'01' ";

                    where = where + ComboxInfos[3].Split('$')[0].ToString() + ComboxInfos[3].Split('$')[1] + " ";
                }

                if (ComboxInfos[4].ToString() != "*")
                {
                    where = where + "  and edu1 " + this.Stringfilter(ComboxInfos[4].ToString());
                }
                if (ComboxInfos[5].ToString() != "*")
                {
                    where = where + "  and CivilServiceClass " + this.Stringfilter(ComboxInfos[5].ToString());
                }
                if (ComboxInfos[6].ToString() != "*")
                {
                    where = where + "  and dept_code ='" + ComboxInfos[6].ToString() + "'";
                }

                if (StaffCodes[StaffCodes.Length - 2] != "*")
                {
                    where = where + " and staff_id not in (" + StaffCodes[StaffCodes.Length - 2] + ")";
                }
            }
            else
            {
                where = where + " and staff_id in (" + StaffCodes[StaffCodes.Length - 2] + ")";
            }

            return OracleOledbBase.ExecuteDataSet(where.ToString());


        }


        public DataSet getStationStore(string DeptCode)
        {
            StringBuilder sql = new StringBuilder();
            if (DeptCode == "")
            {
                sql.AppendFormat(@"SELECT ID VALUE,STATION_NAME TEXT                     
                                FROM {0}.SYS_STATION_MAINTENANCE_DICT 
                                WHERE ID IN(SELECT STATION_CODE FROM {1}.NEW_STAFF_INFO WHERE STATION_CODE IS NOT NULL)", DataUser.COMM, DataUser.RLZY);
            }
            else
            {
                sql.AppendFormat(@" SELECT DISTINCT  A.ID VALUE,A.STATION_NAME TEXT
                                   FROM {0}.SYS_STATION_MAINTENANCE_DICT A,
                                        (SELECT STATION_DICT_CODE
                                           FROM {0}.SYS_STATION_BASIC_INFORMATION
                                          WHERE STATION_YEAR = '{1}' AND DEPT_CODE = '{2}' ) B
                                  WHERE A.ID = B.STATION_DICT_CODE ", DataUser.COMM, DateTime.Now.Year.ToString(), DeptCode);
            }
            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        }
    }
}
