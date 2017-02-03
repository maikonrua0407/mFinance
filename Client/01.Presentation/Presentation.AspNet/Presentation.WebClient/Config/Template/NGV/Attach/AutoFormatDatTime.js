
<!-- Begin
// Check browser version
var isNav4 = false, isNav5 = false, isIE4 = false
var strSeperator = "/"; 
// If you are using any Java validation on the back side you will want to use the / because 
// Java date validations do not recognize the dash as a valid date separator.
var vDateType = 3; // Global value for type of date format
//                1 = mm/dd/yyyy
//                2 = yyyy/dd/mm  (Unable to do date check at this time)
//                3 = dd/mm/yyyy
var vYearType = 4; //Set to 2 or 4 for number of digits in the year for Netscape
var vYearLength = 2; // Set to 4 if you want to force the user to enter 4 digits for the year before validating.
var err = 0; // Set the error code to a default of zero
if(navigator.appName == "Netscape") 
{
    if (navigator.appVersion < "5") 
    {
        isNav4 = true;
        isNav5 = false;
    }
    else
        if (navigator.appVersion > "4") 
    {
        isNav4 = false;
        isNav5 = true;
    }
}
else 
{
    isIE4 = true;
}

function DateFormat(vDateName, vDateValue, e, dateCheck, dateType) {
vDateType = dateType;
// vDateName = object name
// vDateValue = value in the field being checked
// e = event
// dateCheck 
// True  = Verify that the vDateValue is a valid date
// False = Format values being entered into vDateValue only
// vDateType
// 1 = mm/dd/yyyy
// 2 = yyyy/mm/dd
// 3 = dd/mm/yyyy
//Enter a tilde sign for the first number and you can check the variable information.
if (vDateValue == "~") {
alert("AppVersion = "+navigator.appVersion+" \nNav. 4 Version = "+isNav4+" \nNav. 5 Version = "+isNav5+" \nIE Version = "+isIE4+" \nYear Type = "+vYearType+" \nDate Type = "+vDateType+" \nSeparator = "+strSeperator);
vDateName.value = "";
vDateName.focus();
return true;
}
var whichCode = (window.Event) ? e.which : e.keyCode;
// Check to see if a seperator is already present.
// bypass the date if a seperator is present and the length greater than 8
if (vDateValue.length > 8 && isNav4) {
if ((vDateValue.indexOf("-") >= 1) || (vDateValue.indexOf("/") >= 1))
return true;
}
//Eliminate all the ASCII codes that are not valid
var alphaCheck = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ/-";
if (alphaCheck.indexOf(vDateValue) >= 1) {
if (isNav4) {
vDateName.value = "";
vDateName.focus();
vDateName.select();
return false;
}
else {
vDateName.value = vDateName.value.substr(0, (vDateValue.length-1));
return false;
   }
}
if (whichCode == 8 || whichCode == 13 || whichCode == 81 ) //Ignore the Netscape value for backspace. IE has no value
return false;
else {
//Create numeric string values for 0123456789/
//The codes provided include both keyboard and keypad values
var strCheck = '47,48,49,50,51,52,53,54,55,56,57,58,59,95,96,97,98,99,100,101,102,103,104,105,16,17,37,38,39,40';
/*
if (whichCode > 0)
{ 
    //alert(whichCode);
    //alert(strCheck.indexOf(whichCode));    
}
*/

if (strCheck.indexOf(whichCode) != -1) 
{
if (isNav4) {
if (((vDateValue.length < 6 && dateCheck) || (vDateValue.length == 7 && dateCheck)) && (vDateValue.length >=1)) 
{
alert("Invalid Date\nPlease Re-Enter");
vDateName.value = "";
vDateName.focus();
vDateName.select();
return false;
}
if (vDateValue.length == 6 && dateCheck) {
var mDay = vDateName.value.substr(2,2);
var mMonth = vDateName.value.substr(0,2);
var mYear = vDateName.value.substr(4,4)
//Turn a two digit year into a 4 digit year
if (mYear.length == 2 && vYearType == 4) {
var mToday = new Date();
//If the year is greater than 30 years from now use 19, otherwise use 20
var checkYear = mToday.getFullYear() + 30; 
var mCheckYear = '20' + mYear;
if (mCheckYear >= checkYear)
mYear = '19' + mYear;
else
mYear = '20' + mYear;
}
var vDateValueCheck = mMonth+strSeperator+mDay+strSeperator+mYear;
if (!dateValid(vDateValueCheck)) {
alert("Invalid Date\nPlease Re-Enter");
vDateName.value = "";
vDateName.focus();
vDateName.select();
return false;
}
return true;
}
else {
// Reformat the date for validation and set date type to a 1
if (vDateValue.length >= 8  && dateCheck) {
if (vDateType == 1) // mmddyyyy
{
var mDay = vDateName.value.substr(2,2);
var mMonth = vDateName.value.substr(0,2);
var mYear = vDateName.value.substr(4,4)
vDateName.value = mMonth+strSeperator+mDay+strSeperator+mYear;
}
if (vDateType == 2) // yyyymmdd
{
var mYear = vDateName.value.substr(0,4)
var mMonth = vDateName.value.substr(4,2);
var mDay = vDateName.value.substr(6,2);
vDateName.value = mYear+strSeperator+mMonth+strSeperator+mDay;
}
if (vDateType == 3) // ddmmyyyy
{
var mMonth = vDateName.value.substr(2,2);
var mDay = vDateName.value.substr(0,2);
var mYear = vDateName.value.substr(4,4)
vDateName.value = mDay+strSeperator+mMonth+strSeperator+mYear;
}
//Create a temporary variable for storing the DateType and change
//the DateType to a 1 for validation.
var vDateTypeTemp = vDateType;
vDateType = 1;
var vDateValueCheck = mMonth+strSeperator+mDay+strSeperator+mYear;
if (!dateValid(vDateValueCheck)) {
alert("Invalid Date\nPlease Re-Enter");
vDateType = vDateTypeTemp;
vDateName.value = "";
vDateName.focus();
vDateName.select();
return false;
}
vDateType = vDateTypeTemp;
return true;
}
else {
if (((vDateValue.length < 8 && dateCheck) || (vDateValue.length == 9 && dateCheck)) && (vDateValue.length >=1)) {
alert("Invalid Date\nPlease Re-Enter");
vDateName.value = "";
vDateName.focus();
vDateName.select();
return false;
         }
      }
   }
}
else {
// Non isNav Check
if (((vDateValue.length < 8 && dateCheck) || (vDateValue.length == 9 && dateCheck)) && (vDateValue.length >=1)) {
alert("Invalid Date\nPlease Re-Enter");
vDateName.value = "";
vDateName.focus();
return true;
}
// Reformat date to format that can be validated. mm/dd/yyyy
if (vDateValue.length >= 8 && dateCheck) {
// Additional date formats can be entered here and parsed out to
// a valid date format that the validation routine will recognize.
if (vDateType == 1) // mm/dd/yyyy
{
var mMonth = vDateName.value.substr(0,2);
var mDay = vDateName.value.substr(3,2);
var mYear = vDateName.value.substr(6,4)
}
if (vDateType == 2) // yyyy/mm/dd
{
var mYear = vDateName.value.substr(0,4)
var mMonth = vDateName.value.substr(5,2);
var mDay = vDateName.value.substr(8,2);
}
if (vDateType == 3) // dd/mm/yyyy
{
var mDay = vDateName.value.substr(0,2);
var mMonth = vDateName.value.substr(3,2);
var mYear = vDateName.value.substr(6,4)
}
if (vYearLength == 4) {
if (mYear.length < 4) {
alert("Invalid Date\nPlease Re-Enter");
vDateName.value = "";
vDateName.focus();
return true;
   }
}
// Create temp. variable for storing the current vDateType
var vDateTypeTemp = vDateType;
// Change vDateType to a 1 for standard date format for validation
// Type will be changed back when validation is completed.
vDateType = 1;
// Store reformatted date to new variable for validation.
var vDateValueCheck = mMonth+strSeperator+mDay+strSeperator+mYear;
if (mYear.length == 2 && vYearType == 4 && dateCheck) {
//Turn a two digit year into a 4 digit year
var mToday = new Date();
//If the year is greater than 30 years from now use 19, otherwise use 20
var checkYear = mToday.getFullYear() + 30; 
var mCheckYear = '20' + mYear;
if (mCheckYear >= checkYear)
mYear = '19' + mYear;
else
mYear = '20' + mYear;
vDateValueCheck = mMonth+strSeperator+mDay+strSeperator+mYear;
// Store the new value back to the field.  This function will
// not work with date type of 2 since the year is entered first.
if (vDateTypeTemp == 1) // mm/dd/yyyy
vDateName.value = mMonth+strSeperator+mDay+strSeperator+mYear;
if (vDateTypeTemp == 3) // dd/mm/yyyy
vDateName.value = mDay+strSeperator+mMonth+strSeperator+mYear;
} 
if (!dateValid(vDateValueCheck)) {
alert("Invalid Date\nPlease Re-Enter");
vDateType = vDateTypeTemp;
vDateName.value = "";
vDateName.focus();
return true;
}
vDateType = vDateTypeTemp;
return true;
}
else {
if (vDateType == 1) {
if (vDateValue.length == 2) {
vDateName.value = vDateValue+strSeperator;
}
if (vDateValue.length == 5) {
vDateName.value = vDateValue+strSeperator;
   }
}
if (vDateType == 2) {
if (vDateValue.length == 4) {
vDateName.value = vDateValue+strSeperator;
}
if (vDateValue.length == 7) {
vDateName.value = vDateValue+strSeperator;
   }
} 
if (vDateType == 3) {
if (vDateValue.length == 2) {
vDateName.value = vDateValue+strSeperator;
}
if (vDateValue.length == 5) {
vDateName.value = vDateValue+strSeperator;
   }
}
// AutoformatTime
if (vDateType == 4) {
if (vDateValue.length == 2) {
vDateName.value = vDateValue+ ":";
}
}
return true;
   }
}
if (vDateValue.length == 10&& dateCheck) {
if (!dateValid(vDateName)) {
// Un-comment the next line of code for debugging the dateValid() function error messages
//alert(err);  
alert("Invalid Date\nPlease Re-Enter");
vDateName.focus();
vDateName.select();
   }
}
return false;
}
else {
// If the value is not in the string return the string minus the last
// key entered.
if (isNav4) {
vDateName.value = "";
vDateName.focus();
vDateName.select();
return false;
}
else
{
//vDateName.value = vDateName.value.substr(0, (vDateValue.length-1));
return false;
         }
      }
   }
}
function dateValid(objName) 
{
    var strDate;
    var strDateArray;
    var strDay;
    var strMonth;
    var strYear;
    var intday;
    var intMonth;
    var intYear;
    var booFound = false;
    var datefield = objName;
    var strSeparatorArray = new Array("-"," ","/",".");
    var intElementNr;
    // var err = 0;
    var strMonthArray = new Array(12);
    strMonthArray[0] = "Jan";
    strMonthArray[1] = "Feb";
    strMonthArray[2] = "Mar";
    strMonthArray[3] = "Apr";
    strMonthArray[4] = "May";
    strMonthArray[5] = "Jun";
    strMonthArray[6] = "Jul";
    strMonthArray[7] = "Aug";
    strMonthArray[8] = "Sep";
    strMonthArray[9] = "Oct";
    strMonthArray[10] = "Nov";
    strMonthArray[11] = "Dec";
    //strDate = datefield.value;
    strDate = objName;
    if (strDate.length < 1) 
    {
        return true;
    }
    for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) {
    if (strDate.indexOf(strSeparatorArray[intElementNr]) != -1) {
    strDateArray = strDate.split(strSeparatorArray[intElementNr]);
    if (strDateArray.length != 3) {
    err = 1;
    return false;
    }
    else {
    strDay = strDateArray[0];
    strMonth = strDateArray[1];
    strYear = strDateArray[2];
    }
    booFound = true;
       }
    }
    if (booFound == false) {
    if (strDate.length>5) {
    strDay = strDate.substr(0, 2);
    strMonth = strDate.substr(2, 2);
    strYear = strDate.substr(4);
       }
    }
    //Adjustment for short years entered
    if (strYear.length == 2) {
    strYear = '20' + strYear;
    }
    strTemp = strDay;
    strDay = strMonth;
    strMonth = strTemp;
    intday = parseInt(strDay, 10);
    if (isNaN(intday)) {
    err = 2;
    return false;
    }
    intMonth = parseInt(strMonth, 10);
    if (isNaN(intMonth)) {
    for (i = 0;i<12;i++) {
    if (strMonth.toUpperCase() == strMonthArray[i].toUpperCase()) {
    intMonth = i+1;
    strMonth = strMonthArray[i];
    i = 12;
       }
    }
    if (isNaN(intMonth)) {
    err = 3;
    return false;
       }
    }
    intYear = parseInt(strYear, 10);
    if (isNaN(intYear)) {
    err = 4;
    return false;
    }
    if (intMonth>12 || intMonth<1) {
    err = 5;
    return false;
    }
    if ((intMonth == 1 || intMonth == 3 || intMonth == 5 || intMonth == 7 || intMonth == 8 || intMonth == 10 || intMonth == 12) && (intday > 31 || intday < 1)) {
    err = 6;
    return false;
    }
    if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && (intday > 30 || intday < 1)) {
    err = 7;
    return false;
    }
    if (intMonth == 2) {
    if (intday < 1) {
    err = 8;
    return false;
    }
    if (LeapYear(intYear) == true) {
    if (intday > 29) {
    err = 9;
    return false;
       }
    }
    else {
    if (intday > 28) {
    err = 10;
    return false;
          }
       }
    }
    return true;
}

