var CONFIRM = 0;
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;
var QT_Tai_San = "qttaisan";
var arr_DataDonVi = [];
var ListTaiSan = [];
var arr_DataDuAn = [];
var arr_DataHopDong = [];
var arr_DataTinhTrang = [];
var arr_DataLoaiTaiSan = [];
var arr_DataTinhTrangSuDung = [];
$(document).ready(function () {
    GetDropdown();
    GetDropdownDuAn();
    GetDropdownHopDong();
    GetDropdownTinhTrang();
    GetDropdownLoaiTaiSan();
    GetDropdownTinhTrangSuDung();

    function GetDropdown() {
        $.ajax({
            type: "POST",
            url: "/QLNH/TaiSan/GetDropdown",
            success: function (data) {
                arr_DataDonVi = data.data;
            }
        });
    }
    function GetDropdownDuAn() {
        $.ajax({
            type: "POST",
            url: "/QLNH/TaiSan/GetDropdownDuAn",
            success: function (data) {
                arr_DataDuAn = data.data;
            }
        });
    }
    function GetDropdownHopDong() {
        $.ajax({
            type: "POST",
            url: "/QLNH/TaiSan/GetDropdownHopDong",
            success: function (data) {
                arr_DataHopDong = data.data;
            }
        });
    }
    function GetDropdownTinhTrang() {
        $.ajax({
            type: "POST",
            url: "/QLNH/TaiSan/GetDropdownTinhTrang",
            success: function (data) {
                arr_DataTinhTrang = data.data;
            }
        });
    }
    function GetDropdownLoaiTaiSan() {
        $.ajax({
            type: "POST",
            url: "/QLNH/TaiSan/GetDropdownLoaiTaiSan",
            success: function (data) {
                arr_DataLoaiTaiSan = data.data;
            }
        });
    }
    function GetDropdownTinhTrangSuDung() {
        $.ajax({
            type: "POST",
            url: "/QLNH/TaiSan/GetDropdownTinhTrangSuDung",
            success: function (data) {
                arr_DataTinhTrangSuDung = data.data;
            }
        });
    }

    RefreshBindingSelect2();
})
function RefreshBindingSelect2() {
    $("#bodytable_create tr .listdonvi").select2({
        dropdownAutoWidth: true,
        width: '100%',
        matcher: FilterInComboBox
    });

    $("#bodytable_create tr .listduan").select2({
        dropdownAutoWidth: true,
        width: '100%',
        matcher: FilterInComboBox
    });
    $("#bodytable_create tr .listhopdong").select2({
        dropdownAutoWidth: true,
        width: '100%',
        matcher: FilterInComboBox
    });
}

function FilterInComboBox(params, data) {
    // If there are no search terms, return all of the data
    if ($.trim(params.term) === '') {
        return data;
    }

    // Do not display the item if there is no 'text' property
    if (typeof data.text === 'undefined') {
        return null;
    }

    if (data.text.toUpperCase().indexOf(params.term.toUpperCase()) > -1) {
        console.log(18, params.term, data)
        var modifiedData = $.extend({}, data, true);
        //modifiedData.text += ' (matched)';
        return modifiedData;
    }

    return null;
}
function CreateHtmlSelectDonVi(value) {
    var htmlOption = "";
    htmlOption += "<option value=''selected >--Chọn đơn vị--</option>";
    arr_DataDonVi.forEach(x => {
        if (value != undefined && value == x.iID_Ma)
            htmlOption += "<option data-madonvi='" + x.iID_MaDonVi + "' value='" + x.iID_Ma + "'selected >" + x.sTen+ "</option>";
        else
            htmlOption += "<option data-madonvi='" + x.iID_MaDonVi +"' value='" + x.iID_Ma + "' >" + x.sTen + "</option>";
    })
    return "<select class='form-control listdonvi tablethin' name='iID_DonViID' style='min-width: 150px'>" + htmlOption + "</option>";
}

function CreateHtmlSelectDuAn(value) {
    var htmlOption = "";
    htmlOption += "<option value=''selected >--Chọn dự án--</option>";
    arr_DataDuAn.forEach(x => {
        if (value != undefined && value == x.ID)
            htmlOption += "<option value='" + x.ID + "' selected>" + x.sTenDuAn + "</option>";
        else
            htmlOption += "<option value='" + x.ID + "' >" + x.sTenDuAn + "</option>";
    })

    return "<select class='form-control listduan tablethin'name = 'iID_DuAnID'style='min-width: 150px'>" + htmlOption + "</option>";
}

