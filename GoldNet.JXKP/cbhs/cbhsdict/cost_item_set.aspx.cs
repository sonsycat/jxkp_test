using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Model;

namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class cost_item_set : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                if (Request.QueryString["item_code"] != null)
                {
                    string item_code = Request.QueryString["item_code"].ToString();
                    if (item_code != null && !item_code.Equals(""))
                    {
                        Edit_Init(item_code);
                    }
                }
            }
        }
        //初始化界面下拉菜单
        protected void SetDict()
        {
            Cbhs_dict dal = new Cbhs_dict();
            //成本项目类别
            DataTable item_type = dal.GetCostItemType().Tables[0];
            for (int i = 0; i < item_type.Rows.Count; i++)
            {
                this.ITEM_TYPE.Items.Add(new Goldnet.Ext.Web.ListItem(item_type.Rows[i]["COST_TYPE_NAME"].ToString(), item_type.Rows[i]["COST_TYPE_CODE"].ToString()));
            }
            //成本属性
            DataTable item_property = dal.GetCostItemProperty().Tables[0];
            for (int i = 0; i < item_property.Rows.Count; i++)
            {
                this.COST_PROPERTY.Items.Add(new Goldnet.Ext.Web.ListItem(item_property.Rows[i]["COST_PROPERTY"].ToString(), item_property.Rows[i]["ID"].ToString()));
            }
            //军地分摊方案
            DataTable prog = dal.GetProgdict("0").Tables[0];
            for (int i = 0; i < prog.Rows.Count; i++)
            {
                this.ALLOT_FOR_JD.Items.Add(new Goldnet.Ext.Web.ListItem(prog.Rows[i]["PROG_NAME"].ToString(), prog.Rows[i]["PROG_CODE"].ToString()));
            }
            //级次分摊方案
            prog = dal.GetProgdict("1").Tables[0];
            for (int i = 0; i < prog.Rows.Count; i++)
            {
                this.ALLOT_FOR_JC.Items.Add(new Goldnet.Ext.Web.ListItem(prog.Rows[i]["PROG_NAME"].ToString(), prog.Rows[i]["PROG_CODE"].ToString()));
            }
            //人员分摊方案
            prog = dal.GetProgdict("2").Tables[0];
            for (int i = 0; i < prog.Rows.Count; i++)
            {
                this.ALLOT_FOR_RY.Items.Add(new Goldnet.Ext.Web.ListItem(prog.Rows[i]["PROG_NAME"].ToString(), prog.Rows[i]["PROG_CODE"].ToString()));
            }
            //直接/间接成本
            DataTable costdirect = dal.GetCOSTDIRECT().Tables[0];
            for (int i = 0; i < costdirect.Rows.Count; i++)
            {
                this.COST_DIRECT.Items.Add(new Goldnet.Ext.Web.ListItem(costdirect.Rows[i]["COST_DIRECT"].ToString(), costdirect.Rows[i]["ID"].ToString()));
            }
            //核算类型
            DataTable dt = dal.GetAccount_Type().Tables[0];
            Store2.DataSource = dt;
            Store2.DataBind();
            DataTable gettype = dal.GetGetType().Tables[0];
            for (int i = 0; i < gettype.Rows.Count; i++)
            {
                this.GETTYPE.Items.Add(new Goldnet.Ext.Web.ListItem(gettype.Rows[i]["GETTYPE"].ToString(), gettype.Rows[i]["ID"].ToString()));
            }

            string op=Request.QueryString["op"].ToString();
            if (op.Equals("add"))
            {
                this.save.Text = "添加";
                string type = Request.QueryString["id"].ToString();
                this.ITEM_TYPE.SelectedItem.Value = type;
            }
            if (op.Equals("edit"))
            {
                this.save.Text = "保存";
            }
            if (op.Equals("del"))
            {
                this.save.Text = "删除";
            }
        }
        //当父界面有选择行时，初始化界面控件
        protected void Edit_Init(string item_code)
        {
            Cbhs_dict dal = new Cbhs_dict();
            DataTable dt = dal.GetCostItemByCode(item_code).Tables[0];
            this.ROW_ID.Value = dt.Rows[0]["ROW_ID"].ToString();
            this.ITEM_TYPE.SelectedItem.Value = dt.Rows[0]["ITEM_TYPE"].ToString();
            this.ITEM_CODE.Value = dt.Rows[0]["ITEM_CODE"].ToString();
            this.ITEM_NAME.Value = dt.Rows[0]["ITEM_NAME"].ToString();
            this.INPUT_CODE.Value = dt.Rows[0]["INPUT_CODE"].ToString();
            this.COST_PROPERTY.SelectedItem.Value = dt.Rows[0]["COST_PROPERTY"].ToString();
            this.ALLOT_FOR_JD.SelectedItem.Value = dt.Rows[0]["ALLOT_FOR_JD"].ToString();
            this.ALLOT_FOR_JC.SelectedItem.Value = dt.Rows[0]["ALLOT_FOR_JC"].ToString();
            this.ALLOT_FOR_RY.SelectedItem.Value = dt.Rows[0]["ALLOT_FOR_RY"].ToString();
            this.GETTYPE.SelectedItem.Value = dt.Rows[0]["GETTYPE"].ToString();
            this.ACCOUNT_TYPE.SelectedItem.Value = dt.Rows[0]["ACCOUNT_TYPE"].ToString();
            this.COMPUTE_PER.Text =dt.Rows[0]["COMPUTE_PER"].ToString();
            this.COST_DIRECT.SelectedItem.Value = dt.Rows[0]["COST_DIRECT"].ToString();
            string op = Request.QueryString["op"].ToString();

        }
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            Cbhs_dict dal = new Cbhs_dict();

            CostItem model = new CostItem();
            model.Item_type = this.ITEM_TYPE.SelectedItem.Value;
            model.Item_class = this.ITEM_TYPE.SelectedItem.Text;
            model.Item_name = this.ITEM_NAME.Value.ToString();
            model.Input_code = this.INPUT_CODE.Value.ToString();
            model.Cost_property = this.COST_PROPERTY.SelectedItem.Value;
            model.Allot_for_jc = this.ALLOT_FOR_JC.SelectedItem.Value;
            model.Allot_for_jd = this.ALLOT_FOR_JD.SelectedItem.Value;
            model.Allot_for_ry = this.ALLOT_FOR_RY.SelectedItem.Value;
            model.Gettype = this.GETTYPE.SelectedItem.Value;
            model.Account_type = this.ACCOUNT_TYPE.SelectedItem.Value;
            model.Compute_per = Convert.ToDecimal(this.COMPUTE_PER.Value);
            model.Cost_direct = this.COST_DIRECT.SelectedItem.Value;
            //判断是添加还是修改
            if (this.save.Text.Equals("添加"))
            {
                //判断成本项目是否存在
                bool _is = dal.CostsIsExistByName(model.Item_name);
                if (_is)
                {
                    Ext.Msg.Alert("提示", "你添加的成本项目已经存在！").Show();
                }
                else
                {
                    //查询并生成项目代码
                    model.Item_code = dal.GetCostItemCountByType(model.Item_type);
                    try
                    {
                        dal.AddCostItem(model);
                        Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                        scManager.AddScript("parent.RefreshData('添加成功');");
                        scManager.AddScript("parent.DetailWin.hide();");
                    }
                    catch
                    {
                        Ext.Msg.Alert("提示","添加失败").Show();
                    }
                }
            }
            if (this.save.Text.Equals("保存"))
            {
                if (this.ITEM_CODE.Value.ToString().Substring(0,1).Equals(model.Item_type))
                {
                    model.Item_code = this.ITEM_CODE.Value.ToString();
                }
                else
                {
                    model.Item_code = dal.GetCostItemCountByType(model.Item_type);
                }
                try
                {
                    dal.UpdateCostItem(model, this.ROW_ID.Value.ToString());
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData('更新成功');");
                    scManager.AddScript("parent.DetailWin.hide();");
                }
                catch
                {
                    Ext.Msg.Alert("提示","更新失败").Show();
                }
            }
            if (this.save.Text.Equals("删除"))
            {
                dal.DelCostItem(this.ITEM_CODE.Value.ToString());
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.RefreshData('删除成功');");
                scManager.AddScript("parent.DetailWin.hide();");
            }
        }
    }
}
