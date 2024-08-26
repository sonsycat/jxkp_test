using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Model;

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class xyhs_single_cost_input : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {

                }
                this.stardate.Value = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-1").ToString("yyyy-MM-dd");

                SetDict();
                string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
                string date_time = System.DateTime.Now.ToString("yyyyMM");
                Bindlist(item_code, Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-1").ToString("yyyy-MM-dd"));
                this.Button1.Visible = this.IsEdit();
                this.Buttonfh.Visible = this.IsPass();
                if (this.IsEdit() || this.IsPass())
                {
                    this.Button_no.Visible = true;
                }
                else
                    this.Button_no.Visible = false;
            }
        }

        /// <summary>
        /// 获得成本列表
        /// </summary>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        protected void Bindlist(string item_code, string date_time)
        {
            XyhsDetail dal = new XyhsDetail();
            //DataTable dt = dal.GetSingleCost(item_code, date_time, "0", "").Tables[0];//成本录入0
            //盘锦中心医院用
            DataTable dt = dal.GetSingleCost("", date_time, "0", "").Tables[0];//成本录入0
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
            Bindsubcost(Convert.ToDateTime(date_time).ToString("yyyyMM"));
        }

        /// <summary>
        /// 成本提交信息
        /// </summary>
        /// <param name="months"></param>
        protected void Bindsubcost(string months)
        {
            Cost_detail dal = new Cost_detail();
            User user = (User)Session["CURRENTSTAFF"];
            DataTable dt = dal.GetSubmitcost(months, user.UserId);
            this.Store3.DataSource = dt;
            this.Store3.DataBind();
        }

        /// <summary>
        /// 设置成本下拉框
        /// </summary>
        protected void SetDict()
        {

            XyhsDetail dal = new XyhsDetail();
            DataTable dt = new DataTable();
            //成本项目下拉框
            //用户所具有权限的成本项目
            dt = this.xyhsCostItemFilter();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.COST_ITEM.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["ITEM_NAME"].ToString(), dt.Rows[i]["ITEM_CODE"].ToString()));
            }
            if (dt.Rows.Count > 0)
            {
                this.COST_ITEM.SelectedItem.Value = dt.Rows[0]["ITEM_CODE"].ToString();
            }

        }

       

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_look_click(object sender, EventArgs e)
        {
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM-dd");
            Bindlist(item_code, date_time);
        }


        /// <summary>
        /// 删除按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
            XyhsDetail dal = new XyhsDetail();
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
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
            else if (dal.GetIsSubmit(date_time, item_code))
            {
                this.ShowMessage("系统提示", "该成本已经提交，不能删除！");
            }
            else
            {

                try
                {
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string dept_code = selectRow[i]["ITEM_CODE"];
                        dal.DelSingleCosts(Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM-dd"), item_code, dept_code, "0", "");
                    }
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "删除成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("Store1.reload();");
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_del_click");
                }
            }
        }



        /// <summary>
        /// 获取用友总成本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        protected void Button_get_click(object sender, EventArgs e)
        {
            //string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            XyhsDetail dal = new XyhsDetail();

            string rtMsg = "";
            try
            {
                rtMsg = dal.Exec_YongYou_Cost_Deal(date_time);
                if (rtMsg == "")
                {
                    this.ShowMessage("系统提示", "提取用友成本项目数据成功！");


                    //提取成功后重新调用加载界面 刷新下数据
                    string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
                    //string date_time = System.DateTime.Now.ToString("yyyyMM");
                    Bindlist(item_code, Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-1").ToString("yyyy-MM-dd"));
                
                }
                else
                {
                    this.ShowDataError(rtMsg, Request.Path, "Button_get_click");
                }

            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_get_click");
            }

        }




        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        protected void Item_SelectOnChange(object sender, EventArgs e)
        {
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
        }
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            XyhsDetail dal = new XyhsDetail();
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");

            
            //if (dal.GetIsSubmit(date_time, item_code))
            //{
            //    this.ShowMessage("系统提示", "该成本已经提交，不能再保存了！");
            //    return;
            //}

            List<CostDetail> costdetail = e.Object<CostDetail>();

            User user = (User)Session["CURRENTSTAFF"];
            string operators = user.UserName;
            try
            {
                dal.SaveSingleCosts(costdetail, item_code, Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM-dd"), operators, "0", "");

                //保存物质数据
                //DataTable dt = dal.GetTotalCostsFromViews(date_time).Tables[0];
                //dal.SaveWuzhiZhijieChengben(dt); 
                //////////////

                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("Store1.reload();");
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "保存成功",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
            }
        }
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM-dd");
            string costitem = this.COST_ITEM.SelectedItem.Value.ToString();
            XyhsDetail dal = new XyhsDetail();
            this.Store1.DataSource = dal.GetSingleCost(costitem, date_time, "0", "").Tables[0];
            this.Store1.DataBind();
        }
        /// <summary>
        /// 提交成本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_ok_click(object sender, AjaxEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            if (selectRow == null || selectRow.Length < 1)
            {
                this.SelectRecord();
            }
            else
            {
                try
                {
                    XyhsDetail dal = new XyhsDetail();
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string itemcode = selectRow[i]["ITEM_CODE"];
                        if (!dal.GetIsSubmit(date_time, item_code))
                        {
                            dal.Submitcost(date_time, itemcode, user.UserName);
                        }
                    }
                    this.SaveSucceed();
                    Bindlist(item_code, Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM-dd"));
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_ok_click");
                }
            }
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_sh_click(object sender, AjaxEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            if (selectRow == null || selectRow.Length < 1)
            {
                this.SelectRecord();
            }
            else
            {
                try
                {
                    XyhsDetail dal = new XyhsDetail();
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string itemcode = selectRow[i]["ITEM_CODE"];

                        dal.Checkcost(date_time, itemcode, user.UserName);
                    }
                    this.SaveSucceed();
                    Bindlist(item_code, Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM-dd"));
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_sh_click");
                }
            }
        }
        /// <summary>
        /// 复合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_fh_click(object sender, AjaxEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            if (selectRow == null || selectRow.Length < 1)
            {
                this.SelectRecord();
            }
            else
            {
                try
                {
                    Cost_detail dal = new Cost_detail();
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string itemcode = selectRow[i]["ITEM_CODE"];

                        dal.Compcost(date_time, itemcode, user.UserName);
                    }
                    this.SaveSucceed();
                    Bindlist(item_code, Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM-dd"));
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_fh_click");
                }
            }
        }
        protected void Button_no_click(object sender, AjaxEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            if (selectRow == null || selectRow.Length < 1)
            {
                this.SelectRecord();
            }
            else
            {
                try
                {
                    XyhsDetail dal = new XyhsDetail();
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string itemcode = selectRow[i]["ITEM_CODE"];

                        dal.Canclecost(date_time, itemcode, user.UserName);
                    }
                    this.SaveSucceed();
                    Bindlist(item_code, Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM-dd"));
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_no_click");
                }
            }
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveInfoAll(object sender, AjaxEventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            #region 操作excel文件
            string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
            string[] filename = this.photoimg.PostedFile.FileName.ToString().Split('\\');
            DataSet OleDsExcle = null;
            DataTable dt = null;
            try
            {
                System.Web.HttpFileCollection _files = System.Web.HttpContext.Current.Request.Files;
                string fpath = @"/resources/UploadPicTemp/" + Creater + "temp" + filename[filename.Length - 1].Substring(filename[filename.Length - 1].LastIndexOf("."));
                if (System.IO.File.Exists(Server.MapPath(fpath)))
                {
                    System.IO.File.Delete(Server.MapPath(fpath));
                }
                photoimg.PostedFile.SaveAs(Server.MapPath(fpath));          //执行上传操作
                fpath = Server.MapPath(fpath);
                string strConn;
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fpath + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
                System.Data.OleDb.OleDbConnection OleConn = new System.Data.OleDb.OleDbConnection(strConn);
                OleConn.Open();
                //////////////
                DataTable dtDATA = OleConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                if (dtDATA.Rows.Count < 1)
                    return;
                string SheetName = dtDATA.Rows[0][2].ToString().Trim();
                //////////////

                String sql = "SELECT * FROM  [" + SheetName + "]";//可是更改Sheet名称，比如sheet2，等等   

                System.Data.OleDb.OleDbDataAdapter OleDaExcel = new System.Data.OleDb.OleDbDataAdapter(sql, OleConn);
                OleDsExcle = new DataSet();
                OleDaExcel.Fill(OleDsExcle, SheetName);
                dt = OleDsExcle.Tables[0];
                OleConn.Close();
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex, "GoldNet.JXKP.RLZY.BaseInfoTeach", "SaveInfoAll");
                return;
            }
            #endregion
            int dt_length = dt.Rows.Count;
            XyhsDetail dal = new XyhsDetail();
            #region 检查数据
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ////检查必须填写
                    if (dt.Rows[i][0].ToString().Trim().Length == 0 || dt.Rows[i][1].ToString().Trim().Length == 0)
                    {
                        //this.ShowMessage("系统提示", (i + 1) + "行数据第A、B 列必须填写！");
                        //return;
                        dt_length = i;
                        break;

                    }
                    //////检查长度
                    //if (dt.Rows[i][0].ToString().Trim().Length > 80 || dt.Rows[i][1].ToString().Trim().Length > 20 || dt.Rows[i][2].ToString().Trim().Length > 20
                    //    || dt.Rows[i][3].ToString().Trim().Length > 20 || dt.Rows[i][4].ToString().Trim().Length > 20 || dt.Rows[i][5].ToString().Trim().Length > 200
                    //    || dt.Rows[i][7].ToString().Trim().Length > 30 || dt.Rows[i][8].ToString().Trim().Length > 20
                    //    || dt.Rows[i][9].ToString().Trim().Length > 40 || dt.Rows[i][10].ToString().Trim().Length > 1 || dt.Rows[i][11].ToString().Trim().Length > 60
                    //    || dt.Rows[i][12].ToString().Trim().Length > 1800)
                    //{
                    //    this.ShowMessage("系统提示", (i + 2) + "行数据长度错误！");
                    //    return;
                    //}
                }
            }
            catch (Exception exx)
            {
                this.ShowMessage("系统提示", exx.Message);
                return;
            }
            #endregion
            #region 检查通过添加数据
            try
            {
                // dept_code, string item_code, string accounting_date, string gettype, int medicine_costs, int manage_costs
                string dept_code = "";//科室code
                string item_code = "";//项目
                string accounting_date = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");//月份
                string gettype = "0";
                int medicine_costs = 0;
                int manage_costs = 0;


                int x = dal.DelSingCoste(Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM-dd"), gettype);
                //if (x == 0)
                //{
                //    this.ShowMessage("系统提示", "操作失败");
                //    return;
                //}
                for (int i = 0; i < dt_length; i++)
                {
                    dept_code = dal.GetDept_code(dt.Rows[i][0].ToString().Trim());
                    item_code = dal.GetItem_code(Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM-dd"), dt.Rows[i][1].ToString().Trim());
                    try
                    {
                        medicine_costs = int.Parse(dt.Rows[i][2].ToString().Trim());
                    }
                    catch
                    {
                        medicine_costs = 0;
                    }
                    try
                    {
                        manage_costs = int.Parse(dt.Rows[i][3].ToString().Trim());
                    }
                    catch
                    {
                        manage_costs = 0;
                    }
                    if (item_code != "" && dept_code != "")
                    {
                        dal.SaveAllSingleCoste(dept_code, item_code, Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM-dd"), gettype, medicine_costs, manage_costs);

                    }
                    else
                    {
                        this.ShowMessage("系统提示", i + 1 + "行" + dt.Rows[i][0].ToString().Trim() + "或" + dt.Rows[i][1].ToString().Trim() + "不存在");
                        return;
                    }
                }
            }
            catch (Exception ess)
            {
                this.ShowDataError(ess, "GoldNet.JXKP.RLZY.BaseInfoTeach", "SaveInfo");
                return;
            }
            try
            {
                dal.SaveTatalCostsFromViews(Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM"));
                dal.SaveEXPORT_COSTS(Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM"));
            }
            catch (Exception exxx)
            {

            }
            this.SaveSucceed();
            Bindlist("", Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM-dd"));
            #endregion
        }

    }
}
