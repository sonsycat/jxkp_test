using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.OleDb;
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
	public class Station_Dict
	{
        public Station_Dict()
		{}
		#region  成员方法


        /// <summary>
        /// 岗位字典信息列表
        /// </summary>
        /// <returns></returns>
        public  DataSet GetStationDictList()
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"
                    SELECT a.station_name, a.station_code_remark, a.SEQUENCE, a.input_time,a.ID, c.id typecode, c.ATTRIBUE typename,c.SORTNO
                      FROM comm.SYS_STATION_MAINTENANCE_DICT a 
                      LEFT JOIN  comm.SYS_DEPT_ATTR_DICT c
                        ON a.station_type_code = c.id
                     ORDER BY  C.SORTNO,TO_NUMBER (SEQUENCE)" );

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }

        /// <summary>
        /// 岗位字典信息树列表
        /// </summary>
        /// <returns></returns>
        public  DataSet GetStationDictTreeList()
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"
                    SELECT A.STATION_NAME,  A.SEQUENCE,A.ID, C.ID TYPECODE, C.ATTRIBUE TYPENAME
                      FROM COMM.SYS_STATION_MAINTENANCE_DICT A 
                      RIGHT JOIN  COMM.SYS_DEPT_ATTR_DICT C
                        ON A.STATION_TYPE_CODE = C.ID
                     ORDER BY C.SORTNO, TO_NUMBER (SEQUENCE)");

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }


        /// <summary>
        /// 岗位类别列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetStationTypeList()
        {
            string str = string.Format(@"
                    SELECT  c.id typecode, c.ATTRIBUE typename
                      FROM  {0}.SYS_DEPT_ATTR_DICT c
                     ORDER BY SORTNO,id ",DataUser.COMM);

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }


     

        /// <summary>
        /// 岗位字典信息列表排序
        /// </summary>
        /// <returns></returns>
        public DataSet GetStationDictListSort(string dept_type)
        {
            string str = string.Format(@" SELECT A.STATION_NAME,A.SEQUENCE,A.ID  FROM {0}.SYS_STATION_MAINTENANCE_DICT A where station_type_code = '{1}' ORDER BY station_type_code, TO_NUMBER(SEQUENCE)", DataUser.COMM, dept_type);
            return OracleOledbBase.ExecuteDataSet(str);

        }

        /// <summary>
        /// 岗位组织类别信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetDUTY_ORDER()
        {
            string sql = string.Format("select STATION_TYPE_CODE,STATION_TYPE_NAME from {0}.SYS_STATION_TYPE_DICT order by STATION_TYPE_CODE", DataUser.COMM);

            OleDbParameter[] prams = new OleDbParameter[] { };

            return OracleOledbBase.ExecuteDataSet( sql);
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
                                               ) ORDER BY GUIDE_GATHER_CODE ", DataUser.HOSPITALSYS,guide_type);
           return OracleOledbBase.ExecuteDataSet( sql);
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

            return OracleOledbBase.ExecuteDataSet( sql);
        }



        /// <summary>
        /// 岗位字典信息列表排序
        /// </summary>
        /// <returns></returns>
        public DataSet GetStationDetail(string id)
        {
        
            string str = string.Format( @" SELECT 
                                          SEQUENCE              ,
                                          STATION_NAME          ,
                                          STATION_CODE_REMARK   ,
                                          STATION_GRADE_CODE    ,
                                          STIPEND               ,
                                          WORK_CONTENT          ,
                                          POST_COMPETENCY       ,
                                          WORK_CIRCUMSTANCE     ,
                                          INPUT_USER            ,
                                          AUDITING_USER         ,
                                          GUIDE_GATHER_CODE     ,
                                          STATION_BSC_CLASS_01  ,
                                          STATION_BSC_CLASS_02  ,
                                          STATION_BSC_CLASS_03  ,
                                          STATION_BSC_CLASS_04  ,
                                          ID                    ,
                                          STATION_TYPE_CODE     ,
                                          DUTY_ORDER            ,
                                          INPUT_TIME            ,
                                          WHETHER               ,
                                          GUIDE_TYPE            
                                        FROM {0}.SYS_STATION_MAINTENANCE_DICT
                                        WHERE ID ='{1}'",DataUser.COMM,id);
            return OracleOledbBase.ExecuteDataSet(str);
        }


        /// <summary>
        /// 添加岗位字典
        /// </summary>
        public void InsertSatationDict(Dictionary<string, string> FormValues,string stationcode)
        {
            //FormValues结构如下(根据页面各控件的定义名称及类别)：
            //{[StationNameTxt, sss]}	
            //{[StationTypeCombo_Value, 0]}	
            //{[StationTypeCombo, 临床科室]}	
            //{[StationTypeCombo_SelIndex, 1]}	
            //{[SortNoNum, 7]}	
            //{[SortNoNumHidden, 7]}	
            //{[DeptDutyCombo_Value, 4]}	
            //{[DeptDutyCombo, 人]}	
            //{[DeptDutyCombo_SelIndex, 2]}	
            //{[GatherTypeCombo_Value, 0101]}	
            //{[GatherTypeCombo, 机关科室]}	
            //{[GatherTypeCombo_SelIndex, 0]}	
            //{[GuideGatherCombo_Value, 1]}	
            //{[GuideGatherCombo, 院长岗位指标集]}	
            //{[GuideGatherCombo_SelIndex, 0]}	
            //{[RadioGroup1_Group, Radio1]}	
            //{[ScoreNum1, 500]}	
            //{[ScoreNum2, 100]}	
            //{[ScoreNum3, 100]}	
            //{[ScoreNum4, 300]}	
            //{[SalaryTxt, ]}	
            //{[ApplyTxt, 200007]}	
            //{[CreateTxt, ]}	
            //{[CreateDate, 2010-08-19]}	
            //{[StationTxt, ]}	
            //{[WorkTxt, ]}	
            //{[TitleTxt, ]}	
            //{[JobTxt, ]}	

            if (Convert.ToInt32(FormValues["SortNoNum"]) < 1)
            {
                FormValues["SortNoNum"] = "1";
            }

            string selectsql = string.Format(@"SELECT NVL(MAX(SEQUENCE),'1') AS SEQ FROM {0}.SYS_STATION_MAINTENANCE_DICT WHERE STATION_TYPE_CODE='{1}'", DataUser.COMM, FormValues["StationTypeCombo_Value"]);
            
            string dbSEQUENCE =OracleOledbBase.GetSingle(selectsql).ToString();
            if (Convert.ToInt32(FormValues["SortNoNum"]) < Convert.ToInt32(dbSEQUENCE))
            {
                selectsql = string.Format(@"SELECT COUNT(*) AS CNT FROM {0}.SYS_STATION_MAINTENANCE_DICT WHERE STATION_TYPE_CODE='{1}' and SEQUENCE = {2} ", DataUser.COMM, FormValues["StationTypeCombo_Value"], FormValues["SortNoNum"]);
                string cntstr =  OracleOledbBase.GetSingle(selectsql).ToString();
                if (!cntstr.Equals("0"))
                {
                    string update = string.Format(@"UPDATE {0}.SYS_STATION_MAINTENANCE_DICT set SEQUENCE = SEQUENCE+1 where STATION_TYPE_CODE='{1}' and SEQUENCE >= {2}", DataUser.COMM, FormValues["StationTypeCombo_Value"], FormValues["SortNoNum"]);
                    OracleOledbBase.ExecuteNonQuery(update);
                }
            }
            else
            {
                FormValues["SortNoNum"] = (Convert.ToInt32(dbSEQUENCE) + 1).ToString();
            }

            string sql = string.Format(@"insert into {0}.SYS_STATION_MAINTENANCE_DICT(SEQUENCE,Station_Name,Station_Code_Remark,Whether,Stipend,
Work_Content,Post_Competency,Work_Circumstance,Input_User,Input_Time,Auditing_User,Guide_Gather_Code,Station_Bsc_Class_01,Station_Bsc_Class_02,
Station_Bsc_Class_03,Station_Bsc_Class_04,id,STATION_TYPE_CODE,Duty_Order,GUIDE_TYPE
) values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)",DataUser.COMM);

            OleDbParameter[] prams = { 
                new OleDbParameter("SEQUENCE", FormValues["SortNoNum"]),
                new OleDbParameter("Station_Name",FormValues["StationNameTxt"]),
                new OleDbParameter("Station_Code_Remark",FormValues["StationTxt"]),
                new OleDbParameter("Whether",(FormValues["RadioGroup1_Group"].Equals("Radio1")?"是":"否")),
                new OleDbParameter("Stipend",FormValues["SalaryTxt"]),
                new OleDbParameter("Work_Content",FormValues["WorkTxt"]),
                new OleDbParameter("Post_Competency",FormValues["TitleTxt"]),
                new OleDbParameter("Work_Circumstance",FormValues["JobTxt"]),
                new OleDbParameter("Input_User",FormValues["CreateTxt"]),
                new OleDbParameter("Input_Time",FormValues["CreateDate"]),
                new OleDbParameter("Auditing_User",FormValues["ApplyTxt"]),
                new OleDbParameter("Guide_Gather_Code",FormValues["GuideGatherCombo_Value"]),
                new OleDbParameter("Station_Bsc_Class_01",FormValues["ScoreNum1"]),
                new OleDbParameter("Station_Bsc_Class_02",FormValues["ScoreNum2"]),
                new OleDbParameter("Station_Bsc_Class_03",FormValues["ScoreNum3"]),
                new OleDbParameter("Station_Bsc_Class_04",FormValues["ScoreNum4"]),

                new OleDbParameter("id", (stationcode.Equals("")? GetMaxID(FormValues["StationTypeCombo_Value"]).ToString():stationcode) ),
                new OleDbParameter("STATION_TYPE_CODE", FormValues["StationTypeCombo_Value"]),
                new OleDbParameter("Duty_Order",FormValues["DeptDutyCombo_Value"]),
                new OleDbParameter("GUIDE_TYPE",FormValues["GatherTypeCombo_Value"])
            };

            OracleOledbBase.ExecuteNonQuery(sql, prams);
        }

        /// <summary>
        /// 删除岗位字典信息
        /// </summary>
        /// <param name="ids"></param>
        public string  DeleteSatationDict(string ids)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS CNT  FROM {0}.NEW_STAFF_INFO WHERE STATION_CODE  ='{1}' AND ADD_MARK='1'", DataUser.RLZY, ids);
            string cnt = OracleOledbBase.GetSingle(sql).ToString();
            if (!cnt.Equals("0"))
            {
                return "该岗位下已经人员在岗信息,请先维护人力资源信息!";
            }
            sql = string.Format(@"SELECT SEQUENCE, STATION_TYPE_CODE FROM {0}.SYS_STATION_MAINTENANCE_DICT WHERE ID  ='{1}'", DataUser.COMM, ids);
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);
            string station_type = "";
            string sequence = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                station_type = ds.Tables[0].Rows[0]["STATION_TYPE_CODE"].ToString();
                sequence = ds.Tables[0].Rows[0]["SEQUENCE"].ToString();
            }

            sql = string.Format(@"DELETE FROM {0}.SYS_STATION_MAINTENANCE_DICT WHERE ID  ='{1}'", DataUser.COMM, ids);
            OracleOledbBase.ExecuteNonQuery(sql);

            sql = string.Format(@"UPDATE {0}.SYS_STATION_MAINTENANCE_DICT SET SEQUENCE = SEQUENCE -1 WHERE STATION_TYPE_CODE  ='{1}' AND SEQUENCE > {2}", DataUser.COMM, station_type, sequence);
            OracleOledbBase.ExecuteNonQuery(sql);
            return "";
        }


        /// <summary>
        /// 更新岗位字典信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id">岗位ID号</param>
        public void UpdateSatationDict(Dictionary<string, string> FormValues, string id)
        {
            string sql = string.Format(@"DELETE FROM {0}.SYS_STATION_MAINTENANCE_DICT WHERE ID  ='{1}'", DataUser.COMM, id);
            OracleOledbBase.ExecuteNonQuery(sql);
            InsertSatationDict(FormValues, id);
        }



        /// <summary>
        /// 岗位字典ID自动顺序号
        /// </summary>
        private string GetMaxID(string dept_type)
        {
            string res = string.Empty;

            string strsql = string.Format(@"
  SELECT DECODE (MAX (TO_NUMBER (id)) + 1,  NULL, '0',  MAX (TO_NUMBER (id)) + 1  ) AS new_station_code
    FROM {1}.SYS_STATION_MAINTENANCE_DICT
   WHERE id LIKE '{0}_____'
ORDER BY id", dept_type,DataUser.COMM);

            DataSet ds = OracleOledbBase.ExecuteDataSet( strsql);

            if (ds.Tables[0].Rows[0][0].ToString() == "0")
            {
                res = dept_type + "00001";
            }
            else
            {
                res = ds.Tables[0].Rows[0][0].ToString().PadLeft(6, '0');
            }

            return res;
        }

		#endregion  成员方法
	}
}

