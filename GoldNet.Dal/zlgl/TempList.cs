using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;

namespace Goldnet.Dal
{
    public class TempList
    {
        public TempList()
        { }
        /// <summary>
        /// 专业质量科室列表
        /// </summary>
        /// <param name="centerid"></param>
        /// <returns></returns>
        public string GetSpecDeptList(string deptstr, int tempid)
        {
            if (GetConfig.GetConfigString("CHECKDEPT") == "0")
                return "";
            else
            {
                string deptfilter = string.Format("{0} in (", deptstr);
                string strSql = string.Format("select CHECKDEPT from {0}.G_SPECIALTYGUIDE a where a.TEMPLETID={1}", DataUser.ZLGL, tempid);
                string ob = OracleOledbBase.ExecuteScalar(strSql).ToString();
                if (ob.Equals(string.Empty))
                    return "";
                else
                {
                    string[] deptlist = ob.Split(',');

                    for (int i = 0; i < deptlist.Length; i++)
                    {
                        if (!deptlist[i].ToString().Equals(string.Empty))
                            deptfilter = deptfilter + "'" + deptlist[i].ToString() + "',";
                    }
                    deptfilter = deptfilter.Substring(0, deptfilter.Length - 1);
                    deptfilter += ")";
                }
                return deptfilter;
            }

        }
        /// <summary>
        /// 考评内容
        /// </summary>
        /// <param name="conttype"></param>
        /// <returns></returns>
        public DataSet GetGuideContent(string conttype)
        {
            string SQL_GetGuideCont = String.Format("SELECT ID,CheckCont FROM {0}.G_GuideCheckContent WHERE CONTENTYPE = {1} order by id", DataUser.ZLGL, conttype);
            DataSet ds = OracleOledbBase.ExecuteDataSet(SQL_GetGuideCont);
            return ds;
        }
        /// <summary>
        /// 考评标准
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetGuideContentdetail(int id)
        {
            string SQL_GetGuideCont = String.Format("SELECT checkstan FROM {0}.G_GuideCheckContent WHERE id = {1}", DataUser.ZLGL, id);
            return OracleOledbBase.GetSingle(SQL_GetGuideCont).ToString();
        }
        /// <summary>
        /// 扣分列表
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="tableid"></param>
        /// <returns></returns>
        public DataTable GetQulityCheck(string tablename, string tableid)
        {
            string qulity = string.Format("select IS_CHECKLOOK,FLAGS from {0}.QUALITY_ERROR_LIST where TABLE_NAME='{1}' and TABLE_ID='{2}'", DataUser.ZLGL, tablename, tableid);
            return OracleOledbBase.ExecuteDataSet(qulity).Tables[0];
        }
        public string GetCheckPass(string tempid, string check, string flags)
        {
            string strid = "'0',";
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select TABLE_ID from {0}.QUALITY_ERROR_LIST where TEMPLET_ID='{1}'", DataUser.ZLGL, tempid);
            if (check != "0")
            {
                str.AppendFormat(" and IS_CHECKLOOK='{0}'", check);
            }
            if (flags != "0")
            {
                str.AppendFormat(" and FLAGS='{0}'", flags);
            }
            DataTable tb = OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                strid += "'" + tb.Rows[i][0].ToString() + "',";
            }
            strid = strid.Substring(0, strid.Length - 1);
            return strid;
        }
        /// <summary>
        /// 获取某一科室的指标集合
        /// </summary>
        /// <param name="GroupCode">科室代号</param>
        /// <returns>DataSet</returns>
        public DataSet GetGuidesByGroup(string GroupCode)
        {
            string SQL_GetGuidesByGroup = string.Format("SELECT G_CommonGuide.ID as ID,CommGuideName as GuideName,CommGuideTypeID as GuideTypeID,GuideType,TypeSign,GuideNum,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue,Arithmetic FROM {0}.G_CommonGuide , {0}.G_GuideType,{0}.T_TEMPLETDICT where T_TEMPLETDICT.mark='0' and G_GuideType.ID=G_CommonGuide.CommGuideTypeID and T_TEMPLETDICT.ID=g_commonguide.TEMPLETID and T_TEMPLETDICT.FLAG='0' and G_CommonGuide.TEMPLETID not in (select id from zlgl.T_TEMPLETDICT where deleted=1) UNION SELECT G_SpecialtyGuide.ID as ID,SpecGuideName as GuideName,SpecGuideTypeID as GuideTypeID,GuideType,TypeSign,GuideNum,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue,Arithmetic FROM {0}.G_SpecialtyGuide , {0}.G_GuideType where G_GuideType.ID=G_SpecialtyGuide.SpecGuideTypeID and G_SpecialtyGuide.ID = ? and G_SpecialtyGuide.TEMPLETID not in (select id from zlgl.T_TEMPLETDICT where deleted=1 or mark='1') order by GuideTypeID", DataUser.ZLGL);

            int SpecGuideID = GetSpecGuideIDByGroup(GroupCode);        //获取该科室所在的专业指标ID号
            OleDbParameter[] cmdPara = new OleDbParameter[]{
														   new OleDbParameter("SpecGuideID",SpecGuideID)
													   };

            DataSet ds = OracleOledbBase.ExecuteDataSet(SQL_GetGuidesByGroup, cmdPara);     //取所有公共指标和对应的专业指标
            return ds;
        }
        public DataSet GetGuidesByGroupdept(string GroupCode)
        {
            string SpecGuideID = GetSpecGuideIDBydept(GroupCode);        //获取该科室所在的专业指标ID号
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT G_CommonGuide.ID as ID,CommGuideName as GuideName,CommGuideTypeID as GuideTypeID,GuideType,TypeSign,GuideNum,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue,Arithmetic FROM {0}.G_CommonGuide , {0}.G_GuideType,{0}.T_TEMPLETDICT where T_TEMPLETDICT.mark='0' and G_GuideType.ID=G_CommonGuide.CommGuideTypeID and T_TEMPLETDICT.ID=g_commonguide.TEMPLETID and T_TEMPLETDICT.FLAG='0' and g_guidetype.TYPESIGN<>'9' and G_CommonGuide.TEMPLETID not in (select id from zlgl.T_TEMPLETDICT where deleted=1) UNION SELECT G_SpecialtyGuide.ID as ID,SpecGuideName as GuideName,SpecGuideTypeID as GuideTypeID,GuideType,TypeSign,GuideNum,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue,Arithmetic FROM {0}.G_SpecialtyGuide , {0}.G_GuideType where G_GuideType.ID=G_SpecialtyGuide.SpecGuideTypeID and G_SpecialtyGuide.TEMPLETID not in (select id from zlgl.T_TEMPLETDICT where deleted=1 or mark='1') ", DataUser.ZLGL);
            if (!SpecGuideID.Trim().Equals(string.Empty))
            {
                str.Append(" and G_SpecialtyGuide.ID in " + SpecGuideID);
            }
            str.Append(" order by GuideTypeID");
            DataSet ds = new DataSet();
            try
            {
                ds = OracleOledbBase.ExecuteDataSet(str.ToString());     //取所有公共指标和对应的专业指标
            }
            catch (Exception ex)
            {
                string str1 = ex.ToString();
            }
            return ds;
        }

