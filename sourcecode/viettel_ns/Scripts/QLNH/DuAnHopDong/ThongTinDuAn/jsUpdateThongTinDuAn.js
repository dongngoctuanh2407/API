var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;
var maTienTe = ["USD", "VND", "EUR"];
var tbListChiphi = "tbListChiphi"
var lstNgoaiUSD = [];
var arr_DataTenChiPhi = [];

$(document).ready(function () {
    LoadDataTenChiPhi()
    if ($("#slbMaNgoaiTeKhac").val() != GUID_EMPTY) {
        $("#iDTenNgoaiTeKhac").html($("#slbMaNgoaiTeKhac option:selected").html());
    }

    if ($("#slbTiGia").val() != GUID_EMPTY) {
        var maGoc = $("#slbTiGia option:selected").data("mg");
        if (maTienTe.indexOf(maGoc) >= 0) {
            switch (maGoc) {
                case "USD":
                    $("input[name=HopDongVND]").prop("readonly", true);
                    $("input[name=HopDongEUR]").prop("readonly", true);
                    break;
                case "VND":
                    $("input[name=HopDongUSD]").prop("readonly", true);
                    $("input[name=HopDongEUR]").prop("readonly", true);
                    break;
                case "EUR":
                    $("input[name=HopDongUSD]").prop("readonly", true);
                    $("input[name=HopDongVND]").prop("readonly", true);
                    break;
                default:
                    break;
            }
            $("input[name=HopDongNgoaiTeKhac]").prop("readonly", true);
        } else {
            if ($("#slbMaNgoaiTeKhac").val() != GUID_EMPTY) {
                $("input[name=HopDongUSD]").prop("readonly", true);
                $("input[name=HopDongVND]").prop("readonly", true);
                $("input[name=HopDongEUR]").prop("readonly", true);
            }
        }
    }
    function LoadDataTenChiPhi() {
        $.ajax({
            async: false,
            url: "/QLNH/ThongTinDuAn/GetLookupChiPhi",
            type: "POST",
            dataType: "json",
            cache: false,
            success: function (data) {
                arr_DataTenChiPhi = data.data;
            }
        });
    }

});

function ChangeBQPSelect() {
    var id = $("#slbKHTongTheBQP").val();
    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTinDuAn/GetChuongTrinhTheoKHBQP",
        data: { id: id },
        success: function (data) {
            if (data) {
                $("#slbDonVi").empty().html(data.htmlDV);
            }
        }
    });
}
function ChangeBQPSelectImport(element) {
    var value = $(element).val();
    let idelement = $(element).attr('id');
    let index = idelement.replace('slbKHTongTheBQP', '');
    let idElementDonVi = 'slbDonVi' + index;
    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTinDuAn/GetChuongTrinhTheoKHBQP",
        data: { id: value },
        success: function (data) {
            if (data) {
                $("#" + idElementDonVi).empty().html(data.htmlDV);
            }
        }
    });
}

function ChangeDVSelectImport(element) {
    var value = $(element).val();
    let idelement = $(element).attr('id');
    let index = idelement.replace('slbDonVi', '');
    let idElementCT = 'slbChuongTrinh' + index;

    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTinDuAn/GetChuongTrinhTheoDV",
        data: { id: value },
        success: function (data) {
            if (data) {
                $("#" + idElementCT).empty().html(data.htmlCT);
            }
        }
    });
}


function ChangeDVSelect() {
    var idBQP = $("#slbKHTongTheBQP").val();
    var id = $("#slbDonVi").val();
    console.log(id);
    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTinDuAn/GetChuongTrinhTheoDV",
        data: { id: id, idBQP: idBQP},
        success: function (data) {
            if (data) {
                $("#slbChuongTrinh").empty().html(data.htmlCT);
            }
        }
    });

}

