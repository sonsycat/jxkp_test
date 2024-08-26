<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideViewDetail.aspx.cs" Inherits="GoldNet.JXKP.zlgl.SysManage.GuideViewDetail" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
 <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>
  
    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg) {
            Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
            
        function dbonclick(item_class)
		{
		   document.location.href="dept_income_item.aspx?item_class="+item_class;  
		}
		function edit(id,guidetypeid) {
            document.location.href="GuideViewCont.aspx?ID="+id+"&GuideTypeID="+guidetypeid;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server" AutoLoad="true" GroupField="GUIDETYPE" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="GUIDETYPE" />
                    <ext:RecordField Name="GUIDENAME" />
                    <ext:RecordField Name="GUIDENUM" />
                    <ext:RecordField Name="MANADEPT" />
                    <ext:RecordField Name="GUIDETYPEID" />
                    <ext:RecordField Name="GUIDENAMEID" />
                      <ext:RecordField Name="TYPESIGN" />
                    
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server" >
        <Body>
            <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="1">
                        <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                            TrackMouseOver="true" AutoWidth="true" Height="450" Border="false" AutoScroll="true">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                    <Items>
                                    <ext:Button ID="BtnGuideName" runat="server" Text="修改考评指标" Icon="NoteEdit">
                                <AjaxEvents>
                                    <Click OnEvent="BtnGuideName_Click">
                                    </Click>
                                </AjaxEvents>
                            </ext:Button>
                            <ext:Button ID="BtnGuideType" runat="server" Text="修改考评分类" Icon="NoteEdit">
                                <AjaxEvents>
                                    <Click OnEvent="BtnGuideType_Click">
                                    </Click>
                                </AjaxEvents>
                            </ext:Button>
                                      
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="GUIDETYPE" Header="考评类别" Width="200" Align="left" Sortable="true"
                                        DataIndex="GUIDETYPE" MenuDisabled="true" />
                                    <ext:Column ColumnID="GUIDENAME" Header="考评指标" Width="300" Align="left" Sortable="true"
                                        DataIndex="GUIDENAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="GUIDENUM" Header="指标分值" Width="100" Align="right" Sortable="true"
                                        DataIndex="GUIDENUM" MenuDisabled="true" />
                                    <ext:Column ColumnID="MANADEPT" Header="主管部门" Width="300" Align="left" Sortable="true"
                                        DataIndex="MANADEPT" MenuDisabled="true" />
                                        
                                        <ext:Column ColumnID="GUIDETYPEID" Header="" Width="200" Align="left" Sortable="true" Hidden="true"
                                        DataIndex="GUIDETYPEID" MenuDisabled="true" />
                                         <ext:Column ColumnID="GUIDENAMEID" Header="" Width="200" Align="left" Sortable="true" Hidden="true"
                                        DataIndex="GUIDENAMEID" MenuDisabled="true" />
                                        
                                         <ext:CommandColumn Width="60" Align="Center"  Header="操作">
                                    <Commands>
                                        <ext:GridCommand Icon="BookEdit" CommandName="DetailView">
                                            <ToolTip Text="修改考评内容" />
                                        </ext:GridCommand>
                                    </Commands>
                                   
                                </ext:CommandColumn>
                                    
                                </Columns>
                            </ColumnModel>
                           <LoadMask ShowMask="true" />
                               
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <Listeners>
                            <Command   Handler="edit(record.data.GUIDENAMEID,record.data.GUIDETYPEID);" />
                        </Listeners>
                            <View>
                                <ext:GroupingView ID="GroupingView1" HideGroupedColumn="true" runat="server" GroupTextTpl='{text} ({[values.rs.length]})'
                                    EnableRowBody="false">
                                </ext:GroupingView>
                            </View>
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
