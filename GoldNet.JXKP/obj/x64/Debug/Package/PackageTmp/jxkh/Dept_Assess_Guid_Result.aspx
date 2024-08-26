<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dept_Assess_Guid_Result.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.Dept_Assess_Guid_Result" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>科室绩效考核查询</title>
    <style type="text/css">
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
        var applyFilter = function() {
            Store1.filterBy(getRecordFilter());
        };
        var getRecordFilter = function() {
            var f = [];
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'DEPT_NAME', record);
                }
            });
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'STATION_NAME', record);
                }
            });
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'USER_NAME', record);
                }
            });


            var len = f.length;
            return function(record) {
                if (f[0].filter(record) || f[1].filter(record) || f[2].filter(record)) {
                    return true;
                }
                return false;
            }
        };
        var filterString = function(value, dataIndex, record) {
            var val = record.get(dataIndex);
            if (typeof val != "string") {
                return value.length == 0;
            }
            return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
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

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="GUIDE_CODE" />
                    <ext:RecordField Name="GUIDE_NAME" />
                    <ext:RecordField Name="GUIDE_VALUE" />
                    <ext:RecordField Name="GUIDE_CAUSE" />
                    <ext:RecordField Name="GUIDE_FACT" />
                    <ext:RecordField Name="GUIDE_I_VALUE" />
                    <ext:RecordField Name="GUIDE_D_VALUE" />
                    <ext:RecordField Name="GUIDE_F_VALUE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader Root="Staffdepts">
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="DEPT_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader Root="Staffdepts">
                <Fields>
                    <ext:RecordField Name="GUIDE_CODE" />
                    <ext:RecordField Name="GUIDE_NAME" />
                    <ext:RecordField Name="INPUT_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server" FitHeight="true">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="1">
                        <ext:GridPanel ID="GridPanel_List" runat="server" BodyBorder="false" AutoScroll="true"
                            Border="false" StoreID="Store1" StripeRows="true" TrackMouseOver="true" Height="480"
                            AutoWidth="true">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="考核年度：" />
                                        <ext:ComboBox runat="server" ID="Comb_StartYear" Width="60" ListWidth="60" SelectedIndex="0">
                                            <AjaxEvents>
                                                <Select OnEvent="Comb_Year_Selected">
                                                    <EventMask Msg="请稍候..." ShowMask="true" />
                                                </Select>
                                            </AjaxEvents>
                                            <Listeners>
                                                <Change Handler="Store1.removeAll();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem3" runat="server" Text="考核名称：" />
                                        <ext:ComboBox runat="server" ID="Comb_AssessName" Width="140" ListWidth="140">
                                            <Listeners>
                                                <Change Handler="Store1.removeAll();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="科室类别：" />
                                        <ext:ComboBox runat="server" ID="ComboBoxdepttype" Width="140" ListWidth="140">
                                        </ext:ComboBox>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem5" runat="server" Text="科室名称：" />
                                        <ext:ComboBox runat="server" ID="Combodept" Width="140" ListWidth="240" StoreID="Store3"
                                            DisplayField="DEPT_NAME" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                            PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="科室信息">
                                            <Template ID="Template1" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                            </Template>
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem6" runat="server" Text="指标名称：" />
                                        <ext:ComboBox runat="server" ID="Comboguide" Width="140" ListWidth="240" StoreID="Store2"
                                            DisplayField="GUIDE_NAME" ValueField="GUIDE_CODE" TypeAhead="false" LoadingText="Searching..."
                                            PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="指标信息">
                                            <Template ID="Template2" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{GUIDE_CODE}</span>{GUIDE_NAME}</h3>
                                          </div>
                                       </tpl>
                                            </Template>
                                        </ext:ComboBox>
                                        <ext:Button ID="Btn_View" runat="server" Icon="Zoom" Text="查看">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_View_Clicked">
                                                    <EventMask ShowMask="true" Msg="请稍候..." />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_Excel" runat="server" Icon="PageWhiteExcel" Text="EXCEL导出" OnClick="OutExcel"
                                            AutoPostBack="true">
                                        </ext:Button>

                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn />
                                    <ext:Column Header="科室名称" Width="90" ColumnID="DEPT_NAME" DataIndex="DEPT_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="DEPT_CODE" DataIndex="DEPT_CODE" Hidden="true" />
                                    <ext:Column Header="指标名称" Width="160" ColumnID="GUIDE_NAME" DataIndex="GUIDE_NAME"
                                        MenuDisabled="true">
                                    </ext:Column>
                                    <ext:Column ColumnID="GUIDE_CODE" DataIndex="GUIDE_CODE" Hidden="true" />
                                    <ext:Column Header="指标分值" Width="110" ColumnID="GUIDE_VALUE" DataIndex="GUIDE_VALUE"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column Header="目标值" Width="110" ColumnID="GUIDE_CAUSE" DataIndex="GUIDE_CAUSE"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column Header="完成值" Width="120" ColumnID="GUIDE_FACT" DataIndex="GUIDE_FACT"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column Header="加分" Width="120" ColumnID="GUIDE_I_VALUE" DataIndex="GUIDE_I_VALUE"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column Header="扣分" Width="120" ColumnID="GUIDE_D_VALUE" DataIndex="GUIDE_D_VALUE"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column Header="得分" Width="120" ColumnID="GUIDE_F_VALUE" DataIndex="GUIDE_F_VALUE"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" Msg="载入中..." />
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="30" StoreID="Store1"
                                    AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                                    <Items>
                                        <ext:TextField ID="txt_SearchTxt" runat="server" EmptyText="查找信息">
                                            <ToolTips>
                                                <ext:ToolTip ID="ToolTip1" runat="server" Html="根据科室或岗位名称关键字查找">
                                                </ext:ToolTip>
                                            </ToolTips>
                                        </ext:TextField>
                                        <ext:Button ID="btn_Search" Icon="Zoom" runat="server" Text="查询">
                                            <Listeners>
                                                <Click Fn="applyFilter" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:PagingToolbar>
                            </BottomBar>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
