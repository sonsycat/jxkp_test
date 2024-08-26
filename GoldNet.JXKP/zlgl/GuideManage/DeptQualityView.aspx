<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptQualityView.aspx.cs"
    Inherits="GoldNet.JXKP.zlgl.SysManage.DeptQualityView" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg) {
            Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
            
        function dbonclick(item_class)
		{
		   document.location.href="dept_income_item.aspx?item_class="+item_class;  
		}
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server" AutoLoad="true" GroupField="GUIDETYPE" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="GUIDETYPE" />
                    <ext:RecordField Name="GUIDENAME" />
                    <ext:RecordField Name="GUIDENUM" />
                    <ext:RecordField Name="GUIDESPARENUM" />
                    
                    <ext:RecordField Name="TEMPLETID" />
                    <ext:RecordField Name="DATECOL" />
                    <ext:RecordField Name="TARGETCOL" />
                    <ext:RecordField Name="STARTDATE" />
                     <ext:RecordField Name="ENDDATE" />
                    <ext:RecordField Name="DEPTNAME" />
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
                                <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                    <Items>
                                        <ext:Button ID="Button_refresh" runat="server" Text="返回" Icon="ArrowRefresh">
                                            <AjaxEvents>
                                                <Click OnEvent="btnCancle_Click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="GUIDETYPE" Header="考评类别" Width="200" Align="left" Sortable="true"
                                        DataIndex="GUIDETYPE" MenuDisabled="true" />
                                    <ext:Column ColumnID="GUIDENAME" Header="考评项目" Width="300" Align="left" Sortable="true"
                                        DataIndex="GUIDENAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="GUIDENUM" Header="项目分值" Width="100" Align="right" Sortable="true"
                                        DataIndex="GUIDENUM" MenuDisabled="true" />
                                    <ext:Column ColumnID="GUIDESPARENUM" Header="本月得分" Width="100" Align="right" Sortable="true"
                                        DataIndex="GUIDESPARENUM" MenuDisabled="true" />
                                        
                                        <ext:Column ColumnID="TEMPLETID" Header="" Width="200" Align="left" Sortable="true" Hidden="true"
                                        DataIndex="TEMPLETID" MenuDisabled="true" />
                                    <ext:Column ColumnID="DATECOL" Header="" Width="300" Align="left" Sortable="true" Hidden="true"
                                        DataIndex="DATECOL" MenuDisabled="true" />
                                    <ext:Column ColumnID="TARGETCOL" Header="" Width="100" Align="right" Sortable="true" Hidden="true"
                                        DataIndex="TARGETCOL" MenuDisabled="true" />
                                    <ext:Column ColumnID="STARTDATE" Header="" Width="100" Align="right" Sortable="true" Hidden="true"
                                        DataIndex="STARTDATE" MenuDisabled="true" />
                                         <ext:Column ColumnID="ENDDATE" Header="" Width="100" Align="right" Sortable="true" Hidden="true"
                                        DataIndex="ENDDATE" MenuDisabled="true" />
                                    <ext:Column ColumnID="DEPTNAME" Header="" Width="100" Align="right" Sortable="true" Hidden="true"
                                        DataIndex="DEPTNAME" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                           <LoadMask ShowMask="true" />
                                <AjaxEvents>
                                    <DblClick OnEvent="Button_set_click">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw" />
                                        </ExtraParams>
                                    </DblClick>
                                </AjaxEvents>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GroupingView ID="GroupingView1" HideGroupedColumn="true" runat="server" GroupTextTpl='{text} ({[values.rs.length]})'
                                    EnableRowBody="false">
                                </ext:GroupingView>
                            </View>
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
