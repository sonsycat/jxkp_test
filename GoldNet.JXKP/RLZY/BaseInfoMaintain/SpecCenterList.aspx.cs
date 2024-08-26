using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class SpecCenterList : PageBase
    {
        BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                DataTable l_dt = dal.ViewUnitDetail("").Tables[0];
                if (l_dt.Rows.Count > 0)
                {
                    txtUnitcode.Text = l_dt.Rows[0]["UNIT_CODE"].ToString();
                    txtVicedirector.Text = l_dt.Rows[0]["UNIT_NAME"].ToString();
                    txtUnittype.Text = l_dt.Rows[0]["CHARACTER"].ToString();
                }
                else 
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "请添加单位基本信息",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                
                }
                SetCombox();
                this.btnSaveSpecInfo.Visible = this.IsEdit();
            }
        }

        /// <summary>
        /// 初始化Combox
        /// </summary>
        private void SetCombox()
        {
            DictMainTainDal dictdal = new DictMainTainDal();
            DataTable cboDt = null;

            //所属中心类别字典
            cboDt = dictdal.getDictInfo("DEPT_SORT_DICT", "SEID", "SORT_NAME", false, "").Tables[0];
            SetCboData("SEID", "SORT_NAME", cboDt, cboSpectype);
            cboSpectype.SelectedIndex = 0;

            //学历字典
            cboDt = dictdal.getDictInfo("LEARNSUFFER_DICT", "ID", "LEARNSUFFER", true, "IS_DEL").Tables[0];
            SetCboData("ID", "LEARNSUFFER", cboDt, cboLeadedu);

            //职称字典
            cboDt = dictdal.getDictInfo("JOB_DICT", "ID", "JOB", true, "IS_DEL").Tables[0];
            SetCboData("ID", "JOB", cboDt, cboJob);

            //中心字典
            cboDt = dictdal.getSpceCenterListDictInfo().Tables[0];
            for (int idx = 0; idx < cboDt.Rows.Count; idx++)
            {
                cboSpceCenterInfo.Items.Add(new Goldnet.Ext.Web.ListItem(cboDt.Rows[idx]["CENTER_NAME"].ToString(), cboDt.Rows[idx]["CENTER_CODE"].ToString()));
            }
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchInfo(object sender, AjaxEventArgs e)
        {
            this.btnDept.Disabled = true;
            DataTable l_dt = dal.ViewSpecCenterListInfo(this.cboSpceCenterInfo.SelectedItem.Value, this.NumYear.Text).Tables[0];
            setContrls(l_dt);
            if(l_dt.Rows.Count > 0) {
               this.btnDept.Disabled = false;
            }
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void InsertInfo(object sender, AjaxEventArgs e)
        {
            string statMonth = this.NumYear.Text;
            string unitCode = txtUnitcode.Text;
            string unitName = txtVicedirector.Text;
            string Unittype = txtUnittype.Text;
            string Speccode = txtSpeccode.Text;
            string Specname = txtSpecname.Text;
            string Spectype = cboSpectype.SelectedItem.Text;
            string Leadname = txtLeadname.Text;
            string Leadedu = cboLeadedu.SelectedItem.Text;
            string cboJob = this.cboJob.SelectedItem.Text;
            string Leadtecnjob = txtLeadtecnjob.Text;
            string Planttype = txtPlanttype.Text;
            string EnterDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            string birthdate = this.TimeConvert(dtfLeadold);
            string Leadold = (System.DateTime.Now.Year - dtfLeadold.SelectedDate.Year).ToString();
            string Weavebed = NumZero(txtWeavebed.Text);
            string Deploybed = NumZero(txtDeploybed.Text);
            string Persum = NumZero(txtPersum.Text);
            string Doctor = NumZero(txtDoctor.Text);
            string Master = NumZero(txtMaster.Text);
            string Gradu = NumZero(txtGradu.Text);
            string Junior = NumZero(txtJunior.Text);
            string Tech = NumZero(txtTech.Text);
            string Goabroad = NumZero(txtGoabroad.Text);
            string Expertfoll = NumZero(txtExpertfoll.Text);
            string Senddoctor = NumZero(txtSenddoctor.Text);
            string Sendmaster = NumZero(txtSendmaster.Text);
            string Fetchdoctor = NumZero(txtFetchdoctor.Text);
            string Fetchmaster = NumZero(txtFetchmaster.Text);
            string Fostersum = NumZero(txtFostersum.Text);
            string Newtech = NumZero(txtNewtech.Text);
            string Fetch = NumZero(txtFetch.Text);
            string Techindepen = NumZero(txtTechindepen.Text);
            string Techabsorb = NumZero(txtTechabsorb.Text);
            string Techelse = NumZero(txtTechelse.Text);

            dal.DeleteSpecCenterList(statMonth, this.cboSpceCenterInfo.SelectedItem.Value);
            dal.InsertSpecCenterList(statMonth, unitCode, unitName, Unittype, this.cboSpceCenterInfo.SelectedItem.Value, this.cboSpceCenterInfo.SelectedItem.Text, Spectype, Leadname,
                                        Leadold, Leadedu, cboJob, Leadtecnjob, Weavebed, Deploybed, Persum, Doctor, Master,
                                        Gradu, Junior, Tech, Goabroad, Expertfoll, Senddoctor, Sendmaster, Fetchdoctor,
                                        Fetchmaster, Planttype, Fostersum, Newtech, Fetch, Techindepen, Techabsorb, Techelse, EnterDate, birthdate);
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = "信息提示",
                Message = "保存成功",
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
            this.btnDept.Disabled = false;
            return;
        }



        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ViewCenterDept(object sender, AjaxEventArgs e)
        {
            DataTable l_dt = dal.ViewSpecCenterListInfo(this.cboSpceCenterInfo.SelectedItem.Value, this.NumYear.Text).Tables[0];
            if(l_dt.Rows.Count > 0) {
                this.ViewDept.Show();
                Store.DataSource = dal.ViewCenterDept(this.NumYear.Text, l_dt.Rows[0]["ID"].ToString()).Tables[0];
                Store.DataBind();
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataTable l_dt = dal.ViewSpecCenterListInfo(this.cboSpceCenterInfo.SelectedItem.Value, this.NumYear.Text).Tables[0];
            if (l_dt.Rows.Count > 0)
            {
                this.ViewDept.Show();
                Store.DataSource = dal.ViewCenterDept(this.NumYear.Text, l_dt.Rows[0]["ID"].ToString()).Tables[0];
                Store.DataBind();
            }
        }



        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddDept(object sender, AjaxEventArgs e)
        {
            DataTable l_dt = dal.ViewSpecCenterListInfo(this.cboSpceCenterInfo.SelectedItem.Value, this.NumYear.Text).Tables[0];
            if (l_dt.Rows.Count > 0)
            {
                if (this.DeptCodeCombo.SelectedItem.Value != "")
                {
                    dal.InsertCenterDept(this.DeptCodeCombo.SelectedItem.Value, this.DeptCodeCombo.SelectedItem.Text, this.NumYear.Text, l_dt.Rows[0]["ID"].ToString());
                    Store.DataSource = dal.ViewCenterDept(this.NumYear.Text, l_dt.Rows[0]["ID"].ToString()).Tables[0];
                    Store.DataBind();
                }
                else 
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "请选择科室",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    this.btnDept.Disabled = false;
                    return;
                
                }
            }
        }


        [AjaxMethod]
        public void DeptAjaxOper(string id)
        {
            dal.DelCenterDept(id);
        }

         

        /// <summary>
        /// 初始化COMBOX控件
        /// </summary>
        /// <param name="ID">数据库ID名称</param>
        /// <param name="text">数据库NAME名称</param>
        /// <param name="dtSource">数据源</param>
        /// <param name="cbo">COMBOX控件</param>
        private void SetCboData(string ID, string text, DataTable dtSource, ComboBox cbo)
        {
            if (dtSource.Rows.Count < 1)
            {
                return;
            }
            for (int idx = 0; idx < dtSource.Rows.Count; idx++)
            {
                cbo.Items.Add(new Goldnet.Ext.Web.ListItem(dtSource.Rows[idx]["NAME"].ToString(), dtSource.Rows[idx]["ID"].ToString()));
            }
        }


        private void setContrls(DataTable SpecCenterInfo)
        {
            if (SpecCenterInfo.Rows.Count > 0)
            {
                txtUnitcode.Text = SpecCenterInfo.Rows[0]["UNIT_CODE"].ToString(); ;   // 单位代码
                txtVicedirector.Text = SpecCenterInfo.Rows[0]["Unit_name"].ToString();
                txtUnittype.Text = SpecCenterInfo.Rows[0]["Unit_type"].ToString();
                cboSpectype.SetValue(SpecCenterInfo.Rows[0]["SPEC_TYPE"].ToString());   // 专科中心类别
                cboJob.SetValue(SpecCenterInfo.Rows[0]["SPEC_LEAD_TITLE"].ToString());   // 带头人职称
                cboLeadedu.SetValue(SpecCenterInfo.Rows[0]["SPEC_LEAD_EDU"].ToString());   // 带头人学历
                txtSpeccode.Text = SpecCenterInfo.Rows[0]["Spec_Code"].ToString();
                txtSpecname.Text = SpecCenterInfo.Rows[0]["Spec_Name"].ToString();
                txtLeadname.Text = SpecCenterInfo.Rows[0]["Spec_Lead_Name"].ToString();
                txtLeadtecnjob.Text = SpecCenterInfo.Rows[0]["Spec_Lead_Duty"].ToString();
                txtPlanttype.Text = SpecCenterInfo.Rows[0]["Bring_Type"].ToString();
                this.dtfLeadold.SetValue(SpecCenterInfo.Rows[0]["SPEC_LEAD_BIRTHDATE"].ToString());
                txtWeavebed.Text = SpecCenterInfo.Rows[0]["Appro_bed"].ToString();
                txtDeploybed.Text = SpecCenterInfo.Rows[0]["Open_bed"].ToString();
                txtPersum.Text = SpecCenterInfo.Rows[0]["Pers_Num"].ToString();
                txtDoctor.Text = SpecCenterInfo.Rows[0]["Dr_Num"].ToString();
                txtMaster.Text = SpecCenterInfo.Rows[0]["Master_Num"].ToString();
                txtGradu.Text = SpecCenterInfo.Rows[0]["Under_Num"].ToString();
                txtJunior.Text = SpecCenterInfo.Rows[0]["Junior_Num"].ToString();
                txtTech.Text = SpecCenterInfo.Rows[0]["Tech_Num"].ToString();
                txtGoabroad.Text = SpecCenterInfo.Rows[0]["GoAbrBring"].ToString();
                txtExpertfoll.Text = SpecCenterInfo.Rows[0]["ExpertPren"].ToString();
                txtSenddoctor.Text = SpecCenterInfo.Rows[0]["ShowDr"].ToString();
                txtSendmaster.Text = SpecCenterInfo.Rows[0]["ShowMaster"].ToString();
                txtFetchdoctor.Text = SpecCenterInfo.Rows[0]["FetchDr"].ToString();
                txtFetchmaster.Text = SpecCenterInfo.Rows[0]["FetchMaster"].ToString();
                txtFostersum.Text = SpecCenterInfo.Rows[0]["Bring_Num"].ToString();
                txtNewtech.Text = SpecCenterInfo.Rows[0]["NewItemNum"].ToString();
                txtFetch.Text = SpecCenterInfo.Rows[0]["FetchItemNum"].ToString();
                txtTechindepen.Text = SpecCenterInfo.Rows[0]["InaugItemNum"].ToString();
                txtTechabsorb.Text = SpecCenterInfo.Rows[0]["AbsItemNum"].ToString();
                txtTechelse.Text = SpecCenterInfo.Rows[0]["OtherItemNum"].ToString();
                txtSpeccode.Text = SpecCenterInfo.Rows[0]["SPEC_CODE"].ToString();
                txtSpecname.Text = SpecCenterInfo.Rows[0]["SPEC_NAME"].ToString();
            }
            else 
            {
                InitContrl();
            }
        }

        private void InitContrl() 
        {
            DataTable l_dt = dal.ViewUnitDetail("").Tables[0];
            txtUnitcode.Text = l_dt.Rows[0]["UNIT_CODE"].ToString();   // 单位代码
            txtSpecname.Text = this.cboSpceCenterInfo.SelectedItem.Text;   // 专科中心名称
            cboLeadedu.SetValue("");   // 带头人学历
            txtDeploybed.Text = "";   // 学科展开床
            txtGradu.Text = "";   // 本科
            txtExpertfoll.Text = "";   // 专家带徒
            txtFetchmaster.Text = "";   // 引进硕士
            txtFetch.Text = "";   // 其中：引进
            txtVicedirector.Text = l_dt.Rows[0]["UNIT_NAME"].ToString();   // 单位名称
            cboSpectype.SetValue("");   // 专科中心类别
            cboJob.SetValue("");   // 带头人职称
            txtPersum.Text = "";   // 主系列人员数
            txtJunior.Text = "";   // 大专
            txtSenddoctor.Text = "";   // 送学博士
            txtPlanttype.Text = "";   // 培养点类别
            txtTechindepen.Text = "";   // 自主创新
            txtUnittype.Text = l_dt.Rows[0]["CHARACTER"].ToString();   // 单位性质
            txtLeadname.Text = "";   // 带头人姓名
            txtLeadtecnjob.Text = "";   // 带头人学术任职
            txtDoctor.Text = "";   // 其中：博士
            txtTech.Text = "";   // 中专
            txtSendmaster.Text = "";   // 送学硕士
            txtFostersum.Text = "";   // 培训人数
            txtTechabsorb.Text = "";   // 消化吸收
            txtSpeccode.Text = this.cboSpceCenterInfo.SelectedItem.Value;   // 专科中心代码
            dtfLeadold.SetValue("");   // 带头人出生日期
            txtWeavebed.Text = "";   // 学科编制床
            txtMaster.Text = "";   // 硕士
            txtGoabroad.Text = "";   // 出国培养
            txtFetchdoctor.Text = "";   // 引进博士
            txtNewtech.Text = "";   // 新技术新项目
            txtTechelse.Text = "";   // 其他
        }

        /// <summary>
        /// 数字为空返回0
        /// </summary>
        /// <param name="numNull"></param>
        /// <returns></returns>
        private string NumZero(string numNull) 
        {
            if (numNull == "") 
            {
                return "0";
            
            }
            return numNull;
        }


        /// <summary>
        /// 时间控件返回值设定
        /// </summary>
        /// <param name="timeConctrl">时间控件</param>
        /// <returns></returns>
        private string TimeConvert(DateField timeConctrl)
        {
            if (timeConctrl.SelectedValue == null)
            {
                return "";
            }
            else
            {
                return timeConctrl.SelectedDate.ToString("yyyy-MM-dd");
            }
        }
    }
}
