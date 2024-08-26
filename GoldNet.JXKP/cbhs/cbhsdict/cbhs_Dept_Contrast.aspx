<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cbhs_Dept_Contrast.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.cbhsdict.cbhs_Dept_Contrast" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg) {
            Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
            
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData" WarningOnDirty="false"
        AutoLoad="true">
        <%--       OnSubmitData="SubmitData"--%>
        <Reader>
            <ext:JsonReader ReaderID="DEPT_CODE">
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="OTHER_COST_DEPTCODE" />
                    <ext:RecordField Name="OTHER_COST_DEPTNAME" />
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
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                          
                                            <ext:Button ID="bsave" Text="保存" Icon="Disk" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_save">
                                                        <EventMask Msg="正在保存" ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues(false))"
                                                                Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="DEPT_CODE" Header="<div style='text-align:center;'>his科室代码</div>"
                                            Width="90" Align="Left" Sortable="true" DataIndex="DEPT_CODE" MenuDisabled="true" />
                                        <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>his科室名称</div>"
                                            Width="90" Align="Left" Sortable="true" DataIndex="DEPT_NAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="OTHER_COST_DEPTCODE" Header="<div style='text-align:center;'>对照科室代码</div>"
                                            Width="80" Align="Right" Sortable="true" DataIndex="OTHER_COST_DEPTCODE" MenuDisabled="true">
                                            <Editor>
                                                <ext:TextField runat="server" ID="txtDeptCode" MaxLength="20">
                                                </ext:TextField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="OTHER_COST_DEPTNAME" Header="<div style='text-align:center;'>对照科室名称</div>"
                                            Width="80" Align="Right" Sortable="true" DataIndex="OTHER_COST_DEPTNAME" MenuDisabled="true">
                                            <Editor>
                                                <ext:TextField runat="server" ID="txtDeptName" MaxLength="20">
                                                </ext:TextField>
                                            </Editor>
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
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
