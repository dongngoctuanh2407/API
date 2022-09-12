var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var CONFIRM = 0;
var ERROR = 1;

function Save() {
    var result = [];
    $(".table-update tbody tr[data-rowsubmit='isData']").each(function () {
        var allValues = {};

        $(this).find("input").each(function (index) {
            const fieldName = $(this).attr("name");
            allValues[fieldName] = $(this).val();
        });
        $(this).find("td").each(function (index) {
            const fieldName = $(this).data("getname");
            if (fieldName !== undefined) {
                const fieldValue = $(this).data("getvalue");
                allValues[fieldName] = fieldValue;
            }
        });
        result.push(allValues);

    })
    $.ajax({
        type: "POST",
        url: "/QLNH/QuyetToanDuAnHoanThanh/SaveDetail",
        data: { data: result },
        async: false,
        success: function (r) {
            if (r && r.bIsComplete) {
                window.location.href = "/QLNH/QuyetToanDuAnHoanThanh";
            } else {
                var Title = 'Lỗi lưu thông tin quyết toán dự án';
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
function Cancel() {
    window.location.href = "/QLNH/QuyetToanDuAnHoanThanh";
}