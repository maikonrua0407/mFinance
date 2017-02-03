//process hotkey
var isCtrlKey = false;

document.onkeyup = function (e) { if (e.which == 17) isCtrlKey = false; };
document.onkeydown = function (e) {
    if (e.which == 17) isCtrlKey = true;
    //F1 -- help
    if (e.which == 112) {
        $('#btnHelp').click();
        StopDefaultAction(e);
    }
    //F3 -- focus khách hàng
    if (e.which == 114 && isCtrlKey == false) {
        $('#txtKH').focus();
        StopDefaultAction(e);
    }
    //F4 -- focus nhà cung cấp
    if (e.which == 115 && isCtrlKey == false) {
        $('#txtNcc').focus();
        StopDefaultAction(e);
    }
    //F6 -- check tồn kho
    if (e.which == 117) {
        ShowTonKho();
        StopDefaultAction(e);
    }
    //F8 -- In phiếu
    if (e.which == 119) {
        if ($('#printProcess').html() != undefined && $('#printProcess').html() != '') {
            printProcess();
        }
        StopDefaultAction(e);
    }
    //F9 -- quay lại danh sách
    if (e.which == 120) {
        if ($('#listProcess').html() != undefined && $('#listProcess').html() != '') {
            listProcess();
        }
        StopDefaultAction(e);
    }
    //F10 -- thêm phiếu mới
    if (e.which == 121) {
        if ($('#createProcess').html() != undefined && $('#createProcess').html() != '') {
            createProcess();
        }
        StopDefaultAction(e);
    }
    //F12 -- focus mặt hàng
    if (e.which == 118) {
        $('#txtMaHang').focus();
        StopDefaultAction(e);
    }
    //CTRL + S --save phiếu
    if (e.which == 83 && isCtrlKey == true) {
        if ($('#saveProcess').html() != undefined && $('#saveProcess').html() != '') {
            saveProcess();
        }
        StopDefaultAction(e);
    }
    //CTRL + I --save phiếu & in
    if (e.which == 74 && isCtrlKey == true) {
        if ($('#saveprintProcess').html() != undefined && $('#saveprintProcess').html() != '') {
            saveprintProcess();
        }
        StopDefaultAction(e);
    }
    //CTRL + F -- tim kiem
    if (e.which == 70 && isCtrlKey == true) {
        if ($('#searchProcess').html() != undefined && $('#searchProcess').html() != '') {
            searchProcess();
        }
        StopDefaultAction(e);
    }
    
    //downarrow --down row table popup
    if (e.which == 40) {
        selectRowTable("DOWN");
        StopDefaultAction(e);
    }
    
    if (e.which == 38) {
        selectRowTable("UP");
        StopDefaultAction(e);
    }
    
    //F2
    if (e.which == 113) {
        var idTable = $('table[selector=tbl]').attr('id');
        if (idTable != undefined && idTable != null) {
            var rowSelected = $('table[selector=tbl]').find('tr[class=highlightedtd]').attr('id');
            $('#' +rowSelected).dblclick();
        }
        StopDefaultAction(e);
    }

    function selectRowTable(obj) {
        if ($('table[selector=tbl]').html() != undefined && $('table[selector=tbl]').html() != null && $('table[selector=tbl] tr').length > 0) {
            var rowSelected = $('table[selector=tbl]').find('tr[class=highlightedtd]');
            var anchorSelected = rowSelected.attr('anchor');
            //split value and increment index
            var num = parseInt(anchorSelected.split('-')[1]);
            if (obj == "DOWN") num++; else num--;
            var rowNew = anchorSelected.split('-')[0] + '-' + num.toString();
            var rowCheck = $('table[selector=tbl]').find('tr[anchor=' + rowNew + ']').attr('id');
            if (rowCheck != null && rowCheck != undefined) {
                rowSelected.removeClass("highlightedtd");
                var crow = $('table[selector=tbl]').find('tr[anchor=' + rowNew + ']');
                crow.addClass("highlightedtd");
                var dv = crow.parent().parent().parent().get(0);
                if (dv.tagName == 'DIV') {
                    var rTop = crow.position().top;
                    var divSTop = dv.scrollTop;
                    var divHeight = $(dv).height();
                    var cHeight = $(crow).height();
                    var newTop = rTop + cHeight;
                    if (newTop > divHeight) {
                        dv.scrollTop = divSTop + newTop - divHeight;
                    } else if (rTop < 0) {
                        dv.scrollTop = divSTop + rTop;
                    }
                }
            }
        }
    }
    
    //left-right arrow -- chuyen trang
    if (e.which == 37) {
        navigatePage("LEFT");
        StopDefaultAction(e);
    }

    if (e.which == 39) {
        navigatePage("RIGHT");
        StopDefaultAction(e);
    }

    function navigatePage(obj) {
        if ($('#data').html() != undefined && $('#data').html() != null && $('#pageContainer ul').length > 0) {
            var currentTag = $('#pageContainer ul').find('li[class=active]').find('a');
            var currentAnchor = currentTag.attr('anchor');
            //split value and increment index
            var num = parseInt(currentAnchor.split('-')[1]);
            if (obj == "RIGHT") num++; else num--;
            var rowNew = currentAnchor.split('-')[0] + '-' + num.toString();
            var rowCheck = $('#pageContainer ul li').find('a[anchor=' + rowNew + ']').attr('id');
            if (rowCheck != undefined && rowCheck != null) {
                $('#' + rowCheck).click();
            }
        }
    }

    //ESC -- thoát popup
    if (e.which == 27) {
        PopupSearchCancel();
        StopDefaultAction(e);
    }
    return true;
};



function StopDefaultAction(e) {
    if (e.preventDefault) {
        e.preventDefault();
    }
    else {
        e.stop();
    };
    e.returnValue = false;
    e.stopPropagation();
}