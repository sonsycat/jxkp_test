<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WuZi_KC.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Input.WuZi_KC" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
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
        <ext:Store ID="Store1" runat="server" AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader >
                    <Fields> 
                        <ext:RecordField Name="ST_DATE" />                       
                        <ext:RecordField Name="DEPT_NAME" />
                        <ext:RecordField Name="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" />
                        <ext:RecordField Name="ITEM_SPEC" />
                        <ext:RecordField Name="UNITS" />
                        <ext:RecordField Name="QC_AMOUNT" />
                        <ext:RecordField Name="INP_AMOUNT" />
                        <ext:RecordField Name="OUT_AMOUNT" />
                        <ext:RecordField Name="AMOUNT" />
                        <ext:RecordField Name="CLASS_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
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
                                             <%--<ext:Label ID="Label1" runat="server" Text="开始时间：" />
                                             <ext:DateField ID="stardate" runat="server" FieldLabel="开始时间：" Width="80" EnableKeyEvents="true" />
                                             <ext:Label ID="Label2" runat="server" Text="结束时间：" />
                                             <ext:DateField ID="enddate" runat="server" FieldLabel="结束时间：" Width="80" EnableKeyEvents="true" />--%>
                                            <%-- <ext:Label ID="Label5" runat="server" Text="类别：" />
                                            <ext:ComboBox ID="leibie_SelectDept" runat="server" AllowBlank="true"  Width="180" EmptyText="请选择类别"  FieldLabel="类别选择">
                                            </ext:ComboBox>--%>
                                             <ext:Label ID="Label3" runat="server" Text="科室：" />
                                              <ext:ComboBox ID="dept_SelectDept" runat="server" AllowBlank="true"  Width="180" EmptyText="请选择科室"  FieldLabel="科室选择">
                                            </ext:ComboBox>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
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
                                        <ext:Column Header="时间" Width="100" Align="Left" Sortable="true"  MenuDisabled="true" 
                                            ColumnID="ST_DATE" DataIndex="ST_DATE" Hidden="false">
                                        </ext:Column>
                                   
                                        <ext:Column Header="科室名称" Width="150" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="DEPT_NAME" DataIndex="DEPT_NAME">
                                           <%-- <Renderer Fn="rmbMoney" />--%>
                                        </ext:Column>  
                                         <ext:Column Header="项目编码" Width="150" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ITEM_CODE" DataIndex="ITEM_CODE">                                           
                                        </ext:Column>  
                                        <ext:Column Header="项目名称" Width="200" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ITEM_NAME" DataIndex="ITEM_NAME">
                                        </ext:Column>                                  
                                        <ext:Column Header="规格" Width="250" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ITEM_SPEC" DataIndex="ITEM_SPEC">
                                        </ext:Column>
                                        <ext:Column Header="单位" Width="100" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="UNITS" DataIndex="UNITS">
                                        </ext:Column>
                                        <ext:Column Header="期初数量" Width="100" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="QC_AMOUNT" DataIndex="QC_AMOUNT">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                         <ext:Column Header="申请数量" Width="100" Align="Left" Sortable="true"  MenuDisabled="true" 
                                            ColumnID="INP_AMOUNT" DataIndex="INP_AMOUNT" Hidden="false">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                         <ext:Column Header="使用数量" Width="100" Align="Left" Sortable="true"  MenuDisabled="true" 
                                            ColumnID="OUT_AMOUNT" DataIndex="OUT_AMOUNT" Hidden="false">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                         <ext:Column Header="库存数量" Width="100" Align="Left" Sortable="true"  MenuDisabled="true" 
                                            ColumnID="AMOUNT" DataIndex="AMOUNT" Hidden="false">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                         <ext:Column Header="类别" Width="200" Align="Left" Sortable="true"  MenuDisabled="true" 
                                            ColumnID="CLASS_NAME" DataIndex="CLASS_NAME" Hidden="false">                                           
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
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
                                </SelectionModel>
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
