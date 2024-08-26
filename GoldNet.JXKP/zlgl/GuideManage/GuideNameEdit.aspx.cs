using System;
using System.Collections;
using System.ComponentModel;
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
using Goldnet.Ext.Web;
namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class GuideNameEdit : PageBase
    {
        public int intID;
        public string straction;
        public string strsigntype;

        protected void Page_Load(object sender, EventArgs e)
        {
            intID = Convert.ToInt32(Request.QueryString["id"].ToString());   //要修改的指标的ID号
            straction = Request["straction"].ToString();                     //添加或者修改标志
            strsigntype = Request["strsigntype"].ToString();                 //指标类别标志，0为公共，1为专业
            if (!Ext.IsAjaxRequest)
            {
                DataFiller();
            }
        }
        private void DataFiller()
        {
            //绑定指标类别列
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            DataTable guidetypetable = dal.GetGuideTypeBySign(strsigntype).Tables[0];
           
            for (int i = 0; i < guidetypetable.Rows.Count; i++)
            {
                this.DDLGuideType.Items.Add(new Goldnet.Ext.Web.ListItem(guidetypetable.Rows[i]["GuideType"].ToString(), guidetypetable.Rows[i]["ID"].ToString()));
            }

            //绑定模板列
            DataTable templettable = TempletBO.GetAllTempletList();
          
            for (int i = 0; i < templettable.Rows.Count; i++)
            {
                this.DDLTemplet.Items.Add(new Goldnet.Ext.Web.ListItem(templettable.Rows[i]["NAME"].ToString(), templettable.Rows[i]["ID"].ToString()));
            }
            if (!IsPostBack&&straction == "edit")
            {
                
                //bool TempletIsExist = false;                      //初始化保存的模板ID号不存在 
                //如果修改,则显示要修改的记录
                DataSet ds = dal.CommonGuide_Edit_View(intID);
                TBGuideName.Text = ds.Tables[0].Rows[0]["CommGuideName"].ToString();
                TBManaDept.Text = ds.Tables[0].Rows[0]["ManaDept"].ToString();
                TBGuideNum.Text = ds.Tables[0].Rows[0]["GuideNum"].ToString();
                DDLGuideType.SelectedItem.Value = ds.Tables[0].Rows[0]["CommGuideTypeID"].ToString();
                DDLTemplet.SelectedItem.Value = ds.Tables[0].Rows[0]["TempletID"].ToString();
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

        protected void BtnSave_Click(object sender, AjaxEventArgs e)
        {
            //保存控件值
            if (TBGuideName.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示","指标名称不能为空！");
            }
            else if (DDLGuideType.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示","指标类别不能为空！");
            }
            else if (TBManaDept.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示","主管部门不能为空！");
            }
            else if (TBGuideNum.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示","指标分值不能为空！");
            }
            else if (DDLTemplet.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示","模版不能为空！");
            }
            else if (DDLDateCol.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示","模版的时间列不能为空！");
            }
            else if (DDLTargetCol.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示","模版的部门列不能为空！");
            }
            else if (DDLGuideNameColValue.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "模版的数值列不能为空！");
            }
            else
            {
                Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
                string strCommGuideName = CleanString.InputText(TBGuideName.Text.ToString(), 25);
                int intCommGuideTypeID = Convert.ToInt32(DDLGuideType.SelectedItem.Value.ToString());
                string strManaDept = CleanString.InputText(TBManaDept.Text.ToString(), 25);
                double douGuideNum = Convert.ToDouble(TBGuideNum.Text.ToString());

                int intTemplet = Convert.ToInt32(DDLTemplet.SelectedItem.Value.ToString());
                int intDateCol = Convert.ToInt32(DDLDateCol.SelectedItem.Value.ToString());
                int intTargetCol = Convert.ToInt32(DDLTargetCol.SelectedItem.Value.ToString());
                int intGuideNameCol = 1;
                int intGuideNameColValue = Convert.ToInt32(DDLGuideNameColValue.SelectedItem.Value.ToString());
                int intArithmetic = 1;

                try
                {
                    if (straction == "add")  //保存新增数据
                    {

                        bool CommGuideNameAdd = dal.CommonGuide_Add(strCommGuideName, intCommGuideTypeID, strManaDept, douGuideNum, intTemplet, intDateCol, intTargetCol, intGuideNameCol, intGuideNameColValue, intArithmetic);
                        if (CommGuideNameAdd == true)
                        {

                            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                            scManager.AddScript("parent.RefreshData();");
                            scManager.AddScript("parent.guideinfo.hide();");
                        }
                    }

                    else
                    {
                        if (straction == "edit")   //保存修改数据
                        {

                            bool CommGuideNameEdit = dal.CommonGuide_Edit(strCommGuideName, intCommGuideTypeID, strManaDept, douGuideNum, intTemplet, intDateCol, intTargetCol, intGuideNameCol, intGuideNameColValue, intArithmetic, intID);
                            if (CommGuideNameEdit == true)
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
                    ShowDataError(ex, Request.Url.LocalPath, "BtnSave_Click");
                }
            }
        }

        protected void DDLTemplet_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDLTemplet_Selected();
           
        }

        private void DDLTemplet_Selected()
        {
            if (DDLTemplet.SelectedItem.Value != "")
            {
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript(DDLDateCol.ClientID + ".store.removeAll();");
                scManager.AddScript(DDLDateCol.ClientID + ".clearValue();");
                scManager.AddScript(DDLTargetCol.ClientID + ".store.removeAll();");
                scManager.AddScript(DDLTargetCol.ClientID + ".clearValue();");
                scManager.AddScript(DDLGuideNameColValue.ClientID + ".store.removeAll();");
                scManager.AddScript(DDLGuideNameColValue.ClientID + ".clearValue();");
                
                //选择的模板
                int templetID = Convert.ToInt16(DDLTemplet.SelectedItem.Value.ToString());
                //构造模板对象
                TempletBO templet = new TempletBO(templetID);
                //时间
                FieldCollectionTable ddldatecolrow = templet.GetDateFields();
                for (int i = 0; i < ddldatecolrow.Rows.Count; i++)
                {
                    this.DDLDateCol.AddItem(ddldatecolrow.Rows[i]["FieldName"].ToString(), ddldatecolrow.Rows[i]["ID"].ToString());

                    //this.DDLDateCol.Items.Add(new Goldnet.Ext.Web.ListItem(ddldatecolrow.Rows[i]["FieldName"].ToString(), ddldatecolrow.Rows[i]["ID"].ToString()));

                }

                this.DDLDateCol.SelectedIndex = 0;
                //部门
                FieldCollectionTable ddltargetcolrow = templet.GetDeptFields();
                for (int i = 0; i < ddltargetcolrow.Rows.Count; i++)
                {
                    this.DDLTargetCol.AddItem(ddltargetcolrow.Rows[i]["FieldName"].ToString(), ddltargetcolrow.Rows[i]["ID"].ToString());
                    //this.DDLTargetCol.Items.Add(new Goldnet.Ext.Web.ListItem(ddltargetcolrow.Rows[i]["FieldName"].ToString(), ddltargetcolrow.Rows[i]["ID"].ToString()));

                }
                this.DDLTargetCol.SelectedIndex = 0;
                //数值
                FieldCollectionTable ddlguidenamecolvaluerow = templet.GetNumFields();
                for (int i = 0; i < ddlguidenamecolvaluerow.Rows.Count; i++)
                {
                    this.DDLGuideNameColValue.AddItem(ddlguidenamecolvaluerow.Rows[i]["FieldName"].ToString(), ddlguidenamecolvaluerow.Rows[i]["ID"].ToString());
                    //this.DDLGuideNameColValue.Items.Add(new Goldnet.Ext.Web.ListItem(ddlguidenamecolvaluerow.Rows[i]["FieldName"].ToString(), ddlguidenamecolvaluerow.Rows[i]["ID"].ToString()));
                }
                this.DDLGuideNameColValue.SelectedIndex = 0;
                //内容
                FieldCollectionTable ddlguidenamecolrow = templet.GetGuidFields();
                for (int i = 0; i < ddlguidenamecolrow.Rows.Count; i++)
                {
                    this.DDLGuideNameCol.AddItem(ddlguidenamecolrow.Rows[i]["FieldName"].ToString(), ddlguidenamecolrow.Rows[i]["ID"].ToString());
                    //this.DDLGuideNameCol.Items.Add(new Goldnet.Ext.Web.ListItem(ddlguidenamecolrow.Rows[i]["FieldName"].ToString(), ddlguidenamecolrow.Rows[i]["ID"].ToString()));
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
    }
}
