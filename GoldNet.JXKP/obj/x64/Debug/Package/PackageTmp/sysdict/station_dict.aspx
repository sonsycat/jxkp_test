<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="station_dict.aspx.cs" Inherits="GoldNet.JXKP.sysdict.station_dict" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .icon-expand-all  { background-image: url(/resources/images/expand-all.gif) !important; }
        .icon-collapse-all  { background-image: url(/resources/images/collapse-all.gif) !important; } 
    </style>
    <script type="text/javascript">
        var PreviewStationTree = function() {
            TreeCtrl.el.mask('预览载入中...', 'x-loading-mask');
            Goldnet.AjaxMethods.Btn_Preview_Click({
                success: function(result) {
                    var nodes = eval(result);
                    TreeCtrl.root.ui.remove();
                    TreeCtrl.initChildren(nodes);
                    TreeCtrl.root.render();
                    TreeCtrl.el.unmask();
                },
                failure: function(msg) {
                    TreeCtrl.el.unmask();
                }
            });
        }


        var RefreshData = function() {
            Store1.reload();
        }
    </script>
</head>
<body>
   <form id="form1" runat="server">
   <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1"  GroupField="TYPENAME"  OnRefreshData="Store_RefreshData"  >
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="TYPENAME"   />
                    <ext:RecordField Name="TYPECODE"   />
                    <ext:RecordField Name="STATION_NAME"   />
                    <ext:RecordField Name="STATION_CODE_REMARK"   />
                    <ext:RecordField Name="SEQUENCE"   Type="Int" />
                    <ext:RecordField Name="INPUT_TIME"  />
                    <ext:RecordField Name="ID"   />    
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div class="x-hide-display">
        <ext:TreePanel ID="TreeCtrl" runat="server" Width="260" Height="350" Icon="BookOpen" AutoScroll="true" RootVisible="true">
            <TopBar>
                <ext:Toolbar ID="ToolBar2" runat="server">
                    <Items>
                     <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="岗位组织一览" />
                            <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                            <ext:ToolbarButton ID="ToolbarButton2" runat="server"  IconCls="icon-expand-all">
                                <Listeners>
                                    <Click Handler="#{TreeCtrl}.root.expand(true);" />
                                </Listeners>
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip5" IDMode="Ignore" runat="server" Html="全部展开" />
                                </ToolTips>
                            </ext:ToolbarButton>
                            <ext:ToolbarButton ID="ToolbarButton3" runat="server" IconCls="icon-collapse-all">
                                <Listeners>
                                    <Click Handler="#{TreeCtrl}.root.collapse(true);" />
                                </Listeners>
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip6" IDMode="Ignore" runat="server" Html="全部收起" />
                                </ToolTips>
                            </ext:ToolbarButton>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <Root>
            <ext:TreeNode NodeID="root" Text="岗位体系"  ></ext:TreeNode>
            </Root>
        </ext:TreePanel>
    </div>
    
    <div>
      <ext:ViewPort ID="ViewPort1" runat="server">
     <Body>
     <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
     <Columns>
        <ext:LayoutColumn ColumnWidth="1">
            <ext:GridPanel 
                ID="GridPanel_List" 
                runat="server" 
                StoreID="Store1" 
                Border="false" 
                AutoWidth="true" 
                StripeRows="true" 
                
                AutoScroll="true">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server" >
                        <Items>
                            <ext:ToolbarButton ID="Btn_Add" runat="server" Text="增加" Icon="Add">
                                <AjaxEvents>
                                    <Click OnEvent="Btn_Add_Click">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_List}.getRowsValues())" Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>                                    
                                    </Click>
                                </AjaxEvents>
                            </ext:ToolbarButton>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                            <ext:ToolbarButton ID="Btn_Edit" runat="server" Text="编辑" Icon="NoteEdit"  Disabled="true" >
                                <AjaxEvents>
                                    <Click OnEvent="Btn_Edit_Click">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_List}.getRowsValues())" Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </Click>
                                </AjaxEvents>
                            </ext:ToolbarButton>
                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server"></ext:ToolbarSeparator>
                            <ext:ToolbarButton ID="Btn_Del" runat="server" Text="删除" Icon="Delete"  Disabled="true" >
                                <AjaxEvents>
                                    <Click OnEvent="Btn_Del_Click">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_List}.getRowsValues())" Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </Click>
                                </AjaxEvents>
                            </ext:ToolbarButton>
                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></ext:ToolbarSeparator>
                            <ext:ToolbarButton ID="Btn_Refresh" runat="server" Text="刷新" Icon="ArrowRefresh">
                                <AjaxEvents>
                                    <Click OnEvent="Btn_Refresh_Click">
                                       <EventMask Msg="重新载入中..." Target="CustomTarget" CustomTarget="GridPanel_List" ShowMask="true" />
                                    </Click>
                                    
                                </AjaxEvents>
                            </ext:ToolbarButton>
                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server"></ext:ToolbarSeparator>
                            <ext:ToolbarButton ID="Btn_Preview" runat="server" Text="预览" Icon="Outline" >
                                <Menu>
                                     <ext:Menu ID="Menu1" runat="server" >
                                        <Items >
                                            <ext:ElementMenuItem Target="#{TreeCtrl}"  Shift="false" />
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                                <Listeners>
                                    <Click Handler="PreviewStationTree();"  />
                                </Listeners>
                            </ext:ToolbarButton>
                            
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <ColumnModel ID="ColumnModel1" runat="server">
                    <Columns>
                        <ext:Column Header="类别" Width="120" Align="left" Sortable="false" ColumnID="TYPENAME"  DataIndex="TYPENAME" Hidden="true" />
                        <ext:Column Header="序号" Width="50" Align="Center" MenuDisabled="true" Sortable="false"  Fixed="true" ColumnID="SEQUENCE" DataIndex="SEQUENCE" />
                        <ext:Column Header="岗位名称" Width="130" Align="left" MenuDisabled="false" Sortable="false"   Groupable="false"   ColumnID="STATION_NAME" DataIndex="STATION_NAME" />
                        <ext:Column Header="岗位说明" Width="320"  Align="left" MenuDisabled="true" Sortable="false"  ColumnID="STATION_CODE_REMARK" DataIndex="STATION_CODE_REMARK" />
                        <ext:Column Header="录入日期" Width="70" Align="Center" MenuDisabled="true" Sortable="false"  ColumnID="INPUT_TIME" DataIndex="INPUT_TIME">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <AjaxEvents>
                    <RowDblClick  OnEvent="Btn_Edit_Click">                    
                        <ExtraParams>
                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_List}.getRowsValues())" Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                    </RowDblClick>
                </AjaxEvents>
                <Plugins>
                    <ext:GridFilters ID="GridFilters1" runat="server" Local="true" FiltersText="过滤" ShowMenu="true">
                        <Filters>
                            <ext:StringFilter DataIndex="STATION_NAME"  />
                        </Filters>
                    </ext:GridFilters>  
                </Plugins>
                <View>
                
                    <ext:GroupingView  
                        ID="GroupingView1"
                        HideGroupedColumn="true"
                        runat="server" 
                        GroupTextTpl='{text} ({[values.rs.length]})'
                        EnableRowBody="false">
                    </ext:GroupingView>
                </View> 
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" SingleSelect="true">
                        <Listeners>
                            <RowSelect Handler="#{Btn_Edit}.enable();#{Btn_Del}.enable()" />
                            <RowDeselect Handler="if (!#{GridPanel_List}.hasSelection()) {#{Btn_Del}.disable();#{Btn_Edit}.disable()}" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <LoadMask ShowMask="true" />
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" StoreID="Store1" PageSize="100"></ext:PagingToolbar>
                </BottomBar>
            </ext:GridPanel>
        </ext:LayoutColumn>
      </Columns>
     </ext:ColumnLayout>
    </body> 
    </ext:ViewPort>
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="岗位详细信息" Width="552"
            Height="510"  AutoShow="false" Modal="true"  AutoScroll="true" CenterOnLoad="true"  ShowOnLoad="false"  Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
            <Listeners>
                <Close Handler="this.clearContent();" />
            </Listeners>
           
    </ext:Window>
    </div>
    </form>
</body>
</html>
