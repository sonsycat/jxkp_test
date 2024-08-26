using System;
using System.Data;
using System.Data.SqlClient;
using GoldNet.Comm;

namespace GoldNet.Comm.DAL.SqlServer
{
	/// <summary>
	/// ����һ���ṹ�壬����������Ҫִ������Ĳ���
	/// </summary>
	public struct forTrans
	{
		/// <summary>
		/// �����ַ���
		/// </summary>
		public string comStr;
		/// <summary>
		/// ����Ĳ�������
		/// </summary>
		public System.Collections.ArrayList paramList;
	}
	/// <summary>
	/// ͨ�����ݷ�����
	/// ���������ݿ���ʵĻ�������
	/// �����࣬����ʵ��������Ҫ�̳д���
	/// </summary>
	public class DALBase
	{
		private DALBase(){}

		/// <summary>
		/// ֻ�����ݿ������ַ������Ժ��Ϊ�����ļ����á�
		/// </summary>
		// public static string CONN_STRING_NON_DTC = "workstation id=Goldnet;packet size=4096;user id=sa;data source=10.0.0.15;persist security info=True;initial catalog=performance251;password=gaoshan;Connection Lifetime=60;Max Pool Size=3;Min Pool Size=0;";
        //public static string CONN_STRING_NON_DTC = "user id=sa;data source=10.0.0.121;persist security info=True;initial catalog=performance251;password=sa;Connection Lifetime=60;Max Pool Size=3;Min Pool Size=0;";
        public static string CONN_STRING_NON_DTC = System.Configuration.ConfigurationManager.AppSettings["costsql"];

		/// <summary>
		/// �޷���ֵ�����ݿ������Insert��Delete��Update��
		/// </summary>
		/// <param name="connStr">�����ַ���</param>
		/// <param name="cmdText">Ҫִ�е�SQL���ģ�壨��������</param>
		/// <param name="cmdParms">��SQL���Ĳ���</param>
		/// <returns>Ӱ�������</returns>
		/// 
		public static int ExecuteNonQuery(string connStr, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				// ��ʼ��SqlCommand����
				PrepareCommand (cmd, conn, null, CommandType.Text, cmdText, cmdParms);
 
				int val = cmd.ExecuteNonQuery();
				conn.Close();
				cmd.Parameters.Clear();
				return val;
			}
		}

		/// <summary>
		/// �޷���ֵ�����ݿ������Insert��Delete��Update��
		/// </summary>
		/// <param name="cmdText">Ҫִ�е�SQL���ģ�壨��������</param>
		/// <param name="cmdParms">��SQL���Ĳ���</param>
		/// <returns>Ӱ�������</returns>
		public static int ExecuteNonQuery(string cmdText, params SqlParameter[] cmdParms) 
		{
			return ExecuteNonQuery(CONN_STRING_NON_DTC, cmdText, cmdParms);
		}

		/// <summary>
		/// �޷���ֵ�����ݿ������Insert��Delete��Update��
		/// </summary>
		/// <param name="cmdText">Ҫִ�е�SQL���</param>
		/// <returns>Ӱ�������</returns>
		public static int ExecuteNonQuery(string cmdText)
		{
			SqlParameter[] cmdParms = new SqlParameter[0]{};

			return ExecuteNonQuery(cmdText, cmdParms);
		}

