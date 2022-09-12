﻿var CONFIRM = 0;
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;
var TBL_NCCQCT = "tblNhuCauChiQuyChiTiet";
var arrChiQuy = [];
var data = [];
var arrDonvi = [];
var arrBQuanly = [];

$(document).ready(function ($) {
    //var fHanMucDauTu = FormatNumber($("#fHanMucDauTu").val());
    //$("#fHanMucDauTu").val(fHanMucDauTu);
   
    LoadDataHopDong();
    LoadDataBQuanLy();
    LoadDataDonvi();
    ChangeVoucher();
    GetListDataTongHop();
   
});

function ResetChangePage(iCurrentPage = 1) {
    var sSodenghi = "";
    var tabIndex = 0;
    var sNgaydenghi = "";
    var iID_BQuanLyID = GUID_EMPTY;
    var iID_DonViID = GUID_EMPTY;
    var iQuy = 0;
    var iNamKeHoach = null;
    ChangeVoucher();
    if ($('input[name=groupVoucher]:checked').val() == 1) {
        GetListDataTongHop();
    } else {
        GetListData(sSodenghi, sNgaydenghi, iID_BQuanLyID, iID_DonViID, iQuy, iNamKeHoach, tabIndex, iCurrentPage);
    }
    
}



function ChangePage(iCurrentPage = 1) {
    var tabIndex = $('input[name=groupVoucher]:checked').val();
    var sSodenghi = $("#txtSodenghi").val();
    var sNgaydenghi = $("#txtNgaydenghi").val();
    var iID_BQuanLyID = $("#iBQuanly").val();
    var iID_DonViID = $("#iDonvi").val();
    var iQuy = $("#iQuyList").val();
    var iNamKeHoach = $("#txtnam").val();
    ChangeVoucher();
    if ($('input[name=groupVoucher]:checked').val() == 1) {
        GetListDataTongHop();
    } else {
        GetListData(sSodenghi, sNgaydenghi, iID_BQuanLyID, iID_DonViID, iQuy, iNamKeHoach, tabIndex, iCurrentPage);
    }
}

function GetListData(sSodenghi, sNgaydenghi, iID_BQuanLyID, iID_DonViID, iQuy, iNamKeHoach, tabIndex, iCurrentPage) {
    var data = {};
    data.sSodenghi = sSodenghi;
    data.dNgaydenghi = sNgaydenghi;
    data.iID_BQuanLyID = iID_BQuanLyID;
    data.iID_DonViID = iID_DonViID;
    data.iNamKeHoach = iNamKeHoach
    data.iQuy = iQuy;
    _paging.CurrentPage = iCurrentPage;
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/NhuCauChiQuy/NhuCauChiQuySearch",
        data: { _paging: _paging, data : data, tabIndex: tabIndex },
        success: function (data) {
            $("#lstDataView").html(data);
            $("#txtSodenghi").val(sSodenghi);
            $("#txtNgaydenghi").val(sNgaydenghi);
            $("#iBQuanly").val(iID_BQuanLyID);
            $("#iDonvi").val(iID_DonViID);
            $("#iQuyList").val(iQuy);
            $("#txtNam").val(iNamKeHoach);
        }
    });
}

function OpenModalDetail(id) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/NhuCauChiQuy/GetModalDetail",
        data: { id: id },
        success: function (data) {
            $("#contentModalNhuCauChiQuy").html(data);
            $("#modalNhuCauChiQuyLabel").html("Chi tiết nhu cầu chi quý");
        }
    });
}

function OpenModalDetailTongHop(id) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/NhuCauChiQuy/GetModalDetail",
        data: { id: id },
        success: function (data) {
            $("#contentModalNhuCauChiQuy").html(data);
            $("#modalNhuCauChiQuyLabel").html("Chi tiết nhu cầu chi quý");
        }
    });
}

function OpenModal(id, dieuchinh) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/NhuCauChiQuy/GetModal",
        data: { id: id, dieuchinh: dieuchinh},
        success: function (data) {
            $("#contentModalNhuCauChiQuy").html(data);
            if (id == null || id == GUID_EMPTY || id == undefined) {
                $("#modalNhuCauChiQuyLabel").html('Thêm mới nhu cầu chi quý');
            }
            else {
                if (dieuchinh == "true") {
                    $("#modalNhuCauChiQuyLabel").html('Điều chỉnh thông tin nhu cầu chi quý');
                } else {
                    $("#modalNhuCauChiQuyLabel").html('Sửa thông tin nhu cầu chi quý');
                }
            }
        }
    });
}

