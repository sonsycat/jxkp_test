<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="single_cost_input_new_one.aspx.cs" Inherits="GoldNet.JXKP.cbhs.datagather.single_cost_input_new_one" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader >
                <Fields>
                    <ext:RecordField Name="ST_DATE" Type="String" Mapping="ST_DATE" />
                    <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                    <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                    <ext:RecordField Name="COSTS" Type="String" Mapping="COSTS" />
                    <ext:RecordField Name="ROWS_ID" Type="String" Mapping="ROWS_ID" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="1">
                        <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                            TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                    <Items>
                                        <ext:DateField ID="stardate" runat="server" FieldLabel="选择日期：" Width="100" EnableKeyEvents="true"
                                            ReadOnly="true" />
                                        <ext:KeyNav ID="stardate1" runat="server" Target="stardate">
                                            <Enter Handler="var str = document.getElementById('stardate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('stardate').value   =   str.replace(reg, '$1-$2-$3');" />
                                        </ext:KeyNav>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                        
                                        <ext:FileUploadField ID="photoimg" runat="server" ButtonOnly="true" ButtonText="导入"
                                            Icon="ImageAdd" Hidden="true">
                                            <AjaxEvents >
                                                <FileSelected OnEvent="UploadClick" Timeout="99999999">
                                                </FileSelected>
                                            </AjaxEvents>
                                        </ext:FileUploadField>

                                       
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="ID" Hidden="true" />
                                    
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    <Listeners>
                                        <SelectionChange Handler="if (#{GridPanel1}.hasSelection()){#{Button_del}.setDisabled(false);#{Button_update}.setDisabled(false)}else { #{Button_del}.setDisabled(true); #{Button_update}.setDisabled(true);}    " />
                                    </Listeners>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="20" StoreID="Store1" />
                            </BottomBar>
                            <Listeners>
                                <Command Handler="edit(record.data.ID);" />
                            </Listeners>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="成本" Width="370" Height="410"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
     <ext:Window runat="server" ID="Win_BatchInit" AutoShow="false" ShowOnLoad="false"
            Modal="true" Resizable="false" Title="年度批量指标量化" CenterOnLoad="true" AutoScroll="false"
            Width="280" Height="180" CloseAction="Hide" AnimateTarget="Btn_BatInit" Icon="TagPink"
            BodyStyle="padding:2px;">
            <Body>
                <table>
                    <tr>
                        <td colspan="2" align="left">
                            <p>
                                注意：生成数据需要大约2分钟时间，在此时间内请不要关闭您的浏览器或者刷新页面。</p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            目标值参照年份:
                        </td>
                        <td align="left">
                             <ext:DateField ID="DateField1" runat="server" FieldLabel="选择日期：" Width="100" EnableKeyEvents="true"
                                            ReadOnly="true" />
                                        <ext:KeyNav ID="KeyNav1" runat="server" Target="stardate1">
                                            <Enter Handler="var str = document.getElementById('stardate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('stardate').value   =   str.replace(reg, '$1-$2-$3');" />
                              </ext:KeyNav>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <p>
                                <ext:Label runat="server" ID="progressTip" Text="进度" AutoWidth="true">
                                </ext:Label>
                            </p>
                            <ext:ProgressBar ID="Progress1" runat="server" Width="255">
                            </ext:ProgressBar>
                        </td>
                    </tr>
                </table>
                <ext:TaskManager ID="TaskManager1" runat="server">
                    <Tasks>
                        <ext:Task TaskID="longactionprogress" Interval="1000" AutoRun="false" OnStart="#{Btn_BatStart}.setDisabled(true); "
                            OnStop="#{Btn_BatStart}.setDisabled(false);">
                            <AjaxEvents>
                                <Update OnEvent="RefreshProgress" />
                            </AjaxEvents>
                        </ext:Task>
                    </Tasks>
                </ext:TaskManager>
            </Body>
            <BottomBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                        <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="PlayGreen" Text="开始生成">
                            <AjaxEvents>
                                <Click OnEvent="Btn_BatStart_Click" Timeout="1200000">
                                </Click>
                            </AjaxEvents>
                        </ext:ToolbarButton>
                        <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server" />
                        <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="退出">
                            <Listeners>
                                <Click Handler="Win_BatchInit.hide();" />
                            </Listeners>
                        </ext:ToolbarButton>
                    </Items>
                </ext:Toolbar>
            </BottomBar>
            <Listeners>
                <Show Handler="this.dirty = false;" />
                <BeforeHide Handler="
                    if ((this.dirty==false)&&(Btn_BatCancel.text=='取消') ){
                        Ext.Msg.confirm('系统提示', '注意:任务正在运行，确定取消任务并退出吗？', function (btn) { 
                            if(btn == 'yes') { 
                                this.dirty = true;
                                this.hide(); 
                            } 
                        }, this);
                        return false;    
                    }" />
                <Hide Handler="TaskManager1.stopAll();" />
            </Listeners>
            <AjaxEvents>
                <Hide OnEvent="CloseBatInit" />
            </AjaxEvents>
        </ext:Window>
    </form>
</body>
</html>
