<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Per_Type_Power.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SpecManager.Per_Type_Power" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
 var SelectorLayout = function() {
             FormPanel1.setHeight(Ext.lib.Dom.getViewHeight() - FormPanel1.getPosition()[1]- 10);
             //SelectorRight.setHeight(Ext.lib.Dom.getViewHeight() - SelectorRight.getPosition()[1]- 5);
         }
        var TwoSideSelector = {
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
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    <Listeners>
            <DocumentReady Handler=" Ext.EventManager.onWindowResize(SelectorLayout);" />
        </Listeners>
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="ROLE_ID">
                <Fields>
                    <ext:RecordField Name="ROLE_ID" />
                    <ext:RecordField Name="ROLE_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" OnSubmitData="SubmitData">
        <Reader>
            <ext:JsonReader ReaderID="ROLE_ID">
                <Fields>
                    <ext:RecordField Name="ROLE_ID" />
                    <ext:RecordField Name="ROLE_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <form id="form1" runat="server" style="background-color: Transparent">
            <div>
                <ext:FormPanel ID="FormPanel1" runat="server" Border="false" Height="400" Width="480"
                    AutoScroll="true" ButtonAlign="Right" StyleSpec="background-color:transparent"
                    BodyStyle="background-color:transparent">
                    <Body>
                        <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                            StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Button1" runat="server" Text="保存" Icon="Disk">
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
                        </ext:Panel>
                       
                        <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true">
                            <ext:LayoutColumn ColumnWidth="0.5">
                                <ext:GridPanel runat="server" ID="GridPanel1" EnableDragDrop="false" AutoExpandColumn="ROLE_NAME"
                                    StoreID="Store1">
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <ext:Column ColumnID="roleid" Header="ROLE_ID" DataIndex="ROLE_ID" Hidden="true" />
                                        </Columns>
                                        <Columns>
                                            <ext:Column ColumnID="rolename" Header="角色" DataIndex="ROLE_NAME" Sortable="true" />
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" />
                                    </SelectionModel>
                                    <Plugins>
                                        <ext:GridFilters ID="GridFilters1" runat="server" Local="true">
                                            <Filters>
                                                <ext:StringFilter DataIndex="ROLE_NAME" />
                                            </Filters>
                                        </ext:GridFilters>
                                    </Plugins>
                                    <Listeners>
                                        <DblClick Handler="TwoSideSelector.add();" />
                                    </Listeners>
                                </ext:GridPanel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn>
                                <ext:Panel ID="Panel2" runat="server" Width="30" BodyStyle="background-color: transparent;"
                                    Border="false">
                                    <Body>
                                        <ext:AnchorLayout ID="AnchorLayout1" runat="server">
                                            <ext:Anchor Vertical="30%">
                                                <ext:Panel ID="Panel3" runat="server" Border="false" BodyStyle="background-color: transparent;" />
                                            </ext:Anchor>
                                            <ext:Anchor>
                                                <ext:Panel ID="Panel4" runat="server" Border="false" BodyStyle="padding:5px;background-color: transparent;">
                                                    <Body>
                                                        <ext:Button ID="Button2" runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                                                            <Listeners>
                                                                <Click Handler="TwoSideSelector.add();" />
                                                            </Listeners>
                                                            <ToolTips>
                                                                <ext:ToolTip ID="ToolTip1" runat="server" Title="添加" Html="添加左侧选中行" />
                                                            </ToolTips>
                                                        </ext:Button>
                                                        <ext:Button ID="Button3" runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;">
                                                            <Listeners>
                                                                <Click Handler="TwoSideSelector.addAll();" />
                                                            </Listeners>
                                                            <ToolTips>
                                                                <ext:ToolTip ID="ToolTip2" runat="server" Title="添加全部" Html="添加左侧全部" />
                                                            </ToolTips>
                                                        </ext:Button>
                                                        <ext:Button ID="Button4" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                                                            <Listeners>
                                                                <Click Handler="TwoSideSelector.remove(GridPanel1, GridPanel2);" />
                                                            </Listeners>
                                                            <ToolTips>
                                                                <ext:ToolTip ID="ToolTip3" runat="server" Title="移除" Html="移除右侧选中行" />
                                                            </ToolTips>
                                                        </ext:Button>
                                                        <ext:Button ID="Button5" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
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
                                <ext:GridPanel runat="server" ID="GridPanel2" EnableDragDrop="false" AutoExpandColumn="ROLE_NAME"
                                    StoreID="Store2">
                                    <TopBar>
                                <ext:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <ext:Label ID="role" runat="server" Text="人员类别：">
                                        </ext:Label>
                                        <ext:ComboBox ID="ComboBox_Pertype" runat="server" AllowBlank="false" EmptyText="请选择人员类别"
                                            FieldLabel="人员类别选择" Width="120">
                                            <AjaxEvents>
                                                <Select OnEvent="SelectedPertype">
                                                    <EventMask ShowMask="true" />
                                                </Select>
                                            </AjaxEvents>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                                    <ColumnModel ID="ColumnModel2" runat="server">
                                        <Columns>
                                            <ext:Column ColumnID="roleidselected" Header="id" DataIndex="ROLE_ID" Hidden="true" />
                                            <ext:Column ColumnID="rolenameselected" Header="选中角色" DataIndex="ROLE_NAME" Sortable="true" />
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
                </ext:FormPanel>
            </div>
            </form>
        </Body>
    </ext:ViewPort>
</body>
</html>
