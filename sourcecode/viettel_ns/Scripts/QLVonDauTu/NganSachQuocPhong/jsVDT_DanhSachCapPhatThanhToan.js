//============================== Event List ================================//
function GetItemDataList(id) {
    window.location.href = "/QLVonDauTu/GiaiNganThanhToan/Update/" + id;
}

function ViewDetailList(id) {
    window.location.href = "/QLVonDauTu/GiaiNganThanhToan/Detail/" + id;
}

function GetListData(sSoDeNghi, iNamKeHoach, dNgayDeNghiFrom, dNgayDeNghiTo, sDonViQuanLy, iCurrentPage) {
    _paging.CurrentPage = iCurrentPage;
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLVonDauTu/GiaiNganThanhToan/GiaiNganThanhToanView",
        data: { sSoDeNghi: sSoDeNghi, iNamKeHoach: iNamKeHoach, dNgayDeNghiFrom: dNgayDeNghiFrom, dNgayDeNghiTo: dNgayDeNghiTo, sDonViQuanLy: sDonViQuanLy, _paging: _paging },
        success: function (data) {
            $("#lstDataView").html(data);
            $("#txtSoDeNghi").val(sSoDeNghi);
            $("#txtNamKeHoach").val(iNamKeHoach);
            $("#txtNgayDeNghiFrom").val(dNgayDeNghiFrom);
            $("#txtNgayDeNghiTo").val(dNgayDeNghiTo);
            $("#drpDonViQuanLy").val(sDonViQuanLy);
        }
    });
}

function ChangePage(iCurrentPage = 1) {
    var sSoDeNghi = $("#txtSoDeNghi").val();
    var iNamKeHoach = $("#txtNamKeHoach").val();
    var dNgayDeNghiFrom = $("#txtNgayDeNghiFrom").val();
    var dNgayDeNghiTo = $("#txtNgayDeNghiTo").val();
    var sDonViQuanLy = $("#drpDonViQuanLy option:selected").val();
    GetListData(sSoDeNghi, iNamKeHoach, dNgayDeNghiFrom, dNgayDeNghiTo, sDonViQuanLy, iCurrentPage);
}

function DeleteItemList(id) {
    if (!confirm("Chấp nhận xóa bản ghi ?")) return;
    $.ajax({
        type: "POST",
        url: "/QLVonDauTu/GiaiNganThanhToan/DeleteDeNghiThanhToan",
        data: { id: id },
        success: function (r) {
            if (r == "True") {
                ChangePage();
            }
        }
    });
}

function BtnInsertDataClick() {
    location.href = "/QLVonDauTu/GiaiNganThanhToan/Insert";
}

var iIdDeNghiThanhToanId = "";
function XuatFile(id) {
    iIdDeNghiThanhToanId = id;
    $('#configBaocao').modal('show');
}

$(".btn-print").click(function () {
    var links = [];
    var ext = $(this).data("ext");
    var typeBC = $('input[name=loaiBC]:checked').val();
    var dvt = $("#dvt").val();
    var url = $("#urlExport").val() +
        "?ext=" + ext +
        "&dvt=" + dvt +
        "&type=" + typeBC+
        "&id=" + iIdDeNghiThanhToanId;

    url = unescape(url);
    links.push(url);
    openLinks(links);
});