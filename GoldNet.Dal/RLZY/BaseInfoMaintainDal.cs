using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;

namespace Goldnet.Dal
{
    public class BaseInfoMaintainDal
    {
        public BaseInfoMaintainDal()
        { }

        /// <summary>
        /// 科室信息统计
        /// </summary>
        /// <param name="DeptCode">选择科室代码</param>
        /// <param name="power">权限</param>
        /// <returns></returns>
        public DataSet ViewDeptBaseInfoStat(string DeptCode, string power)
        {
            StringBuilder str = new StringBuilder();
            string selectDeptStr = DeptCode == "" ? "" : "AND DEPT_INFO.DEPT_CODE = '" + DeptCode + "'";
            string Power = power == "" ? "" : "AND DEPT_INFO.DEPT_CODE in (" + power + ")";
            str.AppendFormat(@"SELECT 
                                DEPT_INFO.DEPT_CODE DEPT_CODE,
                                DEPT_INFO.DEPT_NAME DEPT_NAME,
                                DEPT_INFO.WEAVE_BED WEAVE_BED, 
                                DEPT_INFO.DEPLOY_BED DEPLOY_BED, 
                                DEPT_ATTRIBUTE_DICT.ATTRIBUE ATTRIBUE, 
                                DEPT_INFO.IS_PIVOT_DEPT IS_PIVOT_DEPT, 
                                DEPT_INFO.SUBDIRECOTR SUBDIRECOTR, 
                                DEPT_INFO.CHARGE_NURSE CHARGE_NURSE, 
                                DEPT_INFO.DIRECTOR DIRECTOR, 
                                DEPT_INFO.PRINCIPAL PRINCIPAL,
                                PERS_SPEC_SORT_DICT.SPEC_SORT_NAME SPEC_SORT_NAME, 
                                DEPT_SORT_DICT.SORT_NAME SORT_NAME,
                                SPEC_CENTER_DICT.CENTER_NAME CENTER_NAME
                              FROM  {0}.DEPT_INFO,{0}.DEPT_ATTRIBUTE_DICT,{0}.PERS_SPEC_SORT_DICT,{0}.DEPT_SORT_DICT, {0}.SPEC_CENTER_DICT,{1}.SYS_DEPT_DICT
                              WHERE DEPT_INFO.DEPT_NAME<>'1' AND  DEPT_INFO.SPES_SORT_ID = PERS_SPEC_SORT_DICT.SERIAL_NO(+)
                                AND DEPT_INFO.CENTER_SORT_ID = DEPT_SORT_DICT.SEID(+)
                                AND DEPT_INFO.CENTER_ID =SPEC_CENTER_DICT.CENTER_CODE(+)
                                AND DEPT_ATTRIBUTE_DICT.ID(+) = DEPT_INFO.DEPT_ATTRIBUTE
                                AND DEPT_INFO.DEPT_CODE=SYS_DEPT_DICT.DEPT_CODE
                                AND COMM.SYS_DEPT_DICT.ATTR='是' {2} {3}", DataUser.RLZY, DataUser.COMM, selectDeptStr, Power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取核算科室
        /// </summary>
        /// <param name="inputcode">部门输入编码</param>
        /// <returns>部门信息</returns>
        public DataSet GetDeptInfo(string inputcode, string power)
        {
            string Power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            string strSql = string.Format("select DISTINCT A.DEPT_CODE, a.dept_name,a.input_code from {0}.SYS_DEPT_DICT a WHERE attr='是' and a.input_code like ? {1} and show_flag = '0' order by a.DEPT_CODE", DataUser.COMM, Power);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql, cmdPara);
        }

        /// <summary>
        /// 获取全部科室
        /// </summary>
        /// <param name="inputcode"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public DataSet GetDeptDict(string inputcode, string power)
        {
            string Power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            string strSql = string.Format("SELECT DISTINCT A.dept_code, a.dept_name,a.input_code from comm.SYS_DEPT_DICT a WHERE a.input_code like ? {1} order by a.DEPT_CODE", "", Power);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql, cmdPara);
        }

        /// <summary>
        /// 获取核算科室
        /// </summary>
        /// <param name="inputcode"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public DataSet GetDeptAccont(string inputcode, string power)
        {
            string Power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            string strSql = string.Format("SELECT DISTINCT A.dept_code, a.dept_name,a.input_code from comm.SYS_DEPT_DICT a WHERE attr='是' and a.input_code like ? {1} order by a.DEPT_CODE", "", Power);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql, cmdPara);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputcode"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public DataSet GetGuideInfo(string inputcode, string power)
        {
            string strSql = string.Format("SELECT DISTINCT GUIDE_CODE,GUIDE_NAME FROM {0}.GUIDE_NAME_DICT WHERE GUIDE_NAME like ? ORDER BY GUIDE_CODE", DataUser.HOSPITALSYS);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql, cmdPara);
        }