function CreateHtmlSelectTenChiPhi(value) {
    var htmlOption = "";
    arr_DataTenChiPhi.forEach(x => {
        if (value != undefined && value == x.ID)
            htmlOption += "<option value='" + x.ID + "' selected>" + x.sTenChiPhi + "</option>";
        else
            htmlOption += "<option value='" + x.ID + "' >" + x.sTenChiPhi + "</option>";
    })
    return "<select class='form-control dataTableChiPhi checkSelected' onchange='ChangeSelection(this)' name = 'iID_ChiPhiID' id = 'txtiTenChiPhi' style='min-width: 150px'>" + htmlOption + "</option>";
}
function GetListDataChitietJson() {
    var items = $("#arr_DataTenChiPhi").val();

    if (!items) {
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


function arrHasValue(x) {
    if (x == null || x == undefined || x.length <= 0) {
        return false;
    }

    return true;
}


function GetListDataChiPhi() {
    return lstNgoaiUSD;
}




function ThemMoiChiPhi() {
    var dongMoi = "";
    dongMoi += "<tr style='cursor: pointer;' class='parent'>";
    dongMoi += "<td class='r_STT' align='center'></td><input type='hidden' name='ID' id='ID' value=''></td>";
    dongMoi += "<td><div name = 'sTenChiPhi' id = 'txtiTenChiPhi' hidden></div><div name = 'sTenChiPhi' id = 'txtiTenChiPhi'>" + CreateHtmlSelectTenChiPhi() + "</div></td>";
    dongMoi += "<td class='ChiPhi' align='left'><div class='ChiPhi' hidden></div><input id = 'txtHopDongUSD' name = 'HopDongUSD' type='text' class='form-control  dataTableChiPhi' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' onblur = 'ChangeGiaTien(this);' /></td>";
    dongMoi += "<td class='ChiPhi' align='left'><div class='ChiPhi' hidden></div><input id = 'txtHopDongVND' name = 'HopDongVND' type='text' class='form-control  dataTableChiPhi' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' onblur = 'ChangeGiaTien(this);' /></td>";
    dongMoi += "<td class='ChiPhi' align='left'><div class='ChiPhi' hidden></div><input id = 'txtHopDongEUR' name = 'HopDongEUR' type='text' class='form-control  dataTableChiPhi' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' onblur = 'ChangeGiaTien(this);' /></td>";
    dongMoi += "<td class='ChiPhi' align='left'><div class='ChiPhi' hidden></div><input id = 'txtHopDongNgoaiTeKhac' name = 'HopDongNgoaiTeKhac'type='text' class='form-control  dataTableChiPhi' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' onblur = 'ChangeGiaTien(this);'/></td>";
    dongMoi += "<td><button class='btn-delete btn-icon' type='button' onclick='XoaDong(this)'><i class='fa fa-trash' aria-hidden='true' ></button></td>";
    dongMoi += "</tr>";
    $("#bodytable_create tbody").append(dongMoi);
    CapNhatCotSttChiPhi();
    $(".date").datepicker({
        todayBtn: "linked",
        language: "it",
        autoclose: true,
        todayHighlight: true,
    });

}
function CapNhatCotSttChiPhi() {

    $("#bodytable_create tr").each(function (index, tr) {
        $(tr).find('.r_STT').text(index + 1);
    });
}

function CapNhatCotSttTS() {
    $("#tbListChiphi tbody tr").each(function (index, tr) {
        $(tr).find('.r_STT').text(index + 1);
    });
}
function ThemMoi() {
    var numberOfRow = $("#tbListChiphi tbody tr").length;
    if (numberOfRow >= arr_DataTenChiPhi.length) {
        return;
    }
    var dongMois = "";

    dongMois += "<tr style='cursor: pointer;' class='parent'>";
    dongMois += "<td class='r_STT' align='center'></td><input type='hidden' name='ID' id='ID' value=''></td>";
    dongMois += "<td><div name = 'sTenChiPhi' id = 'txtiTenChiPhi' hidden></div><div name = 'sTenChiPhi' id = 'txtiTenChiPhi'>" + CreateHtmlSelectTenChiPhi() + "</div></td>";
    dongMois += "<td class='ChiPhi' align='left'><div class='ChiPhi' hidden></div><input id = 'txtHopDongUSD' name = 'HopDongUSD' type='text' class='form-control  dataTableChiPhi' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' onblur = 'ChangeGiaTien(this);' /></td>";
    dongMois += "<td class='ChiPhi' align='left'><div class='ChiPhi' hidden></div><input id = 'txtHopDongVND' name = 'HopDongVND' type='text' class='form-control  dataTableChiPhi' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' onblur = 'ChangeGiaTien(this);' /></td>";
    dongMois += "<td class='ChiPhi' align='left'><div class='ChiPhi' hidden></div><input id = 'txtHopDongEUR' name = 'HopDongEUR' type='text' class='form-control  dataTableChiPhi' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' onblur = 'ChangeGiaTien(this);' /></td>";
    dongMois += "<td class='ChiPhi' align='left'><div class='ChiPhi' hidden></div><input id = 'txtHopDongNgoaiTeKhac' name = 'HopDongNgoaiTeKhac' type='text' class='form-control  dataTableChiPhi' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' onblur = 'ChangeGiaTien(this);' /></td>";
    dongMois += "<td align='center'><button class='btn-delete btn-icon' type='button' onclick='XoaDong(this)'><i class='fa fa-trash' aria-hidden='true' ></button></td>";
    dongMois += "</tr>";
    $("#tbListChiphi tbody").append(dongMois);
    $(".date").datepicker({
        todayBtn: "linked",
        language: "it",
        autoclose: true,
        todayHighlight: true,
    });

    $("input[name='HopDongUSD']").last().blur();
    CapNhatCotSttTS();

}

function ChangeSelection(e) {
    value = $(e).val();
    $(".checkSelected").each(function () {
        $(this).find("option").each(function () {
            if ($(this).val() == value) {
                $(this).prop('disabled', true);
            }
        });
    })
}

function LoadDataViewChitiet(ID,state) {
    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTinDuAn/GetListChiPhiThongTinDuAn",
        async: false,
        data: { id: ID },
        success: function (data) {
            if (data != null) {
                for (var i = 0; i < data.length; i++) {
                    var dongMoi = "";
                    dongMoi += "<tr style='cursor: pointer;' class='parent'>";
                    dongMoi += "<td class='r_STT width-50' align='center'>" + (i + 1) + "</td>";
                    dongMoi += "<td hidden><input type='hidden' name='ID' id='ID' class='dataTableChiPhi' value='" + data[i].ID + "'></td>";
                    dongMoi += "<td>" + CreateHtmlSelectTenChiPhi(data[i].iID_ChiPhiID) + "</td>";  
                    dongMoi += "<td align='left'><div class='fHopDongUSD'><input type='text' id ='txtHopDongUSD' name ='HopDongUSD' class='form-control dataTableChiPhi'onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' onblur = 'ChangeGiaTien(this);' value='" + FormatNumber(data[i].fGiaTriUSD, 2) + "'  /></td>";
                    dongMoi += "<td align='left'><div class='fHopDongVND'><input type='text' id ='txtHopDongVND' name ='HopDongVND' class='form-control dataTableChiPhi'onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' onblur = 'ChangeGiaTien(this);' value='" + FormatNumber(data[i].fGiaTriVND, 0) + "'/></td>";
                    dongMoi += "<td align='left'><div class='fHopDongEUR'><input type='text' id ='txtHopDongEUR' name ='HopDongEUR' class='form-control dataTableChiPhi'onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' onblur = 'ChangeGiaTien(this);' value='" + FormatNumber(data[i].fGiaTriEUR, 2) + "'  /></td>";
                    dongMoi += "<td align='left'><div class='fHopDongNgoaiTeKhac' ></div><input type='text' id = 'txtHopDongNgoaiTeKhac' name='HopDongNgoaiTeKhac' class='form-control dataTableChiPhi' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);'  onblur = 'ChangeGiaTien(this);' value='" + FormatNumber(data[i].fGiaTriNgoaiTeKhac, 2) + "'/></td>";
                    dongMoi += "<td align='center'><button class='btn-delete btn-icon' type='button' onclick='XoaDong(this)'><i class='fa fa-trash' aria-hidden='true' ></button></td>";
                    dongMoi += "</tr>";
                    $("#tbListChiphi tbody").append(dongMoi);
                    CapNhatCotSttTS();
                    $(".date").datepicker({
                        todayBtn: "linked",
                        language: "it",
                        autoclose: true,
                        todayHighlight: true,
                    });
                }
            }
        }
    });;
}
function Event() {    
    arrChiQuy = GetListDataChitietJson();
    lstNgoaiUSD = GetListDataChitietJson();
    
    let state = $("#hState").val();
    let ID = $("#hidDuAnID").val();
    console.log(state);
    if (state == 'CREATE' || state == 'ADJUST' || state == 'UPDATE') {
        if (ID == GUID_EMPTY) {
            ID = null;
        }
        LoadDataViewChitiet(ID);
    }
}

