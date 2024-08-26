
var filterString = function(value, dataIndex, record) {
    var val = record.get(dataIndex);
    if (typeof val != "string") {
        return value.length == 0;
    }
    return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
};
var filterNumber = function(value, dataIndex, record) {
    var val = record.get(dataIndex);
    var v = value;
    if (v.indexOf("<=") > -1) {
        var vv = v.substring(v.indexOf("<=") + 2, v.length);
        if (val <= vv) {
            return true;
        }
        else { return false; }
    }
    if (v.indexOf(">=") > -1) {
        var vv = v.substring(v.indexOf(">=") + 2, v.length);
        if (val >= vv) {
            return true;
        }
        else { return false; }
    }
    if (v.indexOf(">") > -1) {
        var vv = v.substring(v.indexOf(">") + 1, v.length);
        if (val > vv) {
            return true;
        }
        else { return false; }
    }
    if (v.indexOf("<") > -1) {
        var vv = v.substring(v.indexOf("<") + 1, v.length);
        if (val < vv) {
            return true;
        }
        else { return false; }
    }
    if (v.indexOf("=") > -1) {
        var vv = v.substring(v.indexOf("=") + 1, v.length);
        if (val == vv) {
            return true;
        }
        else { return false; }
    }
    return false;
};

var filterDate = function(value, dataIndex, record) {
    var val = record.get(dataIndex);
    var v = value;
    if (v.indexOf("<=") > -1) {
        var vv = v.substring(v.indexOf("<=") + 2, v.length);
        if (!IsDate(vv, "-")) {
            return false;
        }
        dVal = new Date(val);
        dVV = new Date(vv);
        if (val <= vv) {
            return true;
        }
        else {
            return false;
        }
    }
    if (v.indexOf(">=") > -1) {
        var vv = v.substring(v.indexOf(">=") + 2, v.length);
        if (!IsDate(vv, "-")) {
            return false;
        }
        dVal = new Date(val);
        dVV = new Date(vv);
        if (val >= vv) {
            return true;
        }
        else {
            return false;
        }
    }
    if (v.indexOf(">") > -1) {
        var vv = v.substring(v.indexOf(">") + 1, v.length);
        if (!IsDate(vv, "-")) {
            return false;
        }
        dVal = new Date(val);
        dVV = new Date(vv);
        if (val > vv) {
            return true;
        }
        else {
            return false;
        }
    }
    if (v.indexOf("<") > -1) {
        var vv = v.substring(v.indexOf("<") + 1, v.length);
        if (!IsDate(vv, "-")) {
            return false;
        }
        dVal = new Date(val);
        dVV = new Date(vv);
        if (val < vv) {
            return true;
        }
    }
    if (v.indexOf("=") > -1) {
        var vv = v.substring(v.indexOf("=") + 1, v.length);
        if (!IsDate(vv, "-")) {
            return false;
        }
        dVal = new Date(val);
        dVV = new Date(vv);
        if (val == vv) {
            return true;
        }
        else {
            return false;
        }
    }
    return false;
};
function IsDate(DateString, Dilimeter) {
    if (DateString == null) return false;
    if (Dilimeter == '' || Dilimeter == null) Dilimeter = '-';
    var tempy = '';
    var tempm = '';
    var tempd = '';
    var tempArray;
    if (DateString.length < 8 || DateString.length > 10) return false;
    tempArray = DateString.split(Dilimeter);
    if (tempArray.length != 3) return false;
    tempy = tempArray[0];
    tempd = tempArray[2];
    tempm = tempArray[1];
    var tDateString = tempy + '/' + tempm + '/' + tempd;
    var tempDate = new Date(tDateString);
    if (isNaN(tempDate)) return false;

    if (((tempDate.getYear()).toString() == tempy) && (tempDate.getMonth() == myparseInt(tempm) - 1) && (tempDate.getDate() == myparseInt(tempd))) {
        return true;
    }
    else {
        return false;
    }
}
function myparseInt(num) {
    var tempnum = num + "";
    while (tempnum.substr(0, 1) == "0") {
        tempnum = tempnum.substr(1);
    }
    return (parseInt(tempnum));
}
var checkIsDate = function(value) {   
    var v = value;
    if (v.indexOf("<=") > -1) {
        var vv = v.substring(v.indexOf("<=") + 2, v.length);
        if (IsDate(vv, "-")) {
            return true;
        }      
    }
    if (v.indexOf(">=") > -1) {
        var vv = v.substring(v.indexOf(">=") + 2, v.length);
        if (IsDate(vv, "-")) {
            return true;
        }        
    }
    if (v.indexOf(">") > -1) {
        var vv = v.substring(v.indexOf(">") + 1, v.length);
        if (IsDate(vv, "-")) {
            return true;
        }       
    }
    if (v.indexOf("<") > -1) {
        var vv = v.substring(v.indexOf("<") + 1, v.length);
        if (IsDate(vv, "-")) {
            return true;
        }       
    }
    if (v.indexOf("=") > -1) {
        var vv = v.substring(v.indexOf("=") + 1, v.length);
        if (IsDate(vv, "-")) {
            return true;
        }       
    }
    return false;
};
