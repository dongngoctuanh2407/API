var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';

function GetBaoCao() {
    $("#btnGetBaoCao").attr("disabled", "disabled");
    var iID_DonViQuanLyID = $("#iID_DonViQuanLyID").val();
    var sTenDuAn = $("#txtTenDuAn").val();
    var iNamKeHoach = $("#iNamKeHoach").val();
    $.ajax({
        type: "POST",
        url: "/QLVonDauTu/TongHopThongTinDuAn/GetThongTinTongHopDuAn",
        data: { iID_DonViQuanLyID: iID_DonViQuanLyID, sTenDuAn: sTenDuAn, iNamKeHoach: iNamKeHoach },
        success: function (data) {
            $("#showData").html(data);
            TinhLaiDongTong();
            var sTenDonViQL = $("#iID_DonViQuanLyID :selected").html();
            $("#id_donviquanly").html(sTenDonViQL);
            $("#contentReport").removeClass('hidden');
            $("#btnGetBaoCao").removeAttr("disabled");
        }
    });
}

function TinhLaiDongTong() {
    var tongGiaTriDauTu = 0;
    var tongTongMucDauTu = 0;
    var tongLuyKeVonNamTruoc = 0;
    var tongKeHoachVonNamNay = 0;
    var tongDaThanhToan = 0;
    var tongChuaThanhToan = 0;
    var tongLuyKeVonNamNay = 0;
    var tongGiaTriQuyetToan = 0;

    $("#baocaoTHThongTinDuAn" + " tbody tr").each(function (index, row) {
        var fGiaTriDauTu = $(row).find(".r_fGiaTriDauTu").html();
        if (fGiaTriDauTu != undefined && $.isNumeric(UnFormatNumber(fGiaTriDauTu))) {
            tongGiaTriDauTu += parseFloat(UnFormatNumber(fGiaTriDauTu));
        }
        
        var fTongMucDauTu = $(row).find(".r_fTongMucDauTu").html();
        if (fTongMucDauTu != undefined && $.isNumeric(UnFormatNumber(fTongMucDauTu))) {
            tongTongMucDauTu += parseFloat(UnFormatNumber(fTongMucDauTu));
        }

        var fLuyKeVonNamTruoc = $(row).find(".r_fLuyKeVonNamTruoc").html();
        if (fLuyKeVonNamTruoc != undefined && $.isNumeric(UnFormatNumber(fLuyKeVonNamTruoc))) {
            tongLuyKeVonNamTruoc += parseFloat(UnFormatNumber(fLuyKeVonNamTruoc));
        }

        var fKeHoachVonNamNay = $(row).find(".r_fKeHoachVonNamNay").html();
        if (fKeHoachVonNamNay != undefined && $.isNumeric(UnFormatNumber(fKeHoachVonNamNay))) {
            tongKeHoachVonNamNay += parseFloat(UnFormatNumber(fKeHoachVonNamNay));
        }

        var fDaThanhToan = $(row).find(".r_fDaThanhToan").html();
        if (fDaThanhToan != undefined && $.isNumeric(UnFormatNumber(fDaThanhToan))) {
            tongDaThanhToan += parseFloat(UnFormatNumber(fDaThanhToan));
        }

        var fChuaThanhToan = $(row).find(".r_fChuaThanhToan").html();
        if (fChuaThanhToan != undefined && $.isNumeric(UnFormatNumber(fChuaThanhToan))) {
            tongChuaThanhToan += parseFloat(UnFormatNumber(fChuaThanhToan));
        }

        var fLuyKeVonNamNay = $(row).find(".r_fLuyKeVonNamNay").html();
        if (fLuyKeVonNamNay != undefined && $.isNumeric(UnFormatNumber(fLuyKeVonNamNay))) {
            tongLuyKeVonNamNay += parseFloat(UnFormatNumber(fLuyKeVonNamNay));
        }

        var fGiaTriQuyetToan = $(row).find(".r_fGiaTriQuyetToan").html();
        if (fGiaTriQuyetToan != undefined && $.isNumeric(UnFormatNumber(fGiaTriQuyetToan))) {
            tongGiaTriQuyetToan += parseFloat(UnFormatNumber(fGiaTriQuyetToan));
        }
    })

    $("#baocaoTHThongTinDuAn" + " .tong_GiaTriDauTu").html(tongGiaTriDauTu.toLocaleString('vi-VN'));
    $("#baocaoTHThongTinDuAn" + " .tong_TongMucDauTu").html(tongTongMucDauTu.toLocaleString('vi-VN'));
    $("#baocaoTHThongTinDuAn" + " .tong_LuyKeVonNamTruoc").html(tongLuyKeVonNamTruoc.toLocaleString('vi-VN'));
    $("#baocaoTHThongTinDuAn" + " .tong_KeHoachVonNamNay").html(tongKeHoachVonNamNay.toLocaleString('vi-VN'));
    $("#baocaoTHThongTinDuAn" + " .tong_DaThanhToan").html(tongDaThanhToan.toLocaleString('vi-VN'));
    $("#baocaoTHThongTinDuAn" + " .tong_ChuaThanhToan").html(tongChuaThanhToan.toLocaleString('vi-VN'));
    $("#baocaoTHThongTinDuAn" + " .tong_LuyKeVonNamNay").html(tongLuyKeVonNamNay.toLocaleString('vi-VN'));
    $("#baocaoTHThongTinDuAn" + " .tong_GiaTriQuyetToan").html(tongGiaTriQuyetToan.toLocaleString('vi-VN'));

}

