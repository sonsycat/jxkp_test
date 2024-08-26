<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkloadSet_Other.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Input.WorkloadSet_Other" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>
    <script type="text/javascript">
        var SelectorLayout = function () {
            FormPanel1.setHeight(Ext.lib.Dom.getViewHeight() - FormPanel1.getPosition()[1] - 10);
            //SelectorRight.setHeight(Ext.lib.Dom.getViewHeight() - SelectorRight.getPosition()[1]- 5);
        }
        var TwoSideSelector = {
            add: function (source, destination) {
                source = source || GridPanel1;
                destination = destination || GridPanel2;
                if (source.hasSelection()) {
                    destination.store.add(source.selModel.getSelections());
                    source.deleteSelected();
                }
            },
            addAll: function (source, destination) {
                source = source || GridPanel1;
                destination = destination || GridPanel2;
                destination.store.add(source.store.getRange());
                source.store.removeAll();
            },
            addByName: function (name) {
                if (!Ext.isEmpty(name)) {
                    var result = Store1.query("Name", name);
                    if (!Ext.isEmpty(result.items)) {
                        GridPanel2.store.add(result.items[0]);
                        GridPanel1.store.remove(result.items[0]);
                    }
                }
            },
            addByNames: function (name) {
                for (var i = 0; i < name.length; i++) {
                    this.addByName(name[i]);
                }
            },
            remove: function (source, destination) {
                this.add(destination, source);
            },
            removeAll: function (source, destination) {
                this.addAll(destination, source);
            }
        };
    </script>
