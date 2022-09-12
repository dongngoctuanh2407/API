var TBL_CAP_THANH_TOAN_KPQP = "tbl_capthanhtoankpqp";
var TBL_TAM_UNG_KPQP = "tbl_tamungkpqp";
var TBL_THU_UNG_KPQP = "tbl_thuungkpqp";
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var iIDThongTriID = "";
var isClickLoc = false;

$(document).ready(function () {
    iIDThongTriID = $("#iIDThongTriID").val();
    LayThongTinThongTriCu(DinhDangSo);
});

function DinhDangSo() {
    $(".sotien").each(function () {
        $(this).html(FormatNumber($(this).html().trim()) == "" ? 0 : FormatNumber($(this).html().trim()));
    })
}

function LayThongTinThongTriCu(callback) {
    $.ajax({
        type: "POST",
        url: "/QLVonDauTu/QLThongTriThanhToan/LayThongTinChiTietThongTriChiTiet",
        data: { iID_ThongTriID: iIDThongTriID },
        success: function (r) {
            var button = { bUpdate: 0, bDelete: 0, bInfo: 0 };
            if (r != null) {
                var columns = [
                    { sField: "iID_ThongTriChiTietID", bKey: true },
                    { sField: "iID_Parent", bParentKey: true },

                    { sTitle: "Mục", sField: "sM", iWidth: "7%", sTextAlign: "center" },
                    { sTitle: "Tiểu mục", sField: "sTM", iWidth: "7%", sTextAlign: "center" },
                    { sTitle: "Tiết mục", sField: "sTTM", iWidth: "7%", sTextAlign: "center" },
                    { sTitle: "Ngành", sField: "sNG", iWidth: "7%", sTextAlign: "center" },
                    { sTitle: "Nội dung", sField: "sNoiDung", iWidth: "55%", sTextAlign: "left", bHaveIcon: 1 },
                    { sTitle: "Số tiền", sField: "fSoTien", iWidth: "11%", sTextAlign: "right", sClass: "sotien" }];
                if (r.lstTab1 != null) {
                    $("#" + TBL_CAP_THANH_TOAN_KPQP).html(GenerateTreeTable(r.lstTab1, columns, button, TBL_CAP_THANH_TOAN_KPQP));
                } else {
                    $("#" + TBL_CAP_THANH_TOAN_KPQP).html("");
                }

                var columnsNoTM = [
                    { sField: "iID_ThongTriChiTietID", bKey: true },
                    { sField: "iID_Parent", bParentKey: true },

                    { sTitle: "Nội dung", sField: "sNoiDung", iWidth: "55%", sTextAlign: "left", bHaveIcon: 1 },
                    { sTitle: "Số tiền", sField: "fSoTien", iWidth: "11%", sTextAlign: "right", sClass: "sotien" }];

                if (r.lstTab2 != null) {
                    $("#" + TBL_TAM_UNG_KPQP).html(GenerateTreeTable(r.lstTab2, columnsNoTM, button, TBL_TAM_UNG_KPQP));
                } else {
                    $("#" + TBL_TAM_UNG_KPQP).html("");
                }

                if (r.lstTab3 != null) {
                    $("#" + TBL_THU_UNG_KPQP).html(GenerateTreeTable(r.lstTab3, columnsNoTM, button, TBL_THU_UNG_KPQP));
                } else {
                    $("#" + TBL_THU_UNG_KPQP).html("");
                }

                if (callback)
                    callback();
            }
        }
    });
}

function Luu() {
    var thongTri = {};
    thongTri.iID_ThongTriID = iIDThongTriID;
    thongTri.sMaThongTri = $("#sMaThongTri").val();
    thongTri.sNguoiLap = $("#sNguoiLap").val();
    thongTri.sTruongPhong = $("#sTruongPhong").val();
    thongTri.sThuTruongDonVi = $("#sThuTruongDonVi").val();
    thongTri.dNgayLapGanNhat = $("#dNgayLapGanNhat").html();

    if (CheckLoi(thongTri)) {
        $.ajax({
            url: "/QLVonDauTu/QLThongTriThanhToan/Luu",
            type: "POST",
            data: { model: thongTri, bReloadChiTiet: isClickLoc },
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data != null && data == true) {
                    window.location.href = "/QLVonDauTu/QLThongTriThanhToan/Index";
                }
            },
            error: function (data) {

            }
        })
    }
}

function CheckLoi(doiTuong) {
    var messErr = [];
    if (doiTuong.sMaThongTri == "")
        messErr.push("Mã thông tri chưa có hoặc chưa chính xác.");

    if (doiTuong.sNguoiLap == "")
        messErr.push("Người lập thông tri chưa có hoặc chưa chính xác.");

    if (doiTuong.sThuTruongDonVi == "")
        messErr.push("Thủ trưởng đơn vị chưa có hoặc chưa chính xác.");

    if (KiemTraTrungMaThongTri(doiTuong.sMaThongTri) == true)
        messErr.push("Mã thông tri đã tồn tại, vui lòng nhập mã khác.");

    if ($("#" + TBL_CAP_THANH_TOAN_KPQP + " table tr").length == 0
        && $("#" + TBL_TAM_UNG_KPQP + " table tr").length == 0
        && $("#" + TBL_TAM_UNG_KPQP + " table tr").length == 0
    )
        messErr.push("Không có dữ liệu chi tiết.");

    if (messErr.length > 0) {
        alert(messErr.join("\n"));
        return false;
    } else {
        return true;
    }
}

