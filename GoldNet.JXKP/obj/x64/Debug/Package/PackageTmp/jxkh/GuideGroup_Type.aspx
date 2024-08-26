<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideGroup_Type.aspx.cs" Inherits="GoldNet.JXKP.jxkh.GuideGroup_Type" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>指标集类别字典</title>
    <script type="text/javascript">
        function FormatRender(v, p, record, rowIndex) {
            var a = Number(record.data.GUIDE_GROUP_TYPE.toString().substr(1, 1));
            var colors = ["red", "blue", "purple", "lime", "green", "navy","olive", "black","yellow","maroon"];
            var template = '<span style="color:{0};">{1}</span>';
            return String.format(template, colors[a], record.data.P_TYPENAME);
        }
        function FormatRenderAlluse(v, p, record, rowIndex) {
            if (record.data.ALLUSE == null) return;
            var template = '<span style="color:red">{0}</span>';
            return String.format(template,  record.data.ALLUSE);
        }        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server"  />
        <ext:Store runat="server" ID="Store1"  AutoLoad="true" GroupField="P_TYPE" OnRefreshData="Store_RefreshData" >
            <Reader>
                <ext:JsonReader ReaderID="ID">
                    <Fields>
                        <ext:RecordField Name="P_TYPE" />
                        <ext:RecordField Name="P_TYPENAME" />
                        <ext:RecordField Name="GUIDE_GROUP_TYPE" />
                        <ext:RecordField Name="GUIDE_GROUP_TYPE_NAME" />
                        <ext:RecordField Name="ALLUSE" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreCombo" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="P_TYPE">
                    <Fields>
                        <ext:RecordField Name="P_TYPE" />
                        <ext:RecordField Name="P_TYPENAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>        
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel_List" runat="server" Border="false"   StoreID="Store1"
                             StripeRows="true" TrackMouseOver="true" Height="480" AutoWidth="true" >
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server" >
                                        <Items>
                                            <ext:ToolbarButton ID="Btn_Add" runat="server" Text="增加" Icon="Add" >
                                                <AjaxEvents>
                                                    <Click OnEvent ="Btn_Add_Click"></Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server"></ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Edit" runat="server" Text="编辑" Icon="NoteEdit"  Disabled="true" >
                                                <AjaxEvents>
                                                    <Click OnEvent ="Btn_Edit_Click">
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
                                                        <Confirmation Title="系统提示" BeforeConfirm="config.confirmation.message = '确定要删除指标集类别 '+GridPanel_List.getSelectionModel().getSelected().data.GUIDE_GROUP_TYPE_NAME+' 吗？';"  ConfirmRequest="true" />                                                        
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="GridPanel_List.getSelectionModel().getSelected().data.GUIDE_GROUP_TYPE" Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Refresh" runat="server" Text="刷新" Icon="ArrowRefresh">
                                                <Listeners>
                                                    <Click Handler="#{Store1}.reload();" />
                                                </Listeners>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server"></ext:ToolbarSeparator>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>                                
                                
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:RowNumbererColumn />
                                        <ext:Column Header="分类"           Width="110"   ColumnID="P_TYPE"   DataIndex="P_TYPE"  MenuDisabled="true">
                                            <Renderer  Fn="FormatRender" />
                                        </ext:Column>
                                        <ext:Column Header="类别编码"      Width="90"   ColumnID="GUIDE_GROUP_TYPE"   DataIndex="GUIDE_GROUP_TYPE"  MenuDisabled="true" />
                                        <ext:Column Header="指标集类型名称"     Width="200"  ColumnID="GUIDE_GROUP_TYPE_NAME"   DataIndex="GUIDE_GROUP_TYPE_NAME"  MenuDisabled="true" />
                                        <ext:Column Header="是否通用"       Width="90"   ColumnID="ALLUSE"        DataIndex="ALLUSE"  MenuDisabled="true">
                                            <Renderer Fn="FormatRenderAlluse" />
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <Listeners>
                                            <SelectionChange Handler="var tmpflg=  #{GridPanel_List}.hasSelection()?false:true;   #{Btn_Edit}.setDisabled(tmpflg);  #{Btn_Del}.setDisabled(tmpflg);" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                                <View>
                                    <ext:GroupingView  
                                        ID="GroupingView1"
                                        HideGroupedColumn="true"
                                        runat="server" 
                                        GroupTextTpl='{text} ({[values.rs.length]})'
                                        EnableRowBody="false">
                                    </ext:GroupingView>
                                </View>
                                <AjaxEvents>
                                    <RowDblClick OnEvent ="Btn_Edit_Click">
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
        
        <ext:Window ID="DetailWin" runat="server" Icon="PageEdit" Title="详细信息" Width="330" 
             Height="190"  AutoShow="false" Modal="true"  AutoScroll="true" CenterOnLoad="true"  ShowOnLoad="false" 
             Resizable="false" >
            <Body>
                <ext:FormPanel ID="FormPanel1"   runat="server" AutoWidth="true"  Header="false"    LabelWidth="120" BodyStyle="padding:5px;background-color:Transparent; border-left:0px;border-top:0px;border-right:0px;" ButtonAlign="Right" >
                    <Body>
                        <ext:FormLayout runat="server" >
                        <ext:Anchor Horizontal="95%">
                            <ext:Label ID="CodeField" runat="server" FieldLabel="类别编码" />
                        </ext:Anchor>
                        <ext:Anchor Horizontal="95%">
                            <ext:ComboBox ID="TypeField"  runat="server" FieldLabel="所属分类" Editable="false" 
                                AllowBlank="false" CausesValidation="true" BlankText="请选择分类"  StoreID="StoreCombo" DisplayField="P_TYPENAME" ValueField="P_TYPE" >
                                <Listeners>
                                <Select Handler="#{TypeFieldHide}.setValue(this.value);" />
                                </Listeners>
                            </ext:ComboBox>
                        </ext:Anchor>
                        <ext:Anchor Horizontal="95%">
                            <ext:TextField ID="NameField" runat="server"  FieldLabel="指标集类别名称" AllowBlank="false" CausesValidation="true" BlankText="请输入指标集类别名称" />
                        </ext:Anchor>
                        <ext:Anchor Horizontal="95%">
                            <ext:Checkbox ID="UseallField" runat="server" FieldLabel="作为通用指标集类别" ></ext:Checkbox>
                        </ext:Anchor>
                        </ext:FormLayout>
                        <ext:Hidden ID="CodeFieldHide" runat="server"></ext:Hidden>
                        <ext:Hidden ID="TypeFieldHide" runat="server"></ext:Hidden>                        
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
