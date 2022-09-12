var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;
var CONFIRM = 0;
var modelNhiemVuChi = {};

$(document).ready(function () {
    
});

// Tìm kiếm
function GetListData(currentPage = 1) {
    _paging.CurrentPage = currentPage;
    var filter = {
        iID_KHCTBQP_ChuongTrinhID:  $("#iID_KHCTBQP_ChuongTrinhID").val() == GUID_EMPTY ? null : $("#iID_KHCTBQP_ChuongTrinhID").val(),
        iID_DonViID:                $("#iID_DonViID").val() == GUID_EMPTY ? null : $("#iID_DonViID").val(),
        sSoThongTri:                $("#txtSoThongTri").val()?.trim(),
        dNgayLap:                   $("#dNgayLap").val(),
        iNamThucHien:               $("#txtNamThucHien").val()?.trim(),
        iLoaiThongTri:              $("#iLoaiThongTri").val(),
        iLoaiNoiDungChi:            $("#iLoaiNoiDungChi").val()
    }

    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTriQuyetToan/TimKiem",
        data: { paging: _paging, filter: filter },
        success: function (data) {
            // View result
            $("#lstDataView").html(data);

            // Gán lại data cho filter
            $("#iID_KHCTBQP_ChuongTrinhID").val(filter.iID_KHCTBQP_ChuongTrinhID ? filter.iID_KHCTBQP_ChuongTrinhID : GUID_EMPTY)
            $("#iID_DonViID").val(filter.iID_DonViID ? filter.iID_DonViID : GUID_EMPTY)
            $("#txtSoThongTri").val(filter.sSoThongTri);
            $("#dNgayLap").val(filter.dNgayLap);
            $("#txtNamThucHien").val(filter.iNamThucHien);
            $("#iLoaiThongTri").val(filter.iLoaiThongTri);
            $("#iLoaiNoiDungChi").val(filter.iLoaiNoiDungChi);
        }
    });
}

// Làm mới danh sách
function ResetChangePage() {
    $("#iID_KHCTBQP_ChuongTrinhID").val(GUID_EMPTY)
    $("#iID_DonViID").val(GUID_EMPTY)
    $("#txtSoThongTri").val('');
    $("#dNgayLap").val('');
    $("#txtNamThucHien").val('');
    $("#iLoaiThongTri").val('');
    $("#iLoaiNoiDungChi").val('');

    GetListData(1);
}

// Open modal create, edit.
function OpenViewCreateThongTri(id, state) {
    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTriQuyetToan/ViewToCreateOrUpdate",
        data: { id: id, state: state },
        success: function (data) {
            // View result
            $("#lstDataView").html(data);
        }
    });
}