function KiemTraTrungMaThongTri(sMaThongTri) {
    var check = false;
    $.ajax({
        url: "/QLVonDauTu/QLThongTriThanhToan/KiemTraTrungMaThongTri",
        type: "POST",
        data: { sMaThongTri: sMaThongTri, iID_ThongTriID: iIDThongTriID },
        dataType: "json",
        async: false,
        cache: false,
        success: function (data) {
            check = data;
        },
        error: function (data) {

        }
    })
    return check;
}

function Loc() {
    var iIDMaDonVi = $("#iID_DonViID").val();
    var iNguonVon = $("#iNguonVon").val();
    var dNgayLapGanNhat = $("#dNgayLapGanNhat").html();
    var dNgayTaoThongTri = $("#dNgayThongTri").html();
    var iNamThongTri = $("#iNamThongTri").html();
    isClickLoc = true;
    LayThongTinDeNghiThanhToan(iIDMaDonVi, iNguonVon, dNgayLapGanNhat, dNgayTaoThongTri, iNamThongTri, DinhDangSo);
}

function LayThongTinDeNghiThanhToan(iIDMaDonVi, iNguonVon, dNgayLapGanNhat, dNgayTaoThongTri, iNamThongTri, callback) {
    $.ajax({
        type: "POST",
        url: "/QLVonDauTu/QLThongTriThanhToan/GetDeNghiThanhToanChiTiet",
        data: { iID_MaDonVi: iIDMaDonVi, iNguonVon: iNguonVon, dNgayLapGanNhat: dNgayLapGanNhat, dNgayTaoThongTri: dNgayTaoThongTri, iNamThongTri: iNamThongTri },
        success: function (data) {
            var button = { bUpdate: 0, bDelete: 0, bInfo: 0 };
            if (data != null && data.data != null) {
                var columnsTab1 = [
                    { sField: "iID_DeNghiThanhToan_ChiTietID", bKey: true },
                    { sField: "iID_Parent", bParentKey: true },

                    { sTitle: "Mục", sField: "sM", iWidth: "7%", sTextAlign: "center" },
                    { sTitle: "Tiểu mục", sField: "sTM", iWidth: "7%", sTextAlign: "center" },
                    { sTitle: "Tiết mục", sField: "sTTM", iWidth: "7%", sTextAlign: "center" },
                    { sTitle: "Ngành", sField: "sNG", iWidth: "7%", sTextAlign: "center" },
                    { sTitle: "Nội dung", sField: "sNoiDung", iWidth: "55%", sTextAlign: "left", bHaveIcon: 1 },
                    { sTitle: "Số tiền", sField: "fGiaTriThanhToan", iWidth: "11%", sTextAlign: "right", sClass: "sotien" }];
                $("#" + TBL_CAP_THANH_TOAN_KPQP).html(GenerateTreeTable(data.data, columnsTab1, button, TBL_CAP_THANH_TOAN_KPQP));

                var columnsTab2 = [
                    { sField: "iID_DeNghiThanhToan_ChiTietID", bKey: true },
                    { sField: "iID_Parent", bParentKey: true },

                    { sTitle: "Nội dung", sField: "sNoiDung", iWidth: "55%", sTextAlign: "left", bHaveIcon: 1 },
                    { sTitle: "Số tiền", sField: "fGiaTriTamUng", iWidth: "11%", sTextAlign: "right", sClass: "sotien" }];
                $("#" + TBL_TAM_UNG_KPQP).html(GenerateTreeTable(data.data, columnsTab2, button, TBL_TAM_UNG_KPQP));

                var columnsTab3 = [
                    { sField: "iID_DeNghiThanhToan_ChiTietID", bKey: true },
                    { sField: "iID_Parent", bParentKey: true },

                    { sTitle: "Nội dung", sField: "sNoiDung", iWidth: "55%", sTextAlign: "left", bHaveIcon: 1 },
                    { sTitle: "Số tiền", sField: "fGiaTriThuHoi", iWidth: "11%", sTextAlign: "right", sClass: "sotien" }];
                $("#" + TBL_THU_UNG_KPQP).html(GenerateTreeTable(data.data, columnsTab3, button, TBL_THU_UNG_KPQP));
            } else {
                $("#" + TBL_CAP_THANH_TOAN_KPQP).html("");
                $("#" + TBL_TAM_UNG_KPQP).html("");
                $("#" + TBL_THU_UNG_KPQP).html("");
            }

            if (callback)
                callback();
        }
    });
}

function DinhDangSo() {
    $(".sotien").each(function () {
        $(this).html(FormatNumber($(this).html().trim()) == "" ? 0 : FormatNumber($(this).html().trim()));
    })
}

function XoaTextThongTri() {
    $("#sMaThongTri").val("");
    $("#sNguoiLap").val("");
    $("#sTruongPhong").val("");
    $("#sThuTruongDonVi").val("");
}

function Huy() {
    window.location.href = "/QLVonDauTu/QLThongTriThanhToan/Index";
}
