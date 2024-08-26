using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using GoldNet.JXKP.PowerManager;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;
using System.Configuration;
using System.Text;
using Goldnet.Dal;
using GoldNet.Model;


namespace GoldNet.JXKP.Templet.BLL
{
	/// <summary>
	/// ģ���BO
	/// </summary>
	[Serializable()]
	public class TempletBO
	{
		#region --��������-- 
        public static string CONNECT_STRING = ConfigurationSettings.AppSettings["OledbConnString"];
        /// <summary>
        /// �ı����ͱ��
        /// </summary>
        public static int CHARFIELDTYPEID = 1;
        /// <summary>
        /// ��ֵ���ͱ��
        /// </summary>
        public static int NUMBERFIELDTYPEID = 2;
        /// <summary>
        /// ѡ�����ͱ��
        /// </summary>
        public static int SELECTFIELDTYPID = 3;
        /// <summary>
        /// ʱ�����ͱ��
        /// </summary>
        public static int DATEFIELDTYPEID = 4;
        /// <summary>
        /// ָ�����ͱ��
        /// </summary>
        public static int GUIDEFIELDTYPEID = 5;
        /// <summary>
        /// ��Ա���ͱ��
        /// </summary>
        public static int STAFFFIELDTYPEID = 6;
        /// <summary>
        /// �������ͱ��
        /// </summary>
        public static int DEPTFIELDTYPEID = 7;
        /// <summary>
        /// �������ͱ��
        /// </summary>
        public static int PATIENTFIELDTYPEID = 8;
        /// <summary>
        /// �������ͱ��
        /// </summary>
        public static int CHARMODIFYFIELTYPEID = 9;

       

		// ��ӹ���Ȩ����ص��ı�
		private const string TEMPLET_FUNC_TITLE = "ģ�壺";
		
		// ������Ϣ
		private const string ADD_TEMPLET_ERROR = "���ģ��ʱ����������";
		private const string ADD_TEMPLET_ERROR_ON_POWER = "��ϵͳ��ע�Ṧ��Ȩ��ʱ�������󡣴������ϸ��Ϣ��";
		private const string ADD_TEMPLET_ERROR_ON_DATABASE = "�����ݿ����������ʱ�������󡣴������ϸ��Ϣ��";

		#endregion

		#region --˽�б�������-- 
		private TempletInfo _templetInfo;

		private ListView _defaultView;

		private FieldCollection _fields;
		#endregion

		#region --�ڲ�ʵ����VO-- 
		/// <summary>
		/// ģ��ʵ����
		/// </summary>
		[Serializable()]
		internal class TempletInfo
		{
			public int ID;
			public string Name;
			public string Title;
			public string TabName;
			public string Common;
			public DateTime	CreateDate;
			public int DefaultViewID;
            public string FuncKey;
			public bool Deleted;
            public int showorder;
            public string mark;
			
		}
		#endregion

		#region --���캯��-- 
		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="templetID">ģ����</param>
		/// <exception cref="TempletIDNotExistedException">ָ����ָ���Ų�����!</exception>
		public TempletBO(int templetID)
		{
			// ����ģ���ų�ʼ��һ��ģ��, ����:
			// ��ѯ��ģ�����Ƿ����
			// ����VO,������Ϊ˽�б���.
			// �ֶ�? ��ͼ? ������Ϣ?

			string sql; 

			#region 
			sql = string.Format("SELECT ID, Name, Title, TabName, Common, CreateDate, DefaultViewID,FuncKey,showorder,mark FROM {0}.T_TempletDict WHERE (id = ?) AND (Deleted = 0)",DataUser.ZLGL);
			System.Data.DataTable reader = OracleOledbBase.ExecuteDataSet(sql, new OleDbParameter("id", templetID)).Tables[0];
			for (int i=0;i<reader.Rows.Count;i++)
			{
				_templetInfo = new TempletInfo();
				_templetInfo.ID = Convert.ToInt32(reader.Rows[i]["ID"].ToString());
				_templetInfo.Name = reader.Rows[i]["Name"].ToString();
				_templetInfo.Title = reader.Rows[i]["Title"].ToString();
				_templetInfo.TabName = reader.Rows[i]["TabName"].ToString();
				_templetInfo.Common = reader.Rows[i]["Common"].ToString();
				_templetInfo.CreateDate =Convert.ToDateTime(reader.Rows[i]["CreateDate"].ToString());
				_templetInfo.DefaultViewID = Convert.ToInt32(reader.Rows[i]["DefaultViewID"].ToString());
				_templetInfo.FuncKey = reader.Rows[i]["FuncKey"].ToString();
                _templetInfo.showorder = int.Parse(reader.Rows[i]["showorder"].ToString());
                _templetInfo.mark = reader.Rows[i]["mark"].ToString();
				
			}
			#endregion

		}
		#endregion

		#region --����-- 
		/// <summary>
		/// ģ����
		/// </summary>
		public int ID
		{
			get
			{
				return _templetInfo.ID;
			}
		}

		/// <summary>
		/// ģ������
		/// </summary>
		public string Name
		{
			get
			{
				return _templetInfo.Name;
			}
		}

		/// <summary>
		/// ģ�����
		/// </summary>
		public string Title
		{
			get
			{
				return _templetInfo.Title;
			}
		}

		/// <summary>
		/// ģ�����
		/// </summary>
		public string TabName
		{
			get
			{
				return _templetInfo.TabName;
			}
		}
		/// <summary>
		/// ˵��
		/// </summary>
		public string Common
		{
			get
			{
				return _templetInfo.Common;
			}
		}

		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime CreateDate
		{
			get
			{
				return _templetInfo.CreateDate;
			}
		}

