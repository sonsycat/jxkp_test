using System;
using System.Data;
using System.Data.SqlClient;
using GoldNet.Comm;

namespace GoldNet.Comm.DAL.SqlServer
{
	/// <summary>
	/// 定义一个结构体，用来传递需要执行事务的参数
	/// </summary>
	public struct forTrans
	{
		/// <summary>
		/// 命令字符串
		/// </summary>
		public string comStr;
		/// <summary>
		/// 命令的参数集合
		/// </summary>
		public System.Collections.ArrayList paramList;
	}
	/// <summary>
	/// 通用数据访问类
	/// 包含对数据库访问的基本方法
	/// 虚拟类，不能实例化，不要继承此类
	/// </summary>
	public class DALBase
	{
		private DALBase(){}

		/// <summary>
		/// 只读数据库连接字符串：以后改为配置文件设置。
		/// </summary>
		// public static string CONN_STRING_NON_DTC = "workstation id=Goldnet;packet size=4096;user id=sa;data source=10.0.0.15;persist security info=True;initial catalog=performance251;password=gaoshan;Connection Lifetime=60;Max Pool Size=3;Min Pool Size=0;";
        //public static string CONN_STRING_NON_DTC = "user id=sa;data source=10.0.0.121;persist security info=True;initial catalog=performance251;password=sa;Connection Lifetime=60;Max Pool Size=3;Min Pool Size=0;";
        public static string CONN_STRING_NON_DTC = System.Configuration.ConfigurationManager.AppSettings["costsql"];

		/// <summary>
		/// 无返回值的数据库操作：Insert、Delete、Update；
		/// </summary>
		/// <param name="connStr">连接字符串</param>
		/// <param name="cmdText">要执行的SQL语句模板（带参数）</param>
		/// <param name="cmdParms">此SQL语句的参数</param>
		/// <returns>影响的行数</returns>
		/// 
		public static int ExecuteNonQuery(string connStr, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				// 初始化SqlCommand对象
				PrepareCommand (cmd, conn, null, CommandType.Text, cmdText, cmdParms);
 
				int val = cmd.ExecuteNonQuery();
				conn.Close();
				cmd.Parameters.Clear();
				return val;
			}
		}

		/// <summary>
		/// 无返回值的数据库操作：Insert、Delete、Update；
		/// </summary>
		/// <param name="cmdText">要执行的SQL语句模板（带参数）</param>
		/// <param name="cmdParms">此SQL语句的参数</param>
		/// <returns>影响的行数</returns>
		public static int ExecuteNonQuery(string cmdText, params SqlParameter[] cmdParms) 
		{
			return ExecuteNonQuery(CONN_STRING_NON_DTC, cmdText, cmdParms);
		}

		/// <summary>
		/// 无返回值的数据库操作：Insert、Delete、Update；
		/// </summary>
		/// <param name="cmdText">要执行的SQL语句</param>
		/// <returns>影响的行数</returns>
		public static int ExecuteNonQuery(string cmdText)
		{
			SqlParameter[] cmdParms = new SqlParameter[0]{};

			return ExecuteNonQuery(cmdText, cmdParms);
		}

