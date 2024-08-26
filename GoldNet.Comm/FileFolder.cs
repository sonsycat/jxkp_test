using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;


namespace GoldNet.Comm
{
    public class FileFolder
    {
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="fileFolderName">文件夹名</param>
        /// <param name="page">页面</param>
        public void CheckFileFolder(string fileFolderName,Page page)
        {
            if (Directory.Exists(page.Server.MapPath(fileFolderName)) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(page.Server.MapPath(fileFolderName));               
            }
        }
        /// <summary>
        /// 文件转移
        /// </summary>
        /// <param name="tempFilePath">被复制的文件</param>
        /// <param name="copytoFilePath">要复制的文件</param>
        /// <returns></returns>
        public bool FileCopyAndDelete(string tempFilePath, string copytoFilePath)
        {
            try
            {
                File.Move(tempFilePath, copytoFilePath);               
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
