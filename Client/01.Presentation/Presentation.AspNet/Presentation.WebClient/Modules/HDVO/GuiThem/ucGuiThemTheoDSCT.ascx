<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGuiThemDanhSachCT.ascx.cs" Inherits="Presentation.WebClient.Modules.HDVO.GuiThem.ucGuiThemDanhSachCT" %>
<%@ Import Namespace="Presentation.WebClient.Business" %>
<%@ Import Namespace="Utilities.Common" %>

<script>
    var griddataid= '<%=grGuiThemDS.ClientID %>'
    $(document).ready(function () {

        $("#tabs").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
        $("#tabs li").removeClass("ui-corner-top").addClass("ui-corner-left");
        $('input[type=text]').addClass('bovien');
        $('select').addClass('bovien');
        $("#dialog").dialog({
            autoOpen: false,
            show: {
                effect: "blind",
                duration: 1
            },
            hide: {
                effect: "explode",
                duration: 10
            }
        });    

    
     $('#' + griddataid).on('click', 'td', function () {
            var col = $(this).parent().children().index($(this));
            var row = $(this).parent().parent().children().index($(this).parent());
          //  alert('dcm')
            if (col > 0) {
           
                var scb = document.getElementsByName('cbrow')
               
                for (j = 1; j < scb.length; j++) {
                    if (j == row)
                        getObj(scb[j].id).checked = true;
                    else
                        getObj(scb[j].id).checked = false;
                }
            }           
        });
        setbuttonmenu()
    });

   
    function CallInfoCust(idkhachhang) {
        $.ajax({
            type: "POST", url: "PopupContent.aspx/GetInfoCus",
            data: "{'idKhachHang':" + idkhachhang + "}",
            contentType: "application/json; charset=utf-8", dataType: "json",

            success: function (msg) {
                if (msg.d != '') {
                    var sval = msg.d.split('#')
                }
            }
        });
    }

    function Find() {
        var eventInstance = window.event ? event : e;
        var unicode = eventInstance.charCode ? eventInstance.charCode : eventInstance.keyCode;
        if (unicode == hotkey) {
            CallInfoCust(12)
        }
    }
    var ChecktoUpdate;
    var oldval=''
     
    function GeninfoSotiengui()
    {  
            oldval=$('#inpIDTarget').val()
            openPopUpWindow('Modules/HDVO/Popup/ucPopupSoTGui.aspx?f=HDV_GUI_THEM_TIEN_THEO_SO&m=true', document.forms[0].<%=inpIDSO.ClientID %>, window.outerWidth - 120, window.outerHeight - 100)     
            ChecktoUpdate = setInterval('afterchoosesoso()', 2000)            
   }
   function xoadsguithem()
   {
            var scb = document.getElementsByName('cbrow')
            var dschon=''
             for (j = 0; j < scb.length; j++) {
                    if (scb[j].id.indexOf('All')==-1 && getObj(scb[j].id).checked==false )
                    {
                        dschon+=getObj(scb[j].id.replace('cbrow','inp_SoTienGuiThem_')).getAttribute('lang')+'@'+$('#'+scb[j].id.replace('cbrow','inp_SoTienGuiThem_')).val().replace(',','')+
                        '@'+getObj(scb[j].id.replace('cbrow','spnval')).innerHTML.replace(',','')+    '#'
                    }
             }
             if (dschon.length>0)
                dschon=dschon.substring(0,dschon.length-1)
            callrefreshdsthem(dschon)
   }
  function afterchoosesoso()
   {
            //alert($('#inpIDTarget').val())
            if ($('#inpIDTarget').val()!=oldval && $('#inpIDTarget').val()!='0' && $('#inpIDTarget').val()!='')
            {
              clearInterval(ChecktoUpdate)
              
              var scb = document.getElementsByName('cbrow')
              oldval=''
              for (j = 0; j < scb.length; j++) {
                    if (scb[j].id.indexOf('All')==-1)
                    {
                        oldval+=getObj(scb[j].id.replace('cbrow','inp_SoTienGuiThem_')).getAttribute('lang')+'@'+$('#'+scb[j].id.replace('cbrow','inp_SoTienGuiThem_')).val().replace(',','')+
                        '@'+getObj(scb[j].id.replace('cbrow','spnval')).innerHTML.replace(',','')+    '#'
                    }
              }
              
              
              oldval=oldval+$('#inpIDTarget').val()
              $('#inpIDTarget').val('0')
               callrefreshdsthem(oldval)
        }
   }
   function callrefreshdsthem(dschon)
   {
     $.ajax({
                type: "POST", url: "PopupContent.aspx/GuiThemDS_DSGUITHEM",
                data: "{'idSoTGui':'" +dschon + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",            
                beforeSend: function () {
                 getObj('loadingParent').style.display = '';            
                 
                                       }, 
                error: function () {                                
                                    getObj('loadingParent').style.display = 'NONE'
                                },
                success: function (msg) {
                    if (msg.d!='')
                    {
                       var sresult = msg.d.split('@@@')
                       getObj('divtableContent').innerHTML=sresult[0]
                       getObj('<%=lblTongSo.ClientID %>').innerHTML=sresult[1]
                       getObj('<%=lblTongoDuCu.ClientID %>').innerHTML=sresult[2]
                       getObj('<%=lblTongTienGuiThem.ClientID %>').innerHTML=sresult[3]
                   }
                   getObj('loadingParent').style.display = 'NONE'
                }
                });
   }
       function fnaction(strconfirm, action) {

           
           getObj('<%=cfaction.ClientID %>').value=action
          
           if(!validdata())
            return
           if (strconfirm != '' || action == 'SUA') {
               var sendata = ''
               if (action == 'THEM') {
                   sendata = GetFormData('THEM');
               }
               else if (action == 'SUA') {
                   sendata = $('#<%=inpIDSO.ClientID %>').val()
               }
               else if (action == 'XOA') {
                   sendata = GetFormData('XOA');
                   getObj('<%=cfaction.ClientID %>').value = 'XOA'
                   strAction = 'XOA'
               }
               else if (action == 'DUYET') {
                   sendata = GetFormData('DUYET');
                   getObj('<%=cfaction.ClientID %>').value = 'DUYET'
                   strAction = 'DUYET'
               }
               else if (action == 'THOAIDUYET') {
                   sendata = GetFormData('THOAIDUYET');
                   getObj('<%=cfaction.ClientID %>').value = 'THOAIDUYET'
                   strAction = 'THOAIDUYET'
               }
               else if (action == 'TUCHOIDUYET') {
                   sendata = GetFormData('TUCHOIDUYET');
                   getObj('<%=cfaction.ClientID %>').value = 'TUCHOIDUYET'
                   strAction = 'TUCHOIDUYET'
               }
               {
                   var IDVALUE = $('#<%=ID_GIAO_DICH.ClientID %>').val()
                   getObj('<%=lblErr.ClientID %>').value = ''
                   var ck=false
                   if (strconfirm=='')
                   {
                    ck=true;
                   }else {ck=confirm(strconfirm)}
                   if (ck) {
                       $.ajax({
                           type: "POST", url: "PopupContent.aspx/GuiThemDS_" + action,
                           data: "{'data':'" + sendata + "','straction':'" + action + "','IDVALUE':'" + IDVALUE + "'}",
                           contentType: "application/json; charset=utf-8", dataType: "json",
                           beforeSend: function () {
                               getObj('loadingParent').style.display = '';
                           },
                           error: function () {
                               getObj('loadingParent').style.display = 'NONE'
                           },
                           success: function (msg) {
                               setbuttonmenu()
                               getObj('loadingParent').style.display = 'NONE'
                               sendata = ''
                               if (msg.d != '') {
                                   if (action == 'THEM') {
                                       afterthem(msg.d)
                                   }
                                   else if (action == 'SUA') {
                                       aftersua(msg.d)
                                   }
                                   else if (action == 'XOA') {
                                       afterdelete(msg.d)
                                   }
                                   else if (action == 'DUYET') {
                                       afterduyet(msg.d)
                                   }
                                   else if (action == 'THOAIDUYET') {
                                       afterthoaiduyet(msg.d)
                                   }
                                   else if (action == 'TUCHOIDUYET') {
                                       afterthoaiduyet(msg.d)
                                   }
                               }

                           }
                       });
                   }
               }
           }
           else {

               getObj('mainForm').submit()
           }
       }

