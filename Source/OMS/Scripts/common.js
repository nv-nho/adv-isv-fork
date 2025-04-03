/*
* Script on page load
*/
$(function () {

    var sessionId = '#stay';
    var history_api = typeof history.pushState != 'undefined';

    if (history_api) {
        history.pushState(null, '', sessionId);
    } else {
        location.hash = sessionId;
    }
    window.onhashchange = function () {
        // User tried to go back; warn user, rinse and repeat
        if (history_api) {
            history.pushState(null, '', sessionId);
        } else {
            location.hash = sessionId;
        }
    }

    if (typeof OnPageLoad != 'undefined') {
        OnPageLoad();
    }
    $specials_e = eval('(' + $("#specials_e").val() + ')');

    /* bootstrapswitch */    
    $(".switch-tmp").remove();
    /* bootstrapswitch */

    disableIme();
    dateInputSetting();
    timeInputSetting();
    numberInputSetting();
    codeInputSetting();
    inputDisplay();
    registLoading();
    hideLoading();
    ShowModalDefault();
    SetDefaultPostionModal();
    SetContainmentWhenWindowChange();

    
});

///////////////////////////////////////////
//  GET CONTROL BY ID
//  Create  : isv.thuy
//  Date    : 16/07/2014  
///////////////////////////////////////////
function getCtrlById(id, index) {

    /// <summary locid='1'>Get element by id start with</summary>
    /// <param name='id' locid='2'>element id</param>
    /// <param name='index' locid='3'>index (use for data grid)</param>
    if ($.trim(id) === "") {
        return null;
    }
    if (index == undefined) {
        return $("[id$=" + id + "]");
    } else {
        return $("[id$=" + id + "_" + index + "]");
    }
}

///////////////////////////////////////////
//  FOCUS CONTROL ERROR
//  Create  : isv.thuy
//  Date    : 16/07/2014  
///////////////////////////////////////////
function focusErrors() {
    /// <summary locid='1'>Focus to error element</summary>

    var $divs = $(".has-error, .has-warning");
    if ($divs.length > 0) {
        $divs.first().find("input, select, textarea").focus().select();
    }
}

function showLoading() {
    /// <summary locid='1'>Show loading panel</summary>
    $('#loading').modal('show');
}
function hideLoading() {
    /// <summary locid='1'>Hide loading panel</summary>
    $('#loading').modal('hide');
}

function showSuccess() {
    /// <summary locid='1'>Show success panel</summary>
    $("#success").removeClass("hidden");
    $("#success").fadeIn();
}
function hideSuccess() {
    /// <summary locid='1'>Hide success panel</summary>
    $("#success").hide();
}

//
//Get Value of Querry String
//
function getParameter(key) {
    key = key.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + key + "=([^&#]*)"),
    results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

/// Create: ISV_HIEN
/// <summary>
/// TimeToMinute
/// </summary>
/// <param name="pValue"></param>
/// <returns></returns>
function TimeToMinute(pValue) {
    var ary = pValue.split(':');
    var nHours = 0;
    var nMinutes = 0;

    if (ary.length != 2) {
        return 0;
    }

    nHours = parseInt(ary[0]);
    nMinutes = parseInt(ary[1]);
    if (nMinutes < 0) {

        return 0;
    }

    return (nHours * 60 + nMinutes);
}

/// Create: ISV_HIEN
/// <summary>
/// MinuteToTime
/// </summary>
/// <param name="mValue"></param>
/// <returns></returns>
function MinuteToTime(mValue) {
    if (mValue != 0) {
        var sResult = parseInt(mValue / 60).toString().padLeft(2, '0');
        sResult += ":"
        sResult += parseInt(Math.ceil(mValue % 60)).toString().padLeft(2, '0');
    }
    else {
        var sResult = "";
    }
    return sResult;
}

/// Create: ISV_HIEN
/// <summary>
/// IntToTime
/// </summary>
/// <param name="pValue"></param>
/// <param name="pShowZero"></param>
/// <returns></returns>
function IntToTime(pValue, pShowZero) {

    var sResult = "";
    var nHours = 0;
    var nMinutes = 0;


    //ゼロ値表示無効
    if (pValue == 0 && !pShowZero) {
        return "";
    }

    //負数対応
    if (pValue < 0) {
        pValue *= -1;
        sResult = "-";
    }

    //単位変換(600 = 1時間)
    nHours = parseInt(pValue / 600);
    nMinutes = parseInt((pValue % 600) / 10);

    sResult += nHours.toString().padLeft(2, '0');
    sResult += ":";
    sResult += nMinutes.toString().padLeft(2, '0');

    return sResult;
}

