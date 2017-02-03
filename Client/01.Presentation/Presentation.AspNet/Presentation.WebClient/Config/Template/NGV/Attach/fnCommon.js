function getObj(name){if (document.getElementById){if(document.getElementById(name))return document.getElementById(name);}else if (document.all){if (document.all[name])return document.all[name];}else if (document.layers){if (document.layers[name])return document.layers[name];} return false} 
function getTextInBox(el){if ('string' == typeof el.innerText) return el.innerText;if ('string' == typeof el.textContent) return el.textContent;return el.innerHTML.replace(/<[^>]*>/g,'')}
function getHTMLInBox(el){if ('string' == typeof el.innerText) return el.innerText;if ('string' == typeof el.textContent) return el.textContent;return el.innerHTML;}
var linkpopup = document.location.href.replace('Default.aspx', 'PopupContent.aspx')
var my_window;
var ChecktoUpdate;

//'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no');
function showpopup(url, width, height) {
    var params = 'width=' + width + ', height=' + height;
    params += ', directories=no';
    params += ', location=no';
    params += ', menubar=no';
    
    params += ', resizable=no';
    params += ', copyhistory=no';
    params += ', scrollbars=no';
    params += ', status=no';
    params += ', toolbar=no';
    var centeredY, centeredX;
    centeredY = window.screenY  + (((window.outerHeight / 2) - (height / 2)))-30;
    centeredX = window.screenX   + (((window.outerWidth / 2) - (width / 2)));
    if (url != '') {
        my_window = window.open(url, 'PopupWindow', params + ',left=' + centeredX + ',top=' + centeredY); //.focus();
        if (!my_window.closed) my_window.focus();
        //  ChecktoUpdate = setInterval('checkisclosed()', 3000)
    }
}

function removeCharN(string, chr) {
    var tstring = "";
    string = '' + string;
    splitstring = string.split(chr);
    for (i = 0; i < splitstring.length; i++)
        tstring += splitstring[i];
    return tstring;
}

function isInteger(s){
	var i;
    for (i = 0; i < s.length; i++){           
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }    
    return true;
}

  
 function removeCharN(string, chr) {
	var tstring = "";
	string = '' + string;
	splitstring = string.split(chr);
	for(i = 0; i < splitstring.length; i++)
	tstring += splitstring[i];
	return tstring;
}
function commaSplit(srcNumber, fieldName) {
var txtNumber = '' + srcNumber;
txtNumber = removeCharN(txtNumber, ",");

var rxSplit = new RegExp('([0-9])([0-9][0-9][0-9][,.])');
var arrNumber = txtNumber.split('.');
arrNumber[0] += '.';
do {
arrNumber[0] = arrNumber[0].replace(rxSplit, '$1,$2');
} while (rxSplit.test(arrNumber[0]));
if (arrNumber.length > 1) {
return arrNumber.join('');
}
else {
return arrNumber[0].split('.')[0];
      }
}

var errmsg = '<br/><div>&nbsp;&nbsp;<i><font color=red>Truy&nbsp;vấn&nbsp;dữ&nbsp;liệu&nbsp;thất&nbsp;bại!</font></i></div>';
var LStr = "<table style='width:100%'><tr><td style='height:30px' align='center'><img height=32px src='/Images/Loading.gif' alt=''></td> </tr><tr><td  align='center'>Đang&nbsp;tải&nbsp;dữ&nbsp;liệu...</td></tr></table>";
var LStrgenrpt = "<table style='width:100%'><tr><td style='height:30px' align='center'><img height=24px src='/Images/Loading.gif' alt=''></td> </tr><tr><td align='center'>Đang&nbsp;truy&nbsp;vấn&nbsp;dữ&nbsp;liệu...</td></tr></table>";

