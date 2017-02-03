function startclock()
{
	var curTime=new Date();
	var nhours=curTime.getHours();
	var nmins=curTime.getMinutes();
	var nsecn=curTime.getSeconds();
	var nday=curTime.getDay();
	var nmonth=curTime.getMonth();
	var ntoday=curTime.getDate();
	var nyear=curTime.getYear();
	var AMorPM=" ";

//	if (nhours>=12)
//		AMorPM="P.M.";
//	else
//		AMorPM="A.M.";

//	if (nhours>=13)
//		nhours-=12;

	if (nhours==0)
		nhours=0;

	if (nsecn<10)
		nsecn="0"+nsecn;

	if (nmins<10)
		nmins="0"+nmins;

	if (nday==0)
		nday="Ch&#7911; Nh&#7853;t";
	if (nday==1)
		nday="Th&#7913; Hai"; 
	if (nday==2)
		nday="Th&#7913; Ba"; 
	if (nday==3)
		nday="Th&#7913; T&#432;";
	if (nday==4)
		nday="Th&#7913; N&#259;m";
	if (nday==5)
		nday="Th&#7913; S&#225;u";
	if (nday==6)
		nday="Th&#7913; B&#7843;y";


	nmonth+=1;

	if (nyear<=99)
		nyear= "19"+nyear;

	if ((nyear>99) && (nyear<2000))
		nyear+=1900;
	var d;
	d= document.getElementById("theClock");
	
	d.innerHTML=nday+", " + ntoday +"/" + nmonth +"/"+nyear + " " + nhours+": "+nmins+": "+nsecn + " " + AMorPM;

	setTimeout('startclock()',1000);

}

function link_over(obj)
{
	obj.className=obj.className+"_ovr";
}
function link_out(obj)
{
	obj.className=obj.className.substr(0,obj.className.length - 4);
}
function link_click(obj,url, tgt)
{
    var ex = document.getElementById(obj.id + 'ex');
    if(ex != null)
    {
        if(ex.style.display == 'none')
        {
            ex.style.display = '';
        }
        else
        {
            ex.style.display = 'none';
        }
    }
    else
    {
        if(tgt.length > 0)
        {
            window.open(url, tgt);
        }
        else
        {
            document.location = url;   
        }
    }
} 

function setCookie(Name, Path, Expires, Value)
{
	var cstr = Name.concat('=').concat(Value);
	
	if (Path=='')
		path='/';

	cstr=cstr.concat(';path=').concat(Path);


	if (Expires=='')
		Expires=(new Date(2020, 11, 14)).toGMTString();

	document.cookie=cstr.concat(';expires=').concat(Expires);
}

function getCookie(Name, Default)
{
	var cookie = document.cookie;
	var ir = 0, ie = 0, sf = '', i = 0, j = 0;
	Name = Name.toLowerCase();

	if (typeof(Default) == 'undefined')
		Default = '';

	if (cookie.length == 0)
		return Default;

	if ((ir = Name.indexOf('.')) == -1)
	{
		if (cookie.substr(0, Name.length + 1).toLowerCase() == Name.concat('='))
		{
			if ((ie = cookie.indexOf(';')) != -1)
			{
				cookie = cookie.substr(0, ie);
			}
		}
		else
		{
			if ((ie = cookie.toLowerCase().indexOf('; '.concat(Name).concat('='))) == -1)
				return Default;

			cookie = cookie.substr(ie + 2);

			if ((ie = cookie.indexOf(';')) != -1)
			{
				cookie = cookie.substr(0, ie);
			}
		}
		sf = ';';
	}
	else
	{
		if ((i=cookie.toLowerCase().indexOf(Name.concat('='))) != -1)
		{
			if ((j = cookie.indexOf(';', i)) > i + Name.length + 1)
			{
				return ReplaceAll(unescape(cookie.substr(i + Name.length + 1, j - i - Name.length - 1)), '+', ' ');
			}
			else
			{
				j = cookie.length;
				return ReplaceAll(unescape(cookie.substr(i + Name.length + 1, j - i - Name.length - 1)), '+', ' ');
			}
		}

		var Root = Name.substr(0, ir);
		Name = Name.substr(ir + 1);

		if (cookie.substr(0, Root.length + 1).toLowerCase() == Root.concat('='))
		{
			if ((ie = cookie.indexOf(';')) != -1)
			{
				cookie = cookie.substr(0, ie);
			}
		}
		else
		{
			if ((ie = cookie.toLowerCase().indexOf('; '.concat(Root).concat('='))) == -1)
				return Default;

			cookie = cookie.substr(ie + 2);

			if ((ie = cookie.indexOf(';')) != -1)
			{
				cookie = cookie.substr(0, ie);
			}
		}

		cookie = cookie.substr(Root.length + 1);
		sf = '&';
	}

	if (cookie.substr(0, Name.length + 1).toLowerCase() == Name.concat('='))
	{
		ir = Name.length + 1;
	}
	else
	{
		if ((ir = cookie.toLowerCase().indexOf('&'.concat(Name).concat('='))) == -1)
			return Default;

		ir+=Name.length + 2;
	}

	if ((ie=cookie.indexOf(sf, ir)) == -1)
	{
		return ReplaceAll(unescape(cookie.substr(ir)), '+', ' ');
	}
	else
	{
		return ReplaceAll(unescape(cookie.substring(ir, ie)), '+', ' ');
	}
}

