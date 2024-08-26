using System;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace FixGride
{
    [DefaultProperty("Column")]
    [ToolboxData("<{0}:ShowDate runat=server></{0}:ShowDate>")]

    public class ShowDate : WebControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("Ch")]
        [Localizable(true)]
        [Description("固定列表格控件-属性")]
        public string Column
        {
            get
            {
                String s = (String)ViewState["Column"];
                return ((s == null) ? "Ch" : s);
            }
            set
            {
                ViewState["Column"] = value;
            }
        }

        public DataTable Columns
        {
            get
            {
                DataTable s = (DataTable)ViewState["Columns"];
                return s;
            }
            set
            {
                ViewState["Columns"] = value;
            }
        }

        public DataTable DataSource
        {
            get
            {
                DataTable s = (DataTable)ViewState["DataSource"];
                return s;
            }
            set
            {
                ViewState["DataSource"] = value;
            }
        }

        public DataTable DataViewSource
        {
            get
            {
                DataTable s = (DataTable)ViewState["DataViewSource"];
                return s;
            }
            set
            {
                ViewState["DataViewSource"] = value;
            }
        }

        public string Tag
        {
            get
            {
                String s = (String)ViewState["Tag"];
                return s;
            }
            set
            {
                ViewState["Tag"] = value;
            }
        }


        public string TagID
        {
            get
            {
                String s = (String)ViewState["TagID"];
                return s;
            }
            set
            {
                ViewState["TagID"] = value;
            }
        }

        public string Pageid
        {
            get
            {
                String s = (String)ViewState["Pageid"];
                return s;
            }
            set
            {
                ViewState["Pageid"] = value;
            }
        }

        public string TagMode
        {
            get
            {
                String s = (String)ViewState["TagMode"];
                return s;
            }
            set
            {
                ViewState["TagMode"] = value;
            }
        }

        public string RMode
        {
            get
            {
                String s = (String)ViewState["RMode"];
                return s;
            }
            set
            {
                ViewState["RMode"] = value;
            }
        }

        /// <summary>
        /// 初始化脚本引用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            ClientScriptManager curManager = Page.ClientScript;
            curManager.RegisterClientScriptInclude(this.GetType(), "script01", "../../lib/jquery/jquery-1.5.2.min.js");
            curManager.RegisterClientScriptInclude(this.GetType(), "script02", "../../lib/ligerUI/js/ligerui.min.js");
            curManager.RegisterClientScriptInclude(this.GetType(), "script03", "../../lib/js/ligerui.expand.js");
            curManager.RegisterClientScriptInclude(this.GetType(), "script04", "../../lib/json2.js");
            curManager.RegisterClientScriptInclude(this.GetType(), "script05", "../../lib/js/common.js");
            curManager.RegisterClientScriptInclude(this.GetType(), "script06", "../../lib/js/LG.js");
            curManager.RegisterClientScriptInclude(this.GetType(), "script07", "../../lib/js/fieldType.js");

            base.OnPreRender(e);
        }

        /// <summary>
        /// 生成控件
        /// </summary>
        /// <param name="output"></param>
        protected override void RenderContents(HtmlTextWriter output)
        {
            StringBuilder S = new StringBuilder();

            //定义表格数据
            S.Append(@"<script type=text/javascript>
                        var grid = null;
                        var jsonObj = { Rows:[");

            if (!Convert.IsDBNull(DataViewSource) && !Convert.IsDBNull(Columns))
            {
                for (int i = 0; i < DataViewSource.Rows.Count; i++)
                {
                    S.Append(@"{ UNIT_NAME: '" + DataViewSource.Rows[i]["UNIT_NAME"].ToString() + "', ");

                    for (int j = 0; j < Columns.Rows.Count; j++)
                    {
                        string colstr = Columns.Rows[j]["guide_code"].ToString();
                        S.AppendFormat(@"{0}: {1}, ", colstr, DataViewSource.Rows[i][colstr].ToString());
                    }
                    S.Append(@"UNIT_CODE: '" + DataViewSource.Rows[i]["UNIT_CODE"].ToString() + "'}");

                    if (i < DataViewSource.Rows.Count - 1)
                    {
                        S.Append(",");
                    }
                }
            }
            S.Append("] }; ");

            //定义表格
            S.Append("$(function () {grid = $(\"#maingrid\").ligerGrid({");
            S.Append(@"columns: [");
            S.Append(@"{ display: '科室', name: 'UNIT_NAME', align: 'left', width: 200, minWidth: 60, frozen: true },");

            //判断数据列属性是否为空
            if (!Convert.IsDBNull(Columns))
            {
                for (int i = 0; i < Columns.Rows.Count; i++)
                {
                    S.Append(@"{ display: '" + Columns.Rows[i]["guide_name"].ToString() + "', name: '" + Columns.Rows[i]["guide_code"].ToString() + "', align: 'right', width: " + Columns.Rows[i]["SHOW_WIDTH"].ToString() + ", minWidth: 60,type: 'int'},");
                }
            }

            S.Append(@"
                { display: '科室', name: 'UNIT_NAME', align: 'left', width: 20, minWidth: 60, hide:true},
                { display: '科室ID', name: 'UNIT_CODE', align: 'left', width: 20, minWidth: 60, type: 'int',hide:true}
                ], 
                data: jsonObj,
                dataAction: 'server', 
                toolbar: toolbarOptions,
                width: '99%', 
                height: '98%', 
                heightDiff: -5, 
                checkbox: false, 
                usePager: false, 
                rownumbers: true,
                onDblClickRow: function (data, rowindex, rowobj) {
                    LG.ajax({");
            
            S.Append("type: 'AjaxSystem',");
            S.Append("method: 'PersonClick',");
            S.Append("loading: '正在执行中...',");
            S.Append("data: { UNITCODE: data.UNIT_CODE,UNITNAME: data.UNIT_NAME,tag:'"+Tag+"',bonusid:'"+TagID+"',pageid:'"+Pageid+"',tagMode:'"+TagMode+"'},");
            S.Append(@"     success: function (data) {
                                window.location.href=data;
                            },
                            error: function (message) {
                                LG.showError(message);
                            }
                        });
                }
            });
            });
        var toolbarOptions = {
            items: [ ");
            S.Append("{ text: '返回', id: 'addstr', click: toolbarBtnItemClick, img: \"../../lib/icons/32X32/sign_out.gif\" },");
            S.Append("{ line: true },");
            S.Append("{ text: 'EXCEL导出', id: 'impexcel', click: toolbarBtnItemClick, img: \"../../lib/icons/32X32/billing.gif\" },");
            S.Append(@"{ line: true }
        ]
        };

            function toolbarBtnItemClick(item) {
            switch (item.id) { ");
            S.Append("case \"impexcel\":");
            S.Append(@"f_openWindow('print.aspx?exporttype=xls', '导出到Excel', 200, 100);
                      
                      break;");
            S.Append("case \"addstr\":");
            S.Append(@"LG.ajax({
                            type: 'AjaxSystem',
                            method: 'BackClick',
                            loading: '正在执行中...',");

            S.Append("data: { tag:'" + Tag + "',bonusid:'" + TagID + "',pageid:'" + Pageid + "',tagMode:'" + TagMode + "',rMode:'" + RMode + "' },");
            S.Append(@"success: function (data) {
                                window.location.href=data;
                            },
                            error: function (message) {
                                LG.showError(message);
                            }
                        });
                    break;   
            }
        }

        function f_openWindow(url, title, width, height) {
            dialog = $.ligerDialog.open({ width: width, height: height, title: title, url: url, isResize: true
            });
        }
            </script>
            ");

            output.AddAttribute(HtmlTextWriterAttribute.Id, "maingrid");
            output.AddAttribute(HtmlTextWriterAttribute.Style, "margin: 2px;");
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            output.RenderEndTag();
            //window.location.href=message;
            // LG.showSuccess(data);
            output.Write(S);
        }


    }
}
