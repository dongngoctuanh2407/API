
var TBL_THONGTIN_THANHTOAN = 'tblListTTThanhToan';
var arrError = [];
var idContentView = "lstDataView";
var sUrlListView = "/DeNghiThanhToan/MucNganSachSearch";
var _paging = {};
var data_search = {};
var tt_thanhtoan = {};
var thanhtoan_chitiet = {};
var arr_thanhtoan_chitiet = [];
var type_action = 0; // 0: Thêm mới, 1 Sửa đổi, 2 Xem
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var id_denghithanhtoan = GUID_EMPTY;
var listIDChiTietXoa = [];
var ERROR = 1;
var CONFIRM = 0;

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
            $("#modalMucLucNganSach").html(data);
        }
    });
}

function LoadThongTinCreate() {
    $('#dhopdong').hide();
    $('#dhodongusd').hide();
    $('#dhodongvnd').hide();
    $('#dduan').hide();
    $('#ddutoanusd').hide();
    $('#ddutoanvnd').hide();
}

function LoadThongTinUpdate() {
    $('#iloainoidung').val($('#hiloainoidung').val());
    $('#itenchuongtrinh').val($('#hitenchuongtrinh').val());
    $('#ichudautu').val($('#hichudautu').val());
    $('#ithanhtoantheo').val($('#hithanhtoantheo').val());
    $('#itenhopdong').val($('#hitenhopdong').val());
    $('#itenduan').val($('#hitenduan').val());
    $('#iloaidenghi').val($('#hiloaidenghi').val());
    $('#inamngansach').val($('#hinamngansach').val());
    $('#icoquanthanhtoan').val($('#hicoquanthanhtoan').val());
    $('#itigiadenghi').val($('#hitigiadenghi').val());
    $('#itigiapheduyet').val($('#hitigiapheduyet').val());
    $('#itrangthai').val($('#hitrangthai').val());
    $('#itendonvihuongthu').val($('#hitendonvihuongthu').val());

    if ($('#hithanhtoantheo').val() != null && $('#hithanhtoantheo').val() == 1) {
        $('#dhopdong').show();
        $('#dhodongusd').show();
        $('#dhodongvnd').show();
        $('#dduan').hide();
        $('#ddutoanusd').hide();
        $('#ddutoanvnd').hide();
    } else if ($('#hithanhtoantheo').val() == 2) {
        $('#dhopdong').hide();
        $('#dhodongusd').hide();
        $('#dhodongvnd').hide();
        $('#dduan').show();
        $('#ddutoanusd').show();
        $('#ddutoanvnd').show();
    }
    else {
        $('#dhopdong').hide();
        $('#dhodongusd').hide();
        $('#dhodongvnd').hide();
        $('#dduan').hide();
        $('#ddutoanusd').hide();
        $('#ddutoanvnd').hide();
    }
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
                LoadThongTinLuyKe(0);
            }
        }
    })
}

function onChangeThanhToanTheo() {
    var inoidungthanhtoan = $('#ithanhtoantheo').val();
    if (inoidungthanhtoan == 1) {
        $('#dhopdong').show();
        $('#dhodongusd').show();
        $('#dhodongvnd').show();
        $('#dduan').hide();
        $('#ddutoanusd').hide();
        $('#ddutoanvnd').hide();

    } else if (inoidungthanhtoan == 2) {
        $('#dduan').show();
        $('#ddutoanusd').show();
        $('#ddutoanvnd').show();
        $('#dhodongusd').hide();
        $('#dhodongvnd').hide();
        $('#dhopdong').hide();
    }
    else {
        $('#dhodongusd').hide();
        $('#dhodongvnd').hide();
        $('#ddutoanusd').hide();
        $('#ddutoanvnd').hide();
        $('#dduan').hide();
        $('#dhopdong').hide();
    }
}

