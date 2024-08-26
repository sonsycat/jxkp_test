<%@ Page language="c#" Codebehind="View.aspx.cs" AutoEventWireup="false" Inherits="GoldNet.JXKP.Templet.Page.View" %>
<%@ Register TagPrefix="uc1" TagName="title" Src="title.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<uc1:title id="Title1" runat="server"></uc1:title>
	<body class="gs-mainbody">
		<form id="Form1" method="post" runat="server">
			<table class="gs-maintab" cellSpacing="0" cellPadding="0" width="100%" border="0">
				
				<tr>
					<td>
						<table class="gs-input-tab" cellspacing="0" cellpadding="0" border="0" width="100%">
							<tr>
								<td id="ContentTD" valign="top">
									<table id="tabInput" runat="server" border="0" style="WIDTH:100%" valign="top">
										<tr>
											<td class="gs-input-title" colspan="2"><asp:Label id="labTitle" runat="server"></asp:Label></td>
										</tr>
										
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="gs-input-separator2"></td>
				</tr>
				<tr>
					<td height="4"></td>
				</tr>
				<tr>
					<td><asp:Label id="labCommon" runat="server" CssClass="gs-input-desc"></asp:Label></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
