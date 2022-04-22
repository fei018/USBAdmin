
function loadingToReloadPage(second) {
    let layer = layui.layer;
    layer.load(1, { time: second * 1000 });
    setTimeout(function () {
        window.location.reload();
    }, second * 1000)
}

function isReloadPage(yesORno) {
    if (yesORno === undefined) {
        let reload = sessionStorage.getItem('isReloadPage');
        if (reload === 'yes') {
            return true;
        } else {
            return false;
        }
    } else {
        sessionStorage.setItem('isReloadPage', yesORno);
    }
}

function onsubmitFormByAjax(form) {
    //isReloadPage('no');
    let data = $(form).serializeArray();
    let url = $(form).attr('action');
    $.post(url, data, function (json) {
        layer.alert(json.msg, { title: 'Message' });
        isReloadPage('yes');
    }, 'json'
    ).fail(function (jqXHR, textStatus, errorThrown) {
        let error = textStatus + ':' + errorThrown;
        layer.alert(error, { title: 'Error' });
    });
    return false;
}