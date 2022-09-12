var CONFIRM = 0;
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;
var CURRENT_STATE = 'CREATE'

function ResetChangePage(iCurrentPage = 1) {
    GetListData("", "", GUID_EMPTY, GUID_EMPTY, GUID_EMPTY, GUID_EMPTY, null, null, null, null, null, null, null, null, null, null, null, null, null, iCurrentPage);
}

function ChangePage(iCurrentPage = 1) {
    var sMaDuAn = $("<div/>").text($("#txtMaDuAnFilter").val()).html();
    var sTenDuAn = $("#txtTenDuAnFilter").val();
    var sBQuanLy = $("#slbBQuanLyFilter").val();
    var sDonVi = $("#slbsDonViFilter").val();
    var sChuDauTu = $("#slbChuDauTuFilter").val();
    var sPhanCapPheDuyet = $("#slbPhanCapPheDuyetFilter").val();
    var sChuongTrinh = $("#slbChuongTrinh").val();
    var sSoChuTruongDauTu = $("<div/>").text($("#txtSoChuTruongDauTuFilter").val()).html();
    var dNgayChuTruongDauTu = $("<div/>").text($("#txtNgayBanHanhCTDTFilter").val()).html();
    var sSoQuyetDinhDauTu = $("<div/>").text($("#txtSoQuyetDinhDauTuFilter").val()).html();
    var dNgayQuyetDinhDauTu = $("<div/>").text($("#txtNgayBanHanhQuyetDinhDauTuFilter").val()).html();
    var sSoDuToan = $("<div/>").text($("#txtSoDuToanTuFilter").val()).html();
    var dNgayDuToan = $("<div/>").text($("#txtNgayBanHanhDuToanFilter").val()).html();
    var sKhoiCong = $("<div/>").text($("#txtThoiGianThucHienTuFilter").val()).html();
    var sKetThuc = $("<div/>").text($("#txtThoiGianThucHienDenFilter").val()).html();
    var fGiaTriUSD = $("<div/>").text($("#txtGiaTriUSDFilter").val()).html();
    var fGiaTriVND = $("<div/>").text($("#txtGiaTriVNDFilter").val()).html();
    var fGiaTriEUR = $("<div/>").text($("#txtGiaTriEURFilter").val()).html();
    var fGiaTriNgoaiTeKhac = $("<div/>").text($("#txtGiaTriNgoaiTeKhacFilter").val()).html();

    GetListData(sMaDuAn, sTenDuAn, sBQuanLy, sDonVi, sChuDauTu, sPhanCapPheDuyet, sChuongTrinh, sSoChuTruongDauTu,
        dNgayChuTruongDauTu, sSoQuyetDinhDauTu, dNgayQuyetDinhDauTu, sSoDuToan, dNgayDuToan, sKhoiCong, sKetThuc, fGiaTriUSD, fGiaTriVND, fGiaTriEUR, fGiaTriNgoaiTeKhac, iCurrentPage);
}

