<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DoctorDetail.aspx.cs" Inherits="GoldNet.JXKP.DoctorDetail" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
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
        <Listeners>
            <DocumentReady Handler="cbbType.setWidth(80);" />
        </Listeners>
    </ext:ScriptManager>
    <form id="form1" runat="server">
    <ext:Store ID="SReport" AutoLoad="true" runat="server" OnRefreshData="Store_RefreshData" >
        <Reader>
            <ext:JsonReader>
                <Fields>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SDept" runat="server" AutoLoad="true">
    </ext:Store>
    <ext:Store ID="SCostitem" runat="server" AutoLoad="true">
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
                                            <ext:DateField ID="stardate" runat="server" FieldLabel="开始时间：" Width="100" EnableKeyEvents="true"
                                                ReadOnly="true" />
                                            <ext:KeyNav ID="stardate1" runat="server" Target="stardate">
                                                <Enter Handler="var str = document.getElementById('stardate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('stardate').value   =   str.replace(reg, '$1-$2-$3');" />
                                            </ext:KeyNav>
                                            <ext:DateField ID="enddate" runat="server" FieldLabel="结束时间：" Width="100" EnableKeyEvents="true"
                                                ReadOnly="true" />
                                            <ext:KeyNav ID="enddate1" runat="server" Target="enddate">
                                                <Enter Handler="var str = document.getElementById('enddate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('enddate').value   =   str.replace(reg, '$1-$2-$3');" />
                                            </ext:KeyNav>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            <ext:ComboBox ID="cbbType" runat="server" ReadOnly="true" ForceSelection="true" SelectOnFocus="true" SelectedIndex="0">
                                                <Items>
                                                    <ext:ListItem Text="门诊" Value="OUTP_BILL_DOCTOR" />
                                                    <ext:ListItem Text="住院" Value="INP_BILL_DOCTOR" />
                                                    <ext:ListItem Text="合计" Value="ALL_BILL_DOCTOR" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:Label ID="Label4" runat="server" Text="科室：">
                                            </ext:Label>
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
                                            <ext:Label ID="Label3" runat="server" Text="收入项目：" />
                                            <ext:ComboBox ID="cbb_ReckItem" runat="server" StoreID="SCostitem" DisplayField="CLASS_NAME"
                                                ValueField="CLASS_CODE" TypeAhead="false" LoadingText="Searching..." Width="150"
                                                ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                <Template ID="Template4" runat="server">
                                                  <tpl for=".">
                                                   <div class="search-item">
                                                     <h3>{CLASS_NAME}</h3>
                                                   </div>
                                                  </tpl>
                                                </Template>
                                            </ext:ComboBox>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="5">
                                            </ext:ToolbarSpacer>
                                            <ext:Button ID="btn_Query" runat="server" Text="查询" Icon="Zoom">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Query_Click" Timeout="99999999">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            <ext:Button ID="btn_Excel" runat="server"  OnClick="OutExcel" AutoPostBack="true"
                                                Text="导出Excel" Icon="PageWhiteExcel">
                                               
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <center>
                                        <h2>
                                            科室收入明细报表</h2>
                                    </center>
                                    <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="SReport" Border="true"
                                        Width="800" Height="400" AutoScroll="true" StyleSpec="margin:10px">
                                        <ColumnModel ID="ColumnModel1" runat="server">
                                            <Columns>
                                            </Columns>
                                        </ColumnModel>
                                        <Listeners>
                                            <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}this.setHeight( Ext.getBody().getViewSize().height -100); }, this)" />
                                            <Render Handler="if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}this.setHeight( Ext.getBody().getViewSize().height -100);" />
                                        </Listeners>
                                        <LoadMask ShowMask="true" />
                                        <BottomBar>
                                            <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="20" StoreID="SReport"
                                                AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                    </ext:GridPanel>
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
