var CONFIRM = 0;
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;

function ResetChangePage(iCurrentPage = 1) {
    var sMaTiGia = "";
    var sTenTiGia = "";
    var sMoTaTiGia = "";
    var sMaTienTeGoc = "";
    var dNgayTao = "";

    GetListData(dNgayTao, sMaTiGia, sTenTiGia, sMoTaTiGia, sMaTienTeGoc, iCurrentPage);
}

function ChangePage(iCurrentPage = 1) {
    var sMaTiGia = $("<div/>").text($.trim($("#txtMaTiGia").val())).html();
    var sTenTiGia = $("<div/>").text($.trim($("#txtTenTiGia").val())).html();
    var sMoTaTiGia = $("<div/>").text($.trim($("#txtMoTaTiGia").val())).html();
    var sMaTienTeGoc = $("<div/>").text($.trim($("#txtMaTienTeGoc").val())).html();
    var dNgayLap = $("<div/>").text($.trim($("#txtNgayLapFilter").val())).html();

    GetListData(dNgayLap, sMaTiGia, sTenTiGia, sMoTaTiGia, sMaTienTeGoc, iCurrentPage);
}

function GetListData(dNgayLap, sMaTiGia, sTenTiGia, sMoTaTiGia, sMaTienTeGoc, iCurrentPage) {
    _paging.CurrentPage = iCurrentPage;
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/DanhMucTyGia/DanhMucTiGiaSearch",
        data: { _paging: _paging, dNgayLap: dNgayLap, sMaTiGia: sMaTiGia, sTenTiGia: sTenTiGia, sMoTaTiGia: sMoTaTiGia, sMaTienTeGoc: sMaTienTeGoc },
        success: function (data) {
            $("#lstDataView").html(data);

            $("#txtNgayLapFilter").val($("<div/>").html(dNgayLap).text());
            $("#txtMaTiGia").val($("<div/>").html(sMaTiGia).text());
            $("#txtTenTiGia").val($("<div/>").html(sTenTiGia).text());
            $("#txtMoTaTiGia").val($("<div/>").html(sMoTaTiGia).text());
            $("#txtMaTienTeGoc").val($("<div/>").html(sMaTienTeGoc).text());
        }
    });
}

function OpenModalDetail(id) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/DanhMucTyGia/GetModalDetail",
        data: { id: id },
        success: function (data) {
            $("#contentModalTyGia").html(data);
            $("#modalTyGiaLabel").html('Xem chi tiết thông tin tỉ giá hối đoái');
            $("#contentModalTyGia tr").hover(function () {
                $(this).css("background-color", "#e7f8fe");
            }, function () {
                $(this).css("background-color", "");
            });
        }
    });
}

function OpenModal(id) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/DanhMucTyGia/GetModal",
        data: { id: id },
        success: function (data) {
            $("#contentModalTyGia").html(data);
            if (id == null || id == GUID_EMPTY || id == undefined) {
                $("#modalTyGiaLabel").html('Thêm mới thông tin tỉ giá hối đoái');
            }
            else {
                $("#modalTyGiaLabel").html('Sửa thông tin tỉ giá hối đoái');
            }
            $(".date").datepicker({
                todayBtn: "linked",
                language: "vi",
                keyboardNavigation: false,
                forceParse: false,
                autoclose: true,
                todayHighlight: true,
                format: 'dd/mm/yyyy'
            });

            $("#contentModalTyGia tr").hover(function () {
                $(this).css("background-color", "#e7f8fe");
            }, function () {
                $(this).css("background-color", "");
            });

            document.querySelectorAll(".colGiaTriTiGia").forEach(function (element) {
                setInputFilter(element, function (value) {
                    return /^\d*\,?\d*$/.test(value);
                }, "Vui lòng nhập số và dấu phẩy \",\"!");
            });

            setInputFilter(document.getElementById("txtNgayLap"), function (value) {
                return /^\d{0,2}\/?\d{0,2}\/?\d{0,4}$/.test(value);
            }, "Ngày đã nhập không đúng định dạng dd/MM/yyyy hoặc không hợp lệ!", 2);
        }
    });
}

function Delete(id) {
    var Title = 'Xác nhận xóa tỉ giá hối đoái';
    var Messages = [];
    Messages.push('Bạn có chắc chắn muốn xóa?');
    var FunctionName = "DeleteItem('" + id + "')";
    $.ajax({
        type: "POST",
        url: "/Modal/OpenModal",
        data: { Title: Title, Messages: Messages, Category: CONFIRM, FunctionName: FunctionName },
        success: function (data) {
            $("#divModalConfirm").empty().html(data);
        }
    });
}

