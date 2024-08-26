<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Item_Leibie.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Distribute.Item_Leibie" %>

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
        .search-item
        {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }
        .search-item h3
        {
            display: block;
            font: inherit;
            font-weight: bold;
            color: #222;
        }
        .search-item h3 span
        {
            float: right;
            font-weight: normal;
            margin: 0 0 5px 5px;
            width: 100px;
            display: block;
            clear: none;
        }
        p
        {
            width: 650px;
        }
        .ext-ie .x-form-text
        {
            position: static !important;
        }
        
        /* 样式定义 */
        .file-upload-btn
        {
            background-color: #4CAF50; /* 背景色 */
            color: white; /* 字体颜色 */
            border: none; /* 去掉边框 */
            padding: 10px 20px; /* 内边距 */
            text-align: center; /* 文本对齐 */
            text-decoration: none; /* 去掉文本装饰 */
            display: inline-block; /* 使按钮排成一行 */
            font-size: 16px; /* 字体大小 */
            margin: 4px 2px; /* 外边距 */
            cursor: pointer; /* 鼠标悬浮效果 */
        }
        
        .file-upload-btn:hover
        {
            background-color: #45a049; /* 鼠标悬浮背景色 */
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

        function handleFailure(response, opts) {
            if (response.status === -1 && response.statusText === 'transaction aborted') {
                console.log('Request aborted: ' + response.statusText);
            } else {
                Ext.Msg.alert('Error', 'Request failed, status code: ' + response.status);
            }
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                    <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                    <ext:RecordField Name="CLASS_NAME" Type="String" Mapping="CLASS_NAME" />
                    <ext:RecordField Name="ITEM_UNIT" Type="String" Mapping="ITEM_UNIT" />
                    <ext:RecordField Name="ITEM_PRICE" Type="String" Mapping="ITEM_PRICE" />
                    <ext:RecordField Name="PANDU" Type="String" Mapping="PANDU" />
                    <ext:RecordField Name="ZHIXING" Type="String" Mapping="ZHIXING" />
                    <ext:RecordField Name="HULI" Type="String" Mapping="HULI" />
                    <ext:RecordField Name="ROWID" Type="String" Mapping="ROWSID" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Proxy>
        </Proxy>
        <Reader>
            <ext:JsonReader Root="itemlist" TotalProperty="totalCount">
                <Fields>
                    <ext:RecordField Name="ITEM_CODE" />
                    <ext:RecordField Name="ITEM_NAME" />
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
                                                        <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="20">
                                                        </ext:ToolbarSpacer>
                                                        <ext:FileUploadField ID="FileUploadField1" runat="server" ButtonText="选择文件" Width="200"/>
                                                        <ext:Button ID="btn_Import" Text="导入Excel" Icon="PageWhiteExcel" runat="server">
                                                            <AjaxEvents>
                                                                <Click OnEvent="ImportExcel" IsUpload="true">
                                                                    <EventMask Msg="正在导入" ShowMask="true" />
                                                                </Click>
                                                            </AjaxEvents>
                                                        </ext:Button>
                                                        <ext:ToolbarSpacer ID="ToolbarSpacer4" runat="server" Width="20">
                                                        </ext:ToolbarSpacer>
                                                        <ext:Label ID="Label1" runat="server" Text="项目：">
                                                        </ext:Label>
                                                        <ext:ComboBox ID="ItemLeibie" runat="server" StoreID="Store3" DisplayField="ITEM_NAME"
                                                            ValueField="ITEM_CODE" TypeAhead="false" LoadingText="Searching..." Width="300"
                                                            PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1"
                                                            ListWidth="300" EmptyText="选择项目">
                                                            <Template ID="Template1" runat="server">
                                                   <tpl for=".">
                                                      <div class="search-item">
                                                         <h3><span>
                                                          {ITEM_CODE}</span>{ITEM_NAME}</h3>
                                                      </div>
                                                   </tpl>
                                                            </Template>
                                                        </ext:ComboBox>
                                                        <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="20">
                                                        </ext:ToolbarSpacer>
                                                        <ext:Button ID="bquery" Text="查询" Icon="FolderMagnify" runat="server">
                                                            <AjaxEvents>
                                                                <Click OnEvent="Query" Timeout="120000" Failure="handleFailure">
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

                                                        <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="20">
                                                        </ext:ToolbarSpacer>
                                                        <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                            Text="导出Excel" Icon="PageWhiteExcel" CausesValidation="false">
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <ColumnModel ID="ColumnModel2" runat="server">
                                                <Columns>
                                                    <ext:Column ColumnID="ITEM_CODE" Header="<div style='text-align:center;'>项目编码</div>"
                                                        Width="150" Align="left" Sortable="false" DataIndex="ITEM_CODE" MenuDisabled="true">
                                                    </ext:Column>
                                                    <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>项目名称</div>"
                                                        Width="150" Align="left" Sortable="false" DataIndex="ITEM_NAME" MenuDisabled="true">
                                                    </ext:Column>
                                                    <ext:Column ColumnID="CLASS_NAME" Header="<div style='text-align:center;'>类型名称</div>"
                                                        Width="150" Align="left" Sortable="false" DataIndex="CLASS_NAME" MenuDisabled="true">
                                                    </ext:Column>
                                                    <ext:Column ColumnID="ITEM_UNIT" Header="<div style='text-align:center;'>规格</div>"
                                                        Width="150" Align="left" Sortable="false" DataIndex="ITEM_UNIT" MenuDisabled="true">
                                                    </ext:Column>
                                                    <ext:Column ColumnID="ITEM_PRICE" Header="<div style='text-align:center;'>价格</div>"
                                                        Width="150" Align="left" Sortable="false" DataIndex="ITEM_PRICE" MenuDisabled="true">
                                                        <Renderer Fn="rmbMoney" />
                                                    </ext:Column>
                                                    <ext:Column ColumnID="PANDU" Header="<div style='text-align:center;'>PANDU</div>"
                                                        Width="150" Align="left" Sortable="false" DataIndex="PANDU" MenuDisabled="true">
                                                        <Editor>
                                                            <ext:TextField ID="TextField1" runat="server" />
                                                        </Editor>
                                                        <Renderer Fn="rmbMoney" />
                                                    </ext:Column>
                                                    <ext:Column ColumnID="ZHIXING" Header="<div style='text-align:center;'>执行</div>"
                                                        Width="150" Align="left" Sortable="false" DataIndex="ZHIXING" MenuDisabled="true">
                                                        <Editor>
                                                            <ext:TextField ID="TextField2" runat="server" />
                                                        </Editor>
                                                        <Renderer Fn="rmbMoney" />
                                                    </ext:Column>
                                                    <ext:Column ColumnID="HULI" Header="<div style='text-align:center;'>护理</div>" Width="150"
                                                        Align="left" Sortable="false" DataIndex="HULI" MenuDisabled="true">
                                                        <Editor>
                                                            <ext:TextField ID="TextField3" runat="server" />
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
                                            <BottomBar>
                                                <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="50" StoreID="Store1"
                                                    AutoWidth="true" DisplayInfo="false" AutoDataBind="true" ShowRefresh="false">
                                                </ext:PagingToolbar>
                                            </BottomBar>
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
    </form>
</body>
</html>
