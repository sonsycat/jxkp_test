<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptGradeBrowse.aspx.cs"
    Inherits="GoldNet.JXKP.bbgl.MedicalIndicators.DeptGradeBrowse" %>

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
                    <ext:RecordField Name="Columns12" />
                    <ext:RecordField Name="Columns13" />
                    <ext:RecordField Name="Columns14" />
                    <ext:RecordField Name="Columns15" />
                    <ext:RecordField Name="Columns16" />
                    <ext:RecordField Name="Columns17" />
                    <ext:RecordField Name="Columns18" />
                    <ext:RecordField Name="Columns19" />
                    <ext:RecordField Name="Columns20" />
                    <ext:RecordField Name="Columns21" />
                    <ext:RecordField Name="Columns22" />
                    <ext:RecordField Name="Columns23" />
                    <ext:RecordField Name="Columns24" />
                    <ext:RecordField Name="Columns25" />
                    <ext:RecordField Name="Columns26" />
                    <ext:RecordField Name="Columns27" />
                    <ext:RecordField Name="Columns28" />
                    <ext:RecordField Name="Columns29" />
                    <ext:RecordField Name="Columns30" />
                    <ext:RecordField Name="Columns31" />
                    <ext:RecordField Name="Columns32" />
                    <ext:RecordField Name="Columns33" />
                    <ext:RecordField Name="Columns34" />
                    <ext:RecordField Name="Columns35" />
                    <ext:RecordField Name="Columns36" />
                    <ext:RecordField Name="Columns37" />
                    <ext:RecordField Name="Columns38" />
                    <ext:RecordField Name="Columns39" />
                    <ext:RecordField Name="Columns40" />
                    <ext:RecordField Name="Columns41" />
                    <ext:RecordField Name="Columns42" />
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
                                            <ext:Label ID="Label7" runat="server" Text="统计日期:   月份">
                                            </ext:Label>
                                            <ext:DateField runat="server" ID="dd1" Vtype="daterange" AllowBlank="false" Format="Ym"
                                                MaxLength="6" Width="100">
                                            </ext:DateField>
                                            <ext:Label ID="Label6" runat="server" Text="   至   ">
                                            </ext:Label>
                                            <ext:DateField runat="server" ID="dd2" Vtype="daterange" AllowBlank="false" Format="Ym"
                                                MaxLength="6" Width="100">
                                                <Listeners>
                                                    <Render Handler="this.startDateField = '#{dd1}'" />
                                                </Listeners>
                                            </ext:DateField>
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
                                        <ext:Column Header="<center>科室</center>" Sortable="true" ColumnID="Columns1" DataIndex="Columns1"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>入院人数</center>" Sortable="true" ColumnID="Columns2" DataIndex="Columns2"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>占全院_入院人数</center>" Sortable="true" ColumnID="Columns3"
                                            DataIndex="Columns3" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>出院人数</center>" Sortable="true" ColumnID="Columns4" DataIndex="Columns4"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>占全院_出院人数</center>" Sortable="true" ColumnID="Columns5"
                                            DataIndex="Columns5" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>平均住院日</center>" Sortable="true" ColumnID="Columns6" DataIndex="Columns6"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>治愈者平均住院日</center>" Sortable="true" ColumnID="Columns7"
                                            DataIndex="Columns7" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>术前平均住院日</center>" Sortable="true" ColumnID="Columns8"
                                            DataIndex="Columns8" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>平均每日占床数</center>" Sortable="true" ColumnID="Columns9"
                                            DataIndex="Columns9" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>平均每日占床数_占全院</center>" Sortable="true" ColumnID="Columns10"
                                            DataIndex="Columns10" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>编制床使用率</center>" Sortable="true" ColumnID="Columns11"
                                            DataIndex="Columns11" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>床位使用率</center>" Sortable="true" ColumnID="Columns12"
                                            DataIndex="Columns12" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>床位周转次数</center>" Sortable="true" ColumnID="Columns13"
                                            DataIndex="Columns13" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>门诊与出院诊断符合率</center>" Sortable="true" ColumnID="Columns14"
                                            DataIndex="Columns14" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>入院与出院诊断符合率</center>" Sortable="true" ColumnID="Columns15"
                                            DataIndex="Columns15" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>术前与术后诊断符合率</center>" Sortable="true" ColumnID="Columns16"
                                            DataIndex="Columns16" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>临床与病理诊断符合率</center>" Sortable="true" ColumnID="Columns17"
                                            DataIndex="Columns17" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>入院三日确诊率</center>" Sortable="true" ColumnID="Columns18"
                                            DataIndex="Columns18" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>住院病人门诊待诊率</center>" Sortable="true" ColumnID="Columns19"
                                            DataIndex="Columns19" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>住院病人初诊待诊人数</center>" Sortable="true" ColumnID="Columns20"
                                            DataIndex="Columns20" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>治愈率</center>" Sortable="true" ColumnID="Columns21" DataIndex="Columns21"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>住院抢救人次</center>" Sortable="true" ColumnID="Columns22"
                                            DataIndex="Columns22" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>住院抢救成功率</center>" Sortable="true" ColumnID="Columns23"
                                            DataIndex="Columns23" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>危重病人天数</center>" Sortable="true" ColumnID="Columns24"
                                            DataIndex="Columns24" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>手术量</center>" Sortable="true" ColumnID="Columns25" DataIndex="Columns25"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>特大手术率</center>" Sortable="true" ColumnID="Columns26"
                                            DataIndex="Columns26" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>中等以上手术率</center>" Sortable="true" ColumnID="Columns27"
                                            DataIndex="Columns27" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>无菌手术甲级愈合率</center>" Sortable="true" ColumnID="Columns28"
                                            DataIndex="Columns28" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>院内感染率</center>" Sortable="true" ColumnID="Columns29"
                                            DataIndex="Columns29" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>手术并发症发生率</center>" Sortable="true" ColumnID="Columns30"
                                            DataIndex="Columns30" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>无菌手术切口感染率</center>" Sortable="true" ColumnID="Columns31"
                                            DataIndex="Columns31" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>非手术并发症发生率</center>" Sortable="true" ColumnID="Columns32"
                                            DataIndex="Columns32" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>医疗事故发生率</center>" Sortable="true" ColumnID="Columns33"
                                            DataIndex="Columns33" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>医疗差错发生率</center>" Sortable="true" ColumnID="Columns34"
                                            DataIndex="Columns34" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>护理差错发生率</center>" Sortable="true" ColumnID="Columns35"
                                            DataIndex="Columns35" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>甲级病案率</center>" Sortable="true" ColumnID="Columns36"
                                            DataIndex="Columns36" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>门诊量</center>" Sortable="true" ColumnID="Columns37" DataIndex="Columns37"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>军免人数</center>" Sortable="true" ColumnID="Columns38" DataIndex="Columns38"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>地方人数</center>" Sortable="true" ColumnID="Columns39" DataIndex="Columns39"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>专家门诊</center>" Sortable="true" ColumnID="Columns40" DataIndex="Columns40"
                                            Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>专科门诊人次</center>" Sortable="true" ColumnID="Columns41"
                                            DataIndex="Columns41" Align="center" Width="116">
                                        </ext:Column>
                                        <ext:Column Header="<center>急诊量</center>" Sortable="true" ColumnID="Columns42" DataIndex="Columns42"
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