//    val = obj.MA_GDICH + "@@";
//                        string sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
//                        val += sTrangThaiNVu + "@@";
//                        val += LanguageEngine.Instance().GetContent(LanguageType.TypeUI, BusinessConstant.layMaNgonNguNghiepVu(sTrangThaiNVu));
    function afterthoaiduyet(strdata)
    {   
        getObj('<%=lblErr.ClientID %>').innerText=strdata.split('#')[1]
        $("#dialog").dialog("open");
        if(strdata.split('#')[0]=='1')
        {
            var sResult = strdata.split('#')[2].split('@@');                  
            getObj('<%=lblTrangThai.ClientID %>').innerText=sResult[2]   
            $('#<%=TRANG_THAI_NGHIEP_VU.ClientID %>').val(sResult[1])
            getObj('<%=inpAction.ClientID %>').value='SAUTHOAIDUYET'
            getObj('<%=cfaction.ClientID %>').value=''
            setbuttonmenu()    
        }
    }
    function afterduyet(strdata)
    {   
        getObj('<%=lblErr.ClientID %>').innerText=strdata.split('#')[1]
        $("#dialog").dialog("open");
        if(strdata.split('#')[0]=='1')
        {             
            var sResult = strdata.split('#')[2].split('@@');            
            getObj('<%=lblTrangThai.ClientID %>').innerText=sResult[0]      
           
            getObj('<%=inpAction.ClientID %>').value='XEMSAUDUYET'
            getObj('<%=cfaction.ClientID %>').value=''
            setbuttonmenu()
            setstyleinputtext(0)
        }
    }
        function afterthem(strdata)
        {   
                getObj('<%=lblErr.ClientID %>').innerText=strdata.split('#')[1]        
                $("#dialog").dialog("open");
                 if(strdata.split('#')[0]=='1')
                 {
                var sResult = strdata.split('#')[2].split('@@');
                getObj('<%=TRANG_THAI_NGHIEP_VU.ClientID %>').value=sResult[2] 
                $('#<%=txtMaGiaoDich.ClientID %>').val(sResult[1])
                getObj('<%=lblTrangThai.ClientID %>').innerText=sResult[3]      
                $('#<%=txtTrangThai.ClientID %>').val(sResult[4])      
                $('#<%=teldtNgayNhap.ClientID %>').val(sResult[5])     
                $('#<%=txtNguoiLap.ClientID %>').val(sResult[6])   
                $('#<%=ID_GIAO_DICH.ClientID %>')    .val(sResult[0])
                getObj('<%=inpAction.ClientID %>').value='XEM'
                getObj('<%=cfaction.ClientID %>').value=''
                setbuttonmenu()    
                setstyleinputtext(0)
                }
        }     
       function afterdelete(strdata) {
           getObj('<%=lblErr.ClientID %>').innerText = strdata.split('#')[1]
           $("#dialog").dialog("open");
           if (strdata.split('#')[0] == '1') {
               setTimeout('window.close()', 2500)
           }
       }
       function aftersua(strdata) {
           if (strdata.split('#')[0] == '0') {
               getObj('<%=lblErr.ClientID %>').innerText = strdata.split('#')[1]
               $("#dialog").dialog("open");
           }
           else {
               getObj('<%=cfaction.ClientID %>').value = 'SUA'
               getObj('<%=inpAction.ClientID %>').value = 'SUA'
               setbuttonmenu()
               setstyleinputtext(1)
           }
       }
       
       function validdata() {
           var chuanhap = '<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.ThongBao.ChuaNhap") %>'
           var chuachon = '<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.ChuaChonBanGhi") %>'

           if ($('#<%=txtDienGiai.ClientID %>').val() == '') {
               getObj('<%=lblErr.ClientID %>').innerText = chuanhap.replace('{0}', '<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.DienGiai")%>')
               $("#dialog").dialog("open");
               $('#<%=txtDienGiai.ClientID %>').focus();
               return false;
           }
           var stb = document.getElementsByName('SoTienGuiThem')
           if (stb.length==0)
           {
               getObj('<%=lblErr.ClientID %>').innerText =chuachon
               $("#dialog").dialog("open");
              
               return false;
           }
           else{
            for (i=0;i<stb.length;i++)
            {
                
                if (parseFloat(removeCharN(getObj(stb[i].id).value, ','))<=0)
                {
                     getObj('<%=lblErr.ClientID %>').innerText = chuanhap.replace('{0}', 'Số tiền gửi thêm')
                     $("#dialog").dialog("open");                     
                     return false;
                }
            }
           }
           return true;

       }
        function  GetFormData(action)
        {
                 var strdata=''
           
                
                strdata+=  $('#<%=txtMaGiaoDich.ClientID %>').val()+'#';
               // obj.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                strdata+= $('#<%=cmbGD_HinhThuc.ClientID %>').val()+'#';
                strdata+= $('#<%=cmbLoaiTien.ClientID %>').val()+'#';
                strdata+= '0#'
                strdata+= $('#<%=txtNguoiGiaoDich.ClientID %>').val()+'#';
                strdata+=  $('#<%=txtDiaChi.ClientID %>').val()+'#';
                strdata+=  $('#<%=txtDienGiai.ClientID %>').val()+'#';
               
                //Thông tin kiểm soát
                strdata+=getObj('<%=TRANG_THAI_NGHIEP_VU.ClientID %>').value+'#'; 
                strdata+= getObj('<%=TRANG_THAI_BAN_GHI.ClientID %>').value+'#';
                strdata+= $('#<%=teldtNgayNhap.ClientID %>').val()+'#';
                strdata+=  $('#<%=txtNguoiLap.ClientID %>').val()+'#';

                if (action != 'THEM')
                {
                    strdata+= '<%=AppConfig.LoginedUser.NgayLamViecHienTai %>#';
                    strdata+= '<%=AppConfig.LoginedUser.UserName%>#';
                }
                strdata+='@@@'
                var scb = document.getElementsByName('cbrow')
                for (j = 0; j < scb.length; j++) {
                        if (scb[j].id.indexOf('All')==-1)
                        {
                            strdata+=getObj(scb[j].id.replace('cbrow','inp_SoTienGuiThem_')).getAttribute('lang')+'@'+$('#'+scb[j].id.replace('cbrow','inp_SoTienGuiThem_')).val().replace(',','')+
                            '@'+getObj(scb[j].id.replace('cbrow','spnval')).innerHTML.replace(',','')+    '#'
                        }
                }
                return strdata
            
        }
   function setbuttonmenu()
        {    
            
            $('#btnsua').removeClass( "clsButton-disable ic-edit" ).removeClass( "clsButton ic-edit" )
            $('#btnxoa').removeClass( "clsButton-disable ic-delete" ).removeClass( "clsButton ic-delete" )
            $('#btntrinhduyet').removeClass( "clsButton-disable ic-approve" ).removeClass( "clsButton ic-approve" )
            $('#btnbangke').removeClass( "clsButton-disable ic-print" ).removeClass( "clsButton ic-print" )
            $('#btnduyet').removeClass( "clsButton-disable ic-approve" ).removeClass( "clsButton ic-approve" )
            $('#btntuchoi').removeClass( "clsButton-disable ic-cancel" ).removeClass( "clsButton ic-cancel" )
            $('#btnthoaiduyet').removeClass( "clsButton-disable ic-refuse" ).removeClass( "clsButton ic-refuse" )
            $('#btnadddetail').removeClass( "clsButton-disable ic-add-detail" ).removeClass( "clsButton ic-add-detail" )
            $('#btndeletedetail').removeClass("clsButton-disable ic-delete-detail").removeClass("clsButton ic-delete-detail")
            if (getObj('<%=TRANG_THAI_NGHIEP_VU.ClientID %>').value!='' && getObj('<%=cfaction.ClientID %>').value=='0')
            {   
                setstyleinputtext(0)
                var trangthai=getObj('<%=TRANG_THAI_NGHIEP_VU.ClientID %>').value
                if (trangthai=='TCD' || trangthai=='CDU')
                {
                    $('#btnsua').addClass('clsButton ic-edit')
                    $('#btnxoa').addClass('clsButton ic-delete')                    
                    $('#btnduyet').addClass('clsButton ic-approve')
                    $('#btntuchoi').addClass('clsButton ic-cancel')

                    $('#btntrinhduyet').addClass( "clsButton-disable ic-approve" )
                    $('#btnbangke').addClass( "clsButton-disable ic-print" )                   
                    $('#btnthoaiduyet').addClass( "clsButton-disable ic-refuse" )
                    $('#btnadddetail').addClass( "clsButton-disable ic-add-detail" )
                    $('#btndeletedetail').addClass( "clsButton-disable ic-delete-detail" )

                }
                else if (trangthai=='DDU')
                {
                    $('#btnsua').addClass('clsButton-disable ic-edit')
                    $('#btnxoa').addClass('clsButton-disable ic-delete')                   
                             
                    $('#btnthoaiduyet').addClass('clsButton ic-cancel') 
                    
                    $('#btnduyet').addClass('clsButton-disable ic-approve')           
                    $('#btntrinhduyet').addClass( "clsButton-disable ic-approve" )
                    $('#btntuchoi').addClass( "clsButton-disable ic-approve" )
                    $('#btnbangke').addClass( "clsButton-disable ic-print" )  
                    $('#btnadddetail').addClass( "clsButton-disable ic-add-detail" )
                    $('#btndeletedetail').addClass( "clsButton-disable ic-delete-detail" )
                }
                else if (trangthai=='THD')
                {
                    $('#btnsua').addClass('clsButton ic-edit')
                    $('#btnxoa').addClass('clsButton ic-delete')  

                    $('#btnduyet').addClass('clsButton-disable ic-approve')
                    $('#btntuchoi').addClass('clsButton-disable ic-cancel')
                    $('#btntrinhduyet').addClass( "clsButton-disable ic-approve" )
                    $('#btnbangke').addClass( "clsButton-disable ic-print" )                   
                    $('#btnthoaiduyet').addClass( "clsButton-disable ic-refuse" )
                    $('#btnadddetail').addClass( "clsButton-disable ic-add-detail" )
                    $('#btndeletedetail').addClass( "clsButton-disable ic-delete-detail" )
                }
            }
            else 
            {
                if (getObj('<%=inpAction.ClientID %>').value=='XEM')
                {
                    $('#btnsua').addClass('clsButton ic-edit')
                    $('#btnxoa').addClass('clsButton ic-delete')
                    $('#btntrinhduyet').addClass('clsButton-disable ic-approve')
                    $('#btnbangke').addClass('clsButton-disable ic-print')
                    $('#btnduyet').addClass('clsButton ic-approve')
                    $('#btntuchoi').addClass('clsButton ic-cancel')
                    $('#btnthoaiduyet').addClass('clsButton-disable ic-refuse')
                    $('#btnadddetail').addClass('clsButton-disable ic-add-detail')
                    $('#btndeletedetail').addClass('clsButton-disable ic-delete-detail')
                
                }
                else if (getObj('<%=inpAction.ClientID %>').value=='XEMSAUDUYET')
                {
                    $('#btnsua').removeClass( "clsButton ic-edit" ).addClass('clsButton-disable ic-edit')
                    $('#btnxoa').removeClass( "clsButton ic-delete" ).addClass('clsButton-disable ic-delete')
                    $('#btntrinhduyet').removeClass( "clsButton ic-approve" ).addClass('clsButton-disable ic-approve')
                    $('#btnbangke').removeClass( "clsButton-disable ic-print" ).addClass('clsButton-disable ic-print')
                    $('#btnduyet').removeClass( "clsButton ic-approve" ).addClass('clsButton-disable ic-approve')
                    $('#btntuchoi').removeClass( "clsButton ic-cancel" ).addClass('clsButton-disable ic-cancel')
                    $('#btnthoaiduyet').removeClass( "clsButton-disable ic-refuse" ).addClass('clsButton ic-refuse')
                    $('#btnadddetail').removeClass( "clsButton ic-refuse" ).addClass('clsButton-disable ic-add-detail')
                    $('#btndeletedetail').removeClass( "clsButton ic-refuse" ).addClass('clsButton-disable ic-delete-detail')
                }
                else if (getObj('<%=inpAction.ClientID %>').value=='SAUTHOAIDUYET')
                {
                    $('#btnsua').removeClass( "clsButton-disable ic-edit" ).addClass('clsButton ic-edit')
                    $('#btnxoa').removeClass( "clsButton-disable ic-delete" ).addClass('clsButton ic-delete')
                    $('#btntrinhduyet').removeClass( "clsButton ic-approve" ).addClass('clsButton-disable ic-approve')
                    $('#btnbangke').removeClass( "clsButton-disable ic-refuse" ).addClass('clsButton-disable ic-print')
                    $('#btnduyet').removeClass( "clsButton ic-refuse" ).addClass('clsButton-disable ic-approve')
                    $('#btntuchoi').removeClass( "clsButton ic-refuse" ).addClass('clsButton-disable ic-cancel')
                    $('#btnthoaiduyet').removeClass( "clsButton ic-refuse" ).addClass('clsButton-disable ic-refuse')
                    $('#btnadddetail').removeClass( "clsButton ic-refuse" ).addClass('clsButton-disable ic-add-detail')
                    $('#btndeletedetail').removeClass( "clsButton ic-refuse" ).addClass('clsButton-disable ic-delete-detail')
                }
                else
                {   
                    $('#btnsua').removeClass( "clsButton ic-edit" ).addClass('clsButton-disable ic-edit')
                    $('#btnxoa').removeClass( "clsButton ic-delete" ).addClass('clsButton-disable ic-delete')
                    $('#btntrinhduyet').removeClass( "clsButton-disable ic-approve" ).addClass('clsButton ic-approve')
                    $('#btnbangke').removeClass( "clsButton ic-print" ).addClass('clsButton-disable ic-print')
                    $('#btnduyet').removeClass( "clsButton ic-approve" ).addClass('clsButton-disable ic-approve')
                    $('#btntuchoi').removeClass( "clsButton ic-cancel" ).addClass('clsButton-disable ic-cancel')
                    $('#btnthoaiduyet').removeClass( "clsButton ic-refuse" ).addClass('clsButton-disable ic-refuse')
                    $('#btnadddetail').removeClass( "clsButton-disable ic-refuse" ).addClass('clsButton ic-add-detail')
                    $('#btndeletedetail').removeClass( "clsButton-disable ic-refuse" ).addClass('clsButton ic-delete-detail')
                }
            }
        }
        function typeval(P)
        {
           var v=removeCharN(P.value, ',')
           if (!isNaN(v))
            {
            P.value =commaSplit(parseFloat(v), '');
            var oldval = getObj(P.id.replace('inp_SoTienGuiThem_','spnval')).innerText
            
            oldval=parseFloat(removeCharN(oldval, ',') )
            $('#'+P.id.replace('inp_SoTienGuiThem_','inp_SO_DU_MOI_')).val(commaSplit(parseFloat(v)+oldval, ''))
           }
           else{
           }
        }
        function setstyleinputtext(i)
        {
            if (i==0)
            {
                var stab = document.getElementsByName('SoTienGuiThem')
                for (j=0;j<stab.length;j++)
                {
                    $('#'+stab[j].id).removeClass( "bovien_inp" ).addClass('bovien')
                    $('#'+stab[j].id).attr('disabled','disabled');
                    
                }
                $('#<%=txtNguoiGiaoDich.ClientID %>').attr('disabled','disabled');
                $('#<%=txtDiaChi.ClientID %>').attr('disabled','disabled');
                $('#<%=txtDienGiai.ClientID %>').attr('disabled','disabled');
                $('#<%=txtDienGiai.ClientID %>').removeClass( "bovien_inp" ).addClass('bovien') 
            }
            else{
                var stab = document.getElementsByName('SoTienGuiThem')
                for (j=0;j<stab.length;j++)
                {
                    $('#'+stab[j].id).removeClass( "bovien" ).addClass('bovien_inp')
                    $('#'+stab[j].id).removeAttr("disabled");
                }
                $('#<%=txtNguoiGiaoDich.ClientID %>').removeAttr("disabled");
                $('#<%=txtDiaChi.ClientID %>').removeAttr("disabled");
                $('#<%=txtDienGiai.ClientID %>').removeAttr("disabled");
                $('#<%=txtDienGiai.ClientID %>').removeClass( "bovien" ).addClass('bovien_inp') 
            }
        }
