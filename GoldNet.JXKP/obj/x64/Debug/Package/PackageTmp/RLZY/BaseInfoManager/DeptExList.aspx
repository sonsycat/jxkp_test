<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptExList.aspx.cs" Inherits="GoldNet.JXKP.RLZY.BaseInfoManager.DeptExList" %>

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
                var day = myDate.getDate();
                var MMmonth  = month;
                var DDday = day;
                if(month < 10) {
                    MMmonth = '0' + month;
                }
                 if(day < 10) {
                    DDday = '0' + day;
                }
                var data = year +''+ MMmonth +''+ DDday;
                txtExTypes.setValue("");
                txtExSorts.setValue("");
                txtExPerson.setValue("");
                txtExCount.setValue("");
                txtExDates.setValue(data);
                txtExTimes.setValue("");
                txtExMemert.setValue("");
                Btn_BatStart.setText("保存");

                txtExSug.setValue("");
                txtExSetSug.setValue("");

                txtExTypes.setDisabled(false);  
                txtExSorts.setDisabled(false);  
                txtExPerson.setDisabled(false);  
                txtExCount.setDisabled(false);  
                txtExDates.setDisabled(false);
                txtExTimes.setDisabled(false);
                txtExMemert.setDisabled(false);

                txtExSug.setVisible(true);
                txtExSug.setDisabled(true);
                txtExSetSug.setVisible(true);
                txtExSetSug.setDisabled(true);

                Btn_BatStart.setVisible(true); //保存可见
                btnSetSave.setVisible(false); //修改不可见
                btnSaveSet.setVisible(false); //提交不可见
                btnNotApprove.setVisible(false); //审批不通过不可见

                arcEditWindow.show();
            } else if (optype == "2") {
                //初始化
                HiddenId.setValue(record.data.ID);
                txtExTypes.setValue(record.data.EXTYPES);
                txtExSorts.setValue(record.data.EXSORTS);
                txtExPerson.setValue(record.data.EXPERSON);
                txtExCount.setValue(record.data.EXCOUNT);
                txtExDates.setValue(record.data.EXDATES);
                txtExTimes.setValue(record.data.EXTIMES);
                txtExMemert.setValue(record.data.EXMEMERT);
                ADD_MARK.setValue(record.data.ADD_MARK);

                txtExSug.setValue(record.data.MARK_SUG);
                txtExSetSug.setValue(record.data.SETUP_SUG);

                var Name = PowerInfoHidden.value == 1 ? "审批" : "提交";
                Btn_BatStart.setText(Name);                

                //院级审批权限  liu.shh 2012.12.19
                if (PowerInfoHidden.value == 1) {
                    txtExSetSug.setVisible(true);
                    txtExSetSug.setDisabled(false);
                    txtExSetSug.setVisible(true);
                    txtExSetSug.setDisabled(false);

                    txtExTypes.setDisabled(false);
                    txtExSorts.setDisabled(false);
                    txtExPerson.setDisabled(false);
                    txtExCount.setDisabled(false);
                    txtExDates.setDisabled(false);
                    txtExTimes.setDisabled(false);
                    txtExMemert.setDisabled(false);

                    if (Name == "审批") {
                        Btn_BatStart.setVisible(true); //审批可见
                    }
                    else {
                        Btn_BatStart.setVisible(false); //其它不可见
                    }
                    btnSetSave.setVisible(false); //修改不可见
                    btnSaveSet.setVisible(false); //提交不可见
                    btnNotApprove.setVisible(true); //审批不通过可见
                }
                
                //科主任权限  liu.shh  2012.12.19
                if (PowerInfoHidden.value == 2 && hiddenEdit.value == '1') {
                    txtExSug.setVisible(true);
                    txtExSug.setDisabled(true);
                    txtExSetSug.setVisible(true);
                    txtExSetSug.setDisabled(false);

                    if (!cbxOpration.checked) {
                        if (record.data.ADD_MARK == "审批通过") {  //审批已通过的不可再修改
                            txtExTypes.setDisabled(true);
                            txtExSorts.setDisabled(true);
                            txtExPerson.setDisabled(true);
                            txtExCount.setDisabled(true);
                            txtExDates.setDisabled(true);
                            txtExTimes.setDisabled(true);
                            txtExMemert.setDisabled(true);

                            txtExSug.setDisabled(true);
                            txtExSetSug.setDisabled(true);

                            if (Name == "提交") {
                                Btn_BatStart.setVisible(false);
                            }
                            btnSetSave.setVisible(false);
                            btnSaveSet.setVisible(false);
                            btnNotApprove.setVisible(false);
                        }
                        else {
                            txtExTypes.setDisabled(false);
                            txtExSorts.setDisabled(false);
                            txtExPerson.setDisabled(false);
                            txtExCount.setDisabled(false);
                            txtExDates.setDisabled(false);
                            txtExTimes.setDisabled(false);
                            txtExMemert.setDisabled(false);

                            if (Name == "提交") {
                                Btn_BatStart.setVisible(true);
                            }
                            btnSetSave.setVisible(true);
                            btnSaveSet.setVisible(false);
                            btnNotApprove.setVisible(false);
                        }
                    }
                    if (cbxOpration.checked) {
                        txtExTypes.setDisabled(false);
                        txtExSorts.setDisabled(false);
                        txtExPerson.setDisabled(false);
                        txtExCount.setDisabled(false);
                        txtExDates.setDisabled(false);
                        txtExTimes.setDisabled(false);
                        txtExMemert.setDisabled(false);

                        if (Name == "提交") {
                            Btn_BatStart.setVisible(true);
                        }
                        btnSetSave.setVisible(true);
                        btnSaveSet.setVisible(false);
                        btnNotApprove.setVisible(false);
                    }
                }

                //普通医生权限  liu.shh  2012.12.19
                if (PowerInfoHidden.value == 0) {
                    txtExSug.setVisible(true);
                    txtExSug.setDisabled(true);
                    txtExSetSug.setVisible(true);
                    txtExSetSug.setDisabled(true);

                    if(cbxOpration.checked){
                        txtExTypes.setDisabled(false);  
                        txtExSorts.setDisabled(false);  
                        txtExPerson.setDisabled(false);  
                        txtExCount.setDisabled(false);  
                        txtExDates.setDisabled(false);
                        txtExTimes.setDisabled(false);
                        txtExMemert.setDisabled(false);

                        btnSetSave.setVisible(true);
                        btnSaveSet.setVisible(false);
                        btnNotApprove.setVisible(false);
                    }
                    if (!cbxOpration.checked) {
                        if (record.data.ADD_MARK == "审批通过") {  //审批已通过的不可再修改
                            txtExTypes.setDisabled(true);
                            txtExSorts.setDisabled(true);
                            txtExPerson.setDisabled(true);
                            txtExCount.setDisabled(true);
                            txtExDates.setDisabled(true);
                            txtExTimes.setDisabled(true);
                            txtExMemert.setDisabled(true);

                            btnSetSave.setVisible(false);
                            btnSaveSet.setVisible(false);
                            btnNotApprove.setVisible(false);
                        }
                        else { //审批不通过的可以再修改
                            txtExTypes.setDisabled(false);
                            txtExSorts.setDisabled(false);
                            txtExPerson.setDisabled(false);
                            txtExCount.setDisabled(false);
                            txtExDates.setDisabled(false);
                            txtExTimes.setDisabled(false);
                            txtExMemert.setDisabled(false);

                            btnSetSave.setVisible(true);
                            btnSaveSet.setVisible(false);
                            btnNotApprove.setVisible(false);
                        }
                    }
                    if(Name == "提交"){
                        Btn_BatStart.setVisible(false);
                    }
                }
                arcEditWindow.show();
            } else if (optype == "3") {
                if (!cbxOpration.checked && PowerInfoHidden.value != 1) {
                    Ext.MessageBox.alert("提示","项目已审批通过，不可再删除！");
                    return;
                }
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
            GridPanelToDataBase(id,"","","","","","","",optype,"","","","");
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
            var ExDatesFormat = txtExDates.getValue() == ''?myDate:txtExDates.getValue();
            var ExDates = ExDatesFormat.format('Ymd');
            var ExTypes=txtExTypes.getValue();
            var ExSorts=txtExSorts.getValue();
            var ExPerson=txtExPerson.getValue();
            var ExCount=txtExCount.getValue();
            var ExTimes=txtExTimes.getValue();
            var ExMemert = txtExMemert.getValue();
            var ExSug = txtExSug.getValue();
            var ExSetSug = txtExSetSug.getValue();
            var deptCode = DeptCodeCombo.getSelectedItem().value;
            var deptName = '';
            if (optype == "1") {
                GridPanelToDataBase("", ExTypes, ExSorts, ExPerson, ExCount, ExDates, ExTimes, ExMemert, optype, deptCode, deptName, ExSug, ExSetSug);
            } else if (optype == "2") {
                GridPanelToDataBase(HiddenId.value, ExTypes, ExSorts, ExPerson, ExCount, ExDates, ExTimes, ExMemert, optype, Store1.getAt(RowIndex).data.DEPT_CODE, Store1.getAt(RowIndex).data.DEPT_NAME, ExSug, ExSetSug);
           } else if (optype == "5") {
                GridPanelToDataBase(HiddenId.value, ExTypes, ExSorts, ExPerson, ExCount, ExDates, ExTimes, ExMemert, optype, Store1.getAt(RowIndex).data.DEPT_CODE, Store1.getAt(RowIndex).data.DEPT_NAME, ExSug, ExSetSug);
            }
        }

        function GridPanelToDataBase(id, ExTypes, ExSorts, ExPerson, ExCount, ExDates, ExTimes, ExMemert, optype, deptCode, deptName, ExSug, ExSetSug) {
          Goldnet.AjaxMethod.request(
                  'DeptExListAjaxOper',
                    {
                        params: {
                            Id: id, ExTypes: ExTypes, ExSorts: ExSorts, ExPerson: ExPerson, ExCount: ExCount, ExDates: ExDates, ExTimes: ExTimes, ExMemert: ExMemert, OperType: optype, DeptCode: deptCode, DeptName: deptName, ExSug: ExSug, ExSetSug: ExSetSug
                        },
                        success: function(result) {
                            Store1.reload();
                            btn_EchoHandle.setDisabled(true);
                            btn_Delete.setDisabled(true);
                            arcEditWindow.hide();
                        },
                        failure: function(msg) {
                            alert(msg);
                            GridPanel_Show.el.unmask();
                        }
                    });
        }
        
        
        function SetSaveMethod(type) {
            var myDate = new Date();                
            var ExDatesFormat = txtExDates.getValue() == ''?myDate:txtExDates.getValue();
            var ExDates = ExDatesFormat.format('Ymd');
            var ExTypes=txtExTypes.getValue();
            var ExSorts=txtExSorts.getValue();
            var ExPerson=txtExPerson.getValue();
            var ExCount=txtExCount.getValue();
            var ExTimes=txtExTimes.getValue();
            var ExMemert=txtExMemert.getValue();
            var ExSug = txtExSug.getValue();
            var ExSetSug = txtExSetSug.getValue();
            var deptCode =  '';
            var deptName = '';
            if(type == '8') {
                deptCode = DeptCodeCombo.getSelectedItem().value;
            } else {
                deptCode = Store1.getAt(RowIndex).data.DEPT_CODE;
                deptName = Store1.getAt(RowIndex).data.DEPT_NAME;
            }
            GridPanelToDataBase(HiddenId.value,ExTypes,ExSorts,ExPerson,ExCount,ExDates,ExTimes,ExMemert,type,deptCode,deptName,ExSug,ExSetSug);
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
    <ext:Hidden ID="hiddenMeunUp" runat="server"></ext:Hidden>
    <ext:Hidden ID="hiddenEdit" runat="server"></ext:Hidden>
    <ext:Hidden ID="ADD_MARK" runat="server"></ext:Hidden>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="EXDATES" />
                    <ext:RecordField Name="EXTYPES" />
                    <ext:RecordField Name="EXPERSON" />
                    <ext:RecordField Name="EXSORTS" />
                    <ext:RecordField Name="EXTIMES" />
                    <ext:RecordField Name="EXMEMERT" />
                    <ext:RecordField Name="EXCOUNT" />
                    <ext:RecordField Name="MARK_SUG"/>
                    <ext:RecordField Name="SETUP_SUG"/>
                    <ext:RecordField Name="ADD_MARK"/>
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
                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="10" />
                                <ext:Label ID="Label7" runat="server" Text="统计月份">
                                </ext:Label>
                                <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                <ext:ComboBox runat="server" ID="Comb_StartYear" Width="60" ListWidth="60" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                <ext:ComboBox runat="server" ID="Comb_StartMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月" />
                                <ext:ToolbarTextItem ID="ToolbarTextItem7" runat="server" Text="   至   " />
                                <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                <ext:ComboBox runat="server" ID="Comb_EndYear" Width="60" ListWidth="60" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="年" />
                                <ext:ComboBox runat="server" ID="Comb_EndMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem5" runat="server" Text="月" />
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
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
                                <ext:Button ID="btn_Add" runat="server" Text="添加训练" Icon="Add">
                                    <Listeners>
                                        <Click Handler="if(#{DeptCodeCombo}.getSelectedItem().value == '') {Ext.Msg.show({ title: '信息提示', msg: '请选择科室', icon: 'ext-mb-info', buttons: { ok: true }  });} else {TreeOpration(1,'')}" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="删除训练" ID="btn_Delete" runat="server" Icon="Delete" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(3)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="批量处理" ID="btn_EchoHandle" runat="server" Icon="Wrench" Disabled="true">
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
                                                        <ext:Column ColumnID="EXDATES" Header="时间" Sortable="true" DataIndex="EXDATES" />
                                                        <ext:Column ColumnID="DEPT_NAME" Header="科室" Sortable="true" DataIndex="DEPT_NAME" />
                                                        <ext:Column ColumnID="EXTYPES" Header="方式" Sortable="true" DataIndex="EXTYPES" />
                                                        <ext:Column ColumnID="EXPERSON" Header="主讲人" Sortable="true" DataIndex="EXPERSON" />
                                                        <ext:Column ColumnID="EXSORTS" Header="科目" Sortable="true" DataIndex="EXSORTS" />
                                                        <ext:Column ColumnID="EXTIMES" Header="时数" Sortable="true" DataIndex="EXTIMES" />
                                                        <ext:Column ColumnID="EXMEMERT" Header="参加人数" Sortable="true" DataIndex="EXMEMERT" />
                                                        <ext:Column ColumnID="ADD_MARK" Header="审批标识" Sortable="true" DataIndex="ADD_MARK" />
                                                        <ext:Column ColumnID="SETUP_SUG" Header="提交意见" Sortable="true" DataIndex="SETUP_SUG" />
                                                        <ext:Column ColumnID="MARK_SUG" Header="审批意见" Sortable="true" DataIndex="MARK_SUG" />   
                                                        <ext:CommandColumn Width="38" Header="操作">
                                                            <Commands>
                                                                <ext:SplitCommand Icon="TableMultiple">
                                                                    <ToolTip Text="单项操作" />
                                                                    <Menu>
                                                                        <Items>
                                                                            <ext:MenuCommand CommandName="CmdBJGW" Icon="Wrench" Text="单项处理训练">
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
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="科内训练" Width="600"
        Height="300" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtExTypes" runat="server" FieldLabel="训练方式" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtExSorts" runat="server" FieldLabel="科目" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtExPerson" runat="server" FieldLabel="主讲人" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txtExCount" runat="server" FieldLabel="内容简介" Height="140" />
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
                                    <ext:DateField ID="txtExDates" runat="server" FieldLabel="时间" Format="yyyyMMdd"/>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtExTimes" runat="server" FieldLabel="时数" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtExMemert" runat="server" FieldLabel="参加人数" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txtExSetSug" runat="server" FieldLabel="提交意见" Height="65" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txtExSug" runat="server" FieldLabel="审批意见" Height="70" />
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
                    <%--加上审批不通过按钮 --%>
                    <ext:ToolbarButton ID="btnNotApprove" runat="server" Icon="Disk" Text="审批不通过">
                        <Listeners>
                            <Click Handler="SetSaveMethod('-1');" />
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