function onChangeChuDauTu() {
    var iID_NhiemVuChi = $('#itenchuongtrinh').val();
    var iID_ChuDauTu = $('#ichudautu').val();
    $('#itenduan option').remove();
    $.ajax({
        url: "/DeNghiThanhToan/GetThongTinDuAn",
        type: "POST",
        data: {
            iID_NhiemVuChi: iID_NhiemVuChi,
            iID_ChuDauTu: iID_ChuDauTu
        },
        dataType: "json",
        async: false,
        success: function (result) {
            if (result.status == true) {
                var str = " <option value =''>--Chọn tên dự án--</option>";
                $.each(result.data, function (index, val) {
                    str = str + "<option value = '" + val.ID + "'>" + val.sTenDuAn + "</option>";
                });
                $('#itenduan').append(str);
                LoadThongTinLuyKe(0);
            }
        }
    })
}

function onChangeNhiemVuChi() {
    var iID_NhiemVuChi = $('#itenchuongtrinh').val();
    $('#itenhopdong option').remove();
    $.ajax({
        url: "/DeNghiThanhToan/GetThongHopDong",
        type: "POST",
        async: false,
        data: { iID_NhiemVuChi: iID_NhiemVuChi },
        dataType: "json",
        success: function (result) {
            if (result.status == true) {
                var str = "<option value =''>-- Chọn hợp đồng --</option>";
                $.each(result.data, function (index, val) {
                    str = str + "<option value = '" + val.ID + "'>" + val.sTenHopDong + "</option>";
                });
                $('#itenhopdong').append(str);
                onChangeChuDauTu();
               
            }
        }
    })
}

function onChangeHopDong() {
    var idHopDong = $('#itenhopdong').val();
    $.ajax({
        url: "/DeNghiThanhToan/GetHopDongByID",
        type: "POST",
        data: { id: idHopDong },
        dataType: "json",
        success: function (data) {
            if (data.status == true && data.status != null) {
                $("#igtrhopdongusd").val(FormatNumber(data.result.fGiaTriUSD, 2));
                $("#igtrhopdongvnd").val(FormatNumber(data.result.fGiaTriVND, 0));
                LoadThongTinLuyKe(0);
            }
        }
    })
   
}

function onChangeDuAn() {
    var idDuAn = $('#itenduan').val();
    $.ajax({
        url: "/DeNghiThanhToan/GetDuAnByID",
        type: "POST",
        data: { id: idDuAn },
        dataType: "json",
        success: function (data) {
            if (data.status == true && data.status != null) {
                $("#iddutoanusd").val(FormatNumber(data.result.fGiaTriUSD));
                $("#iddutoanvnd").val(FormatNumber(data.result.fGiaTriVND));
                LoadThongTinLuyKe(0);
            }
        }
    })

}

function onChangeLoaiDeNghi() {
      $('#icoquanthanhtoan' + ' option').remove();
    var loaithanhtoan = $('#iloaidenghi').val();
    var str = " <option value=''> -- Chọn cơ quan thanh toán -- </option>";
    if (loaithanhtoan == 1) {
        str = str + " <option value = '2' selected> Đơn vị cấp  </option>";
    }
    else {
        str = str + " <option value = '1' selected> CTC cấp </option>";
        str = str + " <option value = '2' selected> Đơn vị cấp  </option>";
    }
    $('#icoquanthanhtoan').append(str);
}