        /// <summary>
        /// 民族
        /// </summary>
        /// <param name="inputcode">民族输入编码</param>
        /// <returns>民族信息</returns>
        public DataSet GetNationInfo(string inputcode)
        {
            string strSql = string.Format("SELECT SERIAL_NO,NATION_CODE,NATION_NAME FROM {0}.NATION_DICT WHERE  INPUT_CODE like ? order by NATION_NAME", DataUser.RLZY);
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
        /// USERID 
        /// </summary>
        /// <param name="inputcode">user输入编码</param>
        /// <returns>民族信息</returns>
        public DataSet GetUserInfo(string inputcode, string staffid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select USER_ID,NAME USER_NAME,INPUT_CODE DB_USER,DEPT_NAME from rlzy.NEW_STAFF_INFO where staff_id='{0}'", staffid);
            str.AppendFormat(@" union all SELECT T1.USER_ID,T1.USER_NAME,T1.DB_USER,
                                            (SELECT max(DEPT_NAME) FROM {0}.DEPT_DICT T2 WHERE T2.DEPT_CODE = T1.USER_DEPT) DEPT_NAME 
                                            FROM {0}.USERS T1 
                                            WHERE T1.DB_USER LIKE ? ", DataUser.HISFACT);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(str.ToString(), cmdPara);
        }



        /// <summary>
        /// 人员基本信息查询
        /// </summary>
        /// <param name="DeptCode">科室代码</param>
        /// <param name="persontype">人员类别</param>
        /// <param name="power">权限</param>
        /// <param name="isArmy">0:非军队人员,1:军队人员</param>
        /// <returns></returns>
        public DataSet ViewStaffBaseInfo(string DeptCode, string persontype, string power, string isArmy)
        {
            string selectDeptStr = DeptCode == "" ? "" : " AND DEPT_CODE = '" + DeptCode + "'";
            string Power = power == "" ? "" : " AND DEPT_CODE IN (" + power + ")";
            string PersonType = persontype == "" ? "" : " AND STAFFSORT IN (" + persontype + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   STAFF_ID,NAME, JOB, DUTY,DEPT_NAME, SEX, BIRTHDAY, STAFFSORT, DUTYDATE,
                                         WORKDATE, JOBDATE,TOPEDUCATE
                               FROM {0}.NEW_STAFF_INFO
                               WHERE IF_ARMY = '{4}' AND ADD_MARK = '1' and ISONGUARD='是' {1} {2} {3}
                               ORDER BY NAME", DataUser.RLZY, selectDeptStr, PersonType, Power, isArmy);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询信息化建设
        /// </summary>
        /// <param name="timelist">时间关系</param>
        /// <returns></returns>
        public DataSet ViewInformation(string timelist)
        {
            string Timelist = timelist == "" ? "" : " WHERE SUBSTR (STAT_MONTH, 1, 4)" + timelist + "";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   ID, SUBSTR (STAT_MONTH, 1, 4) AS YEARS, STAT_MONTH, OPEN_DATE,
                                     APP_SUBSYS_NUM, NET_NUM, NET_COMP_NUM, SERVER_NUM, INVEST_TOTAL,
                                     HIS_TECH_PERS, PLANET_MEDI_CASE, PLANET_LONG_CASE, PLANET_LONG_PERS
                                FROM {0}.INFORMATION
                                {1}
                                ORDER BY STAT_MONTH DESC", DataUser.RLZY, Timelist);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 添加信息化建设信息
        /// </summary>
        /// <param name="statMonth">统计年月</param>
        /// <param name="openDate">HIS正式启用时间</param>
        /// <param name="appSubsysNum">应用子系统数</param>
        /// <param name="netNum">布网点数</param>
        /// <param name="netCompNum">上网微机数</param>
        /// <param name="serverNum">服务器台数</param>
        /// <param name="investTotal">投资总额</param>
        /// <param name="HIStechPers">HIS技术人员数</param>
        /// <param name="planetMediCase">卫星网医疗会诊例数</param>
        /// <param name="planetLongCase">卫星网远程教学例次</param>
        /// <param name="planetLongPpers">卫星网远程教学参加人次</param>
        public void InsertInformation(string statMonth, string openDate, string appSubsysNum, string netNum, string netCompNum, string serverNum, string investTotal, string HIStechPers, string planetMediCase, string planetLongCase, string planetLongPpers)
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.INFORMATION(ID,STAT_MONTH,OPEN_DATE,APP_SUBSYS_NUM,NET_NUM,NET_COMP_NUM,
                                               SERVER_NUM,INVEST_TOTAL,HIS_TECH_PERS,PLANET_MEDI_CASE,PLANET_LONG_CASE,PLANET_LONG_PERS)
                                               VALUES(?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.RLZY);

            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , OracleOledbBase.GetMaxID("ID","RLZY.INFORMATION")) ,
					new OleDbParameter( "" , statMonth) ,
					new OleDbParameter( "" , openDate) ,
					new OleDbParameter( "" , appSubsysNum) ,
					new OleDbParameter( "" , netNum) ,
					new OleDbParameter( "" , netCompNum) ,
					new OleDbParameter( "" , serverNum) ,
					new OleDbParameter( "" , investTotal) ,
					new OleDbParameter( "" , HIStechPers) ,
					new OleDbParameter( "" , planetMediCase) ,
					new OleDbParameter( "" , planetLongCase) ,
					new OleDbParameter( "" , planetLongPpers) 
				};
            OracleOledbBase.ExecuteNonQuery("DELETE RLZY.INFORMATION WHERE STAT_MONTH=?", new OleDbParameter("", statMonth));
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 更新信息化建设信息
        /// </summary>
        /// <param name="statMonth">统计年月</param>
        /// <param name="openDate">HIS正式启用时间</param>
        /// <param name="appSubsysNum">应用子系统数</param>
        /// <param name="netNum">布网点数</param>
        /// <param name="netCompNum">上网微机数</param>
        /// <param name="serverNum">服务器台数</param>
        /// <param name="investTotal">投资总额</param>
        /// <param name="HIStechPers">HIS技术人员数</param>
        /// <param name="planetMediCase">卫星网医疗会诊例数</param>
        /// <param name="planetLongCase">卫星网远程教学例次</param>
        /// <param name="planetLongPpers">卫星网远程教学参加人次</param>
        public void UpdateInformation(string id, string openDate, string appSubsysNum, string netNum, string netCompNum, string serverNum, string investTotal, string HIStechPers, string planetMediCase, string planetLongCase, string planetLongPpers)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE {0}.INFORMATION
                                       SET OPEN_DATE = ?,
                                           APP_SUBSYS_NUM = ?,
                                           NET_NUM = ?,
                                           NET_COMP_NUM = ?,
                                           SERVER_NUM = ?,
                                           INVEST_TOTAL = ?,
                                           HIS_TECH_PERS = ?,
                                           PLANET_MEDI_CASE = ?,
                                           PLANET_LONG_CASE = ?,
                                           PLANET_LONG_PERS = ?
                                     WHERE ID = ?", DataUser.RLZY);

            OleDbParameter[] cmdPara = new OleDbParameter[]
				{	
				    
					new OleDbParameter( "" , openDate) ,
					new OleDbParameter( "" , appSubsysNum) ,
					new OleDbParameter( "" , netNum) ,
					new OleDbParameter( "" , netCompNum) ,
					new OleDbParameter( "" , serverNum) ,
					new OleDbParameter( "" , investTotal) ,
					new OleDbParameter( "" , HIStechPers) ,
					new OleDbParameter( "" , planetMediCase) ,
					new OleDbParameter( "" , planetLongCase) ,
					new OleDbParameter( "" , planetLongPpers),
				    new OleDbParameter( "" , id)
				   
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 删除指定序号的信息化建设
        /// </summary>
        /// <param name="id">序号</param>
        public void DelInformation(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE FROM  {0}.INFORMATION WHERE ID=?", DataUser.RLZY);
            OracleOledbBase.ExecuteNonQuery(str.ToString(), new OleDbParameter("", id));
        }

        /// <summary>
        /// 添加科室信息
        /// </summary>
        /// <param name="deptCode">科室编号</param>
        public void InsertDeptInfo(String[] updateData)
        {

            DelDeptInfoByCode(updateData[24]);
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO {0}.DEPT_INFO (DEPT_CODE,DIRECTOR,WEAVE_BED,CHARGE_NURSE,SHOULE_NUM,
                                                SHOULD_NUM,APPRO_MANAGER,SUBDIRECOTR,DEPLOY_BED,SPES_SORT_ID,
                                                OUTP_OR_INP,APPRO_DOCTOR,APPRO_TECH,CENTER_ID,DEPT_ATTRIBUTE,PRINCIPAL,
                                                INTERNAL_OR_SERGERY,APPRO_DRUG,APPRO_PROJECT,CENTER_SORT_ID,
                                                IS_PIVOT_DEPT,DEPT_ATTR,APPRO_NUM,APPRO_NURSE,APPRO_OTHER,DEPT_NAME)
					                            VALUES(
                                                '{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',
                                                '{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}',
                                                '{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}')", DataUser.RLZY, updateData[24],
                                                                                                                           updateData[0],
                                                                                                                           updateData[1],
                                                                                                                           updateData[2],
                                                                                                                           updateData[3],
                                                                                                                           updateData[4],
                                                                                                                           updateData[5],
                                                                                                                           updateData[6],
                                                                                                                           updateData[7],
                                                                                                                           updateData[8],
                                                                                                                           updateData[9],
                                                                                                                           updateData[10],
                                                                                                                           updateData[11],
                                                                                                                           updateData[12],
                                                                                                                           updateData[13],
                                                                                                                           updateData[14],
                                                                                                                           updateData[15],
                                                                                                                           updateData[16],
                                                                                                                           updateData[17],
                                                                                                                           updateData[18],
                                                                                                                           updateData[19],
                                                                                                                           updateData[20],
                                                                                                                           updateData[21],
                                                                                                                           updateData[22],
                                                                                                                           updateData[23], updateData[25]);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 查询科室信息
        /// </summary>
        /// <param name="code">科室CODE</param>
        /// <returns></returns>
        public DataSet getDeptInfoByDeptCode(string code)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT DEPT_CODE,DIRECTOR,WEAVE_BED,CHARGE_NURSE,SHOULE_NUM,
                                                        SHOULD_NUM,APPRO_MANAGER,SUBDIRECOTR,DEPLOY_BED,SPES_SORT_ID,
                                                        OUTP_OR_INP,APPRO_DOCTOR,APPRO_TECH,CENTER_ID,DEPT_ATTR,PRINCIPAL,
                                                        INTERNAL_OR_SERGERY,APPRO_DRUG,APPRO_PROJECT,CENTER_SORT_ID,
                                                        IS_PIVOT_DEPT,DEPT_ATTR,APPRO_NUM,APPRO_NURSE,APPRO_OTHER 
                                                        FROM {0}.DEPT_INFO 
                                                        WHERE DEPT_CODE = '{1}'", DataUser.RLZY, code);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        public void DelDeptInfoByCode(string code)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE FROM {0}.DEPT_INFO WHERE DEPT_CODE = '{1}'", DataUser.RLZY, code);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 职称结构分布
        /// </summary>
        /// <param name="dept_code">科室代码</param>
        /// <param name="PersonType">人员类别</param>
        /// <param name="Power">权限</param>
        /// <returns></returns>
        public DataSet ViewJobStat(string dept_code, string PersonType, string Power)
        {
            string code = dept_code == "" ? "" : " AND DEPT_CODE='" + dept_code + "'";
            string porsontype = " AND STAFFSORT IN (" + PersonType + ")";
            string power = Power == "" ? "" : " AND DEPT_CODE IN (" + Power + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT CASE WHEN DEPT_NAME IS NULL THEN '合计' ELSE DEPT_NAME END DEPT_NAME , COUNT(*) AS TOT,
                                        SUM(DECODE(JOB,'主任医师',1,0) ) AS  JOB1,
                                        SUM(DECODE(JOB,'副主任医师',1,0) ) AS JOB2,
                                        SUM(DECODE(JOB,'主治医师',1,0) ) AS JOB3,
                                        SUM(DECODE(JOB,'医师',1,0) ) AS JOB4,
                                        SUM(DECODE(JOB,'见习医师',1,0) ) AS JOB41,
                                        SUM(DECODE(JOB,'主任护师',1,0) ) AS JOB5,
                                        SUM(DECODE(JOB,'副主任护师',1,0) ) AS JOB6,
                                        SUM(DECODE(JOB,'主管护师',1,0) ) AS JOB7,
                                        SUM(DECODE(JOB,'护师',1,0) ) AS JOB8,
                                        SUM(DECODE(JOB,'护士',1,0) ) AS JOB9,
                                        SUM(DECODE(JOB,'见习护士',1,0) ) AS JOB91,
                                        SUM(DECODE(JOB,'主任技师',1,0) ) AS JOB10,
                                        SUM(DECODE(JOB,'副主任技师',1,0) ) AS JOB11,
                                        SUM(DECODE(JOB,'主管技师',1,0) ) AS JOB12,
                                        SUM(DECODE(JOB,'技师',1,0) ) AS JOB13,
                                        SUM(DECODE(JOB,'技士',1,0) ) AS JOB14,
                                        SUM(DECODE(JOB,'主任药师',1,0) ) AS JOB15,
                                        SUM(DECODE(JOB,'副主任药师',1,0) ) AS JOB16,
                                        SUM(DECODE(JOB,'主管药师',1,0) ) AS JOB17,
                                        SUM(DECODE(JOB,'药师',1,0) ) AS JOB18,
                                        SUM(DECODE(JOB,'药士',1,0) ) AS JOB19,
                                        SUM(DECODE(JOB,'高级工程师',1,0) ) AS JOB20,
                                        SUM(DECODE(JOB,'工程师',1,0) ) AS JOB21,
                                        SUM(DECODE(JOB,'助理工程师',1,0) ) AS JOB22,
                                        SUM(DECODE(JOB,'会计师',1,0) ) AS JOB23
                                        FROM  {0}.NEW_STAFF_INFO  WHERE ADD_MARK = '1' and ISONGUARD='是' {1} {2} {3} GROUP BY ROLLUP (DEPT_NAME)", DataUser.RLZY, code, porsontype, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public DataSet Staffselect(string types)
        {
            StringBuilder str = new StringBuilder();

            if (types == "0")
            {
                //按年龄统计
                str.AppendFormat(@"SELECT   BIRDS TYPE_NAME,
                                   COUNT ( * ) sums,
                                   SUM (CASE WHEN title = '正高' THEN 1 ELSE 0 END) zg,
                                   SUM (CASE WHEN title = '副高' THEN 1 ELSE 0 END) fg,
                                   SUM (CASE WHEN title = '中级' THEN 1 ELSE 0 END) zj,
                                   SUM (CASE WHEN title = '初级' THEN 1 ELSE 0 END) cj,
                                   SUM (CASE WHEN title = '其它' THEN 1 ELSE 0 END) qt
                            FROM   rlzy.v_staff_info
                        GROUP BY   BIRDS
                        order  by BIRDS");
            }
            else if (types == "1")
            {
                //按干部类别统计
                str.AppendFormat(@" select b.CADRES_TYPE TYPE_NAME,nvl(a.sums,0) sums,nvl(a.zg,0) zg,nvl(a.fg,0) fg,nvl(zj,0) zj,nvl(cj,0) cj,nvl(qt,0) qt
                                      from
                                     (SELECT   CADRES,
                                               COUNT ( * ) sums,
                                               SUM (CASE WHEN title = '正高' THEN 1 ELSE 0 END) zg,
                                               SUM (CASE WHEN title = '副高' THEN 1 ELSE 0 END) fg,
                                               SUM (CASE WHEN title = '中级' THEN 1 ELSE 0 END) zj,
                                               SUM (CASE WHEN title = '初级' THEN 1 ELSE 0 END) cj,
                                               SUM (CASE WHEN title = '其它' THEN 1 ELSE 0 END) qt
                                        FROM   rlzy.v_staff_info 
                                    GROUP BY   CADRES) a right join rlzy.CADRES_CATEGORIES b
                                    on a.cadres=b.CADRES_TYPE 
                                    order by b.id");
            }
            else
            {
                //按学历统计
                str.AppendFormat(@" select b.LEARNSUFFER TYPE_NAME,nvl(a.sums,0) sums,nvl(a.zg,0) zg,nvl(a.fg,0) fg,nvl(zj,0) zj,nvl(cj,0) cj,nvl(qt,0) qt
                                      from
                                     (SELECT   EDU,
                                               COUNT ( * ) sums,
                                               SUM (CASE WHEN title = '正高' THEN 1 ELSE 0 END) zg,
                                               SUM (CASE WHEN title = '副高' THEN 1 ELSE 0 END) fg,
                                               SUM (CASE WHEN title = '中级' THEN 1 ELSE 0 END) zj,
                                               SUM (CASE WHEN title = '初级' THEN 1 ELSE 0 END) cj,
                                               SUM (CASE WHEN title = '其它' THEN 1 ELSE 0 END) qt
                                        FROM   rlzy.v_staff_info 
                                    GROUP BY   EDU) a right join rlzy.LEARNSUFFER_DICT b
                                    on a.EDU=b.LEARNSUFFER 
                                    order by b.id");
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询学历，年龄分布
        /// </summary>
        /// <param name="dept_code">科室代码</param>
        /// <param name="PersonType">人员类别</param>
        /// <param name="Power">权限</param>
        /// <returns></returns>
        public DataSet ViewSufferAgeStat(string dept_code, string PersonType, string Power)
        {
            string code = dept_code == "" ? "" : " AND DEPT_CODE='" + dept_code + "'";
            string porsontype = " AND STAFFSORT IN (" + PersonType + ")";
            string power = Power == "" ? "" : " AND DEPT_CODE IN (" + Power + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                                        SELECT '全院合计' AS DEPT_NAME, SUM(JOB1) AS JOB1,SUM(JOB2) AS JOB2,SUM(JOB3) AS JOB3,
                                                SUM(JOB4) AS JOB4,SUM(JOB5) AS JOB5,SUM(JOB6) AS JOB6,
                                                SUM(JOB7) AS JOB7,SUM(JOB8) AS JOB8,SUM(JOB9) AS JOB9,SUM(JOB10) AS JOB10,SUM(JOB11) AS JOB11,
                                                SUM(JOB12) AS JOB12,SUM(JOB13) AS JOB13,SUM(JOB14) AS JOB14
                                        FROM (SELECT  DEPT_NAME,
                                                 SUM (DECODE (TOPEDUCATE, '研究生', 1, 0)) AS JOB1,
                                                 SUM (DECODE (TOPEDUCATE, '本科', 1, 0)) AS JOB2,
                                                 SUM (DECODE (TOPEDUCATE, '大专', 1, 0)) AS JOB3,
                                                 SUM (DECODE (TOPEDUCATE, '中专', 1, 0)) AS JOB4,
                                                 SUM (DECODE (TOPEDUCATE,'研究生', 0,'本科', 0,'大专', 0,'中专',0,1)) AS JOB5,
                                                 SUM (DECODE (EDU1, '博士', 1, 0)) AS JOB6,
                                                 SUM (DECODE (EDU1, '硕士', 1, 0)) AS JOB7,
                                                 SUM (DECODE (EDU1, '学士', 1, 0)) AS JOB8,
                                                 SUM(CASE WHEN BIRTH <= 30 THEN 1 ELSE 0 END) AS JOB9,
                                                 SUM(CASE WHEN BIRTH > 30 AND BIRTH <= 40 THEN 1 ELSE 0 END) AS JOB10,
                                                 SUM(CASE WHEN BIRTH > 40 AND BIRTH <= 50 THEN 1 ELSE 0 END) AS JOB11,
                                                 SUM(CASE WHEN BIRTH > 50 AND BIRTH <= 55 THEN 1 ELSE 0 END) AS JOB12,
                                                 SUM(CASE WHEN BIRTH > 55 AND BIRTH <= 60 THEN 1 ELSE 0 END) AS JOB13,
                                                 SUM(CASE WHEN BIRTH > 60 THEN 1 ELSE 0 END) AS JOB14
                                            FROM (SELECT   TOPEDUCATE, EDU1, DEPT_NAME,
                                                           TRUNC (TO_NUMBER((MONTHS_BETWEEN (SYSDATE,TO_DATE(BIRTHDAY,'yyyy-mm-dd'))))/ 12) AS BIRTH,
                                                           DEPT_CODE
                                                      FROM {0}.NEW_STAFF_INFO
                                                     WHERE ADD_MARK = '1' and ISONGUARD='是' {1} {2} {3}
                                                  ORDER BY BIRTHDAY, DEPT_CODE) T1
                                        GROUP BY DEPT_NAME)
                                        UNION ALL 
                                        SELECT DEPT_NAME,JOB1,JOB2,JOB3,JOB4,JOB5,JOB6,JOB7,
                                                JOB8,JOB9,JOB10,JOB11,JOB12,JOB13,JOB14
                                        FROM (SELECT  DEPT_NAME,
                                                 SUM (DECODE (TOPEDUCATE, '研究生', 1, 0)) AS JOB1,
                                                 SUM (DECODE (TOPEDUCATE, '本科', 1, 0)) AS JOB2,
                                                 SUM (DECODE (TOPEDUCATE, '大专', 1, 0)) AS JOB3,
                                                 SUM (DECODE (TOPEDUCATE, '中专', 1, 0)) AS JOB4,
                                                 SUM (DECODE (TOPEDUCATE,'研究生', 0,'本科', 0,'大专', 0,'中专',0,1)) AS JOB5,
                                                 SUM (DECODE (EDU1, '博士', 1, 0)) AS JOB6,
                                                 SUM (DECODE (EDU1, '硕士', 1, 0)) AS JOB7,
                                                 SUM (DECODE (EDU1, '学士', 1, 0)) AS JOB8,
                                                 SUM(CASE WHEN BIRTH <= 30 THEN 1 ELSE 0 END) AS JOB9,
                                                 SUM(CASE WHEN BIRTH > 30 AND BIRTH <= 40 THEN 1 ELSE 0 END) AS JOB10,
                                                 SUM(CASE WHEN BIRTH > 40 AND BIRTH <= 50 THEN 1 ELSE 0 END) AS JOB11,
                                                 SUM(CASE WHEN BIRTH > 50 AND BIRTH <= 55 THEN 1 ELSE 0 END) AS JOB12,
                                                 SUM(CASE WHEN BIRTH > 55 AND BIRTH <= 60 THEN 1 ELSE 0 END) AS JOB13,
                                                 SUM(CASE WHEN BIRTH > 60 THEN 1 ELSE 0 END) AS JOB14
                                            FROM (SELECT   TOPEDUCATE, EDU1, DEPT_NAME,
                                                           TRUNC (TO_NUMBER((MONTHS_BETWEEN (SYSDATE,TO_DATE(BIRTHDAY,'yyyy-mm-dd'))))/ 12) AS BIRTH,
                                                           DEPT_CODE
                                                      FROM {0}.NEW_STAFF_INFO
                                                     WHERE ADD_MARK = '1' and ISONGUARD='是' {1} {2} {3}
                                                  ORDER BY BIRTHDAY, DEPT_CODE) T1
                                        GROUP BY DEPT_NAME)", DataUser.RLZY, code, porsontype, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        ///  查询人员类别分布
        /// </summary>
        /// <param name="dept_code">科室代码</param>
        /// <param name="onDuty">true:在岗,false:不在岗</param>
        /// <param name="Power">权限</param>
        /// <returns></returns>
        public DataSet ViewStaffTypeList(string dept_code, bool onDuty, string Power, string staffsort)
        {
            string code = dept_code == "" ? "" : " AND DEPT_CODE='" + dept_code + "'";
            string countType = onDuty ? "AND (ISONGUARD='在岗' or ISONGUARD='是')" : " AND (ISONGUARD<>'在岗' and ISONGUARD<>'是')";
            string NoCountType = onDuty ? "AND (ISONGUARD<>'在岗' and ISONGUARD<>'是')" : " AND (ISONGUARD='在岗' or ISONGUARD='是')";
            string power = Power == "" ? "" : " AND DEPT_CODE IN (" + Power + ")";
            string porsontype = " AND STAFFSORT IN (" + staffsort + ")";

            string strcode = String.Format(@"SELECT SERIAL_NO AS ID,PERS_SORT_NAME  FROM RLZY.PERS_SORT_DICT");
            DataTable table = OracleOledbBase.ExecuteDataSet(strcode).Tables[0];

            StringBuilder str = new StringBuilder();
            str.Append(@" SELECT '合计' AS DEPT_NAME ");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.Append(",NVL(SUM(A" + table.Rows[i]["ID"].ToString() + "),0) AS A" + table.Rows[i]["ID"].ToString());
            }
            str.Append(@"                  FROM (SELECT  DEPT_NAME,DEPT_CODE ");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.Append(",SUM (DECODE (STAFFSORT, '" + table.Rows[i]["PERS_SORT_NAME"].ToString() + "', 1, 0)) AS A" + table.Rows[i]["ID"].ToString());
            }

            str.AppendFormat(@"             FROM {0}.NEW_STAFF_INFO
                                            WHERE ADD_MARK = '1'  {1} {2} {3} {4}
                                        GROUP BY DEPT_NAME,DEPT_CODE)
                                        UNION ALL 
                                        SELECT DEPT_NAME ", DataUser.RLZY, code, countType, power, porsontype);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.Append(",A" + table.Rows[i]["ID"].ToString());
            }

            str.Append(@"                  FROM (SELECT  DEPT_NAME,DEPT_CODE ");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.Append(",SUM (DECODE (STAFFSORT, '" + table.Rows[i]["PERS_SORT_NAME"].ToString() + "', 1, 0)) AS A" + table.Rows[i]["ID"].ToString());
            }
            str.AppendFormat(@"             FROM {0}.NEW_STAFF_INFO
                                            WHERE ADD_MARK = '1'  {1} {2} {3} {4}
                                        GROUP BY DEPT_NAME,DEPT_CODE)
                                        UNION ALL
                                        SELECT DEPT_NAME", DataUser.RLZY, code, countType, power, porsontype);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.Append(",0 A" + table.Rows[i]["ID"].ToString());
            }

            str.AppendFormat(@"         FROM {0}.NEW_STAFF_INFO WHERE ADD_MARK = '1' {1} {2} AND DEPT_CODE NOT IN (SELECT DEPT_CODE FROM (SELECT  DEPT_NAME,DEPT_CODE ", DataUser.RLZY, code, power);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.Append(",SUM (DECODE (STAFFSORT, '" + table.Rows[i]["PERS_SORT_NAME"].ToString() + "', 1, 0)) AS A" + table.Rows[i]["ID"].ToString());
            }
            str.AppendFormat(@"         FROM {0}.NEW_STAFF_INFO
                                            WHERE ADD_MARK = '1'  {1} {2} {3} {4}
                                        GROUP BY DEPT_NAME,DEPT_CODE))  GROUP BY DEPT_NAME", DataUser.RLZY, code, countType, power, porsontype);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询医院信息
        /// </summary>
        /// <param name="year">时间</param>
        /// <returns></returns>
        public DataSet ViewUnitDetail(string year)
        {
            string StYear = year == "" ? "(SELECT MAX (DAS_ST_YEAR_MONTH) FROM RLZY.HOSP_NAME_DICT)" : " '" + year + "'";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT UNIT_CODE, UNIT_NAME, UNIT_TYPE, DAS_REPORT_TYPE, DAS_ST_YEAR_MONTH,
                                   DAS_NAME_DEAN, DAS_NAME_COMMISSAR, AUTHORIZED_POPLE_TOTAL,
                                   AUTHORIZED_CADRE_HIGHER, FACT_RETIRE, FACT_CADRE_HIGHER, FACT_RELATION,
                                   MAILING_ADDRESS, ZIP_CODE, ARM_PHONE, PLACE_PHONE, CHARACTER,
                                   CHARACTER_CODE, UNIT_LEVEL, UNIT_LEVEL_CODE, SUB_UNIT, CONDTION,
                                   TOTAL_AREA, OPERATION_AREA, OFFICE_AREA, ASSISTANT_AREA, EXIST_AREA,
                                   AMBULANCE, LIBRARY, BOOK_NUM, FOREIGN_BOOK_NUM, MAGAZINE_SORT_NUM,
                                   FOREIGN_MAGAZINE, SORT_NUM, COMPUTER_NUM, COMPUTER_OLD_NUM, LOCATION,
                                   MEDICAL_BILITY, CARE_BILITY
                              FROM {0}.HOSP_NAME_DICT
                             WHERE DAS_ST_YEAR_MONTH = {1}", DataUser.RLZY, StYear);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        public DataTable GetStaffList(string staffid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select * from rlzy.new_staff_info where staff_id={0}", staffid);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        /// <summary>
        /// 添加单位基本信息
        /// </summary>
        public void InsertUnitDetail(string unit_code, string unit_name, string das_st_year_month,
            string das_name_dean, string das_name_commissar, string authorized_pople_total,
            string authorized_cadre_higher, string fact_fetire, string fact_cadre_higher,
            string fact_relation, string mailing_address, string zip_code, string arm_phone,
            string place_phone, string character, string character_code, string unit_level,
            string unit_level_code, string sub_unit, string condtion, string total_area,
            string operation_area, string office_area, string assistant_area, string exist_area,
            string ambulance, string library, string book_num, string foreign_book_num,
            string magazine_sort_num, string foreign_magazine, string sort_num, string computer_num,
            string computer_old_num, string location, string txtMedicalBility, string txtCareBilty)
        {

            DelUnitDetail(das_st_year_month);

            string str = "INSERT INTO RLZY.HOSP_NAME_DICT (UNIT_CODE,UNIT_NAME,DAS_ST_YEAR_MONTH,DAS_NAME_DEAN,DAS_NAME_COMMISSAR,AUTHORIZED_POPLE_TOTAL,AUTHORIZED_CADRE_HIGHER,FACT_RETIRE,FACT_CADRE_HIGHER,FACT_RELATION, MAILING_ADDRESS,ZIP_CODE,ARM_PHONE,PLACE_PHONE,CHARACTER,CHARACTER_CODE,UNIT_LEVEL,UNIT_LEVEL_CODE,SUB_UNIT,CONDTION,TOTAL_AREA,OPERATION_AREA,OFFICE_AREA,ASSISTANT_AREA,EXIST_AREA,AMBULANCE,LIBRARY,BOOK_NUM,FOREIGN_BOOK_NUM,MAGAZINE_SORT_NUM,FOREIGN_MAGAZINE,SORT_NUM,COMPUTER_NUM,COMPUTER_OLD_NUM,LOCATION,MEDICAL_BILITY,CARE_BILITY )values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "UNIT_CODE" , unit_code ) ,
					new OleDbParameter( "UNIT_NAME" , unit_name ) ,
					new OleDbParameter( "DAS_ST_YEAR_MONTH" , das_st_year_month ) ,
					new OleDbParameter( "DAS_NAME_DEAN", das_name_dean ),
					new OleDbParameter( "DAS_NAME_COMMISSAR" , das_name_commissar ) ,
					new OleDbParameter( "AUTHORIZED_POPLE_TOTAL", authorized_pople_total),
					new OleDbParameter( "AUTHORIZED_CADRE_HIGHER", authorized_cadre_higher),
					new OleDbParameter( "FACT_RETIRE", fact_fetire),
					new OleDbParameter( "FACT_CADRE_HIGHER", fact_cadre_higher),
					new OleDbParameter( "FACT_RELATION", fact_relation),
					new OleDbParameter( "MAILING_ADDRESS", mailing_address),
					new OleDbParameter( "ZIP_CODE", zip_code),
					new OleDbParameter( "ARM_PHONE", arm_phone),

					new OleDbParameter( "PLACE_PHONE" , place_phone ) ,
					new OleDbParameter( "CHARACTER" , character ) ,
					new OleDbParameter( "CHARACTER_CODE" , character_code ) ,
					new OleDbParameter( "UNIT_LEVEL", unit_level ),
					new OleDbParameter( "UNIT_LEVEL_CODE" , unit_level_code ) ,
					new OleDbParameter( "SUB_UNIT", sub_unit),
					new OleDbParameter( "CONDTION", condtion),
					new OleDbParameter( "TOTAL_AREA", total_area),
					new OleDbParameter( "OPERATION_AREA", operation_area),
					new OleDbParameter( "OFFICE_AREA", office_area),
					new OleDbParameter( "ASSISTANT_AREA", assistant_area),

					new OleDbParameter( "EXIST_AREA" , exist_area ) ,
					new OleDbParameter( "AMBULANCE" , ambulance ) ,
					new OleDbParameter( "LIBRARY" , library ) ,
					new OleDbParameter( "BOOK_NUM", book_num ),
					new OleDbParameter( "FOREIGN_BOOK_NUM" , foreign_book_num ) ,
					new OleDbParameter( "MAGAZINE_SORT_NUM", magazine_sort_num),
					new OleDbParameter( "FOREIGN_MAGAZINE", foreign_magazine),
					new OleDbParameter( "SORT_NUM", sort_num),
					new OleDbParameter( "COMPUTER_NUM", computer_num),
					new OleDbParameter( "COMPUTER_OLD_NUM", computer_old_num),
					new OleDbParameter( "LOCATION", location),
					new OleDbParameter( "MEDICAL_BILITY", txtMedicalBility),
					new OleDbParameter( "CARE_BILITY", txtCareBilty)
				};

            OracleOledbBase.ExecuteNonQuery(str, cmdPara);
        }

        /// <summary>
        /// 删除单位基本信息
        /// </summary>
        /// <param name="time">时间</param>
        public void DelUnitDetail(string time)
        {
            string strdel = "delete RLZY.HOSP_NAME_DICT where DAS_ST_YEAR_MONTH=?";
            OleDbParameter[] delPara = new OleDbParameter[]
				{
					new OleDbParameter("DAS_ST_YEAR_MONTH",time)
				};
            OracleOledbBase.ExecuteNonQuery(strdel, delPara);
        }

        /// <summary>
        /// 浏览医疗项目
        /// </summary>
        /// <param name="deptCode">科室代码</param>
        /// <param name="add_mark">0:已提交,1:已审批,3:保存未提交</param>
        /// <param name="power">权限</param>
        /// <returns></returns>
        public DataSet ViewSpecMediList(string deptCode, string add_mark, string power, string year)
        {
            string Code = deptCode == "" ? "" : " AND B.DEPT_CODE = '" + deptCode + "'";
            string mark = "AND ADD_MARK=" + add_mark + "";
            string Power = power == "" ? "" : "AND A.DEPT_CODE IN (" + power + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   A.ID, A.STAT_MONTH,SUBSTR (A.STAT_MONTH, 1, 4) AS YEARS, B.DEPT_NAME,
                                      A.SPE_MEDI_ITEM, A.INP_NO, A.NAME, A.SEX, A.AGES,
                                     A.BIRTH, A.UNIT, A.IDENTITY, A.DIAG_DESC, A.INSURANCE_NO, A.E_TIMES,
                                     A.ARMCAR, A.ADD_MARK
                               FROM {0}.SPEC_MEDI A, {5}.SYS_DEPT_DICT B
                               WHERE A.DEPT_CODE = B.DEPT_CODE {1} {2} {3} AND SUBSTR(A.STAT_MONTH, 1, 4) {4}
                               ORDER BY A.STAT_MONTH DESC", DataUser.RLZY, Code, mark, Power, year, DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 添加特殊医疗信息
        /// </summary>
        public void InsertSpecMediList(string iStatMonth, string Name, string Ages, string InpNo, string Unit, string DiagDesc,
                                        string Armcar, string SpeMediItem, string Birth, string ETimes,
                                        string Sex, string Identity, string InsuranceNo, string deptCode, string add_mark, string EnterTime, string EnterPers)
        {
            EnterPers = EnterPers == null ? "Admin" : EnterPers;
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.SPEC_MEDI(ID,STAT_MONTH,SPE_MEDI_ITEM,INP_NO,NAME,
                                            SEX,BIRTH,UNIT,IDENTITY,DIAG_DESC,INSURANCE_NO,DEPT_CODE,ENTER_TIME,ENTER_PERS,AGES,E_TIMES,ARMCAR,ADD_MARK)
                                      VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
                {
                    new OleDbParameter( "ID" , OracleOledbBase.GetMaxID("ID","RLZY.SPEC_MEDI")) ,
                    new OleDbParameter( "STAT_MONTH" , iStatMonth ) ,
                    new OleDbParameter( "SPE_MEDI_ITEM" , SpeMediItem ) ,
                    new OleDbParameter( "INP_NO", InpNo ),
                    new OleDbParameter( "NAME" , Name ) ,
                    new OleDbParameter( "SEX", Sex),
                    new OleDbParameter( "BIRTH", Birth),
                    new OleDbParameter( "UNIT", Unit),
                    new OleDbParameter( "IDENTITY", Identity),
                    new OleDbParameter( "DIAG_DESC", DiagDesc),
                    new OleDbParameter( "INSURANCE_NO", InsuranceNo),
                    new OleDbParameter( "DEPT_CODE", deptCode),
                    new OleDbParameter( "ENTER_TIME", EnterTime),
                    new OleDbParameter( "ENTER_PERS", EnterPers),
                    new OleDbParameter( "AGES" , Ages ) ,
                    new OleDbParameter( "E_TIMES" , ETimes ) ,
                    new OleDbParameter( "ARMCAR" , Armcar ) ,
                    new OleDbParameter( "ADD_MARK", add_mark )
                };
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 更新特殊医疗信息
        /// </summary>
        public void UpdataSpecMediList(string id, string Name, string Ages, string InpNo, string Unit, string DiagDesc,
                                        string Armcar, string SpeMediItem, string Birth, string ETimes,
                                        string Sex, string Identity, string InsuranceNo, string add_mark)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.SPEC_MEDI
                                        SET SPE_MEDI_ITEM=?,INP_NO=?,NAME=?,SEX=?,
                                        BIRTH=?,UNIT=?,IDENTITY=?,DIAG_DESC=?,
                                        INSURANCE_NO=?,AGES=?,E_TIMES=?,ARMCAR=?,ADD_MARK=?
                                        WHERE ID IN (?)", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
                {
                    new OleDbParameter( "SPE_MEDI_ITEM" , SpeMediItem ) ,
                    new OleDbParameter( "INP_NO", InpNo ),
                    new OleDbParameter( "NAME" , Name ) ,
                    new OleDbParameter( "SEX", Sex),
                    new OleDbParameter( "BIRTH", Birth),
                    new OleDbParameter( "UNIT", Unit),
                    new OleDbParameter( "IDENTITY", Identity),
                    new OleDbParameter( "DIAG_DESC", DiagDesc),
                    new OleDbParameter( "INSURANCE_NO", InsuranceNo),
                    new OleDbParameter( "AGES" , Ages ) ,
                    new OleDbParameter( "E_TIMES" , ETimes ) ,
                    new OleDbParameter( "ARMCAR" , Armcar ) ,
                    new OleDbParameter( "ADD_MARK", add_mark ),
                    new OleDbParameter( "ID", id )
                };
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 批量处理审批流程
        /// </summary>
        /// <param name="id"></param>
        /// <param name="add_mark"></param>
        public void EchoUpdataSpecMediList(string id, string add_mark)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.SPEC_MEDI
                                        SET ADD_MARK='{1}'
                                        WHERE ID IN ({2})", DataUser.RLZY, add_mark, id);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 批量删除审批流程
        /// </summary>
        /// <param name="id"></param>
        public void EchoDelSpecMediList(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE FROM {0}.SPEC_MEDI WHERE ID IN ({1})", DataUser.RLZY, id);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }


        /// <summary>
        /// 浏览人员基本信息
        /// </summary>
        /// <param name="deptCode">科室代码</param>
        /// <param name="add_mark">0:已提交,1:已审批,3:保存未提交</param>
        /// <param name="power">权限</param>
        /// <returns></returns>
        public DataSet ViewStaffList(string deptCode, string add_mark, string power, string StaffSort, string onguard, string sex, string endtime)
        {

            string Code = deptCode == "" ? "" : " AND A.DEPT_CODE = '" + deptCode + "'";
            if (onguard != "")
                Code += " and ISONGUARD='" + onguard + "' ";
            if (onguard == "")
            {
                Code += " and (ISONGUARD='是' or ISONGUARD='否') ";
            }
            if (sex != "")
            {
                Code += " and SEX='" + sex + "' ";
            }
            if (endtime != "")
            {
                Code += " and to_char(to_date(CONTRACT_END,'yyyy-mm-dd'),'yyyymm')='" + endtime + "' ";
            }
            string mark = " AND ADD_MARK=" + add_mark + "";
            string Power = power == "" ? "" : "AND A.DEPT_CODE IN (" + power + ")";
            StaffSort = StaffSort == "" ? "" : "AND A.STAFFSORT IN (" + StaffSort + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT  A.EMP_NO,A.STAFF_ID, A.DEPT_CODE, A.DEPT_NAME, A.NAME, A.IF_ARMY, A.ADD_MARK, A.ISONGUARD,
                                       A.BIRTHDAY, A.SEX, A.NATIONALS, A.BONUS_FACTOR, A.GOVERNMENT_ALLOWANCE,
                                       A.CADRES_CATEGORIES, A.STUDY_OVER_DATE, A.DEPT_TYPE, A.TOPEDUCATE,
                                       A.STUDY_SPECSORT, A.INHOSPITALDATE, A.BASEPAY, A.RETAINTERM, A.JOB, A.JOBDATE,
                                       A.STAFFSORT, A.BEENROLLEDINDATE, A.WORKDATE, A.DUTY, A.DUTYDATE, A.TECHINCCLASS,
                                       A.TECHNICCLASSDATE, A.CIVILSERVICECLASS, A.CIVILSERVICECLASSDATE,
                                       A.SANTSPECSORT, A.ROOTSPECSORT, A.MEDICARDMARK, A.MEDICARD, A.INPUT_USER,
                                       A.INPUT_DATE, A.USER_DATE, A.GUARDTEAM, A.GUARDGROUP, A.GUARDDUTY, A.GUARDTYPE,
                                       A.GUARDCHAN, A.GUARDTIME, A.GUARDCAUS, A.GUARDREMARK, A.DEPTGROUP, A.HOMEPLACE,
                                       A.CERTIFICATE_NO, A.MARITAL_STATUS, A.TITLE_LIST, A.EDU1, A.GRADUATE_ACADEMY,
                                       A.DATE_OF_GRADETITLE, A.RANK, A.TITLE, A.GROUP_ID, A.MEMO, A.MARK_USER, A.USER_ID,
                                       A.JW_USER_NAME, A.INPUT_CODE, A.IMG_ID, A.JOB_TITLE, A.TITLE_DATE, A.EXPERT,
                                       A.CREDITHOUR_PERYEAR, A.LEADTECN, A.STATION_CODE, B.STATION_NAME,ISBRAID,C.DEPT_NAME AS GORD_CODE,A.TURNOVER_TIME,A.CONTRACT_START,A.CONTRACT_END,A.BONUS_FLAG,A.CHECK_FLAG,A.BANK_CODE
                                  FROM {0}.NEW_STAFF_INFO A, {1}.SYS_STATION_MAINTENANCE_DICT B,{1}.SYS_DEPT_DICT C
                                 WHERE A.STATION_CODE = B.ID(+) and A.GORD_CODE=C.DEPT_CODE(+) {2} {3} {4} {5} ORDER BY A.STAFF_ID", DataUser.RLZY, DataUser.COMM, Code, mark, Power, StaffSort);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取考勤项目字典
        /// </summary>
        /// <returns></returns>
        public DataSet GetAttendanceDict()
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT * FROM RLZY.ATTENDANCE_NAME_DICT order by attendance_code", "");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取考勤备注字典
        /// </summary>
        /// <returns></returns>
        public DataSet GetAttendanceMemo()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT MEMO FROM RLZY.ATTENDANCE_MEMO_DICT ORDER BY SORT_NO", "");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取考勤结果
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet GetAttendanceInfo(string datetime, string deptcode, string inputname)
        {
            string deptcodes = "";
            string deptaccountdept = "";
            if (!deptcode.Equals(""))
            {
                deptcodes = "AND   A.DEPT_CODE in (" + deptcode + ")";
                deptaccountdept = "and B.ACCOUNT_DEPT_CODE in (" + deptcode + ")";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"  SELECT 
                                      A.*,
                                      B.ATTENDANCE_NAME,
                                      C.NAME STAFF_NAME
                                      FROM
                                      (
                                      SELECT   NVL(B.DEPT_CODE,A.ACCOUNT_DEPT_CODE) DEPT_CODE,
                                               NVL(B.DEPT_NAME,A.ACCOUNT_DEPT_NAME) DEPT_NAME,
                                               NVL(A.EMP_NO,'') EMP_NO,
                                               NVL(A.STAFF_ID,B.STAFF_ID) STAFF_ID,
                                               NVL(B.ATTENDANCE_CODE,A.ATTENDANCE_CODE) ATTENDANCE_CODE,
                                               NVL(B.ATTENDANCE_VALUE,A.ATTENDANCE_VALUE) ATTENDANCE_VALUE,
                                               B.MEMO,
                                               NVL(B.REPORTER,'{2}') REPORTER,
                                               '{0}' YEAR_MONTH,
                                               CASE WHEN check_tag IS NULL THEN '未保存'ELSE DECODE(check_tag,'0','未提交',DECODE(check_tag,'2','已提交','已审核')) END CHECK_TAG,
                                               CASE WHEN NVL(B.ATTENDANCE_CODE,A.ATTENDANCE_CODE)='A01' THEN '1' ELSE '0' END EDIT_TAG,
                                               NVL(B.ATTENDANCE_VALUE,A.ATTENDANCE_VALUE) JZ,
                                               NVL(C.QJTS,0) QJTS
                                        FROM   (SELECT A.*,C.*,B.ACCOUNT_DEPT_CODE,B.ACCOUNT_DEPT_NAME FROM RLZY.NEW_STAFF_INFO A,(SELECT 'A01' ATTENDANCE_CODE,TO_CHAR(LAST_DAY(TO_DATE('{0}','yyyymmdd')),'DD') ATTENDANCE_VALUE FROM DUAL) C,COMM.SYS_DEPT_DICT B where A.DEPT_CODE=B.DEPT_CODE AND a.ADD_MARK='1'and isonguard='是' and CHECK_FLAG='是' {3}) A
                                               FULL JOIN
                                               (SELECT * FROM RLZY.QU_ATTENDANCE_DEPT A WHERE YEAR_MONTH=TO_DATE('{0}','yyyymmdd') {1}) B           
                                               ON A.STAFF_ID=B.STAFF_ID
                                               LEFT JOIN
                                               (SELECT STAFF_ID,SUM(ATTENDANCE_VALUE) QJTS FROM RLZY.QU_ATTENDANCE_DEPT WHERE TO_CHAR(YEAR_MONTH,'yyyy')=SUBSTR('{0}',0,4) AND ATTENDANCE_CODE='B01' GROUP BY STAFF_ID) C
                                               ON A.STAFF_ID=C.STAFF_ID
                                      ) A,
                                      RLZY.ATTENDANCE_NAME_DICT B,
                                      RLZY.NEW_STAFF_INFO C
                                      WHERE A.ATTENDANCE_CODE=B.ATTENDANCE_CODE 
                                            AND A.STAFF_ID=C.STAFF_ID
                                        ", datetime, deptcodes, inputname, deptaccountdept);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 删除考勤结果
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <param name="attcode"></param>
        public void DelAttendanceInfo(string datetime, string deptcode, string attcode, string staffid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE RLZY.QU_ATTENDANCE_DEPT where year_month= to_date('{0}','yyyymmdd') and dept_code='{1}' and attendance_code='{2}' AND STAFF_ID='{3}'", datetime, deptcode, attcode, staffid);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 保存考勤处理
        /// </summary>
        /// <param name="costdetails"></param>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public void SaveAttendanceInfo(Dictionary<string, string>[] costdetails, string datetime, string deptcode, string inputuser)
        {
            MyLists listtable = new MyLists();

            for (int i = 0; i < costdetails.Length; i++)
            {
                if (costdetails[i]["DEPT_CODE"] == null || costdetails[i]["STAFF_ID"].Equals("") || costdetails[i]["ATTENDANCE_CODE"].Equals("") || costdetails[i]["CHECK_TAG"].Equals("已提交")) //|| costdetails[i]["CHECK_TAG"].Equals("已审核")
                {
                    continue;
                }

                string staffid = costdetails[i]["STAFF_ID"].ToString();
                string attendancecode = costdetails[i]["ATTENDANCE_CODE"].ToString();

                if (costdetails[i]["CHECK_TAG"].Equals("已审核"))
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendFormat(@"UPDATE RLZY.QU_ATTENDANCE_DEPT SET ATTENDANCE_VALUE=?,MEMO=?
                                          WHERE year_month=to_date(?,'yyyymmdd') AND dept_code=? AND STAFF_ID=? and ATTENDANCE_CODE=?", "");

                    OleDbParameter[] parameteradd = {
											  
											  new OleDbParameter("ATTENDANCE_VALUE",Convert.IsDBNull(costdetails[i]["ATTENDANCE_VALUE"]) ? 0:Convert.ToDouble(costdetails[i]["ATTENDANCE_VALUE"])),
                                              new OleDbParameter("MEMO",costdetails[i]["MEMO"]),

											  new OleDbParameter("YEAR_MONTH",datetime ),
											  new OleDbParameter("DEPT_CODE",deptcode),
											  new OleDbParameter("STAFF_ID",costdetails[i]["STAFF_ID"]),
                                              new OleDbParameter("ATTENDANCE_CODE",costdetails[i]["ATTENDANCE_CODE"])

										  };
                    List listAdd = new List();
                    listAdd.StrSql = strSql;
                    listAdd.Parameters = parameteradd;
                    listtable.Add(listAdd);
                }
                else
                {

                    StringBuilder strDel = new StringBuilder();
                    strDel.AppendFormat("DELETE RLZY.QU_ATTENDANCE_DEPT WHERE year_month= to_date('{0}','yyyymmdd') AND dept_code='{1}' AND STAFF_ID='{2}' AND ATTENDANCE_CODE='{3}'", datetime, deptcode, staffid, attendancecode);

                    List listDel = new List();
                    listDel.StrSql = strDel;
                    listtable.Add(listDel);

                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendFormat(@"INSERT INTO RLZY.QU_ATTENDANCE_DEPT
                                          (YEAR_MONTH,DEPT_CODE,EMP_NO,ATTENDANCE_CODE,ATTENDANCE_VALUE,MEMO,REPORTER,STAFF_ID,DEPT_NAME)
                                        VALUES(to_date(?,'yyyymmdd'),?,?,?,?,?,?,?,?)", "");

                    OleDbParameter[] parameteradd = {
											  new OleDbParameter("YEAR_MONTH",datetime ),
											  new OleDbParameter("DEPT_CODE",deptcode),
											  new OleDbParameter("EMP_NO",costdetails[i]["EMP_NO"]),
											  new OleDbParameter("ATTENDANCE_CODE",costdetails[i]["ATTENDANCE_CODE"]),
											  new OleDbParameter("ATTENDANCE_VALUE",Convert.IsDBNull(costdetails[i]["ATTENDANCE_VALUE"]) ? 0:Convert.ToDouble(costdetails[i]["ATTENDANCE_VALUE"])),
											  new OleDbParameter("MEMO",costdetails[i]["MEMO"]),
											  new OleDbParameter("REPORTER",inputuser),
											  new OleDbParameter("STAFF_ID",costdetails[i]["STAFF_ID"]),
											  new OleDbParameter("DEPT_NAME",costdetails[i]["DEPT_NAME"])
										  };
                    List listAdd = new List();
                    listAdd.StrSql = strSql;
                    listAdd.Parameters = parameteradd;
                    listtable.Add(listAdd);
                }
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 考勤审核处理
        /// </summary>
        /// <param name="costdetails"></param>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <param name="inputuser"></param>
        public void UpdateAttendanceInfo(Dictionary<string, string>[] costdetails, string datetime, string deptcode, string inputuser)
        {
            MyLists listtable = new MyLists();

            for (int i = 0; i < costdetails.Length; i++)
            {
                if (costdetails[i]["DEPT_CODE"] == null || costdetails[i]["STAFF_ID"].Equals("") || costdetails[i]["ATTENDANCE_CODE"].Equals("") || costdetails[i]["CHECK_TAG"].Equals("未保存") || costdetails[i]["CHECK_TAG"].Equals("已审核") || costdetails[i]["CHECK_TAG"].Equals("未提交"))
                {
                    continue;
                }
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"UPDATE RLZY.QU_ATTENDANCE_DEPT SET CHECK_TAG='1'
                                       WHERE YEAR_MONTH=to_date(?,'yyyymmdd') AND DEPT_CODE=? AND ATTENDANCE_CODE=? AND STAFF_ID=?", "");

                OleDbParameter[] parameteradd = {
											  new OleDbParameter("YEAR_MONTH",datetime ),
											  new OleDbParameter("DEPT_CODE",deptcode),
											  new OleDbParameter("ATTENDANCE_CODE",costdetails[i]["ATTENDANCE_CODE"]),
	   									      new OleDbParameter("STAFF_ID",costdetails[i]["STAFF_ID"])
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 考勤提交处理
        /// </summary>
        /// <param name="costdetails"></param>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <param name="inputuser"></param>
        public void UpdateAttendanceInfoCommit(Dictionary<string, string>[] costdetails, string datetime, string deptcode, string inputuser)
        {
            MyLists listtable = new MyLists();

            for (int i = 0; i < costdetails.Length; i++)
            {
                if (costdetails[i]["DEPT_CODE"] == null || costdetails[i]["STAFF_ID"].Equals("") || costdetails[i]["ATTENDANCE_CODE"].Equals("") || costdetails[i]["CHECK_TAG"].Equals("未保存") || costdetails[i]["CHECK_TAG"].Equals("已提交") || costdetails[i]["CHECK_TAG"].Equals("已审核"))
                {
                    continue;
                }

                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"UPDATE RLZY.QU_ATTENDANCE_DEPT SET CHECK_TAG='2'
                                       WHERE YEAR_MONTH=to_date(?,'yyyymmdd') AND DEPT_CODE=? AND ATTENDANCE_CODE=? AND STAFF_ID=?", "");

                OleDbParameter[] parameteradd = {
											  new OleDbParameter("YEAR_MONTH",datetime ),
											  new OleDbParameter("DEPT_CODE",deptcode),
											  new OleDbParameter("ATTENDANCE_CODE",costdetails[i]["ATTENDANCE_CODE"]),
	   									      new OleDbParameter("STAFF_ID",costdetails[i]["STAFF_ID"])
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 查询代职务岗位津贴
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet SearchDG(string datetime, string deptcode)
        {
            string deptcodes = "";
            if (!deptcode.Equals(""))
            {
                deptcodes = "AND A.DEPT_CODE IN (" + deptcode + ")";
            }

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"    SELECT  A.DEPT_CODE,
                                           B.DEPT_NAME,
                                           A.STAFF_ID,
                                           B.NAME,
                                           A.MEMO DZW,
                                           SUM(ATTENDANCE_VALUE) DGTS,
                                           MAX(SUBSIDY) ZWBZ,
                                           ROUND(MAX(SUBSIDY)/TO_NUMBER(TO_CHAR(LAST_DAY(TO_DATE ('{0}', 'yyyymmdd')),'DD'))*SUM(ATTENDANCE_VALUE),2) DZJT
                                    FROM   RLZY.QU_ATTENDANCE_DEPT A,
                                           RLZY.NEW_STAFF_INFO B,
                                           RLZY.ATTENDANCE_MEMO_DICT C
                                   WHERE       A.STAFF_ID = B.STAFF_ID
                                           AND A.MEMO = C.MEMO
                                           AND year_month = TO_DATE ('{0}', 'yyyymmdd')
                                           AND A.ATTENDANCE_CODE = 'A07'
                                           {1}
                                GROUP BY   A.DEPT_CODE,
                                           B.DEPT_NAME,
                                           A.STAFF_ID,
                                           B.NAME,
                                           A.MEMO,TO_NUMBER(TO_CHAR(LAST_DAY(TO_DATE ('{0}', 'yyyymmdd')),'DD'))", datetime, deptcodes);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet SearchCQSB(string datetime, string deptcode, string enddatetime)
        {
            string deptcodes = "";
            if (!deptcode.Equals(""))
            {
                deptcodes = "AND B.DEPT_CODE IN (" + deptcode + ")";
            }

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"    select 
                                    B.DEPT_CODE ,
                                    B.DEPT_NAME ,
                                    sum ( case when CHECK_TAG='1' then 1 else 0 end) RS, 
                                    sum ( case when CHECK_TAG='2' then 1 else 0 end) RS2
                                    FROM 
                                    (SELECT * FROM RLZY.QU_ATTENDANCE_DEPT WHERE to_char(year_month,'yyyymm')>='{0}' AND to_char(year_month,'yyyymm')<='{2}' AND (CHECK_TAG = '1' or check_tag='2') AND ATTENDANCE_CODE='A01') A,
                                    (SELECT DISTINCT A.DEPT_CODE, a.dept_name,a.input_code from comm.SYS_DEPT_DICT a WHERE ATT_FLAG='1') B
                                    WHERE A.DEPT_CODE(+)=B.DEPT_CODE
                                    {1}
                                    GROUP BY B.DEPT_CODE,B.DEPT_NAME
                                    ORDER BY B.DEPT_CODE", datetime, deptcodes, enddatetime);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet SearchDCQ(string datetime, string deptcode, string enddatetime)
        {
            string deptcodes = "";
            if (!deptcode.Equals(""))
            {
                deptcodes = "AND B.DEPT_CODE IN (" + deptcode + ")";
            }

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"    SELECT  A.DEPT_CODE,
                                           B.DEPT_NAME,
                                           A.STAFF_ID,
                                           B.NAME,
                                           A.MEMO DZW,
                                           SUM(ATTENDANCE_VALUE) DGTS
                                    FROM   RLZY.QU_ATTENDANCE_DEPT A,
                                           RLZY.NEW_STAFF_INFO B,
                                           RLZY.ATTENDANCE_MEMO_DICT C
                                   WHERE       A.STAFF_ID = B.STAFF_ID(+)
                                           AND A.MEMO = C.MEMO(+)
                                           AND TO_CHAR(year_month,'yyyymm') >= '{0}'
                                           AND TO_CHAR(year_month,'yyyymm') <= '{2}'
                                           AND A.ATTENDANCE_CODE = 'A07'
                                           {1}
                                GROUP BY   A.DEPT_CODE,
                                           B.DEPT_NAME,
                                           A.STAFF_ID,
                                           B.NAME,
                                           A.MEMO", datetime, deptcodes, enddatetime);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 转科统计
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <param name="enddatetime"></param>
        /// <returns></returns>
        public DataSet SearchZK(string datetime, string deptcode, string enddatetime)
        {
            //考勤类型
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat(@"SELECT ATTENDANCE_CODE, ATTENDANCE_NAME,ATTENDANCE_CODE kk 
                                  FROM RLZY.ATTENDANCE_NAME_DICT a 
                                UNION ALL
                                SELECT REPLACE(to_char(wmsys.wm_concat(case when ATTENDANCE_CODE='A01' THEN ATTENDANCE_CODE ELSE CASE WHEN CAL_TAG='0' THEN '+'||ATTENDANCE_CODE ELSE '-'||ATTENDANCE_CODE END END)),',','') ATTENDANCE_CODE,
                                       '实际出勤' ATTENDANCE_NAME,'A0111' KK
                                FROM (SELECT A.* FROM RLZY.ATTENDANCE_NAME_DICT a WHERE CAL_TAG <>2 order by ATTENDANCE_CODE) ORDER BY KK", DataUser.RLZY);
            DataTable table = OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"     SELECT   b.CHANGE_DATE,
                                   b.FROM_DEPT_NAME,
                                   a.DEPT_NAME,
                                   a.STAFF_ID,
                                   a.NAME,
                                   a.SEX,");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["ATTENDANCE_NAME"].ToString() == "实际出勤")
                {
                    str.AppendFormat(" {0} AS DAYS,", table.Rows[i]["ATTENDANCE_CODE"].ToString());
                }
            }

            str.AppendFormat(@"  b.INPUT_USER,
                                 '' MEMO 
                            FROM   rlzy.NEW_STAFF_INFO a, 
                                   rlzy.USER_CHANGE b,
                                   (  SELECT year_month,
                                             dept_code,
                                             staff_id AS emp_no,");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["ATTENDANCE_NAME"].ToString() != "实际出勤")
                {
                    str.AppendFormat(" SUM(CASE WHEN attendance_code = '{0}' THEN ATTENDANCE_VALUE ELSE 0 END) {0},", table.Rows[i]["ATTENDANCE_CODE"].ToString());
                }
            }
            str.AppendFormat(@"              staff_id AS STAFF_ID
                                      FROM   RLZY.QU_ATTENDANCE_DEPT
                                  GROUP BY   year_month, dept_code, staff_id) c
                           WHERE   ADD_MARK = '1' 
                               AND a.staff_id = b.staff_id
                               AND a.staff_id = c.staff_id
                               AND TO_DATE(b.CHANGE_DATE,'yyyymmdd') >=TO_DATE('{0}','yyyymmdd') AND  TO_DATE(b.CHANGE_DATE,'yyyymmdd') <=LAST_DAY(TO_DATE('{1}','yyyymmdd'))
                               AND TRUNC (c.year_month, 'mm') >= TO_DATE ('{0}', 'yyyymmdd') and TRUNC (c.year_month, 'mm') <= LAST_DAY(TO_DATE ('{1}', 'yyyymmdd'))
                        ORDER BY   NAME", datetime, enddatetime);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <param name="enddatetime"></param>
        /// <returns></returns>
        public DataSet SearchZWCQ(string datetime, string deptcode, string enddatetime)
        {
            string deptcodes = "";

            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat(@"SELECT ATTENDANCE_CODE, ATTENDANCE_NAME,ATTENDANCE_CODE kk 
                                FROM RLZY.ATTENDANCE_NAME_DICT a 
                                UNION ALL
                                SELECT REPLACE(to_char(wmsys.wm_concat(case when ATTENDANCE_CODE='A01' THEN ATTENDANCE_CODE ELSE CASE WHEN CAL_TAG='0' THEN '+'||ATTENDANCE_CODE ELSE '-'||ATTENDANCE_CODE END END)),',','') ATTENDANCE_CODE,
                                '实际出勤' ATTENDANCE_NAME,'ZZZ' KK
                                FROM (SELECT A.* FROM RLZY.ATTENDANCE_NAME_DICT a WHERE CAL_TAG <>2 order by ATTENDANCE_CODE)  ORDER BY KK", DataUser.RLZY);
            DataTable table = OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];

            if (!deptcode.Equals(""))
            {
                deptcodes = "AND A.DEPT_CODE ='" + deptcode + "'";
            }

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"    SELECT   A.STAFF_ID,
                                   B.ACCOUNT_DEPT_NAME ,
                                   C.NAME ,
                                   C.DUTY ,
                                   SUM (A.A01) YCQ,
                                   SUM(  A.A01
                                       - A.A02
                                       - A.A03
                                       - A.A04
                                       - A.A05
                                       - A.A06
                                       - A.A09
                                       - A.B01
                                       - A.B02
                                       - A.B03
                                       - A.C01
                                       - A.C02
                                       - A.C03) SJCQ,
                                   SUM(  A.A02
                                       + A.A03
                                       + A.A04
                                       + A.A05
                                       + A.A06
                                       + A.A09
                                       + A.B01
                                       + A.B02
                                       + A.B03
                                       + A.C01
                                       + A.C02
                                       + A.C03) QQTS,
                                   SUM(A.A07) DCQTS 
                            FROM   RLZY.QU_ATTENDANCE_DEPT_VIEW A,
                                   COMM.SYS_DEPT_DICT B,
                                   RLZY.NEW_STAFF_INFO C
                           WHERE       TO_CHAR (A.YEAR_MONTH, 'YYYYMM') >= '{0}'
                                   AND TO_CHAR (A.YEAR_MONTH, 'YYYYMM') <= '{2}'
                                   AND A.DEPT_CODE = B.DEPT_CODE
                                   AND A.STAFF_ID = C.STAFF_ID
                                   AND C.STAFFSORT IN
                                            ('干部',
                                             '战士',
                                             '临时工',
                                             '回聘',
                                             '借调干部',
                                             '职工',
                                             '非现役文职',
                                             '非现役工勤',
                                             '合同制聘用',
                                             '轮转生',
                                             '职员',
                                             '博士后在站')
                                   AND C.DUTY IS NOT NULL
                                   AND C.DUTY != '无'
                                   {1}
                        GROUP BY   A.STAFF_ID,
                                   C.NAME,
                                   B.ACCOUNT_DEPT_CODE,
                                   C.DUTY,
                                   B.ACCOUNT_DEPT_NAME ORDER BY B.ACCOUNT_DEPT_CODE, A.STAFF_ID", datetime, deptcodes, enddatetime);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet SearchKSCQ(string datetime, string deptcode, string enddatetime)
        {
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat(@"SELECT ATTENDANCE_CODE, ATTENDANCE_NAME,ATTENDANCE_CODE kk 
                                FROM RLZY.ATTENDANCE_NAME_DICT a 
                                UNION ALL
                                SELECT REPLACE(to_char(wmsys.wm_concat(case when ATTENDANCE_CODE='A01' THEN ATTENDANCE_CODE ELSE CASE WHEN CAL_TAG='0' THEN '+'||ATTENDANCE_CODE ELSE '-'||ATTENDANCE_CODE END END)),',','') ATTENDANCE_CODE,
                                '实际出勤' ATTENDANCE_NAME,'ZZZ' KK
                                FROM (SELECT A.* FROM RLZY.ATTENDANCE_NAME_DICT a WHERE CAL_TAG <>2 order by ATTENDANCE_CODE)  ORDER BY KK", DataUser.RLZY);
            DataTable table = OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];

            StringBuilder str = new StringBuilder();
            string deptcodes = "";
            if (!deptcode.Equals(""))
            {
                deptcodes = "AND B.DEPT_CODE IN (" + deptcode + ")";
            }


            str.AppendFormat("SELECT  B.ACCOUNT_DEPT_CODE,B.ACCOUNT_DEPT_NAME \"科室\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(" ,SUM ({0}) \"{1}\"", table.Rows[i]["ATTENDANCE_CODE"].ToString(), table.Rows[i]["ATTENDANCE_NAME"].ToString());
            }


            str.AppendFormat(@" FROM (SELECT   year_month,
                                               dept_code,
                                               staff_id AS emp_no,");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["ATTENDANCE_NAME"].ToString() != "实际出勤")
                {
                    str.AppendFormat(" SUM(CASE WHEN attendance_code = '{0}' THEN ATTENDANCE_VALUE ELSE 0 END) {0},", table.Rows[i]["ATTENDANCE_CODE"].ToString());
                }
            }

            str.AppendFormat(@"                staff_id as STAFF_ID
                                        FROM   RLZY.QU_ATTENDANCE_DEPT
                                    GROUP BY   year_month, dept_code, staff_id) A,");
            str.AppendFormat(@" 
                                       COMM.SYS_DEPT_DICT B
                               WHERE       TO_CHAR (A.YEAR_MONTH, 'YYYYMM') = '{0}'
                                       AND A.DEPT_CODE = B.DEPT_CODE
                                       {1} {2} {3}
                            GROUP BY   
                                       B.ACCOUNT_DEPT_CODE,
                                       B.ACCOUNT_DEPT_NAME
                            ORDER BY   B.ACCOUNT_DEPT_CODE", datetime, "", "", deptcodes);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 空勤人员
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <param name="enddatetime"></param>
        /// <returns></returns>
        public DataSet SearchKQRY(string datetime, string deptcode)
        {
            string deptcodes = "";
            if (!deptcode.Equals(""))
            {
                deptcodes = "AND B.DEPT_CODE IN (" + deptcode + ")";
            }

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" SELECT   D.DEPT_STAYED dept_code,
                               DECODE (D.DEPT_STAYED, '', '合计', MIN (b.dept_name)) dept_name,
                               COUNT ( * ) rs
                        FROM   HISDATA.PAT_VISIT A, COMM.SYS_DEPT_DICT B, HISDATA.TRANSFER D
                       WHERE       A.DUTY = '空勤'
                               AND A.PATIENT_ID = D.PATIENT_ID
                               AND D.DEPT_STAYED = B.DEPT_CODE
                               AND (TO_CHAR (D.DISCHARGE_DATE_TIME, 'YYYYMMDD') >= '{0}' OR D.DISCHARGE_DATE_TIME IS NULL)
                              
                               AND TO_CHAR (D.ADMISSION_DATE_TIME, 'YYYYMMDD') < '{0}' 
                               AND TO_CHAR (D.ADMISSION_DATE_TIME, 'YYYYMMDD') >= '20140101'
                               
                               AND (TO_CHAR (A.DISCHARGE_DATE_TIME, 'YYYYMMDD') >= '{0}' OR A.DISCHARGE_DATE_TIME IS NULL)
                               AND TO_CHAR (A.ADMISSION_DATE_TIME, 'YYYYMMDD') < '{0}'
                               AND TO_CHAR (A.ADMISSION_DATE_TIME, 'YYYYMMDD') >= '20140101'
                    GROUP BY   CUBE (D.DEPT_STAYED)
                    ORDER BY   D.DEPT_STAYED", datetime);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet SearchJWJT(string datetime, string deptcode)
        {
            string deptcodes = "";
            if (!deptcode.Equals(""))
            {
                deptcodes = "AND A.DEPT_CODE IN (" + deptcode + ")";
            }

            StringBuilder str = new StringBuilder();
            //            str.AppendFormat(@"      SELECT   B.dept_code,
            //                                               B.dept_name,
            //                                               a.staff_Id,
            //                                               a.name,
            //                                               A.duty,
            //                                               SUM(CASE WHEN B.ATTENDANCE_CODE IN ('B01','B02','B03','C01','C02','C03') THEN B.ATTENDANCE_VALUE*-1 ELSE B.ATTENDANCE_VALUE END ) ZGTS,
            //                                               MAX(SUBSIDY) ZWBZ,
            //                                               ROUND(MAX(SUBSIDY)/TO_NUMBER(TO_CHAR(LAST_DAY(TO_DATE ('{0}', 'yyyymmdd')),'DD'))*SUM(CASE WHEN B.ATTENDANCE_CODE IN ('B01','B02','B03','C01','C02','C03') THEN B.ATTENDANCE_VALUE*-1 ELSE B.ATTENDANCE_VALUE END ),2) ZWJT
            //                                        FROM   rlzy.new_staff_info A,                                               --人员信息表
            //                                               rlzy.QU_ATTENDANCE_DEPT B,                                           --考勤信息表
            //                                               rlzy.ATTENDANCE_MEMO_DICT C                                          --考勤职务
            //                                       WHERE       A.STAFF_ID = B.STAFF_ID                                          --人员标号
            //                                               AND A.duty=c.memo                                                    --职务
            //                                               AND A.DUTY IN ('主任', '副主任', '助理员', '护士长', '助理员')         --
            //                                               AND b.year_month = TO_DATE ('{0}', 'yyyymmdd')
            //                                               {1}
            //                                    GROUP BY   B.dept_code,
            //                                               B.dept_name,
            //                                               a.staff_Id,
            //                                               a.name,
            //                                               A.duty", datetime, deptcodes);

            str.AppendFormat(@"       SELECT   B.dept_code,
                                               A.dept_name,
                                               a.staff_Id,
                                               a.name,
                                               A.duty,
                                               SUM(A01-A02-A03-A04-A05-A06-A07-A08-A09-B01-B02-B03-C01-C02-C03) ZGTS,
                                               MAX(SUBSIDY) ZWBZ,
                                               ROUND(MAX(SUBSIDY)/TO_NUMBER(TO_CHAR(LAST_DAY(TO_DATE ('{0}', 'yyyymmdd')),'DD'))*SUM(A01-A02-A03-A04-A05-A06-A07-A08-A09-B01-B02-B03-C01-C02-C03),2) ZWJT
                                        FROM   rlzy.new_staff_info A,                                               --人员信息表
                                               hisdata.QU_ATTENDANCE_DEPT_VIEW B,                                   --考勤信息表
                                               rlzy.ATTENDANCE_MEMO_DICT C                                          --考勤职务
                                       WHERE       A.EMP_NO = B.EMP_NO                                              --人员标号
                                               AND A.duty=c.memo                                                    --职务
                                               --AND A.DUTY IN ('主任', '副主任', '助理员', '护士长', '助理员')         --
                                               AND b.year_month = last_day(TO_DATE ('{0}', 'yyyymmdd'))
                                               AND C.SUBSIDY>0
                                               {1}
                                    GROUP BY   B.dept_code,
                                               A.dept_name,
                                               a.staff_Id,
                                               a.name,
                                               A.duty", datetime, deptcodes);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 保存人员信息
        /// </summary>
        /// <param name="othervo"></param>
        /// <param name="add_mark"></param>
        /// <param name="Guardcaus"></param>
        /// <param name="GuardTime"></param>
        /// <param name="isBraid"></param>
        /// <param name="jwUserName"></param>
        /// <param name="UserId"></param>
        /// <param name="lzdate"></param>
        public void InsertStaffInfo(StaffInfo othervo, string add_mark, string Guardcaus, string GuardTime, string isBraid, string jwUserName, string UserId, string lzdate)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.NEW_STAFF_INFO
                                            (STAFF_ID,EMP_NO, DEPT_CODE,DEPT_NAME, NAME,IF_ARMY,ISONGUARD,
                                             BIRTHDAY,SEX,NATIONALS,BONUS_FACTOR,
                                             GOVERNMENT_ALLOWANCE ,CADRES_CATEGORIES,STUDY_OVER_DATE,DEPT_TYPE,
                                             TOPEDUCATE,STUDY_SPECSORT,INHOSPITALDATE,
                                             BASEPAY,RETAINTERM, JOB,
                                             JOBDATE,STAFFSORT,BEENROLLEDINDATE,
                                             WORKDATE,DUTY,DUTYDATE,
                                             TECHINCCLASS,TECHNICCLASSDATE,CIVILSERVICECLASS,
                                             CIVILSERVICECLASSDATE,SANTSPECSORT,ROOTSPECSORT,MEDICARDMARK,
                                             MEDICARD,INPUT_USER,INPUT_DATE,USER_DATE,ADD_MARK,HOMEPLACE,CERTIFICATE_NO,
                                             MARITAL_STATUS,TITLE_LIST,EDU1,GRADUATE_ACADEMY,DATE_OF_GRADETITLE,RANK,TITLE,
                                             GROUP_ID,MEMO,MARK_USER,JW_USER_NAME,USER_ID,INPUT_CODE,IMG_ID,
                                             JOB_TITLE,TITLE_DATE,EXPERT,CREDITHOUR_PERYEAR,LEADTECN
                                            ,STATION_CODE,GUARDCAUS,GUARDTIME,ISBRAID,GORD_CODE,TURNOVER_TIME,CONTRACT_START,CONTRACT_END,BONUS_FLAG,CHECK_FLAG,BANK_CODE
                                             )
                                            VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,
                                            ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.RLZY);
            string id = OracleOledbBase.GetMaxID("STAFF_ID", DataUser.RLZY + ".NEW_STAFF_INFO").ToString();
            OleDbParameter[] cmdPara = new OleDbParameter[]{
															 new OleDbParameter("",id),
                                                             new OleDbParameter("EMP_NO",othervo.Emp_no),
															 new OleDbParameter("DEPT_CODE",othervo.DeptCode),
															 new OleDbParameter("DEPT_NAME",othervo.DeptName),
															 new OleDbParameter("NAME",othervo.Name),
															 new OleDbParameter("IF_ARMY",othervo.ifArmy),
															 new OleDbParameter("ISONGUARD",othervo.IsOnGuard),
															 new OleDbParameter("BIRTHDAY",othervo.Birthday),
															 new OleDbParameter("SEX",othervo.Sex),
															 new OleDbParameter("NATIONALS",othervo.NATIONAL),										
															 new OleDbParameter("BONUS_FACTOR",othervo.BONUS_FACTOR),
															 new OleDbParameter("GOVERNMENT_ALLOWANCE",othervo.GOVERNMENT_ALLOWANCE),
															 new OleDbParameter("CADRES_CATEGORIES",othervo.CADRES_CATEGORIES),
															 new OleDbParameter("STUDY_OVER_DATE",othervo.STUDY_OVER_DATE),
															 new OleDbParameter("DEPT_TYPE",othervo.DEPT_TYPE),
															 new OleDbParameter("TOPEDUCATE",othervo.TOPEDUCATE),
															 new OleDbParameter("STUDY_SPECSORT",othervo.STUDY_SpecSort),
															 new OleDbParameter("INHOSPITALDATE",othervo.InHospitalDate),
															 new OleDbParameter("BASEPAY",othervo.BasePay),
															 new OleDbParameter("RETAINTERM",othervo.RetainTerm),
															 new OleDbParameter("JOB",othervo.Job),
															 new OleDbParameter("JOBDATE",othervo.JobDate),
															 new OleDbParameter("STAFFSORT",othervo.StaffSort),
															 new OleDbParameter("BEENROLLEDINDATE",othervo.BeEnrolledInDate),
															 new OleDbParameter("WORKDATE",othervo.WrokDate),
															 new OleDbParameter("DUTY",othervo.Duty),
															 new OleDbParameter("DUTYDATE",othervo.DutyDate),
															 new OleDbParameter("TECHINCCLASS",othervo.TechnicClass),
															 new OleDbParameter("TECHNICCLASSDATE",othervo.TechnicClassDate),
															 new OleDbParameter("CIVILSERVICECLASS",othervo.CivilServiceClass),
															 new OleDbParameter("CIVILSERVICECLASSDATE",othervo.CivilServiceClassDate),
															 new OleDbParameter("SANTSPECSORT",othervo.SantSpecSort),
															 new OleDbParameter("ROOTSPECSORT",othervo.RootSpecSort),
															 new OleDbParameter("MEDICARDMARK",othervo.MediCardMark),
															 new OleDbParameter("MEDICARD",othervo.MediCard),
															 new OleDbParameter("INPUT_USER",othervo.INPUT_USER),
															 new OleDbParameter("INPUT_DATE",othervo.INPUT_DATE),
															 new OleDbParameter("USER_DATE",othervo.USER_DATE),
															 new OleDbParameter ("ADD_MARK",add_mark),
															 new OleDbParameter("HOMEPLACE",othervo.HOMEPLACE),
															 new OleDbParameter("CERTIFICATE_NO",othervo.CERTIFICATE_NO),
															 new OleDbParameter("MARITAL_STATUS",othervo.MARITAL_STATUS),
															 new OleDbParameter("TITLE_LIST",othervo.TITLE_LIST),
															 new OleDbParameter("EDU1",othervo.Edu1),
															 new OleDbParameter("GRADUATE_ACADEMY",othervo.GRADUATE_ACADEMY),
															 new OleDbParameter("DATE_OF_GRADETITLE",othervo.DATE_OF_GRADETITLE),
															 new OleDbParameter("RANK",othervo.RANK),
															 new OleDbParameter("TITLE",othervo.TITLE),
															 new OleDbParameter("DEPTGROUP",othervo.DEPTGROUP),
															 new OleDbParameter("MEMO",othervo.MEMO),
															 new OleDbParameter("MARK_USER",othervo.MARK_USER),
															 new OleDbParameter("JW_USER_NAME",jwUserName),
															 new OleDbParameter("USER_ID",UserId),
															 new OleDbParameter("INPUT_CODE",othervo.INPUT_CODE),
															 new OleDbParameter("IMG_ID",othervo.IMG_ID),
															 new OleDbParameter("JOB_TITLE",othervo.JOB_TITLE),
															 new OleDbParameter("TITLE_DATE",othervo.TITLE_DATE),
															 new OleDbParameter("EXPERT",othervo.EXPERT),
															 new OleDbParameter("Credithour_PerYear",othervo.CREDITHOUR_PERYEAR),
				                                             new OleDbParameter("LEADTECN",othervo.LEADTECN),
															 new OleDbParameter("station_code",othervo.Station_Code),
                                                             new OleDbParameter("GUARDCAUS",Guardcaus),
                                                             new OleDbParameter("GUARDTIME",GuardTime),
                                                             new OleDbParameter("ISBRAID",isBraid),
                                                             new OleDbParameter("GORD_CODE",othervo.Gord),
                                                             new OleDbParameter("TURNOVER_TIME",lzdate),
                                                             new OleDbParameter("CONTRACT_START",othervo.Contractstart),
                                                             new OleDbParameter("CONTRACT_END",othervo.Contractend),
                                                             new OleDbParameter("BONUS_FLAG",othervo.Bonusflag),
                                                             new OleDbParameter("CHECK_FLAG",othervo.Checkflag),
                                                             new OleDbParameter("BANK_CODE",othervo.BONUSNUM)
														 };

            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="USER_ID"></param>
        /// <param name="USER_NAME"></param>
        /// <param name="USER_DEPT"></param>
        public void AddHiaUser(string USER_ID, string USER_NAME, string USER_DEPT)
        {
            string password = "0B58CEC40BBDF170";
            string state = "1";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"insert into hisdata.users_NEW(DB_USER,USER_ID,USER_NAME,USER_DEPT,CREATE_DATE,PASSWORD,STATE)
                                            VALUES (?,?,?,?,sysdate,?,?)", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]{
															 new OleDbParameter("DB_USER",USER_ID),
                                                             new OleDbParameter("USER_ID",USER_ID),
															 new OleDbParameter("USER_NAME",USER_NAME),
															 new OleDbParameter("USER_DEPT",USER_DEPT),
															 new OleDbParameter("PASSWORD",password),
															 new OleDbParameter("STATE",state)
															
														 };

            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="USER_ID"></param>
        public void updateHiaUser(string USER_ID)
        {
            string password = "0B58CEC40BBDF170";
            string state = "1";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"update hisdata.users set PASSWORD =? , STATE =? where USER_ID =? ", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]{
															 new OleDbParameter("PASSWORD",password),
                                                             new OleDbParameter("STATE",state),
                                                             new OleDbParameter("USER_ID",USER_ID)
														
															
														 };

            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 更新人员信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="othervo"></param>
        /// <param name="markflag"></param>
        public void UpdateStaffInfo(string id, StaffInfo othervo, string add_mark, string Guardcaus, string GuardTime, string isBraid, string jwuserName, string user_id, string input_code, string lzdate)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.NEW_STAFF_INFO
											SET  EMP_NO=?,NAME=?,IF_ARMY=?,ISONGUARD=?,
                                             BIRTHDAY=?,SEX=?,NATIONALS=?,BONUS_FACTOR=?,
                                             GOVERNMENT_ALLOWANCE=? ,CADRES_CATEGORIES=?,STUDY_OVER_DATE=?,DEPT_TYPE=?,
                                             TOPEDUCATE=?,STUDY_SPECSORT=?,INHOSPITALDATE=?,
                                             BASEPAY=?,RETAINTERM=?, JOB=?,
                                             JOBDATE=?,STAFFSORT=?,BEENROLLEDINDATE=?,
                                             WORKDATE=?,DUTY=?,DUTYDATE=?,
                                             TECHINCCLASS=?,TECHNICCLASSDATE=?,CIVILSERVICECLASS=?,
                                             CIVILSERVICECLASSDATE=?,SANTSPECSORT=?,ROOTSPECSORT=?,MEDICARDMARK=?,
                                             MEDICARD=?,ADD_MARK=?,HOMEPLACE=?,CERTIFICATE_NO=?,MARITAL_STATUS=?,
                                             TITLE_LIST=?,EDU1=?,GRADUATE_ACADEMY=?,DATE_OF_GRADETITLE=?,RANK=?,TITLE=?,USER_DATE=?,
                                             GROUP_ID=?,MEMO=?,MARK_USER=?,JW_USER_NAME=?,USER_ID=?,INPUT_CODE=?,IMG_ID=?,JOB_TITLE=?,TITLE_DATE=?,
                                             EXPERT=?,CREDITHOUR_PERYEAR=?,LEADTECN=?,
											 STATION_CODE=?,GUARDCAUS=?,GUARDTIME=?,ISBRAID=?,GORD_CODE=?,TURNOVER_TIME=?,CONTRACT_START=?,CONTRACT_END=?,BONUS_FLAG=?,CHECK_FLAG=?,BANK_CODE=?
											 WHERE STAFF_ID=?", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]{	
					                                             new OleDbParameter("EMP_NO",othervo.Emp_no),							 
																 new OleDbParameter("NAME",othervo.Name),
																 new OleDbParameter("IF_ARMY",othervo.ifArmy),
																 new OleDbParameter("ISONGUARD",othervo.IsOnGuard),
																 new OleDbParameter("BIRTHDAY",othervo.Birthday),
																 new OleDbParameter("SEX",othervo.Sex),
																 new OleDbParameter("NATIONALS",othervo.NATIONAL),										
																 new OleDbParameter("BONUS_FACTOR",othervo.BONUS_FACTOR),
																 new OleDbParameter("GOVERNMENT_ALLOWANCE",othervo.GOVERNMENT_ALLOWANCE),
																 new OleDbParameter("CADRES_CATEGORIES",othervo.CADRES_CATEGORIES),
																 new OleDbParameter("STUDY_OVER_DATE",othervo.STUDY_OVER_DATE),
																 new OleDbParameter("DEPT_TYPE",othervo.DEPT_TYPE),
																 new OleDbParameter("TOPEDUCATE",othervo.TOPEDUCATE),
																 new OleDbParameter("STUDY_SPECSORT",othervo.STUDY_SpecSort),
																 new OleDbParameter("INHOSPITALDATE",othervo.InHospitalDate),
																 new OleDbParameter("BASEPAY",othervo.BasePay),
																 new OleDbParameter("RETAINTERM",othervo.RetainTerm),
																 new OleDbParameter("JOB",othervo.Job),
																 new OleDbParameter("JOBDATE",othervo.JobDate),
																 new OleDbParameter("STAFFSORT",othervo.StaffSort),
																 new OleDbParameter("BEENROLLEDINDATE",othervo.BeEnrolledInDate),
																 new OleDbParameter("WORKDATE",othervo.WrokDate),
																 new OleDbParameter("DUTY",othervo.Duty),
																 new OleDbParameter("DUTYDATE",othervo.DutyDate),
																 new OleDbParameter("TECHINCCLASS",othervo.TechnicClass),
																 new OleDbParameter("TECHNICCLASSDATE",othervo.TechnicClassDate),
																 new OleDbParameter("CIVILSERVICECLASS",othervo.CivilServiceClass),
																 new OleDbParameter("CIVILSERVICECLASSDATE",othervo.CivilServiceClassDate),
																 new OleDbParameter("SANTSPECSORT",othervo.SantSpecSort),
																 new OleDbParameter("ROOTSPECSORT",othervo.RootSpecSort),
																 new OleDbParameter("MEDICARDMARK",othervo.MediCardMark),
																 new OleDbParameter("MEDICARD",othervo.MediCard),
																 new OleDbParameter("ADD_MARK",add_mark),
																 new OleDbParameter("HOMEPLACE",othervo.HOMEPLACE),
																 new OleDbParameter("CERTIFICATE_NO",othervo.CERTIFICATE_NO),
																 new OleDbParameter("MARITAL_STATUS",othervo.MARITAL_STATUS),
																 new OleDbParameter("TITLE_LIST",othervo.TITLE_LIST),
																 new OleDbParameter("EDU1",othervo.Edu1),
																 new OleDbParameter("GRADUATE_ACADEMY",othervo.GRADUATE_ACADEMY),
																 new OleDbParameter("DATE_OF_GRADETITLE",othervo.DATE_OF_GRADETITLE),
																 new OleDbParameter("RANK",othervo.RANK),
																 new OleDbParameter("TITLE",othervo.TITLE),
																 new OleDbParameter("USER_DATE",othervo.USER_DATE),
																 new OleDbParameter("DEPTGROUP",othervo.DEPTGROUP),
																 new OleDbParameter("MEMO",othervo.MEMO),
																 new OleDbParameter("MARK_USER",othervo.MARK_USER),
                                                                 new OleDbParameter("STAFF_INPUT",jwuserName),
                                                                 new OleDbParameter("user_id",user_id),
                                                                 new OleDbParameter("INPUT_CODE",input_code),
																 new OleDbParameter("IMG_ID",othervo.IMG_ID),
																 new OleDbParameter("JOB_TITLE",othervo.JOB_TITLE),
																 new OleDbParameter("TITLE_DATE",othervo.TITLE_DATE),
																 new OleDbParameter("EXPERT",othervo.EXPERT),
																 new OleDbParameter("Credithour_PerYear",othervo.CREDITHOUR_PERYEAR),
																 new OleDbParameter("",othervo.LEADTECN),
																 new OleDbParameter("station_code",othervo.Station_Code),
                                                                 new OleDbParameter("",Guardcaus),
																 new OleDbParameter("",GuardTime),
                                                                 new OleDbParameter("",isBraid),
                                                                 new OleDbParameter("GORD_CODE",othervo.Gord),
                                                                 new OleDbParameter("TURNOVER_TIME",lzdate),
                                                                 new OleDbParameter("CONTRACT_START",othervo.Contractstart),
                                                                 new OleDbParameter("CONTRACT_END",othervo.Contractend),

                                                                 new OleDbParameter("BONUS_FLAG",othervo.Bonusflag),
                                                                 new OleDbParameter("CHECK_FLAG",othervo.Checkflag),
                                                                 new OleDbParameter("BANK_CODE",othervo.BONUSNUM),
																 new OleDbParameter("STAFF_ID",id)
															 };
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 查询科室人员姓名重复
        /// </summary>
        /// <param name="deptCode">科室代码</param>
        /// <returns>true:存在</returns>
        public bool isExStaffInfoByName(string deptCode, string name)
        {
            string Code = deptCode == "" ? "" : " AND DEPT_CODE = '" + deptCode + "'";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT COUNT(NAME) FROM {0}.NEW_STAFF_INFO WHERE (ADD_MARK='0' OR ADD_MARK='1' OR ADD_MARK='3')
                               AND NAME='{1}' {2}", DataUser.RLZY, name, Code);
            int i = Convert.ToInt32(OracleOledbBase.GetSingle(str.ToString()));
            return i > 0 ? true : false;
        }

        /// <summary>
        /// 查询科室人员姓名重复
        /// </summary>
        /// <param name="deptCode">科室代码</param>
        /// <returns>true:存在</returns>
        public bool isExStaffInfoByName(string deptCode, string name, string staff_id)
        {
            string Code = deptCode == "" ? "" : " AND DEPT_CODE = '" + deptCode + "'";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT COUNT(NAME) FROM {0}.NEW_STAFF_INFO WHERE (ADD_MARK='0' OR ADD_MARK='1' OR ADD_MARK='3')
                               AND NAME='{1}' AND STAFF_ID <> '{3}' {2}", DataUser.RLZY, name, Code, staff_id);
            int i = Convert.ToInt32(OracleOledbBase.GetSingle(str.ToString()));
            return i > 0 ? true : false;
        }

        /// <summary>
        /// 查询科室人员USERID重复
        /// </summary>
        /// <param name="deptCode">科室代码</param>
        /// <returns>true:存在</returns>
        public bool isExUserInfoByUserId(string userid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT COUNT(NAME) FROM {0}.NEW_STAFF_INFO WHERE
                                USER_ID='{1}' AND ADD_MARK <> '2'", DataUser.RLZY, userid.Split('&')[0].ToString());
            int i = Convert.ToInt32(OracleOledbBase.GetSingle(str.ToString()));
            return i > 0 ? true : false;
        }

        /// <summary>
        /// 查询科室人员USERID重复
        /// </summary>
        /// <param name="deptCode">科室代码</param>
        /// <returns>true:存在</returns>
        public bool isExUserInfoByUserId(string userid, string staff_id)
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT COUNT(NAME) FROM {0}.NEW_STAFF_INFO WHERE
                                USER_ID='{1}' AND ADD_MARK <> '2' AND STAFF_ID <> '{2}'", DataUser.RLZY, userid.Split('&')[0].ToString(), staff_id);
            int i = Convert.ToInt32(OracleOledbBase.GetSingle(str.ToString()));
            return i > 0 ? true : false;
        }


        public bool isExUserByUserId(string userid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT COUNT(*) FROM {0}.USERS WHERE
                                USER_ID='{1}'", DataUser.HISFACT, userid.Split('&')[0].ToString());
            int i = Convert.ToInt32(OracleOledbBase.GetSingle(str.ToString()));
            return i > 0 ? true : false;
        }


        /// <summary>
        /// 人员调动信息查询
        /// </summary>
        /// <param name="DeptCode">科室代码</param>
        /// <returns></returns>
        public DataSet ViewStaffDeptInfo(string DeptCode)
        {
            string selectDeptStr = DeptCode == "" ? "" : " AND a.DEPT_CODE = '" + DeptCode + "'";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   a.STAFF_ID,a.NAME, a.JOB, a.DUTY,a.DEPT_NAME, a.SEX, a.BIRTHDAY, a.STAFFSORT, a.DUTYDATE,
                                         a.WORKDATE, a.JOBDATE,a.TOPEDUCATE,a.DEPT_CODE,b.FROM_DEPT_NAME,b.CHANGE_DATE
                               FROM {0}.NEW_STAFF_INFO a,{0}.USER_CHANGE b
                               WHERE ADD_MARK = '1' {1} and  a.staff_id=b.staff_id(+)
                               ORDER BY NAME", DataUser.RLZY, selectDeptStr);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 人员调动数据插入
        /// </summary>
        public void InsertStaffChangeInfo(string staffid, string old_user_name, string from_dept_code, string from_dept_name, string new_user_name, string to_dept_code, string to_dept_name, string input_user, string staff_id, string user_id)
        {
            MyLists list = new MyLists();

            List l_listUpDate = new List();
            l_listUpDate.Parameters = new OleDbParameter[] { new OleDbParameter() };
            l_listUpDate.StrSql = UpDateStaffNameInfoInNewSatffInfo(new_user_name, to_dept_code, to_dept_name, staffid);
            list.Add(l_listUpDate);

            List insert = new List();
            insert.Parameters = new OleDbParameter[] { new OleDbParameter() };
            insert.StrSql = InsertStaffInfoChangeInUserChange(old_user_name, from_dept_code, from_dept_name, new_user_name, to_dept_code, to_dept_name, input_user, staff_id, user_id);
            list.Add(insert);
            OracleOledbBase.ExecuteTranslist(list);
        }

        /// <summary>
        /// 变更人员科室
        /// </summary>
        /// <param name="new_user_name"></param>
        /// <param name="to_dept_code"></param>
        /// <param name="to_dept_name"></param>
        /// <param name="staffid"></param>
        /// <returns></returns>
        public string UpDateStaffNameInfoInNewSatffInfo(string new_user_name, string to_dept_code, string to_dept_name, string staffid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE {0}.NEW_STAFF_INFO 
                                        SET NAME='{1}',DEPT_CODE='{2}',DEPT_NAME='{3}',
                                        USER_DATE='{4}',CHANGE_FLAG='1' WHERE STAFF_ID='{5}'", DataUser.RLZY, new_user_name, to_dept_code, to_dept_name, (System.DateTime.Now.ToString("yyyyMM") + "01"), staffid);
            return str.ToString();
        }

        /// <summary>
        /// 记录人员转科记录
        /// </summary>
        /// <param name="old_user_name"></param>
        /// <param name="from_dept_code"></param>
        /// <param name="from_dept_name"></param>
        /// <param name="new_user_name"></param>
        /// <param name="to_dept_code"></param>
        /// <param name="to_dept_name"></param>
        /// <param name="input_user"></param>
        /// <param name="staff_id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public string InsertStaffInfoChangeInUserChange(string old_user_name, string from_dept_code, string from_dept_name, string new_user_name, string to_dept_code, string to_dept_name, string input_user, string staff_id, string user_id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO {0}.USER_CHANGE 
                                     (ID,CHANGE_DATE,OLE_USER_NAME,NEW_USER_NAME,FROM_DEPT_CODE,FROM_DEPT_NAME,
                                      TO_DEPT_CODE,TO_DEPT_NAME,INPUT_USER,STAFF_ID,USER_ID) 
                                      VALUES 
                                      ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                       DataUser.RLZY, OracleOledbBase.GetMaxID("ID", DataUser.RLZY + ".USER_CHANGE"),
                       System.DateTime.Now.ToString("yyyyMMdd"), old_user_name, new_user_name, from_dept_code, from_dept_name,
                       to_dept_code, to_dept_name, input_user, staff_id, user_id);
            return str.ToString();
        }

        /// <summary>
        /// 科室专科中心查询
        /// </summary>
        /// <param name="SpecCenterCode">专科中心代码</param>
        /// <param name="year">年度</param>
        /// <returns></returns>
        public DataSet ViewSpecCenterListInfo(string SpecCenterCode, string year)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT ID,STAT_MONTH, UNIT_CODE, UNIT_NAME, UNIT_TYPE, SPEC_CODE, SPEC_NAME,
                                   SPEC_TYPE, SPEC_LEAD_NAME, SPEC_LEAD_AGE, SPEC_LEAD_EDU,
                                   SPEC_LEAD_TITLE, SPEC_LEAD_DUTY, APPRO_BED, OPEN_BED, PERS_NUM, DR_NUM,
                                   MASTER_NUM, UNDER_NUM, JUNIOR_NUM, TECH_NUM, GOABRBRING, EXPERTPREN,
                                   SHOWDR, SHOWMASTER, FETCHDR, FETCHMASTER, BRING_TYPE, BRING_NUM,
                                   NEWITEMNUM, FETCHITEMNUM, INAUGITEMNUM, ABSITEMNUM, OTHERITEMNUM,
                                   SPEC_LEAD_BIRTHDATE, ENTER_DATE
                              FROM {0}.SPEC_CENTER 
                              WHERE STAT_MONTH='{2}' AND SPEC_CODE = '{1}'", DataUser.RLZY, SpecCenterCode, year);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 添加专科中心信息
        /// </summary>		
        public void InsertSpecCenterList(string STAT_MONTH, string UNIT_CODE, string UNIT_NAME, string UNIT_TYPE,
                                          string SPEC_CODE, string SPEC_NAME, string SPEC_TYPE,
                                          string SPEC_LEAD_NAME, string SPEC_LEAD_AGE, string SPEC_LEAD_EDU,
                                          string SPEC_LEAD_TITLE, string SPEC_LEAD_DUTY, string APPRO_BED,
                                          string OPEN_BED, string PERS_NUM, string DR_NUM, string MASTER_NUM,
                                          string UNDER_NUM, string JUNIOR_NUM, string TECH_NUM, string GOABRBRING,
                                          string EXPERTPREN, string SHOWDR, string SHOWMASTER, string FETCHDR,
                                          string FETCHMASTER, string BRING_TYPE, string BRING_NUM, string NEWITEMNUM,
                                          string FETCHITEMNUM, string INAUGITEMNUM, string ABSITEMNUM, string OTHERITEMNUM,
                                          string ENTER_DATE, string SPEC_LEAD_BIRTHDATE)
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.SPEC_CENTER( ID, STAT_MONTH, UNIT_CODE, UNIT_NAME, UNIT_TYPE, SPEC_CODE, SPEC_NAME,
                                                            SPEC_TYPE, SPEC_LEAD_NAME, SPEC_LEAD_AGE, SPEC_LEAD_EDU, SPEC_LEAD_TITLE, SPEC_LEAD_DUTY, APPRO_BED,
                                                            OPEN_BED, PERS_NUM, DR_NUM, MASTER_NUM, UNDER_NUM, JUNIOR_NUM, TECH_NUM, GOABRBRING, EXPERTPREN,
                                                            SHOWDR, SHOWMASTER, FETCHDR, FETCHMASTER, BRING_TYPE, BRING_NUM, NEWITEMNUM, FETCHITEMNUM,
                                                            INAUGITEMNUM, ABSITEMNUM, OTHERITEMNUM, 
                                                            ENTER_DATE, SPEC_LEAD_BIRTHDATE )
                                                 VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.RLZY);

            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , OracleOledbBase.GetMaxID("ID",DataUser.RLZY+".SPEC_CENTER") ) ,
					new OleDbParameter( "STAT_MONTH" , STAT_MONTH ) ,
					new OleDbParameter( "UNIT_CODE" , UNIT_CODE ) ,
					new OleDbParameter( "UNIT_NAME" , UNIT_NAME ) ,
					new OleDbParameter( "UNIT_TYPE" , UNIT_TYPE ) ,
					new OleDbParameter( "SPEC_CODE" , SPEC_CODE ) ,
					new OleDbParameter( "SPEC_NAME", SPEC_NAME ),
					new OleDbParameter( "SPEC_TYPE" , SPEC_TYPE ) ,
					new OleDbParameter( "SPEC_LEAD_NAME", SPEC_LEAD_NAME),
                    new OleDbParameter( "SPEC_LEAD_AGE", SPEC_LEAD_AGE),
                    new OleDbParameter( "SPEC_LEAD_EDU", SPEC_LEAD_EDU),
                    new OleDbParameter( "SPEC_LEAD_TITLE", SPEC_LEAD_TITLE),
                    new OleDbParameter( "SPEC_LEAD_DUTY", SPEC_LEAD_DUTY),
                    new OleDbParameter( "APPRO_BED", APPRO_BED),
                    new OleDbParameter( "OPEN_BED", OPEN_BED),
                    new OleDbParameter( "PERS_NUM", PERS_NUM),
					new OleDbParameter( "DR_NUM" , DR_NUM ) ,
					new OleDbParameter( "MASTER_NUM" , MASTER_NUM ) ,
					new OleDbParameter( "UNDER_NUM" , UNDER_NUM ) ,
					new OleDbParameter( "JUNIOR_NUM", JUNIOR_NUM ),
					new OleDbParameter( "TECH_NUM" , TECH_NUM ) ,
					new OleDbParameter( "GOABRBRING", GOABRBRING),
					new OleDbParameter( "EXPERTPREN", EXPERTPREN),
					new OleDbParameter( "SHOWDR", SHOWDR),
					new OleDbParameter( "SHOWMASTER", SHOWMASTER),
					new OleDbParameter( "FETCHDR", FETCHDR),
					new OleDbParameter( "FETCHMASTER", FETCHMASTER),
					new OleDbParameter( "BRING_TYPE" , BRING_TYPE ) ,
					new OleDbParameter( "BRING_NUM" , BRING_NUM ) ,
					new OleDbParameter( "NEWITEMNUM" , NEWITEMNUM ) ,
					new OleDbParameter( "FETCHITEMNUM", FETCHITEMNUM ),
					new OleDbParameter( "INAUGITEMNUM" , INAUGITEMNUM ) ,
					new OleDbParameter( "ABSITEMNUM", ABSITEMNUM),
					new OleDbParameter( "OTHERITEMNUM", OTHERITEMNUM),
				    new OleDbParameter( "ENTER_DATE", ENTER_DATE),
					new OleDbParameter( "SPEC_LEAD_BIRTHDATE", SPEC_LEAD_BIRTHDATE)
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 删除专科中心信息
        /// </summary>
        /// <param name="statMonth"></param>
        /// <param name="SpecCenterCode"></param>
        public void DeleteSpecCenterList(string statMonth, string SpecCenterCode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE {0}.SPEC_CENTER WHERE STAT_MONTH='{1}' AND SPEC_CODE='{2}'", DataUser.RLZY, statMonth, SpecCenterCode);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }



        /// <summary>
        /// 请假查询
        /// </summary>
        /// <param name="deptCode">科室代码</param>
        /// <param name="year">日期</param>
        /// <param name="isCases">true:不在岗</param>
        /// <returns></returns>
        public DataSet ViewStaffStateListInfo(string deptCode, string date, string power, bool isCases, string staffSort)
        {
            string Cases = isCases ? "AND CASES <> '在岗'" : "";
            deptCode = deptCode == "" ? "" : "AND T1.DEPT_CODE = '" + deptCode + "'";
            power = power == "" ? "" : "AND T1.DEPT_CODE IN (" + power + ")";
            staffSort = staffSort == "" ? "" : "AND T1.STAFFSORT IN (" + staffSort + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT T1.STAFF_ID, T1.DEPT_CODE, T1.DEPT_NAME, T1.NAME, T1.ADD_MARK,T1.JOB,T1.STAFFSORT,
                                       T1.ISONGUARD, NVL(T2.CASES,'在岗') CASES,T2.PLACE,T2.PERSON,T2.STRAT_DATE,T2.END_DATE,T2.ID
                                  FROM {0}.NEW_STAFF_INFO T1,
                                       (SELECT  T3.CASES,T3.PLACE,T3.PERSON,T3.STRAT_DATE,T3.END_DATE,T3.ID,T3.DEPT_CODE,T3.USER_NAME
                                          FROM {0}.ONGUARD_CASE T3
                                         WHERE T3.STRAT_DATE = '{2}'
                                            OR (T3.STRAT_DATE < '{2}' AND T3.END_DATE IS NULL)) T2
                                 WHERE ISONGUARD = '是'
                                   AND T2.USER_NAME(+) = T1.NAME
                                   AND T2.DEPT_CODE(+) = T1.DEPT_CODE
                                   AND T1.ADD_MARK = '1' {1} {3} {4} {5}", DataUser.RLZY, deptCode, date, Cases, power, staffSort);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }



        /// <summary>
        /// 插入请假信息
        /// </summary>
        /// <param name="deptcode">部门ID</param>
        /// <param name="name">人员姓名</param>
        /// <param name="datetime">开始请假时间</param>
        /// <param name="content">请假类别</param>
        /// <param name="place">地点</param>
        /// <param name="person">批准人</param>
        /// <param name="user_id">人员姓名</param>
        public void InserteCase(string deptcode, string name, string datetime, string content, string place, string person)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO {0}.ONGUARD_CASE(ID,CASES,STRAT_DATE,DEPT_CODE,USER_NAME,PLACE,PERSON) VALUES (?,?,?,?,?,?,?)", DataUser.RLZY);
            OleDbParameter[] cmdpara = new OleDbParameter[]
				{
					new OleDbParameter("",OracleOledbBase.GetMaxID("ID",DataUser.RLZY+".ONGUARD_CASE")),
					new OleDbParameter( "CASES" , content ) ,
					new OleDbParameter( "DATETIME" , datetime ),
					new OleDbParameter( "DEPT_CODE" , deptcode ) ,
					new OleDbParameter( "USER_NAME" , name ),
					new OleDbParameter( "PLACE" , place ),
					new OleDbParameter( "PERSON" , person )
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdpara);
        }


        /// <summary>
        /// 插入销假信息
        /// </summary>
        /// <param name="datetime">销假时间</param>
        /// <param name="id"></param>
        public void UpdateCase(string dates, string datetime, string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE {0}.ONGUARD_CASE  SET DATES=? , END_DATE=? WHERE ID = ?", DataUser.RLZY);
            OleDbParameter[] cmdpara = new OleDbParameter[]
				{
					new OleDbParameter( "DATES" , dates ),
					new OleDbParameter( "DATETIME" , datetime ),
					new OleDbParameter( "ID" , id )
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdpara);
        }

        /// <summary>
        /// 人员在岗天数信息
        /// </summary>
        /// <param name="datetime">月初日期</param>
        /// <param name="datetime">月末日期</param>
        /// <param name="id"></param>
        public DataSet ViewOnGuardDays(string fromDate, string toDate, string staffSort, string MonthDays, string power, string staffPower)
        {
            staffSort = staffSort == "" ? "" : "AND T1.STAFFSORT IN (" + staffSort + ")";
            power = power == "" ? "" : "AND T1.DEPT_CODE IN (" + power + ")";
            StringBuilder str = new StringBuilder();

            StringBuilder str1 = new StringBuilder();

            str1.AppendFormat(@"SELECT MAX(CASES) CASES,
                                     DEPT_CODE,
                                     USER_NAME,
                                     MAX(PLACE) PLACE,
                                     MAX(PERSON) PERSON,
                                     MAX(ID) ID,
SUM(DECODE ( SIGN(TO_DATE( NVL(END_DATE,'{2}'),'YYYY-MM-DD') - TO_DATE('{2}','YYYY-MM-DD')),
                                            -1, TO_DATE(END_DATE,'YYYY-MM-DD'),
                                            0, TO_DATE( '{2}','YYYY-MM-DD')+1,TO_DATE( '{2}','YYYY-MM-DD')+1)-                                   
DECODE ( SIGN(TO_DATE( STRAT_DATE,'YYYY-MM-DD') -TO_DATE('{1}','YYYY-MM-DD') ),
                                            -1, TO_DATE('{1}','YYYY-MM-DD'),TO_DATE( STRAT_DATE,'YYYY-MM-DD'))) SDAYS

                                   
                                   FROM {0}.ONGUARD_CASE 
                                   WHERE (TO_DATE (STRAT_DATE, 'YYYY-MM-DD')>=TO_DATE ('{1}', 'YYYY-MM-DD') AND TO_DATE (STRAT_DATE, 'YYYY-MM-DD')<=TO_DATE ('{2}', 'YYYY-MM-DD')) 
                                       OR (TO_DATE (END_DATE, 'YYYY-MM-DD')>=TO_DATE ('{1}', 'YYYY-MM-DD') AND TO_DATE (END_DATE, 'YYYY-MM-DD')<=TO_DATE ('{2}', 'YYYY-MM-DD'))
                                       OR (TO_DATE (STRAT_DATE, 'YYYY-MM-DD') < TO_DATE ('{1}', 'YYYY-MM-DD') AND END_DATE IS NULL)
                                       OR (TO_DATE (STRAT_DATE, 'YYYY-MM-DD') < TO_DATE ('{1}', 'YYYY-MM-DD') AND TO_DATE (END_DATE, 'YYYY-MM-DD') > TO_DATE ('{2}', 'YYYY-MM-DD'))
                                   GROUP BY DEPT_CODE,USER_NAME", DataUser.RLZY, fromDate, toDate);

            str.AppendFormat(@"
                                SELECT T1.STAFF_ID, T1.DEPT_CODE, T1.DEPT_NAME, T1.NAME, T1.ADD_MARK,T1.JOB,T1.STAFFSORT,
                                                                       T1.ISONGUARD, T2.CASES,T2.PLACE,T2.PERSON,NVL(T2.DAYS,'{4}') DAYS,T2.ID
                                                                  FROM {0}.NEW_STAFF_INFO T1,
                                                                       (SELECT AAA.*, {4} - AAA.SDAYS AS DAYS 
                                                                        FROM ({6}) AAA WHERE AAA.SDAYS >= 0) T2
                                                                 WHERE (ISONGUARD = '在岗' or ISONGUARD='是')
                                                                   AND T2.USER_NAME(+) = T1.NAME
                                                                   AND T2.DEPT_CODE(+) = T1.DEPT_CODE
                                                                   AND T1.ADD_MARK = '1' {3} {5}", DataUser.RLZY, fromDate, toDate, staffSort, MonthDays, power, str1.ToString());
            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="staffSort"></param>
        /// <param name="MonthDays"></param>
        /// <param name="power"></param>
        /// <param name="staffPower"></param>
        /// <returns></returns>
        public DataSet ViewOnGuardDays2(string datetime, string enddatetime, string staffSort, string MonthDays, string power, string staffPower, string deptcode)
        {
            //考勤类型
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat(@"SELECT ATTENDANCE_CODE, ATTENDANCE_NAME,ATTENDANCE_CODE kk 
                                FROM RLZY.ATTENDANCE_NAME_DICT a 
                                UNION ALL
                                SELECT REPLACE(to_char(wmsys.wm_concat(case when ATTENDANCE_CODE='A01' THEN ATTENDANCE_CODE ELSE CASE WHEN CAL_TAG='0' THEN '+'||ATTENDANCE_CODE ELSE '-'||ATTENDANCE_CODE END END)),',','') ATTENDANCE_CODE,
                                '实际出勤' ATTENDANCE_NAME,'A0111' KK
                                FROM (SELECT A.* FROM RLZY.ATTENDANCE_NAME_DICT a WHERE CAL_TAG <>2 order by ATTENDANCE_CODE)  ORDER BY KK", DataUser.RLZY);
            DataTable table = OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];

            StringBuilder str = new StringBuilder();
            //人员类别
            staffSort = staffSort == "" ? "" : "AND C.STAFFSORT IN (" + staffSort + ")";
            //科室权限
            power = power == "" ? "" : "AND B.DEPT_CODE IN (" + power + ")";
            //科室
            deptcode = deptcode == "" ? "" : "AND B.DEPT_CODE ='" + deptcode + "'";

            str.AppendFormat("SELECT  A.STAFF_ID,B.ACCOUNT_DEPT_NAME \"科室\", C.NAME \"姓名\",C.DUTY \"行政职务\",C.STAFFSORT \"类别\",C.JOB \"技术职务\",C.TITLE_LIST \"职称序列\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(" ,SUM ({0}) \"{1}\"", table.Rows[i]["ATTENDANCE_CODE"].ToString(), table.Rows[i]["ATTENDANCE_NAME"].ToString());
            }

            str.AppendFormat(@" FROM (SELECT   year_month,
                                               dept_code,
                                               staff_id AS emp_no,");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["ATTENDANCE_NAME"].ToString() != "实际出勤")
                {
                    str.AppendFormat(" SUM(CASE WHEN attendance_code = '{0}' THEN ATTENDANCE_VALUE ELSE 0 END) {0},", table.Rows[i]["ATTENDANCE_CODE"].ToString());
                }
            }

            str.AppendFormat(@"                staff_id as STAFF_ID
                                        FROM   RLZY.QU_ATTENDANCE_DEPT
                                    GROUP BY   year_month, dept_code, staff_id) A,");
            str.AppendFormat(@" 
                                       COMM.SYS_DEPT_DICT B,
                                       RLZY.NEW_STAFF_INFO C
                               WHERE       TO_CHAR (A.YEAR_MONTH, 'YYYYMM') >= '{0}'
                                       AND TO_CHAR (A.YEAR_MONTH, 'YYYYMM') <= '{4}'
                                       AND A.DEPT_CODE = B.DEPT_CODE
                                       AND A.STAFF_ID = C.STAFF_ID
                                       {1} {2} {3}
                            GROUP BY   A.STAFF_ID,
                                       C.NAME,
                                       B.ACCOUNT_DEPT_CODE,
                                       B.ACCOUNT_DEPT_NAME,C.DUTY,C.STAFFSORT,C.JOB,C.TITLE_LIST
                            ORDER BY   B.ACCOUNT_DEPT_CODE,C.NAME", datetime, staffSort, power, deptcode, enddatetime);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="staffSort"></param>
        /// <param name="MonthDays"></param>
        /// <param name="power"></param>
        /// <param name="staffPower"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet ViewOnGuardDays3(string datetime, string enddatetime, string staffSort, string MonthDays, string power, string staffPower, string deptcode)
        {
            //考勤类型
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat(@"SELECT ATTENDANCE_CODE, ATTENDANCE_NAME,ATTENDANCE_CODE kk 
                                FROM RLZY.ATTENDANCE_NAME_DICT a 
                                UNION ALL
                                SELECT REPLACE(to_char(wmsys.wm_concat(case when ATTENDANCE_CODE='A01' THEN ATTENDANCE_CODE ELSE CASE WHEN CAL_TAG='0' THEN '+'||ATTENDANCE_CODE ELSE '-'||ATTENDANCE_CODE END END)),',','') ATTENDANCE_CODE,
                                '实际出勤' ATTENDANCE_NAME,'A0111' KK
                                FROM (SELECT A.* FROM RLZY.ATTENDANCE_NAME_DICT a WHERE CAL_TAG <>2 order by ATTENDANCE_CODE)  ORDER BY KK", DataUser.RLZY);
            DataTable table = OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];

            StringBuilder str = new StringBuilder();
            //人员类别
            staffSort = staffSort == "" ? "" : "AND C.STAFFSORT IN (" + staffSort + ")";
            //科室权限
            power = power == "" ? "" : "AND B.DEPT_CODE IN (" + power + ")";
            string aa = "'20001'";
            //科室
            if (deptcode == "平均奖科室")
            {
                aa = "select dept_code from PERFORMANCE.SET_ACCOUNTDEPTTYPE where dept_type in ('20001') and st_date = to_date('" + datetime + "01','yyyymmdd')";
            }
            else
            {
                aa = "select dept_code from PERFORMANCE.SET_ACCOUNTDEPTTYPE where dept_type in ('40001','50001','60001','80001','30001') and st_date = to_date('" + datetime + "01','yyyymmdd')";
            }
            deptcode = deptcode == "" ? "" : "AND B.DEPT_CODE in (" + aa + ")";

            str.AppendFormat("SELECT  A.STAFF_ID,B.ACCOUNT_DEPT_NAME \"科室\", C.NAME \"姓名\",C.DUTY \"行政职务\",C.STAFFSORT \"类别\",C.JOB \"技术职务\",C.TITLE_LIST \"职称序列\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(" ,SUM ({0}) \"{1}\"", table.Rows[i]["ATTENDANCE_CODE"].ToString(), table.Rows[i]["ATTENDANCE_NAME"].ToString());
            }

            str.AppendFormat(@" FROM (SELECT   year_month,
                                               dept_code,
                                               staff_id AS emp_no,");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["ATTENDANCE_NAME"].ToString() != "实际出勤")
                {
                    str.AppendFormat(" SUM(CASE WHEN attendance_code = '{0}' THEN ATTENDANCE_VALUE ELSE 0 END) {0},", table.Rows[i]["ATTENDANCE_CODE"].ToString());
                }
            }

            str.AppendFormat(@"                staff_id as STAFF_ID
                                        FROM   RLZY.QU_ATTENDANCE_DEPT
                                    GROUP BY   year_month, dept_code, staff_id) A,");
            str.AppendFormat(@" 
                                       COMM.SYS_DEPT_DICT B,
                                       RLZY.NEW_STAFF_INFO C
                               WHERE       TO_CHAR (A.YEAR_MONTH, 'YYYYMM') >= '{0}'
                                       AND TO_CHAR (A.YEAR_MONTH, 'YYYYMM') <= '{4}'
                                       AND A.DEPT_CODE = B.DEPT_CODE
                                       AND A.STAFF_ID = C.STAFF_ID
                                       {1} {2} {3}
                            GROUP BY   A.STAFF_ID,
                                       C.NAME,
                                       B.ACCOUNT_DEPT_CODE,
                                       B.ACCOUNT_DEPT_NAME,C.DUTY,C.STAFFSORT,C.JOB,C.TITLE_LIST
                            ORDER BY   B.ACCOUNT_DEPT_CODE,C.NAME", datetime, staffSort, power, deptcode, enddatetime);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="deptCode"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataSet ViewOnGuardDetail(string fromDate, string toDate, string deptCode, string name)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT T3.CASES, T3.PLACE, T3.PERSON,
                                      T3.STRAT_DATE,T3.END_DATE,T3.ID,T3.STRAT_DATE,T3.END_DATE,T3.ID,(SELECT DEPT_NAME FROM {5}.SYS_DEPT_DICT WHERE T3.DEPT_CODE = {5}.SYS_DEPT_DICT.DEPT_CODE) DEPT_NAME,T3.USER_NAME,T3.USER_NAME
                                    FROM   {0}.ONGUARD_CASE T3
                                     WHERE ((T3.STRAT_DATE >= '{1}' AND T3.STRAT_DATE <= '{2}')
                                        OR (T3.END_DATE >='{1}' AND T3.END_DATE <= '{2}')
                                        OR (TO_DATE (STRAT_DATE, 'YYYY-MM-DD') < TO_DATE ('{1}', 'YYYY-MM-DD') AND END_DATE IS NULL)
                                        OR (TO_DATE (STRAT_DATE, 'YYYY-MM-DD') < TO_DATE ('{1}', 'YYYY-MM-DD') AND TO_DATE (END_DATE, 'YYYY-MM-DD') > TO_DATE ('{2}', 'YYYY-MM-DD'))
                                        )
                                        AND T3.DEPT_CODE = '{3}' AND T3.USER_NAME= '{4}'", DataUser.RLZY, fromDate, toDate, deptCode, name, DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 上报科室信息
        /// </summary>
        /// <returns></returns>
        public DataSet getDeptInfo()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.*
                                      FROM {0}.DEPT_INFO A, {1}.SYS_DEPT_DICT B
                                     WHERE A.DEPT_CODE = B.DEPT_CODE AND B.ATTR = '是' AND B.SHOW_FLAG = '0'", DataUser.RLZY, DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 上报专科中心信息
        /// </summary>
        /// <param name="date">yyyy</param>
        /// <returns></returns>
        public DataSet getSpecCenterInfo(string date)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT * FROM {0}.SPEC_CENTER WHERE STAT_MONTH='{1}'", DataUser.RLZY, date);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 上报人员信息
        /// </summary>
        /// <param name="date">yyyyMM01</param>
        /// <returns></returns>
        public DataSet getStaffInfo(string date)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.*  
                               FROM {0}.NEW_STAFF_INFO A ,{1}.SYS_DEPT_DICT B
                               WHERE A.DEPT_CODE = B.DEPT_CODE AND  B.ATTR = '是' 
                                     AND (ADD_MARK='1'oR ADD_MARK='2') 
                                     AND USER_DATE >='{2}' AND B.SHOW_FLAG = '0' AND A.ISONGUARD<>'离职'", DataUser.RLZY, DataUser.COMM, date);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取科室人员信息
        /// </summary>
        /// <returns></returns>
        public DataSet getStaffInfoList()
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.*  
                               FROM {0}.NEW_STAFF_INFO A ,{1}.SYS_DEPT_DICT B
                               WHERE A.DEPT_CODE = B.DEPT_CODE AND  B.ATTR = '是' 
                                     AND ADD_MARK<>'2' AND ADD_MARK IS NOT NULL ", DataUser.RLZY, DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="add_mark"></param>
        /// <param name="power"></param>
        /// <param name="StaffSort"></param>
        /// <param name="onguard"></param>
        /// <param name="sex"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataSet getStaffInfoList(string deptCode, string add_mark, string power, string StaffSort, string onguard, string sex, string endtime)
        {
            string Code = deptCode == "" ? "" : " AND A.DEPT_CODE = '" + deptCode + "'";

            if (onguard != "")
                Code += " and ISONGUARD='" + onguard + "' ";
            if (onguard == "")
            {
                Code += " and (ISONGUARD='是' or ISONGUARD='否') ";
            }

            if (sex != "")
            {
                Code += " and SEX='" + sex + "' ";
            }

            if (endtime != "")
            {
                Code += " and to_char(to_date(CONTRACT_END,'yyyy-mm-dd'),'yyyymm')='" + endtime + "' ";
            }

            string mark = " AND ADD_MARK=" + add_mark + "";

            StaffSort = StaffSort == "" ? "" : "AND A.STAFFSORT IN (" + StaffSort + ")";

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.*  
                               FROM {0}.NEW_STAFF_INFO A ,{1}.SYS_DEPT_DICT B
                               WHERE A.DEPT_CODE = B.DEPT_CODE {2} {3} {4} ORDER BY A.STAFF_ID", DataUser.RLZY, DataUser.COMM, Code, mark, StaffSort);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet getStaffInfo()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select TABLE_NAME,COLUMN_NAME,NVL(COMMENTS,COLUMN_NAME) COMMENTS FROM RLZY.TAB_COL_INFO WHERE TABLE_NAME='NEW_STAFF_INFO'");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 上报成果信息
        /// </summary>
        /// <param name="date">yyyy-MM-01</param>
        /// <returns></returns>
        public DataSet getFriutInfo(string date)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.* FROM {0}.FRUIT A,{1}.SYS_DEPT_DICT B
                             WHERE A.DEPT_CODE=B.DEPT_CODE 
                               AND A.ENTER_DATE>='{2}' 
                              AND A.ADD_MARK='1' AND B.ATTR = '是' AND B.SHOW_FLAG = '0'", DataUser.RLZY, DataUser.COMM, date);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 上报人才培养信息
        /// </summary>
        /// <param name="date">yyyy-MM-01</param>
        /// <returns></returns>
        public DataSet getPersonsInfo(string date)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.* FROM {0}.PERSONS_PLANT_INFO A,{1}.SYS_DEPT_DICT B
                             WHERE A.DEPT_CODE=B.DEPT_CODE 
                               AND A.START_DATE>='{2}' 
                              AND B.ATTR = '是' AND B.SHOW_FLAG = '0'", DataUser.RLZY, DataUser.COMM, date);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 上报课题信息
        /// </summary>
        /// <param name="date">yyyy-MM-01</param>
        /// <returns></returns>
        public DataSet getProblemInfo(string date)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.* FROM {0}.PROBLEM_INFO A,{1}.SYS_DEPT_DICT B
                             WHERE A.DEPT_CODE=B.DEPT_CODE 
                               AND A.RECORD_DATE>='{2}' 
                              AND A.ADD_MARK='1' AND B.ATTR = '是' AND B.SHOW_FLAG = '0'", DataUser.RLZY, DataUser.COMM, date);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 上报论文信息
        /// </summary>
        /// <param name="date">yyyy-MM-01</param>
        /// <returns></returns>
        public DataSet getDiscourseInfo(string date)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.* FROM {0}.DISCOURSE A,{1}.SYS_DEPT_DICT B
                             WHERE A.DEPT_CODE=B.DEPT_CODE 
                               AND A.RECORD_DATE>='{2}' 
                              AND A.ADD_MARK='1' AND B.ATTR = '是' AND B.SHOW_FLAG = '0'", DataUser.RLZY, DataUser.COMM, date);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 上报专著信息
        /// </summary>
        /// <param name="date">yyyy-MM-01</param>
        /// <returns></returns>
        public DataSet getMongraphInfo(string date)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.* FROM {0}.MONOGRAPH A,{1}.SYS_DEPT_DICT B
                             WHERE A.DEPT_CODE=B.DEPT_CODE 
                               AND A.RECORD_DATE>='{2}' 
                              AND A.ADD_MARK='1' AND B.ATTR = '是' AND B.SHOW_FLAG = '0'", DataUser.RLZY, DataUser.COMM, date);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 上报新技术信息
        /// </summary>
        /// <param name="date">yyyy-MM-01</param>
        /// <returns></returns>
        public DataSet getDevNewTechInfo(string date)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.* FROM {0}.DEVELOP_NEW_TECHNIC A,{1}.SYS_DEPT_DICT B
                             WHERE A.DEPT_CODE=B.DEPT_CODE 
                               AND A.DATES>='{2}'
                              AND A.ADD_MARK='1' AND B.ATTR = '是' AND B.SHOW_FLAG = '0'", DataUser.RLZY, DataUser.COMM, date);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 上报学术会议信息
        /// </summary>
        /// <param name="date">yyyy-MM-01</param>
        /// <returns></returns>
        public DataSet getTechMeetingInfo(string date)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT *
                                  FROM {0}.TECI_MEET
                                 WHERE SPEC_CODE IN (
                                          SELECT DEPT_CODE
                                            FROM {0}.CENTER_DEPTDETAIL
                                           WHERE DEPT_CODE IN (
                                                             SELECT A.SPEC_CODE
                                                               FROM {0}.TECI_MEET A, {1}.SYS_DEPT_DICT B
                                                              WHERE A.SPEC_CODE = B.DEPT_CODE
                                                                    AND B.ATTR = '是' AND B.SHOW_FLAG = '0'))
                                   AND ADD_MARK = '1'
                                   AND ENTER_TIME > = '{2}' ", DataUser.RLZY, DataUser.COMM, date);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 上报特殊诊疗信息
        /// </summary>
        /// <param name="date">yyyy-MM-01</param>
        /// <returns></returns>
        public DataSet getSpecMediInfo(string date)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.* FROM {0}.SPEC_MEDI A,{1}.SYS_DEPT_DICT B
                             WHERE A.DEPT_CODE=B.DEPT_CODE 
                               AND A.ENTER_TIME>='{2}' 
                              AND A.ADD_MARK='1' AND B.ATTR = '是' AND B.SHOW_FLAG = '0'", DataUser.RLZY, DataUser.COMM, date);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 上报信息化建设
        /// </summary>
        /// <returns></returns>
        public DataSet getInformationInfo()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT * FROM {0}.INFORMATION 
                                WHERE ID=(SELECT MAX(ID) FROM {0}.INFORMATION)", DataUser.RLZY);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }




        public DataSet getGraphJobMediStat(string power, string stafftype)
        {
            stafftype = stafftype == "" ? "" : "AND STAFFSORT IN (" + stafftype + ")";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT DEPT_NAME UNIT_NAME,
                                        SUM(DECODE(JOB,'医师',1,0) ) AS VALUE
                                        FROM  {0}.NEW_STAFF_INFO  WHERE ADD_MARK = '1' and ISONGUARD='是' {1} {2} GROUP BY DEPT_NAME", DataUser.RLZY, power, stafftype);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询学历，年龄分布
        /// </summary>
        /// <param name="dept_code">科室代码</param>
        /// <param name="PersonType">人员类别</param>
        /// <param name="Power">权限</param>
        /// <returns></returns>
        public DataSet getGraphEduStat(string dept_code, string power, string stafftype)
        {
            string code = dept_code == "" ? "" : " AND DEPT_CODE='" + dept_code + "'";
            stafftype = stafftype == "" ? "" : "AND STAFFSORT IN (" + stafftype + ")";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT  NVL (TOPEDUCATE, '其他') UNIT_NAME,
                                       SUM (DECODE (TOPEDUCATE, '本科', 1, 0))
                                     + SUM (DECODE (TOPEDUCATE, '大专', 1, 0))
                                     + SUM (DECODE (TOPEDUCATE, '中专', 1, 0)) +
                                     SUM (DECODE (TOPEDUCATE,'本科', 0,'大专', 0,'中专', 0,1)) AS VALUE
                                FROM (SELECT   TOPEDUCATE, EDU1, DEPT_NAME, DEPT_CODE
                                          FROM {0}.NEW_STAFF_INFO
                                         WHERE ADD_MARK = '1' and ISONGUARD='是' {1} {2} {3}
                                      ORDER BY DEPT_CODE) T1
                            GROUP BY TOPEDUCATE", DataUser.RLZY, code, stafftype, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 年龄分布
        /// </summary>
        /// <param name="dept_code">科室代码</param>
        /// <returns></returns>
        public DataSet getGraphAgeStat(string dept_code, string power, string stafftype)
        {
            string code = dept_code == "" ? "" : " AND DEPT_CODE='" + dept_code + "'";
            stafftype = stafftype == "" ? "" : "AND STAFFSORT IN (" + stafftype + ")";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                                        SELECT SUM(JOB9),SUM(JOB10),SUM(JOB11),SUM(JOB12),SUM(JOB13),SUM(JOB14)
                                        FROM (SELECT  BIRTH,
                                                 SUM(CASE WHEN BIRTH <= 30 THEN 1 ELSE 0 END) AS JOB9,
                                                 SUM(CASE WHEN BIRTH > 30 AND BIRTH <= 40 THEN 1 ELSE 0 END) AS JOB10,
                                                 SUM(CASE WHEN BIRTH > 40 AND BIRTH <= 50 THEN 1 ELSE 0 END) AS JOB11,
                                                 SUM(CASE WHEN BIRTH > 50 AND BIRTH <= 55 THEN 1 ELSE 0 END) AS JOB12,
                                                 SUM(CASE WHEN BIRTH > 55 AND BIRTH <= 60 THEN 1 ELSE 0 END) AS JOB13,
                                                 SUM(CASE WHEN BIRTH > 60 THEN 1 ELSE 0 END) AS JOB14
                                            FROM (SELECT   TOPEDUCATE, EDU1, DEPT_NAME,
                                                           TRUNC (TO_NUMBER((MONTHS_BETWEEN (SYSDATE,TO_DATE(BIRTHDAY,'yyyy-mm-dd'))))/ 12) AS BIRTH,
                                                           DEPT_CODE
                                                      FROM {0}.NEW_STAFF_INFO
                                                     WHERE ADD_MARK = '1' and ISONGUARD='是' {1} {2} {3}
                                                  ORDER BY BIRTHDAY, DEPT_CODE) T1
                                        GROUP BY BIRTH)", DataUser.RLZY, code, stafftype, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 职称分布
        /// </summary>
        /// <param name="dept_code">科室代码</param>
        /// <returns></returns>
        public DataSet getGraphJobStat(string dept_code, string power, string stafftype)
        {
            string code = dept_code == "" ? "" : " AND DEPT_CODE='" + dept_code + "'";
            stafftype = stafftype == "" ? "" : "AND STAFFSORT IN (" + stafftype + ")";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT JOB UNIT_NAME,
                                        SUM(DECODE(JOB,'主任医师',1,0) ) +
                                        SUM(DECODE(JOB,'副主任医师',1,0) ) +
                                        SUM(DECODE(JOB,'主治医师',1,0) )+
                                        SUM(DECODE(JOB,'医师',1,0) ) +
                                        SUM(DECODE(JOB,'主任护师',1,0) ) +
                                        SUM(DECODE(JOB,'副主任护师',1,0) ) +
                                        SUM(DECODE(JOB,'主管护师',1,0) ) +
                                        SUM(DECODE(JOB,'护师',1,0) ) +
                                        SUM(DECODE(JOB,'护士',1,0) ) +
                                        SUM(DECODE(JOB,'主任技师',1,0) ) +
                                        SUM(DECODE(JOB,'副主任技师',1,0) ) +
                                        SUM(DECODE(JOB,'主管技师',1,0) ) +
                                        SUM(DECODE(JOB,'技师',1,0) ) +
                                        SUM(DECODE(JOB,'技士',1,0) ) +
                                        SUM(DECODE(JOB,'主任药师',1,0) )+
                                        SUM(DECODE(JOB,'副主任药师',1,0) ) +
                                        SUM(DECODE(JOB,'主管药师',1,0) ) +
                                        SUM(DECODE(JOB,'药师',1,0) ) +
                                        SUM(DECODE(JOB,'药士',1,0) ) +
                                        SUM(DECODE(JOB,'高级工程师',1,0) ) +
                                        SUM(DECODE(JOB,'工程师',1,0) ) +
                                        SUM(DECODE(JOB,'助理工程师',1,0) ) +
                                        SUM(DECODE(JOB,'会计师',1,0) ) AS VALUE
                                        FROM  {0}.NEW_STAFF_INFO  WHERE ADD_MARK = '1' and ISONGUARD='是'  {1} {2} {3} GROUP BY JOB", DataUser.RLZY, code, stafftype, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 更新上报时间
        /// </summary>
        /// <returns></returns>
        public void UpdateStaffInfoLoad()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"update {0}.update_data set updata='{1}' where id='1'", DataUser.RLZY, (System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-01"));
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 查询上报时间
        /// </summary>
        /// <returns></returns>
        public DataSet getStaffInfoLoadTime()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select * from {0}.update_data where id = '1'", DataUser.RLZY);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询未审批人员
        /// </summary>
        /// <param name="type">人员类别</param>
        /// <returns></returns>
        public bool getStaffInfoType(string type)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT COUNT(*) FROM {0}.NEW_STAFF_INFO WHERE STAFFSORT IN ({1}) AND ADD_MARK NOT IN ('1','2')", DataUser.RLZY, type);
            return Convert.ToInt32(OracleOledbBase.ExecuteScalar(str.ToString())) > 0 ? true : false;
        }

        public void deleteStaffInfoInput(string type, string userDate)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"delete from {0}.STAFF_INFO_INPUT where STAFFSORT in ({1}) and USER_DATE='{2}'", DataUser.RLZY, type, userDate);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        public void InsertStaffInfoInput(string type, string userdate)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.STAFF_INFO_INPUT 
                               (SELECT TO_NUMBER({2}+ROWNUM-1)STAFF_ID,DEPT_CODE,DEPT_NAME,NAME,IF_ARMY,ADD_MARK,ISONGUARD,BIRTHDAY,SEX,NATIONALS,
                                        BONUS_FACTOR,GOVERNMENT_ALLOWANCE,CADRES_CATEGORIES,STUDY_OVER_DATE,DEPT_TYPE,TOPEDUCATE,
                                        STUDY_SPECSORT,INHOSPITALDATE,BASEPAY,RETAINTERM,JOB,JOBDATE,STAFFSORT,BEENROLLEDINDATE,
                                        WORKDATE,DUTY,DUTYDATE,TECHINCCLASS,TECHNICCLASSDATE,CIVILSERVICECLASS,CIVILSERVICECLASSDATE,
                                        SANTSPECSORT,ROOTSPECSORT,MEDICARDMARK,MEDICARD,INPUT_USER,INPUT_DATE,'{3}' USER_DATE,GUARDTEAM,
                                        GUARDGROUP,GUARDDUTY,GUARDTYPE,GUARDCHAN,GUARDTIME,GUARDCAUS,GUARDREMARK,DEPTGROUP,HOMEPLACE,
                                        CERTIFICATE_NO,MARITAL_STATUS,TITLE_LIST,EDU1,GRADUATE_ACADEMY,DATE_OF_GRADETITLE,RANK,TITLE,
                                        GROUP_ID,MEMO,MARK_USER 
                                        FROM {0}.NEW_STAFF_INFO WHERE ADD_MARK = '1' AND STAFFSORT IN ({1}))", DataUser.RLZY, type, OracleOledbBase.GetMaxID("STAFF_ID", "RLZY.STAFF_INFO_INPUT"), userdate);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 查询军卫汉字输入码
        /// </summary>
        /// <param name="userid">人员ID</param>
        /// <returns></returns>
        public string getHisDataDbUserByUserid(string userid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT T1.USER_ID,T1.USER_NAME,T1.DB_USER,
                                (SELECT DEPT_NAME FROM {0}.DEPT_DICT T2 WHERE T2.DEPT_CODE = T1.USER_DEPT) DEPT_NAME 
                                FROM {0}.USERS T1 
                                WHERE T1.USER_ID = '{1}' ORDER BY T1.USER_NAME", DataUser.HISFACT, userid);

            DataTable l_dt = OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
            if (l_dt.Rows.Count > 0)
            {
                return l_dt.Rows[0]["DB_USER"].ToString();
            }
            else { return ""; }
        }
        /// <summary>
        /// 查询军卫汉字输入码
        /// </summary>
        /// <param name="userid">人员ID</param>
        /// <returns></returns>
        public string getHisDataDbUserByRuserid(string userid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT T1.USER_ID,T1.USER_NAME,T1.DB_USER,
                                (SELECT DEPT_NAME FROM {0}.DEPT_DICT T2 WHERE T2.DEPT_CODE = T1.USER_DEPT) DEPT_NAME 
                                FROM {0}.USERS T1 
                                WHERE T1.USER_ID = '{1}' ORDER BY T1.USER_NAME", DataUser.HISFACT, userid);

            DataTable l_dt = OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
            if (l_dt.Rows.Count > 0)
            {
                return l_dt.Rows[0]["USER_ID"].ToString();
            }
            else { return ""; }
        }
        /// <summary>
        /// 获取人员科室
        /// </summary>
        /// <param name="userid">人员ID</param>
        /// <returns></returns>
        public string getDeptByUserid(string userid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select * from hisfact.users where user_id = '{1}'", DataUser.HISFACT, userid);

            DataTable l_dt = OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
            if (l_dt.Rows.Count > 0)
            {
                return l_dt.Rows[0]["USER_DEPT"].ToString();
            }
            else { return ""; }
        }

        /// <summary>
        /// 添加专科中心科室
        /// </summary>
        /// <param name="deptcode">科室代码</param>
        /// <param name="deptName">科室名称</param>
        /// <param name="year">年度</param>
        /// <param name="centercode">中心代码</param>
        public void InsertCenterDept(string deptcode, string deptName, string year, string centercode)
        {
            string str = "insert into RLZY.CENTER_DEPTDETAIL (id,CENTER_CODE,dept_code,dept_name,years) values (?,?,?,?,?)";
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , OracleOledbBase.GetMaxID("ID",DataUser.RLZY+".CENTER_DEPTDETAIL") ) ,
					new OleDbParameter( "" , centercode ) ,
					new OleDbParameter( "" , deptcode ) ,
					new OleDbParameter( "" , deptName ) ,
				    new OleDbParameter( "" , year)	
				};
            OracleOledbBase.ExecuteNonQuery(str, cmdPara);
        }

        /// <summary>
        /// 专科中心科室ID
        /// </summary>
        /// <param name="id"></param>
        public void DelCenterDept(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"delete  {0}.CENTER_DEPTDETAIL where id in({1})", DataUser.RLZY, id);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 查看中心科室
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="centercode">中心代码</param>
        /// <returns></returns>
        public DataSet ViewCenterDept(string year, string centercode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT * FROM {0}.CENTER_DEPTDETAIL WHERE YEARS='{1}' AND CENTER_CODE={2}", DataUser.RLZY, year, centercode);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }




        //public void InsertStaffInput(string type, string userdate) 
        //{
        //    MyLists list = new MyLists();

        //    List l_listDel = new List();
        //    l_listDel.Parameters = new OleDbParameter[] { new OleDbParameter() };
        //    l_listDel.StrSql = deleteStaffInfoInput(type,userdate);
        //    list.Add(l_listDel);

        //    List insert = new List();
        //    insert.Parameters = new OleDbParameter[] { new OleDbParameter() };
        //    insert.StrSql = InsertStaffInfoInput(type, userdate);
        //    list.Add(insert);
        //    OracleOledbBase.ExecuteTranslist(list);  
        //}
    }
}
