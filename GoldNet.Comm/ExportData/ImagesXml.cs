using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using Goldnet.Ext.Web;
using GoldNet.Comm;


namespace GoldNet.Comm.ExportData
{
    /// <summary>
    /// 读取XML文件
    /// </summary>
    public class ImagesXml
    {
        //变量定义
        private static XmlDocument m_xmlDoc = null;
        
        /// <summary>
        /// 初始化XML
        /// </summary>
        public static void InitXml()
        {
            m_xmlDoc = new XmlDocument();
            //虚拟路径
            string l_path = HttpContext.Current.Server.MapPath("/mrp/customization/WallPaper/");
            string l_fileName = l_path + @"\desktopimgs.xml";
            //加载
            m_xmlDoc.Load(l_fileName);
        }

        /// <summary>
        /// 取得XML信息
        /// </summary>
        /// <returns>数据集</returns>
        public static List<ImageGroup> GetMsgFromXml()
        {

            List<ImageGroup> data = new List<ImageGroup>();
             try { 
                    InitXml();
                    
                 //XML节点
                 XmlNodeList l_nodeList = m_xmlDoc.SelectNodes("desktopimg/groups/group");
                    
                //遍历
                foreach (XmlNode xn in l_nodeList)
                {

                    ImageGroup imageGroupInfo = new ImageGroup();
                    //分类节点值
                    imageGroupInfo.title = xn.Attributes.Item(0).Value;
                    imageGroupInfo.id = xn.Attributes.Item(0).Value;
                

                    foreach (XmlNode item in xn)
                    {
                        //图片信息
                        string id = item.Attributes.Item(0).Value;

                        string imgUrl = "/resources/images/WallPaper/" + id;

                        string descr = item.Attributes.Item(2).Value;

                        imageGroupInfo.samples.Add(new {id, imgUrl, descr});
                    
                    } 
                        data.Add(imageGroupInfo);

                    }
                 }
                catch(Exception ex) 
                {
                    HandleException(ex, "GetMsgFromXml");
                
                }
            
                return data;
        }

        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="ex">错误</param>
        /// <param name="MethodName">方法名称</param>
        private static void HandleException(Exception ex, string MethodName)
        {
            //写入日志文件
           // Loggers.WriteLog(ex.StackTrace, "GoldNet.Comm.ExportData", "ImagesXml", MethodName);
        }

    }

    /// <summary>
    /// 数据映射
    /// </summary>
    public class ImageGroup
    {
        private List<object> items;

        public string id { get; set; }

        public string title { get; set; }

        public List<object> samples
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new List<object>();
                }
                return items;
            }
        }
    }
}
