<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffInfoUpLoad.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.StaffInfoUpLoad" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <DocumentReady Handler="var myDate = new Date();var year = myDate.getFullYear();#{NumYear}.setValue(year)" />
        </Listeners>
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <North>
                        <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0">
                            <Items>
                                <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                <ext:NumberField ID="NumYear" runat="server" MaxLength="4" Width="40" MaxValue="3000"
                                    MinValue="1000">
                                </ext:NumberField>
                                <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                <ext:ComboBox runat="server" ID="Comb_StartMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月　　" />
                                <ext:ToolbarTextItem ID="ToolbarTextItem3" runat="server" Text="上报类别" />
                                <ext:ComboBox ID="cboUploadType" runat="server" Width="100">
                                    <Items>
                                        <ext:ListItem Value="0" Text="单位信息" />
                                        <ext:ListItem Value="1" Text="信息化建设" />
                                        <ext:ListItem Value="2" Text="专科中心信息" />
                                        <ext:ListItem Value="3" Text="科室信息" />
                                        <ext:ListItem Value="4" Text="人员信息" />
                                        <ext:ListItem Value="5" Text="特殊诊疗" />
                                        <ext:ListItem Value="6" Text="课题" />
                                        <ext:ListItem Value="7" Text="成果" />
                                        <ext:ListItem Value="8" Text="专著" />
                                        <ext:ListItem Value="9" Text="论文" />
                                        <ext:ListItem Value="10" Text="学术会议" />
                                        <ext:ListItem Value="11" Text="新技术新业务" />
                                        <ext:ListItem Value="12" Text="人才培养" />
                                    </Items>
                                </ext:ComboBox>
                                <ext:Button ID="btnQuery" runat="server" Text=" 查询 " Icon="DatabaseGo">
                                    <AjaxEvents>
                                        <Click OnEvent="GetQueryPortalet" Before="if(#{cboUploadType}.getSelectedItem().text == '') 
                                                                                    {Ext.Msg.show({ title: '信息提示', msg: '请选择上报类别', icon: 'ext-mb-info', buttons: { ok: true }  }); return false;}">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="Type" Value="Ext.encode(#{cboUploadType}.getSelectedItem().value)" Mode="Raw" ></ext:Parameter>
                                            </ExtraParams>
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarFill>
                                </ext:ToolbarFill>
                                <ext:Button ID="btnExcel" runat="server" Text=" Excel上报 " Icon="PageWhiteExcel" OnClick="OutExcel" AutoPostBack="true">
                                </ext:Button>
                                <ext:Button ID="btnXml" runat="server" Text=" 导出上报数据 " Icon="Xhtml" OnClick="OutXml" AutoPostBack="true">
                                </ext:Button>
                                <ext:Button ID="btnDataBase" runat="server" Text=" 人员数据存档 " Icon="DatabaseTable">
                                    <AjaxEvents>
                                        <Click Success="Ext.Msg.show({ title: '信息提示', msg: '存档成功', icon: 'ext-mb-info', buttons: { ok: true }  })" OnEvent="InsertLoad">
                                            <EventMask Msg="存档中..." ShowMask="true"/>
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:GridPanel ID="GridPanel_Show" runat="server" Border="false" StoreID="Store1"
                            Height="800" Width="600" AutoScroll="true">
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="32" Resizable="true">
                                    </ext:RowNumbererColumn>
                                </Columns>
                            </ColumnModel>
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="20" StoreID="Store1"
                                    AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                                </ext:PagingToolbar>
                            </BottomBar>
                            <LoadMask ShowMask="true" />
                        </ext:GridPanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
