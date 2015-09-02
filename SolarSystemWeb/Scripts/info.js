var infoCallback = function (id) {
    $('#stub').text('Загрузка...');
    $.ajax({
        url: '/Home/SpaceObjectInfo',
        data: { id: id },
        type: 'get',
        dataType: 'html',
        success: function (data) {
            $('#info').html(data.responseText);
        },
        complete: function (data) {
            $('#info').html(data.responseText);
        }
    });
}