function ThemDuLieuChiTiet() {
    var str = "<tr>";
    /* Mục lục ngân sách */
    str = str + "<td id='imuclucngansach' data-ingansach = '' onclick='ShowMucLucNganSach(this)' ></td>";
    /* Nội dung chi */
    str = str + "<td><input class='form-control' id='inoidungchi'></td>";
    /* Số kinh phí đề nghị thanh toán kỳ này USD */
    str = str + "<td><input class='form-control text-right' onkeyup='ValidateNumberKeyUp(this)'  onkeypress='return ValidateNumberKeyPress(this, event);' onblur='UpdateTongThongTinThanhToan(this)' id='sdenghiusd'></td>";
    /* Số kinh phí đề nghị thanh toán kỳ này VND */
    str = str + "<td><input class='form-control text-right' onkeyup='ValidateNumberKeyUp(this)' onkeypress='return ValidateNumberKeyPress(this, event);'  id='sdenghivnd'></td>";
    /* Số kinh phí phê duyệt kỳ này usd */
    str = str + "<td><input class='form-control text-right' onkeyup='ValidateNumberKeyUp(this)' onkeypress='return ValidateNumberKeyPress(this, event);' onblur='UpdateTongThongTinThanhToan(this)' id='sduocduyetusd'></td>";
    /* Số kinh phí phê duyệt kỳ ày vnd */
    str = str + "<td><input class='form-control text-right' onkeyup='ValidateNumberKeyUp(this)' onkeypress='return ValidateNumberKeyPress(this, event);'  id='sduocduyetvnd'></td>";
    /* Thao tác */
    str = str + "<td class ='text-center'><button class='btn-delete btn-icon' onclick='XoaDong(this, \"" + TBL_THONGTIN_THANHTOAN + "\")' type='button'><span class='fa fa-trash-o fa-lg' aria-hidden='true'></span></button></td>";

    str = str + "</tr>";

    $('#tblListTTThanhToan tbody').append(str);
}
function XoaDong(nutXoa, idBang) {
    var dongXoa = nutXoa.parentElement.parentElement;
    var iddong = $(dongXoa).data('idchitiet');
    if (iddong != null) {
        listIDChiTietXoa.push(iddong);
    }
    dongXoa.parentNode.removeChild(dongXoa);
    UpdateTongThongTinThanhToan(nutXoa);
}

function onChangeTiGiaPheDuyet() {
    var tigia = $('#itigiapheduyet').val();
    $('#tblListTTThanhToan tbody tr').each(function (index, tr) {
        var sotiennhap = $(tr).find('#sduocduyetusd').val();
        $.ajax({
            type: "POST",
            url: "/QLNH/DeNghiThanhToan/GetChuyenDoiTyGia",
            data: { matygia: tigia, sotiennhap: parseInt(UnFormatNumber(sotiennhap)), loaitiennhap: 1 },
            async: false,
            success: function (data) {
                $(tr).find('#sduocduyetvnd').val(FormatNumber(data.chuyendoi, 2));
            }
        });
        UpdateTongThongTinThanhToan(this);

    })
}