function ReplaceChar(iStr)
{
	var	r1=/&/g;
	var	r2=/ /g;
	var	r3=/"/g;

	iStr	=iStr.replace(r1, '%26');
	iStr	=iStr.replace(r2, '%20');
	iStr	=iStr.replace(r3, '%22');

	return iStr;
}

function CharReplace(iStr)
{
	var	r1=/%26/g;
	var	r2=/%20/g;
	var	r3=/%22/g;

	iStr	=iStr.replace(r1, '&');
	iStr	=iStr.replace(r2, ' ');
	iStr	=iStr.replace(r3, '"');

	return iStr;
}

function GetPostVariable(vName, vDef)
{
	var	str=location.href;
	var	pos=str.indexOf('?'.concat(vName).concat('='));

	if (pos==-1)
	{
		pos=str.indexOf('&'.concat(vName).concat('='));
		if (pos==-1) return vDef;
	}
	
	str=str.substring(pos + vName.length + 2);
	pos=str.indexOf('&');

	if (pos==-1)
	{
		pos=str.length;
	}	

	if (pos > 0)
	{
		str=str.substring(0, pos);
	}

	return (typeof(vDef)=='number') ? parseInt(str) : CharReplace(str);
}

function GoNothing()
{
}

function ItemMinimize(Name)
{
	if (!fDSp)
	{
		return;
	}

	var MItem=document.getElementById('IDM_'.concat(Name));
	var Image=document.getElementById('IDI_'.concat(Name));
	
	if (MItem.style.display!='')
	{
		//MItem.setAttribute('style','display:""');
		MItem.style.display='';
		Image.src='/Images/min.gif';
	}
	else
	{
		//MItem.setAttribute('style','display:none');
		MItem.style.display='none';
		Image.src='/Images/max.gif';
	}
}

function SetParameter(pFile, pName, pVal)
{
	if ((cPost=pFile.indexOf('&'.concat(pName).concat('=')))==-1)
		cPost=pFile.indexOf('?'.concat(pName).concat('='));

	if (cPost >= 0)
	{
		if ((pPost=pFile.indexOf('&', cPost + 1))==-1)
		{
			pFile=pFile.substring(0, cPost + pName.length + 2).concat(pVal);
		}
		else
		{
			pFile=pFile.substring(0, cPost + pName.length + 2).concat(pVal).concat(pFile.substr(pPost));
		}
	}
	else
	{
		if (pFile.indexOf('?')==-1)
		{
			pFile=pFile.concat('?').concat(pName).concat('=').concat(pVal);
		}
		else
		{
			pFile=pFile.concat('&').concat(pName).concat('=').concat(pVal);
		}
	}

	return pFile;
}

function ReverseFolderByDate()
{
	Ryear = document.Reverse.fYear.options[document.Reverse.fYear.selectedIndex].value;
	Rmonth = document.Reverse.fMonth.options[document.Reverse.fMonth.selectedIndex].value;
	Rday = document.Reverse.fDay.options[document.Reverse.fDay.selectedIndex].value;

	for (; Rday > 0; Rday--)
	{
		Rdate = new Date(Ryear, Rmonth - 1, Rday);
		if (Rdate.getDate() == Rday)
		{
			break;
		}
	}

	LastDate = Ryear.concat('/').concat(Rmonth).concat('/').concat(Rday).concat(' 23:59:59');
	
	if (RelatedFolder < 1000 )
	{
		location.replace(CurrentFolder.concat('/Default.Asp?d=').concat(escape(LastDate)));
	}
	else
	{
		location.href = SetParameter(location.href, 'd', escape(LastDate));
	}	
	
}

