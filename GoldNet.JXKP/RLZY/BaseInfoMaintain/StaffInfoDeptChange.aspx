<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffInfoDeptChange.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.StaffInfoDeptChange" %>

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
        /*
            GRIDPANEL操作
            optype :1 添加;  2 重命名; 3 删除;
        */
        var RowIndex;
        function TreeOpration() {
            cboChangeDept.setValue("");
            staffName.setValue(this.Store1.getAt(RowIndex).get('NAME'));
            arcEditWindow.show();
        }
        
         var CheckForm = function() {
            if (cboChangeDept.validate() == false) {
                return false;
            }
            if (staffName.validate() == false) {
                return false;
            }
            return true;
        }
        
        var Staffid = function() {
            return this.Store1.getAt(RowIndex).get('STAFF_ID');
        }
        
        function GridPanelToDataBase(id) {
          Goldnet.AjaxMethod.request(
                  'PersonsPlantAjaxOper',
                    {
                        params: {
                           Id:id
                        },
                        success: function(result) {
                            Store1.reload();
                            btn_Delete.setDisabled(true);
                            btn_Modify.setDisabled(true);
                            arcEditWindow.hide();
                        },
                        failure: function(msg) {
                            GridPanel_Show.el.unmask();
                        }
                    });
        }
           var applyFilter = function() {
            Store1.filterBy(getRecordFilter());
        };
        var getRecordFilter = function() {
            var f = [];
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'NAME', record);
                }
            });
            var len = f.length;
            return function(record) {
                if (f[0].filter(record)) {
                    return true;
                }
                return false;
            }
        };
        var filterString = function(value, dataIndex, record) {
            var val = record.get(dataIndex);
            if (typeof val != "string") {
                return value.length == 0;
            }
            return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
        };
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="STAFF_ID">
                <Fields>
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="JOB" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="SEX" />
                    <ext:RecordField Name="BIRTHDAY" />
                    <ext:RecordField Name="STAFFSORT" />
                    <ext:RecordField Name="DUTY" />
                    <ext:RecordField Name="DUTYDATE" />
                    <ext:RecordField Name="WORKDATE" />
                    <ext:RecordField Name="JOBDATE" />
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="FROM_DEPT_NAME"></ext:RecordField>
                    <ext:RecordField Name="CHANGE_DATE"></ext:RecordField>
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
                                <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                    <Listeners>
                                        <Click Handler="#{Store1}.reload();#{btnDeptChange}.disable();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnDeptChange" runat="server" Text="科室调动" Icon="LinkGo" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration();" />
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
                                                        <ext:Column ColumnID="NAME" Header="姓名" Sortable="true" DataIndex="NAME" />
                                                        <ext:Column ColumnID="DUTY" Header="职务" Sortable="true" DataIndex="DUTY" />
                                                        <ext:Column ColumnID="DEPT_NAME" Header="科室" Sortable="true" DataIndex="DEPT_NAME" />
                                                        <ext:Column ColumnID="STAFFSORT" Header="人员类别" Sortable="true" DataIndex="STAFFSORT" />
                                                        <ext:Column ColumnID="JOB" Header="职称" Sortable="true" DataIndex="JOB" />
                                                        <ext:Column ColumnID="WORKDATE" Header="工作时间" Sortable="true" DataIndex="WORKDATE" />
                                                        <ext:Column ColumnID="FROM_DEPT_NAME" Header="转科前所在科室" Sortable="true" DataIndex="FROM_DEPT_NAME" />
                                                        <ext:Column ColumnID="CHANGE_DATE" Header="转科时间" Sortable="true" DataIndex="CHANGE_DATE" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                                        <Listeners>
                                                            <RowSelect Handler="#{btnDeptChange}.enable();RowIndex = rowIndex;" />
                                                            <RowDeselect Handler="if (!#{GridPanel_Show}.hasSelection()) {#{btnDeptChange}.disable();RowIndex = -1;}" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <LoadMask ShowMask="true" />
                                                <BottomBar>
                                                   <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="20" StoreID="Store1" AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                                                        <Items>
                                                            <ext:TextField ID="txt_SearchTxt" runat="server" EmptyText="查找信息">
                                                                <ToolTips>
                                                                    <ext:ToolTip ID="ToolTip1" runat="server" Html="根据姓名关键字查找">
                                                                    </ext:ToolTip>
                                                                </ToolTips>
                                                            </ext:TextField>
                                                            <ext:Button ID="btn_Search" Icon="Zoom" runat="server" Text="查询">
                                                                <Listeners>
                                                                    <Click Fn="applyFilter" />
                                                                </Listeners>
                                                            </ext:Button>
                                 
                                                            
                                                        </Items>
                                                    </ext:PagingToolbar>
                                                </BottomBar>
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
        <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="科室调动" Width="250"
            Height="130" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
            Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                    <ext:LayoutColumn ColumnWidth="1">
                        <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                            <Body>
                                <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextField ID="staffName" CausesValidation="true" AllowBlank="false" runat="server"
                                            FieldLabel="调动人员名称" MaxLength="20">
                                        </ext:TextField>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cboChangeDept" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                            Width="120" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                            PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="调动人员科室"
                                            ListWidth="240" CausesValidation="true" AllowBlank="false">
                                            <Template ID="Template2" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                            </Template>
                                        </ext:ComboBox>
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
                        <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="Disk" Text="保存">
                               <AjaxEvents>
                                    <Click OnEvent="SaveInfo" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};" 
                                                            Success=" Store1.reload();
                                                                      btnDeptChange.setDisabled(true);
                                                                      arcEditWindow.hide();">
                                        <ExtraParams>
                                            <ext:Parameter Name="Staffid" Value="Ext.encode(#{Store1}.getAt(RowIndex).get('STAFF_ID'))" Mode="Raw" ></ext:Parameter>
                                            <ext:Parameter Name="staffOldDeptCode" Value="Ext.encode(#{Store1}.getAt(RowIndex).get('DEPT_CODE'))" Mode="Raw"></ext:Parameter>
                                            <ext:Parameter Name="staffOldDeptName" Value="Ext.encode(#{Store1}.getAt(RowIndex).get('DEPT_NAME'))" Mode="Raw"></ext:Parameter>
                                            <ext:Parameter Name="StaffName" Value="Ext.encode(#{Store1}.getAt(RowIndex).get('NAME'))" Mode="Raw"></ext:Parameter>
                                        </ExtraParams>
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
    </div>
    </form>
</body>
</html>
