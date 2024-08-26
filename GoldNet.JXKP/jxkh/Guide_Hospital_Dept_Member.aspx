<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Guide_Hospital_Dept_Member.aspx.cs" Inherits="GoldNet.JXKP.jxkh.Guide_Hospital_Dept_Member" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>指标关联对照列表</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server"  />
        <ext:Store runat="server" ID="Store1"  AutoLoad="true" OnRefreshData="Store_RefreshData" WarningOnDirty="false" >
            <Reader>
                <ext:JsonReader ReaderID="SERIAL_NO">
                    <Fields>
                        <ext:RecordField Name="SERIAL_NO" />
                        <ext:RecordField Name="HOSPITAL_GUIDE_CODE" />
                        <ext:RecordField Name="HOSPITAL_GUIDE_NAME" />
                        <ext:RecordField Name="DEPT_GUIDE_CODE" />
                        <ext:RecordField Name="DEPT_GUIDE_NAME" />
                        <ext:RecordField Name="MEMBER_GUIDE_CODE" />
                        <ext:RecordField Name="MEMBER_GUIDE_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreComboY" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="GUIDE_CODE">
                    <Fields>
                        <ext:RecordField Name="GUIDE_CODE" />
                        <ext:RecordField Name="GUIDE_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreComboK" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="GUIDE_CODE">
                    <Fields>
                        <ext:RecordField Name="GUIDE_CODE" />
                        <ext:RecordField Name="GUIDE_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreComboR" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="GUIDE_CODE">
                    <Fields>
                        <ext:RecordField Name="GUIDE_CODE" />
                        <ext:RecordField Name="GUIDE_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel_List" runat="server" Border="false"   StoreID="Store1" StripeRows="true" TrackMouseOver="true" Height="480" AutoWidth="true" AutoExpandColumn="HOSPITAL_GUIDE_NAME">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server" >
                                        <Items>
                                            <ext:ToolbarButton ID="Btn_Add" runat="server" Text="增加" Icon="Add" >
                                                <AjaxEvents>
                                                    <Click OnEvent ="Btn_Add_Click" >
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server"></ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Edit" runat="server" Text="编辑" Icon="NoteEdit"  Disabled="true" >
                                                <AjaxEvents>
                                                    <Click OnEvent ="Btn_Edit_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
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
                                                    <Click OnEvent="Btn_Del_Click" >
                                                        <Confirmation Title="系统提示" BeforeConfirm="config.confirmation.message = '确定要删除本条指标关联对照吗？';"  ConfirmRequest="true" />                                                        
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="GridPanel_List.getSelectionModel().getSelected().data.SERIAL_NO" Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Refresh" runat="server" Text="刷新" Icon="ArrowRefresh">
                                                <Listeners>
                                                    <Click Handler="#{Store1}.reload();"  />
                                                </Listeners>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server"></ext:ToolbarSeparator>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>                                
                                
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:RowNumbererColumn  Width="32" Resizable="true"></ext:RowNumbererColumn>
                                        <ext:Column Header="院指标代码"        Width="85"  ColumnID="HOSPITAL_GUIDE_CODE"   DataIndex="HOSPITAL_GUIDE_CODE" />
                                        <ext:Column Header="院指标名称"        Width="150"  ColumnID="HOSPITAL_GUIDE_NAME"   DataIndex="HOSPITAL_GUIDE_NAME" />
                                        <ext:Column Header="科指标代码"           Width="85"  ColumnID="DEPT_GUIDE_CODE"        DataIndex="DEPT_GUIDE_CODE" />
                                        <ext:Column Header="科指标名称"         Width="180"  ColumnID="DEPT_GUIDE_NAME"     DataIndex="DEPT_GUIDE_NAME" />
                                        <ext:Column Header="人指标代码"    Width="85"   ColumnID="MEMBER_GUIDE_CODE"         DataIndex="MEMBER_GUIDE_CODE" />
                                        <ext:Column Header="人指标名称"      Width="180"   ColumnID="MEMBER_GUIDE_NAME"            DataIndex="MEMBER_GUIDE_NAME" />                                        
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <Listeners>
                                            <SelectionChange Handler="var tmpflg=  #{GridPanel_List}.hasSelection()?false:true;   #{Btn_Edit}.setDisabled(tmpflg);  #{Btn_Del}.setDisabled(tmpflg);" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="20" StoreID="Store1"   AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                   
                                    </ext:PagingToolbar>
                                </BottomBar> 
                                <LoadMask ShowMask="true" Msg="载入中..." />
                                 <AjaxEvents>
                                    <RowDblClick OnEvent ="Btn_Edit_Click">
                                        <EventMask Msg="载入中..." ShowMask="true" />
                                         <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_List}.getRowsValues())" Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </RowDblClick>
                                </AjaxEvents>
                            </ext:GridPanel>
                            
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>        
           <ext:Window ID="DetailWin" runat="server" Icon="PageEdit" Title="指标关联对照" Width="300" Resizable="false" 
             Height="170"  AutoShow="false" Modal="true"  AutoScroll="true" CenterOnLoad="true"  ShowOnLoad="false" >
            <Body>
                <ext:FormPanel ID="FormPanel1"   runat="server" AutoWidth="true"  Header="false"    LabelWidth="90" BodyStyle="padding:5px;background-color:Transparent; border-left:0px;border-top:0px;border-right:0px;" ButtonAlign="Right" >
                    <Body>
                        <ext:FormLayout ID="FormLayout1" runat="server" >
                        <ext:Anchor Horizontal="95%">
                            <ext:ComboBox ID="GuideYField" runat="server" FieldLabel="院指标"  EmptyText="无关联" ListWidth="200"  Mode="Local" MinChars="1"
                                AllowBlank="true"  StoreID="StoreComboY"   DisplayField="GUIDE_NAME" ValueField="GUIDE_CODE" >
                                <Listeners>
                                <Change Handler="#{GuideYFieldHide}.setValue(this.value);" />
                                </Listeners>
                            </ext:ComboBox>
                        </ext:Anchor> 
                        <ext:Anchor Horizontal="95%">
                            <ext:ComboBox ID="GuideKField" runat="server" FieldLabel="科指标"  EmptyText="无关联"  Mode="Local" MinChars="1"
                                AllowBlank="true"  StoreID="StoreComboK"  DisplayField="GUIDE_NAME" ValueField="GUIDE_CODE" >
                                <Listeners>
                                <Change Handler="#{GuideKFieldHide}.setValue(this.value);" />
                                </Listeners>
                            </ext:ComboBox>
                        </ext:Anchor>
                        <ext:Anchor Horizontal="95%">
                            <ext:ComboBox ID="GuideRField" runat="server" FieldLabel="人指标"  EmptyText="无关联"  Mode="Local" MinChars="1"
                                AllowBlank="true"  StoreID="StoreComboR" DisplayField="GUIDE_NAME" ValueField="GUIDE_CODE" >
                                <Listeners>
                                <Change Handler="#{GuideRFieldHide}.setValue(this.value);" />
                                </Listeners>
                            </ext:ComboBox>
                        </ext:Anchor>
                        </ext:FormLayout>
                        <ext:Hidden ID="SerialNoHidden" runat="server"></ext:Hidden>
                        <ext:Hidden ID="GuideYFieldHide" runat="server"></ext:Hidden>
                        <ext:Hidden ID="GuideKFieldHide" runat="server"></ext:Hidden>
                        <ext:Hidden ID="GuideRFieldHide" runat="server"></ext:Hidden>                        
                    </Body>
                    <Buttons>
                        <ext:Button runat="server" ID="Btn_Save" Text="保存"  Icon="Disk">
                        <AjaxEvents>
                            <Click OnEvent="Btn_Save_Click" >
                                <EventMask Msg="正在保存" ShowMask="false" />
                            </Click>
                        </AjaxEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="Btn_Cancel" Text="取消" Icon="Cancel">
                        <Listeners>
                            <Click Handler="#{DetailWin}.hide();" />
                        </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Body>           
        </ext:Window>

    </div>
    </form>
</body>
</html>
