    var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var CONFIRM = 0;
var ERROR = 1;
function ResetChangePage(iCurrentPage = 1) {
    GetListData( 0);
}
function ChangePage() {
    var iNamKeHoach = $("#slbNamKeHoachFillter").val();
    GetListData( iNamKeHoach);
}
function GetListData(iNamKeHoach) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: sUrlListView,
        data: {  iNamKeHoach: iNamKeHoach },
        success: function (data) {
            $("#lstDataView").html(data);
            $("#iDonViFillter").val(iDonVi);
            $("#slbNamKeHoachFillter").val(iNamKeHoach);
        }
    });
}