</head>
<body>
    <form id="form2" runat="server" style="background-color: Transparent">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server">
            <Listeners>
                <DocumentReady Handler=" Ext.EventManager.onWindowResize(SelectorLayout);" />
            </Listeners>
        </ext:ScriptManager>
        <ext:Store ID="Store1" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="item_code">
                    <Fields>
                        <ext:RecordField Name="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" />
                        <ext:RecordField Name="PRICE" />
                        <ext:RecordField Name="ITEM_SPEC" />
                        <ext:RecordField Name="RATIO" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server" OnSubmitData="leibieData">
            <Reader>
                <ext:JsonReader ReaderID="item_code">
                    <Fields>
                        <ext:RecordField Name="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" />
                         <ext:RecordField Name="PRICE" />
                         <ext:RecordField Name="ITEM_SPEC" />
                        <ext:RecordField Name="RATIO" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:Panel ID="Panel3" runat="server" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar3" runat="server">
                                        <Items>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:Button ID="Button5" runat="server" Text="保存" Icon="Disk">
                                                <Listeners>
                                                    <Click Handler="#{GridPanel2}.submitData();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button ID="btnCancle" runat="server" Text="返回" Icon="ArrowUndo">
                                                <AjaxEvents>
                                                    <Click OnEvent="btnCancle_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true">
                                        <ext:LayoutColumn ColumnWidth="0.5">
                                            <ext:GridPanel runat="server" ID="GridPanel1" EnableDragDrop="false" AutoExpandColumn="item_name"
                                                StoreID="Store1">
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="item_name" Header="名称" DataIndex="ITEM_NAME" Sortable="true" />
                                                    </Columns>
                                                    <Columns>
                                                        <ext:Column ColumnID="item_code" Header="编码" DataIndex="ITEM_CODE" Sortable="true" />
                                                    </Columns>
                                                    
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" />
                                                </SelectionModel>
                                                <Plugins>
                                                    <ext:GridFilters ID="GridFilters1" runat="server" Local="true">
                                                        <Filters>
                                                            <ext:StringFilter DataIndex="item_name" />
                                                        </Filters>
                                                    </ext:GridFilters>
                                                </Plugins>
                                                <Listeners>
                                                    <DblClick Handler="TwoSideSelector.add();" />
                                                </Listeners>
                                                <BottomBar>
                                                    <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="20" StoreID="Store1"
                                                        AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                                    </ext:PagingToolbar>
                                                </BottomBar>
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                        <ext:LayoutColumn>
                                            <ext:Panel ID="Panel2" runat="server" Width="30" BodyStyle="background-color: transparent;"
                                                Border="false">
                                                <Body>
                                                    <ext:AnchorLayout ID="AnchorLayout1" runat="server">
                                                        <ext:Anchor Vertical="30%">
                                                            <ext:Panel ID="Panel1" runat="server" Border="false" BodyStyle="background-color: transparent;" />
                                                        </ext:Anchor>
                                                        <ext:Anchor>
                                                            <ext:Panel ID="Panel4" runat="server" Border="false" BodyStyle="padding:5px;background-color: transparent;">
                                                                <Body>
                                                                    <ext:Button ID="Button1" runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                                                                        <Listeners>
                                                                            <Click Handler="TwoSideSelector.add();" />
                                                                        </Listeners>
                                                                        <ToolTips>
                                                                            <ext:ToolTip ID="ToolTip1" runat="server" Title="添加" Html="添加左侧选中行" />
                                                                        </ToolTips>
                                                                    </ext:Button>
                                                                    <ext:Button ID="Button2" runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;">
                                                                        <Listeners>
                                                                            <Click Handler="TwoSideSelector.addAll();" />
                                                                        </Listeners>
                                                                        <ToolTips>
                                                                            <ext:ToolTip ID="ToolTip2" runat="server" Title="添加全部" Html="添加左侧全部" />
                                                                        </ToolTips>
                                                                    </ext:Button>
                                                                    <ext:Button ID="Button3" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                                                                        <Listeners>
                                                                            <Click Handler="TwoSideSelector.remove(GridPanel1, GridPanel2);" />
                                                                        </Listeners>
                                                                        <ToolTips>
                                                                            <ext:ToolTip ID="ToolTip3" runat="server" Title="移除" Html="移除右侧选中行" />
                                                                        </ToolTips>
                                                                    </ext:Button>
                                                                    <ext:Button ID="Button4" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                                                                        <Listeners>
                                                                            <Click Handler="TwoSideSelector.removeAll(GridPanel1, GridPanel2);" />
                                                                        </Listeners>
                                                                        <ToolTips>
                                                                            <ext:ToolTip ID="ToolTip4" runat="server" Title="移除全部" Html="移除右侧全部" />
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
                                            <ext:GridPanel runat="server" ID="GridPanel2" EnableDragDrop="false" AutoExpandColumn="ITEM_NAME"
                                                StoreID="Store2" ClicksToEdit="1">
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar2" runat="server">
                                                        <Items>
                                                            <ext:ComboBox ID="ComboBox1" runat="server" AllowBlank="false" Width="100" Hidden="true">
                                                                <Items>
                                                                    <ext:ListItem Text="住院" Value="0"  />
                                                                    <ext:ListItem Text="门诊" Value="1" />
                                                                </Items>
                                                                <AjaxEvents>
                                                                    <Select OnEvent="Selectedrole">
                                                                        <EventMask ShowMask="true" />
                                                                    </Select>
                                                                </AjaxEvents>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="ComboBox_Role" runat="server" AllowBlank="false" EmptyText="请选择收入项目"
                                                                FieldLabel="收入项目选择" Width="150" Hidden="true">
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
                                                        <ext:Column ColumnID="deptcodeselected" Header="编码" DataIndex="ITEM_CODE"  Sortable="true" />
                                                        <ext:Column ColumnID="deptnameselected" Header="选中项目" DataIndex="ITEM_NAME" Sortable="true" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="edit" runat="server" />
                                                </SelectionModel>
                                                <Listeners>
                                                    <Render Handler=" this.setHeight(Ext.lib.Dom.getViewHeight() - this.getPosition()[1] );" />
                                                    <DblClick Handler="TwoSideSelector.remove(GridPanel1, GridPanel2);" />
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
