using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Model;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class StaffWorkday : PageBase
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    //Response.End();
                }

                //初始化查询日期
                string year = DateTime.Now.AddMonths(-1).Year.ToString();
                string month = DateTime.Now.AddMonths(-1).Month.ToString();
                //年度下拉列表
                BoundComm boundcomm = new BoundComm();
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.Value = year;
                //
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;

                SetStoreProxy();

                string deptcode = ((User)Session["CURRENTSTAFF"]).AccountDeptCode;

                this.cbbdept.Value = deptcode;

                //获取数据并绑定
                Bindlist(GetBeginDate(), deptcode);

                //按钮权限控制
                this.Button_save.Visible = this.IsEdit();
                this.Button_del.Visible = this.IsEdit();
                this.Button_check.Visible = this.IsPass();
            }
        }

        /// <summary>
        /// 设置下拉控件数据
        /// </summary>
        private void SetStoreProxy()
        {

            BaseInfoMaintainDal tdal = new BaseInfoMaintainDal();

            string deptcode = this.DeptFilter("");

            //科室下拉列表初始化
            HttpProxy pro2 = new HttpProxy();
            pro2.Method = HttpMethod.POST;
            pro2.Url = "/RLZY/WebService/DeptDict.ashx?deptfilter=" + deptcode;
            this.Store2.Proxy.Add(pro2);

            HttpProxy pro3 = new HttpProxy();
            pro3.Method = HttpMethod.POST;
            pro3.Url = "/RLZY/WebService/StaffInfos.ashx";
            this.Store3.Proxy.Add(pro3);

            DataTable dt = tdal.GetAttendanceDict().Tables[0];
            this.Store4.DataSource = dt;
            this.Store4.DataBind();

            DataTable dt1 = tdal.GetAttendanceMemo().Tables[0];
            this.Store5.DataSource = dt1;
            this.Store5.DataBind();
        }

        /// <summary>
        /// 数据获取并绑定
        /// </summary>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        private void Bindlist(string datetime, string deptcode)
        {
            string conditin = this.DeptFilter("");

            if (deptcode.Equals(""))
            {
                deptcode = conditin;
            }
            else
            {
                deptcode = "'" + deptcode + "'";
            }

            BaseInfoMaintainDal tdal = new BaseInfoMaintainDal();
            string inputuser = ((User)Session["CURRENTSTAFF"]).UserName;
            DataTable dt = tdal.GetAttendanceInfo(datetime, deptcode, inputuser).Tables[0];

            //DataRow rw = dt.NewRow();
            //dt.Rows.Add(rw);
            this.Store1.DataSource = dt;
            this.Store1.DataBind();

            this.summoney.Text = "合计：" + dt.Compute("Count(STAFF_NAME)", "").ToString();

            Session.Remove("staffworkday");
            Session["staffworkday"] = dt;
        }

        /// <summary>
        /// 获取查询日期
        /// </summary>
        /// <returns></returns>
        private string GetBeginDateBySelec()
        {
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + month + "01";
            return benginDate;
        }

        /// <summary>
        /// 获取查询日期
        /// </summary>
        /// <returns></returns>
        private string GetBeginDate()
        {
            string year = cbbYear.Value.ToString();
            string month = cbbmonth.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + month + "01";
            return benginDate;
        }

        /// <summary>
        /// 反序列化得到客户端提交的gridpanel数据行
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_look_click(object sender, EventArgs e)
        {
            //选择科室
            string deptcode = this.cbbdept.SelectedItem.Value.ToString();

            Bindlist(GetBeginDateBySelec(), deptcode);
        }

        /// <summary>
        /// 保存处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Save_click(object sender, AjaxEventArgs e)
        {
            BaseInfoMaintainDal tdal = new BaseInfoMaintainDal();

            Dictionary<string, string>[] selectRow = GetSelectRow(e);

            if (selectRow != null)
            {
                string deptcode = this.cbbdept.SelectedItem.Value.ToString();
                string datetime = GetBeginDateBySelec();
                string inputuser = ((User)Session["CURRENTSTAFF"]).UserName;

                if (deptcode.Equals(""))
                {
                    this.ShowMessage("信息提示", "请选择科室！");
                    return;
                }

                for (int i = 0; i < selectRow.Length; i++)
                {
                    if (selectRow[i]["ATTENDANCE_NAME"] == "代出勤" && selectRow[i]["MEMO"] == "")
                    {
                        this.ShowMessage("信息提示", "选择代出勤后备注不能为空！");
                        return;
                    }

                    if (selectRow[i]["ATTENDANCE_NAME"] == "应出勤" && selectRow[i]["ATTENDANCE_VALUE"] != selectRow[i]["JZ"] && selectRow[i]["MEMO"] == "")
                    {
                        this.ShowMessage("信息提示", "应出勤天数未满勤，请填写备注内容！");
                        return;
                    }
                }

                try
                {
                    tdal.SaveAttendanceInfo(selectRow, datetime, deptcode, inputuser);

                    this.ShowMessage("信息提示", "数据保存成功！");

                    Bindlist(GetBeginDateBySelec(), deptcode);

                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
                }
            }
        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["staffworkday"] != null)
            {
                //ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["staffworkday"];

                MHeaderTabletoExcel(dt, null, "考勤结果", null, 0);
                //ex.ExportToLocal(l_dt, this.Page, "xls", "人员信息");
            }
        }

        /// <summary>
        /// 考勤审核处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void check_click(object sender, AjaxEventArgs e)
        {
            BaseInfoMaintainDal tdal = new BaseInfoMaintainDal();

            Dictionary<string, string>[] selectRow = GetSelectRow(e);

            if (selectRow != null)
            {
                string deptcode = this.cbbdept.SelectedItem.Value.ToString();
                string datetime = GetBeginDateBySelec();
                string inputuser = ((User)Session["CURRENTSTAFF"]).UserName;
                try
                {
                    tdal.UpdateAttendanceInfo(selectRow, datetime, deptcode, inputuser);

                    this.ShowMessage("信息提示", "数据审核成功！");

                    Bindlist(GetBeginDateBySelec(), deptcode);

                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
                }
            }
        }

        /// <summary>
        /// 提交处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void commit_click(object sender, AjaxEventArgs e)
        {
            BaseInfoMaintainDal tdal = new BaseInfoMaintainDal();

            Dictionary<string, string>[] selectRow = GetSelectRow(e);

            if (selectRow != null)
            {
                string deptcode = this.cbbdept.SelectedItem.Value.ToString();
                string datetime = GetBeginDateBySelec();
                string inputuser = ((User)Session["CURRENTSTAFF"]).UserName;
                try
                {
                    tdal.UpdateAttendanceInfoCommit(selectRow, datetime, deptcode, inputuser);

                    this.ShowMessage("信息提示", "数据提交成功！");

                    Bindlist(GetBeginDateBySelec(), deptcode);

                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
                }
            }
        }

        /// <summary>
        /// 考勤删除处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
            BaseInfoMaintainDal tdal = new BaseInfoMaintainDal();

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
                        string datetime = selectRow[i]["YEAR_MONTH"];
                        string deptcode = selectRow[i]["DEPT_CODE"];
                        string attcode = selectRow[i]["ATTENDANCE_CODE"];
                        string staffid = selectRow[i]["STAFF_ID"];

                        if (!deptcode.Equals("") && !datetime.Equals("") && !attcode.Equals("") && !staffid.Equals(""))
                        {
                            tdal.DelAttendanceInfo(datetime, deptcode, attcode,staffid);
                        }
                    }

                    this.ShowMessage("信息提示", "数据删除成功！");

                    Bindlist(GetBeginDateBySelec(), this.cbbdept.SelectedItem.Value.ToString());
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_del_click");
                }
            }
        }

        /// <summary>
        /// 科室调整调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveInfo(object sender, AjaxEventArgs e)
        {
            //获取参数
            string id = e.ExtraParams["Staffid"].ToString().Replace("\"", "");
            string oldDpetCode = e.ExtraParams["staffOldDeptCode"].ToString().Replace("\"", "");
            string oldDpetName = e.ExtraParams["staffOldDeptName"].ToString().Replace("\"", "");
            string Name = e.ExtraParams["StaffName"].ToString().Replace("\"", "");
            
            BaseInfoMaintainDal dal = new BaseInfoMaintainDal();

            if (dal.isExStaffInfoByName(this.cboChangeDept.SelectedItem.Value, this.staffName.Text))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "调动科室不能有重复姓名人员",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }

            dal.InsertStaffChangeInfo(id, Name, oldDpetCode, oldDpetName, this.staffName.Text, this.cboChangeDept.SelectedItem.Value,
                this.cboChangeDept.SelectedItem.Text, ((User)Session["CURRENTSTAFF"]).UserName, id, "");

            string deptcode = this.cbbdept.SelectedItem.Value.ToString();
            Bindlist(GetBeginDateBySelec(), deptcode);
        }

    }
}