function UpdateTongThongTinThanhToan(td_dong) {

    //Check tỷ giá đề nghị, tỷ giá quy đổi
    var id_dong = $(td_dong).attr('id');
    var parentElemnt = td_dong.parentElement.parentElement
    var status = false;
    var loaitiennhap = 1;
    var matygia = GUID_EMPTY;
    var sotiennhap = $(parentElemnt).find('#' + id_dong).val();

    if (sotiennhap != undefined && sotiennhap != "") {
        sotiennhap = parseInt(UnFormatNumber($(parentElemnt).find('#' + id_dong).val()));
        if (id_dong == "sdenghiusd" || id_dong == "sdenghivnd") {
            if ($("#itigiadenghi").val() == '') {
                arrError.push("Chưa chọn tỷ giá đề nghị. Vui lòng chọn tỷ giá để thực hiện quy đổi");
                showErr(CONFIRM);
                $(td_dong).val("");
            }
            else {
                if (id_dong == "sdenghivnd") {
                    loaitiennhap = 2;
                }
                matygia = $('#itigiadenghi').val();
                status = true;
            }
        }
        else {
            if (id_dong == "sduocduyetusd" || id_dong == "sduocduyetvnd") {
                if ($("#itigiapheduyet").val() == '') {
                    arrError.push("Chưa chọn tỷ giá phê duyệt.Vui lòng chọn tỷ giá để thực hiện quy đổi");
                    showErr(CONFIRM);
                    $(td_dong).val("");
                }
                else {
                    if (id_dong == "sduocduyetvnd") {
                        loaitiennhap = 2;
                    }
                    matygia = $('#itigiapheduyet').val();
                    status = true;
                }
            }
        }
    }
    else {
        if (id_dong == "sdenghiusd") {
            $(parentElemnt).find('#sdenghivnd').val("");
        }
        if (id_dong == "sdenghivnd") {
            $(parentElemnt).find('#sdenghiusd').val("");
        }
        if (id_dong == "sduocduyetusd") {
            $(parentElemnt).find('#sduocduyetvnd').val("");
        }
        if (id_dong == "sduocduyetvnd") {
            $(parentElemnt).find('#sduocduyetusd').val("");
        }
    }

    if (status) {
        $.ajax({
            type: "POST",
            url: "/QLNH/DeNghiThanhToan/GetChuyenDoiTyGia",
            data: { matygia: matygia, sotiennhap: parseInt(UnFormatNumber(sotiennhap)), loaitiennhap: loaitiennhap },
            async: false,
            success: function (data) {
                if (id_dong == "sdenghiusd") {
                    $(parentElemnt).find('#sdenghivnd').val(FormatNumber(data.chuyendoi, 0));
                    $(parentElemnt).find('#sdenghiusd').val(FormatNumber(sotiennhap, 2));
                }
                if (id_dong == "sdenghivnd") {
                    $(parentElemnt).find('#sdenghiusd').val(FormatNumber(data.chuyendoi, 2));
                    $(parentElemnt).find('#sdenghivnd').val(FormatNumber(sotiennhap, 0));
                }
                if (id_dong == "sduocduyetusd") {
                    $(parentElemnt).find('#sduocduyetvnd').val(FormatNumber(data.chuyendoi, 0));
                    $(parentElemnt).find('#sduocduyetusd').val(FormatNumber(sotiennhap, 2));
                }
                if (id_dong == "sduocduyetvnd") {
                    $(parentElemnt).find('#sduocduyetusd').val(FormatNumber(data.chuyendoi, 2));
                    $(parentElemnt).find('#sduocduyetvnd').val(FormatNumber(sotiennhap, 0));
                }
            }
        });
    }
    //Cập nhập tổng số
    var tong_sdenghiusd = 0;
    var tong_sdenghivnd = 0;
    var tong_sduocduyetusd = 0;
    var tong_sduocduyetvnd = 0;
    $('#tblListTTThanhToan tbody tr').each(function () {
        var sdenghiusd = $(this).find('#sdenghiusd').val();
        var sdenghivnd = $(this).find('#sdenghivnd').val();
        var sduocduyetusd = $(this).find('#sduocduyetusd').val();
        var sduocduyetvnd = $(this).find('#sduocduyetvnd').val();

        tong_sdenghiusd = tong_sdenghiusd + parseFloat(UnFormatNumber(sdenghiusd == "" ? 0 : sdenghiusd));
        tong_sdenghivnd = tong_sdenghivnd + parseInt(UnFormatNumber(sdenghivnd == "" ? 0 : sdenghivnd));
        tong_sduocduyetusd = tong_sduocduyetusd + parseFloat(UnFormatNumber(sduocduyetusd == "" ? 0 : sduocduyetusd));
        tong_sduocduyetvnd = tong_sduocduyetvnd + parseInt(UnFormatNumber(sduocduyetvnd == "" ? 0 : sduocduyetvnd));
    })

    $('#stongdenghiusd').text(FormatNumber(tong_sdenghiusd, 2));
    $('#stongdenghivnd').text(FormatNumber(tong_sdenghivnd, 0));
    $('#stongduocduyetusd').text(FormatNumber(tong_sduocduyetusd, 2));
    $('#stongduocduyetvnd').text(FormatNumber(tong_sduocduyetvnd, 0));
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

function ShowMucLucNganSach(td_dong) {
    td_dong_ngansach = td_dong;
    $('#modalMucLucNganSach').modal('toggle');
    $('#modalMucLucNganSach').modal('show');

    sUrlListView = '/QLNH/DeNghiThanhToan/MucNganSachSearch';
    idContentView = 'modalMucLucNganSach';

    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/DeNghiThanhToan/MucNganSachSearch",
        data: { data: data_search },
        success: function (data) {
            $("#modalMucLucNganSach").html(data);

        }
    });
}

