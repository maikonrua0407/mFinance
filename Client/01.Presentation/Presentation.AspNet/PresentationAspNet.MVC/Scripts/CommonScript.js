// Keep Alive script - SJM July 19th 2010
// How long in minutes should the session keep alive for?
// (This should be a multiple of 10 to work properly).
var WARNING = 'WARNING';
var WARNING_SUCCESS = 'SUCCESS';
var WARNING_ERROR = 'ERROR';
var WARNING_NOTICE = 'THONGBAO';
var WARNING_NOROLE = 'NOROLE';
var WARNING_EXIST_ITEM = 'NOTEXISTITEM';

Date.prototype.FormatDate = function () {

    var yyyy = this.getFullYear().toString();
    var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based         
    var dd = this.getDate().toString();

    return (dd[1] ? dd : "0" + dd[0]) + '/' + (mm[1] ? mm : "0" + mm[0]) + '/' + yyyy;
};


//valid datetime
function isDate(str) {
    var parms = str.split(/[\.\-\/]/);
    var yyyy = parseInt(parms[2], 10);
    var mm = parseInt(parms[1], 10);
    var dd = parseInt(parms[0], 10);
    var date = new Date(yyyy, mm - 1, dd, 0, 0, 0, 0);

    return mm === (date.getMonth() + 1) && dd === date.getDate() && yyyy === date.getFullYear() && (date.getFullYear() + '').length === 4;
}

function checkDate(firstDate, noteFirst, secondDate, noteSecond, thirdDate, noteThird) {
    if ($('#' + firstDate).val() == '' || $('#' + firstDate).val() == "undefined") {
        ShowAlert('Chọn thời gian ' + noteFirst);
        return false;
    }
    if (!isDate($('#' + firstDate).val())) {
        ShowAlert(noteFirst + ' sai định dạng thời gian');
        return false;
    }
    if (typeof noteSecond != 'undefined' && noteSecond != '') {
        if ($('#' + secondDate).val() == '' || $('#' + secondDate).val() == "undefined") {
            ShowAlert('Chọn thời gian ' + noteSecond);
            return false;
        }
        if (!isDate($('#' + secondDate).val())) {
            ShowAlert(noteSecond + ' sai định dạng thời gian');
            return false;
        }
    }
    if (typeof noteThird != 'undefined' && noteThird != '') {
        if ($('#' + thirdDate).val() == '' || $('#' + thirdDate).val() == "undefined") {
            ShowAlert('Chọn thời gian ' + noteThird);
            return false;
        }
        if (!isDate($('#' + thirdDate).val())) {
            ShowAlert(noteThird + ' sai định dạng thời gian');
            return false;
        }
    }
    return true;
}


// type : normal, search, confirm
function PopupSearch(title, width, showButtonSave, funcName, type) {
    ResetAlertInPopup();
    $('#btnPop_Save').show();
    $('#btnPop_Close').show();
    if (typeof type != 'undefined' && type == 'normal') {
        $('#sp_save').html('Lưu');
        $('#sp_close').html('Đóng');
    }
    else if (typeof type != 'undefined' && type == 'search') {
        $('#sp_save').html('Chọn');
        $('#sp_close').html('Đóng');
    }
    else if (typeof type != 'undefined' && type == 'confirm') {
        $('#sp_save').html('Đồng ý');
        $('#sp_close').html('Hủy');
    }
    else if (typeof type != 'undefined' && type == 'approved') {
        $('#sp_save').html('Duyệt');
        $('#sp_close').html('Hủy');
    }
    else if (typeof type != 'undefined' && type == 'view') {
        $('#sp_close').html('Đóng');
    }
    $('#pop_title').html(title);
    $('#modal-form').css({ 'width': width + 'px' });
    fixCenterBox('#modal-form');
    $('#dialog-container').show();
    $('#modal-form').show();

    if (typeof type != 'undefined' && type == 'view') {
        $('#btnPop_Save').hide();
    } else if (type == 'notice') {
        $('#btnPop_Save').hide();
        $('#btnPop_Close').hide();
    } else {
        $('#btnPop_Save').attr("onclick", funcName);
        $('#btnPop_Save').show();
    }
}
// type : normal, search, confirm
function PopupSearchV2(title, width, showButtonSave, funcName, type) {
    ResetAlertInPopup();
    $('.model-form-v2 #btnPop_Save-v2').show();
    $('.model-form-v2 #btnPop_Close-v2').show();
    if (typeof type != 'undefined' && type == 'normal') {
        $('.model-form-v2 #sp_save-v2').html('Lưu');
        $('.model-form-v2 #sp_close-v2').html('Đóng');
    }
    else if (typeof type != 'undefined' && type == 'search') {
        $('.model-form-v2 #sp_save-v2').html('Chọn');
        $('.model-form-v2 #sp_close-v2').html('Đóng');
    }
    else if (typeof type != 'undefined' && type == 'confirm') {
        $('.model-form-v2 #sp_save-v2').html('Đồng ý');
        $('.model-form-v2 #sp_close-v2').html('Hủy');
    }
    else if (typeof type != 'undefined' && type == 'approved') {
        $('.model-form-v2 #sp_save-v2').html('Duyệt');
        $('.model-form-v2 #sp_close-v2').html('Hủy');
    }
    else if (typeof type != 'undefined' && type == 'view') {
        $('.model-form-v2 #sp_save-v2').hide();
        $('.model-form-v2 #sp_close-v2').html('Đóng');
    }
    else if (typeof type != 'undefined' && type == 'refresh') {
        $('.model-form-v2 #sp_save-v2').html('Đóng');
    }
    $('.model-form-v2 #pop_title-v2').html(title);
    $('#modal-form-v2.model-form-v2').css({ 'width': width + 'px' });
    fixCenterBox('#modal-form-v2.model-form-v2');
    $('#dialog-container-v2.model-form-v2').show();
    $('#modal-form-v2.model-form-v2').show();

    if (typeof type != 'undefined' && type == 'view') {
        $('.model-form-v2 #btnPop_Save-v2').hide();
    } else if (type == 'notice') {
        $('.model-form-v2 #btnPop_Save-v2').hide();
        $('.model-form-v2 #btnPop_Close-v2').hide();
    } else if (type == 'refresh') {
        $('.model-form-v2 #btnPop_Save-v2').attr("onclick", funcName);
        $('.model-form-v2 #btnPop_Save-v2').show();
        $('.model-form-v2 #btnPop_Close-v2').hide();
    } else {
        $('.model-form-v2 #btnPop_Save-v2').attr("onclick", funcName);
        $('.model-form-v2 #btnPop_Save-v2').show();
    }
}
function PopupSearchCancel(obj) {
    //$('#data').html('');
    if (obj != undefined) {
        $(obj).hide();
        $('#dialog-container-v2' + obj).hide();
    } else {
        $('#dialog-container').hide();
        $('#modal-form').hide();
    }
}

