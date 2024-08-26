<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeciMeetList.aspx.cs" Inherits="GoldNet.JXKP.RLZY.BaseInfoManager.TeciMeetList" %>

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
                var data = year +'-'+ MMmonth +'-'+ DDday;
                
                dtfMeetDate.setValue(data);   // 日期
                txtMeetName.setValue("");   // 会议名称
                cboGrade.selectByIndex(0);    // 等级
                txtScienceMeetingPlace.setValue("");   // 地点
                txrContent.setValue("");   // 摘要

                txrSug.setValue("");
                txrSetSug.setValue("");
                
                dtfMeetDate.setDisabled(false);// 日期
                txtMeetName.setDisabled(false);     // 会议名称
                cboGrade.setDisabled(false);  // 等级
                txtScienceMeetingPlace.setDisabled(false);     // 地点
                txrContent.setDisabled(false);     // 摘要

                txrSug.setVisible(true);
                txrSug.setDisabled(true);
                txrSetSug.setVisible(true);
                txrSetSug.setDisabled(true);

                Btn_BatStart.setVisible(true);
                btnApprove.setVisible(false);
                btnNotApprove.setVisible(false);
                btnSetUp.setVisible(false);
                btnSetSave.setVisible(false);
                btnSaveSet.setVisible(false);

                arcEditWindow.show();
            } else if (optype == "2") {
                //初始化
                HiddenId.setValue(record.data.ID);
                dtfMeetDate.setValue(record.data.MEET_DATE);   // 日期
                txtMeetName.setValue(record.data.MEET_NAME);   // 会议名称
                cboGrade.setValue(record.data.GRADE);   // 等级
                txtScienceMeetingPlace.setValue(record.data.SCIENCE_MEETING_PLACE);   // 地点
                txrContent.setValue(record.data.CONTENT);   // 摘要

                txrSug.setValue(record.data.MARK_SUG);
                txrSetSug.setValue(record.data.SETUP_SUG);

                Btn_BatStart.setVisible(false);
                btnApprove.setVisible(false);
                btnSetUp.setVisible(false);

                //院级审批权限  liu.shh 2012.12.19
                if (PowerInfoHidden.value == 1) {
                    txrSug.setVisible(true);
                    txrSug.setDisabled(false);
                    txrSetSug.setVisible(true);
                    txrSetSug.setDisabled(false);

                    dtfMeetDate.setDisabled(false); // 日期
                    txtMeetName.setDisabled(false);     // 会议名称
                    cboGrade.setDisabled(false);  // 等级
                    txtScienceMeetingPlace.setDisabled(false);     // 地点
                    txrContent.setDisabled(false);     // 摘要

                    Btn_BatStart.setVisible(false); //保存按钮不可见
                    btnSetUp.setVisible(false);   //纯提交，不可见
                    btnApprove.setVisible(true); //审批按钮可见
                    btnNotApprove.setVisible(true); //审批不通过按钮可见
                    btnSetSave.setVisible(false); //修改按钮不可见
                    btnSaveSet.setVisible(false); //保存时提交，不可见
                }
                //科主任权限  liu.shh  2012.12.19
                if (PowerInfoHidden.value == 2 && hiddenEdit.value == '1') {
                    txrSug.setVisible(true);
                    txrSug.setDisabled(true);
                    txrSetSug.setVisible(true);
                    txrSetSug.setDisabled(false);

                    if (!cbxOpration.checked) {
                        if (record.data.ADD_MARK == "审批通过") {  //审批已通过的不可再修改
                            dtfMeetDate.setDisabled(true); // 日期
                            txtMeetName.setDisabled(true);     // 会议名称
                            cboGrade.setDisabled(true);  // 等级
                            txtScienceMeetingPlace.setDisabled(true);     // 地点
                            txrContent.setDisabled(true);     // 摘要

                            txrSug.setDisabled(true);
                            txrSetSug.setDisabled(true);

                            Btn_BatStart.setVisible(false); //保存按钮不可见
                            btnSetUp.setVisible(false);   //纯提交，不可见
                            btnApprove.setVisible(false); //审批按钮不可见
                            btnNotApprove.setVisible(false); //审批不通过按钮不可见
                            btnSetSave.setVisible(false); //修改按钮不可见
                            btnSaveSet.setVisible(false); //保存时提交，不可见
                        }
                        else {
                            dtfMeetDate.setDisabled(false); // 日期
                            txtMeetName.setDisabled(false);     // 会议名称
                            cboGrade.setDisabled(false);  // 等级
                            txtScienceMeetingPlace.setDisabled(false);     // 地点
                            txrContent.setDisabled(false);     // 摘要

                            Btn_BatStart.setVisible(false); //保存按钮不可见
                            btnSetUp.setVisible(true);   //纯提交，可见
                            btnApprove.setVisible(false); //审批按钮不可见
                            btnNotApprove.setVisible(false); //审批不通过按钮不可见
                            btnSetSave.setVisible(true); //修改按钮可见
                            btnSaveSet.setVisible(false); //保存时提交，不可见
                        }
                    }
                    if (cbxOpration.checked) {
                        dtfMeetDate.setDisabled(false); // 日期
                        txtMeetName.setDisabled(false);     // 会议名称
                        cboGrade.setDisabled(false);  // 等级
                        txtScienceMeetingPlace.setDisabled(false);     // 地点
                        txrContent.setDisabled(false);     // 摘要

                        Btn_BatStart.setVisible(false); //保存按钮不可见
                        btnSetUp.setVisible(true);   //纯提交，可见
                        btnApprove.setVisible(false); //审批按钮不可见
                        btnNotApprove.setVisible(false); //审批不通过按钮不可见
                        btnSetSave.setVisible(true); //修改按钮可见
                        btnSaveSet.setVisible(false); //保存时提交，不可见
                    }
                }
                //普通医生权限  liu.shh  2012.12.19
                if (PowerInfoHidden.value == 0) {
                    txrSug.setVisible(true);
                    txrSug.setDisabled(true);
                    txrSetSug.setVisible(true);
                    txrSetSug.setDisabled(true);

                    if (cbxOpration.checked) {
                        dtfMeetDate.setDisabled(false); // 日期
                        txtMeetName.setDisabled(false);     // 会议名称
                        cboGrade.setDisabled(false);  // 等级
                        txtScienceMeetingPlace.setDisabled(false);     // 地点
                        txrContent.setDisabled(false);     // 摘要

                        Btn_BatStart.setVisible(false); //保存按钮不可见
                        btnSetUp.setVisible(false);   //纯提交，不可见
                        btnApprove.setVisible(false); //审批按钮不可见
                        btnNotApprove.setVisible(false); //审批不通过按钮不可见
                        btnSetSave.setVisible(true); //修改按钮可见
                        btnSaveSet.setVisible(false); //保存时提交，不可见
                    }
                    if (!cbxOpration.checked) {
                        if (record.data.ADD_MARK == "审批通过") {  //审批已通过的不可再修改
                            dtfMeetDate.setDisabled(true); // 日期
                            txtMeetName.setDisabled(true);     // 会议名称
                            cboGrade.setDisabled(true);  // 等级
                            txtScienceMeetingPlace.setDisabled(true);     // 地点
                            txrContent.setDisabled(true);     // 摘要

                            Btn_BatStart.setVisible(false); //保存按钮不可见
                            btnSetUp.setVisible(false);   //纯提交，不可见
                            btnApprove.setVisible(false); //审批按钮不可见
                            btnNotApprove.setVisible(false); //审批不通过按钮不可见
                            btnSetSave.setVisible(false); //修改按钮不可见
                            btnSaveSet.setVisible(false); //保存时提交，不可见
                        }
                        else { //审批不通过的可以再修改
                            dtfMeetDate.setDisabled(false); // 日期
                            txtMeetName.setDisabled(false);     // 会议名称
                            cboGrade.setDisabled(false);  // 等级
                            txtScienceMeetingPlace.setDisabled(false);     // 地点
                            txrContent.setDisabled(false);     // 摘要

                            Btn_BatStart.setVisible(false); //保存按钮不可见
                            btnSetUp.setVisible(false);   //纯提交，不可见
                            btnApprove.setVisible(false); //审批按钮不可见
                            btnNotApprove.setVisible(false); //审批不通过按钮不可见
                            btnSetSave.setVisible(true); //修改按钮可见
                            btnSaveSet.setVisible(false); //保存时提交，不可见
                        }
                    }
                }
                arcEditWindow.show();
            } else if (optype == "3") {
                if(!cbxOpration.checked && PowerInfoHidden.value != 1){
                    Ext.MessageBox.alert("提示","项目已审批，不可再删除！");
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
            GridPanelToDataBase(id,optype);
        }
        
      
        
        function GridPanelToDataBase(id,optype) {
          Goldnet.AjaxMethod.request(
                  'FruitListAjaxOper',
                    {
                        params: {
                           Id:id,optype:optype
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
        
        var prepare = function(grid, toolbar, rowIndex, record) {
            var menuButton = toolbar.items.get(0);
            var menu1 = menuButton.menu.items.get(0);
            menu1.setDisabled(!hiddenMeunUp.getValue());
            var menu2 = menuButton.menu.items.get(1);
            menu2.setDisabled(!hiddenAuthor.getValue());
        }
        
        var gridCommand = function(command, record) {
           if(command == 'CmdEditAuthor') {
              HiddenId.setValue(record.data.ID);
              ViewAuthor.show();
              Store.reload();
           } else if(command == 'CmdAddAuthor') {
              HiddenId.setValue(record.data.ID);
              HiddenName.setValue(record.data.MEET_NAME);
              cboStaffInfo.setValue("");
              txaRemarks.setValue("");
              arcEditAuthor.show();
           } else if(command == 'CmdBJGW') {
              TreeOpration(2,record);
           }
        }
        
        var AuthorOpration = function(optype) {
            if(optype == '1') {
                var staffid = cboStaffInfo.getSelectedItem().value;
                var name = cboStaffInfo.getSelectedItem().text;
                var Remarks = txaRemarks.getValue();
                var code = HiddenId.getValue();
                var MeetName = HiddenName.getValue();
                AuthorDataBaseOper(code,staffid,name,Remarks,optype,MeetName);
            } else if(optype == '2') {
                var selections = CheckboxSelectionModel2.getSelections();
                var id = '';
                for(var i =0;i<selections.length;i++) {
                    if(i == selections.length -1) {
                        id = id+selections[i].data.ID;
                    } else {
                        id = id+selections[i].data.ID +',';
                    }
                }
                AuthorDataBaseOper(id,"","","",optype,"");
            }
        }
        
        var AuthorDataBaseOper = function(OperCode,staffid,name,Remarks,optype,MeetName) {
            Goldnet.AjaxMethod.request(
                  'AuthorDataBaseAjaxOper',
                    {
                        params: {
                           OperCode:OperCode,staffid:staffid,name:name,Remarks:Remarks,optype:optype,MeetName:MeetName
                        },
                        success: function(result) {
                            Store.reload();
                            Store1.reload();
                            btn_EchoHandle.setDisabled(true);
                            btn_Delete.setDisabled(true);
                            btnDelAuthor.setDisabled(true);
                            arcEditAuthor.hide();
                        },
                        failure: function(msg) {
                            GridPanel_Show.el.unmask();
                        }
                    });
        }
       var CheckForm = function() {
            if (txtMeetName.validate() == false) {
                return false;
            }
            return true;
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
    <ext:Hidden ID="HiddenName" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="hiddenMeunUp" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="hiddenAuthor" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="ADD_MARK" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="hiddenEdit" runat="server">
    </ext:Hidden>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="SPEC_CODE" />
                    <ext:RecordField Name="MEET_NAME" />
                    <ext:RecordField Name="GRADE" />
                    <ext:RecordField Name="MEET_DATE" />
                    <ext:RecordField Name="ADD_MARK" />
                    <ext:RecordField Name="SCIENCE_MEETING_PLACE" />
                    <ext:RecordField Name="JOIN_PERSONS" />
                    <ext:RecordField Name="CONTENT" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="SETUP_SUG"/>
                    <ext:RecordField Name="MARK_SUG"/>
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
        <Proxy>
            <ext:HttpProxy Method="POST" Url="/RLZY/WebService/StaffInfos.ashx" />
        </Proxy>
        <Reader>
            <ext:JsonReader Root="StaffInfos">
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="STAFF_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store" runat="server" OnRefreshData="RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="MEET_ID" />
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="MEET_PERSONNEL" />
                    <ext:RecordField Name="MEET_NAME" />
                    <ext:RecordField Name="REMARK" />
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
                                <ext:Label ID="Label1" runat="server" Text="统计月份">
                                </ext:Label>
                                <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                <ext:ComboBox runat="server" ID="Comb_StartYear" Width="60" ListWidth="60" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                <ext:ComboBox runat="server" ID="Comb_StartMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月" />
                                <ext:ComboBox runat="server" ID="Comb_StartDate" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="dd1Name" runat="server" Text="日 " />
                                <ext:ToolbarTextItem ID="ToolbarTextItem7" runat="server" Text="   至   " />
                                <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                <ext:ComboBox runat="server" ID="Comb_EndYear" Width="60" ListWidth="60" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="年" />
                                <ext:ComboBox runat="server" ID="Comb_EndMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem5" runat="server" Text="月" />
                                <ext:ComboBox runat="server" ID="Comb_EndDate" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="dd2Name" runat="server" Text="日 " />
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                <ext:Label ID="Label3" runat="server" Text="科室:">
                                </ext:Label>
                                <ext:ComboBox ID="DeptCodeCombo" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                    Width="70" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
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
                                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                    <Listeners>
                                        <Click Handler="#{Store1}.reload();#{btn_Delete}.disable();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator>
                                </ext:ToolbarSeparator>
                                <ext:Button ID="btn_Add" runat="server" Text="添加会议" Icon="Add">
                                    <Listeners>
                                        <Click Handler="if(#{DeptCodeCombo}.getSelectedItem().value == '') {Ext.Msg.show({ title: '信息提示', msg: '请选择科室', icon: 'ext-mb-info', buttons: { ok: true }  });} else {TreeOpration(1,'')}" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="删除会议" ID="btn_Delete" runat="server" Icon="Delete" Disabled="true">
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
                                                        <ext:Column ColumnID="DEPT_NAME" Header="科室" Sortable="true" DataIndex="DEPT_NAME" />
                                                        <ext:Column ColumnID="MEET_DATE" Header="日期" Sortable="true" DataIndex="MEET_DATE" />
                                                        <ext:Column ColumnID="MEET_NAME" Header="会议名称" Sortable="true" DataIndex="MEET_NAME" />
                                                        <ext:Column ColumnID="GRADE" Header="等级" Sortable="true" DataIndex="GRADE" />
                                                        <ext:Column ColumnID="SCIENCE_MEETING_PLACE" Header="会议地点" Sortable="true" DataIndex="SCIENCE_MEETING_PLACE" />
                                                        <ext:Column ColumnID="JOIN_PERSONS" Header="参会人员" Sortable="true" DataIndex="JOIN_PERSONS" />
                                                        <ext:Column ColumnID="CONTENT" Header="摘要" Sortable="true" DataIndex="CONTENT" />
                                                        <ext:Column ColumnID="ADD_MARK" Header="审批标识" Sortable="true" DataIndex="ADD_MARK" />
                                                        <ext:Column ColumnID="SETUP_SUG" Header="提交意见" Sortable="true" DataIndex="SETUP_SUG" />
                                                        <ext:Column ColumnID="MARK_SUG" Header="审批意见" Sortable="true" DataIndex="MARK_SUG" />
                                                        <ext:CommandColumn Width="38" Header="操作">
                                                            <Commands>
                                                                <ext:SplitCommand Icon="TableMultiple">
                                                                    <ToolTip Text="单项操作" />
                                                                    <Menu>
                                                                        <Items>
                                                                            <ext:MenuCommand CommandName="CmdBJGW" Icon="Wrench" Text="单项处理会议">
                                                                            </ext:MenuCommand>
                                                                            <ext:MenuCommand CommandName="CmdAddAuthor" Icon="TagBlue" Text="添加参加者">
                                                                            </ext:MenuCommand>
                                                                            <ext:MenuCommand CommandName="CmdEditAuthor" Icon="TextListBullets" Text="查看参加者">
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
    <ext:Window ID="arcEditAuthor" runat="server" Icon="Group" Title="参会者编辑" Width="300"
        Height="230" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout3" runat="server">
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel4" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout3" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboStaffInfo" runat="server" StoreID="Store2" DisplayField="STAFF_NAME"
                                        Width="120" ValueField="STAFF_ID" TypeAhead="false" LoadingText="Searching..."
                                        PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="参会者"
                                        ListWidth="240">
                                        <Template ID="Template2" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{STAFF_ID}</span>{STAFF_NAME}　　　　{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                        </Template>
                                    </ext:ComboBox>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txaRemarks" runat="server" FieldLabel="备注" Height="100" />
                                </ext:Anchor>
                            </ext:FormLayout>
                        </Body>
                    </ext:Panel>
                </ext:LayoutColumn>
            </ext:ColumnLayout>
        </Body>
        <BottomBar>
            <ext:Toolbar ID="Toolbar3" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                    <ext:ToolbarButton ID="ToolbarButton1" runat="server" Icon="Disk" Text="保存">
                        <Listeners>
                            <Click Handler="if(#{cboStaffInfo}.getSelectedItem().value != '') {AuthorOpration(1)} else {Ext.Msg.show({ title: '信息提示', msg: '请选择人员', icon: 'ext-mb-info', buttons: { ok: true }  });}" />
                        </Listeners>
                    </ext:ToolbarButton>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                    <ext:ToolbarButton ID="ToolbarButton2" runat="server" Icon="Cancel" Text="退出">
                        <Listeners>
                            <Click Handler="arcEditAuthor.hide();" />
                        </Listeners>
                    </ext:ToolbarButton>
                </Items>
            </ext:Toolbar>
        </BottomBar>
    </ext:Window>
    <ext:Window ID="ViewAuthor" runat="server" Icon="Group" Title="查看参会者" Width="600"
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false">
        <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:Button Text="删除参会者" ID="btnDelAuthor" runat="server" Icon="Delete" Disabled="true">
                        <Listeners>
                            <Click Handler="AuthorOpration(2)" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Body>
            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store" Border="false" AutoWidth="true"
                Header="false" AutoScroll="true" BodyStyle="background-color:Transparent;" Height="300">
                <ColumnModel ID="ColumnModel2" runat="server">
                    <Columns>
                        <ext:Column ColumnID="MEET_NAME" Header="会议名称" Sortable="true" DataIndex="MEET_NAME" />
                        <ext:Column ColumnID="MEET_PERSONNEL" Header="参会人" Sortable="true" DataIndex="MEET_PERSONNEL" />
                        <ext:Column ColumnID="REMARK" Header="备注" Sortable="true" DataIndex="REMARK" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" runat="server">
                        <Listeners>
                            <RowSelect Handler="#{btnDelAuthor}.enable();" />
                            <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{btnDelAuthor}.disable();}" />
                        </Listeners>
                    </ext:CheckboxSelectionModel>
                </SelectionModel>
                <LoadMask ShowMask="true" />
            </ext:GridPanel>
        </Body>
    </ext:Window>
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="学术会议" Width="350"
        Height="370" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:DateField ID="dtfMeetDate" runat="server" FieldLabel="日期" Format="yyyy-MM-dd" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtMeetName" runat="server" FieldLabel="会议名称" CausesValidation="true" AllowBlank="false"/>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboGrade" runat="server" FieldLabel="等级" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtScienceMeetingPlace" runat="server" FieldLabel="地点" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrContent" runat="server" FieldLabel="摘要" Height="50" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrSetSug" runat="server" FieldLabel="提交意见" Height="60" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrSug" runat="server" FieldLabel="审批意见" Height="60" />
                                </ext:Anchor>
                            </ext:FormLayout>
                        </Body>
                    </ext:Panel>
                </ext:LayoutColumn>
            </ext:ColumnLayout>
        </Body>
        <BottomBar>
            <ext:Toolbar ID="Toolbar2" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="Disk" Text="保存">
                        <AjaxEvents>
                            <Click OnEvent="SaveInfo" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="btnSetUp" runat="server" Icon="Disk" Text="提交">
                        <AjaxEvents>
                            <Click OnEvent="SetUpInfo" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="btnApprove" runat="server" Icon="Disk" Text="审批">
                        <AjaxEvents>
                            <Click OnEvent="ApproveInfo" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="btnNotApprove" runat="server" Icon="Disk" Text="审批不通过">
                        <AjaxEvents>
                            <Click OnEvent="NotApproveInfo" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                                <ExtraParams>
                                    <ext:Parameter Name="deptCode" Value="#{Store1}.getAt(RowIndex).data.DEPT_CODE" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="deptName" Value="#{Store1}.getAt(RowIndex).data.DEPT_NAME" Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="btnSetSave" runat="server" Icon="Disk" Text="修改">
                        <AjaxEvents>
                            <Click OnEvent="SetSave" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <%--添加界面直接提交按钮 --%>
                    <ext:ToolbarButton ID="btnSaveSet" runat="server" Icon="Disk" Text="提交">
                        <AjaxEvents>
                            <Click OnEvent="SaveSet" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                            </Click>
                        </AjaxEvents>
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
