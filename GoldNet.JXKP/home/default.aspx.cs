using System;
using System.Collections;
using System.Data;
using System.Linq;
using Goldnet.Ext.Web;
using GoldNet.Model;

namespace Goldnet.JXKP.home
{
    public partial class _default : System.Web.UI.Page
    {
        string url = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = Session.SessionID;
            if (!IsPostBack)
            {
                if (Session["CURRENTSTAFF"] != null)
               {
                   HeadPanel.AutoLoad.NoCache = true;
                   DataTable powertable = ((User)Session["CURRENTSTAFF"]).GetUserPower;
                   Session["menu"] = powertable;
                   //if (powertable.Rows.Count > 0)
                   //{
                   //    url = powertable.Rows[0]["MODID"].ToString();
                   //}
                   
                   //this.tabHome.BodyContainer.InnerHtml = "<img src='../resources/images/image"+url+".jpg' width=100% />";
                   if (this.Request.QueryString.Count > 0) 
                   {
                       url = this.Request.QueryString[0];
                       this.tabHome.BodyContainer.InnerHtml = "<img src='../resources/images/image" + url + ".jpg' width=100%/>";
                   }
                   else
                   {
                       if (powertable.Rows.Count > 0)
                       {
                           url = powertable.Rows[0]["MODID"].ToString();
                       }

                       this.tabHome.BodyContainer.InnerHtml = "<img src='../resources/images/image" + url + ".jpg' width=100% />";
                   }

                   this.HeadPanel.AutoLoad.Url = "/home/header.aspx?" + url + System.DateTime.Now.ToString();
                   //从Session中获得用户在本子系统中所有权限菜单
                   DataRow[] dtPowerDetail;
                   dtPowerDetail = powertable.Select("MODID='" + url + "'");
                   ArrayList PowerGroup = new ArrayList();
                   string[] PowerGroupico = { "Application", "ApplicationForm", "ApplicationKey", "ApplicationViewColumns", "BookLink", "Bookmark", "BoxError", "Calendar", "CalendarLink", "Car", "CarRed", "CarStart", "CarStop", "ChartBar", "ChartBarEdit", "ChartBarLink", "Clipboard", "ClockEdit", "CogGo", "Connect", "DatabaseYellow", "Date", "DriveDisk", "Feed", "FolderHome", "FolderPageWhite", "FolderTable", "FolderUser", "Group", "GroupError", "GroupGear", "GroupKey", "Heart", "House", "HouseGo", "HouseKey", "HouseStar", "HtmlGo", "LayoutHeader", "LockEdit", "Lorry", "LorryAdd", "LorryStart", "MapEdit", "MoneyYen", "Note", "PageWhiteKey", "PrinterColor", "Report", "ReportKey", "Ruby", "ServerChart", "ServerWrench", "ShapeMoveForwards", "ShapeShadowToggle", "Smartphone", "SoundOut", "Table", "TableAdd", "TableCell", "TableDelete", "TagGreen", "Television", "TextListBullets", "Theme", "UserBrown", "UserEdit", "UserGray", "UserMagnify", "UserStar", "UserTick", "WeatherRain", "WeatherSnow", "Wrench" };
                   string strtmp = "";
                   string strmenuid = "";
                   string strmenuname = "";
                   string strpagename = "";
                   
                   //对权限菜单进行分组
                   for (int i = 0; i < dtPowerDetail.Count(); i++)
                   {
                       strtmp = dtPowerDetail[i]["GROUPTEXT"].ToString();
                       if (!PowerGroup.Contains(strtmp))
                       {
                           PowerGroup.Add(strtmp);
                       }
                   }
                   //生成分组树形折叠导航菜单
                   for (int i = 0; i < PowerGroup.Count; i++)
                   {
                       TreePanel tree = new TreePanel();
                       tree.Title = PowerGroup[i].ToString();
                       tree.RootVisible = false;
                       tree.Border = false;
                       tree.AutoScroll = true;
                       tree.Icon = (Icon)Enum.Parse(typeof(Icon), PowerGroupico[i]);
                       //tree.Tools.Add(new Tool(ToolType.Refresh, "Ext.Msg.alert('Message','Refresh Tool Clicked!');", ""));

                       Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
                       root.NodeID = "0" + i.ToString();
                       tree.Root.Add(root);
                       
                       string lastnodemenu = "";
                       Goldnet.Ext.Web.TreeNode lastnode = new Goldnet.Ext.Web.TreeNode();

                       for (int j = 0; j < dtPowerDetail.Count(); j++)
                       {
                           if (dtPowerDetail[j]["GROUPTEXT"].ToString().Equals(PowerGroup[i].ToString()))
                           {
                               strmenuid = dtPowerDetail[j]["MENUID"].ToString() + "_" + dtPowerDetail[j]["MODID"].ToString();
                               strmenuname = dtPowerDetail[j]["MENUNAME"].ToString();
                               strpagename = dtPowerDetail[j]["PAGENAME"].ToString();
                               Goldnet.Ext.Web.TreeNode node1 = new Goldnet.Ext.Web.TreeNode();
                               node1.Text = strmenuname;
                               node1.NodeID = strmenuid;
                               node1.Expanded = true;
                               if (strmenuid.Substring(4, 2).Equals("00") && strpagename.Equals("blank.html"))
                               {
                                   root.Nodes.Add(node1);
                                   lastnodemenu = strmenuid.Substring(0, 4);
                                   lastnode = node1;
                               }
                               else
                               {
                                   try
                                   {
                                       node1.Icon = (Icon)Enum.Parse(typeof(Icon), dtPowerDetail[j]["ICO"].ToString());
                                       node1.Listeners.Click.Handler = "e.stopEvent();loadAppTab('" + dtPowerDetail[j]["PAGEURL"].ToString() + "', '" + strmenuid + "', '" + dtPowerDetail[j]["PAGETITLE"].ToString() + "', '" + dtPowerDetail[j]["ICO"].ToString() + "');";
                                       if (strmenuid.Substring(0, 4).Equals(lastnodemenu))
                                       {
                                           lastnode.Nodes.Add(node1);
                                       }
                                       else
                                       {
                                           root.Nodes.Add(node1);
                                       }
                                   }
                                   catch
                                   {
                                       Response.Redirect("/auth/login/login.aspx");
                                   }

                               }
                           }
                       }
                       this.according1.Items.Add(tree);
                   }
                }
                else
                {
                    Response.Redirect("/auth/login/login.aspx");
                }

            }

        }
  
        
        //注销登录
        protected void Logout(object sender, AjaxEventArgs e)
        {
            //注销用户
            Session.RemoveAll();
            //Response.Redirect("/auth/login/login.aspx");
            Mask.Config msgconfig = new Mask.Config();
            msgconfig.Msg = "页面转向中...";
            msgconfig.MsgCls = "x-mask-loading";
            Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
            Goldnet.Ext.Web.Ext.Redirect("/auth/login/login.aspx");
        }

        //每一分钟自动回传，检查是否有要弹出的提醒消息
        protected void RefreshTime(object sender, AjaxEventArgs e)
        {
        }
        


    }
}






