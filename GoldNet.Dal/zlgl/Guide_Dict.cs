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
    public class ZLGL_Guide_Dict
    {
        public ZLGL_Guide_Dict()
        { }
        /// <summary>
        /// 获取所有指标
        /// </summary>
        /// <returns>DataSet</returns>
        public  DataSet GetAllGuide()
        {
            string SQL_GetAllGuide = string.Format("SELECT G_CommonGuide.ID as ID,CommGuideName as GuideName,CommGuideTypeID as GuideTypeID,GuideType,TypeSign,GuideNum,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue,Arithmetic FROM {0}.G_CommonGuide , {0}.G_GuideType where G_GuideType.ID=G_CommonGuide.CommGuideTypeID and G_CommonGuide.TEMPLETID not in (select id from zlgl.T_TEMPLETDICT where deleted=1) UNION SELECT G_SpecialtyGuide.ID as ID,SpecGuideName as GuideName,SpecGuideTypeID as GuideTypeID,GuideType,TypeSign,GuideNum,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue,Arithmetic FROM {0}.G_SpecialtyGuide , {0}.G_GuideType where G_GuideType.ID=G_SpecialtyGuide.SpecGuideTypeID and G_SpecialtyGuide.TEMPLETID not in (select id from zlgl.T_TEMPLETDICT where deleted=1) order by GuideTypeID", DataUser.ZLGL);
            return OracleOledbBase.ExecuteDataSet(SQL_GetAllGuide);
        }
        /// <summary>
        /// 获取所有时间说明
        /// </summary>
        /// <returns></returns>
        public  DataSet GetAllDateDesc()
        {
            string SQL_GetAllDateDesc = string.Format("SELECT ID,DateDesc,StartDate,EndDate,CheckNum FROM {0}.G_CollectScorePrimary order by StartDate DESC", DataUser.ZLGL);
            return OracleOledbBase.ExecuteDataSet(SQL_GetAllDateDesc);
        }
        /// <summary>
        /// 获取某一指标名称的考评内容
        /// </summary>
        public  DataSet GetGuideContent(int guideNameID, int guideTypeID)
        {
            string SQL_GetGuideCont = String.Format("SELECT ID,CheckCont,CheckStan,CheckMeth FROM G_GuideCheckContent WHERE GuideNameID = {0} AND GuideTypeID = {1}", guideNameID, guideTypeID);

            System.Data.OracleClient.OracleParameter[] cmdPara = new System.Data.OracleClient.OracleParameter[] { };
            DataSet ds = OracleBase.Query(SQL_GetGuideCont);

            return ds;
        }

        //
        /// <summary>
        /// 科室考评指标
        /// </summary>
        public  DataSet GetGuideContentdept(int guideNameID, int guideTypeID, string deptcode)
        {
            OleDbParameter[] cmdPara = new OleDbParameter[]{
															   new OleDbParameter("GuideNameID",guideNameID),
															   new OleDbParameter("GuideTypeID",guideTypeID),
				                                                new OleDbParameter("deptcode",deptcode)
														   };
            string SQL_GetGuideContdept = string.Format("SELECT ID,CheckCont,CheckStan,CheckMeth FROM {0}.G_GuideCheckContent WHERE GuideNameID = ? AND GuideTypeID = ? and deptcode=?", DataUser.ZLGL);
            DataSet ds = OracleOledbBase.ExecuteDataSet(SQL_GetGuideContdept, cmdPara);
            return ds;
        }
        /// <summary>
        /// 指标类别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  DataSet Guide_Type_Dict_by_id(int id)
        {
            string GuideTypeDict = string.Format("select ID,GuideType,TypeSign,GuideTypeNum from {0}.G_GuideType where id=" + id, DataUser.ZLGL);

            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideTypeDict);
            return ds;

        }
        /// <summary>
        /// 查询指标
        /// </summary>
        /// <param name="guidename"></param>
        /// <returns></returns>
        public DataSet getGuideByName(string guidename)
        {
            string SQL_getGuideByName = string.Format("SELECT G_CommonGuide.ID as ID,CommGuideName as GuideName,CommGuideTypeID as GuideTypeID,GuideType,TypeSign,GuideNum,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue FROM {0}.G_CommonGuide , {0}.G_GuideType where G_GuideType.ID=G_CommonGuide.CommGuideTypeID and to_char(CommGuideName) = ? and G_CommonGuide.TEMPLETID not in (select id from zlgl.T_TEMPLETDICT where deleted=1) UNION SELECT G_SpecialtyGuide.ID as ID,SpecGuideName as GuideName,SpecGuideTypeID as GuideTypeID,GuideType,TypeSign,GuideNum,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue FROM {0}.G_SpecialtyGuide , {0}.G_GuideType where G_GuideType.ID=G_SpecialtyGuide.SpecGuideTypeID and G_SpecialtyGuide.TEMPLETID not in (select id from zlgl.T_TEMPLETDICT where deleted=1) and to_char(SpecGuideName) = ?", DataUser.ZLGL);
            OleDbParameter[] cmdPara = new OleDbParameter[]{
                                                           new OleDbParameter("GuideName",guidename),
                                                           new OleDbParameter("CommGuideName",guidename)
                                                       };
            return  OracleOledbBase.ExecuteDataSet(SQL_getGuideByName, cmdPara);
        }
        /// <summary>
        /// 删除某一质量考评汇总数据
        /// </summary>
        public  bool DelCheckCollect(string DateDesc, OleDbTransaction trans)
        {
            string str = string.Format("delete from {0}.G_CollectScoreSecondary  where PRIMARYID in (select ID from {0}.G_CollectScorePrimary where DateDesc='" + DateDesc + "')", DataUser.ZLGL);
            OracleOledbBase.ExecuteNonQuery(trans, CommandType.Text, str, new OleDbParameter[] { });
            OleDbParameter[] cmdPara = new OleDbParameter[]{
                                                           new OleDbParameter("DateDesc",DateDesc)
                                                       };
            string SQL_DelCheckCollect = string.Format("delete FROM {0}.G_CollectScorePrimary WHERE DateDesc = ?", DataUser.ZLGL);
            if (OracleOledbBase.ExecuteNonQuery(trans, CommandType.Text, SQL_DelCheckCollect, cmdPara) == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 添加主表数据
        /// </summary>
        /// <returns>生成的ID<returns>
        public  object AddPrimary(string DateDesc, DateTime StartDate, DateTime EndDate, int CheckNum, OleDbTransaction trans)
        {
            string SQL_AddPrimary = string.Format("insert into {0}.G_CollectScorePrimary(id,DateDesc,StartDate,EndDate,CheckNum,INPUTDATE) values(?,?,?,?,?,?)", DataUser.ZLGL);
            int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.G_CollectScorePrimary", DataUser.ZLGL));
            OleDbParameter[] cmdPara = new OleDbParameter[]{
                                                               new OleDbParameter("ID",id),
                                                           new OleDbParameter("DateDesc",DateDesc),
                                                           new OleDbParameter("StartDate",StartDate),
                                                           new OleDbParameter("EndDate",EndDate),
                                                           new OleDbParameter("CheckNum",CheckNum),
                                                               new OleDbParameter("CheckNum",System.DateTime.Now)
                                                       };
            OracleOledbBase.ExecuteScalar(trans, CommandType.Text, SQL_AddPrimary, cmdPara);
            return id;
        }
        /// <summary>
        /// 增加指标考评汇总数据INPUTDATE
        /// </summary>
        /// <param name="DeptName">科室</param>
        /// <param name="GuideType">指标类别</param>
        /// <param name="GuideName">指标名称</param>
        /// <param name="GuideNum">指标分值</param>
        /// <param name="GuideCutNum">实际扣分</param>
        /// <param name="GuideSpareNum">最后得分</param>
        /// <param name="PrimaryID">主表ID</param>
        /// <returns></returns>
        public  bool addDeptScore(int id, string DeptName, string GuideType, string GuideName, double GuideNum, double GuideCutNum, double GuideSpareNum, int PrimaryID, string TYPESIGN, OleDbTransaction trans)
        {
            string SQL_AddDeptScore = string.Format("insert into {0}.G_CollectScoreSecondary(id,DeptName,GuideType,GuideName,GuideNum,GuideCutNum,GuideSpareNum,PrimaryID,INPUTDATE,TYPESIGN) values(?,?,?,?,?,?,?,?,?,?)", DataUser.ZLGL);

            OleDbParameter[] cmdPara = new OleDbParameter[]{
                                                            new OleDbParameter("id",id),
                                                           new OleDbParameter("DeptName",DeptName),
                                                           new OleDbParameter("GuideType",GuideType),
                                                           new OleDbParameter("GuideName",GuideName),
                                                           new OleDbParameter("GuideNum",GuideNum),
                                                           new OleDbParameter("GuideCutNum",GuideCutNum),
                                                           new OleDbParameter("GuideSpareNum",GuideSpareNum),
                                                           new OleDbParameter("PrimaryID",PrimaryID),
                                                           new OleDbParameter("INPUTDATE",System.DateTime.Now),
                                                           new OleDbParameter("TYPESIGN",TYPESIGN)
                                                       };
            int intAddDeptScore = OracleOledbBase.ExecuteNonQuery(trans, CommandType.Text, SQL_AddDeptScore, cmdPara);
            if (intAddDeptScore == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 取得所有指标类别信息
        /// </summary>
        /// <returns>DataSet</returns>
        public  DataSet Guide_Type_Dict()
        {
            string GuideTypeDict = string.Format("select ID,GuideType,TypeSign,GuideTypeNum from {0}.G_GuideType where TypeSign<>'9' order by ID ASC", DataUser.ZLGL);
            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideTypeDict);
            return ds;
        }
        /// <summary>
        /// 某一段时间每个科室每个指标得分列表
        /// </summary>
        /// <param name="StartDateDesc">开始时间说明</param>
        /// <param name="EndDateDesc">结束时间说明</param>
        /// <returns></returns>
        public  DataTable GuideScoreTable(int StartDateDesc, int EndDateDesc)
        {
            string SQL_GuideScoreTable = string.Format("SELECT DateDesc,DeptName,GuideType,GuideName,GuideSpareNum,GUIDECUTNUM FROM {0}.G_CollectScorePrimary QSP , {0}.G_CollectScoreSecondary QSS where QSS.PrimaryID = QSP.ID and (QSP.ID between ? AND ?) order by qsp.STARTDATE", DataUser.ZLGL);
            int a;
            int b;
            if (StartDateDesc > EndDateDesc)
            {
                a = EndDateDesc;
                b = StartDateDesc;
            }
            else
            {
                a = StartDateDesc;
                b = EndDateDesc;
            }
            OleDbParameter[] cmdPara = new OleDbParameter[]{
                                                               new OleDbParameter("StartDateDesc",a),
                                                               new OleDbParameter("EndDateDesc",b)
                                                           };
            DataTable dt = OracleOledbBase.ExecuteDataSet(SQL_GuideScoreTable, cmdPara).Tables[0];
            return dt;
        }
        /// <summary>
        /// 获取某一段时间每个科室每个指标得分列表  
        /// </summary>
        /// <param name="datestr"></param>
        /// <returns></returns>
        public  DataTable GuideScoreTable(string datestr)
        {
            string SQL_GuideScoreTable = string.Format("SELECT DateDesc,DeptName,GuideType,GuideName,GuideSpareNum,GUIDECUTNUM FROM {0}.G_CollectScorePrimary QSP , {0}.G_CollectScoreSecondary QSS where QSS.PrimaryID = QSP.ID and QSP.datedesc = ?  order by qsp.STARTDATE", DataUser.ZLGL);

            OleDbParameter[] cmdPara = new OleDbParameter[]{
                                                               new OleDbParameter("datedesc",datestr)

                                                           };
            DataTable dt = OracleOledbBase.ExecuteDataSet(SQL_GuideScoreTable, cmdPara).Tables[0];
            return dt;
        }
        /// <summary>
        /// 指标类型
        /// </summary>
        /// <returns></returns>
        public  DataSet Guide_Type_Dict_dept()
        {
            string GuideTypeDict = string.Format("select ID,GuideType,TypeSign,GuideTypeNum from {0}.G_GuideType where TypeSign<>'9' order by ID ASC", DataUser.ZLGL);
            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideTypeDict);
            return ds;
        }
        /// <summary>
        /// 获取时间说明ID
        /// </summary>
        /// <param name="DateDesc">时间说明</param>
        /// <returns></returns>
        public  int GetDateDescID(string DateDesc)
        {
            string sql = string.Format("select * From {0}.G_CollectScorePrimary where DateDesc=?", DataUser.ZLGL);
            OleDbParameter[] cmdPara = new OleDbParameter[]{
                                                           new OleDbParameter("DateDesc",DateDesc)
                                                       };
            int DateDescID = Convert.ToInt32(OracleOledbBase.ExecuteDataSet(sql, cmdPara).Tables[0].Rows[0]["ID"].ToString());
            return DateDescID;
        }
        /// <summary>
        /// 判断某一质量考评汇总是否存在
        /// </summary>
        /// <param name="DateDesc">时间说明</param>
        /// <returns>存在返回True,不存在返回false</returns>
        public  bool IsExistCheckCollect(string DateDesc)
        {
            string SQL_IsExistCheckCollect = string.Format("SELECT ID,DateDesc,StartDate,EndDate,CheckNum,InputDate FROM {0}.G_CollectScorePrimary WHERE DateDesc = ?", DataUser.ZLGL);
            OleDbParameter[] cmdPara = new OleDbParameter[]{
                                                           new OleDbParameter("DateDesc",DateDesc)
                                                       };
            if (OracleOledbBase.ExecuteDataSet(SQL_IsExistCheckCollect, cmdPara).Tables[0].Rows.Count != 0)
                return true;
            else
                return false;
           
        }
        /// <summary>
        /// 周判断
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public bool IsExistCheckCollectWeek(string Date)
        {
            if (GetConfig.GetConfigString("WEEK").ToUpper() == "NO")
            {
                return false;
            }
            else
            {
                int week = ConvertDate.GetWeekIndex(Date);
                string SQL_IsExistCheckCollect = string.Format("select * from {0}.QUALITY_SUBMIT WHERE Week = {1}", DataUser.ZLGL, week);

                if (OracleOledbBase.ExecuteDataSet(SQL_IsExistCheckCollect).Tables[0].Rows.Count != 0)
                    return true;
                else
                    return false;
            }

        }
    }
}
