var infoCallback = function (id) {
    $('#stub').text('Загрузка...');
    $.ajax({
        url: '/Home/SpaceObjectInfo',
        data: { id: id },
        type: 'get',
        dataType: 'html',
        success: function (data) {
            $('#info').html(data.responseText);
            $('#info').css('height', '725px');
        },
        complete: function (data) {
            $('#info').html(data.responseText);
        }
    });
}