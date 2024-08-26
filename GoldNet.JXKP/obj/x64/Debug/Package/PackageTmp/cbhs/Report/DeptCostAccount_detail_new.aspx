<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptCostAccount_detail_new.aspx.cs"
    Inherits="GoldNet.JXKP.DeptCostAccount_detail_new" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript">
        //
        var rmbMoney = function(v) {
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
        };
        
        //
        var totalMoney = function(a, b, c) {
//            lChange.text = rmbMoney(a);
//            lIncome.Text = rmbMoney(b);
//            lBenefit.Text = rmbMoney(c);
        };
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="SReport" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="DEPT_CODE">
                <Fields>
                    <ext:RecordField Name="DEPT_CODE">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="FACACCOUNTINCOMES">
                    </ext:RecordField>
                    <ext:RecordField Name="COSTS">
                    </ext:RecordField>
                    <ext:RecordField Name="PROFIT">
                    </ext:RecordField>
                    <ext:RecordField Name="ARMACCOUNTINCOMES">
                    </ext:RecordField>
                    <ext:RecordField Name="ARMYCOSTS">
                    </ext:RecordField>
                    <ext:RecordField Name="MEDINCOMES">
                    </ext:RecordField>
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
                            <ext:Panel runat="server" ID="p11" AutoScroll="true" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                            <ext:Button ID="Button_refresh" runat="server" Text="返回" Icon="ArrowUndo">
                                                <AjaxEvents>
                                                    <Click OnEvent="btnCancle_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                            <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                Text="导出Excel" Icon="PageWhiteExcel">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <center>
                                        <h2>
                                            医疗收支,药品效益汇总表</h2>
                                    </center>
                                    <ext:ExtGridPanel ID="GridPanel_Show" runat="server" StoreID="SReport" Border="true"
                                        Width="800" Height="400" AutoScroll="true" StyleSpec="margin:10px">
                                        <ExtColumnModel ID="ColumnModel1" runat="server">
                                            <Columns>
                                                <ext:ExtColumn ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>核算单位</div>"
                                                    Width="100" DataIndex="DEPT_NAME" MenuDisabled="true" Align="Left">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="FACACCOUNTINCOMES" Header="<div style='text-align:center;'>收入</div>"
                                                    Width="100" DataIndex="FACACCOUNTINCOMES" MenuDisabled="true" Align="Right">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="COSTS" Header="<div style='text-align:center;'>成本</div>"
                                                    Width="100" DataIndex="COSTS" MenuDisabled="true" Align="Right">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="PROFIT" Header="<div style='text-align:center;'>收益</div>"
                                                    Width="100" DataIndex="PROFIT" MenuDisabled="true" Align="Right">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="MEDINCOMES" Header="<div style='text-align:center;'>药品收益</div>"
                                                    Width="100" DataIndex="MEDINCOMES" MenuDisabled="true" Align="Right">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ARMACCOUNTINCOMES" Header="<div style='text-align:center;'>收入</div>"
                                                    Width="100" DataIndex="ARMACCOUNTINCOMES" MenuDisabled="true" Align="Right">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ARMYCOSTS" Header="<div style='text-align:center;'>成本</div>"
                                                    Width="100" DataIndex="ARMYCOSTS" MenuDisabled="true" Align="Right">
                                                </ext:ExtColumn>
                                            </Columns>
                                            <HeadRows>
                                                <ext:ExtRows>
                                                    <Rows>
                                                        <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                        <ext:ExtRow Header="实际" ColSpan="4" Align="Center" />
                                                        <ext:ExtRow Header="计价" ColSpan="2" Align="Center" />
                                                    </Rows>
                                                </ext:ExtRows>
                                            </HeadRows>
                                        </ExtColumnModel>
                                        <Plugins>
                                            <ext:ExtGroupHeaderGrid ID="ExtGroupHeaderGrid2" runat="server">
                                            </ext:ExtGroupHeaderGrid>
                                        </Plugins>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Listeners>
                                            <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}this.setHeight( Ext.getBody().getViewSize().height -100); }, this)" />
                                            <Render Handler="if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}this.setHeight( Ext.getBody().getViewSize().height -100);" />
                                        </Listeners>
                                        <LoadMask ShowMask="true" />
                                    </ext:ExtGridPanel>
                                </Body>
                            </ext:Panel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