function CreateHtmlSelectHopDong(value) {
    var htmlOption = "";
    htmlOption += "<option value=''selected >--Chọn hợp đồng--</option>";
    arr_DataHopDong.forEach(x => {
        if (value != undefined && value == x.ID)
            htmlOption += "<option value='" + x.ID + "' selected>" + x.sTenHopDong + "</option>";
        else
            htmlOption += "<option value='" + x.ID + "' >" + x.sTenHopDong + "</option>";
    })
    return "<select class='form-control listhopdong tablethin'name = 'iID_HopDongID' style='min-width: 150px'>" + htmlOption + "</option>";
}
function CreateHtmlSelectTinhTrang(value) {
    var htmlOption = "";
    htmlOption += "<option value=''selected >--Chọn trạng thái sử dụng--</option>";
    arr_DataTinhTrang.forEach(x => {
        if (value != undefined && value == x.valueId)
            htmlOption += "<option value='" + x.valueId + "' selected>" + x.labelName + "</option>";
        else
            htmlOption += "<option value='" + x.valueId + "' >" + x.labelName + "</option>";
    })
    return "<select class='form-control tablethin'name='iTrangThai' id='txtiTrangThai'style='min-width: 150px'>" + htmlOption + "</option>";
}
function CreateHtmlSelectLoaiTaiSan(value) {
    var htmlOption = "";
    htmlOption += "<option value=''selected >--Chọn loại tài sản--</option>";
    arr_DataLoaiTaiSan.forEach(x => {
        if (value != undefined && value == x.valueId)
            htmlOption += "<option value='" + x.valueId + "' selected>" + x.labelName + "</option>";
        else
            htmlOption += "<option value='" + x.valueId + "' >" + x.labelName + "</option>";
    })
    return "<select class='form-control tablethin'name = 'iLoaiTaiSan' id = 'txtiLoaiTaiSan' style='min-width: 150px'>" + htmlOption + "</option>";
}
function CreateHtmlSelectTinhTrangSuDung(value) {
    var htmlOption = "";
    htmlOption += "<option value=''selected >--Chọn tình trạng sử dụng--</option>";
    arr_DataTinhTrangSuDung.forEach(x => {
        if (value != undefined && value == x.valueId)
            htmlOption += "<option value='" + x.valueId + "' selected>" + x.labelName + "</option>";
        else
            htmlOption += "<option value='" + x.valueId + "' >" + x.labelName + "</option>";
    })
    return "<select class='form-control tablethin'name = 'iTinhTrangSuDung' id = 'txtiTinhTrangSuDung'style='min-width: 150px' >" + htmlOption + "</option>";
}

function ResetChangePage(iCurrentPage = 1) {
    var sTenChungTu = "";
    var sSoChungTu = "";
    var dNgayChungTu = "";

    GetListData(sTenChungTu, sSoChungTu, dNgayChungTu, iCurrentPage);
}

function ChangePage(iCurrentPage = 1) {
    var sTenChungTu = $("#txtsTenChungTuFillter").val();
    var sSoChungTu = $("#txtsSochungTuFillter").val();
    var dNgayChungTu = $("#dNgayChungTuFillter").val();

    GetListData(sTenChungTu, sSoChungTu, dNgayChungTu, iCurrentPage);
}
function GetListData(sTenChungTu, sSoChungTu, dNgayChungTu, iCurrentPage) {
    _paging.CurrentPage = iCurrentPage;
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/TaiSan/DanhMucTaiSanSearch",
        data: { _paging: _paging, sTenChungTu: sTenChungTu, sSoChungTu: sSoChungTu, dNgayChungTu: dNgayChungTu},
        success: function (data) {
            $("#lstDataView").html(data);
            $("#txtsTenChungTu").val(sTenChungTu);
            $("#txtsSochungTu").val(sSoChungTu);
            $("#dNgayChungTu").val(dNgayChungTu);
        }
    });
}
function OpenModalDetail(id) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/TaiSan/GetModalDetail",
        data: { id: id },
        success: function (data) {
            $("#lstDataView").html(data);
            $(".date").datepicker({
                todayBtn: "linked",
                language: "it",
                autoclose: true,
                todayHighlight: true,
                format: 'dd/mm/yyyy'
            });
        }
    });
}
function OpenModal(id) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/TaiSan/GetModal",
        data: { id: id },
        success: function (data) {
            $("#lstDataView").html(data);
            $(".date").datepicker({
                todayBtn: "linked",
                language: "it",
                autoclose: true,
                todayHighlight: true,
                format: 'dd/mm/yyyy'
            });
        }
    });
}
function OpenCreate() {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/TaiSan/GetCreate",
        success: (e) => {
            $("#lstDataView").html(e);
            $(".date").datepicker({
                todayBtn: "linked",
                language: "it",
                autoclose: true,
                todayHighlight: true,
                format: 'dd/mm/yyyy'
            });
        }
    });
}
function back() {
    window.location.href = "/QLNH/TaiSan";
}

