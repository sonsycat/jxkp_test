<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Person_Appraisal.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.Person_Appraisal" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>人员评价</title>

    <script type="text/javascript">
        var removeItems = function(source) {
            source = source || SelectorLeft;
            var selectionsArray = source.view.getSelectedIndexes();
            var records = [];
            if (selectionsArray.length > 0) {
                for (var i = 0; i < selectionsArray.length; i++) {
                    var rec = source.view.store.getAt(selectionsArray[i]);
                    records.push(rec);
                }
                for (var i = 0; i < selectionsArray.length; i++) {
                    source.store.remove(records[i]);
                }
            }
            if ((source.store.getCount() == 0) || (source.store.getCount() == null)) {
                Btn_Eval.setDisabled(true);
            }
        };

        var addItems = function(source, records) {
            source.store.removeAll();
            if (records.length == 0) {
                Btn_Eval.setDisabled(true);
            }else{
                for (var i = 0; i < records.length; i++) {
                    source.store.add(new Ext.data.Record({ text: records[i][0], value: records[i][1] }));
                }
                if ((SelectorLeft.store.getCount()) >0 && (SelectorRight.store.getCount() >0 )) Btn_Eval.setDisabled(false);
            }
        };

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="Store2">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="GUIDE_NAME" />
                    <ext:RecordField Name="GUIDE_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server" FitHeight="true">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="1">
                        <ext:Panel ID="Panel11" runat="server" Width="400" Height="300" BodyBorder="false"
                            AutoScroll="true">
                            <TopBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="从：" />
                                        <ext:ComboBox runat="server" ID="Comb_StartYear" Width="60" ListWidth="60" SelectedIndex="0">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                        <ext:ComboBox runat="server" ID="Comb_StartMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem3" runat="server" Text="月　至：" />
                                        <ext:ComboBox runat="server" ID="Comb_EndYear" Width="60" ListWidth="60" SelectedIndex="0">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="年" />
                                        <ext:ComboBox runat="server" ID="Comb_EndMonth" Width="40" ListWidth="40">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem5" runat="server" Text="月　　评价方法：" />
                                        <ext:ComboBox runat="server" ID="Comb_App" SelectedIndex="0" Width="88">
                                            <Items>
                                                <ext:ListItem Text="TOPSIS" Value="Topsis" />
                                                <ext:ListItem Text="密切值法" Value="Nearly" />
                                            </Items>
                                        </ext:ComboBox>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_SelectPerson" runat="server" Icon="UserAdd" Text="选择人员">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_SelectPerson_Click">
                                                    <EventMask ShowMask="true" Msg="请稍候..." />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_SelectGuide" runat="server" Icon="ShapeSquareAdd" Text="选择指标">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_SelectGuide_Click">
                                                    <EventMask ShowMask="true" Msg="请稍候..." />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_Eval" runat="server" Icon="DatabaseGo" Disabled="true" Text="开始评价">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Eval_Click" Timeout="900000">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="生成指标评价数据大约耗时2分钟,<br/>是否继续?" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="multi1" Value="Ext.encode(#{SelectorLeft}.getValues(true))"
                                                            Mode="Raw" />
                                                        <ext:Parameter Name="multi2" Value="Ext.encode(#{SelectorRight}.getValues(true))"
                                                            Mode="Raw" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" Msg="请稍候,正在生成指标评价数据..." />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                                        </ext:ToolbarSeparator>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Body>
                                <ext:TableLayout ID="TableLayout1" runat="server">
                                    <ext:Cell>
                                        <ext:Panel ID="Panel1" runat="server" Border="false" ButtonAlign="Left" Width="320"
                                            Height="400">
                                            <Body>
                                                <ext:MultiSelect ID="SelectorLeft" runat="server" Legend="已选择人员" Height="350" AutoWidth="true"
                                                    KeepSelectionOnClick="WithCtrlKey" StyleSpec="margin:5px; ">
                                                </ext:MultiSelect>
                                            </Body>
                                            <Buttons>
                                                <ext:Button runat="server" Text="删除选中人员" ID="Btn_ClearDept" Icon="VcardDelete">
                                                    <Listeners>
                                                        <Click Handler="removeItems();" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Buttons>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel6" runat="server" Border="false" ButtonAlign="Right" Width="320"
                                            Height="400">
                                            <Body>
                                                <ext:MultiSelect ID="SelectorRight" runat="server" Legend="已选择指标" Height="350" AutoWidth="true"
                                                    KeepSelectionOnClick="WithCtrlKey" StyleSpec="margin:5px; ">
                                                </ext:MultiSelect>
                                            </Body>
                                            <Buttons>
                                                <ext:Button runat="server" Text="删除选中指标" ID="Btn_ClearGuide" Icon="ShapeSquareDelete">
                                                    <Listeners>
                                                        <Click Handler="removeItems(SelectorRight);" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Buttons>
                                        </ext:Panel>
                                    </ext:Cell>
                                </ext:TableLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="DetailWin" runat="server" Icon="ApplicationViewIcons" Title="选择"
        Width="760" Closable="true" CloseAction="Hide" Maximizable="true" Height="430"
        AutoShow="false" Modal="true" AutoScroll="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Listeners>
            <Hide Handler="this.clearContent();" />
            <BeforeShow Handler=" var height = Ext.getBody().getViewSize().height; var width = Ext.getBody().getViewSize().width; if (el.getSize().height > height) {  el.setHeight(height - 20) } ;if (el.getSize().width > width) {  el.setWidth(width - 20) }  " />
        </Listeners>
    </ext:Window>
    </form>
</body>
</html>
