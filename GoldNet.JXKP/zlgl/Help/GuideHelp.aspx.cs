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

namespace GoldNet.JXKP.zlgl.Help
{
    public partial class GuideHelp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataFiller();
            }
        }
        private void DataFiller()
        {
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            string strResultsHolder;
            string TypeSign;
            int GuideTypeID;
            int GuideNameID;

            strResultsHolder = "<table width=100% border=0 cellSpacing=0 cellPadding=0>";
            DataTable DTGuideType = dal.GuideType_View().Tables[0];
            for (int i = 0; i < DTGuideType.Rows.Count; i++)
            {
                //指标类别		              
                strResultsHolder += "<tr><td><STRONG>" + DTGuideType.Rows[i]["GuideType"] + "</STRONG></td></tr>";

                TypeSign = DTGuideType.Rows[i]["TypeSign"].ToString();
                GuideTypeID = Convert.ToInt32(DTGuideType.Rows[i]["ID"].ToString());
                DataTable DTGuideName = dal.Guide_Name_Dict(TypeSign, GuideTypeID).Tables[0];

                for (int j = 0; j < DTGuideName.Rows.Count; j++)
                {
                    //指标名称					
                    strResultsHolder += "<tr><td><table width=100% border=0 cellSpacing=0 cellPadding=0>";
                    strResultsHolder += "<TBODY>";
                    strResultsHolder += "<tr><td ><a style=CURSOR:hand onclick=javascript:ShowHideGroup(document.all.groupField" + i + j + ",document.all.imgField" + i + j + ");>&nbsp;&nbsp;&nbsp;&nbsp;<IMG id=\"imgField" + i + j + "\"  src=\"images/PLUS.GIF\" border=0>" + DTGuideName.Rows[j]["GuideName"] + "</a></td></tr>";
                    GuideNameID = Convert.ToInt32(DTGuideName.Rows[j]["GuideNameID"].ToString());
                    DataTable DTGuideCont = dal.Guide_Cont(GuideNameID, GuideTypeID).Tables[0];

                    //指标内容
                    strResultsHolder += "<tbody id=\"groupField" + i + j + "\" style=\"display:none;\">";
                    strResultsHolder += "<tr><td align=center><table width=90% border=1 cellSpacing=0 cellPadding=0 style=\"border-width:1px;border-style:solid;border-collapse:collapse;\">";
                    strResultsHolder += "<tr><td width=20%>考评内容</td><td width=60%>考评标准</td><td width=20%>考评办法</td></tr>";

                    for (int k = 0; k < DTGuideCont.Rows.Count; k++)
                    {
                        strResultsHolder += "<tr align='left'><td>" + DTGuideCont.Rows[k]["CheckCont"] + "</td><td>" + DTGuideCont.Rows[k]["CheckStan"] + "</td><td>" + DTGuideCont.Rows[k]["CheckMeth"] + "</td></tr>";
                    }
                    strResultsHolder += "</table></td></tr>";
                    strResultsHolder += "</tbody>";

                }
                strResultsHolder += "</table></td></tr>";
                strResultsHolder += "<tr><td class=gs-input-separator></td></tr>";
            }

            strResultsHolder += "</table>";
            display.InnerHtml = strResultsHolder;
        }
    }
}
