var TBL_CHI_PHI_DAU_TU = "tblChiPhiDauTu";
var TBL_NGUON_VON_DAU_TU = "tblNguonVonDauTu";
var TBL_HANG_MUC_CHINH = "tblHangMucChinh";
var TBL_TAI_LIEU_DINH_KEM = "tblThongTinTaiLieuDinhKem";
var arrChiPhi = [];
var arrNguonVon = [];
var arrChenhLech = [];
var trLoaiCongTrinh;
var typeSearch = 1;
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var CONFIRM = 0;
var ERROR = 1;

$(document).ready(function ($) {
    $("#dNgayQuyetDinh").change(function () {
        var iID_DonViQuanLyID = $("#iID_DonViQuanLyID").val();

        if (this.value != "" && iID_DonViQuanLyID != "" && iID_DonViQuanLyID != null && iID_DonViQuanLyID != GUID_EMPTY) {
            LayDanhSachDuAnTheoDonViQuanLyVaNgayQuyetDinh(iID_DonViQuanLyID, this.value)
            ClearThongTinDuAn();
        } else {
            $("#iID_DuAnID option:not(:first)").remove();
            $("#iID_DuAnID").trigger("change");
        }
    });

    $("#iID_DonViQuanLyID").change(function () {
        var dNgayQuyetDinh = $("#dNgayQuyetDinh").val();

        if (this.value != "" && this.value != GUID_EMPTY && dNgayQuyetDinh != "") {
            LayDanhSachDuAnTheoDonViQuanLyVaNgayQuyetDinh(this.value, dNgayQuyetDinh)
            ClearThongTinDuAn();
        } else {
            $("#iID_DuAnID option:not(:first)").remove();
            $("#iID_DuAnID").trigger("change");
        }
    });

    $("#iID_DuAnID").change(function (e) {
        if (this.value != "" && this.value != GUID_EMPTY) {
            LayThongTinDuAn(this.value);
        } else {
            ClearThongTinDuAn();
        }
        XoaThemMoiChiPhi();
        XoaThemMoiNguonVon();
    });

    $("#iID_ChiPhiID").change(function (e) {
        var dNgayQuyetDinh = $("#dNgayQuyetDinh").val();
        var iID_DuAnID = $("#iID_DuAnID").val();

        if (this.value != "" && this.value != GUID_EMPTY && iID_DuAnID != null && iID_DuAnID != GUID_EMPTY && dNgayQuyetDinh != "") {
            GetGiaTriDuToan(iID_DuAnID, this.value, dNgayQuyetDinh);
        } else {
            $("#fGiaTriDuToan").text('---');
        }
    });

    $("#iID_MaNguonNganSach").change(function (e) {
        var dNgayQuyetDinh = $("#dNgayQuyetDinh").val();
        var iID_DuAnID = $("#iID_DuAnID").val();

        if (this.value != "" && this.value != GUID_EMPTY && iID_DuAnID != null && iID_DuAnID != GUID_EMPTY && dNgayQuyetDinh != "") {
            GetGiaTriDuToanNguonVon(iID_DuAnID, this.value, dNgayQuyetDinh);
        } else {
            $("#fGiaTriDuToanNguonVon").text('---');
        }
    });

    var fHanMucDauTu = FormatNumber($("#fHanMucDauTu").val());
    $("#fHanMucDauTu").val(fHanMucDauTu);
});

