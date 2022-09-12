var CONFIRM = 0;
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;

$(document).ready(function () {
    var inputGiaiDoanTu = $('#iGiaiDoanTu');
    inputGiaiDoanTu.on('keyup', function () {
        var key = event.keyCode || event.charCode;
        var iGiaiDoanTu = $("#iGiaiDoanTu").val();
        if (iGiaiDoanTu != "") {
            $("#iGiaiDoanDen").val(parseInt(this.value) + 4);
        } else {
            $("#iGiaiDoanDen").val("");
        }
    });

    var inputGiaiDoanDen = $('#iGiaiDoanDen');
    inputGiaiDoanDen.on('keyup', function () {
        var key = event.keyCode || event.charCode;
        var iGiaiDoanDen = $("#iGiaiDoanDen").val();
        if (iGiaiDoanDen != "") {
            $("#iGiaiDoanTu").val(parseInt(this.value) - 4);
        } else {
            $("#iGiaiDoanTu").val("");
        }
    });
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
        url: "/QLVonDauTu/KeHoachTrungHanDeXuat/LoadDataExcel",
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
//duonglt test 
function downloadImpExp() {
    window.location.href = "/QLVonDauTu/KeHoachTrungHanDeXuat/DownloadImportExample";
}
//end test

function ValidateData() {
    var Title = 'Lỗi lấy dữ liệu từ file excel';
    var Messages = [];

    var has_file = $("#FileUpload").val() != '';
    if (!has_file) {
        Messages.push("Đ/c chưa chọn file excel dữ liệu !");
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
function LayGiaTriThayDoiTheoChungTuDuocChon(iID_KeHoach5Nam_DeXuatID) {
    $.ajax({
        url: "/QLVonDauTu/KeHoachTrungHanDeXuat/LayGiaTriThayDoiTheoChungTuDuocChon",
        type: "POST",
        data: { iID_KeHoach5Nam_DeXuatID: iID_KeHoach5Nam_DeXuatID },
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data != null) {
                const split = data.split("  ");
                $("#iID_DonViQuanLyID").html(split[0]);
                document.getElementById('iGiaiDoanTu').setAttribute("value", split[1]);
                document.getElementById('iGiaiDoanDen').setAttribute("value", split[2]);
                $("#ValueItemCt").html(split[3]);
            }
        },
        error: function (data) {

        }
    })
}

