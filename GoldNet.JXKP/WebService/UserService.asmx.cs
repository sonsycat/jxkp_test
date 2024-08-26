using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Text;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OleDb;

namespace GoldNet.JXKP.WebService
{
    /// <summary>
    /// UserService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class UserService : System.Web.Services.WebService
    {
        /// <summary>
        /// 同步用户xml字符串
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        [WebMethod]
        public string SyncUser(string xmlstr)
        {
            XmlDocument xmldoc;
            XmlElement xmlelem;
            xmldoc = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmldoc.AppendChild(xmldecl);
            xmlelem = xmldoc.CreateElement("", "SYSUSERINFO", "");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("SYSUSERINFO");
            XmlElement xe1 = xmldoc.CreateElement("STATE");
            XmlElement xe2 = xmldoc.CreateElement("ERRORMESSAGE");
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(xmlstr);
                XmlNodeReader readerxml = new XmlNodeReader(xdoc);
                DataSet ds = new DataSet();
                ds.ReadXml(readerxml);
                MyLists listtable = new MyLists();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string delsql = string.Format("delete from  comm.SYS_USERS where user_id=?");
                    List listdel = new List();
                    listdel.StrSql = delsql;
                    listdel.Parameters = new OleDbParameter[] { new OleDbParameter("user_id", ds.Tables[0].Rows[i]["USER_ID"].ToString()) };
                    listtable.Add(listdel);
                    //
                    StringBuilder insertsql = new StringBuilder();
                    insertsql.AppendFormat(@"insert into comm.sys_users (db_user,user_id,user_name,user_dept,user_pswd,input_code) values (?,?,?,?,?,?)");
                    OleDbParameter[] parameter = {
                                              new OleDbParameter("db_user",ds.Tables[0].Rows[i]["DB_USER"].ToString().ToUpper()),
											  new OleDbParameter("user_id",ds.Tables[0].Rows[i]["USER_ID"].ToString()),
											  new OleDbParameter("user_name",ds.Tables[0].Rows[i]["USER_NAME"].ToString()),
											  new OleDbParameter("user_dept",ds.Tables[0].Rows[i]["USER_DEPT"].ToString()),
											  new OleDbParameter("user_pswd",ds.Tables[0].Rows[i]["USER_PSWD"].ToString()),
											  new OleDbParameter("input_code",ds.Tables[0].Rows[i]["INPUT_CODE"].ToString())
                                              };
                    List listinsert = new List();
                    listinsert.StrSql = insertsql.ToString();
                    listinsert.Parameters = parameter;
                    listtable.Add(listinsert);
                }
                OracleOledbBase.ExecuteTranslist(listtable);
                xe1.InnerText = "0";
                xe2.InnerText = "成功！";
            }
            catch
            {
                xe1.InnerText = "1";
                xe2.InnerText = "XML文件错误！";
            }
            root.AppendChild(xe1);
            root.AppendChild(xe2);
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xmldoc.WriteTo(xw);
            return sw.ToString();

        }
    }
}