function LeapYear(intYear) 
{
    if (intYear % 100 == 0) 
    {
        if (intYear % 400 == 0) { return true; }
    }
    else 
    {
        if ((intYear % 4) == 0) { return true; }
    }
    return false;
}


function isTime(vDateName, vDateValue) 
{
    var errors = "";
    var regex=/^(2[0-3])|[01][0-9]:[0-5][0-9]$/;
    //alert(vDateValue);
    if(vDateValue!="") 
    {
        if(!regex.test(vDateValue)) 
        {
            errors += vDateValue+" Sai định dạng giờ (HH:MM).<BR>";
            alert( errors);
        }
    }
//alert errors;
}
function AscciiDisable()
{
    var whichCode = (window.Event) ? event.which : event.keyCode;
    if (whichCode < 48 || whichCode > 57)
        event.returnValue =false;
}
//  End -->




/************************/
function SelectAllCheckboxes(spanChk){
	
	// Added as ASPX uses SPAN for checkbox 
	var xState=spanChk.checked;
	var theBox=spanChk;
	
		elm=theBox.form.elements;
		for(i=0;i<elm.length;i++)
		if(elm[i].type=="checkbox" && elm[i].id!=theBox.id)
			{
			//elm[i].click();
			if(elm[i].checked!=xState)
			elm[i].click();
			//elm[i].checked=xState;
			}
	}

	//-------------------------------------------------------------
	//----Select highlish rows when the checkboxes are selected
	//
	// Note: The colors are hardcoded, however you can use 
	//       RegisterClientScript blocks methods to use Grid's
	//       ItemTemplates and SelectTemplates colors.
	//		 for ex: grdEmployees.ItemStyle.BackColor OR
	//				 grdEmployees.SelectedItemStyle.BackColor
	//-------------------------------------------------------------
	function HighlightRow(chkB)	{
	
	// Old code for VS 2002 / .NET framework 1.0 code
	//-----------------------------------------------
	// In .NET 1.0 ASP.NET was using SPAN tag with
	// CheckBox control. 
	//var oItem = chkB.children;
	//xState=oItem.item(0).checked;
	var xState=chkB.checked;
		
	if(xState)
		{chkB.parentElement.parentElement.style.backgroundColor='lightcoral';  // grdEmployees.SelectedItemStyle.BackColor
		 chkB.parentElement.parentElement.style.color='white'; // grdEmployees.SelectedItemStyle.ForeColor
		}else 
		{chkB.parentElement.parentElement.style.backgroundColor='white'; //grdEmployees.ItemStyle.BackColor
		 chkB.parentElement.parentElement.style.color='black'; //grdEmployees.ItemStyle.ForeColor
		}
	}
