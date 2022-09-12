var CONFIRM = 0;
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;


function ResetChangePage(iCurrentPage = 1) {
    var sMa = "";
    var sTenVietTat = "";
    var sTen = "";
    var sMoTa = "";

    GetListData(sMa, sTenVietTat, sMoTa, sTen, iCurrentPage);
}

function ChangePage(iCurrentPage = 1) {
    var sMa = $("#txtMa").val();
    var sTenVietTat = $("#txtTenVietTat").val();
    var sMoTa = $("#txtMoTa").val();
    var sTen = $("#txtTen").val();

    GetListData(sMa, sTenVietTat, sMoTa, sTen, iCurrentPage);
}

function GetListData(sMa, sTenVietTat, sMoTa, sTen, iCurrentPage) {
    _paging.CurrentPage = iCurrentPage;
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/DanhMucPhanCapPheDuyet/DanhMucPhanCapPheDuyetSearch",
        data: { _paging: _paging, sMa: sMa, sTenVietTat: sTenVietTat, sMoTa: sMoTa, sTen: sTen },
        success: function (data) {
            $("#lstDataView").html(data);
            $("#txtMa").val(sMa);
            $("#txtTenVietTat").val(sTenVietTat);
            $("#txtMoTa").val(sMoTa);
            $("#txtTen").val(sTen);
        }
    });
}

function OpenModalDetail(id) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/DanhMucPhanCapPheDuyet/GetModalDetail",
        data: { id: id },
        success: function (data) {
            $("#contentModalPhanCapPheDuyet").html(data);
            $("#modalPhanCapPheDuyetLabel").html('Xem chi tiết thông tin phân cấp phê duyệt ');

        }
    });
}

function OpenModal(id) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/DanhMucPhanCapPheDuyet/GetModal",
        data: { id: id },
        success: function (data) {
            $("#contentModalPhanCapPheDuyet").html(data);
            if (id == null || id == GUID_EMPTY || id == undefined) {
                $("#modalPhanCapPheDuyetLabel").html('Thêm mới thông tin phân cấp phê duyệt');
            }
            else {
                $("#modalPhanCapPheDuyetLabel").html('Sửa phân cấp phê duyệt');
            }
        }
    });
}


function Delete(id) {
    $.ajax({
        type: "POST",
        url: "/QLNH/DanhMucPhanCapPheDuyet/PhanCapPheDuyetPopupDelete",
        data: { id: id },
        success: function (data) {
            $("#contentModalPhanCapPheDuyet").html(data);
            if (id == null || id == GUID_EMPTY || id == undefined) {
                $("#modalPhanCapPheDuyetLabel").html('Xóa  !');
            }
            else {
                $("#modalPhanCapPheDuyetLabel").html('Xác nhận xóa thông tin ');
            }
        }

    });
}

function DeleteItem(id) {
    $.ajax({
        type: "POST",
        url: "/QLNH/DanhMucPhanCapPheDuyet/PhanCapPheDuyetDelete",
        data: { id: id },
        success: function (r) {
            if (r.bIsComplete) {
                ChangePage();
            } else {
                var Title = 'Lỗi';
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

function Save() {
    var data = {};
    data.ID = $("#iID_PhanCapPheDuyetModal").val();
    data.sMa = $("#txtsMa").val();
    data.sTenVietTat = $("#txtsTenVietTat").val();
    data.sTen = $("#txtsTen").val();
    data.sMoTa = $("#txtsMoTa").val()

    if (!ValidateData(data)) {
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/QLNH/DanhMucPhanCapPheDuyet/PhanCapPheDuyetSave",
        data: { data: data },
        success: function (r) {
            if (r.bIsComplete) {
                window.location.href = "/QLNH/DanhMucPhanCapPheDuyet";
            } else {
                var Title = 'Lỗi lưu ';
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

function ValidateData(data) {
    var Title = 'Lỗi thêm mới/chỉnh sửa ';
    var Messages = [];

    if (data.sMa == null || data.sMa == "") {
        Messages.push("Mã chưa nhập !");
    }

    if (data.sTenVietTat == null || data.sTenVietTat == "") {
        Messages.push("Tên  chưa nhập !");
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