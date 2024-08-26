using System;
using System.Collections.Generic;
using GoldNet.Model;
using GoldNet.Comm;
using Goldnet.Ext.Web;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

namespace GoldNet.JXKP
{
    public class PageBase : System.Web.UI.Page
    {
        public PageBase() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="O"></param>
        protected override void OnInit(EventArgs O)
        {
            if (base.Session["CURRENTSTAFF"] == null)
            {
                //Response.Redirect("/home/default.aspx",true);
                Goldnet.Ext.Web.Ext.Redirect("/home/default.aspx");
            }
        }

        /// <summary>
        /// 是否是个人
        /// </summary>
        /// <returns></returns>
        protected bool IsPersons()
        {
            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            return user.GetPersonsPower(menuid, modid);
        }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        /// <returns></returns>
        protected bool IsEdit()
        {
            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            return user.SetControlEdit(menuid, modid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        protected bool IsEdit(string pageid)
        {
            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = pageid.Split('_');
            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            return user.SetControlEdit(menuid, modid);
        }

        /// <summary>
        /// 审核（上报）也可以用在页面的第二个功能
        /// </summary>
        /// <returns></returns>
        protected bool IsPass()
        {
            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            return user.SetControlPass(menuid, modid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        protected bool IsPass(string pageid)
        {
            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = pageid.Split('_');
            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            return user.SetControlPass(menuid, modid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string Pageid()
        {
            return Request.QueryString["pageid"].ToString();
        }

        /// <summary>
        /// 科室权限
        /// </summary>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        protected string DeptFilter(string deptfilter)
        {
            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            return user.GetUserDeptFilter(deptfilter, menuid, modid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptfilter"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        protected string DeptFilter(string deptfilter, string pageid)
        {
            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = pageid.Split('_');
            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            return user.GetUserDeptFilter(deptfilter, menuid, modid);
        }

        /// <summary>
        /// 全成本项目权限
        /// </summary>
        /// <returns></returns>
        protected DataTable xyhsCostItemFilter()
        {
            User user = (User)Session["CURRENTSTAFF"];
            return user.GetxyhsCostFilter();
        }

        //liu.shh  2012.12.18
        /// <summary>
        /// 科室编码
        /// </summary>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        protected string DeptCode()
        {
            User user = (User)Session["CURRENTSTAFF"];
            return user.GetPersonDept(user.UserId);
        }

        /// <summary>
        /// 院科人权限
        /// </summary>
        /// <returns></returns>
        protected string GetUserOrg()
        {
            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            return user.GetUserOrg(menuid, modid);
        }

        /// <summary>
        /// 人员权限
        /// </summary>
        /// <param name="userfilter"></param>
        /// <returns></returns>
        protected string UserFilter(string userfilter)
        {
            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            return user.GetUserFilter(userfilter, menuid, modid);
        }

        /// <summary>
        /// 成本项目权限
        /// </summary>
        /// <returns></returns>
        protected DataTable CostItemFilter()
        {
            User user = (User)Session["CURRENTSTAFF"];
            return user.GetCostFilter();
        }

        /// <summary>
        /// 角色权限
        /// </summary>
        /// <returns></returns>
        protected DataTable GetPerRoleType()
        {
            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            return user.GetPersRoleType(menuid, modid);
        }

        /// <summary>
        /// 人员类别权限
        /// </summary>
        /// <returns></returns>
        protected DataTable PersTypeFilter()
        {
            User user = (User)Session["CURRENTSTAFF"];
            return user.GetPersTypeFilter();
        }

        /// <summary>
        /// 数据操作错误
        /// </summary>
        /// <param name="ee"></param>
        protected void ShowDataError(string messages, string namespaceurl, string method)
        {
            User user = (User)Session["CURRENTSTAFF"];
            Loggers.WriteExLog(CleanString.InputText(messages, 256), namespaceurl, user == null ? "" : user.UserId, method, GetRealIP());
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = SystemMsg.msgdatatitle,
                Message = messages + "    ",
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "ERROR"),
                Modal = true
            });
        }
        /// <summary>
        /// 数据操作错误
        /// </summary>
        /// <param name="ee"></param>
        protected void ShowDataError(Exception ex, string namespaceurl, string method)
        {
            if (ex is GlobalException)
            {
                ShowMessage(((GlobalException)ex).Title, ((GlobalException)ex).Content);
            }
            else
            {
                User user = (User)Session["CURRENTSTAFF"];
                Loggers.WriteExLog(CleanString.InputText(ex.Message, 256), namespaceurl, user == null ? "" : user.UserId, method, GetRealIP());
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgdatatitle,
                    Message = SystemMsg.msgdatacontent + "," + ex.ToString().Substring(0, 100),
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "ERROR"),
                    Modal = true
                });
            }
        }
        /// <summary>
        /// 获得IP地址
        /// </summary>
        /// <returns></returns>
        private string GetRealIP()
        {
            string ip = null;
            HttpRequest request = HttpContext.Current.Request;

            if (request.ServerVariables["HTTP_VIA"] != null)
            {
                ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
            }
            else
            {
                ip = request.UserHostAddress;
                if (ip == null)
                {
                    ip = request.ServerVariables["remote_addr"];
                }
            }
            return ip;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected LoadConfig getLoadConfig(string url)
        {
            LoadConfig loadcfg = new LoadConfig();
            loadcfg.Url = url;
            loadcfg.Mode = LoadMode.IFrame;
            loadcfg.MaskMsg = "载入中...";
            loadcfg.ShowMask = true;
            loadcfg.NoCache = true;
            return loadcfg;
        }

        /// <summary>
        /// 显示明细主窗口
        /// </summary>
        /// <param name="win"></param>
        /// <param name="loadcfg"></param>
        protected void showCenterSet(Window win, LoadConfig loadcfg)
        {
            win.ClearContent();
            win.Show();
            win.LoadContent(loadcfg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qryStr"></param>
        /// <returns></returns>
        protected int GetInt32ByQueryStr(string qryStr)
        {
            int val;
            string theStr = Request.QueryString[qryStr];

            // 判断是否为null，如果为null，int会自动转换为0而不会抛出异常。
            if (theStr == null || theStr == string.Empty)
                throw new NullReferenceException("指定的QueryString名称:" + qryStr + "不存在!");

            // 转换到Int32
            try
            {

                string aa = Encrypt.UnEncryptMyStr("iloveyou", theStr);
                val = Convert.ToInt32(aa);
            }
            catch (Exception ex)
            {
                val = 0;
                throwGetQueryStringError(ex, qryStr);
            }
            return val;
        }

        /// <summary>
        /// 取得通过QueryString传递进来的String值（解密）
        /// </summary>
        /// <param name="qryStr">qry字段名称</param>
        protected string GetStringByQueryStr(string qryStr)
        {
            string theStr = Request.QueryString[qryStr];

            // 判断是否为null，如果为null，int会自动转换为0而不会抛出异常。
            if (theStr == null || theStr == string.Empty)
                throw new NullReferenceException("指定的QueryString名称:" + qryStr + "不存在!");

            return Encrypt.UnEncryptMyStr("iloveyou", theStr);
        }

        /// <summary>
        /// 加密一个QueryString（加密）
        /// </summary>
        /// <param name="theStr">要加密的QueryString</param>
        /// <returns>已加密的字符串</returns>
        protected string EncryptTheQueryString(string theStr)
        {
            return HttpUtility.UrlEncode(Encrypt.EncryptMyStr("iloveyou", theStr));
        }

        /// <summary>
        /// 为获取QueryString的可能异常提供的统一错误提示
        /// </summary>
        /// <param name="ex">错误的异常信息</param>
        /// <param name="qryStr">QueryString字段名称</param>
        protected void throwGetQueryStringError(Exception ex, string qryStr)
        {
            throw new GlobalException("通用错误：页面参数错误！", "尝试获取页面参数时发生错误。"
                + "<br>参数名称：" + qryStr + "。<br>造成此错误的原因可能是由于误操作、错误的链"
                + "接地址、操作时间超时等。请退出并重新登录系统再试，如此问题依然存在，请联系管"
                + "理员解决！", ex);
        }

        /// <summary>
        /// 判断是否是数值型数据
        /// </summary>
        /// <param name="strNum">要判断的字符串</param>
        /// <returns>是数值型返回true,否则返回false</returns>
        protected bool IsNum(object strNum)
        {
            try
            {
                double dbnum = (Convert.ToDouble(strNum));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="page"></param>
        protected void Alert(string msg, string page)
        {
            string _altMsg = "<script language='javascript'>\n<!--\nalert ('" + msg.Replace("'", @"""") + "');\nlocation=\"" + page + "\";\n//-->\n</script>";

            // Response.Clear();
            Response.Write(_altMsg);
            Response.End();
        }

        /// <summary>
        /// Alert 方法:用于对用户的提示，并做后退动作
        /// </summary>
        /// <param name="msg">要提示的信息</param>
        /// <param name="endresponse">是否要结束输出</param>
        protected void Alert(string msg, bool endresponse)
        {
            string _altMsg = "<script language='javascript'>\n<!--\nalert ('" + msg.Replace("'", @"""") + "');\n";
            if (endresponse) _altMsg += "history.back(1);";
            _altMsg += "\n//-->\n</script>";

            // Response.Clear();
            Response.Write(_altMsg);
            if (endresponse) Response.End();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Confirm(string msg, string page)
        {
            string _cfmMsg = "<script language='javascript'>";
            _cfmMsg = _cfmMsg + "if ( confirm('" + msg + "')) {";
            _cfmMsg = _cfmMsg + "location='" + page + "';}else{history.back(1);}</script>";
            Response.Write(_cfmMsg);
            Response.End();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Confirm(string msg, string script1, string script2, bool endResponse)
        {
            string _cfmMsg = "<script language='javascript'>";
            _cfmMsg = _cfmMsg + "if ( confirm('" + msg + "')) {";
            _cfmMsg = _cfmMsg + script1 + "}else{" + script2 + "}</script>";
            Response.Write(_cfmMsg);
            if (endResponse) Response.End();
        }

        /// <summary>
        /// 显示一个新窗口
        /// </summary>
        /// <param name="url">页面地址</param>
        /// <param name="parameter_str">父窗口</param>
        /// <param name="win_name">窗口名称</param>
        protected void Shownewpage(string url, string parameter_str, string win_name)
        {
            string url1 = url + "?" + parameter_str;
            string _winMsg = "<script language='javascript'>newwindow=window.open('" + url1 + "','" + win_name + "','resizable=no,scrollbars=yes,status=yes,toolbar=no,menubar=no,location=no');";
            _winMsg = _winMsg + "var wide = window.screen.availWidth;";
            _winMsg = _winMsg + "var high = window.screen.availHeight;";
            _winMsg = _winMsg + "newwindow.resizeTo(wide,high);";
            _winMsg = _winMsg + "newwindow.moveTo(0,0);</script>";
            Response.Write(_winMsg);
            //Response.End();
        }

        /// <summary>
        /// 保存成功
        /// </summary>
        protected void SaveSucceed()
        {
            ShowMessage("系统提示", "保存成功！");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        protected void Saveshibai(string item)
        {
            ShowMessage("系统提示", item+"项目没有保存，不能提交！");
        }

        /// <summary>
        /// 选择一条记录
        /// </summary>
        protected void SelectRecord()
        {
            ShowMessage("系统提示", "请选择一条记录！");
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        protected void ShowMessage(string title, string message)
        {
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = title,
                Message = message + "   ",
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO"),
                Modal = true
            });
        }

        /// <summary>
        /// excel导出
        /// </summary>
        /// <param name="table"></param>
        /// <param name="tablename"></param>
        public void outexcel(DataTable table, string tablename)
        {
            HttpResponse resp;
            resp = Page.Response;
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            resp.AddHeader("Content-Disposition",
                "attachment; filename=" + System.Web.HttpUtility.UrlEncode(tablename, System.Text.Encoding.UTF8) + ".xls");
            string colHeaders = "", ls_item = "";
            int i = 0;
            DataTable dt = table;
            DataRow[] myRow = dt.Select("");
            for (i = 0; i < dt.Columns.Count; i++)
            {
                colHeaders += dt.Columns[i].ColumnName.ToString() + "\t";

            }
            colHeaders += "\n";
            resp.Write(colHeaders);
            foreach (DataRow row in myRow)
            {
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    ls_item += row[i].ToString() + "\t";

                }
                ls_item += "\n";

                resp.Write(ls_item);
                ls_item = "";
            }
            resp.End();
        }

        /// <summary>
        /// 多表头导出execl
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="header"></param>
        /// <param name="fileName"></param>
        /// <param name="mergeCellNums"></param>
        /// <param name="mergeKey"></param>
        public void MHeaderTabletoExcel(System.Data.DataTable dtData, TableCell[] header, string fileName, Dictionary<int, int> mergeCellNums, int? mergeKey)
        {
            System.Web.UI.WebControls.GridView gvExport = null;
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            System.IO.StringWriter strWriter = null;
            System.Web.UI.HtmlTextWriter htmlWriter = null;

            if (dtData != null)
            {
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                curContext.Response.Charset = "gb2312";
                if (!string.IsNullOrEmpty(fileName))
                {
                    fileName = System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
                    curContext.Response.AppendHeader("Content-Disposition", ("attachment;filename=" + (fileName.ToLower().EndsWith(".xls") ? fileName : fileName + ".xls")));
                }
                strWriter = new System.IO.StringWriter();
                htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

                gvExport = new System.Web.UI.WebControls.GridView();
                gvExport.DataSource = dtData.DefaultView;
                gvExport.AllowPaging = false;
                gvExport.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(dgExport_RowDataBound);
                gvExport.DataBind();
                if (header != null && header.Length > 0)
                {
                    gvExport.HeaderRow.Cells.Clear();
                    gvExport.HeaderRow.Cells.AddRange(header);
                }
                if (mergeCellNums != null && mergeCellNums.Count > 0)
                {
                    foreach (int cellNum in mergeCellNums.Keys)
                    {
                        MergeRows(gvExport, cellNum, mergeCellNums[cellNum], mergeKey);
                    }
                }
                gvExport.RenderControl(htmlWriter);
                curContext.Response.Clear();
                curContext.Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + strWriter.ToString());
                curContext.Response.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="header"></param>
        /// <param name="fileName"></param>
        /// <param name="mergeCellNums"></param>
        /// <param name="mergeKey"></param>
        public void MHeaderTabletoExcel(System.Data.DataView dtData, TableCell[] header, string fileName, Dictionary<int, int> mergeCellNums, int? mergeKey)
        {
            System.Web.UI.WebControls.GridView gvExport = null;
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            System.IO.StringWriter strWriter = null;
            System.Web.UI.HtmlTextWriter htmlWriter = null;

            if (dtData != null)
            {
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                curContext.Response.Charset = "gb2312";
                if (!string.IsNullOrEmpty(fileName))
                {
                    fileName = System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
                    curContext.Response.AppendHeader("Content-Disposition", ("attachment;filename=" + (fileName.ToLower().EndsWith(".xls") ? fileName : fileName + ".xls")));
                }
                strWriter = new System.IO.StringWriter();
                htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

                gvExport = new System.Web.UI.WebControls.GridView();
                gvExport.DataSource = dtData;
                gvExport.AllowPaging = false;
                gvExport.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(dgExport_RowDataBound);
                gvExport.DataBind();
                if (header != null && header.Length > 0)
                {
                    gvExport.HeaderRow.Cells.Clear();
                    gvExport.HeaderRow.Cells.AddRange(header);
                }
                if (mergeCellNums != null && mergeCellNums.Count > 0)
                {
                    foreach (int cellNum in mergeCellNums.Keys)
                    {
                        MergeRows(gvExport, cellNum, mergeCellNums[cellNum], mergeKey);
                    }
                }
                gvExport.RenderControl(htmlWriter);
                curContext.Response.Clear();
                curContext.Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + strWriter.ToString());
                curContext.Response.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgExport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (Regex.IsMatch(cell.Text.Trim(), @"^\d{12,}$") || Regex.IsMatch(cell.Text.Trim(), @"^\d+[-]\d+$"))
                    {
                        cell.Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                    }
                }
            }
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="gvExport"></param>
        /// <param name="cellNum"></param>
        /// <param name="mergeMode"></param>
        /// <param name="mergeKey"></param>
        public void MergeRows(System.Web.UI.WebControls.GridView gvExport, int cellNum, int mergeMode, int? mergeKey)
        {
            int i = 0, rowSpanNum = 1;
            System.Drawing.Color alterColor = System.Drawing.Color.White;
            while (i < gvExport.Rows.Count - 1)
            {
                GridViewRow gvr = gvExport.Rows[i];
                for (++i; i < gvExport.Rows.Count; i++)
                {
                    GridViewRow gvrNext = gvExport.Rows[i];
                    if ((!mergeKey.HasValue || (mergeKey.HasValue && (gvr.Cells[mergeKey.Value].Text.Equals(gvrNext.Cells[mergeKey.Value].Text) || " ".Equals(gvrNext.Cells[mergeKey.Value].Text)))) && ((mergeMode == 1 && gvr.Cells[cellNum].Text == gvrNext.Cells[cellNum].Text) || (mergeMode == 2 && " ".Equals(gvrNext.Cells[cellNum].Text.Trim())) || (mergeMode == 3 && (gvr.Cells[cellNum].Text == gvrNext.Cells[cellNum].Text || " ".Equals(gvrNext.Cells[cellNum].Text.Trim())))))
                    {
                        gvrNext.Cells[cellNum].Visible = false;
                        rowSpanNum++;
                        gvrNext.BackColor = gvr.BackColor;
                    }
                    else
                    {
                        gvr.Cells[cellNum].RowSpan = rowSpanNum;
                        rowSpanNum = 1;
                        if (mergeKey.HasValue && cellNum == mergeKey.Value)
                        {
                            if (alterColor == System.Drawing.Color.White)
                            {
                                gvr.BackColor = System.Drawing.Color.White;
                                alterColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                alterColor = System.Drawing.Color.White;
                            }
                        }
                        break;
                    }
                    if (i == gvExport.Rows.Count - 1)
                    {
                        gvr.Cells[cellNum].RowSpan = rowSpanNum;
                        if (mergeKey.HasValue && cellNum == mergeKey.Value)
                        {
                            if (alterColor == System.Drawing.Color.White)
                                gvr.BackColor = System.Drawing.Color.White;
                        }
                    }
                }
            }
        }
    }
}