function everything(form) {
	is_checkdate_begin(form)
	is_checkdate_end(form)
	is_checktime_begin(form)
	is_checktime_end(form)
	allblanks(form)
}
function allblanks(form) {
	if((is_checkdate_begin(form) && is_checkdate_end(form)) && (is_checktime_begin(form) && is_checktime_end(form))) {
	form.submit()
}
	if((is_checkdate_begin(form) == false || is_checkdate_end(form) == false) || (is_checktime_begin(form) == false || is_checktime_end(form) == false)) {
	compose(form)
   }
}
function compose(form) {
	var text = "Cac thong tin ban nen kiem tra lai:"	
	if(is_checkdate_begin(form) == false) {
	text += "\n + Ngay bat dau"
	}
	if(is_checkdate_end(form) == false) {
	text += "\n + Ngay ket thuc"
	}
	if(is_checktime_begin(form) == false) {
	text += "\n + Thoi diem bat dau"
	}
	if (is_checktime_end(form) == false) {
	text += "\n + Thoi diem ket thuc"
	}
	alert(text)
}
// Kiem tra ngay thang cua Begin
function is_checkdate_begin() {
var err=0
a=document.a_log.fDateBegin.value
if (a==""){
	return true
	}
			if (a.length != 10) err=1
				b = a.substring(0, 2)// day
				c = a.substring(2, 3)// '/'
				d = a.substring(3, 5)// month
				e = a.substring(5, 6)// '/'
				f = a.substring(6, 10)// year
			if (b<1 || b>31) err = 1
			if (c != '/') err = 1
			if (d<1 || d>12) err = 1
			if (e != '/') err = 1
			if (f<0 || f>9999) err = 1
			
			if (d==4 || d==6 || d==9 || d==11){
					if (b==31) err=1
			}
			if (d==2){
				var g=parseInt(f/4)
					if (isNaN(g)) {
					err=1
					}
				if (b>29) err=1
				if (b==29 && ((f/4)!=parseInt(f/4))) err=1
			}
	if (err==1) {
		//alert('Bạn nhập sai định dạng ngày tháng!');
		//document.a_log.fDateBegin.focus();		
		return false
	}
	else{
		return true
		//document.a_log.submit();
    }
}

