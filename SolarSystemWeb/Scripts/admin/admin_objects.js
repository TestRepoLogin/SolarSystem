
function deleteRecord(url, id, trId) {
    $.ajax({
        url: url,
        data: { id: id },
        type: 'get',
        dataType: 'json',
        success: function () {
            $('#' + trId).remove();
            showSuccess('#messages', 'Данные были успешно удалены');
        },
        error: function (data) {
            showAlert('#messages', 'При выполнении запроса произошла ошибка. Поробуйте выполнить операцию позже');
        },
        complete: function () {
            hideModal();
        }
    });
}

function onDeleteObject(id, trId) {
    showModal('Внимание!', 'Вы действительно хотите удалить запись?',
        function () {
            deleteRecord("/Admin/DeleteObject", id, trId);
        });
}

function onDeleteObjectType(id, trId) {
    showModal('Внимание!', 'Вы действительно хотите удалить запись?',
        function () {
            deleteRecord("/Admin/DeleteObjectType", id, trId);
        });
}
