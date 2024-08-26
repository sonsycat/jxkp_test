<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptBenefit.aspx.cs" Inherits="GoldNet.JXKP.GuideLook.Statement.DeptBenefit" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <style type="text/css">
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
        h2
        {
            font-size: 24px;
            letter-spacing: 1px;
            margin: 10px 0 20px;
            padding: 0;
        }
    </style>

    <script type="text/javascript">
           var ViewDeptBenefitInfo = function(grid,rowIndex,colIndex) {
                ColIndex.value = colIndex;
           }
        var viewDetail = function(url) {
            arcEditDetailWindow.show();
            arcEditDetailWindow.clearContent();
            arcEditDetailWindow.loadContent({
            url: url,
                mode: "iframe",
                showMask: true,
                maskMsg: "载入中..."
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Hidden ID="ColIndex" runat="server">
    </ext:Hidden>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="MZJM" SortType="AsFloat" />
                    <ext:RecordField Name="MZDF" SortType="AsFloat" />
                    <ext:RecordField Name="MZXJ" SortType="AsFloat" />
                    <ext:RecordField Name="ZYJM" SortType="AsFloat" />
                    <ext:RecordField Name="ZYDF" SortType="AsFloat" />
                    <ext:RecordField Name="ZYXJ" SortType="AsFloat" />
                    <ext:RecordField Name="MSRJM" SortType="AsFloat" />
                    <ext:RecordField Name="MSRDF" SortType="AsFloat" />
                    <ext:RecordField Name="MSRXJ" SortType="AsFloat" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader Root="Staffdepts">
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="DEPT_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" AutoLoad="true">
        <Proxy>
            <ext:HttpProxy Method="POST" Url="/GuideLook/WebService/StaffInfo.ashx" />
        </Proxy>
        <Reader>
            <ext:JsonReader Root="StaffInfos">
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="STAFF_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:Panel runat="server" ID="p11" AutoScroll="true" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server">
                                        <Items>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="10" />
                                            <ext:Label ID="Label7" runat="server" Text="统计日期">
                                            </ext:Label>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                            <ext:ComboBox runat="server" ID="Comb_StartYear" Width="60" ListWidth="60" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                            <ext:ComboBox runat="server" ID="Comb_StartMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月" />
                                            <ext:ComboBox runat="server" ID="Comb_StartDate" Width="40" ListWidth="40" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="dd1Name" runat="server" Text="日 " />
                                            <ext:ToolbarTextItem ID="ToolbarTextItem7" runat="server" Text="   至   " />
                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                            <ext:ComboBox runat="server" ID="Comb_EndYear" Width="60" ListWidth="60" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="年" />
                                            <ext:ComboBox runat="server" ID="Comb_EndMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem5" runat="server" Text="月" />
                                            <ext:ComboBox runat="server" ID="Comb_EndDate" Width="40" ListWidth="40" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="dd2Name" runat="server" Text="日 " />
                                            <ext:Label ID="Label3" runat="server" Text="科室：">
                                            </ext:Label>
                                            <ext:ComboBox ID="DeptCodeCombo" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                                Width="120" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                                PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="科室信息"
                                                ListWidth="240">
                                                <Template ID="Template1" runat="server">
                                                   <tpl for=".">
                                                      <div class="search-item">
                                                         <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                                      </div>
                                                   </tpl>
                                                </Template>
                                            </ext:ComboBox>
                                            <ext:Label ID="Label1" runat="server" Text="人员：">
                                            </ext:Label>
                                            <ext:ComboBox ID="cboStaffInfo" runat="server" StoreID="Store2" DisplayField="STAFF_NAME"
                                                Width="120" ValueField="STAFF_ID" TypeAhead="false" LoadingText="Searching..."
                                                PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="人员" ListWidth="240">
                                                <Template ID="Template2" runat="server">
                                                   <tpl for=".">
                                                      <div class="search-item">
                                                         <h3><span style="width:auto">{STAFF_ID}</span>{STAFF_NAME}({DEPT_NAME})</h3>
                                                      </div>
                                                   </tpl>
                                                </Template>
                                            </ext:ComboBox>
                                            <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                                <Listeners>
                                                    <Click Handler="#{Store1}.reload();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:ToolbarFill>
                                            </ext:ToolbarFill>
                                            <ext:Button ID="btnExcel" runat="server" Text=" 导出Excel " Icon="PageWhiteExcel" OnClick="OutExcel"
                                                AutoPostBack="true" Disabled="true">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <center>
                                        <h2>
                                            医生收入报表
                                        </h2>
                                    </center>
                                    <ext:ExtGridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Width="800"
                                        Height="400" AutoScroll="true" StyleSpec="margin:10px" Border="true">
                                        <ExtColumnModel ID="ColumnModel1" runat="server">
                                            <Columns>
                                                <ext:ExtColumn ColumnID="DEPT_NAME" Header="所在科室" Sortable="true" DataIndex="DEPT_NAME" />
                                                <ext:ExtColumn ColumnID="DEPT_CODE" Header="本科" Sortable="true" DataIndex="DEPT_CODE"
                                                    Hidden="true" />
                                                <ext:ExtColumn ColumnID="STAFF_ID" Header="本科" Sortable="true" DataIndex="STAFF_ID"
                                                    Hidden="true" />
                                                <ext:ExtColumn ColumnID="NAME" Header="医生" Sortable="true" DataIndex="NAME" />
                                                <ext:ExtColumn ColumnID="MZJM" Header="军免" Sortable="true" DataIndex="MZJM" />
                                                <ext:ExtColumn ColumnID="MZDF" Header="地方" Sortable="true" DataIndex="MZDF" />
                                                <ext:ExtColumn ColumnID="MZXJ" Header="小计" Sortable="true" DataIndex="MZXJ" />
                                                <ext:ExtColumn ColumnID="ZYJM" Header="军免" Sortable="true" DataIndex="ZYJM" />
                                                <ext:ExtColumn ColumnID="ZYDF" Header="地方" Sortable="true" DataIndex="ZYDF" />
                                                <ext:ExtColumn ColumnID="ZYXJ" Header="小计" Sortable="true" DataIndex="ZYXJ" />
                                                <ext:ExtColumn ColumnID="MSRJM" Header="军免" Sortable="true" DataIndex="MSRJM" />
                                                <ext:ExtColumn ColumnID="MSRDF" Header="地方" Sortable="true" DataIndex="MSRDF" />
                                                <ext:ExtColumn ColumnID="MSRXJ" Header="小计" Sortable="true" DataIndex="MSRXJ" />
                                            </Columns>
                                            <HeadRows>
                                                <ext:ExtRows>
                                                    <Rows>
                                                        <ext:ExtRow Header="" ColSpan="4" Align="Center" />
                                                        <ext:ExtRow Header="门诊" ColSpan="3" Align="Center" />
                                                        <ext:ExtRow Header="住院" ColSpan="3" Align="Center" />
                                                        <ext:ExtRow Header="毛收入" ColSpan="3" Align="Center" />
                                                    </Rows>
                                                </ext:ExtRows>
                                            </HeadRows>
                                        </ExtColumnModel>
                                        <Plugins>
                                            <ext:ExtGroupHeaderGrid ID="ExtGroupHeaderGrid2" runat="server">
                                            </ext:ExtGroupHeaderGrid>
                                        </Plugins>
                                        <SelectionModel>
                                            <ext:RowSelectionModel SingleSelect="true" ID="selectRow">
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Listeners>
                                            <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -20);}; this.setHeight( Ext.getBody().getViewSize().height -130); }, this);" />
                                            <Render Handler="if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -20);}; this.setHeight( Ext.getBody().getViewSize().height -130);" />
                                            <CellClick Handler="ViewDeptBenefitInfo(this,rowIndex,columnIndex)" />
                                        </Listeners>
                                        <AjaxEvents>
                                            <DblClick OnEvent="QueryDeptBenefitInfo">
                                                <ExtraParams>
                                                    <ext:Parameter Name="value" Value="Ext.encode(#{GridPanel_Show}.getRowsValues())"
                                                        Mode="Raw">
                                                    </ext:Parameter>
                                                    <ext:Parameter Name="col" Value="#{ColIndex}.value" Mode="Raw">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </DblClick>
                                        </AjaxEvents>
                                        <LoadMask ShowMask="true" Msg="查询中....." />
                                    </ext:ExtGridPanel>
                                </Body>
                            </ext:Panel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Width="580" Height="384"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;"
        Maximizable="true">
    </ext:Window>
    <ext:Window ID="arcEditDetailWindow" runat="server" Icon="Group" Width="580" Height="384"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;"
        Maximizable="true" Title="项目明细">
    </ext:Window>
    </form>
</body>
</html>
