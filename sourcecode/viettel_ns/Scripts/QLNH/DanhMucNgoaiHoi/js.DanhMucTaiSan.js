var CONFIRM = 0;
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;
function ResetChangePage(iCurrentPage = 1) {
    var sMaLoaiTaiSan = "";
    var sTenLoaiTaiSan = "";
    var sMoTa = "";


    GetListData(sMaLoaiTaiSan, sTenLoaiTaiSan, sMoTa, iCurrentPage);
}

function ChangePage(iCurrentPage = 1) {
    var sMaLoaiTaiSan = $("#txtMaLoaiTaiSan").val();
    var sTenLoaiTaiSan = $("#txtTenLoaiTaiSan").val();
    var sMoTa = $("#txtMoTa").val();

    GetListData(sMaLoaiTaiSan, sTenLoaiTaiSan, sMoTa, iCurrentPage);
}
function GetListData(sMaLoaiTaiSan, sTenLoaiTaiSan, sMoTa, iCurrentPage) {
    _paging.CurrentPage = iCurrentPage;
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/DanhMucTaiSan/DanhMucTaiSanSearch",
        data: { _paging: _paging, sMaLoaiTaiSan: sMaLoaiTaiSan, sTenLoaiTaiSan: sTenLoaiTaiSan, sMoTa: sMoTa },
        success: function (data) {
            $("#lstDataView").html(data);
            $("#txtMaLoaiTaiSan").val(sMaLoaiTaiSan);
            $("#txtTenLoaiTaiSan").val(sTenLoaiTaiSan);
            $("#txtMoTa").val(sMoTa);
        }
    });
}
function OpenModalDetail(id) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/DanhMucTaiSan/GetModalDetail",
        data: { id: id },
        success: function (data) {
            $("#contentModalTaiSan").html(data);
            $("#modalTaiSanLabel").html('Chi tiết tài sản');
        }
    });
}

function OpenModal(id) {

    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/DanhMucTaiSan/GetModal",
        data: { id: id },
        success: function (data) {
            $("#contentModalTaiSan").html(data);
            if (id == null || id == GUID_EMPTY || id == undefined) {
                $("#modalTaiSanLabel").html('Thêm mới tài sản');
            }
            else {
                $("#modalTaiSanLabel").html('Sửa tài sản');
            }
        }
    });
}

function Delete(id) {
    $.ajax({
        type: "POST",
        url: "/QLNH/DanhMucTaiSan/TaiSanDelete",
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


function Xoa(id) {
    var Title = 'Xác nhận xóa chứng từ tài sản';
    var Messages = [];
    Messages.push('Bạn có chắc chắn muốn xóa?');
    var FunctionName = "Delete('" + id + "')";
    $.ajax({
        type: "POST",
        url: "/Modal/OpenModal",
        data: { Title: Title, Messages: Messages, Category: CONFIRM, FunctionName: FunctionName },
        success: function (data) {
            $("#divModalConfirm").empty().html(data);
        }
    });
}
function Save() {
    var data = {};
    data.ID = $("#iID_TaiSanModal").val();
    data.sMaLoaiTaiSan = $("#txtsMaLoaiTaiSan").val();
    data.sTenLoaiTaiSan = $("#txtsTenLoaiTaiSan").val();
    data.sMoTa = $("#txtsMoTa").val();
    console.log(data)
    if (!ValidateData(data)) {
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/QLNH/DanhMucTaiSan/TaiSanSave",
        data: { data: data },
        success: function (r) {

            if (r.bIsComplete) {
                window.location.href = "/QLNH/DanhMucTaiSan";
            }
            else {
                var Title = 'Lỗi lưu tài sản';
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
    var Title = 'Lỗi thêm mới/chỉnh sửa loại hợp đồng';
    var Messages = [];

    if (data.sMaLoaiTaiSan == null || data.sMaLoaiHopDong == "") {
        Messages.push("Mã loại hợp đồng chưa nhập !");
    }

    if (data.sTenLoaiTaiSan == null || data.sTenLoaiHopDong == "") {
        Messages.push("Tên loại hợp đồng chưa nhập");
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