function ThemMoiChiPhiDauTu() {
    var tChiPhi = $("#iID_ChiPhiID :selected").html();
    var iIdChiPhi = $("#iID_ChiPhiID").val();
    var tGiaTriQuyetToan = $("#txtAddCpdtGiaTriQuyetToan").val();

    var messErr = [];
    if (iIdChiPhi == GUID_EMPTY) {
        messErr.push("Thông tin chi phí chưa có hoặc chưa chính xác.");
    }
    if (tGiaTriQuyetToan == "" || parseInt(UnFormatNumber(tGiaTriQuyetToan)) <= 0) {
        messErr.push("Giá trị quyết toán của chi phí phải lớn hơn 0.");
    }

    if (messErr.length > 0) {
        var Title = 'Lỗi thêm mới chi phí đầu tư';
        $.ajax({
            type: "POST",
            url: "/Modal/OpenModal",
            data: { Title: Title, Messages: messErr, Category: ERROR },
            success: function (data) {
                $("#divModalConfirm").html(data);
            }
        });
    } else {
        if (arrChiPhi.filter(function (x) { return x.iID_ChiPhiID == iIdChiPhi }).length > 0) {
            $("#" + TBL_CHI_PHI_DAU_TU + " tbody tr").each(function (index, row) {
                if ($(row).find(".r_iID_ChiPhi").val() == iIdChiPhi) {
                    $(row).remove();
                }
            })
            arrChiPhi = arrChiPhi.filter(function (x) { return x.iID_ChiPhiID != iIdChiPhi });
        }

        var dongMoi = "";
        dongMoi += "<tr style='cursor: pointer;'>";
        dongMoi += "<td align='center' class='r_STT'></td>";
        dongMoi += "<td><input type='hidden' class='r_iID_ChiPhi' value='" + iIdChiPhi + "'/>" + tChiPhi + "</td>";
        dongMoi += "<td class='r_GiaTriPheDuyet' align='right'>" + (tGiaTriQuyetToan == "" ? 0 : tGiaTriQuyetToan) + "</td>";
        dongMoi += "<td align='center'><button class='btn-edit btn-icon' type='button' onclick='SuaDong(this, \"" + TBL_CHI_PHI_DAU_TU + "\")'>" +
            "<i class='fa fa-pencil-square-o fa-lg' aria-hidden='true'></i>" +
            "</button> ";
        dongMoi += "<button class='btn-delete btn-icon' type='button' onclick='XoaDong(this, \"" + TBL_CHI_PHI_DAU_TU + "\")'>" +
            "<i class='fa fa-trash-o fa-lg' aria-hidden='true'></i>" +
            "</button> </td>";
        dongMoi += "</tr>";

        $("#" + TBL_CHI_PHI_DAU_TU + " tbody").append(dongMoi);
        CapNhatCotStt(TBL_CHI_PHI_DAU_TU);
        TinhLaiDongTong(TBL_CHI_PHI_DAU_TU);

        arrChiPhi.push({
            iID_ChiPhiID: iIdChiPhi,
            fTienPheDuyet: tGiaTriQuyetToan.replaceAll('.', '')
        })

        // xoa text data them moi
        XoaThemMoiChiPhi();
    }
}

function ThemMoiNguonVonDauTu() {
    var tNguonVon = $("#iID_MaNguonNganSach :selected").html();
    var iIdNguonVon = $("#iID_MaNguonNganSach").val();
    var tGiaTriPheDuyet = $("#txtAddNvdtGiaTriQuyetToan").val();

    var messErr = [];
    if (iIdNguonVon == 0) {
        messErr.push("Thông tin nguồn vốn chưa có hoặc chưa chính xác.");
    }
    if (tGiaTriPheDuyet == "" || parseInt(UnFormatNumber(tGiaTriPheDuyet)) <= 0) {
        messErr.push("Giá trị phê duyệt của nguồn vốn phải lớn hơn 0.");
    }

    if (messErr.length > 0) {
        var Title = 'Lỗi thêm mới nguồn vốn đầu tư';
        $.ajax({
            type: "POST",
            url: "/Modal/OpenModal",
            data: { Title: Title, Messages: messErr, Category: ERROR },
            success: function (data) {
                $("#divModalConfirm").html(data);
            }
        });
        //alert(messErr.join("\n"));
    } else {
        if (arrNguonVon.filter(function (x) { return x.iID_NguonVonID == iIdNguonVon }).length > 0) {
            $("#" + TBL_NGUON_VON_DAU_TU + " tbody tr").each(function (index, row) {
                if ($(row).find(".r_iID_NguonVon").val() == iIdNguonVon) {
                    $(row).remove();
                }
            })
            arrNguonVon = arrNguonVon.filter(function (x) { return x.iID_NguonVonID != iIdNguonVon });
        }

        var dongMoi = "";
        dongMoi += "<tr style='cursor: pointer;'>";
        dongMoi += "<td align='center' class='r_STT'></td>";
        dongMoi += "<td><input type='hidden' class='r_iID_NguonVon' value='" + iIdNguonVon + "'/>" + tNguonVon + "</td>";
        dongMoi += "<td class='r_GiaTriPheDuyet' align='right'>" + (tGiaTriPheDuyet == "" ? 0 : tGiaTriPheDuyet) + "</td>";
        dongMoi += "<td align='center'><button class='btn-edit btn-icon' type='button' onclick='SuaDong(this, \"" + TBL_NGUON_VON_DAU_TU + "\")'>" +
            "<i class='fa fa-pencil-square-o fa-lg' aria-hidden='true'></i>" +
            "</button> ";
        dongMoi += "<button class='btn-delete btn-icon' type='button' onclick='XoaDong(this, \"" + TBL_NGUON_VON_DAU_TU + "\")'>" +
            "<span class='fa fa-trash-o fa-lg' aria-hidden='true'></span>" +
            "</button></td>";
        dongMoi += "</tr>";

        $("#" + TBL_NGUON_VON_DAU_TU + " tbody").append(dongMoi);
        CapNhatCotStt(TBL_NGUON_VON_DAU_TU);
        TinhLaiDongTong(TBL_NGUON_VON_DAU_TU);

        arrNguonVon.push({
            iID_NguonVonID: iIdNguonVon,
            fTienPheDuyet: tGiaTriPheDuyet.replaceAll('.', '')
        })

        // xoa text data them moi
        XoaThemMoiNguonVon();
        GetNoiDungQuyetToan(arrNguonVon);
    }
}

