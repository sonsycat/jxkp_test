using System;
using System.Data;
using System.Collections;
using System.Data.OleDb;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using GoldNet.JXKP.Templet.BLL.Fields;
using GoldNet.Comm;
using GoldNet.JXKP.BLL.Organise;
using GoldNet.JXKP.Templet.BLL;
using GoldNet.JXKP.PowerManager;
using GoldNet.Comm.DAL.Oracle;
using System.Text;
using GoldNet.Model;

namespace GoldNet.JXKP.BLL.Guide
{
	/// <summary>
	/// 指标类
	/// </summary>
	public class QualityGuide
	{
       public static  Goldnet.Dal.ZLGL_Guide_Dict dal = new Goldnet.Dal.ZLGL_Guide_Dict();
		#region   私有变量
        private GuideInfo _guideInfo;

		#endregion

		#region   内部实体  
		public class GuideInfo
		{
			public int ID;                    //指标名称ID
			public string GuideName;          //指标名称
			public int GuideTypeID;           //指标类别ID
			public string GuideType;          //指标类别
			public string TypeSign;           //指标类别标志  0为公共指标，1为专业指标
			public double GuideNum;           //指标分值
			public int TempletID;             //模板列ID
			public int DateCol;               //时间列ID
			public int TargetCol;             //科室列ID
			public int GuideNameCol;          //考评内容列ID
			public int GuideNameColValue;     //考评值列ID

			public GuideInfo(int id,string guideName,int guideTypeID,string guideType,string typeSign,double guideNum,int templetID,int dateCol,int targetCol,int guideNameCol,int guideNameColValue)
			{
				ID = id;
				GuideName = guideName;
				GuideTypeID = guideTypeID;
				GuideType = guideType;
				TypeSign = typeSign;
				GuideNum = guideNum;
				TempletID = templetID;
				DateCol = dateCol;
				TargetCol = targetCol;
				GuideNameCol = guideNameCol;
				GuideNameColValue = guideNameColValue;
			}

			public GuideInfo(){}

		}
		#endregion

		#region  构造函数
		public QualityGuide(string guideName)
		{
			_guideInfo = getGuideByName(guideName);
		}

		public QualityGuide(int id,string guideName,int guideTypeID,string guideType,string typeSign,double guideNum,int templetID,int dateCol,int targetCol,int guideNameCol,int guideNameColValue)
		{
            _guideInfo = new GuideInfo();
			_guideInfo.ID = id;
			_guideInfo.GuideName = guideName;
			_guideInfo.GuideTypeID = guideTypeID;
			_guideInfo.GuideType = guideType;
			_guideInfo.TypeSign = typeSign;
			_guideInfo.GuideNum = guideNum;
			_guideInfo.TempletID = templetID;
			_guideInfo.DateCol = dateCol;
			_guideInfo.TargetCol = targetCol;
			_guideInfo.GuideNameCol = guideNameCol;
			_guideInfo.GuideNameColValue = guideNameColValue;
		}
		#endregion

		#region  外部方法
		/// <summary>
		/// 获取所有指标
		/// </summary>
		/// <returns>DataSet</returns>
		public static DataSet GetAllGuide()
		{
           return dal.GetAllGuide();
		}

		/// <summary>
		/// 获取所有时间说明
		/// </summary>
		/// <returns></returns>
		public static DataSet GetAllDateDesc()
		{
            return dal.GetAllDateDesc();
		}

		/// <summary>
		/// 获取某一指标名称的考评内容
		/// </summary>
		/// <param name="guideName">指标名称</param>
		/// <returns>DataSet</returns>
		public static DataSet GetGuideContent(string guideName)
		{
			QualityGuide guide = new QualityGuide(guideName);                        //初始化指标对象

			int guideNameID = guide.ID;                                             //返回指标名称ID
			int guideTypeID = guide.GuideTypeID;                                    //返回指标类别ID
            return dal.GetGuideContent(guideNameID,guideTypeID);
		}
		//
		/// <summary>
		/// 科室考评指标
		/// </summary>
		/// <param name="guideName"></param>
		/// <param name="deptcode"></param>
		/// <returns></returns>
		public static DataSet GetGuideContentdept(string guideName,string deptcode)
		{
			QualityGuide guide = new QualityGuide(guideName);                        //初始化指标对象

			int guideNameID = guide.ID;                                             //返回指标名称ID
			int guideTypeID = guide.GuideTypeID;                                    //返回指标类别ID

            return dal.GetGuideContentdept(guideNameID,guideTypeID,deptcode);
		}

		public static DataSet GetGuidesByGroupdept(string GroupCode)
		{
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            return dal.GetGuidesByGroupdept(GroupCode);
		}

		/// <summary>
		/// 检验所有科室的指标总分
		/// </summary>
		/// <returns>返回指标总分不为100分的科室</returns>
		public static string CheckAllGroupScore()
		{

            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
			string GroupGuideTotalScore = "";
            if (GetConfig.GetConfigString("CheckScore") != "0")
            {
                DataTable table = dal.GetAAccountDept("").Tables[0];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    double GroupGuideScore = CheckGroupScore(table.Rows[i]["dept_code"].ToString());   //取得每个科室的指标总分
                    if (GroupGuideScore != Convert.ToInt32(OracleOledbBase.CountNumber))                                                                                 //记录指标总分不为100分的科室及得分
                        GroupGuideTotalScore = GroupGuideTotalScore + table.Rows[i]["dept_name"].ToString() + "指标总分为" + GroupGuideScore.ToString() + "分,";
                }
            }
			return GroupGuideTotalScore;
		}