//Kiem tra ngay thang cua ngay ket thuc
function is_checkdate_end() {
var err=0
a=document.a_log.fDateEnd.value
if (a==""){
	return true
	}
			if (a.length != 10) err=1
				b = a.substring(0, 2)// day
				c = a.substring(2, 3)// '/'
				d = a.substring(3, 5)// month
				e = a.substring(5, 6)// '/'
				f = a.substring(6, 10)// year
			if (b<1 || b>31) err = 1
			if (c != '/') err = 1
			if (d<1 || d>12) err = 1
			if (e != '/') err = 1
			if (f<0 || f>9999) err = 1
			
			if (d==4 || d==6 || d==9 || d==11){
					if (b==31) err=1
			}
			if (d==2){
				var g=parseInt(f/4)
					if (isNaN(g)) {
					err=1
					}
				if (b>29) err=1
				if (b==29 && ((f/4)!=parseInt(f/4))) err=1
			}
	if (err==1) {
		//alert('Bạn nhập sai định dạng ngày tháng!');
		//document.a_log.fDateEnd.focus();		
		return false
	}
	else{
		return true
		//document.a_log.submit();
   }
}

//Kiem tra thoi diem bat dau
function is_checktime_begin() {
var err=0

a=document.a_log.fTimeBegin.value
if (a==""){
	return true
	}

			if (a.length != 8) err=1
				b = a.substring(0, 2)// hour
				c = a.substring(2, 3)// ':'
				d = a.substring(3, 5)// minute
				e = a.substring(5, 6)// ':'
				f = a.substring(6, 8)// second
			if (b<0 || b>24) err = 1
			if (c != ':') err = 1
			if (d<0 || d>60) err = 1
			if (e != ':') err = 1
			if (f<0 || f>60) err = 1
			
	if (err==1) {
		//alert('Bạn nhập sai định dạng thời gian!');
		//document.a_log.fTimeBegin.focus();		
		return false
	}
	else{
		return true
		//document.a_log.submit();
    }
} 

