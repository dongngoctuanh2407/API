﻿var guidEmpty = "00000000-0000-0000-0000-000000000000";
var bIsDieuChinh = false;
var sCbxPhuongThuc = "";
var sCbxHinhThuc = "";
var sCbxLoaiHD = "";
var iTypeDuToan = $("#iTypeDuToan").val();
var lstChungTuAll = [];
var itemsChungTu = [];
var itemsChungTuGoiThau = [[],[],[]];
var lstChungTuGoiThau = []; // sử dụng để update cho tất cả các gói thầu, biến dùng chung

var hrefSplit = window.location.href.split('/');
var iIdKHLuaChonNhaThau = hrefSplit[hrefSplit.length - 1];
var arrGoiThau = [];
var arrGoiThauNguonVon = [];
var arrGoiThauChiPhi = [];
var arrGoiThauHangMuc = [];
var arrDuAnChiPhiGoiThau = [];


$(document).ready(function () {
    if ($("#bIsDieuChinh").val() == 1)
        bIsDieuChinh = true;
    GetCbxDonVi();
    GetCbxGoiThau();
    SetupItem();
    GetChungTuDetailByKhlcnt();
    GetAllGoiThauByKhlcntId();
});

function GetDanhSachGoiThau() {
    $.ajax({
        url: "/QLVonDauTu/KHLuaChonNhaThau/LayThongTinGoiThauChiTietTheoKHLuaChonNhaThau",
        type: "POST",
        data: { id: iIdKHLuaChonNhaThau },
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data != null) {
                // fill thong tin chi tiet du an
                if (data != null) {
                    arrGoiThau = data.lstGoiThau;
                    arrGoiThauNguonVon = data.lstGoiThauNguonVon;
                    arrGoiThauChiPhi = data.lstGoiThauChiPhi;
                    arrGoiThauHangMuc = data.lstGoiThauHangMuc;
                    arrDuAnChiPhiGoiThau = data.lstDuAnChiPhis;
                    // merge arrDuAnChiPhiGoiThau to arrGoiThauChiPhi
                    arrGoiThauChiPhi = arrGoiThauChiPhi.map(item => {
                        var chiPhiDuAn =
                            arrDuAnChiPhiGoiThau.filter(ele => ele.iID_DuAn_ChiPhi === item.iID_ChiPhiID)[0];
                        if (chiPhiDuAn) {
                            return {
                                ...item,
                                ...chiPhiDuAn
                            }
                        } else return item;
                    })
                    console.log(arrGoiThauChiPhi)
                }
            }
            arrGoiThau.forEach(gt => {
                lstChungTuGoiThau[gt.iID_GoiThauID] = [[],[],[]];
                GetNguonVonGoiThauDetail(gt.iID_GoiThauID);
                GetChiPhiGoiThauDetail(gt.iID_GoiThauID);
                // calculate sum nguồn vốn
                SetLstNguonVon();
                GetTongChiTiet();
                // update lại array chung cho tất cả gói thầu
                lstChungTuGoiThau[gt.iID_GoiThauID] = [...itemsChungTuGoiThau];
            })
            arrGoiThauNguonVon = [];
            arrGoiThauChiPhi = [];
            arrGoiThauHangMuc = [];
        },
        error: function (data) {
            GetNguonVonGoiThauDetail();
            GetChiPhiGoiThauDetail();
        }
    });
}

function GetListChungTu() {
    var iIdDuAnId = $("#iID_DuAnID").val();
    var iLoaiChungTu = $("#cbxLoaiChungTu").val();
    $("#lstChungTu").empty();
    if (iIdDuAnId == null || iIdDuAnId == "") return;

    $.ajax({
        url: "/QLVonDauTu/KHLuaChonNhaThau/GetListChungTu",
        type: "GET",
        dataType: "html",
        data: { iIdDuAnId: iIdDuAnId, iLoaiChungTu: iLoaiChungTu },
        success: function (result) {
            if (result != null && result != "") {
                $("#lstChungTu").html(result);
                $('input:radio[name="rd_ChungTu"]').change(function () {
                    var iIdChungTu = $(this).val();
                    ConvertItemsChungTu($.map(lstChungTuAll, function (n) { return n.iID_ChungTu == iIdChungTu ? n : null }));
                });
                
                if (iLoaiChungTu == iTypeDuToan) {
                    // trường hợp thêm mới
                    if ($("#iIdDuToanId").val() == '') {
                        $("#iIdDuToanId").val($("input[name=rd_ChungTu]:checked").val());
                    }
                    // trường hợp update
                    $("input[name=rd_ChungTu]:checked").val($("#iIdDuToanId").val());                    
                } else {
                    // trường hợp thêm mới
                    if ($("#iIdQDDauTuId").val() == '') {
                        $("#iIdQDDauTuId").val($("input[name=rd_ChungTu]:checked").val());
                    }
                    // trường hợp update
                    $("input[name=rd_ChungTu]:checked").val($("#iIdQDDauTuId").val());
                }
                var lstChungTu = [];
                $.each($("input[name=rd_ChungTu]:checked"), function (index, item) {
                    lstChungTu.push($(item).val());
                });
                GetAllChungTuDetail(lstChungTu);
            }
        }
    });
}

function GetAllChungTuDetail(lstChungTu) {
    var iLoaiChungTu = $("#cbxLoaiChungTu").val();
    $("#tblDanhSachNguonVon tbody").empty();
    $("#tblDanhSachChiPhi tbody").empty();
    var lstAllNguonVon = [];
    var lstAllChiPhi = [];
    lstChungTuAll = [];
    $.ajax({
        url: "/QLVonDauTu/KHLuaChonNhaThau/GetAllChungTuDetail",
        type: "POST",
        dataType: "json",
        data: { lstChungTu: lstChungTu, iLoaiChungTu: iLoaiChungTu },
        success: function (result) {
            if (result == null || result.data == null || result.data.length == 0) return;
            lstChungTuAll = result.data;
            // test
            var iIdChungTu = $('input:radio[name="rd_ChungTu"]:checked').val();
            ConvertItemsChungTu($.map(lstChungTuAll, function (n) { return n.iID_ChungTu == iIdChungTu ? n : null }));
            // end test
            result.data.forEach(function (item) {
                if (item.iID_NguonVonID != 0) {
                    if (lstAllNguonVon[item.iID_NguonVonID] == null)
                        lstAllNguonVon[item.iID_NguonVonID] = item;
                    else
                        lstAllNguonVon[item.iID_NguonVonID].fGiaTriPheDuyet += item.fGiaTriPheDuyet;
                }
                else if (item.iID_ChiPhiID != null && item.iID_HangMucID == null) {
                    if (lstAllChiPhi[item.iID_ChiPhiID] == null)
                        lstAllChiPhi[item.iID_ChiPhiID] = item;
                    else
                        lstAllChiPhi[item.iID_ChiPhiID].fGiaTriPheDuyet += item.fGiaTriPheDuyet;
                }
            });
            if (lstAllNguonVon.length != 0) RenderNguonVonAll("tblDanhSachNguonVon", lstAllNguonVon);
            if (lstAllChiPhi != null && lstAllChiPhi != []) RenderNguonVonAll("tblDanhSachChiPhi", lstAllChiPhi);
            GetDanhSachGoiThau();
        }
    });
}

function RenderNguonVonAll(iIdTable, Items) {
    var sItem = [];
    Object.keys(Items).forEach(function (key) {
        sItem.push("<tr>");
        sItem.push("<td>" + Items[key].sNoiDung + "</td>");
        sItem.push("<td class='text-right'>" + FormatNumber(Items[key].fGiaTriPheDuyet) + "</td>");
        sItem.push("</tr>");
    });
    $("#" + iIdTable + " tbody").html(sItem.join(""));
}

function GetChungTuDetailByKhlcnt() {
    var id = $("#iIdKeHoachLuaChonNhaThau").val();
    if (id == "" || id == guidEmpty) return;
    $.ajax({
        url: "/QLVonDauTu/KHLuaChonNhaThau/GetChungTuDetailByKhlcntId",
        type: "GET",
        dataType: "json",
        data: { id: id },
        success: function (result) {
        }
    });
}

