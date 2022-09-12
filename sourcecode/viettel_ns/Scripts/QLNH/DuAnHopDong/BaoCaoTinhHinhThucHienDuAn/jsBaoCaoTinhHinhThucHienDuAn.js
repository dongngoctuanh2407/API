var CONFIRM = 0;
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;

function ResetChangePage(iCurrentPage = 1) {
    var iID_DuAnID = GUID_EMPTY;
    var dBatDau = null;
    var dKetThuc = null;
    GetListData(iID_DuAnID, dBatDau, dKetThuc, iCurrentPage, true);
}

function ChangePage(iCurrentPage = 1) {
    var iID_DuAnID = $("#txtiID_DuAnID").val();
    var dBatDau = $("#txtdBatDau").val();
    var dKetThuc = $("#txtdKetThuc").val();
    GetListData(iID_DuAnID, dBatDau, dKetThuc, iCurrentPage, false);
}
function GetListData(iID_DuAnID, dBatDau, dKetThuc, iCurrentPage, isRefresh) {

    if (!isRefresh) {
        if (!ValidateData(iID_DuAnID)) {
            return false;
        }
    }
    _paging.CurrentPage = iCurrentPage;
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/BaoCaoTinhHinhThucHienDuAn/FindDetails",
        data: { _paging: _paging, iID_DuAnID: iID_DuAnID, dBatDau: dBatDau, dKetThuc: dKetThuc },
        success: function (data) {
            $("#lstDataView").html(data);
            $("#txtiID_DuAnID").html(iID_DuAnID);
            $("#txtdBatDau").val(dBatDau);
            $("#txtdKetThuc").val(dKetThuc);
        }
    });
}
/*function ExportExcel(ex) {
    var ext = ex;
    var links = [];
    var iID_DuAnID = $("#txtiID_DuAnID").val();
    if (!isRefresh) {
        if (!ValidateData(iID_DuAnID)) {
            return false;
        }
    }
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/BaoCaoTinhHinhThucHienDuAn/ExportGiayDeNghiThanhToan",
        data: { _paging: _paging, iID_DuAnID: iID_DuAnID},
        success: function (data) {
            $("#lstDataView").html(data);
            $("#txtiID_DuAnID").val(iID_DuAnID);
            var url = "?ext=" + ext;
            url = unescape(url);
            links.push(url);

            openLinks(links);
        }
    });
}*/
function ExportExcel(ex) {
    var ext = ex;
    var iID_DuAnID = $("#txtiID_DuAnID").val();

    if (!ValidateData(iID_DuAnID)) {
        return false;
    }
    var dBatDau = $("#txtdBatDau").val();
    var dKetThuc = $("#txtdKetThuc").val();

    window.open("/BaoCaoTinhHinhThucHienDuAn/ExportGiayDeNghiThanhToan/?ext=" + ext + "&iID_DuAnID=" + iID_DuAnID + "&dBatDau=" + dBatDau + "&dKetThuc=" + dKetThuc, "_blank");
    return false;
}
function GetDuAnByDonVi(sel) {
    $('#txtiID_DuAnID option').remove();
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/BaoCaoTinhHinhThucHienDuAn/GetSeach",
        data: { id: sel.value },
        dataType: "json",
        success: function (result) {
            if (result.status == true) {
                var str = "<option value ='00000000-0000-0000-0000-000000000000'>-- Chọn dự án --</option>";
                $.each(result.data, function (index, val) {
                    str = str + "<option  value = '" + val.ID + "'>" + val.sTenDuAn + "</option>";
                });
                $("#txtiID_DuAnID").append(str);
            }
        }
    });
}
function ValidateData(iID_DuAnID) {
    var Title = 'Lỗi chọn dự án';
    var Messages = [];
    if (iID_DuAnID == null || iID_DuAnID == "00000000-0000-0000-0000-000000000000") {
        Messages.push("VuiLòng chọn dự án !");
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