function ViewInBaoCao(iloai) {
/*    GetSoLuongTo();*/
    $('#rowChonTo').show();
    $('#configBaocao').modal('show');
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/NhuCauChiQuy/GetModalInBaoCao",
        data: {iloai: iloai},
        success: function (data) {
            $("#contentModalNhuCauChiQuy").html(data);
            $("#modalNhuCauChiQuyLabel").html('Báo cáo Kế hoạch Sử dụng kinh phí nguồn Quỹ dự trữ ngoại hối');
        }
    });
}

function GetSoLuongTo() {
    var iNguonNganSach = $("#iID_Nguon").val();
    var dDateFrom = $("#txtTuNgay").val();
    var dDateTo = $("#txtDenNgay").val();
    var dvt = $("#dvt").val();
    var url = '/QLNH/NhuCauChiQuy/Ds_To_BaoCao1/?iNguonNganSach=' + iNguonNganSach +
        '&dDateFrom=' + dDateFrom + '&dDateTo=' + dDateTo + '&dvt=' + dvt;
    fillCheckboxList("To", "To", url);
}

function Delete(id) {
    $.ajax({
        type: "POST",
        url: "/QLNH/NhuCauChiQuy/NhuCauChiQuyPopupDelete",
        data: { id: id },
        success: function (data) {
            $("#contentModalNhuCauChiQuy").html(data);
            if (id == null || id == GUID_EMPTY || id == undefined) {
                $("#modalNhuCauChiQuyLabel").html('Xóa  !');
            }
            else {
                $("#modalNhuCauChiQuyLabel").html('Xác nhận xóa thông tin ');
            }
        }
    });
}

function ChangeVoucher() {
    if ($("input[name=groupVoucher]:checked").val() == "0") {
        $("#tblNhuCauChiQuy").css("display", "");
        $("#NhuCauChiQuy").css("display", "");
        $("#ViewTable").css("display", "none");
        $("#NhuCauChiQuyTongHop").css("display", "none");
        $("#Padding").css("display", "");
        
    } else {
        GetListDataTongHop();
        $("#tblNhuCauChiQuy").css("display", "none");
        $("#ViewTable").css("display", "");
        $("#NhuCauChiQuy").css("display", "none");
        $("#NhuCauChiQuyTongHop").css("display", "");
        $("#Padding").css("display", "none");
    }
}

