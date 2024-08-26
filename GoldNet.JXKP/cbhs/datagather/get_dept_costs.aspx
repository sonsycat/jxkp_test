<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="get_dept_costs.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.datagather.get_dept_costs" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
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
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        var rmbMoney = function(v) {
               if(v==null||v=="")
               {
               return "";
               }
               v = (Math.round((v - 0) * 100)) / 100;
               v = (v == Math.floor(v)) ? v + ".00" : ((v * 10 == Math.floor(v * 10)) ? v + "0" : v);
               v = String(v);
               var ps = v.split('.');
               var whole = ps[0];
               var sub = ps[1] ? '.' + ps[1] : '.00';
               var r = /(\d+)(\d{3})/;
               while (r.test(whole)) {
                   whole = whole.replace(r, '$1' + ',' + '$2');
               }
               v = whole + sub;
               if (v.charAt(0) == '-') {
                   return '-' + v.substr(1);
               }
               return v;
       }
    </script>

    <style type="text/css">
        .x-grid3-cell-inner
        {
            border-right: 1px solid #eceff6;
        }
    </style>
    <link href="../../resources/css/examples.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server" OnSubmitData="SubmitData" OnRefreshData="Data_RefreshData"
        WarningOnDirty="false">
        <Reader>
            <ext:JsonReader ReaderID="ITEM_CODE">
                <Fields>
                    <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                    <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                    <ext:RecordField Name="TOTAL_COSTS" Type="Float" Mapping="TOTAL_COSTS" />
                    <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
                    <ext:RecordField Name="COSTS_ARMYFREE" Type="Float" Mapping="COSTS_ARMYFREE" />
                    <ext:RecordField Name="MEMO" Type="String" Mapping="MEMO" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" AutoLoad="true">
        <Proxy>
        </Proxy>
        <Reader>
            <ext:JsonReader Root="deptlist" TotalProperty="totalCount">
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" WarningOnDirty="false">
        <Reader>
            <ext:JsonReader ReaderID="ITEM_CODE">
                <Fields>
                    <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                    <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                    <ext:RecordField Name="FLAGS" Type="String" Mapping="FLAGS" />
                    <ext:RecordField Name="SUBMIT_PERSONS" Type="String" Mapping="SUBMIT_PERSONS" />
                    <ext:RecordField Name="CHECK_FLAGS" Type="String" Mapping="CHECK_FLAGS" />
                    <ext:RecordField Name="CHECK_NAME" Type="String" Mapping="CHECK_NAME" />
                    <ext:RecordField Name="COMP_FLAGS" Type="String" Mapping="COMP_FLAGS" />
                    <ext:RecordField Name="COMP_NAME" Type="String" Mapping="COMP_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <ext:Panel ID="Panel2" runat="server" BodyBorder="true" Border="false">
                        <Body>
                            <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                                <Columns>
                                    <ext:LayoutColumn ColumnWidth="1">
                                        <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" BodyStyle="color:black"
                                            ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="true"
                                            BodyBorder="true" StripeRows="false">
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                                    <Items>
                                                        <ext:ComboBox ID="years" runat="server" Width="60" AllowBlank="true" EmptyText="请选择年..."
                                                            FieldLabel="年">
                                                            <AjaxEvents>
                                                                <Select OnEvent="Date_SelectOnChange">
                                                                    <EventMask Msg='载入中...' ShowMask="true" />
                                                                </Select>
                                                            </AjaxEvents>
                                                        </ext:ComboBox>
                                                        <ext:ToolbarTextItem ID="dd1Name" runat="server" Text="年 " />
                                                        <ext:ComboBox ID="months" runat="server" Width="60" AllowBlank="true" EmptyText="请选择月..."
                                                            FieldLabel="月">
                                                            <Items>
                                                                <ext:ListItem Text="01" Value="01" />
                                                                <ext:ListItem Text="02" Value="02" />
                                                                <ext:ListItem Text="03" Value="03" />
                                                                <ext:ListItem Text="04" Value="04" />
                                                                <ext:ListItem Text="05" Value="05" />
                                                                <ext:ListItem Text="06" Value="06" />
                                                                <ext:ListItem Text="07" Value="07" />
                                                                <ext:ListItem Text="08" Value="08" />
                                                                <ext:ListItem Text="09" Value="09" />
                                                                <ext:ListItem Text="10" Value="10" />
                                                                <ext:ListItem Text="11" Value="11" />
                                                                <ext:ListItem Text="12" Value="12" />
                                                            </Items>
                                                            <AjaxEvents>
                                                                <Select OnEvent="Date_SelectOnChange">
                                                                    <EventMask Msg='载入中...' ShowMask="true" />
                                                                </Select>
                                                            </AjaxEvents>
                                                        </ext:ComboBox>
                                                        
                                                        <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月 " />
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                        <ext:Label ID="Label4" runat="server" Text="科室：" Width="80" />
                                                        <ext:ComboBox ID="DEPT" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                                                            ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="100"
                                                            ListWidth="250" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                            <Template ID="Template1" runat="server">
                                                            <tpl for=".">
                                                                <div class="search-item">
                                                                    <h3><span>{DEPT_NAME}</span>{DEPT_CODE}</h3>
                                                                </div>
                                                            </tpl>
                                                            </Template>
                                                        </ext:ComboBox>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                                        <ext:Button ID="Button_look" runat="server" Text="查询" Icon="DatabaseGo">
                                                            <AjaxEvents>
                                                                <Click OnEvent="Button_look_click">
                                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                                </Click>
                                                            </AjaxEvents>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                                        <ext:Button ID="Button_del" runat="server" Text="删除" Icon="Delete" Disabled="true">
                                                            <AjaxEvents>
                                                                <Click OnEvent="Button_del_click">
                                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="将删除选中数据,<br/>是否继续?" />
                                                                    <ExtraParams>
                                                                    </ExtraParams>
                                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                                        </ext:Parameter>
                                                                    </ExtraParams>
                                                                </Click>
                                                            </AjaxEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="costdetail" runat="server" Text="详细信息" Icon="Disk">
                                                            <AjaxEvents>
                                                                <Click OnEvent="DbRowClick">
                                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                                        </ext:Parameter>
                                                                    </ExtraParams>
                                                                </Click>
                                                            </AjaxEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="Button_save" runat="server" Text="保存" Icon="Disk" Visible="false">
                                                            <Listeners>
                                                                <Click Handler="#{GridPanel1}.submitData();" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button ID="costs" runat="server" Text="成本提取（1）" Icon="Disk" Visible="false">
                                                            <AjaxEvents>
                                                                <Click OnEvent="costs_click">
                                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                                </Click>
                                                            </AjaxEvents>
                                                        </ext:Button>
                                                        <%-- <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                                        <ext:Label ID="Label3" runat="server" Text="预提次数:" Width="40" />
                                                        <ext:ComboBox ID="Opno" runat="server" AutoDataBind="true" Width="60" AllowBlank="true" EmptyText="0" Enabled="false">
                                                            <AjaxEvents>
                                                                <Select OnEvent="Opno_SelectOnChange">
                                                                    <EventMask Msg='载入中...' ShowMask="true" />
                                                                </Select>
                                                            </AjaxEvents>
                                                        </ext:ComboBox>--%>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server" />
                                                        <ext:Button ID="Button_get_due" runat="server" Text="成本提取" Icon="Disk">
                                                            <AjaxEvents>
                                                                <Click OnEvent="Button_get_due_click" Timeout="99999999">
                                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                                </Click>
                                                            </AjaxEvents>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                                        <ext:Button ID="Button2" runat="server" Text="SQL成本提取" Icon="Disk" Hidden="true">
                                                            <AjaxEvents>
                                                                <Click OnEvent="SQL_get_due_click">
                                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                                </Click>
                                                            </AjaxEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                            Text="导出Excel" Icon="PageWhiteExcel">
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server" />
                                                        <%-- <ext:Button ID="Button_get" runat="server" Text="成本分解" Icon="ArrowOut">
                                                            <AjaxEvents>
                                                                <Click OnEvent="costs_click" Timeout="99999999">
                                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                                </Click>
                                                            </AjaxEvents>
                                                        </ext:Button>--%>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <ColumnModel ID="ColumnModel1" runat="server">
                                                <Columns>
                                                    <ext:Column ColumnID="ITEM_CODE" Hidden="true" />
                                                    <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>成本项目</div>"
                                                        Width="130" Align="left" Sortable="false" DataIndex="ITEM_NAME" MenuDisabled="true" />
                                                    <ext:Column ColumnID="TOTAL_COSTS" Header="<div style='text-align:center;'>成本额</div>"
                                                        Width="130" Align="Right" Sortable="false" DataIndex="TOTAL_COSTS" MenuDisabled="true">
                                                        <Renderer Fn="rmbMoney" />
                                                    </ext:Column>
                                                    <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>实际成本</div>"
                                                        Width="130" Align="Right" Sortable="false" DataIndex="COSTS" MenuDisabled="true">
                                                        <Renderer Fn="rmbMoney" />
                                                    </ext:Column>
                                                    <ext:Column ColumnID="COSTS_ARMYFREE" Header="<div style='text-align:center;'>减免成本</div>"
                                                        Width="130" Align="Right" Sortable="false" DataIndex="COSTS_ARMYFREE" MenuDisabled="true">
                                                        <Renderer Fn="rmbMoney" />
                                                    </ext:Column>
                                                    <ext:Column ColumnID="MEMO" Header="<div style='text-align:center;'>备注</div>" Width="150"
                                                        Align="left" Sortable="false" DataIndex="MEMO" MenuDisabled="true">
                                                        <Editor>
                                                            <ext:TextField ID="TextField2" runat="server" />
                                                        </Editor>
                                                    </ext:Column>
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:CheckboxSelectionModel ID="RowSelectionModel1" runat="server">
                                                    <Listeners>
                                                        <SelectionChange Handler="#{GridPanel1}.hasSelection()? #{Button_del}.setDisabled(false): #{Button_del}.setDisabled(true);" />
                                                    </Listeners>
                                                </ext:CheckboxSelectionModel>
                                            </SelectionModel>
                                        </ext:GridPanel>
                                    </ext:LayoutColumn>
                                </Columns>
                            </ext:ColumnLayout>
                        </Body>
                    </ext:Panel>
                </Center>
                <East MinWidth="200" MaxWidth="400" SplitTip="成本提交状态信息" Collapsible="true" Split="true">
                    <ext:Panel ID="Panel1" runat="server" Border="false" Width="350" Title="成本提交状态信息"
                        Collapsed="true" AutoScroll="true" TitleCollapse="True">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:Button ID="Button_ok" runat="server" Text="提交" Icon="Disk">
                                        <AjaxEvents>
                                            <Click OnEvent="Button_ok_click">
                                                <Confirmation ConfirmRequest="true" Title="系统提示" Message="将提交选中的成本,<br/>是否继续?" />
                                                <ExtraParams>
                                                </ExtraParams>
                                                <EventMask Msg="载入中..." ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </AjaxEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator ID="ToolbarSeparator8" runat="server" />
                                    <ext:Button ID="Button1" runat="server" Text="审核" Icon="Disk">
                                        <AjaxEvents>
                                            <Click OnEvent="Button_sh_click">
                                                <Confirmation ConfirmRequest="true" Title="系统提示" Message="将审核选中的成本,<br/>是否继续?" />
                                                <ExtraParams>
                                                </ExtraParams>
                                                <EventMask Msg="载入中..." ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </AjaxEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator ID="ToolbarSeparator9" runat="server" />
                                    <ext:Button ID="Buttonfh" runat="server" Text="复核" Icon="Disk">
                                        <AjaxEvents>
                                            <Click OnEvent="Button_fh_click">
                                                <Confirmation ConfirmRequest="true" Title="系统提示" Message="将复合选中的成本,<br/>是否继续?" />
                                                <ExtraParams>
                                                </ExtraParams>
                                                <EventMask Msg="载入中..." ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </AjaxEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server" />
                                    <ext:Button ID="Button_no" runat="server" Text="取消" Icon="Disk">
                                        <AjaxEvents>
                                            <Click OnEvent="Button_no_click">
                                                <Confirmation ConfirmRequest="true" Title="系统提示" Message="将取消选中的成本,<br/>是否继续?" />
                                                <ExtraParams>
                                                </ExtraParams>
                                                <EventMask Msg="载入中..." ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </AjaxEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Body>
                            <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true">
                                <Columns>
                                    <ext:LayoutColumn ColumnWidth="1">
                                        <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store3" StripeRows="true"
                                            AutoHeight="true" AutoWidth="true" TrackMouseOver="true" AutoScroll="true">
                                            <ColumnModel ID="ColumnModel2" runat="server">
                                                <Columns>
                                                    <ext:Column ColumnID="ITEM_CODE" Hidden="true" />
                                                    <ext:Column Header="成本项目" Width="100" ColumnID="ITEM_NAME" DataIndex="ITEM_NAME"
                                                        Sortable="false" MenuDisabled="true" />
                                                    <ext:Column Header="状态" Width="60" ColumnID="FLAGS" DataIndex="FLAGS" Sortable="false"
                                                        MenuDisabled="true" />
                                                    <ext:Column Header="提交人" Width="60" ColumnID="SUBMIT_PERSONS" DataIndex="SUBMIT_PERSONS"
                                                        Sortable="false" MenuDisabled="true" />
                                                    <ext:Column Header="审核人" Width="60" ColumnID="CHECK_NAME" DataIndex="CHECK_NAME"
                                                        Sortable="false" MenuDisabled="true" />
                                                    <ext:Column Header="复核人" Width="60" ColumnID="COMP_NAME" DataIndex="COMP_NAME" Sortable="false"
                                                        MenuDisabled="true" />
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:CheckboxSelectionModel ID="RowSelectionModel2" runat="server">
                                                </ext:CheckboxSelectionModel>
                                            </SelectionModel>
                                            <Listeners>
                                            </Listeners>
                                            <LoadMask ShowMask="true" />
                                        </ext:GridPanel>
                                    </ext:LayoutColumn>
                                </Columns>
                            </ext:ColumnLayout>
                        </Body>
                    </ext:Panel>
                </East>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="Cost_Detail" runat="server" Icon="Group" Title="核算项目" Width="850"
        Height="500" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
