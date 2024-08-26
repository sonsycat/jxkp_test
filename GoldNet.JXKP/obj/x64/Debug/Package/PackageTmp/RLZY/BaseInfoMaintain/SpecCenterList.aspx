<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpecCenterList.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.SpecCenterList" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>专科中心</title>
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
      var CheckForm = function() {
            if (dtfLeadold.validate() == false) {
                return false;
            }
            if (txtLeadname.validate() == false) {
                return false;
            }
            return true;
        }
        
         var DeptOpration = function() {
                var selections = CheckboxSelectionModel2.getSelections();
                var id = '';
                for(var i =0;i<selections.length;i++) {
                    if(i == selections.length -1) {
                        id = id+selections[i].data.ID;
                    } else {
                        id = id+selections[i].data.ID +',';
                    }
                }
               Goldnet.AjaxMethod.request(
                  'DeptAjaxOper',
                    {
                        params: {
                           id:id
                        },
                        success: function(result) {
                            Store.reload();
                            btnDelDept.setDisabled(true);
                        },
                        failure: function(msg) {
                            GridPanel1.el.unmask();
                        }
                    });
            }
        
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <DocumentReady Handler="var myDate = new Date();var year = myDate.getFullYear();#{NumYear}.setValue(year)" />
        </Listeners>
    </ext:ScriptManager>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Proxy>
            <ext:HttpProxy Url="/RLZY/WebService/DeptInfo.ashx">
            </ext:HttpProxy>
        </Proxy>
        <Reader>
            <ext:JsonReader Root="Staffdepts">
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="DEPT_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store" runat="server" AutoLoad="true" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="CENTER_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="YEARS" />
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
                                <ext:Label ID="Label3" runat="server" Text="年度：">
                                </ext:Label>
                                <ext:NumberField ID="NumYear" runat="server" MaxLength="4" Width="40" MaxValue="3000"
                                    MinValue="1000">
                                </ext:NumberField>
                                <ext:Label ID="Label1" runat="server" Text="　专科中心：">
                                </ext:Label>
                                <ext:ComboBox ID="cboSpceCenterInfo" runat="server" Editable="false" Width="80">
                                    <AjaxEvents>
                                        <Select OnEvent="SearchInfo" Before="if(#{NumYear}.getValue()==''){Ext.Msg.show({ title: '信息提示', msg: '请添加日期', icon: 'ext-mb-info', buttons: { ok: true }  }); return false;}
                                                       if(#{cboSpceCenterInfo}.getSelectedItem().text == '') {Ext.Msg.show({ title: '信息提示', msg: '请选择专科中心', icon: 'ext-mb-info', buttons: { ok: true }  }); return false;}">
                                            <EventMask ShowMask="true" Msg="请稍候..." />
                                        </Select>
                                    </AjaxEvents>
                                </ext:ComboBox>
                                <ext:Button Text="保存" ID="btnSaveSpecInfo" runat="server" Icon="Disk">
                                    <AjaxEvents>
                                        <Click OnEvent="InsertInfo" Before="if(#{NumYear}.getValue()==''){Ext.Msg.show({ title: '信息提示', msg: '请添加日期', icon: 'ext-mb-info', buttons: { ok: true }  }); return false;}
                                                       if(#{cboSpceCenterInfo}.getSelectedItem().text == '') {Ext.Msg.show({ title: '信息提示', msg: '请选择专科中心', icon: 'ext-mb-info', buttons: { ok: true }  }); return false;}
                                                       if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;}">
                                            <EventMask ShowMask="true" Msg="请稍候..." />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button Text="科室操作" ID="btnDept" runat="server" Icon="ApplicationAdd" Disabled="true">
                                    <AjaxEvents>
                                        <Click OnEvent="ViewCenterDept">
                                            <EventMask ShowMask="true" Msg="请稍候..." />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:Panel ID="Panel3" runat="server">
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout1" runat="server">
                                    <ext:LayoutColumn ColumnWidth=".2">
                                        <ext:Panel ID="Panel1" runat="server" Border="false" Header="false" BodyStyle="margin:10px;">
                                            <Body>
                                                <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtUnitcode" runat="server" FieldLabel="单位代码" ReadOnly="true" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtSpecname" runat="server" FieldLabel="专科中心名称" ReadOnly="true" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:ComboBox ID="cboLeadedu" runat="server" FieldLabel="带头人学历" Editable="false" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtDeploybed" runat="server" FieldLabel="学科展开床" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtGradu" runat="server" FieldLabel="本科" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtExpertfoll" runat="server" FieldLabel="专家带徒" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtFetchmaster" runat="server" FieldLabel="引进硕士" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtFetch" runat="server" FieldLabel="其中：引进" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtVicedirector" runat="server" FieldLabel="单位名称" ReadOnly="true" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:ComboBox ID="cboSpectype" runat="server" FieldLabel="专科中心类别" Editable="false" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:ComboBox ID="cboJob" runat="server" FieldLabel="带头人职称" Editable="false" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtPersum" runat="server" FieldLabel="主系列人员数" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtJunior" runat="server" FieldLabel="大专" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtSenddoctor" runat="server" FieldLabel="送学博士" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtPlanttype" runat="server" FieldLabel="培养点类别" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtTechindepen" runat="server" FieldLabel="自主创新" />
                                                    </ext:Anchor>
                                                </ext:FormLayout>
                                            </Body>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                    <ext:LayoutColumn ColumnWidth=".2">
                                        <ext:Panel ID="Panel2" runat="server" Border="false" BodyStyle="margin:10px;">
                                            <Body>
                                                <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left">
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtUnittype" runat="server" FieldLabel="单位性质" ReadOnly="true" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtLeadname" runat="server" FieldLabel="带头人姓名" MaxLength="20" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtLeadtecnjob" runat="server" FieldLabel="带头人学术任职" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtDoctor" runat="server" FieldLabel="其中：博士" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtTech" runat="server" FieldLabel="中专" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtSendmaster" runat="server" FieldLabel="送学硕士" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtFostersum" runat="server" FieldLabel="培训人数" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtTechabsorb" runat="server" FieldLabel="消化吸收" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtSpeccode" runat="server" FieldLabel="专科中心代码" ReadOnly="true" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:DateField ID="dtfLeadold" runat="server" FieldLabel="带头人出生日期" CausesValidation="true"
                                                            AllowBlank="false">
                                                        </ext:DateField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtWeavebed" runat="server" FieldLabel="学科编制床" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtMaster" runat="server" FieldLabel="硕士" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtGoabroad" runat="server" FieldLabel="出国培养" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtFetchdoctor" runat="server" FieldLabel="引进博士" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtNewtech" runat="server" FieldLabel="新技术新项目" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtTechelse" runat="server" FieldLabel="其他" />
                                                    </ext:Anchor>
                                                </ext:FormLayout>
                                            </Body>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                    <ext:LayoutColumn ColumnWidth=".6">
                                        <ext:Panel ID="Panel4" runat="server" Border="false">
                                            <Body>
                                                <ext:FormLayout ID="FormLayout3" runat="server" LabelAlign="Left">
                                                </ext:FormLayout>
                                            </Body>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
        <ext:Window ID="ViewDept" runat="server" Icon="Group" Title="专科中心科室" Width="600"
            Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
            Resizable="false">
            <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:ComboBox ID="DeptCodeCombo" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                            Width="120" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                            PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="科室信息"
                            ListWidth="240">
                            <Template ID="Template1" runat="server">
                               <tpl for=".">
                                  <div class="search-item">
                                     <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                  </div>
                               </tpl>
                            </Template>
                        </ext:ComboBox>
                        <ext:Button Text="添加科室" ID="btnAddDept" runat="server" Icon="Add">
                            <AjaxEvents>
                                <Click OnEvent="AddDept">
                                </Click>
                            </AjaxEvents>
                        </ext:Button>
                        <ext:Button Text="删除科室" ID="btnDelDept" runat="server" Icon="Delete" Disabled="true">
                            <Listeners>
                                <Click Handler="DeptOpration();" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <Body>
                <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store" Border="false" AutoWidth="true"
                    Header="false" AutoScroll="true" BodyStyle="background-color:Transparent;" Height="335">
                    <ColumnModel ID="ColumnModel2" runat="server">
                        <Columns>
                            <ext:Column ColumnID="ID" Hidden="true">
                            </ext:Column>
                            <ext:Column ColumnID="DEPT_CODE" Header="科室代码" Sortable="true" DataIndex="DEPT_CODE" />
                            <ext:Column ColumnID="DEPT_NAME" Header="科室名称" Sortable="true" DataIndex="DEPT_NAME" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" runat="server">
                            <Listeners>
                                <RowSelect Handler="#{btnDelDept}.enable();" />
                                <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{btnDelDept}.disable();}" />
                            </Listeners>
                        </ext:CheckboxSelectionModel>
                    </SelectionModel>
                    <LoadMask ShowMask="true" />
                </ext:GridPanel>
            </Body>
        </ext:Window>
    </div>
    </form>
</body>
</html>