function ChonMucLucNganSach(tr_muclucngansach) {
    var value_item = $(tr_muclucngansach).find('td').data('dong');
    var value_mangansach = $(tr_muclucngansach).find('td').data('idmuclucngansach');
    $(td_dong_ngansach).html(value_item);
    $(td_dong_ngansach).attr("data-ingansach", value_mangansach);
    $('#modalMucLucNganSach').modal('hide');
}

function onChangeTiGiaDeNghi() {
    var tigia = $('#itigiadenghi').val();
    $('#tblListTTThanhToan tbody tr').each(function (index, tr) {
        var sotiennhap = $(tr).find('#sdenghiusd').val();
        $.ajax({
            type: "POST",
            url: "/QLNH/DeNghiThanhToan/GetChuyenDoiTyGia",
            data: { matygia: tigia, sotiennhap: parseInt(UnFormatNumber(sotiennhap)), loaitiennhap: 1 },
            async: false,
            success: function (data) {
                $(tr).find('#sdenghivnd').val(FormatNumber(data.chuyendoi, 2));
            }
        });
        UpdateTongThongTinThanhToan(this);

    })
}

function onChangeTiGiaPheDuyet() {
    var tigia = $('#itigiapheduyet').val();
    $('#tblListTTThanhToan tbody tr').each(function (index, tr) {
        var sotiennhap = $(tr).find('#sduocduyetusd').val();
        $.ajax({
            type: "POST",
            url: "/QLNH/DeNghiThanhToan/GetChuyenDoiTyGia",
            data: { matygia: tigia, sotiennhap: parseInt(UnFormatNumber(sotiennhap)), loaitiennhap: 1 },
            async: false,
            success: function (data) {
                $(tr).find('#sduocduyetvnd').val(FormatNumber(data.chuyendoi, 0));
            }
        });
        UpdateTongThongTinThanhToan(this);

    })
}

function LoadThongTinLuyKe(type) {
    var id = $('#hidthanhtoan').val();
    var idonvi = $('#idonvi').val();
    var itenchuongtrinh = $('#itenchuongtrinh').val();
    var ichudautu = $('#ichudautu').val();
    var itenhopdong = $('#itenhopdong').val();
    var itenduan = $('#itenduan').val();

    $.ajax({
        url: "/DeNghiThanhToan/LayThongTinLuyKe",
        type: "POST",
        dataType: "json",
        async: false,
        data: {
            id: id,
            idonvi: idonvi,
            inhiemvuchi: itenchuongtrinh,
            ichudautu: ichudautu,
            ihopdong: itenhopdong,
            iduan: itenduan
        },
        success: function (result) {
            if (result.status == true) {
                if (result.thanhtoan != null) {
                    if (type == 1) {
                        $('#iluykekinhphiusd').val(FormatNumber(result.thanhtoan.fLuyKeUSD + result.thanhtoan.fTongPheDuyet_USD, 2));
                    }
                    else if (type == 2) {
                        $('#iluykekinhphivnd').val(FormatNumber(result.thanhtoan.fLuyKeVND + result.thanhtoan.fTongPheDuyet_VND));
                    }
                    else {
                        $('#iluykekinhphiusd').val(FormatNumber(result.thanhtoan.fLuyKeUSD + result.thanhtoan.fTongPheDuyet_USD, 2));
                        $('#iluykekinhphivnd').val(FormatNumber(result.thanhtoan.fLuyKeVND + result.thanhtoan.fTongPheDuyet_VND));
                    }
                }
                else {
                    $('#iluykekinhphiusd').val('');
                    $('#iluykekinhphivnd').val('');
                }
               
            }
        }
    })
}