		/// <summary>
		/// 检验每个指标的模板、时间列、科室列、数值列......是否存在
		/// </summary>
		/// <returns>不存在模板、时间列、科室列、数值列......的相应信息</returns>
		public static string CheckTempletIsExisted()
		{
			string TempletCol = "";
			int TempletID;
			int DateCol;
			int TargetCol;
			int GuideNameColValue;
            DataTable table = GetAllGuide().Tables[0];

            for (int i = 0; i < table.Rows.Count; i++)
			{
				//模板列
                if (table.Rows[i]["TempletID"].ToString() != "")
				{
                    TempletID = Convert.ToInt32(table.Rows[i]["TempletID"].ToString());
					try
					{
						new TempletBO(TempletID);
					}
					catch(TempletIDNotExistedException)
					{
                        TempletCol = TempletCol + table.Rows[i]["GuideName"] + "指标对应的模板不存在,";
					}                  
				}

				else
				{
                    TempletCol = TempletCol + table.Rows[i]["GuideName"] + "指标没有对应的模板,";
				}

				//时间列
                if (table.Rows[i]["DateCol"].ToString() != "")
				{
                    DateCol = Convert.ToInt32(table.Rows[i]["DateCol"].ToString());
					try
					{
						new Field(DateCol);
					}
					catch
					{
                        TempletCol = TempletCol + table.Rows[i]["GuideName"] + "指标对应的时间列不存在,";
					}
				}
				else
				{
                    TempletCol = TempletCol + table.Rows[i]["GuideName"] + "指标没有对应的时间列,";
				}

				//科室列
                if (table.Rows[i]["TargetCol"].ToString() != "")
				{
                    TargetCol = Convert.ToInt32(table.Rows[i]["TargetCol"].ToString());
					try
					{
						new Field(TargetCol);
					}
					catch
					{
                        TempletCol = TempletCol + table.Rows[i]["GuideName"] + "指标对应的科室列不存在,";
					}
				}
				else
				{
                    TempletCol = TempletCol + table.Rows[i]["GuideName"] + "指标没有对应的科室列,";
				}


				//考评数值列
                if (table.Rows[i]["GuideNameColValue"].ToString() != "")
				{
                    GuideNameColValue = Convert.ToInt32(table.Rows[i]["GuideNameColValue"].ToString());
					try
					{
						new Field(GuideNameColValue);
					}
					catch
					{
                        TempletCol = TempletCol + table.Rows[i]["GuideName"] + "指标对应的数值列不存在,";
					}
				}
				else
				{
                    TempletCol = TempletCol + table.Rows[i]["GuideName"] + "指标没有对应的数值列,";
				}
			}
			return TempletCol;
		}

