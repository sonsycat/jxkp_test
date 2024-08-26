<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pers_Type_List.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SpecManager.Pers_Type_List" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript">
        
         var RefreshData = function() {
            Store1.reload();
        }   
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server" AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="ID">
                    <Fields>
                        <ext:RecordField Name="ID" />
                        <ext:RecordField Name="PERS_SORT_NAME" />
                        
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
                       
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="序号" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ID" DataIndex="ID">
                                        </ext:Column>
                                        <ext:Column Header="人员类别" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="PERS_SORT_NAME" DataIndex="PERS_SORT_NAME">
                                        </ext:Column>
                                       
                                       <ext:CommandColumn Header="权限" Sortable="true" ColumnID="Columns5" Align="Center" Width="50">
                                            <Commands>
                                                <ext:GridCommand Icon="CogStart" CommandName="Show" >
                                                </ext:GridCommand>
                                            </Commands>
                                        </ext:CommandColumn>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <AjaxEvents>
                                            <RowSelect OnEvent="RowSelect" Buffer="250">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{Store1}" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="ID" Value="this.getSelected().PERS_SORT_NAME" Mode="Raw" />
                                                </ExtraParams>
                                            </RowSelect>
                                        </AjaxEvents>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                                <AjaxEvents>
                                    <Command OnEvent="SetPower">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="this.store.getAt(rowIndex).get('ID')"  Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </Command>
                                </AjaxEvents>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>

     <ext:Window ID="PersTypeEdit" runat="server" Icon="Group" Title="编辑权限" Width="500"
            Height="450"  AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
           
    </ext:Window>
    </form>
</body>
</html>