function ShowNextFolderItem(LastDate)
{
	if (RelatedFolder < 1000 )
	{
		location.href = SetParameter(location.href + 'Default.Asp', 'd', escape(LastDate));
	}
	else
	{
		location.href = SetParameter(location.href, 'd', escape(LastDate));
	}	
}

function UnderConst()
{
	alert('Sorry!\nThis Page is under construction!\nPlease try latter!');
}

function showDialog(url, width, height)
{
	return showWindow(url, false, false, false, false, false, false, true, true, width, height, 0, 0);
}

function showWindow(url, isStatus, isResizeable, isScrollbars, isToolbar, isLocation, isFullscreen, isTitlebar, isCentered, width, height, top, left)
{
	if (isCentered)
	{
		top = (screen.height - height) / 2;
		left = (screen.width - width) / 2;
	}

	open(url, '_blank', 'status=' + (isStatus ? 'yes' : 'no') + ','
	+ 'resizable=' + (isResizeable ? 'yes' : 'no') + ','
	+ 'scrollbars=' + (isScrollbars ? 'yes' : 'no') + ','
	+ 'toolbar=' + (isToolbar ? 'yes' : 'no') + ','
	+ 'location=' + (isLocation ? 'yes' : 'no') + ','
	+ 'fullscreen=' + (isFullscreen ? 'yes' : 'no') + ','
	+ 'titlebar=' + (isTitlebar ? 'yes' : 'no') + ','
	+ 'height=' + height + ',' + 'width=' + width + ','
	+ 'top=' + top + ',' + 'left=' + left);
}

function openImage(vLink, vHeight, vWidth)
{
	var sLink = (typeof(vLink.href) == 'undefined') ? vLink : vLink.href;

	if (sLink == '')
	{
		return false;
	}

	winDef = 'status=no,resizable=no,scrollbars=no,toolbar=no,location=no,fullscreen=no,titlebar=yes,height='.concat(vHeight).concat(',').concat('width=').concat(vWidth).concat(',');
	winDef = winDef.concat('top=').concat((screen.height - vHeight)/2).concat(',');
	winDef = winDef.concat('left=').concat((screen.width - vWidth)/2);
	newwin = open('', '_blank', winDef);

	newwin.document.writeln('<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0">');
	newwin.document.writeln('<a href="" onClick="window.close(); return false;"><img src="', sLink, '" alt="', (fBrw) ? '&#272;&#243;ng l&#7841;i' : 'Dong lai', '" border=0></a>');
	newwin.document.writeln('</body>');

	if (typeof(vLink.href) != 'undefined')
	{
		return false;
	}
}

function UnicodeSet(iStr)
{
	for (i=0, oStr=''; i < iStr.length; i++)
	{
		switch ((j=iStr.charCodeAt(i)))
		{
		case 34:
			oStr=oStr.concat('&quot;');
			break;
		case 38:
			oStr=oStr.concat('&amp;');
			break;
		case 39:
			oStr = oStr.concat('&#39;');
			break;
		case 60:
			oStr = oStr.concat('&lt;');
			break;
		case 62:
			oStr = oStr.concat('&gt;');
			break;
		default:
			if (j < 32 || j > 127 || j==34 || j==39)
			{
				oStr=oStr.concat('&#').concat(j).concat(';');
			}
			else
			{
				oStr=oStr.concat(iStr.charAt(i)); 
			}
			break;
		}
	}
	
	return oStr;
}

function UnicodeGet(iStr)
{
	for (i=0, oStr=''; i < iStr.length; )
	{
		if (iStr.charCodeAt(i)==38)
		{
			if (iStr.charCodeAt(i + 1)==35)
			{
				p=iStr.indexOf(';', i  + 2);
				if (p!=-1)
				{
					if (p - i <= 7)
					{
						if (isFinite(iStr.substr(i + 2, p - i - 2)))
						{
							oStr = oStr.concat(String.fromCharCode(iStr.substr(i + 2, p - i - 2)));
							i = p + 1;
							continue;
						}
					}
				}
			}
			else
			{
				p=iStr.indexOf(';', i  + 1);
				if (p!=-1)
				{
					switch (iStr.substr(i + 1, p - i - 1))
					{
					case 'amp':
						oStr = oStr.concat('&');
						i = p + 1;
						break;
					case 'quot':
						oStr = oStr.concat('"');
						i = p + 1;
						break;
					case 'lt':
						oStr = oStr.concat('<');
						i = p + 1;
						break;
					case 'gt':
						oStr = oStr.concat('>');
						i = p + 1;
						break;
					}
				}
			}
		}
	
	
		oStr=oStr.concat(iStr.charAt(i));
		i++;
	}
	
	return oStr;
}

