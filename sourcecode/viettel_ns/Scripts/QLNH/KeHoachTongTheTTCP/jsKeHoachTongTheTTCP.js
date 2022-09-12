var CONFIRM = 0;
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;
var CURRENT_STATE = '';
var USE_LAST_NEW_ADJUST = false;
var lstPhongBan = [];

// Document Ready
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

    $('#modalKHTTCP').on('hidden.bs.modal', function () {
        USE_LAST_NEW_ADJUST = false;
    });
});

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

// Làm mới danh sách
function ResetChangePage() {
    $("#txtFromDateFitler").val('');
    $("#txtToDateFitler").val('');
    $("#txtSoKeHoachFilter").val('');
    $("#txtNgayBanHanhFilter").val('');

    GetListData(1);
}

// Phân trang event
function ChangePage(iCurrentPage = 1) {
    GetListData(iCurrentPage);
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
        url: "/QLNH/KeHoachTongTheTTCP/ListPage",
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

// Open model create, edit, update Kế hoạch TTCP
function OpenModal(id, state) {
    $("#modalKHTTCPLabel").html('');

    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/QLNH/KeHoachTongTheTTCP/GetDetail",
        async: false,
        data: { id: id, state: state },
        success: function (data) {
            $("#contentModalKHTTCP").empty().html(data);
            if (state == 'CREATE') {
                $("#modalKHTTCPLabel").empty().html('Thêm mới kế hoạch tổng thể Thủ tướng Chính phủ');
            } else if (state == 'UPDATE') {
                $("#modalKHTTCPLabel").empty().html('Sửa kế hoạch tổng thể Thủ tướng Chính phủ');
            } else {
                $("#modalKHTTCPLabel").empty().html('Điều chỉnh kế hoạch tổng thể Thủ tướng Chính phủ');
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

// Chuyển sang modal chi tiết nhiệm vụ chi
function Save() {
    let state = CURRENT_STATE;

    // Get data
    let data = {};
    data.iLoai = $("#iLoai").val();
    data.iGiaiDoanTu = $("#txtGiaiDoanTu").val().trim();
    data.iGiaiDoanDen = $("#txtGiaiDoanDen").val().trim();
    data.iNamKeHoach = $("#txtNamKeHoach").val().trim();
    data.iID_ParentID = $("#iID_ParentID").val();
    data.sSoKeHoach = $("#txtSoKeHoach").val().trim();
    data.dNgayKeHoach = $("#txtNgayKeHoach").val();
    data.sMoTaChiTiet = $("#txtMoTaChiTiet").val().trim();
    data.bIsXoa = false;

    // Check state
    if (CURRENT_STATE == 'UPDATE') {
        // Update thì truyền thêm ID để lấy data phía BE và update.
        data.id = $("#hIdKeHoachChiTietBQP").val();
    } else if (CURRENT_STATE == 'CREATE') {
        // Thêm mới thì thêm các trường thông tin liên quan đến điều chỉnh
        data.bIsGoc = true;
        data.bIsActive = true;
        data.iLanDieuChinh = 0;
    } else {
        // Điều chỉnh thì gán lại old ID cho data để lấy thông tin nhiệm vụ chi lên view.
        data.bIsGoc = false;
        data.bIsActive = true;
        data.iLanDieuChinh = parseInt($("#hiLanDieuChinh").val()) + 1;
        data.iID_ParentAdjustID = $("#hIdKeHoachChiTietBQP").val();
        data.ID = $("#hIdKeHoachChiTietBQP").val();
    }

    OpenModalDetail(data, CURRENT_STATE, false);
}

// View to detail
function OpenModalDetail(data, state, isFilter = false) {
    // Nếu là xem chi tiết thì data sẽ là Id của kế hoạch TTCP. Nếu ko thì data là model kế hoạch tổng thể TTCP.
    if (state == 'DETAIL') {
        USE_LAST_NEW_ADJUST = false;
        let temp = data;
        data = {
            ID: temp,
            iID_ParentID: null
        };

        if (isFilter) {
            data.iID_BQuanLyID = $("#iID_BQuanLyID").val();
        }
    }

    // View list nhiệm vụ chi
    $.ajax({
        type: "POST",
        url: "/QLNH/KeHoachTongTheTTCP/NhiemVuChiDetail",
        data: { input: data, state: state, isUseLastTTCP: USE_LAST_NEW_ADJUST },
        async: false,
        success: function (result) {
            $("#modalKHTTCP").modal('hide');
            // View result
            $("#lstDataView").html(result);
            if (isFilter) {
                $("#iID_BQuanLyID").val(data.iID_BQuanLyID);
            }
        }
    });

    CURRENT_STATE = state;

    // Lấy danh sách phòng ban để tạo lookup khi thêm dòng nhiệm vụ chi
    if (CURRENT_STATE != 'DETAIL') {
        $.ajax({
            type: "POST",
            url: "/QLNH/KeHoachTongTheTTCP/GetDataLookupDetail",
            async: false,
            success: function (result) {
                lstPhongBan = result.ListPhongBan;
            }
        });
    }
}

// Chuyển về màn danh sách kế hoạch TTCP
function ViewToList() {
    var filter = {
        giaiDoanTu: null,
        giaiDoanDen: null,
        soKeHoach: null,
        ngayBanHanh: null
    }

    $.ajax({
        type: "POST",
        url: "/QLNH/KeHoachTongTheTTCP/ListPage",
        data: { input: filter, paging: null },
        success: function (data) {
            // View result
            $("#lstDataView").html(data);
        }
    });
}

// Thêm dòng nhiệm vụ chi
function AddRowKeHoachChiTiet(e) {
    var sttNew = "1";
    var parentRowIndex = 1;
    if (e) {
        let parentRow = $(e).parents('tr');
        let stt = parentRow.find('td[name="sMaThuTu"]').text();

        // Disable input + button delete của dòng cha.
        parentRow.find('#fGiaTri').prop('disabled', true);
        parentRow.find('button.btn-delete').addClass('disabled');

        // Lấy số thứ tự mới và index dòng mới thêm vào.
        var { sttNew, parentRowIndex } = RefreshThuTuAndGetThuTuMoi(stt, parentRow.index());
    } else {
        let regexParent = new RegExp('^[0-9]+$');
        let arrStt = [];
        let hasValue = false;
        $("#tbodyNhiemVuChi tr td[name='sMaThuTu']").each(function (e) {
            let value = $(this).text();

            if (regexParent.test(value)) {
                arrStt.push({
                    value: value,
                    element: $(this)
                });
                hasValue = true;
            }
        });

        if (hasValue) {
            let maxElement = arrStt.sort((a, b) => parseInt(a.value) - parseInt(b.value))[arrStt.length - 1];
            sttNew = (parseInt(maxElement.value) + 1).toString();
        }
    }

    let rowHtml = `
        <tr>
            <td name="ID_NhiemVuChi" class="hidden"></td>
            <td align="left" name="sMaThuTu">` + sttNew + `</td>
            <td><input type="text" id="sTenNhiemVuChi" class="form-control" value="" maxlength="255" autocomplete="off"></td>
            <td align="left">
                ` + CreateLookupBQuanLy() + `
            </td>
            <td align="right">
                <input type="text" id="fGiaTri" class="form-control" value="" maxlength="255" autocomplete="off" onblur="SumValueParent(this)"
                    onkeyup="ValidateNumberKeyUp(this);" onkeypress="return ValidateNumberKeyPress(this, event);">
            </td>
            <td class="hidden" name="iID_ParentID"></td>
            <td align="center">
                <button type="button" class="btn-detail" onclick="AddRowKeHoachChiTiet(this)"><i class="fa fa-plus" aria-hidden="true"></i></button>
                <button type="button" class="btn-delete" onclick="RemoveRowKeHoachChiTiet(this)"><i class="fa fa-trash-o fa-lg" aria-hidden="true"></i></button>
            </td>
        </tr>`;

    // Add row
    if (e) {
        $("#tbodyNhiemVuChi tr").eq(parentRowIndex).after(rowHtml);
    } else {
        $("#tbodyNhiemVuChi").append(rowHtml);
    }
    
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
                rowParent.find('#fGiaTri').prop('disabled', false);
                rowParent.find('button.btn-delete').removeClass('disabled');
            }
        }
    } else {
        // Nếu nó là dòng cha lớn nhất (level 0) thì check dòng cùng cấp và update lại toàn bộ stt.
        let numOfRow = $('#tbodyNhiemVuChi tr').length;
        if (numOfRow != 0) {
            // Lấy các dòng level 0
            let regexCurrent = new RegExp('^[0-9]+$');
            let arrStt = [];
            let hasValue = false;
            $("#tbodyNhiemVuChi tr td[name='sMaThuTu']").each(function (e) {
                let value = $(this).text();

                if (regexCurrent.test(value)) {
                    arrStt.push({
                        value: value,
                        element: $(this)
                    });
                    hasValue = true;
                }
            });

            // Nêu có thì check valid thứ tự
            if (hasValue) {
                let maxElement = arrStt.sort((a, b) => parseInt(a.value) - parseInt(b.value))[arrStt.length - 1];
                let sttMax = parseInt(maxElement.value);

                // Nếu stt lớn nhất != độ dài mảng của những thằng cùng cấp (tương đương với việc xóa dòng ở trước dòng cuối cùng) thì gán lại stt.
                if (sttMax != arrStt.length) {
                    let newIndex = 1;
                    arrStt.forEach(x => {
                        let oldStt = x.element.text();
                        let newStt = newIndex;

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
                }
            }
        }
    }

    // Tính lại tổng tiền từ dòng hiện tại đổ lên.
    SumValueParent(null, stt);
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
function CreateLookupBQuanLy() {

    var html = '<select class="form-control" id="iID_BQuanLyID">';

    lstPhongBan.forEach(x => {
        html += '<option value="' + x.Id + '">' + x.DisplayName + '</option>';
    });

    html += '</select>';

    return html;
}

// Tính tổng giá trị cho dòng cha
function SumValueParent(element, stt) {
    var sttCurrentRow = '1';
    if (element) {
        sttCurrentRow = $(element).parents('tr').find("td[name='sMaThuTu']").text();
    } else {
        sttCurrentRow = stt;
    }
    
    // Lấy stt dòng hiện tại
    let indexOfDot = sttCurrentRow.lastIndexOf('.');
    if (indexOfDot != -1) {
        // Nêu có thể cắt chuỗi stt thì cắt để tìm stt của parent
        let sttParent = sttCurrentRow.substring(0, indexOfDot);
        let rowParent = undefined;

        // Tạo regex tìm dòng con
        const regexChild = new RegExp('^' + sttParent + '.[0-9]+$');
        let lstUSD = [];
        let lstVND = [];

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
                let valueUSD = UnFormatNumber($(this).find("#fGiaTri").val());

                lstUSD.push(valueUSD.toString());
            }
        });

        // Nếu có rowParent thì gán value
        if (rowParent) {
            $.ajax({
                type: "POST",
                data: { lstNumVND: lstVND, lstNumUSD: lstUSD },
                url: '/QLNH/KeHoachChiTietBQP/SumTwoList',
                async: false,
                success: function (data) {
                    rowParent.find("#fGiaTri").val(FormatNumber(data.resultUSD) == '' ? 0 : FormatNumber(data.resultUSD));
                    SumValueParent(rowParent.find("#fGiaTri"));
                }
            });
        }
    }
}

// Lưu chi tiết nhiệm vụ chi
function SaveDetail() {
    let state = $("#currentState").val();
    let keHoachTongTheTTCP = $("#keHoachTongTheTTCP").val();

    var tableNhiemVuChi = [];
    $("#tbodyNhiemVuChi tr").each(function (e) {
        let rowElement = $(this);

        let rowData = {};
        rowData.ID = rowElement.find('td[name="ID_NhiemVuChi"]').text().trim();
        rowData.sMaThuTu = rowElement.find('td[name="sMaThuTu"]').text().trim();
        rowData.sTenNhiemVuChi = rowElement.find('#sTenNhiemVuChi').val().trim();
        rowData.iID_BQuanLyID = rowElement.find('#iID_BQuanLyID').val();
        rowData.sGiaTri = UnFormatNumber(rowElement.find('#fGiaTri').val()).toString().trim();
        rowData.iID_ParentID = rowElement.find('td[name="iID_ParentID"]').text();
        tableNhiemVuChi.push(rowData);
    });

    $.ajax({
        type: "POST",
        url: "/QLNH/KeHoachTongTheTTCP/SaveKHTongTheTTCP",
        data: { lstNhiemVuChis: tableNhiemVuChi, keHoachTongTheTTCP: keHoachTongTheTTCP, state: state },
        success: function (data) {
            if (data) {
                ViewToList();
            } else {
                var Title = 'Lỗi thêm mới kế hoạch tổng hợp Thủ tướng Chính phủ phê duyệt!';
                var Messages = ['Thêm mới kế hoạch tổng hợp Thủ tướng Chính phủ phê duyệt không thành công!'];
                if (state == 'UPDATE') {
                    Title = 'Lỗi cập nhật kế hoạch tổng hợp Thủ tướng Chính phủ phê duyệt!';
                    Messages = ['Cập nhật kế hoạch tổng hợp Thủ tướng Chính phủ phê duyệt không thành công!'];
                } else if (state == 'ADJUST') {
                    Title = 'Lỗi điều chỉnh kế hoạch tổng hợp Thủ tướng Chính phủ phê duyệt!';
                    Messages = ['Điều chỉnh kế hoạch tổng hợp Thủ tướng Chính phủ phê duyệt không thành công!'];
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

// Confirm xóa
function ConfirmDelete(id, sNam, sSoKeHoach) {
    var Title = 'Xác nhận xóa kế hoạch tổng thể TTCP phê duyệt';
    var Messages = [];
    Messages.push('Bạn có chắc chắn muốn xóa kế hoạch tổng thể: ' + sSoKeHoach + ' - ' + sNam + '?');
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
        url: "/QLNH/KeHoachTongTheTTCP/KH_TTCPDelete",
        data: { id: id },
        success: function (r) {
            if (r.bIsComplete) {
                location.reload();
            } else {
                var Title = 'Lỗi xóa kế hoạch tổng thể TTCP phê duyệt';
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

// Tìm ra thằng cha active và gán lại vào select.
function UpdateParentTTCP(iID_Parent_TTCP) {
    $.ajax({
        type: "POST",
        url: "/QLNH/KeHoachTongTheTTCP/FindParentTTCPActive",
        async: false,
        data: { id: iID_Parent_TTCP },
        success: function (data) {
            $("#iID_ParentID").val(data.result.ID).change();
            USE_LAST_NEW_ADJUST = true;
        }
    });
}

//=================================== NOT USED =========================================//
//function OpenDetail_NVC(id, khtt_id) {
//    $.ajax({
//        type: "POST",
//        dataType: "html",
//        url: "/QLNH/KeHoachTongTheTTCP/Detail_NVC",
//        data: { NVC_ID: id, khtt_id: khtt_id },
//        success: function (data) {
//            $("#contentModalLoaiHopDong").html(data);
//        }
//    });
//}

//function OpenAddNew_NVC(khtt_id) {
//    $.ajax({
//        type: "POST",
//        dataType: "html",
//        data: { khtt_id: khtt_id },
//        url: "/QLNH/KeHoachTongTheTTCP/AddNew_NVC",
//        success: function (data) {
//            $("#contentModalLoaiHopDong").html(data);
//        }
//    });
//}

//function OpenDetail_NVC_AddChild(id, khtt_id) {
//    $.ajax({
//        type: "POST",
//        dataType: "html",
//        url: "/QLNH/KeHoachTongTheTTCP/Detail_NVC_AddChild",
//        data: { NVC_ID: id, khtt_id: khtt_id },
//        success: function (data) {
//            $("#contentModalLoaiHopDong").html(data);
//        }
//    });
//}

//function DeleteLoaiHopDong(id) {
//    $.ajax({
//        type: "POST",
//        url: "/QLNH/KeHoachTongTheTTCP/KH_TTCPDelete",
//        data: { id: id },
//        success: function (r) {
//            if (r.bIsComplete) {
//                ChangePage();
//            } else {
//                var Title = 'Lỗi xóa tỷ giá';
//                var messErr = [];
//                messErr.push(r.sMessError);
//                $.ajax({
//                    type: "POST",
//                    url: "/Modal/OpenModal",
//                    data: { Title: Title, Messages: messErr, Category: ERROR },
//                    success: function (data) {
//                        $("#divModalConfirm").html(data);
//                    }
//                });
//            }
//        }
//    });
//}

//function Save_AddNew_NVC(khtt_id) {
//    var data = {};
//    data.ID = $("#iID_KH_TTCP_NVC_Modal").val();
//    data.sMaThuTu = $("#txtsMaThuTu").val();
//    data.sTenNhiemVuChi = $("#txtsTenNhiemVuChi").val();
//    data.iID_BQuanLyID = $("#ddliID_BQuanLyID").val();
//    data.fGiaTri = $("#txtfGiaTri").val();
//    data.iID_KHTongTheID = $("#txtfGiaTri").val();
//    data.iID_ParentID = parent_id;
//    tongGiaTri = $("#txtfTongGiaTri").val();

//    $.ajax({
//        type: "POST",
//        url: "/QLNH/KeHoachTongTheTTCP/KH_TTCP_NVC_Save",
//        data: { data: data, tongGiaTri: tongGiaTri },
//        success: function (r) {

//            if (r.bIsComplete) {
//                location.reload();
//            }
//            else {
//                var Title = 'Lỗi lưu kế hoạch';
//                var messErr = [];
//                messErr.push(r.sMessError);
//                $.ajax({
//                    type: "POST",
//                    url: "/Modal/OpenModal",
//                    data: { Title: Title, Messages: messErr, Category: ERROR },
//                    success: function (data) {
//                        $("#divModalConfirm").html(data);
//                    }
//                });
//            }
//        }

//    });
//}

//function Save_NVC(khtt_id, parent_id) {
//    var data = {};
//    data.ID = $("#iID_KH_TTCP_NVC_Modal").val();
//    data.sMaThuTu = $("#txtsMaThuTu").val();
//    data.sTenNhiemVuChi = $("#txtsTenNhiemVuChi").val();
//    data.iID_BQuanLyID = $("#ddliID_BQuanLyID").val();
//    data.fGiaTri = $("#txtfGiaTri").val();
//    data.iID_KHTongTheID = khtt_id;
//    data.iID_ParentID = parent_id;
//    tongGiaTri = $("#txtfTongGiaTri").val();

//    $.ajax({
//        type: "POST",
//        url: "/QLNH/KeHoachTongTheTTCP/KH_TTCP_NVC_Save",
//        data: { data: data, tongGiaTri: tongGiaTri },
//        success: function (r) {

//            if (r.bIsComplete) {
//                location.reload();
//            }
//            else {
//                var Title = 'Lỗi lưu kế hoạch';
//                var messErr = [];
//                messErr.push(r.sMessError);
//                $.ajax({
//                    type: "POST",
//                    url: "/Modal/OpenModal",
//                    data: { Title: Title, Messages: messErr, Category: ERROR },
//                    success: function (data) {
//                        $("#divModalConfirm").html(data);
//                    }
//                });
//            }
//        }

//    });
//}

//function Save_NVC_AddChild(khtt_id, parent_id) {
//    var data = {};
//    data.ID = null;
//    data.sMaThuTu = $("#txtsMaThuTu").val();
//    data.sTenNhiemVuChi = $("#txtsTenNhiemVuChi").val();
//    data.iID_BQuanLyID = $("#ddliID_BQuanLyID").val();
//    data.fGiaTri = $("#txtfGiaTri").val();
//    data.iID_KHTongTheID = khtt_id;
//    data.iID_ParentID = parent_id;
//    tongGiaTri = $("#txtfTongGiaTri").val();

//    $.ajax({
//        type: "POST",
//        url: "/QLNH/KeHoachTongTheTTCP/KH_TTCP_NVC_AddChild_Save",
//        data: { data: data, tongGiaTri: tongGiaTri },
//        success: function (r) {

//            if (r.bIsComplete) {
//                location.reload();
//            }
//            else {
//                var Title = 'Lỗi lưu nhiệm vụ chi con';
//                var messErr = [];
//                messErr.push(r.sMessError);
//                $.ajax({
//                    type: "POST",
//                    url: "/Modal/OpenModal",
//                    data: { Title: Title, Messages: messErr, Category: ERROR },
//                    success: function (data) {
//                        $("#divModalConfirm").html(data);
//                    }
//                });
//            }
//        }

//    });
//}

//function AddRowToTable() {
//    var newrows = "";
//    newrows += "<tr style='cursor: pointer;' class='parent'>";
//    newrows += "<td class='ChiPhi' align='left'><input name='thutu' type='text' class='form-control txtThuTu dataTableChiPhi' /></td>";
//    newrows += "<td class='ChiPhi' align='left'><input name='nhiemvu' type='text' class='form-control txtNhiemVu dataTableChiPhi'></td>";
//    newrows += "<td class='ChiPhi' align='left'><input name='quanly' type='text' class='form-control txtQuanLy dataTableChiPhi'/></td>";
//    newrows += "<td class='ChiPhi' align='left'><input name='giatri' type='text' class='form-control txtGiaTri dataTableChiPhi'/></td>";
//    newrows += "<td align='center'><button class='btn-delete btn-icon' type='button' onclick='DeleteRow(this)'><i class='fa fa-trash' aria-hidden='true' ></button></td>";
//    newrows += "</tr>";
//    $("#tbListNhiemVuChi tbody").append(newrows);

//    //UpdateSequenceNumber();
//}

//function DeleteRow(deleteEvent) {
//    var rowToDelelete = deleteEvent.parentElement.parentElement;
//    rowToDelelete.parentNode.removeChild(rowToDelelete);
//}

//function UpdateSequenceNumber() {
//    $("#tbListNhiemVuChi tbody tr").each(function (index, tr) {
//        $(tr).find('.sequence').text(index + 1);
//    });
//}
//============================================================================//