function GetAllGoiThauByKhlcntId() {
    var id = $("#iIdKeHoachLuaChonNhaThau").val();
    if (id == "" || id == guidEmpty) return;
    $.ajax({
        url: "/QLVonDauTu/KHLuaChonNhaThau/GetAllGoiThauByKhlcntId",
        type: "GET",
        dataType: "json",
        data: { id: id, bIsDieuChinh: bIsDieuChinh},
        success: function (result) {
            if (result.data == null) return;
            var sItem = []
            result.data.forEach(function (item) {
                sItem.push("<tr data-id='" + item.iID_GoiThauID + "' data-parentid='" + item.iID_ParentID +"' ondblclick='ViewGoiThauDetail(\"" + item.iID_GoiThauID + "\")'>");
                sItem.push("<td class='width-200'><input type='text'class='form-control sTenGoiThau' value='" + item.sTenGoiThau + "'></td>");
                sItem.push("<td class='fGiaTri text-right'>" + FormatNumber(item.fTienTrungThau) + "</td>");
                sItem.push("<td>" + sCbxHinhThuc + "</td>");
                sItem.push("<td>" + sCbxPhuongThuc + "</td>");
                sItem.push("<td '><input type='text' class='form-control sThoiGianTCLCNT' 'text-right' value='" + item.sThoiGianTCLCNT + "'></td>");
                sItem.push("<td>" + sCbxLoaiHD + "</td>");
                sItem.push("<td '><input type='text' class='form-control iThoiGianThucHien' 'text-right' value='" + item.iThoiGianThucHien + "'></td>");
                sItem.push("<td class='width-50 text-center'><button class='btn-detail'><i class='fa fa-eye fa-lg' aria-hidden='true' onclick=DetailGoiThau('"+item.iID_GoiThauID+"')></i></button><button class='btn-delete'><i class='fa fa-trash-o fa-lg' aria-hidden='true' onclick=DeleteGoiThau($(this))></i></button></td>");
                sItem.push("</tr>");
            });
            $("#tblGoiThau").html(sItem.join(''));

            $.each($("#tblGoiThau tr"), function (index, item) {
                var goithauId = $(item).data("id");
                var itemGoiThau = $.map(result.data, function (n) { return n.iID_GoiThauID == goithauId ? n : null });
                if (itemGoiThau == null || itemGoiThau.length == 0) return false;
                $(item).find(".sHinhThucChonNhaThau").val(itemGoiThau[0].sHinhThucChonNhaThau);
                $(item).find(".sPhuongThucDauThau").val(itemGoiThau[0].sPhuongThucDauThau);
                $(item).find(".sHinhThucHopDong").val(itemGoiThau[0].sHinhThucHopDong);
            });
        }
    });
}

function SetupItem() {
    var iIdKeHoachLuaChonNhaThau = $("#iIdKeHoachLuaChonNhaThau").val();
    if (iIdKeHoachLuaChonNhaThau != guidEmpty) {
        if (bIsDieuChinh)
            $("#cbxLoaiChungTu").attr('disabled', true);
        $("#iID_DonViQuanLyID").attr('disabled', true);
        $("#iID_DuAnID").attr('disabled', true);
    } else {
        $("#cbxLoaiChungTu").attr('disabled', false);
        $("#iID_DonViQuanLyID").attr('disabled', false);
        $("#iID_DuAnID").attr('disabled', false);
    }
}

function GetCbxDonVi() {
    $.ajax({
        url: "/QLVonDauTu/KHLuaChonNhaThau/GetCbxDonViQuanLy",
        type: "GET",
        dataType: "json",
        success: function (result) {
            if (result.data != null && result.data != "") {
                $("#iID_DonViQuanLyID").append(result.data);
            }
            var sMaDonVi = $("#iIdMaDonVi").val();
            if (sMaDonVi != null && sMaDonVi != '') {
                $("#iID_DonViQuanLyID").val(sMaDonVi);
                $("#iID_DonViQuanLyID").change();
            }
        }
    });
}

function GetCbxGoiThau() {
    sCbxPhuongThuc = "<select class='sPhuongThucDauThau form-control'></select>";
    sCbxHinhThuc = "<select class='sHinhThucChonNhaThau form-control'></select>";
    sCbxLoaiHD = "<select class='sHinhThucHopDong form-control'></select>";

    $.ajax({
        url: "/QLVonDauTu/KHLuaChonNhaThau/GetDataDropdown",
        type: "POST",
        async: false,
        dataType: "json",
        success: function (result) {
            if (result != null) {
                sCbxPhuongThuc = "<select class='sPhuongThucDauThau form-control'>";
                if (result.phuongThucLuaChonNT != null) {
                    result.phuongThucLuaChonNT.forEach(function (item) {
                        sCbxPhuongThuc += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                    });
                }
                sCbxPhuongThuc += "</select>";

                sCbxHinhThuc = "<select class='sHinhThucChonNhaThau form-control'>";
                if (result.hinhThucChonNT != null) {
                    result.hinhThucChonNT.forEach(function (item) {
                        sCbxHinhThuc += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                    });
                }
                sCbxHinhThuc += "</select>";

                sCbxLoaiHD = "<select class='sHinhThucHopDong form-control'>";
                if (result.loaiHopDong != null) {
                    result.loaiHopDong.forEach(function (item) {
                        sCbxLoaiHD += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                    });
                }
                sCbxLoaiHD += "</select>";
            }
        }
    });
}

function getCanCu(iLoaiChungTu) {
    var title = iLoaiChungTu == 2 ? "Danh sách QĐĐT" : "Danh sách dự toán";
    var opShowGTPheDuyet;
    if (iLoaiChungTu == 1) {
        opShowGTPheDuyet = "GT phê duyệt TKTC";
    }
    else if (iLoaiChungTu == 2) {
        opShowGTPheDuyet = "GT phê duyệt PDDA";
    }
    else {
        opShowGTPheDuyet = "GT phê duyệt CTĐT";
    }
    $("#idShowTitle").html(title);
    $(".opShowGTPheDuyet").html(opShowGTPheDuyet);
}
getCanCu(iLoaiChungTu = $("#cbxLoaiChungTu").val());

function GetDuAn() {
    lstDuAn = [];
    $("#iID_DuAnID").html("<option value='' data-sMaDuAn='' data-fTongMucDauTu='0' data-iIDMaCDT=''>--Chọn--</option>");
    $("#fTongMucDauTuPheDuyet").val(0);
    var sDonVi = $("#iID_DonViQuanLyID").val();
    var iLoaiChungTu = $("#cbxLoaiChungTu").val();
    getCanCu(iLoaiChungTu);
    if (sDonVi == null || sDonVi == "" || iLoaiChungTu == null || iLoaiChungTu == "") return;

    $.ajax({
        url: "/QLVonDauTu/KHLuaChonNhaThau/FindDuAn",
        type: "GET",
        dataType: "json",
        data: { iID_MaDonViQuanLyID: sDonVi, iLoaiChungTu: iLoaiChungTu },
        success: function (result) {
            lstDuAn = result.data;
            if (lstDuAn != null || lstDuAn != []) {
                $.each(lstDuAn, function (index, item) {
                    $("#iID_DuAnID").append("<option value='" + item.iID_DuAnID + "' data-smaduan='" + item.sMaDuAn + "' data-ftongmucdautu='" + item.fTongMucDauTu + "' data-iidmacdt='" + item.iID_MaCDT + "'>" + item.sTenDuAn + "</option>");
                });
                var iIdDuAn = $("#iIdDuAnId").val();
                if (iIdDuAn != guidEmpty) {
                    $("#iID_DuAnID").val(iIdDuAn);
                } else {
                    $("#iID_DuAnID").val(null);
                }
                $("#iID_DuAnID").change();
            }

        }
    });
}

