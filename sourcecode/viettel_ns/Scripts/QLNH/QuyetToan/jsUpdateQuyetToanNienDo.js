var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;



///save data ---------------------------------
function Save(isTongHop) {
    var data = GetDataQuyetToanDienDo();
    if (!ValidateData(data, isTongHop)) {
        return false;
    }
    $.ajax({
        type: "POST",
        url: "/QLNH/QuyetToanNienDo/Save",
        data: { data: data },
        success: function (r) {
            if (r && r.bIsComplete) {
                window.location.href = "/QLNH/QuyetToanNienDo/Detail?id=" + r.dataID + "&edit=" + !isTongHop;
            } else {
                var Title = 'Lỗi lưu thông tin quyết toán niên độ';
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
function SaveTongHop(listId) {
    var data = GetDataQuyetToanDienDo();
    if (!ValidateData(data, true)) {
        return false;
    }
    $.ajax({
        type: "POST",
        url: "/QLNH/QuyetToanNienDo/SaveTongHop",
        data: { data: data, listId: listId },
        success: function (r) {
            if (r && r.bIsComplete) {
                window.location.href = "/QLNH/QuyetToanNienDo";
            } else {
                var Title = 'Lỗi lưu thông tin quyết toán niên độ';
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

function GetDataQuyetToanDienDo() {
    var data = {};
    data.ID = $("#hidQTNienDoID").val();
    data.sSoDeNghi = $("#txtSoDeNghi").val();
    data.dNgayDeNghi = $("#txtNgayDeNghi").val();
    data.iNamKeHoach = $("#slbNamKeHoach").val() == 0 ? null : $("#slbNamKeHoach").val();
    data.iLoaiQuyetToan = $("#slbLoaiQuyetToan").val() == 0 ? null : $("#slbLoaiQuyetToan").val();

    data.sMoTa = $("#txtMoTa").val();
    data.iID_DonViID = $("#slbDonVi").val() == GUID_EMPTY ? null : $("#slbDonVi").val();
    data.iID_TiGiaID = $("#slbTiGia").val() == GUID_EMPTY ? null : $("#slbTiGia").val();
    data.iID_MaDonVi = $("#slbDonVi").find("option:selected").data("madonvi");
    return data;
}


///Validate///------------------------------------------------------------------------
function ValidateData(data, isTongHop) {
    var Title = 'Lỗi thêm mới/chỉnh sửa quyết toán niên độ';
    var Messages = [];

    if (data.sSoDeNghi == null || data.sSoDeNghi == "") {
        Messages.push("Số đề nghị chưa nhập !");
    }
    if (!isTongHop) {
        if (data.iID_DonViID == null || data.iID_DonViID == GUID_EMPTY) {
            Messages.push("Đơn vị  chưa chọn !");
        }
        if (data.iLoaiQuyetToan == null || data.iLoaiQuyetToan == 0) {
            Messages.push("Loại quyết toán chưa chọn !");
        }
    }

    
    if (data.iNamKeHoach == null || data.iNamKeHoach == 0) {
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
        return false;
    }

    return true;
}
function ValidateNumberKeyDown(event) {
    if (!(!event.shiftKey //Disallow: any Shift+digit combination
        && !(event.keyCode < 48 || event.keyCode > 57) //Disallow: everything but digits
        || !(event.keyCode < 96 || event.keyCode > 105) //Allow: numeric pad digits
        || event.keyCode == 46 // Allow: delete
        || event.keyCode == 8  // Allow: backspace
        || event.keyCode == 9  // Allow: tab
        || event.keyCode == 27 // Allow: escape
        || (event.keyCode == 65 && (event.ctrlKey === true || event.metaKey === true)) // Allow: Ctrl+A
        || (event.keyCode == 67 && (event.ctrlKey === true || event.metaKey === true)) // Allow: Ctrl+C
        //|| (event.keyCode == 86 && (event.ctrlKey === true || event.metaKey === true)) // Allow: Ctrl+Vpasting 
        || (event.keyCode >= 35 && event.keyCode <= 39) // Allow: Home, End
    )) {
        event.preventDefault();
    }
}