function getDataReport() {
    var count = $("#showData tbody").children().length;
    console.log(count);
    var dataTongReport = [];
    var dataReport = [];

    $("#baocaoTHThongTinDuAn" + " tbody tr").each(function (indexR, row) {
        var lstChild = [];
        var ttDuAn = {};
        if (indexR < 2) {
            $(row).find("td").each(function (index) {
                switch (index) {
                    case 0:
                        ttDuAn.stt = $(this).text();
                        break;
                    case 1:
                        ttDuAn.sTenDuAn = $(this).text();
                        break;
                    case 2:
                        ttDuAn.sTienTe = $(this).text();
                        break;
                    case 3:
                        ttDuAn.sSoQuyetDinhCTDT = $(this).text();
                        break;
                    case 4:
                        ttDuAn.dNgayQuyetDinhCTDT = $(this).text();
                        break;
                    case 5:
                        ttDuAn.fGiaTriDauTu = UnFormatNumber($(this).text());
                        break;
                    case 6:
                        ttDuAn.sSoQuyetDinhQDDT = $(this).text();
                        break;
                    case 7:
                        ttDuAn.dNgayQuyetDinhQDDT = $(this).text();
                        break;
                    case 8:
                        ttDuAn.fTongMucDauTu = UnFormatNumber($(this).text());
                        break;
                    case 9:
                        ttDuAn.fLuyKeVonNamTruoc = UnFormatNumber($(this).text());
                        break;
                    case 10:
                        ttDuAn.fKeHoachVonNamNay = UnFormatNumber($(this).text());
                        break;
                    case 11:
                        ttDuAn.fDaThanhToan = UnFormatNumber($(this).text());
                        break;
                    case 12:
                        ttDuAn.fChuaThanhToan = UnFormatNumber($(this).text());
                        break;
                    case 13:
                        ttDuAn.fLuyKeVonNamNay = UnFormatNumber($(this).text());
                        break;
                    case 14:
                        ttDuAn.sSoQuyetDinhQT = $(this).text();
                        break;
                    case 15:
                        ttDuAn.dNgayQuyetDinhQT = $(this).text();
                        break;
                    case 16:
                        ttDuAn.fGiaTriQuyetToan = UnFormatNumber($(this).text());
                        break;
                    default:

                }

                lstChild.push($(this).text());
            });
            dataReport.push(ttDuAn);
        }
    });
    console.log(dataReport);

    exportBCTongHopThongTinDuAn(dataReport);
}

function exportBCTongHopThongTinDuAn(dataReport) {
    $.ajax({
        type: "POST",
        url: "/QLVonDauTu/TongHopThongTinDuAn/ExportBCTongHopTTDuAn",
        data: { dataReport: dataReport },
        success: function (r) {
            if (r) {
                var iID_DonViQuanLyID = $("#iID_DonViQuanLyID").val();
                var sTenDuAn = $("#txtTenDuAn").val();
                var iNamKeHoach = $("#iNamKeHoach").val();
                window.location.href = "/QLVonDauTu/TongHopThongTinDuAn/ExportExcel?iID_DonViQuanLyID=" + iID_DonViQuanLyID + '&sTenDuAn=' + sTenDuAn + '&iNamKeHoach=' + iNamKeHoach;
            }
        }
    });
}
