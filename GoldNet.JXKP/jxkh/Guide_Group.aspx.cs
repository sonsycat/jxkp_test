using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;

namespace GoldNet.JXKP.jxkh
{
    public partial class Guide_Group : PageBase
    {
        Goldnet.Dal.Guide_Group dal = new Goldnet.Dal.Guide_Group();

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
                return;
            }
            if (!Ext.IsAjaxRequest)
            {
                BindGridPanel("");
                BindTypeCombox();
                BindOrganClass();
                BindEvaluationYear();
            }
        }

        /// <summary>
        /// 获取指标集信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            BindGridPanel("");
        }

        /// <summary>
        /// 初始化“适用年份”列表
        /// </summary>
        protected void BindEvaluationYear()
        {
            this.CombEvaluationYear.Items.Add(new Goldnet.Ext.Web.ListItem("通用", "all"));
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.CombEvaluationYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
        }

        /// <summary>
        /// 初始化指标分类列表（院、科、人、组）
        /// </summary>
        protected void BindOrganClass()
        {
            DataTable dt = dal.GetOrganClass().Tables[0];
            this.StoreCombo2.DataSource = dt;
            this.StoreCombo2.DataBind();
        }

        /// <summary>
        /// 绑定数据表格列表
        /// </summary>
        /// <param name="wherestr"></param>
        protected void BindGridPanel(string wherestr)
        {
            DataTable table = dal.GetGuideGroup(wherestr).Tables[0];
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 绑定指标集类别大分类
        /// </summary>
        protected void BindTypeCombox()
        {
            DataTable dt = dal.GetGuideGroupTypeClass().Tables[0];
            this.StoreCombo.DataSource = dt;
            this.StoreCombo.DataBind();
        }

        /// <summary>
        /// 大类别选择刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GuideGroupType_Selected(object sender, AjaxEventArgs e)
        {
            string typestr = this.Combo_GuideGroupType.SelectedItem.Value.ToString();
            BindGridPanel("GUIDE_ATTR LIKE'" + typestr + "%'");
            //绑定小分类
            DataTable table = dal.GetGuideGroupType(typestr).Tables[0];
            this.StoreCombo1.DataSource = table;
            this.StoreCombo1.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CombClass1_Selected(object sender, AjaxEventArgs e)
        {
            string typestr = this.CombClass1.SelectedItem.Value.ToString();
            BindGridPanel("GUIDE_ATTR LIKE'" + typestr + "%'");
            //绑定小分类
            DataTable table = dal.GetGuideGroupType(typestr).Tables[0];
            this.StoreCombo1.DataSource = table;
            this.StoreCombo1.DataBind();
        }

        /// <summary>
        /// 小分类选择事件刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GuideGroupClass_Selected(object sender, AjaxEventArgs e)
        {
            string typestr = this.Combo_GuideGroupClass.SelectedItem.Value.ToString();
            BindGridPanel("GUIDE_ATTR LIKE'" + typestr + "%'");
        }

        /// <summary>
        /// 增加按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            this.CombClass1.SetValue(this.Combo_GuideGroupType.SelectedItem.Value.ToString());
            this.CombClass2.SetValue(this.Combo_GuideGroupClass.SelectedItem.Value.ToString());
            this.GuideGroupName.SetValue("");
            this.CombOrganClass.SelectedIndex = 0;
            this.CombEvaluationYear.SelectedIndex = 0;
            this.SelectedID.SetValue("");
            this.EditWin.SetTitle("添加指标集");
            this.EditWin.Show();
        }

        /// <summary>
        /// 编加按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            string values = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(values);
            this.SelectedID.SetValue(selectedRow[0]["GUIDE_GATHER_CODE"]);
            this.CombClass1.SetValue(selectedRow[0]["GUIDE_ATTR"].Substring(0, 2));
            DataTable table = dal.GetGuideGroupType(selectedRow[0]["GUIDE_ATTR"].Substring(0, 2)).Tables[0];
            this.StoreCombo1.DataSource = table;
            this.StoreCombo1.DataBind();
            this.CombClass2.SetValue(selectedRow[0]["GUIDE_ATTR"]);
            this.GuideGroupName.SetValue(selectedRow[0]["GUIDE_GATHER_NAME"]);
            this.CombOrganClass.SetValue(selectedRow[0]["ORGAN_CLASS"]);
            this.CombEvaluationYear.SetValue(selectedRow[0]["EVALUATION_YEAR"]);
            this.EditWin.SetTitle("编辑指标集");
            this.EditWin.Show();
        }

        /// <summary>
        /// 指标列表命令按钮执行处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Command_Click(object sender, AjaxEventArgs e)
        {
            string command = e.ExtraParams["command"].ToString();
            string guidegathercode = e.ExtraParams["Values"].ToString();
            string guidegathername = e.ExtraParams["Names"].ToString();
            string guidegatherorgan = e.ExtraParams["Organ"].ToString();
            //指标编辑
            if (command == "GuideDefine")
            {
                Session.Add("GUIDEGATHERDEFINE", guidegathercode + ',' + guidegatherorgan);
                showDetailWin(getLoadConfig("Guide_Selector.aspx"), "指标选择--" + guidegathername, "");
            }
            //权限维护
            if (command == "GuideGatherRole")
            {
                LoadConfig loadcfg = getLoadConfig("Guide_Group_Role.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("guidegathercode", guidegathercode));
                RoleEditWin.ClearContent();
                RoleEditWin.Show();
                RoleEditWin.LoadContent(loadcfg);
            }
        }

        /// <summary>
        /// 复制指标集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Copy_Click(object sender, AjaxEventArgs e)
        {
            string codeStr = e.ExtraParams["Values"].ToString();
            this.CopyField.SetValue(codeStr);
            this.NewField.SetValue("");
            this.CopyWin.Show();
        }

        /// <summary>
        /// 复制指标集保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_SaveCopy_Click(object sender, AjaxEventArgs e)
        {
            string newName = this.NewField.Value.ToString().Trim().Replace("'", "''");
            string copyid = this.CopyField.SelectedItem.Value.ToString();
            if (copyid.Equals("")) return;
            if (newName.Equals(""))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle4,
                    Message = "请输入新指标集名！",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            else
            {
                string rtn = "";
                rtn = dal.CopyGuideGroup(copyid, newName);
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle4,
                    Message = (rtn.Equals("") ? "复制指标集成功！" : rtn),
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                if (rtn.Equals(""))
                {
                    this.CopyWin.Hide();
                    Store_RefreshData(null, null);
                }
            }
        }

        /// <summary>
        /// 删除指标集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            string codeStr = e.ExtraParams["Values"];
            string rtn = "";
            rtn = dal.DelGuideGroupByid(codeStr);
            if (rtn.Equals(""))
            {
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript(this.GridPanel_List.ClientID + ".deleteSelected();");
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle4,
                    Message = "删除操作失败！",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }

        /// <summary>
        /// 更新保存指标集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Save_Click(object sender, AjaxEventArgs e)
        {
            string guidegrouptype = CombClass1.SelectedItem.Value.ToString();
            string guidegroupclass = CombClass2.SelectedItem.Value.ToString();
            string selectedid = SelectedID.Value.ToString();
            string guidegroupname = GuideGroupName.Value.ToString().Trim().Replace("'", "");
            string organclass = CombOrganClass.SelectedItem.Value.ToString();
            string evaluationyear = CombEvaluationYear.SelectedItem.Value.ToString();
            string rtn = "";

            if (guidegroupname.Equals(""))
            {
                rtn = "请输入指标集名称！";
            }
            if (guidegrouptype.Equals("") || guidegroupclass.Equals(""))
            {
                rtn = "请选择指标集类别！";
            }
            if (organclass.Equals("") || evaluationyear.Equals(""))
            {
                rtn = "请选择指标集适用对象及年份！";
            }
            if (!rtn.Equals(""))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle4,
                    Message = rtn,
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            rtn = dal.UpdateGuideGroup(selectedid, guidegroupname, guidegroupclass, organclass, evaluationyear);
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = SystemMsg.msgtitle4,
                Message = (rtn.Equals("") ? "保存成功！" : rtn),
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
            if (rtn.Equals(""))
            {
                this.EditWin.Hide();
                Store_RefreshData(null, null);
            }
        }

        //显示详细窗口
        private void showDetailWin(LoadConfig loadcfg, string title, string width)
        {
            DetailWin.ClearContent();
            if (!title.Trim().Equals(""))
            {
                DetailWin.SetTitle(title);
            }
            if (!width.Trim().Equals(""))
            {
                DetailWin.Width = Unit.Pixel(Convert.ToInt16(width));
            }
            DetailWin.Center();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        ////载入参数设置
        //private LoadConfig getLoadConfig(string url)
        //{
        //    LoadConfig loadcfg = new LoadConfig();
        //    loadcfg.Url = url;
        //    loadcfg.Mode = LoadMode.IFrame;
        //    loadcfg.MaskMsg = "载入中...";
        //    loadcfg.ShowMask = true;
        //    loadcfg.NoCache = true;
        //    return loadcfg;
        //}
    }
}
