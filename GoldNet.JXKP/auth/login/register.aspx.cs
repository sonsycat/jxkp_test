using System;
using System.Collections.Generic;
using System.Text;
using Goldnet.Ext.Web;
using GoldNet.Model;

namespace GoldNet.JXKP.auth.login
{
    public partial class register : System.Web.UI.Page
    {
        GoldNet.Model.SysManager.register reg = new GoldNet.Model.SysManager.register();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                BindGrid();

            }
        }
        private void BindGrid()
        {
            string user_id = ((User)Session["CURRENTSTAFF"]).UserId;
            Store1.DataSource = reg.GetUser_new_new(user_id, this.tfBName.Value.ToString());
            Store1.DataBind();
            string bb = reg.GetQX(user_id);
            if (!bb.Equals("1") && !bb.Equals("21"))
            {
                Button_add.Hidden = true;
                Button_del.Hidden = true;
                Button1.Hidden = true;
                tfBName.Hidden = true;
            }

        }

        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            BindGrid();
        }



        //添加记录
        protected void Button_add_click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = new LoadConfig();
            loadcfg.Url = "register_detail.aspx";
            loadcfg.Mode = LoadMode.IFrame;
            loadcfg.MaskMsg = "载入中...";
            loadcfg.ShowMask = true;
            loadcfg.NoCache = true;

            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "add"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", ""));

            showDetailWin(loadcfg);
        }

        //修改记录
        [AjaxMethod]
        public void data_edit(string id)
        {
            LoadConfig loadcfg = new LoadConfig();
            loadcfg.Url = "register_detail.aspx";
            loadcfg.Mode = LoadMode.IFrame;
            loadcfg.MaskMsg = "载入中...";
            loadcfg.ShowMask = true;
            loadcfg.NoCache = true;

            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "edit"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));

            showDetailWin(loadcfg);
        }

        //删除
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
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
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < selectRow.Length; i++)
                {
                    str.AppendFormat("'{0}',", selectRow[i]["USER_ID"]);
                }
                string strsql = str.ToString();
                strsql = strsql.Substring(0, strsql.LastIndexOf(','));
                reg.DelUser_new(strsql);
                BindGrid();
            }
        }

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        //显示添加窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        //时间改变再去验证奖励性绩效是否生成
        protected void Date_SelectOnChange(object sender, EventArgs e)
        {

        }
        protected void Btn_Search_Click(object sender, AjaxEventArgs e)
        {
            BindGrid();
        }




    }
}
