<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SufferAgeStat.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.SufferAgeStat" %>

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
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="JOB1" />
                    <ext:RecordField Name="JOB2" />
                    <ext:RecordField Name="JOB3" />
                    <ext:RecordField Name="JOB4" />
                    <ext:RecordField Name="JOB5" />
                    <ext:RecordField Name="JOB6" />
                    <ext:RecordField Name="JOB7" />
                    <ext:RecordField Name="JOB8" />
                    <ext:RecordField Name="JOB9" />
                    <ext:RecordField Name="JOB10" />
                    <ext:RecordField Name="JOB11" />
                    <ext:RecordField Name="JOB12" />
                    <ext:RecordField Name="JOB13" />
                    <ext:RecordField Name="JOB14" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Proxy>
            <ext:HttpProxy Method="POST" Url="/RLZY/WebService/DeptInfo.ashx" />
        </Proxy>
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
                                            <ext:Label ID="Label1" runat="server" Text="人员类别：">
                                            </ext:Label>
                                            <ext:ComboBox ID="cboPersonType" runat="server" Editable="false">
                                            </ext:ComboBox>
                                            <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                                <Listeners>
                                                    <Click Handler="#{Store1}.reload();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <center>
                                        <h2>
                                            学历，年龄分布
                                        </h2>
                                    </center>
                                    <ext:ExtGridPanel ID="GridPanel_Show" runat="server" StoreID="Store1"
                                        Width="800" Height="400" AutoScroll="true" StyleSpec="margin:10px" Border="true">
                                        <ExtColumnModel ID="ColumnModel1" runat="server">
                                            <Columns>
                                                <ext:ExtColumn ColumnID="DEPT_NAME" Header="" Sortable="true" DataIndex="DEPT_NAME" />
                                                <ext:ExtColumn ColumnID="JOB1" Header="研究生" Sortable="true" DataIndex="JOB1" />
                                                <ext:ExtColumn ColumnID="JOB2" Header="本科" Sortable="true" DataIndex="JOB2" />
                                                <ext:ExtColumn ColumnID="JOB3" Header="大专" Sortable="true" DataIndex="JOB3" />
                                                <ext:ExtColumn ColumnID="JOB4" Header="中专" Sortable="true" DataIndex="JOB4" />
                                                <ext:ExtColumn ColumnID="JOB5" Header="其它" Sortable="true" DataIndex="JOB5" />
                                                <ext:ExtColumn ColumnID="JOB6" Header="博士" Sortable="true" DataIndex="JOB6" />
                                                <ext:ExtColumn ColumnID="JOB7" Header="硕士" Sortable="true" DataIndex="JOB7" />
                                                <ext:ExtColumn ColumnID="JOB8" Header="学士" Sortable="true" DataIndex="JOB8" />
                                                <ext:ExtColumn ColumnID="JOB9" Header="30岁以下" Sortable="true" DataIndex="JOB9" />
                                                <ext:ExtColumn ColumnID="JOB10" Header="31-40" Sortable="true" DataIndex="JOB10" />
                                                <ext:ExtColumn ColumnID="JOB11" Header="41-50" Sortable="true" DataIndex="JOB11" />
                                                <ext:ExtColumn ColumnID="JOB12" Header="51-55" Sortable="true" DataIndex="JOB12" />
                                                <ext:ExtColumn ColumnID="JOB13" Header="56-60" Sortable="true" DataIndex="JOB13" />
                                                <ext:ExtColumn ColumnID="JOB14" Header="60岁以上" Sortable="true" DataIndex="JOB14" />
                                            </Columns>
                                            <HeadRows>
                                                <ext:ExtRows>
                                                    <Rows>
                                                        <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                        <ext:ExtRow Header="学历层次" ColSpan="5" Align="Center" />
                                                        <ext:ExtRow Header="学位" ColSpan="3" Align="Center" />
                                                        <ext:ExtRow Header="年龄段分布" ColSpan="6" Align="Center" />
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
