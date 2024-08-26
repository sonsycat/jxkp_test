<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusDeptList.aspx.cs"
    Inherits="GoldNet.JXKP.BonusDeptList" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />
    <style type="text/css">
        .x-grid-record-gray
        {
            background-color: #CCDDFF;
            border-right: 1px solid #666666;
            border-bottom: 1px solid #666666;
        }
        .x-grid-record-col
        {
            border-right: 1px solid #666666;
            border-bottom: 1px solid #666666;
        }
        .x-grid-record-gray-red
        {
            background-color: #CCDDFF;
            border-right: 1px solid #666666;
            border-bottom: 1px solid #666666;
            color: #FF0000;
        }
        .x-grid-record-col-red
        {
            border-right: 1px solid #666666;
            border-bottom: 1px solid #666666;
            color: #FF0000;
        }
    </style>

    <script type="text/javascript">
         var rmbMoney = function(v, p, record, rowIndex) {
              if (record.data.UNIT_CODE == null ) {
                if(Math.round((v - 0)) < 0)
                {
                    p.css="x-grid-record-gray-red";
                }
                else
                {
                    p.css="x-grid-record-gray";
                }
              }
              else
              {
                if(Math.round((v - 0)) < 0)
                {
                    p.css="x-grid-record-col-red";
                }
                else
                {
                    p.css="x-grid-record-col";
                }
                
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
          };
          
         var highLight = function(v, p, record, rowIndex) {
              if (record.data.UNIT_CODE == null ) {
                p.css="x-grid-record-gray";
              }
              else
              {
                p.css="x-grid-record-col";
              }
              return v;
          }   
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server" AutoLoad="true" GroupField="SEC_UNIT_NAME"
        GroupOnSort="false">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="SEC_UNIT_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <form id="form1" runat="server">
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server" AutoWidth="true">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:ExtGridPanel ID="GridPanel11" runat="server" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" AutoWidth="true" Height="480" Border="true" >
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Button ID="Btn_Excel" Text="导出Excel" Icon="TextColumns" runat="server" OnClick="OutExcel"
                                                AutoPostBack="true">
                                            </ext:Button>
                                            <ext:Button ID="Btn_Back" Text="返回" Icon="ReverseGreen" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Back_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                            <ext:Label ID="Label3" runat="server" Text="科室类型：" Width="60" />
                                            <ext:ComboBox ID="depttype" runat="server" Width="100" AllowBlank="true" EmptyText="请选择...">
                                                <AjaxEvents>
                                                    <Select OnEvent="Item_SelectOnChange">
                                                        <EventMask Msg='载入中...' ShowMask="true" />
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="cbbType" runat="server" ReadOnly="true" ForceSelection="true" SelectOnFocus="true"
                                                SelectedIndex="0">
                                                <Items>
                                                    <ext:ListItem Text="奖金科室" Value="1" />
                                                    <ext:ListItem Text="核算科室" Value="0" />
                                                </Items>
                                                <AjaxEvents>
                                                    <Select OnEvent="Item_SelectOnChange">
                                                        <EventMask Msg='载入中...' ShowMask="true" />
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ExtColumnModel ID="extColumnModel2" runat="server" >
                                    <Columns>
                                    </Columns>
                                </ExtColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <Listeners>
                                </Listeners>
                                <LoadMask ShowMask="true" />
                                <AjaxEvents>
                                    <DblClick OnEvent="Person_Click">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel11}.getRowsValues())" Mode="Raw" />
                                        </ExtraParams>
                                    </DblClick>
                                </AjaxEvents>
                            </ext:ExtGridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