		/// <summary>
		/// ֧������������ݿ����,����INSERT��DELETE��UPDATE���
		/// </summary>
		/// <param name="trans">���������</param>
		/// <param name="cmdType">�����������</param>
		/// <param name="cmdText">�����������</param>
		/// <param name="cmdParms">�����������</param>
		/// <returns>Ӱ�������</returns>
		public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
			int val = cmd.ExecuteNonQuery();
			cmd.Parameters.Clear();
			return val;
		}

		/// <summary>
		/// ���ص�ֵ�����ݿ����
		/// </summary>
		/// <param name="trans">���������</param>
		/// <param name="cmdType">�����������</param>
		/// <param name="cmdText">Ҫִ�е�SQL���</param>
		/// <param name="cmdParms">��SQL���Ĳ���</param>
		/// <returns>���صĵ�һ�е�һ�е�ֵ</returns>
		public static object ExecuteScalar(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
			object val = cmd.ExecuteScalar();
			cmd.Parameters.Clear();
			return val;
		}

		/// <summary>
		/// ���ص�ֵ�����ݿ����
		/// </summary>
		/// <param name="connStr">���ݿ������ַ���</param>
		/// <param name="cmdText">Ҫִ�е�SQL���</param>
		/// <param name="cmdParms">��SQL���Ĳ���</param>
		/// <returns>���صĵ�һ�е�һ�е�ֵ</returns>
		/// 
		public static object ExecuteScalar(string connStr, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();
		
			using (SqlConnection conn = new SqlConnection(connStr)) 
			{
				conn.Open();

				// ��ʼ��SqlCommand����
				PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);

				object val = cmd.ExecuteScalar();
				conn.Close();
				cmd.Parameters.Clear();
				return val;
			}
		}

		/// <summary>
		/// ���ص�ֵ�����ݿ����
		/// </summary>
		/// <param name="cmdText">Ҫִ�е�SQL���</param>
		/// <param name="cmdParms">��SQL���Ĳ���</param>
		/// <returns>���صĵ�һ�е�һ�е�ֵ</returns>
		public static object ExecuteScalar(string cmdText, params SqlParameter[] cmdParms) 
		{
			return ExecuteScalar(CONN_STRING_NON_DTC, cmdText, cmdParms);
		}

		/// <summary>
		/// ���ص�ֵ�����ݿ����
		/// </summary>
		/// <param name="cmdText">Ҫִ�е�SQL���</param>
		/// <returns>���صĵ�һ�е�һ�е�ֵ</returns>
		public static object ExecuteScalar(string cmdText) 
		{
			SqlParameter[] cmdParms = new SqlParameter[]{};

			return ExecuteScalar(cmdText, cmdParms);
		}

		/// <summary>
		/// ����һ��DataSet����Ĳ�ѯ������Select
		/// </summary>
		/// <param name="connStr">���ݿ������ַ���</param>
		/// <param name="cmdText">Ҫִ�е� Select ���ģ�壨��������</param>
		/// <param name="cmdParms">��SQL���Ĳ���</param>
		/// <returns>����������DataSet</returns>
		public static DataSet ExecuteDataSet(string connStr, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();
				
				DataSet rtds = new DataSet();

				// ��ʼ��һ��SqlCommand����.
				PrepareCommand (cmd, conn, null, CommandType.Text, cmdText, cmdParms);

				// ����һ��ʹ�ô�SqlCommand��������
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
		/// ����һ��DataSet����Ĳ�ѯ������Select
		/// </summary>
		/// <param name="cmdText">Ҫִ�е� Select ���ģ�壨��������</param>
		/// <param name="cmdParms">��SQL���Ĳ���</param>
		/// <returns>����������DataSet</returns>
		public static DataSet ExecuteDataSet(string cmdText, params SqlParameter[] cmdParms) 
		{
			return ExecuteDataSet (CONN_STRING_NON_DTC, cmdText, cmdParms);
		}

		/// <summary>
		/// ����һ��DataSet����Ĳ�ѯ������Select
		/// </summary>
		/// <param name="cmdText">Ҫִ�е� Select ���</param>
		/// <returns>����������DataSet</returns>
		public static DataSet ExecuteDataSet(string cmdText) 
		{
			SqlParameter[] cmdParms = new SqlParameter[0]{};

			return ExecuteDataSet (cmdText, cmdParms);
		}

		/// <summary>
		/// ����һ��SqlDataReader����Ĳ�ѯ������Select
		/// </summary>
		/// <param name="connStr">���ݿ������ַ���</param>
		/// <param name="cmdText">Ҫִ�е� Select ���ģ�壨��������</param>
		/// <param name="cmdParms">��SQL���Ĳ���</param>
		/// <returns>����������SqlDataReader</returns>
		public static SqlDataReader ExecuteReader(string connStr, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();
			SqlConnection conn = new SqlConnection(connStr);

			// ����try��������󲢹ر����ݿ�����
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
		/// ����һ��SqlDataReader����Ĳ�ѯ������Select
		/// </summary>
		/// <param name="cmdText">Ҫִ�е� Select ���ģ�壨��������</param>
		/// <param name="cmdParms">��SQL���Ĳ���</param>
		/// <returns>����������SqlDataReader</returns>
		public static SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] cmdParms) 
		{
			return ExecuteReader(CONN_STRING_NON_DTC, cmdText, cmdParms);
		}

		/// <summary>
		/// ΪSqlCommand����׼������
		/// </summary>
		/// <param name="cmd">Ҫִ�е�SqlCommand�������</param>
		/// <param name="conn">���ݿ����Ӷ���</param>
		/// <param name="trans">�����������</param>
		/// <param name="cmdType">�������ͣ�һ����Text(SQL)</param>
		/// <param name="cmdText">�����ַ���</param>
		/// <param name="cmdParms">��������</param>
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
		/// ����һ����¼��������Դ
		/// </summary>
		/// <param name="connStr">���ݿ������ַ���</param>
		/// <param name="newDS">Ҫ���µļ�¼��</param>
		/// <param name="cmdText">ȡ�ü�¼��������</param>
		/// <param name="cmdParms">ȡ�ü�¼���Ĳ���</param>
		public static void UpdataDataSet(string connStr, DataSet newDS, string cmdText, params SqlParameter[] cmdParms)
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();
				
				// ��ʼ��һ��SqlCommand����.
				PrepareCommand (cmd, conn, null, CommandType.Text, cmdText, cmdParms);

				// ����һ��ʹ�ô�SqlCommand��������
				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.UpdateCommand = new SqlCommandBuilder(sda).GetUpdateCommand();

				// ���¼�¼��
				sda.Update(newDS);

				// ע������
				conn.Close();
				cmd.Parameters.Clear();
				conn.Dispose();
				sda.Dispose();
			}
			cmd.Dispose();
		}

		/// <summary>
		/// ����һ����¼��������Դ
		/// </summary>
		/// <param name="newDS">Ҫ���µļ�¼��</param>
		/// <param name="cmdText">ȡ�ü�¼��������</param>
		/// <param name="cmdParms">ȡ�ü�¼���Ĳ���</param>
		public static void UpdataDataSet(DataSet newDS, string cmdText, params SqlParameter[] cmdParms)
		{
			UpdataDataSet(CONN_STRING_NON_DTC,newDS, cmdText, cmdParms);
		}

		/// <summary>
		/// �����������������������.
		/// 
		/// ����������thetrans��������������
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
		/// <param name="connStr">���ݿ������ַ���</param>
		/// <param name="cmd">Ҫִ�е���������</param>
		/// <returns>�Ƿ�ɹ�ִ��</returns>
		public static bool ExecuteTrans(string connStr,forTrans[] cmd)
		{
			bool pass = false;

			SqlConnection myConnection = new SqlConnection(connStr);
			myConnection.Open();

			// ��ʼһ����������
			SqlTransaction myTrans = myConnection.BeginTransaction();

			// �������������
			SqlCommand myCommand = myConnection.CreateCommand();
			myCommand.Transaction = myTrans;


			try
			{
				int i;
				for (i=0;i<cmd.Length;i++)//���ݲ�����ʼ�����ִ�У��Ǻ�
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
						//��������׳����쳣�����û� ���ݵ��������⵽�ƻ��ˣ��Ǻ�
						throw new GlobalException("�ع������г����쳣��", "���ݸ��³��ִ����������������Կ����⵽�ƻ����볢�����ݿ⼶��Ļָ���", ex);
					}
				}

				pass = false;
				throw new GlobalException("���ݴ���ʧ�ܣ� ", "���ݸ��³��ִ������������ԭ�����ԣ�������ϢΪ��" + e.Message);
				
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
		/// ����һ����¼��������Դ
		/// </summary>
		/// <param name="cmd">Ҫִ�е���������</param>
		/// <returns>r</returns>
		public static bool ExecuteTrans(forTrans[] cmd)
		{
			return ExecuteTrans (CONN_STRING_NON_DTC, cmd);
		}


		/// <summary>
		/// ���һ��������
		/// </summary>
		/// <param name="table">Ҫ���ı��</param>
		/// <param name="connStr">�����ַ���</param>
		/// <param name="cmdText">SQL���</param>
		/// <param name="cmdParms">SQL�����б�</param>
		public static void FillTheTable(CollectionTable table, string connStr, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				// ��ʼ��һ��SqlCommand����.
				PrepareCommand (cmd, conn, null, CommandType.Text, cmdText, cmdParms);

				// ����һ��ʹ�ô�SqlCommand��������
				SqlDataAdapter sda = new SqlDataAdapter(cmd);

				sda.Fill(table);

				conn.Close();
				cmd.Parameters.Clear();
			}
		}

	}
}
