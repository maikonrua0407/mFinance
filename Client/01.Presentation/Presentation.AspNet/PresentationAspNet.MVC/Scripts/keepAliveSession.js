
KeepAliveIntervalSeconds = 30;
function WebKeepAlive() {
    $.get(
            "/HomePage/Home/KeepAlive",
            function (data) {
                if (data == '0' || (data.length > 0 && data.search('DOCTYPE html') >= 0))
                    window.location = '/Home/Login';
                else {
                    $('#msgSysAlert').html(data);
                    if (data == '') {
                        $('#albSysAlert').css('display', 'none');
                    } else {
                        $('#albSysAlert').css('display', 'block');
                    }
                }
            }
        );

    setTimeout('WebKeepAlive()', (KeepAliveIntervalSeconds * 1000));
}
setTimeout('WebKeepAlive()', (KeepAliveIntervalSeconds * 1000));
// End Keep Alive script 