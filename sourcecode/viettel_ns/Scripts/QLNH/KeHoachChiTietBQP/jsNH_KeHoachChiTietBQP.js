var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;
var CONFIRM = 0;
var lstDonVi = [];
var lstTiGiaChiTiet = [];
var tiGia = 0;
var isVNDtoUSD = false;
var USE_LAST_NEW_ADJUST = false;

// State: CREATE => tạo mới, UPDATE => Chỉnh sửa, ADJUST => Điều chỉnh
var CURRENT_STATE = '';

$(document).ready(function () {

    // Event change Loại Kế Hoach form thêm mới, chỉnh sửa, điều chỉnh.
    $("#iLoai").on('change', function (e) {
        let value = $(this).val();
        let gdt = $("#formGiaiDoanTu");
        let gdd = $("#formGiaiDoanDen");
        let nkh = $("#formNamKeHoach");
        let pr = $("#formParrentID");
        if (value == 1) {
            gdt.removeClass('hidden');
            gdd.removeClass('hidden');
            nkh.addClass('hidden');
            pr.addClass('invisible');
        } else if (value == 2) {
            gdt.addClass('hidden');
            gdd.addClass('hidden');
            nkh.removeClass('hidden');
            pr.removeClass('invisible');
        } else if (value == 3) {
            gdt.removeClass('hidden');
            gdd.removeClass('hidden');
            nkh.addClass('hidden');
            pr.removeClass('invisible');
        } else {
            gdt.removeClass('hidden');
            gdd.removeClass('hidden');
            nkh.addClass('hidden');
            pr.addClass('invisible');
        }
    });

    $("#iID_ParentID").select2({
        width: '100%',
        matcher: FilterInComboBox
    });

    $("#iID_KHTongTheTTCPID").select2({
        width: '100%',
        matcher: FilterInComboBox
    });

    $('#modalKHChiTietBQP').on('hidden.bs.modal', function () {
        USE_LAST_NEW_ADJUST = false;
    });
});

// #region =================================== Action view list ===================================

// Mở modal thêm mới, sửa, điều chỉnh
function OpenModal(id, state) {
    $("#modalKHChiTietBQPLabel").html('');

    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/KeHoachChiTietBQP/GetModal",
        async: false,
        data: { id: id, state: state },
        success: function (data) {
            $("#contentmodalKHChiTietBQP").empty().html(data);
            if (state == 'CREATE') {
                $("#modalKHChiTietBQPLabel").empty().html('Thêm mới kế hoạch chi tiết Bộ Quốc phòng phê duyệt');
            } else if (state == 'UPDATE') {
                $("#modalKHChiTietBQPLabel").empty().html('Sửa kế hoạch chi tiết Bộ Quốc phòng phê duyệt');
            } else {
                $("#modalKHChiTietBQPLabel").empty().html('Điều chỉnh kế hoạch chi tiết Bộ Quốc phòng phê duyệt');
            }

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

    CURRENT_STATE = state;
}

// Filter in select
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
        var modifiedData = $.extend({}, data, true);
        //modifiedData.text += ' (matched)';
        return modifiedData;
    }

    return null;
}

// Láy chi tiết tỉ giá khi thay đổi tỉ giá trên form thêm mới, sửa, điều chỉnh
function ChangeTiGiaSelect() {
    let tiGiaId = $("#iID_TiGiaID").val();
    $.ajax({
        type: "POST",
        url: "/QLNH/KeHoachChiTietBQP/ChangeTiGia",
        data: { tiGiaId: tiGiaId },
        success: function (data) {
            $("#tienTeQuyDoiID").html(data);
        }
    });
}

