//<![CDATA[
//Detect Device
var deviceAgent = window.navigator.userAgent.toLowerCase();

var __isTouchDevice = (deviceAgent.match(/(iphone|ipod|ipad)/) ||
    deviceAgent.match(/(android)/) ||
    deviceAgent.match(/(iemobile)/) ||
    deviceAgent.match(/iphone/i) ||
    deviceAgent.match(/ipad/i) ||
    deviceAgent.match(/ipod/i) ||
    deviceAgent.match(/blackberry/i) ||
    deviceAgent.match(/bada/i));

var __msie = false;
var __ff = false;
var __chrome = false;
var __fcId = "";

//Javascript Browser Detection - Internet Explorer
if (/msie (\d+\.\d+);/.test(deviceAgent) ||
    /trident\/(\d+\.\d+);/.test(deviceAgent)) //*test for MSIE x.x; True or False*//
{
    __msie = true;
}

//Javascript Browser Detection - FireFox
if (/firefox[\/\s](\d+\.\d+)/.test(deviceAgent)) //*test for Firefox/x.x or Firefox x.x*//
{
    __ff = true;
}

//Javascript Browser Detection - Chrome
if (deviceAgent.lastIndexOf('chrome/') > 0) {
    __chrome = true;
}

//Javascript Browser Detection - Mobile
if (__isTouchDevice) {

    // Check if the orientation has changed 90 degrees or -90 degrees... or 0
    window.addEventListener("orientationchange", function () {
        //alert(window.orientation);
    });
}

var $num = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

var $char = ['q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p',
    'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z',
    'x', 'c', 'v', 'b', 'n', 'm',
    'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P',
    'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z',
    'X', 'C', 'V', 'B', 'N', 'M'];
var $specials_e =
    {
        //    '!': '${33}', //Exclamation mark
        //    '@': '${64}', //At symbol
        //    '#': '${35}', //Number
        //    '$': '${36}', //Dollar
        //    '%': '${37}', //Procenttecken
        //    '^': '${94}', //Caret - circumflex
        //    '&': '${38}', //Ampersand
        //    '*': '${42}', //Asterisk
        //    '(': '${40}', //Open parenthesis 
        //    ')': '${41}' //Close parenthesis
    };

String.prototype.encode = function () {
    var length = this.length;
    var newStr = '';
    for (var i = 0; i < length; i++) {
        var spc = $specials_e[this[i]];
        if (typeof (spc) === 'undefined') {
            spc = this[i];
        }
        newStr += spc;
    }
    return newStr;
};
/**
* Check Attribute has exits
*   if($('.sample').hasAttr('id')){
*
*   }
************************************
*/
jQuery.fn.extend({
    hasAttr: function (name) {
        "use strict";
        return this.attr(name) !== 'undefined';
    },
    selectVal: function (value) {
        "use strict";
        if ($(this).is("select")) {
            $(this).val(value);
            $(this).find('option').each(function (i, select) {
                if ($(this).val() === value) {
                    $(this).attr('selected', 'selected');
                } else {
                    $(this).removeAttr('selected');
                }
            });
            return this;
        } else {
            throw new Error("object must be object input select ");
        }
    },
    valuechange: function () {
        var onvaluechange = $(this).attr("onvaluechange");
        if ($.trim(onvaluechange) != "") {
            eval(onvaluechange + "(this)");
        }
    }
});


var trim = (function () {
    function escapeRegex(string) {
        return string.replace(/[\[\](){}?*+\^$\\.|\-]/g, "\\$&");
    }
    "use strict";
    return function trim(str, characters, flags) {
        flags = flags || "g";
        if (typeof str !== "string" || typeof characters !== "string" || typeof flags !== "string") {
            throw new TypeError("argument must be string");
        }

        if (!/^[gi]*$/.test(flags)) {
            throw new TypeError("Invalid flags supplied '" + flags.match(new RegExp("[^gi]*")) + "'");
        }

        characters = escapeRegex(characters);

        return str.replace(new RegExp("^[" + characters + "]+|[" + characters + "]+$", flags), '');
    };
}());


/**
* htmlのエスケープを戻す
* var text ="&lt;table&gt;TEXT&lt;/table&gt;";
* text = text.convertHtml();
************************************
* result => text = "<table>TEXT</table>"
*/
String.prototype.convertHtml = function () {
    "use strict";
    return this.replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
}

/**
* htmlのエスケープを戻す
* var text ="<table>TEXT</table>";
* text = text.deconvertHtml();
************************************
* result => text = "&lt;table&gt;TEXT&lt;/table&gt;"
*/
String.prototype.deconvertHtml = function () {
    "use strict";

    /// <summary locid='1'>htmlのエスケープを戻す</summary>
    return this.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&quot;/g, '"').replace(/&amp;/g, '&');
};


/**
* Padleft
* var padStr ="1";
* padStr = padStr.padLeft(5, '0');
************************************
* result => padStr = "00005"
*/
String.prototype.padLeft = function (totalWidth, paddingChar) {

    /// <summary locid='1'>padLeft</summary>
    /// <param name='totalWidth' locid='2'>total width</param>
    /// <param name='paddingChar' locid='3'>padding char</param>

    "use strict";
    return new Array(totalWidth - this.length + 1).join(paddingChar) + this;
};

/**
* PadRight
* var padStr ="1";
* padStr = padStr.padRight(5, '0');
************************************
* result => padStr = "50000"
*/
String.prototype.padRight = function (totalWidth, paddingChar) {

    /// <summary locid='1'>padRight</summary>
    /// <param name='totalWidth' locid='2'>total width</param>
    /// <param name='paddingChar' locid='3'>padding char</param>

    "use strict";
    return this + new Array(totalWidth - this.length + 1).join(paddingChar);
};

/**
* ReplaceAt
* var text ="1234567";
* text = text.replaceAt(4, 'ABC');
************************************
* result => text = "123ABC567"
*/
String.prototype.replaceAt = function (index, text) {
    "use strict";

    /// <summary locid='1'>ReplaceAt</summary>
    /// <param name='index' locid='2'>Index</param>
    /// <param name='text' locid='3'>Replace char</param>
    return this.slice(0, index) + text + this.slice(index + 1, this.length);
};

/**
* toBool
* var text ="1";
* var b = text.toBool();
************************************
* result => b = "true"
*/
String.prototype.toBool = function () {

    "use strict";
    /// <summary locid='1'>to Bool</summary>

    var $bVal = this.trim();
    if ($bVal === '') {
        return false;
    }
    if ($bVal.toLowerCase() === 'true' || $bVal.toLowerCase() === '1') {
        return true;
    }
    return false;
};

/**
* addComma
* var number = 1234567;
* var text = number.addComma();
************************************
* result => text = "1,234,567"
*/
String.prototype.addComma = function () {
    "use strict";
    /// <summary locid='1'>add comma</summary>
    return this.replace(/,/g, '').replace(/\B(?=(\d{3})+(?!\d))/g, ",");
};