function DeleteItem(id) {
    $.ajax({
        type: "POST",
        url: "/QLNH/NhuCauChiQuy/NhuCauChiQuyDelete",
        data: { id: id },
        success: function (r) {
            if (r.bIsComplete) {
                window.location.href = "/QLNH/NhuCauChiQuy";
            } else {
                var Title = 'Lỗi lưu nhu cầu chi quý';
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

function LockItem(id, sSoDeNghi, bIsKhoa) {
    var Messages = [];
    var Title = "";
    if (bIsKhoa == "true") {
        Title = 'Xác nhận mở nhu cầu chi quý';
        Messages.push("Bạn có chắc chắn muốn mở nhu cầu chi quý: " + sSoDeNghi);
    } else {
        Title = 'Xác nhận khóa nhu cầu chi quý';
        Messages.push("Bạn có chắc chắn muốn khóa nhu cầu chi quý: " + sSoDeNghi);
    }
    
    var FunctionName = "Lock('" + id + "')";
    $.ajax({
        type: "POST",
        url: "/Modal/OpenModal",
        data: { Title: Title, Messages: Messages, Category: CONFIRM, FunctionName: FunctionName },
        success: function (data) {
            $("#divModalConfirm").html(data);
        }
    });
}

function Lock(id) {
    $.ajax({
        type: "POST",
        url: "/QLNH/NhuCauChiQuy/NhuCauChiQuyLock",
        async: false,
        data: { id: id },
        success: function (r) {
            if (r == "True") {
                ChangePage(1);
            }
        }
    });
}

function Save() {
    var data = {};
    data.id = $("#iID_NhuCauChiQuyModal").val();
    data.sSodenghi = $("#txtSodenghiAdd").val();
    data.dNgaydenghi = $("#txtNgaydenghiAdd").val();
    data.iID_BQuanLyID = $("#iBQuanlyAdd").val();
    data.iID_DonViID = $("#iDonviAdd").val();
    data.iNamKeHoach = $("#txtnamAdd").val();
    data.iQuy = $("#iQuyListAdd").val();
    data.iID_TiGiaID = $("#iTygiaAdd").val();
    data.sMaNgoaiTeKhac = $("#iMaNgoaitekhacAdd").val();
    var dieuchinh = $("#isdieuchinh").val();
    if (!ValidateData(data)) {
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/QLNH/NhuCauChiQuy/NhuCauChiQuySave",
        data: { data: data, dieuchinh: dieuchinh },
        success: function (datainsert) {
            if (datainsert.iTrangthai == 0) {
                window.location.href = "/QLNH/NhuCauChiQuy/UpdateChitiet?id=" + datainsert.ID;
            } else if (datainsert.dieuchinh == "1") {
                window.location.href = "/QLNH/NhuCauChiQuy/SuaChitiet?id=" + datainsert.ID + "&isDieuchinh=1";
            } else {
                window.location.href = "/QLNH/NhuCauChiQuy/SuaChitiet?id=" + datainsert.ID + "&isDieuchinh=0";
            }
           
        }
    });
}

function SaveGeneral() {
    var data = {};
    data.id = $("#iID_NhuCauChiQuyModalTongHop").val();
    data.sSodenghi = $("#txtSodenghiAddTongHop").val();
    data.dNgaydenghi = $("#txtNgaydenghiAddTongHop").val();
    data.iID_BQuanLyID = $("#iBQuanlyAddTongHop").val();
    data.iNamKeHoach = $("#txtnamAddTongHop").val();
    data.iQuy = $("#iQuyListAddTongHop").val();

    var isModified = $('#txtDxModified').val();
    var isAggregate = $('#txtDxAggregate').val();
    //var bIsDetail = $('#txtDetail').val()

    if (!ValidateDataTongHop(data)) {
        return false;
    }
    var lstDuAnChecked = JSON.parse(sessionStorage.getItem('dataChecked'));

    $.ajax({
        type: "POST",
        url: "/QLNH/NhuCauChiQuy/_modeGeneralSave",
        data: { data: data, lstItem: lstDuAnChecked  },
        success: function (datainsert, iTrangthai) {
            window.location.href = "/QLNH/NhuCauChiQuy"
        }
    });
}

function xem() {
    ID = $("#iID_NhuCauChiQuyModal").val();
    if (ID != null) {
        window.location.href = "/QLNH/NhuCauChiQuy/XemChitiet?id=" + ID;
    }
}

function UpdateChitiet(data) {
    $.ajax({
        type: "POST",
        url: "/QLNH/NhuCauChiQuy/UpdateChitiet",
        data: { data: data },
        success: function (data) {
            if (id == null || id == GUID_EMPTY || id == undefined) {
                $("#modalNhuCauChiQuyLabel").html('Lỗi  !');
            }
            else {
                $("#modalNhuCauChiQuyLabel").html('Nhu cầu chi quý chi tiết ');
            }
        }
    });
}

function isStringEmpty(x) {
    if (x == null || x == undefined || x == "") {
        return true;
    }

    return false;
}

function arrHasValue(x) {
    if (x == null || x == undefined || x.length <= 0) {
        return false;
    }

    return true;
}

function Themnoidungchi() {
    var dongMoi = "";
    dongMoi += "<tr style='cursor: pointer;' class='parent'>";
    dongMoi += "<td class='r_STT'></td><input type='hidden' value=''/>";
    dongMoi += "<td class='r_Noidung' align='right'><div class='sNoidung' hidden></div><input type='text' class='form-control txtNoidung'/></td>";
    dongMoi += "<td class='r_ChingoaiteUSD' align='right'><div class='fChingoaiteUSD' hidden></div><input type='text' onblur='TinhLaiDongTong(this)' class='form-control txtChingoaiteUSD' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);'/></td>";
    dongMoi += "<td class='text-right txtChingoaiteVND'></td>";
    dongMoi += "<td class='r_ChitrongnuocVND' align='right'><div class='fChitrongnuocVND' hidden></div><input type='text' onblur='TinhLaiDongTong(this)' class='form-control txtChiTrongnuocVND' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);'/></td>";
    dongMoi += "<td class='text-right txtChiTrongnuocUSD'></td>";
    dongMoi += "<td><button class='btn-delete btn-icon' type='button' onclick='XoaDong(this, \"" + TBL_NCCQCT + "\")'>" +
        "<span class='fa fa-trash-o fa-lg' aria-hidden='true'></span>" +
        "</button></td>";
    dongMoi += "</tr>";

    $("#" + TBL_NCCQCT + " tbody").append(dongMoi);
    CapNhatCotSttChiQuy(TBL_NCCQCT);
}

function Themhopdong() {
    var dongMoi = "";
    dongMoi += "<tr style='cursor: pointer;' class='parent'>";
    dongMoi += "<td class='r_STT'></td><input type='hidden' class='' value=''/>";
    dongMoi += "<td><input type='hidden' class='r_iID_HopDong' /><div class='sTenHopDong' hidden></div><div class='selectHopDong'>" + CreateHtmlSelectChiquy() + "</div></td>";
    dongMoi += "<td class='r_ChingoaiteUSD' align='right'><div class='fChingoaiteUSD' hidden></div><input type='text' onblur='TinhLaiDongTong(this)' class='form-control txtChingoaiteUSD' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);'/></td>";
    dongMoi += "<td class='text-right txtChingoaiteVND'></td>";
    dongMoi += "<td class='r_ChitrongnuocVND' align='right'><div class='fChitrongnuocVND' hidden></div><input type='text' onblur='TinhLaiDongTong(this)' class='form-control txtChiTrongnuocVND' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);'/></td>";
    dongMoi += "<td class='text-right txtChiTrongnuocUSD'></td>";
    //dongMoi += "<button class='btn-save btn-icon' type = 'button' onclick = 'Luu(this, \"" + TBL_NCCQCT + "\")' > " +
    //    "<i class='fa fa-floppy-o fa-lg' aria-hidden='true'></i>" +
    //    "</button> ";
    //dongMoi += "<button class='btn-edit btn-icon' hidden type = 'button' onclick = 'Sua(this, \"" + TBL_NCCQCT + "\")' > " +
    //    "<i class='fa fa-pencil-square-o fa-lg' aria-hidden='true'></i>" +
    //    "</button> ";
    dongMoi += "<td><button class='btn-delete btn-icon' type='button' onclick='XoaDong(this, \"" + TBL_NCCQCT + "\")'>" +
        "<span class='fa fa-trash-o fa-lg' aria-hidden='true'></span>" +
        "</button></td>";
    dongMoi += "</tr>";

    $("#" + TBL_NCCQCT + " tbody").append(dongMoi);
    CapNhatCotSttChiQuy(TBL_NCCQCT);
}

function CapNhatCotSttChiQuy(idBang) {
    $("#" + idBang + " tbody tr").each(function (index, tr) {
        $(tr).find('.r_STT').text(index + 1);
    });
}

var arr_DataHopDong = [];
function LoadDataHopDong() {
    $.ajax({
        url: "/QLNH/NhuCauChiQuy/GetHopDongAll",
        type: "POST",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.status == false) {
                return;
            }

            if (data.data != null)
                arr_DataHopDong = data.data;
        }
    });
}

function CreateHtmlSelectChiquy(value) {
    var htmlOption = "";
    arr_DataHopDong.forEach(x => {
        if (value != undefined && value == x.id)
            htmlOption += "<option value='" + x.id + "' selected>" + x.text + "</option>";
        else
            htmlOption += "<option value='" + x.id + "'>" + x.text + "</option>";
    })
    return "<select class='form-control'>" + htmlOption + "</option>";
}



function LoadDataBQuanLy() {
    $.ajax({
        url: "/QLNH/NhuCauChiQuy/GetListBQuanLy",
        type: "POST",
        dataType: "json",
        cache: false,
        async: false,
        success: function (data) {
            if (data.status == false) {
                return;
            }

            if (data.data != null)
                arrBQuanly = data.data;
        }
    });
}
function CreateHtmlSelectBQuanLy(value) {
    var htmlOption = "";
    arrBQuanly.forEach(x => {
        if (value != undefined && value == x.id)
            htmlOption += "<option value='" + x.id + "' selected>" + x.text + "</option>";
        else
            htmlOption += "<option value='" + x.id + "'>" + x.text + "</option>";
    })
    return "<select class='form-control'>" + htmlOption + "</option>";
}

function LoadDataDonvi() {
    $.ajax({
        async: false,
        url: "/QLNH/NhuCauChiQuy/GetListDonvi",
        type: "POST",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.status == false) {
                return;
            }

            if (data.data != null)
                arrDonvi = data.data;
        }
    });
}
function CreateHtmlSelectDonVi(value) {
    var htmlOption = "";
    arrDonvi.forEach(x => {
        if (value != undefined && value == x.id)
            htmlOption += "<option value='" + x.id + "' selected>" + x.text + "</option>";
        else
            htmlOption += "<option value='" + x.id + "'>" + x.text + "</option>";
    })
    return "<select class='form-control'>" + htmlOption + "</option>";
}
function CreateHtmlSelectQuy() {
    var htmlOption = "<option value='4' selected>Quý 4</option><option value='3' selected>Quý 3</option><option value='2' selected>Quý 2</option><option value='1' selected>Quý 1</option><option value='0' selected>-- Tất cả --</option>";
    return "<select class='form-control'>" + htmlOption + "</select>";
}

