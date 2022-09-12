var CONFIRM = 0;
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;

$(document).ready(function () {
    
});

function loadDataExcel() {
    if (!ValidateData()) {
        return false;
    }

    var fileInput = document.getElementById('FileUpload');
    var file = fileInput.files[0];
    var formData = new FormData();
    formData.append('file', file);
    $.ajax({
        type: "POST",
        url: "/QLVonDauTu/KeHoachVonNamDeXuat/LoadDataExcel",
        data: formData,
        contentType: false,
        processData: false,
        cache: false,
        async: false,
        success: function (r) {
            if (r.bIsComplete) {
                loadFrame();
            } else {
                var Title = 'Lỗi lấy dữ liệu từ file excel';
                var messErr = [];
                messErr.push(r.sMessError);
                $.ajax({
                    type: "POST",
                    url: "/Modal/OpenModal",
                    data: { Title: Title, Messages: messErr, Category: ERROR },
                    success: function (data) {
                        $("#divModalConfirm").html(data);
                    }
                });
            }
        }
    });
}
function ValidateData() {
    var Title = 'Lỗi lấy dữ liệu từ file excel';
    var Messages = [];

    var has_file = $("#FileUpload").val() != '';
    if (!has_file) {
        Messages.push("Đ/c chưa chọn file excel dữ liệu !");
    }
    else {
        var ext = $("#FileUpload").val().split(".");
        ext = ext[ext.length - 1].toLowerCase();
        var arrayExtensions = ["xls", "xlsx"];
        if (arrayExtensions.lastIndexOf(ext) == -1) {
            Messages.push("Đ/c chọn file không đúng định dạng !");
        }
    }

    if (Messages != null && Messages != undefined && Messages.length > 0) {
        $.ajax({
            type: "POST",
            url: "/Modal/OpenModal",
            data: { Title: Title, Messages: Messages, Category: ERROR },
            success: function (data) {
                $("#divModalConfirm").html(data);
            }
        });
        return false;
    }
    return true;
}
