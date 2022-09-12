var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var CONFIRM = 0;
var ERROR = 1;
function ResetChangePage(iCurrentPage = 1) {
    GetListData(GUID_EMPTY, 0);
}
function ChangePage() {
    var iNamKeHoach = $("#slbNamKeHoachFillter").val();
    var iDonVi = $("#iDonViFillter").val();

    var Title = 'Lỗi tìm kiếm báo cáo';
    var Messages = [];
    if (iDonVi == null || iDonVi == GUID_EMPTY) {
        Messages.push("Đơn vị  chưa chọn !");
    }

    if (iNamKeHoach == null || iNamKeHoach == 0) {
        Messages.push("Năm kế hoạch chưa chọn !");
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
    } else {
        GetListData(iDonVi, iNamKeHoach);
    }
}
function GetListData(iDonVi, iNamKeHoach) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: sUrlListView,
        data: { iDonVi: iDonVi, iNamKeHoach: iNamKeHoach },
        success: function (data) {
            $("#lstDataView").html(data);
            $("#iDonViFillter").val(iDonVi);
            $("#slbNamKeHoachFillter").val(iNamKeHoach);
        }
    });
}