function GetNoiDungQuyetToan(arrNguonVon) {
    var dNgayQuyetDinh = $("#dNgayQuyetDinh").val();
    var iID_DonViQuanLyID = $("#iID_DonViQuanLyID").val();
    var iID_DuAnID = $("#iID_DuAnID").val();
    var tongGiaTriPheDuyet = $("#" + TBL_NGUON_VON_DAU_TU + " .cpdt_tong_giatripheduyet").html().replaceAll('.', '');
    var data = {
        arrNguonVon: arrNguonVon,
        iID_DonViQuanLyID: iID_DonViQuanLyID,
        iID_DuAnID: iID_DuAnID,
        dNgayQuyetDinh: dNgayQuyetDinh,
        tongGiaTriPheDuyet: tongGiaTriPheDuyet
    };
    $.ajax({
        url: "/QLVonDauTu/QLPheDuyetQuyetToan/GetNoiDungQuyetToan",
        type: "POST",
        dataType: "json",
        data: { data: data },
        success: function (r) {
            if (r.bIsComplete) {
                $("#fTongGiaTriPhanBo").text(r.data.fTongGiaTriPhanBo != 0 ? FormatNumber(r.data.fTongGiaTriPhanBo) : 0);
                $("#tongGiaTriPheDuyet").text(r.data.tongGiaTriPheDuyet != 0 ? FormatNumber(r.data.tongGiaTriPheDuyet) : 0);
                $("#fTongGiaTriChenhLech").text(r.data.fTongGiaTriChenhLech != 0 ? FormatNumber(r.data.fTongGiaTriChenhLech) : 0);
                arrChenhLech = r.data.lstNoiDungQuyetToan;
                GetListDataNoiDungQuyetToan(r.data.lstNoiDungQuyetToan);
            }
        }
    })
}

function GetListDataNoiDungQuyetToan(lstNoiDungQuyetToan) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLVonDauTu/QLPheDuyetQuyetToan/GetListDataNoiDungQuyetToan",
        data: { lstNoiDungQuyetToan: lstNoiDungQuyetToan },
        success: function (data) {
            $("#lstDataView").html(data);
        }
    });
}

function CapNhatCotStt(idBang) {
    $("#" + idBang + " tbody tr").each(function (index, tr) {
        $(tr).find('.r_STT').text(index + 1);
    });
}

function TinhLaiDongTong(idBang) {
    var tongGiaTriPheDuyet = 0;

    $("#" + idBang + " .r_GiaTriPheDuyet").each(function () {
        tongGiaTriPheDuyet += parseInt(UnFormatNumber($(this).html()));
    });

    $("#" + idBang + " .cpdt_tong_giatripheduyet").html(FormatNumber(tongGiaTriPheDuyet));
}

