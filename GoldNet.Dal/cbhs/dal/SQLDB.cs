using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace get_yongyou.DAL
{
    /// <summary>
    /// SQLDB 的摘要说明
    /// </summary>
    public class SQLDB
    {
        public SQLDB()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private SqlConnection con;
        #region "Fields of base calss"
        protected static string strConn = ConfigurationSettings.AppSettings["costsql"];

        protected static string strSQL;
        #endregion
        #region "Properties of base class"
        private int m_ID;
        private string m_Name;
        public int ID
        {
            get
            {
                return m_ID;
            }
            set
            {
                m_ID = value;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        #endregion

        #region "Functions of base class"


        #region 将DataReader 转为 DataTable
        /// <summary>
        /// 将DataReader 转为 DataTable
        /// </summary>
        /// <param name="DataReader">DataReader</param>
        public DataTable ConvertDataReaderToDataTable(SqlDataReader dataReader)
        {
            DataTable datatable = new DataTable("dt");
            DataTable schemaTable = dataReader.GetSchemaTable();
            //动态添加列
            try
            {

                foreach (DataRow myRow in schemaTable.Rows)
                {
                    ////2003　DataTable绑定数据的用法
                    //DataColumn myDataColumn = new DataColumn();
                    //myDataColumn.DataType = myRow.GetType();
                    //myDataColumn.ColumnName = myRow[0].ToString();
                    //datatable.Columns.Add(myDataColumn);

                    //2005　DataTable绑定数据的用法
                    DataColumn myDataColumn = new DataColumn();
                    myDataColumn.DataType = (Type)myRow["DataType"];
                    myDataColumn.ColumnName = (String)myRow["ColumnName"];
                    datatable.Columns.Add(myDataColumn);
                }
                //添加数据
                while (dataReader.Read())
                {
                    DataRow myDataRow = datatable.NewRow();
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        myDataRow[i] = dataReader[i].ToString();
                    }
                    datatable.Rows.Add(myDataRow);
                    myDataRow = null;
                }
                schemaTable = null;
                return datatable;
            }
            catch (Exception ex)
            {
                ///	Error.Log(ex.ToString());
                return datatable;
            }

        }

        #endregion

        /// <summary>
        /// 执行查询语句，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string strSQL)
        {
            SqlConnection connection = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand(strSQL, connection);
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader();
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }

        }

        protected static int ExecuteSql(string strSQL)
        {
            SqlConnection myCn = new SqlConnection(strConn);
            SqlCommand myCmd = new SqlCommand(strSQL, myCn);
            try
            {
                myCn.Open();
                myCmd.ExecuteNonQuery();
                return 0;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                myCmd.Dispose();
                myCn.Close();
            }
        }


        /// <summary>
        ///executing SQL commands
        /// </summary>
        /// <param name="strSQL">要执行的SQL语句,为字符串类型string</param>
        /// <returns>返回执行情况,整形int</returns>
        protected static int ExecuteSqlEx(string strSQL)
        {
            SqlConnection myCn = new SqlConnection(strConn);
            SqlCommand myCmd = new SqlCommand(strSQL, myCn);

            try
            {
                myCn.Open();
                SqlDataReader myReader = myCmd.ExecuteReader();
                if (myReader.Read())
                {
                    return 0;
                }
                else
                {
                    throw new Exception("Value Unavailable!");
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                myCmd.Dispose();
                myCn.Close();
            }
        }

        public DateTime GetSysDate()
        {
            string strSQL = "select getdate() as sdate ";
            SqlConnection myCn = new SqlConnection(strConn);
            try
            {
                myCn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(strSQL, myCn);
                DataSet ds = new DataSet("ds");
                sda.Fill(ds);
                DataRow dr = ds.Tables[0].Rows[0];
                return System.DateTime.Parse(dr["sdate"].ToString());
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                myCn.Close();
            }
        }
        /// <summary>
        /// get dataset
        /// </summary>
        /// <param name="strSQL">(string)</param>
        /// <returns>(DataSet)</returns>
        /// 

        public static DataSet ExecuteSql4Ds(string strSQL)
        {
            SqlConnection myCn = new SqlConnection(strConn);
            try
            {
                myCn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(strSQL, myCn);
                DataSet ds = new DataSet("ds");
                sda.Fill(ds);
                return ds;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                myCn.Close();
            }
        }


        protected static DataSet ExecuteSql4Ds(string strSQL, int strecord, int maxrecord)
        {
            SqlConnection myCn = new SqlConnection(strConn);
            try
            {
                myCn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(strSQL, myCn);
                DataSet ds = new DataSet("ds");
                sda.Fill(ds, strecord, maxrecord, "tt");
                return ds;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                myCn.Close();
            }
        }

        /// <summary>
        /// get single value
        /// </summary>
        /// <param name="strSQL">(string)</param>
        /// <returns>(int)</returns>
        protected static int ExecuteSql4Value(string strSQL)
        {
            SqlConnection myCn = new SqlConnection(strConn);
            SqlCommand myCmd = new SqlCommand(strSQL, myCn);
            try
            {
                myCn.Open();
                object r = myCmd.ExecuteScalar();
                if (Object.Equals(r, null))
                {
                    throw new Exception("value unavailable！");
                }
                else
                {
                    return (int)r;
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                myCmd.Dispose();
                myCn.Close();
            }
        }


        /// <summary>
        /// get object
        /// </summary>
        /// <param name="strSQL">(string)</param>
        /// <returns>(object)</returns>
        protected static object ExecuteSql4ValueEx(string strSQL)
        {
            SqlConnection myCn = new SqlConnection(strConn);
            SqlCommand myCmd = new SqlCommand(strSQL, myCn);
            try
            {
                myCn.Open();
                object r = myCmd.ExecuteScalar();
                if (Object.Equals(r, null))
                {
                    throw new Exception("object unavailable!");
                }
                else
                {
                    return r;
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                myCmd.Dispose();
                myCn.Close();
            }
        }


        /// <summary>
        /// execute multipul SQL commands 
        /// </summary>
        /// <param name="strSQLs">string</param>
        /// <returns>int</returns>
        protected static int ExecuteSqls(string[] strSQLs)
        {
            SqlConnection myCn = new SqlConnection(strConn);
            SqlCommand myCmd = new SqlCommand();
            int j = strSQLs.Length;

            try
            {
                myCn.Open();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            SqlTransaction myTrans = myCn.BeginTransaction();

            try
            {
                myCmd.Connection = myCn;
                myCmd.Transaction = myTrans;

                foreach (string str in strSQLs)
                {
                    myCmd.CommandText = str;
                    myCmd.ExecuteNonQuery();
                }
                myTrans.Commit();
                return 0;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                myTrans.Rollback();
                throw new Exception(e.Message);
            }
            finally
            {
                myCmd.Dispose();
                myCn.Close();
            }
        }
        public int Get_DbRow(string strSQL)
        {
            SqlConnection myCn = new SqlConnection(strConn);
            SqlCommand myCmd = new SqlCommand(strSQL, myCn);

            try
            {
                myCn.Open();
                SqlDataReader myReader = myCmd.ExecuteReader();
                if (myReader.Read())
                {
                    return myReader.GetInt32(0);
                }
                else
                {
                    throw new Exception("Value Unavailable!");
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                myCmd.Dispose();
                myCn.Close();
            }

        }
        #endregion
        /// <summary>
        /// 传入输入参数
        /// </summary>
        /// <param name="ParamName">存储过程名称</param>
        /// <param name="DbType">参数类型</param></param>
        /// <param name="Size">参数大小</param>
        /// <param name="Value">参数值</param>
        /// <returns>新的 parameter 对象</returns>
        public SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        /// <summary>
        /// 传入返回值参数
        /// </summary>
        /// <param name="ParamName">存储过程名称</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <returns>新的 parameter 对象</returns>
        public SqlParameter MakeOutParam(string ParamName, SqlDbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        /// <summary>
        /// 传入返回值参数
        /// </summary>
        /// <param name="ParamName">存储过程名称</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <returns>新的 parameter 对象</returns>
        public SqlParameter MakeReturnParam(string ParamName, SqlDbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.ReturnValue, null);
        }

        /// <summary>
        /// 生成存储过程参数
        /// </summary>
        /// <param name="ParamName">存储过程名称</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <param name="Direction">参数方向</param>
        /// <param name="Value">参数值</param>
        /// <returns>新的 parameter 对象</returns>
        public SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            SqlParameter param;

            if (Size > 0)
                param = new SqlParameter(ParamName, DbType, Size);
            else
                param = new SqlParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="prams">存储过程所需参数</param>
        /// <returns>返回存储过程返回值</returns>
        public int RunProcNQ(string procName, SqlParameter[] prams)
        {
            this.dbOpen();
            SqlCommand cmd = new SqlCommand(procName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                if (prams != null)
                {
                    foreach (SqlParameter parameter in prams)
                        cmd.Parameters.Add(parameter);
                }
                cmd.Parameters.Add(
                    new SqlParameter("ReturnValue", SqlDbType.Int, 4,
                    ParameterDirection.ReturnValue, false, 0, 0,
                    string.Empty, DataRowVersion.Default, null));
                cmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            this.Closedb();
            return (int)cmd.Parameters["ReturnValue"].Value;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程的名称</param>
        /// <param name="prams">存储过程所需参数</param>
        /// <param name="dataReader">存储过程所需参数</param>
        public void RunProc(string procName, SqlParameter[] prams, out SqlDataReader dataReader)
        {
            // 确认打开连接			
            this.dbOpen();
            SqlCommand cmd = new SqlCommand(procName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                if (prams != null)
                {
                    foreach (SqlParameter parameter in prams)
                        cmd.Parameters.Add(parameter);
                }
                cmd.Parameters.Add(
                    new SqlParameter("ReturnValue", SqlDbType.Int, 4,
                    ParameterDirection.ReturnValue, false, 0, 0,
                    string.Empty, DataRowVersion.Default, null));
                dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
        }


        //直接返回DataSet
        public int RunPro1(string ProName, out DataSet Dataset)
        {//适用于没有传入参数，而且要返回一个DataSet内存数据库
            SqlCommand Cmd = CreateCommand(ProName, null);
            SqlDataAdapter Da = new SqlDataAdapter();
            Da.SelectCommand = Cmd;
            Dataset = new DataSet();
            Da.Fill(Dataset);
            this.dbOpen();
            return (int)Cmd.Parameters["ReturnValue"].Value;

        }
        public int RunPro1(string ProName, SqlParameter[] Parms, out DataSet Dataset)
        {
            SqlCommand Cmd = CreateCommand(ProName, Parms);
            SqlDataAdapter Da = new SqlDataAdapter();
            Da.SelectCommand = Cmd;
            Dataset = new DataSet();
            Da.Fill(Dataset);
            this.Closedb();
            return (int)Cmd.Parameters["ReturnValue"].Value;

        }
        public SqlCommand CreateCommand(string ProName, SqlParameter[] Parms)
        {//建立Command,每一个都会使用Command，但是此Command只能用于存储过程
            this.dbOpen();
            SqlCommand Cmd = new SqlCommand(ProName, con);
            Cmd.CommandType = CommandType.StoredProcedure;
            if (Parms != null)
            {
                foreach (SqlParameter Parm in Parms)
                {
                    Cmd.Parameters.Add(Parm);
                }
            }
            Cmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return Cmd;
        }

        /// <summary>
        /// 打开数据库连接.
        /// </summary>
        private void dbOpen()
        {
            // 打开数据库连接
            if (con == null)
            {
                con = new SqlConnection(strConn);
            }
            if (con.State == System.Data.ConnectionState.Closed)
                con.Open();

        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Closedb()
        {
            if (con != null)
                con.Close();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Disposecon()
        {
            // 确认连接是否已经关闭
            if (con != null)
            {
                con.Dispose();
                con = null;
            }
        }
    }
}