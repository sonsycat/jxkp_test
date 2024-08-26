using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Collections.Generic;
using System.IO;
using Goldnet.Comm.ExportData;
using GoldNet.Comm.ExportData;
using GoldNet.Comm;
using System.Threading;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class gongzuol_daoru : System.Web.UI.Page
    {
        private BoundComm boundcomm = new BoundComm();
        private static List<WorkItem> TaskThread;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                //if (Session["CURRENTSTAFF"] == null)
                //{
                //    Response.End();
                //}

                stardate.Value = DateTime.Now.AddMonths(-1).ToString();
                //Bindlist(, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //Bindlist(System.DateTime.Now.ToString("yyyy-MM-dd"));

            }
        }

        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            string datetime = Convert.ToDateTime(stardate.Value).ToString("yyyy-MM-dd");
            Bindlist(datetime);
        }
        //查取数据、绑定结果
        public void Bindlist(string datetime)
        {
            Appended_income dal = new Appended_income();
            DataTable ds = dal.GetAssetsByDate(datetime).Tables[0];
            this.Store1.DataSource = ds;
            this.Store1.DataBind();
            Session.Remove("Assets_List");
            Session["Assets_List"] = ds;
        }
        //查询记录
        protected void Button_look_click(object sender, EventArgs e)
        {
            string datetime = Convert.ToDateTime(stardate.Value).ToString("yyyy-MM-dd");
            //string end_date = Convert.ToDateTime(enddate.Value).ToString("yyyy-MM-dd");

            // Bindlist(datetime, end_date);
            Bindlist(datetime);

        }
        //添加记录
        protected void Button_add_click(object sender, AjaxEventArgs e)
        {
            //LoadConfig loadcfg = getLoadConfig("Assets_List_Detail.aspx");
            //loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "add"));
            //showDetailWin(loadcfg);
        }
        //修改记录
        [AjaxMethod]
        public void data_edit(string rowsid)
        {
            //LoadConfig loadcfg = getLoadConfig("Assets_List_Detail.aspx");
            //loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "edit"));
            //loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("row_id", rowsid));

            //showDetailWin(loadcfg);

        }
        //删除
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow == null || selectRow.Length < 1)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "请至少选择一条记录",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            else
            {
                for (int i = 0; i < selectRow.Length; i++)
                {
                    Appended_income dal = new Appended_income();
                    dal.AssetsDelById(selectRow[i]["ID"]);
                }
                string datetime = Convert.ToDateTime(stardate.Value).ToString("yyyy-MM-dd");
                Bindlist(datetime);
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_update_click(object sender, AjaxEventArgs e)
        {
            //Dictionary<string, string>[] selectRow = GetSelectRow(e);
            //string id = string.Empty;
            //if (selectRow != null && selectRow.Length == 1)
            //{
            //    id = selectRow[0]["ID"];
            //}
            //LoadConfig loadcfg = getLoadConfig("Assets_List_Detail.aspx");
            //if (id != null)
            //{
            //    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));
            //}
            //loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "update"));
            //showDetailWin(loadcfg);
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        //显示添加窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        DataSet ds = new DataSet();
        protected void UploadClick(object sender, AjaxEventArgs e)
        {
            string station_year = System.DateTime.Now.ToString("yyyy/MM/dd");
            Progress1.UpdateProgress(0, " ");
            Progress1.Text = "导入成功！";
            this.Btn_BatCancel.Text = "退出";
            this.Win_BatchInit.SetTitle(station_year + "导入成本Excel");
            this.Win_BatchInit.Show("Btn_BatInit");
            this.KeyNav1.Focus();
        }

        /// <summary>
        /// 批量指标量化“开始生成”按钮处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_BatStart_Click(object sender, AjaxEventArgs e)
        {
            TaskThread = new List<WorkItem>();
            Progress1.UpdateProgress(0, " ");
            Progress1.Text = "";

            this.Session["LongActionProgress"] = "1:";
            TaskThread.Add(AbortableThreadPool.QueueUserWorkItem(new WaitCallback(BatchInitTargetGuide)));
            ScriptManager1.AddScript("{0}.startTask('longactionprogress');", TaskManager1.ClientID);
            Session["TaskThread"] = TaskThread;
        }

        /// <summary>
        /// 生成过程取消事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CloseBatInit(object sender, AjaxEventArgs e)
        {
            if (TaskThread != null)
            {
                Random rand = new Random();
                while (TaskThread.Count > 0)
                {
                    int i = rand.Next(TaskThread.Count);
                    WorkItem item = TaskThread[i];
                    TaskThread.RemoveAt(i);
                    AbortableThreadPool.Cancel(item, true).ToString();
                }
            }
            this.Session.Remove("LongActionProgress");
            this.Session.Remove("LastStep");
            this.Session.Remove("NowCounter");
        }

        /// <summary>
        /// 进度条更新处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RefreshProgress(object sender, AjaxEventArgs e)
        {
            object progress = this.Session["LongActionProgress"];
            if (progress != null)
            {
                int i = Convert.ToInt16(progress.ToString().Split(':')[0]);
                string msg = "";
                if (progress.ToString().Split(':').Length > 1)
                {
                    msg = progress.ToString().Split(':')[1].ToString();
                }
                if (i >= 0)
                {
                    if (this.Session["LastStep"] == null)
                    {
                        this.Session["LastStep"] = "0";
                    }
                    if (this.Session["NowCounter"] == null)
                    {
                        this.Session["NowCounter"] = "0";
                    }

                    if (!(i.ToString().Equals(this.Session["LastStep"].ToString())))
                    {
                        this.Session["LastStep"] = i.ToString();
                        this.Session["NowCounter"] = "0";
                        float p = ((int)(i - 1)) / 3f;
                        Progress1.UpdateProgress(p, (p).ToString("p0"));
                    }
                    else
                    {
                        this.Session["NowCounter"] = (Convert.ToInt16(this.Session["NowCounter"].ToString()) + 1).ToString();
                        float p = (((int)(i - 1)) / 3f + (Convert.ToInt16(this.Session["NowCounter"].ToString()) > 30 ? 0.3f : Convert.ToInt16(this.Session["NowCounter"].ToString()) / 100f));
                        Progress1.UpdateProgress(p, (p).ToString("p0"));
                    }
                    if (Convert.ToInt16(this.Session["NowCounter"].ToString()) % 4 == 0)
                    {
                        progressTip.Text = msg + "，请稍候";
                    }
                    else if (Convert.ToInt16(this.Session["NowCounter"].ToString()) % 4 == 1)
                    {
                        progressTip.Text = msg + "，请稍候.";
                    }
                    else if (Convert.ToInt16(this.Session["NowCounter"].ToString()) % 4 == 2)
                    {
                        progressTip.Text = msg + "，请稍候..";
                    }
                    else if (Convert.ToInt16(this.Session["NowCounter"].ToString()) % 4 == 3)
                    {
                        progressTip.Text = msg + "，请稍候...";
                    }


                }
                else
                {
                    //出错时处理
                    this.Session.Remove("LongActionProgress");
                    this.Session.Remove("LastStep");
                    this.Session.Remove("NowCounter");
                    ScriptManager1.AddScript("{0}.stopTask('longactionprogress');", TaskManager1.ClientID);
                    this.Btn_BatCancel.Text = "退出";
                    Ext.Msg.Alert("系统提示", msg).Show();
                    this.Win_BatchInit.StyleSpec = "cursor: auto;";
                }
            }
            else
            {
                this.Session.Remove("LongActionProgress");
                this.Session.Remove("LastStep");
                this.Session.Remove("NowCounter");
                ScriptManager1.AddScript("{0}.stopTask('longactionprogress');", TaskManager1.ClientID);
                Progress1.UpdateProgress(1, "100%");
                progressTip.Text = "批量量化初始指标已完成!";
                this.Btn_BatCancel.Text = "退出";
                this.Win_BatchInit.StyleSpec = "cursor: auto;";
            }
        }

        /// <summary>
        /// 批量指标量化处理过程
        /// </summary>
        /// <param name="state"></param>
        private void BatchInitTargetGuide(object state)
        {
            //岗位年度
            string stationYear = Convert.ToDateTime(stardate.Value).ToString();
            try
            {
                //目标参照年度
                string targetYear = Convert.ToDateTime(DateField1.Value).ToString("yyyyMM");

                //使用fileupload控件获取上传文件的文件名
                string strName = Path.GetFileName(photoimg.PostedFile.FileName);
                //保存路径
                string savePath = Server.MapPath("../../resources/ExportDataTemp/");
                //文件名
                string FileName = savePath + DateTime.Now.ToFileTimeUtc() + strName;
                //验证 FileUpload 控件确实包含文件
                if (photoimg.HasFile)
                {
                    //将文件保存在服务器中
                    photoimg.PostedFile.SaveAs(FileName);
                }
                //创建实例
                // ExcelHelper excelHelper = new ExcelHelper(FileName);
                this.Session["LongActionProgress"] = "1:正在读取数据(1/2)";
                DataTable dt;
                using (ExcelHelper ex = new ExcelHelper(FileName))
                {
                    dt = ex.ExcelToDataTable(FileName, true);
                }
                this.Session["LongActionProgress"] = "2:正在保存数据(2/2)";
                Appended_income dal = new Appended_income();
                dal.saveAssets(dt, targetYear);


                this.Session.Remove("LongActionProgress");
                this.Session.Remove("LastStep");
                this.Session.Remove("NowCounter");
                //this.Session["LongActionProgress"] = "-1";
            }
            catch (Exception e)
            {
                this.Session["LongActionProgress"] = "-1:数据生成出错，请联系系统管理员!" + e.Message.ToString();
            }
        }

        //导出Excel
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["Assets_List"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["Assets_List"];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "CARD_NUMBER")
                    {
                        dt.Columns[i].ColumnName = "卡片编号";
                    }
                    else if (dt.Columns[i].ColumnName == "FIXED_ASSET_NUMBER")
                    {
                        dt.Columns[i].ColumnName = "固定资产编号";
                    }
                    else if (dt.Columns[i].ColumnName == "NAME_OF_FIXED_ASSETS")
                    {
                        dt.Columns[i].ColumnName = "固定资产名称";
                    }
                    else if (dt.Columns[i].ColumnName == "SPECIFICATION_MODEL")
                    {
                        dt.Columns[i].ColumnName = "规格型号";
                    }
                    else if (dt.Columns[i].ColumnName == "START_DATE")
                    {
                        dt.Columns[i].ColumnName = "开始使用日期";
                    }
                    else if (dt.Columns[i].ColumnName == "USEFUL_LIFE")
                    {
                        dt.Columns[i].ColumnName = "使用年限（月）";
                    }

                    else if (dt.Columns[i].ColumnName == "THE_ORIGINAL_VALUE")
                    {
                        dt.Columns[i].ColumnName = "原值";
                    }

                    else if (dt.Columns[i].ColumnName == "ACCUMULATED_DEPRECIATION")
                    {
                        dt.Columns[i].ColumnName = "累计折旧";
                    }

                    else if (dt.Columns[i].ColumnName == "GB_NAME")
                    {
                        dt.Columns[i].ColumnName = "国标名称";
                    }

                    else if (dt.Columns[i].ColumnName == "GB_CODE")
                    {
                        dt.Columns[i].ColumnName = "国标编码";
                    }

                    else if (dt.Columns[i].ColumnName == "FINANCIAL_SUBSIDY_FUNDS")
                    {
                        dt.Columns[i].ColumnName = "财政补助资金";
                    }
                    else if (dt.Columns[i].ColumnName == "OWN_FUNDS")
                    {
                        dt.Columns[i].ColumnName = "自有资金";
                    }

                    else if (dt.Columns[i].ColumnName == "PLACE_OF_ORIGIN")
                    {
                        dt.Columns[i].ColumnName = "产地";
                    }

                    else if (dt.Columns[i].ColumnName == "MANUFACTURER")
                    {
                        dt.Columns[i].ColumnName = "厂商";
                    }
                    else if (dt.Columns[i].ColumnName == "USE_DEPARTMENT")
                    {
                        dt.Columns[i].ColumnName = "使用部门";
                    }
                    else if (dt.Columns[i].ColumnName == "LOGOUT_DATE")
                    {
                        dt.Columns[i].ColumnName = "注销日期";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPRECIATION_FOR_THE_YEAR")
                    {
                        dt.Columns[i].ColumnName = "本年计提折旧";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPRECIATION_AMOUNT")
                    {
                        dt.Columns[i].ColumnName = "本月计提折旧额";
                    }
                    else if (dt.Columns[i].ColumnName == "MONTHLY_DEPRECIATION_RATE")
                    {
                        dt.Columns[i].ColumnName = "月折旧率";
                    }
                    else if (dt.Columns[i].ColumnName == "ACCRUED_MONTHS")
                    {
                        dt.Columns[i].ColumnName = "已计提月份";
                    }
                    else if (dt.Columns[i].ColumnName == "NET_WORTH")
                    {
                        dt.Columns[i].ColumnName = "净值";
                    }
                    else if (dt.Columns[i].ColumnName == "CLASS_NAME")
                    {
                        dt.Columns[i].ColumnName = "类别名称";
                    }
                    else if (dt.Columns[i].ColumnName == "KJXMZJ_LJZJ")
                    {
                        dt.Columns[i].ColumnName = "科教项目资金累计折旧";
                    }
                    else if (dt.Columns[i].ColumnName == "KJXMZJ")
                    {
                        dt.Columns[i].ColumnName = "科教项目资金";
                    }
                }
                //string dates = D_DATE + "-" + S_DATE;
                ex.ExportToLocal(dt, this.Page, "xls", "固定资产表");
            }

        }
    }
}