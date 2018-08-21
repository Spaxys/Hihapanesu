function HihapanesuTranslate(input) {
    var dataObject = {
        Text: input,
        IsTest: false
    };

    var dataJson = JSON.stringify(dataObject);
    var resultData;

    var url = "../api/Translate/TranscribeAndGenerate";
    var jqXHR = $.ajax({
        type: "POST",
        url: url,
        data: dataJson,
        async: false,
        contentType: "application/json; charset=utf-8",
        //dataType: "json",
        success: function (data) {
        },
        failure: function (errmsg) {
            alert(errMsg);
        }
    });
    var responseText = jqXHR.responseText;
    var responseToString = JSON.parse(responseText);
    //console.log(responseText);

    return responseToString;
}

function SetHihapanesuValue(elementId, html) {
    var resultElem = document.getElementById(elementId);
    resultElem.innerHTML = html;
}

function htmlEscape(str) {
    return str
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}

// I needed the opposite function today, so adding here too:
function htmlUnescape(str) {
    return str
        .replace(/&quot;/g, '"')
        .replace(/&#39;/g, "'")
        .replace(/&lt;/g, '<')
        .replace(/&gt;/g, '>')
        .replace(/&amp;/g, '&');
}

function PrintElemWithTitle(elem) {
    var mywindow = window.open('', 'PRINT', 'height=400,width=600');

    mywindow.document.write('<html><head><title>' + document.title + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write('<h1>' + document.title + '</h1>');
    mywindow.document.write(document.getElementById(elem).innerHTML);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    mywindow.print();
    mywindow.close();

    return true;
}

function PrintElem(elem) {
    var mywindow = window.open('', 'PRINT', 'height=400,width=600');

    mywindow.document.write('<html><head><title></title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write(document.getElementById(elem).innerHTML);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    mywindow.print();
    mywindow.close();

    return true;
}
