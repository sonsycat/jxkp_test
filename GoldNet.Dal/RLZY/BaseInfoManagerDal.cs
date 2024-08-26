using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OleDb;
using System.Data;
using GoldNet.Comm;

namespace Goldnet.Dal
{
    public class BaseInfoManagerDal
    {
        public BaseInfoManagerDal()
        { }

        /// <summary>
        /// 科内训练查询
        /// </summary>
        /// <param name="fromDate">开始时间</param>
        /// <param name="ToDate">结束时间</param>
        /// <param name="deptCode">科室代码</param>
        /// <param name="power">权限</param>
        /// <param name="add_mark">审批标记 0:已提交,1:已审批,3:保存未提交</param>
        /// <returns></returns>
        public DataSet ViewDeptExList(string fromDate, string ToDate, string deptCode, string power, string add_mark)
        {
            string Code = deptCode == "" ? "" : " AND DEPT_CODE = '" + deptCode + "'";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            string Power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT ID,DEPT_CODE, DEPT_NAME,EXDATES,EXTYPES,EXPERSON,EXSORTS ,EXTIMES,   
                                EXMEMERT, EXCOUNT, MARK_SUG, SETUP_SUG,
                                DECODE(ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK
                                FROM  {0}.DEPT_EX 
                                WHERE SUBSTR (EXDATES, 1, 6) >= '{1}' AND SUBSTR (EXDATES, 1, 6) <= '{2}' {3} {4} {5}", DataUser.RLZY, fromDate, ToDate, Code, mark, Power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        //liu.shh  2012.12.18
        public DataSet ViewDeptExListPerson(string fromDate, string ToDate, string deptCode, string power, string add_mark, string userId)
        {
            string Code = deptCode == "" ? "" : " AND DEPT_CODE = '" + deptCode + "'";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            string Power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            string id = userId == "" ? "" : "AND USER_ID = '" + userId + "'";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT ID,DEPT_CODE, DEPT_NAME,EXDATES,EXTYPES,EXPERSON,EXSORTS ,EXTIMES,   
                                EXMEMERT, EXCOUNT, MARK_SUG, SETUP_SUG,
                                DECODE(ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK
                                FROM  {0}.DEPT_EX 
                                WHERE SUBSTR (EXDATES, 1, 6) >= '{1}' AND SUBSTR (EXDATES, 1, 6) <= '{2}' {3} {4} {5} {6}", DataUser.RLZY, fromDate, ToDate, Code, mark, Power, id);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 插入科内训练
        /// </summary>
        public void InsertDeptExList(string deptCode, string deptName, string exdates, string extype, string experson, string exsorts, string extimes, string exmemert, string excount, string add_mark, string user_id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.DEPT_EX
												(ID,DEPT_CODE, DEPT_NAME, EXDATES, EXTYPES, EXPERSON, 
												EXSORTS, EXTIMES, EXMEMERT, EXCOUNT,ADD_MARK,USER_ID)
												VALUES (?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , OracleOledbBase.GetMaxID("id","RLZY.DEPT_EX") ) ,
					new OleDbParameter( "" , deptCode ) ,
					new OleDbParameter( "" , deptName ) ,
					new OleDbParameter( "" , exdates ) ,
					new OleDbParameter( "" , extype ) ,
					new OleDbParameter( "" , experson ) ,
					new OleDbParameter( "" , exsorts ) ,
					new OleDbParameter( "" , extimes ) ,
					new OleDbParameter( "" , exmemert ) ,
					new OleDbParameter( "" , excount ) ,
				    new OleDbParameter( "", add_mark),
                    new OleDbParameter( "", user_id)
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 更新科内训练
        /// </summary>
        public void UpdataDeptExList(string id, string deptCode, string deptName, string extype, string experson, string exsorts, string extimes, string exmemert, string excount, string add_mark, string exdates, string mark_sug, string setup_sug)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.DEPT_EX
                                    SET DEPT_CODE =?, DEPT_NAME =?, EXTYPES =?, EXPERSON =?, EXSORTS =?, 
                                    EXTIMES =?, EXMEMERT =?, EXCOUNT =?, ADD_MARK =?,EXDATES=?,MARK_SUG=?,SETUP_SUG=?  WHERE ID=?", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , deptCode ) ,
					new OleDbParameter( "" , deptName ) ,
					new OleDbParameter( "" , extype ) ,
					new OleDbParameter( "" , experson ) ,
					new OleDbParameter( "" , exsorts ) ,
					new OleDbParameter( "" , extimes ) ,
					new OleDbParameter( "" , exmemert ) ,
					new OleDbParameter( "" , excount ) ,
					new OleDbParameter( "" , add_mark ) ,
					new OleDbParameter( "" , exdates ) ,
                    new OleDbParameter( "" , mark_sug ) ,
                    new OleDbParameter( "" , setup_sug ) ,
					new OleDbParameter( "" , id ) 
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }


        /// <summary>
        /// 批量修改审批页面内容
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="FieldMarkName">MARK字段名称</param>
        /// <param name="FieldIdName">ID字段名称</param>
        /// <param name="id">ID值</param>
        /// <param name="add_mark">MARK值</param>
        public void EchoUpDataHandle(string TableName, string FieldMarkName, string FieldIdName, string id, string add_mark)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.{3}
                                        SET {4}='{1}'
                                        WHERE {5} IN ({2})", DataUser.RLZY, add_mark, id, TableName, FieldMarkName, FieldIdName);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 批量删除审批页面内容
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="FieldIdName">ID字段名称</param>
        /// <param name="id">ID值</param>
        public void EchoDeleteHandle(string TableName, string FieldIdName, string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE FROM {0}.{2} WHERE {3} IN ({1})", DataUser.RLZY, id, TableName, FieldIdName);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }



        /// <summary>
        /// 查询课题
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="add_mark">状态</param>
        /// <param name="deptCode">科室代码</param>
        /// <param name="Principal">负责人姓名</param>
        /// <param name="power">权限</param>
        /// <returns></returns>
        public DataSet ViewProblemList(string year, string add_mark, string deptCode, string Principal, string power)
        {
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            Principal = Principal == "" ? "" : "AND PRINCIPAL LIKE '%" + Principal + "%'";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   ID, DEPT_CODE, DEPT_NAME, PROBLEM_NAME, PROBLEM_SORT, YEARS,
                                         PRINCIPAL, PRINCIPAL_SPECIALITY, PRINCIPAL_SCHOOL_AGE, PRINCIPAL_JOB,
                                         UNIT, MOSTLY_PERSONS, CONTENT, PROBLEM_CODE, START_DATE, END_DATE,
                                         OUTLAY_TYPE, OUTLAY_NUM, RECORD_DATE, ENTER_PERS, 
                                         DECODE(ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK,
                                         PASSED_UNIT, LERVER, SECOND_AUTHOR, THIRD_AUTHOR, MARK_SUG,SETUP_SUG
                                    FROM {0}.PROBLEM_INFO
                                    WHERE TO_NUMBER(YEARS) {1} {2} {3} {4} {5}
                                ORDER BY YEARS DESC", DataUser.RLZY, year, mark, deptCode, Principal, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataSet ViewRewardList(string year)
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   *
                                  FROM   RLZY.REWARD_INFO A
                                 WHERE    to_number(to_char(TO_DATE (REWARD_DATE, 'yyyy-mm-dd'),'yyyy')) <= to_number('{0}')", year);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        public DataSet ViewRewardList(string TimeOrgan, string year, string deptCode, string name)
        {
            string stryear = "";
            if (TimeOrgan != "" && year != "")
            {
                stryear = "AND to_number(to_char(TO_DATE (REWARD_DATE, 'yyyy-mm-dd'),'yyyy')) " + TimeOrgan + " to_number('" + year + "')";
            }

            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            name = name == "" ? "" : "AND MANAGE_PRS_NAME LIKE '%" + name + "%'";

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   *
                                  FROM   RLZY.REWARD_INFO A
                                 WHERE  1=1 {0} {1} {2}", stryear, deptCode, name);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimeOrgan"></param>
        /// <param name="year"></param>
        /// <param name="deptCode"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataSet ViewProblemList(string TimeOrgan,string year, string deptCode, string name)
        {
            if (TimeOrgan !="" && year!="")
            {
                year = "AND to_number(to_char(TO_DATE (REWARD_DATE, 'yyyy-mm-dd'),'yyyy')) " + TimeOrgan + " to_number('" + year + "')";
            }
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            name = name == "" ? "" : "AND MANAGE_PRS_NAME LIKE '%" + name + "%'";

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   *
                                  FROM   RLZY.REWARD_INFO A
                                 WHERE  1=1 {0} {1} {2}", year, deptCode, name);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        //liu.shh  2012.12.18
        public DataSet ViewProblemListPerson(string year, string add_mark, string deptCode, string Principal, string power, string userId)
        {
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            Principal = Principal == "" ? "" : "AND PRINCIPAL LIKE '%" + Principal + "%'";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            userId = userId == "" ? "" : "AND USER_ID = '" + userId + "'";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   ID, DEPT_CODE, DEPT_NAME, PROBLEM_NAME, PROBLEM_SORT, YEARS,
                                         PRINCIPAL, PRINCIPAL_SPECIALITY, PRINCIPAL_SCHOOL_AGE, PRINCIPAL_JOB,
                                         UNIT, MOSTLY_PERSONS, CONTENT, PROBLEM_CODE, START_DATE, END_DATE,
                                         OUTLAY_TYPE, OUTLAY_NUM, RECORD_DATE, ENTER_PERS,
                                         DECODE(ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK,
                                         PASSED_UNIT, LERVER, SECOND_AUTHOR, THIRD_AUTHOR, MARK_SUG,SETUP_SUG
                                    FROM {0}.PROBLEM_INFO
                                    WHERE TO_NUMBER(YEARS) {1} {2} {3} {4} {5} {6}
                                ORDER BY YEARS DESC", DataUser.RLZY, year, mark, deptCode, Principal, power, userId);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 添加课题信息
        /// </summary>
        /// <param name="deptCode">科室编号</param>
        /// <param name="deptName">科室名称</param>
        /// <param name="problemName">课题名称</param>
        /// <param name="problemSort">题类别</param>
        /// <param name="years">年度</param>
        /// <param name="startEndDate">课题开始终止时间</param>
        /// <param name="principal">负责人</param>
        /// <param name="principalSpeciality">负责人专业</param>
        /// <param name="principalSchoolAge">负责人学历</param>
        /// <param name="principalJob">负责人职称</param>
        /// <param name="unit">协作单位</param>
        /// <param name="mostlyPersons">主要成员</param>
        /// <param name="content">研究主要内容及预期目标</param>
        public void InsertProblem(string deptCode, string deptName, string problemName, string problemSort, string years, string principal, string principalSpeciality,
                                    string principalSchoolAge, string principalJob, string unit, string mostlyPersons, string content, string problemCode, string outlyType,
                                    string StartDate, string EndDate, string outlyNum, string recordDate, string Enter_pers, string add_mark, string PassedUnit, string Lerver, string user_id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.PROBLEM_INFO
                                                (ID,DEPT_CODE, DEPT_NAME, PROBLEM_NAME, PROBLEM_SORT, YEARS, 
                                                PRINCIPAL, PRINCIPAL_SPECIALITY, PRINCIPAL_SCHOOL_AGE, 
                                                PRINCIPAL_JOB, UNIT, MOSTLY_PERSONS, CONTENT,PROBLEM_CODE,OUTLAY_TYPE,
                                                START_DATE,END_DATE,OUTLAY_NUM,RECORD_DATE,ENTER_PERS,ADD_MARK,PASSED_UNIT,LERVER,USER_ID)
                                                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "ID" , OracleOledbBase.GetMaxID("ID","RLZY.PROBLEM_INFO") ) ,
					new OleDbParameter( "DEPT_CODE" , deptCode ) ,
					new OleDbParameter( "DEPT_NAME" , deptName ) ,
					new OleDbParameter( "PROBLEM_NAME" , problemName ) ,
					new OleDbParameter( "PROBLEM_SORT" , problemSort ) ,
					new OleDbParameter( "YEARS" , years ) ,
					new OleDbParameter( "PRINCIPAL" , principal ) ,
					new OleDbParameter( "PRINCIPAL_SPECIALITY" , principalSpeciality ) ,
					new OleDbParameter( "PRINCIPAL_SCHOOL_AGE" , principalSchoolAge ) ,
					new OleDbParameter( "PRINCIPAL_JOB" , principalJob) ,
					new OleDbParameter( "UNIT" , unit ) ,
					new OleDbParameter( "MOSTLY_PERSONS" , mostlyPersons) ,
					new OleDbParameter( "CONTENT" , content ) ,
				    new OleDbParameter( "PROBLEM_CODE" , problemCode ) ,
				    new OleDbParameter( "Outlay_type" , outlyType ) ,
				    new OleDbParameter( "Start_date" , StartDate ) ,
				    new OleDbParameter( "End_date" , EndDate ) ,
				    new OleDbParameter( "Outlay_num" , outlyNum ) ,
				    new OleDbParameter( "Record_date" , recordDate),
				    new OleDbParameter( "Enter_pers" , Enter_pers),
                    new OleDbParameter( "ADD_MARK" , add_mark),
                    new OleDbParameter( "PASSED_UNIT" , PassedUnit),
                    new OleDbParameter( "LERVER" , Lerver),
                    new OleDbParameter( "USER_ID" , user_id)
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);

        }


        public void InsertReward(string rewardDesc,string deptCode, string deptName, string StartDate, string manageDept, string managePrs, string managePrsName, string manageDeptName)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.REWARD_INFO
                                                (ID,DEPT_CODE, DEPT_NAME, REWARD_DATE, REWARD_DESC, MANAGE_DEPT, 
                                                MANAGE_PRS, MANAGE_DEPT_NAME, MANAGE_PRS_NAME)
                                                VALUES (?,?,?,?,?,?,?,?,?)", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "ID" , OracleOledbBase.GetMaxID("ID","RLZY.REWARD_INFO") ) ,
					new OleDbParameter( "DEPT_CODE" , deptCode ) ,
					new OleDbParameter( "DEPT_NAME" , deptName ) ,
					new OleDbParameter( "REWARD_DATE" , StartDate ) ,
					new OleDbParameter( "REWARD_DESC" , rewardDesc ) ,
					new OleDbParameter( "MANAGE_DEPT" , manageDept ) ,
					new OleDbParameter( "MANAGE_PRS" , managePrs ) ,
					new OleDbParameter( "MANAGE_DEPT_NAME" , manageDeptName ),
                    new OleDbParameter( "MANAGE_PRS_NAME" , managePrsName ) 
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }


        public void UpdataReward(string id, string rewardDesc, string deptCode, string deptName, string StartDate, string manageDept, string managePrs, string managePrsName, string manageDeptName)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.REWARD_INFO SET DEPT_CODE =?, 
                                                    DEPT_NAME =?, REWARD_DATE =?, REWARD_DESC =?, MANAGE_DEPT =?, 
                                                    MANAGE_PRS =?, MANAGE_DEPT_NAME =?, 
                                                    MANAGE_PRS_NAME =? WHERE ID=?", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , deptCode ) ,
					new OleDbParameter( "" , deptName ) ,
					new OleDbParameter( "" , StartDate ) ,
					new OleDbParameter( "" , rewardDesc ) ,
					new OleDbParameter( "" , manageDept ) ,
					new OleDbParameter( "" , managePrs ) ,
					new OleDbParameter( "" , manageDeptName ) ,
					new OleDbParameter( "" , managePrsName ),
					new OleDbParameter( "" , id )
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 更新课题信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="deptCode">科室编号</param>
        /// <param name="deptName">科室名称</param>
        /// <param name="problemName">课题名称</param>
        /// <param name="problemSort">题类别</param>
        /// <param name="years">年度</param>
        /// <param name="startEndDate">课题开始终止时间</param>
        /// <param name="principal">负责人</param>
        /// <param name="principalSpeciality">负责人专业</param>
        /// <param name="principalSchoolAge">负责人学历</param>
        /// <param name="principalJob">负责人职称</param>
        /// <param name="unit">协作单位</param>
        /// <param name="mostlyPersons">主要成员</param>
        /// <param name="content">研究主要内容及预期目标</param>
        public void UpdataProblemList(string id, string deptCode, string deptName, string problemName, string problemSort, string years,
                                                    string principal, string principalSpeciality, string principalSchoolAge, string principalJob, string unit,
                                                    string content, string problemCode, string outlyType, string StartDate, string EndDate,
                                                    string outlyNum, string addMark, string Passed_Unit, string Lerver, string mark_sug, string setup_sug)
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.PROBLEM_INFO SET DEPT_CODE =?, 
                                                    DEPT_NAME =?, PROBLEM_NAME =?, PROBLEM_SORT =?, YEARS =?, 
                                                    PRINCIPAL =?, PRINCIPAL_SPECIALITY =?, 
                                                    PRINCIPAL_SCHOOL_AGE =?, PRINCIPAL_JOB =?, UNIT =?,CONTENT =?,
                                                    PROBLEM_CODE=?,OUTLAY_TYPE=?,START_DATE=?,END_DATE=?,OUTLAY_NUM=?,
                                                    ADD_MARK=?,PASSED_UNIT=?,LERVER=?,MARK_SUG=?,SETUP_SUG=? WHERE ID=?", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , deptCode ) ,
					new OleDbParameter( "" , deptName ) ,
					new OleDbParameter( "" , problemName ) ,
					new OleDbParameter( "" , problemSort ) ,
					new OleDbParameter( "" , years ) ,
					new OleDbParameter( "" , principal ) ,
					new OleDbParameter( "" , principalSpeciality ) ,
					new OleDbParameter( "" , principalSchoolAge ) ,
					new OleDbParameter( "" , principalJob ) ,
					new OleDbParameter( "" , unit ) ,
					new OleDbParameter( "" , content ) ,
					new OleDbParameter( "" , problemCode ) ,
					new OleDbParameter( "" , outlyType ) ,
					new OleDbParameter( "" , StartDate ) ,
					new OleDbParameter( "" , EndDate ) ,
					new OleDbParameter( "" , outlyNum ) ,
					new OleDbParameter( "" , addMark),
					new OleDbParameter( "" , Passed_Unit),
					new OleDbParameter( "" , Lerver),
                    new OleDbParameter( "" , mark_sug),
                    new OleDbParameter( "" , setup_sug),
					new OleDbParameter( "" , id )
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 查询作者
        /// </summary>
        /// <param name="deptCode">科室代码</param>
        /// <param name="FieldIdName">ID字段名称</param>
        /// <param name="id">ID值</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public DataSet ViewAuthor(string deptCode,string FieldIdName,string id,string TableName) 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT ID, {1}, STAFF_ID, STAFF_NAME, RANK, REMARK
                                      FROM {0}.{3} WHERE {1} = '{2}'", DataUser.RLZY, FieldIdName, id, TableName);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 插入作者
        /// </summary>
        /// <param name="FieldIdName">ID字段名称</param>
        /// <param name="TableName">表名</param>
        /// <param name="id">审批</param>
        /// <param name="StaffId">人员ID</param>
        /// <param name="StaffName">人员姓名</param>
        /// <param name="remarks">备注</param>
        /// <param name="ranking">排名</param>
        public void InsertAuthor(string FieldCodeName,string TableName,string id, string StaffId,string StaffName,string remarks,string ranking) 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.{1}
                                                (ID,{2}, STAFF_ID, STAFF_NAME, RANK, REMARK)
                                                 VALUES (?,?,?,?,?,?)", DataUser.RLZY, TableName, FieldCodeName);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "ID" , OracleOledbBase.GetMaxID("ID",DataUser.RLZY+"."+TableName)) ,
					new OleDbParameter( FieldCodeName , id ) ,
					new OleDbParameter( "STAFF_ID" , StaffId ) ,
					new OleDbParameter( "STAFF_NAME" , StaffName ) ,
					new OleDbParameter( "RANK" , ranking ),
                    new OleDbParameter( "REMARK" , remarks )
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 删除作者
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="id">id</param>
        public void DeleteAuthor(string TableName, string id) 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE FROM {0}.{2} WHERE ID IN ({1})", DataUser.RLZY, id, TableName);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }


        /// <summary>
        /// 查询成果
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="add_mark">审批标记</param>
        /// <param name="deptCode">科室代码</param>
        /// <param name="ThePalmGrade">获奖类别及等级</param>
        /// <param name="power">权限</param>
        /// <returns></returns>
        public DataSet ViewFruitList(string year, string add_mark, string deptCode, string ThePalmGrade, string power)
        {
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            ThePalmGrade = ThePalmGrade == "全部" ? "" : "AND BEAR_THE_PALM_GRADE LIKE '%" + ThePalmGrade + "%'";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   ID, DEPT_CODE, DEPT_NAME, FRUIT_NAME, MOSTLY_UNIT, MOSTLY_PERSONS,
                                     MOSTLY_PERSONS_SCHOOL_AGE, MOSTLY_PERSONS_JOB, MOSTLY_JON_PERSONS,
                                     BEAR_THE_PALM_DATE, BEAR_THE_PALM_GRADE, FRUIT_KIND, TASK_SOURCE,
                                     PATENT, NEW_READ_NUMBER, THEMATIC, SUMMARY, FRUIT_CODE, AUTH_UNIT,
                                     ISEXTEND_APP, EXTEND_APP_BOUND, EXTEND_INCOME, PATENT_SORT,
                                     PATENT_INCOME, MARK_SUG,SETUP_SUG,
                                     DECODE(ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK
                                FROM {0}.FRUIT
                                WHERE SUBSTR(BEAR_THE_PALM_DATE, 1, 4){1} {2} {3} {4} {5}
                            ORDER BY BEAR_THE_PALM_DATE DESC", DataUser.RLZY, year, mark, deptCode, ThePalmGrade, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        //liu.shh  2012.12.18
        public DataSet ViewFruitListPerson(string year, string add_mark, string deptCode, string ThePalmGrade, string power, string userId)
        {
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            ThePalmGrade = ThePalmGrade == "全部" ? "" : "AND BEAR_THE_PALM_GRADE LIKE '%" + ThePalmGrade + "%'";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            userId = userId == "" ? "" : "AND USER_ID = '" + userId + "'";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   ID, DEPT_CODE, DEPT_NAME, FRUIT_NAME, MOSTLY_UNIT, MOSTLY_PERSONS,
                                     MOSTLY_PERSONS_SCHOOL_AGE, MOSTLY_PERSONS_JOB, MOSTLY_JON_PERSONS,
                                     BEAR_THE_PALM_DATE, BEAR_THE_PALM_GRADE, FRUIT_KIND, TASK_SOURCE,
                                     PATENT, NEW_READ_NUMBER, THEMATIC, SUMMARY, FRUIT_CODE, AUTH_UNIT,
                                     ISEXTEND_APP, EXTEND_APP_BOUND, EXTEND_INCOME, PATENT_SORT,
                                     PATENT_INCOME, MARK_SUG,SETUP_SUG,
                                     DECODE(ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK
                                FROM {0}.FRUIT
                                WHERE SUBSTR(BEAR_THE_PALM_DATE, 1, 4){1} {2} {3} {4} {5} {6}
                            ORDER BY BEAR_THE_PALM_DATE DESC", DataUser.RLZY, year, mark, deptCode, ThePalmGrade, power, userId);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 添加成果信息
        /// </summary>
        /// <param name="deptCode">科室编号</param>
        /// <param name="deptName">科室名称</param>
        /// <param name="fruitName">成果名称</param>
        /// <param name="mostlyUnit">主要完成单位</param>
        /// <param name="mostyPersons">主要完成人员</param>
        /// <param name="mostlyPersonsSchoolAge">主要完成人学历</param>
        /// <param name="mostlyPersonsJob">主要完成人职称</param>
        /// <param name="mostlyJonPersons">主要参加者</param>
        /// <param name="bearThePalmDate">获奖时间</param>
        /// <param name="bearThePalmGrade">获奖类别及等级</param>
        /// <param name="fruitKind">成果性质</param>
        /// <param name="taskSource">任务来源</param>
        /// <param name="patent">专利号</param>
        /// <param name="newReadNumber">新药批文号</param>
        /// <param name="thematic">主题词</param>
        /// <param name="summary">成果摘要</param>
        public void InsertFruitList(string deptCode, string deptName, string fruitName, string mostlyUnit, string mostyPersons,
                                    string mostlyPersonsSchoolAge, string mostlyPersonsJob, string mostlyJonPersons, string bearThePalmDate,
                                    string bearThePalmGrade, string fruitKind, string taskSource, string patent, string newReadNumber, string thematic,
                                    string summary, string fruitCode, string authUnit, string isExtend, string extendBound, string patentSort,
                                    string extendIncome, string patentIncome, string Enter_date, string Enter_pers, string add_mark, string user_id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.FRUIT
                                            (ID,DEPT_CODE, DEPT_NAME, FRUIT_NAME, MOSTLY_UNIT, MOSTLY_PERSONS, 
                                            MOSTLY_PERSONS_SCHOOL_AGE, MOSTLY_PERSONS_JOB, 
                                            MOSTLY_JON_PERSONS, BEAR_THE_PALM_DATE, BEAR_THE_PALM_GRADE, 
                                            FRUIT_KIND, TASK_SOURCE, PATENT, NEW_READ_NUMBER, THEMATIC, 
                                            SUMMARY,FRUIT_CODE,AUTH_UNIT,ISEXTEND_APP,EXTEND_APP_BOUND,EXTEND_INCOME,
                                            PATENT_SORT,PATENT_INCOME,ENTER_DATE,ENTER_PERS,ADD_MARK,USER_ID) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , OracleOledbBase.GetMaxID("ID","RLZY.FRUIT") ) ,
					new OleDbParameter( "" , deptCode ) ,
					new OleDbParameter( "" , deptName ) ,
					new OleDbParameter( "" , fruitName ) ,
					new OleDbParameter( "" , mostlyUnit ) ,
					new OleDbParameter( "" , mostyPersons ) ,
					new OleDbParameter( "" , mostlyPersonsSchoolAge ) ,
					new OleDbParameter( "" , mostlyPersonsJob ) ,
					new OleDbParameter( "" , mostlyJonPersons ) ,
					new OleDbParameter( "" , bearThePalmDate ) ,
					new OleDbParameter( "" , bearThePalmGrade ) ,
					new OleDbParameter( "" , fruitKind ) ,
					new OleDbParameter( "" , taskSource ) ,
					new OleDbParameter( "" , patent ) ,
					new OleDbParameter( "" , newReadNumber ) ,
					new OleDbParameter( "" , thematic ) ,
					new OleDbParameter( "" , summary ) ,
				    new OleDbParameter( "" , fruitCode ) ,
				    new OleDbParameter( "" , authUnit ) ,
				    new OleDbParameter( "" , isExtend ) ,
				    new OleDbParameter( "" , extendBound ) ,
				    new OleDbParameter( "" ,extendIncome) ,
				    new OleDbParameter( "" , patentSort),
				    new OleDbParameter( "" , patentIncome),
				    new OleDbParameter( "" , Enter_date),
				    new OleDbParameter( "" , Enter_pers),
                    new OleDbParameter( "" , add_mark),
                    new OleDbParameter( "" , user_id)
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 更新成果信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="deptCode">科室编号</param>
        /// <param name="deptName">科室名称</param>
        /// <param name="fruitName">成果名称</param>
        /// <param name="mostlyUnit">主要完成单位</param>
        /// <param name="mostyPersons">主要完成人员</param>
        /// <param name="mostlyPersonsSchoolAge">主要完成人学历</param>
        /// <param name="mostlyPersonsJob">主要完成人职称</param>
        /// <param name="mostlyJonPersons">主要参加者</param>
        /// <param name="bearThePalmDate">获奖时间</param>
        /// <param name="bearThePalmGrade">获奖类别及等级</param>
        /// <param name="fruitKind">成果性质</param>
        /// <param name="taskSource">任务来源</param>
        /// <param name="patent">专利号</param>
        /// <param name="newReadNumber">新药批文号</param>
        /// <param name="thematic">主题词</param>
        /// <param name="summary">成果摘要</param>
        public void UpdateFruitList(string id, string deptCode, string deptName, string fruitName, string mostlyUnit, string mostyPersons,
            string mostlyPersonsSchoolAge, string mostlyPersonsJob, string mostlyJonPersons, string bearThePalmDate,
            string bearThePalmGrade, string fruitKind, string taskSource, string patent, string newReadNumber,
            string thematic, string summary, string fruitCode, string authUnit, string isExtend, string extendBound,
            string patentSort, string extendIncome, string patentIncome, string addMark, string mark_sug, string setup_sug)
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.FRUIT
                                        SET DEPT_CODE =?, DEPT_NAME =?, FRUIT_NAME =?, MOSTLY_UNIT =?, 
                                        MOSTLY_PERSONS =?, MOSTLY_PERSONS_SCHOOL_AGE =?, 
                                        MOSTLY_PERSONS_JOB =?, MOSTLY_JON_PERSONS =?, BEAR_THE_PALM_DATE =?, 
                                        BEAR_THE_PALM_GRADE =?, FRUIT_KIND =?, TASK_SOURCE =?, PATENT =?, 
                                        NEW_READ_NUMBER =?, THEMATIC =?, SUMMARY =? ,FRUIT_CODE=?,AUTH_UNIT=?,
                                        ISEXTEND_APP=?,EXTEND_APP_BOUND=?,EXTEND_INCOME=?,PATENT_SORT=?,
                                        PATENT_INCOME=?,ADD_MARK=?,MARK_SUG=?,SETUP_SUG=? WHERE ID=?", DataUser.RLZY);

            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , deptCode ) ,
					new OleDbParameter( "" , deptName ) ,
					new OleDbParameter( "" , fruitName ) ,
					new OleDbParameter( "" , mostlyUnit ) ,
					new OleDbParameter( "" , mostyPersons ) ,
					new OleDbParameter( "" , mostlyPersonsSchoolAge ) ,
					new OleDbParameter( "" , mostlyPersonsJob ) ,
					new OleDbParameter( "" , mostlyJonPersons ) ,
					new OleDbParameter( "" , bearThePalmDate ) ,
					new OleDbParameter( "" , bearThePalmGrade ) ,
					new OleDbParameter( "" , fruitKind ) ,
					new OleDbParameter( "" , taskSource ) ,
					new OleDbParameter( "" , patent ) ,
					new OleDbParameter( "" , newReadNumber ) ,
					new OleDbParameter( "" , thematic ) ,
					new OleDbParameter( "" , summary ) ,
					new OleDbParameter( "" , fruitCode ) ,
				    new OleDbParameter( "" , authUnit ) ,
				    new OleDbParameter( "" , isExtend ) ,
				    new OleDbParameter( "" , extendBound ) ,				    
				    new OleDbParameter( "" , extendIncome),
					new OleDbParameter( "" , patentSort ) ,
				    new OleDbParameter( "" , patentIncome),
					new OleDbParameter( "" , addMark),
                    new OleDbParameter( "" , mark_sug),
                    new OleDbParameter( "" , setup_sug),
					new OleDbParameter( "" , id )
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 查询论文
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="add_mark">审批标记</param>
        /// <param name="deptCode">科室代码</param>
        /// <param name="TheGrade">等级</param>
        /// <param name="power">权限</param>
        /// <returns></returns>
        public DataSet ViewDiscourseList(string year, string add_mark, string deptCode, string TheGrade, string power, string IsSource)
        {
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            TheGrade = TheGrade == "全部" ? "" : "AND PUBLICATION_GRADE = '" + TheGrade + "'";
            IsSource = IsSource == "全部" ? "" : "AND IS_SOURCE = '" + IsSource + "'";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT    ID, DEPT_CODE, DEPT_NAME, YEARS, PUBLICATION_NAME, DISCOURSE_TITLE,
                                         DISCOURSE_SUBJECT, PUBLICATION_GRADE, YEAR, BOOK, EXPECT, AUTHOR,
                                         IS_SOURCE, MAGA_NO, MEET_NAME, MEET_DATE, DISCOU_LEVEL, PUBLISH,
                                         PUBLISH_DATE, DUTY, RECORD_DATE, ENTER_PERS, MARK_SUG,SETUP_SUG,
                                         DECODE(ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK
                                    FROM RLZY.DISCOURSE
                                WHERE TO_NUMBER(YEARS){1} {2} {3} {4} {5} {6}
                            ORDER BY YEARS DESC, EXPECT DESC", DataUser.RLZY, year, mark, deptCode, TheGrade, power, IsSource);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        //liu.shh  2012.12.18
        public DataSet ViewDiscourseListPerson(string year, string add_mark, string deptCode, string TheGrade, string power, string IsSource, string userId)
        {
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            TheGrade = TheGrade == "全部" ? "" : "AND PUBLICATION_GRADE = '" + TheGrade + "'";
            IsSource = IsSource == "全部" ? "" : "AND IS_SOURCE = '" + IsSource + "'";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            userId = userId == "" ? "" : "AND USER_ID = '" + userId + "'";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT    ID, DEPT_CODE, DEPT_NAME, YEARS, PUBLICATION_NAME, DISCOURSE_TITLE,
                                         DISCOURSE_SUBJECT, PUBLICATION_GRADE, YEAR, BOOK, EXPECT, AUTHOR,
                                         IS_SOURCE, MAGA_NO, MEET_NAME, MEET_DATE, DISCOU_LEVEL, PUBLISH,
                                         PUBLISH_DATE, DUTY, RECORD_DATE, ENTER_PERS, MARK_SUG,SETUP_SUG,
                                         DECODE(ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK
                                    FROM RLZY.DISCOURSE
                                WHERE TO_NUMBER(YEARS){1} {2} {3} {4} {5} {6} {7}
                            ORDER BY YEARS DESC, EXPECT DESC", DataUser.RLZY, year, mark, deptCode, TheGrade, power, IsSource, userId);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 添加论文信息
        /// </summary>
        /// <param name="deptCode">科室编号</param>
        /// <param name="deptName">科室名称</param>
        /// <param name="years">年度</param>
        /// <param name="publicationName">刊物名称</param>
        /// <param name="discourseTitle">论文题目</param>
        /// <param name="discourseSubject">论文栏目</param>
        /// <param name="publicationGrade">刊物等级</param>
        /// <param name="year">年</param>
        /// <param name="book">卷</param>
        /// <param name="expect">期</param>
        /// <param name="author">作者</param>
        /// <param name="isSource">是否统计源期刊</param>
        public void InsertDiscourseList(string deptCode, string deptName, string years, string publicationName, string discourseTitle,
                                        string discourseSubject, string publicationGrade, string year, string book, string expect, string author,
                                        string isSource, string discouGrade, string magaNo, string duty, string meetName, string meetDate,
                                        string Publish, string publishDate, string Enter_pers, string add_mark, string user_id)
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.DISCOURSE
                                            (ID,DEPT_CODE, DEPT_NAME, YEARS, PUBLICATION_NAME, DISCOURSE_TITLE, 
                                            DISCOURSE_SUBJECT, PUBLICATION_GRADE, YEAR, BOOK, EXPECT, AUTHOR, 
                                            IS_SOURCE,DISCOU_LEVEL,MAGA_NO,DUTY,MEET_NAME,MEET_DATE,PUBLISH,PUBLISH_DATE,ADD_MARK,
                                            ENTER_PERS,RECORD_DATE,USER_ID) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , OracleOledbBase.GetMaxID("ID",DataUser.RLZY+".DISCOURSE")) ,
					new OleDbParameter( "" , deptCode ) ,
					new OleDbParameter( "" , deptName ) ,
					new OleDbParameter( "" , years ) ,
					new OleDbParameter( "" , publicationName ) ,
					new OleDbParameter( "" , discourseTitle ) ,
					new OleDbParameter( "" , discourseSubject ) ,
					new OleDbParameter( "" , publicationGrade ) ,
					new OleDbParameter( "" , year ) ,
					new OleDbParameter( "" , book ) ,
					new OleDbParameter( "" , expect ) ,
					new OleDbParameter( "" , author ) ,
					new OleDbParameter( "" , isSource ),
					new OleDbParameter( "" , discouGrade ) ,
					new OleDbParameter( "" , magaNo ) ,
					new OleDbParameter( "" , duty ) ,
					new OleDbParameter( "" , meetName ) ,
					new OleDbParameter( "" , meetDate ) ,
					new OleDbParameter( "" , Publish ),
					new OleDbParameter( "" , publishDate ) ,
                    new OleDbParameter( "" , add_mark ) ,
				    new OleDbParameter( "" , Enter_pers),
                    new OleDbParameter( "" , DateTime.Now.ToString("yyyy-MM-dd")),
                    new OleDbParameter( "" , user_id)
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 更新论文信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="deptCode">科室编号</param>
        /// <param name="deptName">科室名称</param>
        /// <param name="years">年度</param>
        /// <param name="publicationName">刊物名称</param>
        /// <param name="discourseTitle">论文题目</param>
        /// <param name="discourseSubject">论文栏目</param>
        /// <param name="publicationGrade">刊物等级</param>
        /// <param name="year">年</param>
        /// <param name="book">卷</param>
        /// <param name="expect">期</param>
        /// <param name="author">作者</param>
        /// <param name="isSource">是否统计源期刊</param>
        public void UpdateDiscourseList(string id, string deptCode, string deptName, string years, string publicationName, string discourseTitle,
                                        string discourseSubject, string publicationGrade, string year, string book, string expect, string author, string isSource,
                                        string discouGrade, string magaNo, string duty, string meetName, string meetDate, string Publish, string publishDate,
                                        string addmark, string mark_sug, string setup_sug)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.DISCOURSE
                                    SET DEPT_CODE =?, DEPT_NAME =?, YEARS =?, PUBLICATION_NAME =?, 
                                    DISCOURSE_TITLE =?, DISCOURSE_SUBJECT =?, PUBLICATION_GRADE =?, 
                                    YEAR =?, BOOK =?, EXPECT =?, AUTHOR =?, IS_SOURCE =?,
                                    DISCOU_LEVEL=?,MAGA_NO=?,DUTY=?,MEET_NAME=?,MEET_DATE=?,
                                    PUBLISH=?,PUBLISH_DATE=? ,ADD_MARK=?,MARK_SUG=?,SETUP_SUG=? WHERE ID=?", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , deptCode ) ,
					new OleDbParameter( "" , deptName ) ,
					new OleDbParameter( "" , years ) ,
					new OleDbParameter( "" , publicationName ) ,
					new OleDbParameter( "" , discourseTitle ) ,
					new OleDbParameter( "" , discourseSubject ) ,
					new OleDbParameter( "" , publicationGrade ) ,
					new OleDbParameter( "" , year ) ,
					new OleDbParameter( "" , book ) ,
					new OleDbParameter( "" , expect ) ,
					new OleDbParameter( "" , author ) ,
					new OleDbParameter( "" , isSource ) ,
					new OleDbParameter( "" , discouGrade ) ,
					new OleDbParameter( "" , magaNo ) ,
					new OleDbParameter( "" , duty ) ,
					new OleDbParameter( "" , meetName ) ,
					new OleDbParameter( "" , meetDate ) ,
					new OleDbParameter( "" , Publish ),
					new OleDbParameter( "" , publishDate ),
					new OleDbParameter( "" , addmark),
                    new OleDbParameter( "" , mark_sug),
                    new OleDbParameter( "" , setup_sug),
					new OleDbParameter( "" , id )
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 查询专著
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="add_mark">审批标记</param>
        /// <param name="deptCode">科室代码</param>
        /// <param name="power">权限</param>
        /// <returns></returns>
        public DataSet ViewMonographList(string year, string add_mark, string deptCode, string power)
        {
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   ID, DEPT_CODE, DEPT_NAME, MONOGRAPH_NAME, PUBLISH, PUBLISH_DATE,
                                         WORD_COUNT, CALL_NUMBER, FORMAT, AMOUNT, AUTHOR, CONTENT,
                                         DISCOU_LEVEL, MAGA_NO, DUTY, MEET_NAME, MEET_DATE, MAGA_NAME,MARK_SUG,SETUP_SUG,
                                         DECODE(ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK
                                    FROM {0}.MONOGRAPH
                                WHERE SUBSTR(PUBLISH_DATE, 1, 4){1} {2} {3} {4}
                            ORDER BY PUBLISH_DATE DESC", DataUser.RLZY, year, mark, deptCode, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        //liu.shh  2012.12.18
        public DataSet ViewMonographListPerson(string year, string add_mark, string deptCode, string power, string userId)
        {
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            userId = userId == "" ? "" : "AND USER_ID = '" + userId + "'";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   ID, DEPT_CODE, DEPT_NAME, MONOGRAPH_NAME, PUBLISH, PUBLISH_DATE,
                                         WORD_COUNT, CALL_NUMBER, FORMAT, AMOUNT, AUTHOR, CONTENT,
                                         DISCOU_LEVEL, MAGA_NO, DUTY, MEET_NAME, MEET_DATE, MAGA_NAME,MARK_SUG,SETUP_SUG,
                                         DECODE(ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK
                                    FROM {0}.MONOGRAPH
                                WHERE SUBSTR(PUBLISH_DATE, 1, 4){1} {2} {3} {4} {5}
                            ORDER BY PUBLISH_DATE DESC", DataUser.RLZY, year, mark, deptCode, power, userId);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 添加专著信息
        /// </summary>
        /// <param name="deptCode">科室编号</param>
        /// <param name="deptName">科室名称</param>
        /// <param name="monographName">专著名称</param>
        /// <param name="publish">出版社</param>
        /// <param name="publishDate">出版日期</param>
        /// <param name="wordCount">字数</param>
        /// <param name="callNumber">图书编号</param>
        /// <param name="format">开本</param>
        /// <param name="amount">印刷数量</param>
        /// <param name="author">作者</param>
        /// <param name="content">内容简介</param>
        public void InsertMonographlist(string deptCode, string deptName, string monographName, string publish, string publishDate,
                                            string wordCount, string callNumber, string format, string amount, string author, string content, string discouGrade,
                                            string magaNo, string duty, string meetName, string meetDate, string magaName, string enterDate, string Enter_pers,
                                            string add_mark, string user_id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.MONOGRAPH
										(ID,DEPT_CODE, DEPT_NAME, MONOGRAPH_NAME, PUBLISH, PUBLISH_DATE, 
										WORD_COUNT, CALL_NUMBER, FORMAT, AMOUNT, AUTHOR, CONTENT,DISCOU_LEVEL,
                                        MAGA_NO,DUTY,MEET_NAME,MEET_DATE,MAGA_NAME,RECORD_DATE,ENTER_PERS,ADD_MARK,USER_ID)
                                        VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.RLZY);

            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , OracleOledbBase.GetMaxID("ID",DataUser.RLZY+".MONOGRAPH") ) ,
					new OleDbParameter( "" , deptCode ) ,
					new OleDbParameter( "" , deptName ) ,
					new OleDbParameter( "" , monographName ) ,
					new OleDbParameter( "" , publish ) ,
					new OleDbParameter( "" , publishDate ) ,
					new OleDbParameter( "" , wordCount ) ,
					new OleDbParameter( "" , callNumber ) ,
					new OleDbParameter( "" , format ) ,
					new OleDbParameter( "" , amount ) ,
					new OleDbParameter( "" , author ) ,
					new OleDbParameter( "" , content ) ,
					new OleDbParameter( "" , discouGrade ) ,
					new OleDbParameter( "" , magaNo ) ,
					new OleDbParameter( "" , duty ) ,
					new OleDbParameter( "" , meetName ) ,
					new OleDbParameter( "" , meetDate ) ,
					new OleDbParameter( "" , magaName ) ,
					new OleDbParameter( "" , enterDate ),
					new OleDbParameter( "" , Enter_pers),
				    new OleDbParameter( "" , add_mark),
                    new OleDbParameter( "" , user_id)
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 更新专著信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deptCode">科室编号</param>
        /// <param name="deptName">科室名称</param>
        /// <param name="monographName">专著名称</param>
        /// <param name="publish">出版社</param>
        /// <param name="publishDate">出版日期</param>
        /// <param name="wordCount">字数</param>
        /// <param name="callNumber">图书编号</param>
        /// <param name="format">开本</param>
        /// <param name="amount">印刷数量</param>
        /// <param name="author">作者</param>
        /// <param name="content">内容简介</param>
        public void UpdateMonographList(string id, string deptCode, string deptName, string monographName, string publish, string publishDate,
                                        string wordCount, string callNumber, string format, string amount, string author, string content, string discouGrade,
                                        string magaNo, string duty, string meetName, string meetDate, string magaName, string addMark, string mark_sug, string setup_sug)
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.MONOGRAPH
                                        SET DEPT_CODE =?, DEPT_NAME =?, MONOGRAPH_NAME =?, PUBLISH =?, PUBLISH_DATE =?, 
                                        WORD_COUNT =?, CALL_NUMBER =?, FORMAT =?, AMOUNT =?, AUTHOR =?, CONTENT =?,
                                        DISCOU_LEVEL=?,MAGA_NO=?,DUTY=?,MEET_NAME=?,MEET_DATE=?,MAGA_NAME=?,ADD_MARK=?,
                                        MARK_SUG=?,SETUP_SUG=?  WHERE ID=?", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , deptCode ) ,
					new OleDbParameter( "" , deptName ) ,
					new OleDbParameter( "" , monographName ) ,
					new OleDbParameter( "" , publish ) ,
					new OleDbParameter( "" , publishDate ) ,
					new OleDbParameter( "" , wordCount ) ,
					new OleDbParameter( "" , callNumber ) ,
					new OleDbParameter( "" , format ) ,
					new OleDbParameter( "" , amount ) ,
					new OleDbParameter( "" , author ) ,
					new OleDbParameter( "" , content ) ,
					new OleDbParameter( "" , discouGrade ) ,
					new OleDbParameter( "" , magaNo ) ,
					new OleDbParameter( "" , duty ) ,
					new OleDbParameter( "" , meetName ) ,
					new OleDbParameter( "" , meetDate ) ,
					new OleDbParameter( "" , magaName ) ,
					new OleDbParameter( "" , addMark),
                    new OleDbParameter( "" , mark_sug),
                    new OleDbParameter( "" , setup_sug),
					new OleDbParameter( "" , id )

				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 查询专著
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="add_mark">审批标记</param>
        /// <param name="deptCode">科室代码</param>
        /// <param name="power">权限</param>
        /// <returns></returns>
        public DataSet ViewDevelopNewTecList(string year, string add_mark, string deptCode, string power)
        {
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT ID, DEPT_CODE, DEPT_NAME, NEW_TECHNIC, NAME, PRINCIPAL,
                                       PRINCIPAL_SCHOOL_AGE, PRINCIPAL_JOB, JOIN_PERSONS, DATES, BRIEF,
                                       CLUB_DEPT, COMP_CASE, LEVELCOL, EFFECT, STAFF_ID, MARK_SUG,SETUP_SUG,
                                       DECODE(ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK
                                  FROM {0}.DEVELOP_NEW_TECHNIC
                                WHERE SUBSTR(DATES ,1,4){1} {2} {3} {4}
                            ORDER BY DATES DESC", DataUser.RLZY, year, mark, deptCode, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        //以个人身份查看  liu.shh  2012.12.18
        public DataSet ViewDevelopNewTecListPerson(string year, string add_mark, string deptCode, string power, string userId)
        {
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            userId = userId == "" ? "" : "AND USER_ID = '" + userId + "'";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT ID, DEPT_CODE, DEPT_NAME, NEW_TECHNIC, NAME, PRINCIPAL,
                                       PRINCIPAL_SCHOOL_AGE, PRINCIPAL_JOB, JOIN_PERSONS, DATES, BRIEF,
                                       CLUB_DEPT, COMP_CASE, LEVELCOL, EFFECT, STAFF_ID, MARK_SUG,SETUP_SUG,
                                       DECODE(ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK
                                  FROM {0}.DEVELOP_NEW_TECHNIC
                                WHERE SUBSTR(DATES ,1,4){1} {2} {3} {4} {5}
                            ORDER BY DATES DESC", DataUser.RLZY, year, mark, deptCode, power, userId);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 添加开展新业务新技术信息
        /// </summary>
        /// <param name="deptCode">科室编号</param>
        /// <param name="deptName">科室名称</param>
        /// <param name="newTechnic">新技术新业务</param>
        /// <param name="name">名称</param>
        /// <param name="principal">负责人</param>
        /// <param name="principalSchoolAge">负责人学历</param>
        /// <param name="principalJob">负责人职称</param>
        /// <param name="joinPersons">参加人员</param>
        /// <param name="date">起始时间</param>
        /// <param name="brief">项目简介</param>
        public void InsertDevelopNewTechnic(string deptCode, string deptName, string newTechnic, string name, string principal,
                                            string principalSchoolAge, string principalJob, string joinPersons, string date,
                                            string brief, string clubDept, string compCase, string level, string effect,
                                            string enterDate, string Enter_pers, string addmark, string staff_id, string user_id)
        {


            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.DEVELOP_NEW_TECHNIC
											(ID,DEPT_CODE, DEPT_NAME, NEW_TECHNIC, NAME, PRINCIPAL, 
											PRINCIPAL_SCHOOL_AGE, PRINCIPAL_JOB, JOIN_PERSONS, DATES, 
                                            BRIEF,CLUB_DEPT,COMP_CASE,LEVELCOL,EFFECT,ENTER_DATE,ENTER_PERS,ADD_MARK,STAFF_ID,USER_ID)
                                            VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , OracleOledbBase.GetMaxID("ID",DataUser.RLZY+".DEVELOP_NEW_TECHNIC")  ) ,
					new OleDbParameter( "" , deptCode ) ,
					new OleDbParameter( "" , deptName ) ,
					new OleDbParameter( "" , newTechnic ) ,
					new OleDbParameter( "" , name ) ,
					new OleDbParameter( "" , principal ) ,
					new OleDbParameter( "" , principalSchoolAge ) ,
					new OleDbParameter( "" , principalJob ) ,
					new OleDbParameter( "" , joinPersons ) ,
					new OleDbParameter( "" , date ) ,
					new OleDbParameter( "" , brief ),
					new OleDbParameter( "" , clubDept),
					new OleDbParameter( "" , compCase),
					new OleDbParameter( "" , level),
					new OleDbParameter( "" , effect),
					new OleDbParameter( "" ,enterDate),
					new OleDbParameter( "" ,Enter_pers),
				    new OleDbParameter( "" ,addmark),
					new OleDbParameter( "" ,staff_id),
                    new OleDbParameter( "" ,user_id)
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }


        /// <summary>
        /// 更新开展新业务新技术信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="deptCode">科室编号</param>
        /// <param name="deptName">科室名称</param>
        /// <param name="newTechnic">新技术新业务</param>
        /// <param name="name">名称</param>
        /// <param name="principal">负责人</param>
        /// <param name="principalSchoolAge">负责人学历</param>
        /// <param name="principalJob">负责人职称</param>
        /// <param name="joinPersons">参加人员</param>
        /// <param name="date">起始时间</param>
        /// <param name="brief">项目简介</param>
        public void UpdateDevelopNewTechnic(string id, string deptCode, string deptName, string newTechnic, string name,
                                                string principal, string principalSchoolAge, string principalJob, string joinPersons,
                                                string date, string brief, string clubDept, string compCase, string level, string effect,
                                                string addMark, string staff_id, string mark_sug, string setup_sug)
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.DEVELOP_NEW_TECHNIC SET DEPT_CODE =?, DEPT_NAME =?,
										NEW_TECHNIC =?, NAME =?, PRINCIPAL =?, PRINCIPAL_SCHOOL_AGE =?, 
										PRINCIPAL_JOB =?, JOIN_PERSONS =?, DATES =?, BRIEF =? ,
                                        CLUB_DEPT =?,COMP_CASE=?,LEVELCOL=?,EFFECT=?,ADD_MARK=?,
                                        STAFF_ID=?,MARK_SUG=?,SETUP_SUG=? WHERE ID=?", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , deptCode ) ,
					new OleDbParameter( "" , deptName ) ,
					new OleDbParameter( "" , newTechnic ) ,
					new OleDbParameter( "" , name ) ,
					new OleDbParameter( "" , principal ) ,
					new OleDbParameter( "" , principalSchoolAge ) ,
					new OleDbParameter( "" , principalJob ) ,
					new OleDbParameter( "" , joinPersons ) ,
					new OleDbParameter( "" , date ) ,
					new OleDbParameter( "" , brief ) ,
					new OleDbParameter( "" , clubDept),
				    new OleDbParameter( "" , compCase),
                    new OleDbParameter( "" , level),
				    new OleDbParameter( "" , effect),
					new OleDbParameter( "" , addMark),
					new OleDbParameter( "" , staff_id),
                    new OleDbParameter( "" , mark_sug),
                    new OleDbParameter( "" , setup_sug),
					new OleDbParameter( "" , id)
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }


        /// <summary>
        /// 查询学术会议
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="add_mark">审批标记</param>
        /// <param name="deptCode">科室代码</param>
        /// <param name="power">权限</param>
        /// <returns></returns>
        public DataSet ViewTeciMeet(string FromDate, string Todate, string add_mark, string deptCode, string power)
        {
            deptCode = deptCode == "" ? "" : "AND SPEC_CODE = '" + deptCode + "'";
            power = power == "" ? "" : "AND SPEC_CODE IN (" + power + ")";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   T1.ID, T2.DEPT_NAME, T1.SPEC_CODE, T1.MEET_NAME, T1.GRADE,
                                         T1.MEET_DATE, T1.SCIENCE_MEETING_PLACE, T1.JOIN_PERSONS, T1.CONTENT,T1.MARK_SUG,T1.SETUP_SUG,
                                         DECODE(T1.ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK
                                    FROM (SELECT T3.ID, T3.SPEC_CODE, T3.MEET_NAME, T3.GRADE, T3.MEET_DATE,
                                                 T3.SCIENCE_MEETING_PLACE, T5.CONCAT JOIN_PERSONS, T3.CONTENT,T3.MARK_SUG,T3.SETUP_SUG,
                                                 T3.ADD_MARK
                                            FROM {0}.TECI_MEET T3,
                                                 (SELECT   WMSYS.WM_CONCAT (MEET_PERSONNEL) AS CONCAT,
                                                           MEET_ID
                                                      FROM {0}.TECI_MEET_PERSONNEL T4
                                                  GROUP BY T4.MEET_ID) T5
                                           WHERE T3.ID = T5.MEET_ID(+)) T1,
                                         {6}.SYS_DEPT_DICT T2
                                WHERE T2.DEPT_CODE = T1.SPEC_CODE 
                                      AND TO_DATE(MEET_DATE,'yyyy-MM-dd') >= TO_DATE('{1}','yyyy-MM-dd') 
                                      AND TO_DATE(MEET_DATE,'yyyy-MM-dd') <= TO_DATE('{2}','yyyy-MM-dd') 
                                      {3} {4} {5}
                            ORDER BY MEET_DATE DESC", DataUser.RLZY, FromDate, Todate, mark, deptCode, power, DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        //liu.shh  2012.12.18
        public DataSet ViewTeciMeetPerson(string FromDate, string Todate, string add_mark, string deptCode, string power, string userId)
        {
            deptCode = deptCode == "" ? "" : "AND SPEC_CODE = '" + deptCode + "'";
            power = power == "" ? "" : "AND SPEC_CODE IN (" + power + ")";
            userId = userId == "" ? "" : "AND T1.USER_ID = '" + userId + "'";
            string mark = "";
            if (add_mark.Equals("1"))
            {
                mark = "AND ADD_MARK IN ('1','-1')";
            }
            else
            {
                mark = "AND ADD_MARK=" + add_mark + "";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   T1.ID, T2.DEPT_NAME, T1.SPEC_CODE, T1.MEET_NAME, T1.GRADE,
                                         T1.MEET_DATE, T1.SCIENCE_MEETING_PLACE, T1.JOIN_PERSONS, T1.CONTENT,T1.MARK_SUG,T1.SETUP_SUG,
                                         DECODE(T1.ADD_MARK,'1','审批通过','-1','审批未通过','3','未提交','0','审批中') AS ADD_MARK
                                    FROM (SELECT T3.ID, T3.SPEC_CODE, T3.MEET_NAME, T3.GRADE, T3.MEET_DATE,
                                                 T3.SCIENCE_MEETING_PLACE, T5.CONCAT JOIN_PERSONS, T3.CONTENT,T3.MARK_SUG,T3.SETUP_SUG,
                                                 T3.ADD_MARK,T3.USER_ID
                                            FROM {0}.TECI_MEET T3,
                                                 (SELECT   WMSYS.WM_CONCAT (MEET_PERSONNEL) AS CONCAT,
                                                           MEET_ID
                                                      FROM {0}.TECI_MEET_PERSONNEL T4
                                                  GROUP BY T4.MEET_ID) T5
                                           WHERE T3.ID = T5.MEET_ID(+)) T1,
                                         {6}.SYS_DEPT_DICT T2
                                WHERE T2.DEPT_CODE = T1.SPEC_CODE 
                                      AND TO_DATE(MEET_DATE,'yyyy-MM-dd') >= TO_DATE('{1}','yyyy-MM-dd') 
                                      AND TO_DATE(MEET_DATE,'yyyy-MM-dd') <= TO_DATE('{2}','yyyy-MM-dd') 
                                      {3} {4} {5} {7}
                            ORDER BY MEET_DATE DESC", DataUser.RLZY, FromDate, Todate, mark, deptCode, power, DataUser.COMM, userId);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 添加学术会议
        /// </summary>		
        public void InsertTeciMeet(string Spec_CODE, string Meet_NAME, string Grade, string Meet_date,
                                    string Enter_pers, string Enter_time, string place, string joinpersons, string content, string addmark, string user_id)
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.TECI_MEET
                                                    (ID,SPEC_CODE, MEET_NAME, GRADE, MEET_DATE,ENTER_PERS,ENTER_TIME,SCIENCE_MEETING_PLACE,
                                                        JOIN_PERSONS,CONTENT,ADD_MARK,USER_ID) 
                                                   VALUES (?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , OracleOledbBase.GetMaxID("ID",DataUser.RLZY+".TECI_MEET") ) ,
					new OleDbParameter( "" , Spec_CODE ) ,
					new OleDbParameter( "" , Meet_NAME ) ,
					new OleDbParameter( "" , Grade ) ,
					new OleDbParameter( "" , Meet_date ),
				    new OleDbParameter( "" , Enter_pers ),
				    new OleDbParameter( "" , Enter_time ),
					new OleDbParameter( "" , place ),
				    new OleDbParameter( "" , joinpersons ),
				    new OleDbParameter( "" , content ),
				    new OleDbParameter( "" , addmark),
                    new OleDbParameter( "" , user_id)
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }
        /// <summary>
        /// 更新学术会议
        ///<summary>
        public void UpdateTeciMeet(string id, string Meet_NAME, string Grade, string Meet_date,
                                          string place, string joinpersons, string content, string add_mark, string mark_sug, string setup_sug)
        {

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.TECI_MEET
                                        SET  MEET_NAME =?, GRADE =?, MEET_DATE =?,ADD_MARK=?,SCIENCE_MEETING_PLACE=?,JOIN_PERSONS=?,CONTENT=?,MARK_SUG=?,SETUP_SUG=? 
                                        WHERE ID=?", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					
					new OleDbParameter( "" , Meet_NAME ) ,
					new OleDbParameter( "" , Grade ) ,
					new OleDbParameter( "" , Meet_date ),
					new OleDbParameter( "" , add_mark),
					new OleDbParameter( "" , place ),
				    new OleDbParameter( "" , joinpersons ),
				    new OleDbParameter( "" , content ),
                    new OleDbParameter( "" , mark_sug ),
                    new OleDbParameter( "" , setup_sug ),
                    new OleDbParameter("",id)
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 查看会议参加人员
        /// </summary>
        /// <param name="meetid">会议ID</param>
        /// <returns></returns>
        public DataSet ViewTeciMeetJoinPerson(string meetid) 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   T2.MEET_ID, T1.MEET_NAME, T2.MEET_PERSONNEL, T2.STAFF_ID, REMARK,
                                         T2.ID
                                    FROM {0}.TECI_MEET T1, {0}.TECI_MEET_PERSONNEL T2
                                   WHERE T2.MEET_ID = T1.ID
                                         AND T1.ID = '{1}'
                                ORDER BY MEET_DATE DESC", DataUser.RLZY, meetid);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 插入会议参加人员
        /// </summary>
        /// <param name="MEET_ID">学术会议CODE</param>
        /// <param name="MEET_NAME">学术会议名称</param>
        /// <param name="MEET_PERSONNEL">学术会议人员</param>
        /// <param name="STAFF_ID"></param>
        /// <param name="REMARK">备注</param>
        public void InsertTeciMeetJoinPerson(string meetid,string meetname,string staffName,string staffId,string remark) 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO {0}.TECI_MEET_PERSONNEL (MEET_ID,MEET_NAME,MEET_PERSONNEL,STAFF_ID,REMARK,ID) 
                                                       VALUES (?,?,?,?,?,?)",DataUser.RLZY);
            OleDbParameter[] prams = new OleDbParameter[]
            {
                new OleDbParameter("",meetid),
                new OleDbParameter("",meetname),
                new OleDbParameter("",staffName),
                new OleDbParameter("",staffId),
                new OleDbParameter("",remark),
                new OleDbParameter("",OracleOledbBase.GetMaxID("ID",DataUser.RLZY+".TECI_MEET_PERSONNEL"))
            };
            OracleOledbBase.ExecuteNonQuery(str.ToString(), prams);
        }


        /// <summary>
        /// 查看人才培养记录
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="deptCode">科室代码</param>
        /// <param name="power">权限</param>
        /// <returns></returns>
        public DataSet ViewPersonsPlant(string FromDate, string Todate,  string deptCode, string power)
        {
            deptCode = deptCode == "" ? "" : "AND A.DEPT_CODE = '" + deptCode + "'";
            power = power == "" ? "" : "AND A.DEPT_CODE IN (" + power + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.ID, A.DEPT_CODE, A.SORT, A.PERSONS, A.UNIT, A.START_DATE, A.END_DATE,
                                       A.TERM, A.CONTENT, A.DEPT_SPECIAL, A.STAFF_ID, A.LIGHTSPOT_NAME,
                                       A.FINISHING_SITUATION, A.CHECK_RESULT, B.DEPT_NAME
                                  FROM {0}.PERSONS_PLANT_INFO A, {1}.SYS_DEPT_DICT B
                                 WHERE A.DEPT_CODE = B.DEPT_CODE(+)   
                                       AND TO_DATE(A.START_DATE,'yyyy-MM-dd')>= TO_DATE('{2}','yyyy-MM-dd')
                                       AND TO_DATE(A.END_DATE,'yyyy-MM-dd')<= TO_DATE('{3}','yyyy-MM-dd') {4} {5}", DataUser.RLZY,DataUser.COMM,FromDate,Todate,deptCode,power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 添加人才培养信息
        /// </summary>
        /// <param name="deptCode">科室编号</param>
        /// <param name="sort">类别</param>
        /// <param name="persons">人员</param>
        /// <param name="unit">单位或地址</param>
        /// <param name="startDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="term">期限</param>
        /// <param name="content">具体内容</param>
        public void InsertPersonsPlant(string deptCode, string sort, string persons, string unit, string startDate, string endDate,
                                            string term, string content, string dept_special, string staff_id, string LightspotName, 
                                            string FinishingSituation, string CheckResult)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO  {0}.PERSONS_PLANT_INFO
													(ID,DEPT_CODE, SORT, PERSONS, UNIT, START_DATE, END_DATE, TERM, CONTENT,DEPT_SPECIAL,STAFF_ID,
                                                        LIGHTSPOT_NAME,FINISHING_SITUATION,CHECK_RESULT)
													VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , OracleOledbBase.GetMaxID("ID",DataUser.RLZY+".PERSONS_PLANT_INFO") ) ,
					new OleDbParameter( "" , deptCode ) ,
					new OleDbParameter( "" , sort ) ,
					new OleDbParameter( "" , persons ) ,
					new OleDbParameter( "" , unit ) ,
					new OleDbParameter( "" , startDate ) ,
					new OleDbParameter( "" , endDate ) ,
					new OleDbParameter( "" , term ) ,
					new OleDbParameter( "" , content ),
					new OleDbParameter( "" , dept_special ),
					new OleDbParameter( "" , staff_id ),
					new OleDbParameter( "" , LightspotName ),
					new OleDbParameter( "" , FinishingSituation ),
					new OleDbParameter( "" , CheckResult )

				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }


        /// <summary>
        /// 更新人才培养信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="deptCode">科室编号</param>
        /// <param name="sort">类别</param>
        /// <param name="persons">人员</param>
        /// <param name="unit">单位或地址</param>
        /// <param name="startDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="term">期限</param>
        /// <param name="content">具体内容</param>
        public void UpdatePersonsPlant(string id, string deptCode, string sort, string persons, string unit, string startDate,
                                                string endDate, string term, string content, string dept_special, string staff_id, 
                                                string LightspotName, string FinishingSituation, string CheckResult)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.PERSONS_PLANT_INFO
														SET SORT =?, PERSONS =?, UNIT =?, START_DATE =?, 
														END_DATE =?, TERM =?,CONTENT =?,DEPT_SPECIAL=?,STAFF_ID=?,
                                                        LIGHTSPOT_NAME=?,FINISHING_SITUATION=?,CHECK_RESULT=? WHERE ID=?", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					
					new OleDbParameter( "" , sort ) ,
					new OleDbParameter( "" , persons ) ,
					new OleDbParameter( "" , unit ) ,
					new OleDbParameter( "" , startDate ) ,
					new OleDbParameter( "" , endDate ) ,
					new OleDbParameter( "" , term ) ,
					new OleDbParameter( "" , content ) ,
					new OleDbParameter( "" , dept_special ) ,
					new OleDbParameter( "" , staff_id ) ,
					new OleDbParameter( "" , LightspotName ),
					new OleDbParameter( "" , FinishingSituation ),
					new OleDbParameter( "" , CheckResult ),
					new OleDbParameter( "" , id )

				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 删除人才培养信息
        /// </summary>
        /// <param name="id">序号</param>
        public void DelPersonsPlant(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE  {0}.PERSONS_PLANT_INFO WHERE ID=?", DataUser.RLZY);
            OracleOledbBase.ExecuteNonQuery(str.ToString(), new OleDbParameter("", id));
        }

        /// <summary>
        /// 查看科室工作总结
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="deptCode">科室代码</param>
        /// <param name="power">权限</param>
        /// <returns></returns>s
        public DataSet ViewDeptWorkList(string FromDate, string Todate, string deptCode, string power)
        {
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT ID, WORK_DATE, DEPT_CODE, DEPT_NAME, INPUT_USER, WORK_DETAIL
                                 FROM {0}.DEPT_WORK_DETAIL
                                 WHERE TO_DATE(WORK_DATE,'yyyy-MM-dd')>= TO_DATE('{1}','yyyy-MM-dd')
                                       AND TO_DATE(WORK_DATE,'yyyy-MM-dd')<= TO_DATE('{2}','yyyy-MM-dd') {3} {4}", DataUser.RLZY, FromDate, Todate, deptCode, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 添加科室工作总结
        /// </summary>
        public void InsertDeptWorkList(string workdate, string deptcode, string deptname, string inputuser, string workdetail)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO {0}.DEPT_WORK_DETAIL 
                                (ID,WORK_DATE,DEPT_CODE,DEPT_NAME,INPUT_USER,WORK_DETAIL) VALUES (?,?,?,?,?,?)", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , OracleOledbBase.GetMaxID("ID",DataUser.RLZY+".DEPT_WORK_DETAIL") ) ,
					new OleDbParameter( "" , workdate ) ,
					new OleDbParameter( "" , deptcode ) ,
					new OleDbParameter( "" , deptname ) ,
					new OleDbParameter( "" , inputuser ) ,
					new OleDbParameter( "" , workdetail ) ,
					
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 更新工作总结
        /// </summary>
        /// <param name="txtwork">汇报</param>
        /// <param name="id"></param>
        public  void UpdateDeptWorkList(string txtwork, string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE {0}.DEPT_WORK_DETAIL SET WORK_DETAIL='" + txtwork + "' WHERE ID=" + id,DataUser.RLZY);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 删除工作总结
        /// </summary>
        /// <param name="id"></param>
        public void DeleteDeptWorkList(string id) 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE FROM  {0}.DEPT_WORK_DETAIL WHERE ID=" + id, DataUser.RLZY);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }


        /// <summary>
        /// 查看医保外联总结
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="deptCode">科室代码</param>
        /// <param name="power">权限</param>
        /// <returns></returns>s
        public DataSet ViewOutYBList(string FromDate, string Todate, string deptCode, string power)
        {
            deptCode = deptCode == "" ? "" : "AND DEPT_CODE = '" + deptCode + "'";
            power = power == "" ? "" : "AND DEPT_CODE IN (" + power + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT ID, WORK_DATE, DEPT_CODE, DEPT_NAME, INPUT_USER, WORK_DETAIL
                                 FROM {0}.DEPT_YB_DETAIL
                                 WHERE TO_DATE(WORK_DATE,'yyyy-MM-dd')>= TO_DATE('{1}','yyyy-MM-dd')
                                       AND TO_DATE(WORK_DATE,'yyyy-MM-dd')<= TO_DATE('{2}','yyyy-MM-dd') {3} {4}", DataUser.RLZY, FromDate, Todate, deptCode, power);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 添加医保外联总结
        /// </summary>
        public void InsertOutYBList(string workdate, string deptcode, string deptname, string inputuser, string workdetail)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO {0}.DEPT_YB_DETAIL 
                                (ID,WORK_DATE,DEPT_CODE,DEPT_NAME,INPUT_USER,WORK_DETAIL) VALUES (?,?,?,?,?,?)", DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[]
				{
					new OleDbParameter( "" , OracleOledbBase.GetMaxID("ID",DataUser.RLZY+".DEPT_YB_DETAIL") ) ,
					new OleDbParameter( "" , workdate ) ,
					new OleDbParameter( "" , deptcode ) ,
					new OleDbParameter( "" , deptname ) ,
					new OleDbParameter( "" , inputuser ) ,
					new OleDbParameter( "" , workdetail ) ,
					
				};
            OracleOledbBase.ExecuteNonQuery(str.ToString(), cmdPara);
        }

        /// <summary>
        /// 更新医保外联总结
        /// </summary>
        /// <param name="txtwork">汇报</param>
        /// <param name="id"></param>
        public void UpdateOutYBList(string txtwork, string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE {0}.DEPT_YB_DETAIL SET WORK_DETAIL='" + txtwork + "' WHERE ID=" + id, DataUser.RLZY);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 删除医保外联总结
        /// </summary>
        /// <param name="id"></param>
        public void DeleteOutYBList(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE FROM  {0}.DEPT_YB_DETAIL WHERE ID=" + id, DataUser.RLZY);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 批量修改审批页面内容
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="FieldMarkName">MARK字段名称</param>
        /// <param name="FieldIdName">ID字段名称</param>
        /// <param name="id">ID值</param>
        /// <param name="add_mark">MARK值</param>
        public void EchoUpDataStaffListHandle(string TableName, string FieldMarkName, string FieldIdName, string id, string add_mark,string Mark_userName,string userdate)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"UPDATE  {0}.{3}
                                        SET {4}='{1}',MARK_USER = '{6}',USER_DATE = '{7}'
                                        WHERE {5} IN ({2})", DataUser.RLZY, add_mark, id, TableName, FieldMarkName, FieldIdName, Mark_userName,userdate);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }



    }
}
