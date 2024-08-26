<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GongZuoLiang_Select.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Input.GongZuoLiang_Select" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        function backToList() {
            window.navigate("RoleList.aspx");
        }
        var RefreshData = function () {
            Store1.reload();
        }
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager2" runat="server" AjaxMethodNamespace="Goldnet" />
        <ext:Store ID="Store1" runat="server" AutoLoad="true" >
            <Reader>
                <ext:JsonReader >
                    <Fields>
                        <ext:RecordField Name="DEPT_NAME" />
                        <ext:RecordField Name="XMMC" />
                        <ext:RecordField Name="JBMC" />
                        <ext:RecordField Name="AMOUNT" />
                        <ext:RecordField Name="JF" />
                        <ext:RecordField Name="ZJF" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
<ext:Store ID="SDept" runat="server" AutoLoad="true">
    </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">

            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" Height="480" AutoWidth="true">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_ZLJK" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
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
                                            <ext:ComboBox ID="cbbType" runat="server" ReadOnly="true" ForceSelection="true"
                                                SelectOnFocus="true" Width="200">
                                                <Items>
                                                    <ext:ListItem Text="执行医生" Value="1" />
                                                    <ext:ListItem Text="护士" Value="2" />
                                                    <ext:ListItem Text="开单医生" Value="3" />
                                                </Items>
                                            </ext:ComboBox>                             
                                            <ext:Button ID="Buttonlist" runat="server" Text="查询" Icon="ArrowRefresh">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetQueryPortalet" Timeout="9999999">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                 Text="导出Excel" Icon="PageWhiteExcel" CausesValidation="false">
                                              </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        </Items>
                                    </ext:Toolbar>

                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="科室名称" Width="150" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="DEPT_NAME" DataIndex="DEPT_NAME" Hidden="false">
                                        </ext:Column>
                                        <ext:Column Header="项目名称" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="XMMC" DataIndex="XMMC">
                                        </ext:Column>
                                        <ext:Column Header="级别名称" Width="100" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="JBMC" DataIndex="JBMC">
                                        </ext:Column>
                                        <ext:Column Header="数量" Width="150" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="AMOUNT" DataIndex="AMOUNT">
                                        </ext:Column>
                                         <ext:Column Header="积分" Width="150" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="JF" DataIndex="JF">
                                        </ext:Column>
                                        <ext:Column Header="总积分" Width="150" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ZJF" DataIndex="ZJF">
                                        </ext:Column>                                                                 
                                    </Columns>
                                </ColumnModel>
                          <%--      <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <AjaxEvents>
                                            <RowSelect OnEvent="RowSelect" Buffer="250">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{Store1}" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="ROLE_ID" Value="this.getSelected().id" Mode="Raw" />
                                                </ExtraParams>
                                            </RowSelect>
                                        </AjaxEvents>
                                    </ext:RowSelectionModel>
                                </SelectionModel>--%>
                                <LoadMask ShowMask="true" />
                                 <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="20" StoreID="Store1"
                                        AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                    </ext:PagingToolbar>
                                </BottomBar>
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

