using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Web.Script.Serialization;

namespace GoldNet.JXKP.Bonus.Set
{
    public partial class SET_DEPT_BONUSPERCENT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            if (action == "getData")
            {
                GetData();
            }
            else if (action == "deleteRow")
            {
                DeleteRow();
            }
            else if (action == "deleteRows") // 处理批量删除
            {
                DeleteRows();
            }
            else if (action == "saveRow")
            {
                SaveRow();
            }
        }

        private void GetData()
        {
            DeptPercent deptpercent = new DeptPercent();
            string selectedDate = Request.QueryString["date"];
            DataTable table = deptpercent.BonusGuidelist_NEWUPDATE(selectedDate, "");

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            List<Dictionary<string, string>> columns = new List<Dictionary<string, string>>();

            // 添加复选框
            columns.Add(new Dictionary<string, string>
            { 
                {"data", (string)null },
                {"title",""},
                {"defaultContent","<input type='checkbox' class='row-select' />"}
            });

            // 获取列信息并格式化数据
            foreach (DataColumn col in table.Columns)
            {
                columns.Add(new Dictionary<string, string>
                {
                    { "data", col.ColumnName },
                    { "title", col.ColumnName }
                });
            }

            // 添加编辑和删除按钮的列
            columns.Add(new Dictionary<string, string>
            { 
                {"data", (string)null },
                {"title","Actions"},
                {"defaultContent","<button class='edit-btn custom-button'>编辑</button> <button class='delete-btn custom-button-del'>删除</button>"}
            });

            // 格式化每一行的数据
            foreach (DataRow row in table.Rows)
            {
                Dictionary<string, object> rowData = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    object value = row[col];
                    if (value is DateTime)
                    {
                        // 格式化日期
                        value = ((DateTime)value).ToString("yyyy-MM-dd");
                    }
                    rowData.Add(col.ColumnName, value);
                }
                rows.Add(rowData);
            }

            var result = new
            {
                columns = columns,
                data = rows,
                recordsTotal = table.Rows.Count,
                recordsFiltered = table.Rows.Count
            };

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            string json = jsSerializer.Serialize(result);

            Response.ContentType = "application/json";
            Response.Write(json);
            Response.End();
        }

        private void DeleteRow()
        {
            string id = Request["id"];
            // 从数据库中删除指定的行
            // 示例代码，实际应该连接数据库并执行删除操作
            bool success = true; // 假设删除成功

            Response.ContentType = "application/json";
            Response.Write("{\"success\":" + (success ? "true" : "false") + "}");
            Response.End();
        }

        private void DeleteRows()
        {
            string ids = Request["ids"];
            string[] idArray = ids.Split(',');

            // 从数据库中删除指定的多行
            // 示例代码，实际应该连接数据库并执行批量删除操作
            bool success = true; // 假设删除成功

            Response.ContentType = "application/json";
            Response.Write("{\"success\":" + (success ? "true" : "false") + "}");
            Response.End();
        }

        private void SaveRow()
        {
            string id = Request["id"];
            string name = Request["name"];
            string age = Request["age"];

            // 执行更新数据库操作
            bool success = true; // 假设保存成功

            Response.ContentType = "application/json";
            Response.Write("{\"success\":" + (success ? "true" : "false") + "}");
            Response.End();
        }
    }
}