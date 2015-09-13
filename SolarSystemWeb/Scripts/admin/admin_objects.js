
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
        error: function () {
            showAlert('#messages', 'При выполнении запроса произошла ошибка. Поробуйте выполнить операцию позже');
        },
        complete: function () {        
            hideModal();            
        }
    });
}

function setAdminPermission(userId, flag) {
    
    $.ajax({
        url: '/User/ChangeAdminPermission',
        data: { userId: userId, flag: flag },
        type: 'get',
        dataType: 'json',
        success: function () {            
            showSuccess('#messages', 'Данные были успешно изменены');
        },
        error: function () {
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

function onDeleteRole(id, trId) {
    showModal('Внимание!', 'Вы действительно хотите удалить запись?',
        function () {
            deleteRecord("/Roles/Delete", id, trId);
        });
}

function onDeleteUser(id, trId) {
    showModal('Внимание!', 'Вы действительно хотите удалить запись?',
        function () {
            deleteRecord("/User/Delete", id, trId);
        });
}