//Kiem tra thoi diem ket thuc
function is_checktime_end() {
var err=0

a=document.a_log.fTimeEnd.value
if (a==""){
	return true
	}

			if (a.length != 8) err=1
				b = a.substring(0, 2)// hour
				c = a.substring(2, 3)// ':'
				d = a.substring(3, 5)// minute
				e = a.substring(5, 6)// ':'
				f = a.substring(6, 8)// second
			if (b<0 || b>24) err = 1
			if (c != ':') err = 1
			if (d<0 || d>60) err = 1
			if (e != ':') err = 1
			if (f<0 || f>60) err = 1
			
	if (err==1) {
		//alert('Bạn nhập sai định dạng thời gian!');
		//document.a_log.fTimeEnd.focus();		
		return false
	}
	else{
		return true
		//document.a_log.submit();
    }
} 

function SelectAllCheckboxesOptionParrent(spanChk,index){			
		
		
		var xState=spanChk.checked;
		var childId="_ctl0_MainContent_GroupAssignControl__ctl0_grdGroupAssign__ctl"+index+"_ckRoleR"
		childCheckbox=document.getElementById(childId)
		if (childCheckbox!=null)
		if(xState) document.getElementById(childId).checked='checked'
		else document.getElementById(childId).checked=''
		
		childId="_ctl0_MainContent_GroupAssignControl__ctl0_grdGroupAssign__ctl"+index+"_ckRoleW"
		childCheckbox=document.getElementById(childId)
		if (childCheckbox!=null)
		if(xState) document.getElementById(childId).checked='checked'
		else document.getElementById(childId).checked=''
		
		childId="_ctl0_MainContent_GroupAssignControl__ctl0_grdGroupAssign__ctl"+index+"_ckRoleD"
		childCheckbox=document.getElementById(childId)
		if (childCheckbox!=null)
		if(xState) document.getElementById(childId).checked='checked'
		else document.getElementById(childId).checked=''
		
			
	}
