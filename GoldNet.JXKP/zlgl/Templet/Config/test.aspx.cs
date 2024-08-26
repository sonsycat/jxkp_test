// using System.Data.SqlClient;
// 
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using GoldNet.JXKP.Templet.BLL;


namespace GoldNet.JXKP.Templet.Config
{
	/// <summary>
	/// test 的摘要说明。
	/// </summary>
	public class test : PageBase
	{
		protected RadioButtonList RadioButtonList1;
		protected RadioButtonList RadioButtonList2;
		protected Button Button1;
		protected CompareValidator CompareValidator1;
		protected TextBox TextBox1;
		protected Button Button2;
		protected DataGrid DataGrid1;
		protected Button Button3;
		protected LinkButton link;

		private void Page_Load(object sender, EventArgs e)
		{
			// Session["CURRENTSTAFF"] = new GoldNet.JXKP.Templet.Page.TestStaff();
			// Session["CURRENTSTAFF"] = new GoldNet.JXKP.BLL.Organise.Staff("531588");
			
			#region jjjj

			// 在此处放置用户代码以初始化页面

			// test.aspx?test=%3c 
			// Response.Write(Request.QueryString["test"]);

			// TempletBO.AddNewTemplet("aaa3", "bbb2", "ccc2");

			#region --insert Test-- 

			//string connstr = "Data Source=(local);Integrated Security=SSPI;database=test2";
			//string sql;

			// // 向视图表中添加一个视图，并返回视图编号，作为默认视图。
			//sql = @"INSERT INTO T_ViewDict (Name, TempletID, PageCount) VALUES (@name, @templetID, @pageCount) SELECT @@IDENTITY AS 'Identity'";
			//
			//int aa = Convert.ToInt32(
			//	OracleOledbBase.ExecuteScalar(connstr, sql, 
			//	new OleDbParameter("@name", "aaaa"),
			//	new OleDbParameter("@templetID", 8),
			//	new OleDbParameter("@pageCount", "0")
			//	));
			//Response.Write("aa:" + aa.ToString());

			#endregion

			// TextBox1.DataBind();
			// CheckBoxList1.da

			// HyperLink1.ImageUrl = "/images/back.gif";

			//PowerManager.Power.AddFunctionInfo("aa", "bb");

			#endregion
		}

		#region Web 窗体设计器生成的代码

		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.link.Click += new EventHandler(this.link_Click);
			this.Button2.Click += new EventHandler(this.Button2_Click);
			this.Button1.Click += new EventHandler(this.Button1_Click);
			this.Button3.Click += new EventHandler(this.Button3_Click);
			this.Load += new EventHandler(this.Page_Load);

		}

		#endregion

		private void link_Click(object sender, EventArgs e)
		{
			Server.Transfer("../page/list.aspx?templetid=" + this.EncryptTheQueryString("1"));
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			Response.Write(RadioButtonList1.SelectedIndex.ToString());
			Response.Write(RadioButtonList1.SelectedValue.ToString());

			Response.Write(RadioButtonList1.Items.Contains(new ListItem("下拉菜单", "0")).ToString());

		}

		private void Button2_Click(object sender, EventArgs e)
		{
			// string sql = "SELECT TOP 15 emp_no, dept_code, name, job FROM COMM_STAFF_DICT";
			// DataSet ds1 = OracleOledbBase.ExecuteDataSet(TempletBO.CONNECT_STRING, sql, null);

			// SqlDataAdapter sda = new SqlDataAdapter(sql, TempletBO.CONNECT_STRING);

			// Test1:
			// collectionTestClass cool = new collectionTestClass();

			// Test2:
			// CollectionTest2 cool = new CollectionTest2();

			// Test3:
			/*
			CollectionTest3 cool = new CollectionTest3();
			sda.Fill(cool);

			foreach (Staff staff in cool)
			{
				Response.Write("Empno:" + staff.EMPNO + "; Name:" + staff.Name + "; Job:" + staff.Job + "<br>");
			}
			*/

			TempletBO templet = new TempletBO(20);
			// FieldCollectionTable fields = templet.GetFieldsTable(0);
			FieldCollectionTable fields = templet.GetDateFields();
			DataGrid1.DataSource = fields;
			DataGrid1.DataBind();

			foreach (Field field in fields)
			{
				Response.Write("ID:" + field.ID + "; Name:" + field.FieldName + "; FieldTypeName:" + field.FieldTypeName + "; FieldTypeObj:" + field.FieldTypeObj.FieldTypeName + "<br>");
			}

		}

