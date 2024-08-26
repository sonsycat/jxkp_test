<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnGuardDaysDept.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoManager.OnGuardDaysDept" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <style type="text/css">
        h2
        {
            font-size: 24px;
            letter-spacing: 1px;
            margin: 10px 0 20px;
            padding: 0;
        }
        .search-item
        {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
            width: 200px;
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
            width: 140px;
            display: block;
            clear: none;
        }
    </style>

    <script type="text/javascript">        
     var RowIndex;

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="STAFF_ID">
                <Fields>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="CASES" />
                    <ext:RecordField Name="USER_NAME" />
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="STRAT_DATE" />
                    <ext:RecordField Name="END_DATE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="PERSON" />
                    <ext:RecordField Name="PLACE" />
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
    <ext:Store ID="SYear" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SMonth" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="MONTH">
                <Fields>
                    <ext:RecordField Name="MONTH" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout3" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel_Show" runat="server" Border="false" StoreID="Store1"
                                StripeRows="true" TrackMouseOver="true" Height="480" ClicksToEdit="1">
                                <TopBar>
                                    <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0">
                                        <Items>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                            <ext:ComboBox ID="NumYear" runat="server" ReadOnly="true" StoreID="SYear" Width="60"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                            <ext:ComboBox ID="Comb_StartMonth" runat="server" ReadOnly="true" StoreID="SMonth"
                                                Width="40" DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月  -  " />
                                            <ext:ComboBox ID="EndYear" runat="server" ReadOnly="true" StoreID="SYear" Width="60"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem3" runat="server" Text="年" />
                                            <ext:ComboBox ID="EndMonth" runat="server" ReadOnly="true" Width="40" StoreID="SMonth"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="月" />
                                            <ext:Label ID="Label1" runat="server" Text="人员类别：">
                                            </ext:Label>
                                            <ext:ComboBox ID="cboPersonType" runat="server" Editable="false" Width="80">
                                            </ext:ComboBox>
                                            <ext:Label ID="Label3" runat="server" Text="科室类别：" />
                                            <ext:ComboBox ID="cbbdept" runat="server" Width="150" ListWidth="150" SelectedIndex="1">
                                            </ext:ComboBox>
                                            <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Query_Click" Timeout="99999999">
                                                        <EventMask Msg="查询中......" ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                Text="导出Excel" Icon="PageWhiteExcel" CausesValidation="false">
                                            </ext:Button>
                                            <ext:ToolbarSeparator>
                                            </ext:ToolbarSeparator>
                                            <ext:Button ID="btnDetail" runat="server" Text="查询月明细" Icon="Picture" Hidden="true" >
                                                <AjaxEvents>
                                                    <Click OnEvent="SearchDetail" Success="#{ViewDetail}.show();">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="name" Value="Ext.encode(#{Store1}.getAt(RowIndex).get('NAME'))"
                                                                Mode="Raw">
                                                            </ext:Parameter>
                                                            <ext:Parameter Name="deptCode" Value="Ext.encode(#{Store1}.getAt(RowIndex).get('DEPT_CODE'))"
                                                                Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <%--                                        <Listeners>
                                            <RowSelect Handler="#{btnDetail}.enable();RowIndex = rowIndex;" />
                                            <RowDeselect Handler="if (!#{GridPanel_Show}.hasSelection()) {#{btnDetail}.disable();RowIndex = -1;}" />
                                        </Listeners>--%>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="ViewDetail" runat="server" Icon="Group" Title="查看明细" Width="750"
        Height="350" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false">
        <Body>
            <ext:Panel ID="Panel2" runat="server">
                <Body>
                    <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true" FitHeight="true">
                        <Columns>
                            <ext:LayoutColumn ColumnWidth="1">
                                <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store" Border="false" Width="700"
                                    Header="false" AutoScroll="true" BodyStyle="background-color:Transparent;" Height="315">
                                    <ColumnModel ID="ColumnModel2" runat="server" >
                                        <Columns>
                                            <ext:Column ColumnID="DEPT_NAME" Header="科室" Sortable="true" DataIndex="DEPT_NAME" />
                                            <ext:Column ColumnID="USER_NAME" Header="姓名" Sortable="true" DataIndex="USER_NAME" />
                                            <ext:Column ColumnID="CASES" Header="请假类别" Sortable="true" DataIndex="CASES" />
                                            <ext:Column ColumnID="STRAT_DATE" Header="请假日期" Sortable="true" DataIndex="STRAT_DATE" />
                                            <ext:Column ColumnID="END_DATE" Header="销假日期" Sortable="true" DataIndex="END_DATE" />
                                            <ext:Column ColumnID="PERSON" Header="批准人" Sortable="true" DataIndex="PERSON" />
                                            <ext:Column ColumnID="PLACE" Header="备注" Sortable="true" DataIndex="PLACE" />
                                        </Columns>
                                    </ColumnModel>
                                    <LoadMask ShowMask="true" />
                                </ext:GridPanel>
                            </ext:LayoutColumn>
                        </Columns>
                    </ext:ColumnLayout>
                </Body>
            </ext:Panel>
        </Body>
    </ext:Window>
    </form>
</body>
</html>
