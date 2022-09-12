$(".currently").trigger("change");

//************** Index **************//
function ChangePage(iCurrentPage = 1) {
    var drpDonVi = $("#drpDonViQuanLy option:selected").val();
    var iIdNguonVon = $("#drpNguonNganSach option:selected").val();
    var dNgayDeNghiFrom = $("#txtNgayLapFrom").val();
    var dNgayDeNghiTo = $("#txtNgayLapTo").val();
    var iNamKeHoach = $("#txtNamKeHoach").val();
    var sMaDonVi = "";
    var iIdDonVi = ""

    if (drpDonVi != undefined && drpDonVi != null && drpDonVi != "") {
        iIdDonVi = drpDonVi.split("|")[0];
        sMaDonVi = drpDonVi.split("|")[1];
    }

    GetListData(iIdDonViQuanLy, iIdMaDonViQuanLy, iIdNguonVon, dNgayDeNghiFrom, dNgayDeNghiTo, iNamKeHoach, iCurrentPage);
}

function GetListData(iIdDonViQuanLy, iIdMaDonViQuanLy, iIdNguonVon, dNgayDeNghiFrom, dNgayDeNghiTo, iNamKeHoach, iCurrentPage) {
    _paging.CurrentPage = iCurrentPage;
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLVonDauTu/BcQuyetToanNienDo/QuyetToanNienDoView",
        data: {
            iIdMaDonViQuanLy: iIdMaDonViQuanLy,
            iIdNguonVon: iIdNguonVon,
            dNgayDeNghiFrom: dNgayDeNghiFrom,
            dNgayDeNghiTo: dNgayDeNghiTo,
            iNamKeHoach: iNamKeHoach,
            _paging: _paging
        },
        success: function (data) {
            $("#lstDataView").html(data);
            $("#drpDonViQuanLy").val(iIdDonViQuanLy + "|" + iIdMaDonViQuanLy);
            $("#drpNguonVon").val(iIdNguonVon);
            $("#txtNgayLapFrom").val(dNgayDeNghiFrom);
            $("#txtNgayLapTo").val(dNgayDeNghiTo);
            $("#txtNamKeHoach").val(iNamKeHoach);
        }
    });
}

function BtnInsertDataClick() {
    location.href = "/QLVonDauTu/BcQuyetToanNienDo/Update";
}

function GetItemDataList(id) {
    location.href = "/QLVonDauTu/BcQuyetToanNienDo/Update/" + id;
}

function DeleteItemList(iId) {
    if (confirm("Bạn có chắc chắn muốn xóa quyết toán này ?")) {
        $.ajax({
            url: "/QLVonDauTu/BcQuyetToanNienDo/DeleteBCQuyetToanNienDo",
            type: "GET",
            data: { iId: iId },
            success: function (data) {
                alert("Xóa thành công !");
                ChangePage();
            }
        });
    }
}

//************** Update **************//
function ChangeVoucher() {
    if ($("input[name=groupVoucher]:checked").val() == "1")
    {
        $("#ViewTable").css("display", "");
        $("#ViewTablePhanTich").css("display", "none");
    } else
    {
        $("#ViewTable").css("display", "none");
        $("#ViewTablePhanTich").css("display", "");
    }
}

function ChangeLoaiThanhToan() {
    if ($("#drpNguonVon").val() != 1 || $("#drpLoaiThanhToan").val() != 1) {
        $("#grp_loaichungtu").css("display", "none");
    } else {
        $("#grp_loaichungtu").css("display", "");
    }
    $("#dxChungTu").prop("checked", true);
    ChangeVoucher();
    RenderGridView();
}

function RenderGridView() {
    var drpDonViQuanLy = $("#drpDonViQuanLy").val();
    var sMaDonViQuanLy = "";
    if (drpDonViQuanLy != null && drpDonViQuanLy != "") {
        sMaDonViQuanLy = drpDonViQuanLy.split("|")[1]
    }
    var iNamKeHoach = $("#txtNamKeHoach").val();
    var iIDNguonVonID = $("#drpNguonVon").val();
    var iCoQuanTaiChinh = $("#drpCoQuanThanhToan").val();
    var iIDBcQuyetToan = $("#iIDBcQuyetToan").val();
    var drpLoaiThanhToan = $("#drpLoaiThanhToan").val();
    if (drpLoaiThanhToan == null) return;
    RenderQuyetToanNienDoTongHop(drpLoaiThanhToan, iIDBcQuyetToan, sMaDonViQuanLy, iNamKeHoach, iIDNguonVonID, iCoQuanTaiChinh);
    RenderQuyetToanNienDoPhanTich(drpLoaiThanhToan, iIDBcQuyetToan, sMaDonViQuanLy, iNamKeHoach, iIDNguonVonID, iCoQuanTaiChinh);
}