		/// <summary>
		/// Ĭ����ͼ���
		/// </summary>
		public int DefaultViewID
		{
			get
			{
				return _templetInfo.DefaultViewID;
			}
		}
        /// <summary>
        /// ��ʾ˳��
        /// </summary>
        public int ShowOrder
        {
            get
            {
                return _templetInfo.showorder;
            }
        }
        /// <summary>
        /// �Ƿ��������
        /// </summary>
        public string Mark
        {
            get
            {
                return _templetInfo.mark;
            }
        }
		/// <summary>
		/// ¼�빦��Ȩ�ޱ��
		/// </summary>
        public string FuncKey
		{
			get
			{
				return _templetInfo.FuncKey;
			}
		}

		/// <summary>
		/// ����Ĭ����ͼ����
		/// </summary>
		public ListView DefaultView
		{
			get
			{
				 _defaultView = new ListView(this, this.DefaultViewID);                
				return _defaultView;
			}
		}
		/// <summary>
		/// ģ��������ֶεļ���
		/// </summary>
		public FieldCollection Fields
		{
			get
			{
				if (this._fields == null)
					this._fields = GetFields(0);

				
				return this._fields;
			}
		}
		public FieldCollection getoutfields(int viewid)
		{
			return GetFields(viewid);

		}
		#endregion

		#region --ģ�����ʹ�õķ���-- 
		/// <summary>
		/// ��ȡ���е�ģ�壬����һ�����԰ﶨ��DataGrid��DataTable��
		/// </summary>
		/// <returns>��������ʹ��ģ���DataTable����</returns>
        public static DataTable GetAllTempletList()
        {
            Goldnet.Dal.TempList dal = new TempList();
            return dal.GetAllTempletList();
        }
		/// <summary>
		/// ���һ���µĿհ�ģ��
		/// </summary>
		/// <param name="name">ģ������</param>
		/// <param name="title">ģ�����</param>
		/// <param name="common">ģ��˵��</param>
		public static void AddNewTemplet(string name, string title, string common,int showorder,string mark)
		{
			// �������:
			// ��ѯģ�������Ƿ�ʹ�ã������ʹ�ã��׳��쳣��
			// ����һ��������������ڣ�������һ�飬ֱ�����ɵı��������ڣ�
			// ��Ȩ�޹��������n�����ܣ�����¼���ܱ�š�
			// ��ʼһ��������
			// �����ݿ������һ��ָ��������
			// ��ģ��������һ����¼�����ؼ�¼�����Ϊģ����
			// ����ͼ�������һ�����ģ���������ͼ����ΪĬ����ͼ��
			// ����ģ�������Ӧ��¼��Ĭ����ͼ���
			// �ύ������
			// ���������ʧ�ܣ���Ȩ�޹���ɾ����n�����ܱ�š�

			string sql, tabName;
			bool existed;
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
			// �ж�ģ�������Ƿ�ʹ��
            sql = string.Format("SELECT COUNT(name) AS existed FROM {0}.T_TempletDict WHERE (DELETED=0 AND Name = ?)", DataUser.ZLGL);
			if (Convert.ToBoolean(OracleOledbBase.ExecuteScalar(sql, new OleDbParameter("name", name))))
				throw new TempletNameInUseException(name);
			int id=OracleOledbBase.GetMaxID("ID",string.Format("{0}.T_TempletDict",DataUser.ZLGL));


			// ��ʼ��һ��ģ�����
			TempletInfo templet = new TempletInfo();
			templet.Name = name;
			templet.Title = title;
			templet.Common = common;
			templet.CreateDate = DateTime.Now;
			templet.DefaultViewID = id;
			templet.Deleted = false;

			#region --����ģ���Ӧ����-- 
			do
			{
				Random rnd = new Random();
				string yearStr = DateTime.Now.Year.ToString();
				string monthStr = DateTime.Now.Month.ToString();
				string dayStr = DateTime.Now.Day.ToString();
                string hors = DateTime.Now.Hour.ToString();
                string mi = DateTime.Now.Minute.ToString();
				string secondStr = DateTime.Now.Second.ToString();
				//string num = rnd.Next(10000).ToString();
				tabName = "CustomTable_" + yearStr + monthStr + dayStr+"_"+hors+"_"+mi+"_"+ secondStr ; //ģ���ʶ�ַ���

               string  sql1 = string.Format("select count(TABLE_NAME) from DBA_TABLES where owner='{0}' AND TABLE_NAME='" + tabName + "'",DataUser.ZLGL.ToUpper());
				existed = Convert.ToBoolean(OracleOledbBase.ExecuteScalar(sql1));
			}while(existed);

			templet.TabName = tabName;
			#endregion

			#region --���ɹ��ܱ��-- 
			try
			{
				templet.FuncKey = dal.AddFunctionInfo(TEMPLET_FUNC_TITLE + name, "");
				
			}
            catch (GuideException ex)
			{
				// ��ӹ���ʱ������ʾ
				throw new TempletException(ADD_TEMPLET_ERROR, ADD_TEMPLET_ERROR_ON_POWER + ex.Title + ex.Content, ex); 
			}
			#endregion

			#region --��ʼһ��������-- 
			OleDbConnection myConnection = new OleDbConnection(CONNECT_STRING);
			myConnection.Open();
			OleDbTransaction myTrans = myConnection.BeginTransaction();

			try
			{
				#region --�����ݿ������һ��ָ����-- 
                string sql2 = string.Format("CREATE TABLE {0}." + tabName + " (ID NUMBER  NOT NULL ,Deleted VARCHAR2(8) NOT NULL ,CreateStaff VARCHAR2(50),CreateDate DATE ,LastEditStaff VARCHAR2(50),LastEditDate DATE ,FLAG VARCHAR2(12) DEFAULT '0',depttemplettype VARCHAR2(50) DEFAULT '0',CHECKCONT_ID  NUMBER(12) DEFAULT 0,DEPT_CODE VARCHAR2(50))", DataUser.ZLGL);
				OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, sql2, null);
				string altsql=string.Format("ALTER TABLE {0}." + tabName + "  ADD (CONSTRAINT P" + tabName +"  PRIMARY KEY(ID)) ",DataUser.ZLGL);
				OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text,altsql,null);
				#endregion