function XoaDong(nutXoa, idBang) {
    var dongXoa = nutXoa.parentElement.parentElement;
    dongXoa.parentNode.removeChild(dongXoa);
    CapNhatCotSttChiQuy(idBang);
    TinhLaiDongTong(idBang);
}

function TinhLaiDongTong(idDong) {
    var sTigia = $("#fTiGia").val();
    var fTigia = sTigia == "" ? 0 : parseInt(UnFormatNumber(sTigia));
    var dongHienTai = $(idDong).closest("tr");

    var fChiNgoaiteUSD = 0;
    var sChiNgoaiteUSD = $(dongHienTai).find(".txtChingoaiteUSD").val();
    if (sChiNgoaiteUSD != "" && sChiNgoaiteUSD != null) {
        fChiNgoaiteUSD = parseInt(UnFormatNumber(sChiNgoaiteUSD));
    }
    var fChingoaiteVND = fChiNgoaiteUSD * fTigia;
    $(dongHienTai).find(".txtChingoaiteVND").html(FormatNumber(fChingoaiteVND));

    var sChiTrongnuocVND = $(dongHienTai).find(".txtChiTrongnuocVND").val();
    var fChiTrongnuocVND = 0;
    if (sChiTrongnuocVND != "" && sChiTrongnuocVND != null) {
        fChiTrongnuocVND = parseInt(UnFormatNumber(sChiTrongnuocVND));
    }
    var fChitrongnuocUSD = fChiTrongnuocVND / fTigia;
    $(dongHienTai).find(".txtChiTrongnuocUSD").html(FormatNumber(fChitrongnuocUSD.toFixed(2)));
    var resultNgoaiUSD = 0;
    var resultNgoaiVND = 0;
    var resultTrongVND = 0;
    var resultTrongUSD = 0;
    var lstNgoaiUSD = GetNgoaiUSDTable();
    if (arrHasValue(lstNgoaiUSD)) {
        lstNgoaiUSD.forEach(x => {
            if ((x.isDelete == undefined || x.isDelete == false) && !isStringEmpty(x.fChiNgoaiTeUSD)) {
                resultNgoaiUSD += parseFloat(x.fChiNgoaiTeUSD);
                //$("#fTMDTDuKienPheDuyet").val(FormatNumber(result));
            }

            if ((x.isDelete == undefined || x.isDelete == false) && !isStringEmpty(x.fChiTrongNuocVND)) {
                resultTrongVND += parseFloat(x.fChiTrongNuocVND);
                //$("#fTMDTDuKienPheDuyet").val(FormatNumber(result));
            }
        });
    }
    $(".cpdt_tong_ngoaiUSD").html(FormatNumber(resultNgoaiUSD));
    $(".cpdt_tong_ngoaiVND").html(FormatNumber(resultNgoaiUSD * fTigia));

    $(".cpdt_tong_trongVND").html(FormatNumber(resultTrongVND));
    $(".cpdt_tong_trongUSD").html(FormatNumber((resultTrongVND / fTigia).toFixed(2)));
}

