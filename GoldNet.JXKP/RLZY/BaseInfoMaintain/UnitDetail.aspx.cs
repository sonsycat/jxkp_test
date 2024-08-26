using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class UnitDetail : System.Web.UI.Page
    {
        BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
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
                setContrl();
                DataTable l_dt = dal.ViewUnitDetail("").Tables[0];
                SetInfo(l_dt);
            }
        }


        /// <summary>
        /// 初始化时间空件
        /// </summary>
        private void setContrl()
        {
            //时间
            this.NumYear.SetValue(DateTime.Now.Year);

            for (int i = 1; i <= 12; i++)
            {
                this.Comb_StartMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_StartMonth.SelectedIndex = 0;


            DictMainTainDal dictdal = new DictMainTainDal();
            DataTable cboDt = null;

            //单位性质
            cboDt = dictdal.getDictInfo("UNIT_TYPE_DICT", "UNIT_TYPE_NO", "UNIT_TYPE_NAME", false, "").Tables[0];
            SetCboData("UNIT_TYPE_NO", "UNIT_TYPE_NAME", cboDt, cboCharacter);
            cboCharacter.SelectedIndex = 0;

            //单位等级
            cboDt = dictdal.getDictInfo("UNIT_LEVEL_DICT", "UNIT_LEVEL_NO", "UNIT_LEVEL_NAME", false, "").Tables[0];
            SetCboData("UNIT_LEVEL_NO", "UNIT_LEVEL_NAME", cboDt, cboUnitLevel);
            cboUnitLevel.SelectedIndex = 0;

        }


        protected void QueryUnit(object sender, AjaxEventArgs e)
        {
            if (this.NumYear.Value.ToString() == "")
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "请输入年度！",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            string StYear = NumYear.Value.ToString() + (this.Comb_StartMonth.SelectedItem.Value.Length == 1?"0"+this.Comb_StartMonth.SelectedItem.Value:this.Comb_StartMonth.SelectedItem.Value);
            DataTable l_dt = dal.ViewUnitDetail(StYear).Tables[0];
            SetInfo(l_dt);
        }



        /// <summary>
        /// 初始化COMBOX控件
        /// </summary>
        /// <param name="ID">数据库ID名称</param>
        /// <param name="text">数据库NAME名称</param>
        /// <param name="dtSource">数据源</param>
        /// <param name="cbo">COMBOX控件</param>
        private  void SetCboData(string ID, string text, DataTable dtSource, ComboBox cbo)
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


        protected void SaveInfo(object sender, AjaxEventArgs e)
        {
            if (this.NumYear.Value.ToString() == "")
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "请输入年度！",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            string StYear = NumYear.Value.ToString() + (this.Comb_StartMonth.SelectedItem.Value.Length == 1?"0"+this.Comb_StartMonth.SelectedItem.Value:this.Comb_StartMonth.SelectedItem.Value);

            //插入数据
            dal.InsertUnitDetail(txtUnitCode.Value.ToString(), txtUintName.Value.ToString(), StYear, txtDasNameDean.Value.ToString(), txtDasNameCommissar.Value.ToString(), txtAuthorizedPepleTotal.Text,txtAuthorizedCadreHighe.Text,
                                txtFactRetire.Text, txtFactCadreHigher.Text, txtFactRelation.Text, txtMailingAddress.Value.ToString(), txtZipCode.Value.ToString(), txtArmPhone.Value.ToString(), txtPlacePhone.Value.ToString(),
                                cboCharacter.SelectedItem.Text, cboCharacter.SelectedItem.Value, cboUnitLevel.SelectedItem.Text, cboUnitLevel.SelectedItem.Value, txtSubUnit.Value.ToString(), txtCondtion.Value.ToString(),
                                txtTotalArea.Text, txtOperationArea.Text, txtOfficeArea.Text, txtAssistantArea.Text, txtExistArea.Text, txtAmbulance.Text, txtLibrary.Text, txtBookNum.Text,
                                txtForeignBookNum.Text, txtMagazineSortNum.Text, txtForeignMagazine.Text, txtSortNum.Text, txtComputerNum.Text, txtComputerOldNum.Text, txtLocation.Value.ToString(),
                                txtMedicalBility.Value.ToString(), txtCareBilty.Value.ToString());
        }



        /// <summary>
        /// 初始化控件值
        /// </summary>
        /// <param name="table"></param>
        private void SetInfo(DataTable table) 
        {
            if (table.Rows.Count > 0) 
            {
                int num = 0;
                string Time = table.Rows[num]["DAS_ST_YEAR_MONTH"].ToString().Substring(0, 4) + "-" + table.Rows[num]["DAS_ST_YEAR_MONTH"].ToString().Substring(4) + "-01";
                NumYear.Text=Convert.ToDateTime(Time).Year.ToString();
                this.Comb_StartMonth.SelectedIndex = (Convert.ToDateTime(Time)).Month - 1;
                txtUnitCode.Value = table.Rows[num]["UNIT_CODE"].ToString();
                txtUintName.Value = table.Rows[num]["UNIT_NAME"].ToString();
                txtDasNameDean.Value = table.Rows[num]["DAS_NAME_DEAN"].ToString();
                txtDasNameCommissar.Value = table.Rows[num]["DAS_NAME_COMMISSAR"].ToString();
                txtAuthorizedPepleTotal.Text = table.Rows[num]["AUTHORIZED_POPLE_TOTAL"].ToString();
                txtAuthorizedCadreHighe.Text = table.Rows[num]["AUTHORIZED_CADRE_HIGHER"].ToString();
                txtFactRetire.Text = table.Rows[num]["FACT_RETIRE"].ToString();
                txtFactCadreHigher.Text = table.Rows[num]["FACT_CADRE_HIGHER"].ToString();
                txtFactRelation.Text = table.Rows[num]["FACT_RELATION"].ToString();
                txtMailingAddress.Value = table.Rows[num]["MAILING_ADDRESS"].ToString();
                txtZipCode.Value = table.Rows[num]["ZIP_CODE"].ToString();
                txtArmPhone.Value = table.Rows[num]["ARM_PHONE"].ToString();
                txtPlacePhone.Value = table.Rows[num]["PLACE_PHONE"].ToString();
                txtCharacterCode.Text = table.Rows[num]["CHARACTER_CODE"].ToString();
                cboCharacter.SetValue(table.Rows[num]["CHARACTER_CODE"].ToString());
                cboUnitLevel.SetValue(table.Rows[num]["UNIT_LEVEL_CODE"].ToString());
                txtUnitLevelCode.Text = table.Rows[num]["UNIT_LEVEL_CODE"].ToString();
                txtSubUnit.Value = table.Rows[num]["SUB_UNIT"].ToString();
                txtCondtion.Value = table.Rows[num]["CONDTION"].ToString();
                txtTotalArea.Text = table.Rows[num]["TOTAL_AREA"].ToString();
                txtOperationArea.Text = table.Rows[num]["OPERATION_AREA"].ToString();
                txtOfficeArea.Text = table.Rows[num]["OFFICE_AREA"].ToString();
                txtAssistantArea.Text = table.Rows[num]["ASSISTANT_AREA"].ToString();
                txtExistArea.Text = table.Rows[num]["EXIST_AREA"].ToString();
                txtAmbulance.Text = table.Rows[num]["AMBULANCE"].ToString();
                txtLibrary.Text = table.Rows[num]["LIBRARY"].ToString();
                txtBookNum.Text = table.Rows[num]["BOOK_NUM"].ToString();
                txtForeignBookNum.Text = table.Rows[num]["FOREIGN_BOOK_NUM"].ToString();
                txtMagazineSortNum.Text = table.Rows[num]["MAGAZINE_SORT_NUM"].ToString();
                txtForeignMagazine.Text = table.Rows[num]["FOREIGN_MAGAZINE"].ToString();
                txtSortNum.Text = table.Rows[num]["SORT_NUM"].ToString();
                txtComputerNum.Text = table.Rows[num]["COMPUTER_NUM"].ToString();
                txtComputerOldNum.Text = table.Rows[num]["COMPUTER_OLD_NUM"].ToString();
                txtLocation.Value = table.Rows[num]["LOCATION"].ToString();
                txtMedicalBility.Value = table.Rows[num]["MEDICAL_BILITY"].ToString();
                txtCareBilty.Value = table.Rows[num]["CARE_BILITY"].ToString();
            }     
        }
    }
}
