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
	/// ָ����
	/// </summary>
	public class QualityGuide
	{
       public static  Goldnet.Dal.ZLGL_Guide_Dict dal = new Goldnet.Dal.ZLGL_Guide_Dict();
		#region   ˽�б���
        private GuideInfo _guideInfo;

		#endregion

		#region   �ڲ�ʵ��  
		public class GuideInfo
		{
			public int ID;                    //ָ������ID
			public string GuideName;          //ָ������
			public int GuideTypeID;           //ָ�����ID
			public string GuideType;          //ָ�����
			public string TypeSign;           //ָ������־  0Ϊ����ָ�꣬1Ϊרҵָ��
			public double GuideNum;           //ָ���ֵ
			public int TempletID;             //ģ����ID
			public int DateCol;               //ʱ����ID
			public int TargetCol;             //������ID
			public int GuideNameCol;          //����������ID
			public int GuideNameColValue;     //����ֵ��ID

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

		#region  ���캯��
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

		#region  �ⲿ����
		/// <summary>
		/// ��ȡ����ָ��
		/// </summary>
		/// <returns>DataSet</returns>
		public static DataSet GetAllGuide()
		{
           return dal.GetAllGuide();
		}

		/// <summary>
		/// ��ȡ����ʱ��˵��
		/// </summary>
		/// <returns></returns>
		public static DataSet GetAllDateDesc()
		{
            return dal.GetAllDateDesc();
		}

		/// <summary>
		/// ��ȡĳһָ�����ƵĿ�������
		/// </summary>
		/// <param name="guideName">ָ������</param>
		/// <returns>DataSet</returns>
		public static DataSet GetGuideContent(string guideName)
		{
			QualityGuide guide = new QualityGuide(guideName);                        //��ʼ��ָ�����

			int guideNameID = guide.ID;                                             //����ָ������ID
			int guideTypeID = guide.GuideTypeID;                                    //����ָ�����ID
            return dal.GetGuideContent(guideNameID,guideTypeID);
		}
		//
		/// <summary>
		/// ���ҿ���ָ��
		/// </summary>
		/// <param name="guideName"></param>
		/// <param name="deptcode"></param>
		/// <returns></returns>
		public static DataSet GetGuideContentdept(string guideName,string deptcode)
		{
			QualityGuide guide = new QualityGuide(guideName);                        //��ʼ��ָ�����

			int guideNameID = guide.ID;                                             //����ָ������ID
			int guideTypeID = guide.GuideTypeID;                                    //����ָ�����ID

            return dal.GetGuideContentdept(guideNameID,guideTypeID,deptcode);
		}

		public static DataSet GetGuidesByGroupdept(string GroupCode)
		{
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            return dal.GetGuidesByGroupdept(GroupCode);
		}

		/// <summary>
		/// �������п��ҵ�ָ���ܷ�
		/// </summary>
		/// <returns>����ָ���ֲܷ�Ϊ100�ֵĿ���</returns>
		public static string CheckAllGroupScore()
		{

            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
			string GroupGuideTotalScore = "";
            if (GetConfig.GetConfigString("CheckScore") != "0")
            {
                DataTable table = dal.GetAAccountDept("").Tables[0];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    double GroupGuideScore = CheckGroupScore(table.Rows[i]["dept_code"].ToString());   //ȡ��ÿ�����ҵ�ָ���ܷ�
                    if (GroupGuideScore != Convert.ToInt32(OracleOledbBase.CountNumber))                                                                                 //��¼ָ���ֲܷ�Ϊ100�ֵĿ��Ҽ��÷�
                        GroupGuideTotalScore = GroupGuideTotalScore + table.Rows[i]["dept_name"].ToString() + "ָ���ܷ�Ϊ" + GroupGuideScore.ToString() + "��,";
                }
            }
			return GroupGuideTotalScore;
		}

		/// <summary>
		/// ����ÿ��ָ���ģ�塢ʱ���С������С���ֵ��......�Ƿ����
		/// </summary>
		/// <returns>������ģ�塢ʱ���С������С���ֵ��......����Ӧ��Ϣ</returns>
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
				//ģ����
                if (table.Rows[i]["TempletID"].ToString() != "")
				{
                    TempletID = Convert.ToInt32(table.Rows[i]["TempletID"].ToString());
					try
					{
						new TempletBO(TempletID);
					}
					catch(TempletIDNotExistedException)
					{
                        TempletCol = TempletCol + table.Rows[i]["GuideName"] + "ָ���Ӧ��ģ�岻����,";
					}                  
				}

				else
				{
                    TempletCol = TempletCol + table.Rows[i]["GuideName"] + "ָ��û�ж�Ӧ��ģ��,";
				}

				//ʱ����
                if (table.Rows[i]["DateCol"].ToString() != "")
				{
                    DateCol = Convert.ToInt32(table.Rows[i]["DateCol"].ToString());
					try
					{
						new Field(DateCol);
					}
					catch
					{
                        TempletCol = TempletCol + table.Rows[i]["GuideName"] + "ָ���Ӧ��ʱ���в�����,";
					}
				}
				else
				{
                    TempletCol = TempletCol + table.Rows[i]["GuideName"] + "ָ��û�ж�Ӧ��ʱ����,";
				}

				//������
                if (table.Rows[i]["TargetCol"].ToString() != "")
				{
                    TargetCol = Convert.ToInt32(table.Rows[i]["TargetCol"].ToString());
					try
					{
						new Field(TargetCol);
					}
					catch
					{
                        TempletCol = TempletCol + table.Rows[i]["GuideName"] + "ָ���Ӧ�Ŀ����в�����,";
					}
				}
				else
				{
                    TempletCol = TempletCol + table.Rows[i]["GuideName"] + "ָ��û�ж�Ӧ�Ŀ�����,";
				}


				//������ֵ��
                if (table.Rows[i]["GuideNameColValue"].ToString() != "")
				{
                    GuideNameColValue = Convert.ToInt32(table.Rows[i]["GuideNameColValue"].ToString());
					try
					{
						new Field(GuideNameColValue);
					}
					catch
					{
                        TempletCol = TempletCol + table.Rows[i]["GuideName"] + "ָ���Ӧ����ֵ�в�����,";
					}
				}
				else
				{
                    TempletCol = TempletCol + table.Rows[i]["GuideName"] + "ָ��û�ж�Ӧ����ֵ��,";
				}
			}
			return TempletCol;
		}

		/// <summary>
		/// ���ɿ����������ݷ���
		/// </summary>
		/// <param name="DateDesc">ʱ��˵��</param>
		/// <param name="StartDate">��ʼʱ��</param>
		/// <param name="EndDate">����ʱ��</param>
		/// <param name="CheckNum">ϵ��</param>
		/// <returns></returns>
        public static void BuildGuideScore(string DateDesc, DateTime StartDate, DateTime EndDate, int CheckNum, OleDbTransaction trans)
		{
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
            if (guidedal.IsExistCheckCollect(DateDesc) == true)               //��ʱ��˵���Ŀ������������Ƿ���ڣ�������ڵ���ɾ������
			{
               guidedal.DelCheckCollect(DateDesc, trans);
			}

            int PrimaryID = Convert.ToInt32(guidedal.AddPrimary(DateDesc, StartDate, EndDate, CheckNum, trans).ToString());       //����������Ϣ      
			DataSet ds = dal.GetAAccountDept("");                 //���п���   
            int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.G_CollectScoreSecondary", DataUser.ZLGL));                         
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)        //ѭ������
			{
				string GroupCode = ds.Tables[0].Rows[i]["dept_code"].ToString();         //���Ҵ���
				DataTable dt = GetGuidesByGroupdept(GroupCode).Tables[0];                     //ĳһ��������ָ��
				for(int j=0;j<dt.Rows.Count;j++)                                          //ѭ��ָ��
				{
					
					string GuideName = dt.Rows[j]["GuideName"].ToString();
					string GuideType = dt.Rows[j]["GuideType"].ToString();
					double GuideNum = Convert.ToDouble(dt.Rows[j]["GuideNum"].ToString())*CheckNum;
                    string TYPESIGN = dt.Rows[j]["TYPESIGN"].ToString();//רҵ����

					QualityGuide guide = new QualityGuide(GuideName);              //��ʼ��ָ�����

					int TempletID = Convert.ToInt32(guide.TempletID.ToString());    //ģ��ID
					int DateCol = Convert.ToInt32(guide.DateCol.ToString());        //ʱ����ID
					int TargetCol = Convert.ToInt32(guide.TargetCol.ToString());    //������ID
					int GuideNameColValue = Convert.ToInt32(guide.GuideNameColValue.ToString());   //��ֵ��ID

					TempletBO templet = new TempletBO(TempletID);                  //��ʼ��ģ�����
					
					try
					{
						double GuideCutScore = Convert.ToDouble(templet.GetCountValue(DateCol,TargetCol,GuideNameColValue,StartDate,EndDate,GroupCode));   //��ȡָ��ʵ�ʿ۷�
						double GuideSpareScore =CountDeptScore(GuideName,GuideCutScore,CheckNum);       //����ָ������÷�
                       
						bool AddDeptScore =guidedal.addDeptScore(id,GroupCode,GuideType,GuideName,GuideNum,GuideCutScore,GuideSpareScore,PrimaryID,TYPESIGN, trans);	//���ӿ�����������
                        id++;
					}

					catch(TempletException ex)
					{
						throw new GlobalException("����������������ʱ�����쳣", "������ϢΪ��", ex);
					}
					catch(Exception ex)
					{
						throw new GlobalException("����������������ʱ�����쳣", "������ϢΪ��", ex);
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
		/// ��ʱ���ѯ��������
		/// </summary>
		/// <param name="DateDesc">ʱ��˵��</param>
		/// <param name="DeptName">����</param>
		/// <param name="GuideType">ָ�����</param>
		/// <returns></returns>
        public static DataTable QualitySearchByDate(int DateDesc, string DeptName, int GuideType, string year, string month, string DeptFilter)
		{
            string datestr = year + "-" + month;
			DataTable dt = null;                  //������
            dt = GuideTable.CreatGuideCollectTableByGuide(GuideType);                  //������
            dt = GuideTable.FillGuideCollectTable(dt, DateDesc, DateDesc, DeptName, GuideType, datestr, DeptFilter);      //����

			return dt;
		}
        public static DataTable QualitySearchlist(string stardate, string enddate)
        {
            string types = "select * from zlgl.G_SPECIALTYGUIDE";
            DataTable table = OracleOledbBase.ExecuteDataSet(types).Tables[0];
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select b.dept_code,b.dept_name \"����\"");
            double numbers = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                numbers += double.Parse(table.Rows[i]["GUIDENUM"].ToString());
                str.AppendFormat(" ,{1}+SUM (DECODE (a.TEMPLET_ID, '{0}', a.NUMBERS, 0)) \"{0}\"", table.Rows[i]["TEMPLETID"].ToString(), table.Rows[i]["GUIDENUM"].ToString());
               
            }
            str.AppendFormat(" ,sum(a.numbers)+{0} \"�ϼ�\"",numbers);
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
		/// ��ָ���ѯ��������
		/// </summary>
		/// <param name="StartDateDesc">��ʼʱ��</param>
		/// <param name="EndDateDesc">����ʱ��</param>
		/// <param name="DeptName">����</param>
		/// <param name="GuideType">ָ�����</param>
		/// <returns></returns>
        public static DataTable QualitySearchByGuide(int StartDateDesc, int EndDateDesc, string DeptName, int GuideType,string deptfilter)
        {
            DataTable dt = GuideTable.CreatDateCollectTableByDate(StartDateDesc, EndDateDesc);    //������

            dt = GuideTable.FillDateCollectTable(dt, StartDateDesc, EndDateDesc, DeptName, GuideType,deptfilter);                         //����                   

            return dt;
        }

       

		#endregion

		#region  �ǹ��з���
		/// <summary>
		/// ����ڲ���VO
		/// </summary>
		/// <param name="guideName">ָ������</param>
		/// <returns>����ָ�����Ƶ�ָ���ID��ָ�����͵�ID......</returns>
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
		/// ����ĳһ���ҵ�ָ���ܷ�
		/// </summary>
		/// <returns>���ؿ��ҵ�ָ���ܷ�</returns>
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
		/// ���ݿ۷���ĳһָ������÷�
		/// </summary>
		/// <param name="GuideName">ָ������</param>
		/// <param name="GuideCutNum">ʵ�ʿ۷�</param>
		/// <param name="CheckNum">ϵ��</param>
		/// <returns>���÷�</returns>
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

		#region  ����
		/// <summary>
		/// ָ������ID
		/// </summary>
		public int ID
		{
			get
			{
				return _guideInfo.ID;
			}
		}

		/// <summary>
		/// ָ������
		/// </summary>
		public string GuideName
		{
			get
			{
				return _guideInfo.GuideName;
			}
		}

		/// <summary>
		/// ָ�����ID
		/// </summary>
		public int GuideTypeID
		{
			get
			{
				return _guideInfo.GuideTypeID;
			}
		}

		/// <summary>
		/// ָ�����
		/// </summary>
		public string GuideType
		{
			get
			{
				return _guideInfo.GuideType;
			}
		}

		/// <summary>
		/// ָ������־
		/// </summary>
		public string TypeSign
		{
			get
			{
				return _guideInfo.TypeSign;
			}
		}

		/// <summary>
		/// ָ���ֵ
		/// </summary>
		public double GuideNum
		{
			get
			{
				return _guideInfo.GuideNum;
			}
		}

		/// <summary>
		/// ģ����ID
		/// </summary>
		public int TempletID
		{
			get
			{
				return _guideInfo.TempletID;
			}
		}

		/// <summary>
		/// ʱ����ID
		/// </summary>
		public int DateCol
		{
			get
			{
				return _guideInfo.DateCol;
			}
		}

		/// <summary>
		/// ������ID
		/// </summary>
		public int TargetCol
		{
			get
			{
				return _guideInfo.TargetCol;
			}
		}

		/// <summary>
		/// ����������ID
		/// </summary>
		public int GuideNameCol
		{
			get
			{
				return _guideInfo.GuideNameCol;
			}
		}

		/// <summary>
		/// ����ֵ��ID
		/// </summary>
		public int GuideNameColValue
		{
			get
			{
				return _guideInfo.GuideNameColValue;
			}
		}

		/// <summary>
		/// ѡ��ֵID
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
	/// ��̬�б��
	/// </summary>
	public class GuideTable
	{

        #region   ˽�б���
		private int CountCol;
		#endregion

		#region ���캯��
		public GuideTable(int GuideType)
		{
			CountCol = CreatGuideCollectTableByGuide(GuideType).Columns.Count;
		}

		public GuideTable(int StartDateDesc,int EndDateDesc)
		{
			CountCol = CreatDateCollectTableByDate(StartDateDesc,EndDateDesc).Columns.Count;
		}
		#endregion

		#region  ���з���
		/// <summary>
		/// ͨ��ָ����𴴽�ָ����ܱ�
		/// </summary>
		/// <param name="GuideType">ָ�����</param>
		/// <returns>��ָ��������ָ��������Ϊ����</returns>
        public static DataTable CreatGuideCollectTableByGuide(int GuideType)
        {
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
            string TypeSign = "";
            if (GuideType != 0)
                TypeSign = dal.GetGuideTypeByID(GuideType).Rows[0]["TypeSign"].ToString();     //��ȡĳһ���ID������־
            DataTable SearchTable = new DataTable();                             //���������

            DataColumn DeptColumn = new DataColumn();                            //��������������	
            if (TypeSign == "9")//���ҿ���
            {
                DeptColumn.ColumnName = "����";
            }
            else
            {
                DeptColumn.ColumnName = "����";
            }
            SearchTable.Columns.Add(DeptColumn);

            if (GuideType == 0)                                                    //ȫ��ָ��
            {
                DataTable dt =guidedal.Guide_Type_Dict().Tables[0];              //����ָ�����  
                for (int i = 0; i < dt.Rows.Count; i++)                                 //��ָ���������Ϊ���������
                {
                    //DataColumn SearchColumn = new DataColumn();
                    //SearchColumn.ColumnName = dt.Rows[i]["GuideType"].ToString();
                    //SearchTable.Columns.Add(SearchColumn);
                    SearchTable.Columns.Add(dt.Rows[i]["GuideType"].ToString(), typeof(decimal));
                }
            }
            else                                                                //����ָ��
            {

                DataTable dt = dal.Guide_Name_Dict(TypeSign, GuideType).Tables[0];                     //һ��ָ������µ�ȫ��ָ��
                for (int i = 0; i < dt.Rows.Count; i++)                                                          //��ָ������Ϊ���������                       
                {
                    //DataColumn SearchColumn = new DataColumn();
                    //SearchColumn.ColumnName = dt.Rows[i]["GuideName"].ToString();
                    //SearchTable.Columns.Add(SearchColumn);
                    SearchTable.Columns.Add(dt.Rows[i]["GuideName"].ToString(), typeof(decimal));
                }
            }
            DataColumn TotalColumn = new DataColumn();                         //����������
            TotalColumn.ColumnName = "����";
            SearchTable.Columns.Add(TotalColumn);

            return SearchTable;
        }

       //����ָ�����
		public static DataTable CreatGuideCollectTableByGuidedept(int GuideType)
		{
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
			string TypeSign="";
			if(GuideType!=0)
				TypeSign = dal.GetGuideTypeByID(GuideType).Rows[0]["TypeSign"].ToString();     //��ȡĳһ���ID������־
			DataTable SearchTable = new DataTable();                             //���������

			DataColumn DeptColumn = new DataColumn();                            //��������������	
			DeptColumn.ColumnName = "����";
			SearchTable.Columns.Add(DeptColumn);

			if(GuideType== 0)                                                    //ȫ��ָ��
			{
				DataTable dt = guidedal.Guide_Type_Dict().Tables[0];              //����ָ�����  
				for(int i=0;i<dt.Rows.Count;i++)                                 //��ָ���������Ϊ���������
				{
					DataColumn SearchColumn = new DataColumn();
					SearchColumn.ColumnName = dt.Rows[i]["GuideType"].ToString(); 			    
					SearchTable.Columns.Add(SearchColumn);
				}					
			}
			else                                                                //����ָ��
			{
				
				DataTable dt = dal.Guide_Name_Dict(TypeSign,GuideType).Tables[0];                     //һ��ָ������µ�ȫ��ָ��
				for(int i=0;i<dt.Rows.Count;i++)                                                          //��ָ������Ϊ���������                       
				{
					DataColumn SearchColumn = new DataColumn(); 
					SearchColumn.ColumnName = dt.Rows[i]["GuideName"].ToString();
					SearchTable.Columns.Add(SearchColumn);	
				}
			} 
			DataColumn TotalColumn = new DataColumn();                         //����������
			TotalColumn.ColumnName = "����";
			SearchTable.Columns.Add(TotalColumn);

			return SearchTable;
		}
 
		/// <summary>
		/// ͨ��ʱ��˵������ʱ����ܱ�
		/// </summary>
		/// <param name="StartDate">��ʼʱ��</param>
		/// <param name="EndDate">����ʱ��</param>
		/// <returns>��ʱ��˵����Ϊ����</returns>
        public static DataTable CreatDateCollectTableByDate(int StartDateDesc, int EndDateDesc)
        {
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            int a;
            int b;
            DataTable SearchTable = new DataTable();             //���������

            DataColumn DeptColumn = new DataColumn();            //����������
            DeptColumn.ColumnName = "����";
            SearchTable.Columns.Add(DeptColumn);
            if (StartDateDesc > EndDateDesc)                        //ID��С��ʱ��˵����ֵ��a��ID�Ŵ��ʱ��˵����ֵ��b
            {
                a = EndDateDesc;
                b = StartDateDesc;
            }
            else
            {
                a = StartDateDesc;
                b = EndDateDesc;
            }
            DataTable dt = dal.GetDateDesc(a, b);       //��ȡʱ��˵���б�
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataColumn SearchColumn = new DataColumn();     //��ʱ��˵��Ϊ���������          
                SearchColumn.ColumnName = dt.Rows[i]["DateDesc"].ToString();
                SearchTable.Columns.Add(SearchColumn);
            }
            return SearchTable;
        }

		/// <summary>
		/// ���ָ����ܱ�
		/// </summary>
		/// <param name="StartDate">��ʼʱ��</param>
		/// <param name="EndDate">����ʱ��</param>
		/// <param name="DeptName">����</param>
		/// <param name="GuideType">ָ��</param>
		/// <returns></returns>
        public static DataTable FillGuideCollectTable(DataTable dt, int StartDateDesc, int EndDateDesc, string DeptName, int GuideType, string datestr, string DeptFilter)
		{
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
			double Score = 0;                       //��ʼ��ÿһ�����ķ���Ϊ0.0
			string StrGuide;                          //����������
			string GroupCode;                         //���Ҵ���
			string GroupName;                         //��������
        	double TotalColumnValue=0;                //������ֵ
	                            
			int TableColCount = dt.Columns.Count-1;                     //Ҫ�����������һ
            DataSet ds = GetGroupByType(DeptName, DeptFilter);

            int DeptNameCount = ds.Tables[0].Rows.Count;                //��������

            DataTable DTGuideScoreTable = guidedal.GuideScoreTable(datestr);     //��ȡĳһ��ʱ��ÿ������ÿ��ָ��÷��б�   
			string strtype=string.Format("select * from {0}.G_GUIDETYPE where TYPESIGN=1",DataUser.ZLGL);
			DataTable typetable=OracleOledbBase.ExecuteDataSet(strtype).Tables[0];
			
			for(int i=0;i<DeptNameCount;i++)                          //ѭ������
			{
				TotalColumnValue=0;
				GroupCode = ds.Tables[0].Rows[i]["dept_code"].ToString();       //���Ҵ���
				GroupName = ds.Tables[0].Rows[i]["dept_name"].ToString();       //��������
                
				DataRow myRow = dt.NewRow();                                     //�ж���
				dt.Rows.Add(myRow);                                              //���һ������
				dt.Rows[dt.Rows.Count-1][0] = GroupName;                         //��ӵ�һ��

				for(int j=1;j<TableColCount;j++)                                 //ѭ��������
				{
					StrGuide = dt.Columns[j].ColumnName.ToString();              //����
					try
					{
						if(GuideType==0)                                             //��ָ�����Ϊ����
						{
                            string str = DTGuideScoreTable.Compute("Sum(GuideSpareNum)", "DeptName='" + GroupCode + "' and GuideType='" + StrGuide + "'").ToString();
							if(str!="")
								Score = Convert.ToDouble(str);
							else
								Score=0;
						}
						else                                                         //��ָ������Ϊ����
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
                                    string str = string.Format(@"select * from {0}.sys_dept_dict where attr='��' and dept_code='{2}' and  dept_code in (
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
                dt.Rows[dt.Rows.Count-1][TableColCount] = TotalColumnValue;      //��������
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
			double Score = 0;                       //��ʼ��ÿһ�����ķ���Ϊ0.0
			string StrGuide;                          //����������
			string GroupCode;                         //���Ҵ���
			string GroupName;                         //��������
			double TotalColumnValue=0;                //������ֵ
	                            
			int TableColCount = dt.Columns.Count-1;                     //Ҫ�����������һ
			DataSet ds = GetGroupByType(DeptName,"");
			int DeptNameCount = ds.Tables[0].Rows.Count;                //��������

			DataTable DTGuideScoreTable = guidedal.GuideScoreTable(StartDateDesc,EndDateDesc);     //��ȡĳһ��ʱ��ÿ������ÿ��ָ��÷��б�   
			
			for(int i=0;i<DeptNameCount;i++)                          //ѭ������
			{
				TotalColumnValue=0;
				GroupCode = ds.Tables[0].Rows[i]["group_code"].ToString();       //���Ҵ���
				GroupName = ds.Tables[0].Rows[i]["group_name"].ToString();       //��������
				DataRow myRow = dt.NewRow();                                     //�ж���
				dt.Rows.Add(myRow);                                              //���һ������
				dt.Rows[dt.Rows.Count-1][0] = GroupName;                         //��ӵ�һ��

				for(int j=1;j<TableColCount;j++)                                 //ѭ��������
				{
					StrGuide = dt.Columns[j].ColumnName.ToString();              //����
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
				dt.Rows[dt.Rows.Count-1][TableColCount] = TotalColumnValue;      //��������
			}
			return dt;
		}


	
		/// <summary>
		/// ���ʱ����ܱ�
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="StartDate">��ʼʱ��</param>
		/// <param name="EndDate">����ʱ��</param>
		/// <param name="DeptName">���п���</param>
		/// <param name="GuideType">����ָ��</param>
		/// <returns></returns>
        public static DataTable FillDateCollectTable(DataTable dt, int StartDateDesc, int EndDateDesc, string DeptName, int GuideType,string deptfilter)
        {
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
            double Score = 0;                      //��ʼ������Ϊ0.0
            string StrGuide;                          //����������
            string StrDateDesc;
            int intDateDesc;
            string GroupCode;                        //���Ҵ���
            string GroupName;                        //��������
            string strGuideType;
            DataSet ds = GetGroupByType(DeptName, deptfilter);                      //���ұ�
            int TableColCount = dt.Columns.Count;                       //�������
            DataTable DTGuideScoreTable = guidedal.GuideScoreTable(StartDateDesc, EndDateDesc);       //��ȡĳһ��ʱ��ÿ������ÿ��ָ��÷��б�
            int DeptNameCount = ds.Tables[0].Rows.Count;              //��������
            string strtype = string.Format("select * from {0}.G_GUIDETYPE where TYPESIGN=1", DataUser.ZLGL);
            DataTable typetable = OracleOledbBase.ExecuteDataSet(strtype).Tables[0];
            for (int i = 0; i < DeptNameCount; i++)                          //ѭ������
            {
                GroupCode = ds.Tables[0].Rows[i]["dept_code"].ToString();       //���Ҵ���
                GroupName = ds.Tables[0].Rows[i]["dept_name"].ToString();       //��������
                DataRow myRow = dt.NewRow();                                     //�ж���
                dt.Rows.Add(myRow);                                              //���һ������
                dt.Rows[dt.Rows.Count - 1][0] = GroupName;                         //��ӵ�һ��

                for (int j = 1; j < TableColCount; j++)                                 //ѭ��������
                {
                    StrGuide = dt.Columns[j].ColumnName.ToString();              //����
                    StrDateDesc = dt.Columns[j].ColumnName.ToString();
                    intDateDesc = guidedal.GetDateDescID(StrDateDesc);
                    try
                    {
                        if (GuideType == 0)                                         //ȫ��ָ��
                        {
                            Score = Convert.ToDouble(DTGuideScoreTable.Compute("Sum(GuideSpareNum)", "DeptName='" + GroupCode + "' and DateDesc='" + StrDateDesc + "'"));
                        }
                        else                                                     //һ��ָ�����
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
                            string str = string.Format(@"select * from {0}.sys_dept_dict where attr='��' and dept_code='{2}' and  dept_code in (
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

		#region      �ǹ��з���
	
		/// <summary>
		/// ͨ����������ȡ�����б�getalldeptgroupdict
		/// </summary>
		/// <param name="DeptType">�������</param>
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

		#region  ����
		/// <summary>
		/// �����е�����
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
        /// ����ӵķ���,������֤��������
        /// </summary>
        public static void verifyGuideData(int year, int month, DateTime startDate, DateTime endDate)
        {
            StringBuilder builder = new StringBuilder();

            // 2. ����������֤(SSJ)
            string qualityGuideValStr = QualityGuide.CheckAllGroupScore().Replace(",", "<br>");
            if (qualityGuideValStr != "")
                builder.AppendFormat("<b>����ָ��������֤�������´���:</b><br>{0}", qualityGuideValStr);
             
            string qualityGuideValStr2 = QualityGuide.CheckTempletIsExisted().Replace(",", "<br>");
            if (qualityGuideValStr2 != "")
                builder.AppendFormat("<b>����ָ���Ӧģ�����ݳ������´���:</b><br>{0}", qualityGuideValStr2);

            string totalValStr = builder.ToString();
            if (totalValStr != "")
                throw new QualityVerifyDataException(totalValStr);
        }
        public static void prepareData(int year, int month, DateTime startDate, DateTime endDate, OleDbTransaction trans)
        {
            try
            {
                // �����÷����ݣ�SSJ��
                QualityGuide.BuildGuideScore(year.ToString() + "-" + month.ToString(), startDate, endDate, 1, trans);
            }
            catch (Exception ex)
            {
                throw new BonusPrepareDataException("����׼���쳣" + ex.Message, ex);
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