function ShowExpand(sobj1, sobj2)
{
	sobj1.style.display = 'none';
	sobj2.style.display = '';
}

function LTrim(iStr)
{
	while (iStr.charCodeAt(0) <= 32)
	{
		iStr=iStr.substr(1);
	}
	return iStr;
}

function RTrim(iStr)
{
	while (iStr.charCodeAt(iStr.length - 1) <= 32)
	{
		iStr=iStr.substr(0, iStr.length - 1);
	}
	return iStr;
}

function Trim(iStr)
{
	while (iStr.charCodeAt(0) <= 32)
	{
		iStr=iStr.substr(1);
	}

	while (iStr.charCodeAt(iStr.length - 1) <= 32)
	{
		iStr=iStr.substr(0, iStr.length - 1);
	}

	return iStr;
}

function Left(str, n)
{
	if (n <= 0)
	    return "";
	else if (n > String(str).length)
	    return str;
	else
	    return String(str).substring(0,n);
}


function Right(str, n)
{
    if (n <= 0)
       return "";
    else if (n > String(str).length)
       return str;
    else {
       var iLen = String(str).length;
       return String(str).substring(iLen, iLen - n);
    }
}

function CheckEmailAddress(Email)
{
	Email = Trim(Email);

	while (Email != '')
	{
		c = Email.charAt(0);	
		if (c==' ' || c=='<' || c==39 || c==':' || c=='.')
		{
			Email = Email.substr(1);
		}
		else
		{
			break;
		}
	}

	i = Email.indexOf('>');
	if (i==-1)
	{
		while (Email != '')
		{
			c = Email.charAt(Email.length - 1);
			if (c==' ' || c==39 || c=='.')
			{
				Email = Email.substr(0, Email.length - 1);
			}
			else
			{
				break;
			}
		}
	}
	else
	{
		Email = Email.substr(0, i);
	}

	if (Email.length > 96)
		return '';

	i = Email.lastIndexOf('@');
	j = Email.lastIndexOf('.');
	if (i < j)
		i = j;

	switch (Email.length - i - 1)
	{
	case 2:
		break;
	case 3:
		switch (Email.substr(i))
		{
		case '.com':
		case '.net':
		case '.org':
		case '.edu':
		case '.mil':
		case '.gov':
		case '.biz':
		case '.pro':
		case '.int':
			break;
		default:
			return '';
		}
		break;
	default:
		switch (Email.substr(i))
		{
		case '.name':
		case '.info':
			break;
		default:
			return '';
		}
		break;
	}

	Email = Email.toLowerCase();

	if (Email == '')
		return '';

	if (Email.indexOf(' ') != -1)
		return '';

	if (Email.indexOf('..') != -1)
		return '';

	if (Email.indexOf('.@') != -1)
		return '';

	if (Email.indexOf('@.') != -1)
		return '';

	if (Email.indexOf(':') != -1)
		return '';

	for (i=0; i < Email.length; i++)
	{
		c = Email.charAt(i);

		if (c >= '0' && c <= '9')
			continue;
		
		if (c >= 'a' && c <= 'z')
			continue;
		
		if ('`~!#$%^&*-_+=?/\\|@.'.indexOf(c) != -1)
			continue;

		return '';
	}

	if ((i=Email.indexOf('@'))==-1)
		return '';

	if (Email.substr(i + 1).indexOf('@')!=-1)
		return '';

	if (Email.charAt(0)=='.' || Email.charAt(Email.length - 1)=='.')
		return '';

	return Email;
}

function ReplaceAll(iStr, v1, v2)
{
	var i = 0, oStr = '', j = v1.length;

	while (i < iStr.length)
	{
		if (iStr.substr(i, j) == v1)
		{
			oStr+=v2;
			i+=j
		}
		else
		{
			oStr+=iStr.charAt(i);
			i++;
		}
	}

	return oStr;
}