				#region --��ģ��������һ����¼,����ȡ��ӵı�š�-- 

				//
				string flag="0";

               string  sql3 = string.Format(@"INSERT INTO {0}.T_TempletDict
      (ID,Name, Title, TabName, Common, CreateDate, DefaultViewID, FuncKey,DELETED,flag,showorder,mark) 
	VALUES (" + id + ",'" + templet.Name + "','" + templet.Title + "','" + templet.TabName + "','" + templet.Common + "',to_date('" + templet.CreateDate.ToString() + "','yyyy-mm-dd hh24:mi:ss')," + id + ",'" + templet.FuncKey + "',0,'" + flag + "',"+showorder+",'"+mark+"')", DataUser.ZLGL);

				OleDbParameter[] parameters = {
												  new OleDbParameter("ID", id),
												  new OleDbParameter("title", templet.Title),
												  new OleDbParameter("common", templet.Common),
                                                  new OleDbParameter("showorder", showorder)
												 
											  };
				templet.ID = id;
                OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, sql3, null);



				#endregion

				#region --����ͼ�������һ����ͼ����������ͼ��ţ���ΪĬ����ͼ��-- 
                int viwid = OracleOledbBase.GetMaxID("ID", string.Format("{0}.T_ViewDict", DataUser.ZLGL));
               string sql4 = string.Format("INSERT INTO {0}.T_ViewDict (ID,Name, TempletID, PageCount) VALUES (" + viwid + ",'" + templet.Name + "'," + templet.ID + ",0)", DataUser.ZLGL);
				
                OleDbParameter[] parameters1 = {
												  new OleDbParameter("id", viwid),
					new OleDbParameter("name", templet.Name),
					new OleDbParameter("templetID", templet.ID)		 
                                               };
                templet.DefaultViewID = viwid;
                OracleOledbBase.ExecuteScalar(sql4);
				#endregion

				#region --����Ĭ����ͼ���-- 
				string upsql = string.Format("UPDATE {0}.T_TempletDict SET DefaultViewID = ? WHERE (ID = ?)",DataUser.ZLGL);
                OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, upsql,
                    new OleDbParameter("defaultViewID", viwid),
					new OleDbParameter("templetID", templet.ID)
					);
				#endregion