function DeleteItem(id) {
    $.ajax({
        type: "POST",
        url: "/QLNH/DanhMucTyGia/TyGiaDelete",
        data: { id: id },
        success: function (r) {
            if (r && r.bIsComplete) {
                ChangePage();
            } else {
                var Title = 'Lỗi xóa tỉ giá';
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

function GetDataTiGiaChiTiet() {
    var dataTiGiaChiTiet = [];
    var obj;
    $.each($("#tblTiGiaChiTiet tbody tr"), function (index, item) {
        var fTiGia = $(item).find(".colGiaTriTiGia").val();
        if (fTiGia == null || fTiGia == "") return;
        obj = {};
        obj.ID = $(item).data("idtgct");
        obj.iID_TiGiaID = ($("#iID_TyGiaModal").val() == "" || $("#iID_TyGiaModal").val() == GUID_EMPTY) ? null : $("#iID_TyGiaModal").val();
        obj.iID_TienTeID = $(item).data("idtiente");
        obj.sMaTienTeQuyDoi = $(item).find(".colTienTeQuyDoi").html();
        obj.fTiGia = UnFormatNumber(fTiGia);
        dataTiGiaChiTiet.push(obj);
    });
    return dataTiGiaChiTiet;
}

function Save() {
    var dataTiGia = {};
    dataTiGia.ID = $("#iID_TyGiaModal").val();
    dataTiGia.sMaTiGia = $("<div/>").text($.trim($("#txtsMaTyGia").val())).html();
    dataTiGia.sTenTiGia = $("<div/>").text($.trim($("#txtsTenTyGia").val())).html();
    dataTiGia.sMoTaTiGia = $("<div/>").text($.trim($("#txtsMotaTyGia").val())).html();
    dataTiGia.iID_TienTeGocID = $("#slbMaTienTeGoc").val();
    dataTiGia.sMaTienTeGoc = $("<div/>").text($.trim($("#slbMaTienTeGoc option:selected").data("matiente"))).html();
    dataTiGia.dThangLapTiGia = $("<div/>").text($.trim($("#txtNgayLap").val())).html();

    if (!ValidateData(dataTiGia)) {
        return false;
    }
    var dataTiGiaChiTietParam = GetDataTiGiaChiTiet();

    $.ajax({
        type: "POST",
        url: "/QLNH/DanhMucTyGia/TyGiaSave",
        data: { dataTiGia: dataTiGia, dataTiGiaChiTietParam: dataTiGiaChiTietParam },
        success: function (r) {
            if (r.bIsComplete) {
                window.location.href = "/QLNH/DanhMucTyGia";
            } else {
                var Title = 'Lỗi lưu tỉ giá';
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
    var Title = 'Lỗi thêm mới/chỉnh sửa tỉ giá';
    var Messages = [];

    if (data.sMaTiGia == null || data.sMaTiGia == "") {
        Messages.push("Mã tỉ giá chưa nhập !");
    }

    if (data.sTenTiGia == null || data.sTenTiGia == "") {
        Messages.push("Tên tỉ giá chưa nhập !");
    }

    if (data.iID_TienTeGocID == null || data.iID_TienTeGocID == GUID_EMPTY) {
        Messages.push("Mã tiền tệ gốc chưa chọn !");
    }

    if (Messages.length > 0) {
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

function ChangeMaTienTeGocSelect() {
    var idTiGia = $("#iID_TyGiaModal").val();
    if (idTiGia != "" && idTiGia != GUID_EMPTY) return;
    var idTienTeGoc = $("#slbMaTienTeGoc").val();
    if (idTienTeGoc == GUID_EMPTY) {
        $("#tblTiGiaChiTiet tbody").empty();
        return;
    }
    var idTienTeGocCu = $("#hidTienTeGocID").val();
    var maTienTeGoc = $("<div/>").text($("#slbMaTienTeGoc option:selected").data("matiente")).html();
    $.ajax({
        type: "POST",
        url: "/QLNH/DanhMucTyGia/ChangeMaTienTeGoc",
        data: { idTiGia: idTiGia, idTienTeGoc: idTienTeGoc, maTienTeGoc: maTienTeGoc, idTienTeGocCu: idTienTeGocCu },
        success: function (res) {
            if (res) {
                $("#tblTiGiaChiTiet tbody").empty().html(res.htmlStr);
                $("#contentModalTyGia tr").hover(function () {
                    $(this).css("background-color", "#e7f8fe");
                }, function () {
                    $(this).css("background-color", "");
                });

                document.querySelectorAll(".colGiaTriTiGia").forEach(function (element) {
                    setInputFilter(element, function (value) {
                        return /^\d*\,?\d*$/.test(value);
                    }, "Vui lòng nhập số và dấu phẩy \",\"!");
                });
            }
        }
    });
}