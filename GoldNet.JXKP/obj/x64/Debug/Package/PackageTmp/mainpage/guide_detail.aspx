<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="guide_detail.aspx.cs"   Inherits="GoldNet.JXKP.mainpage.guide_detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>指标明细</title>
    <script type="text/javascript"  src="/resources/FusionCharts.js"></script>      
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
        <tr>
            <td><asp:Literal ID="FCLiteral" runat="server"></asp:Literal></td>
        </tr>
        <tr>
            <td align="center">
                <asp:DropDownList ID="App" runat="server" Width="104px" AutoPostBack=true>
                                       <asp:ListItem Value="Pie3D">饼状图</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>