function setlinktree(treeid) {

    var links = getObj(treeid).getElementsByTagName("a");
   
    var inputs = getObj(treeid).getElementsByTagName("input");
    for (var i = 0; i < inputs.length; i++) {
        var hef= links[i].href.split('\\')[2]
        inputs[i].setAttribute("name", hef);
    } 
    for (var i = 0; i < links.length; i++) {
        links[i].setAttribute("href", "javascript:NodeClick(\"" + treeid + "\", \"" + links[i].id + "\")");
    }
}
function SelectAllChecBoxWithName(P) {
    var xState = P.checked;
    var elm = document.getElementsByName(P.name)

    for (i = 0; i < elm.length; i++) {       
            getObj(elm[i].id).checked = xState
    }
    }
    function SelectAllChecBoxWithgroup(P,s) {
        var xState = P.checked;
        var elm = getObj(s).getElementsByTagName('input')

        for (i = 0; i < elm.length; i++) {
            if (elm.type == "checkbox") {
                getObj(elm[i]).checked = xState
            }
        }
    }
function OnTreeClick(evt) {

    var src = window.event != window.undefined ? window.event.srcElement : evt.target;

    var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
    if (isChkBoxClick) {
        var parentTable = GetParentByTagName("table", src);
        var nxtSibling = parentTable.nextSibling;
        if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
        {
            if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
            {
                //check or uncheck children at all levels
                CheckUncheckChildren(parentTable.nextSibling, src.checked);
            }
        }
        //check or uncheck parents at all levels
        CheckUncheckParents(src, src.checked);
    }
}
function NodeClick(treeid, id) {
    var cknid = treeid + 'n' + id.replace(treeid, '').replace('t', '') + 'CheckBox'
    $('#' + cknid).trigger('click');

}
function CheckUncheckChildren(childContainer, check) {
    var childChkBoxes = childContainer.getElementsByTagName("input");
    var childChkBoxCount = childChkBoxes.length;
    for (var i = 0; i < childChkBoxCount; i++) {
        childChkBoxes[i].checked = check;
    }
}

function CheckUncheckParents(srcChild, check) {
    var parentDiv = GetParentByTagName("div", srcChild);
    var parentNodeTable = parentDiv.previousSibling;

    if (parentNodeTable) {
        var checkUncheckSwitch;

        if (check) //checkbox checked
        {
            var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
            if (isAllSiblingsChecked)
                checkUncheckSwitch = true;
            else
                return; //do not need to check parent if any(one or more) child not checked
        }
        else //checkbox unchecked
        {
            checkUncheckSwitch = false;
        }

        var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
        if (inpElemsInParentTable.length > 0) {
            var parentNodeChkBox = inpElemsInParentTable[0];
            parentNodeChkBox.checked = checkUncheckSwitch;
            //do the same recursively
            CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
        }
    }
}

function AreAllSiblingsChecked(chkBox) {
    var parentDiv = GetParentByTagName("div", chkBox);
    var childCount = parentDiv.childNodes.length;
    for (var i = 0; i < childCount; i++) {
        if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
        {
            if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                //if any of sibling nodes are not checked, return false
                if (!prevChkBox.checked) {
                    return false;
                }
            }
        }
    }
    return true;
}

//utility function to get the container of an element by tagname
function GetParentByTagName(parentTagName, childElementObj) {
    var parent = childElementObj.parentNode;
    while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
        parent = parent.parentNode;
    }
    return parent;
}


//for disable rigth click
function clickIE4() {
    if (event.button == 2) {
        alert(message);
        return false;
    }
}
function clickNS4(e) {
    if (document.layers || document.getElementById && !document.all) {
        if (e.which == 2 || e.which == 3) {
            alert(message);
            return false;
        }
    }
}


///

function SelectAllChecBoxWithValue(spanChk, value) {
    var xState = spanChk.checked;
    var theBox = spanChk;

    var elm = document.getElementById(spanChk.id.substring(0, spanChk.id.lastIndexOf('_'))).getElementsByTagName("input");
    for (i = 0; i < elm.length; i++) {
        if (elm[i].type == "checkbox") {// && elm[i].id != theBox.id && i % 4 == value && i >= 4) {
            //elm[i].click();
            if (elm[i].checked != xState)
                elm[i].click();
            //elm[i].checked=xState;
        }
    }
}

