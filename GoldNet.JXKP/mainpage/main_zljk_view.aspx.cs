using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Web.UI.HtmlControls;
using System.Web.UI;


namespace GoldNet.JXKP.mainpage
{
    public partial class main_zljk_view : System.Web.UI.Page
    {
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
                string tpid = Request["tpid"].ToString();
                string tbid = Request["tbid"].ToString();
                int tmpid;
                if ((!Int32.TryParse(tpid, out tmpid)) | (!Int32.TryParse(tbid, out tmpid)))
                {
                    Response.End();
                    return;
                }

                string sqlStr = "SELECT ID, Name, Title, TabName FROM ZLGL.T_TempletDict WHERE (id = ?) AND (Deleted = 0)";
                DataTable dt = OracleOledbBase.ExecuteDataSet(sqlStr, new OleDbParameter("id", tpid)).Tables[0];
                if (dt.Rows.Count != 1)
                {
                    Response.End();
                    return;
                }
                
                string tbname = dt.Rows[0]["TabName"].ToString();
                string sqlStr1 = "SELECT * FROM ZLGL." + tbname + "   WHERE (ID = '" + tbid + "') ";
                string sqlStr2 = "SELECT A.ID,A.FIELDNAME FIELDLABEL , B.NAME AS TYPE ,UPPER(REPLACE(B.CLASSNAME,'Field')) AS TYPECLASS, REPLACE(B.CLASSNAME,'Field', '_'||TO_CHAR(A.ID)) AS FIELDNAME  FROM ZLGL.T_TEMPLETFIELDDICT A LEFT JOIN ZLGL.T_FIELDTYPEDICT B ON A.FIELDTYPEID = B.ID WHERE A.TEMPLETID ='" + tpid + "' ORDER BY A.SORTNUM";
                DataTable dt1 = OracleOledbBase.ExecuteDataSet(sqlStr1).Tables[0];
                DataTable dt2 = OracleOledbBase.ExecuteDataSet(sqlStr2).Tables[0];

                if (dt1.Rows.Count > 0 & dt2.Rows.Count > 0)
                {
                    this.labTitle.Text = dt.Rows[0]["Title"].ToString();
                    this.labCommon.Text = "记录创建者: " + dt1.Rows[0]["CREATESTAFF"].ToString() + "   创建时间:" + dt1.Rows[0]["CREATEDATE"].ToString() + "<br/>最后修改人: " + dt1.Rows[0]["LASTEDITSTAFF"].ToString() + "   修改时间:" + dt1.Rows[0]["LASTEDITDATE"].ToString();

                    HtmlTableCell cellFieldName, cellFieldInput;
                    HtmlTableRow rowInput;
                    string val,fieldname,fieldname_tmp;
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        cellFieldName = new HtmlTableCell();
                        cellFieldInput = new HtmlTableCell();
                        rowInput = new HtmlTableRow();

                        cellFieldName.Controls.Add(new LiteralControl(dt2.Rows[i]["FIELDLABEL"] + ":"));
                        cellFieldName.VAlign = "top";
                        cellFieldName.Width = "20%";
                        cellFieldName.Attributes["class"] = "gs-input-desc";
                        cellFieldInput.Attributes["class"] = "gs-input-section";
                        cellFieldInput.Width = "80%";
                        val = "";
                        fieldname =   dt2.Rows[i]["FIELDNAME"].ToString().ToUpper();
                        if (dt2.Rows[i]["TYPECLASS"].ToString().Equals("GUIDE"))
                        { 
                            fieldname_tmp = "GUIDE_STANDARD_" + dt2.Rows[i]["ID"].ToString().ToUpper();
                            val = dt1.Rows[0][fieldname].ToString().Replace("\n", "<br/>") + "<br/><fieldset style='width:80%' class='gs-input-desc'><legend><span class='gs-input-desc'>考评标准：</span></legend>" + dt1.Rows[0][fieldname_tmp].ToString().Replace("\n", "<br>") + "</fieldset>";
                        }
                        else
                        {
                            val = dt1.Rows[0][fieldname].ToString().Replace("\n", "<br/>");
                        }
                        cellFieldInput.Controls.Add(new LiteralControl(val));
                        rowInput.Cells.Add(cellFieldName);
                        rowInput.Cells.Add(cellFieldInput);
                        ((HtmlTable)this.Page.FindControl("tabInput")).Rows.Add(rowInput);

                    }
                }
            }
        }
    }
}
