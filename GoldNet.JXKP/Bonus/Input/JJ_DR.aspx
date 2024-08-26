<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JJ_DR.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Input.JJ_DR" %>

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
    <script language="javascript" type="text/javascript">
        var RowIndex;

        //列表刷新
        var RefreshData = function () {
            Store1.reload();
        };

        //列表单元格格式化（金额单元）
        var rmbMoney = function (v) {
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

        //列表单元格
        function FormatRender(v, p, record, rowIndex) {
            var colors = ["red", "black", "blue"];
            if (record.data.FLAG == "1") {
                var template = '<span style="color:{0};font-weight:bold;">{1}</span>';
                return String.format(template, colors[0], record.data.DEPT_NAME);
            }
            else {
                var templateb = '<span style="color:{0};">{1}</span>';
                return String.format(templateb, colors[1], record.data.DEPT_NAME);
            }
        }

        //科室代码赋值
        function selectOrderedDept(cc) {
            var record2 = Store1.getAt(RowIndex);
            record2.data['DEPT_CODE'] = cc;
            GridPanel1.getView().focusRow(RowIndex);
        };

        function openwin() {
            var cc = "";
            var str = document.getElementById('stardate').value;
            var reg = /^(\d{4})(\d{2})(\d{2})$/;
            cc = str.replace(reg, '$1-$2-$3');
            var costitem = COST_ITEM.getSelectedItem().value;

            var url = "single_cost_submit_new.aspx?pageid=010104_1&startime=" + cc + "&costitem=" + costitem;
            var newwindow = window.open(url, 'newwin', 'resizable=no,scrollbars=yes,status=yes,toolbar=no,menubar=no,location=no');
            var wide = window.screen.availWidth;
            var high = window.screen.availHeight;
            newwindow.resizeTo(wide, high);
            newwindow.moveTo(0, 0);

        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ST_DATE" Type="String" Mapping="ST_DATE" />
                    <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                    <ext:RecordField Name="ID" Type="String" Mapping="ID" />
                    <ext:RecordField Name="COSTS" Type="String" Mapping="COSTS" />
                    <ext:RecordField Name="USER_ID" Type="String" Mapping="USER_ID" />
                    <ext:RecordField Name="USER_NAME" Type="String" Mapping="USER_NAME" />
                    <ext:RecordField Name="ROWID" Type="String" Mapping="ROWSID" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" AutoLoad="true">
        <Proxy>
        </Proxy>
        <Reader>
            <ext:JsonReader Root="deptlist" TotalProperty="totalCount">
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store4" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="MONTH">
                <Fields>
                    <ext:RecordField Name="MONTH" />
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
                            <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true" FitHeight="true">
                                <Columns>
                                    <ext:LayoutColumn ColumnWidth="1">
                                        <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                            ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar2" runat="server" Visible="true" AutoWidth="true">
                                                    <Items>
                                                        <ext:DateField ID="stardate" runat="server" FieldLabel="选择日期：" Width="100" EnableKeyEvents="true"
                                                            ReadOnly="true" Hidden="true" />
                                                        <ext:KeyNav ID="stardate1" runat="server" Target="stardate">
                                                            <Enter Handler="var str = document.getElementById('stardate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('stardate').value   =   str.replace(reg, '$1-$2-$3');" />
                                                        </ext:KeyNav>
                                                        <%--<ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />--%>
                                                        <ext:FileUploadField ID="photoimg" runat="server" ButtonOnly="true" ButtonText="导入"
                                                            Icon="ImageAdd" Hidden="true">
                                                            <AjaxEvents>
                                                                <FileSelected OnEvent="UploadClick" Timeout="99999999">
                                                                </FileSelected>
                                                            </AjaxEvents>
                                                        </ext:FileUploadField>
                                                        <ext:ToolbarSpacer ID="ToolbarSpacer4" runat="server" Width="20">
                                                        </ext:ToolbarSpacer>
                                                        <ext:Label ID="lcaption" runat="server" Text="奖金月份:">
                                                        </ext:Label>
                                                        <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="Store3" Width="70"
                                                            DisplayField="YEAR" ValueField="YEAR">
                                                        </ext:ComboBox>
                                                        <ext:Label ID="lYear" runat="server" Text="年">
                                                        </ext:Label>
                                                        <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="Store4" Width="70"
                                                            DisplayField="MONTH" ValueField="MONTH">
                                                        </ext:ComboBox>
                                                        <ext:Label ID="lmonth" runat="server" Text="月">
                                                        </ext:Label>
                                                        <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="20">
                                                        </ext:ToolbarSpacer>
                                                        <ext:Button ID="bquery" Text="查询" Icon="FolderMagnify" runat="server">
                                                            <AjaxEvents>
                                                                <Click OnEvent="Query">
                                                                </Click>
                                                            </AjaxEvents>
                                                        </ext:Button>
                                                        <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="20">
                                                        </ext:ToolbarSpacer>
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
                                                        <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="20">
                                                        </ext:ToolbarSpacer>
                                                        <ext:Button ID="bsave" Text="保存" Icon="Disk" runat="server">
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
                                                        <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="20">
                                                        </ext:ToolbarSpacer>
                                                        <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                            Text="导出Excel" Icon="PageWhiteExcel" CausesValidation="false">
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <ColumnModel ID="ColumnModel2" runat="server">
                                                <Columns>
                                                    <ext:Column ColumnID="ROW_ID" Hidden="true" />
                                                    <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室</div>"
                                                        Width="130" Align="left" DataIndex="DEPT_NAME" MenuDisabled="true">
                                                        <Editor>
                                                            <ext:ComboBox ID="ComboBox1" runat="server" StoreID="Store2" DisplayField="DEPT_CODE"
                                                                AllowBlank="true" ValueField="DEPT_NAME" TypeAhead="false" LoadingText="Searching..."
                                                                Width="220" ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                                <Template ID="Template1" runat="server">
                                                                  <tpl for=".">
                                                                   <div class="search-item">
                                                                     <h3><span>{DEPT_NAME}</span>{DEPT_CODE}</h3>
                                                                   </div>
                                                                  </tpl>
                                                                </Template>
                                                                <Listeners>
                                                                    <Select Handler="selectOrderedDept(this.getText());" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                        </Editor>
                                                    </ext:Column>
                                                    <ext:Column ColumnID="USER_ID" Header="<div style='text-align:center;'>姓名</div>"
                                                        Width="150" Align="left" Sortable="false" DataIndex="USER_ID" MenuDisabled="true" Hidden="true">
                                                    </ext:Column>
                                                    <ext:Column ColumnID="USER_NAME" Header="<div style='text-align:center;'>姓名</div>"
                                                        Width="150" Align="left" Sortable="false" DataIndex="USER_NAME" MenuDisabled="true">
                                                        
                                                    </ext:Column>
                                                    <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>绩效</div>" Width="150"
                                                        Align="left" Sortable="false" DataIndex="COSTS" MenuDisabled="true">
                                                        <Editor>
                                                            <ext:TextField ID="TextField2" runat="server" />
                                                        </Editor>
                                                        <Renderer Fn="rmbMoney" />
                                                    </ext:Column>
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:CheckboxSelectionModel ID="RowSelectionModel1" runat="server">
                                                    <Listeners>
                                                        <RowSelect Handler="RowIndex = rowIndex" />
                                                        <RowDeselect Handler="RowIndex = rowIndex" />
                                                        <SelectionChange Handler="#{GridPanel1}.hasSelection()? #{Button_del}.setDisabled(false): #{Button_del}.setDisabled(true);" />
                                                    </Listeners>
                                                </ext:CheckboxSelectionModel>
                                            </SelectionModel>
                                            <LoadMask ShowMask="true" />
                                            <%--<Listeners>
                                                    <KeyDown Handler="if (e.getKey() == 40){ #{GridPanel1}.insertRecord(0, {});#{GridPanel1}.getView().focusRow(0);#{GridPanel1}.startEditing(0, 0);} ;" />
                                                </Listeners>--%>
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