function setDefaultMultiValue(ctr_Original,ctr_Copy1,ctr_Copy2,ctr_Copy3,ctr_Copy4,ctr_Copy5)
{	
//    alert(ctr_Copy1);
//    return;
    document.getElementById(ctr_Copy1).value=document.getElementById(ctr_Original).value;
    document.getElementById(ctr_Copy2).value=document.getElementById(ctr_Original).value;
    document.getElementById(ctr_Copy3).value=document.getElementById(ctr_Original).value;
    document.getElementById(ctr_Copy4).value=document.getElementById(ctr_Original).value;
    document.getElementById(ctr_Copy5).value=document.getElementById(ctr_Original).value;
}
//Kien 16/04/2009
	//Script check Validdate
	function isValidDate(source, arguments) 
  {
      if(arguments.Value != "")
      {
         var datePat = /^(\d{1,2})(\/|-)(\d{1,2})\2(\d{4})$/;

         var matchArray = (arguments.Value).match(datePat); 
         if (matchArray == null) 
         {
          arguments.IsValid = false;
         } 
         else
         {
             day = matchArray[1];
             month = matchArray[3];
             year = matchArray[4];
             if (month < 1 || month > 12) {


                 arguments.IsValid = false;
             }else if (day < 1 || day > 31) {

                 arguments.IsValid = false;
             } else if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31) {

                 arguments.IsValid = false;
             }else if (month == 2) {
                 var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
                 if (day > 29 || (day == 29 && !isleap)) {

                     arguments.IsValid = false;
                 }
             }else
             arguments.IsValid = true;
         }
     }else
     {
      arguments.IsValid = true;
     }
     
  }
  
  var lastColor; 
    function DG_changeBackColor(row, highlight) 
    { 
        if (highlight) 
        { 
            row.style.cursor = "hand"; 
            lastColor = row.style.backgroundColor; 
            row.style.backgroundColor = '#545454'; 
        } 
        else 
            row.style.backgroundColor = lastColor; 
    }
/*question*/