function XoaDong(nutXoa, idBang) {
    var dongXoa = nutXoa.parentElement.parentElement;
    if (idBang == TBL_CHI_PHI_DAU_TU) {
        var iID_ChiPhiID = $(dongXoa).find(".r_iID_ChiPhi").val();
        arrChiPhi = arrChiPhi.filter(function (x) { return x.iID_ChiPhiID != iID_ChiPhiID });
    } else if (idBang == TBL_NGUON_VON_DAU_TU) {
        var iID_NguonVonID = $(dongXoa).find(".r_iID_NguonVon").val();
        arrNguonVon = arrNguonVon.filter(function (x) { return x.iID_NguonVonID != iID_NguonVonID });
    } else if (idBang == TBL_HANG_MUC_CHINH) {
        var iId_PTHMCHangMuc = $(dongXoa).find(".r_iID_HangMucChinh").val();
        arrChenhLech = arrChenhLech.filter(function (x) { return x.iId_PTHMCHangMuc != iId_PTHMCHangMuc });
    }
    dongXoa.parentNode.removeChild(dongXoa);
    CapNhatCotStt(idBang);
    TinhLaiDongTong(idBang);
}
/*NinhNV start*/

function Luu() {
    if (CheckLoi()) {
        var quyetToan = {};
        var data = {};

        quyetToan.iID_QuyetToanID = $("#iID_QuyetToanID").val();
        quyetToan.sSoQuyetDinh = $("#sSoQuyetDinh").val();
        quyetToan.dNgayQuyetDinh = $("#dNgayQuyetDinh").val();
        quyetToan.sCoQuanPheDuyet = $("#sCoQuanPheDuyet").val();
        quyetToan.sNguoiKy = $("#sNguoiKy").val();
        quyetToan.iID_DonViQuanLyID = $("#iID_DonViQuanLyID").val();
        quyetToan.iID_DuAnID = $("#iID_DuAnID").val();
        quyetToan.sNoiDung = $("#sNoiDung").val();
        quyetToan.fTienQuyetToanPheDuyet = UnFormatNumber($("#id_tong_giatripheduyet_nguonvon").html());

        if (quyetToan.iID_QuyetToanID === GUID_EMPTY) {
        }

        data = {
            quyetToan: quyetToan,
            listQuyetToanChiPhi: arrChiPhi,
            listQuyetToanNguonVon: arrNguonVon,
            listNguonVonChenhLech: arrChenhLech
        };

        $.ajax({
            type: "POST",
            url: "/QLPheDuyetQuyetToan/QLPheDuyetQuyetToanSave",
            data: { data: data },
            success: function (r) {
                if (r == "True") {
                    window.location.href = "/QLVonDauTu/QLPheDuyetQuyetToan/Index";
                }
            }
        });
    }
}

function CheckLoi() {
    var sSoQuyetDinh = $("#sSoQuyetDinh").val();
    var dNgayQuyetDinh = $("#dNgayQuyetDinh").val();
    var iID_DonViQuanLyID = $("#iID_DonViQuanLyID").val();
    var iID_DuAnID = $("#iID_DuAnID").val();

    var messErr = [];
    if (sSoQuyetDinh == '') {
        messErr.push("Thông tin số quyết định phê duyệt chưa có hoặc chưa chính xác!");
    }
    if (dNgayQuyetDinh == '') {
        messErr.push("Thông tin ngày quyết định phê duyệt chưa có hoặc chưa chính xác!");
    }
    if (iID_DonViQuanLyID == GUID_EMPTY) {
        messErr.push("Thông tin đơn vị quản lý chưa có hoặc chưa chính xác!");
    }
    if (iID_DuAnID == GUID_EMPTY || iID_DuAnID == null) {
        messErr.push("Thông tin nội dung chưa có hoặc chưa chính xác!");
    }
    if (arrChiPhi.length == 0) {
        messErr.push("Thông tin giá trị chi phí phê duyệt chưa có hoặc chưa chính xác!");
    }
    if (arrNguonVon.length == 0) {
        messErr.push("Thông tin giá trị nguồn vốn phê duyệt chưa có hoặc chưa chính xác!");
    }
    
    var tongChiPHiPheDuyet = $("#id_tong_giatripheduyet_chiphi").html();
    var tongNguonVonPheDuyet = $("#id_tong_giatripheduyet_nguonvon").html();
    if (tongChiPHiPheDuyet != undefined && tongNguonVonPheDuyet != undefined && tongChiPHiPheDuyet != '' && tongNguonVonPheDuyet != '') {
        if (parseInt(UnFormatNumber(tongChiPHiPheDuyet)) != parseInt(UnFormatNumber(tongNguonVonPheDuyet))) {
            messErr.push("Giá trị phê duyệt của danh sách chi phí và nguồn vốn không bằng nhau!");
        }
    }
    if (messErr.length > 0) {
        var Title = 'Lỗi lưu thông tin phê duyệt quyết toán';
        $.ajax({
            type: "POST",
            url: "/Modal/OpenModal",
            data: { Title: Title, Messages: messErr, Category: ERROR },
            success: function (data) {
                $("#divModalConfirm").html(data);
            }
        });
        return false;
    } else {
        return true;
    }
}