</script>

<div id="dvDetail" >
<input type="hidden" id="inpIDTarget" value="0"/>
<input type="hidden" id="cfaction" value="THEM" runat="server" />
<input type="hidden" id="inpIDSO" value="0" runat="server" />
<input type="hidden" id="inpAction" value="0" runat="server" />
<input type="hidden" id="ID_GIAO_DICH" value="0" runat="server" />
<input type="hidden" id="MA_GIAO_DICH" value="0" runat="server" />
<input type="hidden" id="LOAI_HANH_DONG" value="0" runat="server" />
<input type="hidden" id="TRANG_THAI_BAN_GHI" value="" runat="server" />
<input type="hidden" id="TRANG_THAI_NGHIEP_VU" value="" runat="server" />
 <div id="tabs" style="height:auto">
   <div class="navbar"  style="height:45px">
   <table width="100%">
 <tr style="height:50px; vertical-align:text-top">
        <td>
            <%--<input type="button" name="btnaction" disabled="disabled"  value="Thêm" id="Button1" class="clsButton  ic-add" />--%>
            <input type="button" name="btnaction"  onclick="fnaction('','SUA')"  value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.Sua") %>" id="btnsua" class="clsButton ic-edit" />
             <input type="button" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiXoa") %>','XOA')" name="btnaction" value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.Xoa") %>" id="btnxoa"  class="clsButton ic-delete" />
            <input type="button" name="btnaction"  onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet") %>','THEM')" value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.TrinhDuyet") %>" id="btntrinhduyet" class="clsButton ic-approve" />            
            <input type="button" name="btnaction"  onclick="fnaction('','bangke')" value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.BangKe") %>" id="btnbangke" class="clsButton ic-print" />
            <input type="button" name="btnaction"  onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet") %>','DUYET')" value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.Duyet") %>" id="btnduyet" class="clsButton ic-approve" />            
            <input type="button" name="btnaction"  onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiTuChoiDuyet") %>','TUCHOIDUYET')" value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.TuChoi") %>" id="btntuchoi" class="clsButton ic-cancel" />
            <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiThoaiDuyet") %>','THOAIDUYET')"  value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.ThoaiDuyet") %>" id="btnthoaiduyet" class="clsButton ic-refuse" />
            <input type="button" name="btnaction" onclick="fnaction('','xemso')"  value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.XemSo") %>" id="btnxemso" class="clsButton ic-print" />            
            <input type="button" name="btnaction"  value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.TroGiup") %>" id="Submit8" class="clsButton ic-help" />
            <input type="button" name="btnaction" value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.Dong") %>" id="cmdClose" class="clsButton" />            
        </td>
        <td align="right" style="width:0%"><input type="checkbox" id="cbThemnhieulan" disabled=disabled runat=server /><label for="<%=cbThemnhieulan.ClientID %>">Thêm nhiều lần</label></td>
    </tr>
   </table>