function checkTrungMaDeNghi() {
    var sodenghi = $('#isodenghi').val();
    var id_denghithanhtoan = $('#hidthanhtoan').val();
    if (id_denghithanhtoan == '' || id_denghithanhtoan == null) {
        type_action = 0;
    }
    else {
        type_action = 1;

    }
    var result = false;
    $.ajax({
        type: "POST",
        dataType: "Json",
        url: "/QLNH/DeNghiThanhToan/CheckTrungMaDeNghi",
        data: { sodenghi: sodenghi, type_action: type_action, idenghi: id_denghithanhtoan },
        async: false,
        success: function (data) {
            if (data.status == true) {
                result = data.results;
            }
        }
    });
    return result;
}

function ValidateBeforSave() {
    arrError = [];
    if ($('#idonvi').val() == "" || $('#idonvi').val() == GUID_EMPTY) {
        arrError.push('Thông tin đơn vị không được bỏ trống.Vui lòng chọn thông tin đơn vị');
    }
    else if ($('#isodenghi').val() == "") {
        arrError.push('Thông tin số quyết định không được bỏ trống. Vui lòng nhập số quyết định');
    }
    else if ($('#iloainoidung').val() == "" || $('#iloainoidung').val() == GUID_EMPTY) {
        arrError.push('Thông tin loại nội dung chi không được bỏ trống. Vui lòng chọn loại nội dung chi');
    }
    else if ($('#itenchuongtrinh').val() == "" || $('#itenchuongtrinh').val() == GUID_EMPTY) {
        arrError.push('Thông tin tên chương trình không được bỏ trống.Vui lòng chọn tên chương trình');
    }
    else if ($('#ichudautu').val() == "" || $('#ichudautu').val() == GUID_EMPTY) {
        arrError.push('Thông tin chủ đầu tư không được bot trống. Vui lòng chọn chủ đầu tư');
    }
    else if ($('#ithanhtoantheo').val() == "" || $('#ithanhtoantheo').val() == GUID_EMPTY) {
        arrError.push('Thông tin thanh toán không được bỏ trống. Vui lòng chọn loại thanh toán"');
    }
    else if ($('#iloaidenghi').val() == "" || $('#iloaidenghi').val() == GUID_EMPTY) {
        arrError.push('Thông tin loại đề nghị không được bỏ trống. Vui lòng chọn loại đề nghị');
    }
    else if ($('#inamkehoach').val() == "" || $('#iinamkehoach').val() == GUID_EMPTY) {
        arrError.push('Thông tin năm kế hoạch không được bỏ trống, vui lòng chọn năm kế hoạch');
    }
    else if ($('#inamngansach').val() == "" || $('#inamngansach').val() == GUID_EMPTY) {
        arrError.push('Thông tin năm ngân sách không được bỏ trống. Vui lòng chọn năm ngân sách');
    }
    else if ($('#icoquanthanhtoan').val() == "" || $('#icoquanthanhtoan').val() == GUID_EMPTY) {
        arrError.push('Thông tin cơ quan thanh toán không được bỏ trống. Vui lòng chọn cơ quan thanh toán');
    }
    else if ($('#itigiadenghi').val() == "" || $('#itigiadenghi').val() == GUID_EMPTY) {
        arrError.push('Thông tin tỷ giá đề nghị không được bỏ trống. Vui lòng chọn tỷ giá đề nghị');
    }
    else if ($('#itigiapheduyet').val() == "" || $('#itigiapheduyet').val() == GUID_EMPTY) {
        arrError.push('Thông tin tỷ giá phê duyệt không được bỏ trống. Vui lòng chọn tỷ giá phê duyệt');
    }
    else if ($('#itrangthai').val() == "" || $('#itrangthai').val() == GUID_EMPTY) {
        arrError.push('Thông tin trạng thai không được bỏ trống. Vui lòng chọn trạng thái');
    }
    else if ($('#itendonvihuongthu').val() == "" || $('#itendonvihuongthu').val() == GUID_EMPTY) {
        arrError.push('Thông tin tên đơn vị hưởng thụ không được bỏ trống. Vui lòng chọn tên đơn vị hưởng thụ ');
    }
    else if ($('#ichuyenkhoanso').val() == "" || $('#ichuyenkhoanso').val() == GUID_EMPTY) {
        arrError.push('Thông tin chuyển khoản bằng số không được bỏ trống. Vui lòng nhập thông tin chuyển khoản');
    }
    else if (checkTrungMaDeNghi() == true) {
        arrError.push('Thông tin số đề nghị bị trùng');
    }

    if (arrError.length > 0) {
        return false;
    }
    else {
        return true;
    }
}