function fixCenterBox(box) {
    var kichThuocBrowser = $(window).width();
    var kichThuocBox = $(box).width();
    var caoThuocBrowser = $(window).height();
    var caoThuocBox = $(box).height();
    var fixLeft = (kichThuocBrowser - kichThuocBox) / 2;
    var fixTop = (caoThuocBrowser - caoThuocBox) / 2;
    $(box).css({ 'left': fixLeft, 'top': (fixTop - 5) });
}


function InputOnlyNumeric(event) {
    // Allow special chars + arrows 
    if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9
        || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 67 || event.keyCode == 86
        || event.keyCode == 88 || event.keyCode == 188 || event.keyCode == 190
        || (event.keyCode == 65 && event.ctrlKey === true)
        || (event.keyCode >= 35 && event.keyCode <= 39)
        || (event.keyCode == 110)
        || event.keyCode == 37
        || event.keyCode == 39) {
        return;
    } else {
        // If it's not a number stop the keypress
        if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
            event.preventDefault();
        }
    }
}

//$(document).ready(function ($) {
//    // tmcuong add - Only input number
//    // with input type = number
//    $('input[type=number]').keydown(function (event) {
//        InputOnlyNumeric(event);
//    });
//});


// Clone object
var clone = function (o) {
    var n = {};
    for (i in o)
        n[i] = (typeof o[i] == 'object') ? arguments.callee(o[i]) : o[i];
    return n;
};

function NavigatePaging(currentPage, pageCount) {
    var iFrom = currentPage - 4;
    if (iFrom <= 0) iFrom = 1;
    var sPaging = '<div class="row"><div class="twelve columns paging-box">';
    if (currentPage == 1)
        sPaging += '<a class="page-panel first-panel" onclick="return false;">&lt;&lt;&nbsp;</a> <a class="page-panel" onclick="return false;">&lt;&nbsp;</a>';
    else
        sPaging += '<a class="page-panel first-panel" href="javascript:void(0);" onclick="gotoPage(1);">&lt;&lt;&nbsp;</a> <a class="page-panel" href="javascript:void(0);" onclick="gotoPage(' + (currentPage - 1).toString() + ');">&lt;&nbsp;</a>';
    for (var i = iFrom; i <= iFrom + 8; i++) {
        if (i <= pageCount) {
            // Nếu i không phải trang đầu tiên thì thêm ... ở đầu
            if (i == iFrom && i > 1) {
                sPaging += '<a class="page-panel" ><span>...</span>    </a>';
            }
            var href = (i == currentPage) ? "" : "href='javascript:void(0);'";
            var clsCSS = (i == currentPage) ? "page-panel current" : "page-panel";
            var funcJs = (i == currentPage) ? "return false;" : "gotoPage(" + i + ");";
            sPaging += '<a id="page_' + i + '" class="' + clsCSS + '" ' + href + ' onclick="' + funcJs + '"><span>' + i.toString() + '</span></a>';
            if (i == iFrom + 8 && iFrom + 8 < pageCount) {
                sPaging += ' <a class="page-panel" ><span>...</span>    </a>';
            }
        }
    }
    if (currentPage < pageCount) {
        sPaging += '<a class="page-panel" href="javascript:void(0);" onclick="gotoPage(' + (currentPage + 1).toString() + ');">&nbsp;&gt;</a>';
        sPaging += '<a class="page-panel last-panel" href="javascript:void(0);" onclick="gotoPage(' + pageCount + ');">&nbsp;&gt;&gt;</a>';
    }
    else {
        sPaging += '<a class="page-panel" onclick="return false;">&nbsp;&gt;</a>';
        sPaging += '<a class="page-panel last-panel" onclick="return false;">&nbsp;&gt;&gt;</a>';
    }
    sPaging += '</div></div>';
    return sPaging;
}
// Ham string format
String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }
    return s;
};
// jquery dialog
function CloseDialog(model) {
    if (model != null && model != undefined)
        model.dialog("close");
    else
        $("#DialogContent").dialog("close");
}

function DialogYesNoConfirm(model, width, yesButtonText, noButtonText, callBackFunctionName, notClose) {
    if (width == undefined || width == null || width == 0) width = 500;
    if (yesButtonText == undefined || yesButtonText == null || yesButtonText == '') yesButtonText = "Đồng ý";
    if (noButtonText == undefined || noButtonText == null || noButtonText == '') noButtonText = "Hủy";
    //add class
    //$(_contDialogFormat.Icon).attr("class", "ui-icon dialog-icon 'icon-none'");

    model.dialog({
        width: width,
        modal: true,
        // position: "center",
        dialogClass: _contDialogFormat.CustomStype,
        buttons: [
            {
                text: yesButtonText,
                //icons: { primary: "ui-icon-check" },
                'class': 'btn btn-small btn-primary',
                click: function () {
                    callBackFunctionName();
                    if (notClose == undefined || notClose == null || notClose == false) $(this).dialog("close");
                }
            },
            {
                text: noButtonText,
                //icons: { primary: "ui-icon-closethick" },
                'class': 'btn btn-small btn-primary',
                click: function () { $(this).dialog("close"); }
            }
        ]
    });

    $(".ui-dialog-title").remove();
    $(".ui-dialog-titlebar-close").remove();
}