		/// <summary>
		/// 支持事务处理的数据库操作,适用INSERT、DELETE、UPDATE语句
		/// </summary>
		/// <param name="trans">事务处理对象</param>
		/// <param name="cmdType">命令对象类型</param>
		/// <param name="cmdText">命令对象内容</param>
		/// <param name="cmdParms">命令参数集合</param>
		/// <returns>影响的行数</returns>
		public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
			int val = cmd.ExecuteNonQuery();
			cmd.Parameters.Clear();
			return val;
		}

		/// <summary>
		/// 返回单值的数据库操作
		/// </summary>
		/// <param name="trans">事务处理对象</param>
		/// <param name="cmdType">命令对象类型</param>
		/// <param name="cmdText">要执行的SQL语句</param>
		/// <param name="cmdParms">此SQL语句的参数</param>
		/// <returns>返回的第一行第一列的值</returns>
		public static object ExecuteScalar(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
			object val = cmd.ExecuteScalar();
			cmd.Parameters.Clear();
			return val;
		}

		/// <summary>
		/// 返回单值的数据库操作
		/// </summary>
		/// <param name="connStr">数据库连接字符串</param>
		/// <param name="cmdText">要执行的SQL语句</param>
		/// <param name="cmdParms">此SQL语句的参数</param>
		/// <returns>返回的第一行第一列的值</returns>
		/// 
		public static object ExecuteScalar(string connStr, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();
		
			using (SqlConnection conn = new SqlConnection(connStr)) 
			{
				conn.Open();

				// 初始化SqlCommand对象
				PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);

				object val = cmd.ExecuteScalar();
				conn.Close();
				cmd.Parameters.Clear();
				return val;
			}
		}

		/// <summary>
		/// 返回单值的数据库操作
		/// </summary>
		/// <param name="cmdText">要执行的SQL语句</param>
		/// <param name="cmdParms">此SQL语句的参数</param>
		/// <returns>返回的第一行第一列的值</returns>
		public static object ExecuteScalar(string cmdText, params SqlParameter[] cmdParms) 
		{
			return ExecuteScalar(CONN_STRING_NON_DTC, cmdText, cmdParms);
		}

		/// <summary>
		/// 返回单值的数据库操作
		/// </summary>
		/// <param name="cmdText">要执行的SQL语句</param>
		/// <returns>返回的第一行第一列的值</returns>
		public static object ExecuteScalar(string cmdText) 
		{
			SqlParameter[] cmdParms = new SqlParameter[]{};

			return ExecuteScalar(cmdText, cmdParms);
		}

		/// <summary>
		/// 返回一个DataSet对象的查询操作：Select
		/// </summary>
		/// <param name="connStr">数据库连接字符串</param>
		/// <param name="cmdText">要执行的 Select 语句模板（带参数）</param>
		/// <param name="cmdParms">此SQL语句的参数</param>
		/// <returns>满足条件的DataSet</returns>
		public static DataSet ExecuteDataSet(string connStr, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();
				
				DataSet rtds = new DataSet();

				// 初始化一个SqlCommand对象.
				PrepareCommand (cmd, conn, null, CommandType.Text, cmdText, cmdParms);

				// 创建一个使用此SqlCommand的适配器
				SqlDataAdapter sda = new SqlDataAdapter(cmd);

				sda.Fill(rtds);
				rtds.AcceptChanges();
				rtds.GetChanges();

				conn.Close();
				cmd.Parameters.Clear();
				return rtds;
			}
		}

		/// <summary>
		/// 返回一个DataSet对象的查询操作：Select
		/// </summary>
		/// <param name="cmdText">要执行的 Select 语句模板（带参数）</param>
		/// <param name="cmdParms">此SQL语句的参数</param>
		/// <returns>满足条件的DataSet</returns>
		public static DataSet ExecuteDataSet(string cmdText, params SqlParameter[] cmdParms) 
		{
			return ExecuteDataSet (CONN_STRING_NON_DTC, cmdText, cmdParms);
		}

		/// <summary>
		/// 返回一个DataSet对象的查询操作：Select
		/// </summary>
		/// <param name="cmdText">要执行的 Select 语句</param>
		/// <returns>满足条件的DataSet</returns>
		public static DataSet ExecuteDataSet(string cmdText) 
		{
			SqlParameter[] cmdParms = new SqlParameter[0]{};

			return ExecuteDataSet (cmdText, cmdParms);
		}

		/// <summary>
		/// 返回一个SqlDataReader对象的查询操作：Select
		/// </summary>
		/// <param name="connStr">数据库连接字符串</param>
		/// <param name="cmdText">要执行的 Select 语句模板（带参数）</param>
		/// <param name="cmdParms">此SQL语句的参数</param>
		/// <returns>满足条件的SqlDataReader</returns>
		public static SqlDataReader ExecuteReader(string connStr, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();
			SqlConnection conn = new SqlConnection(connStr);

			// 放置try来捕获错误并关闭数据库联接
			try 
			{
				PrepareCommand (cmd, conn, null, CommandType.Text, cmdText, cmdParms);

				SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				cmd.Parameters.Clear();
				return rdr;
			}
			catch 
			{
				conn.Close();
				throw;
			}
		}

		/// <summary>
		/// 返回一个SqlDataReader对象的查询操作：Select
		/// </summary>
		/// <param name="cmdText">要执行的 Select 语句模板（带参数）</param>
		/// <param name="cmdParms">此SQL语句的参数</param>
		/// <returns>满足条件的SqlDataReader</returns>
		public static SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] cmdParms) 
		{
			return ExecuteReader(CONN_STRING_NON_DTC, cmdText, cmdParms);
		}

		/// <summary>
		/// 为SqlCommand命令准备参数
		/// </summary>
		/// <param name="cmd">要执行的SqlCommand命令对象</param>
		/// <param name="conn">数据库连接对象</param>
		/// <param name="trans">连接事务对象</param>
		/// <param name="cmdType">连接类型：一般是Text(SQL)</param>
		/// <param name="cmdText">命令字符串</param>
		/// <param name="cmdParms">参数数组</param>
		private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms) 
		{
			if (conn.State != ConnectionState.Open)
				conn.Open();

			cmd.Connection = conn;
			cmd.CommandText = cmdText;

			if (trans != null)
				cmd.Transaction = trans;

			cmd.CommandType = cmdType;

			if (cmdParms != null) 
			{
				foreach (SqlParameter parm in cmdParms)
					cmd.Parameters.Add(parm);
			}
		}

		/// <summary>
		/// 更新一个记录集到数据源
		/// </summary>
		/// <param name="connStr">数据库连接字符串</param>
		/// <param name="newDS">要更新的记录集</param>
		/// <param name="cmdText">取得记录集的命令</param>
		/// <param name="cmdParms">取得记录集的参数</param>
		public static void UpdataDataSet(string connStr, DataSet newDS, string cmdText, params SqlParameter[] cmdParms)
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();
				
				// 初始化一个SqlCommand对象.
				PrepareCommand (cmd, conn, null, CommandType.Text, cmdText, cmdParms);

				// 创建一个使用此SqlCommand的适配器
				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.UpdateCommand = new SqlCommandBuilder(sda).GetUpdateCommand();

				// 更新记录集
				sda.Update(newDS);

				// 注销对象
				conn.Close();
				cmd.Parameters.Clear();
				conn.Dispose();
				sda.Dispose();
			}
			cmd.Dispose();
		}

		/// <summary>
		/// 更新一个记录集到数据源
		/// </summary>
		/// <param name="newDS">要更新的记录集</param>
		/// <param name="cmdText">取得记录集的命令</param>
		/// <param name="cmdParms">取得记录集的参数</param>
		public static void UpdataDataSet(DataSet newDS, string cmdText, params SqlParameter[] cmdParms)
		{
			UpdataDataSet(CONN_STRING_NON_DTC,newDS, cmdText, cmdParms);
		}

		/// <summary>
		/// 调用事务来批处理多条命令.
		/// 
		/// 调用事务处理thetrans（）方法的事例
		/// <code>
		/// 	string wn = "wn";
		/// 	string zxm = "zxm";
		/// 	forTrans[] theCmd = new forTrans [2];
		/// 	theCmd[0].comStr = "Insert into wn_test (w,n) VALUES (@w,@n)";
		/// 	theCmd[0].paramList = new System.Collections.ArrayList ();
		/// 	theCmd[0].paramList.Add(new SqlParameter ("@w",wn));
		/// 	theCmd[0].paramList.Add(new SqlParameter ("@n",zxm));
		/// 	
		/// 	theCmd[1].comStr = "Insert into wn_test (w,n) VALUES (@w,@n)";
		/// 	theCmd[1].paramList = new System.Collections.ArrayList ();
		/// 	theCmd[1].paramList.Add(new SqlParameter ("@w",zxm));
		/// 	theCmd[1].paramList.Add(new SqlParameter ("@n",wn));
		/// 	
		///		ExecuteTrans(theCmd);
		/// </code>
		/// </summary>
		/// <param name="connStr">数据库连接字符串</param>
		/// <param name="cmd">要执行的命令数组</param>
		/// <returns>是否成功执行</returns>
		public static bool ExecuteTrans(string connStr,forTrans[] cmd)
		{
			bool pass = false;

			SqlConnection myConnection = new SqlConnection(connStr);
			myConnection.Open();

			// 开始一个本地事务
			SqlTransaction myTrans = myConnection.BeginTransaction();

			// 命令与事务关联
			SqlCommand myCommand = myConnection.CreateCommand();
			myCommand.Transaction = myTrans;


			try
			{
				int i;
				for (i=0;i<cmd.Length;i++)//根据参数初始化命令并执行，呵呵
				{
					myCommand.CommandText = cmd[i].comStr;
					if (cmd[i].paramList != null) 
					{
						foreach (SqlParameter parm in cmd[i].paramList)
							myCommand.Parameters.Add(parm);
					}

					myCommand.ExecuteNonQuery();
					myCommand.Parameters.Clear();

				}
				myTrans.Commit();
				pass = true;
			}
			catch(Exception e)
			{
				try
				{
					myTrans.Rollback();
				}
				catch (SqlException ex)
				{
					if (myTrans.Connection != null)
					{
						//这里必须抛出个异常提醒用户 数据的完整性遭到破坏了，呵呵
						throw new GlobalException("回滚过程中出现异常：", "数据更新出现错误，您的数据完整性可能遭到破坏，请尝试数据库级别的恢复！", ex);
					}
				}

				pass = false;
				throw new GlobalException("数据处理失败： ", "数据更新出现错误，请分析错误原因并重试，错误信息为：" + e.Message);
				
			}
			finally
			{
				myConnection.Close();
				myCommand.Parameters.Clear();
				myCommand.Dispose();
				myConnection.Dispose();
			}

			return pass;
		}

		/// <summary>
		/// 更新一个记录集到数据源
		/// </summary>
		/// <param name="cmd">要执行的命令数组</param>
		/// <returns>r</returns>
		public static bool ExecuteTrans(forTrans[] cmd)
		{
			return ExecuteTrans (CONN_STRING_NON_DTC, cmd);
		}


		/// <summary>
		/// 填充一个表集合类
		/// </summary>
		/// <param name="table">要填充的表格</param>
		/// <param name="connStr">连接字符串</param>
		/// <param name="cmdText">SQL语句</param>
		/// <param name="cmdParms">SQL参数列表</param>
		public static void FillTheTable(CollectionTable table, string connStr, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				// 初始化一个SqlCommand对象.
				PrepareCommand (cmd, conn, null, CommandType.Text, cmdText, cmdParms);

				// 创建一个使用此SqlCommand的适配器
				SqlDataAdapter sda = new SqlDataAdapter(cmd);

				sda.Fill(table);

				conn.Close();
				cmd.Parameters.Clear();
			}
		}

	}
}