function ChooseDuAn() {
    $("#fTongMucDauTuPheDuyet").val(0);
    var iIdDuAn = $("#iID_DuAnID option:selected").val();
    if (iIdDuAn != undefined) {
        $("#cbxChuDauTu").val($("#iID_DuAnID").find(':selected').data("iidmacdt"));
        $("#fTongMucDauTuPheDuyet").val(FormatNumber($("#iID_DuAnID").find(':selected').data("ftongmucdautu")));
        GetListChungTu();
    }
}

function InsertGoiThau() {
    var sItem = []
    var iIdGoiThau = NewGuid();
    sItem.push("<tr data-id='" + iIdGoiThau + "' ondblclick='ViewGoiThauDetail(\"" + iIdGoiThau + "\")'>");
    sItem.push("<td class='width-200'><input type='text' class='form-control sTenGoiThau' value=''></td>");
    //sItem.push("<td  width-200'><input type='text' class='form-control' id='sTenGoiThau' value='" + item.sTenGoiThau + "'></td>");
    sItem.push("<td width=155.27px class='fGiaTri text-right'></td>");
    sItem.push("<td width=155.27px >" + sCbxHinhThuc + "</td>");
    sItem.push("<td width=155.27px >" + sCbxPhuongThuc + "</td>");
    sItem.push("<td width=155.27px><input type='text' class='form-control sThoiGianTCLCNT' value=''></td>");
    sItem.push("<td width=155.27px >" + sCbxLoaiHD + "</td>");
    sItem.push("<td width=155.27px><input type='text' class='form-control iThoiGianThucHien' value=''></td>");
    //sItem.push("<td ><input type='text' class='form-control' id ='iThoiGianThucHien' value='" + item.iThoiGianThucHien + "'></td>");
    sItem.push("<td class='width-50 text-center'><button class='btn-delete'><i class='fa fa-trash-o fa-lg' aria-hidden='true' onclick='DeleteGoiThau($(this))'></i></button></td>");
    sItem.push("</tr>");
    $("#tblGoiThau").append(sItem.join(''));
}

function DeleteGoiThau(item) {
    $(item).closest("tr").addClass("error-row");
    $(item).css('display', 'none');
}

function ViewGoiThauDetail(iIdGoiThauId) {
    $("#iIdGoiThau").val(iIdGoiThauId);
    $("#modalChiTietGoiThau").modal("show");
    $("#tblHangMucChinh tbody").empty();
    // GetDanhSachGoiThau(iIdGoiThauId);

    if (!lstChungTuGoiThau[iIdGoiThauId]) {
        // update lại array chung cho tất cả gói thầu
        lstChungTuGoiThau[iIdGoiThauId] = [[], [], []];
    }
    
    GetNguonVonGoiThauDetail(iIdGoiThauId);
    GetChiPhiGoiThauDetail(iIdGoiThauId);
    // calculate sum nguồn vốn
    SetLstNguonVon();
    GetTongChiTiet();
}

// isOnBlur: check nếu đang trong sự kiện blur thì mình sẽ sử dụng mảng itemsChungTuGoiThau, vì mảng này là mảng được update
function getNguonVonForCurrentGoiThau(idGoiThau, nguonVon, isOnBlur) {
    let nguonVonConLaiForCurrentGoiThau = nguonVon.fGiaTriPheDuyet;
    for (let goiThau in lstChungTuGoiThau) {
        // trừ đi nguon von đã được sử dụng cho các gói thâu khác
        if (goiThau !== idGoiThau) {
            if (lstChungTuGoiThau[goiThau][0].length > 0) {
                lstChungTuGoiThau[goiThau][0].forEach(nv => {
                    if (nv.iID_NguonVonID === nguonVon.iID_NguonVonID)
                            nguonVonConLaiForCurrentGoiThau -= nv.fGiaTriGoiThau;
                });
            }
        }
        // trừ đi nguồn vốn đang được sử dụng cho gói thầu hiện tại
        else {
            if (isOnBlur) {
                itemsChungTuGoiThau[0].forEach(nv => {
                    if (nv.iID_NguonVonID === nguonVon.iID_NguonVonID)
                        nguonVonConLaiForCurrentGoiThau -= nv.fGiaTriGoiThau;
                });
            } else {
                lstChungTuGoiThau[goiThau][0].forEach(nv => {
                    if (nv.iID_NguonVonID === nguonVon.iID_NguonVonID)
                        nguonVonConLaiForCurrentGoiThau -= nv.fGiaTriGoiThau;
                });
            }
        }
    }
    return nguonVonConLaiForCurrentGoiThau;
}

function getChiPhiForCurrentGoiThau(idGoiThau, chiphi) {
    let chiPhiConLai = chiphi.fGiaTriPheDuyet;
    for (let goiThau in lstChungTuGoiThau) {
        // trừ đi chi phí đã chọn cho các gói thầu khác
        if (goiThau !== idGoiThau && lstChungTuGoiThau[goiThau][1].length > 0) {
            lstChungTuGoiThau[goiThau][1].forEach(cp => {
                if (cp.iID_ChiPhiID === chiphi.iID_ChiPhiID)
                    chiPhiConLai -= cp.fGiaTriGoiThau;
            });
        }
        // trừ đi chi phí đã chọn cho các gói thầu hiện tại
        if (goiThau === idGoiThau) {
            lstChungTuGoiThau[goiThau][1].forEach(cp => {
                if (cp.iID_ChiPhiID === chiphi.iID_ChiPhiID)
                    chiPhiConLai -= cp.fGiaTriGoiThau;
            });
        }
    }
    return chiPhiConLai;
}

function recalculateGiaTriNguonVonConLaiOnBlur(idGoiThau) {
    var lstNguonVon = $.map(itemsChungTu[0], function (n) { return n.iID_NguonVonID != 0 ? n : null });
    $("#tblDanhSachNguonVonModal .fGiaTriConLai").each((ind, item) => {
        $(item).text(FormatNumber(getNguonVonForCurrentGoiThau(idGoiThau, lstNguonVon[ind], true)));
    });
}

function validateGiaTriConLaiBeforeSave(currentGoiThau) {
    // validate nguon von
    var lstNguonVon = $.map(itemsChungTu[0], function (n) { return n.iID_NguonVonID != 0 ? n : null });
    var lstNguonVonConLai = [];
    lstNguonVon.forEach((item) => {
        lstNguonVonConLai.push(getNguonVonForCurrentGoiThau(currentGoiThau, item, true));
    })
    var numOfInValid = lstNguonVonConLai.filter(nv => nv < 0).length;
    if (numOfInValid > 0) {
        alert("Nguồn vốn còn lại không được nhỏ hơn 0")
        return false;
    }
    return true;
}

