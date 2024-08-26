using System;
using System.Data;
using System.Collections;
using System.Data.OleDb;

namespace GoldNet.Comm.DAL.Oracle
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
    /// 数组
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
    /// OracleOledbBase 的摘要说明。
    /// </summary>
    public class OracleOledbBase
    {
        /// <summary>
        /// 绩效连接字符串
        /// </summary>
        public static string ConnString = System.Configuration.ConfigurationManager.AppSettings["OledbConnString"];
        public static string CountNumber = System.Configuration.ConfigurationManager.AppSettings["Score"];
        /// <summary>
        /// 返回一个DataSet对象的查询操作：Select
        /// </summary>
        /// <param name="connStr">数据库连接字符串</param>
        /// <param name="cmdText">要执行的 Select 语句模板（带参数）</param>
        /// <param name="cmdParms">此SQL语句的参数</param>
        /// <returns>满足条件的DataSet</returns>
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

                // 初始化一个OleDbCommand对象.
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);

                // 创建一个使用此OleDbCommand的适配器
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

                // 初始化一个OleDbCommand对象.
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);

                // 创建一个使用此OleDbCommand的适配器
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
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
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
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
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
        /// 返回一个DataSet对象的查询操作：Select
        /// </summary>
        /// <param name="cmdText">要执行的 Select 语句模板（带参数）</param>
        /// <param name="cmdParms">此SQL语句的参数</param>
        /// <returns>满足条件的DataSet</returns>
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
        /// 返回一个DataSet对象的查询操作：Select
        /// </summary>
        /// <param name="cmdText">要执行的 Select 语句</param>
        /// <returns>满足条件的DataSet</returns>
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
        /// 为OleDbCommand命令准备参数
        /// </summary>
        /// <param name="cmd">要执行的OleDbCommand命令对象</param>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="trans">连接事务对象</param>
        /// <param name="cmdType">连接类型：一般是Text(SQL)</param>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdParms">参数数组</param>
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
        /// 返回一个SqlDataReader对象的查询操作：Select
        /// </summary>
        /// <param name="connStr">数据库连接字符串</param>
        /// <param name="cmdText">要执行的 Select 语句模板（带参数）</param>
        /// <param name="cmdParms">此SQL语句的参数</param>
        /// <returns>满足条件的SqlDataReader</returns>
        public static OleDbDataReader ExecuteReader(string connStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection conn = new OleDbConnection(connStr);

            // 放置try来捕获错误并关闭数据库联接
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
        /// 返回一个SqlDataReader对象的查询操作：Select
        /// </summary>
        /// <param name="cmdText">要执行的 Select 语句模板（带参数）</param>
        /// <param name="cmdParms">此SQL语句的参数</param>
        /// <returns>满足条件的SqlDataReader</returns>
        public static OleDbDataReader ExecuteReader(string cmdText, params OleDbParameter[] cmdParms)
        {
            return ExecuteReader(ConnString, cmdText, cmdParms);
        }

        /// <summary>
        /// 无返回值的数据库操作：Insert、Delete、Update；
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        /// <param name="cmdText">要执行的SQL语句模板（带参数）</param>
        /// <param name="cmdParms">此SQL语句的参数</param>
        /// <returns>影响的行数</returns>
        /// 
        public static int ExecuteNonQuery(string connStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();

                // 初始化OleDbCommand对象
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);

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
        public static int ExecuteNonQuery(string cmdText, params OleDbParameter[] cmdParms)
        {
            return ExecuteNonQuery(ConnString, cmdText, cmdParms);
        }

        /// <summary>
        /// 无返回值的数据库操作：Insert、Delete、Update；
        /// </summary>
        /// <param name="cmdText">要执行的SQL语句</param>
        /// <returns>影响的行数</returns>
        public static int ExecuteNonQuery(string cmdText)
        {
            OleDbParameter[] cmdParms = new OleDbParameter[0] { };

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
        public static int ExecuteNonQuery(OleDbTransaction trans, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
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
        /// 返回单值的数据库操作
        /// </summary>
        /// <param name="connStr">数据库连接字符串</param>
        /// <param name="cmdText">要执行的SQL语句</param>
        /// <param name="cmdParms">此SQL语句的参数</param>
        /// <returns>返回的第一行第一列的值</returns>
        /// 
        public static object ExecuteScalar(string connStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();

                // 初始化OleDbCommand对象
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
        /// 返回单值的数据库操作
        /// </summary>
        /// <param name="cmdText">要执行的SQL语句</param>
        /// <param name="cmdParms">此SQL语句的参数</param>
        /// <returns>返回的第一行第一列的值</returns>
        public static object ExecuteScalar(string cmdText, params OleDbParameter[] cmdParms)
        {
            return ExecuteScalar(ConnString, cmdText, cmdParms);
        }

        /// <summary>
        /// 返回单值的数据库操作
        /// </summary>
        /// <param name="cmdText">要执行的SQL语句</param>
        /// <returns>返回的第一行第一列的值</returns>
        public static object ExecuteScalar(string cmdText)
        {
            OleDbParameter[] cmdParms = new OleDbParameter[] { };

            return ExecuteScalar(cmdText, cmdParms);
        }

        /// <param name="connStr">数据库连接字符串</param>
        /// <param name="cmd">要执行的命令数组</param>
        /// <returns>是否成功执行</returns>
        public static bool ExecuteTrans(string connStr, forTrans[] cmd)
        {
            bool pass = false;

            OleDbConnection myConnection = new OleDbConnection(connStr);
            myConnection.Open();

            // 开始一个本地事务
            OleDbTransaction myTrans = myConnection.BeginTransaction();

            // 命令与事务关联
            OleDbCommand myCommand = myConnection.CreateCommand();
            myCommand.Transaction = myTrans;

            try
            {
                int i;
                for (i = 0; i < cmd.Length; i++)//根据参数初始化命令并执行，呵呵
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
            return ExecuteTrans(ConnString, cmd);
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
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
                        //循环
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
        /// 执行多条SQL语句，实现数据库事务。(返回布尔值)2024-08-14
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
                        //循环
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
        /// 事务提交
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
        /// 事务提交
        /// </summary>
        /// <param name="myTrans"></param>
        /// <param name="list"></param>
        public static void ExecuteTranslist(OleDbTransaction myTrans, MyLists list)
        {
            ExecuteSqlTranList(myTrans, list);
        }

        /// <summary>
        /// OracleOledbBase 执行存储过程
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

        /// 导入Excel到DataGrid
        /// </summary>
        /// <param name="fileName">Excel文件名</param>
        /// <param name="sheetName">表格名称</param>
        /// <returns>返回DataSet</returns>
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
        /// 填充一个表集合类
        /// </summary>
        /// <param name="table">要填充的表格</param>
        /// <param name="connStr">连接字符串</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParms">SQL参数列表</param>
        public static void FillTheTable(CollectionTable table, string connStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();

                // 初始化一个SqlCommand对象.
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);

                // 创建一个使用此SqlCommand的适配器
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


        //执行单条语句返回布尔值
        public static bool ExecuteSql(string cmdText)
        {
            OleDbParameter[] cmdParms = new OleDbParameter[] { };

            return ExecuteSql(cmdText, cmdParms);
        }

        //执行单条语句返回布尔值
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
                    success = true; // 执行成功
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