function GetNgoaiUSDTable() {
    var lstNgoaiUSD = [];
    var sTigia = $("#fTiGia").val();
    var fTigia = sTigia == "" ? 0 : parseInt(UnFormatNumber(sTigia));
    $.each($("#tblNhuCauChiQuyChiTiet tbody tr"), function (index, item) {
        var obj = {};
        var bIsDelete = $(this).hasClass("error-row");
        var sID_NhuCauChiQuyID = $("#iID_NhuCauChiQuyID").val();
        obj.iID_NhuCauChiQuyID = sID_NhuCauChiQuyID;
        obj.IDDs = $(item).find(".r_STT").html();
        obj.fChiNgoaiTeUSD = $(item).find(".txtChingoaiteUSD").val();
        obj.fChiTrongNuocVND = $(item).find(".txtChiTrongnuocVND").val();
        obj.isDelete = bIsDelete;
        obj.iID_HopDongID = $(item).find(".selectHopDong select").val();
        obj.sNoidung = $(item).find(".txtNoidung").val();
        if (obj.fChiNgoaiTeUSD == null || obj.fChiNgoaiTeUSD == "") {
            obj.fChiNgoaiTeUSD = 0;
        } else {
            obj.fChiNgoaiTeUSD = parseFloat(obj.fChiNgoaiTeUSD.toString().replaceAll(".", ""));
            obj.fChiNgoaiTeVND = obj.fChiNgoaiTeUSD * fTigia;
            obj.sChiNgoaiTeUSD = obj.fChiNgoaiTeUSD.toString();
            obj.sChiNgoaiTeVND = obj.fChiNgoaiTeVND.toString();
        }


        if (obj.fChiTrongNuocVND == null || obj.fChiTrongNuocVND == "") {
            obj.fChiTrongNuocVND = 0;
        } else {
            obj.fChiTrongNuocVND = parseFloat(obj.fChiTrongNuocVND.toString().replaceAll(".", ""));
            obj.fChiTrongNuocUSD = parseFloat((obj.fChiTrongNuocVND / fTigia).toFixed(2));
            obj.sChiTrongNuocVND = obj.fChiTrongNuocVND.toString();
            obj.sChiTrongNuocUSD = obj.fChiTrongNuocUSD.toString();
        }
        lstNgoaiUSD.push(obj);
    });
    return lstNgoaiUSD;
}

