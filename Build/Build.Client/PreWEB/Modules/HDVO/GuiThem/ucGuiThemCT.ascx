<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGuiThemCT.ascx.cs" Inherits="Presentation.WebClient.Modules.HDVO.GuiThem.ucGuiThemCT" %>

<%@ Import Namespace="Presentation.WebClient.Business" %>
<%@ Import Namespace="Utilities.Common" %>
<script type="text/javascript" language="javascript">



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

        $("#<%=numGD_TongTien.ClientID %>").keyup(function () {
            this.value = commaSplit(removeCharN(this.value, ','), '');
              if ($('#<%=cmbGD_HinhThuc.ClientID %>').val()=='CHUYEN_KHOAN')
                {
                    $("#<%=numGD_SoTienCK.ClientID %>").val(this.value)
                    
               }
                else  $("#<%=numGD_SoTienCK.ClientID %>").val('0')

                var tval = parseFloat( removeCharN(this.value, ','))-parseFloat(removeCharN($("#<%=numGD_SoTienCK.ClientID %>").val(), ','))
                    $('#<%=numGD_SoTienTM.ClientID %>').val(commaSplit(tval,''))
             
          
        });
        $('#<%=numGD_SoTienCK.ClientID %>').keyup(function () {
            this.value = commaSplit(removeCharN(this.value, ','), '');
            var tval = parseFloat(removeCharN($("#<%=numGD_SoTienCK.ClientID %>").val(), ','))
            
            -parseFloat(removeCharN($("#<%=numGD_SoTienCK.ClientID %>").val(), ','))
             $('#<%=numGD_SoTienTM.ClientID %>').val(commaSplit(tval,''))
        });
        //--setReadonly();
        setbuttonmenu();
         
    });
    function fnaction(strconfirm, action) {
        
        var strAction=getObj('<%=cfaction.ClientID %>').value
        
        if (strconfirm!='')
        {
            var sendata=''
            if (action=='THEM')
            {
                sendata= GetFormData(strAction) ;
            }
            else if (action=='SUA')
            {
                sendata=$('#<%=inpID.ClientID %>').val()
            }
            else if (action=='XOA')
            {
                sendata= GetFormData('XOA') ;
                getObj('<%=cfaction.ClientID %>').value='XOA'
                strAction='XOA'
            }
            else if (action=='DUYET')
            {
                sendata= GetFormData('DUYET') ;
                getObj('<%=cfaction.ClientID %>').value='DUYET'
                strAction='DUYET'
            }
            else if (action=='THOAIDUYET')
            {
                sendata= GetFormData('THOAIDUYET') ;
                getObj('<%=cfaction.ClientID %>').value='THOAIDUYET'
                strAction='THOAIDUYET'
            }
            else if (action=='TUCHOIDUYET')
            {
                sendata= GetFormData('TUCHOIDUYET') ;
                getObj('<%=cfaction.ClientID %>').value='TUCHOIDUYET'
                strAction='TUCHOIDUYET'
            }
            if (validdata()) {
                var IDVALUE=$('#<%=txtSoGD.ClientID %>').val()
                getObj('<%=lblErr.ClientID %>').value = ''
                if (confirm(strconfirm)) {
                    $.ajax({
                            type: "POST", url: "PopupContent.aspx/GuiThemCT_"+action,
                            data: "{'data':'" + sendata + "','straction':'"+strAction+"','IDVALUE':'"+IDVALUE+"'}",
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
                                sendata=''
                                if (msg.d != '') {                                    
                                     if (action=='THEM')
                                     {
                                        afterthem(msg.d)
                                     }
                                     else if (action=='SUA')
                                     {
                                         aftersua(msg.d) 
                                     }
                                     else if (action=='XOA' )
                                     {
                                        afterdelete(msg.d)                                        
                                     }
                                     else if (action=='DUYET')
                                     {
                                        afterduyet(msg.d)
                                     }
                                     else if (action=='THOAIDUYET')
                                     {
                                        afterthoaiduyet(msg.d)
                                     }
                                     else if (action=='TUCHOIDUYET')
                                     {
                                        afterthoaiduyet(msg.d)
                                     }
                                }
                              
                            }
                        });
                }
            }
        }
        else
        {
            
             getObj('mainForm').submit()
        }
    }
    function afterdelete(strdata)
    {
        getObj('<%=lblErr.ClientID %>').innerText=strdata.split('#')[1]
        $("#dialog").dialog("open");
        if(strdata.split('#')[0]=='1')
         {  
             setTimeout('window.close()',2500)        
         }
    }
    function aftersua(strdata) 
    {
         if(strdata.split('#')[0]=='0')
            {
                getObj('<%=lblErr.ClientID %>').innerText=strdata.split('#')[1]
                $("#dialog").dialog("open");
            }
            else
            {
                 getObj('<%=cfaction.ClientID %>').value='SUA'
                 getObj('<%=inpAction.ClientID %>').value='SUA'
                 setbuttonmenu()       
            }
    }
    function afterthem(strdata)
    {   
        getObj('<%=lblErr.ClientID %>').innerText=strdata.split('#')[1]        
        $("#dialog").dialog("open");
         if(strdata.split('#')[0]=='1')
         {
            var sResult = strdata.split('#')[2].split('@@');
            $('#<%=txtSoGD.ClientID %>').val(sResult[0])
            getObj('<%=lblTrangThai.ClientID %>').innerText=sResult[1]      
            $('#<%=txtTrangThai.ClientID %>').val(sResult[2])      
            $('#<%=teldtNgayNhap.ClientID %>').val(sResult[3])     
            $('#<%=txtNguoiLap.ClientID %>').val(sResult[4])   
            $('#<%=inpID.ClientID %>')    .val(sResult[5])
            getObj('<%=inpAction.ClientID %>').value='XEM'
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
            $('#<%=numSoDu.ClientID %>').val(sResult[1])            
            getObj('<%=lblTrangThai.ClientID %>').innerText=sResult[2]      
            $('#<%=txtTrangThai.ClientID %>').val(sResult[3]) 
            
            getObj('<%=inpAction.ClientID %>').value='XEMSAUDUYET'
            getObj('<%=cfaction.ClientID %>').value=''
            setbuttonmenu()
        }
    }
    function afterthoaiduyet(strdata)
    {   
        getObj('<%=lblErr.ClientID %>').innerText=strdata.split('#')[1]
        $("#dialog").dialog("open");
        if(strdata.split('#')[0]=='1')
        {
             var sResult = strdata.split('#')[2].split('@@');
            $('#<%=numSoDu.ClientID %>').val(sResult[1])            
            getObj('<%=lblTrangThai.ClientID %>').innerText=sResult[2]      
            $('#<%=txtTrangThai.ClientID %>').val(sResult[3]) 
                       
            getObj('<%=inpAction.ClientID %>').value='SAUTHOAIDUYET'
            getObj('<%=cfaction.ClientID %>').value=''
            setbuttonmenu()    
        }
    }
    function validdata() {
          var chuanhap='<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.ThongBao.ChuaNhap") %>'
          var chuachon='<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.ThongBao.ChuaChon") %>'
          
          if ($('#<%=txtSoTGui.ClientID %>').val()=='')
          {              
              getObj('<%=lblErr.ClientID %>').innerText=chuanhap.replace('{0}','<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.SoSoTGui")%>')
              $("#dialog").dialog("open");
              $('#<%=txtSoTGui.ClientID %>').focus();
              return false;
          }
          else  if ($('#<%=numGD_TongTien.ClientID %>').val()=='' || $('#<%=numGD_TongTien.ClientID %>').val()=='0')
          {              
              getObj('<%=lblErr.ClientID %>').innerText=chuanhap.replace('{0}','<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.TongTienGD")%>')
              $("#dialog").dialog("open");             
              $('#<%=numGD_TongTien.ClientID %>').focus();
              return false;
          }       
       else  if ($('#<%=txtDienGiai.ClientID %>').val()=='')
          {              
              getObj('<%=lblErr.ClientID %>').innerText=chuanhap.replace('{0}','<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.DienGiai")%>')
              $("#dialog").dialog("open");             
              $('#<%=txtDienGiai.ClientID %>').focus();
              return false;
          }          

           return true;
       
    }
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
    function fnsearchTKKH() {
         if ($('#<%=txtSoTGui.ClientID %>').val()=='')
                {
                    getObj('<%=txtSoTGui.ClientID %>').focus()
                    return;
                }
                var m='<%=DatabaseConstant.DanhSachTruyVan.POPUP_DS_TKHOAN_GTHEM_TIEN.getValue() %>'
                var q = '<%=AppConfig.LoginedUser.MaDonVi %>|' + $('#<%=txtSoTGui.ClientID %>').val() + '|<%=BusinessConstant.TinhChatLoaiKhangNBo.KHACH_HANG.layGiaTri() %>'
                openPopUpWindow('Popup.aspx?m='+m+'&q='+q, document.forms[0].<%=txtSoTGui.ClientID %>, '800', '400')
    }
    function fnsearchTKNB() {
         if ($('#<%=txtSoTGui.ClientID %>').val()=='')
                {
                    getObj('<%=txtSoTGui.ClientID %>').focus()
                    return;
                }
                var m='<%=DatabaseConstant.DanhSachTruyVan.POPUP_DS_TKHOAN_GTHEM_TIEN.getValue() %>'
                var q = '<%=AppConfig.LoginedUser.MaDonViGiaoDich %>|MD|<%=BusinessConstant.TinhChatLoaiKhangNBo.NOI_BO.layGiaTri() %>'
                openPopUpWindow('Popup.aspx?m='+m+'&q='+q, document.forms[0].<%=txtSoTGui.ClientID %>, '800', '400')
    }
    var ChecktoUpdate;
    var oldval=''
    function GeninfoSotiengui()
    {  
            oldval=$('#<%=inpIDSO.ClientID %>').val()
            openPopUpWindow('Modules/HDVO/Popup/ucPopupSoTGui.aspx?f=HDV_GUI_THEM_TIEN_THEO_SO&m=false', document.forms[0].<%=inpIDSO.ClientID %>, window.outerWidth - 120, window.outerHeight - 100)     
            ChecktoUpdate = setInterval('afterchoosesoso()', 2000)            
   }
   function setSearchResult(targetField, returnValue) {
            targetField.value = returnValue;
            window.focus();
   }
   function afterchoosesoso()
   {
     
            if ($('#inpIDTarget').val()!=oldval && oldval!='')
            {
             clearInterval(ChecktoUpdate)
             oldval=$('#inpIDTarget').val()
            $.ajax({
            type: "POST", url: "PopupContent.aspx/ThongtinSotiengui",
            data: "{'idSoTGui':" +oldval + "}",
            contentType: "application/json; charset=utf-8", dataType: "json",
            error: function (){reset()},
            beforeSend: function () {
                getObj('loadingParent').style.display = '';            
                                      reset()
                                   }, 
                                    error: function () {                                
                                getObj('loadingParent').style.display = 'NONE'
                            },
            success: function (msg) {
                if (msg.d != '') {
                     getObj('loadingParent').style.display = 'NONE'
                    var sval = msg.d.split('#')   
                    if (sval.length>0)
                    {
                        $('#<%=inpID.ClientID %>').val(sval[0])
                        $('#<%=txtSoTGui.ClientID %>').val(sval[1])
                        $('#<%=numSoDu.ClientID %>').val(sval[2])
                        $('#<%=cmbLoaiTien.ClientID %>').val(sval[3])
                        $('#<%=numLaiSuat.ClientID %>').val(sval[4])
                        $('#<%=raddtNgayMo.ClientID %>').val(sval[5])
                        $('#<%=raddtNgayDH.ClientID %>').val(sval[6])
                        $('#<%=txtMaKH.ClientID %>').val(sval[8])
                        $('#<%=txtTenKH.ClientID %>').val(sval[9])
                        $('#<%=txtDiaChi.ClientID %>').val(sval[10])
                        $('#<%=txtSDT.ClientID %>').val(sval[11])
                        $('#<%=txtSoCMT.ClientID %>').val(sval[12])
                        try{                        
                        $('#<%=raddtNgayCap.ClientID %>').val(sval[13])
                        $('#<%=txtNoiCap.ClientID %>').val(sval[14])                        
                        }catch(e){}
                    }           
                }
            }
        });
        }
   }
   function reset()
   {
    $('#<%=grbThongtinsotiengui.ClientID %>').find('input:text, input:password, input:file, select, textarea')
        .each(function() {
            $(this).val('');
        });     
    $('#<%=grbThongtinKhachang.ClientID %>').find('input:text, input:password, input:file, select, textarea')
        .each(function() {
            $(this).val('');
        }); 

   }
   function changehtgd()
   {    
        if ($('#<%=cmbGD_HinhThuc.ClientID %>').val()=='TIEN_MAT')
        {
            $('#<%=numGD_SoTienTM.ClientID %>').attr('readonly','true');
            $('#<%=numGD_SoTienCK.ClientID %>').attr('readonly','true');
            $('#<%=txtGD_TaiKhoanKH.ClientID %>').attr('readonly','true');
            $('#<%=txtGD_TaiKhoanNB.ClientID %>').attr('readonly','true');
            $('#<%=btnGD_TaiKhoanKH.ClientID %>').attr('readonly','true');
            $('#<%=btnGD_TaiKhoanNB.ClientID %>').attr('readonly','true');
            $('#<%=numGD_SoTienTM.ClientID %>').val($('#<%=numGD_TongTien.ClientID %>').val())
            $('#<%=numGD_SoTienCK.ClientID %>').val('0');
        }
        else if  ($('#<%=cmbGD_HinhThuc.ClientID %>').val()=='TMAT_CKHOAN')
        {
            $('#<%=numGD_SoTienTM.ClientID %>').attr('readonly','true');
            $('#<%=numGD_SoTienCK.ClientID %>').removeAttr('readonly');
            $('#<%=txtGD_TaiKhoanKH.ClientID %>').removeAttr('readonly');
            $('#<%=txtGD_TaiKhoanNB.ClientID %>').removeAttr('readonly');
            $('#<%=btnGD_TaiKhoanKH.ClientID %>').removeAttr('readonly');
            $('#<%=btnGD_TaiKhoanNB.ClientID %>').removeAttr('readonly');
            $('#<%=numGD_SoTienTM.ClientID %>').val($('#<%=numGD_TongTien.ClientID %>').val())
            $('#<%=numGD_SoTienCK.ClientID %>').val('0')
        }
        else
        {
            $('#<%=numGD_SoTienTM.ClientID %>').attr('readonly','true');
            $('#<%=numGD_SoTienCK.ClientID %>').attr('readonly','true');
            $('#<%=txtGD_TaiKhoanKH.ClientID %>').removeAttr('readonly');
            $('#<%=txtGD_TaiKhoanNB.ClientID %>').removeAttr('readonly');
            $('#<%=btnGD_TaiKhoanKH.ClientID %>').removeAttr('readonly');
            $('#<%=btnGD_TaiKhoanNB.ClientID %>').removeAttr('readonly');
            $('#<%=numGD_SoTienTM.ClientID %>').val('0')
            $('#<%=numGD_SoTienCK.ClientID %>').val($('#<%=numGD_TongTien.ClientID %>').val());
        }
   }
   function setReadonly()
   {
        $('#<%=txtSoGD.ClientID %>').attr('readonly','true');
        $('#<%=numSoDu.ClientID %>').attr('readonly','true');
        $('#<%=cmbLoaiTien.ClientID %>').attr('readonly','true');
        $('#<%=numLaiSuat.ClientID %>').attr('readonly','true');
        $('#<%=raddtNgayMo.ClientID %>').attr('readonly','true');
        $('#<%=raddtNgayDH.ClientID %>').attr('readonly','true');
        $('#<%=txtMaKH.ClientID %>').attr('readonly','true');
        $('#<%=txtTenKH.ClientID %>').attr('readonly','true');
        $('#<%=txtDiaChi.ClientID %>').attr('readonly','true');
        $('#<%=txtSoCMT.ClientID %>').attr('readonly','true');
        $('#<%=raddtNgayCap.ClientID %>').attr('readonly','true');
        $('#<%=txtNoiCap.ClientID %>').attr('readonly','true');
        $('#<%=txtSDT.ClientID %>').attr('readonly','true');
        
       
        $('#<%=numGD_SoTienTM.ClientID %>').attr('readonly','true');
        $('#<%=numGD_SoTienCK.ClientID %>').attr('readonly','true');
        $('#<%=txtGD_TaiKhoanKH.ClientID %>').attr('readonly','true');
        $('#<%=txtGD_TaiKhoanNB.ClientID %>').attr('readonly','true');
        $('#<%=btnGD_TaiKhoanKH.ClientID %>').attr('readonly','true');
        $('#<%=btnGD_TaiKhoanNB.ClientID %>').attr('readonly','true');
   } 
   function  SetEnabledAllControls(enable)
        {
           $('#<%=txtSoTGui.ClientID %>').attr('readonly',enable);
           $('#<%=btnSoTgui.ClientID %>').attr('readonly',!enable);

           $('#<%=cmbGD_HinhThuc.ClientID %>').attr('readonly',enable);
           $('#<%=numGD_TongTien.ClientID %>').attr('readonly',enable);
        
            if (enable == true)
            {
                changehtgd()
            }
            else
            {
                
                $('#<%=numGD_SoTienTM.ClientID %>').attr('readonly',enable);
                $('#<%=numGD_SoTienCK.ClientID %>').attr('readonly',enable);
                $('#<%=txtGD_TaiKhoanKH.ClientID %>').attr('readonly',enable);
                $('#<%=txtGD_TaiKhoanNB.ClientID %>').attr('readonly',enable);
                $('#<%=btnGD_TaiKhoanKH.ClientID %>').attr('readonly',!enable);
                $('#<%=btnGD_TaiKhoanNB.ClientID %>').attr('readonly',!enable);
            }
            $('#<%=txtDienGiai.ClientID %>').attr('readonly',enable);
        }
        function setbuttonmenu()
        {          
            if (getObj('<%=inpAction.ClientID %>').value=='XEM')
            {
                $('#btnsua').removeClass( "clsButton-disable ic-edit" ).addClass('clsButton ic-edit')
                $('#btnxoa').removeClass( "clsButton-disable ic-delete" ).addClass('clsButton ic-delete')
                $('#btntrinhduyet').removeClass( "clsButton ic-approve" ).addClass('clsButton-disable ic-approve')
                $('#btnbangke').removeClass( "clsButton-disable ic-print" ).addClass('clsButton-disable ic-print')
                $('#btnduyet').removeClass( "clsButton-disable ic-approve" ).addClass('clsButton ic-approve')
                $('#btntuchoi').removeClass( "clsButton-disable ic-cancel" ).addClass('clsButton ic-cancel')
                $('#btnthoaiduyet').removeClass( "clsButton ic-refuse" ).addClass('clsButton-disable ic-refuse')
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
            }
        }
        function  GetFormData(Saction)
        {
            
                var strdata=''
                //Thông tin sổ               
                strdata+= $('#<%=txtSoGD.ClientID %>').val()+'#';
                strdata+= $('#<%=txtSoTGui.ClientID %>').val()+'#';
                strdata+= removeCharN($('#<%=numSoDu.ClientID %>').val(),',')+'#';
                strdata+= '#';
                strdata+= $('#<%=cmbLoaiTien.ClientID %>').val()+'#';
                strdata+= $('#<%=numLaiSuat.ClientID %>').val()+'#';
                strdata+= $('#<%=raddtNgayMo.ClientID %>').val()+'#';
                strdata+= $('#<%=raddtNgayDH.ClientID %>').val()+'#'  ;              
               
                //Thông tin khách hàng
                strdata+=  $('#<%=txtMaKH.ClientID %>').val()+'#'  ;
                strdata+=  $('#<%=txtTenKH.ClientID %>').val()+'#'  ;
                strdata+=  $('#<%=txtDiaChi.ClientID %>').val()+'#'  ;
                strdata+=  $('#<%=txtSoCMT.ClientID %>').val()+'#'  ;
                strdata+=  $('#<%=raddtNgayCap.ClientID %>').val()+'#'  ;
               
                strdata+=  $('#<%=txtNoiCap.ClientID %>').val()+'#' ;
                strdata+=  $('#<%=txtSDT.ClientID %>').val()+'#';  

                //Thông tin giao dịch
                strdata+= '<%=AppConfig.LoginedUser.MaDonViGiaoDich %>#'
                strdata+= '<%=AppConfig.LoginedUser.MaDonVi %>#';
                strdata+= '<%=AppConfig.LoginedUser.NgayLamViecHienTai %>#';
                strdata+= $('#<%=cmbGD_HinhThuc.ClientID %>').val()+'#';
                strdata+= removeCharN($('#<%=numGD_TongTien.ClientID %>').val(),',')+'#';
                strdata+= removeCharN($('#<%=numGD_SoTienTM.ClientID %>').val(),',')+'#';
                strdata+= removeCharN($('#<%=numGD_SoTienCK.ClientID %>').val(),',')+'#';
                strdata+=$('#<%=txtGD_TaiKhoanKH.ClientID %>').val()+'#';
                strdata+= $('#<%=txtGD_TaiKhoanNB.ClientID %>').val()+'#';
                strdata+= $('#<%=txtDienGiai.ClientID %>').val()+'#';

                //Thông tin kiểm soát
                strdata+= $('#<%=inpTrangThai.ClientID %>').val()+'#';;
                strdata+= '<%=BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri() %>#';
                strdata+= $('#<%=teldtNgayNhap.ClientID %>').val()+'#';
                strdata+= $('#<%=txtNguoiLap.ClientID %>').val()+'#';
                strdata+= Saction+'#';
                if (Saction !='THEM')
                {
                    strdata+= '<%=AppConfig.LoginedUser.NgayLamViecHienTai %>#';
                    strdata+= '<%=AppConfig.LoginedUser.UserName%>#';
                }     
                return strdata   
        }
