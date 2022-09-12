//============================== Event List ================================//
function GetItemDataList(id) {
    window.location.href = "/QLVonDauTu/QLKeHoachChiQuy/Update/" + id;
}

function ViewDetailList(id) {
    window.location.href = "/QLVonDauTu/QLKeHoachChiQuy/Detail/" + id;
}

function GetListData(sSoDeNghi, iNamKeHoach, dNgayDeNghiFrom, dNgayDeNghiTo, iIDMaDonViQuanLy, iIDNguonVon, iQuy, iCurrentPage) {
    _paging.CurrentPage = iCurrentPage;
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLVonDauTu/QLKeHoachChiQuy/KHNhuCauChiQuyView",
        data: {
            sSoDeNghi: sSoDeNghi, iNamKeHoach: iNamKeHoach, dNgayDeNghiFrom: dNgayDeNghiFrom, dNgayDeNghiTo: dNgayDeNghiTo,
            iIDMaDonViQuanLy: iIDMaDonViQuanLy, iIDNguonVon: iIDNguonVon, iQuy: iQuy, _paging: _paging
        },
        success: function (data) {
            $("#lstDataView").html(data);
            $("#txtSoDeNghi").val(sSoDeNghi);
            $("#txtNamKeHoach").val(iNamKeHoach);
            $("#txtNgayDeNghiFrom").val(dNgayDeNghiFrom);
            $("#txtNgayDeNghiTo").val(dNgayDeNghiTo);
            $("#drpDonViQuanLy").val(iIDMaDonViQuanLy);
            $("#drpNguonNganSach").val(iIDNguonVon);
            $("#drpQuy").val(iQuy);
        }
    });
}

function ChangePage(iCurrentPage = 1) {
    var sSoDeNghi = $("#txtSoDeNghi").val();
    var iNamKeHoach = $("#txtNamKeHoach").val();
    var dNgayDeNghiFrom = $("#txtNgayDeNghiFrom").val();
    var dNgayDeNghiTo = $("#txtNgayDeNghiTo").val();
    var iIDMaDonViQuanLy = $("#drpDonViQuanLy option:selected").val();
    var iIDNguonVon = $("#drpNguonNganSach option:selected").val();
    var iQuy = $("#drpQuy option:selected").val();
    GetListData(sSoDeNghi, iNamKeHoach, dNgayDeNghiFrom, dNgayDeNghiTo, iIDMaDonViQuanLy, iIDNguonVon, iQuy, iCurrentPage);
}

function DeleteItemList(id) {
    if (!confirm("Chấp nhận xóa bản ghi ?")) return;
    $.ajax({
        type: "POST",
        url: "/QLVonDauTu/QLKeHoachChiQuy/XoaKeHoachChiQuy",
        data: { id: id },
        success: function (r) {
            if (r == "True") {
                ChangePage();
            }
        }
    });
}

function BtnInsertDataClick() {
    location.href = "/QLVonDauTu/QLKeHoachChiQuy/Insert";
}

function XuatFile(id) {
    location.href = "/QLVonDauTu/QLKeHoachChiQuy/XuatFile?id=" + id;
}