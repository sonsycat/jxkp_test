﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoldNet.JXKP
{
    public partial class print : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        void exportexcel()
        {
            string filename = DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");

            Response.ContentType = "application/ms-excel";

            this.EnableViewState = false;
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            oHtmlTextWriter.WriteLine(hf.Value);
            Response.Write(oStringWriter.ToString());
            Response.End();
            
        }

        void exportword()
        {
            string filename = DateTime.Now.ToString("yyyyMMddHHmm") + ".doc";
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");

            Response.ContentType = "application/ms-word";
            this.EnableViewState = false;
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            oHtmlTextWriter.WriteLine(hf.Value);
            Response.Write(oStringWriter.ToString());
            Response.End();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            exportexcel();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            exportword();
        }
    }
}