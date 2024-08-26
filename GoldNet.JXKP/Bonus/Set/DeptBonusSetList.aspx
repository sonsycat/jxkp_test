<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptBonusSetList.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Set.DeptBonusSetList" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg) {
            Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
            
    </script>
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
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <ext:Store ID="Store1" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="STAFF_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="DAYS">
                    </ext:RecordField>
                    <ext:RecordField Name="BONUSMODULUS">
                    </ext:RecordField>
                    <ext:RecordField Name="PERSONSMODULUS">
                    </ext:RecordField>
                    <ext:RecordField Name="SUMMODULUS">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store4" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="MONTH">
                <Fields>
                    <ext:RecordField Name="MONTH" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
     <ext:Store ID="SDept" runat="server" AutoLoad="true">
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="Store3" Width="70"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                                <AjaxEvents>
                                                    <Select OnEvent="Button_look_click">
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="Store4" Width="70"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="lmonth" runat="server" Text="月">
                                            </ext:Label>
                                             <ext:Label ID="Label3" runat="server" Text="科室：" />
                                            <ext:ComboBox ID="cbbdept" runat="server" StoreID="SDept" DisplayField="DEPT_NAME"
                                                ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="150"
                                                PageSize="10" ItemSelector="div.search-item" MinChars="1" ListWidth="200">
                                                <Template ID="Template1" runat="server">
                                                    <tpl for=".">
                                                        <div class="search-item">
                                                             <h3><span>
                                                          {DEPT_CODE}</span>{DEPT_NAME}</h3>
                                                             </div>
                                                      </tpl>                                                                                                       
                                                </Template>
                                 
                                            </ext:ComboBox>
                                            <ext:Button ID="Button_look" runat="server" Text="查询" Icon="FolderMagnify">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_look_click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室名称</div>"
                                            Width="200" Align="Left" Sortable="true" DataIndex="DEPT_NAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="STAFF_NAME" Header="<div style='text-align:center;'>人员</div>" Width="100"
                                            Align="Left" Sortable="true" DataIndex="STAFF_NAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="DAYS" Header="<div style='text-align:center;'>工作日数</div>"
                                            Width="100" Align="Right" Sortable="true" DataIndex="DAYS" MenuDisabled="true" Hidden="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="BONUSMODULUS" Header="<div style='text-align:center;'>科室自设系数</div>"
                                            Width="100" Align="Right" Sortable="true" DataIndex="BONUSMODULUS" MenuDisabled="true" Hidden="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="PERSONSMODULUS" Header="<div style='text-align:center;'>院标准系数</div>"
                                            Width="100" Align="Right" Sortable="true" DataIndex="PERSONSMODULUS" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="SUMMODULUS" Header="<div style='text-align:center;'>合计系数</div>"
                                            Width="100" Align="Right" Sortable="true" DataIndex="SUMMODULUS" MenuDisabled="true" Hidden="true">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
