using System;
using System.Data;
using Goldnet.Ext.Web;



namespace GoldNet.JXKP.auth.login
{
    public partial class register_detail : System.Web.UI.Page
    {
        GoldNet.Model.SysManager.register reg = new GoldNet.Model.SysManager.register();

        protected void Page_Load(object sender, EventArgs e)
        {
            //DataTable dt = reg.GetUser_new();

            //string pass = GoldNet.Comm.DEncrypt.Decrypt( dt.Rows[0]["PASSWORD"].ToString());

            if (!Ext.IsAjaxRequest)
            {
                string op = Request.QueryString["op"].ToString();


                if (op == "edit")
                {
                    string id = Request.QueryString["id"].ToString();

                    DataTable dt = reg.GetUser_newById(id);

                    DeptCodeCombo.Value = dt.Rows[0]["USER_DEPT"].ToString();
                    USER_NAME.Value = dt.Rows[0]["USER_NAME"].ToString();
                    DB_USER.Value = dt.Rows[0]["DB_USER"].ToString();
                    PASSWORD.Value = GoldNet.Comm.DEncrypt.Decrypt(dt.Rows[0]["USER_PSWD"].ToString());

                    DB_USER.Disabled = true;
                    save.Disabled = false;
                }
                else
                {
                    save.Disabled = true;
                }
            }
        }

        //保存
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            string op = Request.QueryString["op"].ToString();

            string userdept = DeptCodeCombo.SelectedItem.Value;
            string username = USER_NAME.Value.ToString();
            string dbuser = DB_USER.Value.ToString();
            string ps = PASSWORD.Value.ToString();
            string pass = GoldNet.Comm.DEncrypt.Encrypt(ps);

            if (op == "edit")
            {
                string id = Request.QueryString["id"].ToString();
                reg.UpdateUser_new(userdept, username,  pass, id);
            }
            else
            {
                reg.AddUser(userdept, username, dbuser.ToUpper(), pass);
            }

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.Store1.reload();");
            scManager.AddScript("parent.DetailWin.hide();");

        }

        //判断DBUSER是否重复
        [AjaxMethod]
        public string GetDbuser(string dbuser)
        {
            string op = Request.QueryString["op"].ToString();
            if (op == "edit")
            {
                return "0";
            }
            else
            {
                return reg.GetdbUser(dbuser.ToUpper());
            }
        }
    }
}
