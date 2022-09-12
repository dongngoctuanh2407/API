var CONFIRM = 0;
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;

$(document).ready(function () {
    ChooseTable(true);
});

function ResetChangePage(iCurrentPage = 1) {
    GetListData(GUID_EMPTY, GUID_EMPTY, GUID_EMPTY, iCurrentPage);
}
function ChangePage(iCurrentPage = 1) {
    var sDonVi = $("#slbDonViFilter").val();
    var sDuAn = $("#slbDuAnFilter").val();
    var sHopDong = $("#slbDopDongFilter").val();
    GetListData(sDonVi, sDuAn, sHopDong, iCurrentPage);
}

function ChooseTable(val) {
    if (val) {
        $("#tbListBaoCaoTaiSan1").show();
        $("#tbListBaoCaoTaiSan2").hide();
    } else {
        $("#tbListBaoCaoTaiSan1").hide();
        $("#tbListBaoCaoTaiSan2").show();
    }
}

function GetListData(sDonVi, sDuAn, sHopDong, iCurrentPage, changeTable) {
    let state = CheckFirstTableIsShow();

    _paging.CurrentPage = iCurrentPage;
    $.ajax({
        type: "POST",
        dataType: "html",
        url: sUrlListView,
        data: { _paging: _paging, iID_DonViID: sDonVi, iID_DuAnID: sDuAn, iID_HopDongID: sHopDong, changeTable: changeTable},
        success: function (data) {
            $("#lstDataView").html(data);
            $("#slbDonViFilter").val(sDonVi);
            $("#slbDuAnFilter").val(sDuAn);
            $("#slbDopDongFilter").val(sHopDong);

            ChooseTable(state);
        }
    });
}

function CheckFirstTableIsShow() {
    if ($("#tbListBaoCaoTaiSan2").is(":hidden")) {
        return true;
    }
    return false;
}