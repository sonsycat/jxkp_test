<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Adjust_Income.aspx.cs" Inherits="GoldNet.JXKP.cbhs.Report.Adjust_Income" %>
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

        };
        var template = '<span style="color:{0};">{1}</span>';

        var namecolor = function (value) {
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
        <Listeners>
            <DocumentReady Handler="cbbType.setWidth(80);" />
        </Listeners>
    </ext:ScriptManager>
    <form id="form1" runat="server">
    <ext:Store ID="SReport" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="AAA">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPT_CODE">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="ZL">
                    </ext:RecordField>
                    <ext:RecordField Name="GYY">
                    </ext:RecordField>
                    <ext:RecordField Name="YQ">
                    </ext:RecordField>
                    <ext:RecordField Name="HJ">
                    </ext:RecordField>
                    <ext:RecordField Name="ICUZL">
                    </ext:RecordField>
                    <ext:RecordField Name="LCUYQ">
                    </ext:RecordField>
                    <ext:RecordField Name="ICUHJ">
                    </ext:RecordField>

                    <ext:RecordField Name="SSF">
                    </ext:RecordField>
                    <ext:RecordField Name="SSYQ">
                    </ext:RecordField>
                    <ext:RecordField Name="SSHJ">
                    </ext:RecordField>
                    <ext:RecordField Name="FSF">
                    </ext:RecordField>
                    <ext:RecordField Name="CTF">
                    </ext:RecordField>
                    <ext:RecordField Name="DZF">
                    </ext:RecordField>

                    <ext:RecordField Name="JYF">
                    </ext:RecordField>
                    <ext:RecordField Name="QJF">
                    </ext:RecordField>
                    <ext:RecordField Name="BLF">
                    </ext:RecordField>
                    <ext:RecordField Name="FCDJQBHJ">
                    </ext:RecordField>
                    <ext:RecordField Name="Y">
                    </ext:RecordField>
                    <ext:RecordField Name="WC">
                    </ext:RecordField>

                    <ext:RecordField Name="XF">
                    </ext:RecordField>
                    <ext:RecordField Name="ZJ">
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
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            
                                            <ext:ComboBox ID="cbbType" runat="server" ReadOnly="true" ForceSelection="true" SelectOnFocus="true"
                                                SelectedIndex="1">
                                                <Items>
                                                    <ext:ListItem Text="收付实现" Value="1" />
                                                    <ext:ListItem Text="责权发生" Value="0" />
                                                </Items>
                                            </ext:ComboBox>
                                            
                                            <ext:ToolbarSpacer ID="ToolbarSpacer4" runat="server" Width="20">
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
                                    <center>
                                        <h2>
                                            门诊及住院收入报表</h2>
                                    </center>
                                    <ext:ExtGridPanel ID="GridPanel_Show" runat="server" StoreID="SReport" Border="true"
                                        Width="800" Height="400" AutoScroll="true" StyleSpec="margin:10px">
                                        <ExtColumnModel ID="ColumnModel1" runat="server">
                                            <Columns>
                                                <ext:ExtColumn ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室</div>"
                                                    Width="120" DataIndex="DEPT_NAME" MenuDisabled="true">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ZL" Header="<div style='text-align:center;'>治疗费</div>"
                                                    Width="120" DataIndex="ZL" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="GYY" Header="<div style='text-align:center;'>高压氧</div>"
                                                    Width="120" DataIndex="GYY" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="YQ" Header="<div style='text-align:center;'>氧气费</div>"
                                                    Width="120" DataIndex="YQ" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="HJ" Header="<div style='text-align:center;'>合计</div>"
                                                    Width="120" DataIndex="HJ" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ICUZL" Header="<div style='text-align:center;'>ICU治疗</div>"
                                                    Width="120" DataIndex="ICUZL" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="LCUYQ" Header="<div style='text-align:center;'>ICU氧气</div>"
                                                    Width="120" DataIndex="LCUYQ" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ICUHJ" Header="<div style='text-align:center;'>合计</div>"
                                                    Width="120" DataIndex="ICUHJ" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="SSF" Header="<div style='text-align:center;'>手术费</div>"
                                                    Width="120" DataIndex="SSF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="SSYQ" Header="<div style='text-align:center;'>手术氧气</div>"
                                                    Width="120" DataIndex="SSYQ" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="SSHJ" Header="<div style='text-align:center;'>合计</div>"
                                                    Width="120" DataIndex="SSHJ" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="FSF" Header="<div style='text-align:center;'>放射</div>"
                                                    Width="120" DataIndex="FSF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="CTF" Header="<div style='text-align:center;'>CT费</div>"
                                                    Width="120" DataIndex="CTF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="DZF" Header="<div style='text-align:center;'>电诊费</div>"
                                                    Width="120" DataIndex="DZF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="JYF" Header="<div style='text-align:center;'>检验费</div>"
                                                    Width="120" DataIndex="JYF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="QJF" Header="<div style='text-align:center;'>腔镜费</div>"
                                                    Width="120" DataIndex="QJF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="BLF" Header="<div style='text-align:center;'>病理费</div>"
                                                    Width="120" DataIndex="BLF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="FCDJQBHJ" Header="<div style='text-align:center;'>合计</div>"
                                                    Width="120" DataIndex="FCDJQBHJ" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="Y" Header="<div style='text-align:center;'>药费</div>"
                                                    Width="120" DataIndex="Y" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="WC" Header="<div style='text-align:center;'>卫材费</div>"
                                                    Width="120" DataIndex="WC" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="XF" Header="<div style='text-align:center;'>血费</div>"
                                                    Width="120" DataIndex="XF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ZJ" Header="<div style='text-align:center;'>总计</div>"
                                                    Width="120" DataIndex="ZJ" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                
                                            </Columns>
                                            
                                        </ExtColumnModel>
                                        
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Listeners>
                                            <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}; this.setHeight( Ext.getBody().getViewSize().height -100); }, this);" />
                                            <Render Handler="if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}; this.setHeight( Ext.getBody().getViewSize().height -100);" />
                                        </Listeners>
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