        public DataSet GetGuidesByGroupdepttest(string GroupCode)
        {
            string SQL_GetGuidesByGroupdept = string.Format("SELECT G_CommonGuide.ID as ID,CommGuideName as GuideName,CommGuideTypeID as GuideTypeID,GuideType,TypeSign,GuideNum,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue,Arithmetic FROM {0}.G_CommonGuide , {0}.G_GuideType,{0}.T_TEMPLETDICT where T_TEMPLETDICT.mark='0' and  G_GuideType.ID=G_CommonGuide.CommGuideTypeID and T_TEMPLETDICT.ID=g_commonguide.TEMPLETID and T_TEMPLETDICT.FLAG='0' and g_guidetype.TYPESIGN<>'9' and G_CommonGuide.TEMPLETID not in (select id from zlgl.T_TEMPLETDICT where deleted=1) UNION SELECT G_SpecialtyGuide.ID as ID,SpecGuideName as GuideName,SpecGuideTypeID as GuideTypeID,GuideType,TypeSign,GuideNum,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue,Arithmetic FROM {0}.G_SpecialtyGuide , {0}.G_GuideType where G_GuideType.ID=G_SpecialtyGuide.SpecGuideTypeID and G_SpecialtyGuide.ID in ? and G_SpecialtyGuide.TEMPLETID not in (select id from zlgl.T_TEMPLETDICT where deleted=1 or mark='1') order by GuideTypeID", DataUser.ZLGL);
            int SpecGuideID = GetSpecGuideIDByGroup(GroupCode);        //获取该科室所在的专业指标ID号
            OleDbParameter[] cmdPara = new OleDbParameter[]{
                                                               new OleDbParameter("SpecGuideID",SpecGuideID)
                                                           };

            DataSet ds = OracleOledbBase.ExecuteDataSet(SQL_GetGuidesByGroupdept, cmdPara);     //取所有公共指标和对应的专业指标
            return ds;
        }
        /// <summary>
        /// 获取科室专业指标
        /// </summary>
        /// <param name="GroupCode"></param>
        /// <returns></returns>
        public string GetSpecGuideIDBydept(string GroupCode)
        {
            SYS_DEPT_DICT dal = new SYS_DEPT_DICT();
            DataTable dept = dal.GetDeptbydept(GroupCode);
            StringBuilder SpecGuideID = new StringBuilder();

            DataSet ds = SpecialtyGuide_View();       //取专业指标
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)             //循环专业指标
            {
                string[] arrayCheckDept = BreakString.ShowText(ds.Tables[0].Rows[i]["CheckDept"].ToString());   //存放考核科室到数组中      
                for (int j = 0; j < arrayCheckDept.Length; j++)           //循环数组    
                {
                    for (int a = 0; a < dept.Rows.Count; a++)
                    {
                        if (arrayCheckDept[j] == dept.Rows[a]["dept_code"].ToString())
                        {
                            if (SpecGuideID.ToString() == "")
                            {
                                SpecGuideID.Append("(" + ds.Tables[0].Rows[i]["ID"].ToString());

                            }
                            else
                            {
                                SpecGuideID.Append("," + ds.Tables[0].Rows[i]["ID"].ToString());  //取出考核科室所在专业指标行的ID号  
                            }
                        }
                    }

                }
                //取到专业指标ID号，跳出外循环
            }
            if (SpecGuideID.ToString().Equals(string.Empty))
            {
                return "";
            }
            else
            {
                SpecGuideID.Append(")");
                return SpecGuideID.ToString();
            }
        }
        /// <summary>
        /// 浏览专业部分指标名称
        /// </summary>
        /// <returns>获取专业部分指标名称信息</returns>
        public DataSet SpecialtyGuide_View()
        {
            string SpecGuideName = string.Format("select a.ID,a.SpecGuideName,a.SpecGuideTypeID,a.ManaDept,a.GuideNum,b.GuideType,b.TypeSign,a.CheckDept from {0}.G_SpecialtyGuide a, {0}.G_GuideType b,{0}.T_TEMPLETDICT c where b.ID = a.SpecGuideTypeID and A.TEMPLETID=C.ID and C.DELETED=0 and C.MARK=0 order by a.ID ASC", DataUser.ZLGL);

            DataSet ds = OracleOledbBase.ExecuteDataSet(SpecGuideName);
            return ds;

        }
        public DataSet SpecialtyGuide_Viewd()
        {
            string SpecGuideName = string.Format("select a.ID,a.SpecGuideName,a.SpecGuideTypeID,a.ManaDept,a.GuideNum,b.GuideType,b.TypeSign,a.CheckDept from {0}.G_SpecialtyGuide a, {0}.G_GuideType b,{0}.T_TEMPLETDICT c where b.ID = a.SpecGuideTypeID and A.TEMPLETID=C.ID and C.DELETED=0 order by a.ID ASC", DataUser.ZLGL);

            DataSet ds = OracleOledbBase.ExecuteDataSet(SpecGuideName);
            return ds;

        }
        /// <summary>
        /// 浏览公共部分指标名称
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet CommonGuide_View()
        {
            string CommGuideName = string.Format("select a.ID,a.CommGuideName,a.CommGuideTypeID,a.ManaDept,a.GuideNum,b.GuideType,b.TypeSign from {0}.G_CommonGuide a , {0}.G_GuideType b WHERE b.ID = a.CommGuideTypeID order by a.ID ASC", DataUser.ZLGL);

            DataSet ds = OracleOledbBase.ExecuteDataSet(CommGuideName);
            return ds;
        }
        /// <summary>
        /// 显示要修改的指标类别信息
        /// </summary>
        /// <param name="ID">指标类别ID</param>
        /// <returns>符合条件的指标类别信息</returns>
        public DataSet GuideType_Edit_View(int ID)
        {
            string GuideTypeEditView = string.Format("select GuideType,TypeSign,GuideTypeNum from {0}.G_GuideType where ID=?", DataUser.ZLGL);
            OleDbParameter[] CmdParms = new OleDbParameter[]{
                                                            new OleDbParameter("ID",ID)
                                                        };

            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideTypeEditView, CmdParms);
            return ds;
        }
        /// <summary>
        /// 公共部分指标名称添加
        /// </summary>
        /// <returns>是否成功标志</returns>
        public bool CommonGuide_Add(string CommGuideName, int CommGuideTypeID, string ManaDept, double GuideNum, int TempletID, int DateCol, int TargetCol, int GuideNameCol, int GuideNameColValue, int Arithmetic)
        {
            try
            {
                string CommGuideNameAdd = string.Format("insert into {0}.G_CommonGuide (ID,CommGuideName,CommGuideTypeID,ManaDept,GuideNum,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue,Arithmetic) values (?,?,?,?,?,?,?,?,?,?,?)", DataUser.ZLGL);
                int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.G_CommonGuide", DataUser.ZLGL));
                OleDbParameter[] CmdParms = new OleDbParameter[]{
                                                                new OleDbParameter("ID",id),
                                                                new OleDbParameter("CommGuideName",CommGuideName),
                                                                new OleDbParameter("CommGuideTypeID",CommGuideTypeID),
                                                                new OleDbParameter("ManaDept",ManaDept),
                                                                new OleDbParameter("GuideNum",GuideNum),
                                                                new OleDbParameter("TempletID",TempletID),
                                                                new OleDbParameter("DateCol",DateCol),
                                                                new OleDbParameter("TargetCol",TargetCol),
                                                                new OleDbParameter("GuideNameCol",GuideNameCol),
                                                                new OleDbParameter("GuideNameColValue",GuideNameColValue),
                                                                new OleDbParameter("Arithmetic",Arithmetic)
                                                            };

                int Add = OracleOledbBase.ExecuteNonQuery(CommGuideNameAdd, CmdParms);
                if (Add == 1)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw new SaveRecordDataException("保存公共指标时发生错误，", CommGuideName + "已经存在，不能重复添加");
            }
        }
        /// <summary>
        /// 公共部分指标名称修改
        /// </summary>
        /// <returns>是否成功修改标志</returns>
        public bool CommonGuide_Edit(string CommGuideName, int CommGuideTypeID, string ManaDept, double GuideNum, int TempletID, int DateCol, int TargetCol, int GuideNameCol, int GuideNameColValue, int Arithmetic, int ID)
        {
            try
            {
                string CommGuideNameEdit = string.Format("update {0}.G_CommonGuide set CommGuideName=?,CommGuideTypeID=?,ManaDept=?,GuideNum=?,TempletID=?,DateCol=?,TargetCol=?,GuideNameCol=?,GuideNameColValue=?,Arithmetic=? where ID=?", DataUser.ZLGL);
                OleDbParameter[] CmdParms = new OleDbParameter[]{
																new OleDbParameter("CommGuideName",CommGuideName),
																new OleDbParameter("CommGuideTypeID",CommGuideTypeID),
																new OleDbParameter("ManaDept",ManaDept),
																new OleDbParameter("GuideNum",GuideNum),
																new OleDbParameter("TempletID",TempletID),
																new OleDbParameter("DateCol",DateCol),
																new OleDbParameter("TargetCol",TargetCol),                          
																new OleDbParameter("GuideNameCol",GuideNameCol),
																new OleDbParameter("GuideNameColValue",GuideNameColValue),
																new OleDbParameter("Arithmetic",Arithmetic),
																new OleDbParameter("ID",ID)
															};

                int Edit = OracleOledbBase.ExecuteNonQuery(CommGuideNameEdit, CmdParms);
                if (Edit == 1)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw new SaveRecordDataException("保存公共指标时发生错误：", CommGuideName + "已经存在，不能重复添加");
            }
        }
        /// <summary>
        /// 显示要修改的公共部分指标名称信息
        /// </summary>
        /// <param name="ID">公共指标名称ID</param>
        /// <returns>符合条件的公共部分指标名称信息</returns>
        public DataSet CommonGuide_Edit_View(int ID)
        {

            string CommGuideNameView = string.Format("select G_CommonGuide.ID,CommGuideName,CommGuideTypeID,ManaDept,GuideNum,GuideType,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue,Arithmetic from {0}.G_CommonGuide , {0}.G_GuideType where G_GuideType.ID=G_CommonGuide.CommGuideTypeID and G_CommonGuide.ID=?", DataUser.ZLGL);
            OleDbParameter[] CmdParms = new OleDbParameter[]{
                                                            new OleDbParameter("ID",ID)
                                                        };

            DataSet ds = OracleOledbBase.ExecuteDataSet(CommGuideNameView, CmdParms);
            return ds;

        }
        /// <summary>
        /// 专业指标考核科室数组
        /// </summary>
        /// <returns>返回专业指标考核科室数组</returns>
        public string[] GetAllSpecialtyGuideDept()
        {
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            DataSet ds = dal.SpecialtyGuide_View();      //获取所有专业指标
            string SpecGuideDept = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                SpecGuideDept = SpecGuideDept + ds.Tables[0].Rows[i]["CheckDept"].ToString();
            }

            return BreakString.ShowText(SpecGuideDept);
        }

        /// <summary>
        /// 获取不存在专业指标的考核科室
        /// <summary>
        /// <returns>不存在专业指标的考核科室字符串</returns>
        public string GetDeptNotSpecialtyGuide()
        {
            SYS_DEPT_DICT dal = new SYS_DEPT_DICT();
            string SpecGuide = "";                      //记录不存在专业指标的考核科室，初始化为空 
            //获取所有考核科室
            DataSet ds = dal.GetAAccountDept("");
            string[] SpecGuideDept = GetAllSpecialtyGuideDept();    //获取专业指标考核科室数组
            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            {
                string strgroup_code = ds.Tables[0].Rows[k]["dept_code"].ToString();        //临时记录各个科室编码
                int SpecGuideNum = 0;
                for (int p = 0; p < SpecGuideDept.Length; p++)
                {
                    if (strgroup_code == SpecGuideDept[p])
                    {
                        SpecGuideNum = SpecGuideNum + 1;
                    }
                }
                if (SpecGuideNum == 0)
                {
                    SpecGuide = SpecGuide + ds.Tables[0].Rows[k]["dept_name"].ToString() + ','.ToString();
                }
                strgroup_code = "";          //清空临时科室编码
            }

            return SpecGuide;
        }
        /// <summary>
        /// 获取时间说明
        /// </summary>
        /// <param name="a">开始时间ID</param>
        /// <param name="b">结束时间ID</param>
        /// <returns>保存时间说明的数据表</returns>
        public DataTable GetDateDesc(int a, int b)
        {
            string SQL_GetDateDesc = string.Format("SELECT ID,DateDesc FROM {0}.G_CollectScorePrimary  WHERE (ID BETWEEN ? AND ?) order by startdate ASC", DataUser.ZLGL);
            OleDbParameter[] cmdPara = new OleDbParameter[]{
                                                         new OleDbParameter("a",a),
                                                         new OleDbParameter("b",b)
                                                     };
            DataTable dt = OracleOledbBase.ExecuteDataSet(SQL_GetDateDesc, cmdPara).Tables[0];
            return dt;
        }
        /// <summary>
        /// 考评分类
        /// </summary>
        /// <param name="contentname"></param>
        /// <returns></returns>
        public DataSet Conten_Type_all(string contentname)
        {
            string GuideTypeDict = string.Format("select a.ID,a.CONTENT_TYPE from {0}.G_CONTENT_TYPE a,{0}.G_SPECIALTYGUIDE b where a.SPECIAL_TYPE=b.ID AND b.SPECGUIDENAME='" + contentname + "' ORDER BY a.ID", DataUser.ZLGL);

            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideTypeDict);
            return ds;
        }

        /// <summary>
        /// 通过某一指标类别ID获取该指标类别信息
        /// </summary>
        /// <param name="ID">指标类别ID</param>
        /// <returns>DataTable</returns>
        public DataTable GetGuideTypeByID(int ID)
        {
            string SQL_GetGuideTypeByID = string.Format("SELECT ID,GuideType,TypeSign,GuideTypeNum FROM {0}.G_GuideType where ID = ?", DataUser.ZLGL);
            OleDbParameter[] CmdParms = new OleDbParameter[]{
															new OleDbParameter("ID",ID)
														};
            DataTable dt = OracleOledbBase.ExecuteDataSet(SQL_GetGuideTypeByID, CmdParms).Tables[0];
            return dt;
        }
        /// <summary>
        /// 通过类别标志获取指标类别信息
        /// </summary>
        /// <param name="TypeSign">类别标志，0代表公共指标，1代表专业指标</param>
        /// <returns>DataSet</returns>
        public DataSet GetGuideTypeBySign(string TypeSign)
        {
            string GuideType = "";
            if (TypeSign == "0")
                GuideType = string.Format("select ID,GuideType,TypeSign,GuideTypeNum from {0}.G_GuideType where TypeSign <> '1' order by ID ASC", DataUser.ZLGL);

            else
                GuideType = string.Format("select ID,GuideType,TypeSign,GuideTypeNum from {0}.G_GuideType where TypeSign = '" + TypeSign + "' order by ID ASC", DataUser.ZLGL);
            OleDbParameter[] CmdParms = new OleDbParameter[]{
															new OleDbParameter("TypeSign",TypeSign)
														};

            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideType);
            return ds;
        }

        /// <summary>
        /// 通过类别标志、类别ID获取该类别的所有指标名称信息
        /// </summary>
        /// <param name="TypeSign">指标类别标志</param>
        /// <param name="ID">指标类别ID</param>
        /// <returns>DataSet</returns>
        public DataSet Guide_Name_Dict(string TypeSign, int ID)
        {
            string CommGuideNameDict = string.Format("select G_CommonGuide.ID as GuideNameID,CommGuideName as GuideName,CommGuideTypeID as GuideTypeID,ManaDept,GuideNum,TypeSign,g_commonguide.TEMPLETID from {0}.G_CommonGuide , {0}.G_GuideType where G_GuideType.ID = G_CommonGuide.CommGuideTypeID and CommGuideTypeID = ? and TypeSign = ? order by G_CommonGuide.ID ASC", DataUser.ZLGL);
            string SpecGuideNameDict = string.Format("select G_SpecialtyGuide.ID as GuideNameID,SpecGuideName as GuideName,SpecGuideTypeID as GuideTypeID,ManaDept,GuideNum,CheckDept,TypeSign from {0}.G_SpecialtyGuide , {0}.G_GuideType where G_GuideType.ID = G_SpecialtyGuide.SpecGuideTypeID and SpecGuideTypeID=? and TypeSign = ? order by G_SpecialtyGuide.ID ASC", DataUser.ZLGL);

            OleDbParameter[] CmdParms = new OleDbParameter[]{
															new OleDbParameter("GuideTypeID",ID),
														   new OleDbParameter("TypeSign",TypeSign)
														  
													   };


            DataSet ds = new DataSet();
            if (TypeSign != "1")
            {
                ds = OracleOledbBase.ExecuteDataSet(CommGuideNameDict, CmdParms);
            }
            else if (TypeSign == "1")
            {
                ds = OracleOledbBase.ExecuteDataSet(SpecGuideNameDict, CmdParms);
            }
            return ds;
        }
        /// <summary>
        /// 通过类别标志、指标名称ID获取该指标名称信息
        /// </summary>
        /// <param name="TypeSign">指标类别标志</param>
        /// <param name="ID">指标名称ID</param>
        /// <returns>DataSet</returns>
        public DataSet Guide_Name(string TypeSign, int ID)
        {

            string CommGuideName = string.Format("select G_CommonGuide.ID,CommGuideName,CommGuideTypeID,ManaDept,GuideNum,TypeSign from {0}.G_CommonGuide , {0}.G_GuideType WHERE G_GuideType.ID = G_CommonGuide.CommGuideTypeID and G_CommonGuide.ID = ? and TypeSign = ?", DataUser.ZLGL);
            string SpecGuideName = string.Format("select G_SpecialtyGuide.ID,SpecGuideName,SpecGuideTypeID,ManaDept,GuideNum,CheckDept,TypeSign from {0}.G_SpecialtyGuide , {0}.G_GuideType where G_GuideType.ID = G_SpecialtyGuide.SpecGuideTypeID and G_SpecialtyGuide.ID = ? and TypeSign = ?", DataUser.ZLGL);

            OleDbParameter[] CmdParms = new OleDbParameter[]{
																	new OleDbParameter("ID",ID),
																	new OleDbParameter("TypeSign",TypeSign)
															
																};

            DataSet ds = new DataSet();
            if (TypeSign != "1")
            {
                ds = OracleOledbBase.ExecuteDataSet(CommGuideName, CmdParms);
            }
            else if (TypeSign == "1")
            {
                ds = OracleOledbBase.ExecuteDataSet(SpecGuideName, CmdParms);
            }
            return ds;

        }
        /// <summary>
        /// 获得所有指标类别信息
        /// </summary>
        /// <returns>指标类别的DataSet</returns>
        public DataSet GuideType_View()
        {
            string GuideTypeView = string.Format("select ID,GuideType,TypeSign,GuideTypeNum from {0}.G_GuideType order by ID ASC", DataUser.ZLGL);

            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideTypeView);
            return ds;
        }
        /// <summary>
        /// 通过指标名称ID，指标类别ID获取该指标名称的所有内容信息
        /// </summary>
        /// <param name="ID">指标名称ID</param>
        /// <param name="GuideTypeID">指标类别ID</param>
        /// <returns>DataSet</returns>
        public DataSet Guide_Cont(int ID, int GuideTypeID)
        {
            string GuideCont = String.Format("select ID,CheckCont,CheckStan,CheckMeth,GuideNameID,GuideTypeID from {2}.G_GuideCheckContent a where  GuideNameID={0} and GuideTypeID={1} ", ID, GuideTypeID, DataUser.ZLGL);
            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideCont);
            return ds;
        }
        /// <summary>
        /// 取某一科室所在的专业指标ID号
        /// </summary>
        /// <param name="GroupCode">科室代号</param>
        /// <returns>专业指标的ID</returns>
        private int GetSpecGuideIDByGroup(string GroupCode)
        {
            int SpecGuideID = 0;
            DataSet ds = SpecialtyGuide_View();       //取专业指标
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)             //循环专业指标
            {
                string[] arrayCheckDept = BreakString.ShowText(ds.Tables[0].Rows[i]["CheckDept"].ToString());   //存放考核科室到数组中      
                for (int j = 0; j < arrayCheckDept.Length; j++)           //循环数组    
                {
                    if (arrayCheckDept[j] == GroupCode)             //判断数组考核科室是否等于参数
                    {
                        SpecGuideID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"].ToString());  //取出考核科室所在专业指标行的ID号
                        break;                                                                 //跳出内循环
                    }
                }
                if (SpecGuideID != 0)
                    break;                                                                    //取到专业指标ID号，跳出外循环
            }
            return SpecGuideID;
        }
        //删除功能
        public void DeleteFunc(string funckey, string tempid)
        {
            MyLists list = new MyLists();
            //删除功能角色对照
            string strdel = string.Format("DELETE FROM {0}.Sys_Role_Function WHERE Function_ID=?", DataUser.COMM);
            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = new OleDbParameter[] { new OleDbParameter("", funckey) };
            list.Add(listdel);

            //删除功能
            string sqlfun = string.Format("DELETE FROM {0}.Sys_Function_Dict WHERE (Function_ID = ?)", DataUser.COMM);
            List listfun = new List();
            listfun.StrSql = sqlfun;
            listfun.Parameters = new OleDbParameter[] { new OleDbParameter("", funckey) };
            list.Add(listfun);
            //删除模版
            string sqltemp = string.Format("UPDATE  {0}.T_TempletDict SET DELETED=1  WHERE (ID = ?)", DataUser.ZLGL);
            List listtemp = new List();
            listtemp.StrSql = sqltemp;
            listtemp.Parameters = new OleDbParameter[] { new OleDbParameter("", tempid) };
            list.Add(listtemp);
            OracleOledbBase.ExecuteTranslist(list);
        }
        /// <summary>
        /// 判断功能是否存在
        /// </summary>
        /// <param name="FunctionName"></param>
        /// <returns></returns>
        public bool IsExitFunctionInfo(string FunctionName)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0}.Sys_Function_Dict WHERE Function_Name=?", DataUser.COMM);
            int values = Convert.ToInt32(OracleOledbBase.ExecuteScalar(sql, new OleDbParameter("Function_Name", FunctionName)).ToString());
            return values == 0 ? false : true;

        }


        /// <summary>
        /// 添加功能的信息
        /// </summary>
        /// <param name="name">功能名称</param>
        /// <param name="remark">功能备注</param>
        public string AddFunction(string name, string remark)
        {
            string sql = string.Format("INSERT INTO {0}.Sys_Function_Dict "
                + "(FUNCTION_ID,Function_Name, Function_type,del_f) "
                + "VALUES (?, ?, ?,0)", DataUser.COMM);
            int id = int.Parse(Constant.ZLGL_FUN_TYPE + "001");
            string maxfun = string.Format("select max(function_id)+1 from {0}.sys_function_dict a where a.function_type='{1}'", DataUser.COMM, Constant.ZLGL_FUN_TYPE);
            object obj = OracleOledbBase.GetSingle(maxfun);
            if (obj != null)
            {
                id = int.Parse(obj.ToString());
            }

            OleDbParameter[] cmdParms = new OleDbParameter[]{
                                                                new OleDbParameter("id",id),
                                                                new OleDbParameter("name",GoldNet.Comm.CleanString.InputText(name,50)),
                                                                new OleDbParameter("type",Constant.ZLGL_FUN_TYPE)
                                                            };

            OracleOledbBase.ExecuteNonQuery(sql, cmdParms);
            return id.ToString();
        }
        /// <summary>
        /// 获取所有的模板，返回一个可以帮定到DataGrid的DataTable。
        /// </summary>
        /// <returns>包含所有使用模板的DataTable对象</returns>
        public DataTable GetAllTempletList()
        {
            string sql = string.Format("SELECT ID, Name, Title, CreateDate,tabname,showorder,common FROM {0}.T_TempletDict WHERE (Deleted = 0) order by showorder", DataUser.ZLGL);

            DataSet dataset = OracleOledbBase.ExecuteDataSet(sql);

            return dataset.Tables[0];
        }
        /// <summary>
        /// 得到模版的字段
        /// </summary>
        /// <param name="viewID"></param>
        /// <returns></returns>
        public DataTable GetFields(int viewID, int tempid)
        {
            string sql;
            System.Data.DataTable table;

            if (viewID == 0)
            {
                // 返回模板的所有字段集合
                sql = string.Format("SELECT T_TempletFieldDict.ID, T_TempletFieldDict.FieldName, T_TempletFieldDict.TempletID, T_TempletFieldDict.FieldTypeID,T_TempletFieldDict.FieldDefineID, T_TempletFieldDict.SortNum FROM {0}.T_ViewDisplayFiledsDict, {0}.T_TempletFieldDict where        T_ViewDisplayFiledsDict.FieldID = T_TempletFieldDict.ID and (T_TempletFieldDict.TempletID = ?) AND (T_ViewDisplayFiledsDict.ViewID = ?) ORDER BY T_TempletFieldDict.SortNum, T_TempletFieldDict.ID", DataUser.ZLGL);

                table = OracleOledbBase.ExecuteDataSet(sql,
                    new OleDbParameter("templetID", tempid),
                    new OleDbParameter("viewID", viewID)
                    ).Tables[0];
            }
            else
            {
                sql = string.Format("SELECT T_TempletFieldDict.ID, T_TempletFieldDict.FieldName, T_TempletFieldDict.TempletID, T_TempletFieldDict.FieldTypeID,       T_TempletFieldDict.FieldDefineID, T_TempletFieldDict.SortNum FROM  {0}.T_TempletFieldDict where    (T_TempletFieldDict.TempletID = ?) ORDER BY T_TempletFieldDict.SortNum, T_TempletFieldDict.ID", DataUser.ZLGL);

                table = OracleOledbBase.ExecuteDataSet(sql,
                    new OleDbParameter("templetID", tempid)

                    ).Tables[0];
            }
            return table;
        }
        /// <summary>
        /// 添加功能信息
        /// </summary>
        /// <param name="name">功能名称</param>
        /// <param name="remark">功能备注</param>
        /// <returns>返回添加的功能名称的序号</returns>
        public string AddFunctionInfo(string name, string remark)
        {
            if (!IsExitFunctionInfo(name))
            {
                return AddFunction(name, remark);
            }
            else
            {
                throw new GuideException("功能名称已经存在", "[" + name + "]功能名称已经存生，请重新输入功能名称！");
            }
        }
        /// <summary>
        /// 保存科室效率数据
        /// </summary>
        /// <returns></returns>
        public bool UpdateUserpass(string userCode, string oPass, string nPass)
        {
            string updatesql = "update rlzy.new_staff_info set PASS_WORD ='" + nPass + "' where PASS_WORD ='" + oPass + "' and USER_ID ='" + userCode + "'";
            try
            {
                OracleOledbBase.ExecuteNonQuery(updatesql);
                string updatehisPass = "update hisdata.users set password ='" + nPass + "' where state =1 and password='" + oPass + "'and USER_ID ='" + userCode + "'";
                OracleOledbBase.ExecuteNonQuery(updatehisPass);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