function GetNguonVonGoiThauDetail(iIdGoiThau) {
    var lstNguonVon = $.map(itemsChungTu[0], function (n) { return n.iID_NguonVonID != 0 ? n : null });
    var sItem = [], arrNguonVon, usingArrGoiThauNguonVon = true;
    // chỉ dùng arrGoiThauNguonVon 1 lần
    if (arrGoiThauNguonVon.length > 0) {
        arrNguonVon = [...arrGoiThauNguonVon.filter(nv => nv.iID_GoiThauID === iIdGoiThau)];
    }
    else {
        arrNguonVon = lstChungTuGoiThau[iIdGoiThau][0];
        usingArrGoiThauNguonVon = false;
    }
    lstNguonVon.forEach(function (item) {
        var indexInArrGoiThauNguonVon = arrNguonVon.map(ele => ele.iID_NguonVonID).indexOf(item.iID_NguonVonID);
        if (getNguonVonForCurrentGoiThau(iIdGoiThau, item) >= 0) {
            sItem.push("<tr data-id='" + item.iID_NguonVonID + "'>");
            sItem.push("<td class='width-50 text-center'><input type='checkbox' class='ck_NguonVon' value='" + item.iID_NguonVonID + (indexInArrGoiThauNguonVon >= 0 ? '\' checked ' : '\'') + "></td>");
            sItem.push("<td class='sNoiDung'>" + item.sNoiDung + "</td>");
            sItem.push("<td class='fGiaTriPheDuyet text-right'>" + FormatNumber(item.fGiaTriPheDuyet) + "</td>");
            sItem.push(`<td><input type=\'text\' ${indexInArrGoiThauNguonVon >= 0 ? "" : "disabled"} onkeyup=\'ValidateNumberKeyUp(this);\' onkeypress=\'return ValidateNumberKeyPress(this, event);\' class=\'fGiaTriGoiThau form-control\' style=\'text-align:right\' value=\'${indexInArrGoiThauNguonVon >= 0 ? (usingArrGoiThauNguonVon ? arrNguonVon[indexInArrGoiThauNguonVon].fTienGoiThau : arrNguonVon[indexInArrGoiThauNguonVon].fGiaTriGoiThau) : ""}\'/></td>`);
            sItem.push("<td class='fGiaTriConLai text-right'>" + FormatNumber(getNguonVonForCurrentGoiThau(iIdGoiThau, item)) + "</td>");
            sItem.push("</tr>");
        }
       
    });
    // neu update thi tinh tong luon
    if (arrNguonVon.length > 0) {
        SetLstNguonVon();
        GetTongChiTiet();
    }
    $("#tblDanhSachNguonVonModal tbody").html(sItem.join(""));

    $("#tblDanhSachNguonVonModal .ck_NguonVon").change(function () {
        var currentId = $(this).val();
        if (this.checked) {
            $(this).closest("tr").find(".fGiaTriGoiThau").prop("disabled", false);
        } else {
            $(this).closest("tr").find(".fGiaTriGoiThau").prop("disabled", true);
        }
        SetLstNguonVon();
        GetTongChiTiet();
    });

    $("#tblDanhSachNguonVonModal .fGiaTriGoiThau").blur(function () {
        SetLstNguonVon();
        GetTongChiTiet();
        recalculateGiaTriNguonVonConLaiOnBlur(iIdGoiThau)
    });
}

function SetLstNguonVon() {
    itemsChungTuGoiThau[0] = [];
    $.each($("#tblDanhSachNguonVonModal tbody").find(".ck_NguonVon:checkbox:checked"), function (index, child) {
        var obj = {};
        obj.iID_NguonVonID = $(child).closest("tr").data("id");
        obj.sNoiDung = $(child).closest("tr").find(".sNoiDung").text();
        obj.iThuTu = $(child).closest("tr").index();
        obj.fGiaTriPheDuyet = parseFloat($(child).closest("tr").find(".fGiaTriPheDuyet").text().replaceAll(".", ""));
        obj.fGiaTriGoiThau = parseFloat($(child).closest("tr").find(".fGiaTriGoiThau").val().replaceAll(".", ""));
        itemsChungTuGoiThau[0].push(obj);
    });
}

function GetChiPhiGoiThauDetail(iIdGoiThau) {
    var lstNguonVon = $.map(itemsChungTu[1], function (n) { return n.iID_NguonVonID == 0 && n.iID_HangMucID == null ? n : null });
    $("#tblDanhSachChiPhiModal tbody").empty();
    var sItem = [], arrChiPhi = [];
    // chỉ dùng arrGoiThauChiPhi một lần
    if (arrGoiThauChiPhi.length > 0) {
        arrChiPhi = [...arrGoiThauChiPhi.filter(nv => nv.iID_GoiThauID === iIdGoiThau)];
    } else {
        arrChiPhi = lstChungTuGoiThau[iIdGoiThau][1];
    }

    lstNguonVon.forEach(function (item) {
        var parentIndex = arrChiPhi.map(ele => {
            if (ele.iID_ChiPhi) return ele.iID_ChiPhi;
            else return ele.iID_ChiPhiID;
        }
    ).indexOf(item.iID_ParentId);
        var childIndex = arrChiPhi.map(ele => {
            if (ele.iID_ChiPhi) return ele.iID_ChiPhi;
            else return ele.iID_ChiPhiID;
        }).indexOf(item.iID_ChiPhiID);
        sItem.push("<tr data-id='" + item.iID_ChiPhiID + "' data-parent='" + item.iID_ParentId + "'>");
        sItem.push("<td class='width-50 text-center'><input type='checkbox' class='ck_ChiPhi' value='" + item.iID_ChiPhiID +
            (childIndex >= 0 || parentIndex >= 0 ? "\' checked " : "\'") + " onClick='CheckChiPhi(this)'></td>");
        sItem.push("<td class='sNoiDung'>" + item.sNoiDung + "</td>");
        sItem.push("<td class='fGiaTriPheDuyet text-right'>" + FormatNumber(item.fGiaTriPheDuyet) + "</td>");
        // update ke hoach lua chon nha thau
        // if (iIdKHLuaChonNhaThau) {
            if (childIndex < 0 && parentIndex < 0)
                sItem.push(
                    "<td><input type='text' disabled='disabled' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' class='fGiaTriGoiThau form-control' style='text-align:right' value='0'/></td>");
            else {
                sItem.push(
                    "<td><input type='text' onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' class='fGiaTriGoiThau form-control' style='text-align:right' value='" +
                    (childIndex >= 0
                        ? (arrChiPhi[childIndex].fGiaTriGoiThau ? arrChiPhi[childIndex].fGiaTriGoiThau : arrChiPhi[childIndex].fTienGoiThau )
                        : (arrChiPhi[parentIndex].fGiaTriGoiThau ? arrChiPhi[parentIndex].fGiaTriGoiThau : arrChiPhi[parentIndex].fTienGoiThau)) +
                    "'/></td>");
            }
        //}
        // add new ke hoach lua chon nha thau, không thể sửa ô input giá trị chi phí, ô này sẽ có giá trị bằng tổng các hạng mục con được chọn khi mở chi tiết hạng mục chi phí hiện tại, ở button cuối hàng
        /*
        else {
            sItem.push(
                "<td><input type='text' disabled onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' class='fGiaTriGoiThau form-control' style='text-align:right' value='" +
                (0) +
                "'/></td>");
        }
        */
        sItem.push("<td class='fGiaTriConLai text-right'>" + FormatNumber(getChiPhiForCurrentGoiThau(iIdGoiThau, item)) + "</td>");
        sItem.push("<td class='width-50'><button class='btn-primary' onclick='GetHangMucGoiThauDetail(\"" + iIdGoiThau + "\", \"" + item.iID_ChiPhiID + "\")' " + (childIndex >= 0 || parentIndex >= 0 ? " " : " disabled")+"><i class='fa fa-pencil-square-o fa-lg' aria-hidden='true')'></i></button></td>");
        sItem.push("</tr>");
    });
    $("#tblDanhSachChiPhiModal tbody").html(sItem.join(""));

    // update itemsChungTuGoiThau trong trường hợp update
    if (iIdKHLuaChonNhaThau) {
        // update chi phí
        $("#tblDanhSachChiPhiModal .fGiaTriGoiThau").each(function (index, item) {
            CheckChiPhi($(item).closest("tr").find(".ck_ChiPhi")[0]);
        });
        // update hạng mục
        itemsChungTuGoiThau[2] = $.map(itemsChungTuGoiThau[2], function (n) { return n.iID_ChiPhiID == $("#iIdChiPhiChoose").val() ? null : n }); // lấy những thằng cũ
        // cập nhật mảng itemschungtugoithau, thì loop qua các chi phí đang được check, tạo bảng hạng mục, nhưng k hiển thị, cho từng chi phí
        itemsChungTuGoiThau[1].forEach(chiphi => {
            GetHangMucGoiThauDetail(iIdGoiThau, chiphi.iID_ChiPhiID, true);
        });
    }

    $("#tblDanhSachChiPhiModal .fGiaTriGoiThau").change(function () {
        CheckChiPhi($(this).closest("tr").find(".ck_ChiPhi")[0]);
    });
}

function checkIfHangMucIsCheckedForOtherGoiThau(hangmucId, idGoiThau) {
    let returnVal = false;
    for (let goiThauId in lstChungTuGoiThau) {
        if (goiThauId !== idGoiThau) {
            var arrOfIndexHangMuc = lstChungTuGoiThau[goiThauId][2].map(hm => hm.iID_HangMucID);
            if (arrOfIndexHangMuc.indexOf(hangmucId) >= 0) {
                returnVal = true;
            }
        }
    }
    return returnVal;
}

