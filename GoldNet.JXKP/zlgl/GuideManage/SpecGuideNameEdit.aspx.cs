using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GoldNet.JXKP.PowerManager;
using GoldNet.Comm;
using GoldNet.JXKP.Templet.BLL;
using GoldNet.JXKP.BLL.Organise;
using Goldnet.Ext.Web;
using GoldNet.Model;


namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class SpecGuideNameEdit : PageBase
    {
        public int intID;
        public string straction;
        public string strsigntype;
        protected void Page_Load(object sender, EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            intID = Convert.ToInt32(Request.QueryString["id"].ToString());
            straction = Request["straction"].ToString();
            strsigntype = Request["strsigntype"].ToString();
            if (!Ext.IsAjaxRequest)
            {
                Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
                DataRow[] ddlguidetyperow = dal.GetGuideTypeBySign(strsigntype).Tables[0].Select();
                foreach (DataRow rw in ddlguidetyperow)
                {
                    this.DDLGuideType.Items.Add(new Goldnet.Ext.Web.ListItem(rw["GuideType"].ToString(), rw["ID"].ToString()));

                }
                //this.DDLGuideType.SelectedIndex = 0;

                SetDict();
                DataFiller(this.Combo_SelectDept.SelectedItem.Value);
            }
        }
        /// <summary>
        /// 下拉框设置
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataRow[] depttyperow = dal.GetDeptType().Tables[0].Select();
            foreach (DataRow rw in depttyperow)
            {
                this.Combo_SelectDept.Items.Add(new Goldnet.Ext.Web.ListItem(rw["ATTRIBUE"].ToString(), rw["ID"].ToString()));
            }
            //
            DataRow[] ddltemplet = TempletBO.GetAllTempletList().Select();
            foreach (DataRow rw in ddltemplet)
            {
                this.DDLTemplet1.Items.Add(new Goldnet.Ext.Web.ListItem(rw["NAME"].ToString(), rw["ID"].ToString()));
            }
        
        }
        private void DataFiller(string id)
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            Goldnet.Dal.TempList temp = new Goldnet.Dal.TempList();
            TBGuideNum.Text = temp.GetGuideTypeBySign(strsigntype).Tables[0].Rows[0][3].ToString();          //填充专业指标分数

            if (straction == "edit")
            {
                //bool TempletIsExist = false;                      //初始化保存的模板ID号不存在 
                //如果修改，则显示要修改的记录
                DataSet ds = dal.SpecialtyGuide_Edit_View(intID);
                TBGuideName.Text = ds.Tables[0].Rows[0]["SpecGuideName"].ToString();
                TBManaDept.Text = ds.Tables[0].Rows[0]["ManaDept"].ToString();
                TBGuideNum.Text = ds.Tables[0].Rows[0]["GuideNum"].ToString();
                Goldnet.Dal.SYS_DEPT_DICT dept = new Goldnet.Dal.SYS_DEPT_DICT();
                string deptcodes=ds.Tables[0].Rows[0]["CheckDept"].ToString();
                string[] deptlist = deptcodes.Split(',');
                string deptfilter = "";
                for (int i = 0; i < deptlist.Length; i++)
                {
                    if (!deptlist[i].ToString().Equals(string.Empty))
                        deptfilter = deptfilter + "'"+deptlist[i].ToString() + "',";
                }
                if (!deptfilter.Equals(""))
                {
                    deptfilter = deptfilter.Substring(0, deptfilter.Length - 1);
                }
                
                if (deptcodes.Length > 0)
                {
                    this.Store2.DataSource = dept.GetDeptByDeptCodes(deptfilter);
                    this.Store2.DataBind();
                }
                this.DDLGuideType.SelectedItem.Value = ds.Tables[0].Rows[0]["SpecGuideTypeID"].ToString();
                this.DDLTemplet1.SelectedItem.Value = ds.Tables[0].Rows[0]["TempletID"].ToString();
                //DDLTemplet_Selected("edit");
                TempletBO templet = new TempletBO(int.Parse(ds.Tables[0].Rows[0]["TempletID"].ToString()));
                //时间
                FieldCollectionTable ddldatecolrow = templet.GetDateFields();
                for (int i = 0; i < ddldatecolrow.Rows.Count; i++)
                {
                    this.DDLDateCol.Items.Add(new Goldnet.Ext.Web.ListItem(ddldatecolrow.Rows[i]["FieldName"].ToString(), ddldatecolrow.Rows[i]["ID"].ToString()));

                }
                this.DDLDateCol.SelectedIndex = 0;
                //部门
                FieldCollectionTable ddltargetcolrow = templet.GetDeptFields();
                for (int i = 0; i < ddltargetcolrow.Rows.Count; i++)
                {
                    this.DDLTargetCol.Items.Add(new Goldnet.Ext.Web.ListItem(ddltargetcolrow.Rows[i]["FieldName"].ToString(), ddltargetcolrow.Rows[i]["ID"].ToString()));

                }
                this.DDLTargetCol.SelectedIndex = 0;
                //数值
                FieldCollectionTable ddlguidenamecolvaluerow = templet.GetNumFields();
                for (int i = 0; i < ddlguidenamecolvaluerow.Rows.Count; i++)
                {
                    this.DDLGuideNameColValue.Items.Add(new Goldnet.Ext.Web.ListItem(ddlguidenamecolvaluerow.Rows[i]["FieldName"].ToString(), ddlguidenamecolvaluerow.Rows[i]["ID"].ToString()));

                }
                this.DDLGuideNameColValue.SelectedIndex = 0;
                //内容
                FieldCollectionTable ddlguidenamecolrow = templet.GetGuidFields();
                for (int i = 0; i < ddlguidenamecolrow.Rows.Count; i++)
                {
                    this.DDLGuideNameCol.Items.Add(new Goldnet.Ext.Web.ListItem(ddlguidenamecolrow.Rows[i]["FieldName"].ToString(), ddlguidenamecolrow.Rows[i]["ID"].ToString()));

                }
                this.DDLGuideNameCol.SelectedIndex = 0;

            }
           
        }


        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            List<PageModels.deptselected> deptlist = e.Object<PageModels.deptselected>();
            bool saveflags = true;
            if (TBGuideName.Text.Length == 0)
            {
                saveflags = false;
                this.ShowMessage("系统提示","指标名称不能为空！");
            }
            else if (DDLGuideType.SelectedItem.Value == "0") saveflags = false;
            else if (TBGuideNum.Text.Length == 0) saveflags = false;
            else if (!this.IsNum(TBGuideNum.Text.Trim())) saveflags = false;

            else if (DDLTemplet1.SelectedItem.Value == "")
            {
                saveflags = false;
                this.ShowMessage("系统提示", "模版不能为空！");
            }
            else if (DDLGuideNameCol.SelectedItem.Value == "")
            {
                saveflags = false;
                this.ShowMessage("系统提示", "指标列不能为空！");
            }
            else if (DDLDateCol.SelectedItem.Value == "")
            {
                saveflags = false;
                this.ShowMessage("系统提示", "时间列不能为空！");
            }
            else if (DDLTargetCol.SelectedItem.Value == "")
            {
                saveflags = false;
                this.ShowMessage("系统提示", "部门列不能为空！");
            }
            
            else if (DDLGuideNameColValue.SelectedItem.Value == "")
            {
                saveflags = false;
                this.ShowMessage("系统提示", "数值列不能为空！");
            }
            
             if(saveflags==true)
            {
                try
                {
                    string strCommGuideName = CleanString.InputText(TBGuideName.Text.ToString(), 25);
                    int intCommGuideTypeID = Convert.ToInt32(DDLGuideType.SelectedItem.Value.ToString());
                    string strManaDept = CleanString.InputText(TBManaDept.Text.ToString(), 25);
                    double douGuideNum = Convert.ToDouble(TBGuideNum.Text.ToString());
                    StringBuilder strCheckDept = new StringBuilder();
                    foreach (GoldNet.Model.PageModels.deptselected dept in deptlist)
                    {
                        strCheckDept.Append(dept.DEPT_CODE + ",");
                    }

                    int intTemplet = Convert.ToInt32(DDLTemplet1.SelectedItem.Value.ToString());
                    int intDateCol = Convert.ToInt32(DDLDateCol.SelectedItem.Value.ToString());
                    int intTargetCol = Convert.ToInt32(DDLTargetCol.SelectedItem.Value.ToString());
                    int intGuideNameCol = Convert.ToInt32(DDLGuideNameCol.SelectedItem.Value.ToString());

                    int intGuideNameColValue = Convert.ToInt32(DDLGuideNameColValue.SelectedItem.Value.ToString());
                    //int intArithmetic = Convert.ToInt32(DDLArithmetic.SelectedItem.Value.ToString());


                    if (straction == "add")   //保存新增数据
                    {

                        bool SpecGuideNameAdd = dal.SpecialtyGuide_Add(strCommGuideName, intCommGuideTypeID, strManaDept, douGuideNum, strCheckDept.ToString(), intTemplet, intDateCol, intTargetCol, intGuideNameCol, intGuideNameColValue, 1);
                        if (SpecGuideNameAdd == true)
                        {
                            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                            scManager.AddScript("parent.RefreshData();");
                            scManager.AddScript("parent.guideinfo.hide();");
                        }

                    }
                    else
                    {
                        if (straction == "edit")    //保存修改数据
                        {

                            bool SpecGuideNameEdit = dal.SpecialtyGuide_Edit(strCommGuideName, intCommGuideTypeID, strManaDept, douGuideNum, strCheckDept.ToString(), intTemplet, intDateCol, intTargetCol, intGuideNameCol, intGuideNameColValue, 1, intID);
                            if (SpecGuideNameEdit == true)
                            {
                                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                                scManager.AddScript("parent.RefreshData();");
                                scManager.AddScript("parent.guideinfo.hide();");
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "SubmitData");

                }
            }

        }

      
        protected void DDLTemplet_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDLTemplet_Selected("change");
        }

        private void DDLTemplet_Selected(string stations)
        {
            if (DDLTemplet1.SelectedItem.Value != "")
            {
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript(DDLDateCol.ClientID + ".store.removeAll();");
                scManager.AddScript(DDLDateCol.ClientID + ".clearValue();");
                scManager.AddScript(DDLTargetCol.ClientID + ".store.removeAll();");
                scManager.AddScript(DDLTargetCol.ClientID + ".clearValue();");
                scManager.AddScript(DDLGuideNameColValue.ClientID + ".store.removeAll();");
                scManager.AddScript(DDLGuideNameColValue.ClientID + ".clearValue();");
                scManager.AddScript(DDLGuideNameCol.ClientID + ".store.removeAll();");
                scManager.AddScript(DDLGuideNameCol.ClientID + ".clearValue();");
                //选择的模板
                int templetID = Convert.ToInt16(DDLTemplet1.SelectedItem.Value.ToString());
                //构造模板对象
                TempletBO templet = new TempletBO(templetID);
                //时间
                FieldCollectionTable ddldatecolrow = templet.GetDateFields();
                for (int i = 0; i < ddldatecolrow.Rows.Count;i++ )
                {
                    this.DDLDateCol.AddItem(ddldatecolrow.Rows[i]["FieldName"].ToString(), ddldatecolrow.Rows[i]["ID"].ToString());

                }
                
                this.DDLDateCol.SelectedIndex = 0;
                //部门
                FieldCollectionTable ddltargetcolrow = templet.GetDeptFields();
                for (int i = 0; i < ddltargetcolrow.Rows.Count; i++)
                {
                    this.DDLTargetCol.AddItem(ddltargetcolrow.Rows[i]["FieldName"].ToString(), ddltargetcolrow.Rows[i]["ID"].ToString());

                }
                this.DDLTargetCol.SelectedIndex = 0;
                //数值
                FieldCollectionTable ddlguidenamecolvaluerow = templet.GetNumFields();
                for (int i = 0; i < ddlguidenamecolvaluerow.Rows.Count; i++)
                {
                    this.DDLGuideNameColValue.AddItem(ddlguidenamecolvaluerow.Rows[i]["FieldName"].ToString(), ddlguidenamecolvaluerow.Rows[i]["ID"].ToString());
                    
                }
                this.DDLGuideNameColValue.SelectedIndex = 0;
                //内容
                FieldCollectionTable ddlguidenamecolrow = templet.GetGuidFields();
                for (int i = 0; i < ddlguidenamecolrow.Rows.Count; i++)
                {
                    this.DDLGuideNameCol.AddItem(ddlguidenamecolrow.Rows[i]["FieldName"].ToString(), ddlguidenamecolrow.Rows[i]["ID"].ToString());
                   
                }
                this.DDLGuideNameCol.SelectedIndex = 0;

            }
            else
            {
                DDLDateCol.Items.Clear();
                DDLTargetCol.Items.Clear();
                DDLGuideNameColValue.Items.Clear();
            }
        }

       
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.guideinfo.hide();");
        }
        /// <summary>
        /// 选择科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedDept(object sender, EventArgs e)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            this.Store1.DataSource = dal.GetSpeDeptBytype(this.Combo_SelectDept.SelectedItem.Value, this.DDLTemplet1.SelectedItem.Value);
            this.Store1.DataBind();
        }
    }
}
