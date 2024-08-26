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
	/// 模板的BO
	/// </summary>
	[Serializable()]
	public class TempletBO
	{
		#region --常量定义-- 
        public static string CONNECT_STRING = ConfigurationSettings.AppSettings["OledbConnString"];
        /// <summary>
        /// 文本类型编号
        /// </summary>
        public static int CHARFIELDTYPEID = 1;
        /// <summary>
        /// 数值类型编号
        /// </summary>
        public static int NUMBERFIELDTYPEID = 2;
        /// <summary>
        /// 选择类型编号
        /// </summary>
        public static int SELECTFIELDTYPID = 3;
        /// <summary>
        /// 时间类型编号
        /// </summary>
        public static int DATEFIELDTYPEID = 4;
        /// <summary>
        /// 指标类型编号
        /// </summary>
        public static int GUIDEFIELDTYPEID = 5;
        /// <summary>
        /// 人员类型编号
        /// </summary>
        public static int STAFFFIELDTYPEID = 6;
        /// <summary>
        /// 部门类型编号
        /// </summary>
        public static int DEPTFIELDTYPEID = 7;
        /// <summary>
        /// 病人类型编号
        /// </summary>
        public static int PATIENTFIELDTYPEID = 8;
        /// <summary>
        /// 跟踪类型编号
        /// </summary>
        public static int CHARMODIFYFIELTYPEID = 9;

       

		// 添加功能权限相关的文本
		private const string TEMPLET_FUNC_TITLE = "模板：";
		
		// 错误信息
		private const string ADD_TEMPLET_ERROR = "添加模板时，发生错误！";
		private const string ADD_TEMPLET_ERROR_ON_POWER = "向系统中注册功能权限时发生错误。错误的详细信息：";
		private const string ADD_TEMPLET_ERROR_ON_DATABASE = "向数据库中添加数据时发生错误。错误的详细信息：";

		#endregion

		#region --私有变量定义-- 
		private TempletInfo _templetInfo;

		private ListView _defaultView;

		private FieldCollection _fields;
		#endregion

		#region --内部实体类VO-- 
		/// <summary>
		/// 模板实体类
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

		#region --构造函数-- 
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="templetID">模板编号</param>
		/// <exception cref="TempletIDNotExistedException">指定的指标编号不存在!</exception>
		public TempletBO(int templetID)
		{
			// 依据模板编号初始化一个模板, 流程:
			// 查询此模板编号是否存在
			// 生成VO,并保存为私有变量.
			// 字段? 视图? 基本信息?

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

		#region --属性-- 
		/// <summary>
		/// 模板编号
		/// </summary>
		public int ID
		{
			get
			{
				return _templetInfo.ID;
			}
		}

		/// <summary>
		/// 模板名称
		/// </summary>
		public string Name
		{
			get
			{
				return _templetInfo.Name;
			}
		}

		/// <summary>
		/// 模板标题
		/// </summary>
		public string Title
		{
			get
			{
				return _templetInfo.Title;
			}
		}

		/// <summary>
		/// 模板表名
		/// </summary>
		public string TabName
		{
			get
			{
				return _templetInfo.TabName;
			}
		}
		/// <summary>
		/// 说明
		/// </summary>
		public string Common
		{
			get
			{
				return _templetInfo.Common;
			}
		}

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateDate
		{
			get
			{
				return _templetInfo.CreateDate;
			}
		}

		/// <summary>
		/// 默认视图编号
		/// </summary>
		public int DefaultViewID
		{
			get
			{
				return _templetInfo.DefaultViewID;
			}
		}
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int ShowOrder
        {
            get
            {
                return _templetInfo.showorder;
            }
        }
        /// <summary>
        /// 是否汇总数据
        /// </summary>
        public string Mark
        {
            get
            {
                return _templetInfo.mark;
            }
        }
		/// <summary>
		/// 录入功能权限编号
		/// </summary>
        public string FuncKey
		{
			get
			{
				return _templetInfo.FuncKey;
			}
		}

		/// <summary>
		/// 返回默认视图对象
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
		/// 模板包含的字段的集合
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

		#region --模板管理使用的方法-- 
		/// <summary>
		/// 获取所有的模板，返回一个可以帮定到DataGrid的DataTable。
		/// </summary>
		/// <returns>包含所有使用模板的DataTable对象</returns>
        public static DataTable GetAllTempletList()
        {
            Goldnet.Dal.TempList dal = new TempList();
            return dal.GetAllTempletList();
        }
		/// <summary>
		/// 添加一个新的空白模板
		/// </summary>
		/// <param name="name">模板名称</param>
		/// <param name="title">模板标题</param>
		/// <param name="common">模板说明</param>
		public static void AddNewTemplet(string name, string title, string common,int showorder,string mark)
		{
			// 添加流程:
			// 查询模板名称是否被使用，如果被使用，抛出异常。
			// 生成一个表名，如果存在，再生成一遍，直到生成的表名不存在，
			// 向权限管理中添加n个功能，并记录功能编号。
			// 开始一个事务处理
			// 向数据库中添加一个指定表名表。
			// 向模板表中添加一条记录，返回记录编号作为模板编号
			// 向视图表中添加一个与此模板关联的视图，作为默认视图。
			// 更新模板表中相应记录的默认视图编号
			// 提交事务处理
			// 如果事务处理失败，向权限管理删除那n个功能编号。

			string sql, tabName;
			bool existed;
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
			// 判断模板名称是否被使用
            sql = string.Format("SELECT COUNT(name) AS existed FROM {0}.T_TempletDict WHERE (DELETED=0 AND Name = ?)", DataUser.ZLGL);
			if (Convert.ToBoolean(OracleOledbBase.ExecuteScalar(sql, new OleDbParameter("name", name))))
				throw new TempletNameInUseException(name);
			int id=OracleOledbBase.GetMaxID("ID",string.Format("{0}.T_TempletDict",DataUser.ZLGL));


			// 初始化一个模板变量
			TempletInfo templet = new TempletInfo();
			templet.Name = name;
			templet.Title = title;
			templet.Common = common;
			templet.CreateDate = DateTime.Now;
			templet.DefaultViewID = id;
			templet.Deleted = false;

			#region --生成模板对应表名-- 
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
				tabName = "CustomTable_" + yearStr + monthStr + dayStr+"_"+hors+"_"+mi+"_"+ secondStr ; //模板标识字符串

               string  sql1 = string.Format("select count(TABLE_NAME) from DBA_TABLES where owner='{0}' AND TABLE_NAME='" + tabName + "'",DataUser.ZLGL.ToUpper());
				existed = Convert.ToBoolean(OracleOledbBase.ExecuteScalar(sql1));
			}while(existed);

			templet.TabName = tabName;
			#endregion

			#region --生成功能编号-- 
			try
			{
				templet.FuncKey = dal.AddFunctionInfo(TEMPLET_FUNC_TITLE + name, "");
				
			}
            catch (GuideException ex)
			{
				// 添加功能时错误提示
				throw new TempletException(ADD_TEMPLET_ERROR, ADD_TEMPLET_ERROR_ON_POWER + ex.Title + ex.Content, ex); 
			}
			#endregion

			#region --开始一个事务处理-- 
			OleDbConnection myConnection = new OleDbConnection(CONNECT_STRING);
			myConnection.Open();
			OleDbTransaction myTrans = myConnection.BeginTransaction();

			try
			{
				#region --向数据库中添加一个指定表-- 
                string sql2 = string.Format("CREATE TABLE {0}." + tabName + " (ID NUMBER  NOT NULL ,Deleted VARCHAR2(8) NOT NULL ,CreateStaff VARCHAR2(50),CreateDate DATE ,LastEditStaff VARCHAR2(50),LastEditDate DATE ,FLAG VARCHAR2(12) DEFAULT '0',depttemplettype VARCHAR2(50) DEFAULT '0',CHECKCONT_ID  NUMBER(12) DEFAULT 0,DEPT_CODE VARCHAR2(50))", DataUser.ZLGL);
				OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, sql2, null);
				string altsql=string.Format("ALTER TABLE {0}." + tabName + "  ADD (CONSTRAINT P" + tabName +"  PRIMARY KEY(ID)) ",DataUser.ZLGL);
				OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text,altsql,null);
				#endregion

				#region --向模板表中添加一条记录,并获取添加的编号。-- 

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

				#region --向视图表中添加一个视图，并返回视图编号，作为默认视图。-- 
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

				#region --更新默认视图编号-- 
				string upsql = string.Format("UPDATE {0}.T_TempletDict SET DefaultViewID = ? WHERE (ID = ?)",DataUser.ZLGL);
                OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, upsql,
                    new OleDbParameter("defaultViewID", viwid),
					new OleDbParameter("templetID", templet.ID)
					);
				#endregion

				#region --提交事务处理，关闭数据库连接-- 
				myTrans.Commit();
				myConnection.Close();
				myConnection.Dispose();
				#endregion

			}
			catch(Exception e)
			{
				try
				{
					#region --删除添加的功能权限-- 
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
		/// 添加一个新的空白模板
		/// </summary>
		/// <param name="templetID"></param>
		/// <param name="name">模板名称</param>
		/// <param name="title">模板标题</param>
		/// <param name="common">模板说明</param>
        public static void UpdateTempletInfo(int templetID, string name, string title, string common,int showorder,string mark)
        {
            // 修改流程:
            string sql;
            bool existed;

            // 判断模板名称是否被使用，如果被使用，抛出异常。
            sql = string.Format("SELECT COUNT(name) AS existed FROM {0}.T_TempletDict WHERE (Name = ?) AND (ID <> ?)  and  DELETED=0", DataUser.ZLGL);
            existed = Convert.ToBoolean(OracleOledbBase.ExecuteScalar(CONNECT_STRING, sql,
                new OleDbParameter("name", name),
                new OleDbParameter("id", templetID)
                ));

            if (existed)
                throw new TempletNameInUseException(name);

            // 更新模板信息
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
		/// 添加字段
		/// </summary>
		/// <param name="fieldName">字段名称</param>
		/// <param name="fieldTypeID">字段类型编号</param>
		/// <param name="curPage">包含字段可选属性的页面</param>
		/// <param name="addToDefaultView">是否添加此字段到当前的默认视图</param>
		public void AddNewField(string fieldName, int fieldTypeID, System.Web.UI.Page curPage, bool addToDefaultView)
		{
			// 添加字段:
			string sql, fieldDefineSQL = "";
			int existed=0;
			int fieldDefineID = 0;//, fieldID;

			// 判断字段名称是否存在,如果存在,抛出一个异常
			sql = string.Format("SELECT COUNT(ID) AS countid FROM {0}.T_TempletFieldDict WHERE (TempletID = ?) AND (FieldName = ?)",DataUser.ZLGL);
			existed = Convert.ToInt32(OracleOledbBase.ExecuteScalar(sql, 
				new OleDbParameter("templetID", this.ID),
				new OleDbParameter("fieldName", fieldName)
				));

			if (existed>0) throw new FieldNameInUseException(fieldName);

			#region --开始一个事务处理-- 
			OleDbConnection myConnection = new OleDbConnection(CONNECT_STRING);
			myConnection.Open();
			OleDbTransaction myTrans = myConnection.BeginTransaction();

			try
			{
				int id=OracleOledbBase.GetMaxID("ID",string.Format("{0}.T_TempletFieldDict",DataUser.ZLGL));
				#region --向模板表中添加一条记录,并获取添加的编号。-- 
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

				#region --获取一个字段类型对象，插入一条新定义并返回定义编号。
				IFieldType fieldTypeObj = FieldTypeFactory.CreateFieldTypeObj(fieldTypeID, new Field(new Field.FieldInfo(id, fieldName, this.ID, fieldTypeID, fieldDefineID, 0)));

				fieldDefineID = fieldTypeObj.InsertSpecialPropertyFormPage(curPage, ref fieldDefineSQL); 
				#endregion

				#region --向指定表中添加一列-- 
				sql = fieldDefineSQL.Replace("$$TABLE_NAME$$", DataUser.ZLGL+"."+this.TabName);
				OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, sql, null);
				#endregion

				#region --更新字段定义编号-- 
				sql = string.Format("UPDATE {0}.T_TempletFieldDict SET FieldDefineID = ? WHERE (ID = ?)",DataUser.ZLGL);
				OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text,sql,
					new OleDbParameter("fieldDefineID", fieldDefineID),
					new OleDbParameter("fieldID", id));
				#endregion

                #region --添加字段到默认视图中--
                if (addToDefaultView) this.DefaultView.AddDisplayField(myTrans, id);
                #endregion

				#region --提交事务处理，关闭数据库连接-- 
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
		/// 更新一个字段的属性
		/// </summary>
		/// <param name="fieldID">字段编号</param>
		/// <param name="fieldName">字段名称</param>
		/// <param name="curPage">包含字段可选属性的页面</param>
		/// <param name="addToDefaultView">是否添加此字段到当前的默认视图</param>
		public void UpdateField(int fieldID, string fieldName, System.Web.UI.Page curPage, bool addToDefaultView)
		{
			// 添加字段:
			string sql, fieldDefineSQL = "";

			// 判断字段名称是否存在,如果存在,抛出一个异常
			sql = string.Format("SELECT COUNT(ID) AS countid FROM {0}.T_TempletFieldDict WHERE (TempletID = ?) AND (FieldName = ?) AND (ID <> ?)",DataUser.ZLGL);
			object obj=OracleOledbBase.ExecuteScalar(sql, 
				new OleDbParameter("templetID", this.ID),
				new OleDbParameter("fieldName", fieldName),
				new OleDbParameter("fieldID", fieldID)
				);

			if (Convert.ToInt32(obj.ToString())>0) throw new FieldNameInUseException(fieldName);

			// 获取一个Field 对象
			Field field = new Field(fieldID);
			// fieldDefineID = field.FieldDefineID;
			field.UpdateSpecialPropertyFormPage(curPage, ref fieldDefineSQL);
			
			#region --开始一个事务处理-- 
			OleDbConnection myConnection = new OleDbConnection(CONNECT_STRING);
			myConnection.Open();
			OleDbTransaction myTrans = myConnection.BeginTransaction();

			try
			{
				#region --修改表的列定义-- 
				if (fieldDefineSQL != string.Empty)
				{
					sql = fieldDefineSQL.Replace("$$TABLE_NAME$$", DataUser.ZLGL+"."+ this.TabName);
					OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, sql, null);
				}
				#endregion

				#region --向模板表中添加一条记录,并获取添加的编号。-- 
				sql = string.Format(@"UPDATE {0}.T_TempletFieldDict SET FieldName = ? WHERE (ID = ?)",DataUser.ZLGL);
				
				OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, sql, 
					new OleDbParameter("fieldName", fieldName),
					new OleDbParameter("fieldID", fieldID)
					);
				#endregion

                #region --添加字段到默认视图中--
                if (addToDefaultView)
                    this.DefaultView.AddDisplayField(myTrans, fieldID);
                else
                    this.DefaultView.DeleteDisplayField(myTrans, fieldID);
                #endregion

				#region --提交事务处理，关闭数据库连接-- 
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

		#region --非公有方法-- 
		/// <summary>
		/// 获取字段集合的内部方法
		/// </summary>
		/// <param name="viewID">要获取的视图显示字段的视图编号，如果视图编号为0，返回模板的所有字段</param>
		/// <returns>字段列表集合</returns>
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
        /// 判断字段是否存在
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
		/// 返回指定类型的字段对象的集合表
		/// </summary>
		/// <param name="typeName">字段类型名称</param>
		/// <returns>字段集合表</returns>
        private FieldCollectionTable getFieldsByTypes(string typeName)
        {
            string sql;

            FieldCollectionTable fieldTable = new FieldCollectionTable();

            sql = string.Format("SELECT T_TempletFieldDict.ID AS ID, T_TempletFieldDict.FieldName AS FieldName, T_TempletFieldDict.TempletID AS TempletID, T_TempletFieldDict.FieldTypeID AS FieldTypeID, T_TempletFieldDict.FieldDefineID AS FieldDefineID, T_TempletFieldDict.SortNum AS SortNum FROM {0}.T_TempletFieldDict , {0}.T_FieldTypeDict WHERE T_TempletFieldDict.FieldTypeID = T_FieldTypeDict.ID AND (T_FieldTypeDict.Name = '" + typeName + "') AND (T_TempletFieldDict.TempletID = ?) ORDER BY SortNum, ID", DataUser.ZLGL);

            OracleOledbBase.FillTheTable(fieldTable, sql, new OleDbParameter("templetID", this.ID));

            return fieldTable;
        }
		/// <summary>
		/// 物理删除一条记录
		/// 
		/// </summary>
		/// <param name="recID">记录号</param>
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
        /// 检查数据是否能删除
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
        /// 删除记录
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="recID"></param>
        public void deleteRecord(IStaffTargetInfo staff, int recID)
        {
            MyLists listtable = new MyLists();
            List listdeldetail = new List();
            string sql;
            // 删除一条纪录
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

		#region --模板方法-- 
		/// <summary>
		/// 取得所有记录
		/// </summary>
		/// <returns></returns>
		public DataTable GetAllRecord()
		{
            string sql = string.Format("SELECT a.*,nvl(b.sort_no,9999) sort_no FROM {0}." + this.TabName + " a,comm.sys_dept_dict b WHERE a.Deleted = 0 and a.dept_code=b.dept_code(+) order by a.LASTEDITDATE desc", DataUser.ZLGL);

			DataSet dataset = OracleOledbBase.ExecuteDataSet(sql);
			return dataset.Tables[0];
		}
		/// <summary>
		/// 某科室记录
		/// </summary>
		/// <returns></returns>
		public DataTable GetAllRecorddept(string deptcode,int id)
		{
			string sql = string.Format("SELECT * FROM {0}." + this.TabName + " WHERE Deleted = 0 and (flag='"+deptcode+"' or dept_id_"+id.ToString()+"='"+deptcode+"')",DataUser.ZLGL);

			DataSet dataset = OracleOledbBase.ExecuteDataSet(sql);

			return dataset.Tables[0];
		}

		/// <summary>
		/// 取得某个人修改过的所有记录
		/// </summary>
		/// <returns></returns>
		public DataTable GetAllRecordByInputStaff(string name)
		{
			string sql = string.Format("SELECT * FROM {0}." + this.TabName + " WHERE (Deleted = 0) AND (CreateStaff = '" + name + "') OR (Deleted = 0) AND (LastEditStaff = '" + name + "')",DataUser.ZLGL);

			DataSet dataset = OracleOledbBase.ExecuteDataSet(sql);

			return dataset.Tables[0];
		}
        /// <summary>
        /// 扣分分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetQuality()
        {
            string str = string.Format("select * from {0}.quality_type", DataUser.ZLGL);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }
		
		/// <summary>
		/// 在模板中插入一条记录
		/// </summary>
		/// <param name="curPage">包含录入数据的页面</param>
		/// <param name="staff">人员对象</param>
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

                // 插入一条新纪录
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


                // 依次更新每一条记录，如果出现异常，删除记录并抛出异常。

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
		/// 修改一条模板记录
		/// </summary>
		/// <param name="curPage">包含修改数据的页面</param>
		/// <param name="staff">修改人</param>
		/// <param name="recID">修改记录</param>
		public void UpdataRecord(System.Web.UI.Page curPage, IStaffTargetInfo staff, int recID,int id)
		{
            OleDbConnection myConnection = new OleDbConnection(CONNECT_STRING);
            myConnection.Open();
            OleDbTransaction myTrans = myConnection.BeginTransaction();
            try
            {
                string sql;
                // 修改一条纪录
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
                // 依次更新每一条记录，如果出现异常，删除记录并抛出异常。

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
		/// 获取一条记录的修改信息
		/// </summary>
		/// <param name="recID">记录号</param>
		/// <returns>修改信息</returns>
		public string GetModifyInfo(int recID)
		{
			string result = string.Empty;
			string sql = string.Format("SELECT CreateStaff, CreateDate, LastEditStaff, LastEditDate FROM {0}." + this.TabName + " WHERE (ID = ?)",DataUser.ZLGL);

			DataTable reader = OracleOledbBase.ExecuteDataSet( sql, new OleDbParameter("recID", recID)).Tables[0];

			for (int i=0;i<reader.Rows.Count;i++)
			{
				result += "记录创建者: " + reader.Rows[i][0].ToString() + "   创建时间:" + reader.Rows[i][1].ToString() 
					+ "\r\n最后修改人: " + reader.Rows[i][2].ToString() + "   修改时间:" + reader.Rows[i][3].ToString();
			}
			//reader.Close();

			return result;
		}
		#endregion

		#region --指标系统使用的方法-- 
		/// <summary>
		/// 得到所有日期列
		/// </summary>
		/// <returns></returns>
		public FieldCollectionTable GetDateFields()
		{
			return getFieldsByTypes("日期");
		}

		/// <summary>
		/// 得到所有部门列
		/// </summary>
		/// <returns></returns>
		public FieldCollectionTable GetDeptFields()
		{
			return getFieldsByTypes("部门");
		}
       
		/// <summary>
		/// 得到所有数字列
		/// </summary>
		/// <returns></returns>
		public FieldCollectionTable GetNumFields()
		{
			return getFieldsByTypes("数字");
		}

	/// <summary>
	/// 指标
	/// </summary>
	/// <returns></returns>
		public FieldCollectionTable GetGuidFields()
		{
			return getFieldsByTypes("指标");
		}

        

		/// <summary>
		/// 对模板数据作统计
		/// </summary>
		/// <param name="dateFieldID">时间字段编号</param>
		/// <param name="deptFieldID">科室字段编号</param>
		/// <param name="numberFieldID">数字字段编号</param>
		/// <param name="startDate">开始时间</param>
		/// <param name="endDate">结束时间</param>
		/// <param name="GroupCode">科室编号</param>
		/// <returns>SUM()值</returns>
		/// <exception cref="TempletException">模板异常:统计出错!</exception>
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
				throw new TempletException("数据统计时发生异常!", 
					"模板:" + this.Name + "在统计数据:时间为(" 
					+ startDate.ToShortDateString() + "-" 
					+ endDate.ToShortDateString() + ")在字段编号:" 
					+ dateFieldID.ToString() 
					+ "; 科室编号(" + GroupCode + ") 在字段编号:" 
					+ deptFieldID.ToString() 
					+ "; 时发生以下错误：" + ex.Message,
					ex
					);
			}

			return result;
		}

		/// <summary>
		/// 删除多条记录
		/// </summary>
		/// <param name="ids">记录编号：如：1,2,3这样的字符串</param>
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
