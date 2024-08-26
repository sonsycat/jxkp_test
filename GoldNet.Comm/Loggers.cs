using System;
using System.Web;
using System.Data;
using System.Data.OleDb;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;

namespace GoldNet.Comm
{
    public class Loggers
    {

        /// <summary>
        /// 写入异常信息日志
        /// </summary>
        /// <param name="messageInfo">错误信息</param>
        /// <param name="nameSpc">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="method">方法名</param>
        /// <param name="ipstr">IP</param>
        public static void WriteExLog(string messageInfo, string nameSpc, string userid, string method,string ipstr)
        {
            try
            {
                string SqlStr = " INSERT INTO COMM.SYS_EXCEPTION_LOG(EXDATE,EXNAMESPACE,USER_ID,EXMETHOD,EXMSG,EXIP) VALUES(SYSDATE,?,?,?,?,?)";
                OleDbParameter[] parameters = {
                        new OleDbParameter("EXNAMESPACE", nameSpc),
					    new OleDbParameter("USER_ID", userid),
					    new OleDbParameter("EXMETHOD", method),
					    new OleDbParameter("EXMSG", messageInfo),
					    new OleDbParameter("EXIP",ipstr)
                };
                int iRtn = OracleOledbBase.ExecuteNonQuery(SqlStr, parameters);
            }
            catch (Exception )
            {
            }
            finally
            {
            }

        }

        /// <summary>
        /// 写入用户操作日志
        /// </summary>
        /// <param name="messageInfo">日志信息</param>
        /// <param name="title">标题</param>
        /// <param name="oper">操作者</param>
        /// <param name="opurl">请求url</param>
        /// <param name="opip">IP</param>
        /// <param name="err_f">错误系标志</param>
        public static void WriteUserLog(string messageInfo,string title,string oper,string opurl,string opip,string err_f)
        {
            try
            {
                string SqlStr = " INSERT INTO COMM.SYS_OPERATION_LOG(OPDATE,OPTITLE,OPCONENT,OPER,OPURL,OPIP,ERR_F,DEL_F) VALUES(SYSDATE,?,?,?,?,?,?,0) ";
                OleDbParameter[] parameters = {
                        new OleDbParameter("OPTITLE", title),
					    new OleDbParameter("OPCONENT", messageInfo),
					    new OleDbParameter("OPER", oper),
					    new OleDbParameter("OPURL", opurl),
					    new OleDbParameter("OPIP",opip),
                        new OleDbParameter("ERR_F",err_f)
                };
                int iRtn = OracleOledbBase.ExecuteNonQuery(SqlStr, parameters);
            }
            catch (Exception)
            {
            }
            finally
            {
            }

        }

    }
}
