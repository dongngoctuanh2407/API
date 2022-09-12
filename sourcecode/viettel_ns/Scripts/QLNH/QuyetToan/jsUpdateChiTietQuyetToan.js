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
    console.log(result)
    $.ajax({
        type: "POST",
        url: "/QLNH/QuyetToanNienDo/SaveDetail",
        data: { data: result },
        async: false,
        success: function (r) {
            if (r && r.bIsComplete) {
                window.location.href = "/QLNH/QuyetToanNienDo";
            } else {
                var Title = 'Lỗi lưu thông tin quyết toán niên độ';
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
function disabledAll(event) {
    var getClass = $(event).data("getclass");
    var getLevel = $(event).data("getlevel");
    var getId = $(event).data("getid");
    var getParent = $(event).data("getparent");
    //level 1: dự án
    //level 2: hợp đồng
    //level 3: thanh toán
    if (getLevel == "1") {
        //tìm con level 2 hợp đồng để disabled
        $(".table-update tbody tr[data-rowsubmit='isData']").find("input[data-getparent='" + getId + "'][data-getclass='" + getClass + "']").each(function () {
            const currentId = $(this).data("getid");
            $(this).prop("disabled", event.value.length);
            //tìm con level 3 thanh toán để disabled
            $(".table-update tbody tr[data-rowsubmit='isData']").find("input[data-getparent='" + currentId + "'][data-getclass='" + getClass + "']").each(function () {
                $(this).prop("disabled", event.value.length);
            })
        })
    } else if (getLevel == "2") {
        //kiểm tra tất cả level 2 còn value ko
        var check = $(".table-update tbody tr[data-rowsubmit='isData']").
            find("input[data-getparent='" + getParent + "'][data-getclass='" + getClass + "']").toArray()
            .some(x => x.value != "");
        //nếu còn thì disabled level 3 của level 2 đã nhập
        $(".table-update tbody tr[data-rowsubmit='isData']").find("input[data-getparent='" + getId + "'][data-getclass='" + getClass + "']").each(function () {
            $(this).prop("disabled", event.value.length);
        })
        var countError = 0;
        //kiểm tra tất cả level2 
        $(".table-update tbody tr[data-rowsubmit='isData']").find("input[data-getparent='" + getParent + "'][data-getclass='" + getClass + "']").each(function () {
            var currentId = $(this).data("getid");
            //check nếu level 3 còn value thì ẩn level 1 ko  hiển thị
            var check = $(".table-update tbody tr[data-rowsubmit='isData']").
                find("input[data-getparent='" + currentId + "'][data-getclass='" + getClass + "']").toArray()
                .some(x => x.value != "");
            if (check == true) {
                countError++;
            }
        })
        //disabled level 1 của level 2 đã nhập (kiểm tra còn level 3 ko hoặc còn level 2 ko)
        $(".table-update tbody tr[data-rowsubmit='isData']").find("input[data-getid='" + getParent + "'][data-getclass='" + getClass + "']").prop("disabled", countError != 0 ||check==true? true : false);
    } else {
        //kiểm tra level 3 của level 2 đấy còn value ko
        var check = $(".table-update tbody tr[data-rowsubmit='isData']").
            find("input[data-getparent='" + getParent + "'][data-getclass='" + getClass + "']").toArray()
            .some(x => x.value != "");
        let parrentId = "";
        let countError = 0;
        //nếu có thì disable level 2 đấy ko thì hiển thị
        $(".table-update tbody tr[data-rowsubmit='isData']").find("input[data-getid='" + getParent + "'][data-getclass='" + getClass + "']").each(function () {
            parrentId = $(this).data("getparent");
            $(this).prop("disabled", check);
            //get toàn bộ level 2 thuộc cây level 1 đấy
            $(".table-update tbody tr[data-rowsubmit='isData']").find("input[data-getparent='" + parrentId + "'][data-getclass='" + getClass + "']").each(function () {
                var currrentId = $(this).data("getid");
                //kiểm tra toàn bộ level 3 của cây đấy xem còn value ko nếu có thì ẩn level 1 ko thì hiển thị
                var checkParent = $(".table-update tbody tr[data-rowsubmit='isData']").find("input[data-getparent='" + currrentId + "'][data-getclass='" + getClass + "']").toArray()
                    .some(x => x.value != "");
                if (checkParent == true ) {
                    countError++
                }
            })
            //kiểm tra còn level 2 ko
            if ($(".table-update tbody tr[data-rowsubmit='isData']")
                .find("input[data-getparent='" + parrentId + "'][data-getclass='" + getClass + "']")
                .toArray()
                .some(x => x.value != "") == true) {
                countError++
            }
        })
        //ẩn hiện level 1 của cây đấy
        $(".table-update tbody tr[data-rowsubmit='isData']").find("input[data-getid='" + parrentId + "'][data-getclass='" + getClass + "']").prop("disabled", countError != 0 ? true : false);

    }
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
    window.location.href = "/QLNH/QuyetToanNienDo";
}