/// Create: ISV_HIEN
/// <summary>
/// Set key Enter
/// </summary>
$('body').on('keydown', 'input, select', function (e) {
    var self = $(this)
      , form = self.parents('form:eq(0)')
      , focusable
      , next
      ;
    if (e.keyCode == 13) {
        focusable = form.find('input,a,select,button').filter(':visible');
        next = focusable.eq(focusable.index(this) + 1);
        while (next.is('[readonly]') || (next.is('[disabled=disabled]'))) {
            next = focusable.eq(focusable.index(next) + 1);
        }
        if (next.length) {
            next.focus();
        } else {
            form.submit();
        }
        return false;
    }
});

/// Create: ISV_HIEN
/// <summary>
/// Set keykeypress for button Search
/// </summary>
$(".btnSearchKeypress").keypress(
    function (event) {
        if (event.keyCode == 13) {
            $(this).trigger('click');
        }
    });

///////////////////////////////////////////
//  SHOW MODAL DEFAULT
//  Create  : ISV.Hien
//  Date    : 2014/12/13  
///////////////////////////////////////////
function ShowModalDefault() {
 
    $('.modal').on('shown.bs.modal', function () {
        var _idDraggable = $(this).prop("id");
        var Top1 = $(window).scrollTop();
        var Top2 = ($(window).height() - $("#" + _idDraggable + " .modal-dialog").outerHeight()) + $(window).scrollTop();
        var Left = ($(window).width() - $("#" + _idDraggable + " .modal-dialog").outerWidth()) / 2;

        // set default position modal
        $("#" + _idDraggable + " .modal-dialog").css({ top: Math.max(0, (($(window).height() - $("#" + _idDraggable + " .modal-dialog").outerHeight()) / 2)) + $("#" + _idDraggable + " .modal-dialog").outerHeight() / 2,
            left: 0
        });

        //Draggable modal
        $("#" + _idDraggable + " .modal-dialog").draggable({
            cursor: "move",
            containment: [-Left + 6, Top1, Left + 10, Top2],
            scroll: false,
            cancel: 'a,input,button,select,textarea'
        });
    })
}

///////////////////////////////////////////
//  SET DEFAULT POSITION MODAL
//  Create  : ISV.Hien
//  Date    : 2014/12/13  
///////////////////////////////////////////
function SetDefaultPostionModal() {
    $('.modal').on('hidden.bs.modal', function () {
        var _idDraggable = $(this).prop("id");
        $(".modal-dialog").css({ top: Math.max(0, (($(window).height() - $("#" + _idDraggable + " .modal-dialog").outerHeight()) / 2)) + $("#" + _idDraggable + " .modal-dialog").outerHeight() / 2,
            left: 0
        });
    });
}

///////////////////////////////////////////
//  SET CONTAINMENT WHEN WINDOWCHANGE
//  Create  : ISV.Hien
//  Date    : 2014/12/13  
///////////////////////////////////////////
function SetContainmentWhenWindowChange() {
    $(".modal-dialog").mousemove(function (event) {
        var _idModal = $(this).parents().prop("id");
        var Top1 = $(window).scrollTop();
        var Top2 = ($(window).height() - $("#" + _idModal + " .modal-dialog").outerHeight()) + $(window).scrollTop();
        var Left = ($(window).width() - $("#" + _idModal + " .modal-dialog").outerWidth()) / 2;

        //Draggable modal
        $("#" + _idModal + " .modal-dialog").draggable({
            cursor: "move",
            containment: [-Left + 6, Top1, Left + 10, Top2],
            scroll: false
        });
    });
}

function GetMesssageHtml(arrError) {
    let msgContent = []; 
    msgContent.push('<div id="panelError" class="alert alert-danger alert-dismissible" role="alert">');
    msgContent.push('<button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only"></span></button>');
    msgContent.push('<span class="glyphicon glyphicon-remove-sign"></span><strong> 警告!</strong>');
    $.each(arrError, function( index, value ) {
        msgContent.push('<h5>' + value + "</h5>");
    });
    msgContent.push('</div>');
    return msgContent.join(" ");
}