// isAddingToItemsChungTuGoiThau: nếu đang cập nhật itemschungtuGoithau -> true
function GetHangMucGoiThauDetail(iIdGoiThau, iIdChiPhi, isAddingToItemsChungTuGoiThau) {
    $("#iIdChiPhiChoose").val(iIdChiPhi);
    var lstNguonVon = $.map(itemsChungTu[2], function (n) { return n.iID_NguonVonID == 0 && n.iID_ChiPhiID == iIdChiPhi && n.iID_HangMucID != null ? n : null });
    if (!isAddingToItemsChungTuGoiThau) {
        $("#tblHangMucChinh tbody").css("display", "block");
    }
    else
    {
        $("#tblHangMucChinh tbody").css("display", "none");
    }
    $("#tblHangMucChinh tbody").empty();
    var sItem = [], arrHangMuc = [];
    if (arrGoiThauHangMuc.length > 0) {
        arrHangMuc = [...arrGoiThauHangMuc.filter(nv => nv.iID_GoiThauID === iIdGoiThau)];
    } else {
        arrHangMuc = lstChungTuGoiThau[iIdGoiThau][2];
    }
    lstNguonVon.forEach(function (item) {
        // nếu hạng mục này đã được check cho gói thầu khác, thì sẽ k cho hiển thị 
        if (!checkIfHangMucIsCheckedForOtherGoiThau(item.iID_HangMucID, iIdGoiThau)) {
            var parentIndex = arrHangMuc.map(ele => ele.iID_HangMucID).indexOf(item.iID_ParentId);
            var childIndex = arrHangMuc.map(ele => ele.iID_HangMucID).indexOf(item.iID_HangMucID);
            sItem.push("<tr data-id='" + item.iID_HangMucID + "' data-parent='" + item.iID_ParentId + "'>");
            if (childIndex >= 0 || parentIndex >= 0) {
                sItem.push("<td class='width-50 text-center'><input type='checkbox' class='ck_HangMuc' value='" +
                    item.iID_HangMucID +
                    "' checked onClick='CheckHangMuc(this)'></td>");
            } else {
                sItem.push("<td class='width-50 text-center'><input type='checkbox' class='ck_HangMuc' value='" +
                    item.iID_HangMucID +
                    "' onClick='CheckHangMuc(this)'></td>");
            }
            sItem.push("<td class='sMaOrder width-50'>" + item.sMaOrder + "</td>");
            sItem.push("<td class='sNoiDung'>" + item.sNoiDung + "</td>");
            sItem.push("<td class='fGiaTriPheDuyet text-right'>" + FormatNumber(item.fGiaTriPheDuyet) + "</td>");
            sItem.push("<td class='fGiaTriGoiThau text-right'>0</td>");
            sItem.push("<td class='fGiaTriConLai text-right'>" +
                FormatNumber(item.fGiaTriPheDuyet - item.fGiaTriGoiThau) +
                "</td>");
            sItem.push("</tr>");
        }
    });
    $("#tblHangMucChinh tbody").html(sItem.join(""));
    // update hạng mục trong itemschungtugoithau
    itemsChungTuGoiThau[2] = $.map(itemsChungTuGoiThau[2], function (n) { return n.iID_ChiPhiID == $("#iIdChiPhiChoose").val() ? null : n });
    $.each($("#tblHangMucChinh tbody").find(".ck_HangMuc:checkbox:checked"), function (index, child) {
        var obj = {};
        obj.iID_NguonVonID = null;
        obj.iID_ChiPhiID = $("#iIdChiPhiChoose").val();
        obj.iID_HangMucID = $(child).closest("tr").data("id");
        obj.iID_ParentId = $(child).closest("tr").data("parent");
        obj.sNoiDung = $(child).closest("tr").find(".sNoiDung").text();
        obj.iThuTu = $(child).closest("tr").index();
        obj.sMaOrder = $(child).closest("tr").find(".sMaOrder").text();
        obj.fGiaTriPheDuyet = parseFloat($(child).closest("tr").find(".fGiaTriPheDuyet").text().replaceAll(".", ""));
        obj.fGiaTriGoiThau = parseFloat($(child).closest("tr").find(".fGiaTriGoiThau").text().replaceAll(".", ""));
        itemsChungTuGoiThau[2].push(obj);
    });
}

function GetTongChiTiet() {
    var fTongNguonVon = 0;
    $.each($("#tblDanhSachNguonVonModal tbody").find(".ck_NguonVon:checkbox:checked"), function (index, item) {
        var sGiaTri = $(item).closest("tr").find(".fGiaTriGoiThau").val().replaceAll(".", "");
        if (sGiaTri != "")
            fTongNguonVon += parseFloat(sGiaTri);
    });

    var fTongChiPhi = 0;
    $.each($("#tblDanhSachChiPhiModal tbody").find("[data-parent='null'] .ck_ChiPhi:checkbox:checked"), function (index, item) {
        var sGiaTri = $(item).closest("tr").find(".fGiaTriGoiThau").val().replaceAll(".", "");
        if (sGiaTri != "")
            fTongChiPhi += parseFloat(sGiaTri);
    });

    if (fTongNguonVon == 0)
        $(".rTongNguonVon").text("0");
    else
        $(".rTongNguonVon").text(FormatNumber(fTongNguonVon));

    if (fTongNguonVon - fTongChiPhi == 0)
        $(".rConLai").text("0");
    else
        $(".rConLai").text(FormatNumber(fTongNguonVon - fTongChiPhi));
}

function ConvertItemsChungTu(lstChungTu) {
    var lstNguonVon = $.map(lstChungTu, function (n) { return n.iID_NguonVonID != 0 ? n : null });
    var lstChiPhi = $.map(lstChungTu, function (n) { return n.iID_ChiPhiID != null && n.iID_HangMucID == null ? n : null });
    var lstHangMuc = $.map(lstChungTu, function (n) { return n.iID_HangMucID != null ? n : null });

    var lstChiPhiConvert = [];
    $.map(lstChiPhi, function (n) { return n.iID_ParentId == null ? n : null }).forEach(function (item) {
        RecursiveChiPhi(item, lstChiPhi).forEach(function (itemChild) {
            lstChiPhiConvert.push(itemChild);
        });
    })

    var lstHangMucConvert = [];
    $.map(lstHangMuc, function (n) { return n.iID_ParentId == null ? n : null }).forEach(function (item) {
        RecursiveHangMuc(item, lstHangMuc).forEach(function (itemChild) {
            lstHangMucConvert.push(itemChild);
        });
    })

    itemsChungTu = [];
    itemsChungTu.push(lstNguonVon);
    itemsChungTu.push(lstChiPhiConvert);
    itemsChungTu.push(lstHangMucConvert);
}

function RecursiveChiPhi(itemChiPhi, lstChiPhi) {
    var lstChild = [];
    lstChild.push(itemChiPhi);
    $.map(lstChiPhi, function (n) { return n.iID_ParentId == itemChiPhi.iID_ChiPhiID && n.iID_HangMucID == null ? n : null }).forEach(function (item) {
        RecursiveChiPhi(item, lstChiPhi).forEach(function (itemChild) {
            lstChild.push(itemChild);
        });
    });
    return lstChild;
}

function RecursiveHangMuc(itemHangMuc, lstHangMuc) {
    var lstChild = [];
    lstChild.push(itemHangMuc);
    $.map(lstHangMuc, function (n) { return n.iID_ChiPhiID == itemHangMuc.iID_ChiPhiID && n.iID_ParentId == itemHangMuc.iID_HangMucID ? n : null }).forEach(function (item) {
        RecursiveHangMuc(item, lstHangMuc).forEach(function (itemChild) {
            lstChild.push(itemChild);
        });
    });
    return lstChild;
}

//- Check chi phi

function CheckAllChiPhi(item) {
    $.each($("#tblDanhSachChiPhiModal tbody").find("[data-parent='null'] .ck_ChiPhi"), function (index, parent) {
        parent.checked = item.checked;
        CheckChiPhi(parent);
    });
}

