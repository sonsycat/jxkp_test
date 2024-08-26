<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PowerManager.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.PowerManager" %>

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

    <script type="text/javascript">
 var SelectorLayout = function() {
             GridPanel1.setHeight(Ext.lib.Dom.getViewHeight() - GridPanel1.getPosition()[1]- 5);
             GridPanel2.setHeight(Ext.lib.Dom.getViewHeight() - GridPanel2.getPosition()[1]- 5);
         }
        var CountrySelector = {
            add: function(source, destination) {
                source = source || GridPanel1;
                destination = destination || GridPanel2;
                if (source.hasSelection()) {
                    destination.store.add(source.selModel.getSelections());
                    source.deleteSelected();
                }
            },
            addAll: function(source, destination) {
                source = source || GridPanel1;
                destination = destination || GridPanel2;
                destination.store.add(source.store.getRange());
                source.store.removeAll();
            },
            addByName: function(name) {
                if (!Ext.isEmpty(name)) {
                    var result = Store1.query("Name", name);
                    if (!Ext.isEmpty(result.items)) {
                        GridPanel2.store.add(result.items[0]);
                        GridPanel1.store.remove(result.items[0]);
                    }
                }
            },
            addByNames: function(name) {
                for (var i = 0; i < name.length; i++) {
                    this.addByName(name[i]);
                }
            },
            remove: function(source, destination) {
                this.add(destination, source);
            },
            removeAll: function(source, destination) {
                this.addAll(destination, source);
            }
        };
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX">
            <Listeners>
                <DocumentReady Handler=" Ext.EventManager.onWindowResize(SelectorLayout);" />
            </Listeners>
        </ext:ScriptManager>
        <ext:Store ID="Store3" runat="server" AutoLoad="false">
        </ext:Store>
        <ext:Store ID="Store1" runat="server" AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="USER_ID">
                    <Fields>
                        <ext:RecordField Name="ACCOUNT" />
                        <ext:RecordField Name="USER_ID" />
                        <ext:RecordField Name="DB_USER" />
                        <ext:RecordField Name="USER_NAME" />
                        <ext:RecordField Name="DEPT_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server" OnSubmitData="SubmitData">
            <Reader>
                <ext:JsonReader ReaderID="USER_ID">
                    <Fields>  
                        <ext:RecordField Name="ACCOUNT" />
                        <ext:RecordField Name="USER_ID" />
                        <ext:RecordField Name="DB_USER" />
                        <ext:RecordField Name="USER_NAME" />
                        <ext:RecordField Name="DEPT_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:Panel ID="Panel4" runat="server" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Button_EditSet" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Button ID="Buttonadd" runat="server" Text="保存" Icon="DatabaseAdd">
                                                <Listeners>
                                                    <Click Handler="#{GridPanel2}.submitData();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true">
                                        <ext:LayoutColumn ColumnWidth="0.5">
                                            <ext:GridPanel runat="server" ID="GridPanel1" EnableDragDrop="false" AutoExpandColumn="DEPT_NAME"
                                                StoreID="Store1">
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                                        <Items>
                                                            <ext:Label ID="func" runat="server" Text="科室：" Width="40">
                                                            </ext:Label>
                                                            <ext:ComboBox ID="Combo_Dept" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                                                ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="220"
                                                                PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1"
                                                                FieldLabel="科室选择" ListWidth="300">
                                                                <Template ID="Template2" runat="server">
                                                                   <tpl for=".">
                                                                      <div class="search-item">
                                                                         <h3><span>{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                                                  
                                                                      </div>
                                                                   </tpl>
                                                                </Template>
                                                                <AjaxEvents>
                                                                    <Select OnEvent="SelectedDept">
                                                                        <EventMask ShowMask="true" />
                                                                    </Select>
                                                                </AjaxEvents>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="userid" Header="人员编号" DataIndex="USER_ID" Hidden="true" />
                                                    </Columns>
                                                    <Columns>
                                                        <ext:Column ColumnID="dbuser" Header="登录名" DataIndex="DB_USER" Sortable="true" />
                                                    </Columns>
                                                    <Columns>
                                                        <ext:Column ColumnID="username" Header="姓名" DataIndex="USER_NAME" Sortable="true" />
                                                    </Columns>
                                                    <Columns>
                                                        <ext:Column ColumnID="deptname" Header="科室名称" DataIndex="DEPT_NAME" Sortable="true" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" />
                                                </SelectionModel>
                                                <Plugins>
                                                    <ext:GridFilters ID="GridFilters1" runat="server" Local="true">
                                                        <Filters>
                                                            <ext:StringFilter DataIndex="DEPT_NAME" />
                                                        </Filters>
                                                    </ext:GridFilters>
                                                </Plugins>
                                                <Listeners>
                                                    <Render Handler=" this.setHeight(Ext.lib.Dom.getViewHeight() - this.getPosition()[1] );" />
                                                    <DblClick Handler="CountrySelector.add();" />
                                                </Listeners>
                                                <BottomBar>
                                                    <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="20" StoreID="Store1"
                                                        AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                                        <Items>
                                                            <ext:TextField ID="txt_SearchTxt" runat="server" EmptyText="查找人员">
                                                                <ToolTips>
                                                                    <ext:ToolTip ID="ToolTip5" runat="server" Html="根据人员id、姓名、科室等关键字查找">
                                                                    </ext:ToolTip>
                                                                </ToolTips>
                                                            </ext:TextField>
                                                            <ext:Button ID="btn_Search" Icon="Zoom" runat="server" Text="查询">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="select_users">
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:PagingToolbar>
                                                </BottomBar>
                                                <LoadMask ShowMask="true" Msg="载入中..." />
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                        <ext:LayoutColumn>
                                            <ext:Panel ID="Panel2" runat="server" Width="35" BodyStyle="background-color: #DFE8F6;"
                                                Border="false">
                                                <Body>
                                                    <ext:AnchorLayout ID="AnchorLayout1" runat="server">
                                                        <ext:Anchor Vertical="40%">
                                                            <ext:Panel ID="Panel1" runat="server" Border="false" BodyStyle="background-color: transparent;" />
                                                        </ext:Anchor>
                                                        <ext:Anchor>
                                                            <ext:Panel ID="Panel3" runat="server" Border="false" BodyStyle="padding:5px;background-color: transparent;">
                                                                <Body>
                                                                    <ext:Button ID="Button1" runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                                                                        <Listeners>
                                                                            <Click Handler="CountrySelector.add();" />
                                                                        </Listeners>
                                                                        <ToolTips>
                                                                            <ext:ToolTip ID="ToolTip1" runat="server" Title="添加" Html="添加选中行" />
                                                                        </ToolTips>
                                                                    </ext:Button>
                                                                    <ext:Button ID="Button2" runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;">
                                                                        <Listeners>
                                                                            <Click Handler="CountrySelector.addAll();" />
                                                                        </Listeners>
                                                                        <ToolTips>
                                                                            <ext:ToolTip ID="ToolTip2" runat="server" Title="全部添加" Html="全部添加" />
                                                                        </ToolTips>
                                                                    </ext:Button>
                                                                    <ext:Button ID="Button3" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                                                                        <Listeners>
                                                                            <Click Handler="CountrySelector.remove(GridPanel1, GridPanel2);" />
                                                                        </Listeners>
                                                                        <ToolTips>
                                                                            <ext:ToolTip ID="ToolTip3" runat="server" Title="移出" Html="移出选中行" />
                                                                        </ToolTips>
                                                                    </ext:Button>
                                                                    <ext:Button ID="Button4" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                                                                        <Listeners>
                                                                            <Click Handler="CountrySelector.removeAll(GridPanel1, GridPanel2);" />
                                                                        </Listeners>
                                                                        <ToolTips>
                                                                            <ext:ToolTip ID="ToolTip4" runat="server" Title="全部移出" Html="全部移出" />
                                                                        </ToolTips>
                                                                    </ext:Button>
                                                                </Body>
                                                            </ext:Panel>
                                                        </ext:Anchor>
                                                    </ext:AnchorLayout>
                                                </Body>
                                            </ext:Panel>
                                        </ext:LayoutColumn>
                                        <ext:LayoutColumn ColumnWidth="0.5">
                                            <ext:GridPanel runat="server" ID="GridPanel2" EnableDragDrop="false" AutoExpandColumn="DEPT_NAME"
                                                StoreID="Store2">
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar2" runat="server">
                                                        <Items>
                                                            <ext:Label ID="role" runat="server" Text="角色：">
                                                            </ext:Label>
                                                            <ext:ComboBox ID="ComboBox_Role" runat="server" AllowBlank="false" EmptyText="请选择角色"
                                                                FieldLabel="角色选择">
                                                                <AjaxEvents>
                                                                    <Select OnEvent="Selectedrole">
                                                                        <EventMask ShowMask="true" />
                                                                    </Select>
                                                                </AjaxEvents>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <ColumnModel ID="ColumnModel2" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="userid" Header="人员编号" DataIndex="USER_ID" Hidden="true" />
                                                    </Columns>
                                                    <Columns>
                                                        <ext:Column ColumnID="dbuser" Header="登录名" DataIndex="DB_USER" Sortable="true" />
                                                    </Columns>
                                                    <Columns>
                                                        <ext:Column ColumnID="username" Header="姓名" DataIndex="USER_NAME" Sortable="true" />
                                                    </Columns>
                                                    <Columns>
                                                        <ext:Column ColumnID="deptname" Header="科室名称" DataIndex="DEPT_NAME" Sortable="true" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="edit" runat="server" />
                                                </SelectionModel>
                                                <Listeners>
                                                    <Render Handler=" this.setHeight(Ext.lib.Dom.getViewHeight() - this.getPosition()[1] );" />
                                                    <DblClick Handler="CountrySelector.remove(GridPanel1, GridPanel2);" />
                                                </Listeners>
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </ext:ColumnLayout>
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