function GetDataBeforeSave() {
    var resultNgoaiUSD = 0;
    var resultTrongVND = 0;
    var sTigia = $("#fTiGia").val();
    var fTigia = sTigia == "" ? 0 : parseInt(UnFormatNumber(sTigia));
    var data = {};
    var lstNgoaiUSD = data.ListNCCQChiTiet = GetNgoaiUSDTable();

    if (arrHasValue(lstNgoaiUSD)) {
        lstNgoaiUSD.forEach(x => {
            if ((x.isDelete == undefined || x.isDelete == false) && !isStringEmpty(x.fChiNgoaiTeUSD)) {
                resultNgoaiUSD += parseFloat(x.fChiNgoaiTeUSD);
                //$("#fTMDTDuKienPheDuyet").val(FormatNumber(result));
            }

            if ((x.isDelete == undefined || x.isDelete == false) && !isStringEmpty(x.fChiTrongNuocVND)) {
                resultTrongVND += parseFloat(x.fChiTrongNuocVND);
                //$("#fTMDTDuKienPheDuyet").val(FormatNumber(result));
            }
        });
    }
    data.sTongChiNgoaiTeUSD = resultNgoaiUSD.toString();
    data.sTongChiNgoaiTeVND = (resultNgoaiUSD * fTigia).toString();
    data.sTongChiTrongNuocVND = resultTrongVND.toString();
    data.sTongChiTrongNuocUSD = ((resultTrongVND / fTigia).toFixed(2)).toString();
    return data;
}

function ValidateBeforeSave(data) {
    var message = [];
    var title = 'Lỗi lưu nhu cầu chi quý chi tiết';
    var ListNCCQChiTiet = data.ListNCCQChiTiet;
    if (ListNCCQChiTiet.length == 0) {
        message.push("Vui lòng thêm nội dung chi hoặc hợp đồng.");
    }
    for (var i = 0; i < ListNCCQChiTiet.length; i++) {
        if ((ListNCCQChiTiet[i].iID_HopDongID == null || ListNCCQChiTiet[i].iID_HopDongID == "") && (ListNCCQChiTiet[i].sNoidung == null || ListNCCQChiTiet[i].sNoidung == ""))  {
            message.push("Vui lòng chọn hợp đồng hoặc nhập nội dung chi.");
        }
        if (ListNCCQChiTiet[i].fChiTrongNuocVND == null || ListNCCQChiTiet[i].fChiTrongNuocVND == "") {
            message.push("Vui lòng nhập số chi trong nước.");
        }

        if (ListNCCQChiTiet[i].fChiNgoaiTeUSD == null || ListNCCQChiTiet[i].fChiNgoaiTeUSD == "") {
            message.push("Vui lòng nhập số chi ngoại tệ.");
        }
    }

    if (data.sTongChiTrongNuocVND == null || data.sTongChiTrongNuocVND == "") {
        message.push("Vui lòng nhập số chi trong nước.");
    }

    if (data.sTongChiNgoaiTeUSD == null || data.sTongChiNgoaiTeUSD == "") {
        message.push("Vui lòng nhập số chi ngoại tệ.");
    }

    if (message.length > 0) {
        popupModal(title, message, ERROR);
        return false;
    }

    return true;
}

function Luu() {
    var data = GetDataBeforeSave();
    if (!ValidateBeforeSave(data)) {
        return false;
    }
    $.ajax({
        url: "/QLNH/NhuCauChiQuy/LuuNhuCauChiQuyChiTiet",
        type: "POST",
        data: { data: data },
        dataType: "json",
        cache: false,
        success: function (resp) {
            if (resp == null || resp.status == false) {
                popupModal("Lỗi lưu dữ liệu nhu cầu chi quý chi tiết", [resp.message], ERROR);
                return;
            }
            else {
                bIsSaveSuccess = true;
                $.ajax({
                    type: "POST",
                    url: "/Modal/OpenModal",
                    data: { Title: "Thông báo", Messages: "Lưu dữ liệu thành công", Category: ERROR },
                    success: function (data) {
                        $("#divModalConfirm").html(data);
                        $('#confirmModal').on('hidden.bs.modal', function () {
                            window.location.href = "/QLNH/NhuCauChiQuy/SuaChitiet?id=" + resp.id + "&isDieuchinh=" + resp.dieuchinh;
                        });
                    }
                });
            }
        },
        error: function (data) {

        }
    })

}

function Huy() {
    window.location.href = "/QLNH/NhuCauChiQuy";
}

