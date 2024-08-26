<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BenefitAdjustList.aspx.cs"
    Inherits="GoldNet.JXKP.BenefitAdjustList" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />

    <script type="text/javascript">
          var RefreshData = function() {
              Store1.reload();
          }
          var applyFilter = function() {
              Store1.filterBy(getRecordFilter());
          };
          var getRecordFilter = function() {
              var f = [];
              f.push({
                  filter: function(record) {
                      return filterString(txt_SearchTxt.getValue(), 'DEPTNAME', record);
                  }
              });
              f.push({
                  filter: function(record) {
                      return filterDate(txt_SearchTxt.getValue(), 'ADJUSTDATE', record);
                  }
              });
              f.push({
                  filter: function(record) {
                      return filterString(txt_SearchTxt.getValue(), 'TYPE', record);
                  }
              });
              f.push({
                  filter: function(record) {
                      return filterNumber(txt_SearchTxt.getValue(), 'MONEY', record);
                  }
              });
              f.push({
                  filter: function(record) {
                      return filterString(txt_SearchTxt.getValue(), 'DIRECTION', record);
                  }
              });

              var len = f.length;
              return function(record) {
                  if (checkIsDate(txt_SearchTxt.getValue())) {
                      if (f[1].filter(record)) {
                          return true;
                      }
                      else {
                          return false;
                      }
                  } else {
                      if (f[0].filter(record) || f[2].filter(record) || f[3].filter(record) || f[4].filter(record)) {
                          return true;
                      }
                      else {
                          return false;
                      }
                  }
              }
          };
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

          }    
    </script>

    <script type="text/javascript" src="../Orthers/Cbouns.js"></script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <ext:Store ID="Store1" AutoLoad="true" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPTNAME">
                    </ext:RecordField>
                    <ext:RecordField Name="ADJUSTDATE">
                    </ext:RecordField>
                    <ext:RecordField Name="TYPE">
                    </ext:RecordField>
                    <ext:RecordField Name="MONEY">
                    </ext:RecordField>
                    <ext:RecordField Name="DIRECTION">
                    </ext:RecordField>
                     <ext:RecordField Name="INPUTER">
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
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" Height="480">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Button ID="Btn_Add" Text="增加" Icon="Add" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Add_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Edit" Text="编辑" Icon="NoteEdit" runat="server" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Edit_Click">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Del" Text="删除" Icon="Delete" runat="server" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Del_Click">
                                                        <Confirmation BeforeConfirm="config.confirmation.message = '你确定要删除吗？';" Title="系统提示"
                                                            ConfirmRequest="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                             <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="SYear" Width="60"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="SMonth" Width="50"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="lmonth" runat="server" Text="月">
                                            </ext:Label>
                                            <%--<ext:Button ID="Btn_Ref" runat="server" Text="显示全部" Icon="ArrowRefresh">
                                            <AjaxEvents>
                                                 <Click OnEvent="Btn_Ref_Click">
                                                 </Click>
                                             </AjaxEvents>                                             
                                            </ext:Button>--%>
                                              <ext:Button ID="Btn_Search" runat="server" Text="查询" Icon="FolderMagnify">
                                            <AjaxEvents>
                                                 <Click OnEvent="Btn_Search_Click">
                                                 </Click>
                                             </AjaxEvents>                                             
                                            </ext:Button>
                                            <ext:Button ID="Btn_look" runat="server" Text="查看" Icon="Zoom" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Look_Click">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="DEPTNAME" Header="科室" Width="200" DataIndex="DEPTNAME" MenuDisabled="true"
                                            Align="Center">
                                        </ext:Column>
                                        <ext:Column ColumnID="ADJUSTDATE" Header="时间" Width="200" DataIndex="ADJUSTDATE"
                                            MenuDisabled="true" Align="Center">
                                        </ext:Column>
                                        <ext:Column ColumnID="TYPE" Header="调整类别" Width="200" DataIndex="TYPE" MenuDisabled="true"
                                            Align="Center">
                                        </ext:Column>
                                        <ext:Column ColumnID="MONEY" Header="<div style='text-align:center;'>调整金额</div>"
                                            Width="200" DataIndex="MONEY" MenuDisabled="true" Align="Right">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="DIRECTION" Header="调整方向" Width="200" DataIndex="DIRECTION"
                                            MenuDisabled="true" Align="Center">
                                        </ext:Column>
                                        <ext:Column ColumnID="INPUTER" Header="调整人" Width="200" DataIndex="INPUTER"
                                            MenuDisabled="true" Align="Center">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <Listeners>
                                            <RowSelect Handler="#{Btn_Edit}.enable();#{Btn_Del}.enable();#{Btn_look}.enable()" />
                                            <RowDeselect Handler="if (!#{GridPanel2}.hasSelection()) {#{Btn_Del}.disable();#{Btn_Edit}.disable();#{Btn_look}.disable()}" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <AjaxEvents>
                                    <DblClick OnEvent="Btn_Look_Click">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                        </ExtraParams>
                                    </DblClick>
                                </AjaxEvents>
                                <LoadMask ShowMask="true" />
                                <View>
                                    <ext:GridView ID="GridView1" runat="server">
                                        <HeaderRows>
                                            <ext:HeaderRow>
                                                <Columns>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                        <Component>
                                                            <ext:TextField runat="server" ID="summoney" ReadOnly="true" StyleSpec="text-align:right">
                                                            </ext:TextField>
                                                        </Component>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                </Columns>
                                            </ext:HeaderRow>
                                        </HeaderRows>
                                    </ext:GridView>
                                </View>
                                <BottomBar>
                                    <ext:Toolbar runat="server">
                                        <Items>
                                            <ext:Label runat="server" ID="label1" Text="条件查询">
                                            </ext:Label>
                                            <ext:TextField ID="txt_SearchTxt" runat="server" EmptyText="查找">
                                             <ToolTips>
                                                                    <ext:ToolTip ID="ToolTip1" runat="server" Html="根据科室编码，科室名称，时间模糊查找">
                                                                    </ext:ToolTip>
                                                                </ToolTips>
                                            </ext:TextField>
                                            <ext:Button ID="Button1" Icon="FolderMagnify" runat="server" Text="查询">
                                                 <AjaxEvents>
                                                                    <Click OnEvent="select_benefit">
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                    </Click>
                                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </BottomBar>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
        <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="科室效益数据调整录入" Width="400"
            Height="460" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
            ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
            BodyStyle="background-color:Transparent;">
        </ext:Window>
        <%--<ext:Window ID="Search" runat="server" Icon="Group" Title="科室效益数据调整查询" Width="460"
            Height="340" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
            ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
            BodyStyle="background-color:Transparent;">
            <AjaxEvents>
                <Hide OnEvent="WindowsClose">
                </Hide>
            </AjaxEvents>
        </ext:Window>--%>
    </div>
    </form>
</body>
</html>
