<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptBaseInfoStat.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.DeptBaseInfoStat" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="ATTRIBUE" />
                    <ext:RecordField Name="WEAVE_BED" />
                    <ext:RecordField Name="DEPLOY_BED" />
                    <ext:RecordField Name="DIRECTOR" />
                    <ext:RecordField Name="SUBDIRECOTR" />
                    <ext:RecordField Name="CHARGE_NURSE" />
                    <ext:RecordField Name="SPEC_SORT_NAME" />
                    <ext:RecordField Name="SORT_NAME" />
                    <ext:RecordField Name="CENTER_NAME" />
                    <ext:RecordField Name="IS_PIVOT_DEPT" />
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
                                            <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <Click OnEvent="QueryDept">
                                                        <EventMask ShowMask="true" Msg="请稍候..." />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <center>
                                        <h2>
                                            科室信息统计
                                        </h2>
                                    </center>
                                    <ext:ExtGridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Width="800"
                                        Height="400" AutoScroll="true" StyleSpec="margin:10px" Border="true">
                                        <ExtColumnModel ID="ColumnModel1" runat="server">
                                                                                  <Columns>
                                                        <ext:ExtColumn ColumnID="DEPT_NAME" Header="科室" Sortable="true" DataIndex="DEPT_NAME" />
                                                        <ext:ExtColumn ColumnID="ATTRIBUE" Header="科室属性" Sortable="true" DataIndex="ATTRIBUE" />
                                                        <ext:ExtColumn ColumnID="WEAVE_BED" Header="编制床位" Sortable="true" DataIndex="WEAVE_BED" />
                                                        <ext:ExtColumn ColumnID="DEPLOY_BED" Header="展开床位" Sortable="true" DataIndex="DEPLOY_BED" />
                                                        <ext:ExtColumn ColumnID="DIRECTOR" Header="科主任" Sortable="true" DataIndex="DIRECTOR" />
                                                        <ext:ExtColumn ColumnID="SUBDIRECOTR" Header="副主任" Sortable="true" DataIndex="SUBDIRECOTR" />
                                                        <ext:ExtColumn ColumnID="CHARGE_NURSE" Header="护士长" Sortable="true" DataIndex="CHARGE_NURSE" />
                                                        <ext:ExtColumn ColumnID="SPEC_SORT_NAME" Header="专业组" Sortable="true" DataIndex="SPEC_SORT_NAME" Width="180"/>
                                                        <ext:ExtColumn ColumnID="SORT_NAME" Header="负责人" Sortable="true" DataIndex="SORT_NAME" />
                                                        <ext:ExtColumn ColumnID="CENTER_NAME" Header="专科中心" Sortable="true" DataIndex="CENTER_NAME" />
                                                        <ext:ExtColumn ColumnID="IS_PIVOT_DEPT" Header="医院重点科室 " Sortable="true" DataIndex="IS_PIVOT_DEPT" />
                                                    </Columns>
                                                    <HeadRows>
                                                        <ext:ExtRows>
                                                            <Rows>
                                                                <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                                <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                                <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                                <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                                <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                                <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                                <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                                <ext:ExtRow Header="科室专业" ColSpan="2" Align="Center" />
                                                                <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                                <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                            </Rows>
                                                        </ext:ExtRows>
                                                    </HeadRows>
                                        </ExtColumnModel>
                                        <Plugins>
                                            <ext:ExtGroupHeaderGrid ID="ExtGroupHeaderGrid2" runat="server">
                                            </ext:ExtGroupHeaderGrid>
                                        </Plugins>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Listeners>
                                            <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -20);}; this.setHeight( Ext.getBody().getViewSize().height -130); }, this);" />
                                            <Render Handler="if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -20);}; this.setHeight( Ext.getBody().getViewSize().height -130);" />
                                        </Listeners>
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
    </form>
</body>
</html>