/**
* contains
* var str = "To be, or not to be, that is the question.";
* console.log(str.contains("To be"));       // true
* console.log(str.contains("question"));    // true
* console.log(str.contains("nonexistent")); // false
* console.log(str.contains("To be", 1));    // false
* console.log(str.contains("TO BE"));       // false
************************************
*/
if (!String.prototype.contains) {
    String.prototype.contains = function () {
        "use strict";
        /// <summary locid='1'>contains</summary>
        return String.prototype.indexOf.apply(this, arguments) !== -1;
    };
}

//*********************************************************************************

$("textarea,input, button, select, checkbox, radio").focus(selectActiveItem);
function selectActiveItem() {
    if (document.activeElement == null) {
        return;
    }
    __fcId = document.activeElement.id;
    if ($.trim(__fcId) !== '') {
        var $atvElm = getCtrlById(__fcId);
        if ($atvElm.is(':disabled, [readonly]')) {
            return;
        }
        if ($atvElm.is("input") || $atvElm.is("textarea")) {
            var type = $atvElm.attr("type");
            if (type == 'text' || $atvElm.is("textarea")) {
                $atvElm.select();
            }
        }
    }
}

/**
*** Is readonly not allow focus
*** Author: ISV-PHUONG
*** Date  : 2014/07/28
********************************/
function inputDisplay() {
    "use strict";
    $("textarea[readonly='readonly'],input[type='text'][readonly='readonly'],textarea[readonly='true'],input[type='text'][readonly='true']").each(function () {
        $(this).attr("tabindex", -1);
    });
}

/**
*** Disable IME
*** Author: ISV-PHUONG
*** Date  : 2014/07/28
********************************/
function disableIme() {
    $(".disable-ime").keydown(function (e) {
        var a = [e.keyCode || e.which];
        // Remove ime-mode
        if (a === 229) {
            e.preventDefault ? e.preventDefault() : e.returnValue = false;
            return false;
        }
    }).css("imeMode", "disabled");
}

