<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountDeptTypeSet.aspx.cs"
    Inherits="GoldNet.JXKP.AccountDeptTypeSet" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />
</head>

<script type="text/javascript">
     var ShowRadios = function(value, rowrecord) {
         //var store1rowid = Store2.indexOf(rowrecord);
         var store1rowid = rowrecord.DEPTCODE;
         var rcount = Store2.getTotalCount();
         var str = "";
         for (var i = 0; i < rcount; i++) {
             var record = Store2.getAt(i);
             var columnvalue = record.get('NAME');
             var columnid = record.get('ID');
             var template = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id='" + columnid + "_" + store1rowid + "' type='radio' name='" + store1rowid + "'  onclick=typeSelect(" + columnid + ",'" + store1rowid + "') value='" + columnid + "' {0} />" + columnvalue;
             str += String.format(template, (value == columnid) ? 'checked' : 'unchecked'); ;
         }
         return str;
     }
     function typeSelect(inputname, id) {
         var record = Store1.getById(id);
         record.data['TYPESELECT'] = inputname;
     }

     function selectDept(combox) {
         var id = combox.value;
         Store1.filterBy(getRecordFilter(id));
      
     }
     var getRecordFilter = function(id) {
         var f = [];
         f.push({
             filter: function(record) {
             return filterString(id, 'DEPTCODE', record);
             }
         });
         var len = f.length;
         return function(record) {
             if (id == '00000') {
                 return true;
             }
             if (f[0].filter(record)) {
                 return true;
             }
             else {
                 return false;
             }
         }

     }
     var filterString = function(value, dataIndex, record) {

         var val = record.get(dataIndex);
         if (typeof val != "string") {
             return value.length == 0;
         }
         return val == value;
     }
     var filterDate = function(value, dataIndex, record) {
         var val = record.get(dataIndex).clearTime(true).getTime();

         if (!Ext.isEmpty(value, false) && val != value.clearTime(true).getTime()) {
             return false;
         }
         return true;
     }
     var filterNumber = function(value, dataIndex, record) {
         var val = record.get(dataIndex);
         if (!Ext.isEmpty(value, false) && val != value) {
             return false;
         }
         return true;
     } 
</script>

<style type="text/css">
    body
    {
        background-color: #DFE8F6;
        font-size: 12px;
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
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store2" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID">
                    </ext:RecordField>
                    <ext:RecordField Name="NAME">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store1" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="DEPTCODE">
                <Fields>
                    <ext:RecordField Name="DEPTCODE">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPTNAME">
                    </ext:RecordField>
                    <ext:RecordField Name="TYPE">
                        <Convert Fn="ShowRadios" />
                    </ext:RecordField>
                    <ext:RecordField Name="TYPESELECT">
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
                            <ext:GridPanel ID="GridPanel1" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" Height="480">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Label ID="lcaption" runat="server" Text="核算年月份:">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="Store3" Width="70"
                                                DisplayField="YEAR" ValueField="YEAR">
                                                <ToolTips>
                                                    <ext:ToolTip runat="server" Html="选择年月后自动查询设置信息">
                                                    </ext:ToolTip>
                                                </ToolTips>
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="Store4" Width="70"
                                                DisplayField="MONTH" ValueField="MONTH">
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip1" runat="server" Html="选择年月后自动查询设置信息">
                                                    </ext:ToolTip>
                                                </ToolTips>
                                            </ext:ComboBox>
                                            <ext:Label ID="lmonth" runat="server" Text="月">
                                            </ext:Label>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                            <ext:Button ID="bquery" Text="查询" Icon="FolderMagnify" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="CBBSelect_Query">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                            <ext:Label ID="Label3" runat="server" Text="科室：" />
                                            <ext:ComboBox ID="cbbdept" runat="server" StoreID="SDept" DisplayField="DEPT_NAME"
                                                ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="150"
                                                PageSize="10" ItemSelector="div.search-item" MinChars="1" ListWidth="200">
                                                <Template ID="Template1" runat="server">
                                                    <tpl for=".">
                                                        <div class="search-item">
                                                             <h3>{DEPT_NAME}</h3>
                                                             </div>
                                                      </tpl>                                                                                                       
                                                </Template>
                                                <Listeners>
                                                    <Select Fn="selectDept" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                            <ext:Button ID="bsave" Text="保存" Icon="Disk" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Save">
                                                        <EventMask Msg="正在保存" ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues(false))"
                                                                Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="Deptname" Header="<center>科室</center>" MenuDisabled="true" Width="130" DataIndex="DEPTNAME"
                                            Align="Left" />
                                        <ext:Column ColumnID="Type" Align="Center" Header="类别" Width="880" MenuDisabled="true"
                                            DataIndex="TYPE">
                                        </ext:Column>
                                        <ext:Column ColumnID="TypeSelect" Hidden="true" DataIndex="TYPESELECT">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <%--  <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="20" StoreID="Store1"
                                        AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                                    </ext:PagingToolbar>
                                </BottomBar>--%>
                                <LoadMask ShowMask="true" />
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
