var data_search = {};
var idContentView = "lstDataView";
var sUrlListView = "DeNghiThanhToan/DeNghiThanhToanSearch";
var _paging = {};
var arrIdDeNghiThanhToan = [];
var type_baocao = 0;
var CONFIRM = 0;
var ERROR = 1;

function ChangePage(iCurrentPage = 1) {
    data_search = {};
    _paging.CurrentPage = iCurrentPage;
    data_search._paging = _paging;
    data_search.iID_DonVi = $('#idonvi').val();
    data_search.sSoDeNghi = $('#ssodenghi').val();
    data_search.dNgayDeNghi = $('#sngaydenghi').val();
    data_search.iLoaiNoiDungChi = $('#iloaidenghi').val();
    data_search.iID_ChuDauTuID = $('#ichudautu').val();
    data_search.iID_KHCTBQP_NhiemVuChiID = $('#itenchuongtrinh').val();
    data_search.iNamKeHoach = $('#inamkehoach').val();
    data_search.iNamNganSach = $('#inamngansach').val();
    data_search.iCoQuanThanhToan = $('#icoquanthanhtoan').val();
    data_search.iID_NhaThauID = $('#idonvihuongthu').val();
    data_search.iTrangThai = $('#idtrangthai').val();
    GetListData(iCurrentPage);
}

function ResetChangePage(iCurrentPage = 1) {
    data_search = {};
    _paging.CurrentPage = iCurrentPage;
    data_search._paging = _paging;
    GetListData(iCurrentPage);
}


function GetListData(iCurrentPage) {
    $.ajax({
        type: "POST",
        dataType: "html",
        url: sUrlListView,
        data: { data: data_search },
        success: function (data) {
            $("#lstDataView").html(data);
            AddConditionSearch();
        }
    });
}

function AddConditionSearch() {
    $('#idonvi').val(data_search.iID_DonVi);
    $('#ssodenghi').val(data_search.sSoDeNghi);
    $('#sngaydenghi').val(data_search.dNgayDeNghi);
    $('#iloaidenghi').val(data_search.iLoaiNoiDungChi);
    $('#ichudautu').val(data_search.iID_ChuDauTuID);
    $('#itenchuongtrinh').val(data_search.iID_KHCTBQP_NhiemVuChiID);
    $('#inamkehoach').val(data_search.iNamKeHoach);
    $('#inamngansach').val(data_search.iNamNganSach);
    $('#icoquanthanhtoan').val(data_search.iCoQuanThanhToan);
    $('#idonvihuongthu').val(data_search.iID_NhaThauID);
    $('#idtrangthai').val(data_search.iTrangThai);
}

function onChangeDonVi() {
    $('#itenchuongtrinh' + ' option').remove();
    var iIDonVi = $('#idonvi').val();
    $.ajax({
        url: "/DeNghiThanhToan/GetAllNhiemVuChiByDonVi",
        type: "POST",
        data: { iDDonVi: iIDonVi },
        dataType: "json",
        async: false,
        success: function (result) {
            if (result.status == true) {
                var str = "<option value =''>--Chọn chương trình--</option>";
                $.each(result.data, function (index, val) {
                    str = str + "<option value = '" + val.ID + "'>" + val.sTenNhiemVuChi + "</option>";
                });
                $('#itenchuongtrinh').append(str);
            }
        }
    })
}

function onModalThongBaoXoa(id) {
    $.ajax({
        type: "POST",
        url: "/QLNH/DeNghiThanhToan/onThongBaoXoa",
        data: { id: id },
        success: function (data) {
            $('#modalDeNghiThanhToanView').modal('toggle');
            $('#modalDeNghiThanhToanView').modal('show');
            $("#modalDeNghiThanhToanView").html(data);
        }
    });
}


function Insert() {
    window.location.href = "/QLNH/DeNghiThanhToan/Insert";
}

function Update(id) {
    window.location.href = "/QLNH/DeNghiThanhToan/Update?id="+id;
}

function Detail(id) {
    window.location.href = "/QLNH/DeNghiThanhToan/Detail?id=" + id;
}

