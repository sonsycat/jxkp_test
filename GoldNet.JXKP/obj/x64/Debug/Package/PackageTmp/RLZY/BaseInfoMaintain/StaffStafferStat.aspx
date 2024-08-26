<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffStafferStat.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.StaffStafferStat" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="JOB" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="SEX" />
                    <ext:RecordField Name="BIRTHDAY" />
                    <ext:RecordField Name="STAFFSORT" />
                    <ext:RecordField Name="DUTY" />
                    <ext:RecordField Name="JOBDATE" />
                    <ext:RecordField Name="TOPEDUCATE" />
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
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <North>
                        <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0">
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
                                    <AjaxEvents>
                                        <Click OnEvent="QueryStaff">
                                            <EventMask ShowMask="true" Msg="请稍候..." />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:Panel ID="Panel1" runat="server">
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Border="false"
                                                AutoWidth="true" Header="false" AutoScroll="true">
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="NAME" Header="姓名" Sortable="true" DataIndex="NAME" />
                                                        <ext:Column ColumnID="DUTY" Header="职务" Sortable="true" DataIndex="DUTY" />
                                                        <ext:Column ColumnID="DEPT_NAME" Header="科室" Sortable="true" DataIndex="DEPT_NAME" />
                                                        <ext:Column ColumnID="SEX" Header="性别" Sortable="true" DataIndex="SEX" />
                                                        <ext:Column ColumnID="BIRTHDAY" Header="出生年月" Sortable="true" DataIndex="BIRTHDAY" />
                                                        <ext:Column ColumnID="JOB" Header="职称" Sortable="true" DataIndex="JOB" />
                                                        <ext:Column ColumnID="JOBDATE" Header="职称时间" Sortable="true" DataIndex="JOBDATE" />
                                                        <ext:Column ColumnID="STAFFSORT" Header="人员类别" Sortable="true" DataIndex="STAFFSORT" />
                                                        <ext:Column ColumnID="TOPEDUCATE" Header="学历" Sortable="true" DataIndex="TOPEDUCATE" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <LoadMask ShowMask="true" />
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