function CheckChiPhi(item) {
    RecursiveCheckChildChiPhi(item, item.checked);
    RecursiveCheckParentChiPhi(item, item.checked);
    var fTong = 0
    $.each($("#tblDanhSachChiPhiModal tbody").find("[data-parent='null'] .ck_ChiPhi:checkbox:checked"), function (index, parent) {
        if ($(parent).closest("tr").find(".fGiaTriGoiThau").text().replaceAll(".", "") != "")
            fTong += parseFloat($(parent).closest("tr").find(".fGiaTriGoiThau").text().replaceAll(".", ""));
    });

    itemsChungTuGoiThau[1] = [];
    $.each($("#tblDanhSachChiPhiModal tbody").find(".ck_ChiPhi:checkbox:checked"), function (index, child) {
        var obj = {};
        obj.iID_ChiPhiID = $(child).closest("tr").data("id");
        obj.iID_ParentId = $(child).closest("tr").data("parent");
        obj.sNoiDung = $(child).closest("tr").find(".sNoiDung").text();
        obj.iThuTu = $(child).closest("tr").index();
        obj.fGiaTriPheDuyet = parseFloat($(child).closest("tr").find(".fGiaTriPheDuyet").text().replaceAll(".", ""));
        obj.fGiaTriGoiThau = parseFloat($(child).closest("tr").find(".fGiaTriGoiThau").val().replaceAll(".", ""));
        itemsChungTuGoiThau[1].push(obj);
    });
    GetTongChiTiet();
}

function RecursiveCheckParentChiPhi(item, bIsCheck) {
    var parentId = $(item).closest("tr").data("parent");
    if (parentId == null) return;

    var fGiaTri = 0;
    if ($(item).closest("tr").find(".fGiaTriGoiThau").val().replaceAll(".", "") != "")
        fGiaTri = parseFloat($(item).closest("tr").find(".fGiaTriGoiThau").val().replaceAll(".", ""))
    var parentItem = $("#tblDanhSachChiPhiModal tbody [data-id='" + parentId + "'] .ck_ChiPhi");

    var fGiaTriPheDuyet = parseFloat($("#tblDanhSachChiPhiModal tbody [data-id='" + parentId + "'] .fGiaTriPheDuyet").text().replaceAll(".", ""));
    var fGiaTriGoiThau = 0;
    $.each($("#tblDanhSachChiPhiModal tbody [data-parent='" + parentId + "'] .ck_ChiPhi:checkbox:checked"), function (index, child) {
        var sGiaTri = $(child).closest("tr").find(".fGiaTriGoiThau").val().replaceAll(".", "");
        if (sGiaTri != "")
            fGiaTriGoiThau += parseFloat(sGiaTri);
    });

    parentItem[0].checked = bIsCheck;

    if (fGiaTriGoiThau == 0) {
        $(parentItem).closest("tr").find(".fGiaTriGoiThau").val("0");
        $(parentItem).closest("tr").find(".fGiaTriConLai").text(FormatNumber(fGiaTriPheDuyet));
    } else {
        $(parentItem).closest("tr").find(".fGiaTriGoiThau").val(FormatNumber(fGiaTriGoiThau));
        if (fGiaTriPheDuyet - fGiaTriGoiThau != 0)
            $(parentItem).closest("tr").find(".fGiaTriConLai").text(FormatNumber(getChiPhiForCurrentGoiThau($("#iIdGoiThau").val(), item)));
        else
            $(parentItem).closest("tr").find(".fGiaTriConLai").text("0")
    }
    // update giá trị còn lại (bắt buộc) dựa trên các gói thầu
    var lstChiPhi = $.map(itemsChungTu[1], function (n) { return n.iID_NguonVonID == 0 && n.iID_HangMucID == null ? n : null });
    var currChiPhi = lstChiPhi.filter(cp => cp.iID_ChiPhiID === $(item).val())[0];
    $(item).closest("tr").find(".fGiaTriConLai").text(FormatNumber(getChiPhiForCurrentGoiThau($("#iIdGoiThau").val(), currChiPhi)));

    $(parentItem).closest("tr").find(".fGiaTriGoiThau").prop("disabled", true);
    $(parentItem).closest("tr").find("button").prop("disabled", true);

    if ($("#tblDanhSachChiPhiModal tbody").find("[data-parent='" + parentId + "'] .ck_ChiPhi:checkbox:checked").length == 0) {
        if (bIsCheck) {
            $(parentItem).closest("tr").find(".fGiaTriGoiThau").prop("disabled", false);
            $(parentItem).closest("tr").find("button").prop("disabled", false);
        }
    } else if (!bIsCheck) {
        parentItem[0].checked = !bIsCheck;
    }

    RecursiveCheckParentChiPhi(parentItem, bIsCheck);
}

function RecursiveCheckChildChiPhi(item, bIsCheck) {
    $(item).closest("tr").find(".fGiaTriGoiThau").prop("disabled", true);
    $(item).closest("tr").find("button").prop("disabled", true);
    var iId = $(item).val();
    var fSumTong = 0;
    var countChild = 0;
    $.each($("#tblDanhSachChiPhiModal tbody").find("[data-parent='" + iId + "'] .ck_ChiPhi"), function (index, child) {
        child.checked = bIsCheck;
        fSumTong += RecursiveCheckChildChiPhi($(child), bIsCheck);
        countChild++;
    });
    if (bIsCheck && fSumTong == 0) {
        if ($(item).closest("tr").find(".fGiaTriGoiThau").val().replaceAll(".", "") != "")
            fSumTong = parseFloat($(item).closest("tr").find(".fGiaTriGoiThau").val().replaceAll(".", ""));
    }
    var fGiaTriPheDuyet = parseFloat($(item).closest("tr").find(".fGiaTriPheDuyet").text().replaceAll(".", ""));
    var lstHangMuc = $.map(itemsChungTu[2], function (n) { return n.iID_NguonVonID == 0 && n.iID_ChiPhiID == iId && n.iID_HangMucID != null ? n : null });
    
    if (bIsCheck) {
        if (countChild == 0) {
            $(item).closest("tr").find(".fGiaTriGoiThau").prop("disabled", false);
            $(item).closest("tr").find("button").prop("disabled", false);
        } else {
            $(item).closest("tr").find(".fGiaTriGoiThau").prop("disabled", true);
            $(item).closest("tr").find("button").prop("disabled", true);
        }

        if (lstHangMuc.length > 0)
            $(item).closest("tr").find(".fGiaTriGoiThau").prop("disabled", true);
        else
            $(item).closest("tr").find(".fGiaTriGoiThau").prop("disabled", false);

        if (fSumTong == 0)
            $(item).closest("tr").find(".fGiaTriGoiThau").val("0");
        else
            $(item).closest("tr").find(".fGiaTriGoiThau").val(FormatNumber(fSumTong));

        if (fGiaTriPheDuyet - fSumTong == 0)
            $(item).closest("tr").find(".fGiaTriConLai").text("0");
        else
            $(item).closest("tr").find(".fGiaTriConLai").text(FormatNumber(getChiPhiForCurrentGoiThau($("#iIdGoiThau").val(), item)));
    } else {
        $(item).closest("tr").find(".fGiaTriGoiThau").val("0");
        $(item).closest("tr").find(".fGiaTriConLai").text(FormatNumber(fGiaTriPheDuyet));
    }
    // update giá trị còn lại (bắt buộc) dựa trên các gói thầu
    var lstChiPhi = $.map(itemsChungTu[1], function (n) { return n.iID_NguonVonID == 0 && n.iID_HangMucID == null ? n : null });
    var currChiPhi = lstChiPhi.filter(cp => cp.iID_ChiPhiID === $(item).val())[0];
    $(item).closest("tr").find(".fGiaTriConLai").text(FormatNumber(getChiPhiForCurrentGoiThau($("#iIdGoiThau").val(), currChiPhi)));
    return fSumTong;
}

//- Check hang muc
function CheckAllHangMuc(item) {
    $.each($("#tblHangMucChinh tbody").find("[data-parent='null'] .ck_HangMuc"), function (index, parent) {
        parent.checked = item.checked;
        CheckHangMuc(parent);
    });
}

