<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="patient_dict.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.xyhsdict.patient_dict" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../Bonus/Orthers/Cbouns.css" />

    <script type="text/javascript">
        var RefreshData = function(msg) {
//            Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server" AutoLoad="true" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="PATIENT_ID" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="SEX" />
                    <ext:RecordField Name="IDENTITY" />
                    <ext:RecordField Name="CHARGE_TYPE" />
                    <ext:RecordField Name="STAR_DATE" />
                    <ext:RecordField Name="DIAGNOSIS_NAME" />
                    <ext:RecordField Name="VISIT_ID" />
                    <ext:RecordField Name="OUT_OR_IN" />
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
                                        <ext:Button ID="Btn_Add" Text="增加" Icon="Add" runat="server">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Add_Click">
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                   
                                        <ext:Button ID="Btn_Del" Text="删除" Icon="Delete" runat="server">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Del_Click">
                                                    <Confirmation BeforeConfirm="config.confirmation.message = '你确定要删除吗？';" Title="系统提示"
                                                        ConfirmRequest="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="PATIENT_ID" Header="病人id" Width="100" Align="left" Sortable="true"
                                        DataIndex="PATIENT_ID" MenuDisabled="true" />
                                        <ext:Column ColumnID="VISIT_ID" Header="住院标识" Width="100" Align="left" Sortable="true"
                                        DataIndex="VISIT_ID" MenuDisabled="true" />
                                    <ext:Column ColumnID="NAME" Header="病人名称" Width="100" Align="left" Sortable="true"
                                        DataIndex="NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="SEX" Header="性别" Width="200" Align="left" Sortable="true"
                                        DataIndex="SEX" MenuDisabled="true" />
                                    <ext:Column ColumnID="IDENTITY" Header="身份" Width="100" Align="left" Sortable="true"
                                        DataIndex="IDENTITY" MenuDisabled="true" />
                                    <ext:Column ColumnID="CHARGE_TYPE" Header="费别" Width="100" Align="left" Sortable="true"
                                        DataIndex="CHARGE_TYPE" MenuDisabled="true" />
                                     <ext:Column ColumnID="STAR_DATE" Header="起始时间" Width="140" Align="Right" Sortable="true"
                                        DataIndex="STAR_DATE" MenuDisabled="true" />
                                        <ext:Column ColumnID="DIAGNOSIS_NAME" Header="病种" Width="140" Align="Right" Sortable="true"
                                        DataIndex="DIAGNOSIS_NAME" MenuDisabled="true" />
                                         <ext:Column ColumnID="OUT_OR_IN" Header="标识" Width="140" Align="Right" Sortable="true"
                                        DataIndex="OUT_OR_IN" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    
                                </ext:RowSelectionModel>
                            </SelectionModel>
                             <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="25" StoreID="Store1"
                                        AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                        
                                    </ext:PagingToolbar>
                                </BottomBar>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="病人设置" Width="400"
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
