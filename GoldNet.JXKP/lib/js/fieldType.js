
//是否停用类型
var isstopData = [{ value: '1', text: '停用' }, { value: '0', text: '未停用'}];
//类型渲染器
function isstopRender(r, i, value) {
    for (var i = 0, l = isstopData.length; i < l; i++) {
        var o = isstopData[i];
        if (o.value == value) return o.text;
    }
    return "未停用";
}
//是否末级类型
var islastData = [{ value: '0', text: '非末级' }, { value: '1', text: '末级' }];
//类型渲染器
function islastRender(r, i, value) {
    for (var i = 0, l = islastData.length; i < l; i++) {
        var o = islastData[i];
        if (o.value == value) return o.text;
    }
    return "非末级";
}
//职能科室类型
var isfuncData = [{ value: '1', text: '是' }, { value: '0', text: '否'}];
//类型渲染器
function isfuncRender(r, i, value) {
    for (var i = 0, l = isfuncData.length; i < l; i++) {
        var o = isfuncData[i];
        if (o.value == value) return o.text;
    }
    return "否";
}
//预算分类 科目类别
var subjtypeData = [{ value: '1', text: '支出' }, { value: '0', text: '收入'}];
//类型渲染器
function subjtypeRender(r, i, value) {
    for (var i = 0, l = subjtypeData.length; i < l; i++) {
        var o = subjtypeData[i];
        if (o.value == value) return o.text;
    }
    return "";
}
//是否代编
var isspecialData = [{ value: '1', text: '是' }, { value: '0', text: '否'}];
//类型渲染器
function isspecialRender(r, i, value) {
    for (var i = 0, l = isspecialData.length; i < l; i++) {
        var o = isspecialData[i];
        if (o.value == value) return o.text;
    }
    return "否";
}
//预算类型
var budgtypeData = [{ value: '1', text: '月度' }, { value: '0', text: '年度'}];
//类型渲染器
function budgtypeRender(r, i, value) {
    for (var i = 0, l = budgtypeData.length; i < l; i++) {
        var o = budgtypeData[i];
        if (o.value == value) return o.text;
    }
    return "";
}
//预算年度状态
var yearstateData = [{ value: '1', text: '启用' }, { value: '2', text: '未启用' }, { value: '3', text: '封存'}];
//类型渲染器
function yearstateRender(r, i, value) {
    for (var i = 0, l = yearstateData.length; i < l; i++) {
        var o = yearstateData[i];
        if (o.value == value) return o.text;
    }
    return "未启用";
}
//版本类型
var vertypeData = [{ value: '1', text: '测试' }, { value: '2', text: '正式' }];
//类型渲染器
function vertypeRender(r, i, value) {
    for (var i = 0, l = vertypeData.length; i < l; i++) {
        var o = vertypeData[i];
        if (o.value == value) return o.text;
    }
    return "测试";
}
//版本类型
var verstateData = [{ value: '1', text: '新建' }, { value: '2', text: '下发' }, { value: '3', text: '关闭' }, { value: '4', text: '执行'}];
//类型渲染器
function verstateRender(r, i, value) {
    for (var i = 0, l = verstateData.length; i < l; i++) {
        var o = verstateData[i];
        if (o.value == value) return o.text;
    }
    return "";
}
//审批状态
var issubmitData = [{ value: '1', text: '新建' }, { value: '2', text: '提交' }, { value: '3', text: '退回' }, { value: '4', text: '审批'}];
//状态渲染器
function issubmitRender(r, i, value) {
    for (var i = 0, l = issubmitData.length; i < l; i++) {
        var o = issubmitData[i];
        if (o.value == value) return o.text
    }
    return "";
}
//变更类型
var changetypeData = [{ value: '1', text: '年初预算' }, { value: '2', text: '调整预算' }];
//类型渲染器
function changetypeRender(r, i, value) {
    for (var i = 0, l = changetypeData.length; i < l; i++) {
        var o = changetypeData[i];
        if (o.value == value) return o.text;
    }
    return "";
}

//月份类型
var monthsData = [{ value: '1', text: '一月' }, { value: '2', text: '二月' }, { value: '3', text: '三月' }, { value: '4', text: '四月' },
{ value: '5', text: '五月' }, { value: '6', text: '六月' }, { value: '7', text: '七月' }, { value: '8', text: '八月' },
{ value: '9', text: '九月' }, { value: '10', text: '十月' }, { value: '11', text: '十一月' }, { value: '12', text: '十二月' } ];
//类型渲染器
function monthsRender(r, i, value) {
    for (var i = 0, l = monthsData.length; i < l; i++) {
        var o = monthsData[i];
        if (o.value == value) return o.text;
    }
    return "";
}
//审核状态
var hkchkstatusData = [{ value: '0', text: '未审核' }, { value: '1', text: '审核' }];
//状态渲染器
function hkchkstatusRender(r, i, value) {
    for (var i = 0, l = hkchkstatusData.length; i < l; i++) {
        var o = hkchkstatusData[i];
        if (o.value == value) return o.text;
    }
    return "";
}