/**
*** Date Input
*** Author: ISV-PHUONG
*** Date  : 2014/07/28
********************************/
function dateInputSetting() {
    //console.log($(".input-date:not(:disabled):not([readonly])"));
    $(".input-date:not(:disabled):not([readonly])").each(function (idx, input) {

        var _pickTime = $(input).attr('pick-time').toBool();
        var _pickDate = $(input).attr('pick-date').toBool();
        var _pickFormat = $(input).attr('pick-format').toString();

        var alw = "";
        var fmt = _pickFormat;
        if (_pickDate) {
            alw = '/';
            if (_pickTime) {
                alw = '/, ,:';
            }
        } else {
            alw = ':';
        }

        $(input).addClass('input-code')
            .attr("code-type", 1)
            .attr("allow-char", alw)
            .change(function () {                
                var d = this.value.trim();
                if (!isDate(d, _pickDate, _pickTime, _pickFormat)) {
                    $(this).val('');
                } else {
                    var d = d.replace(/\//g, '').replace(/:/g, '');
                    var tmpFmt = fmt.replace(/\//g, '').replace(/:/g, '').toUpperCase();
                    var dt = d.split(' ');

                    $(this).val(iGetDate(tmpFmt, dt));
                }

                if (typeof dateOnChange !== 'undefined' && $.isFunction(dateOnChange)) {
                    dateOnChange();
                }
            }).parents(".date").datetimepicker({
                format: fmt,
                locale: 'en'

            }).on('dp.change', function (e) {
                $(e.target).children('.input-date').trigger('change');
            });

    });
}

function iGetDate(_format, datetime) {
    var day = 0;
    var month = 0;
    var year = 0;
    var hour = 0;
    var minute = 0;
    var seconds = 0;

    if (_format.toUpperCase() == "DDMMYYYY HHMMSS") {
        day = datetime[0].substring(0, 2);
        month = datetime[0].substring(2, 4);
        year = datetime[0].substring(4, 8);

        hour = datetime[1].substring(0, 2);
        minute = datetime[1].substring(2, 4);
        seconds = datetime[1].substring(4, 6);

        return day + "/" + month + '/' + year.padLeft(3, "20") + " " + hour + ":" + minute + ":" + seconds;
    }

    if (_format.toUpperCase() == "DDMMYYYY HHMM") {
        day = datetime[0].substring(0, 2);
        month = datetime[0].substring(2, 4);
        year = datetime[0].substring(4, 8);

        hour = datetime[1].substring(0, 2);
        minute = datetime[1].substring(2, 4);

        return day + "/" + month + '/' + year.padLeft(3, "20") + " " + hour + ":" + minute;
    }

    if (_format.toUpperCase() == "DDMMYYYY") {
        day = datetime[0].substring(0, 2);
        month = datetime[0].substring(2, 4);
        year = datetime[0].substring(4, 8);

        return day + "/" + month + '/' + year.padLeft(3, "20");
    }

    if (_format.toUpperCase() == "MMYYYY") {
        month = datetime[0].substring(0, 2);
        year = datetime[0].substring(2, 6);

        return month + '/' + year.padLeft(3, "20");
    }

    if (_format.toUpperCase() == "YYYYMMDD HHMMSS") {
        year = datetime[0].substring(0, 4);
        month = datetime[0].substring(4, 6);
        day = datetime[0].substring(6, 8);

        hour = datetime[1].substring(0, 2);
        minute = datetime[1].substring(2, 4);
        seconds = datetime[1].substring(4, 6);

        return year.padLeft(3, "20") + "/" + month + '/' + day + " " + hour + ":" + minute + ":" + seconds;
    }

    if (_format.toUpperCase() == "YYYYMMDD HHMM") {
        year = datetime[0].substring(0, 4);
        month = datetime[0].substring(4, 6);
        day = datetime[0].substring(6, 8);

        hour = datetime[1].substring(0, 2);
        minute = datetime[1].substring(2, 4);

        return year.padLeft(3, "20") + "/" + month + '/' + day + " " + hour + ":" + minute;
    }

    if (_format.toUpperCase() == "YYYYMMDD") {
        year = datetime[0].substring(0, 4);
        month = datetime[0].substring(4, 6);
        day = datetime[0].substring(6, 8);

        return year.padLeft(3, "20") + "/" + month + '/' + day;
    }

    if (_format.toUpperCase() == "YYYYMM") {
        year = datetime[0].substring(0, 4);
        month = datetime[0].substring(4, 6);

        return year.padLeft(3, "20") + "/" + month;
    }

    if (_format.toUpperCase() == "YYYY") {
        return datetime[0].padLeft(3, "20");
    }
}

/**
*** Time Input
*** Author: 
*** Date  : 
********************************/
function timeInputSetting() {
    $(".input-time:not(:disabled):not([readonly])").each(function (idx, input) {

        var alw = ':';
        //var fmt = 'HH:mm';

        $(input).addClass('input-code')
            .attr("code-type", 1)
            .attr("allow-char", alw)
            .change(function () {
                var d = this.value.trim();
                d = WorkTimeFormatCommon(d);
                if (!isTime(d)) {
                    $(this).val('');
                } else {
                    d = WorkTimeFormatCommon(d);
                    $(this).val(d);
                }
            });

    });
}

/**
*** Code Input
*** Author: ISV-PHUONG
*** Date  : 2014/07/28
********************************/
function codeInputSetting() {

    var _icged = false;

    var _ibspc = false;
    var _iinst = false;
    var _ihm = false;
    var _ipu = false;
    var _ipd = false;
    var _ien = false;
    var _idlt = false;

    var _ilft = false;
    var _isUp = false;
    var _idwn = false;
    var _irgt = false;

    var _idt = false;
    var _iqte = false;
    $(".input-code").focus(function () {
        //this.select();
        _icged = false;
        $(this).attr("old-val", this.value);
    }).keydown(function (e) {
        _ibspc = false;
        _iinst = false;
        _ihm = false;
        _ipu = false;
        _ipd = false;
        _ien = false;
        _idlt = false;

        _ilft = false;
        _isUp = false;
        _idwn = false;
        _irgt = false;

        _idt = false;
        _iqte = false;

        var a = [e.keyCode || e.which];

        if (a == 8) {
            _ibspc = true;
        }
        if (a == 45) {
            _iinst = true;
        }
        if (a == 36) {
            _ihm = true;
        }
        if (a == 33) {
            _ipu = true;
        }
        if (a == 34) {
            _ipd = true;
        }
        if (a == 35) {
            _ien = true;
        }
        if (a == 46) {
            _idlt = true;
        }
        if (a == 37) {
            _ilft = true;
        }
        if (a == 38) {
            _isUp = true;
        }
        if (a == 39) {
            _irgt = true;
        }
        if (a == 40) {
            _idwn = true;
        }
        if (a == 110 || a == 190) {
            _idt = true;
        }
        if (a == 222) {
            _iqte = true;
        }

        //when escape retore old value
        if (a == 27) {
            $(this).val($(this).attr("old-val"));
            this.select();
            e.preventDefault ? e.preventDefault() : e.returnValue = false;
            return false;
        }
        return true;
    }).keypress(function (e) {
        if ($(this).is(':disabled, [readonly]')) {
            return;
        }
        var a = [e.keyCode || e.which];

        var b = [];
        if ($(this).hasAttr("allow-char")) {
            b = $(this).attr("allow-char").split(",");
        }
        var ct = $(this).attr("code-type");


        var btnSrch = "";
        //if ($(this).hasAttr("search-button")) {
        //    btnSrch = $(this).attr("search-button");
        //}

        var cin = String.fromCharCode(a);
        var alw = $.inArray(cin, b) != -1;

        //------------- SPECIAL KEY -----------------------------------------//
        //remove shift + number
        if (e.shiftKey && (
            a == 33 ||    //!
            a == 35 ||    //#
            a == 36 ||    //$
            (a == 37 && !_ilft) ||    //%
            (a == 38 && !_isUp) ||    //&
            (a == 40 && !_idwn) ||    //(
            a == 41 ||    //)
            a == 42 ||    //*
            a == 64       //@
        ) && (!_ihm && !_ien) && !alw) {
            e.preventDefault ? e.preventDefault() : e.returnValue = false;
            return false;
        }

        // Allow: backspace, delete, tab, escape, and enter
        if (_ibspc || _idlt || a == 9 || a == 13 || a == 27 ||
            // Allow: home, end, left, right
            ((_ihm || _ien || _ilft || _irgt))) {
            // let it happen, don't do anything
            return true;
        }

        //Ctrl + X
        if ((e.ctrlKey === true && a == 120) ||
            //Ctrl + V
            (e.ctrlKey === true && a == 118) ||
            //Ctrl + C
            (e.ctrlKey === true && a == 99)) {
            return true;
        }


        //------------- BEGIN -----------------------------------------//
        if (ct == 1) {
            // Numeric : 0 - 9
            // Ensure numberic input
            if ((a < 48 || a > 57) &&       // 0 - 9
                !alw) {
                e.preventDefault ? e.preventDefault() : e.returnValue = false;
                if ($.trim(btnSrch) != "" && a == 32) {
                    $(this).change().ready(function () {
                        window.setTimeout(function () {
                            $(getCtrlById(btnSrch)).trigger("click");
                        }, 200);
                    });
                }
                return false;
            }

        } else if (ct == 2) {

            // AlphaNumeric :  A - Z, 0 - 9
            if ((a < 48 || a > 57) &&       // 0 - 9
                ((a < 97 || a > 122) &&     // a - z
                    ((a < 65 || a > 90))) &&   // A - Z
                !alw) {
                e.preventDefault ? e.preventDefault() : e.returnValue = false;
                if ($.trim(btnSrch) != "" && a == 32) {
                    $(this).change().ready(function () {

                        window.setTimeout(function () {
                            $(getCtrlById(btnSrch)).trigger("click");
                        }, 200);
                    });
                }
                return false;
            }

        } else if (ct == 3) {
            // AlphaNumeric :  A - Z, 0 - 9, '/'
            if ((a < 48 || a > 57) &&       // 0 - 9
                ((a < 97 || a > 122) &&     // a - z
                    ((a < 65 || a > 90))) &&   // A - Z
                (a != 47) &&                  // '/'
                !alw) {
                e.preventDefault ? e.preventDefault() : e.returnValue = false;
                if ($.trim(btnSrch) != "" && a == 32) {
                    $(this).change().ready(function () {
                        window.setTimeout(function () {
                            $(getCtrlById(btnSrch)).trigger("click");
                        }, 200);
                    });
                }
                return false;
            }

        } else if (ct == 4) {

            // AlphaNumeric :  A - Z, 0 - 9, / , " "
            if ((a < 48 || a > 57) &&       // 0 - 9
                ((a < 97 || a > 122) &&     // a - z
                    ((a < 65 || a > 90))) &&   // A - Z
                (a != 47) &&                  // '/'
                (a != 32) &&                  // " "
                !alw) {
                e.preventDefault ? e.preventDefault() : e.returnValue = false;
                return false;
            } else {
                this.value += " ";
                if ($.trim(btnSrch) != "" && a == 32) {
                    $(this).change().ready(function () {
                        window.setTimeout(function () {
                            $(getCtrlById(btnSrch)).trigger("click");
                        }, 200);
                    });
                }
            }
        } else if (ct == 5) {

            // AlphaNumeric :  A - Z
            if (((a < 97 || a > 122) &&     // a - z
                ((a < 65 || a > 90))) &&   // A - Z
                !alw) {

                if ($.trim(btnSrch) != "" && a == 32) {
                    $(this).change().ready(function () {
                        window.setTimeout(function () {
                            $(getCtrlById(btnSrch)).trigger("click");
                        }, 200);
                    });
                }
                e.preventDefault ? e.preventDefault() : e.returnValue = false;
                return false;
            }
        }

        //------------- END -----------------------------------------//
    }).change(function (e) {
        if (_icged) {
            return;
        }
        _icged = true;
        if ($(this).hasClass("upper")) {
            this.value = this.value.toUpperCase();
        }
        var cin = $.trim(this.value);

        var labelIds = $(this).attr("label-name");
        var fillChar = $(this).attr("fill-char");
        var ctrId = $(this).attr("id");

        //N?u co thi?t gia tr? padleft
        if ($.trim(fillChar) != "" && cin != "") {
            var mxL = $(this).attr("maxlength");
            cin = this.value = cin.padLeft(mxL, fillChar);
        }

        // Co thi?t l?p label 
        if ($.trim(labelIds) != "") {

            var labels = labelIds.split(",");
            var callBack = $(this).attr("call-back");
            var sender = this;

            if (cin != "") {
                var ajaxUrl = $(this).attr("ajax-url-method");
                var params = $(this).attr("params");
                var prs = {};
                if ($.trim(params) != "") {
                    $.each(params.split(","), function (i, obj) {
                        var map = obj.split(":");
                        prs[$.trim(map[0])] = getCtrlById($.trim(map[1])).val();
                    });
                } else {
                    prs["in1"] = cin;
                }

                ajax(ajaxUrl, prs, function (response) {
                    if (response.d) {
                        var result = eval('(' + response.d + ')');
                        //set d? li?u cho toan b? abel ???c thi?t l?p
                        $.each(labels, function (index, label) {
                            var map = label.split(":");
                            if (map.length > 1) {
                                getCtrlById($.trim(map[1])).val(result[$.trim(map[0])]);
                            } else {
                                getCtrlById($.trim(map[0])).val(result[$.trim(map[0])]);
                            }
                        });
                    } else {
                        //Xoa text toan b? label, ngoai control chinh
                        $.each(labels, function (index, label) {
                            var map = label.split(":");
                            var labelCtr = null;
                            if (map.length > 1) {
                                labelCtr = getCtrlById($.trim(map[1]));
                            } else {
                                labelCtr = getCtrlById($.trim(map[0]));
                            }
                            if ($(labelCtr).attr("id") != ctrId) {
                                $(labelCtr).val("");
                            }
                        });
                    }

                    if ($.trim(callBack) != "") {
                        eval(callBack + "(sender)");
                    }
                });
            } else {
                //Xoa text toan b? label 
                $.each(labels, function (index, label) {
                    var map = label.split(":");
                    var labelCtr = null;
                    if (map.length > 1) {
                        labelCtr = getCtrlById($.trim(map[1]));
                    } else {
                        labelCtr = getCtrlById($.trim(map[0]));
                    }
                    $(labelCtr).val("");
                });
                if ($.trim(callBack) != "") {
                    eval(callBack + "(this)");
                }
            }
        }
    }).blur(function () {
        if ($(this).attr("old-val") != this.value) {
            var onvaluechange = $(this).attr("onvaluechange");
            if ($.trim(onvaluechange) != "") {
                eval(onvaluechange + "(this)");
            }
            if (!_icged) {
                $(this).trigger("change");
            }
        }
    }).bind('paste', function (e) {
        var clipboardData = window.clipboardData;
        if (typeof (clipboardData) == 'undefined') {
            clipboardData = (e.originalEvent || e).clipboardData;
        }
        //disable paste
        e.preventDefault();
        var pasted = $.trim(clipboardData.getData('text'));
        if (checkCodePasted(pasted, this)) {
            if (this.selectionStart != this.selectionEnd &&
                (this.selectionEnd - this.selectionStart) == this.value.length) {
                this.value = pasted;
            } else {
                var startNum = this.value.slice(0, this.selectionStart);
                var endNum = this.value.slice(this.selectionEnd, this.value.length);
                var newVal = startNum + pasted + endNum;
                if (checkCodePasted(newVal, this)) {
                    this.value = newVal;
                }
            }
        }
    }).bind('drop', function (e) {
        var dropValue = $.trim((e.originalEvent || e).dataTransfer.getData("text"));
        //disable drop
        e.preventDefault();
        if (checkCodePasted(dropValue, this)) {
            this.value = dropValue;
        }
    });
}

/**
*** Number Input
*** Author: ISV-PHUONG
*** Date  : 2014/07/25
********************************/
function numberInputSetting() {
    var changed = false;

    $(".input-num").focus(function () {
        //this.select();
        changed = false;
        __fcId = this.id;
        $(this).attr("old-val", this.value);
    }).keydown(function (e) {
        if ($(this).is(':disabled, [readonly]')) {
            return;
        }
        var a = [e.keyCode || e.which];
        //khong cho nh?p phim space
        if (a == 32) {
            e.preventDefault ? e.preventDefault() : e.returnValue = false;
            return false;
        }

        // Allow: tab or enter
        if (a == 9 || a == 13 ||
            // Allow: home, end, left, right
            (a >= 35 && a <= 39)) {
            // let it happen, don't do anything
            return true;
        }
        //when escape retore old value
        if (a == 27) {
            $(this).val($(this).attr("old-val").addComma());
            this.select();
            e.preventDefault ? e.preventDefault() : e.returnValue = false;
            return false;
        }

        //remove shift + number
        if (e.shiftKey && (a >= 48 && a <= 57)) {
            e.preventDefault ? e.preventDefault() : e.returnValue = false;
            return false;
        }

        //B? qua cac phim control
        if (isControlKey(e))
            return true;

        //Ctrl + X - Ctrl + V
        if ((a == 88 && e.ctrlKey === true) ||
            (a == 86 && e.ctrlKey === true) ||
            (a == 67 && e.ctrlKey === true)) {
            return true;
        }
        var _isneg = (!__ff && a == 189) || (a == 173 || a == 109);
        // Ch? cho nh?p nh?ng day s? n?m trong kho?ng sau
        // input : 0 -> 9
        // input : numpad0 -> numpad9
        // input : "."
        // input : "-"
        // input : "+"
        // input : "delete"
        // input : "backspace"
        if ((a >= 48 && a <= 57) ||   /* 0 -> 9 */
            (a >= 96 && a <= 105) ||  /* numpad0 -> numpad9 */
            (a == 190 || a == 110) || /* . */
            _isneg ||                 /* - */
            (a == 107) ||               /* + */
            (a == 8) ||                 /* backspace */
            (a == 46)                   /* delete */) {

            var id = $(this).attr("id");
            var val = this.value;

            var dg = $(this).attr("decimal-digit");
            var neg = $(this).attr("allow-negative");
            var intmxL = $(this).attr("int-max-length");

            var selectionStart = this.selectionStart;
            //Cho phep nh?p s? decimal
            var idxDec = val.indexOf(".");
            var idxNeg = val.indexOf("-");

            //Khi nh?n backspace
            if (a == 8) {

                if (this.selectionStart != this.selectionEnd &&
                    //select all text: clear all
                    (this.selectionEnd - this.selectionStart) == val.length) {
                    return true;
                }
                if (this.selectionEnd - this.selectionStart != 0) {
                    var selectionEnd = this.selectionEnd;
                    var numStr = val;
                    var selNum = numStr.slice(selectionStart, selectionEnd);
                    var startNum = numStr.slice(0, selectionStart);
                    var endNum = numStr.slice(selectionEnd, numStr.length);
                    var idxDecS = selNum.indexOf('.');
                    var num = trim((startNum + (idxDecS != -1 ? '.' : '') + endNum), '-');

                    if (parseFloat(num.replace(/,/g, '')) == 0) {

                        var decimalNum = ''.padRight(dg, '0');
                        selectionStart = 0;
                        if (dg > 0) {
                            decimalNum = '0.' + decimalNum;
                        } else {
                            decimalNum = '0';
                        }
                        this.value = decimalNum;
                        setSelectionStart(id, selectionStart);
                        e.preventDefault ? e.preventDefault() : e.returnValue = false;
                        return false;
                    } else if (idxDecS != -1) {

                        var commaCount = val.split(',').length - 1;
                        selectionStart = selectionStart - commaCount;
                        selectionEnd = selectionEnd - commaCount;
                        this.value = ((idxNeg != -1 ? '-' : '') + num).addComma();

                        //Them format vao chuoi da nhap
                        commaCount = this.value.split(",").length - 1 + selNum.split(",").length - 1;

                        selectionStart = selectionStart + commaCount;

                        setSelectionStart(id, selectionStart);
                        e.preventDefault ? e.preventDefault() : e.returnValue = false;
                        return false;
                    }

                } else {
                    if ((selectionStart - 1) == idxDec) {
                        if (trim(val, '-,.') !== '') {
                            //Khi con tr? ?ang n?m ? v? tri d?u "."
                            e.preventDefault ? e.preventDefault() : e.returnValue = false;
                            //di chuy?n con tr? ra phia tr??c d?u "."
                            setSelectionStart(id, selectionStart - 1);
                            return false;
                        }
                    }
                    if ((selectionStart - 1) == idxNeg) {
                        if (trim(val, '-,.') !== '') {
                            //Khi con tr? ?ang n?m ? v? tri d?u "-"
                            e.preventDefault ? e.preventDefault() : e.returnValue = false;
                            //di chuy?n con tr? ra phia tr??c d?u "-"
                            setSelectionStart(id, selectionStart - 1);
                            return false;
                        }
                    }
                    var indexOfComma = val.lastIndexOf(",", selectionStart);
                    if ((selectionStart - 1) == indexOfComma) {
                        //Khi con tr? ?ang n?m ? v? tri d?u ","
                        e.preventDefault ? e.preventDefault() : e.returnValue = false;

                        this.value = val.replaceAt((selectionStart - 1) - 1, "").addComma();
                        setSelectionStart(id, selectionStart - 2);
                        return false;
                    }
                }
                return true;
            }

            //Khi nh?n delete
            if (a == 46) {
                if (this.selectionStart != this.selectionEnd &&
                    (this.selectionEnd - this.selectionStart) == val.length) {
                    return true;
                }
                if (this.selectionEnd - this.selectionStart != 0) {

                    var selectionEnd = this.selectionEnd;
                    var numStr = val;
                    var selNum = numStr.slice(selectionStart, selectionEnd);
                    var startNum = numStr.slice(0, selectionStart);
                    var endNum = numStr.slice(selectionEnd, numStr.length);
                    var idxDecS = selNum.indexOf('.');
                    var num = trim((startNum + (idxDecS != -1 ? '.' : '') + endNum), '-');

                    if (parseFloat(num.replace(/,/g, '')) == 0) {

                        var decimalNum = ''.padRight(dg, '0');
                        selectionStart = 0;
                        if (dg > 0) {
                            decimalNum = '0.' + decimalNum;
                        } else {
                            decimalNum = '0';
                        }
                        this.value = decimalNum;
                        setSelectionStart(id, selectionStart);
                        e.preventDefault ? e.preventDefault() : e.returnValue = false;
                        return false;
                    } else if (idxDecS != -1) {
                        this.value = ((idxNeg != -1 ? '-' : '') + num).addComma();
                        setSelectionStart(id, selectionStart + 1);
                        e.preventDefault ? e.preventDefault() : e.returnValue = false;
                        return false;
                    }

                } else {
                    if (selectionStart == idxDec) {
                        if (trim(val, '-,.') !== '') {
                            //Khi con tr? ?ang n?m ? v? tri d?u "."
                            e.preventDefault ? e.preventDefault() : e.returnValue = false;
                            //di chuy?n con tr? ra phia sau d?u "."
                            setSelectionStart(id, selectionStart + 1);
                            return false;
                        }
                    }
                    if (selectionStart == idxNeg) {
                        if (trim(val, '-,.') !== '') {
                            //Khi con tr? ?ang n?m ? v? tri d?u "-"
                            e.preventDefault ? e.preventDefault() : e.returnValue = false;
                            //di chuy?n con tr? ra phia tr??c d?u "-"
                            setSelectionStart(id, selectionStart + 1);
                            return false;
                        }
                    }
                    var indexOfComma = val.indexOf(",", selectionStart);
                    if (selectionStart == indexOfComma) {
                        //Khi con tr? ?ang n?m ? v? tri d?u ","
                        e.preventDefault ? e.preventDefault() : e.returnValue = false;
                        this.value = val.replaceAt(selectionStart + 1, "");
                        setSelectionStart(id, selectionStart + 1);
                        return false;
                    }
                }

                return true;
            }

            // Nhap so AM
            if (neg.toBool()) {

                //khi nh?p d?u '-'
                if (_isneg) {
                    e.preventDefault ? e.preventDefault() : e.returnValue = false;

                    if (this.selectionStart != this.selectionEnd &&
                        (this.selectionEnd - this.selectionStart) == val.length) {

                        $(this).addClass("negative-num");
                        $(this).val("-");

                        setSelectionStart(id, 1);
                        return true;
                    }

                    //d?u tr?
                    if (idxNeg != -1) {

                        //?a nh?p d?u "-" r?i thi chuy?n thanh s? d??ng
                        val = val.replace(/-/g, '');
                        selectionStart -= 1;
                        $(this).removeClass("negative-num");

                    } else {
                        //Them vao d?u "-" 
                        val = "-" + val;
                        selectionStart += 1;
                        $(this).addClass("negative-num");
                    }

                    $(this).val(val);
                    setSelectionStart(id, selectionStart);
                    return true;

                    // Khi nhap dau '+'
                } else if (a == 107) {

                    if (this.selectionStart != this.selectionEnd &&
                        (this.selectionEnd - this.selectionStart) == val.length) {

                        $(this).removeClass("negative-num");
                        $(this).val("");
                        e.preventDefault ? e.preventDefault() : e.returnValue = false;
                        return false;
                    }
                    e.preventDefault ? e.preventDefault() : e.returnValue = false;

                    //d?u c?ng
                    if (idxNeg != -1) {
                        //n?u ?ang la s? am thi chuy?n thanh s? d??ng
                        val = val.replace(/-/g, '');
                        selectionStart -= 1;
                        $(this).removeClass("negative-num");

                        this.value = val;
                        setSelectionStart(id, selectionStart);
                    }
                    return false;
                }

            } else {
                // "-" || "+"
                if (_isneg || a == 107) {
                    //Khong cho nh?p s? am
                    e.preventDefault ? e.preventDefault() : e.returnValue = false;
                    return false;
                }
            }

            // Nhap so thap phan
            if (dg > 0) {

                if (a == 190 || a == 110) {

                    if (idxDec != -1) {
                        // ?a nh?p d?u "."  r?i thi ko ???c nh?p n?a
                        e.preventDefault ? e.preventDefault() : e.returnValue = false;

                        //chuy?n v? tri con tr? ra ??ng sau d?u "."
                        setSelectionStart(id, idxDec + 1);
                        return false;
                    }
                }

                if (idxDec != -1) {
                    //Khi con tr? n?m ? v? tri sau d?u "."
                    if (selectionStart > idxDec) {
                        //Ki?m tra xem no co v??t qua s? decimal cho phep
                        var decimalNum = val.split(".", 2)[1];
                        if (selectionStart == val.length
                            && dg == decimalNum.length) {

                            e.preventDefault ? e.preventDefault() : e.returnValue = false;
                            return false;
                        } else {
                            //Xoa ky t? sau con tr? ?? n?i v?i s? ???c nh?p
                            this.value = val.replaceAt(selectionStart, "");
                        }
                        setSelectionStart(id, selectionStart);
                        return true;
                    }
                }

            } else {
                if (a == 190 || a == 110) {
                    //Khong cho nh?p s? decimal
                    e.preventDefault ? e.preventDefault() : e.returnValue = false;
                    return false;
                }
            }

            // Khi gia tr? ?ang nh?p la r?ng ma cho phep nh?p s? decimal
            // ho?c ?ang ch?n toan b? text
            // t? ??ng fill ph?n decimal 
            if (trim(val, '-,.') == "" ||
                ((this.selectionStart != this.selectionEnd &&
                    (this.selectionEnd - this.selectionStart) == val.length) && dg > 0)) {

                if (idxNeg == -1) {
                    //B? ??nh d?ng css s? am
                    $(this).removeClass("negative-num");
                }

                var decimalNum = ''.padRight(dg, '0');
                var selectionStart = 0;
                if (dg > 0) {
                    decimalNum = '.' + decimalNum;
                    selectionStart = 1;
                }
                //Nh?p vao d?u "."
                if (a == 190 || a == 110) {
                    this.value = decimalNum;
                } else {

                    if (a >= 48 && a <= 57) {
                        // Nh?p t? 0 -> 9
                        val = String.fromCharCode(a) + decimalNum;
                    } else {
                        // Nh?p t? numpad0 -> numpad9
                        val = String.fromCharCode(a - 48) + decimalNum;
                    }

                    this.value = idxNeg == -1 ? val : '-' + val;

                    if (dg > 0) {
                        selectionStart = (this.value.length - dg) - 1;
                    } else {
                        selectionStart = this.value.length;
                    }
                }

                setSelectionStart(id, selectionStart);
                e.preventDefault ? e.preventDefault() : e.returnValue = false;
                return false;

            } else {

                if (this.selectionStart == this.selectionEnd) {
                    if (this.selectionStart == idxNeg) {
                        e.preventDefault ? e.preventDefault() : e.returnValue = false;
                        return false;
                    }
                    val = val.addComma();
                    var numbers = val.split(".");
                    var number = trim(numbers[0], "-");
                    if (number.length >= intmxL) {
                        e.preventDefault ? e.preventDefault() : e.returnValue = false;
                        return false;
                    }
                }
                else {
                    if (this.selectionEnd - this.selectionStart != 0) {

                        var selNum = val.slice(this.selectionStart, this.selectionEnd);
                        var idxDecS = selNum.indexOf('.');
                        if (idxDecS != -1) {
                            e.preventDefault ? e.preventDefault() : e.returnValue = false;
                            return false;
                        }
                    }
                }
            }
        } else {
            e.preventDefault ? e.preventDefault() : e.returnValue = false;
            return false;
        }
    }).bind('keyup', function (e) {
        if ($(this).is(':disabled, [readonly]')) {
            return;
        }

        //Ti?n hanh format chu?i nh?p vao

        if (isControlKey(e)) {
            return true;
        }
        var a = [e.keyCode || e.which];
        //Ctrl + C,X,V
        if ((a == 88 && e.ctrlKey === true) ||
            (a == 86 && e.ctrlKey === true) ||
            (a == 67 && e.ctrlKey === true)) {
            return true;
        }

        var selectionStart = this.selectionStart;
        var id = $(this).attr("id");

        var commaCount = this.value.split(",").length - 1;
        selectionStart = selectionStart - commaCount;

        //Them format vao chuoi da nhap
        this.value = this.value.addComma();
        commaCount = this.value.split(",").length - 1;

        selectionStart = selectionStart + commaCount;

        setSelectionStart(id, selectionStart);

    }).change(function () {
        if (!changed) {
            changed = true;
        }
        var neg = $(this).attr("allow-negative");
        if (neg.toBool()) {
            var idxNeg = this.value.indexOf("-");
            if (idxNeg != -1) {
                $(this).addClass("negative-num");
            } else {
                $(this).removeClass("negative-num");
            }
        }
    }).blur(function () {
        if ($(this).is(':disabled, [readonly]')) {
            return;
        }

        //Fill Zero
        var val = this.value;

        if (trim(val, '-') != "") {

            var id = $(this).attr("id");

            var dg = $(this).attr("decimal-digit");
            if (dg > 0) {
                var numbers = val.split(".");
                var number = numbers[0];
                if (number == "") {
                    number = "0";
                }
                var decimalNum = "".padRight(dg, "0");
                if (numbers.length > 1) {
                    decimalNum = numbers[1].padRight(dg, "0");
                }
                this.value = number + "." + decimalNum;
            }
            this.value = this.value.addComma();
        } else {
            this.value = "";
        }

        if (!changed) {
            $(this).trigger("change");
        }

        if ($(this).val() !== $(this).attr("old-val")) {
            if (!checkNumberPasted(val, this)) {
                this.value = "";
                return;
            }
            var onvaluechange = eval($(this).attr("onvaluechange"));
            if (typeof (onvaluechange) != 'undefined') {
                onvaluechange(this);
            }
        }
    }).bind('paste', function (e) {
        var clipboardData = window.clipboardData;
        if (typeof (clipboardData) == 'undefined') {
            clipboardData = (e.originalEvent || e).clipboardData;
        }
        //disable paste
        e.preventDefault();
        var pasted = $.trim(clipboardData.getData('text'));
        if (checkNumberPasted(pasted, this)) {

            if (this.selectionStart != this.selectionEnd &&
                (this.selectionEnd - this.selectionStart) == this.value.length) {
                this.value = pasted;
            } else {
                var startNum = this.value.slice(0, this.selectionStart);
                var endNum = this.value.slice(this.selectionEnd, this.value.length);
                var newVal = startNum + pasted + endNum;
                if (checkNumberPasted(newVal, this)) {
                    this.value = newVal;
                }
            }
        }
    }).bind('drop', function (e) {
        var dropValue = $.trim(e.originalEvent.dataTransfer.getData("text"));
        //disable drop
        e.preventDefault();
        if (checkNumberPasted(dropValue, this)) {
            this.value = dropValue;
        }
    }).each(function (i) {
        var neg = $(this).attr("allow-negative");
        if (neg.toBool()) {
            var idxNeg = this.value.indexOf("-");
            if (idxNeg != -1) {
                $(this).addClass("negative-num");
            }
        }
    });
}


///////////////////////////////////////////
//  AJAX LOAD USING FOR SHOW PROPERTIES BY CODE
//  Create  : isv.thuy
//  Date    : 16/07/2014  
///////////////////////////////////////////
function ajax(funcName, params, callback) {

    /// <summary locid='1'>execute ajax to server</summary>
    /// <param name='funcName' locid='2'>function name</param>
    /// <param name='params' locid='3'>paramater</param>
    /// <param name='callback' locid='4'>call back handle</param>

    $.ajax({
        type: 'POST',
        url: $(location).attr('pathname') + '/' + funcName,
        data: JSON.stringify(params),
        contentType: 'application/json',
        dataType: 'json',
        async: false,
        success: function (d) {
            callback(d);
            selectActiveItem();
        }
    });
}

/**
* Check time
************************
* Format : HH:mm
*/
function isTime(time) {
    time = time.replace(/:/g, '');

    if ($.trim(time) == '') {
        return false;
    }

    if (isNaN($.trim(time))) {
        return false;
    }

    if (time.length > 5) {
        return false;
    }

    strHHH = time.substr(0, 2);
    strMM = time.substr(2, 2);
    timeType = 0;
    if (timeType == 0) {
        if ((strHHH < 0 || 47 < strHHH) || (time == "")) {
            //alert("時刻項目には00:00～47:59の範囲で入力してください");			
            return false;
        }
    }
    else if (timeType == 1) {
        if (strHHH < 0 || 99 < strHHH) {
            //alert("時間項目には00:00～99:59の範囲で入力してください");			
            return false;
        }
    }

    if (strMM < 0 || 59 < strMM) {
        //alert("分には0～59の範囲で入力してください");
        return false;
    }

    return true;
}

function WorkTimeFormatCommon(time) {
    time = time.replace(/:/g, '');
    if (time.length == 1) {
        time = time.padLeft(2, '0').padRight(4, '0');
    } else if (time.length == 2) {
        time = time.padRight(4, '0');
    } else if (time.length == 3) {
        time = time.padLeft(4, '0');
    }

    strHHH = time.substr(0, 2);
    strMM = time.substr(time.length - 2, 2);

    time = strHHH + ':' + strMM;

    return time;
}

/**
* Check date
************************
* Format : dd/mm/yyyy
*/
function isDate(date, _pickDate, _pickTime, _pickFormat) {

    date = date.replace(/\//g, '').replace(/:/g, '');
    fmt = _pickFormat.replace(/\//g, '').replace(/:/g, '');

    

    if ($.trim(date) == '') {
        return false;
    }

    if (!_pickTime && isNaN($.trim(date))) {
        return false;
    }

    if (date.length != fmt.length) {
        return false;
    }

    var arrFmt = fmt.split(' ');
    if (arrFmt.length == 2) {

    }
    var datetime = date.split(' ');

    //Checks for ddmmyyyy format.
    var day = 0;
    var month = 0;
    var year = 0;
    var hour = 0;
    var minute = 0;
    var seconds = 0;

    var isDay = true;
    var isMonth = true;
    if (fmt.toUpperCase() == "DDMMYYYY HHMMSS") {
        day = datetime[0].substring(0, 2);
        month = datetime[0].substring(2, 4);
        year = datetime[0].substring(4, 8);

        hour = datetime[1].substring(0, 2);
        minute = datetime[1].substring(2, 4);
        seconds = datetime[1].substring(4, 6);

        return checkYMD(year, month, day);
    }

    if (fmt.toUpperCase() == "DDMMYYYY HHMM") {
        day = datetime[0].substring(0, 2);
        month = datetime[0].substring(2, 4);
        year = datetime[0].substring(4, 8);

        hour = datetime[1].substring(0, 2);
        minute = datetime[1].substring(2, 4);

        return checkYMD(year, month, day);
    }

    if (fmt.toUpperCase() == "DDMMYYYY") {
        day = datetime[0].substring(0, 2);
        month = datetime[0].substring(2, 4);
        year = datetime[0].substring(4, 8);

        return checkYMD(year, month, day);
    }

    if (fmt.toUpperCase() == "YYYYMMDD HHMMSS") {
        year = datetime[0].substring(0, 4);
        month = datetime[0].substring(4, 6);
        day = datetime[0].substring(6, 8);

        hour = datetime[1].substring(0, 2);
        minute = datetime[1].substring(2, 4);
        seconds = datetime[1].substring(4, 6);

        return checkYMD(year, month, day);
    }

    if (fmt.toUpperCase() == "YYYYMMDD HHMM") {
        year = datetime[0].substring(0, 4);
        month = datetime[0].substring(4, 6);
        day = datetime[0].substring(6, 8);

        hour = datetime[1].substring(0, 2);
        minute = datetime[1].substring(2, 4);

        return checkYMD(year, month, day);
    }

    if (fmt.toUpperCase() == "YYYYMMDD") {
        year = datetime[0].substring(0, 4);
        month = datetime[0].substring(4, 6);
        day = datetime[0].substring(6, 8);

        return checkYMD(year, month, day);
    }

    if (fmt.toUpperCase() == "YYYYMM") {
        year = datetime[0].substring(0, 4);
        month = datetime[0].substring(4, 6);

        return checkYM(year, month);
    }

    if (fmt.toUpperCase() == "MMYYYY") {
        month = datetime[0].substring(0, 2);
        year = datetime[0].substring(2, 6);

        return checkYM(year, month);
    }

    if (fmt.toUpperCase() == "YYYY") {
        year = datetime[0];

        return checkY(year);
    }   

    if (hour > 23 || minute > 59 || seconds > 59) {
        return false;
    }

    return true;
}

function checkYMD(year , month , day) {
    if (month < 1 || month > 12) {
        return false;
    } else if ((day < 1 || day > 31)) {
        return false;
    } else if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31) {
        return false;
    } else if (month == 2) {
        var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
        if (day > 29 || (day == 29 && !isleap)) {
            return false;
        }
    } else if (year.length > 2 && (year < 1900 || year > 9999)) {
        return false;
    } else if (year.length < 2) {
        return false;
    }

    return true;
}

function checkYM(year , month) {
    if (month < 1 || month > 12) {
        return false;
    } else if (year.length > 2 && (year < 1900 || year > 9999)) {
        return false;
    } else if (year.length < 2) {
        return false;
    }

    return true;
}

function checkY(year) {
    if (year.length > 2 && (year < 1900 || year > 9999)) {
        return false;
    } else if (year.length < 2) {
        return false;
    }
    return true;
}

/**
*** Check Number Value
*** Author: ISV-PHUONG
*** Date  : 2014/08/29
********************************/
function checkNumberPasted(value, sender) {

    var number = value.replace(/,/g, '');
    var neg = $(sender).attr("allow-negative");

    for (var i = 0; i < number.length; i++) {
        var cin = number.charAt(i);

        if (neg.toBool()) {
            if (cin == '-' && i == 0) {
                continue;
            }
        }
        var num = $.inArray(cin, $num);
        if (num == -1 && cin != '.') {
            return false;
        }
    }

    var dg = $(sender).attr("decimal-digit");
    var intmxL = $(sender).attr("int-max-length");

    var numbers = value.split(".");
    if (numbers.length > 2) {
        return false;
    }
    var intNum = trim(numbers[0].addComma(), '-');
    if (intNum.length > intmxL) {
        return false;
    }
    if (dg > 0 && numbers.length > 1) {
        var decNum = numbers[1];
        if (decNum.length > dg) {
            return false;
        }
    }
    return true;
}

/**
*** Check Code Value
*** Author: ISV-PHUONG
*** Date  : 2014/08/29
********************************/
function checkCodePasted(value, sender) {
    var b = [];
    if ($(sender).hasAttr("allow-char")) {
        b = $(sender).attr("allow-char").split(",");
    }
    var ct = $(sender).attr("code-type");
    var mxL = $(sender).attr("maxlength");
    if (value.length > mxL) {
        return false;
    }
    if (ct == 1) {
        //0-9
        for (var i = 0; i < value.length; i++) {
            var cin = value.charAt(i);
            var alw = $.inArray(cin, b) != -1;
            var num = $.inArray(cin, $num) != -1;

            if (!num && !alw) {
                return false;
            }
        }
        return true;

    } else if (ct == 2) {

        //A-Z, 0-9
        for (var i = 0; i < value.length; i++) {
            var cin = value.charAt(i);
            var alw = $.inArray(cin, b) != -1;

            var _num = $.inArray(cin, $num) != -1;
            var _char = $.inArray(cin, $char) != -1;

            if (!_num && !_char && !alw) {
                return false;
            }
        }
        return true;
    } else if (ct == 3) {

        //A-Z, 0-9, '/'
        for (var i = 0; i < value.length; i++) {
            var cin = value.charAt(i);

            var alw = $.inArray(cin, b) != -1;
            var _num = $.inArray(cin, $num) != -1;
            var _char = $.inArray(cin, $char) != -1;

            if (!_num && !_char && cin != '/' && !alw) {
                return false;
            }
        }
        return true;

    } else if (ct == 4) {
        //A-Z, 0-9, '/', ' '
        for (var i = 0; i < value.length; i++) {
            var cin = value.charAt(i);

            var alw = $.inArray(cin, b) != -1;
            var _num = $.inArray(cin, $num) != -1;
            var _char = $.inArray(cin, $char) != -1;

            if (!_num && !_char && cin != '/' && cin != ' ' && !alw) {
                return false;
            }
        }
        return true;
    } else if (ct == 5) {

        //A-Z
        for (var i = 0; i < value.length; i++) {
            var cin = value.charAt(i);
            var alw = $.inArray(cin, b) != -1;
            var _char = $.inArray(cin, $char) != -1;

            if (!_char && !alw) {
                return false;
            }
        }
        return true;
    }
    return false;
}

/**
* Skip to Control key
************************************
*/
function isControlKey(e) {
    if ([e.keyCode || e.which] > 8 &&
        [e.keyCode || e.which] < 46) {
        return true;
    }
}

/**
* Set selection index
************************************
*/
function setSelectionStart(controlId, selectionStart) {
    var elem = document.getElementById(controlId);

    if (elem != null) {
        if (elem.createTextRange) {
            var range = elem.createTextRange();
            range.move('character', selectionStart);
            range.select();
        }
        else {
            elem.setSelectionRange(selectionStart, selectionStart);
        }
    }
}
/**
*  overide function __doPostBack ASP.NET
*****************************************
*/
// save a reference to the original __doPostBack
var __oldDoPostBack = typeof (__doPostBack) != "undefined" ? __doPostBack : null;

// replace __doPostBack with another function

__doPostBack = hijacking;

/**
*  hijacking function
************************************
*/
function hijacking(tartget, arg) {

    // do whatever pre-form submission

    // chores you need here

    // finally, let the original __doPostBack do its work
    showLoading();

    return __oldDoPostBack(tartget, arg);

}

/**
*  hijacking onclick of button loading
************************************
*/
function registLoading() {
    $('.loading').each(function () {
        // save a reference to the original onclick
        var oldClick = this.onclick;
        // reset `onclick` event handlers
        this.onclick = null;
        $(this).bind("click", function (e) {
            if (oldClick != null) {
                showLoading();
                // finally, let the original onclick do its work
                return oldClick(e);
            }
        });
    });
}

/**
*  F(x) Pressing
************************************
*/
document.onhelp = function () {
    return false;
}

/**
*  On F(x) Pressing
************************************
*/
function onFunctionKey(FKey) {
    return false;
}
/**
*  execute command 
************************************
*/
function execute($button) {
    if ($button.is(':disabled, [readonly]')) {
        return;
    }
    $button.addClass("active");
    if ($button.is("a")) {
        eval($button.attr('href'));

    } else if ($button.is("submit") || $button.is("button")) {
        $button.trigger("click");
        window.setTimeout(function () {
            $button.removeClass("active");
        }, 200);
    }
}

//]]>