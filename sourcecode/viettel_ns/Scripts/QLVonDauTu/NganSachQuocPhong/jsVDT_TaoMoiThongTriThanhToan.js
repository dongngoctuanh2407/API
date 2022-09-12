var TBL_CAP_THANH_TOAN_KPQP = "tbl_capthanhtoankpqp";
var TBL_TAM_UNG_KPQP = "tbl_tamungkpqp";
var TBL_THU_UNG_KPQP = "tbl_thuungkpqp";
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';

$(document).ready(function () {

});

$("#iID_DonViQuanLyID, #sMaNguonVon, #sNamThongTri").change(function () {
    LayNgayLapGanNhat();
})

function LayNgayLapGanNhat() {
    var sMaDonVi = $("#iID_DonViQuanLyID").val();
    var iNguonVon = $("#sMaNguonVon").val();
    var iNamThongTri = $("#sNamThongTri").val();
    if (sMaDonVi != "" && sMaDonVi != GUID_EMPTY && iNguonVon != "" && iNguonVon != 0 && iNamThongTri != "") {
        $.ajax({
            url: "/QLVonDauTu/QLThongTriThanhToan/LayNgayLapGanNhat",
            type: "POST",
            data: { iIDDonViID: sMaDonVi, iNamThongTri: iNamThongTri, iNguonVon: iNguonVon },
            dataType: "json",
            cache: false,
            success: function (data) {
                $("#sNgayLapGanNhat").val(data);
            },
            error: function (data) {

            }
        })
    } else {
        $("#sNgayLapGanNhat").val("");
    }
}

function Loc() {
    var iIDMaDonVi = $("#iID_DonViQuanLyID").val();
    var iNguonVon = $("#sMaNguonVon").val();
    var dNgayLapGanNhat = $("#sNgayLapGanNhat").val();
    var dNgayTaoThongTri = $("#dNgayThongTri").val();
    var iNamThongTri = $("#sNamThongTri").val();

    if (iIDMaDonVi == "" || iIDMaDonVi == GUID_EMPTY) {
        alert("Thông tin đơn vị chưa có hoặc chưa chính xác");
        return;
    }

    if (iNamThongTri == "") {
        alert("Chưa nhập năm thực hiện");
        return;
    }

    if (iIDMaDonVi != "" && iIDMaDonVi != GUID_EMPTY && iNguonVon != "" && iNguonVon != 0 && dNgayTaoThongTri != "" && iNamThongTri != "")
        LayThongTinDeNghiThanhToan(iIDMaDonVi, iNguonVon, dNgayLapGanNhat, dNgayTaoThongTri, iNamThongTri, DinhDangSo);
    else
        alert("Chưa nhập thông tin");
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
    $("#iID_DonViQuanLyID").prop("selectedIndex", 0).trigger("change");
    $("#sMaThongTri").val("");
    $("#sNgayLapGanNhat").val("");
    $("#dNgayThongTri").val("");
    $("#sNamThongTri").val("");
    $("#sNguoiLap").val("");
    $("#sTruongPhong").val("");
    $("#sThuTruongDonVi").val("");
}

function Huy() {
    window.location.href = "/QLVonDauTu/QLThongTriThanhToan/Index";
}

function Luu() {
    var thongTri = {};
    thongTri.iID_DonViID = $("#iID_DonViQuanLyID").val();
    thongTri.bThanhToan = true;
    thongTri.sMaNguonVon = $("#sMaNguonVon").val();
    thongTri.sMaThongTri = $("#sMaThongTri").val();
    thongTri.dNgayThongTri = $("#dNgayThongTri").val();
    thongTri.dNgayLapGanNhat = $("#sNgayLapGanNhat").val();
    thongTri.iNamThongTri = $("#sNamThongTri").val();
    thongTri.sNguoiLap = $("#sNguoiLap").val();
    thongTri.sTruongPhong = $("#sTruongPhong").val();
    thongTri.sThuTruongDonVi = $("#sThuTruongDonVi").val();

    if (CheckLoi(thongTri)) {
        //var doiTuong = { thongTri: thongTri };
        $.ajax({
            url: "/QLVonDauTu/QLThongTriThanhToan/Luu",
            type: "POST",
            data: { model: thongTri },
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
    if (doiTuong.iID_DonViID == "" || doiTuong.iID_DonViID == GUID_EMPTY)
        messErr.push("Đơn vị quản lý chưa có hoặc chưa chính xác.");

    if (doiTuong.sMaNguonVon == "" || doiTuong.sMaNguonVon == 0)
        messErr.push("Nguồn vốn chưa có hoặc chưa chính xác.");

    if (doiTuong.sMaThongTri == "")
        messErr.push("Mã thông tri chưa có hoặc chưa chính xác.");

    if (doiTuong.dNgayThongTri == "")
        messErr.push("Ngày tạo thông tri chưa có hoặc chưa chính xác.");

    if (doiTuong.iNamThongTri == "")
        messErr.push("Năm thực hiện chưa có hoặc chưa chính xác.");

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
        data: { sMaThongTri: sMaThongTri },
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