function XoaDong(nutXoa) {
    var dongXoa = nutXoa.parentElement.parentElement;
    dongXoa.parentNode.removeChild(dongXoa);
    CapNhatCotSttChiPhi();
    CapNhatCotSttTS();
  
}
function ChangeGiaTien(element) {
    var dongHienTai = $(element).closest("tr"); //*khi bao dongHienTai, dong element the tr

    if ($(element).prop("readonly")) return;//*neu o element chi doc thi return
    var idTiGia = $("#slbTiGia").val();
    var idNgoaiTeKhac = $("#slbMaNgoaiTeKhac").val();
    var maNgoaiTeKhac = $("#slbMaNgoaiTeKhac option:selected").html();//* chon ma ntk option:selected
    if (idTiGia == "" || idTiGia == GUID_EMPTY) {//* id rong hoac khong gt
        return;
    } else {
        if (element.name == "HopDongNgoaiTeKhac" && idNgoaiTeKhac == GUID_EMPTY) {//*neu name hopdongntk rong va intk gt bang empty
            return;
        }
    }
    var txtBlur = "";//* khai bao txtBlur
    switch (element.name) { //* name trong element
        case "HopDongUSD"://* bnag hopdongusd
            txtBlur = "USD";//*
            break;
        case "HopDongVND":
            txtBlur = "VND";
            break;
        case "HopDongEUR":
            txtBlur = "EUR";
            break;
        case "HopDongNgoaiTeKhac":
            break;
        default:
            break;
    }
    $("input[name=HopDongUSD]").prop("readonly", true);
    $("input[name=HopDongVND]").prop("readonly", true);
    $("input[name=HopDongEUR]").prop("readonly", true);
    $("input[name=HopDongNgoaiTeKhac]").prop("readonly", true);
    $("#btnLuuModal").prop("disabled", true);
    $("#btnHuyModal").prop("disabled", true);

    var giaTriTienData = {};//* khai bao var convert kieu du lieu giaTriTienData = {}object
    giaTriTienData.sGiaTriUSD = UnFormatNumber($(dongHienTai).find("input[name=HopDongUSD]").val()); //gtusd khi nhan tu html chuyen tu dinh dang ve khong dinh dang  de thuc hien tinh toan voi ti gia
    giaTriTienData.sGiaTriVND = UnFormatNumber($(dongHienTai).find("input[name=HopDongVND]").val());
    giaTriTienData.sGiaTriEUR = UnFormatNumber($(dongHienTai).find("input[name=HopDongEUR]").val());
    giaTriTienData.sGiaTriNgoaiTeKhac = UnFormatNumber($(dongHienTai).find("input[name=HopDongNgoaiTeKhac]").val());

    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTinDuAn/ChangeGiaTien",
        async: false,
        data: { idTiGia: idTiGia, idNgoaiTeKhac: idNgoaiTeKhac, maNgoaiTeKhac: maNgoaiTeKhac, txtBlur: txtBlur, giaTriTienData: giaTriTienData },
        success: function (data) {
            $("input[name=HopDongUSD]").prop("readonly", data.isChangeInputUSD);
            $("input[name=HopDongVND]").prop("readonly", data.isChangeInputVND);
            $("input[name=HopDongEUR]").prop("readonly", data.isChangeInputEUR);
            $("input[name=HopDongNgoaiTeKhac]").prop("readonly", data.isChangeInputNgoaiTe);
            $("#btnLuuModal").prop("disabled", false);
            $("#btnHuyModal").prop("disabled", false);
            if (data && data.bIsComplete) {
                if (data.isChangeInputUSD) $(dongHienTai).find("input[name=HopDongUSD]").val(data.sGiaTriUSD).prop("readonly", true);
                if (data.isChangeInputVND) $(dongHienTai).find("input[name=HopDongVND]").val(data.sGiaTriVND).prop("readonly", true);
                if (data.isChangeInputEUR) $(dongHienTai).find("input[name=HopDongEUR]").val(data.sGiaTriEUR).prop("readonly", true);
                if (data.isChangeInputNgoaiTe) $(dongHienTai).find("input[name=HopDongNgoaiTeKhac]").val(data.sGiaTriNTKhac).prop("readonly", true);
            } else {
                var Title = 'Lỗi tính giá trị thông tin hợp đồng';
                var messErr = [];
                messErr.push(data.sMessError);
                $.ajax({
                    type: "POST",
                    url: "/Modal/OpenModal",
                    data: { Title: Title, Messages: messErr, Category: ERROR },
                    success: function (res) {
                        $("#divModalConfirm").html(res);
                    }
                });
            }
        }
    });
    let resultUSD = .00;
    let resultVND = .00;
    let resultEUR = .00;
    let resultNTK = .00;

    var lstNgoaiUSD = GetTableChiTiet();
    if (arrHasValue(lstNgoaiUSD)) {
        lstNgoaiUSD.forEach(x => {

            resultUSD += parseFloat(UnFormatNumber(x.sGiaTriUSD));

            resultVND += parseFloat(UnFormatNumber(x.sGiaTriVND));

            resultEUR += parseFloat(UnFormatNumber(x.sGiaTriEUR));

            resultNTK += parseFloat(UnFormatNumber(x.sGiaTriNgoaiTeKhac));
        });
    }
    $("input[name=tmdt_USD]").val(FormatNumber(resultUSD));
    $("input[name=tmdt_VND]").val(FormatNumber(resultVND));
    $("input[name=tmdt_EUR]").val(FormatNumber(resultEUR));
    $("input[name=tmdt_NTK]").val(FormatNumber(resultNTK));
}
function GetTableChiTiet() {
    var lstNgoaiUSD = [];
    $.each($("#tbListChiphi tbody tr"), function (_index, item) {
        var obj = {};
        obj.sGiaTriUSD = $(item).find("input[name=HopDongUSD]").val();
        obj.sGiaTriVND = $(item).find("input[name=HopDongVND]").val();
        obj.sGiaTriEUR = $(item).find("input[name=HopDongEUR]").val();
        obj.sGiaTriNgoaiTeKhac = $(item).find("input[name=HopDongNgoaiTeKhac]").val();
        lstNgoaiUSD.push(obj);
    });
    return lstNgoaiUSD;
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
function ChangeTiGiaSelect() {
    $("input[name=HopDongUSD]").prop("readonly", true);
    $("input[name=HopDongVND]").prop("readonly", true);
    $("input[name=HopDongEUR]").prop("readonly", true);
    $("input[name=HopDongNgoaiTeKhac]").prop("readonly", true);
    $("#btnLuuModal").prop("disabled", true);
    $("#btnHuyModal").prop("disabled", true);

    var giaTriTienData = {};
    //giaTriTienData.sGiaTriUSD = UnFormatNumber($("input[name=HopDongUSD]").val());
    //giaTriTienData.sGiaTriVND = UnFormatNumber($("input[name=HopDongVND]").val());
    //giaTriTienData.sGiaTriEUR = UnFormatNumber($("input[name=HopDongEUR]").val());
    var idTiGia = $("#slbTiGia").val();
    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTinDuAn/ChangeTiGia",
        data: { idTiGia: idTiGia, giaTriTienData: giaTriTienData },
        success: function (data) {
            $("input[name=HopDongUSD]").prop("readonly", false);
            $("input[name=HopDongVND]").prop("readonly", false);
            $("input[name=HopDongEUR]").prop("readonly", false);
            $("input[name=HopDongNgoaiTeKhac]").prop("readonly", false);
            $("#btnLuuModal").prop("disabled", false);
            $("#btnHuyModal").prop("disabled", false);
            if (data && data.bIsComplete) {
                if (idTiGia != "" && idTiGia != GUID_EMPTY) {
                    $("#slbMaNgoaiTeKhac").empty().html(data.htmlMNTK);
                    if (data.isChangeInputUSD) $("input[name=HopDongUSD]").val(data.sGiaTriUSD).prop("readonly", true);
                    if (data.isChangeInputVND) $("input[name=HopDongVND]").val(data.sGiaTriVND).prop("readonly", true);
                    if (data.isChangeInputEUR) $("input[name=HopDongEUR]").val(data.sGiaTriEUR).prop("readonly", true);
                    $("input[name=HopDongNgoaiTeKhac]").val("").prop("disabled", data.isReadonlyTxtMaNTKhac);
                } else {
                    $("#slbMaNgoaiTeKhac").val(GUID_EMPTY);
                    $("#iDTenNgoaiTeKhac").html("Ngoại tệ khác");
                    $("input[name=HopDongNgoaiTeKhac]").val("").prop("disabled", false);
                }
                $("#tienTeQuyDoiID").html(data.htmlTienTe);
            } else {
                var Title = 'Lỗi tính giá trị thông tin hợp đồng';
                var messErr = [];
                messErr.push(data.sMessError);
                $.ajax({
                    type: "POST",
                    url: "/Modal/OpenModal",
                    data: { Title: Title, Messages: messErr, Category: ERROR },
                    success: function (res) {
                        $("#divModalConfirm").html(res);
                    }
                });
            }
        }
    });
}