function RenderQuyetToanNienDoTongHop(drpLoaiThanhToan, iIDBcQuyetToan, sMaDonViQuanLy, iNamKeHoach, iIDNguonVonID, iCoQuanTaiChinh) {
    var sUrl = "";
    if (drpLoaiThanhToan == 1)
        sUrl = "QuyetToanNienDoKHVN"
    else
        sUrl = "QuyetToanNienDoKHU";
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLVonDauTu/BcQuyetToanNienDo/" + sUrl,
        data: {
            iIDBcQuyetToan: iIDBcQuyetToan,
            sMaDonVi: sMaDonViQuanLy,
            iNamKeHoach: iNamKeHoach,
            iIDNguonVonID: iIDNguonVonID,
            iCoQuanTaiChinh: iCoQuanTaiChinh,
        },
        success: function (data) {
            $("#ViewTable").html(data);
        }
    });
}

function RenderQuyetToanNienDoPhanTich(drpLoaiThanhToan, iIDBcQuyetToan, sMaDonViQuanLy, iNamKeHoach, iIDNguonVonID, iCoQuanTaiChinh) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLVonDauTu/BcQuyetToanNienDo/QuyetToanNienDoKHVN_PhanTich",
        data: {
            iIDBcQuyetToan: iIDBcQuyetToan,
            sMaDonVi: sMaDonViQuanLy,
            iNamKeHoach: iNamKeHoach,
            iIDNguonVonID: iIDNguonVonID,
            iCoQuanTaiChinh: iCoQuanTaiChinh,
        },
        success: function (data) {
            $("#ViewTablePhanTich").html(data);
        }
    });
}

function ValidateForm() {
    var lstErrors = [];
    if ($("#drpDonViQuanLy").val() == undefined || $("#drpDonViQuanLy").val() == null) {
        lstErrors.push("Đơn vị quản lý chưa được nhập !");
    }
    if ($("#txtNamKeHoach").val() == undefined || $("#txtNamKeHoach").val() == null) {
        lstErrors.push("Năm kế hoạch chưa được nhập !");
    }
    if ($("#txtSoPheDuyet").val() == undefined || $("#txtSoPheDuyet").val() == null || $("#txtSoPheDuyet").val() == "") {
        lstErrors.push("Số quyết định chưa được nhập !");
    }
    if ($("#txtNgayPheDuyet").val() == undefined || $("#txtNgayPheDuyet").val() == null || $("#txtNgayPheDuyet").val() == "") {
        lstErrors.push("Ngày đề nghị chưa được nhập !");
    }
    if ($("#drpNguonVon").val() == undefined || $("#drpNguonVon").val() == null) {
        lstErrors.push("Nguồn vốn chưa được nhập !");
    }
    if ($("#drpLoaiThanhToan").val() == undefined || $("#drpLoaiThanhToan").val() == null) {
        lstErrors.push("Loại thanh toán chưa được nhập !");
    }
    if ($("#drpCoQuanThanhToan").val() == undefined || $("#drpCoQuanThanhToan").val() == null) {
        lstErrors.push("Cơ quan thanh toán chưa được nhập !");
    }
    if (lstErrors.length != 0) {
        alert(lstErrors.join("\n"));
        return false;
    }
    return true;
}

