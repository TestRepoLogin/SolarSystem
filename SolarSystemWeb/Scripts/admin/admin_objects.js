$().ready(
    function() {
        $('.objects').addClass('active');
    });

function deleteObject(id, trId) {
    $.ajax({
        url: "/Admin/DeleteObject",
        data: { id: id },
        type: 'get',
        dataType: 'json',
        success: function () {
            $('#' + trId).remove();
        },
        error: function (data) {
            showAlert('#messages', 'При выполнении запроса произошла ошибка. Поробуйте выполнить операцию позже');
        },
        complete: function () {
            hideModal();
        }
    });
}

function onDelete(id, trId) {
    showModal('Внимание!', 'Вы действительно хотите удалить запись?',
        function () {
            deleteObject(id, trId);
        });
}