function ChangeNgoaiTeKhacSelect() {
    var idTiGia = $("#slbTiGia").val();
    var idNgoaiTeKhac = $("#slbMaNgoaiTeKhac").val();
    var maNgoaiTeKhac = $("#slbMaNgoaiTeKhac option:selected").html();
    if (idNgoaiTeKhac == GUID_EMPTY) {
        $("#iDTenNgoaiTeKhac").html("Ngoại tệ khác");
        if (idTiGia != "" && idTiGia != GUID_EMPTY) {
            if (maTienTe.indexOf($("#slbTiGia option:selected").data("mg")) >= 0) {
                $("input[name=HopDongNgoaiTeKhac]").val("").prop("disabled", true);
            } else {
                $("input[name=HopDongNgoaiTeKhac]").val("").prop("disabled", false);
                $("input[name=HopDongUSD]").val("").prop("readonly", true);
                $("input[name=HopDongVND]").val("").prop("readonly", true);
                $("input[name=HopDongEUR]").val("").prop("readonly", true);
            }
        }
    } else {
        $("#iDTenNgoaiTeKhac").html(maNgoaiTeKhac);
    }
    if (idTiGia == "" || idTiGia == GUID_EMPTY || idNgoaiTeKhac == "" || idNgoaiTeKhac == GUID_EMPTY) {
        return false;
    }

    $("input[name=HopDongNgoaiTeKhac]").prop("readonly", true);
    $("#btnLuuModal").prop("disabled", true);
    $("#btnHuyModal").prop("disabled", true);
    var giaTriTienData = {};

    //giaTriTienData.sGiaTriUSD = UnFormatNumber($("input[name=HopDongUSD]").val());
    //giaTriTienData.sGiaTriVND = UnFormatNumber($("input[name=HopDongVND]").val());
    //giaTriTienData.sGiaTriEUR = UnFormatNumber($("input[name=HopDongEUR]").val());
    //giaTriTienData.sGiaTriNgoaiTeKhac = UnFormatNumber($("input[name=HopDongNgoaiTeKhac]").val());
    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTinDuAn/ChangeTiGiaNgoaiTeKhac",
        data: { idTiGia: idTiGia, idNgoaiTeKhac: idNgoaiTeKhac, maNgoaiTeKhac: maNgoaiTeKhac, giaTriTienData: giaTriTienData },
        success: function (data) {
            $("input[name=HopDongNgoaiTeKhac]").prop("readonly", false);
            $("#btnLuuModal").prop("disabled", false);
            $("#btnHuyModal").prop("disabled", false);
            if (data && data.bIsComplete) {
                if (data.isChangeInputNgoaiTe) $("input[name=HopDongNgoaiTeKhac]").val(data.sGiaTriNTKhac);
                $("input[name=HopDongNgoaiTeKhac]").prop("readonly", data.isReadonlyTxtMaNTKhac);
                if (data.isChangeInputCommon) {
                    $("input[name=HopDongUSD]").val(data.sGiaTriUSD).prop("readonly", true);
                    $("input[name=HopDongVND]").val(data.sGiaTriVND).prop("readonly", true);
                    $("input[name=HopDongEUR]").val(data.sGiaTriEUR).prop("readonly", true);
                }
                $("#tienTeQuyDoiID").empty().html(data.htmlTienTe);
            } else {
                var Title = 'Lỗi tính giá trị ngoại tệ khác thông tin hợp đồng';
                var messErr = [];
                messErr.push(data.sMessError);
                $.ajax({
                    type: "POST",
                    url: "/Modal/OpenModal",
                    data: { Title: Title, Messages: messErr, Category: ERROR },
                    success: function (res) {
                        $("#divModalConfirm").html(res);
                    }
                });
            }
        }
    });
}




