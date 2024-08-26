using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Goldnet.Ext.Web;
using GoldNet.Comm.DAL.Oracle;
using System.Data;
using System.Xml;
using System.Xml.Xsl;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;


namespace GoldNet.Comm.ExportData
{
    public class ExportData
    {
        public bool ExportToLocal(string sql, Page webpage, string exportType, string fileName)
        {
            fileName = fileName + "(" + DateTime.Today.ToShortDateString()+")";

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);

            if (ds == null)
            {
                return false;
            }
            DataTable dt = ds.Tables[0];
            if (dt == null || dt.Rows.Count <= 0)
            {
                return false;
            }
            switch (exportType)
            {
                case "xml":
                    ExportDataTableXML(webpage, new StoreSubmitDataEventArgs(DataTableConvertToJson(dt), null).Xml, fileName);
                    break;
                case "xls":
                    ExportDataTableExcel(webpage, new StoreSubmitDataEventArgs(DataTableConvertToJson(dt), null).Xml, fileName);
                    break;

                case "csv":
                    ExportDataTableCSV(webpage, new StoreSubmitDataEventArgs(DataTableConvertToJson(dt), null).Xml, fileName);
                    break;
            }
            return true;
        }
        public bool ExportToLocal(DataTable dt, Page webpage, string exportType, string fileName)
        {            
            if (dt == null)
            {
                return false;
            }
            switch (exportType)
            {
                case "xml":
                    if (dt.Rows.Count == 0) 
                    {
                        return false;
                    }
                    ExportDataTableXML(webpage, new StoreSubmitDataEventArgs(DataTableConvertToJson(dt), null).Xml, fileName);
                    break;
                case "xls":
                    if (dt.Rows.Count == 0)
                    {
                        ExportDataTableExcel(webpage, new StoreSubmitDataEventArgs(DataTableConvertToJosnByNoData(dt), null).Xml, fileName);
                    }
                    else 
                    {
                        ExportDataTableExcel(webpage, new StoreSubmitDataEventArgs(DataTableConvertToJson(dt), null).Xml, fileName);
                    }
                    
                    break;
                case "csv":
                    if (dt.Rows.Count == 0)
                    {
                        return false;
                    }
                    ExportDataTableCSV(webpage, new StoreSubmitDataEventArgs(DataTableConvertToJson(dt), null).Xml, fileName);
                    break;
            }
            return true;
        }
        /// <summary>
        /// DataTable转化成Json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string DataTableConvertToJson(DataTable dt)
        {            
            StringBuilder sb=new StringBuilder();
            sb.Append( "[");            
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("{");
                foreach (DataColumn dc in dt.Columns)
                {
                    sb.Append("\"" + ReplaceillegalCharacters(dc.ColumnName) + "\":");
                    if (dc.DataType == Type.GetType("System.Int32"))
                    {
                        if (string.IsNullOrEmpty(dr[dc.ColumnName].ToString()))
                        {
                            sb.Append("\"\",");
                        }
                        else
                        {
                            sb.Append("" + dr[dc.ColumnName] + ",");
                        }                     
                    }
                    else
                    {
                         sb.Append("\"" + dr[dc.ColumnName] + "\",");
                    }                    
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// 当TABLE为空的时候导出JSON字符
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string DataTableConvertToJosnByNoData(DataTable dt) 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[{");
            foreach (DataColumn dc in dt.Columns)
            {
                sb.Append("\"" + ReplaceillegalCharacters(dc.ColumnName) + "\":");
                sb.Append("\"\",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}]");
            return sb.ToString();
        }

        public  string ReplaceillegalCharacters(string tmp)
        {
            string content =tmp;
            content = content.Replace("(", "").Replace(")", "");
            content = content.Replace("（", "").Replace("）", "");
            content = content.Replace(" ", "");
            content = content.Replace(",", "");
            //[\u0000-\u0008\u000B\u000C\u000E-\u001F\uD800-\uDFFF\uFFFE\uFFFF]
            //content = Regex.Replace(content, "[\\x00-\\x08\\x0b-\\x0c\\x0e-\\x1f]", "*");
            return content;
        }



        /// <summary>
        /// 生成Excel文件并且下载
        /// </summary>
        /// <param name="webpage">要导出数据的页面</param>
        /// <param name="xml">序列化过得TableData</param>
        private void ExportDataTableExcel(Page webpage,XmlNode xml,string filename)
        {   
            webpage.Response.Clear();
            webpage.Response.ContentType = "application/vnd.ms-excel";
            webpage.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(filename) + ".xls");
            XslCompiledTransform xtExcel = new XslCompiledTransform();
            xtExcel.Load(HttpContext.Current.Server.MapPath("/resources/ExportDataTemp/Excel.xsl"));
            xtExcel.Transform(xml, null, webpage.Response.OutputStream);
            webpage.Response.End();
        }
        /// <summary>
        /// 生成XML文件并且下载
        /// </summary>
        /// <param name="webpage">要导出数据的页面</param>
        /// <param name="xml">序列化过得TableData</param>
        private void ExportDataTableXML(Page webpage, XmlNode xml, string filename)
        {
            string strXml = xml.OuterXml;
            webpage.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ".xml");
            webpage.Response.AddHeader("Content-Length", strXml.Length.ToString());
            webpage.Response.ContentType = "application/xml";
            webpage.Response.Write(strXml);
            webpage.Response.End();
        }
        /// <summary>
        /// 生成CSV文件并且下载
        /// </summary>
        /// <param name="webpage">要导出数据的页面</param>
        /// <param name="xml">序列化过得TableData</param>
        private void ExportDataTableCSV(Page webpage, XmlNode xml, string filename)
        {
            webpage.Response.ContentType = "application/octet-stream";
            webpage.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ".csv");
            XslCompiledTransform xtCsv = new XslCompiledTransform();
            xtCsv.Load(HttpContext.Current.Server.MapPath("/resources/ExportDataTemp/Csv.xsl"));
            xtCsv.Transform(xml, null, webpage.Response.OutputStream);
            webpage.Response.End();
        }
    }
}
