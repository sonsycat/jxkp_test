<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffStateList.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.StaffStateList" %>

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

    <script type="text/javascript">        
        var RowIndex;       
        
        var CheckForm = function() {
            if (dtfUpDate.validate() == false) {
                return false;
            }
            return true;
        } 
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="ADD_MARK" />
                    <ext:RecordField Name="ISONGUARD" />
                    <ext:RecordField Name="CASES" />
                    <ext:RecordField Name="PLACE" />
                    <ext:RecordField Name="PERSON" />
                    <ext:RecordField Name="STRAT_DATE" />
                    <ext:RecordField Name="END_DATE" />
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="STAFFSORT" />
                    <ext:RecordField Name="JOB" />
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
                                <ext:Label ID="Label4" runat="server" Text="　日期:　">
                                </ext:Label>
                                <ext:DateField ID="dtfStratDate" runat="server" Format="yyyy-MM-dd">
                                </ext:DateField>
                                <ext:Label ID="Label2" runat="server" Text="　">
                                </ext:Label>
                                <ext:Checkbox ID="cbxInline" runat="server">
                                </ext:Checkbox>
                                <ext:Label ID="Label1" runat="server" Text="今日不在岗　">
                                </ext:Label>
                                <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                    <Listeners>
                                        <Click Handler="#{Store1}.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnSetCase" runat="server" Text="请假" Icon="KeyAdd" Disabled="true">
                                    <Listeners>
                                        <Click Handler="#{arcEditWindow}.show();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnUpCase" runat="server" Text="销假" Icon="KeyGo" Disabled="true">
                                    <Listeners>
                                        <Click Handler="#{dtfSetUp}.setValue(#{Store1}.getAt(RowIndex).get('STRAT_DATE'));#{winUpDate}.show();" />
                                    </Listeners>
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
                                                        <ext:Column ColumnID="DEPT_NAME" Header="科室" Sortable="true" DataIndex="DEPT_NAME" />
                                                        <ext:Column ColumnID="NAME" Header="姓名" Sortable="true" DataIndex="NAME" />
                                                        <ext:Column ColumnID="SEX" Header="性别" Sortable="true" DataIndex="SEX" />
                                                        <ext:Column ColumnID="ISONGUARD" Header="在岗" Sortable="true" DataIndex="ISONGUARD" />
                                                        <ext:Column ColumnID="JOB" Header="职务" Sortable="true" DataIndex="JOB" />
                                                        <ext:Column ColumnID="STAFFSORT" Header="人员类别" Sortable="true" DataIndex="STAFFSORT" />
                                                        <ext:Column ColumnID="STRAT_DATE" Header="请假日期" Sortable="true" DataIndex="STRAT_DATE" />
                                                        <ext:Column ColumnID="END_DATE" Header="销假日期" Sortable="true" DataIndex="END_DATE" />
                                                        <ext:Column ColumnID="CASES" Header="今日是否在岗" Sortable="true" DataIndex="CASES" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                                        <Listeners>
                                                            <RowSelect Handler="if(#{Store1}.getAt(rowIndex).get('CASES') == '在岗'){#{btnSetCase}.enable();#{btnUpCase}.disable();} else {#{btnUpCase}.enable();#{btnSetCase}.disable();}
                                                                                if(#{Store1}.getAt(rowIndex).get('STRAT_DATE') != null && #{Store1}.getAt(rowIndex).get('END_DATE') != null) {#{btnUpCase}.disable();#{btnSetCase}.disable();} RowIndex = rowIndex" />
                                                            <RowDeselect Handler="#{btnSetCase}.disable();#{btnUpCase}.disable();RowIndex = -1;" />
                                                        </Listeners>
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
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="请假" Width="350"
        Height="220" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboCaseType" runat="server" FieldLabel="请假类别" Editable="false"
                                        SelectedIndex="0">
                                        <Items>
                                            <ext:ListItem Text="休假" Value="休假" />
                                            <ext:ListItem Text="事假" Value="事假" />
                                            <ext:ListItem Text="病假" Value="病假" />
                                            <ext:ListItem Text="产假" Value="产假" />
                                            <ext:ListItem Text="公出" Value="公出" />
                                        </Items>
                                    </ext:ComboBox>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtInputStaff" runat="server" FieldLabel="批准人" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txtMemo" runat="server" FieldLabel="备注" Height="100" />
                                </ext:Anchor>
                            </ext:FormLayout>
                        </Body>
                    </ext:Panel>
                </ext:LayoutColumn>
            </ext:ColumnLayout>
        </Body>
        <BottomBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="Disk" Text="请假">
                        <AjaxEvents>
                            <Click OnEvent="InsertCase" Success="#{arcEditWindow}.hide();#{Store1}.reload();#{btnSetCase}.disable();#{btnUpCase}.disable();">
                                <ExtraParams>
                                    <ext:Parameter Name="deptCode" Value="Ext.encode(#{Store1}.getAt(RowIndex).get('DEPT_CODE'))"
                                        Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="Name" Value="Ext.encode(#{Store1}.getAt(RowIndex).get('NAME'))"
                                        Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
                                <EventMask Msg="正在插入......" />
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="退出">
                        <Listeners>
                            <Click Handler="arcEditWindow.hide();" />
                        </Listeners>
                    </ext:ToolbarButton>
                </Items>
            </ext:Toolbar>
        </BottomBar>
    </ext:Window>
    <ext:Window ID="winUpDate" runat="server" Icon="Group" Title="销假" Width="250" Height="150"
        AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false" Resizable="false"
        StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout3" runat="server">
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel3" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:DateField ID="dtfSetUp" FieldLabel="请假日期" Format="yyyy-MM-dd" runat="server" ReadOnly="true">
                                    </ext:DateField>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:DateField ID="dtfUpDate" FieldLabel="注销日期" Format="yyyy-MM-dd" runat="server"  CausesValidation="true"
                                                            AllowBlank="false">
                                    </ext:DateField>
                                </ext:Anchor>
                            </ext:FormLayout>
                        </Body>
                    </ext:Panel>
                </ext:LayoutColumn>
            </ext:ColumnLayout>
        </Body>
        <BottomBar>
            <ext:Toolbar ID="Toolbar2" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                    <ext:ToolbarButton ID="btnUpDate" runat="server" Icon="Disk" Text="销假">
                        <AjaxEvents>
                            <Click OnEvent="UpdateCase"
                                                        Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;}">
                                <ExtraParams>
                                    <ext:Parameter Name="id" Value="Ext.encode(#{Store1}.getAt(RowIndex).get('ID'))"
                                        Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
                                <EventMask Msg="正在插入......" />
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                    <ext:ToolbarButton ID="btnUpDateHide" runat="server" Icon="Cancel" Text="退出">
                        <Listeners>
                            <Click Handler="winUpDate.hide();" />
                        </Listeners>
                    </ext:ToolbarButton>
                </Items>
            </ext:Toolbar>
        </BottomBar>
    </ext:Window>
    </form>
</body>
</html>