		private void Button3_Click(object sender, EventArgs e)
		{
			// string sql = "INSERT INTO TABLE2 (name, val) VALUES (N'阿嫂打发放阿嫂打发放', 11)";
			// GoldNet.JXKP.DAL.SqlServer.OracleOledbBase.ExecuteNonQuery(TempletBO.CONNECT_STRING, sql, null);

			// TempletBO templet = new TempletBO(20);
			// decimal x = templet.GetCountValue(26, 47, 19, new DateTime(2007, 11, 1), new DateTime(2007, 11, 30), "2251S02");
			// Response.Write(x.ToString());

			// Power.CheckPower()
		}
	} // class test


	public class collectionTestClass : DataTable, IEnumerable
	{
		public Staff this[int i]
		{
			get { return new Staff(this.Rows[i][0].ToString(), this.Rows[i][1].ToString(), this.Rows[i][2].ToString(), this.Rows[i][3].ToString()); }
		}

		public IEnumerator GetEnumerator()
		{
			// return this.Rows.GetEnumerator();
			return new StaffEnumerator(this);
		}

		private class StaffEnumerator : IEnumerator
		{
			private int _position = -1;
			private collectionTestClass _tab;

			public StaffEnumerator(collectionTestClass tab)
			{
				this._tab = tab;
			}

			#region IEnumerator 成员

			public void Reset()
			{
				_position = -1;
			}

			public object Current
			{
				get { return new Staff(_tab.Rows[_position][0].ToString(), _tab.Rows[_position][1].ToString(), _tab.Rows[_position][2].ToString(), _tab.Rows[_position][3].ToString()); }
			}

			public bool MoveNext()
			{
				if (_position < _tab.Rows.Count - 1)
				{
					_position++;
					return true;
				}
				else
				{
					return false;
				}
			}

			#endregion
		}
	}

	public class CollectionTest2 : DataTable, IEnumerable, IEnumerator
	{
		private int _position = -1;

		public Staff this[int i]
		{
			get { return new Staff(this.Rows[i][0].ToString(), this.Rows[i][1].ToString(), this.Rows[i][2].ToString(), this.Rows[i][3].ToString()); }
		}

		public IEnumerator GetEnumerator()
		{
			return this;
		}

		public new void Reset()
		{
			_position = -1;
		}

		public object Current
		{
			get { return this[_position]; }
		}

		public bool MoveNext()
		{
			if (_position < this.Rows.Count - 1)
			{
				_position++;
				return true;
			}
			else
			{
				return false;
			}
		}
	}


	public abstract class CollectBase : DataTable, IEnumerable, IEnumerator
	{
		protected int _position = -1;

		public new void Reset()
		{
			_position = -1;
		}

		public IEnumerator GetEnumerator()
		{
			return this;
		}

		public bool MoveNext()
		{
			if (_position < this.Rows.Count - 1)
			{
				_position++;
				return true;
			}
			else
			{
				return false;
			}
		}

		public abstract object this[int i] { get; }

		public object Current
		{
			get { return this[_position]; }
		}
	}


	public class CollectionTest3 : CollectBase
	{
		public override Object this[int i]
		{
			get { return new Staff(this.Rows[i][0].ToString(), this.Rows[i][1].ToString(), this.Rows[i][2].ToString(), this.Rows[i][3].ToString()); }
		}

	}

	public class Staff
	{
		private string eMPNO;

		public string EMPNO
		{
			get { return eMPNO; }
			set { eMPNO = value; }
		}

		private string deptCode;

		public string DeptCode
		{
			get { return deptCode; }
			set { deptCode = value; }
		}

		private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string job;

		public string Job
		{
			get { return job; }
			set { job = value; }
		}

		//public string 

		public Staff(string empno, string deptCode, string name, string job)
		{
			this.eMPNO = empno;
			this.deptCode = deptCode;
			this.name = name;
			this.job = job;
		}
	}


}
