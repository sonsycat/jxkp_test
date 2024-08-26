<%@ Page language="c#" Codebehind="test.aspx.cs" AutoEventWireup="false" Inherits="GoldNet.JXKP.Templet.Config.test" %>
<%@ Register TagPrefix="uc1" TagName="title" Src="title.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<uc1:title id="Title1" runat="server"></uc1:title>
	<body MS_POSITIONING="GridLayout">
		<TABLE height="712" cellSpacing="0" cellPadding="0" width="474" border="0" ms_2d_layout="TRUE">
			<TR vAlign="top">
				<TD width="474" height="712">
					<form id="Form1" method="post" runat="server">
						<TABLE height="457" cellSpacing="0" cellPadding="0" width="700" border="0" ms_2d_layout="TRUE">
							<TR vAlign="top">
								<TD width="48" height="24"></TD>
								<TD width="56"></TD>
								<TD width="48"></TD>
								<TD width="48"></TD>
								<TD width="64"></TD>
								<TD width="8"></TD>
								<TD width="88"></TD>
								<TD width="160"></TD>
								<TD width="8"></TD>
								<TD width="172"></TD>
							</TR>
							<TR vAlign="top">
								<TD height="48"></TD>
								<TD colSpan="9">
									<asp:LinkButton id="link" runat="server">模板</asp:LinkButton></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="5" height="88"></TD>
								<TD colSpan="2">
									<asp:RadioButtonList id="RadioButtonList2" runat="server" RepeatColumns="4"></asp:RadioButtonList></TD>
								<TD colSpan="3" rowSpan="2"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="2" height="48"></TD>
								<TD colSpan="5" rowSpan="2">
									<asp:RadioButtonList id="RadioButtonList1" runat="server" Width="240px" Height="72px">
										<asp:ListItem Value="0">下拉菜单</asp:ListItem>
										<asp:ListItem Value="1">单选按钮</asp:ListItem>
										<asp:ListItem Value="2">复选框(允许多重选择) </asp:ListItem>
									</asp:RadioButtonList></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="2" height="48"></TD>
								<TD colSpan="2"></TD>
								<TD>
									<asp:Button id="Button2" runat="server" Text="Button"></asp:Button></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="8" height="16"></TD>
								<TD colSpan="2" rowSpan="3">
									<asp:DataGrid id="DataGrid1" runat="server"></asp:DataGrid></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="3" height="48"></TD>
								<TD colSpan="2">
									<asp:Button id="Button1" runat="server" Text="Button"></asp:Button></TD>
								<TD colSpan="3"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="4" height="112"></TD>
								<TD colSpan="3">
									<asp:TextBox id="TextBox1" runat="server"></asp:TextBox></TD>
								<TD>
									<asp:CompareValidator id="CompareValidator1" runat="server" ErrorMessage="CompareValidator" Type="Date"
										Operator="DataTypeCheck" ControlToValidate="TextBox1"></asp:CompareValidator></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="6" height="25"></TD>
								<TD colSpan="4">
									<asp:Button id="Button3" runat="server" Text="Button"></asp:Button></TD>
							</TR>
						</TABLE>
					</form>
				</TD>
			</TR>
		</TABLE>
	</body>
</HTML>