function TrimAndRDS(iStr)
{
	function IsHyperLink(iStr)
	{
		var i = 0, c = ' ';

		if (iStr.charAt(0) == '.')
			return false;

		for (i=0; i < iStr.length; i++)
		{
			c = iStr.charAt(i).toLowerCase();
			if (c >= '0' && c <= '9')
				continue;
		
			if (c >= 'a' && c <= 'z')
				continue;
		
			if ('@_-&.?#+-/:'.indexOf(c) != -1)
				continue;

			return false;
		}
	
		return true;
	}

	function GetLastBreak(iStr, s)
	{
		var f = new Array('(', ')', '<', '>', ' ', '\r', '\n', '\t', ',', ';', '!'), p = 0, i = 0, r = -1;
	
		for (i = 0; i < f.length; i++)
			if ((p = iStr.lastIndexOf(f[i], s)) != -1)
				if (r == -1 || p > r)
					r = p;
		return r;
	}

	function GetNextBreak(iStr, s)
	{
		var f = new Array('(', ')', '<', '>', ' ', '\r', '\n', '\t', ',', ';', '!'), p = 0, i = 0, r = -1;
	
		for (i = 0; i < f.length; i++)
			if ((p = iStr.indexOf(f[i], s)) != -1)
				if (r == -1 || p < r)
					r = p;
		return r;
	}

	function CheckDotAfter(iStr)
	{
		var p0 = 0, p1 = 0, p2 = 0, p3 = 0;

		while ((p1 = iStr.indexOf('.', p0)) != -1)
		{
			if (iStr.charAt(p1 - 1) == ' ')
			{
				iStr = iStr.substr(0, p1 - 1).concat(iStr.substr(p1));
				p0 = p1;
			}
			else
			{
				p0 = p1 + 1;
			}

			if (iStr.charAt(p0) != ' ')
			{
				if ((p3 = GetLastBreak(iStr, p0)) == -1)
				{
					p3 = p0;
				}
				else
				{
					p3 = p3 + 1;
				}
		
				if ((p2 = GetNextBreak(iStr, p3)) == -1)
				{
					if (IsHyperLink(iStr.substr(p3)))
					{
						iStr = iStr.substr(0, p3).concat(iStr.substr(p3).toLowerCase())
						break;
					}
					else
					{
						if (iStr.charAt(p0) < '0' || iStr.charAt(p0) > '9')
						{
							iStr = iStr.substr(0, p0).concat(' ').concat(iStr.substr(p0, 1).toUpperCase()).concat(iStr.substr(p0 + 1));
							p0++;
						}
					}
				}
				else
				{
					if (IsHyperLink(iStr.substring(p3, p2)))
					{
						iStr = iStr.substr(0, p3).concat(iStr.substring(p3, p2).toLowerCase()).concat(iStr.substr(p2));
						p0 = p2 + 1;
					}
					else
					{
						if (iStr.charAt(p0) < '0' || iStr.charAt(p0) > '9')
						{
							iStr = iStr.substr(0, p0).concat(' ').concat(iStr.substr(p0, 1).toUpperCase()).concat(iStr.substr(p0 + 1));
							p0++;
						}
					}
				}
			}
			else
			{
				iStr = iStr.substr(0, p0 + 1).concat(iStr.substr(p0 + 1, 1).toUpperCase()).concat(iStr.substr(p0 + 2));
			}
		}	

		return iStr;
	}

	function CheckCharAfter(iStr, iChar, iUp)
	{
		var p0 = 0, p1 = 0;

		while ((p1 = iStr.indexOf(iChar, p0)) != -1)
		{
			if (iStr.charAt(p1 - 1) == ' ')
			{
				iStr = iStr.substr(0, p1 - 1).concat(iStr.substr(p1));
				p0 = p1;
			}
			else
			{
				p0 = p1 + 1;
			}

			if (iStr.charAt(p0) != ' ')
			{
				if (iStr.charAt(p0) < '0' || iStr.charAt(p0) > '9')
				{
					if (iUp)
					{
						iStr = iStr.substr(0, p0).concat(' ').concat(iStr.substr(p0, 1).toUpperCase()).concat(iStr.substr(p0 + 1));
					}
					else
					{
						iStr = iStr.substr(0, p0).concat(' ').concat(iStr.substr(p0));
					}
					p0++;
				}
			}
			else
			{
				if (iUp)
				{
					iStr = iStr.substr(0, p0 + 1).concat(iStr.substr(p0 + 1, 1).toUpperCase()).concat(iStr.substr(p0 + 2));
				}
			}
		}

		return iStr;
	}

	function CheckScope(iStr, s1, s2)
	{
		var p0 = 0, p1 = 0;

		for (p0 = 0; (p1 = iStr.indexOf(s1, p0)) != -1; )
		{
			if (iStr.charAt(p1 + 1) == ' ')
				iStr = iStr.substr(0, p1 + 1).concat(iStr.substr(p1 + 2));

			if (p1 > 0)
				if (iStr.charAt(p1 - 1) != ' ')
				{
					iStr = iStr.substr(0, p1).concat(' ').concat(iStr.substr(p1));
					p1++;
				}
			
			p0 = p1 + 1;
		}

		for (p0 = 0; (p1 = iStr.indexOf(s2, p0)) != -1; )
		{
			var SkipChar = ':,.;!?'.concat(s2);

			if (p1 > 0)
				if (iStr.charAt(p1 - 1) == ' ')
				{
					iStr = iStr.substr(0, p1 - 1).concat(iStr.substr(p1));
					p1--;
				}

			if (iStr.charAt(p1 + 1) != ' ' && SkipChar.indexOf(iStr.charAt(p1 + 1)) == -1)
				iStr = iStr.substr(0, p1 + 1).concat(' ').concat(iStr.substr(p1 + 1));

			p0 = p1 + 1;
		}		

		return iStr;
	}
	
	iStr = ReplaceAll(iStr, '  ', ' ');
	iStr = ReplaceAll(iStr, ' \r\n', '\r\n');
	iStr = ReplaceAll(iStr, '\r\n ', '\r\n');

	iStr = CheckCharAfter(iStr, ',', false);
	iStr = CheckCharAfter(iStr, ':', false);
	iStr = CheckCharAfter(iStr, ';', false);
	iStr = CheckCharAfter(iStr, '?', true);
	iStr = CheckCharAfter(iStr, '!', true);

	iStr = CheckScope(iStr, '(', ')');
	iStr = CheckScope(iStr, '[', ']');

	iStr = ReplaceAll(iStr, 'http: //', 'http://');
	iStr = CheckDotAfter(iStr);

	iStr = ReplaceAll(iStr, ', \r\n', ',\r\n');
	iStr = ReplaceAll(iStr, ': \r\n', ':\r\n');
	iStr = ReplaceAll(iStr, '; \r\n', ';\r\n');
	iStr = ReplaceAll(iStr, '? \r\n', '!\r\n');
	iStr = ReplaceAll(iStr, '! \r\n', '!\r\n');
	iStr = ReplaceAll(iStr, '. \r\n', '.\r\n');


	if (iStr.charAt(0) == ' ')
		iStr = iStr.substr(1);

	if (iStr.charAt(iStr.length - 1) == ' ')
		iStr = iStr.substr(0, iStr.length - 1);

	return iStr.substr(0, 1).toUpperCase().concat(iStr.substr(1));
}

function dw(wstr)
{
	document.writeln(unescape(wstr));
}

/*Show tooltip*/
function showDivMessage(productid,e)
{
	overShow=true;
	OnMOver(e);
	var ObjDiv=document.getElementById(nameObjShow);
	if(ObjDiv!=null)
	{   
	    var response=Hstc.Website.Modules.Desktop.Products.ctlAjax.GetProductInfo(productid);
	    if (null!=response)
	    {
	        ObjDiv.innerHTML=response.value;
	    }
	}
}

function togglepanel(hidename)
{
    var v_ctl = document.getElementById(hidename);
    if (null != v_ctl) {
        if (v_ctl.style.display == 'none') {
            v_ctl.style.display = '';
        }
        else {
            v_ctl.style.display = 'none';
        }
    }
    
}

    function togglecontrol(octl,hidename)
    {
        var v_ctl = document.getElementById(hidename);
        if (null != v_ctl)
        {
            if (v_ctl.style.display == 'none') {
                v_ctl.style.display = '';
                octl.className = octl.className.replace('icon-arrowdown', 'icon-arrowup');
            }
            else {
                v_ctl.style.display = 'none';
                octl.className = octl.className.replace('icon-arrowup', 'icon-arrowdown');
            }
        }
    }