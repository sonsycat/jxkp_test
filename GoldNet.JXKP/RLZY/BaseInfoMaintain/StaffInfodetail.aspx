<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffInfodetail.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.StaffInfodetail" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
        td strong
        {
            color: Red;
        }
    </style>

    <script type="text/javascript"> 
      function checkType(){
            //得到上传文件的值
            var fileName=document.getElementById("photoimg").value;
            //返回String对象中子字符串最后出现的位置.
            var seat=fileName.lastIndexOf(".");
            //返回位于String对象中指定位置的子字符串并转换为小写.
            var extension=fileName.substring(seat).toLowerCase();
            //判断允许上传的文件格式
            if(extension!=".jpg"&&extension!=".jpeg"&&extension!=".gif"&&extension!=".png"&&extension!=".bmp"){
              Ext.Msg.show({ title: '信息提示', msg: '不支持'+extension+'文件的上传', icon: 'ext-mb-info', buttons: { ok: true }});
              return false;
            }else{
              return true;
            }
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
    <ext:Store ID="Store2" runat="server" AutoLoad="true">
        <Proxy>
            <ext:HttpProxy Method="POST" Url="/RLZY/WebService/NationInfos.ashx" />
        </Proxy>
        <Reader>
            <ext:JsonReader Root="NationInfos">
                <Fields>
                    <ext:RecordField Name="NATION_NAME" />
                    <ext:RecordField Name="NATION_CODE" />
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
    <ext:Store ID="Store4" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader Root="UserInfos">
                <Fields>
                    <ext:RecordField Name="USER_ID" />
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="USER_NAME" />
                    <ext:RecordField Name="DEPT_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="StoreCombo" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="STATION_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <body>
        <ext:FieldSet ID="FieldSet1" runat="server" Width="780" Title="基本信息" Collapsible="true"
            StyleSpec="margin:5px;">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                    <ext:LayoutColumn ColumnWidth=".35">
                        <ext:Panel ID="Panel2" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                            BodyStyle="background-color:Transparent;">
                            <Body>
                                <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="DeptCodeCombo" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                            Width="90" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                            PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="科室" ListWidth="240">
                                            <Template ID="Template1" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                            </Template>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:Panel runat="server" ID="panel6666" StyleSpec="background-color:Transparent;"
                                            BodyStyle="background-color:Transparent;" Border="false">
                                            <Body>
                                                <table>
                                                    <tr>
                                                        <td style="width: 104px">
                                                            <ext:Label ID="Label1" Text="姓名:" runat="server">
                                                            </ext:Label>
                                                        </td>
                                                        <td>
                                                            <ext:ComboBox ID="txtStaffInput" runat="server" StoreID="Store4" DisplayField="USER_NAME"
                                                                Width="120" ValueField="USER_ID" TypeAhead="false" LoadingText="Searching..."
                                                                PageSize="3000" ItemSelector="div.search-item" MinChars="1" FieldLabel="姓名" ListWidth="240"
                                                                CausesValidation="true" AllowBlank="false" MaxLength="20" ForceSelection="false"
                                                                AutoDataBind="true">
                                                                <Template ID="Template3" runat="server">
                                                                   <tpl for=".">
                                                                      <div class="search-item">
                                                                         <h3><span style="width:auto">{DEPT_NAME}</span>{USER_NAME}</h3>
                                                                      </div>
                                                                   </tpl>
                                                                </Template>
                                                            </ext:ComboBox>
                                                        </td>
                                                        <td>
                                                            <ext:Checkbox ID="cboInptName" runat="server">
                                                                <ToolTips>
                                                                    <ext:ToolTip ID="ToolTip1" Html="选择军卫" runat="server">
                                                                    </ext:ToolTip>
                                                                </ToolTips>
                                                            </ext:Checkbox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Body>
                                        </ext:Panel>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:DateField ID="dtfBirthday" runat="server" FieldLabel="出生年月" LabelStyle="color:blue;"
                                            Format="yyyy-MM-dd" CausesValidation="true" AllowBlank="false">
                                        </ext:DateField>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cboSex" runat="server" FieldLabel="性别" Editable="false">
                                            <Items>
                                                <ext:ListItem Text="男" Value="男" />
                                                <ext:ListItem Text="女" Value="女" />
                                            </Items>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cboDuty" runat="server" FieldLabel="行政职务" Editable="false" LabelStyle="color:blue;"
                                            CausesValidation="true" AllowBlank="false">
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cboRootspecsort" runat="server" FieldLabel="从事专业" LabelStyle="color:blue;"
                                            CausesValidation="true" AllowBlank="false" Editable="false" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cboSanispecsort" runat="server" FieldLabel="卫生专业分类" Editable="false">
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextField ID="empno" runat="server" FieldLabel="人员工号" Editable="false" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:DateField ID="dtfContractStart" runat="server" FieldLabel="合同开始时间" Format="yyyy-MM-dd"
                                            CausesValidation="true" AllowBlank="false">
                                        </ext:DateField>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="ComboBox2" runat="server" FieldLabel="是否发放奖金" Editable="false"
                                            LabelStyle="color:blue;" CausesValidation="true" AllowBlank="false">
                                            <Items>
                                                <ext:ListItem Text="否" Value="否" />
                                                <ext:ListItem Text="是" Value="是" />
                                            </Items>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                    <ext:LayoutColumn ColumnWidth=".35">
                        <ext:Panel ID="Panel3" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                            BodyStyle="background-color:Transparent;">
                            <Body>
                                <ext:FormLayout ID="FormLayout3" runat="server" LabelAlign="Left">
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cboBraid" runat="server" FieldLabel="实虚编" SelectedIndex="0" Editable="false"
                                            CausesValidation="true" AllowBlank="false">
                                            <Items>
                                                <ext:ListItem Text="实编" Value="0" />
                                                <ext:ListItem Text="虚编" Value="1" />
                                            </Items>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cboRank" runat="server" FieldLabel="级别" Editable="false">
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:DateField ID="dtfDutydate" runat="server" FieldLabel="行政职务时间" Format="yyyy-MM-dd">
                                        </ext:DateField>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cboPeople" runat="server" StoreID="Store2" DisplayField="NATION_NAME"
                                            Width="90" ValueField="NATION_CODE" TypeAhead="false" LoadingText="Searching..."
                                            PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="民族" ListWidth="240">
                                            <Template ID="Template2" runat="server">
                                                   <tpl for=".">
                                                      <div class="search-item">
                                                         <h3><span style="width:auto">{NATION_CODE}</span>{NATION_NAME}</h3>
                                                      </div>
                                                   </tpl>
                                            </Template>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cboIsOnGuard" runat="server" FieldLabel="在岗否" Editable="false">
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cboStation" runat="server" FieldLabel="岗位名称" StoreID="StoreCombo"
                                            DisplayField="STATION_NAME" ValueField="ID" Editable="false" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cboPerssort" runat="server" FieldLabel="人员类别" Editable="false"
                                            LabelStyle="color:blue;" CausesValidation="true" AllowBlank="false" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cboGord" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                            Width="90" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                            PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="核算组"
                                            ListWidth="240">
                                            <Template ID="Template4" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                            </Template>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:DateField ID="dtfContractEnd" runat="server" FieldLabel="合同终止时间" Format="yyyy-MM-dd"
                                            CausesValidation="true" AllowBlank="false">
                                        </ext:DateField>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="ComboBox3" runat="server" FieldLabel="是否考勤" Editable="false" LabelStyle="color:blue;"
                                            CausesValidation="true" AllowBlank="false">
                                            <Items>
                                                <ext:ListItem Text="否" Value="否" />
                                                <ext:ListItem Text="是" Value="是" />
                                            </Items>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                    <ext:LayoutColumn ColumnWidth=".3">
                        <ext:Panel ID="Panel4" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                            BodyStyle="background-color:Transparent;">
                            <Body>
                                <ext:Image runat="server" Width="210" Height="200" ID="imgStaff" ImageUrl="/resources/UploadPicfile/user_default.png">
                                </ext:Image>
                                <div style="padding-left: -10px;">
                                    <ext:FileUploadField ID="photoimg" runat="server" ButtonOnly="true" ButtonText="上传图片"
                                        Icon="ImageAdd" Width="150">
                                        <AjaxEvents>
                                            <FileSelected OnEvent="UploadClick" Before="return checkType();">
                                            </FileSelected>
                                        </AjaxEvents>
                                    </ext:FileUploadField>
                                </div>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                </ext:ColumnLayout>
            </Body>
        </ext:FieldSet>
        <ext:FieldSet ID="FieldSet2" runat="server" Width="780" Title="相关信息" StyleSpec="margin:5px;"
            Collapsible="true" Collapsed="false">
            <Body>
                <ext:Panel ID="Panel345" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                    BodyStyle="background-color:Transparent;">
                    <Body>
                        <ext:ColumnLayout ID="ColumnLayout3" runat="server">
                            <ext:LayoutColumn ColumnWidth=".25">
                                <ext:Panel ID="Panel5" runat="server" Border="false" Header="false" StyleSpec="background-color:Transparent;"
                                    BodyStyle="background-color:Transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left" LabelWidth="78">
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboSpeciality" runat="server" FieldLabel="所学专业" Editable="false" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboJobDuty" runat="server" FieldLabel="技术职务" LabelStyle="color:blue;"
                                                    CausesValidation="true" AllowBlank="false" Editable="false" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboTiptopLearnStuffer" runat="server" FieldLabel="学历" Editable="false" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboTitle" runat="server" FieldLabel="职称" Editable="false" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboTitleList" runat="server" FieldLabel="职称序列" LabelStyle="color:blue;"
                                                    CausesValidation="true" AllowBlank="false" Editable="false" />
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth=".25">
                                <ext:Panel ID="Panel6" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                    BodyStyle="background-color:Transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout5" runat="server" LabelAlign="Left" LabelWidth="88">
                                            <ext:Anchor Horizontal="92%">
                                                <ext:TextField ID="txtRetainTerm" runat="server" FieldLabel="受聘期限" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboDegree" runat="server" FieldLabel="学位" Editable="false" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboCadreType" runat="server" FieldLabel="干部类别" Editable="false" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboDeptType" runat="server" FieldLabel="所在科室类" Editable="false" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboBackboneCircs" runat="server" FieldLabel="专家骨干情况" Editable="false">
                                                    <Items>
                                                        <ext:ListItem Text="医院名医" Value="医院名医" />
                                                        <ext:ListItem Text="博士生导师" Value="博士生导师" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth=".25">
                                <ext:Panel ID="Panel7" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                    BodyStyle="background-color:Transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout4" runat="server" LabelAlign="Left" LabelWidth="78">
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cmbMaritalStatus" runat="server" FieldLabel="婚姻状况" Editable="false">
                                                    <Items>
                                                        <ext:ListItem Text="已婚" Value="已婚" />
                                                        <ext:ListItem Text="未婚" Value="未婚" />
                                                        <ext:ListItem Text="其他" Value="其他" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:DateField ID="dtfJobDate" runat="server" FieldLabel="技术职务时间" Format="yyyy-MM-dd">
                                                </ext:DateField>
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:DateField ID="dtfStudyOverdate" runat="server" FieldLabel="毕业时间" Format="yyyy-MM-dd">
                                                </ext:DateField>
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:DateField ID="dtfWorkDate" runat="server" FieldLabel="工作时间" Format="yyyy-MM-dd">
                                                </ext:DateField>
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:TextField ID="txtGraduateAcademy" runat="server" FieldLabel="毕业院校" Format="yyyy-MM-dd" />
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth=".25">
                                <ext:Panel ID="Panel8" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                    BodyStyle="background-color:Transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout6" runat="server" LabelAlign="Left" LabelWidth="78">
                                            <ext:Anchor Horizontal="92%">
                                                <ext:TextField ID="txtCredithourPerYear" runat="server" FieldLabel="年平均学分" Hidden="true" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:DateField ID="dtfTitleAssess" runat="server" FieldLabel="资格评定时间" Format="yyyy-MM-dd">
                                                </ext:DateField>
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:NumberField ID="txtBonusCoefficient" runat="server" FieldLabel="奖金系数" Hidden="true" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:DateField ID="dtfInHospitalDate" runat="server" FieldLabel="来院时间" Format="yyyy-MM-dd">
                                                </ext:DateField>
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboTechnicTitle" runat="server" FieldLabel="技术资格" Editable="false" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboGovAllowance" runat="server" FieldLabel="政府津贴" Editable="false">
                                                    <Items>
                                                        <ext:ListItem Text="不享受" Value="不享受" />
                                                        <ext:ListItem Text="享受" Value="享受" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:DateField ID="dtfGradetitleDate" runat="server" FieldLabel="取得学历时间" Format="yyyy-MM-dd">
                                                </ext:DateField>
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                        </ext:ColumnLayout>
                    </Body>
                </ext:Panel>
                <ext:Panel ID="Panel9" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                    BodyStyle="background-color:Transparent;">
                    <Body>
                        <ext:ColumnLayout ID="ColumnLayout4" runat="server">
                            <ext:LayoutColumn ColumnWidth=".5">
                                <ext:Panel ID="Panel10" runat="server" Border="false" Header="false" StyleSpec="background-color:Transparent;"
                                    BodyStyle="background-color:Transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout7" runat="server" LabelWidth="78">
                                            <ext:Anchor>
                                                <ext:TextField ID="txtCertificateNo" runat="server" Width="280" FieldLabel="证件号码"
                                                    MaxLength="30" />
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth=".5">
                                <ext:Panel ID="Panel11" runat="server" Border="false" Header="false" StyleSpec="background-color:Transparent;"
                                    BodyStyle="background-color:Transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout8" runat="server" LabelWidth="78">
                                            <ext:Anchor>
                                                <ext:TextField ID="txtHomeplace" runat="server" Width="280" FieldLabel="地址" MaxLength="100" />
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                        </ext:ColumnLayout>
                    </Body>
                </ext:Panel>
                <ext:Panel ID="Panel19" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                    BodyStyle="background-color:Transparent;">
                    <Body>
                        <ext:ColumnLayout ID="ColumnLayout6" runat="server">
                            <ext:LayoutColumn ColumnWidth=".5">
                                <ext:Panel ID="Panel20" runat="server" Border="false" Header="false" StyleSpec="background-color:Transparent;"
                                    BodyStyle="background-color:Transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout14" runat="server" LabelWidth="78">
                                            <ext:Anchor>
                                                <ext:TextField ID="txtBonusNum" runat="server" Width="280" FieldLabel="银行账号" MaxLength="30" />
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                        </ext:ColumnLayout>
                    </Body>
                </ext:Panel>
                <ext:Panel ID="Panel12" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                    BodyStyle="background-color:Transparent;">
                    <Body>
                        <ext:FormLayout ID="FormLayout9" runat="server" LabelWidth="78">
                            <ext:Anchor>
                                <ext:TextArea ID="txtMemo" runat="server" Width="660" FieldLabel="备 注" MaxLength="300">
                                </ext:TextArea>
                            </ext:Anchor>
                        </ext:FormLayout>
                    </Body>
                </ext:Panel>
            </Body>
        </ext:FieldSet>
        <ext:FieldSet ID="FieldSet3" runat="server" Width="780" Title="军人相关" StyleSpec="margin:5px;"
            Collapsible="true" Collapsed="false">
            <Body>
                <ext:Panel ID="Panel17" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                    BodyStyle="background-color:Transparent;">
                    <Body>
                        <ext:ColumnLayout ID="ColumnLayout5" runat="server">
                            <ext:LayoutColumn ColumnWidth=".25">
                                <ext:Panel ID="Panel13" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                    BodyStyle="background-color:Transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout10" runat="server" LabelAlign="Left" LabelWidth="78">
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboIfarmy" runat="server" FieldLabel="是否军人" LabelStyle="color:blue;"
                                                    CausesValidation="true" AllowBlank="false" Editable="false">
                                                    <Items>
                                                        <ext:ListItem Text="是" Value="1" />
                                                        <ext:ListItem Text="否" Value="0" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:DateField ID="dtfBeEnrolledInDate" runat="server" FieldLabel="入伍时间" Format="yyyy-MM-dd">
                                                </ext:DateField>
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth=".25">
                                <ext:Panel ID="Panel15" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                    BodyStyle="background-color:Transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout12" runat="server" LabelAlign="Left" LabelWidth="78">
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboCivilServiceClass" runat="server" FieldLabel="文职级" Editable="false" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:DateField ID="dtfCivilServiceClassDate" runat="server" FieldLabel="文职级时间" Format="yyyy-MM-dd">
                                                </ext:DateField>
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth=".25">
                                <ext:Panel ID="Panel16" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                    BodyStyle="background-color:Transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout13" runat="server" LabelAlign="Left" LabelWidth="78">
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="ComboBox1" runat="server" FieldLabel="文职级" Editable="false" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:DateField ID="DateField1" runat="server" FieldLabel="离职时间" Format="yyyy-MM-dd">
                                                </ext:DateField>
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth=".25">
                                <ext:Panel ID="Panel14" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                    BodyStyle="background-color:Transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout11" runat="server" LabelAlign="Left" LabelWidth="78">
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboTechnicClass" runat="server" FieldLabel="技术级" Editable="false" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:DateField ID="dtfTechnicClassDate" runat="server" FieldLabel="技术级时间" Format="yyyy-MM-dd">
                                                </ext:DateField>
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                        </ext:ColumnLayout>
                    </Body>
                </ext:Panel>
                <ext:Panel ID="Panel18" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                    BodyStyle="background-color:Transparent;">
                    <Body>
                        <ext:ColumnLayout ID="ColumnLayout7" runat="server">
                            <ext:LayoutColumn ColumnWidth=".5">
                                <ext:Panel ID="Panel21" runat="server" Border="false" Header="false" StyleSpec="background-color:Transparent;"
                                    BodyStyle="background-color:Transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout16" runat="server" LabelWidth="78">
                                            <ext:Anchor>
                                                <ext:TextField ID="txtMedicardmark" runat="server" Width="280" FieldLabel="医疗卡账号"
                                                    MaxLength="30" />
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth=".5">
                                <ext:Panel ID="Panel22" runat="server" Border="false" Header="false" StyleSpec="background-color:Transparent;"
                                    BodyStyle="background-color:Transparent;">
                                    <Body>
                                        <ext:FormLayout ID="FormLayout17" runat="server" LabelWidth="78">
                                            <ext:Anchor>
                                                <ext:TextField ID="txtMediCard" runat="server" Width="280" FieldLabel="医疗卡号" />
                                            </ext:Anchor>
                                        </ext:FormLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                        </ext:ColumnLayout>
                    </Body>
                </ext:Panel>
            </Body>
        </ext:FieldSet>
    </body>
    <bottombar>
        <ext:Toolbar ID="Toolbar2" runat="server">
            <Items>
                <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="Disk" Text="保存">
                    <AjaxEvents>
                        <Click OnEvent="SaveInfo">
                        </Click>
                    </AjaxEvents>
                </ext:ToolbarButton>
                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                <ext:ToolbarButton ID="ToolbarButton1" runat="server" Icon="Disk" Text="提交">
                    <AjaxEvents>
                        <Click OnEvent="SaveInfo">
                        </Click>
                    </AjaxEvents>
                </ext:ToolbarButton>
                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="退出">
                    <Listeners>
                        <Click Handler="parent.staffinfodetail.hide();" />
                    </Listeners>
                </ext:ToolbarButton>
            </Items>
        </ext:Toolbar>
        </bottombar>
    </form>
</body>
</html>
