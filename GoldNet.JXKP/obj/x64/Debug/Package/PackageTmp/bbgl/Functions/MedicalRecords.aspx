<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MedicalRecords.aspx.cs"
    Inherits="GoldNet.JXKP.bbgl.Functions.MedicalRecords" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Columns1" />
                    <ext:RecordField Name="Columns2" />
                    <ext:RecordField Name="Columns3" />
                    <ext:RecordField Name="Columns4" />
                    <ext:RecordField Name="Columns5" />
                    <ext:RecordField Name="Columns6" />
                    <ext:RecordField Name="Columns7" />
                    <ext:RecordField Name="Columns8" />
                    <ext:RecordField Name="Columns9" />
                    <ext:RecordField Name="Columns10" />
                    <ext:RecordField Name="Columns11" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Border="false"
                                AutoWidth="true" Height="591" Title="" MonitorResize="true" MonitorWindowResize="true"
                                StripeRows="true" TrackMouseOver="true">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_ZLJK" runat="server" Visible="true">
                                        <Items>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="10" />
                                            <ext:Label ID="Label7" runat="server" Text="患者姓名: ">
                                            </ext:Label>
                                            <ext:TextField runat="server" ID="name"></ext:TextField>
                                            <ext:Label ID="Label6" runat="server" Text=" 病案号：  ">
                                            </ext:Label>
                                            <ext:TextField runat="server" ID="id"></ext:TextField>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                            <ext:Button ID="Button1" runat="server" Text=" 查询 " Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <%--                            <Click OnEvent="GetQueryPortalet">                            <EventMask Msg="载入中..." ShowMask="true" />                            </Click>--%>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="<center>姓名</center>" Sortable="true" ColumnID="Columns1" DataIndex="Columns1"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>医疗账号</center>" Sortable="true" ColumnID="Columns2" DataIndex="Columns2"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>病案号</center>" Sortable="true" ColumnID="Columns3" DataIndex="Columns3"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>性别</center>" Sortable="true" ColumnID="Columns4" DataIndex="Columns4"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>人员类别</center>" Sortable="true" ColumnID="Columns5" DataIndex="Columns5"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>所属单位</center>" Sortable="true" ColumnID="Columns6" DataIndex="Columns6"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>主要疾病诊断</center>" Sortable="true" ColumnID="Columns7"
                                            DataIndex="Columns7" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>手术操作名称序号</center>" Sortable="true" ColumnID="Columns8"
                                            DataIndex="Columns8" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>入院时间</center>" Sortable="true" ColumnID="Columns9" DataIndex="Columns9"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>出院时间</center>" Sortable="true" ColumnID="Columns10" DataIndex="Columns10"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>实际费用</center>" Sortable="true" ColumnID="Columns11" DataIndex="Columns11"
                                            Align="center" Width="116">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true" />
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                                <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="25" StoreID="Store1"
                                        AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                    </ext:PagingToolbar>
                                </BottomBar>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
