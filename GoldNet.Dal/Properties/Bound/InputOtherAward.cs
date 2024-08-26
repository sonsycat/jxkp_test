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
    public class InputOtherAward
    {
        //添加/修改其他奖项类别
        public void SaveIndicationConfig(int id, string typename, string Value, string REMARK)
        {
            StringBuilder str = new StringBuilder();
            if (id.Equals(0))
            {
                id = OracleOledbBase.GetMaxID("ID", DataUser.CBHS + ".CBHS_SET_INDICATIONCONFIG");
                str.AppendFormat("insert into {0}.CBHS_SET_INDICATIONCONFIG(id,name,PROPERTYVALUE,REMARK,TAG) values (?,?,?,?,?) ", DataUser.CBHS);
                OleDbParameter[] parameters = {
											  new OleDbParameter("ID",id),
											  new OleDbParameter("NAME", typename),
                                              new OleDbParameter("PROPERTYVALUE",Value),
											  new OleDbParameter("REMARK", REMARK),
                                              new OleDbParameter("TAG", "0")
											 
										  };
                OracleOledbBase.ExecuteNonQuery(str.ToString(), parameters);
            }
            else
            {
                str.AppendFormat("update {0}.CBHS_SET_INDICATIONCONFIG set NAME=?,PROPERTYVALUE=?,REMARK=? where id={1}", DataUser.CBHS, id);

                OleDbParameter[] parameters = {
											  new OleDbParameter("NAME", typename),
											  new OleDbParameter("PROPERTYVALUE", Value),
                                               new OleDbParameter("REMARK", REMARK)
                                              };
                OracleOledbBase.ExecuteNonQuery(str.ToString(), parameters);
            }
        }
        /// <summary>
        /// 其他奖惩类别
        /// </summary>
        /// <returns></returns>
        public DataTable GetIndicationConfig(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select * from {0}.CBHS_SET_INDICATIONCONFIG", DataUser.CBHS);
            if (!id.Equals(""))
            {
                str.AppendFormat(" where id={0}", id);
            }
            str.Append(" order by id");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        //删除其他奖惩类别
        public void DeleteIndicationConfig(string id)
        {
            string str = string.Format("delete {0}.CBHS_SET_INDICATIONCONFIG where id={1}", DataUser.CBHS, id);
            OracleOledbBase.ExecuteNonQuery(str);
        }
        #region 工作量参数
        /// <summary>
        /// 类别
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetWordTypeList()
        {
            string sql = "select * from CBHS.CBHS_SET_WORDLOAD_DICT";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return ds.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetWordloadList(string condition)
        {
            string sql = "select ID,to_char(INPUT_DATE,'yyyy-mm-dd') as INPUTDATE,DEPT_NAME as DEPTNAME,REASON as REASON,round(MONEY,2) MONEY,OTHER_DICT_NAME from CBHS.CBHS_WORKLOAD_INFO where DEL_FLAG=0 and (" + condition + ") order by INPUTDATE desc";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildOtherAward();
            }

        }
        /// <summary>
        /// 查询效益调整数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetWordloadList()
        {
            string sql = "select ID,to_char(INPUT_DATE,'yyyy-mm-dd') as INPUTDATE,DEPT_NAME as DEPTNAME,REASON as REASON,round(MONEY,2) MONEY,OTHER_DICT_NAME from  CBHS.CBHS_WORKLOAD_INFO where DEL_FLAG=0 order by INPUTDATE desc";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildOtherAward();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DeleteWordload(string id)
        {
            string sql = " update CBHS.CBHS_WORKLOAD_INFO set DEL_FLAG=1 where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }


        public DataTable GetWorkloadBytime(string condition, string time, string endtime, string typeId, string dept)
        {
            string sql = "select ID,to_char(INPUT_DATE,'yyyy-mm-dd') as INPUTDATE,DEPT_NAME as DEPTNAME,REASON as REASON,round(MONEY,2) MONEY,OTHER_DICT_NAME from CBHS.CBHS_WORKLOAD_INFO where DEL_FLAG=0 and (" + condition + ") and INPUT_DATE > to_date('" + time + "','yyyymm')  and  INPUT_DATE < add_months(to_date('" + endtime + "','yyyymm'),1) ";
            if (typeId != "")
            {
                sql += "  and OTHER_DICT_ID ='" + typeId + "' ";
            }
            if (dept != "")
            {
                sql += "  and dept_code ='" + dept + "' ";
            }
            sql += "  order by INPUTDATE desc ";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildOtherAward();
            }

        }

        public DataTable GetWorkloadBytimes(string time, string endtime, string typeId, string dept)
        {
            string sql = "select ID,to_char(INPUT_DATE,'yyyy-mm-dd') as INPUTDATE,DEPT_NAME as DEPTNAME,REASON as REASON,round(MONEY,2) MONEY,OTHER_DICT_NAME from CBHS.CBHS_WORKLOAD_INFO where DEL_FLAG=0  and INPUT_DATE > to_date('" + time + "','yyyymm')  and  INPUT_DATE < add_months(to_date('" + endtime + "','yyyymm'),1)  ";
            if (typeId != "")
            {
                sql += "  and OTHER_DICT_ID ='" + typeId + "' ";
            }
            if (dept != "")
            {
                sql += "  and dept_code ='" + dept + "' ";
            }
            sql += "  order by INPUTDATE desc ";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildOtherAward();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetWorkType()
        {

            string sql = "select ID TYPEID,NAME TYPENAME from CBHS.CBHS_SET_WORDLOAD_DICT";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildSimpleType();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetWorkType(string id)
        {

            string sql = "select ID TYPEID,NAME TYPENAME,EDITABLE from CBHS.CBHS_SET_WORDLOAD_DICT where ID='" + id + "'";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildSimpleType();
            }
        }
        /// <summary>
        /// 根据ID查询效益调整数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetWordloadSimple(string id)
        {

            string sql = "select ID,DEPT_CODE,DEPT_NAME,OTHER_DICT_ID,OTHER_DICT_NAME,REASON,round(MONEY,2) MONEY,INPUT_DATE,INPUTER,INPUTE_DATE,MODIFIER,MODIFY_DATE from CBHS.CBHS_WORKLOAD_INFO where ID=" + id + "";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }

        }
        public void InsertWordload(string deptname, string deptid, string dictid, string dictname, string reason, double money, DateTime inputdate, string inputer)
        {
            string sql = " insert into CBHS.CBHS_WORKLOAD_INFO(ID,DEPT_CODE,DEPT_NAME,OTHER_DICT_ID,OTHER_DICT_NAME,REASON,MONEY,INPUT_DATE,INPUTER,INPUTE_DATE,DEL_FLAG) ";
            sql += " select nvl(MAX (ID),0) + 1,'" + deptid + "','" + deptname + "','" + dictid + "','" + dictname + "','" + reason + "'," + money + ", ";
            sql += " to_date('" + inputdate.ToString("yyyyMMdd") + "','yyyymmdd'),'" + inputer + "',sysdate,0";
            sql += " from CBHS.CBHS_WORKLOAD_INFO ";
            OracleOledbBase.ExecuteNonQuery(sql);
        }
        public void UpdateWordload(string id, string deptname, string deptid, string dictid, string dictname, string reason, double money, DateTime inputdate, string modifier)
        {
            string sql = " update CBHS.CBHS_WORKLOAD_INFO set DEPT_CODE='" + deptid + "',DEPT_NAME='" + deptname + "',OTHER_DICT_ID='" + dictid + "',OTHER_DICT_NAME='" + dictname + "',";
            sql += " REASON='" + reason + "',MONEY=" + money + ",INPUT_DATE=to_date('" + inputdate.ToString("yyyyMMdd") + "','yyyymmdd'),MODIFIER='" + modifier + "',";
            sql += " MODIFY_DATE=sysdate  where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 其他奖惩类别
        /// </summary>
        /// <returns></returns>
        public DataTable GetWorkloadType(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select * from {0}.CBHS_SET_WORDLOAD_DICT", DataUser.CBHS);
            if (!id.Equals(""))
            {
                str.AppendFormat(" where id={0}", id);
            }
            str.Append(" order by id");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        //删除其他奖惩类别
        public void DeleteWorkloadtype(string id)
        {
            string str = string.Format("delete {0}.CBHS_SET_WORDLOAD_DICT where id={1}", DataUser.CBHS, id);
            OracleOledbBase.ExecuteNonQuery(str);
        }
        //添加/修改其他奖项类别
        public void SaveWorkloadType(int id, string typename)
        {
            StringBuilder str = new StringBuilder();
            if (id.Equals(0))
            {
                id = OracleOledbBase.GetMaxID("ID", DataUser.CBHS + ".CBHS_SET_WORDLOAD_DICT");
                str.AppendFormat("insert into {0}.CBHS_SET_WORDLOAD_DICT(id,name) values (?,?) ", DataUser.CBHS);
                OleDbParameter[] parameters = {
											  new OleDbParameter("ID",id),
											  new OleDbParameter("NAME", typename)
											 
										  };
                OracleOledbBase.ExecuteNonQuery(str.ToString(), parameters);
            }
            else
            {
                str.AppendFormat("update {0}.CBHS_SET_WORDLOAD_DICT set name=? where id={1}", DataUser.CBHS, id);

                OleDbParameter[] parameters = {
											  new OleDbParameter("NAME", typename)
											 
                                              };
                OracleOledbBase.ExecuteNonQuery(str.ToString(), parameters);
            }
        }


        public DataTable GetWorkloadByCopy(string dictid, string inputdate, string inputdateTo)
        {
            string sql = "";
            sql += " select ID,DEPT_CODE,DEPT_NAME,OTHER_DICT_ID,OTHER_DICT_NAME,REASON,MONEY,to_date('" + inputdateTo + "','yyyymm') input_date, ";
            sql += " INPUTER,INPUTE_DATE,DEL_FLAG from CBHS.CBHS_WORKLOAD_INFO ";
            sql += " where to_char(input_date,'yyyymm') ='" + inputdate + "' and del_flag = 0 and OTHER_DICT_ID=" + dictid + " ";
            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        public void InsertWorkloadByCopy(DataTable dt, string dictid, string inputdate, string inputdateTo, DateTime to)
        {
            MyLists listtable = new MyLists();
            int numCount = OracleOledbBase.GetMaxID("ID", DataUser.CBHS + ".CBHS_WORKLOAD_INFO");

            string sql = "update CBHS.CBHS_WORKLOAD_INFO set del_flag =1 WHERE TO_CHAR(input_date,'yyyymm')='" + inputdateTo + "' AND del_flag=0 AND  OTHER_DICT_ID=" + dictid + " ";
            OracleOledbBase.ExecuteNonQuery(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"insert into CBHS.CBHS_WORKLOAD_INFO (ID,DEPT_CODE,DEPT_NAME,OTHER_DICT_ID,OTHER_DICT_NAME,REASON,MONEY,INPUT_DATE,INPUTER,INPUTE_DATE,DEL_FLAG)
                                        VALUES(?,?,?,?,?,?,?,to_date('" + inputdateTo + "','yyyymm'),?,SYSDATE,?)");
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("ID",numCount+i),
											  new OleDbParameter("DEPT_CODE",dt.Rows[i]["DEPT_CODE"]),
											  new OleDbParameter("DEPT_NAME",dt.Rows[i]["DEPT_NAME"]),
											  new OleDbParameter("OTHER_DICT_ID",dt.Rows[i]["OTHER_DICT_ID"]),
											  new OleDbParameter("OTHER_DICT_NAME",dt.Rows[i]["OTHER_DICT_NAME"]),
											  new OleDbParameter("REASON",dt.Rows[i]["REASON"]),
											  new OleDbParameter("MONEY",dt.Rows[i]["MONEY"]),
											 // new OleDbParameter("INPUT_DATE",date),//0为录入
											  new OleDbParameter("INPUTER",dt.Rows[i]["INPUTER"]),
                                              //new OleDbParameter("INPUTE_DATE",dt.Rows[i]["INPUTE_DATE"]),
											  new OleDbParameter("DEL_FLAG",dt.Rows[i]["DEL_FLAG"]),
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);

            }
            OracleOledbBase.ExecuteTranslist(listtable);

        }


        #endregion

        public void InsertOtherAwardByCopy(DataTable dt, string dictid, string inputdate, string inputdateTo, DateTime to)
        {
            MyLists listtable = new MyLists();
            int numCount = OracleOledbBase.GetMaxID("ID", DataUser.PERFORMANCE + ".INPUT_OtherAward");
            //DateTime date = DateTime.Parse(to.ToString("yyyymm"));
            string sql = "update PERFORMANCE.INPUT_OtherAward set del_flag =1 WHERE TO_CHAR(input_date,'yyyymm')='" + inputdateTo + "' AND del_flag=0 AND  OTHER_DICT_ID=" + dictid + " ";
            OracleOledbBase.ExecuteNonQuery(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"insert into PERFORMANCE.INPUT_OtherAward (ID,DEPT_CODE,DEPT_NAME,OTHER_DICT_ID,OTHER_DICT_NAME,REASON,MONEY,INPUT_DATE,INPUTER,INPUTE_DATE,DEL_FLAG)
                                        VALUES(?,?,?,?,?,?,?,to_date('" + inputdateTo + "','yyyymm'),?,SYSDATE,?)");
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("ID",numCount+i),
											  new OleDbParameter("DEPT_CODE",dt.Rows[i]["DEPT_CODE"]),
											  new OleDbParameter("DEPT_NAME",dt.Rows[i]["DEPT_NAME"]),
											  new OleDbParameter("OTHER_DICT_ID",dt.Rows[i]["OTHER_DICT_ID"]),
											  new OleDbParameter("OTHER_DICT_NAME",dt.Rows[i]["OTHER_DICT_NAME"]),
											  new OleDbParameter("REASON",dt.Rows[i]["REASON"]),
											  new OleDbParameter("MONEY",dt.Rows[i]["MONEY"]),
											 // new OleDbParameter("INPUT_DATE",date),//0为录入
											  new OleDbParameter("INPUTER",dt.Rows[i]["INPUTER"]),
                                              //new OleDbParameter("INPUTE_DATE",dt.Rows[i]["INPUTE_DATE"]),
											  new OleDbParameter("DEL_FLAG",dt.Rows[i]["DEL_FLAG"]),
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);

            }
            OracleOledbBase.ExecuteTranslist(listtable);

        }
        public DataTable GetOtherAwardByCopy(string dictid, string inputdate, string inputdateTo)
        {
            string sql = "";//" insert into PERFORMANCE.INPUT_OtherAward (ID,DEPT_CODE,DEPT_NAME,OTHER_DICT_ID,OTHER_DICT_NAME,REASON,MONEY,INPUT_DATE,INPUTER,INPUTE_DATE,DEL_FLAG) ";
            sql += " select ID,DEPT_CODE,DEPT_NAME,OTHER_DICT_ID,OTHER_DICT_NAME,REASON,MONEY,to_date('" + inputdateTo + "','yyyymm') input_date, ";
            sql += " INPUTER,INPUTE_DATE,DEL_FLAG from PERFORMANCE.INPUT_OtherAward ";
            sql += " where to_char(input_date,'yyyymm') ='" + inputdate + "' and del_flag = 0 and OTHER_DICT_ID=" + dictid + " ";
            //sql += " group by DEPT_CODE,DEPT_NAME,OTHER_DICT_ID,OTHER_DICT_NAME,REASON,MONEY,  INPUTER,INPUTE_DATE,DEL_FLAG ";
            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }
        public DataTable GetOtherAwardListBytimes(string time, string endtime, string typeId, string dept)
        {
            string sql = "select ID,to_char(INPUT_DATE,'yyyy-mm-dd') as INPUTDATE,DEPT_NAME as DEPTNAME,REASON as REASON,round(MONEY,2) MONEY from PERFORMANCE.INPUT_OtherAward where DEL_FLAG=0  and INPUT_DATE > to_date('" + time + "','yyyymm')  and  INPUT_DATE < add_months(to_date('" + endtime + "','yyyymm'),1)  ";
            if (typeId != "")
            {
                sql += "  and OTHER_DICT_ID ='" + typeId + "' ";
            }
            if (dept != "")
            {
                sql += "  and dept_code ='" + dept + "' ";
            }
            sql += "  order by INPUTDATE desc ";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildOtherAward();
            }

        }
        public DataTable GetOtherAwardListBytime(string condition, string time, string endtime, string typeId, string dept)
        {
            string sql = "select ID,to_char(INPUT_DATE,'yyyy-mm-dd') as INPUTDATE,DEPT_NAME as DEPTNAME,REASON as REASON,round(MONEY,2) MONEY from PERFORMANCE.INPUT_OtherAward where DEL_FLAG=0 and (" + condition + ") and INPUT_DATE > to_date('" + time + "','yyyymm')  and  INPUT_DATE < add_months(to_date('" + endtime + "','yyyymm'),1) ";
            if (typeId != "")
            {
                sql += "  and OTHER_DICT_ID ='" + typeId + "' ";
            }
            if (dept != "")
            {
                sql += "  and dept_code ='" + dept + "' ";
            }
            sql += "  order by INPUTDATE desc ";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildOtherAward();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetOtherAwardList(string condition)
        {
            string sql = "select ID,to_char(INPUT_DATE,'yyyy-mm-dd') as INPUTDATE,DEPT_NAME as DEPTNAME,REASON as REASON,round(MONEY,2) MONEY from PERFORMANCE.INPUT_OtherAward where DEL_FLAG=0 and (" + condition + ") order by INPUTDATE desc";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildOtherAward();
            }

        }
        /// <summary>
        /// 奖项类别
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetOtherTypeList()
        {
            string sql = "select * from PERFORMANCE.SET_OTHERAWARD_DICT";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return ds.Tables[0];
        }
        /// <summary>
        /// 查询效益调整数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetOtherAwardList()
        {
            string sql = "select ID,to_char(INPUT_DATE,'yyyy-mm-dd') as INPUTDATE,DEPT_NAME as DEPTNAME,REASON as REASON,round(MONEY,2) MONEY from PERFORMANCE.INPUT_OtherAward where DEL_FLAG=0 order by INPUTDATE desc";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildOtherAward();
            }

        }
        public DataTable GetOtherAwardList(string condition, string datetime, string othertype)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select ID,to_char(INPUT_DATE,'yyyy-mm-dd') as INPUTDATE,DEPT_NAME as DEPTNAME,REASON as REASON,round(MONEY,2) MONEY,dept_code,OTHER_DICT_ID,OTHER_DICT_NAME from PERFORMANCE.INPUT_OtherAward where DEL_FLAG=0 ");
            if (condition != "")
            {
                sql.AppendFormat(" and {0}", condition);
            }
            if (datetime != "")
            {
                sql.AppendFormat(" and to_char(INPUT_DATE,'yyyyMM')='{0}'", datetime);
            }
            if (othertype != "")
            {
                sql.AppendFormat(" and OTHER_DICT_ID='{0}'", othertype);
            }
            sql.Append(" order by INPUTE_DATE desc");
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildOtherAward();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetSimpleType()
        {

            string sql = "select ID TYPEID,NAME TYPENAME from PERFORMANCE.SET_OTHERAWARD_DICT";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildSimpleType();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetSimpleType(string id)
        {

            string sql = "select ID TYPEID,NAME TYPENAME,EDITABLE from PERFORMANCE.SET_OTHERAWARD_DICT where ID='" + id + "'";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildSimpleType();
            }
        }

        /// <summary>
        /// 根据ID查询效益调整数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetOtherAwardSimple(string id)
        {

            string sql = "select ID,DEPT_CODE,DEPT_NAME,OTHER_DICT_ID,OTHER_DICT_NAME,REASON,round(MONEY,2) MONEY,INPUT_DATE,INPUTER,INPUTE_DATE,MODIFIER,MODIFY_DATE from PERFORMANCE.INPUT_OtherAward where ID=" + id + "";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 设置表结构
        /// </summary>
        /// <returns></returns>
        private DataTable BuildOtherAward()
        {
            DataTable dt = new DataTable();
            DataColumn dcID = new DataColumn("ID");
            DataColumn dcDEPTNAME = new DataColumn("DEPTNAME");
            DataColumn dcINPUTDATE = new DataColumn("INPUTDATE");
            DataColumn dcREASON = new DataColumn("REASON");
            DataColumn dcMONEY = new DataColumn("MONEY");
            dt.Columns.AddRange(new DataColumn[] { dcID, dcDEPTNAME, dcINPUTDATE, dcREASON, dcMONEY });
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable BuildSimpleType()
        {
            DataTable dt = new DataTable();
            DataColumn dcTYPEID = new DataColumn("TYPEID");
            DataColumn dcTYPENAME = new DataColumn("TYPENAME");
            dt.Columns.AddRange(new DataColumn[] { dcTYPEID, dcTYPENAME });
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deptname"></param>
        /// <param name="deptid"></param>
        /// <param name="dictid"></param>
        /// <param name="dictname"></param>
        /// <param name="reason"></param>
        /// <param name="money"></param>
        /// <param name="inputdate"></param>
        /// <param name="modifier"></param>
        public void UpdateOtherAward(string id, string deptname, string deptid, string dictid, string dictname, string reason, double money, DateTime inputdate, string modifier)
        {
            string sql = " update PERFORMANCE.INPUT_OtherAward set DEPT_CODE='" + deptid + "',DEPT_NAME='" + deptname + "',OTHER_DICT_ID='" + dictid + "',OTHER_DICT_NAME='" + dictname + "',";
            sql += " REASON='" + reason + "',MONEY=" + money + ",INPUT_DATE=to_date('" + inputdate.ToString("yyyyMMdd") + "','yyyymmdd'),MODIFIER='" + modifier + "',";
            sql += " MODIFY_DATE=sysdate  where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptname"></param>
        /// <param name="deptid"></param>
        /// <param name="dictid"></param>
        /// <param name="dictname"></param>
        /// <param name="reason"></param>
        /// <param name="money"></param>
        /// <param name="inputdate"></param>
        /// <param name="inputer"></param>
        public void InsertOtherAward(string deptname, string deptid, string dictid, string dictname, string reason, double money, DateTime inputdate, string inputer)
        {
            string sql = " insert into PERFORMANCE.INPUT_OtherAward (ID,DEPT_CODE,DEPT_NAME,OTHER_DICT_ID,OTHER_DICT_NAME,REASON,MONEY,INPUT_DATE,INPUTER,INPUTE_DATE,DEL_FLAG) ";
            sql += " select nvl(MAX (ID),0) + 1,'" + deptid + "','" + deptname + "','" + dictid + "','" + dictname + "','" + reason + "'," + money + ", ";
            sql += " to_date('" + inputdate.ToString("yyyyMMdd") + "','yyyymmdd'),'" + inputer + "',sysdate,0";
            sql += " from PERFORMANCE.INPUT_OtherAward ";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dt"></param>
        /// <param name="inputer"></param>
        /// <param name="othertype"></param>
        public void SaveOtherAward(string date, DataTable dt, string inputer, string othertype)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            strDel.AppendFormat("DELETE FROM {0}.INPUT_OTHERAWARD WHERE TO_CHAR(INPUT_DATE,'yyyymm')='{1}'", DataUser.PERFORMANCE, date);
            if (othertype != "")
            {
                strDel.AppendFormat(" and OTHER_DICT_ID='{0}'", othertype);
            }
            List listDel = new List();
            listDel.StrSql = strDel.ToString();
            listtable.Add(listDel);
            int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.INPUT_OTHERAWARD", DataUser.PERFORMANCE));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO {0}.INPUT_OTHERAWARD 
                                          (ID,DEPT_CODE,DEPT_NAME,OTHER_DICT_ID,OTHER_DICT_NAME,REASON,MONEY,INPUT_DATE,INPUTER,INPUTE_DATE)
                                        VALUES(?,?,?,?,?,?,?,to_date('" + date + "01" + "','yyyymmdd'),?,sysdate)", DataUser.PERFORMANCE);

                OleDbParameter[] parameteradd = {
											  new OleDbParameter("ID",id),
											  new OleDbParameter("DEPT_CODE",dt.Rows[i]["DEPT_CODE"].ToString()),
											  new OleDbParameter("DEPT_NAME",dt.Rows[i]["DEPTNAME"].ToString()),
											  new OleDbParameter("OTHER_DICT_ID",dt.Rows[i]["OTHER_DICT_ID"].ToString()),
											  new OleDbParameter("OTHER_DICT_NAME",dt.Rows[i]["OTHER_DICT_NAME"].ToString()),
											  new OleDbParameter("REASON",dt.Rows[i]["REASON"].ToString()),
											  new OleDbParameter("MONEY",dt.Rows[i]["MONEY"]),
                                              new OleDbParameter("INPUTER",inputer)
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);
                id++;

            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DeleteOtherAward(string id)
        {
            string sql = " update PERFORMANCE.INPUT_OtherAward set DEL_FLAG=1 where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 其他奖惩类别
        /// </summary>
        /// <returns></returns>
        public DataTable GetOtherAwardType(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select * from {0}.SET_OTHERAWARD_DICT", DataUser.PERFORMANCE);
            if (!id.Equals(""))
            {
                str.AppendFormat(" where id={0}", id);
            }
            str.Append(" order by id");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        //删除其他奖惩类别
        public void DeleteOtherAwardtype(string id)
        {
            string str = string.Format("delete {0}.SET_OTHERAWARD_DICT where id={1}", DataUser.PERFORMANCE, id);
            OracleOledbBase.ExecuteNonQuery(str);
        }

        //添加/修改其他奖项类别
        public void SaveOtherAwardType(int id, string typename)
        {
            StringBuilder str = new StringBuilder();
            if (id.Equals(0))
            {
                id = OracleOledbBase.GetMaxID("ID", DataUser.PERFORMANCE + ".SET_OTHERAWARD_DICT");
                str.AppendFormat("insert into {0}.SET_OTHERAWARD_DICT(id,name) values (?,?) ", DataUser.PERFORMANCE);
                OleDbParameter[] parameters = {
											  new OleDbParameter("ID",id),
											  new OleDbParameter("NAME", typename)
											 
										  };
                OracleOledbBase.ExecuteNonQuery(str.ToString(), parameters);
            }
            else
            {
                str.AppendFormat("update {0}.SET_OTHERAWARD_DICT set name=? where id={1}", DataUser.PERFORMANCE, id);

                OleDbParameter[] parameters = {
											  new OleDbParameter("NAME", typename)
											 
                                              };
                OracleOledbBase.ExecuteNonQuery(str.ToString(), parameters);
            }
        }

        /// <summary>
        /// 获取科室奖金列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="datetime"></param>
        /// <param name="othertype"></param>
        /// <returns></returns>
        public DataTable GetDeptBonusList(string condition, string datetime, string othertype)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select ID,to_char(BONUS_DATE,'yyyy-mm-dd') as INPUTDATE,DEPT_NAME as DEPTNAME,REMARK as REMARK,round(MONEY,2) MONEY,dept_code from PERFORMANCE.INPUT_DEPTBONUS where DEL_FLAG=0 ");

            // 查询条件
            if (condition != "")
            {
                sql.AppendFormat(" and {0}", condition);
            }

            // 时间条件
            if (datetime != "")
            {
                sql.AppendFormat(" and to_char(BONUS_DATE,'yyyyMM')='{0}'", datetime);
            }

            sql.Append(" order by INPUTE_DATE desc");
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildOtherAward();
            }
        }

        /// <summary>
        /// 删除科室奖金
        /// </summary>
        /// <param name="id"></param>
        public void DeleteDeptBonus(string id)
        {
            string sql = " UPDATE PERFORMANCE.INPUT_DEPTBONUS set DEL_FLAG=1 where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 保存科室奖金
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dt"></param>
        /// <param name="inputer"></param>
        /// <param name="othertype"></param>
        public void SaveDeptBonus(string date, DataTable dt, string inputer, string othertype)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            strDel.AppendFormat("DELETE FROM {0}.INPUT_DEPTBONUS WHERE TO_CHAR(INPUT_DATE,'yyyymm')='{1}'", DataUser.PERFORMANCE, date);
            List listDel = new List();
            listDel.StrSql = strDel.ToString();
            listtable.Add(listDel);

            int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.INPUT_DEPTBONUS", DataUser.PERFORMANCE));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO {0}.INPUT_DEPTBONUS 
                                          (ID,DEPT_CODE,DEPT_NAME,REMARK,MONEY,BONUS_DATE,INPUTER,INPUTE_DATE)
                                        VALUES(?,?,?,?,?,?,?,to_date('" + date + "01" + "','yyyymmdd'),?,sysdate)", DataUser.PERFORMANCE);

                OleDbParameter[] parameteradd = {
											  new OleDbParameter("ID",id),
											  new OleDbParameter("DEPT_CODE",dt.Rows[i]["DEPT_CODE"].ToString()),
											  new OleDbParameter("DEPT_NAME",dt.Rows[i]["DEPTNAME"].ToString()),
											  new OleDbParameter("REMARK",dt.Rows[i]["REMARK"].ToString()),
											  new OleDbParameter("MONEY",dt.Rows[i]["MONEY"]),
                                              new OleDbParameter("INPUTER",inputer)
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);
                id++;

            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetDeptBonus(string id)
        {
            string sql = "select ID,DEPT_CODE,DEPT_NAME,REMARK,round(MONEY,2) MONEY,BONUS_DATE,INPUTER,INPUTE_DATE,MODIFIER,MODIFY_DATE from PERFORMANCE.INPUT_DEPTBONUS where ID=" + id + "";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptname"></param>
        /// <param name="deptid"></param>
        /// <param name="dictid"></param>
        /// <param name="dictname"></param>
        /// <param name="reason"></param>
        /// <param name="money"></param>
        /// <param name="inputdate"></param>
        /// <param name="inputer"></param>
        public void InsertDeptBonus(string deptname, string deptid, string reason, double money, DateTime inputdate, string inputer)
        {
            string sql = " insert into PERFORMANCE.INPUT_DEPTBONUS (ID,DEPT_CODE,DEPT_NAME,REMARK,MONEY,BONUS_DATE,INPUTER,INPUTE_DATE,DEL_FLAG) ";
            sql += " select nvl(MAX (ID),0) + 1,'" + deptid + "','" + deptname + "','" + reason + "'," + money + ", ";
            sql += " to_date('" + inputdate.ToString("yyyyMMdd") + "','yyyymmdd'),'" + inputer + "',sysdate,0";
            sql += " from PERFORMANCE.INPUT_DEPTBONUS ";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deptname"></param>
        /// <param name="deptid"></param>
        /// <param name="dictid"></param>
        /// <param name="dictname"></param>
        /// <param name="reason"></param>
        /// <param name="money"></param>
        /// <param name="inputdate"></param>
        /// <param name="modifier"></param>
        public void UpdateDeptBonusDetail(string id, string deptname, string deptid, string reason, double money, DateTime inputdate, string modifier)
        {
            string sql = " update PERFORMANCE.INPUT_DEPTBONUS set DEPT_CODE='" + deptid + "',DEPT_NAME='" + deptname + "',";
            sql += " REMARK='" + reason + "',MONEY=" + money + ",BONUS_DATE=to_date('" + inputdate.ToString("yyyyMMdd") + "','yyyymmdd'),MODIFIER='" + modifier + "',";
            sql += " MODIFY_DATE=sysdate  where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

    }
}