// define FunctionTwo as needed
var t = 0;
function myTimer() {
    if (t < 50)
        t += 1;
    $("#processing").attr('data-percent', t + '%');
    $("#processingbar").width(t + '%');
}
function ShowProcess(funcName, autoClose) {
    t = 0;
    $("#dialog-process").show();
    if (autoClose == undefined || autoClose == null) autoClose = false;
    if (autoClose == true) setTimeout(function () { $("#dialog-process").hide(); }, 500);
    if (funcName != null) funcName();
}
function CloseProcess() {
    t = 50;
    $("#processing").attr('data-percent', '100%');
    $("#processingbar").width('100%');
    setTimeout(function () { $("#dialog-process").hide(); }, 500);
}
function DialogWithSubmit(model, width, yesButtonText, callBackFunctionName, notClose) {
    var defer = $.Deferred();
    if (width == undefined || width == null || width == 0) width = 500;
    if (yesButtonText == undefined || yesButtonText == null || yesButtonText == '') yesButtonText = "Đồng ý";
    //add class
    $(_contDialogFormat.Icon).attr("class", "ui-icon dialog-icon 'icon-none'");
    //ui-dialog-titlebar ui-widget-header 
    model.dialog({
        width: width,
        modal: true,
        // position: "center", //"center",
        dialogClass: _contDialogFormat.CustomStype,
        buttons: [
    {
        text: yesButtonText,
        //icons: { primary: "ui-icon-check" },
        'class': 'icon-agree',
        click: function () {

            callBackFunctionName();

            //callbacks.add(function () { $("#dialog-process").hide(); });
            //callbacks.fire();
            if (!notClose) $(this).dialog("close");
        }
    },
        ],
        close: function () {
            // $("#dialog-process").hide();
        }
    });
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
var _contDialogFormat = new DialogFormat();
function DialogFormat() {
    this.Content = "#DialogContent";    //div content dùng để show dialog
    this.Message = "#DialogMesage";     //div nội dung message của dialog
    this.Icon = "#IconDialog";          //div icon của dialog
    this.Title = "NGVGroup.vn";                 //tiêu đề dialog
    this.Width = 380;                       //width mặc định của dialog
    this.CustomStype = "dialogCustomStyle"; //điều khiển style của dialog
}

/*
Loại dialog
*/
var _contDialogType = new DialogType();
function DialogType() {
    this.Alert = 1;
    this.ConfirmYesNo = 2;
    this.ConfirmYesNoCancel = 3;
}

/*
Loại icon của dialog
*/
var _contDialogIconType = new DialogIconType();
function DialogIconType() {
    this.Information = 1;
    this.Warning = 2;
    this.Error = 3;
    this.Confirm = 4;
    this.None = 5;
}

/*
sinh dialog icon theo icon type truyền vào.
*/
function getDialogIconClass(icon, defaultIcon) {
    var iconClass = '';
    switch (icon) {
        case _contDialogIconType.Information:
            iconClass = 'icon-information';
            break;
        case _contDialogIconType.Warning:
            iconClass = 'icon-warning';
            break;
        case _contDialogIconType.Error:
            iconClass = 'icon-error';
            break;
        case _contDialogIconType.Confirm:
            iconClass = 'icon-confirm';
            break;
        case _contDialogIconType.None:
            iconClass = 'icon-none';
            break;

        default:
            if (defaultIcon)
                //iconClass = getDialogIconClass(defaultIcon);
                iconClass = 'icon-none';
            break;
    }

    return iconClass;
}

function ShowWarning(message, dialogWidth) {
    ShowDialog_Alert(message, _contDialogIconType.Warning, dialogWidth);
}

/*
 show dialog thông báo
dialogIcon:        Icon của dialog
dialogWidth:       Width của dialog
message:            Nội dung dialog
commandName:       Command name sẽ truyền trong param ajax request xuống server
*/
var _tabIndexFocus = null;
function ShowDialog_Alert(message, dialogIcon, dialogWidth, closeCommandName) {
    setTimeout(function () {
        $(function () {
            //add message
            $(_contDialogFormat.Message).html(message);

            //add class
            $(_contDialogFormat.Icon).attr("class", "ui-icon dialog-icon " + getDialogIconClass(dialogIcon, _contDialogIconType.Information)); //show dialog
            var element = document.querySelector(":focus");
            try {
                if (_tabIndexFocus && !checkUndefined(_tabIndexFocus)) {
                    element = $(":input[tabindex='" + _tabIndexFocus + "']");
                }
            } catch (err) { }
            $(_contDialogFormat.Content).dialog({
                title: _contDialogFormat.Title,
                width: getDialogWidth(dialogWidth),
                // position: "center", //"center",
                dialogClass: _contDialogFormat.CustomStype, //'dialogCustomStyle',
                resizable: false,
                modal: true,
                beforeClose: function (event, ui) {
                    //server
                    callServerFunction(closeCommandName);
                },
                buttons:
                [{
                    text: 'Đóng',
                    'class': 'icon-close',
                    click: function () {
                        $(this).dialog("close");

                        if (element) {
                            element.focus();
                        }

                        //server
                        //callServerFunction(closeCommand);
                    }
                }]
            });
        });
    }, 0);
}
function ShowDialog_AlertClientCallBack(message, dialogIcon, dialogWidth, closeCommandName) {
    setTimeout(function () {
        $(function () {
            //add message
            $(_contDialogFormat.Message).html(message);

            //add class
            $(_contDialogFormat.Icon).attr("class", "ui-icon dialog-icon " + getDialogIconClass(dialogIcon, _contDialogIconType.Information)); //show dialog
            var element = document.querySelector(":focus");
            try {
                if (_tabIndexFocus && !checkUndefined(_tabIndexFocus)) {
                    element = $(":input[tabindex='" + _tabIndexFocus + "']");
                }
            } catch (err) { }
            $(_contDialogFormat.Content).dialog({
                title: _contDialogFormat.Title,
                width: getDialogWidth(dialogWidth),
                // position: "center", //"center",
                dialogClass: _contDialogFormat.CustomStype, //'dialogCustomStyle',
                resizable: false,
                modal: true,
                beforeClose: function (event, ui) {
                    //server
                    callClientFunction(closeCommandName);
                },
                buttons:
                [{
                    text: 'Đóng',
                    'class': 'icon-close',
                    click: function () {
                        $(this).dialog("close");

                        if (element) {
                            element.focus();
                        }

                        //server
                        //callServerFunction(closeCommand);
                    }
                }]
            });
        });
    }, 0);
}
/*
show dialog confirm yes no
*/
function ShowDialog_ConfirmYesNo(message, yesCommandName, noCommandName, dialogIcon, dialogWidth, closeCommandName) {
    var defer = $.Deferred();
    setTimeout(function () {
        $(function () {
            //add message
            $(_contDialogFormat.Message).html(message);

            //add class
            $(_contDialogFormat.Icon).attr("class", "ui-icon dialog-icon " + getDialogIconClass(dialogIcon, _contDialogIconType.Confirm)); //show dialog
            $(_contDialogFormat.Content).dialog({
                title: _contDialogFormat.Title,
                width: getDialogWidth(dialogWidth),
                // position: "center", //"center",
                dialogClass: _contDialogFormat.CustomStype, //'dialogCustomStyle',
                resizable: false,
                modal: true,
                beforeClose: function (event, ui) {
                    //server
                    callServerFunction(noCommandName);
                },
                buttons:
                [{
                    text: 'Đồng ý',
                    'class': 'icon-agree',
                    click: function () {
                        defer.resolve(true);

                        $(this).dialog("close");

                        //server
                        callServerFunction(yesCommandName);
                    }
                }, {
                    text: 'Hủy bỏ',
                    'class': 'icon-cancel',
                    click: function () {
                        defer.resolve(false);

                        $(this).dialog("close");

                        //server
                        //callServerFunction(noCommandName);
                    }
                }]
            });
        });
    }, 0);

    return defer.promise(); //important to return the deferred promise
}
/*
*/
function ShowDialog_ConfirmYesNo_Second(message, yesCommandName, noCommandName, dialogIcon, dialogWidth) {
    var defer = $.Deferred();
    setTimeout(function () {
        $(function () {
            //add message
            $(_contDialogFormat.Message).html(message);

            //add class
            $(_contDialogFormat.Icon).attr("class", "ui-icon dialog-icon " + getDialogIconClass(dialogIcon, _contDialogIconType.Confirm)); //show dialog
            $(_contDialogFormat.Content).dialog({
                title: _contDialogFormat.Title,
                width: getDialogWidth(dialogWidth),
                // position: "center", //"center",
                dialogClass: _contDialogFormat.CustomStype, //'dialogCustomStyle',
                resizable: false,
                modal: true,
                buttons:
                [{
                    text: 'Có',
                    'class': 'icon-agree',
                    click: function () {
                        defer.resolve(true);

                        $(this).dialog("close");

                        //server
                        callServerFunction(yesCommandName);
                    }
                }, {
                    text: 'Không',
                    'class': 'icon-no',
                    click: function () {
                        defer.resolve(false);

                        $(this).dialog("close");

                        //server
                        callServerFunction(noCommandName);
                    }
                }]
            });
        });
    }, 0);
    return defer.promise(); //important to return the deferred promise
}
/*
show dialog 3 nút
*/
function ShowDialog_ConfirmYesNoCancel(message, yesCommandName, noCommandName, dialogIcon, dialogWidth, isCheckChange) {
    var defer = $.Deferred();
    setTimeout(function () {
        $(function () {
            //add message
            $(_contDialogFormat.Message).html(message);

            //add class
            $(_contDialogFormat.Icon).attr("class", "ui-icon dialog-icon " + getDialogIconClass(dialogIcon, _contDialogIconType.Confirm)); //show dialog focus
            var element = document.querySelector(":focus");
            try {
                if (_tabIndexFocus && !checkUndefined(_tabIndexFocus)) {
                    element = $(":input[tabindex='" + _tabIndexFocus + "']");
                }
            } catch (err) { }
            //show dialog        
            $(_contDialogFormat.Content).dialog({
                title: _contDialogFormat.Title,
                width: getDialogWidth(dialogWidth),
                // position: "center",
                dialogClass: _contDialogFormat.CustomStype, //'dialogCustomStyle',
                resizable: false,
                modal: true,
                //'verify': true,
                buttons:
                [{
                    text: 'Có',
                    'class': 'icon-agree',
                    click: function () {
                        defer.resolve(true);

                        $(this).dialog("close");

                        //server
                        callServerFunction(yesCommandName);
                    }
                }, {
                    text: 'Không',
                    'class': 'icon-no',
                    click: function () {
                        defer.resolve(false);

                        $(this).dialog("close");
                        if (element) {
                            element.focus();
                        }
                        //server
                        callServerFunction(noCommandName);
                    }
                }, {
                    text: 'Hủy bỏ',
                    'class': 'icon-cancel',
                    click: function () {
                        defer.resolve(null);

                        $(this).dialog("close");
                    }
                }]
            });
        });
    }, 0);

    return defer.promise(); //important to return the deferred promise
}


// Xu ly voi call back client function
function ShowDialog_ConfirmYesNo_ClientCallBack(message, dialogWidth, YesButtonText, NoButtonText, dialogIcon, yesCommandName, noCommandName) {
    var sYes = 'Đồng ý';
    var sNo = 'Hủy bỏ';
    if (YesButtonText && YesButtonText != '') sYes = YesButtonText;
    if (NoButtonText && NoButtonText != '') sNo = NoButtonText;
    var defer = $.Deferred();
    setTimeout(function () {
        $(function () {
            //add message
            $(_contDialogFormat.Message).html(message);

            //add class
            $(_contDialogFormat.Icon).attr("class", "ui-icon dialog-icon " + getDialogIconClass(dialogIcon, _contDialogIconType.Confirm)); //show dialog
            $(_contDialogFormat.Content).dialog({
                title: _contDialogFormat.Title,
                width: getDialogWidth(dialogWidth),
                // position: "center", //"center",
                dialogClass: _contDialogFormat.CustomStype, //'dialogCustomStyle',
                resizable: true,
                modal: true,
                buttons:
                [{
                    text: sYes,
                    'class': 'icon-agree',
                    click: function () {
                        defer.resolve(true);

                        $(this).dialog("close");

                        //server
                        if (yesCommandName)
                            return yesCommandName();
                    }
                }, {
                    text: sNo,
                    'class': 'icon-no',
                    click: function () {
                        defer.resolve(false);

                        $(this).dialog("close");

                        if (noCommandName)
                            return noCommandName();
                    }
                }]
            });
        });
    }, 0);
    return defer.promise(); //important to return the deferred promise
}

//ShowDialog(string message, DialogType dialogType, DialogIconType iconType, int? dialogWidth = null, params object[] commandName)
/*
Hàm show dialog, tùy  thuộc vào dialogType sẽ show ra dialog tương ứng
*/
function showDialog(message, dialogType, dialogIcon, dialogWidth, yesCommand, noCommand, closeCommand) {
    var chArr = ',';   //Ký tự ngăn cách các phần tử trong mảng

    switch (dialogType) {
        case _contDialogType.Alert:
            ShowDialog_Alert(message, dialogIcon, dialogWidth, closeCommand);
            break;

        case _contDialogType.ConfirmYesNo:
            ShowDialog_ConfirmYesNo(message, yesCommand, noCommand, dialogIcon, dialogWidth, closeCommand);
            break;

        case _contDialogType.ConfirmYesNoCancel:
            ShowDialog_ConfirmYesNoCancel(message, yesCommand, noCommand, dialogIcon, dialogWidth);
            break;

        default:
            ShowDialog_Alert("Dialog Type: " + dialogType + " không phù hợp!", _contDialogIconType.Error);
            break;
    }
}

/*
 Hiển thị thông báo lỗi của control validate summary
*/
function showValidateSumary(arrValidateSummaryClientID, dialogWidth) {
    if (arrValidateSummaryClientID) {
        var sMessage = '';
        for (var i = 0; i < arrValidateSummaryClientID.length; i++) {
            sMessage += $('#' + arrValidateSummaryClientID[i]).html();
        }
        ShowDialog_Alert(sMessage, _contDialogIconType.Warning, dialogWidth);
    } else {
        ShowDialog_Alert("arrValidateSummaryClientID: " + arrValidateSummaryClientID + " không phù hợp!", _contDialogIconType.Error);
    }
}

/*
 Sinh width cho dialog
*/
function getDialogWidth(dialogWidth) {
    var iWidth = _contDialogFormat.Width;
    if (dialogWidth && dialogWidth > 0) {
        iWidth = dialogWidth;
    }
    return iWidth;
}

/*
Kiểm tra object
*/
function checkUndefined(obj) {
    return typeof (obj) == 'undefined';
}

/*
Gọi thực thi 1 function truyền vào
*/
function callClientFunction(_function) {
    if (!checkUndefined(_function)) {
        var func = _function;
        func();
    }
}

/*
Gọi ajax xuống server
*/
function callServerFunction(commandName) {
    if (!checkUndefined(commandName) && !commandName.isNullOrEmpty() && !commandName.isLastTrimEmpty()) {
        if (!checkUndefined(callAjaxRequest)) {
            callAjaxRequest(commandName);
        }
    }
}

function cancelEvent(args) {
    if (args && !checkUndefined(args)) {
        args.set_cancel(true);
    }
}

/*
tính position của dialog so với hiển thị của form
*/
function getTopOffset() {
    return (parent.document.documentElement.clientHeight) / 2 + parent.window.pageYOffset - 200;
}

/*
Di chuyển dialog ra giữa màn hình đang thao tác
*/
function getDialogPosition(isCheckChange) {
    if (isCheckChange && isCheckChange == true)
        return "center";
    else
        return ["top", getTopOffset()]; //"center",
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
Định nghĩa lại hàm trim do lỗi trên IE
*/
String.prototype.trim = function () {
    return this.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/g, '').replace(/\s+/g, ' ');
};

/*
Định nghĩa lại hàm string format
*/
String.prototype.format = function () {
    var formatted = this;
    for (var i = 0; i < arguments.length; i++) {
        var regexp = new RegExp('\\{' + i + '\\}', 'gi');
        formatted = formatted.replace(regexp, arguments[i]);
    }
    return formatted;
};

/*
Kiểm tra string empty
*/
String.prototype.isEmpty = function () {
    return this.length == 0;
};

/*
Kiểm tra string null Or empty
*/
String.prototype.isNullOrEmpty = function () {
    return !this;
};

/*
Kiểm tra string empty sau khi trim 
*/
String.prototype.isLastTrimEmpty = function () {
    return this.trim().length == 0;
};

var amount = '';
var tax = '';

function NumericDotOnly(inputObj, e) {
    var isAmount = inputObj.id == "amountVal";
    var e = (!e) ? window.event : e;
    var key = e.keyCode;
    if ((key < 48 || key > 57) && key != 8 && key != 110 && key != 190 || ((key == 110 || key == 190) && ((isAmount) ? amount : tax).indexOf('.') != -1)) {
        inputObj.value = (isAmount) ? amount : tax;
    }

    if (isAmount) {
        amount = inputObj.value;
    }
    else {
        tax = inputObj.value;
    }
}

function formatCurrency(inputObj) {
    if (inputObj.id == "amountVal") {
        amount = convertToFloat(inputObj.value);
    }
    else {
        tax = convertToFloat(inputObj.value);
    }
    inputObj.value = (inputObj.value != '') ? addCommas(convertToFloat(inputObj.value)) : '';
}

function convertToFloat(num) {
    return (num != '') ? parseFloat(num.replace(/,/g, "")).toFixed(2) : num;
}

function addCommas(num) {
    var numParts = num.split('.');
    var numArr = numParts[0].split('').reverse();
    var newArr = [];
    var count = -1;
    for (var i = 0; i < numArr.length; i++) {
        if (i % 3 == 0) {
            newArr[++count] = ',';
        }
        newArr[++count] = numArr[i];
    }
    return newArr.reverse().join('').replace(/((.+?)(,?))$/, '$2') + '.' + numParts[1];
}

function focusCampo(id) {
    var inputField = document.getElementById(id);
    if (typeof inputField != "undefined" && inputField.value.length > 0) {
        var elemLen = inputField.value.length;
        inputField.focus();
        inputField.selectionStart = elemLen;
        inputField.selectionEnd = elemLen;
    } else {
        inputField.focus();
    }
}

function isDate(txtDate) {
    var currVal = txtDate;
    if (currVal == '')
        return false;

    //Declare Regex  
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;

    //Checks for mm/dd/yyyy format.
    dtMonth = dtArray[3];
    dtDay = dtArray[1];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}

function today() {
    var fullDate = new Date();
    var currentDate = parserDate(fullDate);
    return currentDate;
}

function parserDate(fullDate) {
    var twoDigitMonth = ((fullDate.getMonth().length + 1) === 1) ? (fullDate.getMonth() + 1) : '0' + (fullDate.getMonth() + 1);
    var date = twoDigitMonth + "/" + fullDate.getDate() + "/" + fullDate.getFullYear();
    return date;
}

function string2Date(strDate) {
    var arr = strDate.split("/");
    var date = new Date(arr[2], arr[1] - 1, arr[0]);
    return parserDate(date);
}

function formatMoney(totalValue) {
    if (totalValue == null || totalValue == 0) return 0;
    var s = totalValue.toString().split("").reverse().reduce(function (acc, num, i, orig) {
        return num + (i && !(i % 3) ? "." : "") + acc;
    }, "");
    while (s.indexOf("-.") > -1) {
        s = s.replace("-.", "-");
    }
    return s;
}

function formatMoneyV2(nStr, isBlur) {
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 && (parseInt(x[1]) > 0 || isBlur == undefined || !isBlur) ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function RemoveCommas(str) {
    if (str == null || str == "")
        return "";
    var result = str.replace(/,/g, '');
    return result;
}

function replaceMoney(strMoney) {
    if (strMoney == 0)
        return "0";
    else
        return strMoney.replace(/\./g, '');
}
function isValidEmail(email) {
    var pattern = new RegExp(/^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$/);
    return pattern.test(email);
}

function isValidNumber(number) {
    var pattern = new RegExp(/[0-9 -()+]+$/);
    return pattern.test(number);

}

function ShowAlert(msgError, msgWarning, msgSuccess, msgInfo) {
    var show = false;
    if (typeof msgError != 'undefined' && msgError != '') {
        $('#msgError').html(msgError);
        $('#albError').show();
        show = true;
    } else {
        $('#msgError').text('');
        $('#albError').hide();
    }
    if (typeof msgWarning != 'undefined' && msgWarning != '') {
        $('#msgWarning').html(msgWarning);
        $('#albWarning').show();
        show = true;
    } else {
        $('#msgWarning').text('');
        $('#albWarning').hide();
    }
    if (typeof msgSuccess != 'undefined' && msgSuccess != '') {
        $('#msgSuccess').html(msgSuccess);
        $('#albSuccess').show();
        show = true;
    }
    else {
        $('#msgSuccess').text('');
        $('#albSuccess').hide();
    }
    if (typeof msgInfo != 'undefined' && msgInfo != '') {
        $('#msgInfo').html(msgInfo);
        $('#albInfo').show();
        show = true;
    }
    else {
        $('#msgInfo').text('');
        $('#albInfo').hide();
    }
    if (show == true) {
        $('#alert-bar').show().delay(3000).queue(function (n) {
            $(this).hide(); n();
        });
    }
}

function ShowAlertInPopup(msgError, msgWarning, msgSuccess, msgInfo, container) {
    var show = false;
    if (container == undefined || container == '' || $(container).length <= 0) {
        if (typeof msgError != 'undefined' && msgError != '') {
            $('#msgPopupError').html(msgError);
            $('#alpError').show();
            show = true;
        } else {
            $('#msgPopupError').html('');
            $('#alpError').hide();
        }
        if (typeof msgWarning != 'undefined' && msgWarning != '') {
            $('#msgPopupWarning').html(msgWarning);
            $('#alpWarning').show();
            show = true;
        } else {
            $('#msgPopupWarning').text('');
            $('#alpWarning').hide();
        }
        if (typeof msgSuccess != 'undefined' && msgSuccess != '') {
            $('#msgPopupSuccess').html(msgSuccess);
            $('#alpSuccess').show();
            show = true;
        } else {
            $('#msgPopupSuccess').html('');
            $('#alpSuccess').hide();
        }
        if (typeof msgInfo != 'undefined' && msgInfo != '') {
            $('#msgPopupInfo').html(msgInfo);
            $('#alpInfo').show();
            show = true;
        } else {
            $('#msgPopupInfo').html('');
            $('#alpInfo').hide();
        }
        if (show == true) {
            $('#alert-popup-bar').show().delay(3000).queue(function (n) {
                $(this).hide();
                n();
            });
        }
    } else {
        if (typeof msgError != 'undefined' && msgError != '') {
            $(container + ' #msgPopupError').html(msgError);
            $(container + ' #alpError').show();
            show = true;
        } else {
            $(container + ' #msgPopupError').html('');
            $(container + ' #alpError').hide();
        }
        if (typeof msgWarning != 'undefined' && msgWarning != '') {
            $(container + ' #msgPopupWarning').html(msgWarning);
            $(container + ' #alpWarning').show();
            show = true;
        } else {
            $(container + ' #msgPopupWarning').text('');
            $(container + ' #alpWarning').hide();
        }
        if (typeof msgSuccess != 'undefined' && msgSuccess != '') {
            $(container + ' #msgPopupSuccess').html(msgSuccess);
            $(container + ' #alpSuccess').show();
            show = true;
        } else {
            $(container + ' #msgPopupSuccess').html('');
            $(container + ' #alpSuccess').hide();
        }
        if (typeof msgInfo != 'undefined' && msgInfo != '') {
            $(container + ' #msgPopupInfo').html(msgInfo);
            $(container + ' #alpInfo').show();
            show = true;
        } else {
            $(container + ' #msgPopupInfo').html('');
            $(container + ' #alpInfo').hide();
        }
        if (show == true) {
            $(container + ' #alert-popup-bar').show().delay(3000).queue(function (n) {
                $(this).hide();
                n();
            });
        }
    }
}

function ResetAlertInPopup() {

    $('#msgPopupError').text('');
    $('#alpError').hide();

    $('#msgPopupWarning').text('');
    $('#alpWarning').hide();

    $('#msgPopupSuccess').text('');
    $('#alpSuccess').hide();

    $('#msgPopupInfo').text('');
    $('#alpInfo').hide();

    $('#alert-popup-bar').hide();
}

function ShowAlertInNewPopup(msgError, msgWarning, msgSuccess, msgInfo) {
    var show = false;
    if (typeof msgError != 'undefined' && msgError != '') {
        $('#mPopupError').text(msgError);
        $('#pError').show();
        show = true;
    } else {
        $('#mPopupError').text('');
        $('#pError').hide();
    }
    if (typeof msgWarning != 'undefined' && msgWarning != '') {
        $('#mPopupWarning').text(msgWarning);
        $('#pWarning').show();
        show = true;
    } else {
        $('#mPopupWarning').text('');
        $('#pWarning').hide();
    }
    if (typeof msgSuccess != 'undefined' && msgSuccess != '') {
        $('#mPopupSuccess').text(msgSuccess);
        $('#pSuccess').show();
        show = true;
    }
    else {
        $('#mPopupSuccess').text('');
        $('#pSuccess').hide();
    }
    if (typeof msgInfo != 'undefined' && msgInfo != '') {
        $('#mPopupInfo').text(msgInfo);
        $('#pInfo').show();
        show = true;
    }
    else {
        $('#mPopupInfo').text('');
        $('#pInfo').hide();
    }
    if (show == true) {
        $('#alert-popup-bar').show().delay(5000).queue(function (n) {
            $(this).hide(); n();
        });
    }
}

function ShowAlertWin8(type, msg) {
    GenAlertWin8(type, false, null, null, null, null, msg);
}

function GenAlertWin8(type, sticky, horizontalEdge, verticalEdge, heading, life, msg) {
    if (life == undefined || life == null || life == '')
        life = 5000;
    if (horizontalEdge == undefined || horizontalEdge == null || horizontalEdge == '')
        horizontalEdge = 'top';
    if (verticalEdge == undefined || verticalEdge == null || verticalEdge == '')
        verticalEdge = 'right';
    if (heading == undefined || heading == null || heading == '')
        heading = null;
    if (sticky == undefined || sticky == null || sticky == '')
        sticky = false;
    if (type == WARNING_ERROR) {
        theme = 'ruby';
    }
    else if (type == WARNING) {
        theme = 'lemon';
    }
    else if (type == WARNING_SUCCESS) {
        theme = 'lime';
    }
    else if (type == WARNING_NOTICE) {
        theme = 'teal';
    }
    var settings = {
        theme: theme,
        sticky: sticky,
        horizontalEdge: horizontalEdge,
        verticalEdge: verticalEdge
    },
                    $button = $(this);

    if ($.trim(heading) != '') {
        settings.heading = $.trim(heading);
    }

    if (!settings.sticky) {
        settings.life = life;
    }

    $.notific8('zindex', 11500);
    $.notific8($.trim(msg), settings);

    $button.attr('disabled', 'disabled');

    setTimeout(function () {
        $button.removeAttr('disabled');
    }, 1000);
}

function ShowNewDialog(title, width, content, functionName, type, notClose) {
    var buttonSave = 'Lưu';
    var buttonCancel = 'Đóng';
    if (typeof type != 'undefined' && type == 'normal') {
        buttonSave = 'Lưu';
        buttonCancel = 'Đóng';
    }
    else if (typeof type != 'undefined' && type == 'search') {
        buttonSave = 'Chọn';
        buttonCancel = 'Đóng';
    }
    else if (typeof type != 'undefined' && type == 'confirm') {
        buttonSave = 'Đồng ý';
        buttonCancel = 'Hủy';
    }
    else if (typeof type != 'undefined' && type == 'approved') {
        buttonSave = 'Duyệt';
        buttonCancel = 'Hủy';
    }
    content.dialog({
        width: width,
        modal: true,
        // position: "center",
        dialogClass: _contDialogFormat.CustomStype,
        buttons: [
            {
                text: buttonSave,
                //icons: { primary: "ui-icon-check" },
                'class': 'btn btn-primary',
                click: function () {
                    functionName();
                    if (notClose == undefined || notClose == null || notClose == false) $(this).dialog("close");
                }
            },
            {
                text: buttonCancel,
                //icons: { primary: "ui-icon-closethick" },
                'class': 'btn btn-primary',
                click: function () { $(this).dialog("close"); }
            }
        ]
    });
    //$(".ui-dialog-title").html(title);
    //$(".ui-dialog-titlebar-close").css('background-image', 'url("/Content/image/close.png")');
    //}); ({
    //    'width': width,
    //    'title': title,
    //    'message': content,
    //    'function': functionName,
    //    'textSave': buttonSave,
    //    'textCancel': buttonCancel,
    //    'buttons': {
    //        'Yes': {
    //            'class': 'blue',
    //            'action': function () {
    //                functionName();
    //                if (notClose == undefined || notClose == null || notClose == false) $(this).dialog("close");
    //            }
    //        },
    //        'No': {
    //            'class': 'gray',
    //            'action': function () { $(this).dialog("close"); }
    //        }
    //    }
    //});
    //fixCenterBox('#popupwraper');
}

function ShowNewDialogContent(params) {

    if ($('#confirmOverlay').length) {
        return false;
    }

    var buttonHtml = '';
    $.each(params.buttons, function (name, obj) {

        buttonHtml += '<a href="#" class="button ' + obj['class'] + '">' + name + '<span></span></a>';

        if (!obj.action) {
            obj.action = function () { };
        }
    });

    var markup = [
        '<div class="dialog-container"></div>',
        '<div tabindex="-1" class="modal hide in" aria-hidden="false">',
        '<div id="popupwraper" class="modalpopup">',
'<div class="modal-header">',
    '<button data-dismiss="modal" class="close" onclick="HideNewDialogContent()" type="button">×</button>',
    '<h5 class="blue bigger"><span>', params.title, '</span></h5>',
'</div>',
'<div class="modal-body overflow-visible">',
   '<div class="row-fluid">',
        '<div id="alert-popup-bar" style="display: none;margin-top: 10px;">',
            '<div class="span12" style="margin: 0">',
                '<div id="pError" class="alert alert-error" style="display: none;">',
                    '<strong><i class="icon-remove" style="padding-right:5px"></i>Lỗi!</strong>',
                    '<span id="mPopupError">Xảy ra lỗi trong quá trình xử lý.</span>',
                '</div>',
                '<div id="pWarning" class="alert alert-warning" style="display: none;">',
                    '<strong><i class="icon-warning" style="padding-right:5px"></i>Cảnh báo!</strong>',
                    '<span id="mPopupWarning">Thông tin yêu cầu xử lý có thể phát sinh lỗi.</span>',
                '</div>',
                '<div id="pSuccess" class="alert alert-success" style="display: none;">',
                    '<strong><i class="icon-ok" style="padding-right:5px"></i>Thông báo!</strong>',
                    '<span id="mPopupSuccess">Xử lý dữ liệu thành công.</span>',
                '</div>',
                '<div id="pInfo" class="alert alert-info" style="display: none;">',
                    '<strong><i class="icon-info" style="padding-right:5px"></i>Thông tin!</strong>',
                    '<span id="mPopupInfo">Thông tin mở rộng.</span>',
                '</div>',
            '</div>',
        '</div>',
    '</div>',
    params.message,
'</div>',


'<div class="modal-footer">',
'<button class="btn btn-primary" onclick="', params.function, '()"><i class="icon-ok"></i><span id="sp_save">', params.textSave, '</span></button>',
'<button class="btn btn-primary" onclick="HideNewDialogContent()"><i class="icon-reply"></i><span id="sp_close">', params.textCancel, '</span></button>',
'</div>',
'</div>',
        '</div>'


        //'<div id="confirmOverlay">',
        //'<div id="confirmBox" style="width:', params.width, '">',
        //'<h1>', params.title, '</h1>',
        //params.message,
        //'<div id="confirmButtons">',
        //buttonHtml,
        //'</div></div></div>'
    ].join('');

    $(markup).hide().appendTo('body').fadeIn();

    var buttons = $('#confirmBox .button'),
        i = 0;

    $.each(params.buttons, function (name, obj) {
        buttons.eq(i++).click(function () {

            obj.action();
            $.confirm.hide();
            return false;
        });
    });
};

function HideNewDialogContent() {
    $('.modalpopup').remove();
    $('.dialog-container').remove();
};

function GetOneSelect() {
    var str = "";
    $('input:checkbox[name=chkCheck]').each(function () {
        if (str == "") {
            if ($(this).is(':checked') == true) {
                str = $(this).val();
            }
        }
    });
    return str;
}
function GetMultiSelect() {
    var str = "";
    $('input:checkbox[name=chkCheck]').each(function () {
        if ($(this).is(':checked') == true) {
            str += $(this).val() + "!";
        }
    });
    return str;
}

function GetMultiSelectWithComma() {
    var str = "";
    $('input:checkbox[name=chkCheck]').each(function () {
        if ($(this).is(':checked') == true) {
            str += $(this).val() + ",";
        }
    });
    return str;
}

function GetMultiSelectPopup() {
    var str = "";
    $("#modal-form").find('tbody input[type=checkbox]:checked').each(function () {
        str += $(this).val() + "!";
    });
    return str;
}
function GetIdThisFromPopup() {
    var str = "";
    $("#modal-form").find('tbody input[type=checkbox]:checked').each(function () {
        str += $(this).attr("idThis") + ",";
    });
    return str;
}

function StringReplaceAll(strOgr, strOld, strNew) {
    var index = strOgr.indexOf(strOld);
    while (index != -1) {
        strOgr = strOgr.replace(strOld, strNew);
        index = strOgr.indexOf(strOld);
    }
    return strOgr;
}
function FocusQuantity() {
    $("#txtSL").focus();
    $("#txtSL").select();
}

function GetDateStringNoParam() {
    var da = new Date();
    var y = da.getFullYear().toString();
    var m = (da.getMonth() + 1).toString();
    if (m.length == 1) {
        m = "0" + m;
    }
    var day = da.getDate().toString();
    if (day.length == 1) {
        day = "0" + day;
    }
    return y + m + day;
}

function GetDateString(date) {
    var d = new Date(date);
    var y = d.getFullYear().toString();
    var m = (d.getMonth() + 1).toString();
    if (m.length == 1) {
        m = "0" + m;
    }
    var day = d.getDate().toString();
    if (day.length == 1) {
        day = "0" + day;
    }
    return y + m + day;
}

function GetSubDate() {
    var d = new Date();
    return d.getHours() + ":" + d.getMinutes();
}

function GetDateByDDMMYYYY() {
    var d = new Date();
    var y = d.getFullYear().toString();
    var m = (d.getMonth() + 1).toString();
    if (m.length == 1) {
        m = "0" + m;
    }
    var day = d.getDate().toString();
    if (day.length == 1) {
        day = "0" + day;
    }
    return day + "/" + m + "/" + y;
}

function FormatDateByDDMMYYYYFromString(date) {
    var spl = date.split('T');
    if(spl.length > 0) {
        var dt = spl[0].split('-');
        return dt[2] + '/' + dt[1] + '/' + dt[0];
    }
    return '';
}

function ConvertDateToInt(date, type) {
    if(type == 0) {
        var spl = date.split('T');
        if (spl.length > 0) {
            var dt = spl[0].split('-');
            return parseInt(dt[0] + dt[1] + dt[2]);
        }
    }
    var spl1 = date.split('/');
    if (spl1.length > 0) {
        return parseInt(spl1[2] + spl1[1] + spl1[0]);
    }
    return 0;
}

function GetDateStringFull(date) {
    var d = new Date(date);
    var y = d.getFullYear().toString();
    var m = (d.getMonth() + 1).toString();
    if (m.length == 1) {
        m = "0" + m;
    }
    var day = d.getDate().toString();
    if (day.length == 1) {
        day = "0" + day;
    }
    return y + "/" + m + "/" + day + " " + d.getHours() + ":" + d.getMinutes();
}

function FormatDateByNum(date) {
    var d = new Date(date);
    var y = d.getFullYear().toString();
    var m = (d.getMonth() + 1).toString();
    if (m.length == 1) {
        m = "0" + m;
    }
    var day = d.getDate().toString();
    if (day.length == 1) {
        day = "0" + day;
    }
    return parseFloat(y + m + day);
}

function GetDateStringFormatStandardFull(date) {
    var d = new Date(date);
    var y = d.getFullYear().toString();
    var m = (d.getMonth() + 1).toString();
    if (m.length == 1) {
        m = "0" + m;
    }
    var day = d.getDate().toString();
    if (day.length == 1) {
        day = "0" + day;
    }
    return day + "/" + m + "/" + y + " " + d.getHours() + ":" + d.getMinutes();
}

function remove_unicode(str) {
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/!|@@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "");
    str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1- 
    str = replaceSpecialChar(str, "");
    return str;
}
function replaceSpecialChar(str, c) {
    str = str.replace(/!|@@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|$|_/g, c);
    str = str.replace(/\-/g, "");
    return str;
}

function ValidPhone(phoneNumber) {
    var mess = '';
    if (phoneNumber == '') {
        mess = 'Điện thoại không được trống';
        return mess;
    }
    if (phoneNumber.length >= 15) {
        mess = 'Điện thoại không được lớn hơn 15 ký tự';
        return mess;
    }
    var phoneNumberPattern = /^[0-9 .-]+$/;//^\(?([0-9]{5})\)?([ .-]?)([0-9]{3})\2([0-9]{3})$/;
    var check = phoneNumberPattern.test(phoneNumber);
    if (!check) {
        mess = 'Điện thoại không hợp lệ';
        return mess;
    }
    return mess;
}

function ShowPopupCacheAutoWith(idModel, idContainer) {
    fixCenterBox('#' + idModel);
    $('#' + idContainer).show();
    $('#' + idModel).show();
}
function ShowPopupCache(idModel, idContainer, widthPopup) {
    $('#' + idModel).css({ 'width': widthPopup + 'px' });
    fixCenterBox('#' + idModel);
    $('#' + idContainer).show();
    $('#' + idModel).show();
}
function HidePopupCache(idModel, idContainer) {
    $('#' + idContainer).hide();
    $('#' + idModel).hide();
}

function CheckDiv(id) {
    var sp = $('#' + id).children();
    var checked = sp.attr('class');
    if (checked == undefined || checked == '') {
        sp.addClass("checked");
        sp.children().prop('checked', true);

    } else {
        sp.removeClass("checked");
        sp.children().prop('checked', false);
    }

}