function DeleteItem(id) {
    $.ajax({
        type: "POST",
        url: "/QLNH/TaiSan/TaiSanDelete",
        data: { id: id },
        success: function (r) {
            if (r.bIsComplete) {
                ChangePage();
            } else {
                var Title = 'Lỗi xóa tài sản';
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
function Delete(id) {
    $.ajax({
        type: "POST",
        url: "/QLNH/TaiSan/ChungTuTaiSanDelete",
        data: { id: id },
        success: function (r) {
            if (r.bIsComplete) {
                ChangePage();
            } else {
                var Title = 'Lỗi';
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


function Xoa(id) {
    var Title = 'Xác nhận xóa chứng từ tài sản';
    var Messages = [];
    Messages.push('Bạn có chắc chắn muốn xóa?');
    var FunctionName = "Delete('" + id + "')";
    $.ajax({
        type: "POST",
        url: "/Modal/OpenModal",
        data: { Title: Title, Messages: Messages, Category: CONFIRM, FunctionName: FunctionName },
        success: function (data) {
            $("#divModalConfirm").empty().html(data);
        }
    });
}

function Save() {
    let result = [];

    $(".table-update tbody tr").each(function () {
        var allValues = {};

        $(this).find("input").each(function (index) {
            const fieldName = $(this).attr("name");
            allValues[fieldName] = $(this).val();
        });
        $(this).find("select").each(function (index) {
            const fieldName = $(this).attr("name");
            if (fieldName == "iID_DonViID") {
                allValues["iID_MaDonVi"] = $(this).find("option:selected").data("madonvi")
            }
            allValues[fieldName] = $(this).val();
        });
        result.push(allValues);
    })
    var data = {};
    data.ID = $("#idChungTu").val();
    data.sTenChungTu = $("#txtsTenChungTu").val();
    data.sSoChungTu = $("#txtsSoChungTu").val();
    data.dNgayChungTu = $("#txtdNgayChungTu").val();

    if (!ValidateData(data)) {
        return false;
    }
    $.ajax({
       type: "post",
        url: "/QLNH/TaiSan/TaiSanSave",
        data: { datactts: data, datats: result},
        success: function (r) {
            if (r.bIsComplete == true) {
                window.location.href = "/QLNH/TaiSan";
            } else {
                var title = 'lỗi lưu data';
                var messerr = [];
                messerr.push(r.smesserror);
                $.ajax({
                    type: "post",
                  url: "/modal/openmodal",
                  data: { title: title, messages: messerr, category: error },
                  success: function (data) {
                       $("#divmodalconfirm").html(data);
                 }
             });
         }
      }
    });
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

function SaveUpdate() {
    let result = [];

    $("#tbListTaiSans tbody tr").each(function () {
        var allValues = {};

        $(this).find("input").each(function (index) {
            const fieldName = $(this).attr("name");
            allValues[fieldName] = $(this).val();
        });
        $(this).find("select").each(function (index) {
            const fieldName = $(this).attr("name");
            if (fieldName == "iID_DonViID") {
                allValues["iID_MaDonVi"] = $(this).find("option:selected").data("madonvi")
            }
            allValues[fieldName] = $(this).val();
        });
        result.push(allValues);
    })

    var data = {};
    data.ID = $("#idChungTu").val();
    data.sTenChungTu = $("#txtsTenChungTu").val();
    data.sSoChungTu = $("#txtsSoChungTu").val();
    data.dNgayChungTu = $("#txtdNgayChungTu").val();
    console.log(data)
    if (!ValidateData(data)) {
        return false;
    }
    $.ajax({
        type: "post",
        url: "/QLNH/TaiSan/TaiSanSave",
        data: { datactts: data, datats: result },
        success: function (r) {
            if (r.bIsComplete == true) {
                window.location.href = "/QLNH/TaiSan";
            } else {
                var title = 'lỗi lưu data';
                var messerr = [];
                messerr.push(r.smesserror);
                $.ajax({
                    type: "post",
                    url: "/modal/openmodal",
                    data: { title: title, messages: messerr, category: error },
                    success: function (data) {
                        $("#divmodalconfirm").html(data);
                    }
                });
            }
        }
    });
}

function ValidateData(data, datats) {
    var Title = 'Lỗi thêm mới/chỉnh sửa tài sản';
    var Messages = [];
    if (data.sSoChungTu == null || data.sSoChungTu == "") {
        Messages.push("Số chứng từ chưa nhập !");
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


function ThemMoiTaiSan(data) {
    var dongMoi = "";
    dongMoi += "<tr style='cursor: pointer'>";
    dongMoi += "<td class='r_STT' align='center'></td><input type='hidden' class='' value=''/>";
    dongMoi += "<td align='left'> <input type='text' id='txtsMaTaiSan' name='sMaTaiSan' class='form-control tablethin' /></td>";
    dongMoi += "<td align='left'> <input type='text' id='txtsTenTaiSan' name='sTenTaiSan' class='form-control tablethin'  /></td>";
    dongMoi += "<td><div name = 'iLoaiTaiSan' id = 'txtiLoaiTaiSan' hidden></div><div name = 'iLoaiTaiSan' id = 'txtiLoaiTaiSan'>" + CreateHtmlSelectLoaiTaiSan() + "</div></td>";
    dongMoi += "<td align='left'> <input type='text' id='txtsMoTaTaiSan' name='sMoTaTaiSan' class='form-control tablethin'  /></td>";
    dongMoi += "<td align='left'><div class='input-group date'><input type='text' name='dNgayBatDauSuDung' id='txtdNgayBatDauSuDung' class='form-control tablethin'value=''autocomplete='off' placeholder='dd/MM/yyyy' /><span class='btn-default input-group-addon input-calendar'><i class='fa fa-calendar' aria-hidden='true'></i></span></div ></td>";
    dongMoi += "<td><div name='iTrangThai' id='txtiTrangThai' ></div><div  name='iTrangThai' id='txtiTrangThai'>" + CreateHtmlSelectTinhTrang() + "</div></td>";
    dongMoi += "<td align='left'> <input type='text' id='txtfSoLuong' name='fSoLuong' class='form-control tablethin'onkeydown='ValidateNumberKeyDown(event);'/></td>";
    dongMoi += "<td align='left'> <input type='text' id='txtsDonViTinh' name='sDonViTinh' class='form-control tablethin' /></td>";
    dongMoi += "<td align='left'> <input type='text' id='txtfNguyenGia' name='fNguyenGia' class='form-control tablethin'onkeydown='ValidateNumberKeyDown(event);' /></td>";
    dongMoi += "<td><div name = 'iTinhTrangSuDung' id = 'txtiTinhTrangSuDung' hidden></div><div class='selectHopDong'name = 'iTinhTrangSuDung' id = 'txtiTinhTrangSuDung'>" + CreateHtmlSelectTinhTrangSuDung() + "</div></td>";
    dongMoi += "<td><div>" + CreateHtmlSelectDonVi() + "</div></td>";
    dongMoi += "<td><div >" + CreateHtmlSelectDuAn() + "</div></td>";
    dongMoi += "<td><div >" + CreateHtmlSelectHopDong() + "</div></td>";
    dongMoi += "<td><button class='btn-delete btn-icon' type='button' onclick='XoaDong(this)'><i class='fa fa-trash' aria-hidden='true' ></i> Xóa</button></td>";
    dongMoi += "</tr>";
    $("#bodytable_create tr:last").after(dongMoi);
    CapNhatCotSttTaiSan();
    $(".date").datepicker({
        todayBtn: "linked",
        language: "it",
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy'
    });

    RefreshBindingSelect2();
}

function XoaDong(nutXoa) {
    var dongXoa = nutXoa.parentElement.parentElement;
    dongXoa.parentNode.removeChild(dongXoa);
}

function CapNhatCotSttTaiSan() {

    $("#bodytable_create tr").each(function (index, tr) {
        $(tr).find('.r_STT').text(index + 1);
    });
}

function CapNhatCotSttTS() {
    $(".table-update-modal tr").each(function (index, tr) {
        $(tr).find('.r_STT').text(index + 1);
    });
}

function ThemMoi(data) {
    var dongMois = "";
    dongMois += "<tr>";
    dongMois += "<td class='r_STT' align='center'></td><input type='hidden' name='ID' id='ID' value=''></td>";
    dongMois += "<td align='left'> <input type='text' id='txtsMaTaiSan' name='sMaTaiSan' class='form-control tablethin' /></td>";
    dongMois += "<td align='left'> <input type='text' id='txtsTenTaiSan' name='sTenTaiSan' class='form-control tablethin'  /></td>";
    dongMois += "<td><div name = 'iLoaiTaiSan' id = 'txtiLoaiTaiSan' hidden></div><div name = 'iLoaiTaiSan' id = 'txtiLoaiTaiSan'>" + CreateHtmlSelectLoaiTaiSan() + "</div></td>";
    dongMois += "<td align='left'> <input type='text' id='txtsMoTaTaiSan' name='sMoTaTaiSan' class='form-control tablethin'  /></td>";
    dongMois += "<td align='left'><div class='input-group date'><input type='text'  name='dNgayBatDauSuDung' id='txtdNgayBatDauSuDung' class='form-control tablethin'value=''autocomplete='off' placeholder='dd/MM/yyyy' /><span class='btn-default input-group-addon input-calendar'><i class='fa fa-calendar' aria-hidden='true'></i></span></div ></td>";
    dongMois += "<td><div name='iTrangThai' id='txtiTrangThai' ></div><div  name='iTrangThai' id='txtiTrangThai'>" + CreateHtmlSelectTinhTrang() + "</div></td>";
    dongMois += "<td align='left'> <input type='text' id='txtfSoLuong' name='fSoLuong' class='form-control tablethin' onkeydown='ValidateNumberKeyDown(event);'/></td>";
    dongMois += "<td align='left'> <input type='text' id='txtsDonViTinh' name='sDonViTinh' class='form-control tablethin' /></td>";
    dongMois += "<td align='left'> <input type='text' id='txtfNguyenGia' name='fNguyenGia' class='form-control tablethin' onkeydown='ValidateNumberKeyDown(event);'/></td>";
    dongMois += "<td><div name = 'iTinhTrangSuDung' id = 'txtiTinhTrangSuDung' hidden></div><div class='selectHopDong'name = 'iTinhTrangSuDung' id = 'txtiTinhTrangSuDung'>" + CreateHtmlSelectTinhTrangSuDung() + "</div></td>";
    dongMois += "<td><div>" + CreateHtmlSelectDonVi() + "</div></td>";
    dongMois += "<td><div>" + CreateHtmlSelectDuAn() + "</div></td>";
    dongMois += "<td><div>" + CreateHtmlSelectHopDong() + "</div></td>";
    dongMois += "<td><button class='btn-delete btn-icon' type='button' onclick='XoaDong(this)'><i class='fa fa-trash' aria-hidden='true' ></i> Xóa</button></td>";
    dongMois += "</tr>";
    $("#tbListTaiSans tbody").append(dongMois);
    CapNhatCotSttTS();
    $(".date").datepicker({
        todayBtn: "linked",
        language: "it",
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy'
    });

}

function LoadDataViewChitiet() {
    var data = GetListDataNguonVon();
    if (data != null) {
        for (var i = 0; i < data.length; i++) {
            var dongMoi = "";
            dongMoi += "<tr style='cursor: pointer;' class='parent'>";
            dongMoi += "<td class='r_STT width-50'>" + (i + 1) + "</td>";
            dongMoi += "<td hidden><input type='hidden' name='ID' id='ID' value='" + data[i].ID + "'></td>";
            dongMoi += "<td align='left'> <input type='text' id='txtsMaTaiSan' name='sMaTaiSan' class='form-control tablethin' value='" + data[i].sMaTaiSan + "'  /></td>";
            dongMoi += "<td align='left'> <input type='text' id='txtsTenTaiSan' name='sTenTaiSan' class='form-control tablethin' value='" + data[i].sTenTaiSan + "'/></td>";
            dongMoi += "<td><div name = 'iLoaiTaiSan' id = 'txtiLoaiTaiSan' hidden></div><div name = 'iLoaiTaiSan' id = 'txtiLoaiTaiSan' value='" + data[i].iLoaiTaiSan + "'>" + CreateHtmlSelectLoaiTaiSan(data[i].iLoaiTaiSan) + "</div></td>";
            dongMoi += "<td align='left'> <input type='text' id='txtsMoTaTaiSan' name='sMoTaTaiSan' class='form-control tablethin'value='" + data[i].sMoTaTaiSan + "' /></td>";
            dongMoi += "<td align='left'><div class='input-group date'><input type='text'  name='dNgayBatDauSuDung' id='txtdNgayBatDauSuDung' class='form-control tablethin'value='" + data[i].dNgayBatDauSuDungStr + "'autocomplete='off' placeholder='dd/MM/yyyy' /><span class='btn-default input-group-addon input-calendar'><i class='fa fa-calendar' aria-hidden='true'></i></span></div ></td>";
            dongMoi += "<td><div name='iTrangThai' id='txtiTrangThai' ></div><div  name='iTrangThai' id='txtiTrangThai'value='" + data[i].iTrangThai + "'>" + CreateHtmlSelectTinhTrang(data[i].iTrangThai) + "</div></td>";
            dongMoi += "<td align='left'> <input type='text' id='txtfSoLuong' name='fSoLuong' class='form-control tablethin' onkeydown='ValidateNumberKeyDown(event);' value='" + data[i].fSoLuong + "' /></td>";
            dongMoi += "<td align='left'> <input type='text' id='txtsDonViTinh' name='sDonViTinh' class='form-control tablethin' value='" + data[i].sDonViTinh + "' /></td>";
            dongMoi += "<td align='left'> <input type='text' id='txtfNguyenGia' name='fNguyenGia' class='form-control tablethin'onkeydown='ValidateNumberKeyDown(event);' value='" + data[i].fNguyenGia + "'/></td>";
            dongMoi += "<td><div name = 'iTinhTrangSuDung' id = 'txtiTinhTrangSuDung' hidden></div><div class='selectHopDong'name = 'iTinhTrangSuDung' id = 'txtiTinhTrangSuDung' value='" + data[i].iTinhTrangSuDung + "'>" + CreateHtmlSelectTinhTrangSuDung(data[i].iTinhTrangSuDung) + "</div></td>";
            dongMoi += "<td><div value='" + data[i].iID_DonViID + "'>" + CreateHtmlSelectDonVi(data[i].iID_DonViID) + "</div></td>";
            dongMoi += "<td><div value='" + data[i].ID + "'>" + CreateHtmlSelectDuAn(data[i].iID_DuAnID) + "</div></td>";
            dongMoi += "<td><div value='" + data[i].ID + "'>" + CreateHtmlSelectHopDong(data[i].iID_HopDongID) + "</div></td>";
            dongMoi += "<td><button class='btn-delete btn-icon' type='button' onclick='XoaDong(this)'><i class='fa fa-trash' aria-hidden='true' ></i> Xóa</button></td>";
            dongMoi += "</tr>";

            $("#tbListTaiSans tbody").append(dongMoi);

            CapNhatCotSttTS();
            $(".date").datepicker({
                todayBtn: "linked",
                language: "it",
                autoclose: true,
                todayHighlight: true,
                format: 'dd/mm/yyyy'
            });
        }
    }
}
function Event() {

    arrChiQuy = GetListDataChitietJson();
    ListTaiSan = GetListDataChitietJson();
    LoadDataViewChitiet();

}
function GetListDataNguonVon() {
    return ListTaiSan;
}
function GetListDataChitietJson() {
    var items = $("#arrTaiSan").val();
    if (isStringEmpty(items)) {
        return [];
    }

    items = JSON.parse(items);

    if (items != undefined && items.length > 0) {
        for (var i = 0; i < items.length; i++) {
            items[i].id = (i + 1).toString();
        }
    }

    return items;
}
function isStringEmpty(x) {
    if (x == null||  x == undefined || x == "") {
        return true;
    }

    return false;
}

function arrHasValue(x) {
    if (  x != undefined|| x.length <= 0) {
        return false;
    }

    return true;
}