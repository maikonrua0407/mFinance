function getObj(name){if (document.getElementById){if(document.getElementById(name))return document.getElementById(name);}else if (document.all){if (document.all[name])return document.all[name];}else if (document.layers){if (document.layers[name])return document.layers[name];} return false} 
function getTextInBox(el){if ('string' == typeof el.innerText) return el.innerText;if ('string' == typeof el.textContent) return el.textContent;return el.innerHTML.replace(/<[^>]*>/g,'')}
function getHTMLInBox(el){if ('string' == typeof el.innerText) return el.innerText;if ('string' == typeof el.textContent) return el.textContent;return el.innerHTML;}

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
txtNumber = removeChar(txtNumber, ",");

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