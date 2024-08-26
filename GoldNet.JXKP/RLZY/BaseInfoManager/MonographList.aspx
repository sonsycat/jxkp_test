<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonographList.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoManager.MonographList" %>

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
                txtMonographName.setValue("");
                txtPublish.setValue("");      
                txtMagaNo.setValue("");       
                txtWordCount.setValue("");    
                cboFormat.selectByIndex(0);       
                txrContent.setValue("");      
                cboDiscouLevel.selectByIndex(0); 
                dtfPublishDate.setValue(data);  
                txtMagaName.setValue("");     
                txtCallNumber.setValue("");   
                txtAmount.setValue("");       
                txtMeetName.setValue("");     
                txtAuthor.setValue("");       
                cboDuty.selectByIndex(0);
                dtfMeetDate.setValue("");

                txrSug.setValue("");
                txrSetSug.setValue("");

                txtMonographName.setDisabled(false);  
                txtPublish.setDisabled(false);        
                txtMagaNo.setDisabled(false);         
                txtWordCount.setDisabled(false);      
                cboFormat.setDisabled(false);        
                txrContent.setDisabled(false);        
                cboDiscouLevel.setDisabled(false);  
                dtfPublishDate.setDisabled(false); 
                txtMagaName.setDisabled(false);       
                txtCallNumber.setDisabled(false);     
                txtAmount.setDisabled(false);         
                txtMeetName.setDisabled(false);       
                txtAuthor.setDisabled(false);         
                cboDuty.setDisabled(false);
                dtfMeetDate.setDisabled(false);

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
                txtMonographName.setValue(record.data.MONOGRAPH_NAME);   //             专著名称
                txtPublish.setValue(record.data.PUBLISH);   //             出版社
                txtMagaNo.setValue(record.data.MAGA_NO);   //             期刊号
                txtWordCount.setValue(record.data.WORD_COUNT);   //             字数(千字)
                cboFormat.setValue(record.data.FORMAT);   //             开本
                txrContent.setValue(record.data.CONTENT);   //             内容简介      
                cboDiscouLevel.setValue(record.data.DISCOU_LEVEL);   //             专著级别
                dtfPublishDate.setValue(record.data.PUBLISH_DATE);   //             出版日期
                txtMagaName.setValue(record.data.MAGA_NAME);   //             杂志名称
                txtCallNumber.setValue(record.data.CALL_NUMBER);   //             图书编号
                txtAmount.setValue(record.data.AMOUNT);   //             印刷数量
                txtMeetName.setValue(record.data.MEET_NAME);   //             会议名称
                txtAuthor.setValue(record.data.AUTHOR);   //             作者
                cboDuty.setValue(record.data.DUTY);   //             担任职务
                dtfMeetDate.setValue(record.data.MEET_DATE);   //             会议时间

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

                    txtMonographName.setDisabled(false);
                    txtPublish.setDisabled(false);
                    txtMagaNo.setDisabled(false);
                    txtWordCount.setDisabled(false);
                    cboFormat.setDisabled(false);
                    txrContent.setDisabled(false);
                    cboDiscouLevel.setDisabled(false);
                    dtfPublishDate.setDisabled(false);
                    txtMagaName.setDisabled(false);
                    txtCallNumber.setDisabled(false);
                    txtAmount.setDisabled(false);
                    txtMeetName.setDisabled(false);
                    txtAuthor.setDisabled(false);
                    cboDuty.setDisabled(false);
                    dtfMeetDate.setDisabled(false);

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
                            txtMonographName.setDisabled(true);
                            txtPublish.setDisabled(true);
                            txtMagaNo.setDisabled(true);
                            txtWordCount.setDisabled(true);
                            cboFormat.setDisabled(true);
                            txrContent.setDisabled(true);
                            cboDiscouLevel.setDisabled(true);
                            dtfPublishDate.setDisabled(true);
                            txtMagaName.setDisabled(true);
                            txtCallNumber.setDisabled(true);
                            txtAmount.setDisabled(true);
                            txtMeetName.setDisabled(true);
                            txtAuthor.setDisabled(true);
                            cboDuty.setDisabled(true);
                            dtfMeetDate.setDisabled(true);

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
                            txtMonographName.setDisabled(false);
                            txtPublish.setDisabled(false);
                            txtMagaNo.setDisabled(false);
                            txtWordCount.setDisabled(false);
                            cboFormat.setDisabled(false);
                            txrContent.setDisabled(false);
                            cboDiscouLevel.setDisabled(false);
                            dtfPublishDate.setDisabled(false);
                            txtMagaName.setDisabled(false);
                            txtCallNumber.setDisabled(false);
                            txtAmount.setDisabled(false);
                            txtMeetName.setDisabled(false);
                            txtAuthor.setDisabled(false);
                            cboDuty.setDisabled(false);
                            dtfMeetDate.setDisabled(false);

                            Btn_BatStart.setVisible(false); //保存按钮不可见
                            btnSetUp.setVisible(true);   //纯提交，可见
                            btnApprove.setVisible(false); //审批按钮不可见
                            btnNotApprove.setVisible(false); //审批不通过按钮不可见
                            btnSetSave.setVisible(true); //修改按钮可见
                            btnSaveSet.setVisible(false); //保存时提交，不可见
                        }
                    }
                    if (cbxOpration.checked) {
                        txtMonographName.setDisabled(false);
                        txtPublish.setDisabled(false);
                        txtMagaNo.setDisabled(false);
                        txtWordCount.setDisabled(false);
                        cboFormat.setDisabled(false);
                        txrContent.setDisabled(false);
                        cboDiscouLevel.setDisabled(false);
                        dtfPublishDate.setDisabled(false);
                        txtMagaName.setDisabled(false);
                        txtCallNumber.setDisabled(false);
                        txtAmount.setDisabled(false);
                        txtMeetName.setDisabled(false);
                        txtAuthor.setDisabled(false);
                        cboDuty.setDisabled(false);
                        dtfMeetDate.setDisabled(false);

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
                        txtMonographName.setDisabled(false);
                        txtPublish.setDisabled(false);
                        txtMagaNo.setDisabled(false);
                        txtWordCount.setDisabled(false);
                        cboFormat.setDisabled(false);
                        txrContent.setDisabled(false);
                        cboDiscouLevel.setDisabled(false);
                        dtfPublishDate.setDisabled(false);
                        txtMagaName.setDisabled(false);
                        txtCallNumber.setDisabled(false);
                        txtAmount.setDisabled(false);
                        txtMeetName.setDisabled(false);
                        txtAuthor.setDisabled(false);
                        cboDuty.setDisabled(false);
                        dtfMeetDate.setDisabled(false);

                        Btn_BatStart.setVisible(false); //保存按钮不可见
                        btnSetUp.setVisible(false);   //纯提交，不可见
                        btnApprove.setVisible(false); //审批按钮不可见
                        btnNotApprove.setVisible(false); //审批不通过按钮不可见
                        btnSetSave.setVisible(true); //修改按钮可见
                        btnSaveSet.setVisible(false); //保存时提交，不可见
                    }
                    if (!cbxOpration.checked) {
                        if (record.data.ADD_MARK == "审批通过") {  //审批已通过的不可再修改
                            txtMonographName.setDisabled(true);
                            txtPublish.setDisabled(true);
                            txtMagaNo.setDisabled(true);
                            txtWordCount.setDisabled(true);
                            cboFormat.setDisabled(true);
                            txrContent.setDisabled(true);
                            cboDiscouLevel.setDisabled(true);
                            dtfPublishDate.setDisabled(true);
                            txtMagaName.setDisabled(true);
                            txtCallNumber.setDisabled(true);
                            txtAmount.setDisabled(true);
                            txtMeetName.setDisabled(true);
                            txtAuthor.setDisabled(true);
                            cboDuty.setDisabled(true);
                            dtfMeetDate.setDisabled(true);

                            Btn_BatStart.setVisible(false); //保存按钮不可见
                            btnSetUp.setVisible(false);   //纯提交，不可见
                            btnApprove.setVisible(false); //审批按钮不可见
                            btnNotApprove.setVisible(false); //审批不通过按钮不可见
                            btnSetSave.setVisible(false); //修改按钮不可见
                            btnSaveSet.setVisible(false); //保存时提交，不可见
                        }
                        else { //审批不通过的可以再修改
                            txtMonographName.setDisabled(false);
                            txtPublish.setDisabled(false);
                            txtMagaNo.setDisabled(false);
                            txtWordCount.setDisabled(false);
                            cboFormat.setDisabled(false);
                            txrContent.setDisabled(false);
                            cboDiscouLevel.setDisabled(false);
                            dtfPublishDate.setDisabled(false);
                            txtMagaName.setDisabled(false);
                            txtCallNumber.setDisabled(false);
                            txtAmount.setDisabled(false);
                            txtMeetName.setDisabled(false);
                            txtAuthor.setDisabled(false);
                            cboDuty.setDisabled(false);
                            dtfMeetDate.setDisabled(false);

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
              cboRanking.selectByIndex(0);
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
                var AuthorRanking = cboRanking.getSelectedItem().text;
                var Remarks = txaRemarks.getValue();
                var code = HiddenId.getValue();
                AuthorDataBaseOper(code,staffid,name,AuthorRanking,Remarks,optype);
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
                AuthorDataBaseOper(id,"","","","",optype);
            }
        }
        
        var AuthorDataBaseOper = function(OperCode,staffid,name,AuthorRanking,Remarks,optype) {
            Goldnet.AjaxMethod.request(
                  'AuthorDataBaseAjaxOper',
                    {
                        params: {
                           OperCode:OperCode,staffid:staffid,name:name,AuthorRanking:AuthorRanking,Remarks:Remarks,optype:optype
                        },
                        success: function(result) {
                            if(optype == '2') {
                                Store.reload();
                            } else {
                                Store1.reload();
                            }
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
        
         var applyFilter = function() {
            Store1.filterBy(getRecordFilter());
        };
        var getRecordFilter = function() {
            var f = [];
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'AUTHOR', record);
                }
            });
            return function(record) {
                if (f[0].filter(record)) {
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
        
        
        var CheckForm = function() {
            if (dtfPublishDate.validate() == false) {
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
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="MONOGRAPH_NAME" />
                    <ext:RecordField Name="PUBLISH" />
                    <ext:RecordField Name="PUBLISH_DATE" />
                    <ext:RecordField Name="WORD_COUNT" />
                    <ext:RecordField Name="CALL_NUMBER" />
                    <ext:RecordField Name="FORMAT" />
                    <ext:RecordField Name="AMOUNT" />
                    <ext:RecordField Name="AUTHOR" />
                    <ext:RecordField Name="CONTENT" />
                    <ext:RecordField Name="MAGA_NAME" />
                    <ext:RecordField Name="MAGA_NO" />
                    <ext:RecordField Name="MEET_NAME" />
                    <ext:RecordField Name="MEET_DATE" />
                    <ext:RecordField Name="DISCOU_LEVEL" />
                    <ext:RecordField Name="DUTY" />
                    <ext:RecordField Name="MARK_SUG"/>
                    <ext:RecordField Name="SETUP_SUG"/>
                    <ext:RecordField Name="ADD_MARK" />
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
                    <ext:RecordField Name="MONOGRAPH_CODE" />
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="STAFF_NAME" />
                    <ext:RecordField Name="RANK" />
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
                                <ext:Label ID="Label2" runat="server" Text="年度:">
                                </ext:Label>
                                <ext:ComboBox ID="TimeOrgan" runat="server" Width="40">
                                    <Items>
                                        <ext:ListItem Text="&lt;=" Value="&lt;=" />
                                        <ext:ListItem Text="&gt;=" Value="&gt;=" />
                                        <ext:ListItem Text="=" Value="=" />
                                    </Items>
                                </ext:ComboBox>
                                <ext:ComboBox ID="cboTime" runat="server" Width="55">
                                </ext:ComboBox>
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
                                <ext:Button ID="btn_Add" runat="server" Text="添加专著" Icon="Add">
                                    <Listeners>
                                        <Click Handler="if(#{DeptCodeCombo}.getSelectedItem().value == '') {Ext.Msg.show({ title: '信息提示', msg: '请选择科室', icon: 'ext-mb-info', buttons: { ok: true }  });} else {TreeOpration(1,'')}" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="删除专著" ID="btn_Delete" runat="server" Icon="Delete" Disabled="true">
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
                                                        <ext:Column ColumnID="MONOGRAPH_NAME" Header="专著名称" Sortable="true" DataIndex="MONOGRAPH_NAME" />
                                                        <ext:Column ColumnID="PUBLISH" Header="出版社" Sortable="true" DataIndex="PUBLISH" />
                                                        <ext:Column ColumnID="PUBLISH_DATE" Header="出版日期" Sortable="true" DataIndex="PUBLISH_DATE" />
                                                        <ext:Column ColumnID="AUTHOR" Header="作者" Sortable="true" DataIndex="AUTHOR" />
                                                        <ext:Column ColumnID="ADD_MARK" Header="审批标识" Sortable="true" DataIndex="ADD_MARK" />
                                                        <ext:Column ColumnID="SETUP_SUG" Header="提交意见" Sortable="true" DataIndex="SETUP_SUG" />
                                                        <ext:Column ColumnID="MARK_SUG" Header="审批意见" Sortable="true" DataIndex="MARK_SUG" />
                                                        <ext:CommandColumn Width="38" Header="操作">
                                                            <Commands>
                                                                <ext:SplitCommand Icon="TableMultiple">
                                                                    <ToolTip Text="单项操作" />
                                                                    <Menu>
                                                                        <Items>
                                                                            <ext:MenuCommand CommandName="CmdBJGW" Icon="Wrench" Text="单项处理专著">
                                                                            </ext:MenuCommand>
                                                                            <ext:MenuCommand CommandName="CmdAddAuthor" Icon="TagBlue" Text="添加作者">
                                                                            </ext:MenuCommand>
                                                                            <ext:MenuCommand CommandName="CmdEditAuthor" Icon="TextListBullets" Text="查看作者">
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
                                                <BottomBar>
                                                    <ext:Toolbar ID="Toolbar4" runat="server" Height="26">
                                                        <Items>
                                                            <ext:TextField ID="txt_SearchTxt" runat="server" EmptyText="查找信息">
                                                                <ToolTips>
                                                                    <ext:ToolTip ID="ToolTip1" runat="server" Html="根据完成作者关键字查找">
                                                                    </ext:ToolTip>
                                                                </ToolTips>
                                                            </ext:TextField>
                                                            <ext:Button ID="btn_Search" Icon="Zoom" runat="server" Text="查询">
                                                                <Listeners>
                                                                    <Click Fn="applyFilter" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </BottomBar>
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
    <ext:Window ID="arcEditAuthor" runat="server" Icon="Group" Title="作者编辑" Width="300"
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
                                        PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="负责人"
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
                                    <ext:ComboBox ID="cboRanking" runat="server" FieldLabel="排名" />
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
    <ext:Window ID="ViewAuthor" runat="server" Icon="Group" Title="查看作者" Width="600"
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false">
        <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:Button Text="删除作者" ID="btnDelAuthor" runat="server" Icon="Delete" Disabled="true">
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
                        <ext:Column ColumnID="MONOGRAPH_CODE" Header="专著编号" Sortable="true" DataIndex="MONOGRAPH_CODE" />
                        <ext:Column ColumnID="STAFF_NAME" Header="负责人" Sortable="true" DataIndex="STAFF_NAME" />
                        <ext:Column ColumnID="RANK" Header="排名" Sortable="true" DataIndex="RANK" />
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
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="专著" Width="600"
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtMonographName" runat="server" FieldLabel="专著名称" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtPublish" runat="server" FieldLabel="出版社" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtMagaNo" runat="server" FieldLabel="期刊号" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtWordCount" runat="server" FieldLabel="字数(千字)" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboFormat" runat="server" FieldLabel="开本" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrContent" runat="server" FieldLabel="内容简介" Height="50" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrSug" runat="server" FieldLabel="审批意见" Height="120" />
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
                                    <ext:ComboBox ID="cboDiscouLevel" runat="server" FieldLabel="专著级别" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:DateField ID="dtfPublishDate" runat="server" FieldLabel="出版日期" Format="yyyy-MM-dd" CausesValidation="true" AllowBlank="false"/>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtMagaName" runat="server" FieldLabel="杂志名称" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtCallNumber" runat="server" FieldLabel="图书编号" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtAmount" runat="server" FieldLabel="印刷数量" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtMeetName" runat="server" FieldLabel="会议名称" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtAuthor" runat="server" FieldLabel="作者" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboDuty" runat="server" FieldLabel="担任职务" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:DateField ID="dtfMeetDate" runat="server" FieldLabel="会议时间" Format="yyyy-MM-dd" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrSetSug" runat="server" FieldLabel="提交意见" Height="60" />
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
                                <ExtraParams>
                                    <ext:Parameter Name="deptCode" Value="#{Store1}.getAt(RowIndex).data.DEPT_CODE" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="deptName" Value="#{Store1}.getAt(RowIndex).data.DEPT_NAME" Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="btnApprove" runat="server" Icon="Disk" Text="审批">
                        <AjaxEvents>
                            <Click OnEvent="ApproveInfo" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                                <ExtraParams>
                                    <ext:Parameter Name="deptCode" Value="#{Store1}.getAt(RowIndex).data.DEPT_CODE" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="deptName" Value="#{Store1}.getAt(RowIndex).data.DEPT_NAME" Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
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
                                <ExtraParams>
                                    <ext:Parameter Name="deptCode" Value="#{Store1}.getAt(RowIndex).data.DEPT_CODE" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="deptName" Value="#{Store1}.getAt(RowIndex).data.DEPT_NAME" Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
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