function ValidateData(data) {
    var Title = 'Lỗi thêm mới/chỉnh sửa nhu cầu chi quý';
    var Messages = [];

    if (data.sSodenghi == null || data.sSodenghi == "") {
        Messages.push("Chưa nhập số đề nghị");
    }

    if (data.iID_BQuanLyID == null || data.iID_BQuanLyID == GUID_EMPTY) {
        Messages.push("Chưa chọn ban quản lý !");
    }
    if (data.iID_DonViID == null || data.iID_DonViID == GUID_EMPTY) {
        Messages.push("Chưa chọn đơn vị quản lý !");
    }
    if (data.iNamKeHoach == null || data.iNamKeHoach == "") {
        Messages.push("Chưa chọn năm !");
    }
    if (data.iID_TiGiaID == null || data.iID_TiGiaID == GUID_EMPTY) {
        Messages.push("Chưa chọn tỷ giá !");
    }

    //if (CheckExistMaChuDauTu(data.sId_CDT)) {
    //    Messages.push("Đã tồn tại mã nhà thầu !");
    //}

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

function ValidateDataTongHop(data) {
    var Title = 'Lỗi thêm mới/chỉnh sửa nhu cầu chi quý';
    var Messages = [];

    if (data.sSodenghi == null || data.sSodenghi == "") {
        Messages.push("Chưa nhập số đề nghị");
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

function popupModal(title, message, category) {
    $.ajax({
        type: "POST",
        url: "/Modal/OpenModal",
        data: { Title: title, Messages: message, Category: category },
        success: function (data) {
            $("#divModalConfirm").html(data);
        }
    });
}

function popupModalSave(title, message, category) {
    $.ajax({
        type: "POST",
        url: "/Modal/OpenModal",
        data: { Title: title, Messages: message, Category: category },
        success: function (data) {
            $("#divModalConfirm").html(data);
        }
    });
}


SetState = (key) => {
    var idItem = "." + key + ":checked";
    var elementValue = $(idItem).val();

    var itemValue = {
        isChecked: false,
        ID: null
    };
    itemValue.isChecked = (elementValue == "on") ? true : false;
    itemValue.ID = key;
    data.push(itemValue);
    sessionStorage.setItem('dataChecked', JSON.stringify(data));
}


function OpenModalGeneral(id, isModified, isAggregate) {
    var lstDataChecked = JSON.parse(sessionStorage.getItem('dataChecked'));
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/NhuCauChiQuy/UpdateGeneral",
        data: { id: id, isAggregate: isAggregate, lstItem: lstDataChecked },
        success: function (data) {
            if (isAggregate == "true") {
                var jsonData = null;
                try {
                    jsonData = JSON.parse(data);
                }
                catch (error) {
                    jsonData = null;
                }
                if (jsonData != null && jsonData.bIsComplete == false) {
                    $('#modalNhuCauChiQuy').modal('hide');
                    var Title = 'Lỗi tổng hợp nhu cầu chi quý.';
                    var messErr = [];
                    messErr.push(jsonData.sMessError);
                    $.ajax({
                        type: "POST",
                        url: "/Modal/OpenModal",
                        data: { Title: Title, Messages: messErr, Category: ERROR },
                        success: function (data) {
                            $("#divModalConfirm").html(data);

                        }
                    });
                    return;
                }
            }
            $("#contentModalNhuCauChiQuy").html(data);
            $("#txtDxModified").val(isModified);
            $("#txtDxAggregate").val(isAggregate);
            if (id == null || id == GUID_EMPTY || id == undefined) {
                $("#modalNhuCauChiQuyLabel").html('Tổng hợp nhu cầu chi quý');
            }
            else {
                $("#modalNhuCauChiQuyLabel").html('Sửa tổng hợp nhu cầu chi quý');
            }
            $(".date").datepicker({
                todayBtn: "linked",
                language: "vi",
                autoclose: true,
                todayHighlight: true,
                format: 'dd/mm/yyyy'
            });
            $('#modalNhuCauChiQuy').modal('show');
        }
    });
}
var isShowSearchDMLoaiCongTrinh = true;
function GetListDataTongHop() {
    var tabIndex = $('input[name=groupVoucher]:checked').val();
    var sSodenghi = $("#txtSodenghi").val();
    var sNgaydenghi = $("#txtNgaydenghi").val();
    var iID_BQuanLyID = $("#iBQuanly").val();
    var iID_DonViID = $("#iDonvi").val();
    var iQuy = $("#iQuyList").val();
    var iNamKeHoach = $("#txtnam").val();
    var data = {};
    $.ajax({
        type: "POST",
        url: "/QLNH/NhuCauChiQuy/GetListNhuCauChiQuy",
        data: { tabIndex: tabIndex },
        success: function (r) {
            var columns = [
                { sField: "ID", bKey: true },
                { sField: "iID_TongHopID", bParentKey: true },
                //{ sTitle: "STT", sField: "sSTT", iWidth: "3%", sTextAlign: "center" },
                { sTitle: "Số đề nghị", sField: "sSoDeNghi", iWidth: "16%", sTextAlign: "left", bHaveIcon: 1    },
                { sTitle: "Ngày đề nghị", sField: "dNgayDeNghiStr", iWidth: "10%", sTextAlign: "center" },
                { sTitle: "B quản lý", sField: "BPhongBan", iWidth: "13%", sTextAlign: "left"},
                { sTitle: "Đơn vị", sField: "BQuanLy", iWidth: "13%", sTextAlign: "left" },
                { sTitle: "Quý/Năm", sField: "sQuyNam", iWidth: "15%", sTextAlign: "left" },
                { sTitle: "Chi ngoại tệ (USD)", sField: "sTongChiNgoaiTeUSD_TongHop", iWidth: "8%", sTextAlign: "right" },
                { sTitle: "Chi trong nước (VND)", sField: "sTongChiTrongNuocVND_TongHop", iWidth: "8%", sTextAlign: "right" },
            ];
            var button = { bUpdate: 1, bDelete: 1, bInfo: 1 };
            var sortedData = r.data.sort((a, b) => {
                if (a.ID < b.ID) {
                    return -1;
                }
                else return 1;
            })
            var sHtml = GenerateTreeTableNCCQ(sortedData, columns, button, true, false, isShowSearchDMLoaiCongTrinh)
            $("#txtSobannghi").text(r.data.length);
            $("#ViewTable").html(sHtml);
            $('.date')
                .datepicker({
                    todayBtn: "linked",
                    keyboardNavigation: false,
                    forceParse: false,
                    autoclose: true,
                    language: 'vi',
                    todayHighlight: true,
                });
        }
    });
   
}

function GetListData_By_Name() {
    var sSodenghi = $("#txtSodenghiList").val();
    var dNgaydenghi = $("#txtNgaydenghiList").val();
    var iBQuanly = $(".selectBQuanLyList select").val();
    var iDonvi = $(".selectDonViList select").val();
    var iQuy = $(".selectiQuyList select").val();
    var iNam = $("#txtNamList").val();
    if (sSodenghi == "" && dNgaydenghi == "" && iBQuanly == "" && iDonvi == "" && iQuy == "" && iNam == "") {
        GetListData();
    } else {
        $.ajax({
            type: "POST",
            url: "/QLNH/NhuCauChiQuy/GetListNhuCauChiQuyByName",
            data: { sSodenghi, dNgaydenghi, iBQuanly, iDonvi, iQuy, iNam },
            success: function (r) {
                var columns = [
                    { sField: "ID", bKey: true },
                    { sField: "iID_TongHopID", bParentKey: true },
                    /*{ sTitle: "STT", sField: "sSTT", iWidth: "3%", sTextAlign: "center" },*/
                    { sTitle: "Số đề nghị", sField: "sSoDeNghi", iWidth: "16%", sTextAlign: "left", bHaveIcon: 1 },
                    { sTitle: "Ngày đề nghị", sField: "dNgayDeNghiStr", iWidth: "10%", sTextAlign: "center" },
                    { sTitle: "B quản lý", sField: "BPhongBan", iWidth: "13%", sTextAlign: "left" },
                    { sTitle: "Đơn vị", sField: "BQuanLy", iWidth: "13%", sTextAlign: "left" },
                    { sTitle: "Quý/Năm", sField: "sQuyNam", iWidth: "15%", sTextAlign: "left" },
                    { sTitle: "Chi ngoại tệ (USD)", sField: "fTongChiNgoaiTeUSD", iWidth: "8%", sTextAlign: "right" },
                    { sTitle: "Chi trong nước (VND)", sField: "fTongChiTrongNuocVND", iWidth: "8%", sTextAlign: "right" },
                ];
                var button = { bUpdate: 1, bDelete: 1, bInfo: 1 };
                var sortedData = r.data.sort((a, b) => {
                    if (a.ID < b.ID) {
                        return -1;
                    }
                    else return 1;
                })
                var sHtml = GenerateTreeTableNCCQ(r.data, columns, button, true, true, isShowSearchDMLoaiCongTrinh)
                $("#ViewTable").html(sHtml);
                $('.date')
                    .datepicker({
                        todayBtn: "linked",
                        keyboardNavigation: false,
                        forceParse: false,
                        autoclose: true,
                        language: 'vi',
                        todayHighlight: true,
                    });
                //Keep value search
                $("#txtSodenghiList").val(sSodenghi);
                $("#txtNgaydenghiList").val(dNgaydenghi);
                $(".selectBQuanLyList select").val(iBQuanly);
                $(".selectDonViList select").val(iDonvi);
                $(".selectiQuyList select").val(iQuy);
                $("#txtNamList").val(iNam);
            }
        });
    }
}