		/// <summary>
		/// 生成考评汇总数据方法
		/// </summary>
		/// <param name="DateDesc">时间说明</param>
		/// <param name="StartDate">开始时间</param>
		/// <param name="EndDate">结束时间</param>
		/// <param name="CheckNum">系数</param>
		/// <returns></returns>
        public static void BuildGuideScore(string DateDesc, DateTime StartDate, DateTime EndDate, int CheckNum, OleDbTransaction trans)
		{
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
            if (guidedal.IsExistCheckCollect(DateDesc) == true)               //该时间说明的考评汇总数据是否存在，如果存在调用删除方法
			{
               guidedal.DelCheckCollect(DateDesc, trans);
			}

            int PrimaryID = Convert.ToInt32(guidedal.AddPrimary(DateDesc, StartDate, EndDate, CheckNum, trans).ToString());       //增加主表信息      
			DataSet ds = dal.GetAAccountDept("");                 //所有科室   
            int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.G_CollectScoreSecondary", DataUser.ZLGL));                         
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)        //循环科室
			{
				string GroupCode = ds.Tables[0].Rows[i]["dept_code"].ToString();         //科室代号
				DataTable dt = GetGuidesByGroupdept(GroupCode).Tables[0];                     //某一科室所有指标
				for(int j=0;j<dt.Rows.Count;j++)                                          //循环指标
				{
					
					string GuideName = dt.Rows[j]["GuideName"].ToString();
					string GuideType = dt.Rows[j]["GuideType"].ToString();
					double GuideNum = Convert.ToDouble(dt.Rows[j]["GuideNum"].ToString())*CheckNum;
                    string TYPESIGN = dt.Rows[j]["TYPESIGN"].ToString();//专业质量

					QualityGuide guide = new QualityGuide(GuideName);              //初始化指标对象

					int TempletID = Convert.ToInt32(guide.TempletID.ToString());    //模板ID
					int DateCol = Convert.ToInt32(guide.DateCol.ToString());        //时间列ID
					int TargetCol = Convert.ToInt32(guide.TargetCol.ToString());    //部门列ID
					int GuideNameColValue = Convert.ToInt32(guide.GuideNameColValue.ToString());   //数值列ID

					TempletBO templet = new TempletBO(TempletID);                  //初始化模板对象
					
					try
					{
						double GuideCutScore = Convert.ToDouble(templet.GetCountValue(DateCol,TargetCol,GuideNameColValue,StartDate,EndDate,GroupCode));   //获取指标实际扣分
						double GuideSpareScore =CountDeptScore(GuideName,GuideCutScore,CheckNum);       //计算指标的最后得分
                       
						bool AddDeptScore =guidedal.addDeptScore(id,GroupCode,GuideType,GuideName,GuideNum,GuideCutScore,GuideSpareScore,PrimaryID,TYPESIGN, trans);	//增加考评汇总数据
                        id++;
					}

					catch(TempletException ex)
					{
						throw new GlobalException("生成质量汇总数据时发生异常", "错误信息为：", ex);
					}
					catch(Exception ex)
					{
						throw new GlobalException("生成质量汇总数据时发生异常", "错误信息为：", ex);
					}
				}		
			}
		}
        public static void savefiles(string primaryid, string filesname, string filesid)
        {
            string strDel = string.Format(@"delete from zlgl.G_COLLECTS_FILES where PRIMARYID='{0}'", primaryid);
            GoldNet.Comm.DAL.Oracle.MyLists listtable = new GoldNet.Comm.DAL.Oracle.MyLists();
            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] {};
            listtable.Add(listDel);

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"INSERT INTO zlgl.G_COLLECTS_FILES 
                                          (ID,PRIMARYID,FILES_NAME,FILES_ID)
                                        VALUES(?,?,?,?)");
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("id",OracleOledbBase.GetMaxID("id","zlgl.G_COLLECTS_FILES")),
											  new OleDbParameter("PRIMARYID",primaryid),
											  new OleDbParameter("filesname",filesname),
											  new OleDbParameter("filesid",filesid)
											  
										  };
            List listAdd = new List();
            listAdd.StrSql = strSql;
            listAdd.Parameters = parameteradd;
            listtable.Add(listAdd);
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        public static DataTable selectfiles(string primaryid)
        {
            string str = string.Format("select * from zlgl.G_COLLECTS_FILES where PRIMARYID={0}",primaryid);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

		/// <summary>
		/// 按时间查询质量数据
		/// </summary>
		/// <param name="DateDesc">时间说明</param>
		/// <param name="DeptName">科室</param>
		/// <param name="GuideType">指标类别</param>
		/// <returns></returns>
        public static DataTable QualitySearchByDate(int DateDesc, string DeptName, int GuideType, string year, string month, string DeptFilter)
		{
            string datestr = year + "-" + month;
			DataTable dt = null;                  //创建表
            dt = GuideTable.CreatGuideCollectTableByGuide(GuideType);                  //创建表
            dt = GuideTable.FillGuideCollectTable(dt, DateDesc, DateDesc, DeptName, GuideType, datestr, DeptFilter);      //填充表

			return dt;
		}
        public static DataTable QualitySearchlist(string stardate, string enddate)
        {
            string types = "select * from zlgl.G_SPECIALTYGUIDE";
            DataTable table = OracleOledbBase.ExecuteDataSet(types).Tables[0];
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select b.dept_code,b.dept_name \"科室\"");
            double numbers = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                numbers += double.Parse(table.Rows[i]["GUIDENUM"].ToString());
                str.AppendFormat(" ,{1}+SUM (DECODE (a.TEMPLET_ID, '{0}', a.NUMBERS, 0)) \"{0}\"", table.Rows[i]["TEMPLETID"].ToString(), table.Rows[i]["GUIDENUM"].ToString());
               
            }
            str.AppendFormat(" ,sum(a.numbers)+{0} \"合计\"",numbers);
            str.AppendFormat(" from zlgl.QUALITY_ERROR_LIST a, comm.sys_dept_dict b");
            str.AppendFormat("  WHERE a.DATE_TIME >= '{0}' AND a.DATE_TIME <= '{1}' and A.DUTY_DEPT_ID=b.dept_code and a.templet_id in (select TEMPLETID from zlgl.G_SPECIALTYGUIDE)  group by b.dept_name,b.dept_code order by b.dept_code", stardate, enddate);
            DataTable tb= OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
            for (int i = 0;i< tb.Rows.Count; i++)
            {
                DataTable tbguide=GetGuidesByGroupdept(tb.Rows[i]["dept_code"].ToString()).Tables[0];
                for (int j = 2; j < tb.Columns.Count-1; j++)
                {
                    bool flags=false;
                    for (int h = 0; h < tbguide.Rows.Count; h++)
                    {
                        if (tbguide.Rows[h]["TEMPLETID"].ToString() == tb.Columns[j].ColumnName)
                        {
                            flags = true;

                        }
                        
                    }
                    if (flags == false)
                    {
                        tb.Rows[i][tb.Columns.Count - 1] = double.Parse(tb.Rows[i][tb.Columns.Count - 1].ToString()) - double.Parse(tb.Rows[i][j].ToString());
                        tb.Rows[i][j] = 0;
                    }
                }
                

            }
            for (int i = 0;i< tb.Columns.Count; i++)
            {
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    if (tb.Columns[i].ColumnName == table.Rows[j]["TEMPLETID"].ToString())
                        tb.Columns[i].ColumnName = table.Rows[j]["SPECGUIDENAME"].ToString();
                }
            }
            return tb;

        }

		/// <summary>
		/// 按指标查询质量数据
		/// </summary>
		/// <param name="StartDateDesc">开始时间</param>
		/// <param name="EndDateDesc">结束时间</param>
		/// <param name="DeptName">科室</param>
		/// <param name="GuideType">指标类别</param>
		/// <returns></returns>
        public static DataTable QualitySearchByGuide(int StartDateDesc, int EndDateDesc, string DeptName, int GuideType,string deptfilter)
        {
            DataTable dt = GuideTable.CreatDateCollectTableByDate(StartDateDesc, EndDateDesc);    //创建表

            dt = GuideTable.FillDateCollectTable(dt, StartDateDesc, EndDateDesc, DeptName, GuideType,deptfilter);                         //填充表                   

            return dt;
        }

       

		#endregion

		#region  非公有方法
		/// <summary>
		/// 获得内部的VO
		/// </summary>
		/// <param name="guideName">指标名称</param>
		/// <returns>返回指定名称的指标的ID，指标类型的ID......</returns>
        private GuideInfo getGuideByName(string guideName)
        {
            Goldnet.Dal.ZLGL_Guide_Dict dal = new Goldnet.Dal.ZLGL_Guide_Dict();
            DataSet ds = dal.getGuideByName(guideName);
            if (ds.Tables[0].Rows.Count != 0)
            {
                int intID = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                string strGuideName = ds.Tables[0].Rows[0][1].ToString();
                int intGuideTypeID = Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString());
                string strGuideType = ds.Tables[0].Rows[0][3].ToString();
                string strTypeSign = ds.Tables[0].Rows[0][4].ToString();
                double dGuideNum = Convert.ToDouble(ds.Tables[0].Rows[0][5].ToString());
                int intTempletID = Convert.ToInt32(ds.Tables[0].Rows[0][6].ToString());
                int intDateCol = Convert.ToInt32(ds.Tables[0].Rows[0][7].ToString());
                int intTargetCol = Convert.ToInt32(ds.Tables[0].Rows[0][8].ToString());
                int intGuideNameCol = Convert.ToInt32(ds.Tables[0].Rows[0][9].ToString());
                int intGuideNameColValue = Convert.ToInt32(ds.Tables[0].Rows[0][10].ToString());

                GuideInfo guideInfo = new GuideInfo(intID, strGuideName, intGuideTypeID, strGuideType, strTypeSign, dGuideNum, intTempletID, intDateCol, intTargetCol, intGuideNameCol, intGuideNameColValue);
                return guideInfo;
            }
            else
            {
                throw new GuideNameNotExistedException(guideName);
            }
        }

		/// <summary>
		/// 检验某一科室的指标总分
		/// </summary>
		/// <returns>返回科室的指标总分</returns>
        private static double CheckGroupScore(string GroupCode)
        {
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            double GroupGuideScore = 0.0;
            DataTable dt = dal.GetGuidesByGroupdepttest(GroupCode).Tables[0];
            object objSum;
            objSum = dt.Compute("Sum(GuideNum)", "");
            GroupGuideScore = Convert.ToDouble(objSum.ToString().Equals("") ? "0" : objSum.ToString());
            return GroupGuideScore;
        }


		/// <summary>
		/// 根据扣分求某一指标的最后得分
		/// </summary>
		/// <param name="GuideName">指标名称</param>
		/// <param name="GuideCutNum">实际扣分</param>
		/// <param name="CheckNum">系数</param>
		/// <returns>最后得分</returns>
		private static double CountDeptScore(string GuideName,double GuideCutNum,int CheckNum)
		{
			double GuideSpareNum;
			QualityGuide guide = new QualityGuide(GuideName);
            double GuideNum = guide.GuideNum;
			if(CheckNum!=0)
			{
				GuideNum = GuideNum * CheckNum;
			}
			GuideSpareNum = GuideNum + GuideCutNum;
            if (!GetConfig.GetConfigString("Score").Equals("0"))
            {
                if (GuideSpareNum < 0) GuideSpareNum = 0;
            }
			return GuideSpareNum;
		}
		#endregion

		#region  属性
		/// <summary>
		/// 指标名称ID
		/// </summary>
		public int ID
		{
			get
			{
				return _guideInfo.ID;
			}
		}

		/// <summary>
		/// 指标名称
		/// </summary>
		public string GuideName
		{
			get
			{
				return _guideInfo.GuideName;
			}
		}

		/// <summary>
		/// 指标类别ID
		/// </summary>
		public int GuideTypeID
		{
			get
			{
				return _guideInfo.GuideTypeID;
			}
		}

		/// <summary>
		/// 指标类别
		/// </summary>
		public string GuideType
		{
			get
			{
				return _guideInfo.GuideType;
			}
		}

		/// <summary>
		/// 指标类别标志
		/// </summary>
		public string TypeSign
		{
			get
			{
				return _guideInfo.TypeSign;
			}
		}

		/// <summary>
		/// 指标分值
		/// </summary>
		public double GuideNum
		{
			get
			{
				return _guideInfo.GuideNum;
			}
		}

		/// <summary>
		/// 模板列ID
		/// </summary>
		public int TempletID
		{
			get
			{
				return _guideInfo.TempletID;
			}
		}

		/// <summary>
		/// 时间列ID
		/// </summary>
		public int DateCol
		{
			get
			{
				return _guideInfo.DateCol;
			}
		}

		/// <summary>
		/// 科室列ID
		/// </summary>
		public int TargetCol
		{
			get
			{
				return _guideInfo.TargetCol;
			}
		}

		/// <summary>
		/// 考评内容列ID
		/// </summary>
		public int GuideNameCol
		{
			get
			{
				return _guideInfo.GuideNameCol;
			}
		}

		/// <summary>
		/// 考评值列ID
		/// </summary>
		public int GuideNameColValue
		{
			get
			{
				return _guideInfo.GuideNameColValue;
			}
		}

		/// <summary>
		/// 选项值ID
		/// </summary>