function GetData() {
    //lấy thông tin dự án
    /*  $("<div/>").text($('#isodenghi').val()).html();*/
    tt_thanhtoan.ID = $('#hidthanhtoan').val();
    tt_thanhtoan.iID_DonVi = $('#idonvi').val();
    tt_thanhtoan.iID_MaDonVi = $('#idonvi').find(':selected').data('madonvi');
    tt_thanhtoan.sSoDeNghi = $('#isodenghi').val();
    tt_thanhtoan.dNgayDeNghi = $('#ingaydenghi').val();
    tt_thanhtoan.iLoaiNoiDungChi = $('#iloainoidung').val();
    tt_thanhtoan.iID_KHCTBQP_NhiemVuChiID = $('#itenchuongtrinh').val();
    tt_thanhtoan.iID_ChuDauTuID = $('#ichudautu').val();
    tt_thanhtoan.iID_MaChuDauTu = $('#ichudautu').find(':selected').data('machudautu');
    tt_thanhtoan.iThanhToanTheo = $('#ithanhtoantheo').val();
    if (tt_thanhtoan.iThanhToanTheo == 1) {
        tt_thanhtoan.iID_HopDongID = $('#itenhopdong').val();
        tt_thanhtoan.iID_DuAnID = null;
    }
    if (tt_thanhtoan.iThanhToanTheo == 2) {
        tt_thanhtoan.iID_DuAnID = $('#itenduan').val();
        tt_thanhtoan.iID_HopDongID = null;
    }
    if (tt_thanhtoan.iThanhToanTheo == 3) {
        tt_thanhtoan.iID_DuAnID = null;
        tt_thanhtoan.iID_HopDongID = null;
    }
    tt_thanhtoan.sLuyKeUSD = $('#iluykekinhphiusd').val() == "" ? $('#iluykekinhphiusd').val() : UnFormatNumber($('#iluykekinhphiusd').val());
    tt_thanhtoan.sLuyKeVND = $('#iluykekinhphivnd').val() == "" ? $('#iluykekinhphivnd').val() : UnFormatNumber($('#iluykekinhphivnd').val());
    tt_thanhtoan.iLoaiDeNghi = $('#iloaidenghi').val();
    tt_thanhtoan.iNamKeHoach = $('#inamkehoach').val();
    tt_thanhtoan.iNamNganSach = $('#inamngansach').val();
    tt_thanhtoan.iCoQuanThanhToan = $('#icoquanthanhtoan').val();
    tt_thanhtoan.iID_TiGiaDeNghiID = $('#itigiadenghi').val();
    tt_thanhtoan.iID_TiGiaPheDuyetID = $('#itigiapheduyet').val();
    tt_thanhtoan.iTrangThai = $('#itrangthai').val();
    tt_thanhtoan.iID_NhaThauID = $('#itendonvihuongthu').val();
    tt_thanhtoan.fChuyenKhoan_BangSo = $('#ichuyenkhoanso').val() == "" ? $('#ichuyenkhoanso').val() : UnFormatNumber($('#ichuyenkhoanso').val())
    tt_thanhtoan.sChuyenKhoan_BangChu = $('#ibangchu').val();
    tt_thanhtoan.sSoTaiKhoan = $('#isotaikhoan').val();
    tt_thanhtoan.sNganHang = $('#itai').val();
    tt_thanhtoan.sCanCu = $('#icancu').val();

    //Lấy thông tin chi tiết thanh toán
    $('#tblListTTThanhToan tbody tr').each(function (index, tr) {
        var thanhtoan_chitiet = {};
        var ID = $(tr).data('idchitiet');
        var iID_MucLucNganSachID = $(tr).find('#imuclucngansach').data('ingansach');
        var iID_MLNS_ID = $(tr).find('#imuclucngansach').val();
        var sTenNoiDungChi = $(tr).find('#inoidungchi').val();
        var sDeNghiCapKyNay_USD = $(tr).find('#sdenghiusd').val() == "" ? "" : UnFormatNumber($(tr).find('#sdenghiusd').val());
        var sDeNghiCapKyNay_VND = $(tr).find('#sdenghivnd').val() == "" ? "" : UnFormatNumber($(tr).find('#sdenghivnd').val());
        var sPheDuyetCapKyNay_USD = $(tr).find('#sduocduyetusd').val() == "" ? "" : UnFormatNumber($(tr).find('#sduocduyetusd').val());
        var sPheDuyetCapKyNay_VND = $(tr).find('#sduocduyetvnd').val() == "" ? "" : UnFormatNumber($(tr).find('#sduocduyetvnd').val());

        if (iID_MLNS_ID != "" || sTenNoiDungChi != "" || sDeNghiCapKyNay_USD != "" || sDeNghiCapKyNay_VND != "" || sPheDuyetCapKyNay_USD != "" || sPheDuyetCapKyNay_VND != "" || ID != "") {
            thanhtoan_chitiet.ID = ID;
            thanhtoan_chitiet.iID_MucLucNganSachID = iID_MucLucNganSachID;
            thanhtoan_chitiet.iID_MLNS_ID = iID_MLNS_ID;
            thanhtoan_chitiet.sTenNoiDungChi = sTenNoiDungChi;
            thanhtoan_chitiet.sDeNghiCapKyNay_USD = sDeNghiCapKyNay_USD;
            thanhtoan_chitiet.sDeNghiCapKyNay_VND = sDeNghiCapKyNay_VND;
            thanhtoan_chitiet.sPheDuyetCapKyNay_USD = sPheDuyetCapKyNay_USD;
            thanhtoan_chitiet.sPheDuyetCapKyNay_VND = sPheDuyetCapKyNay_VND;
            arr_thanhtoan_chitiet.push(thanhtoan_chitiet);
        }
    })
}

function SaveData() {
    if (ValidateBeforSave()) {
        GetData();
        data = {
            TT_ThanToan: tt_thanhtoan,
            lstTTThanToan_ChiTiet: arr_thanhtoan_chitiet
        }
        $.ajax({
            url: "/DeNghiThanhToan/SaveDeNghiThanhToan",
            type: "POST",
            async: false,
            data: { data: data, listIDChiTietXoa: listIDChiTietXoa.join(',') },
            success: function (r) {
                if (r == "True") {
                    window.location.href = "/QLNH/DeNghiThanhToan";
                }
                else {
                    arrError.push('Lỗi lưu dữ liệu');
                    showErr(CONFIRM);
                }
            }
        })
    }
    else {
        showErr(ERROR);
    }
}

function CancelSaveData() {
    window.location.href = "/QLNH/DeNghiThanhToan";
}