using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.Comm;
namespace Goldnet.Dal
{
    /// <summary>
    /// SYS_DEPT_DICT。
    /// </summary>
    public class StationManager
    {
        public StationManager()
        { }


        #region  成员方法


        /// <summary>
        /// 科室树结构
        /// </summary>
        /// <returns></returns>
        public DataSet GetDeptTree()
        {
            string str = string.Format(@" SELECT ID,NAME,ATTR,PID,ISLEAF FROM {0}.SYS_DEPT_TREEVIEW ", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str);
        }


        /// <summary>
        /// 根据科室编码和岗位年度，获取岗位名称、岗位说明、所属部门、录入员、录入时间，用于显示
        /// </summary>
        /// <param name="dept_code">科室编码</param>
        /// <param name="station_year">岗位年度</param>
        /// <returns></returns>
        public DataSet GetStationListByDeptCode(string dept_code, string station_year)
        {
            StringBuilder strb = new StringBuilder();
            if (station_year.Equals(""))
            {
                station_year = DateTime.Now.Year.ToString();
            }

            strb.Append(" SELECT STATION.STATION_NAME, STATION.STATION_CODE_REMARK, STATION.DEPT_CODE,   ");
            strb.Append("        STATION.INPUT_USER, TO_CHAR(STATION.INPUT_TIME,'yyyy-MM-dd') INPUT_TIME,  ");
            strb.Append("        STATION.STATION_YEAR, STATION.STATION_CODE,STATION.GUIDE_GATHER_CODE, DEPT.DEPT_NAME   ");
            strb.AppendFormat("   FROM {0}.SYS_STATION_BASIC_INFORMATION STATION  ", DataUser.COMM);
            strb.AppendFormat("   LEFT JOIN {0}.SYS_DEPT_DICT DEPT   ", DataUser.COMM);
            strb.Append("     ON STATION.DEPT_CODE = DEPT.DEPT_CODE ");
            strb.Append("  WHERE STATION.STATION_YEAR='" + station_year + "' ");
            if (!dept_code.Equals(""))
            {
                strb.Append("    AND  STATION.DEPT_CODE = '" + dept_code + "'  ");
            }
            strb.Append("  ORDER BY STATION_YEAR,STATION.DEPT_CODE ,SEQUENCE ,STATION_CODE   ");

            return OracleOledbBase.ExecuteDataSet(strb.ToString());
        }
        /// <summary>
        /// 技术档案
        /// </summary>
        /// <param name="staffid"></param>
        /// <returns></returns>
        public DataTable GetStaffDocument(string staffid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select staff_id,attr,value from {0}.staff_document where staff_id='{1}'", DataUser.RLZY, staffid);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        /// <summary>
        /// 根据年度岗位代码，返回该岗位所在科室和下属科室
        /// </summary>
        /// <param name="stationCode">岗位代码</param>
        /// <param name="stationYear">岗位年度</param>
        /// <returns></returns>
        public DataSet GetStationDeptList(string stationCode, string stationYear)
        {
            if (stationYear.Equals(""))
            {
                stationYear = DateTime.Now.Year.ToString();
            }
            string strSql = string.Format(@"
             SELECT DEPT_CODE,DEPT_NAME
               FROM {0}.SYS_DEPT_DICT
              START WITH DEPT_CODE = (SELECT MAX(DEPT_CODE) FROM {0}.SYS_STATION_BASIC_INFORMATION WHERE STATION_CODE='{1}' AND STATION_YEAR='{2}')
            CONNECT BY PRIOR DEPT_CODE = P_DEPT_CODE
            UNION ALL
             SELECT DEPT_CODE,DEPT_NAME
               FROM {0}.SYS_DEPT_DICT
             WHERE DEPT_CODE = (SELECT MAX(P_DEPT_CODE) FROM {0}.SYS_DEPT_DICT WHERE DEPT_CODE = (SELECT MAX(DEPT_CODE) FROM {0}.SYS_STATION_BASIC_INFORMATION WHERE STATION_CODE='{1}' AND STATION_YEAR='{2}'))
                ", DataUser.COMM, stationCode, stationYear);
            return OracleOledbBase.ExecuteDataSet(strSql);
        }
        public DataSet GetGuideGather()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select GUIDE_GATHER_CODE GATHER_CODE,GUIDE_GATHER_NAME GATHER_NAME from hospitalsys.GUIDE_GATHER_CLASS");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        public void UpdateDeptGather(string deptcode, string gathercode, string gather01, string gather02, string gather03, string gather04, string years)
        {
            MyLists listttrans = new MyLists();
            List listindex = new List();
            listindex.StrSql = string.Format(@" DELETE FROM {0}.DEPT_GATHERS  WHERE DEPT_CODE ='{1}' AND STATION_YEAR='{2}'", DataUser.HOSPITALSYS, deptcode, years);
            OleDbParameter[] parameters = {
                    new OleDbParameter("", "")
		    };

            listindex.Parameters = parameters;
            listttrans.Add(listindex);

            StringBuilder strSql = new StringBuilder();

            strSql.AppendFormat("insert into {0}.DEPT_GATHERS(", DataUser.HOSPITALSYS);
            strSql.Append("DEPT_CODE,GATHER_CODE,STATION_BSC_CLASS_01,STATION_BSC_CLASS_02,STATION_BSC_CLASS_03,STATION_BSC_CLASS_04,STATION_YEAR,INPUT_TIME)");
            strSql.Append(" values (");
            strSql.Append("?,?,?,?,?,?,?,sysdate)");
            OleDbParameter[] parameters1 = {
											  new OleDbParameter("DEPT_CODE",deptcode),
											  new OleDbParameter("GATHER_CODE", gathercode),
											  new OleDbParameter("STATION_BSC_CLASS_01", gather01),
											  new OleDbParameter("STATION_BSC_CLASS_02", gather02),
                                               new OleDbParameter("STATION_BSC_CLASS_03", gather03),
                                                new OleDbParameter("STATION_BSC_CLASS_04", gather04),
                                                new OleDbParameter("STATION_YEAR", years)
										  };
            List listadd = new List();
            listadd.StrSql = strSql.ToString();
            listadd.Parameters = parameters1;
            listttrans.Add(listadd);
            OracleOledbBase.ExecuteTranslist(listttrans);

        }
        /// <summary>
        /// 提取科室指标集设置信息
        /// </summary>
        /// <param name="years"></param>
        /// <returns></returns>
        public DataSet GetDeptGuide(string years, string deptcode, string depttype)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   a.dept_code,
         a.dept_name,
         B.GATHER_CODE,
         c.GUIDE_GATHER_NAME GATHER_NAME,
         B.STATION_BSC_CLASS_01,
         B.STATION_BSC_CLASS_02,
         B.STATION_BSC_CLASS_03,
         B.STATION_BSC_CLASS_04
  FROM   comm.sys_dept_dict a,
         hospitalsys.DEPT_GATHERS b,
         hospitalsys.GUIDE_GATHER_CLASS c
 WHERE       a.attr = '是'
         AND A.DEPT_CODE = B.DEPT_CODE(+)
         AND B.STATION_YEAR(+) = '{0}'
         AND B.GATHER_CODE = C.GUIDE_GATHER_CODE(+)", years);
            if (deptcode != "")
            {
                str.AppendFormat(" and a.dept_code='{0}'", deptcode);
            }
            if (depttype != "")
            {
                str.AppendFormat(" and a.DEPT_TYPE='{0}'", depttype);
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 解除科室与指标集的关联
        /// </summary>
        /// <param name="years"></param>
        /// <param name="deptcode"></param>
        /// <param name="depttype"></param>
        /// <returns></returns>
        public void DelDeptGathers(string years, string deptcode, string depttype)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"delete HOSPITALSYS.DEPT_GATHERS where dept_code='{1}' and gather_code='{2}' and station_year='{0}'", years,deptcode,depttype);
            OleDbParameter[] parms = new OleDbParameter[]{
                };
            OracleOledbBase.ExecuteNonQuery(str.ToString(), parms); ;
        }



        /// <summary>
        /// 获得岗位信息
        /// </summary>
        /// <param name="station_code">岗位编码</param>
        /// <param name="station_year">岗位年度</param>
        /// <returns></returns>
        public DataSet GetStationInfo(string station_code, string station_year)
        {
            string sql = string.Format(@"SELECT STATION.DEPT_CODE, STATION.DUTY_ORDER, STATION.WHETHER,
                           STATION.STIPEND, STATION.GUIDE_GATHER_CODE,
                           STATION.STATION_BSC_CLASS_01, STATION.STATION_BSC_CLASS_02,
                           STATION.STATION_BSC_CLASS_03, STATION.STATION_BSC_CLASS_04,
                           STATION.AUDITING_USER,
                           TO_CHAR (STATION.INPUT_TIME, 'YYYY-MM-DD') INPUT_TIME,
                           STATION.INPUT_USER, STATION.STATION_CODE_REMARK, STATION.WORK_CONTENT,
                           STATION.POST_COMPETENCY, STATION.WORK_CIRCUMSTANCE,
                           STATION.STATION_NAME, 
                           STATION.STATION_CODE_SHOW,
                           STATION.GUIDE_TYPE                           
                           FROM {3}.SYS_STATION_BASIC_INFORMATION STATION  WHERE STATION_CODE = '{0}' AND STATION_YEAR = '{1}'", station_code, station_year, DataUser.HOSPITALSYS, DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(sql);

        }



        /// <summary>
        /// 更新年度岗位信息
        /// </summary>
        public void UpdateSatationInfo(Dictionary<string, string> FormValues)
        {
            //FormValues结构如下(根据页面各控件的定义名称及类别)：
            //{[StationCodeTxt, 3200004]}
            //{[StationYearTxt, 2010]}
            //{[StationNameTxt, 护师]}
            //{[RadioGroup1_Group, Radio1]}
            //{[DeptCombo_Value, 32]}
            //{[DeptCombo, 内一科]}
            //{[DeptCombo_SelIndex, 0]}
            //{[DeptDutyCombo_Value, 4]}
            //{[DeptDutyCombo, 人]}
            //{[DeptDutyCombo_SelIndex, 2]}
            //{[GatherTypeCombo_Value, 0101]}
            //{[GatherTypeCombo, 机关科室]}
            //{[GatherTypeCombo_SelIndex, -1]}
            //{[GuideGatherCombo_Value, 12]}
            //{[GuideGatherCombo, 护师岗位]}
            //{[GuideGatherCombo_SelIndex, -1]}
            //{[ScoreNum1, 550]}
            //{[ScoreNum2, 100]}
            //{[ScoreNum3, 100]}
            //{[ScoreNum4, 250]}
            //{[SalaryTxt, ]}
            //{[ApplyTxt, ]}
            //{[CreateTxt, ]}
            //{[CreateDate, 2010-04-23]}
            //{[StationTxt, ]}
            //{[WorkTxt, ]}
            //{[GuideGather_Original, 12]}
            //{[TitleTxt, ]}
            //{[JobTxt, ]}

            StringBuilder strb = new StringBuilder();
            strb.AppendFormat("UPDATE {0}.SYS_STATION_BASIC_INFORMATION ", DataUser.COMM);
            strb.Append("SET station_name = ?,");
            strb.Append("dept_code = ?,station_code_remark = ?,");
            strb.Append("cycle_station = ?,");
            strb.Append("stipend = ?,work_content = ?,");
            strb.Append("post_competency = ?,work_circumstance = ?,");
            strb.Append("input_user = ?,input_time = to_date(?,'yyyy-MM-dd'),");
            strb.Append("auditing_user = ?,duty_order = ?,");
            strb.Append("guide_gather_code = ?,");
            strb.Append("station_bsc_class_01 = ?,");
            strb.Append("station_bsc_class_02 = ?,");
            strb.Append("station_bsc_class_03 = ?,");
            strb.Append("station_bsc_class_04 = ?,");
            strb.Append("station_type = ?,");
            strb.Append("STATION_CODE_SHOW = ?,Whether = ?,");
            strb.Append("guide_type=?");
            strb.Append(" WHERE station_code = ? AND station_year = ?");

            OleDbParameter[] parameters = {
                new OleDbParameter("station_name",FormValues["StationNameTxt"]),
                new OleDbParameter("dept_code",FormValues["DeptCombo_Value"]),
                new  OleDbParameter("station_code_remark",FormValues["StationTxt"]),                
                new OleDbParameter("cycle_station",""),
                new OleDbParameter("stipend",FormValues["SalaryTxt"]),
                new OleDbParameter("work_content",FormValues["WorkTxt"]),
                new OleDbParameter("post_competency",FormValues["TitleTxt"]),
                new OleDbParameter("work_circumstance",FormValues["JobTxt"]),
                new OleDbParameter("input_user",FormValues["CreateTxt"]),
                new OleDbParameter("input_time", FormValues["CreateDate"]),
                new OleDbParameter("auditing_user",FormValues["ApplyTxt"]),
                new OleDbParameter("duty_order", FormValues["DeptDutyCombo_Value"]),
                new OleDbParameter("guide_gather_code",FormValues["GuideGatherCombo_Value"]),
                new OleDbParameter("station_bsc_class_01",FormValues["ScoreNum1"]),
                new OleDbParameter("station_bsc_class_02",FormValues["ScoreNum2"]),
                new OleDbParameter("station_bsc_class_03",FormValues["ScoreNum3"]),
                new OleDbParameter("station_bsc_class_04",FormValues["ScoreNum4"]),
                new OleDbParameter("station_type",FormValues["DeptDutyCombo_Value"]),
                new OleDbParameter("STATION_CODE_SHOW",FormValues["StationCodeTxt"]),
                new OleDbParameter("Whether",(FormValues["RadioGroup1_Group"].Equals("Radio1")?"是":"否")),
                new OleDbParameter("guide_type",FormValues["GatherTypeCombo_Value"]),
                new OleDbParameter("station_code",FormValues["StationCodeTxt"]),
                new OleDbParameter("station_year",FormValues["StationYearTxt"])
            };
            OracleOledbBase.ExecuteNonQuery(strb.ToString(), parameters);

            if (FormValues["GuideGather_Original"] != "" && FormValues["GuideGather_Original"] != FormValues["GuideGatherCombo_Value"])
            {
                string sql = string.Format(@"DELETE {0}.STATION_GUIDE_INFORMATION WHERE STATION_CODE=? AND STATION_YEAR=?", DataUser.HOSPITALSYS);
                OleDbParameter[] parms = new OleDbParameter[]{
                    new OleDbParameter("",FormValues["StationCodeTxt"]),
                    new OleDbParameter("",FormValues["StationYearTxt"])
                };
                OracleOledbBase.ExecuteNonQuery(sql, parms);
            }
        }


        /// <summary>
        /// 绑定通用指标集
        /// </summary>
        /// <returns></returns>
        public DataSet GetGuideGather(string guide_type)
        {
            string sql = string.Format(@"SELECT GUIDE_GATHER_CODE,GUIDE_GATHER_NAME 
                                          FROM {0}.GUIDE_GATHER_CLASS , {0}.GUIDE_GROUP_TYPE
                                         WHERE GUIDE_ATTR = GUIDE_GROUP_TYPE(+)
                                           AND (   ( ('{1}' =  '0000') AND (ALLUSE = '1'))
                                                OR ( ('{1}' <> '0000') AND (GUIDE_ATTR LIKE '{1}%'  OR  ALLUSE = '1'))
                                               ) ORDER BY GUIDE_GATHER_CODE ", DataUser.HOSPITALSYS, guide_type);
            return OracleOledbBase.ExecuteDataSet(sql);
        }


        /// <summary>
        /// 岗位组织类别信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetDUTY_ORDER()
        {
            string sql = string.Format("select STATION_TYPE_CODE,STATION_TYPE_NAME from {0}.SYS_STATION_TYPE_DICT order by STATION_TYPE_CODE", DataUser.COMM);

            OleDbParameter[] prams = new OleDbParameter[] { };

            return OracleOledbBase.ExecuteDataSet(sql);
        }
        /// <summary>
        /// 绑定指标类别
        /// </summary>
        /// <returns></returns>
        public DataSet GetGuide_type()
        {
            string sql = string.Format(@"SELECT GUIDE_GROUP_TYPE,GUIDE_GROUP_TYPE_NAME, SORTNO 
                                          FROM {0}.GUIDE_GROUP_TYPE  WHERE GUIDE_GROUP_TYPE LIKE '01__' 
                              UNION ALL SELECT '0000','通用指标集' ,999 FROM DUAL ORDER BY SORTNO,GUIDE_GROUP_TYPE", DataUser.HOSPITALSYS);

            return OracleOledbBase.ExecuteDataSet(sql);
        }




        /// <summary>
        /// 初始量化岗位指标
        /// 1.保存岗位指标集的指标量化分值
        /// 2.页面中各个gridpanel的数据，存放于guidearr中
        /// 3.组成insert select union all 语句
        /// 4.直接插入至STATION_GUIDE_INFORMATION表中。 ((Dictionary<string,string>)SelectRows[1])["BSC"]
        /// </summary>
        /// <param name="stationcode"></param>
        /// <param name="stationyear"></param>
        /// <param name="guidegathercode"></param>
        /// <param name="guidearr"></param>
        public void SaveStationGuideTarget(string stationcode, string stationyear, string guidegathercode, ArrayList guidearr)
        {
            MyLists listtable = new MyLists();
            string sqlstr = string.Format(@"
                 DELETE FROM {0}.STATION_GUIDE_INFORMATION  WHERE STATION_CODE = '{1}' AND STATION_YEAR = {2}
                        AND GUIDE_CODE IN (SELECT GUIDE_CODE FROM HOSPITALSYS.GUIDE_GATHERS A WHERE A.GUIDE_GATHER_CODE  = '{3}')
                ", DataUser.HOSPITALSYS, stationcode, stationyear, guidegathercode);
            //OracleOledbBase.ExecuteNonQuery(sqlstr);

            List listdel = new List();
            listdel.StrSql = sqlstr;
            listdel.Parameters = new OleDbParameter[] { };
            listtable.Add(listdel);

            for (int i = 0; i < guidearr.Count; i++)
            {
                StringBuilder stradd = new StringBuilder();
                stradd.AppendFormat(@"
               INSERT INTO {0}.STATION_GUIDE_INFORMATION 
               (
                 STATION_CODE,GUIDE_CODE,GUIDE_VALUE,GUIDE_CAUSE,GUIDE_UNIT,INCREASE, INCREASE_ARITHMETIC,DECREASE,DECREASE_ARITHMETIC,STATION_YEAR,MINUSFLAG,PLUSFLAG,PLUS_INCREASE,PLUS_ARITHMETIC,MINUS_INCREASE,MINUS_ARITHMETIC,THRESHOLD_VALUE
               ) values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?) ", DataUser.HOSPITALSYS);
                OleDbParameter[] parameteradd = {   new OleDbParameter("STATION_CODE",stationcode),
                                             new OleDbParameter("GUIDE_CODE",((Dictionary<string, string>)guidearr[i])["GUIDE_CODE"]),
                                             new OleDbParameter("GUIDE_VALUE",((Dictionary<string, string>)guidearr[i])["GUIDE_VALUE"]),
                                             new OleDbParameter("GUIDE_CAUSE",((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE"]),
                                             new OleDbParameter("GUIDE_UNIT",(((Dictionary<string, string>)guidearr[i])["GUIDE_UNIT"].ToLower().Equals("true") ? "T" : "F")),
                                             new OleDbParameter("INCREASE",((Dictionary<string, string>)guidearr[i])["INCREASE"]),
                                             new OleDbParameter("INCREASE_ARITHMETIC",((Dictionary<string, string>)guidearr[i])["INCREASE_ARITHMETIC"]),
                                             new OleDbParameter("DECREASE",((Dictionary<string, string>)guidearr[i])["DECREASE"]),
                                             new OleDbParameter("DECREASE_ARITHMETIC",((Dictionary<string, string>)guidearr[i])["DECREASE_ARITHMETIC"]),
                                             new OleDbParameter("STATION_YEAR",stationyear),
                                             new OleDbParameter("MINUSFLAG",(((Dictionary<string, string>)guidearr[i])["MINUSFLAG"].ToLower().Equals("true") ? "1" : "0")),
                                             new OleDbParameter("PLUSFLAG",(((Dictionary<string, string>)guidearr[i])["PLUSFLAG"].ToLower().Equals("true") ? "1" : "0")),
                                             new OleDbParameter("PLUS_INCREASE",((Dictionary<string, string>)guidearr[i])["PLUS_INCREASE"]),
                                             new OleDbParameter("PLUS_ARITHMETIC",((Dictionary<string, string>)guidearr[i])["PLUS_ARITHMETIC"]),
                                             new OleDbParameter("MINUS_INCREASE",((Dictionary<string, string>)guidearr[i])["MINUS_INCREASE"]),
                                             new OleDbParameter("MINUS_ARITHMETIC",((Dictionary<string, string>)guidearr[i])["MINUS_ARITHMETIC"]),
                                             new OleDbParameter("THRESHOLD_VALUE",((Dictionary<string, string>)guidearr[i])["THRESHOLD_VALUE"])
                                         };
                //OracleOledbBase.ExecuteNonQuery(stradd.ToString(),parameteradd);
                List listAdd = new List();
                listAdd.StrSql = stradd;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);

        }

        /// <summary>
        /// 保存科室指标目标值
        /// </summary>
        /// <param name="stationcode">指标代码</param>
        /// <param name="stationyear"></param>
        /// <param name="guidegathercode"></param>
        /// <param name="guidearr"></param>
        public void SaveDeptGuideTargetByDept(string guidecode, string stationyear, string guidegathercode, ArrayList guidearr)
        {
            MyLists listtable = new MyLists();
            string sqlstr = string.Format(@"DELETE FROM {0}.DEPT_GUIDE_INFORMATION  WHERE GUIDE_CODE = '{1}' AND STATION_YEAR = {2}", DataUser.HOSPITALSYS, guidecode, stationyear);

            List listdel = new List();
            listdel.StrSql = sqlstr;
            listdel.Parameters = new OleDbParameter[] { };
            listtable.Add(listdel);

            for (int i = 0; i < guidearr.Count; i++)
            {
                
                string str13 = string.IsNullOrEmpty(((Dictionary<string, string>)guidearr[i])["PLUS_INCREASE"]) ? "0" : ((Dictionary<string, string>)guidearr[i])["PLUS_INCREASE"];
                string str14 = string.IsNullOrEmpty(((Dictionary<string, string>)guidearr[i])["PLUS_ARITHMETIC"]) ? "0" : ((Dictionary<string, string>)guidearr[i])["PLUS_ARITHMETIC"];
                string str15 = string.IsNullOrEmpty(((Dictionary<string, string>)guidearr[i])["MINUS_INCREASE"]) ? "0" : ((Dictionary<string, string>)guidearr[i])["MINUS_INCREASE"];
                string str16 = string.IsNullOrEmpty(((Dictionary<string, string>)guidearr[i])["MINUS_ARITHMETIC"]) ? "0" : ((Dictionary<string, string>)guidearr[i])["MINUS_ARITHMETIC"];
                string str17 = string.IsNullOrEmpty(((Dictionary<string, string>)guidearr[i])["THRESHOLD_VALUE"]) ? "0" : ((Dictionary<string, string>)guidearr[i])["THRESHOLD_VALUE"];
                StringBuilder stradd = new StringBuilder();
                stradd.AppendFormat(@"
               INSERT INTO {0}.DEPT_GUIDE_INFORMATION 
               (
                 DEPT_CODE,GUIDE_CODE,GUIDE_VALUE,GUIDE_CAUSE,GUIDE_UNIT,INCREASE, INCREASE_ARITHMETIC,DECREASE,DECREASE_ARITHMETIC,STATION_YEAR,MINUSFLAG,PLUSFLAG,FIXNUM,PLUS_INCREASE,PLUS_ARITHMETIC,MINUS_INCREASE,MINUS_ARITHMETIC,THRESHOLD_VALUE,PLUS_LIMIT,MINUS_LIMIT,GUIDE_CAUSE1,GUIDE_CAUSE2,GUIDE_CAUSE3,GUIDE_CAUSE4,GUIDE_CAUSE5,GUIDE_CAUSE6,GUIDE_CAUSE7,GUIDE_CAUSE8,GUIDE_CAUSE9,GUIDE_CAUSE10,GUIDE_CAUSE11,GUIDE_CAUSE12
               ) values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?) ", DataUser.HOSPITALSYS);
                OleDbParameter[] parameteradd = {   new OleDbParameter("DEPT_CODE",((Dictionary<string, string>)guidearr[i])["DEPT_CODE"]),
                                             new OleDbParameter("GUIDE_CODE",guidecode),
                                             new OleDbParameter("GUIDE_VALUE",((Dictionary<string, string>)guidearr[i])["GUIDE_VALUE"]),
                                             new OleDbParameter("GUIDE_CAUSE",((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE"]),
                                             new OleDbParameter("GUIDE_UNIT",(((Dictionary<string, string>)guidearr[i])["GUIDE_UNIT"].ToLower().Equals("true") ? "T" : "F")),
                                             
                                             new OleDbParameter("INCREASE",((Dictionary<string, string>)guidearr[i])["INCREASE"]),
                                             new OleDbParameter("INCREASE_ARITHMETIC",((Dictionary<string, string>)guidearr[i])["INCREASE_ARITHMETIC"]),
                                             new OleDbParameter("DECREASE",((Dictionary<string, string>)guidearr[i])["DECREASE"]),
                                             new OleDbParameter("DECREASE_ARITHMETIC",((Dictionary<string, string>)guidearr[i])["DECREASE_ARITHMETIC"]),

                                             new OleDbParameter("STATION_YEAR",stationyear),
                                             new OleDbParameter("MINUSFLAG",(((Dictionary<string, string>)guidearr[i])["MINUSFLAG"].ToLower().Equals("true") ? "1" : "0")),
                                             new OleDbParameter("PLUSFLAG",(((Dictionary<string, string>)guidearr[i])["PLUSFLAG"].ToLower().Equals("true") ? "1" : "0")),
                                             new OleDbParameter("FIXNUM",(((Dictionary<string, string>)guidearr[i])["FIXNUM"].ToLower().Equals("true") ? "1" : "0")),
                                             new OleDbParameter("PLUS_INCREASE",str13),
                                             new OleDbParameter("PLUS_ARITHMETIC",str14),
                                             new OleDbParameter("MINUS_INCREASE",str15),
                                             new OleDbParameter("MINUS_ARITHMETIC",str16),
                                             new OleDbParameter("THRESHOLD_VALUE",str17),
                                             new OleDbParameter("PLUS_LIMIT",((Dictionary<string, string>)guidearr[i])["PLUS_LIMIT"]),
                                             new OleDbParameter("MINUS_LIMIT",((Dictionary<string, string>)guidearr[i])["MINUS_LIMIT"]),

                                             new OleDbParameter("GUIDE_CAUSE1",Convert.IsDBNull(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE1"])? 0:Convert.ToDouble(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE1"])),
                                             new OleDbParameter("GUIDE_CAUSE2",Convert.IsDBNull(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE2"])? 0:Convert.ToDouble(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE2"])),
                                             new OleDbParameter("GUIDE_CAUSE3",Convert.IsDBNull(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE3"])? 0:Convert.ToDouble(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE3"])),
                                             new OleDbParameter("GUIDE_CAUSE4",Convert.IsDBNull(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE4"])? 0:Convert.ToDouble(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE4"])),
                                             new OleDbParameter("GUIDE_CAUSE5",Convert.IsDBNull(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE5"])? 0:Convert.ToDouble(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE5"])),
                                             new OleDbParameter("GUIDE_CAUSE6",Convert.IsDBNull(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE6"])? 0:Convert.ToDouble(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE6"])),
                                             new OleDbParameter("GUIDE_CAUSE7",Convert.IsDBNull(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE7"])? 0:Convert.ToDouble(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE7"])),
                                             new OleDbParameter("GUIDE_CAUSE8",Convert.IsDBNull(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE8"])? 0:Convert.ToDouble(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE8"])),
                                             new OleDbParameter("GUIDE_CAUSE9",Convert.IsDBNull(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE9"])? 0:Convert.ToDouble(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE9"])),
                                             new OleDbParameter("GUIDE_CAUSE10",Convert.IsDBNull(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE10"])? 0:Convert.ToDouble(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE10"])),
                                             new OleDbParameter("GUIDE_CAUSE11",Convert.IsDBNull(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE11"])? 0:Convert.ToDouble(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE11"])),
                                             new OleDbParameter("GUIDE_CAUSE12",Convert.IsDBNull(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE12"])? 0:Convert.ToDouble(((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE12"]))
                                         };
                //OracleOledbBase.ExecuteNonQuery(stradd.ToString(),parameteradd);
                List listAdd = new List();
                listAdd.StrSql = stradd;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);
            }

            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetDeptGuide(string deptcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select b.dept_code,b.dept_name,a.guide_code,C.GUIDE_NAME,a.vs_guide_code,d.guide_name vs_guide_name,a.GUIDE_CAUSE from hospitalsys.DEPT_GUIDE_VS_GUIDE a,comm.sys_dept_dict b,hospitalsys.GUIDE_NAME_DICT c,hospitalsys.GUIDE_NAME_DICT d 
where A.DEPT_CODE=b.dept_code and a.guide_code=C.GUIDE_CODE and A.VS_GUIDE_CODE=D.GUIDE_CODE and a.dept_code='{0}' order by a.guide_code ", deptcode);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="guidecode"></param>
        /// <param name="vsguidecode"></param>
        public bool DelDeptGuide(string deptcode, string guidecode, string vsguidecode)
        {
            string sqlstr = string.Format(@"DELETE FROM hospitalsys.DEPT_GUIDE_VS_GUIDE  WHERE DEPT_CODE = '{0}' AND GUIDE_CODE = '{1}' and VS_GUIDE_CODE='{2}' ", deptcode, guidecode, vsguidecode);
            return OracleOledbBase.ExecuteSql(sqlstr);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="guidecode"></param>
        /// <param name="vsguidecode"></param>
        /// <returns></returns>
        public DataTable GetDeptGuidebyguide(string deptcode, string guidecode, string vsguidecode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select b.dept_code,b.dept_name,a.guide_code,C.GUIDE_NAME,a.vs_guide_code,d.guide_name vs_guide_name,a.GUIDE_CAUSE from hospitalsys.DEPT_GUIDE_VS_GUIDE a,comm.sys_dept_dict b,hospitalsys.GUIDE_NAME_DICT c,hospitalsys.GUIDE_NAME_DICT d 
where A.DEPT_CODE=b.dept_code and a.guide_code=C.GUIDE_CODE and A.VS_GUIDE_CODE=D.GUIDE_CODE and a.dept_code='{0}' and a.GUIDE_CODE='{1}' and a.VS_GUIDE_CODE='{2}' order by a.guide_code ", deptcode, guidecode, vsguidecode);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        public DataTable GetGuideDept(string deptcode, string years)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select a.guide_code,b.guide_name from hospitalsys.DEPT_GUIDE_INFORMATION a,hospitalsys.GUIDE_NAME_DICT b where A.GUIDE_CODE=B.GUIDE_CODE and a.dept_code='{0}' and a.STATION_YEAR='{1}'", deptcode, years);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="guidecode"></param>
        /// <param name="vsguidecode"></param>
        /// <param name="guidecause"></param>
        public void SavedeptGuide(string deptcode, string guidecode, string vsguidecode, string guidecause)
        {
            MyLists listtable = new MyLists();
            string sqlstr = string.Format(@"DELETE FROM hospitalsys.DEPT_GUIDE_VS_GUIDE  WHERE DEPT_CODE = '{0}' AND GUIDE_CODE = '{1}' and VS_GUIDE_CODE='{2}' ", deptcode, guidecode, vsguidecode);

            List listdel = new List();
            listdel.StrSql = sqlstr;
            listdel.Parameters = new OleDbParameter[] { };
            listtable.Add(listdel);

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"insert into hospitalsys.DEPT_GUIDE_VS_GUIDE
                                        (DEPT_CODE, GUIDE_CODE, VS_GUIDE_CODE, GUIDE_CAUSE)
                                         values(?,?,?,?)");
            OleDbParameter[] parameteradd = {   new OleDbParameter("DEPT_CODE",deptcode),
                                             new OleDbParameter("GUIDE_CODE",guidecode),
                                             new OleDbParameter("VS_GUIDE_CODE",vsguidecode),
                                             new OleDbParameter("GUIDE_CAUSE",guidecause)
                                         };
            List listAdd = new List();
            listAdd.StrSql = str.ToString();
            listAdd.Parameters = parameteradd;
            listtable.Add(listAdd);
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stationcode"></param>
        /// <param name="stationyear"></param>
        /// <param name="guidegathercode"></param>
        /// <param name="guidearr"></param>
        public void SaveDeptGuideTarget(string stationcode, string stationyear, string guidegathercode, ArrayList guidearr)
        {
            MyLists listtable = new MyLists();
            string sqlstr = string.Format(@"DELETE FROM {0}.DEPT_GUIDE_INFORMATION  WHERE DEPT_CODE = '{1}' AND STATION_YEAR = {2}", DataUser.HOSPITALSYS, stationcode, stationyear);

            List listdel = new List();
            listdel.StrSql = sqlstr;
            listdel.Parameters = new OleDbParameter[] { };
            listtable.Add(listdel);

            for (int i = 0; i < guidearr.Count; i++)
            {
                StringBuilder stradd = new StringBuilder();
                stradd.AppendFormat(@"
               INSERT INTO {0}.DEPT_GUIDE_INFORMATION 
               (
                 DEPT_CODE,GUIDE_CODE,GUIDE_VALUE,GUIDE_CAUSE,GUIDE_UNIT,INCREASE, INCREASE_ARITHMETIC,DECREASE,DECREASE_ARITHMETIC,STATION_YEAR,MINUSFLAG,PLUSFLAG,FIXNUM,PLUS_INCREASE,PLUS_ARITHMETIC,MINUS_INCREASE,MINUS_ARITHMETIC,THRESHOLD_VALUE,PLUS_LIMIT,MINUS_LIMIT
               ) values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?) ", DataUser.HOSPITALSYS);
                OleDbParameter[] parameteradd = {   new OleDbParameter("DEPT_CODE",stationcode),
                                             new OleDbParameter("GUIDE_CODE",((Dictionary<string, string>)guidearr[i])["GUIDE_CODE"]),
                                             new OleDbParameter("GUIDE_VALUE",((Dictionary<string, string>)guidearr[i])["GUIDE_VALUE"]),
                                             new OleDbParameter("GUIDE_CAUSE",((Dictionary<string, string>)guidearr[i])["GUIDE_CAUSE"]),
                                             new OleDbParameter("GUIDE_UNIT",(((Dictionary<string, string>)guidearr[i])["GUIDE_UNIT"].ToLower().Equals("true") ? "T" : "F")),
                                             
                                             new OleDbParameter("INCREASE",((Dictionary<string, string>)guidearr[i])["INCREASE"]),
                                             new OleDbParameter("INCREASE_ARITHMETIC",((Dictionary<string, string>)guidearr[i])["INCREASE_ARITHMETIC"]),
                                             new OleDbParameter("DECREASE",((Dictionary<string, string>)guidearr[i])["DECREASE"]),
                                             new OleDbParameter("DECREASE_ARITHMETIC",((Dictionary<string, string>)guidearr[i])["DECREASE_ARITHMETIC"]),

                                             new OleDbParameter("STATION_YEAR",stationyear),
                                             new OleDbParameter("MINUSFLAG",(((Dictionary<string, string>)guidearr[i])["MINUSFLAG"].ToLower().Equals("true") ? "1" : "0")),
                                             new OleDbParameter("PLUSFLAG",(((Dictionary<string, string>)guidearr[i])["PLUSFLAG"].ToLower().Equals("true") ? "1" : "0")),
                                             new OleDbParameter("FIXNUM",(((Dictionary<string, string>)guidearr[i])["FIXNUM"].ToLower().Equals("true") ? "1" : "0")),
                                             new OleDbParameter("PLUS_INCREASE",((Dictionary<string, string>)guidearr[i])["PLUS_INCREASE"]),
                                             new OleDbParameter("PLUS_ARITHMETIC",((Dictionary<string, string>)guidearr[i])["PLUS_ARITHMETIC"]),
                                             new OleDbParameter("MINUS_INCREASE",((Dictionary<string, string>)guidearr[i])["MINUS_INCREASE"]),
                                             new OleDbParameter("MINUS_ARITHMETIC",((Dictionary<string, string>)guidearr[i])["MINUS_ARITHMETIC"]),
                                             new OleDbParameter("THRESHOLD_VALUE",((Dictionary<string, string>)guidearr[i])["THRESHOLD_VALUE"]),
                                             new OleDbParameter("PLUS_LIMIT",((Dictionary<string, string>)guidearr[i])["PLUS_LIMIT"]),
                                             new OleDbParameter("MINUS_LIMIT",((Dictionary<string, string>)guidearr[i])["MINUS_LIMIT"])
                                         };
                //OracleOledbBase.ExecuteNonQuery(stradd.ToString(),parameteradd);
                List listAdd = new List();
                listAdd.StrSql = stradd;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);
            }

            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 保存月目标值设置
        /// </summary>
        /// <param name="deptinfos"></param>
        /// <param name="year"></param>
        public void StationMonths(List<GoldNet.Model.PageModels.stationmonthinfo> deptinfos, string stationcode, string year)
        {
            MyLists listtable = new MyLists();
            StringBuilder strdel = new StringBuilder();
            strdel.AppendFormat("delete hospitalsys.STATION_GUIDE_MONTHS where station_code='{0}' and station_year='{1}'", stationcode, year);
            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = new OleDbParameter[] { };
            listtable.Add(listdel);
            foreach (GoldNet.Model.PageModels.stationmonthinfo deptinfo in deptinfos)
            {
                string flags = "";
                for (int i = 1; i <= 12; i++)
                {
                    if (i == 1) flags = deptinfo.MONTHS1;
                    if (i == 2) flags = deptinfo.MONTHS2;
                    if (i == 3) flags = deptinfo.MONTHS3;
                    if (i == 4) flags = deptinfo.MONTHS4;
                    if (i == 5) flags = deptinfo.MONTHS5;
                    if (i == 6) flags = deptinfo.MONTHS6;
                    if (i == 7) flags = deptinfo.MONTHS7;
                    if (i == 8) flags = deptinfo.MONTHS8;
                    if (i == 9) flags = deptinfo.MONTHS9;
                    if (i == 10) flags = deptinfo.MONTHS10;
                    if (i == 11) flags = deptinfo.MONTHS11;
                    if (i == 12) flags = deptinfo.MONTHS12;
                }
                if (flags != "")
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        string values = "";
                        if (i == 1) values = deptinfo.MONTHS1;
                        if (i == 2) values = deptinfo.MONTHS2;
                        if (i == 3) values = deptinfo.MONTHS3;
                        if (i == 4) values = deptinfo.MONTHS4;
                        if (i == 5) values = deptinfo.MONTHS5;
                        if (i == 6) values = deptinfo.MONTHS6;
                        if (i == 7) values = deptinfo.MONTHS7;
                        if (i == 8) values = deptinfo.MONTHS8;
                        if (i == 9) values = deptinfo.MONTHS9;
                        if (i == 10) values = deptinfo.MONTHS10;
                        if (i == 11) values = deptinfo.MONTHS11;
                        if (i == 12) values = deptinfo.MONTHS12;
                        if (values != "" && values != null)
                        {
                            StringBuilder stradd = new StringBuilder();
                            stradd.AppendFormat(@"insert into hospitalsys.STATION_GUIDE_MONTHS
                                        (STATION_CODE, GUIDE_CODE, STATION_YEAR, MONTHS, VALUE)
                                         values(?,?,?,?,?)");
                            OleDbParameter[] parameteradd = {   new OleDbParameter("STATION_CODE",deptinfo.STATION_CODE),
                                             new OleDbParameter("GUIDE_CODE",deptinfo.GUIDE_CODE),
                                             new OleDbParameter("STATION_YEAR",deptinfo.STATION_YEAR),
                                             new OleDbParameter("MONTHS",i),
                                             new OleDbParameter("VALUE",values)
                                         };
                            List listAdd = new List();
                            listAdd.StrSql = stradd;
                            listAdd.Parameters = parameteradd;
                            listtable.Add(listAdd);
                        }
                    }
                }
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 月岗位指标设置
        /// </summary>
        /// <param name="stationcode"></param>
        /// <param name="stationyear"></param>
        /// <returns></returns>
        public DataTable GetStationMonthsSet(string stationcode, string stationyear)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select d.BSC_CLASS_NAME bsc_type,d.BSC_CLASS_NAME||'--'||C.BSC_CLASS_NAME bsc_name,a.station_code,a.guide_code,b.guide_name,a.station_year ");
            for (int i = 1; i <= 12; i++)
            {
                str.AppendFormat(@",(select value from hospitalsys.STATION_GUIDE_MONTHS c where   A.STATION_CODE=C.STATION_CODE
 and A.GUIDE_CODE=C.GUIDE_CODE and A.STATION_YEAR=C.STATION_YEAR and c.MONTHS='{0}') {1}", i, "months" + i.ToString());
            }
            str.AppendFormat(@" FROM   hospitalsys.STATION_GUIDE_INFORMATION a, hospitalsys.GUIDE_NAME_DICT b,hospitalsys.JXGL_GUIDE_BSC_CLASS_DICT c,hospitalsys.JXGL_GUIDE_BSC_CLASS_DICT d
                               WHERE   a.guide_code = b.guide_code and a.station_code='{0}' and A.STATION_YEAR='{1}' and b.bsc=C.BSC_CLASS_CODE and substr(b.bsc,0,2)=d.BSC_CLASS_CODE order by C.BSC_CLASS_NAME", stationcode, stationyear);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }


        /// <summary>
        /// 初始量化岗位指标
        /// 1.判断STATION_GUIDE_INFORMATION表中是否己存在设置的岗位指标
        /// 2.如果与该岗位指标集里的指标条数相符，表达已经设置过，返回
        /// 3.如果不相符，或者STATION_GUIDE_INFORMATION中指标条数与指标集中不相符
        /// 4.删除原有指标，重新添加指标集中的指标到该岗位下，
        /// 5.并根据该岗位的BSC分值，自动分解至每个岗位指标下。
        /// </summary>
        /// <param name="stationcode"></param>
        /// <param name="stationyear"></param>
        /// <param name="guidegathercode"></param>
        public void InitStationGuideTarget(string stationcode, string stationyear, string guidegathercode)
        {
            string sql = string.Format(@" SELECT COUNT(*) AS CNT FROM {0}.STATION_GUIDEGATHER_UPDATE WHERE  STATION_CODE ='{2}' AND  STATION_YEAR = '{1}' ", DataUser.HOSPITALSYS, stationyear, stationcode);
            //return OracleBase.Query(sql);
            string cnt = OracleOledbBase.ExecuteScalar(sql).ToString();
            if (cnt.Equals("0"))
            {
                return;
            }
            else
            {
                ArrayList SQLStringList = new ArrayList();

                sql = string.Format(@"
                 DELETE FROM {0}.STATION_GUIDE_INFORMATION  WHERE STATION_CODE = '{1}' AND STATION_YEAR = {2}", DataUser.HOSPITALSYS, stationcode, stationyear, guidegathercode);
                SQLStringList.Add(sql);

                sql = string.Format(@"
            INSERT INTO {1}.STATION_GUIDE_INFORMATION
              (
                STATION_CODE,GUIDE_CODE,GUIDE_VALUE,GUIDE_CAUSE,GUIDE_UNIT,INCREASE,
                INCREASE_ARITHMETIC,DECREASE,DECREASE_ARITHMETIC,STATION_YEAR,MINUSFLAG
               )
            WITH 
            OO AS
            (
                SELECT STATION_CODE,STATION_NAME,DEPT_CODE,STATION_YEAR,DUTY_ORDER,
                STATION_BSC_CLASS_01 BSCPOINT_01,    STATION_BSC_CLASS_02 BSCPOINT_02,
                STATION_BSC_CLASS_03 BSCPOINT_03,    STATION_BSC_CLASS_04 BSCPOINT_04
                FROM {0}.SYS_STATION_BASIC_INFORMATION WHERE STATION_CODE = '{2}' AND STATION_YEAR = {3} 
            ),
            PP AS
            (
                SELECT STATION_CODE,GUIDE_CODE,GUIDE_VALUE,  GUIDE_CAUSE,GUIDE_UNIT,INCREASE,INCREASE_ARITHMETIC,
                DECREASE,DECREASE_ARITHMETIC,STATION_YEAR,MINUSFLAG 
                FROM {1}.STATION_GUIDE_INFORMATION  WHERE STATION_CODE = '{2}' AND STATION_YEAR = {3} 
            ),
            BB AS
            (
                SELECT AA.GUIDE_CODE,B.GUIDE_NAME,B.BSC FROM  {1}.GUIDE_GATHERS AA
                  LEFT JOIN {1}.GUIDE_NAME_DICT B ON AA.GUIDE_CODE  = B.GUIDE_CODE 
                 WHERE AA.GUIDE_GATHER_CODE  = '{4}'AND SUBSTR(B.BSC,1,2) <='04'  
                 ORDER BY B.BSC
            ),
            CC AS 
            (
                SELECT  D.BSC_CLASS_CODE BSC_TYPE, D.BSC_CLASS_NAME BSC_TYPE_NAME,
                        BB.BSC, C.BSC_CLASS_NAME BSC_NAME,BB.GUIDE_CODE,BB.GUIDE_NAME,
                        DECODE(D.BSC_CLASS_CODE,'01',NVL(OO.BSCPOINT_01,0),'02',NVL(BSCPOINT_02,0),'03',NVL(BSCPOINT_03,0),'04',NVL(BSCPOINT_04,0)) BSCPOINT,
                        PP.GUIDE_VALUE,PP.GUIDE_CAUSE,PP.GUIDE_UNIT,PP.INCREASE,
                        PP.INCREASE_ARITHMETIC,PP.DECREASE,PP.DECREASE_ARITHMETIC,PP.MINUSFLAG,OO.DEPT_CODE,OO.DUTY_ORDER
                FROM BB
                LEFT JOIN {1}.JXGL_GUIDE_BSC_CLASS_DICT C ON BB.BSC = C.BSC_CLASS_CODE
                LEFT JOIN {1}.JXGL_GUIDE_BSC_CLASS_DICT D ON SUBSTR(BB.BSC,1,2) = D.BSC_CLASS_CODE
                LEFT JOIN OO ON 1=1
                LEFT JOIN PP ON BB.GUIDE_CODE = PP.GUIDE_CODE 
            ),
            DD AS
            (
               SELECT BSC_TYPE,BSC_TYPE_NAME,BSC,BSC_NAME,CC.GUIDE_CODE,GUIDE_NAME,BSCPOINT ,
                   DECODE( ROW_NUMBER() OVER( PARTITION BY BSC_TYPE ORDER BY ROWNUM),
                            COUNT(*) OVER( PARTITION BY BSC_TYPE),
                            --(((BSCPOINT / (COUNT(*) OVER( PARTITION BY BSC_TYPE) )) - FLOOR(BSCPOINT / (COUNT(*) OVER( PARTITION BY BSC_TYPE) ))) * (COUNT(*) OVER( PARTITION BY BSC_TYPE) ) ) + FLOOR(BSCPOINT / (COUNT(*) OVER( PARTITION BY BSC_TYPE) )),
                            BSCPOINT - (FLOOR(BSCPOINT / (COUNT(*) OVER( PARTITION BY BSC_TYPE) )) * ( COUNT(*) OVER( PARTITION BY BSC_TYPE)  - 1)),
                            FLOOR(BSCPOINT / (COUNT(*) OVER( PARTITION BY BSC_TYPE) ))  )  GUIDE_VALUE,
                    NVL(DD.GUIDE_FACT,10) GUIDE_CAUSE,
                   'T' AS GUIDE_UNIT,
                    1  AS INCREASE,
                    1  AS INCREASE_ARITHMETIC,
                    1  AS DECREASE,
                    1  AS DECREASE_ARITHMETIC,
                    1  AS MINUSFLAG
              FROM CC
              LEFT JOIN {1}.ASSESS_RESULT_CURRENT_YEAR DD
            ON CC.GUIDE_CODE = DD.GUIDE_CODE AND DECODE(CC.DUTY_ORDER,'0','00',CC.DEPT_CODE) = DD.DEPT_CODE
            )
          SELECT '{2}' AS STATION_CODE,GUIDE_CODE,GUIDE_VALUE,GUIDE_CAUSE,GUIDE_UNIT,INCREASE,INCREASE_ARITHMETIC,DECREASE,DECREASE_ARITHMETIC,{3} AS STATION_YEAR,MINUSFLAG  
            FROM DD ", DataUser.COMM, DataUser.HOSPITALSYS, stationcode, stationyear, guidegathercode);

                SQLStringList.Add(sql);
                OracleBase.ExecuteSqlTran(SQLStringList);

            }


        }

        /// <summary>
        /// 返回岗位对应的指标集，用来量化岗位下的指标目标值量化
        /// 如果在岗位指标值中有该指标集下所对应的数据时，返回既有数据，
        /// 如果不存在(刚刚设置完岗位指标集的情况下，刚返回初始值（自动计算）)
        /// </summary>
        /// <param name="stationcode"></param>
        /// <param name="stationyear"></param>
        /// <param name="guidegathercode"></param>
        /// <returns></returns>
        public DataSet GetStationGuideTarget(string stationcode, string stationyear, string guidegathercode)
        {
            StringBuilder oo = new StringBuilder();
            oo.AppendFormat(@"SELECT STATION_CODE,
                                     STATION_NAME,
                                     DEPT_CODE,
                                     STATION_YEAR,
                                     DUTY_ORDER,
                                     STATION_BSC_CLASS_01 BSCPOINT_01,
                                     STATION_BSC_CLASS_02 BSCPOINT_02,
                                     STATION_BSC_CLASS_03 BSCPOINT_03,
                                     STATION_BSC_CLASS_04 BSCPOINT_04
                               FROM {0}.SYS_STATION_BASIC_INFORMATION WHERE STATION_CODE = '{1}' AND STATION_YEAR = {2}", DataUser.COMM, stationcode, stationyear);

            StringBuilder pp = new StringBuilder();
            pp.AppendFormat(@"SELECT STATION_CODE,
                                     GUIDE_CODE,
                                     GUIDE_VALUE,
                                     GUIDE_CAUSE,
                                     GUIDE_UNIT,
                                     INCREASE,
                                     INCREASE_ARITHMETIC,
                                     DECREASE,
                                     DECREASE_ARITHMETIC,
                                     STATION_YEAR,
                                     NVL(MINUSFLAG,'0') MINUSFLAG,
                                     NVL(PLUSFLAG,'0') PLUSFLAG,
                                     NVL(PLUS_INCREASE,0) PLUS_INCREASE,
                                     NVL(PLUS_ARITHMETIC,0) PLUS_ARITHMETIC,
                                     NVL(MINUS_INCREASE,0) MINUS_INCREASE,
                                     NVL(MINUS_ARITHMETIC,0) MINUS_ARITHMETIC,
                                     THRESHOLD_VALUE 
                                FROM {0}.STATION_GUIDE_INFORMATION  WHERE STATION_CODE = '{1}' AND STATION_YEAR = {2} ", DataUser.HOSPITALSYS, stationcode, stationyear);

            StringBuilder bb = new StringBuilder();
            bb.AppendFormat(@"SELECT AA.GUIDE_CODE,
                                     B.GUIDE_NAME,
                                     B.BSC,
                                     B.ISHIGHGUIDE
                               FROM {0}.GUIDE_GATHERS AA
                               LEFT JOIN {0}.GUIDE_NAME_DICT B ON AA.GUIDE_CODE  = B.GUIDE_CODE 
                              WHERE AA.GUIDE_GATHER_CODE  = '{1}' AND SUBSTR(B.BSC,1,2) <='04'  
                              ORDER BY B.BSC", DataUser.HOSPITALSYS, guidegathercode);

            StringBuilder ee = new StringBuilder();
            ee.AppendFormat(@"SELECT MAX (CASE WHEN ID = 80 THEN PROPERTYVALUE ELSE 0 END) GY,
                                     MAX (CASE WHEN ID = 81 THEN PROPERTYVALUE ELSE 0 END) DY
                                FROM PERFORMANCE.SET_INDICATIONCONFIG
                               WHERE ID = 80 OR ID = 81 ");

            StringBuilder cc = new StringBuilder();
            cc.AppendFormat(@"SELECT D.BSC_CLASS_CODE BSC_TYPE, 
                                     D.BSC_CLASS_NAME BSC_TYPE_NAME,
                                     BB.BSC, 
                                     C.BSC_CLASS_NAME BSC_NAME,
                                     BB.GUIDE_CODE,
                                     BB.GUIDE_NAME,
                                     DECODE(D.BSC_CLASS_CODE,'01',NVL(OO.BSCPOINT_01,0),'02',NVL(BSCPOINT_02,0),'03',NVL(BSCPOINT_03,0),'04',NVL(BSCPOINT_04,0)) BSCPOINT,
                                     PP.GUIDE_VALUE,
                                     PP.GUIDE_CAUSE,
                                     PP.GUIDE_UNIT,
                                     PP.INCREASE,
                                     PP.INCREASE_ARITHMETIC,
                                     PP.DECREASE,
                                     PP.DECREASE_ARITHMETIC,
                                     PP.MINUSFLAG,
                                     OO.DEPT_CODE,
                                     OO.DUTY_ORDER,

                                     BB.ISHIGHGUIDE,
                                     EE.GY,
                                     EE.DY,
                                     PP.PLUSFLAG,
                                     PP.PLUS_INCREASE,
                                     PP.PLUS_ARITHMETIC,
                                     PP.MINUS_INCREASE,
                                     PP.MINUS_ARITHMETIC,
                                     PP.THRESHOLD_VALUE     
                               FROM ({0}) BB 
                               LEFT JOIN {1}.JXGL_GUIDE_BSC_CLASS_DICT C ON BB.BSC = C.BSC_CLASS_CODE
                               LEFT JOIN {1}.JXGL_GUIDE_BSC_CLASS_DICT D ON SUBSTR(BB.BSC,1,2) = D.BSC_CLASS_CODE
                               LEFT JOIN ({2}) OO ON 1=1
                               LEFT JOIN ({3}) PP ON BB.GUIDE_CODE = PP.GUIDE_CODE 
                               LEFT JOIN ({4}) EE ON 1=1", bb.ToString(), DataUser.HOSPITALSYS, oo.ToString(), pp.ToString(), ee.ToString());

            string sql = string.Format(@"SELECT BSC_TYPE,
                                                BSC_TYPE_NAME,
                                                BSC,
                                                BSC_NAME,
                                                GUIDE_CODE,
                                                GUIDE_NAME,
                                                BSCPOINT,
                                                GUIDE_VALUE,
                                                GUIDE_CAUSE,
                                                GUIDE_UNIT,
                                                nvl(CASE WHEN ISHIGHGUIDE='1' THEN DECODE(GUIDE_VALUE,0,0,(GUIDE_CAUSE-THRESHOLD_VALUE)/GUIDE_VALUE) ELSE 0 END,0) INCREASE,
                                                INCREASE_ARITHMETIC,
                                                nvl(CASE WHEN ISHIGHGUIDE='0' THEN DECODE(GUIDE_VALUE,0,0,(GUIDE_CAUSE-THRESHOLD_VALUE)/GUIDE_VALUE) ELSE 0 END,0) DECREASE,
                                                DECREASE_ARITHMETIC,
                                                round(NVL(MINUSFLAG,'0'),2) MINUSFLAG,
                                                round(NVL(PLUSFLAG,'0'),2) PLUSFLAG,
                                                round(NVL(PLUS_INCREASE,0),2) PLUS_INCREASE,
                                                round(NVL(PLUS_ARITHMETIC,0),2) PLUS_ARITHMETIC,
                                                round(NVL(MINUS_INCREASE,0),2) MINUS_INCREASE,
                                                round(NVL(MINUS_ARITHMETIC,0),2) MINUS_ARITHMETIC,
                                                round(NVL(THRESHOLD_VALUE,0),2) THRESHOLD_VALUE,
                                                ISHIGHGUIDE
                                           FROM
                                           (SELECT BSC_TYPE,
                                                   BSC_TYPE_NAME,
                                                   BSC,
                                                   BSC_TYPE_NAME||'--'||BSC_NAME BSC_NAME ,
                                                   CC.GUIDE_CODE,
                                                   GUIDE_NAME,
                                                   BSCPOINT ,
                                                   NVL ( GUIDE_VALUE,
                                                    DECODE( ROW_NUMBER() OVER( PARTITION BY BSC_TYPE ORDER BY ROWNUM),
                                                            COUNT(*) OVER( PARTITION BY BSC_TYPE),
                                                            --(((BSCPOINT / (COUNT(*) OVER( PARTITION BY BSC_TYPE) )) - FLOOR(BSCPOINT / (COUNT(*) OVER( PARTITION BY BSC_TYPE) ))) * (COUNT(*) OVER( PARTITION BY BSC_TYPE) ) ) + FLOOR(BSCPOINT / (COUNT(*) OVER( PARTITION BY BSC_TYPE) )),
                                                            BSCPOINT - (FLOOR(BSCPOINT / (COUNT(*) OVER( PARTITION BY BSC_TYPE) )) * ( COUNT(*) OVER( PARTITION BY BSC_TYPE)  - 1)),
                                                            FLOOR(BSCPOINT / (COUNT(*) OVER( PARTITION BY BSC_TYPE) ))  )
                                                    ) GUIDE_VALUE,
                                                   NVL(NVL (GUIDE_CAUSE,DD.GUIDE_FACT),10) GUIDE_CAUSE,
                                                   NVL (GUIDE_UNIT,'T') GUIDE_UNIT,
                                                   NVL (INCREASE,1) INCREASE,
                                                   CASE WHEN ISHIGHGUIDE='1' THEN 1 ELSE 0 END INCREASE_ARITHMETIC,
                                                   NVL (DECREASE,1) DECREASE,
                                                   CASE WHEN ISHIGHGUIDE='0' THEN -1 ELSE 0 END DECREASE_ARITHMETIC,
                                                   MINUSFLAG,
                                                   PLUSFLAG,
                                                   PLUS_INCREASE,
                                                   PLUS_ARITHMETIC,
                                                   MINUS_INCREASE,
                                                   MINUS_ARITHMETIC,
                                                   NVL (DECODE(THRESHOLD_VALUE,0,NULL,THRESHOLD_VALUE),DECODE(ISHIGHGUIDE,'1',GUIDE_CAUSE*(GY/100),'0',GUIDE_CAUSE*(DY/100))) THRESHOLD_VALUE,
                                                   ISHIGHGUIDE
                                             FROM ({0}) CC  LEFT JOIN {1}.ASSESS_RESULT_CURRENT_YEAR DD ON CC.GUIDE_CODE = DD.GUIDE_CODE AND DECODE(CC.DUTY_ORDER,'0','00',CC.DEPT_CODE) = DD.DEPT_CODE order by bsc_name)", cc.ToString(), DataUser.HOSPITALSYS);

            //此处注意一下，用ole驱动无法取出sql执行结果，改成oraclebase类，原因待查
            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取科室指标量化值
        /// </summary>
        /// <param name="stationcode"></param>
        /// <param name="stationyear"></param>
        /// <param name="guidegathercode"></param>
        /// <returns></returns>
        public DataSet GetDeptGuideTarget(string stationcode, string stationyear, string guidegathercode)
        {
            StringBuilder oo = new StringBuilder();
            oo.AppendFormat(@"SELECT DEPT_CODE,
                                     STATION_YEAR,
                                     STATION_BSC_CLASS_01 BSCPOINT_01,    
                                     STATION_BSC_CLASS_02 BSCPOINT_02,
                                     STATION_BSC_CLASS_03 BSCPOINT_03,    
                                     STATION_BSC_CLASS_04 BSCPOINT_04
                                FROM {0}.DEPT_GATHERS WHERE DEPT_CODE = '{1}' AND STATION_YEAR = {2}", DataUser.HOSPITALSYS, stationcode, stationyear);

            StringBuilder pp = new StringBuilder();
            pp.AppendFormat(@"SELECT DEPT_CODE,
                                     GUIDE_CODE,
                                     GUIDE_VALUE,  
                                     GUIDE_CAUSE,
                                     GUIDE_UNIT,
                                     INCREASE,
                                     INCREASE_ARITHMETIC,
                                     DECREASE,
                                     DECREASE_ARITHMETIC,
                                     STATION_YEAR,
                                     NVL(MINUSFLAG,'0') MINUSFLAG,
                                     NVL(PLUSFLAG,'0') PLUSFLAG,
                                     NVL(FIXNUM,'0') FIXNUM,
                                     NVL(PLUS_INCREASE,0) PLUS_INCREASE,
                                     NVL(PLUS_ARITHMETIC,0) PLUS_ARITHMETIC,
                                     NVL(MINUS_INCREASE,0) MINUS_INCREASE,
                                     NVL(MINUS_ARITHMETIC,0) MINUS_ARITHMETIC,
                                     THRESHOLD_VALUE,
                                     PLUS_LIMIT,
                                     MINUS_LIMIT
                                FROM {0}.DEPT_GUIDE_INFORMATION  WHERE DEPT_CODE = '{1}' AND STATION_YEAR = {2} ", DataUser.HOSPITALSYS, stationcode, stationyear);

            StringBuilder bb = new StringBuilder();
            bb.AppendFormat(@"SELECT AA.GUIDE_CODE,
                                     B.GUIDE_NAME,
                                     B.BSC,
                                     B.ISHIGHGUIDE,
                                     B.THRESHOLD_RATIO,
                                     B.SORT_NO
                               FROM {0}.GUIDE_GATHERS AA
                                    LEFT JOIN {0}.GUIDE_NAME_DICT B ON AA.GUIDE_CODE  = B.GUIDE_CODE 
                              WHERE AA.GUIDE_GATHER_CODE  = '{1}' AND SUBSTR(B.BSC,1,2) <='04'  
                              ORDER BY B.BSC", DataUser.HOSPITALSYS, guidegathercode);

            StringBuilder ee = new StringBuilder();
            ee.AppendFormat(@"SELECT MAX (CASE WHEN ID = 80 THEN PROPERTYVALUE ELSE 0 END) GY,
                                     MAX (CASE WHEN ID = 81 THEN PROPERTYVALUE ELSE 0 END) DY
                                FROM PERFORMANCE.SET_INDICATIONCONFIG
                               WHERE ID = 80 OR ID = 81 ");

            StringBuilder cc = new StringBuilder();
            cc.AppendFormat(@"SELECT D.BSC_CLASS_CODE BSC_TYPE, 
                                     D.BSC_CLASS_NAME BSC_TYPE_NAME,
                                     BB.BSC, 
                                     C.BSC_CLASS_NAME BSC_NAME,
                                     BB.GUIDE_CODE,
                                     BB.GUIDE_NAME,
                                     DECODE(D.BSC_CLASS_CODE,'01',NVL(OO.BSCPOINT_01,0),'02',NVL(BSCPOINT_02,0),'03',NVL(BSCPOINT_03,0),'04',NVL(BSCPOINT_04,0)) BSCPOINT,
                                     PP.GUIDE_VALUE,
                                     PP.GUIDE_CAUSE,
                                     PP.GUIDE_UNIT,
                                     PP.INCREASE,
                                     PP.INCREASE_ARITHMETIC,
                                     PP.DECREASE,
                                     PP.DECREASE_ARITHMETIC,
                                     PP.MINUSFLAG  ,
                                     OO.DEPT_CODE,
                                     BB.ISHIGHGUIDE,
                                     BB.THRESHOLD_RATIO,
                                     PP.PLUSFLAG,
                                     PP.FIXNUM,
                                     PP.PLUS_INCREASE,
                                     PP.PLUS_ARITHMETIC,
                                     PP.MINUS_INCREASE,
                                     PP.MINUS_ARITHMETIC,
                                     PP.THRESHOLD_VALUE,
                                     PP.PLUS_LIMIT,
                                     PP.MINUS_LIMIT,
                                     BB.SORT_NO    
                               FROM ({0}) BB 
                                     LEFT JOIN {1}.JXGL_GUIDE_BSC_CLASS_DICT C ON BB.BSC = C.BSC_CLASS_CODE
                                     LEFT JOIN {1}.JXGL_GUIDE_BSC_CLASS_DICT D ON SUBSTR(BB.BSC,1,2) = D.BSC_CLASS_CODE
                                     LEFT JOIN ({2}) OO ON 1=1
                                     LEFT JOIN ({3}) PP ON BB.GUIDE_CODE = PP.GUIDE_CODE ", bb.ToString(), DataUser.HOSPITALSYS, oo.ToString(), pp.ToString());


            string sql = string.Format(@"SELECT BSC_TYPE,
                                                BSC_TYPE_NAME,
                                                BSC,
                                                BSC_NAME,
                                                GUIDE_CODE,
                                                GUIDE_NAME,
                                                BSCPOINT,
                                                GUIDE_VALUE,
                                                GUIDE_CAUSE,
                                                GUIDE_UNIT,
                                                nvl(INCREASE,nvl(CASE WHEN ISHIGHGUIDE='1' THEN DECODE(GUIDE_VALUE,0,0,ROUND((GUIDE_CAUSE-THRESHOLD_VALUE)/GUIDE_VALUE,2)) ELSE 0 END,0)) INCREASE,
                                                nvl(INCREASE_ARITHMETIC,0) INCREASE_ARITHMETIC,
                                                nvl(DECREASE,nvl(CASE WHEN ISHIGHGUIDE='0' THEN DECODE(GUIDE_VALUE,0,0,ROUND((GUIDE_CAUSE-THRESHOLD_VALUE)/GUIDE_VALUE,2)) ELSE 0 END,0)) DECREASE,
                                                nvl(DECREASE_ARITHMETIC,0) DECREASE_ARITHMETIC,
                                                round(NVL(MINUSFLAG,'0'),2) MINUSFLAG,
                                                round(NVL(PLUSFLAG,'0'),2) PLUSFLAG,
                                                round(NVL(FIXNUM,'0'),2) FIXNUM,
                                                round(NVL(PLUS_INCREASE,0),2) PLUS_INCREASE,
                                                round(NVL(PLUS_ARITHMETIC,0),2) PLUS_ARITHMETIC,
                                                round(NVL(MINUS_INCREASE,0),2) MINUS_INCREASE,
                                                round(NVL(MINUS_ARITHMETIC,0),2) MINUS_ARITHMETIC,
                                                round(NVL(THRESHOLD_VALUE,0),2) THRESHOLD_VALUE,
                                                ISHIGHGUIDE,
                                                PLUS_LIMIT,
                                                MINUS_LIMIT
                                           FROM
                                           (SELECT BSC_TYPE,
                                                BSC_TYPE_NAME,
                                                BSC,
                                                BSC_TYPE_NAME||'--'||BSC_NAME BSC_NAME ,
                                                CC.GUIDE_CODE,
                                                GUIDE_NAME,
                                                BSCPOINT ,
                                                NVL ( GUIDE_VALUE,
                                                    DECODE( ROW_NUMBER() OVER( PARTITION BY BSC_TYPE ORDER BY ROWNUM),
                                                            COUNT(*) OVER( PARTITION BY BSC_TYPE),
                                                            
                                                            BSCPOINT - (DECODE((COUNT(*) OVER( PARTITION BY BSC_TYPE) ),0,0,FLOOR(BSCPOINT / (COUNT(*) OVER( PARTITION BY BSC_TYPE) )) * ( COUNT(*) OVER( PARTITION BY BSC_TYPE)  - 1))),
                                                            DECODE(COUNT(*) OVER( PARTITION BY BSC_TYPE),0,0,FLOOR(BSCPOINT / (COUNT(*) OVER( PARTITION BY BSC_TYPE) )))  )
                                                ) GUIDE_VALUE,
                                                NVL(NVL (GUIDE_CAUSE,DD.GUIDE_FACT),10) GUIDE_CAUSE,
                                                NVL (GUIDE_UNIT,'T') GUIDE_UNIT,
                                               NVL (INCREASE,1) INCREASE,
                                               nvl(INCREASE_ARITHMETIC,CASE WHEN ISHIGHGUIDE='1' THEN 1 ELSE 0 END) INCREASE_ARITHMETIC,
                                               NVL (DECREASE,1) DECREASE,
                                               nvl(DECREASE_ARITHMETIC,CASE WHEN ISHIGHGUIDE='0' THEN -1 ELSE 0 END) DECREASE_ARITHMETIC,
                                               MINUSFLAG,
                                               PLUSFLAG,
                                               FIXNUM,
                                               PLUS_INCREASE,
                                               PLUS_ARITHMETIC,
                                               MINUS_INCREASE,
                                               MINUS_ARITHMETIC,
                                               NVL (DECODE(THRESHOLD_VALUE,0,NULL,THRESHOLD_VALUE),GUIDE_CAUSE*(THRESHOLD_RATIO/100)) THRESHOLD_VALUE,
                                               ISHIGHGUIDE,
                                               PLUS_LIMIT,
                                               MINUS_LIMIT
                                           FROM ({0}) CC  LEFT JOIN {1}.ASSESS_RESULT_CURRENT_YEAR DD ON CC.GUIDE_CODE = DD.GUIDE_CODE AND CC.DEPT_CODE = DD.DEPT_CODE order by BSC_TYPE,SORT_NO)", cc.ToString(), DataUser.HOSPITALSYS);

            return OracleOledbBase.ExecuteDataSet(sql);
        }


        /// <summary>
        /// 返回因为指标集所包含的指标项目发生改变，
        /// 而导致与该指标集关联的岗位指标目标量化需要更新的岗位代码
        /// </summary>
        /// <param name="stationyear"></param>
        /// <returns></returns>
        public DataSet GetStationGatherDiff(string stationyear)
        {
            string sql = string.Format(@" DELETE FROM {0}.STATION_GUIDE_INFORMATION WHERE  STATION_YEAR = '{1}'AND  STATION_CODE IN 
                                        (SELECT STATION_CODE FROM {2}.SYS_STATION_BASIC_INFORMATION WHERE  TRIM(GUIDE_GATHER_CODE) IS  NULL AND  STATION_YEAR = '{1}')", DataUser.HOSPITALSYS, stationyear, DataUser.COMM);
            OracleOledbBase.ExecuteNonQuery(sql);

            sql = string.Format(@"  SELECT * FROM {0}.STATION_GUIDEGATHER_UPDATE WHERE   STATION_YEAR = '{1}' ", DataUser.HOSPITALSYS, stationyear);

            return OracleOledbBase.ExecuteDataSet(sql);

        }


        /// <summary>
        /// 通过岗位编号和岗位年度删除岗位
        /// </summary>
        /// <param name="station_code">岗位编号</param>
        /// <param name="station_year">岗位年度</param>
        public void DelStationByCodeAndYear(string station_code, string station_year)
        {
            MyLists listttrans = new MyLists();
            List listindex = new List();
            listindex.StrSql = string.Format(@" DELETE FROM {0}.SYS_STATION_BASIC_INFORMATION  WHERE STATION_CODE IN({1}) AND STATION_YEAR='{2}'", DataUser.COMM, station_code, station_year);
            OleDbParameter[] parameters = {
                    new OleDbParameter("", "")
		    };

            listindex.Parameters = parameters;
            listttrans.Add(listindex);

            List listindex1 = new List();
            listindex1.StrSql = string.Format(@" DELETE FROM  {0}.STATION_GUIDE_INFORMATION WHERE STATION_CODE IN ({1}) AND STATION_YEAR='{2}'", DataUser.HOSPITALSYS, station_code, station_year);
            OleDbParameter[] parameters1 = {
				    new OleDbParameter("", ""),
			};

            listindex1.Parameters = parameters1;
            listttrans.Add(listindex1);

            List listindex2 = new List();
            listindex2.StrSql = string.Format(@" DELETE FROM {0}.SYS_STATION_PERSONNEL_INFO  WHERE STATION_CODE IN({1}) AND STATION_YEAR='{2}'", DataUser.COMM, station_code, station_year);

            OleDbParameter[] parameters2 = {
				    new OleDbParameter("", ""),
				};

            listindex2.Parameters = parameters2;
            listttrans.Add(listindex2);

            OracleOledbBase.ExecuteTranslist(listttrans);

        }

        /// <summary>
        /// 建立岗位
        /// </summary>
        /// <param name="station_year">岗位年度</param>
        public void CreateStation(string station_year)
        {
            string sql = string.Format(@"
                INSERT INTO COMM.SYS_STATION_BASIC_INFORMATION
                (
                    STATION_CODE, STATION_NAME, DEPT_CODE, STATION_CODE_REMARK,
                    WHETHER, STIPEND, WORK_CONTENT, POST_COMPETENCY,
                    WORK_CIRCUMSTANCE, INPUT_USER,  INPUT_TIME, AUDITING_USER ,
                    DUTY_ORDER, GUIDE_GATHER_CODE, STATION_YEAR,
                    STATION_BSC_CLASS_01, STATION_BSC_CLASS_02, STATION_BSC_CLASS_03,
                    STATION_BSC_CLASS_04,STATION_TYPE,STATION_DICT_CODE,SEQUENCE,GUIDE_TYPE
                ) 
                WITH DD AS
                (
                SELECT DISTINCT  B.DEPT_CODE,
                       C.STATION_NAME, C.STATION_CODE_REMARK, C.WHETHER,
                       C.STIPEND, C.WORK_CONTENT, C.POST_COMPETENCY,
                       C.WORK_CIRCUMSTANCE, C.INPUT_USER,
                       TO_CHAR(TO_DATE(C.INPUT_TIME,'yyyy-mm-dd'),'yyyy-mm-dd') AS INPUT_TIME,
                       C.AUDITING_USER, C.GUIDE_GATHER_CODE,'{0}' AS STATION_YEAR, C.STATION_BSC_CLASS_01,
                       C.STATION_BSC_CLASS_02, C.STATION_BSC_CLASS_03,
                       C.STATION_BSC_CLASS_04, C.ID AS STATION_DICT_CODE,
                       C.DUTY_ORDER,TO_NUMBER(C.SEQUENCE) AS SEQUENCE,C.GUIDE_TYPE
                  FROM {2}.NEW_STAFF_INFO A,
                       {1}.SYS_DEPT_DICT B,
                       {1}.SYS_STATION_MAINTENANCE_DICT C
                 WHERE DECODE(A.GROUP_ID, NULL,A.DEPT_CODE,A.GROUP_ID ) = B.DEPT_CODE
                   AND A.STATION_CODE = C.ID
                   AND A.ADD_MARK = '1'
                   AND C.STATION_NAME NOT IN (
                    SELECT M.STATION_NAME FROM {1}.SYS_STATION_BASIC_INFORMATION M WHERE M.STATION_YEAR='{0}'
                    )
                ),
                EE AS
                ( 
                SELECT ROW_NUMBER() OVER( PARTITION BY DD.DEPT_CODE  ORDER BY DD.SEQUENCE) AS IDS,BB.STATION_TYPE_CODE, DD.* FROM DD 
                  LEFT JOIN {1}.SYS_STATION_MAINTENANCE_DICT BB 
                    ON DD.STATION_DICT_CODE = BB.ID
                ),
                FF AS 
                (
                SELECT NVL( AA.DEPT_CODE || LPAD(  REPLACE (TO_CHAR(  TO_NUMBER(AA.STATION_CODE)+ EE.IDS ),EE.DEPT_CODE,''),5,'0')  , EE.DEPT_CODE ||  LPAD( TO_CHAR( EE.IDS) ,5,'0')    )  STATION_CODE ,EE.* 
                  FROM EE
                  LEFT JOIN 
                   (
                    SELECT DEPT_CODE,MAX(STATION_CODE) STATION_CODE ,STATION_YEAR  FROM 
                    {1}.SYS_STATION_BASIC_INFORMATION 
                    GROUP BY STATION_YEAR, DEPT_CODE )  AA
                 ON EE.DEPT_CODE = AA.DEPT_CODE
                AND EE.STATION_YEAR = AA.STATION_YEAR
                )
                SELECT   
                     STATION_CODE, STATION_NAME, DEPT_CODE, STATION_CODE_REMARK,
                     WHETHER, STIPEND, WORK_CONTENT, POST_COMPETENCY,
                     WORK_CIRCUMSTANCE, INPUT_USER,  TO_DATE(INPUT_TIME,'yyyy-mm-dd') INPUT_TIME, AUDITING_USER,
                     DUTY_ORDER, GUIDE_GATHER_CODE, STATION_YEAR,
                     STATION_BSC_CLASS_01, STATION_BSC_CLASS_02, STATION_BSC_CLASS_03,
                     STATION_BSC_CLASS_04,STATION_TYPE_CODE,STATION_DICT_CODE,SEQUENCE,GUIDE_TYPE
                FROM FF ", station_year, DataUser.COMM, DataUser.RLZY);
            OracleOledbBase.ExecuteNonQuery(sql);

        }

        /// <summary>
        /// 通过科室编码和岗位时间查不在岗人员
        /// </summary>
        /// <param name="dept_code">科室编码</param>
        /// <param name="station_year">岗位时间</param>
        /// <returns></returns>
        public DataTable GetStationPersonByDept(string dept_code, string station_year)
        {
            string sql = string.Format(@"
                SELECT STAFF_ID, NAME AS STAFF_NAME 
                  FROM {2}.NEW_STAFF_INFO 
                 WHERE DEPT_CODE  = '{0}'
                   AND ADD_MARK = '1'
                   AND STAFF_ID NOT IN (
                                SELECT PERSON_ID FROM {3}.SYS_STATION_PERSONNEL_INFO 
                                 WHERE STATION_CODE IN ( SELECT STATION_CODE FROM {3}.SYS_STATION_BASIC_INFORMATION  WHERE DEPT_CODE ='{0}')
                                   AND STATION_YEAR = '{1}')", dept_code, station_year, DataUser.RLZY, DataUser.COMM);

            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }


        /// <summary>
        /// 通过岗位编码和岗位时间查在岗人员
        /// </summary>
        /// <param name="station_code">岗位编码</param>
        /// <param name="station_year">岗位时间</param>
        /// <returns></returns>
        public DataTable GetStationPersonnelOnJob(string station_code, string station_year)
        {
            string sql = string.Format(@"
                SELECT PERSON_ID AS STAFF_ID, PERSON_NAME AS STAFF_NAME
                  FROM {2}.SYS_STATION_PERSONNEL_INFO 
                 WHERE STATION_CODE = '{0}' AND STATION_YEAR = '{1}'", station_code, station_year, DataUser.COMM);

            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        /// <summary>
        /// 岗位下属人员更新【进岗】/【离岗】操作
        /// </summary>
        /// <param name="flag">flag==1 离岗 flag==0 进岗</param>
        /// <param name="lists">选择的人员列表 取值格式lists[0]["STAFF_ID"]</param>
        /// <param name="stationcode"></param>
        /// <param name="stationyear"></param>
        public void StationPersonUpdate(string flag, Dictionary<string, string>[] lists, string stationcode, string stationyear)
        {
            if (lists.Length.Equals(0))
            {
                return;
            }
            string sqlstr = "";
            StringBuilder sql = new StringBuilder();
            //离岗
            if (flag.Equals("1"))
            {
                sql.AppendFormat(@" DELETE FROM {0}.SYS_STATION_PERSONNEL_INFO WHERE STATION_CODE='{1}' AND STATION_YEAR='{2}' AND PERSON_ID IN ( ",
                    DataUser.COMM, stationcode, stationyear);
                for (int i = 0; i < lists.Length; i++)
                {
                    sql.Append(lists[i]["STAFF_ID"] + " ,");
                }
                sql.Append(")");
                sqlstr = sql.ToString().Replace(",)", ")");
            }
            //进岗
            else if (flag.Equals("0"))
            {
                sql.AppendFormat(@" INSERT INTO {0}.SYS_STATION_PERSONNEL_INFO (STATION_CODE, PERSON_NAME, PERSON_ID, STATION_YEAR) ", DataUser.COMM);
                for (int i = 0; i < lists.Length; i++)
                {
                    sql.AppendFormat(@" SELECT '{0}' AS STATION_CODE,'{1}' AS PERSON_NAME,'{2}' AS PERSON_ID,'{3}' AS STATION_YEAR FROM DUAL UNION ALL ",
                        stationcode, lists[i]["STAFF_NAME"], lists[i]["STAFF_ID"], stationyear);
                }
                sql.Append("END");
                sqlstr = sql.ToString().Replace("UNION ALL END", "");
            }
            OracleOledbBase.ExecuteNonQuery(sqlstr);
            return;
        }


        /// <summary>
        /// 人员进岗
        /// </summary>
        /// <param name="station_year">岗位年度</param>
        public void PersonInStation(string station_year)
        {
            string del = string.Format(@"DELETE FROM {1}.SYS_STATION_PERSONNEL_INFO WHERE STATION_YEAR='{0}'", station_year, DataUser.COMM);
            OracleOledbBase.ExecuteNonQuery(del);

            string sql = string.Format(@"
                INSERT INTO {1}.SYS_STATION_PERSONNEL_INFO
                       (STATION_CODE, PERSON_NAME, PERSON_ID, STATION_YEAR)
                SELECT D.STATION_CODE, A.NAME AS PERSON_NAME, TO_CHAR(A.STAFF_ID) AS PERSON_ID, D.STATION_YEAR
                  FROM {2}.NEW_STAFF_INFO A,
                       {1}.SYS_DEPT_DICT B,
                       {1}.SYS_STATION_BASIC_INFORMATION D
                WHERE DECODE(A.GROUP_ID, NULL,A.DEPT_CODE,A.GROUP_ID ) = B.DEPT_CODE
                  AND B.DEPT_CODE = D.DEPT_CODE
                  AND A.STATION_CODE = D.STATION_DICT_CODE
                  AND A.ADD_MARK = '1'
                  AND D.station_year = '{0}'", station_year, DataUser.COMM, DataUser.RLZY);
            OracleOledbBase.ExecuteNonQuery(sql);


        }



        /// <summary>
        /// 生成岗位评测数据
        /// </summary>
        /// <param name="year">年</param>
        public string GetStationTestKH(string startdate, string enddate, string guideyear, string incount, string station_id)
        {
            string rtn = "";
            OleDbParameter[] parms ={
                new OleDbParameter("startdate",startdate),
                new OleDbParameter("enddate",enddate),
                new OleDbParameter("guideyear",guideyear),
                new OleDbParameter("incount",incount),
                new OleDbParameter("station_id",station_id),
            };
            try
            {
                OracleOledbBase.RunProcedure(string.Format("{0}.KHZB_ASSESS", DataUser.HOSPITALSYS), parms);
            }
            catch (Exception ee)
            {
                rtn = "生成岗位指标测评出错，<BR/>原因：" + ee.Message;
            }
            return rtn;
        }

        /// <summary>
        /// 生成年度实际值
        /// </summary>
        /// <param name="year">年</param>
        public string GetYearTarget(string startdate, string enddate, string guideyear, string incount)
        {
            string rtn = "";
            OleDbParameter[] parms ={
                new OleDbParameter("startdate",startdate),
                new OleDbParameter("enddate",enddate),
                new OleDbParameter("guideyear",guideyear),
                new OleDbParameter("incount",incount)
            };
            try
            {
                OracleOledbBase.RunProcedure(string.Format("{0}.ASSESS_COMPUTE_YEAR", DataUser.HOSPITALSYS), parms);
            }
            catch (Exception ee)
            {
                rtn = "生成年度实际值出错，<BR/>原因：" + ee.Message;
            }
            return rtn;
        }

        /// <summary>
        /// 初始化指标量化数据
        /// </summary>
        /// <param name="Generation">生成数据年度</param>
        /// <param name="Initial">初始化数据年度</param>
        public void Initial(string Generation_year, string Initial_year)
        {
            OleDbParameter[] parms = { };

            MyLists listttrans = new MyLists();

            List dellist = new List();

            dellist.StrSql = string.Format("DELETE FROM {0}.station_guide_information where STATION_YEAR ='{1}'", DataUser.HOSPITALSYS, Initial_year);
            dellist.Parameters = parms;
            listttrans.Add(dellist);

            List ylist = new List();

            string ystr = string.Format(@"INSERT INTO {2}.station_guide_information
            (station_code, guide_code, guide_value, guide_cause, guide_unit,
             increase, increase_arithmetic, decrease, decrease_arithmetic,
             station_year, minusflag)
   SELECT   station.station_code, station.guide_code, 10 AS guide_value,
            NVL (years.guide_fact, 0) AS guide_cause, 'T' AS guide_unit,
            1 AS increase, 1 AS increase_arithmetic, 1 AS decrease,
            1 AS decrease_arithmetic, '{0}' AS station_year,
            '1' AS minusflag
       FROM (SELECT a.station_code, a.station_name, a.dept_code, b.*
               FROM (select * from {1}.SYS_STATION_BASIC_INFORMATION where GUIDE_GATHER_CODE is not null) a,
                    (SELECT a.guide_gather_code, a.guide_gather_name,
                            c.guide_code, c.guide_name, c.ishighguide
                       FROM {2}.guide_gather_class a,
                            {2}.guide_gathers b,
                            {2}.guide_name_dict c
                      WHERE a.guide_gather_code = b.guide_gather_code
                        AND b.guide_code = c.guide_code
                        AND a.organ_class = '01') b
              WHERE a.guide_gather_code = b.guide_gather_code
                    and a.station_year = '{0}') station,
            {2}.assess_result_current_year years
      WHERE station.guide_code = years.guide_code(+)
   ORDER BY station.station_code, station.guide_code", Initial_year, DataUser.COMM, DataUser.HOSPITALSYS);

            ylist.StrSql = ystr;

            ylist.Parameters = parms;
            listttrans.Add(ylist);

            List klist = new List();
            string kstr = string.Format(@"INSERT INTO {2}.station_guide_information
            (station_code, guide_code, guide_value, guide_cause, guide_unit,
             increase, increase_arithmetic, decrease, decrease_arithmetic,
             station_year, minusflag)
   SELECT   station.station_code, station.guide_code, 10 AS guide_value,
            NVL (years.guide_fact, 0) AS guide_cause, 'T' AS guide_unit,
            1 AS increase, 1 AS increase_arithmetic, 1 AS decrease,
            1 AS decrease_arithmetic, '{0}' AS station_year,
            '1' AS minusflag
       FROM (SELECT a.station_code, a.station_name, a.dept_code, b.*
               FROM (select * from {1}.SYS_STATION_BASIC_INFORMATION where GUIDE_GATHER_CODE is not null) a,
                    (SELECT a.guide_gather_code, a.guide_gather_name,
                            c.guide_code, c.guide_name, c.ishighguide
                       FROM {2}.guide_gather_class a,
                            {2}.guide_gathers b,
                            {2}.guide_name_dict c
                      WHERE a.guide_gather_code = b.guide_gather_code
                        AND b.guide_code = c.guide_code
                        AND a.organ_class = '02') b
              WHERE a.guide_gather_code = b.guide_gather_code
                    and a.station_year = '{0}') station,
            {2}.assess_result_current_year years
      WHERE station.dept_code = years.dept_code(+)
            AND station.guide_code = years.guide_code(+)
   ORDER BY station.station_code, station.guide_code", Initial_year, DataUser.COMM, DataUser.HOSPITALSYS);

            klist.StrSql = kstr;
            klist.Parameters = parms;
            listttrans.Add(klist);

            List rlist = new List();

            string rstr = string.Format(@"INSERT INTO {2}.station_guide_information
            (station_code, guide_code, guide_value, guide_cause, guide_unit,
             increase, increase_arithmetic, decrease, decrease_arithmetic,
             station_year, minusflag)
   SELECT   station.station_code, station.guide_code, 10 AS guide_value,
            NVL (years.guide_fact, 0) AS guide_cause, 'T' AS guide_unit,
            1 AS increase, 1 AS increase_arithmetic, 1 AS decrease,
            1 AS decrease_arithmetic, '{0}' AS station_year,
            '1' AS minusflag
       FROM (SELECT a.station_code, a.station_name, a.dept_code, b.*
               FROM (select * from {1}.SYS_STATION_BASIC_INFORMATION where GUIDE_GATHER_CODE is not null) a,
                    (SELECT a.guide_gather_code, a.guide_gather_name,
                            c.guide_code, c.guide_name, c.ishighguide
                       FROM {2}.guide_gather_class a,
                            {2}.guide_gathers b,
                            {2}.guide_name_dict c
                      WHERE a.guide_gather_code = b.guide_gather_code
                        AND b.guide_code = c.guide_code
                        AND a.organ_class = '03') b
              WHERE a.guide_gather_code = b.guide_gather_code
                    and a.station_year = '{0}') station,
            (SELECT   a.guide_code, c.dept_code,
                      CASE
                         WHEN d.ishighguide = '0'
                            THEN MIN (a.guide_fact)
                         ELSE MAX (a.guide_fact)
                      END AS guide_fact
                 FROM {2}.assess_result_current_year a,
                      {3}.new_staff_info b,
                      {1}.sys_dept_dict c,
                      {2}.guide_name_dict d
                WHERE a.dept_code = to_char(b.staff_id)
                  AND a.guide_code LIKE '3%'
                  AND b.dept_code = c.dept_code
                  AND a.guide_code = d.guide_code
             GROUP BY a.guide_code, c.dept_code, d.ishighguide) years
      WHERE station.dept_code = years.dept_code(+)
            AND station.guide_code = years.guide_code(+)
   ORDER BY station.station_code, station.guide_code", Initial_year, DataUser.COMM, DataUser.HOSPITALSYS, DataUser.RLZY);

            rlist.StrSql = rstr;
            rlist.Parameters = parms;

            listttrans.Add(rlist);
            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 初始化科室指标量化数据
        /// </summary>
        /// <param name="Generation">生成数据年度</param>
        /// <param name="Initial">初始化数据年度</param>
        public void deptInitial(string Generation_year, string Initial_year)
        {
            OleDbParameter[] parms = { };

            MyLists listttrans = new MyLists();

            List dellist = new List();

            dellist.StrSql = string.Format("DELETE FROM {0}.DEPT_GUIDE_INFORMATION where STATION_YEAR ='{1}'", DataUser.HOSPITALSYS, Initial_year);
            dellist.Parameters = parms;
            listttrans.Add(dellist);

            List ylist = new List();

            string ystr = string.Format(@"INSERT INTO {2}.DEPT_GUIDE_INFORMATION
                                            (dept_code, guide_code, guide_value, guide_cause, guide_unit,
                                             increase, increase_arithmetic, decrease, decrease_arithmetic,
                                             station_year, minusflag)
                                   SELECT   station.dept_code, station.guide_code, 10 AS guide_value,
                                            NVL (years.guide_fact, 0) AS guide_cause, 'T' AS guide_unit,
                                            1 AS increase, 1 AS increase_arithmetic, 1 AS decrease,
                                            1 AS decrease_arithmetic, '{0}' AS station_year,
                                            '1' AS minusflag
                                       FROM (SELECT a.dept_code, b.*
                                               FROM {2}.DEPT_GATHERS a,
                                                    (SELECT a.guide_gather_code, a.guide_gather_name,
                                                            c.guide_code, c.guide_name, c.ishighguide
                                                       FROM {2}.guide_gather_class a,
                                                            {2}.guide_gathers b,
                                                            {2}.guide_name_dict c
                                                      WHERE a.guide_gather_code = b.guide_gather_code
                                                        AND b.guide_code = c.guide_code
                                                        AND a.organ_class = '02') b
                                              WHERE a.gather_code = b.guide_gather_code
                                                    and a.station_year = '{0}') station,
                                            {2}.assess_result_current_year years
                                      WHERE station.guide_code = years.guide_code(+)
                                      and   station.dept_code = years.dept_code(+)
                                   ORDER BY station.dept_code, station.guide_code", Initial_year, DataUser.COMM, DataUser.HOSPITALSYS);

            ylist.StrSql = ystr;

            ylist.Parameters = parms;
            listttrans.Add(ylist);


            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        public void deptUpdate(string Generation_year, string Initial_year)
        {
            OleDbParameter[] parms = { };

            MyLists listttrans = new MyLists();

            List dellist = new List();

            dellist.StrSql = string.Format("DELETE FROM {0}.DEPT_GUIDE_INFORMATION where STATION_YEAR ='{1}'", DataUser.HOSPITALSYS, Generation_year);
            dellist.Parameters = parms;
            listttrans.Add(dellist);

            List ylist = new List();

            string ystr = string.Format(@"INSERT INTO {0}.DEPT_GUIDE_INFORMATION (dept_code,
                                                guide_code,
                                                guide_value,
                                                guide_cause,
                                                guide_unit,
                                                increase,
                                                increase_arithmetic,
                                                decrease,
                                                decrease_arithmetic,
                                                station_year,
                                                minusflag,
                                                plusflag,
                                                plus_increase,
                                                plus_arithmetic,
                                                MINUS_INCREASE,
                                                MINUS_ARITHMETIC,
                                                THRESHOLD_VALUE,
                                                FIXNUM,
                                                PLUS_LIMIT,
                                                MINUS_LIMIT,
                                                GUIDE_CAUSE1,
                                                GUIDE_CAUSE2,
                                                GUIDE_CAUSE3,
                                                GUIDE_CAUSE4,
                                                GUIDE_CAUSE5,
                                                GUIDE_CAUSE6)
   SELECT dept_code,
          guide_code,
          guide_value,
          guide_cause,
          guide_unit,
          increase,
          increase_arithmetic,
          decrease,
          decrease_arithmetic,
          '{1}' station_year,
          minusflag,
          plusflag,
          plus_increase,
          plus_arithmetic,
          MINUS_INCREASE,
          MINUS_ARITHMETIC,
          THRESHOLD_VALUE,
          FIXNUM,
          PLUS_LIMIT,
          MINUS_LIMIT,
          GUIDE_CAUSE1,
          GUIDE_CAUSE2,
          GUIDE_CAUSE3,
          GUIDE_CAUSE4,
          GUIDE_CAUSE5,
          GUIDE_CAUSE6
     FROM {0}.DEPT_GUIDE_INFORMATION where station_year='{2}'", DataUser.HOSPITALSYS, Generation_year, Initial_year);

            ylist.StrSql = ystr;

            ylist.Parameters = parms;
            listttrans.Add(ylist);


            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="station_year"></param>
        public void UpdateDeptGuideValue(string station_year)
        {
            string viewName = DataUser.HOSPITALSYS + ".V_DEPTGUIDE_INIT_VIEW";
            OleDbParameter[] parms ={
                new OleDbParameter("LIST",viewName)
            };
            OracleOledbBase.RunProcedure("dbms_mview.refresh", parms);
            string sql = string.Format(@"
                                   UPDATE  {0}.DEPT_GUIDE_INFORMATION  B
                                      SET B.GUIDE_VALUE = ( SELECT NVL(MAX(A.GUIDE_VALUE),10) FROM 
                                        {0}.V_DEPTGUIDE_INIT_VIEW A  
                                           WHERE A.DEPT_CODE = B.DEPT_CODE 
                                             AND A.GUIDE_CODE =   B.GUIDE_CODE 
                                             AND A.STATION_YEAR = B.STATION_YEAR
                                     )
                                     WHERE B.STATION_YEAR = '{1}' ", DataUser.HOSPITALSYS, station_year);

            OracleOledbBase.ExecuteNonQuery(sql);
        }

        public void UpdateDpetGathers(string station_year, string target_year,string user,string input_time)
        {
            OleDbParameter[] parms = { };

            MyLists listttrans = new MyLists();

            List dellist = new List();

            dellist.StrSql = string.Format("DELETE FROM {0}.DEPT_GATHERS where STATION_YEAR ='{1}'", DataUser.HOSPITALSYS, station_year);
            dellist.Parameters = parms;
            listttrans.Add(dellist);

            List sqlList = new List();
            string sql = string.Format(@"
                                   INSERT INTO {0}.DEPT_GATHERS ( dept_code, gather_code, STATION_BSC_CLASS_01, STATION_BSC_CLASS_02, 
STATION_BSC_CLASS_03, STATION_BSC_CLASS_04, station_year, input_user, input_time ) SELECT
dept_code,
gather_code,
STATION_BSC_CLASS_01,
STATION_BSC_CLASS_02,
STATION_BSC_CLASS_03,
STATION_BSC_CLASS_04,
'{1}' AS station_year,
'{3}' input_user,
SYSDATE input_time 
FROM
	HOSPITALSYS.DEPT_GATHERS 
WHERE
	station_year = '{2}' ", DataUser.HOSPITALSYS, station_year, target_year, user, input_time);

            sqlList.StrSql = sql;

            sqlList.Parameters = parms;
            listttrans.Add(sqlList);


            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 更新指标分值
        /// </summary>
        /// <param name="station_year">岗位年度</param>
        public void UpdateGuideValue(string station_year)
        {

            #region ///===============旧的方法实现更新指标分值=======================///

            //            //获取岗位信息
            //            string sql = string.Format(@"SELECT station_code,STATION_YEAR, station_bsc_class_01, station_bsc_class_02,
            //                                               station_bsc_class_03, station_bsc_class_04
            //                                          FROM {1}.SYS_STATION_BASIC_INFORMATION
            //                                         WHERE station_year = '{0}'", station_year,DataUser.COMM);
            //            DataTable stationInfo = OracleOledbBase.ExecuteDataSet("station", sql).Tables[0];

            //            for (int i = 0; i < stationInfo.Rows.Count; i++)
            //            {
            //                //通过岗位编码找到指标代码用于计算
            //                string guide_sql = string.Format(@"SELECT a.station_code, a.guide_code, a.guide_value, c.bsc_class_name,
            //                                                       SUBSTR (b.bsc, 1, 2) AS bsc
            //                                                  FROM {1}.station_guide_information a,
            //                                                       {1}.guide_name_dict b,
            //                                                       {1}.jxgl_guide_bsc_class_dict c
            //                                                 WHERE a.guide_code = b.guide_code
            //                                                   AND SUBSTR (b.bsc, 1, 2) = c.bsc_class_code
            //                                                   AND a.station_code = '{0}'", stationInfo.Rows[i]["station_code"].ToString(),DataUser.HOSPITALSYS);
            //                DataTable guide = OracleOledbBase.ExecuteDataSet("guide", guide_sql).Tables[0];

            //                //指标属于内部管理的
            //                DataRow[] dr01 = guide.Select("bsc = '01'");
            //                //指标属于经济财务的
            //                DataRow[] dr02 = guide.Select("bsc = '02'");
            //                //指标属于客户满意度的
            //                DataRow[] dr03 = guide.Select("bsc = '03'");
            //                //指标属于学习与成长的
            //                DataRow[] dr04 = guide.Select("bsc = '04'");
            //                //平均数
            //                int lastnum = 0;
            //                //最后一个补数
            //                int firstnum = 0;

            //                if (dr01.Length > 0)
            //                {
            //                    //平均数
            //                    lastnum = Convert.ToInt32(Math.Round((Convert.ToDecimal(stationInfo.Rows[i]["station_bsc_class_01"]) / dr01.Length)));
            //                    //最后一个补数
            //                    firstnum = Convert.ToInt32(stationInfo.Rows[i]["station_bsc_class_01"]) - (dr01.Length - 1) * lastnum;
            //                    //用于补数的指标编码
            //                    string firstStr01 = dr01[0]["guide_code"].ToString();
            //                    //用于平均数的指标编码
            //                    string lastStr01 = string.Empty;
            //                    StringBuilder str01 = new StringBuilder();

            //                    for (int i01 = 1; i01 < dr01.Length; i01++)
            //                    {
            //                        str01.Append("'" + dr01[i01]["guide_code"].ToString() + "',");
            //                    }

            //                    if (str01.Length > 0)
            //                    {
            //                        lastStr01 = str01.ToString().Substring(0, str01.ToString().LastIndexOf(","));
            //                        //更新平均数的分值
            //                        string updatelast = string.Format(@"update {4}.STATION_GUIDE_INFORMATION 
            //                                                         set GUIDE_VALUE={3}
            //                                                         where STATION_CODE='{0}' and STATION_YEAR='{1}' and GUIDE_CODE in ({2})",
            //                                                             stationInfo.Rows[i]["STATION_CODE"].ToString(),
            //                                                             stationInfo.Rows[i]["STATION_YEAR"].ToString(),
            //                                                             lastStr01, lastnum,DataUser.HOSPITALSYS);
            //                        OracleOledbBase.ExecuteNonQuery( updatelast, new OleDbParameter[] { });
            //                    }

            //                    if (firstStr01.Length > 0)
            //                    {
            //                        //更新补数的分值
            //                        string updatefirst = string.Format(@"update {4}.STATION_GUIDE_INFORMATION 
            //                                                         set GUIDE_VALUE={3}
            //                                                         where STATION_CODE='{0}' and STATION_YEAR='{1}' and GUIDE_CODE='{2}'",
            //                                                              stationInfo.Rows[i]["STATION_CODE"].ToString(),
            //                                                               stationInfo.Rows[i]["STATION_YEAR"].ToString(),
            //                                                               firstStr01, firstnum, DataUser.HOSPITALSYS);
            //                        OracleOledbBase.ExecuteNonQuery( updatefirst, new OleDbParameter[] { });
            //                    }

            //                }


            //                if (dr02.Length > 0)
            //                {
            //                    //平均数
            //                    lastnum = Convert.ToInt32(Math.Round((Convert.ToDecimal(stationInfo.Rows[i]["station_bsc_class_02"]) / dr02.Length)));
            //                    //最后一个补数
            //                    firstnum = Convert.ToInt32(stationInfo.Rows[i]["station_bsc_class_02"]) - (dr02.Length - 1) * lastnum;
            //                    //用于补数的指标编码
            //                    string firstStr01 = dr02[0]["guide_code"].ToString();
            //                    //用于平均数的指标编码
            //                    string lastStr01 = string.Empty;
            //                    StringBuilder str01 = new StringBuilder();

            //                    for (int i01 = 1; i01 < dr02.Length; i01++)
            //                    {
            //                        str01.Append("'" + dr02[i01]["guide_code"].ToString() + "',");
            //                    }
            //                    if (str01.Length > 0)
            //                    {
            //                        lastStr01 = str01.ToString().Substring(0, str01.ToString().LastIndexOf(","));
            //                        //更新平均数的分值
            //                        string updatelast = string.Format(@"update {4}.STATION_GUIDE_INFORMATION 
            //                                                         set GUIDE_VALUE={3}
            //                                                         where STATION_CODE='{0}' and STATION_YEAR='{1}' and GUIDE_CODE in ({2})",
            //                                                             stationInfo.Rows[i]["STATION_CODE"].ToString(),
            //                                                             stationInfo.Rows[i]["STATION_YEAR"].ToString(),
            //                                                             lastStr01, lastnum,DataUser.HOSPITALSYS);
            //                        OracleOledbBase.ExecuteNonQuery( updatelast, new OleDbParameter[] { });
            //                    }

            //                    if (firstStr01.Length > 0)
            //                    {
            //                        //更新补数的分值
            //                        string updatefirst = string.Format(@"update {4}.STATION_GUIDE_INFORMATION 
            //                                                         set GUIDE_VALUE={3}
            //                                                         where STATION_CODE='{0}' and STATION_YEAR='{1}' and GUIDE_CODE='{2}'",
            //                                                              stationInfo.Rows[i]["STATION_CODE"].ToString(),
            //                                                               stationInfo.Rows[i]["STATION_YEAR"].ToString(),
            //                                                               firstStr01, firstnum, DataUser.HOSPITALSYS);
            //                        OracleOledbBase.ExecuteNonQuery( updatefirst, new OleDbParameter[] { });
            //                    }


            //                }


            //                if (dr03.Length > 0)
            //                {
            //                    //平均数
            //                    lastnum = Convert.ToInt32(Math.Round((Convert.ToDecimal(stationInfo.Rows[i]["station_bsc_class_03"]) / dr03.Length)));
            //                    //最后一个补数
            //                    firstnum = Convert.ToInt32(stationInfo.Rows[i]["station_bsc_class_03"]) - (dr03.Length - 1) * lastnum;
            //                    //用于补数的指标编码
            //                    string firstStr01 = dr03[0]["guide_code"].ToString();
            //                    //用于平均数的指标编码
            //                    string lastStr01 = string.Empty;
            //                    StringBuilder str01 = new StringBuilder();

            //                    for (int i01 = 1; i01 < dr03.Length; i01++)
            //                    {
            //                        str01.Append("'" + dr03[i01]["guide_code"].ToString() + "',");
            //                    }
            //                    if (str01.Length > 0)
            //                    {
            //                        lastStr01 = str01.ToString().Substring(0, str01.ToString().LastIndexOf(","));

            //                        //更新平均数的分值
            //                        string updatelast = string.Format(@"update {4}.STATION_GUIDE_INFORMATION 
            //                                                         set GUIDE_VALUE={3}
            //                                                         where STATION_CODE='{0}' and STATION_YEAR='{1}' and GUIDE_CODE in ({2})",
            //                                                             stationInfo.Rows[i]["STATION_CODE"].ToString(),
            //                                                             stationInfo.Rows[i]["STATION_YEAR"].ToString(),
            //                                                             lastStr01, lastnum, DataUser.HOSPITALSYS);
            //                        OracleOledbBase.ExecuteNonQuery( updatelast, new OleDbParameter[] { });
            //                    }

            //                    if (firstStr01.Length > 0)
            //                    {
            //                        //更新补数的分值
            //                        string updatefirst = string.Format(@"update {4}.STATION_GUIDE_INFORMATION 
            //                                                         set GUIDE_VALUE={3}
            //                                                         where STATION_CODE='{0}' and STATION_YEAR='{1}' and GUIDE_CODE='{2}'",
            //                                                              stationInfo.Rows[i]["STATION_CODE"].ToString(),
            //                                                               stationInfo.Rows[i]["STATION_YEAR"].ToString(),
            //                                                               firstStr01, firstnum, DataUser.HOSPITALSYS);
            //                        OracleOledbBase.ExecuteNonQuery( updatefirst, new OleDbParameter[] { });
            //                    }


            //                }


            //                if (dr04.Length > 0)
            //                {
            //                    //平均数
            //                    lastnum = Convert.ToInt32(Math.Round((Convert.ToDecimal(stationInfo.Rows[i]["station_bsc_class_04"]) / dr04.Length)));
            //                    //最后一个补数
            //                    firstnum = Convert.ToInt32(stationInfo.Rows[i]["station_bsc_class_04"]) - (dr04.Length - 1) * lastnum;
            //                    //用于补数的指标编码
            //                    string firstStr01 = dr04[0]["guide_code"].ToString();
            //                    //用于平均数的指标编码
            //                    string lastStr01 = string.Empty;
            //                    StringBuilder str01 = new StringBuilder();

            //                    for (int i01 = 1; i01 < dr04.Length; i01++)
            //                    {
            //                        str01.Append("'" + dr04[i01]["guide_code"].ToString() + "',");
            //                    }
            //                    if (str01.Length > 0)
            //                    {
            //                        lastStr01 = str01.ToString().Substring(0, str01.ToString().LastIndexOf(","));
            //                        //更新平均数的分值
            //                        string updatelast = string.Format(@"update {4}.STATION_GUIDE_INFORMATION 
            //                                                         set GUIDE_VALUE={3}
            //                                                         where STATION_CODE='{0}' and STATION_YEAR='{1}' and GUIDE_CODE in ({2})",
            //                                                             stationInfo.Rows[i]["STATION_CODE"].ToString(),
            //                                                             stationInfo.Rows[i]["STATION_YEAR"].ToString(),
            //                                                             lastStr01, lastnum, DataUser.HOSPITALSYS);
            //                        OracleOledbBase.ExecuteNonQuery( updatelast, new OleDbParameter[] { });
            //                    }
            //                    if (firstStr01.Length > 0)
            //                    {
            //                        //更新补数的分值
            //                        string updatefirst = string.Format(@"update {4}.STATION_GUIDE_INFORMATION 
            //                                                         set GUIDE_VALUE={3}
            //                                                         where STATION_CODE='{0}' and STATION_YEAR='{1}' and GUIDE_CODE='{2}'",
            //                                                              stationInfo.Rows[i]["STATION_CODE"].ToString(),
            //                                                               stationInfo.Rows[i]["STATION_YEAR"].ToString(),
            //                                                               firstStr01, firstnum, DataUser.HOSPITALSYS);
            //                        OracleOledbBase.ExecuteNonQuery( updatefirst, new OleDbParameter[] { });
            //                    }
            //                }
            //            }

            #endregion

            #region  新的方法更新指标分值

            string viewName = DataUser.HOSPITALSYS + ".V_STATIONGUIDE_INIT_VIEW";
            OleDbParameter[] parms ={
                new OleDbParameter("LIST",viewName)
            };
            OracleOledbBase.RunProcedure("dbms_mview.refresh", parms);

            string sql = string.Format(@"
               UPDATE  {0}.STATION_GUIDE_INFORMATION  B
                  SET B.GUIDE_VALUE = ( SELECT NVL(MAX(A.GUIDE_VALUE),10) FROM 
                    {0}.V_STATIONGUIDE_INIT_VIEW A  
                       WHERE A.STATION_CODE = B.STATION_CODE 
                         AND A.GUIDE_CODE =   B.GUIDE_CODE 
                         AND A.STATION_YEAR = B.STATION_YEAR
                 )
                 WHERE B.STATION_YEAR = '{1}' ", DataUser.HOSPITALSYS, station_year);

            OracleOledbBase.ExecuteNonQuery(sql);


            #endregion


        }

        #endregion  成员方法
    }
}