function checkExistMaDuAn(sMaDuAn) {
    $.ajax({
        type: "POST",
        url: "/QLDuAn/CheckExistMaDuAn",
        data: { sMaDuAn: sMaDuAn },
        success: function (r) {
            if (r == "True") {
                alert("Mã dự án đã tồn tại!");
            }
        }
    });
}

function XoaThemMoiChiPhi() {
    // xoa text data them moi
    $("#iID_ChiPhiID").prop("selectedIndex", 0);
    $("#fGiaTriDuToan").text("---");
    $("#txtAddCpdtGiaTriQuyetToan").val("");
}

function XoaThemMoiNguonVon() {
    // xoa text data them moi
    $("#iID_MaNguonNganSach").prop("selectedIndex", 0);
    $("#fGiaTriDuToanNguonVon").text('---');
    $("#txtAddNvdtGiaTriQuyetToan").val("");
}


function CancelSaveData() {
    window.location.href = "/QLVonDauTu/QLPheDuyetQuyetToan/Index";
}

function SuaDong(nutSua, idBang) {
    var dongSua = nutSua.parentElement.parentElement;
    if (idBang == TBL_CHI_PHI_DAU_TU) {
        var iID_ChiPhiID = $(dongSua).find(".r_iID_ChiPhi").val();
        var fGiaTriPheDuyet = $(dongSua).find(".r_GiaTriPheDuyet").text();
        $("#iID_ChiPhiID").val(iID_ChiPhiID);
        $("#txtAddCpdtGiaTriQuyetToan").val(fGiaTriPheDuyet);
        $("#iID_ChiPhiID").trigger("change");
    } else if (idBang == TBL_NGUON_VON_DAU_TU) {
        var r_iID_NguonVon = $(dongSua).find(".r_iID_NguonVon").val();
        var fGiaTriPheDuyet = $(dongSua).find(".r_GiaTriPheDuyet").text();
        $("#iID_MaNguonNganSach").val(r_iID_NguonVon);
        $("#txtAddNvdtGiaTriQuyetToan").val(fGiaTriPheDuyet);
        $("#iID_MaNguonNganSach").trigger("change");
    } else if (idBang == TBL_HANG_MUC_CHINH) {
        var r_iID_HangMucChinh = $(dongSua).find(".r_iID_HangMucChinh").val();
        var r_txt_HangMucChinh = $(dongSua).find(".r_txt_HangMucChinh").text();
        var fGiaTriPheDuyet = $(dongSua).find(".r_GiaTriPheDuyet").text();

        $("#iID_HangMucChinh").val(r_iID_HangMucChinh);
        $("#txtAddPTHMCHangMuc").val(r_txt_HangMucChinh);
        $("#txtAddPTHMCGiaTriPheDuyet").val(fGiaTriPheDuyet);
    }
}

function LayDanhSachDuAnTheoDonViQuanLyVaNgayQuyetDinh(iID_DonViQuanLyID, dNgayQuyetDinh) {
    $.ajax({
        url: "/QLVonDauTu/QLPheDuyetQuyetToan/LayDanhSachDuAnTheoDonViQuanLyVaNgayQuyetDinh",
        type: "POST",
        data: { iID_DonViQuanLyID: iID_DonViQuanLyID, dNgayQuyetDinh: dNgayQuyetDinh },
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data != null && data != "") {
                $("#iID_DuAnID").html(data);
                $("#iID_DuAnID").prop("selectedIndex", 0);
            }
        },
        error: function (data) {

        }
    })
}