function Insert() {
    var data = {};
    var lstData = [];
    if (!ValidateForm()) return;

    data.sSoDeNghi = $("#txtSoPheDuyet").val();
    data.iCoQuanThanhToan = $("#drpCoQuanThanhToan").val();
    data.dNgayDeNghi = $("#txtNgayPheDuyet").val();
    data.iNamKeHoach = $("#txtNamKeHoach").val();
    data.iID_NguonVonID = $("#drpNguonVon").val();
    data.iID_MaDonViQuanLy = $("#drpDonViQuanLy").val().split("|")[1];
    data.iID_DonViQuanLyID = $("#drpDonViQuanLy").val().split("|")[0]
    data.iLoaiThanhToan = $("#drpLoaiThanhToan").val();

    $.ajax({
        type: "POST",
        url: "/QLVonDauTu/BcQuyetToanNienDo/UpdateBcQuyetToanNienDo",
        data: {
            data: data,
        },
        success: function (r) {
            if (r.bIsComplete) {
                alert("Thêm mới thành công !")
                location.href = "/QLVonDauTu/BcQuyetToanNienDo";
            } else {
                alert("Có lỗi xảy ra trong quá trình thêm mới !");
            }
        }
    });
}

function CancelSaveData(){
    location.href = "/QLVonDauTu/BcQuyetToanNienDo";
}

function CheckIsUpdate() {
    var iIDBcQuyetToan = $("#iIDBcQuyetToan").val();
    if (iIDBcQuyetToan == null || iIDBcQuyetToan == "") {
        $("#drpDonViQuanLy").attr("disabled", "disabled");
        $("#txtNamKeHoach").attr("disabled", "disabled");
        $("#txtNgayPheDuyet").attr("disabled", "disabled");
        $("#drpNguonVon").attr("disabled", "disabled");
        $("#drpLoaiThanhToan").attr("disabled", "disabled");
        $("#drpCoQuanThanhToan").attr("disabled", "disabled");
    } else {
        $("#drpDonViQuanLy").removeAttr("disabled");
        $("#txtNamKeHoach").removeAttr("disabled");
        $("#txtNgayPheDuyet").removeAttr("disabled");
        $("#drpNguonVon").removeAttr("disabled");
        $("#drpLoaiThanhToan").removeAttr("disabled");
        $("#drpCoQuanThanhToan").removeAttr("disabled");
    }
}

//------- Event KHVN
function fnChangeGiaTriTamUngDieuChinhGiam(e) {
    var fGiaTriTamUngDieuChinhGiam = $(e).val();
    if (fGiaTriTamUngDieuChinhGiam == null || !$.isNumeric(fGiaTriTamUngDieuChinhGiam)) {
        fGiaTriTamUngDieuChinhGiam = 0
    }
    var fTamUngTheoCheDoChuaThuHoiNamTruoc = $(e).closest("tr").find(".fTamUngTheoCheDoChuaThuHoiNamTruoc").text();
    var fTamUngNamTruocThuHoiNamNay = $(e).closest("tr").find(".fTamUngNamTruocThuHoiNamNay").text();
    var fTamUngTheoCheDoChuaThuHoiKeoDaiNamNay = $(e).closest("tr").find(".fTamUngTheoCheDoChuaThuHoiKeoDaiNamNay").text();
    var fTamUngTheoCheDoChuaThuHoiNamNay = $(e).closest("tr").find(".fTamUngTheoCheDoChuaThuHoiNamNay").text();
    $(e).val(Number(fGiaTriTamUngDieuChinhGiam).toLocaleString('vi-VN'));
    $(e).closest("tr").find(".fLuyKeTamUngChuaThuHoiChuyenSangNam").text(FormatNumber(
        parseFloat(UnFormatNumber(fTamUngTheoCheDoChuaThuHoiNamTruoc)) - parseFloat(UnFormatNumber(fGiaTriTamUngDieuChinhGiam)) - parseFloat(UnFormatNumber(fTamUngNamTruocThuHoiNamNay))
        + parseFloat(UnFormatNumber(fTamUngTheoCheDoChuaThuHoiKeoDaiNamNay)) + parseFloat(UnFormatNumber(fTamUngTheoCheDoChuaThuHoiNamNay))
    ));
}