function DeleteData(id) {
    $.ajax({
        url: "/DeNghiThanhToan/DeleteDeNghiThanhToan",
        type: "POST",
        data: { id: id },
        success: function (r) {
            if (r == "True") {
                ResetChangePage(1);
            }
            else {
                arrError.push('Xóa dữ liệu không thành công');
                $.ajax({
                    type: "POST",
                    url: "/Modal/OpenModal",
                    data: { Title: "Thông báo", Messages: arrError, Category: CONFIRM },
                    success: function (data) {
                        $("#divModalConfirm").html(data);
                    }
                });
            }
        }
    })

}
function ViewShowChoiceBaoCao() {
    if ($('.dDenghithanhtoan').is(':visible')) {
        $('.dDenghithanhtoan').css('display', 'none');
    }
    else {
        $('.dDenghithanhtoan').css('display', 'block');
    }

}

function ChoiceAllCheckBox() {
    //Nếu đang check thì thực hiện all uncheck
    var checkAll = $('#tblListDeNghiThanhToan').find('.checkbox-header');
    if ($(checkAll).is(":checked")) {
        $("#tblListDeNghiThanhToan tbody tr").each(function (index, tr) {
            var checkbox = $(tr).find('.checkbox');
            $(checkbox).prop("checked", true);
        });
    }
    else {
        $("#tblListDeNghiThanhToan tbody tr").each(function (index, tr) {
            var checkbox = $(tr).find('.checkbox');
            $(checkbox).prop("checked", false);
        });
    }
}

function onModalBaoCao(type) {
    arrError = [];
    type_baocao = type;
    var arrIdDeNghiThanhToan = [];
    $("#tblListDeNghiThanhToan tbody tr").each(function (index, tr) {
        var checkbox = $(tr).find('.checkbox');
        if ($(checkbox).is(":checked")) {
            arrIdDeNghiThanhToan.push($(tr).data('idthanhtoan'));
        }
    });
    if (arrIdDeNghiThanhToan.length > 0) {
        $.ajax({
            type: "POST",
            url: "/QLNH/DeNghiThanhToan/OnModalBaoCao",
            data: { type: type },
            success: function (data) {
                $('#modalDeNghiThanhToanView').modal('toggle');
                $('#modalDeNghiThanhToanView').modal('show');
                $("#modalDeNghiThanhToanView").html(data);
                if (type == 1 || type == 2) {
                    $('#ingaythang').hide();
                    $('#icancu').hide();
                    $('#ithoigian').hide();
                    $('#iquanly').hide();
                }
                if (type == 3) {
                    $('#ingaythang').hide();
                    $('#iquanly').hide();
                }
                if (type == 4 || type == 5) {
                    $('#iphong').hide();
                    $('#icancu').hide();
                    $('#ithoigian').hide();
                }
            }
        });
    }
    else {
        arrError.push("Chưa chọn đơn đề nghị thanh toán");
        showErr(CONFIRM);
    }
}

function onInBaoCao(type_xuatbaocao) {
    var arrLink = [];
    var arrIdDeNghiThanhToan = [];
    if (ValidateInBaoCao()) {
        $("#tblListDeNghiThanhToan tbody tr").each(function (index, tr) {
            var checkbox = $(tr).find('.checkbox');
            if ($(checkbox).is(":checked")) {
                arrIdDeNghiThanhToan.push($(tr).data('idthanhtoan'));
            }
        });
        var idPhongBan = $('#iID_MaPhongBan').val();
        var sNoiDung = $('#sNoiDung').val();
        var dvt = $('#iDonViTinh').val();
        if (type_baocao == 1 || type_baocao == 2) {
            for (i = 0; i < arrIdDeNghiThanhToan.length; i++) {
                var idThanhtoan = arrIdDeNghiThanhToan[i];
                arrLink.push("/QLNH/DeNghiThanhToan/ExportGiayDeNghiThanhToan?idThanhtoan=" + idThanhtoan + "&idPhongBan=" + idPhongBan + "&sNoiDung=" + sNoiDung
                    + "&dvt=" + dvt + "&type=" + type_baocao + "&ext=" + type_xuatbaocao);
            }
        }
        if (type_baocao == 3) {
            var lstIdThanhToan = arrIdDeNghiThanhToan.join(',');
            var sCanCu = $('#iCanCu').val();
            var thang = $('#ithang').val();
            var quy = $('#iquy').val();
            var nam = $('#inam').val();

            arrLink.push("/QLNH/DeNghiThanhToan/ExportThongBaoChiNganSach?idPhongBan=" + idPhongBan + "&nam=" + nam + "&thang=" + thang + "&quy=" + quy + "&ext=" + type_xuatbaocao
                + "&lstIdThanhToan=" + lstIdThanhToan + "&sNoiDung=" + sNoiDung + "&sCanCu=" + sCanCu + "&dvt=" + dvt);
        }
        if (type_baocao == 4 || type_baocao == 5) {
            var lstIdThanhToan = arrIdDeNghiThanhToan.join(',');
            var sCanCu = $('#iCanCu').val();
            var dtungay = $('#itungay').val();
            var ddenngay = $('#idenngay').val();
            var iquanly = $('#iID_QuanLy').val();

            arrLink.push("/QLNH/DeNghiThanhToan/ExportThongBaoCapKinhPhi?lstIdThanhToan=" + lstIdThanhToan + "&tungay=" + dtungay + "&denngay=" + ddenngay + "&sNoiDung=" + sNoiDung
                + "&idquanly=" + iquanly + "&dvt=" + dvt + "&ext=" + type_xuatbaocao + "&type=" + type_baocao);
        }

        openLinks(arrLink);
    }
    else {
        showErr(CONFIRM);
    }

}

