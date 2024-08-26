<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PatientDetail.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.Report.PatientDetail" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />
    <style type="text/css">
        h2
        {
            font-size: 24px;
            letter-spacing: 1px;
            margin: 10px 0 20px;
            padding: 0;
        }
        .search-item
        {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
            width: 200px;
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
            width: 140px;
            display: block;
            clear: none;
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
        var template = '<span style="color:{0};">{1}</span>';

        var namecolor = function(value) {
            if (value.indexOf("　") > -1) {
                return String.format(template, 'black', value);
            }
            else {
                return String.format(template, (value == '合计') ? 'red' : 'blue', value);
            }
        };

    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <form id="form1" runat="server">
    <ext:Store ID="SReport" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_CODE">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="PATIENT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="PATIENT_ID">
                    </ext:RecordField>
                    <ext:RecordField Name="VISIT_DATE">
                    </ext:RecordField>
                    <ext:RecordField Name="RCPT_NO">
                    </ext:RecordField>
                    <ext:RecordField Name="COSTS">
                    </ext:RecordField>
                    <ext:RecordField Name="CHARGES">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SYear" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SMonth" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="MONTH">
                <Fields>
                    <ext:RecordField Name="MONTH" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SDept" runat="server" AutoLoad="true">
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:Panel runat="server" ID="p11" AutoScroll="true" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" StyleSpec="border:0">
                                        <Items>
                                            <ext:Label ID="lcaption" runat="server" Text="从：">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="SYear" Width="60"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="SMonth" Width="50"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="lmonth" runat="server" Text="月 到：">
                                            </ext:Label>
                                            <ext:ComboBox ID="ccbYearTo" runat="server" ReadOnly="true" StoreID="SYear" Width="60"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                            </ext:ComboBox>
                                            <ext:Label ID="Label1" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="ccbMonthTo" runat="server" ReadOnly="true" StoreID="SMonth" Width="50"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="Label2" runat="server" Text="月">
                                            </ext:Label>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="50">
                                            </ext:ToolbarSpacer>
                                            <ext:Label ID="Label3" runat="server" Text="科室：" />
                                            <ext:ComboBox ID="cbbdept" runat="server" StoreID="SDept" DisplayField="DEPT_NAME"
                                                ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="150"
                                                PageSize="10" ItemSelector="div.search-item" MinChars="1" ListWidth="200">
                                                <Template ID="Template1" runat="server">
                                                    <tpl for=".">
                                                        <div class="search-item">
                                                             <h3>{DEPT_NAME}</h3>
                                                             </div>
                                                      </tpl>                                                                                                       
                                                </Template>
                                            </ext:ComboBox>
                                            <ext:Label ID="Label4" runat="server" Text="费别：" />
                                            <ext:ComboBox ID="cbbType" runat="server" ReadOnly="true" ForceSelection="true" SelectOnFocus="true"
                                                SelectedIndex="0">
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
                                                        <EventMask Msg="查询中..." ShowMask="true" />
                                                        <EventMask ShowMask="true" Msg="请稍候..." />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                Text="导出Excel" Icon="PageWhiteExcel">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <ext:ExtGridPanel ID="GridPanel_Show" runat="server" StoreID="SReport" Border="true"
                                        Width="800" Height="400" AutoScroll="true" StyleSpec="margin:10px">
                                        <ExtColumnModel ID="ColumnModel1" runat="server">
                                            <Columns>
                                                <ext:ExtColumn ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室</div>"
                                                    Width="120" DataIndex="DEPT_NAME" MenuDisabled="true" Align="Right">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="PATIENT_NAME" Header="<div style='text-align:center;'>姓名</div>"
                                                    Width="120" DataIndex="PATIENT_NAME" MenuDisabled="true" Align="Right">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="PATIENT_ID" Header="<div style='text-align:center;'>ID</div>"
                                                    Width="120" DataIndex="PATIENT_ID" MenuDisabled="true" Align="Right">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="VISIT_DATE" Header="<div style='text-align:center;'>时间</div>"
                                                    Width="120" DataIndex="VISIT_DATE" MenuDisabled="true" Align="Right">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="RCPT_NO" Header="<div style='text-align:center;'>收据号</div>"
                                                    Width="120" DataIndex="RCPT_NO" MenuDisabled="true" Align="Right">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="COSTS" Header="<div style='text-align:center;'>费用</div>"
                                                    Width="120" DataIndex="COSTS" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="CHARGES" Header="<div style='text-align:center;'>应收费用</div>"
                                                    Width="120" DataIndex="CHARGES" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                            </Columns>
                                        </ExtColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Listeners>
                                            <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}; this.setHeight( Ext.getBody().getViewSize().height -60); }, this);" />
                                            <Render Handler="if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}; this.setHeight( Ext.getBody().getViewSize().height -60);" />
                                        </Listeners>
                                        <BottomBar>
                                            <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="20" StoreID="SReport"
                                                AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                        <LoadMask ShowMask="true" Msg="查询中....." />
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