function LayThongTinDuAn(iID_DuAnID) {
    var dNgayQuyetDinh = $("#dNgayQuyetDinh").val();
    var iID_DonViQuanLyID = $("#iID_DonViQuanLyID").val();

    $.ajax({
        url: "/QLVonDauTu/QLPheDuyetQuyetToan/LayThongTinDuAn",
        type: "POST",
        dataType: "json",
        data: { iID_DonViQuanLyID: iID_DonViQuanLyID, iID_DuAnID: iID_DuAnID, dNgayQuyetDinh: dNgayQuyetDinh },
        success: function (r) {
            if (r.bIsComplete) {
                $("#sDiaDiem").text(r.data.sDiaDiem);
                var sKhoiCong = r.data.sKhoiCong != null ? r.data.sKhoiCong : '';
                var sKetThuc = r.data.sKetThuc != null ? r.data.sKetThuc : '';
                var sThoiGianTH = sKhoiCong + ' - ' + sKetThuc ;
                $("#sThoiGianTH").text(sThoiGianTH);
                $("#fTongMucDauTuPheDuyet").text(r.data.fTongMucDauTuPheDuyet != 0 ? FormatNumber(r.data.fTongMucDauTuPheDuyet) : 0);
                $("#fGiaTriUng").text(r.data.fGiaTriUng != 0 ? FormatNumber(r.data.fGiaTriUng) : 0);
                $("#fLKSoVonDaTamUng").text(r.data.fLKSoVonDaTamUng != 0 ? FormatNumber(r.data.fLKSoVonDaTamUng) : 0);
                $("#fLKThuHoiUng").text(r.data.fLKThuHoiUng != 0 ? FormatNumber(r.data.fLKThuHoiUng) : 0);
                $("#fConPhaiThuHoi").text(r.data.fConPhaiThuHoi != 0 ? FormatNumber(r.data.fConPhaiThuHoi) : 0);
                //$("#fLKKHVUDuocDuyet").val(r.data.fLKKHVUDuocDuyet > 0 ? FormatNumber(r.data.fLKKHVUDuocDuyet) : 0);
                //$("#fLKSoVonDaTamUng").val(r.data.fLKSoVonDaTamUng > 0 ? FormatNumber(r.data.fLKSoVonDaTamUng) : 0);
                //$("#fLKThuHoiUng").val(r.data.fLKThuHoiUng > 0 ? FormatNumber(r.data.fLKThuHoiUng) : 0);
            }
        }
    })
}

function ClearThongTinDuAn() {
    $("#sDiaDiem").text('---');
    $("#sThoiGianTH").text('---' + ' - ' + '---');
    $("#fTongMucDauTuPheDuyet").text('---');
    $("#fGiaTriUng").text('---');
    $("#fLKSoVonDaTamUng").text('---');
    $("#fLKThuHoiUng").text('---');
    $("#fConPhaiThuHoi").text('---');
}

function GetGiaTriDuToan(iID_DuAnID, iID_ChiPhiID, dNgayQuyetDinh) {
    $.ajax({
        type: "POST",
        url: "/QLVonDauTu/QLPheDuyetQuyetToan/GetGiaTriDuToan",
        data: { iID_DuAnID: iID_DuAnID, iID_ChiPhiID: iID_ChiPhiID, dNgayQuyetDinh: dNgayQuyetDinh },
        success: function (r) {
            if (r > 0) {
                $("#fGiaTriDuToan").text(FormatNumber(r));
            } else {
                $("#fGiaTriDuToan").text(r);
            }
        }
    });
}

function GetGiaTriDuToanNguonVon(iID_DuAnID, iID_MaNguonNganSach, dNgayQuyetDinh) {
    $.ajax({
        type: "POST",
        url: "/QLVonDauTu/QLPheDuyetQuyetToan/GetGiaTriDuToanNguonVon",
        data: { iID_DuAnID: iID_DuAnID, iID_MaNguonNganSach: iID_MaNguonNganSach, dNgayQuyetDinh: dNgayQuyetDinh },
        success: function (r) {
            if (r > 0) {
                $("#fGiaTriDuToanNguonVon").text(FormatNumber(r));
            } else {
                $("#fGiaTriDuToanNguonVon").text(r);
            }
        }
    });
}

function SetArrsData() {
    arrChiPhi = arrChiPhiTemp;
    CapNhatCotStt(TBL_CHI_PHI_DAU_TU);
    TinhLaiDongTong(TBL_CHI_PHI_DAU_TU);

    arrNguonVon = arrNguonVonTemp;
    CapNhatCotStt(TBL_NGUON_VON_DAU_TU);
    TinhLaiDongTong(TBL_NGUON_VON_DAU_TU);

    GetNoiDungQuyetToan(arrNguonVon);
}
/*NinhNV end*/