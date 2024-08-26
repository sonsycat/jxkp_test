<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Doctor_Report.aspx.cs"
    Inherits="GoldNet.JXKP.Doctor_Report" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
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
    </style>

    <script type="text/javascript">
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
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="ORDERED_BY_DOCTOR">
                <Fields>
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
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" Height="480" ClicksToEdit="1">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" StyleSpec="border:0">
                                        <Items>
                                            <ext:Label ID="lcaption" runat="server" Text="从：">
                                            </ext:Label>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                            <ext:DateField ID="stardate" runat="server" FieldLabel="开始时间：" Width="100" EnableKeyEvents="true"
                                                ReadOnly="true" />
                                            <ext:KeyNav ID="stardate1" runat="server" Target="stardate">
                                                <Enter Handler="var str = document.getElementById('stardate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('stardate').value   =   str.replace(reg, '$1-$2-$3');" />
                                            </ext:KeyNav>
                                            <ext:Label ID="lmonth" runat="server" Text="月 到：">
                                            </ext:Label>
                                            <ext:DateField ID="enddate" runat="server" FieldLabel="结束时间：" Width="100" EnableKeyEvents="true"
                                                ReadOnly="true" />
                                            <ext:KeyNav ID="enddate1" runat="server" Target="enddate">
                                                <Enter Handler="var str = document.getElementById('enddate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('enddate').value   =   str.replace(reg, '$1-$2-$3');" />
                                            </ext:KeyNav>
                                            <ext:Label ID="Label4" runat="server" Text="科室：">
                                            </ext:Label>
                                            <ext:ComboBox ID="ComAccountdeptcode" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                                                ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="100"
                                                PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1"
                                                ListWidth="300" EmptyText="选择科室">
                                                <Template ID="Template2" runat="server">
                                                   <tpl for=".">
                                                      <div class="search-item">
                                                         <h3><span>
                                                          {DEPT_CODE}</span>{DEPT_NAME}</h3>
                                                      </div>
                                                   </tpl>
                                                </Template>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="deptType" runat="server" AllowBlank="true" ForceSelection="true"
                                                SelectOnFocus="true" Width="100" SelectedIndex="0">
                                                <Items>
                                                    <ext:ListItem Text="门诊" Value="0" />
                                                    <ext:ListItem Text="住院" Value="1" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            <ext:Button ID="btn_Query" runat="server" Text="查询" Icon="Zoom">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Query_Click" Timeout="99999999">
                                                        <EventMask Msg="查询中......" ShowMask="true" />
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
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                                <AjaxEvents>
                                    <DblClick OnEvent="DbRowClick">
                                        <%-- <ExtraParams>
                                            <ext:Parameter Name="Values" Value="this.store.getAt(rowIndex).get('ORDERED_BY_DOCTOR')" Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>--%>
                                    </DblClick>
                                </AjaxEvents>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="Doctor_Detail" runat="server" Icon="Group" Title="收入明细" Width="880"
        Height="420" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
