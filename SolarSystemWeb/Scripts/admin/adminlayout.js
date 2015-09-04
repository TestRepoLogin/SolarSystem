function showModal(title, text, onOkCallback) {
    $('.modal .modal-title').text(title);
    $('.modal .modal-body').html(text);

    $('.modal .ok').off('click');
    $('.modal .ok').on('click', onOkCallback);

    $('.modal').modal('show');
}

function hideModal() {
    $('.modal').modal('hide');
}

function showAlert(parentSelector, text) {
    var alert = '<div class="alert alert-dismissible alert-danger">' +
                    '<button type="button" class="close" data-dismiss="alert">×</button>' +
                    text +
                '</div>';
    $(parentSelector).append(alert);
}

function showSuccess(parentSelector, text) {
    var alert = '<div class="alert alert-dismissible alert-success">' +
                    '<button type="button" class="close" data-dismiss="alert">×</button>' +
                    text +
                '</div>';
    $(parentSelector).append(alert);
}