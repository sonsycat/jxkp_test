using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Text;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class StaffInfoUpLoad : PageBase
    {
        private static DataTable ExcelTable = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }
            if (!Ext.IsAjaxRequest)
            {
                for (int i = 1; i <= 12; i++)
                {
                    this.Comb_StartMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
                }
                this.Comb_StartMonth.SelectedIndex = DateTime.Now.Month - 1;
            }
        }

        /// <summary>
        /// 自定义报表
        /// </summary>
        /// <param name="ReportInfo">报表信息</param>
        /// <param name="ReportColName">指标名称</param>
        private void CreateReportPanel(DataTable ReportInfo)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            this.Store1.RemoveFields();
            this.GridPanel_Show.Reconfigure();
            for (int i = 0; i < ReportInfo.Columns.Count; i++)
            {
                RecordField field = new RecordField(ReportInfo.Columns[i].ColumnName, RecordFieldType.String);
                this.Store1.AddField(field, i);
                Column col = new Column();
                col.Header = ReportInfo.Columns[i].ColumnName;
                col.Width = ascii.GetBytes(col.Header).Length * 10;
                col.Sortable = true;
                col.DataIndex = ReportInfo.Columns[i].ColumnName;
                col.Align = Alignment.Right;
                this.GridPanel_Show.AddColumn(col);
            }
            this.Store1.DataSource = ReportInfo;
            this.Store1.DataBind();
        }


        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
            string type = e.ExtraParams["Type"].ToString().Replace("\"", "");
            DataTable l_dt = new DataTable();
            DataTable col_dt = new DataTable();
            string month = this.Comb_StartMonth.SelectedItem.Text.Length == 1 ? "0" + this.Comb_StartMonth.SelectedItem.Text : this.Comb_StartMonth.SelectedItem.Text;
            string date1 = this.NumYear.Text + month + "01";
            string date2 = this.NumYear.Text + "-" + month + "-01";
            switch (type)
            {
                case "0":
                    l_dt = dal.ViewUnitDetail("").Tables[0];
                    break;
                case "1":
                    l_dt = dal.getInformationInfo().Tables[0];
                    break;
                case "2":
                    l_dt = dal.getSpecCenterInfo(this.NumYear.Text).Tables[0];
                    break;
                case "3":
                    l_dt = dal.getDeptInfo().Tables[0];
                    break;
                case "4":
                    //人员信息
                    l_dt = dal.getStaffInfo(date1).Tables[0];

                    col_dt = dal.getStaffInfo().Tables[0];

                    for (int i = 0; i < col_dt.Rows.Count; i++)
                    {
                        l_dt.Columns[i].ColumnName = col_dt.Rows[i]["COMMENTS"].ToString();
                    }

                    //    #region 列名
                    //    l_dt.Columns[0].ColumnName = "序号";
                    //l_dt.Columns[1].ColumnName = "科室代码";
                    //l_dt.Columns[2].ColumnName = "科室名称";
                    //l_dt.Columns[3].ColumnName = "姓名";
                    //l_dt.Columns[4].ColumnName = "是否军人";
                    //l_dt.Columns[5].ColumnName = "是否审核";
                    //l_dt.Columns[6].ColumnName = "是否在岗";
                    //l_dt.Columns[7].ColumnName = "生日";
                    //l_dt.Columns[8].ColumnName = "性别";
                    //l_dt.Columns[9].ColumnName = "民族";
                    //l_dt.Columns[10].ColumnName = "奖金系数";
                    //l_dt.Columns[11].ColumnName = "政府津贴";
                    //l_dt.Columns[12].ColumnName = "干部类别";
                    //l_dt.Columns[13].ColumnName = "毕业时间";
                    //l_dt.Columns[14].ColumnName = "所在科室类";
                    //l_dt.Columns[15].ColumnName = "学历";
                    //l_dt.Columns[16].ColumnName = "所学专业";
                    //l_dt.Columns[17].ColumnName = "来院时间";
                    //l_dt.Columns[18].ColumnName = "应发工资";
                    //l_dt.Columns[19].ColumnName = "受聘期限";
                    //l_dt.Columns[20].ColumnName = "技术职务";
                    //l_dt.Columns[21].ColumnName = "技术职务时间";
                    //l_dt.Columns[22].ColumnName = "人员类别";
                    //l_dt.Columns[23].ColumnName = "入伍时间";
                    //l_dt.Columns[24].ColumnName = "工作时间";
                    //l_dt.Columns[25].ColumnName = "行政职务";
                    //l_dt.Columns[26].ColumnName = "行政职务时间";
                    //l_dt.Columns[27].ColumnName = "技术级";
                    //l_dt.Columns[28].ColumnName = "技术级时间";
                    //l_dt.Columns[29].ColumnName = "文职级";
                    //l_dt.Columns[30].ColumnName = "文职级时间";
                    //l_dt.Columns[31].ColumnName = "卫生专业分类";
                    //l_dt.Columns[32].ColumnName = "从事专业";
                    //l_dt.Columns[33].ColumnName = "医疗卡账号";
                    //l_dt.Columns[34].ColumnName = "医疗卡号";
                    //l_dt.Columns[35].ColumnName = "录入人";
                    //l_dt.Columns[36].ColumnName = "录入时间";
                    //l_dt.Columns[37].ColumnName = "月份时间";
                    //l_dt.Columns[38].ColumnName = "卫勤抽组";
                    //l_dt.Columns[39].ColumnName = "分组情况";
                    //l_dt.Columns[40].ColumnName = "担任职务";
                    //l_dt.Columns[41].ColumnName = "人员类型";
                    //l_dt.Columns[42].ColumnName = "变动情况";
                    //l_dt.Columns[43].ColumnName = "变动时间";
                    //l_dt.Columns[44].ColumnName = "变动原因";
                    //l_dt.Columns[45].ColumnName = "变动备注";
                    //l_dt.Columns[46].ColumnName = "出生地点";
                    //l_dt.Columns[47].ColumnName = "证件号码";
                    //l_dt.Columns[48].ColumnName = "婚姻状况";
                    //l_dt.Columns[49].ColumnName = "职称序列";
                    //l_dt.Columns[50].ColumnName = "学位";
                    //l_dt.Columns[51].ColumnName = "毕业院校";
                    //l_dt.Columns[52].ColumnName = "获得学历时间";
                    //l_dt.Columns[53].ColumnName = "军衔";
                    //l_dt.Columns[54].ColumnName = "职称";
                    //l_dt.Columns[55].ColumnName = "核算组代码";
                    //l_dt.Columns[56].ColumnName = "备注";
                    //l_dt.Columns[57].ColumnName = "核算人";

                    ////l_dt.Columns[58].ColumnName = "审批人";
                    ////l_dt.Columns[59].ColumnName = "USERID";
                    ////l_dt.Columns[60].ColumnName = "输入代码";
                    ////l_dt.Columns[61].ColumnName = "工作职务";
                    ////l_dt.Columns[62].ColumnName = "工作职务时间";
                    //////l_dt.Columns[63].ColumnName = "核算人";


                    //#endregion
                    break;
                case "5":
                    l_dt = dal.getSpecMediInfo(date2).Tables[0];
                    break;
                case "6":
                    l_dt = dal.getProblemInfo(date2).Tables[0];
                    break;
                case "7":
                    l_dt = dal.getFriutInfo(date2).Tables[0];
                    break;
                case "8":
                    l_dt = dal.getMongraphInfo(date2).Tables[0];
                    break;
                case "9":
                    l_dt = dal.getDiscourseInfo(date2).Tables[0];
                    break;
                case "10":
                    l_dt = dal.getTechMeetingInfo(date2).Tables[0];
                    break;
                case "11":
                    l_dt = dal.getDevNewTechInfo(date2).Tables[0];
                    break;
                case "12":
                    l_dt = dal.getPersonsInfo(date2).Tables[0];
                    break;
            }

            ExcelTable = new DataTable();
            ExcelTable = l_dt;
            CreateReportPanel(l_dt);
        }


        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void InsertLoad(object sender, AjaxEventArgs e)
        {
            BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
            DataTable l_dt = PersTypeFilter();
            string sort = "";
            for (int i = 0; i < l_dt.Rows.Count; i++)
            {
                sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
            }
            if (l_dt.Rows.Count == 0)
            {
                sort = "'-1'";
            }
            string month = this.Comb_StartMonth.SelectedItem.Text.Length == 1 ? "0" + this.Comb_StartMonth.SelectedItem.Text : this.Comb_StartMonth.SelectedItem.Text;

            string date2 = this.NumYear.Text + "-" + month + "-01";
            if (dal.getStaffInfoType(sort.TrimEnd(new char[] { ',' })))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "数据未全部审批",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            string userdate = Convert.ToDateTime(date2).ToString("yyyyMMdd");
            dal.deleteStaffInfoInput(sort.TrimEnd(new char[] { ',' }), userdate);
            dal.InsertStaffInfoInput(sort.TrimEnd(new char[] { ',' }), userdate);
        }


        protected void OutExcel(object sender, EventArgs e)
        {
            if (this.cboUploadType.SelectedItem.Value == "")
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "请选择导出信息类别",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            ExportData ex = new ExportData();

            BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
            string type = this.cboUploadType.SelectedItem.Value;
            DataTable l_dt = new DataTable();
            DataTable col_dt = new DataTable();
            string month = this.Comb_StartMonth.SelectedItem.Text.Length == 1 ? "0" + this.Comb_StartMonth.SelectedItem.Text : this.Comb_StartMonth.SelectedItem.Text;
            string date1 = this.NumYear.Text + month + "01";
            string date2 = this.NumYear.Text + "-" + month + "-01";
            switch (type)
            {
                case "0":
                    l_dt = dal.ViewUnitDetail("").Tables[0];
                    break;
                case "1":
                    l_dt = dal.getInformationInfo().Tables[0];
                    break;
                case "2":
                    l_dt = dal.getSpecCenterInfo(this.NumYear.Text).Tables[0];
                    break;
                case "3":
                    l_dt = dal.getDeptInfo().Tables[0];
                    break;
                case "4":
                    l_dt = dal.getStaffInfo(date1).Tables[0];

                    col_dt = dal.getStaffInfo().Tables[0];

                    for (int i = 0; i < col_dt.Rows.Count; i++)
                    {
                        l_dt.Columns[i].ColumnName = col_dt.Rows[i]["COMMENTS"].ToString();
                    }

                    //#region 列名
                    //l_dt.Columns[0].ColumnName = "序号";
                    //l_dt.Columns[1].ColumnName = "科室代码";
                    //l_dt.Columns[2].ColumnName = "科室名称";
                    //l_dt.Columns[3].ColumnName = "姓名";
                    //l_dt.Columns[4].ColumnName = "是否军人";
                    //l_dt.Columns[5].ColumnName = "是否审核";
                    //l_dt.Columns[6].ColumnName = "是否在岗";
                    //l_dt.Columns[7].ColumnName = "生日";
                    //l_dt.Columns[8].ColumnName = "性别";
                    //l_dt.Columns[9].ColumnName = "民族";
                    //l_dt.Columns[10].ColumnName = "奖金系数";
                    //l_dt.Columns[11].ColumnName = "政府津贴";
                    //l_dt.Columns[12].ColumnName = "干部类别";
                    //l_dt.Columns[13].ColumnName = "毕业时间";
                    //l_dt.Columns[14].ColumnName = "所在科室类";
                    //l_dt.Columns[15].ColumnName = "学历";
                    //l_dt.Columns[16].ColumnName = "所学专业";
                    //l_dt.Columns[17].ColumnName = "来院时间";
                    //l_dt.Columns[18].ColumnName = "应发工资";
                    //l_dt.Columns[19].ColumnName = "受聘期限";
                    //l_dt.Columns[20].ColumnName = "技术职务";
                    //l_dt.Columns[21].ColumnName = "技术职务时间";
                    //l_dt.Columns[22].ColumnName = "人员类别";
                    //l_dt.Columns[23].ColumnName = "入伍时间";
                    //l_dt.Columns[24].ColumnName = "工作时间";
                    //l_dt.Columns[25].ColumnName = "行政职务";
                    //l_dt.Columns[26].ColumnName = "行政职务时间";
                    //l_dt.Columns[27].ColumnName = "技术级";
                    //l_dt.Columns[28].ColumnName = "技术级时间";
                    //l_dt.Columns[29].ColumnName = "文职级";
                    //l_dt.Columns[30].ColumnName = "文职级时间";
                    //l_dt.Columns[31].ColumnName = "卫生专业分类";
                    //l_dt.Columns[32].ColumnName = "从事专业";
                    //l_dt.Columns[33].ColumnName = "医疗卡账号";
                    //l_dt.Columns[34].ColumnName = "医疗卡号";
                    //l_dt.Columns[35].ColumnName = "录入人";
                    //l_dt.Columns[36].ColumnName = "录入时间";
                    //l_dt.Columns[37].ColumnName = "月份时间";
                    //l_dt.Columns[38].ColumnName = "卫勤抽组";
                    //l_dt.Columns[39].ColumnName = "分组情况";
                    //l_dt.Columns[40].ColumnName = "担任职务";
                    //l_dt.Columns[41].ColumnName = "人员类型";
                    //l_dt.Columns[42].ColumnName = "变动情况";
                    //l_dt.Columns[43].ColumnName = "变动时间";
                    //l_dt.Columns[44].ColumnName = "变动原因";
                    //l_dt.Columns[45].ColumnName = "变动备注";
                    //l_dt.Columns[46].ColumnName = "出生地点";
                    //l_dt.Columns[47].ColumnName = "证件号码";
                    //l_dt.Columns[48].ColumnName = "婚姻状况";
                    //l_dt.Columns[49].ColumnName = "职称序列";
                    //l_dt.Columns[50].ColumnName = "学位";
                    //l_dt.Columns[51].ColumnName = "毕业院校";
                    //l_dt.Columns[52].ColumnName = "获得学历时间";
                    //l_dt.Columns[53].ColumnName = "军衔";
                    //l_dt.Columns[54].ColumnName = "职称";
                    //l_dt.Columns[55].ColumnName = "核算组代码";
                    //l_dt.Columns[56].ColumnName = "备注";
                    //l_dt.Columns[57].ColumnName = "核算人";
                    //#endregion
                    break;
                case "5":
                    l_dt = dal.getSpecMediInfo(date2).Tables[0];
                    break;
                case "6":
                    l_dt = dal.getProblemInfo(date2).Tables[0];
                    break;
                case "7":
                    l_dt = dal.getFriutInfo(date2).Tables[0];
                    break;
                case "8":
                    l_dt = dal.getMongraphInfo(date2).Tables[0];
                    break;
                case "9":
                    l_dt = dal.getDiscourseInfo(date2).Tables[0];
                    break;
                case "10":
                    l_dt = dal.getTechMeetingInfo(date2).Tables[0];
                    break;
                case "11":
                    l_dt = dal.getDevNewTechInfo(date2).Tables[0];
                    break;
                case "12":
                    l_dt = dal.getPersonsInfo(date2).Tables[0];
                    break;
            }

            ExcelTable = new DataTable();
            ExcelTable = l_dt;

            ex.ExportToLocal(ExcelTable, this.Page, "xls", this.cboUploadType.SelectedItem.Text);
        }

        protected void OutXml(object sender, EventArgs e)
        {
            BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
            DataSet allData = new DataSet();

            string month = this.Comb_StartMonth.SelectedItem.Text.Length == 1 ? "0" + this.Comb_StartMonth.SelectedItem.Text : this.Comb_StartMonth.SelectedItem.Text;
            string date1 = this.NumYear.Text + month + "01";
            string date2 = this.NumYear.Text + "-" + month + "-01";
            dal.UpdateStaffInfoLoad();

            DataTable l_dt13 = dal.getStaffInfoLoadTime().Tables[0];
            DataTable l_InfoLoadTime = l_dt13.Copy();
            l_InfoLoadTime.TableName = "UPDATE_DATA";
            allData.Tables.Add(l_InfoLoadTime);


            DataTable l_dt = dal.ViewUnitDetail("").Tables[0];
            DataTable l_Unit = l_dt.Copy();
            l_Unit.TableName = "HOSP_NAME_DICT";
            allData.Tables.Add(l_Unit);

            DataTable l_dt12 = dal.getInformationInfo().Tables[0];
            DataTable l_dtInformation = l_dt12.Copy();
            l_dtInformation.TableName = "INFORMATION";
            allData.Tables.Add(l_dtInformation);

            DataTable l_dt1 = dal.getSpecCenterInfo(this.NumYear.Text).Tables[0];
            DataTable l_dtSpecCenterInfo = l_dt1.Copy();
            l_dtSpecCenterInfo.TableName = "SPEC_CENTER";
            allData.Tables.Add(l_dtSpecCenterInfo);


            DataTable l_dt2 = dal.getDeptInfo().Tables[0];
            DataTable l_dtDeptInfo = l_dt2.Copy();
            l_dtDeptInfo.TableName = "DEPT_INFO";
            allData.Tables.Add(l_dtDeptInfo);

            DataTable l_dt3 = dal.getStaffInfo(date1).Tables[0];
            DataTable l_dtStaffInfo = l_dt3.Copy();
            l_dtStaffInfo.TableName = "NEW_STAFF_INFO";
            allData.Tables.Add(l_dtStaffInfo);

            DataTable l_dt4 = dal.getSpecMediInfo(date2).Tables[0];
            DataTable l_dtSpecMediInfo = l_dt4.Copy();
            l_dtSpecMediInfo.TableName = "SPEC_MEDI";
            allData.Tables.Add(l_dtSpecMediInfo);

            DataTable l_dt5 = dal.getProblemInfo(date2).Tables[0];
            DataTable l_dtProblemInfo = l_dt5.Copy();
            l_dtProblemInfo.TableName = "PROBLEM_INFO";
            allData.Tables.Add(l_dtProblemInfo);

            DataTable l_dt6 = dal.getFriutInfo(date2).Tables[0];
            DataTable l_dtFriutInfo = l_dt6.Copy();
            l_dtFriutInfo.TableName = "FRUIT";
            allData.Tables.Add(l_dtFriutInfo);


            DataTable l_dt7 = dal.getMongraphInfo(date2).Tables[0];
            DataTable l_dtMongraphInfo = l_dt7.Copy();
            l_dtMongraphInfo.TableName = "MONOGRAPH";
            allData.Tables.Add(l_dtMongraphInfo);

            DataTable l_dt8 = dal.getDiscourseInfo(date2).Tables[0];
            DataTable l_dtDiscourseInfo = l_dt8.Copy();
            l_dtDiscourseInfo.TableName = "DISCOURSE";
            allData.Tables.Add(l_dtDiscourseInfo);

            DataTable l_dt9 = dal.getTechMeetingInfo(date2).Tables[0];
            DataTable l_dtTechMeetingInfo = l_dt9.Copy();
            l_dtTechMeetingInfo.TableName = "CENTER_DEPTDETAIL";
            allData.Tables.Add(l_dtTechMeetingInfo);


            DataTable l_dt10 = dal.getDevNewTechInfo(date2).Tables[0];
            DataTable l_dtDevNewTechInfo = l_dt10.Copy();
            l_dtDevNewTechInfo.TableName = "DEVELOP_NEW_TECHNIC";
            allData.Tables.Add(l_dtDevNewTechInfo);

            DataTable l_dt11 = dal.getPersonsInfo(date2).Tables[0];
            DataTable l_dtPersonsInfo = l_dt11.Copy();
            l_dtPersonsInfo.TableName = "PERSONS_PLANT_INFO";
            allData.Tables.Add(l_dtPersonsInfo);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            allData.WriteXml(ms, System.Data.XmlWriteMode.WriteSchema);

            Response.Clear();
            string filenames = "bcbb" + ".xml";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filenames);
            Response.AddHeader("Content-Length", ms.Length.ToString());
            Response.ContentType = "application/octet-stream";
            //Response
            byte[] b = ms.ToArray();
            Response.OutputStream.Write(b, 0, b.Length);
            Response.End();

        }

    }
}
