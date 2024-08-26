using System;
using System.Data;
using System.Collections;
using System.Data.OleDb;

namespace GoldNet.Comm.DAL.Oracle
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
    /// ����
    /// </summary>
    public class MyLists : ArrayList
    {
        public MyLists()
        {
        }
        public new List this[int index]
        {
            get
            {
                return (List)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
        public int Add(List value)
        {
            return base.Add(value);
        }
    }
    public class List
    {
        public List()
        {
        }
        private object m_strsql;
        private object m_parameter;
        public object StrSql
        {
            get
            {
                return this.m_strsql;
            }
            set
            {
                this.m_strsql = value;
            }
        }

        public object Parameters
        {
            get
            {
                return this.m_parameter;
            }
            set
            {
                this.m_parameter = value;
            }
        }
    }


    /// <summary>
    /// OracleOledbBase ��ժҪ˵����
    /// </summary>
    public class OracleOledbBase
    {
        /// <summary>
        /// ��Ч�����ַ���
        /// </summary>
        public static string ConnString = System.Configuration.ConfigurationManager.AppSettings["OledbConnString"];
        public static string CountNumber = System.Configuration.ConfigurationManager.AppSettings["Score"];
        /// <summary>
        /// ����һ��DataSet����Ĳ�ѯ������Select
        /// </summary>
        /// <param name="connStr">���ݿ������ַ���</param>
        /// <param name="cmdText">Ҫִ�е� Select ���ģ�壨��������</param>
        /// <param name="cmdParms">��SQL���Ĳ���</param>
        /// <returns>����������DataSet</returns>
        public static DataSet ExecuteDataSet(string connStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandTimeout = 120;
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                try
                {
                    conn.Open();
                }
                catch (System.Exception e)
                {
                    string stre = e.ToString();
                }

                DataSet rtds = new DataSet();

                // ��ʼ��һ��OleDbCommand����.
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);

                // ����һ��ʹ�ô�OleDbCommand��������
                OleDbDataAdapter sda = new OleDbDataAdapter(cmd);

                try
                {
                    sda.Fill(rtds);
                }
                catch (System.Exception e)
                {
                    string stre = e.ToString();

                }
                rtds.AcceptChanges();
                rtds.GetChanges();

                conn.Close();
                conn.Dispose();
                cmd.Parameters.Clear();
                return rtds;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="connStr"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string tablename, string connStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();


            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();

                DataSet rtds = new DataSet();

                // ��ʼ��һ��OleDbCommand����.
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);

                // ����һ��ʹ�ô�OleDbCommand��������
                OleDbDataAdapter sda = new OleDbDataAdapter(cmd);

                sda.Fill(rtds, tablename);
                rtds.AcceptChanges();
                rtds.GetChanges();

                conn.Close();
                cmd.Parameters.Clear();
                return rtds;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static bool Exists(string strSql, params OleDbParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="strSql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static bool Exists(string conn, string strSql, params OleDbParameter[] cmdParms)
        {
            object obj = GetSingle(conn, strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="FieldName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static int GetMaxID(string conn, string FieldName, string TableName)
        {
            string strsql = "select max(to_number(" + FieldName + "))+1 from " + TableName;
            OleDbParameter[] cmdParms = new OleDbParameter[] { };
            object obj = GetSingle(conn, strsql, cmdParms);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(to_number(" + FieldName + "))+1 from " + TableName;

            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        /// <summary>
        /// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
        /// </summary>
        /// <param name="SQLString">�����ѯ������</param>
        /// <returns>��ѯ�����object��</returns>
        public static object GetSingle(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnString))
            {
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OracleClient.OracleException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
        /// </summary>
        /// <param name="SQLString">�����ѯ������</param>
        /// <returns>��ѯ�����object��</returns>
        public static object GetSingle(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {  //OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, CommandType cmdType, string cmdText,
                        PrepareCommand(cmd, connection, null, CommandType.Text, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OracleClient.OracleException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static object GetSingle(string conn, string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(conn))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {  //OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, CommandType cmdType, string cmdText,
                        PrepareCommand(cmd, connection, null, CommandType.Text, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OracleClient.OracleException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// ����һ��DataSet����Ĳ�ѯ������Select
        /// </summary>
        /// <param name="cmdText">Ҫִ�е� Select ���ģ�壨��������</param>
        /// <param name="cmdParms">��SQL���Ĳ���</param>
        /// <returns>����������DataSet</returns>
        public static DataSet ExecuteDataSet(string cmdText, params OleDbParameter[] cmdParms)
        {
            return ExecuteDataSet(ConnString, cmdText, cmdParms);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="tablename"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string str1, string str2, string tablename, string cmdText, params OleDbParameter[] cmdParms)
        {
            return ExecuteDataSet(tablename, ConnString, cmdText, cmdParms);
        }

        /// <summary>
        /// ����һ��DataSet����Ĳ�ѯ������Select
        /// </summary>
        /// <param name="cmdText">Ҫִ�е� Select ���</param>
        /// <returns>����������DataSet</returns>
        public static DataSet ExecuteDataSet(string cmdText)
        {
            OleDbParameter[] cmdParms = new OleDbParameter[] { };

            return ExecuteDataSet(cmdText, cmdParms);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string tablename, string cmdText)
        {
            OleDbParameter[] cmdParms = new OleDbParameter[] { };

            return ExecuteDataSet("", "", tablename, cmdText, cmdParms);
        }

        /// <summary>
        /// ΪOleDbCommand����׼������
        /// </summary>
        /// <param name="cmd">Ҫִ�е�OleDbCommand�������</param>
        /// <param name="conn">���ݿ����Ӷ���</param>
        /// <param name="trans">�����������</param>
        /// <param name="cmdType">�������ͣ�һ����Text(SQL)</param>
        /// <param name="cmdText">�����ַ���</param>
        /// <param name="cmdParms">��������</param>
        public static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, CommandType cmdType, string cmdText, OleDbParameter[] cmdParms)
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
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// ����һ��SqlDataReader����Ĳ�ѯ������Select
        /// </summary>
        /// <param name="connStr">���ݿ������ַ���</param>
        /// <param name="cmdText">Ҫִ�е� Select ���ģ�壨��������</param>
        /// <param name="cmdParms">��SQL���Ĳ���</param>
        /// <returns>����������SqlDataReader</returns>
        public static OleDbDataReader ExecuteReader(string connStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection conn = new OleDbConnection(connStr);

            // ����try��������󲢹ر����ݿ�����
            try
            {
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);

                OleDbDataReader rdr = cmd.ExecuteReader();
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
        public static OleDbDataReader ExecuteReader(string cmdText, params OleDbParameter[] cmdParms)
        {
            return ExecuteReader(ConnString, cmdText, cmdParms);
        }

        /// <summary>
        /// �޷���ֵ�����ݿ������Insert��Delete��Update��
        /// </summary>
        /// <param name="connStr">�����ַ���</param>
        /// <param name="cmdText">Ҫִ�е�SQL���ģ�壨��������</param>
        /// <param name="cmdParms">��SQL���Ĳ���</param>
        /// <returns>Ӱ�������</returns>
        /// 
        public static int ExecuteNonQuery(string connStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();

                // ��ʼ��OleDbCommand����
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);

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
        public static int ExecuteNonQuery(string cmdText, params OleDbParameter[] cmdParms)
        {
            return ExecuteNonQuery(ConnString, cmdText, cmdParms);
        }

        /// <summary>
        /// �޷���ֵ�����ݿ������Insert��Delete��Update��
        /// </summary>
        /// <param name="cmdText">Ҫִ�е�SQL���</param>
        /// <returns>Ӱ�������</returns>
        public static int ExecuteNonQuery(string cmdText)
        {
            OleDbParameter[] cmdParms = new OleDbParameter[0] { };

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
        public static int ExecuteNonQuery(OleDbTransaction trans, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
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
        public static object ExecuteScalar(OleDbTransaction trans, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            if (val == null)
                return "";
            else
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
        public static object ExecuteScalar(string connStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();

                // ��ʼ��OleDbCommand����
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);

                object val = cmd.ExecuteScalar();
                conn.Close();
                cmd.Parameters.Clear();
                if (val == null)
                    return "";
                else
                    return val;
            }
        }

        /// <summary>
        /// ���ص�ֵ�����ݿ����
        /// </summary>
        /// <param name="cmdText">Ҫִ�е�SQL���</param>
        /// <param name="cmdParms">��SQL���Ĳ���</param>
        /// <returns>���صĵ�һ�е�һ�е�ֵ</returns>
        public static object ExecuteScalar(string cmdText, params OleDbParameter[] cmdParms)
        {
            return ExecuteScalar(ConnString, cmdText, cmdParms);
        }

        /// <summary>
        /// ���ص�ֵ�����ݿ����
        /// </summary>
        /// <param name="cmdText">Ҫִ�е�SQL���</param>
        /// <returns>���صĵ�һ�е�һ�е�ֵ</returns>
        public static object ExecuteScalar(string cmdText)
        {
            OleDbParameter[] cmdParms = new OleDbParameter[] { };

            return ExecuteScalar(cmdText, cmdParms);
        }

        /// <param name="connStr">���ݿ������ַ���</param>
        /// <param name="cmd">Ҫִ�е���������</param>
        /// <returns>�Ƿ�ɹ�ִ��</returns>
        public static bool ExecuteTrans(string connStr, forTrans[] cmd)
        {
            bool pass = false;

            OleDbConnection myConnection = new OleDbConnection(connStr);
            myConnection.Open();

            // ��ʼһ����������
            OleDbTransaction myTrans = myConnection.BeginTransaction();

            // �������������
            OleDbCommand myCommand = myConnection.CreateCommand();
            myCommand.Transaction = myTrans;

            try
            {
                int i;
                for (i = 0; i < cmd.Length; i++)//���ݲ�����ʼ�����ִ�У��Ǻ�
                {
                    myCommand.CommandText = cmd[i].comStr;
                    if (cmd[i].paramList != null)
                    {
                        foreach (OleDbParameter parm in cmd[i].paramList)
                            myCommand.Parameters.Add(parm);
                    }

                    myCommand.ExecuteNonQuery();
                    myCommand.Parameters.Clear();

                }
                myTrans.Commit();
                pass = true;
            }
            catch (Exception e)
            {
                try
                {
                    myTrans.Rollback();
                }
                catch (OleDbException ex)
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
            return ExecuteTrans(ConnString, cmd);
        }

        /// <summary>
        /// ִ�ж���SQL��䣬ʵ�����ݿ�����
        /// </summary>
        /// <param name="SQLStringList"></param>
        public static void ExecuteSqlTranList(string connStr, MyLists list)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                using (OleDbTransaction myTrans = conn.BeginTransaction())
                {
                    OleDbCommand cmd = new OleDbCommand();
                    try
                    {
                        //ѭ��
                        for (int i = 0; i < list.Count; i++)
                        {
                            string cmdText = ((List)(list[i])).StrSql.ToString();
                            OleDbParameter[] cmdParms = (OleDbParameter[])list[i].Parameters;
                            PrepareCommand(cmd, conn, myTrans, CommandType.Text, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                        }
                        myTrans.Commit();
                    }
                    catch (Exception e)
                    {
                        myTrans.Rollback();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// ִ�ж���SQL��䣬ʵ�����ݿ�����(���ز���ֵ)2024-08-14
        /// </summary>
        /// <param name="SQLStringList"></param>
        public static bool ExecuteSqlTranList_Bool(string connStr, MyLists list)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                using (OleDbTransaction myTrans = conn.BeginTransaction())
                {
                    OleDbCommand cmd = new OleDbCommand();
                    try
                    {
                        //ѭ��
                        for (int i = 0; i < list.Count; i++)
                        {
                            string cmdText = ((List)(list[i])).StrSql.ToString();
                            OleDbParameter[] cmdParms = (OleDbParameter[])list[i].Parameters;
                            PrepareCommand(cmd, conn, myTrans, CommandType.Text, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                        }
                        myTrans.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        myTrans.Rollback();
                        //throw e;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// �����ύ
        /// </summary>
        /// <param name="myTrans"></param>
        /// <param name="list"></param>
        public static void ExecuteSqlTranList(OleDbTransaction myTrans, MyLists list)
        {
            OleDbCommand cmd = new OleDbCommand();

            for (int i = 0; i < list.Count; i++)
            {
                string cmdText = ((List)(list[i])).StrSql.ToString();
                OleDbParameter[] cmdParms = (OleDbParameter[])list[i].Parameters;
                PrepareCommand(cmd, myTrans.Connection, myTrans, CommandType.Text, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public static void ExecuteTranslist(MyLists list)
        {
            ExecuteSqlTranList(ConnString, list);
        }

        public static bool ExecuteTranslist_Bool(MyLists list)
        {
           return  ExecuteSqlTranList_Bool(ConnString, list);
        }

        /// <summary>
        /// �����ύ
        /// </summary>
        /// <param name="myTrans"></param>
        /// <param name="list"></param>
        public static void ExecuteTranslist(OleDbTransaction myTrans, MyLists list)
        {
            ExecuteSqlTranList(myTrans, list);
        }

        /// <summary>
        /// OracleOledbBase ִ�д洢����
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="parameters"></param>
        public static void RunProcedure(string storedProcName, OleDbParameter[] parameters)
        {
            OleDbConnection connection = new OleDbConnection(ConnString);
            connection.Open();
            OleDbCommand command = new OleDbCommand(storedProcName, connection);
            foreach (OleDbParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }

        /// ����Excel��DataGrid
        /// </summary>
        /// <param name="fileName">Excel�ļ���</param>
        /// <param name="sheetName">�������</param>
        /// <returns>����DataSet</returns>
        public static DataSet ImportExcelToDataGrid(string fileName, string sheetName)
        {
            DataSet ds;
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                "Data Source=" + fileName + ";" +
                "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [" + sheetName + "$]", strConn);
            ds = new DataSet();
            myCommand.Fill(ds);
            return ds;
        }

        /// <summary>
        /// ���һ��������
        /// </summary>
        /// <param name="table">Ҫ���ı��</param>
        /// <param name="connStr">�����ַ���</param>
        /// <param name="cmdText">SQL���</param>
        /// <param name="cmdParms">SQL�����б�</param>
        public static void FillTheTable(CollectionTable table, string connStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();

                // ��ʼ��һ��SqlCommand����.
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);

                // ����һ��ʹ�ô�SqlCommand��������
                OleDbDataAdapter sda = new OleDbDataAdapter(cmd);

                sda.Fill(table);

                conn.Close();
                cmd.Parameters.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        public static void FillTheTable(CollectionTable table, string cmdText, params OleDbParameter[] cmdParms)
        {
            FillTheTable(table, ConnString, cmdText, cmdParms);
        }


        //ִ�е�����䷵�ز���ֵ
        public static bool ExecuteSql(string cmdText)
        {
            OleDbParameter[] cmdParms = new OleDbParameter[] { };

            return ExecuteSql(cmdText, cmdParms);
        }

        //ִ�е�����䷵�ز���ֵ
        public static bool ExecuteSql(string sql, OleDbParameter[] cmdParms)
        {
            bool success = false;
            using (OleDbConnection conn = new OleDbConnection(ConnString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                if (cmdParms != null)
                {
                    cmd.Parameters.AddRange(cmdParms);
                }
                try
                {
                    int result = cmd.ExecuteNonQuery();
                    success = true; // ִ�гɹ�
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return success;
        }


    }
}
