<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BounsGuideSQLExpress.aspx.cs" Inherits="GoldNet.JXKP.BounsGuideSQLExpress" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"  runat="server">
    <title>指标算法信息</title>
    <link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <style type="text/css">
        body{
            background-color: #DFE8F6;
        }
    </style>
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server"/>
    <form id="form1" runat="server" >
    <ext:ViewPort ID="ViewPort1" runat="server">
    <Body>
    <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
    <Columns>
    <ext:LayoutColumn ColumnWidth="1">

    <ext:Panel ID="Panel1" runat="server" Border="false"  BodyStyle="background-color:transparent" >
         <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                <ext:Label ID="Label7" runat="server" Text="指标数据 从月份:"></ext:Label>
                <ext:DateField runat="server" ID="dd1" Vtype="daterange"   AllowBlank="false" Format="Ym"  MaxLength="6" Width="70">
                </ext:DateField>
                <ext:Label ID="Label6" runat="server" Text=" 至:"></ext:Label>
                <ext:DateField runat="server" ID="dd2" Vtype="daterange"   AllowBlank="false"  Format="Ym"  MaxLength="6" Width="70">
                    <Listeners>
                        <Render Handler="this.startDateField = '#{dd1}'" />
                    </Listeners> 
                </ext:DateField>
                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></ext:ToolbarSeparator>            
                <ext:Button ID="BtnCreat" runat="server" Text="数据生成" Icon="ArrowInout">
                    <AjaxEvents>
                        <Click   OnEvent="BtnCreat_Click" Timeout="1200000">
                            <EventMask ShowMask="true" Msg="请稍候,正在生成数据..." />
                        </Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server"></ext:ToolbarSeparator>
                <ext:Button ID="BtnPreview" runat="server" Text="预览" Icon="Zoom" Disabled="true">
                    <AjaxEvents>
                        <Click   OnEvent="BtnPreview_Click" Timeout="120000">
                            <EventMask ShowMask="true" Msg="请稍候..." />
                        </Click>
                    </AjaxEvents>
                </ext:Button>

                <ext:ToolbarSeparator ID="ToolbarSeparator22" runat="server"></ext:ToolbarSeparator>
                    <ext:Button ID="BtnSave" runat="server" Text="保存" Icon="Disk">
                    <AjaxEvents>
                        <Click   OnEvent="BtnSave_Click" Timeout="20000000">
                            <EventMask ShowMask="true" Msg="请稍候..." />
                        </Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                <ext:Button ID="CancelButton" runat="server" Text="返回" Icon="ArrowUndo">
                    <Listeners>
                        <Click Handler="parent.DetailWin.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server"></ext:ToolbarSeparator>
                </Items>
            </ext:Toolbar>
            </TopBar>
        <Body>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false"  AutoScroll="true"  Height="510"  Width="530" ButtonAlign="Right" BodyStyle="background-color:transparent"  >
            <Body>
            
                <ext:FieldSet ID="fieldset1" runat="server" Title="指标基本信息"  Collapsible="true" Collapsed="false" StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                    <Body>
                        <table width="95%">
                            <tr>
                                <td style="width: 20%;"> 指标代码:</td>
                                <td>
                                <table cellpadding="0" cellspacing ="0">
                                    <tr>
                                        <td><ext:TextField  ID="GuideCodeTxt" runat="server" ReadOnly="true"  Width="150"  /></td>
                                        <td style="padding-left:2px; padding-bottom:2px;" ><ext:ComboBox ID="OrganComb" runat="server"  Width="40" ListWidth="50" Disabled="true" /></td>
                                    </tr>
                                </table>
                                </td>
                            </tr>
                            <tr>
                                <td>  指标名称:</td>
                                <td>
                                    <ext:TextField   ID="GuideNameTxt" runat="server"  Width="200"  ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </Body>
                </ext:FieldSet>
                <ext:FieldSet ID="fieldset5" runat="server"   Title="单月份指标SQL"  Collapsible="true" Collapsed="false" StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                    <Body>
                          <ext:TextArea ID="GuideSQLTxt" runat="server"  Width="482"  MaxLength ="3800" Height="120" StyleSpec="margin:2px" />
                          <ext:Resizable ID="Resizable1" runat="server" Element="GuideSQLTxt" Handles="South" Wrap="true" Pinned="true" Width="482" Height="120" MinWidth="200" MinHeight="60" Dynamic="true" />
                    </Body>
                </ext:FieldSet>                
                <ext:FieldSet ID="fieldset2" runat="server"   Title="统计区间指标SQL"  Collapsible="true" Collapsed="false" StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                    <Body>
                          <ext:TextArea ID="GuideSQLSumTxt" runat="server"  Width="482"   MaxLength ="3800" Height="120" StyleSpec="margin:2px" />
                          <ext:Resizable ID="Resizable2" runat="server" Element="GuideSQLSumTxt" Handles="South" Wrap="true" Pinned="true" Width="482" Height="120" MinWidth="200" MinHeight="60" Dynamic="true" />
                    </Body>
                </ext:FieldSet>
                <ext:FieldSet ID="fieldset3" runat="server"   Title="指标明细数据SQL"  Collapsible="true" Collapsed="true" StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                    <Body>
                          <ext:TextArea ID="GuideSQLDetailTxt" runat="server"  Width="482" MaxLength ="3800"  Height="120" StyleSpec="margin:2px" />
                          <ext:Resizable ID="Resizable3" runat="server" Element="GuideSQLDetailTxt" Handles="South" Wrap="true" Pinned="true" Width="482" Height="120" MinWidth="200" MinHeight="60" Dynamic="true" />
                    </Body>
                </ext:FieldSet>
            </Body>
       </ext:FormPanel>
       </Body>
    </ext:Panel>
    </ext:LayoutColumn>
    </Columns>
    </ext:ColumnLayout>
    </Body>
    </ext:ViewPort>
    
    <ext:Store runat="server" ID="Store1"  AutoLoad="true" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name ="TJYF" />
                    <ext:RecordField Name ="UNIT_CODE"/>
                    <ext:RecordField Name ="GUIDE_CODE" />
                    <ext:RecordField Name ="GUIDE_VALUE" />
                    <ext:RecordField Name ="GUIDE_TYPE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Window ID="GridWin" runat="server" Icon="Zoom" Title="数据预览" Width="460" Closable="true"
        CloseAction="Hide" Maximizable="true" Height="420" AutoShow="false" Modal="true" Resizable="false"
        AutoScroll="true" CenterOnLoad="true" ShowOnLoad="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
        <Body>
            <ext:FitLayout ID="FitLayout1" runat="server">
                <ext:GridPanel ID="GridPanel1" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                    Height="480" AutoWidth="true">
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn Width="32">
                            </ext:RowNumbererColumn>
                            <ext:Column Header="统计月份" MenuDisabled="true" Width="70" ColumnID="TJYF" DataIndex="TJYF" />
                            <ext:Column Header="对象代码" MenuDisabled="true" Width="70" ColumnID="UNIT_CODE" DataIndex="UNIT_CODE" />
                            <ext:Column Header="指标代码" MenuDisabled="true" Width="100" ColumnID="GUIDE_CODE" DataIndex="GUIDE_CODE" />
                            <ext:Column Header="指标值" MenuDisabled="true" Width="70" ColumnID="GUIDE_VALUE" DataIndex="GUIDE_VALUE" />
                            <ext:Column Header="指标类型" MenuDisabled="true" Width="70" ColumnID="GUIDE_TYPE" DataIndex="GUIDE_TYPE" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="rowselection" SingleSelect="true" />
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="15" StoreID="Store1" AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                        </ext:PagingToolbar>
                    </BottomBar>
                    <LoadMask ShowMask="true" />
                </ext:GridPanel>
            </ext:FitLayout>
        </Body>
    </ext:Window>
    </form>
</body>
</html>
