<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideHelp.aspx.cs" Inherits="GoldNet.JXKP.zlgl.Help.GuideHelp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
<link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
        .search-item
        {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }
        .search-item h3
        {
            display: block;
            font: inherit;
            font-weight: bold;
            color: #222;
        }
        .search-item h3 span
        {
            float: right;
            font-weight: normal;
            margin: 0 0 5px 5px;
            width: 100px;
            display: block;
            clear: none;
        }
        p
        {
            width: 650px;
        }
        .ext-ie .x-form-text
        {
            position: static !important;
        }
    </style>
</head>
<body>
		<SCRIPT language="JavaScript">
		<!--
		function ShowHideGroup(group, img)
		{
			if ((group == null))
			{
				return;
			}
			if (group.style.display != "none")
			{
				group.style.display = "none";
				img.src = "images/PLUS.GIF";
			}
			else
			{
				group.style.display = "inline";
				img.src = "images/minus.gif";
			}
		}
		//-->
		</SCRIPT>
		<form id="Form1" method="post" runat="server">
			<script language="javascript" src="/aspnet_client/system_web/1_1_4322/WebUIValidation.js"></script>
			<TABLE class="gs-maintab" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD>
						<table class="gs-input-tab" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td class="gs-input-desc"><FONT color="#ff0000">*</FONT> 单击 <IMG alt="" src="images/PLUS.GIF" border="0">
									可转为展开模式,再次单击 <IMG alt="" src="images/minus.gif" border="0"> 可返回折叠模式</td>
							</tr>
							<tr>
								<td class="gs-input-separator"></td>
							</tr>
							<tr>
								<td>
									<div id="display" align="left" runat="server"></div>
								</td>
							</tr>
						</table>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</html>