</div>
  <ul style="height:500px">
    <li><a href="#tabs-ThongTinChung"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Tab.ThongTinChung_1")%></a></li> 
    <li><a href="#tabs-ThongTinKiemSoat"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Tab.ThongTinKiemSoat_2")%></a></li>       
  </ul>
  <div id="tabs-ThongTinChung" >
  <asp:Panel runat="server" ID="pnThongTinChung">
    <asp:Panel ID="pntrangthai" runat="server" GroupingText="&nbsp;" CssClass="TitlePanel">
       <table  class="CsTable">
        <tr>
            <td align="left"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.TrangThaiNghiepVu")%></td>
            <td id="tdTrangthai">
                <asp:Label ID="lblTrangThai" runat="server" Text=""></asp:Label></td>
        </tr>
       </table>
    </asp:Panel>
   
    <asp:Panel ID="pnThongtinGiaodich"  runat="server" GroupingText="a" CssClass="TitlePanel">
                     <table  class="CsTable">  
                            <tr>
                                <td style="width:20%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.MaGD")%></td>
                                <td style="width:30%"><asp:TextBox runat=server  ID="txtMaGiaoDich" ReadOnly="true"></asp:TextBox> </td>
                                <td style="width:20%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.Ngay")%></td>
                                <td style="width:30%"><asp:TextBox runat=server  ID="raddtNgay" ReadOnly="true"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td ><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.NguoiGD")%></td>
                                <td><asp:TextBox runat=server  ID="txtNguoiGiaoDich"></asp:TextBox> </td>
                                <td ><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.DiaChi")%></td>
                                <td ><asp:TextBox runat=server  ID="txtDiaChi"></asp:TextBox></td>
                            </tr>
                            <tr style="display:none">
                                <td ><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.HinhThucGiaoDich")%></td>
                                <td><asp:DropDownList runat="server" ID="cmbGD_HinhThuc"></asp:DropDownList></td>
                                <td ><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.LoaiTien")%></td>
                                <td ><asp:DropDownList runat="server" ID="cmbLoaiTien"></asp:DropDownList> </td>
                            </tr>
                            <tr>
                                <td><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.DienGiai")%><font color="red">(*)</font></td>
                                <td colspan="3"><asp:TextBox runat=server  CssClass="bovien_inp"  ID="txtDienGiai" Width="96.5%"></asp:TextBox> </td>
                            </tr>
                    </table>
                </asp:Panel>
    <asp:Panel ID="pnDanhsachGuithem"  runat="server" GroupingText="a" CssClass="TitlePanel">
      <div style="height:45px">
           <table width="100%">
           <tr style="height:50px; vertical-align:text-top">
                <td>
                    <input type="button" name="btnaction"  id="btnadddetail" onclick="GeninfoSotiengui()" class="clsButton ic-add-detail" />
                  <%--  <input type="button" name="btnaction"  id="Button4" class="clsButton ic-edit-detail" />--%>
                    <input type="button" name="btnaction" id="btndeletedetail" onclick="xoadsguithem()"  class="clsButton ic-delete-detail" />                    
                </td>      
            </tr>
           </table>
        </div>
        <div style="overflow: auto;	overflow-x: scroll;" id="divtableContent" style="min-height:260px">        
            <asp:DataGrid runat="server" ID="grGuiThemDS" AutoGenerateColumns="false"
                                        BorderColor="#D9D9D9" CellPadding="4"  DataKeyField="ID"
                                        AlternatingItemStyle-BackColor="#F1F1F2"
                                        BackColor="White" Width="100%"  OnItemDataBound="grGuiThemDS_ItemDataBound"
            BorderWidth="1px">
                                        <ItemStyle CssClass="bordergrid"   Font-Names="Arial" Font-Size="12px" ForeColor="#000"
                                            BackColor="White"></ItemStyle>
                                        <HeaderStyle ForeColor="#FFFFFF" CssClass="tbDataFlowList"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center" Width="15px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <HeaderTemplate>
                                                        <input name="cbrow" type="checkbox" onclick="SelectAllChecBoxWithName(this)" id="chkAll" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input name="cbrow" type="checkbox" tabindex="98888" id="cbrow<%#DataBinder.Eval(Container.DataItem, "ID")%>" />                                                    
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.STT")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "STT")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.SoSo")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "SO_SO_TG")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.TenKhachHang")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem, "TEN_KHANG")%>
                                                 
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.NgayMo")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                
                                                 
                                                 <%#DataBinder.Eval(Container.DataItem, "NGAY_MO_SO")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.NgayDH")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "NGAY_DEN_HAN")%>
                                                
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.KyHan")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "KY_HAN")%>
                                                                                                  
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.SoDu")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <span id="spnval<%#DataBinder.Eval(Container.DataItem, "ID")%>"><%#DataBinder.Eval(Container.DataItem, "SO_DU", "{0:#,##0}")%></span> 
                                               
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.LaiSuat")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem, "LAI_SUAT")%>                                            
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.SoTienGuiThem")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <input type="text" onkeyup="typeval(this)" name="SoTienGuiThem" class="bovien_inp" value="<%#DataBinder.Eval(Container.DataItem, "SO_TIEN_GUI_THEM", "{0:#,##0}")%>" 
                                                style="text-align:right" lang="<%#DataBinder.Eval(Container.DataItem, "ID")%>" id="inp_SoTienGuiThem_<%#DataBinder.Eval(Container.DataItem, "ID")%>" /> 
                                                 
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.SoDuMoi")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <input type="text" class="bovien" value="<%#DataBinder.Eval(Container.DataItem, "SO_DU_MOI", "{0:#,##0}")%>" 
                                                style="text-align:right" tabindex="999999" readonly="readonly" id="inp_SO_DU_MOI_<%#DataBinder.Eval(Container.DataItem, "ID")%>" />                                               
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn Visible="false">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.TaiKhoanThanhToan")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem, "TAI_KHOAN_THANH_TOAN")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                                                                     
                                        </Columns>
                                    </asp:DataGrid>
        </div>
        <div>
                    <table class="CsTable">
                        <tr>
                            <td align="right"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.TongSoSo")%></td>
                            <td align="left"><asp:Label runat="server" Text="0" ID="lblTongSo"></asp:Label></td>  
                             <td align="right"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.TongSoDuCu")%></td>
                            <td align="left"><asp:Label runat="server" Text="0" ID="lblTongoDuCu"></asp:Label></td>       
                             <td align="right"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.TongTienGuiThem")%></td>
                            <td align="left"><asp:Label runat="server" Text="0" ID="lblTongTienGuiThem"></asp:Label></td>                                
                            <td align="right"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.TongSoDuMoi")%></td>
                           <td align="left"><asp:Label runat="server" Text="0" ID="lblTongSoDuMoi"></asp:Label></td>                                  
                        </tr>
                    </table>
        </div>
    </asp:Panel>    
    </asp:Panel>
  </div> 
 
  <div id="tabs-ThongTinKiemSoat" >
    <asp:Panel ID="tbiKiemSoat" Height="100%" runat="server" GroupingText="Thông tin kiểm soát" CssClass="TitlePanel">
     
       <table  class="CsTable" style="width:60%" cellpadding="8px" cellspacing="8px">
        <tr>
            <td style="width:30%" align="left"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.ThongTinKiemSoat.TrangThaiBanGhi:")%></td>
            <td><asp:TextBox runat=server ID="txtTrangThai" ReadOnly="true"   Width="60%" ></asp:TextBox></td>
        </tr>
         <tr>
            <td align="left"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.ThongTinKiemSoat.NgayLap")%></td>
            <td><asp:TextBox runat=server ID="teldtNgayNhap" ReadOnly="true"   Width="60%"></asp:TextBox></td>
        </tr>
         <tr>
            <td align="left"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.ThongTinKiemSoat.NguoiLap")%></td>
            <td><asp:TextBox runat=server ID="txtNguoiLap" ReadOnly="true"    Width="60%"></asp:TextBox></td>
        </tr>
         <tr>
            <td align="left"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.ThongTinKiemSoat.NgayCapNhat")%></td>
            <td><asp:TextBox runat=server ID="teldtNgayCNhat"  ReadOnly="true"   Width="60%"></asp:TextBox></td>
        </tr>
         <tr>
            <td align="left"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.ThongTinKiemSoat.NguoiCapNhat")%></td>
            <td><asp:TextBox runat=server ID="txtNguoiCapNhat"  ReadOnly="true"   Width="60%"></asp:TextBox></td>
        </tr>
       </table>
    </asp:Panel>
   </div>
</div>
</div>
<div id="dialog" title="Thông báo">
  <asp:Label runat="server" ID="lblErr"></asp:Label>
</div>
    <div id="loadingParent" class="loadingParent" style="display: none;">
        <div id="loadingChild" class="loadingChild">
            <img src="Images/Loading.gif" id="loadingImage" alt="Loading..." />
            &nbsp;<span id="loadingTitle">Please wait ...</span>
        </div>
    </div>