<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffInfo.aspx.cs" Inherits="GoldNet.JXKP.jxkh.StaffInfo" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>职员详细信息</title>
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" MonitorValid="true"
            ButtonAlign="Right" BodyStyle="background-color:transparent;">
            <Body>
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
                                                <ext:TextField ID="txtDeptInput" runat="server" FieldLabel="科室" LabelStyle="color:blue;"
                                                    ReadOnly="true" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:TextField ID="txtStaffInput" runat="server" FieldLabel="姓名" LabelStyle="color:blue;"
                                                    ReadOnly="true" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:DateField ID="dtfBirthday" runat="server" FieldLabel="出生年月" LabelStyle="color:blue;"
                                                    Format="yyyy-MM-dd" CausesValidation="true" AllowBlank="false" ReadOnly="true">
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
                                                <ext:ComboBox ID="cboDuty" runat="server" FieldLabel="行政职务" Editable="false">
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
                                                <ext:ComboBox ID="cboRank" runat="server" FieldLabel="级别" LabelStyle="color:blue;"
                                                    CausesValidation="true" AllowBlank="false" Editable="false">
                                                </ext:ComboBox>
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:DateField ID="dtfDutydate" runat="server" FieldLabel="行政职务时间" LabelStyle="color:blue;"
                                                    Format="yyyy-MM-dd" CausesValidation="true" AllowBlank="false" ReadOnly="true">
                                                </ext:DateField>
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboPeople" runat="server" FieldLabel="民族" LabelStyle="color:blue;"
                                                    CausesValidation="true" AllowBlank="false" Editable="false">
                                                </ext:ComboBox>
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboIsOnGuard" runat="server" FieldLabel="在岗否" Editable="false">
                                                </ext:ComboBox>
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboStation" runat="server" FieldLabel="岗位名称" DisplayField="STATION_NAME"
                                                    ValueField="ID" Editable="false" />
                                            </ext:Anchor>
                                            <ext:Anchor Horizontal="92%">
                                                <ext:ComboBox ID="cboPerssort" runat="server" FieldLabel="人员类别" Editable="false"
                                                    CausesValidation="true" AllowBlank="false" />
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
                                                        <ext:TextField ID="txtRetainTerm" runat="server" FieldLabel="受聘期限" ReadOnly="true" />
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
                                                        <ext:DateField ID="dtfJobDate" runat="server" FieldLabel="技术职务时间" Format="yyyy-MM-dd"
                                                            ReadOnly="true">
                                                        </ext:DateField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:DateField ID="dtfStudyOverdate" runat="server" FieldLabel="毕业时间" Format="yyyy-MM-dd"
                                                            ReadOnly="true">
                                                        </ext:DateField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:DateField ID="dtfWorkDate" runat="server" FieldLabel="工作时间" Format="yyyy-MM-dd"
                                                            ReadOnly="true">
                                                        </ext:DateField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtGraduateAcademy" runat="server" FieldLabel="毕业院校" ReadOnly="true" />
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
                                                        <ext:DateField ID="dtfTitleAssess" runat="server" FieldLabel="资格评定时间" Format="yyyy-MM-dd"
                                                            ReadOnly="true">
                                                        </ext:DateField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:DateField ID="dtfInHospitalDate" runat="server" FieldLabel="来院时间" Format="yyyy-MM-dd"
                                                            ReadOnly="true">
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
                                                        <ext:DateField ID="dtfGradetitleDate" runat="server" FieldLabel="取得学历时间" Format="yyyy-MM-dd"
                                                            ReadOnly="true">
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
                                                            ReadOnly="true" MaxLength="30" />
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
                                                        <ext:TextField ID="txtHomeplace" runat="server" Width="280" FieldLabel="出生地点" MaxLength="100"
                                                            ReadOnly="true" />
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
                                        <ext:TextArea ID="txtMemo" runat="server" Width="660" FieldLabel="备 注" MaxLength="300"
                                            ReadOnly="true">
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
                                                        <ext:DateField ID="dtfBeEnrolledInDate" runat="server" FieldLabel="入伍时间" Format="yyyy-MM-dd"
                                                            ReadOnly="true">
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
                                                        <ext:DateField ID="dtfCivilServiceClassDate" runat="server" FieldLabel="文职级时间" Format="yyyy-MM-dd"
                                                            ReadOnly="true">
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
                                                        <ext:DateField ID="dtfTechnicClassDate" runat="server" FieldLabel="技术级时间" Format="yyyy-MM-dd"
                                                            ReadOnly="true">
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
                                                            MaxLength="30" ReadOnly="true" />
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
                                                        <ext:TextField ID="txtMediCard" runat="server" Width="280" FieldLabel="医疗卡号" ReadOnly="true" />
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
            </Body>
            <BottomBar>
                <ext:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                        <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                        <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="退出">
                            <Listeners>
                                <Click Handler="parent.StaffInfoWin.hide();" />
                            </Listeners>
                        </ext:ToolbarButton>
                    </Items>
                </ext:Toolbar>
            </BottomBar>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
