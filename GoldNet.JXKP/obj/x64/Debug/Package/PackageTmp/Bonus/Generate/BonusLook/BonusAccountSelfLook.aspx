<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusAccountSelfLook.aspx.cs"
    Inherits="GoldNet.JXKP.BonusAccountSelfLook" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .pageSize
        {
          
            margin-left: 25px;
            
        }
        .captionStyle
        {
            margin-left: 25px;
         
            color: Blue;
        }
    </style>
     <link rel="stylesheet" type="text/css" href="../../Orthers/Cbouns.css" />
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="SBenefit" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="Type">
                    </ext:RecordField>
                    <ext:RecordField Name="Money">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SQuality" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="GUIDETYPE">
                    </ext:RecordField>
                    <ext:RecordField Name="WORTH">
                    </ext:RecordField>
                    <ext:RecordField Name="SCORE">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SSimpleAward" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="DEPT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="TYPE_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="MONEY">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SOtherAward" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="DEPT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="TYPE_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="MONEY">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
   
    <form id="form1" runat="server">
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="1">
                        <ext:Panel runat="server" ID="panel1" AutoScroll="true" Header="false" Border="false">
                            <TopBar>
                                <ext:Toolbar runat="server" ID="toolBar1">
                                    <Items>
                                        <ext:Button runat="server" ID="btn_Back" Text="返回" Icon="ReverseGreen">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Back">
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Body>
                                <div>
                                    <table width="95%" border="0">
                                        <tr>
                                            <td >                                                
                                                <ext:Label runat="server" ID="lTitle" StyleSpec="padding-left:25px;font-size:22px;font-weight:bold;" Text="科室奖金分项构成信息">
                                                </ext:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%" border="0" class="pageSize">
                                                 <tr>
                                                        <td valign="top">
                                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td valign="top" width="50%">
                                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <ext:GridPanel runat="server" ID="gpsimpleaward" StoreID="SSimpleAward" Height="210" Title="单项奖惩"
                                                                                        Width="350">
                                                                                        <Tools>
                                                                                            <ext:Tool Type="Search" Handler="Goldnet.AjaxMethods.Btn_SimleAward_Click();" Qtip="查看单项奖惩详细">
                                                                                            </ext:Tool>
                                                                                        </Tools>
                                                                                        <ColumnModel>
                                                                                            <Columns>
                                                                                                <ext:Column ColumnID="DEPT_NAME" Header="科室" Width="100" DataIndex="DEPT_NAME" MenuDisabled="true">
                                                                                                </ext:Column>
                                                                                                <ext:Column ColumnID="TYPE_NAME" Header="奖惩类型" Width="100" DataIndex="TYPE_NAME"
                                                                                                    MenuDisabled="true">
                                                                                                </ext:Column>
                                                                                                <ext:Column ColumnID="MONEY" Header="金额" Width="100" DataIndex="MONEY" MenuDisabled="true">
                                                                                                </ext:Column>
                                                                                            </Columns>
                                                                                        </ColumnModel>
                                                                                        <SelectionModel>
                                                                                            <ext:RowSelectionModel ID="RowSelectionModel2" SingleSelect="true">
                                                                                            </ext:RowSelectionModel>
                                                                                        </SelectionModel>
                                                                                    </ext:GridPanel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top">
                                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td valign="top" width="50%">
                                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <ext:GridPanel runat="server" ID="gportheraward" StoreID="SOtherAward" Height="210"
                                                                                        Title="其他奖惩" Width="350">
                                                                                        <Tools>
                                                                                            <ext:Tool Type="Search" Handler="Goldnet.AjaxMethods.Btn_OtherAward_Click();" Qtip="查看其他奖惩详细">
                                                                                            </ext:Tool>
                                                                                        </Tools>
                                                                                        <ColumnModel>
                                                                                            <Columns>
                                                                                                <ext:Column ColumnID="DEPT_NAME" Header="科室" Width="100" DataIndex="DEPT_NAME" MenuDisabled="true">
                                                                                                </ext:Column>
                                                                                                <ext:Column ColumnID="TYPE_NAME" Header="奖惩类型" Width="100" DataIndex="TYPE_NAME"
                                                                                                    MenuDisabled="true">
                                                                                                </ext:Column>
                                                                                                <ext:Column ColumnID="MONEY" Header="金额" Width="100" DataIndex="MONEY" MenuDisabled="true">
                                                                                                </ext:Column>
                                                                                            </Columns>
                                                                                        </ColumnModel>
                                                                                        <SelectionModel>
                                                                                            <ext:RowSelectionModel ID="RowSelectionModel3" SingleSelect="true">
                                                                                            </ext:RowSelectionModel>
                                                                                        </SelectionModel>
                                                                                    </ext:GridPanel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" width="50%">
                                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <ext:GridPanel Visible="false" runat="server" ID="gpIncome" StoreID="SBenefit" Height="210" Width="350" Title="科室效益">
                                                                            <Tools>
                                                                                <ext:Tool Type="Search" Handler="Goldnet.AjaxMethods.Btn_Income_Click();" Qtip="查看科室效益详细">
                                                                                </ext:Tool>
                                                                            </Tools>
                                                                            <ColumnModel>
                                                                                <Columns>
                                                                                    <ext:Column ColumnID="Type" Header="类型" Width="150" DataIndex="Type" MenuDisabled="true">
                                                                                    </ext:Column>
                                                                                    <ext:Column ColumnID="Money" Header="金额" Width="150" DataIndex="Money" MenuDisabled="true">
                                                                                    </ext:Column>
                                                                                </Columns>
                                                                            </ColumnModel>
                                                                            <SelectionModel>
                                                                                <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                                                                </ext:RowSelectionModel>
                                                                            </SelectionModel>
                                                                        </ext:GridPanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="65%">
                                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <ext:GridPanel runat="server" ID="gpzlgl" StoreID="SQuality" Height="210" Width="350" Title="质量考评">
                                                                            <Tools>
                                                                               <%-- <ext:Tool Hidden="true" Type="Search" Handler="Goldnet.AjaxMethods.Btn_Quality_Click();" Qtip="查看质量考评详细">
                                                                                </ext:Tool>--%>
                                                                            </Tools>
                                                                            <ColumnModel>
                                                                                <Columns>
                                                                                    <ext:Column ColumnID="GUIDETYPE" Header="考评类别" Width="100" DataIndex="GUIDETYPE"
                                                                                        MenuDisabled="true">
                                                                                    </ext:Column>
                                                                                    <ext:Column ColumnID="WORTH" Header="分值" Width="100" DataIndex="WORTH" MenuDisabled="true">
                                                                                    </ext:Column>
                                                                                    <ext:Column ColumnID="SCORE" Header="本月得分情况" Width="100" DataIndex="SCORE" MenuDisabled="true">
                                                                                    </ext:Column>
                                                                                </Columns>
                                                                            </ColumnModel>
                                                                            <SelectionModel>
                                                                                <ext:RowSelectionModel ID="RowSelectionModel1" SingleSelect="true">
                                                                                </ext:RowSelectionModel>
                                                                            </SelectionModel>
                                                                        </ext:GridPanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                   
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="IncomeWin" runat="server" Icon="Group" Title="查看效益详细数据" Width="619"
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="false" Resizable="true" 
        ShowOnLoad="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="SimpleWin" runat="server" Icon="Group" Title="查看单项奖惩详细" Width="619" Resizable="true" 
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false"  StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="OtherWin" runat="server" Icon="Group" Title="查看其他奖惩详细" Width="619"  Resizable="true" 
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