function ValidateInBaoCao() {
    arrError = [];
    if (type_baocao == 1 || type_baocao == 2) {
        if ($("#iID_MaPhongBan").val() == "" || $("#iID_MaPhongBan").val() == null) {
            arrError.push("Thông tin phòng ban chưa được chọn");
        } else if ($("#sNoiDung").val() == "") {
            arrError.push("Nội dung không được để trống");
        } else if ($("#iDonViTinh").val() == "" || $("#iDonViTinh").val() == null) {
            arrError.push("Thông tin đơn vị tính chưa được chọn");
        }
    }
    if (type_baocao == 3) {
        if ($("#iID_MaPhongBan").val() == "" || $("#iID_MaPhongBan").val() == null) {
            arrError.push("Thông tin phòng ban chưa được chọn");
        } else if ($("#sNoiDung").val() == "") {
            arrError.push("Nội dung không được để trống");
        } else if ($("#iDonViTinh").val() == "" || $("#iDonViTinh").val() == null) {
            arrError.push("Thông tin đơn vị tính chưa được chọn");
        }
        else if ($("#iCanCu").val() == "") {
            arrError.push("Nội dung căn cứ không được bỏ trống");
        }
    }
    if (type_baocao == 4 || type_baocao == 5) {
        if ($("#itungay").val() == "" || $("#itungay").val() == null) {
            arrError.push("Thông tin từ ngày không được bỏ trống");
        } else if ($("#idenngay").val() == "" || $("#idenngay").val() == null) {
            arrError.push("Thông tin đến ngày không được bỏ trống");
        } else if ($("#iID_QuanLy").val() == "" || $("#iID_QuanLy").val() == null) {
            arrError.push("Thông tin phòng ban chưa được chọn");
        } else if ($("#sNoiDung").val() == "") {
            arrError.push("Nội dung không được để trống");
        } else if ($("#iDonViTinh").val() == "" || $("#iDonViTinh").val() == null) {
            arrError.push("Thông tin đơn vị tính chưa được chọn");
        }
        else {

        }
    }

    if (arrError.length > 0) {
        return false;
    } else { return true; }
}

function onChangeBaoCaoThang() {
    var thang = $('#ithang').val();
    if (thang == 1 || thang == 2 || thang == 3) {
        $('#iquy').val(1);
    }
    if (thang == 4 || thang == 5 || thang == 6) {
        $('#iquy').val(2);
    }
    if (thang == 7 || thang == 8 || thang == 9) {
        $('#iquy').val(3);
    }
    if (thang == 10 || thang == 11 || thang == 12) {
        $('#iquy').val(4);
    }
}
function showErr(CONFIRM) {
    $.ajax({
        type: "POST",
        url: "/Modal/OpenModal",
        data: { Title: "Thông báo", Messages: arrError, Category: CONFIRM },
        success: function (data) {
            $("#divModalConfirm").html(data);
            arrError = [];
        }
    });
}