function CheckHangMuc(item) {
    RecursiveCheckChildHangMuc(item, item.checked);
    RecursiveCheckParentHangMuc(item, item.checked);
    var fTong = 0
    $.each($("#tblHangMucChinh tbody").find("[data-parent='null'] .ck_HangMuc:checkbox:checked"), function (index, parent) {
        fTong += parseFloat($(parent).closest("tr").find(".fGiaTriGoiThau").text().replaceAll(".", ""));
    });

    var rowChiPhi = $("#tblDanhSachChiPhiModal [data-id='" + $("#iIdChiPhiChoose").val() + "']");
    var fGiaTriPheDuyet = parseFloat($(rowChiPhi).find(".fGiaTriPheDuyet").text().replaceAll(".", ""));
    $(rowChiPhi).find(".fGiaTriGoiThau").val(FormatNumber(fTong));
    if (fTong == 0) {
        $(rowChiPhi).find(".fGiaTriGoiThau").prop("disabled", false);
    } else {
        $(rowChiPhi).find(".fGiaTriGoiThau").prop("disabled", true);
    }
    $(rowChiPhi).find(".fGiaTriConLai").text(FormatNumber(fGiaTriPheDuyet - fTong));
    CheckChiPhi(rowChiPhi.find(".ck_ChiPhi")[0]);


    $.each($("#tblHangMucChinh tbody").find(".ck_HangMuc:checkbox:checked"), function (index, child) {
        var obj = {};
        obj.iID_NguonVonID = null;
        obj.iID_ChiPhiID = $("#iIdChiPhiChoose").val();
        obj.iID_HangMucID = $(child).closest("tr").data("id");
        obj.iID_ParentId = $(child).closest("tr").data("parent");
        obj.sNoiDung = $(child).closest("tr").find(".sNoiDung").text();
        obj.iThuTu = $(child).closest("tr").index();
        obj.sMaOrder = $(child).closest("tr").find(".sMaOrder").text();
        obj.fGiaTriPheDuyet = parseFloat($(child).closest("tr").find(".fGiaTriPheDuyet").text().replaceAll(".", ""));
        obj.fGiaTriGoiThau = parseFloat($(child).closest("tr").find(".fGiaTriGoiThau").text().replaceAll(".", ""));
        itemsChungTuGoiThau[2].push(obj);
    });
    itemsChungTuGoiThau[2] = $.map(itemsChungTuGoiThau[2], function (n) { return n.iID_ChiPhiID == $("#iIdChiPhiChoose").val() ? null : n });
}

function RecursiveCheckParentHangMuc(item, bIsCheck) {
    var parentId = $(item).closest("tr").data("parent");
    if (parentId == null) return;

    var parentItem = $("#tblHangMucChinh tbody [data-id='" + parentId + "'] .ck_HangMuc");
    var fGiaTriPheDuyet = parseFloat($("#tblHangMucChinh tbody [data-id='" + parentId + "'] .fGiaTriPheDuyet").text().replaceAll(".", ""));
    var fGiaTriGoiThau = 0;
    $.each($("#tblHangMucChinh tbody [data-parent='" + parentId + "'] .ck_HangMuc:checkbox:checked"), function (index, child) {
        var sGiaTri = $(child).closest("tr").find(".fGiaTriGoiThau").text().replaceAll(".", "");
        if (sGiaTri != "")
            fGiaTriGoiThau += parseFloat(sGiaTri);
    });
    parentItem[0].checked = bIsCheck;
    if (!bIsCheck && $("#tblHangMucChinh tbody").find("[data-parent='" + parentId + "'] .ck_HangMuc:checkbox:checked").length != 0) {
        parentItem[0].checked = !bIsCheck;
    }
    if (fGiaTriGoiThau == 0) {
        $(parentItem).closest("tr").find(".fGiaTriGoiThau").text("0");
        $(parentItem).closest("tr").find(".fGiaTriConLai").text(FormatNumber(fGiaTriPheDuyet));
    }
    else {
        $(parentItem).closest("tr").find(".fGiaTriGoiThau").text(FormatNumber(fGiaTriGoiThau));
        if (fGiaTriPheDuyet - fGiaTriGoiThau != 0)
            $(parentItem).closest("tr").find(".fGiaTriConLai").text(FormatNumber(fGiaTriPheDuyet - fGiaTriGoiThau));
        else
            $(parentItem).closest("tr").find(".fGiaTriConLai").text("0");
    }
    RecursiveCheckParentHangMuc(parentItem, bIsCheck);
}

function RecursiveCheckChildHangMuc(item, bIsCheck) {
    var iId = $(item).val();
    var fSumTong = 0;
    $.each($("#tblHangMucChinh tbody").find("[data-parent='" + iId + "'] .ck_HangMuc"), function (index, child) {
        child.checked = bIsCheck;
        fSumTong += RecursiveCheckChildHangMuc($(child), bIsCheck);
    });
    if (bIsCheck && fSumTong == 0) {
        fSumTong = parseFloat($(item).closest("tr").find(".fGiaTriPheDuyet").text().replaceAll(".", ""));
    }
    var fGiaTriPheDuyet = parseFloat($(item).closest("tr").find(".fGiaTriPheDuyet").text().replaceAll(".", ""));
    if (bIsCheck) {
        $(item).closest("tr").find(".fGiaTriGoiThau").text(FormatNumber(fSumTong));
        if (fGiaTriPheDuyet - fSumTong == 0)
            $(item).closest("tr").find(".fGiaTriConLai").text("0");
        else
            $(item).closest("tr").find(".fGiaTriConLai").text(FormatNumber(fGiaTriPheDuyet - fSumTong));
    } else {
        $(item).closest("tr").find(".fGiaTriGoiThau").text("0");
        $(item).closest("tr").find(".fGiaTriConLai").text(FormatNumber(fGiaTriPheDuyet));
    }
    return fSumTong;
}

//------ Event Window
function SaveDetailGoiThau() {
    var sConLai = $(".rConLai").text().replaceAll(".", "");
    var fConLai = 0;
    if (sConLai != "")
        fConLai = parseFloat(sConLai);
    // Bỏ validate tổng nguồn vốn = tổng chi phí
    /*
    if (fConLai != 0) {
        alert("Tổng nguồn vốn phải bằng tổng chi phí !");
        return;
    }
    */
    
    for (let goiThau in lstChungTuGoiThau) {
        if (!validateGiaTriConLaiBeforeSave(goiThau)) {
            return;
        }
    }

    lstChungTuGoiThau[$("#iIdGoiThau").val()] = [];
    if (itemsChungTuGoiThau == null || (itemsChungTuGoiThau[0].length == 0 && itemsChungTuGoiThau[1].length == 0 && itemsChungTuGoiThau[2].length == 0)) {
        alert("Chưa chọn thông tin chi tiết gói thầu !");
        return;
    }
    lstChungTuGoiThau[$("#iIdGoiThau").val()] = [...itemsChungTuGoiThau];
    $("#tblGoiThau [data-id='" + $("#iIdGoiThau").val() + "'] .fGiaTri").text($(".rTongNguonVon").text());
    alert("Cập nhật thành công !");
    $("#modalChiTietGoiThau").modal("hide");
}

function SaveKHLCNT() {
    if (!ValidateKHLCNT()) return;
    var objKeHoachLCNT = SetKeHoachLuaChonNhaThau();    
    var lstGoiThau = SetGoiThau();

    var obj = {};
    obj.objKHLuaChonNhaThau = objKeHoachLCNT;
    obj.lstGoiThau = lstGoiThau;
    obj.lstDetail = SetGoiThauDetail(obj.lstGoiThau);
    obj.isDieuChinh = bIsDieuChinh;
    if (obj.isDieuChinh == 1) {
        obj.isDieuChinh = true;
    }

    $.ajax({
        url: "/QLVonDauTu/KHLuaChonNhaThau/OnSave",
        type: "POST",
        data: { data: obj },
        dataType: "json",
        async: false,
        success: function (result) {
            if (result.bIsComplete) {
                alert("Cập nhật dữ liệu thành công !");
                location.href = "/QLVonDauTu/KHLuaChonNhaThau";
            } else {
                if (result.messError)
                    alert(result.messError);
                else
                    alert("Có lỗi xảy ra khi lưu dữ liệu !");
            }
        }
    })

}

