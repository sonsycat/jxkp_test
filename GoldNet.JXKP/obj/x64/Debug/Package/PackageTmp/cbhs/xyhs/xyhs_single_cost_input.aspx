<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_single_cost_input.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.xyhs.xyhs_single_cost_input" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script language="javascript" type="text/javascript">
        var rmbMoney = function(v) {
               if(v==null||v=="")
               {
               return "";
               }
               v = (Math.round((v - 0) * 100)) / 100;
               v = (v == Math.floor(v)) ? v + ".00" : ((v * 10 == Math.floor(v * 10)) ? v + "0" : v);
               v = String(v);
               var ps = v.split('.');
               var whole = ps[0];
               var sub = ps[1] ? '.' + ps[1] : '.00';
               var r = /(\d+)(\d{3})/;
               while (r.test(whole)) {
                   whole = whole.replace(r, '$1' + ',' + '$2');
               }
               v = whole + sub;
               if (v.charAt(0) == '-') {
                   return '-' + v.substr(1);
               }
               return v;
       }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server" OnSubmitData="SubmitData" OnRefreshData="Data_RefreshData"
            WarningOnDirty="false">
            <Reader>
                <ext:JsonReader ReaderID="DEPT_CODE">
                    <Fields>
                        <ext:RecordField Name="FINANCE_ITEM" Type="String" Mapping="FINANCE_ITEM" />
                        <ext:RecordField Name="FINANCE_ITEM_GL" Type="String" Mapping="FINANCE_ITEM_GL" />
                        <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                        <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
                        <ext:RecordField Name="MEDICINE_COSTS" Type="String" Mapping="MEDICINE_COSTS" />
                        <ext:RecordField Name="MANAGE_COSTS" Type="String" Mapping="MANAGE_COSTS" />
                        <ext:RecordField Name="MEMO" Type="String" Mapping="MEMO" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store3" runat="server" WarningOnDirty="false">
            <Reader>
                <ext:JsonReader ReaderID="ITEM_CODE">
                    <Fields>
                        <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                        <ext:RecordField Name="FLAGS" Type="String" Mapping="FLAGS" />
                        <ext:RecordField Name="SUBMIT_PERSONS" Type="String" Mapping="SUBMIT_PERSONS" />
                        <ext:RecordField Name="CHECK_FLAGS" Type="String" Mapping="CHECK_FLAGS" />
                        <ext:RecordField Name="CHECK_NAME" Type="String" Mapping="CHECK_NAME" />
                        <ext:RecordField Name="COMP_FLAGS" Type="String" Mapping="COMP_FLAGS" />
                        <ext:RecordField Name="COMP_NAME" Type="String" Mapping="COMP_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center>
                        <ext:Panel ID="Panel2" runat="server" BodyBorder="true" Border="false">
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                                ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                                        <Items>
                                                            <ext:Label ID="Label1" runat="server" Text="时间：" Width="30" />
                                                            <ext:DateField ID="stardate" runat="server" FieldLabel="时间：" Width="100" EnableKeyEvents="true"
                                                                ReadOnly="true" />
                                                            <ext:KeyNav ID="stardate1" runat="server" Target="stardate">
                                                                <Enter Handler="var str = document.getElementById('stardate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('stardate').value   =   str.replace(reg, '$1-$2-$3');" />
                                                            </ext:KeyNav>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                            <ext:Label ID="Label3" runat="server" Text="成本项目：" Width="60" Hidden="true" />
                                                            <ext:ComboBox ID="COST_ITEM" runat="server" Width="100" AllowBlank="true" EmptyText="请选择..."
                                                                Hidden="true">
                                                                <AjaxEvents>
                                                                    <Select OnEvent="Item_SelectOnChange">
                                                                        <EventMask Msg='载入中...' ShowMask="true" />
                                                                    </Select>
                                                                </AjaxEvents>
                                                            </ext:ComboBox>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" Hidden="true" />
                                                            <ext:Button ID="Button_look" runat="server" Text="查询" Icon="DatabaseGo">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Button_look_click">
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                                            <ext:Button ID="Button_del" runat="server" Text="删除" Icon="Delete" Disabled="true">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Button_del_click">
                                                                        <Confirmation ConfirmRequest="true" Title="系统提示" Message="将删除选中数据,<br/>是否继续?" />
                                                                        <ExtraParams>
                                                                        </ExtraParams>
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                                            </ext:Parameter>
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                                            <ext:Button ID="Button_save" runat="server" Text="保存" Icon="Disk">
                                                                <Listeners>
                                                                    <Click Handler="#{GridPanel1}.submitData();" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server" />
                                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" Width="30" runat="server">
                                                            </ext:ToolbarSpacer>
                                                            <ext:FileUploadField ID="photoimg" Visible="true" runat="server" ButtonOnly="true" Disabled="true"
                                                                ButtonText="导 入" Icon="ImageAdd" Width="70">
                                                                <AjaxEvents>
                                                                    <FileSelected OnEvent="SaveInfoAll" Before="return checkType();">
                                                                    </FileSelected>
                                                                </AjaxEvents>
                                                            </ext:FileUploadField>
                                                            
                                                            <ext:Button ID="Button_get" runat="server" Text="提取用友项目成本" Icon="Disk">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Button_get_click">
                                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="提取将覆盖本月数据,<br/>是否继续?" />
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                    <ext:Column ColumnID="FINANCE_ITEM" Header="<div style='text-align:center;'>医疗业务代码</div>"
                                                            Width="130" Align="left" Sortable="true" DataIndex="FINANCE_ITEM" MenuDisabled="true" />
                                                            <ext:Column ColumnID="FINANCE_ITEM_GL" Header="<div style='text-align:center;'>管理费用代码</div>"
                                                            Width="130" Align="left" Sortable="true" DataIndex="FINANCE_ITEM_GL" MenuDisabled="true" />
                                                        <ext:Column ColumnID="ITEM_CODE" Hidden="true" Header="<div style='text-align:center;'>项目代码</div>"
                                                            Width="130" Align="left" Sortable="true" DataIndex="ITEM_CODE" MenuDisabled="true"  />
                                                        <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>项目名称</div>"
                                                            Width="300" Align="left" Sortable="true" DataIndex="ITEM_NAME" MenuDisabled="true" />
                                                        <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>成本额</div>" Width="130"
                                                            Align="Right" Sortable="false" DataIndex="COSTS" MenuDisabled="true">
                                                            <Renderer Fn="rmbMoney" />
                                                        </ext:Column>
                                                        <ext:Column ColumnID="MEDICINE_COSTS" Header="<div style='text-align:center;'>医疗业务成本</div>"
                                                            Width="130" Align="Right" Sortable="false" DataIndex="MEDICINE_COSTS" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:NumberField ID="NumberField3" runat="server" />
                                                            </Editor>
                                                            <Renderer Fn="rmbMoney" />
                                                        </ext:Column>
                                                        <ext:Column ColumnID="MANAGE_COSTS" Header="<div style='text-align:center;'>管理费用</div>"
                                                            Width="130" Align="Right" Sortable="false" DataIndex="MANAGE_COSTS" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:NumberField ID="NumberField4" runat="server" />
                                                            </Editor>
                                                            <Renderer Fn="rmbMoney" />
                                                        </ext:Column>
                                                        <ext:Column ColumnID="MEMO" Header="<div style='text-align:center;'>备注</div>" Width="150"
                                                            Align="left" Sortable="false" DataIndex="MEMO" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:TextField ID="NumberField2" runat="server" />
                                                            </Editor>
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="RowSelectionModel1" runat="server">
                                                        <Listeners>
                                                            <SelectionChange Handler="#{GridPanel1}.hasSelection()? #{Button_del}.setDisabled(false): #{Button_del}.setDisabled(true);" />
                                                        </Listeners>
                                                    </ext:CheckboxSelectionModel>
                                                </SelectionModel>
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </Center>
                    <East MinWidth="200" MaxWidth="400" SplitTip="成本提交状态信息" Collapsible="false" Split="true">
                        <ext:Panel ID="Panel1" runat="server" Border="false" Width="350" Title="成本提交状态信息"
                            Collapsed="false" AutoScroll="true" Hidden="true">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:Button ID="Button_ok" runat="server" Text="提交" Icon="Disk">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_ok_click">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="将提交选中的成本,<br/>是否继续?" />
                                                    <ExtraParams>
                                                    </ExtraParams>
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator8" runat="server" />
                                        <ext:Button ID="Button1" runat="server" Text="审核" Icon="Disk">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_sh_click">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="将审核选中的成本,<br/>是否继续?" />
                                                    <ExtraParams>
                                                    </ExtraParams>
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator9" runat="server" />
                                        <ext:Button ID="Buttonfh" runat="server" Text="复核" Icon="Disk">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_fh_click">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="将复合选中的成本,<br/>是否继续?" />
                                                    <ExtraParams>
                                                    </ExtraParams>
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server" />
                                        <ext:Button ID="Button_no" runat="server" Text="取消" Icon="Disk">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_no_click">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="将取消选中的成本,<br/>是否继续?" />
                                                    <ExtraParams>
                                                    </ExtraParams>
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store3" StripeRows="true"
                                                AutoHeight="true" AutoWidth="true" TrackMouseOver="true" AutoScroll="true">
                                                <ColumnModel ID="ColumnModel2" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="ITEM_CODE" Hidden="true" />
                                                        <ext:Column Header="成本项目" Width="100" ColumnID="ITEM_NAME" DataIndex="ITEM_NAME"
                                                            Sortable="false" MenuDisabled="true" />
                                                        <ext:Column Header="状态" Width="60" ColumnID="FLAGS" DataIndex="FLAGS" Sortable="false"
                                                            MenuDisabled="true" />
                                                        <ext:Column Header="提交人" Width="60" ColumnID="SUBMIT_PERSONS" DataIndex="SUBMIT_PERSONS"
                                                            Sortable="false" MenuDisabled="true" />
                                                        <ext:Column Header="审核人" Width="60" ColumnID="CHECK_NAME" DataIndex="CHECK_NAME"
                                                            Sortable="false" MenuDisabled="true" />
                                                        <ext:Column Header="复核人" Width="60" ColumnID="COMP_NAME" DataIndex="COMP_NAME" Sortable="false"
                                                            MenuDisabled="true" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="RowSelectionModel2" runat="server">
                                                    </ext:CheckboxSelectionModel>
                                                </SelectionModel>
                                                <Listeners>
                                                </Listeners>
                                                <LoadMask ShowMask="true" />
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </East>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
