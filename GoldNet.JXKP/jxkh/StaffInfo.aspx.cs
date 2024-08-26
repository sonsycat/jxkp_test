using System;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.jxkh
{
    public partial class StaffInfo : System.Web.UI.Page
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {

                string staff_id = Request.QueryString["staff_id"].ToString();
                BindList(staff_id);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="staff_id"></param>
        protected void BindList(string staff_id)
        {
            Assess dal = new Assess();
            DataTable dt = dal.GetStaffInfo(staff_id).Tables[0];
            this.txtDeptInput.Value = dt.Rows[0]["DEPT_NAME"].ToString();//科室名称
            this.txtStaffInput.Value = dt.Rows[0]["NAME"].ToString();//人员姓名
            this.dtfBirthday.Value = dt.Rows[0]["BIRTHDAY"].ToString();//出生日期
            this.cboSex.SelectedItem.Value = dt.Rows[0]["SEX"].ToString();//性别
            this.cboDuty.Value = dt.Rows[0]["DUTY"].ToString();//职务
            this.cboRootspecsort.Value = dt.Rows[0]["ROOTSPECSORT"].ToString();//从事专业
            this.cboSanispecsort.Value = dt.Rows[0]["SANTSPECSORT"].ToString();//卫生专业分类
            this.cboBraid.SelectedItem.Value = dt.Rows[0]["ISBRAID"].ToString();//实虚编
            this.cboRank.Value = dt.Rows[0]["RANK"].ToString();//级别
            this.dtfDutydate.Value = dt.Rows[0]["DUTYDATE"].ToString();//行政职务事件
            this.cboPeople.Value = dt.Rows[0]["NATIONALS"].ToString();//民族
            this.cboIsOnGuard.Value = dt.Rows[0]["ISONGUARD"].ToString();//是否在岗
            this.cboStation.Value = dt.Rows[0]["STATION_NAME"].ToString();//岗位名称
            this.cboPerssort.Value = dt.Rows[0]["STAFFSORT"].ToString();//人员类别
            this.cboSpeciality.Value = dt.Rows[0]["STUDY_SPECSORT"].ToString();//所学专业
            this.cboJobDuty.Value = dt.Rows[0]["JOB"].ToString();//技术职务
            this.cboTiptopLearnStuffer.Value = dt.Rows[0]["TOPEDUCATE"].ToString();//学历
            this.cboTitle.Value = dt.Rows[0]["TITLE"].ToString();//职称
            this.cboTitleList.Value = dt.Rows[0]["TITLE_LIST"].ToString();//职称序列
            this.txtRetainTerm.Value = dt.Rows[0]["RETAINTERM"].ToString();//受聘期限
            this.cboDegree.Value = dt.Rows[0]["EDU1"].ToString();//学位
            this.cboCadreType.Value = dt.Rows[0]["CADRES_CATEGORIES"].ToString();//干部类别
            this.cboDeptType.Value = dt.Rows[0]["DEPT_TYPE"].ToString();//所在科室累
            this.cboBackboneCircs.Value = dt.Rows[0]["EXPERT"].ToString();//专家骨干情况
            this.cmbMaritalStatus.Value = dt.Rows[0]["MARITAL_STATUS"].ToString();//婚姻状况
            this.dtfJobDate.Value = dt.Rows[0]["JOBDATE"].ToString();//技术职务时间
            this.dtfStudyOverdate.Value = dt.Rows[0]["STUDY_OVER_DATE"].ToString();//毕业时间
            this.dtfWorkDate.Value = dt.Rows[0]["WORKDATE"].ToString();//工作时间
            this.txtGraduateAcademy.Value = dt.Rows[0]["GUARDCAUS"].ToString();//毕业院校
            this.dtfTitleAssess.Value = dt.Rows[0]["TITLE_DATE"].ToString();//资格评定时间
            this.dtfInHospitalDate.Value = dt.Rows[0]["INHOSPITALDATE"].ToString();//来院时间
            this.cboTechnicTitle.Value = dt.Rows[0]["JOB_TITLE"].ToString();//技术资格
            this.cboGovAllowance.Value = dt.Rows[0]["GOVERNMENT_ALLOWANCE"].ToString();//政府津贴
            this.dtfGradetitleDate.Value = dt.Rows[0]["GUARDTIME"].ToString();//所得学历时间
            this.txtCertificateNo.Value = dt.Rows[0]["CERTIFICATE_NO"].ToString();//证件号码
            this.txtHomeplace.Value = dt.Rows[0]["HOMEPLACE"].ToString();//出生地
            this.txtMemo.Value = dt.Rows[0]["MEMO"].ToString();//备注
            this.cboIfarmy.SelectedItem.Value = dt.Rows[0]["IF_ARMY"].ToString();//是否军人
            this.dtfBeEnrolledInDate.Value = dt.Rows[0]["BEENROLLEDINDATE"].ToString();//入伍时间
            this.cboCivilServiceClass.Value = dt.Rows[0]["CIVILSERVICECLASS"].ToString();//文职级
            this.dtfCivilServiceClassDate.Value = dt.Rows[0]["CIVILSERVICECLASSDATE"].ToString();//文职时间
            this.cboTechnicClass.Value = dt.Rows[0]["TECHINCCLASS"].ToString();//技术级
            this.dtfTechnicClassDate.Value = dt.Rows[0]["TECHNICCLASSDATE"].ToString();//技术级时间
            this.txtMedicardmark.Value = dt.Rows[0]["MEDICARDMARK"].ToString();//医疗卡账号
            this.txtMediCard.Value = dt.Rows[0]["MEDICARDMARK"].ToString();//医疗卡号
            if (dt.Rows[0]["IMG_ID"].ToString() != "")
            {
                this.imgStaff.ImageUrl = "/resources/UploadPicfile/" + dt.Rows[0]["IMG_ID"].ToString();//照片url地址
            }
        }
    }
}
