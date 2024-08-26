<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutRestNonus.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Input.OutRestNonus" %>

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
        <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData" WarningOnDirty="false">
            <Reader>
                <ext:JsonReader ReaderID="ID">
                    <Fields>
                        <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                        <ext:RecordField Name="WEEKS" Type="String" Mapping="WEEKS" />
                        <ext:RecordField Name="FORMAL_AM" Type="Float" Mapping="FORMAL_AM" />
                        <ext:RecordField Name="FORMAL_PM" Type="Float" Mapping="FORMAL_PM" />
                        <ext:RecordField Name="TEMPORARY_AM" Type="Float" Mapping="TEMPORARY_AM" />
                        <ext:RecordField Name="TEMPORARY_PM" Type="Float" Mapping="TEMPORARY_PM" />
                        <ext:RecordField Name="SUMNUB" Type="Float" Mapping="SUMNUB" />
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
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
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
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server" />
                                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" Width="30" runat="server">
                                                            </ext:ToolbarSpacer>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="DEPT_CODE" Header="<div style='text-align:center;'>科室代码</div>"
                                                            Width="80" Align="left" Sortable="true" Hidden="true" DataIndex="DEPT_CODE" MenuDisabled="true" />
                                                        <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室</div>"
                                                            Width="130" Align="left" Sortable="true" DataIndex="DEPT_NAME" MenuDisabled="true" />
                                                        <ext:Column ColumnID="WEEKS" Header="<div style='text-align:center;'>类型</div>" Width="130"
                                                            Align="left" Sortable="true" DataIndex="WEEKS" MenuDisabled="true" />
                                                        <ext:Column ColumnID="FORMAL_AM" Header="<div style='text-align:center;'>正式上午</div>"
                                                            Width="80" Align="Right" Sortable="false" DataIndex="FORMAL_AM" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:NumberField ID="NumberField1" runat="server" />
                                                            </Editor>
                                                            <Renderer Fn="rmbMoney" />
                                                        </ext:Column>
                                                        <ext:Column ColumnID="FORMAL_PM" Header="<div style='text-align:center;'>正式下午</div>"
                                                            Width="80" Align="Right" Sortable="false" DataIndex="FORMAL_PM" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:NumberField ID="NumberField2" runat="server" />
                                                            </Editor>
                                                            <Renderer Fn="rmbMoney" />
                                                        </ext:Column>
                                                        <ext:Column ColumnID="TEMPORARY_AM" Header="<div style='text-align:center;'>临时上午</div>"
                                                            Width="80" Align="Right" Sortable="false" DataIndex="TEMPORARY_AM" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:NumberField ID="NumberField3" runat="server" />
                                                            </Editor>
                                                            <Renderer Fn="rmbMoney" />
                                                        </ext:Column>
                                                        <ext:Column ColumnID="TEMPORARY_PM" Header="<div style='text-align:center;'>临时下午</div>"
                                                            Width="80" Align="Right" Sortable="false" DataIndex="TEMPORARY_PM" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:NumberField ID="NumberField4" runat="server" />
                                                            </Editor>
                                                            <Renderer Fn="rmbMoney" />
                                                        </ext:Column>
                                                        <ext:Column ColumnID="SUMNUB" Header="<div style='text-align:center;'>合计</div>" Width="80"
                                                            Align="Right" Sortable="false" DataIndex="SUMNUB" MenuDisabled="true">
                                                            <Renderer Fn="rmbMoney" />
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
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
