<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusDeptShow.aspx.cs"
    Inherits="GoldNet.JXKP.BonusDeptShow" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        var rmbMoney = function(v) {
               if(v==null||v=="")
               {
               return "";
               }
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
       var pctChange = function(value) {
            return  value + '%';
        }
          function FormatRenderNew(v, p, record, rowIndex) {
            var newFlg = false;
            if (record.data.UNIT_BONUS !=record.data.BONUS_PERSONS_VALUE && record.data.FLAGS == '已提交')
                newFlg = true;
            var colors = ["red", "blue", "purple", "lime", "green", "navy", "olive", "black", "yellow", "maroon"];
           var template = '<span style="color:{0};">{1}</span>';
            return newFlg ? String.format(template,colors[0], record.data.UNIT_NAME) : record.data.UNIT_NAME;
        }
           
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server" AutoLoad="true" GroupField="SEC_UNIT_NAME"
        GroupOnSort="false">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="UNIT_NAME" />
                    <ext:RecordField Name="SEC_UNIT_NAME" />
                    <ext:RecordField Name="UNIT_BONUS" />
                    <ext:RecordField Name="BONUS_PERSONS_VALUE" />
                    <ext:RecordField Name="FLAGS" />
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
                            TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                    <Items>
                                        <ext:Label ID="func" runat="server" Text="选择奖金：" Width="40">
                                        </ext:Label>
                                        <ext:ComboBox ID="comindex" runat="server" AllowBlank="true" EmptyText="请选择奖金" Width="300"
                                            FieldLabel="奖金选择">
                                            <AjaxEvents>
                                                <Select OnEvent="SelectedFunc">
                                                    <EventMask ShowMask="true" />
                                                </Select>
                                            </AjaxEvents>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server" Enabled="false">
                                <Columns>
                                    <ext:Column ColumnID="SEC_UNIT_NAME" Header="二级科" Width="100" Align="left" Sortable="true"
                                        DataIndex="SEC_UNIT_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="UNIT_NAME" Header="科室名称" Width="400" Align="left" Sortable="true"
                                        DataIndex="UNIT_NAME" MenuDisabled="true">
                                        <Renderer Fn="FormatRenderNew" />
                                        </ext:Column>
                                    <ext:Column ColumnID="UNIT_BONUS" Header="<div style='text-align:center;'>应发数</div>" Width="120" Align="Right" Sortable="true"
                                        DataIndex="UNIT_BONUS" MenuDisabled="true">
                                       <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column ColumnID="BONUS_PERSONS_VALUE" Header="<div style='text-align:center;'>实发数</div>" Width="120" Align="Right" Sortable="true"
                                        DataIndex="BONUS_PERSONS_VALUE" MenuDisabled="true">
                                      <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column ColumnID="FLAGS" Header="是否提交" Width="100" Align="left" Sortable="true"
                                        DataIndex="FLAGS" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    <Listeners>
                                        <RowSelect Handler="#{Button_set}.enable()" />
                                        <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{Button_set}.disable()}" />
                                    </Listeners>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            
                            <View>
                                <ext:GroupingView ID="GroupingView1" HideGroupedColumn="true" runat="server" GroupTextTpl='{text} ({[values.rs.length]})'
                                    EnableRowBody="false">
                                    <HeaderRows>
                                            <ext:HeaderRow>
                                                <Columns>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                    <Component>
                                                            <ext:Label runat="server" ID="TextField1" ReadOnly="true" Text="总计"  StyleSpec="text-align:left">
                                                            </ext:Label>
                                                        </Component>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                     <Component>
                                                            <ext:TextField runat="server" ID="yf_bonus" ReadOnly="true"  StyleSpec="text-align:right">
                                                            </ext:TextField>
                                                        </Component>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                        <Component>
                                                            <ext:TextField runat="server" ID="sf_bonus" ReadOnly="true"  StyleSpec="text-align:right">
                                                            </ext:TextField>
                                                        </Component>
                                                    </ext:HeaderColumn>
                                                </Columns>
                                            </ext:HeaderRow>
                                        </HeaderRows>
                                </ext:GroupingView>
                                
                                
                            </View>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