function fnChangeGiaTriNamTruocChuyenNamSau(e) {
    var fGiaTriNamTruocChuyenNamSau = $(e).val();
    if (fGiaTriNamTruocChuyenNamSau == null || !$.isNumeric(fGiaTriNamTruocChuyenNamSau)) {
        fGiaTriNamTruocChuyenNamSau = 0
    }
    var fKHVNamTruocChuyenNamNay = $(e).closest("tr").find(".fKHVNamTruocChuyenNamNay").text();
    var fTongThanhToanVonKeoDaiNamNay = $(e).closest("tr").find(".fTongThanhToanVonKeoDaiNamNay").text();
    $(e).val(Number(fGiaTriNamTruocChuyenNamSau).toLocaleString('vi-VN'));
    $(e).closest("tr").find(".fVonConLaiHuyBoKeoDaiNamNay").text(FormatNumber(
        UnFormatNumber(fKHVNamTruocChuyenNamNay) - UnFormatNumber(fTongThanhToanVonKeoDaiNamNay) - UnFormatNumber(fGiaTriNamTruocChuyenNamSau)
    ));
}

function fnChangeGiaTriNamNayChuyenNamSau(e) {
    var fGiaTriNamNayChuyenNamSau = $(e).val();
    if (fGiaTriNamNayChuyenNamSau == null || !$.isNumeric(fGiaTriNamNayChuyenNamSau)) {
        fGiaTriNamNayChuyenNamSau = 0
    }
    var fKHVNamNay = $(e).closest("tr").find(".fKHVNamNay").text();
    var fTongKeHoachThanhToanVonNamNay = $(e).closest("tr").find(".fTongKeHoachThanhToanVonNamNay").text();
    $(e).val(Number(fGiaTriNamNayChuyenNamSau).toLocaleString('vi-VN'))
    $(e).closest("tr").find(".fVonConLaiHuyBoNamNay").text(FormatNumber(
        UnFormatNumber(fKHVNamNay) - UnFormatNumber(fTongKeHoachThanhToanVonNamNay) - UnFormatNumber(fGiaTriNamNayChuyenNamSau)
    ));
}

// Event KHVU
function onChangeGiaTriThuHoiTheoGiaiNganThucTe(e) {
    var fGiaTriThuHoiTheoGiaiNganThucTe = $(e).val();
    if (fGiaTriThuHoiTheoGiaiNganThucTe == null || !$.isNumeric(fGiaTriThuHoiTheoGiaiNganThucTe)) {
        fGiaTriThuHoiTheoGiaiNganThucTe = 0
    }
    $(e).val(Number(fGiaTriThuHoiTheoGiaiNganThucTe).toLocaleString('vi-VN'))
}

// Event Phan tich

function fnChangeDnQuyetToanNamTrc(e) {
    var FDnQuyetToanNamTrc = $(e).val();
    if (FDnQuyetToanNamTrc == null || !$.isNumeric(FDnQuyetToanNamTrc)) {
        FDnQuyetToanNamTrc = 0
    }
    var FDnQuyetToanNamNay = $(e).closest("tr").find(".FDnQuyetToanNamNay").text();

    $(e).closest("tr").find(".FSumSoDeNghiQuyetToan").text(FormatNumber(
        UnFormatNumber(FDnQuyetToanNamTrc) + UnFormatNumber(FDnQuyetToanNamNay)
    ));

    $(e).val(Number(FDnQuyetToanNamTrc).toLocaleString('vi-VN'))
}

function fnChangeDnQuyetToanNamNay(e) {
    var FDnQuyetToanNamNay = $(e).val();
    if (FDnQuyetToanNamNay == null || !$.isNumeric(FDnQuyetToanNamNay)) {
        FDnQuyetToanNamNay = 0
    }
    var FDnQuyetToanNamTrc = $(e).closest("tr").find(".FDnQuyetToanNamTrc").text();

    $(e).closest("tr").find(".FSumSoDeNghiQuyetToan").text(FormatNumber(
        UnFormatNumber(FDnQuyetToanNamTrc) + UnFormatNumber(FDnQuyetToanNamNay)
    ));

    $(e).val(Number(FDnQuyetToanNamNay).toLocaleString('vi-VN'))
}

function fnChangeDuToanThuHoi(e) {
    var FDuToanThuHoi = $(e).val();
    if (FDuToanThuHoi == null || !$.isNumeric(FDuToanThuHoi)) {
        FDuToanThuHoi = 0
    }
    $(e).val(Number(FDuToanThuHoi).toLocaleString('vi-VN'))
}