</script>
<div id="dvDetail" >
<input type="hidden" id="cfaction" value="THEM" runat="server" />
<input type="hidden" id="inpID" value="0" runat="server" />
<input type="hidden" id="inpIDTarget" value="0" />
<input type="hidden" id="inpIDSO" value="0" runat="server" />
<input type="hidden" id="inpAction" value="0" runat="server" />
<input type="hidden" id="inpKetqua" value="0" runat="server" />
<input type="hidden" id="inpshowresult" value="0" runat="server" />
<input type="hidden" id="inpTrangThai" value="" runat="server" />

 <div id="tabs" style="height:auto">
   <div class="navbar"  style="height:45px">
   <table width="100%">
   <tr style="height:50px; vertical-align:text-top">
        <td>
            <%--<input type="button" name="btnaction" disabled="disabled"  value="Thêm" id="Button1" class="clsButton  ic-add" />--%>
            <input type="button" name="btnaction"  onclick="fnaction('Sửa dữ liệu?','SUA')"  value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.Sua") %>" id="btnsua" class="clsButton ic-edit" />
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
       <table  class="CsTable" style="width:50%">
        <tr>
            <td align="left"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.TrangThaiNghiepVu")%></td>
            <td id="tdTrangthai" style="text-align:left">
                <asp:Label ID="lblTrangThai" runat="server" Text=""></asp:Label></td>
        </tr>
       </table>
    </asp:Panel>
    
     <table  class="CsTable" cellpadding=4px cellspacing="4px">
           <tr>
            <td style="width:50%" valign="top">
                <asp:Panel ID="grbThongtinsotiengui"  runat="server" GroupingText="a" CssClass="TitlePanel">
                     <table  class="CsTable">                         
                             <tr>
                                <td style="width:35%" ><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.SoGiaoDich")%></td>
                                <td colspan="3"><asp:TextBox runat=server  ID="txtSoGD"   Width="96%"></asp:TextBox></td>                
                            </tr>
                             <tr>
                                <td style="width:35%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.SoSoTGui")%><font color="red">(*)</font></td>
                                <td colspan="3"><asp:TextBox runat=server  ID="txtSoTGui"  Width="83%"></asp:TextBox><input type="button" id="btnSoTgui" runat="server" onclick="GeninfoSotiengui()" value="F8" /></td>                
                            </tr>
                             <tr>
                                <td style="width:35%" ><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.SoDu")%></td>
                                <td colspan="3"><asp:TextBox runat=server  ID="numSoDu"   Width="96%" ></asp:TextBox></td>                
                            </tr>
                            <tr>
                                <td style="width:35%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.LoaiTien")%></td>
                                <td style="width:25%"><asp:DropDownList runat="server"  ID="cmbLoaiTien"></asp:DropDownList> </td>
                                <td style="width:20%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.LaiSuat")%></td>
                                <td style="width:30%"><asp:TextBox runat=server   ID="numLaiSuat" Width="76%"></asp:TextBox>%</td>
                            </tr>
                             <tr>
                                <td style="width:35%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.NgayMo")%></td>
                                <td style="width:25%"><asp:TextBox runat=server   ID="raddtNgayMo" ></asp:TextBox></td>
                                <td style="width:20%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.NgayDaoHan")%></td>
                                <td style="width:30%"><asp:TextBox runat=server  ID="raddtNgayDH" ></asp:TextBox></td>
                            </tr>
                    </table>
                </asp:Panel>
            
            </td>
            <td style="width:50%" valign="top">
                <asp:Panel ID="grbThongtinKhachang"  runat="server" GroupingText="a" CssClass="TitlePanel">
                     <table  class="CsTable">   
                        <tr>
                            <td style="width:35%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.MaKH")%></td>
                            <td colspan="3"><asp:TextBox runat="server"  ID="txtMaKH"  Width="96%"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width:35%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.TenKH")%></td>
                            <td colspan="3"><asp:TextBox ID="txtTenKH" runat="server"   Width="96%"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width:35%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.DiaChi")%></td>
                            <td colspan="3"><asp:TextBox ID="txtDiaChi" runat="server"  Width="96%"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width:35%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.SoCMND")%></td>
                            <td style="width:25%"><asp:TextBox ID="txtSoCMT" runat="server" ></asp:TextBox></td>
                            <td style="width:20%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.NgayCap")%></td>
                            <td style="width:30%"><asp:TextBox ID="raddtNgayCap" runat="server" ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td ><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.NoiCap")%></td>
                            <td ><asp:TextBox ID="txtNoiCap" runat="server" ></asp:TextBox></td>
                            <td ><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.SDT")%></td>
                            <td ><asp:TextBox ID="txtSDT" runat="server" ></asp:TextBox></td>
                        </tr>
                     </table>
                </asp:Panel>    
            </td>
           </tr>
           <tr>
            <td valign="top" colspan="2">
                 <asp:Panel ID="pnThongtinGiaodich"  runat="server" GroupingText="a" CssClass="TitlePanel">
                     <table  class="CsTable">   
                        <tr>
                            <td style="width:20%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.HinhThucGD")%><font color="red">(*)</font></td>
                            <td style="width:30%"><asp:DropDownList runat="server" ID="cmbGD_HinhThuc"></asp:DropDownList> </td>
                            <td style="width:20%"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.TongTienGD")%><font color="red">(*)</font></td>
                            <td style="width:30%"><asp:TextBox runat="server" ID="numGD_TongTien"></asp:TextBox></td>
                        </tr>
                         <tr>
                            <td><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.SoTienBangTM")%></td>
                            <td><asp:TextBox runat="server" ID="numGD_SoTienTM" ></asp:TextBox></td>
                            <td><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.SoTienCK")%></td>
                            <td><asp:TextBox runat="server" ID="numGD_SoTienCK" ></asp:TextBox></td>
                        </tr>
                         <tr>
                            <td><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.TaiKhoanKH")%></td>
                            <td><asp:TextBox runat="server" ID="txtGD_TaiKhoanKH" Width="80%" ></asp:TextBox><input type="button" id="btnGD_TaiKhoanKH" runat="server" value="F8" disabled=disabled onclick="fnsearchTKKH()"/></td>
                             <td colspan="2"><asp:Label runat="server" ID="lblGD_TaiKhoanKH" Text=""></asp:Label></td>
                        </tr>
                         <tr>
                            <td><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.TaiKhoanNoiBo")%></td>
                            <td><asp:TextBox runat="server" ID="txtGD_TaiKhoanNB" Width="80%"  ></asp:TextBox><input type="button" id="btnGD_TaiKhoanNB"  runat="server" value="F8" disabled=disabled onclick="fnsearchTKNB()"/></td>
                            <td colspan="2"><asp:Label runat="server" ID="lblGD_TaiKhoanNB" Text=""></asp:Label></td>
                           
                        </tr>
                         <tr>
                            <td><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.DienGiai")%><font color="red">(*)</font></td>
                            <td colspan="3"><asp:TextBox runat="server" ID="txtDienGiai" Width="96%"></asp:TextBox></td>                            
                        </tr>
                     </table>
                  </asp:Panel>  
            </td>
           </tr>
           
        </table>
     
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