using System;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Collections.Generic;
using System.IO;
using Goldnet.Comm.ExportData;
using GoldNet.Comm;
using System.Threading;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class JJ_DR : PageBase
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

                BoundComm boundcomm = new BoundComm();
                //年月下拉列表初始化
                Store3.DataSource = boundcomm.getYears();
                Store3.DataBind();
                cbbYear.SetValue(DateTime.Now.Year);
                Store4.DataSource = boundcomm.getMonth();
                Store4.DataBind();
                cbbmonth.SetValue(DateTime.Now.Month);
                //获取数据并绑定

                SetStoreProxy();

                data(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

            }
        }

        /// <summary>
        /// 设置科室列表
        /// </summary>
        private void SetStoreProxy()
        {
            //科室下拉列表初始化
            HttpProxy pro2 = new HttpProxy();
            pro2.Method = HttpMethod.POST;
            //pro2.Url = "../WebService/BonusDepts.ashx";
            pro2.Url = "../../../WebService/Depts.ashx";
            this.Store2.Proxy.Add(pro2);
        }

        /// <summary>
        /// 获取相关指标数据并绑定
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        private void data(string year, string month)
        {
            string deptcode = this.DeptFilter("");
            DeptPercent deptpercent = new DeptPercent();
            DataTable table = deptpercent.Bonus_Jjdr(year, month,deptcode);
            
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Query(object sender, AjaxEventArgs e)
        {
            data(this.cbbYear.SelectedItem.Value.ToString(), this.cbbmonth.SelectedItem.Value.ToString());
        }


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save(object sender, AjaxEventArgs e)
        {
            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中            
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                DeptPercent deptpercent = new DeptPercent();
                if (deptpercent.SaveBonus_Jjdr(selectRow, cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString()))
                {
                    this.ShowMessage("系统提示", "保存成功！");
                    data(this.cbbYear.SelectedItem.Value.ToString(), this.cbbmonth.SelectedItem.Value.ToString());
                }
                else
                {
                    ShowDataError("", Request.Url.LocalPath, "Save");
                }
            }
        }
        /// <summary>
        /// 删除操作处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
            Cost_detail dal = new Cost_detail();
            Cbhs_dict dal_dict = new Cbhs_dict();
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
                try
                {
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string dept_code = selectRow[i]["DEPT_CODE"];
                        string user_id = selectRow[i]["USER_ID"];
                        string date =this.cbbYear.SelectedItem.Value.ToString() +"0"+this.cbbmonth.SelectedItem.Value.ToString()+"01";
                        if (!dept_code.Equals(""))
                        {

                            dal.Del_Jjdr(date, user_id, dept_code);
                            
                        }
                    }

                    this.ShowMessage("信息提示", "删除成功！");
                    Data_RefreshData(null, null);
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_del_click");
                }
            }
        }

        /// <summary>
        /// 数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            data(this.cbbYear.SelectedItem.Value.ToString(), this.cbbmonth.SelectedItem.Value.ToString());
            
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
                dal.saveJjdr(dt, targetYear);


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
            
                DeptPercent deptpercent = new DeptPercent();
                DataTable dt = deptpercent.Bonus_Jjdr(this.cbbYear.SelectedItem.Value.ToString(), this.cbbmonth.SelectedItem.Value.ToString(), this.DeptFilter(""));

                ExportData ex = new ExportData();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "COSTS")
                    {
                        dt.Columns[i].ColumnName = "金额";
                    }
                    else if (dt.Columns[i].ColumnName == "USER_ID")
                    {
                        dt.Columns[i].ColumnName = "姓名";
                    }
                    
                }
                ex.ExportToLocal(dt, this.Page, "xls", "" + this.cbbYear.SelectedItem.Value.ToString() + "年" + this.cbbmonth.SelectedItem.Value.ToString() + "月,个人奖金表");
            

        }
    }
}