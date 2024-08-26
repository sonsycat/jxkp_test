using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using Goldnet.Ext.Web;
using GoldNet.Model;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class RoleList :PageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            //if (Session["CURRENTSTAFF"] == null)
            //{
            //    Response.End();
            //}
            if (!Ext.IsAjaxRequest)
            {
                GetQueryPortalet(null, null);
                //this.DeptFilter("dept_code");
            }
            //
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonadd_Click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("RoleEdit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("roleid", ""));
            showCenterSet(this.RoleEdit, loadcfg);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttondel_Click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.SelectRecord();
            }
            else
            {

                Ext.Msg.Confirm("系统提示", "您确定要删除选中角色吗？", new MessageBox.ButtonsConfig
                {
                    Yes = new MessageBox.ButtonConfig
                    {
                        Handler = "Goldnet.del()",
                        Text = "确定"

                    },
                    No = new MessageBox.ButtonConfig
                    {
                        Text = "取消"
                    }
                }).Show();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [AjaxMethod]
        public void del()
        {
            Goldnet.Dal.SYS_ROLE_DICT bll = new Goldnet.Dal.SYS_ROLE_DICT();
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            int roleid = int.Parse(sm.SelectedRow.RecordID);
            bll.RemoveRole(roleid);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("RefreshData();");
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonedit_Click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.SelectRecord();
            }
            else
            {
                string roleid = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("RoleEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("roleid", roleid));
                showCenterSet(this.RoleEdit, loadcfg);
            }
        }

        /// <summary>
        /// 角色设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonset_Click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.SelectRecord();
            }
            else
            {
                string roleid = sm.SelectedRow.RecordID;
                
                LoadConfig loadcfg = getLoadConfig("RoleSet.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("roleid", roleid));
                showCenterSet(this.Rle_Set, loadcfg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet1(object sender, EventArgs e)
        {
            // 导出
            //Bitmap m_Bitmap = WebSite.GetWebSiteThumbnail("http://localhost/jxkp/jxkp/WebApp/(S(azgfnv4501qh5qynx4etodr4))/Bonus_DataInput/SingleAwardList.aspx", 600, 400, 600, 400);
            //MemoryStream ms = new MemoryStream();
            //m_Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //byte[] buff = ms.ToArray();
            //Response.AddHeader(" Content-type ", "image/Jpeg");
            //Response.BinaryWrite(buff);
            //Response.End();

            Page.Response.Clear();
            bool success = ResponseFile(Page.Request, Page.Response, "111", Server.MapPath("WebSite.cs"), 1024000);
            if (!success)
                Response.Write("下载文件出错！");
            Page.Response.End();
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Request"></param>
        /// <param name="_Response"></param>
        /// <param name="_fileName"></param>
        /// <param name="_fullPath"></param>
        /// <param name="_speed"></param>
        /// <returns></returns>
        public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
        {
            try
            {
                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;

                    double pack = 10240; //10K bytes
                    //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                    int sleep = (int)Math.Floor(1000 * pack / _speed) + 1;
                    if (_Request.Headers["Range"] != null)
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        //Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength-1, fileLength));
                    }
                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((fileLength - startBytes) / pack) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(int.Parse(pack.ToString())));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();

                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            //
            string days = "11";
            if (days == null | days.Equals(""))
            {
                days = "1";
            }
            DataTable table = GetStoreData(days);
            User user = (User)Session["CURRENTSTAFF"];
           
            System.Data.DataView dv = table.DefaultView;
            dv.RowFilter = user.GetRoleFilter("role_app");
            this.Store1.DataSource = dv;
            
            this.Store1.DataBind();   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowSelect(object sender, AjaxEventArgs e)
        {
            string roleid = e.ExtraParams["ROLE_ID"];

            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;

            string str = sm.SelectedRow.RecordID;      
        }

        /// <summary>
        /// 角色维护
        /// </summary>
        private DataTable GetStoreData(string days)
        {
            //取得传入的选择的会话状态
            string deptcode = Session["curdeptcode"] == null ? "" : Session["curdeptcode"].ToString();
            string stationcode = Session["curstationcode"] == null ? "" : Session["curstationcode"].ToString();
            string personid = Session["curpersonid"] == null ? "" : Session["curpersonid"].ToString();
            string yearstr = Session["curdateyear"] == null ? DateTime.Now.ToString("yyyy") : Session["curdateyear"].ToString();
            string curguide = System.Configuration.ConfigurationManager.AppSettings["curguide"].ToString();
            DataTable dt = GetRoleList(deptcode, days).Tables[0];
            return dt;
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetRoleList(string deptcode, string daydate)
        {
            Goldnet.Dal.SYS_ROLE_DICT roledal = new Goldnet.Dal.SYS_ROLE_DICT();
            return roledal.GetList("");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void  Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetQueryPortalet(null, null);
        }
    }
}