function GetListData(sMaDuAn, sTenDuAn, bQuanLy, sDonVi, sChuDauTu, sPhanCapPheDuyet, sChuongTrinh, sSoChuTruongDauTu,
    dNgayChuTruongDauTu, sSoQuyetDinhDauTu, dNgayQuyetDinhDauTu, sSoDuToan, dNgayDuToan, sKhoiCong, sKetThuc, fGiaTriUSD, fGiaTriVND, fGiaTriEUR, fGiaTriNgoaiTeKhac, iCurrentPage) {
    _paging.CurrentPage = iCurrentPage;
    $.ajax({
        type: "POST",
        dataType: "html",
        url: sUrlListView,
        data: {
            _paging: _paging, sMaDuAn: sMaDuAn, sTenDuAn: sTenDuAn, iID_BQuanLyID: bQuanLy, iID_DonViID: sDonVi, iID_ChuDauTuID: sChuDauTu, iID_CapPheDuyetID: sPhanCapPheDuyet, iID_KHCTBQP_ChuongTrinhID: sChuongTrinh,
            sSoChuTruongDauTu: sSoChuTruongDauTu, dNgayChuTruongDauTu: dNgayChuTruongDauTu, sSoQuyetDinhDauTu: sSoQuyetDinhDauTu, dNgayQuyetDinhDauTu: dNgayQuyetDinhDauTu,
            sSoDuToan: sSoDuToan, dNgayDuToan: dNgayDuToan, sKhoiCong: sKhoiCong, sKetThuc: sKetThuc, fGiaTriUSD: fGiaTriUSD, fGiaTriVND: fGiaTriVND, fGiaTriEUR: fGiaTriEUR, fGiaTriNgoaiTeKhac: fGiaTriNgoaiTeKhac
        },
        success: function (data) {
            $("#lstDataView").html(data);
            $("#txtMaDuAnFilter").val($("<div/>").html(sMaDuAn).text());
            $("#txtTenDuAnFilter").val($("<div/>").html(sTenDuAn).text());
            $("#slbBQuanLyFilter").val(bQuanLy);
            $("#slbsDonViFilter").val(sDonVi);
            $("#slbChuDauTuFilter").val(sChuDauTu);
            $("#slbPhanCapPheDuyetFilter").val(sPhanCapPheDuyet);
            $("#slbChuongTrinhFilter").val(sChuongTrinh);
            $("#txtSoChuTruongDauTuFilter").val($("<div/>").html(sSoChuTruongDauTu).text());
            $("#txtNgayBanHanhCTDTFilter").val($("<div/>").html(dNgayChuTruongDauTu).text());
            $("#txtSoQuyetDinhDauTuFilter").val($("<div/>").html(sSoQuyetDinhDauTu).text());
            $("#txtNgayBanHanhQuyetDinhDauTuFilter").val($("<div/>").html(dNgayQuyetDinhDauTu).text());
            $("#txtSoDuToanTuFilter").val($("<div/>").html(sSoDuToan).text());
            $("#txtNgayBanHanhDuToanFilter").val($("<div/>").html(dNgayDuToan).text());
            $("#txtThoiGianThucHienTuFilter").val($("<div/>").html(sKhoiCong).text());
            $("#txtThoiGianThucHienDenFilter").val($("<div/>").html(sKetThuc).text());
            $("#txtGiaTriUSDFilter").val($("<div/>").html(fGiaTriUSD).text());
            $("#txtGiaTriVNDFilter").val($("<div/>").html(fGiaTriVND).text());
            $("#txtGiaTriEURFilter").val($("<div/>").html(fGiaTriEUR).text());
            $("#txtGiaTriNgoaiTeKhacFilter").val($("<div/>").html(fGiaTriNgoaiTeKhac).text());
        }
    });
}


function OpenModalDetail(id) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/ThongTinDuAn/GetModalDetail",
        data: { id: id },
        success: function (data) {
            $("#lstDataView").html(data);
            $("#modalThongTinDuAnLabel").html();
        }
    });
}

function Cancel() {
    window.location.href = "/QLNH/ThongTinDuAn";
}

function OpenModal(id, state) {
    let isDieuChinh = false;
    if (state == 'ADJUST') {
        isDieuChinh = true;
    }
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/ThongTinDuAn/GetModal",
        data: { id: id, isDieuChinh: isDieuChinh },
        async: false,
        success: function (data) {
            $("#lstDataView").html(data);
            if (state == 'CREATE') {
                $("#modalThongTinDuAnLabel").empty().html("Thêm mới thông tin dự án");
            } else if (state == 'UPDATE') {
                $("#modalThongTinDuAnLabel").empty().html();
            } else {
                $("#modalThongTinDuAnLabel").empty().html();
            }
            $('.date')
                .datepicker({
                    todayBtn: "linked",
                    keyboardNavigation: false,
                    forceParse: false,
                    autoclose: true,
                    language: 'vi',
                    todayHighlight: true,
                });
        }
    });

    this.CURRENT_STATE = state;
}



function Xoa(id) {
    var Title = 'Xác nhận xóa thông tin dự án';
    var Messages = [];
    Messages.push('Bạn có chắc chắn muốn xóa :');
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
        url: "/QLNH/ThongTinDuAn/ThongTinDuAnDelete",
        data: { id: id },
        success: function (data) {
            if (data && data.bIsComplete) {
                ChangePage();
            } else {
                if (data.sMessError != "") {
                    var Title = 'Lỗi xóa thông tin dự án';
                    $.ajax({
                        type: "POST",
                        url: "/Modal/OpenModal",
                        data: { Title: Title, Messages: [data.sMessError], Category: ERROR },
                        success: function (res) {
                            $("#divModalConfirm").html(res);
                        }
                    });
                }
            }
        }
    });
}