function ValidateKHLCNT() {
    var sMessError = [];
    if ($("#cbxLoaiChungTu").val() == undefined || $("#cbxLoaiChungTu").val() == null) {
        sMessError.push("Chưa chọn căn cứ !");
    }
    if ($("#sSoQuyetDinh").val() == undefined || $("#sSoQuyetDinh").val() == null || $("#sSoQuyetDinh").val().trim() == "") {
        sMessError.push("Chưa điền số quyết định !");
    }
    if ($("#dNgayQuyetDinh").val() == undefined || $("#dNgayQuyetDinh").val() == null || $("#dNgayQuyetDinh").val().trim() == "") {
        sMessError.push("Chưa điền ngày quyết định !");
    }
    if ($("#iID_DonViQuanLyID").val() == undefined || $("#iID_DonViQuanLyID").val() == null) {
        sMessError.push("Chưa chọn đơn vị quản lý !");
    }
    if ($("#iID_DuAnID").val() == undefined || $("#iID_DuAnID").val() == null) {
        sMessError.push("Chưa chọn dự án !");
    }
    if ($(".sTenGoiThau ").val() == "" || $(".sTenGoiThau ").val() == undefined || $(".sTenGoiThau").val() == null) {
        sMessError.push("Tên gói thầu chưa được chọn !");
    }
    //if ($("#fGiaTri ").val() == "" || $(".fGiaTri ").val() == undefined || $(".fGiaTri").val() == null) {
    //    sMessError.push("Giá trị gói thầu chưa được chọn !");
    //}
    //if ($("#iThoiGianThucHien ").val() == "" || $("#iThoiGianThucHien ").val() == undefined || $("#iThoiGianThucHien").val() == null) {
    //    sMessError.push("Thời gian thực hiện chưa được chọn !");
    //}   

    if ($('input:radio[name="rd_ChungTu"]:checked').length == 0) {
        sMessError.push("Chưa chọn chứng từ !");
    }
    if ($("#tblGoiThau tr").not(".error-row").length == 0) {
        sMessError.push("Chưa nhập gói thầu !");
    }
    if (sMessError.length != 0) {
        alert(sMessError.join("\n"));
        return false;
    }
    return true;
}

function SetKeHoachLuaChonNhaThau() {
    //if (bIsDieuChinh == 1) {
    //    bIsDieuChinh = true
    //}
    var iLoaiChungTu = $("#cbxLoaiChungTu").val();
    var obj = {};
    if (!bIsDieuChinh) {
        obj.Id = $("#iIdKeHoachLuaChonNhaThau").val();
    } else {
        obj.Id = guidEmpty;
        obj.iID_ParentID = $("#iIdKeHoachLuaChonNhaThau").val();
    }
    obj.sSoQuyetDinh = $("#sSoQuyetDinh").val();
    obj.dNgayQuyetDinh = $("#dNgayQuyetDinh").val();
    obj.iID_DonViQuanLyID = $("#iID_DonViQuanLyID").find(":selected").data("iiddonvi");
    obj.iID_MaDonViQuanLy = $("#iID_DonViQuanLyID").val();
    obj.iID_DuAnID = $("#iID_DuAnID").val();
    obj.sMoTa = $("#sMoTa").val();
    if (iLoaiChungTu == iTypeDuToan)
        obj.iID_DuToanID = $('input:radio[name="rd_ChungTu"]:checked').val();
    else
        obj.iID_QDDauTuID = $('input:radio[name="rd_ChungTu"]:checked').val();
    return obj;
}

function SetGoiThau() {
    var lstGoiThau = [];
    $.each($("#tblGoiThau tr").not(".error-row"), function (index, item) {
        objGoiThau = {};
        objGoiThau.iID_GoiThauID = $(item).data("id");
        objGoiThau.iID_DuAnID = $("#iID_DuAnID").val();
        //objGoiThau.sTenGoiThau = $(item).find("#sTenGoiThauinput").val();
        objGoiThau.sTenGoiThau = $(item).find(".sTenGoiThau").val();
        objGoiThau.sHinhThucChonNhaThau = $(item).find(".sHinhThucChonNhaThau").val();
        objGoiThau.sPhuongThucDauThau = $(item).find(".sPhuongThucDauThau").val();
        objGoiThau.sThoiGianTCLCNT = $(item).find(".sThoiGianTCLCNT").val();
        objGoiThau.sHinhThucHopDong = $(item).find(".sHinhThucHopDong").val();
        //objGoiThau.iThoiGianThucHien = $(item).find("#iThoiGianThucHien input").val();
        objGoiThau.iThoiGianThucHien = $(item).find(".iThoiGianThucHien").val();
        var sGiaTri = $(item).find(".fGiaTri").text().replaceAll(".", "");
        if (sGiaTri != "")
            objGoiThau.fTienTrungThau = parseFloat(sGiaTri);
        else
            objGoiThau.fTienTrungThau = 0;
        objGoiThau.iID_KHLCNhaThau = $("#iIdKeHoachLuaChonNhaThau").val();
        lstGoiThau.push(objGoiThau);
    });
    return lstGoiThau;
}

function SetGoiThauDetail(lstGoiThau) {
    var lstDetail = [];

    $.each(lstGoiThau, function (index, item) {

        // nguon von
        if (lstChungTuGoiThau[item.iID_GoiThauID] == null) return false;

        if (lstChungTuGoiThau[item.iID_GoiThauID][0] != null) {
            lstChungTuGoiThau[item.iID_GoiThauID][0].forEach(function (child) {
                var objNguonVon = {};
                objNguonVon.iID_GoiThauID = item.iID_GoiThauID;
                objNguonVon.iID_NguonVonID = child.iID_NguonVonID;
                objNguonVon.fGiaTriPheDuyet = child.fGiaTriPheDuyet;
                objNguonVon.fGiaTriGoiThau = child.fGiaTriGoiThau;
                lstDetail.push(objNguonVon);
            });
        }

        if (lstChungTuGoiThau[item.iID_GoiThauID][1] != null) {
            lstChungTuGoiThau[item.iID_GoiThauID][1].forEach(function (child) {
                var objNguonVon = {};
                objNguonVon.iID_GoiThauID = item.iID_GoiThauID;
                objNguonVon.iID_ChiPhiID = child.iID_ChiPhiID;
                objNguonVon.iID_ParentId = child.iID_ParentId;
                objNguonVon.sNoiDung = child.sNoiDung;
                objNguonVon.fGiaTriPheDuyet = child.fGiaTriPheDuyet;
                objNguonVon.fGiaTriGoiThau = child.fGiaTriGoiThau;
                lstDetail.push(objNguonVon);
            });
        }

        if (lstChungTuGoiThau[item.iID_GoiThauID][2] != null) {
            lstChungTuGoiThau[item.iID_GoiThauID][2].forEach(function (child) {
                var objNguonVon = {};
                objNguonVon.iID_GoiThauID = item.iID_GoiThauID;
                objNguonVon.iID_ChiPhiID = child.iID_ChiPhiID;
                objNguonVon.iID_HangMucID = child.iID_HangMucID;
                objNguonVon.iID_ParentId = child.iID_ParentId;
                objNguonVon.sNoiDung = child.sNoiDung;
                objNguonVon.sMaOrder = child.sMaOrder;
                objNguonVon.iThuTu = child.iThuTu;
                objNguonVon.fGiaTriPheDuyet = child.fGiaTriPheDuyet;
                objNguonVon.fGiaTriGoiThau = child.fGiaTriGoiThau;
                lstDetail.push(objNguonVon);
            });
        }
    });

    return lstDetail;
}

function CancelSaveData() {
    window.location.href = "/QLVonDauTu/KHLuaChonNhaThau/Index";
}

function DetailGoiThau(id) {
    window.location.href = "/QLVonDauTu/QLThongTinGoiThau/Detail/"+id;
}