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
    public class Guide_Manager
    {
        public Guide_Manager()
        { }
        /// <summary>
        /// 删除指标
        /// </summary>
        /// <param name="ID"></param>
        public void CommonGuide_Del(int ID)
        {
            MyLists listtable = new MyLists();
            //删除指标规则
            string commGuideDetail = string.Format(@"delete from {0}.G_GUIDECHECKCONTENT c where c.ID in (
select a.ID from {0}.G_GUIDECHECKCONTENT a,{0}.G_COMMONGUIDE b where a.GUIDENAMEID=b.ID and a.GUIDETYPEID=b.COMMGUIDETYPEID and b.ID=?)", DataUser.ZLGL);
            List listdeldetail = new List();
            listdeldetail.StrSql = commGuideDetail;
            listdeldetail.Parameters = new OleDbParameter[] { new OleDbParameter("id", ID) };
            listtable.Add(listdeldetail);
            //删除指标
            string CommGuideNameDel = string.Format("delete from {0}.G_CommonGuide where ID=?", DataUser.ZLGL);
            List listdel = new List();
            listdel.StrSql = CommGuideNameDel;
            listdel.Parameters = new OleDbParameter[] { new OleDbParameter("id", ID) };
            listtable.Add(listdel);
            OracleOledbBase.ExecuteTranslist(listtable);

        }
        /// <summary>
        /// 专业部分指标名称删除
        /// </summary>
        public void SpecialtyGuide_Del(int ID)
        {
            MyLists listtable = new MyLists();
            //删除指标规则
            string commGuideDetail = string.Format(@"delete from {0}.G_GUIDECHECKCONTENT c where c.ID in (
select a.ID from {0}.G_GUIDECHECKCONTENT a,{0}.G_COMMONGUIDE b where a.GUIDENAMEID=b.ID and a.GUIDETYPEID=b.COMMGUIDETYPEID and b.ID=?)", DataUser.ZLGL);
            List listdeldetail = new List();
            listdeldetail.StrSql = commGuideDetail;
            listdeldetail.Parameters = new OleDbParameter[] { new OleDbParameter("id", ID) };
            listtable.Add(listdeldetail);
            //删除指标
            string CommGuideNameDel = string.Format("delete from {0}.G_SpecialtyGuide where ID=?", DataUser.ZLGL);
            List listdel = new List();
            listdel.StrSql = CommGuideNameDel;
            listdel.Parameters = new OleDbParameter[] { new OleDbParameter("id", ID) };
            listtable.Add(listdel);
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <returns>指标类别的DataSet</returns>
        public DataSet GetGuideType()
        {
            string GuideTypeView = string.Format("select ID,GuideType,TypeSign,GuideTypeNum from {0}.G_GuideType order by ID ASC", DataUser.ZLGL);

            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideTypeView);
            return ds;
        }

        /// <summary>
        /// 删除指标类别信息
        /// </summary>
        /// <returns>是否成功删除标志</returns>
        public bool GuideType_Del(int ID)
        {
            string GuideTypeDel = string.Format("delete from {0}.G_GuideType where ID=?", DataUser.ZLGL);
            OleDbParameter[] CmdParms = new OleDbParameter[]{
															new OleDbParameter("ID",ID)
														};
            int Del = OracleOledbBase.ExecuteNonQuery(GuideTypeDel, CmdParms);
            if (Del == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 显示要修改的专业指标名称信息
        /// </summary>
        /// <param name="ID">专业指标名称ID</param>
        /// <return>符合条件的专业指标名称信息</return>
        public DataSet SpecialtyGuide_Edit_View(int ID)
        {
            string SpecGuideNameView = string.Format("select G_SpecialtyGuide.ID,SpecGuideName,SpecGuideTypeID,ManaDept,GuideNum,CheckDept,GuideType,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue,Arithmetic from {0}.G_SpecialtyGuide , {0}.G_GuideType where G_GuideType.ID = G_SpecialtyGuide.SpecGuideTypeID and G_SpecialtyGuide.ID=?", DataUser.ZLGL);
            OleDbParameter[] CmdParms = new OleDbParameter[]{
															new OleDbParameter("ID",ID)
														};

            DataSet ds = OracleOledbBase.ExecuteDataSet(SpecGuideNameView, CmdParms);
            return ds;
        }
        /// <summary>
        /// 专业部分指标名称添加
        /// </summary>
        /// <returns>是否成功添加标志</returns>
        public bool SpecialtyGuide_Add(string SpecGuideName, int SpecGuideTypeID, string ManaDept, double GuideNum, string CheckDept, int TempletID, int DateCol, int TargetCol, int GuideNameCol, int GuideNameColValue, int Arithmetic)
        {
            try
            {
                string[] strar = CheckDept.Split(',');
                string SpecGuideNameAdd = string.Format("insert into {0}.G_SpecialtyGuide(ID,SpecGuideName,SpecGuideTypeID,ManaDept,GuideNum,CheckDept,TempletID,DateCol,TargetCol,GuideNameCol,GuideNameColValue,Arithmetic,dept_numbers) values(?,?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.ZLGL);
                int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.G_SpecialtyGuide", DataUser.ZLGL));
                OleDbParameter[] CmdParms = new OleDbParameter[]{
																new OleDbParameter("ID",id),
																new OleDbParameter("SpecGuideName",SpecGuideName),
																new OleDbParameter("SpecGuideTypeID",SpecGuideTypeID),
																new OleDbParameter("ManaDept",ManaDept),
																new OleDbParameter("GuideNum",GuideNum),
																new OleDbParameter("CheckDept",CheckDept),
																new OleDbParameter("TempletID",TempletID),
																new OleDbParameter("DateCol",DateCol),
																new OleDbParameter("TargetCol",TargetCol), 
																new OleDbParameter("GuideNameCol",GuideNameCol),
																new OleDbParameter("GuideNameColValue",GuideNameColValue),
																new OleDbParameter("Arithmetic",Arithmetic),
                                                                new OleDbParameter("dept_numbers",strar.Length-1)
															};

                int Add = OracleOledbBase.ExecuteNonQuery(SpecGuideNameAdd, CmdParms);
                if (Add == 1)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw new SaveRecordDataException("保存专业指标时发生错误：", SpecGuideName + "已经存在，不能重复添加");
            }
        }
        /// <summary>
        /// 专业部分指标名称修改
        /// </summary>
        /// <returns>是否成功修改标志</returns>
        public bool SpecialtyGuide_Edit(string SpecGuideName, int SpecGuideTypeID, string ManaDept, double GuideNum, string CheckDept, int TempletID, int DateCol, int TargetCol, int GuideNameCol, int GuideNameColValue, int Arithmetic, int ID)
        {
            string[] strar = CheckDept.Split(',');
            string SpecGuideNameEdit = string.Format("update {0}.G_SpecialtyGuide set SpecGuideName=?,SpecGuideTypeID=?,ManaDept=?,GuideNum=?,CheckDept=?,TempletID=?,DateCol=?,TargetCol=?,GuideNameCol=?,GuideNameColValue=?,Arithmetic=?,dept_numbers=? where ID=?", DataUser.ZLGL);
            OleDbParameter[] CmdParms = new OleDbParameter[]{
																new OleDbParameter("SpecGuideName",SpecGuideName),
																new OleDbParameter("SpecGuideTypeID",SpecGuideTypeID),
																new OleDbParameter("ManaDept",ManaDept),
																new OleDbParameter("GuideNum",GuideNum),
																new OleDbParameter("CheckDept",CheckDept),
																new OleDbParameter("TempletID",TempletID),
																new OleDbParameter("DateCol",DateCol),
																new OleDbParameter("TargetCol",TargetCol),
																new OleDbParameter("GuideNameCol",GuideNameCol),
																new OleDbParameter("GuideNameColValue",GuideNameColValue),
																new OleDbParameter("Arithmetic",Arithmetic),
                                                                new OleDbParameter("dept_numbers",strar.Length-1),
																new OleDbParameter("ID",ID)
															};
            int Edit = OracleOledbBase.ExecuteNonQuery(SpecGuideNameEdit, CmdParms);
            if (Edit == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 查询指标分类
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataSet GetGuideTypeByid(int ID)
        {
            string GuideTypeEditView = string.Format("select GuideType,TypeSign,GuideTypeNum from {0}.G_GuideType where ID=?", DataUser.ZLGL);
            OleDbParameter[] CmdParms = new OleDbParameter[]{
															new OleDbParameter("ID",ID)
														};

            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideTypeEditView, CmdParms);
            return ds;
        }
        /// <summary>
        /// 查询专业指标内容分类
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataSet GetContenTypeByid(int ID)
        {
            string GuideTypeEditView = string.Format("select CONTENT_TYPE from {0}.G_CONTENT_TYPE where ID=? ", DataUser.ZLGL);
            OleDbParameter[] CmdParms = new OleDbParameter[]{
															new OleDbParameter("ID",ID)
														};

            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideTypeEditView, CmdParms);
            return ds;
        }
        /// <summary>
        /// 删除指标内容分类
        /// </summary>
        /// <param name="id"></param>
        public void DelContenTypeByid(int id)
        {
            string GuideTypeDel = string.Format("delete from {0}.G_CONTENT_TYPE where ID=?", DataUser.ZLGL);
            OleDbParameter[] CmdParms = new OleDbParameter[]{
															new OleDbParameter("ID",id)
														};
            OracleOledbBase.ExecuteNonQuery(GuideTypeDel, CmdParms);
        }
        /// <summary>
        /// 更新专业指标内容分类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="station"></param>
        /// <param name="guidetype"></param>
        public void UpdateContenType(int id, string station, string contentype, string guidetype)
        {
            if (station == "edit")
            {
                string GuideTypeEdit = string.Format("update {0}.G_CONTENT_TYPE set CONTENT_TYPE=? where ID=?", DataUser.ZLGL);
                OleDbParameter[] CmdParms = new OleDbParameter[]{
																	new OleDbParameter("CONTENT_TYPE",contentype),
																	new OleDbParameter("ID",id)
																};

                OracleOledbBase.ExecuteNonQuery(GuideTypeEdit, CmdParms);
            }
            else
            {
                string GuideTypeAdd = string.Format("insert into {0}.G_CONTENT_TYPE (ID,CONTENT_TYPE,SPECIAL_TYPE) values (?,?,?)", DataUser.ZLGL);
                id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.G_CONTENT_TYPE", DataUser.ZLGL));

                OleDbParameter[] CmdParms = new OleDbParameter[]{
																new OleDbParameter("ID",id),
																new OleDbParameter("CONTENT_TYPE",contentype),
																new OleDbParameter("SPECIAL_TYPE",guidetype)
															};

                OracleOledbBase.ExecuteNonQuery(GuideTypeAdd, CmdParms);
            }
        }
        //添加公共指标类别
        public void GuideType_Add(string GuideType, string TypeSign)
        {
            string GuideTypeAdd = string.Format("insert into {0}.G_GuideType (ID,GuideType,TypeSign,ICON) values (?,?,?,'UserEdit')", DataUser.ZLGL);
            int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.G_GuideType", DataUser.ZLGL));

            OleDbParameter[] CmdParms = new OleDbParameter[]{
																new OleDbParameter("ID",id),
																new OleDbParameter("GuideType",GuideType),
																new OleDbParameter("TypeSign",TypeSign)
															};

            OracleOledbBase.ExecuteNonQuery(GuideTypeAdd, CmdParms);
        }
        /// <summary>
        /// 修改指标类别信息
        /// </summary>

        public void GuideType_Edit(string GuideType, int GuideTypeNum, int ID)
        {
            DataTable dt = GetGuideTypeByid(ID).Tables[0];
            string TypeSign = dt.Rows[0]["TypeSign"].ToString();

            if (TypeSign == "0")           //公共指标类别直接修改                         
            {
                string GuideTypeEdit = string.Format("update {0}.G_GuideType set GuideType=?,GuideTypeNum=? where ID=?", DataUser.ZLGL);
                OleDbParameter[] CmdParms = new OleDbParameter[]{
																	new OleDbParameter("GuideType",GuideType),
																	new OleDbParameter("GuideTypeNum",GuideTypeNum),
																	new OleDbParameter("ID",ID)
																};

                OracleOledbBase.ExecuteNonQuery(GuideTypeEdit, CmdParms);
            }
            else                      //专业指标类别在修改后，还要修改所有专业指标分值
            {
                MyLists listtable = new MyLists();

                List listdeldetail = new List();
                listdeldetail.StrSql = string.Format("update {0}.G_GuideType set GuideType=?,GuideTypeNum=? where ID=?", DataUser.ZLGL);
                OleDbParameter[] CmdParms = new OleDbParameter[]{
																	new OleDbParameter("GuideType",GuideType),
																	new OleDbParameter("GuideTypeNum",GuideTypeNum),
																	new OleDbParameter("ID",ID)
																};
                listdeldetail.Parameters = CmdParms;
                listtable.Add(listdeldetail);

                List listspedetail = new List();
                listspedetail.StrSql = string.Format("update {0}.G_SpecialtyGuide set GuideNum=?", DataUser.ZLGL);
                listspedetail.Parameters = new OleDbParameter[] { new OleDbParameter("GuideNum", GuideTypeNum) };
                listtable.Add(listspedetail);

                OracleOledbBase.ExecuteTranslist(listtable);
            }

        }

        /// <summary>
        /// 通过指标名称ID，指标类别ID获取该指标名称的所有内容信息
        /// </summary>
        /// <param name="ID">指标名称ID</param>
        /// <param name="GuideTypeID">指标类别ID</param>
        /// <returns>DataSet</returns>
        public DataSet Guide_Cont(int ID, int GuideTypeID)
        {
            string GuideCont = String.Format("select a.ID,a.CheckCont,a.CheckStan,a.CheckMeth,a.GuideNameID,a.GuideTypeID, b.DEPT_NAME as deptcode from {0}.G_GuideCheckContent a,{1}.SYS_DEPT_DICT b where a.deptcode=b.DEPT_CODE(+) and GuideNameID={2} and GuideTypeID={3} order by deptcode", DataUser.ZLGL, DataUser.COMM, ID, GuideTypeID);
            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideCont);
            return ds;
        }
        /// <summary>
        /// 添加考评指标内容
        /// </summary>
        /// <returns>是否成功添加标志</returns>
        public bool GuideCheckContent_Add(string CheckCont, string CheckStan, string CheckMeth, int GuideNameID, int GuideTypeID, string Contentype, int iskpi)
        {
            string GuideCheckAdd = string.Format("insert into {0}.G_GuideCheckContent (ID,CheckCont,CheckStan,CheckMeth,GuideNameID,GuideTypeID,CONTENTYPE,ISKPI) values (?,?,?,?,?,?,?,?)", DataUser.ZLGL);
            int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.G_GuideCheckContent", DataUser.ZLGL));
            OleDbParameter[] CmdParms = new OleDbParameter[]{
															new OleDbParameter("ID",id),
															new OleDbParameter("CheckCont",CheckCont),
															new OleDbParameter("CheckStan",CheckStan),
															new OleDbParameter("CheckMeth",CheckMeth),
															new OleDbParameter("GuideNameID",GuideNameID),
				                                            new OleDbParameter("GuideTypeID",GuideTypeID),
				                                            new OleDbParameter("ContenType",Contentype.Trim()==""?"0":Contentype),
				                                             new OleDbParameter("ISKPI",iskpi),
														};

            int Add = OracleOledbBase.ExecuteNonQuery(GuideCheckAdd, CmdParms);
            if (Add == 1)
                return true;
            else
                return false;

        }
        /// <summary>
        /// add科室考评内容
        /// </summary>
        /// <param name="CheckCont"></param>
        /// <param name="CheckStan"></param>
        /// <param name="CheckMeth"></param>
        /// <param name="GuideNameID"></param>
        /// <param name="GuideTypeID"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public bool GuideCheckContentdept_Add(string CheckCont, string CheckStan, string CheckMeth, int GuideNameID, int GuideTypeID, string deptcode)
        {
            string GuideCheckAdd = string.Format("insert into {0}.G_GuideCheckContent (ID,CheckCont,CheckStan,CheckMeth,GuideNameID,GuideTypeID,DEPTCODE) values (?,?,?,?,?,?,?)", DataUser.ZLGL);
            int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.G_GuideCheckContent", DataUser.ZLGL));
            OleDbParameter[] CmdParms = new OleDbParameter[]{
																new OleDbParameter("ID",id),
																new OleDbParameter("CheckCont",CheckCont),
																new OleDbParameter("CheckStan",CheckStan),
																new OleDbParameter("CheckMeth",CheckMeth),
																new OleDbParameter("GuideNameID",GuideNameID),
																new OleDbParameter("GuideTypeID",GuideTypeID),
				                                                new OleDbParameter("DEPTCODE",deptcode)
				                                 
			};

            int Add = OracleOledbBase.ExecuteNonQuery(GuideCheckAdd, CmdParms);
            if (Add == 1)
                return true;
            else
                return false;

        }
        /// <summary>
        /// 删除考评指标内容
        /// </summary>
        /// <returns>是否成功删除标志</returns>
        public bool GuideCheckContent_Del(int ID)
        {
            string GuideCheckDel = string.Format("delete from {0}.G_GuideCheckContent where ID=?", DataUser.ZLGL);
            OleDbParameter[] CmdParms = new OleDbParameter[]{
															new OleDbParameter("ID",ID)
														};

            int Del = OracleOledbBase.ExecuteNonQuery(GuideCheckDel, CmdParms);
            if (Del == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guideid"></param>
        /// <param name="guidetype"></param>
        /// <returns></returns>
        public string GuideNameByguide(string guideid, string guidetype)
        {
            StringBuilder str = new StringBuilder();
            string strsign = string.Format("select TYPESIGN from  {0}.g_guidetype where id={1}", DataUser.ZLGL, int.Parse(guidetype));
            string sign = OracleOledbBase.GetSingle(strsign).ToString();
            if (sign.Equals("0"))
            {
                str.AppendFormat("select COMMGUIDENAME from {0}.G_COMMONGUIDE where id={1}", DataUser.ZLGL, guideid);
            }
            else
            {
                str.AppendFormat("select SPECGUIDENAME from {0}.G_SPECIALTYGUIDE where id={1}", DataUser.ZLGL, guideid);
            }
            DataTable table = OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
            if (table.Rows.Count > 0)
                return table.Rows[0][0].ToString();
            else
                return "";

        }
        /// <summary>
        /// 修改考评指标内容
        /// </summary>
        /// <returns>是否成功修改标志</returns>
        public bool GuideCheckContent_Edit(string CheckCont, string CheckStan, string CheckMeth, int ID, int iskpi)
        {
            string GuideCheckEdit = string.Format("update {0}.G_GuideCheckContent set CheckCont=?,CheckStan=?,CheckMeth=?,ISKPI=? where ID=?", DataUser.ZLGL);
            OleDbParameter[] CmdParms = new OleDbParameter[]{
															new OleDbParameter("CheckCont",CheckCont),
															new OleDbParameter("CheckStan",CheckStan),
															new OleDbParameter("CheckMeth",CheckMeth),
															new OleDbParameter("ISKPI",iskpi),
															new OleDbParameter("ID",ID)
														};

            int Edit = OracleOledbBase.ExecuteNonQuery(GuideCheckEdit, CmdParms);
            if (Edit == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 取得某一指标内容ID的该指标内容信息
        /// </summary>
        /// <param name="ID">指标内容ID</param>
        /// <returns>DataSet</returns>
        public DataSet Guide_Cont_Dict(int ID)
        {
            string GuideContDict = string.Format("select ID,CheckCont,CheckStan,CheckMeth,GuideNameID,GuideTypeID,ISKPI from {0}.G_GuideCheckContent where ID={1}", DataUser.ZLGL, ID);
            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideContDict);
            return ds;
        }
        /// <summary>
        /// 专业质量内容分类列表
        /// </summary>
        /// <param name="contentype"></param>
        /// <returns></returns>
        public DataSet getallconten(string contentype)
        {
            string str = string.Format("select a.id,a.CONTENT_TYPE,b.SPECGUIDENAME from {0}.G_CONTENT_TYPE a,{0}.G_SPECIALTYGUIDE b where a.SPECIAL_TYPE=b.id and a.SPECIAL_TYPE='" + contentype + "' order by a.id", DataUser.ZLGL);

            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 根据名称获取考评内容分类
        /// </summary>
        /// <returns></returns>
        public DataSet Conten_Type_all(string contentname)
        {
            string GuideTypeDict = "select a.ID,a.CONTENT_TYPE from zlgl.G_CONTENT_TYPE a,zlgl.G_SPECIALTYGUIDE b where a.SPECIAL_TYPE=b.ID AND b.SPECGUIDENAME='" + contentname + "' ORDER BY a.ID";

            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideTypeDict);
            return ds;
        }
        /// <summary>
        /// 是否在用
        /// </summary>
        /// <param name="id"></param>
        public bool ifexitcontentype(int id)
        {
            string str = string.Format("select id from   {0}.G_GUIDECHECKCONTENT where contentype='" + id + "'", DataUser.ZLGL);
            DataTable table = OracleOledbBase.ExecuteDataSet(str).Tables[0];
            if (table.Rows.Count > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 保存考评分类
        /// </summary>
        /// <param name="contentname"></param>
        /// <param name="specialtype"></param>
        public void savecontentype(int id, string contentname, string specialtype)
        {
            if (id != 0)
            {
                string str = string.Format("update {0}.G_CONTENT_TYPE set CONTENT_TYPE=? where id=?", DataUser.ZLGL);
                OleDbParameter[] CmdParms = new OleDbParameter[]{
																	
																	new OleDbParameter("CONTENT_TYPE",contentname),
					                                                new OleDbParameter("ID",id)
																	
																};
                OracleOledbBase.ExecuteNonQuery(str, CmdParms);
            }
            else
            {
                string str = string.Format("insert into {0}.G_CONTENT_TYPE (id,CONTENT_TYPE,SPECIAL_TYPE) values (?,?,?)", DataUser.ZLGL);
                id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.G_CONTENT_TYPE", DataUser.ZLGL));
                OleDbParameter[] CmdParms = new OleDbParameter[]{
																	new OleDbParameter("ID",id),
																	new OleDbParameter("CONTENT_TYPE",contentname),
																	new OleDbParameter("SPECIAL_TYPE",specialtype)
																};

                int Add = OracleOledbBase.ExecuteNonQuery(str, CmdParms);
            }

        }
        /// <summary>
        /// 修改专业指标科室
        /// </summary>
        /// <param name="id"></param>
        /// <param name="checkdept"></param>
        public void UpdateGuideDept(int id, string checkdept)
        {
            string[] strar = checkdept.Split(',');
            string str = string.Format("update {0}.G_SpecialtyGuide set CheckDept=?,dept_numbers=? where id=?", DataUser.ZLGL);
            OleDbParameter[] CmdParms = new OleDbParameter[]{
																new OleDbParameter("CheckDept",checkdept),
                                                                new OleDbParameter("dept_numbers",strar.Length-1),
																new OleDbParameter("ID",id)
															};
            OracleOledbBase.ExecuteNonQuery(str, CmdParms);

        }
        public DataTable GetQualitySubmit(string  user_id, string dates)
        {
            string str = string.Format(@"select a.id,A.NAME TEMPLET_NAME,D.DATES,to_char(D.SUBMIT_DATE,'yyyy-MM-dd') SUBMIT_DATE,D.SUBMIT_USER from zlgl.T_TEMPLETDICT a,
comm.SYS_POWER_DETAIL b,comm.SYS_ROLE_FUNCTION c,zlgl.QUALITY_SUBMIT d where b.target_id='{0}'
and B.POWER_ID=C.ROLE_ID and A.FUNCKEY=C.FUNCTION_ID and A.ID=D.TEMPLET_ID(+) and d.dates(+)='{1}' order by a.id ", user_id, dates);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }
        public void DelQualitySubmit(string dates, int temp_id)
        {
            string str = string.Format("delete {0}.QUALITY_SUBMIT where dates={1} and TEMPLET_ID={2}", DataUser.ZLGL, dates, temp_id);
            OracleOledbBase.ExecuteNonQuery(str);
        }
        public void SubmitQuality(string temp_id,string date, string user)
        {
            //int week = ConvertDate.GetWeekIndex(date);
            //string dates = ConvertDate.GetWeekRange(date);
            string years = date.Substring(0,4);
            //
            MyLists listtable = new MyLists();
            //删除
            string delete = string.Format(@"delete from {0}.QUALITY_SUBMIT c where c.TEMPLET_ID=? and DATES=?", DataUser.ZLGL);
            List listdeldetail = new List();
            listdeldetail.StrSql = delete;
            listdeldetail.Parameters = new OleDbParameter[] { new OleDbParameter("TEMPLET_ID", temp_id), new OleDbParameter("DATES", date) };
            listtable.Add(listdeldetail);

            string str = string.Format("insert into {0}.QUALITY_SUBMIT (TEMPLET_ID,dates,years,SUBMIT_USER,SUBMIT_DATE) values (?,?,?,?,sysdate)", DataUser.ZLGL);

            OleDbParameter[] CmdParms = new OleDbParameter[]{
																	new OleDbParameter("TEMPLET_ID",temp_id),
																	new OleDbParameter("dates",date),
																	new OleDbParameter("years",int.Parse(years)),
                                                                    new OleDbParameter("SUBMIT_USER",user)
																};
            List add = new List();
            add.StrSql = str;
            add.Parameters = CmdParms;
            listtable.Add(add);

            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 查询质量明细
        /// </summary>
        /// <param name="stardate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetQualityDetail(string stardate, string enddate, string mark, string code)
        {
            string deptCode = "";
            if (code != "")
            {
                deptCode= " AND a.dept_code='"+code+"'";
            }

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select a.ID,
                                       a.dept_code duty_dept_id,
                                       a.duty_dept_name,
                                       a.duty_user_name,
                                       a.checkcont,
                                       a.checkstan,
                                       a.date_time,
                                       nvl(a.numbers,0) numbers,
                                       a.memo,
                                       a.input_user,
                                       a.createdate,
                                       a.numbers_1
                                       from 
                              (SELECT a.ID, a.duty_dept_id dept_code, a.duty_dept_name, a.duty_user_name, b.checkcont,
                                   b.checkstan, a.date_time, a.numbers, a.memo, a.input_user,
                                   a.createdate,a.numbers_1
                              FROM {0}.quality_error_list a, {0}.g_guidecheckcontent b,{0}.T_TEMPLETDICT c
                             WHERE a.TEMPLET_ID=c.id and c.MARK='{3}' 
                              and  a.checkcont_id = b.ID 
                              and to_date(a.date_time,'yyyy-mm-dd')>=date'{1}' 
                              and to_date(a.date_time,'yyyy-mm-dd')<=date'{2}'  
                              order by a.date_time desc) a where 1=1 {4} 
                              UNION ALL
                            SELECT   ROWNUM ID,
                                     A.DEPT_CODE duty_dept_id,
                                     C.DEPT_NAME duty_dept_name,
                                     '' duty_user_name,
                                     B.MENU_GUIDE_NAME checkcont,
                                     B.MENU_GUIDE_NAME checkstan,
                                     TO_CHAR(A.ST_DATE,'yyyy-mm-dd') date_time,
                                     CASE WHEN B.FIELD_FLAG='F' THEN A.MENU_GUIDE_VALUE ELSE 0 END numbers,
                                     '' memo,
                                     '' input_user,
                                     A.ST_DATE createdate,
                                     CASE WHEN B.FIELD_FLAG='W' THEN A.MENU_GUIDE_VALUE ELSE null END numbers_1
                                     
                              FROM   HOSPITALSYS.SYS_MENU_DETAIL A, COMM.SYS_APP_MENU_GUIDE B,COMM.SYS_DEPT_DICT C
                             WHERE   A.APP_MENU_ID=B.APP_MENU_ID
                               AND   A.MENU_GUIDE_ID=B.MENU_GUIDE_ID
                               AND   A.DEPT_CODE=C.DEPT_CODE
                               AND   ST_DATE >= DATE '{1}' 
                               AND   ST_DATE <= DATE '{2}' {4} ORDER BY DATE_TIME,DUTY_DEPT_NAME ", DataUser.ZLGL, stardate, enddate, mark, deptCode);

            //if (code != "" )
            //{
            //    str.AppendFormat(" where dept_code='{0}' ", code);
            //}
          
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        public DataTable GetQualityDetailbycheckont(string stardate, string enddate, string mark, string code, string checkont)
        {
            string deptCode = "";
            string checkontstr = "";
            string checkontstr2 = "";
            if (code != "")
            {
                deptCode = " AND a.dept_code='" + code + "'";
            }

            if (checkont != "")
            {
                checkontstr = " and a.checkcont like '%" + checkont + "%' ";
                checkontstr2 = " AND B.MENU_GUIDE_NAME like '%" + checkont + "%'";
            }

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select a.ID,
                                       a.dept_code duty_dept_id,
                                       a.duty_dept_name,
                                       a.duty_user_name,
                                       a.checkcont,
                                       a.checkstan,
                                       a.date_time,
                                       nvl(a.numbers,0) numbers,
                                       a.memo,
                                       a.input_user,
                                       a.createdate,
                                       a.numbers_1
                                       from 
                              (SELECT a.ID, a.duty_dept_id dept_code, a.duty_dept_name, a.duty_user_name, b.checkcont,
                                   b.checkstan, a.date_time, a.numbers, a.memo, a.input_user,
                                   a.createdate,a.numbers_1
                              FROM {0}.quality_error_list a, {0}.g_guidecheckcontent b,{0}.T_TEMPLETDICT c
                             WHERE a.TEMPLET_ID=c.id and c.MARK='{3}' 
                              and  a.checkcont_id = b.ID 
                              and to_date(a.date_time,'yyyy-mm-dd')>=date'{1}' 
                              and to_date(a.date_time,'yyyy-mm-dd')<=date'{2}'  
                              order by a.date_time desc) a where 1=1 {4} {5}
                              UNION ALL
                            SELECT   ROWNUM ID,
                                     A.DEPT_CODE duty_dept_id,
                                     C.DEPT_NAME duty_dept_name,
                                     '' duty_user_name,
                                     B.MENU_GUIDE_NAME checkcont,
                                     B.MENU_GUIDE_NAME checkstan,
                                     TO_CHAR(A.ST_DATE,'yyyy-mm-dd') date_time,
                                     CASE WHEN B.FIELD_FLAG='F' THEN A.MENU_GUIDE_VALUE ELSE 0 END numbers,
                                     '' memo,
                                     '' input_user,
                                     A.ST_DATE createdate,
                                     CASE WHEN B.FIELD_FLAG='W' THEN A.MENU_GUIDE_VALUE ELSE null END numbers_1
                                     
                              FROM   HOSPITALSYS.SYS_MENU_DETAIL A, COMM.SYS_APP_MENU_GUIDE B,COMM.SYS_DEPT_DICT C
                             WHERE   A.APP_MENU_ID=B.APP_MENU_ID
                               AND   A.MENU_GUIDE_ID=B.MENU_GUIDE_ID
                               AND   A.DEPT_CODE=C.DEPT_CODE
                               AND   ST_DATE >= DATE '{1}' 
                               AND   ST_DATE <= DATE '{2}' {4} {6} ORDER BY DATE_TIME,DUTY_DEPT_NAME ", DataUser.ZLGL, stardate, enddate, mark, deptCode, checkontstr, checkontstr2);

            //if (code != "" )
            //{
            //    str.AppendFormat(" where dept_code='{0}' ", code);
            //}

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 获取考核指标列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCheckcont()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select distinct checkcont from ZLGL.G_GUIDECHECKCONTENT order by checkcont", "");

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 单项奖惩
        /// </summary>
        /// <param name="stardate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetSingleawardDetail(string stardate, string enddate)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT ID, DEPT_CODE, DEPT_NAME, TYPE_ID, TYPE_NAME, CHECKSTAN,
          REMARK, MONEY, to_char(AWARD_DATE,'yyyy-mm-dd') AWARD_DATE, INPUTER, INPUTE_DATE,
          MODIFIER, MODIFY_DATE, DEL_FLAG
     FROM {0}.v_input_singleaward where AWARD_DATE>=date'{1}' and AWARD_DATE<=date'{2}' order by AWARD_DATE desc", DataUser.ZLGL, stardate, enddate);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTableName(string id)
        {
            string sql = string.Format(@"select tabname from {0}.T_TEMPLETDICT where id= '{1}' and rownum <2", DataUser.ZLGL, id);

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);

            string res = string.Empty;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        res = ds.Tables[0].Rows[0][0].ToString();
                    }
                }
            }

            return res;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public DataSet GetColumnName(string tablename)
        {
            return OracleOledbBase.ExecuteDataSet(string.Format("select * from {0}.{1}", DataUser.ZLGL, tablename));
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="db_dept_id">科室编码</param>
        /// <param name="db_staff_id">人员编码</param>
        /// <param name="db_date">时间</param>
        /// <param name="db_number">得分</param>
        /// <param name="startdate">条件——开始时间</param>
        /// <param name="enddate">条件——结束时间</param>
        /// <param name="deptcode">条件——科室编码</param>
        /// <param name="staff_id">条件——人员编码</param>
        /// <param name="dclass">条件——大类</param>
        /// <param name="sclass">条件——小类</param>
        public DataSet GetSelectData(string tablename, string db_dept_id, string db_staff_id, string db_date, string db_number, string startdate, string enddate, string deptcode, string staff_id, string dclass, string sclass)
        {
            string db_staff_name = "LASTEDITSTAFF";
            string db_dept_name = "DEPT" + db_dept_id.Substring(7, db_dept_id.Length - 7);
            if (db_staff_id != "")
            {
                db_staff_name = "STAFF" + db_staff_id.Substring(8, db_staff_id.Length - 8);
            }
            else
                db_staff_id = "LASTEDITSTAFF";
            string sql = string.Empty;

            if (deptcode == "" && staff_id == "")
            {
                sql = string.Format(@" SELECT (SELECT {9} FROM {11}.{5} a WHERE a.{0} = b.{0} and rownum < 2) AS DEPTNAME,'{1}' AS GUIDETYPE,'{2}' AS COMMGUIDE,
(SELECT c.{10} FROM {11}.{5} c WHERE c.{3} = b.{3} and rownum < 2) AS PERSON, b.kgcs AS SORNUM, b.zkf AS ALLSORNUM
FROM (select {0},{3},count(*) as kgcs,sum({4}) as zkf  from {11}.{5} where to_char({6},'yyyymm') >= '{7}' and to_char({6},'yyyymm') <= '{8}'       and DELETED='0' group by {0},{3} ) b",
                    db_dept_id, dclass, sclass, db_staff_id, db_number, tablename, db_date, startdate, enddate, db_dept_name, db_staff_name, DataUser.ZLGL);
            }
            else if (deptcode != "" && staff_id == "")
            {
                sql = string.Format(@" SELECT (SELECT {9} FROM {12}.{5} a WHERE a.{0} = b.{0} and rownum < 2) AS DEPTNAME,'{1}' AS GUIDETYPE,'{2}' AS COMMGUIDE,
(SELECT c.{10} FROM {12}.{5} c WHERE c.{3} = b.{3} and rownum < 2) AS PERSON, b.kgcs AS SORNUM, b.zkf AS ALLSORNUM
FROM (select {0},{3},count(*) as kgcs,sum({4}) as zkf  from {12}.{5} where to_char({6},'yyyymm') >= '{7}' and to_char({6},'yyyymm') <= '{8}' and {0} = '{11}'  and DELETED='0' group by {0},{3} ) b",
                    db_dept_id, dclass, sclass, db_staff_id, db_number, tablename, db_date, startdate, enddate, db_dept_name, db_staff_name, deptcode, DataUser.ZLGL);
            }
            else if (deptcode == "" && staff_id != "")
            {
                sql = string.Format(@" SELECT (SELECT {9} FROM {12}.{5} a WHERE a.{0} = b.{0} and rownum < 2) AS DEPTNAME,'{1}' AS GUIDETYPE,'{2}' AS COMMGUIDE,
(SELECT c.{10} FROM {12}.{5} c WHERE c.{3} = b.{3} and rownum < 2) AS PERSON, b.kgcs AS SORNUM, b.zkf AS ALLSORNUM
FROM (select {0},{3},count(*) as kgcs,sum({4}) as zkf  from {12}.{5} where to_char({6},'yyyymm') >= '{7}' and to_char({6},'yyyymm') <= '{8}' and {3} = '{11}'  and DELETED='0' group by {0},{3} ) b",
                    db_dept_id, dclass, sclass, db_staff_id, db_number, tablename, db_date, startdate, enddate, db_dept_name, db_staff_name, staff_id, DataUser.ZLGL);
            }
            else if (deptcode != "" && staff_id != "")
            {
                sql = string.Format(@" SELECT (SELECT {9} FROM {13}.{5} a WHERE a.{0} = b.{0} and rownum < 2) AS DEPTNAME,'{1}' AS GUIDETYPE,'{2}' AS COMMGUIDE,
(SELECT c.{10} FROM {13}.{5} c WHERE c.{3} = b.{3} and rownum < 2) AS PERSON, b.kgcs AS SORNUM, b.zkf AS ALLSORNUM
FROM (select {0},{3},count(*) as kgcs,sum({4}) as zkf  from {13}.{5} where to_char({6},'yyyymm') >= '{7}' and to_char({6},'yyyymm') <= '{8}' and {0} = '{11}' and {3} = '{12}'  and DELETED='0' group by {0},{3} ) b",
                    db_dept_id, dclass, sclass, db_staff_id, db_number, tablename, db_date, startdate, enddate, db_dept_name, db_staff_name, deptcode, staff_id, DataUser.ZLGL);
            }

            return OracleOledbBase.ExecuteDataSet(sql);

        }

        /// <summary>
        /// 指标大类
        /// </summary>
        /// <returns></returns>
        public DataSet GetDDLguidetype()
        {
            string sql = string.Format(@"select id,guidetype from {0}.g_guidetype", DataUser.ZLGL);

            return OracleOledbBase.ExecuteDataSet(sql);
        }
        /// <summary>
        /// 绑定小类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet Getddl_commonguide(string id)
        {
            string sql = string.Empty;

            if (id != "2")
            {
                sql = string.Format(@"select * from {0}.g_commonguide
									where COMMGUIDETYPEID='{1}'", DataUser.ZLGL, id);
            }
            else
            {
                sql = string.Format(@"SELECT   g_specialtyguide.TEMPLETID AS TEMPLETID, specguidename AS commguidename
    FROM {0}.g_specialtyguide, {0}.g_guidetype
   WHERE g_guidetype.ID = g_specialtyguide.specguidetypeid
ORDER BY g_specialtyguide.ID ASC", DataUser.ZLGL);
            }


            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 指标类别
        /// </summary>
        /// <returns></returns>
        public DataSet Guide_Type_Dict()
        {
            string GuideTypeDict = string.Format("select ID,GuideType,TypeSign,GuideTypeNum from {0}.G_GuideType where TypeSign<>'9' order by ID ASC", DataUser.ZLGL);

            DataSet ds = OracleOledbBase.ExecuteDataSet(GuideTypeDict);
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptcodes"></param>
        /// <returns></returns>
        public DataTable GetDeptByDeptName(string deptname)
        {
            string str = string.Format("select dept_code,dept_name from {0}.SYS_DEPT_DICT where dept_name = '{1}'", DataUser.COMM, deptname);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 某一科室某一时间得分列表
        /// </summary>
        /// <param name="DeptCode">科室代号</param>
        /// <param name="DateDesc">时间说明</param>
        /// <returns></returns>
        public DataTable DeptScoreTable(string DeptCode, string DateDesc)
        {
            string SQL_DeptScoreTable = string.Format(@"SELECT GT.GuideType,CSS.GuideName,GuideNum,GuideSpareNum,PrimaryID,DateDesc,StartDate,EndDate,DeptName,TempletID,DateCol,TargetCol,GuideNameColValue FROM {0}.G_CollectScoreSecondary CSS , 
																			{0}.G_GuideType GT, {0}.G_CollectScorePrimary CSP,(SELECT CommGuideTypeID as GuideTypeID,CommGuideName as GuideName,TempletID,DateCol,TargetCol,GuideNameColValue 
																																																						FROM {0}.G_CommonGuide UNION SELECT SpecGuideTypeID as GuideTypeID,SpecGuideName as GuideName,TempletID,DateCol,TargetCol,
		GuideNameColValue FROM {0}.G_SpecialtyGuide) CS where GT.GuideType=CSS.GuideType  and CSP.ID=CSS.PrimaryID and CS.GuideName = CSS.GuideName 
																																	 and DeptName=? AND DateDesc=? and  gt.GUIDETYPE!='科室考评'", DataUser.ZLGL);
            OleDbParameter[] cmdPara = new OleDbParameter[]{
														   new OleDbParameter("DeptCode",DeptCode),
														   new OleDbParameter("DateDesc",DateDesc)
													   };
            DataTable dt = OracleOledbBase.ExecuteDataSet(SQL_DeptScoreTable, cmdPara).Tables[0];
            return dt;
        }


        /// <summary>
        /// 浏览专业部分指标名称
        /// </summary>
        /// <returns>获取专业部分指标名称信息</returns>
        public DataSet SpecialtyGuide_View()
        {
            string SpecGuideName = string.Format("select a.ID,a.SpecGuideName,a.SpecGuideTypeID,a.ManaDept,a.GuideNum,b.GuideType,b.TypeSign,a.CheckDept from {0}.G_SpecialtyGuide a, {0}.G_GuideType b where b.ID = a.SpecGuideTypeID order by a.ID ASC", DataUser.ZLGL);

            DataSet ds = OracleOledbBase.ExecuteDataSet(SpecGuideName);
            return ds;

        }
        //专业质量
        public DataSet SelectGuideSpe()
        {
            string strtype = string.Format("select * from {0}.G_GUIDETYPE where TYPESIGN=1", DataUser.ZLGL);
            DataSet typeset = OracleOledbBase.ExecuteDataSet(strtype);
            return typeset;
        }
        //指标列表
        public DataSet GetGuideContent()
        {
            string str = string.Format(@"SELECT   b.ID guidenameid, a.guidetype || '-' || c.sumguidenum || '分' AS guidetype,b.commguidename guidename,
         b.guidenum,  b.manadept,b.COMMGUIDETYPEID guidetypeid,a.typesign
    FROM {0}.g_guidetype a,
         {0}.g_commonguide b,
         (SELECT   g_guidetype.ID, g_guidetype.guidetype,
                   SUM (guidenum) AS sumguidenum
              FROM {0}.g_guidetype, {0}.g_commonguide
             WHERE g_commonguide.commguidetypeid = g_guidetype.ID
          GROUP BY g_guidetype.ID, g_guidetype.guidetype,
                   g_guidetype.typesign) c
   WHERE b.commguidetypeid = a.ID AND a.ID = c.ID
union all
SELECT   g_specialtyguide.ID AS guidenameid,'专业质量-'||guidenum||'分' as guidetype, specguidename AS guidename,
        guidenum,manadept, specguidetypeid AS guidetypeid,
         typesign
    FROM {0}.g_specialtyguide, {0}.g_guidetype
   WHERE g_guidetype.ID = g_specialtyguide.specguidetypeid", DataUser.ZLGL);
            return OracleOledbBase.ExecuteDataSet(str);
        }

    }
}