function Save() {
    let state = this.CURRENT_STATE;
    var data = GetDataDuAn(data);
    let oldId = $("#hidDuAnID").val();
    if (state != 'ADJUST') {
        data.ID = $("#hidDuAnID").val();
        $(this).find("select").each(function (index) {
            const fieldName = $(this).attr("name");
            allValues[fieldName] = $(this).val();
        });

    }
    let dataTableChiPhi = [];
    $("#tbodyTableChiPhi tr").each(function () {
        let allValues = {};
        $(this).find("input").each(function (index) {
            const fieldName = $(this).attr("name");
            allValues[fieldName] = UnFormatNumber($(this).val());
        });
        $(this).find("select").each(function (index) {
            const fieldName = $(this).attr("name");
            allValues[fieldName] = $(this).val();
        });

        dataTableChiPhi.push(allValues);
    });

    if (!ValidateData(data)) {
        return false;
    }
    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTinDuAn/Save",
        data: { data: data, state: state, dataTableChiPhi: dataTableChiPhi, oldId: oldId },
        success: function (r) {
            if (r.bIsComplete) {
                window.location.href = "/QLNH/ThongTinDuAn";
            } else {
                var Title = 'Lỗi lưu thông tin dự án';
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
function GetDataDuAn() {
    var data = {};
    data.ID = $("#hidDuAnID").val();
    data.iID_DonViID = $("#slbDonVi").val();
    data.iID_BQuanLyID = $("#slbBQuanLy").val();
    data.iID_MaDonVi = $("#slbDonVi").find("option:selected").data("madonvi");
    data.iID_KHCTBQP_ChuongTrinhID = $("#slbChuongTrinh").val();
    data.sMaDuAn = $("<div/>").text($.trim($("#txtMaDuAn").val())).html();
    data.sTenDuAn = $("<div/>").text($.trim($("#txtTenDuAn").val())).html();
    data.sSoChuTruongDauTu = $("<div/>").text($.trim($("#txtSoChuTruongDauTu").val())).html();
    data.dNgayChuTruongDauTu = $("#txtNgayBanHanhCTDT").val();
    data.sSoQuyetDinhDauTu = $("<div/>").text($.trim($("#txtSoQuyetDinhDauTu").val())).html();
    data.dNgayQuyetDinhDauTu = $("#txtNgayBanHanhQuyetDinhDauTu").val();
    data.sSoDuToan = $("<div/>").text($.trim($("#txtSoDuToanTu").val())).html();
    data.dNgayDuToan = $("#txtNgayBanHanhDuToan").val();
    data.iID_ChuDauTuID = $("#slbChuDauTu").val() == GUID_EMPTY ? null : $("#slbChuDauTu").val();
    data.iID_CapPheDuyetID = $("#slbPhanCapPheDuyet").val() == GUID_EMPTY ? null : $("#slbPhanCapPheDuyet").val();
    data.sKhoiCong = $("<div/>").text($.trim($("#txtThoiGianThucHienTu").val())).html();
    data.sKetThuc = $("<div/>").text($.trim($("#txtThoiGianThucHienDen").val())).html();
    data.iID_TiGiaID = $("#slbTiGia").val() == GUID_EMPTY ? null : $("#slbTiGia").val();
    data.sMaNgoaiTeKhac = $("#slbMaNgoaiTeKhac").val() == GUID_EMPTY ? null : $("#slbMaNgoaiTeKhac").val();
    data.sGiaTriUSD = UnFormatNumber($("<div/>").text($.trim($("#txtHopDongUSD").val())).html());
    data.sGiaTriVND = UnFormatNumber($("<div/>").text($.trim($("#txtHopDongVND").val())).html());
    data.sGiaTriEUR = UnFormatNumber($("<div/>").text($.trim($("#txtHopDongEUR").val())).html());
    data.sGiaTriNgoaiTeKhac = UnFormatNumber($("<div/>").text($.trim($("#txtHopDongNgoaiTeKhac").val())).html());

    return data;
}
function ValidateData(data) {
    var Title = 'Lỗi thêm mới/chỉnh sửa thông tin dự án';
    var Messages = [];

    if (data.sMaDuAn == null || data.sMaDuAn == "") {
        Messages.push("Lỗi thêm mới mã dự án !");
    }

    if (data.sTenDuAn == null || data.sTenDuAn == "") {
        Messages.push("Lỗi thêm mới tên dự án !");
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
function Import() {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/ThongTinDuAn/ImportThongTinDuAn",
        success: function (data) {
            $("#contentModalThongTinDuAn").empty().html(data);
            $("#modalThongTinDuAnLabel").empty().html('Import thông tin dự án');
        }
    });
}
function DownloadFileTemplate() {
    var url = "/QLNH/QLThongTinDuAn/DownloadTemplateImport";
    var arrLink = [];
    arrLink.push(url);
    openLinks(arrLink);
}

function ChangeFile(element) {
    var filePath = $(element).val();
    var fileNameArr = filePath.split("\\");
    $("#lblUpload span").empty().html(fileNameArr[fileNameArr.length - 1]);
}

function RefreshImport() {
    $("#inpFileUpload").val("");
    $("#lblUpload span").empty().html("Lựa chọn file excel");
    $("#tblThongTinDuAnListImport tbody").empty();
}

function LoadDataExcel() {
    if (!ValidateFileImport()) {
        return false;
    }

    var fileInput = document.getElementById('inpFileUpload');
    var file = fileInput.files[0];
    var formData = new FormData();
    formData.append('file', file);
    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTinDuAn/LoadDataExcel",
        data: formData,
        contentType: false,
        processData: false,
        cache: false,
        async: false,
        success: function (r) {
            if (r.bIsComplete) {
                $("#tblThongTinDuAnListImport tbody").empty().html(r.data);
                $(".selectBQP").select2({ width: '100%', dropdownAutoWidth: true, matcher: FilterInComboBox });
                $(".selectBQuanLy").select2({ width: '100%', dropdownAutoWidth: true, matcher: FilterInComboBox });
                $(".selectDonVi").select2({ width: '100%', dropdownAutoWidth: true, matcher: FilterInComboBox });
                $(".selectChuongTrinh").select2({ width: '100%', dropdownAutoWidth: true, matcher: FilterInComboBox });
                $(".selectPhanCapPheDuyet").select2({ width: '100%', dropdownAutoWidth: true, matcher: FilterInComboBox });
                $(".selectChuDauTu").select2({ width: '100%', dropdownAutoWidth: true, matcher: FilterInComboBox });
                document.querySelectorAll(".inputDate").forEach(function (element) {
                    setInputFilter(element, function (value) {
                        return /^\d{0,2}\/?\d{0,2}\/?\d{0,4}$/.test(value);
                    }, "Ngày đã nhập không đúng định dạng dd/MM/yyyy hoặc không hợp lệ!", 2);
                });
                document.querySelectorAll(".inputThoiGianThucHien").forEach(function (element) {
                    setInputFilter(element, function (value) { return /^\d*$/.test(value); }, "Vui lòng nhập số!", 3);
                });
                $('.inputDate').datepicker({
                    todayBtn: "linked",
                    keyboardNavigation: false,
                    forceParse: false,
                    autoclose: true,
                    language: 'vi',
                    todayHighlight: true,
                });
                $("#tblThongTinDuAnListImport tbody tr").hover(function () {
                    $(this).css("background-color", "#e7f8fe");
                }, function () {
                    $(this).css("background-color", "");
                });
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

function DownloadFileTemplate() {
    var url = "/QLNH/ThongTinDuAn/DownloadTemplateImport";
    var arrLink = [];
    arrLink.push(url);
    openLinks(arrLink);
}

function ValidateFileImport() {
    var Title = 'Lỗi lấy dữ liệu từ file excel';
    var Messages = [];

    var has_file = $("#inpFileUpload").val() != '';
    if (!has_file) {
        Messages.push("Vui lòng chọn file excel dữ liệu !");
    }
    else {
        var ext = $("#inpFileUpload").val().split(".");
        ext = ext[ext.length - 1].toLowerCase();
        var arrayExtensions = ["xls", "xlsx"];
        if (arrayExtensions.lastIndexOf(ext) == -1) {
            Messages.push("Chọn file không đúng định dạng !");
        }
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

function ConfirmRemoveRowImport(index) {
    var Title = 'Xác nhận xóa dòng dữ liệu thông tin dự án';
    var Messages = [];
    Messages.push('Bạn có chắc chắn muốn xóa?');
    var FunctionName = "RemoveRow('" + index + "')";
    $.ajax({
        type: "POST",
        url: "/Modal/OpenModal",
        data: { Title: Title, Messages: Messages, Category: CONFIRM, FunctionName: FunctionName },
        success: function (data) {
            $("#divModalConfirm").empty().html(data);
        }
    });
}

function RemoveRow(index) {
    $("#btn-delete" + index).closest("tr").remove();
}

function SaveImport() {
    if ($("#tblThongTinDuAnListImport tbody tr").length == 0) return;
    UpdateRow();

    var validateObj = ValidateDataImport();
    if (!validateObj.check) {
        if (validateObj.messages.length > 0) {
            var title = 'Lỗi nhập dữ liệu thông tin dự án';
            $.ajax({
                type: "POST",
                url: "/Modal/OpenModal",
                data: { Title: title, Messages: validateObj.messages, Category: ERROR },
                success: function (data) {
                    $("#divModalConfirm").html(data);
                }
            });
        }
        return;
    }

    if (validateObj.dataArray.length == 0) {
        var title = 'Lỗi import dữ liệu thông tin dự án';
        $.ajax({
            type: "POST",
            url: "/Modal/OpenModal",
            data: { Title: title, Messages: ["Không có dữ liệu để import"], Category: ERROR },
            success: function (data) {
                $("#divModalConfirm").html(data);
            }
        });
        return;
    }

    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTinDuAn/SaveImport",
        data: { contractList: validateObj.dataArray },
        success: function (r) {
            if (r && r.bIsComplete) {
                window.location.href = "/QLNH/ThongTinDuAn";
            } else {
                var Title = 'Lỗi import dữ liệu thông tin hợp đồng';
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

function ValidateDataImport() {
    var returnObj = {};
    var check = true;
    var messages = [];
    var data = {};
    var dataArray = [];
    $("#tblThongTinDuAnListImport tbody tr.correct").each(function () {
        if (!check) return;
        data = {};
        var index = $(this).data("index");
        if ($("#spanMaDuAn" + index).length > 0) {
            data.sMaDuAn = $("#spanMaDuAn" + index).html();
        } else {
            if ($.trim($("#txtMaDuAn" + index).val()).length > 100) {
                check = false;
                messages.push("Mã dự án vượt quá 100 kí tự!");
                document.getElementById("txtMaDuAn" + index).scrollIntoView();
                return;
            }
            data.sMaDuAn = $("<div/>").text($.trim($("#txtMaDuAn" + index).val())).html();
        }
        if ($("#spanTenDuAn" + index).length > 0) {
            data.sTenDuAn = $("#spanTenDuAn" + index).html();
        } else {
            if ($.trim($("#txtTenDuAn" + index).val()).length == 0) {
                check = false;
                messages.push("Tên dự án chưa được nhập !");
                document.getElementById("txtTenDuAn" + index).scrollIntoView();
                return;
            }

            if ($.trim($("#txtTenDuAn" + index).val()).length > 100) {
                check = false;
                messages.push("Tên dự án vượt quá 100 kí tự !");
                document.getElementById("txtTenDuAn" + index).scrollIntoView();
                return;
            }
            data.sTenDuAn = $("<div/>").text($.trim($("#txtTenDuAn" + index).val())).html();
        }

        if ($("#spanSoChuTruongDauTu" + index).length > 0) {
            data.sSoChuTruongDauTu = $("#spanSoChuTruongDauTu" + index).html();
        } else {
            if ($.trim($("#txtSoChuTruongDauTu" + index).val()).length == 0) {
                check = false;
                messages.push("Số chủ trương đầu tư chưa được nhập !");
                document.getElementById("txtSoChuTruongDauTu" + index).scrollIntoView();
                return;
            }

            if ($.trim($("#txtSoChuTruongDauTu" + index).val()).length > 100) {
                check = false;
                messages.push("Số chủ trương đầu tư vượt quá 100 kí tự !");
                document.getElementById("txtSoChuTruongDauTu" + index).scrollIntoView();
                return;
            }
            data.sSoChuTruongDauTu = $("<div/>").text($.trim($("#txtSoChuTruongDauTu" + index).val())).html();
        }

        if ($("#spanNgayChuTruongDauTu" + index).length > 0) {
            data.dNgayChuTruongDauTu = $("#spanNgayChuTruongDauTu" + index).html();
        } else {
            if ($.trim($("#txtNgayChuTruongDauTu" + index).val()) != "" && !dateIsValid($.trim($("#txtNgayChuTruongDauTu" + index).val()))) {
                check = false;
                messages.push("Ngày kí hợp đồng không hợp lệ !");
                document.getElementById("txtNgayChuTruongDauTu" + index).scrollIntoView();
                return;
            }
            data.dNgayChuTruongDauTu = $("<div/>").text($.trim($("#txtNgayChuTruongDauTu" + index).val())).html();
        }
        if ($("#spanSoQuetDinhDauTu" + index).length > 0) {
            data.sSoQuyetDinhDauTu = $("#spanSoQuetDinhDauTu" + index).html();
        } else {
            if ($.trim($("#txtSoQuetDinhDauTu" + index).val()) != "" && !dateIsValid($.trim($("#txtSoQuetDinhDauTu" + index).val()))) {
                check = false;
                messages.push("Ngày quyết định đầu tư không hợp lệ !");
                document.getElementById("txtSoQuetDinhDauTu" + index).scrollIntoView();
                return;
            }
            data.sSoQuyetDinhDauTu = $("<div/>").text($.trim($("#txtSoQuetDinhDauTu" + index).val())).html();
        }

        if ($("#spanNgayQuetDinhDauTu" + index).length > 0) {
            data.dNgayQuyetDinhDauTu = $("#spanNgayQuetDinhDauTu" + index).html();
        } else {
            if ($.trim($("#txtNgayQuetDinhDauTu" + index).val()) != "" && !dateIsValid($.trim($("#txtNgayQuetDinhDauTu" + index).val()))) {
                check = false;
                messages.push("Ngày quyết định đầu tư không hợp lệ !");
                document.getElementById("txtNgayQuetDinhDauTu" + index).scrollIntoView();
                return;
            }
            data.dNgayQuyetDinhDauTu = $("<div/>").text($.trim($("#txtNgayQuetDinhDauTu" + index).val())).html();
        }
        if ($("#spanSoDuToan" + index).length > 0) {
            data.sSoDuToan = $("#spanSoDuToan" + index).html();
        } else {
            if ($.trim($("#txtSoDuToanTu" + index).val()).length == 0) {
                check = false;
                messages.push("Số dự toán !");
                document.getElementById("txtSoDuToanTu" + index).scrollIntoView();
                return;
            }

            if ($.trim($("#txtSoDuToanTu" + index).val()).length > 100) {
                check = false;
                messages.push("Số dự toán vượt quá 100 kí tự !");
                document.getElementById("txtSoDuToanTu" + index).scrollIntoView();
                return;
            }
            data.sSoDuToan = $("<div/>").text($.trim($("#txtSoDuToanTu" + index).val())).html();
        }


        if ($("#spanNgayDuToan" + index).length > 0) {
            data.dNgayDuToan = $("#spanNgayDuToan" + index).html();
        } else {
            if ($.trim($("#txtNgayBanHanhDuToan" + index).val()) != "" && !dateIsValid($.trim($("#txtNgayBanHanhDuToan" + index).val()))) {
                check = false;
                messages.push("Ngày ban hành dự toán !");
                document.getElementById("txtNgayBanHanhDuToan" + index).scrollIntoView();
                return;
            }
            data.dNgayDuToan = $("<div/>").text($.trim($("#txtNgayBanHanhDuToan" + index).val())).html();
        }

        // lưu phân cấp phê duyệt và chủ đầu tư 
        if ($("#spanMaPhanCap" + index).length > 0) {
            data.iID_CapPheDuyetID = $("#spanMaPhanCap" + index).data("id");
        } else {
            data.iID_CapPheDuyetID = $("#slbPhanCapPheDuyet" + index).val() == GUID_EMPTY ? null : $("#slbPhanCapPheDuyet" + index).val();
        }

        if ($("#spanMaChuDauTu" + index).length > 0) {
            data.iID_ChuDauTuID = $("#spanMaChuDauTu" + index).data("id");
        } else {
            data.iID_ChuDauTuID = $("#slbChuDauTu" + index).val() == GUID_EMPTY ? null : $("#slbChuDauTu" + index).val();
        }

        if ($("#spanThoiGianThucHienTu" + index).length > 0) {
            data.sKhoiCong = $("#spanThoiGianThucHienTu" + index).html();
        } else {
            if ($.trim($("#txtThoiGianThucHienTu" + index).val()) != "" && !dateIsValid($.trim($("#txtThoiGianThucHienTu" + index).val()))) {
                check = false;
                messages.push("Thời gian thực hiện từ không hợp lệ !");
                document.getElementById("txtThoiGianThucHienTu" + index).scrollIntoView();
                return;
            }
            data.sKhoiCong = $("<div/>").text($.trim($("#txtThoiGianThucHienTu" + index).val())).html();
        }
                            
        if ($("#spanThoiGianThucHienDen" + index).length > 0) {
            data.sKetThuc = $("#spanThoiGianThucHienDen" + index).html();
        } else {
            if ($.trim($("#txtThoiGianThucHienDen" + index).val()) != "" && !dateIsValid($.trim($("#txtThoiGianThucHienDen" + index).val()))) {
                check = false;
                messages.push("Thời gian thực hiện đến không hợp lệ !");
                document.getElementById("txtThoiGianThucHienDen" + index).scrollIntoView();
                return;
            }
            data.sKetThuc = $("<div/>").text($.trim($("#txtThoiGianThucHienDen" + index).val())).html();
        }


        if ($(this).find("#slbBQuanLy" + index).val() == "" || $(this).find("#slbBQuanLy" + index).val() == GUID_EMPTY) {
            check = false;
            messages.push("Tên B Quản Lý chưa chọn !");
            document.getElementById("slbBQuanLy" + index).scrollIntoView();
            return;
        } else {
            data.iID_BQuanLyID = $(this).find("#slbBQuanLy" + index).val();
        }

        if ($(this).find("#slbDonVi" + index).val() == "" || $(this).find("#slbDonVi" + index).val() == GUID_EMPTY) {
            check = false;
            messages.push("Tên Đơn vị chưa chọn !");
            document.getElementById("slbDonVi" + index).scrollIntoView();
            return;
        } else {
            data.iID_DonViID = $(this).find("#slbDonVi" + index).val();
        }

        if ($(this).find("#slbChuongTrinh" + index).val() == "" || $(this).find("#slbChuongTrinh" + index).val() == GUID_EMPTY) {
            check = false;
            messages.push("Tên Chương trình chưa chọn !");
            document.getElementById("slbChuongTrinh" + index).scrollIntoView();
            return;
        } else {
            data.iID_KHCTBQP_ChuongTrinhID = $(this).find("#slbChuongTrinh" + index).val();
        }
        data.iLanDieuChinh = 0;

        dataArray.push(data);

    });

    returnObj.check = check;
    returnObj.messages = messages;
    returnObj.dataArray = dataArray;
    return returnObj;
}

function UpdateRow() {
    $("#tblThongTinDuAnListImport tbody tr.wrong").each(function () {
        var index = $(this).data("index");
        if (ValidateRowWrong(this, index)) {
            $(this).removeClass("wrong").addClass("correct");
            $(this).find(".status-icon").empty().html('<i class="fa fa-check fa-lg" style="color:green;" aria-hidden="true"></i>');
            $(this).find(".cellMessageError").empty();
        }
    });
}

function ValidateRowWrong(element, index) {
    var check = true;
    $(element).find("td.cellWrong").each(function () {
        if ($(this).find("#txtTenDuAn" + index).length == 1 && $.trim($(this).find("#txtTenDuAn" + index).val()).length > 100) {
            check = false;
            $(this).addClass("cellWrong");
            return;
        }
        if ($(this).find("#txtSoDuAn" + index).length == 1
            && ($.trim($(this).find("#txtSoDuAn" + index).val()) == "" || $.trim($(this).find("#txtSoDuAn" + index).val()).length > 100)) {
            check = false;
            $(this).addClass("cellWrong");
            return;
        }
        if ($(this).find("#txtThoiGianThucHienTu" + index).length == 1
            && $.trim($(this).find("#txtThoiGianThucHienTu" + index).val()) != ""
            && !dateIsValid($.trim($(this).find("#txtThoiGianThucHienTu" + index).val()))) {
            check = false;
            $(this).addClass("cellWrong");
            return;
        }
        if ($(this).find("#txtThoiGianThucHienDen" + index).length == 1
            && $.trim($(this).find("#txtThoiGianThucHienDen" + index).val()) != ""
            && !dateIsValid($.trim($(this).find("#txtThoiGianThucHienDen" + index).val()))) {
            check = false;
            $(this).addClass("cellWrong");
            return;
        }
        if (check) $(this).removeClass("cellWrong");
    });
    return check;
}
