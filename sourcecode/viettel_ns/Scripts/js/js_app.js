
function ConvertDatetimeToJSon(dateString) {
    if (dateString == undefined || dateString == null || dateString == '') return null;
    var currentTime = new Date(parseInt(dateString.substr(6)));
    var month = ("0" + (currentTime.getMonth() + 1)).slice(-2);
    var day = ("0" + currentTime.getDate()).slice(-2);
    var year = currentTime.getFullYear();
    var date = day + "/" + month + "/" + year;
    return date;
}

function NewGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

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


