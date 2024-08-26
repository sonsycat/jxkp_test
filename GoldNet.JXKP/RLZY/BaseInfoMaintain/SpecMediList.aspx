<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpecMediList.aspx.cs" Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.SpecMediList" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>

    <script type="text/javascript">        
        /*
            GRIDPANEL操作
            optype :1 添加;  2 重命名; 3 删除;
        */
        var RowIndex;
        function TreeOpration(optype,record) {
            if (optype == "1") {
                var myDate = new Date();
                var year = myDate.getFullYear();   //获取完整的年份(4位,1970-????)
                var month = myDate.getMonth() + 1;       //获取当前月份(0-11,0代表1月)
                var MMmonth  = month;
                if(month < 10) {
                    MMmonth = '0' + month;
                }
                var data = year +''+ MMmonth;
                txtStatMonth.setValue(data);                
                txtName.setValue("");       
                txtAges.setValue("");       
                txtInpNo.setValue("");      
                txtUnit.setValue("");   
                txtDiagDesc.setValue("");   
                txtArmcar.setValue(""); 
                cboSpeMediItem.selectByIndex(0);
                dtfBirth.setValue("");      
                txtETimes.setValue("");     
                cboSex.selectByIndex(0);   
                cboIdentity.selectByIndex(0);   
                txtInsuranceNo.setValue("");
                Btn_BatStart.setText("保存");
                btnSetSave.setVisible(false);
                btnSaveSet.setVisible(false);
                if(PowerInfoHidden.value==2 && hiddenEdit.value == "1") {btnSaveSet.setVisible(true);}
                arcEditWindow.show();
            } else if (optype == "2") {
                HiddenId.setValue(record.data.ID);
                txtStatMonth.setValue(record.data.STAT_MONTH);
                txtName.setValue(record.data.NAME);       
                txtAges.setValue(record.data.AGES);       
                txtInpNo.setValue(record.data.INP_NO);      
                txtUnit.setValue(record.data.UNIT);   
                txtDiagDesc.setValue(record.data.DIAG_DESC);   
                txtArmcar.setValue(record.data.ARMCAR); 
                cboSpeMediItem.setValue(record.data.SPE_MEDI_ITEM);
                dtfBirth.setValue(record.data.BIRTH);      
                txtETimes.setValue(record.data.E_TIMES); 
                cboSex.setValue(record.data.SEX);
                cboIdentity.setValue(record.data.IDENTITY);    
                txtInsuranceNo.setValue(record.data.INSURANCE_NO);
                var Name = PowerInfoHidden.value==1?"审批":"提交";
                if(hiddenEdit.value == '2') {
                    txtName.setDisabled(true);       
                    txtAges.setDisabled(true);       
                    txtInpNo.setDisabled(true);      
                    txtUnit.setDisabled(true);   
                    txtDiagDesc.setDisabled(true);   
                    txtArmcar.setDisabled(true); 
                    cboSpeMediItem.setDisabled(true);
                    dtfBirth.setDisabled(true);      
                    txtETimes.setDisabled(true);     
                    cboSex.setDisabled(true);   
                    cboIdentity.setDisabled(true);   
                    txtInsuranceNo.setDisabled(true);
                    txtStatMonth.setDisabled(true);
                }
                Btn_BatStart.setText(Name);
                btnSaveSet.setVisible(false);
                btnSetSave.setVisible(false);
                if(Name == "提交" && cbxOpration.checked) {
                    btnSetSave.setVisible(true);
                }
                arcEditWindow.show();
            } else if (optype == "3") {
                var selections = CheckboxSelectionModel1.getSelections();
                Btn_BatStart.setText("");
                Ext.Msg.confirm("删除项目", "确定要删除该项目吗？", function(btn, text) { if((btn != "ok") && (btn != "yes")){return;} else {OperEchoCallback(selections,optype);}});
            } else if(optype == "4") {
                optype  = PowerInfoHidden.value==1?"6":"4";
                var selections = CheckboxSelectionModel1.getSelections();
                Btn_BatStart.setText("");
                var Name = PowerInfoHidden.value==1?"审批":"提交";
                Ext.Msg.confirm("批量"+Name+"项目", "确定要批量"+Name+"项目吗？", function(btn, text) { if((btn != "ok") && (btn != "yes")){return;} else {OperEchoCallback(selections,optype);}});
            }
        }
        
        function OperEchoCallback(selections,optype) {
            var id = '';
            for(var i =0;i<selections.length;i++) {
                if(i == selections.length -1) {
                    id = id+selections[i].data.ID;
                } else {
                    id = id+selections[i].data.ID +',';
                }
            }
            GridPanelToDataBase(id,"","","","","","","","","","","","","",optype,"");
        }
        
        
        /*
            节点增删改操作回调函数
        */
        function OpCallback (btn) {
            if (Btn_BatStart.text == "保存") {
                optype = "1";
            }
            if(Btn_BatStart.text == "提交") {
                optype = "2";
            }
            if(Btn_BatStart.text == "审批") {
                optype = "5";
            }
            if((btn != "ok") && (btn != "yes")){
                return;
            }
            var myDate = new Date();
            var statmonth = txtStatMonth.getValue();
            var Name=txtName.getValue();    
            var Ages = txtAges.getValue();            
            var InpNo=txtInpNo.getValue();    
            var Unit=txtUnit.getValue();    
            var DiagDesc=txtDiagDesc.getValue();   
            var Armcar=txtArmcar.getValue(); 
            var SpeMediItem=cboSpeMediItem.getSelectedItem().text;      
            var Sex=cboSex.getSelectedItem().text;   
            var Identity=cboIdentity.getSelectedItem().text;   
            var InsuranceNo=txtInsuranceNo.getValue();
            var BirthFormat = dtfBirth.getValue() == ''?myDate:dtfBirth.getValue();
            var Birth = BirthFormat.format('Y-m-d');                
            var ETimesFormat = txtETimes.getValue() == ''?myDate:txtETimes.getValue();
            var ETimes = ETimesFormat.format('Y-m-d');         
            var deptCode = DeptCodeCombo.getSelectedItem().value;
            
            if (optype == "1") {
               GridPanelToDataBase("",statmonth,Name,Ages,InpNo,Unit,DiagDesc,Armcar,SpeMediItem,Birth,ETimes,Sex,Identity,InsuranceNo,optype,deptCode);
            } else if (optype == "2") {
               GridPanelToDataBase(HiddenId.value,statmonth,Name,Ages,InpNo,Unit,DiagDesc,Armcar,SpeMediItem,Birth,ETimes,Sex,Identity,InsuranceNo,optype,Store1.getAt(RowIndex).data.DEPT_CODE);
            } else if(optype == "5") {
                GridPanelToDataBase(HiddenId.value,statmonth,Name,Ages,InpNo,Unit,DiagDesc,Armcar,SpeMediItem,Birth,ETimes,Sex,Identity,InsuranceNo,optype,Store1.getAt(RowIndex).data.DEPT_CODE);
            }
        }
        
        function GridPanelToDataBase(id,statMonth,name,ages,inpNo,unit,diagDesc,armcar,speMediItem,birth,eTimes,sex,identity,insuranceNo,optype,deptCode) {
          Goldnet.AjaxMethod.request(
                  'SpecMediListAjaxOper',
                    {
                        params: {
                           Id:id,StatMonth:statMonth,Name:name,Ages:ages,InpNo:inpNo,Unit:unit,
                           DiagDesc:diagDesc,Armcar:armcar,SpeMediItem:speMediItem,Birth:birth,ETimes:eTimes,
                           Sex:sex,Identity:identity,InsuranceNo:insuranceNo,OperType:optype,DeptCode:deptCode
                        },
                        success: function(result) {
                            Store1.reload();
                            btn_EchoHandle.setDisabled(true);
                            btn_Delete.setDisabled(true);
                            arcEditWindow.hide();
                        },
                        failure: function(msg) {
                            GridPanel_Show.el.unmask();
                        }
                    });
        }
        
        function SetSaveMethod(type) {
            var myDate = new Date();
            var statmonth = txtStatMonth.getValue();
            var Name=txtName.getValue();    
            var Ages = txtAges.getValue();            
            var InpNo=txtInpNo.getValue();    
            var Unit=txtUnit.getValue();    
            var DiagDesc=txtDiagDesc.getValue();   
            var Armcar=txtArmcar.getValue(); 
            var SpeMediItem=cboSpeMediItem.getSelectedItem().text;      
            var Sex=cboSex.getSelectedItem().text;   
            var Identity=cboIdentity.getSelectedItem().text;   
            var InsuranceNo=txtInsuranceNo.getValue();
            var BirthFormat = dtfBirth.getValue() == ''?myDate:dtfBirth.getValue();
            var Birth = BirthFormat.format('Y-m-d');                
            var ETimesFormat = txtETimes.getValue() == ''?myDate:txtETimes.getValue();
            var ETimes = ETimesFormat.format('Y-m-d'); 
            var deptCode =  '';
            if(type == '8') {
                deptCode = DeptCodeCombo.getSelectedItem().value;
            } else {
                deptCode = Store1.getAt(RowIndex).data.DEPT_CODE;
            }
            GridPanelToDataBase(HiddenId.value,statmonth,Name,Ages,InpNo,Unit,DiagDesc,Armcar,SpeMediItem,Birth,ETimes,Sex,Identity,InsuranceNo,type,deptCode);
        }
        
        
        var prepare = function(grid, toolbar, rowIndex, record) {
            var menuButton = toolbar.items.get(0);
            var menu1 = menuButton.menu.items.get(0);
            //if(cbxOpration.checked) {menu1.setDisabled(true);}
            menu1.setDisabled(!hiddenMeunUp.getValue());
        }
        
        var gridCommand = function(command, record) {
           TreeOpration(2,record)
        }
    </script>

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
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Hidden ID="PowerInfoHidden" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="HiddenId" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="hiddenMeunUp" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="hiddenEdit" runat="server">
    </ext:Hidden>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="STAT_MONTH" />
                    <ext:RecordField Name="SPE_MEDI_ITEM" />
                    <ext:RecordField Name="INP_NO" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="SEX" />
                    <ext:RecordField Name="BIRTH" />
                    <ext:RecordField Name="UNIT" />
                    <ext:RecordField Name="IDENTITY" />
                    <ext:RecordField Name="DIAG_DESC" />
                    <ext:RecordField Name="INSURANCE_NO" />
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="ENTER_TIME" />
                    <ext:RecordField Name="ENTER_PERS" />
                    <ext:RecordField Name="ADD_MARK" />
                    <ext:RecordField Name="AGES" />
                    <ext:RecordField Name="E_TIMES" />
                    <ext:RecordField Name="ARMCAR" />
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
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <North>
                        <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0">
                            <Items>
                                <ext:Label ID="Label2" runat="server" Text="年度：">
                                </ext:Label>
                                <ext:ComboBox ID="TimeOrgan" runat="server" Width="40">
                                    <Items>
                                        <ext:ListItem Text="&lt;=" Value="&lt;=" />
                                        <ext:ListItem Text="&gt;=" Value="&gt;=" />
                                        <ext:ListItem Text="=" Value="=" />
                                    </Items>
                                </ext:ComboBox>
                                <ext:ComboBox ID="cboTime" runat="server" Width="60">
                                </ext:ComboBox>
                                <ext:Label ID="Label3" runat="server" Text="科室：">
                                </ext:Label>
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
                                <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                    <Listeners>
                                        <Click Handler="#{Store1}.reload();#{btn_Delete}.disable();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator>
                                </ext:ToolbarSeparator>
                                <ext:Button ID="btn_Add" runat="server" Text="添加信息" Icon="Add">
                                    <Listeners>
                                        <Click Handler="if(#{DeptCodeCombo}.getSelectedItem().value == '') {Ext.Msg.show({ title: '信息提示', msg: '请选择科室', icon: 'ext-mb-info', buttons: { ok: true }  });} else {TreeOpration(1,'')}" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="删除信息" ID="btn_Delete" runat="server" Icon="Delete" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(3)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="批量处理信息" ID="btn_EchoHandle" runat="server" Icon="Wrench" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(4)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Checkbox ID="cbxOpration" runat="server" BoxLabel="显示">
                                    <Listeners>
                                        <Check Handler="#{Store1}.reload();#{btn_EchoHandle}.disable();#{btn_Delete}.disable();" />
                                    </Listeners>
                                </ext:Checkbox>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:Panel ID="Panel1" runat="server">
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Border="false"
                                                AutoWidth="true" Header="false" AutoScroll="true">
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="STAT_MONTH" Header="统计年月" Sortable="true" DataIndex="STAT_MONTH">
                                                        </ext:Column>
                                                        <ext:Column ColumnID="DEPT_NAME" Header="所在科室" Sortable="true" DataIndex="DEPT_NAME" />
                                                        <ext:Column ColumnID="SPE_MEDI_ITEM" Header="诊疗项目" Sortable="true" DataIndex="SPE_MEDI_ITEM"
                                                            Width="75" />
                                                        <ext:Column ColumnID="INP_NO" Header="病案号" Sortable="true" DataIndex="INP_NO" />
                                                        <ext:Column ColumnID="NAME" Header="患者姓名" Sortable="true" DataIndex="NAME" />
                                                        <ext:Column ColumnID="IDENTITY" Header="身份" Sortable="true" DataIndex="IDENTITY" />
                                                        <ext:Column ColumnID="INSURANCE_NO" Header="患者医改帐号" Sortable="true" DataIndex="INSURANCE_NO" />
                                                        <ext:CommandColumn Width="38" Header="操作">
                                                            <Commands>
                                                                <ext:SplitCommand Icon="TableMultiple">
                                                                    <ToolTip Text="单项操作" />
                                                                    <Menu>
                                                                        <Items>
                                                                            <ext:MenuCommand CommandName="CmdBJGW" Icon="Wrench" Text="单项处理信息">
                                                                            </ext:MenuCommand>
                                                                        </Items>
                                                                    </Menu>
                                                                </ext:SplitCommand>
                                                            </Commands>
                                                            <PrepareToolbar Fn="prepare" />
                                                        </ext:CommandColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <Listeners>
                                                    <Command Handler=" gridCommand(command,record);" />
                                                </Listeners>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server">
                                                        <Listeners>
                                                            <RowSelect Handler="#{btn_Delete}.enable();#{btn_EchoHandle}.enable();RowIndex = rowIndex" />
                                                            <RowDeselect Handler="if (!#{GridPanel_Show}.hasSelection()) {#{btn_Delete}.disable();#{btn_EchoHandle}.disable();}" />
                                                        </Listeners>
                                                    </ext:CheckboxSelectionModel>
                                                </SelectionModel>
                                                <LoadMask ShowMask="true" />
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="特殊诊疗信息" Width="600"
        Height="270" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="TextField1" runat="server" FieldLabel="序号" Visible="false" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtStatMonth" runat="server" FieldLabel="统计年月" ReadOnly="true" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtName" runat="server" FieldLabel="患者姓名" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtAges" runat="server" FieldLabel="年龄" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtInpNo" runat="server" FieldLabel="病案号" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtUnit" runat="server" FieldLabel="所在部别" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtDiagDesc" runat="server" FieldLabel="疾病诊断" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtArmcar" runat="server" FieldLabel="证件名称号码" />
                                </ext:Anchor>
                            </ext:FormLayout>
                        </Body>
                    </ext:Panel>
                </ext:LayoutColumn>
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel3" runat="server" Border="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboSpeMediItem" runat="server" FieldLabel="特殊诊疗项目" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:DateField ID="dtfBirth" runat="server" FieldLabel="出生年月" Format="yyyy-MM-dd" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:DateField ID="txtETimes" runat="server" FieldLabel="实施时间" Format="yyyy-MM-dd" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboSex" runat="server" FieldLabel="性别">
                                        <Items>
                                            <ext:ListItem Text="男" Value="男" />
                                            <ext:ListItem Text="女" Value="女" />
                                        </Items>
                                    </ext:ComboBox>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboIdentity" runat="server" FieldLabel="身份" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtInsuranceNo" runat="server" FieldLabel="医改账号" />
                                </ext:Anchor>
                            </ext:FormLayout>
                        </Body>
                    </ext:Panel>
                </ext:LayoutColumn>
            </ext:ColumnLayout>
        </Body>
        <BottomBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="Disk" Text="保存">
                        <Listeners>
                            <Click Handler="OpCallback('ok');" />
                        </Listeners>
                    </ext:ToolbarButton>
                    
                    <%--添加界面直接提交按钮 --%>
                    <ext:ToolbarButton ID="btnSaveSet" runat="server" Icon="Disk" Text="提交">
                        <Listeners>
                            <Click Handler="SetSaveMethod('8');" />
                        </Listeners>
                    </ext:ToolbarButton>
                    
                    <ext:ToolbarButton ID="btnSetSave" runat="server" Icon="Disk" Text="修改">
                        <Listeners>
                            <Click Handler="SetSaveMethod('7');" />
                        </Listeners>
                    </ext:ToolbarButton>
                    
                    <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="退出">
                        <Listeners>
                            <Click Handler="arcEditWindow.hide();" />
                        </Listeners>
                    </ext:ToolbarButton>
                </Items>
            </ext:Toolbar>
        </BottomBar>
    </ext:Window>
    </form>
</body>
</html>
