using System;
using System.Data;
using System.Web;
using System.IO;
using Goldnet.Ext.Web;
using GoldNet.Model;
using GoldNet.JXKP.BLL.Guide;

namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class QualityBuide : PageBase
    {
        private static int num = 0;
        private static string address = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                this.GridPanel1.ColumnModel.RegisterCommandStyleRules();
                ScriptManager1.RegisterIcon(Goldnet.Ext.Web.Icon.Accept);
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                pro.Url = "WebService/AccountDepts.ashx";
                this.Store2.Proxy.Add(pro);
                JsonReader jr = new JsonReader();
                jr.ReaderID = "DEPT_CODE";
                jr.Root = "deptlist";
                jr.TotalProperty = "totalCount";
                RecordField rf = new RecordField();
                rf.Name = "DEPT_CODE";
                jr.Fields.Add(rf);
                RecordField rfn = new RecordField();
                rfn.Name = "DEPT_NAME";
                jr.Fields.Add(rfn);
                this.Store2.Reader.Add(jr);
                //
                DataTable daterow = QualityGuide.GetAllDateDesc().Tables[0];
                this.Store3.DataSource = daterow;
                this.Store3.DataBind();
                this.date.SelectedIndex = 0;
                //
                SetDict();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();

            DataTable depttyperow = dal.Guide_Type_Dict().Tables[0];
            for (int i = 0; i < depttyperow.Rows.Count; i++)
            {
                this.ComGuide.Items.Add(new Goldnet.Ext.Web.ListItem(depttyperow.Rows[i]["GuideType"].ToString(), depttyperow.Rows[i]["ID"].ToString()));
            }
            if (this.date.SelectedItem.Value != "")
            {
                this.GridPanel1.Reconfigure();
                int intDateDesc = Convert.ToInt32(this.date.SelectedItem.Value.ToString());
                string dateDesc = this.date.SelectedItem.Text;

                string year = dateDesc.Split('-')[0];
                string month = dateDesc.Split('-')[1];
                DataTable dt = QualityGuide.QualitySearchByDate(intDateDesc, "", 0, year, month, this.DeptFilter("dept_code"));

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    RecordField field = new RecordField();
                    if (dt.Columns[i].ColumnName.Equals("科室"))
                    {
                        field = new RecordField(dt.Columns[i].ColumnName, RecordFieldType.String);
                    }
                    else
                    {
                        field = new RecordField(dt.Columns[i].ColumnName, RecordFieldType.Float);
                    }
                    this.Store1.AddField(field, i);
                    Column cl = new Column();
                    cl.Header = dt.Columns[i].ColumnName;
                    cl.Sortable = true;
                    cl.MenuDisabled = true;
                    cl.ColumnID = dt.Columns[i].ColumnName;
                    cl.DataIndex = dt.Columns[i].ColumnName;

                    this.GridPanel1.AddColumn(cl);
                }
                CommandColumn col = new CommandColumn();
                GridCommand gc = new GridCommand();
                gc.CommandName = "DetailView";
                gc.Icon = Goldnet.Ext.Web.Icon.Zoom;
                gc.ToolTip.Text = "详细信息";
                col.Width = 30;
                col.Commands.Add(gc);
                this.GridPanel1.ColumnModel.Columns.Add(col);
                this.Store1.DataSource = dt;
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonadd_Click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("GuideTypeEdit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", "0"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("straction", "add"));
            showCenterSet(this.guideDetail, loadcfg);
        }

        /// <summary>
        /// 
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
                string id = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("GuideTypeEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("straction", "edit"));
                showCenterSet(this.guideDetail, loadcfg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UploadClick(object sender, AjaxEventArgs e)
        {
            if (num < 5)
            {
                System.Web.HttpFileCollection _files = System.Web.HttpContext.Current.Request.Files;
                try
                {
                    string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
                    string[] filename = this.photoimg.PostedFile.FileName.ToString().Split('\\');
                    string fileid = filename[filename.Length - 1].ToString() + DateTime.Now.ToString("YYYYMMDDHHmmss").ToString() + Creater + "temp" + filename[filename.Length - 1].Substring(filename[filename.Length - 1].LastIndexOf("."));
                    string fpath = @"files\\" + fileid;
                    //string fpath = @"../Notice/" + filename[4].ToString() + DateTime.Now.ToString();
                    if (System.IO.File.Exists(Server.MapPath(fpath)))
                    {
                        System.IO.File.Delete(Server.MapPath(fpath));
                    }
                    photoimg.PostedFile.SaveAs(Server.MapPath(fpath));          //执行上传操作

                    //this.imgStaff.ImageUrl = fpath + "?temp=" + DateTime.Now.ToString();
                    num = num + 1;
                    Goldnet.Ext.Web.Ext.Msg.Alert("上传成功！", photoimg.PostedFile.FileName).Show();


                    address += filename[filename.Length - 1].ToString() + "," + fpath.ToString() + "&";

                    QualityGuide.savefiles(this.date.SelectedItem.Value, filename[filename.Length - 1].ToString(), fileid);
                }
                catch (Exception ex)
                {
                    Goldnet.Ext.Web.Ext.Msg.Alert("提示", "该文件过大或不存在！").Show();
                }
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Alert("提示", "一个通知允许上传" + num + "文件！").Show();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DownClick(object sender, AjaxEventArgs e)
        {
            DataTable tb = QualityGuide.selectfiles(this.date.SelectedItem.Value);
            if (tb.Rows.Count > 0)
            {
                DownLoadFile(tb.Rows[0]["FILES_ID"].ToString());
            }
            else
                this.ShowMessage("提示", "附件不存在！");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FullFileName"></param>
        public void DownLoadFile(string FullFileName)
        {
            // 保存文件的虚拟路径
            string Url = "files\\" + FullFileName;
            // 保存文件的物理路径
            string FullPath = HttpContext.Current.Server.MapPath(Url);
            // 初始化FileInfo类的实例，作为文件路径的包装
            FileInfo FI = new FileInfo(FullPath);
            // 判断文件是否存在
            if (FI.Exists)
            {
                // 将文件保存到本机
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(FI.Name));
                Response.AddHeader("Content-Length", FI.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.Filter.Close();
                Response.WriteFile(FI.FullName);
                Response.End();
            }
            else
            {
                this.ShowMessage("提示", "附件不存在！");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowSelect(object sender, AjaxEventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            string str = sm.SelectedRow.RecordID;
        }

        /// <summary>
        /// 双击详细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DbRowClick(object sender, AjaxEventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                Ext.Msg.Alert("提示", "请选择一条记录！").Show();
            }
            else
            {
                string dateDesc = date.SelectedItem.Text;
                string year = dateDesc.Split('-')[0];
                string month = dateDesc.Split('-')[1];
                string deptname = sm.SelectedRow.RecordID;
                Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
                DataTable table = dal.GetDeptByDeptName(deptname);
                string Deptcode = table.Rows[0]["dept_code"].ToString();

                LoadConfig loadcfg = getLoadConfig("DeptQualityView.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Deptcode", Deptcode));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("year", year));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("month", month));
                showCenterSet(this.guideDetail, loadcfg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet(object sender, EventArgs e)
        {
            GetPageData(this.date.SelectedItem.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetPageData(this.date.SelectedItem.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Storedate_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataTable daterow = QualityGuide.GetAllDateDesc().Tables[0];
            this.Store3.DataSource = daterow;
            this.Store3.DataBind();
            if (daterow.Rows.Count > 0)
            {
                this.date.SelectedItem.Text = daterow.Rows[0]["DateDesc"].ToString();
            }
            else
            {
                this.date.SelectedItem.Text = "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        private void GetPageData(string date)
        {
            if (date.Equals(""))
            {
                DataTable daterow = QualityGuide.GetAllDateDesc().Tables[0];
                if (daterow.Rows.Count > 0)
                {
                    this.date.SelectedItem.Text = daterow.Rows[0]["DateDesc"].ToString();
                    this.date.SelectedItem.Value = daterow.Rows[0]["id"].ToString();
                }
                else
                {
                    this.date.SelectedItem.Text = "";
                    this.date.SelectedItem.Value = "";
                    this.Store1.RemoveAll();
                    this.Store1.DataBind();
                }
            }
            if (this.date.SelectedItem.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "没有可以查询的数据！");
            }
            else
            {
                this.GridPanel1.Reconfigure();
                if (this.date.SelectedItem.Value.ToString() != "")
                {
                    int intDateDesc = Convert.ToInt32(this.date.SelectedItem.Value.ToString());
                    string strDept = this.ComAccountdeptcode.SelectedItem.Value.Trim();
                    int intGuide = this.ComGuide.SelectedItem.Value == "" ? 0 : Convert.ToInt32(this.ComGuide.SelectedItem.Value.ToString());
                    string dateDesc = this.date.SelectedItem.Text.ToString();
                    string year = dateDesc.Split('-')[0];
                    string month = dateDesc.Split('-')[1];

                    DataTable dt = QualityGuide.QualitySearchByDate(intDateDesc, strDept, intGuide, year, month, this.DeptFilter("dept_code"));
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        RecordField field = new RecordField();
                        field = new RecordField(dt.Columns[i].ColumnName, RecordFieldType.String);
                        this.Store1.AddField(field, i);
                        Column cl = new Column();
                        cl.Header = dt.Columns[i].ColumnName;
                        cl.Sortable = true;
                        cl.MenuDisabled = true;
                        cl.ColumnID = dt.Columns[i].ColumnName;
                        cl.DataIndex = dt.Columns[i].ColumnName;

                        this.GridPanel1.AddColumn(cl);
                    }
                    CommandColumn col = new CommandColumn();
                    GridCommand gc = new GridCommand();
                    gc.CommandName = "DetailView";
                    gc.Icon = Goldnet.Ext.Web.Icon.Zoom;
                    gc.ToolTip.Text = "详细信息";
                    col.Commands.Add(gc);
                    col.Width = 30;
                    this.GridPanel1.ColumnModel.Columns.Add(col);

                    this.Store1.DataSource = dt;
                    this.Store1.DataBind();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("Guide_View.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buidequality_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("BuildGuide.aspx");

            showCenterSet(this.BuideWin, loadcfg);
        }
    }
}