// Lưu thông tin trên form thêm mới, sửa, điều chỉnh
function Save() {
    let state = CURRENT_STATE;

    // Get data
    let data = {};
    data.iLoai = $("#iLoai").val();
    data.iGiaiDoanTu = $("#txtGiaiDoanTu").val().trim();
    data.iGiaiDoanDen = $("#txtGiaiDoanDen").val().trim();
    data.iNamKeHoach = $("#txtNamKeHoach").val().trim();
    data.iID_ParentID = $("#iID_ParentID").val();
    data.iID_KHTongTheTTCPID = $("#iID_KHTongTheTTCPID").val();
    data.iID_TiGiaID = $("#iID_TiGiaID").val();
    data.sSoKeHoach = $("#txtSoKeHoach").val().trim();
    data.dNgayKeHoach = $("#txtNgayKeHoach").val();
    data.sMoTaChiTiet = $("#txtMoTaChiTiet").val().trim();
    data.bIsXoa = false;

    // Check state
    if (CURRENT_STATE == 'UPDATE') {
        // Update thì truyền thêm ID để lấy data phía BE và update.
        data.id = $("#hIdKeHoachChiTietBQP").val();

        // Validate
        if (!ValidateData(data, 'Lỗi sửa kế hoạch chi tiết Bộ Quốc phòng phê duyệt!')) {
            return;
        }
    } else if (CURRENT_STATE == 'CREATE') {
        // Thêm mới thì thêm các trường thông tin liên quan đến điều chỉnh
        data.bIsGoc = true;
        data.bIsActive = true;
        data.iLanDieuChinh = 0;

        // Validate
        if (!ValidateData(data, 'Lỗi thêm mới kế hoạch chi tiết Bộ Quốc phòng phê duyệt!')) {
            return;
        }
    } else {
        // Điều chỉnh thì gán lại old ID cho data để lấy thông tin nhiệm vụ chi lên view.
        data.bIsGoc = false;
        data.bIsActive = true;
        data.iLanDieuChinh = parseInt($("#hiLanDieuChinh").val()) + 1;
        data.iID_ParentAdjustID = $("#hIdKeHoachChiTietBQP").val();
        data.ID = $("#hIdKeHoachChiTietBQP").val();
        // Validate
        if (!ValidateData(data, 'Lỗi điều chỉnh kế hoạch chi tiết Bộ Quốc phòng phê duyệt!')) {
            return;
        }
    }

    OpenModalDetail(data, CURRENT_STATE);
}

