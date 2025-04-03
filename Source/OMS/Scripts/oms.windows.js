/****************************************
* Call Group User Search form
*****************************************/
function showSearchGroupUser(groupCd, groupNm, groupCdCtrlId, groupNmCtrlId) {
    var url = "../Search/FrmGroupSearch.aspx?GroupCD=" + groupCd + "&GroupCDCtrl=" + groupCdCtrlId + "&GroupNmCtrl=" + groupNmCtrlId;
    openWindow(url, "FrmGroupSearch", 800, 600);
}

/****************************************
* Call Project Search form
*****************************************/
function showSearchProject(projectCd, projectNm, initDate, projectCdCtrlId, projectNmCtrlId) {
    var url = "../Search/FrmProjectSearch.aspx?ProjectCD=" + projectCd + "&initDate=" + initDate + "&ProjectCDCtrl=" + projectCdCtrlId + "&ProjectNmCtrl=" + projectNmCtrlId;
    openWindow(url, "FrmProjectSearch", 800, 600);
}

/****************************************
* Call Department Search form
*****************************************/
function showSearchDepartment(departmentCd, departmentNm, departmentCdCtrlId, departmentNmCtrlId) {
    var url = "../Search/FrmDepartmentSearch.aspx?DepartmentCD=" + departmentCd + "&DepartmentCDCtrl=" + departmentCdCtrlId + "&DepartmentNmCtrl=" + departmentNmCtrlId;
    openWindow(url, "FrmDepartmentSearch", 800, 600);
}

/****************************************
* Call User Search form
*****************************************/
function showSearchUser(in1, in2, out1, out2) {
    var url = "../Search/FrmUserSearch.aspx?in1=" + in1 + "&out1=" + out1 + "&out2=" + out2;
    openWindow(url, "FrmUserSearch", 800, 600);
}

var $opener;
var $iopn = false;

/**
* windowのポップアップ
*/
function openWindow(url, name, width, height) {

    /// <summary locid='1'>windowのポップアップ</summary>
    /// <param name='url' locid='2'>URL</param>
    /// <param name='name' locid='3'>Window Name</param>
    /// <param name='width' locid='4'>Width</param>
    /// <param name='height' locid='5'>Height</param>

    var windowFeatures = "";
    //windowFeatures = "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no";
    windowFeatures = "menubar=no, titlebar=no, toolbar=no,scrollbars=yes, resizable=no, left=" + (screen.availWidth - width) / 2 + ",top=" + (screen.availHeight - height) / 2 + ", width=" + width + ", height=" + height + ", directories=no,location=no";
    if (!$iopn) {
        $iopn = true;

        $opener = window.open(url, name, windowFeatures);
        isOpenerClosed();
    }
}

var timeOutStack = [0, 0];
/**
* Check Window.opener is closing
*/
function isOpenerClosed() {
    if ($opener.closed) {
        $iopn = false;
        $.each(timeOutStack, function (i, id) {
            clearTimeout(id);
        });
        hideLoading();
    } else {
        var id = window.setTimeout("isOpenerClosed()", 500);
        if (id % 2 == 0) {
            timeOutStack[1] = id;
        } else {
            timeOutStack[0] = id;
        }
    }
    //console.log(new Date());
}