//		public int GuideSelectColValue
//		{
//			get
//			{
//				return _guideInfo.GuideSelectColValue;
//			}
//		}

		#endregion

	}

	/// <summary>
	/// 动态列表格
	/// </summary>
	public class GuideTable
	{

        #region   私有变量
		private int CountCol;
		#endregion

		#region 构造函数
		public GuideTable(int GuideType)
		{
			CountCol = CreatGuideCollectTableByGuide(GuideType).Columns.Count;
		}

		public GuideTable(int StartDateDesc,int EndDateDesc)
		{
			CountCol = CreatDateCollectTableByDate(StartDateDesc,EndDateDesc).Columns.Count;
		}
		#endregion

		#region  公有方法
		/// <summary>
		/// 通过指标类别创建指标汇总表
		/// </summary>
		/// <param name="GuideType">指标类别</param>
		/// <returns>以指标类别或者指标名称作为列名</returns>
        public static DataTable CreatGuideCollectTableByGuide(int GuideType)
        {
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
            string TypeSign = "";
            if (GuideType != 0)
                TypeSign = dal.GetGuideTypeByID(GuideType).Rows[0]["TypeSign"].ToString();     //获取某一类别ID的类别标志
            DataTable SearchTable = new DataTable();                             //创建表对象

            DataColumn DeptColumn = new DataColumn();                            //创建科室名称列	
            if (TypeSign == "9")//科室考评
            {
                DeptColumn.ColumnName = "姓名";
            }
            else
            {
                DeptColumn.ColumnName = "科室";
            }
            SearchTable.Columns.Add(DeptColumn);

            if (GuideType == 0)                                                    //全部指标
            {
                DataTable dt =guidedal.Guide_Type_Dict().Tables[0];              //所有指标类别  
                for (int i = 0; i < dt.Rows.Count; i++)                                 //以指标类别名称为列名添加列
                {
                    //DataColumn SearchColumn = new DataColumn();
                    //SearchColumn.ColumnName = dt.Rows[i]["GuideType"].ToString();
                    //SearchTable.Columns.Add(SearchColumn);
                    SearchTable.Columns.Add(dt.Rows[i]["GuideType"].ToString(), typeof(decimal));
                }
            }
            else                                                                //单个指标
            {

                DataTable dt = dal.Guide_Name_Dict(TypeSign, GuideType).Tables[0];                     //一个指标类别下的全部指标
                for (int i = 0; i < dt.Rows.Count; i++)                                                          //以指标名称为列名添加列                       
                {
                    //DataColumn SearchColumn = new DataColumn();
                    //SearchColumn.ColumnName = dt.Rows[i]["GuideName"].ToString();
                    //SearchTable.Columns.Add(SearchColumn);
                    SearchTable.Columns.Add(dt.Rows[i]["GuideName"].ToString(), typeof(decimal));
                }
            }
            DataColumn TotalColumn = new DataColumn();                         //创建汇总列
            TotalColumn.ColumnName = "汇总";
            SearchTable.Columns.Add(TotalColumn);

            return SearchTable;
        }

       //科室指标汇总
		public static DataTable CreatGuideCollectTableByGuidedept(int GuideType)
		{
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
			string TypeSign="";
			if(GuideType!=0)
				TypeSign = dal.GetGuideTypeByID(GuideType).Rows[0]["TypeSign"].ToString();     //获取某一类别ID的类别标志
			DataTable SearchTable = new DataTable();                             //创建表对象

			DataColumn DeptColumn = new DataColumn();                            //创建科室名称列	
			DeptColumn.ColumnName = "姓名";
			SearchTable.Columns.Add(DeptColumn);

			if(GuideType== 0)                                                    //全部指标
			{
				DataTable dt = guidedal.Guide_Type_Dict().Tables[0];              //所有指标类别  
				for(int i=0;i<dt.Rows.Count;i++)                                 //以指标类别名称为列名添加列
				{
					DataColumn SearchColumn = new DataColumn();
					SearchColumn.ColumnName = dt.Rows[i]["GuideType"].ToString(); 			    
					SearchTable.Columns.Add(SearchColumn);
				}					
			}
			else                                                                //单个指标
			{
				
				DataTable dt = dal.Guide_Name_Dict(TypeSign,GuideType).Tables[0];                     //一个指标类别下的全部指标
				for(int i=0;i<dt.Rows.Count;i++)                                                          //以指标名称为列名添加列                       
				{
					DataColumn SearchColumn = new DataColumn(); 
					SearchColumn.ColumnName = dt.Rows[i]["GuideName"].ToString();
					SearchTable.Columns.Add(SearchColumn);	
				}
			} 
			DataColumn TotalColumn = new DataColumn();                         //创建汇总列
			TotalColumn.ColumnName = "汇总";
			SearchTable.Columns.Add(TotalColumn);

			return SearchTable;
		}
 
		/// <summary>
		/// 通过时间说明创建时间汇总表
		/// </summary>
		/// <param name="StartDate">开始时间</param>
		/// <param name="EndDate">结束时间</param>
		/// <returns>以时间说明作为列名</returns>
        public static DataTable CreatDateCollectTableByDate(int StartDateDesc, int EndDateDesc)
        {
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            int a;
            int b;
            DataTable SearchTable = new DataTable();             //创建表对象

            DataColumn DeptColumn = new DataColumn();            //科室名称列
            DeptColumn.ColumnName = "科室";
            SearchTable.Columns.Add(DeptColumn);
            if (StartDateDesc > EndDateDesc)                        //ID号小的时间说明负值给a，ID号大的时间说明负值给b
            {
                a = EndDateDesc;
                b = StartDateDesc;
            }
            else
            {
                a = StartDateDesc;
                b = EndDateDesc;
            }
            DataTable dt = dal.GetDateDesc(a, b);       //获取时间说明列表
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataColumn SearchColumn = new DataColumn();     //以时间说明为列名添加列          
                SearchColumn.ColumnName = dt.Rows[i]["DateDesc"].ToString();
                SearchTable.Columns.Add(SearchColumn);
            }
            return SearchTable;
        }

		/// <summary>
		/// 填充指标汇总表
		/// </summary>
		/// <param name="StartDate">开始时间</param>
		/// <param name="EndDate">结束时间</param>
		/// <param name="DeptName">科室</param>
		/// <param name="GuideType">指标</param>
		/// <returns></returns>
        public static DataTable FillGuideCollectTable(DataTable dt, int StartDateDesc, int EndDateDesc, string DeptName, int GuideType, string datestr, string DeptFilter)
		{
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
			double Score = 0;                       //初始化每一个类别的分数为0.0
			string StrGuide;                          //表列名变量
			string GroupCode;                         //科室代号
			string GroupName;                         //科室名称
        	double TotalColumnValue=0;                //汇总列值
	                            
			int TableColCount = dt.Columns.Count-1;                     //要填充表的列数减一
            DataSet ds = GetGroupByType(DeptName, DeptFilter);

            int DeptNameCount = ds.Tables[0].Rows.Count;                //科室数量

            DataTable DTGuideScoreTable = guidedal.GuideScoreTable(datestr);     //获取某一段时间每个科室每个指标得分列表   
			string strtype=string.Format("select * from {0}.G_GUIDETYPE where TYPESIGN=1",DataUser.ZLGL);
			DataTable typetable=OracleOledbBase.ExecuteDataSet(strtype).Tables[0];
			
			for(int i=0;i<DeptNameCount;i++)                          //循环科室
			{
				TotalColumnValue=0;
				GroupCode = ds.Tables[0].Rows[i]["dept_code"].ToString();       //科室代号
				GroupName = ds.Tables[0].Rows[i]["dept_name"].ToString();       //科室名称
                
				DataRow myRow = dt.NewRow();                                     //行对象
				dt.Rows.Add(myRow);                                              //添加一个新行
				dt.Rows[dt.Rows.Count-1][0] = GroupName;                         //添加第一列

				for(int j=1;j<TableColCount;j++)                                 //循环填充各列
				{
					StrGuide = dt.Columns[j].ColumnName.ToString();              //列名
					try
					{
						if(GuideType==0)                                             //以指标类别为列名
						{
                            string str = DTGuideScoreTable.Compute("Sum(GuideSpareNum)", "DeptName='" + GroupCode + "' and GuideType='" + StrGuide + "'").ToString();
							if(str!="")
								Score = Convert.ToDouble(str);
							else
								Score=0;
						}
						else                                                         //以指标名称为列名
						{
                            string str = DTGuideScoreTable.Compute("Sum(GuideSpareNum)", "DeptName='" + GroupCode + "' and GuideName='" + StrGuide + "'").ToString();
							if(str!="")
								Score = Convert.ToDouble(str);
							else
								Score=0;
						}
						
						if(StrGuide==typetable.Rows[0]["GUIDETYPE"].ToString())
						{
                            int tabnums = 0;
							DataSet dsguide = dal.SpecialtyGuide_View();       
							for(int a=0;a<dsguide.Tables[0].Rows.Count;a++)           
							{
								string[] arrayCheckDept = BreakString.ShowText(dsguide.Tables[0].Rows[a]["CheckDept"].ToString()); 

                                //
                                string deptfilter = "";
                                string[] deptlist = dsguide.Tables[0].Rows[a]["CheckDept"].ToString().Split(',');
                                if (dsguide.Tables[0].Rows[a]["CheckDept"].ToString() != "")
                                {
                                    for (int m = 0; m < deptlist.Length; m++)
                                    {
                                        if (!deptlist[m].ToString().Equals(string.Empty))
                                            deptfilter = deptfilter + "'" + deptlist[m].ToString() + "',";
                                    }
                                    deptfilter = deptfilter.Substring(0, deptfilter.Length - 1);
                                    string str = string.Format(@"select * from {0}.sys_dept_dict where attr='是' and dept_code='{2}' and  dept_code in (
select account_dept_code from {0}.sys_dept_dict where dept_code in ({1}))", DataUser.COMM, deptfilter, GroupCode);
                                    DataTable tabnum = OracleOledbBase.ExecuteDataSet(str).Tables[0];
                                    tabnums += tabnum.Rows.Count;
                                }
							}
                            if (tabnums > 1 && Score != 0)
							{
                                Score = Score - double.Parse(typetable.Rows[0]["guidetypenum"].ToString()) * (tabnums - 1);
							}
						}
					}
					catch
					{
						Score=0;
					}
					dt.Rows[dt.Rows.Count-1][j] = Score.ToString();
					TotalColumnValue = TotalColumnValue + Score;
				}
                dt.Rows[dt.Rows.Count-1][TableColCount] = TotalColumnValue;      //填充汇总列
			}
			return dt;
		}
        public static string GetAccoutdept(string deptcode)
        {
            string str = string.Format("select account_dept_code from {0}.sys_dept_dict where dept_code='{1}'",DataUser.COMM,deptcode);
            return OracleOledbBase.ExecuteScalar(str).ToString() ;
        }
		public static DataTable FillGuideCollectTable_dept(DataTable dt,int StartDateDesc,int EndDateDesc,string DeptName,int GuideType)
		{
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
			double Score = 0;                       //初始化每一个类别的分数为0.0
			string StrGuide;                          //表列名变量
			string GroupCode;                         //科室代号
			string GroupName;                         //科室名称
			double TotalColumnValue=0;                //汇总列值
	                            
			int TableColCount = dt.Columns.Count-1;                     //要填充表的列数减一
			DataSet ds = GetGroupByType(DeptName,"");
			int DeptNameCount = ds.Tables[0].Rows.Count;                //科室数量

			DataTable DTGuideScoreTable = guidedal.GuideScoreTable(StartDateDesc,EndDateDesc);     //获取某一段时间每个科室每个指标得分列表   
			
			for(int i=0;i<DeptNameCount;i++)                          //循环科室
			{
				TotalColumnValue=0;
				GroupCode = ds.Tables[0].Rows[i]["group_code"].ToString();       //科室代号
				GroupName = ds.Tables[0].Rows[i]["group_name"].ToString();       //科室名称
				DataRow myRow = dt.NewRow();                                     //行对象
				dt.Rows.Add(myRow);                                              //添加一个新行
				dt.Rows[dt.Rows.Count-1][0] = GroupName;                         //添加第一列

				for(int j=1;j<TableColCount;j++)                                 //循环填充各列
				{
					StrGuide = dt.Columns[j].ColumnName.ToString();              //列名
                    try
                    {

                        string str = DTGuideScoreTable.Compute("Sum(GuideSpareNum)", "DeptName='" + GroupCode + "' and GuideType='" + StrGuide + "'").ToString();
                        if (str != "")
                            Score = Convert.ToDouble(str);
                        else
                            Score = 0;
                    }
                    catch
                    {
                        Score = 0;
                    }
					dt.Rows[dt.Rows.Count-1][j] = Score.ToString();
					TotalColumnValue = TotalColumnValue + Score;
				}
				dt.Rows[dt.Rows.Count-1][TableColCount] = TotalColumnValue;      //填充汇总列
			}
			return dt;
		}


	
		/// <summary>
		/// 填充时间汇总表
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="StartDate">开始时间</param>
		/// <param name="EndDate">结束时间</param>
		/// <param name="DeptName">所有科室</param>
		/// <param name="GuideType">所有指标</param>
		/// <returns></returns>
        public static DataTable FillDateCollectTable(DataTable dt, int StartDateDesc, int EndDateDesc, string DeptName, int GuideType,string deptfilter)
        {
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
            double Score = 0;                      //初始化分数为0.0
            string StrGuide;                          //表列名变量
            string StrDateDesc;
            int intDateDesc;
            string GroupCode;                        //科室代号
            string GroupName;                        //科室名称
            string strGuideType;
            DataSet ds = GetGroupByType(DeptName, deptfilter);                      //科室表
            int TableColCount = dt.Columns.Count;                       //表的列数
            DataTable DTGuideScoreTable = guidedal.GuideScoreTable(StartDateDesc, EndDateDesc);       //获取某一段时间每个科室每个指标得分列表
            int DeptNameCount = ds.Tables[0].Rows.Count;              //科室数量
            string strtype = string.Format("select * from {0}.G_GUIDETYPE where TYPESIGN=1", DataUser.ZLGL);
            DataTable typetable = OracleOledbBase.ExecuteDataSet(strtype).Tables[0];
            for (int i = 0; i < DeptNameCount; i++)                          //循环科室
            {
                GroupCode = ds.Tables[0].Rows[i]["dept_code"].ToString();       //科室代号
                GroupName = ds.Tables[0].Rows[i]["dept_name"].ToString();       //科室名称
                DataRow myRow = dt.NewRow();                                     //行对象
                dt.Rows.Add(myRow);                                              //添加一个新行
                dt.Rows[dt.Rows.Count - 1][0] = GroupName;                         //添加第一列

                for (int j = 1; j < TableColCount; j++)                                 //循环填充各列
                {
                    StrGuide = dt.Columns[j].ColumnName.ToString();              //列名
                    StrDateDesc = dt.Columns[j].ColumnName.ToString();
                    intDateDesc = guidedal.GetDateDescID(StrDateDesc);
                    try
                    {
                        if (GuideType == 0)                                         //全部指标
                        {
                            Score = Convert.ToDouble(DTGuideScoreTable.Compute("Sum(GuideSpareNum)", "DeptName='" + GroupCode + "' and DateDesc='" + StrDateDesc + "'"));
                        }
                        else                                                     //一个指标类别
                        {
                            strGuideType = dal.GetGuideTypeByID(GuideType).Rows[0]["GuideType"].ToString();
                            Score = Convert.ToDouble(DTGuideScoreTable.Compute("Sum(GuideSpareNum)", "DeptName='" + GroupCode + "' and DateDesc='" + StrDateDesc + "'and GuideType='" + strGuideType + "'"));
                        }
                        StringBuilder SpecGuideID = new StringBuilder();

                        //
                        int tabnums = 0;
                        DataSet dsguide = dal.SpecialtyGuide_View();
                        for (int a = 0; a < dsguide.Tables[0].Rows.Count; a++)
                        {
                            string[] arrayCheckDept = BreakString.ShowText(dsguide.Tables[0].Rows[a]["CheckDept"].ToString());

                            //
                            string deptfilter1 = "";
                            string[] deptlist = dsguide.Tables[0].Rows[a]["CheckDept"].ToString().Split(',');

                            for (int m = 0; m < deptlist.Length; m++)
                            {
                                if (!deptlist[m].ToString().Equals(string.Empty))
                                    deptfilter1 = deptfilter1 + "'" + deptlist[m].ToString() + "',";
                            }
                            deptfilter1 = deptfilter1.Substring(0, deptfilter1.Length - 1);
                            string str = string.Format(@"select * from {0}.sys_dept_dict where attr='是' and dept_code='{2}' and  dept_code in (
select account_dept_code from {0}.sys_dept_dict where dept_code in ({1}))", DataUser.COMM, deptfilter1, GroupCode);
                            DataTable tabnum = OracleOledbBase.ExecuteDataSet(str).Tables[0];
                            tabnums += tabnum.Rows.Count;
                        }
                        if (tabnums > 1 && Score != 0 && (GuideType == 0 || GuideType==2))
                        {
                            Score = Score - double.Parse(typetable.Rows[0]["guidetypenum"].ToString()) * (tabnums - 1);
                        }
                        //
                        string str111 = SpecGuideID.ToString();
                 
                    }
                    catch
                    {
                        Score = 0;
                    }
                    dt.Rows[dt.Rows.Count - 1][j] = Score;
                }
            }
            return dt;
        }
		#endregion

		#region      非公有方法
	
		/// <summary>
		/// 通过科室类别获取科室列表getalldeptgroupdict
		/// </summary>
		/// <param name="DeptType">科室类别</param>
		/// <returns></returns>
        private static DataSet GetGroupByType(string DeptName, string DeptFilter)
		{
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataSet ds; 
			switch(DeptName)
			{
				case "":
                    ds = dal.GetAAccountDeptfilter(DeptFilter);
					break;

				default:
                    ds = dal.GetAAccountDept(DeptName);
					break;

			}
			return ds;
		}
		#endregion

		#region  属性
		/// <summary>
		/// 表中列的数量
		/// </summary>
		public int TableColCount
		{
			get
			{
				return CountCol;
			}
		}
		#endregion

	}

	public class privateTab1
	{
        /// <summary>
        /// 后添加的方法,用来验证质量数据
        /// </summary>
        public static void verifyGuideData(int year, int month, DateTime startDate, DateTime endDate)
        {
            StringBuilder builder = new StringBuilder();

            // 2. 质量数据验证(SSJ)
            string qualityGuideValStr = QualityGuide.CheckAllGroupScore().Replace(",", "<br>");
            if (qualityGuideValStr != "")
                builder.AppendFormat("<b>质量指标数据验证出现如下错误:</b><br>{0}", qualityGuideValStr);
             
            string qualityGuideValStr2 = QualityGuide.CheckTempletIsExisted().Replace(",", "<br>");
            if (qualityGuideValStr2 != "")
                builder.AppendFormat("<b>质量指标对应模板数据出现如下错误:</b><br>{0}", qualityGuideValStr2);

            string totalValStr = builder.ToString();
            if (totalValStr != "")
                throw new QualityVerifyDataException(totalValStr);
        }
        public static void prepareData(int year, int month, DateTime startDate, DateTime endDate, OleDbTransaction trans)
        {
            try
            {
                // 质量得分数据（SSJ）
                QualityGuide.BuildGuideScore(year.ToString() + "-" + month.ToString(), startDate, endDate, 1, trans);
            }
            catch (Exception ex)
            {
                throw new BonusPrepareDataException("数据准备异常" + ex.Message, ex);
            }
        }

		
	}
    public class Staff
    {
        public static IStaffTargetInfo GetStaff()
        {
            Object staff = HttpContext.Current.Session["CURRENTSTAFF"];

            User user = (User)(staff);
            GoldNet.JXKP.BLL.Organise.Staff curuser = new GoldNet.JXKP.BLL.Organise.Staff(user.UserId);

            return (IStaffTargetInfo)curuser;
        }
    }
}