// Validate data trên form thêm mới, sửa, điều chỉnh
function ValidateData(data, text) {
    var Title = text;
    var Messages = [];

    if (data.iID_TiGiaID == null || data.iID_TiGiaID == GUID_EMPTY) {
        Messages.push("Chưa có thông tin về tỉ giá, vui lòng chọn tỉ giá!");
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

// Làm mới danh sách
function ResetChangePage() {
    $("#txtFromDateFitler").val('');
    $("#txtToDateFitler").val('');
    $("#txtSoKeHoachFilter").val('');
    $("#txtNgayBanHanhFilter").val('');

    GetListData(1);
}

// Tìm kiếm
function GetListData(currentPage = 1) {
    _paging.CurrentPage = currentPage;
    var filter = {
        giaiDoanTu:     $("#txtFromDateFitler").val().trim(),
        giaiDoanDen:    $("#txtToDateFitler").val().trim(),
        soKeHoach:      $("#txtSoKeHoachFilter").val().trim(),
        ngayBanHanh:    $("#txtNgayBanHanhFilter").val().trim()
    }

    $.ajax({
        type: "POST",
        url: "/QLNH/KeHoachChiTietBQP/TimKiem",
        data: { input: filter, paging: _paging },
        success: function (data) {
            // View result
            $("#lstDataView").html(data);

            // Gán lại data cho filter
            $("#txtFromDateFitler").val(filter.giaiDoanTu);
            $("#txtToDateFitler").val(filter.giaiDoanDen);
            $("#txtSoKeHoachFilter").val(filter.soKeHoach);
            $("#txtNgayBanHanhFilter").val(filter.ngayBanHanh);
        }
    });
}

// Regex number textbox
function RegexNumber(e) {
    let value = $(e).val();
    let regex = /\D/g;
    $(e).val(value.replace(regex, ''));
}

// Regex input date
function RegexInputDate(e) {
    let value = $(e).val();
    let regex = /[^0-9/]/g;
    $(e).val(value.replace(regex, ''));
}

// Phân trang event
function ChangePage(e) {
    GetListData(e);
} 

// Confirm xóa
function ConfirmDelete(id, sNam, sSoKeHoach) {
    var Title = 'Xác nhận xóa kế hoạch chi tiết Bộ Quốc phòng phê duyệt';
    var Messages = [];
    Messages.push('Bạn có chắc chắn muốn xóa kế hoạch chi tiết: ' + sSoKeHoach + ' - ' + sNam + '?');
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

// Xóa
function Delete(id) {
    $.ajax({
        type: "POST",
        url: "/QLNH/KeHoachChiTietBQP/DeleteKeHoachChiTietBQP",
        data: { id: id },
        success: function (data) {
            if (data) {
                ChangePage();
            } else {
                var Title = 'Lỗi xóa kế hoạch chi tiết Bộ Quốc phòng phê duyệt';
                $.ajax({
                    type: "POST",
                    url: "/Modal/OpenModal",
                    data: { Title: Title, Messages: ['Không xóa được dữ liệu!'], Category: ERROR },
                    success: function (res) {
                        $("#divModalConfirm").html(res);
                    }
                });
            }
        }
    });
}

// Tìm ra thằng cha active và gán lại vào select.
function UpdateParentTTCP(iID_TTCP) {
    $.ajax({
        type: "POST",
        url: "/QLNH/KeHoachTongTheTTCP/FindParentTTCPActive",
        async: false,
        data: { id: iID_TTCP },
        success: function (data) {
            $("#iID_KHTongTheTTCPID").val(data.result.ID).change();
            USE_LAST_NEW_ADJUST = true;
        }
    });
}

// View to detail
function OpenModalDetail(data, state, isFilter = false) {

    if (state == 'DETAIL') {
        USE_LAST_NEW_ADJUST = false;
        let temp = data;
        data = {
            ID: temp,
            iID_KHTongTheTTCPID: null
        };

        if (isFilter) {
            data.iID_BQuanLyID = $("#iID_BQuanLyID").val();
            data.iID_DonViID = $("#iID_DonViID").val();
        }
    }

    // Get view detail
    $.ajax({
        type: "POST",
        url: "/QLNH/KeHoachChiTietBQP/ViewDetailKHChiTietBQP",
        data: { input: data, state: state, isUseLastTTCP: USE_LAST_NEW_ADJUST },
        async: false,
        success: function (result) {
            $("#modalKHChiTietBQP").modal('hide');
            // View result
            $("#lstDataView").html(result);
            if (isFilter) {
                $("#iID_BQuanLyID").val(data.iID_BQuanLyID);
                $("#iID_DonViID").val(data.iID_DonViID);
            }
        }
    });

    CURRENT_STATE = state;

    // Tính tỉ giá nếu là update, create, adjust
    if (CURRENT_STATE != 'DETAIL') {

        // Lấy thông tin tỉ giá
        $.ajax({
            type: "POST",
            url: "/QLNH/KeHoachChiTietBQP/GetDataLookupDetail",
            data: { iID_TiGiaID: data.iID_TiGiaID },
            async: false,
            success: function (result) {
                lstDonVi = result.ListDonVi;
                lstTiGiaChiTiet = result.ListTiGiaChiTiet;
            }
        });

        // Check thông tin tỉ giá, điều kiện action
        let fromUSD = lstTiGiaChiTiet.find(x => x.sMaTienTeGoc.trim().toUpperCase() == 'USD');
        let fromVND = lstTiGiaChiTiet.find(x => x.sMaTienTeGoc.trim().toUpperCase() == 'VND');

        // Nếu mã tiền tệ gốc là USD thì enable input USD, disable VND và ngược lại.
        if (fromUSD) {
            let toVND = lstTiGiaChiTiet.find(x => x.sMaTienTeQuyDoi.trim().toUpperCase() == 'VND');
            $(".inpuFromUSD").prop('disabled', false);
            $(".inpuFromVND").prop('disabled', true);
            isVNDtoUSD = false;

            // Nếu có mã tiền tệ quy đổi là VND thì tính tỉ giá, không có thì mặc định là 0
            if (toVND) {
                tiGia = toVND.fTiGia;
            } else {
                tiGia = 0;
            }
        } else if (fromVND) {
            let toUSD = lstTiGiaChiTiet.find(x => x.sMaTienTeQuyDoi.trim().toUpperCase() == 'USD');
            $(".inpuFromUSD").prop('disabled', true);
            $(".inpuFromVND").prop('disabled', false);
            isVNDtoUSD = true;
            if (toUSD) {
                tiGia = toUSD.fTiGia;
            } else {
                tiGia = 0;
            }
        } else {
            // Nếu mã tiền tệ khác USD hoặc VND thì kiểm tra xem mã tiền tệ quy đổi có báo gồm VND và USD không? Nếu có thì tính tỉ giá.
            $(".inpuFromUSD").prop('disabled', false);
            $(".inpuFromVND").prop('disabled', true);
            isVNDtoUSD = false;
            let toUSD = lstTiGiaChiTiet.find(x => x.sMaTienTeQuyDoi.trim().toUpperCase() == 'USD');
            if (toUSD) {
                let toVND = lstTiGiaChiTiet.find(x => x.sMaTienTeQuyDoi.trim().toUpperCase() == 'VND');
                if (toVND) {
                    tiGia = toVND.fTiGia / toUSD.fTiGia;
                } else {
                    tiGia = 0;
                }
            } else {
                tiGia = 0;
            }
        }
    }

    // Khi đã có tỉ giá và lấy được view thì tính lại giá trị theo tỉ giá.
    if (CURRENT_STATE == 'UPDATE' || CURRENT_STATE == 'ADJUST') {
        let oldTiGia = $("#hiID_TiGiaID").val();
        if (oldTiGia != data.iID_TiGiaID) {
            if (isVNDtoUSD) {
                CalcListTiGia('VND', $("input.inputFromVND:not(:disabled)"));
            } else {
                CalcListTiGia('VND', $("input.inputFromUSD:not(:disabled)"));
            }
        }
    }
}

// #endregion ======================================================================

// #region =================================== Action view detail ===================================

// Chuyển về màn danh sách kế hoạch BQP
function ViewToList() {
    var filter = {
        giaiDoanTu: null,
        giaiDoanDen: null,
        soKeHoach: null,
        ngayBanHanh: null
    }

    $.ajax({
        type: "POST",
        url: "/QLNH/KeHoachChiTietBQP/TimKiem",
        data: { input: filter, paging: null },
        success: function (data) {
            // View result
            $("#lstDataView").html(data);
        }
    });
}

// Lưu chi tiết nhiệm vụ chi
function SaveDetail() {
    let state = $("#currentState").val();
    let keHoachChiTietBQP = $("#keHoachChiTietBQP").val();

    var tableNhiemVuChi = [];
    $("#tbodyNhiemVuChi tr").each(function (e) {
        let rowElement = $(this);

        let rowData = {};
        rowData.ID = rowElement.find('td[name="ID_NhiemVuChi"]').text().trim();
        rowData.sMaThuTu = rowElement.find('td[name="sMaThuTu"]').text().trim();
        rowData.sTenNhiemVuChi = rowElement.find('#sTenNhiemVuChi').val().trim();
        rowData.iID_BQuanLyID = rowElement.find('#iID_BQuanLyID').val();
        rowData.bIsTTCP = (rowElement.find('td[name="bIsTTCP"]').text().trim().toLowerCase() === 'true');
        if (!rowData.bIsTTCP) {
            rowData.iID_MaDonVi = rowElement.find('select[name="iID_DonViID"]').find(':selected').data('madonvi');
            rowData.iID_DonViID = rowElement.find('select[name="iID_DonViID"]').val();
        } else {
            rowData.iID_MaDonVi = null;
            rowData.iID_DonViID = null;
        }
        rowData.fGiaTriUSD = UnFormatNumber(rowElement.find('#fGiaTriBQP_USD').val());
        rowData.fGiaTriVND = UnFormatNumber(rowElement.find('#fGiaTriBQP_VND').val());
        rowData.iID_KHTTTTCP_NhiemVuChiID = rowElement.find('td[name="iID_KHTTTTCP_NhiemVuChiID"]').text();
        rowData.iID_ParentID = rowElement.find('td[name="iID_ParentID"]').text();
        tableNhiemVuChi.push(rowData);
    });

    $.ajax({
        type: "POST",
        url: "/QLNH/KeHoachChiTietBQP/SaveKeHoachChiTietBQP",
        data: { lstNhiemVuChis: tableNhiemVuChi, keHoachChiTietBQP: keHoachChiTietBQP, state: state  },
        success: function (data) {
            if (data) {
                ViewToList();
            } else {
                var Title = 'Lỗi thêm mới kế hoạch chi tiết Bộ Quốc phòng phê duyệt!';
                var Messages = ['Thêm mới kế hoạch chi tiết Bộ Quốc phòng phê duyệt không thành công!'];
                if (state == 'UPDATE') {
                    Title = 'Lỗi cập nhật kế hoạch chi tiết Bộ Quốc phòng phê duyệt!';
                    Messages = ['Cập nhật kế hoạch chi tiết Bộ Quốc phòng phê duyệt không thành công!'];
                } else if (state == 'ADJUST') {
                    Title = 'Lỗi điều chỉnh kế hoạch chi tiết Bộ Quốc phòng phê duyệt!';
                    Messages = ['Điều chỉnh kế hoạch chi tiết Bộ Quốc phòng phê duyệt không thành công!'];
                }
                
                $.ajax({
                    type: "POST",
                    url: "/Modal/OpenModal",
                    data: { Title: Title, Messages: Messages, Category: ERROR },
                    success: function (data) {
                        $("#divModalConfirm").html(data);
                    }
                });
            }
        }
    });
}

// Thêm dòng nhiệm vụ chi
function AddRowKeHoachChiTiet(e) {
    let parentRow = $(e).parents('tr');
    let stt = parentRow.find('td[name="sMaThuTu"]').text();
    let tenPB = parentRow.find('td[name="sTenPhongBan"]').text();
    let idPB = parentRow.find('#iID_BQuanLyID').val();
    let iID_TTTCP_NhiemVuChiID = parentRow.find('td[name="iID_KHTTTTCP_NhiemVuChiID"]').text();
    //let ParentIsTTCP = (parentRow.find('td[name="bIsTTCP"]').text().trim().toLowerCase() === 'true');

    // Disable input + button delete của dòng cha.
    //if (!ParentIsTTCP) {
        parentRow.find('#fGiaTriBQP_VND').prop('disabled', true);
        parentRow.find('#fGiaTriBQP_USD').prop('disabled', true);
        parentRow.find('button.btn-delete').addClass('disabled');
    //}

    // Lấy số thứ tự mới và index dòng mới thêm vào.
    let { sttNew, parentRowIndex } = RefreshThuTuAndGetThuTuMoi(stt, parentRow.index());

    let row = `
        <tr>
            <td name="ID_NhiemVuChi" class="hidden"></td>
            <td align="left" name="sMaThuTu">` + sttNew + `</td>
            <td align="left"><input type="text" id="sTenNhiemVuChi" class="form-control" value="" maxlength="255" autocomplete="off" /></td>
            <td align="left" name="sTenPhongBan">` + tenPB + `</td>
            <td class="hidden"><input id="iID_BQuanLyID" class="form-control" value="` + idPB + `" maxlength="255" autocomplete="off"></td>
            <td align="left">` + CreateLookupDonVi() + `</td>
            <td align="right" name="fGiaTriTTCP_USD"></td>
            <td align="right" name="fGiaTriBQP_USD">
                <input type="text" id="fGiaTriBQP_USD" class="form-control inputFromUSD" value="" maxlength="255" autocomplete="off"
                    onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' onblur="CalcTiGia('USD', this)" ` + (isVNDtoUSD ? 'disabled' : '') + `/>
            </td>
            <td align="right" name="fGiaTriBQP_VND">
                <input type="text" id="fGiaTriBQP_VND" class="form-control inputFromVND" value="" maxlength="255" autocomplete="off"
                    onkeyup='ValidateNumberKeyUp(this);' onkeypress='return ValidateNumberKeyPress(this, event);' onblur="CalcTiGia('VND', this)" ` + (isVNDtoUSD ? '' : 'disabled') + `/>
            </td>
            <td class="hidden" name="bIsTTCP">False</td>
            <td class="hidden" name="iID_KHTTTTCP_NhiemVuChiID">` + iID_TTTCP_NhiemVuChiID + `</td>
            <td class="hidden" name="iID_ParentID"></td>
                <td align="center">
                    <button type="button" class="btn-detail" onclick="AddRowKeHoachChiTiet(this)"><i class="fa fa-plus" aria-hidden="true"></i></button>
                    <button type="button" class="btn-delete" onclick="RemoveRowKeHoachChiTiet(this)"><i class="fa fa-trash-o fa-lg" aria-hidden="true"></i></button>
                </td>
        </tr>`;
    // Add row
    $("#tbodyNhiemVuChi tr").eq(parentRowIndex).after(row);
}

// Xóa dòng nhiệm vụ chi
function RemoveRowKeHoachChiTiet(e) {

    // Lấy dòng hiện tại
    let currentRow = $(e).parents('tr');
    let stt = currentRow.find('td[name="sMaThuTu"]').text();

    // Xóa dòng hiện tại thông qua index.
    let index = currentRow.index();
    $("#tbodyNhiemVuChi tr").eq(index).remove();

    // Tìm dòng cha nếu có thì chỉnh lại stt
    let indexOfDot = stt.lastIndexOf('.');
    if (indexOfDot != -1) {
        let sttParent = stt.substring(0, indexOfDot);
        RefreshThuTuAndGetThuTuMoi(sttParent, 0);

        // Sau khi xóa dòng thì tìm xem còn thằng nào cùng cấp không?
        const regex = new RegExp('^' + sttParent + '.[0-9]+$');
        let hasValue = false;
        let rowParent = undefined;
        $("#tbodyNhiemVuChi tr td[name='sMaThuTu']").each(function (e) {
            let value = $(this).text();

            // Nếu match với regex thì là có thằng cùng cấp
            if (regex.test(value)) {
                hasValue = true;
                return;
            }

            // Tiện tìm luôn dòng cha theo stt
            if (value == sttParent) {
                rowParent = $(this).parents('tr');
            }
        });

        // Nếu không có dòng nào cùng cấp thì enable lại function của dòng cha.
        if (!hasValue) {
            if (rowParent) {
                //let isTTCP = (rowParent.find('td[name="bIsTTCP"]').text().trim().toLowerCase() === 'true');
                // Nếu dòng cha không phải thông tin chính phủ thì enable lại các function.
                //if (!isTTCP) {
                    if (isVNDtoUSD) {
                        rowParent.find('#fGiaTriBQP_VND').prop('disabled', false);
                    } else {
                        rowParent.find('#fGiaTriBQP_USD').prop('disabled', false);
                    }
                    rowParent.find('button.btn-delete').removeClass('disabled');
                //}
            }
        }
    }

    // Tính lại tổng tiền từ dòng hiện tại đổ lên.
    SumValueParent(stt);
}

// Get stt, index dòng thêm mới nhiệm vụ chi
function RefreshThuTuAndGetThuTuMoi(stt, index) {
    // Tạo regex tìm những dòng con của stt truyền vào.
    const regex = new RegExp('^' + stt + '.[0-9]+$');
    let arrMatch = [];
    let hasValue = false;

    // Add những dòng con vào 1 mảng
    $("#tbodyNhiemVuChi tr td[name='sMaThuTu']").each(function (e) {
        let value = $(this).text();
        if (regex.test(value)) {
            arrMatch.push({
                value: value,
                element: $(this)
            });
            hasValue = true;
        }
    });

    // Nếu tìm thấy những dòng con thì lấy index và stt để trả ra ngoài. Đồng thời sắp xếp lại.
    if (hasValue) {
        // Lấy dòng con có stt lớn nhất.
        let lastChildRow = arrMatch.sort((a, b) => parseInt(a.value.replace(stt + '.', '')) - parseInt(b.value.replace(stt + '.', '')))[arrMatch.length - 1];
        let maxStt = lastChildRow.value;
        let maxIndexStt = parseInt(maxStt.replace(stt + '.', ''));

        // Tính ra stt mới.
        let indexSttResult = maxIndexStt + 1;
        let indexRow = lastChildRow.element.parents('tr').index();

        // Lấy stt của dòng con lớn nhất để lấy index của dòng con lớn nhất.
        let sttChecker = lastChildRow.value;

        do {
            // Create regex cho dòng con
            let regexChecker = new RegExp('^' + sttChecker + '.[0-9]+$');
            let arrChecker = [];
            let hasValueChecker = false;

            // Tìm xem có dòng con hay không?
            $("#tbodyNhiemVuChi tr td[name='sMaThuTu']").each(function (e) {
                let value = $(this).text();
                if (regexChecker.test(value)) {
                    arrChecker.push({
                        value: value,
                        element: $(this)
                    });
                    hasValueChecker = true;
                }
            });

            // Nếu tìm thấy dòng con thì gán lại index và stt của dòng con để tiếp tục tìm kiếm
            if (hasValueChecker) {
                let lastChildRowChecker = arrChecker.sort((a, b) => parseInt(a.value.replace(sttChecker + '.', '')) - parseInt(b.value.replace(sttChecker + '.', '')))[arrMatch.length - 1];
                sttChecker = lastChildRowChecker.value;
                indexRow = lastChildRowChecker.element.parents('tr').index();
            } else {
                // Nếu không tìm thấy dòng con nào nữa thì break khỏi vòng lặp.
                break;
            }
        } while (true);

        // Nếu stt lớn nhất != độ dài mảng của những thằng con (tương đương với việc xóa dòng ở trước dòng cuối cùng) thì gán lại stt cho những thằng con.
        if (maxIndexStt != arrMatch.length) {
            let newIndex = 1;
            arrMatch.forEach(x => {
                let oldStt = x.element.text();
                let newStt = stt + '.' + newIndex;

                // Update toàn bộ những thằng con về đúng stt mới.
                $("#tbodyNhiemVuChi tr td[name='sMaThuTu']").each(function (e) {
                    let value = $(this).text();
                    if (value.startsWith(oldStt + '.')) {
                        $(this).text(newStt + value.substring(oldStt.length));
                    }
                });

                x.element.text(newStt);
                newIndex++;
            });
            indexSttResult = newIndex;
        }

        return {
            sttNew: stt + '.' + indexSttResult,
            parentRowIndex: indexRow
        };
    } else {
        // Nếu không có thằng con nào thì bắt đầu với 1.
        return {
            sttNew: stt + '.1',
            parentRowIndex: index
        };
    }
}

// Tạo lookup đơn vị
function CreateLookupDonVi() {

    var html = '<select class="form-control" name="iID_DonViID">';

    lstDonVi.forEach(x => {
        html += '<option data-madonvi="' + x.iID_MaDonVi + '" value="' + x.iID_Ma + '">' + x.sMoTa + '</option>';
    });

    html += '</select>';

    return html;
}

// Tính giá trị qua tỉ giá
function CalcTiGia(type, e) {
    let tr = $(e).parents('tr');
    let stt = tr.find("td[name='sMaThuTu']").text();
    if (isVNDtoUSD) {
        if (type == 'VND') {
            let elInputUSD = tr.find('.inputFromUSD');
            let inputVND = parseFloat(UnFormatNumber($(e).val()));
            if (isNaN(inputVND)) {
                elInputUSD.val(0);
                $(e).val(0);
            } else {
                $.ajax({
                    type: "POST",
                    data: { number: inputVND.toString(), numTiGia: tiGia.toString() },
                    url: '/QLNH/KeHoachChiTietBQP/CalcMoneyByTiGia',
                    async: false,
                    success: function (data) {
                        let result = FormatNumber(data.result);
                        elInputUSD.val(result == '' ? 0 : result);
                    }
                });
            }
        }
    } else {
        if (type == 'USD') {
            let elInputVND = tr.find('.inputFromVND');
            let inputUSD = parseFloat(UnFormatNumber($(e).val()));
            if (isNaN(inputUSD)) {
                elInputVND.val(0);
                $(e).val(0);
            } else {
                $.ajax({
                    type: "POST",
                    data: { number: inputUSD.toString(), numTiGia: tiGia.toString() },
                    url: '/QLNH/KeHoachChiTietBQP/CalcMoneyByTiGia',
                    async: false,
                    success: function (data) {
                        let result = FormatNumber(data.result);
                        elInputVND.val(result == '' ? 0 : result);
                    }
                });
            }
        }
    }

    SumValueParent(stt);
}

// Tính tổng giá trị cho dòng cha
function SumValueParent(sttCurrentRow) {
    // Lấy stt dòng hiện tại
    let indexOfDot = sttCurrentRow.lastIndexOf('.');
    if (indexOfDot != -1) {
        // Nêu có thể cắt chuỗi stt thì cắt để tìm stt của parent
        let sttParent = sttCurrentRow.substring(0, indexOfDot);
        let rowParent = undefined;

        // Tạo regex tìm dòng con
        const regexChild = new RegExp('^' + sttParent + '.[0-9]+$');
        let lstVND = [];
        let lstUSD = [];

        // Load qua toàn bộ dòng trong bảng để tìm dòng con và lấy giá trị để tính tổng
        $("#tbodyNhiemVuChi tr").each(function () {
            let sttRow = $(this).find("td[name='sMaThuTu']").text();

            // Lấy dòng cha.
            if (sttRow == sttParent) {
                rowParent = $(this);
            }

            // Nếu dòng con match với regex thì get value.
            if (regexChild.test(sttRow)) {
                // Lấy giá trị của USD và VND rồi cộng vào sum
                let valueUSD = UnFormatNumber($(this).find("#fGiaTriBQP_USD").val());
                let valueVND = UnFormatNumber($(this).find("#fGiaTriBQP_VND").val());

                lstUSD.push(valueUSD.toString());
                lstVND.push(valueVND.toString());
            }
        });

        $.ajax({
            type: "POST",
            data: { lstNumVND: lstVND, lstNumUSD: lstUSD },
            url: '/QLNH/KeHoachChiTietBQP/SumTwoList',
            async: false,
            success: function (data) {
                // Nếu có rowParent thì gán value
                if (rowParent) {
                    rowParent.find("#fGiaTriBQP_USD").val(FormatNumber(data.resultUSD) == '' ? 0 : FormatNumber(data.resultUSD));
                    rowParent.find("#fGiaTriBQP_VND").val(FormatNumber(data.resultVND) == '' ? 0 : FormatNumber(data.resultVND));
                    SumValueParent(sttParent);
                }
            }
        });
    }
}

// Tính giá trị qua tỉ giá nhưng ko tính lại tổng
function CalcListTiGia(type, listInput) {

    var listTiGiaModel = [];
    if (type == 'VND') {
        listInput.each(function() {
            let inputVNDElement = $(this);

            // Lấy thông tin dòng, stt, inputUSD, inputVND
            let tr = $(inputVNDElement).parents('tr');
            let stt = tr.find("td[name='sMaThuTu']").text();
            let elInputUSD = tr.find('.inputFromUSD');
            let inputVND = parseFloat(UnFormatNumber($(inputVNDElement).val()));

            // Nếu inputVND ko phải số ( == '') hoặc bằng 0 thì gán giá trị = 0, ko phải tính.
            if (isNaN(inputVND) || inputVND === 0) {
                elInputUSD.val(0);
                $(inputVNDElement).val(0);
            } else {
                // Nếu giá trị != 0 thì đẩy vào list để tính 1 thể.
                let objTiGia = {
                    sMoney: inputVND.toString(),
                    sMaThuTu: stt,
                    iLevel: stt.split('.').length - 1,
                    iGroup: parseInt(stt.substring(0, stt.indexOf('.'))),
                    iIndexRow: tr.index()
                };
                listTiGiaModel.push(objTiGia);
            }
        });
    } else {
        listInput.each(function() {
            let inputUSDElement = $(this);

            // Lấy thông tin dòng, stt, inputUSD, inputVND
            let tr = $(inputUSDElement).parents('tr');
            let stt = tr.find("td[name='sMaThuTu']").text();
            let elInputVND = tr.find('.inputFromVND');
            let inputUSD = parseFloat(UnFormatNumber($(inputUSDElement).val()));

            // Nếu inputVND ko phải số ( == '') hoặc bằng 0 thì gán giá trị = 0, ko phải tính.
            if (isNaN(inputUSD) || inputUSD === 0) {
                elInputVND.val(0);
                $(inputUSDElement).val(0);
            } else {
                // Nếu giá trị != 0 thì đẩy vào list để tính 1 thể.
                let objTiGia = {
                    sMoney: inputUSD.toString(),
                    sMaThuTu: stt,
                    iLevel: stt.split('.').length - 1,
                    iGroup: parseInt(stt.substring(0, stt.indexOf('.'))),
                    iIndexRow: tr.index()
                };
                listTiGiaModel.push(objTiGia);
            }
        });
    }

    // Tính tỉ giá
    $.ajax({
        type: "POST",
        data: { datas: listTiGiaModel, numTiGia: tiGia.toString() },
        url: '/QLNH/KeHoachChiTietBQP/CalcListMoneyByTiGia',
        async: false,
        success: function (data) {

            let listSTTReSum = [];
            // Gán lại giá trị và tính lại tổng
            data.result.forEach(x => {
                let money = FormatNumber(x.dResult);
                let row = $('#tbodyNhiemVuChi tr:eq(' + x.iIndexRow + ')');
                if (type == 'VND') {
                    let inputUSD = row.find('.inputFromVND');
                    inputUSD.val(money == '' ? 0 : money);
                } else {
                    let inputVND = row.find('.inputFromUSD');
                    inputVND.val(money == '' ? 0 : money);
                }

                // Tính lại stt
                SumValueParent(x.sMaThuTu);
            });
        }
    });
}
// #endregion ======================================================================