				#region --�ύ�������ر����ݿ�����-- 
				myTrans.Commit();
				myConnection.Close();
				myConnection.Dispose();
				#endregion

			}
			catch(Exception e)
			{
				try
				{
					#region --ɾ����ӵĹ���Ȩ��-- 
					dal.DeleteFunc(templet.FuncKey,templet.ID.ToString());
					
					#endregion

					myTrans.Rollback();
					myConnection.Close();
					throw new TempletException(ADD_TEMPLET_ERROR, ADD_TEMPLET_ERROR_ON_DATABASE + e.Message);
				}
				catch (SqlException ex)
				{
					if (myTrans.Connection != null)
					{
						myConnection.Close();
						throw new TempletException(ADD_TEMPLET_ERROR, ADD_TEMPLET_ERROR_ON_DATABASE + ex.Message);
					}
				}
			}
			#endregion
		}

		/// <summary>
		/// ���һ���µĿհ�ģ��
		/// </summary>
		/// <param name="templetID"></param>
		/// <param name="name">ģ������</param>
		/// <param name="title">ģ�����</param>
		/// <param name="common">ģ��˵��</param>
        public static void UpdateTempletInfo(int templetID, string name, string title, string common,int showorder,string mark)
        {
            // �޸�����:
            string sql;
            bool existed;

            // �ж�ģ�������Ƿ�ʹ�ã������ʹ�ã��׳��쳣��
            sql = string.Format("SELECT COUNT(name) AS existed FROM {0}.T_TempletDict WHERE (Name = ?) AND (ID <> ?)  and  DELETED=0", DataUser.ZLGL);
            existed = Convert.ToBoolean(OracleOledbBase.ExecuteScalar(CONNECT_STRING, sql,
                new OleDbParameter("name", name),
                new OleDbParameter("id", templetID)
                ));

            if (existed)
                throw new TempletNameInUseException(name);

            // ����ģ����Ϣ
            sql = string.Format("UPDATE {0}.T_TempletDict SET Name =?, Title =?, Common =?,showorder=?,mark=? WHERE (ID = ?)", DataUser.ZLGL);
            OracleOledbBase.ExecuteNonQuery(sql,
                new OleDbParameter("name", name),
                new OleDbParameter("title", title),
                new OleDbParameter("common", common),
                new OleDbParameter("showorder", showorder),
                 new OleDbParameter("mark", mark),
                new OleDbParameter("id", templetID)
                );
        }

		/// <summary>
		/// ����ֶ�
		/// </summary>
		/// <param name="fieldName">�ֶ�����</param>
		/// <param name="fieldTypeID">�ֶ����ͱ��</param>
		/// <param name="curPage">�����ֶο�ѡ���Ե�ҳ��</param>
		/// <param name="addToDefaultView">�Ƿ���Ӵ��ֶε���ǰ��Ĭ����ͼ</param>
		public void AddNewField(string fieldName, int fieldTypeID, System.Web.UI.Page curPage, bool addToDefaultView)
		{
			// ����ֶ�:
			string sql, fieldDefineSQL = "";
			int existed=0;
			int fieldDefineID = 0;//, fieldID;

			// �ж��ֶ������Ƿ����,�������,�׳�һ���쳣
			sql = string.Format("SELECT COUNT(ID) AS countid FROM {0}.T_TempletFieldDict WHERE (TempletID = ?) AND (FieldName = ?)",DataUser.ZLGL);
			existed = Convert.ToInt32(OracleOledbBase.ExecuteScalar(sql, 
				new OleDbParameter("templetID", this.ID),
				new OleDbParameter("fieldName", fieldName)
				));

			if (existed>0) throw new FieldNameInUseException(fieldName);

			#region --��ʼһ��������-- 
			OleDbConnection myConnection = new OleDbConnection(CONNECT_STRING);
			myConnection.Open();
			OleDbTransaction myTrans = myConnection.BeginTransaction();

			try
			{
				int id=OracleOledbBase.GetMaxID("ID",string.Format("{0}.T_TempletFieldDict",DataUser.ZLGL));
				#region --��ģ��������һ����¼,����ȡ��ӵı�š�-- 
				sql = string.Format(@"INSERT INTO {0}.T_TempletFieldDict
	(ID,TempletID, FieldName, FieldTypeID, FieldDefineID) 
	VALUES (?,?,?,?,?)",DataUser.ZLGL);
				OleDbParameter[] cmdParms = new OleDbParameter[]{
																	new OleDbParameter("id", id),
																	new OleDbParameter("templetID", this.ID),
																	new OleDbParameter("fieldName", fieldName),
																	new OleDbParameter("fieldTypeID", fieldTypeID),
																	new OleDbParameter("fieldDefineID", fieldDefineID)											
																};
				OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text,sql,cmdParms);

string str=string.Format(@" UPDATE {0}.T_TempletFieldDict SET SortNum = (SELECT COUNT(ID) AS CountNum FROM {0}.T_TempletFieldDict WHERE (TempletID = ?)) WHERE (ID = ?)",DataUser.ZLGL);
				
				OracleOledbBase.ExecuteScalar(myTrans, CommandType.Text, str, 
					new OleDbParameter("templetID", this.ID),
					new OleDbParameter("id", id)
					);
				#endregion

				#region --��ȡһ���ֶ����Ͷ��󣬲���һ���¶��岢���ض����š�
				IFieldType fieldTypeObj = FieldTypeFactory.CreateFieldTypeObj(fieldTypeID, new Field(new Field.FieldInfo(id, fieldName, this.ID, fieldTypeID, fieldDefineID, 0)));

				fieldDefineID = fieldTypeObj.InsertSpecialPropertyFormPage(curPage, ref fieldDefineSQL); 
				#endregion

				#region --��ָ���������һ��-- 
				sql = fieldDefineSQL.Replace("$$TABLE_NAME$$", DataUser.ZLGL+"."+this.TabName);
				OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, sql, null);
				#endregion

				#region --�����ֶζ�����-- 
				sql = string.Format("UPDATE {0}.T_TempletFieldDict SET FieldDefineID = ? WHERE (ID = ?)",DataUser.ZLGL);
				OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text,sql,
					new OleDbParameter("fieldDefineID", fieldDefineID),
					new OleDbParameter("fieldID", id));
				#endregion

                #region --����ֶε�Ĭ����ͼ��--
                if (addToDefaultView) this.DefaultView.AddDisplayField(myTrans, id);
                #endregion

				#region --�ύ�������ر����ݿ�����-- 
				myTrans.Commit();
				myConnection.Close();
				myConnection.Dispose();
				#endregion	
			}
            catch(Exception e)
            {
                try
                {
                    myTrans.Rollback();
                    myConnection.Close();
                    myConnection.Dispose();

                    throw new TempletException(ADD_TEMPLET_ERROR, ADD_TEMPLET_ERROR_ON_DATABASE + e.Message);
                }
                catch (SqlException ex)
                {
                    if (myTrans.Connection != null)
                    {
                        myConnection.Close();
                        throw new TempletException(ADD_TEMPLET_ERROR, ADD_TEMPLET_ERROR_ON_DATABASE + ex.Message);
                    }
                }
            }
			#endregion
		}

		/// <summary>
		/// ����һ���ֶε�����
		/// </summary>
		/// <param name="fieldID">�ֶα��</param>
		/// <param name="fieldName">�ֶ�����</param>
		/// <param name="curPage">�����ֶο�ѡ���Ե�ҳ��</param>
		/// <param name="addToDefaultView">�Ƿ���Ӵ��ֶε���ǰ��Ĭ����ͼ</param>
		public void UpdateField(int fieldID, string fieldName, System.Web.UI.Page curPage, bool addToDefaultView)
		{
			// ����ֶ�:
			string sql, fieldDefineSQL = "";

			// �ж��ֶ������Ƿ����,�������,�׳�һ���쳣
			sql = string.Format("SELECT COUNT(ID) AS countid FROM {0}.T_TempletFieldDict WHERE (TempletID = ?) AND (FieldName = ?) AND (ID <> ?)",DataUser.ZLGL);
			object obj=OracleOledbBase.ExecuteScalar(sql, 
				new OleDbParameter("templetID", this.ID),
				new OleDbParameter("fieldName", fieldName),
				new OleDbParameter("fieldID", fieldID)
				);

			if (Convert.ToInt32(obj.ToString())>0) throw new FieldNameInUseException(fieldName);

			// ��ȡһ��Field ����
			Field field = new Field(fieldID);
			// fieldDefineID = field.FieldDefineID;
			field.UpdateSpecialPropertyFormPage(curPage, ref fieldDefineSQL);
			
			#region --��ʼһ��������-- 
			OleDbConnection myConnection = new OleDbConnection(CONNECT_STRING);
			myConnection.Open();
			OleDbTransaction myTrans = myConnection.BeginTransaction();

			try
			{
				#region --�޸ı���ж���-- 
				if (fieldDefineSQL != string.Empty)
				{
					sql = fieldDefineSQL.Replace("$$TABLE_NAME$$", DataUser.ZLGL+"."+ this.TabName);
					OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, sql, null);
				}
				#endregion

				#region --��ģ��������һ����¼,����ȡ��ӵı�š�-- 
				sql = string.Format(@"UPDATE {0}.T_TempletFieldDict SET FieldName = ? WHERE (ID = ?)",DataUser.ZLGL);
				
				OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, sql, 
					new OleDbParameter("fieldName", fieldName),
					new OleDbParameter("fieldID", fieldID)
					);
				#endregion

                #region --����ֶε�Ĭ����ͼ��--
                if (addToDefaultView)
                    this.DefaultView.AddDisplayField(myTrans, fieldID);
                else
                    this.DefaultView.DeleteDisplayField(myTrans, fieldID);
                #endregion

				#region --�ύ�������ر����ݿ�����-- 
				myTrans.Commit();
				myConnection.Close();
				myConnection.Dispose();
				#endregion
			}
			catch(Exception e)
			{
				try
				{
					myTrans.Rollback();
					myConnection.Close();
					myConnection.Dispose();

					throw new TempletException(ADD_TEMPLET_ERROR, ADD_TEMPLET_ERROR_ON_DATABASE + e.Message);
				}
				catch (SqlException ex)
				{
					if (myTrans.Connection != null)
					{
						myConnection.Close();
						throw new TempletException(ADD_TEMPLET_ERROR, ADD_TEMPLET_ERROR_ON_DATABASE + ex.Message);
					}
				}
			}
			#endregion
		}
		#endregion

		#region --�ǹ��з���-- 
		/// <summary>
		/// ��ȡ�ֶμ��ϵ��ڲ�����
		/// </summary>
		/// <param name="viewID">Ҫ��ȡ����ͼ��ʾ�ֶε���ͼ��ţ������ͼ���Ϊ0������ģ��������ֶ�</param>
		/// <returns>�ֶ��б���</returns>
		public FieldCollection GetFields(int viewID)
		{
			Goldnet.Dal.TempList dal=new TempList();
			System.Data.DataTable reader=dal.GetFields(viewID,this.ID);
			FieldCollection result = new FieldCollection();

            for (int i = 0; i < reader.Rows.Count; i++)
            {
                Field.FieldInfo info = new Field.FieldInfo(
                    Convert.ToInt32(reader.Rows[i]["ID"].ToString()),
                    reader.Rows[i]["FieldName"].ToString(),
                    Convert.ToInt32(reader.Rows[i]["TempletID"].ToString()),
                    Convert.ToInt32(reader.Rows[i]["FieldTypeID"].ToString()),
                    Convert.ToInt32(reader.Rows[i]["FieldDefineID"].ToString()),
                    Convert.ToInt32(reader.Rows[i]["SortNum"].ToString()));

                result.Add(new Field(info));
            }
			return result;
		}
        /// <summary>
        /// �ж��ֶ��Ƿ����
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool exitdisplayfild(Field field)
        {
            string str = string.Format("select * from {0}.T_ViewDisplayFiledsDict where viewid=" + field.TempletID + " and FIELDID=" + field.ID, DataUser.ZLGL);
            if (OracleOledbBase.ExecuteDataSet(str).Tables[0].Rows.Count > 0)
                return true;
            else
                return false;

        }

		/// <summary>
		/// ����ָ�����͵��ֶζ���ļ��ϱ�
		/// </summary>
		/// <param name="typeName">�ֶ���������</param>
		/// <returns>�ֶμ��ϱ�</returns>
        private FieldCollectionTable getFieldsByTypes(string typeName)
        {
            string sql;

            FieldCollectionTable fieldTable = new FieldCollectionTable();

            sql = string.Format("SELECT T_TempletFieldDict.ID AS ID, T_TempletFieldDict.FieldName AS FieldName, T_TempletFieldDict.TempletID AS TempletID, T_TempletFieldDict.FieldTypeID AS FieldTypeID, T_TempletFieldDict.FieldDefineID AS FieldDefineID, T_TempletFieldDict.SortNum AS SortNum FROM {0}.T_TempletFieldDict , {0}.T_FieldTypeDict WHERE T_TempletFieldDict.FieldTypeID = T_FieldTypeDict.ID AND (T_FieldTypeDict.Name = '" + typeName + "') AND (T_TempletFieldDict.TempletID = ?) ORDER BY SortNum, ID", DataUser.ZLGL);

            OracleOledbBase.FillTheTable(fieldTable, sql, new OleDbParameter("templetID", this.ID));

            return fieldTable;
        }
		/// <summary>
		/// ����ɾ��һ����¼
		/// 
		/// </summary>
		/// <param name="recID">��¼��</param>
        private void deleteinputRecord(int recID)
        {
            MyLists listtable = new MyLists();
            List listdeldetail = new List();
            string sql = string.Format("DELETE FROM {0}." + this.TabName + " WHERE (ID = ?)", DataUser.ZLGL);
            listdeldetail.StrSql = sql;
            listdeldetail.Parameters =  new OleDbParameter[] { new OleDbParameter("recID", recID) };
            listtable.Add(listdeldetail);
            string str = string.Format("delete {0}.QUALITY_ERROR_LIST where table_name='" + this.TabName + "' and table_id=" + recID, DataUser.ZLGL);
            List listdelerr = new List();
            listdelerr.StrSql = str;
            listdelerr.Parameters = new OleDbParameter[0] { };
            listtable.Add(listdelerr);
            OracleOledbBase.ExecuteTranslist(listtable);
            
        }
        /// <summary>
        /// ��������Ƿ���ɾ��
        /// </summary>
        /// <param name="id"></param>
        public void checkrecorddel(int recID)
        {
            string strdel = string.Format("select DATE_TIME from {0}.QUALITY_ERROR_LIST where table_name='" + this.TabName + "' and table_id=" + recID, DataUser.ZLGL);
            string date = OracleOledbBase.GetSingle(strdel).ToString();
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
            if (guidedal.IsExistCheckCollectWeek(date) == true)
            {
                throw new SaveRecordDataIsNoWeek(ConvertDate.GetWeekRange(date));
            }
            else if (guidedal.IsExistCheckCollect(Convert.ToDateTime(date).Year.ToString() + "-" + Convert.ToDateTime(date).Month.ToString()) == true)
            {
                throw new SaveRecordDataIsNo(Convert.ToDateTime(date).Year.ToString() + "-" + Convert.ToDateTime(date).Month.ToString());
            }
        }
        /// <summary>
        /// ɾ����¼
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="recID"></param>
        public void deleteRecord(IStaffTargetInfo staff, int recID)
        {
            MyLists listtable = new MyLists();
            List listdeldetail = new List();
            string sql;
            // ɾ��һ����¼
            sql = string.Format("UPDATE {0}." + this.TabName + " SET LastEditStaff = ?, LastEditDate = to_date('" + DateTime.Now + "','YYYY-MM-DD HH24:MI:SS'), Deleted = 1 WHERE (ID = ?)", DataUser.ZLGL);
            OleDbParameter[] cmdParms = new OleDbParameter[]{
																new OleDbParameter("LastEditStaff", staff.Name),
																new OleDbParameter("id", recID)										
															};
            listdeldetail.StrSql = sql;
            listdeldetail.Parameters = cmdParms;
            listtable.Add(listdeldetail);

            string str = string.Format("delete {0}.QUALITY_ERROR_LIST where table_name='" + this.TabName + "' and table_id=" + recID, DataUser.ZLGL);
            List listdelerr = new List();
            listdelerr.StrSql = str;
            listdelerr.Parameters = new OleDbParameter[0] { };
            listtable.Add(listdelerr);
            OracleOledbBase.ExecuteTranslist(listtable);

        }
		#endregion

		#region --ģ�巽��-- 
		/// <summary>
		/// ȡ�����м�¼
		/// </summary>
		/// <returns></returns>
		public DataTable GetAllRecord()
		{
            string sql = string.Format("SELECT a.*,nvl(b.sort_no,9999) sort_no FROM {0}." + this.TabName + " a,comm.sys_dept_dict b WHERE a.Deleted = 0 and a.dept_code=b.dept_code(+) order by a.LASTEDITDATE desc", DataUser.ZLGL);

			DataSet dataset = OracleOledbBase.ExecuteDataSet(sql);
			return dataset.Tables[0];
		}
		/// <summary>
		/// ĳ���Ҽ�¼
		/// </summary>
		/// <returns></returns>
		public DataTable GetAllRecorddept(string deptcode,int id)
		{
			string sql = string.Format("SELECT * FROM {0}." + this.TabName + " WHERE Deleted = 0 and (flag='"+deptcode+"' or dept_id_"+id.ToString()+"='"+deptcode+"')",DataUser.ZLGL);

			DataSet dataset = OracleOledbBase.ExecuteDataSet(sql);

			return dataset.Tables[0];
		}

		/// <summary>
		/// ȡ��ĳ�����޸Ĺ������м�¼
		/// </summary>
		/// <returns></returns>
		public DataTable GetAllRecordByInputStaff(string name)
		{
			string sql = string.Format("SELECT * FROM {0}." + this.TabName + " WHERE (Deleted = 0) AND (CreateStaff = '" + name + "') OR (Deleted = 0) AND (LastEditStaff = '" + name + "')",DataUser.ZLGL);

			DataSet dataset = OracleOledbBase.ExecuteDataSet(sql);

			return dataset.Tables[0];
		}
        /// <summary>
        /// �۷ַ���
        /// </summary>
        /// <returns></returns>
        public DataTable GetQuality()
        {
            string str = string.Format("select * from {0}.quality_type", DataUser.ZLGL);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }
		
		/// <summary>
		/// ��ģ���в���һ����¼
		/// </summary>
		/// <param name="curPage">����¼�����ݵ�ҳ��</param>
		/// <param name="staff">��Ա����</param>
        public void InsertNewRecord(System.Web.UI.Page curPage, IStaffTargetInfo staff)
        {
            string sql;
            int recID;
            string flag = "0";
            OleDbConnection myConnection = new OleDbConnection(CONNECT_STRING);
            myConnection.Open();
            OleDbTransaction myTrans = myConnection.BeginTransaction();
            try
            {

                // ����һ���¼�¼
                sql = string.Format("INSERT INTO {0}." + this.TabName + " (id,DELETED,CreateStaff, CreateDate, LastEditStaff, LastEditDate,FLAG,depttemplettype) VALUES (?,0,?, ?,?,?,?,?)", DataUser.ZLGL);

                recID = OracleOledbBase.GetMaxID("id", DataUser.ZLGL + "." + this.TabName);
                OracleOledbBase.ExecuteScalar(myTrans,CommandType.Text,sql,
                    new OleDbParameter("id", recID),
                    new OleDbParameter("createStaff", staff.Name),
                    new OleDbParameter("createDate", DateTime.Now),
                    new OleDbParameter("lastEditStaff", staff.Name),
                    new OleDbParameter("lastEditDate", DateTime.Now),
                    new OleDbParameter("lastEditDate", flag),
                    new OleDbParameter("depttemplettype", this.Title)
                    );


                // ���θ���ÿһ����¼����������쳣��ɾ����¼���׳��쳣��

                int id = OracleOledbBase.GetMaxID("id", string.Format("{0}.QUALITY_ERROR_LIST", DataUser.ZLGL));
                string ischeck = "0";
                if (((CheckBox)curPage.FindControl("quality")).Checked == true)
                {
                    ischeck = "1";
                }
                {
                    string str = string.Format("insert into {0}.QUALITY_ERROR_LIST (id,templet_id,table_name,table_id,input_dept,input_user,flags,date_time,function_id,IS_CHECKLOOK,STAFF_ID,CREATEDATE) values(?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.ZLGL);
                    OleDbParameter[] cmdParms = new OleDbParameter[]{
																		new OleDbParameter("id",id),
																		new OleDbParameter("templet_id",_templetInfo.ID),
																		new OleDbParameter("table_name",this.TabName),
																		new OleDbParameter("table_id",recID),
																		new OleDbParameter("input_dept",staff.Dept),
																		new OleDbParameter("input_user",staff.Name),
																		new OleDbParameter("flags","0"),
																		new OleDbParameter("date_time",DateTime.Now.ToString("yyyy-MM-dd")),
						                                                new OleDbParameter("function_id",int.Parse(((DropDownList) curPage.FindControl("DropDownList1")).SelectedValue)),
                                                                        new OleDbParameter("IS_CHECKLOOK",ischeck),
                                                                        new OleDbParameter("STAFF_ID",staff.Key),
                                                                        new OleDbParameter("createDate", DateTime.Now)
																	};
                    OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text,str, cmdParms);

                }
                foreach (Field field in this.GetFields(_templetInfo.ID))
                {
                    field.UpdateRecord(myTrans,this.TabName, recID, curPage, id);

                }
                myTrans.Commit();
                myConnection.Close();
                myConnection.Dispose();

            }
            catch (GlobalException ex)
            {
                myTrans.Rollback();
                myConnection.Close();
                myConnection.Dispose();
                throw ex;
            }

        }

		/// <summary>
		/// �޸�һ��ģ���¼
		/// </summary>
		/// <param name="curPage">�����޸����ݵ�ҳ��</param>
		/// <param name="staff">�޸���</param>
		/// <param name="recID">�޸ļ�¼</param>
		public void UpdataRecord(System.Web.UI.Page curPage, IStaffTargetInfo staff, int recID,int id)
		{
            OleDbConnection myConnection = new OleDbConnection(CONNECT_STRING);
            myConnection.Open();
            OleDbTransaction myTrans = myConnection.BeginTransaction();
            try
            {
                string sql;
                // �޸�һ����¼
                sql = string.Format("UPDATE {0}." + this.TabName + " SET LastEditStaff =?, LastEditDate = to_date('" + DateTime.Now + "','YYYY-MM-DD HH24:MI:SS') ,depttemplettype=? WHERE (ID = ?)", DataUser.ZLGL);
                OleDbParameter[] cmdParms = new OleDbParameter[]{
																new OleDbParameter("LastEditStaff",staff.Name),
																new OleDbParameter("depttemplettype",this.Title),
																new OleDbParameter("id",recID)												
															};
                OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text,sql, cmdParms);

                string ischeck = "0";
                if (((CheckBox)curPage.FindControl("quality")).Checked == true)
                {
                    ischeck = "1";
                }
                string strup = string.Format("update {0}.QUALITY_ERROR_LIST set IS_CHECKLOOK='{1}' where table_name='{2}' and table_id='{3}'", DataUser.ZLGL, ischeck, this.TabName, recID);
                OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, strup, new OleDbParameter[0] { });
                if ((CheckBox)curPage.FindControl("qualityedit") != null)
                {
                    string flags = "0";
                    if (((CheckBox)curPage.FindControl("qualityedit")).Checked == true)
                    {
                        flags = "1";
                    }

                    string str = string.Format("update {0}.QUALITY_ERROR_LIST set flags='{3}' where table_name='{1}' and table_id='{2}'", DataUser.ZLGL, this.TabName, recID, flags);
                    OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, str, new OleDbParameter[0] { });

                }
                // ���θ���ÿһ����¼����������쳣��ɾ����¼���׳��쳣��

                foreach (Field field in this.GetFields(_templetInfo.ID))
                {
                    field.UpdateRecord(myTrans,this.TabName, recID, curPage, id);
                }
                myTrans.Commit();
                myConnection.Close();
                myConnection.Dispose();

            }
            catch (GlobalException ex)
            {
                myTrans.Rollback();
                myConnection.Close();
                myConnection.Dispose();
                throw ex;
            }
		}


		/// <summary>
		/// ��ȡһ����¼���޸���Ϣ
		/// </summary>
		/// <param name="recID">��¼��</param>
		/// <returns>�޸���Ϣ</returns>
		public string GetModifyInfo(int recID)
		{
			string result = string.Empty;
			string sql = string.Format("SELECT CreateStaff, CreateDate, LastEditStaff, LastEditDate FROM {0}." + this.TabName + " WHERE (ID = ?)",DataUser.ZLGL);

			DataTable reader = OracleOledbBase.ExecuteDataSet( sql, new OleDbParameter("recID", recID)).Tables[0];

			for (int i=0;i<reader.Rows.Count;i++)
			{
				result += "��¼������: " + reader.Rows[i][0].ToString() + "   ����ʱ��:" + reader.Rows[i][1].ToString() 
					+ "\r\n����޸���: " + reader.Rows[i][2].ToString() + "   �޸�ʱ��:" + reader.Rows[i][3].ToString();
			}
			//reader.Close();

			return result;
		}
		#endregion

		#region --ָ��ϵͳʹ�õķ���-- 
		/// <summary>
		/// �õ�����������
		/// </summary>
		/// <returns></returns>
		public FieldCollectionTable GetDateFields()
		{
			return getFieldsByTypes("����");
		}

		/// <summary>
		/// �õ����в�����
		/// </summary>
		/// <returns></returns>
		public FieldCollectionTable GetDeptFields()
		{
			return getFieldsByTypes("����");
		}
       
		/// <summary>
		/// �õ�����������
		/// </summary>
		/// <returns></returns>
		public FieldCollectionTable GetNumFields()
		{
			return getFieldsByTypes("����");
		}

	/// <summary>
	/// ָ��
	/// </summary>
	/// <returns></returns>
		public FieldCollectionTable GetGuidFields()
		{
			return getFieldsByTypes("ָ��");
		}

        

		/// <summary>
		/// ��ģ��������ͳ��
		/// </summary>
		/// <param name="dateFieldID">ʱ���ֶα��</param>
		/// <param name="deptFieldID">�����ֶα��</param>
		/// <param name="numberFieldID">�����ֶα��</param>
		/// <param name="startDate">��ʼʱ��</param>
		/// <param name="endDate">����ʱ��</param>
		/// <param name="GroupCode">���ұ��</param>
		/// <returns>SUM()ֵ</returns>
		/// <exception cref="TempletException">ģ���쳣:ͳ�Ƴ���!</exception>
		public decimal GetCountValue(int dateFieldID, int deptFieldID, int numberFieldID, DateTime startDate, DateTime endDate, string GroupCode)
		{

            Goldnet.Dal.SYS_DEPT_DICT dal=new Goldnet.Dal.SYS_DEPT_DICT();
            
			Field dateField, deptField, numberField;
			DataTable table;

			dateField = new Field(dateFieldID);
			deptField = new Field(deptFieldID);
			numberField = new Field(numberFieldID);
			table = this.GetAllRecord();

			decimal result = 0;
			try
			{
				string computeStr = "SUM(" + numberField.ListDisplayDataName + ")";
				string filterStr = "(" + dateField.ListDisplayDataName + " <= #" + endDate.ToShortDateString() + "#) AND " 
					+ "(" + dateField.ListDisplayDataName + " >= #" + startDate.ToShortDateString() + "#) AND "
                    + "(" + deptField.ListDisplayDataName.Replace("DEPT_", "DEPT_ID_") + " in  " + dal.GetDeptbyaccountdept(GroupCode) + ")";

				object objResult = table.Compute(computeStr, filterStr);
				if (objResult == DBNull.Value)
					return 0;
				else
					result = Convert.ToDecimal(objResult);
			}
			catch(Exception ex)
			{
				throw new TempletException("����ͳ��ʱ�����쳣!", 
					"ģ��:" + this.Name + "��ͳ������:ʱ��Ϊ(" 
					+ startDate.ToShortDateString() + "-" 
					+ endDate.ToShortDateString() + ")���ֶα��:" 
					+ dateFieldID.ToString() 
					+ "; ���ұ��(" + GroupCode + ") ���ֶα��:" 
					+ deptFieldID.ToString() 
					+ "; ʱ�������´���" + ex.Message,
					ex
					);
			}

			return result;
		}

		/// <summary>
		/// ɾ��������¼
		/// </summary>
		/// <param name="ids">��¼��ţ��磺1,2,3�������ַ���</param>
		public void DeleteRecordByIDStr(string ids)
		{
			ids = ids.Trim();

			if (ids != string.Empty && ids != "")
			{
				string sql = "DELETE FROM " + this.TabName + " WHERE (ID IN (" + ids + "))";
				OracleOledbBase.ExecuteNonQuery(CONNECT_STRING, sql, null);
			}
		}
		#endregion
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
