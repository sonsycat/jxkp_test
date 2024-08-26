<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RewardList.aspx.cs" Inherits="GoldNet.JXKP.RLZY.BaseInfoManager.RewardList" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>

    <script type="text/javascript">        
        /*
            GRIDPANEL操作
            optype :1 添加;  2 修改; 3 删除;
        */
        var RowIndex;
        
        //操作
        function TreeOpration(optype) {
            if (optype == "1") {
                txtrewarddesc.selectByIndex(0);          
                dtrewardDate.setValue("");
                //ComboBox1.selectByIndex(0);  
                ComboBox1.setValue(DeptCodeCombo.getSelectedItem().value)       
                //ComboBox2.selectByIndex(0);          
                //ComboBox3.selectByIndex(0);

                Btn_BatStart.setText("保存");

                Btn_BatStart.setVisible(true); //保存可见
                btnSetSave.setVisible(false);  //修改不可见

                arcEditWindow.show();
            } else if (optype == "2") {
                var record = Store1.getAt(RowIndex);
                
                HiddenId.setValue(record.data.ID);
                txtrewarddesc.setValue(record.data.REWARD_DESC);        
                dtrewardDate.setValue(record.data.REWARD_DATE);        
                ComboBox1.setValue(record.data.DEPT_CODE);          
                ComboBox2.setValue(record.data.MANAGE_DEPT);         
                ComboBox3.setValue(record.data.MANAGE_PRS); 

                Btn_BatStart.setVisible(false); //保存可见
                btnSetSave.setVisible(true);    //修改不可见
                
                arcEditWindow.show();
            } else if (optype == "3") {

                var selections = CheckboxSelectionModel1.getSelections();
                Ext.Msg.confirm("删除项目", "确定要删除该项目吗？", function(btn, text) { if((btn != "ok") && (btn != "yes")){return;} else {OperEchoCallback(selections,optype);}});
            } 
        }
        
        //删除处理
        function OperEchoCallback(selections,optype) {
            var id = '';
            for(var i =0;i<selections.length;i++) {
                if(i == selections.length -1) {
                    id = id+selections[i].data.ID;
                } else {
                    id = id+selections[i].data.ID +',';
                }
            }
            GridPanelToDataBase(id,"","","","","","",optype,"","");
        }
        
        // 新增保存处理
        function OpCallback (btn) {
            var myDate = new Date();
            var rewardDesc = txtrewarddesc.getSelectedItem().text;      
            var deptCode = ComboBox1.getSelectedItem().value;   
            var deptName = ComboBox1.getSelectedItem().text;
            var StartDateFormat = dtrewardDate.getValue() == ''?myDate:dtrewardDate.getValue();
            var StartDate = StartDateFormat.format('Y-m-d');   
            var manageDept = ComboBox2.getSelectedItem().value;     
            var managePrs = ComboBox3.getSelectedItem().value;   
            var managePrsName = ComboBox3.getSelectedItem().text;   
            var manageDeptName = ComboBox2.getSelectedItem().text;  
 
            GridPanelToDataBase(HiddenId.value,rewardDesc,deptCode,deptName,StartDate,manageDept,managePrs,'2',managePrsName,manageDeptName);
        }
        
        // 调用后台处理过程
        function GridPanelToDataBase(id,rewarddesc,deptcode,deptname,StartDate,managedept,manageprs,optype,managePrsName,manageDeptName) {
          Goldnet.AjaxMethod.request(
                  'ProblemListAjaxOper',
                    {
                        params: {
                           Id:id,rewardDesc:rewarddesc,deptCode:deptcode,deptName:deptname,StartDate:StartDate,manageDept:managedept,managePrs:manageprs,optype:optype,managePrsName:managePrsName,manageDeptName:manageDeptName},
                        success: function(result) {
                            Store1.reload();
                            btn_Delete.setDisabled(true);
                            arcEditWindow.hide();
                        },
                        failure: function(msg) {
                            alert(msg)
                            GridPanel_Show.el.unmask();
                        }
                    });
        }
        
        // 修改保存处理
        function SetSaveMethod(type) {
            var myDate = new Date();                
            var rewarddesc = txtrewarddesc.getSelectedItem().text;       
            var deptname = ComboBox1.getSelectedItem().text;
            var deptcode = ComboBox1.getSelectedItem().value;
            var StartDateFormat = dtrewardDate.getValue() == ''?myDate:dtrewardDate.getValue();
            var StartDate = StartDateFormat.format('Y-m-d');
            var managedept = ComboBox2.getSelectedItem().value;   
            var manageprs = ComboBox3.getSelectedItem().value;        
            var managePrsName = ComboBox3.getSelectedItem().text;   
            var manageDeptName = ComboBox2.getSelectedItem().text;  
             
            GridPanelToDataBase(HiddenId.value,rewarddesc,deptcode,deptname,StartDate,managedept,manageprs,type,managePrsName,manageDeptName);
        }
        
        //日期有效性检查
        var CheckForm = function() {
            if (dtrewardDate.validate() == false) {
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
    <ext:Hidden ID="HiddenId" runat="server">
    </ext:Hidden>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="REWARD_DATE" />
                    <ext:RecordField Name="REWARD_DESC" />
                    <ext:RecordField Name="MANAGE_DEPT" />
                    <ext:RecordField Name="MANAGE_PRS" />
                    <ext:RecordField Name="MANAGE_DEPT_NAME" />
                    <ext:RecordField Name="MANAGE_PRS_NAME" />
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
                                <ext:Label ID="Label1" runat="server" Text="奖励人员：" />
                                <ext:TextField ID="txtPrName" runat="server" Width="60" />
                                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                    <Listeners>
                                        <Click Handler="#{Store1}.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator>
                                </ext:ToolbarSeparator>
                                <ext:Button ID="btn_Add" runat="server" Text="添加" Icon="Add">
                                    <Listeners>
                                        <Click Handler="if(#{DeptCodeCombo}.getSelectedItem().value == '') {Ext.Msg.show({ title: '信息提示', msg: '请选择科室', icon: 'ext-mb-info', buttons: { ok: true }  });} else {TreeOpration(1)}" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="Button1" runat="server" Text="修改" Icon="Wrench" >
                                    <Listeners>
                                        <Click Handler="TreeOpration(2)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="删除" ID="btn_Delete" runat="server" Icon="Delete" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(3)" />
                                    </Listeners>
                                </ext:Button>
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
                                                        <ext:Column ColumnID="ID" Header="编号" Sortable="true" DataIndex="ID" />
                                                        <ext:Column ColumnID="REWARD_DATE" Header="奖励时间" Sortable="true" DataIndex="REWARD_DATE" />
                                                        <ext:Column ColumnID="DEPT_NAME" Header="科室名称" Sortable="true" DataIndex="DEPT_NAME" />
                                                        <ext:Column ColumnID="REWARD_DESC" Header="奖励名称" Sortable="true" DataIndex="REWARD_DESC" />
                                                        <ext:Column ColumnID="MANAGE_DEPT_NAME" Header="批准单位" Sortable="true" DataIndex="MANAGE_DEPT_NAME" />
                                                        <ext:Column ColumnID="MANAGE_PRS_NAME" Header="奖励人员" Sortable="true" DataIndex="MANAGE_PRS_NAME" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server">
                                                        <Listeners>
                                                            <RowSelect Handler="#{btn_Delete}.enable();RowIndex = rowIndex" />
                                                            <RowDeselect Handler="if (!#{GridPanel_Show}.hasSelection()) {#{btn_Delete}.disable();}" />
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
    
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="奖励信息" Width="600"
        Height="410" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                     <ext:ComboBox ID="txtrewarddesc" runat="server" FieldLabel="奖励名称" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:DateField ID="dtrewardDate" runat="server" FieldLabel="奖励时间" Format="yyyy-MM-dd" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="ComboBox1" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                        Width="120" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                        PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="科室名称"
                                        ListWidth="240">
                                        <Template ID="Template3" runat="server">
                                           <tpl for=".">
                                              <div class="search-item">
                                                 <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                              </div>
                                           </tpl>
                                        </Template>
                                    </ext:ComboBox>
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
                                    <ext:ComboBox ID="ComboBox2" runat="server" FieldLabel="批准单位" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="ComboBox3" runat="server" StoreID="Store2" DisplayField="STAFF_NAME"
                                        Width="120" ValueField="STAFF_ID" TypeAhead="false" LoadingText="Searching..."
                                        PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="奖励人员"
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
                        <Listeners>
                            <Click Handler="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！')} else {OpCallback('ok');}" />
                        </Listeners>
                    </ext:ToolbarButton>

                    <ext:ToolbarButton ID="btnSetSave" runat="server" Icon="Disk" Text="修改">
                        <Listeners>
                            <Click Handler="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！')} else {SetSaveMethod('7');}" />
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

