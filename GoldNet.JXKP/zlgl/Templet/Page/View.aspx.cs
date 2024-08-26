using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GoldNet.Comm;
using GoldNet.JXKP.Templet.BLL;
namespace GoldNet.JXKP.Templet.Page
{
	/// <summary>
	/// �鿴ҳ��
	/// </summary>
	public class View : PageBase
	{
		protected System.Web.UI.WebControls.Label labTitle;
		protected System.Web.UI.WebControls.Label labCommon;
		protected System.Web.UI.HtmlControls.HtmlTable tabInput;
		private int _templetID, _recID;
		private TempletBO _templet;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// ��ʼ��ģ��
			_templetID = int.Parse(Request["templetid"].ToString());
			_recID = int.Parse(Request["recid"].ToString());
			_templet = new TempletBO(_templetID);

			if (!this.IsPostBack)
			{
				// ��ʼ��ҳ��
				this.labTitle.Text = _templet.Title;
				this.labCommon.Text = _templet.GetModifyInfo(_recID).Replace("\n", "<br>");
				HtmlTableCell cellFieldName, cellFieldInput;
				HtmlTableRow rowInput;
				foreach (Field field in _templet.GetFields(_templetID))
				{
					cellFieldName = new HtmlTableCell();
					cellFieldInput = new HtmlTableCell();
                    rowInput = new HtmlTableRow();
					cellFieldName.Controls.Add(new LiteralControl(field.FieldName + ":"));
					cellFieldName.VAlign = "top";
					cellFieldName.Width = "24%";
					cellFieldName.Attributes["class"] = "gs-input-desc";
					cellFieldInput.Attributes["class"] = "gs-input-section";
					cellFieldInput.Width = "76%";
					field.ShowViewData(_templet.TabName, _recID, cellFieldInput);
					rowInput.Cells.Add(cellFieldName);
					rowInput.Cells.Add(cellFieldInput);
					((HtmlTable)this.Page.FindControl("tabInput")).Rows.Add(rowInput);
				}
			}
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