// Confirm xóa
function ConfirmDelete(id, sSoThongTri) {
    var Title = 'Xác nhận xóa Thông tri quyết toán';
    var Messages = [];
    Messages.push('Bạn có chắc chắn muốn xóa Thông tri quyết toán: ' + sSoThongTri + '?');
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

// Xóa thông tri quyết toán
function Delete(id) {
    $.ajax({
        type: "POST",
        url: "/QLNH/ThongTriQuyetToan/DeleteThongTriQuyetToan",
        data: { id: id },
        success: function (data) {
            if (data) {
                ChangePage(1);
            } else {
                var Title = 'Lỗi xóa thông tri quyết toán';
                $.ajax({
                    type: "POST",
                    url: "/Modal/OpenModal",
                    data: { Title: Title, Messages: ['Không xóa được dữ liệu!'], Category: ERROR },
                    success: function (res) {
                        $("#divModalConfirm").html(res);
                    }
                });
            }
        },
        error: function () {
            $("#confirmModal").modal('hide')
            setTimeout(function () {
                var Title = 'Lỗi xóa thông tri quyết toán';
                $.ajax({
                    type: "POST",
                    url: "/Modal/OpenModal",
                    data: { Title: Title, Messages: ['Không xóa được dữ liệu!'], Category: ERROR },
                    success: function (res) {
                        $("#divModalConfirm").html(res);
                    }
                });
            }, 500);
        }
    });
}

// event thay đổi đơn vị
function ChangeDonViSelect() {
    let idDonVi = $("#form-iID_DonViID").val();

    $.ajax({
        type: "POST",
        url: "ThongTriQuyetToan/GetLookupChuongTrinhByDonViId",
        data: { iID_DonViID: idDonVi },
        success: function (rs) {
            let selectElement = $("#form-iID_KHCTBQP_ChuongTrinhID");
            selectElement.find('option').remove();
            rs.data.forEach(x => {
                let opt = `<option value='${x.Id}'>${x.DisplayName}</option>`;
                selectElement.append(opt);
            });
            selectElement.val(GUID_EMPTY);

            LoadDataChiTiet();
        }
    });
}

// event change loại thông tri - thay đổi tên cột trong bảng chi tiết, giá trị cột
function ChangeLoaiThongTri(e) {
    let iLoaiThongTri = $(e).val();
    if (iLoaiThongTri == 1) {
        $("#lblLoaiThongTri").text('Số đề nghị quyết toán năm');
        $("#tbodyThongTriQTCT tr td[name='fDeNghiQuyetToanNam_USD']").removeAttr('hidden');
        $("#tbodyThongTriQTCT tr td[name='fDeNghiQuyetToanNam_VND']").removeAttr('hidden');
        $("#tbodyThongTriQTCT tr td[name='fThuaNopTraNSNN_USD']").attr('hidden', true);
        $("#tbodyThongTriQTCT tr td[name='fThuaNopTraNSNN_VND']").attr('hidden', true);

        let usdMoney = FormatNumber(modelNhiemVuChi.fDeNghiQuyetToanNam_USD, 2);
        let vndMioney = FormatNumber(modelNhiemVuChi.fDeNghiQuyetToanNam_VND, 0);
        $("#totalUSD").text(usdMoney == '' ? 0 : usdMoney);
        $("#totalVND").text(vndMioney == '' ? 0 : vndMioney);
    } else {
        $("#lblLoaiThongTri").text('Số thừa nộp trả ngân sách nhà nước');
        $("#tbodyThongTriQTCT tr td[name='fDeNghiQuyetToanNam_USD']").attr('hidden', true);
        $("#tbodyThongTriQTCT tr td[name='fDeNghiQuyetToanNam_VND']").attr('hidden', true);
        $("#tbodyThongTriQTCT tr td[name='fThuaNopTraNSNN_USD']").removeAttr('hidden');
        $("#tbodyThongTriQTCT tr td[name='fThuaNopTraNSNN_VND']").removeAttr('hidden');

        let usdMoney = FormatNumber(modelNhiemVuChi.fThuaNopTraNSNN_USD, 2);
        let vndMioney = FormatNumber(modelNhiemVuChi.fThuaNopTraNSNN_VND, 0);
        $("#totalUSD").text(usdMoney == '' ? 0 : usdMoney);
        $("#totalVND").text(vndMioney == '' ? 0 : vndMioney);
    }
}

// khi chọn năm, đợn vị, chương trình thì load data chi tiết.
function LoadDataChiTiet() {
    let iNamThucHien = $("#form-iNamThucHien").val();
    let iID_DonViID = $("#form-iID_DonViID").val();
    let iID_ChuongTrinhID = $("#form-iID_KHCTBQP_ChuongTrinhID").val();
    let iLoaiThongTri = $("#form-iLoaiThongTri").val();

    $("#form-iLoaiThongTri").removeAttr('disabled');

    if (iNamThucHien != '' && iID_DonViID != GUID_EMPTY && iID_DonViID != '' && iID_ChuongTrinhID != GUID_EMPTY && iID_ChuongTrinhID != '') {
        $.ajax({
            type: "POST",
            url: "ThongTriQuyetToan/GetDataThongTriQTCT",
            data: { iID_DonViID: iID_DonViID, iID_KHCTBQP_ChuongTrinhID: iID_ChuongTrinhID, iNamThucHien: iNamThucHien },
            success: function (rs) {
                modelNhiemVuChi = {};
                let htmlCode = "";
                rs.data.forEach(x => {
                    if (!x.bIsNhiemVuChi) {
                        let tr = `
                        <tr>
                            <td align="center" hidden name="iID_ChiTietTTQT">${x.ID ? x.ID : GUID_EMPTY}</td>
                            <td align="center" hidden name="iID_DuAnIDTTQT">${x.iID_DuAnID ? x.iID_DuAnID : ''}</td>
                            <td align="center" hidden name="iID_HopDongIDTTQT">${x.iID_HopDongID ? x.iID_HopDongID : ''}</td>
                            <td align="center" hidden name="iID_ThanhToanTTQT">${x.iID_ThanhToan_ChiTietID ? x.iID_ThanhToan_ChiTietID : ''}</td>

                            <td align="left" class="${x.bIsTittle == true ? 'fw-bold' : ''}"  frozenCol" name="sMaThuTu">${x.sMaThuTu ? x.sMaThuTu : ''}</td>
                            <td align="center">${x.sM ? x.sM : ''}</td>
                            <td align="center">${x.sTM ? x.sTM : ''}</td>
                            <td align="center">${x.sTTM ? x.sTTM : ''}</td>
                            <td align="center">${x.sNG ? x.sNG : ''}</td>
                            <td align="left" name="sTenNoiDungChi">${x.sTenNoiDungChi ? x.sTenNoiDungChi : ''}</td>
                            <td name="fDeNghiQuyetToanNam_USD" ${iLoaiThongTri == 2 ? "hidden" : ""} align="right">${FormatNumber(x.fDeNghiQuyetToanNam_USD, 2)}</td>
                            <td name="fDeNghiQuyetToanNam_VND" ${iLoaiThongTri == 2 ? "hidden" : ""} align="right">${FormatNumber(x.fDeNghiQuyetToanNam_VND)}</td>
                            <td name="fThuaNopTraNSNN_USD" ${iLoaiThongTri == 2 ? "" : "hidden"} align="right">${FormatNumber(x.fThuaNopTraNSNN_USD, 2)}</td>
                            <td name="fThuaNopTraNSNN_VND" ${iLoaiThongTri == 2 ? "" : "hidden"} align="right">${FormatNumber(x.fThuaNopTraNSNN_VND)}</td>
                        </tr>`;
                        htmlCode += tr;
                    } else {
                        modelNhiemVuChi = x;
                        $("#totalUSD").text(iLoaiThongTri == 1
                            ? FormatNumber(x.fDeNghiQuyetToanNam_USD, 2) == '' ? 0 : FormatNumber(x.fDeNghiQuyetToanNam_USD, 2)
                            : FormatNumber(x.fThuaNopTraNSNN_USD, 2) == '' ? 0 : FormatNumber(x.fThuaNopTraNSNN_USD, 2));
                        $("#totalVND").text(iLoaiThongTri == 1
                            ? FormatNumber(x.fDeNghiQuyetToanNam_VND) == '' ? 0 : FormatNumber(x.fDeNghiQuyetToanNam_VND)
                            : FormatNumber(x.fThuaNopTraNSNN_VND) == '' ? 0 : FormatNumber(x.fThuaNopTraNSNN_VND));
                    }
                });

                if (rs.data.length == 0) {
                    $("#totalUSD").text(0);
                    $("#totalVND").text(0);
                }

                $("#tbodyThongTriQTCT").html(htmlCode);
            }
        });
    } else {
        let htmlCode = "";
        modelNhiemVuChi = {};
        $("#tbodyThongTriQTCT").html(htmlCode);
        $("#totalUSD").text(0);
        $("#totalVND").text(0);
    }
}

// Lưu
function Save() {
    let state = $("#hState").val()

    let thongTriQuyetToan = {};
    let thongTriQuyetToanChiTiet = [];
    if (state == 'UPDATE') {
        thongTriQuyetToan.ID = $("#hId").val();
    }
    thongTriQuyetToan.sSoThongTri = $("#form-sSoThongTri").val()?.trim();
    thongTriQuyetToan.dNgayLap = $("#form-dNgayLap").val()?.trim();
    thongTriQuyetToan.iID_KHCTBQP_ChuongTrinhID = $("#form-iID_KHCTBQP_ChuongTrinhID").val();
    thongTriQuyetToan.iID_DonViID = $("#form-iID_DonViID").val();
    thongTriQuyetToan.iID_MaDonVi = $("#form-iID_DonViID").find(":selected").data('madonvi').toString();
    thongTriQuyetToan.iNamThongTri = $("#form-iNamThucHien").val();
    thongTriQuyetToan.iLoaiThongTri = $("#form-iLoaiThongTri").val();
    thongTriQuyetToan.iLoaiNoiDungChi = $("#form-iLoaiNoiDungChi").val();
    thongTriQuyetToan.sThongTri_USD = UnFormatNumber($("#totalUSD").text()).toString();
    thongTriQuyetToan.sThongTri_VND = UnFormatNumber($("#totalVND").text()).toString();

    $("#tbodyThongTriQTCT tr").each(function (e) {
        let el = $(this);
        let obj = {
            iID_DuAnID: el.find('td[name="iID_DuAnIDTTQT"]').text(),
            iID_HopDongID: el.find('td[name="iID_HopDongIDTTQT"]').text(),
            iID_ThanhToan_ChiTietID: el.find('td[name="iID_ThanhToanTTQT"]').text(),
            sDeNghiQuyetToanNam_VND: UnFormatNumber(el.find('td[name="fDeNghiQuyetToanNam_VND"]').text()).toString(),
            sDeNghiQuyetToanNam_USD: UnFormatNumber(el.find('td[name="fDeNghiQuyetToanNam_USD"]').text()).toString(),
            sThuaNopTraNSNN_VND: UnFormatNumber(el.find('td[name="fThuaNopTraNSNN_VND"]').text()).toString(),
            sThuaNopTraNSNN_USD: UnFormatNumber(el.find('td[name="fThuaNopTraNSNN_USD"]').text()).toString(),
            sMaThuTu: el.find('td[name="sMaThuTu"]').text()?.trim(),
            sTenNoiDungChi: el.find('td[name="sTenNoiDungChi"]').text()?.trim()
        };

        thongTriQuyetToanChiTiet.push(obj)
    });

    let param = {};
    param.ThongTriQuyetToan = thongTriQuyetToan;
    param.ThongTriQuyetToan_ChiTiet = thongTriQuyetToanChiTiet;

    if (!ValidateFrom(param, state)) {
        return;
    }

    $.ajax({
        type: "POST",
        url: "ThongTriQuyetToan/SaveThongTriQuyetToan",
        data: { input: param, state: state }, 
        success: function (rs) {
            if (rs) {
                ResetChangePage();
            } else {
                var Title = 'Lỗi thêm mới thông tri quyết toán!';
                var Messages = ['Thêm mới thông tri quyết toán không thành công!'];

                $.ajax({
                    type: "POST",
                    url: "/Modal/OpenModal",
                    data: { Title: Title, Messages: Messages, Category: ERROR },
                    success: function (data) {
                        $("#divModalConfirm").html(data);
                    }
                });
            }
        },
        error: function () {
            var Title = 'Lỗi thêm mới thông tri quyết toán!';
            var Messages = ['Thêm mới thông tri quyết toán không thành công!'];

            $.ajax({
                type: "POST",
                url: "/Modal/OpenModal",
                data: { Title: Title, Messages: Messages, Category: ERROR },
                success: function (data) {
                    $("#divModalConfirm").html(data);
                }
            });
        }
    });
}

// Phân trang event
function ChangePage(e) {
    GetListData(e);
}

function ValidateFrom(data, state = 'CREATE') {
    var Title = '';
    if (state == 'CREATE') {
        Title = 'Lỗi thêm mới thông tri quyết toán!';
    } else {
        Title = 'Lỗi cập nhật thông tri quyết toán!';
    }

    var Messages = [];

    if (data.ThongTriQuyetToan.sSoThongTri.trim() == '') {
        Messages.push("Chưa có thông tin về số thông tri, vui lòng nhập số thông tri!");
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